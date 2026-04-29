# FAZ 1 — CORE LOOP (Combat Prototype)

*Claude: Sadece bu dosyayı oku. Bu fazın scope'u dışındaki hiçbir şeyi implemente etme.*

---

## SCOPE

**Hedef:** Combat hissi çalışıyor mu? 1 class ile oda temizleme loop'u test edilebilir.

**Ne VAR:**
- 1 oynanabilir class (Warblade)
- Act 1 odaları (8-9 oda, sadece Combat + Elite tipleri; opsiyonel 1 Shop)
- Temel düşman AI (7 mob)
- Skill draft UI (Common tier)
- Rage sistemi
- Ölüm + restart
- Map fragment (kısmi görünür harita)
- Act 1 boss (Penitent Sovereign — Faz 1 sadece)

**Ne YOK:**
- Shop, Spirit Encounter, Curse Gate, Event odası
- Secondary class seçimi
- Cross-class pasifler
- Rare/Epic/Legendary tier
- Echoes (meta currency)
- Hub NPC etkileşimi (sadece placeholder)

---

## CLASS: WARBLADE ⚔️

**Core Fantasy:** "Yaklaş. Sabitle. Zırh kır. İnfaz et."
**Kaynak:** Rage (0-100) — hasar VEREREK +10/vuruş, CC'li düşmana +20, boşta -5/sn
**[V] Burst:** BLADESTORM — Rage %100: 5s spin, CC immune, her 0.5s AoE %120 hasar
**Aktif Slot:** 4 (Q, E, R, F) — 2 slot kilitli görünür ama kullanılamaz

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Iron Charge** ★ | ▶⬡ | Core | 8m dash + 1.5s stun, Rage+20 | Stun'daki hedefe ilk vuruş → +%80 hasar |
| 2 | **Crippling Blow** | ⚡💥 | Core | Büyük hasar + iyileşme -%50 (6s) | Iron Charge sonrası → iyileşme -%100 |
| 3 | **Iron Crush** | ✦ | Core | 6s: tüm hasar +%30 | Burst window açıkken → katlanır |
| 4 | **Gravity Cleave** | ⬡⚡ | Core | Silahı yere çarpar, 4m çapında çeker + %140 hasar, 0.8s slow | Iron Charge sonrası → çekilenler 1.5s stun, Rage+15 |
| 5 | **Sunder Mark** | ✦↓ | Core | Hedefe işaret: 8s zırh -%40, tüm hasar bonusu görünür | Death Blow aktifken → zırh -%60 |
| 6 | **War Stomp** | ⬡↑ | Core | 3m knockup 2s, Rage+25 | Bladestorm sırasında → +1s uzar |
| 7 | **Ironclad Momentum** | ↑✦ | Core | 6s: alınan hasar %30 yok sayılır + her 10 hasar = +10 Rage | War Stomp sonrası → savunma %50'ye çıkar |
| 8 | **Iron Counter** | ↑⚡ | Core | 0.8s pencere: vurulursa %180 karşı saldırı + Rage+25 + 0.5s stun | Rage 80+ → 2× tetiklenir |
| 9 | **Blade Rush** | ▶↑ | Advanced | 6m dash + çizgideki herkese %120, Rage+15/hedef | 3+ hedef → Rage+50 |
| 10 | **Battle Surge** | ✦↑ | Advanced | 8s: her Rage harcaması = HP +%5 | Rage 80+'ta aktive → süre 12s |
| 11 | **Deep Wound** | ⚡↑ | Advanced | Bleed DoT 8s + Rage+20 | Iron Crush window → bleed tick 2× |
| 12 | **Death Blow** | 💥⬡ | Master | SADECE HP<%30: %400 hasar, Rage boşaltır | Crippling Blow aktifken → %600 |

**Build Eksenleri:**
- **Execution:** Iron Charge → Crippling Blow → Iron Crush → Death Blow
- **Control Breaker:** Gravity Cleave → War Stomp → Sunder Mark → Death Blow
- **Last Stand:** Ironclad Momentum → Iron Counter → Battle Surge → Death Blow

**Skill Draft — Common Tier:**
- Bu fazda skill'ler sadece Common tier'da teklif edilir
- Her oda temizlenince 3 skill offer (yeni skill ekle VEYA mevcut skill'i Common→Rare atlatma şansı)
- **İmza Skill (★):** Iron Charge ilk odada garantili teklif
- Tier atlatma bu fazda yok — sadece Common versiyonlar aktif

**Resource Bar UI:**
- Rage bar: çatlak, sert, agresif form — kenarlarda kırmızı enerji
- Dolunca: Bladestorm kullanılabilir göstergesi (parıldama)

---

## MOB'LAR (7 Mob — Act 1)

### Grunt Tier (96px)

| Mob | Mekanik | Sprite Durumu |
|-----|---------|--------------|
| **ShardWalker** | Orta hız, parça fırlatma, ölünce parça AoE | ✅ Prefab hazır |
| **VoidThrall** | Yakın dövüş, ölünce İKİYE BÖLÜNÜR (2× HalfThrall) | ✅ Prefab hazır |
| **Penitent** | 3-hit combo, son vuruş armor break | ✅ Prefab hazır |
| **ChainWarden** | Oyuncuyu zincirle çeker | ✅ Prefab hazır |
| **FractureImp** | Melee + shard scatter ikincil hasar | ✅ Prefab hazır |
| **SeamCrawler** | Zemin çatlaklarında kayar, ambush | ✅ Anim hazır |

### Swarm Tier (48px)

| Mob | Mekanik | Sprite Durumu |
|-----|---------|--------------|
| **Hollow Mite** | Çok hızlı, zigzag, sürü taktik (4-8 birden spawn) | ❌ Üretilecek |

### Elite Varyantlar (Bu fazda)

Her grunt mob'a 1 affix uygulanabilir:

| Affix | Efekt | Görsel |
|-------|-------|--------|
| **Blazing** | Saldırılar ateş DoT ekler | Turuncu aura |
| **Glacial** | Saldırılar slow/freeze | Mavi kristal |
| **Void-Touched** | Gecikmeli zone + teleport | Mor parıltı |
| **Fractured** | Ölünce shard scatter veya echo spawn | Çatlak efekt |

### Özel Mob (Nadir)

| Mob | Mekanik | Sprite Durumu |
|-----|---------|--------------|
| **The Wound** | Pasif: yakındaki düşmanlara 2HP/s iyileştirme, ölünce flash + tüm düşmanlar %20 HP hasar | ❌ Üretilecek |

---

## BOSS: PENİTENT SOVEREİGN (Faz 1 — Sadece F1)

> Bu fazda sadece Faz 1 implemente edilir. Tam boss (2 faz) Faz 2'de tamamlanır.

**Tema:** Boyun eğmiş ama kırılmamış. Zincirleriyle savaşır.
**Arena:** Standart combat odası boyutunda, etrafta zincir kalıntıları

### Faz 1 — "Zincirlerin Altında" (HP: 100% → 50%)

| Saldırı | Mekanik | Uyarı |
|---------|---------|-------|
| **Zincir Kamçı** | Önüne 6m düz çizgi hasar | Kolunu geriye çekiyor |
| **Penitent Surge** | 4m çevreye AoE itme + hasar | Zemine yumruk, halka görünür |
| **Kelepçe Fırlatma** | Uzak oyuncuya zincir atar, 2s slow | Zincir havada görünür |
| **Kutsanmış Kırbaç** | 180° yay hasar, yakın mesafe | Omzunu döndürüyor |

**%50 HP'de:** Boss yere çöker — geçiş sahnesi başlar ama Faz 2'ye geçmez (bu fazda sadece "Boss %50'de ölüyor" şeklinde placeholder). Tam 2-faz deneyimi Faz 2'de.

---

## ODA TİPLERİ

| Tip | İkon | İçerik |
|-----|------|--------|
| **Combat** | ⚔️ | Standart düşman dalgası. Temizle → skill offer |
| **Elite** | 💀 | Daha zor (affix'li mob'lar). Rare+ ödül + küçük HP yenilemesi |

**Act 1 Harita Yapısı:**
- 8-9 oda lineer/hafif dallanma (5-6 Combat, 1 Elite, 0-1 Shop, 1-2 Unknown, 1 Boss)
- Kapılar oda tipini gösterir (ikon + renk)
- Her oda temizlenince zemine harita parçası düşer — almak zorunlu
- Parça alınmadan sonraki odaya geçilemiyor
- Tam oda detayı: `TASARIM/ROOM_MECHANICS.md`

---

## SİSTEMLER

### İmplemente Edilecek

| Sistem | Script | Durum |
|--------|--------|-------|
| Rage kaynağı | `RageSystem.cs` | ✅ Var |
| 8-yön hareket + dash | `PlayerController.cs` | ✅ Var |
| 3-hit combo (25→30→40) | `ComboSystem.cs` | ✅ Var |
| Skill draft UI (3 seçenek) | `SkillOfferGenerator.cs`, `DraftManager.cs` | ✅ Var |
| Status efekt sistemi | `StatusEffectSystem.cs` | ✅ Var |
| HUD 6 slot (4 aktif, 2 kilitli) | `SkillBarUI.cs` | ✅ Var |
| Component-based mob mimarisi | `BaseMobBehavior.cs` + `MobAttack_*.cs` | ✅ Var |
| Elite affix sistemi | `MobAffix_*.cs` (4 tip) | ✅ Var |
| EnemyTier (Normal/Elite/Champion) | `EnemyTier.cs` | ✅ Var |
| PassiveStatusUI (HUD efekt ikonları) | `PassiveStatusUI.cs` | ✅ Var |
| **Ölüm + restart ekranı** | `DeathScreen.cs` | ❌ İmplemente et |
| **Map fragment / kısmi harita** | `MapFragment.cs`, `FogOfWar.cs` | ❌ İmplemente et |
| **Act 1 tilemap (gerçek tile)** | Tilemap sistemleri | ⚠️ Placeholder var |
| **Boss AI (Penitent Sovereign F1)** | `BossAI_PenitentSovereign.cs` | ❌ İmplemente et |
| **Boss HP bar** | `BossHealthBar.cs` | ❌ İmplemente et |

### Ölüm Ekranı (Minimal)
1. Seni öldüren düşman (sprite + isim)
2. Hangi odada öldün (oda tipi)
3. Kısa run recap: kaç düşman, hangi skill'ler aktifti

---

## ANİMASYON BÜTÇESİ

| İçerik | PixelLab Gen | Durum |
|--------|-------------|-------|
| Warblade tüm set | — | ✅ Tamamlandı |
| ShardWalker idle/walk/attack/death | — | ✅ Tamamlandı |
| VoidThrall idle/walk/attack/split | — | ✅ Tamamlandı |
| Penitent walk/attack/death | — | ✅ Tamamlandı |
| ChainWarden walk/attack/death | — | ✅ Tamamlandı |
| FractureImp walk/attack/death | — | ✅ Tamamlandı |
| SeamCrawler slide/attack/death | — | ✅ Tamamlandı |
| **Hollow Mite idle/walk/death** | ~16 gen | ❌ Üretilecek |
| **The Wound idle/pulse/death** | ~16 gen | ❌ Üretilecek |
| **Penitent Sovereign (boss) idle/attack×4/phase/death** | ~60 gen | ❌ Üretilecek |
| **Toplam yeni üretim** | **~92 gen** | |

---

## ÇIKIŞ KRİTERLERİ

Bu faz "tamamlandı" sayılması için:

- [ ] Warblade ile 8-9 oda sırayla temizlenebilir
- [ ] Her oda sonrası skill draft çalışır (3 seçenek gösterilir)
- [ ] 4 slot'a skill yerleştirilebilir
- [ ] Elite oda'da affix'li mob spawn olur
- [ ] Penitent Sovereign Faz 1 %50 HP'de "ölür" (placeholder)
- [ ] Ölüm ekranı gösterilir, restart çalışır
- [ ] Combat hissi test edilmiş: hitstop, hit flash, death VFX çalışır
- [ ] Harita parçası mekanizması çalışır (parça al → sonraki oda)
