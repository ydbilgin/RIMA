ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
P7.5 (Elementalist + enemy invisible-sprite) fix'inin commit-öncesi feasibility/regresyon review'u — CODE lens. Demo-kritik.

ANALYSIS ONLY. Dosya değiştirme. Unity açma. Sonucu CODEX_DONE.md'ye yaz.

## Bağlam
Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Systems/PlayerClassManager.cs Assets/Scripts/Enemies/EnemyAnimator.cs`
Teşhis: STAGING/_process/2026-06/_diag_invisible_sprites_2026-06-14.md
Kök: animator clip'leri stale/missing sprite GUID'lerine işaret ediyor → animator her frame SpriteRenderer.sprite'ı null'a sürüyor.

Fix'ler:
- FIX A (PlayerClassManager.ApplyPrimaryClassVisual): `if (!animatorDriving || FindBodySprite(player)==null) ApplyClassIdleSprite(...)` — animatorDriving olsa bile body SR null ise idle sprite uygula. **ONE-TIME** (sadece class-apply anında çalışır).
- FIX B (EnemyAnimator): Awake'te prefab sprite cache + **LateUpdate'te HER FRAME** null ise restore. (Enemy animator her frame null'ladığı kanıtlandı.)

## Sub-questions (KRİTİK ASİMETRİ)
1. **#1 KRİTİK — FIX A persistence:** Enemy animator HER FRAME sprite'ı null'lıyor (bu yüzden FIX B per-frame LateUpdate). Player (Elementalist) AYNI mekanizma değil mi? Elementalist AnimatorController + idle .anim clip'lerini incele: idle state'in sprite curve'ü (m_PPtrCurves) VAR mı ve player Animator idle state'i loop'layıp HER FRAME bu null curve'ü Body SR'ye yazıyor mu? Eğer EVET → FIX A'nın one-time fallback'i frame-1'den sonra animator tarafından EZİLİR → Elementalist tekrar görünmez. O zaman player de FIX B gibi PERSISTENT (LateUpdate/PlayerAnimator) guard'a ihtiyaç duyar. Eğer HAYIR (player curve null yazmıyor / curve yok / drop ediliyor) → one-time yeterli. NET yargı + kanıt (hangi asset/satır). Player ile enemy arasındaki farkı açıkla.
2. **FindBodySprite doğruluğu:** "Body" adlı SR'yi buluyor; bu, animator'ın sürdüğü SR ile AYNI mı (facing fix: Animator on Body child)? Yanlış SR'yi okursa null-tespit hatalı olur.
3. **FIX B robustluk:** Awake'te `_fallbackSprite = sr.sprite` — o anda prefab sprite zaten null ise (Awake animator'dan önce mi sonra mı?) cache null kalır mı? 14 etkilenmeyen enemy LateUpdate guard'dan zarar görür mü (normal animasyonu null'a düşmediği için no-op, doğrula)? Ölü enemy (_isDead) guard'lı.
4. Genel regresyon: Warblade hâlâ animator-driven (fallback fire etmiyor)?

## Çıktı (CODEX_DONE.md, kısa)
VERDICT = PASS / PASS-with-nits / FAIL. En fazla 3 bulgu: severity · dosya:satır · 1-cümle (+fix). **#1 (player re-null mu?) için NET evet/hayır + kanıt ZORUNLU.**
