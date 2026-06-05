# Görev — Gemini 3.5 Flash: Sodaman araştırmasını Laureth Studio'ya kopyala + CURRENT_STATUS'e pointer ekle

Sen Gemini 3.5 Flash'sın. Bu görev DOSYA İŞLEMİ — analiz değil. Terminal/dosya araçlarınla GERÇEKTEN şu 3 adımı yap, sonra ne yaptığını raporla. ASCII kullan (Türkçe özel karakter yok).

## Mutlak yollar (aynen kullan)
- KAYNAK araştırma dökümanı: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\SODAMAN_LEARNINGS_DECISION_2026-06-04.md`
- HEDEF klasör: `F:\laurethstudio`
- STATUS dosyası: `F:\Antigravity Projeler\2d roguelite\RIMA\CURRENT_STATUS.md`

## ADIM 1 — Hedef klasörü garanti et
`F:\laurethstudio` klasörü yoksa oluştur (PowerShell: `New-Item -ItemType Directory -Force -Path 'F:\laurethstudio'`).

## ADIM 2 — Araştırmayı kopyala
KAYNAK dökümanı `F:\laurethstudio\SODAMAN_LEARNINGS_DECISION_2026-06-04.md` olarak kopyala (orijinali STAGING'de KALSIN — taşıma değil, kopyalama).
PowerShell: `Copy-Item -Path 'F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\SODAMAN_LEARNINGS_DECISION_2026-06-04.md' -Destination 'F:\laurethstudio\SODAMAN_LEARNINGS_DECISION_2026-06-04.md' -Force`
Kopyanın oluştuğunu doğrula (Test-Path).

## ADIM 3 — CURRENT_STATUS.md SONUNA pointer EKLE (append)
SADECE dosyanın EN SONUNA aşağıdaki satırları EKLE (append). Dosyanın geri kalanına DOKUNMA, hiçbir şeyi silme/değiştirme. Bu notu SEN (Gemini 3.5 Flash) eklediğini birinci ağızdan belirt.

Eklenecek metin (aynen, sonuna bir boş satır bırakarak append et):

```
---

> 📎 [Gemini 3.5 Flash — 2026-06-05] Sodaman arastirma dokumanini (`SODAMAN_LEARNINGS_DECISION_2026-06-04.md`) `F:\laurethstudio` klasorune kopyaladim. Bu pointer'i da ben ekledim (Claude degil).
```

Append yöntemi (PowerShell, UTF8, dosya sonuna ekler, üzerine YAZMAZ):
`Add-Content -Path 'F:\Antigravity Projeler\2d roguelite\RIMA\CURRENT_STATUS.md' -Value "...yukaridaki metin..." -Encoding UTF8`

## RAPOR
Bittiğinde kısaca yaz: (a) klasör vardı/oluşturuldu, (b) kopya yolu + Test-Path sonucu, (c) CURRENT_STATUS.md son N satırını göster (append'in eklendiğini kanıtla). Bir adım başarısızsa hangisi ve neden (izin/yol hatası) açıkça yaz.
