# RIMA — Bitirme Projesi Demo Raporu

**Öğrenci:** Yasin Derya Bilgin — 231450075
**Tarih:** 13.06.2026
**Proje:** RIMA — 2D Roguelite Aksiyon RPG (Unity 6, URP 2D)

---

## 1. Özet

Hocam merhaba,

Dönem başında paylaştığım plan doğrultusunda, RIMA'nın temel sistemlerini içeren oynanabilir bir demo / vertical slice hazırladım. Bu rapor, vaat ettiğim sistemlerin güncel durumunu, öne çıkan teknik çalışmaları ve geliştirme sürecimde kurduğum yöntemi özetliyor.

Dönem başındaki hedefim 9 ana sistemdi; **9'unun da çalışır durumda olduğunu** aşağıdaki tabloda tek tek gösteriyorum. Bunlara ek olarak, planımda olmayan ama süreçte ihtiyacını görüp kendim tasarladığım bir **oyun-içi geliştirici aracı (Director Mode)** ekledim — demoda canlı göstereceğim.

## 2. Vaat Edilen Sistemler ve Durumu

| # | Dönem başı vaadi | Durum | Demoda nasıl gösterilecek |
|---|---|---|---|
| 1 | Temel combat system | ✅ Çalışıyor | Canlı oynanış: temel saldırı, hasar hesaplama, vuruş geri bildirimi |
| 2 | Birden fazla oynanabilir sınıf | ✅ Çalışıyor (5 sınıf tam yetenek setiyle, 10 sınıf stat profiliyle) | Sınıf seçim ekranı + Warblade ile oynanış |
| 3 | Sınıflara özgü yetenek + kaynak mekanikleri | ✅ Çalışıyor | Her sınıfın kendi kaynağı: Rage (Warblade), Mana (Elementalist), Energy, Focus, Tension |
| 4 | Enemy AI + farklı düşman davranışları | ✅ Çalışıyor | Farklı davranışlı düşman tipleri + tier sistemi (Elite/Champion/MiniBoss) |
| 5 | Oda bazlı ilerleme + karşılaşma yapısı | ✅ Çalışıyor | Oda temizle → ödül → kapı → sonraki oda döngüsü canlı |
| 6 | Elite düşman + boss tasarımı | ✅ Çalışıyor | Boss (Penitent Sovereign) + intro sekansı |
| 7 | Oda sonu ability selection / progression | ✅ Çalışıyor | 3 kart draft sistemi: run boş loadout'la başlar, her oda yetenek ekler |
| 8 | İlk boss sonrası dual-class | ✅ Çalışıyor — **otomatik testle kanıtlı** | Boss sonrası ikincil sınıf seçimi; testte iki sınıfın yetenek denetleyicileri ve kaynakları birlikte doğrulandı |
| 9 | Ölüm → yeniden başlama roguelite loop | ✅ Çalışıyor | Ölüm ekranı + run yeniden başlatma |

## 3. Plan Dışı Eklenen: Director Mode (oyun-içi geliştirici aracı)

Geliştirme sırasında en çok zamanımı alan şeyin "denge değeri değiştir → editörü durdur → tekrar derle → tekrar test et" döngüsü olduğunu fark ettim. Bunun üzerine, oyun çalışırken kullanabileceğim bir araç tasarladım ve yazdım:

- **Canlı stat ayarlama:** Karakter statlarını oyun açıkken slider'larla değiştirip hasara etkisini anında görüyorum.
- **Düşman spawn paneli:** İstediğim düşman kompozisyonunu anında sahneye koyup karşılaşma tasarımı deniyorum.
- **Telemetri:** Anlık DPS / öldürme süresi ölçümü; denge verisini CSV olarak dışarı alabiliyorum.
- **Prop/ışık yerleştirme:** Seviye dekorunu editörsüz, oyun içinden yerleştiriyorum.
- **Hızlı reset:** Test sırasında ölünce run'ı bozmadan devam edebiliyorum.

Bu araç bana iki şey kazandırdı: denge iterasyonu süresi dakikalardan saniyelere indi ve demoda sistemlerimin "içini" canlı gösterebileceğim bir pencere oldu.

## 4. Geliştirme Yöntemim ve Yapay Zeka Destekli Süreç

Dönem başında belirttiğim gibi geliştirmede yapay zeka destekli araçlardan yararlanıyorum; bu dönem bunu bir adım öteye taşıyıp **çok-ajanlı (agentic) bir geliştirme hattı** kurdum. Açık olmak isterim: bu araçlar benim yerime proje yapmıyor — ben tasarlıyorum, kararları veriyorum ve her çıktıyı denetletip kendim onaylıyorum. Kurduğum düzen şöyle çalışıyor:

1. **Tasarım ve mimari kararlar bende.** Sınıf sistemi, dual-class kurgusu, hasar mimarisi, oda akışı gibi tüm tasarım kararlarını ben veriyorum ve karar dokümanlarında gerekçeleriyle tutuyorum.
2. **Uygulamayı ben yönetiyorum, tekrarlı işleri ajanlara dağıtıyorum.** Belirlediğim değişiklikleri net görev tanımlarıyla kod-ajanlarına yaptırıyorum; görev tanımını, kapsam sınırını ve kabul kriterini ben yazıyorum.
3. **Hiçbir değişiklik denetimsiz girmiyor.** Kurduğum kuralda kodu yazan ajan ile denetleyen ajan her zaman farklı; denetimden geçemeyen değişiklik geri gönderiliyor. Bu dönem birden fazla değişiklik bu denetimde reddedilip düzeltildikten sonra projeye girdi.
4. **Doğrulama otomatik.** Projede 540+ birim testi koşuyor; kritik değişikliklerde ayrıca çalışma anında ölçüm yapan doğrulama kodları (ör. "bu hasar yolu şu çarpanlardan etkilenmemeli" gibi) kullanıyorum. Dual-class sisteminin çalıştığını da bu yöntemle, oyun içinde otomatik senaryo koşturarak kanıtladım.

Bu düzeni kurmak işin kendisi kadar öğretici oldu: görev tanımı yazma, kapsam sınırlama, kod inceleme kriterleri ve test-öncelikli düşünme pratiği kazandım. Bundan sonraki içerik üretiminde (kalan 5 sınıfın yetenek setleri, ek boss'lar, yeni act'ler) bu hat sayesinde çok daha hızlı ilerleyeceğim.

## 5. Kalite Güvence

- **Birim testleri:** 541 EditMode testi (kritik hasar/durum mantığı kapsanıyor); her değişiklik sonrası koşuluyor.
- **Kod denetimi:** Yazan-denetleyen ayrımıyla çift kontrol; denetim raporları proje deposunda saklanıyor.
- **Sistem taraması:** Demo öncesi combat mantığı, oyun-durumu makinesi ve Unity sahne bağlantıları üç ayrı mercekle baştan sona tarandı; bulunan kritik sorunların tamamı giderildi.
- **Sürüm kontrolü:** Tüm geçmiş GitHub'da; her değişiklik gerekçeli commit'lerle izlenebilir.

## 6. Demo Kapsamı (sunumda göstereceklerim)

1. Sınıf seçimi ve temel combat (vaat 1-4)
2. Oda ilerlemesi ve yetenek draft'ı (vaat 5, 7)
3. Director Mode ile canlı denge ayarı ve telemetri (plan dışı tooling)
4. Düşman spawn + içerik yerleştirme
5. Ölüm ve hızlı yeniden başlama (vaat 9)
6. Dual-class sistemi (vaat 8) — canlı veya otomatik test kanıtıyla
7. Boss tanıtımı (vaat 6)

## 7. Sonraki Adımlar

- Kalan 5 sınıfın yetenek setlerinin tamamlanması
- Act yapısının genişletilmesi (yeni biyomlar, ek boss'lar)
- Ses tasarımı ve müzik
- Steam'e yönelik içerik ve cila süreci

Uygun görürseniz demoda tüm bunları canlı gösterip sorularınızı yanıtlamaktan memnuniyet duyarım.

Saygılarımla,
**Yasin Derya Bilgin** — 231450075

---
*Not: Bu rapor demo günü sabahına kadar güncellenmektedir (son güncelleme: 13.06.2026 gece).*
