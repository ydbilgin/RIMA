# EXEC 5 — imagegen On-Brand Placeholders (CX, laurethayday imagegen)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical — only generate + import the 3 listed assets (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"`

## Amaç
Demo map'in 3 eksik görsel parçasını imagegen ile ON-BRAND üret + Unity import + mevcut placeholder prefab'lara bağla. Bunlar PixelLab DEĞİL (karakter değil, environment chrome). Çıktıyı `CODEX_DONE.md` → `## EXEC5 IMAGEGEN` başlığına yaz.

## ON-BRAND STİL (HARD — uyma zorunlu)
- İzometrik / top-down 3/4 uyumlu, **PPU 64**, transparent PNG, point-sample temiz kenar.
- Palet: koyu bazalt/demir/taş + cyan-blue (#00FFCC) AZ aksan (rift/rune ışığı). **YASAK:** realistic/photoreal, gloss, gradient, gold, text, karakter, drop-shadow bake.
- Her çıktı = PLACEHOLDER → `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`'ye logla (sonra PixelLab ile değişebilir).
- imagegen pack temiz cell-split + transparent olmalı (magenta-bake ETME). Tek-tek sabit-canvas tercih.

## ÜRETİLECEK 3 ASSET (ax+cx prompt birleşik)
1. **Portal/Gate** (128×128): ruined basalt arch / iron-banded threshold, kırık taş taban, açıklıkta ince cyan rune çizgisi, top-down 3/4 okunur. 2 varyant: closed gate + active/open portal (cyan parlak).
2. **Map fragment** (64×64): yırtık parşömen VEYA oyulmuş slate shard + cyan route glyph, asimetrik silüet, küçük boyutta okunur. 2 varyant: idle + selected/glow.
3. **Reward pickup** (64×64 veya 96×96): küçük relikari/cache/kristal, cyan çekirdek + koyu metal çerçeve. 2 varyant: idle + sparkle/glow.

## IMPORT + BAĞLAMA
- İndir → `Assets/Sprites/Environment/Placeholders/` (yeni klasör). Import: PPU 64, Sprite (2D), Point filter, transparent, uncompressed.
- Mevcut placeholder prefab'lara sprite bağla (cx detect etti, sprite={fileID:0}):
  - `Assets/Prefabs/MapFragment.prefab` → map fragment idle sprite
  - `Assets/Prefabs/RewardPickup.prefab` → reward idle sprite
  - (Chest.prefab varsa reward ile aynı veya ayrı)
- `PlaceholderSprite` component'i varsa, gerçek sprite atanınca devre dışı bırak/temizle (sadece sprite ata yeterli olabilir — kontrol et).
- Portal/gate prefab'ı YOKSA sadece sprite'ı üret + import et + registry'ye `portal` tag'iyle girsin (baker root'a Placeholders/ ekle VEYA AssetPackSO'ya entry). Prefab kurma (Opus sonra demo'ya bağlar).

## DoD
1. 3 asset (6 varyant) üretildi + Placeholders/'a import (doğru import ayarı).
2. MapFragment + RewardPickup prefab'larına sprite bağlandı.
3. PLACEHOLDER_REGISTRY güncellendi.
4. Portal sprite registry'de `portal` tag'i alacak şekilde ayarlandı (baker root veya AssetPackSO).
5. Compile-clean (Unity AÇIK — build deneme). Değişen dosyalar + BLOCKED.
