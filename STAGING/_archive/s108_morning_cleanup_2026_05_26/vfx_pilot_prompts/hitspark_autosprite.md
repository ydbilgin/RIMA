# Hitspark -- Autosprite MCP Pipeline

## Specs
- Canvas: 64x64 px per frame
- Frames: 6 extracted (maxFrames: 6)
- Color: white core, warm orange edge (pilot placeholder for per-class tinting)
- Blend mode in Unity: Additive
- Type: one-shot (not a loop)
- Background: fully transparent

## Pipeline

Same 4-step pipeline as dash_trail_autosprite.md:
  Step 1: generate_asset_preview  (1 credit)
  Step 2: create_asset            (free)
  Step 3: animate_asset           (schema TBD)
  Step 4: generate_asset_spritesheet (free)

NOTE: animate_asset schema must be loaded via ToolSearch before Step 3.
See dash_trail_autosprite.md for full pipeline blocker notes.

## Step 1 -- generate_asset_preview MCP call

Tool: mcp__autosprite__generate_asset_preview

Parameters:
  category:    "effect"
  style:       "pixel"
  quality:     "ultra"
  description: "Hitspark contact burst VFX, white core radial spikes, orange edge fade, transparent bg, painterly pixel art, no character, 64x64, symmetric burst"

NOTE: description above is 174 chars -- within 200 char limit.
Returns 1 URL (ultra) -- user selects, passes to Step 2.

## Step 2 -- create_asset MCP call

Tool: mcp__autosprite__create_asset

Parameters:
  name:        "RIMA_Hitspark_WhiteBurst_v1"
  imageUrl:    "<URL selected from Step 1>"
  description: "Hitspark VFX 64x64 white core radial, RIMA pilot"

Returns: assetId (required for Steps 3 and 4)

## Step 3 -- animate_asset MCP call

Tool: mcp__autosprite__animate_asset  (load schema before calling)

Suggested intent: animate the burst from compact flash to full expansion then fade.
Exact parameters: UNKNOWN until schema loaded.
Recommended: if schema supports "style" or "motion" param, target "burst expand fade".

## Step 4 -- generate_asset_spritesheet MCP call

Tool: mcp__autosprite__generate_asset_spritesheet

Parameters:
  assetId:   "<assetId from Step 2>"
  frameSize: 64
  maxFrames: 6
  removeBg:  "ultra"

Returns: jobId -- poll with get_asset_job_status, then get_asset_spritesheet.

## Post-processing note
Autosprite animation may produce a looping video.
For a one-shot VFX, select only the first 6 frames covering the expand-then-fade arc.
Discard any loop-back frames. Trim in Aseprite if needed.

## Unity integration note
Tint the SpriteRenderer color to per-class color at runtime via MaterialPropertyBlock.
The white core blends correctly with additive material regardless of tint color.
Edge warm-orange from generation becomes class-color after tinting (approximate).

## Acceptance criteria
- 6 frames extracted at 64x64
- Background transparent (removeBg "ultra")
- White-dominant core at peak frame (frame 2-3)
- Radial spike pattern visible at 64px scale
- Frame 6 near-transparent for clean one-shot stop in Unity

## Cost estimate
- Step 1 generate_asset_preview (ultra): 1 credit
- Step 2 create_asset: free
- Step 3 animate_asset: TBD (schema unknown)
- Step 4 generate_asset_spritesheet: free
- Total estimated: 2-6 credits per pilot run

## Blockers
- animate_asset schema not confirmed -- load via ToolSearch before Step 3
- One-shot vs loop extraction depends on Autosprite video output behavior (untested)
