# Karakter State Workflow — Pose & Animation
**Tarih:** 2026-05-16 S86_LATE — Karar #145 LOCK
**Önce:** `character_production_prompts.md` ile 10 base character üret. Bu dosya **sonraki aşama**: state özelliği ile pozlar + animasyon.

---

## Mantık

Base character (`character_production_prompts.md`) **tek poz** ile üretildi (standing calm). Idle / Run / Attack / Hit / Death pozları **State özelliği** ile her sınıfa ayrı ayrı eklenir. Her state'ten **first-frame-locked animasyon** üretilir (Karar #145).

PixelLab Web UI V3 → Character card → **Create State** butonu × N.

---

## State Inventory (her sınıf için)

5 direction (S, SE, E, NE, N) × 5 state kategorisi = **25 state per class**. Mirror (SW, W, NW) animasyon aşamasında PixelLab Web UI Mirror Horizontal ile.

| State kategorisi | İsimlendirme | Amaç (animasyon anchor) |
|---|---|---|
| **Idle** | `idle_S`, `idle_SE`, `idle_E`, `idle_NE`, `idle_N` | Idle 4f animasyonu başlangıç frame'i |
| **Mid-walk** | `midwalk_{dir}` | Run 6f animasyonu için (try-and-see, kalite düşükse skip) |
| **Attack anticipation** | `attack_{dir}` | Attack 3-seg windup |
| **Hit recoil** | `hit_{dir}` | Hit 3f recoil |
| **Death start** | `death_{dir}` | Death 6-8f başlangıç |

---

## State prompt patternleri (sınıf-bağımsız, sadece direction değişir)

### Idle (per direction)

| State | Prompt |
|---|---|
| `idle_S` | `Standing idle facing camera (south direction), face visible, both hands relaxed at the sides, calm balanced stance.` |
| `idle_SE` | `Standing idle at 45° diagonal toward camera-right (south-east), 3/4 front view, right shoulder closer to camera, both hands visible.` |
| `idle_E` | `Standing idle full profile facing camera-right (east), side view, weight balanced on both feet.` |
| `idle_NE` | `Standing idle at 45° diagonal away from camera-right (north-east), 3/4 back view, looking away over right shoulder.` |
| `idle_N` | `Standing idle facing away from camera (north), back to viewer, back of head visible.` |

### Mid-walk (per direction, try-and-see)

```
Mid-walk pose facing {direction}, one leg forward in stride and the other back,
body weight on the forward leg, arms swinging naturally in motion,
sense of forward movement.
```

### Attack anticipation (per direction)

```
Attack windup pose facing {direction}, right arm raised above the shoulder ready to strike forward,
left arm braced at chest level, body weight shifted onto the back foot,
knees slightly bent, focused intent in the expression.
```

### Hit recoil (per direction)

```
Hit recoil pose facing {direction}, body jerked backward from impact,
head tilted slightly back, both arms instinctively raised defensively,
weight shifting onto the back foot, brief moment of impact.
```

### Death start (per direction)

```
Death start pose facing {direction}, body collapsing forward,
knees buckling, head dropping toward the chest,
both arms going slack, visible loss of vitality.
```

---

## Animasyon üretimi (her state'ten)

Her state üretildikten sonra → **Add your first animation** → Custom Animation V3:

| Animasyon | Source state | Animation prompt | Generated frames | Final frames |
|---|---|---|---|---|
| Idle 4f | `idle_{dir}` | `Idle breathing loop, subtle shoulder rise and fall, weight balanced, hands relaxed` | 3 | 4 |
| Run 6f | `midwalk_{dir}` (or `idle_{dir}` fallback) | `Walk loop, continuous stride motion, body weight shifting smoothly` | 5 | 6 |
| Attack 3-seg | `attack_{dir}` | `Attack strike, swift forward arm motion, full body commit, hit follow-through` | 2 | 3 |
| Hit 3f | `hit_{dir}` | `Hit reaction recovery, body returning to neutral after impact` | 2 | 3 |
| Death 6-8f | `death_{dir}` | `Death sequence, falling to ground, final collapse` | 5–7 | 6–8 |

**Hard ayar:** `first frame: ON` + `enhance prompt: ON` (Karar #145).

---

## Mirror (animation level, PixelLab Web UI)

Her animasyon üretildikten sonra **PixelLab Web UI Mirror Horizontal** ile:

| Mirror target | Source |
|---|---|
| `_SW` | `_SE` |
| `_W` | `_E` |
| `_NW` | `_NE` |

**Unity flipX KULLANMA** — Karar #144 weapon child SR bozulur.

---

## Pixelorama cleanup pass

Her animasyon üretiminden sonra:

| Sorun | Fix |
|---|---|
| Face/mouth drift across frames | Frame 1'in face'ini diğer frame'lere kopyala |
| Environmental artifact (grass, items) | Erase |
| Color palette drift per frame | Color picker correction |
| Hallucinated weapon | HARD FAIL → animasyonu regenerate |
| South direction too quick | Reroll budget 2× (Pixelorama fix etmez) |

---

## Sınıf bazında state inventory tally

| Asset | Count per class |
|---|---|
| V3 base character | 1 (already produced via `character_production_prompts.md`) |
| Idle states | 5 |
| Midwalk states | 1-5 (try-and-see, S önce) |
| Attack anchor states | 5 |
| Hit anchor states | 5 |
| Death anchor states | 5 |
| Generated animations | 25 (5 anim × 5 direction) |
| Mirrored animations | 15 (3 mirror × 5 anim) |
| **TOTAL anims per class** | **40** |
| **TOTAL frames per class** | ~192 |
| **TOTAL credit estimate per class** | 70-90 |

10 sınıf × 70-90 credit = **~700-900 credit** (Batch 1+2 base 10 generation ekstra ~50 credit, toplam ~750-950 credit roster).

---

## Klasör yapısı (per class)

```
Assets/Sprites/Characters/{ClassName}/
├── base/
│   └── {classname}_base_v3.png         (Batch 1/2'den gelen base character)
├── states/
│   ├── {classname}_idle_{S,SE,E,NE,N}.png
│   ├── {classname}_midwalk_{...}.png    (try-and-see)
│   ├── {classname}_attack_{...}.png
│   ├── {classname}_hit_{...}.png
│   └── {classname}_death_{...}.png
└── anim/
    ├── idle/   {classname}_idle_{8 direction}.gif    (4f)
    ├── run/    {classname}_run_{...}.gif              (6f)
    ├── attack/ {classname}_attack_{...}.gif           (3f)
    ├── hit/    {classname}_hit_{...}.gif              (3f)
    └── death/  {classname}_death_{...}.gif            (6-8f)
```

---

## PASS criteria per class

- [ ] Body identity preserved across 5 idle states (face, palette, silhouette stable)
- [ ] Face/mouth drift minimal across frames (<10% delta)
- [ ] All 5 directions read correctly (silhouette legibility)
- [ ] Run cycle smooth (mid-walk PASS or idle→run fallback PASS)
- [ ] Attack 3-seg reads as wind-up → strike → follow-through
- [ ] Mirror W/SW/NW directions visually correct
- [ ] South direction usable after max 2× reroll
- [ ] **Hallucinated weapon = HARD FAIL** (Karar #144)
- [ ] Pixelorama cleanup pass <30 min per direction

---

## Üretim sırası (10 sınıf rollout)

1. `character_production_prompts.md` ile **10 base character** üret (Batch 1+2, ~50 credit)
2. Bu dosyaya geç → her sınıf için state+anim üretim (Warblade önce → pattern validate → diğer 9 sınıf)
3. Her sınıf bitince Unity asset path'e kayıt
4. Sprint 13+: Unity'de weapon child SR ekleme, animation events, runtime integration

---

End of state workflow sheet. Base character üretimi `character_production_prompts.md`.
