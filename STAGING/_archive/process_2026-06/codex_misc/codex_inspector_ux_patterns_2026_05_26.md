# Inspector/Property Editor UX patterns — practical patterns for RIMA Room Painter

## Mission
Sang Hendrix'in "Realtime Parallax Map Builder for RPG Maker MZ" plugin'i ve benzer 2D map editor'lerin inspector/property editor pattern'lerini analiz et. RIMA Unity Editor "Room Painter" tool'unun inspector'ını daha işlevsel hale getirmek için **uygulanabilir kod-seviyesinde** öneriler üret.

## Sources (research her birini)
1. https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin (ana referans — sayfadaki tüm videoları izle, frame analizi yap, ffmpeg vs gerekirse)
2. Tiled Map Editor properties panel — https://doc.mapeditor.org/en/stable/manual/custom-properties/
3. LDtk inspector — https://ldtk.io/docs/ (Entity instance fields, Field definitions panel)
4. Unity 2D Tilemap Editor + RuleTile inspector
5. Godot TileMap inspector + TileSet editor
6. GameMaker Room Editor — Layer Properties panel
7. Aseprite Layer panel + properties

## RIMA mevcut Room Painter (oku ve referans al — dispatch sırasında)
**Konum:** `Assets/Editor/RoomPainter/`

**Dosyalar:**
- `RimaRoomPainterWindow.cs` — main EditorWindow, 3-pane layout (palette | preview | inspector), splitter drag, asset cache, scene placer dispatch
- `Inspector/RoomPainterInspectorPanel.cs` — hero card + 6 colored section bands (Identity/Placement/Physics/Parallax/Visual/Metadata)
- `Inspector/Sections/IdentitySection.cs` — displayName, path readonly, GUID, Ping/Reveal butonları
- `Inspector/Sections/PlacementSection.cs` — layer enum, sortingLayer popup, order int, ySort toggle, ySortAxis, pivotAnchor Vector2, scale, visualOffset
- `Inspector/Sections/PhysicsSection.cs` — isBlock/isTrigger toggle, bodyType, colliderShape, colliderSize Vector2, physicsLayer popup, mini hitbox preview canvas, Fit/Tight/Half/Square quick actions
- `Inspector/Sections/ParallaxSection.cs` — Tier dropdown (Custom + 7 preset), Custom override slider 0.01-1.50, cameraRelative, pixelSnap
- `Inspector/Sections/VisualSection.cs` — tint, material override, castShadow, receiveLight, Apply Visual button
- `Inspector/Sections/MetadataSection.cs` — tags list, notes
- `Preview/RoomPainterPreviewPane.cs` — live preview canvas, zoom (Fit/1x...8x), 3D mock toggle, pan, rotate
- `Preview/PreviewSpriteRenderer.cs` — sprite/prefab pivot placement
- `Preview/PreviewOverlayRenderer.cs` — shadow, cliff ramp, parallax tint, pivot, y-sort, dashed bounds
- `AssetPipeline/RoomPainterAssetPostprocessor.cs` — auto-create RoomPainterAsset SO on import + on-demand
- `Helpers/RoomPainterAssetScanner.cs` — layer keyword inference

**Karar gerekçesi:** Şu an asset-bazlı inspector. Yani palette'ten bir sprite/prefab seçince onun RoomPainterAsset SO field'larını editliyor. Sahnedeki instance'a apply için ayrı buton var. Sang Hendrix'in plugin'inde muhtemelen layer-bazlı manager + drag-reorder + live preview senkronu var — bu pattern'i adapte etmek mantıklı mı, yoksa asset-centric model'imize sadık kalmalı mıyız?

## Bulmamı istediklerim

### Bölüm 1 — Sang Hendrix plugin'inden 5-7 adapte edilebilir pattern
Sayfadaki videolardan kareler çek (varsa ffmpeg, yoksa manuel açıklama). Her pattern için:
1. **Pattern adı**
2. **Video saniye referansı**
3. **Ne yapıyor?** (1-2 cümle)
4. **Bizdeki muadili (varsa) ve eksik kısım**
5. **RIMA'ya entegre kod planı:**
   - Hangi dosya değişecek? (yukarıdaki listeden tam path)
   - Hangi C# API kullanılacak? (örn `ReorderableList`, `EditorGUI.PrefixLabel`, `IMGUI custom drawer`)
   - Effort tahmini: S (<2sa) / M (2-6sa) / L (1+ gün)

### Bölüm 2 — Diğer tool'lardan 3-5 pattern
Tiled / LDtk / RuleTile / Godot / GameMaker / Aseprite'tan RIMA için en uygun 3-5 pattern. Aynı format.

### Bölüm 3 — Inspector mimari kararı
Şu iki yaklaşımdan hangisi RIMA için daha uygun? Gerekçeyle:
- **A) Asset-centric (mevcut):** Palette'ten asset seç → onun SO'sunu editle → "Apply to scene" butonuyla instance'a yansıt
- **B) Instance-centric (Sang Hendrix tipi):** Sahnede placed instance'lar layer manager listesi gibi inspector'da görünür → her satır kendi ayarları olan mini-row, drag reorder, live sync
- **C) Hybrid:** Üstte asset SO editor, altta "Instances of this asset in scene" listesi (count badge + jump-to-each butonu)

A vs B vs C için trade-off matrisi (3 satır × 5 sütun: ease of use / power / authoring speed / code complexity / RIMA fit). Verdict 1 cümle.

### Bölüm 4 — Top 3 acil ihtiyaç
Bizim mevcut inspector'a bakınca (yukarıdaki dosyaları aç ve oku — `Assets/Editor/RoomPainter/Inspector/`), HEMEN 3 gün içinde uygulayabileceğimiz en yüksek-değerli 3 değişiklik:
1. (priority 1) — pattern + dosya + 5-10 satır pseudocode
2. (priority 2) — aynı
3. (priority 3) — aynı

Pseudocode değil, **doğrudan C# Unity Editor IMGUI** kodu yaz. Compile-ready snippet'ler ver.

### Bölüm 5 — Verdict
Şuna net cevap: **Inspector'a daha fazla yatırım yapmalı mıyız, yoksa Phase A Day 5b (Visual Collider Authoring) ve Day 6+ (tools, save/load, polish) ile devam mı?**

Kullanıcı bağlamı: oyunun ana özelliği roguelite ARPG combat + procedural level. Room Painter sadece authoring tool. Ne kadar yatırım hak ediyor?

## Çıktı formatı
Markdown. Pratik. C# snippet'leri compile-ready. Kod block'ları için ```csharp etiketi.

Output path: `C:/tmp/inspector_ux_patterns_codex.md` (Windows /tmp)

Eğer Codex sandbox /tmp yazma erişimi yoksa, alternatif: `STAGING/inspector_ux_patterns_codex.md` (RIMA root). Output'u kaybetme.

## Effort
medium. Plan, list dosyalar, oku, sentezle, yaz. Implementation yok (Claude orchestrator implementasyon kararını sonra verecek).
