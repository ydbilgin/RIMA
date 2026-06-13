ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amac
RIMA oyun-ici Build Mode'a iki referans teknigini (world-space-texture / firca-tabanli organik terrain + az-GameObject terrain temsili) FIZIBILITE + REUSE lensiyle degerlendir. ANALIZ ONLY — kod degisikligi YOK, Unity'ye DOKUNMA. Onceki audit'leri tekrar etme.

LENS (sen = cx): feasibility / RIMA'da hali hazirda NE VAR / reuse-vs-build. Gercek dosyalara bakarak somutla. Oku (ornek): Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs, RoomTemplateSO + overlayMask/walkableGrid alanlari, Assets/ altindaki *.shader / Material'lar (var mi pixel-art/terrain shader?), Pixel Perfect Camera + URP 2D Renderer ayarlari, mevcut Tilemap kullanimi. Grep ile RIMA'nin shader/texture-blend kapasitesini tespit et.

## RIMA BAGLAM
Unity 2D top-down 3/4 ARPG. Grid = ISOMETRIC cellLayout (fake-iso / 3-4 staggered), 64px KARE top-down sprite tile (floor451), URP 2D Renderer + Pixel Perfect Camera (PPU 64), duz ortografik kamera. Oda-bazli (RTS degil; her oda ~kucuk-orta, IsoRoomBuilder runtime'da RoomTemplateSO'dan insa ediyor). Build Mode insa ediliyor: P1 BITTI (quote tusu -> kamera zoom-out + pause + Director Build sekmesi). P2 devam (PropRegistry palette + iso-grid yesil/kirmizi validity ghost + LMB yerlestir/RMB sil + rotate/flip + undo, HEPSI Grid API'den; ASLA dikdortgen matematik = SECTION 3.5 tile kanunu). Planli fazlar: P3 tile/walkability brush, P4 light+auto-scatter+runtime save/load, P5 selection/move. Sunum ~1 hafta sonra, in-editor.

## REFERANS TEKNIKLERI (videolardan, gorsel analiz)
- REF1 (Amine Rehioui): Runtime RTS map editor. Organik FIRCA-tabanli terrain boyama (kanyon/yol serbest ciziliyor, tile-grid degil), WORLD-SPACE dokulu zemin (organik harmanlanma, sert tile kenari yok), sag palet (firca/obje/birim), sari footprint highlight = validity. Perf: "birim+bina disindaki her seyden GameObject overhead'ini kaldirdim, cok daha hizli" -> terrain texture/data, az GameObject.
- REF2 (Orb): "Pixel art oyunlar terrain icin tileset yerine WORLD-SPACE TEXTURE yaklasimindan faydalanir." Yumusak dairesel firca -> grass/dirt/water organik harmanlaniyor, pixel-art gorunum SHADER ile korunuyor (quantize/dither). "Kullanmasi eglenceli."

## YANITLA (4)
1) World-space-texture / firca-tabanli organik terrain RIMA'nin iso-tile + Pixel Perfect mimarisiyle CAKISIR MI yoksa uzerine eklenebilir mi? URP 2D shader ile pixel-art terrain blend feasible mi, PPU-64 pixel-perfect korunur mu? RIMA'da bunu destekleyecek altyapi (shader/material/rendertexture) VAR MI yoksa sifirdan mi?
2) Az-GameObject / data-oriented terrain RIMA icin gerekli mi yoksa mevcut Tilemap yeterli mi? RIMA oda-bazli ARPG (RTS degil) — olcek farki bu teknigi gereksiz kiliyor mu?
3) SOMUT ONERI: hangi teknik SIMDI (P2-P4'e haritala), hangisi POST-DEMO, hangisi ATLA? Karpathy "modulerligi hak ediyor mu / overcomplicate mi" lensiyle. RIMA'da reuse edilebilecek mevcut parca var mi?
4) RISK: ~1 hafta demo; bu degisiklikler calisan P1-P2'yi bozar mi? Firca-terrain AYRI opsiyonel bir tool olarak mi eklenmeli?

Cikti: kisa net durus + AL/POST-DEMO/ATLA tablosu + neden. CODEX_DONE.md'ye yaz.
