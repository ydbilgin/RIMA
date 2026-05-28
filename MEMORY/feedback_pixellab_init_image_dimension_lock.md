# PixelLab Init Image Dimension Lock — S99 LATE finding

## Rule
PixelLab **Create Image S-XL (new)** (`create_image_pixen`) — kullanırken **init image upload** edersen, output dimensions **otomatik kilitlenir** init image boyutuna eşit veya proportional. Multi-aspect-ratio set (örn N wall 384×128 + W wall 128×384 + corner 128×128) tek workflow'da yapılamaz.

## Why
Init image S-XL new'de spatial/structure reference rolü oynar — tool çıktıyı init image structure'ına benzetmeye çalışır → boyut değişimi structural compatibility için bloklanır.

## How to apply
- **Style chaining + multi-aspect set istiyorsan:** Init image kullanma, **prompt-only style lock** dene (palette/lighting kelimeleri her gen'de aynı), VEYA tool değiştir.
- **Tool alternatif:** `create_image_pro` (Pro tier, 20 gen) — bunda **style image slot ayrı** (max 512×512), output dimension'ı kilitlemez. Boyut serbest seç, style chain anchor görsel ayrı upload.
- **Atlas approach:** Tek büyük canvas'ta multi-piece composition üret → Python ile slice. Init image gereksiz, tek painting → %100 style consistency.

## Verified S99 LATE
- N wall section 384×128 init verince → S edge 384×64 üretmek istedik → tool 384×128 kilitli verdi
- W vertical 128×384 init image yokken üretilebiliyor ama style drift var
- Pro tool style image slot dimensions kilitlemiyor — confirmed via docs (`MEMORY/PIXELLAB_TOOL_GUIDE.md`)

## Related
- [[project_wall_production_pipeline_s99_late]] — current wall pipeline state
- [[PIXELLAB_TOOL_GUIDE]] — tool capability reference
- [[feedback_chatgpt_pixellab_hybrid_workflow]] — workflow context
