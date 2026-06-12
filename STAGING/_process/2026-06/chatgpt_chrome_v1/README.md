# RIMA UI Chrome — KIT A + KIT B2

Üretim notları:
- `sheets/` içindeki `*_1024.png` dosyaları yüklenen blueprint PNG boyutlarıyla birebir aynı canvas ölçüsündedir.
- `*_512.png` dosyaları brief'teki mantıksal canvas için nearest-neighbor küçültülmüş sürümdür.
- `slices/` altında her role ayrı transparent PNG olarak kesildi.
- İç alanlar şeffaftır; ikon, yazı, HP/resource/XP fill ve aktif-state rengi koddan gelmeli.
- Kenar ortaları düz bırakıldı; köşe süsleri köşe bölgelerinde tutuldu. Bu yüzden 9-slice esnetmede orta kenarlar bozulmaz.
- Cyan kullanılmadı. Aktif slot ember yoğunluğu ile ayrıştırıldı.

Önerilen Unity kullanımı:
1. Sprite Mode: Multiple veya tekil slice PNG.
2. Mesh Type: Full Rect.
3. Filter Mode: Point (no filter), Compression: None.
4. Sprite Editor > Border: `cutlist_and_9slice.json` içindeki önerilen border değerlerini kullan.
5. Filled bar değerlerini ayrı Image olarak bu frame'in alt/üst sibling'ı yap; bu PNG'lerde dolgu yok.
