# RIMA LoRA Training — Start / Stop / Resume Guide

## Auto-resume confirmed
ai-toolkit'in source code'unda doğrulandı (`BaseSDTrainProcess.get_latest_save_path()`):
- Training her **500 step'te** checkpoint kaydeder → `F:/AI/ai-toolkit/output/rima_pixel_style_v1/*.safetensors`
- Training kapatıldığında en son checkpoint diskte kalır
- **Aynı config ile yeniden çalıştırınca otomatik olarak en son checkpoint'ten devam eder**
- Step counter + metadata da restore edilir (gerçek devam, baştan değil)

## GPU verify — onaylı
- RTX 5080 detected
- VRAM 95% usage (15475/16303 MiB)
- "Loading checkpoint shards", "Quantizing transformer", "Loading weights 219/219" = GPU operations
- CUDA 13.2 driver, CUDA 12.8 torch

## Komutlar

### Status check (her zaman):
```powershell
F:\AI\training\rima_train.bat status
```
Gösterir: mevcut checkpoint'ler, son sample image'lar, GPU kullanımı.

### Durdur:
```powershell
F:\AI\training\rima_train.bat stop
```
Veya manuel:
```powershell
Get-Process python | Where-Object {$_.Path -like '*ai-toolkit*'} | Stop-Process -Force
```

### Başlat / Resume:
```powershell
F:\AI\training\rima_train.bat
```
- İlk çalıştırma → baştan başlar
- Sonraki çalıştırma → en son checkpoint'ten otomatik devam

## Checkpoint dosyaları
Path: `F:/AI/ai-toolkit/output/rima_pixel_style_v1/`

Format:
- `rima_pixel_style_v1_000000500.safetensors` (500. step LoRA)
- `rima_pixel_style_v1_000001000.safetensors` (1000. step LoRA)
- ...
- `samples/` (her 500 step'te üretilen 8 prompt × 1024×1024 sample image)

`max_step_saves_to_keep: 4` — sadece son 4 checkpoint disk'te tutulur (eskisi silinir).

## Hangi checkpoint'i kullanmalı?

| Step | Durum | Kullan? |
|---|---|---|
| 500 | Erken — style learning başlıyor | Test için |
| 1000 | Style oturmaya başladı | Test için |
| 1500-2000 | **Style LoRA peak zone** | ✅ Yes |
| 2500 | İnce ayar | ✅ Yes |
| 3000 | Final — bazen overfit | Test edip seç |

Style LoRA'lar genelde 1500-2000 step'te en iyi sonucu verir. 3000'e gerek yok aslında, ama opsiyon var.

## LoRA test etmek (training devam ederken)
ComfyUI'de mevcut LoRA loader node ile herhangi bir checkpoint test edilebilir:
- Path: `F:/AI/ai-toolkit/output/rima_pixel_style_v1/rima_pixel_style_v1_000001500.safetensors`
- Prompt: `rima_style, pixel art dungeon room, ...`
- ComfyUI Flux dev workflow'una LoRA Loader ekle

## Troubleshooting

**Training hızı yavaş?** GPU util %50'nin altında: dataset cache bottleneck. `cache_latents_to_disk: true` yardımcı, ilk epoch sonra hızlanır.

**OOM hatası?** VRAM doluyor. Şu an `low_vram: true` aktif. Daha agresif: `linear: 8` (rank düşür) veya `resolution: [512]` (sadece 512).

**Checkpoint yok ama training fail oldu?** İlk 500 step'e ulaşmadan crash → step 0'dan başlar.

## Şu an aktif
- Background ID: `brgt6ky5m`
- Started: 2026-05-23 (PixelWave restart)
- Config: `rima_pixel_style_v1.yaml`
- **Base model: `mikeyandfriends/PixelWave_FLUX.1-dev_03`** (pixel-art-tuned Flux dev)
- Dataset: 335 images (PixelLab-only, upscaled to min 256×256)
- Steps: 2000, save every 500 (4 checkpoints)
- EMA: **disabled** (RAM tasarrufu)
- low_vram: true (5080 16GB için zorunlu)
- ETA: ~2-2.5 hours (PixelWave download + training)

## Why PixelWave base?
PixelWave_FLUX.1-dev_03 = Flux dev'in pixel art için fine-tune'lanmış full versiyonu (1199 downloads, 194 likes — en popüler Flux pixel art model). Bunun üstüne RIMA-spesifik LoRA train etmek = "fine-tuning on existing pixel art expertise" → daha az step (2000 vs 3000), daha iyi sonuç. Final LoRA hem PixelWave'in pixel art skill'ini hem RIMA'nın dark fantasy style'ını kapsar.

## RAM/VRAM beklenen kullanım
- VRAM: 14-16 GB (sınır, beklenen)
- RAM: 22-26 GB (low_vram offload, EMA disable sonrası iyileşti)
- Disk space: ~30 GB (Flux models + cache + checkpoints)
