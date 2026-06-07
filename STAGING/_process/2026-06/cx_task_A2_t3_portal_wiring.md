ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
A2 / T3 — Portal görsel wiring'i: imagegen portal pack'ini CANLI çıkış-kapısı yoluna bağla (STATICAUDIT_DECISION_2026-06-07 adım 4/4; üretim hazırlığı=TASK_portalpack_production_2026-06-07.md — OKU).

# ⚠️ KULLANICININ KESİN KURALLARI (ihlal = FAIL)
1. RoomLoader/Gate.cs path'ine DOKUNMA — legacy (başlarında LEGACY mührü var).
2. Hedef: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` → `CreateExitDoorObject` (+ `BuildExitDoors`) + _Arena serialized sprite ref'leri.
3. N soketi = FRONTAL kemer · NW = AÇILI kemer · NE = açılı kemerin runtime flipX'i.
4. Rün/badge/label FLIP EDİLMEZ (ayrı child'da tut; sadece kemer gövdesi flip'lenir).
5. Portal tipleri: Combat, Elite, Chest/Reward, Boss. Heal/Lore YOK.
6. Event yok (DungeonGraph guard'ı dün eklendi — Event üretilmiyor; yine de default-case'i Combat görseline düşür).
7. Açılış/unlock animasyonu SADECE child visual'da — root/collider scale DEĞİŞMEZ (regression test şart).

# Kanıt/bağlam
- Canlı yol kanıtı: STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md (şu an her sokete AYNI gateNorthSprite atanıyor — IsoRoomBuilder.cs:805-818; trigger=RoomRunExitDoorTrigger, choice-index zinciri RoomRunDirector.cs:969-985/1032-1040 — bu zincir BOZULMAYACAK).
- Asset kaynağı: `STAGING/imagegen/assets/portal_pack_2026-06-07/` + `portal_pack_batch2_2026-06-07/` (manifest.md'leri oku). elite_v1=DEPRECATED import etme; boss-angled=PARKED import etme; 32px rünler=NEEDS_REGEN import etme (mevcut rün sprite'ları neyse o kalır).

# İşler
1. **Import:** Kemerleri `Assets/Art/Portals/` altına (frontal+angled × combat/elite/chest/boss), telegraph setini `Assets/Art/Telegraphs/` altına (AYRI — wire etme, sadece import), boss intro-ring/ritual-circle `Assets/Art/Boss/`. Import ayarları: Point, PPU 64, uncompressed, mip yok (skill-ikon fix'i 7826cb10 ile aynı kalıp). PORTAL_PACK_MANIFEST.json yaz (path/type/facing/status alanlarıyla — TASK_portalpack_production şeması).
2. **Skin eşlemesi:** Tip×cephe eşleme yapısı — MEVCUT mimariye en uygun minimal çözümü seç: IsoRoomBuilder'a serialized sprite alanları (mevcut gateNorthSprite kalıbının genişletilmesi: frontal/angled × 4 tip) VEYA küçük bir PortalSkin tablosu/SO. REUSE-first: sıfırdan paralel sistem YOK; seçimini gerekçele. _Arena sahnesindeki serialized ref'leri yeni sprite'lara bağla.
3. **CreateExitDoorObject güncelle:** slot index'e göre cephe seç (0=NW açılı, 1=N frontal, 2=NE açılı+flipX); doorType'a göre tip sprite'ı; rün child'ı flip'lenmez; collider/trigger boyut-konum davranışı AYNEN korunur (LIVE_FLOW_PROOF'taki zincir).
4. **Görsel doğrulama:** _Arena'da 1/2/3-çıkışlı üç template'i kur (diamond/donut/crescent probe kalıbı) + boss odası (center-only) → her birinden screenshot al `STAGING/_process/2026-06/t3_verify/` altına. NE kapısında rünün DÜZ olduğunu karede doğrula.
5. **Testler:** (a) slot→cephe eşleme unit testi; (b) root-scale regression (unlock anim sırasında root localScale==1 ve collider bounds sabit — anim yoksa da assert kalsın); (c) mevcut RoomTemplateSocketTests + smoke 26/26 yeşil kalır. EditMode'da koş.
6. Compile temiz. COMMIT YAPMA (orchestrator yapar). PORTAL_IMPORT_SETTINGS.md + PORTAL_SOCKET_PLACEMENT_GUIDE.md kısa dokümanlarını da yaz (TASK_portalpack_production'daki içerik listesi; Türkçe metinler TAM Türkçe karakterle).

# Çıktı
CODEX_DONE'a: iş 1-6 DONE/BLOCKED + seçilen skin-eşleme mimarisi ve gerekçesi + değişen dosya:satır + test sonuçları + 4 verify screenshot path'i.
