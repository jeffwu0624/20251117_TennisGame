using System.Drawing;

namespace TennisScoring.WinForms.Entities;

public class Paddle
{
    public RectangleF Bounds { get; set; }
    public float Speed { get; set; }
    public Color Color { get; set; }

    public Paddle(float x, float y, float width, float height, float speed, Color color)
    {
        Bounds = new RectangleF(x, y, width, height);
        Speed = speed;
        Color = color;
    }

    public void MoveUp(float deltaTime, float topLimit)
    {
        float newY = Bounds.Y - (Speed * deltaTime);
        if (newY < topLimit)
            newY = topLimit;
        
        Bounds = new RectangleF(Bounds.X, newY, Bounds.Width, Bounds.Height);
    }

    public void MoveDown(float deltaTime, float bottomLimit)
    {
        float newY = Bounds.Y + (Speed * deltaTime);
        if (newY + Bounds.Height > bottomLimit)
            newY = bottomLimit - Bounds.Height;

        Bounds = new RectangleF(Bounds.X, newY, Bounds.Width, Bounds.Height);
    }
}
