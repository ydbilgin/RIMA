# F4 + F5 Fix — Opus Review CONDITIONAL Blockers

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Opus review (`STAGING/CLIFF_F1_F4_F5_REVIEW.md`) F4 + F5 için 4 MAJOR blocker tespit etti. Surgical fix.

## Bağlam
- F1 ✅ PASS (Opus review)
- F4 ⚠️ CONDITIONAL — 2 major + 1 perf risk
- F5 ⚠️ CONDITIONAL — 2 major
- Review verdict detay: `STAGING/CLIFF_F1_F4_F5_REVIEW.md`
- Spec ref'leri: `STAGING/f4_dust_particle_task.md`, `STAGING/f5_cliff_idle_anim_task.md`

## F4 Fix (CliffEdgeDustEmitter.cs ~3-10 LOC)

### Blocker 1: ParticleSystemRenderer.material atanmıyor (URP magenta riski)
- `CliffEdgeDustEmitter.cs` içinde her `ParticleSystem` instantiate sonrası:
  ```csharp
  var renderer = ps.GetComponent<ParticleSystemRenderer>();
  if (renderer != null && renderer.sharedMaterial == null)
  {
      // URP 2D Default Sprite-Lit material — built-in path
      renderer.sharedMaterial = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
      // Veya URP 2D default: Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default")
  }
  ```
- Alternatif: `CliffDustSettings` SO'ya `[SerializeField] private Material particleMaterial` ekle, Inspector'da assign edilebilir, fallback Sprites-Default

### Blocker 2: Camera.main Start'ta cache — Update lazy re-fetch
- Mevcut: `_cam = Camera.main` Start'ta sadece
- Fix:
  ```csharp
  void Update() {
      if (_cam == null) _cam = Camera.main;  // lazy re-fetch
      if (_cam == null) return;  // still null, skip LOD this frame
      // ... existing LOD logic
  }
  ```

### Blocker 3 (perf): maxParticles=200 per-emitter global cap eşit
- 283 emitter × 200 = teorik 56k overshoot
- Fix: per-emitter `maxParticles = Mathf.Max(10, settings.maxTotal / Mathf.Max(1, emitterCount))`
- Veya basit: per-emitter `maxParticles = 20` (200 / 10 avg active = 20 each)
- Settings SO'ya `maxPerEmitter` field ekle veya hard-coded 20

## F5 Fix (CliffFaceIdleAnimator.cs ~40 LOC revision)

### Blocker 1: Plain Tile pool sadece spritesS[] kullanıyor → DirectionalCliffTile direction-aware bypass
- **Problem:** SetTile plain Tile ile → DirectionalCliffTile.transformOffset + direction-aware sprite (spritesE/SE/SW/N/NE/NW) KAYBOLUR
- **Fix:** Per-cell yön compute ETMEDEN, **DirectionalCliffTile asset'ini DIRECTLY SetTile et**, sprite array index swap için **DirectionalCliffTile'ın internal variant rotation'una güven**
- Veya: 8-direction Tile pool oluştur, her direction için ayrı pool:
  ```csharp
  // Build pool per direction
  for (int dir = 0; dir < 8; dir++) {
      for (int variant = 0; variant < spritesFor(dir).Length; variant++) {
          var tile = ScriptableObject.CreateInstance<Tile>();
          tile.sprite = spritesFor(dir)[variant];
          tile.transform = Matrix4x4.Translate(_cliffTileSource.transformOffset);  // PRESERVE
          _pool[dir, variant] = tile;
      }
  }
  ```
- VEYA daha basit revision: Per-cell yön CliffAutoPlacer logic ile compute et, sonra `spritesFor(direction)[variantIndex]` swap

### Blocker 2: ManualPaintedCells D5.5 üstüne yazma riski
- Mevcut: `_cliffCells` cliff tilemap'teki tüm cell'leri sayıyor
- Fix: 
  ```csharp
  // Skip cells in ManualPaintedCells whitelist (D5.5 LIVE)
  if (_cliffAutoPlacer != null && _cliffAutoPlacer.ManualPaintedCells.Contains(cell)) continue;
  
  // Veya GetTile check:
  if (_tilemap.GetTile(cell) != _cliffTileSource) continue;  // skip non-source tiles
  ```
- Animasyon SADECE algorithmic cliff cell'lerine — manual painted/decor dokunma

## Dosyalar (scope)
- `Assets/Scripts/Environment/CliffEdgeDustEmitter.cs` (F4 fix ~10 LOC)
- `Assets/Scripts/Environment/CliffDustSettings.cs` (opsiyonel material field)
- `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs` (F5 fix ~40 LOC revision)
- Toplam ~50 LOC fix

## YASAK
- F1/F2/F3 (Sonnet PASS, dokunma)
- CliffAutoPlacer.cs (LIVE)
- DirectionalCliffTile.cs / DirectionalCliffTile_Hades.asset (LIVE)
- Yeni .cs (sadece mevcut fix)
- Refresh ZORUNLU sonrası: `mcp__UnityMCP__refresh_unity scope=all mode=force`

## Verify
- 0 err / 0 warn
- F4: ParticleSystem PlayMode'da görünür (magenta yok)
- F5: Cliff sprite swap doğru yön + transformOffset korunur + cell yukarı zıplama yok
- F5: ManualPaintedCells (DecorCliffTilemap içerikleri etkilenmez) — verify D5.5 LIVE

## Output
- `STAGING/F4_F5_FIX_DONE.md` — değişen dosyalar + Opus re-review için checklist

## Code rotation
Sen Sonnet yaz. Opus re-review F4+F5 PASS sonrası (rotation devam).
