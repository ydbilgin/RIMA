# Dash Trail -- Autosprite MCP Pipeline

## Specs
- Canvas: 64x64 px per frame
- Frames: 8 extracted (maxFrames: 8)
- Color: cold blue, transparent background
- Blend mode in Unity: Additive
- Direction: non-directional

## Pipeline

Autosprite does NOT generate spritesheets from a prompt directly.
The correct pipeline is:
  Step 1: generate_asset_preview  -- generates source still image (1 credit)
  Step 2: create_asset            -- saves selected preview URL as asset (free)
  Step 3: animate_asset           -- generates animation video from asset (credit cost TBD)
  Step 4: generate_asset_spritesheet -- extracts frames from video (free)

NOTE: animate_asset schema was not confirmed in this session (deferred tool).
Load schema via ToolSearch "select:mcp__autosprite__animate_asset" before executing Step 3.

## Step 1 -- generate_asset_preview MCP call

Tool: mcp__autosprite__generate_asset_preview

Parameters:
  category:    "effect"
  style:       "pixel"
  quality:     "ultra"
  description: "Dash trail VFX, cold blue #4FC3F7 glowing smear, transparent bg, painterly pixel art, no character, radial alpha fade, icy particle flecks, 64x64"

NOTE: description is capped at 200 characters. The line above is 159 chars -- within limit.
Returns 1 URL (ultra) or 4 URLs (turbo) -- user selects best, passes to Step 2.

## Step 2 -- create_asset MCP call

Tool: mcp__autosprite__create_asset

Parameters:
  name:        "RIMA_DashTrail_BlueSmear_v1"
  imageUrl:    "<URL selected from Step 1>"
  description: "Dash trail VFX 64x64 cold blue, RIMA pilot"

Returns: assetId (required for Steps 3 and 4)

## Step 3 -- animate_asset MCP call

Tool: mcp__autosprite__animate_asset  (load schema before calling)

Suggested intent: animate the saved still into a looping smear cycle.
Exact parameters: UNKNOWN until schema loaded. Check ToolSearch first.

## Step 4 -- generate_asset_spritesheet MCP call

Tool: mcp__autosprite__generate_asset_spritesheet

Parameters:
  assetId:   "<assetId from Step 2>"
  frameSize: 64
  maxFrames: 8
  removeBg:  "ultra"

Returns: jobId -- poll with get_asset_job_status until complete, then get_asset_spritesheet.

## Acceptance criteria
- 8 frames extracted at 64x64
- Background transparent (removeBg "ultra" mode)
- Cold blue core visible in majority of frames
- Loop reads at 12 fps in Unity additive blend
- No hard silhouette of character or directional shape

## Cost estimate
- Step 1 generate_asset_preview (ultra): 1 credit
- Step 2 create_asset: free
- Step 3 animate_asset: TBD (schema unknown -- expect 1-5 credits based on provider pattern)
- Step 4 generate_asset_spritesheet: free
- Total estimated: 2-6 credits per pilot run

## Blockers
- animate_asset schema not confirmed -- MUST load via ToolSearch before executing Step 3
- Autosprite animation style fidelity unknown for VFX (first real test per memory)
- No guarantee of cyclic loop behavior; may need manual frame selection in Aseprite
