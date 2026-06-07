# 07 — Evidence Notes

Bu dosya Claude'a hangi kanıtın nereden geldiğini gösterir.

## AI_READER facts
- Oda doktrini: yüzen ada + arka kenarda Rift portalları.
- Fiziksel kapı yok.
- Heal/Lore portal yok.
- 2 authored angle: N frontal, NW angled, NE runtime flipX.
- Canvas/PPU: 120x120, PPU64, Point.
- 10 class data model, demo 4 playable.
- 111 skill, 67 implemented, placeholders filtered.

## CURRENT_STATUS facts
- T3.0 gate-slot DONE deniyor.
- R4 açısı: N frontal + NW angled + NE flipX.
- UI↔JSON done.
- Walkable enforcement done.
- T1/T2 done.
- T3 portal wiring contact sheet onayına bağlı durmuş.
- Fig01-05 kullanıcı göz kontrolü/gating devam ediyor gibi.

## Gate-slot facts
- 1 exit -> N
- 2 exit -> NW + NE
- 3 exit -> NW + N + NE
- ENTRY = playerSpawn, physical entry object yok.
- Sıfır schema change: `door_NW_01`, `door_N_01`, `door_NE_01`.

## R4 facts
- SO canonical + JSON mirror.
- exitSlots JSON v2.
- PortalSkin frameFrontal/frameAngled.
- NE runtime flipX.

## Code facts
- RoomSequenceData tek gatePosition/gateSize taşıyor.
- RoomLoader tek Gate_Room exit oluşturuyor gibi.
- RoomLoader comments root collider unscaled kalmalı diyor.
- Gate.OpenAnimCoroutine root scale değiştiriyor.
- RuntimeRoomManager obsolete ama büyük eski door lifecycle taşıyor.
- SkillOfferGenerator fallback allSkills path isImplemented filter uyguluyor; SkillDatabase path doğrulanmalı.

## Weapon facts
- 1 sınıf = 1 silah = 1 silüet locked.
- Elementalist staff yasak.
- Gunslinger western yasak.
- Shadowblade embedded glow yasak.
- Brawler silah yok.
- Weapon table big-canvas/final-size kararları canlı pratikle tekrar doğrulanmalı.
