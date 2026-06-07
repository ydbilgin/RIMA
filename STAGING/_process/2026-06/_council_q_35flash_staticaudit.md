# Council Sorusu — ChatGPT Deep Static Audit (LEAN / SHIP-FAST LENS)

Sen RIMA council'inin pragmatik danışmanısın. ChatGPT, repoyu STATİK okuyarak 15 bulgu + 7-commit'lik patch planı çıkardı. Demo teslimi yaklaşıyor; kullanıcı aynı anda silah üretimi + T3 portal wiring + rapor kapanışı yapıyor. Görevin: over-engineering eleme + minimum yol.

## Read these files
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/02_FINDINGS_BY_SEVERITY.md`
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/04_PATCH_PLAN_ORDERED.md`

## Bilinen bağlam (audit'in göremediği)
- Canlı akış play-verified: RoomRunDirector + IsoRoomBuilder.BuildExitDoors (NW/N/NE gate-slot) + DungeonGraph branching 1/2/3-kapı CANLI. RoomLoader/RuntimeRoomManager büyük ihtimalle legacy/dormant (cx kanıtlıyor).
- SkillDatabase isImplemented filtresi İKİ path'te de bugün KANITLANDI (F-004 büyük ihtimalle false-positive).
- fig01-05 bugün yeniden çekildi; portal 1/2-exit in-game görüntüsü mevcut (F-013/F-015 kısmen bayat).
- Weapon override kararı zaten ChatGPT silah-danışma turunda çözülmek üzere (F-009 paralel işte).

## Soruların (3)
1. **Severity yeniden notlandırma:** Yukarıdaki bağlamla 15 bulgudan hangileri FALSE-POSITIVE/bayat düşer? Kalan gerçek işler hangileri ve gerçek severity'leri ne? (Tablo: F-NNN · yeni severity · tek satır gerekçe)
2. **7 commit → kaça iner?** Hangi commit'ler birleşir/düşer? Örn: Commit-1 audit-utility TOOL yazmak yerine cx'in tek seferlik kanıt dokümanı yeter mi? Commit-3 gate-scale fix'i legacy dosyadaysa hiç gerekir mi? Önerdiğin minimal commit listesi + toplam süre tahmini.
3. **T3'ü bloklayan GERÇEK minimum ne?** "T3 portal wiring başlamadan önce şu 1-3 şey şart" listesi — gerisi T3 sonrası/paralel.

## Çıktı
(1) yeniden-notlandırma tablosu, (2) minimal commit planı + süre, (3) T3-öncesi zorunlu minimum. Kısa ve keskin. Türkçe.
