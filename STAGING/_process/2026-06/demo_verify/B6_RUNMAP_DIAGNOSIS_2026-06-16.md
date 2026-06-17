# B6 run-map "Graph=NULL" diagnozu (2026-06-16, read-only)

## SONUÇ: kod/wiring bug'ı BULUNAMADI → "temiz play'de doğrula" işi (live Unity + user)

### Kanıt (statik, _Arena.unity + kaynak)
- `RunMapOverlay` _Arena'da MEVCUT (`&...3979` block, toggleKey 109='m', show:0).
- `RunMapOverlay.director` = fileID **1169920644** → bu component = `RoomRunDirector` (`&1169920644`):
  - `m_Enabled: 1` (aktif)
  - `buildOnStart: 1` (Start → BeginRun → `graph = DungeonGraph.Generate(...)`)
  - `forceDemoSequence: 0` (branching açık — run-map fix korunmuş)
  - `depthCount: 6`
- `RoomRunDirector.cs:149` `public DungeonGraph Graph => graph;` · `BeginRun` graph üretir + `CurrentNodeId=graph.startId` (l.207).
- `RunMapOverlay.OnGUI` (l.43-47): `director.Graph` null/boşsa erken döner → M boş ekran. Director wire + buildOnStart olduğu için temiz play'de graph dolu olmalı.

### Yorum
Önceki "RunMapOverlay.director.Graph=null → M boş" gözlemi büyük olasılıkla **anormal stale-play session** artifact'i (aynı session'da DirectorMode.Instance de NULL'dı — CURRENT_STATUS B6 notu "o session ANORMAL'di" diyor). Temiz `_Arena` dev-direct play'de M, branching DAG'i göstermeli.

### KALAN (user / live Unity — unsupervised mutasyon YAPILMADI)
1. Temiz `_Arena` Play → açılış draft'ını gerçek kart seçimiyle kapat → **M bas** → branching map görünüyor mu? screenshot.
2. Görünmüyorsa: runtime'da `director.Graph` ve `director.CurrentNodeId` execute_code ile oku (NULL mı, hangi node?).
3. "iki-sistem (Core+MapDesigner) reconcile" endişesi: statikte tek aktif RoomRunDirector var (fileID 1169920644); ikinci buildOnStart:0 component (l.493) = IsoRoomBuilderTester (farklı sınıf, çakışmıyor). Reconcile gerekmiyor gibi — live doğrula.

> Portal-bar (alt bar + mavi-ışın T5) önkoşulu M/graph'tı; graph zaten üretiliyorsa portal-bar M'nin alt-şeridine direkt bağlanabilir.
