# COUNCIL — Bugünün Stratejisi ve PixelLab Değer-Yargısı

## 1. Animasyon Yatırımı (Maliyet/Fayda) ve Mob Kararı
- **Karakter (Weaponless):** 4/4 state'in Unity'e bağlanması **YÜKSEK** önceliklidir. Ana karakter sürekli merkezdedir; akıcı bir his, projenin "tamamlanmış" algısını doğrudan artırır.
- **Mob Animasyonları: YAPMA.** Tez bir animasyon showcase'i değil. Mob'lar combat karmaşasında zaten detaylı incelenmeyecek. Ancak "Siyah-blob" görünümü (okunabilirlik sorunu) kabul edilemez; bunun yerine mob'lar için **statik (tek frame) belirgin sprite'lar** üretilip, kod üzerinden basit reaksiyonlar (squash/stretch/tint) verilmeli.

## 2. PixelLab Kredisi Harcama Önceliği (Sıralı)
Krediler bugün sıfırlanacağından, en yüksek demo/tez etkisine göre harcanmalıdır:
1. **Eksik Prop Sprite'ları (Fıçı, Tuzak, Sütun vb.):** *Gerekçe:* "Build Mode" tezinin ana tooling sunumlarından biri. Görünmez/hayalet asset'ler yerleştirmek aracın hatalı görünmesine sebep olur; güzel prop'lar tool'u parlatır.
2. **Mob Statik Sprite'ları (Black-blob çözümü):** *Gerekçe:* En büyük görsel açık. Görsel okunabilirliği sağlayıp "öğrenci işi" hissini anında siler.
3. **Karakter Weaponless Animate:** *Gerekçe:* Hazır olan 4/4 statikleri (V3/ucuz jenerasyonla) canlandırıp ana karakter hissiyatını tamamlamak.
4. **UI / İkon (Director-Skin için):** *Gerekçe:* Artan krediyle sunumun vizyon yüzü olan arayüze ufak cilalar atmak.

## 3. Bugünün Optimal İş-Sırası (1.5 Gün Kala)
1. **Build Mode Stabilizasyonu:** Arka planda devam eden fix'in bitmesi (Tez P0).
2. **PixelLab Asset Üretimi (Batch):** Kredileri bitirmek için Prop ve Mob sprite'larının üretimi.
3. **Char Unity Wire:** Hazır olan Weaponless animasyonların Unity Animator'e bağlanıp ana karakterin yürütülmesi.
4. **Director-Skin / UI:** Mimari vizyonu en iyi anlatan yönetim ekranının görselleştirilmesi (Data-Binding).

## 4. En Büyük Görsel Risk
Demo esnasında "siyah-blob" mob'lar ve Build Mode'daki hayalet-asset'ler hocayı en çok rahatsız edecek görsel açıklardır. Mimari kusursuz çalışsa bile görünmez objeler yerleştirmek tüm şovu gölgeleyebilir. Öncelik bunları kapatmak.
