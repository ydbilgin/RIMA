# TASK 9 — Ödül-ekranı monolog-bleed fix (cerrahi, ~30-45dk)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.

## Sorun (kullanıcı bildirdi + auditor kök-nedenledi)
ÖDÜL SEÇ ekranında (SkillOfferUI, 3 kart) arka planda `RoomMonologController`'ın cyan flavor/monolog metni ("Bu...değil. Sadece dur...") kartların arkasından SIZIYOR.
**Kök-neden (auditor, statik-doğrulandı):** sorting DOĞRU (scrim sortingOrder=1050 > monolog 1030) AMA reward scrim alfası `RimaUITheme.OverlayDark=0.85` (`RimaUITheme.cs:78`) = **%15 saydam** → altındaki cyan monolog metni scrim'den geçiyor. Monolog `RoomMonologController.cs:194` (sorting), `:214,229` (pozisyon).

## Fix — SEÇENEK B (semantik, scrim'e dokunma)
**Draft/reward (SkillOfferUI) açıkken `RoomMonologController`'ın görünür group'larını gizle**, kapanınca eski haline döndür. Mantık: ödül-seçim sırasında oda-flavor metni zaten görünmemeli.
- En temiz coupling: `SkillOfferUI` Show/Open → `RoomMonologController`'a "gizle" çağrısı (public metot ekle, ör. `SetSuppressed(bool)` → group'ları SetActive(false)/restore). Close → restore.
- VEYA RoomMonologController bir draft-open event'i dinlesin. Hangisi mevcut mimariye daha cerrahi oturuyorsa onu seç (önce oku, varsayma).
- ⚠️ Global `OverlayDark` alfasını DEĞİŞTİRME (tüm modalları etkiler — pause vb. aşırı kararır). Reward-scrim'e lokal alfa da bir alternatif ama monolog-suppress daha temiz; sen karar ver, gerekçesini yaz.

## Kısıt
- Cerrahi: `SkillOfferUI.cs` + `RoomMonologController.cs` (gerekirse). RimaUITheme global'e DOKUNMA. Başka UI'a dokunma.
- Monolog'un normal (reward-dışı) görünürlüğü/akışı DEĞİŞMESİN — sadece reward-açıkken bastır.
- git'e DOKUNMA.

## ⚠️ VERIFY — GERÇEK-AKIŞ (forced-path açığını kapat)
Önceki test forced-auto path kullandı, monolog taşımıyordu → bleed'i göremedi. Sen:
- **Monolog AKTİF olacak şekilde** reward ekranına gel: normal oda akışı (oda gir → monolog tetiklensin → oda temizle → reward açılsın), VEYA monolog'u manuel tetikleyip sonra reward'ı aç.
- Reward açıkken screenshot/assert: monolog metni kartların arkasında GÖRÜNÜYOR mu? Fix sonrası **GÖRÜNMEMELİ.**
- Reward kapanınca monolog normal davranışına dönüyor mu (gerekiyorsa).
- `read_console` 0-error.

## ÇIKTI (E1: ≤10 satır)
Evidence + screenshot → `STAGING/_process/2026-06/demo_fix_tasks/DONE_9_bleed.md`. Dönüşte: değişen dosyalar + hangi yaklaşım (suppress/alfa) + gerçek-akış verify (bleed GİTTİ mi, kanıt) + console durumu.
