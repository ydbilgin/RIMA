# Gemini 3.5 Flash — Design/Priority Review (RIMA)

You are giving a SECOND OPINION (design + player-experience judgment, no code) on a plan to take RIMA — a Unity 2D isometric ARPG roguelite — from a "playable skeleton" to a "playable demo". Be concise and opinionated.

## Current state (audited)
End-to-end loop works: MainMenu → CharacterSelect → 3 maps → Victory/Death (runtime-verified). BUT:
- Combat lands damage with ZERO feel: no hitstop, no screenshake, no damage numbers, no enemy hit-flash, no knockback, no slash arc. The signature finisher "Sundered Beat" (BREAK→EXECUTE) never fires.
- The 3 maps are content-identical duplicates.
- Pressing "PLAY AGAIN" soft-locks the run (reloads mid-run map instead of resetting).
- Mobs work mechanically (chase/attack/die) but use placeholder colored-square visuals (intended for now).
- Deferred by owner (DO NOT propose doing now): animations beyond idle, audio, the Slay-the-Spire graph/preview/orb map layer, mob visual upgrades.

## The plan (priority buckets)
**P0 (must-fix so demo isn't hollow/broken):** fix PLAY AGAIN soft-lock + timeScale leak; remove a stale `[Obsolete]` room-manager component causing a dual room-clear conflict; fix an elite enemy that can teleport through walls (unkillable → blocks room clear); fix reward item vanishing in builds; **add a JuiceManager (hitstop/screenshake/damage-numbers/camera-punch) to the 3 play scenes; add knockback + hit-flash to all enemy prefabs; add a slash-arc VFX to the player swing**; gate off 6 broken stub classes in character select.
**P1 (feel good):** real AoE for the rage-dump skill; remove a sprite-sort flicker; add Ronin's animator; make the 3 maps visually distinct; on-camera reward drop; fix skill-list for all 10 classes; verify HUD bar art import.
**P2 (deferred/nice):** enemy death-fade polish; remove dead objects; archive orphan duplicate-HP script.

## Questions (answer numbered, concise, opinionated)
1. Do you AGREE with the P0/P1/P2 split for "make it feel like a game"? If not, what moves?
2. From a PLAYER's first-30-seconds standpoint, what is the SINGLE highest-impact fix here?
3. Is anything MISSING that a player would immediately notice but the plan ignores (within the non-deferred scope)?
4. The fork: the signature finisher needs an attack ANIMATION state to exist, but animations are deferred. (a) Defer the finisher with animations, or (b) add ONE minimal placeholder attack-state now so the finisher can fire? Which gives a better demo for less risk?
5. Any risk in this plan we're UNDERESTIMATING?
