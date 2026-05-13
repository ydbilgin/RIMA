import os
from PIL import Image

def resize_img(path, out_path, max_width=1024):
    if not os.path.exists(path):
        print(f"File not found: {path}")
        return
    with Image.open(path) as img:
        img = img.convert("RGB")
        ratio = max_width / float(img.size[0])
        new_h = int((float(img.size[1]) * float(ratio)))
        img = img.resize((max_width, new_h), Image.Resampling.LANCZOS)
        img.save(out_path, "JPEG", quality=85)
        print(f"Resized and saved: {out_path} ({os.path.getsize(out_path)/1024:.1f} KB)")

# Resize PixelLab Map Screenshot
resize_img(r"C:\Users\ydbil\Downloads\map_with_tileset.png", r"f:\Antigravity Projeler\2d roguelite\RIMA\STAGING\map_with_tileset_resized.jpg")

# Resize the Twitter Reference Video Frame (just in case)
resize_img(r"f:\Antigravity Projeler\2d roguelite\RIMA\twitter_frame.png", r"f:\Antigravity Projeler\2d roguelite\RIMA\STAGING\twitter_frame_resized.jpg")
