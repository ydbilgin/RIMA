ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA'yı referans projedeki zengin tilemap görsel kalitesine yaklaştırmak — FEASIBILITY / REUSE / "RIMA'da ne zaten var" merceğinden değerlendir. ANALYSIS ONLY, kod DEĞİŞTİRME. Sonucu CODEX_DONE.md'ye yaz.

# Bağlam
Kullanıcı bir referans Unity 6 projesinde gördüğü çok güzel ZENGİN İZOMETRİK fantasy tilemap'leri RIMA'ya uyarlamak istiyor. Referans: Asset Store paketleri (Fantasy kingdom Tileset + TopDown 2D pixel Characters), Tile Palette + çok katmanlı Tilemap, Animated Tile ile animasyonlu su, sprite-sheet+Animator ile meşale, autotile pürüzsüz kenar geçişleri, büyük el-authored harita.

# RIMA KİLİTLERİ (saygı duy; ihlal öneriyorsan AÇIKÇA flag'le)
- room-based roguelite (açık-dünya DEĞİL). Demo-finalizasyon, deadline var.
- S59: HIGH TOP-DOWN 3/4 (~70-80°). "NO iso projection, NO 45° diamond." Referans TRUE ISO → çakışıyor.
- 2026-06-11: RIMA = PixelLab-only (asset-store tileset çakışır; ama PixelLab'ın create_topdown_tileset/create_tiles_pro/animate_object araçları VAR).
- 32×32 tile, PPU=64, URP 2D + Pixel Perfect + 2D Lights.

# KOD GROUND-TRUTH (orchestrator Explore+grep ile doğruladı; sen ÖNCE bunları DOĞRULA/derinleştir)
- Live demo zemini `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` BuildFloor = DÜZ tek `floorTile` (+ opsiyonel checker `floorTileAlt`), autotile KULLANMIYOR (satır ~310-314).
- AMA repo'da zengin Wang/layered boyama sistemi VAR ama demo'ya BAĞLI DEĞİL: `Assets/Scripts/RoomPainter/FloorWangResolver.cs`, `WangResolver.cs`, `Assets/Scripts/Systems/Map/WangTileResolver.cs`, `CornerWangPainter.cs`, `CornerWangTileSetSO.cs`, `LayeredRoomPainter.cs`, `LayeredRoomGenerator.cs`, `CliffYSortManager.cs`, `FeatureEdgeSmoothingPass.cs`, `MapTerrain.cs`, `TerrainDefinition.cs`.
- Prop/dekorasyon sistemi ~%80 VAR (BridsonPoissonAutoPlacer, PropFootprintValidator, CompositionRoleMap, PropRuntimeSpawner) ama live akışa tam bağlı değil. Plan: STAGING/PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md.
- Background per-room (SubRoomSequenceController.PaintBackgroundLayers, teardown'da yok); ParallaxLayer.cs (5-tier) VAR ama kullanılmıyor.

# SORULAR (feasibility/reuse merceği)
1. **Dormant Wang/layered painter ne kadar production-ready?** FloorWangResolver/WangTileResolver/CornerWangPainter/LayeredRoomPainter/CliffYSortManager dosyalarını OKU. Çalışır durumda mı, test var mı, IsoRoomBuilder'a bağlamak ne kadar iş (kaç dosya/satır, hangi entegrasyon noktası — BuildFloor mi BuildCliffs mi)? Yoksa yarım/ölü kod mu?
2. **Autotile'ı live floor'a bağlamak DOĞRU kaldıraç mı, yoksa düz-tile + iyi dekorasyon (props plan) yeter mi?** Görsel kazanç/risk/efor oranı.
3. **Animated su/meşale RIMA'da nasıl kurulur?** Unity Animated Tile (2D Tilemap Extras paketi projede var mı? Packages kontrol et) su için; meşale = animated sprite prefab (Animator). PixelLab animate_object ile kare üretimi. RIMA'da zaten animated prop/VFX var mı (brazier, SlashArc)?
4. **ISO'ya geçmek:** mevcut IsoRoomBuilder/camera/sorting altyapısı gerçekte ne kadar iso, ne kadar top-down? True-iso'ya geçiş kaç sistemi kırar (camera, Y-sort, cliff, collision, input)? Deadline'da feasible mi?
5. **Demo-safe öncelik sırası + kim yapar** (cx kod / kullanıcı-PixelLab / Claude wiring). Her adıma kaba efor (S/M/L).

ÇIKTI: file:line kanıtlı, net. Kilit-ihlali öneren her şeyi [LOCK-RİSK] ile işaretle. Önceki audit'i TEKRARLAMA. Sonucu CODEX_DONE.md'ye yaz.
