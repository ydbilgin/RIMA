# GEMINI — Class Concept Düzeltme Üretimi

**Brawler ✅ PASS**
**Ranger ✅ PASS**
**Ronin ✅ PASS — Warblade standardıyla eşdeğer**

**Üretilecek:** Summoner, Gunslinger, Hexer (3 class)

---

**Her prompttan önce yapılacak:** Gemini'ye 3 referans görsel ekle:
- `warrior_idle_128.png`  ← AÇI KİLİDİ — tüm oyun bu açıya kilitli
- `elementalist_idle_128.png`
- `rima_style_anchor.png`

**Kamera standardı (kilitlendi — Warblade = referans):**
- ≈60-65° overhead steep ARPG camera
- Kafa tepesi baskın, omuzlar foreshortened
- Karakterin gözleri VAR — ama ileri bakıyor, kameraya değil. Kamera yukarıdan baktığı için gözler görünmüyor.
- Prompt ifadesi: "MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png"
- "80 degree straight down" YAZMA — çok dik, yanlış standart

**Çıktıyı kaydet:** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/`

---

## CLASS 1 — SUMMONER (Kadın) ❌ FAIL

**Kaydet:** `summoner_idle_128.png`

**Sorun:** Önceki üretimde ~40-50° cephe açısı. Yüz + gözler kameraya bakıyordu.

**Prompt:**
Draw a single pixel art CHARACTER SPRITE CONCEPT for the SUMMONER battlefield commander class of a Fractured Epic action RPG. CRITICAL CAMERA CORRECTION: THE PREVIOUS VERSION HAD THE WRONG ANGLE — THE CHARACTER WAS FACING THE CAMERA. THIS IS A FAILURE. YOU MUST FIX THIS. STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY — steep overhead ARPG camera, approximately 60-65 degrees downward. IMPORTANT: The character has normal eyes and a normal face — they just face FORWARD (not upward toward the camera). Because the camera is above and looking down steeply, we see the TOP of the head as the dominant shape. The character is NOT eyeless and NOT masked — the face simply points away from the camera. Do NOT draw eyes looking up at the viewer. Do NOT draw a face directed at the camera. The lower chin may barely peek out from beneath the head mass, but the eyes are hidden because they face forward, not upward. Single character, full body, plain LIGHT NEUTRAL background. CHARACTER: Female battlefield commander. Tall, upright posture. NOT a necromancer. Commander identity. HEAD: Top of her brown hair and a purple gemstone tiara — crown is dominant shape from above. OUTFIT: Worn muted purple robes with aged gold-bronze trim. NOT bright purple — muted, weathered, field-worn. Armored shoulder piece on left shoulder. Multiple robe layers. WEAPONS: Right hand raised holding a golden scepter vertically — tip pointing UPWARD above the head silhouette, clearly visible. Cold-blue glow at the crystal tip only. Left hand extended at waist height, open palm, commanding gesture. POSE (IDLE): Right arm raised with scepter. Left arm extended forward. Upright, authoritative. ENERGY: Cold-blue glow at scepter crystal tip ONLY. No circles, no summoning effects, no hand glow. PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.

**QC:**
- [ ] Kamera açısı: warrior_idle_128.png ile eşleşiyor — kafa tepesi baskın
- [ ] Gözler kameraya BAKMIYOR (ileri bakıyor, kameraya değil)
- [ ] Kadın silhouette net — commander, nekromancer değil
- [ ] Sağ elde dikey scepter, tip yukarı
- [ ] Sol elde açık avuç komuta jesti
- [ ] Cold-blue sadece scepter kristal ucunda

---

## CLASS 2 — GUNSLINGER (Kadın) ⚠️ PARTIAL

**Kaydet:** `gunslinger_idle_128.png`

**Sorun:** Önceki üretimde burun alanına kadar yüz görünüyordu — Warblade standardından fazla açıktı.

**Prompt:**
Draw a single pixel art CHARACTER SPRITE CONCEPT for the GUNSLINGER pistol duelist class of a Fractured Epic action RPG. CAMERA ANGLE CORRECTION: THE PREVIOUS VERSION SHOWED TOO MUCH FACE. STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY — steep overhead ARPG camera, approximately 60-65 degrees downward. IMPORTANT: The character has a normal face with normal eyes — they face FORWARD, not upward at the camera. The steep camera angle means we see the TOP of the head as dominant. The lower chin/jaw may barely be visible just like in warrior_idle_128.png. The face is not masked, not hidden by a hat — it simply points away from the camera because the character looks ahead, not up. Do NOT draw eyes directed at the viewer. Single character, full body, plain LIGHT NEUTRAL background. CHARACTER: Female pistol duelist. Lean athletic build. Slight forward-leaning duelist stance. HEAD: Top of her deep auburn red hair tied back — crown dominant from above. OUTFIT: Long asymmetric charcoal-dark-teal duelist coat — one side longer, aged edges. Aged copper trim on coat lapels only — NOT gold. Tactical harness beneath open coat. Dark trousers, boots. WEAPONS: Right hand gripping compact pistol at right hip — clearly a GUN, metal barrel visible. Left hand gripping compact pistol at left hip — clearly a GUN, metal barrel visible. BOTH pistols at hips, NOT raised. NOT daggers, NOT wands. POSE (IDLE): Relaxed but ready. Slight forward lean. ENERGY: NONE. No glowing hands, no magical effects. PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.

**QC:**
- [ ] Kamera açısı: warrior_idle_128.png ile eşleşiyor — kafa tepesi baskın
- [ ] Gözler kameraya BAKMIYOR
- [ ] Kadın silhouette net
- [ ] İki tabanca kalçada, metal barrel — dagger/wand değil
- [ ] Deep auburn red saç (kızıl)
- [ ] Charcoal-dark-teal coat, asimetrik, aged copper trim
- [ ] Ellerde glow/enerji YOK

---

## CLASS 3 — HEXER (Kadın) ❌ FAIL

**Kaydet:** `hexer_idle_128.png`

**Sorun:** Önceki üretimde ~30-40° tam cephe. Karakter direkt kameraya bakıyordu, tüm yüz görünüyordu.

**Prompt:**
Draw a single pixel art CHARACTER SPRITE CONCEPT for the HEXER curse mage class of a Fractured Epic action RPG. CRITICAL CAMERA CORRECTION: THE PREVIOUS VERSION WAS COMPLETELY WRONG — THE CHARACTER WAS FACING THE CAMERA DIRECTLY. THIS IS A TOTAL FAILURE. STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY — steep overhead ARPG camera, approximately 60-65 degrees downward. IMPORTANT: The character has a normal face with normal eyes — they face FORWARD, not upward toward the camera. The camera is above looking down steeply, so we see the TOP of her head as the dominant shape. The character is NOT a masked figure, NOT a ghost, NOT faceless — she is a real woman with a normal face that simply points away from the camera. Do NOT draw her looking up at the viewer. Do NOT draw glowing eyes facing the camera. The lower chin may barely be visible, but the eyes are hidden because the character looks ahead, not up. Single character, full body, plain LIGHT NEUTRAL background. CHARACTER: Female curse mage. Tall, thin figure. Pale skin. Deliberate movement quality. HEAD: Top crown of her dark hair — dominant from above. OUTFIT: Ankle-length dark crimson robes, tattered and fraying at hem. Worn leather belt at waist. Muted dark crimson, NOT bright red. WEAPONS (TWO SEPARATE ITEMS): ITEM 1: Right hand gripping a dark wooden staff, tip planted vertically on ground beside her right foot — staff standing tall. ITEM 2: Left hand holding an iron lantern by its handle at waist height — hanging downward. Inside the lantern: a cursed flame that is BOTH green AND purple simultaneously — mixed, not alternating. POSE (IDLE): Staff planted on ground in right hand. Lantern hanging in left hand. Weight slightly on staff side. GROUND EFFECT: Void decay tendrils — thin dark wisps at feet only, ground-level. ENERGY: Green-purple mixed flame INSIDE lantern only. No body aura, no robe glow. PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.

**QC:**
- [ ] Kamera açısı: warrior_idle_128.png ile eşleşiyor — kafa tepesi baskın
- [ ] Gözler kameraya BAKMIYOR (karakter ileri bakıyor, yukarı değil)
- [ ] Kadın silhouette net
- [ ] Sağ elde dikey tahta asa — yerde duruyor
- [ ] Sol elde demir fener — içinde HEM yeşil HEM mor alev (mixed)
- [ ] Ayak çevresinde void tendrils (ground level sadece)
- [ ] Vücut/robe'da glow YOK

---

## Tamamlanınca

Dosyaları şuraya kaydet: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/`

Dosya adları: `summoner_idle_128.png`, `gunslinger_idle_128.png`, `hexer_idle_128.png`

QC geçemezse aynı promptu tekrar gönder ve ekle:
`CORRECTION: The character is still facing the camera. They must look FORWARD — not upward. Match warrior_idle_128.png exactly. Redo with this correction only.`
