# Codex Gorusu - RIMA Room Pipeline Karar Destegi

## 1. Executive decision
- Yon: C ana yon, sinirli hibrit destek ile. Production-safe ana pipeline "Full 2D fractured chamber / semi-wall illusion system" olmali; mevcut Modular Wall Shell MVP ise ana oda cozumu degil, sadece north backwall landmark ve ozel kapali oda varyantlari icin kontrollu bir alt sistem olarak yasamali.
- Gerekce: RIMA lore'u zaten kirilmis mekan, void, rift catlagi ve eksik mimari uzerine kurulu; bu yuzden kusursuz duvar seam'i uretmeye calismak yerine kirik kompozisyonu dil haline getirmek hem daha hizli hem daha tutarli. 21_29_22 (1), 21_29_23 (2) ve 21_29_23 (4) gorsellerinde guclu dungeon atmosferi tam duvarlardan degil, zemin adasi + low edge + north landmark + cyan/amber kontrastindan geliyor.
- Confidence: high.

## 2. PixelLab reality check
- Tam duvar zor cunku PixelLab tek parca "oda posteri" uretmeye kayiyor; modular strip istendiginde ayni alt hiza, ayni perspektif, ayni tas ritmi ve ayni edge kalinligi garanti degil. 21_29_29 sheet'inin "neden tam duvari PixelLab'dan yapamiyorum" panelindeki perspektif uyumsuzlugu, kose/baglanti zorlugu ve seam gorunurlugu pratikte asil riskler.
- Full high wall 2D icin north/west ayrimi, kose donusu, door opening ve cap parcasi arttikca asset sayisi buyur; her yeni tema ayni sorunu tekrar acar. Bu nedenle "oda buyuk asset" degil, "kucuk parca + curated assembly" dogru yaklasim.
- Daha dogru asset modeli: 32x32 floor tile ve decal, 64x64 / 96x64 low edge, 128x96 veya 160x128 backwall card, 64x96 seam cover, 64x64 prop/decal. Buyuk oda render'i PixelLab'dan tek parca alinmamali.
- Sheet onerisi: floor ve low edge icin 4x4 sheet mantikli; 16 cell yeterli varyasyon verir. Backwall icin 4x2 ya da 4x4 sheet olabilir ama N/W wall family separation yan track olarak kalmali: north backwall parcalari ayri, side card parcalari ayri uretilmeli. Torches, banners ve lights baked olmamali; bunlar overlay decor olarak ayrilmali.
- Mevcut 4x4 Modular Wall Shell MVP fractured chamber ile cakismiyor. Co-exist edebilir, fakat MVP'nin rolu "oda cevresini tam kapatma" degil; boss gate, prison backwall, archive shelf-wall gibi landmark band uretmek olmali.

## 3. Best visual system for RIMA
- Net tanim: RIMA odasi = kirik dark granite floor island + dusuk kirik edge siniri + black/rift void + cyan rift catlaklari + amber pratik isiklar + opsiyonel north landmark/backwall + kenar prop yogunlugu + temiz combat merkezi.
- Zorunlu katmanlar: Floor, Edge, Void/BG, Collision, Encounter anchors, Door/socket markers. Gorsel kimlik icin zorunlu destek: cyan crack decal seti ve amber light prefab'lari.
- Opsiyonel katmanlar: North backwall landmark, left/right side card, theme prop cluster, fog/darkness mask, water overlay, ritual circle, banner/shelf/cage dressing.
- ChatGPT fractured chamber tanimina ekleme: merkez her zaman combat-okunur tutulmali; dekor gucu kenarlara ve kuzey landmark'a tasinmali. 21_29_23 (3) gorselindeki yaklasik 20 aktor okunurluk icin ust sinira yakin; RIMA 64x64 chibi sprite'lariyla normal combat odasinda 6-10 dusman + 1-3 player/allied actor daha saglam olur.
- Cikarilacak madde: her odada buyuk portal veya yogun rift crack kullanmak. Cyan vurgu oda kimligi icin guclu, ama telegraph ve spell VFX ile karismamasi icin base room catlaklari daha dusuk parlaklikta tutulmali.

## 4. First asset batch
- 1. PixelLab: Dark Granite Floor A, temiz 32x32 tile seti.
- 2. PixelLab: Dark Granite Floor B, catlakli 32x32 varyasyon.
- 3. PixelLab: Broken Floor Chunk 2x2, floor island siluetini bozmak icin.
- 4. PixelLab: Low Edge Straight N/S, kirik tas kenar.
- 5. PixelLab: Low Edge Straight E/W, yan edge.
- 6. PixelLab: Low Edge Corner inner/outer, oda adasi formu icin.
- 7. PixelLab: Rubble Seam Cover Small/Medium, wall/edge seam gizleme.
- 8. PixelLab: North Backwall Broken Mid, baked torch/banner olmadan.
- 9. PixelLab: North Backwall Left/Right End, ayni baseline ile.
- 10. PixelLab: Boss/Rift Gate Center Card, 21_29_23 (2) ve 21_29_29 Rift Gate Chamber icin.
- 11. PixelLab: Stone Pillar/Seam Cover, dikey hizalama gizleme.
- 12. PixelLab: Prop Cluster - crates/sarcophagus/cage, kenar siniri icin.
- 13. Overlay: Cyan Crack Decal Small/Medium/Large, Unity tint/intensity kontroluyle.
- 14. Overlay: Amber Torch/Candle Light Prefab, sprite + 2D Light ayrik.
- 15. Reuse/Unity: Fog/Void Edge Mask, shader/sprite overlay olarak.
- Uretim sirasi: once floor + low edge cunku oda olcegi ve collision bundan dogrulanir; sonra backwall/gate cunku kompozisyon kimligi ekler; en son props/decal/light cunku bunlar tema cesitliligi ve polish saglar.

## 5. Unity assembly approach
- Room kurulumu: Hybrid Template + Decor Overlay ile uyumlu prefab/template yaklasimi. `Assets/Scripts/Rooms/RoomTemplate.cs` baseImage, wallPathLocalPoints, anchors, doorSocketsLocalPoints, enemySpawnPoints ve cameraBounds alanlariyla bu modele uygun.
- Base room: Tilemap veya baked baseImage olarak kurulabilir; MVP icin hizli yol, floor island + low edge'i template base olarak bake etmek ve overlay anchor'larla decor/props/light yerlestirmek. Uzun vadede floor Tilemap + decor prefab anchors daha esnek olur.
- Collision: low edge ve void siniri gameplay collision path olarak `wallPathLocalPoints` icine girmeli; gorsel backwall collision degil, sadece north boundary/occluder olabilir.
- Sorting layer onerisi: BG/Void (-50), Floor (0), Floor Decal (5), Edge/Low Wall (20), Props (30), Characters (40, y-sort), Front Edge/Foreground Props (55), Deco/Light Sprites (60), VFX/Telegraph (80), UI (100). Task'taki BG/Prop/Edge/Floor/Deco ifadesini pratikte render icin yeniden siralamak gerekir: BG altta, Floor onun ustunde, Edge/Prop karakterle y-sort uyumlu, Deco/Light en ust kontrol katmani.
- Camera: 64x64 chibi, PPU 64 ve 32x32 tile ile normal oda icin yaklasik 24x14 tile ile 32x18 tile arasi oynanabilir alan hedeflenmeli. Boss/ritual oda 36x20 tile'a cikabilir ama tek ekranda okunurluk korunacaksa camera bounds ve encounter spawn'lari onceden author edilmeli.
- Lighting: baked torch kullanma; amber 2D Light prefab, cyan rift light prefab ve dusuk global ambient ile kontrol edilmeli. Cyan crack base sprite parlakligi VFX telegraph'tan daha dusuk olmali.

## 6. Recommended first room archetypes
- 1. Shattered Keep Combat Room: temel savas odasi. En yuksek reuse; floor, low edge, rubble, 1 north landmark ve 2-3 prop cluster yeter. Estimated unique asset: 8-10.
- 2. Rift Gate Chamber: transition/exit dogrulama odasi. Door socket, gate landmark ve cyan light pipeline'ini test eder. Estimated unique asset: 10-12.
- 3. Ritual Chamber: boss/elite telegraph okunurlugu icin iyi test. Merkez ritual circle ile alan kompozisyonu guclenir ama dusman sayisi dusuk tutulur. Estimated unique asset: 12-15.
- 4. Prison Hold: prop boundary ve theme dressing icin iyi; cage, chain, bars gibi overlay asset'ler duvar ihtiyacini azaltir. Estimated unique asset: 12-16.
- 5. Flooded Crypt ya da Library Archive: ikinci batch'e aday. Flooded Crypt su overlay ve reflection riski tasir; Library Archive prop yogunlugu ve occlusion riski tasir. Ilk batch'te biri secilecekse Flooded Crypt daha guclu biome farki verir, Library daha cok prop/sorting testi verir.

## 7. Bastion / other reference takeaways
- Bastion ogretisi: mekan hissi icin tam duvar sart degil; kirik/floating platform, edge language, sahne kompozisyonu ve dis bosluk hissi alani tanimlayabilir. RIMA bunu daha karanlik, daha dusuk doygunluklu ve daha combat-okunur uygulamali. Kaynak: Supergiant Bastion sayfasi ve screenshot/blog referanslari, https://www.supergiantgames.com/games/bastion/ ve https://www.supergiantgames.com/blog/bastion-e3-2011-screenshots/
- Hades ogretisi: oda bazli run akisi, guclu reward/door okunurlugu ve her encounter icin net arena formu. RIMA Hades'ten combat readability ve oda odulu/exit netligini almali; fazla illustratif, elle boyanmis yogun arka plan dilini birebir kopyalamamali. Kaynak: https://www.supergiantgames.com/games/hades/ ve https://store.steampowered.com/app/1145360/Hades/
- Children of Morta ogretisi: pixel art + modern lighting birlikte calisabilir; karakter animasyonu ve dungeon atmosferi ayni anda okunabilir. RIMA icin en yakin teknik his bu: 2D isometric/top-down pixel art, procedural dungeon varyasyonu ve 2D lighting. Kaynak: https://store.steampowered.com/app/330020/Children_of_Morta/
- Curse of the Dead Gods ogretisi: dark temple, darkness/light kontrasti ve oda bazli risk atmosferi RIMA'ya yakin; fakat RIMA'nin chibi 64x64 sprite okunurlugu icin cok karanlik ve cok dusuk kontrastli alanlardan uzak durulmali. Kaynak: https://www.focus-entmt.com/en/games/curse-of-the-dead-gods ve https://store.steampowered.com/app/1123770/Curse_of_the_Dead_Gods/
- En yakin referans: Children of Morta teknik piksel/lighting dengesi + Bastion fractured platform mantigi. Hades room clarity icin destek referans. Curse atmosfer referansi, ama okunurluk limitleri nedeniyle dikkatli kullanilmali.

## 8. Risks and mitigation
- Risk 1 - Production drift: fractured chamber kolay gorunup her oda icin ozel poster uretimine donebilir. Mitigation: her oda ayni 10-15 parcalik asset grammar'i ile kurulup sadece landmark/prop temasi degismeli.
- Risk 2 - Combat okunurlugu: cyan cracks, amber lights ve karanlik zemin telegraph/VFX ile karisabilir. Mitigation: base decor emissive dusuk, gameplay telegraph emissive yuksek ve renk/anim ayrimi net olmali.
- Risk 3 - Wall shell scope creep: 4x4 wall MVP tam oda shell hedeflenirse eski seam problemine geri donulur. Mitigation: wall MVP'yi north landmark band + side card ile sinirla; corner-perfect full shell hedefleme.
- Risk 4 - Asset scale mismatch: PixelLab parcalari farkli perspektif ve baseline ile gelebilir. Mitigation: her sheet icin ayni prompt, ayni canvas, ayni bottom alignment ve Unity import review checklist'i kullan.
- Risk 5 - Procedural overreach: MASTER Poisson + Dual Grid kilidine ragmen oda estetigi tamamen procgen'e birakilirsa staging kompozisyonu bozulur. Mitigation: layout/collision procedural olabilir; landmark, door, combat center ve high-value prop clusters curated anchor olmali.

## 9. MVP plan
- 1. Claude/user: final yonu lock eder. Output: C ana pipeline + bounded wall side track karari. Blocker.
- 2. PixelLab: floor + low edge 4x4 sheet uretir. Output: granite floor, cracked floor, straight/corner low edge cells. Blocker.
- 3. Unity/Codex: tek Shattered Keep Combat Room template'i kurar. Output: RoomTemplate asset, collision path, camera bounds, 3 door socket, 6-8 enemy spawn. Blocker.
- 4. PixelLab: north backwall broken mid/end + boss/rift gate center card uretir. Output: baseline uyumlu landmark parcalari. Paralel: 3. adimdan sonra baslayabilir.
- 5. Unity/Codex: decor overlay anchor sistemiyle rubble, pillar, torch, crack decal ve gate prefablarini yerlestirir. Output: Hybrid Template + Decor Overlay calisir oda prefab'i.
- 6. Unity/user: 64x64 chibi test aktorleriyle combat okunurlugu testi yapar. Output: oyuncu, 6-10 dusman, telegraph ve movement path kontrolu. Blocker.
- 7. PixelLab: prison/ritual prop batch uretir. Output: cage, chain, sarcophagus, ritual circle, small altar, rubble seam covers. Paralel.
- 8. Unity/Codex: Rift Gate Chamber ve Ritual Chamber varyantlarini ayni grammar ile kurar. Output: 3 oda archetype'i ayni pipeline'da calisir.
- 9. Claude/rima-qc: visual + mechanical QC yapar. Output: scale, sorting, lighting, collision, door readability pass/fail.
- 10. Claude: MVP kabul kararini verir. MVP done tanimi: tek run flow icinde en az 3 oda tipi, 64x64 chibi aktorlerle okunur combat, collision/door sockets dogru, 2D lights ayrik, wall shell sadece landmark olarak kullanilmis, yeni oda uretimi icin asset grammar tekrar edilebilir.
