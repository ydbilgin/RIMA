# S110 — Room Painter ALL-IN-ONE UX REDESIGN spec

**Agent:** general-purpose Sonnet sub-agent (design + planning, no code)
**Effort:** ~45 dakika (kapsamlı UX redesign spec)

---

## User direktifi (verbatim)

> "Gerçekten UI/UX işlevsellik olarak 4 farklı model ancak bunu mu yapabildiniz daha güzel bi tasarım bulamadınız mı kullanıcının her şeyi yapabileceği hatta rigidbody2body'i unity üzerinden ayarlamak yerine kendi içindeki bi mantıkla ben ayarlasam block koyulacak objelerde de onu unity'e import edince kendi halinde koysa olmuyor mu"

## User eleştirisinin özü

Mevcut Room Painter (Day 1-3 LIVE) **minimum viable** — sekme + asset grid + SceneView click. Designer aşağıdaki için Unity Inspector'a gitmek zorunda kalıyor:
- Rigidbody2D / Collider2D ekleme
- Sorting Layer / Order değiştirme
- Y-sort axis ayarı
- ParallaxLayer factor manuel
- Tag / Layer assignment

**User vizyonu:** Designer Unity Editor'a hiç dokunmasın. Room Painter **tek tool**, her şey içinde. Sprite import edildiğinde otomatik kategorize edilsin (manuel Refresh gerekmesin).

## Hedef — kapsamlı UX redesign

`STAGING/ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md` yaz. **9 bölüm.**

### Bölüm 1: UX vizyon manifestosu (300 kelime)
- Mevcut Day 1-3 ne sunuyor, ne sunmuyor (4 madde her biri)
- "All-in-one designer tool" hedefi — designer Unity Inspector'a **hiç** gitmesin
- 3-pane layout vs. multi-tab layout vs. context-aware floating panel tartışması
- Inspiration: **Tiled, Aseprite, Pyxel Edit, Krita** tek-pencere paradigm

### Bölüm 2: Ana layout (ASCII art mockup)
Pencere düzeni:
```
+----------------------------------------------------------------+
| TOOLBAR: [Tab1] [Tab2] | folder | refresh | Settings | Help    |
+----------------+---------------------------+-------------------+
| ASSET PALETTE  | SCENE PREVIEW MINI-MAP    | INSPECTOR         |
| (sol, 300px)   | (opt: middle, 200px)      | (sağ, 350px)      |
| - thumbnails   | - room overview           | - selected asset  |
| - search/filter|                           |   properties      |
| - drag-drop    |                           | - physics         |
|                |                           | - sorting         |
|                |                           | - parallax        |
+----------------+---------------------------+-------------------+
| STATUS BAR: selected name | layer | sort | tier | hint        |
+----------------------------------------------------------------+
```
**Boyut, hizalama, gizleyebilme** kuralları belirt.

### Bölüm 3: Inspector panel — All-in-one asset properties (KRİTİK, 500 kelime)

Designer seçili asset'i tıklar (palette'ten veya SceneView'da) → sağdaki inspector **canlı** dolar.

Inspector bölümleri (foldout veya tab):

#### 3.A — Identity
- Asset path (read-only), preview thumbnail (128×128), display name override

#### 3.B — Placement
- Default layer (RoomLayer dropdown, override per-instance possible)
- Default sorting layer + order (Day 4'te `RoomLayerData` SO'dan default'lar gelir)
- Y-sort enabled toggle + axis (-Y default for 3/4 top-down)
- Pivot anchor (auto detect from sprite import, manuel override)

#### 3.C — Physics (Rigidbody2D + Collider2D)
- **Block toggle** (yes/no — "Is this a physics blocker?")
- Eğer Yes:
  - Body type dropdown (Static / Dynamic / Kinematic — default Static for env)
  - Collider shape dropdown (Box / Circle / Capsule / Polygon)
  - Size auto-detected from sprite bounds + manuel override
  - Trigger toggle
  - Layer (Unity physics layer) dropdown
- Eğer No: Section gri

#### 3.D — Parallax (sadece Parallax tab aktifken)
- Tier dropdown (FG/Playable/Near/Mid/Far/Skyline/Horizon agy preset)
- Manuel override slider (0.01-1.50) — preset'in dışına çıkmak istersen
- Camera relative checkbox
- Pixel snap toggle

#### 3.E — Visual
- Sprite tint color
- Material override (URP 2D Lit / Unlit / custom)
- Light interaction (cast shadow, receive light, both)

#### 3.F — Metadata (designer convenience)
- Tags (custom strings, multi)
- Notes (designer comment field)

Inspector boş state: "Click an asset in palette or scene to edit"

### Bölüm 4: Auto-import classification (Bölüm 3'ün tetikleyicisi)

Yeni sprite/prefab Assets/Sprites/Environment veya Assets/Prefabs/Environment'a düştüğünde:
- **AssetPostprocessor** subclass — OnPostprocessAllAssets
- Yeni asset detected → RoomPainterAssetScanner.InferLayer çağrıldı
- Asset metadata (RoomPainterAsset SO) otomatik üretilir, klasör `Assets/RoomPainter/AssetMetadata/<filename>.asset`
- SO'da default Identity + Placement + Physics inferred from name keywords ("rigidbody"=yes, "decal"=no, vb.)
- Room Painter window açıksa palette otomatik refresh (Refresh button gizli, defter event-driven)

### Bölüm 5: Block/Physics inference rules (Bölüm 4'ün AI'ı)

Naming keyword → physics default tablosu:
- `wall*`, `cliff*`, `pillar*`, `column*`, `door*` → Block YES, BoxCollider2D, static
- `floor*`, `decal*`, `moss*`, `crack*`, `parallax*` → Block NO
- `enemy*`, `npc*` → Block YES, CapsuleCollider2D, dynamic
- `pickup*`, `item*`, `coin*` → Block NO (trigger YES)
- `prop*` → Block YES default, designer override

Tablo en az 15 keyword.

### Bölüm 6: Drag-drop workflow (Asset palette → SceneView)

Şu anki: palette select → SceneView click. Eklenecek:
- **Drag from palette to SceneView** (Unity DragAndDrop API)
- Drag sırasında ghost preview (mevcut DrawGhost reuse, mouse follows)
- Drop = MouseUp anında PaintCell trigger
- Multi-select palette → drag = randomized scatter (Phase B)

### Bölüm 7: Block/eraser tools

Day 3'te brush yok ama: 
- **Erase mode** (E shortcut) — sahnedeki yerleştirilmiş asset'i siler (ilk hit detection)
- **Pick mode** (P shortcut) — sahnedeki asset'i tıkla → palette'te aynı asset seçilir
- **Box select** (drag with Ctrl) — multi-select edit veya bulk move/delete
- **Lasso** (Phase B nice)

### Bölüm 8: Save/Load + RoomData integration (Day 5 prep)

Window içinde:
- "Save Room" buton → mevcut sahneyi RoomData SO'ya snapshot (PlacementRecord listesi)
- "Load Room" dropdown → existing RoomData asset'lerini listele, seç + sahne yenile (eski yerleşim sil + yenisini place)
- "Export Prefab" → snap'i prefab olarak kaydet
- Auto-save toggle (5 dakikada bir backup)

### Bölüm 9: Day 4-9 yeni roadmap (mevcut planı revize et)

Mevcut plan:
- D4 Layer system + sorting + Y-sort
- D5 Save/Load
- D6-7 polish

Yeni plan (UX redesign için):
- **D4:** Inspector panel + asset metadata SO + auto-import postprocessor + block inference (Bölüm 3+4+5 ana)
- **D5:** Drag-drop + Erase/Pick/Box-select + physics integration (Bölüm 6+7)
- **D6:** Save/Load + RoomData snapshot (Bölüm 8)
- **D7-8:** Polish + parallax slider + auto-cliff edge brush + minimap (eğer zaman varsa)
- **D9:** Docs + demo room recording

## Çıktı

`STAGING/ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md` 9 bölüm full. **2000-3000 kelime** kapsamlı.

Inline rapor (Agent yanıtı, ≤400 kelime):
- 9 bölüm done/not
- TOP 3 critical decisions (örn "inspector pane sabit vs. floating?")
- 3 risk + mitigation
- Codex Day 4 dispatch'i için **handoff özet** (Sonnet plan → Codex implementable form, "Day 4 = X dosyaları yarat, Y signature'lar")

DESIGN ONLY — kod yazma. Mevcut PainterSuite ColliderPainter pattern reuse opportunities not düş. Mevcut RoomPainterAssetScanner.cs Day 2 review'u (`STAGING/s110_phase_a_day2_review_sonnet.md`) ve cliff pivot memory `cliff_pivot_manual_brush_2026_05_26.md` referans al.
