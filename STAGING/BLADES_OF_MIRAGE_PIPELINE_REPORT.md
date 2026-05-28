# Blades of Mirage Pipeline Report

Date: 2026-05-28
Scope: RIMA reference research + LaurethStudio transfer note
Reviewed by: Codex web pass + Antigravity second-eye pass

## Short Verdict

Blades of Mirage should be treated as a real-time 3D isometric action RPG, not a pure 2D sprite game.

The strongest public evidence is:
- Steam positions it as an "isometric action RPG" and its user tags include `3D`, `3D Platformer`, `Stylized`, `Isometric`, `Action RPG`.
- The Steam community/dev blog explains that the team chose an isometric POV for combat readability, dodging, enemy pressure, environment navigation, and puzzle clues.
- Agate's own announcement describes co-development with Red Dunes Games, stylized Southeast Asian landscapes, fast-paced combat, weapon/style switching, and physics-based environmental puzzles.
- Minimum GPU requirement is GTX 1050-class hardware, which fits a 3D realtime render target more than a simple 2D sprite target.

Important caveat: Antigravity's second-eye analysis claimed Unreal Engine / GAS / AnimBlueprint-style implementation. I did not find a public official source confirming that engine/toolchain detail. Treat that as plausible technical inference, not confirmed fact.

## 3D vs 2D

Decision: 3D, with 2D support art.

Likely production shape:
- 3D characters with skeletal animation.
- 3D environments viewed through a fixed/isometric camera.
- Realtime stylized lighting, shadows, water/VFX, and camera-space combat readability.
- 2D work still exists for concepts, UI, icons, promo art, decals, texture masks, and VFX flipbooks.

Not likely:
- Pure 2D top-down sprite/tile pipeline.
- Static AI-image backgrounds only.
- Frame-by-frame 2D character animation as the main production method.

## Blender / 3D Skill Requirement

For a game at this visual/combat target, yes: a team needs Blender, Maya, Max, or equivalent 3D production skill.

Required competencies:
- Character modeling, retopology, UVs, rigging, and clean animation export.
- Modular environment modeling for temples, ruins, foliage, props, cliffs, gates, and set dressing.
- Stylized material/texturing workflow, likely hand-painted or Substance-style.
- Tech art for water weapons, hit trails, impact VFX, shader polish, and readable enemy telegraphs.
- Engine-side camera, collision, nav, lighting, LOD, and performance tuning.

But "cok iyi Blender bilmeden hic olmaz" is too broad. A small prototype can survive with:
- Low-poly primitives and modular kits.
- Asset-store/kitbash base models.
- Mixamo/simple rig pipelines for early characters.
- AI-generated texture/reference sheets.
- One strong 3D generalist doing cleanup and integration.

For a premium trailer-quality result, AI images alone will not replace a 3D artist/tech artist.

## AI Image Pipeline Feasibility

AI image tools can support the pipeline, but they should not be the core realtime asset source if the goal is Blades-level 3D combat.

Strong use cases:
- Concept art for biomes, ruins, characters, bosses, weapon forms.
- Mood boards and style targets.
- Texture ideas, trim-sheet motifs, decals, masks, icon sets.
- VFX flipbook references and impact shape exploration.
- Pre-rendered 2.5D props or marketing art.
- Fast iteration on cultural visual language before modeling.

Weak use cases:
- Consistent 360-degree character rotations.
- Clean topology, UVs, rigging, and animation-ready meshes.
- Collision-aware level geometry.
- Realtime lighting consistency across moving characters and VFX.
- High-FPS combat animation with stable silhouettes.
- Water-form weapon animation without temporal artifacts.

Practical simplified route:
1. AI image generates concept and style sheets.
2. Human/kitbash converts only the selected assets into simple 3D modules.
3. AI assists textures, decals, icons, and VFX sheets.
4. Engine shader/VFX pass gives the simple geometry a premium stylized look.
5. Keep gameplay scope small enough that animation count stays controllable.

## RIMA Applicability

Use Blades of Mirage as a readability/style reference, not as a mandate to convert RIMA into full 3D.

Useful for RIMA:
- Isometric combat readability: wide camera, clear silhouettes, readable attack arcs.
- Water/element weapon idea: one core element can produce many weapon forms without needing many unrelated weapons.
- Biome identity: each island/zone can have a distinct material palette and prop language.
- Boss arenas as puzzle-combat spaces, not only DPS checks.
- AI-assisted asset direction: concept, prop sheets, VFX sprites, decals, tile motifs.

Avoid for RIMA right now:
- Full 3D character/environment pipeline pivot.
- Heavy Blender dependency inside the current 2D/2.5D production flow.
- Overbuilding physics puzzles before the core roguelite combat loop is stable.

RIMA recommendation:
- Stay 2D/2.5D.
- Borrow isometric composition, silhouette discipline, biome palette logic, water/VFX readability, and modular arena dressing.
- Use AI images to generate prop/terrain/VFX references, then convert into controlled sprites or simple scene modules.

## LaurethStudio Applicability

For LaurethStudio, Blades of Mirage is useful as a studio-level pattern:
- "Regional myth + stylized action" can differentiate a small game better than generic western fantasy.
- One flexible element mechanic can replace a huge weapon roster.
- 2.5D or low-poly 3D with strong shader/VFX art can give premium feel without AAA asset volume.
- AI can reduce concept and texture iteration cost, but not eliminate the need for a 3D integration pass.

Recommended LaurethStudio angle:
- Prototype a small, scope-controlled "single element, many forms" action idea.
- Use low-poly/modular 3D only if the design truly benefits from realtime camera/lighting.
- If the goal is fast solo/small-team production, prefer 2D/2.5D AI-assisted assets over full 3D.

## Antigravity Second-Eye Summary

Antigravity agreed with the 3D interpretation and emphasized:
- likely realtime 3D skeletal animation;
- significant need for rigging, UV, texture, VFX, and shader skill;
- AI images are useful for concept/texture/pre-rendered 2.5D, but weak for temporally consistent combat animation and realtime interaction;
- RIMA can use modular isometric structure and shader/VFX lessons;
- LaurethStudio can use regional cultural identity and a single flexible element mechanic.

Unverified from public sources:
- specific Unreal Engine / GAS / AnimBlueprint implementation details.

## Sources

- Steam store: https://store.steampowered.com/app/3227500/Blades_of_Mirage/
- Steam community/dev blog page: https://steamcommunity.com/app/3227500
- Agate announcement: https://agate.id/news/agate-announces-global-partnership-with-red-dunes-games-for-the-co-development-of-blades-of-mirage-a-southeast-asia-inspired-action-rpg/
- Cubed3 TGS preview: https://www.cubed3.com/features/previews/blades-of-mirage-tokyo-game-show-2025
- Local Antigravity output: `AGY_DONE_laurethgame.md`
