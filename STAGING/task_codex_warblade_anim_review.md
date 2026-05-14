# TASK: Warblade Animation Technical Review (READ-ONLY, NO COMMIT)

## SCOPE
Bu sadece teknik review task'ı. Kod YAZMA, commit ETME. Sadece analiz raporu döndür.

## CONTEXT

Warblade animasyon spec'i — kullanıcı PixelLab web UI'da üretecek. Önce teknik review iste:

**Mevcut spec:**
- Canvas 252×252 ZORUNLU (PixelLab v3)
- Unity'ye 128×128 crop import (PPU=64)
- Pixel budget: w × h × frame ≤ 524,288
- 252² × frame ≤ 524,288 → 8 frame MAX
- Tool çift sayı: 4/6/8/10/12/14/16 only
- MCP karakter anim YASAK, sadece web UI

**Anim set (NLM):**
| Anim | Frame | Yön | Tool |
|---|---|---|---|
| Idle | 6-8 | 3 (S/E/N) | Animate Text NEW |
| Run | 6 | 8 (flip yok) | PixelLab Animate built-in |
| Hurt | 4 | 3 (S/E/N) | Animate Text NEW |
| Death | 6 | 3 (S/E/N) | Animate Text NEW |
| Dash | 4 | ? | Animate Text NEW |
| Attack_LMB | 4+5+5 (3 beat) | ? | Animate Text + Interpolate |
| Attack_RMB | 4+4=8 PEAK-shared | ? | Animate Text + Interpolate |

**Class profile:** Warblade = 2-el greatsword (~%45 sprite), heavy deliberate, low guard idle, hit-stop friendly.

**Kararlar (referans, değiştirme):**
- Karar #71: silah hep elde, sheath/draw YOK
- Karar #42: Walk YOK Run var, Idle interpolate first+last aynı
- Karar #46: Run 6 frame 8 yön flip yok
- Karar #48 (eski): Death/Hit 4 yön — Karar #114 (yeni) 8 yön LOCKED locomotion
- Karar #108: Custom V3 4-16 frame, Create State 20-40 gen
- Karar #80: Class Silhouette Bible Warblade
- Karar #107: animation prompts skill alignment final

## REVIEW SORULARI (sırayla cevap ver)

1. **Pixel budget hesabı doğru mu?** 252×252×8 = 508,032 ≤ 524,288 ✓ → 8 frame OK. 252×252×10 = 635,040 > 524,288 ✗ → 10 frame YASAK. Doğru mu, başka subtle constraint var mı?

2. **Canvas 252×252 dışında alternatif?** PixelLab v3 zorunlu 252 — daha küçük canvas (örn 200×200 veya 160×160 silahlı) PixelLab'da seçilebilir mi? Resmi docs varsa kontrol et.

3. **Attack_LMB 3-segment workflow (4+5+5 frame, ayrı 3 anim olarak)** — PixelLab Animate with Text NEW + Interpolate NEW kombinasyonu bu yaklaşımı destekliyor mu? Birleştirme Unity'de mi yapılır yoksa PixelLab tek-anim mı verir? Single 14-frame attack üretmek mümkün mü?

4. **8 frame max heavy attack için yeterli mi?** Hades Bouldy ~14-18 frame heavy. Bizim 8 frame PEAK-shared olunca pratikte 7 unique frame. Industry standard heavy attack için minimum frame nedir?

5. **Karar #114 8-direction locomotion + Karar #48 4-direction death/hurt** çelişki yok — locomotion 8, hit/death 4 → resolve edildi mi? Code/script veya Unity Animator'da 4-yön+8-yön karışım nasıl yönetilir?

6. **NLM eski spec'te 3 yön (S/E/N)** ama Karar #48 4 yön, Karar #114 8 yön — hangisi geçerli? Kararlar üzerinden döküm yap.

7. **PixelLab Animate with Text NEW vs Custom V3** ayrımı — Karar #108 "Custom V3 4-16 frame" diyor. "Animate with Text NEW" farklı bir tool mu? Aynı şey mi? Hangi farklılıklar?

8. **8 frame attack için tek "gerçek" frame planı:** 1 start + 3 windup + 1 PEAK + 2 follow + 1 recovery = 8. Bu Warblade heavy hit-stop ile uyumlu mu (REF_NUGGETS §1: heavy hit-stop 2-4 frame)?

## OUTPUT FORMAT

≤350 word, madde halinde her 8 soruya cevap. Sonunda "TECHNICAL VERDICT" 3 cümle: spec teknik olarak yapılabilir mi, riskli noktalar neler, frame count yeterli mi?

## YASAK
- Kod yazma
- Commit yapma
- Dosya değiştirme
- Test çalıştırma

Sadece analiz, görüş, teknik doğruluk raporu. CODEX_DONE.md'ye yaz.
