ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect ekranını onaylı mockup v2'ye benzet (Unity, runtime-built UI). Kullanıcı onayı verildi; "karakter ve hitbox'ını sen ayarla, yerlerini gerekirse ben topluca ayarlarım" dedi → yerleşim SERIALIZED ve Inspector'dan toplu düzenlenebilir olacak.

# Referanslar (OKU)
- SPEC = `STAGING/mockups/charselect_mockup_v2.html` (onaylı görsel hedef; data/FIT/layout değerleri içinde)
- Kararlar = `STAGING/CHARSELECT_V3_DECISION_2026-06-05.md` (kullanıcı onayları bölümü dahil)
- Kendi envanterin = `CODEX_DONE_yasinderyabilgin.md` (file:line'lar)
- Ana dosya = `Assets/Scripts/UI/CharacterSelectScreen.cs` (+ `RimaUITheme.cs`)

# KURALLAR (ihlal = FAIL)
- **KOD-ONLY: hiçbir .unity sahne dosyasını DİSKTE DÜZENLEME** (editor'de açık sahne modal kilitler). Tüm değerler field-initializer default'larıyla gelsin (scene serialize gerektirmesin).
- **"◈" GLİFİ YASAK (U+25C8 ve tüm elmas glifleri ◆◇◊♦ LiberationSans SDF atlas'ta YOK — kutu çıkar; doğrulandı).** Para metni HER YERDE "SHATTERED ECHO" yazısı (örn. "80 SHATTERED ECHO", "KİLİDİ AÇ — 150 SHATTERED ECHO").
- Dikey scroll YOK (ScrollRect kaldır) · seçilide scale/bob/dönen-VFX YOK (statik halka+glow) · kilitli = OPAK SİYAH silüet (mevcut davranış korunur) · mağaza hissi YOK.
- Bakiye/harcama SADECE `EchoWallet` üzerinden (`Assets/Scripts/Systems/EchoWallet.cs` — yeni eklendi).

# İŞ LİSTESİ

## 1. Direktif ihlali fix'leri (kendi raporundaki satırlar)
- `:996-1002` seçili scale 1.12 → KALDIR (boyut sabit).
- `:1075-1085` dikey bob coroutine → KALDIR.
- `:1086-1120` dönen ring/pulse/mote VFX → STATİK cyan ayak-halkası + soft glow'a indir (pedestal_seal sprite reuse OK; rotasyon/pulse animasyonu yok).
- `:1159-1201` sağ panel ScrollRect → sabit kompakt satırlar (madde 5).

## 2. Yerleşim: 5+5, kullanıcı-ayarlanabilir
- `RosterPlacements` (`:75-87`, şu an 4+6 hard-coded) → **[SerializeField] alanlara çevir**: 
  `[SerializeField] private Vector2[] frontRowAnchors` + `backRowAnchors` (normalized 0-1, backdrop'a göre) — default'lar mockup'tan: front y≈0.73: x = .22/.37/.52/.67/.82 (Warblade/Elementalist/Ranger/Shadowblade/Ronin) · back y≈0.51: x = .28/.43/.58/.73/.88 (Ravager/Gunslinger/Brawler/Summoner/Hexer). Inspector'dan TOPLUCA düzenlenebilir olsun (kullanıcının açık isteği). Sınıf→slot eşlemesi sabit dizi.

## 3. Karakter boyut + oturma (PIXEL-MEASURED FIT — mockup'la birebir aynı değerler)
Alpha-scan ölçümleri (canvas px / görünür-char-yüksekliği px / ayak-altı-boşluk px / x-merkez-sapma px):
```
warblade:120/61/30/-0.5  elementalist:120/60/30/-0.5  ranger:128/61/34/-3
shadowblade:124/61/32/-0.5  ronin:128/62/34/-0.5  ravager:124/60/32/-0.5
gunslinger:124/61/32/-0.5  brawler:120/60/30/-0.5  summoner:124/61/32/-2  hexer:124/61/32/-0.5
```
- **Boyut:** her karakterin GÖRÜNÜR yüksekliği aynı olacak → image rect height = BASE × (canvas/visH), Warblade baseline (120/61). BASE = mevcut karakter boyutuyla benzer kalsın (görsel olarak mockup oranı: char ≈ karo genişliği).
- **Oturma:** RectTransform **pivot.y = feetGap/canvas** (örn. Warblade 30/120=0.25), pivot.x = 0.5 + (xBias/canvas) → pivot tam AYAK noktası olur; anchoredPosition = anchor noktası → ayak karoya basar. (Mockup'taki translate çözümünün Unity karşılığı.)
- Kilitli karakterler de AYNI boyut/oturma (siyah silüet renk swap'ı korunur).

## 4. Hitbox (kullanıcı: "sen ayarla")
- Mevcut full-rect transparan hit Image (`:399-420`) → **görünür karaktere oturan rect**: genişlik ≈ visW tabanlı (FIT'e ekle: visW px = 38/30/35/30/28/38/36/34/33/32 sırasıyla) + ~%15 padding, yükseklik = görünür char + ~%10. Ayak-pivot'a hizalı. `[SerializeField] private Vector2 hitPaddingScale = new(1.15f, 1.10f)` → kullanıcı tweak edebilir.
- Kilitli karakter TIKLANABILIR kalır (bilgi panelde açılır; mevcut davranış).

## 5. Sağ panel: TAM skill listesi + tooltip (kullanıcı onaylı yapı)
- Scroll YOK; kompakt satırlar: **3 başlangıç aktifi** ikon+isim+1-satır kısa açıklama; kalan skill'ler ikon+isim tek-satır (SkillDatabase'den; ikon yoksa harf-rozet placeholder).
- **Açılma-şartlı (mastery) satırlar:** KARANLIK + SADECE İSİM + şart metni ("Açılış: Act 1'i Warblade ile bitir") — açıklama GİZLİ, fiyat ASLA. Şart verileri mockup data'sındaki condition string'leri (her sınıf son 3 skill teaser).
- **Hover tooltip (sadece AÇIK skill satırları):** imleç üstüne gelince açıklama kutusu — `TooltipSystem.cs:84-218` HAZIR ve hiç bağlı değil; UYGUNSA onu bağla (en yüksek reuse), değilse minimal lokal tooltip (tek panel + TMP, cursor-follow). Kilitli satırda tooltip YOK.
- SkillDatabase'de sadece 4 sınıf kayıtlı → kayıtlı olmayanlarda mockup'taki placeholder isimler statik fallback.

## 6. Sol panel: kimlik + 5 stat barı
- Mevcut kimlik bloğu (ad/tag/motto/açıklama/kaynak) korunur; **5 stat barı eklenir** (HASAR/DAYANIKLILIK/HIZ/KONTROL/ZORLUK, 10-segment, cyan dolu/koyu boş).
- Stat verileri: mockup v2 data'sındaki stats dizilerini `RimaUITheme.ClassIdentity`'ye (veya yanına statik tabloya) taşı — 10 sınıf.

## 7. Ekonomi/metin
- `UnlockCost` (`:938-947`, şu an 120/180/250) → **CANON: Ronin 150, Ravager 150, Gunslinger 200, Brawler 200, Summoner 200, Hexer 250** (W/E/R/S default-açık kalır).
- Kilitli etikette **OR-yolu**: "150 SHATTERED ECHO · veya {şart}" (şartlar decision doc'taki canon tablo: Ravager=Act2 boss'u Warblade ile · Ronin=Act2 boss'u Shadowblade ile · Gunslinger=Ranger ile Act2'ye ulaş · Brawler=Ravager ile Act2'ye ulaş · Summoner=art arda 3 run Act2 · Hexer=Elementalist ile run bitir). Şartlar ŞİMDİLİK sadece metin (achievement-tracking kodu YOK — scope dışı).
- Üst bar: sol "RIMA — KARAKTER SEÇ", sağ "{bakiye} SHATTERED ECHO" (EchoWallet.Balance).
- Paneller: siyaha-yakın opak arka (mockup: rgba(4,9,13,.95)), ekran kenarlarına yaslı; skill paneli sol panelden geniş (mockup oranları: sol %21.2, sağ %24.6).

## Doğrulama (raporla)
1. `dotnet build RIMA.Runtime.csproj` PASS + UnityMCP `read_console` 0 error.
2. UnityMCP ile CharacterSelect sahnesini/akışını PLAY-OBSERVE et (MainMenu→CharSelect): hiyerarşide doğrula → 10 karakter var, 5+5 anchor'larda, kilitlilerde siyah silüet + cost+OR metni, sağ panelde scroll YOK + tam liste + karanlık şart satırları, sol panelde 5 stat bar, seçili scale==1.0 ve pozisyon animasyonu yok, "◈" karakteri HİÇBİR text'te yok (programatik tara!), buton state'leri SEÇ/KİLİDİ AÇ/YETERSİZ. Screenshot overlay-UI'ı yakalamaz — hiyerarşi/component değerleriyle kanıtla.
3. CODEX_DONE.md: değişen dosya+satır aralıkları, doğrulama kanıtları, bilinen kısıtlar.
