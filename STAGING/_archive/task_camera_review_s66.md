# Codex Independent Review: Kamera + Perspektif Kararı (S66)

**Tarih:** 2026-05-13
**Tip:** INDEPENDENT REVIEW (Antigravity + Opus + Karar #100 değerlendir)
**Çıktı:** CODEX_DONE.md'ye yeni bölüm append

## Bağlam

Kullanıcı (YD Bilgin) Antigravity (Gemini 3.1 Pro) ile konuşup şu hibrit kamera kararı önerisi getirdi:
1. Karakter: ~35° (mevcut anchor'lar korunsun)
2. Çevre/tile: 45° (Zelda-vari)
3. Kamera: Orthographic Size artır (zoom-out)
4. Drop shadow: karakter ayakları altına oval — "35° karakter 45° zeminde havada uçuyor" hissini gizlemek için

Hedef: Geniş savaş alanı hissi (referans: klasik 45-60° JRPG/Aksiyon).

## Mevcut LOCKED Karar #100 (S62, 2026-05-12)

- Pure 2D Top-Down chibi pixel art
- 64x64 chibi karakter, 32x32 tile
- High top-down **~30-35°** (Hades match)
- URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- PPU=64, 10-12 fps anim
- Karar #104: 10/10 karakter anchor PASS — ~30-35° tutarlı

## Opus Initial Judgment (review için)

**Verdict:** ACCEPT WITH MODIFICATIONS — Antigravity'nin "35° karakter + 45° tile" karışım önerisi REJECTED.

**Opus alternatifi: Karar #112 candidate**
> Karakter + tile + VFX tek konverjans: ~35° high top-down (Hades-Death's Door band).
> Karışık perspektif (35° char + 45° tile) REJECT — drop shadow ile maskelenemez.
> Geniş savaş alanı hedefi **Orthographic Size kalibrasyonu** ile sağlanır, tile açısı değiştirilerek DEĞİL.
> Pixel Perfect Camera + PPU=64 sabit. Drop shadow opsiyonel aesthetic polish (perspektif maskesi DEĞİL).
> Karar #100 REVISE: "~30-35°" → "~35° (Hades reference)" netleştirildi.
> Mevcut 10 anchor + anim prompt rework GEREKMEZ.

**Opus'un kilit argümanları:**
1. Karışık perspektif "hover" drop shadow ile maskelenemez (sprite ayak temas çizgisi ↔ tile vanishing point çelişir, özellikle walk/dash'te)
2. Hades bile karakter + tile aynı ~30° konvansiyonunu paylaşır — kamera sadece daha yukarı/uzakta
3. Geniş savaş alanı argümanı VALID ama çözüm **kamera mesafesi**, tile açısı değil
4. 10/10 anchor PASS (Karar #104) ve VFX AngleMode (#102) 35° için kalibre — 45° tile yeniden iş yaratır
5. 40° kompromis bile mümkün (35-40° band), sprite anchor'lar tolere eder

## Opus'un Codex'e Açık Soruları

1. Pixel Perfect Camera + Orthographic Size artışı + PPU=64 sabit → upscaling artifact riski? Doğru "Upscale Render Texture" + "Crop Frame" kombinasyonu?
2. URP 2D Renderer + 2D Lights, zoom-out'ta light range/falloff kalibrasyonu nasıl?
3. Drop shadow shader/mask implementation maliyeti (Sprite Mask vs custom shader vs simple child SpriteRenderer)? Bu karar kapsamına mı yoksa ayrı polish task mı?
4. 40° tile + 35° karakter (5° fark) playtest'te algılanabilir mi, yoksa 35-40° band güvenli mi?
5. PixelLab `create_topdown_tileset` "high top-down" preset gerçek açı değeri? UI Pro slider varsa exact value lock edilebilir mi?
6. Mevcut 10 anchor + anim prompt'ları 35° tutulursa kesinlikle rework GEREKMEZ — onaylar mısın?

## Antigravity'nin PIXELLAB_MASTER_REHBER.md İddiaları

Antigravity bu dosyayı 15 PixelLab eğitim videosu analiziyle "Altın Standart" olarak güncellediğini iddia ediyor:
- Generate → Clean → Animate → Refine döngüsü
- Inpaint Pro ile harita/tileset/parallax üretimi
- Karakter ölçekleme kuralları
- Hibrit kamera (35° char + 45° tile + drop shadow)

**Sen rehberi okuyup doğrula:** `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/PIXELLAB_MASTER_REHBER.md`
- Mevcut S60 LOCKED pipeline (Create Image Pixen NEW, Custom Animation V3, PPU=64, 32x32) ile çelişki var mı?
- Karar #75 (Map Workshop yasak) ile Inpaint Pro önerisi çelişiyor mu?
- Karar #106 (MCP üretim değil web UI) korunuyor mu?
- Karar #100 (~30-35°) Antigravity'nin 45° önerisiyle çelişiyor — rehber hangisini önermiş?

## Senin Görevin (Codex)

### Output Format (CODEX_DONE.md'ye append)

```
# Codex Review: Camera + Perspective + PixelLab Rehber S66

## Antigravity Önerisi Review
- Verdict: [SUPPORT OPUS / SUPPORT ANTIGRAVITY / THIRD WAY]
- Gerekçe:

## Opus Initial Judgment Review
- Verdict: [AGREE / MODIFY / DISAGREE]
- Gerekçe:

## Opus'un 6 Açık Sorusuna Cevap
1. Pixel Perfect Camera + Orthographic Size + PPU=64 upscaling artifact:
2. URP 2D Lights zoom-out kalibrasyon:
3. Drop shadow implementation maliyeti:
4. 40° tile + 35° char (5° fark) playtest algılanabilirlik:
5. PixelLab "high top-down" preset gerçek açı değeri:
6. Mevcut anchor + anim prompt rework gerekli mi:

## PIXELLAB_MASTER_REHBER.md Doğrulama
- Dosya gerçekten "Altın Standart" mı, yoksa eksik/çelişkili mi?
- Karar #75 çelişki:
- Karar #100 çelişki (45° hibrit):
- Karar #106 (MCP vs web UI) durumu:
- S60 pipeline uyumu:
- Net ÖNERİ: Kabul / Revize / Red

## Yeni Risk Tespitleri (Opus kaçırdı)

## Codex Karar Önerisi (#112 candidate revize)
[Opus metnini olduğu gibi onayla VEYA revize et]

## Opus'a Final Synthesis İçin Notlar
- En kritik 2-3 belirsizlik
- LOCK noktasında uzlaşılması gereken
```

## Kısıt
- Kod yazma, dosya değiştirme, commit YOK
- LOCKED karar override etme; revize öner
- Türkçe
- CODEX_DONE.md'ye append, 800-1200 kelime
- PIXELLAB_MASTER_REHBER.md'yi DOĞRUDAN OKU ve doğrula
- Effort: high
