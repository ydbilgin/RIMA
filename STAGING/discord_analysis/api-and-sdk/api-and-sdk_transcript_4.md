# PixelLab Discord Analysis: api-and-sdk (Screenshots 016-020)

## Target Context
**Channel:** `api-and-sdk`
**Batch:** 016 - 020
**Date Analyzed:** 2026-05-02
**Purpose:** Technical extraction of complex payload structures, `force_colors` constraints, and MCP vs V2 API discussions.

---

## Technical Synthesis

### 1. Advanced Prompting & Rules Enforcement
An extensive payload example by user "Tommy" demonstrates the community's approach to strict constraint management for character generation:
- **Framing Lock:** Strict sizing constraints enforced directly in the prompt: `Final frame: 48x48`, `Target visible character: ~24x44`, `Leave ~2px padding top and bottom (10% vertical buffer)`.
- **Direction Lock:** `4 directions only: south, north, east, west`, `Exact same character across directions (no redesign)`.
- **Palette Control (`force_colors`):** The API accepts a `color_image` (Base64) accompanied by `force_colors: true`. 
  - *Bug Fix Noted:* The API previously expected raw RGBA bytes but has been patched to accept PNG-encoded Base64 strings.
  - The prompt also includes exhaustive `PALETTE SLOT MEANINGS` and `SKIN COLOR RULES` to prevent the model from deviating from standard shading ramps.
- **Rendering Style (Strict):** `no anti-aliasing`, `no smoothing`, `no blended pixels`, `no semi-transparent pixels`, `no gradients, blur, glow, or filters`, `hard pixel edges, discrete palette usage, clean pixel clusters`.

### 2. API Quirks and Bugs
- **Isometric Tiles:** The `create isometric tile` endpoint natively produces a different perspective compared to `create-tiles-pro`. (Resolution/Setting adjustments weren't answered in this snippet, but it highlights a discrepancy).
- **Background Removal:** On `animate-v3`, there is a known bug where the API occasionally introduces a gray background and fails to remove it.
- **Silent Job Failures:** Background tasks can sometimes create a DB record but produce zero logs, getting permanently stuck in the "initializing" state. This requires server-side clearance.

### 3. MCP vs V2 API
- **Tool Discrepancies:** The community notes that the V1 API is missing many tools. V2 is the recommended path for automation, despite technically being "unofficial" at the time of these screenshots.
- **LLM Context:** For agentic use, the recommended source of truth for the LLM context is `https://api.pixellab.ai/v2/llms.txt`.
- **Hybrid Workflow:** Some users consider using the MCP for generating 4-direction base sprites and then switching to the V2 API (via code) to handle animation generation logic.

---

## Original Transcripts

### Screenshot 016
**PixelLab Bot:** Hey Tommy! We found the issue - your `color_image` was PNG-encoded but our system expected raw RGBA bytes. We just deployed a fix that accepts both formats, so `force_colors` with a `color_image` should work now. We also cleared your stuck job. Give it another try and let us know if it works!
**bolyai:** Hi - can anyone let me know why the create isometric tile endpoint produces isometric tiles in a different perspective than the create-tiles-pro? what settings do i need to adjust to get the same behavior?
**PixelLab Bot:** Hey Tommy! Investigated your job 136c250b. Found 3 of your jobs stuck at "initializing" (same Ari Orchard character). The background tasks created DB records but produced zero logs - the async tasks silently died before processing started. I've cleared all 3 stuck jo...
**Saavy:** If I happened to gen nsfw and you call me out like that we would have a world war.
Just a thought I'd share, in case this was an automated response
**孟浩:** is animate-v3 introducing a gray background expected? I can't seem to remove background using API as I do via Aseprite
**Kaninen:** it should remove the backgroud but there seems to be a bug where it sometimes doesnt do that
**jojowiga:** Hi, I'm experiencing a very high failure rate on the `/animate-with-text-v2` endpoint. Jobs consistently stall at exactly 49-50% progress, sit there for 2-4 minutes, then report status "failed" with "Unknown error". When it doesn't fail the results are as expected.
To rule out my code, I tested with a minimal standalone script — no SDK, no wrapper, just raw httpx POST + polling. Simplest possible input: 64x64 PNG, action = "walking forward".
Results across 11 sequential tests were 1 success, 10 fails, all stalling at .50 or .49, 9 of which returned 'unknown error' one of which I quit after 60 polls (945 seconds)

### Screenshot 017 & 018
*(Extensive payload posted by Tommy)*
```json
{
  "stage": "create",
  "task": {
    "kind": "npc_sprite_4dir",
    "slug": "ari-orchard",
    "spec_path": "pixellab/characters-4dir/ari-orchard.json",
    "prompt_path": null
  },
  "payload": {
    "description": "Ari Orchard, Traveling Orchard Grower. Bright scarf, straw hat, easy grin, and sleeves rolled for climbing ladders and pruning branches. Mischievous, playful, and social. Cozy medieval fantasy farming MMO NPC. Style: cozy medieval fantasy pixel art.\n\nFRAMING:\n- Final frame: 48x48\n- Target visible character: ~24x44\n- Leave ~2px padding top and bottom (10% vertical buffer)\n- Do not fill full height; preserve breathing room for animation\n- 4 directions only: south, north, east, west\n- Exact same character across directions (no redesign)\n\nSKIN COLOR RULES:\n- skin must use skin_light and skin_mid only\n- off_white must never be used for skin\n\nPALETTE SLOT MEANINGS:\n- transparent: reserved (not a shading color)\n- outline_dark: line work / outline\n- off_white: cool highlight / neutral light\n- skin_light, skin_mid, skin_shadow: skin ramp only\n- hair_light, hair_mid, hair_shadow: hair ramp only\n- base_light, base_mid, base_shadow: primary outfit/material ramp (sprites)\n- cloth_1_light/mid/shadow: primary clothing ramp (portraits)\n- cloth_2_light/mid/shadow: secondary clothing ramp (portraits)\n- accent_1, accent_2: secondary accents\n\nPIXEL RENDERING RULES (STRICT):\n- no anti-aliasing\n- no smoothing\n- no blended pixels\n- no semi-transparent pixels\n- no gradients, blur, glow, or filters\n- hard pixel edges, discrete palette usage, clean pixel clusters\n\nNEGATIVE PROMPT:\n- no anti-aliasing\n- no blur\n- no soft shading\n- no gradients Palette guidance: primary: earthy neutral, secondary: soft accent color, accent: profession accent, leather: brown.",
    "image_size": {
      "width": 64,
      "height": 64
    },
    "async_mode": true,
    "view": "high top-down",
    "seed": 131121082,
    "color_image": {
      "type": "base64",
      "base64": "iVBORw0KGgoAAAANSUhEUgAA...",
      "format": "png"
    },
    "force_colors": true,
    "time": "2026-03-21T17:55:49+00:00"
  }
}
```
**Tommy:** removed the `color_image` and `force_colors` tag and boom, it generated. T_T
**Kaninen:** Ah force color
**Tommy:** Remove force_color?
**Tommy:** Appears to be stalling out even with force colors removed.
**PixelLab Bot:** Hey Tommy! Investigated your job 136c250b. Found 3 of your jobs stuck at "initializing" (same Ari Orchard character). The background tasks created DB records but produced zero logs - the async tasks silently died before processing started.
I've cleared all 3 stuck jobs so they won't block your concurrent job limit anymore. Please retry - it should work now. If it stalls again, let us know and we'll dig deeper into the force_colors + color_image code path.
**Tommy:** According to create-character I still have a job pending/stalled out. Would be job:54f01f47-5213-4f6b-...
**PixelLab Bot:** Hey Tommy! We found the issue - your color_image was PNG-encoded but our system expected raw RGBA bytes. We just deployed a fix that accepts both formats, so force_colors with a color_image should work now. We also cleared your stuck job.

### Screenshot 019
**Tommy:** What is the difference/benefit in using MCP or the v2 api?
**Imakero:** Some things are not in the mcp I think, but other than that personal preference and how you like to work.
**Tommy:** Sweet. I'm thinking of using the mcp to generate my 4 direction sprites and then using v2 to do my animations and such. I'm not sure 100% how I want to do it yet, tbh.
**Imakero:** I see, I didn't know about that.
**Tommy:** Sorry!
**Imakero:** Well, since v1 is missing a lot of the new tools you'll probably want to use v2 anyways. It's more that we've reserved the rights to change how things work in v2.
But we should make it official soon.
**Tommy:** No worries. I'm 100% using v2. LOL
**SirScriptsALot:** Excited to be using pixellab for my game. Doing aseprite for high quality stuff but really want to get the api figured out so i can automate more work.
**Tommy:** Can one of the devs look at this job and tell me why it's failing? job: 136c250b-f770-4738-aaf2-cf0734a18f2c
**Kaninen:** Send 64x64 color image

### Screenshot 020
*(Screenshot shows Cursor MCP Server configuration and setting up assistant JSON)*
**User:** Any ideas? Nevermind, I got it working.
**Tommy:** What is the difference/benefit in using MCP or the v2 api?
**Tommy:** IDK who is in charge of the website, but on `https://www.pixellab.ai/pixellab-api` the view docs link links to v1 docs (`https://api.pixellab.ai/v1/docs`) and not v2 docs (`https://api.pixellab.ai/v2/docs`)
**Imakero:** Yep, the v2 api is not the official one yet but it probably should be very soon
**Tommy:** Ah okay. We're shoving the v2 to the llm though, no?
`https://api.pixellab.ai/v2/llms.txt`
when I click AI assistant docs it copies that
