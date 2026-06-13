"""RIMA Create UI kit blueprints — wireframe mockups for manual placement reference."""
from PIL import Image, ImageDraw, ImageFont
import os

OUT = os.path.join(os.path.dirname(__file__), "runmap_kit_blueprints")
os.makedirs(OUT, exist_ok=True)
S = 2  # upscale for readability

BG      = (20, 18, 26)      # dark canvas
GRID    = (40, 38, 50)
SHAPE   = (79, 224, 200)    # cyan outline (like the tool)
FILL    = (58, 26, 74, 90)  # void purple translucent
TXT     = (240, 240, 235)
SUB     = (150, 200, 190)

def font(sz):
    for p in ["C:/Windows/Fonts/consola.ttf", "C:/Windows/Fonts/arial.ttf"]:
        if os.path.exists(p):
            return ImageFont.truetype(p, sz)
    return ImageFont.load_default()

def draw_kit(name, output_size, shapes):
    W, H = [int(x) for x in output_size.split("x")]
    img = Image.new("RGBA", (W*S, H*S), BG)
    d = ImageDraw.Draw(img, "RGBA")
    # grid every 32px
    for gx in range(0, W+1, 32):
        d.line([(gx*S, 0), (gx*S, H*S)], fill=GRID, width=1)
    for gy in range(0, H+1, 32):
        d.line([(0, gy*S), (W*S, gy*S)], fill=GRID, width=1)
    # title
    d.text((8, 6), f"{name}   canvas {W}x{H}", font=font(18), fill=SHAPE)
    # shapes
    for s in shapes:
        x, y = s["pos"]; w, h, r = s["w"], s["h"], s["r"]
        box = [x*S, y*S, (x+w)*S, (y+h)*S]
        d.rounded_rectangle(box, radius=max(1, r*S), outline=SHAPE, width=2, fill=FILL)
        cx, cy = (x + w/2)*S, (y + h/2)*S
        f1, f2 = font(15), font(12)
        l1 = s["type"]; l2 = s["role"]; l3 = f"{w}x{h}  r{r}"
        # stack 3 lines centered
        for i, (t, ff, col) in enumerate([(l1, f1, TXT), (l2, f2, SUB), (l3, f2, SUB)]):
            tw = d.textlength(t, font=ff)
            d.text((cx - tw/2, cy - 20 + i*16), t, font=ff, fill=col)
    p = os.path.join(OUT, f"{name}.png")
    img.save(p)
    print("saved", p)

KITS = {
 "KIT_A_Frames": ("512x512", [
    {"type":"Window","role":"minimap_frame","w":288,"h":208,"r":8,"pos":(16,16)},
    {"type":"Panel","role":"node_frame","w":112,"h":112,"r":8,"pos":(336,16)},
    {"type":"Panel","role":"tooltip_box","w":168,"h":120,"r":8,"pos":(320,160)},
    {"type":"Panel","role":"reward_card","w":176,"h":232,"r":8,"pos":(16,256)},
 ]),
 "KIT_B1_Bars": ("688x384", [
    {"type":"Panel","role":"boss_hp_bar","w":420,"h":30,"r":6,"pos":(16,24)},
    {"type":"Panel","role":"player_hp_bar","w":300,"h":24,"r":6,"pos":(16,90)},
    {"type":"Panel","role":"resource_bar","w":300,"h":18,"r":6,"pos":(16,150)},
    {"type":"Panel","role":"xp_bar","w":360,"h":14,"r":4,"pos":(16,205)},
 ]),
 "KIT_B2_Slots": ("512x512", [
    {"type":"Icon button","role":"slot_normal","w":80,"h":80,"r":8,"pos":(16,16)},
    {"type":"Icon button","role":"slot_active","w":80,"h":80,"r":8,"pos":(112,16)},
    {"type":"Icon button","role":"slot_lmb_rmb","w":96,"h":96,"r":8,"pos":(208,16)},
    {"type":"Button","role":"ribbon_base","w":180,"h":44,"r":6,"pos":(16,140)},
    {"type":"Button","role":"menu_button","w":240,"h":52,"r":8,"pos":(16,220)},
 ]),
 "KIT_C_Menu": ("512x512", [
    {"type":"Tab","role":"codex_tab_normal","w":200,"h":52,"r":6,"pos":(16,16)},
    {"type":"Tab","role":"codex_tab_selected","w":200,"h":52,"r":6,"pos":(16,84)},
    {"type":"Avatar","role":"portrait_frame","w":120,"h":120,"r":0,"pos":(16,160)},
    {"type":"Button","role":"pause_button","w":300,"h":56,"r":8,"pos":(160,160)},
    {"type":"Panel","role":"pause_main_panel","w":220,"h":260,"r":8,"pos":(160,240)},
 ]),
}

for n, (sz, sh) in KITS.items():
    draw_kit(n, sz, sh)
print("done")
