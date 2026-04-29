# ChatGPT Sınıf Tasarım Promptu — Yeni Sınıf Yarat
*"Derin araştırma" modunu aç. Bu prompt seni bir oyun tasarımcısı olarak konumlandırıyor.*

---

## KİMİM VE NE YAPIYORUM

Solo indie developer'ım (Laureth). Unity 6.3 LTS ile 6 ayda Steam'e çıkarmayı hedeflediğim bir **flat top-down 2D roguelite aksiyon oyunu** geliştiriyorum. Pixel art, solo üretim.

**Tek hedef:** Oyuncuya MMORPG'deki "bu build insane" anını — cooldown senkronizasyonu, proc zincirleri, kaynak patlaması — roguelite formatında yaşatmak.

**Referanslar:** Guild Wars 1 (dual-class derinliği) + Slay the Spire (skill acquisition) + Hades (oda yapısı, Duo Boon keşfi) + Shadow of Mordor (Grudge/Nemesis) + WoW/FFXIV/Lost Ark (sınıf kimliği ve kaynak sistemleri)

---

## MEVCUT OYUN SİSTEMLERİ

### Dual-Class Yapısı
- Run başında 8 sınıftan 2 seçilir
- Her sınıfın 12 aktif skill havuzu var (toplam 96 skill)
- "Signature + Weighted Draft" ile oda oda 4 aktif skill biriktirilir
- 28 benzersiz kombinasyonun her birinin arketip adı, cross-class pasifi ve cross-class ultimate'ı var

### Tag Sistemi (her skill 1-2 tag taşır)
| Tag | Sembol | Tanım |
|-----|--------|-------|
| ANCHOR | ⚓ | Tek başına güçlü |
| OPENER | ▶ | Zincirin ilk adımı |
| CHAIN | ⚡ | Belirli skill sonrası bonus |
| BUILDER | ↑ | Kaynak/setup üretir |
| SPENDER | ↓ | Kaynağı harcar |
| FINISHER | 💥 | Koşullu büyük hasar |
| CONTROL | ⬡ | CC (başlangıçta gizli) |
| AMPLIFIER | ✦ | Diğer skill'lerin gücünü artırır (başlangıçta gizli) |

### Skill Tier
- **Core (8 skill):** Tek başına güçlü, erken odalarda ağırlıklı
- **Advanced (3 skill):** Sinerji gerektirir
- **Master (1 skill):** 2+ class skill olmadan offer pool'a girmez

### Kaynak Sistemi
Her sınıfın kendine özgü kaynağı var, [V] tuşuyla burst tetiklenir.

---

## MEVCUT 8 SINIF — BUNLARI TEKRARLAMA

Aşağıdaki 8 sınıf zaten tasarlandı. Yeni önerilerinin bunlarla **doğrudan çakışmaması** gerekiyor:

| Sınıf | Core Fantasy | Kaynak | [V] Burst |
|-------|-------------|--------|-----------|
| **Warblade** | "Duruyorum, geçemiyorsun" | Rage (hasar al/ver) | Bladestorm 5s spin |
| **Elementalist** | "Ritim + elementleri yönetiyorum" | Mana + Fire/Frost State | Inferno arena-wide ateş |
| **Rogue** | "Görmüyorsun, zaten geç" | Energy + Combo Points | Shadow Dance 8s stealth |
| **Ranger** | "Sana ulaşamazsın" | Focus (mesafe bazlı) | Rain of Arrows |
| **Brawler** | "Az canken daha tehlikeliyim" | Fury (sadece hasar alarak) | Berserk Mode |
| **Paladin** | "Kesilemem, bu bana güç veriyor" | Holy Power (Builder/Spender) | Avenging Wrath |
| **Summoner** | "Feda ediyorum — en güçlü an bu" | Charges (minyon ölümüyle) | Army of Dead |
| **Hexer** | "Sabır. 10'a gelince sen bitiyorsun" | Hex Stacks (0-10/düşman, 4 faz) | Hex Cascade |

**Kapsanan temalar:** melee tank, caster/elemental, stealth melee, ranged kite, berserker, holy tank, necromancer, debuffer.

---

## GÖREVIN

### BÖLÜM 1 — NEDEN YENİ SINIF?

Önce şunu araştır ve yanıtla:

1. **Hangi MMORPG arketipleri yukarıdaki 8'de YOK?**
   - Referans: WoW (12 sınıf), FFXIV (21 job), Guild Wars 1 (10 sınıf), Lost Ark (15+ class), Path of Exile (19 ascendancy)
   - Hangi "core fantasy" boşta kaldı? Örnekler: zamancı, şekil değiştiren, kan büyücüsü, taktisyen, büyü yansıtıcı, hız/kinetik...

2. **Mevcut 28 dual-class kombinasyonunda hangi "his" eksik?**
   - Örneğin: "hız + hız = saf kinetik" kombosunu yaratacak bir sınıf yok
   - Oyunun duygusal paleti tamamlanmış mı, yoksa belirgin bir eksik var mı?

3. **Solo dev için uygulanabilirlik:**
   - Yeni sınıfın kaynağı ve burst mekaniği Unity 2D'de karmaşık olmayan bir şekilde implemente edilebilmeli
   - Tamamen yeni fizik/shader gerektiren önerilerden kaçın

---

### BÖLÜM 2 — 2 YENİ SINIF TASARLA

Her biri için tam tasarım şeması üret. Şu kurallar geçerli:

**Zorunlu kurallar:**
- Mevcut 8 sınıfın core fantasy'siyle DOĞRUDAN çakışmamalı
- Kaynak sistemi mevcut 8'den farklı bir mantıkta çalışmalı
- Her sınıf 12 aktif skill içermeli (8 Core, 3 Advanced, 1 Master)
- Her skill 1-2 tag taşımalı, mevcut tag sistemiyle uyumlu olmalı
- Her skill'in bir Chain Koşulu → Bonus'u olmalı
- Dual-class'ta mevcut 8 sınıfla en az 3 "çok güçlü" kombinasyon üretmeli

**Tasarım formatı:**

```
## [EMOJİ] [SINIF ADI]

**Core Fantasy:** "[Tek cümle — oyuncunun kafasındaki his]"
**MMORPG Arketipi:** [Hangi oyunlardaki hangi sınıftan ilham aldı]
**Kaynak Sistemi:** [İsim (min-max) + Nasıl dolar + Nasıl boşaltılır]
**[V] Burst:** [İsim — Koşul: efekt + süre]
**Dual-class Pozisyonu:** PRIMARY mi olur, SECONDARY mi, ya da her ikisi de?

### 12 Aktif Skill Tablosu

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | ... | ... | Core | ... | ... |
| ... | | | | | |
| 12 | ... | ... | Master | ... | ... |

### Build Eksenleri (en az 3)
- **"[İsim]"** → [Skill A + B + C + D] — [Neden "insane build" hissi veriyor]

### Mevcut Sınıflarla En Güçlü 5 Kombinasyon
| Dual-Class | Arketip Adı | Neden Güçlü | Cross-Class Pasif Önerisi |
|-----------|-------------|-------------|--------------------------|

### Tasarım Gerekçesi
- Bu sınıf hangi boşluğu dolduruyor?
- Oyuncunun "bu sınıfı ilk kez seçtiğinde" hissi nasıl olmalı?
- "Bu build insane" anı bu sınıfta nasıl tetikleniyor?
```

---

### BÖLÜM 3 — YENİ SINIFLARLA GENİŞLETİLMİŞ KOMBİNASYON HARİTASI

2 yeni sınıf eklendikten sonra:

1. Toplam sınıf sayısı: 10 → kombinasyon sayısı: C(10,2) = 45. Kaç yeni kombinasyon eklendi?
2. Bu yeni kombinasyonların **en az 3'ü** için tam arketip kartı yaz:
   ```
   Arketip Adı: [...]
   Core Fantasy'si: [...]
   Cross-Class Pasif: [...]
   Cross-Class Ultimate: [...]
   "Bu build insane" anı: [Hangi koşulda, nasıl]
   ```
3. Yeni sınıflar mevcut 28 kombinasyonun hangi "duygusal tonunu" tamamlıyor?

---

### BÖLÜM 4 — HEROIC/LEGENDARY SKİLL (YENİ SINIFLAR İÇİN)

"The Awakened Link" sistemini (önceki araştırmadan biliyorsun) kullanarak:

Her yeni sınıf için 1 Heroic skill yaz:
```
Sınıf: [...]
Heroic Skill Adı: [...]
Efekt: [Oyunda daha önce mümkün olmayan bir şey yapmalı]
Kazanma Koşulu: [Skill/build ustalığını ödüllendirmeli]
Neden efsanevi: [Bu skill olmadan yapılamayan nedir]
```

---

## ARAŞTIRMA KAYNAKLARI

Bu kaynakları kullanarak yeni sınıf önerilerini destekle:

**Boşluk analizi için:**
- r/MMORPG, r/rpg, r/gamedesign: "what class archetype do you wish existed?"
- Guild Wars 1 ve 2 sınıf tasarım retrospektifleri
- Path of Exile 2 sınıf tasarım notları
- FFXIV Job tasarım felsefesi (Yoshi-P röportajları)
- Lost Ark class design topluluk tartışmaları

**Kaynak sistemi ilhamı için:**
- "Most satisfying resource system in MMOs" Reddit tartışmaları
- Hades gauge sistemi — Darkness/Darkness+
- Sifu "Wude" mekaniği — zaman yönetimi kaynak olarak

**Dual-class sinerji için:**
- Grim Dawn mastery kombination guides
- r/GuildWars "best secondary profession" tartışmaları

---

## ÇIKTI FORMATI

Yanıtını şu sırayla ver:
1. BÖLÜM 1 — Boşluk Analizi (kısa, max 300 kelime)
2. BÖLÜM 2 — Sınıf A tam tasarım
3. BÖLÜM 2 — Sınıf B tam tasarım
4. BÖLÜM 3 — Kombinasyon haritası
5. BÖLÜM 4 — Heroic skill önerileri

**Tüm yanıtı Türkçe yaz.**
**Sayısal değerler kullan** — "güçlü hasar" değil, "%200 hasar" gibi.
**Solo dev kapsamını göz önünde bulundur** — 12 skill uygulanabilir olmalı, her biri farklı bir Unity 2D sistemi gerektirmemeli.
