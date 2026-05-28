# RIMA — Animation Prompt Catalog (Warblade Demo Faz 1 + 9 Class Faz 4 master)

**Tarih:** 2026-05-27 gece
**Author:** rima-design (Opus 4.7) — triple-AI synthesis (NLM canonical + agy benchmark + asset envanteri)
**Status:** PLAN — kullanıcı PixelLab Web UI "Add Animation" ekranında uygulayacak. Gen yok bu task'te.
**Sources:** NLM 30ddffa5 (S43/S44/S99 canonical lock'lar), agy yasinderyabilgin dispatch, asset envanteri + Animator inspect.

---

## 0. NEDEN bu katalog (kullanıcı verbatim)

> "Karakterlere animasyonlar yapılacak ya hangi animasyon için hangi stateler kullanılabilir end ve first frame gerekir mi prompt ne olur gibi. Bu karaktere senin prompt vereceğin benim animasyonu yapacağım ekran. Start ve end frame i karakterimize state üretimiyle ayarlayabiliyoruz ve smoothluk için kaç frame olmalı onu da düşün."

Kullanıcı PixelLab "Add Animation" UI'da her Warblade animasyonu için: animation type seç, frame count seç, start/end frame yükle, prompt yapıştır, generate. Bu katalog her animasyon için **tüm girdileri** sağlar.

---

## 1. CANONICAL Framework (NLM lock — değişmez)

### 1.1 Karar #120 SPLIT-ANIMATION pipeline (LOCKED 2026-05-13)
**Trigger:** Frame budget ≥ 12f **VE** net tek apex frame (gameplay/visual kimliği olan impact) varsa → split MANDATORY.

3-stage:
1. **Stage 1 — Apex State** (Create State, 20-40 gen): apex pozunu 8 yön için **kilitle**, pixel hash kaydet
2. **Stage 2 — Part 1** (Custom V3): Frames 1..APEX (apex dahil), End Frame = apex_state
3. **Stage 3 — Part 2** (Custom V3): Frames APEX..N (apex dahil), First Frame = apex_state

Apex frame iki part'ta **paylaşılır** → toplam unique = N frame (apex bir kez sayılır).

### 1.2 Pixel Budget (252×252 canvas, ZORUNLU)
`width × height × frames ≤ 524,288`
- 252×252 → MAX **8 unique frame** (silahlı karakterler için)
- 160×160 → 16 frame (silahsız only)
- 128×128 → KULLANMA (silah kırpılır)

→ Warblade greatsword'lü 8 frame zorunlu cap.

### 1.3 Karar #114 — 5+3 mirror (LOCKED)
- Native gen: **S, SE, E, NE, N** (5 yön)
- Unity runtime mirror: **W, SW, NW** (flipX)
- PixelLab "2 gen/direction" → 5 yön × 2 = **10 gen per anim** (south-only başlangıç = 2 gen)

### 1.4 Karar #71 — WEAPON LOCK (WEAPONLESS PRODUCTION'DA UYGULANMAZ)
~~Prompt'ta YASAK: `drop, release, slip, fall, throw, separation, let go, weapon falls, weapon flies, weapon detaches, weapon on ground`~~

~~Her silahlı anim prompt'una **negatif olarak** dahil et: `WEAPON LOCK: weapon stays firmly in hands, do not use words drop/release/slip/fall/throw`~~

**Karar #71 ARTIK weaponless production'da uygulanmaz.** Warblade body sprite silahsız (LIVE, verify done). Weapon mount Unity child SR aracılığıyla yapılır (Karar #144 + #123 + #146 LIVE). WEAPON LOCK satırı tüm anim prompt'larından kaldırıldı. Weaponless negative prompt şu şekilde: `NO weapons, NO held items, NO sword, NO shield, hands empty throughout.`

### 1.5 Canvas + padding kuralı
- **252×252 canvas** (v3 zorunluluğu)
- Karakter ~%60 alan kaplar (~168×168 px)
- ~%40 transparent padding **zorunlu** (silah arc, VFX, lunge)
- Prompt: `don't fill canvas, leave wide transparent headroom`
- ASLA `fill canvas` veya `occupies entire frame` yazma

### 1.6 Animator timing kanonu (S43 lock)
- Impact/apex frame: **40ms** (snappy)
- Diğer frame'ler: **80-100ms**
- Idle: **4 frame**, KeepFirst **ON**
- Blend Tree Exit Time: **OFF**, transition 0.05s
- `run_stop` state MANDATORY animator graph'ta

### 1.7 MCP YASAK (2026-05-02 LOCK)
- `animate_character` MCP **kalıcı YASAK** → Web App only
- `create_character_state` MCP **user approval sonrası OK** (feedback_state_gen_mcp_user_approval_exception)
- Animasyon clip in-between **kullanıcı yapar** (Aseprite tween / Unity Animator)

### 1.8 Karar #108 — Gen count
> **SUPERSEDED (S114 V3 cost mapping LIVE):** Aşağıdaki yeni maliyet tablosu Karar #108'i günceller.

**Yeni V3 cost mapping (S114 LIVE):**
| Frame count | Gen/dir |
|---|---|
| 4f | 1 gen |
| 6-8f | 2 gen |
| 10-12f | 3 gen |
| 14-16f | 4 gen |

- Apex State 20-40 gen (8-dir lock — değişmedi)
- Animate with Text v3 v0.4.92+ → cost yukarıdaki tabloya göre
- ~~Custom V3 minimum 3 gen/dir~~ → frame count'a göre yukarıdaki tablo kullan

---

## 2. PixelLab Add Animation flow — kullanıcı için step-by-step

PixelLab Web UI'da her anim için:

1. **Add Animation** tıkla
2. **Direction** seç: önce **South** (Tier 1 demo south-only)
3. **Animation Type** seç:
   - **Built-in** (Idle 2 opt / Walking 19 opt / Running 3 opt / Reactions 4 opt / Punching 6 opt) — düşük cost, hızlı, template tabanlı
   - **Custom Animation (V3)** — kompleks attack/skill için, Action Description yaz
4. **Frame Count** slider (4-16, V3)
5. **Keep first frame** checkbox:
   - LOOP anim (idle/walk/run) → **işaretli ✓** (cycle coherence)
   - ONE-SHOT (attack/hit/death/skill) → **işaretsiz** (transition için)
6. **Advanced Options → Custom Frames**:
   - **Start Frame**: character_state'den ilk poz (gerekli ise)
   - **End Frame**: character_state'den son poz (gerekli ise)
7. **Generate** → cost düşülür (declared 2 gen/dir built-in, V3 1-9 gen/dir)

---

## 3. Animation Catalog — Warblade Demo Faz 1

### 3.1 Tier 1 — Demo BLOCKER (5 anim, mandatory, south-only ilk)

#### A — Idle
| Field | Value |
|---|---|
| Type | Built-in Idle (2 opt) **veya** Custom V3 |
| Loop? | **YES loop** |
| Frame Count | **6 frame** (agy + NLM 4-6 sweet spot @ 10fps) — NLM lock = 4f Idle Keep ON. **Karar:** 4 frame (NLM canonical override) |
| Keep First Frame | **✓ ON** |
| Start Frame | mevcut `warblade_south.png` (idle anchor LIVE) |
| End Frame | — (Keep First ON, gerek yok) |
| Cost (south) | 2 gen (built-in) veya 1-3 gen (V3) |
| Prompt (V3 ise) | `subtle idle breathing, slight body weight shift left to right, arms relaxed at sides with hands in loose fist, weapon-ready grip on right hand. 4-frame ping-pong loop (1→4→1). Feet firmly planted. NO weapons, NO held items, hands empty throughout.` + global negative |

**Notes:** Ping-pong loop (1→4→1) mümkün, üretilen unique = 4 frame ama görünür = 7 (1,2,3,4,3,2,1). NLM `Idle: 4 frames, KeepFirst ON` katı.

#### B — Walk
| Field | Value |
|---|---|
| Type | Custom V3 (**Brian's Extreme Pose** method, NLM canonical) **veya** Built-in Walking |
| Loop? | **YES loop** |
| Frame Count | **8 frame** (Pixar/Disney contact-passing-contact standard, NLM + agy mutabık) |
| Keep First Frame | **✓ ON** |
| Start Frame | **Pose A** state: en uç stride (sol diz yukarı, sağ ayak yerde) |
| End Frame | **Pose B** state: Pose A'nın `flipX` versiyonu (sağ diz yukarı) **veya** opposite contact |
| Cost (south) | 2 gen (built-in 19 opt) **veya** Pose A/B state + 1-3 gen interpolate V3 |
| Prompt (V3 ise) | `confident walking cycle, arms swing naturally at sides, right hand in loose weapon-ready grip, hip sway moderate, 8-frame contact-passing-contact loop. Frame 1: right foot forward, left arm forward. Frame 5 (opposite): left foot forward, right arm forward. NO weapons, NO held items, hands empty throughout.` + global negative |

**Notes (Brian's Extreme Pose):**
1. Animate with Text NEW (v3) 12 frame walk al → en uç poz **Pose A** seç
2. Aseprite'ta Pose A yatay flip → **Pose B**
3. Interpolate NEW (v2 252×252) Input: Pose A + Pose B → 4-6 frame
4. Geniş silah kırpma yaşarsan: Animation-to-Animation Bridging Mode kullan

#### C — Basic Attack (LMB single swing) — SPLIT
| Field | Value |
|---|---|
| Type | **Custom V3 + SPLIT** (Karar #120, 8 frame total, apex paylaşımı) |
| Loop? | **NO one-shot** |
| Frame Count | **8 unique** (windup 4 + follow 4, apex shared) |
| Keep First Frame | **✗ OFF** |
| Apex Frame | f04 — silahın horizontal arc'in tepe noktası, en geniş silhouette |
| Stage 1 — Apex State | Create State, 8-dir, 20-40 gen, kaydet `warblade_attack_LMB_apex_weaponless_state.json` |
| Stage 2 — Part 1 (windup, 4 frame) | Custom V3, End Frame = apex_state, Frame Range 1-4 |
| Stage 3 — Part 2 (follow, 4 frame) | Custom V3, First Frame = apex_state, Frame Range 4-7 (apex paylaşılır, ek 3 frame) |
| Cost (south total) | Apex 20-40 gen + Part 1 2 gen (8f→V3 cost) + Part 2 2 gen ≈ **24-44 gen** (S114 V3 mapping) |
| Prompt Part 1 | `Frame 1: right arm raised high above right shoulder, elbow bent, hand open palm facing left, weight shifting to right foot, body coiling. Frame 4 (apex): right arm fully extended in wide horizontal arc sweeping left, hand open-palm reaching maximum extension, body fully rotated, weight on left foot. The right arm motion traces the path a greatsword would take in a full horizontal swing. NO weapons, NO held items, hands empty throughout. 2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing. don't fill canvas, leave wide transparent headroom.` |
| Prompt Part 2 | `Frame 1 (apex): right arm fully extended in wide horizontal arc sweeping left, hand open-palm at maximum extension, body fully rotated. Frame 4: right arm returning low at right hip, hand returning to rest fist, body back to forward-facing stance, recovery. NO weapons, NO held items, hands empty throughout. 2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing. don't fill canvas, leave wide transparent headroom.` |
| Animator timing | f04 (apex) = 40ms, diğer = 80-100ms (S43 lock) |
| Export checklist | (5 madde, NLM lock) Part 1 son frame hash == Part 2 ilk frame hash; 8 frame toplam; 1 fNN_APEX label; frame order Part 1 full + Part 2 frames 2..N; apex hash == Stage 1 kayıt |

#### D — Hurt (Hit-React, Scar Memory 1.2s için zorunlu)
| Field | Value |
|---|---|
| Type | Built-in Reactions (4 opt) **veya** Custom V3 single-pass |
| Loop? | **NO one-shot** |
| Frame Count | **4 frame** (windup 1, hold 1, follow 2 — agy frame budget) |
| Keep First Frame | **✗ OFF** |
| Start Frame | mevcut `warblade_south.png` (idle anchor) |
| End Frame | — (recovery → animator Idle'a transition) |
| Cost (south) | 1-2 gen (built-in) veya 1-3 gen V3 |
| Prompt (V3 ise) | `Frame 1: body recoil sharply backward from frontal impact, right arm flailing outward, left arm raised instinctively as guard, torso twisted back. Frame 4: body returning to upright idle stance, arms settling back to sides. Snappy recoil, no weapon interaction. NO weapons, NO held items, hands empty throughout.` + global negative |
| Animator timing | snap recoil 80ms, recovery 100-120ms |
| Direction | 4 yön yeter (S/E/N/W) readability için, diagonal'ı reuse |

#### E — Death
| Field | Value |
|---|---|
| Type | Custom V3 single-pass (apex yok, drama) |
| Loop? | **NO one-shot** (final state, no exit) |
| Frame Count | **6 frame** (NLM lock = 6) — agy 10-12 önerdi ama NLM canonical override, pixel budget 252×252 cap |
| Keep First Frame | **✗ OFF** |
| Start Frame | mevcut idle anchor |
| End Frame | character_state: yerde yatık poz (transparent BG, dust optional) |
| Cost (south) | 1-3 gen V3 + 1 End State (opsiyonel) |
| Prompt | `Frame 1: standing wounded, hand on chest, head bowing, knees buckling. Frame 6: collapsed forward face-down on ground, right arm extended, left arm tucked under torso, final resting position. Slow, dramatic fall. NO weapons, NO held items, hands empty throughout.` + global negative |
| Direction | 4 yön (S/E/N/W) |

**Tier 1 cost özet (south-only, ilk pass) — S114 V3 mapping:**
- Idle (4f): 1 gen (V3) veya 2 gen (built-in)
- Walk (8f): 2 gen (built-in) veya 2 gen (V3 interpolate)
- Basic Attack LMB (8f split): Apex 20-40 gen + Part 1 2 gen + Part 2 2 gen ≈ **24-44 gen**
- Hurt (4f): 1 gen (V3) veya 1-2 gen (built-in)
- Death (6-8f): 2 gen (V3)
- **Toplam Tier 1 south-only ≈ 28-52 gen**

5-dir expansion (S→E→N→SE→NE) ×5: **~140-260 gen** Tier 1 full multi-view.

---

### 3.2 Tier 2 — Skill anim (4-6 skill, demo havuz)

#### F — Iron Charge (8m dash + 1.5s stun, RIMA signature) — SPLIT
| Field | Value |
|---|---|
| Type | **Custom V3 + SPLIT** (apex = impact moment) |
| Loop? | **NO one-shot** |
| Frame Count | **8 unique** (windup 4: crouch lean / dash mid 1 / impact apex 1 / follow 2) |
| Apex Frame | f05 — full shoulder forward, ground crack |
| Stage 1 Apex State | charge_impact_state, 8-dir |
| Cost (south) | Apex 20-40 + Part1 2 gen + Part2 2 gen ≈ **24-44 gen** (S114 V3 mapping) |
| Prompt Part 1 | `Frame 1: crouch into low sprint stance, right arm trailing behind right shoulder in weapon-grip pose, body leaning aggressively forward, left arm driving forward. Frame 4 (apex): full shoulder slam forward, right arm driving forward at chest height, ground crack underfoot, brief golden dust trail. Right arm motion mirrors where greatsword would trail in a charge. NO weapons, NO held items, hands empty throughout.` |
| Prompt Part 2 | `Frame 1 (apex): full shoulder driven forward, right arm at chest height, ground crack, golden trail. Frame 4: standing upright, right arm raised mid-follow-through, recovery stance. NO weapons, NO held items, hands empty throughout.` |

#### G — Earthsplitter (3m knockup 2s, Rage+25) — SPLIT
| Field | Value |
|---|---|
| Type | **Custom V3 + SPLIT** (apex = sword embed) |
| Frame Count | **8 unique** (windup overhead 4, follow 4) |
| Apex Frame | f04 — sword planted ground, dust burst |
| Stage 1 Apex State | sword_planted_state |
| Cost | Apex 20-40 + Part1 2 gen + Part2 2 gen ≈ **24-44 gen** (S114 V3 mapping) |
| Prompt Part 1 | `Frame 1: both arms raised high overhead in maximum windup, right and left fists clasped together above head in two-hand grip pose, body leaning back, maximum stretch. Frame 4 (apex): massive downward slam with both hands driving toward ground, dust burst around impact point, ground crack radiating outward, body fully committed. Two-hand clasped grip throughout — greatsword will be placed between hands. NO weapons, NO held items, hands empty throughout.` |
| Prompt Part 2 | `Frame 1 (apex): both hands at ground level, dust burst, ground crack, body fully extended downward. Frame 4: pulling both arms up from ground, recovery to standing stance. NO weapons, NO held items, hands empty throughout.` |

#### H — Gravity Cleave (4m pull + 140% damage) — SPLIT
| Field | Value |
|---|---|
| Type | **Custom V3 + SPLIT** (apex = blade rip) |
| Frame Count | **8 unique** |
| Apex Frame | f04 — blade horizontal ripping motion, gravity arc |
| Stage 1 Apex State | spin_release_state |
| Cost | Apex 20-40 + Part1 2 gen + Part2 2 gen ≈ **24-44 gen** (S114 V3 mapping) |
| Prompt Part 1 | `Frame 1: wide stance, right arm lowered and extended to left side at hip level, body wound up for spin, feet planted. Frame 4 (apex): body in violent 360-degree rotation mid-spin, right arm at full horizontal extension reaching maximum arc, golden energy trail visible, debris pulled inward (environmental VFX only, not character-held). NO weapons, NO held items, hands empty throughout.` |
| Prompt Part 2 | `Frame 1 (apex): right arm at full horizontal extension, body mid-spin, golden trail, debris vortex. Frame 4: right arm resting at right side, body back to forward facing, recovery. NO weapons, NO held items, hands empty throughout.` |

#### I — Death Blow (execute, %400 hasar, Rage spend) — SPLIT
| Field | Value |
|---|---|
| Type | **Custom V3 + SPLIT** (signature finisher, dramatic) |
| Frame Count | **8 unique** (NLM cap, agy 14 önerdi ama pixel budget cap) — **Karar:** 8 frame canonical. Frame slow timing ile dramatic hissi (windup frame'lerde 120ms) |
| Apex Frame | f05 — full horizontal swing extension, red rage glow |
| Stage 1 Apex State | finisher_strike_state |
| Cost | Apex 20-40 + Part1 2 gen + Part2 2 gen ≈ **24-44 gen** (S114 V3 mapping) |
| Prompt Part 1 | `Frame 1: low crouch, full torso twist to right, right arm fully wound back behind right shoulder in grip pose, red rage aura glowing around body, intense. Frame 5 (apex): devastating horizontal execution swing, full body rotation, right arm extended fully to left at maximum reach, red glow trail, debris kick. NO weapons, NO held items, hands empty throughout.` |
| Prompt Part 2 | `Frame 1 (apex): right arm fully extended to left at maximum reach, red glow trail, body fully rotated. Frame 4: right arm resting low at right side, body forward, exhausted recovery stance. NO weapons, NO held items, hands empty throughout.` |
| Animator timing | Slow apex (60ms hold) — dramatic film grain hissi |

#### J — Iron Counter (parry, 0.8s pencere, %180 reflect) — SPLIT veya single
| Field | Value |
|---|---|
| Type | **Custom V3** (parry hızlı, split optional) |
| Frame Count | **6 unique** (windup 2, hold/parry 1, reflect 3) |
| Loop? | **HOLD trigger** (parry pencere açıkken loop, hit yerse one-shot reflect) |
| Apex Frame | f03 — block contact moment, sparks |
| Stage 1 Apex State | parry_block_state |
| Cost | Apex 20-40 + V3 2 gen (6f) ≈ **22-42 gen** (S114 V3 mapping) |
| Prompt | `Frame 1: defensive block stance, both forearms raised horizontally as shield barrier, weight back on right foot, alert. Frame 3 (apex): incoming attack contact moment, arms bracing impact, sparks from contact point (environmental VFX), shoulder shuddering. Frame 6: instant counter-slash motion with right arm sweeping outward in horizontal arc, reflective gold glow emanating from right fist. NO weapons, NO held items, hands empty throughout.` |

#### K — Sunder Mark (zırh -%40, 8s) — single
| Field | Value |
|---|---|
| Type | Custom V3 single-pass (kısa cast) |
| Frame Count | **6 frame** |
| Loop? | **NO one-shot** |
| Apex | n/a (single-pass, Karar #120 trigger değil — apex belirsiz) |
| Cost | 2 gen V3 (6f — S114 V3 mapping) |
| Prompt | `Frame 1: right arm extended forward pointing toward target, fingers spread, left hand raised to chin in focus gesture. Frame 6: runic sigil burst from right fingertips/fist toward target direction, cyan armor-break glyph materializes, right arm holds extension. NO weapons, NO held items, hands empty throughout.` |

**Tier 2 cost özet (south-only, 6 skill) — S114 V3 mapping:**
- Iron Charge (8f split): Apex 20-40 + Part 1 2 gen + Part 2 2 gen = **24-44 gen**
- Earthsplitter (8f split): Apex 20-40 + Part 1 2 gen + Part 2 2 gen = **24-44 gen**
- Gravity Cleave (8f split): Apex 20-40 + Part 1 2 gen + Part 2 2 gen = **24-44 gen**
- Death Blow (8f split): Apex 20-40 + Part 1 2 gen + Part 2 2 gen = **24-44 gen**
- Iron Counter (6f split): Apex 20-40 + V3 2 gen = **22-42 gen**
- Sunder Mark (6f): 2 gen (V3 single-pass)
- **Toplam Tier 2 south-only ≈ 120-220 gen**

5-dir full: **~600-1100 gen** Tier 2.

---

### 3.3 Tier 3 — Polish (Track B otonom, demo blocker DEĞİL)
- Basic Attack hit 2 + hit 3 (3-hit combo full chain)
- Run (8 frame faster cycle, walk variant veya built-in Running 3 opt)
- Dash (4 frame, NLM canonical state #5 — Iron Charge'dan ayrı, hareket dash)
- Jump / dodge roll (gameplay'de yok şu an, Track B kararına bağlı)
- Victory pose (demo sonu opsiyonel)
- Idle alternates (boredom)

---

## 4. Global Prompt Building Blocks

Her PixelLab V3 prompt **3 blok** (NLM `pixellab_prompt_structure` lock):

### 4.1 [CHARACTER] Block (Warblade — her promptta SABIT)
```
64x64 chibi top-down character, male heavy warrior,
EMPTY HANDS, fists loosely clenched in weapon-ready grip posture,
dark steel armor uniform with bulky shoulder pads, brown leather straps,
light skin, messy black hair, stern neutral face,
view 35 degree high top-down ARPG angle,
dark brown body #4F3A2C brass accent #C09455
```

> **Değişiklik (B1 weaponless cleanup):** "two-handed greatsword" kaldırıldı. "EMPTY HANDS, fists loosely clenched in weapon-ready grip posture" eklendi. Weapon mount Unity HandAnchor child SR ile yapılır (Karar #144 + #123 + #146).

### 4.2 [ACTION] Block (anim-specific, yukarıdaki tablolarda)
Format: `Frame 1 (windup): <pose desc>. Frame N (apex): <pose desc>. Frame K (recovery): <pose desc>.`

### 4.3 [CONSTRAINTS] Block (her promptta SABIT)
```
2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
don't fill canvas, leave wide transparent headroom.
NO weapons, NO held items, hands empty throughout.
```

> **Değişiklik (B1 weaponless cleanup):** "WEAPON LOCK: weapon stays firmly in hands, do not use words drop, release, slip, fall, throw." satırı kaldırıldı. "NO weapons, NO held items, hands empty throughout." eklendi.

### 4.4 Negative (UI'da Negative Prompt alanı varsa)
```
no extra weapons, no shield, no dropped sword, no anti-aliasing, no blurry edges,
no background, no extra characters
```

---

## 5. character_state Üretim Listesi (Tier 1 + Tier 2 Apex States)

Split-animation pipeline gereği şu Apex State'lerin **gen edilmesi gerekli** (8-dir):

| # | State Name | Animasyon | Cost (8-dir) | Tier |
|---|---|---|---|---|
| 1 | `attack_LMB_apex_weaponless` | Basic Attack | 20-40 gen | T1 |
| 2 | `charge_impact_state` | Iron Charge | 20-40 gen | T2 |
| 3 | `sword_planted_state` | Earthsplitter | 20-40 gen | T2 |
| 4 | `spin_release_state` | Gravity Cleave | 20-40 gen | T2 |
| 5 | `finisher_strike_state` | Death Blow | 20-40 gen | T2 |
| 6 | `parry_block_state` | Iron Counter | 20-40 gen | T2 |

Optional end-state (single-pass anim için, char_state yerine V3 first-frame yeter):
- `death_pose_state` (Death sonu) — opsiyonel
- `walk_pose_A` (Walk Brian Extreme) — Aseprite flip yerine state üretmek yerine **agy önerisi:** char_state pahalı, **interpolate v2 ile pose A+B yeter**, state gen YAPMA

**Apex State toplam (Tier 1+2):** 6 state × 20-40 gen = **120-240 gen**

**character_state vs n_frames + reference_image_base64 trade-off** (NLM canonical):
- **Custom V3 + reference_image_base64** (~1-9 gen/dir, single-pass) — kompleks olmayan anim için
- **character_state Apex** (20-40 gen, multi-pass pixel-hash lock) — split-animation MANDATORY olduğunda (frame ≥12 + net apex)
- **RIMA bağlamı:** 252×252 cap 8 frame zaten, ama apex frame split ile shared frame budget'ı genişletir (effective 12+ frame visual). Bu split fiyatı haklı.

---

## 6. Production Priority Sırası (kullanıcı onayı sonrası, iteratif)

### Phase 1 — Tier 1 south-only (1-2 gün)
1. **Idle south** — Built-in Idle 2 opt, 4 frame, Keep ON (1-3 gen)
2. **Walk south** — Built-in Walking 19 opt VEYA Brian's Extreme V3 (2-4 gen)
3. **Hurt south** — Built-in Reactions 4 opt, 4 frame (1-2 gen)
4. **Death south** — Custom V3 single, 6 frame (1-3 gen)
5. **Basic Attack south (SPLIT)** — Apex State + Part 1 + Part 2 (26-46 gen) ← ÇOK PAHALI

→ Unity import + Warblade prefab south view test → demo loop'ta görüntü kontrol → **kullanıcı onay**.

### Phase 2 — Tier 2 south-only (1-2 gün)
6. **Iron Charge south (SPLIT)** — 26-46 gen
7. **Earthsplitter south (SPLIT)** — 26-46 gen
8. **Gravity Cleave south (SPLIT)** — 26-46 gen
9. **Death Blow south (SPLIT)** — 26-46 gen
10. **Iron Counter south (SPLIT)** — 21-43 gen
11. **Sunder Mark south** — 1-3 gen single

→ Skill draft UI testi (SkillOfferUI.cs S108 LIVE) → demo combat loop full Warblade south-only.

### Phase 3 — Multi-view expansion (5-dir batch)
South → E, N, SE, NE her Tier 1 + Tier 2 anim için. ~5x cost (4 ek yön).

### Phase 4 — 9 Class Faz 4 master rollout (Faz 1 demo onayı SONRASI)
[nine_class_animation_states_demo_phase1_plan.md] sırası: Elementalist → Ranger → Shadowblade → Ronin → Gunslinger → Ravager → Hexer → Brawler → Summoner.

---

## 7. Animator State Machine setup (Unity, kullanıcı yapacak)

### 7.1 Mevcut durum (asset envanteri)
- `Assets/Animations/Characters/Warblade/Warblade.controller` mevcut
- 8 idle clip (`warblade_idle_south/east/north/west/SE/NE/NW/SW.anim`) mevcut ama her biri **1 sprite static** (m_LoopTime: 0, single frame placeholder)
- Walk/Attack/Hit/Death state **YOK** — eklenecek

### 7.2 Hedef state machine
| State | Trigger / Condition | Loop? | Exit Time |
|---|---|---|---|
| idle_{dir} (8 state) | default per dir | YES | n/a |
| walk_{dir} (8 state) | Speed > 0.1 | YES | n/a |
| run_{dir} (8 state) | Speed > 5 | YES | n/a |
| basic_attack_{dir} (8) | AttackTrigger | NO | 1 (full play) |
| hurt_{dir} (4) | HurtTrigger Any State | NO | 0 (interrupt) |
| death_{dir} (4) | DeathTrigger Any State | NO | no exit (final) |
| iron_charge_{dir} (8) | IronChargeTrigger | NO | 1 |
| earthsplitter_{dir} (8) | EarthsplitterTrigger | NO | 1 |
| gravity_cleave_{dir} (8) | GravityCleaveTrigger | NO | 1 |
| death_blow_{dir} (8) | DeathBlowTrigger | NO | 1 |
| iron_counter_{dir} (8) | IronCounterTrigger (hold) | NO | 0 (hold→one-shot reflect) |
| sunder_mark_{dir} (8) | SunderMarkTrigger | NO | 1 |

**Parameter count:** ~10 trigger + 1 Speed float + 1 Direction int (veya BlendTree). Direction int için **1D BlendTree** önerilir (8-dir interpolation).

**Transition count:** ~30-40 (state-to-state direct + Any State for hurt/death).

**Tahmin:** Animator setup ~1-2 saat ek work post-asset, kod TODO yok (LOC = 0, asset wire only).

---

## 8. Risk + Open Question (5 kritik)

1. **Cost realist:** Tier 1 + Tier 2 south-only **≈ 148-272 gen** (S114 V3 mapping). 5-dir expansion ×5 = **~740-1360 gen**. PixelLab credit balance check ZORUNLU başlangıçta (`mcp__pixellab__get_balance` veya Web UI'da görünür).

2. **Apex State 20-40 gen — RIMA `feedback_state_vs_n_frames_cost_lock` 4-8x pahalı uyarısı:** Split-animation pipeline'da apex state **MANDATORY**, alternatif yok (pixel hash lock pixel-perfect alignment için). Trade-off kabul.

3. **flipX silah sağ/sol el dönüşü:** Warblade body weaponless → flipX güvenli (armor symmetric). Greatsword weapon prefab ayrı render edilir; WeaponSorter + OrientationSync flipX ile doğru mirror (Karar #144+#123+#146 LIVE). Ronin katana + sheath asymmetric → 8-dir native gen gerekebilir (Faz 4 Ronin'de revisit).

4. **m_LoopTime mevcut clip'lerde 0:** Mevcut 8 idle clip single-frame, loop kapalı. Yeni multi-frame clip'lerde m_LoopTime = 1 ZORUNLU (Idle/Walk/Run için), 0 (Attack/Hurt/Death/Skill için).

5. **PixelLab V3 "Keep first frame" + Custom Frames Start/End** entegrasyonu confirm: V3 panel'inde Custom Frames Advanced açıkken Start ve End frame **karakter character_state'inden gelir** mi yoksa **upload PNG** mi? Kullanıcı UI'da test etmeli. NLM canonical: Stage 2 End Frame = Apex State, Stage 3 First Frame = Apex State → bu Custom Frames slot'larına Apex State image yüklenir.

---

## 9. Triple-AI Summary

**agy (yasinderyabilgin, indie benchmark + V3 best practice):**
> Idle 6f (RIMA 4f override OK), Walk 8f Pixar standart, Attack 8f windup-hold-follow (Hades 3-1-3-3), Death 10f dramatic (RIMA 6f cap, pixel budget zorunluluk). Keep first ON loop / OFF one-shot. 5+3 mirror cost-optimal. Apex hitstop 2-3f hold = "vuruş hissi". V3 + reference_image n_frames birleşik MANDATORY.

**NLM (RIMA canonical — Karar #120/#114/#71/#100/#108):**
> Split-animation pipeline LOCKED frame ≥12 + apex var → 3-stage (Apex State + Part 1 + Part 2). Pixel budget 252×252 cap 8 frame. 7 canonical anim (Idle/Walk/Attack_LMB/Attack_RMB/Dash/Hurt/Death) + Summoner Summon_Cast. Animator timing 40ms apex / 80-100ms diğer. animate_character MCP YASAK. character_state OK. Custom V3 1-9 gen (v0.4.92+). Prompt structure [CHARACTER]/[ACTION]/[CONSTRAINTS] 3-blok.

**Opus sentez (rima-design):**
Warblade Demo Faz 1 için ANIMATION_PROMPT_CATALOG = (a) Tier 1 5 anim south-only (Idle 4f, Walk 8f, Basic Attack 8f SPLIT, Hurt 4f, Death 6f), (b) Tier 2 6 skill anim south-only (Iron Charge / Earthsplitter / Gravity Cleave / Death Blow / Iron Counter / Sunder Mark — 5 SPLIT + 1 single), (c) 6 Apex character_state gen ZORUNLU split-pipeline için (120-240 gen tek seferde), (d) Tier 1 south-only total ~28-52 gen + Tier 2 south-only ~120-220 gen = **toplam ~148-272 gen ilk pass** (S114 V3 mapping), (e) 5-dir expansion sonradan ~5x cost. Kullanıcı PixelLab Web UI'da "Add Animation" → built-in (Idle/Walk/Hurt) veya Custom V3 (Attack/Skill) seçer, [CHARACTER]+[ACTION]+[CONSTRAINTS] 3-blok prompt yapıştırır, Apex State image'i Custom Frames Start/End'e yükler, Generate. Cost-realist düşürmek için: önce 4-5 anim south-only validate, sonra batch.

---

## 10. Sonraki Adım (kullanıcı action)

1. **Bu katalogu oku**, prompt template'leri + frame count + cost beklentileri onayla
2. **PixelLab credit balance check** (Web UI veya `mcp__pixellab__get_balance` — eğer gece halt user-approved exception izin verirse)
3. **Phase 1 başla** — south-only 5 anim (Idle/Walk/Hurt/Death + Basic Attack SPLIT)
4. Built-in Idle/Walking/Reactions ucuz (1-3 gen), Custom V3 Attack pahalı (Apex State 20-40)
5. Her anim **Web UI'da generate → preview → onay** → Unity import → Animator wire
6. Phase 1 onayı sonrası Phase 2 (Tier 2 skill) → Phase 3 (5-dir) → Faz 4 9-class

---

## Cross-link
[[warblade_animation_states_demo_phase1_plan]] [[nine_class_animation_states_demo_phase1_plan]] [[warblade_12_common_skills_spec]] [[weapon_master_spec_10_class]] [[feedback_state_gen_mcp_user_approval_exception]] [[feedback_state_vs_n_frames_cost_lock]] [[feedback_no_pixellab_night_autonomous]] [[project_demo_phase1_milestone_lock]] [[reference_pixellab_prompt_grammar]] [[feedback_pixellab_mcp_halt_strict]]

**Canonical references (NLM citations):**
- Karar #120 (Split-animation pipeline LOCKED 2026-05-13)
- Karar #114 (5+3 mirror LOCKED)
- Karar #71 (Weapon lock — WEAPONLESS PRODUCTION'DA UYGULANMAZ, weapon mount Karar #144+#123+#146 ile yapılır)
- Karar #100 (124×124 chibi 35deg)
- Karar #108 (SUPERSEDED S114 — yeni cost mapping: 4f=1/6-8f=2/10-12f=3/14-16f=4 gen/dir)
- Pixel budget formula 524,288 cap
- Karar #71 weapon lock prompt blok (forbidden words list)
- 2026-05-02 animate_character MCP kalıcı yasak
- v0.4.92 animate_with_text v3 cost 1-9 gen (v0.4.69 era 40 gen revoked)
