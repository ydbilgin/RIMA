# RIMA — Visual Identity & Build Görselleştirme Tasarımı
*Oluşturulma: 2026-04-12 | Review: ChatGPT + Gemini bekliyor*

> Amaç: Oyuncu ne seçerse, karakter bunu yansıtsın.
> "Build'im görünüyor" hissi — mekanik derinliğin görsel yüzeyi.
> Küçük eklemeler, büyük kimlik.

---

## 0B. ONAYLANAN KURALLAR (review sonrası — bunlar değişmez)

**Görsel öncelik sırası:**
```
1. Tehlike (hasar, CC, ölüm riski)
2. Oynanış durumu (buff aktif mi, kaynak nerede)
3. Build kimliği (hangi ekol, secondary)
4. Kozmetik tatmin (kill juice, trail, aksesuarlar)
```
Üstteki katman her zaman alttakinden daha okunur olmalı.

**Renk kuralı:** Karakter üzerinde aynı anda max 2 dominant renk.
- Çekirdek sınıf rengi: daima var, güçlü
- Buff/secondary rengi: ince, geçici veya aksesuarda
- 3. renk: çok küçük accent, asla dominant değil

**Kill juice stratifikasyonu:**
- Normal kill: hafif (2-3f hitstop, küçük kan)
- Execute kill: güçlü (5f hitstop, büyük kan, micro-shake)
- Streak (5+ kill): UI tarafında küçük gösterge — altın flash değil
- Altın flash sadece Duo Skill açılımı veya Relic alımı gibi nadir anlarda

**Efekt yoğunluk kuralı:**
- Persistent (sürekli) efektler: düşük yoğunluk, dikkat çekmiyor
- Burst (anlık) efektler: kısa ama güçlü
- Gameplay-critical efektler: renk öncelikli, net okunur
- Kozmetik efektler: desatüre ve küçük

**Faz 1 sınırı:** Sadece shader + particle + trail. PixelLab gerektiren hiçbir şey.

---

## 0. FELSEFE

Bir oyuncu 20 dakika oynadıktan sonra ekran görüntüsü alsa,
o ekran görüntüsünden hangi build oynadığı anlaşılmalı.

Path of Exile bunu karakter üzerindeki efektlerle yapar.
Hades bunu weapon görünümü + renk paleti ile yapar.
Dead Cells bunu weapon animasyonu + trail ile yapar.

RIMA'nın cevabı: **Partiküller + Renk Tonları + Küçük Sprite Eklemeleri + Dünya Reaksiyonu.**

Hiçbir şey büyük veya pahalı olmamalı.
Her biri tek başına küçük, hepsi birlikte "bu benim karakterim" hissi yaratmalı.

---

## 1. KAYNAK DURUMU — DİNAMİK GÖRSEL

Karakterin mevcut kaynak seviyesi görünür olmalı. Stat bar'da değil, karakter üzerinde.

### Rage (Warblade primary)

| Rage seviyesi | Görsel |
|--------------|--------|
| 0-30 | Normal. Hiç efekt yok. |
| 31-60 | Gözlerde hafif kırmızı ışık. Nefes yoğunlaşıyor (buhar partikülü). |
| 61-80 | Omuzlardan kırmızı-turuncu parıltı. Silah kenarı ısınıyor (ince glow). |
| 81-99 | Tüm vücutta titreyen kırmızı halo. Ayaklar altında küçük kıvılcım. Adım sesleri daha ağır. |
| 100 (BLADESTORM hazır) | Sürekli dönüp duran kırmızı ok partikülleri. Çevredeki zemin hafif çatlıyor. |

**PixelLab gereksinimi:** Hayır. Tümü Unity particle system + sprite shader (emission). 

---

### Mana (Elementalist secondary)
| Seviye | Görsel |
|--------|--------|
| 0-30 | Soluk mavi parıltı parmak uçlarında |
| 60+ | Küçük arcane rünler karakter etrafında yavaşça döner |
| 100 | Mavi alev saç/omuz efekti |

### Energy (Shadowblade secondary)
| Seviye | Görsel |
|--------|--------|
| 0-30 | Hiç efekt — bu da bir mesaj: "kaynak yok" |
| 60+ | Mor gölge iz adım adım (0.3s kaybolur) |
| 100 | Karakter hafif transparan + gözler mor |

### Focus (Ranger secondary)
| Seviye | Görsel |
|--------|--------|
| 60+ | Yeşil nişan noktası gözlerde |
| 100 | Ok/mermi yolunun önünde ince yeşil ışık hattı |

---

## 2. LMB EKOL — SİLAH VİZYEL KİMLİĞİ

Seçilen ekol, silahın görünümünü değiştirir.
Sprite değişimi gerekmez — sadece shader/particle farklılaşır.

| Ekol | Silah görünümü |
|------|---------------|
| Fury Strikes (Rage) | Kırmızı-turuncu kor hattı silah kenarında. İsabet anında kıvılcım. |
| Savage Edge (Bleed) | Kırmızı damlalar silah yüzeyinde — hareket ederken saçılır. |
| Bone Breaker (Execute) | Beyaz-gri çatlak dokusu silahta. HP<%30 düşmana darbe anında beyaz flash. |

**Lv2'de:** Efekt yoğunlaşır (daha fazla partikül, daha parlak).
**Lv3'te:** Kalıcı ambient efekt — silah "yaşıyor" hissi verir.

---

## 3. SEÇİLEN SECONDARY CLASS — KARAKTER TONU

Secondary seçildiğinde karakter üzerinde kalıcı ama ince bir visual katman eklenir.
"Bu oyuncu cross-class oynuyor" anlaşılmalı ama dikkat dağıtmamalı.

| Secondary | Görsel katman |
|-----------|--------------|
| Elementalist | Parmaklarda küçük ateş veya buz kristali (elemente göre). Zırht üzerinde runik ışık hattı. |
| Shadowblade | Hafif mor aura. Gölge "bir adım geride" kalır. Ayak izi mor renk. |
| Ranger | Gözlerde yeşil parıltı. Omuzda görünmez yay aksesuarı (sadece silüette fark edilir). |
| Ravager | Omuzlarda kan sıçraması izi. Dişlerde hafif glow (kızgın). |
| Ronin | Katana ruhu — hareket ederken ince beyaz iz. Durunca iz kaybolur. |
| Gunslinger | Belde silah duman izi. Heat yüksekken barrel glow. |
| Brawler | Yumrukta bantlar/yaralar görünür. Hasar verince knuckle spark. |

**PixelLab gereksinimi:** EVET, bazıları için.
Omuz aksesuarı, katana ruhu gibi "sprite ekleme" işleri PixelLab'da Edit Image PRO ile yapılabilir.
~2-4 gen/secondary class. Faz 2'de planlanacak.

---

## 4. PASİF KATMANLARIn GÖRSEL ETKİSİ

Pasif Lv3'e ulaşınca küçük bir görsel işaret eklenir. "Bu pasifi maxladım" göstergesi.

| Pasif | Lv3 görsel |
|-------|-----------|
| Iron Body | Zırht üzerinde taş/demir doku parıltısı. Kalıcı ama ince. |
| Blood Drinker | Kill anında küçük kırmızı kan absorbsiyon efekti (karaktere doğru akar). |
| Predator's Eye | Gözlerde altın parlama. Düşmana bakınca ince hedef çizgisi. |
| Ironclade Momentum | Hareket halinde mavi-gri momentum izi. Durduğunda anında kaybolur. |
| Unyielding | HP<%50 iken beyaz kristal parçaları döner. Tetiklenince tam beyaz flash. |
| Adrenaline Rush | Kill sonrası 3s sarı-altın hız aura. |
| Ancient Instinct | Saldırı algılanınca çevresinde mor halka (0.2s, uyarı işareti). |

---

## 5. AKTİF BUFF DURUMU — "BUFF GÖRÜYORUM"

Bazı skillerin buffları aktifken karakter görünümü değişir.

| Buff | Görsel |
|------|--------|
| Iron Crush aktif (6s) | Tüm vücutta turuncu kor. Darbe anında kor parlar. |
| Battle Surge aktif (8s) | Yeşil nabız efekti (heal lopu). Her kill'de yeşil parıltı. |
| Evasion aktif (4s) | Mor transparan kopya — dodge anında iz bırakır. |
| Vanish | Tam transparan. Yalnız ince mor outline kalır (oyuncu için). |
| Iron Counter penceresi | Mavi kalkan efekti. Mükemmel blok → beyaz patlama. |
| Death Wish aktif (5s) | Tüm vücut kırmızı. HP bar yanıp söner. Ses daha derin. |
| Lich Form aktif | Tam ghost görünümü. Minyonlar da aura alır. |

---

## 6. HAREKET İZLERİ — BUILD'E GÖRE TRAIL

Karakter koşarken iz bırakır. İz, build kimliğini yansıtır.

| Build/Class | Trail tipi |
|------------|-----------|
| WB solo | Kırmızı kıvılcım (küçük, kaybolur) |
| WB + Elem | Ateş veya buz kıvılcımı (aktif elementine göre) |
| WB + Shadow | Mor gölge siluet iz (0.4s) |
| WB + Ranger | Yeşil focus izi (çok ince, neredeyse yok) |
| Fury Strikes Lv3 | Turuncu parlamalı iz |
| Savage Edge Lv3 | Kırmızı damlalar iz boyunca |

**Teknik:** Basit `TrailRenderer` + renk gradient. Her build kombinasyonu için ayrı renk tonu.

---

## 7. DÜNYA REAKSİYONU — ZEMIN VE ÇEVRE

Karakter güçlendikçe dünya da tepki verir.

| Durum | Çevre reaksiyonu |
|-------|-----------------|
| Rage 100 (BLADESTORM hazır) | Yakındaki zemin çatlar (ince crack line partikülü) |
| War Stomp kullanımı | Zemin titrer + 3m çevredeki küçük nesneler savrulur |
| Iron Crush aktif + hareket | Adım adım zemin baskısı (crush efekti, 0.2s) |
| Death Blow kullanımı | Kill anında kamera 3 frame dondurur (hitstop) + zemin kan lekesi |
| Lich Form aktif | Çevredeki ışıklar kararır. Yakın torchlar söner. |
| Berserk Mode aktif (Ravager) | Çevre kırmızıya kayar (post-process tint, hafif) |
| Blizzard cast | Yerdeki su/kan efektleri donar |

---

## 8. KİLL ANI — JUICE

Kill "tatmin" anı en önemli feedback loop'u. Her kill aynı hissettirmemeli.

| Kill tipi | Görsel |
|-----------|--------|
| Normal kill | Kısa hitstop (2-3 frame) + kan efekti |
| Execute kill (Death Blow) | Uzun hitstop (5 frame) + büyük kan + kamera micro-shake |
| Backstab kill | Sessiz ölüm — az kan, hızlı dissolve |
| Bleed kill | Yavaş çöküş — karakter hâlâ kanıyor |
| Yanma kill | Kül dissolve efekti — yanarak dağılır |
| Dondurma kill | Kırılma efekti — buz heykeli parça parça |
| 5'li kill streak | Altın flash + kill counter göstergesi |
| Boss kill | Özel ölüm animasyonu (kendi anim) + yavaş düşüş |

---

## 9. LEVEL-UP / SKILL ALIMI ANI

Draft ekranında skill seçilince küçük bir "kazandım" anı.

| Durum | Görsel |
|-------|--------|
| Normal aktif skill alımı | Kart parlar, karakter etrafında kısa ışık halkası |
| Pasif Lv2 alımı | Pasif ikonu parlıyor, kısa renk bulutu |
| Pasif Lv3 (MAX) alımı | Daha güçlü patlama + "MAX" yazısı float eder |
| Sinerji skill açılımı | Özel animasyon — iki renk birleşir, spiral efekt |
| Duo skill açılımı | Farklı ses + altın-mor özel patlama efekti |
| LMB ekol seçimi (Forge) | Silah değişim animasyonu — kısa shader geçişi |
| Relic alımı | Boss alanında büyük ışık sütunu + relic ikonu float |

---

## 10. HASAR ALMA — "BU ACIDI" GÖSTER

Oyuncu hasar aldığında ekran + karakter tepki verir.

| Durum | Görsel |
|-------|--------|
| Normal hasar | Kırmızı vignette flash (0.1s), karakter kırmızı flicker |
| Kritik hasar (HP <%30) | Vignette kalıcılaşır, karakter tonusu grileşir |
| Bleed altında | Küçük kırmızı damlalar ekranda (köşelerde) |
| Freeze altında | Karakter buz kristali kaplı (shader), hareket yavaşlar |
| CC altında | Karakter başında dönüşlü ikon (yıldız vs.) |
| Unyielding tetiklendi | Tüm ekran beyaz flash (0.15s) + karakter kristal görünümü |
| HP = 1 (Death Wish) | Ekran siyah-kırmızı, nabız efekti, ses derin |

---

## 11. KÜÇÜK AMA ETKİLİ EKLEMELEr (QUICK WIN LİSTESİ)

Bunlar kolay yapılır, büyük etki yaratır:

| Ekleme | Etki | Zorluk |
|--------|------|--------|
| Rage 80+ vignette (kırmızı kenar) | Gerginlik hissi | ★☆☆ |
| Kill sonrası 0.1s time slow | Tatmin | ★☆☆ |
| Adım tozu (stop dust) | Hareket gerçekçiliği | ★☆☆ |
| Silah ekol renk değişimi | Build kimliği | ★★☆ |
| Trail renderer (build bazlı) | Build kimliği | ★★☆ |
| Secondary seçimi sonrası karakter renk tonu değişimi | Kimlik | ★★☆ |
| Passive MAX animasyonu | Tatmin | ★☆☆ |
| Boss kill yavaş düşüş | Dramatik an | ★★★ |

---

## 12. GELECEK FAZ FİKİRLERİ (şimdi yapılmaz ama not edilsin)

- **Armor evolution:** 3+ pasif alınca karakter görünümüne küçük armor parçası eklenir (sprite ekleme — PixelLab)
- **Title effect:** Çok güçlü build kurulunca karakter başında ufak ikon (taç, kafatası, ateş vs.)
- **Room entry aura:** Oda kapısı açılınca karakter kısa ışık ile girer — güç göstergesi
- **Relic fiziksel görünümü:** Alınan relic karakter üzerinde görünür (kemik, taş, kristal)
- **Buff stack göstergesi:** Pasif Lv3 + ekol Lv3 + buff aktif → karakter "radiant" görünümü alır

---

## REVIEW SORULARI (ChatGPT + Gemini için)

1. **Hangi görsel katman en önce yapılmalı?** Rage renk sistemi mi, ekol silah efektleri mi, yoksa kill juice mı?
2. **Secondary class renk değişimi:** Çok kalabalık görünür mü? WB'nin kendi kırmızısına Shadowblade'in moru eklenirse okunabilir mi?
3. **Dünya reaksiyonu (zemin çatlama, ışık sönme):** Performans açısından risk var mı? Sıklıkla kullanılırsa dikkat dağıtır mı?
4. **Kill juice seviyesi:** 5 kill streak altın flash çok mu gösterişli? Roguelite kill başına bu kadar feedback gerekli mi?
5. **PixelLab ile sprite ekleme (relic görünümü, armor evolution):** Bu tür görsel identity Faz 1'de gerekli mi yoksa erken mi?
6. **Karşı argüman:** "Tüm bu partiküller çok gürültülü olur, savaşta neyin ne olduğunu anlayamazsın" — bu endişeyi nasıl yönetirsin?

---

## PixelLab GEREKSİNİM ÖZETİ

| Eleman | PixelLab Gerekli? | Gen Tahmini | Faz |
|--------|------------------|------------|-----|
| Rage renk/glow | ❌ Shader | 0 | 1 |
| Trail renderer | ❌ Unity built-in | 0 | 1 |
| Kill kan efektleri | ❌ Mevcut VFX | 0 | 1 |
| Ekol silah rengi | ❌ Shader | 0 | 1 |
| Secondary karakter tonu | ❌ Shader | 0 | 1 |
| Omuz aksesuarı (sprite) | ✅ Edit Image PRO | ~4 gen | 2 |
| Armor evolution parçaları | ✅ Edit Image PRO | ~8 gen | 3 |
| Relic fiziksel görünümü | ✅ Edit Image PRO | ~6 gen | 3 |
| Boss kill animasyonu | ✅ Interpolate | ~8 gen/boss | 2 |

**Faz 1 için PixelLab sıfır.** Tümü kod + shader ile yapılabilir.

---

*Bu belge ChatGPT + Gemini review sonrası güncellenecek.*
*Kabul edilen kararlar → CURRENT_STATUS + implementasyon listesi*
