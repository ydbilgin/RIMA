# Council — ax Gemini 3.5 Flash (High) — LEAN / ship-fast / over-engineering critique

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/UI/SkillBarUI.cs Assets/Scripts/UI/TooltipSystem.cs`

## Fix'ler
- FIX A: basic slot 0/1 tooltip BasicAttackProfile'dan; skill-slot guard description→skillName.
- FIX B: TooltipSystem panel lazy+idempotent build (EnsureBuilt).

## Lean sorular (demo ~20 Haz)
1. **Demo-bozan risk var mı?** Bu 2 fix içinde demo'da tooltip'i bozacak / NRE atacak tek şey (özellikle EnsureBuilt null-panel, basic profile null) — evet/hayır.
2. **Over-engineering:** BuildBasicAttackTooltip StringBuilder + renk/size markup gereğinden ağır mı? Daha yalın yol?
3. **Eksik ucuz guard:** 1-2 satırla eklenebilecek atlanmış bir null/empty kontrolü (profile null, comboDamage boş array) var mı?
4. Commit'e gider mi?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (1-cümle). Kısa.
