# Warblade Run V2 — State-Anchored Custom V3 (S69)

**State Karakter:** `8d5d8d19-9d61-46ea-b5bf-463f96eba744` (running stride apex)
**Base Karakter:** `6bf7afdb-fc51-4bb4-9fc0-6c1039b1c0f3` (warblade — animasyon buraya port edilebilir)

## State Inceleme (2026-05-13)
- ✅ Forward lean
- ✅ Stride pose (geniş combat stance kırıldı)
- ✅ Cape dinamik
- ✅ Greatsword silhouetted (Karar #99)
- ⚠️ Kılıç önde tutuluyor (yan trail değil) — kabul

## PROMPT (Custom V3, enhance KAPALI)

Chibi warrior continues sprinting forward in a fast, weighted gait. Each frame advances the stride cycle — the lifted knee drives down and plants, the back leg sweeps forward into a new high knee lift, alternating in a smooth running rhythm. The forward body lean stays constant throughout the run. The cape trails and ripples behind with each push-off. The greatsword stays carried in the hand, blade swaying gently with the body's momentum, the weapon clearly visible and silhouetted in every pose. The motion feels fast, heavy, but agile — a soldier closing distance at full speed.

## Test Protokolü

1. **South** önce dene (8f, 2 gen)
2. Frame'leri kontrol et:
   - Bacaklar alternate ediyor mu? (sadece bob değil)
   - Forward lean korunuyor mu?
   - Kılıç her frame'de görünür mü?
   - Cape akıyor mu?
3. **PASS** → 7 diğer yön (toplam 16 gen)
4. **FAIL** → frame_005-008 büyük olasılıkla sorun; prompt'a "the stride continues into a second cycle — knee lifts again" ekle

## Cost
- 8 yön × 2 gen = 16 gen total
- State gen + bu anim = ~20 gen total (8 yön anim için)
