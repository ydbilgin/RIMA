ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Chamber rework-3 sonrası kullanıcı playtest geri bildirimi (screenshot). Ana dosya `Assets/Scripts/UI/ChamberSelectBootstrap.cs`. 5 somut düzeltme. Legacy'ye dokunma. ("Hades-tarzı seçim" + silah seçimi AYRI karar, bu task'a DAHİL DEĞİL.)

# DÜZELTMELER (kullanıcı, kesin)
1. **DUMMY YERİ:** Dummy ("Kukla") şu an karakter dizisinin içinde/yakınında. Onu sınıf dizisinden NET AYRI, kendi açık alanına taşı (kullanıcı sağ taraftaki boş alanı işaret etti). Walkable, spawn/portal/roster ile çakışmayan temiz bir nokta seç (örn. chamber'ın sağ-orta açık alanı). Üstündeki "KUKLA CANI 100000/100000" etiketi dummy ile gitsin.

2. **DUMMY RE-SKIN GERİ AÇ (dummy-only):** rework-3'te dummy G-popup'ı KALDIRILDI. Kullanıcı artık dummy'i istediği sınıf olarak seçebilmek istiyor. Dummy yakınında G → sınıf seçici aç → seçilen sınıf SADECE dummy görselini değiştirsin (zaten `ApplySelectedClassToDummyOnly` + `AcceptClassicSelectionFromPopup` var; sadece dummy G-giriş noktasını yeniden bağla). PLAYER'a/PlayerClassManager'a DOKUNMASIN (bu kuralı koru). Prompt net: "[G] KUKLA: SINIF SEÇ" gibi.

3. **PORTAL YERİ + SİYAH ARTEFAKT:** Chamber çıkış portalı kötü konumda + arkasında/üstünde siyah bir dikdörtgen (gate header/silüet) var = çirkin. (a) Portalı temiz, kasıtlı bir yere koy (örn. arka kenar ortası, düzgün çerçeveli, floor'a hizalı — kenarda yamuk durmasın). (b) Arkasındaki siyah artefaktı kaldır/düzelt (gate'in siyah header'ı veya entry-portal black-alpha silüeti — chamber'da gereksiz/çirkinse temizle).

4. **MAVİ/CYAN ÇİZGİ:** Player'dan portala giden cyan LineRenderer (locator/guide). Chamber'da gereksiz (portal görünür) ve kafa karıştırıyor. Kaynağını bul, CHAMBER'da kapat. (Gerçek combat-oda exit locator'ı ayrı sistemse ona dokunma; sadece chamber'da görünmesin.)

5. **MOR/MAGENTA KARE (debug gizmo):** Ekranın sağında play sırasında çizilen magenta kare = debug gizmo (muhtemelen RoomDebugGizmo veya room/camera bounds). Kaynağını bul, play/build'de görünmesin (#if UNITY_EDITOR scope'u veya debug-toggle arkasına al). Daha önceki audit'te "kalıcı mor çizgi kaynağı statik bulunamadı" denmişti — bu KARE'yi (gizmo) bulup kapat.

# KORU / KISIT
- Dummy ölümsüzlüğü/trigger-only + rework-3'teki figür-G-confirm player seçim akışı bozulmasın.
- Walkable enforcement + room-completion + chamber testleri yeşil kalsın.
- Cerrahi: ChamberSelectBootstrap.cs + (gizmo için) ilgili debug script. PlayerAnimator/combat'a dokunma.

# ÇIKTI (CODEX_DONE.md)
- Değişen dosyalar + file:line + her 5 maddenin nasıl çözüldüğü (dummy yeni koordinatı, portal yeni konumu, cyan-line/gizmo kaynağı).
- Compile temiz mi. (Unity açık → test/screenshot bloke olabilir, not düş; kullanıcı playtest edecek.)
- BLOCKED yaz belirsizse. Commit ETME. Untracked dosyalara dokunma.
