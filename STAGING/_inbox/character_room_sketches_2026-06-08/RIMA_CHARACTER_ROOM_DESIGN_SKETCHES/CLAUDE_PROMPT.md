# Claude'a Verilecek Tasarım Feedback Prompt'u

Mevcut Chamber/Character Select ekranı yanlış yöne kaymış:
- ekranda birden fazla Warblade var,
- diğer sınıflar görünmüyor,
- combat HUD/stat elemanları selection akışını kirletiyor,
- pedestal'lar çok büyük,
- scene geniş ve boş duruyor,
- asset dili tutarsız.

## Ana hedef
Character select'i klasik stat ekranı gibi değil, RIMA'nın diegetic Attunement Chamber'ı gibi kur.

## Kesin düzeltmeler

1. Her pedestal kendi class sprite/ghost'unu göstermeli.
   - Warblade her yere kopyalanmayacak.
   - Sprite eksikse class-specific silhouette placeholder kullanılacak.
   - 10 pedestal = 10 farklı class kimliği.

2. Pedestal scale küçülecek.
   - Mevcut pedestal %25-35 küçült.
   - Class sprite pedestal üstünde okunmalı.
   - Pedestal karakterden rol çalmamalı.

3. Chamber sırasında combat HUD kapalı olmalı.
   - HP bar, skill hotbar, combat stat UI görünmemeli.
   - Sadece minimum prompt + selected class strip.

4. Layout iki yay şeklinde olacak.
   - 5 class üst/arka yay
   - 5 class alt/ön yay
   - Merkezde Rift/Attunement altar
   - Exit portal arka kenarda

5. Seçili class bilgisi alt strip'te.
   - Class adı
   - 1 cümle playstyle
   - weapon silhouette
   - [G] Bürün
   - Kilitliyse Echo cost

6. Dil ve prompt standardı:
   - `[G] Bürün: Warblade`
   - `[G] Rift'e Gir`
   - `Bir Echo seç`
   - "kapı" değil "Rift portalı"

7. Asset tutarlılığı:
   - Çok sci-fi duran dev diskleri azalt veya taş/ritual hale getir.
   - Portal arch, pedestal, monolith aynı taş/cyan-rift dilinde olmalı.
   - Realistic asset + chibi karakter karışımı azaltılmalı.

## Oda tasarım ilkesi

Combat odaları:
- Normal: 18×12 - 22×14
- Elite: 20×14 - 24×15
- Reward: 12×8 - 16×10
- Boss: 26×16 - 30×18

Büyük odalar çöpe atılmayacak; prop/decal/landmark ile combat core daraltılacak.

## Yapılmayacaklar
- Full wall sistemi kurma.
- Floorları baştan üretme.
- 8 yön portal üretme.
- Entry portal object ekleme.
- Character select'e stat panel yığma.
- Her yere Warblade placeholder koyma.
