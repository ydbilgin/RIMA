@echo off
set "CODEX_HOME=C:\Users\ydbil\.codex-profiles\laurethgame"
set "CODEX_TASK_FILE=F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_task_a.txt"
set "CODEX_OUTPUT=F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_task_a_output.txt"
set "CODEX_STDOUT=F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_stdout2.txt"
set "CODEX_STDERR=F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_stderr2.txt"
cd /d "F:\Antigravity Projeler\2d roguelite\RIMA"
node "%APPDATA%\npm\node_modules\@openai\codex\bin\codex.js" exec --model gpt-5.5 --sandbox danger-full-access --output-last-message "%CODEX_OUTPUT%" - < "%CODEX_TASK_FILE%" > "%CODEX_STDOUT%" 2> "%CODEX_STDERR%"
echo EXIT_CODE=%ERRORLEVEL% >> "%CODEX_STDOUT%"
