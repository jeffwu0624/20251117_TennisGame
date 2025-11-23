# Tasks: 球速遞增機制

**Input**: Design documents from `/specs/004-ball-speed-progression/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: 專案初始化與測試環境準備

- [x] T001 建立測試檔案 tests/TennisScoring.WinForms.Tests/PongEngineScoringTests.cs

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: 核心基礎建設 (本功能無特殊基礎建設需求)

- [ ] (無基礎建設任務)

## Phase 3: User Story 1 - 基本球速遞增 (Priority: P1) 🎯 MVP

**Goal**: 實作每局結束後球速增加 0.5 倍的機制

**Independent Test**: 執行 PongEngineScoringTests，驗證第一局結束後 Ball.Speed 變為 1.5 倍

### Implementation for User Story 1

- [x] T002 [US1] 實作 ServeBall 支援速度倍率計算並更新 Ball.Speed 屬性 src/TennisScoring.WinForms/Engine/PongEngine.cs
- [x] T003 [US1] 實作 HandleScore 遊戲結束時的倍率遞增邏輯 src/TennisScoring.WinForms/Engine/PongEngine.cs

## Phase 4: User Story 2 - 球速上限限制 (Priority: P1)

**Goal**: 限制球速最高為初始速度的 2.0 倍

**Independent Test**: 執行 PongEngineScoringTests，驗證第三局後球速維持 2.0 倍

### Implementation for User Story 2

- [x] T004 [US2] 實作倍率上限保護邏輯 (Max 2.0) src/TennisScoring.WinForms/Engine/PongEngine.cs

## Phase 5: User Story 3 - 視覺回饋與玩家感知 (Priority: P2)

**Goal**: 確保物理引擎在高速度下運作正常

**Independent Test**: 執行 PongEnginePhysicsTests，驗證高速度下的碰撞行為

### Implementation for User Story 3

- [ ] T005 [US3] 新增高速度下的物理碰撞測試案例 tests/TennisScoring.WinForms.Tests/PongEnginePhysicsTests.cs

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: 程式碼清理與文件完善

- [ ] T006 檢查並補全正體中文註解 src/TennisScoring.WinForms/Engine/PongEngine.cs
- [ ] T007 執行完整測試套件確保無回歸錯誤 tests/TennisScoring.WinForms.Tests/

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: 無相依性
- **User Story 1 (Phase 3)**: 依賴 Setup
- **User Story 2 (Phase 4)**: 依賴 User Story 1
- **User Story 3 (Phase 5)**: 依賴 User Story 2 (建議順序，亦可平行)

### User Story Dependencies

- **User Story 1**: 必須最先實作，建立基礎倍率機制
- **User Story 2**: 依賴 US1 的倍率機制進行擴充
- **User Story 3**: 驗證性質，可隨時進行

## Parallel Example: User Story 3

```bash
# 當 US1 實作完成後，US2 與 US3 可由不同開發者平行進行
Task: "實作倍率上限保護邏輯 (Max 2.0) src/TennisScoring.WinForms/Engine/PongEngine.cs"
Task: "新增高速度下的物理碰撞測試案例 tests/TennisScoring.WinForms.Tests/PongEnginePhysicsTests.cs"
```

## Implementation Strategy

### MVP First (User Story 1)

1. 完成 Setup
2. 實作 US1 (基本遞增)
3. 驗證遊戲體驗

### Incremental Delivery

1. 加入 US2 (上限保護)
2. 加入 US3 (物理驗證)
3. 最終 Polish
