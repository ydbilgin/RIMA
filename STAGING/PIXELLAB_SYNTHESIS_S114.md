# PixelLab Envanter Sentezi (S114, 3-AI converge)

**Tarih:** 2026-05-29
**Yöntem:** Opus (visual/brand) + Codex (teknik/codebase) + agy (endüstri/mimari) bağımsız analizleri
**Kural:** 3/3 hemfikir → kesin verdict. 2/1 → çoğunluk + "CONFLICT" işareti. 3 farklı → REVIEW + üç görüş

---

## Özet Sayılar

| Verdict | Grup/Obje Sayısı | Tekil Obje Tahmini |
|---|---|---|
| KEEP-T1 | 10 grup | ~37 obje |
| KEEP-T2 | 14 grup | ~109 obje |
| DELETE | 8 grup | ~51 obje |
| REVIEW | 9 grup | ~46 obje |
| **TOPLAM** | | **~243 obje** |

**Kesin çelişki noktaları:** 6 grup (kullanıcı kararı gerekiyor)

---

## KEEP-T1 Tablosu

| Grup/Obje | Obje Adet | Gerekçe | 3-AI Uyum |
|---|---|---|---|
| walls_s95 (act1_wall_pieces + pilot_a + structural_v1) | 16 | Taş + cyan rift fracture brand-locked. abf9c178, 7daff11c arch örnek. Hepsi on-brand cool grey. | 3/3 KEEP — Opus: visual flagship / Codex: import-ready, PPU uyumlu / agy: endüstri standardı |
| walls_batch1 (RIMA_Wall_Production_v1_Batch1) | 4 | S112+ üretim, darker palette, production-ready. | 3/3 KEEP — Opus: on-brand / Codex: 96x96 pipeline uyumlu / agy: Hades pattern uyumlu |
| painterly_hand 128x128 | 5 (29537438, f9544ccc, 734280a8, d536f49c, e529d0a2) | Cyan rift portal explosion frames. GOLD-TIER VFX — hero piece. Skill ulti VFX veya boss-room flagship. | 3/3 KEEP-T1 — Opus: "HERO TIER" / Codex: import scope öncelik / agy: VFX katmanı için zorunlu |
| weapons_1dir tagged (5 class) | 5 (31ee0f73, 4bde2642, a032d9b5, 9312ea86, 894bba4a) | Class-tagged, kullanıma hazır 1-dir sprites. HandAnchor sistemine uyumlu. | 3/3 KEEP — Opus: class-locked / Codex: WeaponDatabaseSO mapping mevcut / agy: 1-dir+HandAnchor model doğru |
| vfx_anim (hitspark + dash trail) | 2 (11127e69, 58c183a0) | Hitspark 7f cyan burst + dash trail 9f — Faz 1 demo combat juice için CORE. | 3/3 KEEP-T1 — Opus: "MUST" / Codex: anim import öncelik / agy: VFX katmanı temel |
| statues_64x64 (act1_statue_ritual_s95) | 14 | Grey ritual stone, monolith/altar/obelisk variants. Cool grey, on-brand. Local: 11 zaten indirilmiş. | 3/3 KEEP — Opus: on-brand / Codex: 29 prefab içinde 14'ü local+wrapper hazır / agy: dungeon dressing standart |
| concept_mockup b55849ca + 25c6ab46 | 2 | b55849ca = cyan-rune-golem vs warrior GOLD mood board. 25c6ab46 rogue+cyan-circle brand-yakın. | 3/3 KEEP (design ref) — Opus: "flagship" / Codex: ref/moodboard klasörüne taşı / agy: görsel stil kılavuz |
| scatter_floor1 (16 complete) | 16 (4b320e3d vb.) | Dirt/rubble/moss/stones x4 — scatter brush distribution için ideal. Cool grey/green on-brand. | 3/3 KEEP — Opus: atmosphere dressing / Codex: import pipeline hazır (32x32/48x48) / agy: zemin katmanı standart |
| keep_decal_v2 (8 complete) | 8 (21406da0 vb.) | Cyan thin crack + dark floor overlays — atmospheric decal. On-brand. | 3/3 KEEP — Opus: on-brand / Codex: 32x32 correct size / agy: overlay katmanı için uygun |
| alabaster_decal (4 complete) | 4 (5ccc5721, 3b41f8eb vb.) | Tiny crack decals, sade overlay. 1 tanesi zaten local. | 3/3 KEEP — tüm AI'lar onaylı |

---

## KEEP-T2 Tablosu

| Grup/Obje | Obje Adet | Gerekçe | 3-AI Uyum |
|---|---|---|---|
| mounting_apparatus (15 complete) | 15 | Sword, banner, torch lantern, vines vb. Sahne dressing çeşitliliği. Local: 15 zaten indirilmiş. | 3/3 KEEP — Opus: karışık ama subset keep / Codex: 15 local+prefab hazır, D2 backfill / agy: prop layer standart |
| room_decor_misc (13+ obje) | 27 (5699e87d–36f3331f aralığı) | Wood crates, stone floors, cracks, dust, skeleton, cage, torch, altar, pillar vb. Atmospheric on-brand. | 3/3 KEEP-T2 — Opus: "import-ready atmosphere" / Codex: cloud-only, import önceliği orta / agy: dressing katmanı |
| painterly_hand 64x64 | 5 (a5dd2437, c88143c5 vb.) | Mini rift portal icon, cyan starburst hitspark alternatif. UI/skill icon placeholder olarak kullanılabilir. | 2/3 KEEP — Opus: on-brand / agy: UI standart uyumlu / Codex: SkillOfferUI şu an placeholder çiziyor (CONFLICT: import öncesi UI wiring gerekli) |
| painterly_topdown 64x64 (KEEP alt kümesi) | ~10 (22e83d26 kesin + diğerleri) | 22e83d26 cyan rift wall seam mükemmel. c8e3ba1a dark cluster sade. Zemin dressing. | 2/3 KEEP-T2 — Opus/agy: on-brand / Codex: import-only after visual audit (CONFLICT: toplu audit önerir, bireysel check gerekli) |
| transforms (4 complete) | 4 (4793b916, 02cee97d, b204a08b, 9b562391) | RIMA wall corner pieces, dark grey on-brand. 01c8269f FAILED zaten yok. | 3/3 KEEP — Opus: on-brand / Codex: complete status / agy: duvar katmanı uyumlu |
| walls_misc (2 complete) | 2 (4c561563, 102ca46d) | Tall wall + low wall. Kullanım sahnesi belirsiz ama atmosferik. | 3/3 KEEP (low priority) — tüm AI'lar tutarlı |
| cliff_face (2a383ea6) | 1 | Vertical monolithic dark grey cliff — parallax candidate. On-brand. | 3/3 KEEP — Opus: parallax sahne uyumlu / Codex: 128x96 import hazır / agy: derinlik katmanı standart |
| props_crates (2 complete) | 2 (e27c411d, ce1d1144) | Wood crate iron banding — neutral atmosphere prop. | 3/3 KEEP-T2 — tüm AI'lar tutarlı |
| keep_wall_v2 (4 complete) | 4 (76693f8f vb.) | Dark warm grey weathered — limitli kullanım ama edge/border için ready. | 3/3 KEEP (low priority) — tüm AI'lar tutarlı |
| alabaster_wall (4 complete) | 4 (56cc237f vb.) | Smooth top-down dungeon wall — walless V1'de az kullanım ama hazır. | 3/3 KEEP (low priority) — tüm AI'lar tutarlı |
| mob KEEP alt kümesi | 6 (ee4439c9, de32fa37, 3b22bdfa, 9938f947, dd2c1909, ff768082) | Cyan jellyfish wisp, cyan slime, cyan/green wraith, lanky undead, cyan spider — roster candidate. | 3/3 KEEP — Opus: FractureImp/PenitentSovereign/HollowHulk/ShardWalker mapping / Codex: mob prefab hazır / agy: endüstri standard mob |
| misc_props KEEP alt kümesi | 4 (075242f4, 6b52751d, 60502d16, eea16a35) + f2ba1bed | Rubble heap, broken pillar, violet rift dust (accent), loose rubble, hairline fracture — on-brand atmosphere. | 3/3 KEEP-T2 — tüm AI'lar tutarlı |
| skill_icons_special (3) | 3 (ca29419d, a49fbc6c, 213a59b5) | Crushing blow (cyan), rift portal strike, spinning sword — brand-acceptable. | 3/3 KEEP — Opus: "KEEP" / Codex: 64x64 format doğru / agy: UI standart uyumlu |
| concept_mockups (diğer 7) | 7 (a271376e vb.) | Design reference / mood board değeri var. Gameplay sprite değil. | 2/3 KEEP-T2 — Opus/agy: ref değeri var / Codex: ref klasörüne taşı, cloud kapasitesi yeterli (CONFLICT: Codex "taşı+audit" önerir, Opus ikisi direkt KEEP der) |

---

## DELETE Tablosu

| Grup/Obje | Obje Adet | Gerekçe | Conflict? |
|---|---|---|---|
| weapons_8dir (3 piece) | 3 (441bccf0, 692f43ce, e84d8c62) | HandAnchor + OrientationSync sistemi 1-sprite/weapon kullanır. 8dir batch redundant. Class tag yok. | HAYIR — 3/3 DELETE (Opus: "redundant" / Codex: "not aligned with locked runtime model" / agy: "1-dir runtime doğru") |
| mob DELETE alt kümesi | 10 (457560c3, 510d2864, 92904369, a17d9fd4, 3b7b3d40, 709f2c76, b364028d, e8695fff, e42dd84f, 67dd4af5) | Off-roster generic D&D goblin/bat/rat/red-demon/dark-crab. Off-brand palette veya decor-not-mob mistag. | HAYIR — 3/3 DELETE (Opus: visual / Codex: roster-dışı / agy: endüstri standart mob roster kuralı) |
| skill_icons generic (20) | 20 (71a8ea2f–aa0b0f61, 3 special hariç) | VIOLET/PURPLE dominant palette — Hades Elysium V1 cyan #00FFCC brand kuralına TERS. SkillOfferUI zaten placeholder çiziyor; import öncesi UI wiring gerekiyor. | HAYIR — 3/3 DELETE veya recolor (Opus: "OFF-BRAND, delete+recreate cyan" / Codex: "SkillOfferUI UI'da tüketilmiyor, blind import gereksiz" / agy: "renk kodlaması cyan olmalı") |
| misc_props DELETE alt kümesi | 3 (5dbfb74a, 93130cc6, fd1ab1b9) | Hexagonal honeycomb (RIMA non-hex), sand drift (desert palette), pink dust cloud (pink off-palette). | HAYIR — 3/3 DELETE |
| awaiting-selection weapon reviews | 5 (c2eb31d0, b1a64418, a5996413, bc226196, aac5e466) | Tagged versiyonlarının duplicate review kopyaları. Web UI'da reject. | HAYIR — 3/3 DELETE (cloud silme kuralı: user web UI manuel reject) |
| tiles_review awaiting (2) | 2 (e07d289e, 2fae026e) | Review subset — tagged versiyonlar zaten var. Web UI reject. | HAYIR — 3/3 DELETE |
| keep_decal_v2 review (1) | 1 (3449f001) | Review duplicate. Web UI reject. | HAYIR — 3/3 DELETE |
| keep_wall_v2 review / alabaster review | 3 (93c905fa, e390287f, ab0f5ab4) | Review duplicates. Web UI reject. | HAYIR — 3/3 DELETE |

---

## REVIEW Tablosu

| Grup/Obje | Obje Adet | Neden Belirsiz | 3 Görüş |
|---|---|---|---|
| walls_weathered_large (b2703abf) | 1 | VIOLET tone sızmış. Yan yana walls_s95 ile renk kopukluğu. | Opus: REVIEW (renk tonu warm-morumsu); Codex: cloud-only, import öncesi visual check; agy: brand lock cyan kuralı ile çelişiyor — kullanıcı gözle kontrol etmeli |
| floor_large (a3f9fcf1, 1d73e775) | 2 | Tipik VIOLET sapma. Cool blue-grey ile uyumsuz. Diorama mockup gibi görünüyor. | Opus: REVIEW; Codex: import öncesi ton kontrolü; agy: zemin tile standart dışı boyut (192x128) — tile pipeline'a uymaz |
| weapons_1dir untagged (soul lantern, hammer) | 2 (afcab14c, 19693073) | Hangi class? Hexer secondary? Warblade ult? Tag eksik. | Opus: REVIEW (hangi class?); Codex: soul lantern→Summoner (32x32 too small), hammer→Ravager (class fantasy mismatch); agy: class binding belirsiz, önce canonik sınıf tespiti |
| weapons_untagged_extra (compound bow, two-handed greatsword) | 2 (ebc33ebf, c0509b93) | Bow = Ranger? Greatsword = Warblade backup? Canvas too small. | Opus: REVIEW (class binding belirsiz); Codex: bow→Ranger good semantic, greatsword→Warblade backup, ama canvas 64x64 too small for 128x256/128x192 spec; agy: sınıf onayı gerekli |
| painterly_topdown 64x64 (REVIEW alt kümesi) | ~7 | 17 topdown'dan Opus ~10 KEEP dedi; 7'si belirsiz. Başlıklar kısa ("Painterly pixel art, top-..."), bireysel görsel audit yok. | Opus: spesifik delete listesi "2. tur gerekli" / Codex: import öncesi audit / agy: gruplu import riski — her biri ayrı değerlendirmeli |
| tiles_rift_cliff (a5dbe36c, 886684b6) | 2 | rift_pool VIOLET swirl — cyan brand'e ters. cliff_drop ufak sliver, kullanım belirsiz. | Opus: REVIEW (rift_pool violet, regen gerekli); Codex: 32x32 tile, pipeline uyumlu; agy: rift pool cyan tonda yeniden üretilmeli |
| vfx_blood review (cbe06cc3) | 1 | Tek piece, blood damage feedback. Tarz tutarlılığı? | Opus: REVIEW (kullanıcı onayı gerekli); Codex: awaiting-selection, web UI confirm; agy: damage feedback için fonksiyonel |
| props_crates awaiting (7f531dcb) | 1 | Awaiting-selection — visual kalite bilinmiyor. | Opus: REVIEW (web UI confirm); Codex: awaiting-selection; agy: tag edilmemiş, confirm gerekli |
| scatter_floor1 awaiting (4) | 4 (03f64e79, 080951ef, 21a56fd6, 083c9827) | Awaiting-selection duplicates. Tagged versiyonlarla aynı görsel mi? | Opus: DELETE web UI / Codex: reject / agy: tagged set yeterli — 3'ü DELETE diyor ama awaiting-selection flow = kullanıcı web UI görsel confirm + eşler temiz değilse REVIEW |

---

## Çelişkiler — Kullanıcı Kararı Gerekenler

| Obje/Grup | Çelişki Konusu | Opus | Codex | agy | Öneri |
|---|---|---|---|---|---|
| **skill_icons generic 20'nin kaderi** | Delete mi, cyan recolor mu? | DELETE + recreate (32 ikon, 8 class x 4) = brand-coherent | SkillOfferUI.BuildSkillCard zaten placeholder çiziyor — import öncesi UI wiring zorunlu, bulk import gereksiz | 64x64 çizim doğru boyut, import 128x128'e upscale + downscale öner, renk recode cyan | Kullanıcı karar: (A) 20'yi sil, class başına 4 yeniden üret; (B) Web UI'da cyan recolor regen; (C) SkillOfferUI wire tamamlanana kadar askıya al |
| **weapons_1dir tagged: canvas boyutu** | Mevcut 32x32/64x64 canvas küçük | 5 class-tagged sprite KEEP, canvas yeterli | 31ee0f73 (64x64) Warblade için "too small for 128x256 directive" / 894bba4a, 9312ea86 (32x32) "too small, regenerate/pad" | 128x256 Warblade, 96x192 Hexer, 64x192 Ronin spec verir | Kullanıcı karar: mevcut küçük sprite'ları Faz 1 placeholder olarak kullan, sonra per-class canonical boyuta regen yap |
| **Hexer silahı: staff mi whip mi?** | 4bde2642 "curse staff" KEEP, ama memory Hexer = grimoire/scepter | KEEP curse staff as Hexer draft | whip mevcut kod spec → grimoire/totem/scepter canonical (memory `weapon_master_spec_10_class.md`) | NOT eklenmiş: "Hexer = Whip spekülatif, canonical = Grimoire / Cursed Totem / Scepter" | Kullanıcı karar: 4bde2642 curse staff Hexer placeholder kabul et, sonra canonical scepter/grimoire üret |
| **mob mapping onayı** | 6 KEEP mob hangi roster karakteri? | ee4439c9→FractureImp, de32fa37→FractureImp alt, 3b22bdfa→PenitentSovereign, 9938f947→PenitentSovereign alt, dd2c1909→HollowHulk, ff768082→ShardWalker | Mob prefab hazır, roster mapping dokümanı yok | Roster hash yok, mapping spekülatif | Kullanıcı karar: Opus mapping onaylanırsa KEEP-T1'e yükselt; değilse yeniden üret |
| **concept_mockups 7'sinin kaderi** | 2 kesin KEEP, 7'si tartışmalı | b55849ca+25c6ab46 KEEP, diğer 7 "kullanıcı inspect" | Ref klasörüne taşı, cloud kapasitesi yeterli | Görsel stil kılavuz olarak değerli | Kullanıcı karar: 9'unun tamamını _Reference/MoodBoard/ klasörüne indir — silme için teknik sebep yok |
| **tiles_rift_cliff: regen mi sil mi?** | rift_pool violet, cliff_drop belirsiz kullanım | REVIEW — regen cyan tonda | 32x32 pipeline uyumlu, kullanım belirsiz | rift_pool cyan tonda yeniden üretilmeli | Kullanıcı karar: a5dbe36c sil ve cyan rift_pool yeniden üret; 886684b6 cliff_drop kullanım sahnesi netleşince karar |

---

## Önemli Teknik Notlar (Codex bulgularından)

1. **SkillOfferUI.cs çelişkisi:** 19 local icon `Assets/Sprites/UI/Icons` altında mevcut. 7 SkillData asset'i icon GUID içeriyor. AMA `SkillOfferUI.BuildSkillCard` her zaman colored placeholder çiziyor — `skill.icon` hiç tüketilmiyor. Cloud skill icon import öncesi bu borç kapatılmalı.

2. **Stat prefab D2 backfill:** 29 prop prefab sorting layer = `Decor_Cliff` order 50 (mounting) / Default order 100 (statue). `AssetCategory.CliffFaceDecor` / `WallBlocker` binding yok. RoomPainter öncesi metadata düzeltmesi gerekiyor.

3. **Orphan local sprites:** 3675a661, d5574785, c5711681 — local dosyada var, cloud'da yok. Silinmiş veya farklı account selection. Local tutulabilir; cloud yokluğu silme sebebi değil.

4. **Awaiting-selection toplam 15 obje:** Sadece kullanıcı web UI'da manuel onay/red yapabilir. Otomatik silinemez (cloud delete rule locked).
