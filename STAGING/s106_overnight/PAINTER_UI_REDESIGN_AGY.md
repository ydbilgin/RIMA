# Tile Painter UI Redesign — Antigravity (Gemini 3 Pro), VISUAL/UX LENS

ACTIVE RULES: (1) think before answering (2) net, no waffle (3) UX-design lens (4) BLOCKED if can't be concrete.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: User mevcut Map Designer Tile Painter sekmesinden memnun değil — "çekilmiyor ayarlanmıyor, daha güzel olsun, kullanıcı dostu olsun". Sen UX/visual hierarchy lens'inden bak, Codex paralel Unity engineering tarafı çalışıyor. Ben (Opus) iki verdict'i sentezleyip implement edeceğim.

## CURRENT STATE
- Mevcut UI screenshot: `STAGING/s106_overnight/painter_v2_user_feedback.png` (USER RECORDED, 1270×780, **READ THIS FIRST**)
- Kod dosyası: `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` (~550 satır, IMGUI EditorWindow)
- Yer aldığı container: `UnifiedMapDesigner.cs` (tab-based wrapper, reflection ile MinimalTilePainter embed eder)

## OBSERVED PROBLEMS (from user screenshot)

1. **Text truncation epidemic:**
   - Side panel grup headerları: "Cobblestone (stone) [", "Cyan Veins (accent) [" — yarıda kesik
   - Main panel: "Theme (click a tile or header in side pan…" — kesik
   - Active: "Active: Cobblestone (stone)  (t…" — kesik
   - Brush size: "1×1 2×2 3×3 4×" — kesik (5×5 görünmüyor)
   - Buttons: "Group (theme batch) / Sin…" — single mode label kesik
   - "Clear Selecte…" — kesik

2. **Side panel verimsiz:**
   - 4 tile kartı 56px × 56px ama tek kolon — yandaki boş alan kullanılmamış
   - Sadece 2 thumbnail görünüyor, scroll lazım — 16 tile için kötü deneyim
   - "Refresh tile assets" butonu altta yarıya kesik

3. **Resize separator:**
   - Kullanıcı "çekilmiyor ayarlanmıyor" diyor — middle separator (sidePanelWidth değişimi) belli değil, fark edilmiyor
   - Kullanılabilirliği zayıf

4. **Visual hierarchy:**
   - Header'lar (Active Tilemap, Paint Mode, Tool, Brush Size) hep aynı boyut/renk — göz kayıyor
   - Accent renk kullanımı zayıf (sadece sol kenarda 4px stripe — yetersiz)
   - "Status" bilgi kutusu dolu/önemli görünüyor ama sadece teknik info

## YOUR JOB

Read the screenshot. Then design a CLEANER, USER-FRIENDLY layout (Unity Editor IMGUI conventions).

### Specific questions to answer:

#### 1. SIDE PANEL LAYOUT
- Kaç sütun thumbnail? (önerim 2-3)
- Theme grup header'ları nasıl: collapsible foldout mu, sticky banner mı, accent only mı?
- Tile thumbnail boyutu ne olsun? (üstüne label gerekiyor mu yoksa hover tooltip mi?)
- Tile sayısı arttığında (16 → 64 → 200) nasıl scale eder?

#### 2. RESIZE HANDLE
- Kullanıcının net hissedeceği bir grip nasıl olur?
- Inspector / Project window'a benzer Unity-native bir yaklaşım var mı?
- Min/max constraint ne olsun?

#### 3. MAIN PANEL CONTROLS
- "Paint Mode", "Tool", "Brush Size" sıralaması doğru mu, yoksa yan yana grup mu?
- Brush size 1×1..5×5 yerine kısa label "1, 2, 3, 4, 5" mi? Slider mı?
- Group/Single toggle daha net olsun (segmented button?)
- Active theme/tile preview nereye konsun? (büyük tile thumbnail + ad?)

#### 4. INFORMATION HIERARCHY
- Boyut: H1 (title) / H2 (section) / H3 (sub) ayrımı nasıl?
- Renk: accent stripe ne kadar geniş, theme renk haritası nasıl daha hissedilsin?
- "Status" info kutusu daha az dikkat çeksin / footer'a gitsin?

#### 5. WINDOW MINIMUM SIZE
- Hangi minimum boyutta hâlâ kullanılabilir kalır? (currently minSize 520×420)
- Ne zaman hangi şeyleri gizler/sıkıştırır? (ör. <600px → side panel collapsible, <400px → tek kolon)

#### 6. KEYBOARD SHORTCUTS
- 1-5 brush size, P/E paint/erase, T theme cycle, S single mode — kullanıcı için akış mı?
- Tooltip metinlerini önerirken net Türkçe / İngilizce hangisi (proje hardcoded Türkçe karışık)

### Deliverable format

Write to: `STAGING/s106_overnight/PAINTER_UI_VERDICT_AGY.md`

Sections:
1. **First impressions** — screenshot okuduktan sonra net 3-4 cümle
2. **Layout proposal** — ASCII wireframe veya component grid
3. **Side panel spec** — sütun sayısı, thumb boyut, grup header style
4. **Main panel spec** — section order + sizing
5. **Resize handle spec** — visual hint, cursor, drag range
6. **Color/typography hierarchy** — H1/H2/H3, accent usage, contrast
7. **Responsive breakpoints** — window size based behavior
8. **Final verdict** — keep / overhaul / specific changes priority order

Max 700 words. Final line: `VERDICT: <KEEP-LAYOUT | INCREMENTAL | OVERHAUL>` + 1 cümle gerekçe.
