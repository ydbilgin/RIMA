İşte Gemini 3.1 Pro (High) perspektifinden, derin mimari ve risk öncelikli 24 saatlik jüri demosu stratejisi:

### 1. 24 Saati Nasıl Bölmeli? (En Az Riskli Sıralama)
Jüri demolarında altın kural şudur: **Kötü görünen ama çalışan bir oyun zayıf not alır; ama kırılan/çöken bir oyun SIFIR alır.**

**En Düşük Riskli Sıralama:**
1. **(d) Build-Gap (Hemen Şimdi - 15 dk):** Boss prefab'ını `Resources` klasörüne taşı (veya Addressables'a ekle). Standalone build'in önündeki tek mimari blokajı derhal kaldır.
2. **(a) Playtest + Hardening (Bölüm 1 - 2 Saat):** Kod tamamlandı ama insan eli değmedi. Senaryoyu baştan sona (Warblade ile) 3 kez oyna. Garip buton basışları, UI transition anlarında spamlemeler yap.
3. **(b) Animasyon Üretimi (Ana Blok - 6-8 Saat):** Oyun "kırılmaz" olduğunu kanıtladıktan sonra görsel cilaya (user-present) girilir.
4. **(c) Sanat Polish (Zaman Kalırsa - 2 Saat):** Placeholer'ları "bilinçli soyut" hale getirme (aşağıda detaylandırdım).
5. **(a) Playtest + Hardening (Bölüm 2 - Build Dondurma - 2 Saat):** Sadece standalone build üzerinden final test.

### 2. Animasyon Stratejisi: 24h Gerçekçiliği ve Yüksek ROI
2 sınıf + Boss + Moblar için tam set animasyon **24 saat için ölümcül bir scope creep (kapsam kayması) riskidir.** Entegrasyonu (Animator state machine, event'ler, transition bug'ları) üretimi kadar zaman alır.

**Strateji:** "Warblade Tam Odak + Boss Keyframeleri + Elementalist/Moblar Kod-Driven (Tweening)"
Jüriye Warblade'i seçtirip ana demoyu onunla yapmalısın. Elementalist "bakın başka sınıflarımız da var" demek için kalsın. 

**En Yüksek Demo Değeri (ROI) Veren 6 Animasyon:**
1. **Warblade Attack (Slash):** En çok görülecek eylem. Vuruş hissi (anticipation ve follow-through) mükemmel olmalı.
2. **Warblade Idle:** Karakterin "yaşadığını" gösterir.
3. **Warblade Run:** Oyunun %50'si yürüme.
4. **Boss Telegraph (Saldırı Öncesi):** Mimari olarak telegraph wire var demiştin, bunu net bir animasyonla desteklemek Boss savaşını "adil" ve okunabilir hissettirir.
5. **Mob Death (Ölüm):** Düşman öldürmek tatmin edici olmalı (kan sıçraması, parçalanma veya etkileyici bir yok oluş).
6. **Boss Death:** Victory ekranına girmeden önceki doruk noktası.

### 3. Boss Prefab Build-Gap: Editör mü Standalone mu?
**Kesinlikle Standalone Build.** Editör üzerinden demo yapmak jüri önünde amatörce durur ve devasa mimari riskler taşır (yanlışlıkla bir şeye tıklama, arkada derleme başlaması, editör performans dropları, console log yığılması). 
Build-gap 1 satırlık bir fix (prefab'ı Resources içine almak ve `Resources.Load` veya Addressables kullanmak). Bu fix'i ertelemek, "demo anında çalışmazsa" riskini 24 saat boyunca taşımak demektir. **İlk yapacağın iş build almak olsun.**

### 4. En Büyük "Jüri Önünde Kırılır" Riski ve De-Risk
**Risk:** Sahne geçişleri, UI State'leri (özellikle Pause Menu ve Shop etkileşimi) veya Draft Timeout sırasında oyuncunun beklenmedik kombinasyonlar yapması (Örn: Shop'ta item alırken tam o frame'de ESC'ye basmak).

**De-Risk Stratejisi (Mimari Sigorta):** 
Görünmez bir **"Panic Button" (örn: F12 tuşu)** implemente et. Bu tuş:
- Oyunu zorla unpause eder (`Time.timeScale = 1`).
- Mevcut tüm UI ekranlarını kapatır.
- Veya doğrudan bir sonraki "Combat" sahnesine force-load yapar.
Jüri önünde oyun softlock olursa, "evet burada bir boss phase'e geçiyoruz" diyerek çaktırmadan F12'ye basıp demoyu kurtarabilirsin.

### 5. Sanat Placeholder ve Görsel Tutarlılık
Jüri placeholder'ları anlar ve tolere eder, **ANCAK tutarsızlığı tolere etmez.** Mükemmel anime edilmiş bir Warblade ile Unity'nin default gri/beyaz materyalli küpleri yan yana gelirse oyun "bozuk" görünür.

**Minimum Tutarlılık Barı:**
Eğer Elementalist rune disc veya Shop standları kare/daire kalacaksa, onları **bilinçli (intentional)** göster. 
- Unity default renklerini kullanma. Oyunun paletine uygun (örn: neon mavi, koyu mor) renkler ver.
- Basit bir script ile o karelere yavaşça yukarı-aşağı süzülme (sine wave bobbing) ve kendi etrafında dönme ekle. 
Böylece jüri "ha, bu objenin modeli bitmemiş ama yazılımcı buraya bir sistem kurmuş" der, "bu obje bug'da kalmış" demez.

### 6. Saat-Saat 24h Programı (Sıfır Noktası: Şimdi)
*(14-16 saatlik aktif çalışma varsayımıyla)*

*   **T-24 : T-23 (1 Saat): Tesisat ve İlk Kan**
    *   Boss build-gap fix. (Prefab'ı taşı).
    *   İlk Standalone Build'i al.
    *   Build üzerinden acımasızca 3 tam tur playtest yap. Hataları not al, çözme.
*   **T-23 : T-21 (2 Saat): Hardening**
    *   Playtest'te çıkan bug'ları ez. 
    *   Gizli "Panic Button"ı (F12) kodla ve test et.
*   **T-21 : T-16 (5 Saat): Animasyon Jam (User-Present Blok - KRİTİK)**
    *   Sadece hedeflenen 6 animasyona odaklan.
    *   Kusursuzluk arama, "akıcılık ve karakter" ara.
*   **T-16 : T-14 (2 Saat): Animasyon Entegrasyonu**
    *   Animator Controller'ların bağlanması.
    *   Koddan trigger'ların (Attack, Hurt) tetiklenmesi ve frame-perfect hit-box ayarları.
*   **T-14 : T-06 (8 Saat): UYKU VE BEYİN DİNLENDİRME**
    *   *Bu es geçilemez. Uykusuz bir zihin demoda jürinin sorularına saçmalayabilir.*
*   **T-06 : T-04 (2 Saat): Görsel Tutarlılık (Polish)**
    *   Placeholder'ları renklendir, kod-driven basit rotasyon/süzülme (bobbing) ekle.
    *   Kılıç vuruşlarına basit partiküller veya kamera sarsıntısı (Camera Shake - vuruş hissi için mucizedir) ekle.
*   **T-04 : T-02 (2 Saat): Final Build ve Son Playtest**
    *   Son Standalone Build'i (Release) al.
    *   Oyunu başından sonuna kadar **hiçbir loga bakmadan** sadece oyna.
*   **T-02 : T-00 (2 Saat): Demo Provası**
    *   Klavye/Mouse bırakılır. Kod DONDURULUR (Freeze). "Sadece şu rengi değiştireyim" demek yasaktır (kelebek etkisi yaratır).
    *   Jüriye söylenecek cümleler sesli prova edilir. F12 tuşunun çalıştığı son kez teyit edilir.

