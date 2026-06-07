# 01 — Executive Summary

## Net karar

RIMA'da şu an en tehlikeli şey "eksik sistem" değil. En tehlikeli şey:

> Aynı işi yapıyor gibi duran birden fazla eski-yeni sistem var ve hangisinin canlı path olduğu tam kanıtlanmadan yeni iş yapılabilir.

Bu, Unity projelerinde klasik çukur: bir sistem geliştirildi, başka sistem sahnede çalışıyor. Sonra herkes "ama commit var" diye birbirini teselli ediyor. Bilgisayar ise gülüyor.

## En kritik 6 bulgu

### 1. Live gate/portal path belirsizliği
Canon:
- Rift portal
- NW/N/NE slot
- 1→N, 2→NW+NE, 3→NW+N+NE

Ama `RoomSequenceData`:
- tek `gatePosition`
- tek `gateSize`

`RoomLoader`:
- `_sequence: RoomSequenceData[]`
- tek `Gate_Room{index}_Exit`
- `LoadNext()` lineer geçiş

Bu kesin bug mı?
- Hayır, canlı path başka olabilir.
Ama blocker şüphe:
- T3 portal wiring yanlış path'e bağlanabilir.

### 2. Gate root scale bug adayı
`RoomLoader` root collider sabit kalsın diyor.
`Gate.OpenAnimCoroutine()` root transform scale değiştiriyor.
Bu collider dünya boyutunu animasyon sırasında oynatabilir.

### 3. SYSTEM_MAP agent için tehlikeli derecede stale
SYSTEM_MAP hâlâ:
- RuntimeRoomManager
- physical door
- wall tile close/open
- N/S/E/W gate

anlatıyor. AI_READER bunları revoked sayıyor.

### 4. R4 / GATESLOT / PORTAL_PACK kararları birbirinin üstüne binmiş
Portal round-2 önce "1 yön + 3 slot" diyor.
Gate-slot round-3 "NW/N/NE authored slot" diyor.
R4 "2 authored angle + NE flipX" diye en güncel kararı veriyor.

Bu normal, ama belgelerde "son karar budur" guard'ı şart.

### 5. Skill canon drift riski
Skill kararlarında bazı isimler revoked gibi görünüyor.
Kodda `SkillDatabase` içinde eski isimler geçiyor gibi:
- Backstab
- Shadow Step
- Fan of Knives
- Aimed Shot
- Disengage
- Multi Shot

Bunlar bilerek compatibility ise açıklansın. Değilse draft'a eski skill çıkabilir.

### 6. Weapon canon kendi içinde pratikle çelişebilir
`01_CANON_WEAPONS` Warblade için 256→192→96 gibi büyük canvas/final boyut yaklaşımı yazıyor.
Ama son üretim mantığında target-size / grip-pivot daha güvenli konuşuldu.
Bu konu "kanon update" istiyor; yoksa Claude büyük canvas/downscale dayatır.

## P0 sırada ne yapılmalı?

1. `LIVE_FLOW_PROOF` çıkar.
2. `Gate root scale` test/fix.
3. `SYSTEM_MAP` ve eski docs stale guard.
4. `PortalSkin live binding` doğrulama.
5. `SkillDatabase canonical + isImplemented` audit.
