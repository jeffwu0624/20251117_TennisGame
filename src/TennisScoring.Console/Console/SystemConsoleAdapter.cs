using System.Threading;
using System.Threading.Tasks;
using TennisScoring.Console.Abstractions;

namespace TennisScoring.Console.Console;

internal sealed class SystemConsoleAdapter : IConsoleAdapter
{
    public async Task<string?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await System.Console.In.ReadLineAsync().ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    public async Task WriteLineAsync(string message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await System.Console.Out.WriteLineAsync(message).ConfigureAwait(false);
    }

    public async Task WriteAsync(string message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await System.Console.Out.WriteAsync(message).ConfigureAwait(false);
    }
}
