**2026-05-14 — S60 SESSION 3 SONU | 7 Yeni Karar LOCKED (#85-91) | 5 yeni TASARIM doc + Codex E1+E2 LIVE | S61 Asset Üretim Hazır**

> **S60 SESSION 3 KAPANIŞ — 2026-05-14 (otonom orchestrator + sabah kullanıcı onayı):**
>
> ### 🔒 LOCKED Kararlar (#85-91 — 7 yeni karar)
> | # | Karar | Özet |
> |---|---|---|
> | **#85** | Open-World Backdrop Language | Arena chain DNA korunur, 3-layer parallax + 3 vista room template (cliff/balcony/rift) Faz 1 zorunlu |
> | **#86** | Map Object Set (Faz 1.0 / 1.5 ayrımı) | 6 required (chest/barrel/lever/shrine/spike/rift) + 6 nice-to-have decor + 2 hazard; portal+shop Faz 1.5'e |
> | **#87** | Skill Effect AngleMode (5 kategori) | ProjectileRotated/Directional8 + BeamRotated + Radial + Cone; Faz 1.0 MVP 12 effect (sınıf×3 core), Faz 1.5 polish 16 ek; Directional8 limit 4 hero |
> | **#88** | 4 yön + flipX LOCKED + Sayısal Trigger | S59 KEEP; "Hades match" → "Hades-like responsiveness"; Faz 2 8-yön trigger %30+/3 saat playtest |
> | **#89** | Game Language EN-First Canonical | İngilizce canonical, TR localization. Architect monolog + boss dialog + lore → EN doğar |
> | **#90** | PixelLab Batch Economy | CFSR 32px=64 cell, 64px=16 cell tek generation; esnek (1×N variant veya N×1 cell); map obj 14 obje ~4-6 saat (eski tahmin 12-18h) |
> | **#91** | Accessibility Telegraph 3-Kanal Standard | Tüm hazard'larda outline pulse + ground shake + color glow; color tek başına güvenilirlik değil |
>
> ### Bu Oturumda Yapilan (S60 Session 3 — otonom + kapanış)
> 1. **5 yeni TASARIM doc** (rima-doc):
>    - `TASARIM/TONE_SURFACES_STANDARD.md` (5.2KB) — Karar #79+#89, EN-first not eklendi (tam rewrite S61'de)
>    - `TASARIM/CLASS_SILHOUETTE_BIBLE.md` (6.5KB) — Karar #80, 10 sınıf (V1 4 + V2 6 NLM teyit)
>    - `TASARIM/T2_MOB_PROTOTYPE_SPEC.md` (6.5KB) — Karar #82+#84, 3 T2 mob + 17 tuning Q
>    - `TASARIM/BOSS_PHASE2_RIFT_TEAR_SPEC.md` (5.5KB) — Penitent Sovereign Faz 2 + 6 decision + accessibility
>    - `TASARIM/BIG_DESIGN_DECISIONS_2026-05-13.md` — **7 LOCKED karar consolidation** (rima-design + Codex + Opus önerileri)
>
> 2. **MASTER_KARAR_BELGESI.md** — Karar #85-91 eklendi (7 satır)
>
> 3. **Codex E1+E2 Unity implementation** (commit `f7afc01` + benim compile fix commit `7d838b0`):
>    - TileImportWizard (568 satır) + PixelPerfectCanvasPreview (115 satır) + asmdef
>    - Compile fix: `UnityEngine.Rendering.Universal` → `UnityEngine.U2D` + Unity.2D.PixelPerfect asmdef ref
>
> 4. **🚨 Kritik düzeltme — Rift Bloom NLM teyit:**
>    - Faz 1: Rift Tear YOK (Litany of Restraint)
>    - **Faz 2: Rift Tear 3m + Rift Bloom 8s INTRODUCED**
>    - Faz 3: Echo Phantom Summon
>
> 5. **5 yeni feedback memory** (auto-memory):
>    - feedback_rima_design_no_write (judgment-only, doc → rima-doc)
>    - feedback_subagent_no_nlm_tool (orchestrator NLM yapıp inline geçmeli)
>    - feedback_nlm_skill_deprecated_nb (skill template yasak NB; manuel bash)
>    - feedback_codex_task_workflow (CODEX_TASK.md stale içerik uyarısı)
>    - **feedback_pixellab_batch_economy (Karar #90 detayı — CFSR esnekliği)**
>
> ### Sıradaki Adım (S61'de yapılacak)
>
> **PRIORITY 0 — Pipeline Pilot (Önce Warblade):**
> - Tek Warblade pilot tam üretim: 4 yön + 6-8 anim + manuel cleanup
> - Pipeline süre ölç (sprite başına gerçek dakika)
> - Pilot OK → batch 16 sprite üretim
>
> **PRIORITY 1 — Asset Batch (PixelLab Karar #90 ile):**
> - 16 sprite (10 sınıf + 6 mob, Warblade dahil) PixelLab Create Image Pro
> - 14 map obj 32px batch (8 zorunlu + 6 decor) — tek generation potential
> - Tile: F1 floor 16var + W1 wall 8var + decal 4var (32x32)
>
> **PRIORITY 2 — Vista Room Template (Karar #85 öne çekildi):**
> - Room Designer F3 vista bölümü öncelik
> - 3 vista template: cliff edge / balcony / rift opening
> - 9 parallax layer image (3 template × 3 katman)
>
> **PRIORITY 3 — Skill Effect SO (Karar #87 MVP):**
> - 5 AngleMode enum (SkillEffectDef ScriptableObject)
> - Faz 1.0 MVP: 4 sınıf × 3 core effect = 12 effect (LMB + RMB + V Burst)
> - Directional8 limit: 4 hero effect (sınıf başına 1, V Burst)
>
> **PRIORITY 4 — TONE_SURFACES EN-first rewrite (Karar #89):**
> - rima-doc dispatch: tüm 7 surface EN canonical + TR localization formatına
> - Architect monolog + boss cinematic dialog EN-first kompozisyon
>
> **PRIORITY 5 — T2 Mob + Boss Phase 2 Implementation:**
> - 3 T2 mob behavior tree + skill (Shard Walker / Penitent Bruiser / Fracture Imp)
> - Penitent Sovereign Phase 2 hazard merge (Rift Tear + Rift Bloom + Accessibility 3-kanal)
>
> ### Açık Uçlar (Faz 1+ planlamada)
> - Faz 3 Echo Phantom Summon Karar #82 T3 disabled bağlamında ne zaman? (#84 bütçe)
> - T2 mob sayısal balance placeholder'ları playtest (17 tuning Q)
> - CLASS_SILHOUETTE 5 open question (Ravager silah / Summoner staff vs totem / Hexer / V2 palette / Ronin flipX)
>
> ### Önceki S60 Session 3 Notları (otonom üretim, kullanıcı uyurken)
>
> ### Bu Oturumda Yapilan (S60 Session 3)
> 1. **4 yeni TASARIM doc** (rima-doc):
>    - `TASARIM/TONE_SURFACES_STANDARD.md` (5.2KB) — Karar #79, 7 surface × TR+EN örnek
>    - `TASARIM/CLASS_SILHOUETTE_BIBLE.md` (6.5KB) — Karar #80, V1 4 + V2 6 sınıf (Ravager/Ronin/Gunslinger/Brawler/Summoner/Hexer NLM teyit), Warblade pilot
>    - `TASARIM/T2_MOB_PROTOTYPE_SPEC.md` (6.5KB) — Karar #82+#84, 3 T2 mob spec + group comp matrix + 17 tuning question
>    - `TASARIM/BOSS_PHASE2_RIFT_TEAR_SPEC.md` (5.5KB) — STATUS KARAR ADAYI, 6 design decision + accessibility 3-kanal telegraph
>
> 2. **4 BÜYÜK Karar Adayı (#85-88) — Açık dünya kullanıcı emri:**
>    - `TASARIM/BIG_DESIGN_DECISIONS_2026-05-13.md` — rima-design judgment + Codex (yasinderyabilgin, 16k) review consolidate
>    - **#85 Açık Dünya:** Hades-vari arena chain + "open-world backdrop language" (Codex naming) + her biome 2-3 vista room template — KAPALI YAPI KORUN
>    - **#86 Map Objects:** 14 obje hedef (**8 required gameplay + 6 nice-to-have decor**, decor release blocker DEĞİL), 32x32 base + 32x64 tall, multi-tile Faz 1 YASAK
>    - **#87 Skill Effect AngleMode:** **5 kategori** (ProjectileRotated/ProjectileDirectional8/BeamRotated/Radial/Cone), Faz 1 Directional8 **limit 2-3 hero effect**
>    - **#88 4 yön vs 8 yön:** S59 4 yön + flipX LOCKED korunur (Codex AGREE), "Hades match" → "Hades-like responsiveness", Faz 2 trigger "diagonal + readability BİRLİKTE"
>    - Codex Overall: 4 karar #72/#81/#82/#83/#84 ile uyumlu. Faz 1 scope realism: **4-5 hafta full-time** (172-218h)
>
> 3. **Codex E1+E2 Unity implementation** (rima-codex agent, laurethgame profil, commit `f7afc01`):
>    - `Assets/Editor/TileImport/TileImportWizard.cs` (568 satır) — Single + Sheet (16 tile) mode, 5 validation check, auto-fix offer
>    - `Assets/Editor/TileImport/RIMA.Editor.TileImport.asmdef` (16 satır)
>    - `Assets/Editor/RoomDesigner/PixelPerfectCanvasPreview.cs` (115 satır)
>    - `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` (+54 satır surgical edit)
>    - **Compile fix orchestrator:** Codex yanlış `UnityEngine.Rendering.Universal` using yazmıştı → `UnityEngine.U2D` ile düzeltildi + asmdef'e `Unity.2D.PixelPerfect` reference eklendi. Unity console TEMIZ.
>
> 4. **🚨 KRİTİK DÜZELTME — Rift Bloom NLM teyit:** S60 Session 2 entry'sindeki "Rift Bloom DEĞİL, Rift Tear genişletilir" notu YANLIŞTI. Gerçek (NLM teyit 2026-05-13):
>    - Faz 1 (100→66% HP): Rift Tear YOK, Litany of Restraint (4 chain anchor 40 HP each)
>    - **Faz 2 (66→33% HP): Rift Tear 3m + Rift Bloom 8s cycle INTRODUCED (escalation değil, introduction)**
>    - Faz 3 (33→0% HP) "Sovereign Awakened": Rift Tear KEEP + Echo Phantom Summon (3 max)
>    - Karar #84 ihlali YOK çünkü Faz 2 spec'i zaten LOCKED bu mekanikleri içeriyor
>
> ### Sabah Kullanıcı Confirm Checklist (zorunlu)
> - [ ] **`TASARIM/BIG_DESIGN_DECISIONS_2026-05-13.md` oku** → 4 karar adayı confirm:
>   - #85 "open-world backdrop language" naming + vista room template kabul mü?
>   - #86 8 required + 6 nice-to-have ayrımı tamam mı? Multi-tile Faz 2-3 optional kabul mü?
>   - #87 5 AngleMode + Faz 1 Directional8 limit 2-3 kabul mü?
>   - #88 "Hades-like responsiveness" naming + Faz 2 trigger kabul mü?
>   - Confirm sonrası: `MASTER_KARAR_BELGESI.md`'ye Karar #85-88 LOCKED eklensin
> - [ ] **`TASARIM/BOSS_PHASE2_RIFT_TEAR_SPEC.md` oku** → STATUS KARAR ADAYI, 6 decision confirm (özellikle Decision 1 transition cooldown + Decision 4 accessibility 3-kanal)
> - [ ] **`TASARIM/T2_MOB_PROTOTYPE_SPEC.md` 17 tuning question** — placeholder sayısal değerler playtest'te belirlenecek, kabul mü?
> - [ ] **`TASARIM/CLASS_SILHOUETTE_BIBLE.md` 5 open question** (Ravager silah / Summoner staff vs totem / Hexer grimoire vs hançer / V2 palette / Ronin flipX)
> - [ ] **E1+E2 Unity'de test:** menu `RIMA → Tile Import Wizard` çalışıyor mu + Room Designer pixel-perfect overlay toggle çalışıyor mu
>
> ### Sıradaki Adım (S61'de yapılacak — confirm SONRASI)
>
> **PRIORITY 0 — Karar #85-88 LOCKED işlemi (5 dk):**
> - Confirm → MASTER_KARAR_BELGESI.md'ye 4 satır ekleme
> - CURRENT_STATUS S61 entry hazırla
>
> **PRIORITY 1 — Asset Üretim (kullanıcı yarın iş, manuel):**
> - PixelLab Web App Create Image Pro: 16 sprite (10 sınıf + 6 mob, Warblade dahil — CLASS_SILHOUETTE_BIBLE prompt hook'ları ile)
> - Tile: F1 floor 16var + W1 wall 8var + decal 4var (32x32 NEW, E1 Tile Import Wizard ile import)
> - Hibrit cleanup 5-15 dk/sprite
>
> **PRIORITY 2 — Karar #86 Map Object batch (8 required öncelik):**
> - PixelLab Create Object endpoint test (chest/barrel/lever önce — dinamik 3)
> - Aseprite cleanup → MapObjectDef SO author → Room Designer Object Brush
>
> **PRIORITY 3 — Karar #87 SkillEffect SO:**
> - 5 AngleMode enum (ProjectileRotated/Directional8/BeamRotated/Radial/Cone)
> - SkillEffectDef ScriptableObject author + per-class skill catalog draft
> - Faz 1 Directional8 list (2-3 hero effect seçimi)
>
> **PRIORITY 4 — Karar #85 vista room template:**
> - 2-3 vista room template her biome için (kırık duvar / balkon / cliff edge / rift vista)
> - Room Designer F3 ile birleşir
>
> ### Açık Uçlar (kullanıcı geri dönmeli)
> - Faz 3 Echo Phantom Summon Karar #82 T3 disabled bağlamında Faz 1 budget'ında mı?
> - Sayısal balance placeholder'ları (T2 mob telegraph, Rift Tear contact damage, Bruiser self-damage %) playtest sırası
> - 5 open class question (Ravager silah seçimi vb. CLASS_SILHOUETTE_BIBLE)
>
> ### Token Economy + Agent Sub-Lessons
> - **rima-design Write/Edit YOK** (sadece judgment) — orchestrator yanlış dispatch yaptıysa rima-doc'a route etmek lazım. Lesson: agent description scope'unu prompt yazımı öncesi doğrulamalı.
> - **NLM agent erişimi yok** (deferred listede mcp__notebooklm__notebook_query mevcut değil) — orchestrator NLM query yapıp sonucu inline geçmeli.
> - **CODEX_TASK.md stale içerik dispatch bug** — Codex automation'u dosyayı okuyor; dispatch öncesi temizlik şart.
>
> ---
>
> ## Önceki Session (S60 Session 2)
>
> **2026-05-13 — S60 ORCHESTRATOR SESSION | Kararlar #77-84 LOCKED | Agent Lean Migration | Sirada: Tile Asset + Codex E1+E2 Implementation**

> **S60 SESSION 2 ENTRY POINT (2026-05-13 19:00 — bu session sonu):**
>
> ### Bu Oturumda Yapilan (S60 2)
> 1. **Compile fix:** BrushTests.cs + RoomDesignerSkeletonTests.cs hotfix (4 satir). Unity console temiz.
> 2. **NLM:** 24 dosya batch sync + 16 orphan cleanup + 8 incremental yeni dosya sync.
> 3. **PixelLab video synthesis (rima-codex 31k):** 15 transcript + 19 gemma analiz + frame ornekleri → `STAGING/pixellab_videos_synthesis_2026-05-13.md`. 2 kritik celiski tespit (8-yon vs 4-yon, 252px vs 64px).
> 4. **pixellab_master_pipeline.md S60 OVERRIDE section** eklendi (2.5D-era kurallarini supersede ediyor).
> 5. **Chibi+Lore Integration (rima-design 65k + Codex review):** `TASARIM/chibi_lore_integration_decision_2026-05-13.md`. Kararlar #77 Vivid Vulnerability + #78 Premium Cinematic Portrait Tier + #79 Tone Surfaces Standard + #80 Class Silhouette Bible LOCKED.
> 6. **Open Vista Decision (Opus + Codex review):** `TASARIM/open_vista_decision_2026-05-13.md`. Kararlar #81 Open Vista Composition + #83 Pocket Guardrails LOCKED. Sade alternatif: Faz 1 = F1 parallax + boss backdrop only, pocket Faz 2-3.
> 7. **Mob/Boss Skill Expansion (Opus + Codex review):** `TASARIM/mob_boss_skill_expansion_2026-05-13.md`. Kararlar #82 3-Tier Skill System (STAGED) + #84 Mob T2/T3 Staged Budget LOCKED. Faz 1 = 3 T2 prototype + boss hazard merge only.
> 8. **Room Designer Top-Down Enhancement Plan:** `TASARIM/room_designer_topdown_enhancement_2026-05-13.md`. E1 (Tile Import Wizard) + E2 (Pixel-Perfect Canvas) + tile validation (Codex feedback) yeni Codex dispatch sirasi.
> 9. **Ghost Attack Revision (Karar #31):** Z/X kanal REVOKED, Shadow Echo (Karar #60) + F-slot cross-class (Karar #24) ile uyumlandirildi. MaterialPropertyBlock class tint Shadow Echo phantom layer'ina entegre.
> 10. **Lint fixes:** Karar #31 + #46 override notu, FAZ_MASTER header S60'a guncel.
> 11. **Auto-memory STUB:** `C:\Users\...\memory\MEMORY.md` 84 satir → 25 satir stub. Sub-agent baseline -3k token. Backup: `_FULL_INDEX.md`.
> 12. **🎯 AGENT LEAN MIGRATION:** RIMA_2.5D'den 5 lean agent definition copy edildi (rima-asset/design/doc/qc/research). rima-codex 164-line versiyon korundu ama `tools: Bash` frontmatter eklendi. Sub-agent spawn baseline -10 to -25k token (her tool def yok artik). Backup: `.claude/agents/_backup_2026-05-13/`.
> 13. **Codex bundled review BAŞARILI** PowerShell pipe ile (~55k token, gpt-5.5 xhigh). `STAGING/codex_s60_bundle_review_output.txt`.
>
> ### LOCKED Yeni Kararlar Bu Oturum
> | # | Konu | Detay |
> |---|------|-------|
> | #77 | Vivid Vulnerability Tonal Model | Hades+Salt and Sanctuary primary ref, HLD/Hammerwatch/Don't Starve CAUTION |
> | #78 | Premium Cinematic Portrait Tier v1 | Architect + 1 ending ONLY (max 2), NPC modal CUT |
> | #79 | Tone Surfaces Standard | death/run/codex/class/title/achievement/loading tip uniform tone |
> | #80 | Class Silhouette Bible | 10 sinif x 6-alan identity profile |
> | #81 | Open Vista Composition + Side-Pockets | 3-layer parallax + 20x40 pocket (Karar #83 guardraili) |
> | #82 | Mob 3-Tier Skill System (STAGED) | T1+T2+T3 staged Faz 1 → 2 → 3 |
> | #83 | Side-Pocket Guardrails | Sub-room only, Shop/Rest/Entry/Boss YASAK, max 2/run, etc. |
> | #84 | Mob T2/T3 Staged Budget | Normal 2 T2 / Elite 3 T2 + 1 T3 / Boss T3 disabled |
> | #31 REV | Ghost Attack → Shadow Echo + F-slot | Z/X REVOKED, mechanic Karar #60+#24'e migrate |
>
> ### Sıradaki Adım (S61'de yapılacak)
>
> **PRIORITY 0 — TILE + KARAKTER ASSET URETIMI (yarın iş):**
> 1. PixelLab Web App Create Image Pro: 16 sprite varyasyon (10 sinif + 6 mob), Warblade dahil
> 2. Tile uretimi: F1 floor 16var (32x32 NEW) + W1 wall 8var (32x32 NEW) + decal 4var (32x32 NEW)
> 3. Manuel cleanup 5-15dk per sprite (hibrit pipeline LOCKED Karar #72)
>
> **PRIORITY 1 — Codex E1+E2 Implementation:**
> - Tile Import Wizard (PixelLab PNG → Unity Tile asset, tile validation report dahil Codex R6 feedback)
> - Pixel-Perfect Canvas Preview (PPU=64 + 32x32 grid + PPC overlay)
> - Direct cx dispatch (no rima-codex spawn, ~250 satir C# tahmini)
>
> **PRIORITY 2 — Faz 1 Combat Expansion (Karar #82+84):**
> - 3 mob T2 prototype pick: Shard Walker (Crystal Bloom) + Penitent Bruiser (Self-Mortification) + Fracture Imp (Imp Tide)
> - Penitent Sovereign Phase 2 hazard merge: Rift Tear 3m + Rift Bloom 8s cycle INTRODUCED (NLM teyit S60 Session 3 — eski "Rift Bloom DEGIL" notu yanlıştı, BOSS_PHASE2_RIFT_TEAR_SPEC.md'ye bakınız)
> - Behavior tree expansion + cooldown gates
>
> **PRIORITY 3 — Open Vista Faz 1 (sade alternatif, Codex R8):**
> - F1 parallax kit (1 sky layer + 1 rift overlay) — Open Vista decision dokumanindaki sadelestirme
> - Boss arena F1 backdrop (Penitent Sovereign rift opening)
> - Pocket template Faz 2-3'e ertelendi (3 template DEGIL, Faz 1'de 0)
>
> **PRIORITY 4 — Tone Surfaces + Class Silhouette Drafts:**
> - `TASARIM/TONE_SURFACES_STANDARD.md` (Karar #79) — 7 yuzey icin tone ornekleri TR+EN
> - `TASARIM/CLASS_SILHOUETTE_BIBLE.md` (Karar #80) — Warblade pilot tam profil + 9 sinif iskelet
>
> ### Acik Ucu Kalan (Kullanici Geri Donmeli)
> - **RIMA_2.5D yapı** (`F:\Antigravity Projeler\2d roguelite\RIMA_2.5D\`): tam salvage hazir (Scripts/Combat/Core/CrossClass/Editor/Enemies/Rendering/Skills/Systems/UI + Sprites/Anchors/Bodies/Weapons + Billboard.cs + POC scenes). S59'a göre arşivlenmeli ama yapı tutuluyor — gelecekte 2.5D revisit için referans.
> - **AGENTS.md, RULES.md sync** (bu session edit edilmedi, NLM'de hala kalmiş olabilir)
>
> ---
>
> ## Önceki S60 Entry (oturum 1)
>
> **2026-05-13 — S60 | Tum Kararlar FORCE LOCKED | Visual=Into Samomor | Unity 2D Hazir | Sirada: Mob/Boss/Skill Yenileme + 16 Sprite Batch**

> **S60 ENTRY POINT (yeni session ilk okuyacagi — CLEAR sonrasi handoff):**
>
> ### Onceki Session (S59) Tamamlanan
> 1. **2.5D mimari REVOKED** → Pure 2D top-down + URP 2D Renderer + Pixel Perfect Camera LOCKED
> 2. **64x64 chibi karakter silahli 1-piece LOCKED** (body-only anchor + WeaponAnchorMap REVOKED)
> 3. **Boyut hiyerarsisi 2^n + PPU=64 standardize LOCKED** (eski PPU=32 REVOKED)
> 4. **Visual canonical pin: Into Samomor (Sang Hendrix RPG Maker MZ)** — Cinderia referansi YANLISTI (Antigravity tespit), Into Samomor: kamera 35° + 1-piece sprite + 4 yon + engine-side VFX + palette HEX'leri (#4A4A4A/#2A2E35/#1A2B1A env + #FFB000/#FFF000 neon + #00FFCC Rift Cyan)
> 5. **Hibrit AI+manual pipeline LOCKED** — PixelLab AI uretim + manuel cleanup (5-15 dk per sprite, esnek)
> 6. **Unity setup:** com.unity.2d.pixel-perfect 5.1.1 kuruldu, Main Camera'ya PixelPerfectCamera component eklendi (Ref 320x180, PPU=64, PillarboxAndLetterbox, PixelSnapping), _IsoGame.unity save edildi
> 7. **Lint fix batch:** 6 auto-memory dosyasi REVOKED banner, MASTER_KARAR_BELGESI #72-76 yeni karar, FAZ_MASTER S59 row, room_designer_plan revert notu
> 8. **NLM komple sil + baştan yükle:** 200 source fresh upload + 5 değişen dosya re-sync (199 toplam)
> 9. **Hotfix:** RoomMetadataPanel.cs `using UnityEditor.UIElements;` eklendi (compile error fix)
> 10. **Memory locks:** project_production_force_lock_2026-05-12.md + project_visual_canonical_pin_2026-05-12.md + project_64px_armed_character_locked.md + project_boss_size_hierarchy_2026-05-12.md
> 11. **Sub-agent strategy:** feedback_subagent_inline_context.md — reasoning agent dispatch'lerinde inline tam baglam + "DO NOT read memory" hint (auto-memory ~25-50k overhead minimize)
>
> ### Sıradaki Adım (S60'da yapılacak)
>
> **PRIORITY 0 — Mob/Boss/Skill Tasarım Yenileme (kullanici emri 2026-05-12):**
> "moblar bosslar skillerini yap last epoch gibi oyunlardan esinlenebilirsin ama rimaya özgü olmalı. sadece büyük olması yetmez skilleri olmalı bi şey summonlaması bi yerlerden bi şeyler çıkarıp vurması gibi gibi bir sürü varyasyon. rima-design ile konuş codexle review ettir en son rima-design'a karar verdirt"
>
> **NLM bağlam dosyaları HAZIR (STAGING'de):**
> - `STAGING/nlm_mob_design_2026-05-12.md` — mob design son LOCKED kararlar (16 mob listesi, role'ler, armor variant)
> - `STAGING/nlm_boss_design_2026-05-12.md` — boss design (Penitent/Echo Twin/Fracture Sovereign/Architect, faz/posture/skill/Fracture Echoes)
> - `STAGING/nlm_skill_system_2026-05-12.md` — skill system v2 (10 sınıf, 4 aktif tip, 3 pasif tip, V Burst, Cross-class, Shadow Echo)
>
> **Workflow:**
> 1. rima-design dispatch (INLINE TAM BAĞLAM, auto-memory ignore hint) — Last Epoch + Hades esinli, RIMA özgün, mob/boss skill variety + summon + environment etkileşim
> 2. Codex review (cx.cmd direkt Bash, sub-agent değil) — mantık, balance, conflict check
> 3. rima-design final karar (Codex feedback ile revize)
> 4. Asset batch prompt güncelle (`STAGING/batch_15_sprites_prompt_2026-05-12.md` → 16 sprite Warblade dahil, yeni mob/skill bilgileri ile)
> 5. PixelLab Web App: Create Image Pro 16 varyasyon (10 sınıf + 6 mob, kullanıcı tek seferde)
>
> **PRIORITY 1 — Compile error fix:**
> - `Assets/Tests/EditMode/Editor/BrushTests.cs` — FakeContext interface eski (IRoomDesignerContext.ActiveBlueprint + IsWallOverrideMode implement etmiyor). 1-3 satır hotfix yeterli.
>
> **PRIORITY 2 — Asset Üretim Sonrası:**
> - Pilot sınıf seç (16'dan) → PixelLab Web App Create from Reference v3 (8 yön otomatik) → Custom Animation V3 (idle/walk/attack/dash/hurt/death)
> - Unity _IsoGame.unity'ye placeholder GameObject + Warblade sprite test
>
> ---
>
> ## Önceki Pivot (S59)
>
> **2026-05-12 — S59 | 2.5D DETOUR TERKEDILDI | Pure 2D Top-Down + 64px Chibi LOCKED | Eski RIMA projeye geri donus**

> **S59 PIVOT OZETI (yeni session ilk okuyacagi):**
> - **2.5D mimari FAILED** — RIMA_2.5D nested proje (3D env + billboard) terkedildi. Sebepler:
>   - 128px detayli karakter PixelLab tutarliligi kotu (yuksek AI variance)
>   - KayKit chibi 3D = AD aesthetic'e uymuyor, Reallusion alternatifi pahali workflow
>   - 3 sistem aynia anda (3D env + 2D sprite + ikisinin uyumlu render) solo dev'e fazla
>   - Procedural oda generation 3D'de cok zor
> - **Pure 2D Top-Down secildi** — Hammerwatch + Hyper Light Drifter mantigi
>   - Karakter 64x64 chibi, PixelLab Create Character (Pixen) sweet spot
>   - Tile 32x32, klasik top-down grid
>   - VFX 64-128px mix (karakter < VFX boyutu = juice)
>   - URP 2D Renderer + Pixel Perfect Camera + 2D Lights = atmosfer
>   - 4 yon (N/S/E/W, W=flipX) MVP, 8 yon sonra
> - **Eski RIMA projesine donus** — bu klasor (`F:\Antigravity Projeler\2d roguelite\RIMA\`)
>   - Tum infra burada: Scripts/Player + Combat + Skills + Enemies + Map + Systems
>   - `_IsoGame.unity` + `_Sandbox.unity` haz
>   - TASARIM/ + MEMORY/ tam karar tarihcesi
>   - Room Designer F2 COMPLETED
> - **Karar onaylari:** Codex + Sonnet bagimsiz onayli (64px chibi + pure 2D top-down + mix VFX)
> - **Siradaki:** CURRENT_STATUS LOCKED tablosu update + lint + NLM tutarsizlik tarama -> first 64px Warblade in PixelLab -> _Sandbox.unity test

> **REVOKED LOCKED Kararlar (S57-S58):**
> | Eski Karar | Sebep |
> |---|---|
> | Render mimarisi: 2.5D (3D env + 2D Billboard) | Cozum yerine 3 problem yarattI, AI variance + tooling complexity |
> | Unity proje: Yeni URP 3D, mevcut arsiv | Mevcut RIMA proje matur, 1 ay kazanc |
> | Tilemap KALDIRILACAK | 2D Tilemap = ana sistem RESTORE |
> | Dungeon mimari: 2.5D kare grid arena | 2D Tilemap kare grid arena |
> | Body-only anchor 128px (V1 ChatGPT 30deg ref) | 64px chibi PixelLab Create Character native |
> | KayKit + Blender pipeline (RIMA_2.5D) | Test edildi, chibi proporsiyon AD'ye uymadi |
> | RIMA_2.5D nested proje (`F:\...\RIMA\RIMA_2.5D\`) | Arsivlenecek (`_ARCHIVE/RIMA_2.5D_attempt_2026-05-11/`) |

> **YENI LOCKED Kararlar (S59):**
> | Alan | Karar |
> |---|---|
> | Mimari | **Pure 2D Top-Down (Hammerwatch/HLD ref) — 2026-05-12 LOCKED** |
> | Karakter sprite | **64x64 chibi, PixelLab Create Character (Pixen) — 2026-05-12 LOCKED** |
> | Tile sprite | **32x32 top-down grid — 2026-05-12 LOCKED** (eski 64x64 floor + 64x128 wall iso REVOKE) |
> | VFX boyut | **64-128px mix (kucuk vfx 64-80, ultimate 96-128) — 2026-05-12 LOCKED** |
> | Anim yon (MVP) | **4 yon (N/S/E), W=flipX — 8 yon sonra — 2026-05-12 LOCKED** (eski karar KEEP) |
> | Anim fps | **10-12 fps (pixel art ideal) — 2026-05-12 LOCKED** |
> | Renderer | **URP 2D Renderer + Pixel Perfect Camera + 2D Lights — 2026-05-12 LOCKED** |
> | Texture filter | **Point/Nearest, no compression, no mipmap — 2026-05-12 LOCKED** |
> | PPU | **64 (sprite boyutu = PPU) — 2026-05-12 LOCKED** |
> | Anim view | **High top-down ~30-35deg Hades match — KEEP from S57** |
> | Pixel art only, Blender REJECTED | **KEEP from S56** |
> | UI icon | **32x32 veya 64x64 — 2026-05-12 LOCKED** |
> | Karakter silah entegrasyonu | **Silahli 1-piece, sinif silah sabit, PixelLab Create Character native 8-yon — 2026-05-12 LOCKED (silah anchor REVOKED)** |
> | Mob/boss boyut hiyerarsisi (2^n) | **64=char+kucuk/orta mob, 128=elite mob+miniboss, 256=act boss+final boss. Tum sprite PPU=64 standardize. — 2026-05-12 LOCKED** |
> | Final Boss boyut | **256x256 canvas + PPU=64 (sahnede ~2.5x oyuncu, Hades benchmark) — 2026-05-12 LOCKED, eski PPU=32 REVOKE** |
> | Boss mekanik (faz sayisi) | **NOT_LOCKED yet — design sprint bekliyor (4 faz vs 3 faz vs adaptive)** |
> | **Visual canonical pin** | **Into Samomor (Sang Hendrix, RPG Maker MZ, EL CIZIMI) TOP 1 referansi: kamera 35° + 1-piece sprite + 4 yon + engine-side VFX + palette mat env (#4A4A4A/#2A2E35/#1A2B1A) + neon accent (#FFB000/#FFF000). HLD palette+atmosphere TOP 2. Hero Siege scale TOP 3. Cinderia referansi YANLISTI (Antigravity tespit, baska oyun) — 2026-05-12 LOCKED REVIZE** |
> | **Production pipeline** | **Hibrit AI+manual ZORUNLU — PixelLab raw → Aseprite/Photoshop manuel cleanup (silhouette+outline+padding) → Unity import. Saf MCP-only YETERSIZ. — 2026-05-12 LOCKED** |

> **Workflow Pivot Notlari:**
> - PixelLab subscription (5000 gen) tamamen kullanilir: tile + character + mob + vfx + icon hepsi
> - Blender / KayKit / Reallusion 3D pipeline GEREKSIZ — referans icin tutulabilir
> - Eski 30deg ChatGPT init image ankor calismasi (`Characters/anchors/anchors/`) artik MVP icin gereksiz (V1'de 128px planlanmisti, simdi 64px PixelLab native uretim)
> - Existing `Assets/Scripts/Player/PlayerController.cs` + `CameraFollow.cs` zaten 2D, kullanilabilir

---

**2026-05-11 — S58 BASLADI | Unity URP 3D proje acildi, POC GATE asamasinda | Aktif Sprint: Faz 1-2**

> **S58 ENTRY POINT (yeni session ilk okuyacagi):**
> - Yeni Unity proje **RIMA/RIMA_2.5D/** (nested, intentional) — Universal 3D template, Unity 6.3 LTS
> - UnityMCP yeni instance'a baglanmasi gerek: `set_active_instance` ile RIMA_2.5D pin et
> - Eski proje `pre-2.5d-transition` git tag'li (commit 9ff3f10) — recovery point
> - Devam: TASARIM/USER_WORKFLOW_GUIDE.md Faz 0.2 (Salvage Manifest)
> - Tum karar tree LOCKED (2.5D + weapon + V1 pipeline + face cleanup + NEW>PRO + tool guide canonical)
> - Pending: POC GATE adimlari 0.2 → 0.7

---

**2026-05-11 — S57 | 2.5D Mimari Gecis Kararlari Kilitlendi | Aktif Sprint: Faz 1-2**

> **S56 bu session ozet:**
> - **Room Designer F2 TAM BITTI (QC PASS):** BiomeType, RoomBlueprint F2 alanlari, FloorVariantPainter (domain-warp Perlin bake+preview), WallAutoConnect (4-bit mask 8 tip), RoomMetadataPanel (Reseed/Preview/Override toggle), StampBrush+EraserBrush wall hook, ActiveBlueprint live instance, IsWallOverrideMode, SaveCurrentRoom -> MetadataPanel wiring. 15 commit, tum QC PASS.
> - **Pre-rendered 2D karari LOCKED:** Blender/Hades tarzi REJECTED v1; PixelLab pixel art kalir. TASARIM/PRERENDERED_2D_DECISION.md
> - **Commits:** ee365a2 -> 65f8a74 (15 commit)

> **S57 bu session ozet:**
> - **2.5D Mimari LOCKED:** 3D cevre + 2D billboard, yeni Unity URP 3D proje, salvage manifest
> - **6 Mimari Karar LOCKED (Opus):** Proje stratejisi, silah sistemi, 4 yon+flip, body-only anchor, WeaponController, Room Designer ITileWriter
> - **Silah Anchor Sistemi LOCKED (Opus):** body-only "empty hands gripping", WeaponAnchorMap, PPU=64, senkron normalizedTime
> - **Lint:** 7 cakisma tespit edildi, doc guncelleme tamamlandi

> **Siradaki session (S58):**
> 1. 2.5D POC — Yeni Unity URP 3D proje ac, 1 oda + Warblade south anchor billboard + lighting test
> 2. Salvage manifest — Assets/Scripts/ hangileri tasiniyor, hangileri yeniden yaziliyor
> 3. Body-only anchor uretimi — 4 class x 4 yon = 16 anchor (PixelLab Web App)

---

## Acik Isler (Oncelik Sirasina Gore)

### Yuksek Oncelik

0. **2.5D POC** (ONCELIK 0 — hicbir sey bu olmadan baslamiyor)
   - Yeni Unity URP 3D proje (3D template, URP)
   - 1 3D oda (Plane floor + Cube walls), Warblade south anchor billboard, point light
   - Pixel-perfect + lighting + billboard rendering dogrulama
   - PASS olmadan: Room Designer dokunulmaz, anim uretimi baslamaz

1. **Tile uretimi** (Room Designer F2 done -- tile'lar olmadan test yapilamiyor) (2.5D sonrasi 3D texture pipeline'ina tasiniyor)
   - F1 floor: 16 variant (Create Tiles Pro, URETIM_REHBERI.md)
   - W1 wall: 8 variant (Create Tile Isometric) -- WallAutoConnect'i calistiracak
   - Obstacles: Pillar once (style anchor)

2. **Room Designer F3** (F2 DONE -- tile bekliyor) (2.5D port bekliyor — POC sonrasi)
   - AI panel + MCP bridge (STAGING/mcp_requests/ -> responses/)
   - T10 Object Library, T11 Template Wizard, T12 RoomSaver export

### Orta Oncelik

4. **Warblade anim uretimi** -- ANIMASYON_URETIM.md hazir, PIXELLAB_OUTPUTS/warblade/ referans
   - Eraser Pass -> Idle -> Hurt -> Death -> Walk -> Attack LMB/RMB -> Dash -> Weapon Pass

5. **Tile uretimi** -- URETIM_REHBERI.md hazir
   - F1 floor (16 var, Create Tiles Pro) -> F2 -> F3 -> Trans
   - W1 wall (8 variant, Create Tile Isometric)
   - Obstacles (Pillar first -> style anchor)

### Dusuk Oncelik / Backlog

- Lore rework (STORY_RUN_PROGRESSION, HUB_DESIGN_v1, 3-ending cutscene)
- Map Fragment + DungeonGraph -- spec hazir
- MAKEUP_BACKLOG 8 eksiklik -- polish
- Cinematic Layer A/B/C/D -- Faz 2-5

---

## LOCKED Kararlar Ozeti (referans)

| Alan | Karar |
|---|---|
| **Mob infighting** | **Hayir. Penitent Bruiser aura faction-blind (%50 heal azaltma, 3m) — 2026-05-11 LOCKED** |
| **Terrain hazard** | **Var — F1 rift catlagi / F2 coken zemin / F3 lav+rift — hikayeye uygun — 2026-05-11 LOCKED** |
| **Room peek** | **Sadece harita parcasiyla VEYA cleared oda — 2026-05-11 LOCKED** |
| **Hub practice** | **Skill test + dummy, Hades'ten farkli — 2026-05-11 LOCKED** |
| **Karakter secimi** | **Her run degistirilebilir, heat per-character (STS mantigi) — 2026-05-11 LOCKED** |
| **Wall tile variety** | **Rule Tile hybrid (auto-connect + manual override) -- 2026-05-11 LOCKED** |
| **Floor tile variety** | **Domain-warped Perlin 3-katman, edit-time bake, template-fixed -- 2026-05-11 LOCKED** |
| **Tile kenar invariance** | **3px border = mortar #1A1C20 only, accent merkeze -- 2026-05-11 LOCKED** |
| **Room Designer vizyon** | **MapForge -- genel arac, isometric P0, topdown/sidescroller template -- 2026-05-11** |
| **Pre-rendered 2D** | **Blender/pre-render REJECTED v1; PixelLab pixel art LOCKED -- 2026-05-11** |
| **Render mimarisi** | ~~2.5D: 3D cevre + 2D Billboard~~ **REVOKED 2026-05-12 (S59)** → Pure 2D Top-Down + URP 2D Renderer + Pixel Perfect Camera. Tilemap = ana sistem. |
| **Unity proje** | ~~Yeni URP 3D proje, mevcut arsiv~~ **REVOKED 2026-05-12 (S59)** → Mevcut RIMA proje kullanilir, RIMA_2.5D nested arsivlenir. |
| **Silah sistemi** | ~~Ayrik 2D sprite child + WeaponAnchorMap + AnimationCurve senkron, body-only anim~~ **REVOKED 2026-05-12 (S59) — 64px chibi'de AI variance yuksek, pixel precise anchor imkansiz; PixelLab Create Character native silahli 1-piece uretiyor.** |
| **Karakter silah entegrasyonu** | **Silahli 1-piece — sinif silah sabit, karakter spriteinin parcasi, ayri attach yok. PixelLab create_character 8-yon native. — 2026-05-12 LOCKED** |
| **Anim yonler** | **4 yon + yatay flip (8 yon REVOKE). N/S/E uretilir, W=flipX. — 2026-05-11 LOCKED (S59 KEEP)** |
| **Body-only anchor** | ~~Yeni 16 anchor (4 class x 4 yon), 128px~~ **SUPERSEDED 2026-05-12 (S59)** → 64x64 chibi, PixelLab Create Character native (4 yon). |
| **Room Designer** | ~~ITileWriter → 3D prefab output~~ **REVOKED 2026-05-12 (S59)** → 2D Tilemap output (eski Room Designer F2 yapisi RESTORE). |
| **FloorVariantPainter params** | **warpFreq=0.05, zoneFreq=0.05, warpStrength=4.0; tiers base/accent/hero -- 2026-05-11 LOCKED** |
| **WallAutoConnect variants** | **8 tip: straight_H/V, corner_NW/NE/SW/SE, end_L/R; 4-bit NSEW mask -- 2026-05-11 LOCKED** |
| **PIXELLAB klasor** | **PIXELLAB_OUTPUTS/ (kalici) -- STAGING/PIXELLAB kaldirildi -- 2026-05-11** |
| Map editor | Custom Unity EditorWindow (RIMA Room Designer) -- 2026-05-10 LOCKED |
| Concept art stili | Pixel art ZORUNLU, painterly YASAK -- anchor metadata.json referans |
| PixelLab MCP yonetimi | Sonnet dogrudan cagri, Codex pre-review + post-QC -- 2026-05-10 |
| Tile chromakey | #00FF00, filter G>200 AND R<60 AND B<60, binary alpha snap |
| Duvar boyutu (PixelLab) | ~~64x128 isometric~~ **REVOKED 2026-05-12 (S59)** → 32x32 top-down wall tile |
| Zemin boyutu | ~~64x64, 16 var~~ **REVOKED 2026-05-12 (S59)** → 32x32 top-down floor tile, varyasyon sayisi pipeline ile |
| Anim view | High top-down (~30-35 deg, Hades match) — **S59 KEEP** |
| Anim yonler | 4 yon + flip (N/S/E uretilir, W=flipX) — 2026-05-11 (S59 KEEP) |
| v1 sprint siniflari | Warblade / Ranger / Shadowblade / Elementalist (kalan 6 -> v2) |
| Dungeon mimari | ~~Prefab-per-room, 2.5D kare grid arena~~ **REVOKED 2026-05-12 (S59)** → 2D Tilemap kare grid arena, prefab-per-room korunur |
| Karakter sprite boyutu (S59) | 64x64 chibi top-down — 2026-05-12 LOCKED |
| Tile sprite boyutu (S59) | 32x32 top-down — 2026-05-12 LOCKED |
| VFX sprite boyutu (S59) | 64-128px mix (small 64-80, large 96-128) — 2026-05-12 LOCKED |
| Skill keybind | LMB/RMB + Q/E/R/F + V(ult) + Space(dash) |
| Shadow Echo | 3 katman (aura+phantom+UI flash), cyan #00FFCC, 50 havuz |
| Act 1 map | 15 node procedural (topoloji sabit, icerik random) |
| Boss posture | Faz1=700 / Faz2=850 / Faz3=1000 |
| Final Boss canavar | ~~252-256px + PPU=32 (Faz 4 = 96px insan formu)~~ **REVOKED 2026-05-12 (S59) — yeni: 256x256 native sprite + PPU=64, sahnede ~2.5x oyuncu (Hades final boss benchmark). Tum sprite PPU=64 standardize, boyut sprite canvas ile gelir. Faz 4 boyut degisimi REVOKE (mekanik/VFX ile yapilir).** |

---

## Mimari Referanslar

- **2.5D mimari karar:** `MEMORY/project_2.5d_architecture.md`
- **Silah anchor sistemi:** `MEMORY/project_weapon_anchor_system.md`
- **Room Designer plan:** `MEMORY/project_room_designer_plan.md`
- **Tile variety sistemi:** `MEMORY/project_tile_variety_system.md`
- **Domain warp:** `MEMORY/project_domain_warp_tile_system.md`
- **MapForge vizyon:** `MEMORY/project_map_tool_vision.md`
- **PixelLab uretim:** `PIXELLAB_OUTPUTS/` (floors/ walls/ obstacles/ warblade/ ranger/ shadowblade/ elementalist/)
- **Uretim rehberi:** `STAGING/PIXELLAB/URETIM_REHBERI.md`
- **PixelLab MCP vs Manual:** `MEMORY/project_pixellab_mcp_vs_manual.md`
- **Visual identity bible:** `MEMORY/project_rima_visual_identity.md`
- **PixelLab playbook:** `PIXELLAB_OUTPUTS/PRODUCTION_PLAYBOOK.md`
- **Animation Bible:** `TASARIM/ANIMATION_BIBLE.md`
- **Skill System v2:** `TASARIM/SKILL_SYSTEM_v2.md`
- **Shadow Echo:** `TASARIM/SHADOW_ECHO_MATRIX.md`
- **Act 1 map:** `TASARIM/dungeon_act1_map.md`
- **Damage calc:** `TASARIM/DAMAGE_CALCULATION.md`
- **Mob rules:** `TASARIM/MOB_COMPOSITION_RULES.md`
- **Art pipeline:** `GUIDES/RIMA_MASTER_ART_PIPELINE.md`
- **Anchor karakterleri:** `Characters/anchors/<class>/metadata.json` (10 sinif)
