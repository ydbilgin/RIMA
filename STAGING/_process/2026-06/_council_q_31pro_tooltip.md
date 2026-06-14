# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/UI/SkillBarUI.cs Assets/Scripts/UI/TooltipSystem.cs`
SkillBarUI.ShowSlotTooltip/BuildBasicAttackTooltip + TooltipSystem.Show/EnsureBuilt + BasicAttackProfile alanlarını READ et.

## Fix'ler
- FIX A: basic slot 0/1 tooltip BasicAttackProfile'dan (lmbName/comboDamage[0]||projectileDamage; rmbName/rmbDamage/rmbCost/rmbCooldown). Skill-slot guard description→skillName.
- FIX B: TooltipSystem panel build lazy+idempotent (EnsureBuilt, built bool), Show() başında.

## Derin sorular (UI lifecycle / doğruluk)
1. **TooltipSystem lifecycle:** EnsureBuilt + Start() birlikte — domain reload / sahne yeniden yükleme / play-stop sonrası built bool ve panel referansı tutarlı mı? Build iki kez (Start + ilk Show) çalışma riski tam kapandı mı?
2. **Basic-attack semantik doğruluğu:** slot1 "Temel Yetenek" (RMB) etiketi + maliyet/CD gösterimi RIMA tasarımıyla uyumlu mu (RMB gerçekten kaynak/CD'li bir "ability" mi, yoksa salt basic mi)? Gerekirse NLM sorgula. comboDamage[0] LMB ilk-vuruş için doğru alan mı?
3. **Relaxed guard:** description→skillName değişimi başka tooltip-tüketen yolu (drag, replace-mode preview) etkiliyor mu?
4. Daha sağlam/temiz alternatif var mı (over-engineering'e kaçmadan)?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (severity · dosya:satır · 1-cümle + öneri).
