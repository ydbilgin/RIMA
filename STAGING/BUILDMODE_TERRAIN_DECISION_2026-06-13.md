# BUILD MODE — TERRAIN TEKNIK KARARI (Council, 2026-06-13)

> Soru: Amine Rehioui (runtime RTS map editor, organik firca terrain, az-GameObject) ve Orb (tileset yerine world-space texture terrain) videolarindaki teknikleri RIMA Build Mode'a uygular miyiz?
> Advisorlar: cx (feasibility/reuse) + ax Gemini 3.1 Pro (derin mimari) + ax Gemini 3.5 Flash (lean/ship-fast). Ham cikti: `_process/2026-06/_council_*_buildmode_terrain.md`.

## KARAR: World-space-texture / organik firca terrain = DEMO ICIN ATLA. Iso Grid/Tilemap otoritesi korunur.

**Oybirligi (3/3 advisor).** Demo ~1 hafta, in-editor.

### Neden (uzlasma noktalari)
1. **Paradigma catismasi:** World-space splat terrain Tilemap ile degil, Quad/Mesh + custom splat shader ile calisir. Tilemap pipeline'ini cope atmak = felaket scope creep.
2. **Pixel-art kirilir:** Standart alpha-blend splat = HD bulanik. PPU-64 korumak icin shader'da texel-quantize + Bayer dithering + point-sample sart (ciddi efor). Pixel Perfect Camera world-space mesh shader'i otomatik pixel-perfect YAPMAZ.
3. **Gorsel/lojik kopmasi:** Serbest organik firca, iso staggered walkability grid'ine "snap" edilemez -> hangi hucre walkable belirsizlesir. RIMA'nin "source of truth"u = Grid; firca onu bypass eder.
4. **Performans motivasyonu GECERSIZ:** REF1'in "GameObject overhead kaldirma" gerekcesi RTS-olcegi icin. RIMA oda-bazli; Unity Tilemap zaten chunk'lar halinde tek mesh/draw-call cizer, taban terrain'de GameObject yok. Cozulmus problem.
5. **Estetik:** Act1 canon (Slate/Void/Ember) brutalist, keskin-hatli, discrete tile gecisleri ister; organik yumusak zemin okunabilirligi DUSURUR. Grid-okunabilirligi ARPG icin esastir.

### Onemli REUSE kesfi (cx)
RIMA'da **dormant world-space terrain altyapisi MEVCUT:** `Assets/Shaders/TerrainBlend.shader` (`RIMA/TerrainBlend`, splat-by-UV + texture-by-worldPos.xy + noise/blend-sharpness), `Systems/Map/TerrainBlendRenderer.cs` + `TerrainBlendConfig.cs` (quad mesh + RGBA splat prototip), `Art/TerrainBlend/` material+config+textures, `MapLayerOrchestrator.useShaderBlend` flag (kapali), arsiv `ShaderBlend_Test_s99.unity`. EKSIK: aktif Build Mode entegrasyonu, pixel-art-final quantize/dither pass, runtime save/load, RoomTemplateSO splat storage. -> Post-demo'da SIFIRDAN degil, UYANDIRILARAK yapilir. Ayrica brush/decal altyapisi (`MapDesigner/Brush/**`, `RoomDecalChunkRenderer`, `TransitionBrushPainter`, `DetailDecalPainter`, `BrushLayerOperation.respectsWalkableMask`) opsiyonel organik decal icin reuse-edilebilir.

## AL / POST-DEMO / ATLA tablosu

| Teknik | Karar | Neden |
|---|---|---|
| P2 prop placement (iso Grid API, ghost, rotate, undo) | ✅ AL (BITTI) | Implement + runtime-dogrulandi (prop tam hucre merkezine oturuyor) |
| **P3 = basit hucre-otoriter tile/walkability/overlay brush** (Grid API `SetTile` + RoomTemplateSO arrays) | ✅ AL (SIRADA) | Dusuk risk, demo-gorunur; reuse: `IsoRoomBuilder.BuildFloor/BuildOverlay`, brush presetleri, `WalkabilityMap.InitFromTemplate`. ORGANIK DEGIL — discrete tile. |
| P4 light placement + runtime save/load | ✅ AL | Isik = en ucuz "WOW" (karanlik oda aninda aydinlanir); save/load = teknik basari kaniti. |
| Opsiyonel P3B: gorsel-only organik decal/patch brush (walkable-mask saygili) | 🟡 AL (vakit kalirsa) | REF "firca hissi"ni lojik grid'e dokunmadan verir; reuse mevcut Brush/Decal altyapisi |
| Validity ghost premium polish (soft outline/pulse Shader Graph) | 🟡 AL (ucuz) | ax Flash: premium his, sifir risk |
| World-space splat terrain (REF1/REF2 asil teknik) | 🔵 POST-DEMO | Feature-flag arkasinda, dormant TerrainBlend prototipini uyandir + pixel-art pass + decorative UNDERLAY (lojik grid'e ASLA dokunmaz) |
| Az-GameObject / data-only mesh terrain rewrite | 🔴 ATLA | Tilemap zaten cozuyor; RTS-olcegi yok; walkability/collision/build tool'lari bozar |

## RISK
Terrain'i SIMDI yapmak = calisan P1/P2'yi (Grid-otoriteli) bozma riski (rendering+walkability+collision+save/load+kamera fidelity hepsine dokunur). Yapilirsa SADECE mekanigi etkilemeyen ayri gorsel katman olarak. Demo plani DEGISMEZ: P3 (basit grid brush) -> P4 (light+save/load).

## NOT (routing): cx bu task'i `yekta` profilinde calistirdi (CODEX_DONE_yekta.md) — yekta memory'de DISABLED. Auto-secim digerleri musait olmadigi icin yekta'ya dustu olabilir; quota tool'un reaktif codex izlemesi (codex_status) bunu yakalamali. Ayrica kontrol et.
