namespace TennisScoring.Core.Services;

/// <summary>
/// 定義分數格式化服務的契約
/// </summary>
public interface IScoreFormatter
{
    /// <summary>
    /// 將雙方原始得分轉換為網球術語文字
    /// </summary>
    /// <param name="playerAScore">A 球員的得分次數（>= 0）</param>
    /// <param name="playerBScore">B 球員的得分次數（>= 0）</param>
    /// <returns>
    /// 網球術語文字，遵循以下規則：
    /// - 基本計分（0-3）：Love, Fifteen, Thirty, Forty
    /// - 平分：Love-All, Fifteen-All, Thirty-All
    /// - Deuce：雙方 >= 3 分且平手
    /// - Advantage：Deuce 後一方領先 1 分（格式 "PlayerA Adv" 或 "PlayerB Adv"）
    /// - Win：一方 >= 4 分且領先 >= 2 分（格式 "PlayerA Win" 或 "PlayerB Win"）
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 當分數為負數時拋出
    /// </exception>
    string FormatScore(int playerAScore, int playerBScore);
}
