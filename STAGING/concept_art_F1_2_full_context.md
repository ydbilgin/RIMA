# Codex Task — Concept Art F1.2 Full-Context Retry

**Status:** READY FOR DISPATCH
**Branch:** master
**Reason for retry:** F1.1 (commit b0a25e90, 5 sahne 1672×941) bağlam yetersizdi → karakterler RIMA anchor karakterlerine uymadı (generic warrior/ranger çıktı), çözünürlük küçük. Bu retry TAM bağlam + büyük boyut + anchor uyumu sağlayacak.

**Allowed paths:**
- `STAGING/concept_art/01_combat/**` (yeni `codex_v2_` prefix)
- `STAGING/concept_art/_DISCARDED_codex_v1_*/` (eski 5 sahne yedeği)
- `STAGING/concept_art/INDEX.md` (commit edilecek tek dosya)

## Adım 0 — BACKUP (ZORUNLU, herhangi bir gen'den ÖNCE)

Mevcut 5 dosyayı koru:
```
STAGING/concept_art/01_combat/codex_scene_01_warblade_iron_combo.png
STAGING/concept_art/01_combat/codex_scene_02_ranger_volley.png
STAGING/concept_art/01_combat/codex_scene_03_shadowblade_veil_strike.png
STAGING/concept_art/01_combat/codex_scene_04_elementalist_cast.png
STAGING/concept_art/01_combat/codex_scene_05_keep_arena_overview.png
```

→ taşı:
```
STAGING/concept_art/_DISCARDED_codex_v1_<UTC_timestamp>/codex_scene_01..05_*.png
```

## Adım 1 — REQUIRED READS (image gen ÖNCESİ tüm bunları oku)

**Karakter anchor referansları (canonical character source of truth):**
- `Characters/anchors/warblade/metadata.json`
- `Characters/anchors/warblade/warblade_anchor.png` (görsel referans — vision input olarak imagegen'e geçir eğer destekliyorsa)
- `Characters/anchors/warblade/rotations/south.png`
- `Characters/anchors/ranger/metadata.json`
- `Characters/anchors/ranger/ranger_anchor.png`
- `Characters/anchors/ranger/rotations/south.png`
- `Characters/anchors/shadowblade/metadata.json`
- `Characters/anchors/shadowblade/shadowblade_anchor.png`
- `Characters/anchors/shadowblade/rotations/south.png`
- `Characters/anchors/elementalist/metadata.json`
- `Characters/anchors/elementalist/elementalist_anchor.png`
- `Characters/anchors/elementalist/rotations/south.png`

**TASARIM/ kapsamlı (oku, internalize et):**

*Karakter + sınıf identity:*
- `TASARIM/STYLE_BIBLE.md` — RIMA pixel art stil reçetesi
- `TASARIM/ANIMATION_BIBLE.md` — kamera açısı 30-35° low top-down LOCKED + 7 anim seti
- `TASARIM/MASTER_KARAR_BELGESI.md` — locked design decisions
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` — sınıf identity matrisi
- `TASARIM/CLASS_STATE_CONTRACT.md` — sınıf state machine
- `TASARIM/CLASS_RMB_REDESIGN_2026-05-06.md` — RMB skill identity
- `TASARIM/BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md` — LMB/RMB sözleşmesi
- `TASARIM/SUMMONER_HEXER_CLASS_DESIGN.md` — caster sınıf detay (eğer scene'de varsa)
- `TASARIM/SUMMONER_ECONOMY_RULES.md`

*Skill / VFX (her sahnenin attack visualı için ZORUNLU):*
- `TASARIM/SKILL_SYSTEM_v2.md` — skill sistemi
- `TASARIM/SKILL_VISUAL_CONTRACT.md` — VFX visual contract (palette, particle mantığı)
- `TASARIM/SKILL_POOLS_10CLASS_2026-05-07.md` — sınıf bazlı skill listesi
- `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md` — skill taxonomy
- `TASARIM/SKILL_AUDIT_DECISION_2026-04-30.md` — skill audit kararları
- `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md` — Hades-style draft
- `TASARIM/SHADOW_ECHO_MATRIX.md` — Shadow Echo cross-class system (Scene 3 KRITIK)
- `TASARIM/SHADOWBLADE_ECHO_SYSTEM.md`
- `TASARIM/CROSS_CLASS_PROC_SYSTEM.md` — cross-class proc'lar
- `TASARIM/MAKEUP_VFX_CONTRACT.md` — VFX kuralları
- `TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` — kamera + boss scale matematiği

*Mob + Boss design (sahnelerde yer alan düşmanlar için):*
- `TASARIM/MOB_COMPOSITION_RULES.md` — mob hierarchy + scale
- `TASARIM/COMBAT_ROSTER.md` — Act 1 mob roster (skeleton, ghoul, vs detayları)
- `TASARIM/BOSS_DESIGN.md` — boss visual identity
- `TASARIM/DAMAGE_CALCULATION.md` — combat math (sahne tension için)

*Room + Environment design (Scene 5 KRITIK + tüm sahnelerin background'ı):*
- `TASARIM/ROOM_DESIGN_PHILOSOPHY.md` — Hades-tarzı closed arena, mood
- `TASARIM/ROOM_MECHANICS.md` — room mekaniği
- `TASARIM/ROOM_INSPIRATION_PATTERNS_AND_RIMA_ADAPTATION_2026-05-03.md`
- `TASARIM/ROOM_CONNECTED_GENERATION_AND_ACT_EVOLUTION_PROPOSAL_2026-05-03.md`
- `TASARIM/ROOM_STAGING_AND_MAP_VARIANTS_DECISION_2026-05-03.md`
- `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md` — Act 1 keep biome detayı (Scene 5 source of truth)
- `TASARIM/ACT1_SHATTERED_KEEP_ROOM_BLOCKOUT_SET_2026-05-04.md` — blockout set
- `TASARIM/dungeon_act1_map.md` — Act 1 map topoloji
- `TASARIM/MAP_ITEM_SYSTEM.md`
- `TASARIM/map_fragment_system.md`
- `TASARIM/room_authoring.md`
- `TASARIM/GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md` — gate socket görsel detay (Scene 5)
- `TASARIM/RIFT_PORTAL_OPPORTUNITY.md`

*Quality + visual standards:*
- `TASARIM/VISUAL_QUALITY_STANDARDS.md` — pixel cluster, palette, kalite gate

**MEMORY/ (visual + scale):**
- `MEMORY/project_rima_visual_identity.md` — anchor stil bible özet
- `MEMORY/project_camera_scale_zoom_rules.md` — 35° ARPG kamera + tile-karakter oran
- `MEMORY/project_mob_boss_sizes.md` — boyut hiyerarşisi (252px boss + PPU 32 vs)
- `MEMORY/feedback_concept_art_pixel_art_only.md` — pixel art ZORUNLU, painterly YASAK
- `MEMORY/project_pixellab_no_concept_art.md` — PixelLab MCP kullanma; imagegen tool kullan
- `MEMORY/project_pixellab_pipeline.md` — palette, view açısı, tutarlılık
- `MEMORY/project_animation_bible.md` — anim seti
- `MEMORY/project_v1_anim_4classes.md` — 4 sınıf önceliği

**GUIDES/:**
- `GUIDES/RIMA_MASTER_ART_PIPELINE.md` — master pipeline LOCKED
- `GUIDES/RIMA_CAMERA_AND_WALL_OCCLUSION_SYSTEM_2026-05-03.md`
- `GUIDES/RIMA_ISOMETRIC_ENVIRONMENT_PRODUCTION_FEEDBACK_2026-05-03.md`

**Toplam ~50 dosya. HEPSI okunmadan generate ETME.** Read sırasında çıkar:
1. **Karakter:** body description + palette + weapon + accent + posture + face/hood (her sınıf metadata.json + anchor PNG)
2. **Skill / VFX:** her sahnedeki attack'ın spesifik VFX detayı (örn Warblade LMB greatsword horizontal slash trail rengi, Ranger LMB bow fire/charge, Shadowblade RMB veil-strike phase fade, Elementalist LMB cast burst)
3. **Mob:** sahneye giren düşmanların ROSTER detayı (skeleton armor, ghoul cloak rags, elite tier scale)
4. **Room:** Act 1 Shattered Keep biome — wall stones, floor tiles, gate sockets, torches, altar, runic accents
5. **Camera:** 35° low top-down ARPG — bird's eye / pure iso / side-scroll YASAK

## Adım 2 — Görsel Spec (her sahne için)

| Parametre | Değer |
|---|---|
| Resolution | **TARGET: 1920×1080**. Imagegen tool fixed size kullanıyorsa, 16:9 oranındaki **EN BÜYÜK** boyutu seç. 1672×941 minimum kabul, daha büyük bul. Tool size param varsa explicit "1920x1080" veya "2048x1152" iste. Önceki batch 1672×941 idi — bunu aşmaya çalış. |
| Aspect | 16:9 (1.778) |
| Style | Pixel art, chunky cluster min 4px, hard edges, no anti-aliasing, BUT cinematic wide screenshot composition (depth, mood lighting, atmospheric perspective) |
| Camera | **35° low top-down ARPG** (Diablo 2 / PoE / Hades) — `TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` ve `MEMORY/project_camera_scale_zoom_rules.md`'a SADIK |
| Player figure | ~10% screen height (varsa). Mob scale `MOB_COMPOSITION_RULES`'a göre |
| Palette | Anchor palette: cold blue accent #7BA7BC, dark slate base #1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575. Warm rim light optional per scene mood. |

## Adım 3 — Per-Scene Prompts (anchor-aware, character-aware)

Her sahnenin prompt'unda ilgili sınıfın **metadata.json'dan çekilmiş açıklama + palette + weapon detayı** olmalı. Kuru "warrior" yerine "Warblade — heavy plate armor with cold blue cloth accent #7BA7BC at sash and shoulder straps, broad shoulders, two-handed greatsword" vs. "Ranger — hooded cloak (forest green #3A4A38), cold blue undertunic #7BA7BC, compound bow in left hand, light leather armor".

**Output filenames (exact, prefix `codex_v2_`):**
1. `STAGING/concept_art/01_combat/codex_v2_scene_01_warblade_iron_combo.png`
2. `STAGING/concept_art/01_combat/codex_v2_scene_02_ranger_volley.png`
3. `STAGING/concept_art/01_combat/codex_v2_scene_03_shadowblade_veil_strike.png`
4. `STAGING/concept_art/01_combat/codex_v2_scene_04_elementalist_cast.png`
5. `STAGING/concept_art/01_combat/codex_v2_scene_05_keep_arena_overview.png`

**Prompt template (her sahne kendi konusu + her sahnede aynı tail):**

> [SCENE-SPECIFIC SUBJECT — pulled from metadata.json + design docs]. Pixel art rendered as a cinematic wide 16:9 screenshot, 35-degree low top-down ARPG camera (Diablo 2 / Path of Exile / Hades angle, NOT bird's eye, NOT pure isometric, NOT side-scroll). Hard pixel edges, chunky pixel clusters minimum 4px wide, no anti-aliasing, no smooth gradients. Composed with depth, atmospheric perspective, mood lighting, environmental detail. Palette anchor: dark slate base (#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575) with cold blue character accent (#7BA7BC) and warm rim light from torches/braziers contrasting the cold metal. Style reference: Hyper Light Drifter, Eastward, Blasphemous wide cinematic shots — pixel art content rendered at high resolution with cinematic framing.
> Negative: blur, 3d render, smooth gradient, ambient occlusion, anti-aliasing, photo-realistic, soft shading, anime, deformed, flat 2d cartoon, painterly, illustration, oil painting, pastel, watercolor, generic warrior, generic mage, default armor.

**Per-scene subjects (anchor-aware + skill-aware + mob-aware + room-aware, FILL with details from REQUIRED READS):**

**Scene 1 — codex_v2_scene_01_warblade_iron_combo.png:**
> **Karakter:** Warblade warrior matching `Characters/anchors/warblade/metadata.json` — heavy plate steel armor (#4A4E5A / #5C6070 / #6E7280), cold blue cloth accent (#7BA7BC) at sash and shoulder straps, broad shoulders, stoic stance, hair dark brown.
> **Skill (LMB ile başlayan combo, her bir vuruşu okunabilir):** Mid-combo greatsword horizontal slash — two-handed grip, blade tip past character silhouette right edge. **VFX:** cold blue weapon trail (#7BA7BC), per `SKILL_VISUAL_CONTRACT.md` Warblade LMB convention. Body twisted 30° to follow slash, weight on back foot.
> **Mob (per `COMBAT_ROSTER.md` + `MOB_COMPOSITION_RULES.md`):** Standard tier melee skeleton (rusted plate fragments, exposed bone joints, dim red eye sockets) recoiling from impact, sparks + bone dust kicked up, posture broken backward.
> **Room (per `ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md` + `ROOM_DESIGN_PHILOSOPHY.md`):** Stone keep dungeon corridor combat room, mossy weathered stone walls (#2A2D34 / #1A1C20) with Norse-inspired runic carvings, cracked floor tiles (#3A3D48 / #4E5260), iron sconces with torches casting warm orange rim light contrasting the cold blue weapon trail. Atmospheric haze for depth.
> **Camera:** 35° low top-down ARPG (Hades). Player ~10% screen height. Skeleton ~9% (slightly behind player plane).

**Scene 2 — codex_v2_scene_02_ranger_volley.png:**
> **Karakter:** Ranger matching `Characters/anchors/ranger/metadata.json` — hooded cloak forest green (#3A4A38 / #4E5E48), cold blue undertunic (#7BA7BC), light leather armor (#3A2818 / #5A4028), lean agile build, hood up with partial face shadow, quiver visible on back with leather strap, skin (#C9A084).
> **Skill (LMB ShotCadence per `BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md` + `SKILL_VISUAL_CONTRACT.md`):** Compound bow at full draw, left arm extended forward holding bow vertical, right hand at cheek anchor, arrow released — multiple arrows mid-flight in the foreground showing rapid volley. **VFX:** cold blue (#7BA7BC) glints on bowstring + arrow tips, faint motion blur on flight path (kept pixel-art chunky).
> **Mob (per `COMBAT_ROSTER.md` Act 1):** Three crypt ghouls advancing — tattered grey-green burial shrouds (#3A4A38 desaturated), exposed greying flesh, hunched posture with elongated arms, one ghoul mid-leap, one walking, one rising from prone. Cold blue eyes (matching player accent for unified Act 1 palette).
> **Room (per `ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE` outdoor courtyard variant):** Overgrown stone courtyard with mossy ruins, broken pillars, cracked flagstones, grass tufts pushing through, low fog at ground level. Warm rim light from a setting sun behind ranger (left side), cold ambient cast on ghouls (right side) creating tension contrast.
> **Camera:** 35° low top-down ARPG. Ranger ~10% screen height. Mid-ground ghouls ~8%, far ghoul ~6% (depth via scale).

**Scene 3 — codex_v2_scene_03_shadowblade_veil_strike.png:**
> **Karakter:** Shadowblade assassin matching `Characters/anchors/shadowblade/metadata.json` — full dark hooded cloak silhouetted, palette: cloak black-purple (#1A0E2A / #2A1A3A / #3A2A4E), accent void violet (#5A2A8A), only chin and jawline visible below deep hood, leather straps (#3A2818), slim agile build, low crouched ready posture.
> **Skill (RMB VeilStrike per `SKILL_VISUAL_CONTRACT.md` + `SHADOWBLADE_ECHO_SYSTEM.md` + `SHADOW_ECHO_MATRIX.md`):** Phase-strike — TWO visible moments fused into one frame: (1) original position shows fading violet silhouette with smoke wisps trailing right, (2) strike position has fully re-materialized character with twin short blades crossing in an X-shape on an elite mob. Violet streak (#5A2A8A) connects the two positions. Per Shadow Echo system — keep VFX cyan #00FFCC accent visible as Echo aura on phantom trail (cross-class proc layer).
> **Mob:** Elite tier mob — armored ghoul or undead knight (rusted partial plate, larger silhouette ~12% screen height = elite +20% scale per `MOB_COMPOSITION_RULES.md`), recoiling from blade strike with green-grey ichor splatter pixels.
> **Room (per `ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE` crypt variant):** Dark crypt arena interior, violet ambient haze pooling at floor, cracked stone tile floor with carved sigils glowing faint cyan (#00FFCC Echo undertone), distant low-burning torches with cold blue pools (not warm — crypt is cooler than keep), fog at edges, broken sarcophagi visible in background.
> **Camera:** 35° low top-down. Player phantom ~10% scaled to 70% alpha at original spot, full opacity at strike position. Elite mob ~12%.

**Scene 4 — codex_v2_scene_04_elementalist_cast.png:**
> **Karakter:** Elementalist mage matching `Characters/anchors/elementalist/metadata.json` — long flowing robe deep blue-grey (#2A3848 / #3E4C5E / #525E74) cool neutral default, faint cool trim (#B8C8D0), sash leather (#3A2818), hood NOT up (face visible — confident mage), short hair, hands free for spell gestures, skin (#C9A084).
> **Skill (LMB CastRhythm per `BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT` + `SKILL_VISUAL_CONTRACT`):** Both hands extended forward, palms outward, fingers spread — peak cast moment. Energy gathers at palms as a placeholder VFX zone (~10% character height each). **CRITICAL:** Sprite element-agnostic — DO NOT embed Fire/Frost/Lightning color on character; engine adds runtime VFX overlay. Show the burst zone as a soft cool-white glow (#B8C8D0) without committing to an element. Robe billows backward from cast force.
> **Mob:** Volcanic biome enemy emerging from magma fissure — molten golem or fire elemental (Act 1 Shattered Keep variant doesn't have volcanic enemies; this is Act 2 preview OR re-frame to Keep guard with skeleton-type). Reframe: **use a brutish skeleton warrior** with greatsword raised, bone armor charred from prior battles, contrasting cold mage with warm bone-grey (#A09080).
> **Room:** Act 1 Shattered Keep boss antechamber — wider arena framing, broken stained glass behind mage casting cold ambient light, distant gate sockets visible left/right, cracked floor with sigil ring at center where fight occurs.
> **Camera:** 35° low top-down. Mage ~12% (slightly forward casting pose). Mob ~10%.

**Scene 5 — codex_v2_scene_05_keep_arena_overview.png:**
> **No central character** — environmental establishing shot of Act 1 Shattered Keep combat arena, per `ACT1_SHATTERED_KEEP_ROOM_BLUEPRINT_CATALOGUE_2026-05-03.md` + `ACT1_SHATTERED_KEEP_ROOM_BLOCKOUT_SET_2026-05-04.md` (combat room template, Hades-style closed arena).
> **Room blueprint compliance (per `GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md`):** Four gate sockets visible at cardinal walls (N/E/S/W), gate visuals = stone arches with iron portcullis, faint cold blue glow at gate edges (#7BA7BC) marking active connections.
> **Focal feature:** Central ritual altar — square stone block ~2 unit height, cold blue glow (#7BA7BC) emanating from carved sigil on top, faint particle wisps. Or alternative: shattered fountain with cold blue luminous water remnants.
> **Lighting (per `ROOM_DESIGN_PHILOSOPHY.md`):** Scattered iron-sconce torches in alcoves casting warm orange pools of light (#FFB060 mid + #803010 deep) contrasting central cold blue. Atmospheric haze for depth, dust motes catching torchlight beams.
> **Material palette:** Stone floor tiles cool slate (#3A3D48 / #4E5260) showing wear, cracks, ritual sigil engravings. Wall stones (#2A2D34 / #1A1C20) carved with Norse-inspired runic accents (matching `STYLE_BIBLE.md` Norse-influence direction). Hints of plant overgrowth in corners (low priority).
> **Atmospheric depth:** Distant gate openings show silhouettes of corridors beyond (recede into darkness with cool blue ambient). Empty pre-combat tension — no characters, no mobs, just the room awaiting the player.
> **Camera:** 35° low top-down — wider than character scenes (this is establishing shot). Room fills frame, altar slightly off-center golden ratio composition.

**Anchor PNG vision input (kritik):** Eğer imagegen tool görsel input destekliyorsa (image-to-image, vision conditioning, reference image) `Characters/anchors/<class>/<class>_anchor.png` dosyasını ilgili sahnenin generation çağrısında reference olarak geçir. Tool desteklemiyorsa metadata.json'daki textual description'ı çok detaylı yaz (renkler, silüet, postür, palette).

## Adım 4 — STOP at Scene 5 Quality Gate

5 sahne tamamlandıktan sonra:
1. `STAGING/concept_art/INDEX.md` güncelle:
   - Eski 5 codex_scene_*.png satırlarını "DISCARDED v1, see _DISCARDED_codex_v1_<timestamp>/" olarak işaretle
   - Yeni 5 codex_v2_scene_*.png satırı ekle (resolution actually achieved, status: GENERATED, audit: pending user)
2. Commit INDEX.md only (PNGs gitignored)
3. **DUR — sahne 6-30 ÜRETME.** Quality gate orchestrator + kullanıcı tarafından tutuluyor.

## Acceptance Criteria

- [ ] Backup tamamlandı (`_DISCARDED_codex_v1_<ts>/` 5 dosya içeriyor)
- [ ] REQUIRED READS bölümünün TÜM dosyaları okundu — Codex transcript'inde her okuma kayıtlı
- [ ] 5 yeni `codex_v2_scene_*.png` üretildi
- [ ] Resolution ≥ 1672×941 (önceki batch baseline) — daha büyük tercih (ideal 1920×1080+)
- [ ] Karakter detayları metadata.json'la **eşleşiyor** — generic değil
- [ ] Palette anchor uyumu (#7BA7BC cold blue accent her karakter sahnesinde)
- [ ] Camera 35° low top-down (bird's eye / pure iso / side-scroll YASAK)
- [ ] INDEX.md güncel + commit edildi

## CODEX_DISPATCH Global Kurallar

- Model: gpt-5.5
- Yorum yazma — WHY açık değilse istisna
- Hata yönetimi: image gen API failure'larında retry max 2, sonra raporla
- Güven Döngüsü zorunlu — REQUIRED READS bittikten SONRA Adım 2-3'e geç

## Commit Message

```
feat(concept-art): F1.2 retry — 5 codex_v2 scenes with full anchor + design context

- Backup: codex_scene_01..05 → _DISCARDED_codex_v1_<ts>/
- New: codex_v2_scene_01..05 with metadata.json-aware character descriptions
- Read context: Characters/anchors/{4 classes}/metadata.json + rotations/south.png
- Read context: TASARIM/{STYLE,ANIMATION,BOSS,COMBAT,VISUAL_QUALITY,ROOM_DESIGN_PHILOSOPHY,AIM_SHOT,ACT1_KEEP,MASTER_KARAR,SINIF_SKILL,MOB_COMPOSITION}.md
- Read context: MEMORY/{visual_identity,camera_scale,mob_boss_sizes,concept_art_pixel_art,pixellab_no_concept,pixellab_pipeline,animation_bible,v1_anim_4classes}.md
- Read context: GUIDES/{MASTER_ART_PIPELINE,CAMERA_WALL_OCCLUSION,ISOMETRIC_ENV_FEEDBACK}.md
- Resolution: <actual>×<actual> via image_gen.imagegen tool
- INDEX.md updated: v1 marked DISCARDED, v2 GENERATED
```

## Önceki batch'in eksiklikleri (FIX)

- ❌ Generic warrior/ranger/mage çıktı → ✅ anchor metadata.json detayı prompt'a inject
- ❌ 1672×941 küçük → ✅ tool max 16:9 aranacak, 1920×1080+ hedef
- ❌ Bağlam fakir (kısa prompt) → ✅ ~30 dosya REQUIRED READ
- ❌ Anchor PNG referans verilmedi → ✅ vision input destekliyorsa geçir, desteklemiyorsa textual ultra-detaylı

## Kaynak

- `MEMORY/feedback_image_gen_full_context.md` — bu kuralın memory'si
- `MEMORY/feedback_dispatch_approval_required.md` — onay alındı (kullanıcı 2026-05-10 net dedi: "öyle yapsın")
- Önceki commit: `b0a25e90` (codex_v1, FAIL — anchor uyumsuz)
