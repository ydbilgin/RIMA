ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

5 konulu büyük research — AI pixel art üretim derinleştirme + tileset programları + wang tile DIY.

User talimatı (verbatim):
> "Bu tweette şunu diyolar 'o kadar haklı ki. AI kullanacaksanız bile bırakın promptu game artist girsin, fark inanılmaz oluyor.' Bunu nasıl profesyonelleştiririm promptları. Ayrıca [3 tweet URL] bunları inceler misin. Tileset programı neyi çözer, PixelLab ile nasıl kendi Wang stilimizi yaratırız, bunun basit bir matematiği var mı, AI yapabilir mi, ben basit şekilde nasıl yapabilirim bunları araştırt agy'ye."

## RIMA / LaurethStudio bağlam

- Unity 2022.3+, 2D top-down 3/4 (~70-80°), URP 2D Renderer
- PPU 64, tile 32×32, karakter 120×120, 8 yön (5 sprite + 3 flipX mirror)
- Aktif sahne Hades Elysium estetiği — yüzen ada + cliff edge + cyan rune + KitC 7-layer parallax
- PixelLab access: Create Image Pro / Map Object / Tiles Pro / Edit Image (init image AI Freedom 0.0-1.0)
- Aktif iş: RIMA Realtime Room Painter v0.1, manuel cliff brush + Cliff/Parallax sekmeli palet LIVE
- Tüm 2D illusion KB locked (32 teknik catalog), önceki agy reports STAGING'de mevcut

---

# Bölüm 1 — Game artist seviyesinde PixelLab prompt yazımı

User "AI kullansa bile promptu game artist yazmalı, fark inanılmaz" tweet'ine ikna oldu. Hedef: **şu an indie dev'in yazdığı amatör prompt** → **professional game artist seviyesinde prompt** dönüşümünü öğrenmek.

## Araştır

### 1.1 Profesyonel pixel artist prompt anatomisi
- AAA studio pixel artist'lerinin kullandığı dil (Discord/Twitter postlarından, ArtStation portföyleri, GDC talk'ları)
- "Subject + Style + Palette + Lighting + Composition + Constraints" yapı taşı sırası
- Olumsuz prompt (negative prompt) ne zaman kullanılır, hangi ifadeler artist-level
- Reference image kullanım stratejisi (init image + AI Freedom 0.0-0.4 range)
- Camera angle terimleri (3/4 top-down vs isometric vs orthographic vs front-facing)

### 1.2 Mevcut RIMA prompt'larından örnekleri analiz
NLM'de RIMA prompt'ları aranabilir. Veya STAGING içindeki agy reports'ta geçen prompt'lara bak (cliff_*.png, KitB, KitC üretim prompt'ları).

- Hangileri amatör seviyede? Niye? (örn "make a cliff" → çok generic)
- Hangileri pro seviyede? (örn "ancient ornate marble wall with top border visible, 3/4 perspective, elysium fantasy theme, pixel art, tileable horizontally")
- 3 örnek "BEFORE → AFTER" prompt rewrite göster

### 1.3 Profesyonel prompt template kütüphanesi (TOP 7)
LaurethStudio için reusable template'ler. Her biri aşağıdaki kategorilerden:
1. Top-down floor tile (32×32 PPU 64)
2. Cliff face (128×192 top-pivot, drop face)
3. Parallax bg layer (688×384 16:9, atmospheric)
4. Character idle (120×120, 4-direction)
5. Prop (small 64×64, isolated alpha)
6. VFX (cyan glow particle, 64×64, additive blend)
7. Tileable texture (seamless, 4-corner wrap)

Her template için:
- BEFORE (amateur)
- AFTER (pro game artist seviyesi)
- Ne fark eder (output görsel kalite diferansiyatör)

---

# Bölüm 2 — 3 tweet derin re-inceleme

Mevcut analiz dosyaları:
- `STAGING/x_posts_research_agy_2026_05_26.md` (aminerehioui + orb_3d, S108)
- `CODEX_DONE_eringijirou_review.md` (S109)

Şimdi **gerçek vision research** + daha derin. Video varsa frame-by-frame.

### 2.1 https://x.com/aminerehioui/status/2055785406315090062
**Isometric harita editörü.** Real-time iso terrain painter. Önceki analizde: sarı iso grid + sol panel yükseklik (0/1/2/3/RAMP) + sağ panel building/unit palette + mountain drag tool + tree paint + ramp tool + tema swap + minimap.

**Şimdi araştır:**
- Hangi engine (Unity? Godot? Custom JS?)
- Mesh/instancing vs Tilemap? (GameObject-Free claim'ini doğrula veya çürüt)
- Brush radius algorithm — flood fill? splat?
- Height system implementation hint (cliff face auto-spawn nasıl?)
- Tema swap mekanizması (texture atlas swap mı, shader LUT mı?)
- LaurethStudio Room Painter'a transfer edilebilir 3 net pattern

### 2.2 https://x.com/orb_3d/status/2043745118054940794
**World-space pixellated splat shader.** Pixel art orman + mor karakter + kürek + brush halka preview. Önceki analizde: world-space splat shader (R=çim, G=toprak, B=su channel), PPU snap floor(worldPos*64)/64, dairesel dalga grass regrowth.

**Şimdi araştır:**
- Tam shader yapısı (Shader Graph mi HLSL mi)
- Brush stroke → splat texture write akışı (RenderTexture mı GPU compute mu?)
- Multi-channel mask (R/G/B/A → kaç farklı terrain?)
- "Organic grass regrowth wave" — sin time + radius mı, particle mı?
- Splat texture resolution (1024×1024? 2048? scene size dependency)
- AI tool desteği var mı (orb_3d Twitter altında comment'lerde?)
- LaurethProc.Splat lib feasibility (RIMA için XS/M/L effort?)

### 2.3 https://x.com/eringijirou/status/2059224550718779767
**RPG Maker MV/MZ Map Auto-Decoration Plugin** (thirop/シロップ, BOOTH paid). Önceki analizde: Wang autotile + scatter + hybrid manual flow.

**Şimdi araştır:**
- BOOTH ürün sayfasında ek özellik listesi var mı (PreviewVideo demonstration)
- TRP_CORE.js + TRP_MapDecorator.js + TRP_MapObject.js — public source yok ama community reverse-engineering var mı?
- Plugin'in kullanıcı feedback'i (TwitLonger, RPG Maker forums)
- LaurethStudio.PainterSuite v1.2+ "Tilemap Decorator Painter" modülü için 5 concrete feature

---

# Bölüm 3 — Tileset programları ve neyi çözer

**Araştır:** modern pixel artist'in kullandığı tileset oluşturma araçları.

| Tool | Tip | USP | Pricing |
|---|---|---|---|

En az 8 tool karşılaştırması:
- **Pyxel Edit** (Windows/Mac, $9 — autotile, sub-tile)
- **Aseprite** (multi-platform, $19.99 — tile mode, brushes)
- **Tilesetter** (HTML5, $5 free demo — auto-tile gen)
- **Tilemap Town / Tilesetter Pro** — workflow
- **Tiled Map Editor** (free, .tmx — multi-layer)
- **Pyxel Edit + Tiled combo** — community pattern
- **GraphicsGale** (free Win — animation focus)
- **Tile Studio** (legacy free — tile-focused)

Her tool için **neyi çözer**:
- Wang autotile setup (manuel 16/47/256 tile çizimi yerine ne yapıyor)
- Sub-tile / corner / edge variantları
- Bitmask çıkarımı (Unity Rule Tile, Godot terrain export)
- Sprite atlas + collision data

---

# Bölüm 4 — Wang tile DIY matematiği (basit)

User şunu öğrenmek istiyor: **"Bunun basit bir matematiği var mı?"**

### Anlat (kısa, max 300 kelime)

1. **Wang tile nedir?** (köşe-eşleşmeli tile, komşular arası geçiş düzgün)
2. **2-corner vs 2-edge Wang** — fark
3. **Bitmask formülü:** `4-bit corner` veya `8-bit edge+corner` → tile index lookup
4. **Minimum tile sayısı:** 2-corner = 16 tile, 47-tile autotile, "blob" tile = 256
5. **Basit örnek tablo:** 16 tile için bitmask → görsel hangi köşeler "dolu/boş"

### Görsel örnek
ASCII art veya bullet list ile 16-tile Wang setinin bitmask haritasını göster:
```
0 (0000) → tüm köşeler boş
1 (0001) → sadece TL dolu
2 (0010) → sadece TR dolu
...
15 (1111) → tüm köşeler dolu
```

---

# Bölüm 5 — AI ile Wang tile üretimi + DIY workflow

### 5.1 AI yapabilir mi?

PixelLab + ChatGPT + Midjourney + diğer:
- **PixelLab Tiles Pro** — wang16 / wang47 / wang256 yapıyor mu? (NLM query veya official docs)
- **PixelLab Create Map Object** ile manuel wang üretim mümkün mü?
- **ChatGPT 4 Vision + DALL-E** wang tile çiziyor mu? Tutarlılık?
- **Stable Diffusion + LoRA** training mantığı — ne kadar effort?
- **Midjourney** pixel art wang? (typically poor, but check)

Her seçenek için:
- Quality (1-10)
- Effort (XS/S/M/L)
- Cost
- Tile-edge tutarlılığı (en önemli kriter — wang seamless gerekir)

### 5.2 Basit DIY workflow (user-friendly)

User için **3 adımlık basit yöntem** + her adım için talimat:
1. **Base sprite üretim** — PixelLab/AI ile 1 ana base tile (örn "stone path top-down 32×32 pixel art")
2. **Edge variantlar** — base'i init image olarak ver, "with mossy edge on left" / "with crack on right" prompt'larıyla 16 wang varyantı üret. AI Freedom 0.3-0.4.
3. **Manual cleanup + tilemap import** — Pyxel Edit veya Aseprite'ta seam'leri düzelt, Unity'ye Rule Tile olarak import.

### 5.3 Wang tile asset library önerileri
Hazır kullanılabilir wang setleri:
- OpenGameArt (CC0 wang sets)
- Kenney.nl pixel dungeon (16-tile basic)
- Tilesetter sample exports
- Pyxel Edit community packs

### 5.4 LaurethStudio için verdict

Final 3-line karar: AI üretim + manuel cleanup hibrit mi, full manual mı, hazır asset adapt mı?

---

# Çıktı

Markdown, max **2000 kelime** (büyük catalog/research). Bölümler net başlık altında. Web search izinli.

**TOP 5 ACTIONABLE for RIMA bugün:**
- Bölüm 1'den 1: en kritik pro-prompt template
- Bölüm 2'den 1: tweet'lerden en uygulanabilir pattern
- Bölüm 3'ten 1: hangi tool şu an indirilmeli
- Bölüm 4'ten 1: wang formülü TL;DR
- Bölüm 5'ten 1: bugün başlanacak adım

Hedef: User yarın bu doc'a bakıp "tamam, şu işi yapacağım" diyebilsin.
