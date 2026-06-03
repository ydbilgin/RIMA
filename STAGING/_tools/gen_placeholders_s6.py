# -*- coding: utf-8 -*-
"""
RIMA S6 — Temporary placeholder generator (procedural, zero AI cost).
Produces brand-consistent drop-in placeholders at the EXACT sizes/paths from
STAGING/IMAGEGEN_PACK_S6.md so the self-building demo UI wires up NOW.
All of these are meant to be SWAPPED later by PixelLab/Codex image-gen.

Brand: cyan rift #00FFCC (seal energy) over deep-purple #3A1A4A -> black void;
slate stone structure; tarnished gold #F2BC3D = reward-only.

Run:  python STAGING/_tools/gen_placeholders_s6.py
"""
import os, math, random
import numpy as np
from PIL import Image, ImageDraw, ImageFilter, ImageFont

random.seed(6); np.random.seed(6)

ROOT = r"F:\Antigravity Projeler\2d roguelite\RIMA"
UI   = os.path.join(ROOT, "Assets", "Resources", "UI", "RIMA")
MOB  = os.path.join(ROOT, "Assets", "Sprites", "Mobs", "_Placeholders")
os.makedirs(UI, exist_ok=True)
os.makedirs(MOB, exist_ok=True)

CYAN   = (0, 255, 204)
CYAN_D = (0, 150, 122)
PURPLE = (58, 26, 74)
VOID   = (8, 6, 14)
SLATE  = (96, 102, 116)
SLATE_D= (44, 48, 58)
GOLD   = (242, 188, 61)
EMBER  = (255, 150, 70)
RED    = (200, 40, 50)
TEAL   = (51, 153, 153)
VIOLET = (128, 51, 160)
GREY   = (140, 140, 150)

count = 0
def save(img, name, folder=UI):
    global count
    p = os.path.join(folder, name)
    img.save(p)
    count += 1
    print(f"  [{count:2d}] {os.path.relpath(p, ROOT)}  {img.size[0]}x{img.size[1]}")

# ---------- numpy helpers ----------
def new_rgba(w, h, rgba=(0,0,0,0)):
    a = np.zeros((h, w, 4), dtype=np.float32)
    a[...] = rgba
    return a

def to_img(arr):
    return Image.fromarray(np.clip(arr, 0, 255).astype(np.uint8), "RGBA")

def radial_field(w, h, cx, cy, r):
    yy, xx = np.mgrid[0:h, 0:w].astype(np.float32)
    d = np.sqrt((xx-cx)**2 + (yy-cy)**2) / max(r, 1e-3)
    return np.clip(d, 0, 1)  # 0 at center -> 1 at radius

def lerp_rgba(inner, outer, t):
    """t: HxW field 0..1 -> HxWx4 from inner(at 0) to outer(at 1)."""
    h, w = t.shape
    out = np.zeros((h, w, 4), dtype=np.float32)
    for i in range(4):
        out[..., i] = inner[i] + (outer[i]-inner[i]) * t
    return out

def add_glow(base_img, draw_fn, blur=6, passes=1):
    """Draw onto a transparent layer, blur it, composite additively-ish over base."""
    layer = Image.new("RGBA", base_img.size, (0,0,0,0))
    d = ImageDraw.Draw(layer)
    draw_fn(d)
    for _ in range(passes):
        layer = layer.filter(ImageFilter.GaussianBlur(blur))
    return Image.alpha_composite(base_img, layer)

def jagged(x0, y0, x1, y1, segs=7, jitter=10):
    pts = []
    for i in range(segs+1):
        t = i/segs
        x = x0 + (x1-x0)*t + random.uniform(-jitter, jitter)
        y = y0 + (y1-y0)*t + random.uniform(-jitter, jitter)
        pts.append((x, y))
    return pts

def rift_cracks(img, anchors, glow_rgb=CYAN, width=2, glow=5):
    """Draw glowing cyan rift cracks from a list of (x0,y0,x1,y1,segs,jit)."""
    def core(d):
        for (x0,y0,x1,y1,segs,jit) in anchors:
            d.line(jagged(x0,y0,x1,y1,segs,jit), fill=glow_rgb+(255,), width=width, joint="curve")
    glowed = add_glow(img, lambda d: [d.line(jagged(a[0],a[1],a[2],a[3],a[4],a[5]),
                fill=glow_rgb+(180,), width=width+3, joint="curve") for a in anchors],
                blur=glow, passes=2)
    return add_glow(glowed, core, blur=1)

# =======================================================================
# PRIORITY A — Menu + Conversion screens
# =======================================================================
def menu_bg(w=640, h=360):
    t = radial_field(w, h, w*0.5, h*0.46, h*0.85)
    arr = lerp_rgba(PURPLE+(255,), VOID+(255,), t**1.2)
    img = to_img(arr)
    # floating keep-fragment slab (torn bottom edge)
    d = ImageDraw.Draw(img)
    cxp, top, bot = w*0.5, h*0.42, h*0.70
    span = w*0.30
    top_pts = [(cxp-span, top), (cxp+span, top)]
    bot_pts = []
    n = 14
    for i in range(n+1):
        x = cxp+span - (2*span)*(i/n)
        y = bot + random.uniform(-6, 22) + (abs(i-n/2)/n)*30
        bot_pts.append((x, y))
    poly = top_pts + bot_pts
    d.polygon(poly, fill=SLATE_D+(255,))
    d.polygon(poly, outline=(20,18,26,255))
    # top lip highlight
    d.line([(cxp-span, top),(cxp+span, top)], fill=SLATE+(255,), width=2)
    # cyan rift cracks along the slab
    img = rift_cracks(img, [
        (cxp-span*0.4, top, cxp-span*0.2, bot+10, 6, 7),
        (cxp+span*0.1, top+4, cxp+span*0.5, bot, 6, 9),
        (cxp-span*0.7, top+6, cxp-span*0.5, bot-6, 5, 6),
    ], width=2, glow=4)
    # one warm ember brazier
    img = add_glow(img, lambda dd: dd.ellipse([cxp+span*0.55-4, top-6-4, cxp+span*0.55+4, top-6+4],
                                              fill=EMBER+(255,)), blur=7, passes=2)
    # drifting dust motes
    d = ImageDraw.Draw(img)
    for _ in range(40):
        x, y = random.uniform(0,w), random.uniform(0,h)
        a = random.randint(20, 70)
        d.point((x,y), fill=(180,180,200,a))
    return img

def death_overlay(w=640, h=360):
    # transparent center -> dark edges vignette + faint cyan embers
    t = radial_field(w, h, w*0.5, h*0.5, h*0.62)
    arr = lerp_rgba((6,5,10,0), (4,3,8,235), t**1.6)
    img = to_img(arr)
    img = add_glow(img, lambda dd:[dd.ellipse([x-1,y-1,x+1,y+1], fill=CYAN+(160,))
                  for (x,y) in [(random.uniform(0,w), random.uniform(h*0.4,h)) for _ in range(18)]],
                  blur=3, passes=1)
    return img

def victory_bg(w=640, h=360):
    t = radial_field(w, h, w*0.5, h*0.5, h*0.95)
    arr = lerp_rgba(PURPLE+(255,), VOID+(255,), t**1.1)
    img = to_img(arr)
    # distant island bloom
    img = add_glow(img, lambda dd: dd.ellipse([w*0.5-30, h*0.5-18, w*0.5+30, h*0.5+18],
                                              fill=CYAN+(255,)), blur=24, passes=2)
    img = add_glow(img, lambda dd: dd.ellipse([w*0.5-10, h*0.5-7, w*0.5+10, h*0.5+7],
                                              fill=(180,255,240,255)), blur=8, passes=2)
    d = ImageDraw.Draw(img)
    d.polygon([(w*0.5-26,h*0.52),(w*0.5+26,h*0.52),(w*0.5+18,h*0.6),(w*0.5-18,h*0.6)], fill=SLATE_D+(220,))
    return img

def lowhp_vignette(w=640, h=360):
    t = radial_field(w, h, w*0.5, h*0.5, h*0.7)
    arr = lerp_rgba((RED[0],RED[1],RED[2],0), (RED[0],RED[1],RED[2],210), t**2.0)
    return to_img(arr)

def logo_glyph(w=256, h=96):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    d = ImageDraw.Draw(img)
    txt = "RIMA"
    font = None
    for fp in [r"C:\Windows\Fonts\impact.ttf", r"C:\Windows\Fonts\arialbd.ttf", r"C:\Windows\Fonts\arial.ttf"]:
        if os.path.exists(fp):
            try: font = ImageFont.truetype(fp, 64); break
            except Exception: pass
    if font is None: font = ImageFont.load_default()
    bb = d.textbbox((0,0), txt, font=font)
    tw, th = bb[2]-bb[0], bb[3]-bb[1]
    pos = ((w-tw)//2 - bb[0], (h-th)//2 - bb[1])
    # eroded stone letters
    d.text(pos, txt, font=font, fill=SLATE+(255,))
    img = add_glow(img, lambda dd: dd.text(pos, txt, font=font, fill=SLATE+(255,)), blur=1)
    # single cyan rift through the letters
    img = rift_cracks(img, [(w*0.12, h*0.62, w*0.9, h*0.40, 9, 6)], width=2, glow=4)
    return img

def pill_btn(w, h):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    d = ImageDraw.Draw(img)
    r = h//2
    d.rounded_rectangle([1,1,w-2,h-2], radius=r, fill=SLATE_D+(235,), outline=CYAN+(255,), width=2)
    img = add_glow(img, lambda dd: dd.rounded_rectangle([1,1,w-2,h-2], radius=r, outline=CYAN+(180,), width=2), blur=4)
    # steam glyph hint (left)
    d = ImageDraw.Draw(img)
    cx, cy = h*0.55, h*0.5
    d.ellipse([cx-h*0.28, cy-h*0.28, cx+h*0.28, cy+h*0.28], outline=CYAN+(255,), width=2)
    d.ellipse([cx-h*0.06, cy-h*0.06, cx+h*0.06, cy+h*0.06], fill=CYAN+(255,))
    return img

def class_silhouette(w=128, h=192):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    d = ImageDraw.Draw(img)
    cx = w//2
    # robed mage body
    d.polygon([(cx, h*0.16),(cx-w*0.30, h*0.92),(cx+w*0.30, h*0.92)], fill=(10,8,16,255))  # robe
    d.ellipse([cx-w*0.14, h*0.10, cx+w*0.14, h*0.30], fill=(10,8,16,255))                   # hood/head
    img = add_glow(img, lambda dd: dd.line([(cx-w*0.30,h*0.92),(cx,h*0.16),(cx+w*0.30,h*0.92)],
                                          fill=CYAN+(160,), width=2), blur=5, passes=2)     # cyan rim
    return img

def steam_glyph(s=24):
    img = Image.new("RGBA", (s,s), (0,0,0,0))
    d = ImageDraw.Draw(img)
    d.ellipse([2,2,s-3,s-3], outline=CYAN+(255,), width=2)
    d.ellipse([s*0.42,s*0.42,s*0.7,s*0.7], fill=CYAN+(255,))
    d.line([s*0.55,s*0.55, s*0.85,s*0.3], fill=CYAN+(255,), width=2)
    return img

# =======================================================================
# PRIORITY B — HUD + Draft frames
# =======================================================================
def stone_frame(w, h, border, corner_cyan=True, fill_alpha=0):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    d = ImageDraw.Draw(img)
    # outer border ring, transparent center
    d.rectangle([0,0,w-1,h-1], outline=SLATE+(255,), width=border)
    d.rectangle([border-1,border-1,w-border,h-border], outline=SLATE_D+(255,), width=1)
    if fill_alpha:
        d.rectangle([border,border,w-border-1,h-border-1], fill=(20,18,26,fill_alpha))
    if corner_cyan:
        for (cx,cy) in [(border,border),(w-border,border),(border,h-border),(w-border,h-border)]:
            img = add_glow(img, lambda dd,cx=cx,cy=cy: dd.line([(cx-4,cy),(cx+4,cy+3)], fill=CYAN+(220,), width=1), blur=2)
    return img

def hex_mask(s=32, color=(255,255,255)):
    img = Image.new("RGBA", (s,s), (0,0,0,0))
    d = ImageDraw.Draw(img)
    cx, cy, r = s/2, s/2, s/2-1
    pts = [(cx+r*math.cos(math.radians(60*i-90)), cy+r*math.sin(math.radians(60*i-90))) for i in range(6)]
    d.polygon(pts, fill=color+(255,))
    return img

def icon_frame_hex(s=64):
    img = Image.new("RGBA", (s,s), (0,0,0,0))
    d = ImageDraw.Draw(img)
    cx, cy, r = s/2, s/2, s/2-2
    pts = [(cx+r*math.cos(math.radians(60*i-90)), cy+r*math.sin(math.radians(60*i-90))) for i in range(6)]
    d.polygon(pts, outline=SLATE+(255,), width=3)
    ri = r-4
    ptsi = [(cx+ri*math.cos(math.radians(60*i-90)), cy+ri*math.sin(math.radians(60*i-90))) for i in range(6)]
    img = add_glow(img, lambda dd: dd.polygon(ptsi, outline=CYAN+(200,)), blur=2)
    return img

def rarity_glow(s=128, rgb=GREY):
    t = radial_field(s, s, s/2, s/2, s/2)
    arr = lerp_rgba(rgb+(220,), rgb+(0,), t**1.6)
    return to_img(arr)

def banner_underline(w=320, h=48):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    img = add_glow(img, lambda dd: dd.line([(w*0.05,h*0.5),(w*0.95,h*0.5)], fill=CYAN+(160,), width=2), blur=4, passes=2)
    d = ImageDraw.Draw(img)
    d.line([(w*0.1,h*0.5),(w*0.9,h*0.5)], fill=CYAN+(220,), width=1)
    return img

def boss_skull(s=32):
    img = Image.new("RGBA", (s,s), (0,0,0,0))
    d = ImageDraw.Draw(img)
    d.ellipse([s*0.2,s*0.15,s*0.8,s*0.65], outline=CYAN+(255,), width=2)        # cranium
    d.rectangle([s*0.38,s*0.6,s*0.62,s*0.85], outline=CYAN+(255,), width=2)     # jaw
    d.ellipse([s*0.3,s*0.32,s*0.45,s*0.47], fill=CYAN+(255,))                   # eye
    d.ellipse([s*0.55,s*0.32,s*0.7,s*0.47], fill=CYAN+(255,))                   # eye
    return img

# =======================================================================
# PRIORITY C — Particles / caps
# =======================================================================
def soft_dot(s, rgb, edge_alpha=0):
    t = radial_field(s, s, s/2, s/2, s/2)
    arr = lerp_rgba(rgb+(255,), rgb+(edge_alpha,), t**1.4)
    return to_img(arr)

def card_flash(w=160, h=224):
    img = Image.new("RGBA", (w,h), (0,0,0,0))
    d = ImageDraw.Draw(img)
    d.rounded_rectangle([4,4,w-5,h-5], radius=10, fill=(255,255,255,235))
    return img.filter(ImageFilter.GaussianBlur(3))

# =======================================================================
# Boss placeholder (PenitentSovereign — no sprite yet)
# =======================================================================
def boss_placeholder(s=192):
    img = Image.new("RGBA", (s,s), (0,0,0,0))
    d = ImageDraw.Draw(img)
    cx = s/2
    # tall chained robed silhouette
    d.polygon([(cx, s*0.10),(cx-s*0.26, s*0.94),(cx+s*0.26, s*0.94)], fill=(12,9,18,255))
    d.ellipse([cx-s*0.13, s*0.07, cx+s*0.13, s*0.27], fill=(12,9,18,255))  # hooded head
    # chains hint (cyan dashed arcs from arms)
    for sx in (-1, 1):
        for k in range(5):
            x = cx + sx*(s*0.18 + k*s*0.04)
            y = s*0.45 + k*s*0.07
            d.ellipse([x-2,y-2,x+2,y+2], outline=CYAN+(200,), width=1)
    img = add_glow(img, lambda dd: dd.line([(cx-s*0.26,s*0.94),(cx,s*0.10),(cx+s*0.26,s*0.94)],
                                          fill=CYAN+(150,), width=2), blur=6, passes=2)  # rim
    # cyan rift heart
    img = add_glow(img, lambda dd: dd.ellipse([cx-5,s*0.5-5,cx+5,s*0.5+5], fill=CYAN+(255,)), blur=6, passes=2)
    return img

# =======================================================================
# RUN
# =======================================================================
print("PRIORITY A — Menu + Conversion screens")
save(menu_bg(640,360), "menu_bg_island.png")
save(menu_bg(1280,720), "menu_bg_island@2x.png")
save(logo_glyph(256,96), "logo_rima_glyph.png")
save(death_overlay(640,360), "death_overlay.png")
save(pill_btn(256,48), "wishlist_cta_btn.png")
save(pill_btn(320,64), "wishlist_cta_btn_lg.png")
save(victory_bg(640,360), "victory_bg_bloom.png")
save(class_silhouette(128,192), "next_class_silhouette.png")
save(steam_glyph(24), "steam_glyph_cyan.png")

print("PRIORITY B — HUD + Draft frames")
save(stone_frame(80,80,4), "minimap_frame_stone.png")
save(hex_mask(32), "hex_slot_mask.png")
save(lowhp_vignette(640,360), "lowhp_vignette.png")
save(stone_frame(160,224,5, fill_alpha=40), "card_frame_stone.png")
save(rarity_glow(128, GREY),   "rarity_glow_common.png")
save(rarity_glow(128, TEAL),   "rarity_glow_rare.png")
save(rarity_glow(128, VIOLET), "rarity_glow_epic.png")
save(rarity_glow(128, (204,166,38)), "rarity_glow_legendary.png")
save(icon_frame_hex(64), "icon_frame_hex.png")
save(banner_underline(320,48), "banner_underline_rune.png")
save(boss_skull(32), "boss_skull_glyph.png")

print("PRIORITY C — Particles / caps")
save(soft_dot(8, CYAN),  "particle_ember.png")
save(soft_dot(4, GREY),  "dust_mote.png")
save(card_flash(160,224), "card_select_flash.png")

print("BOSS placeholder")
save(boss_placeholder(192), "PenitentSovereign_placeholder.png", folder=MOB)

print(f"\nDONE — {count} placeholder PNGs written.")
