ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v3.2 RESTYLE — kullanıcı şikayetleri: (1) seçimde aşırı flash, (2) GERİ/SEÇ butonları bayat,
(3) paneller okunmuyor + her şey çok mavi. Çözüm spec'i ax-3.5-Flash web-araştırmasından sentezlendi
(Hades/Darkest Dungeon/Hollow Knight/Children of Morta/StS pattern'leri; Opus karar verdi).

# Dosya
`Assets/Scripts/UI/CharacterSelectScreen.cs` (+ gerekirse `RimaUITheme.cs` renk sabitleri). KOD-ONLY,
.unity düzenleme YASAK. "◈/elmas glifleri" YASAK. Karakter boyutu/pozisyonu DEĞİŞMEZ (FIT+anchors dokunma).

# SPEC (KARARLAR — uygula)

## 1. Seçim feedback'i: FLASH TAMAMEN KALDIRILIR
Yerine 3-parça subtle feedback (Children of Morta modeli):
- (a) Seçili OLMAYAN karakterler **%40 dim** (sprite color multiply ~0.6; kilitli siyahlar zaten siyah — onlara dokunma).
- (b) Ayak halkası **0.2s fade-in** ile belirir, sonra STATİK kalır (pulse/dönme yok — mevcut statik halka korunur, sadece beliriş yumuşar).
- (c) Karakter SCALE/FLASH/BOB YOK (kullanıcı kuralları).
Mevcut select-flash/highlight-flash kodu ne varsa SİL.

## 2. Renk disiplini: 60-30-10 (mavi çorba biter)
RimaUITheme'e nötr sabitler ekle ve CharSelect'te uygula:
- **%60 zemin:** void-siyah `#0F0D15` (panel fill bununla).
- **%30 nötr:** çerçeve demir-gri `#2F3037` (1px iç highlight) · gövde metni `#B0B3BC` · standart metin parşömen `#EAEAEA` · ayraç çizgileri soluk-mor `#493B5E`.
- **%7 CYAN `#00FFCC` SADECE:** seçili ayak-halkası + seçili karakterin adı (sol panel başlığı) + hover kenarı. BAŞKA HİÇBİR YERDE (tag chip'leri, stat barları, panel başlıkları, LOCK chip → nötrleşir).
- **%3 ORANGE `#E89020` SADECE:** KİLİDİ AÇ buton metni + SHATTERED ECHO miktarı (üst bar + maliyet satırları) + stat-pozitif vurgu.
- Stat barları: dolu segment **nötr açık `#C8CDD8`**, boş segment koyu `#1A1B22` + ince gri çerçeve (cyan kaldır).
- Skill satırları: isimler parşömen; sadece 3 başlangıç aktifinin İKON çerçevesi hafif cyan kalabilir; kilitli şart metni gri-mor.

## 3. Paneller: "siyah ama saydam, okunur"
- Panel fill: `#0D0D11` @ **%85 opaklık** (tek düz katman; mevcut çok-katmanlı mavi gradient KALDIRILIR).
- Çerçeve: `#2F3037` + 1px iç açık çizgi (inset highlight); köşe süsleri (mevcut before/after köşe çizgileri) nötr gri olur.
- Blur shader EKLEME (lean karar — %85 opaklık yeterli).

## 4. Butonlar (Hades/DD/StS sentezi)
- **Primary (SEÇ / KİLİDİ AÇ):** geniş plaka; mevcut `Resources/UI/RIMA/Pack` 9-slice'larından en uygunu (panel_frame_9slice / bar_frame_9slice) ile **açısal/demir plaka** görünümü: koyu fill `#15161C` + demir çerçeve + ALL-CAPS bold geniş letter-spacing label. SEÇ metni parşömen `#EAEAEA`; KİLİDİ AÇ metni **orange**; YETERSİZ pasif: gri metin + %50 alpha.
  - Hover: çerçeve orange'a döner + buton (sadece BUTON) %5 scale + hafif glow. Pressed: %2'ye iner.
- **Secondary (GERİ):** daha küçük düz koyu taş-plaka, gri `#9E9E9E` metin; hover: kenar cyan + %2 scale.
- Yeni asset üretimi YOK (gen GATED) — mevcut Pack 9-slice + renk/typografi ile çöz.

## 5. Üst bar
- "RIMA — KARAKTER SEÇ": RIMA parşömen-beyaz, ayraç ve alt-başlık gri (cyan kaldır).
- Sağ: "{n} SHATTERED ECHO" → miktar **orange**, "SHATTERED ECHO" kelimesi gri küçük.

# Doğrulama (raporla)
1. dotnet build PASS + read_console 0 error.
2. Play-observe: (a) seçim değişiminde flash objesi/coroutine YOK (kod tarama + runtime), (b) seçili-olmayan açık karakterler dim (%40 civarı renk değeri ölç), (c) cyan kullanan UI eleman SAYISI ≤ 4 tip (halka, seçili-ad, hover, aktif-ikon-çerçevesi) — programatik renk taraması ile listele, (d) panel fill alpha ~0.85 `#0D0D11`, (e) buton 3 state + hover scale çalışır.
3. CODEX_DONE.md: değişen satırlar + renk-tarama dökümü.
