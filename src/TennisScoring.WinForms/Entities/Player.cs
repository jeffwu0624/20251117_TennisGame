namespace TennisScoring.WinForms.Entities;

public class Player
{
    public string Name { get; }
    public Side Side { get; }
    public Paddle Paddle { get; }

    public Player(string name, Side side, Paddle paddle)
    {
        Name = name;
        Side = side;
        Paddle = paddle;
    }
}
