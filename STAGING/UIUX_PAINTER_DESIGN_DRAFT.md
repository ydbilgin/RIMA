# RimaUnifiedPainter UI/UX Redesign — DRAFT v3.1 (FINAL — LIVE)

> **Owner:** rima-design (Opus)
> **Source file (DO NOT EDIT this iter):** `Assets/Editor/RimaUnifiedPainterWindow.cs` (2812 satır)
> **Mode:** Spec only. Hiç kod yazılmadı. Mevcut dosya bozulmadı.
> **Started:** 2026-05-20 S95
> **v3.1 revision:** Codex v3 NEEDS_REVISION'da bulduğu 2 son polish (open-question contradiction + missing caller) düzeltildi. Codex v3.1 polish review = **LIVE_WITH_MINOR_NOTES** (implementation-ready).

## Verdict (FINAL)

**LIVE_WITH_MINOR_NOTES** — Codex v3.1 review onaylı. Spec downstream implementation task'ına hazır.

Codex v3.1 quotable:
> "v3.1 fixes the two remaining polish blockers: the SO script path is now consistent and the CollisionResolver caller list includes the bulk asset-pack setup path. The spec is approved for downstream implementation, with only stale editorial labels left as minor notes."

**Per-panel final:** Panel 1 LIVE, Panel 2 LIVE_WITH_MINOR_NOTES, Panel 3 LIVE, Panel 4 LIVE_WITH_MINOR_NOTES.

**Minor cleanup left for implementation Codex (informational, not blocking):**
- Stale label "Final Spec (v2)" → "Final Spec (v3.1)" at line ~116.
- Stale "Codex Re-Review v2" section header at line ~567.
- Cache key wording at line ~256 still mentions "typed ValueTuple key" but pseudo-spec correctly shows `struct PreviewCacheKey` — wording carryover, struct authoritative.

## Iter Log

- **v1** (2026-05-20 11:58 → 12:07): Opus initial spec → Codex review v1 = LIVE_WITH_NOTES.
  - 4/4 panel implementable ✓.
  - 5 corrections: SetActive→SceneVisibilityManager, cache key expand, PeekTargetParent, ZWSP cleanup, lock durability tooltip.
  - Extra: colorblind gizmo, GroupClassifier + CollisionResolver single sources.
- **v2** (2026-05-20 12:07 → 12:20): Opus v2 → Codex review v2 = **NEEDS_REVISION**.
  - 4 hard issues caught: (a) `SceneVisibilityManager` named arg yanlış (`includeChildren` → must be `includeDescendants`), (b) ZWSP düzeltmesi spec snippet'ında HÂLÂ literal U+200B var — kod örneği `"_\u200B"` escape ile yazılmalı, dosyaya kaçmış invisible char değil, (c) `CollisionRulesSO` asmdef yerleştirmesi yanlış — painter `Assets/Editor/` (predefined assembly), `Assets/Editor/MapDesigner/` (asmdef'li) ona referans veremez (`autoReferenced: false`), (d) banner state matrix kafa karıştırıcı satırlar.
  - Minor: cache `Dictionary<int>` yerine typed ValueTuple key, SessionState key namespace, `OpenPropertyEditor`, `resolveReason` GC, GetTargetParent → PeekTargetParent refactor zorunlu.
- **v3** (2026-05-20 12:20 → 12:55): Opus v3 → Codex review v3 = **NEEDS_REVISION** (close to LIVE).
  - 4 hard fix + 5 minor fix tümü `W1=Y W2=Y W3=No` (1 hariç).
  - 2 son polish: (i) Open question 1 hâlâ eski yanlış asmdef konumunu öneriyordu — body düzeltilmiş ama soru güncel değildi. (ii) Q8 caller list `ConfigureAssetPackColliders` (line 1799-1801, button line 704-709) eksikti.
  - Z3 (HashCode.Combine) + Z4 (OpenPropertyEditor) Unity 6000.3 verified by Codex live reflection.
- **v3.1 (this doc):** v3 body korunur, sadece 2 open-question fix. Açık sorular 1 ve 8 güncellendi.
- **v3.1 review** (2026-05-20 12:32 → ~12:35): Codex polish pass = **LIVE_WITH_MINOR_NOTES**. P1+P2+P3 hepsi PASS. Byte-level ZWSP = 0 verified. Spec implementation-ready.

## Scope Reminders (HARD)

- Sadece **UI/UX** layout. Painter'ın paint/erase/save/load mantığına dokunulmayacak.
- Geri dönülebilir: bu spec uygulanmadan önce mevcut window olduğu gibi çalışır.
- Cerrahi (Karpathy #3): yeni sistem değil, var olanı görselleştirmek + 1 yeni SO (CollisionRulesSO).
- **v2 yeni hedef:** Single source of truth — `GroupClassifier` ve `CollisionResolver` helper'ları drift'i kapatır.

## Mevcut Window Yapısı (referans)

```
RimaUnifiedPainterWindow.OnGUI()
├── DrawHeader()                      [line 394]
└── HorizontalScope
    ├── Left (260px wide)
    │   └── DrawOptionsPanel()        [line 422]
    │       ├── Category buttons     (Zemin/Duvar/Obje/Canavar)
    │       ├── Selected Brush Status
    │       ├── Foldout 1: Target Configuration (biome / tilemap / parent)
    │       ├── Painting Tools (B/E/I)
    │       ├── Foldout 3: Brush & Placement Settings
    │       ├── Foldout 4: Collider Boundaries  ← Prop+Wall only
    │       ├── Foldout 5: Level File Management
    │       └── Status HelpBox (warning/info)
    └── Right (flexible)
        └── DrawPalettePanel()        [line 766]
            ├── Toolbar search field
            └── Scroll: grid of DrawPaletteItemButton(92×110 px)
```

5 bilinen UX sorunu (görev brief'ten):

1. Grup paneli yok — Walls/Statues/Mobs/Patches/WallMountings/FloorProps sadece Hierarchy'de.
2. Target parent warning yanıltıcı — `GetTargetParent()` Props_Root auto-create ediyor (line 2265) ama HelpBox "Assign a Target Parent" diyor.
3. Palette ad clip — 92×110 px tile, 20px label, uzun adlar (örn `wall_face_NS_damaged`) "wall_face..." olarak kesiliyor.
4. Hitbox ayarı görünmez — `customCollisionMode = Custom` seçilince Vector2Field açılır; Auto/Wall/Full/Small modlarının gerçekte ürettiği size/offset gözükmez, Scene'de gizmo yok.
5. Per-prefab override yok — sadece per-instance. "Bu prefab adına tutturulu default collider'ı şöyle yap" diyemiyor.

---

## v2 Genel Düzeltmeler (Codex v1 feedback)

### Single source of truth helper'lar

İki yeni static (yine Editor-only, aynı window içinde nested veya partial class):

```csharp
// Pseudo-spec. Implementation YOK. Sadece kontrat:

internal static class GroupClassifier {
    // Tek nokta: prefab+category → "Walls" / "Statues" / "WallMountings" /
    //                              "Patches" / "Mobs" / "FloorProps".
    // GetOrCreateGroupParent (line 2290) buna delege olur.
    // Panel 1 ITERATE bu listenin sabit 6 ismi üzerinden.
    public static readonly string[] CanonicalGroups =
        { "Walls", "Statues", "WallMountings", "Patches", "Mobs", "FloorProps" };
    public static string Classify(string prefabName, PaletteCategory cat);
}

internal static class CollisionResolver {
    // Tek nokta: prefab+category+chosenMode+rulesSO → ResolvedCollider.
    // ConfigureCollider (line 1901) buna delege olur (dry-run versiyonu Preview).
    public struct ResolvedCollider {
        public CollisionMode effectiveMode;     // Auto resolve sonrası
        public Vector2 worldSize;
        public Vector2 worldOffset;
        public string layerName;
        public int sortingOrder;
        public string resolveReason;             // "rule:wall_*" / "keyword:torch" / "category:Wall"
    }
    public static ResolvedCollider Resolve(
        GameObject prefab,
        PaletteCategory cat,
        CollisionMode chosenMode,
        Vector2 customSize, Vector2 customOffset,
        float scaleMult, int rotationSteps,
        CollisionRulesSO rules);
    // Dry-run; NO AddComponent, NO SetActive, NO asset mutation.
}
```

Bu yapı Codex v1 F2 endişesini (palette badge / scene gizmo / placement drift) kapatır: 3 caller tek fonksiyona gider.

**Önemli:** Bu helper'ları **spec uygulaması sırasında** yazacak Codex/orchestrator. Bu doc onları "var" olarak referans alır. Mevcut `ConfigureCollider` ve `GetOrCreateGroupParent` bozulmaz; sadece içleri `Resolver`'a delege olur (1 line each).

---

## Final Spec (v2 — Codex re-review için)

### Genel Layout Değişikliği

Sol kolon 260px → **300px** (daha rahat collision inspector). Sağ kolonun üstüne ince bir **Status Banner** eklenir (header'ın altı). Palette artık alt-grid + üstte rozet sütunu olur.

```
+----------------------------------------------------------------------+
|  RIMA Unified Painter         [active brush icon + name]            |  ← DrawHeader (mevcut, dokunma)
+----------------------------------------------------------------------+
|  STATUS BANNER  ✓ Tilemap: ShatteredKeep_TM | Parent: Props_Root(auto) |  ← YENİ Panel 4
+--------------------+-------------------------------------------------+
|  OPTIONS (300px)   |  PALETTE                                        |
|                    |                                                 |
|  Category btns     |  [search] [Clear]                               |
|  Brush status      |                                                 |
|                    |  +-----+ +-----+ +-----+ +-----+                |
|  Foldout 1: Target |  |  ◼  | |  ◼  | |  ◼  | |  ◼  |  ← Panel 3    |
|  Painting tools    |  | img | | img | | img | | img |                |
|  Foldout 3: Brush  |  | nam | | nam | | nam | | nam |                |
|  Foldout 4: Coll.  |  | e wr| | e wr| | e wr| | e wr|                |
|  Foldout 5: Maps   |  | [B] | | [P] | | [B] | | [B] |  ← badge      |
|                    |  +-----+ +-----+ +-----+ +-----+                |
|  Foldout NEW:      |                                                 |
|  Scene Org panel   |                                                 |
|  ← Panel 1         |                                                 |
|                    |                                                 |
|  Foldout NEW:      |                                                 |
|  Coll. inspector   |                                                 |
|  ← Panel 2         |                                                 |
+--------------------+-------------------------------------------------+
```

---

### Panel 1: Scene Organization (in-window hiyerarşi)

**Konum:** Sol kolon, "5. Level File Management" foldout'unun ALTINA, yeni `BeginFoldoutHeaderGroup("6. Scene Organization")` ekle. Default expanded.

**Mockup:**

```
▼ 6. Scene Organization
┌─────────────────────────────────────────┐
│ Root: Props_Root (auto)        [Ping]   │
│ ─────────────────────────────────────── │
│ ▸ Walls          12  [👁] [🚫] [×]      │
│ ▸ Statues         3  [👁] [🚫] [×]      │
│ ▸ WallMountings   8  [👁] [🚫] [×]      │
│ ▸ Patches         5  [👁] [🚫] [×]      │
│ ▸ Mobs            2  [👁] [🚫] [×]      │
│ ▸ FloorProps     27  [👁] [🚫] [×]      │
│ ─────────────────────────────────────── │
│ Total instances: 57                     │
│ [Select All in Group] [Frame in Scene]  │
└─────────────────────────────────────────┘
```

**Davranış (v2 düzeltilmiş):**

- **Group iteration:** `GroupClassifier.CanonicalGroups` array'i (sabit 6 isim). `parent.Find(groupName)` `Transform` döner — yoksa "0  [+ Create]" göster.
- **Count:** `group.childCount`. Cheap.
- **👁 (visibility):** ~~`groupGo.SetActive`~~ ✗ **DEĞİŞTİ.** Şimdi `SceneVisibilityManager.instance.Hide(groupGo, includeDescendants: true)` / `Show(groupGo, includeDescendants: true)`. Bu **gerçek editor-only visibility**, save'i etkilemez, runtime davranışı bozmaz. Toggle state için `SceneVisibilityManager.instance.IsHidden(groupGo, includeDescendants: true)` query'si. **NOT (Codex v2 V1):** named argument `includeDescendants` — `includeChildren` parametre adı YOK Unity API'da. Üç çağrı da aynı named arg kullanır.
  - Tooltip: "Toggle editor visibility (Scene View only, does not affect save or runtime)."
- **🚫 (picking lock):** `SceneVisibilityManager.instance.DisablePicking(groupGo, includeDescendants: true)` / `EnablePicking(groupGo, includeDescendants: true)`.
  - Tooltip: "Toggle editor scene picking. **Note:** this is editor state stored under `Library/`, not a durable scene-saved lock. May reset on project re-open."
  - Codex E3 uyarısı: durable lock vaat etmiyoruz, sadece "shift+click ile yanlışlıkla seçimi engelle" workflow yardımcısı.
- **×:** `EditorUtility.DisplayDialog("Delete N objects in group X?", "This cannot be undone via Hierarchy if you remove the group itself. Children objects will be destroyable via Undo.", "Delete", "Cancel")`. Confirm sonra `Undo.IncrementCurrentGroup() + Undo.SetCurrentGroupName($"Clear {groupName}")` + her child için `Undo.DestroyObjectImmediate(child)`. Group transform'unu KORU (sadece children silinir) — drift önleme.
- **[Ping]:** root için. Root parent `targetParent` veya `targetTilemap.parent` peek edilir (auto-create YOK, aşağıdaki Panel 4 ile aynı `PeekTargetParent` helper kullanılır).
- **[Select All in Group]:** `Selection.objects = group children.Select(t => t.gameObject).ToArray()`.
- **[Frame in Scene]:** `SceneView.lastActiveSceneView.Frame(boundsFromChildren, false)`.

**Group existence state:**

- Group yoksa: row label gri + "(empty)" badge, butonlar disabled (`BeginDisabledGroup`).
- Group var ama empty: row aktif, count "0", silme butonu group transform'u siler (`Undo.DestroyObjectImmediate(groupGo)`).
- Group var + dolu: tam fonksiyonel.

**Refresh strategy (Codex B1 düzeltilmiş):**

- `[SerializeField] bool sceneOrgDirty = true` flag. (NOT serialized as dictionary — runtime only `bool`.)
- `OnHierarchyChange()` override → `sceneOrgDirty = true; Repaint();`
- `OnEditorUpdate()` içinde (`EditorApplication.update`): zaman tabanlı refresh **YOK**. Sadece `OnHierarchyChange` trigger'ı yeterli.
- Bu sayede infinite repaint loop kapatılır.
- Drift uyarısı: `OnHierarchyChange` bir frame gecikebilir, ama count read-only label'a 1-frame stale OK.

**IMGUI notları (Codex A2 uyarısı):**

- Group row: `EditorGUILayout.HorizontalScope` içinde `GUILayout.Label` + `GUILayout.Button` mini'ler. **`GUILayoutUtility.GetRect` ile manuel rect KULLANMA** — Codex A2 layout race uyarısı: aynı row'da GetRect + EditorGUILayout karıştırma.
- Foldout expand state: `Dictionary<string,bool> groupExpanded` runtime field (Codex D2 + v2 N7: dictionary serialize edilmez; persistence için window-unique namespace `SessionState.GetBool($"RIMA.UnifiedPainter.groupExpand.{name}", true)`). 6 grup × bool = SessionState'e 6 entry, ucuz.
- Icons: Unity built-in — `EditorGUIUtility.IconContent("animationvisibilitytoggleon")` / `"...off"`, `"InspectorLock"` / `"InspectorLockOff"`, `"TreeEditor.Trash"`. Emoji yerine bunlar (Codex cross-platform stable).

---

### Panel 2: Collision Inspector (always-visible)

**Konum:** Mevcut "4. Collider Boundaries" foldout'unun YERİNE geç. İsim değişir: "4. Collision Inspector". Default expanded. Prop+Wall+Mob'da görünür (Floor'da gizli — tile'lar collider'sız).

**Mockup:**

```
▼ 4. Collision Inspector
┌─────────────────────────────────────────┐
│ Active Mode: (Auto resolved)            │
│   ⓘ Resolves to: WallBlock              │
│      ← prefab name starts with "wall_"  │
│      ← (or "rule:wall_*" if SO matched) │
│                                         │
│ Override Mode: [Auto         ▼]         │
│                                         │
│ Live preview (read-only):               │
│   Size:   (2.00, 0.80) world            │
│   Offset: (0.00, -0.60) world           │
│   Layer:  Walls                         │
│   Sort:   20                            │
│                                         │
│ ☑ Show gizmo in Scene View              │
│                                         │
│ Per-Prefab Rule:                        │
│   Source: CollisionRulesSO (default)    │
│   [Edit Rules SO...]                    │
│   [Save Current as Rule for "wall_*"]   │
│ ─────────────────────────────────────── │
│ ⓘ Brush slot live-preview               │
│   tracks selected palette tile.         │
└─────────────────────────────────────────┘
```

**Davranış parçaları:**

#### 2.1 Always-visible default resolver

`CollisionResolver.Resolve(...)` çağrılır → `ResolvedCollider` döner. Bu helper SİNGLE SOURCE OF TRUTH (Codex F2 düzeltmesi).

- `effectiveMode` → "Resolves to: X" satırı
- `resolveReason` → ⓘ açıklama (rule:wall_* / keyword:torch / category:Wall / user:override)
- `worldSize`, `worldOffset`, `layerName`, `sortingOrder` → read-only fieldlar

#### 2.2 Cache key (Codex B2 düzeltilmiş)

Cache key v1'de eksikti. v3 (Codex v2 N3 + V1 düzeltme): **typed ValueTuple key**, hash collision'ı azaltır.

```
// Pseudo-spec
readonly struct PreviewCacheKey : IEquatable<PreviewCacheKey> {
    public int prefabId;                  // 0 if null
    public PaletteCategory category;
    public CollisionMode chosenMode;
    public float scaleMult;
    public int rotationSteps;
    public Vector2 customSize;            // default if not Custom
    public Vector2 customOffset;
    public int rulesSOVersion;            // EditorUtility.GetDirtyCount(rulesSO) or 0
    // Equals + GetHashCode HashCode.Combine ile
}
Dictionary<PreviewCacheKey, ResolvedCollider> previewCache;
```

Selected brush, mode, custom field, SO edit, rotation, scale değiştiğinde otomatik invalidate (key inequality). SO save edildiğinde `EditorUtility.GetDirtyCount(rulesSO)` artar (Codex v2 N6 verified — non-obsolete API) — natural cache miss.

**`resolveReason` GC riski (Codex v2 N3):** Sadece cache miss'te hesapla; cached entry zaten string tutar, OnGUI repaint'inde sadece label render — zero alloc. Const/static string'leri prefer et (örn `"category:Wall"`, `"keyword:torch"`); rule match için `$"rule:{pattern}"` cache miss yolunda kalır.

Codex uyarısı: `GetSpriteVisibleBounds` texture pixel scan edebilir (line 1988 cache zaten var). Resolver bu mevcut cache'i kullanır.

#### 2.3 Scene gizmo (Codex E2 düzeltilmiş)

`OnSceneGUI` (line 947) içinde, `UpdatePreviewObject` yanına ekle. Renkler **colorblind-safe + alpha + outline**:

| Mode | Wire color | Alpha | Backing outline |
|------|-----------|-------|-----------------|
| Passable | green (#3ad17b) | 0.6 | white dashed |
| SmallFootprint | yellow (#e5c54a) | 0.7 | dark thin |
| FullFootprint | orange (#e58a3a) | 0.8 | dark thin |
| WallBlock | red (#d54a4a) | 0.9 | white solid (dark scene için) |
| Custom | magenta (#c050d0) | 0.8 | white dashed |

Codex E2 önerisi: ek olarak **shape hint** — WallBlock için `Handles.DrawDottedLine` ile 2 cross diagonals, Passable için sadece outline, no fill. Bu colorblind kullanıcıya shape ile geri-fallback verir.

Toggle: panel'deki `☑ Show gizmo in Scene View`. State: `[SerializeField] bool showCollisionGizmo = true;`.

#### 2.4 Per-prefab override — `CollisionRulesSO`

**Yeni asset:** `Assets/Editor/CollisionRulesSO.cs` + `.asset` instance(s).

**Codex v2 V14 + N1 düzeltmesi:** Mevcut painter dosyası `Assets/Editor/RimaUnifiedPainterWindow.cs` **predefined Editor assembly**'de (root `Assets/Editor/`). `Assets/Editor/MapDesigner/RIMA.MapDesigner.Editor.asmdef` (autoReferenced=false, references `RIMA.Runtime` + InputSystem + Tilemap.Extras) painter'a referans VERMEZ — painter o asmdef'in dışında.

Eğer SO MapDesigner asmdef altında olursa: SO → painter type referansı verilemez (asmdef predefined assembly'den okuyamaz, painter assembly'sini referans listesinde tutmadan). Bu **compile-break**.

**Karar:** SO script'i `Assets/Editor/CollisionRulesSO.cs` — painter ile aynı predefined Editor assembly. `RimaUnifiedPainterWindow.CollisionMode` nested enum'a doğrudan referans verebilir. Asset dosyası (`.asset` instance) `Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset` olabilir (asset data konum-bağımsız, script konumu kritik).

```csharp
// Pseudo-spec — yazılmayacak, sadece şekil
[CreateAssetMenu(menuName = "RIMA/Map/Collision Rules", fileName = "CollisionRulesSO")]
public class CollisionRulesSO : ScriptableObject {
    [System.Serializable]
    public class PrefabRule {
        public string namePattern;        // "wall_*" or "statue_column_*"
        public RimaUnifiedPainterWindow.CollisionMode mode;
        public Vector2 customSize;
        public Vector2 customOffset;
        [TextArea] public string note;
    }
    public List<PrefabRule> rules = new();
    public RimaUnifiedPainterWindow.CollisionMode fallback = RimaUnifiedPainterWindow.CollisionMode.Passable;
}
```

**Resolver yeni öncelik (CollisionResolver.Resolve içinde):**

1. User per-instance `chosenMode != Auto` → kullan
2. `chosenMode == Auto` ise:
   1. `CollisionRulesSO` içinde matching pattern var mı? Glob match (`name.StartsWith(pattern[:-1])` if endswith `*`)
   2. Match yoksa: mevcut `GetDefaultCollisionMode` hardcoded keyword listesi (line 1849) — geri uyum
3. Hâlâ resolve olmadıysa: `rules.fallback` → `Passable`

**[Save Current as Rule for "{pattern}"]:**

- Disabled (`BeginDisabledGroup`) if `selectedPrefab == null || customCollisionMode == CollisionMode.Auto`
- Click: pattern auto-suggest `selectedPrefab.name` prefix (e.g., `wall_face_NS_damaged` → suggest `wall_*`)
- Confirm dialog: "Save rule: pattern=`wall_*`, mode=WallBlock to CollisionRulesSO?"
- Yes → `rules.Add(new PrefabRule { namePattern = "wall_*", mode = customCollisionMode, customSize=..., customOffset=..., note="" })` + `EditorUtility.SetDirty(rulesSO)` + `AssetDatabase.SaveAssetIfDirty`
- Cache invalidate (GetDirtyCount artar)

**[Edit Rules SO...]:** Codex v2 N8 düzeltmesi — `Selection.activeObject + Ping` lock'lu Inspector'ı edit moduna almaz. v3 davranış: `EditorUtility.OpenPropertyEditor(rulesAsset)` ile **yeni floating Property Editor** aç. Fallback: API yoksa eski (`Selection.activeObject = rulesAsset; EditorGUIUtility.PingObject(rulesAsset)`), buton label "Select Rules SO" olur. SO'nun kendi default inspector'ı (reorderable list standart). Ayrı custom editor şu an YOK.

**No-SO durumu:**

- `CollisionRulesSO` referansı `[SerializeField] private CollisionRulesSO collisionRules` painter window'da.
- Asset yoksa: panel'de "No CollisionRulesSO assigned" + `[Create Default]` butonu (`AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CollisionRulesSO>(), "Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset")`).
- Resolver SO null ise direkt fallback chain (keyword list → Passable).

**IMGUI notları:**

- `EditorGUI.BeginDisabledGroup` scoped pattern (Codex E4 onayı).
- Foldout balance: tek `BeginFoldoutHeaderGroup` → tek `EndFoldoutHeaderGroup` (Codex A3).
- `EditorGUI.indentLevel++/--` mevcut line 579-582 paired pattern korunur (Codex A4).
- Foldout state korunabilir field `showPrefabSettingsSection` (rename semantik OK, Codex D1).

---

### Panel 3: Palette Tile Redesign

**Konum:** `DrawPaletteItemButton` (line 825). Genişlik+yükseklik+label+badge değişir.

**Mockup (tek tile, 110×130 px):**

```
+------------+
|            |
|   ◼ IMG    |   ← 96×96 thumb
|            |
|            |
+------------+
| wall_face  |   ← label satır 1
| _NS_dmg    |
|     [B]    |   ← badge bottom-right
+------------+
```

**Boyutlar:**

- Tile width: 92 → **110 px**
- Tile height: 110 → **130 px**
- Thumb: 76×76 → **96×96**
- Label area: 20 → **30 px** (2 line wrap)
- Badge: bottom-right, 18×18 px, renkli kapsül

**Label wrap (Codex E1 düzeltilmiş):**

```
// IMGUI label style:
labelStyle.wordWrap = true;
labelStyle.clipping = TextClipping.Clip;     // emniyet 3. satır
labelStyle.fontSize = 9;
```

**ZWSP injection sadece display string için, kod escape ile:**

```
// Hiçbir source-literal'a invisible char yapıştırma — Codex E1 uyarısı.
string displayName = item.displayName.Replace("_", "_\u200B");
```

Bu sadece label render için lokal değişken; `item.displayName` source değişmez (asset adıyla eşleşik). ASCII-safe + explicit.

Test prefab adları wrap sonuçları:

- `wall_face_NS_damaged` → "wall_ face_ NS_ damaged" (4 break point, 2 satıra sığar)
- `mounting_torch_iron` → "mounting_ torch_ iron"
- `floor_tile_granite_a` → "floor_ tile_ granite_ a"

**Badge spec (block/passable rozeti):**

Badge çağırır: `CollisionResolver.Resolve(item.prefab, currentCategory, CollisionMode.Auto, ...)` → `effectiveMode` → renk + harf.

| Mode | Letter | Color | Tooltip |
|------|--------|-------|---------|
| Passable | P | green (#3ad17b) | "Walks through" |
| SmallFootprint | S | yellow (#e5c54a) | "Small collider (lamp, urn)" |
| FullFootprint | F | orange (#e58a3a) | "Full footprint blocker" |
| WallBlock | B | red (#d54a4a) | "Wall blocker" |
| Custom | C | magenta (#c050d0) | "Custom override" |

- Floor category: badge yok
- Mob category: badge yok (mob navigation kendi sisteminde)
- Selected tile için badge `effectiveMode` `customCollisionMode != Auto` ise live update

**IMGUI notları (Codex A2):**

- Tile **tek rect** olarak reserve edilir (`GUILayoutUtility.GetRect(width, height)` line 827 mevcut). İçeride sadece `GUI.*` ve `EditorGUI.DrawRect` — **NO `EditorGUILayout.*` calls** rect içinde (Codex A2 uyarısı, mevcut kod zaten doğru pattern).
- Badge `GUI.Label(badgeRect, new GUIContent(letter, tooltip), badgeStyle)` + `EditorGUI.DrawRect(badgeRect, color)` alt katmanda.
- Grid column count: `viewWidth = position.width - 320f`, `itemWidth = 110f`, `columns = Mathf.Max(1, Mathf.FloorToInt(viewWidth / 110f))`.

---

### Panel 4: Target Status Banner

**Konum:** `DrawHeader` (line 394) ile `EditorGUILayout.HorizontalScope` arasına yeni satır.

**Mockup:**

```
+----------------------------------------------------------------------+
| Status:  ✓ Tilemap: ShatteredKeep_TM                                |
|          ✓ Parent:  Props_Root (auto-created)            [Ping]     |
|          ⚠ Biome:   None — assets show all categories               |
+----------------------------------------------------------------------+
```

**State matrix (Codex v2 S4 düzeltilmiş — 3 ayrı eksen):**

**Eksen 1 — Tilemap satırı (her zaman görünür):**

| `targetTilemap` | Icon | Text | Color |
|-----------------|------|------|-------|
| null | ✗ | "No Target Tilemap — assign in Foldout 1" | red |
| not null | ✓ | "Tilemap: {targetTilemap.name}" | green |

**Eksen 2 — Parent satırı (sadece tilemap OK ise görünür):**

| `targetParent` field | `PeekTargetParent()` result | Icon | Text | Color |
|----------------------|----------------------------|------|------|-------|
| not null | (kullanılmaz) | ✓ | "Parent: {targetParent.name}" | green |
| null | returns existing Props_Root transform | ✓ | "Parent: Props_Root (auto, exists)" | green |
| null | returns tilemapParent (not Grid/Tilemap-ish) | ✓ | "Parent: {tilemapParent.name} (inferred)" | green |
| null | returns null (will need create on paint) | ⚠ | "Parent will auto-create Props_Root on first paint" | yellow |

**Eksen 3 — Biome satırı (her zaman görünür):**

| `activeBiome` | Icon | Text | Color |
|---------------|------|------|-------|
| null | ⚠ | "Biome: None — all assets shown" | yellow |
| not null | ✓ | "Biome: {activeBiome.name}" | green |

**Davranış (Codex B3 + F3 düzeltilmiş):**

`GetTargetParent()` (line 2250) Props_Root auto-create yapıyor — Panel 4 ping/status BU FONKSİYONU ÇAĞIRMAZ. Yerine yeni helper:

```csharp
// Pseudo-spec
private Transform PeekTargetParent() {
    // GetTargetParent ile aynı resolve logic AMA hiçbir GameObject create etmez.
    if (targetParent != null) return targetParent;
    if (targetTilemap == null) return null;
    Transform tilemapParent = targetTilemap.transform.parent;
    if (tilemapParent == null) return targetTilemap.transform;
    bool isGridOrTilemap =
        tilemapParent.GetComponent<Grid>() != null ||
        tilemapParent.GetComponent<Tilemap>() != null ||
        tilemapParent.name.ToLower().Contains("grid") ||
        tilemapParent.name.ToLower().Contains("tilemap");
    if (isGridOrTilemap) {
        GameObject existing = GameObject.Find("Props_Root");
        return existing != null ? existing.transform : null;   // YOKSA NULL DÖN — create etme
    }
    return tilemapParent;
}
```

Banner ve Panel 1 [Ping] butonu **sadece `PeekTargetParent()` kullanır**.

**Drift önleme (Codex v2 N4):** Implementation task'ında `GetTargetParent()` da refactor olur — body `Peek + create-if-null` olur:

```csharp
// Pseudo-spec
private Transform GetTargetParent() {
    Transform peeked = PeekTargetParent();
    if (peeked != null) return peeked;
    // Auto-create branch — sadece paint pipeline buradan geçer
    // (banner/Panel 1 Ping buraya ULAŞMAZ — onlar Peek kullanır)
    return CreatePropsRoot();    // GameObject.Find + new GameObject + Undo.Register
}
```

Bu sayede tek resolve mantığı, ikiye bölünmez.

- `PeekTargetParent() == null && targetParent == null` → yellow "auto-resolve on first paint"
- `PeekTargetParent() != null` → green
- `[Ping]` button disabled if peek=null (`BeginDisabledGroup`)

**Collapse:**

- `[SerializeField] bool showStatusBanner = true`. Mini `[▼]` `[▲]` toggle button. SessionState backup (Codex D2 dictionary uyarısı: bu basit bool, serialize OK).

**IMGUI notları (Codex A2):**

- Custom toolbar row: `EditorGUILayout.BeginHorizontal(EditorStyles.toolbar)` + `GUILayout.Label(icon + text)` + `GUILayout.Button("Ping", EditorStyles.toolbarButton)`.
- 3 satır için `EditorGUILayout.BeginVertical` içinde 3 `BeginHorizontal`. Mixing GUILayoutUtility.GetRect YOK.
- Icons stable: `EditorGUIUtility.IconContent("console.warnicon.sml")` / `"console.erroricon.sml"` / `"TestPassed"`.

**Eski HelpBox temizliği:**

- "Assign a Target Parent" HelpBox (line 689) **silinir** — banner zaten gösteriyor.
- "Use standard keys in Scene View" HelpBox (line 693) **kalır**, Foldout 3 "Brush & Placement Settings" altına taşınır.

---

### Panel 5: Selected Instance Editor (post-place tweak)

> **Eklenme tarihi:** 2026-05-20 S95 LATE NIGHT (user direktif — "elimle çekiştirerek gözlemime göre ayarlayabilme")
> **Tetik:** `Selection.activeGameObject` Props_Root altında ise auto-show; yoksa "No selection" placeholder

#### Mockup

```
[Selected Instance]                                     ▾
  Object: wall_face_NS_v01 (3)         [Ping] [Focus]
  Group:  Walls                        [Move to ▾]
  
  ── Collision ──────────────────────────────
  Mode: [Auto: WallBlock ▾]    (CollisionRulesSO matched: wall_*)
  Size:   [ 1.000 ] [ 0.500 ]
  Offset: [ 0.000 ] [-0.250 ]
  □ Override per-instance
  
  [ ▣ Edit Collider in Scene ]   ← scene'de drag handles aktive
  [ Reset to Auto ]
  
  ── Transform ──────────────────────────────
  Rotation: [ 0 ▾ ]  (0/90/180/270)
  Scale:    [ 1.000 ]
  
  [ Apply Changes ]   [ Delete Instance ]
```

#### Drag-handle çalışma şekli

User isteği: collider boyutunu **scene view'da gözlemleyerek elle çekiştirerek** ayarlama.

**Implementation yaklaşımı (Codex implementation Codex task'ında):**

1. **`Edit Collider in Scene` butonu** → Unity'nin native BoxCollider2D edit modunu aktive eder:
   ```csharp
   // HARD LOCK: ToolManager.SetActiveTool YASAK (Unity 6'da BoxCollider2DEditTool internal, derleme hatası verir).
   // Tek kabul edilen API:
   UnityEditorInternal.EditMode.ChangeEditMode(
       EditMode.SceneViewEditMode.Collider2D,
       collider.bounds,
       owner);
   ```
   *Antigravity Bölüm C review'inde tespit edildi (2026-05-20 S95 LATE NIGHT) — ToolManager.SetActiveTool() internal/restricted, derleme hatası riski.*
   Bu modda BoxCollider2D'nin 4 köşesi + 4 kenarında **drag handle** belirir, user fare ile çekiştirir.
2. **Canlı yansıma:** SceneView drag sırasında `collider.size` ve `collider.offset` değişir. Painter penceresi `OnInspectorUpdate` veya `EditorApplication.update` callback ile bu değerleri her frame okur, IMGUI Vector2Field'lerine yazar. User scene'de çekiştirir, sayılar pencerede canlı değişir.
3. **Undo support:** Edit moduna girerken `Undo.RecordObject(collider, "Edit Collider")`. Drag tamamlandıktan sonra Ctrl+Z geri al. Native Unity zaten yapıyor — onu kullan, custom Undo handling ekleme.
4. **Exit edit mode:** Tool'dan başkasına geçince veya başka GameObject seçilince auto-exit. Painter penceresinde de "Exit Edit" butonu (toggle state).
5. **Custom mode auto-switch:** "Edit Collider in Scene" tetiklenince Mode dropdown otomatik `Custom`'a geçer (Auto/SmallFootprint/vs ile drag yan yana mantıksız — Custom = user-driven).

**Native API'nin avantajı:** Unity zaten optimize, snap (Ctrl+drag), grid snap, multi-axis lock vs hepsi built-in. Custom Handles yazmak gereksiz.

**Spec katı sınırı:** Drag sırasında SADECE Custom mode'a override eder. Auto resolver mantığına dokunmaz. CollisionRulesSO'nun verdiği default değişmez — user "bu instance için özel" diyor.

#### Selection-driven auto-refresh

- `EditorApplication.selectionChanged` callback ile painter penceresi panel 5 içeriğini güncelle
- Multi-select desteği: 2+ instance seçili → "3 instances selected (mixed types)" header + **batch apply** ("All to WallBlock", "Reset all to Auto"). Multi-select edit edilmek istenirse spec v2 — şu an tek instance scope.
- Selection Props_Root altında değilse (örn Grid veya başka root) panel "Not editable — only Props_Root instances" mesajı.

#### Move to / Delete

- **Move to ▾:** dropdown ile başka canonical group'a taşı (örn yanlışlıkla Walls'a yerleşmiş statue'yu Statues'a aktar). `Transform.SetParent(otherGroup, worldPositionStays: true)`.
- **Delete Instance:** `Undo.DestroyObjectImmediate(instance)`. Confirmation dialog YOK (Karpathy #3 surgical, user undo ile geri alır). Tooltip: "Ctrl+Z to restore."

#### IMGUI integration notes (Codex'e)

- Panel 5 default expand state `SessionState.GetBool("RIMA.UnifiedPainter.section.selectedInstance", true)`.
- `OnSelectionChange()` window callback ile selection dirty flag, `Repaint()` çağır.
- "Edit Collider in Scene" buton state'i: ToolManager.activeToolType BoxCollider2DEditTool ise basılı görünüm.
- Multi-instance selection mass-edit Karpathy #3 surgical sınırında **dışarıda** (spec v2 backlog).

---

## Removed / Deferred

| Item | Status | Reason |
|------|--------|--------|
| Foldout 4 "Collider Boundaries" eski yeri | REPLACED by Panel 2 | Aynı yeri yeni adla doldur, state `showPrefabSettingsSection` korunur |
| Bottom HelpBox "Assign a Target Parent" | REMOVED | Banner replacement |
| Bottom HelpBox "Use standard keys" | MOVED to Foldout 3 alt | Status alanı temiz |
| Hierarchy prefix mantığı (`GetOrCreateGroupParent`) | DELEGATED to `GroupClassifier` | Tek source of truth (v2 new) |
| `ConfigureCollider` keyword logic | DELEGATED to `CollisionResolver` | Tek source of truth (v2 new) |
| `CollisionMode` enum hoist | UNCHANGED (nested) | SO aynı Editor asmdef'inde — Codex C2 onayı |
| Refactor: paint pipeline | NOT SCOPED | Sadece görsel + 1 SO + 2 helper class |
| Multi-select palette | DEFER | Karpathy #3 — out of scope |
| Group rename UI | DEFER | 6 sabit grup, custom group yok |

## Açık Sorular (kullanıcıya — v3)

1. **CollisionRulesSO konumu:** Q1 LOCK: Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset (asset konumu sabit). Painter source kod path zaten Assets/Editor/CollisionRulesSO.cs.
2. **Group lock semantiği:** Kullanıcıya tooltip ile "editor state, durable lock değil" yazılacak. Yine de göstermek istiyor musun, yoksa lock butonunu tamamen kaldıralım mı? Önerim: göster + tooltip uyarısı (workflow yardımı için yararlı).
3. **Status banner her zaman expand mi?** Default expand. Collapse SessionState'te saklı. ONAY?
4. **Badge görseli:** Letter + color + tooltip. Colorblind hardening shape (cross diagonals WallBlock için) gizmo'da var, badge'de yok (18×18 yer dar). Tooltip yeterli mi? ONAY?
5. **CollisionRulesSO migration:** Mevcut hardcoded keyword list (`GetDefaultCollisionMode` line 1849) SO yoksa fallback kalır. SO varsa SO öncelik. Bu doğru mu, yoksa SO yoksa hata mı verelim? Önerim: fallback (geri uyum + yumuşak migration).
6. **GroupClassifier + CollisionResolver helper'ları:** Bu spec uygulanırken Codex bunları yazacak (mevcut `ConfigureCollider` ve `GetOrCreateGroupParent` içleri 1 line delegation olur). Bu refactor implementation task'ında OK mi? Önerim: evet, küçük refactor + drift kapatır.
7. **CanonicalGroups extensibility:** `mob_*` prefix gelmiş, başka prefix gelirse (örn `decal_*`, `vfx_*`) `GroupClassifier`'a hardcode edilir. SO-based extensibility şu an scope dışı. ONAY?
8. **CollisionResolver caller site refactor (Codex v2 S2 + v3 Z2 scope check):** Implementation task'ında, mevcut painter'da collision resolve eden **6 noktanın** hepsi `CollisionResolver.Resolve` kullanmaya geçer:
   - `PaintPrefab` line 1453-1458 (placement)
   - `DrawPrefabOutline` line 1606-1617 (scene preview outline)
   - `ConfigureAssetPackColliders` line 1799-1801 (setup button at line 704-709 — bulk asset-pack bootstrap; **Codex v3 Z2 missing caller**)
   - `PaintWallWithConnections` line 2613-2614 (wall paint)
   - `UpdateWallConnectionsAt` line 2730-2731 (wall autoconnect)
   - Save/Load (`LoadMapData`) line 2460-2545 (collision mode serialize → deserialize)

   `ConfigureAssetPackColliders` özel: bulk operation, batch mode, SO rules de orada uygulanmalı (drift'i tamamen kapatmak için). Implementation Codex tüm 6 callsite'ı tek `CollisionResolver.Resolve` API'na bağlar. 6 caller × ~3 line delegation = ~18 line edit + 1 helper static class + 1 SO. Karpathy #3 surgical sınırında. ONAY?
9. **`OpenPropertyEditor` API:** Unity 6 / 2022 LTS'de stable. Implementation Codex bunun versiyonda olduğunu doğrulayacak. Yoksa fallback (`Selection.activeObject + Ping`) buton label "Select Rules SO" olur. ONAY?
10. **Panel 5 Selected Instance Editor:** S95 LATE NIGHT user direktif. Scope'a +1 panel + native BoxCollider2D edit tool wiring + selection-driven auto-refresh. Implementation süresi tahmini +30 dk Codex. Multi-select mass-edit v2 backlog (şu an tek instance scope). Drag handles için Unity native API (`BoxCollider2DEditTool` / `EditMode.ChangeEditMode`) kullanılır, custom Handles yazılmaz. ONAY?
11. **Panel 5 — Move to group:** Yanlış group'a düşen instance'ı dropdown ile başka canonical group'a taşıma. `Transform.SetParent` + `worldPositionStays: true`. Tek instance scope; multi-instance batch move v2. ONAY?

## Codex Re-Review v2 — Beklenen Çıktı

`STAGING/CODEX_DONE_uiux_painter_review_v2.md`:
- v1 düzeltmeleri uygulandı mı (5 madde)
- v2'de yeni risk var mı (CollisionResolver / GroupClassifier helper'ları)
- Per-panel verdict + Overall verdict
- Hedef: PASS (LIVE veya LIVE_WITH_NOTES minor)

## Codex Review Excerpts (v1 → v2 → v3)

**v1 verdict:** LIVE_WITH_NOTES
**v2 verdict:** NEEDS_REVISION (4 hard issues, 5 minor)

**v1 quotable (Codex):**
> "The redesign is implementable in Unity IMGUI, but v2 should correct three technical claims before implementation: `SetActive` is not non-destructive visibility, the collision preview cache key is too narrow, and status/ping code must not call the auto-creating `GetTargetParent()` path."

**v2 quotable (Codex):**
> "v2 fixes the main design direction, but it is not implementation-ready: SceneVisibilityManager named args are wrong, the ZWSP cleanup still contains a literal invisible character, and the proposed CollisionRulesSO asmdef placement does not match the current painter assembly."

### Düzeltme matrix v1 → v2:

| Codex v1 finding | Reference | v2 fix |
|------------------|-----------|--------|
| Panel 1 `SetActive` is destructive (E3, F3) | line 49, 55 | `SceneVisibilityManager.Hide/Show` kullan. Lock için durable claim kaldır. |
| Panel 2 cache key too narrow (B2) | line 33 | Key: + category + customSize + customOffset + rulesSO version. |
| Panel 4 Ping calls `GetTargetParent()` side effect (B3, F3) | line 34, 55 | Yeni `PeekTargetParent()` helper, create etmez. |
| Panel 3 ZWSP literal in source (E1) | line 47 | Display-only string: `name.Replace("_", "_\u200B")` runtime. |
| Gizmo colorblind hardening (E2) | line 48 | WallBlock için cross diagonals, white outline. |
| Drift across palette badge / scene gizmo / placement (F2) | line 54 | `CollisionResolver.Resolve` single source. |
| GroupClassifier duplication risk (F2) | line 54 | `GroupClassifier.CanonicalGroups` single source. |
| Foldout naming/state (D1) | line 42 | `showPrefabSettingsSection` korunur, rename semantik OK. |
| Dictionary<string,bool> non-serialized (D2) | line 43 | `SessionState.GetBool` kullan, 6 entry yeterli. |
| Editor visibility durability (E3) | line 49 | Tooltip "editor state, not durable scene lock" eklendi. |
| `OnHierarchyChange` infinite loop (B1) | line 32 | Time-based polling kaldırıldı, sadece event-driven dirty flag. |
| Banner derives from OnGUI directly (B3) | line 34 | OK, banner read-only Repaint trigger gerekmez. |
| EditorGUI.BeginDisabledGroup pattern (E4) | line 50 | Mevcut pattern kullanılır, raw `GUI.enabled` mutation YOK. |
| asmdef placement caution (C1) | line 37 | `Assets/Editor/MapDesigner/Rules/` mevcut asmdef altında. ← v3'te DÜZELTİLDİ |

### Düzeltme matrix v2 → v3:

| Codex v2 finding | v2 sec | v3 fix |
|------------------|--------|--------|
| `SceneVisibilityManager.Hide/Show` named arg `includeChildren` API'da YOK (V1) | line 9 | Tüm 3 çağrı (`Hide`, `Show`, `IsHidden`) `includeDescendants: true` named arg kullanır. `DisablePicking`/`EnablePicking` aynı. |
| ZWSP literal hâlâ doc içinde (V5) | line 24-25 | 3 ZWSP byte sequence (UTF-8 `E2 80 8B`) byte-level temizlendi, escape text `\u200B` ile değiştirildi. |
| `CollisionRulesSO` asmdef yanlış yer (V14) | line 60-61 | SO `Assets/Editor/CollisionRulesSO.cs` (predefined Editor asm, painter ile aynı). `Assets/Editor/MapDesigner/` (autoReferenced=false) HARİCİ ASM, painter type'ına referans veremez. Asset instance konumu serbest. |
| Banner state matrix kafa karıştırıcı satırlar (S4) | line 77 | 3 ayrı eksen tablosu (Tilemap / Parent / Biome), her eksen ayrı state matrix. |
| Cache `Dictionary<int>` hash collision riski (V3) | line 17 | Typed `PreviewCacheKey` struct, IEquatable + HashCode.Combine. `Dictionary<PreviewCacheKey, ResolvedCollider>`. |
| SessionState key namespace çakışma riski (N7) | line 70 | `RIMA.UnifiedPainter.groupExpand.{name}` window-unique prefix. |
| `Selection.activeObject + Ping` lock'lu Inspector'ı edit etmez (N8) | line 71 | `EditorUtility.OpenPropertyEditor(rulesAsset)` floating editor. Fallback Select+Ping. |
| `resolveReason` GC riski (N3) | line 66 | Cache miss yolunda hesapla; const/static reason string'ler prefer. |
| `GetTargetParent` drift riski (N4) | line 67 | Implementation task'ında `GetTargetParent` refactor: body = `Peek + create-if-null`. |
