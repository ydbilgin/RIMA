# Council Sorusu — ChatGPT Deep Static Audit Değerlendirmesi (DEEP/ARCHITECTURE LENS)

Sen RIMA council'inin derin mimari danışmanısın. ChatGPT projenin GitHub reposunu STATİK okuyarak 15 bulguluk bir audit çıkardı (oyunu ÇALIŞTIRAMADI — bu önemli bir sınır). Görevin: audit'in mimari önerilerini değerlendirmek.

## Read these files
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/01_EXECUTIVE_SUMMARY.md`
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/02_FINDINGS_BY_SEVERITY.md`
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/04_PATCH_PLAN_ORDERED.md`
- `STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/08_CLEANUP_AND_RED_LIST.md`
- Bağlam: `CURRENT_STATUS.md` (İLK 45 satır — gece zinciri ve canlı sistemin ne olduğu)

## Bilinen bağlam (audit'in göremediği canlı gerçekler)
- Canlı akış play-verified: MainMenu→Chamber→_Arena; RoomRunDirector state-machine + IsoRoomBuilder.BuildExitDoors (NW/N/NE gate-slot, commit f63ac34c, ax-Opus review PASS) + DungeonGraph branching (1/2/3 kapı canlı kanıtlı)
- RoomLoader/RuntimeRoomManager = büyük ihtimalle legacy (eski sistem); cx ayrıca kanıtlayacak
- SkillDatabase.GetPool isImplemented filtresi BUGÜN kanıtlandı (iki path de filtreli)
- fig01-05 BUGÜN sahnelenmiş runtime karelerle yeniden çekildi; portal 1/2-exit in-game kanıtı var

## Soruların (4 soru)
1. **"Önce LiveFlowProof, sonra kod" yaklaşımı:** Audit'in F-001 framing'i ("blocker şüphe") statik-okuma sınırından kaynaklanıyor; yine de LIVE_FLOW_PROOF dokümanı kalıcı değer taşır mı, yoksa cx'in tek seferlik kanıtı yeterli mi? T3 öncesi zorunlu gate olmalı mı?
2. **F-007 Gate/RiftPortalView ayrımı (logic/visual separation):** T3 portal wiring'de bu refactor'ı ŞİMDİ yapmak mı (temiz mimari), yoksa mevcut canlı ExitDoor yapısına minimal skin-binding yapıp refactor'ı post-demo'ya bırakmak mı? Demo yaklaşırken trade-off'u değerlendir.
3. **Stale-guard stratejisi (F-003 + Commit-2):** Audit 6 dosyaya banner öneriyor. Alternatifler: (a) banner'lar, (b) AI_READER_GUIDE'a "stale dosyalar" bölümü eklemek (tek nokta), (c) ikisi. Dokümantasyon-zehirlenmesini en az bakım yüküyle ne çözer?
4. **Patch plan sırası (7 commit):** Sıralama mantıklı mı? Demo-kritiklik açısından senin sıralaman ne olurdu? Hangileri birleştirilebilir, hangisi gereksiz?

## Çıktı formatı
15 bulgu için tek satır: `F-NNN: ACCEPT / MODIFY(nasıl) / REJECT(neden)` + 4 sorunun cevabı (her biri ≤1 paragraf) + "T3 öncesi zorunlu minimum" listesi. Türkçe.
