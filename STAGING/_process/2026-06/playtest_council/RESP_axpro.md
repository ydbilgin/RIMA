# 🏛️ [axpro] rima-design (Opus) — Gameplay, Feel & Systems Advisor

## A. BUG FIXES & ENDÜSTRİ MİMARİSİ (Tasarım ve Akış Düzlemi)

### B1 🔴 Reward-flow Stall (Oyuncu Psikolojisi)
- **Tasarım Sorunu:** Oyuncu zorlu bir savaştan çıkıyor, ödül bekliyor ama oyun donuyor. Bu durum roguelite "loop"unun kalbine saplanmış bir bıçaktır.
- **Endüstri (Hades):** Hades'te oda temizlendiğinde "Room Clear" ritüeli kusursuzdur. Her şey yavaşlar, müzik iner, özel bir ses efektiyle ödül dramatik bir şekilde belirir. Ödül alma eylemi asla "başarısız" olamaz. RIMA'da bu akışın kırılgan olması tasarım açısından riskli. Eğer spawn başarısız olursa, oyuncuya otomatik "Joker" (örneğin can veya para) verilmeli. Boş geçilmemeli.

### B2 🟠 Wave Boyutu & B3 Overlay Bleed
- **B2:** Hades dalgalı gelir, evet. Çünkü 2 düşman kesip kapı açmak bir "Encounter" değil, "Pürüz" hissiyatı verir. Oyuncunun bir ritme girmesi, combo yapması için yoğunluğa ihtiyacı var.
- **B3:** UI karışıklığı oyuncunun zihnindeki harita algısını kırar. Run-Map bir "karar" anıdır, Draft bir "ödül" anıdır. Bu iki duygu üst üste binmemeli.

---

## B. GAMEPLAY TASARIMI (Hissiyat ve Pacing)

- **Hades-Style Wave (Demo İçin İdeal Pacing):**
  - **Wave 1 (Isınma - %20 yoğunluk):** 2-3 standart mob. Oyuncu yeteneklerini hatırlar.
  - **Wave 2 (Tırmanış - %40 yoğunluk):** İlk wave'in %70'i öldüğünde (tempo düşmeden) spawn olmalı. 5-6 mob.
  - **Wave 3 (Tepe Noktası - %40 yoğunluk):** 2 Elite veya kalabalık sürü.
  - Toplam oda combat süresi 20-30 saniye olmalı. Şu anki 2 mob = 3 saniye combat. Bu tempo çok zayıf.
- **Boss Mob (Demo):** Boss sadece "büyük" olmamalı, "okunabilir ve korkutucu" olmalı.
  - **Scale:** Oyuncunun en az 2 katı (vuruş hissiyatı için geniş hitbox).
  - **Tehdit:** Bir boss'u boss yapan şey "kaçılması zorunlu" vuruşlardır (Telegraph). Kırmızı alan dolduğunda oyuncu "Eğer dash atmazsam canımın %30'u gidecek" gerilimini yaşamalı.
- **Mob Boyutları (Siyah Blob Sorunu):** Top-down ARPG'lerde düşmanlar siluetlerinden tanınmalıdır (Gungeon kurşunları, Dead Cells zombileri). Eğer sadece siyah bir leke görünüyorlarsa, tasarım dili bozuktur. Acilen boyutları x1.5 yapıp, içlerine kırmızı/göz rengi gibi belirgin "Focus Point" pikseller eklenmeli.

---

## C. PLAYTEST PLANI (İyi Oynanış Kriterleri)

- **"İyi Oynanış" (Demo-Ready) Kriterleri:**
  1. **Flow State:** Oda temizle -> Ödül Seç -> Harita Seç -> Yeni Oda döngüsü KESİNTİSİZ 5 kere çalışabiliyor mu? (1 kere bile takılma varsa PASS vermem).
  2. **Juice & Impact:** Düşmana vurulduğunda Hit-stop (0.05 sn donma), Flash (beyazlama) ve tatmin edici bir Particle (kan/kıvılcım) çıkıyor mu? Siyah siluet bunu öldürür.
  3. **Escalation:** İkinci odaya geçildiğinde zorluk (mob sayısı/hızı) belirgin şekilde artıyor mu?
- **Otomatik vs Manuel Test:** Combat ritmi, hit-stop hissiyatı, UI okunaklılığı ASLA otomatize edilemez. Bunları manuel OBS kaydı ile sen oynamalısın, benim ekranı/videoyu incelemem gerekir. Kod hatalarını (NullRef) otomasyona bırak.
