# CURRENT_STATUS

## ⏯️ RESUME (2026-06-18 AKŞAM — demo-hardening, /clear handoff) — demo ~yarın, EDİTÖRDE

**Durum:** Demo-hardening maratonu. Rapor demo-hazır (önceki commit'ler). Bu oturumda: skill-fix'ler + editör-stall çözümü + chamber skill-deneme + bug-avı + demo-polish. **2 bug-hunt workflow ARKADA KOŞUYOR** — /clear sonrası bildirimle dönecek, plan aşağıda.

### 🟢 ARKADA KOŞAN (clear sonrası bildirimle gelir — KAYBOLMAZ)
- **Deep-systems-logic hunt** `w47oqbov3` — combat-kuralı / AI-boss / state-progression-economy mantığı. **HÂLÂ KOŞUYOR** → bildirim gelince confirmed-list'i aşağıdaki bug listesine EKLE.
- ✅ Game-logic hunt `wvkdr15rt` BİTTİ (15→7 confirmed, aşağıdaki listede). ✅ Architectural hunt BİTTİ (3 confirmed). ✅ Demo-polish council 3/3 BİTTİ (sentez aşağıda).
- Read-only (Unity'ye dokunmaz). Bildirim gelince **bu RESUME'u oku + PLAN'ı uygula.**

### 📋 İKİ HUNT DÖNÜNCE PLAN (yeni session)
1. Gelen confirmed bug'ları + **aşağıdaki architectural-3** + **polish-sentezi** birleştir.
2. Önceliklendir: **ROOT-FIX önce** (SkillBase veto → tüm "ölü buton" skilleri tek yerde) → demo-etkili bug'lar → Tier-1 polish.
3. **TEK Unity-batch**, serial (tek-Unity-ajan), builder-opus. Stall mitigasyonu: kısa Play, FrameRateGuard aktif, execute_code data-proof tercih.
4. auditor-opus gate (özellikle ChamberSelectBootstrap/SkillBase/DraftManager demo-kritik) → selective commit.

### 🐞 CONFIRMED BUG (architectural 3 + game-logic 7, deduped — KAYBOLMASIN)
**🔴 HIGH / demo-killer:**
- **MOVEMENT OFF-MAP (GERÇEK odalarda!)** — Blink (`Blink.cs:32-66`: ölü "Wall" raycast + hard teleport, WalkabilityMap check YOK) + Iron Charge (`IronCharge.cs:53`) + Blade Rush (`BladeRush.cs:40`) raw velocity, clamp YOK → void'e ışınlanma/strand, irrecoverable. **Fix (ROOT):** hepsini `WalkabilityMap.ClampVelocityToWalkable`/`IsDashableWorld`'den geçir (PlayerController.TryDash + KnockbackReceiver zaten kullanıyor). Chamber guard SADECE chamber; gerçek run korumasız.
- **🔑 SkillBase spend-before-veto** (`SkillBase.cs:72-87`) — Execute no-op'sa mana+cd boşa = ölü buton (Chain Lightning menzilde düşman yokken). **Fix:** `CanExecute()` veto + cost/cd SADECE başarıda.
- **Yetersiz-kaynak = SESSİZ no-op** (Gravity Cleave Q 25 rage / run 0 rage başlar; tüm skiller — ses/flash/mesaj YOK). **Fix:** failed-cast feedback (toast/SFX/flash).
**🟡 MED:**
- **healMultiplier KALICI bozulma** — Penitent boss AntiHealAura × Warblade Crippling Blow concurrent save/restore race (boss path). **Fix:** save/restore stack-guard.
- **Director dup-slot** (`DraftManager.AssignActive:740-750`) — zaten-equipped skill 2. slota → aynı component, shared cd. **Fix:** bind öncesi diğer slotu temizle.
**🟢 LOW:**
- **Arcane Blast escalating mana off-by-one** (önceki cast'in maliyetini alıyor, finisher undercharged).
- **Evasion "%100 dodge" yine 1 hasar** (incomingMult=0 ama Health her vuruşu ≥1'e floor'luyor).
> ⏳ **Deep-logic hunt (`w47oqbov3`) dönünce confirmed'leri buraya EKLE**, sonra PLAN'ı uygula.

### 🎨 DEMO-POLISH SENTEZİ (council 3/3 — 0 PixelLab gen; dosyalar `STAGING/_process/2026-06/_council_demo_polish_*.md`)
- **Tier-1 (yap):** URP Bloom+Color Grading (Global Volume, en ucuz büyük sıçrama) · Dinamik 2D ışık SkillVfx'e (global kıs) · düşman hit-flash beyaz + combat hit-okunabilirliği · **Build Mode centerpiece cilası** (sakin grid+tool-state+placement) · HUD lerp HP-bar+toast ease + **low-HP/Rage kırmızı-ekran de-stack** (glitch).
- **Tier-2 (mevcudu tune, YENİ kod yok):** hit-stop/shake/camera-zoom/damage-numbers zaten var (`HitPauseDriver`/`ScreenShakeDriver`/`DamagePopup`). ⚠️ hit-stop SADECE HitPauseDriver'la (yeni `timeScale=0` YOK — donma riski).

### ✅ BU OTURUMDA COMMIT (master)
`f4bcc9ad` 8-yön anim · `8a03c756` Elementalist VFX+ArcaneBlast+telemetri · `9dc1116c` F3 log overlay+enstrümantasyon+pasif-toast · `2784089b` pedestal-lock+husk-fallback · `8118c90f` rapor council-fix+DOCX · `708cd810` process · `859237d7` LMB-VFX+chamber-loadout+FrameRateGuard · `eb3a16cd` **timeScale donma-fix** · `9568d85b` status · `d6e6ce0f` **chamber skill-bar+off-map-guard** (gerçek-test).

### 🖥️ EDİTÖR STABİLİTE (demo EDİTÖRDE) — DEMO CHECKLIST
- Hang sebepleri çözüldü: **timeScale stuck-0** (HitStop×HitPauseDriver dual-owner → `eb3a16cd`) + **uncapped FPS** (RTX 5080 D3D11 thrash → `FrameRateGuard` 60-cap, `859237d7`).
- **Demo günü:** (1) Play'i bir kez durdur→recompile→taze Play (cap+overlay+kod yüklenir) · (2) **canlı demoda MCP'yi KAPAT** (eşzamanlı bridge yükü = stall'ın ana tetikleyicisi) · (3) hâlâ takılırsa D3D12'ye geç (projede var, restart) + NVIDIA sürücü güncelle.

### 🧰 ARAÇLAR / METOT
- **F3 DebugLogOverlay** (Play'de ekranda Debug.Log/Warn/Error + `[Cast]/[Damage]/[Grant]`).
- **graphify query ÇALIŞIYOR:** `cd STAGING/_process/2026-06/graphify_fullmap && python -m graphify query "<soru>" --budget N` (graph Jun-14, çekirdek mimari; en yeni kod eksik olabilir). GRAPH_REPORT.md de var.
- Dispatch cheat-sheet: memory `reference_dispatch_skills`. Council oy-ağırlığı: cx2/opus2/ax_pro1/ax_flash0.5 (`feedback_council_vote_weighting`).
- ⚠️ Skiller **fare-nişanlı** (dummy'i vurmak için fareyi tut) — bug değil.

### ⏸️ ERTELENEN (post-demo) · 🛑 DOKUNMA
- **Task #1/#3 silah mount × animasyon:** pivot bıçak-merkezi + per-yön hand-socket 8'den 1'inde → gerçek el-hizalama = weaponless-anim (kilitli). Şekil 9 caption dürüst.
- Ronin (demo-dışı) 4 HitStop çağrısı + HitStop emekliye ayır.
- Working tree'de KALAN (BENİM DEĞİL, dokunma): `Jersey10 SDF` · `Assets/_Recovery/0 (2).unity` (crash artifact) · `capture_v3.zip`.
- Locked: GATE/Boss/reward-bleed/Build-core/weaponless-anim/branching-seed.
