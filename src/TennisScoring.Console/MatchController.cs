using System.Threading;
using System.Threading.Tasks;
using TennisScoring;

namespace TennisScoring.Console;

internal sealed class MatchController
{
    private readonly ConsoleUI _ui;
    private readonly InputValidator _validator;

    private string? _playerOneName;
    private string? _playerTwoName;
    private Game? _game;

    public MatchController(ConsoleUI ui, InputValidator validator)
    {
        _ui = ui;
        _validator = validator;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (!await InitializePlayersAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        _game = new Game();
        var scoreText = _game.GetScoreText();
        await _ui.ShowInitialScoreAsync(scoreText, cancellationToken).ConfigureAwait(false);
        
        // Begin scoring loop
        while (true)
        {
            if (_game.IsFinished)
                break;

            var selection = await _ui.PromptScoreSelectionAsync(cancellationToken).ConfigureAwait(false);

            if (selection is null)
            {
                // No input available (test adapter exhausted) -> exit
                return;
            }

            if (_validator.IsExitCommand(selection))
            {
                return;
            }

            if (!_validator.TryParseScoringSelection(selection, out var playerIndex))
            {
                await _ui.ShowInvalidSelectionAsync(cancellationToken).ConfigureAwait(false);
                continue;
            }

            var side = playerIndex == 0 ? Side.PlayerA : Side.PlayerB;
            _game.PointWonBy(side);

            var current = _game.GetScoreText();
            await _ui.ShowCurrentScoreAsync(current, cancellationToken).ConfigureAwait(false);

            if (_game.IsFinished)
            {
                var winnerName = _game.Winner == Side.PlayerA ? _playerOneName : _playerTwoName;
                await _ui.ShowWinnerAsync(winnerName ?? "", cancellationToken).ConfigureAwait(false);
                break;
            }
        }
    }

    private async Task<bool> InitializePlayersAsync(CancellationToken cancellationToken)
    {
        _playerOneName = await CapturePlayerNameAsync(() => _ui.PromptFirstPlayerNameAsync(cancellationToken), cancellationToken).ConfigureAwait(false);
        if (_playerOneName is null)
        {
            return false;
        }

        _playerTwoName = await CapturePlayerNameAsync(() => _ui.PromptSecondPlayerNameAsync(cancellationToken), cancellationToken).ConfigureAwait(false);
        if (_playerTwoName is null)
        {
            return false;
        }

        return true;
    }

    private async Task<string?> CapturePlayerNameAsync(Func<Task<string?>> prompt, CancellationToken cancellationToken)
    {
        while (true)
        {
            var input = await prompt().ConfigureAwait(false);
            if (input is null)
            {
                // Treat closed input stream as exit
                return null;
            }

            if (_validator.IsExitCommand(input))
            {
                return null;
            }

            if (_validator.TryNormalizePlayerName(input, out var normalized))
            {
                return normalized;
            }

            await _ui.ShowEmptyNameWarningAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
