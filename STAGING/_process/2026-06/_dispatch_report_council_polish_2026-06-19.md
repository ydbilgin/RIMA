# GÖREV: RIMA Akademik Bitirme Raporu — Council Polish Review (READ-ONLY)

Sen bağımsız bir akademik danışmansın. Bir bilgisayar mühendisliği **bitirme projesi (senior design)** raporunu jüri gözünden inceleyeceksin.

## 🔒 READ-ONLY — KESİN
- ❌ HİÇBİR dosyayı düzenleme/oluşturma. ❌ `git` çalıştırma (add/commit/push HİÇBİRİ). ❌ DOCX üretme, script çalıştırma.
- ✅ Sadece OKU + değerlendir. Bulguları kendi STDOUT'una + done dosyana yaz.
- (Geçmişte bir advisor `git add .` ile her şeyi commit'ledi — SAKIN. Sen sadece okuyup rapor bırak.)

## İNCELENECEK
- Tek dosya: `STAGING/report/RIMA_Senior_Design_Report.md` (~72KB, tam akademik Türkçe rapor). TAMAMINI oku.
- Rapor 12 figür içerir (Şekil 1-12). Figür satır formatı: `[Şekil N: caption | dosya.png]`.
- Bağlam (gerekirse): bu RIMA = Unity 2D top-down ARPG roguelite bitirme projesi; tez = "oyun + yazılım mimarisi + AI-destekli geliştirme disiplini" (sadece prompt-engineering DEĞİL). Rapor daha önce 2 review turu gördü; bu = son **cila** turu.

## ÇIKTI: TÜM TÜRKÇE KARAKTER ZORUNLU (ç ğ ı İ ö ş ü), akademik Türkçe, ASCII-leştirme YOK.

## DEĞERLENDİRME LENSLERİ
1. **Akademik yapı & akış:** bölüm sıralaması, geçişler, eksik/zayıf/tekrarlı bölüm, başlık tutarlılığı.
2. **İddia ↔ kanıt:** desteksiz/abartılı iddia; "prompt engineering raporu" havası; kalan LLM-vari şişme/coşku cümleleri (ör. "en güçlü anlatılardan biri", gereksiz "ancak/dolayısıyla" zincirleri).
3. **Figür tutarlılığı:** 12 figürün caption'ı içerikle uyumlu mu; gövdedeki "Şekil N" atıfları doğru mu; eksik/fazla/yanlış-numara var mı.
4. **AI-odağı dengesi:** oyun tasarımı + yazılım mimarisi yeterince öne çıkıyor mu; AI-süreç dengeli mi yoksa baskın mı.
5. **Türkçe akademik dil kalitesi:** terminoloji tutarlılığı, çeviri-kokusu, cümle netliği.
6. **Jüri gözünden en kritik 3-5 zayıflık** + her biri için SOMUT düzeltme önerisi.

## ROL FARKI
- **(ax_pro / Gemini 3.1 Pro):** derin yapısal + akademik + figür-caption↔içerik mantığı. Detaylı, gerekçeli.
- **(ax_flash / Gemini 3.5 Flash):** yalın/lean lens — fazlalık/şişme/tekrar nerede, neyi KESELİM, jüri neyi gereksiz bulur.

## FORMAT
Bulguları öncelikli ver: **P1 (kritik) / P2 / P3**. Her bulgu: `bölüm/başlık-anchor` + sorun + önerilen düzeltme (kısa). Sonunda "en kritik 3" özeti.
Done dosyana yaz (ax_pro → `STAGING/_process/2026-06/_council_axpro_report_polish_2026-06-19.md`; ax_flash → `STAGING/_process/2026-06/_council_axflash_report_polish_2026-06-19.md`).
Dönüşün ≤10 satır: P1'ler + en kritik 3 + done-dosya yolu.
