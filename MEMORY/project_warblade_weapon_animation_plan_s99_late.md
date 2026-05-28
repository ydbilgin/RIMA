# Warblade Weapon + Animation Plan — S99 LATE evening LOCK

## Context
S99 LATE evening — wall pipeline Discord cevabı bekliyor. Paralel iş olarak karakter weapon + animation production'a geçiş. Vertical slice için Warblade focus, sonra diğer 9 character.

## Locked sequence

### Step 1 — Weapon sprite production (USER, web UI)
**Tool:** PixelLab S-XL new (web UI manual)
**Source:** `STAGING/weapon_web_prompts_v1.md` (11 prompt ready)
**Settings per weapon:**
- Canvas: 32×32
- Background: Transparent
- Direction: None
- Style anchor: Warblade canonical anchor (`warblade_south.png` or similar)
- ~1 gen each, total ~11 gen

**11 weapon sprites:**
1. Warblade greatsword
2. Ronin katana drawn
3. Gunslinger pistol L + R (mirror)
4. Ranger bow + arrow
5. Elementalist orb (Karar #146)
6. Shadowblade dagger L + R
7. Ravager axe
8. Hexer scepter
9. Brawler knuckle L + R
10. Summoner staff

**Save path:** `Assets/Art/Weapons/<Class>/<weapon_filename>.png`

### Step 2 — Warblade prefab scaffolding (CODEX, paralel)
Dispatch while user produces weapons:
- **Edit Warblade prefab** (`Assets/Prefabs/Characters/Warblade.prefab`):
  - HandAnchor child GameObject exists (Phase E aec965a8) — verify position
  - Add `WeaponHolder` child GameObject under HandAnchor
  - WeaponHolder has SpriteRenderer (sortPoint=Pivot, sortingLayer="Entities", sortingOrder=1 above body)
  - Initial weapon = Warblade greatsword (referans after Step 1)
- **WeaponDefinition SO scaffold** (`Assets/Data/Weapons/WeaponDefinition.cs` if not exists):
  - Fields: id, displayName, sprite (Sprite ref), pivotOffset, attackVFX (optional), audioCue (optional)
- **WeaponEquip script** (`Assets/Scripts/Combat/WeaponEquip.cs`):
  - Equip(WeaponDefinition) → set WeaponHolder.SpriteRenderer.sprite
  - Per-class default weapon mapping
- Codex commit + verdict

### Step 3 — Warblade animation production (CODEX, after weapons ready)
**Tool:** PixelLab `animate_with_text_pro` (Pro tier, 20-40 gen/anim) OR `create_animated_pro`
**Reference:** `MEMORY/pixellab_animation_techniques.md` + `MEMORY/feedback_animate_character.md`

**Animations needed (Warblade only, vertical slice):**
1. **Walk cycle** — 8 yön (S, SE, E, NE, N + mirror W/SW/NW = 5 native + 3 flipX)
2. **Attack** — 8 yön (greatsword swing, weapon-aware)

**Per-animation:**
- ~6-8 frame cycle
- Existing idle pose as init frame
- Same view as idle (top-down chibi 64×64)
- HandAnchor position consistent (weapon stays attached)

**Cost estimate:** 2 animations × 5 native × ~20-40 gen = ~200-400 gen (Pro tier)
**Alternative:** `animate_with_text` (Included tier, cheaper) — try first

**Hurt + Death deferred** (S100, after walk+attack working).

### Step 4 — Unity integration validation
- Scene: `Assets/Scenes/Demo/TopDownTest_Map1.unity` veya yeni `WarbladeWeaponTest.unity`
- Play mode: WASD walk + LMB attack test
- Verify:
  - Weapon visible attached at HandAnchor
  - Animation plays per direction
  - Weapon follows hand during animation (no detach)
  - 8-direction blend tree firing correctly
- Screenshot → verdict

### Step 5 — Expand to other characters (S100+, defer)
9 character × same pipeline:
- Ronin katana drawn + sheathed pair
- Gunslinger dual pistol
- Ranger bow + arrow nock
- Elementalist orb float
- Shadowblade dual dagger
- Ravager axe heavy
- Hexer scepter
- Brawler knuckle dual
- Summoner staff

**Per character:** ~2-3 weapons + 2-4 animations = significant scope. **Defer to S100+.**

## Key locks (do not change)
- **HandAnchor pattern locked** (Karar #144)
- **Body weaponless sprite** (Karar #123) — body sprite has no weapon baked
- **Per-class default weapon** mapped via WeaponDefinition SO
- **8-direction system locked** (Karar #114) — 5 native + 3 flipX
- **PixelLab state pipeline YASAK** (`feedback_state_vs_n_frames_cost_lock`) — use animate_with_text_pro, NOT create_object_state
- **No autonomous PixelLab night** (`feedback_no_pixellab_night_autonomous`) — user awake required

## Discord wall pipeline pending
Wall pipeline iteration paused waiting Discord community answer. Next session pickup:
- Check Discord help-questions response
- IF Pro tool style image solves dimension lock → resume wall production
- IF community suggests new tool/workflow → adapt plan
- IF no answer → continue wall iteration with per-direction prompt-only style approach (init drop)

Wall pipeline state: see [[project_wall_production_pipeline_s99_late]]

## Related
- [[project_weapon_pipeline_lock]] — Karar #123/#144/#146 canonical lock
- [[project_canonical_character_roster_v2]] — 10 character list
- [[feedback_animate_character]] — animate_character usage notes
- [[pixellab_animation_techniques]] — run cycle, interpolation
- [[PIXELLAB_TOOL_GUIDE]] — tool capability reference
- [[feedback_state_vs_n_frames_cost_lock]] — state pipeline ban
- [[feedback_no_pixellab_night_autonomous]] — gen timing rule
- [[project_wall_production_pipeline_s99_late]] — concurrent paused work
