# PixelLab Discord Analysis: api-and-sdk (Screenshots 011-015)

## Target Context
**Channel:** `api-and-sdk`
**Batch:** 011 - 015
**Date Analyzed:** 2026-05-02
**Purpose:** Technical extraction of API limits, timeout behaviors, proportion constraints, and stability patterns.

---

## Technical Synthesis

### 1. API Stability & Timeout Behaviors
- **Background WebSocket Errors:** Intermittent backend errors (`BackgroundWebSocket.receive_json() called but no request_data was provided at construction`) occur on background job endpoints (`/generate-image-v2`, `/generate-ui-v2`). This is a server-side issue causing immediate job failures after the `202` response.
- **Queue Times & Timeouts:** 
  - Standard timeout limits (like a 3-minute kill switch in Claude Code or similar scripts) might be too short during high load.
  - The `animate-with-text-v3` endpoint processing a 256x256 max frame payload can legitimately take **2-3 minutes** of processing time.
  - Under heavy queue conditions, jobs might take up to **8 minutes**. **Constraint:** Any automation script must extend polling timeouts to at least 10 minutes to avoid premature client-side aborts.
- **Stalling at 50% Progress:** A known issue exists on older endpoints (`/animate-with-text-v2`) where jobs stall at `0.49` or `0.50` progress, hanging for 15+ minutes or failing with an `Unknown error`. The `v3` endpoint is recommended for a much higher success rate.

### 2. Custom Proportions Constraints
When calling the `/v2/create-character-with-8-directions` endpoint with a custom `proportions` object, strict bounds are enforced.
- **Valid Range:** All custom proportion multipliers (e.g., `arms_length`, `head_size`, `hip_width`, `legs_length`, `shoulder_width`) **MUST** be within the range of **0.5 to 2.0**.
- **Error Behavior:** Passing values outside this bound (e.g., `head_size: 0.4`) results in a `422 Unprocessable Entity` error.
- **Documentation Note:** `head_size` recommends a max of `1.7`.

---

## Original Transcripts

### Screenshot 011
**ruge:** I'm experiencing consistent generation failures on the `generate-ui-v2` endpoint. Every job returns status "failed" immediately after processing begins — even when using the exact example from your documentation.
Account: Pro subscriber
Steps to reproduce:
POST `/v2/generate-ui-v2` with the following body:
```json
{
  "description": "medieval stone button with gold trim",
  "color_palette": "brown and gold",
  "image_size": { "width": 64, "height": 64 },
  "no_background": true,
  "seed": 42
}
```
Observed behavior:
Submit returns 202 with a `background_job_id`
Polling the job immediately returns status: "failed"
`usage.usd = 0` (no credits consumed)
Error detail: "Generation failed. Please try again."
One attempt returned a more specific detail: `"BackgroundWebSocket.receive_json() called but no request_data was provided at construction"`
Additional notes:
Issue is reproducible on every request (5+ attempts)
Affects both simple and complex descriptions
Affects requests with and without `concept_image`
Image sizes tested: 54x36 and 64x64

### Screenshot 012
**ruge:** didn't see any more problems so far thx for quick response
**s:** devs, if you may, how are you guys doing it? where are you based from? Could we talk in DM?
**jdjdjd6180:** Hi, I'm experiencing an issue with the `generate-image-v2` endpoint. All background jobs are failing immediately with the following error:
```json
{
  "code": 5000,
  "type": "error",
  "error": "Generation failed",
  "detail": "BackgroundWebSocket.receive_json() called but no request_data was provided at construction"
}
```
Request body:
```json
{
  "description": "a cute cactus",
  "image_size": { "width": 48, "height": 48 },
  "no_background": true
}
```
The job is created successfully (returns `background_job_id` with status "processing"), but when polling via GET `/background-jobs/{id}`, it immediately returns status "failed" with the error above.
I've also tested with 32x32 size — same result. Other endpoints like `create-image-bitforge` are working fine with the same API key.
This was working correctly 2 days ago. No changes were made to the request structure on my end.
API Tier: 3 (Pixel Architect)
Endpoint: POST `/v2/generate-image-v2`
Is this a known server-side issue? Thanks!

### Screenshot 013
**Kaninen:** oh, seems like something might be wrong i will take a look. Thank you for reporting
**SiiKOZ:** is it back ? I got the issue too. But I don't want to burn my credit
**Kaninen:** yes should be good now
**SiiKOZ:** Thank you
**meatsuit:** my requests are hanging and returning nothing and still taking credits
**Kaninen:** hmm, sounds weird that they dont return anything. The queue was long but they should still complete eventually
**meatsuit:** how long is long? my claude code would kill the call after ~3min
waited even longer sometimes
**Kaninen:** wait till its done
with that queue time (when machines had crashed) maybe it could take like 8 minutes?
but ye, the animate with text v3 itself with 256x256 max frames can take 2-3 minutes on its own
**meatsuit:** its just weird first one would return fast then just hang
just a 64x64 hero
then after a bit no progress at all no longer how long I waite
i'll try it again right now though and see
**meatsuit:** didn't see any more problems so far thx for quick response

### Screenshot 014
*(Screenshot shows API documentation for Character body proportions)*
`proportions` nullable
Character body proportions (preset or custom values). Only applies to humanoid characters.
One of `CharacterProportions`
Character proportions with individual control.
- `arms_length` number - Arms Length - min: 0.5 - max: 2 - Default. Arm length multiplier
- `head_size` number - Head Size - min: 0.5 - max: 2 - Default. Head size multiplier (recommended max: 1.7)
- `hip_width` number - Hip Width - min: 0.5 - max: 2 - Default. Hip width multiplier
- `legs_length` number - Legs Length - min: 0.5 - max: 2 - Default. Leg length multiplier
- `shoulder_width` number - Shoulder Width - min: 0.5 - max: 2 - Default. Shoulder width multiplier
- `type` const: custom - Default. Proportion type identifier

**Neo:** OMG Thank you!!!

### Screenshot 015
*(User continuing an issue report)*
Results across 11 sequential tests were 1 success, 10 fails, all stalling at .50 or .49, 9 of which returned 'unknown error' one of which I quit after 60 polls (945 seconds)
Key observations:
- The `no_background` parameter makes no difference
- Different seeds make no difference
- Failure always happens at the ~50% progress boundary (first-pass -> second-pass transition?)
- Test 11 never got a "failed" status — it sat at 0.49 for 15+ minutes and the server never timed it out
Is this a known issue? Is there anything on the client side that could help, or is this a server-side problem at the processing boundary? Happy to provide more details or logs.
I have a Tier 2 Pixel Artisan subscription
Thanks!
**jojowiga:** Hi, I'm experiencing a very high failure rate on the `/animate-with-text-v2` endpoint. Jobs consistently stall at exactly 49-50% progress, sit there for 2-4 minutes, then report status "failed" with "Unknown error". When it doesn't fail the results are as expected. To rule out my code...
**Kaninen:** Sorry to hear that, to verify do you mean animate with text v3? the new one
10% sucess rate seems incredibly low, could you dm me your email so i can check if i can find what happened
**jojowiga:** Hi, I mean v2. v3 has a better rate. I will send my email
**Neo:** Hi, I'm getting a 422 error on POST `/v2/create-character-with-8-directions` when using custom character proportions (`head ~0.4` with heroic-ish body). Presets like `realistic_male` and `heroic` work fine, but any custom values for `head`, `arms_length`, `legs_length`, `shoulder_width`, `hip_width` return 422. Is this related to the known bone_scaling server-side bug? Could someone share a working raw JSON payload example with custom proportion values? Thanks!
**Imakero:** If I'm not mistaken all the custom proportions need to be in the range 0.5 to 2.0
Yep, `https://api.pixellab.ai/v2/docs#tag/character-from-template/POST/create-character-with-8-directions`
