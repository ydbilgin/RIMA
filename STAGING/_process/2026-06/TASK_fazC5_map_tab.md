ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ C5 — Director "Map" sekmesini doldur. Node grafiği göster + node'a atla + seed reroll. GATE: derleme 0 error + jump/reroll çalışıyor. Görsel doğruluk SABAHA.

# ⚠️ İLK İŞ — MAP/GRAPH HOOK DOĞRULA (tahmin etme)
- Run-map/dungeon node grafiği sistemi nerede? Grep: `DungeonGraph`, `RunMap`, `MapNode`, `Generate`, `JumpToNode`, node graph üreten/yöneten sınıf.
- SANDBOX §4 Map hook'u: `DungeonGraph.Generate`, `JumpToNode`. Bu metodlar/sınıf VAR MI? İmzaları ne?
- Mevcut node sembol asset'leri: `Assets/Sprites/UI/MapNodes/` (combat/elite/boss/shop/chest/forge/event/curse_gate/rest/unknown/player — 11 sembol, atlas `UI_MapNodes.spriteatlas`). Grafik bunları kullansın.
- Hook net DEĞİLSE → CODEX_DONE'a BLOCKED yaz, ne eksik belirt, COMMIT ETME. Yeni map/graph sistemi YAZMA.

# OKU
1. `Assets/Scripts/UI/DirectorMode.cs` — Faz B/C1/C2/C3/C6. Panel: Map (boş CanvasGroup). C3 kalıbı (chrome/font/Loc).
2. Map/graph sistemi (yukarıda araştır) — Generate/Jump/seed API.
3. `Assets/Sprites/UI/MapNodes/UI_MapNodes.spriteatlas` — 11 node sembolü (tip→sprite eşleme).
4. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` §4 Map + §3 (node `node_frame`, büyük aksiyon Jump/Reroll `ribbon_base`).
5. `Loc.cs`.

# İŞ — Map sekmesi (core, min)
- **Node grafiği:** mevcut run-map'i göster — node'lar (tip→MapNodes sprite), bağlantılar (çizgi). Mevcut grafik verisini OKU (yeni üretme, varsa).
- **Seç→atla:** node'a tıkla → `JumpToNode` (doğrulanan hook) ile o node'a geç.
- **Reroll seed:** `Generate` (doğrulanan hook) yeni seed'le yeniden üret.
- Chrome/font/Loc C3 kalıbı.

# GATE + COMMIT
- `read_console` 0 error.
- UnityMCP doğrula (Play): Director→Map → grafik görünüyor → node'a tıkla→jump çalışıyor → reroll yeni grafik. (execute_code assert.)
- CombatContract `run_tests` 3/3 bozulmamalı.
- Geçerse commit: `feat(director): Faz C5 Map tab — node graph + jump + seed reroll [visual unverified]`. BLOCKED ise COMMIT ETME, sebep yaz.
- CODEX_DONE.md: map hook ne çıktı (imza), doğrulama, derleme/test, commit hash veya BLOCKED.

# YAPMA
- Diğer sekmeler YOK. Yeni map/graph/seed sistemi YOK — mevcut API'leri çağır. Hook belirsizse BLOCKED. Node sembollerine/atlas'a dokunma (sadece kullan).
