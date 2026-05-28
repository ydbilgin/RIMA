## Codex Verdict - bagimsiz technical analiz

Kaynaklar: `STAGING/PIXELLAB_INVENTORY_MASTER.md`, local asset globlari, `PlayableArena_Test01.unity`, `Assets/Prefabs/**/*.prefab`, `MEMORY/weapon_master_spec_10_class.md`, `MEMORY/warblade_12_common_skills_spec.md`, `SkillOfferUI.cs`, weapon runtime code/assets. NotebookLM sorgusu denendi, auth expired oldugu icin dosyalara fallback yapildi.

### Local vs Cloud cross-check
| Kategori | Local | Cloud only | Orphan |
|---|---:|---:|---:|
| PixelLab cloud object inventory | 43 cloud-matched local PNG + 29 prop prefab wrapper | 197 (task/master context) | 3 local statue PNG + 3 matching prefab wrapper |
| Explicit prop PNG glob (`Assets/Sprites/Environment/ShatteredKeep_PixelLab/**/*.png`) | 29 | n/a | 3 |
| Explicit prop prefab glob (`Assets/Prefabs/Props/ShatteredKeep_PixelLab/**/*.prefab`) | 29 | n/a | 3 |
| Additional local PixelLab mobs | 16 PNG + 16 mob prefab wrapper | n/a | 0 |
| Additional local PixelLab selected asset | 1 alabaster decal PNG | n/a | 0 |

Notes:
- Explicit task glob has 29 PNG and 29 prop prefabs: 15 mounting + 14 statue. The task mentions "33 prefab D2 backfill"; current filesystem has 29 under the specified prop prefab path and no `*obstacle*.prefab` under `Assets/Prefabs`.
- Orphan IDs confirmed from filename-to-master cross-check: `3675a661` (`statue_04`), `d5574785` (`statue_11`), `c5711681` (`statue_13`). Keep locally; cloud absence alone is not a delete reason.
- Import readiness: all 29 prop PNG are 64x64, alpha transparency on, PPU 32. Mounting imports have `alignment: 2` (Unity TopCenter); statues have `alignment: 0` (Center). All 29 prop prefabs contain a SpriteRenderer linked to the matching sprite.

### Codebase referans
- Kullanilan local: 3 local sprites are referenced in `PlayableArena_Test01.unity`: `mounting_09_bb34bab3` (26 GUID occurrences), `mounting_14_41342e20` (4), `alabaster_decal_5ccc5721` (1).
- Kullanilan by prefab wrapper only: 45 sprites are referenced by local wrapper prefabs (29 prop sprites + 16 mob sprites). These are import-ready but not scene-instantiated through prefab GUIDs.
- Kullanilmayan local: 29 prop prefabs and 16 mob prefabs have 0 references from `PlayableArena_Test01.unity` and 0 references from `Assets/Prefabs/**/*.prefab`; the scene currently references sprites directly.
- Cloud weapon 18 - import scope oneri: import only after remapping to the 10-class weapon spec; do not bulk-import all as-is. Master summary says 18 weapon objects, but concrete table rows expose 17 (`3x 8dir`, `7x 1dir`, `5x review`, `2x untagged`). Treat the missing review row as an inventory-doc mismatch.

D2 backfill audit:
- Mounting prefab sorting: all 15 prop prefabs use `m_SortingLayerID: 1200000005`, which maps to `Decor_Cliff`, order 50. This is technically coherent for `CliffFaceDecor`.
- Statue prefab sorting: all 14 statue prefabs use Default sorting layer, order 100. For `WallBlocker`/obstacle use this is incomplete; expected layer/category metadata should be explicit.
- AssetCategory metadata: no `AssetCategory.CliffFaceDecor` / `AssetCategory.WallBlocker` asset-pool binding was found for the 29 prop prefabs. Only `DefaultCollisionRules.asset` has name rules: `mounting_*` => Passable, `statue_*` => FullFootprint. `RoomPainter/AssetMetadata` has only one mounting asset (`mounting_09`) with category string `Props`, not MapDesigner `AssetCategory`.
- Mounting pivot: PixelLab/cloud inventory stores canvas size/status, not Unity pivot metadata. Local import is TopCenter by alignment; the `mounting_09` RoomPainter metadata uses `pivotAnchor: {x: 0.5, y: 0}`. That is a local-authoring mismatch to resolve before broader cliff-decor use.

### 10 Class silah boyut spec
Recommendation keeps runtime compatibility with current `HandAnchorAttach`, `OrientationSync`, and `WeaponDatabaseSO` tables: one static 1-dir weapon sprite, imported as transparent PNG, runtime rotation/offset, no 8-dir weapon import.

| Class | Weapon | Canvas | PPU | Pivot | Mount type |
|---|---|---:|---:|---|---|
| Warblade | Greatsword | 128x256 | 64 | grip/handle center at x 0.50, y 0.22-0.28 | 2-hand |
| Ronin | Katana + sheath | 96x192 | 64 | tsuka grip at x 0.50, y 0.18-0.24 | 1-hand + sheath visual |
| Gunslinger | Pistol | 64x64 per hand | 64 | grip at x 0.25-0.35, y 0.45-0.55 | dual mirrored |
| Ranger | Bow + arrow | 128x192 | 64 | bow hand point at x 0.50, y 0.50 | 2-hand/ranged |
| Elementalist | Rune disc / orb | 96x96 | 64 | center x 0.50, y 0.50 | hover/off-hand |
| Shadowblade | Dagger | 64x96 per dagger | 64 | grip at x 0.50, y 0.18-0.25 | dual mirrored |
| Ravager | Greataxe / heavy axe | 128x192 | 64 | grip at x 0.50, y 0.20-0.28 | 2-hand or dual-heavy variant |
| Hexer | Whip | 128x192 | 64 | handle at x 0.45-0.55, y 0.18-0.25 | 1-hand flexible |
| Brawler | Gauntlets | 96x96 pair sheet or 64x64 per fist | 64 | fist center x 0.50, y 0.50 | body-attached pair |
| Summoner | Tome + orb | 128x128 | 64 | tome grip x 0.35, y 0.45; orb center x 0.70, y 0.60 | off-hand/hover |

Codebase alignment findings:
- Current live weapon asset is `Assets/Resources/Weapons/Warblade_Greatsword.png`, 64x16, PPU 64, pivot `0.18,0.5`, referenced by `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` and `Assets/Prefabs/Combat/Weapons/Warblade.prefab`.
- `WeaponDatabaseSO` currently has only Warblade/Base, anchorOffset `{x:0.2,y:0.1,z:0}`, twoHanded true, orientBetweenHands true.
- Existing cloud 8-dir weapon objects are not aligned with the locked runtime model. Prefer one clean 1-dir source per class, then tune `anchorOffset`, `gripOffset`, and `handOffsets`.

### Weapon cloud object analysis
| ID | Title | Class mapping | Fit |
|---|---|---|---|
| `441bccf0` | 96x96 longsword 8dir | Warblade legacy/live reference | Still in master as complete; redundant for 1-dir runtime; not ideal for 128x256 greatsword source. |
| `692f43ce` | 64x64 katana 8dir | Ronin legacy/live reference | Still in master as complete; redundant for 1-dir runtime; lacks sheath source requirement. |
| `e84d8c62` | 64x64 longsword 8dir | Warblade backup | Redundant and small. |
| `4bde2642` | curse staff 64x64 | Hexer only if staff/scepter variant accepted | Usable draft, but current user spec says whip and memory prefers grimoire/totem/scepter. |
| `894bba4a` | flintlock pistol 32x32 | Gunslinger | Too small as source; regenerate/pad to 64x64. |
| `9312ea86` | reverse-grip dagger 32x32 | Shadowblade | Too small as source; regenerate/pad to 64x96. |
| `a032d9b5` | katana blade 64x64 | Ronin | Usable draft blade; missing sheath and preferred longer canvas. |
| `31ee0f73` | greatsword 64x64 | Warblade T2/backup | Too small for current 128x256 directive. |
| `afcab14c` | soul lantern 32x32 | Summoner | Too small; regenerate/pad to 128x128 or 96x96. |
| `19693073` | single-handed hammer/axe 32x32 | Ravager candidate only if axe read holds | Too small and class fantasy mismatch if Ravager is greataxe. |
| `ebc33ebf` | compound bow 64x64 | Ranger | Good semantic match; source canvas should be 128x192. |
| `c0509b93` | two-handed greatsword 64x64 | Warblade backup | Semantic match; canvas too small. |
| review IDs | staff/pistol/dagger/katana/greatsword awaiting-selection | Same class drafts | Do not import until review selection is finalized. |

### Skill icon 22 cloud
- Master has 20 generic `skill_icons` + 3 `skill_icons_special` = 23, not 22.
- Strict 12-skill spec exact-title match: 0 from cloud metadata. Fuzzy usable matches: 2-3 (`crushing blow` can map to Crippling Blow/Iron Crush ambiguity, `spinning sword` can map to Blade Rush/Gravity Cleave ambiguity, `rift portal strike` is not Warblade-common).
- Format spec: 64x64 PNG is correct for `SkillOfferUI`'s 64x64 icon slot.
- Import scope: GEREKSIZ as a blind bulk import. GEREK only if each icon is renamed/mapped to a concrete `SkillData` and `SkillOfferUI.BuildSkillCard` is changed to render `skill.icon`.
- Codebase contradiction: local non-cloud UI icons already exist under `Assets/Sprites/UI/Icons` (19 PNG, all 64x64, PPU 100). Seven serialized `Assets/Data/Skills/*.asset` files already have icon GUIDs (`Iron Charge`, `Gravity Cleave`, `Death Blow`, `War Stomp`, `Crushing Blow`, `Rift Strike`, `Whirlwind Slash`). However live `SkillDatabase` creates runtime `SkillData` without icon assignment, and `SkillOfferUI.cs` always draws a colored placeholder instead of `skill.icon`. This explains why cloud icons are not functionally imported: the live UI path does not consume icons yet.

### Open technical questions
1. Inventory says 18 cloud weapons but the detailed table exposes 17 weapon rows. Which review object is missing from the master table?
2. Should the 29 prop prefabs be the canonical scene-placement assets, or should RoomPainter continue stamping raw sprites directly with `RoomPainterAssetBinding`?
3. For D2 categories, should mounting/statuary be registered in a MapDesigner `AssetPoolSO` with `AssetCategory.CliffFaceDecor` / `WallBlocker`, or should RoomPainter string metadata remain the source of truth?
4. Should weapon import standard stay at PPU 64 to match current `HandAnchor` offsets, or move to PPU 100 like character/UI icons with a coordinated offset retune?
5. Should Hexer follow this task's whip request, or the current memory spec's grimoire/totem/scepter direction?
6. Should cloud skill icons be ignored in favor of the existing local `Assets/Sprites/UI/Icons` set, then wire the live runtime UI to those local icons first?
