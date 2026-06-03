# RIMA — LOCKED DIRECTION (S6, NLM-canon authoritative)

> Source: NLM canonical query (notebook 30ddffa5, 2026-05-30). This is the **single locked direction** for the game.
> Drift rule: NLM canon > local memory > prompt iteration. Anything contradicting this gets corrected to this.
> The ORDERED ROADMAP (what we do, in sequence) is a separate doc built from this + state-audit + cx/ax input.

## 1. North-star (one sentence)
**A ~10-minute, highly-polished vertical slice whose job is to win Steam wishlists** — pure 2D top-down chibi
pixel-art ARPG roguelite, "Hades Elysium V1" look: wall-less floating arenas over a dark abyss, neon cyan rift
energy + warm braziers, the fantasy of fighting on a severed seal-keep fragment.

## 2. Demo scope = LOCKED (1 class, 5 rooms, 1 boss)
- **ONE class: Warblade** — melee greatsword, 8-dir move, 3-hit melee combo, **Rage** resource. (Other 9 classes = deferred.)
- **5 authored rooms:** 3 Combat → 1 Reward/Skill-Draft → 1 Boss.
- **Boss = Penitent Sovereign**, demo = **"2+1" fight**: 50%-HP chains-break beat (live) + lightweight Phase-3 "Unleashed"
  modifier overlay @33% (cooldown×0.8/speed×1.15/biased rotation, commit `ab96137f`; supersede documented in
  `BOSS_MOB_DESIGN_S6.md` §0 + `DESIGN_LOCK_DEMO_S6.md` §1.3). Full raid-scale 3-phase fight (distinct per-phase rosters) still deferred.
- **~4 mobs:** FractureImp (primary) + ShardWalker + HollowHulk + boss.
- **Combat:** cursor-aim melee+ranged, dash, weaponless body + HandAnchor weapon sprite + OrientationSync.
- **Skills:** Hades-style **3-card draft** (Common/Rare) after each room; **4 active skills (Q/E/R/F)**.

## 3. The moment-to-moment loop (canon, per combat room)
`Clear wave → Map Fragment drops (center) → pickup (cyan beam, reveals next room 1-2 ahead) → 3-card Draft → exit Gate unlocks → walk into gate → 0.75s fade-to-black Y-offset teleport → next room`

## 4. Must-have systems (the demo lives or dies on these)
- **Combat juice / 3-layer feedback:** hit-stop 0.04-0.18s, directional screen shake, white hit-flash, floating
  damage numbers, **painterly VFX overlays**.
- **RoomLoader + Gate** state machine (Y-offset teleport, 0.75s fade).
- **Map-Fragment ecosystem** (drop → pickup → cyan beam → reveal → gate unlock).
- **Skill Draft UI** (3-card Common/Rare).
- **Marketing conversion UI:** Death/Victory screen with run stats + next-class teaser + **clickable Steam Wishlist CTA**.

## 5. Locked pivots (these are decided — don't re-litigate)
- **Pure 2D** (2.5D/3D-billboard detour reverted). 64px chibi, 32×32 tiles, URP 2D, PPU64.
- **VFX-first & graybox-combat-first:** animation/weapon-sprite production is **HALTED until combat timing
  (wind-up/active/recovery) is frozen on graybox** — prevents asset churn. ⬅️ this gates art production.
- **Detached weapon mount:** weaponless body + weapon sprite via HandAnchor/OrientationSync. **During a swing the weapon
  stays VISIBLE but is FADED (~0.4 alpha) while a painterly slash-arc VFX trail reads on top — it is NOT hidden.**
  ⚠️ SUPERSEDES the earlier "weapon HIDDEN during swing" wording (3-AI verdict 2026-05-30: full-hide kills weapon identity
  + breaks motion tracking; Hades/Dead Cells/Children of Morta all keep the weapon visible + trail). The reduced opacity
  masks pixel-rotation jaggies and avoids the rotating-cardboard look without removing the weapon. See `WEAPON_HAND_SYNTHESIS_S6.md`.
- **T3 standalone live editor** (Tool.exe + Game.exe via JSON FileSystemWatcher).

## 6. Explicitly DEFERRED / CUT for the demo (vision delayed, not deleted)
Other 9 classes · cross-class passives + ultimates (the core long-term vision) · Acts 2-3 + meta hub (Ferryman,
Sister Mourne) · shop/forge/event/curse/spirit rooms · full raid-scale boss fight (distinct per-phase attack rosters; the
demo's lightweight "2+1" overlay shipped — see §2) + Rift-Tear hazards · real multiplicative
tag synergies (demo = **display-only popups** for screenshot value).

## 7. ⚠️ CONFLICT caught (NLM canon vs this session's VFX synthesis) — Opus resolution
- **`VFX_STRATEGY_SYNTHESIS_S6.md` said: slash-arc = keep native LineRenderer.** That was grounded in the *current
  code* (SlashArcVFX is a LineRenderer today) + a feasibility argument (variable radius/width).
- **NLM canon says: slash-arc = painterly VFX flipbook** that REPLACES the hidden weapon during the swing — it's the
  centerpiece of the whole detached-weapon-mount design, not an afterthought.
- **Opus resolution (canon wins, per drift rule):** the slash **VISUAL = painterly VFX flipbook** (8-dir baked or
  render-texture-snapped to avoid mixels); the **hitbox/timing stays code-driven**. The current LineRenderer is a
  **stopgap**, not the target. → I will correct the VFX synthesis doc accordingly. (Flagging per your "conflict'te
  tekrar sorabilirsin" — but canon is clear here, so I'm deciding rather than blocking. Object if you disagree.)
