# REVIEW TASK: parallax_L4 changes (agy / Antigravity)

Amac: Review the just-implemented "parallax_L4" stick-to-map / stick-to-camera
authoring changes for correctness bugs and RIMA convention issues. Respond
INLINE (NOT to a file). Be concise.

## Project root
F:/Antigravity Projeler/2d roguelite/RIMA

## Files to read and review
1. Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs   (editor-only authoring UI)
2. Assets/Scripts/Background/ParallaxLayer.cs                        (runtime parallax component)

Read both fully before judging. Briefly describe the intent in your own words
first, then review.

## Design focus (review against these specifically)
(a) Anchor toggle correctness: "Stick to Camera" must map to factor 1.0,
    "Stick to Map" must map to factor 0.0, and custom values sit in between.
(b) The toggles must write the EXISTING parallaxFactor field (RoomPainterAsset)
    that bakes onto ParallaxLayer.factor at scene-place time. There must be NO
    duplicate / parallel data model for the anchor state.
(c) Canonical authoring range 0.05 - 1.10 must be honored. The camera-lock /
    Stick-to-Map path is the only one allowed to clamp fully to 0.
(d) Editor-only code must stay inside the Editor asmdef (RIMA.RoomPainter.Editor)
    with no runtime leakage. EditorPreviewOffset / preview pan paths must be
    UNITY_EDITOR-guarded so nothing ships in player builds.
(e) No scene-breaking: parallax math must preserve Z (sorting), and edit-mode
    preview must not permanently drift layer origins.
(f) RIMA conventions: PPU 64, namespaces, surgical minimal code, no speculative
    fields, consistent factor semantics.

## Things to specifically sanity-check
- ParallaxSection MinFactor=0.01 / MaxFactor=1.50 vs ParallaxLayer canonical
  0.05 - 1.10: is the slider range inconsistent with the canonical contract?
- DrawStickPresets: isStickToMap uses asset.parallaxFactor directly, but
  isStickToCamera uses ResolveCurrentFactor(asset). Is that asymmetry a bug
  (e.g. when parallaxFactor is 0 and tier resolves to a non-zero value)?
- ResolveCurrentFactor returns 1f as a fallback when factor<=0 and no valid
  tier: does that mask the Stick-to-Map (0) state in the depth slider UI?
- factor is a Vector2 on ParallaxLayer but the authoring writes a single
  parallaxFactor scalar: confirm the X/Y mapping is intentional and not a
  silent data-loss mismatch.
- RecaptureAllLayerOrigins / EditorPreviewOffset round-trip: any chance of
  cumulative origin drift across repeated scrubs?

## Output format
- Concise findings list (bullet per issue, cite file + line/area).
- Final line: a single verdict token, exactly "PASS" or "CONCERNS".

Respond INLINE in your reply. Do NOT write to any file.
