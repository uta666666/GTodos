# GTodos - Google Tasks TODO表示アプリ

.NET 9を使用してGoogle Tasks APIからTODOを取得して表示するアプリケーションです。

## プロジェクト構成

このソリューションは2つのプロジェクトで構成されています：

### GTodosCLI
コンソールアプリケーションです。Google Tasksからタスクリストとタスクを取得して表示します。

### GTodosLib
Google Tasks API呼び出しを行う共有ライブラリです。CLIアプリケーションだけでなく、将来作成するWindowsFormsアプリケーションなどでも利用できます。

```
GTodos/
├── GTodos.sln              # ソリューションファイル
├── GTodosCLI/              # CLIアプリケーション
│   ├── GTodosCLI.csproj
│   ├── Program.cs
│   ├── credentials.json    # Google API認証情報（自分で配置）
│   └── token.json/         # 認証トークン（自動生成）
├── GTodosLib/              # 共有ライブラリ
│   ├── GTodosLib.csproj
│   └── GoogleTasksService.cs
└── README.md               # このファイル
```

## 機能

- Google Tasks APIを使用してTODOリストを取得
- すべてのタスクリストとそのタスクを表示
- 完了/未完了ステータスの表示
- タスクのメモと期限の表示

## セットアップ手順

### 1. Google Cloud Consoleでの設定

1. [Google Cloud Console](https://console.cloud.google.com/) にアクセス
2. 新しいプロジェクトを作成するか、既存のプロジェクトを選択
3. 左側のメニューから「APIとサービス」→「ライブラリ」に移動
4. "Google Tasks API" を検索して有効化
5. 左側のメニューから「APIとサービス」→「認証情報」に移動
6. 「認証情報を作成」→「OAuthクライアントID」をクリック
7. 同意画面の構成が必要な場合は設定（テストユーザーとして自分のGoogleアカウントを追加）
8. アプリケーションの種類として「デスクトップアプリ」を選択
9. 名前を入力して「作成」をクリック
10. ダウンロードボタンをクリックしてJSONファイルをダウンロード
11. ダウンロードしたファイルを `credentials.json` にリネームして `GTodosCLI` フォルダに配置

### 2. ビルド

```bash
dotnet build
```

### 3. アプリケーションの実行

```bash
cd GTodosCLI
dotnet run
```

または直接実行ファイルを実行：

```bash
GTodosCLI\bin\Debug\net9.0\GTodosCLI.exe
```

初回実行時は、ブラウザが開いてGoogleアカウントでのログインと権限の承認が求められます。

## GTodosLibの使用方法

他のプロジェクトからGTodosLibを使用する場合の例：

```csharp
using GTodosLib;

var service = new GoogleTasksService("Your Application Name");
await service.AuthenticateAsync();

var taskLists = await service.GetTaskListsAsync();
foreach (var taskList in taskLists.Items)
{
    var tasks = await service.GetTasksAsync(taskList.Id);
    // タスクの処理
}
```

## 使用技術

- .NET 9
- Google.Apis.Tasks.v1
- Google.Apis.Auth

## ライセンス

MIT License

# GTodos

Google Tasks APIを使用してTODOリストを管理するC#アプリケーション

## プロジェクト構成

- **GTodosLib**: Google Tasks APIとの連携を行うライブラリプロジェクト
- **GTodosCLI**: コマンドラインインターフェースアプリケーション
- **GTodosLib.Tests**: 単体テストプロジェクト（xUnit）

## 必要な環境

- .NET 9 SDK
- Google Cloud Platform アカウント
- Google Tasks API の認証情報

## セットアップ

### 1. Google Tasks API の認証情報を取得

1. [Google Cloud Console](https://console.cloud.google.com/) にアクセス
2. プロジェクトを作成または選択
3. 「APIとサービス」→「認証情報」に移動
4. 「認証情報を作成」→「OAuthクライアントID」を選択
5. アプリケーションの種類として「デスクトップアプリ」を選択
6. ダウンロードしたJSONファイルを `credentials.json` として保存
7. Google Tasks APIを有効化

### 2. アプリケーションのビルドと実行

```bash
# ビルド
dotnet build

# 実行
dotnet run --project GTodosCLI
```

## テストの実行

### 基本的なテスト実行

```bash
dotnet test
```

### カバレッジ付きでテスト実行

#### オプション1: コンソールでカバレッジを表示

```bash
dotnet test /p:CollectCoverage=true
```

#### オプション2: PowerShellスクリプトを使用（推奨）

**英語版（文字化けなし）**
```powershell
# 簡易版
.\Run-TestWithCoverage.ps1

# HTMLレポート生成
.\Get-TestCoverage.ps1

# HTMLレポートを自動的に開く
.\Get-TestCoverage.ps1 -OpenReport
```

**日本語版（UTF-8モード必要）**
```powershell
chcp 65001
.\Get-TestCoverage-JP.ps1
```

### カバレッジの詳細設定

詳細な設定方法については、[COVERAGE.md](COVERAGE.md) を参照してください。

## プロジェクトの機能

### GTodosLib

`GoogleTasksService` クラスが提供する機能：

- **認証**: Google OAuth2.0 認証
- **タスクリスト取得**: ユーザーのタスクリストを取得
- **タスク取得**: 特定のタスクリストのタスクを取得
- **タスク作成**: 新しいタスクを作成（タイトル、メモ、期限の設定が可能）

### GTodosCLI

コマンドラインインターフェース機能：

1. タスクリストの表示
2. タスクの登録
3. インタラクティブなメニュー操作

## 開発

### テストの追加

テストは `GTodosLib.Tests` プロジェクトに追加してください。xUnitフレームワークを使用しています。

```csharp
[Fact]
public void YourTest()
{
    // Arrange
    var service = new GoogleTasksService("Test App");
    
    // Act
    // ...
    
    // Assert
    Assert.NotNull(service);
}
```

### カバレッジ目標

- 全体的なカバレッジ: 80%以上
- 重要なビジネスロジック: 90%以上

現在のカバレッジは `dotnet test /p:CollectCoverage=true` で確認できます。

## スクリプト一覧

| スクリプト | 言語 | 説明 |
|-----------|------|------|
| `Get-TestCoverage.ps1` | 英語 | HTMLカバレッジレポート生成（推奨） |
| `Get-TestCoverage-JP.ps1` | 日本語 | HTMLカバレッジレポート生成 |
| `Run-TestWithCoverage.ps1` | 英語 | 簡易カバレッジ取得 |
| `Run-TestWithCoverage-JP.ps1` | 日本語 | 簡易カバレッジ取得 |

## ライセンス

このプロジェクトのライセンスについては、リポジトリの所有者にお問い合わせください。

## 貢献

プルリクエストは歓迎します。大きな変更の場合は、まずissueを開いて変更内容を議論してください。

## トラブルシューティング

### 認証エラー

`credentials.json` ファイルが正しい場所にあることを確認してください。

### PowerShellで文字化けする場合

英語版スクリプト `.\Get-TestCoverage.ps1` を使用するか、日本語版を使用する場合は `chcp 65001` でUTF-8モードに切り替えてから実行してください。

### テストの失敗

```bash
# 詳細なログを表示
dotnet test --logger "console;verbosity=detailed"
```

### カバレッジが0%と表示される

プロジェクト参照を確認：

```bash
dotnet list GTodosLib.Tests/GTodosLib.Tests.csproj reference
