---
name: 3d-portability-strategy
description: "RIMA Room Composer mimarisi renderer-agnostic kurgu — 2D pixel-art'tan HD-2D / sprite-in-3D'ye direkt port; data + composition logic taşınır, sadece rendering swap yapılır"
metadata: 
  node_type: memory
  type: project
  originSessionId: a12da79a-6b77-423a-8b7c-59af8ccea2f8
---

# 3D Portability Strategy (2026-05-17)

User sordu: "İleride bu oyunu veya başka oyunları 3D ortamda 2D boyamayla yapabilir miyiz? Map designer / sistemimiz uygun mu?"

**Cevap: EVET.** ChatGPT 2026-05-17 verdict zaten renderer-agnostic mimari kuruyor (bkz [[room-composer-paint-intent-lock]]). 2D'den HD-2D (Octopath Traveler / Sea of Stars) veya sprite-in-3D (Hades) oyunlara port'ta data + logic katmanları taşınır, sadece rendering swap'lanır.

## Portable katmanlar (rebuild gerektirmez)

| Katman | İçerik | 3D'de karşılığı |
|---|---|---|
| L0 Data | cornerField, collision, walkability, elevation, locks | Aynı, pure data |
| SO contracts | TerrainDef, PatchAtlas, PropDef, RoomVisualProfile, AdjacencyRule, ZoneMask, ImportAssetRole | Aynı |
| RoomPaintStroke 3-mode | TerrainPaint / OrganicPaint / StampPaint, semantic intent | Aynı |
| Composition logic | Density, adjacency 3×3, zone-based scatter, organic placement loop | Aynı |
| Macro patch (paint-big) | 64/128/256 patches with irregular alpha | 3D'de quad-decal veya terrain splat texture |
| Wang16 corner-mask | Elevation/feature edges | Aynı (3D'de yine grid-aligned terrain için geçerli) |

## Rework gerektiren katmanlar (rendering swap)

| 2D | 3D | İşlem |
|---|---|---|
| Unity Tilemap | Mesh-on-plane (single quad per terrain region) | Swap component |
| SpriteRenderer decal | Quad mesh decal (Y-billboarded veya plane) | Swap component |
| sortingOrder | Z-depth (otomatik) | Drop sortingOrder field |
| URP 2D ShadowCaster (L9) | URP 3D ShadowCaster | Swap component |
| PPU=32 LOCK | Irrelevant (free scale) | Drop PPU field |
| Orthographic camera | Perspective camera at iso angle | Swap camera config |

## Asset taşıma

- **PixelLab pixel art sprite → HD-2D**: Quad mesh'e yapıştır, billboard veya fixed-plane. Direkt port ✅
- **PixelLab sprite → Full 3D (Pseudoregalia low-poly tarz)**: Port olmaz, farklı asset pipeline gerek ❌
- **Macro patch (büyük painterly source)**: 3D'de texture splat layer veya quad-decal mesh ✅

## Şimdiden uygulanacak design rule

1. **SO interface'leri Unity-rendering ref tutmamalı** — sadece data + Sprite/Texture2D field. SpriteRenderer/Tilemap ref'leri concrete spawner'larda kalır.
2. **Paint stroke world-space coordinate** (cell index değil) — 3D'de yine geçerli olur, 2D'de cell index'e map'lenir.
3. **Composition logic Unity-API'sinden bağımsız** — `BrushStrokeManager.EvaluateStroke()` gibi headless API'ler pure-CSharp (Mathf hariç UnityEngine ref'siz).
4. **Renderer layer ayrıştır** — `IRoomRenderer` interface (TilemapRoomRenderer vs MeshRoomRenderer concrete impl'ler) gelecekte eklenebilir.

## MythicTile (UE5 plugin) referans — 3D mesh-based painted top-down (2026-05-18)

Studio Genki "MythicTale" pixel art roguelike, UE5 + kendi plugin'i (MythicTile). User feedback 2026-05-18: "designer ileride 3D'yi de kapsayacak, alabilecekleri al."

**Görülen feature'lar (video 2055658499540238423):**

| Feature | Açıklama | RIMA 3D port'unda kullanım |
|---|---|---|
| **Surface brush** | Düz top-down paint (bizim Blueprint Painter eşdeğeri) | Aynı, mesh-on-plane'e map |
| **Cliff Stack brush** | Height-aware paint, kenarlardan dik kayalık üretiyor | 3D port'ta: terrain elevation field + auto-cliff mesh |
| **Pillar Stack brush** | Vertical pillar üretimi | 3D port'ta: prop-pillar prefab pool, Y-axis stack |
| **Raise / Ramp / Fill** | Build Tools button bar — height manipülasyonu | 3D'de elevation grid edit |
| **Vertical Count = 3** | Per-cell height parametresi | 3D L0 data field: cellHeight (2D'de hep 0) |
| **Block Sets** | Atlas of placeable mesh sets | 3D'de prop mesh pool (bizim PropPoolSO'nun 3D mesh varyantı) |
| **Layers (Layer 0 / N2)** | Multi-layer paint stack | Zaten 8-layer canonical recipe'mizde var |
| **Map-link** | Multi-map connection (room-to-room) | RoomBank evolution: connection points + portal mesh |
| **Performance tab** | Mesh build ms, collision ms metrics | 3D port'ta build performance overlay |
| **Atlas tab** | Block browser | Asset Pack Browser 3D varyantı (mesh thumbnail) |
| **Brush Shape: Square / Diamond / Circle** | Stroke geometry | 2D Brush V1'e Diamond shape ekle (low effort, paralel yarar) |
| **Replace Existing Blocks toggle** | Overwrite mode | 2D + 3D: mevcut Brush V1 paint mode'a paralel |

**Şimdiden uygulanabilecekler (2D, hemen):**
1. **Diamond brush shape** — Brush V1 Square/Circle yanına Diamond ekle (Phase B-5 backlog'una)
2. **Performance overlay** — Auto-Populate sonrası "X cells / Y props / Z ms" metrics gösterimi
3. **Map-link konsept** — RoomBank V2'de room-to-room connection point field'ı (RoomVisualProfile'a `connectionPoints: List<ConnectionPoint>` ekle)

**3D port'unda gelecek genişleme:**
4. **cellHeight field** — L0 data'da elevation field (default 0, 3D port'ta non-zero)
5. **Cliff/Pillar brush** — Brush V1 paint mode'larına `CliffStack` ve `PillarStack` eklenir (2D'de no-op, 3D'de aktif)
6. **Block Sets = PropPool 3D variant** — PropPoolSO interface'ine `meshRef: Mesh` alternatif field (Sprite vs Mesh tek katmanlı SO ile karşılanır)

**Yasaklar (MythicTile bizden farklı):**
- Cliff Stack 2D'de aktif değil — height-aware brush 2D top-down'da görsel olarak işe yaramaz, sadece data field korunur
- Mesh-based mod 2D'de no-op, asla 2D pixel-art rendering pipeline'ı bozmamalı

## Cross-references

- [[room-composer-paint-intent-lock]] — 2026-05-17 ChatGPT FINAL verdict, mimari LOCK
- [[karar-143-layered-pipeline]] — L0-L11 layer model (3D'de aynı)
- [[brush-tool-v1]] — Brush V1 current state, semantic 3-mode extension portable

## Status

Strategic decision logged. Phase 1A bu prensiplere uyduğu sürece gelecek 3D port'u rebuild değil, swap olur. Tasarım yön sapması yok.
