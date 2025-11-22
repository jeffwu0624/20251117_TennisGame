# Research: 網球單局計分系統

**Feature**: 001-tennis-scoring
**Date**: 2025-11-22
**Status**: Phase 0 Complete

## 研究目標

根據 Technical Context 中的技術選擇，研究最佳實踐和實作策略，以確保系統設計符合專案憲章和 .NET Core 標準。

## 技術決策

### 1. .NET 版本選擇

**Decision**: 使用 .NET 8 (LTS)

**Rationale**:
- .NET 10 尚未發布（截至 2025-11-22，最新版本為 .NET 9，LTS 版本為 .NET 8）
- .NET 8 是目前最新的長期支援版本（LTS），提供至 2026-11 的支援
- 符合專案憲章要求使用最新 LTS 版本作為備選方案
- 提供 C# 12 完整支援及 Nullable Reference Types

**Alternatives Considered**:
- .NET 9（最新版本）：非 LTS，穩定性較低，不適合生產環境
- .NET 6（舊 LTS）：雖穩定但缺少 C# 12 新功能

### 2. 測試框架組合

**Decision**: xUnit + SpecFlow + FluentAssertions

**Rationale**:
- **xUnit**: .NET 社群最廣泛使用的單元測試框架，與 .NET Core 整合良好
- **SpecFlow**: .NET 平台最成熟的 BDD 框架，支援 Gherkin 語法，符合規格要求
- **FluentAssertions**: 提供可讀性高的斷言語法（如 `result.Should().Be("Love-All")`），降低測試維護成本
- 三者組合符合憲章 Principle IV (Testability) 要求

**Alternatives Considered**:
- NUnit + SpecFlow：NUnit 較舊，社群活躍度不如 xUnit
- MSTest：功能較基礎，擴充性較差

### 3. 專案結構設計模式

**Decision**: 簡化版 Clean Architecture (Domain-Driven Design Lite)

**Rationale**:
- **Domain 層**: 純粹的業務邏輯（Game, Side），無外部依賴，易於測試
- **Services 層**: 輔助邏輯（ScoreFormatter），負責將領域狀態轉換為外部表示
- 符合 SRP 原則：Game 負責狀態管理，ScoreFormatter 負責文字轉換
- 未來擴充 Set/Match 時，可在不修改 Game 的情況下新增類別（符合 OCP）

**Alternatives Considered**:
- 單一類別包含所有邏輯：違反 SRP，測試困難
- 完整 Clean Architecture（加入 Repository, UseCase 層）：過度工程，當前無外部依賴需求

### 4. 計分狀態建模策略

**Decision**: 整數計分 + 狀態計算方法

**Rationale**:
- 使用兩個整數（playerAScore, playerBScore）追蹤原始得分
- 透過計算方法（GetScoreText）動態判斷狀態（Love-All, Deuce, Adv, Win）
- 優點：
  - 簡單直觀，易於理解和測試
  - 記憶體占用極低（2 個 int + 枚舉）
  - 符合效能要求（<10ms）
- 避免使用 State Pattern（狀態模式）的複雜性，因當前狀態轉換邏輯簡單

**Alternatives Considered**:
- State Pattern（狀態設計模式）：過度設計，增加類別數量和複雜度
- 預先計算並儲存文字狀態：每次得分需更新文字，違反 DRY 原則

### 5. 例外處理策略

**Decision**: 比賽結束後拋出 InvalidOperationException

**Rationale**:
- 符合規格 FR-010 要求：「比賽結束後嘗試記錄得分應拋出 InvalidOperationException」
- .NET 標準例外類型，語意明確（無效操作狀態）
- 強制呼叫端檢查 Game 狀態，防止進入無效狀態
- 符合 Fail-Fast 原則

**Alternatives Considered**:
- 自訂例外（GameFinishedException）：增加複雜度，標準例外已足夠
- 靜默忽略得分：違反規格，且隱藏邏輯錯誤

### 6. 測試覆蓋策略

**Decision**: 單元測試（xUnit）+ BDD 驗收測試（SpecFlow）雙層測試

**Rationale**:
- **單元測試**: 針對每個方法的邊界條件和異常情況（如超過 10 分的 Deuce 循環）
- **BDD 驗收測試**: 對應規格中 26 個驗收情境，確保需求 100% 覆蓋
- 目標覆蓋率 >80%，實際預期可達 95%+（因邏輯簡單且完全可測試）
- 測試金字塔：大量單元測試 + 完整驗收測試

**Alternatives Considered**:
- 僅單元測試：缺少與需求的直接對應
- 僅 BDD 測試：執行速度慢，無法覆蓋所有邊界條件

## 最佳實踐研究

### .NET Core 專案配置

**Nullable Reference Types**:
```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <AnalysisMode>All</AnalysisMode>
</PropertyGroup>
```

**中央套件管理** (Directory.Packages.props):
```xml
<Project>
  <ItemGroup>
    <PackageVersion Include="xunit" Version="2.6.0" />
    <PackageVersion Include="SpecFlow.xUnit" Version="4.0.0" />
    <PackageVersion Include="FluentAssertions" Version="6.12.0" />
  </ItemGroup>
</Project>
```

### SpecFlow Feature 檔案結構

根據規格中的 Gherkin 格式，建議結構如下：

```gherkin
# BasicScoring.feature
Feature: 基本計分顯示（0-3分）
  作為一位網球比賽計分員
  我需要系統能夠正確顯示雙方球員在 0 到 3 分範圍內的比分文字描述
  以便我能快速了解當前局面

  Scenario: 0-0 顯示 Love-All
    Given 一局新的網球比賽開始
    When A 球員得 0 分且 B 球員得 0 分
    Then 顯示的比分文字應該是 "Love-All"

  # ... 其他情境
```

### 效能考量

**計分計算複雜度**: O(1)
- 所有判斷皆為簡單整數比較（if-else 或 switch）
- 預期執行時間 <1ms（遠低於 10ms 要求）

**記憶體占用**: ~48 bytes per Game instance
- 2 個 int (8 bytes)
- 1 個 Side? enum for winner (4 bytes)
- 物件 overhead (16-24 bytes)
- 遠低於 <1MB 限制

## 潛在風險與緩解策略

### 風險 1: SpecFlow 設定複雜度

**緩解策略**:
- 使用 SpecFlow Visual Studio 擴充套件簡化設定
- 參考官方文檔 (https://docs.specflow.org/projects/getting-started)
- 在 quickstart.md 提供詳細設定步驟

### 風險 2: 未來擴充至 Set/Match 時的重構成本

**緩解策略**:
- 現階段設計介面 `IGame`，為未來多型設計預留空間
- 確保 Game 類別僅負責單一局邏輯，符合 SRP
- Set/Match 可透過組合（Composition）而非繼承實現

### 風險 3: 測試案例維護成本

**緩解策略**:
- 使用 SpecFlow 的 Scenario Outline 減少重複
- 集中管理測試資料於 Examples 表格
- 保持步驟定義（StepDefinitions）簡潔可重用

## 研究結論

所有技術選擇已明確，無 NEEDS CLARIFICATION 項目：

- ✅ **Language/Version**: C# 12 / .NET 8 (LTS)
- ✅ **Primary Dependencies**: xUnit 2.6.0, SpecFlow 4.0, FluentAssertions 6.12
- ✅ **Testing**: xUnit (單元) + SpecFlow (BDD) 雙層測試
- ✅ **Project Type**: 類別庫專案（Clean Architecture Lite）
- ✅ **Performance**: 計分計算 O(1)，<1ms 執行時間
- ✅ **Constraints**: ~48 bytes per instance，無 I/O 操作

可繼續進行 Phase 1 設計階段（data-model.md, contracts/, quickstart.md）。
