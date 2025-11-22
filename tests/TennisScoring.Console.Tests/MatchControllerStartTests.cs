using System.Threading.Tasks;
using TennisScoring.Console;
using TennisScoring.Console.Tests.Console;
using Xunit;

namespace TennisScoring.Console.Tests;

public class MatchControllerStartTests
{
    [Fact]
    public async Task RunAsync_WithValidNames_ShouldDisplayInitialScore()
    {
        var adapter = new FakeConsoleAdapter(new[] { "Alice", "Bob" });
        var ui = new ConsoleUI(adapter);
        var validator = new InputValidator();
        var controller = new MatchController(ui, validator);

        await controller.RunAsync();

        Assert.Collection(
            adapter.WrittenLines,
            first => Assert.Equal("請輸入第一位球員姓名：", first),
            second => Assert.Equal("請輸入第二位球員姓名：", second),
            third => Assert.Equal("比分：Love-All", third));
    }

    [Fact]
    public async Task RunAsync_WhenNameEmpty_ShouldPromptAgain()
    {
        var adapter = new FakeConsoleAdapter(new[] { "", "Alice", "Bob" });
        var ui = new ConsoleUI(adapter);
        var validator = new InputValidator();
        var controller = new MatchController(ui, validator);

        await controller.RunAsync();

        Assert.True(adapter.WrittenLines.Count >= 5, "至少應有五個輸出含重新提示與比分");
        Assert.Equal("請輸入第一位球員姓名：", adapter.WrittenLines[0]);
        Assert.Equal("姓名不可為空，請重新輸入。", adapter.WrittenLines[1]);
        Assert.Equal("請輸入第一位球員姓名：", adapter.WrittenLines[2]);
    }
}
