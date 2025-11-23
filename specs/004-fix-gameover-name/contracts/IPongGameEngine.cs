using System.Drawing;

namespace TennisScoring.WinForms.Engine;

/// <summary>
/// Pong 遊戲引擎的合約。
/// 定義 UI (Form) 與遊戲邏輯之間的互動。
/// </summary>
public interface IPongGameEngine
{
    /// <summary>
    /// 啟動遊戲迴圈。
    /// </summary>
    void Start();

    /// <summary>
    /// 停止遊戲迴圈。
    /// </summary>
    void Stop();

    /// <summary>
    /// 更新單一 Tick 的遊戲狀態。
    /// </summary>
    /// <param name="deltaTime">自上次更新以來經過的時間。</param>
    void Update(float deltaTime);

    /// <summary>
    /// 處理玩家輸入。
    /// </summary>
    /// <param name="input">輸入狀態（按下的按鍵）。</param>
    void HandleInput(InputState input);

    /// <summary>
    /// 取得用於渲染的遊戲實體目前狀態。
    /// </summary>
    GameState GetState();

    /// <summary>
    /// 當分數改變時觸發的事件。
    /// </summary>
    event EventHandler<ScoreChangedEventArgs> ScoreChanged;

    /// <summary>
    /// 當遊戲結束時觸發的事件。
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
    public string WinnerName { get; }
    public string Message { get; }

    public GameEndedEventArgs(Side winner, string winnerName, string message)
    {
        Winner = winner;
        WinnerName = winnerName;
        Message = message;
    }
}
