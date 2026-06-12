# Council Question — RIMA Tilemap Visual Quality (DEEP / architecture / design lens)

Sen ax Gemini 3.1 Pro High'sın. RIMA için DERİN MİMARİ/TASARIM görüşü ver. Kod yazma; mimari + sanat-yönü + risk analizi. Kısa ve net, madde madde.

## KONU
Kullanıcı bir referans Unity 6 projesinde (Senior Design) gördüğü çok güzel ZENGİN İZOMETRİK fantasy tilemap'leri RIMA'ya uyarlamak istiyor. Referans özellikleri: Asset Store paketleri (Fantasy kingdom Tileset + TopDown 2D pixel Characters pack 1), Unity Tile Palette + çok katmanlı Tilemap, Animated Tile ile animasyonlu su, sprite-sheet+Animator ile animasyonlu meşale, autotile (RuleTile/Wang) ile pürüzsüz kenar geçişleri, krallık/kasaba ölçeğinde el-authored büyük harita.

## RIMA KİLİTLERİ (saygı duy; ihlal öneriyorsan AÇIKÇA "[LOCK-RİSK]" ile işaretle)
- RIMA = room-based 2D ARPG roguelite (açık-dünya DEĞİL). Demo-finalizasyon fazı, deadline var.
- Sanat kilidi S59: HIGH TOP-DOWN 3/4 (~70-80°), Hades / Children of Morta / Diablo III ref. "NO iso projection math, NO true 45° diamond." Referans proje TRUE ISO → çakışıyor.
- Asset kilidi (2026-06-11): RIMA = PixelLab-only. Asset-store tileset PixelLab-only ile çakışır. AMA PixelLab'ın create_topdown_tileset / create_tiles_pro / animate_object araçları VAR.
- 32×32 tile, PPU=64, URP 2D Renderer + Pixel Perfect Camera + 2D Lights.

## KOD GROUND-TRUTH (doğrulandı, güvenebilirsin)
- Live demo zemini IsoRoomBuilder.BuildFloor = DÜZ tek floorTile (+ opsiyonel checker), autotile KULLANMIYOR.
- AMA repo'da zengin Wang/layered painter sistemi VAR ama demo'ya BAĞLI DEĞİL: FloorWangResolver, WangResolver, WangTileResolver, CornerWangPainter, CornerWangTileSetSO, LayeredRoomPainter, LayeredRoomGenerator, CliffYSortManager, FeatureEdgeSmoothingPass, MapTerrain, TerrainDefinition. (Ne kadar bitmiş bilinmiyor.)
- Prop/dekorasyon sistemi ~%80 VAR (BridsonPoissonAutoPlacer seeded AI-placement, PropFootprintValidator, CompositionRoleMap zone+DoorSafety radius=3, PropRuntimeSpawner) ama live akışa tam bağlı değil. Plan zaten yazıldı.
- Background per-room (teardown'da yok); ParallaxLayer.cs (5-tier) VAR ama kullanılmıyor → env-level parallax planlandı.

## SORULAR (her birine kısa, net cevap ver)
1. RIMA bu görsel kaliteye **top-down-3/4 içinde kalarak** ne kadar yaklaşabilir? Gerçekçi mi, yoksa "o look" intrinsik olarak iso'ya mı bağlı? Hangi unsurlar (autotile, katman, ışık, animated detay, palet-uyumu, dekor-yoğunluğu) gerçek farkı yaratıyor — sırala.
2. Dormant Wang/layered painter'ı live'a bağlamak **mimari olarak doğru kaldıraç mı**, yoksa düz-tile + güçlü dekorasyon (props plan) görsel olarak yeter mi? Hangi durumda hangisi?
3. Animated su/meşale on-brand nasıl (PixelLab animate_object + Unity Animated Tile/Animator)? Mimari risk var mı (perf, sorting, Pixel Perfect ile titreme)?
4. ISO'ya geçmek deadline'da **delilik mi**, post-demo değer mi? Eğer post-demo, hangi koşulda mantıklı (yeni act / gelecek proje)? RIMA'nın kimliği için iso vs top-down-3/4 hangisi daha doğru?
5. Demo-safe öncelik sırası öner (hangi adım önce, neden) + her adımın görsel-etki/risk dengesi.

Kapanışta: "RIMA'yı bozMADAN en yüksek görsel kazanç" için TEK bir net tavsiye cümlesi.
