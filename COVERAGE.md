# テストカバレッジの取得方法

このドキュメントでは、GTodosプロジェクトのテストカバレッジを取得する方法を説明します。

## 前提条件

- .NET 9 SDK がインストールされていること
- coverlet.collector と coverlet.msbuild パッケージがインストール済み（既に設定済み）

## カバレッジの取得方法

### PowerShellスクリプトを使用する場合

#### 注意: 文字化けを防ぐために

PowerShellで日本語を正しく表示するには、以下のいずれかの方法を使用してください：

**方法1: 英語版スクリプトを使用（推奨）**
```powershell
.\Get-TestCoverage.ps1
```

**方法2: 日本語版スクリプトをUTF-8モードで実行**
```powershell
chcp 65001
.\Get-TestCoverage-JP.ps1
```

または、Windows Terminal を使用している場合は、設定で UTF-8 を有効にすることができます。

#### スクリプト一覧

| スクリプト | 言語 | 説明 |
|-----------|------|------|
| `Get-TestCoverage.ps1` | 英語 | HTMLレポート生成（推奨） |
| `Get-TestCoverage-JP.ps1` | 日本語 | HTMLレポート生成（UTF-8モード必要） |
| `Run-TestWithCoverage.ps1` | 英語 | 簡易カバレッジ取得 |
| `Run-TestWithCoverage-JP.ps1` | 日本語 | 簡易カバレッジ取得（UTF-8モード必要） |

#### HTMLレポートを自動的に開く

```powershell
.\Get-TestCoverage.ps1 -OpenReport
```

### 1. 基本的なカバレッジ取得（コンソール出力）

```powershell
dotnet test /p:CollectCoverage=true
```

### 2. カバレッジをJSON形式で出力

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=json /p:CoverletOutput=./TestResults/
```

### 3. カバレッジをHTMLレポートで出力

まず、ReportGeneratorツールをインストール：

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
```

次に、カバレッジを収集してHTMLレポートを生成：

```powershell
# カバレッジデータを収集
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/coverage.cobertura.xml

# HTMLレポートを生成
reportgenerator -reports:"./GTodosLib.Tests/TestResults/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:Html
```

HTMLレポートは `./TestResults/CoverageReport/index.html` に生成されます。

### 4. 複数の形式で出力

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"json,cobertura,lcov\" /p:CoverletOutput=./TestResults/
```

### 5. 特定のプロジェクトのみカバレッジを取得

```powershell
dotnet test GTodosLib.Tests/GTodosLib.Tests.csproj /p:CollectCoverage=true
```

## Visual Studio でのカバレッジ取得

Visual Studio Enterprise エディションを使用している場合：

1. テストエクスプローラーを開く（テスト → テストエクスプローラー）
2. 「すべてのテストをコードカバレッジで実行」をクリック
3. カバレッジ結果ウィンドウで結果を確認

## カバレッジ設定のカスタマイズ

`GTodosLib.Tests/coverlet.runsettings` ファイルで、除外ルールなどを設定できます。

使用方法：

```powershell
dotnet test --settings GTodosLib.Tests/coverlet.runsettings /p:CollectCoverage=true
```

## カバレッジ結果の見方

- **Line Coverage（行カバレッジ）**: 実行された行の割合
- **Branch Coverage（分岐カバレッジ）**: 実行された分岐の割合
- **Method Coverage（メソッドカバレッジ）**: 実行されたメソッドの割合

## 推奨されるカバレッジ目標

- 全体的なカバレッジ: 80%以上
- 重要なビジネスロジック: 90%以上
- ユーティリティクラス: 70%以上

## トラブルシューティング

### PowerShellで文字化けする場合

#### 解決方法1: 英語版スクリプトを使用
```powershell
.\Get-TestCoverage.ps1
```

#### 解決方法2: コードページをUTF-8に変更
```powershell
chcp 65001
.\Get-TestCoverage-JP.ps1
```

#### 解決方法3: Windows Terminalを使用
Windows Terminal（推奨）では、デフォルトでUTF-8がサポートされています。

### カバレッジが0%と表示される場合

プロジェクト参照が正しく設定されているか確認してください：

```powershell
dotnet list GTodosLib.Tests/GTodosLib.Tests.csproj reference
```

### ReportGeneratorが見つからない場合

グローバルツールとしてインストールされているか確認：

```powershell
dotnet tool list -g
```

インストールされていない場合は、再インストール：

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
