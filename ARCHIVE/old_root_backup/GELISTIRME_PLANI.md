# RIMA — Geliştirme Planı
> Faz roadmap. Mevcut durum: `CURRENT_STATUS.md` · Tasarım detayı: `TASARIM/GDD.md`
> Ajan kuralları ve görev listesi: `AGENTS.md`

---

## FAZ 0 — Proje Kurulumu ✅
Unity projesi, URP 2D, klasör iskeleti, PlayerController, test odası, ana menü. **Tamamlandı.**

---

## FAZ 1 — Core Loop ✅ (devam ediyor)

**Tamamlanan:**
- Warblade animasyon sistemi — 47 clip, 8 yön, Animator Controller
- Hades-style 3-hit combo (25→30→40 dmg, 1.5s window)
- ShardWalker + VoidThrall — sprite, animasyon, Unity entegrasyonu
- Skill Draft sistemi — SkillOfferGenerator, DraftManager, SkillOfferUI
- StatusEffectSystem — Ice/Fire/Poison/Shocked/RiftMark zincirleri
- EnemyTier — Normal/Elite/Champion/MiniBoss
- PassiveStatusUI — HUD efekt ikonları
- Tilemap kurulumu — 20x15 placeholder ground

**Kalan:**
- [ ] SeamCrawler — attack+death animasyonu + Unity entegrasyonu
- [ ] Act 1 tilemap — soğuk taş + mavi rift çatlağı (gerçek tile)
- [ ] Iron Warden boss — AI + animasyon + boss HP bar
- [ ] Ölüm + restart ekranı
- [ ] Map fragment / fog-of-war sistemi

**Faz 1 bitti mi?** 1 class, 3 düşman tipi, 1 boss, skill draft, ölüp restart. Şu an: 3/5.

---

## FAZ 2 — Act 1 Tamamlanıyor

**Hedef:** 3 class seçilebiliyor, Act 1 tam 6-7 oda, boss sonrası secondary class.

- [ ] Class sistemi altyapısı (ClassData, ClassManager, ClassSelectUI)
- [ ] Elementalist class — ManaSystem, 4 skill, animasyon
- [ ] Shadowblade + Ranger class — EnergyComboSystem, FocusSystem, 4'er skill
- [ ] Secondary class seçimi — Act 1 boss sonrası 2 kart
- [ ] Act 1 tam harita — Act1MapGenerator, 6 oda prefabı
- [ ] Shop sistemi — GoldManager, ShopManager, ShopUI
- [ ] Elite: The Twice-Born — hasar paylaşım + berserk mekaniği
- [ ] Fracture-Born spawn sistemi — 4 aşamalı çıkış animasyonu

---

## FAZ 3 — Demo

**Hedef:** Act 1-2 tam, cross-class çalışıyor, hub var, Steam demo'ya hazır.

- [ ] Cross-class sistemi — 28 CrossClassPassiveData.asset, Act 2 boss sonrası ultimate slot
- [ ] Act 2 + Fractured King boss — build-adaptive faz tetikleyici
- [ ] Spirit Encounter — 3 NPC tipi, dialog, offer sistemi
- [ ] Grudge sistemi — öldürme tipine göre düşman direnç
- [ ] Curse Gate + Event odası — HP maliyet kapısı, 10 event
- [ ] Temel hub — MetaProgressionManager, Shattered Echoes, 4 NPC
- [ ] Lokalizasyon altyapısı (TR/EN)

---

## FAZ 4 — Full Game

**Hedef:** 8 class, 3 act, full meta-progression.

- [ ] Kalan 4 class: Hexer, Summoner, Ravager, Brawler (+ animasyon)
- [ ] Act 3 + Hollow Sovereign boss (build-adaptive, 3 faz)
- [ ] Nexus Core final boss (çok fazlı, build-aware)
- [ ] Visual polish — VFX, shader, ışık geçişleri
- [ ] Tüm skill ikonları + skill açıklamaları

---

## FAZ 5 — Ses + Steam

- [ ] Ses sistemi — FMOD veya Unity Audio, müzik/SFX
- [ ] Steam build — Steamworks entegrasyonu, achievements temel set
- [ ] Playtest döngüsü — balance, ölçümler, feedback iterasyon
