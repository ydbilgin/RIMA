# Gemini Sınıf & Skill Promptu
*Gemini web'e yapıştır. Derin araştırma modunu aç.*

---

## BEN KİMİM

Solo indie developer (Laureth). Unity 6.3 LTS ile **flat top-down 2D roguelite aksiyon** oyunu yapıyorum. 6 ayda Steam. Tek hedef: MMORPG'nin "bu build insane" anını — cooldown senkronizasyonu, proc zincirleri, kaynak patlaması — roguelite formatında yaşatmak.

**Referanslar:** Guild Wars 1 dual-class + Slay the Spire acquisition + Hades oda yapısı + WoW/FFXIV/Lost Ark sınıf kimliği.

---

## SİSTEM ÖZETİ

- Run başında 8 sınıftan 2 seçilir, 12'şer skill havuzu karışık sunulur
- Oyuncu oda oda **4 aktif skill** biriktirir ("Signature + Weighted Draft")
- Her skill **1-2 tag** taşır: ⚓ANCHOR ▶OPENER ⚡CHAIN ↑BUILDER ↓SPENDER 💥FINISHER ⬡CONTROL ✦AMPLIFIER
- Tier: Core (8) / Advanced (3) / Master (1)
- Her skill bir **Chain Koşulu → Bonus** içerir

---

## MEVCUT 8 SINIF

| Sınıf | Core Fantasy | Kaynak |
|-------|-------------|--------|
| Warblade | "Duruyorum, geçemiyorsun" | Rage (hasar al/ver) |
| Elementalist | "Ritimle her şeyi yakıyorum" | Mana + Fire/Frost State |
| Rogue | "Görmüyorsun, zaten geç" | Energy + Combo Points |
| Ranger | "Sana ulaşamazsın" | Focus (mesafe bazlı) |
| Brawler | "Az canken daha tehlikeliyim" | Fury (sadece hasar alarak) |
| Paladin | "Kesilemem, bu bana güç veriyor" | Holy Power (Builder/Spender) |
| Summoner | "Feda ediyorum — en güçlü an bu" | Charges (minyon ölümüyle) |
| Hexer | "Sabır. 10'a gelince sen bitiyorsun" | Hex Stacks (0-10/düşman, 4 faz) |

---

## GÖREV — 3 BÖLÜM

### BÖLÜM 1: MEVCUT SINIF SORUNLARI + GÜNCELLEME

Şu tespitler yapıldı, her biri için somut öneri ver:

**A. Warblade + Brawler çok benzer:**
Her ikisi melee, her ikisinde CC ve knockback var, Whirlwind skill'i her ikisinde tekrar ediyor.
→ Soru: Bu iki sınıfı yeterince farklı kılmak için Warblade'e ne eklenmeli/çıkarılmalı? Yoksa Warblade tamamen farklı bir sınıfla mı değiştirilmeli? Eğer değiştirilecekse hangi arketip?

**B. Şu sınıflar için hâlâ eksik kalan ikinci skill önerisi:**
Her biri için somut öneri yaz (isim, tag, tier, efekt, chain koşulu):
- Ranger: Spirit Wolf yerine ne gelmeli?
- Brawler: War Cry yerine ne gelmeli?
- Paladin: Blessed Weapon yerine ne gelmeli?
- Summoner: Sacrificial Pact yerine ne gelmeli?
- Hexer: Silence Hex yerine ne gelmeli? (Enervate kalıyor, Silence Hex gidiyor)

**C. Duygusal palet boşluğu:**
Mevcut 8 sınıfta şu temalar yok:
- Hız/kinetik saf fantezi (hız = güç)
- Zaman/ritim manipülasyonu
- Tamamen terrain/taktik odaklı

Bu boşluklar önemli mi? Mevcut 8'den birini revize mi, yoksa yeni sınıf mı?

---

### BÖLÜM 2: 1-2 YENİ SINIF TASARLA

Mevcut 8'i tamamlayan, **onlarla doğrudan çakışmayan** 1-2 yeni sınıf tasarla.

**Kısıtlar:**
- Kaynak sistemi mevcut 8'den farklı mantıkta çalışmalı
- Solo dev → Unity 2D'de karmaşık olmayan implementation
- Dual-class'ta mevcut sınıflarla en az 3 güçlü kombinasyon üretmeli

**Her sınıf için:**
```
Sınıf Adı:
Core Fantasy: (tek cümle)
MMORPG Referansı: (hangi oyunun hangi sınıfından ilham)
Kaynak Sistemi: (isim, nasıl dolar/boşalır)
[V] Burst: (koşul + efekt + süre)

12 Skill Tablosu:
| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |

Build Eksenleri (en az 3):
Mevcut sınıflarla en güçlü 3 kombinasyon + arketip adı:
```

---

### BÖLÜM 3: HEROİC/LEGENDARY SİSTEM KARARI

Önceki araştırmadan "The Awakened Link" sistemi önerildi:
- 1. Heroic: Act 1 boss sonrası, Boss Soul Ascend seviyesinde
- 2. Heroic: Cross-class pasif 20 kez tetiklenince (7. Transcendent Slot)

**Bunu değerlendir:**
1. Bu sistem solo dev için uygulanabilir mi? (basit flag-based implementation mümkün mü?)
2. "Act 1 boss + Cross-class milestone" kombinasyonu doğru zamanlama mı? Risk of Rain 2 kırmızı item ve Hades Legendary Boon ile kıyasla.
3. Yeni tasarladığın sınıflar için birer Heroic skill öner.

---

## ÇIKTI FORMATI

Türkçe yaz. Sayısal değerler kullan ("%200 hasar", "4s süre" gibi). Kısa tut — her bölüm maksimum bir sayfaya sığsın. Araştırma özeti değil, **somut öneri** istiyorum.
