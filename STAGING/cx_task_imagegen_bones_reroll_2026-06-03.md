# CX TASK — $imagegen RE-ROLL: bones_marker (tek asset, QC fail)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / STAGING / memory.

## Amaç
Batch-1'deki `bones_marker.png` QC'de ZAYIF çıktı: bulanık, düşük-kontrast, silüet okunmuyor. Tek bu asset'i `$imagegen` ile YENİDEN ÜRET — daha net, okunaklı, yüksek kontrast. Diğer 9 asset İYİ, onlara DOKUNMA.

## STİL KİLİDİ (Batch-1 ile aynı)
- Ortak prompt kuyruğu: `pixel art, true isometric 2:1 dimetric projection, clean readable outline, flat neutral lighting with ZERO baked shadows or glows, transparent background, limited palette slate stone #3A3D42 base`
- Sprite'a gölge/glow BAKE ETME. Background TRANSPARAN.
- Ölçek: 64px görünür-karakter çıpası (Warblade). bones ~1 world unit genişlik.

## ÜRETİLECEK — 1 asset
**bones_marker** — Hedef **64×48**, pivot center.
Prompt: `scattered humanoid skeleton remains — a clearly readable skull plus several rib and long bones, bright bone-white and pale-grey, HIGH CONTRAST against bare dark slate stone, crisp clean pixel outline, each bone distinct and recognizable, a failed-containment death marker, NO cyan, NO blur` + kuyruk.
- Kontrast vurgusu: kemikler bone-white (#E8E4D8 civarı), zeminden net ayrışsın. Bulanıklık YOK, her kemik ayrı okunsun.

## ÇIKTI KURALLARI
- `STAGING/imagegen/assets/bones_marker_raw.png` + `_keyed.png` + `_clean.png` + final `bones_marker.png` (ÜZERİNE YAZ — eski zayıf olanı değiştir).
- $imagegen 1024² verirse 64×48'e DOWNSCALE et (nearest/point), transparan koru.
- Final: RGBA, 64×48, transparan köşe, dolu alpha.
- Pencere/console flash'sız (background).
- `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`'de bones_marker satırını güncelle (re-roll notu).
- CODEX_DONE.md'ye kısa sonuç.

BLOCKED yaz: $imagegen ref-image desteklemiyorsa (metin-stil ile üret, belirt).
