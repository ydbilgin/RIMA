# Boss Sistemi Tasarımı
*Son güncelleme: 2026-03-29*

---

## GENEL FELSEFE

Hades: Boss'lar **narrative-first** → tanrısal karakterler, diyaloglar, mythological bağlar.
Bu oyun: Boss'lar **mechanic-first** → her boss, build'inin bir zayıflığını test eder.

> Her boss bir soru sorar: "Bu senaryoda build'in çalışıyor mu?"

**Paylaşılan özellikler (tüm boss'lar):**
- 2 faz min (HP eşiği ile değil, koşul/tetikleyici ile)
- Arenaya özgü bir mekanik (zemin, alan, çevre)
- Öldürünce run-içi ödül + meta-progress kaynağı
- Nemesis bağlantısı: aynı boss'a 3+ kez öldüysen %35 ek resist

---

## ACT 1 — ROTATING GUARDIANS (3 Varyant)

Her run bu üçünden biri gelir. Hangisinin geleceği önceden gösterilmez.
Aynı zorluk, farklı mekanik — meta-solve önlenir.

---

### 🛡️ THE IRON WARDEN

**Mekanik odak:** Savunma kırma, hasar patlama yönetimi

**Tasarım:**
- HP'nin %80, %50, %20'sinde **Iron Shell** aktif olur (3-5s tam hasar bağışıklığı)
- Shell aktifken sadece **zırh kıran** efektler (Sunder Mark, zırh debuff) hasar verir
- Shell kırılınca: 2s stagger penceresi → tüm hasar +%50 (execution window)
- Normal saldırıları: yavaş ama geniş alan sweep'ler

**Oyuncu sorusu:** "Hasar patlamam var ama zamanında ateşleyebiliyor muyum?"

**Faz 2 tetikleyicisi:** HP %50'nin altına düştüğünde değil — oyuncu Shell'i 3 kez kırdığında aktive olur.
- Faz 2: Shell süresi uzar (5→8s), aralarına ek sweep pattern eklenir
- Ama Shell kırıldığında execution window da uzar (2→4s) — ödül dengeli

**Arena:** Geniş, az engel. Iron Warden için açık alan avantajlı (sweep'ler daha tehlikeli).

**Ödül:** Shattered Echoes (meta-currency) + Common/Rare skill offer + secondary class seçim ekranı

---

### 🌀 THE VOID WARDEN

**Mekanik odak:** Pozisyon yönetimi, alan okuma

**Tasarım:**
- Her 8-10 saniyede bir **Void Step**: arena boyunca kaybolup yeniden belirir
- Kaybolmadan önce 3 adet **Void Zone** bırakır (3s boyunca hasar veren kırmızı alan)
- Void Zone'ların **aralarından** geçmek bonus: **Convergence Burst** → sonraki saldırı +%30 hasar
- Normal saldırıları: orta hızda, yönlü

**Oyuncu sorusu:** "Hareket ederken doğru pozisyonu koruyabiliyor muyum?"

**Faz 2 tetikleyicisi:** Oyuncu 3 kez Convergence Burst bonus aldıysa (yani aktif olarak zone'lar arası geçiyorsa) → Warden faz değiştirir.
- Faz 2: 5 Void Zone (3 yerine), Warden daha hızlı, ama Convergence Burst +%60'a çıkar

**Arena:** Orta büyüklük, kenarları Void Zone spawn hatası olarak işaretli.

---

### ⛓️ THE CHAIN WARDEN

**Mekanik odak:** Hareket kısıtlama, resource yönetimi

**Tasarım:**
- Her 6s'de **Chain Throw**: oyuncuya zincir fırlatır, 2s hareketsizlik
- Hareketsizken: Rage/Momentum/Focus/Fury gibi hareket-bağlı resource'lar decay hızlanır (%3×)
- **Zinciri kırmak için:** Chain düştükten 0.5s içinde herhangi bir skill kullan → zincir kırılır, +resource
- Kırmadan beklersen: 2s hasar zinciri, resource kaybı

**Oyuncu sorusu:** "Kısıtlandığımda panikliyor muyum yoksa resource kazanıyor muyum?"

**Faz 2 tetikleyicisi:** Oyuncu zinciri 4 kez kırdığında (yani sistemi öğrendiyse).
- Faz 2: Çift zincir (2 zincir aynı anda), ama kırınca double resource bonus

**Arena:** Dar, duvarlı. Zincir kırılmayı daha önemli yapar.

---

## ACT 2 — THE FRACTURED KING

**Tasarım konsepti:** Kırık dünyaların birleşimi. Hem aşina hem yabancı hissettiren boss.

---

### FAZ 1 — The Construct

**Normal faz:**
- Büyük humanoid, kırık dünya parçalarından oluşmuş zırh
- 3 hareket: Sweeping Slash (%120 hasar, geniş), Ground Fissure (zemin çatlağı, yavaş), Fragment Hurl (uzak mesafe fırlatma)
- Normal HP eşiği yok

**FAZ 2 TETİKLEYİCİSİ — Aksiyon bazlı:**
> HP %50'ye inmesi değil:
> Oyuncu 10 saniye içinde 4+ farklı skill kullandıysa faz 2 başlar.

**Neden bu tetikleyici?**
- Tek skill spam yapan oyuncuyu cezalandırmaz ama çeşitliliği ödüllendirir
- Oyuncunun ne zaman faz değiştireceği kendine bağlı — control hissi
- Roguelite build çeşitliliğini teşvik eder

**Geçiş animasyonu:** Construct dağılır, parçalar havada döner, yeniden birleşir.

---

### FAZ 2 — The Shattered Form

**Mekanik değişimi:**
- Bedeni 3 bağımsız "shard" olarak parçalanır, her biri farklı saldırı yapar
- Shard'lardan birini öldürmek diğerlerini geçici olarak %20 daha güçlü yapar
- En son shard → tüm powerları toplar, HP bar görünür, executionable

**Strateji:** Hepsini aynı anda indirmeye çalış veya en güçlüyü önce sek.

**Arena:** Büyük, açık. Birden fazla şeyi takip etmek için alan gerekli.

**Ödül:** Cross-class Ultimate kilidi + Rare/Epic skill offer + Echoes

---

## ACT 3 — THE HOLLOW SOVEREIGN (Adaptive Counter)

Bu boss, oyuncunun equipped skill tag'lerini analiz eder ve counter davranışı seçer.

---

### Nasıl Çalışır — Teknik Detay

**Adım 1 — Tag Sayımı (Boss spawn anında çalışır)**

```csharp
void AnalyzePlayerBuild()
{
    var skills = Player.Instance.EquippedSkills; // List<SkillData>

    int finisherCount  = skills.Count(s => s.HasTag(SkillTag.FINISHER));
    int controlCount   = skills.Count(s => s.HasTag(SkillTag.CONTROL));
    int builderCount   = skills.Count(s => s.HasTag(SkillTag.BUILDER));
    int aoeCount       = skills.Count(s => s.isAreaEffect);
    int hpSpendCount   = skills.Count(s => s.costsHP);

    // Dominant strategy tespiti
    DominantStyle = DetermineStyle(finisherCount, controlCount, builderCount, aoeCount);
}

DominantStyle DetermineStyle(int fin, int ctrl, int build, int aoe)
{
    if (aoe >= 2)      return DominantStyle.AOE;
    if (fin >= 2)      return DominantStyle.BurstFinisher;
    if (ctrl >= 2)     return DominantStyle.ControlHeavy;
    if (build >= 3)    return DominantStyle.ResourceBuilder;
    return             DominantStyle.Balanced;
}
```

**Adım 2 — Counter Davranışı Seçimi**

```csharp
void SetBehaviorModifiers()
{
    switch (DominantStyle)
    {
        case DominantStyle.AOE:
            EnableArmorShell();        // Periyodik hasar azaltma
            SpawnScatteredMinions();   // Gruplaşmayı önler, AoE verimini düşürür
            break;

        case DominantStyle.BurstFinisher:
            EnableRegenPhase();        // HP eşiğinde 3s hızlı regen
            AddInterruptAttack();      // Execution window sırasında interrupt
            break;

        case DominantStyle.ControlHeavy:
            EnableCCImmunity();        // %50 CC direnci
            SpawnFastSmallEnemies();   // CC'yi zorlaştıran küçük hedefler
            break;

        case DominantStyle.ResourceBuilder:
            AddFrequentInterrupts();   // Resource buildup'ı bozar
            EnableDrainAura();         // Yakın durulunca resource azalır
            break;

        case DominantStyle.Balanced:
            // Default davranış, extra mod yok
            break;
    }
}
```

**Adım 3 — Oyuncuya Görünürlük**

- Boss spawn anında kısa bir "scan" animasyonu oynar (Sovereign sizi inceler)
- HUD'da küçük bir ikon belirir: "Sovereign is watching..." (oyuncu ne beklediğini bilir ama detayı değil)
- Bu şeffaflık "oyun beni trollüyor" hissini önler

---

### Sovereign'ın Normal Saldırı Seti

- **Void Slash** — önüne geniş yay sweep
- **Sovereign's Grasp** — bir alanı seçer, 1.5s sonra patlama (önceden gösterilir)
- **Essence Drain** — 3s kanalı, bu sürede oyuncu resource gain yok
- **Phase Shift** — pozisyon değiştirir, 0.5s i-frame

**Faz 2 tetikleyicisi:** HP %40'ın altına inmesi (bu boss için basit HP eşiği yeterli, adaptasyon zaten mekanik fark yaratıyor)
- Faz 2: Counter modifierleri intensify olur. AOE counter alıyorsa armor shell daha sık.

**Arena:** Orta büyüklük, kenarlarda periyodik void zone'lar.

**Ödül:** Epic/Legendary skill offer + büyük Echoes + Act 3'e geçiş

---

## FINAL BOSS — THE NEXUS CORE

**Konsept:** Confluence'ın merkezindeki enerji çekirdeği. Hem the Fracturing'in kaynağı hem sonucu.

**Faz yapısı:** 3 faz, her biri oyuncunun build'ini farklı bir açıdan test eder.

---

### Faz 1 — Primary Class Mirroring

Nexus Core, **primary class'ınızın signature skill'ini kopyalar** ve size karşı kullanır.

Örnekler:
| Primary Class | Nexus Faz 1 Davranışı |
|--------------|----------------------|
| Warblade | Iron Charge + Gravity Cleave versiyonu — sizi köşeye sıkıştırmaya çalışır |
| Elementalist | Fireball + Glacial Spike çapraz — elemantal ritmi bozar |
| Shadowblade | Shadowstep + Backstab — sürekli konumlanır |
| Ranger | Zincir kök atışları + mesafe koruması |

**Neden bu:** "Bu sınıfın nasıl oynandığını biliyorum — şimdi bana karşı kullanabiliyor musun?"

**Faz 2 tetikleyicisi:** HP %65 altı

---

### Faz 2 — Cross-Class Exploitation

Nexus, **cross-class pasif'inizin zayıflığını** hedef alır.
Bu faz tamamen dinamik — hangi dual-class kombinasyonunuz olduğuna göre değişir.

Örnek:
- Warblade + Shadowblade = Nemesis Reaper (karanlık hasar odaklı cross-class)
  → Faz 2: Nexus ışık patlamaları yapar (karanlık build counter)
- Elementalist + Ranger = Storm Warden (alan kontrolü odaklı)
  → Faz 2: Nexus sık teleport yapar (pozisyonlu skill'leri işlevsizleştirir)

**Implementation:** Cross-class kombo ID → behavior lookup table (28 giriş, her birinde 1-2 modifier flag). Basit switch-case.

**Faz 3 tetikleyicisi:** HP %30 altı

---

### Faz 3 — The Final Reflection

**Legendary skill varsa:**
Sahaya oyuncunun Legendary skill'inin yansıması çıkar — boss o skill'i sıfırlanmadan kullanır.
Oyuncu kendi Legendary'inin ne kadar güçlü olduğunu "diğer taraftan" görür.

**Legendary skill yoksa:**
Önceki 2 fazın en etkili saldırı pattern'leri kombine edilir — daha hızlı, daha agresif.

**Nemesis Bağlantısı:**
Eğer bu run öncesinde aynı boss'a (herhangi bir boss) 3+ kez öldüysen:
- Nexus o boss'un death screen mesajını "hatırlar" — diyalog satırı değişir
- O boss tipinin mekanik modifier'ı Nexus'a eklenir (%35 extra resist)

---

### Nexus Arena

- Dairesel, büyük
- 3 köşede "fragment pillar" — yıkılabilir, yıkılınca 5s güçlü hasar alanı oluşur ama resource dolar
- Zemin aktive olabilir: Faz 3'te zemin parçaları kaybolur, ayakta durulacak alan daralır

**Ödül:** Hikaye sonu + özel meta-progress + Legendary skill guaranteed offer (bir sonraki run başında)

---

## BOSS REWARD EKONOMİSİ

| Boss | Run-içi Ödül | Meta Ödül (Echoes) |
|------|-------------|-------------------|
| Act 1 Guardian | Rare+ skill offer + secondary class seçimi | 30-50 Echoes |
| Act 2 Fractured King | Cross-class Ultimate + Epic skill offer | 80-100 Echoes |
| Act 3 Hollow Sovereign | Epic/Legendary offer | 150-180 Echoes |
| Final Boss | Hikaye sonu, Legendary guarantee sonraki run | 300 Echoes |

---

## IMPLEMENTATION ÖNCELİK SIRASI

**Faz 1 Demo için:**
- The Iron Warden implement et (en basit — shell mekanik = bool + timer)
- Faz 2 tetikleyicisi: `shelBreakCount >= 3` integer sayacı

**Faz 2 Demo için:**
- Fractured King Faz 1 ekle
- Aksiyon tetikleyici: `uniqueSkillsUsedInWindow` set + 10s timer

**Early Access için:**
- Hollow Sovereign adaptive sistem (tag counting = 10 satır kod)
- Nexus Core multi-faz
- Full Nemesis bağlantısı
