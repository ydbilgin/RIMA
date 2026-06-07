# GÖREV: Batch-3 DÜZELTME TURU — 3 FAIL regen + magenta saçak temizliği + DÜRÜST yeniden-QC

Klasör: `STAGING/imagegen/assets/batch3_2026-06-07/`. Önceki turda self-QC "hepsi PASS" dedi ama Opus denetimi 3 KESİN FAIL buldu — bu tur QC kriterleri sıkı.

## A) YENİDEN ÜRETİLECEK 3 DOSYA (kök neden: ince/seyrek içerik magenta-chroma'da yok oluyor / model yazı üretiyor)
1. `ground_decal_combat_scratches.png` (64×32) — ÖNCEKİ SORUN: neredeyse boş. YENİ YAKLAŞIM: çizikleri KALIN ve YOĞUN üret (3-5 belirgin koyu çizik kümesi, hafif kenar aydınlığı), magenta yerine DÜZ KOYU GRİ (#3A3A3A) zemin üzerinde üret → zemin rengini chroma-key'le çıkar (koyu gri tek ton).
2. `ground_decal_portal_scorch.png` (128×64) — aynı sorun. YENİ: belirgin radyal yanık izi (koyu merkez + cyan kıvılcım noktaları), aynı koyu-gri zemin tekniği.
3. `hole_rim_glow.png` (64×32) — ÖNCEKİ SORUN: model YAZI üretti ("CURVED CORNER TRIM"). YENİ PROMPT'TA: "no text, no letters, no labels" AÇIKÇA yaz; içerik = kırık taş kenar şeridi + alt kenarında ince cyan ışıma.

## B) MAGENTA SAÇAK TEMİZLİĞİ (tüm transparan asset'ler)
`distant_island_silhouette_01/02/03` ve diğer transparan PNG'lerin kenarlarında magenta fringe var. Python/PIL ile defringe geç: alpha kenarındaki magenta-kontamine pikselleri komşu renge çek veya alpha'ya yut (yarı saydam magenta piksel = ya temizle ya tam saydam yap). pixel-cleanup aracın varsa onu kullan; yoksa basit defringe script'i yaz. HER dosyayı işle.

## C) DÜRÜST YENİDEN-QC (22 dosyanın TAMAMI)
Her dosyayı AÇ ve 4× yakınlaştırılmış bak. Kriterler: (1) içerik İSTENEN ŞEY Mİ (boş değil, yazı değil), (2) magenta artığı SIFIR, (3) boyut doğru. Tablo: dosya · PASS/REGEN_DONE/NEEDS_REVIEW + tek satır gözlem. "PASS" yazmadan önce GERÇEKTEN bakmış ol — önceki turun hatası tekrarlanmayacak.

## ÇIKTI
manifest.md güncelle + batch3_contact_sheet.html yenile + stdout'a final QC tablosu.
