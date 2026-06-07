# 06 — Code Review Checklist

## Live Flow

- [ ] Main scene active room manager kanıtlandı.
- [ ] RoomLoader live mı, legacy mi net.
- [ ] RuntimeRoomManager live mı, obsolete mı net.
- [ ] RoomTemplateSO/IsoRoomBuilder live path mi net.
- [ ] Gate/portal spawn eden method net.
- [ ] Branch choice-index path net.

## Portal

- [ ] 1 exit -> EXIT_N
- [ ] 2 exit -> EXIT_NW + EXIT_NE
- [ ] 3 exit -> EXIT_NW + EXIT_N + EXIT_NE
- [ ] N frontal
- [ ] NW angled
- [ ] NE angled flipX
- [ ] NE flipX sadece arch üzerinde
- [ ] Rune/badge/label flip edilmiyor
- [ ] Boss center-only
- [ ] Heal/Lore yok
- [ ] Entry portal object yok

## Gate

- [ ] Root scale animasyonda değişmiyor.
- [ ] Collider bounds unlock anim sırasında sabit.
- [ ] Visual child open anim oynuyor.
- [ ] OnPlayerEntered one-shot.
- [ ] Gate placeholder live görünmüyor.

## Docs

- [ ] SYSTEM_MAP stale guard var.
- [ ] GDD future/demo ayrımı var.
- [ ] CLASS_SILHOUETTE_BIBLE tech superseded.
- [ ] STYLE_BIBLE tech superseded.
- [ ] MASTER_PLAN task state için CURRENT_STATUS'a yönlendiriyor.
- [ ] AI_READER wins uyarısı var.

## Skills

- [ ] SkillDatabase.GetPool isImplemented filter uyguluyor.
- [ ] Placeholder skills offer'a düşmüyor.
- [ ] Old/canonical skill name audit yapıldı.
- [ ] Draft 3 offer garanti korunuyor.
- [ ] Secondary unlock scenario testli.

## Weapons

- [ ] Target-size vs big-canvas karar güncel.
- [ ] Grip pivot vs center pivot karar güncel.
- [ ] Brawler WeaponDatabase yok.
- [ ] Elementalist no staff.
- [ ] Gunslinger no western.
- [ ] Shadowblade dagger no baked glow.
- [ ] Ronin scabbard body/static torso attachment.

## Report

- [ ] fig01-05 clean ScreenshotMode.
- [ ] fig06 Warblade not placeholder.
- [ ] fig08 pipeline actual.
- [ ] fig13 test result honest.
- [ ] fig14 before/after honest.
- [ ] 549 test inventory vs 410 pass run ayrılmış.
