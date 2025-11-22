---

description: "Task list for feature 002-console-tennis-game"
---

# Tasks: 互動式網球主控台

**Input**: `/specs/002-console-tennis-game/plan.md`, `/specs/002-console-tennis-game/spec.md`
**Prerequisites**: `plan.md`、`spec.md`

## Phase 1: Setup (共同基礎)

**Purpose**: 建立新主控台專案與對應測試專案

- [X] T001 在 `src/TennisScoring.Console` 建立 .NET 8 主控台專案 `TennisScoring.Console.csproj`
- [X] T002 將 `src/TennisScoring.Console/TennisScoring.Console.csproj` 加入 `TennisScoring.sln`
- [X] T003 在 `src/TennisScoring.Console/TennisScoring.Console.csproj` 新增對 `src/TennisScoring/TennisScoring.csproj` 的專案參考
- [X] T004 更新 `src/TennisScoring.Console/TennisScoring.Console.csproj` 使 `<Nullable>` 與 `<TreatWarningsAsErrors>` 啟用
- [X] T005 在 `tests/TennisScoring.Console.Tests` 建立 xUnit 專案 `TennisScoring.Console.Tests.csproj`
- [X] T006 將 `tests/TennisScoring.Console.Tests/TennisScoring.Console.Tests.csproj` 加入 `TennisScoring.sln` 並參考 `src/TennisScoring.Console`

---

## Phase 2: Foundational (阻斷性前置作業)

**Purpose**: 建立可測且具測試替身的核心骨架

- [X] T007 在 `src/TennisScoring.Console/Program.cs` 建立入口骨架並啟動 `MatchController`
- [X] T008 在 `src/TennisScoring.Console/Abstractions/IConsoleAdapter.cs` 定義主控台介面以利測試替換
- [X] T009 在 `src/TennisScoring.Console/Console/SystemConsoleAdapter.cs` 實作 `IConsoleAdapter` 封裝 `System.Console`
- [X] T010 在 `src/TennisScoring.Console/ConsoleUI.cs` 建立類別骨架並注入 `IConsoleAdapter`
- [X] T011 在 `src/TennisScoring.Console/InputValidator.cs` 建立輸入驗證骨架與方法簽章
- [X] T012 在 `src/TennisScoring.Console/MatchController.cs` 建立比賽流程骨架（含建構函式與 `RunAsync` 雛形）
- [X] T013 [P] 在 `tests/TennisScoring.Console.Tests/Console/FakeConsoleAdapter.cs` 建立可注入的主控台測試替身

---

## Phase 3: User Story 1 - 開始新的網球對局 (Priority: P1)

**Goal**: 讓使用者輸入兩位球員姓名並顯示開局比分

**Independent Test**: 從命令列啟動程式，輸入兩個有效姓名後應顯示 `Love-All`

### 實作任務

- [X] T014 [P] [US1] 在 `tests/TennisScoring.Console.Tests/MatchControllerStartTests.cs` 撰寫測試涵蓋輸入姓名與初始比分
- [X] T015 [US1] 在 `src/TennisScoring.Console/InputValidator.cs` 實作姓名驗證（非空白、去除前後空白）
- [X] T016 [US1] 在 `src/TennisScoring.Console/ConsoleUI.cs` 實作雙方球員姓名提示與錯誤訊息顯示
- [X] T017 [US1] 在 `src/TennisScoring.Console/MatchController.cs` 實作初始化流程並於啟動時顯示 `Game.GetScoreText()`

**Checkpoint**: 完成後可獨立啟動比賽並確認初始比分顯示

---

## Phase 4: User Story 2 - 記錄得分並顯示比分 (Priority: P1)

**Goal**: 允許使用者逐回合指定得分球員並顯示最新網球術語比分

**Independent Test**: 啟動程式後連續輸入得分方，比分需正確轉換為 Love/15/30/40、Deuce、Advantage 等術語

### 實作任務

- [ ] T018 [P] [US2] 在 `tests/TennisScoring.Console.Tests/MatchControllerScoreTests.cs` 撰寫測試涵蓋一般與 Deuce/Advantage 情境
- [ ] T019 [US2] 在 `src/TennisScoring.Console/InputValidator.cs` 實作得分選項解析與錯誤訊息
- [ ] T020 [US2] 在 `src/TennisScoring.Console/ConsoleUI.cs` 實作得分選擇提示與比分顯示函式
- [ ] T021 [US2] 在 `src/TennisScoring.Console/MatchController.cs` 實作回合迴圈並呼叫 `Game.PointWonBy`

**Checkpoint**: 完成後可連續輸入得分並正確顯示更新後的術語比分

---

## Phase 5: User Story 4 - 宣告勝者與新局選項 (Priority: P1)

**Goal**: 當某方獲勝時提示結果並允許使用者開啟新局或結束

**Independent Test**: 模擬玩家持續得分直到勝出，系統需顯示勝利訊息並提供再次開局或結束的選項

### 實作任務

- [ ] T022 [P] [US4] 在 `tests/TennisScoring.Console.Tests/MatchControllerWinTests.cs` 撰寫測試驗證勝利流程與重新開局
- [ ] T023 [US4] 在 `src/TennisScoring.Console/MatchController.cs` 實作勝利判定、結果顯示與重新初始化邏輯
- [ ] T024 [US4] 在 `src/TennisScoring.Console/ConsoleUI.cs` 實作勝利訊息與再次開局/結束選項提示

**Checkpoint**: 完成後單局結束時可明確宣告勝者並決定是否開啟新局

---

## Phase 6: User Story 3 - 隨時結束對局 (Priority: P2)

**Goal**: 允許使用者在任何提示輸入 `exit`（不分大小寫）以立即結束

**Independent Test**: 於輸入姓名或得分過程中輸入 `EXIT`，程式應即時結束並顯示告別訊息

### 實作任務

- [ ] T025 [P] [US3] 在 `tests/TennisScoring.Console.Tests/MatchControllerExitTests.cs` 撰寫測試驗證不同階段輸入 `exit`
- [ ] T026 [US3] 在 `src/TennisScoring.Console/InputValidator.cs` 擴充退出指令解析（大小寫不敏感）
- [ ] T027 [US3] 在 `src/TennisScoring.Console/MatchController.cs` 處理 `exit` 指令並於必要時終止迴圈
- [ ] T028 [US3] 在 `src/TennisScoring.Console/ConsoleUI.cs` 顯示結束訊息並安全結束流程

**Checkpoint**: 完成後使用者可於任一階段輸入 `exit` 安全離開

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: 改善使用體驗、補強邊界條件與手動測試指南

- [ ] T029 在 `src/TennisScoring.Console/ConsoleUI.cs` 強化無效輸入提示涵蓋空字串、非法符號與重複姓名
- [ ] T030 [P] 在 `tests/TennisScoring.Console.Tests/ConsoleUITests.cs` 撰寫邊界案例單元測試（空白姓名、錯誤選項）
- [ ] T031 在 `specs/002-console-tennis-game/quickstart.md` 撰寫手動測試與執行指引

---

## Dependencies & Execution Order

1. **Phase 1 → Phase 2**：必須先完成專案與測試骨架才能進行核心設計
2. **Phase 2 → Phase 3/4/5/6**：`ConsoleUI`、`MatchController` 與測試替身建立後，四個使用者故事可平行展開
3. **Phase 3 → Phase 4**：US2 依賴 US1 的球員初始化流程
4. **Phase 4 → Phase 5**：US4 需要既有的得分迴圈與比分顯示
5. **Phase 3/4 → Phase 6**：US3 的退出流程需整合姓名輸入與得分迴圈
6. **Phase 3-6 → Phase 7**：所有故事完成後再進行精修與文件整理

## Parallel Opportunities by User Story

- **US1**：`T014` 與 `T015`、`T016` 可分別由測試與驗證、UI 負責人並行處理
- **US2**：`T018` 可與 `T019`、`T020` 同步，完成後再整合 `T021`
- **US4**：`T022`（測試）與 `T024`（UI）可與 `T023`（流程）並行協作
- **US3**：`T025` 可先行撰寫測試案例，再與 `T026`、`T027`、`T028` 並行落實

## Implementation Strategy

- **MVP**：完成 Phase 1-3 後即可提供最小可行產品（輸入姓名並顯示初始比分）
- **增量交付**：依序加入 Phase 4、Phase 5 的得分與勝利流程，最後補上 Phase 6 的退出能力
- **品質收斂**：全部故事完成後進行 Phase 7，確保錯誤處理、測試覆蓋與手動驗證文件完善
