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
