# Quickstart Guide: 網球單局計分系統

**Feature**: 001-tennis-scoring
**Date**: 2025-11-22

## 前置準備

### 開發環境需求

- **.NET SDK**: 8.0 或以上（LTS 版本）
  - 下載連結: https://dotnet.microsoft.com/download/dotnet/8.0
  - 驗證安裝: `dotnet --version`（應顯示 8.x.x）

- **IDE**（擇一）:
  - Visual Studio 2022 17.8+ (推薦 Community Edition 免費版)
  - JetBrains Rider 2023.3+
  - Visual Studio Code + C# Dev Kit 擴充套件

- **Git**: 版本控制（clone 專案用）
  - 下載連結: https://git-scm.com/

### 驗證環境

```powershell
# 檢查 .NET SDK 版本
dotnet --version

# 檢查 C# 編譯器支援
dotnet new console -n TestCSharp
cd TestCSharp
dotnet build
cd ..
Remove-Item -Recurse -Force TestCSharp
```

---

## 專案建立步驟

### 步驟 1: 建立 Solution 和專案結構

```powershell
# 1. 建立專案根目錄
mkdir TennisScoring
cd TennisScoring

# 2. 建立 Solution 檔案
dotnet new sln -n TennisScoring.Core

# 3. 建立核心類別庫專案
mkdir -p src/TennisScoring.Core
dotnet new classlib -n TennisScoring.Core -o src/TennisScoring.Core -f net8.0

# 4. 建立單元測試專案
mkdir -p tests/TennisScoring.UnitTests
dotnet new xunit -n TennisScoring.UnitTests -o tests/TennisScoring.UnitTests -f net8.0

# 5. 建立 BDD 驗收測試專案
mkdir -p tests/TennisScoring.AcceptanceTests
dotnet new xunit -n TennisScoring.AcceptanceTests -o tests/TennisScoring.AcceptanceTests -f net8.0

# 6. 將專案加入 Solution
dotnet sln add src/TennisScoring.Core/TennisScoring.Core.csproj
dotnet sln add tests/TennisScoring.UnitTests/TennisScoring.UnitTests.csproj
dotnet sln add tests/TennisScoring.AcceptanceTests/TennisScoring.AcceptanceTests.csproj

# 7. 加入專案參考
dotnet add tests/TennisScoring.UnitTests/TennisScoring.UnitTests.csproj reference src/TennisScoring.Core/TennisScoring.Core.csproj
dotnet add tests/TennisScoring.AcceptanceTests/TennisScoring.AcceptanceTests.csproj reference src/TennisScoring.Core/TennisScoring.Core.csproj
```

### 步驟 2: 安裝必要 NuGet 套件

#### 2.1 建立中央套件管理檔案

在專案根目錄建立 `Directory.Packages.props`:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <!-- 測試框架 -->
    <PackageVersion Include="xunit" Version="2.6.0" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="2.5.4" />
    <PackageVersion Include="FluentAssertions" Version="6.12.0" />

    <!-- BDD 框架 -->
    <PackageVersion Include="SpecFlow.xUnit" Version="4.0.31-beta" />
    <PackageVersion Include="SpecFlow.Tools.MsBuild.Generation" Version="4.0.31-beta" />

    <!-- 程式碼覆蓋率 -->
    <PackageVersion Include="coverlet.collector" Version="6.0.0" />

    <!-- 測試主機 -->
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  </ItemGroup>
</Project>
```

#### 2.2 修改測試專案檔案

**tests/TennisScoring.UnitTests/TennisScoring.UnitTests.csproj**:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>false</IsPackable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisMode>All</AnalysisMode>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" />
    <PackageReference Include="xunit.runner.visualstudio" />
    <PackageReference Include="FluentAssertions" />
    <PackageReference Include="coverlet.collector" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\TennisScoring.Core\TennisScoring.Core.csproj" />
  </ItemGroup>
</Project>
```

**tests/TennisScoring.AcceptanceTests/TennisScoring.AcceptanceTests.csproj**:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>false</IsPackable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" />
    <PackageReference Include="xunit.runner.visualstudio" />
    <PackageReference Include="FluentAssertions" />
    <PackageReference Include="SpecFlow.xUnit" />
    <PackageReference Include="SpecFlow.Tools.MsBuild.Generation" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\TennisScoring.Core\TennisScoring.Core.csproj" />
  </ItemGroup>
</Project>
```

#### 2.3 安裝套件

```powershell
# 在專案根目錄執行
dotnet restore
```

### 步驟 3: 設定核心類別庫專案

修改 `src/TennisScoring.Core/TennisScoring.Core.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisMode>All</AnalysisMode>
    <LangVersion>12</LangVersion>
  </PropertyGroup>
</Project>
```

### 步驟 4: 建立資料夾結構

```powershell
# 核心專案資料夾
mkdir -p src/TennisScoring.Core/Domain
mkdir -p src/TennisScoring.Core/Services

# 單元測試資料夾
mkdir -p tests/TennisScoring.UnitTests/Domain
mkdir -p tests/TennisScoring.UnitTests/Services

# BDD 測試資料夾
mkdir -p tests/TennisScoring.AcceptanceTests/Features
mkdir -p tests/TennisScoring.AcceptanceTests/StepDefinitions

# 刪除預設產生的 Class1.cs 和 UnitTest1.cs
Remove-Item src/TennisScoring.Core/Class1.cs -ErrorAction SilentlyContinue
Remove-Item tests/TennisScoring.UnitTests/UnitTest1.cs -ErrorAction SilentlyContinue
Remove-Item tests/TennisScoring.AcceptanceTests/UnitTest1.cs -ErrorAction SilentlyContinue
```

---

## 快速驗證

### 驗證建置

```powershell
# 建置整個 Solution
dotnet build

# 預期輸出：Build succeeded. 0 Warning(s), 0 Error(s)
```

### 執行測試（初始狀態）

```powershell
# 執行所有測試（目前無測試檔案，應顯示 0 tests）
dotnet test

# 預期輸出：
# Total tests: 0
# Passed: 0
```

---

## 開發工作流程

### TDD 紅綠重構循環

#### 1. 紅（Red）：撰寫失敗測試

在 `tests/TennisScoring.UnitTests/Domain/GameTests.cs` 建立測試：

```csharp
using FluentAssertions;
using TennisScoring.Core.Domain;
using Xunit;

namespace TennisScoring.UnitTests.Domain;

public class GameTests
{
    [Fact]
    public void NewGame_ShouldStartAtLoveAll()
    {
        // Arrange & Act
        var game = new Game();

        // Assert
        game.GetScoreText().Should().Be("Love-All");
    }
}
```

執行測試（應失敗，因為 Game 類別尚未建立）：

```powershell
dotnet test
# 預期：編譯錯誤（Game 類別不存在）
```

#### 2. 綠（Green）：撰寫最小可行實作

在 `src/TennisScoring.Core/Domain/Game.cs` 建立類別：

```csharp
namespace TennisScoring.Core.Domain;

public class Game
{
    public string GetScoreText()
    {
        return "Love-All";
    }
}
```

執行測試（應通過）：

```powershell
dotnet test
# 預期：Passed! - Total: 1, Passed: 1
```

#### 3. 重構（Refactor）：優化程式碼

持續迭代，逐步實作所有功能需求。

### BDD 驗收測試流程

#### 1. 建立 Feature 檔案

在 `tests/TennisScoring.AcceptanceTests/Features/BasicScoring.feature`:

```gherkin
# language: zh-TW
功能: 基本計分顯示（0-3分）
  作為一位網球比賽計分員
  我需要系統能夠正確顯示雙方球員在 0 到 3 分範圍內的比分文字描述
  以便我能快速了解當前局面

  場景: 0-0 顯示 Love-All
    假如 一局新的網球比賽開始
    當 A 球員得 0 分且 B 球員得 0 分
    那麼 顯示的比分文字應該是 "Love-All"

  場景: 1-0 顯示 Fifteen-Love
    假如 一局新的網球比賽開始
    當 A 球員得 1 分且 B 球員得 0 分
    那麼 顯示的比分文字應該是 "Fifteen-Love"
```

#### 2. 產生步驟定義骨架

```powershell
# 建置專案會自動產生步驟定義類別骨架
dotnet build tests/TennisScoring.AcceptanceTests
```

#### 3. 實作步驟定義

在 `tests/TennisScoring.AcceptanceTests/StepDefinitions/TennisScoringSteps.cs`:

```csharp
using FluentAssertions;
using TechTalk.SpecFlow;
using TennisScoring.Core.Domain;

namespace TennisScoring.AcceptanceTests.StepDefinitions;

[Binding]
public class TennisScoringSteps
{
    private Game _game = null!;
    private string _actualScoreText = string.Empty;

    [Given(@"一局新的網球比賽開始")]
    public void Given一局新的網球比賽開始()
    {
        _game = new Game();
    }

    [When(@"A 球員得 (.*) 分且 B 球員得 (.*) 分")]
    public void WhenA球員得分且B球員得分(int playerAScore, int playerBScore)
    {
        for (int i = 0; i < playerAScore; i++)
            _game.PointWonBy(Side.PlayerA);

        for (int i = 0; i < playerBScore; i++)
            _game.PointWonBy(Side.PlayerB);

        _actualScoreText = _game.GetScoreText();
    }

    [Then(@"顯示的比分文字應該是 ""(.*)""")]
    public void Then顯示的比分文字應該是(string expectedScore)
    {
        _actualScoreText.Should().Be(expectedScore);
    }
}
```

#### 4. 執行 BDD 測試

```powershell
dotnet test --filter "FullyQualifiedName~AcceptanceTests"
```

---

## 常見問題排除

### 問題 1: SpecFlow 未產生步驟定義檔案

**解決方案**:
```powershell
# 安裝 SpecFlow 生成工具
dotnet tool install --global SpecFlow.Tools.MsBuild.Generation

# 重新建置專案
dotnet clean
dotnet build
```

### 問題 2: Nullable Reference Types 警告

**解決方案**:
在類別屬性加上 `= null!;` 或 `?` 標示：

```csharp
// Option 1: 明確賦值（建構函式會初始化）
private Game _game = null!;

// Option 2: 宣告為可空
private Game? _game;
```

### 問題 3: 測試執行時找不到測試

**解決方案**:
```powershell
# 清除並重新建置
dotnet clean
dotnet build
dotnet test --no-build
```

---

## 推薦 IDE 設定

### Visual Studio 2022

1. 安裝擴充套件：
   - SpecFlow for Visual Studio 2022
   - Test Explorer

2. 啟用即時單元測試（Live Unit Testing）：
   - Test > Live Unit Testing > Start

### Visual Studio Code

1. 安裝擴充套件：
   - C# Dev Kit
   - .NET Core Test Explorer
   - Cucumber (Gherkin) Full Support

2. 設定 `settings.json`：

```json
{
  "dotnet.defaultSolution": "TennisScoring.Core.sln",
  "omnisharp.enableRoslynAnalyzers": true,
  "omnisharp.enableEditorConfigSupport": true
}
```

---

## 程式碼覆蓋率檢查

### 產生覆蓋率報告

```powershell
# 執行測試並收集覆蓋率
dotnet test --collect:"XPlat Code Coverage"

# 安裝報告產生工具
dotnet tool install --global dotnet-reportgenerator-globaltool

# 產生 HTML 報告
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# 開啟報告
start coveragereport/index.html
```

### 檢查覆蓋率門檻（>80%）

在 CI/CD 管線中加入檢查：

```yaml
# GitHub Actions 範例
- name: Test with Coverage
  run: dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings

- name: Check Coverage Threshold
  run: |
    dotnet tool install --global dotnet-coverage
    dotnet coverage merge "**/*.coverage" -o merged.coverage -f cobertura
    dotnet coverage check merged.coverage --threshold 80
```

---

## 下一步

1. 參考 `data-model.md` 了解領域模型設計
2. 參考 `contracts/` 查看 API 契約定義
3. 執行 `/speckit.tasks` 產生實作任務清單（tasks.md）
4. 遵循 TDD 循環實作每個任務
5. 確保每個任務對應一次 atomic commit

**祝開發順利！**
