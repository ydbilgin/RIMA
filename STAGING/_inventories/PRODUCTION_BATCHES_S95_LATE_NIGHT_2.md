# PixelLab Production Batches — S95 LATE NIGHT 2 (Onay Bekliyor)

> User direktifi: "Düz zemin + seamless duvar yapısı. Önceki duvarlar gibi doğru yönde üret. 128px için n=4 batch. Description'ı ortak tutup item'ları güzelce anlat. agent_help kullanarak ilerle. Eksikleri ve batch'leri listele, onay verirsem üreteceksin."

## Eksikler — Mevcut KEEP_FIT sonrası boşluklar

| # | Eksik | Sebep | Etki |
|---|---|---|---|
| 1 | **Flat iso granite tile** (relief-detay-free) | Mevcut 3 tile yüzeyinde 4 mini taş blok relief var → Y axis seam visible | Düz zemin görünmesi imkânsız |
| 2 | **Wang Wall Pieces** (face_NS + corner_inner + T_junction + end_cap) | Pilot A 4 piece batch'inde face_NS drift, diğer 3 wang gap'i hiç gen edilmedi | Tam birleşen perimeter duvar mümkün değil, batı duvarı 0 piece |
| 3 | **True floor patch overlay** (3 type) | dust_drift tek 2D alpha overlay; cave_moss + cracked_rubble 3D oyuk drift olduğu için archive | Floor variation katmanı düşük |
| 4 | **iso mechanic obje** (pressure_plate, iron_grate_floor) | Mevcut sadece spike_trap_dormant; pressure_plate + iron_grate top-down drift archive | Gameplay floor mechanic eksik |

---

## Batch Plan (onay bekliyor)

### MCP Recipe Standardı (her batch'te uygulanır)

| Parametre | Değer | Sebep |
|---|---|---|
| Tool | `mcp__pixellab__create_object` | Pilot A workflow ile aynı (CURRENT_STATUS S95 doğrulu) |
| `directions` | 1 | Tek yön, billboard sprite |
| `n_frames` | **4** (128px) veya **16** (64px) | User direktifi: 128px max 4 batch; agent_help review pipeline confirme |
| `size` | 128 (wall + obje) veya 64 (patch + decal) | Sprite type'a göre |
| `view` | `side` (wall) veya `high top-down` (floor/patch/obje) | Pilot A pattern |
| Workflow | `create_object` → `get_object` 16 review candidate → `select_object_frames(indices=[..])` | Review pipeline |
| Style anchor | Pilot A face_EW (mevcut) — color/material/lighting ortak | Seamless edge alignment |

### Saçmalık önleme (her prompt'a)
- "isometric 35°, side view, dark granite stone, cyan crack accent, baked shadow base, transparent background, no ground rubble overflow, edge-aligned tileable"
- "no top-down projection, no 3D bowl effect, no extra props, single piece silhouette"

---

## BATCH A — Wang Core Walls (Pilot A 1.1b)

**Hedef:** 4 wang piece — birbiriyle ve mevcut Pilot A 3 piece ile seamless edge connect.

| # | Piece | Description prompt | Notlar |
|---|---|---|---|
| A1 | **face_NS** (kuzey-güney duvar) | `isometric dungeon stone wall NORTH-SOUTH face, vertical billboard, granite blocks, cyan crack edge highlight, 35° iso side view, baked left shadow, transparent background, no floor rubble, tileable edge` | drift olan frame_0 yerine |
| A2 | **corner_inner** (iç köşe) | `isometric dungeon stone wall INNER 90° corner, two faces meeting concave, granite blocks, cyan crack accent, 35° iso side view, baked shadow, transparent, tileable both edges` | Pilot A'da corner_outer var, inner gap |
| A3 | **T_junction** (T birleşim) | `isometric dungeon stone wall T-junction, three faces meeting, granite blocks, cyan crack accent, 35° iso side view, baked shadow, transparent, tileable three edges` | Branching wall için |
| A4 | **end_cap** (duvar sonu) | `isometric dungeon stone wall END CAP, single piece terminating cleanly, granite blocks, cyan crack highlight, 35° iso side view, baked shadow, transparent, tileable one edge` | Wall end pieces için |

**Tek MCP call başına 4 variant gen** (n_frames=4) → review pick best 1.
**4 piece × 5 gen review pipeline = ~20 gen toplam**

**Onay sonrası dispatch:**
```python
for piece in [A1, A2, A3, A4]:
    result = create_object(
        description=piece.description,
        directions=1,
        n_frames=4,
        size=128,
        view="side",
        object_view="top-down",
        tags=["act1_wall_wang_core_s95", piece.id]
    )
    # → review status, kullanıcı get_object ile preview, select_object_frames ile pick
```

---

## BATCH B — Flat Iso Granite Floor Tile

**Hedef:** Düz yüzey base tile (relief detay YOK). Mevcut 3 tile decoration tile olarak korunur, base flat eklenir.

| # | Variant | Description prompt |
|---|---|---|
| B1 | **flat_clean** | `isometric flat floor tile, smooth dark granite surface, uniform plane, 35° iso diamond, no relief no chiseled detail no stone blocks, transparent edges, seamless tileable, dim base lighting` |
| B2 | **flat_worn** | `isometric flat floor tile, weathered granite surface, smooth plane, light wear stains, 35° iso diamond, NO 3D blocks NO relief, transparent edges, seamless tileable` |
| B3 | **flat_polished** | `isometric flat floor tile, polished granite surface, smooth reflective sheen, 35° iso diamond, NO relief NO bumps, transparent edges, seamless tileable, dim sheen highlight` |

**3 piece × n_frames=4 × 5 gen avg = ~15 gen toplam**

Critical: prompt'ta NO RELIEF + SEAMLESS TILEABLE + UNIFORM PLANE keyword'leri zorunlu — relief'siz çıkması için.

---

## BATCH C — True Floor Patch Overlay (Pure 2D Alpha)

**Hedef:** 3 farklı patch tip, pure 2D alpha blob (dust_drift gibi), NO 3D oyuk, NO 3D pile.

| # | Variant | Description prompt |
|---|---|---|
| C1 | **moss_stain** | `pure 2D alpha overlay, organic moss stain, deep green discoloration spreading on ground, flat soft-edge blob, NO 3D depth NO sunken bowl NO raised pile, transparent edges, painted alpha texture for floor overlay` |
| C2 | **blood_stain** | `pure 2D alpha overlay, dark dried blood stain on ground, irregular crimson splatter blob, flat soft-edge, NO 3D depth NO bumps, transparent edges, painted alpha for floor overlay` |
| C3 | **rift_seep** | `pure 2D alpha overlay, cyan rift energy seeping into ground, faint glowing patch, flat ethereal blob, NO 3D effect NO crystals NO objects, transparent edges, painted alpha for floor overlay` |

**3 type × n_frames=16 × 64px × 1 review batch each = ~15 gen toplam**

n_frames=16 @ 64px doğru per agent_help (smaller size → more frames OK). 16 candidate'tan en iyi 4 select.

---

## BATCH D — iso Mechanic Obje

**Hedef:** Pressure plate + iron grate floor — iso projection, top-down değil.

| # | Variant | Description prompt |
|---|---|---|
| D1 | **iso_pressure_plate** | `isometric dungeon floor pressure plate, raised stone square slightly inset into floor, 35° iso diamond projection, dark granite material with metal trim edge, baked subtle shadow, transparent base, gameplay mechanic obje silhouette` |
| D2 | **iso_iron_grate** | `isometric dungeon floor iron grate, dark iron grille on rectangular floor inset, 35° iso diamond projection, deep shadow void below grate, granite border frame, transparent background, gameplay mechanic obje silhouette` |

**2 piece × n_frames=4 × 5 gen avg = ~10 gen toplam**

---

## Toplam Tahmin

| Batch | Açıklama | Gen |
|---|---|---|
| A | Wang Core Walls (face_NS + corner_inner + T + end_cap) | ~20 |
| B | Flat granite floor tile (clean + worn + polished) | ~15 |
| C | True floor patch overlay (moss + blood + rift_seep) | ~15 |
| D | iso mechanic obje (pressure_plate + iron_grate) | ~10 |
| **Toplam** | | **~60 gen** |

**Mevcut bütçe:** 2,413 / 5,000 → 2,587 reserve. **60 gen rahatlıkla sığar (kalır ~2,527).**

---

## Sıralı Dispatch Önerisi

1. **Batch A önce** — Wang Core wall'lar — sahnenin batı duvarını + birleşen perimeter olanağı sağlar. **Kritik.**
2. **Batch B paralel veya sıra** — Düz zemin → Y axis seam fix. **Kritik.**
3. **Batch C** — Patch overlay variety → floor variation katmanı.
4. **Batch D** — Mechanic obje — gameplay layer.

Tek MCP call'larla 4 batch ayrı ayrı dispatch edilir (paralel veya sıralı, tercih sizin).

---

## Workflow (her batch için)

1. `create_object(description=..., directions=1, n_frames=N, size=S, view=V)` → review_status object_id
2. `get_object(object_id)` → 4 (veya 16) review candidate inline image
3. **Orchestrator (Claude) candidate görsel inceler** → en iyi 1-4 frame index belirler
4. `select_object_frames(object_id, indices=[i,j,k,l])` → her seçim ayrı 1-dir object olur
5. PNG'ler download edilir → `Assets/Art/AssetPacks/Act1_ShatteredKeep/...` doğru klasöre
6. Pivot + import settings (PPU 64, Point, custom pivot) Codex tarafından
7. Prefab oluşturma (wall pieces için) — Pilot A pattern korunur

---

## ONAY GEREKEN

Bu plan onaylanırsa:
1. Batch A dispatch → review → select → import → prefab (~30 dk)
2. Batch B dispatch → review → select → import (~20 dk)
3. Batch C dispatch → review → select → import (~20 dk)
4. Batch D dispatch → review → select → import (~15 dk)
5. Sahneye yeni piece'leri yerleştir → screenshot

**Toplam tahmini süre:** ~1.5 saat (batch'ler arasında orchestrator review için bekleme dahil).

User'a sorular:
- [ ] Bu 4 batch onay mı? Veya değişiklik var mı?
- [ ] Sıralı mı, paralel mi dispatch?
- [ ] Wang Core'da face_NS + corner_inner + T + end_cap yeterli mi, ek piece (örn. cross_intersection, wall_low_partition) lazım mı?
- [ ] Flat tile + decoration tile'ları ikisi de KEEP mi, yoksa flat tile geldiğinde mevcut 3 tile archive mı?
- [ ] Patch overlay 3 tip yeterli mi, blood_stain Act1 için uygun mu, alternatif tema?
