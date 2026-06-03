# TASK: Cliff Placement Görsel Analiz (Antigravity)

ACTIVE RULES: (1) think before reviewing (2) min response, cite specific image areas (3) inline only (4) BLOCKED if image unreadable.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files / belirtilen screenshot dosyaları.

Amaç: RIMA cliff Tilemap placement görsel kalite analizi. User report: "bazı yerler aşırı taşıyo, bazı yerler güzel oturuyo". Hangi bölge taşıyor, hangi tipte taşma, sebep nedir, somut tweak önerileri.

## Screenshot Paths (oku ve incele)
1. `Assets/Screenshots/cliff_analysis_full.png` — Scene View, geniş açı, tüm sahne görünür
2. `Assets/Screenshots/cliff_analysis_game.png` — Game View, Player merkez, ana arena view

Önceki screenshot referansları (daha eski iterasyonlar — context için, ama yorum yeni 2'sine):
- `Assets/Screenshots/screenshot-20260526-020121.png` — önceki konfig, transformOffset.y=0.30
- `Assets/Screenshots/screenshot-20260526-020343.png` — sorting fix öncesi

## Mevcut Konfig
```
Sprite: 128×192 px, PPU 64 → world 2×3 unit
DeterministicVariantTile:
  spriteScale: (1.0, 1.0)
  transformOffset: (0, 1.5, 0)   ← 1.5 unit up, sprite üst floor cell içine girer
TilemapRenderer:
  Cliff: layer=Floor, order=-1   ← Floor sprite ardında render
  Floor: layer=Floor, order=0
Floor cellSize: (1, 0.609) unit
Cliff tilemap: 413 tile (her boş komşu cell için 1 tile, 8 direction check)
```

## Algoritma (CliffAutoPlacer.cs)
- Her floor cell için 8 yön komşu kontrol (S/N/E/W + 4 corner)
- Boş komşu varsa o cell'e cliff tile yerleştir
- DeterministicVariantTile variant pool (9 sprite, deterministic seed per cell)
- Tilemap.HasTile dedup safeguard

## Sorular (her biri için somut cevap)

### 1. Görsel Kalite Analizi
- Hangi bölgeler **güzel oturuyor**? (sağ kenar / sol kenar / köşeler / iç boşluklar)
- Hangi bölgeler **aşırı taşıyor**? Spesifik konum (ekran sağ üst, sol alt, vb.)
- Taşma tipi: drop face çok uzun mu / cliff sayısı çok yoğun mu / overlap mı?

### 2. Sebep Analizi
- TransformOffset.y=1.5 her bölge için uygun mu? Diagonal kenar vs yatay/dikey kenar farkı var mı?
- 8-direction placement corner cell'leri (NE/NW/SE/SW) doğru handle ediyor mu?
- Sprite oranı (2 cell width sprite) corner'larda nasıl davranıyor?
- 413 tile = floor 134 cell × ortalama 3 yön = normal, ama görsel olarak yoğun mu?

### 3. Önerilen Tweak (öncelik sırayla)
- Tek bir global offset tweak yeter mi? Hangi değer?
- Per-direction offset gerekli mi (S için farklı, N için farklı)?
- Spacing/spawn algoritması değişikliği gerekli mi (her 8 yön yerine 4 cardinal?)
- Sprite-specific scale ayarı gerekli mi (yanlardaki cliff width küçültme)?
- Acceptance criteria: "her bölge tutarlı oturur, drop face ~1-2 cell sarkma, no major overlap"

### 4. Karşılaştırma — Hades / Children of Morta / Diablo III
Bu referans oyunlarda cliff placement nasıl çalışıyor (perspective + density + sorting)? RIMA'nın mevcut yaklaşımıyla farkı? Pattern adopte edilebilir mi?

## Hard Constraints
- Inline only — dosya YAZMA
- Spesifik image bölgelerine referans ver ("üst sağ köşede", "sol alt kenarda")
- Speculative chaining yasak — net tweak değer önerisi
- "Bilmiyorum" demek serbest — uydurma

## Beklenen toplam uzunluk
500-800 kelime, max signal.
