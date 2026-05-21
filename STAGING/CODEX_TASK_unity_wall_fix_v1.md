# Codex Task — Unity Wall Smooth Connection Fix (RuleTile + Y-Sort)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**UnityMCP REQUIRED.** Unity Editor açık olmalı.

---

## Görev

Pilot A wall 7-piece setini Unity Tilemap **RuleTile** asset'ine convert et + iso depth için generic **Y-based SortingOrder** script yaz. Walls smooth + neighbor-aware connect olsun.

## Workflow

### Step 1 — Wang Core 4 RuleTile Setup

Path: `Assets/Art/Tilesets/Act1_WallRuleTile.asset`

**Source pieces (mevcut):**
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_1_face_EW.png`
- `pilot_a_frame_2_corner_outer.png`
- `pilot_a_frame_3_arch_opening.png`
- `pilot_a_frame_4_face_NS.png`
- `pilot_a_frame_5_corner_inner.png`
- `pilot_a_frame_6_T_junction.png`
- `pilot_a_frame_7_end_cap.png`

**RuleTile rules (Wang Core 4 — N/E/S/W neighbor presence):**

| Neighbors | Piece | Rotation |
|---|---|---|
| All 4 (NESW) | T_junction (4-way fallback) | 0° |
| 3 (NES / ESW / NSW / NEW) | T_junction | 0/90/180/270° |
| 2 opposite (N+S) | face_NS | 0° |
| 2 opposite (E+W) | face_EW | 0° |
| 2 adjacent (N+E) | corner_outer | 0° |
| 2 adjacent (E+S) | corner_outer | 90° |
| 2 adjacent (S+W) | corner_outer | 180° |
| 2 adjacent (W+N) | corner_outer | 270° |
| 1 only (N) | end_cap | 0° (cap facing N) |
| 1 only (E) | end_cap | 90° |
| 1 only (S) | end_cap | 180° |
| 1 only (W) | end_cap | 270° |
| 0 neighbors | end_cap | 0° (standalone fallback) |

**arch_opening:** Reserve as manual placement only (entry/exit). NOT in RuleTile auto pool. User Painter ile manuel yerleştirir.

**corner_inner:** Use when wall pivots into interior corner (T-junction shape with concave inner side). RuleTile rule: 2-neighbor adjacency case where wall wraps around (advanced rule — if simpler `corner_outer` 90°-rotation works, defer corner_inner usage).

### Step 2 — RuleTile Asset Creation

UnityMCP ile:
1. `Tilemap` package mevcut mu check (yoksa Package Manager ile ekle)
2. `Assets > Create > 2D > Rule Tile` ile asset create
3. Path: `Assets/Art/Tilesets/Act1_WallRuleTile.asset`
4. Default sprite: `pilot_a_frame_7_end_cap.png` (fallback)
5. Tiling rules: 12 entry (yukarıdaki tablo)
6. Her rule: Neighbor mask (8-cell visualizer'da N/E/S/W) + Sprite + Rotation

### Step 3 — Painter Integration

`Assets/Scripts/Tools/RimaWorldPainterWindow.cs` (existing) — extend:
- "Wall RuleTile mode" toggle ekle (default ON)
- Wall layer'da paint: RuleTile auto-connect kullansın
- Eski "manual wall piece select" mode opsiyonel kalsın (arch_opening için)

Painter API mevcut — sadece `wallPaintMode` enum + Tilemap.SetTile() çağrısı eklenecek.

### Step 4 — Y-Based SortingOrder Script

Path: `Assets/Scripts/Utilities/IsoSortingOrder.cs`

```csharp
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class IsoSortingOrder : MonoBehaviour {
    [SerializeField] int sortOffset = 0;
    [SerializeField] float multiplier = 100f;
    SpriteRenderer sr;

    void OnEnable() {
        sr = GetComponent<SpriteRenderer>();
        UpdateOrder();
    }

    void LateUpdate() {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        UpdateOrder();
    }

    void UpdateOrder() {
        if (sr == null) return;
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * multiplier) + sortOffset;
    }
}
```

**Attach strategy:**
- Auto-attach utility menu: `RIMA/Tools/Attach IsoSortingOrder to Selected`
- Scene'deki tüm wall + prop + player GameObject'lerine attach
- Tilemap için ayrı: Tilemap renderer'ı kendi sort axis kullanır (Edit > Project Settings > Graphics > Camera Settings > Transparency Sort Mode: Custom, Axis Y=1)

### Step 5 — Tilemap Project Settings

UnityMCP ile global graphics ayarı:
- `Edit > Project Settings > Graphics > Camera Settings > Transparency Sort Mode`: **Custom Axis**
- Transparency Sort Axis: `(0, 1, 0)` (Y-axis dominant)

Bu Tilemap ve Sprite Renderer'lar için iso depth otomatik hesaplar.

### Step 6 — Test

`Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` üzerinde:
1. Mevcut manual placed wall'ları temizle (`mcp__UnityMCP__find_gameobjects` + delete child wall GameObjects)
2. Yeni Tilemap GameObject ekle (Wall layer): `WallTilemap`
   - Components: Tilemap, TilemapRenderer (sortingOrder layer "Walls")
3. RuleTile asset'i Painter'a yükle
4. 16×10 closed rectangle çiz Painter ile (W mode auto-connect)
5. Arch_opening manuel south wall ortasına yerleştir
6. Screenshot `STAGING/screenshots/wall_ruletile_test.png`
7. Mevcut wall connections görsel test: corners + faces + junctions auto-pick doğru mu

## Output

1. `Assets/Art/Tilesets/Act1_WallRuleTile.asset`
2. `Assets/Scripts/Utilities/IsoSortingOrder.cs`
3. `Assets/Scripts/Tools/RimaWorldPainterWindow.cs` — extended (wall mode)
4. `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` — wall layer Tilemap'e migrate edildi
5. `STAGING/screenshots/wall_ruletile_test.png` (before/after comparison)
6. Project Settings → Transparency Sort Mode değişti
7. `CODEX_DONE_*.md`: visual quality verdict + RuleTile rule count + compile status

## Compile Check (HARD RULE — 2026-05-21 LOCK)

- Console hatalarını OTOMATIK fix et: dispatch sonrası `read_console` çağır
- Error/warning varsa çöz + recheck
- Hâlâ hata varsa BLOCKED + raporla
- Bu task scope'a embed — separately "fix errors" task açmaya gerek YOK

## Kısıt

- Mevcut Pilot A 7 piece YETERLİ — yeni asset gen YASAK
- Manuel arch_opening kalır (Painter manual mode)
- corner_inner kompleks rule (defer if corner_outer + rotation yeterli)
- Painter UI yıkma — sadece wall mode toggle ekle, kalan UI dokunma
- Player + prop transform Z=0 — sadece Y-axis sortingOrder belirler

## Effort
high
