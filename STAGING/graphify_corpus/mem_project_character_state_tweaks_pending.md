---
name: character-state-tweaks-pending
description: "10 canonical anchor için Karar #145 v2 Use #6 state tweak listesi — user 'karakterleri üretecem' dediğinde bu listeyi göster"
metadata: 
  node_type: memory
  trigger_phrase: karakterleri üretecem
  trigger_action: show this list as PixelLab Web UI state production guide
  type: project
  originSessionId: 20cf7214-b515-4cce-814f-df3b0dd176f2
---

# Character State Tweaks — PENDING (yarın PixelLab production'da uygulanacak)

**Anchor source:** `ANCHORS/characters/` (Antigravity ordering, 10 LIVE)
**Karar:** #145 v2 Use #6 (Conditional Variant via Natural Language State Prompt)
**Gen budget:** 5000 yarın geliyor — ~10-15 gen kullanılacak (<%1)

---

## 🔴 MANDATORY 3 — Codex marked critical, anchor identity drift düzeltir

### 1. `08_elementalist.png` — Hair drift fix (Codex catch — Opus kaçırmıştı)
```
Keep the same female Elementalist outfit: dusty indigo crop, cream sash, deep teal skirt. Change hair to honey-blonde low bun. Not red, not auburn, not dark.
```

### 2. `10_summoner.png` — Summoning gesture (MANDATORY)
```
Keep the same female Summoner with long dark hair and indigo green-black robe. Raise one hand in a summoning gesture with faint cyan fingertip glow, palm facing downward conducting.
```

### 3. `05_shadowblade.png` — Void purple accent (MANDATORY)
```
Keep the same male assassin identity, silhouette, face, and outfit. Add clear void-purple glow only on shoulder edges, belt seam, and dagger-side accents. Keep armor near-black purple, not blue, not teal.
```

---

## 🟡 OPTIONAL 6 — Antigravity SPRITE_REVISION_DIRECTIVES + Opus prompts

### 4. `01_warblade.png` — Yaş ayarı **(YÖNÜ USER ONAYI BEKLİYOR)**

User mesajları çelişik:
- Önce: "yaşlı durmuş eski warbladeimiz gibi" → AGED
- Sonra: "warblade i daha genç yaptırırız mesela" → YOUNGER veya ÖRNEK
- **Karar bekliyor:** Aged / Younger / Olduğu gibi bırak

**Eğer AGED:**
```
Keep the same male Warblade armor, beard shape, and broad stance. Make the face a late-40s weathered veteran with light wrinkles, sunken tired eye sockets, grizzled beard with grey streaks at chin, weathered tan skin with one 1-pixel scar across right brow. Do not make him elderly or frail.
```

**Eğer YOUNGER:**
```
Keep the same male Warblade armor, beard, and broad stance. Make the face slightly younger, late 20s to early 30s, smooth skin, clean beard, sharp confident gaze. Keep the chibi big-head readable face.
```

**Ek opsiyonel (per SPRITE_REVISION_DIRECTIVES):**
```
Keep the same Warblade identity. Add a thick weathered wolf fur collar around the neck/shoulders, dark grey wolf fur with subtle weathering. Add deep battle scar marks across the armor plates, a few darker rust streaks.
```

### 5. `02_brawler.png` — Rift energy tattoo scars
```
Keep the same dark-skin male Brawler with bald head, leather hand wrappings, and orange knuckle glow. Add vibrant glowing orange rift energy tattoo scars across the bare chest and stomach — irregular cracked lines like cooling magma glow, faint warm halo around the tattoo lines.
```

### 6. `03_ravager.png` — Spike pauldrons + blood/rust gritty
```
Keep the same male Ravager body, dark messy shoulder-length hair, and dark blood-red armor. Make the shoulder pauldrons sharper and more aggressive — add a sharp spike or jagged metal edge protruding upward from each shoulder. Add rust stains and dried blood splatter marks on the chest armor and harness.
```

### 7. `04_ranger.png` — Asymmetric silhouette + survival pouches
```
Keep the same female Ranger with bleached-ivory hair and dark forest green armor. Make the silhouette clearly asymmetric: heavier right shoulder pauldron (the non-bow arm), left forearm with a leather wrap and arrow-fletching peeking out near the elbow. Add small survivalist leather pouches to the belt — three small cylindrical containers hanging on the right hip.
```

### 8. `07_ronin.png` — Topknot clearer + saya scabbard
```
Keep the same Asian male Ronin and dark navy kimono/hakama outfit. Tie the black hair into a visible samurai topknot at the back of the head with the knot clearly readable from the above angle. Add a visible empty katana scabbard (saya) at the left hip with dark wood finish and brass buckle, sword removed (weapon-free rule).
```

### 9. `09_hexer.png` — Bone/blood-stone macabre charms
```
Keep the same pale female hooded Hexer in dark purple-black robe with dark red hex-rune accents. Add a few hanging macabre charms from the lower robe hem — small dirty cream bone fragments and one dark red blood-stone crystal, tied with thin leather cord.
```

### 10. `06_gunslinger.png` — Outfit canonical (USER APPROVED BASE)
Saved separately: `STAGING/gunslinger_outfit_state_prompt.md`

Özet:
```
Keep the EXACT SAME brown-skin Gunslinger female: same chibi face, same short dark hair, same body proportions, same calm idle pose, same camera angle. ONLY change the outfit: replace the teal cape with a long dark grey-purple trench coat reaching mid-thigh, add a dark crimson rift bandana at the neck, add bone-and-feather accessories on collar and belt, add tactical dark leather pistol holsters on both upper thighs (EMPTY — no visible gun).
```

---

## Universal Negative Prompt (her state için)

```
change face, change hair color from canonical, change body shape, change pose, change camera angle, anime cute face, smooth gradient, anti-aliasing, weapons visible, gun in hand, sword visible, hilt, drawn weapon, side view, side profile, isometric projection, low angle, 3d render, soft shading, painterly, text, words, letters, captions, numbers, watermark
```

---

## PixelLab Workflow Reminder

| Adım | İş |
|---|---|
| 1 | PixelLab Web UI → "create character" — her anchor için (ANCHORS/characters/NN_classname.png upload) |
| 2 | Her character'a "create state" → state prompt yapıştır (yukarıdan ilgili) |
| 3 | Strength: 0.45-0.55 (orta — identity korumak için) |
| 4 | Çıktı doğrula → Pixelorama cleanup gerekirse <5 dk |
| 5 | Final state PNG → `ANCHORS/characters/state_variants/` altında save (yeni klasör) |

**Toplam süre tahmini:** 10 character × ~10-15 dk = **2-3 saat**

---

## Cross-link

- Anchor source: `ANCHORS/characters/`
- Karar #145 v2: `memory/project_pixellab_character_states_workflow.md`
- Canonical roster lock: `memory/project_canonical_character_roster_lock.md`
- User-approved Gunslinger base: `STAGING/gunslinger_outfit_state_prompt.md`
- Opus + Codex final verdict: `STAGING/OPUS_FINAL_VERDICT_antigravity_characters.md`
- Antigravity sprite directives source: `STAGING/SPRITE_REVISION_DIRECTIVES.md`
- Class identity canonical: `memory/project_character_visual_identity.md`
- Gender LOCK: `memory/project_class_genders.md`
