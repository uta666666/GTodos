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
