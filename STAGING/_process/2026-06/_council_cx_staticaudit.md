ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ChatGPT'nin DEEP STATIC AUDIT raporundaki (15 bulgu) İDDİALARI kod kanıtıyla doğrula/çürüt — özellikle "hangi sistem CANLI" sorusu. ANALYSIS ONLY, kod değişikliği YOK.

# READ these source files
- STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/01_EXECUTIVE_SUMMARY.md
- STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/02_FINDINGS_BY_SEVERITY.md
- STAGING/_incoming/deep_static_audit_2026-06-07/RIMA_DEEP_STATIC_AUDIT_2026-06-07/03_LIVE_FLOW_PROOF_SPEC.md

# Sub-questions (her biri için KANIT: dosya:satır)
S1. **LIVE FLOW PROOF (F-001/F-006 — EN KRİTİK):** Gerçek canlı demo akışı hangisi? Kanıt zinciri çıkar: (a) MainMenu→Chamber→_Arena akışında oda kim kuruyor (RoomRunDirector? IsoRoomBuilder? EncounterController?); (b) `RoomLoader`/`RoomSequenceData`/`Gate.cs` HERHANGİ bir canlı sahnede/prefab'da referanslı mı yoksa legacy mi (rg ile caller ara; _Arena/_IsoGame/MainMenu sahne dosyalarında component var mı bak); (c) çıkış kapılarını kim spawn ediyor — `IsoRoomBuilder.BuildExitDoors` (gate-slot f63ac34c) mi `RoomLoader.BuildRoomContent` mi; (d) dallanma: ExitDoor tıklaması → DungeonGraph child seçimi zinciri (RoomRunDirector.AdvanceTo?) gerçekten 1/2/3-çıkış choice-index taşıyor mu. SONUÇ: 03_LIVE_FLOW_PROOF_SPEC.md'deki formatta verdict yaz (ROOMLOADER_LIVE | TEMPLATE_BUILDER_LIVE | MIXED) → çıktıyı CODEX_DONE'a göm (ayrıca STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md dosyasına da yaz — bu istisna olarak dosya yazımı SERBEST, analiz dokümanı).
S2. **F-002 Gate root scale:** `Gate.OpenAnimCoroutine` root scale değiştiriyor iddiası — Gate.cs canlı path'te mi (S1'e bağlı)? CANLI çıkış kapısının (ExitDoor_*) açılış animasyonu var mı, varsa root mu child mı scale ediyor? Severity verdict ver.
S3. **F-004 SkillDatabase:** `GetPool`'un isImplemented filtresi (SkillDatabase.cs ~580) — İKİ path de (Instance + serialized fallback) filtreli mi, kanıtla. Ayrıca audit'in andığı isimler (Backstab, Shadow Step, Fan of Knives, Aimed Shot, Disengage, Multi Shot) DB'de var mı, isImplemented değerleri ne, hangi sınıfta? NLM'e sor: bu isimler canonical skill-pool'da var mı yoksa revoked/eski isim mi? Verdict: gerçek drift mi, false-positive mi?
S4. **F-003 SYSTEM_MAP:** SYSTEM_MAP.md (kökte veya .claude/) gerçekten RuntimeRoomManager/physical-door anlatıyor mu — 3-5 satır kanıt al. Stale-guard banner gereksinimi doğru mu?
S5. **F-005/F-007:** Canlı path'te portal görseli şu an ne (IsoRoomBuilder hangi sprite'ı kullanıyor — gateNorthSprite? generic gate_arch?); GateBehavior/DoorTrigger canlıda mı legacy mi? T3 binding hedefi hangi dosya/method olmalı?
S6. **Audit'in atladıkları:** 02_FINDINGS'te bariz YANLIŞ veya bayat başka iddia var mı (bizim 2026-06-07 gece-zinciri commit'leriyle çelişen)?

# Çıktı
CODEX_DONE'a: S1-S6 verdict'leri kanıtla + bulgu-bazlı tablo (F-001..F-015: VERIFIED-TRUE / FALSE-POSITIVE / PARTIAL + tek satır kanıt). LIVE_FLOW_PROOF dosyasını da yaz.
