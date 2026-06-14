# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Skills/DraftManager.cs`
HandleActivePick + OnReplaceChosen + FindNext*Slot + HasFree*Slot + FindSlotOf + IsSecondarySkill + skill controller slot layout'u READ et.

## Fix'ler
- BUG1: replace tetiği band-hedefli (HasFreePrimarySlot/HasFreeSecondarySlot).
- BUG2: OnReplaceChosen FindSlotOf<0 → güvenli abort (slot-0 clobber yerine).

## Derin sorular (slot state-machine / tutarlılık)
1. **Band modeli bütünlüğü:** primary(0-3) / secondary(4-5) band ayrımı + IsSecondarySkill ile bir skill'in hangi band'e gideceğinin belirlenmesi tutarlı mı? "Band dolu" tetiği ile FindNext* yerleştirmesi aynı invariant'ı paylaşıyor mu (mirror)? currentActiveSkills sayımı band-farkındalığı olmadan başka yerlerde (UI ShowReplaceMode) hâlâ kullanılıyor mu — tutarsızlık?
2. **BUG2 abort tasarımı:** Replace-mode'da kullanıcı bir slot seçti ama FindSlotOf bulamadı = invariant ihlali. Abort doğru kurtarma mı, yoksa bu durum hiç olmamalı mı (daha derin bir bug'ın semptomu)? toReplace neden bulunamayabilir?
3. **Regresyon:** ShowReplaceMode → OnReplaceChosen/OnSkipChosen akışı + normal (dolu-değil) yerleştirme + passive yolu sağlam mı?
4. Daha temiz/sağlam alternatif (over-engineering'e kaçmadan)?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (severity · dosya:satır · 1-cümle + öneri).
