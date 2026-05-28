# TASK: 2D oyunlarda ele eklenen silah teknikleri arastirmasi (Codex)

ACTIVE RULES: (1) think before answering (2) min output, no speculation — kanitlanmamis iddiayi "DOGRULANMADI" diye isaretle (3) sadece istenen arastirma (4) bulamazsan BLOCKED yaz.

**Amac:** 2D oyunlarda (ozellikle top-down / 8-yonlu pixel-art ARPG) karakterin ELINE silah ekleme/takma tekniklerini arastir. RIMA mimarisi: silahsiz body (8 yonlu baked sprite) + HandAnchor child SpriteRenderer + OrientationSync (silah tek sprite, yone gore konum/aci/sort). Bu yaklasimi DOGRULA veya daha iyi alternatif bul.

Web erisimin var — kullan. Arama anahtarlari: "2D top-down weapon attachment hand anchor", "8 direction weapon sprite layering", "Unity sprite weapon socket per direction", "pixel art character weapon mount", "Spine/DragonBones weapon attachment", oyun ornekleri (Hades, CrossCode, Children of Morta, Enter the Gungeon, Brotato).

## Cevaplaman gerekenler

### A. Ana teknikler (kategorize et)
1. **Ayri silah sprite + hand anchor/socket** (RIMA'nin yaptigi): silah body'den ayri, ele/socket'e mount. Per-direction nasil konumlanir/donur/sort edilir?
2. **Body'e bake** (silah karakter sprite'inin parcasi, her yon/frame ayri cizilir): ne zaman tercih edilir, maliyeti?
3. **Skeletal / bone-based** (Spine, DragonBones, Unity 2D IK): bone'a silah parent'lama, hand bone socket. 2D ARPG'de yaygin mi?
4. **Hibrit** (per-frame hand anchor data — her animasyon frame'inde elin pixel koordinati kayitli, silah ona snap).

### B. 8-yon / top-down ozelinde
1. Silah her yon icin nasil konumlanir? Per-direction offset tablosu mu, per-frame anchor data mi?
2. Z-sorting: silah ne zaman body'nin onunde/arkasinda (kuzeye bakinca arkada)? Nasil cozuluyor?
3. Sol/sag yon icin silah flip/mirror mi yoksa rotation mi? Bicak "ters durma" problemi nasil cozuluyor?
4. Saldiri swing'inde el hareket ederse silah nasil takip ediyor (per-frame anchor anim)?

### C. RIMA icin cikarim
- RIMA'nin HandAnchor + OrientationSync (8 explicit offset+rotation+sort) yaklasimi endustri pratigiyle uyumlu mu? Eksik/risk var mi?
- Somut iyilestirme onerileri (ornek: per-frame anchor, weapon flipX W yonu icin, two-hand grip).

## Cikti
Inline dondur (dosyaya YAZMA). Basliklar A/B/C. Her kritik iddiaya kaynak URL. Oyun ornekleriyle somutlastir. Dogrulayamadigini "DOGRULANMADI" yaz.
