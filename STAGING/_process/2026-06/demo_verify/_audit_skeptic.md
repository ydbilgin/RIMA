# SKEPTIK AUDIT — Golden-path live-test (bağımsız council lensi)

> Hakem: acımasız skeptik sub-agent. Soru: orchestrator "erken pes mi?" / yanlış-varsayım deseni var mı?
> Kaynak: tool-def (manage_camera), kod-anchor (DirectorMode/BuildModeController/RewardPickup), proje memory, LIVE_TEST_RESULTS.

## VERDICT: KISMEN ERKEN PES. W2 YANLIŞ-VARSAYIM DEĞİL (kanıtlı), W1 AŞIRI-GENELLEME (çözülebilir, denenmeden bırakıldı).

---

## W1 INPUT — "wasPressedThisFrame enjekte edilemez" → ⚠️ AŞIRI-GENELLEME (kırmızı bayrak)
- **Doğru kısım:** consumer'lar (`BuildModeController.cs:180/184`, `RewardPickup.cs:71`, DirectorMode:196) HEPSİ
  `wasPressedThisFrame` (edge) okuyor; `isPressed` (level) yeterli DEĞİL. Bu kod-gerçek.
- **YANLIŞ kısım (erken-pes imzası):** "MCP enjekte EDEMEZ" sonucu, BAŞARISIZ TEK denemeden çıkarıldı — üstelik o deneme
  orchestrator'ın kendi notuyla **kirli state'liydi** (çift-play-session + EnsureOwns key-registry sahiplik kirliliği,
  "elenmedi" diye yazılmış). Kirli-kontrollü bir testten "imkânsız" sonucu = metodoloji hatası.
- **Kök neden = TIMING, imkânsızlık DEĞİL:** `wasPressedThisFrame`, Input System'in bir Update'ten diğerine state-delta'sıdır.
  Orchestrator `execute_code` İÇİNDE QueueStateEvent + InputSystem.Update() çağırdı → edge o manuel Update'te
  doğdu ve consumer'ın bir SONRAKİ player-loop Update'inden ÖNCE tüketildi/sıfırlandı. Edge ile tüketici aynı frame'e
  hizalanmadı. Bu **çözülebilir** bir senkron problemi.
- **DENENMEMİŞ yollar (orchestrator atlamış):** (a) press-event queue → bir gerçek player-frame BEKLE (execute_code'dan
  çık, editor bir Update geçirsin) → consumer kendi Update'inde edge'i görür; (b) `InputSystem.settings.updateMode`
  kontrolü + `onAfterUpdate` hook'a press enjekte; (c) EnsureOwns kirliliğini TEMİZLE (tek temiz play-session) sonra
  tekrar dene; (d) en pragmatik: `BuildModeController.Update`'in F2 dalı zaten `Toggle()` çağırıyor — gerçek-tuş-yolunu
  test için kısa süreli bir test-shim/`#if` ile edge'i besleyen kontrollü repro. Hiçbiri denenmedi → "imkânsız" damgası erken.

## W2 SCREENSHOT — "overlay screenshot'a çıkmaz" → ✅ DOĞRU (orchestrator'ın LEHİNE; brief'teki şüphe YANLIŞ)
- Brief "manage_camera tanımı ALL layers diyor → W2 muhtemelen yanlış" diyor. **Bu çıkarım hatalı.** Tool-def gerçekten
  "no camera → ScreenCapture API, includes Screen Space-Overlay" diyor AMA bu **genel Unity davranışı**, RIMA'da test edildi
  ve ÇÖKTÜ: memory `feedback_screenspaceoverlay_not_in_screenshot` (2026-06-13 TEYİT) — MCP camera-suz screenshot Director
  overlay'i (Canvas_DirectorOverlay, ScreenSpaceOverlay sort 950) YAKALAMADI; "Main Camera path'ine düştü", sadece dünya çıktı;
  **geçici ScreenSpaceCamera swap'i BİLE çıkarmadı** (ortografik 5.668 + scaler uyumsuzluğu). Bu eski varsayım değil, kanıtlı bulgu.
- Yani: tool-def teorik doğru, RIMA-pratiği aksini KANITLADI. Orchestrator overlay'i data-proof'a almakta HAKLI.
  Burada erken-pes YOK; aksine doğru memory'ye dayandı. (Brief'in kendisi yanlış-varsayım yapıyor.)

## DIRECTOR MODE "OKUNABİLİR DEĞİL" BUG'I — orchestrator ne YAPMALIYDI (yapmadı)
Kullanıcı raporu "okunabilir değil" = render/kontrast/scale/font sorunu. Overlay screenshot'a çıkmadığı için orchestrator
"kullanıcıya bakar" dedi — AMA okunabilirliğin BÜYÜK kısmı MCP ile data-proof edilebilirdi, yapılmadı:
1. **scene_view force-capture:** overlay ScreenSpaceOverlay → game-view'a çıkmaz, AMA canvas'ı geçici ScreenSpaceCamera
   yap + scene_view capture YA DA contrast'ı GEOMETRİK ölç (aşağı). (Swap riskli ama denenebilir, memory'de "riskli" yazıyor, "imkânsız" demiyor.)
2. **Kontrast = HESAPLA, gözleme:** execute_code ile TMP renk + arka-plan Image rengi oku → WCAG luminance kontrast oranı
   hesapla (DirectorMode:711 dimmer alpha 0.35, panel renkleri kodda). "Okunabilir mi" = sayısal eşik, gözle değil.
3. **Scale/taşma:** her TMP `fontSize` + `RectTransform.rect` + `preferredWidth/Height` + `enableAutoSizing` + textInfo.lineCount
   oku → küçük font / taşma / tek-şeride-çökme tespit edilir (B3 UI-01'de bunu yapıyor, Director paneline UYGULAMADI).
4. **Font-null/garbled:** her TMP `.font` null mı + atlas alt-asset var mı (Jersey10 garbled kök-neden kuralı) → garbled = data-proof.
5. **Reference-res mismatch:** scaler referenceResolution 1920x1080, match 0.5 — gerçek game-view (memory: 1111x710/1441x650)
   ile oran → UI gerçek-boyutta küçülüyor olabilir; bu HESAPLANIR. "Okunabilir değil" çoğu zaman = scaler/res mismatch, font değil.
=> Orchestrator okunabilirlik bug'ı için TEK bir geometrik/kontrast probe çalıştırmadan kullanıcıya devretti = erken-pes #2.

## TEST SENARYOSU — mantık hatası / eksik beat
- ✅ Plan GÜÇLÜ: bypass-tuzakları (ForceCollect/ForValidation), beat-sıra gerekçesi, REWARD-02 dersi, alpha>activeInHierarchy ayrımı hepsi sağlam.
- ⚠️ **Eksik #1:** B4 (telemetry) + B5 (room/F1) "input bloklu → ÇALIŞTIRILAMADI" → W1 erken-pes bu iki beat'i de düşürdü.
  W1 timing-fix denenseydi bu beat'ler de koşabilirdi. İki ⏳ doğrudan W1'in maliyeti.
- ⚠️ **Eksik #2:** B3 "playerInRange true OLUR" diye GREEN dedi ama gerçek-G basılamadığı için
  `CheckInitialPlayerOverlap`→draft→3-kart ZİNCİRİ runtime'da koşmadı; predikat-canlı ≠ akış-canlı. Doğru etiketlenmiş (kullanıcıya) ama "GREEN" kelimesi iyimser.
- ⚠️ **Eksik #3:** Director "okunabilirlik" hiçbir beat'in DATA-PROOF'unda kontrast/scale ölçümü olarak yok (sadece B0
  fontNull + emptyTxt sayıyor, kontrast/fontSize/res-mismatch yok). Kullanıcı-raporlu bug için ölçüm-beat eksik.

## "Kullanıcıya attım" gerçekten zorunlu muydu? — kısmi HAYIR
- Estetik "güzel mi" (B0) → EVET kullanıcı (öznel, overlay görsel). Meşru.
- Fiziksel oynanış HİSSİ (combo-feel) → EVET kullanıcı. Meşru.
- Ama: F2/G/`/ESC toggle DOĞRULAMASI (W1) ve Director OKUNABİLİRLİK (kontrast/scale/font) → HAYIR, MCP ile data-proof
  edilebilirdi; denenmeden kullanıcıya devredildi. Bunlar "atıldı", "yapılamadı" değil.

## ORCHESTRATOR'IN YAPMASI GEREKENLER (öncelik sırası)
1. W1'i ÇÖZ: temiz tek-play-session (EnsureOwns kirliliğini ele) → press-queue + BİR gerçek player-frame bekle → consumer Update'inde
   wasPressedThisFrame yakalanır mı re-test et. "İmkânsız" damgasını kaldır; çözülürse B4+B5+B3-akış+F2/`/G/ESC fiziksel-yol koşar.
2. Director okunabilirlik DATA-PROOF beat'i ekle: her TMP fontSize + rect/preferred + lineCount + .font-null + TMP-renk↔panel-renk
   WCAG kontrast oranı + scaler-res mismatch hesabı. Bug'ı sayısal kanıtla, kullanıcıya gözle-bak deme.
3. B3'ü "predikat-GREEN, akış-PENDING" diye AYIR; "GREEN" tek-kelimesini akış koşmadan kullanma.
4. W2'yi koru (data-proof doğru) AMA brief'teki "tool-def ALL layers diyor" yanılgısını kayda geç: RIMA'da kanıtlı çıkmıyor.
5. Genel refleks: bir yol "yapılamaz" demeden ÖNCE ≥2 alternatif + temiz-state tek-deneme; tek kirli-test'ten "imkânsız" çıkarma.
