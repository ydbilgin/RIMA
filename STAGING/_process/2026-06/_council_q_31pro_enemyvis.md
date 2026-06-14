# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Enemies/EnemyAnimator.cs Assets/Scripts/Player/PlayerAnimator.cs`
Teşhis: STAGING/_process/2026-06/_done_enemy_combat_visible_2026-06-14.md

Kök: enemy clip null'lıyor → BaseMobBehavior.EnsureVisibleSprite kırmızı placeholder (boş adlı) basıyor → enemy kırmızı kare. Fix: EnemyAnimator [DefaultExecutionOrder(100)] + guard (sprite null || ad boş) restore. PlayerAnimator guard (sprite null || texture null).

## Derin sorular (mimari)
1. **Çift-keeper mimarisi:** Artık 2 keeper aynı SR için yarışıyor: BaseMobBehavior (kırmızı placeholder, order 0) + EnemyAnimator (gerçek sprite, order 100). Bu execution-order'a-bağımlı override kırılgan mı? Doğru soyutlama: BaseMobBehavior'ın kırmızı placeholder'ı GERÇEK cached sprite'ı basacak şekilde düzeltmek (tek keeper) ya da clip GUID'lerini onarmak mı? Demo için pragmatik mi?
2. **DefaultExecutionOrder(100) yan etkisi:** EnemyAnimator'ın Update'i (facing/anim param) de geç koşacak — diğer enemy sistemleriyle (AI, hareket) sıralama bozulur mu?
3. **Asimetri:** enemy name-based, player texture-based guard — aynı "broken sprite" sınıfı için iki farklı tespit. Birleşik/tutarlı bir guard daha mı sağlam (over-engineering'e kaçmadan)?
4. Regresyon: 14 sağlam enemy + Warblade + death-state.

## Çıktı
NET PASS/FAIL + en fazla 3 bulgu (severity · dosya:satır · 1-cümle + öneri).
