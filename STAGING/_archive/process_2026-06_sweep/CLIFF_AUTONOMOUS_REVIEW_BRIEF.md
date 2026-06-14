ACTIVE RULES: (1) think before answering (2) concrete/min-fluff (3) scope below (4) flag uncertain.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read: STAGING/CLIFF_DEPTH_SYNTHESIS_S114S5.md, Assets/Scripts/Environment/CliffAutoPlacer.cs, CURRENT_STATUS.md. RESPOND INLINE.

# Amaç
Opus otonom modda cliff+depth backdrop'u ilerletiyor. Reviewer olarak 2 somut spec + 1 review istiyorum.

# Bağlam
STAGING/CLIFF_DEPTH_SYNTHESIS_S114S5.md = bu session yapılanlar. Cliff şu an: floor altında render, tek varyant cliff_S, perimeter-only, "güneyinde N=5 hücre floor varsa kes" (south-only; diagonal probe diamond'da 57/59 kesti, geri alındı). 55 cliff. AO contact-shadow var. Depth backdrop (RoomBackgroundRig) gerçek boyuta açıldı, parallax+unlit OK.

# DELIVERABLE 1 — Robust cut rule (impl-ready)
South-probe N=5 çalışıyor ama map-shape'e kırılgan. Daha sağlam kural: exterior-void flood-fill (map bounds'tan) + open-drop. CliffAutoPlacer.CollectCliffCells için PSEUDOCODE ver:
- Diamond ada üzerinde OVER-CUT yapmayacak (kritik — diagonal probe bunu yaptı).
- Sadece kameraya bakan dış/açık-düşüş kenarlarına cliff; concave notch / arka peninsula atla.
- Mevcut floodfill var mı (ComputeOrphanClusters benzeri) reuse et.

# DELIVERABLE 2 — Per-map background system spec
Kullanıcı: "arka plan map durumuna göre olur." RoomBackgroundRig'i RoomType/RoomSequenceData'ya göre preset seçecek sistem. Hangi veri yapısı, nasıl bağlanır (RoomLoader.OnRoomLoaded?), kaç preset. Min-code.

# DELIVERABLE 3 — Review
CLIFF_DEPTH_SYNTHESIS_S114S5.md'de hata/risk/eksik var mı? Özellikle: AO yaklaşımı, parallax Y=½X, seamless/tileable + ParallaxLayer no-wrap çözümü doğru mu?

Kısa, madde-madde, impl-ready.
