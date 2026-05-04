# CURRENT STATUS
**2026-05-02 · S43 · Phase 1**

## Active Block
Core 4 skill implementation pass completed; ready for review/playtest.

Current implementation pointer:
`STAGING/CORE4_SKILL_IMPLEMENTATION_POINTER_2026-05-03.md`

Latest input/settings/lighting pointer:
`STAGING/INPUT_SETTINGS_LIGHTING_FOLLOWUP_2026-05-03.md`

Latest visual/settings/class/lighting feedback pointer:
`STAGING/VISUAL_SETTINGS_CLASS_LIGHTING_FEEDBACK_2026-05-03.md`

Latest logo/branding pointer:
`STAGING/RIMA_LOGO_RIFTMARCH_BRANDING_NOTE_2026-05-03.md`

Latest Hades-like attack-facing fix pointer:
`STAGING/HADES_LIKE_ATTACK_FACING_FIX_2026-05-03.md`

Latest map layout/spawn pass pointer:
`STAGING/RIMA_MAP_LAYOUT_AND_SPAWN_PASS_2026-05-03.md`

Latest dungeon story mapping / black edge fix pointer:
`STAGING/RIMA_DUNGEON_STORY_MAPPING_AND_BLACK_EDGE_FIX_2026-05-03.md`

Latest locked room staging / map variant decision pointer:
`TASARIM/ROOM_STAGING_AND_MAP_VARIANTS_DECISION_2026-05-03.md`

Latest connected room generation / act evolution proposal pointer:
`TASARIM/ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md`

Latest PixelLab environment module notes pending Claude pointer:
`TASARIM/PIXELLAB_ENVIRONMENT_MODULE_NOTES_PENDING_CLAUDE_2026-05-03.md`

Latest verification:
- Unity compile/validation clean for game scripts touched in Core 4 pass.
- EditMode tests: 129/129 PASS.
- Play Mode smoke: all four primary classes switch correctly and receive canonical default slots.
- Follow-up input/settings/lighting pass:
  - Default attack/skill aim is now mouse/cursor.
  - Attack facing is briefly locked so moving in one direction and attacking the opposite direction turns the character for the attack.
  - ESC menu now has gameplay toggles plus temporary test class switch buttons.
  - Lighting uses cool global fill plus stronger local room accents.
  - Verification screenshot: `Assets/Screenshots/debug_game_view_lighting_settings_facing_2026_05_03.png`
- Latest visual/settings/class feedback pass:
  - ESC/test class switching now changes player class appearance through `PlayerClassManager`.
  - Script validation clean for `PlayerClassManager`, `CharacterSelectScreen`, `CharacterSelectTests`.
  - EditMode `CharacterSelectTests`: 7/7 PASS.
- Latest Hades-like attack-facing pass:
  - Movement facing and combat facing are now separate in `PlayerController`.
  - Attack/skill startup uses a short combat-facing override so held movement does not immediately
    overwrite cursor attack direction.
  - `PlayerAnimator` applies combat-facing immediately while the override is active.
  - Follow-up hotfix after manual FAIL: active combat-facing override now sends Animator
    `Speed=0` so current idle-only directional controllers can visibly switch to cursor-facing
    startup while movement input is held.
  - Follow-up correction: combat-facing override now lasts `0.18s` and, when it ends while
    movement is still held, `PlayerAnimator` immediately returns to movement-facing instead of
    lingering on attack-facing.
  - Script validation clean for touched files; Unity console showed no game compile errors.
  - Unity `execute_code` smoke confirmed override behavior:
    `during=(-1,00,0,00) after=(1,00,0,00) overrideAfter=False`.
  - Additional Animator smoke confirmed controller gating:
    `Speed=1` stayed on `warblade_idle_south`; `Speed=0` with SW direction switched to
    `warblade_idle_SW`.
  - Latest automation smoke confirmed held-movement return:
    `before speed=1 dir=(1,-1); during speed=0 dir=(-1,-1) lock=0.18; after speed=1 dir=(1,-1)`.
  - EditMode test runner failed to initialize before running tests; see pointer.
- Latest Unity freeze/performance finding:
  - `LargeDungeonMapPainter.paintOnAwake` was still enabled in `_IsoGame.unity`, so the large
    tilemap could be painted once in `Awake` and again by `RuntimeRoomManager.StartRoom()`.
  - Disabled `paintOnAwake` in script default and scene instance; `RuntimeRoomManager` remains the
    single room-paint authority.
  - Unity process was responsive at OS level, but some MCP calls still timed out; avoid further
    test-runner calls until Unity/MCP is restarted or stabilized.
- Latest RIMA style-anchor lighting/camera pass:
  - Main Camera edit/default framing is centered on the room/player read, with darker background
    and `orthographicSize=6.15` for a more reference-like isometric room angle.
  - Global 2D light lowered to cold dark fill: intensity `0.26`, color `(0.25, 0.31, 0.38)`.
  - Added `RoomMoodLightPool` radial overlay component so warm/cold light pools remain visible
    even when current tile materials do not respond strongly to point Light2D.
  - `_IsoGame.unity` now has a saved `RIMA Lighting Preview` root with warm torch, cold rift,
    cool sconce, moon fill, and ember accent pools.
  - Runtime procedural room lights now also create matching `Room Light Pool` overlays.
  - Verification screenshot: `Assets/Screenshots/debug_after_lighting_angle_vfx_preview_2026_05_03.png`.
- Latest movement/combat-facing + collision/shadow follow-up:
  - `PlayerAnimator` now stores the pre-combat movement visual facing and restores that exact
    facing when combat override ends while movement is still held.
  - Automation smoke confirmed: `before=(1,-1) speed=1; during=(-1,-1) speed=0; after=(1,-1) speed=1`.
  - Core enemy AI rigidbodies are now configured as kinematic with full kinematic contacts so
    the player cannot push mobs around by walking into them; AI/knockback still drive velocity.
  - Added `GroundBlobShadow`; Player and key enemy scripts create Unity-side blob shadows at
    runtime instead of depending on source sprites to contain shadows.
- Latest map layout/spawn pass:
  - `LargeDungeonMapPainterBase` combat template cycle expanded from 9 to 15 layouts:
    added CrescentSanctum, BrokenCauseway, ReliquaryLoop, ForkedOssuary, AmbushCloister,
    and RiftWell.
  - Each new layout has a floor-mask silhouette, interior wall/pillar feature pass, and
    warm/cool/magic light socket pattern.
  - Enemy spawning is now banded instead of pure random: flank, side, rear/lower pressure,
    diagonal pockets, and wider elite bands.
  - Spawn validation now avoids walls, player/center area, existing spawn positions, and
    collider overlaps before falling back.
  - Tool decision remains: use LDtk first or Tiled second for authored room masks/metadata,
    keep Unity Tilemap as renderer/import target, defer WFC to micro-detail only.
  - Verification: `RuntimeRoomManager.cs` script validation PASS; Unity assembly smoke confirmed
    room index 1..15 maps through all expected layouts and sizes.
- Latest dungeon story mapping / black edge fix:
  - Combat room selection now follows narrative dungeon bands instead of a flat modulo list:
    threshold -> ossuary -> sanctum -> rift.
  - Special rooms now map to meaningful templates: Chest -> ReliquaryLoop, Forge ->
    BrokenCauseway, Elite -> AmbushCloister/ForkedOssuary, Event -> CrescentSanctum/RiftWell,
    Boss -> BossAntechamber.
  - Opening/default room size increased to `220x150`; other layout sizes increased as well.
  - IMPORTANT correction after Unity crash feedback: the large `cameraSafetyFloorPadding=80`
    apron was too brute-force and likely unsafe because it produced about `117800` floor tiles.
    It has been reduced to default `0` with a hard runtime cap of `16`; do not use giant
    tile floods to hide black edges.
  - Unity closed before post-correction script validation could run through MCP. Local scene file
    size check shows `_IsoGame.unity` is about `586 KB`, so the huge test tilemap was not left as
    a multi-MB serialized scene bloat.
  - Correct visual direction: hide map edges with authored high perimeter walls/foreground
    occluders, camera clamp, and small overscan only. This is closer to Hades-like room staging
    than filling the entire off-room area with floor tiles.
  - Main Camera fallback background changed to dark stone-blue instead of pure black.
  - Local tooling check: Unity `com.unity.2d.tilemap.extras` is installed at `6.0.1`.
  - Tool direction: use LDtk first or Tiled second for authored room metadata; keep Unity
    Tilemap/Tilemap Extras as renderer and rule-tile layer.
  - Verification screenshot:
    `Assets/Screenshots/debug_after_narrative_map_player_center_no_black_2026_05_03.png`.
- Latest locked room staging / map variant decision:
  - Large playable floor floods are rejected as the black-edge fix.
  - RIMA rooms now lock to playable combat floor plus non-playable visual shell, authored
    perimeter walls/foreground occluders, void backdrop, and camera clamp.
  - Act 1 room language: Shattered Ruins / Sunken Keep, an old controlled structure broken by
    the Fracturing. Variants should range from built/orderly to collapsed to rift-torn while
    keeping the combat question authored and readable.
- Latest connected room generation / act evolution proposal:
  - Room naturalization should be connected, not random scatter.
  - Build semantic skeleton first, then derive connected masks/influence fields for collapse,
    rift, chain/prison, reliquary/ossuary, lights, and floor detail.
  - Proposed act form language: Act 1 controlled ruins broken by Fracturing; Act 2 living wound
    consuming architecture; Act 3 clean/strange void-gold reality architecture; Final mirror-like
    Nexus Core spaces.
- Latest PixelLab environment module note:
  - Pending Claude final decision; not locked.
  - PixelLab should generate reusable environment modules, not final playable rooms.
  - Floor tile size is not locked to `32px` or `32x64`.
  - First test recommendation: Create tiles PRO, Isometric, `64px`, view angle about 45,
    thickness 0%, then accept by measured output footprint near `64x32`.

Current user feedback to address next:
- Re-test Hades-like attack-facing after hotfix in Play Mode: hold movement one way, attack toward
  cursor in another direction, player should switch to cursor-facing startup briefly, then return
  to held movement-facing without releasing movement.
- Logo/name direction: main read is `RI MA` / `RIMA`, with hidden broken `RI'ft` and
  `MA'rch` secondary read toward `Rift March`.
- ESC/test class switching must change the player class appearance too, not only skills/default slots.
- Current settings menu is rejected as final direction; treat it as temporary debug/settings UI and redesign it.
- Current dungeon lighting is rejected as too uniform; rooms need authored light pools/contrast/identity.
- Current procedural rooms now have more silhouettes, spawn bands, and narrative selection.
  The brute-force no-black floor apron was disabled for stability; next visual fix must be
  authored perimeter wall/occluder pieces plus camera framing, not huge extra tile fill.
- Locked map staging decision: no giant floor padding. Use non-playable visual shell, thicker
  perimeter architecture, foreground occluders, void backdrop, and camera clamp.
- Connected map generation proposal is ready for Claude final review: no independent random prop
  scatter; details should come from sources/fields and evolve by act.
- PixelLab environment module size/tool choice is pending Claude final review. Do not lock `32px`
  or `32x64` until a measured test confirms the correct flat `64x32` visible floor footprint.
- Environment should move toward `rima_style_anchor` mood while regenerating proper floor/wall modules.
- Review/playtest Core 4 skill kits and tune obvious feel/balance issues.
- Add `SkillAnimationBridge` after final skill clip naming and direction policy are locked.
- Continue natural dungeon/map polish after skill review.

## PixelLab Research (IN PROGRESS)
- Previous: 15 YouTube tutorials (qwen2.5:14b text-only, no frames) + Discord #share-tips-tricks
- **NEW: YouTube re-analysis with Gemini 2.5 Pro (native video, frame+audio)**
  - 38 priority videos (score >= 4), Antigravity running Tools/youtube_gemini_pipeline.py
  - Output: STAGING/youtube_analysis/<id>/analysis.md + SYNTHESIS.md
- **NEW: Discord monitoring pipeline set up**
  - Task file: STAGING/discord_analysis/DISCORD_MONITOR_TASK.md (Antigravity reads this)
  - Channels: #mcp-and-vibe-coding, #api-and-sdk, #share-your-tips-and-tricks, #announcements, #help-questions-support, #pixellab-art-gallery
  - Automation risk: Playwright = ban risk on main account. PyAutoGUI = safe. See MEMORY/discord_automation_risk.md
  - To run: give Antigravity the DISCORD_MONITOR_TASK.md path
- Local pipeline tool: Tools/youtube_pipeline.py (qwen2.5vl:7b fallback, frame-by-frame)

## 2026-05-02 Session #2
- Direction mapping fixed: 12 .anim files corrected, PlayerAnimator flipX bug removed
- Anchor rotation files renamed to true visual content (10 classes x 8 files)
- Unity Animator controllers updated: Speed < 0.5 condition on all idle transitions (walk-ready)
- STAGING/PRODUCTION_GUIDE_S43.md created (canonical pipeline reference)
- Discord analysis synthesized -> 5 memory files updated/created
- animate-with-text cost corrected in memory: 40 gen -> 1-9 gen (v0.4.92)
- **Hades-style 4-direction system confirmed as canonical approach for all characters**
  - [CODEX 2026-05-03] Superseded: visual directions are now diagonal `SE/NE/NW/SW`, not flat cardinal front/profile/back.
  - [CODEX 2026-05-03] Superseded again after frame-by-frame check: canonical player facing is now 8-way `S/SE/E/NE/N/NW/W/SW`.
  - Full spec: STAGING/PRODUCTION_GUIDE_S43.md

## 2026-05-03 Session (latest) [CODEX]
- [CODEX 2026-05-03] Skill system research + application done from
  `STAGING/SKILL_SYSTEM_ANALYSIS_PROMPT_NOTES.md`.
  - Added decision doc: `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md`
  - Updated routing locks: `TASARIM/GLOBAL_REPEAT_RULES.md`,
    `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
  - Added memory pointer: `MEMORY/project_skill_offer_system.md`,
    `MEMORY/INDEX.md`
  - Summary: Hades-style 3-choice room offers, active/passive routing,
    PixelLab feasibility tiers, cinematic-skill simplification, minimum tag set.
- [CODEX 2026-05-03] Superseded after user review: runtime player visual facing is now
  4 diagonal Hades-like quadrants only: `SE/NE/NW/SW`. `S/E/N/W` cardinal facing states
  should not be used for production movement/run.
- [CODEX 2026-05-03] PlayerAnimator now outputs only diagonal `DirX/DirY` pairs (`+/-1,+/-1`).
  Pure horizontal/vertical input preserves the previous missing axis so the character does not
  snap to flat profile/front/back in isometric play.
- [CODEX 2026-05-03] PlayerController now starts at `SE`, keeps deadzone-safe movement facing,
  and adds `AttackAimMode`: attacks/skills can use either last character facing or mouse cursor
  direction. Mouse mode does not drive walking facing continuously; it turns the character only
  when an attack/skill is fired, Hades-like.
- [CODEX 2026-05-03] Settings UI updated with attack/skill aim mode toggle:
  `SON YON` = character last facing, `MOUSE` = cursor direction.
- [CODEX 2026-05-03] SettingsMenuUI now auto-initializes at runtime with an overlay canvas;
  pressing ESC opens/closes the settings menu even when no SettingsMenu object is placed in the scene.
- [CODEX 2026-05-03] Warblade animation guide rewritten again for 4 diagonal run-only production:
  generate `run_SE`, `run_NE`, `run_NW`, `run_SW` only.
- [CODEX 2026-05-03] Verification after aim-mode change:
  - Script validation: no compile errors in PlayerController, PlayerAnimator, SettingsMenu, SettingsMenuUI.
  - EditMode tests: 128/128 PASS (`RIMA.Tests.EditMode`).
- [CODEX 2026-05-03] Verification after ESC settings integration:
  - Script validation: no compile errors in SettingsMenuUI and PlayerController.
  - EditMode tests: 128/128 PASS (`RIMA.Tests.EditMode`).
- [CODEX 2026-05-03] Correction after user feedback:
  - Mouse aim mode now affects only attack/skill cast direction, not walking facing.
  - Basic attack calls `PlayerController.FaceCombatTarget()` before hit/VFX/anim trigger.
  - `SkillBase.TryActivate()` calls `FaceCombatTarget()` before skill execution, so all skill
    classes inherit the same Hades-like attack aim behavior.
  - Verification: script validation no compile errors in PlayerController, PlayerAttack,
    PlayerAnimator, SkillBase; EditMode tests 128/128 PASS (`RIMA.Tests.EditMode`).
- [CODEX 2026-05-03] Direction-turn research/action:
  - Hades reference: Hades has Attack at Cursor + Aim Assist options; Hades 1 had visible tank-turn
    behavior, while Hades II improved this with turn animations and subtle leaning blends.
  - RIMA 4-diagonal transition matrix checked: only opposite pairs are high-risk hard cuts:
    `SE<->NW` and `NE<->SW` (4 directed transitions). Adjacent transitions are lower-risk.
  - PlayerAnimator now delays visual movement-facing switches slightly: adjacent turn delay 0.05s,
    opposite turn delay 0.10s. Attack/skill `FaceCombatTarget()` bypasses this delay and updates
    visual combat facing immediately.
  - Verification: script validation no compile errors in PlayerAnimator, PlayerController,
    PlayerAnimatorDirectionTests. EditMode test runner failed to initialize twice due MCP/Unity timeout;
    no game compile errors were present in console.
- [CODEX 2026-05-03] Direction/facing conclusion for Claude:
  - System-level behavior is now close to the Hades principle: movement direction, combat aim,
    and visual facing are separated.
  - Mouse aim is attack/skill-only, not walking-facing control.
  - Movement visual facing uses 4 diagonal quadrants with small hysteresis to reduce hard cuts.
  - Attack/skill facing bypasses movement hysteresis so combat still feels responsive.
  - Remaining gap is polish/assets, not core logic: needs `run_SE/NE/NW/SW`, attack startup frames
    that hide direction changes, and optional future turn/anticipation clips for 180-degree changes.
- [CODEX 2026-05-03] Unity direction system corrected to 8-way Hades-like visual facing:
  `idle_S`, `idle_SE`, `idle_E`, `idle_NE`, `idle_N`, `idle_NW`, `idle_W`, `idle_SW`.
  - Superseded by 4-diagonal decision above.
- [CODEX 2026-05-03] Wave-1 class controllers rebuilt for 8-way idle:
  Warblade / Elementalist / Ranger / Shadowblade.
- [CODEX 2026-05-03] Idle clips re-bound frame-by-frame to the correct 8 anchor sprites from
  `Characters/anchors/<class>/rotations/`.
- [CODEX 2026-05-03] PlayerAnimator now snaps movement to nearest 45-degree sector and preserves last
  snapped facing on stop; PlayerController ignores deadzone/noise before updating facing.
- [CODEX 2026-05-03] Warblade animation guide rewritten in Turkish ASCII for 8-way Hades-style run pipeline:
  `STAGING/WARBLADE_ANIMATION_PIPELINE.md`.
  - Superseded by 4-diagonal run-only guide above.
- [CODEX 2026-05-03] Skill sheet evaluation written:
  `STAGING/SKILL_SHEETS_CODEX_EVALUATION.md` -- review of `RIMA_skill_sheets` visual consistency; verdict: strong concept refs, not direct production canon.
- [CLAUDE/CODEX 2026-05-03] Tile/wall/object production context captured:
  - Guide written: `GUIDES/TILE_WALL_OBJECT_PRODUCTION_GUIDE.md`.
  - MCP verdict: static environment assets are good MCP targets. Use `create_tiles_pro`
    for consistent isometric floor batches, `create_map_object` for walls, `create_object`
    for props/landmarks.
  - Stone Dungeon floor pass completed. Initial 4 one-off floor tiles were moved to
    `Assets/Sprites/Environment/StoneDungeon/Rejected/`.
  - `create_tiles_pro` produced 16 floor candidates; visual QC selected 6 final Stone Dungeon
    floor tiles: `stone_base`, `stone_cracked`, `stone_worn`, `stone_damaged`,
    `stone_grate`, `stone_minimal`.
  - Final floor sprites: `Assets/Sprites/Environment/StoneDungeon/Tiles/`.
  - Final tile assets: `Assets/Tiles/StoneDungeon/`.
  - Tile palette: `Assets/TilePalettes/StoneDungeon_Palette.prefab`.
  - Mossy Crypt candidates separated for later biome work:
    `Assets/Sprites/Environment/MossyCrypt/Tiles/moss_floor_pro_0..2.png`.
  - Stone Dungeon wall candidates generated: `Assets/Sprites/Environment/StoneDungeon/Walls/stone_wall_pro_0..15.png`.
  - [CODEX follow-up] Wall work completed for test scene:
    selected `stone_wall_pro_0/1/4/5/10`, added 5 wall Tile assets under
    `Assets/Tiles/StoneDungeon/`, added them to `StoneDungeon_Palette` row 1,
    and repainted `_IsoGame.unity` with a larger 40x28 outer room / 36x24 floor interior.
  - Visual mockup generated: `STAGING/stone_dungeon_room_mockup.png`.
  - Wall QC notes: `STAGING/STONE_DUNGEON_WALL_QC_2026-05-03.md`.
  - QC verdict: PASS for test scene. Caveat: selected wall sprites read as block/parapet
    border, not final tall wall faces; final environment should still get a dedicated
    straight/corner/pillar wall pass.
  - [CODEX floor QC] Floor visual QC added: `STAGING/STONE_DUNGEON_FLOOR_QC_2026-05-03.md`.
    Verdict: PARTIAL/FAIL for final continuous floor. Current selected floor tiles read as
    overlapping raised slabs, not a single natural floor. Measured opaque bounds are roughly
    `63x43` on `64x64` PNGs; target footprint should be `64x32`. Keep current floor only as
    temporary test-scene placeholder and regenerate flat top-surface floor tiles before final art.
- [CODEX 2026-05-03] Unity scene visibility/camera repair after environment repaint:
  - Cause: `_IsoGame.unity` had two YAML separator newline breaks from the tilemap rewrite:
    `e33: 1--- !u!...` before the Walls TilemapRenderer and before the Player GameObject.
    Unity could not load Player into hierarchy from the scene data.
  - Fixed separators in `Assets/Scenes/_IsoGame.unity`; Player is visible again in Play Mode.
  - Camera was temporarily tested at `orthographicSize=6.5` for wider QC framing; gameplay
    camera was later tightened to `5.35` by the camera/occlusion pass below.
  - Verification screenshots:
    `Assets/Screenshots/debug_game_view_play_after_fix.png`,
    `Assets/Screenshots/debug_game_view_play_camera_6_5_settled.png`.
- [CODEX 2026-05-03] RIMA isometric environment production feedback written from style anchor:
  `GUIDES/RIMA_ISOMETRIC_ENVIRONMENT_PRODUCTION_FEEDBACK_2026-05-03.md`.
  - Reference: `F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`.
  - Verdict: keep orthographic isometric camera/projection; produce modular room pieces
    (64x32 flat floors, 64x96 wall modules, separate props/lights), not whole baked rooms.
  - Foot shadow guidance: use Unity-side actor blob shadows as child SpriteRenderers under
    players/enemies; do not rely on real-time light shadows for grounding.
- [CODEX 2026-05-03] Camera + wall occlusion readability system added:
  `GUIDES/RIMA_CAMERA_AND_WALL_OCCLUSION_SYSTEM_2026-05-03.md`.
  - `Assets/Scripts/Player/CameraFollow.cs` now snaps to Player on start, auto-reads bounds
    from `IsoGrid/Ground`, and clamps camera movement to floor renderer bounds.
  - `Assets/Scripts/Core/WallOcclusionFader.cs` added and attached to `IsoGrid/Walls`;
    nearby wall tile cells fade using runtime `Tilemap.SetColor` so tight rooms stay readable.
  - `_IsoGame.unity`: Main Camera `orthographicSize` set to `5.35`, `useBounds=1`,
    `autoBoundsFromFloorTilemap=1`, `snapToTargetOnStart=1`.
  - Verification screenshot: `Assets/Screenshots/debug_game_view_camera_occlusion_system.png`.
  - Unity console: no compile/game errors after refresh; full test suite not rerun in this pass.
- [CODEX 2026-05-03] Large Hades-like room map system added:
  `GUIDES/RIMA_LARGE_ROOM_MAP_SYSTEM_2026-05-03.md`.
  - `LargeDungeonMapPainter` added under `Assets/Scripts/Core/`; implementation is in
    `LargeDungeonMapPainterBase` inside `RuntimeRoomManager.cs` with a proper file-name wrapper
    so Unity keeps the component binding.
  - `RuntimeRoomManager` now repaints a large layout at room start, updates runtime room size
    from the painter, and moves Player to the first room center before camera startup.
  - Current layout families: GrandArena, LongGallery, Crossroads, TwinChambers, SpiralVault,
    BossHall.
  - Current opening/default map scale: about `156x108` cells; other layouts range up to
    about `190x72` / `178x106` / `164x118`.
  - Main Camera `orthographicSize` tightened to `3.6` so gameplay sees character-local space,
    not the entire room border.
  - `Systems` has exactly one `LargeDungeonMapPainter` component after cleanup.
  - Verification screenshot: `Assets/Screenshots/debug_game_view_large_map_component_fixed.png`.
  - Unity console: no compile/game errors after refresh; full test suite not rerun in this pass.
- [CODEX 2026-05-03] Next-session environment/map handoff written:
  `STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ENV_MAP.md`.
  - Captures current implementation, latest user feedback, remaining visual problems,
    map naturalization direction, lighting needs, and map-tool research targets.
- [CODEX 2026-05-03] Verification:
  - EditMode direction + character select tests: 16/16 PASS.
  - Frame sample: all 8 stop states stable for 4 frames each:
    `S/SE/E/NE/N/NW/W/SW` -> matching `warblade_idle_*` sprite.
- [CODEX 2026-05-03] Latest visual/settings/class/lighting feedback captured:
  `STAGING/VISUAL_SETTINGS_CLASS_LIGHTING_FEEDBACK_2026-05-03.md`.
  - User rejected current settings direction as final UI.
  - User rejected current dungeon lighting as too uniform.
  - `rima_style_anchor` remains the target mood/read; current floor/wall art is placeholder.
  - `PlayerClassManager.SetPrimaryClass()` now applies player class visuals centrally, so ESC
    test class buttons change appearance as well as skill bindings.
  - Added EditMode regression coverage for primary class visual controller swap.
  - Verification: script validation clean for touched C# files; `CharacterSelectTests` 7/7 PASS.
- [CODEX 2026-05-03] Logo/branding direction captured:
  `STAGING/RIMA_LOGO_RIFTMARCH_BRANDING_NOTE_2026-05-03.md`.
  - Main logo read should remain `RIMA` / `RI MA`.
  - Hidden secondary read should imply `Rift March` through broken `RI'ft` and `MA'rch`
    fragments, without sacrificing main-title readability.
- [CODEX 2026-05-03] Hades-like attack-facing fix captured:
  `STAGING/HADES_LIKE_ATTACK_FACING_FIX_2026-05-03.md`.
  - Hades reference checked: Attack at Cursor is separate from movement/dash cursor behavior.
  - `PlayerController` now separates `movementFacingDir` and `combatFacingDir`.
  - `FacingDirection` returns combat-facing while a short override is active.
  - `PlayerAnimator` bypasses movement turn delay during combat-facing override.
  - Follow-up after manual fail: combat-facing override now also forces Animator `Speed=0`
    during the startup window, because current controllers only have directional idle states and
    their direction transitions require `Speed < 0.5`.
  - Follow-up after held-movement return feedback: combat-facing lock shortened to `0.18s`; when
    the override ends, held movement-facing is restored immediately.
  - Added one-time local PlayerPrefs migration to force Hades-like cursor attack default after
    old settings may have left `AttackAimMode` on character-facing.
  - Verification: script validation clean; console had no game compile errors; Unity
    `execute_code` smoke confirmed combat override wins while active and returns to movement
    facing after expiry. Additional Animator smoke confirmed `Speed=0` is required for current
    Warblade directional idle switching. EditMode runner failed to initialize before executing
    tests, including an older test group.

## 2026-05-02 Session (earlier)
- Canvas size research finalized: 252px = 8 frames, pixel budget formula confirmed (w*h*n <= 524,288)
- Interpolation strategy LOCKED: 252px + 8 frames + interpolation-v2 = production workflow
- Idle sprites imported to Unity for 4 wave-1 chars (Warblade, Elementalist, Ranger, Shadowblade):
  - Assets/Sprites/Characters/<Class>/<class>_idle_{south,north,east,west}.png
  - Assets/Animations/Characters/<Class>/<class>_idle_<dir>.anim (4 per class, sprite-linked)
  - Assets/Animations/Characters/<Class>/<Class>.controller (all 4 classes)
  - Assets/Resources/Characters/<Class>/ (controllers + south sprites for runtime load)
- CharacterSelectScreen.cs written (Assets/Scripts/UI/):
  - Auto-creates via [RuntimeInitializeOnLoadMethod] -- no scene setup needed
  - Auto-creates EventSystem if missing
  - 4 class cards, portrait sprites from Resources.Load, OYNA button
  - On select: swaps Animator controller, calls PlayerClassManager.SetPrimaryClass()
- PlayerClassManager: SetPrimaryClass() + OnPrimaryClassSet event added
- Warblade animation pipeline doc: STAGING/WARBLADE_ANIMATION_PIPELINE.md
  - 28 prompts: idle x4, walk x4, 3-hit combo x6, dash x2, death x1, 4 skills x7
  - West = east + Unity flipX (no separate generation)
  - [CODEX 2026-05-03] Superseded by 8-way run-first guide: no walk, no flipX, use `S/SE/E/NE/N/NW/W/SW`.

## Open Issues
- [CODEX 2026-05-03] Unity MCP: connected during 2026-05-03 Codex pass.
- Scene view clutter: SS Overlay canvas appears at pixel coords in scene view -- hide via Layers dropdown (UI layer off)
- [CODEX 2026-05-03] Large map system is technical prototype only: current generated maps are
  too rectangular/artificial and do not yet match `rima_style_anchor.png` composition.
- [CODEX 2026-05-03] Large map paint performance risk: room painting is still synchronous on
  room start; `paintOnAwake` double-paint was disabled, but future work should batch/yield or
  prebuild room templates if hitches remain.
- [CODEX 2026-05-03] Lighting pass not done: needs warm/cold local 2D lights, ambient tuning,
  and room landmark lighting. Superseded by first style-anchor lighting/camera pass above; still
  needs authored props/light-source art, but mood/angle baseline is now closer.
- [CODEX 2026-05-03] Map design tool/workflow research requested but not done yet.
- [CODEX 2026-05-03] Run animations not yet created; current production target is 4 diagonal
  run clips per class: `SE/NE/NW/SW`.
- Compile errors fixed: RIMA.Editor.asmdef + RIMA_EditMode_Tests.asmdef missing references, SceneViewSetup.backgroundColor removed

## Animation Workflow (LOCKED 2026-05-02)
- animate_character MCP = FORBIDDEN (quality issues)
- User animates in PixelLab UI "Animate with Text NEW"
- Canvas: 252px. Frame count: 8. Smooth: interpolation-v2 after.
- Wave 1: Warblade / Elementalist / Ranger / Shadowblade
- Wave 2: Brawler / Ravager / Ronin / Gunslinger / Summoner / Hexer

## Production Gate -- Status

| # | Gate item | Status |
|---|---|---|
| 1 | Canonical doc rewritten per v2 audit | DONE (22ed58c) |
| 2 | Visual Contract template written | DONE (2026-05-01) |
| 3 | Top-4 class contracts (Brawler/Ravager/Ranger/Shadowblade) | DONE -- 47/47 SIGNED-OFF |
| 4 | Unity state overlay spec | DONE (TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| 5 | Brawler char_id idle/walk/dash anchor | DONE (char_id c6ae7f09, 8 rotations) |
| 6 | Core wave-1 contracts (Warblade + Elementalist) | DONE -- 23/23 SIGNED-OFF |
| 7 | Idle sprites in Unity (4 wave-1 chars) | DONE (2026-05-02) |
| 8 | CharacterSelectScreen + class swap | DONE -- MainMenu flow, GraphicRaycaster fix, buttons work |
| 9 | Hades-style player facing | DONE -- [CODEX 2026-05-03] 4 diagonal quadrant facing, deadzone-safe facing persistence |
| 10 | Stone Dungeon floor tile palette | PARTIAL -- 6 selected tiles in palette, but visual QC fails final continuous-floor standard |
| 11 | Stone Dungeon wall test scene | DONE -- 5 wall tiles selected, palette row added, 40x28 room painted |

## Next Priorities
1. **Natural dungeon map design pass** -- replace rectangle-like generated layouts with natural
   authored/procedural room templates matching `rima_style_anchor.png` composition.
2. **Map design tool research** -- evaluate Unity RuleTile/2D Tilemap Extras, Tiled, LDtk,
   Dungeon Architect, DunGen, WFC/Ogmo for RIMA workflow.
3. **Lighting pass** -- add/tune 2D lights for torch/cold magic pools and dungeon ambience.
4. **Run animations** -- [CODEX 2026-05-03] generate Warblade run clips per 4-diagonal Hades-like spec:
   `run_SE`, `run_NE`, `run_NW`, `run_SW`.
5. **Unity run import** -- [CODEX 2026-05-03] import `run_SE/NE/NW/SW` clips and add Animator transitions after PixelLab output exists
6. **Regenerate Stone Dungeon floor** -- flat 64x32 top-surface-only floor tiles; current set reads like overlapping raised slabs.
7. **Tile/object MCP production next batch** -- common scatter props after floor replacement and wall border read correctly.
8. **Final wall art pass later** -- generate dedicated straight/corner/pillar wall pieces; current wall set is a test-room block/parapet border.

## Budget
- PixelLab gen budget: ~2378 remaining estimate after environment asset gens, EXPIRES 2026-05-18 (no rollover)
  - Estimate assumes 36 gen spent after previous ~2414 snapshot: 4 initial floor tests + 16 pro floors + 16 wall candidates.
  - Confirm exact PixelLab balance before any large batch.
- New 5,000 arrives 2026-05-18
- MCP: tiles/objects/icons only. Character animation = UI manual.

## Critical Numbers
- PixelLab gen budget: ~2378/5000 left estimate
- Tests: 24/24 PASS (PlayMode, 2026-04-30)

## Locked Design
See MASTER_KARAR_BELGESI.md (rules #54-#58) and TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md.

## Refs
- Skills: TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md (v2 canonical, commit 22ed58c)
- Audit: TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md (LOCKED)
- Architecture: SYSTEM_MAP.md
- Decisions: MASTER_KARAR_BELGESI.md
- Scene: Assets/Scenes/_IsoGame.unity
- Tile/wall/object guide: GUIDES/TILE_WALL_OBJECT_PRODUCTION_GUIDE.md
- Stone Dungeon floor sprites: Assets/Sprites/Environment/StoneDungeon/Tiles/
- Stone Dungeon wall candidates: Assets/Sprites/Environment/StoneDungeon/Walls/
- Stone Dungeon tile assets: Assets/Tiles/StoneDungeon/
- Stone Dungeon tile palette: Assets/TilePalettes/StoneDungeon_Palette.prefab
- Stone Dungeon wall QC: STAGING/STONE_DUNGEON_WALL_QC_2026-05-03.md
- Isometric environment production feedback: GUIDES/RIMA_ISOMETRIC_ENVIRONMENT_PRODUCTION_FEEDBACK_2026-05-03.md
- Camera + wall occlusion system: GUIDES/RIMA_CAMERA_AND_WALL_OCCLUSION_SYSTEM_2026-05-03.md
- Large room map system: GUIDES/RIMA_LARGE_ROOM_MAP_SYSTEM_2026-05-03.md
- Next session environment/map handoff: STAGING/NEXT_SESSION_HANDOFF_2026-05-03_ENV_MAP.md
- Stone Dungeon floor QC: STAGING/STONE_DUNGEON_FLOOR_QC_2026-05-03.md
- Stone Dungeon visual mockup: STAGING/stone_dungeon_room_mockup.png
- Mossy Crypt floor candidates: Assets/Sprites/Environment/MossyCrypt/Tiles/
- Warblade anim prompts: STAGING/WARBLADE_ANIMATION_PIPELINE.md
- Skill sheet Codex evaluation: STAGING/SKILL_SHEETS_CODEX_EVALUATION.md
- Skill offer system decision: TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md
- Skill offer memory: MEMORY/project_skill_offer_system.md
- Last Epoch design notes: STAGING/LAST_EPOCH_RIMA_TASARIM_NOTLARI.md
- Anchors: Characters/anchors/<class>/rotations/
