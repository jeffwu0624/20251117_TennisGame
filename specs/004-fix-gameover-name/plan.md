# 實作計畫：修正遊戲結束對話框中顯示錯誤的球員名稱

**分支**: `004-fix-gameover-name` | **日期**: 2025-11-23 | **規格書**: [spec.md](spec.md)
**輸入**: 來自 `/specs/004-fix-gameover-name/spec.md` 的功能規格書

**注意**: 此範本由 `/speckit.plan` 指令填寫。執行流程請參閱 `.specify/templates/commands/plan.md`。

## 摘要

目標是在遊戲結束對話框中顯示實際的球員名稱（例如 "Alice"），而不是內部的 `Side` 識別碼（例如 "PlayerA"）。這將透過擴充 `GameEndedEventArgs` 以攜帶獲勝者名稱，並更新 `PongEngine` 來填入該名稱以及 `GameForm` 來顯示它來實現。

## 技術背景

**語言/版本**: C# 12 (.NET 8)
**主要依賴**: Windows Forms
**儲存**: N/A
**測試**: xUnit
**目標平台**: Windows
**專案類型**: 桌面應用程式
**效能目標**: N/A
**限制**: N/A
**規模/範圍**: 小型修復

## 憲法檢查

*閘門：必須在階段 0 研究之前通過。在階段 1 設計後重新檢查。*

- **SRP**: 符合。引擎處理邏輯，UI 處理顯示。
- **OCP**: 符合。擴充事件參數。
- **可測試性**: 符合。
- **原子提交**: 將會遵循。
- **正體中文**: 文件使用正體中文。

## 專案結構

### 文件 (此功能)

```text
specs/004-fix-gameover-name/
├── plan.md              # 此檔案 (/speckit.plan 指令輸出)
├── research.md          # 階段 0 輸出 (/speckit.plan 指令輸出)
├── data-model.md        # 階段 1 輸出 (/speckit.plan 指令輸出)
├── quickstart.md        # 階段 1 輸出 (/speckit.plan 指令輸出)
├── contracts/           # 階段 1 輸出 (/speckit.plan 指令輸出)
│   └── IPongGameEngine.cs
└── tasks.md             # 階段 2 輸出 (/speckit.tasks 指令 - 非由 /speckit.plan 建立)
```

### 原始碼 (儲存庫根目錄)

```text
src/
├── TennisScoring.WinForms/
│   ├── Engine/
│   │   ├── IPongGameEngine.cs
│   │   └── PongEngine.cs
│   └── Forms/
│       └── GameForm.cs
```

**結構決策**: 修改單一專案結構中的現有檔案。

## 複雜度追蹤

> **僅在憲法檢查有必須合理化的違規時填寫**

| 違規 | 為何需要 | 拒絕更簡單替代方案的原因 |
|-----------|------------|-------------------------------------|
| 無 | | |
