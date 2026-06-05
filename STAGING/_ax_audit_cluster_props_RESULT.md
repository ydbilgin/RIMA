# Cluster-Prop Audit SONUÇ (ax-3.5-Flash, 2026-06-05)

## BUGÜN kullanılabilir (yeni art GEREKMEZ) — 3 aday
1. **stone_moss_pair.png** 128×128 → 2×2 yosunlu çift-kaya kümesi (blocking)
   ⚠️ `Assets/Sprites/Environment/_archive~/Phase0_ScaleTest/_picks/wang16/` içinde — `~` klasörünü
   Unity IMPORT ETMEZ → önce aktif path'e (Props/) KOPYALANMALI.
2. **rubble_pile.png** 96×64 (`RuinedKeepKit/`) → 2×1 moloz yığını (low-blocking)
3. **pl_rubble.png** 88×56 (`PixelLabKit/`) → 2×1 tuğla-döküntü scatter (non/low-blocking)

## Mevcut 7 PropDefinitionSO envanteri
BonesMarker 1×1 · Brazier 1×1(Circle) · ChasmGap 3×2 decal · FloorRiftCrack 1×1 decal ·
Pillar 1×1(Capsule) · WallStub 2×1(Box) · barrel_001 ⚠️ SPRITE ATAMASI EKSİK (yan bulgu).

## GEN SPEC taslakları (GATED — kullanıcıyla; PixelLab create_map_object)
1. **BoulderPile_2x2** 128×128: slate-grey granit kümesi, çatlaklarda ≤%15 cyan emissive.
2. **FallenLog_3x2** 192×128: çürük devrik kütük, kül-gri kabuk, %0 cyan.
3. **ContainmentBoneHeap_2x2** 128×128: bone-white yığın + merkez kafatası/kaburga üstünde ≤%10 cyan rün
   (canon: kemik = failed-containment bedenleri).
Tam prompt taslakları transcript'te (2026-06-05 ax audit) — gerektiğinde buradan genişletilir.

## Sonraki adım
cx overlay-tilemap bitince → Sonnet-MCP task: stone_moss_pair'i aktif path'e kopyala+import (PPU64/Point) →
3 yeni PropDefinitionSO (BoulderPair_2x2 / RubblePile_2x1 / DebrisScatter_2x1) + PropRegistry kaydı +
barrel_001 sprite fix → IsoRoomBuilder'da örnek odada doğrula.
