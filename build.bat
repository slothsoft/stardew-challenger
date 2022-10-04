
Rem Release the entire project
Rem dotnet publish -c Release

Rem Create a folder that contains everything that should be in the ZIP
set projectFolder=%cd%\Challenger
set zipFolder=%projectFolder%\bin\ReleaseZip
set outputFolder=%zipFolder%\Challenger
if exist "%zipFolder%" rmdir /s /q "%zipFolder%"
if not exist "%outputFolder%" mkdir "%outputFolder%"

xcopy /y %projectFolder%\bin\Release\net5.0\publish\Challenger.dll %outputFolder%
xcopy /y %projectFolder%\bin\Release\net5.0\publish\Challenger.pdb %outputFolder%
xcopy /y %projectFolder%\manifest.json %outputFolder%
xcopy /y %projectFolder%\i18n\ %outputFolder%\i18n\
xcopy /y %cd%\LICENSE %outputFolder%

Rem Make a HTML file out of the README
Rem If this stops working, maybe run "choco install pandoc" on the PowerShell again?
pandoc %cd%\README.md -t html -o %outputFolder%\Readme.html -s --metadata title="Slothsoft Challenger"

Rem Replace the image URLs of the HTML
powershell -Command "((gc %outputFolder%\Readme.html -encoding utf8) -replace 'readme/', 'https://github.com/slothsoft/stardew-challenger/raw/main/readme/') | Out-File -encoding utf8 %outputFolder%\Readme.html"

Rem Now zip the entire folder
"C:\Program Files\7-Zip\7z.exe" a %zipFolder%\Challenger-%1.zip %zipFolder%/*