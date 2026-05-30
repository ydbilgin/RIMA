# RIMA — DEMO DESIGN LOCK (S6 canonical)

> Status: **LOCKED** · Scope: 5-room demo (3 combat -> reward -> boss) · Brand: cyan rift `#00FFCC` over deep-purple/black void · Boss: Penitent Sovereign · Camera: 640x360 ref-res, PPU 64, integer upscale.
> This document merges 5 design-dimension proposals (Story/Lore, Lighting, Maps, Visual-Screens, Game-Feel) into ONE non-contradictory canon. Where the 5 proposals touched the same fact, the shared answer is recorded once here. All five dimensions converge on a single causal premise: **the island floats because the seal holds it; cyan is the seal's energy; the player maintains that seal by running again.**

---

## 0. THE ONE CAUSAL PREMISE (binds all 5 dimensions)

> **The floor is not floating rock. It is a severed structural fragment of RIMA — an old containment seal-keep — pinned aloft by the active seal that also holds back the Rift March. Cyan `#00FFCC` is the seal's Rift energy bleeding through the wounds (cliff edges = torn cuts, floor cracks = strain seams, central rune = the seal's surface). You are both the one who fractured the world and its last surviving fragment; dying does not lose the run, it re-counts the seal.**

Every other section is downstream of this sentence:
- **Lighting** -> the island is lit *from its own wound* (cyan rift = primary light, warm = a single ritual ember).
- **Maps** -> the 5 rooms are ONE biome (severed seal-keep "The Sundered Brink"), connected by rift-threshold gates cut into the same island, never bridges.
- **Visual screens** -> every background is a framed view of THAT island-in-void; cyan = energy/interactive, never friendly-UI.
- **Game-feel** -> dash-edge forgiveness is "cliff-edge grace" (diegetic to a floating island), not platformer coyote-time.

---

## 1. LOCKED STORY / LORE (demo-scoped, built ON existing NLM canon)

Built on NLM canon notebook `30ddffa5` — not re-invented. This locks only the **demo-readable surface** and resolves the one open contradiction (why it floats).

### 1.1 Why the island floats (CONTRADICTION RESOLVED)
The world did not break by accident. To stop the reality-consuming **Rift March**, every connection between worlds was severed in one irreversible act: **The Fracturing**. The play-floor is a surviving fragment of **RIMA, a containment seal-keep** (Act 1 = "The Shattered Keep" / demo island = "The Sundered Brink"): its thresholds, guard halls, chain galleries and containment arenas were torn loose and left suspended over the void when the connections were cut. **It floats because it is anchored to the seal, not to ground** — the seal holds the fragments aloft the same way it holds the Rift March back. Cliffs are torn edges; void is the cut; cyan is the seal's energy bleeding through. This is canon-grounded (RIMA-as-seal, source `61237986`), NOT a new mechanism.

### 1.2 Cyan + abyss (canon, palette-locked)
The abyss below is the outside void the Rift March marches from. Cyan `#00FFCC` is Rift energy — the volatile outside leaking in through the wounds. Palette law: **cyan = raw energy + world boundary; warm amber `#E89020` = remnant life / ritual; slate = neutral structure.** So cyan-edged cliffs/floor-cracks read "the seal strains here," warm braziers read "someone still keeps a vigil." The "Vivid Vulnerability" tone is diegetic — cyan is the visible *cost* of staying sealed.

### 1.3 The Penitent Sovereign (canon, source `0263dc65`)
Former guardian of this fragment. As self-punishment for what The Fracturing cost, he absorbed the leaking Rift energy into his own body to keep this section of the seal from failing. **His chains are not restraints — they are the metaphor for his self-discipline holding the corruption IN.** "Bowed but unbroken." At **33% HP** his concentration fails, chains shatter, his rift-corrupted true form unleashes (3-phase). He is tragic, never a generic conquering villain. The demo climax is morally loaded: the player must break the very thing keeping the fragment stable.

### 1.4 "One more run" framing (canon, demo = first loop only)
The player is BOTH the perpetrator of The Fracturing AND its last surviving fragment (hero / criminal / victim — deliberately ambiguous). Dying and returning is the **mechanical necessity of maintaining the seal**, not failure. Death copy is cold-observational ("The rift remembers. You won't."). Each loop returns a sliver of who you were (meta = Shattered Echoes). **Demo only needs the first loop to land this.** Full meta-hub, class unlocks, Nexus Core reveal stay OUT.

### 1.5 Per-room narrative beats (text minimal, environment-first)
| Room | Beat | Optional 1-line (typewriter) |
|---|---|---|
| R1 Tutorial / Broken Entry Gate (cyan rune center) | Teach "cyan = boundary/energy" wordlessly via rune + void edges. Combat tutorial only. | none |
| R2 Guard Hall (NW brazier) | First warm note = "a vigil still kept." | EN: *"Someone kept this fire. Long after the order fell."* |
| R3 Ambush Cloister (broken NE column + chain galleries) | Torn structure + hanging chains foreshadow the Sovereign's chains. | EN: *"The chains here lead somewhere. They always have."* |
| R4 Map-Fragment Vestibule (NO mobs, plinth) | Quiet beat. Skill draft framed as **remembering** (recovering a past self). | EN on interact: *"You knew this once."* |
| R5 Penitent Containment (14x14 ritual platform, 4 chain anchors) | Boss. Approach line + title card. | Approach EN: *"The Sovereign's breath is colder here."* Title card: **THE PENITENT SOVEREIGN — He took the wound so the seal would hold.** |

- **Phase-2 transition (33% HP, chains shatter):** 1 beat — *"Discipline breaks before the chain does."*
- **Death screen:** rotate cold canon lines (`"The rift remembers. You won't."` / `"Not an ending. Just a place where you stopped."`) + demo Steam Wishlist CTA + next-class teaser silhouette.

---

## 2. LOCKED LIGHTING (mapped to the live URP 2D rig in `PlayableArena.unity`)

### 2.1 Concept (downstream of 0/1)
**The PRIMARY light source is the CYAN RIFT ENERGY itself, not flames.** The island is lit from its own wound: cool cyan from below/within (rift seams + central rune well + crystalline cliff rim) is the dominant fill; warm `#E89020` is demoted to ONE justified ritual ember near the entrance. This is the exact inversion the live scene needs — today four `Brazier_*_WarmLight` point lights run `#FF8033` @ intensity **4.5** while `Global Light 2D` sits at **0.08** and the rift emits **zero** light, so warm corner-pools read as gas lamps. Flip the hierarchy.

NLM `43003919` HARD rule satisfied: every point light must have a visible prop owner — the rift well, glowing cliff crystal, and rune circle ARE owners; flame-on-posts is the forbidden gas-lamp pattern.

### 2.2 Concrete changes on the EXISTING rig (all Light2D, zero Shadowcaster2D, no bake)
1. **`Global Light 2D`** (~line 8661): intensity `0.08 -> 0.22`; tint cooler toward `#1E1B2E` (r~0.12 g~0.106 b~0.18). Keep `m_ApplyToSortingLayers` = Floor, Decor_Cliff(12), Decor_Floor(13), Gameplay.
2. **ADD `Rune_Pulse_Cyan`** (Point Light2D, the missing main fill) at rune-circle center (~0,0): `#00FFCC` (r:0 g:1 b:0.8), intensity ~`1.1`, inner `0.4`, outer ~`6`, falloff `0.5`; reuse `LightPulse` for ±0.2 slow pulse.
3. **ADD 2-4 `Rift_Seam_Cyan`** Point Light2D on floor-crack positions: `#00FFCC`, intensity `0.5-0.7`, inner `0.2`, outer `2-3` — these replace the warm fill.
4. **Convert `RimLight_*_Cyan`** Point(type 3, outer 1) -> **Freeform**(type 2) traced along the actual void-facing cliff edge; intensity ~`1.2`, FalloffIntensity `0.3` (sharp); target Decor_Cliff(12) + Floor edge + Gameplay.
5. **DEMOTE warm:** keep ONE entrance brazier — intensity `4.5 -> ~1.0`, recolor `#FF8033 -> #E89020` (r:0.91 g:0.565 b:0.125), keep `LightFlicker` ±0.1. **Delete/disable the other 3 `Brazier_*_WarmLight` + the `Pillar_AmberLight` ring.**
6. **Reparent ALL lights** (rim, rune, seam, surviving brazier) under one independent `Scene_Lighting` GameObject — NEVER as children of `RIMA_Cycle2_Dressing`/decor (deactivating decor would silently kill lights = the black-cliff root cause, N3 fix #2).
7. **Verify** at 640x360 play: cool-cyan-dominant, warm = single ember, cliff void-rim glows cyan, gameplay still readable, no warm corner pools. Tune Rune ±0.3 / rim ±0.4. **No Shadowcaster2D at any point.**

### 2.3 Per-room mood ramp (same rig, light only — see also Maps §3)
R1 neutral cool -> R2 slight cyan floor-bleed -> R3 densest rift veins + tighter ambient (tension) -> R4 warm safe lift (brazier-lit Rest, NO combat) -> R5 boss: darkest ambient + brightest boss spotlight + cyan/red rim. Authored via URP 2D Light intensity/color + per-room Global Volume. No new tileset.

---

## 3. LOCKED MAP / CONNECTED-ROOMS

### 3.1 Framing (single most important map decision)
**The 5 rooms are ONE biome — "The Sundered Brink," a single fractured floating island — NOT five biomes.** A 5-room demo lacks the room budget to establish multiple identities (Hades/Dead Cells get cohesion from *dozens* of rooms per biome); five different looks would read as disjointed test scenes. RIMA's lever is **intra-biome coherence** (shared floor tileset + shared cliff family + one palette + cyan-only accent) plus **per-room MOOD via lighting + one hero landmark.**

### 3.2 Connection model — KEEP the canonical gate teleport (NOT bridges)
RoomLoader already implements gate thresholds with a **0.75s fade** (0.3 out / 0.15 hold / 0.3 in), Y-offset teleport, and the map-fragment beat (clear -> fragment drop -> G pickup -> 3-card draft + map +1-hop reveal -> gate unlocks). This is the Hades model and is correct for a void island — literal bridges contradict the setting. The fix is **fiction + continuity**, not mechanism: the gate reads as a rift-threshold cut INTO the same island; the pre-fade and post-fade frames share lighting/palette so the cut feels like one continuous place.

### 3.3 Four hard consistency rules
1. **ONE floor tileset + ONE cliff variant family** across all 5 rooms.
2. **ONE palette + ONE accent:** desaturated cool stone base + `#00FFCC` as the ONLY saturated hue; cyan = "rift/interactive" everywhere (veins, fragment, gate seal, boss tells). Like Uncharted's "yellow = climbable."
3. **Lighting continuity:** constant cool ambient + recurring cyan rift anchor in every room; crossing a gate never resets eye adaptation.
4. **Landmark-per-room:** each room gets exactly ONE hero focal element (`RoomSequenceData.focalElementPrefab` exists) that the brightest light points to. R1 fractured cyan obelisk · R2 broken archway · R3 collapsed rift-spire · R4 brazier/altar (safe-room beacon) · R5 Sovereign's throne dais.

### 3.4 Beauty at the edges, not in floor clutter
Keep the play-floor calm/low-noise (readability). Push the "beautiful" budget into: cliff perimeter, depth backdrop (`RoomBackgroundRig` parallax abyss, Y=1/2 X), AO contact shadows, cyan rift accent. **The floating-island silhouette against the void IS the beauty shot every room shares.** Apply `CLIFF_DEPTH_SYNTHESIS_S114S5.md` rules identically in all 5 rooms (single variant family, exterior-void cut, AO, parallax abyss). Map panel (`MapProgressController`) reveals +1 hop on pickup; node tints must MATCH gate category tints (cyan combat / gold treasure / red boss = Hades door-symbol parity).

---

## 4. LOCKED VISUAL-SCREENS SPEC + EXACT CODEX IMAGE-GEN SIZES

### 4.1 UI language ("Ashen Glyph" — codified in `RimaUITheme.cs` + NLM)
"UI is nonexistent, only information": no hard rectangular borders, no opaque RPG stat-sheet panels, translucent fractured-stone/iron frames, **tarnished gold `#F2BC3D` ONLY for rewards**, cyan rift light for life/active/CTA. Everything pulses or fades; nothing stays fully static. Every menu/death/victory background is a framed view of the island-in-void, never an abstract UI plate.

### 4.2 BRAND CYAN RESOLUTION (critical)
- **`#00FFCC` (green-leaning, canonical rift energy):** ALL diegetic rift/world energy, logo glyph, active-skill glow, **Wishlist CTA accent.** Image-gen prompts MUST pin `#00FFCC` — never just "cyan."
- **`#48E0FF` (blue-leaning, the existing `RimaUITheme.Cyan` constant):** keep ONLY for already-wired incidental UI text (room-name, ready-glow). Low risk, no refactor mid-demo. Flagged for a future unify pass (see Open Questions).

### 4.3 Sizing law (640x360 ref, PPU 64, integer upscale)
- Full-screen art = **exactly 640x360** (or a clean **1280x720** 2x master to downscale). NEVER 1920x1080 ("just in case" blurs under Pixel Perfect Camera).
- HUD primitives stay sub-100px (they live at 1:1 logical px). Icon SOURCE max 2x the on-screen footprint (request 64x64 for a 20px hex slot; never 256px).

### 4.4 Screen-by-screen + asset sizes

**SCREEN 1 — MAIN MENU.** Full-bleed island-in-void low push-in, cyan rift cracks + brazier flicker. Logo lockup centered-upper. Text-only PLAY/SETTINGS/QUIT lower-center-left with cyan rift underline igniting on hover (no boxed buttons). Drifting dust/ember + slow void parallax. Version bottom-right (TextMuted). Translucent dark frame ~10% only behind the button column; NO gold.
- `menu_bg_island.png` **640x360** (island-in-void)
- `menu_bg_island@2x.png` **1280x720** (optional master)
- `logo_rima_glyph.png` **256x96** (wordmark + cyan glyph, transparent)
- `particle_ember.png` **8x8**, `dust_mote.png` **4x4** (additive)

**SCREEN 2 — IN-GAME HUD (overlay, never a panel).** Top-left: HP track **72x4** (fill `#4A9EBF` >60% -> `#C8742A` 30-60% -> `#C42B2B` <30%, pulses <30%) + number label; under it resource bar **48x3** (`#7B3FA0`, invisible at 0, white-glow pulse at full). Top-right: minimap **72x72** flat "Bone Tablet" (NOT iso) in 4px broken-stone frame, shows visited/current/+1-2 only. Top-center: micro room-banner (Screen 4). Bottom-center: 7 hex skill slots (LMB+RMB **20px**, 1-5 **16px**, 5px gap), bg `#0D0D12` @55%, radial-pie cooldown, 1px cyan ready-glow. Low-HP = blood-edge vignette (full-screen additive), NOT more UI.
- `hex_slot_mask.png` **32x32** (white-on-transparent, used at 20/16px)
- `skill_icon_<name>.png` **64x64** source each (white pixel silhouette, cyan accent)
- `minimap_frame_stone.png` **80x80** (4px border, transparent center)
- `minimap_node.png` **16x16** (code-tinted per state)
- `hp_resource_cap.png` **8x8** (optional fractured-iron endcap)
- `lowhp_vignette.png` **640x360** (radial red/black, transparent center, additive)

**SCREEN 3 — DRAFT / SKILL-SELECT (Hades 3-choice, gameplay frozen+dimmed).** Full-screen `#0D0D12` @85% over blurred arena. Three cards slide to center, **~150x210 logical** each, ~24px gap. Card: rarity-tinted hex/diamond icon frame (Common gray / Rare teal `#339999` / Epic purple `#8033A0` / Legendary gold `#CCA626`), white-silhouette icon, NAME (TextPrimary), TYPE+TIER row, 1-line effect (TextMuted), cyan synergy chips. Hover = lift + cyan rim; select = white flash + chosen pulses, rejected fade. Right edge: compact build-summary strip. Bottom: replace-mode row only if slots full.
- `card_frame_stone.png` **160x224** (translucent fractured-stone, 9-slice, transparent)
- `rarity_glow_<tier>.png` **128x128** (4 variants, code-tinted)
- `icon_frame_hex.png` **64x64**
- reuse `skill_icon_<name>.png` **64x64**
- `tag_chip.png` **32x16** (cyan rounded, 9-slice)
- `card_select_flash.png` **160x224** (white additive)

**SCREEN 4 — ROOM BANNER (transient micro-banner, top-center).** ~**320x48**, fade-in 0.4s / hold 3s / fade-out 1.2s (matches `HUDController.ShowRoomName`). Italic room-type line in cyan (`#48E0FF` wired text OK) over a near-invisible torn-parchment / cyan underline rune; combat = enemy-count tick, boss = cyan skull glyph + red underline. NO solid box — soft cyan gradient rule only.
- `banner_underline_rune.png` **320x48** (faint cyan rift-rune, transparent, additive)
- `boss_skull_glyph.png` **32x32** (cyan line-art skull)
- NO opaque banner plate.

**SCREEN 5 — DEATH SCREEN (slow-mo to black, demo CTA gate).** SlowMo (timeScale 0.15, 1.5s) -> freeze -> fade-in 0.8s near-black with faint cyan embers. Center: non-blaming canon line. Below: run-stats (Room/Kills/Time/build, TextMuted). Conversion row: clickable cyan `#00FFCC` **WISHLIST ON STEAM** button (`steam://openurl` + http fallback, ignites on hover), "Copy Build Seed", faint next-class teaser silhouette lower-right, plus "TRY AGAIN [R]".
- `death_overlay.png` **640x360** (near-black, faint cyan ember/vignette, transparent ramp)
- `wishlist_cta_btn.png` **256x48** (dark stone pill, cyan `#00FFCC` edge, Steam glyph + WISHLIST, 9-slice, transparent)
- `next_class_silhouette.png` **128x192** (dark teaser, faint cyan rim)
- `steam_glyph_cyan.png` **24x24**

**SCREEN 6 — VICTORY + WISHLIST CTA (boss defeated = demo complete).** Brief slowMo 0.2 + zoom on fallen Sovereign -> triumphant-but-somber overlay, island in stronger cyan rift bloom (the seal briefly answered). Center-top: "DEMO COMPLETE" + short observational victory line. Center: run-summary card on translucent stone + **tarnished-gold edge (gold allowed — reward moment)**. Center-lower: the SAME cyan `#00FFCC` WISHLIST button (largest CTA in build) + "The full descent awaits." + next-class teaser strip. Buttons: MAIN MENU / PLAY AGAIN.
- `victory_bg_bloom.png` **640x360** (island + amplified cyan rift bloom)
- `summary_card_gold.png` **320x180** (translucent stone, tarnished-gold `#F2BC3D` edge, 9-slice, transparent)
- `wishlist_cta_btn_lg.png` **320x64** (cyan `#00FFCC` edge + Steam glyph)
- reuse `next_class_silhouette.png` **128x192**

---

## 5. GAME-FEEL PUNCH-LIST (ranked P0->P3, concrete numbers)

Finish the ALREADY-WIRED hit-confirm chain; do not rebuild. White-flash is DONE (`HitFlashDriver` -> `Health.OnDamageTaken`, 0.08s, router-independent). Only spark + arc legs are missing — and the cause is **data, not code**.

### P0 — BLOCKING (VFXRouter is data-starved)
- **P0-1:** `VFXRouter.entries[]` is EMPTY, so every `SpawnByTag` returns false -> no spark/arc/kill VFX. Populate minimum tags: `hit_default` + `hit_physical` (hitspark, lifetime 0.5), `kill_default` (burst, 0.8), `dash_default` (trail, 0.4).
- **P0-2:** Confirm the SlashArc prefab field is assigned on the LMB attack. Verify via `VFXBusDemo.PublishHit()` in play mode.
- **P0-3:** Confirm `CameraFollow` reads + ADDS `CameraPunchController.Instance.CurrentOffset` each LateUpdate (the controller deliberately does NOT write the transform). If ignored, directional punch is silently dead.

### P1 — Tune the impact that already plays
- **P1-1 hitstop tiers** (`HitPauseDriver`, timeScale 0, ICD 0.05): normal **0.04** / crit(=finisher) **0.07** / kill **0.12** / boss-death **0.20** via `TriggerPause(float)`. Route the combo finisher (`isCrit=isFinisher`) through the crit tier so swing 3 reads heavier.
- **P1-2 camera kick OPPOSITE swing:** in `CameraPunchController.HandleHit` negate direction -> `Apply(-e.hitDirection, impulse)`. Keep impulse hit **0.08** / crit **0.16** / kill **0.22**, decay 6/s.
- **P1-3 directional shake:** in `ScreenShakeDriver.ShakeRoutine` bias offset along hit axis (~**60% axis / 40% random**) instead of pure `insideUnitCircle`. Keep hit 0.05/0.10, crit 0.12/0.18.
- **P1-4 dedupe timeScale owners:** mark legacy `Core/HitStop.cs` `[Obsolete]` and remove from scenes so ONLY `HitPauseDriver` mutates `Time.timeScale`.

### P2 — Forgiveness + dash feel
- **P2-1 attack buffer:** extend `InputBufferService.Pending` with `Attack` + `RequestAttack()` re-firing LMB when commit clears, window **0.15-0.18s** (mirror dash 0.18). Click during commit/hitstop queues next swing.
- **P2-2 dash-edge grace (cliff-edge "coyote", diegetic):** ~**0.10s** grace in `PlayerController.TryDash` so a dash within 0.10s of leaving a walkable cell still launches. Pair with existing `WalkabilityMap.IsDashableWorld`.
- Knockback (per-step 4/5/8, dur 0.10/0.12/0.18) — KEEP values; ensure dash i-frame (`Health.SetImmune`) is paired with `dash_default` trail so the i-frame reads visually.

### P3 — Audio placeholder hooks (content deferred, call-sites wired NOW)
`AudioManager.cs` is a complete procedural skeleton, auto-bootstraps, already hooked for Hit/Death/Gate/Draft. Add one-liners at existing publish sites: `Play(Sfx.Dash)` after `PublishDash`; distinct crit/finisher accent on the `isFinisher` branch; `Play(Sfx.Shatter)` on `PublishKill`. **Do NOT use `VFXRouter.VFXEntry.soundEffect`** (second divergent audio path) — keep audio centralized in `AudioManager`.

---

## 6. PLACEHOLDER REGISTRY

| # | Placeholder NOW | Replaced BY | Production source |
|---|---|---|---|
| **LORE** | | | |
| L1 | Skill draft "remembering" = simple pick + 1 flavor line | Full Shattered Echoes meta-currency + class-unlock memory | NLM class-unlock table + meta canon `a1e375ae` |
| L2 | Next-class teaser = static Elementalist silhouette | Dynamic next-unlockable by Echo count | NLM class unlock cost table |
| L3 | Boss-intro = text card over cyan rift VFX | Animated rift-rise cutscene + final boss sprite | Boss art GATED (A-archive / RTX-local); 3-phase spec `0263dc65` |
| L4 | Nexus Core / Rift March endgame | OMITTED from demo (Act-3 reveal) | NLM story_lore `61237986` — must NOT hint beyond ambiguous identity |
| L5 | Meta-hub NPCs (Ferryman/Vrel/Cartographer) | Hub scene between runs | full-vision synthesis `354134ca` |
| **LIGHTING** | | | |
| Li1 | Cyan pools = bare radial Light2D gradients | Same Light2D + secondary-texture emission mask (outer 1-2px white x `#00FFCC`) | Python edge-extract -> 32x32 grayscale masks (scratch/), PPU 64 point, NO PixelLab |
| Li2 | Cliff void-rim = dynamic Freeform RimLight only | + secondary-texture emission baked into cliff sprites (void-facing 1-2px white) | Same Python mask pipeline; fallback `cyan_rim_edge_strip` additive 128x32 |
| Li3 | Surviving warm brazier = plain orange Point Light2D | Brazier prop sprite + flame VFX owning the light | Existing PixelLab brazier prop + `brazier_glow_mask` 128x128 amber radial (Python) |
| Li4 | Void bg = existing parallax nebula (L0_Void/L1_Nebula) | Dedicated unlit `void_gradient_bg` `#3A1A4A`->`#1C162E`->black + sparse cyan veins, OUT of every Light2D layer | Python Pillow gradient 512x512 / 1x256 stretch |
| **MAPS** | | | |
| M1 | PlayableArena test floor tileset | Final single coherent floor family (12-swatch palette) | PixelLab top-down tileset; same set in all 5 rooms |
| M2 | `cliff_S.png` (mid-cleanup) | Cliff variant FAMILY (3 heights x 3 textures, stable top-contact) | PixelLab; next art task in CLIFF_DEPTH_SYNTHESIS |
| M3 | RoomBackgroundRig non-seamless sprites | SEAMLESS/tileable layers (688x384 16:9 etc.) | PixelLab; per-RoomType preset = future |
| M4 | `Gate.cs` 8x8 grey procedural placeholder (category-tinted) | Canonical gate sprite (rift-threshold arch, 1.5-2x char, 6-8 frame open anim) | PixelLab; tint mapping already final |
| M5 | Runtime map-fragment GameObject + 48x48 icon | Canonical Broken Stone Tablet sprite (`#00FFCC`, bob ±0.10u@2.2Hz, alpha pulse) | PixelLab `map_fragment_canonical_spec` |
| M6 | `focalElementPrefab` slots mostly empty | 5 hero landmarks (obelisk/archway/rift-spire/altar/throne dais) | PixelLab `create_map_object`, on-palette |
| **VISUAL SCREENS** | | | |
| V1 | `RimaUITheme` procedural rounded-rect frames (Resource/SkillSlot/MiniMap/RoomBanner/SmallPanel) | PixelLab fractured-stone/iron 9-slice at reserved Resource paths (code already loads) | PixelLab `create_map_object` 32-160px, point filter PPU 64 |
| V2 | Skill icons null -> code draws violet/gray fill | White high-contrast pixel-silhouette icons 64x64 -> `SkillBase.icon` on Data/Skills/*.asset | Codex image-gen now; PixelLab final palette-locked later |
| V3 | `MenuDungeonBackground` (iso dungeon residue) | `menu_bg_island.png` 640x360 (island-in-void) at Resource path | Codex image-gen now; PixelLab animated abyss later |
| V4 | Death copy "YOU DIED" | Non-blaming canon rotation | string-table edit, pure code |
| V5 | Wishlist CTA + Victory screen NOT BUILT (rank-5b) | `wishlist_cta_btn.png` 256x48 / `_lg` 320x64 + `VictoryScreen` MonoBehaviour, `Application.OpenURL("steam://openurl/...")` + http fallback | Codex image-gen for button art; code wiring rank-5b |
| **GAME-FEEL** | | | |
| G1 | `VFXRouter.entries` spark/arc/burst = simple cyan `#00FFCC` particle/quad (rank-3) | PixelLab hitspark + slash-arc sprite sheets; **tag contract unchanged** | PixelLab MCP weapon-VFX batch (WEAPON_BATCH_PLAN) |
| G2 | `AudioManager` procedural Noise/Tone/Sweep clips | Real recorded/licensed SFX in clips dict; **call-sites permanent** | Deferred audio pass (Sora+Gemini / RTX-local per memory) |
| G3 | `isCrit = isFinisher` (last combo step) | Real crit-roll (chance + multiplier from stats/affix) | Stat/affix layer; tiers already authored to receive it |
| G4 | Dash trail = placeholder cyan streak | Production afterimage shader/sprite; **tag stays `dash_default`** | PixelLab / shader pass |
| G5 | Shake blend (~60/40) + dash grace 0.10s | Playtest-tuned numbers in serialized inspector / consts (data-only) | Playtest |

---

## 7. CROSS-CUTTING CONTRADICTIONS RESOLVED

1. **Floating floor (the core brief problem):** NEVER write "magically floats" or "natural rock plateau / sky-island whimsy." It is **severed seal-keep architecture pinned by the active seal.** Lore §1.1 is the single source; lighting (lit-from-wound), maps (one-biome severed keep), and screens (always island-in-void) all derive from it. RESOLVED.
2. **Cyan meaning (one law for all 5 dimensions):** `#00FFCC` = Rift energy / world boundary / interactive — NEVER friendly/heal/safe/neutral-UI; NEVER on decorative props. Warm `#E89020` = remnant life/ritual. Slate = structure. Gold `#F2BC3D` = reward moments ONLY. The brand cyan `#00FFCC` vs UI-constant `#48E0FF` split is resolved per §4.2 (pin `#00FFCC` for all diegetic/brand/CTA; keep `#48E0FF` only for wired incidental UI text).
3. **Lighting = the map-cohesion tool:** the same change that motivates the light physically (rift energy that holds the island aloft also lights it) is the fix the live scene needs AND the per-room mood lever for "one biome, modulated by light." Story, render, and map cohesion converge with no new art.
4. **Connection model:** gates (rift-thresholds cut into the same island), NOT bridges/teleport pads — bridges contradict the void setting and the canonical single-scene Y-offset gate (Karar #149). Map-panel node tints MUST equal gate category tints (Hades door-symbol parity).
5. **Penitent Sovereign tone:** tragic self-punishing guardian holding corruption IN; chains = self-discipline. Never menacing-evil. Intro/death/phase-2 copy keep this.
6. **Death = seal-maintenance, not failure:** cold-observational copy only; no comfort, no blame, no "try again!" cheer. Demo CTA (Wishlist + next-class teaser) layered on top without breaking tone.
7. **Demo scope discipline:** do NOT surface Nexus Core, Rift March endgame, multi-class memory, or hub NPCs. Demo = first loop + ambiguous identity hint only.
8. **Two timeScale owners:** `HitPauseDriver` (live, bus) vs legacy `Core/HitStop.cs` (different values, no bus) — retire HitStop (P1-4) so they don't stomp `Time.timeScale`.
9. **Two audio paths:** keep audio in `AudioManager` only; leave `VFXRouter.VFXEntry.soundEffect` null (would double-fire / diverge).
10. **No Shadowcaster2D, no baked glow:** organic tilemap = perf collapse; baked+dynamic overlap breaks pixel-art illusion. Under-island darkness = static `CliffDropShadowTilemap` (unlit/multiply). Cyan = Light2D + secondary-texture emission only.
11. **Void bg never a Light2D target:** if combat flashes light it, it reads as a flat cardboard cutout and kills the floating-in-void illusion.
12. **Icon canvas vs on-screen size:** generate icon SOURCE at 64x64 (max 2x footprint), let Unity point-downscale; never request 256px hero icons for 20px HUD slots; full-screen art at exactly 640x360 (or 1280x720 master).

### BOSS-ROOM FLOW (single watch-item all dimensions flagged)
Memory bug: boss death currently mis-routes to **class-select** via a wrong MapFragment alarm. Canon-correct demo close = Room 5 fires **`RaiseDemoComplete`** -> Victory screen + Wishlist CTA (§4.6), NOT the fragment/draft chain, NOT a mid-run class pick. Room 5 is **gate-less** (adding a gate re-introduces a softlock). Lore-side this is "the first loop closes."

---

## 8. OPEN QUESTIONS (require Opus / user decision)

1. **Brand-cyan unify:** keep the two-cyan split permanently (`#00FFCC` brand + `#48E0FF` incidental UI) or do a dedicated pass to unify `RimaUITheme.Cyan -> #00FFCC`? (Deferred to avoid mid-demo regression; needs a decision before final polish.)
2. **Boss-room mis-route fix ownership:** confirm WHO/WHEN fixes the boss -> class-select MapFragment alarm so Room 5 fires `RaiseDemoComplete`. This is the single demo-blocking flow bug; lock the fix into the rank-5b Victory work.
3. **Audio production lane:** Sora+Gemini vs RTX-local vs licensed — which lane for the deferred SFX pass (call-sites are wired now per P3)? Affects timeline only, not code.
4. **Boss art gating:** L3/boss-intro cutscene art is gated (A-archive / RTX-local). Confirm whether the demo ships with the text-card placeholder (§L3) or waits on the animated rift-rise — affects demo-readiness date.
5. **Per-room backdrop presets:** ship the demo with ONE shared parallax abyss backdrop (current lock) or build the per-RoomType backdrop preset system (M3 future) for the boss room specifically? (Recommended: one shared for demo; preset = post-demo.)
6. **Skill-hit feel parity:** skills/projectiles do NOT currently publish `CombatEventBus.OnHit` (only `BasicAttackBehaviorBase` does) — hitspark/hitstop/camera-punch fire on basic attacks but are SILENT on skill hits. Decide whether to add skill hit-publishing for the demo or accept inconsistent feel on skills.

---

### Canonical source references
- NLM notebook `30ddffa5-292f-4248-8e77-68074af901be` (Fracturing/Rift March/RIMA-as-seal `d480e169`/`a1e375ae`/`61237986`; Sovereign `0263dc65`; tone `edfe3a3e`; lighting `8701f51b`/`e69c369a`/`43003919`/`1c685c90`; 5-room `2af8ef85`; Shattered Keep `2c478c8f`/`a15b9a59`; full-vision `354134ca`)
- `STAGING/N3_LIGHTING_DESIGN_FINAL.md`, `STAGING/CLIFF_DEPTH_SYNTHESIS_S114S5.md`, `STAGING/MOMENT_SPEC_S6.md` (rank-3 VFXRouter.entries), `STAGING/INTEGRATION_BACKLOG_S6.md`
- `Assets/Scenes/Test/PlayableArena.unity` (live light mis-config), `Assets/Scripts/UI/RimaUITheme.cs`, `HUDController.cs`, `SkillBarUI.cs`, `Core/DeathScreenManager.cs`
- `Assets/Scripts/Combat/VFXRouter.cs`, `Juice/HitPauseDriver.cs`, `Core/HitStop.cs`, `Juice/CameraPunchController.cs`, `Juice/ScreenShakeDriver.cs`, `Juice/HitFlashDriver.cs`, `CombatEventBus.cs`, `BasicAttack/BasicAttackBehaviorBase.cs`, `InputBufferService.cs`, `Player/PlayerController.cs`, `Audio/AudioManager.cs`, `Core/KnockbackComponent.cs`
- `Assets/Scripts/Systems/Map/RoomLoader.cs`, `Environment/Gate.cs`, `Environment/RoomTypeData.cs`, `UI/Map/MapProgressController.cs`

---

## 9. OPUS RATIFICATION + OPEN-QUESTION DECISIONS (S6, 2026-05-30)

**Status: RATIFIED — this is the canonical demo design lock.** Three sources converged with no blocking conflict: workflow synthesis (this doc) + agy "Shattered Keep" premise (= same seal-keep core) + cx code-architecture. Opus decides the §8 open questions (user AWAY, "Opus karar verici"):

1. **Brand-cyan:** KEEP the split for the demo — `#00FFCC` for all diegetic/rift/logo/CTA, `#48E0FF` for already-wired incidental UI text. Unify pass = POST-demo polish (no mid-demo refactor / regression).
2. **Boss class-select misroute:** OWNED by PHASE 1 Batch A item A1 (cx-yekta, in flight) + rank-5b Victory. Room 5 stays gate-less; fires `RaiseDemoComplete` only.
3. **Audio lane:** **Sora (ChatGPT Plus) + Gemini Pro** (user-specified). DEFERRED; call-sites wired NOW (P3). RTX-local = fallback.
4. **Boss art:** demo ships with the **text-card placeholder (§L3)** so the loop is autonomously completable. Animated rift-rise + real boss sprite = GATED (user art decision, post).
5. **Per-room backdrop:** ONE shared parallax abyss for the demo. Per-RoomType preset = post-demo (M3).
6. **Skill-hit feel parity:** ADD skill `CombatEventBus.OnHit` publishing for the demo (hitspark/hitstop/camera-punch must fire on skill hits too). Small code, big consistency → folded into PHASE 1 game-feel.

**cx code-architecture folded (HOW to build):** Lighting → `RoomLightingProfile` SO + `RoomLightingController` on `RoomLoader.OnRoomChanged` (explicit role refs, NOT blind `FindObjectsOfType`). Maps → `RoomConnectionProfile` fields on `RoomSequenceData`, applied in `BuildRoomContent`; `DungeonGraph` untouched (linear demo). Screens → `RimaUIScreen` base + `RimaScreenRegistry` + `RimaBackgroundImage` helper; `RimaUITheme` stays the token source; `UIManager` stays sole timeScale owner. ⚠️ use the LIVE `Systems/Map/RoomLoader.cs`, NOT `Map/Runtime/RoomLoader.cs`. ScreenShakeDriver transform-write → convert to additive offset (P1-4).

**agy feel folds (added to §5):** chromatic impact frame (2-frame cyan→purple screen color-split on crit/finisher) = P2 add · cliff-edge slate dust on dash/run near void edge = P3 (diegetic) · emissive cyan weapon ghost-trail = G4 · boss chain-break (33%) 0.1s freeze + zoom = §1.5 phase-2 beat.

**Execution:** PHASE 1 (design-independent) building NOW (Batch A `bw95dfno4` in flight). PHASE 2 (lighting/story/map/screens) is now UNBLOCKED by this lock. Roadmap + writer→reviewer routing: `STAGING/MASTER_PLAN_S6_AUTONOMOUS.md`.
