# 单元测试覆盖率生成脚本 (PowerShell)
# 用法: .\scripts\generate-coverage.ps1 [参数]
# 参数:
#   -Threshold <number>       覆盖率阈值 (默认: 90)
#   -NoThresholdCheck         跳过覆盖率阈值检查
#   -OutputDir <path>         覆盖率报告输出目录 (默认: CoverageReport)
#   -TestResultsDir <path>    测试结果目录 (默认: TestResults)
#   -Help                     显示帮助信息

param(
    [int]$Threshold = 90,
    [switch]$NoThresholdCheck,
    [string]$OutputDir = "CoverageReport",
    [string]$TestResultsDir = "TestResults",
    [switch]$Help
)

# 脚本根目录
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$ProjectRoot = Split-Path -Parent $ScriptDir

# 颜色定义
$Colors = @{
    Red = "Red"
    Green = "Green"
    Yellow = "Yellow"
    Blue = "Blue"
    Cyan = "Cyan"
}

# 帮助信息
function Show-Help {
    Write-Host "单元测试覆盖率生成脚本 (PowerShell)" -ForegroundColor $Colors.Blue
    Write-Host ""
    Write-Host "用法: .\scripts\generate-coverage.ps1 [参数]"
    Write-Host ""
    Write-Host "参数:"
    Write-Host "  -Threshold <number>       覆盖率阈值 (默认: 90)"
    Write-Host "  -NoThresholdCheck         跳过覆盖率阈值检查"
    Write-Host "  -OutputDir <path>         覆盖率报告输出目录 (默认: CoverageReport)"
    Write-Host "  -TestResultsDir <path>    测试结果目录 (默认: TestResults)"
    Write-Host "  -Help                     显示帮助信息"
    Write-Host ""
    Write-Host "示例:"
    Write-Host "  .\scripts\generate-coverage.ps1                           # 使用默认设置"
    Write-Host "  .\scripts\generate-coverage.ps1 -Threshold 80             # 设置覆盖率阈值为80%"
    Write-Host "  .\scripts\generate-coverage.ps1 -NoThresholdCheck         # 跳过阈值检查"
    Write-Host "  .\scripts\generate-coverage.ps1 -OutputDir MyReport       # 自定义输出目录"
}

# 日志函数
function Write-InfoLog {
    param([string]$Message)
    Write-Host "ℹ️  $Message" -ForegroundColor $Colors.Blue
}

function Write-SuccessLog {
    param([string]$Message)
    Write-Host "✅ $Message" -ForegroundColor $Colors.Green
}

function Write-WarningLog {
    param([string]$Message)
    Write-Host "⚠️  $Message" -ForegroundColor $Colors.Yellow
}

function Write-ErrorLog {
    param([string]$Message)
    Write-Host "❌ $Message" -ForegroundColor $Colors.Red
}

# 检查工具是否安装
function Test-Tool {
    param([string]$ToolName)

    try {
        $null = Get-Command $ToolName -ErrorAction Stop
        return $true
    }
    catch {
        return $false
    }
}

# 安装 ReportGenerator
function Install-ReportGenerator {
    Write-InfoLog "检查 ReportGenerator 安装状态..."

    try {
        $toolList = dotnet tool list -g 2>$null
        if ($toolList -match "dotnet-reportgenerator-globaltool") {
            Write-InfoLog "ReportGenerator 已安装"
        }
        else {
            Write-InfoLog "安装 ReportGenerator..."
            dotnet tool install -g dotnet-reportgenerator-globaltool
            if ($LASTEXITCODE -eq 0) {
                Write-SuccessLog "ReportGenerator 安装完成"
            }
            else {
                Write-ErrorLog "ReportGenerator 安装失败"
                exit 1
            }
        }
    }
    catch {
        Write-ErrorLog "检查或安装 ReportGenerator 时出错: $_"
        exit 1
    }
}

# 清理旧文件
function Clear-OldFiles {
    Write-InfoLog "清理旧的测试结果和覆盖率报告..."

    $testResultsPath = Join-Path $ProjectRoot $TestResultsDir
    $outputPath = Join-Path $ProjectRoot $OutputDir

    if (Test-Path $testResultsPath) {
        Remove-Item -Path $testResultsPath -Recurse -Force
    }

    if (Test-Path $outputPath) {
        Remove-Item -Path $outputPath -Recurse -Force
    }
}

# 运行单元测试并生成覆盖率数据
function Invoke-Tests {
    Write-InfoLog "运行单元测试并生成覆盖率数据..."

    Set-Location $ProjectRoot

    $testArgs = @(
        "test"
        "--configuration", "Release"
        "--verbosity", "normal"
        "--collect:XPlat Code Coverage"
        "--results-directory", "./$TestResultsDir/"
        "--logger", "trx"
        "--logger", "console;verbosity=detailed"
        "--"
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura"
        "--"
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Include=[Ray.Infrastructure]*"
        "--"
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Exclude=[Ray.Infrastructure.Tests]*"
    )

    & dotnet @testArgs

    if ($LASTEXITCODE -ne 0) {
        Write-ErrorLog "单元测试失败!"
        exit 1
    }

    Write-SuccessLog "单元测试完成!"

    # 列出生成的覆盖率文件
    Write-InfoLog "生成的覆盖率文件:"
    Get-ChildItem -Path "./$TestResultsDir" -Recurse -Include "*.xml", "coverage.*" | Select-Object -First 10 | ForEach-Object {
        Write-Host "  $($_.FullName)"
    }
}

# 查找覆盖率文件
function Find-CoverageFiles {
    $testResultsPath = Join-Path $ProjectRoot $TestResultsDir

    # 优先查找 cobertura 格式文件
    $coberturaFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "*.cobertura.xml" -ErrorAction SilentlyContinue
    if ($coberturaFiles) {
        Write-InfoLog "找到 cobertura 格式覆盖率文件"
        return ($coberturaFiles.FullName -join ";")
    }

    # 备选 opencover 格式文件
    $opencoverFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "*.opencover.xml" -ErrorAction SilentlyContinue
    if ($opencoverFiles) {
        Write-InfoLog "找到 opencover 格式覆盖率文件"
        return ($opencoverFiles.FullName -join ";")
    }

    # 查找通用 coverage.xml 文件
    $coverageFiles = Get-ChildItem -Path $testResultsPath -Recurse -Filter "coverage.xml" -ErrorAction SilentlyContinue
    if ($coverageFiles) {
        Write-InfoLog "找到通用 coverage.xml 文件"
        return ($coverageFiles.FullName -join ";")
    }

    Write-ErrorLog "未找到覆盖率文件!"
    exit 1
}

# 生成覆盖率报告
function New-CoverageReport {
    Write-InfoLog "生成覆盖率报告..."

    $coverageFiles = Find-CoverageFiles
    Write-InfoLog "使用覆盖率文件模式: $coverageFiles"

    $reportArgs = @(
        "-reports:$coverageFiles"
        "-targetdir:$OutputDir"
        "-reporttypes:Html;Cobertura;JsonSummary;TextSummary;Badges"
        "-verbosity:Info"
    )

    & reportgenerator @reportArgs

    if ($LASTEXITCODE -eq 0) {
        Write-SuccessLog "覆盖率报告生成完成!"
    }
    else {
        Write-ErrorLog "覆盖率报告生成失败!"
        exit 1
    }
}

# 显示覆盖率详情
function Show-CoverageDetails {
    Write-InfoLog "覆盖率详细分析:"
    Write-Host "=============================="

    # 显示整体摘要
    $summaryPath = Join-Path $ProjectRoot "$OutputDir/Summary.txt"
    if (Test-Path $summaryPath) {
        Write-Host ""
        Write-InfoLog "整体覆盖率摘要:"
        Get-Content $summaryPath
        Write-Host ""
    }

    # 解析并显示类级别覆盖率
    $jsonPath = Join-Path $ProjectRoot "$OutputDir/Summary.json"
    if (Test-Path $jsonPath) {
        Write-Host ""
        Write-InfoLog "类级别覆盖率详情:"
        Write-Host "--------------------------------"

        try {
            $summary = Get-Content $jsonPath | ConvertFrom-Json
            $assembly = $summary.coverage.assemblies | Where-Object { $_.name -like "*Ray.Infrastructure*" }

            if ($assembly -and $assembly.classesinassembly) {
                # 显示所有类的覆盖率
                $assembly.classesinassembly | Where-Object { $_.name -ne "" } | ForEach-Object {
                    $branchCoverage = if ($_.branchcoverage -eq $null) { "N/A" } else { "$($_.branchcoverage)%" }
                    Write-Host "Class: $($_.name) | Line Coverage: $($_.coverage)% | Branch Coverage: $branchCoverage | Lines: $($_.coveredlines)/$($_.coverablelines)"
                }

                Write-Host ""
                Write-WarningLog "低覆盖率类 (< 50%):"
                Write-Host "------------------------------------"

                $lowCoverageClasses = $assembly.classesinassembly | Where-Object {
                    $_.name -ne "" -and $_.coverage -is [double] -and $_.coverage -lt 50
                }

                if ($lowCoverageClasses) {
                    $lowCoverageClasses | ForEach-Object {
                        Write-Host "❌ $($_.name): $($_.coverage)% ($($_.coveredlines)/$($_.coverablelines) lines)" -ForegroundColor $Colors.Red
                    }
                }
                else {
                    Write-Host "无低覆盖率类" -ForegroundColor $Colors.Green
                }

                Write-Host ""
                Write-SuccessLog "高覆盖率类 (>= 80%):"
                Write-Host "--------------------------------------"

                $highCoverageClasses = $assembly.classesinassembly | Where-Object {
                    $_.name -ne "" -and $_.coverage -is [double] -and $_.coverage -ge 80
                }

                if ($highCoverageClasses) {
                    $highCoverageClasses | ForEach-Object {
                        Write-Host "✅ $($_.name): $($_.coverage)% ($($_.coveredlines)/$($_.coverablelines) lines)" -ForegroundColor $Colors.Green
                    }
                }
                else {
                    Write-Host "无高覆盖率类"
                }
            }
        }
        catch {
            Write-WarningLog "解析覆盖率 JSON 文件时出错，无法显示详细信息: $_"
        }
    }
}

# 检查覆盖率阈值
function Test-CoverageThreshold {
    if ($NoThresholdCheck) {
        Write-InfoLog "跳过覆盖率阈值检查"
        return $true
    }

    Write-InfoLog "检查覆盖率阈值..."

    $jsonPath = Join-Path $ProjectRoot "$OutputDir/Summary.json"
    if (Test-Path $jsonPath) {
        try {
            $summary = Get-Content $jsonPath | ConvertFrom-Json
            $coverage = $summary.summary.linecoverage
            Write-InfoLog "当前覆盖率: $coverage%"

            if ($coverage -ge $Threshold) {
                Write-SuccessLog "覆盖率检查通过! ($coverage% >= $Threshold%)"
                return $true
            }
            else {
                Write-ErrorLog "覆盖率检查失败! 当前: $coverage%, 要求: $Threshold%"
                return $false
            }
        }
        catch {
            Write-WarningLog "解析覆盖率数据时出错，无法检查阈值: $_"
            return $true
        }
    }
    else {
        Write-ErrorLog "覆盖率报告文件未找到!"
        return $false
    }
}

# 主函数
function Main {
    if ($Help) {
        Show-Help
        return
    }

    Write-Host "========================================" -ForegroundColor $Colors.Cyan
    Write-InfoLog "开始生成单元测试覆盖率报告"
    Write-Host "========================================" -ForegroundColor $Colors.Cyan
    Write-Host ""

    Write-InfoLog "配置信息:"
    Write-Host "  项目根目录: $ProjectRoot"
    Write-Host "  测试结果目录: $TestResultsDir"
    Write-Host "  覆盖率报告目录: $OutputDir"
    Write-Host "  覆盖率阈值: $Threshold%"
    Write-Host "  检查阈值: $(-not $NoThresholdCheck)"
    Write-Host ""

    # 检查必要工具
    if (-not (Test-Tool "dotnet")) {
        Write-ErrorLog ".NET SDK 未安装，请先安装 .NET SDK"
        exit 1
    }

    # 安装 ReportGenerator
    Install-ReportGenerator

    # 清理旧文件
    Clear-OldFiles

    # 运行测试
    Invoke-Tests

    # 生成报告
    New-CoverageReport

    # 显示详情
    Show-CoverageDetails

    # 检查阈值
    $thresholdPassed = Test-CoverageThreshold

    Write-Host ""
    Write-Host "========================================" -ForegroundColor $Colors.Cyan

    $reportPath = Join-Path $ProjectRoot "$OutputDir/index.html"

    if ($thresholdPassed) {
        Write-SuccessLog "覆盖率报告生成完成!"
        Write-InfoLog "报告位置: $reportPath"

        # 询问是否打开报告
        $openReport = Read-Host "是否打开覆盖率报告? (y/N)"
        if ($openReport -eq "y" -or $openReport -eq "Y") {
            if (Test-Path $reportPath) {
                Start-Process $reportPath
            }
        }
    }
    else {
        Write-ErrorLog "覆盖率报告生成完成，但未达到阈值要求!"
        Write-InfoLog "报告位置: $reportPath"
        exit 1
    }

    Write-Host "========================================" -ForegroundColor $Colors.Cyan
}

# 设置错误处理
$ErrorActionPreference = "Stop"

# 运行主函数
try {
    Main
}
catch {
    Write-ErrorLog "脚本执行过程中发生错误: $_"
    exit 1
}
