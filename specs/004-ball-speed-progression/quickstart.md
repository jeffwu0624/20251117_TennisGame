# Quickstart: 球速遞增機制實作指南

**Feature**: 004-ball-speed-progression  
**Branch**: `004-ball-speed-progression`  
**Target**: 開發人員快速上手指南

---

## 🎯 功能概述

實作球速遞增機制，使每局遊戲結束後下一局的球速自動加快：
- 初始速度：1.0x（600f）
- 第二局：1.5x（900f）
- 第三局及以後：2.0x（1200f，上限）

---

## 📋 前置需求

### 開發環境

- ✅ .NET SDK 10.0 或更新版本
- ✅ Visual Studio 2022 或 VS Code with C# extension
- ✅ Git（已在分支 `004-ball-speed-progression` 上）

### 確認分支

```powershell
git branch
# 應顯示: * 004-ball-speed-progression
```

---

## 🚀 快速開始（5 分鐘）

### 步驟 1: 建置專案

```powershell
cd C:\Projects\SpecKit\20251117_TennisGame
dotnet build TennisScoring.sln -c Debug
```

**預期結果**: 編譯成功，無錯誤

---

### 步驟 2: 執行現有測試

```powershell
dotnet test TennisScoring.sln --no-build
```

**預期結果**: 
- 總計 45 個測試
- 成功 43 個
- 跳過 2 個（待實作）
- 失敗 0 個

---

### 步驟 3: 開啟關鍵檔案

在 VS Code 或 Visual Studio 中開啟：

1. **實作目標**: `src/TennisScoring.WinForms/Engine/PongEngine.cs`
2. **測試目標**: `tests/TennisScoring.WinForms.Tests/PongEngineScoringTests.cs`
3. **參考文件**: `specs/004-ball-speed-progression/data-model.md`

---

## 📝 實作流程（TDD 循環）

### Task 1: 新增速度倍率欄位

#### 1.1 寫失敗測試

在 `PongEngineScoringTests.cs` 新增：

```csharp
[Fact]
public void FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x()
{
    // Arrange
    var engine = new PongEngine("A", "B", new Size(800, 600));
    engine.Start();
    
    // 模擬第一局：Player A 獲勝（4 分）
    for (int i = 0; i < 4; i++)
    {
        engine.Ball.Reset(new PointF(805, 300), new PointF(100, 0));
        engine.Update(0.1f); // Player A 得分
    }
    
    // Act: 開始第二局並發球
    engine.HandleInput(new InputState { Serve = true });
    engine.Update(0.016f);
    
    // Assert: 球速應為 1.5 倍
    float expectedSpeed = 600f * 1.5f; // 900f
    Assert.Equal(expectedSpeed, engine.Ball.Speed, precision: 1);
}
```

#### 1.2 執行測試（應失敗）

```powershell
dotnet test --filter FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x
```

#### 1.3 實作程式碼

在 `PongEngine.cs` 新增欄位：

```csharp
public class PongEngine : IPongGameEngine
{
    // ... 現有欄位 ...
    private float _speedMultiplier = 1.0f;
```

在 `HandleScore` 方法中更新：

```csharp
if (ScoringGame.IsFinished)
{
    IsRunning = false;
    _speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f);
    // ... 其餘邏輯 ...
}
```

在 `ServeBall` 方法中套用：

```csharp
private void ServeBall()
{
    // ... 現有方向計算 ...
    float speed = BallSpeed * _speedMultiplier;
    Ball.Speed = speed;
    Ball.Velocity = new PointF((dirX / length) * speed, (dirY / length) * speed);
}
```

#### 1.4 再次執行測試（應通過）

```powershell
dotnet test --filter FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x
```

#### 1.5 提交原子 commit

```powershell
git add src/TennisScoring.WinForms/Engine/PongEngine.cs
git add tests/TennisScoring.WinForms.Tests/PongEngineScoringTests.cs
git commit -m "T001: 新增速度倍率欄位，第一局結束後球速增至 1.5 倍"
```

---

### Task 2: 驗證上限保護

重複 TDD 循環：

1. **紅燈**: 寫測試驗證第三局後速度維持 2.0 倍
2. **綠燈**: 確認 `Math.Min` 邏輯正確
3. **重構**: 清理測試重複程式碼（如需要）
4. **提交**: 原子 commit

---

### Task 3: 驗證現有測試

```powershell
dotnet test tests/TennisScoring.WinForms.Tests/
```

**確保**: 所有現有測試仍通過

---

## 🧪 測試指令速查

### 執行特定測試類別

```powershell
dotnet test --filter FullyQualifiedName~PongEngineScoringTests
```

### 執行特定測試方法

```powershell
dotnet test --filter FirstGameEnd_ShouldIncreaseBallSpeedTo1_5x
```

### 執行所有 WinForms 測試

```powershell
dotnet test tests/TennisScoring.WinForms.Tests/
```

### 顯示詳細輸出

```powershell
dotnet test --logger "console;verbosity=detailed"
```

---

## 🐛 除錯技巧

### 技巧 1: 輸出速度值

在測試中加入：

```csharp
Console.WriteLine($"Ball Speed: {engine.Ball.Speed}");
Console.WriteLine($"Velocity Magnitude: {Math.Sqrt(engine.Ball.Velocity.X * engine.Ball.Velocity.X + engine.Ball.Velocity.Y * engine.Ball.Velocity.Y)}");
```

### 技巧 2: 斷點位置

建議設定斷點於：
- `PongEngine.HandleScore` 的 `if (ScoringGame.IsFinished)` 行
- `PongEngine.ServeBall` 的速度計算行
- 測試中的 Assert 行前

### 技巧 3: 驗證倍率值

在 `HandleScore` 中暫時加入（除錯後移除）：

```csharp
System.Diagnostics.Debug.WriteLine($"Speed Multiplier: {_speedMultiplier}");
```

---

## ✅ 完成檢查清單

### 程式碼實作

- [ ] `_speedMultiplier` 欄位已新增並初始化為 `1.0f`
- [ ] `HandleScore` 在遊戲結束時正確更新倍率
- [ ] `ServeBall` 正確套用倍率計算速度
- [ ] `Ball.Speed` 屬性正確更新

### 測試覆蓋

- [ ] 測試：第一局結束後速度為 1.5 倍
- [ ] 測試：第二局結束後速度為 2.0 倍
- [ ] 測試：第三局結束後速度維持 2.0 倍
- [ ] 測試：Ball.Speed 與 Velocity 大小一致
- [ ] 所有現有測試仍通過

### 程式碼品質

- [ ] 遵循 C# 命名慣例（私有欄位 `_camelCase`）
- [ ] 使用 `float` 字面值後綴 `f`
- [ ] 使用 `Math.Min` 確保上限
- [ ] 無編譯警告或錯誤
- [ ] 程式碼覆蓋率 >80%

### Git 提交

- [ ] 每個 task 一個原子 commit
- [ ] Commit 訊息使用正體中文
- [ ] Commit 包含測試與實作

---

## 📚 參考資料

### 專案文件

- [規格文件](../spec.md)
- [實作計畫](../plan.md)
- [技術研究](../research.md)
- [資料模型](../data-model.md)
- [API 契約](../contracts/PongEngine-internal.md)

### Constitution 相關

- [專案憲章](../../../.specify/memory/constitution.md)
- SOLID 原則檢查清單
- TDD 工作流程

### .NET 文件

- [C# 命名慣例](https://learn.microsoft.com/zh-tw/dotnet/csharp/fundamentals/coding-style/identifier-names)
- [xUnit 測試框架](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)

---

## 💡 最佳實踐提示

### 提示 1: 遵循 Red-Green-Refactor

始終按照順序：
1. 🔴 寫失敗測試
2. 🟢 寫最少程式碼使測試通過
3. 🔵 重構改善程式碼品質
4. 提交原子 commit

### 提示 2: 保持測試獨立

每個測試應：
- 建立自己的 `PongEngine` 實例
- 不依賴其他測試的執行順序
- 清楚表達測試意圖（Arrange-Act-Assert）

### 提示 3: 使用有意義的變數名稱

```csharp
// ✅ 好
float expectedSpeed = 600f * 1.5f;

// ❌ 不好
float s = 900f;
```

---

## 🎮 手動測試指南

### 建置並執行遊戲

```powershell
cd src/TennisScoring.WinForms
dotnet run
```

### 測試場景

1. **場景 1**: 快速完成一局
   - 讓 Player A 快速得 4 分
   - 觀察第二局球速是否明顯變快

2. **場景 2**: 連續完成三局
   - 觀察第三局後球速是否不再增加
   - 確認遊戲仍可正常進行

3. **場景 3**: 重啟遊戲
   - 關閉並重新啟動遊戲
   - 確認球速重置為初始速度

---

## ❓ 常見問題

### Q1: 測試失敗 - 球速不正確

**解答**: 確認以下幾點：
- `_speedMultiplier` 是否正確初始化
- `HandleScore` 是否在正確時機更新倍率
- `ServeBall` 是否使用倍率計算速度
- 測試中是否正確模擬遊戲結束（4 分）

### Q2: 編譯錯誤 - 找不到 `_speedMultiplier`

**解答**: 確認欄位宣告在類別層級，而非方法內部

### Q3: 所有測試通過但手動測試感覺速度沒變化

**解答**: 
- 確認 `Ball.Speed` 屬性有被更新
- 使用除錯輸出查看實際速度值
- 確認遊戲確實完成一局（顯示 GameEnded 訊息）

---

## 🚧 下一步

完成實作後：

1. 執行完整測試套件
2. 更新 `plan.md` 標記 Phase 1 完成
3. 執行 `/speckit.tasks` 產生詳細任務清單
4. 開始 Phase 2 逐任務實作

---

**快樂編碼！🎉**
