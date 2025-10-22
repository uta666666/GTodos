# �e�X�g�J�o���b�W���擾���AHTML���|�[�g�𐶐�����X�N���v�g�i���{��Łj
# UTF-8 BOM�G���R�[�f�B���O�ŕۑ�����K�v������܂�
# �g�p���@: chcp 65001; .\Get-TestCoverage-JP.ps1

# �R���\�[����UTF-8�ɐݒ�
chcp 65001 | Out-Null
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

# �p�����[�^
param(
    [string]$OutputDir = "./TestResults",
    [switch]$OpenReport = $false
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  GTodos �e�X�g�J�o���b�W�擾" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# �o�̓f�B���N�g���̍쐬
if (Test-Path $OutputDir) {
    Write-Host "�����̌��ʂ��N���[���A�b�v��..." -ForegroundColor Yellow
    Remove-Item -Path $OutputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

# �e�X�g���s�ƃJ�o���b�W���W
Write-Host "�e�X�g�����s���ăJ�o���b�W�f�[�^�����W��..." -ForegroundColor Green
$coverageFile = "$OutputDir/coverage.cobertura.xml"

dotnet test /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="$OutputDir/" `
    /p:Exclude="[*.Tests]*"

if ($LASTEXITCODE -ne 0) {
    Write-Host "�e�X�g�̎��s�Ɏ��s���܂���" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "�J�o���b�W�f�[�^�̎��W���������܂���" -ForegroundColor Green

# カバレッジファイルを出力ディレクトリにコピー
Write-Host "カバレッジファイルを出力ディレクトリにコピー中..." -ForegroundColor Green
$generatedCoverageFiles = Get-ChildItem -Path . -Recurse -Filter "coverage.cobertura.xml" -ErrorAction SilentlyContinue
foreach ($file in $generatedCoverageFiles) {
    Copy-Item -Path $file.FullName -Destination $coverageFile -Force
    Write-Host "  コピー完了: $($file.FullName)" -ForegroundColor Gray
}

# ReportGenerator���C���X�g�[������Ă��邩�m�F
$reportGenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue

if (-not $reportGenerator) {
    Write-Host ""
    Write-Host "ReportGenerator���C���X�g�[������Ă��܂���" -ForegroundColor Yellow
    Write-Host "�C���X�g�[�����܂���? (Y/N): " -NoNewline -ForegroundColor Yellow
    $response = Read-Host
    
    if ($response -eq 'Y' -or $response -eq 'y') {
        Write-Host "ReportGenerator���C���X�g�[����..." -ForegroundColor Green
        dotnet tool install -g dotnet-reportgenerator-globaltool
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "ReportGenerator�̃C���X�g�[���Ɏ��s���܂���" -ForegroundColor Red
            exit $LASTEXITCODE
        }
        
        # ���ϐ����X�V
        $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    } else {
        Write-Host "HTML���|�[�g�̐������X�L�b�v���܂�" -ForegroundColor Yellow
        Write-Host "�J�o���b�W�t�@�C��: $coverageFile" -ForegroundColor Cyan
        exit 0
    }
}

# HTML���|�[�g����
Write-Host ""
Write-Host "HTML���|�[�g�𐶐���..." -ForegroundColor Green

$reportDir = "$OutputDir/CoverageReport"
$coverageFiles = Get-ChildItem -Path $OutputDir -Filter "coverage.cobertura.xml" -Recurse

if ($coverageFiles.Count -eq 0) {
    Write-Host "�J�o���b�W�t�@�C����������܂���" -ForegroundColor Red
    exit 1
}

$reportFiles = ($coverageFiles | ForEach-Object { $_.FullName }) -join ";"

reportgenerator `
    -reports:$reportFiles `
    -targetdir:$reportDir `
    -reporttypes:"Html;HtmlSummary;Badges;TextSummary"

if ($LASTEXITCODE -ne 0) {
    Write-Host "���|�[�g�����Ɏ��s���܂���" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  �J�o���b�W���|�[�g��������" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "���|�[�g�̏ꏊ: $reportDir\index.html" -ForegroundColor Green

# �T�}���[�\��
$summaryFile = "$reportDir\Summary.txt"
if (Test-Path $summaryFile) {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Cyan
    Write-Host "  �J�o���b�W�T�}���[" -ForegroundColor Cyan
    Write-Host "=====================================" -ForegroundColor Cyan
    Get-Content $summaryFile | Write-Host
}

# ���|�[�g���J��
if ($OpenReport) {
    Write-Host ""
    Write-Host "HTML���|�[�g���J���Ă��܂�..." -ForegroundColor Green
    Start-Process "$reportDir\index.html"
}

Write-Host ""
Write-Host "�������܂����I" -ForegroundColor Green
