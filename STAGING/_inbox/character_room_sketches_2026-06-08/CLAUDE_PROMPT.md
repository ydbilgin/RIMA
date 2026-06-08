# Claude'a Verilecek Ana Prompt — Chamber + Room + Interaction Prompt Fix

RIMA'da mevcut Character Select / Chamber ekranı ve interaction prompt sistemi için düzeltme yap.

## Mevcut gözlenen sorunlar

1. Chamber'da birden fazla Warblade görünüyor.
   - Her pedestal aynı class gibi duruyor.
   - Diğer sınıflar görünmüyor.
   - Bu kırmızı alarm: character select kimliği bozuk.

2. Combat HUD / stat UI selection ekranında görünüyor.
   - HP bar, skill hotbar, stat gibi şeyler Chamber'da kapalı olmalı.
   - Character Select ekranında oyuncuya combat UI değil class kimliği gösterilir.

3. Pedestal'lar fazla büyük.
   - Karakterden ve class kimliğinden rol çalıyor.
   - %25-35 küçült.

4. Oda gereksiz geniş ve boş.
   - Scene view'da dev taş halı gibi duruyor.
   - Seçim alanı merkezde sıkılaşmalı.

5. Asset dili karışık.
   - Sci-fi disk, realistic portal, chibi karakter, dev prop parçaları aynı görsel ailede değil.
   - Dil: taş + cyan rift + kırık ritüel.

6. Interaction prompt bug'ı var.
   - `[G] [G] Rift'e Gir` gibi duplicate key metni oluşabiliyor.
   - Bu test otomasyonu ile yakalanmalı.

---

## A) Chamber / Character Select düzeltmeleri

### A1 — Her pedestal farklı class kimliği gösterecek

Kural:
- 10 pedestal = 10 class.
- Her pedestal kendi class sprite/ghost/silhouette kullanır.
- Sprite yoksa generic Warblade kopyalama YASAK.
- Geçici çözüm: class-specific silhouette placeholder.

Acceptance:
- Ekranda aynı Warblade sprite'ı 10 kez görünmez.
- En azından 10 farklı silhouette/placeholder görünür.

### A2 — Chamber sırasında combat HUD kapalı

Selection sırasında kapat:
- HP bar
- skill hotbar
- combat stat UI
- boss/combat overlay
- unnecessary debug gizmo/marker

Sadece göster:
- minimal interaction prompt
- selected class strip
- Echo balance gerekiyorsa küçük ve sade

Acceptance:
- Character select screenshot'ında HP/hotbar görünmez.

### A3 — Pedestal scale ve layout

- Pedestal scale: mevcut boyuttan %25-35 küçük.
- Layout: 5+5 iki yay.
- Merkezde Attunement altar.
- Arka kenarda Rift exit.
- Seçili class glow/ring ile vurgulanır.

Acceptance:
- Pedestal karakterden büyük rol çalmaz.
- Class label kırpılmaz.
- Seçili class 1 saniyede anlaşılır.

### A4 — Selected class alt strip

Alt strip içeriği:
- Class name
- weapon silhouette
- 1 cümle playstyle
- `[G] Bürün`
- locked ise Echo cost

Örnek:
```text
WARBLADE — ağır kılıç, yakın dövüş, Broken/Sundered infaz
[G] Bürün
```

---

## B) Oda tasarım ilkesi

Önerilen walkable boyutlar:

```text
Normal Combat: 18×12 - 22×14
Elite:         20×14 - 24×15
Reward:        12×8 - 16×10
Boss:          26×16 - 30×18
```

Büyük odalar çöpe atılmayacak. Kenarlara prop/decal/landmark konarak combat core görsel olarak daraltılacak.

Yap:
- edge rubble
- rift shard
- broken pillar
- ground crack decal
- hole rim
- portal ground glow

Yapma:
- full wall sistemi
- floorları baştan üretme
- 8 yön portal
- entry portal object

---

## C) Interaction prompt standardı

Ana karar:
Localization stringleri tuş içermez. Tuşu sadece tek sistem ekler.

Yanlış:
```text
Loc: "[G] Rift'e Gir"
Presenter: "[G] " + Loc
Sonuç: "[G] [G] Rift'e Gir"
```

Doğru:
```text
Loc: "Rift'e Gir"
Formatter: "[G] Rift'e Gir"
```

Final prompt formatı:
```text
[KEY] Aksiyon
```

Örnekler:
```text
[G] Bürün: Warblade
[G] Rift'e Gir
[G] Ödülü Al
[RMB] İnfaz
[M] Harita
[TAB] Karakter
```

---

## D) Test otomasyonu ekle

Yeni dosyalar önerisi:

```text
Assets/Scripts/UI/InteractionPromptFormatter.cs
Assets/Tests/EditMode/UI/InteractionPromptFormatterTests.cs
Assets/Tests/EditMode/UI/UITextLintTests.cs
Assets/Tests/PlayMode/UI/ChamberPromptTests.cs
```

Test hedefleri:
- Duplicate key yok.
- Localization key token taşımaz.
- Chamber pedestal prompt tek `[G]` içerir.
- Rift exit prompt tek `[G]` içerir.
- TR/EN toggle duplicate üretmez.
- `[G] [G]`, `G G`, `[E] [E]`, `[RMB] [RMB]` yakalanır.

---

## RED LIST

- Büyük UI rewrite yapma.
- Input sistemi baştan yazma.
- ChamberSelectBootstrap içine yeni string concat çamuru ekleme.
- Her ekranda ayrı prompt formatlama yapma.
- Warblade'i her pedestal için fallback kullanma.
- Combat HUD'ı Character Select'te gösterme.

İlk commit önerisi:
1. `ui/interaction-prompt-formatter-tests`
2. `chamber/class-silhouette-binding`
3. `chamber/hide-combat-hud`
4. `chamber/layout-scale-pass`
