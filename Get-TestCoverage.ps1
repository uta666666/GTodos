# �p�����[�^
param(
    [string]$OutputDir = "./TestResults",
    [switch]$OpenReport
)

# �e�X�g�J�o���b�W���擾���AHTML���|�[�g�𐶐�����X�N���v�g
# UTF-8�G���R�[�f�B���O��ݒ�
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  GTodos Test Coverage" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# �o�̓f�B���N�g���̍쐬
if (Test-Path $OutputDir) {
    Write-Host "Cleaning up existing results..." -ForegroundColor Yellow
    Remove-Item -Path $OutputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

# �e�X�g���s�ƃJ�o���b�W���W
Write-Host "Running tests and collecting coverage data..." -ForegroundColor Green
$coverageFile = "$OutputDir/coverage.cobertura.xml"

dotnet test /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="$OutputDir/" `
    /p:Exclude="[*.Tests]*"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Test execution failed" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Coverage data collection completed" -ForegroundColor Green

# カバレッジファイルを出力ディレクトリにコピー
Write-Host "Copying coverage files to output directory..." -ForegroundColor Green
$generatedCoverageFiles = Get-ChildItem -Path . -Recurse -Filter "coverage.cobertura.xml" -ErrorAction SilentlyContinue
foreach ($file in $generatedCoverageFiles) {
    Copy-Item -Path $file.FullName -Destination $coverageFile -Force
    Write-Host "  Copied: $($file.FullName)" -ForegroundColor Gray
}

# ReportGenerator���C���X�g�[������Ă��邩�m�F
$reportGenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue

if (-not $reportGenerator) {
    Write-Host ""
    Write-Host "ReportGenerator is not installed" -ForegroundColor Yellow
    Write-Host "Do you want to install it? (Y/N): " -NoNewline -ForegroundColor Yellow
    $response = Read-Host
    
    if ($response -eq 'Y' -or $response -eq 'y') {
        Write-Host "Installing ReportGenerator..." -ForegroundColor Green
        dotnet tool install -g dotnet-reportgenerator-globaltool
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "ReportGenerator installation failed" -ForegroundColor Red
            exit $LASTEXITCODE
        }
        
        # ���ϐ����X�V
        $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    } else {
        Write-Host "Skipping HTML report generation" -ForegroundColor Yellow
        Write-Host "Coverage file: $coverageFile" -ForegroundColor Cyan
        exit 0
    }
}

# HTML���|�[�g����
Write-Host ""
Write-Host "Generating HTML report..." -ForegroundColor Green

$reportDir = "$OutputDir/CoverageReport"
$coverageFiles = Get-ChildItem -Path $OutputDir -Filter "coverage.cobertura.xml" -Recurse

if ($coverageFiles.Count -eq 0) {
    Write-Host "Coverage file not found" -ForegroundColor Red
    exit 1
}

$reportFiles = ($coverageFiles | ForEach-Object { $_.FullName }) -join ";"

reportgenerator `
    -reports:$reportFiles `
    -targetdir:$reportDir `
    -reporttypes:"Html;HtmlSummary;Badges;TextSummary"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Report generation failed" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  Coverage Report Generated" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Report location: $reportDir\index.html" -ForegroundColor Green

# �T�}���[�\��
$summaryFile = "$reportDir\Summary.txt"
if (Test-Path $summaryFile) {
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Cyan
    Write-Host "  Coverage Summary" -ForegroundColor Cyan
    Write-Host "=====================================" -ForegroundColor Cyan
    Get-Content $summaryFile | Write-Host
}

# ���|�[�g���J��
if ($OpenReport) {
    Write-Host ""
    Write-Host "Opening HTML report..." -ForegroundColor Green
    Start-Process "$reportDir\index.html"
}

Write-Host ""
Write-Host "Done!" -ForegroundColor Green
