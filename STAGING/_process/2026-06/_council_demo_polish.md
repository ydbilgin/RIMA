READ-ONLY brainstorm. Kod/git/PixelLab-gen YOK — sadece FİKİR üret. İzin verilen tek yazma: kendi fikir dosyan.

# Amaç
RIMA demo YARIN (editör-demo). Demo'nun GÖRSEL güzelliğini/cilasını **MİNİMUM PixelLab generation** ile artıracak fikirler. Öncelik: **engine-seviyesi juice + mevcut asset reuse** (yeni sprite üretimi son çare). Her fikir, jürinin EN ÇOK gördüğü yere odaklanmalı.

## Mevcut araçlar/asset (bunları kullan, sıfırdan üretme)
- `SkillVfx` engine katmanı: tint / additive / trail / impact / cast-flash / melee-arc / ground-crack / chain-bolt (Assets/Scripts/VFX/SkillVfx.cs).
- URP 2D Renderer + **2D Lights** + Pixel Perfect Camera; FrameRateGuard (60 cap).
- Mevcut sprite: Warblade + Elementalist (8-yön), tiles (32px), VFX sprite'ları, decal'lar, prop'lar, HUD, F3 log overlay.
- IsoRoomBuilder cliff-island, DraftManager, DirectorMode, HUDController (ShowToast yeni).

## Lens'ler (sana hangi lens verildiyse ONA odaklan, ama ekleyebilirsin)
- **cx (teknik fizibilite):** EN UCUZ uygulanan engine/kod cilası — post-processing/bloom (URP), 2D ışık ayarları, screen-shake/hit-stop juice, mevcut-sprite'la partikül, kamera (zoom/lerp/follow), UI mikro-polish, damage-number/feedback. "Birkaç satırlık kazanç" nerede?
- **ax Pro (görsel/tasarım gözü):** demo'nun görünüşünü EN ÇOK yükseltecek şey — renk/palet/kontrast, ışık/atmosfer, VFX zenginliği, kompozisyon, kamera çerçeveleme, okunabilirlik. Şu an görsel olarak NEREDE zayıf (chamber, combat, boss, centerpiece-editör)? Mümkünse mevcut figürleri/screenshot'ları aç.
- **ax Flash (yalın / etki-başına-efor):** EN yüksek bang-for-buck İLK 5; NEYİ ATLAMALI; demo-eve gerçekçiliği; PixelLab'ı minimumda tut.

## Çıktı
Fikirleri YAZ: `STAGING/_process/2026-06/_council_demo_polish_<lens>.md` (lens = cx / axpro / axflash).
Her fikir için: kısa açıklama + **etiket [engine/kod | asset-reuse | PixelLab-gen:N]** + etki (yüksek/orta/düşük) + efor (saat) + jüri-görünürlüğü. Sonunda **İLK 5 öneri** (öncelik sırası).
Dönüşte (stdout) ≤12 satır: İLK 5 + toplam PixelLab-gen ihtiyacı.
