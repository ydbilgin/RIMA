# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Dosya değiştirme, Unity açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Systems/PlayerClassManager.cs Assets/Scripts/Enemies/EnemyAnimator.cs`
Teşhis: STAGING/_process/2026-06/_diag_invisible_sprites_2026-06-14.md

İki invisible-sprite bug (demo-kritik), aynı kök: animator clip stale/missing sprite GUID → her frame SR.sprite null.
- FIX A (PlayerClassManager): one-time fallback (animatorDriving ama body SR null ise ApplyClassIdleSprite).
- FIX B (EnemyAnimator): per-frame LateUpdate sprite-keeper.

## Derin sorular
1. **TUTARLILIK/ASİMETRİ (KRİTİK):** Enemy per-frame guard gerektirdi (animator her frame null'lıyor). Player one-time fallback aldı. Aynı kök mekanizma ise bu asimetri bir BUG: player animator'ı da her frame Body SR'yi null'larsa Elementalist frame-1'den sonra tekrar görünmez olur. Player Animator idle state'inin sprite curve davranışını (Elementalist controller + .anim) analiz et: player one-time YETER mi yoksa enemy gibi PERSISTENT guard mı gerekir? Mimari olarak iki fix aynı pattern'i mi kullanmalı (tek bir "SpriteKeeper" component player+enemy)?
2. **Doğru soyutlama:** Bu "broken-clip→null" sorununun kök çözümü clip GUID'lerini onarmak; runtime guard semptom maskeleme. Demo için guard pragmatik mi, yoksa daha temiz bir yer mi var (asset onarımı vs runtime)?
3. Regresyon: Warblade/diğer sınıflar + 14 sağlam enemy etkilenir mi? Ölü enemy/death state guard'la çakışır mı (sprite restore vs death sprite)?

## Çıktı
NET PASS/FAIL + en fazla 3 bulgu. #1 (player persistence) için NET yargı.
