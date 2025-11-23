# Research Document: 球速遞增機制

**Feature**: 004-ball-speed-progression  
**Created**: 2025-11-23  
**Status**: Complete

## Phase 0: 技術研究與決策

### 研究主題 1：速度倍率儲存位置

**Decision**: 在 `PongEngine` 類別中新增私有欄位 `_speedMultiplier`

**Rationale**:
- `PongEngine` 已負責管理遊戲狀態（球、板子、計分），速度倍率屬於遊戲狀態的一部分
- 封裝於引擎內部，不暴露於公開介面，符合 SRP 原則
- 避免在 `Ball` 類別中儲存倍率，保持 Ball 作為單純的實體類別

**Alternatives Considered**:
1. **在 `Ball` 類別中新增倍率欄位** - 拒絕原因：Ball 應只關注自身物理屬性（位置、速度、半徑），不應知道遊戲規則
2. **建立獨立的 `SpeedManager` 類別** - 拒絕原因：增加不必要的複雜度，違反 YAGNI 原則

---

### 研究主題 2：速度倍率觸發時機

**Decision**: 在 `HandleScore` 方法中檢測 `ScoringGame.IsFinished` 為 true 時更新倍率

**Rationale**:
- `HandleScore` 已是處理得分與遊戲結束邏輯的集中點
- `ScoringGame.IsFinished` 準確標示一局完全結束
- 在 `GameEnded` 事件觸發前更新倍率，確保下一局開始時使用新速度

**Alternatives Considered**:
1. **在 `GameEngine_GameEnded` 事件處理器中更新** - 拒絕原因：該事件在 UI 層處理，違反關注點分離
2. **在 `ResetBall` 方法中更新** - 拒絕原因：ResetBall 在每次得分後都會呼叫，無法區分是否為一局結束

---

### 研究主題 3：速度計算方法

**Decision**: 在 `ServeBall` 方法中將 `BallSpeed` 常數乘以 `_speedMultiplier`

**Rationale**:
- `ServeBall` 是設定球初始速度的唯一位置
- 使用 `Math.Min(_speedMultiplier + 0.5f, 2.0f)` 確保上限為 2.0 倍
- 同時更新 `Ball.Speed` 屬性，保持一致性

**Alternatives Considered**:
1. **直接修改 `BallSpeed` 常數** - 拒絕原因：const 欄位不可變，且每局需要累加而非替換
2. **在每次 `Ball.Move` 時套用倍率** - 拒絕原因：效能損耗，且違反職責分離

---

### 研究主題 4：倍率重置策略

**Decision**: 遊戲重啟時（建立新 `PongEngine` 實例）自動重置為 1.0

**Rationale**:
- `_speedMultiplier` 為私有欄位，初始化為 1.0f
- 每次遊戲重啟會建立新的 `PongEngine` 實例，自然重置
- 符合現有架構模式，無需額外重置邏輯

**Alternatives Considered**:
1. **新增公開的 `Reset()` 方法** - 拒絕原因：違反 OCP 原則，需修改介面
2. **在 `HandleScore` 檢測重啟** - 拒絕原因：HandleScore 不應關心遊戲生命週期管理

---

### 研究主題 5：測試策略

**Decision**: 在 `PongEngineScoringTests.cs` 新增專門的球速遞增測試

**Rationale**:
- 球速遞增與計分邏輯緊密相關，放在 Scoring 測試檔案中合理
- 測試需模擬多局遊戲並驗證速度變化，需要 Scoring 邏輯配合
- 遵循現有測試組織模式（Scoring vs Physics 分離）

**Test Cases**:
1. 第一局結束後，第二局球速為 1.5 倍
2. 第二局結束後，第三局球速為 2.0 倍
3. 第三局結束後，第四局球速維持 2.0 倍（上限測試）
4. 驗證 Ball.Speed 屬性正確更新
5. 驗證物理引擎在各速度下正確運作

**Alternatives Considered**:
1. **新建獨立測試檔案** - 拒絕原因：功能簡單，不需要獨立檔案
2. **放在 Physics 測試中** - 拒絕原因：與物理碰撞邏輯無關，屬於遊戲規則層

---

## 最佳實踐參考

### C# 浮點數運算

```csharp
// 使用 float 字面值後綴 f
private float _speedMultiplier = 1.0f;

// 使用 Math.Min 確保上限
_speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f);
```

### xUnit 測試模式

```csharp
[Fact]
public void GameEnd_ShouldIncreaseSpeedMultiplier()
{
    // Arrange: 設定初始狀態
    // Act: 執行動作
    // Assert: 驗證結果
}
```

### 命名慣例

- 私有欄位：`_camelCase` with underscore prefix
- 常數：`PascalCase`
- 方法：`PascalCase`
- 測試方法：`MethodName_Scenario_ExpectedBehavior`

---

## 風險與緩解措施

### 風險 1：速度過快導致碰撞檢測失效

**Mitigation**: 
- 現有物理引擎使用 delta time 計算，理論上支援任意速度
- 在 Physics 測試中驗證 2.0 倍速下碰撞仍正確
- 如果發現問題，考慮降低上限至 1.5 倍或優化碰撞檢測

### 風險 2：倍率累加精度問題

**Mitigation**:
- 使用簡單的 `_speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f)`
- 避免浮點數累加誤差，直接計算而非多次累加
- 在測試中使用 FluentAssertions 的 `BeApproximately()` 處理浮點數比較

### 風險 3：遊戲重啟後倍率未重置

**Mitigation**:
- 依賴 `PongEngine` 實例化時的欄位初始化
- 在測試中驗證重啟場景
- 文件中明確說明生命週期管理

---

## 實作順序建議

1. **T001**: 新增 `_speedMultiplier` 欄位與初始化
2. **T002**: 在 `HandleScore` 中偵測遊戲結束並更新倍率
3. **T003**: 在 `ServeBall` 中套用倍率計算實際速度
4. **T004**: 更新 `Ball.Speed` 屬性以保持一致性
5. **T005**: 新增單元測試驗證倍率遞增邏輯
6. **T006**: 新增單元測試驗證上限保護
7. **T007**: 驗證現有 Physics 測試在新速度下仍通過

---

## 結論

所有技術決策已完成，無 [NEEDS CLARIFICATION] 標記。可進入 Phase 1 設計階段。
