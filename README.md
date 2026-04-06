# Dota1 Warkey

专为 DotA 1（Dota1）设计的键盘改键工具。

## 项目介绍

这是基于 [WarkeyNETIII](https://github.com/noobhacker/WarkeyNETIII) 重构的 Dota1 专用改键工具，使用现代 .NET 技术重新编写，保留了经典改键功能的同时提升了稳定性和性能。

## 功能特性

- **专为 Dota1 设计**：完美适配 DotA 1 游戏场景
- **智能改键**：支持自定义物品栏改键，不影响正常聊天输入
- **低资源占用**：最小化时几乎不占用 CPU 资源
- **现代 UI**：基于 WPF 开发，界面简洁清晰
- **无广告**：完全免费，没有任何广告弹窗
- **开源**：代码完全开源可查

## 项目结构

```
Dota1Warkey/
├── Dota1Warkey.csproj     # 项目文件
├── App.xaml/App.xaml.cs   # 应用程序入口
├── MainWindow.xaml/cs     # 主窗口
├── Settings.cs            # 设置管理
├── Converters/            # 值转换器
├── Items/                 # 数据模型
│   └── HotkeyItem.cs      # 改键项目
├── Pages/                 # 页面
│   ├── WarkeyPage         # 改键设置页面
│   └── SettingsPage       # 全局设置页面
├── Services/              # 核心服务
│   ├── KeyboardHookService.cs    # 键盘钩子服务
│   ├── MainService.cs            # 主服务逻辑
│   ├── ForegroundWindowService.cs # 窗口前景检测
│   └── PostMessageService.cs     # 消息发送服务
├── ViewModels/            # MVVM 视图模型
│   ├── MainViewModel
│   ├── WarkeyViewModel
│   └── SettingsViewModel
├── publish-fd/            # 框架依赖发布版
└── publish-small/         # 单文件精简发布版
```

## 改键原理

- 使用全局键盘钩子捕获按键输入
- 检测魔兽争霸 III 窗口是否在前台
- 如果匹配改键规则，则将改键后的按键发送到游戏窗口
- 抑制原始按键传递，避免双击

## 使用说明

1. 下载并运行 `Dota1Warkey.exe`
2. 在界面上设置你需要的物品栏改键
3. 启动魔兽争霸 III 和 Dota1 游戏
4. 程序会自动检测游戏窗口并启用改键

## 系统要求

- Windows 10 / Windows 11
- .NET Framework 4.8（大多数 Windows 系统已预装）
  - 如果运行提示缺少框架，可以从微软官网下载安装：<https://dotnet.microsoft.com/download/dotnet-framework/net48>

## 下载发布版

在 `publish-fd` 或 `publish-small` 目录中已包含预编译版本：

- **publish-fd/**：框架依赖版，体积较小（约 200KB），需要系统已安装 .NET Framework 4.8
- **publish-small/**：单文件版，包含必要运行时组件，体积较大（约 28MB），可直接运行

## 构建项目

### 需求

- Visual Studio 2022
- .NET 工作负载（支持 .NET Framework 4.8 和 .NET 6+/NET 10）

### 构建步骤

1. 克隆仓库
2. 使用 Visual Studio 打开 `Dota1Warkey.csproj`
3. 选择目标框架（net48 推荐兼容大多数系统）
4. 生成 → 生成解决方案

## 技术细节

- 框架：支持 `net48` 和 `net10-windows` 双目标
- UI 框架：WPF
- 依赖：
  - System.Text.Json - 设置序列化
  - Fody + Costura.Fody - 嵌入依赖程序集

## 原项目

- 原始项目：[https://github.com/noobhacker/WarkeyNETIII](https://github.com/noobhacker/WarkeyNETIII)

## 许可证

MIT