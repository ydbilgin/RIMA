# 00 — Neredeyiz, Ne Yapmalıyız?

## Güncel Durum

RIMA artık fikir aşamasında değil. Kod tarafında şu ana kadar yeterince sistem yazılmış durumda:

- Unity 6 LTS tabanlı 2D roguelite proje.
- RoomRunDirector ile oda akışı.
- Demo sequence: Combat → Combat → Merchant → Combat → Boss.
- Warblade oynanabilir sınıf.
- Skill draft sistemi.
- Enemy / boss akışı.
- Boss olarak Penitent Sovereign.
- PlayerClassManager üzerinde primary + secondary class sistemi.
- SkillBarUI ve DraftManager tarafında secondary class sonrası UI/draft desteği.
- ClassSelectionTrigger ile test modunda class selection açılabiliyor.

Bu yüzden artık “hangi büyük sistemi ekleyeyim?” aşaması bitti.

Teslim için ana hedef:

```text
Çalışan, anlaşılır, baştan sona oynanabilen vertical slice.
```

Yani hedef full Act 1 değil. Hedef, hocanın karşısında 5-10 dakika içinde oynatılabilecek teknik demo.

## Önceki Yorumun Güncellenmiş Hali

Önceki değerlendirmede “Act 1 vertical slice” demek doğruydu ama artık daha keskin sınırlamak gerekiyor:

### Teslim Build Hedefi

```text
5-node demo run:
Combat → Combat → Merchant → Combat → Boss
```

Buna şu eklenmeli:

```text
Boss ölümü → Secondary class seçimi → secondary unlock draft → mümkünse kısa post-boss test odası
```

Bu yoksa dual-class sistemi raporda güzel görünür ama demoda kanıtlanmaz. Kod var ama kullanıcı akışına bağlanmadıysa jüri açısından sistem yok gibi davranılır. Acı ama yazılımda görünmeyen şey var olmuyor; Schrödinger'in feature'ı gibi saçma bir durum.

## Teslim Öncesi P0 Öncelikler

### P0-1: Warblade Greatsword Render / Attach Bug

Belirti:
- Silah zeminin altında kalabiliyor.
- Silah ele düzgün oturmuyor.

Muhtemel kök neden:
- `AttachWeapon()` sonradan çağrıldığında yeni weapon instance sorting layer almıyor.
- `weaponRenderer` eski instance'a bakıyor olabilir.
- `bodyRenderer` atanmadıysa layer kopyalama atlanıyor.
- `UpdateWeaponSortOrder()` sadece order güncelliyor, layer güncellemiyor.
- `OrientationSync.handOffsets` hardcoded ve güncel 4-yön / feet pivot kararlarına göre bayat.

Bu canlı demoda direkt göze çarpar. İlk çözülmesi gereken iş bu.

### P0-2: Dual-Class Boss Sonrası Akışa Bağlı mı?

Belirti:
- PlayerClassManager'da secondary sistemi var.
- ClassSelectionTrigger test tuşuyla seçim açabiliyor.
- DraftManager secondary seçiminden sonra unlock draft açabilecek gibi.
- Ama boss death akışı doğrudan Victory / DemoComplete'e gidiyorsa sistem oynanabilir değil.

Minimum teslim fix:
- Boss ölünce `PlayerClassManager.Instance.TriggerClassSelection()` çağrılmalı.
- DemoComplete bundan önce çıkmamalı.
- Secondary seçildikten sonra unlock draft görünmeli.
- Mümkünse bir post-boss combat/test odası eklenmeli.

## Teslimde Gösterilecek Sistemler

Hocaya canlı gösterimde şu akış en güvenli:

1. Oyuncu Warblade ile başlar.
2. 1-2 combat oda temizler.
3. Oda sonunda draft/ödül sistemi görünür.
4. Merchant ya da ara oda görünür.
5. Boss odasına girilir.
6. Boss telegraph ve faz geçişleri gösterilir.
7. Boss ölür.
8. Secondary class seçim ekranı açılır.
9. Secondary class seçilir.
10. Skill bar 6 slota genişlemiş / secondary skill draft açılmış görünür.
11. Mümkünse kısa post-boss odada secondary skill kullanılır.

## Yapılmayacaklar

Teslimden önce şunlara girilmemeli:

- 10 class'ın tamamını oynanabilir yapma.
- Full dual-class matrix.
- Full item/legendary system.
- Meta progression.
- Act 2/3.
- Yeni enemy roster büyütme.
- Büyük görsel overhaul.
- Yeni map editor sistemi yazma.
- Yeni mechanic ekleme.

Bu aşamada yeni sistem eklemek, batan gemiye balkon yapmak gibi olur. Çalışan sistemi parlat.
