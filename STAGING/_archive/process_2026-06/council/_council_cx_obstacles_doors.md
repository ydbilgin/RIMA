ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA izo odalarına gerçek (placeholder olmayan) ENGEL/PROP + 3-yön KAPI görselleri üretmeden ÖNCE, FEASIBILITY/REUSE lens'iyle: RIMA'da bu görselleri tutan/yerleştiren sistem ZATEN var mı, üretilen PNG'ler nasıl prop/obje olur, ve asset-pack vs tek-tek import hangisi daha az risk?

ANALYSIS ONLY — kod DEĞİŞTİRME. Sonucu CODEX_DONE.md'ye yaz. Önceki bir audit'i TEKRAR ÜRETME.

## Bağlam
- Eski placeholder'lar: `Assets/Prefabs/Obstacles/{StoneColumn,NarrowPassage,Chasm}.prefab` (jenerik, "anlamsız").
- Yeni runtime builder az önce eklendi: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (floor+cliff+boundary+door/spawn MARKER üretir; prop YERLEŞTİRMİYOR).
- `RoomTemplateSO.props` = `List<RIMA.MapDesigner.Props.PropPlacementData>` (propDefinitionGuid, tilePosition, rotationSteps, variantIndex).
- Iso cell = 0.96×0.585 world @ PPU64 (~61×37px elmas taban).

## Sub-sorular (feasibility/reuse lens — DOSYALARI OKU, satır/path ver)
1. **Prop sistemi REUSE:** `Assets/Scripts/MapDesigner/Props/` altında ne var? (PropPlacer / PropRuntimeSpawner / PropFootprintValidator / BridsonPoissonAutoPlacer / PropDefinition SO?) Bir "PropDefinition" SO + GUID kayıt mekanizması var mı (PropPlacementData.propDefinitionGuid neyi referanslıyor)? Üretilen bir sprite'ı PROP olarak kullanmak için NE gerekiyor (SO yarat + sprite ata + GUID registry)? Yani obstacle = yeni prefab MI olmalı yoksa mevcut PropDefinition SO akışına mı girmeli? Dosya/satır ver.
2. **KAPI yerleştirme:** IsoRoomBuilder şu an door MARKER (boş Transform) koyuyor; gerçek kapı görseli/Trigger wiring P5. Door sprite'ı runtime'da nasıl SpriteRenderer'a bağlanır (mevcut `DoorTrigger`/`GateBehavior` sprite alanları neler — `Assets/Scripts/Core/DoorTrigger.cs`, GateBehavior)? 3 yönlü door sprite'ı (S/SW/SE + SE=flipX) bu alanlara nasıl oturur?
3. **imagegen import pipeline:** Memory [[feedback-imagegen-asset-pack-clean-cell-split]] + [[feedback-imagegen-onbrand-not-realistic-s6]] geçerli. Üretilen PNG'lerin Unity import ayarları (PPU64, pivot bottom-center, point filter, alphaIsTransparency) ve "placeholder registry" akışı ne? Daha önce portal/reward nasıl import edildi (STAGING/imagegen/assets/PLACEMENT_MANIFEST.md)?
4. **ASSET-PACK vs TEK-TEK (import-risk lens):** Tek sheet→cell-split (chroma-key + grid kesim + her hücre transparent + ayrı sprite) vs her objeyi ayrı PNG üretmek — Unity import + boyut-doğruluğu + pivot tutarlılığı açısından hangisi daha AZ riskli/daha az manuel iş? Mevcut pixel_cleanup / cell-split tool'u (`Tools/pixel_cleanup/`?) var mı, asset-pack'i otomatik kesiyor mu?
5. RoomType (Combat/Elite/Boss/...) farklı oda tipleri farklı prop setleri ister mi — yoksa tek ortak Shattered-Keep prop havuzu mu yeterli (lean)?

Çıktı: her sub-soruya kısa, path/satır-destekli cevap + "obstacle = PropDefinition SO akışı mı / yeni prefab mı" net tavsiye + pack-vs-tekil import-risk verdict.
