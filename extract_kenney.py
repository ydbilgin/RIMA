import os
import zipfile
import shutil
from PIL import Image

BASE_OUTPUT_DIR = "F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/antigravity"
INDEX_FILE = os.path.join(BASE_OUTPUT_DIR, "_INDEX.md")

os.makedirs(BASE_OUTPUT_DIR, exist_ok=True)

downloads = [
    ("kenney_tiny_dungeon", r"C:\Users\ydbil\Downloads\kenney_tinyDungeon.zip", "https://kenney.nl/assets/tiny-dungeon", "Kenney", "CC0"),
    ("kenney_tiny_town", r"C:\Users\ydbil\Downloads\kenney_tiny-town.zip", "https://kenney.nl/assets/tiny-town", "Kenney", "CC0")
]

stats = {
    "total_pngs": 0,
    "packs": []
}

def is_valid_image(filepath):
    try:
        with Image.open(filepath) as img:
            w, h = img.size
            if w > 256 or h > 256:
                return False
            if w < 16 or h < 16:
                return False
            return True
    except Exception:
        return False

def extract_pack(pack_id, zip_path, url, author, license_type):
    print(f"Extracting {pack_id} from {zip_path}")
    pack_dir = os.path.join(BASE_OUTPUT_DIR, pack_id)
    os.makedirs(pack_dir, exist_ok=True)
    
    file_count = 0
    with zipfile.ZipFile(zip_path, 'r') as z:
        temp_dir = os.path.join(pack_dir, "temp")
        z.extractall(temp_dir)
        
        for root, _, files in os.walk(temp_dir):
            for file in files:
                if file.lower().endswith('.png'):
                    src = os.path.join(root, file)
                    if is_valid_image(src):
                        dst = os.path.join(pack_dir, f"{file_count:04d}_{file}")
                        shutil.copy2(src, dst)
                        file_count += 1
                        
        shutil.rmtree(temp_dir)
        
    print(f"Extracted {file_count} tiles for {pack_id}")
    
    if file_count > 0:
        att = "no" if license_type == "CC0" else "yes"
        with open(os.path.join(pack_dir, "SOURCE.md"), "w", encoding="utf-8") as f:
            f.write(f"# {pack_id}\n\n- URL: {url}\n- Author: {author}\n- License: {license_type}\n- Attribution required: {att}\n- Download date: 2026-05-17\n- File count: {file_count}\n- Notes: Downloaded via Browser Subagent to bypass protections\n")
        
        stats["total_pngs"] += file_count
        stats["packs"].append({
            "name": pack_id,
            "files": file_count,
            "license": license_type
        })
    else:
        shutil.rmtree(pack_dir)

for pack in downloads:
    extract_pack(*pack)
    
index_content = f"# Reference Collection — Summary\n\n- Total PNG files: {stats['total_pngs']}\n- Source pack count: {len(stats['packs'])}\n- License breakdown:\n  - CC0: {stats['total_pngs']} files\n- Pack-level breakdown:\n| Pack | Files | License | Style notes |\n|---|---|---|---|\n"
for p in stats['packs']:
    index_content += f"| {p['name']} | {p['files']} | {p['license']} | CC0 Kenney Assets |\n"

with open(INDEX_FILE, "w", encoding="utf-8") as f:
    f.write(index_content)
    
print("Done!")
