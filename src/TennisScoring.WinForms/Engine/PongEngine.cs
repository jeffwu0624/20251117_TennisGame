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

    /// <summary>
    /// 當前球速倍率，範圍 1.0 ~ 2.0
    /// 每局遊戲結束後增加 0.5 倍，直到達到上限
    /// </summary>
    private float _speedMultiplier = 1.0f;

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

        // 初始化球拍
        float centerY = (gameArea.Height - PaddleHeight) / 2;
        var paddleA = new Paddle(PaddleMargin, centerY, PaddleWidth, PaddleHeight, PaddleSpeed, Color.Blue);
        var paddleB = new Paddle(gameArea.Width - PaddleMargin - PaddleWidth, centerY, PaddleWidth, PaddleHeight, PaddleSpeed, Color.Red);

        // 初始化玩家
        PlayerA = new Player(playerAName, Side.PlayerA, paddleA);
        PlayerB = new Player(playerBName, Side.PlayerB, paddleB);

        // 初始化球
        Ball = new Ball(0, 0, BallRadius, BallSpeed);

        // 隨機決定發球方
        ServingSide = new Random().Next(2) == 0 ? Side.PlayerA : Side.PlayerB;
        
        ResetBall();
    }

    private void ResetBall()
    {
        // 將球放置在發球方球拍前方
        if (ServingSide == Side.PlayerA)
        {
            Ball.Reset(
                new PointF(PlayerA.Paddle.Bounds.Right + BallRadius + 5, PlayerA.Paddle.Bounds.Y + PaddleHeight / 2),
                PointF.Empty // 發球前速度為 0
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

        // 處理輸入 (移動)
        if (_currentInput.PlayerAUp) PlayerA.Paddle.MoveUp(deltaTime, 0);
        if (_currentInput.PlayerADown) PlayerA.Paddle.MoveDown(deltaTime, _gameArea.Height);

        if (_currentInput.PlayerBUp) PlayerB.Paddle.MoveUp(deltaTime, 0);
        if (_currentInput.PlayerBDown) PlayerB.Paddle.MoveDown(deltaTime, _gameArea.Height);

        // 等待發球時同步球與球拍位置
        if (Ball.Velocity.IsEmpty)
        {
            if (ServingSide == Side.PlayerA)
            {
                Ball.Position = new PointF(PlayerA.Paddle.Bounds.Right + BallRadius + 5, PlayerA.Paddle.Bounds.Y + PaddleHeight / 2);
            }
            else
            {
                Ball.Position = new PointF(PlayerB.Paddle.Bounds.Left - BallRadius - 5, PlayerB.Paddle.Bounds.Y + PaddleHeight / 2);
            }
        }

        // 處理輸入 (發球)
        if (_currentInput.Serve && Ball.Velocity.IsEmpty)
        {
            ServeBall();
        }
        
        // 物理與碰撞運算
        UpdatePhysics(deltaTime);
    }

    private void UpdatePhysics(float deltaTime)
    {
        // 移動球
        Ball.Move(deltaTime);

        // 牆壁碰撞 (上/下)
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

        // 球拍碰撞
        if (Ball.Bounds.IntersectsWith(PlayerA.Paddle.Bounds))
        {
            // 檢查球是否向球拍移動 (避免黏在內部)
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

        // 得分判定 (左/右牆)
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
            
            // 在遊戲結束時更新球速倍率
            _speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f);
            
            var winnerName = ScoringGame.Winner!.Value == Side.PlayerA ? PlayerA.Name : PlayerB.Name;
            GameEnded?.Invoke(this, new GameEndedEventArgs(ScoringGame.Winner!.Value, winnerName, ScoringGame.GetScoreText()));
            
            // 重置遊戲狀態以便進行下一局
            ScoringGame.Reset();
            ResetBall();
        }
        else
        {
            ServingSide = winner;
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
        // 增加些微隨機 Y 分量 (-0.5 到 0.5)
        float dirY = (float)(new Random().NextDouble() - 0.5); 
        
        // 正規化向量
        float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
        
        // 套用速度倍率
        float speed = BallSpeed * _speedMultiplier;
        
        // 更新 Ball.Speed 屬性以保持一致性
        Ball.Speed = speed;
        
        Ball.Velocity = new PointF((dirX / length) * speed, (dirY / length) * speed);
    }

    public GameState GetState()
    {
        // 狀態檢索邏輯
        return new GameState
        {
            PlayerAPaddle = PlayerA?.Paddle?.Bounds ?? RectangleF.Empty,
            PlayerBPaddle = PlayerB?.Paddle?.Bounds ?? RectangleF.Empty,
            BallPosition = Ball?.Position ?? PointF.Empty,
            ScoreText = ScoringGame?.GetScoreText() ?? "Initializing..."
        };
    }
}
