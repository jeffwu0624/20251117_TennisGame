using System.Threading;
using System.Threading.Tasks;

namespace TennisScoring.Console.Abstractions;

public interface IConsoleAdapter
{
    Task<string?> ReadLineAsync(CancellationToken cancellationToken = default);

    Task WriteLineAsync(string message, CancellationToken cancellationToken = default);

    Task WriteAsync(string message, CancellationToken cancellationToken = default);
}
