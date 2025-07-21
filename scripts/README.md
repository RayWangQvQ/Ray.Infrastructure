# 覆盖率生成脚本使用说明

本项目提供了两个脚本用于生成单元测试覆盖率报告，支持在本地和CI/CD环境中使用。

## 脚本文件

- `scripts/generate-coverage.sh` - Linux/macOS Shell脚本
- `scripts/generate-coverage.ps1` - Windows PowerShell脚本

## 功能特性

✅ **自动化测试执行** - 运行所有单元测试并收集覆盖率数据  
✅ **多格式报告** - 生成HTML、JSON、XML等多种格式的覆盖率报告  
✅ **详细分析** - 提供类级别的覆盖率详情和统计信息  
✅ **阈值检查** - 支持自定义覆盖率阈值并进行验证  
✅ **智能文件查找** - 自动查找并使用最合适的覆盖率数据格式  
✅ **彩色输出** - 提供清晰的彩色日志输出  

## 使用方法

### Linux/macOS (Shell)

```bash
# 基本用法 - 使用默认设置
./scripts/generate-coverage.sh

# 设置覆盖率阈值为80%
./scripts/generate-coverage.sh --threshold 80

# 跳过覆盖率阈值检查
./scripts/generate-coverage.sh --no-threshold-check

# 自定义输出目录
./scripts/generate-coverage.sh --output-dir MyReport

# 查看帮助
./scripts/generate-coverage.sh --help
```

### Windows (PowerShell)

```powershell
# 基本用法 - 使用默认设置
.\scripts\generate-coverage.ps1

# 设置覆盖率阈值为80%
.\scripts\generate-coverage.ps1 -Threshold 80

# 跳过覆盖率阈值检查
.\scripts\generate-coverage.ps1 -NoThresholdCheck

# 自定义输出目录
.\scripts\generate-coverage.ps1 -OutputDir "MyReport"

# 查看帮助
.\scripts\generate-coverage.ps1 -Help
```

## 参数说明

| 参数 | Shell | PowerShell | 默认值 | 说明 |
|------|--------|------------|--------|------|
| 覆盖率阈值 | `--threshold <number>` | `-Threshold <number>` | 90 | 设置覆盖率阈值百分比 |
| 跳过阈值检查 | `--no-threshold-check` | `-NoThresholdCheck` | false | 跳过覆盖率阈值验证 |
| 输出目录 | `--output-dir <path>` | `-OutputDir <path>` | CoverageReport | 覆盖率报告输出目录 |
| 测试结果目录 | `--test-results <path>` | `-TestResultsDir <path>` | TestResults | 测试结果文件目录 |
| 帮助信息 | `--help` | `-Help` | - | 显示使用帮助 |

## 输出文件

脚本执行成功后会在输出目录（默认为 `CoverageReport`）生成以下文件：

```
CoverageReport/
├── index.html              # 主HTML报告（推荐查看）
├── Summary.json            # JSON格式摘要数据
├── Summary.txt             # 文本格式摘要
├── Cobertura.xml          # Cobertura格式报告
└── badge_*.svg            # 覆盖率徽章文件
```

## 前置要求

### 必需工具
- **.NET SDK** - 用于编译和运行测试
- **ReportGenerator** - 脚本会自动安装此全局工具

### 可选工具
- **jq** (Linux/macOS) - 用于解析JSON并显示详细的类级别覆盖率信息
  ```bash
  # Ubuntu/Debian
  sudo apt-get install jq
  
  # macOS
  brew install jq
  ```

## 使用示例

### 开发环境快速检查
```bash
# 快速生成覆盖率报告，阈值设为60%
./scripts/generate-coverage.sh --threshold 60
```

### CI/CD环境
```bash
# 严格模式，要求90%覆盖率
./scripts/generate-coverage.sh --threshold 90
```

### 本地调试模式
```bash
# 只生成报告，不检查阈值
./scripts/generate-coverage.sh --no-threshold-check
```

## 输出示例

脚本运行时会显示详细的进度信息：

```
========================================
ℹ️  开始生成单元测试覆盖率报告
========================================

ℹ️  配置信息:
  项目根目录: /path/to/Ray.Infrastructure
  测试结果目录: TestResults
  覆盖率报告目录: CoverageReport
  覆盖率阈值: 90%
  检查阈值: true

ℹ️  安装 ReportGenerator...
ℹ️  ReportGenerator 已安装
ℹ️  清理旧的测试结果和覆盖率报告...
ℹ️  运行单元测试并生成覆盖率数据...

✅ 单元测试完成!

ℹ️  生成覆盖率报告...
✅ 覆盖率报告生成完成!

ℹ️  覆盖率详细分析:
==============================

ℹ️  整体覆盖率摘要:
Summary
  Generated on: 2025-07-21T19:19:19Z
  Line coverage: 65.5%
  Branch coverage: 58.2%
  ...

✅ 覆盖率检查通过! (65.5% >= 60%)

========================================
✅ 覆盖率报告生成完成!
ℹ️  报告位置: /path/to/Ray.Infrastructure/CoverageReport/index.html
========================================
```

## 故障排除

### 常见问题

1. **权限错误 (Linux/macOS)**
   ```bash
   chmod +x ./scripts/generate-coverage.sh
   ```

2. **PowerShell执行策略错误**
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

3. **找不到覆盖率文件**
   - 确保测试项目正确配置
   - 检查是否有可运行的单元测试

4. **ReportGenerator安装失败**
   - 确保.NET SDK已正确安装
   - 检查网络连接

### 获取帮助

如果遇到问题，请：
1. 首先查看脚本的详细输出日志
2. 使用 `--help` 或 `-Help` 参数查看使用说明
3. 检查前置要求是否满足
