# Shadowblade — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> karakter sayfasi -> Add Animation -> **Custom Animation V3**
- **YASAK**: Standalone "Animate with Text NEW" | `animate_character` MCP | Preset butonlar
- **Start Frame kaynagi**: `Characters/anchors/shadowblade/rotations/<direction>_clean.png` (Eraser Pass sonrasi)
- **Canvas**: 252x252 (v3 otomatik)
- **Yon uretimi**: 8 yon HEPSI ayri uretilir, FLIP YOK.
  Yonler: `south`, `south-west`, `south-east`, `east`, `north-east`, `north`, `north-west`, `west`
- **Frame kurallari**:
  - Idle: 6-8 frame | Keep First: ON
  - Hurt: 4 frame TAM SAYI | Keep First: ON
  - Death: 6-8 frame | Keep First: **OFF**
  - Walk interpolation: 6 frame | Keep First: ON
  - Attack PEAK: 4 frame | Keep First: **OFF**
  - Attack Windup: 4 frame | Keep First: ON | Start=clean, End=PEAK
  - Attack Follow: 4 frame | Keep First: ON | Start=PEAK, End=clean
  - Toplam attack unique: 8 frame
  - Dash: 4 frame | Keep First: ON

---

## KARAKTER GORSEL OZETI (uymak ZORUNLU)

- **Silah**: Her iki elde mor/violet **hancer**. Kisa ince bicak (~karakter boyu %35). Hafif one + asagi yonlu tutus.
- **Kiyafet**: Koyu lacivert / indigo kiyafet (pantolon + ceket, gevsek ama duzgun). Hood/baslik YOK.
- **Karakter**: Erkek, koyu deri, siyah kisa sac.
- **DURUS**: **DIK ve hazir**. Karakter dik duruyor, hafif one egim.
- **YOK**: derin predatory comelme, hood, pelerin, uzun kilic, tek hancer.

---

## ERASER PASS (ZORUNLU)

8 yon icin Pixelorama:
1. `Characters/anchors/shadowblade/rotations/<direction>.png` ac.
2. Magic Wand ile yesil bg sec, sil.
3. Disindaki kalintilar -> Eraser.
4. **Iki hanceri de silme** -- ikisi de karakter parcasi.
5. Kaydet -> `<direction>_clean.png`.

---

## YON REFERANS TABLOSU (cift hancer)

| Yon | Sag hancer | Sol hancer |
|---|---|---|
| south | Screen-left | Screen-right |
| south-east | Screen-left, hafif one | Screen-right |
| east | One (prominent) | Gizli/arka |
| north-east | Screen-right | Screen-left |
| north | Screen-right | Screen-left |
| north-west | Screen-right | Screen-left, hafif one |
| west | Gizli/arka | One (prominent) |
| south-west | Screen-left | Screen-right |

> Diagonaller cardinal'lar arasi interpolasyon — yakin cardinal'a daha yakin yorumla.

---

## ADIM 1 -- Idle

**Settings**: 7 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, idle ready stance. UPRIGHT posture with slight
forward lean, NOT a deep crouch. Both hands hold violet daggers in a
ready guard, blades angled slightly forward and down. Weight shifts
slowly between feet. Head sweeps subtly, eyes hidden in shadow. Dark
indigo outfit (pants + jacket) breathes with quiet motion. Threatening
calm — minimal motion, controlled tension. No blade swing, no foot lift.
```
**DIRECTION NOTE**: Yon tablosundan hangi hancerin one cikip hangisinin gizlendigini ekle.
  Ornek (east): "Right dagger forward and prominent on screen, left dagger tucked behind body."

---

## ADIM 2 -- Hurt

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, 4-frame reflex defensive reaction. Frame 1: neutral
ready. Frame 2: both daggers cross in front of chest in instant defensive
X-guard, body recoils backward. Frame 3: head bows slightly, weight back,
guard held high. Frame 4: recovery, daggers lower back to ready stance.
Sharp reflexive motion, no flailing. Dark indigo fabric shifts with
recoil.
```
**DIRECTION NOTE**: X-guard her yonde govdenin onunde olusur; tablodaki goruntu pozisyonu hicbir zaman karakteri arkadan vurmaz.

---

## ADIM 3 -- Death

**Settings**: 7 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, death sequence. Frame 1: stagger. Frame 2: both
daggers slip from hands SIMULTANEOUSLY, falling to either side. Frame
3-4: upright stance fully breaks down, knees buckle. Frame 5-6: torso
collapses forward, weight surrenders. Frame 7: body settles, head down.
Quiet weight-driven collapse, NO drama, NO theatrics. The unshakeable
stance finally breaks.
```
**DIRECTION NOTE**: Sag hancer yon tablosundaki sag-tarafa, sol hancer sol-tarafa duser.

---

## ADIM 4 -- Walk Cycle (3 alt-adim)

### 4a -- PoseA secimi
1. Standalone Animate with Text NEW -> 12 frame walk uret.
2. En uc stride pozunu sec.
3. Eraser pass -> `outputs/walk/<direction>/PoseA_clean.png`.

### 4b -- PoseB (stride flip)
- Aseprite/Pixelorama -> PoseA_clean -> Horizontal Flip.
- Stride parity flip.
- Kaydet -> `PoseB_clean.png`.

> Karakter cift hancer tasidigi icin flip silah simetrisini bozmaz; sag/sol etiketler stride parity icindir.

### 4c -- Walk interpolation
**Settings**: 6 frame | Keep First: ON | Start=PoseA | End=PoseB
```
High top-down view, silent controlled walk. Both daggers held in ready
position close to the body, blades angled forward and down. Foot
placement quiet, implied minimal noise. Torso STAYS STABLE while legs
do the work — the upper body barely bobs. Indigo fabric has minimal
sway. UPRIGHT posture maintained throughout, no slouch, no crouch.
```
**DIRECTION NOTE**: Iki hancer tablodaki tarafta kalir; yuruyus sirasinda guard bozulmaz.

---

## ADIM 5a -- Attack LMB (Fast Slash) -- 3 segment

### 5a.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, fast right-hand horizontal slash, peak on final
frame. Frame 4 (PEAK): right arm fully extended in horizontal cut,
violet dagger has crossed body silhouette to the opposite side. Left
hand stays in guard position with its dagger in front of chest — the
counter. Body twist 20 degrees into the cut, weight on leading foot.
Sharp deliberate motion. NO trail VFX, NO blur.
```
PEAK -> `outputs/attack_lmb/<direction>/PEAK.png` (Eraser pass).

### 5a.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Right arm coils back, elbow pulls behind body, right dagger angles
back. Left dagger raises into guard in front of chest. Body weight
loads onto back foot. Tight compact wind-up.
```

### 5a.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
Right arm continues across body to opposite side then snaps back to
ready guard. Body untwists. Both daggers return to ready position.
Quick recovery, no drift.
```
**DIRECTION NOTE**: PEAK'te sag hancer tablodaki "sag hancer" tarafinin **karsi** tarafina gecmis olur; sol hancer kendi tarafinda guard'da kalir.

---

## ADIM 5b -- Attack RMB (Thrust) -- 3 segment

### 5b.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, left-hand straight thrust attack, peak on final
frame. Frame 4 (PEAK): left arm fully extended forward in linear
thrust, violet dagger pointing directly at target line. Right arm
extended forward as feint/decoy with right dagger reaching outward.
Left hip pushed forward, weight committed left and forward. Body
forms a piercing arrow shape. NO impact VFX.
```
PEAK -> `outputs/attack_rmb/<direction>/PEAK.png`.

### 5b.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Left dagger rotates inward, point lined up with target. Right dagger
extends slightly forward as decoy. Hips coil left, weight loads onto
back foot. Compact loading, no telegraphed pull.
```

### 5b.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
Left arm retracts from thrust back toward chest guard. Right dagger
returns to ready position. Weight rebalances between feet. Body
returns to upright ready stance.
```
**DIRECTION NOTE**: Thrust ekseni karakterin one yonu boyunca; sol hancer one cikar, sag hancer feint olarak yon tablosundaki kendi tarafini destekler.

---

## ADIM 6 -- Dash

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, fast shadow-step dash. Frame 1: launch. Frame 2:
peak forward lean (~25 degrees), BOTH daggers tucked close to body
along the forearms (parallel tuck), low silhouette. Frame 3: front
foot lands. Frame 4: recovery to upright ready stance. Indigo fabric
streams behind. Daggers stay secured throughout.
```
**DIRECTION NOTE**: Dash karakterin baktigi yone dogru. Hancerler vucuda yapisik tuck pozisyonunda; tablodaki taraf bilgisi tuck sirasinda gecerli degildir.

---

## ADIM 7 -- Weapon Pass (Edit Image Pro)

**Prompt**
```
Refine the twin daggers, one in each hand. Each dagger: short straight
blade, length about 35% of character height. Dark steel body #4A4E5A,
violet glowing edge / crystal #5A2A8A with brighter rim #7A3AB0.
Hilt: dark leather wrap #1A0E2A. Both daggers identical mirrored
geometry. Maintain consistent silhouette across all frames of this
direction. No motion trails, no glow halo, no sparks.
```
Frame -> Eraser pass -> kaydet.

---

## QC CHECKLIST

- [ ] IKI hancer var mi (her elde bir tane)?
- [ ] Hancer boyu ~karakter boyu %35 mi (uzun kilic REJECT)?
- [ ] Blade rengi violet/mor mu?
- [ ] DIK durus mu (deep crouch REJECT)?
- [ ] Hood/baslik eklenmemis mi?
- [ ] Indigo/koyu lacivert kiyafet korunmus mu?
- [ ] Yon tablosundaki hancer pozisyonlari dogru mu?
- [ ] Idle: hancerler ready guard'da hafif one+asagi mi?
- [ ] Frame sayisi tam mi?
- [ ] Eraser pass yapildi mi?
- [ ] Karakter canvas merkezinde mi?

---

## KAYIT KLASORU

```
Characters/anchors/shadowblade/
  rotations/
    south_clean.png, ... (8 yon)

PIXELLAB_OUTPUTS/shadowblade/outputs/
  idle/<direction>/frames/
  hurt/<direction>/frames/
  death/<direction>/frames/
  walk/<direction>/PoseA_clean.png
  walk/<direction>/PoseB_clean.png
  walk/<direction>/frames/
  attack_lmb/<direction>/PEAK.png
  attack_lmb/<direction>/windup/
  attack_lmb/<direction>/follow/
  attack_rmb/<direction>/PEAK.png
  attack_rmb/<direction>/windup/
  attack_rmb/<direction>/follow/
  dash/<direction>/frames/
```

8 yon x 7 animasyon = 56 set. Attack unique frame = 8.
