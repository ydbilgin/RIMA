# S110 — Parallax Yol B (ChatGPT ambitious 10-layer Room Painter) Phase A Plan

**Agent:** general-purpose Sonnet sub-agent (design + planning, no code)
**Effort:** ~45 dakika (spec sentezi + Phase A scope dosyası)

---

## Active rules

1. Think before coding. Varsayım listele.
2. Min code, no speculation — DESIGN ÖZET, kod yazma.
3. Surgical — bu task SADECE plan dosyası üretir.
4. BLOCKED if unclear → orchestrator'a sor.

---

## Bağlam

User Yol B'yi seçti: ChatGPT ambitious **10-layer Realtime Room Painter** (2-3 hafta toplam, PainterSuite v1.1 ana feature).

Source spec: `STAGING/s109_chatgpt_room_painter_spec.md` (full ChatGPT verbatim, 78+ satır)
Önceki ParallaxLayer.cs (LIVE): `Packages/com.laureth.painter-suite/Runtime/ParallaxLayer.cs`
ParallaxLayer factor: L0=0.03, L1=0.05, L2=0.08, L3=0.14, L4=0.10

## Görev

ChatGPT spec'i tam oku. Phase A scope çıkar: **5-9 günde ship edilebilir MVP**, 10 layer'ın hepsi değil, **ilk 3-4 layer + EditorWindow + scene placement + save**. Sonraki phase'lere parça parça koy.

### Çıktı dosyası

`STAGING/PARALLAX_PAINTER_PHASE_A_PLAN.md` yaz. İçerik:

#### Bölüm 1: Phase A scope (MVP, 5-9 gün)
- Hangi 3-4 layer? (Floor + Cliff + Props + Parallax önerim — hep-kullanılan + parallax core)
- EditorWindow temel: asset palette + scene click place + layer toggle + save prefab
- BIRAKILAN: collision painting, occlusion zones, theme presets (Phase B/C)

#### Bölüm 2: 3 SO hierarchy
- **RoomPainterAsset** (single asset definition — sprite/prefab + defaults). Field listesi tam.
- **RoomLayerData** (per-room layer config — name, depth, sort layer, static/locked/relative). Field listesi tam.
- **RoomData** (top-level room asset — list of placements + layers + parallax profile). Field listesi tam.
- Cross-reference diyagram (ASCII art OK).

#### Bölüm 3: 10 layer enum + Phase ramp
- Tam 10 layer (ChatGPT'den) listele: Floor / Edge / Cliff / Wall / Props / Decals / Lighting / Collision / Occlusion / Parallax
- Her layer için:
  - Phase atama (A/B/C)
  - Default sort layer adı
  - Default order range
  - Y-sort gerek mi?

#### Bölüm 4: EditorWindow architecture
- File listesi (yeni asmdef + ana window + helper service'ler)
- ÇİZGİ (ASCII layout) — window'un nasıl görüneceği (sol palette, sağ inspector, üst toolbar)
- SceneView overlay sorumluluğu (placement gizmo, ghost preview)
- Mevcut PainterSuite altyapısından NE kullanılır (BrushExecutorRouter? AutoLayeringService? VisualEditorScenePainter pattern reuse?)

#### Bölüm 5: Phase A günlük breakdown
- Gün 1: 3 SO + asmdef + window stub
- Gün 2: Asset palette panel (folder scan, thumbnail)
- Gün 3: SceneView click placement + ghost preview
- Gün 4: Layer toggle + sorting integration
- Gün 5: Save/Load RoomData (prefab + SO)
- Gün 6-7: Polish + bug fix + docs
- Gün 8-9: Buffer / Phase B'ye eğer hızlı bitti geçiş

#### Bölüm 6: Risk register
- Mevcut RIMA Cliff system ile çakışma riski
- ParallaxLayer.cs (UPM package) ile entegrasyon riski
- PainterSuite ColliderPainter ile namespace çakışma riski
- 3 risk × mitigation strateji

#### Bölüm 7: Phase B/C teaser
- Phase B (Hafta 2): Collision painting, occlusion zones, parallax depth controls
- Phase C (Hafta 3): RIMA-specific brushes (Cliff Edge / Wall Line / Prop Scatter / Rift Decal) + 5 theme preset

## Önemli

- **DESIGN ÖZET**, kod yazma. Sadece spec'i yapısallaştır + Phase A'ya odakla.
- **5-9 gün scope çok katı tut** — daha fazla feature istersen Phase B'ye it.
- **Mevcut PainterSuite v0.4.0 LIVE state'i** referans al — `Packages/com.laureth.painter-suite/Editor/Core/PainterSuiteWindow.cs` pattern reuse şansını araştır.

Inline raporlama (Agent yanıtı) — dosyayı yazdıktan sonra ana noktaları 200 kelime özetle.
