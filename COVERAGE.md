# �e�X�g�J�o���b�W�̎擾���@

���̃h�L�������g�ł́AGTodos�v���W�F�N�g�̃e�X�g�J�o���b�W���擾������@��������܂��B

## �O�����

- .NET 9 SDK ���C���X�g�[������Ă��邱��
- coverlet.collector �� coverlet.msbuild �p�b�P�[�W���C���X�g�[���ς݁i���ɐݒ�ς݁j

## �J�o���b�W�̎擾���@

### PowerShell�X�N���v�g���g�p����ꍇ

#### ����: ����������h�����߂�

PowerShell�œ��{��𐳂����\������ɂ́A�ȉ��̂����ꂩ�̕��@���g�p���Ă��������F

**���@1: �p��ŃX�N���v�g���g�p�i�����j**
```powershell
.\Get-TestCoverage.ps1
```

**���@2: ���{��ŃX�N���v�g��UTF-8���[�h�Ŏ��s**
```powershell
chcp 65001
.\Get-TestCoverage-JP.ps1
```

�܂��́AWindows Terminal ���g�p���Ă���ꍇ�́A�ݒ�� UTF-8 ��L���ɂ��邱�Ƃ��ł��܂��B

#### �X�N���v�g�ꗗ

| �X�N���v�g | ���� | ���� |
|-----------|------|------|
| `Get-TestCoverage.ps1` | �p�� | HTML���|�[�g�����i�����j |
| `Get-TestCoverage-JP.ps1` | ���{�� | HTML���|�[�g�����iUTF-8���[�h�K�v�j |
| `Run-TestWithCoverage.ps1` | �p�� | �ȈՃJ�o���b�W�擾 |
| `Run-TestWithCoverage-JP.ps1` | ���{�� | �ȈՃJ�o���b�W�擾�iUTF-8���[�h�K�v�j |

#### HTML���|�[�g�������I�ɊJ��

```powershell
.\Get-TestCoverage.ps1 -OpenReport
```

### 1. ��{�I�ȃJ�o���b�W�擾�i�R���\�[���o�́j

```powershell
dotnet test /p:CollectCoverage=true
```

### 2. �J�o���b�W��JSON�`���ŏo��

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=json /p:CoverletOutput=./TestResults/
```

### 3. �J�o���b�W��HTML���|�[�g�ŏo��

�܂��AReportGenerator�c�[�����C���X�g�[���F

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
```

���ɁA�J�o���b�W�����W����HTML���|�[�g�𐶐��F

```powershell
# �J�o���b�W�f�[�^�����W
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/coverage.cobertura.xml

# HTML���|�[�g�𐶐�
reportgenerator -reports:"./GTodosLib.Tests/TestResults/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:Html
```

HTML���|�[�g�� `./TestResults/CoverageReport/index.html` �ɐ�������܂��B

### 4. �����̌`���ŏo��

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"json,cobertura,lcov\" /p:CoverletOutput=./TestResults/
```

### 5. ����̃v���W�F�N�g�̂݃J�o���b�W���擾

```powershell
dotnet test GTodosLib.Tests/GTodosLib.Tests.csproj /p:CollectCoverage=true
```

## Visual Studio �ł̃J�o���b�W�擾

Visual Studio Enterprise �G�f�B�V�������g�p���Ă���ꍇ�F

1. �e�X�g�G�N�X�v���[���[���J���i�e�X�g �� �e�X�g�G�N�X�v���[���[�j
2. �u���ׂẴe�X�g���R�[�h�J�o���b�W�Ŏ��s�v���N���b�N
3. �J�o���b�W���ʃE�B���h�E�Ō��ʂ��m�F

## �J�o���b�W�ݒ�̃J�X�^�}�C�Y

`GTodosLib.Tests/coverlet.runsettings` �t�@�C���ŁA���O���[���Ȃǂ�ݒ�ł��܂��B

�g�p���@�F

```powershell
dotnet test --settings GTodosLib.Tests/coverlet.runsettings /p:CollectCoverage=true
```

## �J�o���b�W���ʂ̌���

- **Line Coverage�i�s�J�o���b�W�j**: ���s���ꂽ�s�̊���
- **Branch Coverage�i����J�o���b�W�j**: ���s���ꂽ����̊���
- **Method Coverage�i���\�b�h�J�o���b�W�j**: ���s���ꂽ���\�b�h�̊���

## ���������J�o���b�W�ڕW

- �S�̓I�ȃJ�o���b�W: 80%�ȏ�
- �d�v�ȃr�W�l�X���W�b�N: 90%�ȏ�
- ���[�e�B���e�B�N���X: 70%�ȏ�

## �g���u���V���[�e�B���O

### PowerShell�ŕ�����������ꍇ

#### �������@1: �p��ŃX�N���v�g���g�p
```powershell
.\Get-TestCoverage.ps1
```

#### �������@2: �R�[�h�y�[�W��UTF-8�ɕύX
```powershell
chcp 65001
.\Get-TestCoverage-JP.ps1
```

#### �������@3: Windows Terminal���g�p
Windows Terminal�i�����j�ł́A�f�t�H���g��UTF-8���T�|�[�g����Ă��܂��B

### �J�o���b�W��0%�ƕ\�������ꍇ

�v���W�F�N�g�Q�Ƃ��������ݒ肳��Ă��邩�m�F���Ă��������F

```powershell
dotnet list GTodosLib.Tests/GTodosLib.Tests.csproj reference
```

### ReportGenerator��������Ȃ��ꍇ

�O���[�o���c�[���Ƃ��ăC���X�g�[������Ă��邩�m�F�F

```powershell
dotnet tool list -g
```

�C���X�g�[������Ă��Ȃ��ꍇ�́A�ăC���X�g�[���F

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
