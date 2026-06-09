RIMA jüri demosu için 24 saatlik süre sınırını göz önünde bulundurarak hazırladığım yalın (lean) ve acımasız önceliklendirme içeren stratejik analiz raporunu **[rima_jury_demo_plan.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/brain/a9ae584e-f757-4103-a57e-9ba20df53fe8/rima_jury_demo_plan.md)** dosyası altında oluşturdum. 

Öne çıkan kritik başlıklar ve tavsiyelerim şunlardır:

### 1. 24 Saatte En Yalın Yol ve Kesilecekler
*   **Kesilecekler:** Elementalist'in karmaşık rune kombinasyonları (tek bir çalışan yeteneğe indirgenmeli), Shop'taki çoklu eşya varyasyonları (tek bir çalışan stat boost'a düşürülmeli) ve `_IsoGame` legacy sahnesi tamamen devre dışı bırakılmalıdır. 
*   **Odak:** Hedef, jürinin 3-5 dakika içinde hata almadan baştan sona oynayıp bitirebileceği stabil bir dikey dilimdir (vertical slice).

### 2. Animasyon Tuzağı (PixelLab Gated & Over-engineering)
*   Sıfırdan yürüme veya detaylı düşman animasyonları çizmek **kesin bir zaman tuzağıdır.**
*   **Minimum Cila:** Sadece **Warblade Idle + Attack** sprite animasyonu yeterlidir. Kalan her şey (hasar alma flash efektleri, boss telegraph alanları, düşman yürüme hareketleri) sprite çizmek yerine **kod-driven** (hafif tilt/bounce, scale animasyonları, renk değişimi) olarak çözülmelidir.

### 3. Boss Build-Gap
*   **Kesinlikle Fixleyin:** Editör üzerinden jüriye sunum yapmak büyük risk taşır (performans düşüşü, editör çökmesi vb.). Prefab'ı `Resources` altına taşımak maks. 10 dakika sürer. Bu fixi yapıp hemen standalone build alınmalıdır.

### 4. Playtest ve Doğrulama
*   Testlerin yeşil olması oyun hissiyatının doğru olduğunu garanti etmez. Raporda detaylandırılan **5 kritik senaryoyu** (Ölüm/Yeniden Başlama akışı, Ekonomi/Shop etkileşimi, Boss hitbox ve Telegraph kaçışı vb.) build üzerinde bizzat test edin.

### 5. Sanat Placeholder'ları
*   **Değişmeli:** Elementalist Rune Disc ve Shop Standı renkli kare olarak kalmamalıdır (en azından basit bir halka ve masa sprite'ı koyulmalı).
*   **Kalabilir:** Zemin/duvar tile'ları ve düşman sprite varyasyonları (farklı düşmanlar için sadece hue-shift/renk değişimi yeterlidir).

Detaylı zaman planı grafiğine ve tüm analiz maddelerine **[rima_jury_demo_plan.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/brain/a9ae584e-f757-4103-a57e-9ba20df53fe8/rima_jury_demo_plan.md)** raporundan ulaşabilirsiniz. Kolay gelsin!

