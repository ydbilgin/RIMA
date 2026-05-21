---
status: REFERENCE
date: 2026-05-21
purpose: NLM canonical batch — 5 focused design queries for orchestrator context
---

### Q1: Combined Items (9 Adet)
A: Faz 1'de 9 Combined Item var, 2 Component birleşiminden oluşur, her biri stat profili + sinerji efekti taşır. Karar #18 (Item System D) ile kilitlendi — ekipman slot yok, stat bloat yok. Faz 2'de Surge Catalyst + Arcane Bastion eklenerek toplam 11'e çıkıyor.

| # | Formul | Sonuc | Sinerji Efekti |
|---|---|---|---|
| A | Iron Shard + Blood Gem | Vampiric Blade | Overkill → full heal burst |
| B | Void Fragment + Shadow Veil | Phantom Weave | Dodge → void burst proc |
| C | Rift Stone + Soul Ember | Frenzy Core | Crit → 0.5s haste |
| D | Iron Shard + Chain Links | Warlord's Plate | Hit taken → +resource (Rage) |
| E | Iron Shard + Rift Stone | Rift Piercer | RiftMark stack basi armor ignore |
| F | Blood Gem + Soul Ember | Soul Tap | Kill → skill charge |
| G | Void Fragment + Rift Stone | Fracture Amp | RiftMark'li dustmana +20% bonus dmg |
| H | Shadow Veil + Soul Ember | Ghost Step | Dodge → phantom strike |
| I | Chain Links + Blood Gem | Iron Will | Skill kullan → brief shield burst |

LOCK status: HARD LOCK Karar #18
Source: NLM [0c1d196a], [1de3aa55]

---

### Q2: Wall Canonical Types per Act
A: Act 1 L3 wall set 8 class — top hero / bottom hero / side L / side R / corner / arch / pillar / collapsed stub. Her class'tan 3 variant = 24 sprite. "Cracked/mossy/sealed" duvar state'leri ayri sprite SET olarak uretilmez; L4 (Cave Moss, Cracked Rubble) + L6 (Cyan Rift accent) overlay'larla saglanir. Per-encounter material continuity: tum sub-room'lar ayni granit set, varyasyon sadece composition + density'de.

LOCK status: HARD LOCK Karar #150 + Karar #143
Source: NLM [441c5b25], [7e2f78ee]

---

### Q3: Floor Canonical Types per Act
A: Act 1 L1 = Cool Granite (16 tile Wang set, palette #3A3D42), L2 = Worn Stone Path (16 tile, #4A4842). Cracked + Cyan Rift sizmasi L4/L6 yamalari olarak uygulanir, ayri tile olarak uretilmez. Blood-stain Act 1'e ait DEGIL — Act 2 (Bleeding Wastes) materyali.

Karar #143 6-Layer ozet:
- L1: Floor base (create_tiles_pro, PURE top-down)
- L2: Floor variation (create_tiles_pro)
- L3: Wall overlay (create_object, fake-isometric depth)
- L4: Large patches — Cave Moss/Dust Drift/Cracked Rubble (create_map_object, 8 variant each)
- L5: Scatter — small stones/chain debris/skeleton fragments (create_object)
- L6: Accent — Cyan rift hero + Brazier + Banner (create_object)
Edge-biased density rule (143-E): wall yakini 10x, center 0.1x.

LOCK status: HARD LOCK Karar #143 + Karar #150
Source: NLM [988e6490], [441c5b25], [d2e1b7f1]

---

### Q4: Skill Bank V1 Current State
A: 48-skill bank Opus review DONE (OPUS_DONE_skill_bank_balance_review.md). Statü: NEEDS REVISION minor — 4 weakness, 5 polish ihtiyaci, Death Imprint gap, Warblade/Shadowblade trigger anomalisi. Tag reclassification 9->7+2 (Karar #65 update) revision listesinde ama henuz uygulanmadigi icin LOCK degil. Death Imprint passive one-liner'lar eksik — gap kayitli. Skill Draft 3-choice pool tablosu CANONICAL ve tam:

Offer tipi agirliklar:
- Slotlar bos + tier upgrade mevcut: %40 New / %40 Tier Up / %20 Echo
- Tum slotlar dolu: %10 New / %70 Tier Up / %20 Echo
- Hic tier upgrade yok: %60 New / — / %40 Echo

Class pool per Act: Act1 %100 Primary | Act2 erken %65P/%20S/%15N | Act2 gec %55/%30/%15 | Act3 %45/%45/%10

LOCK status: SOFT LOCK (pool table LOCK, bank revision PENDING)
Source: NLM [bbbc1fc9], [de3f1ff2]

---

### Q5: Death Imprint Spec Gate
A: Death Imprint PROPOSAL/CANDIDATE — locked spec YOK. "Epic mechanic candidate" olarak kayitli (proje_echo_imprint_cascade_signature_candidate.md). Etki: narrative hafiza + kosullu passive — oyuncu olum sirasinda encounterId + graph node + subRoomIndex + mob composition + mimari tag kaydedilir; ayni noktaya gelindiginde bu imzaya ozel Echo Imprint efekti tetiklenir. Codex prototype spec gate sart kostu, Opus spec yazimini Morning Sequence'de planladi ama henuz yazilmadi.

LOCK status: PROPOSAL (spec gate bekliyor, Opus design asamasinda)
Source: NLM [bbbc1fc9], [8d247aa0]

---

## CROSS-REFERENCE

Uyum: Tum 5 cevap birbiriyle tutarli. Celi§ki yok.

Dikkat noktalari:

1. Q4 <-> Q5 baglantisi: Skill Bank revision listesinde "Death Imprint gap" kayitli (Q4), ve Q5 da Death Imprint spec gate'in hala Opus yazimini bekledigini dogruluyor. Ikisi ayni eksigi gosteriyor — tutarli.

2. Q2 <-> Q3 entegrasyonu: Wall state'leri (cracked/mossy) L4 overlay ile saglanirken, floor state'leri (cracked/cyan-rift) de L4/L6 overlay ile saglanir. Her ikisi de ayri sprite set URETMEZ — pattern tutarli, 6-layer architecture ile uyumlu.

3. Q1 item system ile floor/wall ayni session: Karar #18 (item) ve Karar #143 (floor) farkli kararlar, herhangi bir cakisma yok.

4. Drift uyarisi: Blood-stain floor Act 2 materyali — Act 1 uretiminde drift riski var. Q3 bunu teyit ediyor (BANNED for Act 1). Herhangi bir Act 1 gen batch'inde blood-stain prompt kullanilmamali.

5. Tag reclassification 9->7+2 hala pending — Karar #65 henuz resmi olarak update edilmemis. Bu drift kaynagi olabilir eger tag sistemi referans alinirsa.
