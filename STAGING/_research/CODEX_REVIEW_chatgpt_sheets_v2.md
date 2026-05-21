# Codex Review - ChatGPT v6 Sheets + Hard-Skill Analysis

## Section 1 - Sheet Quality Audit

| Sheet | Class | Skill Count Expected | Skill Count Visible | Sprite Faithful (Y/N) | Painterly Quality (1-5) | Issues |
|---|---|---:|---:|---|---:|---|
| 1 | Warblade | 14 | 14 | Y | 5 | Strong greatsword/heavy armor read. Same skeleton/armored mobs repeat heavily. Captions readable. Perspective consistent. |
| 2 | Ronin | 4 | 4 | Y | 5 | Clean katana/topknot identity. Same undead mob repeated. Captions very readable. Perspective consistent. |
| 3 | Shadowblade | 22 | 22 | N | 5 | Skill count complete and quality high, but canonical is tan male, short black hair, compact dark assassin; sheet reads dark-skinned long-haired/dreadlocked assassin. Dual daggers correct. Repeated undead mob set. |
| 4 | Ranger | 20 | 19 | Y | 5 | Bow/blonde hunter identity is faithful, but only 19 numbered skill panels are visible plus class card. Expected 20. Mobs repeat. Captions readable. |
| 5 | Summoner | 8 | 8 | N | 5 | Quality strong, but canonical has very pale skin and long dark teal-black hair; sheet drifts to orange-haired book caster, closer to Elementalist hair. Spirit/tome identity good. |
| 6 | Gunslinger | 8 | 8 | N | 4 | Twin pistols correct, but canonical is dark brown-skinned female with textured locks/longcoat; sheet reads light-skinned short-haired male/neutral gunner. Captions readable. Smoke Round has soft painterly quality. |
| 7 | Ravager | 8 | 8 | PARTIAL/N | 4 | Berserker body/hair read is close, but weapon identity is a single oversized double-headed axe, not dual axe per user note. Repeated mobs. Strong red VFX. |
| 8 | Hexer | 8 | 7 | Y | 5 | Pale cursed caster identity is faithful, but only 7 skill panels visible plus class card. Expected 8. Shackle Curse chain visible. Captions readable. |
| 9 | Brawler | 8 | 8 | Y | 5 | Canonical fist/gauntlet identity, dark skin, compact boxer stance all match. Repeated undead mob set. Captions readable. |
| 10 | Elementalist | 15 | 14 full + cropped bottom content | N | 4 | Canonical has orange hair and teal-blue caster palette; sheet uses black-haired staff mage. Bottom row is cropped/truncated, so the 15th skill is not fully reviewable/readable. Element VFX are strong. |

### Per-Sheet Notes

- Warblade: Acceptable as live concept reference. Main issue is mob repetition, not class fidelity. VFX avoids the v5 geometric/programmatic look.
- Ronin: Best sheet for caption readability and panel clarity. Low mob variety, but class identity is clean.
- Shadowblade: Quality is high, but sprite fidelity should be failed because the generated character identity does not match the canonical south sprite. This matters if sheets are used as character reference rather than pure VFX reference.
- Ranger: Strong sheet, but missing one visible skill panel for a 20-skill target. Tethering Arrow is readable and useful for later LineRenderer implementation.
- Summoner: Strong skill language, but class character drift is substantial. The orange hair conflicts with the canonical dark teal-black hair and makes it visually overlap Elementalist.
- Gunslinger: Weapon identity passes; character identity does not. If used for reference, use only the gun VFX/action ideas, not the character rendering.
- Ravager: Regenerate priority is high because the weapon signature is wrong for the current user note. If the canonical design is changed back to single huge axe, this becomes a pass; under the task note, it is a fail.
- Hexer: Strong cursed mood and chain/tether reference, but incomplete skill count. Needs the missing 8th panel before being marked live-complete.
- Brawler: Pass. This is one of the most faithful sheets and a good VFX/action reference.
- Elementalist: VFX language is useful, but the sheet is not complete enough as a 15-skill reference because the bottom content is cropped and captions are missing.

## Section 2 - Hard-Skill Analysis Validation

NLM cross-check attempted with the task command, but authentication was expired:

`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "..."`

Result: `Authentication expired. Run 'nlm login'...`

Fallback web/doc cross-check used:
- Unity Line Renderer manual: https://docs.unity.cn/2021.1/Documentation/Manual/class-LineRenderer.html
- Unity Sprite Shape Controller manual: https://docs.unity.cn/Packages/com.unity.2d.spriteshape%4010.0/manual/SSController.html
- Unity Particle System Trails module: https://docs.unity.cn/Manual/PartSysTrailsModule.html
- Risk of Rain 2 Ukulele chain lightning behavior reference: https://riskofrain2.wiki.gg/wiki/Ukulele

| Section | Verdict | Notes |
|---|---|---|
| Category A EASY (~75 skill) | AGREE | The classification is directionally right: melee swings, character-centered auras, static marks, single projectiles, and self-buffs can be handled with PixelLab spritesheets plus ordinary Unity timing. Caveat: Prism Beam and Soul Drain should not be treated as pure PixelLab if they need dynamic endpoint tracking. |
| Category B MEDIUM (~25 skill) | AGREE | Placed traps, summons, clones, teleport/afterimage, fog, and placed VFX correctly need separate objects/prefabs and Unity composition. This category is realistic. |
| Category C MEDIUM-HARD (5 skill) | AGREE | Shackle Curse, Spirit Bind, Tethering Arrow, Soul Link, and Pact Drain are correctly mapped to dynamic caster-target link patterns. Unity LineRenderer supports multi-point world-space lines and tiled textures, so a scrolling/tiled segment material is a valid production path. |
| Category D HARD (3 skill) | AGREE WITH ADJUSTMENT | Chain Lightning and Chain Cull fit sequential runtime composition. Sweep Volley is not really HARD if it is just angled projectile fan/spawn timing; it belongs MEDIUM unless target-seeking per-arrow logic or terrain-aware arcs are required. |
| 6 Industry Patterns (#1-#6) | PARTIAL AGREE | The Unity implementation patterns are valid. However, the named game-specific implementation claims for Hades/PoE/Diablo/Hollow Knight are not publicly verified here and should be phrased as visual/genre analogies, not evidence of exact tech. Risk of Rain 2 chain lightning behavior is externally corroborated, but not its internal implementation. |
| Production budget (6 gens + 12-18 saat Unity) | AGREE | 6 PixelLab texture/impact gens is realistic if reuse is enforced. 12-18 Unity hours is realistic for prototype-quality utilities and hookups, not fully polished combat feel, balance, SFX, hit-stop, pooling, and QA. For shippable feel, budget 2-3x after prototype. |

### Missing / Reclassified Items

- Prism Beam: If endpoint follows a target, classify with LineRenderer/beam utility, not EASY.
- Soul Drain: The doc lists Soul Drain as EASY beam/stream, but if it is a drain tether it should reuse Pattern #1 with Pact Drain.
- Sweep Volley: Reclassify from HARD to MEDIUM unless it is target-jumping or terrain-aware.
- Tethering Arrow: MEDIUM-HARD is correct because pull physics and anchor cleanup are gameplay work, not just VFX.
- Soul Link: The simplified burst version is production-safe, but the doc should explicitly record that it is a simplification option. Persistent ally tether would be MEDIUM-HARD.

### Pattern Library Priority

1. `ChainBinder.cs`: caster-target LineRenderer, tiled texture, UV scroll, lifetime, sorting layer, anchor offsets.
2. `SequentialStrike.cs`: ordered target list, per-jump delay, impact prefab, damage callback.
3. `ProjectileFanSpawner.cs`: arc/spread projectile spawning for Sweep Volley, Fan the Hammer, Multi Shot.
4. `PlacedEffectSpawner.cs`: trap/pin/beacon/frozen-orb placement with telegraph and cleanup.
5. `AfterimageTrail.cs`: dash/teleport afterimage utility.
6. `CurvedChainBinder.cs`: only after straight ChainBinder ships; SpriteShape curved chains are valid but lower immediate ROI.

## Section 3 - v2.3 LOCK Consistency Check

| Item | Verdict | Notes |
|---|---|---|
| O1: B01/B02 mutual exclusivity | Phase 1 blocking for map topology implementation | The prose says v2.3 LOCK, but leaves 16 nodes vs 15 nodes unresolved. If code generation starts now, node graph builders need one answer. Recommended Phase 1 decision: use 14 main + 1 branch slot, with B01 Curse Gate and B02 Forge mutually exclusive per run. This preserves Karar #62 15-node intent and keeps replay variety. |
| O2: Architect ending meta-unlock content | Not Phase 1 blocking | The doc clearly marks Phase 1 as narrative-only and Phase 2+ for meta-unlock content. No current implementation blocker unless reward UI needs a placeholder. |
| O3: Hub Phase 2+ catalog | Not Phase 1 blocking | Phase 1 spend is class unlock only. Cosmetics/starting Imprint/run modifier can wait. |
| Topology revision | NEEDS PATCH NOTE | v2.3 states both 14+2=16 and the alternative 14+1=15. This is intentionally open, but the final conclusion also says the 3 open questions are not Phase 1 blocking. O1 is actually blocking for Phase 1 map generation if graph count is hardcoded. |
| Bug fix integration | MOSTLY COMPLETE | Hybrid Rest 1/Act, Boss Depth 12 auto-unlock, and MacroRoomController-only fragment spawn are explicitly integrated in the relevant sections. |
| 7 Codex Conflict + 3 Antigravity Bug baked-in | COMPLETE ENOUGH | All 10 are listed in Section 4 and reflected in the mechanics sections. O1 remains the only unresolved mechanics conflict with immediate implementation impact. |

### Hidden Conflict Scan

- Act 1 table still contains `N11 Combat 7/6 | Wait - recalculate (see below)` while the final topology says only six combat nodes. This stale row should be removed or rewritten before code tasks consume the table.
- Node naming jumps from N10 to N12 after the recalculation. That is fine if N11 is intentionally deleted, but the table still shows N11, so current docs are ambiguous.
- `Total: 14 main + 2 branch = 16 nodes` conflicts with a 15-node locked topology if branch nodes are both materialized in the same run. Decide O1 before implementation.
- Locked class UI says 6 locked classes have unlock conditions, while the 4-class demo also treats 6 as Coming Soon. If Phase 1 allows Shattered Echoes class unlocks, "Coming Soon" and priced unlocks need UI copy separation: locked-but-unlockable vs post-demo unavailable.
- Boss reward says class-specific Legendary 3-choice, while Act 1 starts with four classes only. This is consistent if "class-specific" means current primary class only.

## Section 4 - Recommendations

### Per-Class Regenerate Priority

| Priority | Class | Action |
|---:|---|---|
| P0 | Ravager | Regenerate or manually correct weapon identity to dual axe if the user note is canonical for this sheet batch. Current sheet fails the class signature weapon check. |
| P0 | Hexer | Regenerate/repair to include the missing 8th skill panel. Current sheet is high quality but incomplete. |
| P0 | Elementalist | Regenerate/repair crop so all 15 skills and captions are visible; also restore orange-haired canonical identity. |
| P1 | Ranger | Repair/regenerate missing 20th visible skill panel. Otherwise strong. |
| P1 | Summoner | Regenerate character identity only if sheets are used as canonical character refs; VFX/action refs are still useful. |
| P1 | Gunslinger | Regenerate if character fidelity matters; keep as gun VFX reference otherwise. |
| P2 | Shadowblade | Regenerate if strict canonical character identity matters. Skill/VFX coverage is complete. |
| P2 | Warblade, Ronin, Brawler | Keep. Only mob variety can be improved later. |

### Industry Pattern Code Library Priority

- Build `ChainBinder.cs` first. It unlocks Shackle Curse, Spirit Bind, Tethering Arrow visual link, Pact Drain, Soul Drain if dynamic, and optional Soul Link flash.
- Build `SequentialStrike.cs` second. It unlocks Chain Lightning and Chain Cull without overproducing PixelLab assets.
- Build projectile fan/spread utility third. It handles Sweep Volley, Multi Shot, Fan the Hammer, and similar skills.
- Defer SpriteShape curved chains until a straight LineRenderer version proves insufficient.

### v2.3 LOCK Patch List Suggestions

- Resolve O1 now as: "Act 1 has 14 main nodes + 1 active branch slot. B01/B02 are mutually exclusive per run. Total runtime node count remains 15."
- Remove stale `N11 Combat 7/6` from Section 1.2 or mark it deleted.
- Add one sentence to Section 5: "O1 must be resolved before map graph implementation; O2/O3 are Phase 2+ non-blockers."
- Split hub UI wording into "Phase 1 unlockable locked classes" vs "post-demo Coming Soon" if all six non-start classes are not actually unlockable in the demo.

## Section 5 - Open Questions (orchestrator/user)

1. Is Ravager's current canonical weapon single huge axe, or is the user note "should be dual axe" now authoritative for v6/v7 sheet generation?
2. Should Phase 1 include unlockable non-start classes, or should all six non-start classes be Coming Soon silhouettes with prices hidden/deferred?
3. Should O1 be locked immediately to mutually exclusive B01/B02, so implementation can preserve 15 runtime nodes?
4. Are the hard-skill industry examples meant to be proof of exact shipped implementation, or just visual/production analogies? Recommended wording is analogy only.
