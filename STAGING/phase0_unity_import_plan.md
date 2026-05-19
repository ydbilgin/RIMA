# Phase 0 Unity Import Plan — PPU=32 Scale Test

## Asset paths (PixelLab download sonrası)

```
Assets/Sprites/Environment/Phase0_ScaleTest/
  ├── floor/
  │   └── tile_01_stone.png      (32×32 native)
  ├── wang16/
  │   └── stone_moss_pair.png    (4×4 grid, 16 tiles, 128×128 sheet)
  ├── props/
  │   └── crate_pick_01.png      (64×64 native)
  └── decals/
      └── moss_patch_pick_01.png (64×64 native)
```

## Unity Editor script görevi

1. **Import settings (per-asset):**
   - Floor: PPU=32, FilterMode=Point, alpha=true, max 64
   - Wang16: PPU=32, FilterMode=Point, sprite mode=Multiple, 4×4 grid slice
   - Crate: PPU=32, FilterMode=Point, alpha=true (64×64 → 2×2 unit display)
   - Moss decal: PPU=32, FilterMode=Point, alpha=true (64×64 → 2×2 unit)

2. **Create scene `Phase0_ScaleTest`:**
   - Camera orthographic, size=5, position (4, 4, -10)
   - Grid GameObject with cellSize=(1, 1, 0) — confirms 32×32 = 1 unit

3. **Layer compose:**
   ```
   Origin (0,0):    Floor base 5×5 grid (25 tiles, all stone_01)
   Wang transition: (2,2) center — 3×3 patch of moss-stone Wang corner pattern
   Crate prop:      (1,1) position, 64×64 sprite = visual 2×2 unit
   Moss decal:      (3,3) position, overlay on floor
   Character:       (4,4) center, 64×64 Warblade @ PPU=32 = 2×2 unit
   ```

4. **Visual verdict checks:**
   - 32×32 tiles 1 Unity unit boyutunda mı?
   - Wang16 corner pattern doğru hizalanıyor mu (no half-cell offset)?
   - Crate prop 2×2 unit görsel boyut mu (karakter ile kıyaslanabilir)?
   - Moss decal floor üstüne doğru overlay oluyor mu?
   - Karakter (2×2 unit) tile grid'de doğal mı duruyor?

5. **Screenshot Game View** → Phase 0 verdict
