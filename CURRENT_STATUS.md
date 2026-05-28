# CURRENT_STATUS

> **Session:** S113 KAPANIŞ → S114 pickup (2026-05-28, MOLA) | **Read first:** `.claude/PROJECT_RULES.md` + this file ONLY.
> **Geçmiş session detayı (S106→S112):** `STAGING/_archive/current_status_pre_S114_20260528.md` (tam snapshot, arşiv).

---

## 🟡 S114 PICKUP

**Tek cümle:** S113'te 26 task otonom rotation ile tamamlandı. 3-AI PixelLab analizi (Opus/Codex/agy) ÜÇÜ DE BİTTİ — sentez (Task #41) + cleanup (Task #42) bekliyor.

### 3-AI PixelLab analiz çıktıları (hepsi DONE)
| Agent | Output | Durum |
|---|---|---|
| Opus | `STAGING/PIXELLAB_ANALYSIS_OPUS.md` | ✅ DONE (33 T1-KEEP / 113 T2-KEEP / 51 DELETE / 22 REVIEW) |
| Codex | `STAGING/PIXELLAB_ANALYSIS_CODEX.md` | ✅ DONE (cross-check + 10-class weapon spec + 6 açık soru) |
| agy | `STAGING/PIXELLAB_ANALYSIS_AGY.md` | ✅ DONE (10KB — status'ün eski "HUNG" notu geçersiz) |

### Pickup sırası
1. **Unity restart + compile verify** — `refresh_unity scope=all mode=force` + `read_console` → 0 err beklenir. F2/F5 final fix Unity kapalıyken yazıldı, doğrulanmadı.
2. **3-AI sentez (Task #41)** — Sonnet bg dispatch, 3 verdict birleştir → `STAGING/PIXELLAB_INVENTORY_WEAPON_SPEC_MASTER.md`. Sonra **cleanup (Task #42)** — kullanıcı onayı sonrası `mcp__pixellab__delete_object` batch.
3. **Cliff F path manuel wire** — `CliffClusterRules_Default` → `CliffAutoPlacer.clusterRules` (F1); `CliffEdgeDustEmitter` GO oluştur + `cliffPlacer`/`settings` wire (F4); PlayMode 5-component visual verify.
4. **Oda transitions playtest** — Room1 → 3 FractureImp kill → Fragment → G → Draft → Gate → fade → Room2..5 → PenitentSovereign %50 HP → DemoComplete.
5. **A1 PixelLab greatsword (kullanıcı manuel)** — `441bccf0` longsword değerlendir; yetersizse 128×256 yeni gen. Plan: `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md` §2.3.
6. **PlayerAttack.cs:142 NullRef** investigate (düşük öncelik, BasicAttackProfile null S111 carry).

### Kullanıcı kararı bekleyen sorular (3-AI analizden)
- **skill_icons:** 20 violet cloud icon recolor mı recreate mi? → Codex: **cloud import GEREKSIZ** — 19 local `Assets/Sprites/UI/Icons` (64×64) + 7 `Assets/Data/Skills/*.asset` (icon GUID'li) zaten var; tek eksik `SkillOfferUI.cs:289-291` placeholder Image → `skill.icon` wire (`BuildSkillCard` imzası icon almıyor). **Doğrulandı.**
- 4 untagged weapon hangi class'a bağlı?
- Mob roster onay (FractureImp / ShardWalker / HollowHulk / PenitentSovereign).
- Weapon PPU **64** (HandAnchor offset uyumlu) mu **100** (char/UI gibi) mu? Hexer **whip** mi **grimoire/totem/scepter** mı?
- statue 12 prefab `AssetCategory` backfill gap (D2) — Sonnet fix dispatch.
- 18 vs 17 weapon mismatch (master tabloda 1 review object eksik).

### Sentez bağlamı
- PixelLab **Tier 2, 1208 gen kalan.** 243 cloud obje. Master: `STAGING/PIXELLAB_INVENTORY_MASTER.md`.
- **Opus animasyon flow kararı:** Hibrit = sade body + HandAnchor weapon + Painterly VFX layer (cyan hitspark + dash trail). Sade body trade-off'u juice (hit pause + shake + freeze) ile telafi.

---

## 🎮 Referans-oyun araştırması (2026-05-28, Codex + Antigravity)

- **Blades of Mirage** (`STAGING/BLADES_OF_MIRAGE_PIPELINE_REPORT.md`): Gerçek-zamanlı 3D isometric ARPG. **RIMA 2D/2.5D KAL — 3D'ye pivot ETME.** Sadece ödünç al: isometric okunabilirlik, net silhouette, biome palet kimliği, su/VFX disiplini. (Antigravity'nin "Unreal/GAS" iddiası doğrulanmadı.)
- **Colossus - Eternal Blight** (`STAGING/COLOSSUS_ETERNAL_BLIGHT_RIMA_WEAPON_REPORT.md`): 2D pixel ARPG, RIMA ölçeğine çok yakın. **"VFX-first weapon → sonra attached sprite"**, silahı her yöne bake ETME, **2 rhythm (quick/heavy) > class switching**, **Blight corruption = power-at-cost roguelite hook.** HandAnchor lock + Opus hibrit kararıyla **3 bağımsız kaynak aynı yöne** işaret ediyor.
- RIMA çıkarımları memory'de: [[project-reference-games-weapon-combat-takeaways]].

---

## 🔒 Çözülen çelişkiler — canonical lock (2026-05-28, NLM tespit + kullanıcı onayı)

NLM 7 çelişki tespit etti, kullanıcı tek tek onayladı. Eski doc'lara SUPERSEDED banner eklendi.

| # | Konu | CANONICAL | Eski (bannerlı) |
|---|---|---|---|
| 1 | Parallax factor | **0.05–1.10** 6-katman (`F3_PARALLAX_6LAYER_DONE`) | 0.03–0.14 (`BG_LAYER_ARCHITECTURE_VERDICT`) |
| 2 | Weapon PPU | **64** (body uyumlu, `WEAPON_ANIM_VFX_PRODUCTION_LOCK`) | 100 (`WEAPONLESS_ANIM_..._PLAN`) |
| 3 | Asset layer | **6-layer L1-L6** (`d2_layer_arch_lock`) | 4/5 (`RIMA_LIVE_TOOL_DECISION`, T3 banner'lı) |
| 4 | Kamera | **High Top-Down 3/4 ~70-80°** (iso-art OK, iso-MATH değil). **Zoom LOCKED: PixelPerfectCamera refResolution 640×360 + upscaleRT ON + pixelSnapping OFF** (assetsPPU 64, ~%17.8 hero scale, 1080p=3x/2K=4x/4K=6x integer). pixelSnapping OFF kritik — painterly VFX/shake jitter önler (multi-res araştırma doğruladı). orthographicSize'a dokunma — PPC override eder. Ref: `STAGING/CAMERA_ZOOM_RECOMMENDATION.md` + `STAGING/MULTI_RESOLUTION_SCALING_RESEARCH.md` | 1280×720 (çok geniş) / diamond-iso terminoloji (`ROOM_DESIGN_PHILOSOPHY` 04-30) |
| 5 | Live tool | **T3 full standalone** (`T3_TOOL_FULL_DESIGN`) | T2 (`RIMA_LIVE_TOOL_DECISION`, banner'lı) |
| 6 | Character canvas | **64px içerik / 120px canvas** (animasyon headroom, "64 olarak düşün") | 64-only / 252→128 crop |
| 7 | Hexer silah | **Grimoire / Cursed Totem / Scepter** (`weapon_master_spec_10_class`) | "Whip" (agy AI hatası, not'landı) |

**Ders:** NLM recency'de %100 güvenilir değil — #6 canvas'ta eski guide'ı current gösterdi, PROJECT_RULES (05-24) ile cross-check düzeltti.

---

## ✅ S113 KAPANIŞ özet

**22 task tamam** (4 design + 8 impl + 5 review + 5 fix iter). Detay: arşiv snapshot.

**LIVE özellikler:**
- **Painter unification D2-D5.5:** `RimaRoomPainterWindow` 4 mode tab + L1-L6 filter + Prefab Mode collider drag-handle (`ColliderShapeSwapper`) + `DirectionalCliffTile` + `DecorCliffPainter` (Shift+Click).
- **Cliff F path FINAL (F1-F7):** `AdaptiveClusterFilter` (283→128) + drop shadow + 6-katman parallax + dust particle + face idle anim + culling.
- **Oda transitions LIVE:** `RoomLoader.LoadNext` + 5 `RoomSequenceData` SO + Y offset teleport + `RoomTransitionFX` fade + `DemoCompleteOverlay`.
- **T3-F1:** JSON schema 1.0 + `RoomLayoutSerializer` + `RoomManifestSO.schemaVersion` + `StreamingAssets/live/`.
- **Animation catalog:** 11 anim + 6 Apex state, weaponless (`STAGING/ANIMATION_PROMPT_CATALOG.md`).

### Locked decisions
| Karar | Lock | Ref |
|---|---|---|
| Live tool tier | **T3 full standalone** | `STAGING/RIMA_LIVE_TOOL_DECISION.md` |
| Asset layer count | **6-layer** (L1 Floor / L2 Cliff base / L3 Cliff face decor / L4 Walkable decor / L5 Wall blocker / L6 Gameplay) | D2 LIVE |
| Mounting pivot | **Top-center** | D2 |
| Phase order | **Hybrid** (cliff Fix 0 → layer arch) | D2 |
| Collider workflow | **Option A** (Prefab Mode) | D4 |
| Save format | **JSON** default | D6 |
| Migration scope | **Phase 1 critical ~30 prefab** | D2 |

### Aktif HARD rules (S112-S113, detay auto-memory'de)
- `feedback_autonomous_no_block` — otonom akış, kritik soruda sor ama durdurma
- `feedback_code_writer_rotation` — yazan ≠ reviewer rotation
- `feedback_triple_ai_inside_subagent_synthesis` — triple-AI subagent içinde, sentez orchestrator'a döner
- `feedback_codex_agy_profile_race` — Codex + agy ayrı profile zorunlu
- `feedback_sonnet_default_opus_exception` — Sonnet DEFAULT, Opus sadece 2+ system deep judgment + reviewer
- `feedback_legacy_script_kinematic_override` — physics debug ilk adım `grep "rb.bodyType"`

---

## ⚙️ Sonraki büyük scope (kullanıcı onayı sonrası)
- **T3-F2..F7** (~5-7 gün, ~1130 LOC) — `STAGING/T3_TOOL_FULL_DESIGN.md` (509 satır spec).
- **Animation production B2-B7** (PixelLab Web UI manuel) — `STAGING/ANIMATION_PROMPT_CATALOG.md`. Cost: 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen per dir. Phase 1 ucuz başla (Idle 4f=1 gen).
- **Weapon Block A2-D3** — `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md`.
