---
name: phase-1-mob-sprites
description: "S86 Update - 12 mobs on disk, 4 pending, Karar"
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

> **Karar #8:** 16 benzersiz mob total target.
> **Disk count (2026-05-16):** 12 produced, 4 not yet produced.
> **Style drift score:** 5/10 (rima-qc 2026-05-16).

# TECHNICAL SPEC (S86)
* Canvas: 64x64px native (Karar #74 LOCK -- 128px REVOKED)
* PPU: 64
* Tool: PixelLab Create Image Pro / create_tiles_pro batch
* In-game Scale: 0.5x - 1.0x (handled via Unity sprite scale, Karar #74)
* Style Ref: OPTIONAL (Mob identity distinct from players)

# MOB ROSTER (16 target — 12 on disk, 4 pending)

| # | Mob | Disk | QC Status | Notes |
|---|-----|------|-----------|-------|
| 1 | fracture_imp | YES | KEEP | |
| 2 | relic_caster | YES | REVISE | Re-prompt Low Top-Down |
| 3 | seam_crawler | YES | KEEP | |
| 4 | plate_widow | YES | REVISE | Re-prompt Low Top-Down |
| 5 | hollow_arbitter | YES | REVISE | Re-prompt Low Top-Down |
| 6 | rift_gound | YES | REGEN | Full rebuild |
| 7 | 11_spire_choirling | YES | REGEN | Full rebuild |
| 8 | 12_shard_walker | YES | KEEP | |
| 9 | 13_penitent_bruiser | YES | KEEP | |
| 10 | 14_riftbound_augur | YES | REVISE | Re-prompt Low Top-Down |
| 11 | 15_hollow_hulk | YES | KEEP | |
| 12 | 16_rift_acolyte | YES | REVISE | Re-prompt Low Top-Down |
| 13 | (pending mob 13) | NO | NOT PRODUCED | |
| 14 | (pending mob 14) | NO | NOT PRODUCED | |
| 15 | (pending mob 15) | NO | NOT PRODUCED | |
| 16 | (pending mob 16) | NO | NOT PRODUCED | |

QC Legend: KEEP = production-ready. REVISE = re-prompt Low Top-Down (correct angle/style). REGEN = full rebuild required.

# PRODUCTION SETTINGS
* Mode: Low Top-Down ~35 deg (Karar #40/#45 -- NOT 75-80 deg bird-eye)
* Detail: Low
* Outline: Single color
* Background: Transparent
* Palette: Cyan + violet for rift-type mobs (Karar #98)

# VARIANT MATRIX — Karar #145 Use Case #2 (S86 LIVE 2026-05-16)

PixelLab Character States enables **1 base mob × N outfit variants** without re-generating identity. This expands the 16-target roster ekonomik şekilde.

**Variant matrix recipe:**
1. Generate base mob via Create Image Pro (existing flow, no change)
2. **Create State × N** with outfit-variant prompts that preserve identity:
   - "same mob, **armored variant** with heavy plating and helmet"
   - "same mob, **elite variant** with gold trim and captain insignia"
   - "same mob, **boss variant** with large pauldron and red glowing eyes"
   - "same mob, **damaged variant** showing visible armor cracks and wounds"
3. Each variant inherits identity, only outfit/equipment/wear-state changes
4. Per variant → animation production using state-first workflow

**Output multiplier:** 16 base mob × 3-4 variant each = **48-64 visually distinct enemies** from 16 base sprite budgets.

**Why this matters:** Encounter design depth tradeoff; mob roster expansion was a Phase 2-3 backlog item, now viable in Phase 1 via state augmentation.

**Cross-link:** [[pixellab-character-states-workflow]] (RIMA local) + [[pixellab-character-states-animation-workflow]] (Lauret Studio global canonical). State-first first-frame retention also resolves the older mob style-drift complaint (5/10 score) — anim frames anchored to a known good state instead of inventing from neutral idle.
