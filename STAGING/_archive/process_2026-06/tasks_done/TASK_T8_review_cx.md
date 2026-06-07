ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: not needed for this review.
Direct-read only: code / STAGING / asset files listed below.

# Amaç
REVIEW-ONLY (kod değişikliği YAPMA): başka bir agent'ın T8 işini (checker zemin + 15 Generated odaya prop auto-placement) diff üzerinden denetle.

# Değişen dosyalar (uncommitted, working tree'de)
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (floorTileAlt + ((x+y)&1) checker; null-safe fallback bekleniyor)
- `Assets/Scripts/Editor/Map/PopulateGeneratedPropsMenu.cs` (YENİ: menu utility, CompositionRoleMapGenerator + BridsonPoissonAutoPlacer, density 0.55, Undo/SetDirty/SaveAssets)
- `Assets/Scenes/_Arena.unity` (floorTileAlt = floor451_1 ataması — sadece bu fark olmalı!)
- `Assets/Data/Rooms/Generated/*.asset` (15 SO, props yazıldı)

# Denetim soruları
1. IsoRoomBuilder diff'i: null-safe mı (floorTileAlt atanmadıysa eski davranış AYNEN)? Mevcut overlay/cliff path'lerine yan etki var mı?
2. _Arena.unity diff'i SADECE floorTileAlt referansı mı? (git diff ile bak — başka sahne değişikliği = RED FLAG)
3. Editor utility: Undo/SetDirty/SaveAssets doğru mu; tekrar çalıştırılırsa props ÇOĞALIR mı (idempotency)? Çoğalıyorsa not düş.
4. Prop yoğunluğu dağılımı: oda başına 4-90 prop (avg 37; elite_trident=90, hourglass=65, lshape=61). 2-3 SO'nun YAML'ına bak: prop'lar oynanabilir alanı (merkez/kapı önü) bloke ediyor mu — CompositionRoleMap clean-center pass'i gerçekten korunmuş mu? 90 prop bir odada makul mü; değilse önerilen density/cap değeri ver.
5. PropPlacementData alanları (flipX, position, prop referansı) IsoRoomBuilder.BuildProps'un beklediği formatta mı?

# Çıktı
`STAGING/_review_T8_checker_props_cx.md`: verdict PASS / PASS-WITH-NOTES / FAIL + numaralı bulgular file:line kanıtlı + (gerekirse) önerilen density düzeltmesi. Kod DEĞİŞTİRME, commit YAPMA.
