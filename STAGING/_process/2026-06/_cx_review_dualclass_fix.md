ACTIVE RULES: (1) think before judging (2) evidence-based — diff'i kendin oku (3) surgical — SADECE git diff/dosya oku + en fazla BİR EditMode koşusu, başka HİÇBİR ŞEY değiştirme (4) emin değilsen UNCERTAIN.
NLM ACCESS: Gerekmez.

# Amaç — DUAL-CLASS FIX DOĞRULAMA REVIEW (writer=Claude Opus sub-agent; sen bağımsız reviewer'sın)
Önceki review (CODEX_DONE_yekta.md) dual-class butonunu 3 bulguyla FAIL'lemişti. Fix yapıldı. Fix raporu: `STAGING/_process/2026-06/_opus_fix_dualclass_button_2026-06-13.md` (önce oku). SADECE 3 bulgunun kapanıp kapanmadığını + regresyon olup olmadığını doğrula. Tek dosya diff: `Assets/Scripts/UI/DirectorMode.cs`.

## Doğrula (git diff, uncommitted):
1. **BULGU 1 (overlay layering):** Tetikte Director canvas root fiilen kapatılıyor mu (`SetOverlayVisible(false)` benzeri) → ClassSelectionUI topmost/clickable oluyor mu? Director root artık raycast bloke etmiyor mu? Eski `SetState(Test)`'e güvenmeyen bir yol mu?
2. **BULGU 2 (death guard):** `TriggerDualClassDraft()` başında `IsDead`/death-screen guard var mı, `SelectDirectorClass` (1873-1879) ile aynı semantik mi? Death aktifken seçim AÇILMIYOR mu?
3. **BULGU 3 (visibility event):** `OnSecondaryClassSelected` aboneliği LEAK'siz mi (abonelik idempotent + OnDisable/OnDestroy'da unhook simetrik)? Seçim sonrası buton gizleniyor mu?
4. **timeScale:** Director kapalı → ClassSelectionUI 0 sahibi → seçim → 1. Ekstra/çakışan timeScale yazımı EKLENMEMİŞ mi? Bugünkü death-guard/ResolveTimeScaleForState modeliyle çatışma yok mu?
5. **Regresyon:** Stat preset kodu (önceki review PASS) DEĞİŞMEMİŞ mi? Diff SADECE dual-class fix + DirectorMode.cs mi? Yeni event aboneliği başka bir akışı (panel refresh, tab switch) bozuyor mu?
6. **EditMode:** BİR KEZ koş (assembly RIMA.Tests.EditMode); fail ≤11 + isimler baseline'la uyumlu + DirectorMode-ilişkili yeni fail YOK.

## RAPOR → `CODEX_DONE_<kendi profilin>.md`
Madde başına PASS/FAIL/UNCERTAIN + kanıt (satır no). Sonda: GENEL VERDİKT + commit'e uygun mu.
