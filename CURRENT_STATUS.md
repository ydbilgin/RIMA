# CURRENT_STATUS

> **Session:** S114 SESSION 4 (2026-05-29) — Opus 4.8. Weapon mount kodu (workflow impl+3AI review+fix, compile-clean) + weapon size/style/batch kararları + flash-fix + dispatch fixes. **/clear pickup = "S114 SESSION 4 PICKUP" bölümü (hemen aşağıda).** | **Read first:** `.claude/PROJECT_RULES.md` + this file ONLY.
> **Geçmiş session detayı (S106→S112):** `STAGING/_archive/current_status_pre_S114_20260528.md` (tam snapshot, arşiv).

---

## 🟢 S114 — AKTİF (post-/clear pickup buradan)

**Tek cümle:** 10 commit atıldı (local baseline temiz), push BLOCKED (remote divergence — kullanıcı kararı), roadmap LIVE. Faz 1 demo combat'a odaklan. **EN GÜNCEL DURUM = aşağıdaki "S114 SESSION 3 PROGRESS".**

---

### ✅ S114 SESSION 4 PICKUP (2026-05-29, Opus 4.8) — ⭐ /CLEAR PICKUP BURADAN

**Tek cümle:** Weapon mount kodu LIVE (workflow impl + 3-AI review + fix, compile-clean 0 err, UNCOMMITTED); weapon size/style/3-batch kararları LOCKED; flash-fix + dispatch fixes DONE. Yeni session = aşağıdaki OTONOM TASK QUEUE'yu sırayla yap.

**🔓 KULLANICI YETKİSİ (kritik, ban override):** Kullanıcı Claude'a **PixelLab MCP ile silah üretme yetkisi verdi** (S114 S4). `feedback_pixellab_mcp_halt_strict` ban'ı **SADECE bu weapon-gen görevi için** kalktı, **3 batch ile SINIRLI**. Diğer MCP gen (character/animate/tile) YASAK kalır.

**Bu session DONE:**
- **Weapon mount kodu** (workflow `wpmonw5vi`: impl + Codex+agy+Opus paralel review + fix): `OrientationSync` per-dir flipY (W/NW/SW) + procedural swing (`BeginSwing`, strike-frame=attackStartup'a hizalı) + `HandAnchorAttach` combo-step trigger + slash VFX hook. 4 dosya, +363 satır, **compile 0 err (verified read_console)**. Review GERÇEK timing bug yakaladı (swing vuruştan 50-150ms geç) → düzeltildi + mid-swing facing desync + dropped-hit guard. **UNCOMMITTED.**
- **Weapon kararları LOCKED** (KB §4 + `STAGING/WEAPON_BATCH_PLAN.md`): 1 sprite/silah, 8-yön KOD (rotation+flipY+sort), PPU 64, karakter 64px. Boyut: küçük 32-40px / orta-büyük 64px. Tool=`create_1_direction_object`. ŞEMA KISITI: size+style_images birlikte VERİLEMEZ, en büyük style-img çıktı boyutunu belirler → ref'i hedef px'te hazırla. style_images=mevcut-weapon(stil/boyut)+downscale-karakter(sınıf rengi).
- **Mevcut weapon:** cyan greatsword `31ee0f73` (Warblade demo, ✅ on-brand 64px) + katana `a032d9b5`/staff `4bde2642`/dagger `9312ea86`/pistol `894bba4a`/bow `ebc33ebf`. 8dir-baked YANLIŞ format (sil).
- **flash-fix DONE** (Task Scheduler S4U, no-flash kullanıcı-verified) + **agy priority+fallback** (ydbilgin>ydbilginn>yasinderyabilgin>laurethayday>laurethgame) + **Codex cx_dispatch STATUS-anchor fix**. Memory: [[feedback-codex-agy-dispatch-invocation-fix]].
- **parallax L4:** buton eklendi AMA pre-existing CRITICAL (inspector `asset.parallaxFactor` bake'e bağlı değil — placer window-tier okuyor) → toggle runtime ETKİSİZ. Tek-kaynak kararı gerek (DEFER — demo parallax istemez).

**📋 OTONOM TASK QUEUE (yeni session, Unity AÇIK, sırayla):**
1. **T-W1 Weapon batch gen (YETKİLİ, MCP):** `STAGING/WEAPON_BATCH_PLAN.md` 3 batch'i üret. Akış: style-ref base64 hazırla (mevcut weapon + downscale karakter, hedef px) → `create_1_direction_object` → review → `get_object` → `select_object_frames`. SADECE 3 batch. Öncelik eksikler: Ravager greataxe, Elementalist staff, Summoner tome, Brawler gauntlet + swap varyantları.
2. **T-W2 Demo weapon live-test:** cyan greatsword `31ee0f73` download → Unity import (PPU 64, point/no-compress) → weapon prefab → `Resources/WeaponDatabase.asset` Warblade/Base entry → play: 8-dir flipY + swing + slash VFX doğrula + screenshot QC.
3. **T-W3 Player.prefab re-save:** yeni OrientationSync alanları (weaponRenderer, swingBackswing=45, swingFollowThrough=90, strikeFraction) Inspector'da görünür/tunable.
4. **T-W4 Tune:** handOffsets/weaponRotations/swing değerleri play mode göz ayarı (A5-feel).
5. **(DEFER):** Parallax tek-kaynak fix · statue kategorizasyon #3 · combat .cs commit (kullanıcı isterse).

**Combat .cs UNCOMMITTED** (reviewed+fixed+compile-clean) — git-recoverable, kullanıcı onayıyla commit.

---

### ✅ S114 SESSION 3 PROGRESS (2026-05-28 gece, Opus 4.8 + workflow/agy)

**Bu session locklananlar (hepsi memory'de):**
- **Routing KATMANLI** ([[feedback-sonnet-default-opus-exception]] güncellendi): Orchestrator + zor/multi-system kod + kritik review = **Opus 4.8**; mekanik bulk = Sonnet/Codex. Opus 4.8 farkları: tool-calling verimli, kod hatası 4× az kaçırma, plana itiraz, desteksiz iddia 4× az. Fast mode = HIZ oyunu (2× pahalı, tasarruf değil). Dispatch'te `model` explicit.
- **Weapon/anim CONVERGED** ([[project-weapon-anim-converged-s114]]): silah 8 yöne BAKE EDİLMEZ → weaponless body + HandAnchor child SR + OrientationSync, PPU 64. N-facing = VFX-first. agy+Claude+endüstri+memory converge (CoM postmortem = bake death spiral kanıtı).
- **Silah ÜRETİM aracı kararı:** `create_1_direction_object` batch (size≤85→16 item, `item_descriptions[]` + `style_images[]`) tüm sınıf silahları tek batch; hero greatsword için `create_image_pro` (512² max). create_object map-prop yönelimli, hero için zayıf.
- **State-anchored anim** (Karar #145 öğrenildi): mid-walk state → animate, `first_frame`+`enhance` ON. ⚠️ warblade'in HENÜZ state'i YOK (sadece 8 idle rotation, `animations: none`) → demo = 5 south state üret + her birinden anim (~25-35 gen). char id: `2656075d-d113-4f18-a6c1-94b5a6b8bf65`.
- **Demo asset locks** ([[project-demo-asset-locks-s114]]): mob seti = FractureImp + ShardWalker + HollowHulk + **PenitentSovereign (boss, sprite YOK→üret 128-192px)**. PixelLab T1~35/T2~100 KEEP, 51 DELETE. Player.prefab'a Animator EKLE.
- **PixelLab Knowledge Base LIVE:** `STAGING/PIXELLAB_KNOWLEDGE_BASE.md` ([[reference-pixellab-knowledge-base-s114]]) — tool matrix (batch yetenekleri) + state workflow + prompt grammar + Discord legal gap.
- **Dynamic workflows** ([[reference-dynamic-workflows-usage]]): Claude `Workflow` tool kullanabilir ("workflow" kelimesiyle opt-in). ultracode = oturum ayarı (kullanıcı `/effort ultracode`, Claude tetikleyemez). Bu session 2 workflow koştu.

**Scene (PlayableArena_Test01, KAYDEDİLDİ):** loose cliff sprite (14)+stray silindi · drop-shadow açık · braziers sütun ÜSTÜNE taşındı · floor **bounded ada R=14**'e trimlendi (2365→615 hücre). ⚠️ **Kullanıcı sonra cliff'leri SİLDİ** → oda rebuild bekliyor (task #2).

**🔓 AÇIK KARARLAR (kullanıcı — /clear sonrası ilk gündem):**
1. ✅ **ÇÖZÜLDÜ 2026-05-28 (kod-doğrulamalı):** Body = **8 baked directional sprite, runtime flipX YOK** (`PlayerAnimator.cs:103` flipX=false + DirX/DirY blend tree). Mirror = ÜRETİM kararı: W/SW/NW'yi PixelLab Mirror Horizontal ile bake et (mevcut karakterlerde 8 yön var → hazır). Silah `OrientationSync` 8 explicit offset ile decoupled — counter-flip gerekmez. (İlk "5+3 Unity flipX" lock'u kod ile düzeltildi.)
2. ✅ **ÇÖZÜLDÜ 2026-05-28:** interpolation_v2 canvas = **256 max** (v3); create_character size = 128 max (ayrı limit). G10 schema-doğrulandı. **→ PixelLab pipeline LOCKED.**
3. AssetPool vs RuntimeAssetRegistry statue kategorizasyon (11 boş AssetPoolSO). **← tek açık kalan.**

**📋 AÇIK TASK QUEUE:**
- **#2 Doğal oda rebuild:** organik (kare DEĞİL) tile şekli + cliff'ler tile ALTINA katmanlı (Decor_Cliff < Floor sorting) + en arkaya **KitC_BG parallax** (`Assets/Sprites/Environment/KitC_BG/` bg_L0_void→L4_fog, codex/PixelLab BG kiti) derinlik için.
- **#3 Unity asset silme:** güvenli junk audit'te HAZIR → `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/` (16, Karar#150 ihlali) · `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` (3) · `Assets/Art/_TempReferencePacks/` (2) · `Assets/Art/Characters/Warblade/south.png` (dupe) · void-dışı StoneColumn'lar. **Referans-check sonrası sil** (git-recoverable). Riskli 3 (Phase0_ScaleTest, rift_pool violet, floor_large/walls violet) + PixelLab cloud 51 = kullanıcı nod/web-UI.
- ✅ **#5 agy flash-fix ÇÖZÜLDÜ 2026-05-28 (kullanıcı no-flash teyit):** non-Unity dispatch Task Scheduler S4U (non-interactive) session'da → flash gorunecek masaustu yok. `agy_detached_runner.py` + `agy_detached.ps1` + tek-seferlik admin `Register-ScheduledTask RIMA_agy_detached`. Sonra her tetik admin'siz. Unity dispatch hâlâ `agy_dispatch.cmd` (minik flash OS limiti). Ayrıca agy priority+auto-fallback eklendi (ydbilgin>ydbilginn>yasinderyabilgin>laurethayday>laurethgame). Codex `cx_dispatch` false-fail FIXED (STATUS-anchor). Memory: [[feedback-codex-agy-dispatch-invocation-fix]].

**Discord:** scrape = ToS ihlali, YAPILMADI. Kullanıcı help-support'ta legal soru postladı (resmi bot/API/export var mı). Manuel küratörlük + docs/YouTube yolu. İzlenecek video: Character States `oCJWxfEwX-o`.

---

### ⚠️ PUSH BLOCKED — kullanıcı kararı gerek
- `origin/master` diverge: tepe `05e15540 "Initial check-in"` = **kullanıcının 23 May, 3350-file line-ending normalization** commit'i (parent `32f204b7`).
- Local: ortak atadan **19 commit ileri** (gerçek iş + bugünkü 10). Fast-forward DEĞİL.
- Seçenekler: (a) `git rebase origin/master` — line-ending conflict riski yüksek; (b) merge — aynı risk; (c) **force-push** — master'a TEHLİKELİ + "Initial check-in"i siler, SADECE kullanıcı explicit onayıyla. **Claude force-push YAPMAZ.** Kullanıcı seçecek.

### ✅ İlk dalga DONE — sonuçlar + post-clear next action
| İş | Sonuç | Next action |
|---|---|---|
| **Cliff siyah teşhisi** (`STAGING/CLIFF_BLACK_LAYER_DIAGNOSIS.md`) | KÖK NEDEN: `Decor_Cliff` sorting layer hiçbir Light2D target'ında YOK (D2'de eklendi, ışıklar önce yapılandırıldı) → 0 ışık → Lit material siyah. Layer atamaları DOĞRU. 2ndary bug: `DirectionalCliffTile.GetTileData` yön çözümü `#if UNITY_EDITOR` içinde → Play'de hep güney yüz. | **FIX (kullanıcı onayı sonrası, Sonnet+Codex review):** Light2D `m_ApplyToSortingLayers`'a Decor_Cliff(12)+Decor_Floor(13) ekle (Global+4 autolight); inactive `RimLight_*_Cyan`+`Brazier` aktive (brand cyan-rim). P2: DirectionalCliffTile `#if UNITY_EDITOR` kaldır. |
| **A1 WeaponDB** (`STAGING/A1_WEAPONDB_CLARIFY.md`) | Canonical = `Resources/WeaponDatabase.asset` (Player.prefab HandAnchorAttach.weaponDatabase). `WeaponDatabaseSO.asset` orphan→sil. `OrientationSync.Sync(FacingDir8)`=A2 mount API→**WIRE**, WeaponSorter→sil. | **A2 mount bridge** başlat: `HandAnchorAttach.cs`. ⚠️ Verify: Player.prefab `bodyRenderer` null (Level2 için ata) + canonical `handOffsets[]` boş (orphan'da değer var). |
| **SkillOfferUI icon wire** (`STAGING/D_SKILLOFFERUI_ICON_WIRE_DONE.md`) | ✅ 4 satır, 0 err. `Data/Skills/*.asset` skills icon gösterir; SkillDatabase runtime placeholder (no regression). | `SkillOfferUI.cs` **uncommitted** → sonraki commit batch'e. |

### 📋 MASTER PLAN (CANONICAL — kaybetme): `STAGING/MASTER_EXECUTION_PLAN.md`
Tüm açık işlerin tek sıralı master planı (Opus sentez + agy validate, S114). Faz 0 baseline → 1a combat → 1b art → 1c demo + paralel FILL track'leri + 3 gate (A5/D3/git-push). Session pickup'ta ÖNCE bunu oku. Companion bağımlılık grafiği: aşağıdaki roadmap.

### ✅ S114 SESSION 2 PROGRESS (2026-05-28, Sonnet impl + Opus review)
Master plan Faz 0 + Faz 1a kritik path TAMAM (autonomous, sıralı, her adım Opus review'lı):
- **Faz 0:** cliff black fix (16/16 Light2D Decor_Cliff hedefliyor + rim/brazier aktive, kök neden `RIMA_Cycle2_Dressing` parent kapalı) · DirectionalCliffTile `#if UNITY_EDITOR` kaldırıldı (runtime yön) · WeaponDatabaseSO orphan silindi (8 handOffsets `A1_WEAPONDB_CLARIFY.md §7`'ye kaydedildi).
- **A2 mount bridge:** silah ele mount + 8-dir orient (VectorToDir8 + OrientationSync), per-dir sorting, WeaponSorter silindi. ⚠️ Player.prefab *asset*'inde PlayerController yok (sahne instance'ında var; teleport-transition demo'da güvenli, prefab re-instantiate kırar).
- **A3 timing:** hit artık 80ms startup (windup) sonrası iniyor; `attackStartup` knob (A5-tunable); ApplyMeleeHit imzası korundu; PublishHit/Kill zaten wired'dı.
- **A4 juice:** `CombatJuice` GO sahneye eklendi (HitPause/ScreenShake/CameraPunch/DamageNumber/VFXRouter) — kod zaten vardı, sadece sahneye bağlı değildi. Dash → PublishDash (PlayerController.TryDash). FeelToggle default'lar ON. VFXRouter.entries boş = D placeholder.
- **FILL T3-MVP F2:** `Assets/Scripts/Live/RuntimeAssetRegistry.cs` (C4, API dondurulmuş: Get/GetSprite/GetTile/GetPrefab/GetByTag/GetByLayer/Contains) + C3 baker (menü). F1 (RoomLayoutSerializer/RoomManifestSO) zaten vardı. F3-F7 = T3-Polish (demo sonrası).
- **FILL statue:** `AssetPool_WallBlocker_Statues.asset` oluşturuldu (14 statue, cat=WallBlocker). ⚠️ Diğer 11 AssetPoolSO boş — statue'nin asıl kategorizasyon yolu (pool vs RuntimeAssetRegistry keyword/RoomLayer) belirsiz, kullanıcı doğrulaması iyi olur.
- **FILL T3-Polish F3-F7 TAMAM** (Session 2, "hepsi sırayla" otonom — Sonnet write + rima-qc review + fix loop): Live editor inşa edildi. F3 palette (`LiveToolPaletteWindow`+`RuntimeBrushPalette`+`RuntimeAssetLoader`, nullable layer-filter), F4 `RuntimeColliderHandles` (ColliderShapeSwapper reuse), F5 `Assets/Scripts/Live/` `LiveRoomReloader`+`JsonFileWatcher`+`RoomLayoutData` (self-bootstrap RoomLoader.OnRoomLoaded, `#if DEVELOPMENT_BUILD||UNITY_EDITOR`, thread-marshal), F6 `LiveToolLauncher` (Process.Start Tool+Game.exe, try/finally define-guard) + painter toolbar buton, F7 `Assets/Tests/EditMode/LiveToolSmokeTests.cs` (29/29 PASS). **DEFER:** cliff tile live-reload no-op (floor+prop reload çalışıyor; cliff GUID-reconstruction + CliffCell direction/manual field ayrı iş) · T3 spec doc §F1 schema camelCase yazıyor ama impl snake_case (impl tutarlı, doc stale).
- **Demo skeleton SCOPED (implementasyon ertelendi):** `STAGING/DEMO_SKELETON_PLAN.md`. Room-seq+gate+fragment KODU var; eksik = gate-flow wire (`useFragmentGateFlow=false`) + mob çeşitliliği + fragment drop. **3 KARAR gerek:** fragment-gated mi clear-gate mi · A5 sahnesine dokunma onayı (yoksa Demo_Skeleton.unity duplicate) · 4 mob fonksiyonel mi. Otonom motor burada durdu (A5 sahnesini + tasarımı kullanıcı onayı olmadan rewire etmemek için).
- **⛔ A5 BEKLİYOR:** kullanıcı combat feel playtest (PlayableArena_Test01). "freeze" → B/C/D art açılır; "tune" → değerler değişir.
- **Otonom run STOP noktası:** Faz 0 + A2-A4 + T3-Polish(F3-F7) + statue bitti. Kalan otonom-güvenli iş tükendi — demo skeleton (kullanıcı 3 karar) / decor-parallax #18 (underspecified, scope gerek) / #41 (pixellab-doc) / A2 hardening (min-code ihlali, atlandı). Sıra A5 verdict'inde.

### 🗺️ Roadmap: `STAGING/FORWARD_EXECUTION_ROADMAP.md`
- **Kritik path (combat, seri):** A1→A2 mount bridge→A3 graybox→A4 juice→**A5 ⛔ timing-freeze (kullanıcı gate)**→B/C/D weapon art→**D3 ⛔ playtest**→demo loop.
- **Paralel:** B=T3 live tool (F2-F7, C4 registry-first) / C=parallax+cliff / D=asset hygiene. A5/PixelLab beklerken fill.
- **Demo'ya en kısa yol:** T3/parallax/hygiene'e İHTİYAÇ YOK — room-transition loop LIVE. Track A + playtest yeter.
- **Scene-save dikkat:** A4 + T3-C10 ikisi de PlayableArena'ya dokunabilir — aynı anda SAVE etme; LiveRoomReloader self-bootstrap.

### ✅ 10 commit (local baseline, fe697247'e kadar)
dispatch-ignore+cookie guard / docs-status-lean+conflict-locks / docs-staging-locks / feat-editor-parallax-preview+painter / chore-project-sorting-layers / feat-content-camera-640×360 / chore-tools-cx_dispatch+painter-suite / chore-deps-MCP-9.7.1 / chore-tools-agy-scripts.

### Gate'ler (kullanıcı-manuel, akışı bekletir)
- **A5** combat timing-freeze · **PixelLab gen** (MCP otonom YASAK) · **D3** playtest · **Cliff fix** onayı (diagnosis sonrası).

### Carry (eski S114, hâlâ açık ama Track'lere folded)
- PixelLab sentez (#41) + cleanup (#42 delete) → Track D. Master: `PIXELLAB_INVENTORY_MASTER.md` (Tier 2, 1208 gen).
- Cliff F path manuel wire (F1 slot + F4 GO) + Unity restart compile verify + oda transitions playtest.
- **Opus animasyon flow:** sade body + HandAnchor weapon + Painterly VFX + juice telafi.

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
