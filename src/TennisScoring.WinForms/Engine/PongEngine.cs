using System.Drawing;
using TennisScoring.WinForms.Entities;

namespace TennisScoring.WinForms.Engine;

public class PongEngine : IPongGameEngine
{
    public event EventHandler<ScoreChangedEventArgs>? ScoreChanged;
    public event EventHandler<GameEndedEventArgs>? GameEnded;

    public Player PlayerA { get; private set; }
    public Player PlayerB { get; private set; }
    public Ball Ball { get; private set; }
    public Game ScoringGame { get; private set; }
    public bool IsRunning { get; private set; }
    public Side ServingSide { get; private set; }

    public PongEngine(string playerAName, string playerBName, Size gameArea)
    {
        // Initialization logic will be implemented in T010
        PlayerA = null!;
        PlayerB = null!;
        Ball = null!;
        ScoringGame = null!;
    }

    public void Start()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Update(float deltaTime)
    {
        if (!IsRunning) return;
        // Update logic will be implemented in T016
    }

    public void HandleInput(InputState input)
    {
        if (!IsRunning) return;
        // Input handling logic will be implemented in T015
    }

    public GameState GetState()
    {
        // State retrieval logic
        return new GameState
        {
            PlayerAPaddle = PlayerA?.Paddle?.Bounds ?? RectangleF.Empty,
            PlayerBPaddle = PlayerB?.Paddle?.Bounds ?? RectangleF.Empty,
            BallPosition = Ball?.Position ?? PointF.Empty,
            ScoreText = ScoringGame?.GetScoreText() ?? "Initializing..."
        };
    }
}
