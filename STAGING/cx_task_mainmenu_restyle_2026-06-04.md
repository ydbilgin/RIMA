ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
MainMenu'yu canon "ink-on-paper / no boxes" tonuna göre TAM restyle — runtime-rebuild yaklaşımı (council kararı). Gerçek menü = sahne `Assets/Scenes/UI/MainMenu.unity` (MainMenuCanvas + MainMenuController + authored "Root").

Council kararı (uygula): STAGING/_council_a_31pro_mainmenu.md (özet aşağıda).

## Dosya
- Assets/Scripts/UI/MainMenuController.cs (ana iş — kod-driven menü build ekle)
- (MainMenuScreen.cs'e DOKUNMA — o ayrı bootstrap/test target.)

## Yapılacaklar (MainMenuController runtime-rebuild)
1. `Start()`'ta menüyü KODLA kur (RimaUITheme styled). Authored "Root" altındaki eski UI'ı temizle/deaktive et (SetActive false ya da children temizle) ki çakışmasın. Backdrop'u koru: `RimaUITheme.CreateFullScreenBackdrop(canvas, "UI/Backgrounds/main_menu_bg", ...)`.
2. **Radial vignette overlay** (tam-ekran, click-through Image, merkez transparent → kenar ~%60 void-mor/siyah) — metni çerçeveler, KUTU YOK.
3. **Başlık hiyerarşisi (sol-hizalı kolon, ~sol kenardan ~100px):**
   - "RIMA" — büyük pixel-serif, TextPrimary + **void-glow Shadow** (cyan düşük-alpha offset ya da void-mor).
   - "THE RIFT HUNTERS" — küçük, harf-aralığı açık (tracked), RimaUITheme slate/muted renk, geride durur.
   - "Yine geldin." — minik pixel font fısıltı, ayrı (buton listesinin hemen üstü ya da sol-alt köşe). Başlık gibi DURMASIN.
4. **Butonlar = çıplak metin (BAŞLA / AYARLAR / ÇIKIŞ, Türkçe kalsın):** sol-hizalı dikey kolon. Hover'da renk muted-slate→cyan #00FFCC SNAP + solda minik cyan ">" belirir. Opak buton kutusu YOK (Pack frame de istersen çok hafif/translucent). Wiring KORU: BAŞLA→OnStartClicked (CharacterSelect), ÇIKIŞ→OnQuitClicked, AYARLAR→OnSettingsClicked.
5. **AYARLAR → stilize "Yakında." overlay:** OnSettingsClicked artık tam-ekran ~%80 koyu overlay + ortada tek satır cyan "Yakında." göstersin; tıklayınca kapansın. (Mevcut tooltip coroutine yerine bu.)
6. Version label: sağ-alt, çok küçük, ~%35 opacity.

## Doğrulama
- validate_script + Unity compile temiz (0 hata). Play-mode'a GİRME (Opus play-verify edecek; MCP overlay screenshot alamıyor → yapısal doğrula: runtime'da RIMA/THE RIFT HUNTERS/Yine geldin./3 buton/vignette var mı).
- CODEX_DONE.md'ye: değişen satırlar + 6 madde durumu + compile + (authored Root nasıl ele alındı).
