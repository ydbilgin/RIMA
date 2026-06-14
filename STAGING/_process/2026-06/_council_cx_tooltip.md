ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
P2 skill-bar tooltip fix'inin (uncommitted) commit-öncesi feasibility/reuse/regresyon review'u — CODE lens.

ANALYSIS ONLY. Dosya değiştirme. Unity'yi açma/sürme. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/UI/SkillBarUI.cs Assets/Scripts/UI/TooltipSystem.cs`
İlgili metodları + BasicAttackProfile alanlarını READ et.

Fix'ler:
- FIX A (SkillBarUI.ShowSlotTooltip + yeni BuildBasicAttackTooltip): basic slot 0/1 artık BasicAttackProfile'dan tooltip kuruyor (slot0=lmbName + comboDamage[0]/projectileDamage; slot1=rmbName + rmbDamage/rmbCost/rmbCooldown). Skill-slot guard `description`→`skillName` gevşetildi.
- FIX B (TooltipSystem): BuildTooltip sadece Start()'taydı → EnsureBuilt() (built bool guard), Show() başında çağrılıyor (ilk-hover panel null drop'u fix).

## Sub-questions (CODE/reuse lens)
1. **Data doğruluğu:** LMB hasarı `comboDamage[0]` (yoksa `projectileDamage`) — bu, LMB'nin GERÇEKTE uyguladığı ilk-vuruş hasarını doğru temsil ediyor mu (BasicAttackBehavior/combo nasıl hesaplıyor)? rmbDamage/rmbCost/rmbCooldown alanları RMB'nin gerçek değerleri mi?
2. **Relaxed guard regresyonu:** `description`→`skillName` — skillName dolu ama slot aslında boş/placeholder olan bir durum tooltip'i yanlış tetikler mi? Boş slot hâlâ null dönüp tip'lenmiyor mu?
3. **FIX B reuse/double-build:** EnsureBuilt idempotent mi, Start() çağrısı hâlâ var mı (çift build riski)? ShowDelayed coroutine timing ilk-hover'da artık garantili panel buluyor mu?
4. Genel: skill-slot (idx≥2) FormatSkill yolu değişmedi mi? HideSlotTooltip OK?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 yüksek-güven bulgu: severity · dosya:satır · 1-cümle (+fix). #1 (data doğruluğu) için NET yorum.
