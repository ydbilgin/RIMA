# 06 — Claude'a Verilecek Feedback Prompt

Aşağıdaki görev RIMA ekran akışı zip'ine göre uygulanacak UX/görsel make-up düzeltme planıdır.

Önce `FLOW_MAP.md` oku. Mob görselleri henüz final değil; echo dummy'leri değerlendirme. Düşman sanatı üretme.

## Ana verdict

Sistem akışı çalışıyor, ama sunum hâlâ prototip gibi. En kritik sorunlar:
1. Oda boyutları fazla büyük/boş.
2. Portallar tür olarak ayırt edilmiyor.
3. Chamber karakter seçimi dev pedestal kalabalığı gibi.
4. Boss ekranı debug/prototip hissi veriyor.
5. UI dili TR/EN karışık.
6. Her geçişte oyuncuya "şimdi ne yapacağım?" net söylenmiyor.

## Yapılacaklar

### A1 — UX state prompts
Her major state için kısa banner/prompt ekle:
- Echo seç
- Rift'e Gir
- Düşmanları Temizle
- Oda Temizlendi
- Bir yetenek seç
- Bir portal seç
- Boss yaklaşıyor
- Koşu bitti

### A2 — Oda boyutu / kamera
Combat odaları çok büyük görünüyor. Kamera %10-15 yakınlaştırmayı test et. Oda fiziksel olarak büyük kalacaksa prop/decal ile combat core'u görsel olarak daralt.

Önerilen alanlar:
- Normal combat: 18×12 - 22×14
- Elite: 20×14 - 24×15
- Reward: 12×8 - 16×10
- Boss: 26×16 - 30×18

### A3 — Portal readability
Portal tipleri:
- Combat
- Elite
- Chest/Reward
- Boss

Her portal:
- rune icon
- small label/plaque
- ground rift crack
- proximity highlight

N=frontal, NW=angled, NE=angled flipX. Rune/badge/label flip edilmez.

### A4 — Chamber make-up
- Pedestal boyutunu %25-35 küçült.
- 10 pedestal iki yay diziliminde.
- Class name kırpılmayacak.
- Seçili class glow + weapon silhouette.
- `[E] Bürün: ClassName` prompt'u net.

### A5 — Boss make-up
- Yeşil debug kare kesin kaldır.
- Boss HP bar düz sarı blok olmayacak; koyu framed 9-slice.
- Boss ritual circle.
- Boss intro 1.5s: arena dim + name + bar slide.
- Boss telegraph decals.

### A6 — UI language pass
Tek dil: Türkçe demo.
Değiştir:
- Settings → Ayarlar
- Run Path → Koşu Yolu
- Skill Codex → Yetenek Kodeksi
- Main Menu → Ana Menü
- Play Again → Tekrar Oyna
- Rift Gir → Rift'e Gir
- Portal Kapıları → Rift Portalları

### A7 — Asset production
Öncelikli üret:
1. 4 portal rune 32×32
2. portal label plaque 96×24 / 128×32
3. hole rim decals
4. arrival ring VFX
5. boss HP frame
6. ground decals
7. edge filler props
8. Seal Monolith / Rift Crystal / Broken Altar
9. smaller pedestal set
10. boss ritual circle

## RED
- Floor yeniden üretme.
- Full wall sistemi kurma.
- Mob art üretme.
- 8 yön portal üretme.
- Entry portal object ekleme.
- Heal/Lore portal ekleme.
- Her oda için full background üretme.

## Acceptance criteria
- Oyuncu her karede bir sonraki amacı anlayabilir.
- Portal tipleri 1 saniyede ayırt edilir.
- Chamber'da class seçimi label kırpılmadan okunur.
- Boss ekranında debug kare yok, HP bar premium görünür.
- UI dili tekleşmiş.
- Normal odalar büyük taş boşluk gibi değil, oynanabilir combat adası gibi görünür.
