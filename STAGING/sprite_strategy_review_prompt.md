# Sprite Üretim Stratejisi — Cross-Review (ChatGPT için)

## Bağlam

RIMA 2D roguelite, Hades-tarzı painter pixel art, 128px native. Karar #143 ile 6-layer map pipeline locked:
- L1 floor base (Tilemap 32×32 grid)
- L2 floor variation (Tilemap 32×32 grid)
- L3 wall overlay (SpriteRenderer perimeter cap, NOT tileset)
- L4 transition brush (SpriteRenderer, oval moss/dirt, free placement)
- L5 detail decal (SpriteRenderer, small cracks/rubble, free placement)
- L6 accent rift (SpriteRenderer, hero rift fracture, sparse)

Brush V1 (Sprint 1-8 LIVE) custom EditorWindow → composite brush stroke ile aynı anda birden çok layer boyar. Photoshop-style hotkey (B/E/[/]/1-9), AnimationCurve density, weighted asset pick (Q5 LOCK).

**Asset üretim aracı:** PixelLab web UI "Create Image Pro" (user manuel). MCP'de YOK. Boyut seçenekleri:
32×32, 64×64, 128×128, 256×256, 344×192 (16:9), 341×341, 384×216 (16:9), **512×512, 512×288 (16:9), 632×424 (3:2), 424×632 (2:3), 688×384 (16:9)**.

Camera: 30-35° high top-down ARPG view (NOT isometric grid). 4 cardinal directions for characters.

## Problem

Brush V1'in "Aseprite-tarzı brush size" davranışı için sprite kaynağı stratejisi gerekiyor. Pixel art'ta runtime scale (non-integer) YASAK — blur olur. Aseprite analoji: pen size küçük → küçük stamp, büyük → büyük stamp. Bizde de "brush radius `[` `]` ile değişince farklı boyutta dekal düşmeli" ama scale değil.

İki strateji aday:

### Strateji A — Pre-baked Sub-Cuts (Statik PNG Variant)

**Yöntem:** User PixelLab'da 1 tane büyük master sprite üretir (örn. L4 moss için 512×512). Claude veya Codex bu master'ı önceden bilinen N parçaya keser (örn. 1× 256×256 hero + 4× 128×128 medium + 12× 64×64 small = 17 ayrı PNG dosyası). Her parça ayrı Sprite asset olarak Unity'ye import edilir, AssetPool'a girer.

**Brush davranışı:** Brush radius `[` `]` ile değişince → BrushPack içindeki size bucket pool'undan weighted random pick.

**Artıları:**
- Basit zihinsel model — her variant ayrı dosya
- Asset Pool yönetimi kolay (PNG sayar gibi)
- Pre-cut bake script (one-time) → runtime cost YOK
- AssetPool'a ekleme/çıkarma manuel + kontrol edilebilir

**Eksileri:**
- 17 ayrı PNG dosyası = file system kalabalığı
- Master sprite'tan bir variant beğenilmezse → tüm master regen + recut
- Texture memory = N tane ayrı texture (Unity batching iyimser değil)
- Sub-cut sınırları master üretim sırasında bilinmeli (post-hoc esneklik düşük)

---

### Strateji B — Unity Sprite Editor Multi-Slice (Runtime Sub-Rect)

**Yöntem:** Tek 512×512 PNG Unity'ye import edilir, Sprite Editor "Multiple Mode" ile içinde 8-16 sprite rect tanımlanır (örn. hero rect(0,0,256,256), corner rect(256,0,128,128), small rect(384,0,64,64) ...). Tek texture, tek import, ama Sprite[] dizisi.

**Brush davranışı:** Aynı — radius değişince Sprite[] array içinden size bucket pick.

**Artıları:**
- Tek texture → Unity sprite batching dostu (draw call az)
- Texture memory tek alan
- Sub-cut sınırları Unity'de **post-hoc düzenlenebilir** (master regen gerekmez, slice'ı kaydır)
- Master sprite tek dosya → asset browser temiz
- Sprite Editor'da custom pivot per slice → free placement kontrolü iyi

**Eksileri:**
- Unity Sprite Editor manuel slicing iş yükü (her import sonrası check)
- Auto-slice grid garanti vermez (organic boyutlar için manual)
- Slice tanımı `.meta` dosyasında → versiyon kontrolünde dikkat
- BrushPack ScriptableObject Sprite reference array → asset dependency zinciri uzar
- Sub-sprite'lar arası boşluk (transparent padding) master üretiminde planlanmalı, yoksa sprite'lar birbirine bulaşır

---

## UX GEREKSİNİMİ (KRİTİK)

Kullanıcı sadece **Paint-style basit fırça davranışı** ister:
- Brush seç → sahnede sürükle → bırak
- Boyut `[` `]` ile küçült/büyüt
- Sonuç: **otomatik mantıklı, biome-tutarlı, atlas-kurallı dekoratif set**

**Backend (brush V1 + AssetPool + slice mantığı) görünmez olmalı.** Kullanıcı:
- Hangi sub-cut seçileceğini düşünmemeli
- Master/variant ayrımıyla uğraşmamalı
- Atlas density/edgeBias/minDistance ayarlarını manuel girmemeli
- Slice rect'leri elle çizmemeli
- Composite layer'lar arası koordinasyonu kafasında tutmamalı

**Sistem otomatik yapmalı:**
- Brush radius → uygun size bucket pick (linear/exponential/preset — kararını sen ver)
- Atlas kurallarını (encounterAvoid, edgeBias, walkable filter) stroke sırasında uygula
- Variant pick'i weighted random + minDistance + flip ile çeşitlendir
- Composite brush ise birden çok layer'ı tek stroke'ta koordine et
- Stroke bittiğinde sonuç görsel olarak "Hades sahnesi gibi mantıklı" oluşmalı

**Bu UX gereği iki stratejiyi nasıl etkiler?**
- Strateji A (pre-cut PNG) → AssetPool'da N variant + size bucket etiketi gerekir. Brush selection backend'de filtreler.
- Strateji B (Unity slice) → Sprite[] array'inde size metadata + bucket tag gerekir. Slice tanımı + pivot per slice manuel iş.

İki stratejiden hangisi bu "Paint-gibi-davran, mantıklı-set-çıkar" promise'ini **kullanıcı tarafında SIFIR ek iş** ile garanti edebilir? Cevabı buna göre tartart.

## Sorulan Sorular

1. **Hangi stratejiyi öneriyorsun (A/B/Hybrid)?** Neden?
2. **Pixel art ile uyumu** — her iki stratejinin pixel-perfect kalış garantileri nedir? Slice boundary'lerinde yaşanabilecek artefakt riskleri?
3. **Aseprite/Krita/Polybrush analojisi** — bu tools'lar brush size'ı nasıl çözüyor? Bizim için doğru mental model hangisi?
4. **Production iş yükü** — User PixelLab'da kaç dispatch yapacak? Master + slice planı toplam credit sayısını nasıl etkiler?
5. **Master boyut önerisi (her layer için):**
   - L3 wall horizontal (yatay duvar cap, irregular silhouette, organic edge)
   - L3 wall vertical (dikey duvar cap)
   - L3 wall corner (4 corner — NE/NW/SE/SW — tek master'a sığar mı?)
   - L3 wall doorway (gap transition)
   - L4 transition (moss/dirt patches, multi-size brush istenir)
   - L5 detail (small cracks/rubble, çoğunlukla küçük)
   - L6 accent (rift hero — büyük dramatic)

6. **Hybrid mümkün mü?** Örn. L3 wall = Strateji A (her duvar tipi ayrı PNG), L4-L5 = Strateji B (master + slice). Hangi kombinasyon mantıklı?
7. **Brush radius → sprite pick mapping** — `[` `]` hotkey ile radius 1-10 değişirken hangi size bucket'ı pick etmeli? Linear mapping mi, exponential mi, manuel preset (small/medium/large/hero) mi?
8. **Karar #143 atlas kurallarıyla uyum** — encounterAvoidRadius, edgeBiased, minDistance, wallProximityFactor: hangi strateji bu kurallarla daha iyi entegre olur?
9. **Brush V1 mevcut implement** ile değişiklik gerektirir mi? Sprint 6 (default brush pack + composite executor) ve Sprint 7 (Auto-Dress + Regenerate + Smart Fill) bu stratejilerden etkilenir mi?
10. **Ölçeklenebilirlik (V2)** — biome marketplace, namespace prefix, conflict resolution gibi V2 hedefleri hangi stratejiyle daha kolay?

## Beklenen Çıktı Formatı

```
1. ÖNERİ: [A / B / Hybrid] — tek cümle özet
2. PRINCIPAL REASON: [3-5 cümle teknik gerekçe]
3. PIXEL-ART RISKS: [her stratejinin spesifik risk listesi]
4. PRODUCTION COST: [dispatch sayısı ve credit tahmini her layer için]
5. MASTER BOYUT TABLOSU: [her layer için önerilen master boyut + slice sayısı]
6. BRUSH RADIUS MAPPING: [`[` `]` ↔ size bucket spesifik kural]
7. KARAR #143 UYUM: [atlas field'larıyla nasıl çalışır]
8. V1 IMPLEMENT IMPACT: [değişen sprint/file/spec listesi]
9. V2 FORWARD-COMPAT: [marketplace/namespace uyumu]
10. SHIPPED EXAMPLES: [Hades / Dead Cells / Hyper Light Drifter / Polybrush gibi referans örnek var mı?]
11. UX PROMISE GUARANTEE: [hangisi "kullanıcı sadece Paint gibi boyar, sistem otomatik mantıklı set üretir" promise'ini SIFIR ek iş ile karşılar?]
```

## Notlar

- Pixel-perfect KRITIK — runtime scale YOK, integer scale bile tartışılır
- Brush V1 LIVE — geriye uyumluluk önemli ama mecbursa kırılır
- User PixelLab credit'i azalmasını tercih eder (master + slice = az dispatch)
- Sub-cut bake script gerekirse Claude/Codex implement eder, problem değil
- Karar VERİLDİĞİNDE batch dosyaları (`pixellab_l3_wall_batch.md` + `pixellab_l4_l5_l6_batch.md`) baştan yazılacak
