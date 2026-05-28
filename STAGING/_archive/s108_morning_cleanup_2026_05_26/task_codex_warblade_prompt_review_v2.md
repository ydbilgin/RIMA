# TASK: Warblade Prompt Final Review v2 (READ-ONLY, NO COMMIT)

## SCOPE
Sadece review/analiz. Kod YAZMA, commit ETME, dosya değiştirme.

## CONTEXT

Warblade idle prompt v1 drift'e yol açtı (kafa öne kayma + minimal motion). v2 prompt'lar yazıldı.

GUIDE DOSYASI: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\Warblade_Production_Guide.md

İçinde 9 anim prompt'u var: Idle, Run, Hurt, Death, Dash, LMB Beat 1/2/3, RMB Heavy.

## REVIEW SORULARI

Her prompt için cevap ver:

1. **PixelLab Custom V3 motion capacity:** Bu prompt'u Custom V3 12/14 frame'e dönüştürebilir mi? Hangi cümle muğlak olabilir?

2. **Drift riski:** Loop anim (Idle, Run) için seamless loop ihlali tetikleyebilecek kelime var mı?
   - YASAK list (idle v2 dersi): scanning, head tilt, weight shift, occasional, drift, between feet
   - Bu listeyi tüm prompt'lara uygula

3. **Pixel miktarı:** Spesifik pixel sayısı var mı (örn "3-4 pixel chest rise")? Yoksa AI ne kadar hareket bilmiyor.

4. **Body part ayrımı:** Hangi part hareket, hangi sabit net mi?

5. **Frame count:** 
   - Idle 6 frame (12→6 revize edildi) — subtle breathing için yeterli mi?
   - Run 8 frame — full stride cycle için yeterli mi?
   - Hurt 6 frame — recoil + recovery için OK mi?
   - Death 12 frame — collapse drama için OK mi?
   - Dash 8 frame — anticipation + thrust + landing OK mi?
   - LMB Beat 1 8f, Beat 2 12f, Beat 3 14f, RMB 14f — combat feel için OK mi?
   - Pixel budget kontrol: 124×124 × frame ≤ 524,288 (her anim için ✓)

6. **Karar #71 silah hep elde:** Her prompt'ta silahın elde olduğu (sheath/draw YOK) implied mi?

7. **Chain transition:** LMB Beat 1 son frame ≈ Beat 2 ilk frame? Beat 2 son ≈ Beat 3 ilk? Smooth chain için kontrol.

## OUTPUT FORMAT

≤350 word. Format:

```
- Idle: PASS / FIX (gerekçe). Eğer FIX: spesifik prompt edit önerisi (1-2 cümle).
- Run: PASS / FIX
- Hurt: PASS / FIX
- Death: PASS / FIX
- Dash: PASS / FIX
- LMB Beat 1: PASS / FIX
- LMB Beat 2: PASS / FIX
- LMB Beat 3: PASS / FIX
- RMB Heavy: PASS / FIX

TECHNICAL VERDICT: 2-3 cümle. Tüm prompt'lar PixelLab Custom V3'te üretime hazır mı? Riskli noktalar?

OVERALL FRAME BUDGET: 
- 9 anim × ortalama 10 frame = 90 frame total
- 9 anim × 5 gen × ortalama 12 sec = 9 dakika gen süresi
- Tahmini credit: 45-90 (Custom V3)
```

CODEX_DONE.md'ye yaz, başka dosya dokunma.
