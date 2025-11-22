using System.Threading;
using System.Threading.Tasks;

namespace TennisScoring.Console;

internal sealed class MatchController
{
    private readonly ConsoleUI _ui;
    private readonly InputValidator _validator;

    public MatchController(ConsoleUI ui, InputValidator validator)
    {
        _ui = ui;
        _validator = validator;
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
