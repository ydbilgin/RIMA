# Weapon Pipeline v1 — LOCK (Karar #123 + #144 + #146)

> **Source:** NLM canonical query 2026-05-22. Notebook 30ddffa5.
> **Status:** Decoded from canon — implementation-ready
> **No new design choices in this doc.** Just consolidates NLM into a single actionable spec.

---

## 1. Decouple Architecture (Karar #123)

**Universal rule:** Karakter body sprite **SİLAHSIZ** üretilir. Silah ayrı bir sprite olarak üretilir ve Unity'de **Child SpriteRenderer** olarak HandAnchor'a parent edilir.

### Pipeline diagram
```
PixelLab Create Character Pro (10 class body, weaponless)
   ↓
Unity Sprite (8-dir rotations + animation states per Karar #145)
   ↓
Player.prefab (Class variant: Warblade/Ronin/etc) — body SpriteRenderer
   └── HandAnchor (empty GameObject child at hand position)
        └── WeaponSR (Child SpriteRenderer, weapon sprite assigned at runtime)

PixelLab Create Image S-XL (32×32 weapon sprite, transparent BG, Direction: None)
   ↓
Unity Sprite (custom pivot at grip)
   ↓
WeaponPrefab (prefab containing single SpriteRenderer + weapon sprite + custom pivot)
   ↓
WeaponDatabaseSO entry binds (classId, formId) → WeaponPrefab + anchorOffset
   ↓
At runtime: WeaponSpawner reads database → instantiates WeaponPrefab → parents to HandAnchor → applies offsets
```

### Exceptions
| Class | Exception |
|---|---|
| **Ronin** | Scabbard (kın) is part of body sprite (identity element). Weapon sprite = drawn katana only. WeaponSR idle = empty (sheath visible on body), attack = drawn katana. |
| **Brawler** | NO weapon sprite. Body has clenched fists embedded. WeaponDatabaseSO entry empty for Brawler. |
| **Elementalist** | **AMENDED 2026-05-22 (user direction):** Sprite-first approach — arcane orb sprite (32×32, transparent) IS generated. Unity VFX overlay (particle swirl, glow flare) layered ON TOP of sprite at runtime. Previous NLM canon (VFX-only, no sprite) superseded. Rationale: sprite-as-base anchor lets the orb still register as physical item in inventory/upgrades, VFX adds dynamic juice. WeaponDatabaseSO entry now: `classId="Elementalist", formId="Base", weaponPrefab=Elementalist_ArcaneOrb.prefab`. |

---

## 2. Tool Selection (per asset class)

| Asset | Tool | Direction | Canvas | BG |
|---|---|---|---|---|
| Character body (10 class) | **Create Character Pro New** OR **Create Image Pro** | 5 dir gen + 3 mirror (S/SE/E/NE/N + flipX → W/SW/NW) | 64×64 chibi | Opaque |
| Character anim states (idle/run/attack/dash/hit/death) | **Create Character State** (Karar #145) | First-frame state lock + 4f/6f anim | 64×64 | Opaque |
| **Weapon sprite (8 weapons)** | **Create Image S-XL (new)** | **None** (1 frame static) | **32×32** | **Transparent** |
| Wall (placeholder for Phase B) | create_object | n_frames=16 multi-variant | 64×64 or higher | Opaque |
| Decoration / props | create_object | n_frames=4-16 | 32-64 | Opaque/transparent mix |
| Tiles (floor materials) | create_tiles_pro | tile_view_angle=90, tile_depth_ratio=0 | 64×64 | Opaque |

**Weapon tool rationale (Karar from NLM):**
- `Create Image S-XL (new)` has init image (style/ref) support AND Direction setting
- `Create Image Pro` similar but no Direction setting (must write into prompt)
- `Create Object` no init image — can't lock to character style anchor → not preferred for weapons

**Recommended:** Create Image S-XL (new), init image = `Characters/anchors/reference.png` (canonical style anchor), Direction: None, canvas 32×32, transparent BG.

---

## 3. Direction & Frame Count (Karar #114, #144)

| Asset type | Directions | Frames | Note |
|---|---|---|---|
| Character body | **8 (5 gen + 3 mirror)** | per-state (idle 4f, run 6f, attack varies) | Karar #114 hard lock |
| **Weapon sprite** | **1 (None)** | **1 (static)** | Karar #144 — silah yön bazlı çizilmez |
| Two-handed weapon | 1 sprite | 1 frame | Same as above |
| Dual-wield (Gunslinger / Shadowblade / Ravager) | **2 sprite (Left + Right)** | 1 frame each | Mirror Left to Right via Unity flipX OR generate as separate sprites |

**Weapon swing animation:** NOT generated as new sprite. Achieved at runtime via Unity `AnimationCurve` rotating weapon Transform (Karar #144).

---

## 4. Attachment Geometry (HandAnchor)

### Level 1 — MVP (current target)
- Each class prefab has **single HandAnchor** child empty GameObject at fixed local position (e.g., (0.3, 0, 0) for south-facing)
- Weapon parented at runtime, position offset via `WeaponEntry.anchorOffset` (Vector3)
- Weapon swing = AnimationCurve on weapon Transform rotation

```csharp
// Pseudocode
weaponInstance = Instantiate(weaponPrefab, handAnchor);
weaponInstance.transform.localPosition = weaponEntry.anchorOffset;
weaponInstance.transform.localRotation = Quaternion.Euler(0, 0, weaponEntry.orientationOffsetDegrees);
```

### Level 2 — Polish (post-MVP)
- Per-animation-frame hand position via `SpriteHandData` ScriptableObject
- Weapon Transform syncs to current frame's hand pixel position
- More expensive (per-frame update), reserved for final polish phase

**Pivot:** Each weapon sprite has `customPivot` set in Unity Sprite Editor — pixel-precise at the grip (kabza). This makes weapon rotate around grip during swing, not center.

---

## 5. Weapon List (8 sprites — Karar #80 Class Silhouette Bible)

Brawler and Elementalist excluded (no sprite). Remaining 8 classes × 1 weapon (Gunslinger/Shadowblade/Ravager × 2 sprites = dual-wield).

| # | Class | Weapon | Sprite count | Notes |
|---|---|---|---|---|
| 1 | Warblade | Greatsword | 1 | Heavy two-handed. T2 Rift variant = Karar #124 (separate gen, post-MVP) |
| 2 | Ronin | Katana (drawn) | 1 | Scabbard stays on body. Iaido draw exception (Karar #146). |
| 3a | Gunslinger | Flintlock pistol (LEFT) | 1 | Barrel facing east |
| 3b | Gunslinger | Flintlock pistol (RIGHT) | 1 | Barrel facing west (mirror of LEFT) |
| 4 | Ranger | Recurve bow | 1 | Bow string vertical, ready posture |
| 5a | Shadowblade | Dagger (LEFT) | 1 | Curved blade, dark steel |
| 5b | Shadowblade | Dagger (RIGHT) | 1 | Mirror |
| 6a | Ravager | Axe (LEFT) | 1 | Heavy single-bit |
| 6b | Ravager | Axe (RIGHT) | 1 | Mirror |
| 7 | Hexer | Hex staff | 1 | Wood + bound crystal at top, mid-length |
| 8 | Summoner | Soul lantern | 1 | Iron frame, chain at top, ghost-flame inside |

**Total unique sprites:** 11 (Warblade, Ronin, Gunslinger×2, Ranger, Shadowblade×2, Ravager×2, Hexer, Summoner)

**Optional mirror reuse:** Gunslinger/Shadowblade/Ravager Left could be generated and Right via Unity `flipX` to reduce to 8 unique sprites. Decision per-class — generation cost low (~50 gen total). Recommended: **generate both** for canon hand orientation, avoid mirror artifacts.

---

## 6. Web UI Production Workflow (user-driven, MCP not required)

PixelLab MCP currently has connection timeout issues (server-side). User can produce via web UI:

**Web URL:** https://pixellab.ai/app/create

**Steps per weapon:**
1. Tool: `Create Image S-XL (new)`
2. Description: copy from `STAGING/weapon_web_prompts_v1.md` (one prompt per weapon)
3. Style anchor (init image): `Assets/Art/Characters/anchors/reference.png` (or canonical reference)
4. Direction: **None**
5. Canvas size: **32×32**
6. Background: **Transparent**
7. Generate (single image, ~1-2 gen credit)
8. Download PNG → save to `Assets/Art/Weapons/<ClassName>/<weapon_name>.png`
9. In Unity Sprite Editor: set Pivot → Custom → click on grip pixel
10. Apply

**Bulk production:** All 11 weapons in one session ~30-60 min user time.

---

## 7. Code State (already in repo)

| File | Status | Notes |
|---|---|---|
| `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs` | ✅ LIVE | Has `WeaponEntry` with classId, formId, weaponPrefab, anchorOffset, gripOffset, twoHanded, orientBetweenHands, orientationOffsetDegrees |
| `Assets/Resources/WeaponDatabase.asset` | ✅ LIVE | Singleton instance; populate entries when sprites land |
| HandAnchor child on Warblade prefab | ⏳ Phase E STEP 4 | Codex will add empty GameObject child during prefab build |
| WeaponSpawner.cs (runtime instantiate at HandAnchor) | ❌ Missing | New file needed — Phase E+ small task, ~30 min |
| WeaponVisibility input puff system (Karar #146) | ❌ Missing | Polish task post-vertical-slice |

**Phase E addendum:** When Codex builds Warblade prefab (STEP 4), include HandAnchor child empty GameObject at localPos (0.3, 0, 0). Ready for weapon Child SR attach.

**Follow-up tasks (Phase J+ or post-K):**
1. `WeaponSpawner.cs` — reads WeaponDatabaseSO + instantiates weapon prefab as HandAnchor child
2. `WeaponVisibility.cs` — input-driven puff system (Karar #146): combat input → puff-in 0.15s; idle 5s → puff-out 0.3s; Ronin exception
3. Weapon swing animation: AnimationCurve on weapon Transform.rotation triggered by attack frames

---

## 8. Visibility System (Karar #146 — input-driven puff)

Body sprite weaponless (universal). WeaponSR child default hidden. Trigger logic:

```
Combat input pressed (LMB/RMB/Q/E/R/F/V):
   → WeaponVisibility.PuffIn(0.15s)
   → WeaponSR.enabled = true
   → scale lerp 0→1, alpha 0→1
   → reset lastAttackTimer = 0

Last attack timer > 5s (no combat input):
   → WeaponVisibility.PuffOut(0.3s)
   → scale lerp 1→0, alpha 1→0
   → WeaponSR.enabled = false on complete

Dash (Space):
   → NOT trigger puff (movement-only input)

Ronin exception:
   → Puff system DISABLED
   → WeaponSR idle = sheath sprite (or hidden if sheath embedded in body)
   → WeaponSR attack = drawn katana sprite
   → No puff-in/out; sharp swap on attack input
```

VFX: puff-in/out optional small dust particle (Karar #146 cinematic detail). MVP can skip particle, use scale+alpha lerp only.

---

## 9. Open Items / Sub-decisions

1. **Anchor reference pose:** `Characters/anchors/reference.png` exists? If not, pick canonical pose (warblade_south.png best candidate).
2. **Dual-wield (Gunslinger/Shadowblade/Ravager):** generate both L+R sprite, OR generate one + Unity flipX? Recommendation: **generate both** (10 extra gen, canon-correct mirror artifacts avoided).
3. **T2 Rift variants (Karar #124):** Warblade T2 first (post-MVP polish). Defer all 10 T2 to post-vertical-slice.
4. **Weapon prefab style:** prefab-per-form (`Warblade_Base.prefab`, `Warblade_T2_Rift.prefab`) OR sprite-swap on same prefab? Recommendation: **prefab-per-form** for cleaner WeaponDatabaseSO entries.

---

## 10. Implementation Order (post-Phase E)

| # | Task | Agent | Time | Phase |
|---|---|---|---|---|
| 1 | User produces 11 weapon sprites via web UI | User manual | 30-60 min | Parallel to Phase F-K |
| 2 | Sprite import + custom pivot setup | Codex (or user) | 15 min | Post-weapon-gen |
| 3 | WeaponPrefab build batch (11 prefabs) | Codex | 30 min | Post-sprite-import |
| 4 | WeaponDatabase.asset populate (11 entries) | Codex | 10 min | Post-prefab-build |
| 5 | `WeaponSpawner.cs` write + wire to Player.prefab | Codex → Opus QC | 45 min | Phase J+ |
| 6 | `WeaponVisibility.cs` (Karar #146 puff system) | Codex → Opus QC | 1 h | Phase J+ |
| 7 | Weapon swing AnimationCurve setup | Codex | 30 min | Phase J+ |
| 8 | Integration test (10 class × weapon equipped) | Codex + manual | 30 min | Phase K |

**Critical path:** User generates 11 sprites ANY TIME. Code can land first; sprites bind to existing prefab slots when ready.
