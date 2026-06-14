# Council — ax Gemini 3.5 Flash (High) — LEAN / ship-fast / over-engineering critique

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs Assets/Scripts/VFX/SkillVfx.cs`

## Değişiklikler
- Warblade swing tek ember arc (mavi SlashArcVFX kaldırıldı).
- SkillVfx.ApplyAdditiveSprite (yeni): additive + element-agnostik boost `1.5/max(r,g,b)`.

## Lean sorular (demo ~20 Haz)
1. **Demo-bozan risk:** Bu fix demo'da Warblade vuruş VFX'ini veya GravityCleave'i bozar mı? NRE (all-zero color div, null SpriteRenderer), görünmez arc, ya da overexposed beyaz patlama riski? Evet/hayır.
2. **Over-engineering:** ApplyAdditiveSprite + PlaySweep param gereğinden karmaşık mı? Daha yalın yol var mıydı (örn. doğrudan additive material + tint, ekstra metot olmadan)?
3. **Eksik ucuz guard:** 1-2 satırlık atlanmış null/zero kontrolü?
4. Commit'e gider mi?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (1-cümle). Kısa.
