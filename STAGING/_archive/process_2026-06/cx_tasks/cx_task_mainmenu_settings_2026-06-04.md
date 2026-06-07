ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
MainMenu ekranını yeniden kompoze et (premium, ink-on-paper) + 4K çözünürlük scaler bug'ını koddan düzelt + "AYARLAR" butonunu "Yakında" stub yerine MEVCUT tam-fonksiyonel SettingsMenuUI'a bağla. KOD-ONLY (sahne .unity dosyasına DOKUNMA — scaler'ı runtime'da koddan ayarla, modal-lock önle).

# Spec kaynağı (OKU)
STAGING/UI_REDESIGN_3SCREENS_DECISION_2026-06-04.md  (Section A = MainMenu, Section B = Settings wire). Renk/kompozisyon detayları orada.

# Dosyalar (sadece bunlar)
1) Assets/Scripts/UI/MainMenuController.cs
2) Assets/Scripts/UI/SettingsMenuUI.cs

# Yapılacaklar

## MainMenuController.cs
- **Scaler fix (koddan):** BuildRuntimeMenu()'de kullanılan Canvas'ın CanvasScaler'ını ZORLA ayarla: uiScaleMode = ScaleWithScreenSize, referenceResolution = (1920,1080), screenMatchMode = MatchWidthOrHeight, matchWidthOrHeight = 0.5f. (Sahne scaler'ı 480×270 — sahneyi diskte düzenleme, koddan ensure et.)
- **Kompozisyon restyle** (BG `main_menu_bg` + radial vignette + "v1.0" KORUNUR):
  - İnteraktif blok = SOL-ALT çeyrek, sola hizalı dikey kolon. Sağ taraf BG sanatına nefes bıraksın.
  - Başlık "RIMA" büyük, ağır, geniş letter-spacing (sol kolon üstü). Hemen altında "THE RIFT HUNTERS" muted + ekstra tracked.
  - "Yine geldin." = küçük italic WARM-ORANGE (#E89020) fısıltı, BAŞLA'nın hemen üstünde.
  - Butonlar BAŞLA / AYARLAR / ÇIKIŞ dikey kolon; yanında ince **2px dikey CYAN (#00FFCC) divider** (üst/alt fade). Buton durumları: idle = dim beyaz (alpha ~0.7); hover = saf cyan + scale 1.08 + sola `>` caret reveal; pressed = warm-orange (#E89020).
  - Türkçe metinler aynen kalsın.
- **Settings wire:** OnSettingsClicked() içinden "Yakında" stub'ını (ShowSettingsOverlay / BuildSettingsOverlay çağrısı) KALDIR. Yerine gerçek ayarları aç:
    `if (UIManager.Instance != null) UIManager.Instance.OpenSettings(); else FindFirstObjectByType<SettingsMenuUI>()?.Open();`
  - Artık kullanılmayan ShowSettingsOverlay()/BuildSettingsOverlay() stub metotlarını sil (bu redesign'ın parçası, ilgili dead code).

## SettingsMenuUI.cs
- **AutoInit scaler:** AutoInit()'te eklenen CanvasScaler'ı yapılandır: ScaleWithScreenSize, (1920,1080), MatchWidthOrHeight, match 0.5f (şu an yapılandırılmamış → 4K'da çöker).
- **Menu-context guard:** Aim ve Dash gameplay-only toggle SATIRLARINI, sahnede Player yokken (GameObject.FindGameObjectWithTag("Player") == null) GİZLE/disable et (menüde anlamsızlar, no-op'lar). Diğer bölümler (Accessibility/Audio/Controls-rebind) menüde aynen çalışsın. KeyBindManager statik/lazy — gameplay objesi gerektirmez, dokunma.
- Controls bölümü (click-to-rebind) + default tuşlar ZATEN çalışıyor — DEĞİŞTİRME, sadece menüden erişilebilir olduğundan emin ol.

# Doğrulama (ZORUNLU)
- Unity MCP ile compile: refresh_unity compile=request → read_console types=error filter=CS → SIFIR CS hatası olmalı.
- Değiştirdiğin metot/satırları + compile sonucunu CODEX_DONE.md'ye yaz. Sahne dosyası DEĞİŞMEMELİ.
- BELİRSİZLİK olursa BLOCKED yaz, tahmin etme.
