# Codex Task — v2.2 LOCK + Gate Resolution Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```
TASARIM/ direct read also acceptable (NLM CLI sub-agent profilinde uçabilir).

---

## Görev

Orchestrator v2.2 LOCK draft yazdı + 5 açık karar gate için resolution önerdi. Bunları üçüncü göz olarak review et. Filozofi: **basit tut, scope dar, her run farklı hissi, mekanik şişirme YOK**.

## Girdi Dosyalar

1. `STAGING/_plans/progression/PROGRESSION_PLAN_v2_2_LOCK.md` — yeni LOCK draft
2. `STAGING/_plans/progression/GATE_RESOLUTIONS_v2_2_PROPOSAL.md` — orchestrator 5 gate resolution + rationale
3. `STAGING/_plans/progression/PROGRESSION_PLAN_v2_1_REVIEW.md` — v2.1 Codex review (önceki bulgular)
4. TASARIM canonical (NLM ile sorgula veya direct read):
   - MAP_ITEM_SYSTEM.md (item ekonomisi, Forge, Combined recipe)
   - ROOM_MECHANICS.md (Echo Imprint, Skill Draft, Cross-class)
   - GDD.md (run timeline, Stay/Break/Carry ending)
   - CROSS_CLASS_PROC_SYSTEM.md (proc detail)
   - SUBROOM_TEMPLATES_ACT1.md (sub-room spec)
5. `memory/project_progression_canonical_lock.md` (Karar #60/61/62/63)
6. `memory/project_rima_style_manifesto.md` (sentez stil)

---

## Review Et — 5 Gate

### Gate 1 — Forge Node Conflict
Orchestrator pick: **(d) Forge feature Shop Anvil içinde Act 1, dedicated Forge node Act 2+ only**.

**Sor:**
- TASARIM Forge canonical olarak destiny node mu, yoksa Shop sub-feature mı? Karar referansı var mı?
- Act 1'de Combined Item nadir görünmek RIMA player experience hedefiyle uyumlu mu?
- Shop Anvil mevcut spec'inde "rare/expensive safety valve" deniliyor — orchestrator Phase 1 için bunu Act 1 main craft path yapmayı öneriyor. Mevcut Shop probability tablosu (Anvil 200G %30) bu rolü kaldırır mı?
- Alternative önerisi var mı?

### Gate 2 — Echo Imprint Trigger Math
Orchestrator pick: **(d) Trigger = "after first Elite of each Act"** (Act 1 N03 ilk Elite sonrası).

**Sor:**
- TASARIM canonical Echo Imprint trigger NLM ne diyor? "Every 3 combat rooms" canonical mı, yoksa orchestrator yorumu mu?
- "First Elite of Act" rule conflicting canonical karar var mı?
- Player experience: predictable trigger boring olur mu, vs "her 3 room" random hissi daha mı iyi?
- Alternative trigger önerisi (örn. "Combat run-streak 3 sonu" / "her boss-1 sonra" / "macro encounter 3 sonra")?

### Gate 3 — Architect Meta-Unlock
Orchestrator pick: **(d) Story-only first kill, Stay/Break/Carry 3 ending = replayability, mechanical meta-unlock Phase 2+ patch content**.

**Sor:**
- Stay/Break/Carry 3 ending'in TASARIM canonical detayı var mı (GDD section 16)? Her ending farklı mekanik unlock mu, sadece narrative mı?
- "Architect kill = story ending only" Phase 1 scope için yeterli motivation mı?
- "Phase 2+ post-launch yeni class unlock" pattern çoğu roguelite'da var (Hades Skelly, Dead Cells DLC) — Phase 2 patch için yeterli mı?
- Alternative: Architect kill her seferinde +X Echoes bonus + birinci kill story unlock (hibrit)?

### Gate 4 — Hub Spend Catalog
Orchestrator pick: **(e) Phase 1 = class unlock only (4→10), Phase 2+ = pre-pick Imprint / cosmetic / run modifier**.

**Sor:**
- TASARIM canonical hub spend yapı var mı, yoksa açık mı?
- Class unlock economy: orchestrator 100/150/200/250/300/400 Echoes önerdi. Earn rate (Boss kill +10, Act complete +5 → run ~30-40 Echoes) ile 100 Echoes = 3-4 run. Bu pace OK mı, çok hızlı / çok yavaş mı?
- 4 starting class hangileri? Warblade/Elementalist/Ranger/Shadowblade canonical mı (orchestrator memory'den çekti, doğrulanmalı)?
- Alternative: tüm 10 class start unlocked, hub Phase 1 = sadece cosmetic + small QoL? Veya Hades Mirror-style mini meta-passive (3-5 node)?

### Gate 5 — Image 13 Handling
Orchestrator pick: **(b) Relabel, no regen — alt bant sil, S/B/C Act 4 row sağına "Ending Choice" etiketi**.

**Sor:**
- Codex image-edit (relabel) kalite OK mı, vs full regenerate?
- Image 13 dışında S/B/C içeren başka render var mı (compact_sheets/, threshold_gallery/)?
- Master flow Act 4 row layout S/B/C ikonlarını absorb edebilir mi space-wise?

---

## Diğer Mantıksızlık Taraması (orchestrator gözünden kaçan)

1. **Skill slot 4→6 progression:** Act 1 başı 4 active, 2 visible-locked. Locked slot'lar Skill Draft pool'da görünür mü? Player offer alır da slot dolduramaz mı? CHECK.

2. **Cross-class proc Act 1:** Cross-class system Act 2 başında aktif. Ama CROSS_CLASS_PROC_SYSTEM.md "LIVE, LOCKED" diyor. Act 1'de proc trigger olur mu (secondary class yok), yoksa Act 2'de mi başlar? CHECK.

3. **Pity system Act 1 math:** "Skill absent 5 consecutive drafts → 80% next draft". Act 1 = 6 Combat + 2 Elite = 8 fragment-bearing node = 8 Skill Draft (Elite'lar fragment veriyor + Skill Draft). 5 consecutive absent tetiklenir mi gerçekten? Edge case kontrol.

4. **Mystery branch fragment economy:** Mystery (B02) chance-based fragment veriyor (bonus). Player Mystery node'a girip fragment alırsa boss gate 9 fragment ile gider — mandatory 8'i geçer. UI nasıl gösterir? Counter "8/8" duraklasın mı, "9/8" gösterilsin mi?

5. **Curse Gate (B01) reward path:** Curse Gate "burden/gift" risk-reward branch. Kabul ederse ne kazanır (Component / Combined / Relic / Echoes)? Reject ederse +5% HP. Mevcut TASARIM canonical detay var mı? CHECK.

6. **Combined Item Forge cost:** Combined craft Forge'da "1 main action per visit". Cost gold/component/free? Free ise neden Anvil pahalı (200G %30)? Pricing logic check.

7. **Boss Legendary choice 3'ten** seçim: TASARIM canonical "class-specific Legendary" diyor. Class Legendary pool kaç skill, 3-choice nasıl random seçim?

---

## Çıktı

`STAGING/_plans/progression/CODEX_REVIEW_v2_2_GATES.md`:

```
# v2.2 Gate Resolution Review

## Section 1 — Per-Gate Verdicts (5 gate)
[Her gate için: AGREE / PARTIAL AGREE / DISAGREE + reason, canonical evidence, alternative]

## Section 2 — Additional Logic Conflicts (7 madde)
[Orchestrator'dan kaçan, her biri için: severity (CRITICAL/WARN/INFO) + fix önerisi]

## Section 3 — Filozofi Compliance Audit
[Her resolution "basit tut, scope dar, mekanik şişirme yok" filtresinden geçer mi?]

## Section 4 — Recommended v2.3 Patch List
[Gate'ler resolve edildikten sonra v2.2 → v2.3 LOCK için patch list, explicit before/after]

## Section 5 — Open Questions (user'a kalan)
[Codex resolve edemeyenler]
```

## Kısıt

- Sub-agent context koruma: NLM ile sorgula, direct file read minimal
- v2.2 LOCK'u sıfırdan yazma — sadece review ve patch list
- Orchestrator resolution'ı blindly approve etme — canonical evidence göster
- "Bu da var" speculation YASAK — sadece TASARIM/Memory canonical'a dayan
- Filozofi vurgu: **basit, scope dar, replayability variation kanallarından, mekanik şişirme YOK**

## Effort
high
