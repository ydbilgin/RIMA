# Kullanici Aksiyon Rehberi
Guncelleme: 2026-04-22

## Oncelik 1 - Simdi Yap
### 1) Gemini kamera metni duzeltmesi (13 blok)
**Ne:** `CHARACTER_PROMPT_PIPELINE.md` icindeki kamera/metin bloklarini kilitli aciya gore tek tek duzelt.
**Nerede:** Metin edit (dosya), sonra Gemini cikti uretimi.
**Adimlar:**
1. `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/CHARACTER_PROMPT_PIPELINE.md` dosyasini ac.
2. Kamera acisi satirlarini karar kilidine gore birlestir: bas tepesi hakim, yuz/göz gorunmez kuralini net yaz.
3. Tum 13 blokta ayni standardi uygula (tek tek kontrol et).
4. Her blok icin bir test prompt calistirip hatali aci ureten bloklari not et.
5. Hatali bloklari revize edip final versiyonu kilitle.

### 2) 6 class Gemini yeniden uretim
**Ne:** Brawler, Summoner, Ronin, Gunslinger, Hexer, Ranger conceptlerini yeniden uret ve QC icin secili adaylari hazirla.
**Nerede:** Gemini + PixelLab referans hazirlama akisi.
**Adimlar:**
1. Her class icin tek hedefli prompt hazirla (karar belgesindeki kimlik kilitlerine sadik).
2. Class basina en az 3 aday concept PNG uret.
3. Her adayi su kriterlerle ele: silhouette okunurlugu, class kimligi, renk ayrimi.
4. Class basina en iyi 1 adayi sec ve `*_approved_candidate.png` gibi net isimle ayir.
5. Secilen adaylari PixelLab Create-from-Reference girisine hazir klasore koy.

### 3) Warblade run animasyon seti (8 yon, no mirror)
**Ne:** Kilitli V2 kurala gore Warblade run setini 8 yon icin tamamen direct uret.
**Nerede:** PixelLab (Animate with text NEW + Interpolate NEW) + Aseprite son duzen.
**Adimlar:**
1. Yalnizca `SE -> E -> S -> NE -> N -> SW -> W -> NW` sirasiyla ilerle.
2. Her yon icin keyframe A/B/C olustur (contact/passing/contact).
3. Animate with text NEW ile ana hareket bloklarini uret.
4. Interpolate NEW ile `A->B` ve `B->C` aralarini doldur.
5. Aseprite'de duplicate/padded frame temizligi yap, loop akisini sabitle.
6. Her yonu `warblade_run_<DIR>.png` adiyla export et.

### 4) Unity Inspector reset (RageSystem zorunlu)
**Ne:** Player objesindeki RageSystem degerlerini kilitli hedefe cek.
**Nerede:** Unity Editor -> `_IsoGame` sahnesi -> Player Inspector.
**Adimlar:**
1. `Assets/Scenes/_IsoGame.unity` sahnesini ac.
2. Hierarchy'de `Player` sec.
3. `RageSystem` componentinde su degerleri gir:
4. `ragePerHitDealt = 2`
5. `ragePerKill = 5`
6. `decayDelay = 1.5`
7. `decayPerSecond = 10`
8. Scene'i kaydet ve Play mode'da 1 oda test et.

## Oncelik 2 - Sirada
### 5) Warblade attack animasyon seti (3-segment)
**Ne:** Windup -> Impact -> Recovery yapisinda 8 yon attack setini uret.
**Nerede:** PixelLab + Aseprite.
**Adimlar:**
1. Her yon icin A (windup), B (impact), C (recovery) keyframe kilitle.
2. Animate with text NEW ile ana hareketi al.
3. Interpolate NEW ile gecisleri yumusat.
4. Warblade kilit constraint'leri tum denemelerde koru (iki el ayni hilt).
5. Aseprite'de PEAK frame tek kopya kalacak sekilde timeline birlestir.
6. `warblade_attack_<DIR>.png` olarak export et.

### 6) Eksik Faz 1 mob sprite uretimi
**Ne:** Hollow Mite ve The Wound icin gerekli anim setlerini tamamla.
**Nerede:** PixelLab + Aseprite.
**Adimlar:**
1. Hollow Mite icin idle/walk/death setlerini ayri ayri uret.
2. The Wound icin idle/pulse/death setlerini ayri ayri uret.
3. Her seti frame sayisi ve silhouette tutarliligina gore QC et.
4. Aseprite'de son cleanup yapip importa hazir sprite sheet cikar.

### 7) Unity Inspector manuel baglantilar (olum/restart + map fragment)
**Ne:** Faz 1 exit kriterleri icin inspector-level referans ve UI atamalarini tamamla.
**Nerede:** Unity Editor, ilgili scene objeleri ve UI prefablari.
**Adimlar:**
1. Death screen panelinin sprite/text alan referanslarini inspector'da bagla.
2. Restart butonu olayini sahne yeniden yukleme metoduna bagla.
3. Map fragment toplama objesi ile kapi acma/oda gecis tetiklerini inspector'da eslestir.
4. Oda temizligi -> fragment spawn -> fragment pickup -> sonraki oda zincirini manuel test et.

## Oncelik 3 - Bekleyebilir
### 8) DOTween HUD animasyonlari
**Ne:** HUD'da sade ama okunur tween animasyonlari ekle (zorunlu degil, polish).
**Nerede:** Unity Editor + DOTween setup.
**Adimlar:**
1. DOTween package'in projede aktif oldugunu dogrula.
2. Rage/HP dolumunda kisa easing animasyonu uygula.
3. Boss HP bar acilis-kapanisinda hafif tween kullan.
4. Asiri hareketten kacinip combat okunurlugunu koru.

### 9) Faz 1 cikis kriteri playtest checklist'i
**Ne:** Faz 1 tamamlama kriterlerini manuel test ile tikla.
**Nerede:** Unity Play mode.
**Adimlar:**
1. Warblade ile 8-9 oda run denemesi yap.
2. Her oda sonrasi skill draft'in acildigini dogrula.
3. Elite oda affix spawn davranisini kontrol et.
4. Bossun %50 HP break davranisini dogrula.
5. Olum ekrani + restart + map fragment gate akisini tek run'da tekrar kontrol et.

## Karar Bekleyen Sorular
### 10) Acik tasarim sorusu var mi?
**Ne:** Bu turde okunan dosyalarda Claude'un acik bir USER_DECISION sorusu net formatta listelenmemis.
**Nerede:** N/A
**Adimlar:**
1. Yeni bir karar sorusu acildiginda bu bolume ekle.
2. Soruya "karar tarihi + karar no" formatiyla cevap yaz.
