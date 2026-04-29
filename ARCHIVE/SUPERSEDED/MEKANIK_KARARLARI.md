# RIMA — Mekanik Kararlar
*2026-04-03 | Araştırma + Gemini + ChatGPT + Claude analizi sentezi*
*GDD'nin "ruhu" ayrı kalır. Bu dosya "motorun parçaları."*

---

## KARAR TABLOSU

| Mekanik | Durum | Faz |
|---|---|---|
| İki para birimi (Echoes + Shards) | ✅ ALINDI | 1 |
| 3 sandık tipi | ✅ ALINDI | 1 |
| In-run shop + meta shop | ✅ ALINDI | 1 |
| 4 elite affix | ✅ ALINDI | 1 |
| Mob unique mekanikleri | ✅ ALINDI | 1 |
| Hub NPC derinleştirme (Ferryman/Vrel/Mourne) | ✅ ALINDI | 1 |
| Codex + item description lore | ✅ ALINDI | 1 |
| Resource bar form farklılaştırması | ✅ ALINDI | 1 |
| HUD 6 slot mimarisi | ✅ ALINDI | 1 |
| 1 reroll (draft ekranı) | ✅ ALINDI | 1 |
| Basit ölüm ekranı | ✅ ALINDI | 1 |
| Component-based mob mimarisi | ✅ ALINDI | 1 |
| Spirit Encounter derinleştirme | ⚠️ KOŞULLU | 2 |
| Curse sistemi (5 basit efekt) | ⚠️ KOŞULLU | 2 |
| Yarı-gizli sinerjiler | ⚠️ KOŞULLU | 2 |
| TwiceBorn (Curse Gate event boss) | ⚠️ KOŞULLU | 2 |
| Named elite + grudge bağlantısı | ⚠️ KOŞULLU | 2 |
| Rift Gambit / gambling | ❌ ERTELENDI | 3+ |
| Detaylı ölüm analitiği | ❌ ERTELENDI | 3+ |
| Co-op | ❌ KALICI HAYIR | — |

---

## BÖLÜM 1 — EKONOMİ

### Para Birimleri

**Echoes** (meta currency) — GDD'de zaten var, değişmiyor.
- Run sonunda Hub'a döner, birikmez kaybolmaz
- Ne için: class unlock, hub NPC unlock, codex açılımı, room varyantı eklemesi, küçük QoL

**Shards** (in-run currency) — yeni, in-run'a özel
- Run içinde düşmandan ve odadan düşer
- Ne için: in-run shop alışverişi, reroll, Curse Gate maliyeti, sandık kilidi
- Run bitince sıfırlanır

**Neden iki ayrı:** Oyuncu run içinde Shards harcayıp ekonomik baskı hisseder. Echoes'a dokunmadığı için "bu run'da her şeyi harcayayım" kararı alabilir. Risk-reward doğru çalışır.

**Meta shop sınırı:** Stat verme. Sadece içerik açılımı. "Daha güçlü başla" değil "daha fazla seçenek gör." Aksi hâlde oyun idle markete döner.

### Sandık Sistemi

**3 tip, fazlası yok:**

| Sandık | İçerik | Özellik |
|---|---|---|
| Common | Shards, küçük augment | Garantili, sık |
| Rare | Güçlü item / skill tier atlatma | Görece seyrek |
| Rift Chest | Build-shaping, bazen riskli ödül | Gerçekten farklı — sadece "daha iyi" değil |

**Rift Chest felsefesi:** Sıradan loot tablosundan çıkmayan şeyler içerir. Örnek: o run'un secondary class'ından bir skill erken gelmesi, nadir bir pasif, ya da "Cursed Offer" (güçlü ama curse ekler). Sıradan değil, build'i şekillendirir.

### Rift Gambit — ERTELENDI

**Neden:** Gambling sistemi ancak ekonomi dengesi oturmuşsa anlamlı olur. Core loop stabil olmadan bu "cool mekanik" değil, dikkat dağıtan gürültü olur. Faz 3'e not düşüldü.

---

## BÖLÜM 2 — BUILD ÇEŞİTLİLİĞİ

### Spirit Encounter Derinleştirme (mevcut sistemi üste katman değil)

GDD'de Spirit Encounter zaten var — koşullu build bonusu veren olay odası. Ayrı bir "boon sistemi" eklenmeyecek. Bunun yerine mevcut sistem derinleştirilecek:

- Hızlı oda temizlendiyse farklı Spirit offer
- Hiç hasar alınmadıysa farklı offer
- Curse taşıyarak girilmişse farklı bonus

Kararlar KOŞULLU — core loop stabil olunca.

**İsim:** "Boon" kullanılmıyor. Spirit Blessing veya Echo Mark — RIMA'ya özgü kalacak.

### Elite Affix Sistemi

**4 affix, Faz 1'de yeterli:**

| Affix | Efekt | Act |
|---|---|---|
| **Blazing** | Saldırılar alevlenir, yakında duran oyuncuya DoT | Act 1-2 |
| **Glacial** | Saldırılar slow uygular, yeterli hit sonrası freeze | Act 1-2 |
| **Void-Touched** | Gecikmeli zone bırakır, teleport davranışı | Act 2-3 |
| **Fractured** | Ölünce shard scatter veya echo spawn | Act 2-3 |

Her normal mob + 1 affix = Elite. Görsel olarak renk shift alır (Blazing → turuncu aura, Glacial → mavi kristal).

Named elite (The Glacial Gaoler, Fractured Penitent gibi) → Faz 2, grudge sistemi hazır olunca.

### Reroll

- Run başına 1 ücretsiz reroll
- Ek reroll: in-run Shards ile, fiyat her kullanımda artar
- Spam'e dönmez, oyuncu kararı önemli kalır

### Yarı-Gizli Sinerjiler — KOŞULLU

Tam gizli değil. Model:
1. İlk kez sinerji tetiklenince küçük görsel işaret + ses
2. Codex'e otomatik not düşer
3. Sonraki run'larda oyuncu takip edebilir

"Ben nereden bilecektim?" soru sorulmaz. Keşif var ama kör değil.

---

## BÖLÜM 3 — MOB MİMARİSİ (KRİTİK TEKNİK KARAR)

### Component-Based Mob Sistemi

**Karar:** Her mob yeteneği ayrı MonoBehaviour component. Statik script'e gömülü değil.

**Neden zorunlu:**
- Elite affix sistemi runtime'da component eklemesi gerektirir
- Blazing ShardWalker = ShardWalker script + MobAffix_Blazing component — bunu tek scriptte yapamazsın
- Named elite / grudge sistemi Faz 2'de gelince aynı mimariyi kullanacak
- Bug fix = 1 yerde düzelt, heryerde geçerli

**Mimari:**

```
BaseMobBehavior.cs          ← hareket, sağlık, ölüm, temel state machine
├── MobAttack_Throw.cs      ← fırlatma (ShardWalker, VoidThrall)
├── MobAttack_Melee.cs      ← yakın dövüş (Penitent, FractureImp)
├── MobAttack_ChainPull.cs  ← zincir çekme (ChainWarden)
├── MobAttack_Barrier.cs    ← bariyer oluşturma (RelicCaster)
├── MobAttack_Summon.cs     ← çağırma (VoidThrall elite)
│
├── MobAffix_Blazing.cs     ← tüm saldırılara ateş DoT ekler
├── MobAffix_Glacial.cs     ← tüm saldırılara slow/freeze ekler
├── MobAffix_VoidTouched.cs ← gecikme zone + teleport davranışı
└── MobAffix_Fractured.cs   ← ölüm davranışı override
```

**EnemyTier.cs** (zaten var) → spawn anında affix component ekler:
```csharp
// Elite spawn
if (isElite) {
    var affix = Instantiate(affixPrefab);
    mob.GetComponent<BaseMobBehavior>().ApplyAffix(affix);
}
```

**Kural:** Her mob'a 1 dominant mekanik. İkincil katman → Elite affix'i getirir.

### Mob Unique Mekanikler — Kesin Kararlar

| Mob | Dominant Mekanik | Elite Varyant Farkı | Karar |
|---|---|---|---|
| **ShardWalker** | Yere düşen shard → AoE patlama | Glacial affix → shard freeze zone bırakır | ✅ ALINDI |
| **VoidThrall** | Ranged void projectile | Elite → ölünce Voidling spawn (normal varyantta YOK) | ✅ ALINDI (Elite only) |
| **SeamCrawler** | Zemine hasar izi bırakan trail | Elite → homing projectile (ikisi aynı anda değil) | ✅ ALINDI (trail base, homing elite) |
| **ChainWarden** | Oyuncuyu çekerek yakına alır | Void-Touched → çektikten sonra delayed zone | ✅ ALINDI |
| **Penitent** | 3-hit combo, son vuruş armor break | Blazing → combo ateşlenir, overhead miss → kısa stun | ✅ ALINDI + kimlik güçlendi |
| **RelicCaster** | Bariyer/duvar oluşturur | Fractured → ölünce bariyer shard'a dönüşür | ✅ ALINDI |
| **FractureImp** | Melee hit → shard scatter ikincil hasar | Blazing → shard'lar ateş bırakır | ✅ ALINDI |

**VoidThrall notu:** On-death spawn zinciri normal varyanta yüklenmez. Sürümde yorucu olur. Elite'e özel.
**SeamCrawler notu:** Homing + trail aynı anda "nereye kaçacağım" durumu yaratır. Base = trail, Elite = homing. Ayrı varyantlar.

---

## BÖLÜM 4 — BOSS

### TwiceBorn Konumlandırması

GDD'deki Act boss sırası korunur (IronWarden, Fractured King, vs.). TwiceBorn:
- Act 1 main boss değil
- Act 2 Curse Gate event boss'u VEYA Act 2 mid-boss
- İki fazlı yapısı korunur, arena collapse korunur

**Neden kaydırıldı:** Act 1 boss kimliğini çalmaz. Ayrıca "yeniden doğan" konsepti Faz 2-3 tonuna daha uygun.

### Boss Drop Felsefesi

**Diablo item drop yok.** RIMA bir skill draft oyunu, direkt "TwiceBorn's Fang" gibi stat item vermez.

Boss ölünce düşen şeyler:
- **Bol Echoes** (meta progression)
- **Bol Shards** (in-run shop için)
- **Boss Fragment** → Hub'da Vrel'e getir → build tag'ine göre farklı upgrade'e dönüşür
  - Warblade ağırlıklı build → Fragment → Rage augment
  - Elementalist ağırlıklı → Fragment → cast speed / element tier
  - Bu sistem oyunun kimliğiyle uyumlu, Diablo değil roguelite

---

## BÖLÜM 5 — HUB VE ANLATI

### NPC'ler — Mevcut İsimler Korunuyor

GDD'deki isimler daha karakterli. Jenerik isimler kullanılmıyor:

| NPC | İşlev | Yeni Mekanik Bağlantısı |
|---|---|---|
| **Ferryman** | Lore, run tanıklığı | Her run sonrası 1 yorum — boss'a göre değişir |
| **Vrel** | Craft / upgrade | Boss Fragment işleme, augment craft |
| **Sister Mourne** | Healing / HP | Curse sistemi kurbanlarını bilir — curse aktifse özel diyalog |

### Codex + Item Description

- Her item ve skill'in 1-2 cümle lore metni olur
- İlk kez sinerji tetiklenince codex'e not düşer
- Nadir item açıklamaları rift dünyasını bölük pörçük anlatır
- Oyuncu okumak zorunda değil — ama okursa dünya genişler

---

## BÖLÜM 6 — HUD

### 6 Slot Mimarisi (Faz 1'den itibaren)

Act 1'de 4 slot aktif, 2 slot kilitli görünür (soluk/gri). Act 1 boss öldürülünce kilit açılır. HUD tasarımı baştan 6 slot düşünülerek yapılır — Act 1 sonrası yeniden yazılmaz.

```
[Q] [E] [R] [F]  ←  Act 1 aktif (4)
              [1] [2]  ←  Act 1 kilitli, Act 1 boss sonrası açılır
```

### Resource Bar Form Farklılaştırması

Sadece renk değil, form da değişir:

| Class | Resource | Form |
|---|---|---|
| Warblade | Rage | Çatlak, sert, agresif — kenarlarda kırmızı enerji |
| Elementalist | Mana | Akışkan, dalga hareketli — sakin dolup boşalan |
| Shadowblade | Energy + Combo | Segmentli hızlı bar + 5 ayrı CP noktası |
| Ranger | Focus | Menzile göre dolan ince halka — uzaktan dolu, yakından boşalır |

### Ölüm Ekranı — Minimal İlk Versiyon

3 şey yeterli:
1. Seni öldüren düşman (sprite + isim)
2. Hangi odada öldün (act + oda tipi)
3. Kısa run recap: kaç düşman, hangi skill'ler aktifti

Death heatmap, kaynak analizi, yakın düşman listesi → Faz 3. Core loop stabil olmadan bunlar anlamsız.

---

## BÖLÜM 7 — CO-OP

**Hayır. Bu dosyada başka yer yok.**

Netcode + latency + 8 class resource senkronizasyonu = ayrı oyun. RIMA'nın gücü solo power fantasy. Odak dağıtılmaz.

---

## MODÜLERLİK KURALI (Tüm Sistemler İçin)

Her yeni mekanik şu soruyu geçmeli:
> "Bu, mevcut bir sistemi derinleştiriyor mu — yoksa yeni bir katman mı ekliyor?"

Derinleştirme → yap.
Yeni katman → beklet, Faz 2'ye ertele.

Örnek:
- Spirit Encounter'a koşul eklemek → derinleştirme ✅
- Spirit Encounter'ın yanına ayrı bir Boon sistemi kurmak → yeni katman ❌
