# GÖREV: Rapor BİRLEŞİK PASS — council cila + §11 T9/T7 vakaları + figür atıfları → DOCX

SADECE .md + .py + DOCX. **Unity'ye DOKUNMA. Git'e DOKUNMA (commit/push/add YOK).**
ACTIVE RULES: (1) think (2) min/surgical — sadece belirtilen düzenlemeler (3) BLOCKED if unclear.
**TÜM TÜRKÇE KARAKTER ZORUNLU** (ç ğ ı İ ö ş ü), akademik Türkçe.

## Dosyalar
- Düzenlenecek: `STAGING/report/RIMA_Senior_Design_Report.md`
- **Edit-spec (TÜMÜNÜ OKU ve UYGULA):** `STAGING/_process/2026-06/REPORT_POLISH_EDITSPEC_2026-06-19.md` — Opus'un council sentez kararları. Oradaki "UYGULA" maddelerini ve "§11 BİRLEŞİK PASS" maddelerini hayata geçir.
- DOCX: `cd STAGING/report && python make_akademik_docx.py`

## 1. EDIT-SPEC'İ UYGULA (council cila)
Edit-spec'teki tüm maddeler. Özellikle:
- **Şekil atıfları:** gövdeye 12 figürün her birine net "(bkz. Şekil N)" / "Şekil N'de görüldüğü üzere" ekle. (Figürler doğrulandı, DOKUNMA — sadece metin atıfı ekle.)
- **Jargon/Centerpiece:** §5 başlığından "(Centerpiece)" çıkar; oyun/İngilizce terimleri akademik Türkçe + ilk-kullanım parantez (edit-spec eşleme tablosu).
- **AI dili:** model kod-adları (gpt-5.5/builder-opus/crafter-sonnet/cx/ax) prosadan çık → rol-soyutlama + TEK dipnot; "Danışman Konsey/Ajan" → nesnel terimler. **METODOLOJİ KATKISINI KORU** (silme, sadece dili akademikleştir).
- Ham JSON/QC-log (§6.4/§9.3) → şematik özet · bold→#### (§4.1/§8.6) · §12.1 mükerrer istatistik sadeleştir · Şekil 12 gerekçelendir.

## 2. §11 BİRLEŞİK (council + yeni T9/T7 vakaları)
- "Ders:" → "Çıkarım:" + nesnel ton (tüm §11).
- §11.1 (RTX5080/D3D12 donanım çökme) → tek cümleye indir veya çıkar (akademik değil).
- §11.9 → timeScale tek-sahip-yazma DERSİNİ KORU; RTX5080/D3D12/FPS donanım-spesifiğini buda.
- §11.7 (meta-ilerleme) → silme, NESNELLEŞTİR: "entegrasyon testiyle yakalanan mantıksal-bağlantı hatası" çerçevesi.
- **YENİ §11 alt-bölüm EKLE — Tekil yönetici (singleton) yaşam-döngüsü hatası:**
  > Oyun-içi yeniden başlatma sonrası açılış yetenek draft'ı açılmıyordu. Kök neden: `DraftManager` tekil yöneticisinin statik `_shuttingDown` bayrağı, sahne yeniden yüklemesinde sıfırlanmadan açık kalıyordu — statik sıfırlama yalnızca oyun-modu girişinde çalışır, sahne yeniden yüklemesinde değil. Bu yüzden yeni sahnede örnek oluşsa bile `Instance` erişimcisi null dönüyor, açılış draft'ı tetiklenmiyordu. Çözüm: `OnDestroy` artık bu bayrağı set etmiyor (yalnızca gerçek uygulama kapanışında); bu, projede aynı hata-sınıfı için zaten kullanılan kanonik deseni (`AttackTokenManager`) genelleştirir. Aynı gizli (latent) hata üç kardeş tekil yöneticide (`RunStats`, `CheckpointManager`, `BuildTileBrushController`) de giderildi. Düzeltme çalışma-zamanında doğrulandı (yeniden başlatma → açılış draft'ı açılıyor). **Çıkarım:** çapraz-kesen bir yaşam-döngüsü hatası, tek bir referans desenin tutarlı yayılmasıyla giderilebilir; bağımsız bir denetim ajanı düzeltmeyi onayladı.
- **T7 savunma notu (kısa, §11 ilgili vakaya veya §4 timeScale bağlamına):** Açık bir yetenek draft'ı sırasında zaman-ölçeğini geri yükleyen yolun, draft duraklatmasını ezmesini önlemek için savunma amaçlı bir koşul eklendi (temiz akışta üretilemeyen bir yarış durumuna karşı sigorta).
- §11 alt-başlık sayısını dengele: zayıf/operasyonel olanları sadeleştir, güçlü mimari vakaları (off-map/WalkabilityMap, veto, boss-faz, singleton-T9) KORU. "Logbook" havasını azalt ama tez kanıtını gutlamama.

## BİTİRİNCE
`cd STAGING/report && python make_akademik_docx.py` → verify: gömülü görsel **13** (12 figür + kapak logosu), **placeholder 0**, figür numaraları 1-12 sıralı.

## ÇIKTI
Done notu: `STAGING/_process/2026-06/_done_report_combined_2026-06-19.md` (madde madde ne değişti + DOCX figür/placeholder durumu + şüpheli nokta).
Dönüşün ≤10 satır: ana değişiklikler + DOCX placeholder 0 mı + done-dosya yolu.