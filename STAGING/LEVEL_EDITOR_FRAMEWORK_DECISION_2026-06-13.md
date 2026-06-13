# LEVEL EDITOR FRAMEWORK — FEATURE & MIMARI KARARI (Council, 2026-06-13)

> Soru: RIMA Build Mode'u tasinabilir bir editor framework'une (LaurethStudio.LevelEditor) cevirirken baska ne eklenmeli + nasil mimari?
> Advisorlar: cx (feasibility/reuse) + ax Gemini 3.1 Pro (derin mimari/genre) + ax Gemini 3.5 Flash (lean/ship-fast). Ham cikti: `_process/2026-06/_council_*_editor_framework.md` + `CODEX_DONE_yekta.md`.
> Yeniden-siniflandirma: "atla" denenler "vakit kalirsa yarar mi?" lensinden gecirildi -> YAPILABILIR(opsiyonel).

## ANA KARAR: anlasmazligi ZAMAN EKSENINDE coz (soyutla-simdi vs cikar-sonra)

ax Flash "erken-soyutlama tuzagi, RIMA'ya sikica bagli yaz, arayuzleri en son cikar" dedi; ax Pro + cx "core'u sozlesmelere ayir, retrofit pahali" dedi. **Ikisi de hakli — ayni anda degil, sirayla:**

- **DEMO (~1 hafta, RIMA-ici, in-editor):** ax Flash kazanir. RIMA'ya BAGLI yaz, RESMI ARAYUZ YOK. AMA **retrofit'i pahali, ucuz-simdi 5 tohum** baked-in:
  1. Command-undo: `BuildCommandStack` ZATEN var (=ICommand-hazir) ✅
  2. Tool/Mode selector: `BuildTool{Prop,Tile}` ZATEN var (=IToolState tohumu) ✅
  3. **Custom Data slot** (her placement'a string/dict metadata) — EKLE (genre-kritik, ucuz, sonradan eklemek veriyi bozar)
  4. **Save = Newtonsoft + `schemaVersion` alani** — EKLE (JsonUtility migration'da zayif; Newtonsoft zaten projede)
  5. **Working-copy session + Apply/Discard** — working copy ZATEN var; Apply/Discard'i formalize et (ucuz, kaynak-asset kirligini onler)
- **PAKET (demo sonrasi cikarma):** ax Pro + cx kazanir. Modularite burada HAK EDILIR (2. oyun kanitlar). Sozlesmeleri cikar + altyapi sistemlerini ekle.

## AL — demo cekirdegi (simdi)
| Ozellik | Kaynak | Not |
|---|---|---|
| Asset Catalog (4 genisletilebilir kategori) + Browser (ikon+scroll+arama) | plan | Faz A |
| Odaya ekle/sil + select/move/rotate/duplicate + Object Inspector | plan/ax Pro | Faz B |
| Isik + runtime Save/Load (Newtonsoft+schemaVersion+session Apply/Discard) | plan/cx | Faz D |
| Katalog CRUD (asset ekle/sil + kategori ekle) | kullanici | Faz C (in-editor AssetDatabase) |
| **Play-test toggle** (duzenle<->oyna) | ben/Flash | En yuksek wow; kamera+pause zaten var |
| **Scatter/stamp brush** | ben/Flash | BridsonPoisson RIMA'da VAR (deterministic seed) |
| **Clipboard JSON export/import** | Flash | En ucuz persistence + Discord paylasimi (S) |
| **Ghost shadow + pivot helper** | Flash | Iso "havada mi?" + pivot kaymasi debug'ini sifirlar (S) |
| Custom Data slot · Command-undo · Tool-mode | ax Pro/cx | yukaridaki 5 tohum |
| Solvability check (IsReachableFromPlayer) | Flash/cx | RIMA'da VAR, baglanir |

## YAPILABILIR — vakit kalirsa / paket-fazi (opsiyonel)
| Ozellik | Kaynak | Deger |
|---|---|---|
| Eyedropper (damlalik) | Flash | hizli authoring |
| Reference image overlay (konsept altlik trace) | Flash | solo-dev cizim hizi (S) |
| Hierarchy/Outliner paneli | ax Pro | kalabalik sahnede secim |
| Group/Composite objeler | ax Pro | masa+sandalye birlikte |
| Volume/Region araclari (zone/box) | ax Pro | spawn/camera bounds |
| **Logic Wiring** (obje->obje bag, ID) | ax Pro | puzzle/ARPG icin guclu; core'a sadece ID-bag |
| Layers paneli | ben/ax Pro | iso-sort cogunu cozer; cok-katman lazimsa |
| Minimap+focus+zoom presetleri | ben | navigasyon |
| Coordinate/shape debug overlay | cx | portability'de grid/footprint hatasini gosterir |
| Capability manifest (genre feature negotiation) | cx | core ayni, oyun yetenegini bildirir |
| Versioned schema + migration pipeline | cx/ax Pro | paket uretime girince ILK is |
| Asset dependency lockfile + missing-asset quarantine | cx | cross-project asset referans cozumu |
| Deterministic recipe persistence (scatter=seed+param) | cx | procedural'i reusable yapar |
| Dirty-rect incremental rebuild | cx | buyuk oda performansi |
| Asset metadata validator (footprint/pivot/sorting eksik mi) | cx | her oyunun ilk problemi asset kalitesi |
| Stable extension event bus (OnPlaced/OnTileChanged...) | cx | telemetry/tutorial/achievement core-fork'suz |
| Adapter conformance test suite | cx | "adapter yazdim ama yarisi kirik" riskini keser |
| Async asset resolver (+Addressables adapter) | cx/ax Pro | Addressables kuruluysa |
| Level diff viewer · diagnostics panel · thumbnail cache · recipe-to-prefab bake | cx | team/support/scale |
| Editor theming · hotkey config · undo-history paneli | ben | paket-nicety (Flash: demo'da zaman cobu) |

## ATLA — gercekten gereksiz (vakit kalsa bile, bu olcekte)
| Ozellik | Neden |
|---|---|
| Multiplayer/collaborative editing | XL senkronizasyon/conflict/authority; karmasikligi 10x |
| Full node-based visual scripting editor | Build Mode MVP'yi yutar; recipe persistence yeter |
| In-game marketplace/mod portal | asset trust/sandbox/licensing/upload cok agir |
| Full visual scripting bridge | event bus yeterli; oyun kendi ekler |
| World-space organik terrain shader | onceki council kararı (paradigma catismasi) |
| Path/spline bezier editing | waypoint obje + Logic Wiring daha ucuz/guvenli |
| Cloud sync | once ILevelStore backend |

## POST-DEMO PAKET CIKARMA — sozlesmeler (cx+ax Pro sentezi)
core = "spatial document + asset catalog + placement commands + validation + persistence" (iso ARPG editoru DEGIL). asmdef bol: `Core` (UnityEngine.UI YOK) · `UnityRuntime` · `UnityUGUI` · `UnityEditor` · `RIMA.Adapter`.
Arayuzler (planli 5 + cx/ax Pro eklemeleri):
- `ISpaceMapper` (eski IGridSpace): CellShape, adjacency 4/8/side, footprint rotation, depth/sort projection, bounds-local indexer
- `IAssetCatalog` 2-katman: core(id/tags/preview/deps) + adapter(resolve/async-load/instantiate)
- `IPlacementValidator` plugin zinciri: core(bounds/overlap/footprint) + genre(walkability/jump-arc/path/solvability) + severity/owner/autofix
- `ILevelStore` coklu backend (user save / editor asset / streaming / cloud)
- `IPlaceable` davranis-sozlesmesi (footprint/anchors/sort/collider/tags/deps/payload)
- + `IRenderAdapter`/`ILevelPresentation`, `IInputAdapter`, `IEditorHost` (in-editor/standalone/player-build/dev-overlay)
RIMA reuse hazinesi (cx kaniti): BuildMode working copy, PropRegistry/Validator/Poisson, WalkabilityMap, RoomTemplateSO, RuntimeAssetRegistry, LiveRoomReloader, Brush/** data. **Uyari (cx):** mevcut RIMA.Runtime.asmdef oyun+UGUI+URP referanslari tasiyor; oldugu gibi pakete almak portability borcu. Resources.Load + JsonUtility + DontDestroyOnLoad singleton'lar paket-fazinda refactor ister.

## GUNCEL FAZ SIRASI
Demo: **A** (Catalog+Browser) -> **B** (select/move/delete/inspector) -> **D** (light+save/load, Newtonsoft+session) -> **C** (catalog CRUD). Ucuz wow'lar (play-test, scatter, clipboard JSON, ghost-shadow/pivot) ilgili faza serpistirilir; 5 tohum bastan baked-in.
Paket (demo sonrasi): sozlesmeleri cikar + capability manifest + conformance tests + migration + lockfile + recipe persistence + event bus + asmdef bol.

NOT (routing): cx yine `yekta` (DISABLED olmali) profilinde kostu — laurethayday/yasinderyabilgin musait degil. codex_status/kota kontrol.
