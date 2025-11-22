using System.Threading;
using System.Threading.Tasks;
using TennisScoring.Console.Abstractions;

namespace TennisScoring.Console;

internal sealed class ConsoleUI
{
    private const string FirstPlayerPrompt = "請輸入第一位球員姓名：";
    private const string SecondPlayerPrompt = "請輸入第二位球員姓名：";
    private const string EmptyNameMessage = "姓名不可為空，請重新輸入。";

    private readonly IConsoleAdapter _console;

    public ConsoleUI(IConsoleAdapter console)
    {
        _console = console;
    }

    public Task<string?> PromptFirstPlayerNameAsync(CancellationToken cancellationToken = default)
    {
        return PromptPlayerNameInternalAsync(FirstPlayerPrompt, cancellationToken);
    }

    public Task<string?> PromptSecondPlayerNameAsync(CancellationToken cancellationToken = default)
    {
        return PromptPlayerNameInternalAsync(SecondPlayerPrompt, cancellationToken);
    }

    public Task ShowEmptyNameWarningAsync(CancellationToken cancellationToken = default)
    {
        return _console.WriteLineAsync(EmptyNameMessage, cancellationToken);
    }

    public Task ShowInitialScoreAsync(string scoreText, CancellationToken cancellationToken = default)
    {
        return _console.WriteLineAsync($"比分：{scoreText}", cancellationToken);
    }

    private async Task<string?> PromptPlayerNameInternalAsync(string prompt, CancellationToken cancellationToken)
    {
        await _console.WriteLineAsync(prompt, cancellationToken).ConfigureAwait(false);
        return await _console.ReadLineAsync(cancellationToken).ConfigureAwait(false);
    }
}
