# Task: CodexAuthManager — Add Active-Profile Shim Feature

## Goal
Mevcut `cx` workflow'unu **BOZMADAN** ekstra özellik: kullanıcı `cx switch <profile>` deyince, **sonraki tüm `codex` çağrıları** (codex-plugin-cc'nin slash command'leri dahil) o profili kullansın. Plugin tek-account kısıtı aşılır.

## Hard constraints
- ❌ `cx run <profile> exec ...` davranışı değişmemeli (mevcut tek-shot çalıştırma)
- ❌ `~/.codex/` dizini olan ama profile manager kullanmayan kullanıcılar etkilenmemeli
- ❌ `npm install -g @openai/codex` yeniden çalıştırılınca shim bozulmamalı
- ❌ Mevcut `codex_profile.ps1` fonksiyon imzaları değişmemeli
- ✓ Backward-compat 100%

## Architecture

### Active-profile file
- Path: `%USERPROFILE%\.codex-active-profile`
- Content: tek satır profile adı (örn: `laurethayday`) veya boş/missing = default
- Yazılınca: sonraki tüm `codex` çağrıları bu profile'ı kullanır
- Silinince: default davranış (system `~/.codex/`)

### Shim binary
- Path: `%USERPROFILE%\.codex-profiles\bin\codex.cmd`
- Behavior:
  ```batch
  @echo off
  setlocal
  if exist "%USERPROFILE%\.codex-active-profile" (
    set /p ACTIVE_PROFILE=<"%USERPROFILE%\.codex-active-profile"
    if defined ACTIVE_PROFILE (
      set "CODEX_HOME=%USERPROFILE%\.codex-profiles\%ACTIVE_PROFILE%"
    )
  )
  REM Real codex still at npm location
  "%USERPROFILE%\AppData\Roaming\npm\codex.cmd" %*
  endlocal
  ```
- PATH precedence: `%USERPROFILE%\.codex-profiles\bin` must come **BEFORE** `%USERPROFILE%\AppData\Roaming\npm` in user PATH
- Result: `codex` çağrısı → shim → CODEX_HOME set (varsa) → real codex.cmd

### New cx commands
- `cx switch <profile>` → `.codex-active-profile` dosyasına yazar
- `cx switch` (no arg) → mevcut aktif profile'ı gösterir (read)
- `cx clear` → `.codex-active-profile`'ı siler (default'a döner)
- `cx active` → alias for `cx switch` (no arg)

## Files to modify

### 1. `F:\Antigravity Projeler\CodexAuthManager\codex_profile.ps1`

ADD functions (after `Get-CodexProfileInfo`):

```powershell
function Get-CodexActiveProfileFile {
    return Join-Path $env:USERPROFILE ".codex-active-profile"
}

function Get-CodexActiveProfile {
    $path = Get-CodexActiveProfileFile
    if (Test-Path $path) {
        $content = (Get-Content $path -Raw -ErrorAction SilentlyContinue).Trim()
        if ($content) { return $content }
    }
    return $null
}

function Set-CodexActiveProfile {
    param([Parameter(Mandatory)][string]$ProfileName)
    $safeName = Get-SafeProfileName -Name $ProfileName
    $profileDir = Join-Path (Get-CodexProfileHome) $safeName
    if (-not (Test-Path $profileDir)) {
        Write-Error "Profile '$ProfileName' does not exist at $profileDir"
        return
    }
    $path = Get-CodexActiveProfileFile
    Set-Content -Path $path -Value $safeName -NoNewline
    Write-Host "Active codex profile set to: $safeName"
    Write-Host "Subsequent 'codex' invocations (incl. plugin) will use this profile."
}

function Clear-CodexActiveProfile {
    $path = Get-CodexActiveProfileFile
    if (Test-Path $path) {
        Remove-Item $path -Force
        Write-Host "Active codex profile cleared. Subsequent calls use default ~/.codex/"
    } else {
        Write-Host "No active profile was set."
    }
}

function Show-CodexActiveProfile {
    $active = Get-CodexActiveProfile
    if ($active) {
        $profileDir = Join-Path (Get-CodexProfileHome) $active
        Write-Host "Active codex profile: $active"
        Write-Host "  CODEX_HOME: $profileDir"
        Write-Host "  exists: $(Test-Path $profileDir)"
    } else {
        Write-Host "No active profile. Using default ~/.codex/"
    }
}
```

### 2. `F:\Antigravity Projeler\CodexAuthManager\codex_profile.ps1` — cx command dispatcher

Find the `switch ($Command)` block (or equivalent — should be the main param dispatcher near top of file). Add 3 cases:

```powershell
"switch" {
    if ($Arguments.Count -ge 1) {
        Set-CodexActiveProfile -ProfileName $Arguments[0]
    } else {
        Show-CodexActiveProfile
    }
    return
}
"clear" {
    Clear-CodexActiveProfile
    return
}
"active" {
    Show-CodexActiveProfile
    return
}
```

### 3. NEW FILE: `F:\Antigravity Projeler\CodexAuthManager\codex_shim_install.ps1`

```powershell
# Installs the codex shim at %USERPROFILE%\.codex-profiles\bin\codex.cmd
# and ensures PATH precedence.

$ErrorActionPreference = "Stop"

$shimDir = Join-Path $env:USERPROFILE ".codex-profiles\bin"
if (-not (Test-Path $shimDir)) {
    New-Item -ItemType Directory -Path $shimDir -Force | Out-Null
}

$shimPath = Join-Path $shimDir "codex.cmd"
$shimContent = @'
@echo off
setlocal
if exist "%USERPROFILE%\.codex-active-profile" (
  set /p ACTIVE_PROFILE=<"%USERPROFILE%\.codex-active-profile"
  if defined ACTIVE_PROFILE (
    set "CODEX_HOME=%USERPROFILE%\.codex-profiles\%ACTIVE_PROFILE%"
  )
)
"%USERPROFILE%\AppData\Roaming\npm\codex.cmd" %*
endlocal
'@

Set-Content -Path $shimPath -Value $shimContent -Encoding ASCII -NoNewline
Write-Host "Shim installed at: $shimPath"

# Ensure PATH precedence
$userPath = [Environment]::GetEnvironmentVariable("Path", "User")
$npmPath = "$env:USERPROFILE\AppData\Roaming\npm"
$shimPathDir = $shimDir

if ($userPath -notlike "*$shimPathDir*") {
    # Prepend shim dir to user PATH
    $newPath = "$shimPathDir;$userPath"
    [Environment]::SetEnvironmentVariable("Path", $newPath, "User")
    Write-Host "Added $shimPathDir to user PATH (prepended)"
} else {
    # Ensure shim dir comes before npm dir
    $parts = $userPath -split ';'
    $shimIdx = [Array]::IndexOf($parts, $shimPathDir)
    $npmIdx = [Array]::IndexOf($parts, $npmPath)
    if ($shimIdx -gt $npmIdx -and $npmIdx -ge 0) {
        # Move shim before npm
        $cleanedParts = $parts | Where-Object { $_ -ne $shimPathDir }
        $newPath = ($cleanedParts | ForEach-Object {
            if ($_ -eq $npmPath) { "$shimPathDir;$_" } else { $_ }
        }) -join ';'
        [Environment]::SetEnvironmentVariable("Path", $newPath, "User")
        Write-Host "Reordered PATH: shim dir now precedes npm dir"
    } else {
        Write-Host "PATH already correct (shim precedes npm)"
    }
}

Write-Host ""
Write-Host "IMPORTANT: Open a new terminal for PATH changes to take effect."
Write-Host "Test with: where codex"
Write-Host "  - First result should be: $shimPath"
Write-Host "  - Second result should be: $npmPath\codex.cmd"
```

### 4. `F:\Antigravity Projeler\CodexAuthManager\install_global.ps1`

Append at the end (after existing install logic):

```powershell
Write-Host ""
Write-Host "Installing codex shim for active-profile support..."
& (Join-Path $PSScriptRoot "codex_shim_install.ps1")
```

### 5. `F:\Antigravity Projeler\CodexAuthManager\README.md`

Append section after `## Install`:

```markdown
## Active Profile Mode (codex CLI shim)

After installing the codex shim, you can set an "active profile" that all `codex` invocations will use — including plugin slash commands (e.g. `/codex:rescue` in Claude Code).

### Usage

```powershell
# Set active profile — all subsequent codex calls use it
cx switch laurethayday

# Show current active profile
cx switch
# (or)
cx active

# Clear active profile — return to default ~/.codex/
cx clear
```

### How it works

The shim at `%USERPROFILE%\.codex-profiles\bin\codex.cmd` intercepts `codex` calls. If `%USERPROFILE%\.codex-active-profile` exists, it sets `CODEX_HOME` to the corresponding profile directory before invoking the real codex binary.

### Compatibility

- `cx run <profile> exec ...` still works unchanged (one-shot inline profile)
- If no active profile is set, behavior is identical to vanilla codex (uses ~/.codex/)
- Reinstalling codex via npm does NOT overwrite the shim (different location, PATH precedence)
- The shim is transparent — `codex --version` etc. all forward to the real binary
```

## Tests / acceptance

1. After install, `where codex` shows shim FIRST, npm SECOND.
2. Without `.codex-active-profile`, `codex auth` shows current default account.
3. `cx switch laurethayday` → `.codex-active-profile` file created with content `laurethayday`.
4. `codex auth` after switch → shows laurethayday account info.
5. Open new Claude Code session, `/codex:setup` → shows laurethayday auth.
6. `cx switch laurethgame` → next `/codex:rescue` uses laurethgame.
7. `cx clear` → `codex auth` returns to original default.
8. `cx run yasinderyabilgin exec "echo test"` STILL WORKS (existing path).
9. Uninstall test: delete shim dir, PATH still valid (codex still works via npm).

## Don't do
- Do NOT modify `~/.codex/auth.json` directly (CodexAuthManager's strength is CODEX_HOME isolation)
- Do NOT remove existing functions or change signatures in `codex_profile.ps1`
- Do NOT touch `~/.codex/` for users without profiles
- Do NOT require admin permission (user-level PATH + user-level shim dir only)

## Style
- PowerShell: 4-space indent, brace style follows existing `codex_profile.ps1`
- Batch shim: ASCII encoding, no BOM, CRLF line endings (Windows convention)
- README markdown: existing voice / formatting

## Output
- Commit message: `[CodexAuthManager] Add active-profile shim — cx switch/clear/active commands`
- Include all 5 file modifications in single commit (or split logically if cleaner)
- No tests required (Windows-specific, manual acceptance only)
