"""
RIMA — Review Klasörü İndirme Scripti
Çalıştır: python ART/_REVIEW/download_reviews.py
Bitince ART/_REVIEW/ altındaki klasörlere south.png'leri kaydeder.
"""

import os
import time
import requests

API_KEY = "PIXELLAB_API_KEY_BURAYA"  # .env'den veya manuel gir — koda yazma
BASE    = "https://api.pixellab.ai/v1"

HEADERS = {"Authorization": f"Bearer {API_KEY}"}

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

# ─── Karakter listesi ─────────────────────────────────────────────────────────
CHARACTERS = {
    # Player Classes (96px Pro)
    "player_classes/warblade_v3":      "bbdad318-d1e4-4898-9b5a-2cd4e1ade2e9",

    # Player Referanslar (96px standard)
    "player_classes/elementalist":     "84ed4660-3f97-41f3-9a16-a206c98e68b0",
    "player_classes/shadowblade":      "b6c2b76d-42b7-42a2-9d86-611b6136c626",
    "player_classes/ranger":           "861aaba0-4a39-478b-8aa2-373be15a9aaa",
    "player_classes/ravager":          "0c639cc6-d95f-43fe-9b53-a5090877f147",
    "player_classes/hexer":            "2c8dd8ac-39c4-4f77-8314-bbc3692d3bf1",
    "player_classes/summoner":         "7bfdd9dd-3fe9-4085-b424-9a4c045920e6",
    "player_classes/templar":          "909800b5-e2ae-4c61-be64-4aa4c42562be",
    "player_classes/hemomancer":       "55d0c734-75b0-4d45-b97c-ac6d7200ddd3",

    # Mobs (64px standard)
    "mobs/shard_walker":               "1969b26b-04d9-4ffb-a934-ba3c551a8ccb",
    "mobs/void_thrall":                "892cf781-917f-4d23-aa1f-39409c68261b",
    "mobs/echo_hound":                 "a78c3424-5ad1-419f-9193-8c242fd1562d",

    # Bosses (128px standard)
    "bosses/iron_warden":              "6cfd7015-1ee9-49d9-87ee-8551ef79e7b9",
    "bosses/fractured_king":           "bb98ea65-473c-4459-b740-7037936b9682",
}

def get_character(char_id):
    r = requests.get(f"{BASE}/characters/{char_id}", headers=HEADERS)
    r.raise_for_status()
    return r.json()

def download_image(url, path):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    r = requests.get(url, stream=True)
    r.raise_for_status()
    with open(path, "wb") as f:
        for chunk in r.iter_content(8192):
            f.write(chunk)

def main():
    print(f"RIMA Review İndirme — {len(CHARACTERS)} karakter\n")
    remaining = dict(CHARACTERS)
    done = {}

    while remaining:
        still_waiting = {}
        for name, char_id in remaining.items():
            try:
                data = get_character(char_id)
                # Pending jobs varsa bekle
                if data.get("pending_jobs"):
                    still_waiting[name] = char_id
                    print(f"  ⏳ {name} — işleniyor...")
                    continue

                # Rotasyonlardan south'u bul
                rotations = data.get("rotation_images") or data.get("rotations", [])
                south_url = None

                if isinstance(rotations, list):
                    for r in rotations:
                        if isinstance(r, dict) and r.get("direction") == "south":
                            south_url = r.get("url") or r.get("image_url")
                            break
                elif isinstance(rotations, dict):
                    south_url = rotations.get("south")

                if not south_url:
                    print(f"  ⚠️  {name} — south URL bulunamadı, tüm data: {list(data.keys())}")
                    done[name] = "no_url"
                    continue

                out_path = os.path.join(SCRIPT_DIR, name, "south.png")
                download_image(south_url, out_path)
                print(f"  ✅ {name} → {out_path}")
                done[name] = "ok"

            except requests.HTTPError as e:
                print(f"  ❌ {name} — HTTP {e.response.status_code}")
                still_waiting[name] = char_id

        remaining = still_waiting
        if remaining:
            print(f"\n{len(remaining)} karakter bekleniyor, 15 sn sonra tekrar...\n")
            time.sleep(15)

    print(f"\nTamamlandı: {sum(1 for v in done.values() if v == 'ok')}/{len(CHARACTERS)} indirildi.")
    print(f"Klasör: {SCRIPT_DIR}")

if __name__ == "__main__":
    main()
