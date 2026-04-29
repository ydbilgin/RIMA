"""
mimicmotion_run.py — Gemini BASE gorselinden animasyon uretir.

Kullanim:
    python tools/mimicmotion_run.py --char warblade --anim walk
    python tools/mimicmotion_run.py --char grunt_shard --anim idle
    python tools/mimicmotion_run.py --all  (gece modunda tum karakterleri isler)

Referans videolar: tools/pose_refs/ klasoründe olmali
    idle.mp4   — yerinde sallanma
    walk.mp4   — yuruyus dongusu
    attack.mp4 — saldiri hareketi
    dash.mp4   — kacis hareketi
    death.mp4  — olum animasyonu
"""

import sys
import os
import subprocess
import argparse
from pathlib import Path

ROOT = Path(__file__).parent.parent
MIMIC_DIR = ROOT / "tools" / "MimicMotion"
MODELS_DIR = ROOT / "models" / "mimicmotion"
POSE_REFS = ROOT / "tools" / "pose_refs"
ART_DIR = ROOT / "ART"

CHARS = {
    "warblade":    ("karakterler/warblade/warblade_gemini_base.png", 64),
    "elementalist":("karakterler/elementalist/elementalist_gemini_base.png", 64),
    "shadowblade": ("karakterler/shadowblade/shadowblade_gemini_base.png", 64),
    "ranger":      ("karakterler/ranger/ranger_gemini_base.png", 64),
    "ravager":     ("karakterler/ravager/ravager_gemini_base.png", 64),
    "paladin":     ("karakterler/paladin/paladin_gemini_base.png", 64),
    "summoner":    ("karakterler/summoner/summoner_gemini_base.png", 64),
    "hexer":       ("karakterler/hexer/hexer_gemini_base.png", 64),
    "grunt_shard": ("dusmanlar/grunt_shard/grunt_shard_gemini_base.png", 32),
}

ANIMS = ["idle", "walk", "attack", "dash", "death"]

ANIM_FRAMES = {
    "idle":   4,
    "walk":   6,
    "attack": 6,
    "dash":   4,
    "death":  8,
}


def check_setup():
    if not MIMIC_DIR.exists():
        print("HATA: MimicMotion kurulmamis. Once calistir:")
        print("  tools\\mimicmotion_setup.bat")
        sys.exit(1)
    if not MODELS_DIR.exists():
        print("HATA: Model dosyalari eksik. mimicmotion_setup.bat'i tekrar calistir.")
        sys.exit(1)


def check_pose_refs():
    POSE_REFS.mkdir(exist_ok=True)
    missing = []
    for anim in ANIMS:
        ref = POSE_REFS / f"{anim}.mp4"
        if not ref.exists():
            missing.append(str(ref))
    if missing:
        print("UYARI: Su referans videolar eksik:")
        for m in missing:
            print(f"  {m}")
        print("\nMixamo.com veya YouTube'dan ucretsiz hareket videolari indir.")
        print("Adim adim kilavuz: ART/SPRITE_WORKFLOW.md -> Pose Referanslar bolumu")
        if len(missing) == len(ANIMS):
            sys.exit(1)


def run_mimicmotion(char_name: str, anim: str):
    char_info = CHARS.get(char_name)
    if not char_info:
        print(f"HATA: Karakter bilinmiyor: {char_name}")
        return

    img_rel, target_size = char_info
    img_path = ART_DIR / img_rel
    pose_video = POSE_REFS / f"{anim}.mp4"
    out_dir = img_path.parent

    if not img_path.exists():
        print(f"ATLA: Gorsel bulunamadi: {img_path}")
        return

    if not pose_video.exists():
        print(f"ATLA: Referans video yok: {pose_video}")
        return

    out_video = out_dir / f"{char_name}_{anim}_raw.mp4"
    out_sheet = out_dir / f"{char_name}_S_{anim}.png"

    print(f"\n[{char_name}] {anim} animasyonu uretiliyor...")

    cmd = [
        sys.executable,
        str(MIMIC_DIR / "scripts" / "inference.py"),
        "--reference_image", str(img_path),
        "--motion_video",    str(pose_video),
        "--output",          str(out_video),
        "--num_frames",      str(ANIM_FRAMES[anim]),
        "--resolution",      "512",           # yuksek coz -> asagida pixelate ederiz
        "--checkpoint",      str(MODELS_DIR),
    ]

    try:
        subprocess.run(cmd, check=True, cwd=str(MIMIC_DIR))
        print(f"  Ham video: {out_video.name}")
        video_to_spritesheet(out_video, out_sheet, target_size, ANIM_FRAMES[anim])
        print(f"  Sprite sheet: {out_sheet.name}  ({ANIM_FRAMES[anim]} frame x {target_size}px)")
    except subprocess.CalledProcessError as e:
        print(f"  HATA: {e}")


def video_to_spritesheet(video_path: Path, out_path: Path, target_size: int, frame_count: int):
    """Video'dan frame'leri al, pixelate et, sprite sheet olustur."""
    try:
        import cv2
        from PIL import Image
    except ImportError:
        print("  pip install opencv-python pillow")
        return

    cap = cv2.VideoCapture(str(video_path))
    total = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    indices = [int(i * total / frame_count) for i in range(frame_count)]

    frames = []
    for idx in indices:
        cap.set(cv2.CAP_PROP_POS_FRAMES, idx)
        ret, frame = cap.read()
        if not ret:
            continue
        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        img = Image.fromarray(rgb).convert("RGBA")
        # Pixelate: kucult, sonra orijinal boyuta buyut (opsiyonel)
        small = img.resize((target_size, target_size), Image.LANCZOS)
        frames.append(small)
    cap.release()

    if not frames:
        return

    sheet = Image.new("RGBA", (target_size * len(frames), target_size), (0, 0, 0, 0))
    for i, f in enumerate(frames):
        sheet.paste(f, (i * target_size, 0))
    sheet.save(out_path)


def main():
    parser = argparse.ArgumentParser(description="MimicMotion animasyon uretici")
    parser.add_argument("--char", help="Karakter adi (warblade, grunt_shard, ...)")
    parser.add_argument("--anim", default="idle", choices=ANIMS, help="Animasyon turu")
    parser.add_argument("--all", action="store_true", help="Tum karakterler, tum animasyonlar")
    args = parser.parse_args()

    check_setup()
    check_pose_refs()

    if args.all:
        print("GECE MODU: Tum karakterler x tum animasyonlar")
        print(f"Toplam is: {len(CHARS)} karakter x {len(ANIMS)} animasyon = {len(CHARS)*len(ANIMS)} batch\n")
        for char in CHARS:
            for anim in ANIMS:
                run_mimicmotion(char, anim)
        print("\nTUM ANIMASYONLAR TAMAMLANDI.")
    elif args.char:
        run_mimicmotion(args.char, args.anim)
    else:
        parser.print_help()


if __name__ == "__main__":
    main()
