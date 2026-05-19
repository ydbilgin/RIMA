---
name: weaponless-animation-v1
description: "Karar #144 + #146 LOCK — silahsız body sprite + weapon child SR + input-driven puff system (S91 LOCK). Overrides Karar #71 + #73. Spec at STAGING/animation_spec_weaponless.md."
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

# Silahsız Karakter + Weapon Child SR — V1 Lock

**Status:** LOCK in this session (Karar #144 proposal). User'a iletildi, onay sonrası MASTER_KARAR_BELGESI'ne eklenecek.
**Date:** 2026-05-16 S86 SPRINT10.
**Overrides:** Karar #71 (silah hep elde single-state) + Karar #73 (silahlı 1-piece).
**Why:** Karar #73 64px body-only + WeaponAnchorMap'i REVOKE etmişti (AI variance + pixel-precise anchor imkansızlığı). Yeni karar **Karar #144** ile body silahsız + weapon ayrı child SR — modular weapon swap mümkün, body sprite reuse. Brawler silahsız sınıf KEEP (Karar #71 Brawler kısmı korunur). Ronin sheath/draw kimliği WeaponSR idle/attack state'leriyle korunur.
**How to apply:** Tüm karakter sprite üretiminde silah/hand-held obje YASAK. Body sadece kol hareketi gösterir. Unity'de WeaponSR child SR olarak parent karaktere bağlanır. Senkron: Animation Events + HandAnchorMap ScriptableObject.

## Sprite production decisions (LOCKED bu session)

- **Sprite boyutu:** 64×64 native (Karar #74 LOCK confirm, eski memory `128px-pivot-s43` STALE)
- **Direction:** 8 yön sprite üret (5 produce + 3 PixelLab mirror: S/SE/E/NE/N üret, SW/W/NW mirror)
- **Animation:** V1 = 4-cardinal animations (S/E/N + W flipX per Karar #53). 8-dir anim V2 polish.
- **Weapon production:** Silahsız body. PixelLab prompt'unda hand-held obje YASAK kuralı sabit.
- **Hand anchor:** Per sprite, right/left hand pixel koordinatları → `HandAnchorMap` ScriptableObject.

## Animation frame counts (V1, locked)

| Anim | Frame | FPS | Source |
|---|---|---|---|
| Idle | 4 | 4-6 | Karar #42 (idle interpolate first=last) |
| Run | 6 | 10-12 | Karar #42 (PixelLab Animate built-in) |
| Attack basic | 3 (3-seg) | impact=40ms, others 80-100ms | Karar #14 (KF+Interpolate) |
| Dash | 3 | 12-15 | Karar #58 |
| Hit reaction | 3 | impact=40ms | Karar #48 (4-dir only) |
| Death | 6-8 | 8-10 | Karar #48 |

## Per-class attack body motion (silahsız)

| Class | Body motion | Weapon child does | Impact frame |
|---|---|---|---|
| Warblade | iki elli yan slash sağ-üst→sol-alt | sword swings forward | F1 |
| Ravager | üstten dikey slam her iki el | axe overhead bring-down | F1 |
| Brawler | sağ kol jab + body step-in 4-8px | EMPTY (silahsız) — glow VFX opsiyonel | F1 |
| Ronin | sağ kalçadan iaido draw | katana sheath→strike | F1 |
| Shadowblade | sağ kol ileri thrust | dagger forward stab | F1 |
| Ranger | iki el bow draw + release | bow draw + arrow loose | F2 |
| Gunslinger | sağ kol kaldır + recoil | pistol raise + flash | F1 |
| Elementalist | sağ kol overhead cast | staff raise + cast point | F1 |
| Summoner | iki el ileri uzatma | crystal staff extend | F1 |
| Hexer | sağ kol curse gesture | EMPTY (silahsız caster) — curse glyph | F1 |

## Brawler özel
- Silah YOK (Karar #71 Brawler kısmı kept)
- Bandit Knight insight: Frame 1 impact'te body 4-8px forward translate ("step-in")
- Crit version: 6-10px translate
- Hitstop 80ms + 50ms micro = 130ms total "ağır" hit feel

## Ronin özel
- Sheath/draw kimliği KEEP
- WeaponSR idle state = sheath (kınında, görünür)
- WeaponSR attack state = drawn (elde)
- Body iaido draw motion ile sync

## Hand anchor convention (64×64)

| Direction | Right hand (x,y) | Left hand mirror |
|---|---|---|
| S | (42-44, 24-26) | (~20-22, 24-26) |
| SE | (44-46, 25-27) | (~18-20, 24-26) |
| E | (44-46, 24-26) | (sol el gizli, profil) |
| NE | (42-44, 26-28) | (~20-22, 26-28) |
| N | (22-24, 24-26) | (~40-42, 24-26) |
| Mirror W/SW/NW | x = 64 - original | x = 64 - original |

`HandAnchorMap` ScriptableObject: per class, per direction, right + left + optional weapon pivot offset.

## Weapon child sync mechanism

**Idle/Run:** WeaponSR position set from `HandAnchorMap.GetAnchor(currentDirection).rightHandPixel * PPU`. Run sırasında 6 keyframe sine wave bob (±1-2px).

**Attack/Skill:** BodyAnimator state events trigger WeaponAnimator transitions:
- F0: `WeaponAnimator.SetTrigger("AttackWindup")` — silah arkaya
- F1: `WeaponAnimator.SetTrigger("AttackStrike")` — silah ileri + hit detection açık
- F2: `WeaponAnimator.SetTrigger("AttackRecovery")` — silah geri

## V1 production scope (per FAZ_MASTER)

- Faz 1: 10 class × 8 yön idle base sprite (silahsız) + Warblade tam anim (idle/run/attack/dash/hit/death)
- Faz 2: +Elem/Shadow/Ranger tam anim
- Faz 3: +Ravager/Ronin/Gunslinger/Brawler tam anim
- Faz 4: +Summoner/Hexer tam anim

## Weapon Visibility Triggers (Karar #146 — S91 LOCK)

**WeaponSR default state:** hidden (alpha 0 / deactivated).

**Puff-in triggers (0.15s materialize):** LMB / RMB / Q / E / R / F / V — any combat input.
**Puff-out trigger (0.3s dissolve):** last-attack timer expires after 5s with no combat input.
**NOT triggers (puff excluded):** Space dash / WASD movement / Interact key — movement-only inputs.

## Ronin Exception (Karar #146)

Ronin DOES NOT use puff-in/puff-out. Identity = explicit iaido sheath/draw open animation.
- WeaponSR idle = katana in sheath (visible at hip), NOT hidden.
- Draw animation plays on first combat input of engagement.
- Re-sheath plays after 5s no-combat-input (animated, not dissolve).
- Karar #71 Ronin identity preserved through Karar #146.

## Cinematic and Hub Override (Karar #146)

- Cinematic scenes: explicit weapon visibility state set by authored cinematic layer (TASARIM/CINEMATIC_LAYER_v1.md).
- Hub / non-combat zones: weapon hidden by default, override allowed per authored scene config.

## Cross-links
[[combat-feel-research-combined]] [[anchor-selections-s43]] (stale — update needed)
[[idle-poses]] (stale — update needed)
[[animation-notes]] (stale — 128px → 64×64)
[[karar-143-layered-pipeline]]

## Spec file
`STAGING/animation_spec_weaponless.md` — full implementation spec with WeaponSR sync details.
