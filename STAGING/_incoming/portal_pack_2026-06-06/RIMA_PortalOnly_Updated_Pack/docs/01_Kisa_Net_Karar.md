# 01 — Kısa Net Karar

## Floor
- Floor zaten elinde var.
- Bu yüzden şu an floor asset üretimine dönme.
- Mevcut floor set yeterli; sadece kullanım mantığını netleştir.

## Wall
- Wall ana sistem olmayacak.
- Normal combat odalarında full duvar kurma.
- Duvar yalnızca özel odalarda veya düşük parapet / kırık çevre elemanı gibi düşünülebilir.

## Portal
- Portal bu oyunda gerçek "kapı" yerine geçecek.
- Yani oda geçişini duvara oturan kapı değil, world-space rift portal anlatacak.

## En mantıklı çözüm
### Asset üretimi açısından:
- **1 portal facing direction üret**
- aynı portalı back edge üzerinde farklı slotlara yerleştir

### Placement / graph açısından:
- 3 exit slot üret:
  - EXIT_NW
  - EXIT_N
  - EXIT_NE
- 1 entry slot:
  - ENTRY_S

## Bu ne demek?
Görsel olarak:
- oyuncu alttan gelir
- oda temizlenince üst/back edge'de 1 ila 3 portal belirir
- portallar aynı genel yönde durur
- sadece solda / ortada / sağda konumlanırlar

## Sonuç
"Kapı için kaç yön üretilmeli?" sorusunun cevabı:
- **1 gerçek yön**
- **3 farklı çıkış pozisyonu**
