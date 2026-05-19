# Codex Görev: RIMA Dungeon Feel Analysis & Implementation Roadmap (S95)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## User Direktifi (2026-05-19 S95)

User reference target: **Karar #150 v4 painted concept image** (`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`) — Codex image_gen ile gpt-image-1 backend tarafından üretildi.

User'ın frustrasyon notu: "hayır yahu bunu yapmak bunun formülü çok mu zor ben anlamadım nasıl yapmalıyım"

External reference: https://x.com/Thelastnecro24/status/2056751940562792464 (Twitter tweet — dungeon version of a top-down pixel art game tarz). yt-dlp ile fetch deneniyor; eğer fail olursa Codex senin profile'ında x.com erişimi varsa kontrol et.

## Mevcut Durum

**Karar mimari (LIVE):**
- Karar #150: Fake isometric + dungeon-inside, 32×22 default sub-room
- Karar #149: Sub-room sequence (3-5 sub-rooms per encounter)
- Karar #144: Weaponless body + WeaponSR child
- Karar #100/#114: 8-dir, 35° tilt baked in art
- S94 Transform Squash: Tilemap parent localScale.y = 0.819
- S95 LOCK: Wall decoration = pure attachment only

**Envanter (Act 1 Shattered Keep) 119 PNG:**
- 4 unified wall pieces (straight horizontal, corner L NE, arch opening, cyan rift integrated)
- 3 pillar variants (broken, intact cyan, chained)
- 2 arch portals (entry + exit cyan rift)
- 15 mounting decoration (banner×3, candle, torch, chain×2, shackle, cage, lantern, trophy×3, ivy)
- 3 statues + 5 ritual objects (altar, bench, obelisk, marker, headstone)
- 16 mob enemies (bone walker, imp, slime, rat, spider, wraith, wisp, bat, crawler, goblin, skull, hand, ghost, husk, archer, rat_king)
- 13 props (brazier×2, barrel, urn, ladder, crate, treasure, spike trap, pressure plate, iron grate, lever, rubble heap, debris)
- 35 floor tiles (granite_base 16 pure top-down + granite_low_topdown_v02 16 baked-perspective + iso 3)
- 16 floor decals (crack, pebble, dust, bone chip × 4 variants)
- 3 L4 patches (cave moss, dust drift, cracked rubble)
- 3 rift accents
- 2 scatter (skull pile, bone offering)

**Scene durumu (RoomPipelineTest.unity Spawn_01):**
- 32×22 grid painted with baked-perspective tiles
- Top + bottom + side wall perimeter (wall_straight repeated, corners, 1 cyan rift accent, 1 arch opening)
- 37 mounting decoration overlay sprites on wall positions
- 8 statue/ritual + 3 pillar + 3 brazier + 5 prop
- 7 Light2D point lights (brazier + candle + arch)
- Global ambient 0.25 (dark atmosphere)

**SORUN — neden v4 hissi YAKALANMIYOR:**
1. Tile grid borders visible (PixelLab PNG "ABSOLUTELY NO BORDERS" promptuna rağmen)
2. Wall sprite'lar stamp/repeat görünümlü (Hades cohesive painted illusion DEĞİL)
3. Karakter çok küçük (camera ortho 12, room 32×22)
4. Composition focal point yok
5. Atmospheric vignette + cinematic lighting yok

**Önceki orchestrator önerisi (henüz yapılmadı):**
"Painted background per room" — her oda için TEK büyük painted image (Codex image_gen, 1536×1024), gameplay sprite'lar üstüne overlay. v4 reference image bu yöntemle üretilmişti.

## Görev — Codex'e Sor

**Sor 1:** v4 reference image (`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`) detaylı analiz et. Aşağıdakileri belirle:
- Floor pattern technique (tileable mı painted mı, perspektif level)
- Wall construction (stamped piece mi unified painted mı, depth tekniği)
- Lighting layering (key/fill/rim, point light placement, vignette)
- Decoration density + composition rules (focal point, sub-zoning)
- Color palette extraction (granite cool grey + cyan rift + warm torch orange)
- Character scale relative to room

**Sor 2:** Bu feel'i Unity'de yakalamanın **3 mümkün yolu** sırala (en pragmatic'ten en complex'e):
- (A) Painted background per room (Codex image_gen, 1536×1024) + overlay sprite gameplay
- (B) Modular pieces stamped + extensive shader work (vignette, color grade, blur on tile borders)
- (C) Hybrid — painted background floor + walls as sprites + overlay sprites
- Her birinin pros/cons + RIMA için recommended seçim

**Sor 3:** Mevcut 119 PNG envanteri Y%U metoduyla nasıl entegre olur? Painted background gen'i mevcut sprite'ları reference olarak alabilir mi (Codex image_gen prompt input)?

**Sor 4:** External reference (necro24 tweet) — eğer x.com erişimin varsa fetch et, analiz et, RIMA hedefine compare et. Erişim yoksa: PixelArt + top-down dungeon style genel community trend'i tarif et.

**Sor 5:** Concrete implementation roadmap — 1 oda demo için (player + 1 mob + walking) önümüzdeki 2 saatte ne yapacağımı sıralı belirt. Painted background yolu önerirsen Codex image_gen prompt template'i ver.

**Sor 6:** Risk inventory — bu pivot'ta neyi kaybediyoruz (mevcut 119 PNG gen budget, scene compose work)? Salvageable nedir?

## Çıktı Format

```
## V4 Reference Analysis
[detaylı teknik analiz, 3-5 madde]

## 3 Path Comparison
A. Painted Background:
   Pros: ...
   Cons: ...
   Cost: ...
B. Modular + Shader:
   ...
C. Hybrid:
   ...
Recommended: [seçim] — Why: ...

## External Reference (necro24)
[fetch sonuç veya genel community style note]

## Mevcut Envanter Entegrasyon Stratejisi
[119 PNG nasıl salvage edilir]

## Implementation Roadmap (2-saat 1-oda demo)
1. ...
2. ...
3. ...

## Risk Inventory
[3-5 risk + mitigation]

## Sonraki 3 Concrete Action (en pragmatic 30dk)
1. ...
2. ...
3. ...
```

Cevap max 2500 token, code önerme (planlama only). NLM gerekirse query yap.
