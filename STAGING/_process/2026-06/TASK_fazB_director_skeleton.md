ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ B — Director Mode İSKELET (sadece kabuk: toggle+kamera+chrome Window+sol rail+6 boş sekme). İçerik YOK (Faz C). GATE: derleme 0 error + Director gir/çık + 6 sekme toggle çalışır. Görsel doğruluk SABAHA (blind-commit, mesaja "visual unverified" not).

# OKU (zorunlu)
1. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` — TÜM yapı (mod, chrome eşleme §3, sekme sırası §2, UX §5)
2. `STAGING/_process/2026-06/chatgpt_sandbox_output/docs/C_uGUI_hierarchy.md` — ChatGPT tam uGUI ağacı (DOĞRUDAN REFERANS, hiyerarşiyi buna göre kur)
3. Chrome atlas: `Assets/Sprites/UI/Chrome/UI_Chrome.spriteatlas` (9 slice hazır, 9-slice border set edilmiş)
4. Font: `Assets/Fonts/Jersey10/Jersey10-Regular SDF.asset` (Faz A'da üretildi)
5. Mevcut: `Assets/Scripts/UI/HUDController.cs` (mevcut UI pattern), `Loc.cs` (TR+EN, `Loc.T()`)

# İŞ (sadece iskelet)

## B1 — DirectorMode controller
- Yeni `DirectorMode.cs` (uygun namespace), `#if DEMO_BUILD || DEVELOPMENT_BUILD` guard.
- Master tuş `` ` `` (backtick) → DIRECTOR (timeScale=0) ↔ TEST (timeScale=1) toggle.
- Free-cam: kamera lerp/pan **unscaledDeltaTime** (timeScale=0'da çalışmalı). ZORUNLU.
- Mod state machine (DIRECTOR/TEST), public enum + event (sekmeler/HUD dinleyebilsin).
- "Başlat" çift-durum mantığı kancası (DIRECTOR="BAŞLAT" / TEST="DIRECTOR'A DÖN") — buton var, davranış iskelet.

## B2 — uGUI Canvas + chrome skin
- ScreenSpaceOverlay Canvas, CanvasScaler 1920×1080 match 0.5, Point filter.
- Ana panel (Window) = `minimap_frame` Sliced. Sol dikey rail (~96px) = `slot_normal`/`slot_active`. Büyük aksiyon (Başlat) = `ribbon_base`. Alt şerit mod yazısı (`menu_button` stretched).
- Hiyerarşi = `C_uGUI_hierarchy.md` ağacına göre. Jersey10 SDF font tüm metinlerde.
- UnityMCP ile sahne/prefab kur (Director prefab veya boot-scene objesi). Asset kirletme (DEMO guard).

## B3 — Sekme sistemi (6 boş panel)
- Sol rail 6 sekme butonu: **Spawn · Class&Skill · Stats · Build · Map · Telemetry** (demo sırası §2).
- Geçiş = **CanvasGroup** (alpha+interactable+blocksRaycasts). Destroy YOK.
- 6 panel BOŞ (başlık + "yakında" placeholder). İçerik Faz C.
- Aktif sekme görseli `slot_active`, pasif `slot_normal`.

# GATE + COMMIT
- Derleme: `read_console` 0 error (domain reload sonrası).
- Doğrula (UnityMCP execute_code/play): backtick toggle DIRECTOR↔TEST, timeScale değişiyor, 6 sekme CanvasGroup ile geçiyor, kamera timeScale=0'da hareket ediyor.
- Otomatik test yoksa: yapısal doğrulama (hiyerarşi var, font atanmış, chrome sprite bağlı). VARSA `run_tests` yeşil.
- Geçerse commit: `feat(director): Faz B Director Mode skeleton — toggle/cam/chrome Window/rail/6-tab CanvasGroup [visual unverified]`. Geçmezse CODEX_DONE'a BLOCKED + sebep, COMMIT ETME.
- CODEX_DONE.md: ne kuruldu, hiyerarşi, derleme/doğrulama sonucu, görsel-bekleyen notlar, commit hash.

# YAPMA
- Sekme İÇERİĞİ YOK (Faz C: spawn palette, slider, vs.). Sadece boş panel.
- Spawn/PaintCell/Cliff hook'larına DOKUNMA (Faz C).
- Spekülatif sistem YOK. Sadece iskelet + listelenen dosyalar.
