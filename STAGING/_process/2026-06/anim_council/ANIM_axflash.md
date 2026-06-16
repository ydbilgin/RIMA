# Warblade Animation Prioritization & Budget Plan (ax_flash Edition)
**Tarih:** 2026-06-15  
**Açı/Format:** State-first workflow ve 5 günlük demo kısıtları göz önüne alınarak hazırlanan bağımsız analiz.

---

## S1: Demoya Hangi Warblade Animasyonları Girmeli? (Önceliklendirme)

*   **P1 (Must - Golden-Path Videoda Görünecekler):**
    *   **Idle (Bekleme):** 12 frame. Karakterin kimliğini ve duruşunu satar.
    *   **Run (Koşma):** 8 frame. Lokomosyon temelidir, odada gezinirken görünür.
    *   **Attack_LMB Beat 1 (Temel Vuruş - Düşük Sweep):** 8 frame. Combat centerpiece'in hasar çıkarma anını tetikleyen yegane kritik animasyondur.
*   **P2 (Nice-to-Have - Vakit Kalırsa Eklenecekler):**
    *   **Hurt (Hit-react):** 6 frame (4 yön: S/E/N/W). Karakter hasar aldığında geri bildirim verir, combat'ın "stat-deaf" hissini kırar.
    *   **Dash (Atılma):** 8 frame (4 yön: S/E/N/W). Mobilite hissiyatını artırır.
*   **P3 (Post-Demo / Reuse):**
    *   **Attack_LMB Beat 2 & Beat 3:** Demo için tek vuruşluk LMB hasar testi yeterlidir, kombo zinciri gereksiz efor oluşturur.
    *   **Attack_RMB Heavy:** Görsel etkisi büyük olsa da vuruş hissi post-effect (hit-stop, screen shake) ile LMB üzerinden taklit edilebilir.
    *   **Death (Ölüm):** Demoda ölmek hedeflenmediği için post-demo'ya bırakılmalıdır.

---

## S2: Q/E/R/F Skill Animasyonları (Bespoke vs. Reuse)

*   **Karar:** **REUSE (LMB Beat 1 veya Generic Cast - Kesinlikle Bespoke Üretilmemeli).**
*   **Gerekçe:** Q/E/R/F yetenekleri `bypassStatScaling` (stat-deaf) özelliğine sahiptir. Demodaki ana tezimiz **stat -> damage lineer ölçeklenmesi** olup, bu durum sadece LMB ile gösterilmektedir. Q/E/R/F koreografiden ibarettir. 5 günlük kısıtlı sürede bu 4 skill için ayrı ayrı state üretmek ve cleanup yapmak büyük bir zaman intiharıdır. LMB vuruş animasyonu veya ufak bir renk/VFX tint ile bu yeteneklerin reuse edilmesi yeterlidir.

---

## S3: P1/P2 için State-First Planı

*   **Idle:**
    *   *State:* Mevcut base character frame'i doğrudan kullanılabilir (yeni state üretmeye gerek yok).
    *   *Workflow:* Base frame ilk kare tutularak `Animate with Text V3` ile "idle loop, heavy breathing" üretilir.
    *   *Yön:* 5 unique yön üretilir (S, SE, E, NE, N), kalan 3 yön (SW, W, NW) aynalanır.
*   **Run:**
    *   *State:* `mid-run` (koşu ortası, bacaklar açık ve dinamik poz) state'i üretilir.
    *   *Workflow:* Bu state ilk kare olarak verilip `Custom Animation V3` ile "run loop" tetiklenir.
    *   *Yön:* 5 unique yön üretilir, 3 yön aynalanır.
*   **Attack_LMB Beat 1:**
    *   *State:* `strike-windup` (kılıcı geriye çekmiş, vuruşa hazır) state'i üretilir.
    *   *Workflow:* `strike-windup` (start) ile normal `idle` (end) frame arasına interpolation uygulanarak "sword slash attack" hareketi çıkarılır.
    *   *Yön:* 5 unique yön üretilir, 3 yön aynalanır.

---

## S4: Kaba Bütçe ve 5 Günlük Gerçekçilik Analizi

*   **Minimum Set (P1):**
    *   Idle: 5 yön x 1 gen = 5 gen call (60 frame)
    *   Run: 5 yön x 1 gen = 5 gen call (40 frame)
    *   Attack_LMB Beat 1: 5 yön x 1 gen = 5 gen call (40 frame)
    *   **Toplam P1 Eforu:** 15 generation call.
*   **Risk ve Zaman Maliyeti:** PixelLab generation kredimiz (874) fazlasıyla yeterli. Asıl risk **Pixelorama cleanup** aşamasıdır. 15 animasyonun temizliği (yüz kaymaları, silahın el değiştirmesi, ayna düzeltmeleri) animasyon başına ortalama 20-30 dakika sürer. Bu da toplamda **5 ila 7.5 saatlik** bir manuel temizlik eforuna denk gelir. Bu süre 5 günlük takvimde son derece **güvenli ve uygulanabilirdir**.
*   Eğer tüm spec (40 gen call) yapılmaya çalışılsaydı, cleanup süresi 20+ saati bulacak ve demoyu riske atacaktı.

---

## TEK CÜMLE TAVSİYE
Q/E/R/F için bespoke animasyonlardan tamamen kaçının; sadece Idle, Run ve tek bir LMB Attack animasyonunu (P1 seti, toplam 15 gen call) state-first workflow ile üretip temizleyerek riski sıfırlayın.
