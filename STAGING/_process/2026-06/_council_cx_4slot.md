ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
P4 (4-slots-full replace-mode edge) fix'inin commit-öncesi feasibility/reuse/regresyon review'u — CODE lens.

ANALYSIS ONLY. Dosya değiştirme. Unity açma. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Skills/DraftManager.cs`
HandleActivePick + OnReplaceChosen + FindNextPrimarySlot/FindNextSecondarySlot + yeni HasFreePrimarySlot/HasFreeSecondarySlot + FindSlotOf + IsSecondarySkill metodlarını READ et.

Fix'ler:
- BUG1: replace-mode tetiği `currentActiveSkills.Count >= maxSlots` → band-hedefli `secondary ? !HasFreeSecondarySlot() : !HasFreePrimarySlot()`.
- BUG2: OnReplaceChosen'da `FindSlotOf<0` fallback FindNextPrimarySlot()→0 (slot-0 clobber) → güvenli abort (log, pendingSkill=null, Hide, IsDraftActive=false, return).

## Sub-questions (CODE/reuse lens)
1. **Mirror doğruluğu (KRİTİK):** HasFreePrimarySlot host-çözümü + slot aralığı (0-3) FindNextPrimarySlot ile BİREBİR aynı mı? HasFreeSecondarySlot (4-5, SecondaryUnlocked) FindNextSecondarySlot ile aynı mı? "Band dolu mu?" ile "nereye düşer?" aynı slot kümesini görmeli — uyuşmazlık (off-by-one, host farkı, UseElementalistPrimary) var mı?
2. **Normal vaka:** 4 primary dolu + primary pick → replace-mode HÂLÂ tetikleniyor mu? Yarı-dolu primary + primary pick → replace DEĞİL, boş slot mu (yani BUG1 fix yanlış-pozitifi gerçekten kaldırdı mı)?
3. **BUG2 abort:** FindSlotOf<0 normal akışta erişilebilir mi (toReplace currentActiveSkills'ten geliyor)? Abort pick'i atıyor — kabul edilebilir mi yoksa geçerli bir slot seçmeli mi? Abort state'i temiz bırakıyor mu (IsDraftActive, pendingSkill, offerUI)?
4. **Reuse/regresyon:** HasFree* mantığı mevcut bir helper'la sağlanabilir miydi? Normal replace/skip/passive yolu etkilendi mi? skillController==null dalı (HasFreePrimarySlot true döner) AssignActive ile tutarlı mı?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 yüksek-güven bulgu: severity · dosya:satır · 1-cümle (+fix). #1 (mirror) için NET yargı.
