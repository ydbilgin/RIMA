# DEMO — HOCA SORU-CEVAP HAZIRLIĞI (2026-06-13)

> Format: ekran açık, sohbet eşliğinde canlı demo. Ezber YOK. Amaç: sistemleri canlı gösterirken hocanın sorularını sağlam, savunulabilir cevaplamak. Her cevap gerçek dosya/kanıta dayalı — uydurma yok, bilmediğini "şu an prova edilmedi" diye dürüst söyle.

---

## A. PLAYTEST / TEST / KALİTE (en güçlü kozun — hoca buraya en çok girer)

**S: Bunu nasıl test ettin? Sadece elle mi oynadın?**
> Üç katmanlı test ediyorum: (1) **541 otomatik birim testi** (EditMode) — hasar, can, durum mantığı; her değişiklikten sonra koşuyor. (2) **Çalışma-anı veri kanıtı** — kritik mekanikleri oyun içinde programatik tetikleyip sonucu ölçüyorum (ör. "bu hasar yolu şu çarpanlardan etkilenmemeli" diye sayısal doğruluyorum). (3) **Uçtan uca senaryo testi** — tüm demo akışını otomatik oynatıp her adımı assert ediyorum. Bunların hepsi proje deposunda kayıtlı.
>
> **Somut kanıt (demo sabahı koştuğum uçtan uca test — 10/10 PASS):** sınıf seçimi → combat → öldürme → stat ayarı → preset'ler → spawn limiti → ölüm/reset → duraklatma kilidi → dual-class → telemetri, hepsi otomatik doğrulandı, 0 hata. Örnek ölçümler: physPower 50→250 yapınca temel saldırı hasarı tam 5 kat; debugGlobalDamageMult 1→3 yapınca tam 3 kat; öldürme bildirimi tam 1 kez (çift sayım yok); duraklatmada DPS saati gerçekten donuyor (15.5 sn geçti, sayaç ilerlemedi). Yani "çalışıyor" derken arkasında sayısal kanıt var.

**S: Dengeyi nasıl ölçüyorsun? Nereden biliyorsun bir sınıf çok güçlü değil diye?**
> Director Mode'da **telemetri** var: anlık DPS ve öldürme süresi (TTK) ölçüyorum, denge verisini CSV olarak dışarı alıp karşılaştırıyorum. *(Canlı göster: stat slider → LMB vur → DPS değişimi)*. Stat'ları oyun açıkken değiştirip etkiyi anında görebildiğim için denge iterasyonu çok hızlı.

**S: Hiç bug buldun mu? Nasıl yakaladın?**  ← BUNA HAZIR OL, güçlü cevap
> Evet — demo öncesi oyunu üç ayrı mercekle (combat mantığı, oyun-durumu makinesi, Unity sahne bağlantıları) baştan sona taradım. Birkaç gerçek sorun çıktı ve hepsini düzelttim:
> - Ölüm ekranı/duraklatma sırasında saldırı girdisi hâlâ tetikleniyordu → merkezi bir aktif/pasif anahtarıyla kapattım.
> - Yetenekle öldürülen düşman "öldürme" sayılmıyordu (sadece temel saldırı sayıyordu) → öldürme bildirimini tek merkeze taşıdım.
> - Mermi hasarının bir dalı yanlışlıkla durum-çarpanlarından etkileniyordu → hasarı birebir koruyan ayrı bir yol açtım, sayısal olarak kanıtladım.
> Bu süreç bana test-öncelikli düşünmeyi öğretti: önce "nasıl yanlış gidebilir" sorusunu soruyorum.

**S: Bu animasyon/his nasıl yapıldı?**
> Tek sprite + motor efekti yaklaşımı (Dead Cells mantığı): idle nefes, yürüme eğimi, saldırı vuruşu kodla — sıçrama, sallanma, kısa duraklama. Frame frame çizim yerine matematiksel hareket; performanslı ve tüm karakterlere aynı anda uygulanabilir.

---

## B. SİSTEM / MİMARİ (ekranda gösterirken açıkla)

**S: Combat nasıl çalışıyor?**
> Tüm hasar tek merkezden geçiyor: hasar hesaplayıcı → can sistemi. Temel saldırı stat-ölçeklenebilir bir "paket" olarak gidiyor; yetenekler kendi yollarından. Bu merkezi yapı sayesinde telemetri, çarpanlar ve efektler tek yerden yönetiliyor.

**S: Sınıflar gerçekten farklı mı, yoksa sadece stat mı değişiyor?**
> Her sınıfın kendine özgü **kaynak mekaniği** var: Warblade Rage biriktiriyor, Elementalist Mana harcıyor, Shadowblade Energy, Ranger Focus, Ronin Tension. Yetenekler de sınıfa özel. *(5 sınıf tam yetenek setiyle oynanabilir; 10 sınıfın stat profili tanımlı — kalan 5'in yetenek setini ekliyorum.)*

**S: Dual-class nasıl çalışıyor? Göster.**  ← Director'da CANLI gösterilebilir
> İlk boss yenilince oyun ikincil sınıf seçimi açıyor; iki sınıfın yetenekleri ve kaynakları aynı run'da birleşiyor. *(Director Mode'da "Dual-Class Draft" butonuyla bu kapıyı canlı tetikleyebilirim — seçim ekranı açılıyor, sınıf seçince ikinci yetenek seti ve kaynağı geliyor.)* Bunu otomatik testle de doğruladım: seçim sonrası yetenek denetleyicisi birden ikiye çıkıyor, ikincil kaynak ekleniyor.

**S: Ölünce ne oluyor?**
> Ölüm ekranı → run yeniden başlıyor (roguelite döngüsü). Demo için ek olarak bir "hızlı reset" koydum: test sırasında ölünce akışı bozmadan anında devam edebiliyorum.

**S: Oda ilerlemesi?**
> Oda temizle → ödül draft'ı (3 karttan yetenek seç) → kapı → sonraki oda. Run boş loadout'la başlıyor, yeteneklerini sen kuruyorsun — her run farklı.

---

## C. SÜREÇ / AI KULLANIMI (dürüst + kendine güvenli)

**S: Bunda yapay zekayı ne kadar kullandın? Kendin ne yaptın?**  ← NET VE DÜRÜST
> Yapay zeka destekli bir geliştirme hattı kurdum ama araçlar benim yerime proje yapmıyor. Tasarım ve mimari kararları ben veriyorum, görev tanımlarını ve kabul kriterlerini ben yazıyorum. Önemli bir kuralım var: kodu yazan ile denetleyen her zaman farklı — denetimden geçmeyen değişiklik geri dönüyor; bu dönem birkaç değişiklik bu denetimde reddedilip düzeltildi. Bu düzeni kurmak işin kendisi kadar öğreticiydi: görev tanımı yazma, kapsam sınırlama, kod inceleme ve test disiplini kazandım. Detaylı raporu hazırladım.

**S: Yani kodu sen yazmadın?**
> Mimariyi ve kararları ben kuruyorum; tekrarlı uygulama işlerini yönettiğim ajanlara dağıtıyorum, her çıktıyı denetletip onaylıyorum. Sürüm geçmişinin tamamı GitHub'da gerekçeli commit'lerle — hangi değişikliğin neden yapıldığı izlenebilir.

---

## D. ZAYIF NOKTALAR — soru gelirse dürüst karşıla (ASLA "yok" deme)

| Hoca sorabilir | Dürüst + güçlü cevap |
|---|---|
| "Neden 10 değil 5 sınıf oynanabilir?" | "10 sınıfın stat profili ve kimliği tanımlı; 5'i tam yetenek setiyle oynanabilir. Kalan 5'in yeteneklerini aynı mimariyle ekliyorum — sistem hazır, içerik üretimi sürüyor." |
| "Dual-class'ı canlı göremez miyim?" | "Gösterebilirim — Director aracıyla boss-sonrası kapıyı anında tetikliyorum. Normal oynanışta boss'a kadar süre alıyor, o yüzden kısayolu kullanıyorum; mekanik aynı." |
| "Müzik yok?" | "Ses tasarımı sonraki faz; altyapı hazır, müzik dosyası eklenince çalışıyor." |
| "Bu gerçek bir oyun mu olacak?" | "Hedef Steam'de yayımlanabilir bir yapı. Bu bitirme kapsamı oynanabilir bir vertical slice — çekirdek 9 sistem çalışıyor; bundan sonrası içerik genişletme." |

---

## E. CANLI GÖSTERİRKEN İŞE YARAYAN ARAÇLAR (sohbet akışında)

- **Director Mode** (` ` ` ya da panel): stat tuning, telemetri, spawn, prop yerleştirme, Quick Reset, Dual-Class Draft butonu, stat preset'leri (Tank/Glass Cannon).
- **F1 debug paneli:** God Mode (ölüm riskini kapat), Kill All Mobs, oda atlama — akışı hızlandırmak için.
- Hoca "şunu zorlaştır / şu sınıfı dene / şuraya düşman koy" derse → Director'da canlı yap. **Bu, sistemin veri-güdümlü ve modüler olduğunu kanıtlayan en güçlü an.**

> Hatırlatma (teknik tuzaklar, sohbet sırasında kafanda olsun): stat slider'ını **temel saldırıyla** göster (yetenekler farklı ölçekleniyor); kill/juice göstereceksen temel saldırıyla öldür; konsolu sunumdan önce temizle.
