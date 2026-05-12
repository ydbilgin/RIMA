# CURRENT_STATUS
**2026-05-13 -- S63 | Game Feel Kararları LOCKED + Batch17 devam**

## Active Priorities
P0: Batch17 uretimi -- south-facing hard rule eklendi (STAGING/batch17_chibi_master_prompt.md), 12 PASS karakter pipeline'a girecek, 3 regen bekliyor (Gunslinger, Fracture Imp, Brawler physique)
P1: Faz 1.0 Game Feel impl -- #1 InputBufferService + SkillBase RegisterCoroutine helper, #2 AttackTokenManager (sirayla, #2 once enemy audit gerekiyor)
P2: Asset batch -- 14 map obj 32px (8 required: chest/barrel/lever/shrine/spike/rift + 2 hazard); tile F1 floor 16var + W1 wall 8var
P3: Vista Room Template (Karar #85) -- 3 template (cliff/balcony/rift), runtime RoomLoader sistemine eklenecek
P4: TONE_SURFACES EN-first rewrite (Karar #89) -- 7 surface EN canonical + TR localization
P5: Local audio/video pipeline -- MusicGen Large + AudioGen + F5-TTS (RTX 5080 local, placeholder assets icin)

## Open Questions
- Batch17: Gunslinger/Fracture Imp/Brawler 3 regen + crop + Create Character pipeline
- Enemy prefab audit: EnemyAI.cs vs BaseMobBehavior.cs hangi prefablarda aktif? (AttackToken oncesi gerekli)
- NLM sync: nlm login gerekiyor (auth expired), login sonrasi batch sync
- Camera shake Faz 1.0'a ne zaman girer: combat core (#1+#2) oturduktan sonra

## Session History

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

## Recent LOCKED Kararlar (son 9: #86-#91, #101-#103)

| # | Alan | Karar ozeti |
|---|------|-------------|
| #86 | Map Object Set | 6 required (chest/barrel/lever/shrine/spike/rift) + 6 decor + 2 hazard; portal+shop Faz 1.5 |
| #87 | Skill Effect AngleMode | 5 kategori (ProjectileRotated/Directional8/BeamRotated/Radial/Cone); Faz 1.0 MVP 12 effect; Directional8 limit 4 hero |
| #88 | 4 Yon + flipX | S59 KEEP; "Hades-like responsiveness"; Faz 2 trigger %30+/3h playtest |
| #89 | Game Language EN-First | Ingilizce canonical, TR localization; Architect monolog + boss dialog + lore EN dogar |
| #90 | PixelLab Batch Economy | CFSR 32px=64 cell, 64px=16 cell tek generation; esnek 1xN variant; 14 map obj ~4-6h |
| #91 | Accessibility Telegraph | Tum hazard: outline pulse + ground shake + color glow; color tek basina guvenilirlik degil |
| #101 | Game Feel Sirasi | InputBuffer+AttackToken FAZ 1.0 ONCE; CameraShake SONRA; ElementSurface FAZ 2 |
| #102 | SkillEffectDef SO | Assets/Scripts/Combat/Skills/SkillEffectDef.cs, AngleMode enum 5 mod, RIMA.Combat.Skills ns |
| #103 | Local Asset Pipeline | MusicGen Large+AudioGen+F5-TTS RTX 5080 local; placeholder muzik/SFX/VO icin |
