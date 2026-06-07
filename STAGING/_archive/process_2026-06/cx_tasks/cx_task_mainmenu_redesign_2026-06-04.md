ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Giriş ekranını canon "sessiz tespit + ink-on-paper" tonuna çek.

READ FIRST: STAGING/UI_REDESIGN_SCREENS_DECISION_2026-06-04.md (§4 MainMenu).

## Dosya (sadece bu)
- Assets/Scripts/UI/MainMenuScreen.cs

## Yapılacaklar
1. **Tagline DEĞİŞ:** "THE SEAL BENEATH THE KEEP" → **"Yine geldin."** (küçük, cyan, fısıltı; epik slogan YOK).
2. **Backdrop:** mevcut `UI/Backgrounds/main_menu_bg` reuse (zaten CreateFullScreenBackdrop ile yükleniyor — dokunma, iyi). Bloom-pulse EKLEME (crisp dirty-paper'a ters).
3. **Butonlar:** Pack `button_9slice` (RimaUITheme) translucent stil + cyan hover; NEW RUN hafif vurgulu (primary). Opak kutu YOK.
4. **SETTINGS butonunu TAMAMEN GİZLE** (stub — disabled "soon" bile gösterme; canon: çalışmıyorsa var olmamalı). Kalan: NEW RUN + QUIT.
5. **Version label:** sağ-alt, çok küçük, ~%35 opacity (mevcut "S43 Dev Build" konumunu sağ-alta al, küçült).

## Doğrulama
- Derleme temiz (0 hata). Play-mode'a GİRME (kullanıcı D3D11 restart edecek — MainMenu zaten crash riski yüksekti).
- CODEX_DONE.md'ye: değişen satırlar + 5 madde + compile.
