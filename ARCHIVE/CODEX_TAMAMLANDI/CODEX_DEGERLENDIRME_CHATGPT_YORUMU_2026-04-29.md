# Codex Degerlendirmesi — ChatGPT Skill Feedback

Date: 2026-04-29
Status: CODEX_REVIEW — Claude final karar vermeli
Input: `STAGING/CHATGPT_YORUMU_2026-04-29.md`

## Kisa Sonuc

ChatGPT feedback'i genel olarak dogru yonde ve onceki Codex audit bulgulariyla uyumlu. En guclu tarafi, tekil skill isimlerinden ziyade sistemik tekrar risklerini yakalamasi: counter/parry, dash/phase, execute, cursor-zone, stack/rotation-lock ve cross-class state eksikligi.

Benim degerlendirmem: Bu feedback dogrudan revizyon listesine cevrilmemeli; once Claude tarafinda "state vocabulary + class verb ownership" dokumanina indirgenmeli. Aksi halde 10 sinifta ayni anda cok fazla redesign baslar ve kapsam kayar.

## Yuksek Guvenle Katildigim Noktalar

1. Cross-class sisteminin state tabanli netlesmesi gerekiyor.
   - Bu, feedback'in en degerli maddesi.
   - Her class'in export ettigi 2-4 state ve diger class'larin bunlara verdigi davranissal cevaplar yazilmadan skill revizyonu daginik kalir.
   - "+%10 damage" tipi pasifler RIMA'nin fracture temasini tasimaz.

2. Execute kosullari HP<%30 klisesinden cikmali.
   - Warblade, Ranger, Gunslinger, Ronin ve Shadowblade ayni execute ailesine kayabilir.
   - Her execute class-specific gate istemeli: Broken/Sundered, Mark+Trap, reload/line aim, Tension/draw window, Scar collapse.

3. Counter ayrimi zorunlu.
   - Warblade / Ronin / Brawler ayni "savun ve vur" hissine dusmemeli.
   - Ayrim net: Warblade absorbs/breaks, Ronin waits/draws, Brawler evades/weaves.

4. Brawler Charge sistemi fazla karar yuklu olabilir.
   - Charge hem skill amplify hem V/Overdrive fuel olursa 4/6 slotlu roguelite akista muhasebe artar.
   - Claude bir ana kaynak rolu secmeli: harcanan guclendirme mi, bankalanan hype motoru mu?

5. Hexer stack dongusu davranissal hale getirilmeli.
   - Sadece stack uygula -> yay -> patlat dongusu calisir ama uzun vadede linear kalir.
   - Hex'in enemy behavior bozmasi RIMA tonuna daha uygun.

6. Elementalist shape verb'i guclendirilmeli.
   - Fire/Frost/Light sadece damage/control renkleri gibi kalirsa sinif siradanlasir.
   - Wall/orb/beam/rune etkilesimleri Elementalist'i ozel yapar.

7. Summoner sacrifice engine abuse riski gercek.
   - Blood for Power, Mass Sacrifice, Dark Pact ve Lich Form bir araya gelince sonsuz kaynak/tempo riski dogar.
   - Internal CD, minion cap, sacrifice recovery ve summon capacity penalty gibi kilitler tasarimda acik yazilmali.

## Kismen Katildigim veya Dikkatli Ele Alinmasi Gereken Noktalar

1. "Skill sayisi fazla ama davranis farki az" dogru risk, ama her skill'i asiri kompleks yapma riski de var.
   - RIMA'nin 4/6 slot sistemi skill'lerin ozel sebep tasimasini ister.
   - Fakat her skill bir mini-sistem olursa UI/okunabilirlik ve implementation maliyeti artar.
   - Claude basit ama davranissal skill hedeflemeli: tek net soru, tek net cevap.

2. Warblade CC yogunlugu riskli, fakat Faz 1 demo class oldugu icin okunur ve tatmin edici kontrol hissi de gerekli.
   - Warblade tamamen CC'den arindirilmamali.
   - CC etkilerinin bir kismi "state opener" sonucuna baglanmali: Broken, Sundered, Staggered.

3. Gunslinger icin reload rhythm dogru, ama perfect reload sistemi input/UI maliyeti yaratabilir.
   - Faz 1 disi oldugu icin tasarlanabilir.
   - Ancak core combat input sadeligi korunacaksa "last shot / empty mag / heat breakpoint" gibi daha az twitch isteyen kosullar daha guvenli olabilir.

4. Ranger mark yogunlugu riskli, ama mark sistemi sinifin ana omurgasi olabilir.
   - Sorun mark sayisi degil, mark skill'lerinin ayni davranmasi.
   - Claude mark/trap/detonate rollerini ayrimlastirirsa 4 mark skill'i bile calisabilir.

5. Shadowblade movement tekrari dogru risk, fakat phase class'ta birden fazla phase araci kabul edilebilir.
   - Her phase aracinin farkli maliyet/sonuc/Scar davranisi olmasi yeterli.
   - "Phase Step" savunma, "Veil Flicker" Scar apply, "Seam Rend" delayed Scar, "Twin Carve" finisher olursa tekrar azalir.

## Eksik veya Fazla Genis Kalan Yerler

1. Feedback iyi ama onceliklendirme hala fazla genis.
   - 10 sinifta ayni anda redesign yapmak riskli.
   - Claude once sistemik kurallari kilitlemeli, sonra sinif sinif uygulamali.

2. Cross-class state listesi iyi bir baslangic ama sayi fazla.
   - Her class icin 2-3 export state daha uygulanabilir.
   - Ornek: Warblade = Sundered/Staggered/Broken; Shadowblade = Rift Scar/Collapsing; Brawler = Launched/Wall-Slammed.

3. Feedback implementation maliyetini tartmiyor.
   - Prism Beam'in Frost Wall'dan kirilmasi, Scar teleport, boss attack curse delay gibi fikirler iyi ama Unity scope/maliyet acisindan farkli seviyelerde.
   - Claude fikirleri "Faz 1 / Faz 2 / later" diye ayirmali.

4. Enemy design baglantisi eksik.
   - "Her skill encounter sorusuna cevap vermeli" dogru.
   - Bunun icin once dusman sorulari listesi lazim: shield, swarm, charger, ranged, healer, summoner, elite armor, boss telegraph vb.

## Claude Icin Onerilen Isleme Sirasi

1. Yeni revizyon yapmadan once bir "Class State Contract" yaz.
   - Her class icin: verbs, resource, 2-3 export state, 2-3 consume trigger, yasaklanan generic davranis.

2. Sonra global tekrar kurallarini kilitle.
   - Counter ayrimi.
   - Movement ayrimi.
   - Execute ayrimi.
   - Cursor-zone ayrimi.
   - Resource/rotation complexity limitleri.

3. Ardindan sadece en riskli 4 sinifi once ele al.
   - Brawler: Charge/Overdrive sadelestirme.
   - Hexer: stack davranisini enemy behavior'a baglama.
   - Elementalist: shape interactions.
   - Gunslinger veya Ranger: reload/mark-trap ayrimi.

4. Warblade'i Faz 1 demo class oldugu icin asiri bozma.
   - Iron Crush ve Death Blow kosullari revize edilsin.
   - CC output state opener'a cevrilsin.
   - Core feel korunmali.

5. Cross-class pasifleri en sona degil, her skill revizyonuyla birlikte tasarla.
   - Her skill icin soru: Ne state uretir? Hangi state'i tuketir? Hangi class bunu sever?

## Pratik Revizyon Kurali

Her skill icin Claude su 5 satirlik testi uygulayabilir:

1. Verb: Bu skill hangi class verb'ini temsil ediyor?
2. State: Hangi state'i uretir veya tuketir?
3. Slot reason: 4/6 slotta neden secilir?
4. Overlap: Ayni sinifta veya baska sinifta ayni isi yapan skill var mi?
5. Abuse: Cross-class veya resource loop abuse riski var mi?

Bu testten gecmeyen skill silinmeli, birlestirilmeli veya redesign edilmeli.

## Codex Net Karari

ChatGPT feedback'i Claude'a verilmeye deger. Dogrudan "hepsini uygula" denmemeli. En dogru kullanim:

- Feedback'i sistemik risk listesi olarak kabul et.
- State-based cross-class contract'i bir sonraki tasarim dokumani haline getir.
- Brawler, Hexer, Elementalist, Gunslinger/Ranger tarafinda oncelikli redesign ac.
- Warblade/Ravager/Ronin/Shadowblade/Summoner icin once overlap ve abuse kontrollerini yap, sonra minimal revizyon uygula.

Claude'a verilecek ana sinyal:

> ChatGPT feedback'i genelde isabetli; ancak revizyon tek tek skill silme/yazma olarak degil, once state vocabulary ve class ownership kurallari kilitlenerek yapilmali.
