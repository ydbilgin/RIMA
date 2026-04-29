# RIMA — Legendary & Mythic Tier Skill Tasarımı
*Onay: 2026-04-12 | Gemini öneri + Claude revize + final karar*

---

## TİER SİSTEMİ (FİNAL)

```
Common (55%) → Rare (27%) → Epic (12%) → Mythic (5%) → Legendary (3%)
```

| Tier | Ağırlık | Depth Lock | Tanım |
|------|---------|------------|-------|
| Common | 55% | Oda 1+ | Temel güçlenme |
| Rare | 27% | Oda 1+ | Kaynak/mekanik amplifikasyon |
| Epic | 12% | Oda 3+ | Cross-class sinerji + güçlü duo mekanikler |
| **Mythic** | 5% | Oda 5+ | Primary-only mastery — yalnızca o class'ı primary seçmişsen havuza girer |
| **Legendary** | 3% | Oda 7+ | Run-definer — oyunun temel kuralını kırar |

**Neden "Heroic" değil "Mythic":**
Heroic = gaming kültüründe difficulty mode terimi (WoW Heroic dungeon).
Mythic = rarity/power tier, Epic'ten üstte sezgisel, RIMA dünyasıyla uyumlu.

---

## LEGENDARY TİER — TASARIM KURALI

Legendary skill = **mekanik devrim**, salt hasar artışı değil.

Kriter:
1. Oyunun bir core kuralını oyuncu lehine kırar (kaynak yönetimi, zaman, can, cooldown)
2. O class'ı tanımlar — başka bir class'ın elinde anlamsız olur
3. "Bu run'ı bu skill kazandırdı" denebilmeli
4. **Birden fazla alınamaz** (unique per run) — eğer havuzdaysa draft'ta bir kez sunar

---

## 10 CLASS LEGENDARY SKİLLER

---

### 🛡️ WARBLADE — "Shattered Rift Slam"
**Koşul:** Oda 7+, Warblade primary
**Efekt:** Rage %100 olduğunda her 3-hit combo'nun son vuruşu 5m şok dalgası yaratır ve vurduğu her hedefe Sunder Mark uygular.
**Kural kıran:** Sunder Mark'ı manuel kullanma zorunluluğunu kaldırır. Rage 100'de otomatik alan zırhı kırma = mass execute setup.

---

### ❄️ ELEMENTALİST — "Confluence" *(Gemini'nin "Elemental Singularity"'sinden revize)*
**Koşul:** Oda 7+, Elementalist primary
**Efekt:** 10s boyunca Fire ve Frost State aynı anda aktif kalır; Rift Bolt her vuruşta hem yakar hem dondurur. 10s sonra state sıfırlanır, CD 30s. Element switch mekanizması hâlâ çalışır.
**Kural kıran:** İki element aynı anda = hem DoT hem slow hem Elemental State birikimi — ama window sınırlı.
**Revize gerekçesi:** Element switch tamamen kalkmaz — Elementalist'in core ritmi element geçişinden güç alır. Window ile sınırlı confluence daha sağlıklı.

---

### 🗡️ SHADOWBLADE — "Phantom Protocol"
**Koşul:** Oda 7+, Shadowblade primary
**Efekt:** Stealth halindeyken hareket hızı +%100, geçilen düşmanlara otomatik 5 CP'lik Rupture uygulanır.
**Kural kıran:** Kaynak harcamadan en güçlü finisher setup'ı pasif hale gelir. Stealth artık sadece kaçış değil, hasar döngüsünün kendisi.

---

### 🏹 RANGER — "Infinite Quiver"
**Koşul:** Oda 7+, Ranger primary
**Efekt:** Focus 75+ olduğunda tüm oklar Ricochet + Pierce kazanır. Duvara çarpan oklar mini patlayıcı tuzaklara dönüşür (1s sonra tetiklenir).
**Kural kıran:** Tekli hasar odaklı okçuyu bullet-hell temizlik makinesine çevirir.

---

### ⚔️ RONİN — "Mugen no Kiri" *(Gemini'nin orijinalinden revize)*
**Koşul:** Oda 7+, Ronin primary
**Efekt:** Tension 100 olduğunda 10s boyunca alınan hasarın %150'si saldırgan hedefe yansır. Ronin hâlâ hasar alır.
**Kural kıran:** Savunmayı saldırıya çevirir — kalabalık odada Ronin'e saldırmak intihar olur.
**Revize gerekçesi:** Orijinalde iframes + yansıtma = tam ölümsüzlük, counterplay sıfır. Yansıtma tek başına yeterince güçlü ve tematik.

---

### 🔫 GUNSLINGER — "Heat Singularity"
**Koşul:** Oda 7+, Gunslinger primary
**Efekt:** Overheat süresi bittiğinde silahlar kilitlenmez; bunun yerine karakterden çevreye 5s boyunca saniyede otomatik mermi saçılır (8 yön, yüksek hasar).
**Kural kıran:** Overheat cezasını silaha dönüştürür. "Kaçınılmaz aşırı ısınma" artık oyuncunun seçimi haline gelir.

---

### 🪓 RAVAGER — "Undying King"
**Koşul:** Oda 7+, Ravager primary
**Efekt:** Berserk Mode aktifken can %1'in altına inemez. Kill-gate CD sıfırlama her 3 saniyede bir otomatik tetiklenir (kill gerekmez).
**Kural kıran:** "Kill yaparsan hayatta kalırsın" riskini "Berserk'te ölmezsin" garantisine çevirir — ama Berserk dışında koruma yok.

---

### 🥊 BRAWLER — "Echoing Impact"
**Koşul:** Oda 7+, Brawler primary
**Efekt:** Her başarılı Weave (dodge) sonrası, son kullanılan skill 3s boyunca 4 kez daha gölge kopyalar tarafından tekrar edilir.
**Kural kıran:** Tekil güçlü vuruşu 5 vuruşa çevirir. Weave'i savunma değil combo çarpanına dönüştürür.

---

### 💀 SUMMONER — "Rift Apocalypse"
**Koşul:** Oda 7+, Summoner primary
**Efekt:** Çağrılan tüm minyonlar öldüğünde patlamak yerine o noktada 3s mini Lich Form alanı yaratır — bu alan içinde Summoner fiziksel hasara bağışık olur.
**Kural kıran:** Minyon feda etme mekaniğini Summoner'ın hayatta kalma garantisine dönüştürür. "Minyonlarım öldü, ben de ölüyorum" → "Minyonlarım kalkan oldu."

---

### 🔮 HEXER — "Cursed Singularity"
**Koşul:** Oda 7+, Hexer primary
**Efekt:** Bir düşman 10 stack Hex'e ulaşınca Hexblast patlaması odadaki TÜM düşmanlara aktarılır (stack sayısına bakılmaksızın, tam hasar).
**Kural kıran:** Sabır gerektiren tekli hedef eritme sistemini zincirleme oda temizleme tuşuna çevirir. Tek Hexblast, tüm oda.

---

## MYTHİC TİER — TASARIM KURALI

Mythic skill = **class kimliğinin zirvesi.**

Kriter:
1. Sadece o class primary seçilmişse havuza girer — secondary olarak çıkmaz
2. "Bu class bu olabilir" dedirten skill — başka class'ın elinde anlamsız
3. Core mekanizmayı parlatır, değiştirmez (Legendary'den farkı bu)
4. Depth lock Oda 5+

### Örnek Mythic Skiller (taslak)

| Class | Mythic Skill | Efekt Özeti |
|-------|-------------|-------------|
| Warblade | **Blood Throne** | Rage 100'de ölmek yerine 1 HP'de donarsın, 5s Rage yakarken her tick heal — süre bitince tam HP |
| Elementalist | **Rift Core** | Elemental State 5'e ulaşınca bir sonraki spell anında cast, 0 mana, garantili crit — State sıfırlanmaz |
| Shadowblade | **Voidwalk** | 8s boyunca tüm saldırılar stealth'ten çıkmaz — sürekli stealth combat window |
| Ranger | **Dead Zone** | Focus 100'de bir sonraki Aimed Shot anında ateşlenir, 2× menzil, pierce, guaranteed crit |
| Ronin | **Draw Horizon** | Tension 50+ iken Draw Tension serbest bırakılınca Quickdraw CD sıfırlanır, zincir katana mümkün |
| Gunslinger | **Overclocked** | Heat 80+ iken tüm saldırılar +%40 hasar ve ateş hızı — Overheat riski yüksek tutmanın ödülü |
| Ravager | **Undying Rage** | Kill başına Berserk süresi +2s uzar — oda temizlenene kadar Berserk bitmez |
| Brawler | **Perfected Form** | Charge 5+ iken LMB'nin her 5. vuruşu otomatik Weave tetikler — Echoing Impact ile sinerjik |
| Summoner | **Dark Covenant** | Minyon başına max HP +20; 10 minyon öldürünce bir boss-tier minyon çağırılır |
| Hexer | **Hex Mastery** | Hex stack kazanımı ×2, Hexblast CD -%40 — Hexer'in tüm döngüsü hız kazanır |

*Mythic skiller tam tasarım gerektiriyor — bu taslak Faz 2 öncesinde detaylandırılacak.*

---

## UYGULAMA NOTLARI

**SkillOfferGenerator.cs'e eklenecek depth lock:**
```csharp
// Mevcut:
if (skill.tier == SkillTier.Epic || skill.tier == SkillTier.Legendary)
    if (currentRoom < 3) continue;

// Yeni:
if (skill.tier == SkillTier.Epic)    if (currentRoom < 3) continue;
if (skill.tier == SkillTier.Mythic)  if (currentRoom < 5) continue;
if (skill.tier == SkillTier.Legendary) if (currentRoom < 7) continue;

// Mythic primary-only check:
if (skill.tier == SkillTier.Mythic)
    if (playerClassManager.PrimaryClass != skill.RequiredClass) continue;
```

**SkillTier enum'a eklenecek:**
```csharp
public enum SkillTier { Common, Rare, Epic, Mythic, Legendary }
```

---

*Referans: Gemini öneri (2026-04-12) + ChatGPT onay + Claude final revize*
*Bağlantılı: PROGRESSION_SCHEMA_FINAL.md, CROSS_CLASS_SKILL_MATRIX.md, SkillDatabase.cs*
