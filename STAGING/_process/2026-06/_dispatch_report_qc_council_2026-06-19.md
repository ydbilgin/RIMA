# GÖREV: REVİZE RIMA Akademik Raporu — QC Council (READ-ONLY)

Bağımsız akademik danışman. Bir bilgisayar mühendisliği **bitirme (senior design)** raporunu JÜRİ gözünden incele. Rapor YENİ bir revizyondan geçti — revizyonların doğru oturduğunu doğrula + KALAN eksikleri yakala.

## 🔒 READ-ONLY — KESİN
❌ Dosya düzenleme/oluşturma YOK. ❌ `git` (add/commit/push) HİÇBİRİ. ❌ DOCX/script çalıştırma YOK. ✅ Sadece OKU + bulguları done dosyana yaz. (Geçmişte bir advisor `git add .` yaptı — SAKIN.)

## İNCELE
- Tek dosya: `STAGING/report/RIMA_Senior_Design_Report.md` (~72KB). TAMAMINI oku. 12 figür (Şekil 1-12).
- **Bu revizyonda yapılanlar (doğru oturmuş mu KONTROL ET):** (1) gövdeye 12 figüre "(bkz. Şekil N)" atıfları eklendi → doğru numara/yerde mi? (2) §5 başlığından "(Centerpiece)" çıktı; jargon akademikleşti (run→koşu, void→harita dışı geçersiz alan, vb.) → tutarlı mı, kaçak kaldı mı? (3) AI model adları (gpt-5.5/builder-opus/cx/ax) prosadan çıkıp §8.6'da TEK dipnota indi, metodoloji katkısı korundu → denge doğru mu, prompt-engineering havası gitti mi? (4) §11: "Ders:"→"Çıkarım:", §11.1/11.9 donanım-debug budandı, §11.7 reframe, YENİ §11.10 (singleton T9) + §11.11 (T7) → teknik doğru + nesnel mi, §11.3 ile §11.10 gereksiz tekrar mı?

## ÇIKTI: TÜM TÜRKÇE KARAKTER ZORUNLU (ç ğ ı İ ö ş ü), akademik Türkçe.
- **(ax_pro/Gemini Pro):** derin yapısal + figür caption↔içerik + akademik mantık.
- **(ax_flash/Gemini Flash):** yalın lens — kalan fazlalık/şişme/tekrar, jüri neyi gereksiz bulur.
- Bulgular öncelikli: **P1/P2/P3**, her biri `bölüm-anchor` + sorun + somut düzeltme. Sonunda net **verdict: rapor demo-hazır mı / eksik VAR mı**.
- Done dosyana yaz (ax_pro → `STAGING/_process/2026-06/_council_axpro_report_qc_2026-06-19.md`; ax_flash → `STAGING/_process/2026-06/_council_axflash_report_qc_2026-06-19.md`).
Dönüşün ≤10 satır: P1'ler + verdict + done-dosya yolu.
