# Codex Review Task — Oyun Fikirleri Logic-Build ARPG Sentez

**Tarih:** 2026-05-15
**Dispatch:** RIMA orchestrator (Sonnet)
**Hedef:** 3-AI sentez kararının (Circuit Breaker + Rayline çift prototip) **brutal honest review**'u.

## Bağlam (kullanıcı ana sorusu)

Solo dev kullanıcı RIMA (2D top-down ARPG, 10 sınıf, 80 commit, asset pipeline LIVE) projesinde **identity krizi** yaşadı son hafta:
- Library cache disaster (~4 saat sebepsiz debug)
- "Bu oyunu istediğim gibi yapabilecek miyim, başta oyun fikrine mi geçsem" diye samimi soru sordu
- Şu an oyun_fikirleri klasöründe 22 yeni konsept üretti (Codex + Gemini + Claude)
- 3-AI sentez kararı: **Circuit Breaker (Arcwright) + Rayline Runner (Geometry Blade) çift prototip**

**Ana strateji sorusu:** Solo dev olarak tek tür mü (cozy sim veya logic-ARPG), yoksa multi-genre stüdyo mu kurmalı?

## Senin (Codex) görevin

`F:\Antigravity Projeler\oyun_fikirleri\OYUN_FIKIRLERI\00_STRATEJI\18_AI_SLOP_BRAINSTORM_2026_05_14\17_FINAL_SENTEZ_LOGIC_BUILD_ARPG.md` dosyasını oku. **Sentez kararının zayıf noktalarını bul.**

Özellikle şu soruları cevapla:

### 1. Çift Prototip Kararı: Doğru mu?
- 10 iş günü CB + 5 iş günü Rayline = 15 iş günü spike + 3 gün karar = ~4 hafta
- **Gizli maliyet var mı?** (CB devre simülasyon engine + Rayline shape recognizer = 2 ayrı tech stack)
- **Tek prototipe odaklanmak (sadece CB veya sadece Rayline) daha rasyonel mi?**
- RIMA'nın mevcut 80 commit'lik altyapısı (top-down ARPG, Wang tilesetler, weapon attach, beat3commit) bu çift prototipte ne kadar kullanılabilir? **Yüzde ver.**

### 2. RIMA-LB Pivot vs Yeni Proje
RIMA zaten Hades-style top-down ARPG. Circuit Breaker da öyle.
- **RIMA'yı Circuit Breaker'a evrimleştirmek** (10 sınıfı 3 sınıfa düşür + devre forge ekle) bağımsız yeni prototipten daha iyi olur mu?
- Yoksa RIMA'yı bitir (Faz 1 MVP) sonra CB'ye başla mı?
- Bu iki yolun "psikolojik" maliyeti farklı (RIMA pivot = yatırım koruma, yeni proje = temiz başlangıç) — hangisi daha sağlıklı?

### 3. Yeni S Adayların (Null Directive / Goldweave Core) Durumu
Sentezde "backlog sezon 2" denmiş. Bunlar gerçekten S aday mı, yoksa Codex+Gemini'nin tek sinyali yeterli değil mi?
- **Null Directive boolean operatör** action-RPG bağlamında oyuncu hızı/anlama eğrisini düşürür mü?
- **Goldweave Core graph teorisi** PoE-pasif-ağacı çağrışımı çok güçlü değil mi? (rip-off riski)

### 4. Studio Türü Stratejisi
Solo dev için strateji önerin:
- **Plan X: Tek tür stüdyo** (cozy sim VEYA logic-ARPG, 2-3 oyun aynı türde sıralı) — pipeline compound returns
- **Plan Y: İkili portfolio** (cozy + logic-ARPG paralel sezon) — risk hedge, yorgunluk hedge
- **Plan Z: Kademeli** (RIMA bitir → cozy sim → logic-ARPG) — kademeli güven inşa

Gerçek emsallere referans ver:
- Lucas Pope (Papers Please → Obra Dinn) tek tür mü?
- Edmund McMillen (Meat Boy → Isaac → Bum-bo) tek tür mü?
- Toby Fox (Undertale → Deltarune) tek tür mü?
- Eric Barone (Stardew Valley 8 yıl tek oyun) tek tür mü?

### 5. Identity Kriz Feedback
Kullanıcı "yapamayacak mıyım" sorusu samimi. Sentez 22 konsept ile cevap veriyor ama bu konseptlerden biri kullanıcıya gerçekten ölçülebilir milestone vermez ise yine aynı kriz patlar.
- **CB Arcwright 10 iş günü spike'ı** somut bir milestone mu, yoksa "büyük tasarım"a kaçış mı?
- 10 iş günü sonunda "ilk node'u vurdum tüm oda devreye döndü" 5 sn'de okunmazsa ne yapılacak? Sentez bu fallback'i tanımlamış mı?

## Format

Türkçe, terse, brutal honest. Maks ~800 satır. Ham tablo + verdict.

## Yasak

- 22 konsepti tekrar tier'lama (sentez yapıldı, geri gitme)
- Yeni konsept üretme
- "Hepsi iyi" yanıtı (review = en zayıf noktaları çıkar)
