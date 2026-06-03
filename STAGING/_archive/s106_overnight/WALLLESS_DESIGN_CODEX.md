# Codex Cross-Validation: Wall-less Top-Down Game Design

ACTIVE RULES: (1) think before answering (2) min words, no fluff (3) engineering-first perspective (4) BLOCKED if uncertain.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: User RIMA için "duvarsız oyun" (wall-less, sadece tile + harita objeleri ile boundary) fikrini değerlendiriyor. Antigravity (Gemini 3 Pro) paralel olarak industry research yapıyor. Sen Codex (gpt-5.5) olarak code/engineering perspektifinden cross-validate et — Opus + Codex birlikte karar verecek.

## RIMA mevcut durum (önemli)
- 2D top-down ARPG, 35° iso, PPU 64, 8-yön karakter, 120×120 chibi
- **Mevcut wall sistemi tamamı çalışıyor:** S104'te V2 namespace P0 fix DONE (14 placeholder prefab, 4 sheet PixelLab walls, WallChainRoomBuilder, RoomPainterWindow)
- Live scene: `Assets/Scenes/Test/PlayableArena.unity` — 352-cell iso floor, walls NONE, player WASD verified
- 26-object inventory planlandı: Cyan Rift Archway, Monolithic Column, Wall Torch, Granite Altar, Brazier (P0)

## SORU (4 alan)

### 1. Engineering cost karşılaştırması
- **Wall path:** PixelLab wall prefab gen (12 piece minimum) + WallChainRoomBuilder + collision tuning. Mevcut durum: ~80% complete (S104 sonrası).
- **Wall-less path:** Object prefab gen (~10-15 prop) + per-prop collider + procgen distribution + readability test. Mevcut durum: ~30% (sadece taxonomy var, prop'lar yok).
- Hangisi 2-3 hafta içinde "shippable demo" verir?

### 2. Procgen mantığı
- Wall-based: Room shape → wall chain → fill — algoritmik basit
- Object-based: Walkable area → boundary detection → prop placement satisfying density+readability — algoritma daha karmaşık
- Hangi yaklaşım Unity'de daha az custom code ile gelir? (Tilemap2D + GridLayout assumption ile)

### 3. Combat gameplay impact
- Walls: knockback wall-slam, cover, line-of-sight blocking — combat dynamic'in parçası
- Wall-less: Objects daha küçük → cover %30-50 daha az, knockback hedefi yok
- RIMA combat (Warblade melee + ranged classes) wall-less'tan zarar görür mü?

### 4. Antigravity ile farklı görüşün varsa belirt
- Antigravity büyük ihtimal "wall-less premium görünür, Hades-style hibrit yap" diyecek
- Sen kod tarafından bakarak agree veya disagree et — neden?
- Net verdict ver: "RIMA için doğru yön = WALLS / WALL-LESS / HYBRID (specify)"

## Çıktı formatı
- ~500 kelime
- 4 başlık altında bullet
- Final satır: `VERDICT: <walls|wall-less|hybrid:<detail>>`
- Codex transcript otomatik `CODEX_DONE_<profile>.md`'ye yazılacak — ekstra dosya yazma
