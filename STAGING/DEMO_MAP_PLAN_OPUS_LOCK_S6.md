# DEMO MAP — OPUS SYNTHESIS + LOCKED PLAN (2026-06-01)

Yazan: Opus (orchestrator). Kaynak: cx teknik analiz (`CODEX_DONE.md → ## CX ANALYSIS — DEMO MAP`) + ax design analiz (`AGY_DONE_ydbilgin.md`) + Opus kod-okuma (RoomCliffSolver/RoomDepthStack/CliffGenerateAction/menü envanteri). cx+ax fikrini verdi, **son karar Opus**.

## Hedef
Güzel seamless izometrik demo map + temiz tek-araç designer + asset pack (floor/cliff/object) + matematiksel cliff-under-floor. Karakter/anim/mob HARİÇ (kullanıcı yapacak).

## 🔒 LOCKED KARARLAR (Opus, re-litigate etme)

### Floor kaynağı
- **`PixelLabFloorFlat/flat_0-15`** = demo default (64×64, PPU64, center, point, uncompressed — cx doğruladı doğru). `pl_floor_*` (PPU100 bilinear compressed) = demo default DEĞİL → archive adayı.
- Grid: iso layout, cellSize (0.94,0.94,1) — proje konvansiyonu (0.94 overlap = seam gizleme). KORU.

### Cliff math (cx + ax converge)
- Cliff **floor cell'in ÜSTÜNE** konur (void cell'e değil), top-pivot sprite aşağı sarkar = "ada havada" hissi. KitB_Cliff sprite'ları DOĞRU (128×192, PPU64, top-pivot 0.5,1).
- Sorting: `RoomDepthStack` Cliff = `Ground` / order `-10` (floor'un altı). DOĞRU.
- **Offset:** PPU64'te floor 64px=1u, cliff 192px=3u. Top-pivot cliff floor-cell merkezine konunca üst kenar merkeze hizalanır → ax'in `offset.y≈1.5` (cliff yüksekliğinin yarısı kadar aşağı sark) önerisi mantıklı. **DirectionalCliffTile transformOffset normalize edilecek** (şu an 0 = yanlış).
- Cliff yön: ax "sadece S/SE/SW" diyor (kamera-önü). RoomCliffSolver zaten kamera-arkası N/NE/NW'yi kesiyor ✓ — uyumlu.
- **8-yön bitmask kontratı (cx önerdi, Opus onayladı):** void-facing N=1,E=2,S=4,W=8,NE=16,NW=32,SE=64,SW=128 → S=`cliff_S`, SE=`cliff_SE`, SW=`cliff_SW`, E=`cliff_E`, W=`cliff_W`. `DirectionalCliffTile_Hades` şu an 8 yönü AYNI sprite'a bağlamış = ANA GAP, düzeltilecek.

### Look (ax)
- Floor şekli = düzensiz jagged iso diamond (kare grid değil), concave notch + peninsula.
- Cyan #00FFCC = görünür yüzeyin **%5-8**'i (rift/rune/rim). Aşma.
- 3-zon: merkez combat (temiz, %15 max decor) / çevre prop (pillar/brazier/banner) / void background (floating ruins parallax).
- Cliff dibinde contact fog/AO (cut-cardboard önle) → mevcut `edge_ao_rim` + `corner_fade` overlay'leri kullan.

### Demo oda çeşitliliği (ax)
- Oda 1 Combat Arena: temiz charcoal granite, 2 çıkış portalı.
- Oda 2 Sanctuary: warm amber (brazier_lit), moss/rubble, dinlenme.
- Oda 3 Boss containment: yüksek rift/rune yoğunluğu, void boundary.

## 🚧 EN KRİTİK BLOCKER (cx keşfi — demo'dan önce ÇÖZ)
`RuntimeAssetRegistry` tag'leri: cliff/column/decal/door/misc/mounting/statue/tile/wall VAR; **floor/prop/portal/light YOK**. `DesignerCategoryMap.RegistryTag` tam bunları bekliyor → floor/object/portal/light palette'leri BOŞ. Ayrıca `PixelLabFloorFlat`+`KitB_Cliff` bake-root'larda yok. → **EXEC 2'de registry baker root+tag düzeltilecek, re-bake.** Bu olmadan demo map araçtan boyanamaz.

## ▶ EXEC SIRASI (Opus lock)

**EXEC 1 — Menü konsolidasyonu** (cx/yekta yazar, Opus+ax review). Bağımsız, mekanik. cx 3 ek yan-etki buldu → task'a eklendi: (a) `LiveToolPaletteWindow.cs:229` ExecuteMenuItem çağrısını yeni path'e güncelle, (b) `SampleRoomLibraryGenerator` + `SampleRoomLibraryGeneratorTests` taşı, (c) DungeonSetup/CombatTestSetup validate overload'larını da güncelle.

**EXEC 2 — Asset Pack + Registry bake fix** (BLOCKER). (a) Baker root'larına `PixelLabFloorFlat`, `KitB_Cliff`, `IsoKit/decor` ekle. (b) Tag keyword'lerine floor (flat_/floor), prop (decor_/pillar/brazier/rubble), portal, light ekle. (c) Portable `AssetPackSO` (floor/cliff/object kategorileri) → bake-to-RuntimeAssetRegistry. (d) Re-bake → palette dolu doğrula.

**EXEC 3 — Cliff math** (writer + 2 review + EditMode test). DirectionalCliffTile 8-yön bitmask (KitB_Cliff 8 sprite doğru yöne) + transformOffset normalize (cliff hang-down) + RoomCliffSolver cluster-filter parity (cx CX REVIEW#5). cliff_S_new1-4 demo'da kullanma (PPU uyumsuz, ana kit yeterli).

**EXEC 4 — Demo map assembly** (Opus + MCP). ax 3-zon layout → RoomData programatik (RoomDataMutator) + GenerateCliffs + RoomDataComposer.Compose + screenshot-verify (edit-mode).

**EXEC 5 — imagegen placeholder** (cx/laurethayday imagegen). Eksik: door/gate (portal marker), map-fragment, reward pickup. cx+ax prompt birleşik (iso, charcoal/iron + cyan-sparing, PPU64, transparent). PLACEHOLDER_REGISTRY'ye logla. MapFragment/RewardPickup/Chest prefab'ları VAR ama sprite boş → sadece sprite bağla.

**EXEC 6 — Final**: EditMode test suite + CURRENT_STATUS + memory + commit/push GATED.

## Routing
writer≠reviewer (cx yazar → Opus+ax review / Opus yazar → cx+ax review) · cx: laurethayday öncelik, biterse yekta · ax: gemini reviewer · her kod adımı Unity-compile + console-clean · MCP scene-edit play-mode-OFF · commit/push GATED · PixelLab YENİ üretim YOK (mevcut tile'lar) · imagegen sadece eksik door/fragment/reward.
