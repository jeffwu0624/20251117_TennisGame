# Data Model: 網球單局計分系統

**Feature**: 001-tennis-scoring
**Date**: 2025-11-22
**Status**: Phase 1 Complete

## 領域實體

### 1. Game（網球局）

**職責**: 追蹤單一網球局的計分狀態，提供得分記錄和查詢介面

**屬性**:

| 屬性名稱 | 型別 | 可空性 | 說明 |
|---------|------|--------|------|
| PlayerAScore | int | 必填 | A 球員的原始得分次數（0, 1, 2, 3, ...） |
| PlayerBScore | int | 必填 | B 球員的原始得分次數（0, 1, 2, 3, ...） |
| Winner | Side? | 可空 | 獲勝球員（未結束為 null，結束後為 PlayerA 或 PlayerB） |
| IsFinished | bool | 必填 | 比賽是否已結束（唯讀屬性，由 Winner 推導） |

**方法**:

| 方法簽名 | 回傳型別 | 說明 |
|---------|---------|------|
| PointWonBy(Side side) | void | 記錄指定球員得分（若比賽已結束則拋出 InvalidOperationException） |
| GetScoreText() | string | 取得當前比分的網球術語文字（如 "Love-All", "Deuce", "PlayerA Adv"） |
| Reset() | void | 重新開始新局（重置雙方分數為 0） |

**狀態轉換邏輯**:

```
初始狀態: PlayerAScore = 0, PlayerBScore = 0, Winner = null
↓
得分階段 (0-3分): 透過 PointWonBy() 累加分數
↓
分支判斷:
├─ 一方 >= 4 分且領先 >= 2 分 → Winner 設為該方, IsFinished = true
├─ 雙方 >= 3 分且平手 → Deuce 狀態（持續至領先 2 分）
└─ 其他 → 繼續得分階段
```

**不變條件（Invariants）**:
- PlayerAScore >= 0 且 PlayerBScore >= 0 (分數不可為負)
- IsFinished = true 時，Winner != null
- IsFinished = true 時，禁止呼叫 PointWonBy()

**驗證規則**:
- PointWonBy() 呼叫前檢查 IsFinished 狀態
- 若 IsFinished = true，拋出 InvalidOperationException("Game has already finished. Cannot record more points.")

---

### 2. Side（球員方）

**職責**: 識別網球比賽中的球員方（PlayerA 或 PlayerB）

**型別**: Enum

**值**:

| 列舉值 | 整數值 | 說明 |
|-------|--------|------|
| PlayerA | 0 | A 球員（發球方或左側球員） |
| PlayerB | 1 | B 球員（接球方或右側球員） |

**使用情境**:
- 傳遞給 `Game.PointWonBy(Side side)` 指定得分方
- 儲存於 `Game.Winner` 表示獲勝方
- 用於 `ScoreFormatter` 生成帶球員標籤的文字（如 "PlayerA Adv"）

---

### 3. ScoreFormatter（分數格式化器）

**職責**: 將 Game 的數字分數轉換為符合網球規則的術語文字

**屬性**: 無（Stateless 靜態服務或單例）

**方法**:

| 方法簽名 | 回傳型別 | 說明 |
|---------|---------|------|
| FormatScore(int playerAScore, int playerBScore) | string | 根據雙方分數計算並回傳網球術語文字 |

**轉換邏輯**:

```csharp
// 偽代碼
string FormatScore(int a, int b)
{
    // 1. 檢查獲勝條件
    if ((a >= 4 || b >= 4) && Math.Abs(a - b) >= 2)
        return a > b ? "PlayerA Win" : "PlayerB Win";

    // 2. 檢查 Deuce/Advantage
    if (a >= 3 && b >= 3)
    {
        if (a == b) return "Deuce";
        return a > b ? "PlayerA Adv" : "PlayerB Adv";
    }

    // 3. 基本計分（0-3）
    string scoreA = MapScore(a); // 0→Love, 1→Fifteen, 2→Thirty, 3→Forty
    string scoreB = MapScore(b);

    if (a == b) return $"{scoreA}-All"; // 平分
    return $"{scoreA}-{scoreB}";       // 非平分
}

string MapScore(int points)
{
    return points switch
    {
        0 => "Love",
        1 => "Fifteen",
        2 => "Thirty",
        3 => "Forty",
        _ => throw new ArgumentOutOfRangeException()
    };
}
```

**測試案例映射**（從規格 spec.md）:

| PlayerA 分數 | PlayerB 分數 | 預期輸出 | 規則類型 |
|-------------|-------------|---------|---------|
| 0 | 0 | Love-All | 平分（基本） |
| 1 | 0 | Fifteen-Love | 非平分（基本） |
| 3 | 3 | Deuce | Deuce |
| 4 | 4 | Deuce | Deuce |
| 4 | 3 | PlayerA Adv | Advantage |
| 5 | 3 | PlayerA Win | 獲勝 |
| 3 | 5 | PlayerB Win | 獲勝 |

---

## 實體關係圖

```
┌─────────────────────────────┐
│         Game                │
├─────────────────────────────┤
│ - PlayerAScore: int         │
│ - PlayerBScore: int         │
│ - Winner: Side?             │
│ + IsFinished: bool          │
├─────────────────────────────┤
│ + PointWonBy(Side): void    │
│ + GetScoreText(): string ───┼──> 呼叫 ScoreFormatter
│ + Reset(): void             │
└─────────────────────────────┘
           │
           │ 使用
           ▼
┌─────────────────────────────┐
│      ScoreFormatter         │
├─────────────────────────────┤
│ (Stateless)                 │
├─────────────────────────────┤
│ + FormatScore(int, int):    │
│   string                    │
└─────────────────────────────┘

┌─────────────────────────────┐
│      Side (Enum)            │
├─────────────────────────────┤
│ PlayerA = 0                 │
│ PlayerB = 1                 │
└─────────────────────────────┘
```

## 資料流程

### 得分記錄流程

```
使用者呼叫 Game.PointWonBy(Side.PlayerA)
  ↓
Game 檢查 IsFinished 狀態
  ├─ true  → 拋出 InvalidOperationException
  └─ false → 繼續
  ↓
PlayerAScore += 1
  ↓
檢查獲勝條件
  ├─ 符合 → 設定 Winner = Side.PlayerA
  └─ 不符 → 維持 Winner = null
```

### 查詢分數流程

```
使用者呼叫 Game.GetScoreText()
  ↓
Game 將 (PlayerAScore, PlayerBScore) 傳給 ScoreFormatter.FormatScore()
  ↓
ScoreFormatter 應用轉換邏輯
  ↓
回傳文字（如 "Thirty-Fifteen"）
```

## 資料驗證

### Game 建構函式驗證

```csharp
public Game()
{
    PlayerAScore = 0;
    PlayerBScore = 0;
    Winner = null;
    // 無需驗證，固定初始化為 0-0
}
```

### PointWonBy 驗證

```csharp
public void PointWonBy(Side side)
{
    if (IsFinished)
        throw new InvalidOperationException(
            "Game has already finished. Cannot record more points.");

    // 累加分數邏輯...
}
```

### ScoreFormatter 驗證

```csharp
private static string MapScore(int points)
{
    if (points < 0 || points > 3)
        throw new ArgumentOutOfRangeException(
            nameof(points),
            "Basic score mapping only supports 0-3 points.");

    // 映射邏輯...
}
```

## 延伸性考量

### 未來擴充至 Set/Match

**設計原則**:
- `Game` 類別保持獨立，不依賴 Set 或 Match
- 透過組合（Composition）建立階層：`Match` 包含多個 `Set`，`Set` 包含多個 `Game`
- 使用介面 `IGame` 實現多型（如 TieBreakGame 可實作相同介面）

**範例結構**:

```
Match (1:N) → Set (1:N) → Game
                       └→ TieBreakGame (實作 IGame)
```

### 國際化支援

**潛在擴充**:
- 新增 `IScoreFormatter` 介面
- 實作 `EnglishScoreFormatter`、`ChineseScoreFormatter` 等
- 透過 DI 注入 Formatter 實例

## 資料模型驗證

### 符合性檢查

- ✅ **FR-001**: Game 類別追蹤 PlayerAScore 和 PlayerBScore
- ✅ **FR-002**: ScoreFormatter 實作分數映射（0→Love, ...）
- ✅ **FR-003**: ScoreFormatter 處理平分邏輯（Fifteen-All, Deuce）
- ✅ **FR-004**: ScoreFormatter 處理非平分格式（Fifteen-Love）
- ✅ **FR-005**: ScoreFormatter 處理 Advantage 狀態
- ✅ **FR-006**: Game 判定獲勝條件並設定 Winner
- ✅ **FR-007**: Game.PointWonBy() 提供記錄介面
- ✅ **FR-008**: Game.GetScoreText() 提供查詢介面
- ✅ **FR-009**: Game.Reset() 提供重置介面
- ✅ **FR-010**: Game.PointWonBy() 在 IsFinished 時拋出例外
- ✅ **FR-011**: Game() 建構函式固定初始化為 0-0

### SRP 符合性

- ✅ **Game**: 單一職責 = 狀態管理（分數追蹤、獲勝判定）
- ✅ **ScoreFormatter**: 單一職責 = 文字轉換（數字→術語）
- ✅ **Side**: 單一職責 = 球員識別

資料模型設計完成，可進行 Phase 1 後續步驟（contracts/, quickstart.md）。
