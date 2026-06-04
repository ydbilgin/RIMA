ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Oda-tasarım council — FEASIBILITY/REUSE/WHAT-WE-HAVE lens. ANALYSIS ONLY, kod yok.

READ: STAGING/ROOM_DESIGN_COUNCIL_BRIEF_2026-06-04.md (tam bağlam, 6 soru).
ChatGPT pack: STAGING/chatgpt_rooms_pack/rima_large_rooms_pack/ (rooms JSON + ROOM_PREVIEW.md).

Cevapla (feasibility/reuse lens):
1. **Mevcut kütüphaneyi AUDIT et:** `Assets/Data/Rooms/Library/*.asset` (Combat_Small/Medium/Large, Boss_Intro_01, Elite_01, Corridor_Linear/LShape, Shrine_01, Spawn_01, Treasure_01). HER BİRİ için: bounds (w×h), walkableGrid VAR mı (şekilli mi yoksa boş=full-rect mi?), kaç doorSocket/enemySpawnSocket/playerSpawn. Yani şu an odalarımız ŞEKİLLİ mi yoksa düz dikdörtgen mi?
2. **Ingest path:** ChatGPT pack'i (RoomJsonImporter zaten yazıldı, `Assets/Editor/Rooms/RoomJsonImporter.cs`) RoomTemplateSO'ya çevirmek + IsoRoomBuilder ile render + RoomBankSO/MapFlowManager run-flow'a sokmak — hangi adımlar gerekli, ne kadar iş (S/M/L)? IsoRoomBuilder bu büyük şekilleri (38w) kaldırır mı?
3. **Reuse vs build:** ChatGPT şekillerinden hangileri elimizdekiyle ÖRTÜŞÜYOR (diamond/cross/L/hourglass/donut zaten legacy sahnelerde var), hangileri YENİ değer (teardrop/blob/twin-basins/trident/crescent/zigzag)? Boyut/tip boşlukları (small/med shape, shrine/merchant/event) için ne lazım?
4. **Run-flow entegrasyonu:** mevcut MapFlowManager (sahne-bazlı) vs RoomBankSO/IsoRoomBuilder (veri-bazlı) — oda havuzunu veri-bazlıya taşımak için minimum yol ne? (B-12 ile ilgili.)

CODEX_DONE.md'ye yaz. Önceki audit'i tekrarlama. KISA.
