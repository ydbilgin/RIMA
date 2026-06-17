# 7 — Two-Day Demo Plan

Hedef: Yeni özellik üretmek değil, çalışan sistemin amatör/debug görünmesini engellemek.

## P0 — Bitmeden demo capture alma

1. **Capture QA düzelt**
   - 09 Stats
   - 19 Character Sheet
   - 20 Skill Draft
   - 21 Run Map
2. **Boss presentation fix**
   - scale/pivot
   - shop residue cleanup
   - health bar
   - subtitle position
3. **HUD readability**
   - HP/resource büyüt
   - slot/key/cooldown okunurluğu
4. **Black blob fix**
   - eksik sprite ise gerçek sprite
   - kasıtlı karanlık obje ise rim-light + iç değer
5. **Low HP overlay**
   - edge vignette
6. **Director visual shell pass**
   - giant frame kaldır
   - top/left/right/bottom hierarchy
   - font büyüt

## P1 — Build Mode kontrollü polish

- Mevcut isometric grid açısını ve geniş çalışma alanını koru.
- Grid Low/Normal/High görünürlük seviyeleri ekle.
- Hover cell, footprint, valid/invalid sebebi ve placed-object pulse ekle.
- Yapısal layout değişikliği yapma.

## Gün 1 — Sabah

- Screenshot duplicate detector ekle
- Character Sheet / Draft / Run Map capture flow düzelt
- Boss state cleanup
- Boss scale/pivot/PPU test
- Boss health bar phase notch

## Gün 1 — Öğleden sonra

- HUD bar/slot resize
- low-HP vignette
- black blob/rim-light
- draft synergy text
- regression playtest

## Gün 2 — Sabah

- Director shell: top bar, left rail, left content, right inspector, status bar
- decorative frame inflation kaldır
- Spawn ve Stats sekmelerini yeni shared components'e geçir

## Gün 2 — Öğleden sonra

- Build grid visibility levels; **gridi room bounds içine kırpma**
- asset thumbnails + selected state
- hover cell + footprint + valid/invalid reason
- yeni screenshot seti
- 1080p demo rehearsal
- assert rerun

## Zaman kalırsa

- Pause panel scale
- Settings spacing
- Codex row height
- Merchant icon replacement

## Kesinlikle yapılmayacaklar

- yeni room type
- yeni boss skill/faz
- tam Codex architecture rewrite
- telemetry graph sistemi
- gamepad navigation overhaul
- yeni shader pipeline
- bütün UI art pack'ini sıfırdan üretme
- Director backend refactor

İki günde “her şeyi biraz düzeltmek” en hızlı şekilde hiçbir şeyi bitirmemenin kadim yöntemidir. P0 dışında kalanlar acımasızca ertelenmeli.

## Demo çıkış kriteri

- 0 console error
- Build 8/8 PASS
- Director 6/6 PASS
- screenshot state verification PASS
- boss tam görünür
- HUD 1080p'de koltuktan okunur
- hiçbir placeholder colored square boss/merchant ana sunumunda kalmaz
- Director viewport ekranın en az %55'i
