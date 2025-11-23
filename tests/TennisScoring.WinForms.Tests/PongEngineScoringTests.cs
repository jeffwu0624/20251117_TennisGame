using System.Drawing;
using TennisScoring.WinForms.Engine;
using TennisScoring.WinForms.Entities;
using Xunit;

namespace TennisScoring.WinForms.Tests;

public class PongEngineScoringTests
{
    [Fact]
    public void BallOutLeft_ShouldScoreForPlayerB()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball at left edge, moving left
        engine.Ball.Reset(new PointF(-5, 300), new PointF(-100, 0));
        
        string? scoreText = null;
        engine.ScoreChanged += (s, e) => scoreText = e.ScoreText;
        
        // Act
        engine.Update(0.1f); // Move out
        
        // Assert
        // Initial score is Love-All. Player B scores -> Love-Fifteen.
        Assert.Equal("Love-Fifteen", scoreText);
        Assert.Equal("Love-Fifteen", engine.GetState().ScoreText);
    }

    [Fact]
    public void BallOutRight_ShouldScoreForPlayerA()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball at right edge, moving right
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        
        string? scoreText = null;
        engine.ScoreChanged += (s, e) => scoreText = e.ScoreText;
        
        // Act
        engine.Update(0.1f); // Move out
        
        // Assert
        // Initial score is Love-All. Player A scores -> Fifteen-Love.
        Assert.Equal("Fifteen-Love", scoreText);
    }

    [Fact]
    public void GameShouldEnd_WhenWinConditionMet()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        bool gameEnded = false;
        Side? winner = null;
        engine.GameEnded += (s, e) => { gameEnded = true; winner = e.Winner; };

        // Simulate Player A winning 4 points (Love -> Fifteen -> Thirty -> Forty -> Win)
        // Note: TennisScoring logic: 4 points and lead by 2.
        // 0-0 -> 15-0 -> 30-0 -> 40-0 -> Win
        
        // Point 1
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        engine.Update(0.1f);
        
        // Point 2
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        engine.Update(0.1f);
        
        // Point 3
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        engine.Update(0.1f);
        
        // Point 4 (Win)
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        engine.Update(0.1f);
        
        // Assert
        Assert.True(gameEnded);
        Assert.Equal(Side.PlayerA, winner);
        Assert.False(engine.IsRunning);
    }

    [Fact]
    public void FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Helper to win a game
        void WinGame(Side winner)
        {
            // Need 4 points to win (assuming other player has 0)
            for (int i = 0; i < 4; i++)
            {
                // Serve
                engine.HandleInput(new InputState { Serve = true });
                engine.Update(0.1f);
                
                // Score
                if (winner == Side.PlayerA)
                {
                    // Ball out Right -> Player A wins point
                    engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
                }
                else
                {
                    // Ball out Left -> Player B wins point
                    engine.Ball.Reset(new PointF(-5, 300), new PointF(-100, 0));
                }
                engine.Update(0.1f); // Trigger HandleScore
            }
        }

        // Act: Win Game 1
        WinGame(Side.PlayerA);
        
        // Assert: Game ended
        Assert.False(engine.IsRunning);
        
        // Act: Start Game 2
        engine.Start();
        engine.HandleInput(new InputState { Serve = true });
        engine.Update(0.1f); // Process serve
        
        // Assert: Speed should be 1.5x (600 * 1.5 = 900)
        Assert.Equal(900f, engine.Ball.Speed);
    }
}
