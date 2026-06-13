# CURRENT_STATUS

## ⏯️ RESUME (2026-06-13 — sunum ~1 HAFTA sonra (~20 Haz), UNITY IN-EDITOR Play Mode; yeni session devralıyor)

> ⚠️ TIMELINE DÜZELTME: Demo "bugün" değil — sunum ~1 hafta sonra, **Unity Editor'de Play Mode** ile. `#if DEMO_BUILD…` gating in-editor'da sorun DEĞİL (UNITY_EDITOR tanımlı). Zaman var → Build Mode'u sırayla inşa ediyoruz.

**⚠️ ROUTING:** Orchestrator=Fable/Opus 4.8 · **execute=Claude Opus sub-agent** (Agent tool model:opus, ax Opus DEĞİL) · **review=cx** (writer≠reviewer, quota-aware profil) · **council=cx+ax Gemini 3.1 Pro+ax Gemini 3.5 Flash**. Unity'ye dokunan her dispatch'e UNITY ERROR CHECK satırı ZORUNLU. E1-E8 aktif. (Detay memory: `opus-executes-cx-reviews`, `unitymcp-error-check-in-dispatch`.)

**🎯 DEMO = BİTİRME BÜTÜNLEME TESLİMİ.** 3 ana doküman:
- **`STAGING/DEMO_SUNUM_PLANI_2026-06-13.md`** = sunum ezberi (run-of-show + risk/fallback + F1 panel). Koreografi: stat beat'i LMB ile · Ravager'ı DPS demosunda kullanma · prop demosu reset'ten önce · sunum öncesi konsolu temizle · müzik YOK.
- **`STAGING/BITIRME_DEMO_RAPORU_2026-06-13.md`** = hocaya sunulacak proje raporu (öğrenci dili, agentic AI dengeli). Yeni commit'lerle güncellenecek.
- **`STAGING/GECE_RAPORU_2026-06-13.md`** = master gece raporu (tüm iş tablosu).

**✅ BU GECE PUSHLANAN (5 commit, hepsi cx-review'lı):**
- `435e9eeb` Quick Reset · DealDamageRaw (balans birebir) · TelemetryClock pause donması · riftcrack→Resources
- `523ca242` SetPlayerActive (Director/ölümde saldırı kapalı) · merkezi PublishKillIfDead (skill-kill juice)
- `af4b1879` 3-lens tam oyun audit (combat 8 · state 15 · binding 6; tüm 🔴 fix) · /lint · DEMO_SUNUM_PLANI
- `9ca74693` CharacterJuice (idle bob/tilt/lunge) · IsoSorter sortReference · dual-class Play-Mode kanıtı
- `91780dc4` Director ekleri: **Dual-Class Draft butonu** + **Stat preset** (Tank/Glass/Default) [cx FAIL→fix→PASS]

**📊 DURUM:** 9/9 vaat çalışıyor. Vaat #8 dual-class UNCERTAIN→KANITLANDI (Play-Mode). EditMode baseline 541/11 fail (gece işlerinden YENİ fail yok).

**🔄 YENİ SESSION İLK İŞ:** E2E playtest arka planda koşuyordu (Opus agent) → raporu OKU: `STAGING/_process/2026-06/_e2e_playtest_2026-06-13.md`. (Yoksa/yarımsa yeniden dispatch et — 10 adımlı demo akışı doğrulaması, task `_process/2026-06/_opus_e2e_playtest.md`.) Bulduğu 🔴 varsa demo öncesi fix.

**🏗️ BUILD MODE (yeni ANA İŞ — oyun-içi runtime map editörü, sunum öncesi):** Spec=`STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md` (ultracode R&D çıktısı). Hibrit tasarım, **~%70 zaten `DirectorMode.cs`'te var**. Konumlandırma: sanghendrix'in "canlı oyun üstünde düzenleme"si + RIMA ekstraları (prosedürel scatter, composition role, footprint+walkability/solvability doğrulama). **🔴 TILE KANUNU:** grid=Isometric (fake-iso/3-4 staggered, floor451_0=64×64 kare, kamera düz), yerleştirme HER ZAMAN Grid API (`WorldToCell`/`GetCellCenterWorld`/`SetTile`), ASLA dikdörtgen matematik (spec §3.5). **Sıra:** ✅P1 (kamera-zoom giriş `"` tuşu) → ✅P2 (prop palette+iso ghost+rotate+erase+undo, **prop tam hücre merkezine oturuyor runtime-kanıtlı**) → **P3 = BASİT hücre-otoriter tile/walkability/overlay brush (SIRADA)** → P4 light+runtime save/load → P5 selection/move. Her faz: builder-opus → auditor-opus/cx review → Unity runtime-doğrula. Commit'ler: 70bf3ab7(lighting) fbc21be9(P1) bcf7b9a2(P2).
**🎨 TERRAIN COUNCIL KARARI** (`STAGING/BUILDMODE_TERRAIN_DECISION_2026-06-13.md`, oybirliği cx+ax Pro+ax Flash): world-space-texture/organik firça terrain = **DEMO İÇİN ATLA** (paradigma çatışması, pixel-art kırılır, görsel/lojik kopar, perf motivasyonu RIMA'da geçersiz, Act1 keskin-hat ister). **P3 organik DEĞİL, basit grid `SetTile` brush.** Terrain shader = POST-DEMO: RIMA'da **dormant TerrainBlend altyapısı VAR** (`Shaders/TerrainBlend.shader`+`TerrainBlendRenderer`+`MapLayerOrchestrator.useShaderBlend`) → sıfırdan değil uyandırarak, decorative underlay (lojik grid'e dokunmaz). DEFER MINOR: scene-reload stale buildCamera. NOT: cx yekta profilinde koştu (DISABLED olmalı) — codex_status kontrol. Demo tool durumu: 7 tool E2E-kanıtlı çalışıyor (in-editor); **Light placement YOK** (P4'te eklenecek).

**⏭️ KUYRUK (demo-koruma önce):**
1. E2E raporu işle → kritik kırık varsa Opus fix + cx review
2. Smoke test süiti: gece data-proof'larını KALICI teste çevir (DealDamageRaw/TelemetryClock/PublishKillIfDead/SetPlayerActive/preset → EditMode `RIMA_SmokeTests`) + cx review
3. (Vakit kalırsa, KOZMETİK) Silah-ele-oturtma: `attachMode=Level1Static`→Level2, `spriteHandData` BOŞ→8 yön doldur, silah pivot DOĞRU. Silah TEK sprite (8 yön=rotation+flip+sort). ROLLBACK=enum geri. Level1 demoda "kabul edilebilir".
4. NLM temizliği `/nlm-sync --cleanup-dry`→K4 (limit penceresi sonrası)
5. Sabah paketi: GECE_RAPORU + hoca raporu final + 5-dk görsel kontrol listesi + push

**📋 SABAH KULLANICI (5 dk):** bob hissi (Inspector `enableJuice` kapatılabilir) · yeni 2 buton (Dual-Class Draft + presets) · konsol temizle · sunum planını ezberle.

**🔁 KRONİK:** Blueprint asset×6 + TMP fallback Play Mode'da kirleniyor → her commit ÖNCESİ `git checkout --` ile revert (kök fix=TMP static-atlas, DEMO SONRASI).

**⏳ DEFER (demo sonrası):** Ravager LMB merkezi yol · debugGlobalDamageMult çıplak-yol kapsamı · Weakened/Scorch yetenek çarpanı · DamageZone çok-hedef tick · mob'lara CharacterJuice · PixelLab idle (anchor SONRASI) · backlog 13 · superpowers skill graft analizi · memory index-dışı ~20 dosya · LaurethStudio.

---
*Önceki bloklar git history'de.*
