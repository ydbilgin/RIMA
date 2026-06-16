# DEMO-VERIFY METHODOLOGY — CORRECTED (2026-06-16)

> Bagimsiz 3-lens denetim sentezi (input / capability / skeptic) + CANLI KOD TEYIDI.
> Sonuc: orchestrator IKI duvarda da ERKEN PES etti. Ikisi de **calisilmadan** "imkansiz" damgalandi.
> Bu dosya = duzeltilmis metodoloji + tam teknik + snippet'ler. Kaynak: `_audit_input/_capability/_skeptic.md`
> + canli kod: unity-mcp **9.7.3** `ScreenshotUtility.cs` / `ManageScene.cs` / `DirectorMode.cs` / `T2_GateFlowTest.cs`.

---

## TL;DR VERDICT
- **W1 (input enjekte edilemez) = YANLIS.** Enjeksiyon MUMKUN. Orchestrator'in kaniti tek-shot + kirli-state
  (cift-play-session + EnsureOwns key-registry) deneyden geldi; teknigi yanlisti, imkan degil. Repoda KANITLI
  presedan var: `Assets/Tests/PlayMode/Phase1Demo/T2_GateFlowTest.cs:191-193` InputTestFixture `Press/Release`
  ile `wasPressedThisFrame`'i GERCEKTEN tetikliyor.
- **W2 (overlay screenshot'a cikmaz) = UNRESOLVED / kanit-eksik (orchestrator yine de erken pes etti).**
  9.7.3'te YENI bir composited yol var (`CaptureCompositedAfterFrame` + `WaitForEndOfFrame`) ve canli kodda
  AKTIF — RIMA'da bu yol HIC temiz test EDILMEDI. Eski "cikmiyor" memory'leri (2026-06-09/06-13) **camera-render
  fallback** yolundan geldi, bu composited yoldan DEGIL. ⇒ W2 ne "kesin dogru" ne "kesin yanlis"; **3 dakikalik
  canli denek** ile cozulur. Bypass: data-proof her hal-de gecerli birincil yol; screenshot = bonus teyit.

---

## (1) INPUT ENJEKTE EDILEBILIR Mi? → EVET. Tam teknik.

### Kok neden (orchestrator neyi kacirdi)
`wasPressedThisFrame` = FRAME-KENARI semantigi: "onceki input-update'te basili-DEGIL, BU update'te basili".
Iki ARDISIK update + aralarinda durum-gecisi ister. Orchestrator tek `QueueStateEvent(pressed)` + tek `Update()`
yapti → `isPressed=True` (durum aktarildi) ama edge dogmadi → `wasPressedThisFrame=False`. **Beklenen davranis,
koprunun limiti DEGIL.**

### RIMA-spesifik AVANTAJ (neden web sikayetleri gecersiz)
RIMA tum toggle/interact tuslarini **dogrudan cihaz polling** ile okuyor
(`Keyboard.current.f2Key.wasPressedThisFrame`, `keyboard[Key.G]...`, `backquoteKey...`) — InputActionReference
binding DEGIL. Webdeki "wasPressedThisFrame false" sikayetlerinin cogu binding-bug'i; RIMA o sinifa GIRMIYOR.
✅ Presedan: `T2_GateFlowTest.cs:191-193` `Press(kb.gKey); yield return null; Release(kb.gKey);` →
`MapFragment.Update`'in `gKey.wasPressedThisFrame`'ini gercekten tetikliyor. F2/ESC/backquote/LMB AYNI mekanik.

### YONTEM A — iki-update tek-shot (execute_code, en hizli; MCP tek cagrida toggle)
Sonraki oyun-frame'ine guvenmez; iki update'i manuel pompalar. F2 ornegi (G/ESC icin `Key` sabitini degistir):
```csharp
using UnityEngine.InputSystem; using UnityEngine.InputSystem.LowLevel;
var kb = Keyboard.current; double t = Time.realtimeSinceStartupAsDouble;
InputSystem.QueueStateEvent(kb, new KeyboardState(), t);             InputSystem.Update(); // released zemin -> previous=not-pressed
InputSystem.QueueStateEvent(kb, new KeyboardState(Key.F2), t+0.01);  InputSystem.Update(); // edge -> wasPressedThisFrame=true, BuildModeController.Update bu Update'te okur
InputSystem.QueueStateEvent(kb, new KeyboardState(), t+0.02);        InputSystem.Update(); // release (sonraki toggle icin temiz birak)
return $"IsActive={RIMA.BuildModeController.IsActive}";
```
> NOT: consumer'in `Update`'i `InputSystem.Update()` ICINDE (player-loop'ta) kosar; manuel-pump deseni edge'i
> consumer ile ayni update'e hizalar. timeScale=0 (Director/pause) iken cihaz event'i ISLENIR ama oyun-Update
> kosmayabilir → toggle/read icin **Test state / timeScale=1**.

### YONTEM B — InputTestFixture deseni (en saglam; cok-cagrili MCP)
`press-queue + Update` cagrisini "oku" cagrisindan AYIR: press yap → BIR gercek player-frame BEKLE (execute_code'dan
cik / sonraki MCP cagrisi) → consumer kendi Update'inde edge'i gorur → release. T2 testinin birebir deseni.

### LMB / fare
`new MouseState{ position=<screen px>, buttons=<left bit> }` + iki update. Pointer pozisyonu UI-uzeri/raycast
icin **gercek dunya/screen koordinati** ver (Build place: UI-DISI hucre; combat: dusman uzeri).

### ZORUNLU on-kosullar (orchestrator'in ELENMEDI dedigi confound)
1. **Tek temiz play-session** — cift-session (DisableDomainReload) = 2 canli BuildModeController →
   `InPlayToolKeyRegistry.RegisterExclusive` ikinciyi REDDEDER → `EnsureOwns(Key.F2)=false`
   (`BuildModeController.cs:180`) → F2 kusursuz enjekte edilse bile `Toggle()` cagrilmaz. Gerekirse
   `InPlayToolKeyRegistry.ClearAll()`.
2. **timeScale=1 / Test state** okuma anında (pause'da oyun-Update kosmaz / hasar islemez).
3. Press → BIR frame → Release sirasi (level-takilma onler).

---

## (2) SCREENSHOT ile GORSEL-DOGRULANABILIR BEAT'LER

### W2 gercegi (canli kod, 9.7.3 — tartismayi BITIR)
- `ManageScene.cs:613-621`: camera VERME + `include_image:true` + `Application.isPlaying` → `CaptureComposited(...)`
  (camera-render YOLU DEGIL).
- `ScreenshotUtility.cs:218-223`: play-mode'da `CaptureCompositedAfterFrame` → `ScreenshotCapturer` MB
  `yield return new WaitForEndOfFrame()` (`:761-768`) SONRA `ScreenCapture.CaptureScreenshotAsTexture`.
  = "backbuffer'i composite olmadan okuma" bug'ini cozer. **Bu yol RIMA'da HIC test edilmedi.**
- Eski "cikmiyor" memory'leri (2026-06-09 chamber prompt, 2026-06-13 Director overlay) **camera-render fallback**
  yolundan (cameralı / "Main Camera path") — composited yoldan DEGIL. ⚠️ Docstring dar: "UI Toolkit overlays" der;
  RIMA uGUI (ScreenSpaceOverlay + TextMeshProUGUI) — bu yolun uGUI overlay'i yakalayip yakalamadigi **AMPIRIK
  belirsiz**. ⇒ once 3-dk denek, sonra karar.
- ❗ Tuzak: **camera parametresi verirsen** camera-render → overlay HARIC (eski davranis, tool-def dogru).
  Composited yol icin camera VERME.

### 3-DAKIKALIK W2 DENEK (orchestrator ILK bunu kosmalı)
`_Arena` Play → backquote ile Director overlay AC (ScreenSpaceOverlay gorunur durum) →
`manage_scene`/`manage_camera action=screenshot` **camera YOK + include_image:true** → donen base64'e BAK:
overlay var mi? VAR ise W2 yanlis → asagidaki beat'leri gorsel teyit et. YOK ise W2 dogru (composited bile uGUI
overlay'i atliyor) → data-proof'ta KAL, kullaniciya juri-provasi birak. **Iddia degil, denek.**

### Composited cikar ISE gorsel-dogrulanabilir beat'ler (data-proof'a EK)
| Beat | Gorsel imza |
|---|---|
| B0 panels estetik | Pause/Settings/Codex/HUD "guzel mi", tasma, font garbled |
| B3 UI-01 footer | Desc dikey serite cokuyor mu (rect/lineCount proxy'sine ek GORSEL) |
| B7 director-bleed | A1 overlay gorunur / A2 Test'e gecince overlay yok — composited SS FARKI direkt gosterir |
| B6 BuildPaletteCanvas | panel render + reward-spawn dunya gorseli |
- ⚠️ SS ≠ INPUT: screenshot "panel acildiginda nasil gorunur"u kanitlar; tetigi (F2/G/ESC) **basmaz**. State'i
  execute_code+input ile KUR, sonra SS ile estetik teyit et.

### Dunya-uzayi (her zaman calisir, W2'den BAGIMSIZ)
Player/silah/oda/reward-chest/prop = `capture_source=scene_view` GUVENILIR (game-view RT limitine takilmaz).

---

## (3) ORCHESTRATOR'IN DUZELTILMIS METODOLOJISI

### MCP ile GERCEKTEN yapilabilen (orchestrator'in "yapamam" dedikleri)
| Yetenek | Yontem | Eski yanlis kabul |
|---|---|---|
| Gercek tus enjekte (F2/G/ESC/backquote) | YONTEM A/B (iki-update / TestFixture) + temiz session | "wasPressedThisFrame enjekte edilemez" ❌ |
| Gercek fare/LMB | `MouseState` + iki update, gercek koordinat | combat/place simule edilemez ❌ |
| Overlay gorsel teyit | camerasiz + include_image:true + Play composited (ONCE 3-dk denek) | "overlay SS'a cikmaz" (kanitsiz genelleme) ⚠️ |
| Director okunabilirlik | KONTRAST/scale/font SAYISAL probe (asagi) | "okunabilir mi gozle bak, kullaniciya" ❌ |
| Force-render dunya | scene_view pivot/size + Repaint + capture | (dogru kullaniliyordu) ✅ |
| Wiring/state/precondition | execute_code + `*ForValidation` read-back | (dogru kullaniliyordu) ✅ |
| Telemetry/damage/reward sayac | gercek-input combat → `Telemetry*ForValidation` / `RunStats` | input bloklu sanildi → B4/B5 dustu ❌ |

### GERCEKTEN kullanici-gereken MINIMUM (mesru devir)
- **Estetik "guzel mi" oznel yargi** (panel zevki, renk hissi) — composited SS yardimci ama nihai = juri-provasi insan.
- **Fiziksel oynanis HISSI** (combo-feel, juice, responsiveness) — insan elinde.
- **Gercek donanim klavye/fare end-to-end** (OS → Unity tam zincir) — son guvence; AMA wiring+enjekte-input ile
  %95 once dogrulanir, kullanici sadece "evet hissi de iyi" der.
> Kural: bir yol "yapilamaz" demeden ONCE ≥2 alternatif + temiz-state tek-deneme. Tek kirli-test'ten "imkansiz"
> CIKARMA. "Atildi" ile "yapilamadi"yi karistirma.

### Yeniden-kosulacak beat'ler (W1 cozumuyle acilir)
B2 (LMB combat empirik delta), B3 (gercek G→3-kart akis-canli), B4 (telemetry), B5 (room-transition/F1 leak),
B6 (gercek F2+click place), B7 (gercek backquote + bleed). Hepsi YONTEM A/B + temiz session ile kosulabilir.

---

## (4) DIRECTOR-READABILITY FIX YAKLASIMI

### Once: bug'i SAYISAL kanitla (gozle DEGIL) — bu probe orchestrator'da YOKTU
Kullanici "okunabilir degil" = render/kontrast/scale/font. Hepsi execute_code ile olculur:
1. **WCAG kontrast HESAPLA:** her TMP `.color` + arka-plan Image `.color` oku → relative luminance → kontrast orani.
   Anchor'lar (`UI/DirectorMode.cs`): ScreenDimmer `Color(0.031,0.027,0.063,0.35)` (`:711`); TabRail
   `(0.05,0.06,0.08,0.80)` (`:795`); hint/meta text `(0.78,0.84,0.86,~0.70-0.72)` (`:866,:889,:943`); TopBadge
   `(0.48,0.18,0.06,0.92)` (`:775`). Dusuk-alpha panel uzeri dusuk-alpha gri text = dusuk kontrast → SAYI ile kanit.
2. **Scaler-vs-actual-res mismatch:** `Canvas_DirectorOverlay` (`:694`) CanvasScaler referenceResolution
   `1920x1080` + match `0.5` (`:704-706`). Gercek game-view (memory: ~1111x710 / 1441x650) ile oran hesapla →
   UI gercek-boyutta KUCULUYOR olabilir = "okunamiyor"un en olasi kok-nedeni (font degil, scaler).
3. **Font-scale/tasma:** her TMP `fontSize` + `enableAutoSizing` + `RectTransform.rect` + `preferredWidth/Height`
   + `textInfo.lineCount` → kucuk-font / tasma / tek-serite-cokme.
4. **Font-null/garbled:** her TMP `.font` null mi + Jersey10 atlas alt-asset var mi (garbled kok-neden kurali).

### Sonra: fix yaklasimi (kanit yonlendirir)
- **Mismatch ise (en olasi):** dimmer alpha 0.35→~0.55-0.65 (panel arka kontrasti) VEYA panel Image alpha ↑
  VEYA text alpha 1.0'a cek (0.70-0.86 yerine) → WCAG ≥4.5:1 hedefle. Scaler match'i icerik-yogunluguna gore
  ayarla. fontSize tabani ↑ / autosize min-max daralt.
- **Garbled ise:** Jersey10 SDF rebake (atlas+material alt-asset) — kok-neden kurali, fallback DEGIL.
- Demo-scope: en kucuk cerrahi degisiklik (alpha/fontSize), no-refactor. Fix sonrasi 3-dk W2 denek geçerse
  composited SS ile gorsel before/after; gecmezse kontrast SAYISI before/after.

---

## AKSIYON SIRASI (orchestrator)
1. **3-dk W2 denek** (camerasiz composited SS, overlay acikken) → W2'yi LIFE/duvar olarak KESINLESTIR.
2. **Temiz tek-play-session** (EnsureOwns/ClearAll) → YONTEM A ile F2 enjekte → IsActive toggle dogrula (W1 cozum kaniti).
3. W1 calisinca B2/B3-akis/B4/B5/B6/B7 GERCEK-input ile yeniden kos (data-proof + W2 gecerse gorsel).
4. Director-readability SAYISAL probe (kontrast+scale+font+res) → bug'i sayiyla kanitla → cerrahi fix.
5. LIVE_TEST_RESULTS'taki "MCP enjekte EDEMIYOR / overlay cikmaz" mutlak ifadelerini bu bulgularla guncelle.
