---
name: blueprint-first-map-design
description: "Map composition'da önce semantic zone blueprint (yol/çim/taş/duvar/su/feature), sonra zone'a göre rule-based prop placement. \"Saçma obj yerleştirme\" engellemek için zorunlu adım."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 2beacb18-e0f5-45c2-8ce5-d1b7fd2c9826
---

# Blueprint-First Map Design

**Kural:** Map composition dispatch'i (Codex pro redesign, Phase A v* iterasyonu, vb.) **3 adımlı sırayla** çalışır. Random prop scatter YASAK; her prop bir zone'a ait olmalı.

1. **Adım 1 — Semantic Blueprint (intent map):**
   - Map'i 6-10 semantic zone'a böl: `path` (toprak yol/cobble), `grass` (çim/flora), `stone` (kaya/sert zemin), `wall` (engel/sınır), `water` (sulak/wet), `feature` (statue/altar/landmark/encounter spot), `transition` (zone sınırı)
   - Her zone için **bounding region + intent label** belirle (ASCII grid veya screenshot annotation OK)
   - Codex consultation gate: "Bu blueprint mantıklı mı?" PASS/FAIL

2. **Adım 2 — Rule-Based Prop Placement:**
   - Her zone kendi pool'undan prop alır:
     - `path` → cobble, sand, footprints, lanterns
     - `grass` → flora, bushes, small flowers, fallen leaves
     - `stone` → rocks, pebbles, cracks, mossy stones
     - `wall` → barriers, fences, broken walls
     - `water` → puddles, reeds, wet stones
     - `feature` → statue, altar, banner, encounter marker
   - **Pool dışı prop yasak.** "Grass zone'a barrel" gibi mantıksızlık banned.
   - Density rule: feature zone = 1-2 hero prop, transition zone = sparse boundary props

3. **Adım 3 — Adjacency Rules (transition):**
   - Komşu zone'lar arası geçiş decal'i: grass↔stone = mossy stones; path↔grass = scattered pebbles+grass; water↔grass = reeds
   - Hard cut yasak — her zone boundary'sinde 1-2 transition decal

**Why:** User feedback 2026-05-18 night ("kalkıyorum" mesajında) — Combat v14, v11, v12 hep "saçma obj yerleştirme" problemi yaşadı. v11 8-zone biome map var ama içine props random gelmiş, "tam map gibi değil" verdict'i. Sorun projection veya art değil, **placement intent eksikliği**. ChatGPT layered recipe (L0-L11) zone framework veriyor ama prop pool/adjacency rule yok — kapatılması gereken boşluk burası.

**How to apply:**
- **Yeni Codex map composition dispatch'ı yazarken:** Spec'in §1'i "Blueprint draft → consultation → approve" olmalı, §2 prop placement, §3 adjacency. Codex Adım 1 PASS olmadan Adım 2'ye geçmesin.
- **Phase B-3 Blueprint Painter feature:** Map Designer'a semantic zone brush ekle (6 brush: path/grass/stone/wall/water/feature). "Auto-Populate" butonu → her zone'a rule-based prop scatter. User önce zone paint, sonra "populate" → mantıklı map çıkar.
- **Phase A v15+ retry (Combat v14 fix yetersizse):** Yeni dispatch'te blueprint-first zorunlu — Codex önce 8 zone intent map çıkarsın, sonra populate.
- **Existing Brush V1 PaintMode enum (Floor/Path/Decor/Wall)** zaten semantic — Blueprint Painter bunun üstünde "intent layer" olarak çalışır, atlas/asset swap'a değmez.

**Re-design trigger (S88_LATE user direktifi):** Blueprint Painter (Phase B-3) LIVE olur olmaz **mevcut Combat v14 map'i baştan yeni mantıkla redesign** edilecek. Eski "saçma scatter" v14 atılır, yeni blueprint zone'larıyla yeniden inşa. **Eksik asset** (transition decal, feature prop, zone-specific dressing) tespit edilirse → **Codex imagegen dispatch** (gpt-image-1, hybrid asset pipeline LOCK uyarınca). Asset üretim sırası: zone-specific tile/floor → transition decals → feature props → atmospheric accents.

Related: [[brush-v1-manual-composition-system]] [[room-composer-paint-intent-lock]] [[karar-143-layered-pipeline]] [[hybrid-asset-pipeline-lock]]
