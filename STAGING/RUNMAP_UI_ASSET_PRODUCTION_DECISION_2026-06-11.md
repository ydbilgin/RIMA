# KARAR — RunMap + UI Asset Üretim Stratejisi (2026-06-11)

> Sentez merci: rima-design (Opus). Girdi: PixelLab yetenek analizi (orchestrator, DOĞRULANMIŞ) + Gemini endüstri araştırması (CONFIDENCE: HIGH) + ChatGPT önceki çıktı salvage durumu.
> Mevcut kod DOĞRULANDI: `MapNodeUI.cs`, `MapNodeData.cs`, `MapGraphData.cs`, `RoomType.cs`. Karar bu yapıya OTURUR, sıfırdan sistem önermez.

---

## 0. KRİTİK ÖN BULGU — iki enum çatışması (kararı şekillendiriyor)

Kod tabanında İKİ ayrı enum var ve AYNI değil:
- `RIMA.Core.RoomType` (kod/run): Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor
- `RIMA.UI.Map.MapNodeType` (UI/harita): Combat, Elite, Rest, Boss, Event, Shop, CurseGate, Mystery, Entry

Asset üretimi MapNodeType'a göre yapılmalı (harita UI bunu kullanıyor), ama brief RoomType'a göre asset istiyor (chest/forge/merchant). **Eşleme tablosu olmadan asset üretmek ikon-enum kaymasına yol açar.** Önerilen köprü: Chest→(yeni ikon), Forge→(yeni ikon), Merchant=Shop, Curse=CurseGate, Corridor=görünmez/çizgi. Bu köprü bir kez `RoomTypeToNode` dict'inde netleşmeli — asset adlandırması bu dict'e göre olmalı. **rima-doc/orchestrator notu: ikon dosya adları MapNodeType + Chest/Forge eki üzerinden.**

Ayrıca mevcut `MapNodeUI.GetColor` palet-canon İHLAL EDİYOR: default node saf kırmızı (0.82,0.18,0.14). Canon = void mor / ember / slate. Renk tint'leri yeni atlas + canon'a göre revize edilmeli (bu kod işi, asset işi değil — orchestrator/cx).

---

## 1. YETENEK MATRİSİ (asset × araç × gerekçe)

| Asset | Araç | Gerekçe |
|---|---|---|
| Node ikonları (combat/elite/boss/merchant/chest/forge/event) | **PixelLab MCP `create_map_object`** | Sabit-boyut ICON, scale edilmez, high top-down transparent. MCP'den otonom. Gemini: icon=AI OK. |
| Node çerçevesi (ornate border, sabit boyut) | **el-Aseprite** | CHROME ama 9-slice DEĞİL (sabit boyut). Yine de pixel-art chrome AI'da zayıf; Hades "frame+content" modülaritesi için tek temiz frame el-çizimi daha ucuz/temiz. ALT: tek frame, scale yok → AI denenebilir ama el-çizim risksiz. |
| Node state — current-glow (cyan) | **Unity-procedural** (ring Image + tint/pulse) | Zaten `MapNodeUI.ringImage` var. Sprite üretme; cyan glow shader/tint + scale-pulse. Cyan ≤%15 kuralı: sadece AKTİF node'da. |
| Node state — hidden (silüet) | **Unity-procedural** (aynı ikon, koyu tint + alfa) | Zaten kodda var (`isRevealed` → 0.12 gri). Ayrı asset GEREKSİZ. |
| Bağlantı çizgileri | **Unity-procedural — UI Line / LineRenderer + dashed material** | Gemini HIGH: sprite DEĞİL. `MapNodeData.connections` zaten var, çizim katmanı yok → eklenecek. |
| Rarity ribbon (common/rare/epic, baked text) | **ChatGPT çıktısı SALVAGE** (PASS) | Zaten temiz üretilmiş. Yeniden üretme. CHROME ama sabit-boyut baked → kabul. |
| Minimap marker — room tile, door | **ChatGPT çıktısı SALVAGE** (PASS) | Temiz. Kullan. |
| Minimap marker — player (yönlü ok) | **PixelLab MCP `create_map_object`** (tek ok ikonu) | ChatGPT "A harfi" REJECT. Yönlü ok = küçük sabit ikon → MCP ideal. ALT: el-Aseprite (8x8 trivial, gen harcama). **Tercih: el-Aseprite** — 1 ufak ok için gen harcamak israf. |
| Minimap frame (9-slice) | **el-Aseprite** | KESİN. 9-slice chrome, flat-edge kuralı, köşe ornament köşe-sınırı içinde. AI burada BAŞARISIZ (Gemini HIGH + ChatGPT frame REJECT bunu kanıtladı). |

---

## 2. "Her şey PixelLab ile olur mu?" — **HAYIR.**

Net cevap: **Sadece sabit-boyut ICON'lar PixelLab MCP ile otonom üretilir.** Üç sınıf PixelLab MCP DIŞINDA kalır:
1. **9-slice chrome (minimap frame)** → el-Aseprite zorunlu. MCP'de UI tool YOK; web `create_ui_pro` var ama otomatize edilemez (manuel tarayıcı) + Gemini AI-chrome'da başarısız diyor. İkisi de aynı yöne işaret ediyor: el-çizim.
2. **Procedural çizgiler/glow** → Unity (LineRenderer + dashed, ring tint-pulse). Bunlar ASSET değil, runtime render.
3. **Baked ribbon/tile** → zaten ChatGPT'de PASS, üretme.

`create_ui_pro` (web, 20 gen, manuel) kararı: **KULLANMA.** Gerekçe: (a) otomatize değil, manuel tarayıcı emeği; (b) tek temiz frame için el-Aseprite zaten daha hızlı ve 9-slice flat-edge garantisi veriyor; (c) memory kuralı "kullanıcı çizer, Claude mount eder" ruhuna el-Aseprite daha uygun.

---

## 3. MODÜLER UNITY MİMARİSİ (mevcut yapıya oturur)

Mevcut: `MapNodeUI` tek prefab + `MapNodeData` + tint mantığı ZATEN var. Eklenecekler:

- **Tek MapNode prefab** (mevcut `MapNodeUI`): `nodeImage` (Image) + `ringImage` (cyan glow, var) + opsiyonel `frameImage` (ornate border, el-Aseprite, sabit). Text label → sprite-swap'a geçince label OPSİYONEL/debug.
- **`NodeIconLibrary` ScriptableObject**: `Dictionary<MapNodeType, Sprite>` + Chest/Forge için ek anahtar (enum köprüsü §0). `MapNodeUI.Refresh()` içinde `nodeImage.sprite = library.Get(data.nodeType)` — mevcut renk-tint yerine sprite swap. Tint sadece state için (visited=koyu, hidden=silüet) korunur.
- **`NodeTintTable`**: RoomType/MapNodeType → canon tint (void mor / ember / slate). Mevcut saf-kırmızı default'u EZER. Cyan yalnız current-glow.
- **SpriteAtlas (ZORUNLU)**: tüm node ikonları + frame + ribbon + marker tek atlasta → tek draw call. Gemini HIGH.
- **Bağlantı çizimi**: yeni `MapEdgeRenderer` — `MapGraphData` üzerinden `connections` okur, node anchoredPosition'lar arası UI Line (veya LineRenderer overlay) + dashed material çizer. Procedural, sprite yok.
- **Integer-scale**: ikonlar native PPU 64 katında üret, nearest-neighbor downscale. 265×385→120 gibi non-integer YASAK (Point filter zaten katı).

DungeonGraph oturması: graph algoritması node'ları `position`'a spawn ediyor (STS mimarisi ile birebir). Tek prefab + dict swap + atlas + edge renderer = mevcut `MapGraphData`/`MapNodeUI` akışına ek katman; refactor değil.

---

## 4. ÜRETİM SIRASI + GENERATION BÜTÇE TAHMİNİ (bütçe: 967 gen)

`create_map_object` = standart gen (Pro tool 20 değil; map_object tekil). Konservatif 1 obje ≈ 1-2 gen + olası retry.

1. **Node ikonları ×7** (combat/elite/boss/merchant/chest/forge/event): 7 obje × ~2 (retry payı) = **~14 gen**.
2. **Player yön-oku**: el-Aseprite → **0 gen** (MCP'ye düşerse +2).
3. Frame / minimap frame: el-Aseprite → **0 gen**.
4. Ribbon / room tile / door: ChatGPT salvage → **0 gen**.
5. Çizgi/glow/silüet: Unity procedural → **0 gen**.

**Toplam tahmin: ~14-18 gen** (bütçenin %2'sinden az). Retry/varyant için tampon bol. Karar: ikonları TEK BATCH üret, palet canon'u prompt'a göm (void mor zemin, ember vurgu, transparent), 7'sini gör, sonra Unity atlas + dict bağla.

---

## 5. ChatGPT SALVAGE vs YENİDEN ÜRETİM

| Çıktı | Karar |
|---|---|
| Rarity ribbon ×3 (common/rare/epic) | **SALVAGE** — kullan, üretme |
| Room tile + door marker | **SALVAGE** — kullan |
| Player marker "A" harfi | **REJECT → yeniden** (el-Aseprite yönlü ok) |
| Node'lar (baked text + non-integer + saf kırmızı) | **REJECT → yeniden** (PixelLab MCP, baked-text YOK, integer-scale, canon palet) |
| Minimap frame (aksan köşe ortasında) | **REJECT → yeniden** (el-Aseprite, 9-slice flat-edge) |

Node'larda baked text REJECT gerekçesi mimariyle örtüşüyor: sprite-swap dict modeli zaten ikon-üzeri-text istemiyor; "C/EL/B" gibi label ayrı procedural Text katmanı (mevcut `MapNodeUI.label`), ikona BAKED EDİLMEZ.

---

## 6. ÇAKIŞMA / RİSK + LOCKED KURAL İHLALİ

**LOCKED KURAL İHLALİ: NONE.** Tüm yönlendirmeler memory hard-rule'ları ile uyumlu:
- "asset üretimi cx değil" → ikonlar orchestrator-PixelLab MCP, frame el-Aseprite. cx'e asset verilmedi. UYUMLU.
- "kullanıcı silah çizer, Claude mount eder" → bu UI chrome, silah değil; yine de frame el-çizim ruha uygun, çatışma yok.
- "sormadan local model çalıştırma" → hiçbir local model yok. UYUMLU.
- Palet canon (void mor #3A1A4A / ember #E89020 / cyan ≤%15) → ikon prompt'ları ve tint table canon'a kilitlendi; cyan yalnız current-glow.

**RİSKLER:**
1. **Enum köprü kayması (§0)** — en yüksek risk. Asset adlandırma RoomType↔MapNodeType eşlemesi netleşmeden başlarsa ikon yanlış slot'a düşer. AZALTMA: önce `RoomTypeToNode` dict netleşsin, sonra üret.
2. **Mevcut saf-kırmızı/yeşil renkler canon ihlali** — `MapNodeUI.GetColor` ve `GetLabel` revize edilmeli (kod işi). Asset üretiminden ayrı, ama aynı PR'da olmazsa görsel tutarsızlık.
3. **create_ui_pro cazibesi** — 20 gen'lik web tool'a sapma israf; karar net HAYIR.

---

## ORCHESTRATOR NEXT STEP
1. **rima-doc**: §0 enum köprüsünü (`RoomTypeToNode`) ve §3 mimariyi TASARIM/ altına net spec yaz.
2. **Orchestrator (PixelLab MCP)**: §4 batch — 7 node ikonu, canon palet prompt, transparent, integer-scale. Önce dict netleştikten sonra.
3. **el-Aseprite (kullanıcı)**: minimap frame (9-slice flat-edge) + player yön-oku.
4. **cx/orchestrator (kod)**: `NodeIconLibrary` SO + sprite-swap + `MapEdgeRenderer` + canon tint table + SpriteAtlas. Asset DEĞİL, kod.
5. ChatGPT ribbon/tile/door → Unity import + atlas.
