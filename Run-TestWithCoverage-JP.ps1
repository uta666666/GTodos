# シンプルなカバレッジ取得スクリプト（日本語版）
# コンソール出力のみ
# 使用方法: chcp 65001; .\Run-TestWithCoverage-JP.ps1

# コンソールをUTF-8に設定
chcp 65001 | Out-Null
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "テストを実行してカバレッジを取得中..." -ForegroundColor Green
Write-Host ""

dotnet test /p:CollectCoverage=true `
    /p:CoverletOutputFormat=json `
    /p:CoverletOutput="./TestResults/" `
    /p:Exclude="[*.Tests]*"

Write-Host ""
Write-Host "カバレッジ取得完了" -ForegroundColor Green
Write-Host "詳細なHTMLレポートを生成するには、Get-TestCoverage-JP.ps1 を実行してください" -ForegroundColor Cyan
