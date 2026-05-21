# Codex Task — UIUX Bug Fix + User-Friendly Redesign (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_uiux_bug_fix_plus_user_friendly_s95.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "unified painter'ı kullanıcı dostu yap basit şekilde ayarlayabileyim teknik değil kullanıcı arayüzlü olacak arka planda bağlayacaksın"

User kararları:
- **Naming:** Painter generic "wall" içeren naming destekle (pilot_a_wall_*, act1_wall_*, wall_face_*)
- **IsoSorter:** Static walls için DISABLE, karakter+mob için aktif kalır

## 4 Bug Fix + User-Friendly Revize

Tek atomik edit `Assets/Editor/RimaUnifiedPainterWindow.cs` + (gerekirse) yeni helper file.

### Bug 1 — Painter Palette Pilot A'yı Görmüyor

**Root cause:** `ScanPrefabsInFolder` line 247-251 sadece `Assets/Prefabs/Props/ShatteredKeep_PixelLab/` + `wall_` prefix.

**Fix:**
- `ScanAllAssets` line 176'da wall scanning'i çoklu path/pattern desteğine genişlet
- Yeni helper: `ScanWallPrefabsMultiFolder(List<string> folders, List<string> namePatterns)`:
  - Folders: `Assets/Prefabs/Props/ShatteredKeep_PixelLab/`, `Assets/Prefabs/Walls/pilot_a/`, `Assets/Prefabs/Walls/` (root)
  - Name patterns: filename contains "wall" (case-insensitive)
  - Class field `wallScanFolders` ve `wallScanNamePatterns` SerializedField (default sabit, future configurable)

**Test:** Painter aç → Duvar kategori → palette'te `pilot_a_wall_face_EW`, `pilot_a_wall_corner_outer`, `pilot_a_wall_arch_opening` görünmeli + mevcut legacy `wall_*` da çalışsın.

### Bug 2 — Auto-Connect Pilot A Algılamıyor

**Root cause:** `FindWallAtCell` line ~2698, `UpdateWallConnectionsAt` line 2730 sadece `wall_` prefix kontrol.

**Fix:**
- `IsWallObject(GameObject go)` helper: `name.ToLowerInvariant().Contains("wall")` veya sprite name contains "wall"
- `FindWallAtCell` ve `UpdateWallConnectionsAt` bu helper'a delege
- Auto-connect mantığı naming bağımsız çalışsın (pilot_a_wall_face_EW, act1_wall_*, wall_*, my_custom_wall vb.)

### Bug 3 — Selected Instance Scale Compensated Scale Bozar

**Root cause:** Panel 5 (Selected Instance Editor) `localScale.x` yazarken uniform (x=y=z) write. Grid parent altı (scale 1, 0.5, 1) walls için y compensation bozulur — wall "büyük üste geliyor".

**Fix:**
- Panel 5'te scale field "**Boyut**" (Türkçe) olarak görünür — TEK FLOAT (uniform world scale)
- Set sırasında: `target.localScale = Vector3.one * worldScale / target.parent.lossyScale.y` (parent compensation)
- Veya daha basit: `target.transform.SetScale(worldScale)` ile world-space scale yaz (Unity API yoksa manuel parent lossyScale division)
- Edge case: parent identity (Props_Root) → division by 1, fark yok

### Bug 4 — IsoSorter Static Walls'u Override Ediyor

**Root cause:** `ApplySorting` (line ?) IsoSorter component her wall'a ekliyor, LateUpdate her frame `sortingOrder = round(-y*100)` override ediyor. CollisionResolver'ın verdiği order=20 görünmez.

**Fix:**
- `ApplySorting` içinde category check: `if (category == PaletteCategory.Wall) return` (IsoSorter eklemeyin)
- Wall sortingOrder = CollisionResolver.Resolve.sortingOrder (sabit 20)
- Karakter, mob, prop (Wall hariç) için IsoSorter aktif kalır
- Mevcut sahnedeki wall'lardan IsoSorter component'ini temizle (one-time migration helper veya bilgilendirici hint)

### User-Friendly Label Revize (Türkçe + Sade)

`RimaUnifiedPainterWindow.cs` UI label'larını Türkçeleştir + sadeleştir.

**Mevcut (teknik) → Yeni (sade Türkçe):**

| Mevcut | Yeni |
|---|---|
| Category: Floor/Wall/Prop/Mob | Tür: Zemin / Duvar / Obje / Canavar (zaten Türkçe, kontrol et) |
| `CollisionMode: Auto/Passable/SmallFootprint/FullFootprint/WallBlock/Custom` | `Çarpışma: Otomatik / Geçilebilir / Küçük Engel / Büyük Engel / Duvar / Özel` |
| `useRandomVariants` | `Çeşitlilik` (varyantları rastgele seç) |
| `autoConnectWalls` | `Duvarları Otomatik Bağla` |
| `randomizeWallCracks` | `Duvar Bozulması Rastgele` |
| `prefabScaleMultiplier` | `Boyut Çarpanı` |
| `snapToGrid` | `Hücreye Hizala` |
| `targetTilemap` | `Hedef Zemin` |
| `targetParent` | `Hedef Klasör` |
| `activeBiome` | `Aktif Biyom` |
| Panel headers (Foldout) | Türkçe + emoji opsiyonel (📦 Hedef, 🎨 Fırça, vb. opsiyonel) |
| `sortingLayer` / `sortingOrder` field | **GİZLE** (arka planda, kullanıcı görmez) |
| `BoxCollider2D.size / offset` Vector2Field | "Sürükle handle Scene'de" mesajı (Edit Collider butonu yeterli) |

**Hide technical fields:** sortingLayer/sortingOrder/spritePivot Panel 2'de gösterilmez. CollisionResolver arka planda halleder. User sadece "Çarpışma" türünü seçer.

**Panel 5 sade:**
- "**Seçili Obje**" header
- "Tür: Duvar / Geçilebilir / Engel" dropdown (CollisionMode mapping)
- "**Boyut**: [1.0]" tek float (Bug 3 fix uniform world scale)
- "**Dönme**: [0/90/180/270]"
- "**Scene'de Düzenle**" button (Edit Collider entry)
- "**Sil**" button (Delete Instance)
- Move to group: "**Klasör değiştir ▾**" dropdown

**Tooltip ekle her field için** — Türkçe açıklama (max 1 satır).

## Wall Randomize Variants Hazırlık (Trigger Gate)

Memory `project_wall_randomize_variants_when_states_live`:
- Şu an wall variant pool YOK (sadece 3 intact wall var)
- Yapılacak: `WallVariantGroup` data structure ekle (TerrainGroup paralel, line 74-80 örnek)
- `useRandomVariants` aktifse wall paint sırasında variant pool'dan random pick
- ŞU AN POOL BOŞ — function-ready, asset trigger gate (state'ler üretilince doğrudan çalışır)

**Implementation:**
- `wallVariantGroups: List<WallVariantGroup>` field
- Boş ise paint normal (single variant)
- Dolu ise `useRandomVariants` toggle aktifse random pick
- Future state üretim sonrası bu groups otomatik build edilir (asset scan)

## Verify

1. `dotnet build` 0 error
2. Painter window manuel smoke (Codex MCP üzerinden):
   - Duvar kategori → palette pilot_a + legacy wall görünür
   - Pilot A wall paint test → auto-connect çalışıyor (köşelerde uygun piece)
   - Wall seçince Panel 5 doldurulur, Boyut field uniform scale uyumlu
   - IsoSorter wall'larda YOK (component check)
   - sortingLayer/Order arka planda, kullanıcı görmüyor
3. Console clean (paint + select + edit collider 0 error)
4. Türkçe label'lar uygulandı

## Output Format

```markdown
# UIUX Bug Fix + User-Friendly — Codex Report

## Bug 1 Fix: Multi-Folder Wall Scanner
- ScanWallPrefabsMultiFolder helper added
- Folders: [list]
- Painter palette pilot_a_wall_* + legacy wall_*: PASS
- Files touched: RimaUnifiedPainterWindow.cs (X lines)

## Bug 2 Fix: Generic Wall Naming Detection
- IsWallObject helper: case-insensitive contains "wall"
- FindWallAtCell + UpdateWallConnectionsAt refactored
- Auto-connect pilot_a_wall_*: PASS

## Bug 3 Fix: Parent-Compensated Scale
- Panel 5 Boyut field uniform world scale + parent.lossyScale division
- Grid parent test: scale preserved YES/NO
- Props_Root parent test: identity preserved YES

## Bug 4 Fix: IsoSorter Wall Skip
- ApplySorting: PaletteCategory.Wall → skip IsoSorter
- Mevcut sahne wall IsoSorter cleanup: N component removed
- CollisionResolver sortingOrder=20 effective

## User-Friendly Label Revize
- 15+ field label Türkçeleştirildi (list)
- 3 technical field hidden (sortingLayer/sortingOrder/spritePivot)
- Panel 5 sadeleştirildi: 5 row (Tür, Boyut, Dönme, Düzenle, Sil)
- Tooltip her field için

## Wall Randomize Variants Hazırlık
- WallVariantGroup data structure added
- Empty pool fallback: single variant paint
- Trigger gate: state üretildiğinde otomatik aktive

## Verify
- dotnet build: 0 error
- Painter smoke: PASS / FAIL (detail)
- Console: clean
- Türkçe labels applied: YES

## Git Diff Summary
- RimaUnifiedPainterWindow.cs: +N -M lines

## Açık Sorular
- ...
```

## Hard Constraints

- Sadece RimaUnifiedPainterWindow.cs (+ yeni helper file gerekirse). Başka script/scene/prefab YASAK.
- Auto-commit YOK.
- Karpathy #3 cerrahi — refactor sınırı 4 bug + label revize + variant prep.
- Spec'e (UIUX DRAFT v3.1) sapma yapma — bu implementation o spec'in revision/extension'ı.
- BLOCKED if unclear: Unity API uyumsuzluk (lossyScale, EditMode enum), helper file gereklilik vb. STOP.
