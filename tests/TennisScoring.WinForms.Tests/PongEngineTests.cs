using System.Drawing;
using TennisScoring.WinForms.Engine;
using Xunit;

namespace TennisScoring.WinForms.Tests;

public class PongEngineTests
{
    [Fact]
    public void Constructor_ShouldInitializePlayersAndBall()
    {
        // Arrange
        var size = new Size(800, 600);
        
        // Act
        var engine = new PongEngine("Alice", "Bob", size);

        // Assert
        Assert.NotNull(engine.PlayerA);
        Assert.Equal("Alice", engine.PlayerA.Name);
        Assert.Equal(Side.PlayerA, engine.PlayerA.Side);
        
        Assert.NotNull(engine.PlayerB);
        Assert.Equal("Bob", engine.PlayerB.Name);
        Assert.Equal(Side.PlayerB, engine.PlayerB.Side);

        Assert.NotNull(engine.Ball);
        Assert.NotNull(engine.ScoringGame);
    }

    [Fact]
    public void Constructor_ShouldSetServingSide()
    {
        // Arrange
        var size = new Size(800, 600);

        // Act
        var engine = new PongEngine("Alice", "Bob", size);

        // Assert
        Assert.True(engine.ServingSide == Side.PlayerA || engine.ServingSide == Side.PlayerB);
    }

    [Fact]
    public void ResetBall_ShouldPlaceBallNearServingPaddle()
    {
        // Arrange
        var size = new Size(800, 600);
        var engine = new PongEngine("Alice", "Bob", size);

        // Act
        // ResetBall is called in constructor

        // Assert
        if (engine.ServingSide == Side.PlayerA)
        {
            Assert.True(engine.Ball.Position.X > engine.PlayerA.Paddle.Bounds.Right);
        }
        else
        {
            Assert.True(engine.Ball.Position.X < engine.PlayerB.Paddle.Bounds.Left);
        }
        
        Assert.Equal(PointF.Empty, engine.Ball.Velocity);
    }
}
