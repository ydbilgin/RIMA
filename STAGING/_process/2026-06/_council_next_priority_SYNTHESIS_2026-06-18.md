# COUNCIL SYNTHESIS — Demo-arifesi sıradaki iş (2026-06-18)

Advisors: cx (Codex, w2) + ax_pro (Gemini 3.1 Pro, w1) + ax_flash (Gemini 3.5 Flash, w0.5). Demo ~yarın, editörde canlı. HIGH-4+CanExecute zaten commit'li (`6ba61ff5`, test-clean).

## Convergence (ağırlıklı)
- **UNANIMOUS:** polish-commit + exposure 0.6 (Risk:0) · A1 failed-cast feedback · A3 Director dup-slot · HUD lerp + low-HP de-stack · ERTELE: healMultiplier/combo/perf.
- **Ayrışma:** A4 Merchant Echo-drain — cx(2) yap > ax(1.5) ertele → **DAHİL**. dead-but-acting + hit-flash → cx koşullu/dar.

## KARAR — Sıralı plan
**FAZ 0 (orchestrator, Risk-0, ayrı commit):**
- Exposure postExposure 0.35→0.6 → screenshot doğrula → polish paketini (`_Arena` + `ArenaPostFX.asset` + `AtmoLights`) gameplay-fix'lerden AYRI commit'le.

**FAZ 1 (builder serial, tek Unity-batch, demo-relevant bugfix):**
1. **A1 Failed-cast feedback** — CanExecute veto + yetersiz-kaynak durumunda kısa SFX + caster flash + toast. ⚠️ SADECE veto/feedback ucu; hasar/execution path'e DOKUNMA. (CanExecute infra zaten var.)
2. **A3 Director dup-slot guard** — zaten-equipped skill 2. slota alınamaz / swap. Loadout/slot validation seviyesi; combat numerics'e dokunma.
3. **A4 Merchant persistent-Echo drain** — shop spend kaynağını run-local currency'ye sabitle (`PlayerEconomy.Gold`) VEYA persistent-spend yolunu demo için hard-block. EchoWallet drain'i kapat.
4. **B1 HUD HP-bar lerp + toast ease** — UI smoothing, core combat sonucu değişmez.
5. **B2 Low-HP/Rage red-screen de-stack** — overlay arbitration/alpha clamp; glitch fix.
6. **(KOŞULLU) Hit-flash beyaz** — SADECE mevcut hasar-feedback ucunda LOCAL değişiklikle yapılabiliyorsa al; damage path'ine geniş dokunma gerekiyorsa SKIP + raporla.

**ERTELE (post-demo):** healMultiplier race · Glacial+Burn/Ice-Shatter/Severance combo · dead-but-acting (dar guard, demo-rastlantısal) · 9 Find-in-hot-path perf (guarded-cache).

---

## ⭐ CRITIC-REVISED FINAL (adversarial Opus-4.8, VERDICT: REVISE → uygulandı)
Kritik dosyası: `_council_critic_next_priority_2026-06-18.md`. Binding fix'ler:
- **A4 Merchant KESİLDİ → post-demo.** Kanıt: EchoWallet=PlayerPrefs persist(200), PlayerEconomy.Gold=run-local in-memory(start 0, sadece Draft+Chest besler). Spend'i Gold'a sabitlemek → Gold=0'da shop ölür = YENİ demo-killer. Currency-migration cerrahi değil.
- **hit-flash KESİLDİ** — zaten var (HitFlash.cs + HitFlashDriver.cs self-wired). **B1 HUD lerp + B2 low-HP de-stack → post-demo** (B2 3-yönlü cross-system, demo-eve minimalizm).
- **FAZ-0:** exposure zaten 0.6 (no-op); kala = commit.

### FİNAL FAZ-1 (builder serial, SADECE 2 madde):
1. **A1 Failed-cast feedback (cooldown-ayrımlı):** feedback (SFX + caster flash + toast) SADECE (a) resource yetersiz (b) CanExecute veto durumunda. **Cooldown (IsReady-fail) SESSİZ kalmalı** (tuş-spam SFX yok). Tetik `TryActivate()` içinde. Hasar/execution path'e DOKUNMA.
2. **A3 Director dup-slot REJECT:** zaten-equipped skill 2. slota → **swap değil REJECT**, mevcut `out error`/`status.assign_failed` UI'ını kullan. `AddComponent(skillType)` combat-equip path olduğundan orphan-component leak'e dikkat (reject = AddComponent ÇAĞIRMA).

### ERTELE (post-demo): A4 Merchant · B1 HUD lerp · B2 low-HP de-stack · healMultiplier · combo correctness · dead-but-acting · perf 9-Find.

## Gerekçe
Demo-eve disiplini: "canlı sunumda bozuk görünür" riskleri kapat, core combat numerics'e minimum dokun. FAZ-1 hepsi UI/feedback/validation/economy ucu — combat damage/execution path'i KORUNUYOR. Polish ayrı commit (gameplay fix'lerden izole).
