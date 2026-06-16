# Görsel Polish Ekranları — Ne Anlatıyor?

## VP-01 Gameplay HUD

Referans: `GENERATED_POLISH_SCREENS/VP-01_GAMEPLAY_HUD_POLISH.png`

- Mevcut room geometry ve portal sahnesi korunuyor.
- HP/resource sol altta okunabilir cluster.
- LMB/RMB görsel önceliği yüksek.
- Oda başlığı transient olmalı; görselde sürekli görünmesi zorunlu değil.
- Minimap frame modüler 9-slice olmalı.

## VP-02 Reward Selection

- Kartlar gerçek oyun ekranı üzerinde modal.
- Seçilen kart cyan/energy state alıyor.
- Bu görseldeki skill metinlerini canonical kabul etme.
- Sinerji kutusunun ayrı, geniş ve kısa olması ana layout kararıdır.

## VP-03 Pause

- Koyu overlay oyunu hâlâ gösteriyor.
- Panel edge/corner parçaları tekrar kullanılabilir.
- Focus state yalnız border değil; fill, glow ve marker birleşimi.

## VP-04 Settings

- Sol kategori rail ve sağ içerik ayrımı.
- Toggle/slider/keybind aynı kitin parçaları.
- Footer butonları viewport'tan bağımsız sabit.

## VP-05 Codex

- Üç bilgi seviyesi aynı ekranda.
- Üst class tab bar görsel alternatiftir; sol class rail zaten varsa ikisi aynı anda zorunlu değil. Claude sadeleştirerek birini seçebilir.
- Text/data görselden kopyalanmaz.

## VP-06 Modular Asset Showcase

- Nihai kesilecek assetlerin kategorisini gösterir.
- Görseldeki bileşenleri tek büyük bitmap olarak kullanma.
- Her panel/button/slot/state ayrı PNG veya atlas sprite olmalı.
