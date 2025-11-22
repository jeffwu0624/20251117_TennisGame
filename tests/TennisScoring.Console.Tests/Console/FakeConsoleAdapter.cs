using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TennisScoring.Console.Abstractions;

namespace TennisScoring.Console.Tests.Console;

public sealed class FakeConsoleAdapter : IConsoleAdapter
{
    private readonly Queue<string?> _inputs;

    public FakeConsoleAdapter(IEnumerable<string?>? inputs = null)
    {
        _inputs = inputs is null ? new Queue<string?>() : new Queue<string?>(inputs);
    }

    public IList<string> WrittenLines { get; } = new List<string>();

    public IList<string> WrittenFragments { get; } = new List<string>();

    public Task<string?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_inputs.Count > 0 ? _inputs.Dequeue() : null);
    }

    public Task WriteLineAsync(string message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        WrittenLines.Add(message);
        return Task.CompletedTask;
    }

    public Task WriteAsync(string message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        WrittenFragments.Add(message);
        return Task.CompletedTask;
    }

    public void EnqueueInput(string? value)
    {
        _inputs.Enqueue(value);
    }
}
