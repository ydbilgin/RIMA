ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece render script + 3 çıktı (4) BLOCKED if unclear.
NLM gerekmez. Unity gerekmez (saf Python + filesystem). Çıktıyı CODEX_DONE.md'ye yaz, dönüş ≤10 satır + yollar.

# Amaç
RIMA graphify full-map'ini hocaya sunum/rapor için görselleştir. Tez: "RIMA bir oyun değil, environment + tooling" — KANIT: 6925 node'un en bağlı 10'unun **6'sı editor aracı**. 3 çıktı üret: (a) tam graf PNG, (b) odaklı god-node PNG, (c) canlı interaktif HTML.

# Girdi
`STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`
- Format: NetworkX node-link. `nodes`: her biri `{label, id, community, source_file, file_type, ...}`. `links`: her biri `{_src, _tgt, weight, relation, ...}` (DİKKAT: standart `source/target` değil, **`_src`/`_tgt`**). 6925 node, 14321 edge, 118 community.
- Graf kurarken: `nx.DiGraph` (directed) ya da `nx.Graph`; node id = `id`, edge = (`_src`,`_tgt`), label = node `label`.

# God-node listesi (GRAPH_REPORT.md'den, degree ile)
Top-10 (label → degree), match: graph node `label` ile birebir; bulunamazsa `norm_label`/contains dene.
- **EDITOR (6, VURGULA):** DirectorMode(168), InPlayMapPaintOverlay(93), RoomPainterWindow(88), LargeDungeonMapPainterBase(78), MinimalTilePainter(70), BuildPlacementController(66)
- **OYUN/RUNTIME (4):** ChamberSelectBootstrap(84), CharacterSelectScreen(75), RuntimeRoomManager(69), RoomRunDirector(65)

# Çıktılar (klasör: `STAGING/report/graphify/`, yoksa oluştur)

## (a) `full_graph.png` — ölçek görseli
- TÜM 6925 node + edge'ler. Hızlı layout kullan (büyük graf!): graphviz `sfdp` (pygraphviz/pydot varsa) TERCİH; yoksa `nx.forceatlas2_layout` (networkx 3.x) ya da düşük-iter `spring_layout`. Layout YAVAŞ olmasın (sfdp ideal).
- Normal node'lar: küçük, soluk gri (alpha düşük). Edge'ler ince, çok soluk.
- 10 god-node BÜYÜK: 6 editor = ember/turuncu (#E89020), 4 oyun = gri-mavi (#5A7AA0). Top-10'a etiket (label) koy.
- Yüksek çöz: en az 3500px genişlik veya dpi=300. Başlık: "RIMA codebase — 6925 nodes, 14321 edges · 6 of 10 most-connected = editor/tooling".
- Legend: "editor/tooling (6)" vs "game/runtime (4)".

## (b) `god_nodes_focus.png` — KANIT figürü (rapor için en değerli)
- Top-10 god-node + DOĞRUDAN komşuları (1-hop ego) ya da sadece 10 god-node arası + en güçlü komşular (okunur kalsın, ~80-150 node max).
- Node boyutu ∝ degree. 6 editor = ember (#E89020), 4 oyun = gri-mavi, komşular = soluk.
- TÜM 10 god-node etiketli (okunur font). Temiz layout (spring/sfdp).
- Legend + başlık: "10 most-connected nodes — 6 are editor tools (orange), 4 are game/runtime (blue)".
- Yüksek çöz (dpi=300).

## (c) `graph_interactive.html` — canlı sunum için
- pyvis (vis-network) ile interaktif, zoom/pan/hover-label. Physics açık ama PERFORMANSLI olsun (sunumda takılmamalı).
- 6925 node pyvis'te TAKILIRSA: god-node'ların **2-hop ego network**'ünü kullan (birkaç yüz node, akıcı). Tam grafı denersin, lag varsa ego'ya düş — kararı sen ver, akıcılık öncelik.
- 6 editor god-node = ember büyük, 4 oyun = mavi, gerisi soluk küçük. Hover'da label + degree.
- Tek dosya (CDN ya da inline JS), çift-tıkla açılır olsun.

# Kütüphaneler
networkx, matplotlib, pyvis gerekli. Yoksa `pip install`. graphviz/pygraphviz opsiyonel (sfdp için); yoksa forceatlas2/spring fallback. Script'i `STAGING/report/graphify/render_graphify_figures.py` olarak kaydet (tekrar çalıştırılabilir).

# Doğrulama
3 çıktı da var mı + boyut makul mu (PNG'ler >100KB, HTML açılıyor mu) kontrol et. full_graph layout süresini raporla (çok uzunsa not düş).

# Dönüş (≤10 satır)
3 dosya yolu + her birinin boyutu/node-sayısı + hangi layout kullanıldı + god-node match sayısı (10/10 bulundu mu). Rapor içeriğini şişirme.
