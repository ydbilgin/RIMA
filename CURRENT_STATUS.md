**2026-05-11 — S55 sonu | Aktif Sprint: Faz 1-2**

> **S55 bu session ozet:**
> - **PROMPT.md anchor workflow rewrite DONE:** 4 sinif (warblade/ranger/shadowblade/elementalist) PIXELLAB_OUTPUTS PROMPT.md dosyalari anchor-based workflow'a gecirildi. "Base body only / silahsiz" kaldirildi. Commit e717063.
> - **Concept art batch 1+2 DONE:** 12 sahne uretildi (4 karakter+mob sahnesi commit 25d9fb8, 8 otonom sahne commit 7b0d291). PIXELLAB_OUTPUTS/concept_art/ altinda.
> - **D1 LOCKED:** Mob infighting yok. Penitent Bruiser anti-heal aura faction-blind (%50 azaltma, 3m radius).
> - **D2 LOCKED:** Terrain hazard sistemi var — hikayeye uygun. F1: rift catlaklari, F2: coken zemin, F3: lav/aktif rift yirtigi.
> - **D3 LOCKED:** Komsu oda peek — sadece harita parcasi edinilmisse VEYA oda daha once temizlenmisse gorunur.
> - **D4 LOCKED:** Hub practice alani var, Hades'ten farkli tarz. Skill test + dummy.
> - **Hub design vizyon:** Safe zone, NPC alani, karakter secim odasi (fiziksel dunya "Reclaim" mekanigi — fikir kaydi), skill test ayni alanda. Karakter secimi her run degistirilebilir, heat per-character (STS mantigi).

> **Siradaki session (S56):**
> 1. Room Designer F2 — Batch 1 Codex dispatch (T1 RoomBlueprint + T2 BiomeType + T3 FloorVariantPainter)
> 2. Hub design detaylandirma (HUB_DESIGN_v1 backlog'dan cikar, netlestir)
> 3. Terrain hazard spec yazimi (D2 implement icin)
> 4. /nlm-sync (S55 karar ve dosyalari)

---

## Acik Isler (Oncelik Sirasina Gore)

### Yuksek Oncelik

1. **Room Designer F2 -- Batch 1** (Codex dispatch hazir)
   - T1: RoomBlueprint -- noiseSeed, variantIndex byte[], overrideVariantIndex, roomWidth/Height/Origin
   - T2: BiomeType enum (Keep/Crypt/Volcanic)
   - T3: FloorVariantPainter -- domain-warped Perlin (warpFreq 0.05, zoneFreq 0.05, detailFreq 0.22), BustClusters
   - Dispatch order: T1+T2+T3 paralel -> compile -> QC -> Batch 2

2. **Room Designer F2 -- Batch 2** (Batch 1 compile sonrasi)
   - T4: RoomMetadata panel (roomId, biome, gateCount, noiseSeed, Reseed button)
   - T5: WallRuleTileBrush + WallOverridePanel (8 variant, auto/manual toggle)
   - T6: PerlinFloorBrush (tilemap integration, domain-warp bake)

3. **Room Designer F2 -- Batch 3+4** (sonraki session)
   - T8: Layer panel refresh, T9: Reseed wiring, T10: Object Library, T11: Template Wizard, T12: RoomSaver export

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
| **PIXELLAB klasor** | **PIXELLAB_OUTPUTS/ (kalici) -- STAGING/PIXELLAB kaldirildi -- 2026-05-11** |
| Map editor | Custom Unity EditorWindow (RIMA Room Designer) -- 2026-05-10 LOCKED |
| Concept art stili | Pixel art ZORUNLU, painterly YASAK -- anchor metadata.json referans |
| PixelLab MCP yonetimi | Sonnet dogrudan cagri, Codex pre-review + post-QC -- 2026-05-10 |
| Tile chromakey | #00FF00, filter G>200 AND R<60 AND B<60, binary alpha snap |
| Duvar boyutu (PixelLab) | 64x128 (32x64 base + 2x nearest-neighbor upscale) |
| Zemin boyutu | 64x64, 16 var |
| Anim view | High top-down (~30-35 deg, Hades match) |
| Anim yonler | 8 yon ayri (flip yok) |
| v1 sprint siniflari | Warblade / Ranger / Shadowblade / Elementalist (kalan 6 -> v2) |
| Dungeon mimari | Prefab-per-room, Hades-style closed arena |
| Skill keybind | LMB/RMB + Q/E/R/F + V(ult) + Space(dash) |
| Shadow Echo | 3 katman (aura+phantom+UI flash), cyan #00FFCC, 50 havuz |
| Act 1 map | 15 node procedural (topoloji sabit, icerik random) |
| Boss posture | Faz1=700 / Faz2=850 / Faz3=1000 |
| Final Boss canavar | 252-256px PixelLab + PPU=32 (Faz 4 = 96px insan formu) |

---

## Mimari Referanslar

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
