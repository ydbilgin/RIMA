# Writes PNG slices with explicit IEND chunk and post-write verification -- do not remove.
#!/usr/bin/env python3
"""Universal tile sheet splitter with chromakey removal for RIMA dungeon tiles.

Usage:
  python STAGING/process_tiles.py --source <image> --output <dir> [options]

Examples:
  F1 floor:  python STAGING/process_tiles.py --source "path/to/f1.png" --output Assets/Art/Tiles/Act1/F1 --prefix f1_
  W1 wall:   python STAGING/process_tiles.py --source "path/to/w1.png" --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 3 --width 64 --height 96 --prefix w1_
"""
import argparse, os, sys, uuid
from io import BytesIO
from PIL import Image
import numpy as np


def _verify_iend(path):
    with open(path, "rb") as h:
        data = h.read()
    if data[-12:] != b"\x00\x00\x00\x00IEND\xaeB`\x82":
        raise RuntimeError(f"PNG missing IEND: {path}")


def _save_png_slice(image, out_path):
    buf = BytesIO()
    image.save(buf, format="PNG", optimize=True)
    data = buf.getvalue()
    if data[-12:] != b"\x00\x00\x00\x00IEND\xaeB`\x82":
        raise RuntimeError(f"PNG buffer missing IEND before write: {out_path}")
    with open(out_path, "wb") as h:
        h.write(data)


def make_meta(path, tile_w, tile_h):
    guid = uuid.uuid4().hex
    sprite_id = uuid.uuid4().hex[:22]
    pivot_y = 1.0  # top-center
    content = f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 11
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMasterTextureLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 128
  textureSettings:
    serializedVersion: 2
    filterMode: 0
    aniso: 1
    mipBias: 0
    wrapU: 1
    wrapV: 1
    wrapW: 1
  nPOTScale: 1
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 1
  spritePivot: {{x: 0.5, y: {pivot_y}}}
  spritePixelsToUnits: 16
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 3
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 128
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 2
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    spriteID: {sprite_id}
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {{}}
  spritePackingTag:
  pSDRemoveMatte: 0
  userData:
  assetBundleName:
  assetBundleVariant:
"""
    with open(path + ".meta", "w", newline="\n") as f:
        f.write(content)


def process(source, output_dir, cols, rows, out_w, out_h, prefix):
    if not os.path.exists(source):
        print(f"ERROR: source not found: {source}", file=sys.stderr)
        sys.exit(1)
    os.makedirs(output_dir, exist_ok=True)

    img = Image.open(source).convert("RGBA")
    data = np.array(img)
    # Chromakey: magenta #FF00FF ±30 tolerance
    r, g, b = data[:, :, 0], data[:, :, 1], data[:, :, 2]
    mask = (r > 225) & (g < 30) & (b > 225)
    data[mask] = [0, 0, 0, 0]
    img = Image.fromarray(data)

    W, H = img.size
    cw, ch = W // cols, H // rows
    count = 0
    for row in range(rows):
        for col in range(cols):
            cell = img.crop((col * cw, row * ch, (col + 1) * cw, (row + 1) * ch))
            if out_w != cw or out_h != ch:
                cell = cell.resize((out_w, out_h), Image.NEAREST)
            out_path = os.path.join(output_dir, f"{prefix}{count:02d}.png")
            _save_png_slice(cell, out_path)
            _verify_iend(out_path)
            make_meta(out_path, out_w, out_h)
            print(f"  {out_path} ({out_w}x{out_h})")
            count += 1
    print(f"\nDone: {count} tiles + {count} .meta files -> {output_dir}")


if __name__ == "__main__":
    ap = argparse.ArgumentParser(description="Split tile sheet and remove chromakey")
    ap.add_argument("--source", required=True, help="Input image path")
    ap.add_argument("--output", required=True, help="Output directory")
    ap.add_argument("--cols", type=int, default=4, help="Grid columns (default 4)")
    ap.add_argument("--rows", type=int, default=4, help="Grid rows (default 4)")
    ap.add_argument("--width", type=int, default=64, help="Output tile width (default 64)")
    ap.add_argument("--height", type=int, default=64, help="Output tile height (default 64)")
    ap.add_argument("--prefix", default="tile_", help="Output filename prefix")
    args = ap.parse_args()
    process(args.source, args.output, args.cols, args.rows, args.width, args.height, args.prefix)
