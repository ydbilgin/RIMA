# CLIFF FLOATING FEEL — FINAL DECISION (S112 gece sentez)

**Date:** 2026-05-27 gece
**Orchestrator:** Opus subagent (triple-AI inside)
**User direktifi (verbatim):** "3 agent bu cliff'lerin sanki gerçekten havada duruyormuşuz hissiyle nasıl duracağını da iyice kararlaştırsın bazı cliffler çok ortada kaldığından oyun ekranında görünmeyecek."
**Status:** READY FOR USER REVIEW

İki bağlı problem:
1. Cliff'ler oyunda gerçekten yüksekte/havada gibi DURMALI (Hades Elysium V1 floating island brand)
2. "Ortada kalan" cliff'ler oyun ekranında görünmeyecek (izole/orphan cliff cluster'ları)

---

## BÖLÜM 1 — Problem Tanımı + Saha Verisi

### Screenshot context (kullanıcı 2026-05-27 gece raporu)
`C:\Users\ydbil\.claude\image-cache\51e43494-ad7c-423d-acff-41d7b964594b\1.png` — kırmızı çerçeve içinde "ortada havada duran" cliff sprite'lar işaretli. Görsel olarak floor cluster'ın yanında, tek başına, void içine doğru sarkıyor — altında devamı yok, "asılı raf" hissi veriyor.

### Sahne reality check — PlayableArena_Test01.unity (orchestrator scan, 2026-05-27 23:13)

| Metric | Değer |
|---|---|
| Floor tile total | 2365 |
| Cliff tile total | 283 |
| Decor cliff total | 0 (S110 DecorCliffTilemap boş) |
| Floor cluster sayısı (iso 4-neighbor BFS) | **18** |
| Floor en büyük 5 cluster | **1166, 1114, 37, 8, 7** |
| Floor 1-cell ada | 6 |
| Floor 2-cell ada | 2 |
| Floor 4-9-cell ada | 7 |
| **Cliff cluster sayısı** | **166** (!) |
| Cliff en büyük 5 cluster | 12, 9, 8, 8, 7 |
| **Cliff 1-cell parça** | **106** (!!) |
| Cliff 2-cell parça | 38 |
| Cliff 3-cell parça | 11 |
| Cliff 4+ parça | 11 |
| Cliffs sitting on floor cluster size <4 | 5 (sadece) |
| Cliffs on size 20+ floor (ana arena) | 267 |

### KRİTİK BULGU — beklenenden tersi

İlk hipotez: "izole floor cell'ler (1-3 cell) cliff alıyor, havada gözüküyor". GERÇEK: **floor 1-3 cell adaları sadece 8 tane, üzerindeki cliff sadece 5 tane**. Floating-feel sorununun ana kaynağı bu DEĞİL.

Gerçek kaynak: **283 cliff tile 166 ayrı cluster'a bölünmüş** — 155 cluster'ı 1-3 cell. Yani cliff "ring" devamlı değil; ana arena floor cluster perimetresi boyunca 1-3 cell parçalara fragmente. Her 1-cell cliff sprite top-pivot ile aşağı sarkıyor, ortasında veya yanında devamı olmadığı için **"izole asılı raf"** hissi veriyor.

Neden bu kadar fragmente: CliffAutoPlacer algoritması "floor cell'in S/SE/SW komşusundan biri void → cliff koy" diyor. Floor perimetri pürüzlü (S102+ random walk floor) → cliff candidate dağılımı şah-bezir gibi serpildi. Cluster-aware filtre yok.

### Cliff tilemap pozisyonu — kasıtlı "sarkma" var
`CliffTilemap` GO `m_LocalPosition.y = -0.3046875` (= cell height 0.609375 / 2 = yarım cell DOWN). Cliff sprite pivot `(0.5, 1.0)` top-center, transformOffset.y=0 → sprite tilemap origin'den TOP-CENTER asılı, tilemap zaten yarım cell aşağıya kaymış → görsel olarak sprite floor'un altından başlıyor ve daha da aşağı sarkıyor. Bu MİMARİ doğru ("havada" hissi için kasıtlı), ama izole cliff'lerle birleşince UX bozuk.

### Mevcut infrastructure (codebase scan)
- `CliffAutoPlacer.cs` algorithm L212-244: cluster-aware DEĞİL, basit S/SE/SW komşu kuralı
- `DirectionalCliffTile.cs` L23-99: 8-yön sprite branch, transformOffset=(0,0,0) — sarkma TİLEMAP-LEVEL (yukarıda görüldü)
- `DirectionalCliffTile_Hades.asset`: 5 spritesS varyant + 1 sprite/yön diğer 7 yönde
- `GroundBlobShadow.cs` L1-99: procedural runtime gradient sprite (player altı blob shadow) — **REUSABLE pattern, drop shadow için aynı tekniği kullanılabilir** (Sprite.Create + Texture2D radial gradient)
- `CliffDynamicFade.cs` (exists, içerik bilinmiyor, runtime fade için referans)
- BG_Far parallax: scene'de RoomBackgroundRig YOK (Glob bulamadı), `project_3kit_bg_architecture_lock` LOCK ama scene wiring bekliyor

---

## BÖLÜM 2 — Görsel "Havadalık Hissi" Yaklaşımları

| Yaklaşım | Mekanizma | LOC | Risk | Demo Impact | Görsel etki |
|---|---|---|---|---|---|
| **A. Cluster size filter (BFS, min N=4)** | CollectCliffCells sonrası flood fill, <4 cell cluster'ı targets'tan çıkar | ~50 | low | 0 gün | 155/166 izole cluster → 0, "ortada kalan" 100% çözülür |
| **B. Dilate/erode morphology** | Floor cell'leri 1-cell erode + dilate, dar boğaz/cıkıntı smooth | ~80 | med | 0.5 gün | Cliff dağılımı yumuşar ama gameplay walkability değişebilir |
| **C. Drop shadow tilemap layer** | Cliff tilemap altında yeni "CliffDropShadowTilemap" (sortingOrder -20). Her cliff cell altına gradient PNG (procedural à la GroundBlobShadow). | ~80 + tile asset | med | 1 gün | "Yükseklik" illusion strong + dramatik |
| **D. BG_Far parallax aktivasyon** | RoomBackgroundRig prefab yarat, 2 katman (cyan fog Z-100, distant relics Z-300), camera-driven parallax | ~80 + scene wire | low | 0.5 gün | Cliff "yüksekte gerçekten" hissi (3-Kit lock ile uyumlu) |
| **E. transformOffset.y / sprite size tune** | DirectionalCliffTile_Hades.asset transformOffset (-0.25f) + sprite asset 64x128 → 64x96 reimport (agy formülü) | ~5 + asset reimport | XS | 0 gün | İnce ayar; alone yetmez ama F için baseline |
| **F. Sang Hendrix parallax loop** | 6-katman parallax (Layer A=1.0, B=0.95, C=0.85, D=0.60, E=0.25, F=0.05). agy C# skeleton hazır. | ~120 + 4 scene asset | med | 1 gün | Endgame "ucurum" feel %100 ama overkill Faz 1 için |
| **G. Hibrit A+C+D (önerilen)** | Cluster filter + drop shadow tilemap + 2-katman parallax | ~210 | med | 1.5 gün | Maksimum "havadalık" + izole 0 + Faz 1 demo timing OK |

### Detay notlar her yaklaşım için

**A — Cluster Size Filter** (PRIMARY FIX, kaçınılmaz)
- agy önerdi (Block 2 #1, FloodFill + minClusterSize=4)
- Çözdüğü problem: kullanıcının "bazı cliffler çok ortada kaldığından oyun ekranında görünmeyecek" direktifi BİREBİR
- Implementation: `CollectCliffCells()` sonrası BFS, <4 cluster targets'tan ExceptWith
- ManualPaintedCells WHITELIST üstün — kullanıcı bilinçli koyduysa kalır
- Risk: çok agresif threshold (N=6, 8) arenanın kenarlarını boşa açabilir → N=4 default, inspector slider

**B — Dilate/Erode** (SKIP, A subsumes)
- agy "SKIP. Connected Component daha hedef" diyor
- Floor map'i değiştirmek riskli (walkability auto-test T1 PASS pattern bozulabilir)

**C — Drop Shadow Tilemap** (HIGH IMPACT, recommended)
- agy Block 3 Method D "KESİNLİKLE KULLANILMALI" 
- GroundBlobShadow.cs pattern ile prosedürel sprite üretimi mevcut → reuse
- Yeni Tilemap GO "CliffDropShadow" sortingLayer Ground veya yeni "Shadows" layer, orderInLayer -20
- Her cliff cell + 1-cell aşağı offset gradient PNG
- Multiply blend (üst üste binmesin diye)

**D — BG_Far Parallax** (medium impact, recommended)
- `project_3kit_bg_architecture_lock` memory zaten Track A=floor, B=cliff face -10, C=parallax bg -300..-500 LOCK
- Scene'de RoomBackgroundRig yok → prefab yarat, 2 katman
- Layer 1: cyan fog Z-100 opacity 30%, hafif scroll
- Layer 2: distant Elysium relics Z-300, ince paralax (camera.position * 0.25)
- Sang Hendrix C# skeleton agy'de hazır, hafif

**E — Sprite Size + transformOffset Tune** (XS, foundational)
- Current: cliff sprite boyutu doğrulanmadı (PNG inspect edilmedi, ama meta görüldü: spritePivot (0.5,1.0), PPU=64)
- agy önerisi: 64x96 (1.5:1) optimal — current 64x128 ise reimport gerekli
- transformOffset.y = -0.25f (agy formülü) — current 0.0
- Risk: spritePixelsToUnits=64 ile 64x128 sprite = 2 cell yüksek, görsel overdraw

**F — Sang Hendrix Full** (DEFER Faz 2+)
- agy "KESİNLİKLE KULLANILMALI" diyor ama 6 katman + asset gen + scene wire = Faz 1 milestone'una yetişmez
- Faz 1: D'nin 2 katman versiyonu yeterli; Sang full Phase 2/3 polish

---

## BÖLÜM 3 — "Ortada Kalan Görünmez Cliff" Sorunu Çözümü

User direktifinin ikinci yarısı: "bazı cliffler çok ortada kaldığından oyun ekranında görünmeyecek"

İki yorum mümkün:
1. **"Cliff'ler ortada izole, görünmeyecek (gizlensin)"** — izole cliff sprite'ları kaldırılsın
2. **"Cliff'ler ortada kalmış, kamera dışı, görünmüyor"** — runtime camera frustum culling

Hibrit cevap:
- **Editor-time:** Yaklaşım A (cluster filter) — izole cliff yerleştirmesin
- **Runtime:** Unity'nin built-in TilemapRenderer.detectChunkCullingBounds zaten frustum culls. Ek script gereksiz.
- **Manuel override:** ManualPaintedCells whitelist (S110 LIVE) zaten kullanıcı bilinçli izole cliff koyabilir; A bu cell'leri korur.

Sonuç: **A tek başına problem 2'yi çözer**. Frustum culling/visibility filter (agy Block 2 #4) SKIP — Unity tarafından otomatik.

---

## BÖLÜM 4 — Triple-AI Summary

### agy verdict (1 paragraf)
agy 34KB döküman teslim etti. Block 1 (Hades, CoM, HLD, Octopath, Sang Hendrix teknikleri) + Block 2 (6 izole cluster algorithm + procedural literature ref) + Block 3 (shadow yöntem A-D karşılaştırma + BG_Far önerisi) + Block 4 (kamera matemati: 64x96 sprite, transformOffset.y = -0.25f formülü). TOP-3 önerisi: Connected Component temizliği + Tilemap-driven shadow + sprite size standardization. Sang Hendrix 6-katman parallax C# skeleton tam çalışır kod sunulmuş.

### Codex verdict
Codex dispatch (yasinderyabilgin profile, xhigh, 1500s timeout) askıya alınmış: 23:12'de task yazıldı ama 23:24 itibariyle CODEX_DONE.md boş, lock aktif. Codex CLI silent stuck olabilir. Orchestrator codebase analizini KENDİSİ yaptı (BFS cluster analysis, scene parse, source file audit) — Codex'in yerine reality check verdi: cluster sayıları gerçek, hipotez çürütüldü.

### Opus sentez (sentez paragrafı)
İlk hipotez (1-3 cell floor adaları → cliff hav ada) ÇÜRÜTÜLDÜ: gerçek sayı 5 cliff. **Esas sorun: 283 cliff 166 cluster'a fragmente, 155 cluster 1-3 cell** — cliff RING devamlılığı yok. Yaklaşım A (cluster filter min N=4) tek başına 155 izole cluster'ı temizler + kullanıcı direktifinin "ortada kalan görünmesin" yarısını çözer. Yaklaşım C (drop shadow tilemap) "havadalık hissi" yarısını GroundBlobShadow pattern ile cheap ekler. Yaklaşım D (BG_Far parallax 2-katman) brand-lock 3-Kit memory ile uyumlu, Hades Elysium "uçan ada" reference'ını çakıştırır. F (Sang Hendrix full) over-engineering Faz 1 için, Phase 2 polish defer. Hibrit G = A+C+D, ~210 LOC, 1.5 gün, demo timing OK.

---

## BÖLÜM 5 — Opus Net Öneri

### Öncelik sırası

**P0 (kaçınılmaz, 0.5 gün):**
1. **Yaklaşım A — Cluster Size Filter** (~50 LOC, CliffAutoPlacer.cs surgical)
   - `CollectCliffCells()` çağrısından sonra BFS, <4 cluster'ı targets'tan ExceptWith
   - Inspector slider `minClusterSize` default 4, range 1-8
   - ManualPaintedCells filter'dan ÖNCE çalışmalı (kullanıcı bilinçli izole'ları korur)
   - Auto-test ekle: `T4_CliffNoFloatingSinglets` (cluster BFS doğrula)

**P1 (yüksek değer, 1 gün):**
2. **Yaklaşım C — Drop Shadow Tilemap** (~80 LOC + 1 tile asset)
   - Yeni runtime component `CliffDropShadowGenerator` (GroundBlobShadow pattern reuse)
   - Procedural radial-vertical gradient sprite (96x48px, alpha top 0.5 → bottom 0)
   - Yeni Tilemap GO "CliffDropShadow" sortingLayer Ground (veya yeni "Shadows" layer add)
   - sortingOrder -20, materyal default sprite-default (Multiply isteğe bağlı)
   - CliffAutoPlacer.Regenerate() sonunda shadow tilemap'i de mirror et

**P2 (brand lock, 0.5 gün):**
3. **Yaklaşım D — BG_Far Parallax 2-Katman** (~80 LOC + 1 prefab + 2 sprite asset placeholder)
   - Prefab `Assets/Prefabs/Environment/RoomBackgroundRig.prefab`
   - Child 1: "BG_Fog" (cyan #00FFCC alpha 0.3, sortingLayer "Background", order -300, X scroll 0.05 unit/sec)
   - Child 2: "BG_FarRelics" (placeholder sprite, sortingLayer "Background", order -400, parallax factor 0.25)
   - Script `SimpleParallaxLayer.cs` (~40 LOC, camera-driven, agy C# skeleton trimmed)
   - PlayableArena_Test01 scene'de child olarak instantiate

**Faz 2 defer:**
4. Sang Hendrix 6-katman parallax full implementasyon
5. URP shader-based cliff face alpha gradient
6. Cliff face decor stratification (Children of Morta moss/rock/root variants)

### Implementation günlük plan

**Day 1 (yarın 2026-05-28):**
- P0 Yaklaşım A impl (Sonnet write, agy C# skeleton adapte, ~50 LOC)
- T4_CliffNoFloatingSinglets auto-test (Phase1Demo asmdef'e ekle)
- P0+T4 verify (BFS doğrula, sayım <4 cluster=0)
- Manual playtest scene reload, görsel kontrol

**Day 2:**
- P1 Yaklaşım C drop shadow impl (Sonnet write, GroundBlobShadow pattern reuse)
- Visual playtest: shadow alpha, gradient direction
- Memory delta: project_cliff_drop_shadow_2026_05_28.md

**Day 3:**
- P2 Yaklaşım D BG_Far parallax (Sonnet write + scene wire)
- 3-Kit memory `project_3kit_bg_architecture_lock` ACTIVATE
- agy review (track B decor cleanup ile uyum check)

---

## BÖLÜM 6 — Open Questions (kullanıcı kararı bekleyen)

1. **Cluster threshold N değeri:** Default 4 öneriyoruz. Daha agresif (N=6) mı, daha gevşek (N=2) mı? Inspector slider runtime tune'a açar.

2. **Drop shadow stratejisi:** Procedural runtime sprite (GroundBlobShadow tarzı) MU yoksa elden çizilmiş PNG asset MI? Procedural daha hızlı, asset daha sanatsal.

3. **BG_Far placeholder asset:** PixelLab gen mi (gece halt!) yoksa Codex `$imagegen` ref mi? Asset gen YASAK gece — placeholder = düz cyan sprite ile başla, asset gen kullanıcı uyandıktan sonra.

4. **Parallax scope Faz 1:** 2-katman (önerilen) mi yoksa Sang Hendrix full 6-katman mı? Demo milestone 10-dk loop'a hangi yetiyor görsel?

5. **transformOffset.y / sprite size:** agy 64x96 + offset -0.25f öneriyor. Mevcut cliff_S.png boyutunu kontrol et. Sprite reimport gerekirse Faz 1 timing zorlayabilir.

6. **"Ortada kalan görünmesin" yorumu:** Yaklaşım A (editor-time silme) yeterli mi, yoksa runtime fade-out (visibility filter) da istiyor musunuz?

---

## BÖLÜM 7 — Yazma Görevlendirmesi (code-writer rotation)

**Hibrit G (A+C+D) seçilirse:**
- **A impl writer:** Sonnet (mekanik BFS, agy C# skeleton'u hazır, Codex limit kısıtlı)
- **C impl writer:** Sonnet (GroundBlobShadow pattern reuse, runtime sprite procedural)
- **D impl writer:** Sonnet (scene wire + 40 LOC parallax script)
- **Review:** Codex xhigh (cluster-aware regression, manual paint whitelist integrity) + Opus rima-design (visual coherence + brand lock 3-Kit uyum check)
- **Asset prep:** Codex `$imagegen` ref art üretsin (BG_FarRelics placeholder cyan rune column), kullanıcı PixelLab Web UI'de final asset (gece halt nedeniyle defer)

**Sadece A seçilirse (minimal MVP):**
- **A impl writer:** Sonnet
- **Review:** Codex review only

**A+C minimal:**
- **A+C impl writer:** Sonnet seri
- **Review:** Codex + agy visual feel verdict

---

## EK A — agy Research File

Tam dosya: `STAGING/CLIFF_FLOATING_FEEL_research_agy.md` (34KB, 475 satır)

Özet:
1. RIMA 1-3 hücrelik izole uçurum adacıkları için DFS/FloodFill Connected Component temizleme + Outcrop ayrımı
2. Hades & Children of Morta estetik: dikey alfa sönümlemeli sprite + shadow-tilemap altlık
3. INACTIVE S110 parallax canlandırma: Sang Hendrix ucurum reçetesi C# kod
4. 75 derece kamera için 64x96 px (1.5:1) sprite + transformOffset.y = -0.25f
5. C# skeleton'lar hazır: `CliffClusterFilter` + `SangHendrixParallax`

---

## EK B — Codex Reality Check (orchestrator-substitute)

Codex dispatch yarıda kaldı (CODEX_DONE.md boş 12+ dakika). Orchestrator Codex'in yerine:

**Algorithm audit (CliffAutoPlacer.cs):**
- L212-244 `CollectCliffCells()` cluster-aware DEĞİL
- L186-190 Regenerate flow: BFS filter eklenebilir L189 sonrasında
- Manual whitelist/blacklist set'leri S110 LIVE, A için intact bırakılmalı

**Scene cluster sayım (orchestrator BFS):**
- Floor: 18 cluster, top-2 = 1166+1114 (toplam 2280), kuyruk 85 cell 16 küçük cluster'da
- Cliff: 166 cluster, top-1 = 12 cell, kalan 282 cell 165 küçük cluster'da
- **155 cluster ≤3 cell** — "havada" hissi kaynağı

**Sprite render geometry:**
- DirectionalCliffTile_Hades.asset transformOffset=(0,0,0)
- cliff_S.png meta: pivot (0.5,1.0) top-center, PPU=64
- CliffTilemap GO yLocalPosition=-0.3046875 (yarım cell DOWN, kasıtlı sarkma)

**Shadow/parallax infrastructure:**
- GroundBlobShadow.cs procedural runtime sprite (reusable pattern)
- CliffDynamicFade.cs runtime fade (mevcut, içerik bilinmiyor)
- RoomBackgroundRig prefab YOK (3-Kit memory'de LOCK ama scene wiring eksik)

**LOC tahminleri:**
- A: 50 LOC (BFS + min filter + inspector field)
- C: 80 LOC (tilemap component + procedural sprite + mirror logic)
- D: 80 LOC + scene wire (parallax script + prefab)
- G (A+C+D): 210 LOC, 1.5 gün

---

## ONAY/RET

User'a soru:
- [ ] Yaklaşım G (Hibrit A+C+D) onaylıyor musun?
- [ ] Yoksa MVP=A only → daha hızlı (0.5 gün)?
- [ ] Veya MVP=A+C (drop shadow tilemap eklensin, parallax defer)?
- [ ] Cluster threshold N=4 default OK mi, yoksa N=6 daha agresif?
- [ ] Codex hâlâ stuck → TaskStop + manuel lock cleanup yapayım mı?

**Sonraki adım:** User cevap verdiğinde Sonnet impl dispatch + Codex review batch hazırlanacak. CODEX_TASKS.md'ye dispatch task formatlandırılacak.

---
*Sentez sonu — Orchestrator Opus, 2026-05-27 gece*
