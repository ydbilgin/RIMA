# v2.2 Open Gate — Orchestrator Resolution Proposals

**Filozofi:** Basit tut, her run farklı hissi şart, mekanik şişirme YOK.

User asked orchestrator to evaluate all 5 open gates with rationale + dispatch Codex for review.

---

## Gate 1 — Forge Node Conflict

**Problem:** MAP_ITEM_SYSTEM.md Forge = guaranteed "destiny node", Karar #62 Act 1 15-node table'da YOK. Combined Item Act 1 craft path belirsiz.

**Options:**
| # | Option | Cost | Phil-fit |
|---:|---|---|---|
| a | Forge node Act 1'e ekle | Topology değişir, Karar #62 break | FAIL |
| b | Confirm Act 1 NO Forge — sadece Shop Anvil rare | Combined Act 1 nadir görünür | WARN |
| c | Rest node multi-purpose (opsiyonel forge) | Rest semantik bozulur | WARN |
| **d** | **Forge feature Shop içinde (Anvil expanded), dedicated Forge node Act 2+ only** | **Karar #62 korunur, Shop genişler, progression rhythm yaratır** | **PASS** |

**Orchestrator pick: (d).**
- Act 1 = Component collection + Shop Anvil basit craft (limited recipes mı, tam mı — Codex review etsin)
- Act 1 Boss kill → Cross-class unlock + (yeni) Act 2 Forge dedicated node access
- Act 2+ = full Forge progression
- Karar #62 hiç değişmez (Act 1 8 node type LOCK)
- Shop Anvil semantik: "emergency craft", 1 visit / 1 action, pahalı
- Forge node Act 2+: free, optimal, Combined+Legendary path

**Pro:** Progression rhythm doğal — Act 1 toplama, Act 2 crafting opens up. User filozofi "basit" korunur.

**Risk:** Act 1'de Combined Item neredeyse hiç görünmez. Bunun OK mı, oversight mı — Codex'e sor.

---

## Gate 2 — Echo Imprint Trigger vs Slot Math

**Problem:** "Every 3 combat rooms" + "1 slot per act, max 4/run" + Act 1 = 6 combat → 2 trigger ama 1 slot cap. Math conflict.

**Options:**
| # | Option | Cost | Phil-fit |
|---:|---|---|---|
| a | Trigger Skill Draft OFFER yapar, slot cap controls allocation | Confusing — 2. trigger boş offer | WARN |
| b | Trigger frequency düşür "every 6 combat" → 1/Act | Predictable ama "her 3" yapısı kaybolur | PASS |
| c | Act 1 2 slot izin ver (5 total, cap 4) | Cap break, math hala bozuk | FAIL |
| **d** | **Trigger = "after first Elite of each Act"** | **Predictable, 1/Act garanti, simpler counter** | **PASS** |

**Orchestrator pick: (d).**
- Act 1: N03 (ilk Elite) clear sonrası → Echo Imprint trigger as Skill Draft option
- Act 2: ilk Elite → trigger 2
- Act 3: ilk Elite → trigger 3
- Act 4: tek Elite varsa → trigger 4
- Toplam = 4 trigger = 4 slot cap (tam denk)
- Counter yok, milestone-driven
- Predictable: player "Elite'i geçtim, sırada Echo Imprint" diye bilir

**Pro:** Math conflict yok. Milestone existing system'e bağlı. "Every 3 rooms" counter sub-systemini siler — basitleşir.

**Risk:** Echo Imprint timing rigid (her zaman aynı node). Random hissi azalır — ama 4 kategori × 4 act = 16 farklı Imprint kombinasyonu zaten variety sağlıyor.

---

## Gate 3 — Architect Meta-Unlock

**Problem:** Act 4 The Architect yenilince ne unlock olur — yeni class / keepsake / hub feature / story-only?

**Options:**
| # | Option | Cost | Phil-fit |
|---:|---|---|---|
| a | New class (11. class) | Major content, design+art | FAIL Phase 1 |
| b | Keepsake item (yeni run-start slot system) | Yeni system, complexity | WARN |
| c | Hub feature (NPC/economy lane) | Open-ended scope | WARN |
| **d** | **Story-only first kill + Stay/Break/Carry 3 ending + ekstra meta-unlock Phase 2+ patch content** | **Simplest, replayability 3 ending üzerinden** | **PASS** |

**Orchestrator pick: (d).**
- İlk Architect kill → ending sequence (Stay/Break/Carry seçim)
- 3 ending = 3 farklı epilogue → replayability (oyun "bitirip 3 ending görme" goal'u doğal)
- Mechanical unlock şu an YOK
- Phase 2+ patch content: yeni class / keepsake / hub feature post-launch
- Shattered Echoes 50-75 bonus = canonical, ekstra "büyük bonus" yok

**Pro:** Filozofi "basit tut" tam uyumlu. Phase 1 scope dar. 3 ending'i tek başına replayability.

**Risk:** Hard-core players "ending'i 3 kez gördüm, sonra ne?" diye sorabilir → cevap: yeni class/keepsake post-launch içerik.

---

## Gate 4 — Shattered Echoes Hub Spend Catalog

**Problem:** Phase 4+ hub spend ne — class unlock / starting relic / cosmetic / meta-passive tree?

**Options:**
| # | Option | Cost | Phil-fit |
|---:|---|---|---|
| a | Hades Mirror-style meta-passive tree | Yeni big system | FAIL |
| b | Class unlock only (lock 6, unlock with Echoes) | Clear long-term goal | PASS |
| c | Starting relic per run | Yeni system + balance | WARN |
| d | Cosmetic only | Mechanical impact yok, motivasyon zayıf | WARN |
| **e** | **Phase 1 = class unlock only. Phase 2+ = starting Echo Imprint pre-pick / cosmetics / run modifiers patch content** | **Minimal Phase 1 + scalable** | **PASS** |

**Orchestrator pick: (e).**
- **Phase 1 hub:** Sadece class unlock. Başlangıçta 4 class (Warblade/Elementalist/Ranger/Shadowblade — Karar #?). Diğer 6 class: 100/150/200/250/300/400 Shattered Echoes ile sıralı unlock.
- **Phase 2+ patch content:** Starting Echo Imprint pre-pick (sonraki run 1 Imprint hazır), cosmetic skin, run modifier (challenge mode).
- Hub Phase 1 = 1 NPC ("Echo Keeper") + class unlock UI. Basit.

**Pro:** Phase 1 minimum scope, long-term progression visible (4→10 class). User filozofi tam uyumlu.

**Risk:** Class unlock economy balance gerek (100 Echoes = ne kadar run?). Codex'e sor.

---

## Gate 5 — Image 13 Handling

**Problem:** Stay/Break/Carry image 13 alt bantta, run-başı meta-track yanılgısı yaratıyor.

**Options:**
| # | Option | Cost | Phil-fit |
|---:|---|---|---|
| a | Regenerate full image | 1 Codex imagegen gen | WARN cost |
| **b** | **Relabel — alt bantı sil, S/B/C ikonlarını Act 4 row sağına taşı + "Ending Choice" etiketi** | **0 gen, küçük edit** | **PASS** |

**Orchestrator pick: (b).**
- Image 13 master flow korunur
- Bottom band silinir veya "Ending Choice" diye yeniden etiketlenir
- Act 4 row'a S/B/C ikonları taşınır
- Photoshop / Codex small edit task — 1 küçük dispatch

**Pro:** Gen budget korunur. Visual semantik düzelir.

**Risk:** Codex image-edit kalitesi düşük olabilir → regenerate plan B.

---

## Orchestrator Summary

| Gate | Pick | Rationale Özet |
|---|---|---|
| 1 Forge | (d) Shop Anvil Act 1 + Forge node Act 2+ | Karar #62 korunur, progression rhythm |
| 2 Echo Imprint | (d) First Elite trigger per Act | Math fix, milestone-driven |
| 3 Architect | (d) Story-only first kill, mech-unlock Phase 2+ | Filozofi basit tut |
| 4 Hub spend | (e) Class unlock only Phase 1 | Minimum scope, long-term goal |
| 5 Image 13 | (b) Relabel, no regen | Gen budget korunur |

**Filozofi check:** Tüm pick'ler "basit tut, scope dar, Phase 2+ content patch" prensibine uyumlu. Cross-system şişirme yok.

**Codex review için sorular:**
1. Gate 1 (d) — Act 1'de Combined Item nadir görünmek OK mi? Player run feel'i monoton mu olur?
2. Gate 2 (d) — "First Elite trigger" rule conflicting karar var mı NLM canonical?
3. Gate 3 (d) — Stay/Break/Carry 3 ending replayability single-kill için yeterli mi?
4. Gate 4 (e) — Class unlock economy (100-400 Echoes) Phase 1 run sayısı tahmini ile uyumlu mu? (Boss kill +10, Act complete +5 → run başına ~30-40 Echoes ortalama)
5. Gate 5 (b) — Image 13 relabel Codex image-edit ile kaliteli yapılabilir mi?

**Diğer mantıksızlık taraması:**
- v2.2 LOCK Section 1-5 tarafsız 3. göz review et — orchestrator gözünden kaçan conflict / contradiction var mı?
- Skill slot 4→6 progression Act 1 başında 4 active, 2 visible-locked: locked slot'lar Skill Draft pool'da görünüyor mu?
- Cross-class proc system LIVE — Act 1'de henüz secondary yok, proc nasıl tetiklenir?
- Pity system "skill absent 5 consecutive drafts → 80%" Act 1'de 6 draft var (6 combat) → pity Act içinde gerçekten tetiklenir mi?

---

## Codex Review Talep

Output: `STAGING/_plans/progression/CODEX_REVIEW_v2_2_GATES.md`

Format:
1. Per-gate verdict (AGREE / PARTIAL / DISAGREE + reason)
2. Alternative önerisi (gate başına)
3. Diğer mantıksızlık tespiti (orchestrator gözünden kaçan)
4. Final recommendation (gate'ler resolve edildikten sonra v2.2 → v2.3 LOCK için patch list)
