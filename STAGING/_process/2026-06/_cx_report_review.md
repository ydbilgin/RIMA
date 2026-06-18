ACTIVE RULES: (1) önce düşün, sonra denetle (2) DÜZENLEME YOK — sadece REVIEW (3) cerrahi kapsam: yalnız rapor + iddia-doğrulama (4) belirsizse BLOCKED yaz.

READ-ONLY HARD KISIT: Bu bir DENETİM görevi. Raporu/kodu DÜZENLEME. HİÇBİR git komutu (add/commit/push/checkout) ÇALIŞTIRMA. (Geçmişte bir advisor rogue `git add .` yaptı — KESİNLİKLE YASAK.) İZİN VERİLEN tek yazma: kendi bulgu dosyan (aşağıda).

# Amaç
RIMA Senior Design Report'un (akademik bitirme raporu, KTO formatı, **Türkçe**) demo-öncesi SON eleştirel METİN denetimi. YARIN jüriye sunulacak — bu son geçit. Amaç: içerik onayı + öncelikli düzeltme listesi.

## Oku
- Rapor (738 satır): `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\report\RIMA_Senior_Design_Report.md`
- İddia doğrulama için kod tabanına bakabilirsin: `Assets/Scripts/**` (Grep/Read).
- GRAPHIFY: cross-file/mimari iddia için önce graph.json sorgula (`STAGING/_process/2026-06/graphify_fullmap/graphify-out/`) — bulk-read'den ~71x ucuz.

## Denetim boyutları (her bulguya KANIT + satır no zorunlu)
1. **İDDİA DOĞRULUĞU (EN KRİTİK):** Rapor, kodda OLMAYAN bir özellik/sistem iddia ediyor mu? Sayılar doğru + tutarlı mı? Kodla/graf ile çapraz-kontrol et: 6925 node, 118 community, 10 oynanabilir sınıf, 12 düşman, 8-yön sprite, 6 oda, vb. Overclaim / kanıtsız iddia = **P0**.
2. **AKADEMİK YAPI:** Senior design report bölüm akışı eksiksiz + mantıklı mı (özet → giriş → tasarım → inşa → görsel → süreç → doğrulama → zorluklar → sonuç)? Boşluk/atlama?
3. **İÇ TUTARLILIK:** Şekil 1-12 referansları metinle eşleşiyor mu (doğru şekil doğru yerde)? Aynı sayı/terim her yerde aynı mı? Çelişki var mı?
4. **DİL:** Tam Türkçe karakter (ç ğ ı ş ö ü İ) eksiksiz mi (mojibake/ASCII-bozulma yok)? ChatGPT-vari / AI-slop ifade var mı (şişirme sıfatlar, "sorunsuz biçimde", gereksiz rule-of-three, boş geçiş cümleleri)?
5. **TON:** §8.6 İnsan-YZ iş bölümü dengeli mi, yoksa AI-katkısını abartıyor mu? (Akademik bitirme raporunda öğrenci-katkısı önde olmalı.)

## Çıktı
Bulguları şu dosyaya YAZ: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_process\2026-06\report_council\cx_findings.md`
Format: en üstte **VERDICT: PASS / PASS-WITH-FIXES / FAIL**, sonra tablo: `| Öncelik(P0/P1/P2) | Satır | Sorun | Önerilen düzeltme |`.
Dönüşte (stdout) ≤15 satır: VERDICT + P0 ve P1 başlıkları (detay dosyada).
