# RIMA — HAYALİMİZ vs ŞU ANKİ DURUM
Rapora ek olarak ChatGPT'ye verilecek dosya. Amaç: her alanda **ne hayal ettik** → **şu an ne var** → ChatGPT bize **aradaki farkı kapatmak için ne yapabileceğimizi** önersin. Dürüst yazıldı; "şu an" kısımları eksikleriyle anlatılıyor.

---

## 1. Karakter seçim ekranı (Attunement Chamber)
**Hayal:** Hades'in ev sahnesi gibi atmosferik bir "oda": loş mor void üstünde taş bir platform, hilal dizilimde 10 pedestal, her pedestalda o sınıfın ruhani/donuk heykeli. Oyuncu içinde YÜRÜR, heykele yaklaşınca sinematik bir "bürünme" anı yaşar (cyan enerji sarması, kısa bir flash/parçacık), heykelin kimliği hissedilir (Warblade heybetli, Shadowblade sinsi duruşlu). Işıklandırma dramatik: pedestal spot'ları, rünlerden yayılan cyan parıltı, brazier'lardan sıcak turuncu. Bir "kutsal salon" hissi.
**Şu an:** Sistem ÇALIŞIYOR (yürüme + [G] bürünme + dummy testi + kapıdan çıkış) ama atmosfer hayalin gerisinde: oda düz/çıplak duruyor, ışık dramaturjisi yok (2D Light kullanımı minimal), heykeller tek tip gri silüet (sınıf kimliği duruştan okunmuyor), bürünme anı görsel olarak sönük (efekt zayıf), pedestal çevresi boş — "kutsal salon" değil "test odası" hissi var.
**ChatGPT'ye soru:** Sınırlı sprite bütçesiyle (yeni karakter animasyonu üretmeden) bu odayı atmosferik kılmak için en yüksek etkili 5 müdahale ne olur? (ışık, vinyet, parçacık, ses, kompozisyon, prop yerleşimi...)

## 2. Combat hissi
**Hayal:** Hades vuruş hissi: her darbe ekranda hissedilir (hit-stop, ekran sarsıntısı dozunda, parlak hit-flash, düşman geri savrulur), ağır vuruşta düşman YERE SERİLİR ve kalkarken savunmasızdır, ölümler iz bırakır (kemik/leke decal). Skill kombinasyonları görünür sinerji üretir (BREAK durumundaki düşmana EXECUTE bonusu — "Sundered Beat" imza mekaniği).
**Şu an:** Temel zincir kurulu: hit-flash + knockback + knockdown (kod-only: eğme+squash+i-frame) + ölüm decal'i ÇALIŞIYOR. Ama doz ayarı yapılmadı (feel-tuning bekliyor), hit-stop/screen-shake dozları test edilmedi, skill VFX'leri çoğunlukla placeholder (Elementalist büyüleri görsel olarak zayıf), imza mekaniğin (BREAK→EXECUTE) oyuncuya öğretimi yok — oynayan kişi sinerjileri keşfedemiyor.
**ChatGPT'ye soru:** Animasyon üretmeden, sadece kod/particle/timing ile combat hissini bir kademe yükseltecek öncelikli liste ne olmalı? İmza mekaniği oyuncuya nasıl sezdiririz (tutorial yazısı olmadan)?

## 3. Oda görselliği ve çeşitlilik
**Hayal:** Her oda "void üstünde yüzen kadim taş ada": kenarlarından mor boşluğa bakınca derinlik hissi, odalar arası belirgin görsel çeşitlilik (kemik bahçesi, yıkık şapel, kristal mağara hissi), prop'lar hikâye anlatır (kırık mühür taşları = başarısız hapisler). Önümüzdeki odaların önizlemesi void'de silüet adalar olarak görünür (StS haritasının diegetik hali).
**Şu an:** 26 oda template'i + otomatik prop yerleşimi + checker zemin + yönlü cliff'ler çalışıyor; ada hissi VAR ama odalar birbirine çok benziyor (aynı zemin seti, aynı prop havuzu — tema varyantı yok), void tamamen boş düz renk (derinlik yok), diegetik önizleme adaları hiç yapılmadı (M-tuşu overlay haritası var, o da soyut).
**ChatGPT'ye soru:** Tek zemin/prop setiyle tema çeşitliliği yanılsaması nasıl yaratılır (renk grading? ışık? decal katmanı? prop yoğunluk profilleri?)? Void'e ucuz derinlik katmanın yolları?

## 4. Skill draft / build kurma
**Hayal:** Her kart seçimi anlamlı bir karar: kartlar elindeki build'le konuşur (sinerji parlar, anti-sinerji soluk), rarity heyecanı var, seçim anı kutlanır (kart büyür, mühür kırılır efekti). Run sonunda "şu build'i kurdum" diyebilmelisin.
**Şu an:** 3-kart draft + hover tooltip + sinerji pulse çalışıyor, kart juice (hover/glow) var. Ama skill havuzu sığ: 10 sınıftan sadece 4'ünün gerçek skill'leri var (diğerleri placeholder), derinlik (Phase-1 stub'ları) eksik — build çeşitliliği şu an sınırlı. Seçim anı kutlaması yok.
**ChatGPT'ye soru:** Az sayıda skill'le bile "build kuruyorum" hissi nasıl güçlendirilir? Draft ekranına eklenecek en değerli 2-3 mikro-etkileşim?

## 5. Kapılar ve run yapısı
**Hayal:** Oda temizlenince arka duvarda kapılar belirir (Hades), her kapı gideceğin odanın türünü rünüyle söyler, risk/ödül seçimi hissedilir (elite = tehlike ama iyi ödül). Run, dallanan bir harita üstünde "yol çizme" hissi verir.
**Şu an:** TAM ÇALIŞIYOR ve yeni düzeltmeyle kapı konumları template'te sabit slot'lara oturdu: dal sayısı kadar kapı, tür rünleri, kilit/açılma, M-haritası. Eksik: kapı türleri arasında ödül farkı hissedilir değil (elite seçmenin getirisi belirsiz), kapı açılma anı sönük (cyan parıltı var ama "belirme" draması yok).
**ChatGPT'ye soru:** Oda türü seçimini gerçek bir risk/ödül kararına çevirecek en hafif sistem ne olur (önizleme bilgisi? ödül ikonu? elite bonusu)?

## 6. Karakter/silah görselliği
**Hayal:** 10 sınıfın her biri eline silahını almış, silah saldırıda doğal savruluyor; karakter ekranda "yerine oturmuş" (ayaklar zeminde, derinlik sıralaması doğru), her sınıf silüetinden tanınıyor.
**Şu an:** Silah sistemi kodlu ve Warblade'de uçtan uca canlı (kılıç elde, swing çalışıyor) ama 10 sınıftan sadece 1'inin silahı üretildi. Karakterler bazen "havada" görünüyor (pivot kalibrasyonu ŞU AN yapılıyor) ve player'ın derinlik sıralaması düzeltiliyor. Sınıf kimliği idle sprite'larda var ama animasyon çeşidi az (yeni animasyon üretimi bilinçli olarak demo sonrasına ertelendi).
**ChatGPT'ye soru:** Sıradaki 3 silah (Elementalist rün-diski, Ranger yayı, Shadowblade ikiz hançer) üretilirken görsel tutarlılık için nelere dikkat etmeliyiz? Tek idle+swing'le silah çeşitliliği hissi nasıl maksimize edilir?

## 7. Mob/boss çeşitliliği
**Hayal:** Act-1'de 4-6 ayırt edilebilir mob arketipi (hızlı dalıcı, menzilli, tank, patlayan) + okunaklı telegraph'lı bir boss; her düşman tehdidini silüetinden ve hareketinden anlatır.
**Şu an:** ~13 mob knockback/hit-flash'a bağlı ve dövüşüyor; ama görsel olarak çoğu placeholder/benzer, davranış çeşitliliği sınırlı (çoğunlukla yaklaş-vur), boss var ama mekanik olarak basit (telegraph dili tutarlı değil). Mob/boss animasyon üretimi sıradaki büyük iş.
**ChatGPT'ye soru:** Az animasyonla (idle+walk+attack) davranış üzerinden mob çeşitliliği yaratmanın en ekonomik yolları? Boss telegraph dili için pixel-art'ta okunaklılık kuralları?

## 8. Meta döngü ve ekonomi
**Hayal:** Her run bir şey bırakır: Shattered Echo birikir, yeni sınıf açılır, "bir run daha" çekimi oluşur. Ölüm ceza değil ilerleme.
**Şu an:** Döngü kapalı ve çalışıyor: run-sonu Echo ödülü (oda*3+kill/5) + sınıf kilidi açma (80-250 ◈ veya achievement). Ama tuning yapılmadı (kazanım/fiyat dengesi tahmini), sınıf açmak dışında Echo harcanacak yer yok (bilinçli — mağaza istemiyoruz ama "biriktirme amacı" zayıflayabilir), achievement yolu UI'da yazıyor fakat takibi gösterilmiyor.
**ChatGPT'ye soru:** Mağazalaşmadan (skill/item satmadan) Echo'ya ikinci bir anlam katmanın zarif yolları var mı? (kozmetik? chamber dekorasyonu? bilgi/lore açma?)

## 9. Ses ve müzik
**Hayal:** Loş, ritüel atmosfer: derin ambient + combat'ta yükselen perküsyon, vuruşlarda etli SFX, bürünmede koral bir an.
**Şu an:** SES HİÇ YOK (bilinçli erteleme — demo görsel/sistem odaklı gitti). En büyük eksik atmosfer katmanı muhtemelen bu.
**ChatGPT'ye soru:** Bitirme demosu için minimum-emek maksimum-etki ses paketi nasıl kurulur (kaç SFX, hangi öncelik sırası, ücretsiz kaynak stratejisi)?

## 10. İlk 5 dakika / öğretim
**Hayal:** Oyuncu hiçbir şey okumadan akar: menü→chamber'da merakla dolaşır→bürünür→kapıdan geçer→ilk odada vurmayı öğrenir→draft'ta "ooo" der.
**Şu an:** Akış teknik olarak kesintisiz çalışıyor ama hiçbir yönlendirme yok: chamber'a düşen oyuncu ne yapacağını bilmiyor ([G] prompt'u yaklaşınca çıkıyor ama oraya yürüme sebebi verilmiyor), kontroller hiçbir yerde yazmıyor, draft'ta kartların ne işe yaradığını ilk kez gören anlamayabilir.
**ChatGPT'ye soru:** Tutorial ekranı/metin duvarı olmadan, sahne diliyle (ışık, ses, kompozisyon, tek satırlık diegetik metin) ilk 5 dakikayı yönlendirmenin somut önerileri?

---

## ChatGPT'den istenen çıktı formatı
Her bölüm için:
1. **Teşhis:** hayal-mevcut farkının kök nedeni (1-2 cümle)
2. **Öneriler:** etki/emek oranına göre sıralı 3-5 somut aksiyon (yüksek etki + düşük emek üstte). "Animasyon üretimi gerektirmez" kısıtına saygı duy — tek geliştirici + AI-destekli pipeline, sprite bütçesi dar.
3. **Kırmızı çizgiler:** önerirken kaçınılması gereken tuzaklar (scope creep, bitirme teslim tarihi var).
Sonda: tüm bölümlerden seçilmiş, teslim tarihine kadar yapılabilecek **TOP-10 öncelik listesi**.
