# RIMA Final Raporu — Acımasız İkinci Tur Review

Bu dosya, `RAPOR_RIMA_2026-06-06.docx` için jüri gözüyle yapılmış cerrahi düzeltme listesidir.

Amaç raporu baştan yazmak değil; final teslim öncesi güven kıracak yerleri, görsel/metin çelişkilerini, abartılı iddiaları ve savunmada soru doğuracak noktaları temizlemektir.

---

## Genel hüküm

Raporun omurgası artık iyi. Yeni eklenen teknik bölümler doğru yöne gitmiş: gate-slot sistemi, UI↔JSON editör, ScreenshotMode, oyun hissi katmanı ve bağımsız reviewer vakası projeyi “oyun yaptım” seviyesinden “oyun geliştirme süreci ve araç zinciri tasarladım” seviyesine taşıyor.

Ama hâlâ üç büyük tehlike var:

1. **Metin güçlenmiş, şekiller hâlâ prototip/debug gibi duruyor.**
2. **Bazı yeni iddialar güçlü ama kanıt dili eksik.**
3. **Portal-only / floating-island doktrini ile “kapı/duvar” terminolojisi yer yer çakışıyor.**

Finale kalırsa jüri özellikle şu üç şeyi yakalar:

- kötü/debug’lı şekiller,
- sayı/terim çelişkileri,
- placeholder görsel notları.

---

# 1. [Şekil 1–5] Metin iddialı, görseller amatör kalmış

**Sorun:** Attunement Chamber, combat, draft ve kapı görselleri metnin iddia ettiği kaliteyi taşımıyor. Şekillerde debug kareleri, yanlış UI state’leri, okunmayan promptlar ve test odası hissi var.

**Somut öneri:**
Şekil 1–5’i ScreenshotMode ile yeniden al:

- Şekil 1: dummy HP yok, 10 pedestal/sınıf kimliği okunuyor.
- Şekil 2: `[G] Bürün — SINIF` prompt’u net.
- Şekil 3: gerçek aktif combat anı, death/retry/draft UI yok.
- Şekil 4: kartlar + tooltip + sinerji açık, debug kare yok.
- Şekil 5: UI butonu değil, back-edge Rift portalları görünüyor.

**Önem:** KRİTİK

---

# 2. [§3.5.5 / §2.3] “Kapı” yerine “Rift portalı” terminolojisi sabitlenmeli

**Sorun:** Güncel karar wall-heavy dungeon değil, floating-island + portal. Buna rağmen raporda “arka duvar”, “kapı”, “kemer” gibi kelimeler eski wall-door mantığını çağırıyor.

**Somut öneri:**
Şunu değiştir:

> Oda temizlendikten sonra arka duvarda 1 ila 3 arasında kapı açılır.

Buna çevir:

> Oda temizlendikten sonra odanın arka kenarında 1 ila 3 Rift portalı belirir.

Ayrıca §3.5.5’te teknik isimlendirme:
- “kapı” → “çıkış portalı”
- “door slot” → “portal socket”
- “arka duvar” → “arka kenar / back edge”

**Önem:** KRİTİK

---

# 3. [§3.5.5] Portal yönü ve slot sayısı net yazılmalı

**Sorun:** Portal sisteminde yön ve slot kavramları karışabilir. 1 portal facing direction + 3 exit slot kararı açıkça savunulmalı.

**Somut öneri:**
Şu açıklamayı ekle:

> RIMA’da portal yön sayısı ile portal slot sayısı ayrıdır. Demo kapsamında tek bir temel portal facing direction kullanılmakta, aynı portal gövdesi back-edge üzerindeki üç ayrı çıkış soketine yerleştirilmektedir: EXIT_NW, EXIT_N ve EXIT_NE. Graph bir çıkış üretirse yalnızca EXIT_N, iki çıkış üretirse EXIT_NW + EXIT_NE, üç çıkış üretirse üçü birlikte render edilir. Bu karar, 8 yönlü kapı üretimini kapsam dışı bırakarak asset maliyetini düşürür ve rota kararını oyuncu için okunur tutar.

**Önem:** KRİTİK

---

# 4. [§3.5.5] 25 şablon migrasyonu / 26 oda şablonu çelişkisi açıklanmalı

**Sorun:** Rapor genelinde 26 oda şablonu deniyor; gate-slot bölümünde 25 şablon migrasyonu geçiyorsa jüri sayı çelişkisi sanabilir.

**Somut öneri:**
Eğer doğruysa şu cümleyi ekle:

> Toplam 26 oda şablonundan 25’i run odası olduğu için gate-slot migrasyonuna dahil edilmiştir; Attunement Chamber özel akışa sahip olduğundan bu migrasyonun dışında tutulmuştur.

**Önem:** KRİTİK

---

# 5. [§3.5.5] Fallback center-anchor dili riskli

**Sorun:** “Template slot sayısını karşılayamazsa eski merkez-anchor fallback kullanılır” cümlesi sistemi yarım/oturmamış gösterir.

**Somut öneri:**
Bunu runtime davranışı gibi değil, geliştirme güvenlik ağı gibi yaz:

> Geçersiz template’lerde runtime sessizce başarısız olmaz; geliştiriciye uyarı verilir ve editor doğrulaması template’in düzeltilmesini zorunlu kılar. Fallback yalnızca geliştirme aşamasında sahne çökmesini engelleyen bir koruma mekanizmasıdır.

**Önem:** KRİTİK

---

# 6. [§3.5.6] UI↔JSON editörünün mimari değeri başta anlatılmalı

**Sorun:** UI↔JSON bölümü özellik listesi gibi akıyor. Asıl katkı “veri kaynağı çakışmasını önleme” olmalı.

**Somut öneri:**
Bölüm başına ekle:

> Bu editörün amacı yalnızca oda boyamak değildir; veri kaynağı çakışmasını önlemektir. ScriptableObject oyun içi canonical kaynak olarak kalırken JSON dışa aktarım ve LLM/araç entegrasyonu formatıdır. Böylece görsel editör, otomatik importer ve test hattı aynı veriyi iki ayrı gerçeklik gibi taşımak zorunda kalmaz.

**Önem:** ORTA

---

# 7. [§3.5.6] Round-trip ve debounce iddialarına test adı eklenmeli

**Sorun:** “Aynı şablon iki kez export edilince sıfır dosya yazma doğrulandı” ve “round-trip kayıpsız” iddiaları iyi ama test adı yoksa havada kalır.

**Somut öneri:**
Paragraf sonuna gerçek test adlarını ekle:

> Bu davranış `RoomJsonRoundTripTests` ve `JsonExportDebounceTests` test gruplarıyla doğrulanmıştır.

Test adları farklıysa gerçek adlarla değiştir.

**Önem:** ORTA

---

# 8. [§3.5.7] ScreenshotMode sadece kamera preset’i değil, debug temizleme aracı olarak yazılmalı

**Sorun:** ScreenshotMode anlatılıyor ama mevcut şekiller debug’lıysa araç kendi kendini yalanlıyor.

**Somut öneri:**
Şu cümleyi ekle:

> ScreenshotMode yalnızca kamera preset’i değil, rapor görsellerinden debug marker’larını, test UI kalıntılarını ve yanlış state overlay’lerini temizlemek için kullanılan sunum modudur.

Sonra Şekil 1–5’i gerçekten bu modla yeniden al.

**Önem:** KRİTİK

---

# 9. [§2.6 / §8] Ses durumu çelişkiye düşmemeli

**Sorun:** Oyun hissi bölümünde demo SFX entegre gibi anlatılırken yol haritasında ses/müzik eksik deniyorsa “ses var mı yok mu?” sorusu doğar.

**Somut öneri:**
İki seviyeye ayır:

> Demo kapsamında temel SFX geri bildirimi CC0 kliplerle entegre edilmiştir; özgün müzik, sınıfa özel prodüksiyon sesleri ve adaptif müzik katmanları gelecek çalışma kapsamındadır.

**Önem:** KRİTİK

---

# 10. [§2.6] Hit-pause değerleri gerekçelendirilmeli

**Sorun:** 0.03 / 0.06 / 0.10 saniye gibi değerler iyi ama “neden bu değerler?” belirsiz.

**Somut öneri:**
Eğer playtest yapıldıysa:

> Bu değerler, akışkanlığı bozmadan hafif/ağır/infaz darbeleri arasında hissedilir ağırlık farkı yaratmak için playtest sırasında ayarlanmıştır.

Eğer playtest yapılmadıysa:

> Bu değerler ilk demo tuning değeri olarak belirlenmiştir.

**Önem:** ORTA

---

# 11. [§2.6 / Şekil 3] `[RMB] İnfaz` prompt’u görsel kanıt ister

**Sorun:** İnfaz prompt’u güzel mekanik ama görselde yoksa raporda “anlatılmış ama gösterilmemiş” kalır.

**Somut öneri:**
Yeni Şekil 3’te veya ek bir mini görselde Broken/Sundered düşmanın üstünde `[RMB] İnfaz` prompt’u görünsün.

**Önem:** ORTA

---

# 12. [§5] Reviewer-FAIL vakası anekdot değil tablo olmalı

**Sorun:** “9 gerçek bug yakaladı” güçlü iddia ama dağınık anlatılırsa anekdot gibi kalır.

**Somut öneri:**
Metodoloji bölümüne küçük tablo ekle:

| Review vakası | Bulgu sayısı | Kritik bulgu | Sonuç |
|---|---:|---|---|
| Knockdown review | 2 kritik | i-frame leak, double resistance | merge öncesi düzeltildi |
| Room QC review | 2 fail + 9 şüpheli | prop outside island | sistemik placer fix |
| JSON editor review | 3 küçük | undo, line ending, props korunumu | mikro-fix |

Gerçek sayılar farklıysa tabloyu gerçek verilere göre düzelt.

**Önem:** KRİTİK

---

# 13. [§5] “Reviewer-FAIL” akademik raporda tanımlanmalı

**Sorun:** “Reviewer-FAIL” ekip içi jargon gibi durabilir.

**Somut öneri:**
Başlığı şöyle yap:

> Bağımsız İnceleme ile Yakalanan Hatalar: Reviewer-FAIL Vakası

İlk cümlede tanımla:

> Bu raporda Reviewer-FAIL, kodu üreten ajandan bağımsız bir inceleme ajanının merge öncesi gerçek hata tespit ettiği durumları ifade eder.

**Önem:** ORTA

---

# 14. [§6.2 Tablo 6.1] Test envanteri ve koşu sonucu ayrılmalı

**Sorun:** ~490 EditMode + ~39 PlayMode test tanımı ile 410 PASS / 0 FAIL / 1 inconclusive aynı yerde verilirse kafa karıştırır.

**Somut öneri:**
Tabloyu ikiye böl:

1. Test envanteri:
   - EditMode tanımı: yaklaşık 490
   - PlayMode tanımı: yaklaşık 39

2. Son kayıtlı koşu sonucu:
   - 410 PASS
   - 0 FAIL
   - 1 inconclusive
   - Kapsam: EditMode snapshot
   - Hariç kalanlar: PlayMode / yeni eklenen gruplar / editör bağımlı testler

Başlık:
> Tablo 6.1 — Test Envanteri ve Son Kayıtlı Koşu Özeti

**Önem:** KRİTİK

---

# 15. [Şekil 13] Test Runner placeholder kalmamalı

**Sorun:** Şekil 13 placeholder ise test güvence bölümünün etkisi düşer.

**Somut öneri:**
Görselde:
- 410 PASS
- 0 FAIL
- 1 inconclusive
- tarih/snapshot adı

Caption:
> Şekil 13: Son kayıtlı EditMode Test Runner koşusu — 410 PASS / 0 FAIL / 1 inconclusive.

**Önem:** KRİTİK

---

# 16. [Şekil 14] QC before/after kanıtı şart

**Sorun:** Prop outside island / görsel QC hikâyesi raporun en iyi mühendislik kanıtlarından biri. Ama before/after olmadan tam vurmaz.

**Somut öneri:**
Şekil 14:
- Sol: prop outside island, kırmızı daire.
- Sağ: fix sonrası, yeşil check.
- Alt: “2 FAIL + 9 suspicious → LastFloorCells validation fix → re-audit.”

**Önem:** KRİTİK

---

# 17. [§3.2 Walkable fizik] Donut / clamp / tünelleme üçe ayrılmalı

**Sorun:** Walkable fizik zorlaması değerli ama tek paragrafta çok yoğun durabilir.

**Somut öneri:**
Alt başlıklarla böl:

- Oda geometrisi: walkable olmayan iç boşluklar.
- Hareket güvenliği: knockback/mob clamp.
- Doğrulama: tünelleme analizi + test grubu.

Kapanış:
> Bu katman, oyuncunun veya düşmanın görsel ada sınırını ihlal etmesini engelleyerek floating-island temasının oynanış güvenliğiyle çelişmesini önler.

**Önem:** ORTA

---

# 18. [§2.4 Skill sistemi] Placeholder skill’lerin draft’a sızmadığı yazılmalı

**Sorun:** 111 skill / 67 implementasyon / 44 placeholder dürüst ama jüri “placeholder draft’a çıkıyor mu?” diye sorabilir.

**Somut öneri:**
Eğer doğruysa ekle:

> Placeholder kayıtlar SkillDatabase’de tasarım envanteri olarak tutulmakta, demo draft havuzuna dahil edilmemektedir; draft sistemi yalnızca implementasyonu tamamlanmış ve sınıf filtresinden geçen skill’leri sunar.

Eğer doğru değilse önce kodu düzelt, sonra yaz.

**Önem:** KRİTİK

---

# 19. [§1.1] “Çıtanın altında kalmayı hedeflemedi” ifadesi fazla artistik

**Sorun:** Hades/Dead Cells/Slay the Spire ile aynı paragrafta fazla iddialı durabilir.

**Somut öneri:**
Şunu:

> Bu proje, o çıtanın altında kalmayı hedeflemedi.

Buna çevir:

> Bu proje, ticari ölçekteki bu örneklerin kapsamını birebir yakalamayı değil; aynı türün temel döngülerini tek geliştirici ölçeğinde veri-güdümlü ve doğrulanabilir bir prototipe dönüştürmeyi hedefledi.

**Önem:** ORTA

---

# 20. [§2.3 Combat] “Müzik tonu değişir” iddiası gerçek sistem durumuna bağlanmalı

**Sorun:** Adaptif müzik yoksa bu iddia fazla kaçar.

**Somut öneri:**
Eğer sadece SFX/ambient varsa:

> Oda temizlendiğinde clear SFX’i ve görsel geri bildirim tetiklenir.

Eğer müzik state’i gerçekten varsa:

> AudioManager, combat/clear durumlarına bağlı olarak ilgili ambient veya müzik kanalını değiştirir.

**Önem:** ORTA

---

# 21. [§3.5.5] Gate-slot için küçük şema görseli ekle

**Sorun:** ENTRY_S / EXIT_NW / EXIT_N / EXIT_NE metinle anlaşılır ama görsel çok daha iyi taşır.

**Somut öneri:**
Yeni küçük teknik figür:
- ENTRY_S
- EXIT_NW / EXIT_N / EXIT_NE
- 1 çıkış = N
- 2 çıkış = NW + NE
- 3 çıkış = NW + N + NE

**Önem:** ORTA

---

# 22. [Şekil 6] Warblade placeholder kalmamalı

**Sorun:** Warblade bölümünde görsel yoksa sınıf tasarım felsefesi soyut kalır.

**Somut öneri:**
Şekil 6:
- büyütülmüş Warblade render,
- ayak pivot doğru,
- silah görünür,
- caption: “Warblade — iki elli kılıç ve ağır omuz zırhı silüeti”.

**Önem:** KRİTİK

---

# 23. [Şekil 8] Pipeline diyagramı eksik kalmamalı

**Sorun:** İçerik üretim hattı raporun ana teknik katkılarından biri.

**Somut öneri:**
Şekil 8’i şu akışla üret:

```text
Room JSON / Editor Paint
        ↓
RoomJsonImporter
        ↓
RoomTemplateSO
        ↓
Validator
        ↓
IsoRoomBuilder
        ↓
_Arena Runtime Room
        ↓
QC Screenshot / Smoke Test
```

**Önem:** KRİTİK

---

# 24. [§5] “AI ajanları araçtır; süreci geliştirici tasarladı” vurgusu korunmalı

**Sorun:** Yok. Bu iyi bir ayrım. Rapor “AI yaptı” değil, “AI destekli mühendislik süreci tasarlandı” gibi görünmeli.

**Somut öneri:**
Şunu metodoloji katkısı olarak bağla:

> Bu nedenle projenin katkısı yalnızca AI kullanımında değil, AI çıktısını denetlenebilir mühendislik adımlarına dönüştüren süreç tasarımındadır.

**Önem:** KOZMETİK / GÜÇLENDİRME

---

# 25. [§6/§7] Test + QC + reviewer üçlüsü tek kalite güvence tablosunda bağlanmalı

**Sorun:** Test altyapısı, görsel QC ve reviewer-FAIL ayrı ayrı iyi; birleşince daha güçlü.

**Somut öneri:**

| Katman | Ne yakalar? | Örnek |
|---|---|---|
| Otomatik test | mantık/sözleşme hataları | gate-slot, round-trip |
| Görsel QC | yapısal olarak geçerli ama görsel olarak yanlış durum | prop outside island |
| Bağımsız review | çalışan ama yanlış entegre edilmiş kod | i-frame leak, double resistance |

**Önem:** ORTA

---

# 26. [Bütün şekiller] Caption gösterilmeyen şeyi vaat etmemeli

**Sorun:** “aktif düşman grubu”, “oda türü simgeleri görünür”, “pedestal’lar ve sınıf silüetleri” gibi caption’lar görselde net değilse güven kırar.

**Somut öneri:**
Ya görseli değiştir ya caption’ı dürüstleştir. Tercih: görseli değiştir.

**Önem:** KRİTİK

---

# 27. [§8 Sonuç] ~529 test / 410 PASS ayrımı net yazılmalı

**Sorun:** Sonuçta test envanteri ile son koşu sonucu karışabilir.

**Somut öneri:**

> ~529 test tanımı envanteri; son kayıtlı EditMode koşusunda 410 PASS / 0 FAIL / 1 inconclusive.

**Önem:** ORTA

---

# 28. [§2 / §8] Boss iddiası törpülenmeli

**Sorun:** Boss mekanik olarak basitse “nihai boss tasarımı tonal manifestoyu somutlaştırır” fazla epik kalır.

**Somut öneri:**

> Demo kapsamında boss düğümüne ulaşan ve temel saldırı/zafer akışını tamamlayan bir boss karşılaşması bulunmaktadır; daha zengin faz yapısı ve telegraph çeşitliliği ticari sürüm yol haritasına bırakılmıştır.

**Önem:** ORTA

---

# 29. [§4 Görsel üretim] AI görsel üretiminde kayıt/lisans/reproducibility netleşmeli

**Sorun:** AI/PixelLab görsel üretimi varsa jüri lisans ve tekrar üretilebilirlik sorabilir.

**Somut öneri:**

> Varlık üretiminde prompt kayıtları, çıktı dosya adları, import ayarları ve kullanım amacı dokümante edilmiştir. Demo varlıkları akademik prototip kapsamında kullanılmış; üçüncü taraf seslerde CC0 lisans dosyaları projeye eklenmiştir.

**Önem:** ORTA

---

# 30. [Bütün rapor] Türkçe karakter / encoding temizliği yapılmalı

**Sorun:** “Cift Yonlu”, “Katmani”, “dogru”, “calisması” gibi ASCII’ye düşmüş kelimeler final raporu ucuz gösterir.

**Somut öneri:**
Find/replace + manuel kontrol:
- Cift → Çift
- Yonlu → Yönlü
- Katmani → Katmanı
- dogru → doğru
- calisması → çalışması
- sarsintisi → sarsıntısı
- tanımlanmıstır → tanımlanmıştır

**Önem:** KRİTİK

---

# En acil 10 düzeltme

1. Şekil 1–5’i ScreenshotMode ile yeniden al.
2. “Kapı / arka duvar” terminolojisini “Rift portalı / arka kenar”a çevir.
3. Portal yönü = 1, slot = 3+1 kararını açık yaz.
4. 25/26 şablon migrasyon farkını açıkla.
5. Test envanteri ile test koşu sonucunu ayır.
6. Şekil 6, 8, 13, 14 placeholder kalmasın.
7. Ses durumunu demo SFX / final müzik diye ayır.
8. Reviewer-FAIL’i tabloya dök.
9. Placeholder skill’lerin draft’a sızmadığını yaz.
10. Türkçe karakter / encoding temizliği yap.

---

# Nihai karar

Rapor doğru yönde. Teknik katkı artık sadece “oyun yaptım” değil, “veri-güdümlü üretim hattı + çok-ajanlı kalite güvence süreci kurdum” seviyesinde okunuyor.

Ama final teslim öncesi görsel kanıtlar temizlenmezse rapor kendi ayağına sıkacak. Metin takım elbise giymiş, şekiller hâlâ pijamayla geziyor. Bu ikisini aynı salona sokma.
