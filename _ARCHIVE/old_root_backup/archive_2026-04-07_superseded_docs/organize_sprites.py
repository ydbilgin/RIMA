"""
RIMA Sprite Organizer
_STAGING klasöründeki zip/png dosyalarını Unity Assets/Sprites altına yerleştirir.
Önce _BACKUP/{timestamp}/ altına yedek alır.
"""

import os
import shutil
import zipfile
import json
from datetime import datetime
from pathlib import Path

# Kök dizinler
BASE_DIR = Path(__file__).parent
STAGING_DIR = BASE_DIR / "_STAGING"
UNITY_SPRITES_DIR = BASE_DIR / "RIMA" / "Assets" / "Sprites"
BACKUP_DIR = BASE_DIR / "_BACKUP"

# Yön adı -> Unity-friendly kısa ad
DIRECTION_MAP = {
    "south": "S",
    "south-east": "SE",
    "east": "E",
    "north-east": "NE",
    "north": "N",
    "north-west": "NW",
    "west": "W",
    "south-west": "SW",
}


def log(msg):
    print(f"[RIMA Organizer] {msg}", flush=True)


def backup_staging():
    """_STAGING altındaki mevcut içeriği _BACKUP/{timestamp}/ ye kopyala."""
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    backup_path = BACKUP_DIR / timestamp
    if STAGING_DIR.exists():
        shutil.copytree(STAGING_DIR, backup_path, dirs_exist_ok=False)
        log(f"Yedek alındı: {backup_path}")
    else:
        log("_STAGING bulunamadı, yedek atlandı.")
    return backup_path


def ensure_dir(path: Path):
    path.mkdir(parents=True, exist_ok=True)


def organize_character_sprites():
    """_STAGING/Characters/Players/{Name}/sprites.zip -> Assets/Sprites/Characters/{Name}/"""
    chars_dir = STAGING_DIR / "Characters" / "Players"
    if not chars_dir.exists():
        log("Characters/Players klasörü yok, atlandı.")
        return

    for char_folder in chars_dir.iterdir():
        if not char_folder.is_dir():
            continue
        zip_path = char_folder / "sprites.zip"
        if not zip_path.exists():
            log(f"  {char_folder.name}: sprites.zip yok, atlandı.")
            continue

        char_name = char_folder.name
        dest_dir = UNITY_SPRITES_DIR / "Characters" / char_name
        ensure_dir(dest_dir)

        with zipfile.ZipFile(zip_path, 'r') as zf:
            # metadata.json'dan resmi adı al (fallback: klasör adı)
            if "metadata.json" in zf.namelist():
                meta = json.loads(zf.read("metadata.json"))
                char_name = meta.get("character", {}).get("name", char_name)

            extracted = 0
            for member in zf.namelist():
                p = Path(member)
                if p.parts[0] == "rotations" and p.suffix == ".png":
                    direction_raw = p.stem  # e.g. "south-east"
                    direction_short = DIRECTION_MAP.get(direction_raw, direction_raw.upper())
                    out_name = f"{char_name}_{direction_short}.png"
                    out_path = dest_dir / out_name
                    data = zf.read(member)
                    out_path.write_bytes(data)
                    extracted += 1

        log(f"  {char_name}: {extracted} yön sprite -> {dest_dir}")


def organize_enemy_sprites():
    """_STAGING/Enemies/Act{n}/{Name}/sprites.zip -> Assets/Sprites/Enemies/{Name}/"""
    enemies_base = STAGING_DIR / "Enemies"
    if not enemies_base.exists():
        log("Enemies klasörü yok, atlandı.")
        return

    for act_folder in enemies_base.iterdir():
        if not act_folder.is_dir():
            continue
        for enemy_folder in act_folder.iterdir():
            if not enemy_folder.is_dir():
                continue
            # TwiceBorn gibi alt klasörlü düşmanlar: Primary / Secondary
            sub_zips = list(enemy_folder.rglob("sprites.zip"))
            for zip_path in sub_zips:
                # Klasör hiyerarşisinden isim oluştur: EnemyName[_Primary]
                rel = zip_path.relative_to(enemy_folder)
                parts = list(rel.parts[:-1])  # sprites.zip öncesindeki parçalar
                if parts:
                    sub_name = f"{enemy_folder.name}_{'_'.join(parts)}"
                else:
                    sub_name = enemy_folder.name

                dest_dir = UNITY_SPRITES_DIR / "Enemies" / sub_name
                ensure_dir(dest_dir)

                with zipfile.ZipFile(zip_path, 'r') as zf:
                    if "metadata.json" in zf.namelist():
                        meta = json.loads(zf.read("metadata.json"))
                        raw_name = meta.get("character", {}).get("name", sub_name)
                        # TwiceBorn Primary gibi durumlarda sub-suffix ekle
                        if parts:
                            display_name = f"{raw_name}_{'_'.join(parts)}"
                        else:
                            display_name = raw_name
                    else:
                        display_name = sub_name

                    extracted = 0
                    for member in zf.namelist():
                        p = Path(member)
                        if p.parts[0] == "rotations" and p.suffix == ".png":
                            direction_raw = p.stem
                            direction_short = DIRECTION_MAP.get(direction_raw, direction_raw.upper())
                            out_name = f"{display_name}_{direction_short}.png"
                            out_path = dest_dir / out_name
                            data = zf.read(member)
                            out_path.write_bytes(data)
                            extracted += 1

                log(f"  {display_name}: {extracted} yön sprite -> {dest_dir}")


def organize_skill_icons():
    """_STAGING/Icons/Skills/{Class}/{SkillName}.png -> Assets/Sprites/UI/Icons/Icon_{Class}_{SkillName}.png"""
    icons_dir = STAGING_DIR / "Icons" / "Skills"
    if not icons_dir.exists():
        log("Icons/Skills klasörü yok, atlandı.")
        return

    dest_dir = UNITY_SPRITES_DIR / "UI" / "Icons"
    ensure_dir(dest_dir)

    copied = 0
    for class_folder in icons_dir.iterdir():
        if not class_folder.is_dir():
            continue
        for icon_file in class_folder.glob("*.png"):
            out_name = f"Icon_{class_folder.name}_{icon_file.name}"
            shutil.copy2(icon_file, dest_dir / out_name)
            copied += 1

    log(f"Skill ikonları: {copied} PNG -> {dest_dir}")


def organize_tiles():
    """_STAGING/Tiles/{Act}/{filename}.png -> Assets/Sprites/Tiles/{filename}.png"""
    tiles_dir = STAGING_DIR / "Tiles"
    if not tiles_dir.exists():
        log("Tiles klasörü yok, atlandı.")
        return

    dest_dir = UNITY_SPRITES_DIR / "Tiles"
    ensure_dir(dest_dir)

    copied = 0
    for tile_file in tiles_dir.rglob("*.png"):
        # Dosya adı çakışmasını önlemek için alt klasör adını prefix yap
        rel = tile_file.relative_to(tiles_dir)
        parts = list(rel.parts)
        if len(parts) > 1:
            prefix = "_".join(parts[:-1]) + "_"
        else:
            prefix = ""
        out_name = prefix + tile_file.name
        shutil.copy2(tile_file, dest_dir / out_name)
        copied += 1

    log(f"Tile'lar: {copied} PNG -> {dest_dir}")


def organize_animation_sprites():
    """
    _STAGING içindeki animasyon zip'lerini çıkar.
    Örn: _STAGING/Enemies/Act1/ShardWalker/animations/idle/shardwalker_idle.zip
         -> Assets/Sprites/Enemies/ShardWalker/animations/idle/
    """
    anim_zips = list(STAGING_DIR.rglob("animations/**/*.zip"))
    if not anim_zips:
        log("Animasyon zip'i bulunamadı, atlandı.")
        return

    for zip_path in anim_zips:
        # zip yolundan Unity hedefini ters mühendislikle bul
        # _STAGING/Characters/Players/Warblade/animations/idle/warblade_idle.zip
        # veya _STAGING/Enemies/Act1/ShardWalker/animations/idle/shardwalker_idle.zip
        try:
            rel = zip_path.relative_to(STAGING_DIR)
        except ValueError:
            continue

        parts = list(rel.parts)
        anim_idx = parts.index("animations") if "animations" in parts else -1
        if anim_idx < 0:
            continue

        # Üst entity klasörünü belirle
        entity_parts = parts[:anim_idx]
        # Characters/Players/Warblade -> Characters/Warblade
        # Enemies/Act1/ShardWalker -> Enemies/ShardWalker
        if entity_parts[0] == "Characters":
            entity_type = "Characters"
            entity_name = entity_parts[-1]
        elif entity_parts[0] == "Enemies":
            entity_type = "Enemies"
            entity_name = entity_parts[-1]
        else:
            entity_type = entity_parts[0]
            entity_name = entity_parts[-1]

        anim_sub = Path(*parts[anim_idx:anim_idx + 2]) if len(parts) > anim_idx + 1 else Path("animations")
        dest_dir = UNITY_SPRITES_DIR / entity_type / entity_name / anim_sub
        ensure_dir(dest_dir)

        with zipfile.ZipFile(zip_path, 'r') as zf:
            extracted = 0
            for member in zf.namelist():
                if Path(member).suffix in (".png", ".json"):
                    data = zf.read(member)
                    out_path = dest_dir / member
                    out_path.parent.mkdir(parents=True, exist_ok=True)
                    out_path.write_bytes(data)
                    extracted += 1

        log(f"  Animasyon: {zip_path.name} -> {dest_dir} ({extracted} dosya)")


def cleanup_zips():
    """Organize edilen sprites.zip dosyalarını sil."""
    zips = list(STAGING_DIR.rglob("sprites.zip"))
    anim_zips = list(STAGING_DIR.rglob("animations/**/*.zip"))
    all_zips = zips + anim_zips
    for z in all_zips:
        z.unlink()
        log(f"  Silindi: {z}")
    log(f"Toplam {len(all_zips)} zip temizlendi.")


def main():
    log("=== RIMA Sprite Organizer başlıyor ===")
    log(f"  BASE  : {BASE_DIR}")
    log(f"  STAGING: {STAGING_DIR}")
    log(f"  UNITY  : {UNITY_SPRITES_DIR}")
    print()

    log("1/6 Yedek alınıyor...")
    backup_staging()
    print()

    log("2/6 Karakter sprite'ları...")
    organize_character_sprites()
    print()

    log("3/6 Düşman sprite'ları...")
    organize_enemy_sprites()
    print()

    log("4/6 Skill ikonları...")
    organize_skill_icons()
    print()

    log("5/6 Tile'lar...")
    organize_tiles()
    print()

    log("6/6 Animasyon zip'leri...")
    organize_animation_sprites()
    print()

    log("Zip'ler temizleniyor...")
    cleanup_zips()
    print()

    log("=== Tamamlandı! ===")
    log(f"Unity Asset Database'i yenilemek için Unity Editor'de Ctrl+R yapın.")


if __name__ == "__main__":
    main()
