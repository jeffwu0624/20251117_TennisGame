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

    private readonly Size _gameArea;
    private const float PaddleWidth = 20f;
    private const float PaddleHeight = 100f;
    private const float PaddleSpeed = 400f;
    private const float BallRadius = 10f;
    private const float BallSpeed = 500f;
    private const float PaddleMargin = 30f;

    public PongEngine(string playerAName, string playerBName, Size gameArea)
    {
        _gameArea = gameArea;
        ScoringGame = new Game();

        // Initialize Paddles
        float centerY = (gameArea.Height - PaddleHeight) / 2;
        var paddleA = new Paddle(PaddleMargin, centerY, PaddleWidth, PaddleHeight, PaddleSpeed, Color.Blue);
        var paddleB = new Paddle(gameArea.Width - PaddleMargin - PaddleWidth, centerY, PaddleWidth, PaddleHeight, PaddleSpeed, Color.Red);

        // Initialize Players
        PlayerA = new Player(playerAName, Side.PlayerA, paddleA);
        PlayerB = new Player(playerBName, Side.PlayerB, paddleB);

        // Initialize Ball
        Ball = new Ball(0, 0, BallRadius, BallSpeed);

        // Randomize Server
        ServingSide = new Random().Next(2) == 0 ? Side.PlayerA : Side.PlayerB;
        
        ResetBall();
    }

    private void ResetBall()
    {
        // Place ball in front of the serving paddle
        if (ServingSide == Side.PlayerA)
        {
            Ball.Reset(
                new PointF(PlayerA.Paddle.Bounds.Right + BallRadius + 5, PlayerA.Paddle.Bounds.Y + PaddleHeight / 2),
                PointF.Empty // Velocity 0 until served
            );
        }
        else
        {
            Ball.Reset(
                new PointF(PlayerB.Paddle.Bounds.Left - BallRadius - 5, PlayerB.Paddle.Bounds.Y + PaddleHeight / 2),
                PointF.Empty
            );
        }
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
