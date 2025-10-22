# シンプルなカバレッジ取得スクリプト（コンソール出力のみ）
# UTF-8エンコーディングを設定
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "Running tests with coverage..." -ForegroundColor Green
Write-Host ""

dotnet test /p:CollectCoverage=true `
    /p:CoverletOutputFormat=json `
    /p:CoverletOutput="./TestResults/" `
    /p:Exclude="[*.Tests]*"

Write-Host ""
Write-Host "Coverage collection completed" -ForegroundColor Green
Write-Host "For detailed HTML report, run: .\Get-TestCoverage.ps1" -ForegroundColor Cyan
