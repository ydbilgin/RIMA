# Last Epoch'tan RIMA'ya Alinabilecek Sistem Fikirleri

Durum: Dusunme notu. Kilit karar degildir.
Amac: RIMA'nin sinif, skill ve kosu ici build sistemine Last Epoch'tan hangi tasarim prensiplerinin alinabilecegini degerlendirmek.

## Kisa Cevap

Last Epoch'tan RIMA'ya alinacak ana fikir loot karmasasi degil; skill derinligi, tag tabanli sinerji ve oyuncuya kontrollu build kurma hissidir.

Bu fikirler skillerin tamamini bastan degistirmek zorunda degil. En dogru model:

- Skill'in ana kimligi korunur.
- Tag, scaling ve durum etkileri sistemi eklenir.
- Dusuk/orta kademe upgrade'ler cogunlukla sayisal veya yan etki verir.
- Epic/Legendary gibi yuksek kademe upgrade'ler bazi skillerin ana davranisini degistirebilir.

Yani her skill her zaman baska bir skille donusmemeli. Ama secili kritik upgrade'lerde skill'in oynanisi anlamli bicimde degisebilir.

---

## 1. Skill Specialization Mantigi

Last Epoch'un en guclu tarafi, bir skill'in yalnizca hasarinin artmamasi; bazi secimlerle calisma biciminin degismesidir.

RIMA'da bu fikir mevcut kademe sistemine baglanabilir:

- Common / Rare: hasar, cooldown, alan, menzil, resource maliyeti gibi temel ayarlar.
- Epic: skill'e yeni bir yan mekanik ekler.
- Legendary: skill'in rota veya kullanim kimligini degistirebilir.

Ornek Warblade:

- `Cleave`: genis on ark saldirisi.
- Epic varyant: vurulan hedeflerde Bleed birakir.
- Epic varyant: ark daralir ama boss hasari artar.
- Legendary varyant: Rage doluyken ikinci bir dalga cikarir.

Bu model Slay the Spire benzeri secim hissini guclendirir. Oyuncu sadece "hangi skill daha cok hasar vurur" diye dusunmez; "bu skill'i hangi role cevireyim" diye dusunur.

---

## 2. Tag Tabanli Skill Sistemi

Last Epoch'taki tag mantigi RIMA'nin cift sinif yapisina cok uygun.

Her skill bir veya birden fazla tag tasiyabilir:

- `Melee`
- `Projectile`
- `Area`
- `Bleed`
- `Burn`
- `Shock`
- `Curse`
- `DoT`
- `Minion`
- `Mark`
- `Dash`
- `Rage`
- `Focus`
- `Mana`

Bu sayede siniflar arasi sinerji daha okunur olur.

Ornek:

| Kombinasyon | Sinerji Mantigi |
|---|---|
| Warblade + Hexer | Bleed, Curse, DoT |
| Ranger + Elementalist | Projectile, Shock, Area |
| Summoner + Hexer | Minion, Curse, DoT |
| Shadowblade + Ronin | Dash, Mark, Counter |
| Ravager + Brawler | Stagger, Heavy Hit, Knockback |

Bu sistem GW1 benzeri cift sinif felsefesini daha mekanik ve okunabilir hale getirir.

---

## 3. Durum Etkisi Ekonomisi

RIMA'da durum etkileri cok fazla olmamali. Az sayida ama net kimlikli etki daha iyi calisir.

Onerilen cekirdek durum etkileri:

| Etki | Rol | Uygun Siniflar |
|---|---|---|
| Bleed | Fiziksel DoT, stack baskisi | Warblade, Ravager, Shadowblade |
| Burn | Alan hasari, patlama, sureli baski | Elementalist |
| Shock | Zincirleme, stun esigi, hizli tetik | Elementalist, Ranger |
| Hex | Zayiflatma, curse, dusman davranisini bozma | Hexer |
| Mark | Hedef secme, kritik veya takip hasari | Ranger, Shadowblade |
| Expose | Boss veya elit kirilganligi | Ronin, Brawler |

Bu etkiler skill'lerin sadece gorsel efekti olmamali; build kararlarini etkileyen mekanik etiketler olmali.

---

## 4. Deterministik Upgrade Hissi

Last Epoch oyuncuya tamamen rastgele hissettirmeyen bir gelisim sunar. RIMA'da bu loot crafting olarak degil, kosu ici skill gelisimi olarak uyarlanabilir.

Oda odulu ornekleri:

- Bir `Bleed` tag'li skill'i yukselt.
- Bir `Projectile` skill'e sekme ekle.
- Bir primary class skill'i garanti Epic secenek havuzuna tasir.
- Secondary class'tan gelen bir skill'i mevcut tag'lerle eslestirir.
- Bir `Area` skill'in alanini buyutur ama cooldown ekler.

Bu, oyuncuya "build'im elimden kayiyor" hissi yerine "build'i yonlendiriyorum" hissi verir.

---

## 5. Monolith / Echo Fikrinin Oda Haritasina Uyarlanmasi

Last Epoch'un Monolith sistemi kisa hedef ve odul beklentisi uzerine kurulu. RIMA'da bu fikir oda haritasina cevrilebilir.

Oda modlari:

- Bleed dusmanlari guclenir, odul: fiziksel skill upgrade.
- Elit yogunlugu artar, odul: Legendary node sansi.
- Curse room, odul: secondary class secenek kalitesi.
- Sure baskisi, odul: resource upgrade.
- Projectile dusmanlari artar, odul: mobility veya defense secenegi.

Bu sistem Hades benzeri oda akisi ile uyumlu olur.

---

## Skill Ana Etkenleri Degisecek mi?

Kural onerisi:

Skill'in temel fantezisi ve sinif kimligi korunmali. Her upgrade ana etkiyi degistirmemeli.

Uc katmanli model daha saglikli:

## Katman 1 - Cekirdek Skill Kimligi

Bu katman kolay degismemeli.

Ornek:

- Warblade `Cleave`: onde genis fiziksel saldiri.
- Ranger `Pinning Shot`: hedefi sabitleyen menzilli ok.
- Hexer `Curse`: dusmani zayiflatan buyu.
- Summoner `Spirit Call`: vekil veya ruh cagirma.

Bu cekirdek kimlik korunursa oyun okunabilir kalir.

## Katman 2 - Effect / Modifier

Cogu upgrade burada kalmali.

Ornek:

- Hasar artisi.
- Cooldown azalmasi.
- Alan genislemesi.
- Bleed eklenmesi.
- Ek hedefe sicrama.
- Resource maliyeti degismesi.
- Kisa slow veya mark eklenmesi.

Bu katman skill'in ana mantigini bozmaz, sadece build yonunu belirler.

## Katman 3 - Davranis Degistiren Upgrade

Bu katman nadir olmali ve genelde Epic / Legendary seviyeye konmali.

Ornek:

- Genis alan saldirisi daralir ama tek hedef boss hasari artar.
- Projectile artik duz gitmek yerine hedefler arasinda seker.
- Dash saldirisi artik savunma araci degil, execute araci olur.
- Curse artik sureli debuff degil, patlayabilen bir mark olur.
- Summon kalici vekil yerine kisa sureli patlayici ruh olur.

Bu katman oyuncuya heyecan verir ama fazla kullanilirsa skill kimligini bulaniklastirir.

---

## RIMA Icin Pratik Karar

Mevcut skill tasarimlari tamamen cope atilmamali. Ana etkenler su anda sinif kimligini tasiyorsa korunmali.

Degismesi gereken sey:

- Skill dosyalarina tag sistemi eklenmesi.
- Upgrade seceneklerinin sadece hasar artisi olmamasi.
- Bazi skill'lerin Epic/Legendary seviyede alternatif davranis kazanmasi.
- Secondary class sinerjisinin tag'ler uzerinden okunabilir hale gelmesi.

Degismemesi gereken sey:

- Her sinifin ana oynanis rolu.
- Her skill'in temel fantezisi.
- Roguelite kosunun hizli ve okunabilir olmasi.
- Oyuncunun 55-70 dakikalik kosuda karar yorgunluguna bogulmamasi.

## Sonuc

Last Epoch etkisi RIMA'ya "daha fazla stat" olarak degil, "daha anlamli skill evrimi" olarak girmeli.

En dogru hedef:

Skill'ler temel kimligini korur; upgrade'ler build yonunu belirler; sadece yuksek kademe secimler skill'in davranisini degistirir.

