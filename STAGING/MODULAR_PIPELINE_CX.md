# Modular PixelLab Production Pipeline

## 1. Katman ayristirmasi

Target look oda tek bir baked illustration olarak uretilmemeli. Oynanabilir prosedurel oda icin katmanlar ayrilmalidir:

1. Floor tile layer: IsoGrid/Ground uzerinde Tilemap. Ana granit zemindir. Gameplay collider yok veya mevcut floor collision kuralina bagli kalir. Cyan catlak, rune ve shock izi burada baked olmamalidir.
2. Cliff-edge + ada-alti layer: ayri Cliff/Edge Tilemap veya y-sortlu SpriteRenderer prefab seti. Amac odanin dis konturunu, kalin granit kenari, alt kaya kirigini ve void'e dusen hacmi vermek. Gameplay walkability floor mask'ten gelir; cliff sadece gorsel/engel okumasi verir.
3. Duvar-harabe layer: Wall Tilemap + y-sortlu wall prefab. Kisa arka duvarlar, kirik bloklar, gate arch, chain anchor, boss arena rimleri burada durur. Collider gerektirenleri prefab veya wall tile collider alir.
4. Props layer: Props root altinda y-sortlu prefablar. Sandik, portal ayagi, shrine, rubble, chain, pillar, rune slab, floating shard gibi objeler. RoomLoader decorProps ve AssetPackSO/RuntimeAssetRegistry ile cekilebilir.
5. Cyan VFX/decal layer: Decals Tilemap + Lighting root + particle/prefab. Cyan #00FFCC catlak, rune, shock-ring, portal swirl ve boss seal enerjisi bu katmandadir. Base floor atlasinda baked degildir. Bilesenler: transparent crack/rune sprites, additive/alpha SpriteRenderer materyali, Light2D point/freeform pool, particle burst prefab.
6. Character/boss layer: runtime entity prefab/sprite. Boss cyan flame, attack ring ve hit VFX karakter assetlerinden ayri tutulur; oda kiti sadece arena decal/light anchorlarini verir.
7. Void/parallax layer: Parallax veya background root. Mor void, uzak yildiz/parca ve map-node baglanti cizgileri oda tilemap'ine karismaz.

Cyan butce somut kural: her oda kompozisyonunda toplam piksel alani/ekran vurgusunun yaklasik %5-8'i cyan olsun. Zemin taslari, cliff ve wall ana degeri charcoal/blue-grey kalir. Cyan sadece (a) ince crack/rune decal tilelari, (b) Light2D pool alpha/intensity, (c) particle burst ve (d) portal/boss gibi odak prop emissive alanlarinda kullanilir. Floor texture icinde cyan kullanilacaksa sadece cok soluk, silinebilir guide varyasyonu olarak tutulur; final floor RuleTile clean granite kalir.

## 2. Asset-tipi -> PixelLab araci eslemesi

Floor:
- MVP ve prosedurel terrain icin ana tercih `create_topdown_tileset`. KB lock: Wang16 icin `create_topdown_tileset`, `create_tiles_pro` degil. Neden: gercek 16 terrain mask/metadata verir; `PixelLabWangImporter` bu metadata ile RuleTile uretebilir.
- Mevcut floor451 `create_tiles_pro` ile uretilmis ve dogrulanmis keeper oldugu icin style anchor/backup olarak kalir. Yeni prosedurel kitlerde floor451 hissi `style_images[]` olarak kullanilir; yeni Wang16 set ise `create_topdown_tileset` ile alinmalidir.
- `create_tiles_pro` sadece non-Wang floor varyasyon denemesi veya numbered decorative tile batch icin kullanilir.

Cliff-edge + ada-alti:
- Cliff top edge/corner logical seti icin iki parcali yol: kucuk tile overlay setleri `create_tiles_pro` numbered prompt ile, hacimli alt kaya yuzeyleri `create_1_direction_object` batch ile transparent sprite olarak.
- Tam Wang ihtiyaci varsa `create_topdown_tileset` ancak "upper=walkable top granite, lower=void/drop edge" gibi net terrain mask istenirse kullanilir. Sadece ada-alti hacmi icin Wang zorlamak gereksiz maliyet ve yanlis tile semantics uretir.
- Batch verimi: 64-85px edge/corner/chunk sprite adaylari `create_1_direction_object` ile 16 veya 64 aday/call olarak alinabilir.

Duvar-harabe:
- `create_1_direction_object` primary. Kirik wall segment, half-wall, cracked block, gate arch, pillar stump, chain anchor gibi transparent object setleri ayni style ref ile batchlenir.
- Tek hero duvar/gate gerekiyorsa `create_image_pro` kalite icin kullanilir, sonra daha kucuk varyasyonlar `create_1_direction_object` ile genisletilir.

Props:
- `create_1_direction_object` primary prop motoru. KB'ye gore size <=42 ise 64 aday/call, <=85 ise 16 aday/call; moloz, rune shard, chain, small shrine, brazier, chest variants icin en ucuz ve kontrollu yol.
- Buyuk portal frame, boss ritual core, shrine hero prop gibi tekil odak assetleri `create_image_pro` veya `create_map_object` ile yapilabilir; fakat prosedurel kit verimi icin once batch prop secilir.

Cyan VFX/decal:
- Static transparent crack/rune/decal sprites: `create_1_direction_object` veya `create_from_style_pro` style matched 16 varyasyon.
- Shock-ring/portal swirl gibi hero VFX frame source: `create_image_pro` tek sprite veya Web animate route. Oda pipeline dokumani icin asset uretimi yok; Unity tarafinda particle/Light2D prefab olarak paketlenmesi yeterli.

Character/boss:
- Boss base silhouette icin `create_character`/Web character workflow veya `create_image_pro` hero sprite; animasyon gerekiyorsa KB'deki state-anchored Web V3 workflow. Oda kiti bu asseti uretmez, sadece boss arena floor/decal/light anchorlarini tanimlar.

Void map nodes:
- `create_1_direction_object` ile kucuk floating island node batch; cyan baglanti line'lari Unity UI/LineRenderer tarafinda procedural veya sprite olarak tutulur. Node platformlari oda floor kitinden turlenmis mini island prop gibi davranir.

## 3. Stil tutarliligi

Concept image'leri dogrudan final asset degil, style reference olarak kullanilmalidir. Her PixelLab promptu RIMA Style Lock string ile baslar ve conceptlerden sadece palette, material breakup, silhouette ve cyan-violet accent oranini alir.

Style image stratejisi:
- Floor Wang16: `concept01_hero_room_ISO.png` + mevcut floor451 ornegi style refs. Init/reference strength 250-350 arasi: renk, stone cluster, painterly edge hissi gelsin; tile mask ve Wang topology bozulmasin.
- Cliff/edge: `concept01_hero_room_ISO.png` ve `concept03_sundered_beat_ISO.png`. Strength 350-500: kalin ada yan yuzeyi ve kirik silhouette korunur, ama batch adaylari serbest varyasyon alir.
- Reward/portal props: `concept05_portal_chest_ISO.png`. Strength 450-600: portal formu ve cyan glow kimligi daha siki tutulsun.
- Boss arena props/decal: `concept07_boss_arena_ISO.png`. Strength 400-550: circular rune/rim/chain language alinir, runic text baked okunabilir harf gibi urememeli.
- Void map nodes: `concept09_void_map_AGY.png`. Strength 300-450: node dili ve cyan thread alinir, fakat UI layout Unity tarafinda kurulur.

Init-strength karari KB skalasina gore:
- 0-300: sadece renk/stil. Tileable/Wang floor icin en guvenli aralik.
- 300-400: kaba sekil. Cliff/chunk/duvar icin iyi varsayilan.
- 400-600: varyasyon referansa yakin. Portal, chest, boss arena rune ring gibi kimlikli props.
- 600-900: kucuk edit. Bu pipeline'in ilk uretiminde kullanilmaz; tileability ve batch cesitliligi daralir.

HD -> PixelLab koprusu uygulanabilir ama sadece style bridge olarak: agy/Imagen konseptleri "north star"; PixelLab ciktilari crisp pixel art olacagi icin promptlarda `matte hand-pixeled clusters`, `hard pixel edges`, `no anti-aliasing` ve teknik palette dili korunur. HD konseptten birebir init ile asset cikarmak yerine dusuk/orta strength style_images kullanmak daha guvenli.

## 4. Unity montaj otomasyonu

Mevcut akisin kullanimi:

1. PixelLab output staging:
   - Wang floor: `STAGING/PixelLab/.../metadata.json` + PNG sheet.
   - Batch prop/decal: transparent PNG sheet veya tek PNG seti.
2. Import:
   - Wang floor icin `Assets/Editor/TileImport/PixelLabWangImporter.cs`: 4x4, 32px, 16 tile bekler; `metadata.json` parse eder; RuleTile asset uretir; PPU=32, point filter, alpha, uncompressed ayarlar.
   - 8x8 PNG sheet icin `PixelLabPngSheetImporter.cs`: 256x256 sheet, 64 sprite slice, RandomTile ve RuleTile uretir. Bu helper skeleton grid olusturuyor ama cellSize'i locked iso recipe degil; runtime recipe icin kaynak degil, import kolayligi olarak kullanilmali.
3. SpriteAtlas:
   - `PatchAtlasSpriteAtlasBuilder.cs`: PatchAtlasSO variants listesinden SpriteAtlas uretir. Decal/patch atlasi icin mevcut yol yeterli.
4. Registry:
   - `RuntimeAssetRegistryBaker.cs`: sprite/tile/prefab ve AssetPackSO entry'lerini `RuntimeAssetRegistry.asset` icine basar. `AssetPackSO.Entry` ile `category`, `registryTag`, `layer` explicit verilebildigi icin yeni kitleri bake etmeye uygundur.
5. Runtime/authoring use:
   - Floor/cliff TileBase entry'leri `RoomLayer.Floor` / `RoomLayer.Cliff`.
   - Props/portal/light prefablari `RoomLayer.Props` / `RoomLayer.Lighting`.
   - Decal sprites `RoomLayer.Decals` ve PatchAtlasSO veya prefab overlay olarak.
   - `LargeDungeonMapPainterBase` floorTiles/wallTiles pool'u tile array alir; `SetTilePool` ile depth/biome bazli swap yapabilir. `RoomLoader` daha cok room sequence prefab/decor/focal element akisini kurar.

Eksik minimum tooling listesi, kod degil:
- PixelLab kit manifest standardi: her generation batch icin source image refs, tool, prompt, strength, cost, selected candidate id, Unity target layer.
- Wang importer output folder preset: ShatteredKeep path'ine dogru RuleTile/asset cikarmasi icin editor preset veya menu parametresi.
- Cliff/edge kit assembler: selected cliff sprites -> prefab/TileBase + AssetPackSO entries. Mevcut baker entries'i okuyabilir; eksik kisim uretimden sonra entry listesini hizli doldurma.
- Cyan decal/light pairing preset: decal sprite ile Light2D intensity/radius/default sorting bilgisini ayni prefabda tutan kucuk authoring standardi.
- RuntimeAssetRegistryBaker scan root guncelleme: yeni `Assets/Art/AssetPacks/Act1_ShatteredKeep/...` veya secilecek canonical folder bake kapsaminda degilse scan root'a eklenmeli ya da AssetPackSO entries ile guvenli sekilde dahil edilmeli.

## 5. Arketip kit yapisi

Canonical kit yapisi:

```
Assets/Art/AssetPacks/Act1_ShatteredKeep/
  ShatteredKeep_Base/
    Tiles/Floor_Wang16/
    Tiles/Cliff_Edge/
    Sprites/Walls/
    Sprites/Decals_Cyan/
    Sprites/Props_Common/
    Prefabs/Walls/
    Prefabs/Cliff/
    Prefabs/Decals_Lights/
    AssetPacks/AP_ShatteredKeep_Base.asset
    Atlases/PA_ShatteredKeep_Decals.asset
  Archetypes/
    Combat/
      Prefabs/
      AssetPacks/AP_ShatteredKeep_Combat.asset
    Boss/
      Sprites/Runes_Chains/
      Prefabs/BossArena/
      AssetPacks/AP_ShatteredKeep_Boss.asset
    Reward/
      Sprites/Portal_Chest/
      Prefabs/
      AssetPacks/AP_ShatteredKeep_Reward.asset
    Hub/
      Sprites/Shrine/
      Prefabs/
      AssetPacks/AP_ShatteredKeep_Hub.asset
```

SO organizasyonu:
- Base kit: one `AssetPackSO` with `packId=act1_shattered_keep_base`, floor RuleTile entries tagged `floor`, cliff entries tagged `cliff`, common props tagged `prop`, cyan overlays tagged `decal`, light prefabs tagged `light`.
- Archetype packs: one `AssetPackSO` per archetype. Boss pack contains rune circle decal/prefab, chain anchors, boss arena rim, cyan flame anchors. Reward pack contains portal/chest/floating shard. Hub pack contains shrine, safe-room pedestal, ambient seal lights.
- PatchAtlasSO:
  - `PA_ShatteredKeep_Decals_Cyan`: role OrganicDecal/Accent, variants crack/rune/rim sprites, density low, center reduction high.
  - `PA_ShatteredKeep_Rubble`: role DetailScatter, edgeBiased true, minDistance tuned.
  - `PA_ShatteredKeep_BossRunes`: role Accent, rotate transforms restricted if symbols must read as abstract marks.
- RoomRecipe/RoomVisualProfile:
  - RoomRecipe points to floor terrain, transition/decal/accent atlases and scatter brush.
  - Procedural room-builder chooses base kit always, then merges archetype pack by RoomType: Combat, Boss, Chest/Reward, Merchant/Hub.
- RoomLoader/LargeDungeonMapPainter contract:
  - LargeDungeonMapPainter consumes TileBase pools for floor/wall/cliff visual pass.
  - RoomLoader consumes focalElementPrefab/decorProps for archetype anchors, portal/chest/boss room focus.
  - RuntimeAssetRegistry is the lookup spine so room JSON/authoring can reference stable GUIDs instead of path strings.

## 6. Uretim sirasi + maliyet

MVP oda hedefi: tek playable Shattered Keep combat/reward-capable room. Icerik: clean granite Wang16 floor, reusable cliff/edge illusion, 1 broken wall mini-set, common rubble props, cyan crack/rune decal pack, one reward portal/chest focal pair, basic cyan Light2D prefab standardi.

PixelLab call sirasi ve yaklasik gen maliyeti:

1. Floor Wang16 base terrain
   - Tool: `create_topdown_tileset`
   - Inputs: floor451 + concept01 as style refs, strength 250-350
   - Output: 16 tile PNG + metadata.json
   - Cost: 20-40 gen
   - Reason: RuleTile-ready procedural terrain foundation.

2. Cliff edge/corner and island underside candidates
   - Tool: `create_1_direction_object`, size 64-85, item_descriptions per candidate
   - Output: straight edges, convex/concave corners, broken side faces, underside chunks, floating shards
   - Cost: 20-40 gen/call
   - Reason: batch-efficient and transparent object format fits y-sort/edge prefabs.

3. Broken wall + ruin kit
   - Tool: `create_1_direction_object`, size 64-85
   - Output: cracked half wall, corner wall, fallen blocks, pillar stump, gate arch fragment
   - Cost: 20-40 gen/call
   - Reason: wall silhouettes need batch choice, not tileability.

4. Common prop/rubble pack
   - Tool: `create_1_direction_object`, size <=42 for small rubble or <=85 for larger props
   - Output: 16-64 candidates: rubble, chains, rune stones, small shrine fragments, loose slabs
   - Cost: 20-40 gen/call
   - Reason: strongest cost-per-candidate path.

5. Cyan decal pack
   - Tool: `create_1_direction_object` or `create_from_style_pro`
   - Output: transparent crack lines, rune marks, circular seal fragments, shock-ring quarter arcs
   - Cost: 20-40 gen/call
   - Reason: keep cyan separate from floor and reuse with Light2D.

6. Reward focal pair
   - Tool: `create_1_direction_object` for chest/portal variants; `create_image_pro` only if selected portal hero quality is insufficient
   - Output: portal frame, chest, portal glow sprite/decal
   - Cost: 20-40 gen for batch; optional +20 gen for hero image
   - Reason: reward room identity needs one strong focal prop but still benefits from variants.

7. Boss arena add-on, after MVP passes
   - Tool: `create_1_direction_object` for chains/rim pieces + `create_image_pro` for boss rune circle if needed
   - Output: circular rune ring decal pieces, chain anchors, boss seal VFX source
   - Cost: 20-40 gen/call, optional +20 gen hero
   - Reason: boss kit is archetype-specific; not needed before base room is proven.

MVP cost envelope:
- Minimum: floor Wang16 20-40 + cliff batch 20-40 + wall batch 20-40 + prop/decal combined if tightly scoped 20-40 = about 80-160 gen.
- Practical first pass: add separate cyan decal and reward focal calls = about 120-240 gen.
- Boss add-on: +40-100 gen depending on whether hero image is required.

Quality gate order:
1. Import floor via Wang importer and verify IsoGrid cellSize (0.96, 0.585, 1), no squash.
2. Assemble cliff edge around a rectangular/irregular floor mask and verify readable floating island silhouette.
3. Add cyan decal/light pass and cap cyan to %5-8 visual area.
4. Bake RuntimeAssetRegistry, confirm tags/layers: floor, cliff, prop, portal, light, decal.
5. Let RoomLoader/LargeDungeonMapPainter pull base kit + one archetype pack in a test room.
