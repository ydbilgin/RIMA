---
name: rima-character-sprite-system
description: Weaponless body + Unity WeaponChild SR (Karar
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

> WARNING: DEPRECATED 2026-05-17. See [[weaponless-animation-v1]] (Karar #144) for current spec. Aseprite composite ref is obsolete.

> **Active spec:** [[weaponless-animation-v1]] + Karar #144 (Karar #71 + #73 REVOKE).
> Current pipeline: PixelLab Create Image Pro (silahsiz body) + Unity child WeaponSR (Karar #144).
> Aseprite was V0 pipeline; replaced by PixelLab. Aseprite still used for frame assembly/cleanup only.

* **Architecture (Karar #144):** Body sprite SILAHSIZ uretilir. Silah Unity'de child SpriteRenderer olarak parent karaktere baglanir. Body animation silahsiz kol hareketi gosterir; weapon child SR kendi animasyonunu oynar; senkron Animation Events + HandAnchorMap ScriptableObject.
* **Class weapons:** Warblade (Greatsword), Shadowblade (Dual Dagger), Ranger (Bow), Ronin (Katana), Gunslinger (Dual Pistols), Ravager (Dual Axes), Summoner (Scepter), Hexer (Grimoire), Brawler (Unarmed — no weapon child). Elementalist: body unarmed, Unity VFX disc (no weapon SR).
* **Exceptions:** Brawler = bare fists (no weapon SR). Elementalist = Unity VFX disc. Ronin = sheath on body sprite (kimlik), drawn sword = weapon SR.
* **Cross-Class Ghost (Soul Shader):**
  - Logic: Ghost sprite appears during skill use (Shadow Echo layer).
  - FX: Desaturate + 0.6 Alpha + Emission.
  - Colors: Warblade (Cold blue), Shadowblade (Violet), Elementalist (Orange), Ranger (Green).
