# COUNCIL POLISH REVIEW: RIMA Akademik Bitirme Raporu

**Değerlendirme Mercii:** Bağımsız Akademik Danışman (ax_pro)
**Odak:** Akademik yapı, iddia-kanıt dengesi, figür tutarlılığı ve akademik Türkçe kalitesi.

## BULGULAR VE DÜZELTME ÖNERİLERİ

### P1 (Kritik) | Şekil Atıflarının (In-text Reference) Eksikliği
- **Bölüm/Başlık:** Tüm rapor gövdesi.
- **Sorun:** Raporda 12 adet figür ve açıklayıcı altyazıları (caption) mevcut, ancak gövde metninde bunlara "(bkz. Şekil N)" veya "Şekil N'de görüldüğü üzere" şeklinde doğrudan atıf yapılmamış. Yalnızca "Aşağıdaki şekil..." gibi zayıf yönlendirmeler kullanılmış. Jüriler bu referans kopukluğunu büyük bir eksiklik olarak görür.
- **Önerilen Düzeltme:** İlgili figürden önceki veya sonraki paragrafta figür numarası açıkça zikredilmeli. Örneğin Bölüm 6.2'deki "Aşağıdaki şekil..." ifadesi "Şekil 6'da gösterildiği üzere..." biçimine çevrilmelidir.

### P1 (Kritik) | Oyun Jargonu, İngilizce Terim Yoğunluğu ve "Centerpiece"
- **Bölüm/Başlık:** Genel, özellikle Bölüm 5 Başlığı ve Bölüm 11.
- **Sorun:** "Centerpiece", "Tooling", "Run", "Spawn", "Idle", "Juice", "QC" gibi terimler ya hiç Türkçeleştirilmemiş ya da "Oturum / Koşu / Run" şeklinde tutarsız kullanılmış. Bölüm 5'in başlığındaki "(Centerpiece)" ifadesi akademik bir başlıktan çok bir sunum veya blog yazısı havası veriyor.
- **Önerilen Düzeltme:** 
  - Bölüm 5'in başlığından "(Centerpiece)" ifadesi tamamen çıkarılmalıdır.
  - Sık kullanılan İngilizce terimler ilk kullanımda Türkçe karşılıkla verilip parantez içinde orijinali bırakılmalı (ör. "Oturum (Run)", "Geliştirme Araçları (Tooling)"), sonrasında seçilen Türkçe terimle devam edilmelidir.

### P2 | YZ Etmenlerinde Aşırı İnsansallaştırma (Antropomorfizm) ve Coşkulu Dil
- **Bölüm/Başlık:** Bölüm 8 (Yapay Zekâ Destekli Çok-Ajanlı Geliştirme Metodolojisi)
- **Sorun:** "Danışman Konsey", "Adversarial eleştirmen", "Yazılımcı Ajan", "anlaşmazlıktan değer üretme disiplini" gibi ifadeler projeyi bir mühendislik sisteminden ziyade bir rol-yapma konsepti gibi gösteriyor. Bu dil, tezin ciddiyetini gölgeleyerek projeye "prompt engineering deneyi" veya "yapay zekaya oyun yaptırdım" sığlığı atfedilmesine sebep olabilir.
- **Önerilen Düzeltme:** Roller daha nesnel ve teknik ifade edilmeli: "Danışman Konsey" yerine "Mimari Değerlendirme / Çapraz Doğrulama Modelleri", "anlaşmazlıktan değer üretme" yerine "farklı LLM'lerin çapraz denetimiyle risk tespiti" gibi yapısal tanımlamalar kullanılmalı.

### P2 | "Geliştirici Günlüğü" (Devlog) Havası
- **Bölüm/Başlık:** Bölüm 11 (Karşılaşılan Zorluklar ve Çözümler)
- **Sorun:** Her problemin sonunda yer alan "Ders: ..." formatı, akademik bir rapordan çok bir post-mortem veya devlog (geliştirici günlüğü) hissi veriyor. Akademik raporlarda kişisel derslerden ziyade nesnel çıkarımlar sunulur.
- **Önerilen Düzeltme:** "Ders:" başlıkları "Sonuç/Çıkarım:" şeklinde güncellenmeli ve kişisel/didaktik ton daha objektif bir mühendislik tespiti diline çekilmelidir.

### P3 | Format Kullanımı: Kalın Metin (Bold) Yığılması
- **Bölüm/Başlık:** Bölüm 4.1 ve 8.6
- **Sorun:** Paragraf başlarındaki konuları ayırmak için yoğun olarak **kalın metin** kullanılmış (Ör: **Attunement Chamber**, **Combat**, **3-Kart Skill Draft**).
- **Önerilen Düzeltme:** Bu alt kırılımlar dördüncü seviye başlık (`####`) formatına dönüştürülürse doküman yapısı (TOC'a girmese bile) çok daha resmi ve okunabilir durur.

---

### JÜRİ GÖZÜNDEN EN KRİTİK 3 ZAYIFLIK (ÖZET)
1. **Gövdede Şekil Numaralarının Anılmaması:** Okuyucu metin ile görsel eşleştirmesini kendi kendine yapmak zorunda kalıyor; net "(bkz. Şekil N)" atıfları eklenmeli.
2. **Akademik Olmayan Oyun/Geliştirici Jargonu:** Özellikle Bölüm 5 başlığındaki "Centerpiece" ve metin içindeki tutarsız "run/koşu" kullanımı raporun ciddiyetini zedeliyor.
3. **Bölüm 8'deki Aşırı İnsansallaştırılmış Dil:** "Danışman konsey", "eleştirmen" gibi tabirler mühendislik tezinden çok fantastik bir kurgu veya sığ bir prompt çalışması izlenimi veriyor; dil nesnelleştirilmeli.
