# 04 — Manuel Test Checklist

Bu liste teslim build'inden önce Unity Editor'da tek tek denenmeli.

Her madde için:
- [ ] Geçti
- [ ] Kaldı
- Not:

---

## A. Build Açılış Testi

- [ ] Oyun sahnesi açılıyor.
- [ ] Console'da kırmızı error yok.
- [ ] Player spawn oluyor.
- [ ] Kamera player'ı takip ediyor.
- [ ] HUD görünüyor.
- [ ] HP bar görünüyor.
- [ ] Skill bar görünüyor.
- [ ] Warblade basic attack çalışıyor.
- [ ] Dash çalışıyor.

---

## B. Warblade Sword Render Testi

### Başlangıç

- [ ] Sword ilk spawn'da görünür.
- [ ] Sword zeminin altında değil.
- [ ] Sword gövde layer'ı ile aynı sorting layer'da ya da Entities layer'da.
- [ ] Console'da HandAnchorAttach warning/error yok.

### Hareket

- [ ] South yönünde sword doğru yerde.
- [ ] East yönünde sword doğru yerde.
- [ ] North yönünde sword body arkasında/uygun yerde.
- [ ] West yönünde sword flip/rotation doğru.
- [ ] Diagonal hareketlerde sword kopmuyor.
- [ ] Koşarken sword Floor altına düşmüyor.

### Attack

- [ ] LMB attack sırasında sword kaybolmuyor.
- [ ] Swing sırasında sword çok kararmıyor/kaybolmuyor.
- [ ] Sword slash VFX ile çakışsa bile okunuyor.
- [ ] Attack sonrası sword eski doğru pozisyona dönüyor.
- [ ] Direction change + attack spam sırasında sword bozulmuyor.

### Runtime AttachWeapon

- [ ] Eğer AttachWeapon runtime'da tekrar çağrılıyorsa sword yine doğru layer'da.
- [ ] Yeni weapon instance OrientationSync'e bağlı.
- [ ] Eski/destroyed renderer referansı kalmıyor.

---

## C. Room Loop Testi

- [ ] Odaya girince encounter başlıyor.
- [ ] Kapılar combat sırasında kapalı.
- [ ] Enemy'ler doğru spawn oluyor.
- [ ] Tüm enemy ölünce oda clear oluyor.
- [ ] Reward/draft/fragment akışı çıkıyor.
- [ ] Reward alınmadan/akış bitmeden sonraki odaya geçilemiyor.
- [ ] Door açılınca bir sonraki odaya geçiliyor.
- [ ] Demo sequence beklenen sırada ilerliyor:
  - [ ] Combat
  - [ ] Combat
  - [ ] Merchant
  - [ ] Combat
  - [ ] Boss
  - [ ] varsa PostBossCombat

---

## D. Draft Testi

- [ ] Oda clear sonrası draft açılıyor.
- [ ] 3 offer görünüyor.
- [ ] Skill seçilebiliyor.
- [ ] Passive seçilebiliyor veya fallback reward çalışıyor.
- [ ] Skill slot dolunca replace UI patlamıyor.
- [ ] UI kapanınca oyun devam ediyor.
- [ ] Draft açıkken player kontrolü gerekiyorsa duruyor/durmuyorsa bilinçli tercih.

---

## E. Boss Testi

- [ ] Boss odasına giriliyor.
- [ ] Boss spawn oluyor.
- [ ] Boss health bar görünüyor.
- [ ] Boss intro oyunu kilitlemeden çalışıyor.
- [ ] Boss telegraph'ları görünür.
- [ ] Phase 1 attack'ları çalışıyor.
- [ ] 50% phase transition çalışıyor.
- [ ] 33% / unleashed varsa çalışıyor.
- [ ] Boss ölünce console error yok.
- [ ] Boss death sonrası akış beklenen sırada ilerliyor.

---

## F. Dual-Class Testi

### Test Tuşu

- [ ] T ile class selection açılıyor. Bu debug doğrulama.
- [ ] Shift+T ile secondary reset çalışıyor.

### Gerçek Demo Akışı

- [ ] Boss ölünce T basmadan class selection açılıyor.
- [ ] DemoComplete class selection'dan önce çıkmıyor.
- [ ] Secondary class seçilebiliyor.
- [ ] Secondary seçim bir kez yapılabiliyor, spam ile tekrar açılmıyor.
- [ ] Warblade secondary slotları açılıyor.
- [ ] SkillBarUI 6 slot mantığını gösteriyor.
- [ ] DraftManager unlock draft açıyor.
- [ ] Secondary skill draft'tan gelebiliyor.
- [ ] Post-boss oda varsa secondary skill kullanılabiliyor.
- [ ] Post-boss oda yoksa unlock draft sonrası DemoComplete çıkıyor.

---

## G. Teslim Build Smoke Test

Build almadan önce:

- [ ] Editor Play Mode 3 kez üst üste çalıştı.
- [ ] Scene reload sonrası static singleton'lar patlamıyor.
- [ ] Console kırmızı error yok.
- [ ] Demo baştan sona bir kez oynandı.
- [ ] Boss öldürüldü.
- [ ] Dual-class akışı gösterildi.
- [ ] DemoComplete görüldü.
- [ ] Build alındı.
- [ ] Build exe olarak açıldı.
- [ ] Build içinde input çalışıyor.
- [ ] Build içinde UI scale bozulmadı.
- [ ] Build içinde boss sonrası akış çalıştı.

---

## Teslim Günü Kısa Demo Rotası

1. Oyunu aç.
2. Warblade hareket/dash/basic attack göster.
3. İlk combat odasını temizle.
4. Draft ekranını göster.
5. Bir skill seç.
6. Boss'a hızlı geçmek için gerekirse debug kısayolu kullan ama bunu söyleme; demo akışını bozmadan yap.
7. Boss telegraph/phase göster.
8. Boss'u öldür.
9. Secondary class seçim ekranını göster.
10. Secondary seç.
11. Unlock draft/slot genişlemesini göster.
12. Post-boss oda varsa 30 saniye oynat.
13. Demo complete göster.
