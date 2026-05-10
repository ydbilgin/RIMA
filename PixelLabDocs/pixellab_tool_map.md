# PixelLab Tool Map & Ecosystem Overview

Bu doküman, PixelLab'ın sunduğu tüm araçları, yetenekleri ve platformları özetleyen bir haritadır. Claude veya diğer AI asistanlarına projenin bağlamını ve kullanabileceğiniz PixelLab yeteneklerini aktarmak için tasarlanmıştır.

## 1. Erişim Yöntemleri (Platforms)
PixelLab araçlarına 4 farklı yoldan erişilebilir:
1. **Web App (Simple Web Creator):** Tarayıcı üzerinden hızlı üretim.
2. **Pixelorama / Aseprite Eklentileri:** Doğrudan piksel sanat editörleri içinde üretim ve inpainting yapma imkanı.
3. **API (v2):** Kendi uygulamalarınız için programlanabilir REST uç noktaları.
4. **MCP (Vibe Coding):** AI asistanlarının (Cursor, Claude) doğrudan editör içinden varlık (asset) üretebilmesini sağlayan sistem.

---

## 2. MCP (AI Asistan) Araçları
Bu araçlar arka planda asenkron çalışır (2-5 dk) ve yapay zekanın doğrudan kod yazarken varlık üretmesini sağlar. *Not: RIMA projesinde `animate_character` kullanımı animasyon kalitesi nedeniyle kısıtlanmıştır, Web App tercih edilmelidir.*

* `create_character`: 4 veya 8 yönlü sprite üretir (İnsansı veya 4 ayaklı).
* `animate_character`: Var olan karakteri canlandırır (Walk, Idle, Attack vb.).
* `create_topdown_tileset`: Birbiriyle zincirlenebilir (base_tile_id) Wang tileset'ler üretir (örn. okyanus -> kumsal -> çim).
* `create_sidescroller_tileset`: 2D Platformer oyunlar için zeminler (üstü çimenli taş bloklar vb.) üretir.
* `create_isometric_tile`: İzometrik oyunlar için nesneler veya blok zeminler oluşturur.
* `create_object` & `animate_object`: Çevre objeleri ve proplar yaratıp animasyon ekler.

---

## 3. Web & Eklenti Araçları (Tool Kategorileri)

### A. Create Image (Görsel Üretimi)
Sıfırdan veya referanslardan görsel üretmeye yarayan ana araçlar:
* **Create M-XL Image (Flux):** Yüksek kaliteli, büyük boyutlu yeni nesil üretim.
* **Create S-L Image (Pro):** Orta-Büyük boyutlarda profesyonel üretim.
* **Consistent Style (Pro):** Stili referans alarak tutarlı yeni görseller üretme.
* **Image to Pixel Art:** Normal fotoğrafları/çizimleri piksel sanata dönüştürme.
* **Pose to Image:** Bir çöp adam veya poz referansını baz alarak karakter üretme.
* **Image to Image (Depth):** Derinlik haritası kullanarak silüeti koruyup görseli değiştirme.

### B. Edit & Inpaint (Düzenleme ve Boyama)
Mevcut pikselleri düzeltme araçları:
* **Inpaint / Inpaint v3:** Görselin sadece siyahla işaretlenen kısmını (mask) yeniden çizer.
* **Edit Image / Edit Image Pro:** Görselin tamamını bir komutla (prompt) değiştirir.
* **Remove Background:** Arkaplanı temizler (Karmaşık arkaplanlar için prompt girilebilir).
* **Resize & Unzoom:** Piksel yapısını bozmadan akıllıca yeniden boyutlandırır veya upscale edilmiş pikselleri orijinal haline (unzoom) çevirir.
* **Reduce Colors:** Optimizasyon için paletteki renk sayısını azaltır.

### C. Animate (Canlandırma)
Statik görselleri hareket ettiren sistemler:
* **Animate with Text (New/Pro):** Mevcut statik görsele metin komutuyla (örn: "yürü") animasyon ekler.
* **Text to Animation (Pro):** Sıfırdan hem karakteri hem de animasyonunu tek adımda üretir.
* **Animate with Skeleton:** İskelet sistemi kurarak karmaşık custom animasyonlar (örn. özel saldırılar) üretir.
* **Interpolation (New v2):** İki keyframe (Extreme Pose) arasına akıcı geçiş kareleri çizer.
* **Animation-to-Animation:** Bir animasyonu alıp başka bir karaktere uygular (Örn. zırh değişimi veya silah değişimi).

### D. Rotate & Pose (Döndürme)
* **Rotate:** Karakteri farklı açılara (yan, arka vb.) döndürür.
* **Create 8-directional sprite (Pro):** Tek bir referanstan 8 yönlü hareket sprite sheet'i çıkarır.
* **Re-pose:** Karakteri mevcut açısından alıp tamamen farklı bir poza sokar.

### E. Map & Tiles (Harita ve Çevre)
* **Extend Map:** Mevcut bir haritayı veya zemin kaplamasını dışa doğru genişletir.
* **Create Map / Texture:** Yukarıdan görünümlü haritalar ve dokular üretir.
* **Create Tiles (Pro):** Hexagon (altıgen), Isometric veya Top-down gibi spesifik grid formatlarına uygun döşemeler oluşturur.

---

## 4. Abonelik ve Limit Çerçevesi (Kritik Metrikler)
* **Canvas Sınırları:** Araçların çoğu Free tier'da maksimum 128x128 ile 200x200 piksellik bir alanda çalışır. Tier 2 ve üzerinde bu alan 400x400'e kadar çıkabilir.
* **Özel Çözünürlükler:** `Interpolation` gibi bazı araçlar eskiden 64x64 zorunluluğuna sahipti, ancak v2 güncellemeleri ile 252x252'ye kadar destek sunmaktadır.
* **Pro/Deneysel Araçlar:** `Try-on`, `Multi-image` ve bazı karmaşık Tileset oluşturucular sadece **Tier 1** veya **Tier 2** aboneliği gerektirir.
