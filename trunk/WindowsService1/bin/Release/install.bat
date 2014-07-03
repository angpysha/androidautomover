@echo off
rem AutoMover Install
set cur_dir=%~dp0
sc create AndroidAutoMover binPath="%cur_dir%Andoid AutoMover.exe" start=auto
sc start AndroidAutoMover
pause