# Regresyon Test Planı

## Reward/session
- [ ] Reward seçmeden kapıya gidince kapı kilitli veya açık skip aksiyonu görünür.
- [ ] Bir seçenek claim edilince kardeşler temizlenir.
- [ ] 10 ardışık odada stale reward yok.
- [ ] Aynı anda yalnız bir active reward session.
- [ ] `G` doğru target ve doğru room/session açar.
- [ ] Pause -> Resume sonrası interaction çalışır.
- [ ] Scene reload/run restart pending session bırakmaz.
- [ ] Reward iki kez apply edilemez.
- [ ] Egg hatch sırasında input spam duplicate reward üretmez.

## Aim/facing
- [ ] Sola yürürken cursor sağa: body+weapon aynı frame sağa.
- [ ] Idle iken dört yön cursor body'yi günceller.
- [ ] Attack ortasında cursor döndürme hit geometry'yi ayırmaz.
- [ ] Projectile cursor yönüne.
- [ ] Cone/cleave cursor yönüne.
- [ ] Ground target cursor world point'e.
- [ ] Dash attack tanımında hangi yön kullanıldığı explicit.
- [ ] Controller right-stick aynı davranış.

## UI/layout
- [ ] 1920×1080: 80/100/125/150 UI scale.
- [ ] 2560×1440.
- [ ] 3440×1440 ultrawide.
- [ ] Türkçe/İngilizce uzun skill adları.
- [ ] Combo box min width korunur.
- [ ] TMP harf harf dikey sarılmaz.
- [ ] Controller/keyboard focus her modalda görünür.
- [ ] Pause menüsü reward modalı üstünde input çakışması oluşturmaz.

## Lifecycle
- [ ] Play mode 20 enter/exit, warning yok.
- [ ] Scene 20 change, duplicate controller yok.
- [ ] Domain Reload açık/kapalı.
- [ ] Build placement active/inactive unload.
- [ ] Controller count 0/1.

## Visual/accessibility
- [ ] Rarity yalnız renk ile anlatılmıyor; shape/icon/label var.
- [ ] Low health effect kapatılabilir.
- [ ] Camera shake ve hit-stop intensity ayarlanabilir.
- [ ] UI %150'de clipping yok.
- [ ] World reward focus, color-blind modda okunur.
