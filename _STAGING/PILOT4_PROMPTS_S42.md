# Pilot 4 Prompts (S42 · 64px Create-from-Style-Reference Pro · 8-Ref Hero Siege)

**Tarih:** 2026-04-26 · **Mode:** PixelLab "Create from Style Reference (Pro)" · **Pilot order:** Warblade → Ranger → Ronin → Ravager · **Cap:** UI max 8 ref

## UI Settings (her 4 run için aynı)

| Alan | Değer |
|---|---|
| Mode | Create from Style Reference (Pro) |
| Size | 64 px |
| Body type | bipedal-realistic |
| View | high top-down |
| n_directions | 8 |
| Variations | 16 (default 4×4) |
| Style description (optional) | boş |
| Background removal | OFF |

**Style image checkbox:** Outline ON · Shading ON · Color OFF · Detail OFF

## Style Reference 8-Set (sırayla yükle)

| Slot | Dosya | Sebep |
|---|---|---|
| 1 | `_STAGING/style_refs_s42/anchor_camera_60.png` | Anchor — kamera 60° lock kanıtlı |
| 2 | `_STAGING/hero_siege_wiki_sprites_64/Viking.png` | Heavy melee + fur + beard (Warblade+Ravager body) |
| 3 | `_STAGING/hero_siege_wiki_sprites_64/Shield_Lancer.png` | Plate + vertical weapon (Warblade silüet) |
| 4 | `_STAGING/hero_siege_wiki_sprites_64/Jotunn.png` | Heavy armored (Warblade alt, **boynuzlu değil**) |
| 5 | `_STAGING/hero_siege_wiki_sprites_64/Butcher.png` | Brutal cleaver (Ravager direkt) |
| 6 | `_STAGING/hero_siege_wiki_sprites_64/Samurai.png` | Katana + hakama (Ronin direkt match) |
| 7 | `_STAGING/hero_siege_wiki_sprites_64/Amazon.png` | Female athletic + light armor (Ranger body) |
| 8 | `_STAGING/hero_siege_wiki_sprites_64/Demon_Slayer.png` | Lean dark dual-blade (Ronin lean alt) |

**Pool dışı (curate edildi, drop):** Marksman (low-angle drift) · Paladin (3. horned helm averaging riski) · Pyromancer/Necromancer/White_Mage/Stormweaver (mage pilot bekle) · Pirate/Marauder/Demonspawn/Exo/Illusionist/Shaman/Redneck/Nomad (alt pool).

**Experiment notu:** 8-ref averaging riski var (önceki kararda 1-3 ref önerilmişti). Refs cleaned 64px (`hero_siege_wiki_sprites_64/`, transparent BG, contact sheet kontrol edildi). Bilinen leak riskleri: 2× horned helm (Viking+Butcher → Warblade/Ravager horned cliché), Demon_Slayer kırmızı robe (Ronin'de kırmızı sızma), Amazon yeşil saç (Ranger'da yeşil ton — düşük). PASS rate < 30% → fallback 3-ref set: anchor + Viking + Samurai. PASS rate < 60% → 5-ref set: anchor + Viking + Samurai + Amazon + Butcher.

## QC her variation için (5 kapı)

1. Kamera 60° (head-top, eyes high&small, foreshortened torso)
2. Anchor isolation (hooded/lantern/shield/plague mask leak yok + Hero Siege exact-costume copy yok)
3. Identity match (silah/silüet/palette per-class spec)
4. Palette disiplin (yasaklı renkler yok)
5. NO TEXT (tek harf bile = RED)

PASS eşiği: 6+/16 havuz hazır · 3-5 sınır (prompt tweak) · 0-2 fallback ref-set'e geç.

---

## 01 Warblade — UI Description (kopyala-yapıştır, tek blok)

```
ABSOLUTELY NO TEXT ANYWHERE. No class names, no labels, no captions, no letters, no words, no numbers, no UI text, no health bars, no banners, no tags, no watermarks, no frames, no grid lines. The sprite must contain only the character on a fully transparent background, no typography of any kind.

Camera angle: ~60 degrees from horizontal (~30 degrees from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera, locked. South-facing front pose for the primary frame; character faces the player/camera. Camera is above the character. Top of head and hair clearly visible. Forehead and hair-top read more than the lower face. Eyes small and high on the face, NOT centered like a portrait. Nose and mouth tiny and simplified. Chin compressed. Upper shoulder planes visible. Torso slightly compressed by foreshortening. Feet lower in the frame and slightly smaller than head/shoulders. Avoid: southeast diagonal 3/4 view, eye-level character-select pose, flat front paper-doll, pure side view, pure 90-degree bird's-eye.

8-direction sprite set: south faces viewer, north shows back with greatsword crossing the back clearly visible, east and west are true side profiles, four diagonals clearly distinct from cardinals. Same character identity, same outfit, same weapon, same grip, same palette in every direction.

IMPORTANT: This character is a MAN. Male. Adult, mid-30s, mature heroic proportions, NOT chibi, battle-worn weathered veteran.

Identity: adult male battle-worn melee warrior, broad-shouldered grounded stance. Dark steel plate armor over warm brown weathered leather, light scuffs and battle wear, no shine, no mirror polish. Greatsword carried on the RIGHT shoulder, hilt up at the right shoulder, blade angled diagonally across the back from right shoulder to left hip — clearly a two-handed greatsword silhouette in every direction. Both hands free at the sides OR right hand resting on the hilt — same hand assignment in every direction. Heavy plated boots, leather gauntlets. Short cropped dark hair, light beard or stubble.

Palette: dark steel grey, warm desaturated brown leather, ember orange accent on belt buckle or pommel only, dull silver edge highlights on plate. Muted, weathered, grounded.

Strictly avoid: blue, purple, green, magic glow, runes, gems, long flowing cape, hooded face, shining knight polish, samurai elements, plague doctor mask, dual weapons, single one-handed sword, fur mantle, no greatsword on the back. NOT a paladin, NOT a samurai, NOT a hooded ranger.

Use the attached style references ONLY for: camera angle (~60 degrees from horizontal high top-down ARPG), outline weight, shading style, pixel density, and color saturation level. Do NOT copy from the references: any character design, any mask, any hood shape, any costume, any specific weapon shape, any shield, any lantern, any prop, any specific color palette, any identity element. The reference figures (plague-doctor, viking, shield-lancer, jotunn, butcher, samurai, amazon, demon-slayer) are NOT in this game. Generate a completely original character matching the identity description above; the references are STYLE anchors only.
```

---

## 02 Ranger — UI Description (kopyala-yapıştır, tek blok)

```
ABSOLUTELY NO TEXT ANYWHERE. No class names, no labels, no captions, no letters, no words, no numbers, no UI text, no health bars, no banners, no tags, no watermarks, no frames, no grid lines. The sprite must contain only the character on a fully transparent background, no typography of any kind.

Camera angle: ~60 degrees from horizontal (~30 degrees from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera, locked. South-facing front pose for the primary frame; character faces the player/camera. Camera is above the character. Top of head and hair clearly visible. Forehead and hair-top read more than the lower face. Eyes small and high on the face, NOT centered like a portrait. Nose and mouth tiny and simplified. Chin compressed. Upper shoulder planes visible. Torso slightly compressed by foreshortening. Feet lower in the frame and slightly smaller than head/shoulders. Avoid: southeast diagonal 3/4 view, eye-level character-select pose, flat front paper-doll, pure side view, pure 90-degree bird's-eye.

8-direction sprite set: south faces viewer, north shows back with quiver clearly visible, east and west are true side profiles, four diagonals clearly distinct from cardinals. Same character identity, same outfit, same weapon, same grip, same palette in every direction.

IMPORTANT: This character is a WOMAN. Female. Adult, mid-20s, mature heroic proportions, NOT chibi, NOT cute archer girl. Every direction must show a female character.

Identity: adult female rift stalker, athletic lean build, alert upright stance. Half-shaved hairstyle with a long braid falling on the unshaved side, dark hair. War paint stripes painted horizontally across the eyes. Head fully visible — NO hood over the face, NO cap, NO helmet. Sharp focused eyes, serious dangerous expression. Bone-recurve longbow held in the LEFT hand at the grip, RIGHT hand resting near the string in a draw-ready pose — same grip and same hand assignment in every direction. Bow always held away from the body and fully visible in silhouette, never hidden behind the torso. Side-mounted arrow quiver on the RIGHT hip with fletching visible from south, full quiver clearly visible from north. Light leather armor over a fitted dark undershirt, exposed forearms, leather pauldrons, belt with small pouches, fitted dark pants and tightly laced mid-calf boots.

Palette: bone white and weathered off-white cloth, dark weathered brown leather, charcoal accents, cold rift-purple glow only on the bowstring and arrow fletching tips. Muted, desaturated, weathered.

Strictly avoid: chibi, elf ears, male character, masculine face, face hood, Robin Hood cap, green forest tones, bright saturated colors, magic robes, cape, white background, bow hidden behind body, mismatched grip across directions.

Use the attached style references ONLY for: camera angle (~60 degrees from horizontal high top-down ARPG), outline weight, shading style, pixel density, and color saturation level. Do NOT copy from the references: any character design, any mask, any hood shape, any costume, any specific weapon shape, any shield, any lantern, any prop, any specific color palette, any identity element. The reference figures (plague-doctor, viking, shield-lancer, jotunn, butcher, samurai, amazon, demon-slayer) are NOT in this game. Generate a completely original character matching the identity description above; the references are STYLE anchors only.
```

---

## 03 Ronin — UI Description (kopyala-yapıştır, tek blok)

```
ABSOLUTELY NO TEXT ANYWHERE. No class names, no labels, no captions, no letters, no words, no numbers, no UI text, no health bars, no banners, no tags, no watermarks, no frames, no grid lines. The sprite must contain only the character on a fully transparent background, no typography of any kind.

Camera angle: ~60 degrees from horizontal (~30 degrees from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera, locked. South-facing front pose for the primary frame; character faces the player/camera. Camera is above the character. Top of head and hair clearly visible. Forehead and hair-top read more than the lower face. Eyes small and high on the face, NOT centered like a portrait. Nose and mouth tiny and simplified. Chin compressed. Upper shoulder planes visible. Torso slightly compressed by foreshortening. Feet lower in the frame and slightly smaller than head/shoulders. Avoid: southeast diagonal 3/4 view, eye-level character-select pose, flat front paper-doll, pure side view, pure 90-degree bird's-eye.

8-direction sprite set: south faces viewer, north shows back with sashed waist and trailing robe edges visible, east and west are true side profiles with the katana sheath silhouette clearly readable on the left hip, four diagonals clearly distinct from cardinals. Same character identity, same outfit, same weapon, same grip, same palette in every direction.

IMPORTANT: This character is a MAN. Male. Adult, late 20s to early 30s, lean disciplined build, mature proportions, NOT chibi.

Identity: adult male iaido swordsman, lean disciplined upright posture, calm focused expression. Single katana sheathed at the LEFT hip in a dark scabbard with a wrapped handle, blade NOT drawn. LEFT hand resting on the scabbard near the guard, RIGHT hand draw-ready hovering near the hilt — same hand assignment in every direction. Loose layered outer robe over a fitted under-armor shirt, sash tied at the waist, dark hakama-like wide trousers, traditional split-toe footwear or simple wraps. Short dark hair, no topknot, headband acceptable but optional, face fully visible no mask.

Palette: muted indigo and black for robe and trousers, off-white sash, dull silver blade-guard accent, weathered brown scabbard. No bright saturated colors. Disciplined and weathered.

Strictly avoid: bright red, floral motifs, cherry blossoms, oni mask, demon mask, topknot cliche, full samurai shogun armor, generic anime samurai, dual katanas, no katana, drawn naked blade, fur mantle, plate armor, hood, magic glow.

Use the attached style references ONLY for: camera angle (~60 degrees from horizontal high top-down ARPG), outline weight, shading style, pixel density, and color saturation level. Do NOT copy from the references: any character design, any mask, any hood shape, any costume, any specific weapon shape, any shield, any lantern, any prop, any specific color palette, any identity element. The reference figures (plague-doctor, viking, shield-lancer, jotunn, butcher, samurai, amazon, demon-slayer) are NOT in this game — including the Hero Siege Samurai reference, do NOT copy its costume, color, or weapon shape, only borrow camera and outline. Generate a completely original character matching the identity description above; the references are STYLE anchors only.
```

---

## 04 Ravager — UI Description (kopyala-yapıştır, tek blok)

```
ABSOLUTELY NO TEXT ANYWHERE. No class names, no labels, no captions, no letters, no words, no numbers, no UI text, no health bars, no banners, no tags, no watermarks, no frames, no grid lines. The sprite must contain only the character on a fully transparent background, no typography of any kind.

Camera angle: ~60 degrees from horizontal (~30 degrees from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera, locked. South-facing front pose for the primary frame; character faces the player/camera. Camera is above the character. Top of head and hair clearly visible. Forehead and hair-top read more than the lower face. Eyes small and high on the face, NOT centered like a portrait. Nose and mouth tiny and simplified. Chin compressed. Upper shoulder planes visible. Torso slightly compressed by foreshortening. Feet lower in the frame and slightly smaller than head/shoulders. Avoid: southeast diagonal 3/4 view, eye-level character-select pose, flat front paper-doll, pure side view, pure 90-degree bird's-eye.

8-direction sprite set: south faces viewer with both axes clearly visible one in each hand, north shows back with the fur mantle fully visible and both axe heads peeking from the sides of the silhouette, east and west are true side profiles with one axe in front and one behind, four diagonals clearly distinct from cardinals. Same character identity, same outfit, same TWO weapons, same hand assignment, same palette in every direction.

IMPORTANT: This character is a MAN. Male. Adult, mid-30s, broad-shouldered brutal berserker, mature proportions, thick beard, NOT chibi.

Identity: adult male brutal berserker, broad shoulders, thick dark beard, intimidating heavy stance. TWO one-handed axes — ONE axe in the LEFT hand and ONE axe in the RIGHT hand, both axes clearly visible in every direction, NEVER a single two-handed axe and NEVER bare fists and NEVER a sword. Heavy fur mantle draped over both shoulders, bare or fabric-wrapped torso under the mantle showing battle scars, leather kilt or skirt at the waist, heavy leather and fur boots, leather wrist wraps. No helmet, hair pulled back roughly or wild, beard prominent.

Palette: dirty bronze metal on axe heads and belt, dark brown and grey fur on mantle and boots, crimson cloth wraps at the waist or wrists, dull iron axe edges, weathered tan skin. Muted, dirty, weathered. NO clean polish.

Strictly avoid: blue, purple, green, magic glow, runes, gems, single two-handed axe, single one-handed axe with shield, sword, greatsword, gauntlets only with no weapons, full plate armor, hood, cape, helmet, polished steel. NOT a Brawler (no boxing guard, no tattoo glow, no gauntlets-only), NOT a Warblade (no greatsword on the back, no plate armor), NOT a viking caricature (no horned helm).

Use the attached style references ONLY for: camera angle (~60 degrees from horizontal high top-down ARPG), outline weight, shading style, pixel density, and color saturation level. Do NOT copy from the references: any character design, any mask, any hood shape, any costume, any specific weapon shape, any shield, any lantern, any prop, any specific color palette, any identity element. The reference figures (plague-doctor, viking, shield-lancer, jotunn, butcher, samurai, amazon, demon-slayer) are NOT in this game — including the Hero Siege Viking and Butcher references, do NOT copy their helmets, costumes, or weapon shapes, only borrow camera and outline. Generate a completely original character matching the identity description above; the references are STYLE anchors only.
```

---

## Workflow

1. PixelLab UI → Create from Style Reference (Pro)
2. 8 ref slot'a yukarıdaki sırada upload (anchor #1)
3. Settings (yukarıdaki tablo)
4. Description = ilgili class block kopyala-yapıştır
5. Run → 16 variation döner
6. Sheet (4×4) → Claude'a at → 5 kapı QC
7. PASS variation → kullanıcı `character_id` verir
8. 4 class PASS sonrası → Claude MCP `animate_character` ile id üzerinden Run/Attack/Idle/Hit/Death

**Pilot 4 tamamlanınca:** PASS rate raporu → ya 8-ref devam (kalan 12 class) ya fallback ref-set'e geç.
