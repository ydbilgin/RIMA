import os
import requests
import zipfile
import io
from PIL import Image
import shutil
import time
import sys

BASE_OUTPUT_DIR = "F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/antigravity"
INDEX_FILE = os.path.join(BASE_OUTPUT_DIR, "_INDEX.md")

os.makedirs(BASE_OUTPUT_DIR, exist_ok=True)

stats = {
    "total_pngs": 0,
    "packs": [],
    "licenses": {"CC0": 0, "CC-BY": 0, "Free personal": 0, "Other": 0},
    "skipped": []
}

HEADERS = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
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

def write_source_md(pack_dir, pack_name, url, author, license_type, attribution, file_count, notes):
    content = f"# {pack_name}\n\n- URL: {url}\n- Author: {author}\n- License: {license_type}\n- Attribution required: {attribution}\n- Download date: 2026-05-17\n- File count: {file_count}\n- Notes: {notes}\n"
    with open(os.path.join(pack_dir, "SOURCE.md"), "w", encoding="utf-8") as f:
        f.write(content)

def process_pack(pack_name, url, author, license_type, zip_url_or_content, is_url=True):
    print(f"Processing ZIP pack: {pack_name}")
    sys.stdout.flush()
    pack_dir = os.path.join(BASE_OUTPUT_DIR, pack_name)
    os.makedirs(pack_dir, exist_ok=True)
    
    file_count = 0
    try:
        if is_url:
            print(f"  Downloading ZIP from: {zip_url_or_content}")
            sys.stdout.flush()
            r = requests.get(zip_url_or_content, headers=HEADERS, timeout=20)
            r.raise_for_status()
            content = r.content
        else:
            content = zip_url_or_content
            
        print("  Extracting ZIP...")
        sys.stdout.flush()
        with zipfile.ZipFile(io.BytesIO(content)) as z:
            temp_dir = os.path.join(pack_dir, "temp_extract")
            os.makedirs(temp_dir, exist_ok=True)
            z.extractall(temp_dir)
            
            for root, _, files in os.walk(temp_dir):
                for file in files:
                    if file.lower().endswith('.png'):
                        src_path = os.path.join(root, file)
                        if is_valid_image(src_path):
                            dst_path = os.path.join(pack_dir, f"{file_count:04d}_{file}")
                            shutil.copy2(src_path, dst_path)
                            file_count += 1
            
            shutil.rmtree(temp_dir)
            print(f"  Extracted {file_count} valid tiles.")
            sys.stdout.flush()
            
    except Exception as e:
        print(f"  Failed to process {pack_name}: {e}")
        sys.stdout.flush()
        stats["skipped"].append(f"{pack_name} (Error: {e})")
        return

    record_pack(pack_name, url, author, license_type, pack_dir, file_count)

def record_pack(pack_name, url, author, license_type, pack_dir, file_count):
    if file_count > 0:
        att = "yes" if license_type != "CC0" else "no"
        write_source_md(pack_dir, pack_name, url, author, license_type, att, file_count, "Automated download")
        stats["total_pngs"] += file_count
        stats["packs"].append({
            "name": pack_name,
            "files": file_count,
            "license": license_type,
            "notes": "Automated download"
        })
        if license_type in stats["licenses"]:
            stats["licenses"][license_type] += file_count
        else:
            stats["licenses"]["Other"] += file_count
    else:
        try:
            shutil.rmtree(pack_dir)
        except:
            pass
        stats["skipped"].append(f"{pack_name} (No valid 16-256px PNGs found)")

def fetch_dynamic_github_repos():
    print("Searching GitHub for pixel art tilesets...")
    sys.stdout.flush()
    # Broaden query to ensure we get results
    search_url = "https://api.github.com/search/repositories?q=tileset+pixel&sort=stars&order=desc"
    try:
        r = requests.get(search_url, headers=HEADERS, timeout=15)
        r.raise_for_status()
        data = r.json()
        items = data.get("items", [])[:8] # Get top 8 repos
        
        if not items:
            print("No repos found. Falling back to known repos...")
            sys.stdout.flush()
            # fallback
            process_pack("github_0x72_DungeonTileset-II", "https://github.com/0x72/DungeonTileset-II", "0x72", "CC0", "https://github.com/0x72/DungeonTileset-II/archive/refs/heads/master.zip")
            return
            
        for item in items:
            repo_full_name = item["full_name"]
            repo_owner = item["owner"]["login"]
            repo_name = item["name"]
            default_branch = item["default_branch"]
            license_info = item.get("license")
            
            license_type = "Other"
            if license_info:
                key = license_info.get("key", "").lower()
                if "cc0" in key or "mit" in key:
                    license_type = "CC0" if "cc0" in key else "Other/MIT"
            
            url = item["html_url"]
            zip_url = f"https://github.com/{repo_full_name}/archive/refs/heads/{default_branch}.zip"
            pack_name = f"github_{repo_owner}_{repo_name}"
            
            process_pack(pack_name, url, repo_owner, license_type, zip_url)
            time.sleep(1) # Be nice to GitHub API
            
    except Exception as e:
        print(f"Error fetching from GitHub API: {e}")
        sys.stdout.flush()
        stats["skipped"].append(f"GitHub API Search (Error: {e})")

def main():
    print("Starting collection...")
    sys.stdout.flush()
    
    # Due to OpenGameArt returning 502 Bad Gateway consistently, we are using stable GitHub repos
    stats["skipped"].append("OpenGameArt sources (Skipped due to persistent 502 Bad Gateway server errors)")
    stats["skipped"].append("itch.io sources (Skipped due to anti-bot protection blocking automated curl/requests)")
    
    fetch_dynamic_github_repos()
        
    print("Generating _INDEX.md...")
    index_content = f"# Reference Collection — Summary\n\n- Total PNG files: {stats['total_pngs']}\n- Source pack count: {len(stats['packs'])}\n- License breakdown:\n  - CC0: {stats['licenses']['CC0']} files\n  - CC-BY: {stats['licenses']['CC-BY']} files\n  - Free personal: {stats['licenses']['Free personal']} files\n  - Other/MIT: {stats['licenses']['Other']} files\n- Pack-level breakdown:\n| Pack | Files | License | Style notes |\n|---|---|---|---|\n"
    
    for p in stats['packs']:
        index_content += f"| {p['name']} | {p['files']} | {p['license']} | {p['notes']} |\n"
        
    index_content += f"\n- Issues / skipped sources: {', '.join(stats['skipped']) if stats['skipped'] else 'None'}\n"
    
    with open(INDEX_FILE, "w", encoding="utf-8") as f:
        f.write(index_content)
        
    print(f"Done. Collected {stats['total_pngs']} images.")

if __name__ == "__main__":
    main()
