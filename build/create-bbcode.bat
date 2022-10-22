
set markdownFile=%~1
set outputFile=%~2
set htmlFile=%~2.html

pandoc %markdownFile% -t html -o %htmlFile% --wrap=none
powershell -ExecutionPolicy Bypass -File %cd%/build/prepare-html-for-nexus.ps1 %htmlFile% 
powershell -ExecutionPolicy Bypass -File %cd%/build/html-to-bbcode.ps1 %htmlFile% %outputFile%
Rem del %htmlFile%