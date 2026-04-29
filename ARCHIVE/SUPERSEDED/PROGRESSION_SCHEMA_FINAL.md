# RIMA — Progression & Draft Sistemi — FİNAL KARAR ŞEMASI
*Onay: 2026-04-12 | Gemini + ChatGPT + Claude final karar*

---

## ÖZET: 5 KATMAN

```
┌─────────────────────────────────────────────────────────────────┐
│  KATMAN 5 — RELİK                                               │
│  Boss kill ödülü. Çok güçlü. Max 3/run. Slot yok.             │
├─────────────────────────────────────────────────────────────────┤
│  KATMAN 4 — TRAIT                                               │
│  Saf stat birikim. Slot yok, seviye yok.                       │
│  KAYNAK: SADECE sandık + forge — draft ekranına GİRMEZ.        │
├─────────────────────────────────────────────────────────────────┤
│  KATMAN 3 — PASİF SKİLL                                        │
│  Draft'tan. Slot yok. Max 3 seviye. Upgrade edilir.            │
├─────────────────────────────────────────────────────────────────┤
│  KATMAN 2 — AKTİF SKİLL                                        │
│  Draft'tan. 4+2 slot. Değiştirilebilir.                        │
├─────────────────────────────────────────────────────────────────┤
│  KATMAN 1 — TEMEL SALDIRI (LMB / RMB)                         │
│  Her zaman var. Attack Forge ile ekol seçilir.                 │
└─────────────────────────────────────────────────────────────────┘
```

**Temel kural:** Oyuncu bu 5 katmanla aynı anda karşılaşmaz.
Katmanlar zamana yayılır — her milestone'da bir yeni katman açılır.

---

## KATMAN 1 — TEMEL SALDIRI (LMB / RMB)

### Ekol Sistemi
- LMB ve RMB için ayrı ekol seçimi
- 3 ekol × 3 seviye
- Ekol seçimi run başında değil, milestone anında yapılır

### Attack Forge Kuralları (REVİZE — 2026-04-12)
| Forge | Oda | Ne yapılır |
|-------|-----|-----------|
| Forge #1 | **Oda 4** | Ekol seç (Lv1) |
| Forge #2 | **Oda 8** | Sadece yükselt (Lv2) — değiştirme yok |
| Forge #3 | Boss kill ödülü (opsiyonel) | Lv3 VEYA ekol değiştirme hakkı |

**Neden değiştirme yok Oda 8'de?**
Oyuncu Oda 4'te ne seçtiyse ona yatırım yapmış olur.
Oda 8'de "değiştir mi yükselt mi" sorusu build sadakatini öldürür.
Ekol switch = özel ödül — kazanılması lazım.

### Warblade LMB Ekolları

| Ekol | İsim | Lv1 | Lv2 | Lv3 |
|------|------|-----|-----|-----|
| Rage | **Fury Strikes** | Hit başına +2 Rage | Her 3. hit +8 Rage bonus | Rage %80+ → LMB +%30 hasar |
| Bleed | **Savage Edge** | 3. hit bleed (3s) | Bleed hedefe +%20 LMB hasar | Bleed tick LMB CD -0.1s |
| Execute | **Bone Breaker** | Son hit yavaşlatır | HP<%50 hedefe +%25 LMB hasar | Knockback + 1s mikro stun |

### Warblade RMB Ekolları (Faz 2)

| Ekol | İsim | Lv1 | Lv2 | Lv3 |
|------|------|-----|-----|-----|
| Block | **Iron Guard** | Süre +0.3s | Başarılı block → Rage +15 | Mükemmel block → 1.5s stun |
| Counter | **Blade Mirror** | Hasarın %30'unu yansıt | Yansıma +%50 | Yansıma AoE (1m) |
| Drain | **Blood Feast** | Block başına +5 HP | Block healing ×2 | Block bozulursa Rage patlaması |

### Faz Planı
- **Faz 1:** LMB ekol (3 seçenek, Forge #1 Oda 4, Forge #2 Oda 8)
- **Faz 2:** RMB ekol eklenir

---

## KATMAN 2 — AKTİF SKİLL

### Sistem (onaylandı)
- 4 primary slot (Q/E/R/F) + 2 secondary slot (Z/X — secondary class sonrası)
- Draft'tan seçilir, slot dolu → replace modu (ücretsiz)
- Tier ağırlık: Common 55 / Rare 27 / Epic 12 / Mythic 5 / Legendary 3
- Depth lock: Epic/Mythic Oda 3+, Legendary Oda 7+ (run depth lock — YAPILACAK)
- Mythic: Primary-only mastery — o class'ı primary seçmemişsen havuza girmez
- Legendary: Run-definer — oyunun temel kuralını kırar, bkz. LEGENDARY_SKILLS.md

### Havuz boyutu
- WB solo: 12 aktif
- WB + Secondary: 12 WB + 6 cross-class + 1 sinerji = 19 aktif

### Faz Planı
- **Faz 1:** WB aktifler (12)
- **Faz 2:** Cross-class aktifler + sinerji skilleri

---

## KATMAN 3 — PASİF SKİLL

### Sistem (onaylandı)
- Slot yok, max 3 seviye
- Aynı pasifi tekrar almak = upgrade
- Upgrade önceliği: Lv1 pasifler daha çok çıkar

### Pasif Listesi (REVİZE — 2026-04-12)

**Neutral (8) — tüm class kombinasyonlarına açık:**

| İsim | Tier | Etki |
|------|------|------|
| Iron Body | Common | Max HP +25/40/60 |
| Berserker's Blood | Rare | Kaynak kazanımı +%15/25/35 |
| Predator's Eye | Rare | Tüm hasar +%8/15/22 |
| War Veteran | Common | Kill → +5/8/12 kaynak |
| Unyielding | Epic | HP<%50: 3/4/5s hasar bağışıklığı |
| Battle Scars | Common | Oda temizlenince +3/5/8 max HP |
| Adrenaline Rush | Rare | Kill sonrası 3s +%20/30/40 hız |
| Ancient Instinct | Epic | Saldırı algılanınca hasar -%20/30/40 |
| **Opportunistic Strike** | Rare | CC altındaki hedefe +%15/25/40 crit şans *(Trait'ten pasife taşındı)* |
| **Veteran's Scar** | Common | Oda temizlenince +2 perm hasar (max +30) *(Trait'ten pasife taşındı)* |

**Warblade (5):**
Ironclade Momentum / Blood Drinker / Wrath Protocol / Tempered Fury / Berserker's Resolve

**Cross-class (3/secondary) — Faz 2:**
- Elem: Arcane Attunement / Elemental Surge / Mana Shield
- Shadow: Shadow's Embrace / Crimson Edge / Opportunist
- Ranger: Eagle Eye / Survival Instinct / Hunter's Focus

### Faz Planı
- **Faz 1:** Neutral (10) + WB (5) = 15 pasif
- **Faz 2:** Cross-class pasifler (9) eklenir

---

## KATMAN 4 — TRAIT

### Sistem (REVİZE — 2026-04-12)
- Saf sayısal bonus — mekanik koşul YOK
- "Pasif build yönü belirler, Trait güçlendirir" — bu çizgi korunur
- Kaynak: **SADECE sandık + forge** — draft ekranına GİRMEZ
- Aynı trait max 3 kez alınabilir (stacks)

### Trait Listesi (saflaştırıldı — mekanik olanlar pasife taşındı)

| İsim | Değer | Max Stack |
|------|-------|-----------|
| **Toughened Hide** | Max HP +20 | 3x |
| **Honed Reflexes** | Tüm CD -%5 | 2x |
| **Iron Will** | CC süresi -%20 | 2x |
| **Deep Reserves** | Kaynak max +15 | 3x |
| **Stoic Endurance** | Savaş dışı HP regen +1/sn | 3x |
| **Killing Momentum** | Kill sonrası 3s hız +%15 | 3x |

### Nereden Gelir
1. **Sandık:** Gold / Skill / Trait — 3 seçenekten biri sunulur
2. **Forge Draft (Faz 2):** Her 5. draftta trait seçeneği

### Faz Planı
- **Faz 1:** Sadece sandıktan
- **Faz 2:** Forge Draft entegrasyonu

---

## KATMAN 5 — RELİK (Faz 2)

### Sistem
- Boss kill ödülü, 3 seçenek sunulur, 1 alınır
- Max 3 relic/run
- Run-defining güç seviyesi

### Relic Listesi
| İsim | Boss | Efekt |
|------|------|-------|
| Bone Trophy | Boss 1 | Her kill +1 perm hasar (max +25) |
| Warblade's Crest | Boss 1 | Rage max +30, Rage asla 0'a inmez |
| Echo Stone | Boss 2 | Son skill 3s sonra otomatik tekrar |
| Blood Pact | Boss 2 | Oda başı HP -10, tüm hasar +%15 |
| Void Shard | Boss 3 | Kill'de %5 patlama zinciri |
| Phantom Grip | Boss 3 | 2 skill aynı anda aktif (CD +%30) |

---

## SİNERJİ SKİLLERİ (REVİZE — 2026-04-12)

### Açılma Koşulu (yeni)
Secondary seçilince hemen DEĞİL.

```
KOŞUL:
  ✓ Secondary class seçilmiş
  ✓ Primary'den ≥1 aktif veya pasif alınmış
  ✓ Secondary'den ≥1 aktif veya pasif alınmış
→ Sinerji havuza girer
→ Her geçen draft ile çıkma ağırlığı artar
```

**Neden?** Oyuncu her iki kimliği de kurmuş olmalı.
Sinerji skill "kazanılmış" hissetmeli — bedava değil.

### Sinerji Skill Listesi
| İsim | Combo | Tier | Özet |
|------|-------|------|------|
| Molten Wrath | WB+Elem | Epic | Rage 50+ Fireball → lav patlaması, yanma ×2 |
| Iron Phantom | WB+Shadow | Epic | Iron Charge → Shadow Step → backstab + mini stealth |
| Predator's Advance | WB+Ranger | Epic | CC'li hedefe Iron Charge → hasar ×2 + Rage +50 |

---

## BİR RUN'UN TAM AKIŞI

```
[ODA 1]   İlk draft → 2 WB aktif + 1 neutral pasif
[ODA 2]   Normal draft
[ODA 3]   Normal draft
           ── SANDIK: Gold / Skill / Trait seçimi ──
[ODA 4]   ⚔️  ATTACK FORGE #1 → LMB ekol seç (Lv1)
[ODA 5]   Normal draft
[ODA 6]   Normal draft
           ── SANDIK ──
[ODA 7]   Normal draft
[ODA 8]   ⚔️  ATTACK FORGE #2 → LMB yükselt (Lv2) — değiştirme yok
[ODA 9]   Normal draft
[BOSS 1]  Boss kill → Relic seç + opsiyonel LMB Lv3 veya ekol switch
[ODA 10+] Cross-class draft, sinerji skilleri açılır
[BOSS 2]  Relic #2
...
```

---

## FAZ 1 YAPILACAKLAR (güncellendi)

| Görev | Dosya | Öncelik |
|-------|-------|---------|
| Oda sayacı + milestone trigger | DraftManager.cs | ⭐ 1 |
| Epic/Mythic/Legendary run-depth lock (Oda 3+ / 3+ / 7+) | SkillOfferGenerator.cs | ⭐ 1 |
| LMB ekol sistemi | LMBUpgradeSystem.cs (yeni) | ⭐ 2 |
| Attack Forge UI | AttackForgeUI.cs (yeni) | ⭐ 2 |
| WB 4 pasif + Neutral 2 pasif ekleme | SkillDatabase.cs | ⭐ 2 |
| Trait sistemi (sandıktan) | TraitSystem.cs (yeni) | ⭐ 3 |
| Sinerji koşul logic | DraftManager.cs | Faz 2 |
| RMB ekol | — | Faz 2 |
| Relic sistemi | RelicSystem.cs (yeni) | Faz 2 |

---

*Referans: ChatGPT + Gemini review 2026-04-12*
*Değişiklikler: Trait kaynağı kısıtlandı, Forge #2 Oda 8'e çekildi, ekol switch boss ödülüne taşındı, sinerji koşulu güçlendirildi, 2 trait pasife taşındı*
