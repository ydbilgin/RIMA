' silent_dispatch.vbs — Global silent wrapper for agy_dispatch.py and cx_dispatch.py
'
' Problem: agy_dispatch.py spawns winpty + agy.exe; agy.exe is a CONSOLE-subsystem
' binary so Windows briefly allocates a conhost/OpenConsole window. pywinpty's
' SetWinEventHook tries to catch this but on Windows 11 with Windows Terminal
' default, a Z-order flash still steals focus / mid-game window.
'
' Solution layers (this VBS):
'   1) VBS files themselves never open a console window (no cscript/wscript shell visible).
'   2) WScript.Shell.Run with intWindowStyle=0 (vbHide) tells Windows to spawn the
'      child process with hidden window state at CreateProcess level.
'   3) We invoke pythonw.exe (GUI-subsystem variant of python.exe) — Windows allocates
'      NO console for it. Child agy.exe + winpty ConPTY still spawn, but the parent
'      python process won't flash a conhost.
'   4) Optional bWaitOnReturn=True lets the caller wait for completion + capture exit code.
'
' Usage from Bash tool:
'   cscript //nologo "F:/Antigravity Projeler/2d roguelite/RIMA/silent_dispatch.vbs" agy --task-file "..." --print-timeout 1800
'   cscript //nologo "F:/Antigravity Projeler/2d roguelite/RIMA/silent_dispatch.vbs" cx --task-file "..." --effort xhigh --timeout 2400
'
' Or with full Python path:
'   cscript //nologo "...silent_dispatch.vbs" agy [args]
'
' Output: dispatcher already writes to AGY_DONE_<account>.md / CODEX_DONE_<profile>.md
' so background-task output capture is NOT broken — Bash tool reads result file
' instead of stdout.
'
' Exit codes:
'   0  Dispatch ran successfully
'   1  No args / unknown dispatcher type
'   2  pythonw.exe not found
'   N  pass-through from Python dispatcher
'
' DEPENDENCIES:
'   - pythonw.exe in standard Python install location
'   - agy_dispatch.py / cx_dispatch.py siblings of this VBS file

Option Explicit

Dim objShell, objFS, objArgs, scriptDir
Dim dispatcherType, scriptPath, pythonwPath
Dim cmdLine, i, exitCode

Set objShell = CreateObject("WScript.Shell")
Set objFS = CreateObject("Scripting.FileSystemObject")
Set objArgs = WScript.Arguments

' Get the directory this VBS lives in (RIMA root)
scriptDir = objFS.GetParentFolderName(WScript.ScriptFullName)

If objArgs.Count < 1 Then
    ' Cannot show MsgBox in silent mode — write error to log file instead
    objShell.LogEvent 1, "silent_dispatch.vbs: missing args. Usage: cscript silent_dispatch.vbs {agy|cx} [args...]"
    WScript.Quit 1
End If

dispatcherType = LCase(objArgs(0))

If dispatcherType = "agy" Then
    scriptPath = scriptDir & "\agy_dispatch.py"
ElseIf dispatcherType = "cx" Or dispatcherType = "codex" Then
    scriptPath = scriptDir & "\cx_dispatch.py"
Else
    objShell.LogEvent 1, "silent_dispatch.vbs: unknown dispatcher type '" & dispatcherType & "'. Use 'agy' or 'cx'."
    WScript.Quit 1
End If

If Not objFS.FileExists(scriptPath) Then
    objShell.LogEvent 1, "silent_dispatch.vbs: dispatcher script not found at " & scriptPath
    WScript.Quit 1
End If

' Locate pythonw.exe (GUI-subsystem variant, no console allocation)
Dim pythonwCandidates
pythonwCandidates = Array( _
    "C:\Program Files\Python312\pythonw.exe", _
    "C:\Program Files\Python313\pythonw.exe", _
    "C:\Program Files\Python311\pythonw.exe", _
    "C:\Users\" & objShell.ExpandEnvironmentStrings("%USERNAME%") & "\AppData\Local\Programs\Python\Python312\pythonw.exe", _
    "C:\Users\" & objShell.ExpandEnvironmentStrings("%USERNAME%") & "\AppData\Local\Programs\Python\Python313\pythonw.exe" _
)

pythonwPath = ""
For i = 0 To UBound(pythonwCandidates)
    If objFS.FileExists(pythonwCandidates(i)) Then
        pythonwPath = pythonwCandidates(i)
        Exit For
    End If
Next

If pythonwPath = "" Then
    objShell.LogEvent 1, "silent_dispatch.vbs: pythonw.exe not found in any known location"
    WScript.Quit 2
End If

' Build command line: pythonw.exe "scriptPath" arg1 arg2 ...
cmdLine = """" & pythonwPath & """ """ & scriptPath & """"

For i = 1 To objArgs.Count - 1
    ' Always quote args to handle paths with spaces
    cmdLine = cmdLine & " """ & objArgs(i) & """"
Next

' Run with intWindowStyle=0 (SW_HIDE), bWaitOnReturn=True (wait for exit)
' This is the kernel-level CreateProcess flag that hides the window.
exitCode = objShell.Run(cmdLine, 0, True)

WScript.Quit exitCode
