namespace TennisScoring;

/// <summary>
/// 網球單局計分系統
/// </summary>
public class Game
{
    private int _playerAScore;
    private int _playerBScore;

    /// <summary>
    /// 建立新的網球局，初始分數為 0-0 (Love-All)
    /// </summary>
    public Game()
    {
        _playerAScore = 0;
        _playerBScore = 0;
    }

    /// <summary>
    /// 記錄指定球員得分
    /// </summary>
    /// <param name="side">得分的球員方（PlayerA 或 PlayerB）</param>
    public void PointWonBy(Side side)
    {
        if (side == Side.PlayerA)
            _playerAScore++;
        else
            _playerBScore++;
    }

    /// <summary>
    /// 取得當前比分的網球術語文字表示
    /// </summary>
    /// <returns>比分文字，如 "Love-All"、"Fifteen-Love"、"Deuce" 等</returns>
    public string GetScoreText()
    {
        // Deuce 判斷（雙方 >= 3 分且平手）
        if (_playerAScore >= 3 && _playerBScore >= 3 && _playerAScore == _playerBScore)
            return "Deuce";

        // Advantage 判斷（雙方 >= 3 分且差距為 1）
        if (_playerAScore >= 3 && _playerBScore >= 3 && Math.Abs(_playerAScore - _playerBScore) == 1)
            return _playerAScore > _playerBScore ? "PlayerA Adv" : "PlayerB Adv";

        // 基本計分（0-3）
        string scoreA = MapScore(_playerAScore);
        string scoreB = MapScore(_playerBScore);

        // 平分格式
        if (_playerAScore == _playerBScore)
            return $"{scoreA}-All";

        // 非平分格式
        return $"{scoreA}-{scoreB}";
    }

    /// <summary>
    /// 將數字分數映射為網球術語
    /// </summary>
    private static string MapScore(int points)
    {
        return points switch
        {
            0 => "Love",
            1 => "Fifteen",
            2 => "Thirty",
            3 => "Forty",
            _ => throw new ArgumentOutOfRangeException(nameof(points), "Basic score mapping only supports 0-3 points.")
        };
    }
}
