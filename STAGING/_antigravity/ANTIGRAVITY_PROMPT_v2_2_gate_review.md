# Antigravity Prompt — v2.2 Progression Gate Third-Opinion Review

**Kullanım:** Aşağıdaki code-fenced bloğu Antigravity IDE'ye olduğu gibi yapıştır.

**Bağlam:** v2.2 progression plan'da 6 açık karar gate'i + 7 Codex logic conflict var. Orchestrator (Claude Opus) ve Codex (GPT-5.5) review tamamlandı, ikisinin önerileri yer yer çakışıyor. Antigravity (Gemini 3.5 Flash) ÜÇÜNCÜ göz olarak canonical kaynakları okuyup bağımsız değerlendirme verecek.

---

```
ACTIVE RULES: (1) canonical evidence quote zorunlu (2) min spec, no speculation (3) "by design vs oversight" ayır (4) BLOCKED if unclear.

# Görev — RIMA Progression v2.2 Third-Opinion Gate Review

Orchestrator (Claude Opus) + Codex (GPT-5.5) reviewlar tamam. Çakışan öneriler var. Senden ÜÇÜNCÜ göz canonical-evidence-based değerlendirme istiyoruz.

## Read These Files (project root: F:/Antigravity Projeler/2d roguelite/RIMA)

### Plan Dökümanları (en güncel state)
- STAGING/_plans/progression/PROGRESSION_PLAN_v2_2_LOCK.md
- STAGING/_plans/progression/PROGRESSION_PLAN_v2_1_REVIEW.md
- STAGING/_plans/progression/CODEX_REVIEW_v2_2_GATES.md
- STAGING/_plans/progression/GATE_RESOLUTIONS_v2_2_PROPOSAL.md

### TASARIM Canonical (canonical source of truth)
- TASARIM/MAP_ITEM_SYSTEM.md
- TASARIM/ROOM_MECHANICS.md
- TASARIM/GDD.md
- TASARIM/map_fragment_system.md
- TASARIM/CROSS_CLASS_PROC_SYSTEM.md
- TASARIM/SUBROOM_TEMPLATES_ACT1.md

### Memory (locked decisions)
- memory/project_progression_canonical_lock.md
- memory/project_rima_style_manifesto.md
- memory/project_canonical_character_roster_v2.md

### Skill Data
- STAGING/concepts/skill_sheets_v3/skill_enumeration_v3.json (115 skill, 10 class)

## HARD Filozofi Constraint

1. Basit tut, scope dar
2. Her run farklı hissi ZORUNLU
3. Mekanik şişirme YASAK
4. Demo önce, expansion post-launch

Her önerinde bu 4 filtreden geçmiş olmalı. Filozofiyi bozan öneri = otomatik FAIL.

## 6 Açık Karar Gate

### Gate 1 — Forge Act 1'de olsun mu?
Çelişen iki canonical kaynak:
- Karar #62: "Act 1 = 8 oda tipi (Entry/Combat/Elite/Rest/Shop/Curse Gate/Mystery/Boss), Forge YOK"
- MAP_ITEM_SYSTEM.md: "Node ~4 = Forge Room (guaranteed, sabit)"

Forge nedir: Oyun-içi craft odası. 2 Component birleşince 1 Combined Item (örn. Iron Shard + Blood Gem = Vampiric Blade). 9 canonical recipe var.

Seçenekler:
- **A1)** Karar #62 reopen. Mystery B02 branch node → Forge ile değiştir. Act 1'de Combined craft aktif öğretilir. Topology değişir AMA player Act 1'de craft cycle deneyimler.
- **A2)** Karar #62 LOCK kal. Act 1 = Component collection only, Combined craft Act 2'den itibaren. MAP_ITEM_SYSTEM Phase 1 için explicit "Act 1 no Forge by design" notu eklenir.

Senden: Hangisi + canonical evidence + RIMA filozofi check.

### Gate 2 — Echo Imprint Trigger
Canonical (ROOM_MECHANICS.md): "Her 3 combat oda sonrası normal Skill Draft'a EK olarak sunulur" + "Max 4/run, act başına 1 slot açılır".

Conflict: Act 1 = 6 Combat → 2 trigger ama 1 slot/Act cap. Bir fazla trigger ne olacak?

Seçenekler:
- **B1)** Codex önerisi: canonical "her 3 combat" kuralı korunur, trigger gelmeye devam eder AMA o Act'ın 1 slot dolduysa Echo Imprint kartı ekstra Skill Draft seçenek olarak sunulur, yeni slot AÇMAZ. Act 1 implementation note: "Locked topology'de bu ilk Elite'e denk gelir."
- **B2)** Orchestrator eski: "First Elite of each Act" global rule (Act 1 N03 → trigger 1, Act 2 N03 → trigger 2, vb). Predictable, "her 3 combat" sub-system silinir. AMA sonraki Act'larda first Elite ≠ 3rd combat olabilir, rule kırılır.

Senden: Hangisi + canonical evidence + filozofi check.

### Gate 3 — Architect Break Ending NPC Removal
GDD Section 16 (Codex NLM'den buldu): "BREAK ending seçilirse Hub'dan bazı NPC'ler KALICI silinir (save dosyası yazılır)".

Bu mechanical consequence — Stay/Break/Carry pure narrative değil.

Seçenekler:
- **C1)** Defer — Phase 1 demo'da BREAK visual/text only. NPC permanent removal post-launch'a ertelenir. GDD spec korunur ama implementation defer notu eklenir.
- **C2)** Implement — gerçek consequence Phase 1'de. Hardcore commitment, demo'da risk + balance + UI ekstra iş.
- **C3)** Cancel — GDD'den NPC removal kuralı kaldır, 3 ending tamamen narrative. Canonical override gerekir.

Senden: Hangisi + GDD canonical quote + Phase 1 scope filozofi check.

### Gate 4 — Hub Class Unlock
Codex NLM canonical buldu:
- Warblade = tek starting class (0 cost)
- Elementalist, Ranger = 80 Echoes
- Shadowblade = 150 Echoes OR Act 1 3 clear milestone
- Ravager, Ronin = 150 Echoes + milestone alternative
- Gunslinger, Brawler, Summoner = ~200 Echoes + milestone (Brawler/Hexer cost conflict canonical sources arasında)
- Hexer = 250 Echoes + "Elementalist ile 1 run yap" precondition

Earn rate: Boss kill +10, Act complete +5 → run ortalama 30-40 Echoes.

Seçenekler:
- **D1)** Canonical kabul: Warblade-only start, canonical unlock economy. Phase 1 hub = Warblade + 1 NPC "Echo Keeper" + class unlock UI.
- **D2)** Canonical override: 4 starting class (Warblade + Elementalist + Ranger + Shadowblade — Karar #150 playtest priority ile uyumlu). Custom economy. Diğer 6 class meta-unlock.

Senden: Hangisi + canonical evidence + Karar #150 ile çelişki var mı + economy pace verdict.

### Gate 5 — Image 13 Stay/Break/Carry Bottom Band
STAGING/concepts/overnight/13_all_acts_master_flow.png alt bantta Stay/Break/Carry ikonları var. Orchestrator başlangıçta "run-başı meta-track" yanılgısı yaşadı. Aslında Architect ending choice.

Seçenekler:
- **E1)** Bottom band sil, Architect boss kartının ALTINA compact "Ending Choice: Stay / Break / Carry" tek-satır strip. Manual edit, gen yok.
- **E2)** Bottom band ikonlarını Act 4 row'una sağa taşı. Codex placement riski WARN dedi — Act 4 row zaten Tier IV card + info card ile kalabalık.

Senden: Hangisi + Act 4 row visual capacity yorumun + manual edit feasibility.

### Gate 6 — Demo Scope (Class Sayısı)
Mevcut skill dağılımı (skill_enumeration_v3.json):
- Ronin: 4 skill (az)
- Gunslinger/Ravager/Hexer/Brawler/Summoner: 8 skill (orta-dar)
- Warblade: 14 / Elementalist: 15 / Ranger: 20 / Shadowblade: 22 (zengin)

Karar #150 LOCK: "playtest priority = Warblade + Elementalist + Ranger + Shadowblade" (4 class).

Seçenekler:
- **F1)** Demo'da sadece 4 class (Karar #150 priority). Diğer 6 class post-demo skill expansion (her birini 12-15 skill'e çıkar). Demo solid + build variety bol.
- **F2)** 10 class demo. Az-skill class'lar dar build hisseder. Balance riski.
- **F3)** 4 class demo, demo başarılıysa 6 class skill expansion + full release.

Senden: Hangisi + Karar #150 alignment + demo scope filozofi check.

## 7 Codex Logic Conflict Validation

Codex bunları teknik issue olarak işaretledi. Senden: her birini AGREE / DISAGREE + reasoning.

1. **Skill slot 4→6 draft routing:** Act 1 başı 4 active slot, 2 visible-locked. Locked slot'lar Skill Draft pool'da görünür mü? Player offer alıp slot dolduramaz mı?
   - Codex fix: "Visible-locked = UI only. Draft cannot place. 4 slot full → New Skill draft replace flow VEYA suppress (weight table)."

2. **Cross-class proc Act 1:** CrossClassProcManager LIVE/LOCKED. Ama Act 1'de secondary class yok → proc neye trigger olur?
   - Codex fix: "CrossClassProcManager disabled until secondary class selected (Act 1 boss kill sonrası). Act 1 tooltip 'locked until second face recovered'."

3. **Pity counter source:** "Skill absent 5 consecutive drafts → 80% next draft". Elite Reward "normal Draft yerine" diyor → Elite Reward pity sayar mı?
   - Codex fix: "Pity sadece normal 3-choice Skill Draft'ı sayar. Elite Reward + Boss reward sayılmaz."

4. **Mystery branch fragment HUD:** Mandatory 8 + bonus Mystery fragment = total 9/8 confusing.
   - Codex fix: "Mandatory cap 8/8, bonus fragment ayrı '+1 bonus reveal' badge gösterilsin."

5. **Curse Gate reward path:** 3 canonical conflicting path (Burden/Gift, Legendary shortcut, bonus reveal).
   - Codex fix: "Phase 1 için 1 lane seç, 3'ü hepsi YOK by default. Reject = +5% HP only."

6. **Forge/Anvil pricing semantic:** Forge = no gold + 1 main action / Anvil = gold + rare + same 1 action.
   - Codex fix: "Explicit yaz. If Act 1 no Forge, Combined craft intentionally rare."

7. **Boss Legendary 3-choice:** v2.2 "class-specific Legendary 3-choice", MAP_ITEM_SYSTEM "each class has 3 Legendary options" → 3 anchor'ı sun (random değil).
   - Codex fix: "Current primary class's 3 canonical Legendary anchors offered. Random only if class >3."

## Bonus — Senin Bulduğun Mantıksızlıklar

Plan + canonical dosyaları okuduktan sonra, orchestrator + Codex'in gözünden kaçan ek mantıksızlık / drift / inconsistency varsa raporla. Severity (CRITICAL/WARN/INFO) + evidence + fix önerisi.

## Output Format

`STAGING/_plans/progression/ANTIGRAVITY_REVIEW_v2_2_GATES.md` (UnityMCP file_write):

```markdown
# Antigravity Third-Opinion Review — v2.2 Progression Gates

## Section 1 — 6 Gate Verdicts

### Gate 1 (Forge)
**Verdict:** [A1 / A2]
**Canonical evidence:** [TASARIM quote + file:line]
**Filozofi check:** [4 madde ile uyum]
**Risk:** [varsa]

[Gate 2-6 aynı format]

## Section 2 — 7 Codex Logic Conflict Validation

### Conflict 1 (Skill slot 4→6)
**Stance:** [AGREE / DISAGREE / PARTIAL]
**Reasoning:** [canonical evidence + production logic]

[Conflict 2-7 aynı format]

## Section 3 — Antigravity'nin Bulduğu Ek Mantıksızlık

[Orchestrator+Codex'in kaçırdığı her madde için: Severity + evidence + fix]

## Section 4 — Final Recommendation

| Priority | Action | Rationale |
|---|---|---|
| 1 | [hangi karar önce çözülmeli] | [why] |
| 2 | ... | ... |

## Section 5 — Open Questions for Orchestrator/User

[Antigravity'nin de cevaplayamadığı, user'a kalan sorular]
```

## Kısıt

- Canonical evidence quote ZORUNLU her verdict için
- "Bence" / "sanırım" speculation YASAK
- Mevcut plan'ı re-write etme — sadece review + patch list
- Filozofi compliance her madde için kontrol et
- "Phase 1 demo first" prensibinden sapma = otomatik WARN

## Effort
high
```

---

**Notlar (senin için, Antigravity'e değil):**

- Bu prompt'u Antigravity'de Gemini 3.5 Flash'a yapıştır. UnityMCP file_read ile dosyaları kendi okur, proje tanıtımı yapmaya gerek yok.
- Gemini 3.5 Flash long context iyi — tüm canonical + plan dosyalarını tek pass'te okuyabilmeli.
- Output `STAGING/_plans/progression/ANTIGRAVITY_REVIEW_v2_2_GATES.md` olarak yazılırsa, orchestrator + Codex review ile yan-yana karşılaştırırız.
- 3-way verdict (Orchestrator + Codex + Antigravity) ile her gate için **majority recommendation** çıkar.
