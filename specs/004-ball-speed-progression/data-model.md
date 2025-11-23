# Data Model: 球速遞增機制

**Feature**: 004-ball-speed-progression  
**Created**: 2025-11-23

## 概述

本功能不涉及持久化資料模型或資料庫結構，所有狀態均為運行時記憶體狀態。以下描述涉及的實體與狀態欄位。

---

## 實體：PongEngine（遊戲引擎）

### 新增欄位

| 欄位名稱 | 類型 | 預設值 | 說明 | 可見性 |
|---------|------|--------|------|--------|
| `_speedMultiplier` | `float` | `1.0f` | 當前球速倍率，範圍 1.0 ~ 2.0 | private |

### 欄位行為

- **初始化**: 在 `PongEngine` 建構函式中初始化為 `1.0f`
- **更新時機**: 在 `HandleScore` 方法中，當 `ScoringGame.IsFinished == true` 時
- **更新邏輯**: `_speedMultiplier = Math.Min(_speedMultiplier + 0.5f, 2.0f)`
- **使用時機**: 在 `ServeBall` 方法中計算實際球速時

### 狀態轉換

```
初始狀態: _speedMultiplier = 1.0f
         ↓ (第一局結束)
第二局:   _speedMultiplier = 1.5f
         ↓ (第二局結束)
第三局:   _speedMultiplier = 2.0f
         ↓ (第三局結束)
第四局:   _speedMultiplier = 2.0f (達上限，不再增加)
```

---

## 實體：Ball（球）

### 現有欄位（無需修改）

| 欄位名稱 | 類型 | 說明 |
|---------|------|------|
| `Position` | `PointF` | 球的當前位置 |
| `Velocity` | `PointF` | 球的速度向量 |
| `Radius` | `float` | 球的半徑 |
| `Speed` | `float` | 球的速度值（會被更新） |

### 欄位行為變更

- **Speed 屬性**: 在 `ServeBall` 中會被設定為 `BallSpeed * _speedMultiplier`
- **Velocity 屬性**: 方向向量會被標準化並乘以新的 speed 值

---

## 實體：Game（計分系統）

### 現有欄位（無需修改）

| 欄位名稱 | 類型 | 說明 |
|---------|------|------|
| `IsFinished` | `bool` | 標示一局是否結束 |
| `Winner` | `Side?` | 贏家方（如果已結束） |

### 相依關係

- `PongEngine._speedMultiplier` 的更新依賴 `Game.IsFinished` 的狀態

---

## 資料流程圖

```
                  HandleScore(winner)
                         │
                         ├─→ ScoringGame.PointWonBy(winner)
                         │
                         ├─→ ScoreChanged event ↑
                         │
                         ├─→ Check: ScoringGame.IsFinished?
                         │         │
                         │         ├─ Yes → Update _speedMultiplier
                         │         │         _speedMultiplier = Min(_speedMultiplier + 0.5f, 2.0f)
                         │         │         │
                         │         │         └─→ GameEnded event ↑
                         │         │
                         │         └─ No ─→ ResetBall()
                         │                    │
                         │                    └─→ Place ball, Velocity = Empty
                         │
                         └─→ Next serve: ServeBall()
                                  │
                                  ├─→ Calculate speed = BallSpeed * _speedMultiplier
                                  │
                                  ├─→ Set Ball.Speed = speed
                                  │
                                  └─→ Set Ball.Velocity = normalized_direction * speed
```

---

## 驗證規則

| 規則 | 檢查點 | 錯誤處理 |
|------|--------|----------|
| 倍率範圍 1.0 ~ 2.0 | `HandleScore` 更新時 | 使用 `Math.Min(value, 2.0f)` 確保上限 |
| 倍率遞增固定為 0.5 | `HandleScore` 更新時 | 硬編碼 `+ 0.5f` |
| 僅在遊戲結束時更新 | `HandleScore` | 檢查 `ScoringGame.IsFinished` |
| Ball.Speed 與倍率一致 | `ServeBall` | 同步設定 `Ball.Speed = BallSpeed * _speedMultiplier` |

---

## 無需持久化

本功能所有狀態均為運行時狀態，遊戲關閉後不保留。每次啟動新遊戲時，`_speedMultiplier` 會重置為 `1.0f`。

---

## 與現有架構的整合

- **不修改公開介面**: `IPongGameEngine` 保持不變
- **不新增公開屬性**: `_speedMultiplier` 為私有欄位
- **不影響 UI 層**: `GameForm` 無需任何修改
- **不影響 Entities**: `Ball`, `Paddle`, `Player` 類別保持不變
- **僅擴充內部邏輯**: 在 `PongEngine` 內部封裝所有球速管理邏輯
