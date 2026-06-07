# COUNCIL BRIEF — Modular Map Learnings: Prop-Groups + Path-Mask (+Terraces?) (2026-06-05)

## Amaç
Kullanıcı bambaşka bir oyundan screenshot verdi (cozy iso bahçe oyunu) ve "modüler koymuş değil mi, otomatik map'i mantıklı yapabilmiş — alabileceklerini council'e sor, al" dedi. Karar: RIMA'nın veri-bazlı oda sistemine (IsoRoomBuilder + RoomTemplateSO) hangi teknikler, hangi öncelikle alınır?

## Görsel
`STAGING/mockups/ref_modular_garden_game_2026-06-05.png` (görebilen baksın; tarif aşağıda).
Tarif: bulutların üstünde yüzen iso elmas-grid ada; 2-3 yükseklik terası (istif blok cliff'ler);
çim karoları İKİ TONLU damalı; kahverengi patika bantları (tile-tipi maskesi, geometri değil);
1-karo genişlik nehir kanalları (kıyı autotile'lı); büyük kaya kümeleri (kaya yığını + devrik kütük =
TEK GRUP, 2-3 varyant tekrar+mirror ile serpiştirilmiş); ağaç/çalı/çiçek scatter; çadır+duman.
HUD: su 4/12, yaprak 1875, dal 1294, çiçek 48.

## Opus ön-analizi (orchestrator, görseli gördü) — advisorlar buna KATILMAK ZORUNDA DEĞİL
1. Pipeline tahmini: heightmap→teras quantize → cliff autotile → nehir oyma → patika maskesi → prop-GRUP yerleştirme (footprint) → scatter.
2. RIMA'ya adaylar: (a) **prop-grup prefab'ları** (kaya+kütük kümesi tek unit; düşük maliyet/yüksek değer), (b) **path/decal tile-maske katmanı** (RoomTemplateSO'ya ikincil maske), (c) damalı ton (floor451 16-varyantla zaten var), (d) **çok-teras** (orta-ağır; v2 adayı).

## RIMA mevcut sistem (bağlam — advisorlar dosyaları okuyabilir)
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`: RoomTemplateSO walkableGrid→iso floor + yönlü cliff (RoomCliffSolver) + boundary + props + kapılar. Tek düzlem (teras yok).
- `Assets/Data/Props/`: PropDefinitionSO ×7 + PropRegistry; `PropColliderAutoBuilder` (Box-only, colliderShape follow-up bekliyor). Props TEK obje (grup kavramı YOK).
- `Assets/Data/Rooms/Generated/`: 15 ChatGPT odası import'lu (RoomJsonImporter). Odalar şu an PROP-FAKİRİ ("sonra süsleriz" denmişti).
- B-12 production RoomBank işi SIRADA (8 büyük keeper + küçük/orta Library karışımı + pacing weights).
- Karar felsefesi (ROOM_DESIGN_DECISION_2026-06-04): çeşitlilik geometriden değil "kapı+wave+mirror"dan.
- Tile drift açık konusu: 32px-rule vs iso-cell (PIXELART_SCALING_REPORT).

## SORULAR (her advisor cevaplasın)
1. **Prop-grup prefab'ları:** RIMA'ya alınmalı mı? En lean veri modeli ne? (örn. PropGroupSO = üye listesi + relative offset + footprint birleşimi; vs RoomTemplateSO içine baked çoklu-prop; vs prefab-variant). Mirror/varyant desteği nasıl? IsoRoomBuilder + PropRegistry'ye dokunuş maliyeti?
2. **Path/decal maske katmanı:** RoomTemplateSO'ya ikincil tile-tipi maskesi (patika/çatlak/halı) değer mi? Render: ayrı tilemap layer mı, decal sprite'lar mı? Iso-cell drift riskine etkisi?
3. **Çok-teras (heightGrid):** ŞİMDİ alınmalı mı, v2'ye mi? (depth-sort Custom-Axis kilidi + collision katmanları + cliff solver etkisi). Net öneri: şimdi/sonra/asla + gerekçe.
4. **Sıralama/paketleme:** Bunlar B-12 production-RoomBank işine mi gömülür, ayrı task mı? Önerilen iş sırası + tahmini kapsam (S/M/L).
5. Görseldeki başka çalınmaya değer numara var mı (advisor görebiliyorsa)?

## Çıktı formatı
Madde madde, kısa; her soru için NET pozisyon + gerekçe. Disagreement açıkça yaz.
