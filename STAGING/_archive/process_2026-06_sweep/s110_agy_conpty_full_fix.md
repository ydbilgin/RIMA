# S110 — agy_dispatch ConPTY child window flash FULL FIX

**Agent:** general-purpose Sonnet sub-agent (mekanik implementation)
**Effort:** ~1.5 saat (1 dosya refactor + 1 hook helper + test)

---

## Active rules

1. Think before coding. Varsayım listele.
2. Min code, no speculation, no refactor beyond listed scope.
3. Surgical — sadece `agy_dispatch.py` dosyasına dokun.
4. BLOCKED if unclear → orchestrator'a sor.

---

## Bağlam

User talimatı (verbatim):
> "agy dispatchte yine window açıp kapanıyor bunu çözmemiş miydik? ... tamamen fixle her şeyi"

**Halihazırda çözülmüş:** `python.exe → pythonw.exe` parent flash (CRLF + Python ordering, bu sabah). Log'da `python.exe relaunched=0` yok.

**Hâlâ duran flash:** `agy.exe` spawn anında `pywinpty` ConPTY allocate eder → Windows `OpenConsole.exe` veya `conhost.exe` child window pop-up. Memory `feedback_agy_dispatch_cmd_wrapper.md` "OS limit, kabul" demişti — user şimdi tamamen fix istiyor.

**Mevcut suppression** (`agy_dispatch.py` line 116-130, re-exec sonrası path için):
```python
_hwnd = _kernel32.GetConsoleWindow()
if not _hwnd:
    _kernel32.AllocConsole()
    _hwnd = _kernel32.GetConsoleWindow()
if _hwnd:
    _user32.SetWindowPos(_hwnd, 1, -32000, -32000, 0, 0, 0x15)
    _user32.ShowWindow(_hwnd, SW_HIDE)
```

Bu **self-console**'u gizliyor. `pywinpty.PtyProcess.spawn()` (line 294) `OpenConsole.exe` child window'u Windows'tan allocate edince **ayrı pencere** — parent gizli olsa bile child kendi başına flash atıyor.

## Strateji (3-katmanlı)

### Katman 1 (zorunlu) — SetWinEventHook ile child window yakalama

`pywinpty.PtyProcess.spawn` çağrısı **etrafında** Windows event hook kur:
- `EVENT_OBJECT_CREATE` (0x8000) için `SetWinEventHook` çağır
- `WINEVENT_OUTOFCONTEXT` mode (system thread'de çalışır, Python GIL safe)
- Callback'te `GetClassNameW(hwnd)` ile pencere sınıfını al
- Class match ederse (`ConsoleWindowClass` / `PseudoConsoleWindow` / `OpenConsole`):
  - `ShowWindow(hwnd, SW_HIDE)` (= 0)
  - `SetWindowPos(hwnd, 1, -32000, -32000, 0, 0, 0x15)` belt-and-suspenders
- Spawn tamamlandıktan sonra `UnhookWinEvent`

**Implementation detayı:**
```python
import ctypes
from ctypes import wintypes

EVENT_OBJECT_CREATE = 0x8000
WINEVENT_OUTOFCONTEXT = 0x0000
WINEVENT_SKIPOWNPROCESS = 0x0002
SW_HIDE = 0

WinEventProcType = ctypes.WINFUNCTYPE(
    None,
    ctypes.c_void_p,   # hWinEventHook
    ctypes.c_ulong,    # event
    ctypes.c_void_p,   # hwnd
    ctypes.c_long,     # idObject
    ctypes.c_long,     # idChild
    ctypes.c_ulong,    # idEventThread
    ctypes.c_ulong,    # dwmsEventTime
)

_HIDE_CLASS_NAMES = {
    "ConsoleWindowClass",
    "PseudoConsoleWindow",
    "OpenConsoleWindow",   # try both possible spellings
    "OpenConsole",
    "Windows.UI.Core.CoreWindow",  # sometimes WinUI3 wrappers
}

def _make_hide_callback():
    user32 = ctypes.WinDLL("user32", use_last_error=True)
    def _cb(hWinEventHook, event, hwnd, idObject, idChild, idEventThread, dwmsEventTime):
        if not hwnd:
            return
        try:
            buf = ctypes.create_unicode_buffer(256)
            user32.GetClassNameW(hwnd, buf, 256)
            cls = buf.value
            if cls in _HIDE_CLASS_NAMES:
                # SWP_NOSIZE(1) | SWP_NOACTIVATE(0x10) | SWP_NOZORDER(4) = 0x15
                user32.SetWindowPos(hwnd, 1, -32000, -32000, 0, 0, 0x15)
                user32.ShowWindow(hwnd, SW_HIDE)
        except Exception:
            pass
    return WinEventProcType(_cb)

# In run_agy_via_pty, before proc = winpty.PtyProcess.spawn(...):
hook = None
callback_ref = None
if sys.platform == "win32":
    try:
        user32 = ctypes.WinDLL("user32", use_last_error=True)
        callback_ref = _make_hide_callback()  # keep strong ref so GC doesn't reap
        hook = user32.SetWinEventHook(
            EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE,
            None,
            callback_ref,
            0, 0,
            WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS
        )
    except Exception:
        hook = None

try:
    proc = winpty.PtyProcess.spawn(argv, env=env, cwd=str(ROOT), dimensions=(40, 200))
    # ... existing read loop ...
finally:
    if hook and sys.platform == "win32":
        try:
            user32.UnhookWinEvent(hook)
        except Exception:
            pass
    callback_ref = None  # release after spawn done
```

**Önemli:** `callback_ref` Python tarafında strong reference olarak tut, hook süresi boyunca GC'lenirse callback çağrılınca Python crash.

### Katman 2 (zorunlu) — Pre-spawn console yeniden gizle

`run_agy_via_pty()` başında, `spawn` öncesi:
- `GetConsoleWindow()` çağır
- Eğer geri dönen HWND visible ise (race condition — domain reload sonrası tekrar göründü olabilir), tekrar `SetWindowPos -32000,-32000` + `ShowWindow(SW_HIDE)`

### Katman 3 (opsiyonel, opt-in) — `--detached` flag

Eğer dispatch UnityMCP gerektirmiyorsa (pure research/text task):
- `--detached` CLI flag ekle
- Bu flag varsa, agy_dispatch'i `subprocess.Popen(["powershell", "Start-Process", ...])` ile detached Job Object içinde başlat
- Çağıran orchestrator için instant return, output redirect dosyaya
- UnityMCP gerektiren task'lerde (default) flag yok, normal akış

**Bu katman task scope dışında — bırak.** Sadece TODO comment ekle, implement etme.

## Yapılacak edit

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\agy_dispatch.py`

### Edit 1: Yeni module-level helpers (winpty import sonrası, line ~152 civarı)

`_make_console_hider_callback()` + ctypes type tanımları (yukarıdaki kod).

### Edit 2: `run_agy_via_pty()` body (line 281-350)

Spawn'ı try/finally ile sar, hook install/uninstall ekle. Mevcut read loop dokunulmaz.

### Edit 3: TODO comment

Function header'a:
```python
# S110 ConPTY child window flash fix:
#   - SetWinEventHook EVENT_OBJECT_CREATE catches OpenConsole/ConsoleWindowClass spawn → SW_HIDE
#   - Pre-spawn re-hide of own console (race-safety)
#   - TODO: --detached flag for non-UnityMCP dispatch via Scheduled Task (Session 0)
```

## Verification — Sonnet tarafından

1. `grep -n "SetWinEventHook\|_make_console_hider_callback\|EVENT_OBJECT_CREATE" agy_dispatch.py` → en az 5 hit
2. `python -c "import ast; ast.parse(open('agy_dispatch.py', encoding='utf-8').read())"` → 0 exit (syntax OK)
3. Edit sonrası dosya CRLF mi LF mi? — sadece agy_dispatch.cmd CRLF gerekiyor, .py LF kalabilir

**Test ÇALIŞTIRMA YAPMA** — orchestrator (Claude) live test edecek.

## Çıktı

Inline raporla (Agent yanıtı, ≤350 kelime):
- Edit 1/2/3 PASS/FAIL + dokunulan satır aralıkları
- Verification çıktıları (grep + ast.parse)
- Eğer pywinpty API exposure beklediğimden farklıysa BLOCKED + ne göründü
- Önerdiğin class name listesi sadece tahmin — eğer Windows reference docs'ta net liste varsa not düş (yine de tahmin listeyi kullan, miss yapsa da SW_HIDE non-targeted pencereye zarar vermez)

**KORUNACAK:**
- Mevcut pythonw self-relaunch logic (line 53-114)
- AllocConsole/SetWindowPos own-console hide (line 116-130)
- swap_account STARTUPINFO+SW_HIDE
- strip_ansi, list_accounts, locks, state — değişmez
- argparse arguments
- run_agy_via_pty read loop iç logic (sadece etrafına try/finally + hook)

**YAPMA:**
- pywinpty fork/patch
- `--detached` implement (sadece TODO comment)
- Scheduled Task script yazma
- subprocess.Popen ile pywinpty replace
