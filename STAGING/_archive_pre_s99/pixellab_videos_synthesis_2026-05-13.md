# PixelLab Video Bilgi Sentezi -- RIMA Production Tips
> 15 transcript + 19 gemma analiz + 2 native video analysis, 2026-05-13 sentez

Scope: RIMA S60 pure 2D top-down ARPG. Character target is 64x64 chibi, 4 MVP directions (N/S/E, W=flipX), 10-12 FPS, PPU=64, 32x32 top-down tiles, VFX runtime-side. PixelLab Map Tools remain DISABLED by locked decision #75.

## 1. KAYNAK ENVANTERI

### Transcript set

| ID | Topic | Clean transcript | Qwen summary |
|---|---|---|---|
| THwZYWuOdZI | PixelLab MCP + Claude/Godot full game | yes | yes |
| XdgK1KeN-3s | Animate Pixel Art with Text | yes | yes |
| 1CjxHZoZE_I | Animate Between 2 Frames / Interpolate | yes | yes |
| zghUW8fGqsM | Animate with Text V3 | yes | yes |
| 8TRHAC3fUpo | Walking animations | yes | yes |
| CuBvG9mfQng | Isometric tiles and map creation | yes | yes |
| T4by1uEXuE4 | Isometric animals workflow | yes | yes |
| RISPOYqeEGo | Rotations + animations in one click | yes | yes |
| hOZzbQBjKPc | Animate + rotate + inpaint character | yes | yes |
| nITrIQw1gag | Consistent pixel art for games | yes | yes |
| iBMq3P_Fazk | Style consistent images | yes | yes |
| Hhx9QZwYoZY | Object Creator intro | yes | yes |
| LQS4J4ub8G4 | Punching / attack animations | yes | yes |
| 0SQRclReGo4 | Destructible environments | yes | yes |
| OdRIHQ4ar2c | Pixel art UI creation | yes | yes |

### Gemma / visual set

| Source | Visual analysis | Frame samples |
|---|---:|---:|
| 20250212_Tutorial__How_to_create_tilesets_using_AI | yes | 204 |
| 20250312_Creating_pixel_art_characters_with_rotation_and_animations_in_one_click | yes | 97 |
| 20250515_Tutorial__How_to_create_tilesets_and_maps_using_AI | yes | 151 |
| 20250612_Tutorial__Creating_a_pixel_art_isometric_tiles_and_map_with_PixelLab | yes | 88 |
| 20250620_Tutorial__Animation_to_animation_with_PixelLab | yes | 66 |
| 20250813_Creating_isometric_pixel_art_tiles_with_PixelLab | yes | no yt_frames dir found |
| 20250819_Tutorial__Generating_pixel_art_characters_and_animations | yes | 89 |
| 20250824_Tutorial__Generating_pixel_art_tilesets | yes | 241 |
| 20250919_PixelLab_update__Generate_Tibia_styled_characters_and_animations | yes | no yt_frames dir found |
| 20251119_PixelLab_Map_Workshop_Tutorial__Make_Maps_10x_Faster_with_Al_Tilesets | yes | 72 |
| 20251223_How_to_Animate_Pixel_Art_with_TEXT | yes | 61 |
| 20251229_How_to_Generate_Consistent_Pixel_Art_for_Games | yes | 50 |
| 20251231_This_AI_Tool_Changes_Pixel_Art_Forever | yes | no yt_frames dir found |
| 20260122_Building_a_Cohesive_Game_Art_Style_Using_PixelLab | yes | no yt_frames dir found |
| 20260205_How_To_Create_Isometric_Animals_for_Games_Using_PixelLab | yes | 123 |
| 20260226_How_to_make_bosses_with_PixelLab | yes | no yt_frames dir found |
| 20260309_How_to_Create_Interior_Maps_for_Top-Down_Games | yes | 107 |
| 20260325_New_PixelLab_Tool__Animate_Between_2_Frames | yes | 155 |
| 20260427_Introducing_Object_Creator | yes | 28 |
| youtube_analysis/hOZzbQBjKPc | native frame analysis | 62 |
| youtube_analysis/a_ygRPpBmnI | native frame analysis | 13 |

Note: prompt expected 20 gemma files; repo contains 19 files under `ARCHIVE/RESEARCH_RAW/gemma_analyses`. I included the 2 additional native visual analyses under `ARCHIVE/RESEARCH_RAW/youtube_analysis`.

Frame samples viewed: `youtube_analysis/hOZzbQBjKPc/frames/frame_0007.jpg` (Pixelorama cleanup), `frame_0010.jpg` (Rotate panel), `youtube_analysis/a_ygRPpBmnI/frames/frame_0003.jpg` (PixelLab UI), `frame_0008.jpg` (Create Character 64x64 + Oblique option).

## 2. TOPIC SENTEZ (RIMA-uygulanabilir)

### 2.1 Character Generation (64x64 chibi, sinifa sabit silah)

- Tip (vid_transcript_utf8 / GBA adventure): Clean base before animation. The tutorial creates 8 directions, then fixes stray pixels in Pixelorama before any animation.
  RIMA application: every class/mob/boss base gets `_clean.png` before Custom Animation V3. This supports existing Eraser Pass.
  Locked decision: UYGUN.

- Tip (nITrIQw1gag / consistent art): Teach style through reference images, not repeated style text. 5-10 good references beat a noisy pile.
  RIMA application: build `rima_style_anchor_64.png` from approved 64x64 chibi class/mob sprites. Use it for class/mob batch, not old 252px 2.5D anchors.
  Locked decision: GENISLET. Keeps style-anchor logic, but updates source to S60 64x64 pure 2D.

- Tip (20260226 boss gemma): For bosses, put the player sprite on a larger canvas as scale reference, inpaint the boss area, then iterate.
  RIMA application: boss concept pass should include a 64x64 Warblade/Ranger ghost or guide layer for scale. Do not trust text-only "large boss" prompts.
  Locked decision: UYGUN.

- Tip (a_ygRPpBmnI visual frames): Create Character UI supports 64x64 and high/low top-down plus Oblique beta [frame_0008.jpg ref].
  RIMA application: use 64x64 high top-down for production. Oblique/Tibia is a research branch only; it does not match Hammerwatch/HLD/Hades target.
  Locked decision: UYGUN, with OBLIQUE NOT FOR MVP.

### 2.2 Tile Generation (32x32 top-down floor/wall/decal)

- Tip (THwZYWuOdZI): A full game tutorial uses "top-down 32x32 grass and dirt tile sets" as the practical size.
  RIMA application: confirms S60 32x32 tile target. Use PixelLab for individual tile assets/packs, then import into Unity Tilemap/Room Designer.
  Locked decision: UYGUN.

- Tip (20250824 tileset gemma): Number tile prompts to control variants and keep edge rules explicit.
  RIMA application: every 32x32 floor batch prompt should be numbered: `1). clean stone 2). cracked stone 3). moss edge...`
  Locked decision: GENISLET.

- Tip (CuBvG9mfQng / older isometric tile flow): init/reference shape and guidance weight help keep tile geometry stable.
  RIMA application: apply only to single top-down tile/decal generation. Do not revive isometric wall/floor pipeline.
  Locked decision: UYGUN if top-down; CELISKI if used to restart isometric map workflow.

### 2.3 Animation Pipeline (Animate with Text, Skeleton, Between 2 Frames)

- Tip (8TRHAC3fUpo / walking): Head/body drift is expected; paste the clean head from the static/rotation sheet into animation frames.
  RIMA application: keep Head Swap QC for idle/walk/attack. For 64x64 chibi, head region is small but still the strongest identity anchor.
  Locked decision: UYGUN.

- Tip (vid_transcript_utf8 / GBA adventure): For walk loops, do not seed from a neutral stand if the result is stiff; seed from a mid-stride frame.
  RIMA application: after first walk gen, select the best extreme/mid-stride frame and re-run Custom Animation V3 from it.
  Locked decision: UYGUN.

- Tip (1CjxHZoZE_I / Interpolate): Interpolate New needs first frame + last frame; cleanup after generation is still required.
  RIMA application: use for attack windup/follow and VFX timing where start/end poses are clear. Keep 4/6/8 frame limits.
  Locked decision: UYGUN.

- Tip (20250620 animation-to-animation gemma): Motion transfer is valuable, but current canonical guide says Animation-to-Animation is limited to 128/64/32/16 and not for 252px.
  RIMA application: now that S60 uses 64x64, re-test Animation-to-Animation for mobs and simple class walk/attack. Do not replace Custom Animation V3 until QC passes.
  Locked decision: GENISLET.

- Tip (hOZzbQBjKPc visual analysis): Skeleton/inpaint workflow can freeze one frame and generate two, but needs manual skeleton point edits and cleanup [frame_0010.jpg ref].
  RIMA application: reserve skeleton workflow for complex mobs/bosses only. For 16-sprite batch, direct Character Creator + Custom Animation V3 is faster.
  Locked decision: UYGUN.

### 2.4 Map / Tileset Workshop

Map Workshop remains locked out by decision #75.

- Tip (20251119 Map Workshop gemma): Map Workshop is fast for previewing terrain transitions and style coherence.
  RIMA application: use as visual reference only, never as production map source. Recreate useful tiles in Unity Room Designer pipeline.
  Locked decision: UYGUN.

- Tip (vid_transcript_utf8 / map section): The creator improves water/grass by editing the tileset with prompts like muted/no gradients/texture.
  RIMA application: extract prompt lessons: muted palette, no gradients, texture pass. Do not use the workshop map output.
  Locked decision: UYGUN.

- Tip (20260309 interior maps gemma): Interior map generation can inpaint walls/doors into a blocked layout.
  RIMA application: use for concepting room mood and prop language only. RIMA rooms are authored by tile grammar, not baked AI maps.
  Locked decision: UYGUN.

### 2.5 Style Consistency Methods

- Tip (20260122 cohesive style gemma): Start from one strong visual anchor and generate every asset against it.
  RIMA application: make a new S60 anchor sheet from approved 64x64 class/mob sprites, 32x32 tiles, and 64-128 VFX. Keep old 2.5D anchors out of S60.
  Locked decision: GENISLET.

- Tip (iBMq3P_Fazk / style image): The model follows colors found in the style image; init strength/style guidance change similarity.
  RIMA application: use palette-locked references for class families: cold blue classes, shadow violet, elemental neutral. Do not rely on prose palette alone.
  Locked decision: UYGUN.

- Tip (nITrIQw1gag): Smaller canvas allows more style examples/outputs; at 64x64, style-reference workflows give 16 frames/options.
  RIMA application: 64x64 chibi is the right production size for batch variety. 128/256 should be boss/VFX only.
  Locked decision: UYGUN.

### 2.6 New Tools / Recent Features

- Object Creator (Hhx9QZwYoZY / 20260427): Generates object packs, supports style references, top-down/sidescroller perspective, 8 directions, create-state.
  RIMA use: YES for props, interactables, destructible states, chest open/closed, torch lit/unlit. Use 1 direction for floor props unless rotation matters.
  Risk: Do not use it for class characters unless Character Creator flow is worse.

- Animate Between 2 Frames (1CjxHZoZE_I / 20260325): First/last frame interpolation.
  RIMA use: YES for attacks, chest open, portal activate, boss phase transitions, VFX anticipation/release.
  Risk: It creates in-betweens, not design. Bad endpoints produce bad animation.

- Animate with Skeleton (8TRHAC3fUpo, hOZzbQBjKPc): Can guide motion and transfer templates.
  RIMA use: MAYBE for mobs/bosses and later polish. For current 16-sprite batch, keep lower complexity.
  Risk: color drift, skeleton misfit, manual cleanup.

- Cohesive Game Art Style (20260122): locked reference image as visual anchor.
  RIMA use: YES, high priority. Rebuild anchors for pure 2D.

- Boss generation (20260226): scale reference + inpaint + animate with text + style reference.
  RIMA use: YES for 4 boss + final boss concept pass.
  Risk: Do not crop boss down to 64x64. Boss final import can be larger, but must stay top-down and readable.

### 2.7 Workflow Tricks / Non-Obvious Findings

- Manual cleanup is not optional. Multiple videos repeat that AI output is a start, not final production art.
- For symmetrical characters, generate E and mirror W. For asymmetric weapons/arms, generate W separately.
- Style Reference Pro / style image copies scale and padding. In S60, do not feed mixed 252px 2.5D anchors into 64x64 chibi runs.
- Use final frame as next reference to extend long animation, but prune weak frames afterward.
- For Object Creator packs, describe each item separately when variety matters.
- UI assets should use concept image + palette, then reuse earlier UI pieces as concept refs.
- Destructible props work best as state assets first, animation second. Make intact/damaged/broken frames, then interpolate or animate.

### 2.8 Confirmed Limitations (NE YAPILMAMALI)

- Do not ship raw generations. Clean stray pixels, double outlines, color drift, and head/face inconsistency.
- Do not use Map Workshop output as RIMA production maps. Decision #75 stays active.
- Do not seed walk cycles only from neutral idle if motion feels robotic.
- Do not flip asymmetric characters/weapons without checking hand/weapon identity.
- Do not ask one prompt to generate, edit, animate, and style-lock at the same time. Choose the tool per step.
- Do not overfeed random style refs. 5-10 coherent references are better than a large noisy set.
- Do not embed gameplay VFX in core character frames unless it is a deliberate placeholder for runtime overlay.

## 3. CELISKILER + GUNCELLEME ONERILERI

| Mevcut karar | Yeni bulgu | Onerilen aksiyon |
|---|---|---|
| `MEMORY/pixellab_master_pipeline.md` says 8 directions mandatory for every character/mob/boss | S60 prompt says MVP 4 directions, N/S/E with W=flipX | CELISKI. Update master pipeline for S60: production MVP 4 directions; optional 8 directions only for future polish or asymmetric assets. |
| Master pipeline still has 252x252 / 256x256 character assumptions in several sections | S60 locked context is 64x64 chibi Create Character | CELISKI. Split old 2.5D/large-canvas rules from S60 pure 2D rules. Keep pixel budget, cleanup, negative prompts; replace default canvas. |
| Master pipeline says Map tools not used | Videos show useful Map Workshop iteration | No change. Keep locked ban; add "visual learning only" note. |
| Animation-to-Animation was marked unusable for 252px | S60 returns to 64x64 where tool is applicable | GENISLET. Pilot one mob walk transfer, QC before adoption. |
| Old style anchor may be 252px/padded | Videos show style refs copy scale/colors | CELISKI if reused. Create `rima_s60_style_anchor_64.png` from approved 64x64 outputs. |

## 4. PRIORITE GUNCELLE TABLOSU

1. Rebuild S60 PixelLab pipeline doc around 64x64 chibi and 32x32 tiles; demote 252px rules to archive/large-boss appendix.
2. Create a 64x64 style anchor set before the 16 sprite batch.
3. For 16 sprite batch, use Character Creator / Create Character at 64x64 high top-down, 4 directions where allowed.
4. Run Eraser Pass before every animation; Head Swap QC after idle/walk/attack.
5. Use mid-stride seed recovery for all walk loops that look robotic.
6. Test Animation-to-Animation on one mob because S60 size makes it viable again.
7. Use Object Creator for props/destructible states, not for rooms or production maps.
8. Use Interpolate New for attack endpoints and prop state transitions.
9. Boss workflow: player scale reference + inpaint/concept + style reference + manual cleanup.
10. Keep Map Workshop out of production; harvest only palette/texture wording.

## 5. REFERANS BAGLANTILARI

- Cleanup before animation: `vid_transcript_utf8.txt:27` says to clean the sprite and fix stray pixels before continuing.
- Head swap: `vid_transcript_utf8.txt:48` and `vid_transcript_utf8.txt:64` describe copying the static face/head into animation frames.
- Mirror economy: `vid_transcript_utf8.txt:59-60` says cleaned east/southeast can be mirrored for west/southwest.
- Mid-stride seed: `vid_transcript_utf8.txt:157-168` says not to use a standing frame for natural walking; use a middle stride frame.
- Canvas scale trap: `vid_transcript_utf8.txt:204` notes reducing canvas to 32x32 fixed character scale.
- Style refs: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/nITrIQw1gag.en_clean.txt:10-11` says to teach a visual style with reference images.
- Good refs count: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/nITrIQw1gag.en_clean.txt:27` says 5 to 10 good images are enough.
- Object Creator: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/Hhx9QZwYoZY.en_clean.txt:4-7` covers asset packs, size slider, and style refs.
- Object directions/states: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/Hhx9QZwYoZY.en_clean.txt:25` and `:32` cover 8 directions and create states.
- Interpolate: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/1CjxHZoZE_I.en_clean.txt:34` says the tool uses first and last frames.
- Interpolate cleanup: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/1CjxHZoZE_I.en_clean.txt:141` says to clean sprites after generation.
- 32x32 tile confirmation: `ARCHIVE/RESEARCH_RAW/youtube_transcripts/THwZYWuOdZI.en_clean.txt:418` references top-down 32x32 grass/dirt tilesets.

## STATUS

STATUS: DONE
FILES_READ: 15 transcript clean files + 19 gemma files + QWEN_ANALYSIS.json + 2 native youtube_analysis markdown files + 4 frame samples viewed
FILES_WRITTEN: STAGING/pixellab_videos_synthesis_2026-05-13.md
NEXT_SIGNAL: "pixellab_video_synthesis_done"
