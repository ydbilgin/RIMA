# Oyun Tasarım Sentez Master Planı
*Son güncelleme: 2026-03-30 — Hikaye/epilog güncellendi, dosya haritası yenilendi*

> Bu belge tek referans noktasıdır.
> Diğer dosyalar bölüm detaylarını içerir; bu belge karar özeti + üretim sırasıdır.

---

## 1. Oyunun Ana Kimliği

> "Run tek bir primary class ile başlar, ilk boss sonrası random secondary class ile kırılır,
> build oda oda büyür ve sonunda delice bir şeye dönüşür."

**Oyunun ruhu:** MMORPG'deki "bu build insane" anını — cooldown senkronizasyonu,
proc zincirleri, kaynak patlaması — roguelite formatında yaşatmak.

**Referanslar:** Guild Wars 1 (dual-class) + Slay the Spire (skill acquisition) +
Hades (oda yapısı, boon sistemi) + Enter the Gungeon (harita hissi) +
WoW/FFXIV/Lost Ark (sınıf kimliği)

---

## 2. Run Akışı (Final)

```
HUB → 1 primary class seç → Act 1 (6-7 oda, sadece primary)
     → Act 1 Boss Kill
     → 2 random secondary class kartı → 1 seç
     → +2 aktif slot açılır + Cross-class Pasif aktif
     → Act 2 (8-9 oda, karışık draft)
     → Act 2 Boss Kill
     → Cross-class Ultimate açılır
     → Act 3 (10-11 oda, tam dual-class)
     → Act 3 Boss Kill → Final Boss kilidi
     → Final (5 oda + boss)
     → Ölüm: HUB'a dön | Zafer: hikaye sonu
```

### Draft Oranı

| Aşama | Primary | Secondary | Nötr |
|-------|---------|-----------|------|
| Act 1 | %100 | — | — |
| Act 2 başı | %60 | %25 | %15 |
| Act 2 ortası | %50 | %35 | %15 |
| Act 3 | %45 | %45 | %10 |

> Nötr = iki class'a da yarar cross-class köprü skill'leri

---

## 3. Harita Sistemi

- **Kısmi görünür harita:** Tam StS gibi açık değil, tam Hades gibi kör de değil
- Mevcut floor görünür (5-7 oda), ötesi kapalı
- Kapılar oda tipini gösterir

### Yırtık Harita Mekaniği
- Her oda temizlenince harita parçası düşer (zorunlu al — Hades mantığı)
- 1 saniyelik **ritual animasyon**: parça yerine oturur, mürekkep reveal
- **Puzzle yok** — sadece estetik + okunur hale gelme
- Parça alınmadan ilerlenemez

---

## 4. Oda Tipleri

| İkon | Tip | Açıklama |
|------|-----|----------|
| ⚔️ | Combat | Standart, her oda sonrası skill offer |
| 💀 | Elite | Daha zor, Rare+ ödül + küçük HP yenilemesi |
| 👁️ | Spirit Encounter | Ruh varlık → koşullu build bonusu |
| 🛒 | Shop | HP, max HP, tier atlatma, gear |
| 🌀 | Curse Gate | HP harca → büyük build bonusu |
| 🎲 | Event | İki seçenekli hikaye/karar odası |
| ❓ | Unknown | İkon yok. İçinde ne var bilinmez. |

**Rest odası yok** — HP dağıtık: shop + elite drop + event + curse gate

**Unknown oda içeriği:** Combat %25, mini-boss %20, gizli shop %15, spirit %15, tuzak %10, max HP oda %10, minor reward %5. Boş oda yok.

---

## 5. Spirit Encounter Sistemi

Hades'teki tanrı odalarının işlevi → dünyaya özgü ruh varlıkları ile karşılanır.
**Farkı:** Koşullu offer — nasıl oynadığın ne teklif göreceğini etkiler.

| Ruh | Tema | Koşullu Offer |
|-----|------|---------------|
| 🔥 Forge Wraith | Saldırı, zırh kırma | Koşulsuz — her zaman 2-3 seçenek |
| 🐾 Shadow Hound | Hareket, reposition | Son odayı 5s'de temizlediysen +1 seçenek |
| 🩸 Blood Oracle | HP trade, lifesteal | HP %60 altındaysa en güçlü seçenek görünür |
| 👁️ Void Seer *(Faz 2)* | Kaos, şans | İkonlar kapalı — kör seçim |
| ⚔️ Fallen Champion *(Faz 2)* | Kombo, chain | Son 3 odayı chain ile geçtiysen unlock |
| 💀 Ancient Relic *(Faz 3)* | Güçlü ama bedelli | Her offer'a curse eşlik eder |

---

## 6. Boss Sistemi

### Genel Felsefe
Hades'te boss'lar narrative-first (tanrısallar, diyaloglar). Bu oyunda **mechanic-first**:
her boss, build'inizin bir zayıflığını test eder. Narratif bunu destekler, önde yürümez.

### Act 1 — Rotating Guardians (3 varyant, Hades Fury mantığı)

Her run Act 1 boss'u bu üçünden biridir:

| Boss | Mekanik Odak | Nasıl Zorlıyor |
|------|-------------|----------------|
| **The Iron Warden** | Kalkan/zırh | Hasar patlamalarını bloke eden dönemsel kalkan. Kırılmalı. |
| **The Void Warden** | Teleport + alan | Kaybolur, sahada void zone'lar bırakır, geri gelir. Pozisyon şart. |
| **The Chain Warden** | Bağlama + kısıtlama | Zincir fırlatır. Hareketsiz kalırsan Fury/Rage/Momentum kaybı. |

**Neden döner:** Oyuncunun Act 1'i ezberleyip geçmesini önler. Her run ilk boss farklı hissettirse de aynı zorluk seviyesindedir.

**Fark Hades'ten:** Diyalog yok, narrative yok — saf mekanik fark. Sonraki fazlarda lore eklenebilir.

### Act 2 — The Fractured King (sabit, 2 faz)

- **Faz 1:** Kırık dünya kalıntılarından oluşan construct. Normal savaş.
- **Faz 2 tetikleme:** HP %50'ye düştüğünde DEĞİL — oyuncu 10 saniye içinde 3+ farklı skill kullandıysa faz açılır. (Agresif ve çeşitli oynanışı ödüller)
- **Faz 2:** Tamamen farklı saldırı seti. Yapısı dağılır, her parçası bağımsız döner.

### Act 3 — The Hollow Sovereign (build-adaptive)

Bu boss mevcut equipped skill'lerinize göre adaptasyon yapar:

| Senin Build'in | Boss'un Adaptasyonu |
|---------------|---------------------|
| Çoğunlukla AoE skill | Zırh katmanı + sıkı gruplaşma |
| Single target ağırlıklı | Birden fazla bağımsız hedef spawnar |
| HP harcayan build | Lifesteal'i bloke eden alan |
| Uzun CD'li skill ağırlığı | Sık interrupt attakları |

Bu mekanik roguelite'a çok uygun çünkü her run farklı build = farklı boss davranışı = tekrar oynanabilirlik.

**Implementasyon:** Boss script, equipped skill tag'leri okur (FINISHER ağırlığı var mı? CONTROL çok mu?), davranış ağırlıklarını buna göre ayarlar. Tag sayacı = int array, 3 if-check yeterli.

### Final Boss — The Nexus Core (multi-phase, build-aware)

- **Faz 1:** Primary class'ının signature skill'ine karşı olan mekanik (Warblade → armor + pushback)
- **Faz 2:** Cross-class pasif'in zayıflığını hedef alır
- **Faz 3 (Legendary/Heroic varsa):** Sahaya Legendary skill'inizin yansımasını çıkarır

**Nemesis Bağlantısı:** Eğer daha önce aynı boss'a öldüyseniz (+3 run), boss o kill method'unu hatırlar ve %35 extra resist uygular (Grudge sistemi).

---

## 7. Skill Tier Sistemi

| Tier | Renk | Efekt |
|------|------|-------|
| Common | ⬜ Beyaz | Temel |
| Rare | 🟦 Mavi | +%30 sayı + küçük mekanik |
| Epic | 🟣 Mor | +%60 sayı + anlamlı mekanik |
| Legendary | 🟡 Altın | Skill'in çalışma biçimi değişir |

Offer garantisi: Her offer'da min 1 kart mevcut skill'i tier atlatır.

---

## 8. Demo Class Seti (Faz 1)

| Class | Fantasy | Kaynak | Oda Önceliği |
|-------|---------|--------|-------------|
| **Warblade** | "Yaklaş. Sabitle. Zırh kır. İnfaz et." | Rage (vererek dolar) | Primary |
| **Elementalist** | "Ritim + elementleri yönetiyorum." | Mana + Elemental State | Primary |
| **Shadowblade** | "Görmüyorsun. Zaten geç." | Energy + Combo Points | Primary |
| **Ranger** | "Sana ulaşamazsın." | Focus (mesafe bazlı) | Primary |

---

## 9. Warblade — Final Skill Tablosu

**Rage VEREREK dolar** (hasar alarak dolduran Ravager'dan net ayrım)
**[V] BLADESTORM** — Rage 100: 5s spin, CC immune, her 0.5s AoE

| # | İsim | Tag | Tier | Efekt | Chain → Bonus |
|---|------|-----|------|-------|---------------|
| 1 | **Iron Charge** ★ | ▶⬡ | Core | 8m dash + 1.5s stun, Rage+20 | Stun'daki hedefe → +%80 |
| 2 | **Crippling Blow** | ⚡💥 | Core | Büyük hasar + iyileşme -%50 (6s) | Iron Charge sonrası → -%100 |
| 3 | **Iron Crush** | ✦ | Core | 6s: tüm hasar +%30 | Burst window → katlanır |
| 4 | **Gravity Cleave** | ⬡⚡ | Core | 4m çekme + %140 hasar, 0.8s slow | Iron Charge sonrası → 1.5s stun |
| 5 | **Sunder Mark** | ✦↓ | Core | 8s zırh -%40 | Death Blow aktifken → -%60 |
| 6 | **War Stomp** | ⬡↑ | Core | 3m knockup 2s, Rage+25 | Bladestorm sırasında → +1s |
| 7 | **Ironclad Momentum** | ↑✦ | Core | 6s: alınan hasar %30 yok sayılır + 10 hasar = +10 Rage | War Stomp sonrası → %50'ye çıkar |
| 8 | **Iron Counter** | ↑⚡ | Core | 0.8s pencere: vurulursa %180 karşı + Rage+25 + 0.5s stun | Rage 80+ → 2× tetiklenir |
| 9 | **Blade Rush** | ▶↑ | Advanced | 6m dash + çizgideki herkese %120, Rage+15/hit | 3+ hedef → Rage+50 |
| 10 | **Battle Surge** | ✦↑ | Advanced | 8s: Rage harcaması = HP +%5 | Rage 80+'ta → süre 12s |
| 11 | **Deep Wound** | ⚡↑ | Advanced | Bleed DoT 8s + Rage+20 | Iron Crush window → 2× tick |
| 12 | **Death Blow** | 💥⬡ | Master | HP<%30: %400 hasar, Rage boşaltır | Crippling Blow aktifken → %600 |

**Build Eksenleri:**
- Execution: Iron Charge + Crippling Blow + Iron Crush + Death Blow
- Control Breaker: Gravity Cleave + War Stomp + Sunder Mark + Death Blow
- Last Stand: Ironclad Momentum + Iron Counter + Battle Surge + Death Blow

---

## 10. Hikaye — RIMA

> *"The Fracturing bir felaket değildi yalnızca. Bir tercihti.*
> *Ve sen, o tercihin hem faili hem kalıntısısın."*

### Dünya

Dünyalar arasında yayılan, geri çevrilemez bir tüketim vardı. Onu durdurmak için bağlantılar tek elden, tek anda, geri dönüşsüz koparıldı — **The Fracturing**. Hesap yapıldı, bedel kabul edildi. Ama bedel çok fazla çıktı.

O karar ikiye ayrıldı:
- **Nexus Core** — kararın donmuş iradesi. Değişemiyor, şüphe edemiyor. Seni "kopyalamıyor": senin olabileceğin yüzleri biliyor çünkü aynı kaynaktan geliyor. O durdu. Sen devam ettin.
- **Sen** — kararın bedelini taşıyan, nedenini unutmuş, hâlâ değişebilen parça. Her run biraz daha hatırlıyorsun.

**Kahraman mı? Suçlu mu? Kurban mı?** — Kasıtlı olarak belirsiz.

### Mekanik ↔ Lore Bağı

| Mekanik | Lore Anlamı |
|---------|-------------|
| Tek class ile başlama | Yalnızca bir yüzünü hatırlıyorsun |
| Secondary class açılması | Daha derine inince başka bir yüz geri geliyor |
| Shattered Echoes | Fracturing'de saçılan class yüzlerin — toplayınca bütünleşiyorsun |
| Loop / ölüp dönme | Mühür — ceza değil, zorunluluk |
| Nexus Core'un faz taklidi | Senin kaynaklarını biliyor, ondan geldi |

### Üç Kapanış (Run 10+)

| Kapı | Tone | Bedel |
|------|------|-------|
| **"Kal"** | Melankolik huzur | Loop devam eder — ama artık senin seçimin |
| **"Kır"** | Gerçek risk, gerçek kayıp | Bazı NPC'ler yok olur. Tüketen şeyin gittiğini bilmiyorsun. |
| **"Taşı"** | En saf roguelite sonu | Gerçeği öğrendin, yine de döneceğin kapıya yürüyorsun |

### Twist Açılış Sırası (9 run)

```
Run 1-3:  Ferryman seni "the Bearer" diye çağırır — açıklamadan
          Cartographer haritasında: "Bu yolu bilen biri vardı. Artık bilmiyor."
Run 4-6:  Nexus Core açmadığın skill'leri kullanıyor
          Vrel: "Bu silahı daha önce tutmuş biri vardı. Ellerin aynı şekli taşıyor."
Run 7-8:  Echo fragment → karar anı anısı (elleri titriyor, yüz yok)
          Mourne: "Senin kaybın... farklı."
Run 9:    Ferryman ilk kez oturuyor. Tanıklığını anlatıyor — her şeyi bilmiyor,
          bazı yerlerde kendini düzeltiyor. Oyuncu boşlukları dolduruyor.
Run 10+:  Threshold'da üç kapı belirir.
```

### Act Ortamları

| Act | Ortam | Palet |
|-----|-------|-------|
| Act 1 | Shattered Ruins — enkaz, soğuk | Gri + #7BA7BC |
| Act 2 | Bleeding Wastes — çürüme, bataklık | Koyu mor + #9E4FE0 |
| Act 3 | Core Approach — gerçeklik inceliyor | Siyah + #FFD700 |

### Hub Karakterleri

- **The Ferryman** — Meta-progression. Run 9'da ilk kez oturup tanıklığını anlatır; her şeyi bilmiyor.
- **Vrel** — Craft/upgrade. *"Bu silahı daha önce tutmuş biri vardı."*
- **Sister Mourne** — HP/healing. *"Senin kaybın farklı."*
- **The Cartographer** — Harita upgrade. Haritasında eski bir el yazısı notu.

→ Detay: `LOKALIZASYON_VE_HİKAYE.md` — tam diyaloglar, string key'ler, epilog tetikleyici sistemi

---

## 11. Lokalizasyon Mimarisi

**Kural:** Oyunda görünen her metin bir string key'dir. Asla hardcode yok.

**Key formatı:** `[KATEGORİ].[ALT].[ID]`
Örnek: `SKILL.WARBLADE.IRON_CHARGE.NAME`, `UI.HUD.HP_LABEL`

**Tablolar:** SkillNames, SkillDescriptions, ClassStrings, UIStrings,
DialogueHub, DialogueBoss, StoryFlavor, SystemMessages, EncounterStrings

**Dil sırası:**
1. Önce tüm metinleri EN yaz
2. TR manuel doldur
3. ES için Ollama batch script (LOKALIZASYON_VE_HİKAYE.md'de hazır)

---

## 12. Üretim Sırası

### Faz 0 — Unity Proje İskeleti
```
- Unity 6.3 LTS, URP 2D
- Klasör yapısı kur
- Input System (New Input System)
- Unity Localization Package kurulumu
- ScriptableObject şemaları: SkillData, ClassData, RoomData, EncounterData
- String table temeli (EN + TR)
- Scene yapısı: Bootstrap → Hub → Game
```

### Faz 1 — Combat Prototype
```
- Player movement (top-down, 8-yön)
- Dash sistemi
- Basic attack
- 1 class: Warblade (6 skill)
- Rage sistemi
- Düşman basic AI
- Oda temizleme loop'u
- Reward popup (skill offer, Common tier)
Hedef: Combat hissi çalışıyor mu?
```

### Faz 2 — First Playable Loop
```
- 4 class (Warblade, Elementalist, Shadowblade, Ranger)
- Act 1 (6-7 oda)
- Elite oda
- Act 1 boss (The Iron Warden / Void Warden / Chain Warden — 1 impl)
- Skill draft sistemi
- Shop odası
- Yırtık harita reveal sistemi
- Kısmi harita görünürlüğü
Hedef: Primary class loop oturuyor mu?
```

### Faz 3 — Secondary Class Break
```
- Boss sonrası 2 random secondary class kartı
- +2 aktif slot açılması
- Cross-class passive sistemi
- Mixed draft oranları (60/25/15 → 45/45/10)
- Spirit Encounter (3 tip: Forge Wraith, Shadow Hound, Blood Oracle)
Hedef: "Run burada değişti" hissi geliyor mu?
```

### Faz 4 — Full Demo
```
- Act 2 + Act 2 boss (Fractured King, 2-faz)
- Cross-class Ultimate sistemi
- Epic skill tier
- Event odaları, Curse Gate, Unknown oda
- Void Seer, Fallen Champion spirit tipleri
- Temel meta-progression (Echoes toplanır + harcanır)
Hedef: 30-45 dakikalık run loop
```

### Faz 5 — Early Access
```
- Tüm 8 class
- Act 3 + boss (Hollow Sovereign — adaptive)
- Final Boss (Nexus Core)
- Legendary tier + Heroic/Awakened
- Tüm 28 cross-class kombo
- Nemesis/Grudge sistemi
- Tam meta-progression
```

---

## 13. Görünüm ve His Hedefi

| An | Oyuncu Hissi |
|----|-------------|
| Act 1 başlangıcı | "Bu class ne yapıyor, anlıyorum." |
| Act 1 boss sonrası | "Tamam. Oyun şimdi başka bir şeye dönüştü." |
| Act 2 ortası | "Ben artık build kuruyorum." |
| Geç Act 2 | "Bu skill kombinasyonu... beklemedim." |
| Act 3 | "Bu build insane." |

**Görsel ton:** Karanlık ama okunaklı. Parçalanmış diyar hissi.
Yırtık harita / eski mürekkep estetiği. Güçlü class siluetleri.

---

## 14. Class Unlock Sistemi

**Lore temel:** Her class = Fracturing'de saçılan bir "yüz." Unlock = o yüzü geri kazanmak.
Ferryman unlock menüsü: *"Bir şey tanıdık geliyor. Bir parçan geri dönüyor."*
Kilitli classlar UI'da kırık siluet + sis olarak görünür. Açılınca: 1 cümle anı + class fantasy özeti.

### Unlock Tipleri

| Sembol | Tür | Açıklama |
|--------|-----|----------|
| 💀 | Echo Unlock | Shattered Echoes ile direkt satın al |
| ⚔️ | Koşul Unlock | Narratif bağlı başarı — resource gerekmez |
| 🔗 | Karma | İkisi birden gerekli |

### Unlock Tablosu

| Class | Tip | Koşul | Ferryman Satırı |
|-------|-----|-------|-----------------|
| **Warblade** | — | Başlangıçta açık | "İlk hatırladığın yüz bu." |
| **Elementalist** | 💀 | 80 Echo | "Bu parça hep yakındaydı. Şimdi adını buldun." |
| **Ranger** | 💀 | 80 Echo | "Mesafe hem silah hem alışkanlık olmuş." |
| **Shadowblade** | 💀⚔️ | 150 Echo VEYA Act 1'i 3 kez tamamla | "Karanlıkta saklanan yüz en son çıkar." |
| **Ravager** | 💀⚔️ | 150 Echo VEYA Warblade ile Act 2 boss öldür | "Savaşın özüne inince bu yüz açılır." |
| **Summoner** | 💀⚔️ | 200 Echo VEYA 3 üst üste run'da Act 2'ye ulaş | "Bağ hafızadan önce gelir." |
| **Hexer** | 🔗 | 200 Echo + Elementalist ile run bitir | "Yasak bilgi başka bilgiden büyür." |
| **Brawler** | 🔗 | 350 Echo + herhangi class ile Act 3'e ulaş | "En başta bıraktığın. En son dönen." |

### Notlar
- "VEYA" koşullar: iki yol da geçerli — resource grind veya oynanış başarısı
- Hexer + Brawler için her iki koşul da zorunlu (en sert yüzler en zor kazanılır)
- Unlock animasyonu: siluetteki sis dağılır → kısa hafıza yankısı satırı → class kartı belirir
- Meta-progression ekranında unlocked classların lore satırı kalıcı görünür

---

## 15. Dosya Referans Haritası

| Dosya | İçerik | Durum |
|-------|--------|-------|
| `oyun_tasarim_sentez_master_plan.md` | **Bu dosya — ana referans** | ✅ Güncel |
| `GELISTIRME_PLANI.md` | Faz faz Unity geliştirme — Claude/Antigravity/MCP rolleri | ✅ Güncel |
| `OYUN_YAPI_VE_ROADMAP.md` | Oda sistemi, run akışı detayları, faz roadmap | ✅ Güncel |
| `LOKALIZASYON_VE_HİKAYE.md` | Hikaye, epilog, NPC diyalog setleri, boss satırları, 5 event, skill fısıltıları, string key | ✅ Güncel |
| `SINIF_VE_SKILL_KARAR_BELGESI.md` | Tüm 8 class × 12 skill final tabloları | ✅ Warblade güncellendi |
| `BOSS_SISTEMI.md` | 4 boss tam tasarımı, adaptive counter kodu, ödül tablosu | ✅ Güncel |
| `GORSEL_YONERGE.md` | URP 2D lighting, post-processing, sprite boyutu, shader rehberi | ✅ Güncel |
| `MOB_TASARIMI.md` | 12 mob — lore, davranış, PixelLab prompt, animasyon, act dağılımı | ✅ Güncel |
| `ART/RIMA_LOGO_VE_TEMA.md` | Logo Aseprite adımları, görsel kimlik, akt temaları | ✅ Güncel |
| `ART/URETIM_PLANI.md` | Faz faz sanat üretimi — asset/boyut/prompt/parametre | ✅ Güncel |
| `SANAT_PROMPTLARI.md` | Renk paletleri, SD promptları (konsept aşaması) | ✅ Referans |
| `MASTER_SINIF_VE_CROSSCLASS.md` | 28 cross-class kombo listesi | ⚠️ Güncelleme bekler |
| `GDD.md` | Tam GDD — 19 bölüm, tüm 8 class, cross-class, boss, hikaye | ✅ Güncel |
| `arsiv/` | Aşılmış/birleştirilmiş eski dosyalar | 📦 Dokunma |
| `arastirma/` | Ham araştırma notları (ChatGPT/Gemini çıktıları) | 📦 Ham kaynak |
