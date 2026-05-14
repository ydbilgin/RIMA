# S75 Autonomous Execution Plan
**Date:** 2026-05-14 night
**Mode:** User AFK (okul). Sonnet orchestrator drives sequential Codex dispatches + UnityMCP tests.
**Goal:** Map Designer PixelLab parity (UX + visual) + upper/lower terrain logic clarity + automation chain to Faz 1.5 features.

---

## USER LAST FEEDBACK (verbatim, 2026-05-14)

> "bu tam istediğim gibi pixellabdaki gibi çalışmıyor. ben şimdi okula gidecem bunu ve bundan sonraki bütün görevleri listele ve sırayla yaptır bütün her şeye. upper lower terrain mantığını doğru şekilde ver yaptırt çalıştır. otomasyona bağla"

**Interpretation:**
- PixelLab Map Tool UX'ini birebir vermemiz lazım
- Upper/lower terrain mantığı kullanıcıya açık olmalı (hangi pairing aktif, neden bu tile geliyor)
- Otomasyon: ben (Sonnet) sırayla phase'leri dispatch edip test edeceğim, user dönüşünde tüm chain bitmiş olacak

---

## PHASES (Sequential)

### ✅ S75-A: Map Designer UX Deep PixelLab Parity
**Codex task spec:** `STAGING/codex_s75_a_mapdesigner_ux_deep.md`
**Effort:** high (~700-1000 satır touch)

**Fix list:**
1. **Canvas default size 32×24** (was 16×12) — PixelLab parity bigger map
2. **Auto-Fit on open** — cellSize otomatik canvas'a sığacak şekilde compute
3. **Hover preview = REAL TILE** — şu anki cell hover yeşil renkli rect; YENİ: cursor altında gerçek paint sonucu sprite preview overlay
4. **Brush radius visual** — radius > 1 ise canvas'ta circle/square outline göster
5. **Drag-paint cell interpolation** — fast mouse drag iki cell arası boş bırakmasın (Bresenham line draw)
6. **Active terrain indicator on cursor** — paint cursor altında küçük thumbnail
7. **Pairing info panel** — sağ alt mini panel: "Active pair: Wall(1)↔Floor(0) → FloorWall_CornerWangTileSet (transitionSize=0.25)"
8. **Lower/Upper labels in palette** — terrain butonu altında "id=N" + biome'da bu terrain'in hangi pairing'lerin lower/upper'ı olduğu mini-text
9. **Status bar 3-line** — Cell info, Active pairing info, Tips
10. **Smooth scroll zoom** — şu an 2f delta; cubic ease daha smooth
11. **Cursor sprite preview** — eraseMode'da kırmızı X cursor, paint mode'da terrain thumbnail
12. **Larger default cellSize** — 32 → 40 (PixelLab daha ferah)

**Validation:**
- Unity dotnet build PASS
- Reflection-based mouse tests still PASS (BrushInputHandler doğru)
- Live screenshot capture, hover/drag tests via UnityMCP execute_code

---

### ✅ S75-B: Multi-variant per Wang Key
**Codex task spec:** `STAGING/codex_s75_b_multivariant_wang.md`
**Effort:** high

**Goal:** Aynı Wang key için 2-4 varyant tile destekle → tile-grid tekrarı kırılır, PixelLab-like organik görünüm.

**Implementation:**
1. **CornerWangTileSetSO extend:**
   ```csharp
   [System.Serializable] public class WangKeyVariants { public TileBase[] variants = new TileBase[1]; }
   public WangKeyVariants[] tilesByKey = new WangKeyVariants[16];
   public TileBase GetTile(int nw, int ne, int sw, int se, int hashSeed = 0) {
     int key = ...;
     var variants = tilesByKey[key].variants;
     if (variants.Length <= 1) return variants.FirstOrDefault();
     int idx = (hashSeed * 73856093) ^ key & 0x7fffffff;
     return variants[idx % variants.Length];
   }
   ```
2. **Backward compat:** Eski `tiles[16]` array korunur, GetTile(...) fallback `tiles[key]` döner if `tilesByKey` empty.
3. **CornerWangPainter.Paint:** position hash → variant seçimi (deterministic seed = (x*73856093) ^ (y*19349663))
4. **RebuildAllWangTilesets:** tileset_meta.json yanında `tileset_meta_v2.json` veya `tileset_meta.json` içinde `variants` array varsa multi-variant import et. Şimdilik tüm tileset'ler tek varyant (legacy).
5. **(Optional)** Auto-Variant generator: editor menu "RIMA > Tools > Generate Wang Variant Stub" — mevcut tile sprite'ı 90/180/270 rotate ederek 4 variant üretir (placeholder until real PixelLab Pro gen).

**Validation:**
- F1 biome variants=1 → görsel davranış aynı kalmalı
- Stub rotate generator çalıştırıldığında 4 variants per key
- Paint sonrası farklı cell'lerde farklı rotation tile'ları görülmeli

---

### ✅ S75-C: Object Layer Faz 1.5 (Stub Impl)
**Codex task spec:** `STAGING/codex_s75_c_object_layer.md`
**Effort:** medium

**Goal:** PixelLab object manifest karşılığı — NPC/prop/spawn placement.

**Data model:**
```csharp
[Serializable] public class MapObjectPlacement {
  public string prefabPath; // "Assets/Prefabs/Mobs/Knight.prefab"
  public Vector2 position; // pixel coords in canvas
  public string id;
  public int layer;
}

// MapSaveData += List<MapObjectPlacement> objects
```

**UI:**
- Toolbar [Objects] button — şu an placeholder; tıklayınca **ObjectsPanel** aç (sağda yan panel)
- ObjectsPanel: prefab dropdown (Mobs/, Props/, SpawnPoints/) + Place button
- Place mode active iken canvas'ta tıkla → MapObjectPlacement ekle
- Placed objects = canvas'ta küçük sprite preview
- Right-click → delete

**Apply to Scene:**
- ApplyToScene'de tilemap paint + objects instantiate
- Position = canvas-pixel / cellSize → world coords

**Limit:** Faz 1.5 stub — UI mevcut, prefab assignment user'a kalmış. PixelLab manifest.json compatible export.

---

### ✅ S75-D: CharacterClass + MobDefinition SO Scaffold
**Codex task spec:** `STAGING/codex_s75_d_class_mob_so.md`
**Effort:** medium

**Goal:** 10 class + 6 new mob için ScriptableObject data containers.

```csharp
[CreateAssetMenu] class CharacterClassDefinition : ScriptableObject {
  public string className; // "Warblade"
  public Sprite idleSprite; // user generates via PixelLab, wires here
  public Sprite weaponSprite;
  public Vector2Int weaponCanvas; // (56, 20) etc per weapons_pixel_sizes_LOCK_S74.md
  public WeaponSpec weapon; // ref to WeaponDatabase entry
  public CharacterStats stats;
  public List<EchoSkillDefinition> echoSkills;
  // ... Karar #80 silhouette identity fields
}

[CreateAssetMenu] class MobDefinition : ScriptableObject {
  public string mobName; // "Seam Crawler"
  public MobRole role; // Swarm/Elite/Caster/Pack/MiniBoss/Support
  public Sprite idleSprite;
  public Vector2Int canvasSize; // 64x64 or 80x80 or 96x96
  public MobStats stats;
  public string riftPaletteAccent;
}
```

**Assets:**
- 10 CharacterClassDefinition asset oluştur, isim + canvas + stat placeholder
- 6 MobDefinition asset oluştur (Seam Crawler / Plate Widow / Relic Caster / Rift Hound / Hollow Arbiter / Spire Choirling)
- User PixelLab gen ettiğinde sadece Sprite alanını doldurur

**Storage:**
- `Assets/Data/Classes/*.asset`
- `Assets/Data/Mobs/F1/*.asset`

---

### ✅ S75-E: Stub Placeholder Sprites (Asset bypass for testing)
**Codex task spec:** `STAGING/codex_s75_e_stub_sprites.md`
**Effort:** small

**Goal:** PixelLab gen olmadan oyun mekanik test edilebilsin. 64×64 PNG renkli placeholder sprite'lar oluştur ve SO'lara bağla.

**Method:**
- Editor tool `RIMA > Tools > Generate Placeholder Sprites`
- Her class için tek renkli + initials (örn. "WB" Warblade) 64×64 PNG
- Her mob için role-based renk + initials
- SO'lara assign

**Result:** Oyun açıldığında karakter+mob görünür (placeholder), gameplay test edilebilir. Asset gen sonrası user manual replace edebilir.

---

### ✅ S75-F: Live integration sanity + final commit
**Codex task spec:** `STAGING/codex_s75_f_integration_test.md`
**Effort:** small

**Test scenarios:**
1. Map Designer aç, 32×24 boyutta map çiz, Apply to Scene
2. ObjectsPanel ile 1 SeamCrawler + 1 Knight place
3. Demo scene play → karakter + mob spawn, basic AI
4. Karakter seç (CharacterSelect scene), Warblade ile başla
5. Compile + console error 0

**Validation:**
- Unity compile clean
- Demo scene loads + plays
- Basic gameplay loop intact

---

## DISPATCH ORDER

Sequential, her phase Codex bitince UnityMCP test, sonra sonraki:

1. **S75-A** Map Designer UX deep parity (this phase: live test, screenshot, iterate)
2. **S75-B** Multi-variant Wang
3. **S75-C** Object layer
4. **S75-D** Class + Mob SO scaffold
5. **S75-E** Stub placeholder sprites
6. **S75-F** Integration test + commit
7. **S75 CLOSE** CURRENT_STATUS update, S76 handoff hazır

Toplam estimate: 4-6 saat Codex execution + benim UnityMCP testing.

---

## FALLBACK / FAIL-SAFE

Her Codex dispatch'inden sonra:
- `git log --oneline -3` ile commit doğrula
- `read_console types=["error"]` ile compile error check
- Compile error varsa: spec'i revize, re-dispatch (max 2 retry)
- Test geçmezse: UnityMCP execute_code ile direct fix → commit "[S75-X fix]"

User dönünce session log:
- CURRENT_STATUS'ta S75-* commits listesi
- Her phase için screenshot path
- Bilinen kalan sorunlar (varsa)

---

## NOT NOT NOT

- Mevcut commit'lere dokunma (history korunur)
- Asset gen YAPMA (user manual yapacak PixelLab'da)
- 8-dir Create Character batch YAPMA (separate pipeline)
- Demo scene'i bozma (gameplay regression yasak)
