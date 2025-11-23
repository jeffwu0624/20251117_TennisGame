# 快速入門：修正遊戲結束對話框中顯示錯誤的球員名稱

## 概述
此功能更新了遊戲結束對話框，以顯示獲勝球員的實際名稱。

## 使用方法
1. 啟動遊戲。
2. 輸入玩家 A 和玩家 B 的名稱。
3. 進行遊戲直到有一方獲勝。
4. 觀察遊戲結束對話框。它應該顯示 "Winner: [球員名稱]" 而不是 "Winner: PlayerA/B"。

## 開發
- 修改 `IPongGameEngine.cs` 中的 `GameEndedEventArgs`。
- 更新 `PongEngine.cs` 以傳遞獲勝者名稱。
- 更新 `GameForm.cs` 以使用事件參數中的獲勝者名稱。
