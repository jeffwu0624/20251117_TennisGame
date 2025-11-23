using System.Drawing;
using TennisScoring.WinForms.Engine;
using TennisScoring.WinForms.Entities;
using Xunit;

namespace TennisScoring.WinForms.Tests;

public class PongEnginePhysicsTests
{
    [Fact]
    public void Update_ShouldMoveBall()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Force serve
        engine.HandleInput(new InputState { Serve = true });
        engine.Update(0.016f); // Process serve
        
        var initialPos = engine.Ball.Position;
        
        // Act
        engine.Update(0.1f); // Move
        
        // Assert
        Assert.NotEqual(initialPos, engine.Ball.Position);
    }

    [Fact]
    public void Ball_ShouldBounceOffTopWall()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball near top wall, moving up
        engine.Ball.Reset(new PointF(400, 5), new PointF(100, -100));
        
        // Act
        engine.Update(0.1f); // Should hit wall and bounce
        
        // Assert
        Assert.True(engine.Ball.Velocity.Y > 0); // Velocity Y should be positive (down)
    }

    [Fact]
    public void Ball_ShouldBounceOffBottomWall()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball near bottom wall, moving down
        engine.Ball.Reset(new PointF(400, 595), new PointF(100, 100));
        
        // Act
        engine.Update(0.1f); // Should hit wall and bounce
        
        // Assert
        Assert.True(engine.Ball.Velocity.Y < 0); // Velocity Y should be negative (up)
    }

    [Fact]
    public void Ball_ShouldBounceOffPlayerAPaddle()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball near Player A's paddle (Left), moving left
        // Paddle A is at X=30, Width=20. Right edge = 50.
        // Ball Radius = 10.
        engine.Ball.Reset(new PointF(65, 300), new PointF(-100, 0));
        
        // Act
        engine.Update(0.2f); // Move enough to hit
        
        // Assert
        Assert.True(engine.Ball.Velocity.X > 0); // Should bounce right
    }

    [Fact]
    public void Ball_ShouldBounceOffPlayerBPaddle()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Place ball near Player B's paddle (Right), moving right
        // Paddle B is at X=800-30-20 = 750. Left edge = 750.
        engine.Ball.Reset(new PointF(735, 300), new PointF(100, 0));
        
        // Act
        engine.Update(0.2f); // Move enough to hit
        
        // Assert
        Assert.True(engine.Ball.Velocity.X < 0); // Should bounce left
    }

    [Fact]
    public void Ball_AtHighSpeed_ShouldBounceOffWallCorrectly()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Set high speed (2.0x = 1200)
        float highSpeed = 1200f;
        engine.Ball.Reset(new PointF(400, 15), new PointF(highSpeed, -highSpeed)); // Moving up-right fast
        
        // Act
        engine.Update(0.02f); // Small step, but fast movement
        
        // Assert
        // Should have hit top wall (Y=0) and bounced down
        Assert.True(engine.Ball.Velocity.Y > 0, "Ball should bounce down off top wall at high speed");
    }

    [Fact]
    public void Ball_AtHighSpeed_ShouldBounceOffPaddleCorrectly()
    {
        // Arrange
        var engine = new PongEngine("A", "B", new Size(800, 600));
        engine.Start();
        
        // Set high speed (2.0x = 1200)
        float highSpeed = 1200f;
        
        // Place ball near Player B's paddle (Right), moving right fast
        // Paddle B is at X=750.
        engine.Ball.Reset(new PointF(730, 300), new PointF(highSpeed, 0));
        
        // Act
        engine.Update(0.02f); // Move
        
        // Assert
        Assert.True(engine.Ball.Velocity.X < 0, "Ball should bounce left off paddle at high speed");
    }
}
