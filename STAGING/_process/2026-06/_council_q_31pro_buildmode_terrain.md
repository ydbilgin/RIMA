# Council — ax Gemini 3.1 Pro (High) — DEEP / ARCHITECTURE & DESIGN lens

LENS (sen = ax 3.1 Pro): derin mimari + tasarim yargisi. URP 2D + Pixel Perfect ile world-space pixel-art terrain shader'inin teknik fizibilitesi, data-oriented terrain takaslar, RIMA estetik/oyun-tasarim uyumu. Derinlemesine ama net.

## RIMA BAGLAM
Unity 2D top-down 3/4 ARPG. Grid = ISOMETRIC cellLayout (fake-iso / 3-4 staggered), 64px KARE top-down sprite tile (floor451), URP 2D Renderer + Pixel Perfect Camera (PPU 64), duz ortografik kamera. Oda-bazli (RTS DEGIL; her oda kucuk-orta, runtime'da RoomTemplateSO'dan IsoRoomBuilder ile insa ediliyor; prop/Poisson scatter + walkability/solvability + composition-role sistemleri VAR). Build Mode insa ediliyor: P1 BITTI (quote -> kamera zoom-out + pause + Director Build sekmesi). P2 devam (PropRegistry palette + iso-grid yesil/kirmizi validity ghost + LMB/RMB + rotate/flip + undo, hepsi Grid API'den; ASLA dikdortgen matematik = SECTION 3.5). Planli: P3 tile/walkability brush, P4 light+auto-scatter+runtime save/load, P5 selection/move. Sunum ~1 hafta, in-editor.

## REFERANS TEKNIKLERI (videolardan)
- REF1 (Amine Rehioui): Runtime RTS map editor. Organik FIRCA-tabanli terrain (serbest cizim, tile-grid degil), WORLD-SPACE dokulu zemin (organik harmanlanma), palet, sari footprint=validity. Perf: birim+bina disindaki her seyden GameObject overhead kaldirilmis (terrain texture/data).
- REF2 (Orb): Tileset yerine WORLD-SPACE TEXTURE terrain; yumusak dairesel firca -> grass/dirt/water organik harman, pixel-art SHADER ile korunuyor (quantize/dither). Kullanmasi eglenceli.

## YANITLA (4)
1) World-space-texture / firca-tabanli organik terrain RIMA'nin iso-tile + Pixel Perfect mimarisiyle CAKISIR MI yoksa eklenebilir mi? URP 2D (2D Renderer, SpriteRenderer/Tilemap pipeline) ile splat/mask-tabanli pixel-art terrain blend nasil yapilir, PPU-64 pixel-perfect netligi korunur mu (texel snapping/quantize shader)? Iso staggered grid bunu zorlastirir mi?
2) Az-GameObject / data-oriented terrain RIMA icin anlamli mi? RIMA oda-bazli (RTS olcegi yok). Tilemap chunk rendering zaten verimli — REF1'in perf motivasyonu RIMA'da gecerli mi yoksa cozulmus-problem mi?
3) SOMUT ONERI: hangi teknik SIMDI (P2-P4'e haritala), POST-DEMO, yoksa ATLA? Estetik kazanc (organik zemin) RIMA'nin Act1 canon'una (slate/void/ember) deger katar mi, yoksa mevcut tile-art yeterli mi? Modulerlik/overcomplicate lensi.
4) RISK: ~1 hafta demo; calisan P1-P2 bozulur mu? Firca-terrain AYRI opsiyonel tool mu olmali?

Cikti: kisa net durus + AL/POST-DEMO/ATLA tablosu + neden.
