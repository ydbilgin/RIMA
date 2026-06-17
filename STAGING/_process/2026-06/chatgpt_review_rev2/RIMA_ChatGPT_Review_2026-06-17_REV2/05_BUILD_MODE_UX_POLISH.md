# 5 — Build Mode UX Polish

## Karar

Build Mode için **layout redesign yapılmayacak**. Mevcut isometric viewport, diamond grid, sol asset browser ve sağ tool paneli korunacak.

İlk paketteki “grid yalnız room bounds içinde görünmeli” önerisi geçersizdir. Gridin mevcut floor şeklinin dışına devam etmesi, oda genişletme ve tile painting için gereklidir.

## Neden mevcut geometri doğru

RIMA'nın authoring sistemi isometric diamond tile mantığına dayanır:

- Floor tile: 128×64 diamond
- Yerleşim: world-space isometric hücre
- Araçlar: prop placement ve floor/walkable/overlay tile painting
- Oda üretimi: mevcut walkable şeklin dışına yeni hücre eklenebilir

Bu nedenle çalışma düzlemi:

- eğik görünür,
- siyah/boş viewport alanında devam edebilir,
- mevcut oda silüetinden daha büyük olabilir,
- yalnız bitmiş floor alanıyla sınırlanmaz.

Bunlar kusur değil, aracın temel işlevinin görsel sonucudur.

## Değişmeyecekler

- Diamond grid açısı ve hücre geometrisi
- Gridin mevcut room/floor sınırı dışına uzanabilmesi
- Prop ve Tile modları
- Sol asset seçimi, orta viewport ve sağ tool ayarı yaklaşımı
- Working-copy isolation
- Undo/redo
- Tile coordinate snap
- Kaynak asset'i doğrudan mutate etmeme kuralı

## UX polish hedefleri

### 1. Grid görünürlük seviyeleri

Tek bir aşırı parlak veya aşırı soluk değer yerine üç seviye:

- `Low`: yaklaşık %14–18
- `Normal`: yaklaşık %22–28
- `High`: yaklaşık %35–42

Aktif hover hücresi her seviyede okunur kalmalı. Her 4 hücrede major line opsiyonel olabilir, fakat diamond ritmini bozacak kadar kalın olmamalı.

### 2. Mevcut floor ile genişletilebilir alanı ayır

Grid kırpılmayacak. Bunun yerine anlam farkı gösterilecek:

- mevcut floor üstündeki grid: normal değer,
- henüz floor olmayan ama düzenlenebilir hücreler: daha soluk,
- geçersiz veya edit sınırı dışı alan: hover sırasında kırmızı/çizgili feedback.

Bu ayrım, “grid neden dışarı taşıyor?” sorusunu açıklarken oda büyütme kabiliyetini korur.

### 3. Hücre ve footprint feedback

Cursor altında:

```text
Floor Brush · Layer: Walkable
Cell (2,3) · Radius 1
```

Prop için:

```text
Broken Pillar · 2×1
Cell (2,3) · VALID
```

Geçersiz örnek:

```text
Broken Pillar · 2×1
INVALID — overlaps collision
```

- Hover cell: cyan translucent fill
- Footprint: ince outline + tüm kaplanan hücreler
- Valid: yeşil outline + `VALID`
- Invalid: kırmızı outline + kısa sebep
- Sadece renge güvenilmez; metin veya ikon da bulunur

### 4. Yerleştirilen objeyi kanıtla

`11_buildmode_prop_placed.png` gibi capture'larda değişiklik açıkça görülmeli:

- yeni yerleştirilen prop 0.6–1.0 sn pulse,
- selected outline,
- sağ panelde asset adı ve cell koordinatı,
- status bar'da placed count / undo count değişimi.

### 5. Aktif tool ve layer görünürlüğü

Tile modunda şu bilgiler tek bakışta görünmeli:

- aktif tool: Paint / Erase / Fill
- aktif layer: Floor / Walkable / Overlay
- radius veya brush shape
- replace mode

Bu bilgi yalnız sağ panelde saklanmamalı; cursor yakını veya alt status bar'da kısa biçimde tekrarlanmalı.

### 6. Panel hiyerarşisi

Mevcut yapıyı yıkmadan sadeleştir:

- `PROP / TILE` ana mod seçimi korunabilir.
- Alt kategori satırı yalnız aktif moda uygun seçenekleri göstermeli.
- `PROPS / TILES / LIGHTS / DECALS` ile üst modlar aynı şeyi tekrar ediyorsa birleştirilmeli.
- Asset kartı thumbnail + isim + footprint + selected state göstermeli.
- Sağ inspector Prop ve Tile modunda aynı bölgede kalmalı; panel zıplamamalı.

### 7. Alt status bar

Her zaman görünür olabilecek kısa bilgiler:

- active asset/tool
- layer
- cell
- valid/invalid
- selected count
- undo/redo count
- `Working copy safe`
- temel hotkey'ler

## Yapılmayacaklar

- Diamond grid kare screen-space gride çevrilmeyecek.
- Grid yalnız mevcut room bounds içine kırpılmayacak.
- Siyah workspace sırf oyun ekranına benzemiyor diye kaldırılmayacak.
- Build Mode zorunlu olarak Director'ın önerilen yeni panel layout'una taşınmayacak.
- 8/8 çalışan placement çekirdeği yeniden yazılmayacak.
- Grid, floor şeklinin dışındaki yeni tile üretimini engelleyecek şekilde maskelenmeyecek.

## Demo acceptance criteria

- Tile `(2,3)` placement assert'i korunur.
- Grid isometric hücreleri net gösterir fakat prop ve floor sanatını bastırmaz.
- Mevcut floor ve düzenlenebilir dış alan birbirinden anlaşılır.
- Hover cell ve footprint açıkça görünür.
- Valid/invalid state renk + metin/ikonla ayrılır.
- Yeni yerleştirilen prop screenshot'ta açıkça seçilir.
- Undo/redo ve working-copy safe durumu görünürdür.
