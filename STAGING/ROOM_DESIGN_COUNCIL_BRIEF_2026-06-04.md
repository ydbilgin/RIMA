# Room Design Council Brief — gameplay-feel oda seti (2026-06-04)

**Amaç:** RIMA "The Shattered Keep" için, İYİ OYNANIŞ HİSSİ veren oda-tipi/şekil/boyut SETİNİ ve tasarım ilkelerini belirlemek. ChatGPT pack + elimizdeki kütüphaneyi BİRLEŞTİR, gerekirse esinlenip DAHA İYİSİNİ öner. **Bu körü körüne import DEĞİL** — kullanıcı: "analiz et, birleştir, iyi his veren tipleri bul, gerekirse daha iyisini yap."

## RIMA combat bağlamı (oda şekli neyi etkiler)
- Hızlı iso ARPG (Hades + Dead Cells hissi). 10 sınıf: melee/ranged/caster/summoner. Wave-based encounter (EncounterController + nextWaveKillFraction + enemySpawnSockets).
- Oda = void'de yüzen taş ada. **Void kenarı bir MEKANİK** olabilir (knockback→void = ölüm; Warblade/Brawler knockback'i parlar). Kenar = risk/ödül.
- Run = 10-15 oda (random, pool genişletiliyor). Pacing önemli (gerilim-nefes ritmi).
- Canon: güney kapı YOK (N/E/W). Cliff'ler floor boundary'den otomatik. Cyan #00FFCC = seal/kapı.

## A) ChatGPT pack (15 oda — hepsi BÜYÜK; analiz=`STAGING/chatgpt_rooms_pack/.../ROOM_PREVIEW.md`)
Combat (24-30w): diamond(3-exit diagonal), cross(uzun E-W lane), L-shape(kite+ambush pocket), bridge-lobes(2 lob+geniş köprü, rotasyon), hourglass(2 kase+bel, dash-through risk), donut(merkez void, çift-ark baskı), teardrop(dar giriş+şişkin merkez+kuyruk).
CombatLarge(36-38w): organic-blob(geniş açık merkez, build-space), twin-basins(2 kase+orta shelf, flank lane).
Elite(30x22): trident(3 prong→tek arena, az ama güçlü düşman), crescent(oyuk pençe, sağ-boynuz baskı).
Boss(36x28): shattered-oval (simetrik, tek N giriş, merkez boss).
Chest(20-22): reliquary-diamond(2 chest+yan çıkış), donut-vault(halka+merkez seal).
Corridor(8x20): zigzag-bridge (dar, 2 çıkış, 2 ambush).
**Güçlü yanı:** şekil çeşitliliği + tactical not. **Zayıf yanı:** HEPSİ büyük (boyut çeşitliliği yok), Shrine/Spawn-safe/Merchant/Event yok, hazard/cover/pillar/elevation yok (düz floor).

## B) Elimizdeki kütüphane (`Assets/Data/Rooms/Library/` — RoomTemplateSO)
Combat_Small / Combat_Medium / Combat_Large · Boss_Intro_01 · Elite_01 · Corridor_Linear / Corridor_LShape · Shrine_01 · Spawn_01 · Treasure_01 + DemoRoomBank(RoomBankSO). **Boyut + tip kapsamı var** (Small/Med/Large + Shrine/Spawn/Treasure) ama şekiller muhtemelen sade/dikdörtgensi (cx audit etsin).
Ayrıca 6 sahne-bazlı iso harita (legacy): diamond/bridge/cross/ell/hourglass/donut.

## Tasarım soruları (advisorlar cevaplasın — KISA, somut)
1. **Hangi ŞEKİLLER en iyi gameplay-feel verir ve NEDEN** (combat dinamiği bazında)? ChatGPT pack'ten KEEPER'ları seç, zayıfları ele, iyileştirme öner.
2. **BOYUT çeşitliliği:** ChatGPT hepsi büyük. Küçük/orta gergin odalar pacing için gerekli mi? 10-15 odalık run'da boyut dağılımı ne olmalı?
3. **TİP kapsamı:** ChatGPT'de Shrine/Spawn-safe/Merchant/Event yok. Bunlar için kendi kütüphanemizi mi tutalım? Eksik tip var mı?
4. **Şekillerin desteklemesi gereken MEKANİKLER:** void-kenar knockback, chokepoint (melee↔ranged), çok-lob flank, wave-spawn yerleşimi, hazard/cover/pillar bölgeleri. Hangi şekiller hangilerini iyi destekler?
5. **Run PACING/dizilim:** oda tipleri/boyutları bir run'da nasıl akmalı (gerilim-nefes)?
6. **NİHAİ SET önerisi:** kurulacak oda-tipi × şekil × boyut matrisi (ChatGPT'den esinlen+iyileştir). Hangi ChatGPT odası as-is import / adapt / skip? Eksik üretilecekler?
