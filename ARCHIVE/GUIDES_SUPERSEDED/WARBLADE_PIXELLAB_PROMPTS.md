# WARBLADE — PixelLab Üretim Rehberi (v12)
> Framework: `CHARACTER_IDENTITY_FRAMEWORK.md`
> "dark fantasy" keyword kullanma. Malzeme ve ton diliyle his kur.
> İlk turda helmet yok — önce doğru gövde, sonra headgear.

---

## WORKFLOW

```
AŞAMA 1 — Keşif:    Create Image Pro → 128px, High Top-Down → doğru beden + class kimliği
AŞAMA 2 — Headgear: Gerekirse second pass → hafif baş ekipmanı ekle
AŞAMA 3 — Rotation: Create from Reference → Rotate Character → 8 direction, High Top-Down
```

---

## TOOL SETUP

**Aşama 1-2 (Create Image Pro):**
| Alan | Değer |
|------|-------|
| Output Size | **128px** |
| Reference Images | boş (gerekirse warblade_hades.png) |
| Kamera | **prompt içinde yaz** |

**Aşama 3 (Create Character → Create from Reference → Rotate Character):**
| Alan | Değer |
|------|-------|
| Mode | **Pro** |
| Camera View | **High Top-Down** |
| Size | **128px** |
| Rotation Type | **8 Directions** |
| AI Freedom | **~30%** |

---

## AŞAMA 1 — KEŞİF

Aşağıdaki 3 prompttan birini dene. Kötü çıkarsa sıradakine geç.

**Prompt 1:**
```
tall adult warblade, long legs, broad shoulders, worn partial armor over visible dark cloth and chain details, dark crimson battle wrap, believable greatsword held low, muted iron and ash colors, neutral combat-ready stance, viewed from above at high overhead angle, top-down camera looking down, not chibi, not dwarf-like, not squat, not stocky, not oversized head, not tiny legs, not full plate knight, not closed knight helmet, not flashy
```

**Prompt 2:**
```
battle-worn adult swordsman, readable leg length, broad chest and shoulders, layered dark cloth with partial iron protection, worn crimson wrap, large but believable sword, muted worn metal palette, grounded and practical, neutral stance, high angle overhead view, camera looking down from above, not chibi, not dwarf-like, not squat, not bulky knight, not paladin, not closed knight helmet
```

**Prompt 3:**
```
last-standing warblade survivor, adult proportions, long legs, broad shoulders, practical mixed armor with visible cloth and chain detail, weathered crimson wrap, heavy believable sword at rest, worn iron and tarnished red palette, neutral stance, top-down overhead camera high angle, not chibi, not dwarf-like, not squat, not stocky, not full plate knight, not flashy
```

---

## BEST PICK CRITERIA

```
[ ] Yetişkin oranı hemen okunuyor mu?
[ ] Bacaklar kısa görünmüyor mu?
[ ] Omuzlar geniş ama gövde cüce değil mi?
[ ] Zırh partial/readable — full plate knight'a kaçmadı mı?
[ ] Crimson wrap ve sword class kimliğini destekliyor mu?
[ ] Karakter grounded ve yıpranmış hissettiriyor mu?
```

Seçilen çıktıyı kaydet → `TASARIM/CLASS_CONCEPTS/warblade_base_pick.png`

---

## AŞAMA 2 — SECOND PASS (Headgear — opsiyonel)

Doğru gövde bulunduysa ve hafif baş ekipmanı eklemek istersen:

```
same warblade body and proportions, keep long legs and broad shoulders, keep partial armor and visible cloth, add only simple battered iron head protection or simple worn hood, keep grounded practical tone, not closed knight helmet, not paladin, not full plate knight, not flashy
```

Yeni pick'i kaydet → `TASARIM/CLASS_CONCEPTS/warblade_base_pick_v2.png`

---

## AŞAMA 3 — 8 ROTATION (Create from Reference)

**1.** **Create Character** → **Create from Reference** → **Rotate Character**

**2.** Reference Image → `warblade_base_pick.png` veya `_v2.png` yükle

**3.** Camera: **High Top-Down** | Size: **128px** | Rotation: **8 Directions** | AI Freedom: **~30%**

**4.** Prompt:
```
preserve same warblade identity, keep adult proportions, long legs, broad shoulders, same partial armor, same visible cloth and chain details, same worn crimson wrap, same believable sword size, same grounded tone, not chibi, not dwarf-like, not squat, not full plate knight, not closed knight helmet, not flashy
```

**5.** QC:
```
[ ] 8 yön aynı karakter mı?
[ ] Cüce değil, yetişkin boy mu?
[ ] Kılıç ve kıyafet tutarlı mı?
[ ] Flat/sticker değil mi?
[ ] Açı oyun için okunuyor mu?
```

> **Geçti:** `Assets/Sprites/Characters/Warblade/base/` kaydet
> **Geçmedi:** Failure Fixes tablosuna bak

---

## FAILURE FIXES

| Sorun | Çözüm |
|-------|-------|
| Cüce / squat çıktı | `long legs`, `readable leg length`, `not squat`, `not tiny legs` güçlendir |
| Full plate knight | `partial armor only`, `visible cloth`, `not full plate knight`, `not paladin` güçlendir |
| Kaska kilitleniyor | Helmet/headgear ifadesini tamamen kaldır → Aşama 2'ye taşı |
| Çok düzleşme | `volumetric`, `layered`, `readable body depth` ekle |
| Fazla arcade | `muted`, `worn`, `practical` kelimelerini güçlendir |
| Kimlik Aşama 3'te kayıyor | Guardrail promptu kısalt, AI Freedom azalt |
