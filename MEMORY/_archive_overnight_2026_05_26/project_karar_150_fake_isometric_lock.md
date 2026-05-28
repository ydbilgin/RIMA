---
name: project-karar-150-fake-isometric-lock
description: "2026-05-19 S94 LATE LIVE — Karar #150 Act-Aware Dungeon-Inside Architecture LOCK. Fake iso + 8-dir (Karar #114) + 32x22 sub-room, internal-arch primary, diamond constraint kaldırıldı. v4 PASS reference, Codex APPROVE_WITH_REVISIONS integrated."
metadata:
  node_type: memory
  type: project
  originSessionId: 9aecb83a-6b4d-4534-98af-97da4c678d26
---

# Karar #150 LIVE — Act-Aware Dungeon-Inside Architecture (S94 LATE 2026-05-19)

**Status:** CANDIDATE → **LIVE** (user lock 2026-05-19, Codex review IN FLIGHT — revisions optional)
**Concept PASS:** `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`
**Full spec:** `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md`

## Ne kararlaştırıldı

RIMA dungeon görünümü **"fake isometric + dungeon-inside"** tekniği. Hades 3D mesh REJECTED. Walls fake-iso depth, character **8-dir (Karar #114 LOCK)** — 5 sprite üret (S, SE, E, NE, N) + 3 mirror (W/SW/NW), room generator irregular layout. **Diamond constraint kaldırıldı.** Tüm Act'ler (1/2/3) aynı mimari grameri kendi tematik palet/material ile yorumlar.

## Why

- Karar #100 35° tilt + Karar #114 8-dir korumak — combat readability + movement clarity
- Stardew/Octopath/SNES RPG aesthetic — proven 16-bit RPG tekniği
- v4 image_gen evidence: 32×22 + internal-architecture-primary + irregular layout = "dungeon İÇİNDE" hissi (user's requested feel)
- v3 (18×12 + diamond) "arena yukarıdan izlemek" hissi → REJECTED
- 3 Act'lik scale için sustainable production roadmap (110 asset per Act × 3 = ~330 + buffer)

## How to apply

### Core visual rules (all acts)

| Kural | Spec |
|---|---|
| Map silhouette | Irregular ~32×22 sub-room default (Karar #149 16×10 → 32×22 revize) |
| Perimeter walls | Minimal / off-screen; frame %75 internal architecture |
| Internal walls | Min 2 free-standing pillar + 1 archway connector + 1 collapsed stub |
| Wall depth | Fake iso (top cap + front face + base shadow). Flat YASAK |
| Floor angle | 35° tilt + 0° Y rotation (Karar #100 LOCK) |
| Character | 8-dir (Karar #114 LOCK 2026-05-13): 5 sprite üret (S, SE, E, NE, N) + 3 mirror (W←E, SW←SE, NW←NE) Unity SpriteRenderer.flipX |
| Door connection | Sub-room transition mirror archway (Karar #149 fade-to-black) |

### L3 wall class set (per Act)

5 sınıf + arch + pillar + collapsed_stub = **8 class × 3 variant = 24 wall sprites** per Act.

### Per-Act material adaptation

| Act | Theme | Dominant | Accent (Karar #98) | L3 variant |
|---|---|---|---|---|
| Act 1 Shattered Keep | Fragmented ancient order | `#3A3D42` granite | `#00FFCC` cyan | Granite + vine creep |
| Act 2 Bleeding Wastes | Living corrupted wound | `#3A2840` bog | `#C8502A` rust ember | Bone-wrapped granite |
| Act 3 Core Approach | Transcendental cosmic | `#0A0810` void | `#FFD700` gold | Void-stone + gold sigil |

### Karar #149 sub-room door-through

- Combat/Elite node = 1 EncounterTemplateSO × 3-5 sub-room
- Sub-room transition: archway exit (sub-room N) → fade-to-black → archway entry (sub-room N+1, MIRROR placement)
- Visual consistency: same Act material + palette
- Logical connection: optional debris trail L5 breadcrumb sub-room N → N+1
- 5 sub-room slot types: Entry chamber / Pillar arena / Collapse corridor / Ritual hall / Crypt cell

### Asset count

- Per Act: 16 L1 + 16 L2 + 24 L3 + 24 L4 + 18 L5 + 12 L6 = ~110
- 3 Act × 110 = 330 + 25% regen buffer = ~445 effective
- Current budget: 3500/5000 PixelLab → comfortable for all 3 Acts

### Cross-Act reuse

| Asset | Universal? |
|---|---|
| Small stones (4), VFX atomic, generic floor decals | ✅ |
| L1/L2 floor materials | ❌ Per-Act |
| L3 wall classes | ❌ Per-Act (palette + overlay swap evaluation pending Codex) |
| L4 patches | ❌ Per-Act |
| L5 scatter | Mixed (generic universal, Act-themed per-Act) |
| L6 hero accent | ❌ Per-Act |

**HARD RULE:** Yeni asset gen ÖNCE check `_Universal` veya başka Act'te benzer var mı.

## Concept iteration history

| Version | Style | Status |
|---|---|---|
| v1 | 50-60° isometric | REJECTED — 8-dir char + camera angle uyumsuz |
| v2 | 35° + 0° Y rotation, flat walls | REJECTED — wall depth yok |
| v3 | 35° + fake iso + DIAMOND | REJECTED — arena-feel, küçük (18×12) |
| **v4** | 35° + fake iso + IRREGULAR + 32×22 + internal-arch primary | **PASS — LOCKED** |

## Conflict checks (all preserved)

- Karar #98 cyan rift: per-Act color variant (cyan/rust/gold)
- Karar #100 35° + Karar #114 8-dir: PRESERVED
- Karar #143 6-layer: PRESERVED (sadece L3 wall class değişti)
- Karar #147 Multi-Layer Painter: PRESERVED
- Karar #148 Branch D+E: PRESERVED
- Karar #149 sub-room sequence: **UPDATED — default 16×10 → 32×22**

## Codex review pending revisions

Dispatch: `STAGING/CODEX_TASK_karar_150_review.md` (background, profile rotation).

Review boyutları:
- Architecture feasibility (RoomTemplateSO 32×22 + 8 wall class schema)
- Asset economics (330+ gen budget feasibility)
- Sub-room connection (archway mirror rule implementation)
- Layout grammar (5 sub-room slot types data model)
- Roadmap × 3 Act sustainability

Verdict + revisions apply → Faz 1' dispatch trigger.

## NLM staleness

NLM hâlâ Karar #149 öncesi "1 oda = 1 arena wave-based" canonical'ı söylüyor. Karar #149 + Karar #150 NLM'e sync edilmeli (`/nlm-sync` dispatch).

## Trigger

- v4 PASS evidence ✅
- User lock ✅ (2026-05-19 S94 LATE)
- Codex review → revisions optional integration
- Faz 1' dispatch hazır (Antigravity Opus 4.6 veya Sonnet UnityMCP) → Act 1 isometric wall regen

## Related

- [[project-roadmap-dungeon-buildup-lock]] — 6-faz piece-by-piece discipline
- [[project-karar-149-subroom-encounter-lock]] — sub-room sequence integration
- [[project-asset-pack-organization-lock]] — folder hierarchy
- [[project-alabaster-dawn-pipeline-lock]] — Karar #143 6-layer
- [[project-canonical-character-roster-v2]] — char 8-dir constraint (Karar #114)
- [[feedback-8dir-mirror-production-strategy]] — 5 produce + 3 mirror pipeline
- [[feedback-user-cannot-draw-full-autonomy-required]] — production discipline
