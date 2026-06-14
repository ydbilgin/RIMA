ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
P5 (Warblade swing tek-ember-arc + element-agnostik additive tint) fix'inin commit-öncesi feasibility/reuse/regresyon review'u — CODE lens.

ANALYSIS ONLY. Dosya değiştirme. Unity açma. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs Assets/Scripts/VFX/SkillVfx.cs`
SkillVfx.MeleeArc + PlaySweep + ApplyAdditiveSprite + Palette + MeleeChainBehavior :90-120 + SlashArcVFX kullanımını READ et.

Değişiklikler:
- MeleeChainBehavior :97/:112: Warblade basic swing'den mavi EmitSlashArc/SlashArcVFX çağrıları kaldırıldı; ember SkillVfx.MeleeArc kaldı.
- SkillVfx.PlaySweep: yeni `additiveSprite=false` param (default false); true ise ApplyAdditiveSprite çağrılıyor.
- SkillVfx.MeleeArc: base sprite slash_arc_crescent (önce) + PlaySweep(additiveSprite:true).
- SkillVfx.ApplyAdditiveSprite (yeni): SpriteRenderer'ları additive material'e çevirir + vertex color'ı element palette'ine göre hue-koruyarak boost eder: `scale = 1.5 / max(r,g,b)` (dominant kanal 1.5, oran+alpha korunur).

## Sub-questions (CODE/reuse lens)
1. **Shared helper regresyon (KRİTİK):** SkillVfx.MeleeArc'ın TÜM çağıranlarını Grep'le. Bilinen: MeleeChainBehavior :99/:115 (Physical), GravityCleave :39 (Void). Başka çağıran/element var mı? Element-agnostik formül (`1.5/max(r,g,b)`) HER VfxElement palette rengi için hue'yu koruyor mu (Physical ember, Void mor, varsa Fire/Ice/Lightning)? Siyah/all-zero palette'te max(...) sıfır-bölme guard'lı mı?
2. **PlaySweep param:** additiveSprite default false → MeleeArc DIŞINDAKİ PlaySweep çağıranları etkilenmiyor, değil mi? Grep'le doğrula.
3. **Blue arc kaldırma:** SlashArcVFX class/prefab/diğer çağıranlar (MarkPulseBehavior :71 Ravager) sağlam mı, dead code/kırık ref yok mu? EmitSlashArc artık çağrılmıyorsa unused mu kaldı (silinmeli mi, yoksa başka kullanıcı var)?
4. **Additive material/sorting:** SharedAdditiveMaterial'e geçiş sorting/render side-effect yaratıyor mu?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 yüksek-güven bulgu: severity · dosya:satır · 1-cümle (+fix). #1 (shared helper tüm element/çağıran) için NET yargı.
