# Implementation Plan: 網球單局計分系統

**Branch**: `001-tennis-scoring` | **Date**: 2025-11-22 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-tennis-scoring/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

建立一個 .NET Core 10 C# 網球單局計分類別庫（TennisScoring），負責追蹤單一 Game 的計分狀態，將數字分數轉換為正確的網球術語（Love, Fifteen, Thirty, Forty, Deuce, Advantage, Win），並提供完整的 BDD 測試覆蓋。此系統遵循標準 ITF 網球規則，從 Love-All 起始，處理基本計分、Deuce、Advantage 及獲勝判定邏輯。

## Technical Context

**Language/Version**: C# 12+ / .NET Core 10（若尚未發布則使用最新 LTS 版本如 .NET 8）
**Primary Dependencies**: 無外部業務邏輯依賴，測試框架使用 xUnit, SpecFlow (Gherkin BDD), FluentAssertions
**Storage**: N/A（純記憶體計分，不需持久化）
**Testing**: xUnit (單元測試), SpecFlow (BDD 驗收測試), FluentAssertions (斷言), 測試覆蓋率目標 >80%
**Target Platform**: .NET Core 跨平台（Windows, Linux, macOS）
**Project Type**: Single（類別庫專案 + 單元測試專案）
**Performance Goals**: 單次計分計算 <10ms, 支援 1000+ 次連續得分操作無效能衰退
**Constraints**: 記憶體占用 <1MB per Game instance, 無外部 I/O 操作
**Scale/Scope**: 小型類別庫（預估 <500 LOC 生產代碼），核心類別 2-3 個（Game, Side enum, ScoreFormatter 等）

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. Single Responsibility Principle (SRP)
- ✅ **PASS**: 計畫中核心類別設計遵循 SRP
  - `Game` 類別：負責追蹤單一局比賽狀態和得分邏輯
  - `Side` enum：識別球員方（PlayerA/PlayerB）
  - `ScoreFormatter`（或類似）：負責將數字分數轉換為網球術語文字
  - 每個類別有明確的單一職責，符合 SRP 原則

### II. Open/Closed & Liskov Substitution Principles (OCP + LSP)
- ✅ **PASS**: 設計允許未來擴充（如 Set/Match 計分）而無需修改現有 Game 邏輯
  - 透過介面（如 `IGame`）可實現 OCP，未來可擴充為 `Set`、`Match` 類別
  - 當前無繼承結構，LSP 暫不適用，未來若引入多型設計需遵守

### III. Interface Segregation & Dependency Inversion Principles (ISP + DIP)
- ✅ **PASS**: 此階段為獨立類別庫，無外部依賴需注入
  - 未來若需整合外部服務（如持久化、事件發布），將透過介面和 DI 實現
  - 測試中可透過抽象介面（如 `IGame`）實現 mock，符合 DIP

### IV. Testability & Atomic Commits
- ✅ **PASS**: 需求明確要求 BDD 測試（SpecFlow + Gherkin）及 >80% 覆蓋率
  - 將遵循 TDD 紅綠重構循環
  - 所有功能需求（FR-001 至 FR-011）皆有對應驗收情境
  - 每個 tasks.md 任務將對應一次原子性 commit
  - 使用 xUnit + FluentAssertions 進行單元測試

### V. Explicit Configuration & Observability
- ✅ **PASS**: 此階段為純計算邏輯類別庫，無需外部配置或日誌
  - 未來若整合 API 或服務，將遵循 appsettings.json + 結構化日誌規範
  - 當前無機敏資訊需保護

### VI. Traditional Chinese Documentation
- ✅ **PASS**: 所有規格文件（spec.md, plan.md）已使用正體中文撰寫
  - 程式碼類別、方法、變數將遵循英文命名規範（如 `Game.PointWonBy(Side side)`）
  - 複雜業務邏輯將使用正體中文註解
  - Commit messages 將使用正體中文

### .NET Core Standards
- ✅ **PASS**: 技術堆疊符合憲章要求
  - 使用 .NET Core 10（或最新 LTS）+ C# 12+
  - 啟用 Nullable Reference Types
  - 測試框架：xUnit + FluentAssertions
  - 專案結構將遵循 Clean Architecture（Domain 邏輯於 Core 專案）
  - 將啟用 TreatWarningsAsErrors 和 .NET Analyzers

### 初步結論
- **GATE STATUS**: ✅ **PASS** - 無憲章違規，可繼續進行 Phase 0 研究

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── TennisScoring.Core/               # 類別庫專案（核心業務邏輯）
│   ├── Domain/
│   │   ├── Game.cs                   # 網球局計分核心類別
│   │   └── Side.cs                   # 球員方列舉（PlayerA, PlayerB）
│   ├── Services/
│   │   └── ScoreFormatter.cs         # 分數轉換為網球術語文字
│   └── TennisScoring.Core.csproj
│
└── TennisScoring.Core.sln            # Solution 檔案

tests/
├── TennisScoring.UnitTests/          # 單元測試專案
│   ├── Domain/
│   │   └── GameTests.cs              # Game 類別單元測試
│   ├── Services/
│   │   └── ScoreFormatterTests.cs    # ScoreFormatter 單元測試
│   └── TennisScoring.UnitTests.csproj
│
└── TennisScoring.AcceptanceTests/    # BDD 驗收測試專案（SpecFlow）
    ├── Features/
    │   ├── BasicScoring.feature      # 基本計分情境（0-3分）
    │   ├── DeuceHandling.feature     # Deuce 狀態情境
    │   ├── AdvantageHandling.feature # Advantage 狀態情境
    │   └── WinCondition.feature      # 獲勝判定情境
    ├── StepDefinitions/
    │   └── TennisScoringSteps.cs     # Gherkin 步驟定義實作
    └── TennisScoring.AcceptanceTests.csproj
```

**Structure Decision**:
選擇 **Option 1: Single project** 結構，因為此功能為獨立類別庫，無需 Web 或 Mobile 複雜度。

- **src/TennisScoring.Core/**: 遵循 Clean Architecture 原則，將領域邏輯（Game, Side）和服務邏輯（ScoreFormatter）分離於 Domain 和 Services 資料夾
- **tests/TennisScoring.UnitTests/**: 針對每個核心類別的單元測試，使用 xUnit + FluentAssertions
- **tests/TennisScoring.AcceptanceTests/**: 使用 SpecFlow (Gherkin) 實現 BDD 驗收測試，對應規格中的 26 個驗收情境
- 專案命名遵循 .NET 慣例（PascalCase），檔案名稱與類別名稱一致

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | 無憲章違規 | N/A |

---

## Phase 1 設計完成總結

### 已產生的設計文件

- ✅ **research.md**: 技術決策與最佳實踐研究（.NET 8, xUnit, SpecFlow, Clean Architecture Lite）
- ✅ **data-model.md**: 領域實體設計（Game, Side, ScoreFormatter）及狀態轉換邏輯
- ✅ **contracts/**: API 契約定義（IGame, IScoreFormatter, Side）
- ✅ **quickstart.md**: 開發環境設定與 TDD/BDD 工作流程指南

### Constitution Check 再評估

**Phase 1 設計後重新評估結果**:

#### I. Single Responsibility Principle (SRP)
- ✅ **仍符合**: 設計文件確認三個核心類別職責分離
  - `Game`: 狀態管理（分數追蹤、得分記錄、獲勝判定）
  - `ScoreFormatter`: 文字轉換（數字分數 → 網球術語）
  - `Side`: 球員識別（列舉型別）

#### II. Open/Closed & Liskov Substitution Principles (OCP + LSP)
- ✅ **仍符合**: contracts/ 定義 IGame 和 IScoreFormatter 介面
  - 未來可新增 TieBreakGame 實作 IGame，無需修改現有程式碼
  - 未來可新增 ChineseScoreFormatter 實作 IScoreFormatter，支援多語系

#### III. Interface Segregation & Dependency Inversion Principles (ISP + DIP)
- ✅ **仍符合**: 介面設計遵循 ISP（小而專注）
  - IGame 專注於計分狀態管理（5 個屬性 + 3 個方法）
  - IScoreFormatter 專注於格式化（1 個方法）
  - Game 可依賴 IScoreFormatter 介面，支援 DI 注入

#### IV. Testability & Atomic Commits
- ✅ **仍符合**: quickstart.md 詳細說明 TDD/BDD 流程
  - 紅綠重構循環範例
  - SpecFlow Feature 檔案結構
  - 步驟定義實作指南
  - 原子性 commit 將在 tasks.md 執行時強制執行

#### V. Explicit Configuration & Observability
- ✅ **仍符合**: 類別庫無需外部配置，quickstart.md 包含 .csproj 配置範例
  - TreatWarningsAsErrors、AnalysisMode 設定
  - Nullable Reference Types 啟用
  - 中央套件管理（Directory.Packages.props）

#### VI. Traditional Chinese Documentation
- ✅ **仍符合**: 所有設計文件使用正體中文
  - research.md: 完整正體中文技術決策說明
  - data-model.md: 正體中文實體描述與註解
  - contracts/README.md: 正體中文契約說明
  - quickstart.md: 正體中文開發指南
  - 程式碼範例遵循英文命名規範（如 Game.PointWonBy）

### .NET Core Standards 符合性
- ✅ **仍符合**: quickstart.md 包含完整專案配置
  - .NET 8 (LTS) 設定
  - C# 12 語言版本
  - Nullable Reference Types 啟用範例
  - 中央套件管理設定
  - Clean Architecture 資料夾結構

### 最終 Gate 評估
- **GATE STATUS**: ✅ **PASS** - Phase 1 設計完全符合專案憲章，無違規項目

---

## 下一步驟

執行 `/speckit.tasks` 命令以產生實作任務清單（tasks.md），將設計轉換為可執行的開發任務。
