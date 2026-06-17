# GÖREV: RIMA Akademik Rapor Revizyonu (council feedback uygula → DOCX yeniden üret)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
**TÜM TÜRKÇE KARAKTER ZORUNLU** (ç ğ ı İ ö ş ü) — akademik Türkçe, ASCII-leştirme YOK.
**Bu görev Unity'ye DOKUNMAZ** (paralel başka ajan Unity'de). Sadece .md + .py + figür dosyaları.

NLM ACCESS gerekirse: NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"
GRAPHIFY: mimari/çok-dosya sorusunda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.

## Tek kaynak + pipeline
- Düzenlenecek: `STAGING/report/RIMA_Senior_Design_Report.md` (markdown = TEK kaynak; figür satırı formatı `[Şekil N: caption | path_hint]`)
- DOCX üretici: `STAGING/report/make_akademik_docx.py` (çalıştır: `cd STAGING/report && python make_akademik_docx.py`) → `RIMA_Senior_Design_Report.docx`
- Council verdict (uygulanacak bulgular): `STAGING/_process/2026-06/AX_REPORT_REVIEW_VERDICT.md` — OKU.
- Mimari kaynağı: `CODE_MAP.md` (kök, 54 satır) + gerekirse graphify query / hedefli Grep `Assets/Scripts/`. **Sınıf sorumluluklarını UYDURMA — CODE_MAP + gerçek kod kanıtı.**

⚠️ Satır numaraları düzenledikçe KAYAR. **Üstten-alta DEĞİL, benzersiz-string/başlık anchor'larıyla** çalış. Her edit sonrası bir sonraki anchor'ı tekrar bul.

## YAPILACAKLAR

### 1. FİGÜRLER
- **Şekil 6** (`report_screenshots/11_map_designer.png`) = dosya YOK, raporda kırık placeholder. → **Figür satırını ve civarındaki "Şekil 6'da/Map Designer penceresi" metin atıflarını SİL.**
- **Figür yeniden-numaralandır:** Şekil 6 çıktığı için **eski Şekil 7→6, 8→7, 9→8, 10→9, 11→10** yap. HEM figür tanım satırlarını HEM gövdedeki tüm "Şekil N" atıflarını güncelle (Grep `Şekil 7|Şekil 8|Şekil 9|Şekil 10|Şekil 11` ile bul, hepsini kaydır). Tutarlılık kritik.
- **Şekil 3** (`fig06_warblade.png`): caption "omuz zırhı ve **iki elli kılıç silueti**" diyor ama sprite'ta kılıç YOK (silahsız bake). → caption'ı gerçeğe çek: kılıç iddiasını kaldır, gösterilen şeyi tarif et (ör. "Warblade sınıfı — omuz zırhlı silahsız temel sprite; silah mount sistemi Şekil 7'de"). Silah figürü zaten ayrı (eski Şekil 8 = fig_weapon_mount, yeni no'ya göre güncelle).
- Diğer 9 figür gerçek-Unity + yerinde → DOKUNMA.

### 2. AI-ODAĞINI AZALT (Bölüm 8)
Verdict B + E2. Bölüm 8'i ~yarıya indir, mühendislik çıktısına odakla:
- **8.4 "10-Task Otonom Gece Kuyruğu"**: tek paragrafa indir (deney anlatısı değil, mühendislik sonucu).
- **8.5 satır ~448-449** ("ajanlar söyleneni yapan araçlardır... tasarlayan kişi geliştiricidir") = kendini-savunma tonu → SİL, yerine nötr "araçlar şu mimariyle orkestre edildi" tek cümle.
- **Bölüm 6.4 satır ~331-332** ("15'i ChatGPT'den AI-destekli 'seviye tasarımcısı' olarak alınmıştır") → teknikleştir: "Prosedürel JSON oda şablonları harici araçlarla üretilip içe aktarıldı".
- AI BAHSEDİLSİN ama dengeli — oyun + yazılım mimarisi öne çıksın, "prompt engineering raporu" havası gitsin.

### 3. ChatGPT-VARİ PASAJLARI BUDA (Verdict D)
- **Satır ~23-24** (1.2 civarı): "...kendi yazdığı kodu kendi gözden geçirmek ve kendi tasarladığı seviyenin monotonluğunu kendi fark etmek giderek güçleşir." → felsefi şişirme, çıkar/sadeleştir.
- **Satır ~253-254** (4.6 Game-Feel): "Mekanik doğruluk... yeterlidir; ancak bir aksiyon oyununun inandırıcı hissetmesi ek bir çalışma gerektirir." → yapay "ancak" geçişi; doğrudan "Oyun hissi (game-feel) katmanı..." ile başla.
- **Satır ~456** (9.1): "Ekip büyüdükçe doğal dağılan kalite filtresi... RIMA bu boşluğu..." → "RIMA'nın kalite güvence (QC) süreci üç katmandan oluşur:" ile sadeleştir.
- **Satır ~564** (11.4): "Bu vaka, projenin en güçlü mühendislik anlatılarından biridir ve 'otomatik testlerin geçmesi...' ilkesini somutlaştırır." → jenerik LLM coşkusu; nötr akademik cümleyle değiştir.

### 4. SİSTEM MİMARİSİ / KLASÖR YAPISI BÖLÜMÜ EKLE (Verdict C)
Bölüm 2'ye yeni alt-bölüm: **"2.2 Proje Klasör Yapısı ve Sınıf Sorumlulukları"** (mevcut 2.2→2.3, 2.3→2.4 olarak kaydır + TOC/atıfları güncelle; TOC `make_akademik_docx.py` `_add_toc` ile üretiliyorsa otomatik, yine de kontrol et).
İçerik:
- `Assets/Scripts/` klasör ağacı (anlamlı modüller: Core, Combat, Skills, Map, Encounter, Environment, Data, DevTools/Editor, UI, VFX, Demo vb. — CODE_MAP.md'deki küratörlü liste; legacy/duplike dizinleri (Enemy vs Enemies) şişirme yapma).
- **God-node sınıfların sorumlulukları** (graphify god-node tezi Bölüm 10 ile bağ): `RoomRunDirector`, `IsoRoomBuilder`, `SkillController` (+ varsa BuildModeController/DirectorMode) — her biri ne yapar, hangi sistemlerle etkileşir (1-2 cümle, hiyerarşik). Akademik "katmanlı kod analizi" üslubu (referans: tipik bitirme "Proje Yapısı" bölümü).
- Kısa: veri katmanı (ScriptableObject) ↔ runtime manager ↔ sahne ilişkisi (Bölüm 2.1/2.3 ile çakışma yaratma, tamamlayıcı ol).

## BİTİRİNCE
- `cd STAGING/report && python make_akademik_docx.py` çalıştır → DOCX üret, script'in verify çıktısını kontrol et (gömülü figür sayısı doğru mu, "görsel bulunamadı" placeholder KALMAMALI).
- Çıktı: `STAGING/_process/2026-06/REPORT_REVISION_DONE.md` — ne değişti (madde madde), figür yeni-numara haritası, DOCX figür-sayısı/placeholder durumu, kalan/şüpheli nokta.

Dönüşün ≤10 satır: madde özeti + DOCX path + placeholder 0 mı + done-dosyası yolu.
