# API Contracts: 網球單局計分系統

**Feature**: 001-tennis-scoring
**Date**: 2025-11-22

## 概述

此目錄包含網球單局計分系統的公開 API 契約（介面定義）。這些契約定義了系統的核心行為規範，確保實作符合功能需求。

## 契約清單

### 1. IGame.cs

**用途**: 定義網球單局（Game）的核心操作契約

**核心方法**:
- `void PointWonBy(Side side)`: 記錄球員得分
- `string GetScoreText()`: 查詢當前比分文字
- `void Reset()`: 重新開始新局

**核心屬性**:
- `int PlayerAScore`: A 球員得分次數（唯讀）
- `int PlayerBScore`: B 球員得分次數（唯讀）
- `Side? Winner`: 獲勝球員（唯讀）
- `bool IsFinished`: 比賽是否結束（唯讀）

**例外處理**:
- 當 `IsFinished = true` 時呼叫 `PointWonBy()` 會拋出 `InvalidOperationException`

**使用情境**:
```csharp
IGame game = new Game();
game.PointWonBy(Side.PlayerA); // A 得 1 分
game.GetScoreText(); // "Fifteen-Love"
game.PointWonBy(Side.PlayerB); // B 得 1 分
game.GetScoreText(); // "Fifteen-All"
```

---

### 2. Side.cs

**用途**: 定義球員方列舉

**列舉值**:
- `PlayerA = 0`: A 球員
- `PlayerB = 1`: B 球員

**使用情境**:
```csharp
game.PointWonBy(Side.PlayerA);
if (game.Winner == Side.PlayerB)
{
    Console.WriteLine("PlayerB 獲勝！");
}
```

---

### 3. IScoreFormatter.cs

**用途**: 定義分數格式化服務契約

**核心方法**:
- `string FormatScore(int playerAScore, int playerBScore)`: 將數字分數轉換為網球術語

**轉換規則**:

| 條件 | 輸出格式 | 範例 |
|------|---------|------|
| 雙方 0-2 分且平手 | `{Score}-All` | `Love-All`, `Fifteen-All` |
| 雙方 >= 3 分且平手 | `Deuce` | `Deuce` |
| 雙方 >= 3 分且差距 1 分 | `{Leader} Adv` | `PlayerA Adv` |
| 一方 >= 4 分且領先 >= 2 分 | `{Winner} Win` | `PlayerA Win` |
| 其他（非平手） | `{A 分數}-{B 分數}` | `Fifteen-Love` |

**使用情境**:
```csharp
IScoreFormatter formatter = new ScoreFormatter();
formatter.FormatScore(0, 0); // "Love-All"
formatter.FormatScore(3, 3); // "Deuce"
formatter.FormatScore(4, 3); // "PlayerA Adv"
formatter.FormatScore(5, 3); // "PlayerA Win"
```

---

## 設計原則

### 1. 介面隔離原則（ISP）

- `IGame`: 專注於狀態管理（得分、重置、查詢）
- `IScoreFormatter`: 專注於文字轉換（無狀態）
- 兩者職責分離，符合 SRP

### 2. 依賴反轉原則（DIP）

- 未來整合測試時，可透過 `IGame` 進行 Mock
- `Game` 類別可依賴 `IScoreFormatter` 介面而非具體實作
- 支援未來國際化（實作不同語言的 Formatter）

### 3. 開放封閉原則（OCP）

- 新增 `TieBreakGame` 可實作 `IGame` 介面，無需修改現有程式碼
- 新增 `ChineseScoreFormatter` 可實作 `IScoreFormatter`，擴充多語系支援

## 測試案例對應

契約設計涵蓋規格中所有功能需求：

- ✅ **FR-001**: `PlayerAScore`, `PlayerBScore` 屬性
- ✅ **FR-002**: `IScoreFormatter.FormatScore()` 分數映射
- ✅ **FR-003**: `FormatScore()` 平分邏輯（All, Deuce）
- ✅ **FR-004**: `FormatScore()` 非平分格式
- ✅ **FR-005**: `FormatScore()` Advantage 邏輯
- ✅ **FR-006**: `Winner` 屬性及獲勝判定
- ✅ **FR-007**: `PointWonBy()` 方法
- ✅ **FR-008**: `GetScoreText()` 方法
- ✅ **FR-009**: `Reset()` 方法
- ✅ **FR-010**: `PointWonBy()` 例外處理
- ✅ **FR-011**: `IGame` 建構函式隱含 0-0 起始（由實作保證）

## 版本控制

**當前版本**: 1.0.0
**變更歷程**:
- 2025-11-22: 初始版本，定義 IGame, Side, IScoreFormatter 契約

**未來擴充考量**:
- 新增 `ISet` 介面（Set 計分）
- 新增 `IMatch` 介面（Match 計分）
- 新增 `IGameEventListener` 介面（事件通知機制）
