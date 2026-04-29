# Iris Ogli Kanali 2D/AI Pipeline Arastirmasi (2026)

Bu dokuman, `https://youtu.be/vOvYazUBlpQ` videosunu merkez alir, ayni kanaldaki ilgili videolari inceler, ve bunu RIMA uretim akisina (PixelLab + Aseprite + Unity) nasil uyarlayabilecegimizi netlestirir.

## 1) Kisa Sonuc

- Evet, kanalin 2026 videolari ise yarar.
- En yuksek degerli iki icerik:
  - `How to Build a 2D Game Asset Pack with Free AI` (2026-02-18)
  - `Create a FREE 2D Parallax System Using AI` (2026-03-03)
- 3D tarafina kayan videolar (image->3D->Mixamo) bu sprint icin ana hat olmamali; uzun vadeli alternatif hat olarak durabilir.
- Bizim stack icin en dogru cekirdek:
  - Karakter/anim: PixelLab + Aseprite cleanup + Unity 8-direction state/blend
  - Cevre: AI uretim + layer split (far/mid/foreground) + Unity parallax
  - QC: PPU, pivot, frame alignment, direction mapping, transition snap kontrolu

## 2) Incelenen Kaynaklar

### Ana Video
- `https://youtu.be/vOvYazUBlpQ`
  - Baslik: **How to Build a 2D Game Asset Pack with Free AI**
  - Yayin: **2026-02-18**
  - Vaat: ucretsiz AI araclariyla karakter + anim + cevre + UI dahil game-ready asset pack.

### Kanalda Ek Olarak Incelenen Ilgili Videolar
- `https://youtu.be/4_u5Ues1zPY`
  - **Create a FREE 2D Parallax System Using AI ( Game Ready )**
  - Yayin: **2026-03-03**
- `https://youtu.be/M1dWBasJqqY`
  - **Turn Any Image Into a 3D Animated Character (FREE AI Tools)**
  - Yayin: **2026-03-01**
- `https://youtu.be/iqn0g-RYNx0`
  - **Getting that 2D look in 3D**
  - Yayin: **2022-12-27**
- `https://youtu.be/qHlhWNk97IA`
  - **2D / 3D Game Art**
  - Yayin: **2020-10-30**
- `https://youtu.be/WNaWjcQZcYI`
  - **From ZBrush to Unity - Building My First Game Character**
  - Yayin: **2021-04-01**

### Kanal Site Icerikleri (workflow ozetleri)
- `https://irisogli.com/2d-game-assets-pack`
- `https://irisogli.com/parallax-background-creation`
- `https://irisogli.com/how-to-turn-any-image-into-a-3d-animated-character-free-ai-tools`

## 3) vOvYazUBlpQ Videosu: Bizim Icın Ne Diyor?

Videonun ana fikri, tek tek asset yerine paket mantigi:
- Karakter
- Animasyon
- Platform/background
- UI
- Motora hazir set

Bizim projedeki karsiligi:
- Karakter anim uretimi zaten var.
- Eksik kritik nokta: **uretim sozlesmesi + QC standardi**.
  - Ayni PPU
  - Ayni pivot referansi
  - Ayni direction mapping
  - Snapsiz transition

Bu videodan alinacak en net prensip:
- **Prompttan once production spec kilitlenmeli.**

## 4) Kanal Videolari: Deger Siralamasi

### A) Dogrudan yuksek deger (hemen uygulanir)

1. `https://youtu.be/vOvYazUBlpQ`
- Neden: Tum pipelinei tek pakette dusunuyor (character+anim+env+UI).
- Bizde etkisi: dağinik asset uretimini standardize eder.

2. `https://youtu.be/4_u5Ues1zPY`
- Neden: 2D parallaxi katman mantigiyla net tarifliyor.
- Bizde etkisi: biome/background uretiminde hiz + kalite.

### B) Orta deger (ileri faz)

3. `https://youtu.be/M1dWBasJqqY`
- Neden: 2D image -> 3D animated akisi.
- Bizde yeri: ana gameplay degil; promo/cinematic/previs icin yan hat.

4. `https://youtu.be/iqn0g-RYNx0`
- Neden: 2D look in 3D estetik yaklasim.
- Bizde yeri: 2.5D hedef olursa faydali; saf pixel top-down icin simdilik secondary.

### C) Dusuk deger (simdilik arsiv)

5. `qHlhWNk97IA`, `WNaWjcQZcYI` ve eski Maya/ZBrush agirlikli videolar
- Neden: ilham veriyor ama mevcut 2D production bottleneckini direkt cozmuyor.

## 5) 2026 Icin Bizim Stacke Uygun 2D Pipeline

### 5.1 Uretim Sozlesmesi (sabit kurallar)
- Canvas: `128x128`
- Pivot: tum yonlerde ayni referans (ayak tabani merkez)
- PPU: tum karakter setlerinde sabit
- Palette lock: karakter bazli renk drift yasak
- Naming:
  - `idle_N, idle_NE, ...`
  - `run_N, run_NE, ...`
  - `run_start_*`, `run_stop_*`
  - `attack_light_*`, `attack_heavy_*`

### 5.2 Karakter Anim Uretimi
1. Idle 8 yon setini lock et.
2. Run uretimi:
  - Once 3 yon (E/SE/S) kalite onayi.
  - Sonra kalan 5 yon.
3. Warblade gibi idle/run pose farki olan karakter:
  - `run_start` zorunlu
  - `run_stop` zorunlu
  - Direkt idle<->run ancak testte snap yoksa kalir.

### 5.3 PixelLab Tarafi Pratik Kurallar
- `Keep first frame (idle pose)`:
  - Run loopte genelde kapali
  - Run_start uretiminde referansli kullanilabilir
- Frame sayisi:
  - Run: 6 veya 8 (8 daha smooth, maliyet daha yuksek)
  - Attack: 8-10 daha guvenli
- Input mutlaka temiz/transparent olmalı; backgroundli input gereksiz frame bozar.

### 5.4 Aseprite Cleanup
- 9 frame cikarsa ilk frame cop mu kontrol et; gerekirse at.
- Tum framelerde:
  - Karakter merkezi sabit
  - Ayak tabani ve silah ucu jitter kontrolu
  - BBox tasmasinda canvas sabit kalacak sekilde reposition

### 5.5 Unity Entegrasyon
- 8 yon mapping kesin:
  - `S + D` => `SE`
  - `S + A` => `SW`
  - `W + D` => `NE`
  - `W + A` => `NW`
- Flip ile fake yon yerine gercek 8 clip.
- Transition:
  - `idle -> run_start -> run_loop`
  - `run_loop -> run_stop -> idle`
- Test checklist:
  - Diagonal input dogru clipi tetikliyor mu?
  - Idle/run scale farki var mi?
  - Geciste snap var mi?

## 6) Kanal Akisini Elimizdekilerle Basitlestirme

Kanalda Mixboard/Grok/Blender geciyor. Bizde en az surtunmeli uygulama:

1. Karakter+anim: PixelLab + Aseprite cleanup
2. Environment/parallax: AI image + manual layer split + Unity parallax
3. UI: once manuel minimal set (AI sadece concept)
4. Otomasyon: import preset + naming validator + hizli QC checklist

Neden:
- Tool sayisi arttikca stil drift ve revizyon maliyeti artiyor.
- En buyuk kazancimiz arac degil, **tutarlilik**.

## 7) Uygulanabilir Sprint Sirasi

1. Warblade run 8 yonunu ayni scale/pivot standardina getir.
2. `run_start` ve `run_stop` 8 yon setini tamamla.
3. Unity animatorda 8 direction mapping + transition baglarini test et.
4. 1 demo biome icin parallax katmanli background uret.
5. Import/QC checklistini sabitle (PPU, pivot, naming, snap test).
6. Ayni sablonu diger karakterlere kopyala.

## 8) Claude Icin Hazir Task Metni (Copy/Paste)

```md
TASK: RIMA 2D production pipelineini 2026 standardinda stabilize et.

GOAL:
- Warblade ve sonraki karakterlerde idle/run/attack uretiminde scale, pivot,
  direction ve transition tutarsizligini sifira indir.

RULES:
- 128x128 sabit canvas
- 8 gercek yon (flip ile sahte yon yok)
- run_start/run_stop Warblade icin zorunlu
- Mapping: S+D=SE, S+A=SW, W+D=NE, W+A=NW
- PPU ve pivot tum cliplerde sabit

EXECUTION:
1) Mevcut run setinde scale/pivot audit
2) Eksik yonleri tamamla
3) run_start/run_stop ekle
4) Animator transitionlarini test et
5) Snap/jitter yoksa lock et, varsa duzeltme turu ac

DELIVERABLE:
- Guncel clip listesi
- Animator state/transition ozeti
- Kalan riskler + net next step
```

## 9) Final Not

Bu kanalin 2026 icerikleri dogru yonde. Ama bizim basari kriterimiz:
- Ayni stil
- Ayni olcek
- Dogru yon
- Snapsiz gecis

Bu dortu kilitlersek pipeline surdurulebilir olur.

