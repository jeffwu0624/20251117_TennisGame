using System.Drawing;

namespace TennisScoring.WinForms.Entities;

public class Ball
{
    public PointF Position { get; set; }
    public PointF Velocity { get; set; }
    public float Radius { get; set; }
    public float Speed { get; set; }

    public RectangleF Bounds => new RectangleF(Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2);

    public Ball(float x, float y, float radius, float speed)
    {
        Position = new PointF(x, y);
        Radius = radius;
        Speed = speed;
        Velocity = PointF.Empty;
    }

    public void Move(float deltaTime)
    {
        Position = new PointF(
            Position.X + (Velocity.X * deltaTime),
            Position.Y + (Velocity.Y * deltaTime)
        );
    }

    public void BounceX()
    {
        Velocity = new PointF(-Velocity.X, Velocity.Y);
    }

    public void BounceY()
    {
        Velocity = new PointF(Velocity.X, -Velocity.Y);
    }

    public void Reset(PointF position, PointF velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}
