# �V���v���ȃJ�o���b�W�擾�X�N���v�g�i���{��Łj
# �R���\�[���o�͂̂�
# �g�p���@: chcp 65001; .\Run-TestWithCoverage-JP.ps1

# �R���\�[����UTF-8�ɐݒ�
chcp 65001 | Out-Null
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "�e�X�g�����s���ăJ�o���b�W���擾��..." -ForegroundColor Green
Write-Host ""

dotnet test /p:CollectCoverage=true `
    /p:CoverletOutputFormat=json `
    /p:CoverletOutput="./TestResults/" `
    /p:Exclude="[*.Tests]*"

Write-Host ""
Write-Host "�J�o���b�W�擾����" -ForegroundColor Green
Write-Host "�ڍׂ�HTML���|�[�g�𐶐�����ɂ́AGet-TestCoverage-JP.ps1 �����s���Ă�������" -ForegroundColor Cyan
