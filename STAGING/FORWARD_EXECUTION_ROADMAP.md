# Forward Execution Roadmap (Opus triple-AI synthesis, S114 2026-05-28)

Amaç: Bundan sonraki işi SIRALI (bağımlılık) vs PARALEL (eşzamanlı multi-AI) olarak organize et. Faz 1 demo'ya en kısa yol.

## Bağımlılık grafiği

```
ACTION 0  commit (✅ done) → push (kullanıcı gate)
   │
   ├─ TRACK A (gameplay, KRİTİK PATH — tek gerçek seri zincir, combat içinde)
   │     A1 WeaponDatabase netleştir → A2 mount bridge → A3 graybox timing
   │       → A4 juice wire → A5 ⛔ TIMING-FREEZE (kullanıcı onay gate)
   │       → [B-anim / C-weapon-sprite / D-VFX fan out] → D3 ⛔ playtest gate
   │       → demo combat loop
   │
   ├─ TRACK B (engine tool T3-F2..F7)   ── A'dan BAĞIMSIZ, paralel
   │     F2 RuntimeAssetRegistry → F3 Tool UI/palette → F4 RuntimeColliderHandles
   │       → F5 LiveRoomReloader+FileSystemWatcher → F6 Launch button → F7 smoke
   │
   ├─ TRACK C (decor/parallax P1-P3)    ── paralel
   │     SceneView kamera hook / ParallaxRig authoring paneli / factor-to-scene link
   │
   └─ TRACK D (asset hygiene)            ── paralel
         SkillOfferUI icon wire / statue AssetCategory backfill / #41 sentez / #42 delete
```

## SIRALI (kritik path)
Sadece **Track A combat zinciri** gerçek seri: A1→A2→A3→A4→**A5 freeze gate**→B/C/D(weapon art)→**D3 playtest**. NLM-locked VFX-first kuralı (WEAPON_ANIM_VFX_PRODUCTION_LOCK L1): anim (B) ve weapon sprite (C) **A5'ten önce başlamaz** — yoksa asset churn. 3 kaynak (Codex+Colossus+Opus) hemfikir.

## PARALEL track'ler (write-disjoint, Glob-doğrulandı — ortak dosya yok)
| Track | Dosyalar | Writer |
|---|---|---|
| **A gameplay** | `Combat/HandAnchorAttach`, `Combat/BasicAttack/MeleeChainBehavior`, `Combat/CombatEventBus`, `Player/PlayerController`, `VFX/SlashArcVFX` | Sonnet write / Codex review |
| **B engine (T3)** | `Scripts/Live/**`, `Scripts/LiveTool/**`, `Editor/RoomPainter/LiveTool/**` + `RuntimeRoomManager.OnRoomLoaded` hook | Codex(algo)/Sonnet(UI) |
| **C decor** | `Background/ParallaxLayer.cs`, `Inspector/Sections/ParallaxSection.cs` | Sonnet |
| **D hygiene** | `UI/SkillOfferUI.cs` + asset-only (statue backfill, #41, #42) | Sonnet + user-manual |

## ⚠️ Çakışma riski + Codex düzeltmeleri (2026-05-28)
- **Scene save:** BLOK A (A4) ve T3-C10 ikisi de `PlayableArena_Test01.unity` wire edebilir. Mitigasyon: `LiveRoomReloader` **self-bootstrap** olsun (veya ayrı setup prefab) — combat sahnesini hiç co-edit etmesin. İki agent aynı `.unity`/`.prefab`'i AYNI ANDA KAYDETMESİN.
- **T3 doc §7.1 HATASI düzeltildi:** C10 `RuntimeRoomManager.OnRoomLoaded`'a değil → `RoomLoader.OnRoomLoaded` (static event, `Scripts/Systems/Map/RoomLoader.cs:16`) hook olur. RuntimeRoomManager sadece subscribe ediyor. Peer-add, modify değil. (T3_TOOL_FULL_DESIGN.md patch'lendi.)
- **Track A iç sahiplik (A3≠A4 collision):** A2 (HandAnchorAttach + Player.prefab), A3 (MeleeChainBehavior + BasicAttackProfile — timing/hitbox dosyaları), A4 (CombatEventBus + scene/prefab juice + dash). **A2 compile-prereq DEĞİL A3 için** — A2+A3 paralel yazılabilir AMA strict ownership şart: A3 ve A4 `BasicAttackBehaviorBase.cs`/`PlayerAttack.cs`/`SlashArcVFX` timing'e ikisi birden dokunursa çakışır.
- **Track B iç dep:** **C4 RuntimeAssetRegistry API compile-dependency** — C6 palette + C9 loader + C10 instantiate C4'e hard-blocked. C5 UI shell + C7 handles + C8 cliff hover + C11 file-watcher shell C4-stub'a karşı paralel yazılabilir. F2 (C3/C4) önce.
- **Git gerçek sayı:** 3152 değil — `--untracked-files=all` ile **834 untracked + 334 unpushed commit** (3152 default-status dir-collapse şişmesiydi). Hazard verdict aynı: paralel writer'lardan önce temiz baseline.
- **A1 WeaponDatabase adayları:** `Resources/WeaponDatabase.asset` vs `ScriptableObjects/Weapons/WeaponDatabaseSO.asset` — Sonnet netleştirir, kullanıcı Inspector-verify.

## Human gate'ler (pipeline'ı bekletir)
- **A5** combat timing freeze (kullanıcı onayı)
- **B/C PixelLab gen** (kullanıcı manuel — MCP otonom YASAK)
- **D3** integrated playtest

Track B/C/D, A5/PixelLab beklerken FILL olarak koşar (`feedback_2track` pattern).

## Track B+ — Sang Hendrix L5/L9 (T3 sonrası, demo-blocking DEĞİL)
3-AI converge sonrası eklenen 2 yeni T3 işi (detay: `T3_TOOL_FULL_DESIGN.md §0.5` + memory `project_sang_hendrix_live_editor_canonical_ref`). L1-L4,L6-L8 zaten kurulu.
- **L5 iki-katmanli persistence** (KÜÇÜK): authoring data → RoomLayoutSerializer JSON; transient runtime toggle (göster/gizle) yazılmaz; explicit Save/Apply. Sang "Control Layer" deseni. Writer: Sonnet, RoomLayoutSerializer + LiveRoomReloader'a flag.
- **L9 region-ID occlusion** (YENİ feature, ERTELENEBİLİR): oyuncu foreground/çatı layer altındayken o layer opacity fade. Writer: Sonnet + design pass. Demo'ya gerekmez — V2 polish kandidatı.

## Faz 1 demo'ya EN KISA yol
Demo, T3/parallax/hygiene'in HİÇBİRİNE ihtiyaç duymuyor — room-transition loop zaten kodda LIVE. Track A + room-transition playtest dışındaki her şeyi gate-wait fill olarak koştur.

## İlk 3 aksiyon (şimdi başlanabilir)
1. **ACTION 0** — commit (✅) + push (güvenli paralel dispatch prereq'i; agent diff'leri izole/revert için).
2. **A1** — Sonnet aktif `WeaponDatabase.asset`+prefab'i netleştir (2 aday) + dead OrientationSync/WeaponSorter; kullanıcı Inspector-verify.
3. **Paralel fill** — T3-F2 (Sonnet C3/C4 + Codex review) + SkillOfferUI icon wire (en ucuz demo-görünür kazanç) eşzamanlı; ikisi de Track A'dan disjoint.

## Process notları
- **agy invocation FIX'lendi (2026-05-28):** eski "işe yaramadı" = bash `cmd /c` MSYS path-mangling (`/c`→`C:\`). DOĞRU: PowerShell tool ile `.cmd` çağır + sonucu `AGY_DONE_<account>.md`'den oku. Kanıtlandı (Little Master araştırması). Codex de fix'lendi: `cx_dispatch` STATUS-anchor (eski substring "BLOCKED" yanlış-fail). Memory: `feedback_codex_agy_dispatch_invocation_fix`.
- Codex (`blbnooqn4`) finalize anında hâlâ koşuyordu; intra-T3 sırası zaten T3 design doc'ta tam, bloklanmadı.
