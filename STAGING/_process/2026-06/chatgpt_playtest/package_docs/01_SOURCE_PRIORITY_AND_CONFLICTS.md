# Kaynak Önceliği ve Çelişki Kuralları

RIMA belgelerinde geçmiş kararlar ve override edilmiş maddeler var. Claude bir eski belgeyi görüp sistemi geriye götürmesin diye öncelik sırası aşağıdadır.

## Öncelik sırası

1. `MASTER_KARAR_BELGESI.md`
2. `SINIF_VE_SKILL_KARAR_BELGESI.md`
3. `CLASS_STATE_CONTRACT.md` ve `GLOBAL_REPEAT_RULES.md`
4. `ROOM_MECHANICS.md`, `MAP_ITEM_SYSTEM.md`, `ITEM_BUILD_MATRIX.md`
5. `STYLE_BIBLE.md`, `ROOM_DESIGN_PHILOSOPHY.md`, `COMBAT_ROSTER.md`, `BOSS_DESIGN.md`
6. Bu paketteki playtest raporları ve yeni UI polish kararları
7. `HUD_DESIGN_SPEC.md` yalnız yerleşim fikri olarak; mevcut piksel ölçüleri playtestte yetersiz kaldı.
8. `VISUAL_QUALITY_STANDARDS.md` içindeki combat-feedback ilkeleri kullanılabilir fakat walk/frame sayıları güncel Master kararlarıyla kontrol edilmelidir.
9. `GDD.md` lore/elevator pitch içindir; dosya kendisini stale olarak işaretliyor.

## Bu paket için kilit yorumlar

- Kamera ve yön sistemi: 35° ARPG görünüm, S/E/N/W cardinal yön. Eski 8-yön ve aşırı overhead kararlarını kullanma.
- Asimetrik karakter ve silahlar flip edilmemeli; cardinal yönler ayrı sprite/clip olarak ele alınmalı.
- `Walk` yerine `Run` kullanılmalı.
- UI placement fikri korunabilir: vitality sol alt, skill bar alt merkez, minimap sağ üst. Ancak mevcut 72px gibi aşırı küçük HUD ölçüleri 1080p/1440p için büyütülmeli.
- Reward lifecycle, oda ilerlemesinin atomik parçasıdır. Ödül tamamlanmadan kapı açılmamalı veya açık bir `Reddet/Atla` işlemi bulunmalıdır.
