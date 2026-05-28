# LaurethStudio 2D Painter Suite

Visual Collider Painter + Iso/2D Parallax Layer Painter for Unity.

**Status:** v0.1.0 -- internal alpha (scaffold only)
**Unity:** 2022.3 LTS+
**License:** MIT (placeholder, may change before Asset Store release)

## What this package does

- **Visual Collider Painter** -- Drag in SceneView to create Collider2D components. Multiple unique colliders per object. Template library + copy-paste. Replaces Unity's Inspector-driven Edit Collider workflow.
- **Iso/2D Parallax Layer Painter** -- Photoshop-style layer composition with realtime parallax depth preview. Sort-axis aware (iso/2.5D supported). Drag-drop sprites into SceneView.

## Install

Embedded UPM package. Manifest entry:

```json
"com.laureth.painter-suite": "file:Packages/com.laureth.painter-suite"
```

## Open

`Window > LaurethStudio > Painter Suite`

## Status (v0.1.0)

- [x] UPM scaffold (package.json + asmdef)
- [x] Runtime: ParallaxLayer.cs
- [x] Editor: PainterSuiteWindow stub
- [ ] Collider Painter (drag-to-create)
- [ ] Layer Painter (drag-drop sprite)
- [ ] Template system
- [ ] Demo pack
- [ ] Docs + tutorial

## Roadmap

See `STAGING/LAURETH_2D_PAINTER_SUITE_PLAN_V2_RIMA_REUSE.md` (project root).
