# TASK 5 — Director Mode kod-skin (IDE-dock) + global-guard fix (4-6h)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`).

## Bağlam + KARAR (council-kilitli)
Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md` + layout spec `STAGING/_process/2026-06/chatgpt_review_rev2/RIMA_ChatGPT_Review_2026-06-17_REV2/04_DIRECTOR_MODE_REDESIGN.md`. Görsel referans: `.../visuals/director_mode_proposed_layout.png` (AÇ VE İNCELE).
**DirectorMode = runtime UI FACTORY** (`DirectorMode.cs:715-745,2945-2954`), prefab-authored DEĞİL → **factory KODUNDA skin/anchor pass**. PREFAB-MİGRATION YOK, shared-prefab sistemi YOK (council: o tuzak).

## Hedef: "debug-overlay" → "profesyonel runtime editor (IDE-dock)"
Mevcut callback/logic/tab mantığını KORU; sadece görsel kabuk + layout + renk + font değişsin.

### 1. Süslü çerçeve enflasyonunu kaldır
- Tüm-viewport etrafındaki kalın turuncu/mor frame KALDIR. Her butona 9-slice süslü frame UYGULAMA. Ana container'lara 1px slate border yeter. RIMA-chrome sadece başlık/birincil-action'da.

### 2. IDE-dock layout (1920×1080)
- Top app-bar 56px (room/mode/Play-Step-Reset)
- Sol icon-rail 64px (Playtest/Spawn/Class/Stats/Build/Map/Telemetry)
- Sol contextual library 280-320px (arama/filtre/kart)
- **Orta viewport: ekranın ≥%55'i**
- Sağ inspector 320-360px (seçim/stat/transform/validation)
- Bottom status-bar 28-32px (hotkey/tool/cell/undo/error)
- Telemetry: kalıcı kutu DEĞİL, bottom-drawer kapalı-varsayılan

### 3. Renk hiyerarşisi
base `#11131A` · panel `#1B1F28` · raised `#252A35` · border `#343B48` · selection/valid cyan `#55D6E3` (ekranın ≤%15'i) · commit/reset ember `#C8742A` (karar-rengi, her panelin iskeleti DEĞİL) · locked void-purple `#9B5DE5` · text-primary bone `#F2EFE8` · text-secondary `#9AA3B2`.

### 4. Font/spacing (⚠️ SDF-netlik koru — bulanıklaştırma)
Başlık 20-24 · panel-heading 16-18 · body/input 14-16 · status/hotkey **min 12-13px** · control-height 36-40 · 8px spacing (8/16/24/32). Minik pixel-font KALDIR; TMP SDF netliği bozma.

### 5. ⚠️ Global-guard fix (cx-sweep bulgusu)
cx tespit etti: **DirectorMode hâlâ GLOBAL EventSystem guard** kullanıyor (Build #1'in aynı over-broad deseni — Director tıklamalarını/viewport-etkileşimini bloklayabilir). Build'de panel-rect-only'e çevrildi. **Director'da da kontrol et:** tüm-ekran `IsPointerOverGameObject()` viewport etkileşimini engelliyorsa → panel-rect-only'e daralt (Build'deki `IsPointerOverBuildPanel` deseni gibi). Engellemiyorsa raporla.

## Kısıt
- Cerrahi: `DirectorMode.cs` (+ ilgili director UI helper'ları). Spawn/stat/telemetry/Build-Mode ÇEKİRDEĞİNE ve callback'lere DOKUNMA — sadece görsel/layout/guard. Build Mode kendi iso-viewport geometrisini KORUR (Director layout'u onu override ETMEZ).
- Assert 6/6 KORU. git'e DOKUNMA.

## VERIFY (runtime)
Director'ı aç (backquote/full-flow). Screenshot + assert:
- Süslü çerçeve YOK, IDE-dock layout, **viewport ≥%55**, font'lar ≥12px okunur (SDF net), selection/hover/pressed/disabled ayrı.
- Spawn sonrası status-bar count artışı; Stats'ta `physPower=177` görünür; telemetry kapalıyken viewport'u işgal etmez; Play/Paused tek-bakışta belli.
- Director tıklamaları/etkileşimi çalışıyor (global-guard fix sonrası).
- `read_console` 0-error. assert 6/6 PASS.

## ÇIKTI (E1: ≤10 satır)
Evidence + screenshot → `STAGING/_process/2026-06/demo_fix_tasks/DONE_5_director.md`. Dönüşte: değişen dosyalar + layout/renk/font durumu + global-guard sonucu + assert 6/6 + viewport% + console + kalan risk.
