# TASK: $imagegen BATCH-2 — Angled portal variants + boss/telegraph set (one-by-one, QC'd)

ACTIVE RULES: (1) think before generating (2) one asset at a time, inspect each (3) surgical — only write under STAGING/imagegen/ (4) BLOCKED if unclear.

## Amaç
Council R4 decision (Opus advisor, Option 2): exit portals need ANGLED variants for the diamond room's diagonal back edges — N slot stays frontal (batch-1 arches), NW slot gets an ANGLED-LEFT variant, NE = runtime flipX (do NOT generate NE). Plus boss/telegraph decal set (shared with mob telegraphs). Output: `STAGING/imagegen/assets/portal_pack_batch2_2026-06-07/` + manifest.md. NO Unity import, NO commit.

## Style continuity
MATCH batch-1 family: `STAGING/imagegen/assets/portal_pack_2026-06-07/portal_arch_combat.png` (+elite/reward/boss) — use them as image references where supported. Same slate stone + cyan rift palette. Pipeline: generate → magenta chroma-key cleanup → nearest-neighbor downscale → verify silhouette at target size → save + manifest entry (prompt, size, retries). 1 retry max per asset, then SKIP+log.

## ⚠️ Chroma rule (repeat)
Background SOLID MAGENTA #FF00FF. NEVER pure magenta in art. Elite accents = deep crimson/dark violet-red (match batch-1 elite). Telegraph red-magenta = use #D8364C crimson-leaning, NOT #FF00FF family.

## Angled perspective spec (for assets 1-4 — this wording matters)
"Viewed from a HIGH 3/4 DIMETRIC ANGLE (2:1 isometric), the arch is rotated ~30 degrees so its LEFT-SIDE FACE is clearly visible — the stone thickness/depth of the left pillar reads, right pillar partially occluded by the arch body. The arch sits on a small dimetric stone base plate whose back-left edge is the longest visible edge, as if planted on the NORTH-WEST diagonal edge of an isometric diamond room. The rift core inside the archway is a slanted oval matching the iso plane (NOT a flat frontal ellipse). Keystone rune slightly foreshortened to the left."

## Asset list (IN ORDER)
1. `portal_arch_combat_angled.png` — TARGET 96x128. Angled-left version of batch-1 combat arch (same stone family, cyan core, crossed-blades rune).
2. `portal_arch_elite_angled.png` — TARGET 96x128. Angled-left elite: cracked frame, deep crimson accent CRACKS (subtle — not painted-red columns), skull-crown rune.
3. `portal_arch_reward_angled.png` — TARGET 96x128. Angled-left reward: calm dim cyan core, soft gold trim, chest/star rune.
4. `portal_arch_boss_angled.png` — TARGET 128x176. Angled-left boss: heavy oversized arch, fractured great-seal crest, dark red rune, dim slow core.
5. `portal_arch_elite_v2.png` — TARGET 96x128, FRONTAL. Re-gen of batch-1 elite with SUBTLER red: slate stone dominant, crimson appears only as thin glowing CRACKS in the stone + the skull rune; no painted red trim/columns.
6. `boss_seal_fragment_set.png` — TARGET 128x32 strip (4 shards ~24px each). Floating rune shards, slate stone with cyan rift cracks, glowing edges; will orbit boss head at runtime; no shadow.
7. `telegraph_line_beam.png` — TARGET 256x32, horizontally tileable. Crimson (#D8364C) energy beam ground decal, inner white hot-line, fades at ends, top-down projection style.
8. `telegraph_circle_ring.png` — TARGET 256x256. Crimson ring telegraph decal, thin outer line + faint inner glow, PURE top-down perfect circle (scaled at runtime), runic notch marks at N/E/S/W.
9. `telegraph_cone_fan.png` — TARGET 256x256. Crimson 60-degree cone telegraph decal, apex bottom-center fanning to top, soft inner gradient, sharp outer arc, top-down.
10. `boss_intro_seal_ring.png` — TARGET 256x256. Large CYAN ritual seal ring ground decal, runic glyphs around perimeter, pure top-down; boss-arena floor mark.

## Done criteria
- 10 PNGs (or logged SKIPs) + manifest.md in the batch2 folder; chroma-clean, correct sizes, transparent bg
- Angled arches (1-4) read as SIBLINGS of batch-1 frontals (same palette/stone) with clearly visible left-side depth
- Write summary to CODEX_DONE.md (per-asset status). No code, no import, no commit.
