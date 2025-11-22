using System.Threading;
using System.Threading.Tasks;
using TennisScoring.Console.Abstractions;

namespace TennisScoring.Console;

internal sealed class ConsoleUI
{
    private readonly IConsoleAdapter _console;

    public ConsoleUI(IConsoleAdapter console)
    {
        _console = console;
    }

    public Task WriteLineAsync(string message, CancellationToken cancellationToken = default)
    {
        return _console.WriteLineAsync(message, cancellationToken);
    }

    public Task WriteAsync(string message, CancellationToken cancellationToken = default)
    {
        return _console.WriteAsync(message, cancellationToken);
    }

    public Task<string?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        return _console.ReadLineAsync(cancellationToken);
    }
}
