# RIMA Realtime Room Painter — ChatGPT Spec (verbatim)

**Date:** 2026-05-26 (S109)
**Source:** ChatGPT verdict (user paylaştı, Hendrix Realtime Parallax Map Builder esinli)
**Status:** Spec kayıt — Codex tamamlanınca üçlü AI sentezi yapılacak (agy + Codex + ChatGPT)

---

## Inspiration & Boundary

We are building a Unity Editor tool inspired by Hendrix Realtime Parallax Map Builder for RPG Maker MZ, but for our own 2D pixel-art roguelite project RIMA.

**Important:**
Do not copy or decompile the original plugin. Use only the high-level workflow idea:
- realtime visual room editing
- grid-free sprite/tile painting
- layer management
- drag-and-drop placement
- collision painting
- occlusion/fade zones
- depth/parallax/sorting controls
- prefab export

## Project Goal

Create a Unity tool called **RIMA Realtime Room Painter**.

This is **NOT** a procedural dungeon generator first.
This is a **human-authored visual map/room builder**. The designer places and paints assets quickly, like a parallax map editor, then we add RIMA-specific automation later.

## Core MVP (10 maddelik)

1. **EditorWindow** — Create a Unity EditorWindow named `RIMA Room Painter`.
2. **Asset palette panel:**
   - Load sprites and prefabs from configured folders.
   - Show thumbnails.
   - Allow selecting an asset for placement.
3. **Layer system (10 layer):**
   - Floor
   - Edge
   - Cliff
   - Wall
   - Props
   - Decals
   - Lighting
   - Collision
   - Occlusion
   - Parallax
4. **SceneView placement:**
   - Click to place selected sprite/prefab.
   - Drag to move.
   - Delete selected object.
   - Duplicate selected object.
   - Optional grid snap on/off.
   - Snap size configurable.
   - Fine movement with keyboard.
5. **Sorting/depth controls:**
   - Sorting Layer
   - Order in Layer
   - Optional Y-sort mode.
   - Visual offset pixels.
   - Pivot/sorting anchor debug gizmo.
6. **Collision painting:**
   - Paint rectangular or polygon collision zones.
   - Store them as child GameObjects or generated `PolygonCollider2D`/`BoxCollider2D`.
   - Toggle collision debug view.
7. **Occlusion/fade zones:**
   - Create rectangles/polygons that mark areas where foreground objects should fade when the player walks underneath.
   - Store opacity/fade settings.
8. **Parallax/depth layer support:**
   - Each layer has depth value.
   - Each layer can be marked static, room-locked, or camera-relative.
9. **Save/export:**
   - Save current room as a prefab.
   - Save metadata as ScriptableObject or JSON.
   - Keep an editable RoomData asset.
10. **Guide:**
    - `/GUIDES/RIMA_REALTIME_ROOM_PAINTER_MVP.md`
    - Explain setup, folder structure, layer usage, and how to place assets.

## RIMA-specific later features (MVP stable olunca)

- **Cliff Edge Brush:** automatically place cliff skirts below floor edges.
- **Wall Line Brush:** drag a wall line and automatically place straight walls, corners, caps, doors, and pillars.
- **Prop Scatter Brush:** place themed props with weighted randomness.
- **Rift Decal Brush:** paint cyan cracks/glow decals.
- **Room Theme Presets:** ShatteredKeep, RitualChamber, Library, Prison, FloodedCrypt.

## ScriptableObject hierarchy

### RoomPainterAsset
- id
- displayName
- category
- sprite or prefab reference
- defaultLayer
- defaultSortingLayer
- defaultOrder
- defaultScale
- defaultVisualOffset
- defaultCollisionMode
- themeTags
- randomWeight

### RoomLayerDefinition
- id
- displayName
- sortingLayer
- baseOrder
- isParallax
- parallaxDepth
- locked
- visible

### RoomData
- roomName
- roomSize
- layers
- placedObjects
- collisionZones
- occlusionZones
- metadata

## Implementation rules

- **Do not place every asset on one Tilemap.**
- **Use separate child roots per layer** under a RoomRoot object.
- **Prefer SpriteRenderer or prefab placement** for grid-free parallax-style objects.
- **Use Tilemap only for parts that truly benefit from tile painting**, such as floor base.
- **Keep the system editor-first and designer-friendly.**
- **Add debug gizmos** for layer, collision, occlusion, sorting anchor, and snap grid.
- **Code locations:**
  - Editor: `/Assets/Editor/RIMA/RoomPainter/`
  - Runtime data: `/Assets/Scripts/RIMA/RoomPainter/`

## First deliverable

Analyze the current Unity project and asset folders.
Then implement the smallest working version:
- EditorWindow
- asset palette
- layer selection
- place selected sprite/prefab in SceneView
- snap on/off
- save RoomRoot as prefab
- basic collision rectangle brush
- documentation

**Report missing assets separately:**
If the asset pack does not contain proper wall segments, cliff edges, corners, caps, or door modules, list exactly what needs to be generated instead of faking it with unrelated props.

---

## Sentez beklemede

| AI | Status | Çıktı |
|---|---|---|
| **agy (Antigravity)** | ⚠️ Yarım (timeout) | Q1-Q4 cevaplı, Q5-Q7 eksik. 8 mekanik breakdown + Hades uyum tablosu sağlandı. |
| **Codex (GPT-5.5)** | 🔄 Çalışıyor (`b0z6hgdva`) | Bekleniyor — Q1-Q7 full coverage muhtemel |
| **ChatGPT (user verdict)** | ✅ Bu doc | Implementation spec (10-madde MVP + later features + SO hierarchy) |

Codex çıktığında: **üçlü sentez** → RIMA Realtime Room Painter implementation roadmap.
