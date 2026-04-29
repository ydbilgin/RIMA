# PixelLab Top-Down Üretim Kararı (Kök Çözüm)

Tarih: 2026-04-20  
Kapsam: RIMA karakter üretim pipeline kararı (High Top-Down, 128px, 8 yön)

## 1) Net Karar: PixelLab uygun mu?

Evet, uygun.  
Ama yalnızca şu koşulla: **tek prompt + tek sefer 8 yön** değil, **iteratif referans zinciri + QC kapısı + gerekirse yön-bazlı yeniden üretim**.

Resmi dokümanlar da bunu doğruluyor:
- PixelLab “Create 8-directional sprite (Pro)” tek sefer üretimi destekliyor, ama bunun “ideal” olması her karakterde tutarlılık garantisi vermiyor.
- PixelLab “Rotating a character” rehberi doğrudan iki yöntem veriyor:
  1. Tek referanstan tüm yönler  
  2. Her adımda yeni referansla 45° iteratif dönüş  
  Bu rehber, küçük dönüşlerin daha iyi çalıştığını ama iterasyonda hata birikebileceğini açıkça söylüyor.
- PixelLab “Camera options” sayfası view/direction kontrolünün **weak** olduğunu belirtiyor; yani prompt tek başına kamera kilitlemez, init/reference gerekir.

## 2) Kök Sorun Neden Oluyor?

Senin yaşadığın “cüceleşme + açı kayması + knight’a dönme” üçlüsünün temel nedeni:

1. **View kontrolü zayıf**: “High top-down” seçimi tek başına yeterli değil (resmi doküman bunu söylüyor).  
2. **Tek adım 8 yön drift**: model bazı yönlerde (özellikle north / diagonaller) silueti bozuyor.  
3. **Kimlik-geometri karışması**: aynı promptta zırh/silah/tema fazla baskınsa model oranı ikinci plana atıyor.  
4. **Yanlış araç başlangıcı**: M-XL ile başlayan görsellerde “concept art” bias’ı geliyor; piksel grid disiplini bozuluyor.

## 3) Uygulanacak Çalışma Standardı (Zorunlu)

## A) South Master Lock (tek hedef)
- Araç: Create from Concept/Template Pro (pixel-art odaklı akış)
- Boyut: 128x128
- View: High top-down
- Arkaplan: Transparent
- Üretim hedefi: yalnızca `south` yönünde doğru gövde/oran/siluet
- Bu aşamada geçme kriteri:
  - yetişkin oran (uzun bacak, normal kafa-gövde oranı)
  - no helmet (eğer o class için kilit buysa)
  - full plate knight görünümüne kaymama

## B) Rotate Zinciri (iteratif)
- Tek sefer 8 yön yerine:
  - S -> SE -> E -> NE -> N
  - S -> SW -> W -> NW
- Her adımda bir önceki onaylı frame referans olur.
- Her adım sonrası QC fail ise o adım tekrar üretilir, zincire fail frame sokulmaz.

## C) Yön-bazlı yeniden üretim hakkı
- E/W profile’a kayarsa, o yön ayrı promptla yeniden üretilir.
- North çok basıksa ayrı üret + gerekirse 2-3px Aseprite düzeltmesi.

## D) Prompt yapısı (sabit sıra)
- Kamera/geometri -> oran -> pose -> class identity -> render
- Kimlik bloğu geometriyi ezmeyecek kadar kısa tutulur.

## 4) Kullanılacak Prompt Kalıbı (şablon)

```text
high top-down view, camera angled steeply from above, full body readable from above, top of head visible, torso clearly readable, volumetric body forms with visible depth and thickness, mature adult proportions, long legs, normal head-to-body ratio, not short, not squat, [CLASS_IDENTITY_SHORT], pixel art, 128px canvas, clean readable silhouette, light cel shading, transparent background
```

Negatif kilit (kısa ve sabit):

```text
not chibi, not dwarf-like, not full plate knight, not side-view portrait, not isometric, not paper-thin, not sticker-like
```

## 5) Araç Seçimi Kararı (kritik)

- **Create M-XL**: final character direction üretimi için önerilmez (style drift riski yüksek).  
- **Create from Concept/Template Pro + Rotate Pro**: ana yol.  
- **Inpaint / init-image strength iterasyonu**: hatalı yön toparlama için zorunlu yardımcı yol.

## 6) GO/NO-GO Kriteri

GO (PixelLab ile devam):
- South master 3 denemede tutuyorsa
- 8 yönün en az 6’sı tek zincirde tutarlıysa
- Kalan 2 yön yön-bazlı fix ile toparlanıyorsa

NO-GO (hibrit akışa geç):
- South master sürekli cüce/knight’a kayıyorsa
- Rotate zincirinde kimlik 2+ adımda bozuluyorsa
- North/E/W sürekli profile-flat çıkıyorsa

NO-GO durumunda çözüm:
- Gemini/A1111 ile sadece **south master silhouette** üret
- PixelLab’i yalnızca rotate/inpaint aşamasında kullan

## 7) Claude’a Verilecek Kısa Talimat

```text
Use iterative rotation only (S->SE->E->NE->N and S->SW->W->NW), no single-pass 8-direction generation as default.
Lock geometry first (adult proportions, long legs, normal head/body, non-squat) before class identity.
Treat camera controls as weak; enforce with init/reference images and per-step QC.
If a direction drifts (profile/flat/knight), regenerate that direction only; do not continue chain with failed frames.
```

## 8) Kaynaklar

- PixelLab Docs — Create 8-directional sprite (Pro): https://www.pixellab.ai/docs/tools/create-8-rotations-pro  
- PixelLab Docs — Rotating a character (guide): https://www.pixellab.ai/docs/guides/rotating-a-character  
- PixelLab Docs — Rotate tool: https://www.pixellab.ai/docs/tools/rotate  
- PixelLab Docs — Camera options (view/direction weak control): https://www.pixellab.ai/docs/options/camera  
- PixelLab YouTube (erişilebilen örnek): “Tutorial: How to rotate your character with PixelLab” https://www.youtube.com/watch?v=bNtMm0SeJ74

