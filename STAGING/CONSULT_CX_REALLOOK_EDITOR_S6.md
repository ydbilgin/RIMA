# CX (Codex) CONSULT — technical path to "real game look" + REAL map-editor architecture

ACTIVE RULES: (1) think (2) concrete & minimal (3) flag what to reuse vs build (4) BLOCKED if unclear.
Senior Unity 2D engineer. Two questions. Concise, ranked, implementable markdown. No code files — this is a PLAN; Opus implements.
READ to ground yourself:
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (F2 in-Play overlay: drag-place, Prop mode, thumbnail palette, pause)
- `Assets/Scripts/DevTools/WallRunBuilder.cs` + `RuinedKeepComposer.cs`
- `Assets/Scripts/RoomPainter/` (RoomData.cs, RoomLayer.cs, RoomPainterAsset.cs, RoomLayerData.cs) + `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`
- `Assets/Scripts/LiveTool/` (T3 Live Editor scaffold) + `Assets/Scripts/Live/RuntimeAssetRegistry.cs`
- `STAGING/livetool_t3/00_T3_STATUS.md` if present

## CONTEXT
RIMA Unity URP 2D, PPU64, top-down 3/4. Current room = imagegen PLACEHOLDER walls + basic 2D lights + vignette; PixelLab is the intended final-art pipeline. The project already has: a T3 Live Editor scaffold (Assets/Scripts/LiveTool + ToolMain scene), a RoomPainter Editor window (RimaRoomPainterWindow) with RoomData/RoomPainterAsset assets, RuntimeAssetRegistry (baked asset lookup), and the new in-Play F2 overlay.

## Q1 — Technical path to a "real game look" (honest, ranked)
Given the stack, the realistic ENGINEERING path from "placeholder blockout" to "real game look". Rank by ROI:
- PixelLab finals: detailed lit masonry walls/props at correct (un-stretched) sizes — is this the gate, vs polishing imagegen?
- URP 2D lighting quality: normal maps on sprites (Sprite Lit + secondary normal tex) for real surface shading; 2D shadow casters; light falloff/intensity curves; blend styles; HDR + bloom + color grading via Volume.
- Floor: textured tile variation + decals + AO/contact-shadow blobs.
- Particles (dust/embers/fog) + parallax void depth.
What's worth doing in-engine now vs blocked on PixelLab art? Give the ordered, concrete list.

## Q2 — REAL map-editor tool architecture (the user's core ask)
User wants: edit-mode opens a REAL tool that LISTS their maps, shows top-down view, and place→creates/saves the map. NOT just the IMGUI overlay.
Design the architecture, maximizing REUSE of what exists:
- **Map list / browse + new/save/load:** what persistence to use — `RoomPainterAsset`/`RoomData` ScriptableObjects (scan a folder, list with thumbnails), or scene-based rooms? How to list, create new, duplicate, delete. Where saved (`Assets/Data/Rooms/`?).
- **Top-down place loop:** reuse the F2 overlay's drag-place + `WallRunBuilder` + `RuinedKeepComposer`, or the Editor `RimaRoomPainterWindow`? Recommend: extend the RoomPainter Editor window (full dockable tool, map-list panel + top-down SceneView/preview + palette) OR upgrade the in-Play overlay into a fuller tool. Tradeoffs (Editor window = real tool, but not "in-play"; in-Play overlay = live but limited).
- **Place → build → save:** placing writes to the RoomData/segments, building instantiates (RuinedKeepComposer/WallRunBuilder), saving persists the asset; loading a map re-composes it.
- Concrete component/window list: what to reuse, what to add, the data flow (palette → place → RoomData → save asset → Composer loads). Note the existing RuinedKeepComposer `_segments` + WallRunBuilder.BuildRun as the build core.

Rank the build steps. Recommend whether the "real tool" = enhanced RoomPainter Editor window (top-down, map list, save) or enhanced in-Play overlay. Opus decides + implements.
