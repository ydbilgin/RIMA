# Codex Konsolide Paket — S66 Final Lock + Cleanup

**Tarih:** 2026-05-13
**Tip:** MECHANICAL — judgment yapılmış, sadece yaz/taşı/sil
**Effort:** medium
**Çıktı:** CODEX_DONE.md'ye final summary

## Bölüm A — Yeni Karar LOCK'ları

### A1. MASTER_KARAR_BELGESI.md — Karar #115 (LOCK)

Tabloya ekle:
```
| #115 | AI-Assisted Map Builder | RIMA Map Builder = Unity Editor Window tabanli AI-assisted authoring araci. Faz 1.0: deterministic C# RoomBaselineGenerator (System.Random, GenerationInput{seed,biome,archetypeId,w,h,generatorVersion} kontrati), RoomBaselineTemplate ScriptableObject parametre kaynagi, toolbar Generate butonu, floor/wall tilemap ciktisi, FloorVariantPainter+WallAutoConnect bake entegrasyonu, byte[] grid + LUT variant metadata, RoomBlueprint+Prefab+RoomPrefabLink+RoomConfig save koprusu. Faz 1.5: Unity-internal inpaint region re-seed (kilitsiz hucreler), lock-aware overrideVariantIndex+floorOverrideVariantIndex rebake, tile-mask anchor zone painter (zone type+weight), force re-seed komutu, RenderTexture cache+debounce, preview kamera ~35 derece konverjans kalibrasyonu. REJECTED: fullscreen oyun-gibi in-game editor framing, LLM runtime/editor cagrisi, PixelLab Inpaint API cagrisi, PNG export, runtime procedural 15-node placement override, RoomLoader secim mantigi bypass, rect/polygon anchor schema, UnityEngine.Random global state kullanimi. Path: Assets/Scripts/Systems/Map/ + Assets/Scripts/Runtime/Rooms/. Exit criteria Faz 1.0: ayni seed+biome+archetype bit-identical room, 5 oda generate runtime hatasiz, designer duzeltme %20 alti, RoomLoader RoomConfig-missing hata yok. | 2026-05-13 |
```

### A2. MASTER_KARAR_BELGESI.md — Karar #116 (LOCK)

```
| #116 | Tile Transition Quality Standard | F1+ tum tilesets icin zorunlu kalite standardi: (a) Floor-wall transition Raggedness >=40% (grid-block hissi yasak); (b) Ayni terrain icinde 3+ varyant zorunlu (mosaic-repeat hissi yasak); (c) Aseprite cleanup pass'inde edge-blend kontrol — 2 tile yan yana dikis okunmamali; (d) Lichen/moss/dust/rune patches Floor varyantlarinin %20-30 organik asimetrik dagilim; (e) Light bleed/rim shadow tile icinde baked DEGIL, Unity URP 2D Light runtime'da uygulanir. QC: 4x zoom pixel grain dogal, hairline crack continuity bozulmaz. Referans: Alabaster Dawn (transition smoothness), Hades (palette discipline). Bu kalite Karar #75 REVISION'la onaylanan create_topdown_tileset Pro mode + create_tiles_pro style mode kombinasyonu ile saglanir. | 2026-05-13 |
```

### A3. CURRENT_STATUS.md — Recent LOCKED Kararlar tablosuna ekle

```
| #115 | AI-Assisted Map Builder | Unity Editor F2 + Generate + brush rotur; Faz 1.0 MVP generator + RoomConfig save kopru |
| #116 | Tile Transition Quality | Raggedness >=40, 3+ variant, edge-blend QC, runtime lighting only |
```

CURRENT_STATUS S66 sonu bolumune ek satir:
```
- Karar #115 AI-Assisted Map Builder LOCKED: deterministic procedural baseline + designer brush polish; Faz 1.0 MVP 12-16h, Faz 1.5 polish 30-40h
- Karar #116 Tile Transition Quality Standard LOCKED: Raggedness/variant/edge-blend/runtime lighting kriterleri
- Karar #115 onaylama: kullanici Antigravity onerisini netlestirdi (oyun gibi = PixelLab Map editor uyarlamasi, fullscreen game-view DEGIL)
```

Active Priorities guncelle:
```
P0: F1 Shattered Keep tileset Aseprite cleanup + Unity import (Karar #116 quality bar)
P1: F1 props uretim (broken_pillar, rubble_pile, cracked_rune_stone, iron_cage) — tileset style reference ile
P2: Codex Faz 1.0 MVP RoomBaselineGenerator dispatch (Karar #115)
P3: PixelPerfectCamera ayar test (Karar #113 — GridSnapping/CropFrame/Orthographic)
P4: Combat FAZ 1.0 Unity playtest (InputBuffer+AttackToken+MercifulDodge)
```

### A4. TASARIM/room_authoring.md — Revize

Dosyaya yeni bolum ekle (var olan bolumlerin sonuna):

```markdown
## RIMA Map Builder (Karar #115 LOCKED)

### Mimari
- Unity Editor Window tabanli (mevcut F2 hotkey).
- Fullscreen "in-game editor" framing REJECTED — mevcut Editor Window kalir, brush UX ve toolbar polish ile evrilir.
- LLM/PixelLab API cagrisi YASAK (Karar #106 uyumlu) — tum AI baseline pure C# deterministic.

### Faz 1.0 Bilesenleri (MVP, 12-16 saat)
- `Assets/Scripts/Systems/Map/RoomBaselineGenerator.cs` — System.Random, GenerationInput kontrati.
- `Assets/Scripts/Systems/Map/RoomBaselineTemplate.cs` — ScriptableObject (biome, archetypeId, w/h araligi, floor variant weight, wall variant rules, anchor zone defaults).
- `RoomDesignerWindow` toolbar `btn-generate` butonu.
- Save akisi: RoomBlueprint asset + Room Prefab + RoomPrefabLink + **RoomConfig** (zorunlu, RoomLoader kontratina uyumluluk).
- FloorVariantPainter + WallAutoConnect bake entegrasyonu (mevcut sistemler cagrilir).
- byte[] grid + LUT variant metadata (mevcut RoomBlueprint.floorVariantIndex byte[] uyumlu).

### Faz 1.5 Bilesenleri (Polish, 30-40 saat)
- Inpaint Region brush mode — kilitsiz hucreleri re-seed, locked hucreler dokunulmaz.
- Force re-seed komutu — lock'lari yok sayar (explicit designer action).
- Anchor Zone painter — tile-mask + zone type enum + weight float. Save sirasinda Transform/child marker'a donusur.
- RenderTexture cache + repaint debounce.
- Preview kamera ~35 derece konverjans kalibrasyonu (Karar #113 uyumu).
- floorOverrideVariantIndex eklenmesi (wall icin mevcut overrideVariantIndex var, floor icin de gerekli).

### Exit Criteria Faz 1.0
- Ayni GenerationInput ile bit-identical RoomBlueprint uretilir.
- 5 farkli seed/biome/archetype generate, RoomLoader runtime hatasiz yukler.
- Designer manuel duzeltme orani %20 alti.
- Save edilen prefab RoomConfig referansi tasir, RoomLoader RoomConfig-missing hata atmaz.
- UnityEngine.Random global state generator cagrisi sirasinda degismez.

### Naming Netlestirme
"8 wall variant" terminolojisi yanlis okunabilir. Dogrusu: **4-bit NSEW mask → 8 wall connection tile variants** (her komsuluk kombinasyonu icin ayri tile). Karakter 8 yon animasyonu (Karar #114) ile karistirilmamali — ayri domain.

### REJECTED Listesi
- Fullscreen "oyun gibi" in-game editor (Antigravity onerisi, scope yutucu)
- LLM runtime/editor cagrisi
- PixelLab Inpaint API cagrisi (Karar #106 ihlali)
- PNG export (RIMA prefab tabanli)
- Runtime procedural 15-node placement override (Karar #62 ihlali)
- RoomLoader secim mantigi bypass
- Rect/polygon anchor schema (tile-mask kullanilir)
- UnityEngine.Random global state kullanimi (determinism)
```

### A5. TASARIM/GDD.md — Yeni Pipeline Bolumu

Uygun yere (Pipeline veya Tools bolumune) ekle:

```markdown
## AI-Assisted Map Builder Pipeline (Karar #115)

1. Designer Unity Editor Window'da F2 ile RoomDesigner acar.
2. Toolbar'dan seed + biome + archetype secer → Generate butonu.
3. `RoomBaselineGenerator` deterministic baseline uretir (floor variant grid, wall NSEW mask, anchor zone tile-mask, prop anchor candidate marker).
4. Designer brush'larla (Stamp/Eraser/Picker/Bucket + Faz 1.5 Inpaint Region/Anchor Zone) zone'lari boyar; kilitsiz hucreler re-seed edilebilir.
5. Save → RoomBlueprint asset + Room Prefab + RoomPrefabLink + RoomConfig olusturulur.
6. Runtime'da RoomLoader degisiklik olmadan yukler; mob spawn + prop placement Karar #62 prosedurel sistem tarafindan yapilir.
7. LLM/PixelLab API runtime'a hic dokunmaz; tum AI is Editor-only.
```

## Bölüm B — Cleanup (Sonnet Deep Scan Raporu Uygulamasi)

### B1. SİL (içerik tamamen yok, gercek deger yok)

```bash
rm "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_deep_research_2026-05-12.md"
rm "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_ai_criticism_2026-05-12.md"
```

### B2. ARŞİVLE (TASARIM kararlari, MASTER'a islenmis, tarihsel deger)

```bash
mkdir -p "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/BIG_DESIGN_DECISIONS_2026-05-13.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/room_designer_f2_ux.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/STYLE_BIBLE.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/VISUAL_QUALITY_STANDARDS.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/"
```

### B3. ARŞİVLE (STAGING araştırma dosyalari)

```bash
mkdir -p "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_research_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/hero_siege_hammerwatch_research_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/asset_production_plan_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/pixellab_research_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/nlm_boss_design_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/nlm_mob_design_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/nlm_skill_system_2026-05-12.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"
```

### B4. ARŞİVLE (MEMORY duplicate)

```bash
mkdir -p "F:/Antigravity Projeler/2d roguelite/RIMA/MEMORY/_archive"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/MEMORY/pixellab_master_pipeline.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/MEMORY/_archive/"
```

### B5. ARŞİVLE (STAGING/lighting research → nugget çıktıktan sonra)

```bash
mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/DUNGEON_LIGHTING_GENERATION_RESEARCH.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"

mv "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/alabaster_dawn_polish_ref.md" \
   "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/"
```

### B6. YENİ OLUSTUR — TASARIM/REF_NUGGETS.md

İçerik (tam):

```markdown
# RIMA — Polish & Inspiration Nuggets

> Konsolide ilham kaynaklari. Buyuk arastirma dosyalarindan damitilmis.
> Son guncelleme: 2026-05-13 (S66)
> Original kaynak dosyalar: STAGING/_archive/

## 1. Game Feel / Combat Juice (Alabaster Dawn referansi)

- **Blob shadow:** multiply blend %30-40 opacity, karakter Y offset → shadow alpha + scale lerp
- **Hit-stop:** hafif = 1 frame, agir = 2-4 frame, particle/trail unscaled time ile devam
- **Landing dust + squash/stretch:** 6-8 px fan-out 0.15s, scale Y 0.85 → 1.0 hizli donus
- **Damage numbers:** TMPro world-space, beyaz/sari/yesil/element renk kodu, 0.6-0.8s fade
- **Dash trail:** Trail Renderer time=0.08-0.1s additive VEYA 2-3 ghost sprite %30-50 opacity
- **Parallax 3-4 katman:** 0.1x / 0.3x / 0.6x / 1.0x, yatay + dikey
- **White health bar lag:** hasar → kirmizi anida, white bar ~0.5s gecikmeli (HK tarz)
- **Skill micro zoom-out:** scale 1.02x 0.1s impact

## 2. Dungeon Lighting Spec (DUNGEON_LIGHTING_GENERATION_RESEARCH)

- **Global Light 2D base:** #1E2030 to #262838, intensity 0.18-0.32
- **Torch:** #C8A96E intensity 0.75-1.10 outer 3.25-4.50 flicker ±0.08-0.16
- **Rift Crystal:** #00FFCC intensity 0.45-0.80 inner 0.50-0.90 pulse
- **Candle:** #E0C58D intensity 0.25-0.45 outer 1.20-2.10
- **Floor depth bands:** F1 torch fill %70-90 / F2 %40-65 (broken fixture ±%20-35) / F3 %10-30
- **Kural:** Her Point Light 2D'nin gorunur prop sahibi olmali. Floating light YASAK.
- **PropSpec alanlari:** emitsLight, lightSourceKind, requiresVisibleSource, depthBand weights
- **Anchor tag listesi:** WallLight_N/E/S/W, FloorLantern, RiftAccent, CandleCluster, NoLightZone, CriticalCombatClearance

## 3. PixelLab Pipeline Ek İpuçlari

- **vary_object seed-locked:** sabit seed + style_options outline+shading → mob varyant tutarliligi
- **128px boss 4-referans limiti:** anchor olarak en "saf" 64px asset sec, style_guidance_weight yuksek
- **create_map_object background_image:** prop uretiminde zemin tile referansi ver, stil tutarsizligi onler
- **Mid-stride seed recovery:** walk loop stiff gorunuyorsa neutral idle yerine mid-stride frame'den seed'le
- **Frankensprite teknigi:** AI animasyon + static kafa Aseprite paste (kafa-yon stabilizasyonu)
- **Edit Image Pro tile refinement:** AI tile her zaman plastik/flat → "muted, dark gritty palette, no gradients, heavy texture" ek prompt

## 4. Hero Siege / Hammerwatch / Cinderia (Referans Onayi)

- **64px chibi sweet spot:** Hero Siege oranlari (64px karakter / 32px tile) PixelLab icin ideal (Karar #100 kanitlandi)
- **Aesthetic match ranking:** HLD 9/10, Cinderia 9/10, Hero Siege 8/10
- **Karanlık ortam + parlak skill VFX:** glow embedding SPRITE icinde YASAK; Unity URP 2D Bloom + Particle engine-side
- **Zemin tile satürasyon:** karakterden her zaman daha dusuk satürasyon ve soguk ton — zemin goz yormamali
- **%60 padding bosluğu:** slash trail ve buyu VFX icin Unity icinde kullanilir, asla doldurmayin
```

### B7. MASTER_KARAR_BELGESI.md — Cross-Ref Düzeltme

Şu satırlarda referans varsa arşiv path'e güncelle (varsa, yoksa atla):
- `BIG_DESIGN_DECISIONS_2026-05-13.md` → `_ARCHIVE_2.5D_2026-05-12/BIG_DESIGN_DECISIONS_2026-05-13.md`
- `SKILL_AUDIT_DECISION_2026-04-30.md` → `_ARCHIVE_2.5D_2026-05-12/SKILL_AUDIT_DECISION_2026-04-30.md`
- `STYLE_BIBLE.md` → `_ARCHIVE_2.5D_2026-05-12/STYLE_BIBLE.md`
- `VISUAL_QUALITY_STANDARDS.md` → `_ARCHIVE_2.5D_2026-05-12/VISUAL_QUALITY_STANDARDS.md`

### B8. SKILL_POOLS_10CLASS_2026-05-07.md — Ghost Attack REVOKED Notu

Ghost Attack Z/X slot referanslari geçen satirlarin altina (ilgili her bolumde):
```
> **REVOKED 2026-05-13:** Ghost Attack Z/X slot sistemi Karar #60 + SKILL_SYSTEM_v2 ile kaldirildi. Shadowblade icin sadece Shadow Echo kalir.
```

### B9. Doğrulama Komutlari

Tum operasyonlar bitince calistir:
```bash
echo "=== SİL doğrulama ==="
ls "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_deep_research_2026-05-12.md" 2>&1 | head -2
ls "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_ai_criticism_2026-05-12.md" 2>&1 | head -2

echo "=== TASARIM/_ARCHIVE_2.5D_2026-05-12/ icerik ==="
ls "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/_ARCHIVE_2.5D_2026-05-12/" | head -20

echo "=== STAGING/_archive/ icerik ==="
ls "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_archive/" | head -20

echo "=== MEMORY/_archive/ icerik ==="
ls "F:/Antigravity Projeler/2d roguelite/RIMA/MEMORY/_archive/" | head -5

echo "=== REF_NUGGETS olustu mu ==="
ls -la "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/REF_NUGGETS.md" | head -2

echo "=== MASTER_KARAR_BELGESI #115 #116 var mi ==="
grep -E "^\| #11[56] " "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/MASTER_KARAR_BELGESI.md" | head -2
```

## Çıktı (CODEX_DONE.md'ye append)

```markdown
# S66 Final Lock + Cleanup Pack — TAMAMLANDI

**Tarih:** 2026-05-13

**Yeni Kararlar LOCKED:**
- #115 AI-Assisted Map Builder (Unity Editor F2 + procedural baseline + brush polish, REJECTED list inline)
- #116 Tile Transition Quality Standard (Raggedness >=40, 3+ variant, edge-blend, runtime lighting only)

**Doc Güncellemeler:**
- TASARIM/MASTER_KARAR_BELGESI.md (#115 + #116 satirlari, cross-ref arsiv path)
- TASARIM/room_authoring.md (Map Builder bolumu)
- TASARIM/GDD.md (AI-Assisted Map Builder Pipeline)
- CURRENT_STATUS.md (yeni Kararlar tablosu + S66 sonu satirlari + Active Priorities)
- TASARIM/SKILL_POOLS_10CLASS_2026-05-07.md (Ghost Attack REVOKED notu)
- TASARIM/REF_NUGGETS.md (YENI dosya, 4 bolum konsolidasyon)

**Silinen:**
- STAGING/cinderia_deep_research_2026-05-12.md
- STAGING/cinderia_ai_criticism_2026-05-12.md

**Arşivlenen (TASARIM/_ARCHIVE_2.5D_2026-05-12/):**
- BIG_DESIGN_DECISIONS_2026-05-13.md
- ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md
- room_designer_f2_ux.md
- SKILL_AUDIT_DECISION_2026-04-30.md
- STYLE_BIBLE.md
- VISUAL_QUALITY_STANDARDS.md

**Arşivlenen (STAGING/_archive/):**
- cinderia_research_2026-05-12.md
- hero_siege_hammerwatch_research_2026-05-12.md
- asset_production_plan_2026-05-12.md
- pixellab_research_2026-05-12.md
- nlm_boss_design_2026-05-12.md
- nlm_mob_design_2026-05-12.md
- nlm_skill_system_2026-05-12.md
- DUNGEON_LIGHTING_GENERATION_RESEARCH.md
- alabaster_dawn_polish_ref.md

**Arşivlenen (MEMORY/_archive/):**
- pixellab_master_pipeline.md (8 yon celiski sebebiyle acil arsiv)

**Doğrulama:** ✓ Tum operasyonlar PASS

**Sonraki orchestrator adim:** NLM sync paketi (Master+CURRENT+room_authoring+GDD+REF_NUGGETS) ve Codex Faz 1.0 MVP dispatch (RoomBaselineGenerator).
```

## Kısıtlar

- git commit YOK
- Türkçe dosya/karar metni
- LOCKED kararlari sorgulama
- Yazma/silme/tasima tamamlanmadan sonraki adima gecme
- Effort: medium (uzun mekanik task)
