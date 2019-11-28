@echo off

%1(start /min cmd.exe /c %0 :&exit)

TITLE=DataTransferServer

java -Xms1024M -Xmx1024M -jar DataTransferServer.jar