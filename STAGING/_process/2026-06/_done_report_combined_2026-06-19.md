# DONE — Rapor Birleşik Pass (2026-06-19)

## Uygulanan Değişiklikler

### Edit-spec UYGULA maddeleri
1. **Şekil atıfları:** 12 figürün tamamına gövde paragrafına "(bkz. Şekil N)" / "Şekil N'de görüldüğü üzere" atıfları eklendi.
2. **§5 başlığı:** "(Centerpiece)" kaldırıldı.
3. **Jargon/İngilizce:** run→koşu (run), build→yapı (build), void→harita dışı geçersiz alan (void), Idle→boşta (Idle), spawn→üretim (spawn), "yalan telegraph"→telegraph-hasar uyumsuzluğu, "az iş çok his"→minimum kaynakla yüksek oyun hissi, clobber→üzerine yazma, tooling environment→geliştirme ortamı (ilk kullanım + tek gloss), flip→çevirme, "az iş çok his" kaldırıldı.
4. **AI dili / model adları:** §8.2 tablosu soyutlandı (Kod Üretim Modülü / Kalite Kontrol Ajanı / Mimari Çapraz-Doğrulama); §8.6 prose'dan builder-opus/crafter-sonnet/cx/ax kaldırıldı, rol terminolojisi eklendi; model adları TEK dipnot (¹) olarak §8.6 sonuna işlendi. "Danışman Konseyi" → "Mimari Değerlendirme Paneli". "Anlaşmazlıktan değer üretme" → "farklı modellerin çapraz denetimiyle risk tespiti". Metodoloji katkısı KORUNDU.
5. **Ham JSON/QC-log:** §6.4 iki JSON code-block → şema tablosu + metin özeti; §9.3 ham log → doğrulama tablosu (3 satır × 5 sütun).
6. **Bold → ####:** §4.1 yedi bold paragraph-başı #### alt-başlığa çevrildi.
7. **§12.1 mükerrer istatistik:** Önceden geçen 26 oda / 549 test / 80 sprite tekrarları kaldırıldı; bölüm üç gruba sadeleştirildi ve "bkz. §9 ve §10" yönlendirmesi eklendi.
8. **Şekil 12 gerekçe:** Grafın hangi mimari karara dayanak oluşturduğu (araç/runtime katman bağımsızlığı) somut cümleyle açıklandı.

### §11 Birleşik Pass
9. **"Ders:" → "Çıkarım:":** §11 tümünde (replace_all).
10. **§11.1 donanım-spesifik:** Tek paragrafa indirildi; RTX5080/D3D12 spesifik teknik ayrıntı kaldırıldı.
11. **§11.9 donanım budama:** RTX5080/D3D12/FPS donanım-spesifiğini buda; timeScale tek-sahip-yazma dersi KORUNDU.
12. **§11.7 reframe:** "Dikkatsizlik" algısı kaldırıldı; "entegrasyon testiyle yakalanan mantıksal-bağlantı hatası" çerçevesi eklendi.
13. **YENİ §11.10:** Tekil yönetici (singleton) yaşam-döngüsü hatası (T9 vakası) — DraftManager statik bayrak / sahne yeniden yüklemesi / üç kardeş singleton / çalışma zamanı doğrulaması / bağımsız denetim ajanı onayı. Çıkarım: kanonik desen tutarlı yayılması.
14. **YENİ §11.11:** Savunma amaçlı koşul (T7 notu) — zaman-ölçeği geri yükleme yolunun draft duraklatmasını geçersiz kılmaması; §11.9 ile ilişkisi açıklandı.

## DOCX Doğrulaması
- Gömülü görsel: **13** (12 figür + kapak logosu) ✓
- Placeholder: **0** ✓
- Heading 1 sayısı: 13 (§1–§12 + Kaynakça) ✓
- Dosya boyutu: 9570 KB ✓

## Şüpheli Nokta
- §4.6 alt-bölümlerindeki bold paragraph başları değiştirilmedi (edit-spec yalnızca §4.1 ve §8.6 belirtiyor; §4.6 zaten farklı formatta). Eğer §4.6 bold'ları da #### isteniyorsa ek pass gerekir.
- "Build Mode" / "Director Mode" öz-adları korundu (edit-spec: ilk kullanımda Türkçe gloss — Türkçe karşılıkları parantez ile vardı, dokunulmadı).
