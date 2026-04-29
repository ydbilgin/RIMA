# KIRO TASK — Character Base Animations (Phase 1 + 2 Pilot)
*Date: 2026-04-08 | Read this file, apply in order. Do not read other files.*

---

## RISK LEVEL: LOW

---

## CREDENTIALS

**PixelLab REST API:** `https://api.pixellab.ai/v2/`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

User has 4 direction base sprites (S/N/E/W, 128×128px) for each of 4 characters:
- Warblade, Elementalist, Ranger, Shadowblade

Phase 1: Generate diagonal rotations (NE/NW/SE/SW) for each character from their south sprite.
Phase 2 (PILOT): Generate idle animation for Warblade south direction only, using their sprite as reference.
After Phase 2: Stop and report. Claude will decide whether to continue with full batch.

---

## FILES TOUCHED

**Read (inputs):**
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_S.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_N.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_E.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_W.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base/elementalist_S.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base/elementalist_N.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base/elementalist_E.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base/elementalist_W.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base/ranger_S.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base/ranger_N.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base/ranger_E.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base/ranger_W.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base/shadowblade_S.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base/shadowblade_N.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base/shadowblade_E.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base/shadowblade_W.png`

**Write (outputs):**
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/{Char}/base/{char}_NE.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/{Char}/base/{char}_NW.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/{Char}/base/{char}_SE.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/{Char}/base/{char}_SW.png`
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/animations/fight-stance-idle/south/frame_000.png` (pilot only)
- `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/_STAGING/DONE.txt`

---

## STOP AND ESCALATE — Report to Claude if:

- Any step requires a decision or judgment
- Output image is all black, all transparent, or obviously wrong character
- API returns error other than timeout/retry
- Phase 2 pilot produces zero valid frames
- You are about to touch a file not listed above

---

## MANDATORY QC PROTOCOL

After each generation:
1. Read the output PNG
2. Describe: "What character, what direction, what pose?"
3. **PASS:** Correct character visible, facing correct direction, transparent background, no black fill
4. **FAIL:** Black image, wrong direction (mirrored when it shouldn't be), missing character, pixel noise/static
5. On FAIL: retry once with same parameters. If still fails, note in REPORT and continue to next.

---

## HELPER CODE — Async Poll

Use this pattern for all async API calls:

```python
import requests, base64, time, os

API_BASE = "https://api.pixellab.ai/v2"
HEADERS = {"Authorization": "Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"}

def load_b64(path):
    with open(path, "rb") as f:
        return base64.b64encode(f.read()).decode()

def save_png(b64_data, path):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "wb") as f:
        f.write(base64.b64decode(b64_data))

def poll_job(job_id, timeout=300):
    """Poll until job complete. Returns result dict."""
    for _ in range(timeout // 5):
        r = requests.get(f"{API_BASE}/background-jobs/{job_id}", headers=HEADERS)
        data = r.json()
        if data.get("status") == "complete":
            return data
        if data.get("status") == "failed":
            raise RuntimeError(f"Job {job_id} failed: {data}")
        time.sleep(5)
    raise TimeoutError(f"Job {job_id} timed out after {timeout}s")
```

---

## PHASE 1 — Generate Diagonals (4 characters)

For each character, call `generate-8-rotations-v2` using the SOUTH sprite.
The endpoint returns 8 directions. Save only the 4 diagonals (indices 1,3,5,7).

**Direction index map:**
```
index 0 = south      → SKIP (already have)
index 1 = south-west → save as {char}_SW.png
index 2 = west       → SKIP (already have)
index 3 = north-west → save as {char}_NW.png
index 4 = north      → SKIP (already have)
index 5 = north-east → save as {char}_NE.png
index 6 = east       → SKIP (already have)
index 7 = south-east → save as {char}_SE.png
```

```python
CHARACTERS = [
    {
        "name": "Warblade",
        "key": "warblade",
        "south_path": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_S.png",
        "base_dir": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base",
    },
    {
        "name": "Elementalist",
        "key": "elementalist",
        "south_path": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base/elementalist_S.png",
        "base_dir": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/base",
    },
    {
        "name": "Ranger",
        "key": "ranger",
        "south_path": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base/ranger_S.png",
        "base_dir": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/base",
    },
    {
        "name": "Shadowblade",
        "key": "shadowblade",
        "south_path": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base/shadowblade_S.png",
        "base_dir": "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/base",
    },
]

DIAGONAL_MAP = {1: "SW", 3: "NW", 5: "NE", 7: "SE"}

for char in CHARACTERS:
    print(f"Generating diagonals for {char['name']}...")
    b64 = load_b64(char["south_path"])

    payload = {
        "image_size": {"width": 128, "height": 128},
        "method": "rotate_character",
        "reference_image": {
            "image": {
                "base64": b64,
                "type": "base64",
                "format": "png"
            },
            "width": 128,
            "height": 128
        },
        "view": "low top-down",
        "no_background": True
    }

    r = requests.post(f"{API_BASE}/generate-8-rotations-v2", json=payload, headers=HEADERS)
    r.raise_for_status()
    job_id = r.json()["background_job_id"]
    print(f"  Job ID: {job_id}")

    result = poll_job(job_id)
    print(f"  Response keys: {list(result.keys())}")  # Log for Claude to verify

    # result["images"] is a list of 8 base64 images (key may vary — log above confirms)
    images = result.get("images", result.get("frames", result.get("rotations", [])))
    if len(images) != 8:
        print(f"  WARNING: Expected 8 images, got {len(images)}")

    for idx, suffix in DIAGONAL_MAP.items():
        if idx < len(images):
            out_path = f"{char['base_dir']}/{char['key']}_{suffix}.png"
            save_png(images[idx], out_path)
            print(f"  Saved: {out_path}")
        else:
            print(f"  MISSING index {idx} ({suffix})")

    print(f"  {char['name']} diagonals done.")
```

**QC after each character:**
- Open each saved PNG
- PASS: correct character visible, diagonal direction plausible, no black fill, transparent bg
- FAIL: record in REPORT, do NOT retry (continue to next character)

---

## PHASE 2 — Pilot: Warblade Idle (south only)

Generate fight-stance-idle animation for Warblade, south direction, using their south sprite as reference.
This is a **pilot** — generate south only. Stop after this.

```python
WARBLADE_SOUTH = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/base/warblade_S.png"
OUT_DIR = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/animations/fight-stance-idle/south"

b64 = load_b64(WARBLADE_SOUTH)

payload = {
    "reference_image": {
        "type": "base64",
        "base64": b64,
        "format": "png"
    },
    "reference_image_size": {"width": 128, "height": 128},
    "action": "Heavily armored plate warrior standing in combat fight stance, breathing slightly, idle loop animation, low top-down view, facing downward toward camera, no movement, looping",
    "image_size": {"width": 128, "height": 128},
    "view": "low top-down",
    "direction": "south",
    "no_background": True
}

r = requests.post(f"{API_BASE}/animate-with-text-v2", json=payload, headers=HEADERS)
r.raise_for_status()
job_id = r.json()["background_job_id"]
print(f"Pilot job ID: {job_id}")

result = poll_job(job_id)

frames = result.get("frames", result.get("images", []))
print(f"Frames received: {len(frames)}")

os.makedirs(OUT_DIR, exist_ok=True)
for i, frame_b64 in enumerate(frames):
    save_png(frame_b64, f"{OUT_DIR}/frame_{i:03d}.png")
    print(f"  Saved frame_{i:03d}.png")

print("Pilot complete. STOP HERE — report to Claude.")
```

**QC for pilot:**
- Read frame_000.png
- PASS: Warblade character visible, south-facing, slight breathing/idle movement between frames, transparent bg
- FAIL: black frames, wrong character, static (single frozen frame), no transparency
- Record frame count actually received

---

## PHASE 3 — DO NOT EXECUTE

Phase 3 (full animation batch) will be written by Claude after reviewing the pilot results.
Stop after Phase 2. Fill REPORT below and tell the user.

---

## COMPLETION LOG

Append to `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/_STAGING/DONE.txt`:

```
[DONE-CHAR-DIAGONALS] 4 chars × 4 diagonal dirs (NE/NW/SE/SW) | 2026-04-08
[DONE-PILOT-IDLE] Warblade fight-stance-idle south pilot | 2026-04-08
[QC-DIAGONALS] [char] NE/NW/SE/SW PASS/FAIL | 2026-04-08
[QC-PILOT] Warblade idle south PASS/FAIL — [frame count] frames | 2026-04-08
```

---

## REPORT — Fill before saying anything to user

```
STATUS: FAILED

COMPLETED:
  - Phase 1 diagonals — 0 out of 16 saved
  - Phase 2 pilot — NOT STARTED

ERRORS:
  - REST API generate-8-rotations-v2: Job creation succeeds but get-image/{job_id} returns 404 Not Found
  - Tested with 300s and 600s timeouts, both failed
  - Job IDs attempted: 65a47f5f-c4c0-4402-a4f4-c3139462d6a4, 90fae91f-4f26-4088-bdc6-3f6832cdc957
  - Polling endpoint consistently returns {"detail":"Not Found"}
  - Phase 1 blocked, Phase 2 not attempted

QC_RESULT:
  - Warblade diagonals — NOT GENERATED — API failure
  - Elementalist diagonals — NOT GENERATED — API failure
  - Ranger diagonals — NOT GENERATED — API failure
  - Shadowblade diagonals — NOT GENERATED — API failure
  - Pilot idle south — NOT GENERATED — Phase 1 prerequisite failed

PILOT_FRAMES: N/A
PILOT_RESPONSE_KEYS: N/A

ESCALATION: REST API endpoint generate-8-rotations-v2 is non-functional. MCP tools do not have direct equivalent. Alternative approach needed.

NEXT_SIGNAL: [BLOCKED - awaiting Claude decision]
```
