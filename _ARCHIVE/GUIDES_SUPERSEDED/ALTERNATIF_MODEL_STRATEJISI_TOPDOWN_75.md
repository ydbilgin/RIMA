# Alternatif Model Stratejisi (Top-Down ~75° Hedefi)

Tarih: 2026-04-20  
Proje: RIMA 2D Roguelite  
Amaç: PixelLab ile tam kilitlenemeyen ~75° yukarıdan bakış ve oran tutarlılığını çözmek

---

## 1) Kısa Sonuç

- PixelLab tek başına hızlıdır ama `high top-down` kontrolü doğal olarak zayıf olabilir.
- ~75° gibi daha dik açı hedefinde en güvenilir yaklaşım: **hibrit pipeline**.
- Hibrit pipeline:
  1. Dış modelde **South master silhouette** üret (açı/oran kilidi)
  2. PixelLab’de **rotate + inpaint + cleanup** yap

Bu, “cüceleşme / helmet drift / açı kayması” sorununu en az tekrar ile çözer.

---

## 2) Hangi model?

## A) En güvenli teknik seçenek (önerilen)

**ComfyUI + Stable Diffusion 3.5 + ControlNet (Depth veya Pose)**  
- Artı:
  - Kamera/açı kontrolü PixelLab’dan daha güçlü yönetilebilir.
  - Oran kilidi (uzun bacak, kafa-gövde oranı) daha deterministik hale gelir.
  - Aynı karakterin varyantlarını kontrollü üretmek kolaydır.
- Eksi:
  - Kurulum ve node akışı öğrenme maliyeti var.

## B) Güçlü alternatif

**FLUX.1 [dev/pro] + kontrol tabanlı img2img akışı**  
- Artı:
  - Prompt adherence ve detay üretimi güçlü.
  - South master concept üretiminde kalite yüksek.
- Eksi:
  - Pixel-art görünümünü ayrıca disipline etmek gerekir (palette/cleanup).

## C) Sadece PixelLab ile kalınacaksa

- Iterative rotate zinciri zorunlu:
  - `S -> SE -> E -> NE -> N`
  - `S -> SW -> W -> NW`
- Fail yön zincire sokulmaz, o adım yeniden üretilir.
- Bu çalışır ama 75° hedefinde stabilite yine sınırlı olabilir.

---

## 3) Neyi nerede üretelim?

## Dış modelde üretilecekler
- Sadece `south_master.png` (tek frame)
- Hedef:
  - yetişkin oran (not short, not squat)
  - uzun bacak
  - no helmet (eğer class lock buysa)
  - istenen world kimliği
  - top-down hissi güçlü

## PixelLab’de üretilecekler
- 8 yön rotasyon
- Yön-bazlı düzeltmeler
- Inpaint ile problemli bölgeler (silah, omuz, bacak)
- Son temizleme ve export

---

## 4) Uygulama Planı (Net)

1. Dış modelde Warblade `south_master` üret  
2. `south_master` için 5 QC kontrolü:
   - açı yeterince yukarıdan mı?
   - cüce/squat görünüm var mı?
   - kafa-gövde oranı normal mi?
   - siluet temiz mi?
   - kimlik (weapon/armor/accent) net mi?
3. Geçerse PixelLab Rotate Pro’ya ver  
4. Iterative zincirle 8 yön üret  
5. Her adımda fail olursa tekrar; fail frame ile devam etme  
6. North veya diagonal zayıfsa ayrı pass + küçük pixel cleanup

---

## 5) Claude’a direkt verilecek talimat

```text
We will use a hybrid pipeline.
Generate only a locked south master in an external model first (top-down steep feel, mature adult proportions, long legs, non-squat silhouette, class identity locked).
Then use PixelLab only for iterative rotation and inpaint cleanup:
S->SE->E->NE->N and S->SW->W->NW.
Do not continue the chain with a failed frame.
If a direction drifts (helmet, squat, side-profile), regenerate that direction only.
```

---

## 6) Kaynaklar

- PixelLab Docs — Create 8-directional sprite (Pro):  
  https://www.pixellab.ai/docs/tools/create-8-rotations-pro
- PixelLab Docs — Rotating a character (iterative yöntem):  
  https://www.pixellab.ai/docs/guides/rotating-a-character
- PixelLab Docs — Rotate tool:  
  https://www.pixellab.ai/docs/tools/rotate
- PixelLab Docs — Camera options (view/direction weak):  
  https://www.pixellab.ai/docs/options/camera
- Stability AI models sayfası (SD 3.5):  
  https://stability.ai/Models
- Stability AI — SD 3.5 duyurusu:  
  https://stability.ai/news/introducing-stable-diffusion-3-5
- FLUX.1 [dev] model card:  
  https://huggingface.co/black-forest-labs/FLUX.1-dev
- ComfyUI ControlNet guide:  
  https://docs.comfy.org/tutorials/controlnet/controlnet

---

## 7) Karar Matrisi

- Hız en önemliyse: PixelLab-only (ama daha fazla retry bekle)
- Açı ve oran doğruluğu en önemliyse: Hybrid (önerilen)
- En düşük risk: SD3.5/FLUX ile south lock + PixelLab rotate

