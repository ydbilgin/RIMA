# TASK: Cliff Rendering Optimization Strategy — Research + Verdict

ACTIVE RULES: (1) think before reviewing (2) min response, no speculation (3) cite specific Unity APIs / docs / benchmarks (4) BLOCKED if uncertain.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: RIMA'nın CliffAutoPlacer sistemi şu an her cliff için ayrı bir GameObject + SpriteRenderer üretiyor. 200-500 tile'lık bir arenada bu 500-1500 cliff GameObject olur. Üç açıdan etki analizi + somut optimization önerileri istiyorum:
1. **Editor perf:** Hierarchy 500+ child GameObject, Scene View'da slow scroll/render
2. **Runtime perf:** Draw call sayısı, batching, frame time
3. **Memory:** GameObject + Transform + SpriteRenderer overhead per cliff

Çıktı **inline** — dosya YAZMA, sadece chat response. RIMA orchestrator (Opus) sentez yapacak, kullanıcıya sunacak.

## RIMA Context
- Engine: Unity 6 (URP 17.3.0, 2D Renderer)
- Render path: URP 2D Renderer + 2D Lights (Light2D), Pixel Perfect Camera (şu an disabled, gelecek için potansiyel)
- Sahne: V1 wall-less Hades Elysium floating arena, ~134 tile floor, ~536 cliff GameObject (4-side check per cell, deterministic)
- Cliff sprite: 128×192, PPU=64, top-center pivot, statik (hareket yok)
- Sprite atlas: yok şu an (her cliff sprite'ı ayrı asset)
- Target FPS: 60 (PC), 30+ (mobile potansiyel future)
- Lighting: Global Light 2D + Freeform + URP Bloom
- Cliff parent: tek CliffRing GameObject altında 500+ child

## Görev — 4 bölüm

### Bölüm 1: Mevcut sistemin maliyeti (somut sayılar)
- 536 GameObject = kaç bytes memory? (Transform + SpriteRenderer + GameObject overhead Unity'de net)
- Draw call etkisi: 536 sprite, sprite atlas yok → kaç draw call beklenir? (URP 2D dynamic batching nasıl?)
- Editor Hierarchy 536 child performansı: bilinen issue var mı?
- Scene View frame time impact

### Bölüm 2: Optimization Alternatifleri — Karşılaştırma
4 yaklaşımı **artı/eksi** ile değerlendir:

| Yaklaşım | Implementasyon zorluğu | Perf kazancı | Editor UX kaybı |
|---|---|---|---|
| **A. Status quo + SpriteAtlas batching** | XS | ? | yok |
| **B. Static Batching flag** | XS | ? | yok |
| **C. Cliff Tilemap (ikinci tilemap, GameObject yok)** | M | ? | inspect tek tile |
| **D. Mesh combine runtime** | L | ? | edit-mode'da görünmez |

Her satır için:
- Implementation detayı (hangi Unity API, hangi script)
- Beklenen frame time / draw call değişimi (somut sayı veya range)
- Trade-off (editor'da görsellik, debug edilebilirlik, vs)
- RIMA'nın 8→1 yön refactor sonrası (tek sprite + N varyant) durumunda uygulanabilirlik

### Bölüm 3: Tilemap-Based Cliff Rendering — Derin Bakış
"Cliff'leri ayrı bir Tilemap'e tile olarak yerleştir" yaklaşımı RIMA için en güçlü aday görünüyor. Detay:
- Cliff sprite'ları RuleTile veya basit Tile asset'leri olarak nasıl wrap edilir
- Per-cell variant seçimi nasıl yapılır (CliffAutoPlacer'ın deterministic random'ı tilemap'e nasıl transfer edilir)
- Y-axis offset (worldOffset.y=0.15) tilemap pivot ile uyumlu mu
- Sorting layer/order tilemap'te nasıl çalışır
- Lighting (Light2D) tilemap'te aynı çalışır mı, GameObject'ten farkı var mı
- Hades / Children of Morta / Diablo III gibi referans oyunlar bunu nasıl yapıyor (eğer info varsa)

### Bölüm 4: Verdict — RIMA için Öneri
- En önerilen yaklaşım (1 adet, kesin)
- Neden (somut justification)
- Implementation effort (S/M/L)
- Risk (eğer varsa)
- 8→1 yön refactor'la birleştirilmesi gerekli mi yoksa bağımsız mı

## Hard constraints
- **Inline only** — dosya yazma, scratch dahil
- Speculative chaining yasak — "şöyle de olabilir" tipi öneriler değil, net karar
- Unity API isimlerini doğru ver (URP 17.3, Tilemap API, SpriteAtlas API)
- BLOCKED: bilgi yetersiz veya çelişki varsa söyle, uydurma

## Beklenen toplam uzunluk
500-800 kelime. Karpathy 4 — max signal, no fluff.
