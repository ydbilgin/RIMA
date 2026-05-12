---
status: REFERENCE
faz: 1
tarih: 2026-05-14
ozet: "RIMA referans dokumani"
---
# FAZ 4 — FULL DEMO

*Claude: Sadece bu dosyayı oku. Faz 1-3 tamamlanmış varsay.*

---

## SCOPE

**Hedef:** 30-45 dakikalık tam run. Act 1-2 tam, cross-class Ultimate, Curse Gate, Event odası, Fracture Echoes, temel meta-progression. Steam Demo'ya hazır.

**Ne VAR (Faz 3'e ek):**
- **+2 class: Summoner, Hexer** (toplam 10 class tamamlanır)
- Cross-class Ultimate (Act 2 boss sonrası açılır)
- Legendary skill tier
- Curse Gate odası (HP harca → büyük bonus)
- Event odası (ikili seçenekli hikaye kararı)
- Curse sistemi (5 basit efekt)
- **Fracture Echoes** (ilk 2 boss: Penitent Sovereign + Echo Twin)
- Temel meta-progression (Echoes harcanabilir: class unlock, hub)
- Hub NPC etkileşimi (Ferryman, Vrel, Sister Mourne)
- +2 mob: Spore Hollow, The Wound (tam implementasyon)
- Fracture Sovereign boss (Act 3 başlangıcı — sadece F1)
- Codex + item description lore

**Ne YOK:**
- Act 3 tam (sadece başlangıç, boss test)
- Grudge / Nemesis
- Final Boss
- Zorluk modları
- Fracture Echoes (kalan 2 boss: Fracture Sovereign, The Architect)

---

## CROSS-CLASS ULTIMATE

Act 2 boss (Echo Twin) öldükten sonra açılır. Tüm 10 class kombinasyonu için benzersiz (90 kombo toplam, bu fazda 56 aktif):

| Primary | Secondary | Arketip | Ultimate | Efekt |
|---------|-----------|---------|----------|------|
| Warblade | Elementalist | Runeguard | **Runic Eruption** | Rage + Mana birleşir → büyük AoE rune patlaması |
| Warblade | Shadowblade | Iron Shadow | **Killing Floor** | 6s: her kill Rage+20 + instant stealth |
| Warblade | Ranger | Vanguard | **Spearhead** | Iron Charge + Aimed Shot birleşir → full arena dash + hasar |
| Elementalist | Warblade | Runeguard | **Runic Eruption** | Mana 0'a düşürür → tüm skill'ler 5s ücretsiz + Rage'den Mana regen |
| Elementalist | Shadowblade | Arcane Shadow | **Phantom Burst** | Blink × 3 zincirleme, her biri AoE + stealth |
| Elementalist | Ranger | Stormchaser | **Arcane Volley** | Rain of Arrows + elemental infuse → yağmur freeze/burn |
| Shadowblade | Warblade | Iron Shadow | **Killing Floor** | Shadow Dance + Bladestorm birleşir → spin + stealth |
| Shadowblade | Elementalist | Arcane Shadow | **Phantom Burst** | Vanish + Blink explosions |
| Shadowblade | Ranger | Phantom Archer | **Shadow Volley** | Stealth'teyken Rapid Fire → her ok backstab hasarı |
| Ranger | Warblade | Vanguard | **Spearhead** | Disengage + Iron Charge → push-pull combo, arena kontrolü |
| Ranger | Elementalist | Stormchaser | **Arcane Volley** | Focus 100: otomatik Chain Lightning → tüm ok'lara elektrik |
| Ranger | Shadowblade | Phantom Archer | **Shadow Volley** | Rain of Arrows sırasında stealth + her ok poison |

---

## CURSE SİSTEMİ (5 Efekt)

Curse Gate'ten veya bazı event'lerden alınır. Güçlü bonus karşılığında kalıcı debuff:

| Curse | Debuff | Bonus |
|-------|--------|-------|
| **Curse of Fragility** | Max HP -%20 | Tüm hasar +%25 |
| **Curse of Impatience** | Skill CD +%30 | Her kill CD -%2s |
| **Curse of Hunger** | HP regen yok | Her kill +5 HP |
| **Curse of Glass** | Alınan hasar +%40 | Crit şansı +%25 |
| **Curse of Echoes** | Her 30s: rastgele skill 5s kilitlenir | Kilitli skill'in hasarı ×2 açıldığında |

---

## ODA TİPLERİ (Faz 3'e ek)

### Curse Gate 🌀
- Odaya girmek HP harcar (max HP'nin %15'i)
- İçeride: 1 Curse seçeneği (yukarıdaki tablodan)
- **Terk etme:** HP kaybı geri gelmez ama curse almazsın + küçük HP yenilemesi

### Event 🎲
- İkili seçenekli hikaye/karar odası
- Seçenekler build'e göre değişmez (statik)

**10 Event:**

| # | Olay | Seçenek A | Seçenek B |
|---|------|-----------|-----------|
| 1 | Kırık ayna | Yansımaya bak → +1 skill tier | Kır → +50 Shards |
| 2 | Ölü gezgin | Eşyalarını al → random skill offer | Gömür → +15 max HP |
| 3 | Rift çatlağı | İçine atla → 2 oda atla (3 oda ilerisinde çık) | Kapat → +30 Echoes |
| 4 | Zincirli ruh | Serbest bırak → Spirit Encounter burada | Güç al → +%20 hasar, -10 max HP |
| 5 | Eski altar | Dua et → HP full | Yık → büyük Shards + curse riski |
| 6 | Tüccar hayaleti | Alışveriş (ucuz) | Soygunla → tüm Shards ×2 ama sonraki shop yok |
| 7 | Karanlık kuyu | Atla → bilinmeyen ödül | Geç → +5 max HP |
| 8 | Kırık taht | Otur → tüm skill CD sıfırla + HP full | Devir → Shards + rare augment |
| 9 | Echo parçası | Hatırla → lore fragment + küçük stat | Unut → +20 Echoes |
| 10 | Savaş arenası | Gir → wave savaş, büyük ödül | Reddet → +10 HP, devam |

---

## MOB'LAR (Faz 3'e ek)

| Mob | Act | Mekanik | Tier | Sprite |
|-----|-----|---------|------|--------|
| **Spore Hollow** | 2 | Yavaş yaklaşım, zemine spor bulutu, ölünce büyük poison AoE | Elite (160px) | ❌ |
| **The Wound** | 1,2,3 | Pasif iyileştirici, önce öldürülmesi gereken hedef | Special (128px) | ❌ |

---

## BOSS: FRACTURE SOVEREİGN (Act 3 — Sadece F1 test)

> Tam boss Faz 5'te. Bu fazda sadece Faz 1 prototip.

### Faz 1 — "Yara Açıldı" (HP: 100% → 60%)

| Saldırı | Mekanik |
|---------|---------|
| **Fracture Beam** | Yavaş dönen lazer çizgisi |
| **Void Shard** | 3 yönde projectile |
| **Sovereign Step** | Boss teleport |
| **Gravity Pull** | Oyuncuyu kenara çeker |

---

## META-PROGRESSİON (Temel)

### Echoes Harcama

| Harcama | Maliyet | Efekt |
|---------|---------|-------|
| Elementalist Unlock | 80 Echo | Class seçilebilir |
| Ranger Unlock | 80 Echo | Class seçilebilir |
| Shadowblade Unlock | 150 Echo | Class seçilebilir |
| Vrel: Augment Craft | 50 Echo | 1 random augment craft |
| Cartographer: Harita QoL | 100 Echo | Haritada 1 oda ilerisini gör |

### Hub NPC'ler

| NPC | İşlev | Diyalog Tetikleyici |
|-----|-------|---------------------|
| **Ferryman** | Lore, tanıklık | Her run sonrası 1 yorum — boss'a göre değişir |
| **Vrel** | Craft / augment | Boss Fragment işleme |
| **Sister Mourne** | HP / iyileştirme | Curse aktifse özel diyalog |
| **Cartographer** | Harita upgrade | Echo ile harita genişletme |

---

## SİSTEMLER

| Sistem | Açıklama |
|--------|----------|
| **CrossClassUltimateData.asset** | 90 kombo × ultimate ScriptableObject (bu fazda 56 aktif) |
| **CrossClassUltimateManager.cs** | Echo Twin sonrası ultimate aktive et |
| **UltimateSlotUI.cs** | HUD'da ultimate göstergesi + CD |
| **CurseManager.cs** | 5 curse efekti + HUD ikonu |
| **CurseGateRoom.cs** | Oda mechanic: HP harca → curse offer |
| **EventRoom.cs** | 10 event tanımı + ikili seçim UI |
| **EventData.asset** | ScriptableObject: her event'in tanımı |
| **MetaProgressionManager.cs** | Echoes biriktir + harca + unlock |
| **HubManager.cs** | Hub sahne yönetimi + NPC etkileşimi |
| **CodexManager.cs** | Lore fragment toplama + gösterim |
| **Legendary Skill Tier** | Epic'ten atlatma: skill'in çalışma biçimi kökten değişir |

---

## ANİMASYON BÜTÇESİ

| İçerik | PixelLab Gen |
|--------|-------------|
| Spore Hollow tüm set | ~40 gen |
| The Wound idle/pulse/attack/death | ~24 gen |
| Fracture Sovereign F1 (4 saldırı) | ~40 gen |
| Hub NPC sprite'ları (4 NPC × idle) | ~16 gen |
| Event oda görseleri (10 event × ikon) | ~10 gen |
| **Toplam** | **~130 gen** |

---

## ÇIKIŞ KRİTERLERİ

- [ ] Act 2 boss sonrası cross-class Ultimate açılır ve kullanılabilir
- [ ] Curse Gate: HP harcanır, curse alınır, HUD'da görünür
- [ ] Event odası: 10 event'ten rastgele 1, ikili seçim çalışır
- [ ] Echoes hub'da harcanabilir (class unlock, craft)
- [ ] Hub NPC'lerle konuşulabilir
- [ ] Codex'e lore fragment eklenir
- [ ] Legendary tier: en az 1 skill'de kökten değişim test edilmiş
- [ ] Tam run: Act 1 + Act 2 + 2 boss = 30-45 dakika
- [ ] **Bu build Steam Demo olarak yayınlanabilir**

