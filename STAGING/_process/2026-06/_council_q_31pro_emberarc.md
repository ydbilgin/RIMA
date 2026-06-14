# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs Assets/Scripts/VFX/SkillVfx.cs`
SkillVfx.MeleeArc/PlaySweep/ApplyAdditiveSprite/Palette + MeleeChainBehavior :90-120 READ et.

## Değişiklikler
- Warblade swing tek arc (ember SkillVfx.MeleeArc); eski mavi SlashArcVFX kaldırıldı.
- ApplyAdditiveSprite (yeni): additive blend + element-agnostik hue-koruyan boost `1.5/max(r,g,b)`.
- MeleeArc base sprite → slash_arc_crescent; PlaySweep additiveSprite:true.

## Derin sorular (mimari / doğruluk)
1. **Element-agnostik tint tasarımı:** `1.5/max(r,g,b)` boost yaklaşımı doğru mu? Tüm VfxElement'lerde (Physical ember, Void mor) hue'yu koruyup teal base'i bastırıyor mu? Additive blend + boosted vertex color kombinasyonu beyaz-doygunluğa (overexposure) kaçar mı parlak sahnede? Daha sağlam bir yaklaşım (örn. base sprite'ı grayscale/luminance kullan, ya da shader-level) demo için over-engineering mi olur?
2. **Shared MeleeArc mimari:** MeleeArc'ı hem basic-attack hem skill (GravityCleave) paylaşıyor — bu helper'ın element-parametreli olması doğru soyutlama mı, yoksa basic-attack'in kendi arc'ı mı olmalı? (canon: ember #E89020, void MOR #3A1A4A)
3. **Regresyon:** mavi arc kaldırılınca SlashArcVFX'in MarkPulse/Ravager kullanımı + EmitSlashArc'ın dead kalıp kalmadığı; PlaySweep yeni param geriye uyumlu mu?
4. Daha temiz alternatif (over-engineering'e kaçmadan)?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (severity · dosya:satır · 1-cümle + öneri).
