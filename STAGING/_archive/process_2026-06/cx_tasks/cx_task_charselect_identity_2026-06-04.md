ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Karakter seçim ekranında sağ panel sınıf seçilince DİNAMİK sınıf kimliği göstersin (şu an statik label). Kullanıcı: "karakter seçince yanda açıklaması yazsın."

READ FIRST: STAGING/UI_REDESIGN_SCREENS_DECISION_2026-06-04.md (§2 CharSelect) + STAGING/UI_REDESIGN_BRIEF_2026-06-04.md (Ek-A = 10 sınıf NLM kimlikleri, VERİ KAYNAĞI burada).

## Dosyalar (sadece bunlar)
- Assets/Scripts/UI/RimaUITheme.cs (yeni ClassIdentity API)
- Assets/Scripts/UI/CharacterSelectScreen.cs (sağ panel dinamik bind)

## Yapılacaklar
1. **RimaUITheme.cs:** Yeni API ekle `public static (string motto, string playstyle, string resource) ClassIdentity(ClassType cls)`. ClassTagline'ı GENİŞLETME (center panel onu kullanıyor, kırılır). Veri = brief Ek-A (10 sınıf: Warblade..Hexer; motto + oynanış 1-2 cümle + ana kaynak). Türkçe metinler ASCII-güvenli olmak ZORUNDA DEĞİL (UI runtime string, TMP destekler) ama dosyada Türkçe karakter sorun çıkarırsa İngilizce-ASCII yaz.
2. **CharacterSelectScreen.cs:** `BuildRightPanel` (≈ satır 322-364) statik 6-label yerine dinamik kimlik paneli:
   - **motto** (üstte, bold, ClassAccent(cls) renk)
   - **playstyle** (altında, wrap, RimaUITheme.TextMuted)
   - **resource** (altında, RimaUITheme.TextPrimary, keskin — hiyerarşi: muted flavor vs primary mekanik)
   - accent bar = ClassAccent(cls).
   - Sınıf seçimi değişince güncelle (mevcut OnClassSelected/refresh akışına bağla). Kilitli sınıf: kimlik görünür + "X Echoes gerekli" eklenir.
   - SKILL LİSTESİ EKLEME (kullanıcı + council: tooltip'siz isim listesi clutter). Sadece kimlik+oynanış+kaynak.
3. Opak kutu yapma — mevcut panel stilini koru (translucent). Min code, cerrahi.

## Doğrulama
- Derleme temiz (0 hata). Play-mode'a GİRME (kullanıcı D3D11 restart edecek, verify ben yapacağım).
- CODEX_DONE.md'ye: değişen satırlar + ClassIdentity 10-sınıf eklendi mi + compile durumu.
