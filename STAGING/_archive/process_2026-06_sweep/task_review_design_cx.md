# TASK: Katmanlı iso loop tasarımı — TEKNİK REVIEW (cx/Codex)

ACTIVE RULES: (1) think before reviewing (2) min output, no speculation (3) surgical — sadece oku + 1 review dosyası yaz, KOD DEĞİŞTİRME (4) belirsizse açıkça yaz.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read: STAGING + Assets/Scripts (kod).

Amaç: Opus'un yazdığı `STAGING/LAYERED_ISO_LOOP_DESIGN_S6.md` tasarımını TEKNİK fizibilite açısından review et. Kod-hook'ları gerçek mi, riskler ne, açık soruların teknik cevabı ne. Sen REVIEWER'sın (writer≠reviewer) — kod yazma, sadece denetle.

## OKU
1. `STAGING/LAYERED_ISO_LOOP_DESIGN_S6.md` (tasarım — ana hedef)
2. `Assets/Scripts/Core/DungeonGraph.cs` (run graph)
3. `Assets/Scripts/Core/RuntimeRoomManager.cs` (legacy loader, _IsoGame'de aktif — OnPlayerEnteredDoor→Navigate var mı doğrula)
4. `Assets/Scripts/Systems/Map/RoomLoader.cs` (linear loader)
5. `Assets/Scripts/Environment/Portal.cs` (portal stub)
6. CliffAutoPlacer (grep: `Assets/Scripts/**/CliffAutoPlacer*`) + DirectionalCliffTile — cliff yerleşim gerçekte ne yapıyor, sprite overflow var mı, kaç yön kullanıyor

## REVIEW ET (her bölüm A-F için: FEASIBLE / RISK / FIX)
- **A Katman modeli:** sorting layer'lar custom-axis (0,1,0) pivot Y-sort ile çatışır mı? PreviewBand/FarParallax/Cliffs ekran-Y ile sıralanan Entities'ten BAĞIMSIZ sortingLayer olabilir mi yoksa Y-sort onları karıştırır mı?
- **B Boundary:** 4-köşe iso elmas PolygonCollider2D yaklaşımı sağlam mı? Mevcut Walls/CompositeCollider2D zaten iso sınır sağlıyor mu (RoomLoader/RuntimeRoomManager onu nasıl kuruyor)? Player CapsuleCollider2D hangi layer, hangi layer'la collide?
- **C Graph-aware loader:** RuntimeRoomManager mı RoomLoader mı omurga olmalı? RRM gerçekten OnPlayerEnteredDoor→DungeonGraph.Navigate yapıyor mu (satır ver)? Graph→oda içeriği köprüsü nerede kurulmalı?
- **D Portal:** Portal.cs'i Configure(ExitChoice) ile genişletmek temiz mi? OnEntered→travel→Navigate→load zinciri double-trigger riskine karşı nasıl korunur?
- **E Preview adalar:** statik prefab band'de — collider/AI/spawner OLMADAN nasıl garanti edilir? RoomData→runtime composer var mı yoksa demo elle prefab mı?
- **F Orb travel:** PlayerController + PlayerAttack disable + visualRoot gizleme — Player hiyerarşisinde visualRoot var mı? state restore riski nerede?

## AÇIK SORULARA TEKNİK CEVAP (design doc'taki 1-7)
Özellikle: #1 boundary, #4 loader seçimi, #6 cliff overflow (CliffAutoPlacer gerçek davranış), #3 preview detay.

## ÇIKTI: `STAGING/REVIEW_DESIGN_CX.md`
- Her bölüm: VERDICT (FEASIBLE/CONCERN/BLOCKER) + gerekçe + dosya:satır + somut FIX.
- Build order önerisi (M1-M4 doğru mu, sıra değişmeli mi).
- En riskli 3 nokta.
- Açık sorulara önerilen cevap.
ASCII yaz, Türkçe.
