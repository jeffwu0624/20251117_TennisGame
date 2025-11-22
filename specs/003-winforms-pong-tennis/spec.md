# Feature Specification: Windows Forms Tennis Game (Pong)

**Feature Branch**: `003-winforms-pong-tennis`  
**Created**: 2025-11-23  
**Status**: Draft  
**Input**: User description: "請詳細研讀 #file:constitution.md , 參考乓 PONG  畫面製做 windows form 的 Tennis Game, 並加入 TennisScoring 專案為計分顯示方式,同TennisScroing.Console
1.遊戲開始先輸入雙方選手姓名, 再由亂數決定誰先開球,球將在球員身上
2.第一位選手安排在左側, 第二位選手安排在右側
2.每局開始在按下空白鍵開始發球到對方
3.鍵盤的Q、J代表左側選手上、下移動, 鍵盤的上、下鍵為右側選手的上、下移動鍵
4. 選手以一個長條狀代表,球碰到長條狀不用在按任何鍵就可回擊球給對方
5.其中一方分數出現Win就算贏,或按Esc鍵結束遊戲"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - 遊戲初始化與開始 (Priority: P1)

玩家啟動遊戲，輸入雙方姓名，系統隨機決定發球方，並準備開始。

**Why this priority**: 這是遊戲的入口，確立玩家身分與初始狀態是進行遊戲的前提。

**Independent Test**: 啟動程式，輸入姓名後，確認畫面顯示正確的玩家名稱，且球位於隨機決定的發球方球拍處。

**Acceptance Scenarios**:

1. **Given** 遊戲啟動畫面, **When** 輸入 "PlayerA" 和 "PlayerB" 並確認, **Then** 進入遊戲主畫面，左側顯示 "PlayerA"，右側顯示 "PlayerB"。
2. **Given** 進入遊戲主畫面, **When** 遊戲尚未開始, **Then** 系統隨機選定一方為發球者，球顯示在該方球拍前方，並跟隨球拍移動。

---

### User Story 2 - 遊戲進行與控制 (Priority: P1)

玩家使用鍵盤控制球拍移動，發球並進行對打，球在碰到球拍時自動反彈。

**Why this priority**: 這是遊戲的核心互動機制，沒有移動與擊球就無法進行遊戲。

**Independent Test**: 驗證雙方控制鍵能正確移動對應球拍，且球能被發出並在接觸球拍時反彈。

**Acceptance Scenarios**:

1. **Given** 發球準備狀態, **When** 按下空白鍵 (Space), **Then** 球以一定速度向對方場地移動。
2. **Given** 遊戲進行中, **When** 按下 Q 鍵或 J 鍵, **Then** 左側球拍分別向上或向下移動。
3. **Given** 遊戲進行中, **When** 按下 上方向鍵 或 下方向鍵, **Then** 右側球拍分別向上或向下移動。
4. **Given** 球移動至球拍位置, **When** 球與球拍發生碰撞, **Then** 球自動反彈回對方場地，無需額外按鍵。

---

### User Story 3 - 計分與勝負判定 (Priority: P2)

根據網球規則計分，顯示當前比分，並在一方獲勝時結束遊戲。

**Why this priority**: 賦予遊戲目標與勝負機制，並整合現有的計分邏輯。

**Independent Test**: 模擬球出界導致得分，確認分數顯示符合網球術語，且達到勝利條件時遊戲結束。

**Acceptance Scenarios**:

1. **Given** 球越過球拍進入底線 (出界), **When** 一方得分, **Then** 呼叫 `TennisScoring` 更新分數，並在畫面上顯示新的網球術語比分 (如 "Fifteen-Love")。
2. **Given** 比分達到獲勝條件 (如 4分且領先2分), **When** 該分結束, **Then** 畫面顯示獲勝者訊息 (如 "PlayerA Win")。
3. **Given** 遊戲進行中, **When** 按下 Esc 鍵, **Then** 遊戲程式關閉。

### Edge Cases

- **球拍移動邊界**: 當球拍移動到視窗頂部或底部時，應停止移動，不可超出視窗範圍。
- **球的反彈角度**: 球擊中球拍不同位置時，是否應有不同的反彈角度 (可選，若未指定則依物理反射)。
- **視窗縮放**: 視窗大小改變時，遊戲元件應保持相對位置或禁止縮放。

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: 系統必須使用 Windows Forms 開發。
- **FR-002**: 系統必須引用並使用 `TennisScoring` 專案中的 `Game` 類別進行計分與勝負判斷。
- **FR-003**: 系統必須提供輸入介面讓使用者輸入雙方選手姓名。
- **FR-004**: 系統必須在每局開始前隨機決定發球方。
- **FR-005**: 發球前，球必須鎖定在發球方的球拍前方，並隨球拍移動。
- **FR-006**: 系統必須偵測空白鍵 (Space) 作為發球指令。
- **FR-007**: 左側選手 (Player 1) 的控制鍵必須為：Q (上移)、J (下移)。
- **FR-008**: 右側選手 (Player 2) 的控制鍵必須為：鍵盤上鍵 (上移)、鍵盤下鍵 (下移)。
- **FR-009**: 選手必須以長條狀 (Paddle) 圖形呈現。
- **FR-010**: 系統必須實作碰撞偵測，當球接觸球拍時自動反彈。
- **FR-011**: 當一方得分時，必須更新並顯示當前的網球比分 (使用 `Game.GetScoreText()`)。
- **FR-012**: 當比分顯示 "Win" (獲勝) 時，必須顯示獲勝訊息並停止遊戲。
- **FR-013**: 系統必須偵測 Esc 鍵，按下後結束應用程式。

### Key Entities *(include if feature involves data)*

- **Player**: 屬性包含姓名、所屬側 (左/右)、當前分數。
- **Ball**: 屬性包含 X/Y 座標、X/Y 速度向量、半徑。
- **Paddle**: 屬性包含 X/Y 座標、寬度、高度、移動速度。
- **TennisGame**: 負責協調遊戲迴圈、碰撞偵測與計分狀態。

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 遊戲能完整進行一場比賽，從輸入姓名到產生獲勝者。
- **SC-002**: 計分顯示準確無誤，與 `TennisScoring` 邏輯一致 (包含 Deuce, Advantage 狀態)。
- **SC-003**: 雙方玩家能流暢控制球拍，無明顯輸入延遲。
- **SC-004**: 碰撞判定準確，球不會穿透球拍或異常卡住。
