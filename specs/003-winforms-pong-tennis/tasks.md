# Tasks: Windows Forms Tennis Game (Pong)

**Feature Branch**: `003-winforms-pong-tennis`
**Spec**: [spec.md](spec.md)
**Plan**: [plan.md](plan.md)

## Phase 1: Setup (設定)
*Goal: 初始化專案結構與相依性*

- [x] T001 在 `src/` 建立 `TennisScoring.WinForms` 專案
- [x] T002 在 `TennisScoring.WinForms` 中加入對 `TennisScoring` 的專案參考
- [x] T003 在 `tests/` 建立 `TennisScoring.WinForms.Tests` xUnit 測試專案
- [x] T004 在 `TennisScoring.WinForms.Tests` 中加入對 `TennisScoring.WinForms` 的專案參考

## Phase 2: Foundational (基礎建設)
*Goal: 實作資料模型與合約中定義的核心實體與介面*

- [x] T005 [P] 在 `src/TennisScoring.WinForms/Engine/IPongGameEngine.cs` 定義 `IPongGameEngine`, `InputState`, `GameState`
- [x] T006 [P] 在 `src/TennisScoring.WinForms/Entities/Paddle.cs` 實作 `Paddle` 實體
- [x] T007 [P] 在 `src/TennisScoring.WinForms/Entities/Ball.cs` 實作 `Ball` 實體
- [x] T008 [P] 在 `src/TennisScoring.WinForms/Entities/Player.cs` 實作 `Player` 實體

## Phase 3: User Story 1 - Initialization (遊戲初始化)
*Goal: 遊戲開始時輸入玩家姓名並隨機決定發球方*

- [x] T009 [US1] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 建立實作 `IPongGameEngine` 的 `PongEngine` 類別骨架
- [x] T010 [US1] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作 `PongEngine` 初始化邏輯 (玩家、隨機發球方、重置狀態)
- [x] T011 [US1] 在 `tests/TennisScoring.WinForms.Tests/PongEngineTests.cs` 建立 `PongEngine` 初始化的單元測試
- [x] T012 [US1] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 建立 `GameForm` 並啟用 DoubleBuffered
- [x] T013 [US1] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 實作簡易姓名輸入機制 (例如 InputBox 或初始表單狀態)
- [x] T014 [US1] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 連接 `GameForm` 以使用輸入的姓名初始化 `PongEngine`

## Phase 4: User Story 2 - Gameplay & Control (遊戲進行與控制)
*Goal: 球拍移動、球移動、碰撞處理*

- [x] T015 [US2] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作 `HandleInput` 方法 (處理 Q, A, P, L, Space)
- [x] T016 [US2] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作 `Update` 方法 (物理移動、碰撞偵測、計分)
- [x] T017 [US2] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作球拍與球的碰撞偵測及反彈邏輯
- [x] T018 [US2] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作發球邏輯 (空白鍵發射球)
- [x] T019 [US2] 在 `tests/TennisScoring.WinForms.Tests/PongEnginePhysicsTests.cs` 加入移動與碰撞邏輯的單元測試
- [x] T020 [US2] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 實作 `GameForm` 輸入處理 (`KeyDown`/`KeyUp`) 並對應至 `InputState`
- [x] T021 [US2] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 實作 `GameForm.OnPaint` 以繪製 `GameState` (球拍、球)
- [x] T022 [US2] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 使用 `System.Windows.Forms.Timer` 實作遊戲迴圈

## Phase 5: User Story 3 - Scoring & Win (計分與勝負)
*Goal: 整合 TennisScoring、顯示分數、處理獲勝條件*

- [x] T023 [US3] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 將 `TennisScoring.Game` 整合至 `PongEngine` 並處理 "球出界" 事件
- [x] T024 [US3] 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 實作 `ScoreChanged` 與 `GameEnded` 事件觸發
- [ ] T025 [US3] 在 `tests/TennisScoring.WinForms.Tests/PongEngineScoringTests.cs` 加入計分整合的單元測試
- [x] T026 [US3] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 更新 `GameForm` 以顯示 `GameState` 中的分數文字
- [ ] T027 [US3] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 實作 `GameEnded` 事件時顯示遊戲結束並停止迴圈
- [x] T028 [US3] 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 實作 Esc 鍵關閉應用程式

## Phase 6: Polish (修飾)
*Goal: 視覺改進與參數調整*

- [ ] T029 在 `src/TennisScoring.WinForms/Engine/PongEngine.cs` 調整物理常數 (速度、球拍大小) 以改善遊戲手感
- [ ] T030 在 `src/TennisScoring.WinForms/Forms/GameForm.cs` 改進繪圖 (顏色、字型、中線)

## Dependencies

1.  **Setup** (T001-T004) -> **Foundational** (T005-T008)
2.  **Foundational** -> **US1** (T009-T014)
3.  **US1** -> **US2** (T015-T022)
4.  **US2** -> **US3** (T023-T028)
5.  **US3** -> **Polish** (T029-T030)

## Parallel Execution Examples

*   **Phase 2**: T006 (Paddle), T007 (Ball), T008 (Player) 可平行實作。
*   **Phase 4**: T019 (測試) 可在 T016/T017 (邏輯) 實作時同步撰寫。
*   **Phase 5**: T025 (測試) 可在 T023 (邏輯) 實作時同步撰寫。

## Implementation Strategy

*   **MVP Scope**: 完成至 Phase 5 (US3)。
*   **Testing**: 引擎邏輯的單元測試至關重要，因為 WinForms UI 難以自動化測試。
*   **Atomic Commits**: 每個任務代表一個 Commit。
