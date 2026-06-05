# DECISION — Modular Map Learnings: Prop Mirror + Overlay Mask + Checker Floor (2026-06-05)

**Council:** ax-3.1-Pro (architecture) ‖ ax-3.5-Flash (lean) ‖ Opus-advisor (design+görsel, `_council_opus_modular_props.md`) → Opus sentez. (cx o sırada CharSelect v3.1'de meşguldü; Opus-advisor dosya-doğrulamalı feasibility yaptı — BridsonPoissonAutoPlacer/CompositionRoleMap/PropDefinitionSO.PickVariant mevcut ve çalışır.)
**Girdi:** kullanıcı referans görseli `STAGING/mockups/ref_modular_garden_game_2026-06-05.png` (cozy iso bahçe oyunu) + brief `_council_brief_modular_props.md`.

## 🔑 EN ÖNEMLİ BULGU (Opus-advisor, görsel reality-check)
Referansın "derinliği" %90 KOZMETİK: kenar cliff-skirt (RIMA'da ZATEN VAR) + tek çukur nehir basamağı.
Gerçek çok-katlı playfield YOK. Zenginlik = ~10-12 kaynak asset'in mirror+grup ile tekrarı + düz patika
maskesi + sıkı A/B damalı çim + temiz-merkez kompozisyon. **Ve: RIMA'nın en büyük ROI'si yeni kod DEĞİL —
zaten sahip olduğu `BridsonPoissonAutoPlacer` + `CompositionRoleMap` altyapısını 15 prop-fakiri odaya
ÇALIŞTIRMAK.**

## KARARLAR (3/3 OYBİRLİĞİ)

### K1 — PropGroupSO: HAYIR (yeni SO tipi yok)
Kümeler (kaya yığını + devrik kütük) = **tek multi-cell PropDefinitionSO** olarak author edilir (footprint +
variant sistemi ZATEN destekliyor). + `PropPlacementData`'ya **`bool flipX`** eklenir (runtime
`spriteRenderer.flipX`), auto-placer %50 şansla mirror atar → varyete bedavaya ikiye katlanır (~3-10 satır).
(3.1 Pro'nun "author-time stamp macro PropGroupSO" fikri = v2-only, authoring hacmi büyürse.)

### K2 — Overlay/Path maskesi: EVET — ikinci Tilemap (decal sprite ASLA)
`RoomTemplateSO`'ya `overlayMask` (byte grid) + IsoRoomBuilder'da AYNI Grid'i paylaşan `OverlayTilemap`
(Ground'un hemen üstü sort order). Serbest decal sprite YASAK (Custom-Axis depth-sort arenasına girer =
glitch; tilemap depth-sort'u tamamen bypass eder). Grid-paylaşımı 32px tile-drift riskini de NÖTRALİZE eder.

### K3 — Çok-teras (heightGrid): YAPILMAYACAK (şimdi değil; muhtemelen hiç)
Custom-Axis depth-sort kilidi + Player/Enemy collision kilidi + cliff-solver tek-düzlem varsayımıyla çakışır
(L+ iş). Referans bile gerçek teras kullanmıyor. Derinlik hissi gerekirse: cliff-skirt (var) + "çukur" overlay
tile illüzyonu.

### K4 — Sıralama: AYRI küçük task'ler, B-12'den ÖNCE (çift-iş önleme)
1. **[S] flipX mirror** (PropPlacementData + IsoRoomBuilder + auto-placer %50 şans)
2. **[S] 2-3 multi-cell küme prop'u author et** (kaya+kütük; mevcut PropDefinitionSO; PixelLab/asset üretimi
   gerekirse KULLANICIYLA — gen GATED)
3. **[M] Overlay path tilemap** (RoomTemplateSO.overlayMask + builder layer)
4. **[M] Damalı zemin: `(x+y)%2` A/B parite** (BuildFloor'da; 16-random-varyant=gürültü, sıkı checker=bakımlı
   görünüm — Opus-advisor brief'in (c) maddesini reddetti, kabul) **+ mevcut BridsonPoissonAutoPlacer'ı
   CompositionRoleMap kurallarıyla 15 odaya ÇALIŞTIR** (yeni kod ~yok, en görünür kazanım)
5. → sonra **B-12 production RoomBank [L]** yeni oyuncaklarla.

## Dosya referansları (Opus-advisor doğruladı)
`Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs` · `Assets/Scripts/MapDesigner/Composition/CompositionRoleMap.cs` · `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (BuildProps ~L542, BuildFloor ~L240) · `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs` (PickVariant ~L51).

## Disagreement kaydı
- 3.1 Pro: PropGroupSO-stamp istedi → 2/3 çoğunluk + lean gerekçeyle REDDEDİLDİ (v2 notu düşüldü).
- Brief'in ön-analizi (Opus orchestrator) iki yerde düzeltildi: terası "v2 adayı" demişti → council "yapma";
  16-varyant damalı yeterli demişti → sıkı A/B checker tercih edildi.
