# Sunum Q&A ve Çerçeve Notları — 2026-06-18

## Açılış Çerçevesi (ilk 60 sn)

"Bu proje, bitmiş ticari bir oyun değil; veri-güdümlü bir mimari, oyun-içi araç zinciri ve çalışan bir dikey dilimdir. Katkı; kod satırı adedinden önce sistem tasarımında, ajan rollerinin ve doğrulama kriterlerinin belirlenmesinde ve tüm bu parçaların birbirine bağlandığı süreç mimarisindedir. Oyun oynayabilirsiniz, ama asıl değer ölçülebilir: 6925 düğümlü bağımlılık grafında en bağlı 10 düğümün 6'sı geliştirici aracıdır."

---

## Test Sayısı Savunması

**Soru:** "Demo 411 test gösteriyor, rapor 549 diyor — hangisi doğru?"

**Hazır cevap:** 549 toplam test method envanteridir; kaynak kodunda 675+ method ile tutarlı, muhafazakâr bir sayımdır. 411 ise son tam EditMode koşusunun aktif koştuğu test sayısıdır. Aradaki fark, o koşudan sonra eklenen test gruplarından kaynaklanmaktadır. Rapor §9.2 bu ayrımı açıklar: "envanterden koşulan altkümesi" ayrımı bilinçlidir.

---

## Q&A — 3 Zor Soru

### (a) "AI yazdıysa senin katkın ne?"

Mimari tasarım, her görevin kabul kriterleri ve otomatik test/QC altyapısının insan tarafından tanımlanmasıdır. Ajanlar bu insan-tanımlı mantığı koda döker; hangi sistemin nasıl kurulacağına, hangi çıktının "tamam" sayılacağına ve hangi kararın belgeleneceğine geliştirici karar verir. Sessiz başarısızlık, yarım iş, tasarım zevki — bunların hiçbiri ajanlara devredilmedi.

### (b) "DirectorMode 168 bağ = god object değil mi?"

DirectorMode yalnızca `#if DEVELOPMENT_BUILD` / `#if DEMO_BUILD` direktifi altında derlenir; production build'e dahil olmaz. Bu, runtime coupling değil dev-tool coupling'dir — tıpkı Unity Editor'ün kendi araç pencereleri gibi. Oyun mantığı DirectorMode'a bağımlı değildir; DirectorMode oyun sistemlerini gözlemler ve tetikler.

### (c) "Boss özgün mü?"

Görsel olarak `hollow_hulk` sprite'ının büyütülmüş versiyonu kullanılmaktadır — bu bilinçli bir placeholder kararıdır, yol haritasında "The Architect" olarak adlandırılan nihai boss için kaynak ayrılmamıştır. Buna karşın davranış bütünüyle özgündür: `PenitentSovereign.cs` sıfırdan yazılmış, altı ayrı telegraph saldırısı (HolyLash, FractureStrike, ChainExplosion, SovereignsWrath, FractureCharge, ShackleThrow) bespoke faz mantığıyla bağlanmıştır.
