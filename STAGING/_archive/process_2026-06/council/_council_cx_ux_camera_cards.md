ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
İki UX sorununu FEASIBILITY/REUSE lens'inden çöz: (A) pixel-perfect kamerada smooth scroll-zoom, (B) draft kartlarının hover'da titremesini bitirip premium "öne çık" efekti. ANALYSIS ONLY, no edits → CODEX_DONE.md.

## OKU (kod):
- Assets/Scripts/Camera/CameraZoom.cs  (YENİ smooth-zoom: scroll'da PPC disable + orthographicSize sür, ~0.12s sonra PPC re-enable=crisp. Kullanıcı "hâlâ kötü/yanlış" diyor.)
- Assets/Scripts/UI/SkillOfferUI.cs  (3-kart draft; hover jitter; ConfirmPickRoutine — SEÇ tıklaması)
- Assets/Scripts/UI/SkillBarUI.cs  (skill bar; slot 20/16px küçük)
- Assets/Scripts/Skills/DraftManager.cs  (ShowDraft, OnOfferSelected; timeScale=0 draft sırasında)
- Assets/Scripts/Core/DoorTrigger.cs  (REFERANS: press-G prompt pattern — Key.G poll + playerInRange + promptCanvas)
- Assets/Scripts/Core/RewardPickup.cs  (şu an OnTriggerEnter2D OTOMATİK; G-tuşu yapılacak)

## RECON (zaten bulundu — tekrar etme, üstüne kur):
- Hover jitter kaynağı: HoverScale=1.08 tüm kart ROOT'una uygulanıyor + bg-Image VE Button'da AYRI CardJuiceHandler aynı PointerInsideCount'u besliyor → scale raycast-target'ı kursör altından kaydırıyor → enter/exit flicker + PulseGlow + Canvas.overrideSorting churn.
- "Seç çalışmıyor" en olası: draft'ta Time.timeScale=0; ConfirmPickRoutine scaled-time (WaitForSeconds) kullanıyorsa stall → onPick hiç çağrılmaz. DOĞRULA (ConfirmPickRoutine'de WaitForSeconds vs WaitForSecondsRealtime?).
- UI küçük: HUD CanvasScaler 1920×1080 + Match=0(width); kart 180×260, font name14/desc9.

## CEVAPLA (feasibility):
**A — Kamera zoom:**
1. Pixel-perfect (PixelPerfectCamera, refRes 640×360, PPU64) ile TRUE-smooth zoom Unity'de en temiz nasıl yapılır? Mevcut "PPC disable→orthoSize sür→re-enable snap" yaklaşımının somut kusuru ne (settle-pop? CameraFollow ile çakışma? upscaleRT?)? Daha iyi reçete öner (örn. snap'i crisp seviyeye EASE et; ya da PPC'yi hep kapalı tutup pixel-snap'i manuel yap).
2. Bu codebase'de zoom için yeniden kullanılabilir ne var? min/max/default zoom mantıklı değerler (ARPG, top-down 3/4)? Ekran görüntüsünde çok UZAK görünüyor — default daha yakın mı olmalı?
**B — Kart hover + Seç:**
3. uGUI'de jitter'sız hover'ın DOĞRU kurulumu (tek full-size raycast target; raycast-target'ı DEĞİL child-visual'ı scale et; bring-to-front sibling/overrideSorting). Mevcut çift-handler'ı nasıl tek'e indiririz (cerrahi, SkillOfferUI.cs satırları)?
4. "Seç" stall'ı kesin sebebi + minimal fix (unscaled time?). DoorTrigger pattern'iyle RewardPickup'ı press-G yapmak için reuse edilecek tam parçalar.
5. SkillBarUI slot boyutları + draft kart boyut/font: 1920×1080 ref'te okunaklı/ortalı için önerilen değerler (over-engineering'siz).

**C — Görsel asset üretim planı ($imagegen ile üretilecek):**
6. UI'ı güzelleştirmek için $imagegen ile üretilecek asset LİSTESİ + tam boyutlar (px): kart çerçevesi/arkalığı (CardWidth×CardHeight'a oranlı), skill ikonları (SkillBarUI slot + draft kart ikon alanına uygun kaç px?), HUD panel/backing, skill bar slot çerçevesi/hex. Hangileri GERÇEKTEN $imagegen gerektirir, hangileri uGUI (9-slice sprite / düz renk / gradient) ile yeterli (OVER-PRODUCE ETME — minimal ama güzel)?
7. Pack olarak mı tek tek mi üretmeli? On-brand canon (NLM çek: cyan #00FFCC = Rift/mühür enerjisi, void-mor #3A1A4A, warm-orange #E89020 = 2.accent; Act1 Shattered Keep). Üretilen PNG'ler nereye import (path) + hangi UI alanlarına nasıl atanır (file:line)?

Sonucu CODEX_DONE.md'ye yaz. NOT: ÜRETİM bu görevde DEĞİL — sadece PLAN; üretim council sentezinden sonra ayrı cx $imagegen dispatch ile yapılacak.
