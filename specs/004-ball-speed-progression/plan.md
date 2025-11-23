# Implementation Plan: 球速遞增機制

**Branch**: `004-ball-speed-progression` | **Date**: 2025-11-23 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/004-ball-speed-progression/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

實作球速遞增機制，使每局遊戲結束後下一局的球速自動增加 0.5 倍（即變為當前的 1.5 倍），最高上限為初始球速的 2.0 倍。技術方法為在 `PongEngine` 類別中新增速度倍率追蹤欄位，在 `HandleScore` 方法中檢測遊戲結束事件並更新倍率，在 `ServeBall` 方法中套用倍率計算實際球速。設計遵循 SRP 原則，將速度管理邏輯封裝於引擎內部，並透過 TDD 方法確保正確性。

## Technical Context

**Language/Version**: C# 12+ with .NET 10 (net10.0-windows for WinForms project)  
**Primary Dependencies**: System.Drawing (WinForms), xUnit, FluentAssertions  
**Storage**: N/A（運行時記憶體狀態，無需持久化）  
**Testing**: xUnit + FluentAssertions  
**Target Platform**: Windows Desktop (WinForms application)  
**Project Type**: Desktop game application（現有專案擴充功能）  
**Performance Goals**: 維持 60 FPS 遊戲迴圈，球速變化不影響物理運算準確性  
**Constraints**: 
- 球速倍率範圍：1.0x ~ 2.0x
- 每次增量固定為 0.5x
- 物理引擎必須在所有速度倍率下保持碰撞檢測準確性
- 速度變化僅在一局完全結束（`ScoringGame.IsFinished == true`）時觸發
**Scale/Scope**: 單一 WinForms 專案修改，影響範圍：`PongEngine` 類別及其單元測試

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### SOLID Principles 合規性檢查

- ✅ **SRP (Single Responsibility)**: `PongEngine` 負責遊戲引擎邏輯，速度倍率管理屬於遊戲狀態管理的一部分，不違反 SRP
- ✅ **OCP/LSP (Open/Closed & Liskov Substitution)**: 透過修改現有 `PongEngine` 內部邏輯，不修改公開介面 `IPongGameEngine`，符合封閉修改原則
- ✅ **ISP/DIP (Interface Segregation & Dependency Inversion)**: 不新增介面方法，現有依賴注入結構不變

### TDD & Atomic Commits 檢查

- ✅ **Test-First**: 每個任務必須先寫失敗測試，再實作程式碼（Red-Green-Refactor）
- ✅ **Atomic Commits**: 每個 task 對應一個原子 commit，包含測試與實作
- ✅ **Coverage**: 目標 >80% 程式碼覆蓋率

### 正體中文文件規範檢查

- ✅ **Spec/Plan/Tasks**: 使用正體中文撰寫
- ✅ **Code Comments**: 複雜邏輯使用正體中文註解
- ✅ **Commit Messages**: 使用正體中文（技術術語可用英文）
- ✅ **Source Code**: 類別、方法、變數名稱使用英文（符合 .NET 規範）

### 結論

**GATE STATUS**: ✅ PASS - 無 Constitution 違規，可進行 Phase 0 研究

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
├── TennisScoring/                  # Core scoring logic (不修改)
├── TennisScoring.Console/          # Console UI (不修改)
└── TennisScoring.WinForms/         # WinForms Pong Game (本功能修改範圍)
    ├── Engine/
    │   ├── PongEngine.cs           # ✏️ 修改：新增速度倍率欄位與邏輯
    │   └── IPongGameEngine.cs      # ✅ 不修改：介面保持不變
    ├── Entities/
    │   └── Ball.cs                 # ✅ 不修改：Ball.Speed 屬性已存在
    └── Forms/
        └── GameForm.cs             # ✅ 不修改：UI 層不受影響

tests/
├── TennisScoring.Tests/            # Core tests (不修改)
├── TennisScoring.Console.Tests/    # Console tests (不修改)
└── TennisScoring.WinForms.Tests/   # Pong Engine tests (本功能新增測試)
    ├── PongEngineTests.cs          # ✅ 現有測試保持
    ├── PongEngineScoringTests.cs   # ✏️ 新增：球速遞增測試
    └── PongEnginePhysicsTests.cs   # ✅ 現有測試保持
```

**Structure Decision**: 

本功能為現有 WinForms Pong 專案的擴充，採用現有專案結構。主要修改集中在 `TennisScoring.WinForms` 專案的 `PongEngine.cs`，不影響其他專案或公開介面。測試新增於 `TennisScoring.WinForms.Tests` 專案中，遵循現有測試組織模式（按功能分類：Scoring、Physics）。

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

無 Constitution 違規事項。本功能完全符合 SOLID 原則、TDD 工作流程與正體中文文件規範。

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | N/A | N/A |

---

## Phase 0: Research (完成 ✅)

研究文件已完成，所有技術決策已明確：
- ✅ 速度倍率儲存於 `PongEngine._speedMultiplier` 私有欄位
- ✅ 倍率更新於 `HandleScore` 方法中偵測遊戲結束時
- ✅ 速度計算於 `ServeBall` 方法中套用倍率
- ✅ 測試策略於 `PongEngineScoringTests` 中新增專門測試
- ✅ 無 [NEEDS CLARIFICATION] 標記

**文件**: [research.md](./research.md)

---

## Phase 1: Design (完成 ✅)

設計文件已完成，包含：
- ✅ **資料模型**: `_speedMultiplier` 欄位定義與狀態轉換圖
- ✅ **API 契約**: 內部方法修改說明（不影響公開介面）
- ✅ **快速入門**: TDD 實作流程與測試指令

**文件**: 
- [data-model.md](./data-model.md)
- [contracts/PongEngine-internal.md](./contracts/PongEngine-internal.md)
- [quickstart.md](./quickstart.md)

---

## Phase 2: Tasks (待執行)

使用 `/speckit.tasks` 命令產生詳細任務清單與實作順序。

**預期任務數量**: 5-7 個原子任務

**預估工作量**: 1-2 小時（包含測試撰寫）

---

## 結論

技術規劃已完成，所有前置研究與設計文件就緒。可進入 Phase 2 產生任務清單並開始實作。
