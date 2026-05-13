# Codex Review: Tile Generation Tool Seçimi (S66)

**Tarih:** 2026-05-13
**Tip:** REVIEW + FINAL DECISION (kod yazma yok, dosya değişikliği yok)
**Çıktı:** CODEX_DONE.md'ye markdown raporu

## Bağlam

Kullanıcı room designer'ı test etmek istiyor ve PixelLab'dan "doğal görünümlü tile" üretmek için hangi tool'u kullanması gerektiğini soruyor.

Orchestrator (Sonnet) ilk olarak `create_tiles_pro` (PixelLab MCP) önerdi. Sonra NLM'den bağlam çekildi → mevcut LOCKED kararlarla çelişme tespit edildi. Opus (rima-design) bu çelişkiyi değerlendirip karar verdi. Şimdi senin (Codex) bağımsız review'in + final kararın gerekiyor.

## NLM Bulguları (LOCKED Kararlar)

- **Karar #75 (2026-05-04):** PixelLab Map Workshop tool'ları (`create_topdown_tileset` vb.) YASAK. Discord deneyimi: autotile/connected çıktı vermiyor, ayrık tile veriyor. Önerilen: tekil tile + Unity RuleTile birleştirme.
- **Karar #90:** PixelLab Batch Economy — 32x32 sprite → 64 cell tek generation (CFSR tool).
- **Karar #106:** PixelLab MCP üretim için kullanılmaz (reference image yok), web UI kullanılır.
- **S60 Pipeline (LOCKED):** Tile için `create_image_pixen NEW (S-XL)` (web UI) + manuel cleanup + Unity import (PPU=64, Point filter).

## Mevcut Pipeline Bileşenleri

- `Assets/Editor/RoomDesigner/FloorVariantPainter.cs` — Perlin 3-katman bake
- `Assets/Editor/RoomDesigner/WallAutoConnect.cs` — 4-bit NSEW mask → 8 wall variants
- `TileImportWizard` (S60, 568 ln, commit'li) — PixelLab PNG → Unity Tile asset
- Tema: Vivid Vulnerability (Karar #77), Act 1 Shattered Keep — zemin #2C2A2A, duvar #4A3F3F, mavi rift #7BA7BC

## Opus Kararı (rima-design, review için)

> KARAR: `create_image_pixen NEW (S-XL)` (web UI) — single 32x32 izole tile, pilot-then-batch.
> `create_tiles_pro` MCP tool'unu **kullanma**.
>
> GEREKÇE:
> 1. Karar #75 ruh olarak `create_tiles_pro`'yu da kapsar — yasağın gerekçesi tool ismi değil davranış: PixelLab tile tool'ları autotile/connected vermiyor, ayrık döküyor. "Pro" engine bu temel sınırlamayı (RuleTile-uyumlu NSEW mask) çözmüyor.
> 2. Karar #106 MCP'yi üretim için zaten dışlıyor — reference image yok, Vivid Vulnerability paleti tutturulamaz.
> 3. S60 pipeline LOCKED — TileImportWizard + WallAutoConnect + FloorVariantPainter zinciri çalışıyor. Yeni tool zinciri kırar.
> 4. RuleTile autoconnect Unity tarafında çözülüyor — PixelLab'dan beklenen tek şey temiz, izole, palet-uyumlu 32x32.
> 5. Karar #90 batch ekonomisi single-tile akışla uyumlu.
>
> RİSK: `create_tiles_pro` belki gerçekten autotile veriyordur. **Mitigation:** Locked pipeline'ı bozmadan paralel pilot ile test et, kararı şimdi revize etme.
>
> NEXT STEP: 1 pilot tile, `create_image_pixen NEW (S)`, prompt "32x32 top-down floor tile, dark rubble stone #2C2A2A, hairline cyan rift cracks #7BA7BC, no border, transparent bg, Salt and Sanctuary chibi-serious style". Aseprite cleanup → TileImportWizard → test scene'de WallAutoConnect dikiş kontrolü. Geçerse 8x8 batch.
>
> AÇIK SORU:
> 1. `create_tiles_pro` gerçek davranışı NLM'de yok — Discord/PixelLab dokümanından doğrulanmalı.
> 2. Pixen NEW boyut seçeneği (S/M/L/XL) 32x32 için optimal hangisi.

## Senin Görevin (Codex)

Bağımsız review yap — Opus'un akıl yürütmesini sorgula, mevcut LOCKED kararlarla çelişki/uyum durumunu doğrula, kendi final kararını ver.

**Çıktı formatı (CODEX_DONE.md'ye yaz):**

```
# Codex Final Decision: Tile Tool Seçimi

## Verdict
[PASS / REVISE / REJECT] Opus'un kararı

## Reasoning
[3-6 madde — neden PASS/REVISE/REJECT. LOCKED kararlarla cross-ref. Eğer REVISE/REJECT ise alternatif öner.]

## Final Decision (binding)
[Tool: ...]
[Workflow: pilot → batch → cleanup → Unity import]
[Açık riskler ve nasıl çözülecek]

## Actionable Next Step (concrete)
1. [Specific komut/aksiyon]
2. [İkinci adım]
3. [...]

## Disagreements with Opus (varsa)
[Spesifik nokta, gerekçe]

## Open Questions Still Unresolved
[Kullanıcıya/orchestrator'a soruluacak şey varsa]
```

## Kısıtlar

- **Kod yazma, dosya değiştirme, commit YOK.** Sadece review ve karar dokümanı.
- LOCKED kararları override etme — sadece doğru yorumlandı mı değerlendir.
- PixelLab MCP tool davranışı hakkında belirsizlik varsa "doğrulanmalı" notu düş, kullanıcıya soruluacak.
- Türkçe yaz.
- CODEX_DONE.md'ye yaz, üzerine yazma — sonuna append et (yeni bölüm başlığı ile).

Effort: high.
