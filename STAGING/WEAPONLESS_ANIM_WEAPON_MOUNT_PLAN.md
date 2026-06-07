# WEAPONLESS ANIM + WEAPON PRODUCTION + HANDANCHOR MOUNT PLAN
**Tarih:** 2026-05-28  
**Author:** Sonnet orchestrator (triple-AI sentez: codebase verify + industry research + design synthesis)  
**Status:** PLAN — asset gen yok. Kullanıcı action listesi Bölüm 4'te.

> ⚠️ **SUPERSEDED / REFERENCE-ONLY (weapon PPU + sıra):** Weapon PPU bu plandaki 100 değil — **canonical PPU = 64** (body ile uyumlu, HandAnchor offset). Ayrıca üretim sırası **VFX-first / graybox-combat-first** olarak TERS çevrildi. Her ikisi de `STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md` (2026-05-28, triple-AI lock, kullanıcı onayı) ile kilitli. Bu plan yalnızca anim prompt blokları + HandAnchor mount geçmişi için referanstır; PPU/sıra için LOCK doc'una bak.
**Sources:** Codebase verify (OrientationSync.cs, HandAnchorAttach.cs, WeaponSorter.cs, SpriteHandData.cs, FacingDir8.cs, WeaponDatabaseSO.cs), warblade sprite görsel inceleme, ANIMATION_PROMPT_CATALOG.md canonical

---

## CRITICAL FINDING: Warblade body ZATEN silahsız

**`warblade_south.png` görsel inceleme sonucu: karakter eli BOŞ, silah YOK.**

- Mevcut 8 rotation sprite (south/east/north/west/SE/NE/NW/SW) tümü weaponless body.
- `Assets/Art/Weapons/` klasörü YOK — weapon PNG asset henüz üretilmedi.
- `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` var ama sprite referansı boş olabilir.
- Warblade sprite ~120×120 chibi, bulky armor, messy black hair — ANIMATION_PROMPT_CATALOG'daki CHARACTER block ile tam uyumlu.

**Bu durumun anlamı:**  
Animasyon üretiminde WEAPON LOCK rule kaldırılır. Mevcut CHARACTER block'tan greatsword satırları temizlenir. HandAnchor weapon overlay sistemi hazır bekliyor, sadece weapon asset üretilip Unity'e import edilmesi gerekiyor.

---

## Bölüm 1 — Silahsız Animasyon Prompt Update

### 1.1 Mevcut [CHARACTER] Block (ANIMATION_PROMPT_CATALOG'dan, GÜNCEL DEĞİL)

```
64x64 chibi top-down character, male heavy warrior with two-handed greatsword,
dark steel armor uniform with bulky shoulder pads, brown leather straps,
light skin, messy black hair, stern neutral face,
view 35 degree high top-down ARPG angle,
dark brown body #4F3A2C brass accent #C09455
```

### 1.2 Yeni [CHARACTER] Block — Weaponless (TÜM 11 ANİM İÇİN)

```
64x64 chibi top-down character, male heavy warrior,
dark steel armor uniform with bulky shoulder pads, brown leather straps,
light skin, messy black hair, stern neutral face,
EMPTY HANDS, fists loosely clenched, no held items, no weapons,
right hand carries weapon-ready grip posture (open palm, slightly curved),
view 35 degree high top-down ARPG angle,
dark brown body #4F3A2C brass accent #C09455
```

**Değişiklikler:**
- "two-handed greatsword" → KALDIRILDI
- "EMPTY HANDS, fists loosely clenched, no held items, no weapons" → EKLENDİ
- "right hand carries weapon-ready grip posture" → EKLENDİ (HandAnchor placement için)

### 1.3 Yeni [CONSTRAINTS] Block — Weaponless (TÜM 11 ANİM İÇİN)

```
2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
don't fill canvas, leave wide transparent headroom.
NO weapons, NO held items, NO sword, NO shield, hands empty throughout all frames.
```

**Değişiklikler:**
- WEAPON LOCK satırı → KALDIRILDI (silah yoksa gereksiz)
- "NO weapons, NO held items, NO sword, NO shield, hands empty throughout all frames." → EKLENDİ

### 1.4 Negative Prompt (Weaponless için)

```
no weapons, no sword, no shield, no dagger, no held objects, no anti-aliasing,
no blurry edges, no background, no extra characters, no items in hands
```

---

## 1.5 Her Animasyon İçin Güncellenmiş [ACTION] Block

### A — Idle (weaponless variant)

```
[ACTION]: subtle idle breathing, slight body weight shift left to right,
arms relaxed at sides with hands in loose fist, weapon-ready grip on right hand.
4-frame ping-pong loop (1→4→1). Feet firmly planted.
```
- Keep First Frame: ON
- Frame Count: 4
- Type: Built-in Idle 2 opt (preferred) veya Custom V3

---

### B — Walk (weaponless variant)

```
[ACTION]: confident walking cycle, arms swing naturally at sides,
right hand in loose weapon-ready grip, hip sway moderate,
8-frame contact-passing-contact loop.
Frame 1: right foot forward, left arm forward. Frame 5 (opposite): left foot forward, right arm forward.
```
- Brian's Extreme Pose method aynı kalır (Pose A + flipX Pose B)
- Frame Count: 8, Keep First: ON

---

### C — Basic Attack (weaponless — motion guide only)

```
[ACTION]:
Frame 1: right arm raised high above right shoulder, elbow bent, hand open palm facing left,
weight shifting to right foot, body coiling.
Frame 4 (apex): right arm fully extended in wide horizontal arc sweeping left,
hand open-palm reaching maximum extension, body fully rotated, weight on left foot.
Frame 8: right arm low at right hip, hand returning to rest fist,
body returning to forward-facing stance.
The right arm motion traces the path a greatsword would take in a full horizontal swing.
```
- SPLIT pipeline aynı kalır (apex frame shared)
- Apex State: `warblade_attack_LMB_apex_weaponless_state` (8-dir, 20-40 gen)
- Part 1 End Frame = apex state | Part 2 First Frame = apex state

---

### D — Hurt (weaponless variant)

```
[ACTION]:
Frame 1: body recoil sharply backward from frontal impact, right arm flailing outward,
left arm raised instinctively as guard, torso twisted back.
Frame 4: body returning to upright idle stance, arms settling back to sides.
Snappy recoil, no weapon interaction.
```
- Frame Count: 4, No loop

---

### E — Death (weaponless variant)

```
[ACTION]:
Frame 1: standing, hand on chest, head bowing, knees buckling.
Frame 6: collapsed forward face-down on ground, right arm extended,
left arm tucked under torso, final resting position.
Slow, dramatic fall — no weapon involved.
```
- Frame Count: 6, End Frame: death_pose_state (optional)

---

### F — Iron Charge (weaponless variant)

```
[ACTION]:
Frame 1: crouch into low sprint stance, right arm trailing behind right shoulder
in weapon-grip pose, body leaning aggressively forward, left arm driving forward.
Frame 4 (apex): full shoulder slam forward, right arm driving forward at chest height,
ground crack underfoot, brief golden dust trail.
Frame 8: standing upright, right arm raised mid-follow-through, recovery stance.
Right arm motion mirrors where greatsword would trail in a charge.
```
- SPLIT pipeline, apex = charge_impact_state

---

### G — Earthsplitter (weaponless variant)

```
[ACTION]:
Frame 1: both arms raised high overhead in maximum windup, two-hand grip pose
(right and left fists clasped together above head), body leaning back,
maximum stretch.
Frame 4 (apex): massive downward slam with both hands driving toward ground,
dust burst around impact point, ground crack radiating outward, body fully committed.
Frame 8: pulling both arms up from ground, recovery to standing stance.
Two-hand clasped grip throughout — greatsword will be placed between hands.
```
- SPLIT pipeline, apex = sword_planted_state (rename: earthsplitter_apex_state)

---

### H — Gravity Cleave (weaponless variant)

```
[ACTION]:
Frame 1: wide stance, right arm lowered and extended to left side at hip level,
body wound up for spin, feet planted.
Frame 4 (apex): body in violent 360-degree rotation mid-spin,
right arm at full horizontal extension reaching maximum arc, golden energy trail visible,
debris pulled inward (environmental VFX only, not character-held).
Frame 8: right arm resting at right side, body back to forward facing, recovery.
```
- SPLIT pipeline, apex = spin_release_state

---

### I — Death Blow (weaponless variant)

```
[ACTION]:
Frame 1: low crouch, full torso twist to right, right arm fully wound back
behind right shoulder in grip pose, red rage aura glowing around body, intense.
Frame 5 (apex): devastating horizontal execution swing, full body rotation,
right arm extended fully to left at maximum reach, red glow trail, debris kick.
Frame 8: right arm resting low at right side, body forward, exhausted recovery stance.
```
- SPLIT pipeline, apex = finisher_strike_state
- Animator timing: apex 60ms hold (dramatic)

---

### J — Iron Counter (weaponless variant)

```
[ACTION]:
Frame 1: defensive block stance, both forearms raised horizontally as shield barrier,
weight back on right foot, alert.
Frame 3 (apex): incoming attack contact moment, arms bracing impact,
sparks from contact point (environmental VFX), shoulder shuddering.
Frame 6: instant counter-slash motion with right arm sweeping outward in horizontal arc,
reflective gold glow emanating from right fist.
```
- SPLIT optional, apex = parry_block_state

---

### K — Sunder Mark (weaponless variant)

```
[ACTION]:
Frame 1: right arm extended forward pointing toward target, fingers spread,
left hand raised to chin in focus gesture.
Frame 6: runic sigil burst from right fingertips/fist toward target direction,
cyan armor-break glyph materializes, right arm holds extension.
```
- Single-pass, no split

---

## Bölüm 2 — Silah Asset Production Spec

### 2.1 Canvas ve PPU Spec (chibi 120×120 body için)

**Codebase gerçeği:** Mevcut body sprite ~120×120 chibi (252×252 PixelLab canvas, karakter ~%60 = ~150px). OrientationSync.cs handOffsets world units cinsinden (0.08-0.10 birim = ~8-10 px at standard 100 PPU).

**Weapon canvas kararı:**

| Weapon boyutu | Canvas | PPU | Açıklama |
|---|---|---|---|
| Greatsword (2H büyük) | **128×256 px** | 100 | Uzun silah, dikey uzun canvas. Warblade body height ~120px, greatsword ~2x boy. |
| Katana (1H orta-uzun) | **64×192 px** | 100 | Ronin için |
| Dagger (1H kısa) | **48×96 px** | 100 | Shadowblade için |
| Staff (2H uzun) | **64×256 px** | 100 | Elementalist için |
| Bow (2H yatay) | **128×192 px** | 100 | Ranger için, yatay ağırlıklı |
| Greataxe (2H geniş) | **128×192 px** | 100 | Ravager için, geniş baş |
| Whip (1H uzun) | **96×192 px** | 100 | Hexer için, kıvrık form |
| Gauntlets (2× 1H) | **64×64 px** × 2 | 100 | Brawler için, birer el |
| Pistol/SMG (1H) | **96×64 px** | 100 | Gunslinger, yatay |
| Tome+Orb (1H her biri) | **64×64 px** × 2 | 100 | Summoner için |

**Pivot noktası (tüm silahlar için kural):**  
Sprite pivot = grip handle'in ortası. Weapon'ı HandAnchor'a parent ederken localPosition = (0,0) olacak şekilde pivot ayarlı olmalı. Unity Sprite Editor'da pivot "Custom" yapılır.

**Greatsword için pivot:** PNG'nin alt-merkezi değil, handle'in geometrik ortası — tipik olarak PNG height'ın %20-25'i yukarıdan (grip alt kısmı).

---

### 2.2 Weapon Asset Envanteri (mevcut durum)

| Silah | PNG path | Prefab | Status |
|---|---|---|---|
| Greatsword (Warblade) | YOK — üretilmedi | `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` (sprite boş) | ÜRETILMELI |
| Katana (Ronin) | YOK | YOK | Faz 4 |
| Pistol (Gunslinger) | YOK | YOK | Faz 4 |
| Bow (Ranger) | YOK | YOK | Faz 4 |
| Staff (Elementalist) | YOK | YOK | Faz 4 |
| Dagger (Shadowblade) | YOK | YOK | Faz 4 |
| Greataxe (Ravager) | YOK | YOK | Faz 4 |
| Whip (Hexer) | YOK | YOK | Faz 4 |
| Gauntlets (Brawler) | YOK | YOK | Faz 4 |
| Tome+Orb (Summoner) | YOK | YOK | Faz 4 |

**PixelLab asset ID referansları (memorydeki eski üretimler):**
- Warblade longsword: `441bccf0` — eski üretim, verify gerekli (greatsword için uygun mu?)
- Ronin katana: `692f43ce` — Faz 4 için

**Üretim önceliği:**
1. **Faz 1 Demo:** SADECE Warblade Greatsword — canvas 128×256, handle pivot
2. **Faz 4:** 9 class silah

---

### 2.3 Warblade Greatsword PixelLab Üretim Spec

**PixelLab araç:** `create_map_object` (NOT `create_character` — silahlar character değil)  
**Canvas:** 512×512 PixelLab canvas (max), output 128×256 effective sprite içinde  
**MCP rule:** Kullanıcı Web UI'da manuel üretim (PixelLab gen halt rule — gece otonom YASAK)

**PixelLab Web UI prompt (Warblade Greatsword):**

```
[OBJECT]: A large two-handed fantasy greatsword weapon item,
dark steel blade with slight runes etched near crossguard,
brass accent crossguard #C09455, dark leather-wrapped handle grip,
blade is straight double-edged, slightly wider near base tapering to sharp point.
Top-down view at 35 degree ARPG angle as if lying flat on surface.
Separated weapon item only, no character, no hand holding it.

[SPECS]: 128x256 pixel art sprite, handle at bottom 25% of canvas,
blade extending upward 75% of canvas height.
Pivot point: center of handle grip.
Transparent background.

[STYLE]: 2D pixel art, clean hard edges, no anti-aliasing,
dark steel #2A2A3A with slight blue-grey highlight,
brass crossguard #C09455, leather handle #4F3A2C.
Style consistent with chibi dungeon RPG character sprite set.
Clean pixel clusters, no noise.
```

**Alternatif yöntem:** ChatGPT `$imagegen` → Codex pixel cleanup → PixelLab Remove BG → Unity import

---

## Bölüm 3 — HandAnchor Mount System (Codebase Reality)

### 3.1 Mevcut Sistem Özeti (LIVE)

**OrientationSync.cs** — 8-dir handOffset + weaponRotation tablosu LIVE:

```
FacingDir8 enum sırası: S(0), SE(1), E(2), NE(3), N(4), NW(5), W(6), SW(7)

handOffsets (world units, body 1 unit = ~100px):
  S(0):  (0.00, -0.08)   → karakter önü
  SE(1): (0.08, -0.04)   → sağ-ön
  E(2):  (0.10,  0.00)   → sağ
  NE(3): (0.07,  0.05)   → sağ-arka
  N(4):  (0.00,  0.08)   → arka
  NW(5): (-0.07, 0.05)   → sol-arka
  W(6):  (-0.10, 0.00)   → sol
  SW(7): (-0.08, -0.04)  → sol-ön

weaponRotations (Euler Z-axis degrees):
  S(0):  -90°
  SE(1): -45°
  E(2):    0°
  NE(3):  45°
  N(4):   90°
  NW(5): 135°
  W(6):  180°
  SW(7): -135°
```

**WeaponSorter.cs** — sort order logic:
- `N, NE, NW` → weapon `bodyOrder - 1` (arkada, karakter önünde görünür)
- Diğer yönler → weapon `bodyOrder + 1` (önde)

**HandAnchorAttach.cs** — Level 1 (static) + Level 2 (SpriteHandData per-sprite pixel):
- Level 1: weapon prefab `handAnchor` transform'a parent, `anchorOffset` + `gripOffset` ayarlanır
- Level 2: her sprite frame için SpriteHandData SO → pixel-accurate hand position
- Warblade two-hand: `twoHanded=true` + `orientBetweenHands=true` → weapon iki el ortasına yerleşir

**WeaponDatabaseSO.cs** — class+formId ile weapon lookup:
- `classId = "Warblade"`, `formId = "Base"` → Greatsword prefab entry

**SpriteHandData.cs** — per-sprite hand pixel koordinatı:
- `handLeftPx`, `handRightPx` → pixel coordinates of left/right hand in sprite
- Level 2 mode'da her anim frame için ayrı SpriteHandData SO gerekli

---

### 3.2 8-Dir Offset Table (Warblade Greatsword, Codebase-Verified)

Bu tablo **OrientationSync.cs kaynak değerlerine dayalı** (placeholder DEĞİL — LIVE codebase'den):

| Direction | handOffset (world units) | handOffset (px @ 100 PPU) | weaponRotation | WeaponSorter | flipX (sprite) |
|---|---|---|---|---|---|
| S | (0.00, -0.08) | (0, -8) | -90° | front (+1) | false |
| SE | (0.08, -0.04) | (8, -4) | -45° | front (+1) | false |
| E | (0.10, 0.00) | (10, 0) | 0° | front (+1) | false |
| NE | (0.07, 0.05) | (7, 5) | 45° | behind (-1) | false |
| N | (0.00, 0.08) | (0, 8) | 90° | behind (-1) | false |
| NW | (-0.07, 0.05) | (-7, 5) | 135° | behind (-1) | true (mirror) |
| W | (-0.10, 0.00) | (-10, 0) | 180° | front (+1) | true (mirror) |
| SW | (-0.08, -0.04) | (-8, -4) | -135° | front (+1) | true (mirror) |

**Not:** flipX karakter sprite'ının mirrored olduğu durumlarda uygulanır (Karar #114 — W/SW/NW 3 yön mirror). Weapon flipX body ile birlikte SpriteRenderer.flipX = true → weapon otomatik mirror.

---

### 3.3 Greatsword WeaponDatabaseSO Entry Spec

Warblade.prefab'a wired edilecek `WeaponDatabaseSO` entry'si:

```
classId: "Warblade"
formId: "Base"
weaponPrefab: Assets/Prefabs/Weapons/Warblade_Greatsword.prefab
anchorOffset: (0, 0, 0)      ← Level 1 static, sprite pivot'a güven
gripOffset: (0, -0.05, 0)    ← greatsword ağırlık merkezi yukarı kaydırma, test ile ayarla
twoHanded: true
orientBetweenHands: false    ← Level 1 statik, rotation OrientationSync'ten gelir
orientationOffsetDegrees: 0
handOffsets[8]: OrientationSync'teki değerler (override gerekmez, WeaponDatabaseSO.handOffsets boş kalabilir Level 1'de)
```

---

### 3.4 Level 1 vs Level 2 — Faz 1 Kararı

**Faz 1 Demo için:** **Level 1 Static** kullan.
- SpriteHandData SO'ları GEREKMEZ (her frame için üretme)
- HandAnchor transform fixed position, weapon parent
- OrientationSync per-frame çağrılır (PlayerAnimator.cs → Sync(dir))
- WeaponSorter.UpdateSort(dir) çağrılır

**Level 2 (SpriteHandData per-sprite pixel):** Faz 2+ için, animasyon bitince her unique frame için pixel-accurate pivot.

---

### 3.5 PlayerAnimator.cs Integration Check

`OrientationSync.Sync(FacingDir8)` çağrısının nerede yapıldığını kontrol etmek gerekiyor. Mevcut kod `PlayerAnimator.cs` içinde olması beklenir. Bu dosya görülmedi fakat `Assets/Scripts/Player/PlayerAnimator.cs` mevcut.

**Action:** PlayerAnimator.cs'e `OrientationSync` + `WeaponSorter` çağrısı eklenip eklenmediğini verify et (Bölüm 4 TODO #5).

---

## Bölüm 4 — TODO Listesi (YAPILACAKLAR — Faz 1 Demo Sıra)

### BLOK A — Hazırlık (engine side, önce)

**A1. Greatsword PNG üret (PixelLab Web UI, kullanıcı manuel)**
- Amaç: weapon asset yoksa mount sistemi test edilemez
- Spec: 128×256 canvas, handle bottom-25%, blade upward, transparent BG
- Prompt: Bölüm 2.3'teki spec kullan
- Sonra: Remove BG açık olduğu için arka plan zaten temiz
- Output: greatsword_base.png → `Assets/Art/Weapons/Warblade_Greatsword.png`

**A2. Greatsword PNG Unity import + Sprite ayarla**
- Amaç: prefab'ta SpriteRenderer'ı besle
- Adımlar: Import → Sprite mode Single → PPU=100 → Pivot=Custom → handle ortasına koy → Apply
- Path: `Assets/Art/Weapons/Warblade_Greatsword.png`
- Warblade_Greatsword.prefab'a SpriteRenderer sprite ref bağla

**A3. WeaponDatabaseSO entry doldur**
- Amaç: HandAnchorAttach.cs'in weapon'ı spawn etmesi için
- classId="Warblade", formId="Base", weaponPrefab=Warblade_Greatsword.prefab
- gripOffset PlayMode'da ince ayar gerektir

**A4. Warblade.prefab HandAnchorAttach wire**
- Amaç: HandAnchorAttach + OrientationSync + WeaponSorter component'ları Warblade.prefab'ta mevcut mu?
- Check: Warblade.prefab → Inspector → HandAnchorAttach.cs componenti var mı?
- Eğer yoksa: Warblade.prefab'a ekle, handAnchor Transform slot'una HandAnchor child objesini bağla
- weaponDatabase SO'yu bağla

---

### BLOK B — Animasyon üretimi (PixelLab Web UI, kullanıcı manuel)

**B1. Weaponless CHARACTER block güncelle** ← Bu dosyanın Bölüm 1.2
- Amaç: ANIMATION_PROMPT_CATALOG'daki tüm 11 anim için CHARACTER block değişir
- Action: Kullanıcı PixelLab'da character sayfasını aç → description edit → "two-handed greatsword" sil → "EMPTY HANDS, fists loosely clenched" ekle

**B2. Phase 1 — Idle south üret** (4f, Built-in)
- Weaponless CHARACTER block ile
- Built-in Idle 2 opt, 4 frame, Keep First ON
- Start Frame: mevcut warblade_south.png

**B3. Phase 1 — Walk south üret** (8f)
- Brian's Extreme Pose method
- Animate with Text V3 → Pose A seç → Aseprite flipX Pose B → Interpolate

**B4. Phase 1 — Hurt south üret** (4f, Built-in Reactions)
- Weaponless variant, Bölüm 1.5-D'deki ACTION block

**B5. Phase 1 — Death south üret** (6f, Custom V3)
- Weaponless variant, Bölüm 1.5-E'deki ACTION block

**B6. Phase 1 — Basic Attack Apex State üret** (8-dir, 20-40 gen, MCP user-approved)
- `create_character_state` → warblade_attack_LMB_apex_weaponless_state
- Weaponless apex: right arm full horizontal extension, open palm

**B7. Phase 1 — Basic Attack south (SPLIT) üret**
- Part 1: windup → apex (Bölüm 1.5-C Part 1 ACTION block)
- Part 2: apex → recovery (Bölüm 1.5-C Part 2 ACTION block)

---

### BLOK C — Unity mount test (B1 + A1-A4 tamamlandıktan sonra)

**C1. PlayMode test: Body + Greatsword mount**
- Amaç: Weapon doğru elde görünüyor mu?
- Test: WASD hareket → 8 yön → weapon hand'de mi kalıyor?
- HandAnchorAttach Level 1 static: weapon sabit anchor'da

**C2. WeaponSorter sort order verify**
- N/NE/NW'de weapon karakter arkasına geçiyor mu?
- S/SE/E'de weapon önde mi?

**C3. OrientationSync 8-dir rotation verify**
- Her yönde weapon doğru açıda mı?
- Özellikle S (-90°) ve N (90°) kontrol et

**C4. PlayerAnimator.cs OrientationSync çağrısı verify**
- `Assets/Scripts/Player/PlayerAnimator.cs` → `OrientationSync.Sync(dir)` var mı?
- WeaponSorter.UpdateSort(dir) var mı?
- Eğer yoksa: PlayerAnimator.cs'e ekle (5-10 LOC)

---

### BLOK D — Faz 4 (demo sonrası, sıra)

**D1. Tier 2 skill animasyonları (B6 sonrası, sıra)**
- Iron Charge → Earthsplitter → Gravity Cleave → Death Blow → Iron Counter → Sunder Mark
- Her biri için weaponless variant prompt (Bölüm 1.5)

**D2. Multi-view expansion (Tier 1+2 tümü için)**
- South → SE, E, NE, N (5 yön)
- W/SW/NW: flipX mirror (gen gerekmez)

**D3. 9 class silah üretimi**
- Sıra: Elementalist Staff → Ranger Bow → Shadowblade Dagger × 2 → Ronin Katana → Gunslinger Pistol → Ravager Greataxe → Hexer Whip → Brawler Gauntlets × 2 → Summoner Tome+Orb × 2
- Her biri için WeaponDatabaseSO entry + Prefab

---

## Bölüm 5 — Risk + Open Questions

### R1 — PixelLab credit balance (KRITIK)
Tier 1 south-only weaponless: ~31-58 gen. Apex State pahalı (20-40 gen tek bir state için). Credit kontrol ZORUNLU başlamadan önce.

### R2 — gripOffset ince ayar
`HandAnchorAttach.cs` Level 1'de `anchorOffset` + `gripOffset` var. Greatsword için `gripOffset.y` negatif (sword'u yukarı kaydır) değer PlayMode'da görsel olarak ayarlanmalı. Başlangıç değeri: `(0, 0.05, 0)` önerisi.

### R3 — PlayerAnimator.cs'te OrientationSync/WeaponSorter çağrısı
`PlayerAnimator.cs` okunmadı. Bu dosyada `OrientationSync.Sync(dir)` çağrısı VAR MI check edilmeli. Eğer yoksa kısa ek gerekir.

### R4 — Warblade PixelLab asset `441bccf0` (longsword)
Memory'de longsword var. Greatsword farklı (daha büyük, two-hand). Bu asset greatsword için USE EDILEBILIR mi yoksa yeni üretim mi? Kullanıcı PixelLab'da asset'i incele → boyut ve stil uyuyorsa reuse, uymuyorsa yeni üretim.

### R5 — SpriteHandData (Level 2) animasyon için
Level 2 per-sprite hand data → her animasyon frame'i için ayrı SpriteHandData SO gerekir. Bu yüzden Faz 1'de Level 1 static ile başlamak doğru karar. Level 2 ekstra iş ve ancak animasyonlar bitince yapılır.

### R6 — Two-handed weapon visual correctness
Greatsword two-hand: `twoHanded=true` → `HandAnchorAttach.cs` iki el ortasını hesaplar (Level 2 modda). Level 1 static modda ise weapon sadece `handAnchor.localPosition + anchorOffset`. Two-hand visual için Level 1'de weapon'ı iki elin arasına getirecek şekilde `anchorOffset` manuel ayarla.

---

## Bölüm 6 — Sentez: Sıra Özeti

```
1. [KULLANICI] PixelLab credit balance kontrol et
2. [KULLANICI] PixelLab Web UI → Warblade character description → greatsword → weaponless update
3. [KULLANICI] Greatsword PNG üret (Web UI, Bölüm 2.3 prompt)
4. [KULLANICI] greatsword PNG Unity import + pivot ayarla
5. [KOD VERIFY] PlayerAnimator.cs OrientationSync/WeaponSorter çağrısı var mı? (A4)
6. [KULLANICI] WeaponDatabaseSO Warblade entry doldur
7. [KULLANICI] Warblade.prefab HandAnchorAttach wire check/add
8. [KULLANICI] Phase 1 animasyonları üret (B2-B7 sıra)
9. [KULLANICI] PlayMode test: weapon mount + 8-dir rotation (C1-C4)
10. [KULLANICI] Phase 2 Tier 2 skill animasyonlar (D1)
11. [KULLANICI] Multi-view expansion (D2)
12. [GELECEK] 9 class silah Faz 4 (D3)
```

---

## Cross-links

- `STAGING/ANIMATION_PROMPT_CATALOG.md` — 11 anim Warblade katalog (bu doc Bölüm 1.5 action block'larını replaces/extends)
- `Assets/Scripts/Combat/OrientationSync.cs` — 8-dir offset table kaynak
- `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` — Level 1/2 weapon attach logic
- `Assets/Scripts/Combat/WeaponSorter.cs` — sort order logic
- `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs` — weapon lookup SO
- `Assets/Scripts/Data/SpriteHandData.cs` — Level 2 per-sprite hand pixel data
- `Assets/Scripts/Core/FacingDir8.cs` — enum sırası S=0..SW=7
- `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` — sprite boş, doldurulacak
- `Assets/Prefabs/Characters/Warblade.prefab` — HandAnchorAttach wire edilecek

**Memory refs:**
- `project_weapon_pipeline_lock.md` — Karar #144/#123/#146 decouple LOCK
- `project_weapon_system_8dir_lock.md` — S100 LOCK HandAnchor+OrientationSync+WeaponSorter
