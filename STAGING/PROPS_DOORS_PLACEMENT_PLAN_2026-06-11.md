# RIMA — Props / Doors / Landmarks / AI-Placement + Environment Background — PLAN (Opus, 2026-06-11)

> **PLAN-ONLY.** Kaynak: ChatGPT paketi `Downloads/RIMA_Props_Doors_AI_Placement_PLAN_ONLY (1).zip` (8 dosya, okundu) → RIMA gerçek koduyla (3 Explore agent ground-truth) yeniden temellendirildi.
> **Execute = SONRAKİ session, cx (Codex) ile** (kullanıcı Codex limiti yenilenince). Bu session kod YAZMADI / Unity DEĞİŞMEDİ / asset ÜRETMEDİ.

---

## 0. EN ÖNEMLİ BULGU — ChatGPT planı greenfield varsaydı, RIMA'da sistem ZATEN VAR (~%80)

ChatGPT "şunları inşa et" diye 10 parça istedi. RIMA'da bunların çoğu **kodlu + test'li**. İş yeniden-inşa değil; **içerik + wiring + küçük boşluklar**.

| ChatGPT'nin "inşa et"i | RIMA gerçek durumu | Dosya |
|---|---|---|
| PropDefinitionSO | ✅ VAR (footprint, collision, roller, variant, seed) | `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs` |
| PropRegistry | ✅ VAR (GUID→prop, editor auto-index) | `.../Props/PropRegistrySO.cs` |
| Collider auto-build | ✅ VAR (footprint'ten BoxCollider2D, rotasyon) | `.../Props/Runtime/PropColliderAutoBuilder.cs` |
| AI placement | ✅ VAR — **`BridsonPoissonAutoPlacer` SEEDED** (Poisson-disk, rol-yoğunluk, walkable-saygı, flipX) | `.../Props/Auto/BridsonPoissonAutoPlacer.cs` |
| Validator | ✅ VAR (bounds/walkable/rol/overlap/mesafe, 6 sonuç) | `.../Props/PropFootprintValidator.cs` |
| Zone detection | ✅ VAR — `CompositionRoleMap` (WallBand/DoorSafety **radius=3**/DecoratedEdge/CleanCenter/FocalCluster) | `.../Composition/CompositionRoleMap(Generator).cs` |
| Runtime spawn + Y-sort | ✅ VAR | `.../Props/Runtime/PropRuntimeSpawner.cs`, `PropSorterRuntime.cs` |
| Editor placer | ⚠️ KISMİ — `PropsTab` manuel click-place var; auto-placer-önizleme + overlay-painter UI yok | `.../Props/Editor/PropsTab.cs` |
| Metadata şema | ✅ VAR (aşağıda §5'te ChatGPT alanları ↔ mevcut alanlar map'lendi) | — |
| Mevcut prop asset | ✅ ~17 Act1 prop (brazier/lever/urn/rubble/skull/spike/treasure/barrel) + ağırlıklı pool'lar | `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/`, `Assets/Data/Blueprint/PropPools/` |

**Sonuç:** Plan = mevcut altyapıyı **bağla + içerikle besle**, paralel sistem KURMA. ChatGPT'nin önerdiği yeni `Assets/Art/RIMA/...` klasör ağacı = mevcut yapıyla çakışır → kullanma (§3).

---

## 1. FİNAL PIPELINE ÖNERİSİ

```
RoomTemplateSO (oda tanımı + walkableGrid + doorSockets + spawn'lar)
   │
   ▼  IsoRoomBuilder.Build()  [floor→overlay→cliff→boundary→marker→props]
   │
   ├─(YENİ wiring) BuildDecorations(template, runSeed)   ← GEÇ-PASS, line 116 sonrası
   │     • CompositionRoleMapGenerator → zone grid (VAR)
   │     • BridsonPoissonAutoPlacer.Place(seed, roleMap, registry) (VAR) → aday liste
   │     • PropFootprintValidator.Validate() her aday (VAR) + YENİ dash-lane guard
   │     • geçerliler → PropRuntimeSpawner ile spawn (collider YOK, sadece görsel)
   │
   ▼  RoomRunDirector → EncounterController → Reward → Draft → Boss → Victory  (DOKUNMA)

PARALEL TRACK (oda'dan bağımsız):
   _Arena scene root → EnvironmentBackground (persistent) → ParallaxLayer ×5 (VAR, kullanılmıyor)
```

**İki ilke (ChatGPT constraint'leriyle uyumlu):**
- **Dekorasyon = GEÇ-PASS**, floor/wall/door üretildikten SONRA, deterministik (run seed) → her oynayışta aynı oda aynı görünür ama odalar birbirinden farklı.
- **Okunabilirlik > atmosfer:** dekor collider'sız (oynanışı bloklamaz); obstacle (collider'lı) ayrı ve seyrek; clearance'lar (kapı önü 3×3, spawn, reward, dash-lane) validator'da HARD-FAIL.

---

## 2. ASSET LİSTESİ — SPLIT (PixelLab küçük / Codex-Unity-imagegen büyük)

### 🔒 PROP BOYUT KATMANLARI (LOCKED 2026-06-11 — kullanıcı kararı)
Tek-boyut atlas DEĞİL; 4 katman (orantı merdiveni = "tasarlanmış oda" hissi). Hepsi **şeffaf bg, Point, PPU 64**. Her katmanın İLK prop'u VERIFY-LIVE (üret→Unity→boyut onay→batch).

| Katman | Rol | PixelLab canvas | Footprint (tile) | Pivot | Batch |
|---|---|---|---|---|---|
| **T1 Floor decal** | yere yatık | **32×32** | 1×1 | center | ~24-32 |
| **T2 Küçük prop** | ayakta küçük | **64×64** | 1×1 | bottom-center | ~16 |
| **T3 Focal prop** | göz-çeken orta | **96×96** (gerekirse 128) | 1×1 / 2×2 | bottom-center | ~8-12 |
| **T4 Landmark** | oda-kimliği büyük | **128×128 → 192×256** | 2×2 / 2×3 | bottom-center | tek tek / ~4-6 |

Üretim sırası (görsel kazanç): T1+T2 önce → T3 focal (brazier vb., animasyonlu brazier zaten var) → T4 demo-sonrası. Üretim=kullanıcı-gated; import/slice/pivot/SO=Claude.

### ODA TASARIMI (LOCKED 2026-06-11)
Küçük çıplak platform (chamber elması gibi) YOK. Demo combat odaları **≥24×18** (teardrop/donut/oval/cross/hourglass — DemoRoomBank'te var). "Güzel oda" = boyut + dekor-dolgu + ışık + 1 odak landmark + okunur kapı. Büyük+dolu+ışıklı (büyük+çıplak DEĞİL).

### A. PixelLab atlas (şeffaf) — KÜÇÜK DEKOR/DECAL (T1+T2, yukarıdaki katman boyutlarıyla)
ChatGPT'nin 64-listesi RIMA Act1 kanonuna uygun (cyan-rift + kemik=failed-containers + warm-orange brazier). 4 kategori × ~16:
- **Floor decals (1-12):** crack/chip/ash/stain/scratch/dust/broken-tile
- **Cyan rift decals (13-24):** seam/rift-crack/sealed-crack/rune-dot/glyph/portal-residue/elite-mark/boss-mark
- **Rubble/debris (25-36):** pebble/shard/rubble-pile/brick/gravel/urn-shard
- **Story + ritual/marker (37-64):** chain/shackle/bone/skull/broken-blade/cloth/scroll/candle/ember/brazier-base/offering-bowl/rift-crystal/door-marker N/E/W/reward-sparkle/shrine/forge mark

> Üretim = **kullanıcı-gated** ([[feedback-user-draws-weapons-claude-mounts]]). Yöntem = 32px obje-batch (64-ikon koşusundaki gibi tek/az çağrı), kullanıcı "sen üret" derse Claude PixelLab MCP'den üretir. Kanon ref: [[project-act1-environment-asset-canon]].

### B. Codex/Unity + imagegen — BÜYÜK gameplay/landmark asset
PixelLab 32px atlasına SOKMA. Boyut hedefleri (ChatGPT doc 02'den, RIMA'ya uyarlı):
- **Kapı görselleri** — NW/N/NE slot'ları (compass N/E/W DEĞİL; §4). 128×128, state: Locked/Open/Hover. **DÜŞÜK öncelik — kapılar zaten çalışıyor, bu sadece cila.**
- **Boss gate** 192×192 / 256×192 (boss arenası arkası)
- **Landmark prefab'ları** (oda-kimliği, 1/oda): RiftObelisk 128², ShrineAltar 128², ForgeAnvil 128², MerchantTent 160², BrokenThrone 160², LargePillar 96×128, RitualCircleLarge 192×96 decal.
- Üretim opsiyonu: imagegen 1024² silüet (on-brand, [[reference-ax-agy-cli-mechanism]]) VEYA PixelLab büyük-obje VEYA Codex prosedürel placeholder. Hepsi kullanıcı-gated.

---

## 3. UNITY KLASÖR YAPISI — MEVCUDU GENİŞLET (ChatGPT'nin yeni ağacını KULLANMA)

ChatGPT `Assets/Art/RIMA/Props/...` + `Assets/Scripts/RIMA/RoomDecoration/...` önerdi = mevcut yapıyla **çakışır, fragmentasyon**. Mevcut kanonik yerler:

| İçerik | MEVCUT yer (buraya koy) | ChatGPT'nin yanlış önerisi |
|---|---|---|
| Prop/decal sprite (yeni atlas) | `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/atlas_32/` (yeni alt-klasör) | ~~Assets/Art/RIMA/Props/SmallAtlas_32~~ |
| Landmark sprite | `Assets/Art/AssetPacks/Act1_ShatteredKeep/landmarks/` | ~~Assets/Art/RIMA/Props/Landmarks~~ |
| Kapı sprite | `Assets/Art/AssetPacks/Act1_ShatteredKeep/doors/` | ~~Assets/Art/RIMA/Doors~~ |
| Prop/Decor SO | Mevcut PropDefinitionSO asset'lerinin olduğu klasör (cx execute'ta `t:PropDefinitionSO` ile bulur) | ~~Assets/ScriptableObjects/RIMA/Props~~ |
| Dekor kodu | `Assets/Scripts/MapDesigner/Props/` (mevcut namespace) | ~~Assets/Scripts/RIMA/RoomDecoration~~ |
| Editor tool | `Assets/Scripts/MapDesigner/Props/Editor/` (PropsTab yanı) | ~~Assets/Editor/RIMA/RoomDecoration~~ |

---

## 4. IMPORT / PIVOT AYARLARI

**PixelLab 32px atlas:** Sprite(2D/UI) · Multiple · **Filter=Point** · Compression None/HQ · Mip Off · Alpha-Is-Transparency On · **PPU=64** (RIMA kanon, S59) · Pivot: floor-decal=center, ayakta-duran-tiny-prop=bottom-center.
**Büyük kapı/landmark:** aynı + Pivot: kapı/uzun-landmark=bottom-center, zemin-ritüel-decal=center.
> ⚠️ S59 LOCK: scale YASAK, crop OK. PPU=64 sabit. [[reference-pixelart-scaling]] (import Point, Bilinear değil).

---

## 5. METADATA ŞEMASI — ChatGPT alanları ↔ MEVCUT PropDefinitionSO

Mevcut `PropDefinitionSO` zaten taşıyor: `propId, displayName, icon, worldSprite, footprintSize, spriteAnchor, blocksWalkable, requiresWalkableTile, preferredRoles, forbiddenRoles, distanceFromOtherProps, variantSprites, sortingMode, collider(shape/ratio/offset)`.

ChatGPT'nin istediği ama EKSİK olan (additive, non-breaking — cx ekler):
- `category` (enum: FloorDecal / RiftDecal / Rubble / StoryProp / RitualMarker / Obstacle / Landmark) — placement filtreleme
- `allowedRoomTypes` (Combat/Elite/Boss/Shop/Shrine) — şu an sadece rol-bazlı; oda-tipi filtresi yok
- `maxPerRoom` (int) — spam önleme (şu an yoğunluk auto-placer rol-density'den; per-prop cap yok)
- `visualWeight` (float) — skorlama için (pool ağırlığı `BlueprintPropPoolSO`'da var ama prop-başı değil)

**Zone karşılığı:** ChatGPT'nin `allowedZones` (CenterCombat/EdgeDecoration/DoorClearance/...) = mevcut `CompositionRole` (CleanCenter/DecoratedEdge/DoorSafety/WallBand/FocalCluster) → YENİ alan gerekmez, `preferredRoles/forbiddenRoles` kullan.

**Dekor vs obstacle ayrımı:** İki opsiyon — (A) yeni `List<DecorationPlacementData> decorations` alanı RoomTemplateSO'ya (props=obstacle, decorations=görsel-only; net), VEYA (B) `PropPlacementData`'ya `PlacementType` enum flag. **Öneri = A** (collider mantığı net ayrışır, walkable-grid'i etkilemez).

---

## 6. AI PLACEMENT KURAL PLANI

Mevcut `BridsonPoissonAutoPlacer` + `CompositionRoleMap` ZATEN: Poisson-disk dağıtım, rol-yoğunluk (CleanCenter 0.1 / DecoratedEdge 1.0 / FocalCluster 2.0 / DoorSafety+Wall 0), walkable-saygı, mesafe-kuralı, flipX. **Wiring = bunu live `BuildCurrentRoom` akışına bağla** (şu an editor/test-time; live'da `spawnProps` Combat/Elite için KAPALI).

ChatGPT'nin HARD-FAIL kuralları → validator'a eklenecekler (çoğu var):
- ✅ kapı-önü-blok (DoorSafety radius=3 VAR), spawn-blok (DoorSafety exclusion), reward-blok, overlap, same-prop-spam (mesafe-kuralı VAR)
- ➕ **YENİ: dash-lane guard** — combat odası ≥2 temiz koridor (uzunluk 8+, genişlik 2+ tile); landmark-kapı-overlap; center-block sayacı.
- Skorlama (opsiyonel, 1-hafta): `visual_interest + edge_bonus + asymmetry + room_identity − door/spawn/dash/center/repetition penalty`.

**Yoğunluk profilleri** (doc 04 — oda-tipi başına): Combat decal 10-18 / Elite 8-14 / Boss 12-24 / Shop-Shrine 6-12. cx bunları per-room-type config'e koyar.

---

## 7. VALIDATOR PLANI

`PropFootprintValidator` VAR (bounds/walkable/rol/overlap/mesafe). Genişletme:
- ➕ Dash-lane kontrolü (combat odası, yukarıda).
- ➕ Landmark-vs-door overlap (büyük landmark kapı slotunu kapatamaz).
- ➕ "center-block" eşiği (combat merkezde max-obstacle).
- Mevcut 6-sonuçlu enum'a yeni fail-reason'lar eklenir. Test'ler genişletilir (mevcut `PropFootprintValidatorTests` paterni).

---

## 8. EDITOR TOOL PLANI

- ✅ `PropsTab` manuel placer VAR.
- ➕ **Auto-placer önizleme** — PropsTab'a "Auto-Decorate (seed)" butonu → BridsonPoisson çalıştır → scene'de göster → kabul/iptal.
- ➕ **Debug overlay** — CompositionRole grid'ini ve clearance/dash-lane'leri Gizmo ile çiz (doğrulama).
- ➕ **Overlay-mask painter UI** (opsiyonel polish) — `RoomTemplateSO.overlayMask` data var, editor UI yok.
- ➕ **Landmark placer** — 1-per-room landmark'ı LandmarkZone'a yerleştir.

---

## 9. ENVIRONMENT BACKGROUND — AYRI TRACK (kullanıcının açık isteği) ✅ ONAYLIYORUM

**Kullanıcının noktası DOĞRU + mimari olarak da daha temiz.** Background'u her map'in arkasına tek tek koymak yerine **environment-seviyesinde** vermeli; map değişse de environment kalır, environment (act/biome) değişince playtime'da değişir.

**Ground-truth:** Şu an background'lar **per-room** — `SubRoomSequenceController.PaintBackgroundLayers()` (line 175) her odaya child olarak basıyor, teardown'da yok ediliyor. `ParallaxLayer.cs` (5-tier preset L0-L4: void 0.03 / nebula 0.05 / ruins 0.08 / islands 0.14 / fog 0.10) VAR ama **hiç kullanılmıyor**. Environment-level persistent container YOK. `RoomTemplateSO.biomeId` var (data) ama runtime environment objesi yok.

**Neden environment-level doğru:**
1. **Perf** — 5 katman oda başına yeniden-instantiate edilmiyor, bir kez boyanıyor.
2. **Görsel süreklilik** — parallax oda-geçişlerinde doğru okunur (kamera-delta sürekli), per-room'da her odada sıfırlanır.
3. **Concern ayrımı** — oda = oynanış zemini (floor decal per-room KALIR), environment = ruh/derinlik (uzak katmanlar).

**Minimal değişiklik (cx execute):**
1. `_Arena` scene root'a persistent `EnvironmentBackground` GameObject (oda teardown'undan etkilenmez).
2. Biome/act başında bir kez `PaintEnvironmentBackground(biomeId)` → 5 uzak katman.
3. Her katmana `ParallaxLayer` component + tier factor (0.03-0.14).
4. `SubRoomSequenceController.PaintBackgroundLayers()` → **uzak katmanları çıkar** (floor-decal katmanı per-room kalabilir; sadece void/nebula/islands/fog environment'a taşınır).
5. Kamera (fixed 5.0 ortho + CameraFollow) zaten parallax'ı sürer — ek iş yok.

**Nüans:** `RoomTemplateSO.backgroundLayers` (Hades-stil per-room boyalı) ile gerilim var → **floor-yakın katman per-room, uzak-atmosfer katman environment-level** diye böl. cx execute'ta hangi katmanın hangi tarafta olduğunu netleştirir.

---

## 10. RİSK LİSTESİ

| Risk | Etki | Önlem |
|---|---|---|
| Dekorasyon clearance'ı bozar (kapı/spawn/reward/dash bloklanır) | Softlock / oynanamaz oda | Validator HARD-FAIL (var) + dash-lane guard (yeni) + her oda completable test (mevcut `RoomCompletionInvariantTests`) |
| `_Arena`/RoomRunDirector akışı bozulur | Demo kırılır | Dekorasyon = ek geç-pass, mevcut Build sırasına DOKUNMA; `spawnProps` flag'ı koru; feature-flag arkasında |
| Per-room→environment background taşıması parallax/sorting bozar | Görsel bozulma | Önce feature-flag'li, floor-decal per-room kalır, sadece uzak katman taşınır; play-verify |
| PixelLab atlas tutarsız stil/ölçek | Görsel kırılma | VERIFY-LIVE ilk asset, 32px+PPU64 sabit, kanon-prompt [[project-act1-environment-asset-canon]] |
| Landmark çok büyük/okunmaz | Atmosfer>okunabilirlik ihlali | 1/oda limit, LandmarkZone, silüet-kontrast hiyerarşisi (doc 05) |
| Codex limiti yetmez (büyük batch) | İş yarıda kalır | Görevleri küçük-fazlara böl (1g/3g/7g), her faz bağımsız commit |
| Drift: ChatGPT door N/E/W'yi base alır | Yanlış kapı asset | NW/N/NE konvansiyonu LOCK (§4), kapı işi düşük-öncelik |
| Mobil/PC perf (çok prop/katman) | FPS düşüşü | Yoğunluk cap'leri (doc 04), decal collider'sız, atlas tek-texture, parallax bir-kez-boya |

---

## 11. YOL HARİTASI — 1 GÜN / 3 GÜN / 1 HAFTA

### 🟢 1 GÜN — "Wiring, sıfır yeni art" (en hızlı görünür kazanım, en düşük risk)
1. **`BuildDecorations` geç-pass'i** IsoRoomBuilder.Build line 116 sonrası — mevcut `BridsonPoissonAutoPlacer` + `CompositionRoleMapGenerator` + `PropFootprintValidator` zincirini live akışa bağla, **mevcut 17 Act1 prop + pool'larla** (yeni art GEREKMEZ).
2. **Dash-lane + center-block guard** validator'a ekle; kapı-clearance (radius=3) zaten var.
3. **Feature-flag** (`enableAutoDecoration`) + deterministik run-seed.
4. **Debug overlay** (role/clearance Gizmo) + editör "Auto-Decorate" butonu.
- **Çıktı:** Live odalar mevcut prop'larla dekore, clearance korunuyor, 0 softlock. → **demo-relevant, demo-safe.**
- **Sahip:** cx (1-2 odak task). QC: Claude + `RoomCompletionInvariantTests`.

### 🟡 3 GÜN — "+Environment background +Atlas içerik"
5. **Environment-level parallax** (§9) — persistent container, ParallaxLayer wire, per-biome boya, per-room paint'ten uzak-katman ayır. Feature-flag'li, play-verify.
6. **PixelLab 64-atlas üretim** (kullanıcı-gated) + import/slice/pivot (PPU64/Point) + PropDefinitionSO asset'leri (cx batch).
7. **PropDefinitionSO additive alanlar** (category/allowedRoomTypes/maxPerRoom/visualWeight) + `DecorationPlacementData` ayrımı (§5 opsiyon A).
- **Çıktı:** Odalar çeşitli dekorlu + derinlikli kalıcı background. → demo-polish.
- **Sahip:** cx (kod) + kullanıcı/Claude (PixelLab). QC: Claude.

### 🔵 1 HAFTA — "+Landmark kimliği +Skorlama +Editor cila"
8. **Landmark sistemi** — 1 oda-kimliği landmark/oda-tipi (shrine/forge/merchant/boss-throne/rift-obelisk), LandmarkZone yerleşimi, large asset (imagegen/PixelLab/Codex). Boss arenası arka-taht silüeti.
9. **Yoğunluk profilleri** per-room-type (doc 04) + skorlama fonksiyonu (§6 opsiyonel).
10. **Overlay-mask painter UI** + landmark placer editor tool.
11. **Tam QC** — yeni test'ler (dash-lane, landmark-door, decoration-serialization), full suite, build smoke.
- **Çıktı:** "Shattered Keep floating arena diorama" hissi (doc 05). → post-demo derinleştirme.
- **Sahip:** cx (kod) + kullanıcı (large art). QC: Claude + rima-qc.

---

## 12. GÖREV DAĞILIMI (kim, neyi)

| İş | Sahip | Not |
|---|---|---|
| Plan/tasarım/karar (bu doc) | **Opus (ben)** | tamam |
| Kod execute (wiring, validator, env-bg, SO, editor tool) | **cx (Codex)** | sonraki session, STAGING/_process/'e task .md → `cx dispatch --effort high --timeout 3600` (L-task) |
| PixelLab 64-atlas üretim | **kullanıcı-gated** | "sen üret" → Claude MCP (32px obje-batch); değilse kullanıcı üretir |
| Atlas import/slice/pivot/SO | cx (batch) veya Claude MCP | PPU64/Point |
| Büyük landmark/kapı art | kullanıcı-gated | imagegen silüet / PixelLab / Codex placeholder |
| Bilgi-toplama (oda screenshot, prop envanter sayım) | **ax Gemini 3.5 Flash High** | ön-inceleme yapmadan direkt prompt [[feedback-ax-gemini-flash-mcp-info-gathering]] |
| QC (kod + görsel) | Claude + rima-qc | her cx batch sonrası |

---

## ÖZET (TL;DR)
- **Sistem var, sıfırdan yazma.** İş = (1) mevcut auto-placer'ı live akışa **bağla** (1 gün, sıfır art), (2) **environment-background** parallax'ı persistent yap (senin noktan, doğru), (3) **PixelLab 64-atlas** içerikle besle, (4) **landmark** kimliği ekle.
- **Kapı = düşük öncelik** (çalışıyor; NW/N/NE konvansiyonu, ChatGPT'nin N/E/W'si yanlış).
- **Demo-safe sıra:** wiring → env-bg → atlas → landmark. Flag'li, clearance HARD-FAIL, `_Arena` akışına dokunma.
- **Execute = cx, sonraki session.**
