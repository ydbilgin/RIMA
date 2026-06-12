# CURRENT_STATUS

## ⏯️ RESUME (2026-06-12 — review-fix planı HAZIR, uygulama + ChatGPT batch yeni session'da)

**⚠️ MODEL:** Orchestrator=Opus. Writer=cx dispatch, reviewer=council (writer≠reviewer).

**🖼️ GÖRSEL SKILL'LERİ (bu session yeniden düzenlendi):**
- `/agy_image` = ax→agy (Imagen), 1024² opak. `/codex_image` = cx dispatch→Codex gpt-image-2. (eski `generate_image` + `codex-images` skill'i KALDIRILDI). Detay → [[feedback_image_skill_naming]].
- **KANIT:** bare `codex exec` / `cx <profil> exec` passthrough Windows'ta non-interactive **asılı kalıyor**; **`cx dispatch` tek güvenilir codex yolu** (smoke test: dispatch 24s, bare 5dk+ hang). Görsel kıyas: `STAGING/imagegen/_compare_2026-06-12/` (Imagen + gpt-image-2 ikisi de iyi; gpt-image-2 top-down çerçeve daha iyi).

**🔍 CHATGPT OVERNIGHT REVIEW → COUNCIL → KARAR (bu session):**
- ChatGPT review paketi geldi (`STAGING/_inbox/chatgpt_overnight_review_2026-06-12/`), Claude gerçek kodla **5 bulgu CONFIRMED**, council (cx+3.1Pro+Flash) risk-denetledi.
- **KARAR DOSYASI:** `STAGING/OVERNIGHT_REVIEW_FIX_DECISION_2026-06-12.md` (uygula sırası + test-kırılma + 3.1Pro'nun ekstra E1/E2'si).
- **UYGULANMADI** — yeni session'da: A1 finisher≠crit · A3 packet bypass (Ranger+HeatGauge×2) · A2 lean defender-stat helper · B1 zero-damage (**+ `HealthTests.cs:66` assert ÇEVİR**) · B2/E1/E2 TODO. Tek cx dispatch + CombatContract gate. **A4 Director raycast = Play-mode'da doğrula, kör commit YOK.**

**📦 SONRAKİ ADIM (yeni session):**
1. **Review-fix dispatch** (yukarıdaki plan, karar dosyasından).
2. **ChatGPT batch review:** `STAGING/CHATGPT_BATCH_REVIEW_PACKAGE_2026-06-12.md` (4 batch, tek tek). C4 numeric-tablo doğrulaması Batch 3'te.
3. **Görsel playtest:** Director aç (` tuş) → B + C1/C2/C3/C6.
4. Kalan: **C5 Map** (öneri child-choice nav) · **C4 Build** (PaintCell refactor riskli) · HUD Layout · Faz D · Loc TR (ı/ğ/ş).

**Durum:** 22 commit push'lı (`github.com/ydbilgin/RIMA`). Gece detay → `STAGING/AUTONOMOUS_RUN_2026-06-12.md`. Kararlar: damage taksonomisi + HUD layout DECISION dosyalarında.

---
*Önceki session blokları git history'de.*
