ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
P7.5c (enemy combat görünürlüğü — kırmızı placeholder override) fix'inin commit-öncesi feasibility/regresyon review'u — CODE lens. Demo-kritik.

ANALYSIS ONLY. Dosya değiştirme. Unity açma. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Enemies/EnemyAnimator.cs Assets/Scripts/Player/PlayerAnimator.cs`
Teşhis: STAGING/_process/2026-06/_done_enemy_combat_visible_2026-06-14.md
Kök: enemy clip her frame sprite'ı null'lıyor → `BaseMobBehavior.EnsureVisibleSprite` (LateUpdate order 0) non-null KIRMIZI 48x48 placeholder (boş ADLI) basıyor → eski `sprite==null` guard tetiklenmiyor → enemy kırmızı kare. Fix: EnemyAnimator `[DefaultExecutionOrder(100)]` (LateUpdate en son) + guard `sprite==null || IsNullOrEmpty(sprite.name)`. PlayerAnimator guard `sprite==null || sprite.texture==null`.

## Sub-questions (CODE lens)
1. **#1 KRİTİK name-guard over-fire:** GERÇEK enemy animasyon frame sprite'larının ADI var mı (boş/null değil)? Çalışan enemy'lerin (16'dan etkilenmeyen 14) clip frame sprite'larını + asset isimlerini kontrol et — herhangi birinin sprite.name'i boş/null ise guard yanlışlıkla onların gerçek sprite'ını _fallbackSprite ile ezer = regresyon. NET yargı.
2. **#2 DefaultExecutionOrder(100):** Bu, EnemyAnimator'ın TÜM mesajlarını (Awake/Update/LateUpdate) order 100'e alır. Update'teki hareket/facing/animator-param mantığı diğer enemy component'lerine (BaseMobBehavior, EnemyAI) göre GEÇ koşunca facing/animasyon bozulur mu? Sadece LateUpdate'i geciktirmek yeterken Update de gecikiyor — risk?
3. **#3 BaseMobBehavior root:** `BaseMobBehavior.EnsureVisibleSprite` neden kırmızı placeholder basıyor (bu da bir keeper) — asıl çözüm orayı düzeltmek mi (kırmızı yerine gerçek sprite ya da clip onarımı)? EnemyAnimator override pragmatik mi yoksa iki keeper çakışması mı? 1-frame kırmızı flash kalıyor mu (order 100 aynı frame'de sonra koştuğu için hayır mı)?
4. PlayerAnimator texture-guard: gerçek player frame'inde texture==null olan var mı (over-fire)? Enemy name-based vs player texture-based asimetri sorun mu?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 bulgu: severity · dosya:satır · 1-cümle (+fix). #1 (name-guard over-fire) ve #2 (execution order) için NET yargı.
