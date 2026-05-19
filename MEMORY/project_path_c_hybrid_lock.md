---
topic: project_path_c_hybrid_lock
updated: 2026-05-19
---

# Project Path C Hybrid Lock

Use when: Act 1 production pipeline, Codex image_gen floor/wall, painted base + sprite overlay, layer architecture, Path A vs C decision, Hades formula, Map Designer Brush V1 authoring.

## 2026-05-19 S95 LATER — Production Status

**5 PNG LIVE** (`STAGING/codex_floor_walls_v01/`):
- floor_A_granite_v01.png (1024x1024, 4 variant) — drift accepted, alt 2 quadrant warm brown
- floor_B_cracked_v01.png (1024x1024, 4 variant) — PASS, cyan rift cracked stone
- floor_C_dirt_v01.png (1024x1024, 4 variant) — PASS, dirt + rubble warm palette
- floor_D_ritual_v01.png (1024x1024, 4 variant) — PASS, her tile center cyan sigil (5% spawn sparse)
- walls_set_v01.png (1024x1536, 6 piece, transparent BG) — PASS

**QC Verdict:** 4/5 PASS, Floor A drift kabul edilebilir (user "tutarli gibi geldi" — re-gen iptal).

**Unity base dispatch fired (Claude Sonnet sub-agent, background):** PathC_BaseTest.unity (16x10) + 16 Tile.asset + Tilemap weighted paint (Stone 50/Cracked 25/Dirt 20/Ritual 5) + per-tile random rotate (0/90/180/270) + flip 50% + 12 wall perimeter compose + screenshot.

**Grid-akiclilik stratejisi LOCKED:** Random rotate + flip + 4 variant × 4 material weighted = repeat-stamp kırılır. Sırıtırsa fallback: pixel art chunky re-gen (4-6 pixel block detail, 16-color palette).

## 2026-05-19 S95 HARD LOCK

Path A (pure painted) test demo'su gerekirse yapilir ama PRODUCTION yolu = **Path C Hybrid**. Codex 2 verdict integrated.

## Why (Codex)

> "Test demo ile karar ver: 1 saatlik pure painted oda ile duyguyu ve readability'yi kanitla, fakat demo gecerse bile 30+ template production icin Hybrid sec. Pure painted yolu sadece hero/reference oda icin kullan; asil oyun Hybrid olursa hem guzel kalir hem roguelite olarak calisir."

Pure painted RIMA roguelite mimari ile catisir:
- Karar #149 sub-room sequence (3-5 oda) + 20-30 template ihtiyaci
- Procedural variation → painted PNG fixed
- Iteration cost yuksek (style drift)

## Layer Architecture

```
LAYER 4 (COLLISION)          → Invisible BoxCollider2D'ler (authored)
        ↑
LAYER 3 (GAMEPLAY)           → Warblade + mob + VFX + interactive (sprite)
        ↑
LAYER 2 (OBJECT OVERLAY)     → PixelLab 119 PNG (sprite)
                                 ↳ user Map Designer Brush V1 ile yerlestirir
LAYER 1 (BASE PAINT)         → Codex image_gen (painted, cohesive)
        ├ Floor: 512x512 painted chunks (16 variant)
        └ Wall: 512x512 painted pieces (6 unified piece)
```

## Tool Assignment LOCKED

| Layer | Tool | Asset Count |
|---|---|---|
| L1 Floor base | **Codex image_gen** | 16 (4 material x 4 variant) |
| L1 Wall base | **Codex image_gen** | 6 (straight, corner NE/NW, arch, rift, door) |
| L2 Objects | **PixelLab** (zaten 119 PNG) | 119 mevcut + Faz 2 gen |
| L3 Player/Mob | PixelLab cloud anchor | 4 class + 16 mob (cloud) |
| L4 Collision | Authored manual | Map Designer Brush V1 |

## Variety Strategy

**Floor (16 chunk):**
- 4 material: Stone, Cracked Stone, Dirt/Rubble, Ritual Accent
- 4 variant per material (crack density, moss spread, dust, wear)
- Tilemap weighted random: Stone 50%, Cracked 25%, Dirt 20%, Ritual 5%
- 512x512 chunk, 1 chunk = 8 unit world (PPU 64)
- 32x22 room → ~11 visible tiles, 16 unique = good variety

**Wall (6 pieces):**
- 1 straight horizontal (tileable left-right)
- 2 corner inside L (NE + NW)
- 1 arch opening
- 1 cyan rift integrated (hero accent)
- 1 door opening
- Manual placement compose perimeter

**Real-world comparison (Codex):**
- Hades = "EN YAKIN kalite referansi" — painted look + modular gameplay layers
- Dead Cells = "EN YAKIN workflow dersi" — retake-friendly pipeline
- Octopath = RIMA'ya uymaz (HD-2D, 3D background heavy)

## Dispatch Plan

5 Codex image_gen dispatch toplam (`STAGING/CODEX_DISPATCH_path_c_floor_walls_s95.md`):
1. Floor Material A — Cool Granite (2x2 grid 1024x1024)
2. Floor Material B — Cracked Stone (2x2 grid)
3. Floor Material C — Dirt + Rubble (2x2 grid)
4. Floor Material D — Ritual Accent (2x2 grid)
5. Wall Set (2x3 grid 1024x1536, 6 wall pieces)

Cost: ~$5-15 total. NEXT SESSION dispatch (user switching Claude account).

## Map Designer Workflow (user)

Brush V1 LIVE S87+ (328 EditMode PASS) + paint-brush-architecture-lock:

1. Sahne Spawn_01 acilir
2. L1 Tilemap = Codex 16 floor chunk weighted paint
3. Wall sprites perimeter manual / brush
4. **SEN brush'la objeleri yerlestirirsin** (PixelLab 119 PNG): pillar, statue, ritual, brazier, decoration, mob spawn socket
5. Save → RoomTemplateSO + EncounterTemplateSO (Karar #149)
6. Procedural sub-room runtime composer template'leri kullanir

## Risk + Mitigation (Codex)

| Risk | Mitigation |
|---|---|
| 119 PNG lost investment | Reuse: reference/overlays/collision guides/asset vocab — env hala kullanilir |
| Production scalability (30+ room) | 5-10 room archetype prompts standardize + overlay reuse |
| Gameplay mismatch (baked walls != blockers) | Invisible colliders first, debug overlay pass |
| Visual mismatch (sprite overlay looks pasted) | Shared color grade, low ambient, contact shadows, limited overlay count |
| Iteration risk (regenerating drifts style) | Keep v4 image + curated sprite sheets as locked references |

## Mevcut Envanter Salvage (119 PNG)

- **Reference/prompt vocab** → painted background gen prompt'larina dahil
- **Overlay sprites** → painted L1 ustune (pillar, statue, ritual, brazier, decoration, mob, prop)
- **Collision guides** → painted wall silhouettes icin BoxCollider2D shape reference
- 35 floor tiles (granite_base + low_topdown_v02) — Faz 2'de painted Codex floor icin reference olarak Codex prompt'a verilebilir, ya da DELETE (artik primary degil)

## Status

- Test demo Path A: opsiyonel (1 saat, feel proof)
- Production: Path C **HARD LOCK**

## Baglantilar

- `project_karar_150_fake_isometric_lock.md` — Karar #150 LIVE spec (target visual)
- `project_karar_150_act1_envanter_live.md` — 119 PNG envanter (overlay olarak kullanilir)
- `project_karar_149_subroom_encounter_lock.md` — sub-room sequence (template count)
- `feedback_basic_attack_combo_identity.md` — combat identity (gameplay layer)
- `STAGING/CODEX_DISPATCH_path_c_floor_walls_s95.md` — 5 prompt dispatch dosyasi
- `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md` — Karar #150 full spec
