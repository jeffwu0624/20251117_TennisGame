# 資料模型：修正遊戲結束對話框中顯示錯誤的球員名稱

## 實體

### GameEndedEventArgs
*已修改*
- **Winner** (`Side`): 贏得遊戲的一方。（現有）
- **Message** (`string`): 最終比分訊息。（現有）
- **WinnerName** (`string`): 獲勝球員的顯示名稱。（新增）

### Player
*未修改*
- **Name** (`string`): 球員的顯示名稱。
- **Side** (`Side`): 球員所在的一方。
