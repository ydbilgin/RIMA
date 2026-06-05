ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharSelect FONKSİYONEL düzeltmeler (görsel-redesign DEĞİL — o ayrı HTML-mockup akışıyla gelecek). 5 iş: (1) class-carry BUG fix, (2) demo Echo bakiyesi, (3) çalışan unlock, (4) locked-seçilemez, (5) locked=siyah-silüet. KOD-ONLY. Panel boyutları / skill-layout / genel görünüme DOKUNMA (onlar mockup sonrası).

# Dosyalar (gerektiği kadar)
Assets/Scripts/UI/CharacterSelectScreen.cs (ana) + PlayerClassManager + Echo/unlock store (grep ile bul).

# İşler
1. **CLASS-CARRY BUG (öncelik):** Oyuna girince seçilen sınıf ne olursa olsun HER ZAMAN Warblade geliyor. Kök-neden bul: `PlayerClassManager.SelectedClass` (grep) gameplay'e taşınıyor; CharacterSelectScreen'in SelectClass / SEÇ-OnStartRun bunu set ediyor mu yoksa sadece UI mı güncelliyor? Set kayıp/default Warblade ise FIX: SEÇ'e basınca (OnStartRun) seçili `ClassType` `PlayerClassManager.SelectedClass`'a yazılsın, sonra scene yüklensin. Doğrula: Elementalist seç→SelectedClass=Elementalist.
2. **DEMO ECHO:** Echo currency nerede (RunStats/PlayerPrefs/meta-save — grep `Echo`). Demo için başlangıç bakiyesi **200 Echo** seed et (en az-kod; PlayerPrefs default ya da uygun store). CharSelect'te mevcut Echo görünür bir yerde gösterilsin (küçük chip, mevcut bir text-helper ile — basit).
3. **ÇALIŞAN UNLOCK:** Locked char seçiliyken "KİLİDİ AÇ — {cost} Echo" butonu: Echo ≥ cost ise ENABLED + tıklanınca → Echo'dan cost düş + o sınıfı UNLOCKED işaretle (persist; mevcut unlock store/PlayerPrefs) + char'ı normale çevir (silüet→normal renk) + artık selectable. Echo < cost ise muted/disabled. Hexer özel (250 + Elementalist run) korunur.
4. **LOCKED NOT SELECTABLE:** Locked char'a tıklayınca PLAYABLE seçim OLMASIN (SEÇ sadece unlocked sınıfta aktif + scene yükler). Locked tık → sadece unlock CTA gösterir (kimlik/skill preview gösterebilir ama "oyna" seçimi yapmaz).
5. **LOCKED = SİYAH SİLÜET:** locked char sprite Image.color ≈ near-black `#0A0510` (alpha KORUNUR → sadece silüet). Unlock olunca → normal (white) renge dön. (Mevcut dim ~0.40 yerine tam siyah-silüet.)

# DOKUNMA
Panel anchor/boyutları, skill-row layout, backdrop, VFX-ring, genel görünüm — bunlar HTML-mockup akışıyla ayrı gelecek. Sadece yukarıdaki 5 fonksiyonel iş.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode probe: SelectClass(Elementalist)→PlayerClassManager.SelectedClass=Elementalist (+ SEÇ scene-load doğru sınıfı taşır); Echo bakiyesi=200; locked char (Ronin) tık→SEÇ playable DEĞİL ama "KİLİDİ AÇ 120" enabled (Echo 200≥120); unlock→Ronin unlocked+normal renk+selectable+Echo=80; locked char rengi near-black silüet.
- Değişen metot/satır + kök-neden + compile + probe sonucu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
