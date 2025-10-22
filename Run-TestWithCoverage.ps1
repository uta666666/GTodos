# �V���v���ȃJ�o���b�W�擾�X�N���v�g�i�R���\�[���o�͂̂݁j
# UTF-8�G���R�[�f�B���O��ݒ�
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
