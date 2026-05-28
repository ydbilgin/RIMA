# Open Decisions — Sabah User'ı Bekleyen

**Status:** 5 karar bekliyor, 2026-05-26 sabahı
**Related:** [[Cliff_System]] [[Walkability_Dash]] [[Reward_Portal_Flow]] [[S107_Overnight_Log]]

## 1. NLM Conflict — Reward Flow Pattern

NLM canonical = Map Fragment + Skill Draft (long-term reward primitives).
S107 plan = Phase 1 Pattern C MVP (existing `SkillOfferGenerator`) → Phase 2 Tarz 2 (3 portal fan) → Phase 3 D (portal+preview).

**Karar:** Phased adoption OK mi? Confirm Phase 1 = Skill Draft only, Map Fragment defers.

## 2. Reachability Constraint (Portal Spawn)

"Stuck alana portal spawn olmasın" — `PortalSpawnAnchor` must validate destination is Player-reachable.

**Plan:** Sonnet follow-up dispatch (flood-fill from Player start, intersect WalkabilityMap, retry anchor placement if unreachable).

**Karar:** Sonnet dispatch'ini kim tetikleyecek (user vs orchestrator auto)?

## 3. Cliff Sprite v2

Python `cliff_generator` output: `STAGING/cliff_bases/cliff_v01.png ... v10.png` (64×96 dimetric mocks).

**Workflow:** PixelLab Web UI S-XL Pro New + init image manuel feed → AI Freedom=0 → output overwrite `Assets/Sprites/Environment/KitB_Cliff/`.

**Karar:** v2 sprites'a şimdi mi geçilecek yoksa current 9 sprite ile gameplay ileri mi gidilecek?

## 4. Portal Sprite (Dikey Yarık)

64×128 dikey rift sprite + 4 idle frame animate + room-type icon inpaint.

**Tool options:** PixelLab Create Object MCP (auto) vs Web UI manual.

**Karar:** Hangi tool, hangi room-type ikonları öncelikli (combat / treasure / elite / boss)?

## 5. Reward+Portal Phase 1 Implement

Sonnet dispatch hazır — MVP `SkillOfferGenerator` wire-up, single portal exit, 3-choice popup.

**Karar:** Dispatch trigger zamanı (sabah ilk iş / başka task'lar sonrası).

## Secondary (Less Urgent)

- **NLM sync retry** — S107 sync partial (~120 hata), sabah retry gerek (NLM API quota reset bekleyebilir)
- **Graphify scope** — corpus too large, user'a sorulacak: Cliff system bireysel graf / full project / specific subsystem
- **Tile regen (long-term)** — 16 floor tile strict 64×32 dimetric vs current `cellSize.y=0.609375` quick fix
- **Scheduled Task wrapper for agy** — brief OpenConsole.exe flash sorununu çözer (~30 dk impl, optional)

## Hard Rules Active (carry, do not break)

1. NO autonomous PixelLab gen
2. Codex/agy = review only; mechanical work = Sonnet
3. "Opus'a sor" = rima-design agent dispatch, NOT orchestrator inline
4. Every dispatch needs "Amaç:" satırı
5. agy_dispatch `--print-timeout` (not `--timeout`)
