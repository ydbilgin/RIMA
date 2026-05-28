# TASK: 2D oyunlarda ele eklenen silah teknikleri (Antigravity/agy)

ACTIVE RULES: (1) think before answering (2) min output, no speculation — kanitlanmamis iddiayi "DOGRULANMADI" isaretle (3) sadece istenen arastirma (4) bulamazsan BLOCKED yaz.

Amac: 2D oyunlarda (top-down / 8-yonlu pixel-art ARPG) karakterin ELINE silah ekleme tekniklerini arastir. RIMA: silahsiz body (8 yon baked) + HandAnchor child SpriteRenderer + OrientationSync (silah tek sprite; yone gore konum/aci/sort). Bu yaklasimi dogrula ya da daha iyisini bul.

Web erisimin var — kullan. Anahtarlar: "2D top-down weapon attachment hand anchor 8 direction", "Unity sprite weapon socket per direction", "Spine DragonBones weapon bone", "pixel art equip weapon layering", oyun ornekleri Hades/CrossCode/Children of Morta/Enter the Gungeon/Brotato.

INLINE cevap ver, dosyaya YAZMA (~/.gemini scratch dahil). ASCII kullan.

## Cevaplaman gerekenler

### A. Ana teknikler
1. Ayri silah sprite + hand socket (RIMA'nin yaptigi): per-direction konum/aci/sort nasil?
2. Body'e bake (her yon ayri cizim): ne zaman, maliyet?
3. Skeletal/bone (Spine, DragonBones, Unity 2D IK): hand bone socket — 2D ARPG'de yaygin mi?
4. Hibrit per-frame hand anchor data (her frame elin pixel koordinati).

### B. 8-yon / top-down ozelinde
1. Per-direction offset tablosu mu, per-frame anchor mi?
2. Z-sorting: kuzeye bakinca silah arkada — nasil?
3. Sol/sag: flip mi rotation mi? Bicak "ters durma" cozumu?
4. Saldiri swing'inde el hareketini silah nasil takip eder?

### C. RIMA icin cikarim
- HandAnchor + OrientationSync (8 explicit offset+rotation+sort) endustriyle uyumlu mu? Risk/eksik?
- Somut iyilestirme (per-frame anchor, W yonu weapon flip, two-hand grip).

## Cikti
Basliklar A/B/C. Her kritik iddiaya kaynak URL. Oyun ornekleriyle somutlastir. Dogrulayamadigini "DOGRULANMADI" yaz.
