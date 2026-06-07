# Council Sorusu — ChatGPT Rapor Review-2 Paketi Değerlendirmesi (DEEP/ARCHITECTURE LENS)

Sen RIMA bitirme projesi council'inin DERİN MİMARİ/ARGÜMANTASYON danışmanısın. Görev: ChatGPT'nin rapor review paketindeki 30 bulguyu jüri-savunulabilirliği ve raporun tez bütünlüğü açısından değerlendir.

## Read these files
- `STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/01_ACIMASIZ_REVIEW.md` (30 bulgu — ANA GİRDİ)
- `STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/02_ONCELIKLI_DUZELTME_LISTESI.md`
- `STAGING/report/RAPOR_DRAFT_2026-06-06.md` (hedef rapor — İçindekiler + §1 + §2.6 + §3.5.5-3.5.7 + §5 + §6 + §8'i oku; tamamı ~93KB)

## Bağlam (doğrulanmış olgular)
- Rapor = Türkçe bitirme (senior design) final raporu; ana tez: "tek geliştirici, veri-güdümlü üretim hattı + çok-ajanlı AI kalite-güvence süreci kurdu"
- Oyun doktrini: floating-island + Rift portalı (duvarsız); kodda socketId'ler `door_NW_01/door_N_01/door_NE_01`; girişler authored, kapı görseli = portal kemeri
- Encoding bulgusu (#30) DOĞRULANDI: son edit turunda eklenen §2.6/§3.5.6 ASCII Türkçe ile yazılmış — düzeltilecek
- Rapor 8 gömülü şekil içeriyor; ChatGPT 14 şekilli listeden bahsediyor (bazıları placeholder)
- Skill draft guard'ı kodda var (isImplemented filtresi)

## Senin sorularıın (4 soru, derin lens)
1. **Terminoloji (#2/#3):** Raporda "kapı"→"Rift portalı" toptan değişimi tez bütünlüğünü güçlendirir mi, yoksa kod adlarıyla (door_*) çelişki yaratıp jüride "kod ile rapor uyuşmuyor" sorusu mu doğurur? Köprüleme stratejisi öner (ör. bir kez "kodda tarihsel olarak door_* adlandırması kullanılır, oyun dilinde bunlar Rift portalıdır" dipnotu yeterli mi?).
2. **Metodoloji bulguları (#12/#13/#24/#25):** Reviewer-FAIL tablosu + üçlü kalite-güvence tablosu + "AI araçtır, süreci geliştirici tasarladı" vurgusu — bunlar raporun ana tezini nasıl en güçlü şekilde taşır? Tablo yapıları ChatGPT'nin önerdiği gibi mi olmalı, yoksa daha iyi bir çerçeve var mı?
3. **Şekil stratejisi (#1/#8/#15/#16/#21/#22/#23/#26):** 14 şekillik ideal listeye karşı sınırlı zaman gerçeği: jüri etkisi açısından şekilleri ÖNCELİKLENDİR (hangi 3-4 şekil yatırımın karşılığını en çok verir; hangileri caption-dürüstleştirme ile kurtarılır). "Metin takım elbise, şekiller pijama" eleştirisi ne kadar ağır basar?
4. **REJECT/MODIFY adayları (#5/#10/#19/#20/#28/#29 vb.):** Hangi bulgular yanlış varsayıma dayanıyor, hangileri akademik rapor için gereksiz süs, hangileri kapsam dışı? Her REJECT için tek cümle gerekçe.

## Çıktı formatı
30 bulgunun her biri için tek satır: `#N: ACCEPT / MODIFY(nasıl) / REJECT(neden)` + sonda 1 paragraf "en kritik 5 iş" sıralaması. Markdown. Türkçe.
