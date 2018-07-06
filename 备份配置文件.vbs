
Rem 以下为VbScript脚本
Set WshShell = WScript.CreateObject("WScript.Shell")
strDesktop = WshShell.SpecialFolders("Desktop") :'特殊文件夹“桌面”

Dim folderName1
folderName1 = ""

Dim fso1
Set fso1 = CreateObject("Scripting.FileSystemObject")

Dim fullpath
fullpath = fso1.GetAbsolutePathName(folderName1)



set fso = CreateObject("Scripting.FileSystemObject")
fso.CopyFile fullpath +"\mySystem\mySystem\bin\Release\mySystem.EXE.config", strDesktop+"\ERP配置文件备份",False