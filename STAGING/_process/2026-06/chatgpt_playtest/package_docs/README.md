# RIMA — Playtest Bugs, UI/UX Polish, Asset Pack and Implementation Package

Bu paket, bu sohbet boyunca bildirilen **gerçek playtest sorunlarını**, kullanıcının gönderdiği **altı özgün ekran görüntüsünü**, ChatGPT Image ile üretilen **oyun ekranı seviyesinde görsel polish örneklerini**, önerilen **modüler UI asset pack yapısını**, **Rift-Forged Egg / world reward** yaklaşımını ve Claude'un projede izlemesi gereken teknik yolu tek ZIP içinde toplar.

## Claude için ilk dosya

`00_READ_FIRST/00_CLAUDE_START_HERE.md`

## Paket ilkesi

- Görseller **final bitmap olarak doğrudan oyuna atılacak assetler değildir**. Hedef kalite, hiyerarşi ve malzeme dilini gösterir.
- UI, Unity uGUI + TMP + 9-slice + ayrı state overlay'leri ile modüler kurulmalıdır.
- Önce P0 oynanış hataları, sonra UI layout, en son görsel polish yapılmalıdır.
- `08_CANONICAL_REFERENCE_DOCS/` içindeki bazı belgeler kendi içinde `STALE` etiketi taşır. Öncelik sırası ayrı dosyada açıklanmıştır.
