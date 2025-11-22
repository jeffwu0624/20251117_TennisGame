namespace TennisScoring.Core.Domain;

/// <summary>
/// 定義網球單局計分系統的公開契約
/// </summary>
public interface IGame
{
    /// <summary>
    /// 取得 A 球員的當前得分次數
    /// </summary>
    int PlayerAScore { get; }

    /// <summary>
    /// 取得 B 球員的當前得分次數
    /// </summary>
    int PlayerBScore { get; }

    /// <summary>
    /// 取得獲勝球員（null 表示比賽尚未結束）
    /// </summary>
    Side? Winner { get; }

    /// <summary>
    /// 取得比賽是否已結束（唯讀屬性，由 Winner 推導）
    /// </summary>
    bool IsFinished { get; }

    /// <summary>
    /// 記錄指定球員得分
    /// </summary>
    /// <param name="side">得分的球員方（PlayerA 或 PlayerB）</param>
    /// <exception cref="InvalidOperationException">
    /// 當比賽已結束（IsFinished = true）時拋出此例外
    /// </exception>
    void PointWonBy(Side side);

    /// <summary>
    /// 取得當前比分的網球術語文字表示
    /// </summary>
    /// <returns>
    /// 比分文字，格式範例：
    /// - "Love-All"（0-0）
    /// - "Fifteen-Love"（1-0）
    /// - "Deuce"（3-3 或以上平手）
    /// - "PlayerA Adv"（一方在 Deuce 後領先 1 分）
    /// - "PlayerA Win"（一方獲勝）
    /// </returns>
    string GetScoreText();

    /// <summary>
    /// 重新開始新局（重置雙方分數為 0，Winner 為 null）
    /// </summary>
    void Reset();
}
