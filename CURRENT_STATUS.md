**2026-05-11 — S57 | 2.5D Mimari Gecis Kararlari Kilitlendi | Aktif Sprint: Faz 1-2**

> **S56 bu session ozet:**
> - **Room Designer F2 TAM BITTI (QC PASS):** BiomeType, RoomBlueprint F2 alanlari, FloorVariantPainter (domain-warp Perlin bake+preview), WallAutoConnect (4-bit mask 8 tip), RoomMetadataPanel (Reseed/Preview/Override toggle), StampBrush+EraserBrush wall hook, ActiveBlueprint live instance, IsWallOverrideMode, SaveCurrentRoom -> MetadataPanel wiring. 15 commit, tum QC PASS.
> - **Pre-rendered 2D karari LOCKED:** Blender/Hades tarzi REJECTED v1; PixelLab pixel art kalir. TASARIM/PRERENDERED_2D_DECISION.md
> - **Commits:** ee365a2 -> 65f8a74 (15 commit)

> **S57 bu session ozet:**
> - **2.5D Mimari LOCKED:** 3D cevre + 2D billboard, yeni Unity URP 3D proje, salvage manifest
> - **6 Mimari Karar LOCKED (Opus):** Proje stratejisi, silah sistemi, 4 yon+flip, body-only anchor, WeaponController, Room Designer ITileWriter
> - **Silah Anchor Sistemi LOCKED (Opus):** body-only "empty hands gripping", WeaponAnchorMap, PPU=64, senkron normalizedTime
> - **Lint:** 7 cakisma tespit edildi, doc guncelleme tamamlandi

> **Siradaki session (S58):**
> 1. 2.5D POC — Yeni Unity URP 3D proje ac, 1 oda + Warblade south anchor billboard + lighting test
> 2. Salvage manifest — Assets/Scripts/ hangileri tasiniyor, hangileri yeniden yaziliyor
> 3. Body-only anchor uretimi — 4 class x 4 yon = 16 anchor (PixelLab Web App)

---

## Acik Isler (Oncelik Sirasina Gore)

### Yuksek Oncelik

0. **2.5D POC** (ONCELIK 0 — hicbir sey bu olmadan baslamiyor)
   - Yeni Unity URP 3D proje (3D template, URP)
   - 1 3D oda (Plane floor + Cube walls), Warblade south anchor billboard, point light
   - Pixel-perfect + lighting + billboard rendering dogrulama
   - PASS olmadan: Room Designer dokunulmaz, anim uretimi baslamaz

1. **Tile uretimi** (Room Designer F2 done -- tile'lar olmadan test yapilamiyor) (2.5D sonrasi 3D texture pipeline'ina tasiniyor)
   - F1 floor: 16 variant (Create Tiles Pro, URETIM_REHBERI.md)
   - W1 wall: 8 variant (Create Tile Isometric) -- WallAutoConnect'i calistiracak
   - Obstacles: Pillar once (style anchor)

2. **Room Designer F3** (F2 DONE -- tile bekliyor) (2.5D port bekliyor — POC sonrasi)
   - AI panel + MCP bridge (STAGING/mcp_requests/ -> responses/)
   - T10 Object Library, T11 Template Wizard, T12 RoomSaver export

### Orta Oncelik

4. **Warblade anim uretimi** -- ANIMASYON_URETIM.md hazir, PIXELLAB_OUTPUTS/warblade/ referans
   - Eraser Pass -> Idle -> Hurt -> Death -> Walk -> Attack LMB/RMB -> Dash -> Weapon Pass

5. **Tile uretimi** -- URETIM_REHBERI.md hazir
   - F1 floor (16 var, Create Tiles Pro) -> F2 -> F3 -> Trans
   - W1 wall (8 variant, Create Tile Isometric)
   - Obstacles (Pillar first -> style anchor)

### Dusuk Oncelik / Backlog

- Lore rework (STORY_RUN_PROGRESSION, HUB_DESIGN_v1, 3-ending cutscene)
- Map Fragment + DungeonGraph -- spec hazir
- MAKEUP_BACKLOG 8 eksiklik -- polish
- Cinematic Layer A/B/C/D -- Faz 2-5

---

## LOCKED Kararlar Ozeti (referans)

| Alan | Karar |
|---|---|
| **Mob infighting** | **Hayir. Penitent Bruiser aura faction-blind (%50 heal azaltma, 3m) — 2026-05-11 LOCKED** |
| **Terrain hazard** | **Var — F1 rift catlagi / F2 coken zemin / F3 lav+rift — hikayeye uygun — 2026-05-11 LOCKED** |
| **Room peek** | **Sadece harita parcasiyla VEYA cleared oda — 2026-05-11 LOCKED** |
| **Hub practice** | **Skill test + dummy, Hades'ten farkli — 2026-05-11 LOCKED** |
| **Karakter secimi** | **Her run degistirilebilir, heat per-character (STS mantigi) — 2026-05-11 LOCKED** |
| **Wall tile variety** | **Rule Tile hybrid (auto-connect + manual override) -- 2026-05-11 LOCKED** |
| **Floor tile variety** | **Domain-warped Perlin 3-katman, edit-time bake, template-fixed -- 2026-05-11 LOCKED** |
| **Tile kenar invariance** | **3px border = mortar #1A1C20 only, accent merkeze -- 2026-05-11 LOCKED** |
| **Room Designer vizyon** | **MapForge -- genel arac, isometric P0, topdown/sidescroller template -- 2026-05-11** |
| **Pre-rendered 2D** | **Blender/pre-render REJECTED v1; PixelLab pixel art LOCKED -- 2026-05-11** |
| **Render mimarisi** | **2.5D: 3D cevre (Ortho kamera) + 2D Billboard karakter/mob/obje/VFX. Alabaster Dawn tarzi. Tilemap KALDIRILACAK. — 2026-05-11 LOCKED** |
| **Unity proje** | **Yeni URP 3D proje. Mevcut proje arsiv. Salvage manifest ile tasinir. — 2026-05-11 LOCKED** |
| **Silah sistemi** | **Ayrik 2D sprite child + WeaponAnchorMap + AnimationCurve senkron. PPU=64. — 2026-05-11 LOCKED** |
| **Anim yonler** | **4 yon + yatay flip (8 yon REVOKE). N/S/E uretilir, W=flipX. — 2026-05-11 LOCKED** |
| **Body-only anchor** | **Yeni 16 anchor (4 class x 4 yon), silahsiz. Eski silahlı → _archive/. — 2026-05-11 LOCKED** |
| **Room Designer** | **ITileWriter abstraction → 3D prefab output. Brush/palette/save mantigi korunur. — 2026-05-11 LOCKED** |
| **FloorVariantPainter params** | **warpFreq=0.05, zoneFreq=0.05, warpStrength=4.0; tiers base/accent/hero -- 2026-05-11 LOCKED** |
| **WallAutoConnect variants** | **8 tip: straight_H/V, corner_NW/NE/SW/SE, end_L/R; 4-bit NSEW mask -- 2026-05-11 LOCKED** |
| **PIXELLAB klasor** | **PIXELLAB_OUTPUTS/ (kalici) -- STAGING/PIXELLAB kaldirildi -- 2026-05-11** |
| Map editor | Custom Unity EditorWindow (RIMA Room Designer) -- 2026-05-10 LOCKED |
| Concept art stili | Pixel art ZORUNLU, painterly YASAK -- anchor metadata.json referans |
| PixelLab MCP yonetimi | Sonnet dogrudan cagri, Codex pre-review + post-QC -- 2026-05-10 |
| Tile chromakey | #00FF00, filter G>200 AND R<60 AND B<60, binary alpha snap |
| Duvar boyutu (PixelLab) | 64x128 (32x64 base + 2x nearest-neighbor upscale) |
| Zemin boyutu | 64x64, 16 var |
| Anim view | High top-down (~30-35 deg, Hades match) |
| Anim yonler | 4 yon + flip (N/S/E uretilir, W=flipX) — 2026-05-11 |
| v1 sprint siniflari | Warblade / Ranger / Shadowblade / Elementalist (kalan 6 -> v2) |
| Dungeon mimari | Prefab-per-room, 2.5D kare grid arena |
| Skill keybind | LMB/RMB + Q/E/R/F + V(ult) + Space(dash) |
| Shadow Echo | 3 katman (aura+phantom+UI flash), cyan #00FFCC, 50 havuz |
| Act 1 map | 15 node procedural (topoloji sabit, icerik random) |
| Boss posture | Faz1=700 / Faz2=850 / Faz3=1000 |
| Final Boss canavar | 252-256px PixelLab + PPU=32 (Faz 4 = 96px insan formu) |

---

## Mimari Referanslar

- **2.5D mimari karar:** `MEMORY/project_2.5d_architecture.md`
- **Silah anchor sistemi:** `MEMORY/project_weapon_anchor_system.md`
- **Room Designer plan:** `MEMORY/project_room_designer_plan.md`
- **Tile variety sistemi:** `MEMORY/project_tile_variety_system.md`
- **Domain warp:** `MEMORY/project_domain_warp_tile_system.md`
- **MapForge vizyon:** `MEMORY/project_map_tool_vision.md`
- **PixelLab uretim:** `PIXELLAB_OUTPUTS/` (floors/ walls/ obstacles/ warblade/ ranger/ shadowblade/ elementalist/)
- **Uretim rehberi:** `STAGING/PIXELLAB/URETIM_REHBERI.md`
- **PixelLab MCP vs Manual:** `MEMORY/project_pixellab_mcp_vs_manual.md`
- **Visual identity bible:** `MEMORY/project_rima_visual_identity.md`
- **PixelLab playbook:** `PIXELLAB_OUTPUTS/PRODUCTION_PLAYBOOK.md`
- **Animation Bible:** `TASARIM/ANIMATION_BIBLE.md`
- **Skill System v2:** `TASARIM/SKILL_SYSTEM_v2.md`
- **Shadow Echo:** `TASARIM/SHADOW_ECHO_MATRIX.md`
- **Act 1 map:** `TASARIM/dungeon_act1_map.md`
- **Damage calc:** `TASARIM/DAMAGE_CALCULATION.md`
- **Mob rules:** `TASARIM/MOB_COMPOSITION_RULES.md`
- **Art pipeline:** `GUIDES/RIMA_MASTER_ART_PIPELINE.md`
- **Anchor karakterleri:** `Characters/anchors/<class>/metadata.json` (10 sinif)
