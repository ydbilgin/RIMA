# EXEC 2 — Asset Pack + Registry Bake Fix (CX) — DEMO BLOCKER

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the files listed (4) BLOCKED if unclear.

NLM ACCESS: RIMA design context için:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory.

## Amaç (EN KRİTİK BLOCKER — cx kendi analizinde buldu)
UnifiedMapDesigner + F2 palette'leri `RuntimeAssetRegistry.GetByTag(tag)`'ten besleniyor. `DesignerCategoryMap.RegistryTag` tam `floor/cliff/prop/portal/light` bekliyor AMA baked registry'de sadece `cliff/column/decal/door/misc/mounting/statue/tile/wall` var → **floor/object/portal/light palette'leri BOŞ**. Ayrıca yeni floor (`PixelLabFloorFlat`) + cliff (`KitB_Cliff`) + decor (`IsoKit/decor`) klasörleri bake-root'larda YOK. Bu düzeltilmeden demo map araçtan boyanamaz.

Çıktıyı `CODEX_DONE.md` → `## EXEC2 ASSETPACK REGISTRY` başlığına yaz.

## Bağlam (cx'in kendi analizinden, dosya:satır)
- `Assets/Scripts/Live/RuntimeAssetRegistry.cs` — Instance `Resources/Live/RuntimeAssetRegistry` yükler (:123-130), GUID+tag index (:155-178), GetByTag/GetByLayer (:52-101), RegistryEntry: guid/displayName/tag/layer/sprite/tile/prefab/kind (:184-212).
- `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` — `Assets/Resources/Live/RuntimeAssetRegistry.asset` yazar (:25-26), sabit root tarar (:32-40), Sprite/TileBase/GameObject bulur (:107-110), layer çözer (:145), tag'i dosya-adı keyword'üyle çözer (:148, :169-183).
- `Assets/Scripts/RoomPainter/DesignerCategoryMap.cs:59-63` — RegistryTag: Floor→"floor", Cliff→"cliff", Object→"prop", Portal→"portal", Light→"light".
- Layer map (`DesignerCategoryMap.cs:15-20`): Floor→Floor, Cliff→Cliff, Object/Portal→Props, Light→Lighting.

## YAPILACAKLAR

### Adım 1 — Baker root + tag fix (MİNİMUM, demo'yu açar)
`RuntimeAssetRegistryBaker.cs`:
1. **Bake root'larına ekle:** `Assets/Sprites/Environment/PixelLabFloorFlat`, `Assets/Sprites/Environment/KitB_Cliff`, `Assets/Sprites/Environment/IsoKit/decor`, `Assets/Sprites/Environment/PixelLabKit`. (Mevcut root listesini grep'le bul, bu 4'ünü ekle — diğerlerine dokunma.)
2. **Tag keyword resolver'ına ekle** (mevcut keyword tablosu :169-183):
   - `floor` ← dosya adı `flat_` veya `floor` veya `pl_floor` içeriyor (ama `pl_floor_solid` zaten ayrı; flat_0-15 = floor).
   - `prop` ← `decor_`, `pillar`, `brazier`, `rubble`, `banner`, `sarcophagus`, `bones`, `moss`, `rune`, `seal`, `slab`, `rift`, `debris`, `blocks`, `bricks` (IsoKit/decor + PixelLabKit objeleri).
   - `cliff` ← `cliff` (KitB_Cliff zaten cliff keyword'ü taşır, root ekleyince gelir).
   - `portal` ← `portal`, `gate` (şu an asset yoksa boş kalır, EXEC5'te imagegen gelince dolacak — keyword'ü ŞİMDİ ekle).
   - `light` ← `brazier_lit`, `light`, `lamp`, `torch` (brazier_lit hem prop hem light olabilir — light önceliği ver veya prop bırak; sen karar ver, raporda belirt).
   - ÇAKIŞMA KURALI: bir dosya birden çok keyword'e uyarsa öncelik sırası tanımla (örn. floor > cliff > light > prop > decal > misc) ve raporla.
3. PixelLabFloorFlat `flat_*` sprite'ları TileBase değil düz Sprite → registry'de `kind=Sprite`, tag=`floor`, layer=`Floor` olmalı. Eğer designer floor için TileBase bekliyorsa (kontrol et: `UnifiedMapDesigner`/`RoomDataComposer` floor'u Sprite mı Tile mı koyuyor), gerekirse import'ta Tile üretme YERINE Sprite-as-floor path'ini doğrula. BLOCKED ise işaretle.

### Adım 2 — Portable AssetPackSO (mevcut sistemle çakışmadan)
Yeni: `Assets/Scripts/RoomPainter/AssetPackSO.cs` (RIMA.RoomPainter, runtime-safe, `[CreateAssetMenu]`):
- Alanlar: `packId` (string), `displayName`, `version`, `List<Entry> entries`.
- `Entry`: `string guid` (veya sprite/tile/prefab object ref), `DesignerCategory category` (Floor/Cliff/Object/Portal/Light enum'u zaten var), `string registryTag`, `string layer`, opsiyonel `int sortingOverride`.
- AMAÇ portable: pack değiştir = başka oyun. RIMA-hardcode yok.
- **Bake köprüsü** (Editor): `AssetPackSO` → mevcut `RuntimeAssetRegistry.Entry` kayıtları üret (yeni runtime API yaratma, mevcut registry'yi besle). Baker'a "AssetPackSO'lardan da bake et" yolu ekle VEYA ayrı küçük editor action. Mevcut root-scan'i BOZMA, ek kaynak olarak ekle.
- Bu adım Adım 1'i geçersiz kılmaz — Adım 1 hızlı-fix (root+tag), Adım 2 portable yapı. İkisi birlikte çalışsın.

### Adım 3 — Re-bake + doğrula
- Baker menüsünü çalıştır (RIMA/Legacy/Live Tool/Bake Asset Registry — EXEC1 taşımış olabilir, yeni path'i kullan). Unity AÇIK olduğundan sen execute_menu_item DENEME → Opus MCP ile bake edip doğrulayacak.
- Bunun yerine: bake'in floor/prop/cliff/light tag'lerini ÜRETECEĞİNİ kod-mantığıyla kanıtla (hangi dosya hangi tag'e düşecek, say). Raporda "PixelLabFloorFlat 16 → floor, IsoKit/decor 16 → prop, KitB_Cliff 8 → cliff" gibi beklenen sonucu yaz.

## DoD
1. Baker root + tag değişiklikleri yapıldı (surgical, mevcut root'lar korundu).
2. AssetPackSO + bake köprüsü eklendi (compile-safe, RIMA.RoomPainter runtime).
3. Beklenen bake sonucu kanıtlandı (hangi klasör → hangi tag → kaç entry).
4. Compile-clean (Unity AÇIK — sen build deneme; "compile bekleniyor" de, Opus doğrular).
5. Değişen dosya listesi + BLOCKED durumları.
