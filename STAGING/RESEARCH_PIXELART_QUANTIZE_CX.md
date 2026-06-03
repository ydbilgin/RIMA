# Pixel-art Quantize Fizibilite - cx laurethayday

## VERDICT

EVET, yapilir. Yeni AI gerekmez.

Hazir proje ici tool kismen var:
- `pixelify`: PixelLab route + Pillow quantize + nearest resize yapar. HD -> pixel-art donusumu icin uygun, ama mevcut PixelLab/imagegen ciktilarina sabit RIMA paletiyle deterministik batch quantize araci degil.
- `pixel-cleanup`: stray pixel, alpha, palette outlier, snap_to_palette, make_palette, smart_merge var. Palet snap yetenegi var. Eksik olan kisim: tek net "pixelquant" akisi; sabit/curated palet dosyasi, dither policy, adaptive N-color quantize ve raporlu batch wrapper.

Dis tool var ama RIMA icin zorunlu degil:
- Pillow `Image.quantize(colors, method, palette, dither)` yeterli temel. Bu local Python akisa en temiz entegrasyon.
- Pillow local build: `Quantize.MEDIANCUT/MAXCOVERAGE/FASTOCTREE/LIBIMAGEQUANT` enum var, ama `features.check_feature("libimagequant") == False`. Yani LIBIMAGEQUANT bu ortamda aktif degil.
- `libimagequant` pip paketi var; pngquant/libimagequant kalite olarak iyi, fakat ekstra binary/binding riski getirir.
- `pngquant` ve `didder` CLI local PATH'te yok.
- `scikit-image` local kurulu, ama bu is icin agir. Color quantize/segmentation icin kullanilabilir, ilk secim degil.
- `hitherdither` pip index'te bu ortam icin bulunmadi.

## Gozlem

Olculen mevcut ciktilar:
- `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png`: 64x64, 8 opaque renk, partial alpha 0.
- `floor451_10.png`: 64x64, 25 opaque renk, partial alpha 0.
- `STAGING/imagegen/portal_reward/portal_rift.png`: 128x128, 6031 opaque renk, partial alpha 0.
- `reward_relic.png`: 128x128, 6402 opaque renk, partial alpha 0.
- runeler: 64x64, yaklasik 473-852 opaque renk.

Sonuc: PixelLab floor451 zaten palet olarak pixel-art'a yakin. Imagegen portal/reward dosyalari HD/anti-alias renk dagilimi tasiyor; asil hedef onlar.

## Onerilen Tek Pipeline

1. Input PNG oku, RGBA koru.
2. Alpha:
   - partial alpha varsa `pixel-cleanup --alpha_threshold 128` ile 0/255 yap.
   - portal glow gibi VFX icin gerekirse `--keep_alpha`.
3. Palet:
   - Default: RIMA curated 32 renk paleti.
   - 8-bit hedef: 16 veya 32 renk.
   - 16-bit hedef: 128 veya 256 renk, ama RIMA icin 32-64 daha tutarli.
4. Quantize:
   - Sabit palet varsa: Pillow palette image + `quantize(palette=..., dither=Image.Dither.NONE)`.
   - Sabit palet yoksa: `quantize(colors=N, method=MEDIANCUT veya MAXCOVERAGE, dither=NONE)`.
   - RGBA icin alpha ayri korunur; RGB quantize edilir, alpha geri konur.
5. Palette snap:
   - RIMA paletine en yakin renge snap.
   - Mevcut `pixel-cleanup --snap_to_palette` bunun cekirdegi.
6. Dither:
   - Default KAPALI.
   - Tile/sprite icin Floyd-Steinberg kapali olmali; nokta noise ve HD hissi uretiyor.
   - Ordered/blue-noise sadece buyuk gradient VFX'te opsiyonel, dusuk kuvvetle.
7. Pixel block:
   - Asset zaten 64/128 px ve Unity Point filter/PPU dogruysa downscale-upscale gerekmez.
   - HD kaynakta once hedef logical size'a `NEAREST` degil, gerekirse `LANCZOS`/area ile kucult, sonra quantize, sonra sadece preview icin `NEAREST` buyut.
   - PixelLab 64 px tile icin sadece renk quantize yeterli.

## RIMA Palet Stratejisi

RIMA-ozel palet DB16/AAP-64'ten daha iyi. Hazir paletler tutarli ama RIMA'nin cyan/void kimligini kilitlemez.

Baslangic paleti:
- near black / iron: `#05070B #0B1018 #111827 #1A2230 #263244`
- dark slate stone: `#2F3B46 #3E4B55 #55616B #6A7480`
- void purple: `#241038 #351854 #4B2471 #6A35A0`
- cyan rift: `#003D4D #006B85 #00A6B8 #00FFCC #44E5FF #7FF0FF`
- bone/gold/warm accents: `#B8A878 #E0D5B0 #B8941F #FFD700 #FF8000`
- danger: `#8B0000 #C82020`

Bu palet `Tools/pixel_cleanup/palette_examples/rima_void_cyan_32.json` gibi eklenebilir.

## PoC Script Iskeleti

Calistirma hedefi degil; mevcut tool'a gomulecek cekirdek:

```python
from pathlib import Path
from PIL import Image

def load_hex_palette(path: Path) -> list[tuple[int, int, int]]:
    colors = []
    for line in path.read_text(encoding="ascii").splitlines():
        line = line.strip()
        if not line or line.startswith("#") and len(line) != 7:
            continue
        value = line.lstrip("#")
        colors.append(tuple(int(value[i:i+2], 16) for i in (0, 2, 4)))
    return colors

def palette_image(colors: list[tuple[int, int, int]]) -> Image.Image:
    pal = Image.new("P", (1, 1))
    flat = []
    for r, g, b in colors[:256]:
        flat.extend([r, g, b])
    flat.extend([0, 0, 0] * (256 - len(colors)))
    pal.putpalette(flat)
    return pal

def quantize_png(src: Path, dst: Path, colors: int = 32, fixed_palette: Path | None = None) -> None:
    rgba = Image.open(src).convert("RGBA")
    alpha = rgba.getchannel("A")
    rgb = rgba.convert("RGB")

    if fixed_palette:
        q = rgb.quantize(palette=palette_image(load_hex_palette(fixed_palette)), dither=Image.Dither.NONE)
    else:
        q = rgb.quantize(colors=colors, method=Image.Quantize.MEDIANCUT, dither=Image.Dither.NONE)

    out = q.convert("RGBA")
    out.putalpha(alpha.point(lambda a: 255 if a >= 128 else 0))
    out.save(dst)
```

## Entegrasyon Karari

Yeni ayri `/pixelquant` skill yerine mevcut `pixel-cleanup` skill'e eklemek daha iyi:
- Zaten palette snap, report, batch, masks, smart_merge var.
- Yeni flag seti yeterli: `--quantize N`, `--fixed_palette palette.hex|json`, `--dither none|ordered|floyd`, `--alpha_binary`.
- `pixelify` HD -> PixelLab donusum skill'i olarak kalsin.
- `pixel-cleanup` ise "final deterministic post-process" olsun.

Minimum implementasyon:
1. `Tools/pixel_cleanup/pixel_quantize.py` veya `pixel_cleanup.py` icine `quantize_to_palette()`.
2. `palette_examples/rima_void_cyan_32.json`.
3. CLI: `python pixel_cleanup.py --input X.png --output X_px.png --quantize 32 --palette rima_void_cyan_32.json --dither none --apply_cleanup`.
4. Report'a `pre_unique_colors`, `post_unique_colors`, `palette_name`, `dither` ekle.

## Riskler

- Sabit palet cok agresifse karakteristik highlight kaybolur; portal/reward icin 32-64 renk daha guvenli.
- Dither acilirsa "HD noise" hissi geri gelebilir.
- Quantize tek basina silhouette/pixel readability duzeltmez; imagegen dosyasi 128 px'te zaten bulaniksa once elle/PixelLab yeniden pixel-art source almak daha iyi.

## Kisa Sonuc

Tool VAR: `pixel-cleanup` ve `pixelify` cekirdek yetenekleri sagliyor.

Eksik: RIMA palet kilitli, dither default off, raporlu quantize wrapper.

Yapilacak is kucuk: Pillow + mevcut `pixel-cleanup` uzerine 1 modul/flag seti. En temiz sonuc icin default `fixed RIMA palette + Dither.NONE + alpha binary + optional snap_to_palette`.
