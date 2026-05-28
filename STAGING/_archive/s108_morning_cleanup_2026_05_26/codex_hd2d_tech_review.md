# Codex HD-2D Tech Review

## Verdict
Choice: RESEARCH_MORE
Confidence: med

Top 3 wins: 3D walls/floors solve modular room silhouette limits; real lights, depth, fog, bloom, and camera composition can approach HD-2D; existing 64px chibi sprites and PixelLab decor can remain usable.

Top 3 risks: current stack is 2D-oriented, so real HD-2D likely needs Universal/Forward Renderer; sprite-vs-wall depth sorting is not solved by Sorting Order alone; Octopath-style sprite light/shadow integration needs shader/render-layer work.

## Tech analysis
1. Camera setup: use orthographic, not perspective, for tactical readability and pixel scale. Treat the "85-90 degree" lock as near top-down with tuned iso yaw/tilt, not a literal flat 90 degree view. Pixel Perfect Camera can help sprite crispness, but it will not make angled 3D textures pixel-perfect by itself. Use a base world camera plus UI overlay camera/canvas. Current assets include URP 17.3, Pixel Perfect 5.1.1, and `Assets/Settings/Renderer2D.asset`; for 3D shadows/lights, make a separate Universal Renderer proof path.

2. Sprite-on-mesh characters: SpriteRenderer Sorting Order works inside sorting-layer rules; it does not naturally occlude against 3D mesh depth. Characters should be vertical quads/billboards in 3D space with foot pivot, fixed camera-facing rotation, tested transparent depth behavior, and optional SortingGroup. Y-sorting can continue for sprites/decor, but wall occlusion needs depth, render queues, or explicit occluder planes. 3D point lights will not light URP 2D sprites like meshes; expect a custom/fake-lit sprite shader. Shadows need alpha-clipped shadow casting, blob/projector shadows, or a custom shadow pass.

3. 3D wall mesh + 2D texture pipeline: ProBuilder is best for the first proof; Blender/FBX is better after the wall kit stabilizes. Import textures with Point filtering, compression off/high quality, and mipmaps disabled or controlled. Angled walls expose texel-density problems, so UVs need grid consistency and shared pixel-per-world-unit targets. PixelLab Wang/tileable assets remain useful as source textures, but meshes want trim sheets plus separate decals for cracks, banners, torches, and arches.

4. Lighting system: Light2D and ShadowCaster2D are optimized for 2D Renderer sorting layers, not true 3D mesh lighting. For HD-2D, test Universal Renderer with 3D Point/Spot/Directional lights, no baked GI at first, and one torch point light. Real-time lights are fine for a small room; baked lighting can come later. The hard part is making sprites look affected by the same light without losing pixel identity.

5. Solo-dev complexity: environment helper code is moderate, roughly 300-800 LOC, but shader/render setup can grow fast. ProBuilder is faster to learn; Blender gives better long-term modeling/UV control. Minimum proof assets: one floor material, one wall trim, one corner/cap, one arch, one torch, one character sprite.

6. Existing scaffold integration: RoomTemplate still applies as encounter metadata, but `baseImage` becomes optional or a prefab/scene reference. OverlayAnchor and RoomDecorationSpawner remain useful, but `localPos` should become Vector3 or a struct with position, rotation, surface type, and normal. Current Vector2 wallPath/door/enemy points are not enough for true 3D room shells.

7. Performance and polish: 20-80 low-poly meshes plus room props is safe on PC if materials are batched. Draw calls are a material-count issue more than polygon count. Z-fighting will appear at wall joins unless modules overlap deliberately or use beveled/capped seams. Pixel crispness is threatened by mipmaps, rotated UVs, camera sub-pixel movement, post-AA, and bloom/DOF overuse. HD-2D references show the style is expensive because lighting, palette, shadows, and post must be tuned together.

## Implementation outline (5-7 Unity steps)
1. Create `Assets/Scenes/Demo/HD2D_Proof.unity` using a Universal Renderer asset separate from `Assets/Settings/Renderer2D.asset`.
2. Build one floor plane and one modular wall segment in ProBuilder; assign one PixelLab tileable material with Point filtering.
3. Add an orthographic main camera with Pixel Perfect Camera, fixed iso/top-down rotation, and Y-axis transparency sort validated.
4. Place the existing warblade/chibi character as a camera-facing SpriteRenderer or quad at foot-pivot world origin.
5. Add one torch Point Light and compare unlit sprite, Sprite-Lit material, and custom/fake-lit sprite material.
6. Add a wall occluder test: walk sprite behind/in front of a wall and verify depth/sorting without per-object order hacks.
7. Capture screenshots against `chatgpt_ref` and decide only after visual QC: believable HD-2D, readable combat, no mushy pixels.

## Code-level risks
Renderer migration may invalidate Light2D assumptions in existing scenes. Sorting bugs can become systemic if Vector2 anchors and Sorting Order are stretched into 3D responsibilities. Sprite lighting/shadow code may require custom shader maintenance across URP versions. Texture import mistakes can silently destroy the pixel-art look. Next move: one-day proof slice, not full rewrite.
