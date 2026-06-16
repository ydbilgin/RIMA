# Warblade Animasyon Önceliklendirme ve Üretim Planı (State-First)

## S1: Animasyon Önceliklendirmesi (P1 / P2 / P3)
- **P1 (Must-Have / Golden-Path):** Idle, Koşma, LMB Vuruş. (Edit-to-Play videonun belkemiği ve golden-path için zorunlu olan tek set).
- **P2 (Yüksek Değer / Vakit Varsa):** Hit-React (Flinch). (Combat stat→damage hissini pekiştirir ancak demo blocker değildir).
- **P3 (Demo Dışı / Reuse):** Q/E/R/F yetenekleri (bespoke animasyonlar), Warblade Death. (5 günlük sürede ölüm animasyonuna harcanacak efor gereksizdir, ekran efekti ile geçiştirilmeli).

## S2: Q/E/R/F Skill Stratejisi
**Kesinlikle REUSE.** %60 mimari / %20 oyun tezi ve 5 günlük dar zaman/cleanup eforu göz önüne alındığında, stat-deaf yetenekler için sıfırdan 8 yönlü animasyon üretmek risklidir. Q, E, R, F yeteneklerinin tamamında LMB vuruş animasyonunu (veya renklendirilmiş/VFX eklenmiş halini) geri dönüştür.

## S3: State-First Üretim Planı
Sıralama: *Poz (State) Üretimi → Custom Animation V3 → Gerekirse Interpolation*. Toplam 5 yön (S, SE, E, NE, N) üretilip, kalan 3 yön mirrorlanacak.
1. **Idle (P1):** State = *Breathing/Standby Pose* → Custom Anim V3 (Nefes alma/bekleme loop).
2. **Koşma (P1):** State = *Mid-stride/Action Run Pose* → Custom Anim V3 (Koşma döngüsü).
3. **LMB Vuruş (P1):** State = *Strike Windup (Kılıcı arkaya çekme)* → Custom Anim V3 (Vuruş ve follow-through frame'leri için interpolation).
4. **Hit-React (P2):** State = *Flinch/Recoil Pose* → Custom Anim V3 (Geriye sekme ve idle'a dönüş).

## S4: Bütçe ve Gerçekçilik Analizi
- **Budget:** 874 generation fazlasıyla yeterli. Bol bol reroll yapılabilir.
- **Zaman/Cleanup:** Asıl risk budur. P1 paketi (3 animasyon × 5 yön = 15 ayrı sheet) Pixelorama'da ciddi manuel cleanup gerektirecektir. 5 günlük sürede bu **gerçekçidir** ancak P2'ye (Flinch) veya başka bir aksiyona girilmemesi, tüm eforun P1'in videoda kusursuz görünmesi için harcanması şarttır.

---
**TEK CÜMLE TAVSİYE:** Demoyu riske atmamak için tüm cleanup eforunu sadece Idle, Koşma ve LMB Vuruş üçlüsüne yatır, Q/E/R/F yeteneklerinde kesinlikle LMB animasyonunu VFX ile geri dönüştür.
