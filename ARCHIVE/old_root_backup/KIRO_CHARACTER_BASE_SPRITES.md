# KIRO TASK — Character Base Sprites: 8-Direction Rotation
*Date: 2026-04-08 | Read this file only. Do not read other files.*

---

> ⚠️ SEN KİROSUN — EXECUTION AGENT.
> Sadece bu dosyada yazılı adımları uygula.
> - Başka dosya okuma (CURRENT_STATUS, README vs.) YOK
> - Karar verme, ekleme, değiştirme YOK
> - Agent başlatma YOK
> - Status güncelleme YOK
> Sadece verilen görevi yap. Bitmeden önce dur ve bildir.

---

## RISK LEVEL: LOW

Deterministic, mechanical, isolated, bounded, verifiable.

---

## CREDENTIALS

```
PixelLab API: https://api.pixellab.ai/v2
Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0
```

---

## CONTEXT

User has created 4 cardinal direction sprites (S, N, E, W) for 4 characters using pixellab.ai.
Use `generate-8-rotations-v2` with `rotate_character` to produce all 8 directions from each south.png.
This gives consistent diagonals (NE, NW, SE, SW) that match the cardinal directions.

---

## FILES TOUCHED

**Input (read only):**
```
C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/shadowblade/shadowblade_S.png
C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/elementalist/elementalist_S.png
C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/ranger/ranger_S.png
C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/warblade/warblade_S.png
```

**Output (create):**
```
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/reference/
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/reference/
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/reference/
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/reference/
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/STAGING/DONE.txt
```

Each reference folder gets 8 files:
`south.png, north.png, east.png, west.png, south-east.png, south-west.png, north-east.png, north-west.png`

---

## STOP AND ESCALATE — Report to Claude if:

- API returns an error
- Output character looks different from input (wrong outfit, wrong gender)
- Black or empty frames
- Job does not complete after 10 minutes

---

## QC PROTOCOL — After every character

Read south.png and north.png from output.
- PASS: same character as input, silhouette readable, no black frames
- FAIL: different character, black frame, completely different outfit
- On FAIL: stop and report to Claude

---

## SHARED ROTATION CALL (apply to all 4 characters)

```bash
python - << 'EOF'
import base64, json, time, urllib.request, os

TOKEN = "037c442d-d3cf-4f38-83a9-707e05dc62b0"
INPUT_PATH = "{INPUT_SOUTH_PATH}"
OUTPUT_DIR = "{OUTPUT_DIR}"

# Read and encode image
with open(INPUT_PATH, "rb") as f:
    b64 = base64.b64encode(f.read()).decode()

# generate-8-rotations-v2: reference_image = {type, base64, format} — raw base64, no data: prefix
payload = json.dumps({
    "method": "rotate_character",
    "image_size": {"width": 128, "height": 128},
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
}).encode()

req = urllib.request.Request(
    "https://api.pixellab.ai/v2/generate-8-rotations-v2",
    data=payload,
    headers={"Authorization": f"Bearer {TOKEN}", "Content-Type": "application/json"},
    method="POST"
)
with urllib.request.urlopen(req) as r:
    response = json.loads(r.read())

print("Response:", json.dumps(response, indent=2))
job_id = response.get("background_job_id")
if not job_id:
    print("ERROR: no background_job_id in response")
    exit(1)

# Poll until complete — output order per spec:
# index 0=South, 1=South-West, 2=West, 3=North-West,
# index 4=North, 5=North-East, 6=East, 7=South-East
DIR_ORDER = [
    "south", "south-west", "west", "north-west",
    "north", "north-east", "east", "south-east"
]

for attempt in range(60):
    time.sleep(10)
    req2 = urllib.request.Request(
        f"https://api.pixellab.ai/v2/background-jobs/{job_id}",
        headers={"Authorization": f"Bearer {TOKEN}"}
    )
    with urllib.request.urlopen(req2) as r:
        status = json.loads(r.read())
    print(f"Poll {attempt+1}: status={status.get('status')}")

    if status.get("status") == "completed":
        last = status.get("last_response", {})
        images = last.get("images", [])
        os.makedirs(OUTPUT_DIR, exist_ok=True)
        for i, direction in enumerate(DIR_ORDER):
            if i >= len(images):
                print(f"WARNING: missing image for {direction}")
                continue
            img_entry = images[i]
            # img_entry may be dict with 'url' or 'base64', or plain string
            if isinstance(img_entry, dict):
                img_data = img_entry.get("url") or img_entry.get("base64", "")
            else:
                img_data = img_entry
            filename = f"{direction}.png"
            if img_data.startswith("http"):
                with urllib.request.urlopen(img_data) as r:
                    data = r.read()
            else:
                # strip data URI prefix if present
                if "," in img_data:
                    img_data = img_data.split(",", 1)[1]
                data = base64.b64decode(img_data)
            with open(os.path.join(OUTPUT_DIR, filename), "wb") as f:
                f.write(data)
            print(f"Saved {filename}")
        print("DONE")
        break

    if status.get("status") == "failed":
        print("FAILED:", json.dumps(status, indent=2))
        break
else:
    print("TIMEOUT after 10 minutes")
EOF
```

Direction key → filename mapping:
```
south      → south.png
north      → north.png
east       → east.png
west       → west.png
south-east → south-east.png
south-west → south-west.png
north-east → north-east.png
north-west → north-west.png
```

---

## TASK 1 — Shadowblade

Run the script above with:
```
INPUT_PATH = "C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/shadowblade/shadowblade_S.png"
OUTPUT_DIR = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/reference"
```
QC pass: slim hooded assassin, dual daggers visible, dark purple/black.

---

## TASK 2 — Elementalist

Run the script above with:
```
INPUT_PATH = "C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/elementalist/elementalist_S.png"
OUTPUT_DIR = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/reference"
```
QC pass: female mage, staff with amber crystal visible, dark purple robe.

---

## TASK 3 — Ranger

Run the script above with:
```
INPUT_PATH = "C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/ranger/ranger_S.png"
OUTPUT_DIR = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/reference"
```
QC pass: female hunter, longbow visible, green/charcoal cloak, quiver on back.

---

## TASK 4 — Warblade

Run the script above with:
```
INPUT_PATH = "C:/Users/ydbil/OneDrive/Masaüstü/chars-rima/warblade/warblade_S.png"
OUTPUT_DIR = "F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/reference"
```
QC pass: massive male warrior, greatsword visible, dark plate armor.

---

## TASK 5 — Cleanup (her batch sonrası zorunlu)

Move completed/obsolete files to `_BACKUP`. Do not delete.

```bash
# RIMA/ root — temp files
mv "F:/Antigravity Projeler/2d roguelite/RIMA/temp_halfthrall_qc.png" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RIMA/temp_qc_frame.png" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RIMA/pixellab_ai_session" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_ANIMATION_BATCH3.md" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_PIXELLAB_ACT1_TILES.md" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_UNITY_MOB_PREFABS.md" "F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/" 2>/dev/null

# 2d roguelite/ root — done KIRO files and temp files
mv "F:/Antigravity Projeler/2d roguelite/KIRO_DEATH_ANIM_FIX.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/KIRO_MOB_ATTACKS.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/KIRO_VOID_HALFTHRALL.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/ANIMATION_PROGRESS.txt" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/ARASTIRMA_MEKANIK.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/ASEPRITE_GOREVLER.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/GELISTIRME_PLANI.md" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RelicCaster_temp" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
mv "F:/Antigravity Projeler/2d roguelite/RelicCaster_temp.zip" "F:/Antigravity Projeler/2d roguelite/_BACKUP/" 2>/dev/null
```

Report what was moved.

---

## COMPLETION LOG

Append to `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/STAGING/DONE.txt`:
```
[DONE-CHAR-ROTATIONS] {Character} — 8 directions saved | 2026-04-08
[QC-PASS/FAIL] {Character}/{direction} — "{what was seen}" | 2026-04-08
```

---

## REPORT — Fill this before saying anything to user

```
STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Task 1 Shadowblade — [result]
  - Task 2 Elementalist — [result]
  - Task 3 Ranger — [result]
  - Task 4 Warblade — [result]
  - Task 5 Cleanup — [result]

ERRORS:
  - [exact error + which task] or NONE

QC_RESULT:
  - Shadowblade — PASS / FAIL — [reason]
  - Elementalist — PASS / FAIL — [reason]
  - Ranger — PASS / FAIL — [reason]
  - Warblade — PASS / FAIL — [reason]

NEXT_SIGNAL: "rotasyonlar hazır"
```

---

## AFTER KIRO FINISHES

Say "rotasyonlar hazır" → Claude imports to Unity and writes animation batch.
