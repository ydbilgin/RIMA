# S74 HANDOFF — Map Designer Production Readiness Test
**From:** S73 Sonnet orchestrator (Claude)
**To:** S74 agent (Sonnet veya Opus — kararı sen ver)
**Status:** Map Designer multi-terrain refactor done ama kullanıcı "doğru çalışmıyor" dedi → Diagnostic gerekli
**User name:** Laureth (solo indie dev, 25-day school deadline, 11 days left)

---

## TLDR
Map Designer şu an `commit 19a4828` durumunda. Multi-terrain refactor tamamlandı. Kullanıcı yine de tatmin olmadı. **Senin görevin:**

1. Map Designer'ı kullanıcı ile birlikte test et
2. Spesifik sorun ne — öğren
3. Düzelt
4. Production-ready hale getir
5. Kullanıcı "iyi" diyene kadar iterate

---

## Önce OKU (sırayla)

1. `F:\Antigravity Projeler\2d roguelite\RIMA\CURRENT_STATUS.md` — session state
2. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\smart_map_painter_design_LOCK.md` — rima-design (Opus) verdict
3. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\pixellab_maps_ux_research.md` — PixelLab UX research
4. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\full_mesh_tileset_generation_log.md` — 5 new tileset IDs
5. `F:\Antigravity Projeler\2d roguelite\RIMA\CODEX_DONE_laurethayday.md` — son Codex sonucu (dispatch bazhzdr4k)

---

## Map Designer Mimari Özet (S73 Dispatch 1.6 sonu)

```
RimaMapDesignerWindow (single grid, multi-terrain)
 ├─ Single int[,] terrainGrid (vertex value = terrain ID, not binary)
 ├─ Biome dropdown (RimaBiomePreset)
 ├─ Left panel: Terrain palette (4+ buttons: Wall, Path, Rift, ...)
 ├─ Right panel: PAINT (green) + ERASE (red) + Brush + Cell/Vertex toggle + Advanced foldout
 ├─ Toolbar: New / Save / Load / Apply to Scene / Generate Room / Clear All / Cell / Vertex / Erase / Fit / cellSize slider
 ├─ Canvas: live tile preview + hover highlight (green=paint, red=erase)
 │   ├─ Cell ile 3+ terrain → RED X (error)
 │   └─ Cell 2 terrain ama pairing yok → ORANGE warning
 ├─ Pixelorama controls: scroll zoom, +/-, Space+drag pan, Fit to window
 ├─ Drag-paint (mouse drag = continuous paint)
 └─ Status bar: "Cell (X,Y) Terrain=N | Biome: ... | Tool: ... | Mode: ..."

Helper classes:
- BrushInputHandler — mouse → cell/vertex resolution
- TilesetPaletteDrawer — biome'dan terrain butonları render
- TilemapMutator — Wang painter delegate
- CliffYSortManager (runtime) — TilemapRenderer Individual mode for cliff cells

RimaBiomePreset (extended):
- List<MapTerrain> terrains (id, name, paletteColor, baseTile)
- List<TilesetPairing> tilesetPairings (lowerTerrainId, upperTerrainId, tileSet)
- FindPairing(int t1, int t2), IsValidPair(int t1, int t2)

F1 BiomePreset content (after dispatch bazhzdr4k):
- 4-5 terrain: Floor (0), Wall (1), Path (2), Rift (3), [Moss (4)]
- 6-7 pairing (full mesh + opsiyonel moss)
```

---

## Bilinen / Şüpheli Sorunlar

### 1. Floor BaseTile Bug (S73'te tespit edildi)
QC screenshot `STAGING/qc_d16_final.png`'de canvas background MAUVE HEXAGON görünüyordu — Shattered Keep biome'da olmamalı, RUBBLE olmalı.

**Diagnostic:**
```csharp
var biome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
var floor = biome.terrains.Find(t => t.id == 0);
Debug.Log("Floor baseTile: " + (floor?.baseTile?.name ?? "NULL"));
// Should be "wang_floor_wall_tile_0", not "wang_mauve_hexagon_tile_0"
```

Dispatch `bazhzdr4k` bu bug'ı düzeltmesi gerekiyordu — başarılı oldu mu kontrol et.

### 2. "Map Designer doğru çalışmıyor" — Belirsiz
Kullanıcı net söylemedi. Olası noktalar:
- Mouse precision (canvas-local koord)
- Paint feedback ne kadar net
- Tileset render quality
- Erase davranışı
- Pixelorama controls keyboard layout (Turkish? US?)
- Drag-paint hassasiyeti
- Multi-terrain pairing logic kullanıcı için sezgisel mi
- Performance (60+ tileset varsa palette scroll)

**KULLANICIYA SOR:**
> "Map Designer'da spesifik olarak ne çalışmıyor? Adım adım anlat veya screenshot at."

---

## Test Senaryosu (kullanıcı ile birlikte)

1. **Açılış:** `RIMA > Tools > Map Designer` — pencere açılır mı? Layout düzgün mü?
2. **Biome seçim:** Sol panel "Biome" dropdown → "Shattered Keep" seç → terrain palette doluyor mu?
3. **Floor görünüm:** Hiç boyamadan canvas'a bak → tüm hücreler RUBBLE FLOOR mu? (mauve değil mi?)
4. **Wall paint:** Sol panel "Wall" tıkla → sağ panel PAINT (yeşil) zaten aktif → canvas'a tıkla → wall tile boyandı mı? Wang transition kenarlarda doğru mu?
5. **Drag paint:** Mouse'u sürükle → continuous paint geliyor mu?
6. **Path paint:** Sol panel "Path" tıkla → başka bir alana boya → path tile + transition
7. **Wall + Path direkt komşu:** Bir wall cell yanına path cell koy → wall_path pairing tile gelmeli (eğer Dispatch bazhzdr4k başarıyla wall_path SO oluşturduysa)
8. **3+ terrain test:** Bir cell'in 4 köşesine 3+ farklı terrain koy → RED X gelmeli
9. **Erase:** ERASE butonuna bas → cursor kırmızı → tıkla → cell base'e (Floor) dönmeli
10. **Zoom:** Scroll wheel / +/- keys → cellSize değişiyor mu
11. **Pan:** Space tuşu basılı + drag → canvas pan
12. **Fit button:** "Fit" toolbar → cellSize otomatik
13. **Apply to Scene:** Output Tilemap ata → Apply tıkla → sahnedeki tilemap güncellenmeli
14. **Save/Load:** Save → JSON oluştu mu? Load → geri yüklendi mi?
15. **Generate Room:** "Generate Room" toolbar → RoomGeneratorWindow açılır → template seç → Map Designer'a yüklenir

Her adımda screenshot al (PowerShell ile editor window), kullanıcıya göster.

---

## Editor Screenshot (kullanıcının GERÇEK gördüğü)

UnityMCP execute_code:
```csharp
var winType = System.Type.GetType("RIMA.Editor.RimaMapDesignerWindow,Assembly-CSharp-Editor");
var w = Resources.FindObjectsOfTypeAll(winType).FirstOrDefault() as EditorWindow;
w.Focus(); w.Repaint();
return $"x={w.position.x} y={w.position.y} w={w.position.width} h={w.position.height}";
```

PowerShell:
```powershell
Start-Sleep -Milliseconds 1500
Add-Type -AssemblyName System.Drawing
$bmp = New-Object System.Drawing.Bitmap <w>, <h>
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.CopyFromScreen(<x>, <y>, 0, 0, $bmp.Size)
$bmp.Save("F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\s74_<scenario>.png")
```

Sonra Read tool ile gör + kullanıcıya göster.

---

## Dispatch Pattern (S73'te öğrenildi)

Codex dispatch için profil öncelik: **laurethayday → laurethgame → yasinderyabilgin** (last resort)

```bash
python '/f/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py' \
  --task-file 'STAGING/<task>.md' \
  --effort high \
  --profile <profile>
```

`run_in_background: true` zorunlu (cx_dispatch hard rule).

---

## PixelLab MCP

Yeni tileset gerekirse:
```
mcp__pixellab__create_topdown_tileset(
  lower_description="...", 
  upper_description="...",
  lower_base_tile_id="<existing tile id for chain>",
  upper_base_tile_id="<existing tile id>",
  transition_size=0 (zemin↔zemin) or 0.25-0.5 (floor↔wall),
  tile_size={"width":32,"height":32},
  shading="medium shading",
  detail="medium detail",
  view="high top-down"
)
```

~100sn async. `get_topdown_tileset(id)` ile durum.

Base tile IDs (F1 terrain chain):
- rubble (`2165fb86-...`)
- wall (`02586a60-...`)
- path (`7f5b8f02-...`)
- rift (`6e5e6639-...`)

---

## Karakter Prompt Status (paralel iş, kullanıcı PixelLab'da)

`STAGING/character_idle_weaponless_prompts_LOCK.md` — 16 character base + 11 weapon prompts hazır. Kullanıcı Create Image Pro ile generate ediyor.

Bu agent'ı etkilemez — paralel kullanıcı tarafı task.

---

## Token Economy Tip

- Direct Bash + UnityMCP execute_code = ~0k baseline
- Sub-agent spawn = ~22k baseline
- Çoğu Map Designer fix doğrudan execute_code ile yapılabilir
- Sadece tasarım kararı veya 50+ satır kod gerekirse sub-agent

Memory load `~/.ccs/.../memory/` üzerinden gelir (STUB MODE, manual trigger phrases).

---

## Bu Session Sonunda Yapılması Gereken

- Map Designer kullanıcı "iyi çalışıyor" diyene kadar test + fix
- Production-ready commit
- CURRENT_STATUS.md güncelle (S74 işleri)
- (Opsiyonel) Faz 1.7 ergonomi: Biome Editor Window + Missing Pairing Detector + One-click Generate

Başarılar.
