# CURRENT_STATUS
> Archive: `STAGING/_archive/current_status_pre_s95.md` (S94 ve öncesi)

---

## LIVE — 2026-05-20 S95 LATE NIGHT → Design audit + production plan lock + bug tespiti

**QC Review Sonuçları (Opus + Codex):**
- BasicAttackBehaviorBase.cs → **PASS** (legacy cleanup temiz)
- MarkPulseBehavior.cs → **YANLIŞ bırakıldı** — Ravager 6 legacy call hâlâ var, bus bypass ediyor. Bir sonraki Antigravity görevi
- RimaUnifiedPainterWindow.cs → **PASS_WITH_NOTES**
  - Bug: auto-connect walls `Walls` subgrubunu bypass ediyor (direkt Props_Root'a koyuyor)
  - Bug: LoadMapData sub-grup hiyerarşisini restore etmiyor
  - Bug: `prefabScaleMultiplier` default 0.5f, Reset 1.0f'a dönüyor (tutarsız)
  - UX Öneri #1: Scene organization panel (subgroup count + rebuild button)
  - UX Öneri #2: Wall auto-connect transparency (variant preview)
  - UX Öneri #3: Map file safety (dirty state + last save info)
- PathC_BaseTest.unity → **PASS** (Props_Root identity transform, scene root)

**Sonraki Antigravity Görevi (henüz verilmedi):**
- MarkPulseBehavior.cs bus migration (6 legacy → PublishHit/PublishKill)
- RimaUnifiedPainterWindow.cs: PaintWallWithConnections → GetOrCreateGroupParent route
- LoadMapData → sub-grup restore
- prefabScaleMultiplier default/reset tutarlı yap (0.5f → 0.5f)

### Bu Session Sonu Kararlar

**Design Plan Lock:**
- `STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md` yazıldı
- Duvar: Hibrit L2a (create_isometric_tile, taban) + L2b (create_object tall sprite, yüzey)
- Corner = ayrı sprite (perspektif baked-in, rotation çalışmaz)
- Void Flame: RIMA-özgün wall-mounted ışık, Act rengi, Unity Point Light 2D bağlı
- Obje boyut standardı: 32×32 zemin/item, 64×64 mob/prop, 64×128 dikey prop
- Yıkık iç duvar piece'leri scope'a dahil

**Tespit Edilen Bug'lar (Antigravity'ye verildi):**
- BasicAttackBehaviorBase.cs: çift event (CombatEventBus + legacy juice birlikte ateşliyor)
- MarkPulseBehavior.cs: aynı pattern kontrol gerekiyor
- Prop parent sorunu: Grid(scale 1,0.5,1) altında kalan prop'lar Y'de eziliyor → Props_Root fix

**Sonraki Session (onay bekleyen):**
- `STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md` — tüm onay checklistleri orada
- Antigravity çalışmasını ANTIGRAVITY_DONE.md'ye yazacak (background)
- Onay sonrası: duvar üretim kuyruğu başlar

---

## LIVE — 2026-05-20 S95 LATE NIGHT → Design audit batch + isometric tile import + combat juice P0/P1/P2

### Bu Session Tamamlanan (2026-05-20)

**Asset Import:**
- 16 isometric floor tile indirildi (PixelLab `b340684f-552b-49e6-a281-ab360d376564`) → `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/`
- Gemini 3.5 (Antigravity) tile import + 16×10 grid paint + seam fix yaptı:
  - `Grid.cellSize=(0.94, 0.94, 1)`, `scale=(1, 0.5, 1)`, Transparency sort axis `(0, 1, -0.26)`, Sprite Atlas padding 8
  - Iso seam çözümü → `MEMORY/project_iso_seam_solution.md`

**Combat Juice (Codex implemented, ~25 satır 3 dosya):**
- ✅ P0: `CombatEventBus.PublishHit/Kill` bağlandı → `BasicAttackBehaviorBase.cs`
- ✅ P1: Dash i-frame (`SetImmune` + prior-state preserve) → `PlayerController.cs`
- ✅ P2: EnemyAI 0.35s windup + `EnemyTelegraph.SpawnCircle` → `EnemyAI.cs`
- Console: 0 error, 0 warning. Migration safety: legacy juice çağrıları korundu, cleanup ayrı task

**Design Audits (4 Opus dispatch — sadece spec/öneri, kod değil):**
- Mekanik bankası audit → 5 TAM UYGULANABILIR + 4 ESINLENME + 8 REDDET + 3 yeni primitif (M165-M167)
- Combat completeness audit → P0/P1/P2 (yukarıda uygulandı)
- Room template tasarım → 5 canonical tag layout, Faz-1: entry_chamber + pillar_arena
- Sınıf balance review → 10 sınıf + 5 kritik karar + 3 broken kombo + 5 dead combo

**Mechanic Bank güncellemesi:**
- `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\_MEKANIK_BANKASI.md` → KATEGORI 16 (M165-M167) eklendi

### Sonraki Session İçin Bekleyenler

**Onay/red bekleyen design önerileri:**
- `STAGING/CLASS_BALANCE_REVIEW_S95.md` — 10 sınıf detaylı balance + 5 üst öncelik + broken/dead combo listeleri
- `STAGING/CROSSCLASS_TIER_SPEC_S95.md` — T1-T4 mimari spec, migration path, EnemyFamilyTagTracker tasarımı (Karar #122 codification)
- `TASARIM/SUBROOM_TEMPLATES_ACT1.md` — Faz-1 entry_chamber + pillar_arena lock önerisi, diğer 3 backlog
- Mekanik öncelikleri: M73 Shadow Echo Tail (Faz 2 ⭐), M68/M82/M81/M99

**Kritik blocker (sınıf balance #1):**
- CrossClassSkillManager Karar #122 ile uyumsuz → T1-T4 mimari refactor şart, Faz 2 başlamadan
- Warblade kod skeleton **henüz yok** — Faz 1 Warblade implementation öncelikli

**Tile devamı:**
- PathC_BaseTest sahnesi: 16×10 granite floor LIVE, console temiz
- Wall asset üretimi bekliyor — Karar: `create_object`, tall sprite Hades-style (Grid dışı, world-space)
- Mevcut wall assetları (`Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/` × 5) fake-iso uyumluluk kontrolü yapılmadı

### S95 Sub-Agent Audit Sonuçları (referans)

| Sonuç | Dosya |
|---|---|
| Mekanik audit özet | `MEMORY/project_mechanic_bank_audit_s95.md` |
| Sınıf balance detay | `STAGING/CLASS_BALANCE_REVIEW_S95.md` |
| Room template | `TASARIM/SUBROOM_TEMPLATES_ACT1.md` |
| Iso seam fix | `MEMORY/project_iso_seam_solution.md` |
| Codex P0/P1/P2 diff | git diff `BasicAttackBehaviorBase.cs` + `PlayerController.cs` + `EnemyAI.cs` |

---

## LIVE — 2026-05-19 S95 LATE → Isometric Tilemap LIVE, PixelLab tile üretimi bekliyor

### Tamamlanan (bu session)
- **Isometric Tilemap LIVE** — `PathC_BaseTest.unity`
  - `m_CellLayout: 3` (Isometric), `cellSize (1, 0.5, 1)`
  - Placeholder diamond tile: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/placeholder_iso.asset`
  - 8×8 test grid çizildi, screenshot: `STAGING/codex_iso_setup_v01/iso_grid_test_v01.png`
  - 0 console error
  - Not: Karakter + wall Y-sort için ileride `IsometricZAsY` (4) gerekebilir
- **Square tilemap seam sorunu** → isometric grid ile çözüldü (diamond geometry seam'i perceptual olarak gizler)

### Sonraki action (yeni session)
1. **PixelLab → 4 isometric floor tile üret** (user yapıyor)
   - `create_tiles_pro`: `tile_type=isometric`, `tile_view_angle=90`, `tile_depth_ratio=0`, `tile_size=64`, `outline_mode=segmentation`
   - 4 variant: granite / cracked+cyan / dirt / ritual
   - Bkz: `memory/reference_pixellab_create_tiles_pro_4type.md`
2. **Tile import + 16×10 repaint** → Codex dispatch (placeholder → gerçek tile'lar)
3. **PixelLab → isometric wall tile üret**
   - Duvarlar da PixelLab'dan — background'lu sprite set olarak
   - `create_tiles_pro` isometric + thickness (tile_depth_ratio > 0) veya `create_isometric_tile` block shape
   - Karar: wall tile mi, wall sprite mi? → yeni session'da belirle
4. Karakter (Warblade) sahneye indir → isometric floor üstünde perspektif uyumu test et

---

## İsometric Mimari (S95 LOCK)

| Katman | İçerik | Durum |
|---|---|---|
| L1 Floor | PixelLab isometric tile × 4 mat × 4 var | **Üretim bekliyor** |
| L2 Walls | PixelLab isometric wall (background'lu) | **Tasarım bekliyor** |
| L3 Object overlay | PixelLab 119 PNG Act1 envanter | HAZIR |
| L4 Gameplay | Warblade + mob + VFX | HAZIR |
| L5 Collision | BoxCollider2D | Map Designer |

**Unity scene:** `Assets/Scenes/Demo/PathC_BaseTest.unity`
**Tile path:** `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/`

---

## Kilitli Kararlar (özet, detay MEMORY'de)

| Karar | Özet | Memory |
|---|---|---|
| **Isometric S95** | Floor = isometric diamond tile (view_angle 90, thickness 0). Seam sorunu çözümü. | — |
| #150 LIVE | Act-aware dungeon-inside, 3 Act (granite/bog/void), 32×22 sub-room, 5 canonical tag | `project_karar_150_fake_isometric_lock.md` |
| #149 LIVE | Combat/Elite = 3-5 sub-room sequence, fade-to-black, mirror archway | `project_karar_149_subroom_encounter_lock.md` |
| #148 | Hades floor de-emphasis (Branch D) + Transform squash y=cos(35°)=0.819 | `project_tile_angle_verdict_branch_d_e_lock.md` |
| #114 LOCK | 8-dir: 5 üret (S/SE/E/NE/N) + 3 mirror flipX | `project_camera_angle_revisit_s43.md` |

**Per-Act material:**
- Act 1 Shattered Keep: granite `#3A3D42` + cyan `#00FFCC`
- Act 2 Bleeding Wastes: bog `#3A2840` + rust `#C8502A`
- Act 3 Core Approach: void `#0A0810` + gold `#FFD700`

---

## Budget

| Kaynak | Durum |
|---|---|
| PixelLab | ~2,500 / 5,000 |
| NLM | 136 / 300 source |
| Codex | cx_dispatch.py — idle |
| Orchestrator | **Sonnet (default)**, Opus sadece cross-system design |

---

## Session başı pickup
1. `.claude/PROJECT_RULES.md`
2. Bu CURRENT_STATUS.md (tümü)
3. PixelLab tile'ları geldiyse → Codex tile import + repaint dispatch
