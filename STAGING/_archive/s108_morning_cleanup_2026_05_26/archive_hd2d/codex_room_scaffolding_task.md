# Codex Task — RIMA Room Template Scaffolding

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Scaffold the Unity C# code skeleton for RIMA's hybrid room generation architecture (Option C, locked per `STAGING/architecture_decision.md`). NO rendering logic yet — only data structures + component skeletons. Wires up template + decor overlay system that loads PixelLab-pixel-art templates and spawns decor at authored anchor points.

## NLM ACCESS
If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

Direct-read only: CURRENT_STATUS.md, .claude/PROJECT_RULES.md, STAGING/architecture_decision.md, STAGING/codex_arch_review.md, existing C# code under Assets/Scripts/

## Files to create (4 new files only)

### 1. `Assets/Scripts/Rooms/RoomTemplate.cs`
ScriptableObject — defines a baked room (LoRA-generated full painting) + collision data + decor anchor points.

Fields:
- `string templateId` (string, unique key per template)
- `Sprite baseImage` (the LoRA-generated room painting, 1024×1024 typical)
- `Vector2[] wallPathLocalPoints` (PolygonCollider2D vertices in local space; serialized as array)
- `OverlayAnchor[] anchors` (decor spawn points, see #2)
- `Vector2[] doorSocketsLocalPoints` (door connection points for procgen room linking)
- `Vector2[] enemySpawnPoints` (combat encounter spawn positions)
- `Vector2 cameraBoundsCenter`, `Vector2 cameraBoundsSize` (camera confine rectangle)
- `string biomeTag` (e.g., "ShatteredKeep", "BleedingWastes" — for procgen filtering)
- `string lightingVariant` (e.g., "day", "dusk", "rift_corrupted" — multiple variants of same template)

Use `[CreateAssetMenu]` so user can right-click create instances.

### 2. `Assets/Scripts/Rooms/OverlayAnchor.cs`
Serializable struct (NOT ScriptableObject — embedded in RoomTemplate).

Fields:
- `Vector2 localPos` (position in template local space)
- `DecorCategory category` (enum)
- `bool required` (must spawn something here?)
- `float spawnWeight` (probability weight when picking)
- `string optionalTag` (for filtering specific subtype, e.g. "altar_intact_only")

### 3. `Assets/Scripts/Rooms/DecorCategory.cs`
Public enum.

Values (per architecture_decision.md decor catalog):
```
Torch, Banner, Statue, Debris, RiftVein, Altar, BreakableProp, FloorDecal, WallCrack
```

### 4. `Assets/Scripts/Rooms/RoomDecorationSpawner.cs`
MonoBehaviour — attaches to spawned room root. On `Initialize(RoomTemplate template, int runSeed)`:
- Iterate template.anchors
- For each anchor: pick a decor prefab from a registry (TBD — leave a `[SerializeField] DecorRegistry registry` field but don't implement DecorRegistry yet)
- Instantiate prefab at anchor.localPos relative to transform
- Apply random mirror flip if `template.mirrorFlipAllowed`
- Use System.Random seeded with runSeed for determinism

Skeleton only:
- `public void Initialize(RoomTemplate template, int runSeed) { /* TODO */ }`
- `private GameObject SpawnDecorAtAnchor(OverlayAnchor anchor, System.Random rng) { /* TODO */ return null; }`
- Don't implement the prefab picking yet — leave `// TODO: pick from registry` comments.

## DO NOT do in this task
- DO NOT implement DecorRegistry / decor prefab assets
- DO NOT modify RimaWorldPainterWindow (separate future task)
- DO NOT touch any scene files
- DO NOT add rendering code (no SpriteRenderer setup)
- DO NOT compile-test in Unity (orchestrator will verify)
- DO NOT create any test scripts

## Validation
- All 4 files compile (verify with `dotnet build` if available, else just syntax sanity check)
- No external using directives except UnityEngine + System
- All public types have brief XML doc comment (≤ 2 lines each)
- Follow existing Assets/Scripts/ naming conventions

## Output
Write code files to disk. Report file paths in `CODEX_DONE_*.md` per cx_dispatch protocol.
