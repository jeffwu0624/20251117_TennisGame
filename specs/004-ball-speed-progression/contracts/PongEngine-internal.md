# API Contract: PongEngine (內部方法修改)

**Feature**: 004-ball-speed-progression  
**Created**: 2025-11-23  
**Contract Type**: 內部實作（不修改公開介面）

## 概述

本功能不修改公開 API 介面 `IPongGameEngine`，僅修改 `PongEngine` 類別的內部私有方法實作。以下記錄內部契約變更以供參考。

---

## 類別：PongEngine

### 新增私有欄位

```csharp
namespace TennisScoring.WinForms.Engine;

public class PongEngine : IPongGameEngine
{
    // ... 現有欄位 ...
    
    /// <summary>
    /// 當前球速倍率，範圍 1.0 ~ 2.0
    /// 每局遊戲結束後增加 0.5 倍，直到達到上限
    /// </summary>
    private float _speedMultiplier = 1.0f;
}
```

---

## 方法：HandleScore (內部修改)

### 修改前簽章（保持不變）

```csharp
private void HandleScore(Side winner)
```

### 修改後行為

```csharp
private void HandleScore(Side winner)
{
    ScoringGame.PointWonBy(winner);
    ScoreChanged?.Invoke(this, new ScoreChangedEventArgs(ScoringGame.GetScoreText()));

    if (ScoringGame.IsFinished)
    {
        IsRunning = false;
        
        // ✨ 新增：在遊戲結束時更新球速倍率
        _speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f);
        
        var winnerName = ScoringGame.Winner!.Value == Side.PlayerA ? PlayerA.Name : PlayerB.Name;
        GameEnded?.Invoke(this, new GameEndedEventArgs(ScoringGame.Winner!.Value, winnerName, ScoringGame.GetScoreText()));
    }
    else
    {
        ServingSide = winner;
        ResetBall();
    }
}
```

### 行為變更說明

- **觸發條件**: 當 `ScoringGame.IsFinished` 為 `true` 時
- **更新邏輯**: 將 `_speedMultiplier` 增加 0.5，使用 `Math.Min` 確保上限為 2.0
- **執行時機**: 在 `GameEnded` 事件觸發之前，確保下一局開始時使用新倍率

---

## 方法：ServeBall (內部修改)

### 修改前簽章（保持不變）

```csharp
private void ServeBall()
```

### 修改後行為

```csharp
private void ServeBall()
{
    float dirX = ServingSide == Side.PlayerA ? 1 : -1;
    float dirY = (float)(new Random().NextDouble() - 0.5);
    
    float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
    
    // ✨ 修改：套用速度倍率
    float speed = BallSpeed * _speedMultiplier;
    
    // ✨ 新增：更新 Ball.Speed 屬性以保持一致性
    Ball.Speed = speed;
    
    Ball.Velocity = new PointF((dirX / length) * speed, (dirY / length) * speed);
}
```

### 行為變更說明

- **速度計算**: 使用 `BallSpeed * _speedMultiplier` 取代原本的固定 `BallSpeed`
- **Ball.Speed 更新**: 同步設定 `Ball.Speed` 屬性，確保與實際速度一致
- **方向計算**: 保持不變，仍使用標準化向量

---

## 不變的公開契約

以下公開介面方法與屬性**完全不變**：

```csharp
public interface IPongGameEngine
{
    void Start();
    void Stop();
    void Update(float deltaTime);
    void HandleInput(InputState input);
    GameState GetState();
    
    event EventHandler<ScoreChangedEventArgs> ScoreChanged;
    event EventHandler<GameEndedEventArgs> GameEnded;
}
```

### 不變保證

- ✅ 所有公開方法簽章保持不變
- ✅ 所有事件定義保持不變
- ✅ `GameState` 結構保持不變
- ✅ 客戶端程式碼（`GameForm`）無需修改
- ✅ 符合 OCP（Open/Closed Principle）

---

## 測試契約

### 新增測試方法（位於 `PongEngineScoringTests.cs`）

```csharp
namespace TennisScoring.WinForms.Tests;

public class PongEngineScoringTests
{
    [Fact]
    public void FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x()
    {
        // 驗證第一局結束後，下一局球速為 1.5 倍
    }

    [Fact]
    public void SecondGameEnd_ShouldIncreaseBallSpeedTo2_0x()
    {
        // 驗證第二局結束後，下一局球速為 2.0 倍
    }

    [Fact]
    public void ThirdGameEnd_ShouldMaintainBallSpeedAt2_0x()
    {
        // 驗證第三局結束後，球速維持 2.0 倍（上限）
    }

    [Fact]
    public void BallSpeedProperty_ShouldMatchActualVelocityMagnitude()
    {
        // 驗證 Ball.Speed 屬性與實際速度向量大小一致
    }
}
```

---

## 向後相容性

- ✅ **完全向後相容**: 不影響現有任何公開 API
- ✅ **現有測試保持**: 所有現有測試無需修改，應全部通過
- ✅ **無破壞性變更**: 客戶端程式碼無需調整

---

## 實作檢查清單

- [ ] 新增 `_speedMultiplier` 私有欄位並初始化為 `1.0f`
- [ ] 在 `HandleScore` 中檢測 `ScoringGame.IsFinished` 並更新倍率
- [ ] 在 `ServeBall` 中套用倍率計算實際速度
- [ ] 在 `ServeBall` 中更新 `Ball.Speed` 屬性
- [ ] 使用 `Math.Min` 確保倍率上限為 2.0
- [ ] 新增單元測試驗證倍率遞增
- [ ] 新增單元測試驗證上限保護
- [ ] 驗證現有所有測試仍通過
- [ ] 手動測試遊戲體驗（速度變化是否可感知）
