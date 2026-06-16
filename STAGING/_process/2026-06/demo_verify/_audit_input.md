# W1 INPUT-INJECTION AUDIT — 2026-06-16

> Bagimsiz denetim: orchestrator'in "MCP wasPressedThisFrame enjekte EDEMEZ -> tum tus-simulasyonu imkansiz"
> duvarini (W1) Unity new Input System uzmani gozuyle dogrula. Sonuc: **W1 YANLIS (erken pes).**

---

## VERDICT: W1 YANLIS — enjeksiyon MUMKUN

execute_code'dan basilan bir tusun GAME loop'una `wasPressedThisFrame=true` olarak ulasmasi
**MUMKUN**. Orchestrator'in kaniti hatali bir TEK-shot deneyden geldi; teknigi yanlistı, imkan degil.

---

## KOK NEDEN: `wasPressedThisFrame` = FRAME-KENARI semantigi

`wasPressedThisFrame`, kontrolun **onceki input-update'te basili-DEGIL, bu update'te basili** olmasini
ister. Yani iki ARDISIK update'e ve aralarinda bir durum-gecisine ihtiyac duyar:
1. (zemin) released durum islensin -> previous-frame = not-pressed
2. pressed state event queue + InputSystem.Update -> bu update'te "press" -> `wasPressedThisFrame=true`

Orchestrator'in deneyi: tek QueueStateEvent(pressed) + tek Update. Sonuc `isPressed=True` (durum dogru
aktarildi) ama `wasPressedThisFrame=False` cunku "previous = not-pressed -> current = pressed" gecisi
TEK update icinde olusturulmadi (ya previous zaten pressed sayildi ya da edge tek update'te yutuldu).
Bu **beklenen davranis**, koprunun limiti DEGIL. (Unity Discussions "QueueStateEvent lifecycle" +
"WasPressedThisFrame not working" + Input System Testing docs ile dogrulandi.)

## RIMA-SPESIFIK KRITIK AVANTAJ
RIMA tum toggle/interact tuslarini **dogrudan cihaz polling** ile okuyor (`Keyboard.current.f2Key.
wasPressedThisFrame`, `keyboard[Key.G].wasPressedThisFrame`, `backquoteKey...`) — **InputAction binding
DEGIL.** Webdeki cogu "wasPressedThisFrame false" sikayeti InputActionReference/binding bug'i; RIMA o
sinifa GIRMIYOR. Dogrudan polling tam olarak QueueStateEvent+Update'in surdugu yol. ✅ ZATEN KANITLI
PRESEDAN repoda: `Assets/Tests/PlayMode/Phase1Demo/T2_GateFlowTest.cs:191-193` -> InputTestFixture
`Press(kb.gKey); yield return null; Release(kb.gKey);` ile `MapFragment.Update`'in
`gKey.wasPressedThisFrame`'ini GERCEKTEN tetikliyor. F2/ESC/backquote/LMB ayni mekanik.

## EnsureOwns/key-registry KIRLILIGI testi NASIL BOZDU (ayri eksen)
Orchestrator'in F2-toggle-etmedi gozlemi **enjeksiyon degil sahiplik** sorunu. Cift-play-session
(DisableDomainReload) -> 2 canli BuildModeController. `InPlayToolKeyRegistry.RegisterExclusive`
(satir 50-56) ikinci canli sahibi REDDEDER -> `EnsureOwns(Key.F2)` **false** (BuildModeController.cs:180)
-> F2 KUSURSUZ enjekte edilse bile Toggle() cagrilmaz. **Temiz tek-session'da (oncesinde Stop + sahne
reload + gerekirse `InPlayToolKeyRegistry.ClearAll()`) F2 enjeksiyonu CALISIR.** W1'in F2 ayagi
elenmemis confound; W1 kaniti gecersiz.

## ONERILEN execute_code SNIPPET (F2; G/ESC/backquote ICIN Key sabitini degistir)
Tek-shot body, sonraki oyun-frame'ine guvenmez — iki update'i manuel pompalar:
```csharp
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
var kb = Keyboard.current; double t = Time.realtimeSinceStartupAsDouble;
// 1) released zemin -> previous = not-pressed
InputSystem.QueueStateEvent(kb, new KeyboardState(), t);            InputSystem.Update();
// 2) pressed -> bu update'te edge -> wasPressedThisFrame=true (GAME Update bunu ayni frame okur)
InputSystem.QueueStateEvent(kb, new KeyboardState(Key.F2), t+0.01); InputSystem.Update();
// (Toggle bu Update sirasinda BuildModeController.Update tarafindan okunur)
InputSystem.QueueStateEvent(kb, new KeyboardState(), t+0.02);       InputSystem.Update(); // release
return $"IsActive={RIMA.BuildModeController.IsActive}";
```
ALTERNATIF (daha saglam, MCP cok-cagrili): InputTestFixture/T2 deseni — Press(kb.f2Key) yap, BIR
oyun-frame BEKLE (yield/sonraki MCP cagrisi), sonra Release; for MCP, "press queue + Update" cagrisini
"oku" cagrisindan AYRI tut. LMB icin `new MouseState{ position=..., buttons=...}` + iki update; pointer
pozisyonu UI-uzeri/raycast icin gercek dunya/screen koordinati ver. Not: timeScale=0 (Director/pause)
iken cihaz event'i islenir ama oyun-Update kosmayabilir -> toggle okumasi icin Test state/timeScale=1.

## SONUC
- W1 = **YANLIS.** Imkansizlik yok; sadece iki-update / frame-kenari teknigi + temiz tek-session sart.
- Demo live-test'te ESC/F2/G/backquote/LMB **gercek-input olarak enjekte edilebilir** (bypass'a gerek yok).
- Sart: (a) iki-update press/release deseni, (b) timeScale=1/Test state okuma, (c) cift-session yok +
  gerekirse `InPlayToolKeyRegistry.ClearAll()`, (d) RIMA dogrudan-polling oldugu icin InputAction bug'i
  ENGEL DEGIL.
