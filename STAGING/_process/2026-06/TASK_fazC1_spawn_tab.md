ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ C1 — Director "Spawn" sekmesini doldur. Enemy palette → tıkla-koy (sınırsız) + ghost preview + sağ-tık sil. GATE: derleme 0 error + tıkla→mob spawn oluyor. Görsel doğruluk SABAHA.

# ⚠️ İLK İŞ — SPAWN HOOK DOĞRULA (tahmin etme)
SANDBOX §7: `SpawnEnemy(id,pos)` imzası DOĞRULANAMADI. ÖNCE araştır:
- `Assets/Scripts/Encounter/EncounterController.cs`, `ThreatBudget.cs` (Spawn wave-tabanlı), enemy prefab registry / `MobDefinition` SO, mob prefab path'leri.
- Tek-mob-by-id-at-position spawn yolu VAR MI? Varsa REUSE. Yoksa: Director-only minimal helper (mob prefab registry'den Instantiate + pozisyon). 
- **Spawn yolu net DEĞİLSE → CODEX_DONE'a BLOCKED yaz, hangi bilgi eksik belirt, COMMIT ETME.** Yeni encounter sistemi YAZMA.

# OKU
1. `Assets/Scripts/UI/DirectorMode.cs` — Faz B/C2/C3. Panel: ContentArea > Spawn (boş CanvasGroup). C3/C2 kalıbı (chrome/font/Loc). DirectorMode kamera + screen→world dönüşümü.
2. Enemy tanım kaynağı (yukarıda araştır) — palette bu listeden.
3. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` §4 Spawn + §5 (ghost ZORUNLU, grid-snap, sağ-tık sil, "pop" anim).
4. `Loc.cs` — `Loc.T()`.

# İŞ — Spawn sekmesi (core, min)
- **Enemy palette grid** (`slot_normal` hücreler): mevcut enemy/mob tanımlarından. Seç → aktif "yerleştirilecek" tip.
- **Tıkla-koy:** Director modda dünya tıklaması → seçili mob'u o pozisyonda spawn (doğrulanan hook). SINIRSIZ.
- **Ghost preview ZORUNLU:** fare dünya pozisyonunda yarı-saydam önizleme + grid-snap. Yerleşince "pop".
- **Sağ-tık sil:** imlecin altındaki spawn'lanan mob'u sil.
- (Opsiyonel, basitse) wave preset butonu (Triple/Clear) — değilse ERTELE, BLOCKED değil.
- Chrome/font/Loc C3 kalıbı.

# GATE + COMMIT
- `read_console` 0 error.
- UnityMCP doğrula (Play, _Arena): Director→Spawn → palette seç → dünya tıkla → mob spawn oldu (execute_code: enemy count arttı). Sağ-tık → silindi. Ghost görünüyor.
- CombatContract `run_tests` 3/3 bozulmamalı.
- Geçerse commit: `feat(director): Faz C1 Spawn tab — enemy palette click-place + ghost + right-click delete [visual unverified]`. BLOCKED ise COMMIT ETME, sebep yaz.
- CODEX_DONE.md: spawn hook ne çıktı (reuse mu yeni helper mı + imza), doğrulama, derleme/test, commit hash veya BLOCKED.

# YAPMA
- Diğer sekmeler YOK. Dummy AI mode / hitbox / wave-preset detayı = Faz D/sonra (core spawn'a odaklan). Yeni encounter/AI sistemi YOK. Hook belirsizse BLOCKED.
