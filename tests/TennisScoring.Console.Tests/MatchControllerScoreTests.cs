using System.Threading.Tasks;
using TennisScoring.Console;
using TennisScoring.Console.Tests.Console;
using Xunit;

namespace TennisScoring.Console.Tests;

public class MatchControllerScoreTests
{
    [Fact(Skip = "T018: pending implementation")]
    public async Task Scoring_BasicPoints_UpdatesScore()
    {
        // Arrange: 待實作 - 將透過 FakeConsoleAdapter 提供姓名與得分輸入

        // Act: 呼叫 MatchController.RunAsync()

        // Assert: 檢查 WrittenLines 包含預期的比分顯示（例如 "比分：Thirty-Love"）
    }

    [Fact(Skip = "T018: pending implementation")]
    public async Task Scoring_DeuceAndAdvantage_BehavesCorrectly()
    {
        // Arrange: 待實作 - 模擬達到 Deuce 與 Advantage 的得分序列

        // Act: 呼叫 MatchController.RunAsync()

        // Assert: 檢查顯示 "Deuce"、"Advantage" 與勝利訊息
    }
}
