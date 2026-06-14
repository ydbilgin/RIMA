# Council — ax Gemini 3.5 Flash (High) — LEAN / ship-fast / over-engineering critique

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Systems/PlayerClassManager.cs Assets/Scripts/Enemies/EnemyAnimator.cs`

Demo-kritik invisible-sprite fix:
- FIX A (PlayerClassManager): one-time fallback, animatorDriving ama body SR null ise ApplyClassIdleSprite.
- FIX B (EnemyAnimator): per-frame LateUpdate sprite restore.

## Lean sorular (demo ~20 Haz)
1. **#1 Demo-bozan risk:** Elementalist (2 demo sınıfından biri) GERÇEKTEN görünür kalıyor mu, yoksa player animator one-time fallback'i ezip tekrar görünmez mi yapar? (Enemy per-frame guard aldı — player neden almadı? Eğer player de her frame null'lanıyorsa FIX A yetmez.) Evet/hayır.
2. **Over/under-engineering:** Enemy LateUpdate guard 16 enemy'de çalışıyor (2 etkilenmiş) — perf/karmaşıklık sorun mu? FIX A & FIX B aynı sorunu farklı şekilde çözüyor — birleştirilmeli mi?
3. **Eksik ucuz guard:** Atlanmış 1-2 satırlık kontrol?
4. Commit'e gider mi?

## Çıktı
NET PASS/FAIL + en fazla 3 bulgu (1-cümle). #1 net evet/hayır. Kısa.
