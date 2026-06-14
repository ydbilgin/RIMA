ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Phase-1 skill-reward fix'inin (uncommitted) commit-öncesi feasibility/reuse/regresyon review'u — CODE lens.

ANALYSIS ONLY. Hiçbir dosyayı değiştirme. Unity'yi açma/sürme (başka akış sahibi). Sonucu CODEX_DONE.md'ye yaz. Önceki bir audit'i tekrarlama.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Uncommitted working-tree değişikliği, 2 dosya. Diff'i şununla al:
`git diff Assets/Scripts/Skills/DraftManager.cs Assets/Scripts/Core/ChestBehavior.cs`
Sonra ilgili TAM metodları + çağıran/kapatan yolları READ et (sadece diff yetmez).

Fix'ler:
- FIX1 = `DraftManager.AssignActive`: skillType==null ise erken return + Debug.LogWarning (eskiden dead entry ekliyordu).
- FIX3 = `DraftManager.ShowDraftWithSkill`: `if (IsDraftActive || IsDraftPending) return;` + `EnsureDependencies()` + offerUI null log + `IsDraftActive = true;`.
- FIX2 = `ChestBehavior.BuildChestOffers`: `GetAll()` yerine inline `isImplemented` filtreli liste.

## Sub-questions (CODE/feasibility/reuse lens)
1. **FIX3 IsDraftActive LEAK:** ShowDraftWithSkill `IsDraftActive=true` yapıyor. Ortak tamamlanma/iptal yolu (OnOfferSelected, draft-close, cancel) bu bayrağı skill SEÇİLİNCE **ve** SKIP/iptal edilince false'a resetliyor mu? Reset noktasını Grep ile bul (`IsDraftActive` yazımları) ve NET evet/hayır + dosya:satır ver. Resetlenmiyorsa = kalıcı kilit = BLOCKER.
2. **FIX1 erken return yan-etki:** AssignActive'in çağıranını (HandleActivePick/OnOfferSelected) izle. Erken return; UI kapatma / draft-complete event / slot sayacı gibi gereken bir yan-etkiyi atlayıp draft'ı açık veya sayacı tutarsız bırakıyor mu?
3. **FIX2 reuse:** Inline isImplemented filtresi yerine zaten var olan bir helper (`GetPool`, filtre predicate) kullanılmalı mıydı? GetPool chest'in de uyması gereken EK filtre (class kısıtı) uyguluyor mu, yoksa "tüm implemented" chest için doğru mu?
4. **Genel regresyon:** normal (chest-dışı) ShowDraft yolu etkilendi mi? null-deref?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 yüksek-güven bulgu: severity · dosya:satır · 1-cümle (+fix). #1 ve #2 için NET evet/hayır zorunlu.
