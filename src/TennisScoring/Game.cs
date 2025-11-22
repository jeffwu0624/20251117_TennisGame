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
}
