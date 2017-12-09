
Rem 以下为VbScript脚本
Set WshShell = WScript.CreateObject("WScript.Shell")
strDesktop = WshShell.SpecialFolders("Desktop") :'特殊文件夹“桌面”

Dim folderName1
folderName1 = ""

Dim fso1
Set fso1 = CreateObject("Scripting.FileSystemObject")

Dim fullpath
fullpath = fso1.GetAbsolutePathName(folderName1)


set oShellLink = WshShell.CreateShortcut(strDesktop & "\颇尔奥新ERP系统.lnk")
oShellLink.TargetPath = fullpath  + "\mySystem\mySystem\bin\Release\mySystem.exe" : '目标
oShellLink.WindowStyle = 3 :'参数1默认窗口激活，参数3最大化激活，参数7最小化
oShellLink.IconLocation = fullpath  + "\mySystem\mySystem\pic\logo32.png"
oShellLink.WorkingDirectory = fullpath  + "\mySystem\mySystem\bin\Release" '起始位置
oShellLink.Save : '创建保存快捷方式
