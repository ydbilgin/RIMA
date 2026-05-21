# CB Pivot Design Review — Codex Verdict

## 1. Scope realism (16 hafta breakdown)

**Net cevap:** Tam genişletilmiş CB scope'u 16 haftaya sigmaz. 16 haftaya sigan versiyon: 3 sinif, 5 element, 2-mod tetik silahi, 7 tile state + 3-4 hibrit, 12-18 modifier, 5 oda + 1 boss, basit room template, temel meta. Secret boss, 5 NPC, class story hooks, 30-50 modifier, 7 hibritin tamami, status hybridlerin tam seti post-MVP.

| Hafta | MVP isi | Lock | Post-MVP'ye atilan |
|---:|---|---|---|
| 1 | CB Unity setup, top-down controller, mouse aim, dash, hitbox, camera, 32x24 grid sandbox | Controller feel + pure top-down kamera | RIMA-lite polish, 6-layer map composition |
| 2 | CombatEventBus, damage pipeline, enemy health, 3 dummy enemy, basic projectile | Event bus erken kurulsun | Full 15 enemy family |
| 3 | GroundMarkSystem v1: su, yag, ates, statik, buz, toz, normal tile lifecycle | Tile expiry, owner, radius, tags | Asit/manyetik/lav/zehir/gaz/bitki/karanlik/spor |
| 4 | 5 element tetik silahi v1: sol-tik trigger, Q/E swap, HUD active element | 5 trigger weapon core | 2-mod drop mode henuz sadece test flag |
| 5 | 2-mod weapon UX prototip: left trigger, right drop/hold drop, input buffer, danger preview | 2-mod silah MVP kimligi | Her element drop'un full polish'i |
| 6 | TileStateMachine v1: 7 base state + 3 hibrit: Slush, Emulsion, Volatile | Hibrit sistemin ispat seti | 7 hibritin tamami |
| 7 | StatusMatrix v1: Wet/Shocked/Burning/Oiled/Frozen ve 3 status hybrid | Dusman ustu reaction hissi | Magnetized ve tum chain edge case'ler |
| 8 | Aquamancer + Pyrotechnist skill set: 3 skill + Element Form stub | 2 sinif playable | 3. sinif full polish |
| 9 | Stormcaller skill set + 3 Element Form oynanabilir | 3 sinif tamam | Alchemist/Magnetist |
| 10 | ModifierDef SO + 12 modifier: sinif basi 4, event bus integration, proc limiter | Modifier mimarisi | 30-50 modifier hedefi |
| 11 | RoomTemplateSO port/adaptasyon, 5 preset oda, spawn waves, reward screen | Basit oda akisi | BlueprintZoneSystem full port |
| 12 | Boss 1: 3 phase, arena state phase 2, empty arena/charge phase 3 | 1 iyi boss | Secret boss, 2. boss, elite boss |
| 13 | Run loop: 5 oda + boss, 1 choice fork tipi, shop/heal/reroll, Spark/Cinder minimal | Run kapanisi | 5 NPC event, curse/mystery tam set |
| 14 | Map art integration: yeni CB Act 1 tileset, Wang16 minimum, hazard/readability pass | CB'nin kendi tile kimligi | RIMA F1 tileset'i final olarak kullanmak |
| 15 | Balance/readability pass: 20-25 dk hedefi, tutorial, fail-safe, color grammar, VFX limiter | Demo loop | Class story hooks |
| 16 | Bugfix, build, trailer capture, 15 sn cascade clip, final content lock | Steam demo MVP | Secret rooms, Echo, leaderboard |

### 16 haftaya sigmayan ham liste

| Sistem | Tam kapsam maliyeti | MVP karari |
|---|---:|---|
| 3 sinif x 4 skill + 3 form | 4-5 hafta | Lock, ama skill sayisi 3 aktif + form olarak baslat |
| 5 element x 2-mod silah | 2-3 hafta | Lock, cunku CB'yi bos arenada da oynatir |
| 7 hibrit zemin | 2 hafta + balance | MVP'de 3-4 hibrit, kalan post |
| 5 status hybrid | 1-2 hafta | MVP'de 3 hybrid, kalan post |
| 30-50 modifier | 4-6 hafta | MVP'de 12-18 modifier |
| 5 oda + boss + secret boss | 3-4 hafta | 5 oda + 1 boss lock, secret boss post |
| Choice fork + 5 NPC + story hooks | 4-5 hafta | 1 fork tipi lock, NPC/story post |
| 3-tier currency | 1-2 hafta | Spark + Cinder lock, Echo post |
| Map system port | 1-2 hafta | RoomTemplate + palette/brush lock, BlueprintZone optional |

**Verdict:** 16 hafta MVP, "full roguelite content" degil, "CB core grammar demo" olmali. Hedef: oyuncu 10 dakika icinde drop + swap + trigger + hybrid + modifier zincirini anlayip 15 saniyelik viral cascade klibi uretebilsin.

## 2. MVP LOCK 5 vs DEFER 5

### LOCK 5

| Oncelik | Sistem | Gerekce |
|---:|---|---|
| 1 | 2-mod tetik silahi | Düşman bagimliligini azaltir. Boss arenasi bosken bile oyuncu setup-payoff kurabilir. CB'nin RIMA'dan ayrilan asil eli bu. |
| 2 | Tile state system + GroundMarkSystem | CB'nin motoru. Zemin lifecycle, expiry, ownership, reaction ve hybrid olmadan oyun sadece spell shooter olur. |
| 3 | 3 sinif + Element Form | Class identity ve hiz hissi gerekir. Form ultimate Hades "signature spell" boslugunu doldurur ama 3 saniyelik net form olarak tutulmali. |
| 4 | ModifierDef SO + 12-18 modifier | Roguelite tekrar oynanabilirligi icin sart. 30-50 degil; mimariyi kur, az sayida guclu modifier ile test et. |
| 5 | 5 oda + 1 boss + phase-arena interaction | Run loop ve boss state testi olmadan tile grammar sadece sandbox kalir. Boss phase 2 zemini birlestirme mecburiyeti getirmeli. |

### DEFER 5

| Oncelik | Sistem | Neden post-MVP |
|---:|---|---|
| 1 | 5 NPC mid-run event | Event authoring, UI, reward edge-case ve balance maliyeti yuksek. Ilk MVP'ye bir vendor/shop yeter. |
| 2 | Class story hooks | Okunabilir MVP'ye mekanik katmaz. 10 run sonra arena degisimi gibi vaatler production debt uretir. |
| 3 | Secret boss + hidden rooms | Onemli retention katmani ama once normal boss ve core run calismali. Secret sistem, gereksiz trigger/content carpani ekler. |
| 4 | Echo currency + leaderboard/Pact | Meta zorluk ve leaderboard, core balance sabitlenmeden erken gelirse yanlis metrik uretir. |
| 5 | 30-50 modifier tam set | 12-18 iyi modifier, 50 yarim modifierdan daha degerli. Event bus entegrasyonu erken, content carpani gec. |

### Kenar karar

7 hibrit zemin tamamen defer edilmemeli. MVP'de 3-4 hibrit lock, kalan 3-4 post-MVP. Cunku "2+ zemin birlestirme" dokumandaki USP; hic hibrit yoksa tasarim iddiasi eksik kalir.

## 3. Teknik riskler

- **2-mod UX:** Risk orta-yuksek. Sol-tik trigger, sag-tik drop, Q/E swap yapisi kisa vadede okunur; fakat sag-tik "current element drop" ile sol-tik "current element trigger" ayni HUD'da iki farkli anlam tasir. Cozum: element wheel'de iki ikon goster: üstte trigger, altta drop footprint. Sag-tik basili tutunca yerde footprint preview ve self-harm alan rengi cikmali. Tap/hold farki MVP'de tehlikeli; ilk surumde sag-tik = drop, sol-tik = trigger, hold sadece aim preview olsun.
- **Hibrit tile state:** 32x24 grid = 768 cell. 14 state ve ~30 transition performans riski degil, karmaşıklık riski. Her frame tum grid scan etme. Event-driven dirty cell queue kullan: tile spawn/expire/neighbor change oldugunda 4 komsu kontrol et. Reaction chain icin ProcLimiter + max transitions/frame sart. Performans rahat; debug tooling zorunlu.
- **Modifier system:** `ModifierDef` SO yeterli. Node graph MVP icin fazla. Gerekli yapi: trigger enum, filters, operations, scalar, addTag, spawnDef, cooldown, maxProcPerEvent, priority. 30-50 modifier icin bile SO + event bus calisir. Node graph ancak user-generated spells veya mod editor hedeflenirse gerekir.

### 2-mod UX detay

| Problem | Risk | Cozum |
|---|---|---|
| Current element hem trigger hem drop tasir | Oyuncu ne atiyorum / ne birakiyorum karistirir | HUD'da iki-mode panel: LMB icon + RMB footprint |
| Self-damage alanlari | Haksiz olum hissi | Drop preview danger border + 0.25 sn arm time |
| Q/E swap hizli combo | Input kacirma | 0.2-0.3 sn swap buffer, last input queue |
| Sag-tik basili mi tap mi | Ilk 5 dk confusion | MVP'de tek davranis: RMB drop, hold preview |
| Boss uzerine patch birakma | Trivial burst riski | Boss hurtbox altina drop atilir ama boss moving/phase clears counterplay getirir |

### TileStateMachine detay

| Alan | Karar |
|---|---|
| Grid boyutu | 32x24 = 768 cell, rahat |
| Update modeli | Event-driven dirty cells |
| State kaydi | TileRuntimeState struct: baseState, hybridState, ownerId, expireAt, charges, tags |
| Transition kaydi | Data table: A+B adjacency -> hybrid, triggerTag -> reaction |
| Chain koruma | ProcLimiter: reactionId per tile cooldown + max 64 proc/frame |
| Debug ihtiyaci | Runtime overlay: state color, expire timer, last reaction |

### ModifierDef SO detay

| Gereken alan | Neden |
|---|---|
| trigger | OnCast, OnTileSpawn, OnReaction, OnKill, OnDash, OnFormEnter |
| filter | class, element, tile tag, status tag, skill id |
| operation | radiusAdd, cooldownMul, damageMul, durationAdd, spawnTile, addStatus |
| proc rules | cooldown, maxPerRoom, maxPerEvent, priority |
| VFX tags | Modifier hissi icin gorunur fark |
| description template | UI metni otomatik ve tutarli olsun |

**Node graph karari:** Hayir. MVP icin node graph, tasarim araci degil scope creep. SO table + small interpreter yeterli.

## 4. Run length validation

Mevcut hesap:

| Parca | Sure |
|---|---:|
| 5 oda x 90-150 sn | 7.5-12.5 dk |
| Boss | 3-5 dk |
| Choice fork + NPC/shop | 1-2 dk |
| Toplam | 12-19 dk |
| Hedef | 20-25 dk |
| Acik | 5-7 dk |

**Net onerim:** Acigi "ek oda" ile doldur, "longer boss" ile degil.

| Secenek | Karar | Gerekce |
|---|---|---|
| Extra dalga | Kismen | Dalga sayisi artarsa yorgunluk yapar. 1 odada elite wave olabilir. |
| Ek oda | Ana cozum | 5 oda + boss yerine 6 oda + boss hedefle. 1 oda = 2-3 dk net ek sure. Choice fork ile oyuncu ajansi artar. |
| Longer boss | Hayir | 3-5 dk zaten yeterli. 7-8 dk boss solo dev icin pattern/polish borcu ve frustration riski. |

### Onerilen MVP run yapisi

| Segment | Hedef sure |
|---|---:|
| Oda 1 tutorial arena | 2:00 |
| Oda 2 normal | 2:30 |
| Choice fork | 0:30 |
| Oda 3 reward-risk room | 3:00 |
| Oda 4 elite/hazard room | 3:00 |
| Oda 5 build test room | 3:00 |
| Shop/shrine | 1:00 |
| Oda 6 pre-boss arena | 2:30 |
| Boss | 4:00 |
| Toplam | 21:30 |

**Neden 6 oda:** 20-25 dk hedefi icin en temiz ekleme. Boss'u uzatmak yerine build'in iki kez nefes almasini saglar. 6 oda hala MVP'de kontrol edilebilir; her oda yeni tile/enemy ogretmez, sadece layout/pressure degisir.

### Secret room karari

MVP'de secret room yok. Eger 16 haftada content rahat giderse "1 hidden bonus chest room" eklenebilir, secret boss degil. Secret boss, 20-25 dk run hedefini saptirir ve post-MVP retention katmani olarak daha degerli.

## 5. Map design port

### Sakla / sil tablosu

| RIMA parcasi | CB karari | Gerekce |
|---|---|---|
| Karar #143 6-layer painted composition | SIL | CB pure top-down ve discrete tile state oyunu. 6-layer Hades/RIMA composition burada scope drift. |
| Tile Palette + RuleTile sistemi | SAKLA | Room authoring hizini artirir. CB'nin tile reaction sistemi icin editor altyapisi is yapar. |
| Wang16 tilesets `Assets/Art/Tiles/F1/Tilesets/` | GECICI SAKLA, finalde yeni CB Act 1 tile gen | Teknik testte kullan. Steam/demo kimligi icin circuit/lab/keep temali yeni tile gerekir. |
| Asset Pack Browser | SAKLA | Tile variant ve hazard asset secimi hizlanir. |
| Brush Executor | SAKLA | Editor workflow degerli. |
| Room Template SO | LOCK | CB MVP icin yeterli ana map data modeli. |
| Blueprint Zone System | DEFER/OPSİYONEL | 5-6 oda preset MVP'de zone generation gerektirmez. Faz 2 procedural room icin saklanabilir. |
| Adjacency Rules | SAKLA | Hibrit tile ve transition readability icin dogrudan faydali. |
| 6-layer visual pipeline | SIL | CB'de state okunurlugu gorsel guzellikten daha kritik. |
| RIMA class/pose-heavy art assumptions | SIL | CB 32px uzak kamera, minimal asset, VFX-heavy. |

### Karar #143 silme onayi

**Onayliyorum:** CB icin Karar #143 6-layer painted composition silinmeli. Bu RIMA'nin perspektif/asset pipeline sorununu CB'ye tasir. CB'nin ihtiyaci:

1. Pure top-down grid.
2. State-readable tiles.
3. Low-cost Wang16/RuleTile transitions.
4. Runtime overlay/debug.
5. VFX clarity budget.

6-layer composition, CB'de "guzel map" uretir ama "hangi tile ne state'te" sorusunu bulaniklastirir.

### Wang16 karari

RIMA Wang16 tilesetleri CB'ye **prototype placeholder** olarak saklanabilir. Final Act 1 icin yeni tile generation daha dogru:

| Kullanim | Karar |
|---|---|
| Week 1-8 prototip | RIMA Wang16 kullan |
| Week 9-14 demo art | CB Act 1 tileset generate et |
| Steam capsule/trailer | RIMA F1 kullanma |

Sebep: CB'nin pazar hook'u "circuit-collapse/reactive-grid". RIMA F1 fantasy/biome izlenimi verirse pivot temizligine zarar verir.

### BlueprintZoneSystem karari

MVP'de BlueprintZoneSystem gerekmez. RoomTemplateSO + spawn anchors + hazard masks yeterli.

Gerekli minimum data:

| Data | Aciklama |
|---|---|
| roomId | preset kimligi |
| size | 32x24 veya varyasyon |
| baseTilemap | floor/wall/decor/hazard |
| spawnAnchors | enemy wave spawn noktasi |
| bossAnchors | boss/telegraph referansi |
| tileStateSeed | baslangic hazard/state |
| rewardType | normal/shop/elite/boss |
| exits | next room/fork |

BlueprintZoneSystem ancak 2. biome veya procedural map hedeflenince geri gelsin.

### RoomDataSO vs CBTileStateSO

**Net karar:** `RoomDataSO` mevcut map layout'u tutsun; runtime state icin ayri `CBTileStateSO`/`TileStateDef` ailesi kurulsun.

| Model | Tutsun | Tutmasin |
|---|---|---|
| RoomDataSO | Layout, spawn anchor, reward, hazard seed, tile palette ref | Runtime expiry, reaction, status, proc |
| CBTileStateSO / TileStateDef | State tags, duration, visual, reaction hooks, hazard rules | Oda layout'u |
| TileRuntimeState | current state, owner, expire, charges, last proc | Designer-authored static data |

Neden: RoomDataSO'ya state logic gommek RIMA editor modelini CB runtime sim'e baglar. Ayri SO ile CB sistemi test edilebilir, port edilebilir, debug edilebilir kalir.

## 6. Pivot timing — NET ÖNERİ (A/B/C)

**Karar: C — Hybrid.**

CB design doc tamamla (1 hafta), RIMA icinde kucuk POC yap (2-mod tetik + 3 tile reaction + 1 boss dummy, 2 hafta), sonra full pivot karari ver.

### Neden A degil: Full pivot now

Full pivot psikolojik olarak temiz gorunur, ama okul odevinin RIMA olarak verilmis olmasi riski var. Ayrica CB'nin en riskli parcasi dokuman degil, controller + tile readability + combo UX. Bunu gormeden yeni Unity setup'a 1-2 hafta gomulmek yanlis.

### Neden B degil: RIMA-lite once

RIMA-lite 4-6 hafta daha harcatir ve zaten drift sebebi olan map/tile pipeline'a geri sokar. Karar #143 silinse bile RIMA'nin sinif, pose, camera, asset beklentileri CB'nin pure top-down ritminden farkli. Bu yol "yatirimi koruma" gibi gorunur ama creative burnout riskini uzatir.

### Neden C

Hybrid, okul baglamina en satilabilir yol:

1. "RIMA scope iteration" diye anlatilabilir.
2. Mevcut RIMA altyapisinda POC ile teknik risk 2 haftada gorulur.
3. CB'nin gercek hook'u test edilir: drop + swap + trigger oyuncuya 5 dakikada geciyor mu?
4. POC basariliysa full pivot daha savunulabilir.
5. POC basarisizsa RIMA-lite'a donus icin 2 hafta kayip kabul edilebilir.

### 19 haftalik gercekci pivot akisi

| Hafta | Is |
|---:|---|
| -3 | CB design doc final, MVP cut list, input scheme lock |
| -2 | RIMA POC: 2-mod trigger, water/oil/static, electric/fire trigger |
| -1 | RIMA POC: 1 dummy boss, 1 room, readability/user test, pivot gate |
| 1 | Full CB setup veya RIMA branch cleanup |
| 2-16 | Yukaridaki MVP plan |

### Pivot gate kriterleri

Full pivot sadece su 5 kriter gecerse:

| Gate | Pass kosulu |
|---|---|
| 5-minute comprehension | Oyuncu su + elektrik combo'yu aciklama okumadan yapar |
| 15-second clip | 3 action ile ekranda net cascade gorulur |
| Input clarity | LMB/RMB/Q/E karisikligi 2 denemeden sonra bitiyor |
| Readability | 5+ tile state ayni ekranda ayirt ediliyor |
| Solo-dev effort | 1 yeni tile reaction eklemek 30 dk - 2 saat bandinda |

**Okul odev cümlesi:** "RIMA, kapsam iterasyonu sonucu daha okunur pure top-down reactive-grid MVP'ye indirildi; eski RIMA kapsamı Faz 2/lore olarak donduruldu." Bu, "projeyi terk ettim" yerine "scope refinement" anlatısı verir.

## 7. Anti-klon kontrolü

| Oyun | CB artik neye benziyor | Risk | Mitigasyon |
|---|---|---|---|
| Magicka | Element kombo ve hizli swap hissi artti | Orta | Element kombinasyonu elde/menu degil, zeminde kurulan arena state ve drop-trigger payoff olarak sunulacak. |
| Noita | Element reaction ve chain cascade benzerligi artti | Orta | Continuous physics/pixel sim yok; discrete 32x24 tile state, sinirli reaction table ve manuel combat one cikarilacak. |
| Hades | Boon/modifier, boss phase ve run fork ile benzerlik artti | Orta-yuksek | Olympian boon anlatimi, isometric melee pacing ve narrative chamber yapisi yok; CB capsule'i "reactive-grid spellcraft" olacak. |
| Magicraft | Modifier/spell evolution ve 32px VFX spectacle yakin | Orta | Magicraft wand/spell graph degil; CB'nin build'i spatial tile setup + class field control + trigger weapon uzerinden ayrilacak. |

### Ek klon guardrail

| Guardrail | Uygulama |
|---|---|
| Magicka'dan uzaklas | Element recipe yok, friendly-fire comedy yok, co-op chaos MVP yok |
| Noita'dan uzaklas | Simulasyon degil deterministic grid, terrain destruction yok |
| Hades'ten uzaklas | God boon UI yok, dash-strike melee core yok, narrative portrait economy yok |
| Magicraft'tan uzaklas | Spell editor/wand chain yok, arena tile grammar var |

## 8. EXECUTIVE SUMMARY (5 madde, kullanıcı için)

1. Full genisletilmis CB 16 haftaya sigmaz; ama kesilmis MVP sigar: 3 sinif, 2-mod trigger, 3-4 hibrit, 12-18 modifier, 6 oda + 1 boss.
2. MVP'nin kilidi 2-mod silah + TileStateMachine. Bunlar calisirsa CB RIMA'dan net ayrilir; calismazsa oyun sadece Hades/Magicraft benzeri spell shooter olur.
3. 30-50 modifier, 5 NPC, class story, secret boss, Echo/leaderboard post-MVP. Bunlar iyi fikir ama ilk demo icin yanlis oncelik.
4. RIMA Map Designer'dan palette/RuleTile/brush/RoomTemplate sakla; Karar #143 6-layer composition'i CB icin sil. CB pure top-down state-readability oyunu.
5. Net pivot onerisi C: 1 hafta CB doc final + 2 hafta RIMA icinde POC, sonra full pivot. Okul baglaminda bunu "RIMA scope refinement" olarak satmak en temiz yol.

CODEX VERDICT COMPLETE
