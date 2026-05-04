# Warblade 4-Diagonal Animasyon Uretim Rehberi

Bu dosya Warblade animasyonlarini PixelLab web UI icinde manuel uretmek icin
kullanilacak Turkce yonlendirme rehberidir.

Not: Proje ici md dosyalari ASCII kalmali. Bu yuzden Turkce karakterler
diacriticsiz yazildi.

---

## 1. Ana Karar

Warblade Hades'e daha yakin 4-diagonal gorsel yonle calisir. Bu Hades'in
internal kodu oldugu iddiasi degildir; goruntu davranisi icin pratik RIMA
kararidir: hareket yonu izometrik kameraya gore 4 ara/quadrant yonden birine
snap olur, durunca son net yon korunur.

Kurallar:

- Sola kosarsa son dikey niyete gore `run_SW` veya `run_NW`, durunca ayni idle.
- Saga kosarsa son dikey niyete gore `run_SE` veya `run_NE`, durunca ayni idle.
- Yukari-sola kosarsa `run_NW`, durunca `idle_NW`.
- Asagi-saga kosarsa `run_SE`, durunca `idle_SE`.
- Saf yukari/asagi hareket son yatay niyeti korur; karakter anlamsiz front/back
  cardinal state'e gecmez.
- Input noise/deadzone altinda yon degismez.

| Gorsel yon | Unity suffix | PixelLab anchor |
|---|---|---|
| South-east | `_SE` | `Characters/anchors/warblade/rotations/south-east.png` |
| North-east | `_NE` | `Characters/anchors/warblade/rotations/north-east.png` |
| North-west | `_NW` | `Characters/anchors/warblade/rotations/north-west.png` |
| South-west | `_SW` | `Characters/anchors/warblade/rotations/south-west.png` |

Ana runtime facing sadece bu 4 diagonal yondur. `S/E/N/W` cardinal state'leri
production run pipeline'inda kullanilmaz.

Unity isim standardi:

```text
warblade_idle_SE.anim
warblade_run_SE.anim
warblade_attack_basic_1_SE.anim
warblade_run_NW.anim
warblade_attack_basic_1_NW.anim
```

---

## 2. PixelLab Temel Ayarlari

- Tool: `Animate with Text NEW` yani animate-with-text-v3.
- Canvas: `252 x 252`.
- Frame count: `8`.
- Sonra gerekli kliplerde `Interpolate NEW` / interpolation-v2 kullan.
- Character animasyonu icin `animate_character` MCP kullanma.
- VFX'i karakter frame'ine gomdurme. Impact spark, projectile, trail ve shockwave
  ayri VFX olarak uretilmeli.

Neden 252px:

```text
252 * 252 * 8 = 508,032
PixelLab limit: 524,288
```

Warblade'in uzun kilici 128px canvas'ta kirpiliyor. 252px keyframe uretimi daha
guvenli. Unity importta son sheet 128x128 hucre standardina oturtulacak.

---

## 3. Once Run Uret

Walk yok. Hades hissi icin ana hareket `run` olacak.

Uretim sirasi:

1. Ilk once 4 diagonal run clip'i uret: `SE`, `NE`, `NW`, `SW`.
2. Her yon icin ilgili anchor'i yukle.
3. Animate with Text NEW ile 8 frame al.
4. Tek bir frame bile bozuksa interpolate yapmadan once reroll yap.
5. 8 frame temizse adjacent frame'ler arasina Interpolate NEW uygula.
6. Final siralama:

```text
1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8
```

Beklenen sonuc: 8 keyframe -> 14-15 efektif frame.

Run kotu cikarsa:

1. Sadece iyi bir extreme run pose uret.
2. High-knee / long-step pozunu sec.
3. Gerekirse karsi extreme pose'u ayri uret veya elde duzelt.
4. Iki extreme pose arasinda Interpolate NEW kullan.
5. Sonra Aseprite/Pixelorama temizligi yap.

---

## 4. Her Prompta Eklenecek Kilit Kurallar

Asagidaki blok her Warblade animasyon promptunda bulunmali:

```text
full body, centered, same scale as reference, no zoom-in
both hands on the same long sword hilt
right hand near the crossguard, left hand near the pommel
no projectile effects, no impact sparks, no energy trails
clean pixel clusters, no dithering, no blur
```

Yon cumleleri:

| Suffix | Prompt icinde kullan |
|---|---|
| `_SE` | facing down-right, moving down-right across the isometric floor |
| `_NE` | facing up-right, moving up-right across the isometric floor |
| `_NW` | facing up-left, moving up-left across the isometric floor |
| `_SW` | facing down-left, moving down-left across the isometric floor |

Compass kelimelerini tek basina kullanma. `walking north` veya `running north`
gibi ifadeler PixelLab'da ters veya yanlis yon uretebilir. Kamera goreli ifade
kullan.

---

## 5. Run Promptlari

### run_SE

```text
heavily armored warrior running down-right across an isometric stone floor,
heavy but responsive sprint, knees lifting, alternating arms and legs, large
two-handed sword carried low and stable across the body, armor plates swaying
with each step
```

Reference:
`Characters/anchors/warblade/rotations/south-east.png`

Frames: `8 + interpolate`

### run_NE

```text
heavily armored warrior running up-right across an isometric stone floor,
heavy but responsive sprint, back shoulder leading, knees lifting, large
two-handed sword carried low and stable across the body, armor plates swaying
with each step
```

Reference:
`Characters/anchors/warblade/rotations/north-east.png`

Frames: `8 + interpolate`

### run_NW

```text
heavily armored warrior running up-left across an isometric stone floor,
heavy but responsive sprint, back shoulder leading, knees lifting, large
two-handed sword carried low and stable across the body, armor plates swaying
with each step
```

Reference:
`Characters/anchors/warblade/rotations/north-west.png`

Frames: `8 + interpolate`

### run_SW

```text
heavily armored warrior running down-left across an isometric stone floor,
heavy but responsive sprint, knees lifting, alternating arms and legs, large
two-handed sword carried low and stable across the body, armor plates swaying
with each step
```

Reference:
`Characters/anchors/warblade/rotations/south-west.png`

Frames: `8 + interpolate`

---

## 6. Basic Combo Prompt Sablonu

Her yon icin 3 ayri clip uret:

- `attack_basic_1_<DIR>`: hizli thrust
- `attack_basic_2_<DIR>`: yatay cleave
- `attack_basic_3_<DIR>`: overhead slam

### attack_basic_1

```text
heavily armored warrior in <DIR PHRASE>, quick forward sword thrust, lead foot
steps into the strike, both hands keep the long hilt stable, blade extends then
retracts back toward idle stance
```

Timing:
`frame 1 idle, frame 3 anticipation, frame 4-5 full extension, frame 8 recovery`

Frames: `8 + interpolate`

### attack_basic_2

```text
heavily armored warrior in <DIR PHRASE>, wide horizontal two-handed sword cleave,
hips rotate, shoulders follow the blade, weight transfers through the front foot,
armor mass makes the swing feel heavy
```

Timing:
`frame 1 idle, frame 2-3 windup, frame 4-5 slash arc, frame 8 recovery`

Frames: `8 + interpolate`

### attack_basic_3

```text
heavily armored warrior in <DIR PHRASE>, raises the two-handed sword overhead
with both hands, plants both feet, slams the blade down in front with full body
weight, then settles into recovery
```

Timing:
`frame 1 idle, frame 2-4 overhead windup, frame 5-6 impact pose, frame 8 recovery`

Frames: `8 + interpolate`

---

## 7. Skill Prompt Sablonlari

### dash_<DIR>

```text
heavily armored warrior in <DIR PHRASE>, explosive short lunge in the facing
direction, body leans hard into the dash, sword trails behind but stays gripped
with both hands, feet slide then plant
```

Frames: `8 + interpolate`

### gravity_cleave_<DIR>

```text
heavily armored warrior in <DIR PHRASE>, leaps forward with sword raised overhead,
hangs briefly at peak height, descends into a heavy blade slam, full body weight
behind the impact, returns toward ready stance
```

Frames: `8 + interpolate`

### war_stomp_<DIR>

```text
heavily armored warrior in <DIR PHRASE>, braces with two-handed sword, raises one
armored boot, slams the boot down hard, torso compresses with the impact, returns
to ready stance
```

Frames: `8 + interpolate`

### iron_charge_<DIR>

```text
heavily armored warrior in <DIR PHRASE>, lowers shoulder and charges forward,
two-handed sword held back for impact, heavy armor momentum carries the body,
ends in a short stopping slam
```

Frames: `8 + interpolate`

### death_blow_<DIR>

```text
heavily armored warrior in <DIR PHRASE>, slow deliberate windup, sword drawn far
back over the shoulder, then one explosive finishing strike with full body weight,
heavy recovery after the blow
```

Frames: `8 + interpolate`

---

## 8. Unity Import Beklentisi

Production icin final export dosyalari:

```text
Assets/Sprites/Characters/Warblade/Animations/warblade_run_SE.png
Assets/Sprites/Characters/Warblade/Animations/warblade_run_NE.png
Assets/Sprites/Characters/Warblade/Animations/warblade_run_NW.png
Assets/Sprites/Characters/Warblade/Animations/warblade_run_SW.png
```

Import standardi:

- Sprite Mode: Multiple
- Cell size: 128 x 128
- PPU: 64
- Pivot: center `(0.5, 0.5)`
- Filter: Point
- Compression: Uncompressed

Scale farkini Unity'de kodla duzeltme. Idle/run occupancy farki belirginse art
problemidir; regenerate veya manuel cleanup gerekir.

---

## 9. QC Checklist

- [ ] 4 diagonal idle anchor ile ayni siluet olcegi
- [ ] Iki el ayni uzun sword hilt uzerinde kaliyor
- [ ] Kilik uzunlugu frame'ler arasinda stabil
- [ ] Character frame'lerine VFX gomulmemis
- [ ] Run loop'ta foot sliding yok
- [ ] Frame 1 ve final frame temiz loop oluyor
- [ ] 252px uretimde clipping yok
- [ ] Unity sheet 128x128 cell, PPU 64, center pivot

End of document.
