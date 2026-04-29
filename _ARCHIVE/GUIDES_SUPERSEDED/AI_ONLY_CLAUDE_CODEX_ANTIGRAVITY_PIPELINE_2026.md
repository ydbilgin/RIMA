# AI-Only Pipeline (Claude + Codex + Antigravity) - 2026

Bu rehber su soruya cevap verir:
- PixelLab olmadan ilerlenir mi?
- Blender bilmeden 2D ve 3D uretim yapılır mı?
- 128px'e kilitlemek zorunda misin?

## 1) En Net Cevap

- Evet, PixelLab olmadan da ilerlenir.
- Evet, Blender bilmeden de cikis alinabilir.
- Hayir, pixelart yapmiyorsan her seyi 128px'e kilitlemek dogru degil.

Dogru mantik:
- `Master asset` yuksek cozunurlukte uretilir.
- Oyuna girerken hedef platforma gore optimize edilir.

## 2) Cozunurluk Karari (Pixelart degilsen)

128 sadece runtime hedef olabilir; source icin kucuk kalir.

Onerilen:
- Karakter master frame: `512x512` (minimum) veya `768x768`
- UI/portrait/concept: `1024+`
- Runtime export:
  - PC: 256 veya 384 karakter frame
  - Mobile: 128 veya 192 (profiling sonucuna gore)

Kural:
- Once kaliteyi masterda koru.
- Sonra kontrollu downscale yap.

## 3) Sadece AI Tool + Claude + Codex Ile Uretim

## 3.1 Roller
- `Antigravity`: Goruntu/video uretimi ve artistik secim.
- `Claude`: Uretim sozlesmesi, prompt stratejisi, sprint kontrolu.
- `Codex`: Otomasyon, donusum, QC, Unity entegrasyon scriptleri.

## 3.2 PixelLab'siz 2D Pipeline
1. Karakter kimligi kilitlenir:
   - siluet, renk paleti, silah orani, kamera acisi.
2. AI ile 8 yon idle key pose seti uretilir.
3. AI edit ile run/attack key pose'lar uretilir.
4. Codex otomasyon:
   - dosya naming standardi
   - frame sequence duzeltme
   - 512->(256/128) batch export
   - Unity import preset (PPU/pivot/filter/compression)
5. Unity'de animator map + transition test.

Risk:
- AI frame-to-frame drift.
Karsi cozum:
- Her anim once key pose tabanli gitmeli, direkt uzun loop generate etme.

## 3.3 Blender bilmeden 3D->2D yaklasimi

Blender yoksa da alternatif var:
- AI image -> AI 3D tool (mesh) -> auto rig -> preset anim -> 8 yon render/bake.
- Render/bake kisminda DCC bilmek avantajli ama zorunlu degil; hazir web tool zinciriyle cikis alinabilir.

Yine de bil:
- "One click 3D" ile mesh temizligi her zaman iyi gelmez.
- Codex ile otomatik kontroller kurulmazsa kalite tutarsiz olur.

## 4) Surdurulebilirlik: Video -> Sprite dogrudan kullanilir mi?

Kisa cevap: ana gameplay icin tek basina guvenli degil.

Neden:
- frame jitter
- oran kaymasi
- ayak temas noktasi drift
- state gecislerinde snap

Dogru kullanim:
- Video: trailer, cutscene, VFX overlay.
- Gameplay loop: sprite sheet veya rig kaynakli kontrollu cikti.

## 5) Akici Oyun Icin Zorunlu Teknikler

1. Sabit kurallar:
- PPU sabit
- pivot sabit
- direction map sabit
- naming sabit

2. State gecis:
- `idle -> run_start -> run`
- `run -> run_stop -> idle`

3. 8 yon input map:
- `S+D=SE`
- `S+A=SW`
- `W+D=NE`
- `W+A=NW`

4. QC automation (Codex):
- missing frame kontrolu
- scale drift kontrolu
- pivot drift kontrolu
- alpha/background hatasi kontrolu

## 6) 30 Gunluk Pratik Plan (Blender bilmeden)

Hafta 1:
- Style bible + character spec lock
- 1 karakter idle/run temel set

Hafta 2:
- Attack + hit + death set
- Codex batch importer + naming validator

Hafta 3:
- Environment + parallax layer set
- Performans profil (PC + mobile hedef)

Hafta 4:
- Cleanup + bir karakter daha ayni pipeline ile
- Pipeline dokumani lock

## 7) Minimum Tool Set (Senin Durumuna Uygun)

- Uretim: Antigravity + ChatGPT Images
- Otomasyon/QC: Codex
- Plan/Prompt/Orkestrasyon: Claude
- Opsiyonel cleanup: Aseprite (sadece final fix)

Bu setle Blender bilmeden de cikis alirsin.
Ama uzun vadede bir noktada 3D ve animation kontrolu icin temel DCC bilgisi (veya guvenilir partner) kaliteyi ciddi artirir.

## 8) Nihai Karar Cizgisi

- "Hizli cikar, sonra toparlarim": Tam AI 2D.
- "Uzun vadede tutarli ve teknik borcu az olsun": Hibrit (AI + otomasyon + kontrollu cleanup).
- "En tutarli anim ve aci kontrolu": 3D taban + 2D bake (Blender bilgisi olmasa da arac zinciriyle baslatilabilir).

