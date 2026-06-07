# 03 — Duvarsız Floating Island Stratejisi

## Ana fikir
Senin mevcut room görüntün şunu söylüyor:
- büyük combat alanı var
- cliffler havada hissi veriyor
- duvar kurmak zor
- arka plana background koyacaksın

Bu durumda en mantıklı çözüm:
**Normal odaları duvarsız floating island gibi ele almak.**

## Normal runtime room formülü
Bir oda şu katmanlardan oluşmalı:

1. Floor (zaten var)
2. Decal overlay
3. Cliff perimeter
4. 1-3 exit portal
5. 1 entry point
6. 2-5 prop
7. Void background / fog / distant islands

## Duvarsız yaklaşımın avantajları
- combat okunurluğu artar
- procedural yerleşim daha kolay olur
- asset yükü azalır
- yanlış wall birleşimlerinden kurtulursun
- havada asılı ada hissi güçlenir
- raporda bilinçli stil tercihi olarak anlatılabilir

## Arka edge ne olmalı?
3 opsiyon var:

### A. tamamen açık
- en temiz çözüm
- portal okunurluğu çok yüksek

### B. düşük parapet / trim
- arka sınırı biraz hissettirir
- duvar kadar ağır değildir

### C. broken fragments
- birkaç taş kalıntı
- yıkık keep hissi verir
- tam wall sistemine mecbur bırakmaz

En mantıklı yaklaşım:
- normal combat odalarında A veya B
- özel odalarda C veya yarı duvarlı çözüm

## Hangi odalar duvarsız olmalı?
- normal combat
- elite combat
- reward room
- branching room
- preview / transition room

## Hangi odalar daha özel çevre almalı?
- Attunement Chamber
- Boss room
- Ritual / vault room
- Lore sanctuary

## Raporda savunma cümlesi
"Most runtime rooms are intentionally presented as floating combat islands rather than enclosed masonry rooms. This preserves readability, reduces assembly complexity in procedural layouts, and reinforces the void-suspended identity of RIMA's world."
