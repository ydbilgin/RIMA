ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç — kısa follow-up

Önceki dispatch (`b0xpgeskz`) Bölüm 1-3 raporladı (pro prompts + 3 tweet + tileset programları). Bölüm 4-5-Final TOP 5 yarıda kesildi. Bu bölümleri tamamla.

## Bağlam — daha kısa

- RIMA: Unity 2022.3+, 2D top-down 3/4, PPU 64, tile 32×32
- PixelLab: Create Image Pro, Map Object, Tiles Pro, Edit Image (init image + AI Freedom 0.0-1.0)
- LaurethStudio Room Painter LIVE (Day 1-3, manuel cliff brush + sekmeli palet Pattern C)
- Önceki Bölüm 3 tileset programları analiz LIVE (Pyxel Edit, Aseprite, Tilesetter, Tiled vb.)

## Görev — sadece şu 3 bölüm

### Bölüm 4 — Wang Tile DIY Matematik (max 400 kelime)

User şunu öğrenmek istiyor: "Bunun basit bir matematiği var mı?"

1. **Wang tile nedir?** (köşe-eşleşmeli tile, komşular arası geçiş düzgün) — 1-2 cümle
2. **2-corner vs 2-edge Wang farkı** — fark + hangisi RIMA için
3. **Bitmask formülü:** `4-bit corner` veya `8-bit edge+corner` → tile index lookup
4. **Tile sayıları:** 2-corner=16, 47-tile autotile (Aseprite/Godot standard), "blob" tile=256 — neden bu sayılar
5. **16-tile bitmask örnek tablosu (ASCII):**
```
Index | Binary | TL TR BL BR | Görsel anlamı
  0   |  0000  |  .  .  .  . | tüm köşeler boş
  1   |  0001  |  .  .  .  X | sadece BR (sağ-alt) dolu
  ...
  15  |  1111  |  X  X  X  X | tüm köşeler dolu
```
Tüm 16 satırı eksiksiz göster.

### Bölüm 5 — AI ile Wang Tile Üretimi + DIY Workflow (max 500 kelime)

#### 5.1 AI yapabilir mi? — kıyaslama
Şu 4 seçenek için Quality(1-10) / Effort(XS/S/M/L) / Cost / Tile-edge tutarlılık:
- **PixelLab Tiles Pro** (4-type top-down tileset native support — varsa)
- **PixelLab Map Object + Edit Image** (init image base + edge variant prompts)
- **ChatGPT 4 + DALL-E** (tek-shot wang üretim, tutarlılık zayıf)
- **Stable Diffusion + LoRA** (custom training, M effort, en yüksek tutarlılık)

#### 5.2 Basit 3-adımlık DIY workflow (User için)

Step 1: **Base sprite** — PixelLab Create Image Pro ile 1 base tile (örn `"seamless top-down stone path tile, 32x32, pixel art, --no organic edge"`)

Step 2: **16 wang varyantı** — base'i init image olarak ver, AI Freedom 0.3-0.4. 16 prompt:
- `"with mossy edge on TL corner"`, `"with mossy edge on TR corner"` vb.
- HER köşe kombinasyonu için ayrı prompt
- Veya 1 prompt ile 16-grid sheet: `"16 tile sprite sheet, all wang corner variants, 4x4 grid, pixel art, --keep base style consistent"`

Step 3: **Cleanup + Unity import**
- Pyxel Edit veya Aseprite'ta seam'leri düzelt (~10 dk per tile)
- Unity'de Rule Tile asset oluştur, 16 sprite slot doldur, bitmask kuralları gir
- Tilemap'e at, test et

#### 5.3 Hazır wang asset kaynakları (TOP 3 free)
- OpenGameArt CC0 wang sets (link)
- Kenney pixel dungeon (16-tile basic)
- Tilesetter sample exports

#### 5.4 LaurethStudio için 3-cümle verdict
AI üretim + manuel cleanup hibrit mi, full manual mı, hazır asset adapt mı?

### Bölüm Final — TOP 5 ACTIONABLE for RIMA bugün

Önceki raporla birlikte (Bölüm 1-3 + bu Bölüm 4-5):
- Bölüm 1'den 1: en kritik pro-prompt template
- Bölüm 2'den 1: tweet'lerden en uygulanabilir pattern
- Bölüm 3'ten 1: hangi tool şu an indirilmeli
- Bölüm 4'ten 1: wang formülü TL;DR
- Bölüm 5'ten 1: bugün başlanacak adım

## Çıktı

Markdown, ~1300 kelime toplam. Web search izinli.

Önemli: bu önceki dispatch'in **devamı**. Bölüm 1-3'ü tekrar etmeyin, sadece 4-5-Final.
