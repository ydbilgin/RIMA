ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v3.1 — kullanıcı editor screenshot'ıyla 3 sorun bildirdi: (1) açık karakterlerde de görünen bozuk LockGlyph (✳ gibi), (2) karakter-başına UI öğeleri dağınık (LOCK yazısı yukarıda, fiyat bandı ortada, karakter aşağıda — "her karakterin bütün olayı TEK BOX'ta olsun, ben o box'ı oynatayım"), (3) karakterler sağ skill panelinin ARKASINA giriyor; arka/orta boş alan kullanılmıyor.

# Dosya
`Assets/Scripts/UI/CharacterSelectScreen.cs` (dünkü v3 işinin devamı — kendi yazdığın kod). KOD-ONLY, .unity dosyası düzenleme YASAK. "◈" ve elmas glifleri YASAK (fontta yok).

# İŞ

## 1. TEK-KUTU REFACTOR (kullanıcının ana isteği)
Her `RoomCharacter_<Class>` köküne ait HER ŞEY o kökün İÇİNDE, köke-göre (relative) konumlanacak:
- sprite (ayak-pivot FIT korunur) · isim etiketi (ayağın hemen altı) · kilitliyse: maliyet/OR satırı (ismin altı, küçük) + kilit göstergesi.
- Kök rect = karakter kutusu (görünür char + etiket alanını saran boyut). **Kök anchor'u taşıyınca her şey beraber taşınmalı** — kullanıcı Inspector'dan tek tek/dizi halinde oynatacak.
- Serialized yerleşim: `frontRowAnchors/backRowAnchors` yerine **tek `rosterAnchors` (Vector2[10], sınıf sırasıyla — Warblade,Elementalist,Ranger,Shadowblade,Ronin,Ravager,Gunslinger,Brawler,Summoner,Hexer)** + her elemana Tooltip attribute. Default değerler madde 3'teki yerleşim.

## 2. LockGlyph fix
- Mevcut LockGlyph (✳ görünümlü) HER karakterde var — AÇIK karakterlerde TAMAMEN YOK olacak (instantiate etme).
- Kilitlilerde: o bozuk glif/sprite KULLANILMAYACAK → küçük "KİLİTLİ" text-chip (ince çerçeve, düşük kontrast) isim satırının yanında/üstünde — kutunun içinde, silüete bitişik. "LOCK" İngilizce metni de Türkçe'ye dönsün ("KİLİTLİ").
- Maliyet satırı: "150 SHATTERED ECHO · veya Act2 boss'unu Warblade ile" — tek satır, küçük, ismin altında. Yukarıda yüzen ayrı band YOK.

## 3. YERLEŞİM: paneller arasına + arka boş alana yay (kullanıcı: "arka sıralar daha boş, oralara koy mantıklı şekilde")
- Kullanılabilir x bandı = SOL panel sağ kenarı (~0.225) ile SAĞ panel sol kenarı (~0.745) arası; HİÇBİR karakter kutusu bu bandın dışına taşmayacak (paneller arkasına girme biter).
- Default dizilim **3 sıra** (arka boşluğu kullan): arka y≈0.40: 4 kilitli (Ravager .29 / Gunslinger .41 / Brawler .53 / Summoner .65) · orta y≈0.54: 2 kilitli (Ronin .35 / Hexer .59) · ön y≈0.69: 4 açık (Warblade .28 / Elementalist .42 / Ranger .56 / Shadowblade .70). (Normalized, backdrop'a göre; kullanıcı sonra oynayacak.)
- Roster container'ı da bu banda sınırla; sıralar arası dikey aralık etiketlerle çakışmasın (arka sıra etiketi orta sıra karakterine binmemeli — kontrol et).

## 4. Z-order
- Roster, side-panel'lerden ÖNCE çizilsin (paneller üstte kalsın) AMA madde 3 ile zaten çakışma olmamalı; ikisini de uygula (savunma).

## 5. Buton label kontrolü
- ConfirmButton text'inde "SEÇT" görüldü (hiyerarşi taramasında cp=0053,0045,00C7,0054). State metinlerini doğrula/düzelt: "SEÇ" / "KİLİDİ AÇ — {n} SHATTERED ECHO" / "YETERSİZ SHATTERED ECHO".

# Doğrulama (raporla)
1. `dotnet build RIMA.Runtime.csproj` PASS + `read_console` 0 error.
2. Play-observe (MainMenu→CharSelect): (a) açık karakterlerde LockGlyph objesi YOK (programatik say), (b) her RoomCharacter kökünün TÜM çocukları kök rect'inin içinde (bounds check), (c) hiçbir RoomCharacter kökü x<0.225 / x>0.745 bandı dışında değil, (d) 3-sıra yerleşim değerleri uygulandı, (e) buton 3 state metni doğru, (f) "✳/◈/⌾" tarzı glif hiçbir text'te yok.
3. CODEX_DONE.md: değişen satır aralıkları + kanıt metrikleri.
