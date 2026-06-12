ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
3 eksik run-map node sembolünü üret + import. Mevcut 8 sembolle BİREBİR eşleş. PixelLab DEĞİL — `$imagegen` skill (şeffaf, istenen boyut).

# OKU
- `STAGING/_process/2026-06/TASK_import_node_symbols_2026-06-11.md` — mevcut 8 sembolün import ayarları (PPU/filter/atlas). AYNISINI uygula.
- Mevcut referans: `Assets/Sprites/UI/MapNodes/combat.png` (64×64 RGBA), atlas `UI_MapNodes.spriteatlas`.

# ÜRET ($imagegen skill, şeffaf)
3 sembol, her biri **64×64 transparan PNG**, stil = **monochrome slate-grey pixel-art icon, high top-down view** (mevcut 8 ile aynı görsel dil):
1. `rest.png` — kamp ateşi VEYA çadır (dinlenme noktası)
2. `unknown.png` — soru işareti (bilinmeyen oda)
3. `player.png` — konum/işaretçi (oyuncunun şu anki düğümü)

Alfa DOĞRULA: arka plan tam şeffaf (magenta/beyaz artık YOK). Cleanup gerekirse chroma-key.

# IMPORT
- `Assets/Sprites/UI/MapNodes/` altına 3 PNG.
- Unity importer: Sprite (2D and UI), Single, PPU 64, Point filter, Compression None, MipMaps OFF, Alpha Is Transparency ON (mevcut 8 ile aynı — doğrula).
- `UI_MapNodes.spriteatlas`'a 3 yeni sprite'ı packable ekle.
- `read_console` 0 error.

# COMMIT
- `feat(ui): import 3 remaining map node symbols (rest/unknown/player) via imagegen + atlas`
- CODEX_DONE.md'ye: üretildi mi, alfa temiz mi, import ayarları doğru mu, atlas güncel mi, commit hash.

# YAPMA
- Mevcut 8 sembole dokunma. Kod değişikliği YOK (sadece asset + atlas + meta).
