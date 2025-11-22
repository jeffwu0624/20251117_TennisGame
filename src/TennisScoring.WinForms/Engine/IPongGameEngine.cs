using System.Drawing;

namespace TennisScoring.WinForms.Engine;

/// <summary>
/// Contract for the Pong Game Engine.
/// Defines the interaction between the UI (Form) and the Game Logic.
/// </summary>
public interface IPongGameEngine
{
    /// <summary>
    /// Starts the game loop.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the game loop.
    /// </summary>
    void Stop();

    /// <summary>
    /// Updates the game state for a single tick.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since last update.</param>
    void Update(float deltaTime);

    /// <summary>
    /// Handles player input.
    /// </summary>
    /// <param name="input">Input state (keys pressed).</param>
    void HandleInput(InputState input);

    /// <summary>
    /// Gets the current state of the game entities for rendering.
    /// </summary>
    GameState GetState();

    /// <summary>
    /// Event triggered when the score changes.
    /// </summary>
    event EventHandler<ScoreChangedEventArgs> ScoreChanged;

    /// <summary>
    /// Event triggered when the game ends.
    /// </summary>
    event EventHandler<GameEndedEventArgs> GameEnded;
}

public class InputState
{
    public bool PlayerAUp { get; set; }
    public bool PlayerADown { get; set; }
    public bool PlayerBUp { get; set; }
    public bool PlayerBDown { get; set; }
    public bool Serve { get; set; }
}

public class GameState
{
    public RectangleF PlayerAPaddle { get; set; }
    public RectangleF PlayerBPaddle { get; set; }
    public PointF BallPosition { get; set; }
    public string ScoreText { get; set; } = string.Empty;
}

public class ScoreChangedEventArgs : EventArgs
{
    public string ScoreText { get; }

    public ScoreChangedEventArgs(string scoreText)
    {
        ScoreText = scoreText;
    }
}

public class GameEndedEventArgs : EventArgs
{
    public Side Winner { get; }
    public string Message { get; }

    public GameEndedEventArgs(Side winner, string message)
    {
        Winner = winner;
        Message = message;
    }
}
