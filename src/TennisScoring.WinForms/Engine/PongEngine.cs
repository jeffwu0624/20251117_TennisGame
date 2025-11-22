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
    private const float PaddleSpeed = 500f;
    private const float BallRadius = 10f;
    private const float BallSpeed = 600f;
    private const float PaddleMargin = 30f;
    
    private InputState _currentInput = new InputState();

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

        // Process Input (Movement)
        if (_currentInput.PlayerAUp) PlayerA.Paddle.MoveUp(deltaTime, 0);
        if (_currentInput.PlayerADown) PlayerA.Paddle.MoveDown(deltaTime, _gameArea.Height);

        if (_currentInput.PlayerBUp) PlayerB.Paddle.MoveUp(deltaTime, 0);
        if (_currentInput.PlayerBDown) PlayerB.Paddle.MoveDown(deltaTime, _gameArea.Height);

        // Process Input (Serve)
        if (_currentInput.Serve && Ball.Velocity.IsEmpty)
        {
            ServeBall();
        }
        
        // Physics & Collision (T016)
        UpdatePhysics(deltaTime);
    }

    private void UpdatePhysics(float deltaTime)
    {
        // Move Ball
        Ball.Move(deltaTime);

        // Wall Collision (Top/Bottom)
        if (Ball.Position.Y - Ball.Radius < 0)
        {
            Ball.BounceY();
            Ball.Position = new PointF(Ball.Position.X, Ball.Radius);
        }
        else if (Ball.Position.Y + Ball.Radius > _gameArea.Height)
        {
            Ball.BounceY();
            Ball.Position = new PointF(Ball.Position.X, _gameArea.Height - Ball.Radius);
        }

        // Paddle Collision
        if (Ball.Bounds.IntersectsWith(PlayerA.Paddle.Bounds))
        {
            // Check if ball is moving towards paddle (to avoid sticking inside)
            if (Ball.Velocity.X < 0)
            {
                Ball.BounceX();
                Ball.Position = new PointF(PlayerA.Paddle.Bounds.Right + Ball.Radius + 1, Ball.Position.Y);
            }
        }
        else if (Ball.Bounds.IntersectsWith(PlayerB.Paddle.Bounds))
        {
            if (Ball.Velocity.X > 0)
            {
                Ball.BounceX();
                Ball.Position = new PointF(PlayerB.Paddle.Bounds.Left - Ball.Radius - 1, Ball.Position.Y);
            }
        }

        // Scoring (Left/Right Walls)
        if (Ball.Position.X + Ball.Radius < 0)
        {
            HandleScore(Side.PlayerB);
        }
        else if (Ball.Position.X - Ball.Radius > _gameArea.Width)
        {
            HandleScore(Side.PlayerA);
        }
    }

    private void HandleScore(Side winner)
    {
        ScoringGame.PointWonBy(winner);
        ScoreChanged?.Invoke(this, new ScoreChangedEventArgs(ScoringGame.GetScoreText()));

        if (ScoringGame.IsFinished)
        {
            IsRunning = false;
            GameEnded?.Invoke(this, new GameEndedEventArgs(ScoringGame.Winner!.Value, ScoringGame.GetScoreText()));
        }
        else
        {
            ResetBall();
        }
    }

    public void HandleInput(InputState input)
    {
        if (!IsRunning) return;
        _currentInput = input;
    }

    private void ServeBall()
    {
        float dirX = ServingSide == Side.PlayerA ? 1 : -1;
        // Add a slight random Y component (-0.5 to 0.5)
        float dirY = (float)(new Random().NextDouble() - 0.5); 
        
        // Normalize vector
        float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
        float speed = BallSpeed;
        
        Ball.Velocity = new PointF((dirX / length) * speed, (dirY / length) * speed);
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
