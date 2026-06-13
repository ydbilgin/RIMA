# CURRENT_STATUS

## ⏯️ RESUME (2026-06-13 GECE — otonom doğrulama+fix turu BİTTİ; Faz 6 kullanıcı testi BUGÜN)

**⚠️ MODEL:** Orchestrator=Fable 5 (kullanıcı geçirdi). Writer=cx, reviewer=council, Opus=sub-agent dispatch.

**✅ BU TUR BİTENLER (kanıt: `STAGING/playtest_caps_2026-06-13/`):**
- **3-tanık E2E proof (rima-qc PASS):** Fable/Opus + ax Gemini 3.1 Pro + ax Opus 4.6 bağımsız play-mode koşusu → physPower 50→250 ⇒ finalDamage 50→250 birebir. ax/cx'in UnityMCP'yi otonom sürebildiği AMPİRİK kanıtlandı.
- **Gerçek vuruş kanıtı (QC coverage-gap kapandı):** packetized DealDamage canlı düşmanda → HP düşüşü 10→50 (5×, calc ile çapraz-doğrulandı) + telemetry events +2, DPS 600, düşman öldü.
- **Fable playtest** (`fable/_NOTES.md`): Elementalist canlı geçiş + Fireball TryActivate gerçek cast ✓ · düşman AI kovalıyor (BaseMobBehavior+MobAttack_Melee) ✓ · ölüm akışı VAR (DeathScreenCanvas+input kilidi) ✓ · console tüm tur 0 error.
- **flag#1 FIX (cx, diff-QC PASS):** rift_crystal → `Assets/Resources/DirectorProps/` + `Resources.Load` (editor fallback korundu). Play-mode'da palette Resources yolundan yüklendi, doğrulandı.
- **flag#2 FIX:** rift_crystal 1.8× + Light2D 1.6 → demo'da belirgin (screenshot var).
- **VFX tint doğrulandı:** Fire/Lightning/Frost ayrışıyor (`vfx_hitspark_simulated.png`, ps.Simulate tekniği) — `[visual unverified]` kalkabilir. ⚠️ Void koyu zeminde zayıf (minor palette lighten, cx'lik).
- **🖼️ BACKDROP CANLI:** init_01 PPU32 → MEVCUT parallax iskeleti `L1_BG_Far`'a native 21.25×12 takıldı (unlit asset mat, tint .82). L2/L3 placeholder perdeleri OFF (art bekliyor), L4_Fog α=0.12. +2 cyan RiftPulse Light2D (LightFlicker). Play-mode parallax ✓. `backdrop_native.png`.
- **Drift triage:** 7 gürültü dosyası revert ×2 (PropPool/Profile/TMP **her play'de yeniden kirleniyor** — kök neden TMP dynamic fallback atlas; kalıcı fix=static atlas, cx backlog).
- **LaurethStudio playbook:** `STAGING/LAURETHSTUDIO_PLAYBOOK_EXTRACTION_2026-06-13.md` (Opus çıkarımı; ~%70 zaten global skill, eksikler bootstrap-project'e doküman-kuralı).

**🚩 BUGÜN (kullanıcı testi + prova notları):**
1. **Ölüm GERİ DÖNÜŞSÜZ** — Heal ölüde no-op, respawn yok → sunumcu ölürse scene restart. PROVA EZBERİ.
2. Ölüyken Director class-switch controller enable ediyor (IsDead check yok) — minor, istenirse cx guard.
3. Düşman HP bar full canda KIRMIZI (kozmetik kafa karışıklığı).
4. 🔴 **VFX build-safety:** HitSpark/DeathBurst Resources dışında → standalone'da görünmez (rift_crystal sınıfı bug; editor-canlı demoya blocker DEĞİL) → cx task bekliyor.
5. Council vision verdikt: `council_vision_verdict.md` (ax Pro dispatch edildi, sonucu oku).
6. Fireball aim idle-yönde ıskalar (input-bağımlı, demo'da mouse-aim var — bilgi).

**⚠️ UNCOMMITTED:** Faz 2 + bugünün tüm fix'leri (DirectorMode.cs, _Arena.unity backdrop, rift_crystal Resources move, Backdrops/, testler, edge_filler PPU32) — kullanıcı onayı bekliyor. PropPool/TMP gürültüsü commit ÖNCESİ tekrar revert edilmeli (kronik).
- **🆕 cx skill:** `effort:<low|medium|high|xhigh>` + `timeout:<sn>` token desteği eklendi. ax: `--model` bayrağı ile model-paralel (Unity-süren işler hâlâ seri — tek socket).

---
*Önceki session blokları git history'de.*
