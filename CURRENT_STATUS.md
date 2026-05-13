# CURRENT_STATUS
**2026-05-13 -- S65 sonu | Animation Prompts Skill-Aligned + Ambient Idle System**

## Active Priorities
P0: PixelLab animasyon uretimi (web UI) -- STAGING/pixellab_animation_prompts_10char.md HAZIR (30/30 prompt, skill-aligned). Opus final judgment pending (yeni sessionda bak: STAGING/anim_prompts_skill_review_result.md). Ronin attack_basic FIX bekleniyor (FAIL: stationary draw-slash → moving Sheath Walk slash).
P1: Unity cleanup -- eski placeholder/dead code temizligi (Codex gorevi yeni sessionda). BaseMobBehavior OnDestroy token cleanup hala bekliyor.
P2: Unity combat impl devami -- Plan+Opus ile sonraki adim (InputBuffer, AttackToken entegrasyonu).
P3: Asset batch -- 14 map obj 32px; tile F1+W1
P4: Vista Room Template (Karar #85)
P5: Local audio/video pipeline

## Open Questions
- Ronin attack_basic: Opus final kararı bekleniyor (stationary draw-slash → moving Sheath Walk)
- Bazi WARN items: Elementalist Rift Bolt el kaynagi, Ravager 3-hit zincir, Brawler jab kimliği — Opus FIX/ACCEPT karar verecek
- Batch17 4 FAIL mob regen: PixelLab web UI ile uretim (Karar #106)
- Camera shake Faz 1.0'a ne zaman: combat core oturduktan sonra

## Session History

### S65 sonu (2026-05-13) -- Animation Prompts Skill-Aligned + Ambient Idle System
- PixelLab UI kapasitesi LOCKED (Karar #108): Custom V3 4-16f, Create State 20-40 gen, Custom Frames interpolasyon workflow, keep-first-frame optional
- Animation prompts 3 pass: teknik QC (30/30) → skill alignment (4 fix: Ravager/Summoner/Hexer/Gunslinger) → skill review (21/30 PASS, 1 FAIL Ronin, 8 WARN)
- Opus final judgment TAMAMLANDI: 5 FIX / 4 ACCEPT karari
  - FIX: Ronin attack_basic (moving Sheath Walk), Ronin attack_heavy (Soken-giri fan), Elementalist attack_basic (bolt elden), Ranger attack_basic (compound bow), Ravager attack_basic (3-hit zincir)
  - ACCEPT: Warblade attack_heavy, Shadowblade attack_heavy, Brawler attack_basic, Hexer skill_cast
  - Yeni session: rima-asset ile 5 FIX prompt yazdır, sonra uret
- Ambient Idle System LOCKED (Karar #109): 10 karakter kisilik idle animasyonu, TASARIM/AMBIENT_IDLE_SYSTEM.md
- Memory: Karar #108 + #109 yazildi

### S64 sonu (2026-05-13) -- Anchor QC Final + Animation Prompts
- 10/10 karakter anchor QC PASS: tutarli low top-down ~30-35 derece, chibi, south-facing (Karar #104)
- Create Character parametreleri LOCKED (Karar #105): view=low top-down, n_directions=8, proportions=chibi
- PixelLab MCP uretim icin kullanilmayacak (reference image yok), web UI kullanilacak (Karar #106)
- Animation prompts LOCKED (Karar #107): run+attack_basic+attack_heavy/skill_cast, 10 karakter, STAGING/pixellab_animation_prompts_10char.md
- STAGING temizligi: test_dispatch/test_unitymcp/nlm_reset/batch17_chibi_master/batch17_style_reference silindi
- Sonraki session: Unity implementasyon isleri

### S64 (2026-05-13) -- Infrastructure + AttackTokenManager + Anchor QC
- NLM full reset: 2 eski notebook silindi, yeni notebook 30ddffa5 (129 kaynak)
- cx_dispatch.py 3 bug fix: wrapper kosulu, STATUS fallback write, prompt "execute" keyword
- ask_gemini model fix: gemini-3.1-pro-preview -> gemini-2.5-pro
- AttackTokenManager.cs implement edildi (Codex), compile PASS, OnDestroy fix pending
- 16 anchor QC: 12 PASS, 4 FAIL (fracture_imp/seam_crawler/plate_widow/rift_hound)
- Fail4 regen prompt: STAGING/batch17_fail4_regen_prompt.md hazir

### S63 sonu (2026-05-13) -- Game Feel + Batch17
- 3 turlu Opus/Codex/Opus analizi: 4 Game Feel sistemi degerlendirildi
- LOCKED: #1 InputBuffer+Cancel+MercifulDodge FAZ 1.0, #2 AttackToken FAZ 1.0
- ERTELENDI: #4 CameraShake (combat core sonrasi), #3 ElementSurface FAZ 2
- P3 SkillEffectDef SO: Codex tamamladi (Assets/Scripts/Combat/Skills/SkillEffectDef.cs, commit d91db65)
- Batch17: south-facing hard rule eklendi prompta; Image #3 12 PASS, 3 regen bekliyor
- RoomBuilder editor-only ama runtime sistemi tam calisir (RoomLoader + prefab pipeline)
- Local ses/video pipeline karari: MusicGen + AudioGen + F5-TTS RTX 5080'de

### S63 (2026-05-13)
- Batch16 chibi review (Image #9 + Image #10): rima-qc + Opus -> 4 BLOCKING regen (Ronin/Elementalist/Ravager/Summoner/Ranger), 5 PASS (Warblade/Shadowblade/Gunslinger/Brawler/Hexer)
- Image #11 mature ARPG batch review: NLM history + rima-sonnet analysis + Opus -> Karar #94 ACCEPT (mature 60 deg pivot) -> kullanici sorgusu sonrasi REVOKE: chibi 64x64'te ~11px kafa = yuz detayi yok, animasyon pipeline chibi-optimized
- Karar #100 LOCKED: S62 mature pivot REVOKE, chibi 64x64 + ~35 deg kamera RESTORE, dark gritty Salt and Sanctuary chibi tone KEEP, Image #12 yeni style anchor
- Korunan iyilestirmeler: #95 (Elementalist NO STAFF + floating golden rune disc + cropped top + midriff + skirt), #96 (Summoner cyan/violet ZERO green), #97 (Hexer green-flame curse staff), #98 (rift color cyan+violet), #99 (weapon-in-hand kural)
- Batch17 chibi master prompt FINAL: 10 sinif + 6 mob/boss blok, STAGING/batch17_chibi_master_prompt.md
- Alabaster Dawn polish ref dosyasi: STAGING/alabaster_dawn_polish_ref.md (drop shadow, hit-stop, dash trail, parallax fikirleri)

### S62 (2026-05-12)
- Warblade pilot prompt yazildi: Create Image S-XL New -> style reference workflow (STAGING/warblade_pilot_prompt_2026-05-12.md)
- cx profil temizligi: cop profiller silindi (model_reasoning_effort=low vb 7 profil), laurethayday yeniden login edildi
- codex_profile.ps1 fix: bilinmeyen profil adina hata ver (profil onceden cx add ile olusturulmali)
- CodexAuthManager v0.3.0 GitHub release: profile validation + README beginner improvements (npm install, Node.js link, ChatGPT Plus gereksinimleri)
- CURRENT_STATUS.md lean formata indirildi (~66 satir); sub-agent background launch feedback memory kaydedildi
- batch16_final_v3.md hazirlanidi: Global Style Block Opus MERGE karari (B-base + A reference-lock + Hades/HLD + NEGATIVE PROMPT); her varyasyona "True south, direct front view." eklendi
- Reference image workflow LOCKED: dual-blade south-facing karakter -> Create Image Pro reference image -> aciklama: "Match this camera angle and south-facing pose exactly. Do not copy colors, armor, or weapon design."
- Rift crack karar: HYBRID -- sprite'a baked minimal 1-2px hint (kimlik), skill VFX ayri katman (Karar #91 telegraph uyumlu)
- Workflow LOCKED: Create Image S-XL New (south frame) -> PixelLab Create Character -> Custom Animation v3

### S61 (2026-05-12)
- PixelLab 16-slot batch v1 uretildi + QC (PARTIAL PASS) -> v2 hazirla: south-facing guclendir, Ravager dual axe, canvas fill zorunlu
- CLASS_SILHOUETTE_BIBLE: Ravager dual short compact axes LOCKED (eski: single two-handed battleaxe)
- Pipeline temizligi: 4 eski guide arsivilendi, GEMINI.md silindi, pixellab_master_pipeline REVOKED banner; agent modeli rima-doc+rima-codex Haiku'ya indirildi; cx profil sirasi LOCKED (laurethayday -> laurethgame -> yasinderyabilgin)

### S60 (2026-05-13/14) -- 3 session
- Kararlar #77-91 LOCKED (15 karar): chibi+lore integration, open vista, mob/boss/skill expansion, game language EN-first, PixelLab batch economy, accessibility telegraph
- Codex E1+E2 Unity implementation: TileImportWizard (568 ln) + PixelPerfectCanvasPreview (115 ln) + compile fix (URP -> U2D)
- Agent Lean Migration: 5 lean agent definition, sub-agent baseline -10-25k token; MEMORY.md stub; auto-memory 5 yeni feedback

## Recent LOCKED Kararlar (son 9: #99-#107)

| # | Alan | Karar ozeti |
|---|------|-------------|
| #99 | Weapon-in-Hand Kural | Tum karakterler sol elde weapon (dual-blade/staff/gun/bow), uyun konsistensi |
| #101 | Game Feel Sirasi | InputBuffer+AttackToken FAZ 1.0 ONCE; CameraShake SONRA; ElementSurface FAZ 2 |
| #102 | SkillEffectDef SO | Assets/Scripts/Combat/Skills/SkillEffectDef.cs, AngleMode enum 5 mod, RIMA.Combat.Skills ns |
| #103 | Local Asset Pipeline | MusicGen Large+AudioGen+F5-TTS RTX 5080 local; placeholder muzik/SFX/VO icin |
| #104 | Anchor QC | 10/10 karakter anchor PASS; tutarli low top-down chibi south-facing |
| #105 | Create Character Params | view=low top-down, n_directions=8, proportions=chibi, outline=black |
| #106 | PixelLab MCP Scope | Uretim icin MCP kullanilmaz (ref image yok); web UI kullanilir |
| #107 | Animation Prompts | run+attack_basic+attack_heavy/skill_cast; STAGING/pixellab_animation_prompts_10char.md |
