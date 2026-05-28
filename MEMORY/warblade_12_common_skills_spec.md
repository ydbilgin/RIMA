---
name: warblade-12-common-skills-spec
description: Warblade 12 Common (beyaz tier) skill canonical liste — NLM source (S41/S43/S44 lock). S44 Active Slot Rule sonrası 8 active + 4 passive/echo. Demo Faz 1 skill draft havuzu için 6 öncelikli yetenek seç.
metadata:
  type: project
  source: NLM 30ddffa5 2026-05-27 (SINIF_VE_SKILL_KARAR_BELGESI.md + SKILL_POOLS_10CLASS_2026-05-07.md)
---

## Warblade kaynak: Rage
- Pasif dolmaz, hasar verme + CC + skill bonus ile dolar
- **Rage Outlet** (default RMB, slot işgal etmez, Rage>30 push action)

## 12 Common (Beyaz Tier) Liste

### Active skills (8)
1. **Iron Charge ★ (signature)** — 8m dash + 1.5s stun, Rage+20, Hold(0.5s)→Blade Rush 6m line damage
2. **Crippling Blow** — büyük hasar + iyileşme -%50 6s
3. **Gravity Cleave** — 4m pull + %140 + 0.8s slow, Iron Charge sonrası → stun + Rage+15
4. **Sunder Mark** — hedef zırh -%40 8s (Sundered state)
5. **Earthsplitter** (eski War Stomp) — 3m knockup 2s, Rage+25, Hold→3-wave + Broken stack
6. **Iron Counter** — Rage+25, 0.8s parry window → %180 reflect + 0.5s stun
7. **Blade Rush** — Iron Charge Hold ile entegre, Rage+15 per hedef
8. **Deep Wound** — Rage+35, 8s Bleed DoT
9. **Death Blow** — Broken/Sundered hedef üzerinde, tüm Rage harca → %400 hasar (execute)

### Passive/Echo (3) — S44 active'dan çıkarıldı
10. **Iron Crush** — +%30 hasar 6s (Rare/Epic upgrade aday)
11. **Ironclad Momentum** — alınan hasar -%30 + her 10 hasar +10 Rage (defensive echo)
12. **Battle Surge** — Rage spend → HP +%5 (2s ICD, sustain boon)

## State system (Warblade-spesifik)
- **Broken** — Earthsplitter Hold uygular
- **Sundered** — Sunder Mark uygular
- **Death Blow** sadece bu state'ler üzerinde çalışır

## Demo Faz 1 Skill Draft havuzu (öncelik 6)
NLM verdict — bu 6 yetenek havuza girer:
1. **Iron Charge** — Room 1 garantili (signature)
2. **Earthsplitter** — Rage doldurma + Broken state
3. **Gravity Cleave** — Iron Charge sinerji + control
4. **Sunder Mark** — zırh kırma + priority targeting
5. **Death Blow** — Rage spend + execute fantezi
6. **Iron Counter** — Penitent Sovereign telegrafına karşı reaktif

## Fantezi disiplini
"Yaklaş → Sabitle → Zırh kır → İnfaz et" — 10 dk loop'ta öğretilmeli.

## Demo'ya GİRMEYEN
- Pasif/Echo (Iron Crush, Ironclad Momentum, Battle Surge) — Faz 1 milestone'da skill draft sadece active 6 sunar
- Cross-class proc/passive (Faz 4'te açılır)
- Rare/Epic/Legendary tier (Common only)

## Demo skill draft mekanik özeti
- Combat oda cleared → Map Fragment drop → pickup → **3 kart Hades-tarzı offer**
- 3 kart 6 havuzdan random (Iron Charge garanti Room 1, sonra random)
- Replace mode (4 active slot full ise hangi slot değiş)
- Skill draft UI mevcut: `SkillOfferUI.cs` (S108 LIVE)

## Cross-link
[[project-demo-phase1-milestone-lock]] [[map-fragment-canonical-spec]] [[gate-socket-canonical-spec]] [[canonical-character-roster-v2]]
