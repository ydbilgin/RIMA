# Unity Demo Polish Batch 2 — DONE (3 cerrahi fix)

Tarih: 2026-06-18 · Builder (Opus, tek Unity ajanı) · spec: `unity_polish_batch2_2026-06-18.md`

## FIX A — HUD HP rengi cyan→crimson  ✅ DONE
- **Dosya:** `Assets/Scripts/UI/HUDController.cs`
- **Kök neden:** `OnHPChanged` HP fill'i eşik-tint ile boyuyordu — `RimaUITheme.HpHealthy` (#4A9EBF, cyan-mavi) >%60'ta uygulanıyordu → HP barı cyan görünüyordu.
- **Değişiklik:**
  - `OnHPChanged`: eşik-tint (HpHealthy/HpWarning/HpCritical) kaldırıldı → HP fill **HER ZAMAN** `HpFillCrimson` (#C01020). `!isPulsing` guard ile pulse sırasında üzerine yazmaz.
  - Düşük-HP pulse KORUNDU; pulse base'i `HpFillCrimson` yapıldı (bar her zaman crimson, pulse beyaza shimmer).
  - `StopPulse()`: shimmer bitince renk crimson'a resetlenir (mid-lerp tint takılmasın).
  - Resource/mana barı sınıf-tint / cyan DEĞİŞMEDİ (cyan o barda kalır).
- **Data-proof:**
  - `HpFillCrimson = R:0.753 G:0.063 B:0.125 A:1` = #C01020 ✔
  - Runtime: `OnHPChanged(90/100)` (eski cyan yolu) → hpFill.color R:0.753 G:0.063 B:0.125 = crimson ✔
  - Runtime: `OnHPChanged(50/100)` (eski orange yolu) → hpFill.color crimson ✔

## FIX B — Prop collider taban-merkez  ✅ DONE
- **Dosya:** `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs`
- **Kök neden:** `ApplyFootprint` offset = `(size.x*0.5, size.y*0.5)` → kutu origin'in sağ-üstüne kaydı (alt-sol köşe origin'de); collider sprite tabanına oturmuyordu, içinden yürünüyordu.
- **Değişiklik:** offset = `(0f, box.size.y*0.5f)` → **taban-merkez**: yatay origin'de ortalı (x=0), kutunun alt kenarı taban çizgisinde (y=+halfHeight). Prop GameObject hücre merkezinde + bottom-anchored sprite ile collider artık ayak/taban footprint'inde.
- **Per-prop data:** `blocksWalkable` (EnsureCollider gate'i) + `footprintSize` (ApplyFootprint) ZATEN `PropDefinitionSO`'dan okunuyordu — ek alan gerekmedi (spec: "alan varsa kullan").
- **B-2 ölü kod (colliderShape/colliderFootprintRatio): DOKUNULMADI** — bırakıldı.
- **Data-proof:** Recompile 0 error; offset matematiği base-center'a geçti (kod doğrulandı). Mevcut `_Arena` prop spawn'u `blocksWalkable && colliderShape != None` (IsoRoomBuilder) / `blocksWalkable` (PropRuntimeSpawner) gate'lerinden geçer.

## FIX C — Reward hitbox + chasm  ✅ DONE
### #5b RewardPickup trigger radius
- **Dosya:** `Assets/Prefabs/RewardPickup.prefab`
- **Değişiklik:** CircleCollider2D `m_Radius 0.45→0.22`, `m_Offset (0,0.5)→(0,0)` (sprite merkezine ortalandı). Scale 0.55 ile world radius `0.2475→0.121` (grid-kare → elmas/item görsel boyutu).
- **Data-proof:** radius=0.22 offset=(0,0) isTrigger=True → worldRadius=0.121 ✔

### #4 Chasm (yer-yarığı)
- **Dosyalar:** `Assets/Prefabs/Obstacles/Chasm.prefab` + `Assets/Data/Props/ChasmGap.asset` (doğrulama)
- **Değişiklik (Chasm.prefab):**
  - SpriteRenderer sorting layer: **Entities (1293760285) → Decals (1200000001)**, sortingOrder 5→2 (yer-decal; #1 prop-Ysort ile tutarlı).
  - BoxCollider2D `m_Size 0.0001×0.0001 → 0.4×0.4`, offset (0,0) = **küçük merkez collider** (tam ortasına düşülmesin; kenarlar yürünür). isTrigger=True korundu → `Chasm.cs CanWalkThrough()` push-back'i sadece merkezde tetikler (gameplay koduna DOKUNULMADI).
  - `blocksWalkable=false`: prop-sistemi canonical `ChasmGap.asset` ZATEN `blocksWalkable:0` + `isFloorDecal:1` (Decals routing) → doğrulandı, değişiklik gerekmedi.
- **Data-proof:**
  - ChasmGap.asset: blocksWalkable=False, isFloorDecal=True ✔
  - Chasm.prefab: sortingLayerName=Decals, Box size=(0.4,0.4) offset=(0,0) isTrigger=True ✔

## Console durumu
- Başlangıç: 0 error / 0 warning.
- FIX A+B recompile sonrası: 0 error / 0 warning.
- Tüm data-proof + asset refresh sonrası: **0 error / 0 warning** (detailed read_console).
- Not: asset refresh sırasında bir kez "Connection closed before reading expected bytes" (geçici import domain-reload blip'i) — re-check'te köprü canlı + console temiz. Veri kaybı yok.

## DOKUNULMAYAN
- B-2 ölü kod: `PropDefinitionSO.colliderShape` / `colliderFootprintRatio` / `blocksMovement` — bırakıldı.
- `Chasm.cs` (gameplay), GATE/Boss/reward-bleed/Build-çekirdek/weaponless-anim/branching-seed — dokunulmadı.

## Kullanıcı uyarısı
- Script recompile (FIX A+B) Unity'yi kısa süre kesmiş olabilir (kullanıcı aktifse domain reload). Prefab/asset değişiklikleri editörde anında geçerli; ayrıca commit kullanıcı onayına bırakıldı.

## Değişen dosyalar
1. `Assets/Scripts/UI/HUDController.cs` (~10 satır net)
2. `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs` (~5 satır net)
3. `Assets/Prefabs/RewardPickup.prefab` (2 alan)
4. `Assets/Prefabs/Obstacles/Chasm.prefab` (5 alan: sorting + collider)
