# Research: Sang Hendrix + "Realtime Parallax Map Builder" — relevance to RIMA's in-game map/room builder
Date: 2026-06-13 | web-research agent | (agent had no Write tool; orchestrator saved this)

## 0. Source-access caveats
- X post `https://x.com/sanghendrix96/status/2065718350479823312` **inaccessible**: direct 402 (login wall), oEmbed 402, jina 451, not indexed. Snowflake ID dates it ~mid-2026 (very recent). Content NOT verified — not guessed.
- YouTube descriptions not extractable; video-only mechanics reconstructed from itch text + community page (flagged).

## 1. Who is Sang Hendrix
- Vietnamese solo game designer / pixel artist / prolific RPG Maker MZ **plugin dev**; brand = make games "look like it wasn't made in RPG Maker." [https://sanghendrix.itch.io/] [https://itch.io/profile/sanghendrix]
- Flagship game: **Into Samomor** — psychological horror action RPG "inspired by Zelda and Soulslike," branching story, combat demo. Free on Google Play + itch. [https://sanghendrix.itch.io/intosamomor]
- Commercial plugins (recurring signature = realtime WYSIWYG visual editors on top of RPG Maker): RPG Maker Action Combat ($39.99), Pictures UI Creator, Hendrix HUD Designer, Post-Processing (realtime WYSIWYG), Particles Builder, Non-Destructive Localization, 27+ bundle. [https://itch.io/c/3965240/hendrix-rpg-maker-plugins]
- **Moved Godot→Unity (Sept 2024)** — recent work Unity-aware, but Parallax Map Builder stays RPG Maker MZ-only (no Unity port found). [https://x.com/sanghendrix96/status/1840058067280638156]

## 2. Plugin: "Hendrix Realtime Parallax Map Builder" (RPG Maker MZ)
Tagline: "Create parallax maps and paint tilemap grid-free in real time using a visual editor (WYSIWYG) and drag-n-drop." Price **$17.99+**, v1.2.1. [https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin]
- **Two modes**: parallax-image mapping + grid-free tilemap painting (v1.2.0).
- **Photoshop-like layer panel**: layer→import→instant; lock, duplicate, drag-reorder, dbl-click rename, scale (drag/type), per-layer chip count. [v1.1.6 overhaul]
- **Parallax depth**: per-layer position, z-index, blend mode, looping/tiling, snapping.
- **Occlusion auto-fade**: parallax image auto-fades when player walks underneath; adjustable opacity; **Region-ID-gated**. [price-change devlog]
- **Animated parallax** elements; **drag-n-drop import**; **keyboard camera pan**; **snap-to-grid**.
- **Editor invoked from an in-game on-screen button** — editor lives INSIDE the running game. [v1.1.4]
- **Non-destructive runtime layer toggle** (show/hide w/o writing save; resets on reload). [v1.1.3]
- Reads parallax PNGs from `Parallaxes` folder + subfolders (asset-folder-driven catalog). [v1.1.6]

## 3. User reception
- Praise: "makes parallax mapping intuitive and simple"; VisuStella Bright Effects compatible. [community]
- Complaints (real user reports): **large-image FPS cliff (20 FPS even on dGPU)**; **coarse collision** (hard to place precise collision points); requests for in-editor image resize + >3-frame anim + reflecting edits in base editor. [https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin/community]

## 4. ACTIONABLE for RIMA
(a) Model: each backdrop element = layer {image, pos, z-index, scroll-factor, blend, loop, scale, opacity}; parallax = scroll-factor + z-order; live mutation = WYSIWYG. Matches RIMA painted-backdrop parallax.
- **Occlusion fade** (fade when player behind/under, Region-ID-gated) = highest-value borrow; RIMA iso Y-sort can trigger same auto-fade on overhead/foreground props — no per-prop scripting.
(b) UX to borrow: (1) editor launched from IN-GAME button — validates RIMA F2 in-play editor; (2) Photoshop layer panel (drag-reorder/rename/lock/dup/opacity/scale); (3) asset-folder-driven catalog (matches RIMA Asset Catalog); (4) grid-free hold-LMB brush + grid toggle; (5) non-destructive runtime layer toggle; (6) snap-to-grid + kb camera pan.
(c) Pitfalls to pre-empt: large-image perf cliff → keep RIMA downscale→PPU32 + chunk/atlas; collision must be a **first-class separate brush** never inferred from art; **single source of truth** between in-play editor + runtime to avoid WYSIWYG drift.

## 5. Recommendations
1. ADD occlusion auto-fade for foreground/overhead props via iso depth/Region trigger (reuse Y-sort).
2. ADOPT layer-panel interaction set for RIMA Asset Browser.
3. KEEP collision/walkability a separate explicit brush, never inferred from art.
4. ENFORCE layer image-size budgets (chunk/atlas/downscale).
5. GUARANTEE single-source-of-truth in-play editor ↔ runtime (directly relevant to RIMA ④ refresh/sync work).

## 6. Open questions
- X post 2065718350479823312 content — blocked; needs logged-in X or user paste.
- No Unity counterpart of his parallax editor found (RPG Maker MZ only).
- In-editor scroll-factor/occlusion-fade math is video-only, not text-verifiable.
