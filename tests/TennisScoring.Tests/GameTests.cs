using FluentAssertions;

namespace TennisScoring.Tests;

public class GameTests
{
    // 基本計分測試 (US1)
    [Fact]
    public void NewGame_ShouldReturn_LoveAll()
    {
        var game = new Game();
        game.GetScoreText().Should().Be("Love-All");
    }

    [Fact]
    public void ScoreOneZero_ShouldReturn_FifteenLove()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("Fifteen-Love");
    }

    [Fact]
    public void ScoreTwoZero_ShouldReturn_ThirtyLove()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("Thirty-Love");
    }

    [Fact]
    public void ScoreThreeZero_ShouldReturn_FortyLove()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("Forty-Love");
    }

    [Fact]
    public void ScoreZeroOne_ShouldReturn_LoveFifteen()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Love-Fifteen");
    }

    [Fact]
    public void ScoreZeroTwo_ShouldReturn_LoveThirty()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Love-Thirty");
    }

    [Fact]
    public void ScoreZeroThree_ShouldReturn_LoveForty()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Love-Forty");
    }

    [Fact]
    public void ScoreOneOne_ShouldReturn_FifteenAll()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Fifteen-All");
    }

    [Fact]
    public void ScoreTwoTwo_ShouldReturn_ThirtyAll()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Thirty-All");
    }
}
