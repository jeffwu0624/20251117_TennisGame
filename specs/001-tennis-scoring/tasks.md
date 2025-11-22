# Implementation Tasks: 網球單局計分系統

**Feature**: 001-tennis-scoring
**Branch**: `001-tennis-scoring`
**Generated**: 2025-11-22
**Spec**: [spec.md](./spec.md) | **Plan**: [plan.md](./plan.md)

## 總覽

本文件包含網球單局計分系統的完整實作任務清單，遵循 TDD 開發流程（僅使用 xUnit 單元測試，無 BDD）。採用簡化設計，避免過度工程。每個任務對應一次原子性 commit。

### 設計原則（避免過度設計）

- ✅ **使用**: 簡單的類別與方法
- ✅ **使用**: 直接的計分邏輯（if-else 判斷）
- ✅ **使用**: xUnit 單元測試（無 SpecFlow）
- ❌ **避免**: 複雜的設計模式（State Pattern, Strategy Pattern）
- ❌ **避免**: 過多的介面抽象（僅在必要時使用）
- ❌ **避免**: 過度拆分類別

### 任務統計

- **總任務數**: 21
- **Setup 階段**: 4 個任務
- **Foundational 階段**: 2 個任務
- **使用者故事階段**: 12 個任務
  - US1 (基本計分): 3 個任務
  - US2 (Deuce 狀態): 3 個任務
  - US3 (Advantage 狀態): 3 個任務
  - US4 (獲勝判定): 3 個任務
- **Polish 階段**: 3 個任務

### MVP 範圍建議

**最小可行產品 (MVP)**: 僅實作使用者故事 1 (基本計分 0-3 分)
- 核心計分邏輯
- 可獨立展示並測試
- 預估 LOC: ~100 行（含測試 ~200 行）

---

## Phase 1: 專案設定

**目標**: 建立最小化的 .NET 專案結構

### 任務清單

- [ ] T001 建立 Solution 和核心類別庫專案 `src/TennisScoring/TennisScoring.csproj` (target: net8.0)
- [ ] T002 建立單元測試專案 `tests/TennisScoring.Tests/TennisScoring.Tests.csproj` 並參考核心專案
- [ ] T003 [P] 配置專案 .csproj 啟用 Nullable Reference Types 和 TreatWarningsAsErrors
- [ ] T004 [P] 安裝測試套件（xUnit 2.6.0, FluentAssertions 6.12.0）並執行 `dotnet restore`

**驗證標準**:
- `dotnet build` 成功編譯
- `dotnet test` 執行無錯誤

**專案結構**（簡化版）:
```
src/TennisScoring/
  ├── Game.cs              # 主要計分類別
  ├── Side.cs              # 球員方列舉
  └── TennisScoring.csproj

tests/TennisScoring.Tests/
  ├── GameTests.cs         # 所有測試
  └── TennisScoring.Tests.csproj
```

---

## Phase 2: 基礎元件

**目標**: 實作最基本的領域模型

### 任務清單

- [ ] T005 實作 Side 列舉於 `src/TennisScoring/Side.cs`（PlayerA = 0, PlayerB = 1）
- [ ] T006 建立 Game 類別骨架於 `src/TennisScoring/Game.cs`（包含 PlayerAScore, PlayerBScore 私有欄位及建構函式初始化為 0）

**驗證標準**:
- `dotnet build` 編譯成功，無警告

---

## Phase 3: 使用者故事 1 - 基本計分（0-3分） [P1]

**故事目標**: 正確顯示 0 到 3 分範圍內的比分文字

**獨立測試標準**: 能輸入 A/B 球員各自 0-3 分，輸出正確文字（Love-All, Fifteen-Love 等）

### 任務清單

- [ ] T007 [P] [US1] 撰寫單元測試於 `tests/TennisScoring.Tests/GameTests.cs` 涵蓋所有基本計分情境（0-0 至 3-3，共 9 個測試方法）
- [ ] T008 [US1] 實作 Game.PointWonBy(Side) 方法於 `src/TennisScoring/Game.cs`（累加對應球員分數）
- [ ] T009 [US1] 實作 Game.GetScoreText() 方法於 `src/TennisScoring/Game.cs`（包含基本分數映射 0→Love, 1→Fifteen, 2→Thirty, 3→Forty，以及平分/非平分格式邏輯）

**測試範例**（涵蓋規格中的 9 個驗收情境）:
```csharp
[Fact]
public void NewGame_ShouldReturn_LoveAll()
{
    var game = new Game();
    game.GetScoreText().Should().Be("Love-All");
}

[Fact]
public void ScoreOneZero_ShouldReturn_FifteenLove()
{
    var game = new Game();
    game.PointWonBy(Side.PlayerA);
    game.GetScoreText().Should().Be("Fifteen-Love");
}

// ... 7 more test methods covering all scenarios
```

**驗證標準**:
- ✅ 所有 9 個單元測試通過
- ✅ 覆蓋率 >80%

---

## Phase 4: 使用者故事 2 - Deuce 狀態 [P2]

**故事目標**: 雙方 >= 3 分且平手時顯示「Deuce」

**獨立測試標準**: 能讓雙方都達到 3+ 分且相同，驗證輸出「Deuce」

### 任務清單

- [ ] T010 [P] [US2] 撰寫單元測試於 `tests/TennisScoring.Tests/GameTests.cs` 測試 Deuce 情境（3-3, 4-4, 5-5，共 3 個測試方法）
- [ ] T011 [US2] 擴充 Game.GetScoreText() 於 `src/TennisScoring/Game.cs` 加入 Deuce 判斷邏輯（if a >= 3 && b >= 3 && a == b）
- [ ] T012 [US2] 執行所有測試確認 US1 未受影響（迴歸測試）

**驗證標準**:
- ✅ 3 個 Deuce 測試通過
- ✅ US1 測試仍全部通過

---

## Phase 5: 使用者故事 3 - Advantage 狀態 [P3]

**故事目標**: Deuce 後某方領先 1 分時顯示「PlayerX Adv」

**獨立測試標準**: 在 Deuce 後讓某方多得 1 分，驗證輸出「PlayerA Adv」或「PlayerB Adv」

### 任務清單

- [ ] T013 [P] [US3] 撰寫單元測試於 `tests/TennisScoring.Tests/GameTests.cs` 測試 Advantage 情境（4-3, 3-4, 5-4, 4-5，共 4 個測試方法）
- [ ] T014 [US3] 擴充 Game.GetScoreText() 於 `src/TennisScoring/Game.cs` 加入 Advantage 判斷邏輯（if a >= 3 && b >= 3 && |a-b| == 1）
- [ ] T015 [US3] 執行所有測試確認 US1-US2 未受影響（迴歸測試）

**驗證標準**:
- ✅ 4 個 Advantage 測試通過
- ✅ US1-US2 測試仍全部通過

---

## Phase 6: 使用者故事 4 - 獲勝判定 [P4]

**故事目標**: 某方 >= 4 分且領先 >= 2 分時顯示「PlayerX Win」，並阻止後續得分

**獨立測試標準**: 驗證獲勝條件、Win 文字輸出、比賽結束後例外處理

### 任務清單

- [ ] T016 [P] [US4] 撰寫單元測試於 `tests/TennisScoring.Tests/GameTests.cs` 測試獲勝情境（5-3, 3-5, 4-0, 4-1, 4-2，共 5 個測試方法）及 Winner/IsFinished 屬性
- [ ] T017 [P] [US4] 撰寫單元測試於 `tests/TennisScoring.Tests/GameTests.cs` 測試比賽結束後呼叫 PointWonBy() 拋出 InvalidOperationException
- [ ] T018 [US4] 實作 Game.Winner 和 IsFinished 屬性於 `src/TennisScoring/Game.cs`（Winner 為 Side?, IsFinished 由 Winner != null 推導）
- [ ] T019 [US4] 擴充 Game.PointWonBy() 於 `src/TennisScoring/Game.cs` 加入獲勝判定邏輯（累加分數後檢查獲勝條件，設定 Winner）並在方法開頭檢查 IsFinished 拋出例外
- [ ] T020 [US4] 擴充 Game.GetScoreText() 於 `src/TennisScoring/Game.cs` 加入獲勝判斷邏輯（if (a >= 4 || b >= 4) && |a-b| >= 2）
- [ ] T021 [US4] 執行所有測試確認 US1-US3 未受影響（迴歸測試）

**驗證標準**:
- ✅ 6 個獲勝相關測試通過
- ✅ 所有 US1-US4 測試通過（共 22+ 個測試）

---

## Phase 7: 完善

**目標**: 實作剩餘功能與邊界測試

### 任務清單

- [ ] T022 [P] 撰寫邊界測試於 `tests/TennisScoring.Tests/GameTests.cs`（測試超過 10 分的 Deuce 循環，如 10-10, 11-10）
- [ ] T023 [P] 實作 Game.Reset() 方法於 `src/TennisScoring/Game.cs`（重置 PlayerAScore, PlayerBScore 為 0，Winner 為 null）並撰寫測試
- [ ] T024 [P] 加入 XML 文件註解於 `src/TennisScoring/Game.cs` 和 `src/TennisScoring/Side.cs` 所有公開成員

**驗證標準**:
- ✅ 邊界測試通過
- ✅ Reset 測試通過
- ✅ 最終測試覆蓋率 >80%
- ✅ 所有公開 API 有 XML 文件註解

---

## 實作策略

### TDD 紅綠重構循環

每個使用者故事遵循：
1. **紅（Red）**: 撰寫失敗測試
2. **綠（Green）**: 撰寫最小可行實作
3. **重構（Refactor）**: 優化程式碼（保持測試通過）
4. **Commit**: 原子性 commit

### MVP 優先（推薦）

**第一次迭代**: Phase 1 + Phase 2 + Phase 3 (US1)
- 交付時間: ~半天
- 可展示基本計分功能

**後續迭代**: 依序增加 US2 → US3 → US4
- 每個故事約 1-2 小時

---

## 依賴關係

```
Phase 1 (Setup)
    ↓
Phase 2 (Foundational)
    ↓
Phase 3 (US1: 基本計分) ← 獨立，MVP 範圍
    ↓
Phase 4 (US2: Deuce) ← 依賴 US1
    ↓
Phase 5 (US3: Advantage) ← 依賴 US1 + US2
    ↓
Phase 6 (US4: 獲勝判定) ← 依賴 US1 + US2 + US3
    ↓
Phase 7 (Polish)
```

---

## 核心實作指引（避免過度設計）

### Game.cs 簡化實作範例

```csharp
public class Game
{
    private int _playerAScore;
    private int _playerBScore;

    public Side? Winner { get; private set; }
    public bool IsFinished => Winner != null;

    public Game()
    {
        _playerAScore = 0;
        _playerBScore = 0;
        Winner = null;
    }

    public void PointWonBy(Side side)
    {
        if (IsFinished)
            throw new InvalidOperationException("Game has already finished.");

        if (side == Side.PlayerA)
            _playerAScore++;
        else
            _playerBScore++;

        // 檢查獲勝條件
        if ((_playerAScore >= 4 || _playerBScore >= 4) &&
            Math.Abs(_playerAScore - _playerBScore) >= 2)
        {
            Winner = _playerAScore > _playerBScore ? Side.PlayerA : Side.PlayerB;
        }
    }

    public string GetScoreText()
    {
        // 獲勝判斷
        if ((_playerAScore >= 4 || _playerBScore >= 4) &&
            Math.Abs(_playerAScore - _playerBScore) >= 2)
        {
            return _playerAScore > _playerBScore ? "PlayerA Win" : "PlayerB Win";
        }

        // Deuce/Advantage 判斷
        if (_playerAScore >= 3 && _playerBScore >= 3)
        {
            if (_playerAScore == _playerBScore)
                return "Deuce";
            return _playerAScore > _playerBScore ? "PlayerA Adv" : "PlayerB Adv";
        }

        // 基本計分（0-3）
        string scoreA = MapScore(_playerAScore);
        string scoreB = MapScore(_playerBScore);

        if (_playerAScore == _playerBScore)
            return $"{scoreA}-All";
        return $"{scoreA}-{scoreB}";
    }

    private static string MapScore(int points)
    {
        return points switch
        {
            0 => "Love",
            1 => "Fifteen",
            2 => "Thirty",
            3 => "Forty",
            _ => throw new ArgumentOutOfRangeException(nameof(points))
        };
    }

    public void Reset()
    {
        _playerAScore = 0;
        _playerBScore = 0;
        Winner = null;
    }
}
```

**設計說明**:
- ✅ 單一類別包含所有邏輯（無需 ScoreFormatter 分離）
- ✅ 直接的 if-else 判斷（無需 State Pattern）
- ✅ 私有方法 MapScore() 輔助分數映射
- ✅ 簡單明瞭，易於測試和維護

---

## 測試執行指令

```bash
# 執行所有測試
dotnet test

# 執行測試並顯示詳細輸出
dotnet test --logger "console;verbosity=detailed"

# 產生覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"
```

---

## 驗收檢查清單

- [ ] 所有 22+ 個單元測試通過（涵蓋規格中所有驗收情境）
- [ ] 程式碼覆蓋率 >80%
- [ ] `dotnet build` 無警告
- [ ] 邊界測試通過（超過 10 分的情境）
- [ ] 例外處理測試通過（比賽結束後得分）
- [ ] Reset 功能測試通過
- [ ] 所有公開 API 有 XML 文件註解

---

**任務清單產生完成！** 🎯

總任務數從 44 個精簡至 **21 個任務**，移除 BDD 複雜度，採用簡化設計。

建議立即開始執行 Phase 1 Setup 任務（T001-T004）。
