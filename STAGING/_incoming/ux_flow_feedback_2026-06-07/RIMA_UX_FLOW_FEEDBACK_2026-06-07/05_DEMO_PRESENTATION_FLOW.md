# 05 — Jüriye Gösterilecek En İyi Demo Akışı

19 karelik akış tüm sistemi gösteriyor ama canlı sunumda hepsini eşit göstermeyin. İnsan dikkati sınırlı, jüri dikkati daha da sınırlı; çünkü kahve genelde kötü oluyor.

## Önerilen 7-8 dakikalık sunum akışı

### 1. Main Menu — 10 saniye
Amaç: mood ve isim.
Göster:
- Menü background
- Başla

Söyle:
"RIMA, her koşuda bir Echo'ya bürünerek odalarda ilerlediğiniz 2D roguelite prototipi."

### 2. Chamber — 45 saniye
Amaç: sınıf seçimi diegetic olarak gösterilsin.
Göster:
- Chamber wide
- bir class'a yaklaş
- `[E] Bürün`
- attune VFX
- Rift'e gir

Söyle:
"Karakter seçimi ayrı menü değil; oyun dünyasındaki Attunement Chamber içinde yapılıyor."

### 3. Run spawn + combat — 90 saniye
Amaç: ana oynanış.
Göster:
- oda spawn
- 2-3 düşman
- light/heavy hit
- Broken/Sundered
- `[RMB] İnfaz`

Dikkat:
Mob placeholder ise açıkça söyle:
"Bu karelerde düşmanlar placeholder; sistem ve UX akışı test ediliyor."

### 4. Room clear + Draft — 75 saniye
Amaç: roguelite build kararı.
Göster:
- Oda temizlendi
- ödül al
- 3 kart
- bir kart seç

Söyle:
"Her oda sonrası draft sistemi oyuncunun build yönünü değiştiriyor."

### 5. Portal choice + Map overlay — 60 saniye
Amaç: run rota seçimi.
Göster:
- 2/3 portal
- map overlay

Söyle:
"Portallar oda türünü temsil ediyor; map overlay koşu rotasını gösteriyor."

### 6. Boss — 60 saniye
Amaç: demo hedefi.
Göster:
- boss intro
- boss HP
- telegraph
- birkaç saldırı

Not:
Boss placeholder ise burada çok uzun kalma. Boss görseli zayıfsa sadece HP/telegraph sistemini göster.

### 7. Victory veya Death — 30 saniye
Amaç: loop kapanışı.
Göster:
- Demo Complete veya Death
- Echo breakdown
- Main menu dönüşü

Söyle:
"Koşu bitince Echo kazanımıyla tekrar Chamber'a dönülüyor."

## Sunumda açmaman daha iyi olan ekranlar

### Settings
Sadece sorarlarsa göster. Akışta tempo öldürür.

### Skill Codex
Şu an çok tablo gibi. Jüriye "bakın çok sistem var" demek için açarsan ters teper. UI cilalanmadan gösterme.

### Character Sheet
Şu an fazla boş ve placeholder. Sunumda açma veya 2 saniye "devam eden ekran" diye geç.

## Gösterim sırası

```text
Main Menu
→ Chamber
→ Attune
→ Rift'e Gir
→ Compact Combat Room
→ Execute
→ Room Clear
→ Draft
→ Portal Choice
→ Map Overlay kısa
→ Boss
→ Victory
→ Main Menu dönüş
```

## Sunum için kesin temizlenecek şeyler

- 16_boss_room yeşil debug kare
- Victory ekranındaki dev sarı panel
- Portal tip label/rune eksikliği
- Chamber label kırpılması
- TR/EN karışımı
- Draft card text küçük/lorem hissi
