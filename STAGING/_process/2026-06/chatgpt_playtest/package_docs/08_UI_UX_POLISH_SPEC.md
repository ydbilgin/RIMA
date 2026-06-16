# RIMA UI/UX Visual Polish Spec

## Hedef

UI, mevcut yüzen taş oda ve rift portalı üzerinde sonradan yapıştırılmış web paneli gibi değil, aynı dünyadan oyulmuş bir oyun sistemi gibi görünmeli.

## Malzeme dili

- Ana gövde: isli slate/obsidian taş
- Bağlantı: aşınmış koyu demir ve çok kontrollü bronz/amber
- Aktif enerji: cyan rift crack
- Selection/focus: amber işaret + cyan enerji, ikisi aynı anda bağırmayacak
- Tehlike: koyu kırmızı
- Metin: kemik beyazı; secondary soğuk gri

## HUD

- Vitality sol alt.
- HP ve class resource büyük, saniyenin onda birinde okunur.
- LMB/RMB alt merkezde en büyük iki slot.
- Q/E/R/F/V ikincil boyutta.
- Minimap sağ üst; oyun alanını kapatmayacak.
- Oda adı girişte 2-3 saniye görünür, sonra kaybolur.
- Currency düşük öncelikli.

### 1920×1080 başlangıç ölçüsü

- Vitality cluster: 360–420 × 140–190 px
- HP fill: 270–320 × 22–28 px
- Resource: 230–300 × 15–20 px
- LMB/RMB: 82–92 px
- Skill slots: 64–76 px
- Minimap: 220–280 px
- Ana font: 16–22 px eşdeğer okunurluk

## Reward screen

- 3 kart, merkezde; arka plan karartılır ama oda görünür.
- Kart 360–420 × 540–640 px.
- Header/rarity, ikon, başlık, description, combo box, select button ayrı bölgeler.
- Combo box sabit minimum genişlikte.
- Seçili kart hafif büyüyebilir veya glow alır; diğer kartlar tamamen görünmez olmaz.
- “Eşleşir” yerine `KOMBO AÇAR` + trigger/outcome.

## Pause

- 560–680 px merkez panel.
- Güçlü focus state.
- `Devam Et, Ayarlar, Yetenek Kodeksi, Koşu Özeti, Ana Menü, Oyundan Çık`.
- Main Menu/Exit onay ister.

## Settings

- 1200–1500 px merkez genişliği.
- Sol kategori rail: Oynanış, Görüntü, Ses, Erişilebilirlik, Kontroller.
- Sağ içerik scroll olabilir; footer sabit.
- UI Scale 80–150, camera shake, hit-stop intensity, damage numbers, low-health FX, chromatic aberration, VFX intensity.

## Codex

- Sol: class rail.
- Orta: skill list/filter.
- Sağ: detail.
- Detail: ikon, rol, cooldown, resource, effect, produced/consumed states, combo trigger/outcome, boss rule, upgrades.
- Skill satırları 48–64 px yükseklik; 1080p'de mikroskobik metin yok.

## Canvas

```text
Canvas Scaler: Scale With Screen Size
Reference: 1920x1080
Match: 0.5
Safe Area root
User UI Scale: ayrı multiplier
```

3440×1440 ultrawide'da HUD ekranın fiziksel köşelerine kaçmasın; safe-area ve max-width anchors kullan.
