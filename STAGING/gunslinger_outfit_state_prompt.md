---
status: USER_APPROVED_BASE
faz: 1
tarih: 2026-05-18
ozet: "Gunslinger canonical base LOCKED — outfit-only state fix prompt (Karar #145 v2 Use #6)"
anchor_file: ANCHORS/characters/10_gunslinger.png
karar_ref: #145 v2 Use #6, #100 chibi 30-35°, #144 weaponless body
---

# Gunslinger — Outfit State Fix (USER APPROVED BASE)

## ✅ User Verdict
- **Anchor BASE LOCKED:** Dark-skin female, kısa koyu saç, chibi proportions, yüz/silüet/kamera açısı = **kusursuz, dokunulmayacak**
- **Sadece outfit değişecek:** Şu anki teal/blue cape → **canonical grey-purple trench + holsters + rift bandana + bone/feather accessories**

---

## PixelLab Use #6 State Prompt (copy-paste ready)

### Workflow
1. **PixelLab Web UI** → "create character" (eğer character ID yoksa) veya mevcut character'a "create state"
2. **State prompt** (identity-preserving):
3. **Strength setting:** 0.45-0.55 (orta — outfit değişimi için yeterli, face/body korumak için yeterli)

### State Prompt
```
Keep the EXACT SAME brown-skin Gunslinger female: same chibi face, same short dark hair, same body proportions, same calm idle pose, same 30-35 degree high top-down camera angle, same chibi big-head readable face. Do NOT change face, hair, body shape, or pose.

ONLY change the outfit:
- Replace the teal/blue cape with a long dark grey-purple trench coat reaching to mid-thigh, with the flaps falling evenly at the sides
- Add a rift-marked bandana tied around the neck with the knot visible at the side, dark crimson red with subtle hex-line accent
- Add bone-and-feather accessories visible on the collar and belt (a few small bone fragments + dark feather tufts hanging from a leather thong)
- Add tactical dark leather pistol holsters strapped to both upper thighs, holsters EMPTY (no visible gun, weapon-free rule), with brass buckle accents on the straps
- Boots stay dark leather

Palette change: teal/blue dominant → dark grey-purple trench dominant, dark crimson bandana accent, dirty cream bone accents, dark leather holsters with brass buckle highlights, faint fire orange rim highlight on the bandana edge.

Keep weapon-free rule: hands open at sides, palms empty, holsters empty.
```

### Negative Prompt
```
red hair, auburn hair, ginger hair, change face, change hair, change body shape, anime cute face, smooth gradient, anti-aliasing, weapons visible, gun in hand, pistol visible, hilt, drawn weapon, generic western cowboy, goggles, modern style, side view, side profile, isometric projection, 3d render, soft shading, painterly, text, words, letters, captions, numbers, watermark
```

### Beklenen Sonuç
- Aynı yüz / saç / vücut / pose / kamera açısı ✅
- Outfit canonical Gunslinger: grey-purple trench + rift bandana + bone/feather + tactical holsters ✅
- Silahsız body kuralı korunur (Karar #144)

---

## ANCHORS/characters/10_gunslinger.png

USER FIX 06_Gunslinger_Ana ile değiştirildi. Bu base'i PixelLab'a yükle → state prompt uygula → outfit canonical olur.

---

## Cross-link
- Canonical roster lock: `STAGING/CANONICAL_ANCHORS_LOCK.md`
- Karar #145 v2 Use #6: `memory/project_pixellab_character_states_workflow.md`
- Class canonical identity: `memory/project_character_visual_identity.md`
- Class gender LOCK (5M/5F): `memory/project_class_genders.md`
