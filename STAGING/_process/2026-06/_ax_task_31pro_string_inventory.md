# GÖREV: RIMA UI string envanteri + TR/EN lokalizasyon tablosu (T-LOC hazırlığı)

Sen RIMA'nın derin analiz ajanısın. Kod tabanını TARA ve lokalizasyon görevinin temelini hazırla. KOD DEĞİŞTİRME — sadece analiz + tablo dosyası üret.

## Bağlam
Kullanıcı kararı (2026-06-07): UI çift dilli olacak — TR + EN, ayarlardan seçilecek; seçili dilde gösterilecek. Şu an string'ler hard-coded ve TR/EN karışık. Sonraki adımda bir Sonnet agent'ı LocalizationService + dil ayarı implement edecek — senin çıktın onun girdisi.

## İş
1. `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\` altında KULLANICIYA GÖRÜNEN tüm UI string'lerini bul (rg/grep ile: SetText, text =, GUIContent, başlık/buton/prompt literal'leri). Odak dosyalar (kesin tara): MainMenuController, SettingsMenuUI, CharacterSelectScreen, ChamberSelectBootstrap, DeathScreenManager, DemoCompleteOverlay, RunMapOverlay, SkillCodexUI, CharacterSheetUI, ExecutePromptDriver, HUDController, DraftManager/SkillOfferUI, RewardPickup, BossIntroController. Debug.Log/editor-only string'ler HARİÇ.
2. Çıktı dosyası: `STAGING/_process/2026-06/LOCALIZATION_STRING_TABLE_2026-06-07.md` — tablo: `anahtar_önerisi · dosya:satır · mevcut metin · TR · EN`. 
   - TR çevirileri için STANDART tablo: `STAGING/_incoming/ux_flow_feedback_2026-06-07/RIMA_UX_FLOW_FEEDBACK_2026-06-07/04_LANGUAGE_AND_TERMS.md` dosyasını OKU ve oradaki terim kararlarını AYNEN kullan (Ana Menü, Koşu Yolu, Yetenek Kodeksi, Rift'e Gir, İnfaz, "kapı" deme "portal" de, tuş formatı [G] köşeli parantez...).
   - EN tarafını da standardize et (title-case başlıklar, aynı terim ailesi: Rift Portal, Attune, Execute...).
   - "Wishlist on Steam" = her iki dilde EN kalır (CTA istisnası).
3. Anahtar adlandırma önerisi: `ekran.eleman` formatı (örn. `menu.start`, `death.retry`, `hud.objective_clear`).
4. Sonda kısa bölüm: "Uygulama önerisi" — kaç string, hangi ekranlarda yoğun, dil ayarının nereye ekleneceği (SettingsMenuUI'da mevcut yapı), PlayerPrefs anahtarı önerisi.

## Format
Markdown, TAM Türkçe karakterlerle. Tablo eksiksiz olsun — Sonnet senin tablonu birebir uygulayacak, atladığın string karışık kalır.
