# RIMA — Silah Üretim Guide (2.5D Ayrık Katman Sistemi)

**Tarih:** 2026-05-11
**Statü:** LOCKED — V1 üretim için ana referans
**Kapsam:** Warblade greatsword, Shadowblade dagger, Ranger bow
**Bağlı:** `MEMORY/project_weapon_anchor_system.md`, `MEMORY/pixellab_master_pipeline.md`

---

## 1. KONSEPT — Niye Tek Sprite Yeter

Saat ibresi mantığı:
- **Sprite** = statik tek görsel (örn: kılıç dik yukarı, kabza altta)
- **Unity** = sprite'ı GameObject'e atar, Transform üzerinden döndürür
- **Attack** = AnimationCurve `Transform.rotation`'ı zaman içinde değiştirir

Sonuç: Aynı görsel, farklı açıda render edilir → **swing efekti**.

```
Frame    Body Anim (silahsız)       Weapon Rotation    Görünüm
0        Kollar tepede              0°                 ⚔️ dik yukarı
2        Kollar yandan iniyor       -90°               ⚔️ yatay sağ
4        Kollar aşağıda             -180°              ⚔️ dik aşağı
```

Body PixelLab'dan gelir (kol hareketi). Rotasyon Unity'den gelir (AnimationCurve).
Hand Anchor (magenta-dot) silahın grip noktasını body'nin eline pinler.

**1 sprite + N curve = N saldırı animasyonu.** Sprite yeniden çizilmez.

Farklı silah = sadece sprite swap. AnimationCurve'lara dokunulmaz, body anim'e dokunulmaz.

---

## 2. TOOL SEÇİMİ — Create Image S-XL (new)

User PixelLab interface verification (2026-05-11): Create Image S-XL (new) silah üretimi için optimal.

| Özellik | Create Image S-XL (new) | Create Image PRO | Create Object |
|---|---|---|---|
| Init image (style/ref eşdeğeri) | ✅ Var | ✅ Style + 4 ref | ❌ Yok |
| Direction setting | ✅ **DirectionNone** (non-character önerilen) | ❌ Prompt'a yazılır | ✅ 1/8 yön |
| Native canvas sizing | ✅ Spesifik pixel | ✅ Grid-based | ✅ Slider |
| Transparent bg | ✅ Native | Remove Background opsiyonu | ❌ Çevrede gürültü |
| Non-character optimize | ✅ Evet (önerilen) | Tüm tipler | Animasyonlu obje |
| **RIMA seçimi** | **✅ ANA TOOL** | Yedek (multi-ref gerekirse) | Animasyonlu çevre objeleri |

**Karar: Silah sprite üretimi = Create Image S-XL (new) + DirectionNone + init image.**

Sebep:
- DirectionNone seçeneği zaten "non-character için önerilen" diye işaretli → tool RIMA silah use-case'i için tasarlanmış
- Init image = anchor'dan crop edilmiş silah görseli → görsel DNA aktarılır
- Native canvas = target boyut → trim gerekmez
- Transparent bg native → post-processing minimal

---

## 3. CANVAS BOYUTLARI — Per Class

Create Image S-XL (new) Width/Height seçenekleri: **32 / 64 / 128 / 256 / 512 / 768**. 192 yok → 256 kullan (transparent padding sorun değil).

| Sınıf | Silah | Canvas (W×H) | Direction | Footprint | Pivot Setup |
|---|---|---|---|---|---|
| Warblade | Two-handed greatsword | **256×256** | None | ~150px tall | Post-gen: grip pixel'i Sprite Editor'da işaretle |
| Shadowblade | Dagger (1 sprite, 2 instance) | **64×64** | None | ~40px | Post-gen: kabza pixel ~(32, 48) |
| Ranger | Recurve bow | **128×128** | None | ~110px tall | Post-gen: bow center grip ~(64, 64) |
| Elementalist | — (Particle System) | — | — | — | — |

Diğer ayarlar (hepsi için aynı):
- **View:** High top-down
- **Detail:** Highly detailed
- **Outline:** Single color outline
- **Transparent background:** ON

PPU=64 ZORUNLU (body ile eşleşme). Padding transparent → Unity'de sorun yok, pivot grip pixel'inde.

---

## 4. PROMPT ŞABLONU (Init Image ile Kısaltılmış)

Init image silahın görsel DNA'sını taşıdığı için prompt kısalır. Sadece tip + iyileştirme talimatı + style guardrail.

```
TYPE: weapon prop, [silah tipi]
INSTRUCTION: Recreate the weapon shown in init image with cleaner details
  and refined pixel work. Same overall proportions, design, and color palette
  as init image. Pixel art, 64 PPU, 1px solid black outline, hard pixel edges,
  no anti-aliasing.
WEAPON DETAILS: [opsiyonel — anchor'da net görünmeyen detay, örn. "add subtle
  fuller line on blade", "leather wrap shows minor wear pattern"]
COLOR: match init image palette exactly, no new colors introduced
NEGATIVE: blur, 3d render, smooth gradient, anti-aliasing, ambient occlusion,
  photo-realistic, soft shading, painterly, double outline
```

### Init Image Hazırlığı (per sınıf)

Aseprite'ta:
1. `<class>_south_clean.png` aç (face cleanup sonrası anchor)
2. Sadece silah bölgesini seç (rectangle marquee — kılıç, dagger, bow)
3. Crop → yeni dosya
4. Padding ekle (target canvas boyutunda, silah merkezde):
   - Warblade: 192×192 canvas, silah ~150px ortada
   - Shadowblade: 64×64 canvas, dagger ~40px ortada
   - Ranger: 128×128 canvas, bow ~110px dikey ortada
5. Save: `<class>_<weapon>_init.png`
6. PixelLab init image olarak yükle

Init image'in **PALETTE + SİLÜET** init olarak iletilir. Output cleaner versiyon olur.

---

## 5. ÜRETİM AKIŞI (Per Silah)

```
ADIM 1: Init Image Hazırla
   - <class>_south_clean.png aç (face cleanup sonrası)
   - Aseprite'ta silah bölgesini seç + crop
   - Target canvas'ta padding ekle (silah merkezde, native target boyutu kadar):
     - Warblade: 256×256 canvas, kılıç ~150px tall, ortalı
     - Shadowblade: 64×64 canvas, dagger ~40px ortalı
     - Ranger: 128×128 canvas, bow ~110px dikey ortalı
   - Save: <class>_<weapon>_init.png

ADIM 2: PixelLab Create Image S-XL (new) ayarları
   - Direction: **None** (non-characters için önerilen)
   - View: **High top-down**
   - Detail: **Highly detailed**
   - Outline: **Single color outline**
   - Init image: <class>_<weapon>_init.png yükle
   - Width × Height:
     - Warblade: 256 × 256
     - Shadowblade: 64 × 64
     - Ranger: 128 × 128
   - Transparent background: ✅ ON
   - Description: Bölüm 4 prompt şablonu (init image varken kısa)
   - Generate

ADIM 3: Generation review
   - Init image'e proporsiyon olarak benzer mi?
   - Outline 1px solid black mi?
   - Renk paleti init image ile birebir mi (yeni renk eklenmemiş)?
   - Pixel density 64 PPU (clean, no anti-alias)?
   - Yön: side profile, dikey (blade yukarı, grip aşağı)?
   FAIL → Prompt revize veya init image düzenle, re-generate

ADIM 4: Eraser Pass (Pixelorama, master pipeline Bölüm 11)
   - 1 canonical sprite (S-XL native canvas = 1 output)
   - Stray pixel temizliği, outline tek 1px, mixel düzeltme
   - Background tam transparent mı (alpha=0)? Değilse manuel temizle

ADIM 5: Unity Import (master pipeline Bölüm 13)
   - Texture Type: Sprite (2D and UI)
   - Sprite Mode: Single
   - Filter Mode: Point
   - Compression: Uncompressed
   - Pivot: Custom (Sprite Editor'da grip pixel koordinatı)
     - Warblade: (96, 100)
     - Shadowblade: (32, 48)
     - Ranger: (64, 64)
   - PPU: 64

ADIM 6: Style/QC Check
   - Body sprite ile yan yana koy → outline kalınlığı eşleşiyor mu?
   - Pixel density eşleşiyor mu?
   - Renk paleti class accent uyumlu mu?
   FAIL → Edit Image Pro ile düzelt veya yeniden üret
```

---

## 6. UNITY ENTEGRASYONU

### Prefab Hiyerarşisi

```
Player (GameObject)
├── BodySpriteRenderer       (252×252, PPU=64, animasyon clipleri)
├── ShadowBlob               (zemin gölgesi)
├── WeaponPivot (Transform)  (child, hand anchor takibi için)
│   ├── WeaponSpriteRenderer (kılıç sprite'ı, custom pivot grip'te)
│   └── WeaponHitbox (BoxCollider3D, attack sırasında enabled)
└── OrbVFX (sadece Elementalist — Particle System, right hand parented)
```

### WeaponController.cs (state machine)

```csharp
public enum WeaponMode {
  AnchorFollow,  // idle/walk — hand anchor takibi
  AttackDriven   // attack — AnimationCurve rotasyon
}

// LateUpdate:
// AnchorFollow → weaponPivot.localPosition = handAnchorMap.GetAnchor(currentClip, currentFrame)
//                weapon.localRotation = idleRotation (sabit, hafif sway)
//
// AttackDriven → weaponPivot.localPosition = attackProfile.positionCurve.Evaluate(normalizedTime)
//                weapon.localRotation = Quaternion.Euler(0, 0, attackProfile.rotationCurve.Evaluate(normalizedTime))
//                weapon.sortingOrder = body.sortingOrder + Mathf.RoundToInt(attackProfile.sortingCurve.Evaluate(normalizedTime))
//                hitbox.enabled = (normalizedTime >= attackProfile.hitboxStart && normalizedTime <= attackProfile.hitboxEnd)
```

### AttackProfile (ScriptableObject)

```
- Name: "Warblade_OverheadSlash"
- BodyAnimClip: ref Warblade_AttackOverhead_South
- Duration: 0.5s
- RotationCurve: 0s = 0°, 0.3s = -90°, 0.5s = -180°
- PositionCurve: hand anchor takibi (offset = 0)
- SortingOrderCurve: 0s = +1, 0.25s = +1 → -1 (mid-swing flip), 0.5s = -1
- HitboxStart: 0.2 (normalizedTime)
- HitboxEnd: 0.4
- HitboxSize: (40, 110, 20) — sword footprint
```

### Senkronizasyon (Codex review kritik nokta)

```csharp
// Loop boundary fix (Codex feedback Risk 1):
float phase = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
int frameIndex = Mathf.Min((int)(phase * totalFrames), totalFrames - 1);

// LateUpdate'te konsolide (Codex feedback Risk 3):
void LateUpdate() {
  UpdateAnchorPosition();
  UpdateWeaponRotation();
  UpdateSortingOrder();
  UpdateHitboxState();
  Physics.SyncTransforms(); // anlık physics query için
}
```

---

## 7. ÖRNEK — Warblade Greatsword Üretim

### Adım 1-2: Style Ref + Prompt

```
Style image: rima_style_anchor.png
Reference 1: warblade_south_clean.png (silahlı anchor, post Face Cleanup)
Reference 2: warblade_sword_crop.png (anchor'dan kestik silah bölgesi)

Prompt:
TYPE: two-handed greatsword weapon prop
STYLE: 16-bit RPG weapon, pixel art, hard edges, no anti-aliasing
WEAPON: large European-style two-handed greatsword, straight broad blade,
  crossguard with downturned quillons, leather-wrapped two-hand grip,
  round pommel
MATERIAL: dark steel blade with subtle fuller, brass crossguard, dark leather
  grip wrap, polished steel pommel
ORNAMENT: minimal — leather wrap shows wear, no engraving
VIEW: side profile, weapon vertical (blade up, grip down), East-facing
SIZE: blade ~110px, crossguard ~25px wide, grip ~35px (hands stacked),
  pommel ~8px diameter — total height ~150px, width ~30px max
COLOR: blade #5A5860/#7A7880/#9A9890, guard #7A5030/#9A6040, grip #2A1010/#4A2020,
  pommel #6A6060/#8A8078, accent steel highlight #BAB8B0
--- RULES ---
Single weapon centered on canvas, transparent background, sword in vertical pose.
PRESERVE EXACTLY: 64 PPU pixel density, 1px solid black outline,
no anti-aliasing, hard pixel edges only, no smoothing, no gradient blur.
Match style and palette of reference image 1 (warblade character),
weapon design should match the sword visible in reference image 2.
NEGATIVE: blur, 3d render, anti-aliasing, ambient occlusion, photo-realistic,
soft shading, painterly, double outline, glow effect
```

### Adım 3-5: Generate → Eraser → Trim

- Generate (256×256, 1 frame, 20 gen)
- Stray pixel + outline temizliği Pixelorama'da
- 256×256 → 192×192 trim (canvas merkezinde silah)

### Adım 6: Unity Import

- File: `Assets/Sprites/Weapons/Warblade/greatsword_v1.png`
- Sprite Editor: Custom pivot (96, 100) — grip ortası
- PPU: 64, Point filter, Uncompressed

### Adım 7: AttackProfile ScriptableObject

```
WarbladeOverheadSlash.asset:
  RotationCurve: linear 0° → -180° over 0.5s (curve animasyon)
  PositionCurve: hand anchor + slight forward translate (4px push)
  HitboxWindow: 0.2-0.4 normalizedTime
  HitboxSize: BoxCollider3D (40, 110, 20) — kılıç boyu
  SortingOrderCurve: +1 → -1 keyframe @ 0.25s (mid-swing flip)
```

### Test

- Body anim oynat (silahsız)
- WeaponController = AnchorFollow → silah elde durur
- Attack input → WeaponController = AttackDriven → silah döner
- Mob'a temas → hitbox damage

---

## 8. CREDIT + ZAMAN TAHMİNİ

| Sınıf | Silah Üretimi | Output | Bonus | Pivot Setup | AttackProfile |
|---|---|---|---|---|---|
| Warblade | 20 gen, ~30 dk (+ trim) | 1×192 canonical | — | 5 dk | 30 dk |
| Shadowblade | 20 gen, ~30 dk | 1×64 canonical | 15 dagger variant | 5 dk | 30 dk |
| Ranger | 20 gen, ~30 dk + arrow ayrı | 1×128 canonical | 3 bow variant | 5 dk | 30 dk |
| Elementalist | YOK (Particle System) | — | — | — | Particle setup 45 dk |

**Toplam: ~60 gen, ~3 saat aktif iş.**
**Bonus: 15 dagger + 3 bow varyant V2 skin sistemi için bekliyor (extra credit yok).**

---

## 9. KONTROL LİSTESİ (Per Silah)

```
[ ] Style image hazırlandı (rima_style_anchor.png)
[ ] Class anchor reference olarak yüklendi
[ ] Cropped weapon area reference olarak yüklendi
[ ] Structured prompt yazıldı (Bölüm 4 şablonu)
[ ] Canvas 256×256 (1 frame output garantili)
[ ] Remove Background ON
[ ] Generation review (outline 1px, palette eşleşmeli)
[ ] Eraser Pass Pixelorama'da
[ ] Target canvas'a trim
[ ] Unity import (Point filter, Uncompressed, custom pivot)
[ ] Body sprite ile yan yana QC (outline + density eşleşme)
[ ] AttackProfile ScriptableObject oluşturuldu
[ ] WeaponController test: AnchorFollow + AttackDriven
[ ] Hitbox damage test (mob'a temas)
```

---

## 10. ÇAKIŞMA UYARISI

`pixellab_master_pipeline.md` Bölüm 9.1 "Edit Image Pro (Weapon Pass)" kuralı baked-in weapon yaklaşımıydı. **Bu yaklaşım artık geçerli değil.** RIMA 2.5D ayrık silah katmanı sistemine geçti (`MEMORY/project_weapon_anchor_system.md`).

Edit Image Pro silah sprite üretimi için DEĞİL, body-only weapon removal pass'i için kullanılır.

---

**Sonraki adım:** POC GATE PASS sonrası Shadowblade pilot dagger sprite üretimi. POC checklist'i: `TASARIM/V1_PRODUCTION_PIPELINE_LOCKED.md`.
