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

    // Deuce 狀態測試 (US2)
    [Fact]
    public void ScoreThreeThree_ShouldReturn_Deuce()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("Deuce");
    }

    [Fact]
    public void ScoreFourFour_ShouldReturn_Deuce()
    {
        var game = new Game();
        for (int i = 0; i < 4; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.GetScoreText().Should().Be("Deuce");
    }

    [Fact]
    public void ScoreFiveFive_ShouldReturn_Deuce()
    {
        var game = new Game();
        for (int i = 0; i < 5; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.GetScoreText().Should().Be("Deuce");
    }

    // Advantage 狀態測試 (US3)
    [Fact]
    public void ScoreFourThree_ShouldReturn_PlayerAAdv()
    {
        var game = new Game();
        for (int i = 0; i < 3; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Adv");
    }

    [Fact]
    public void ScoreThreeFour_ShouldReturn_PlayerBAdv()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("PlayerB Adv");
    }

    [Fact]
    public void ScoreFiveFour_ShouldReturn_PlayerAAdv()
    {
        var game = new Game();
        for (int i = 0; i < 4; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Adv");
    }

    [Fact]
    public void ScoreFourFive_ShouldReturn_PlayerBAdv()
    {
        var game = new Game();
        for (int i = 0; i < 4; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("PlayerB Adv");
    }

    // 獲勝判定測試 (US4)
    [Fact]
    public void ScoreFourZero_ShouldReturn_PlayerAWin()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Win");
        game.Winner.Should().Be(Side.PlayerA);
        game.IsFinished.Should().BeTrue();
    }

    [Fact]
    public void ScoreFourOne_ShouldReturn_PlayerAWin()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Win");
        game.Winner.Should().Be(Side.PlayerA);
        game.IsFinished.Should().BeTrue();
    }

    [Fact]
    public void ScoreFourTwo_ShouldReturn_PlayerAWin()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Win");
        game.Winner.Should().Be(Side.PlayerA);
        game.IsFinished.Should().BeTrue();
    }

    [Fact]
    public void ScoreFiveThree_ShouldReturn_PlayerAWin()
    {
        var game = new Game();
        for (int i = 0; i < 3; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Win");
        game.Winner.Should().Be(Side.PlayerA);
        game.IsFinished.Should().BeTrue();
    }

    [Fact]
    public void ScoreThreeFive_ShouldReturn_PlayerBWin()
    {
        var game = new Game();
        for (int i = 0; i < 3; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerB);
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("PlayerB Win");
        game.Winner.Should().Be(Side.PlayerB);
        game.IsFinished.Should().BeTrue();
    }

    [Fact]
    public void PointWonByAfterGameFinished_ShouldThrow_InvalidOperationException()
    {
        var game = new Game();
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);

        var act = () => game.PointWonBy(Side.PlayerA);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Game has already finished.");
    }

    // 邊界測試 (Boundary Tests)
    [Fact]
    public void ScoreTenTen_ShouldReturn_Deuce()
    {
        var game = new Game();
        for (int i = 0; i < 10; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.GetScoreText().Should().Be("Deuce");
        game.IsFinished.Should().BeFalse();
    }

    [Fact]
    public void ScoreElevenTen_ShouldReturn_PlayerAAdv()
    {
        var game = new Game();
        for (int i = 0; i < 10; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Adv");
        game.IsFinished.Should().BeFalse();
    }

    [Fact]
    public void ScoreTwelveThirteen_ShouldReturn_PlayerBAdv()
    {
        var game = new Game();
        for (int i = 0; i < 12; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerB);
        game.GetScoreText().Should().Be("PlayerB Adv");
        game.IsFinished.Should().BeFalse();
    }

    [Fact]
    public void ScoreFifteenThirteen_ShouldReturn_PlayerAWin()
    {
        var game = new Game();
        for (int i = 0; i < 13; i++)
        {
            game.PointWonBy(Side.PlayerA);
            game.PointWonBy(Side.PlayerB);
        }
        game.PointWonBy(Side.PlayerA);
        game.PointWonBy(Side.PlayerA);
        game.GetScoreText().Should().Be("PlayerA Win");
        game.Winner.Should().Be(Side.PlayerA);
        game.IsFinished.Should().BeTrue();
    }
}
