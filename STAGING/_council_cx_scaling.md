ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
"Pixel-art küçük üret (32/64px) → Unity'de scale et, kalite kaybı olmadan mümkün mü + algoritma" sorusunun ENGINE/UNITY-FEASIBILITY + RIMA-PRAKTİK lensi. ANALİZ ONLY. Sonucu profil-DONE'a yaz.

# READ
STAGING/PIXELART_SCALING_BRIEF_2026-06-04.md. Ayrıca RIMA'da MEVCUT pratiği kontrol et: PPU64 kuralı (.claude/PROJECT_RULES.md), Pixel Perfect Camera kullanımı (grep `PixelPerfectCamera` / `PixelPerfect`), karakter sprite import metaları (`Assets/Resources/Characters/*/*_idle_south.png.meta` PPU/filter), CameraZoom.cs (integer pixel-ratio zoom — `Assets/Scripts/Camera/`), tile/cellSize.

# Answer (concrete, cite RIMA files/settings)
1. Unity'de pixel-art scale: INTEGER (2x/3x/4x) nearest-neighbor neden crisp; non-integer neden bozuk/shimmer. RIMA'nın Pixel Perfect Camera + PPU64 + integer-zoom (CameraZoom.cs) setup'ı bu kuralı zaten uyguluyor mu?
2. Asset üretim res'i: RIMA karakteri "64px görünür" (canvas 120px [[project-character-64px-canvas-large-for-animation]]) — native küçük üretip integer-scale mı, yoksa? Tile 32px, VFX, UI-icon 64px için doğru native res + import (PPU/Point/Compression/FilterMode) ne?
3. AI-gen gerçeği: PixelLab native-küçük gerçek-pixel verir (bu stratejiye UYGUN); Imagen/imagegen 1024² büyük "pixel-style" verir → DOWNSCALE(nearest)+cleanup gerekir (bu stratejinin TERSİ). RIMA hangi asset'i hangi araçla üretmeli (mevcut karar dosyaları ASSET_PIPELINE_DECISION/SPELLVFX_SKILLICON ile tutarlı mı)?
4. Unity import + Pixel Perfect Camera + runtime-zoom için EN AZ-friction reçete (gerçek RIMA değerleriyle). Var olan pixel_cleanup (Tools/pixel_cleanup) downscale/snap'te nasıl yardımcı?
5. RIMA'da şu an yanlış yapılan/iyileştirilebilecek bir şey var mı (örn non-integer scale eden bir yer, yanlış filter, room_bg PPU)?

Terse, cite RIMA paths/settings.
