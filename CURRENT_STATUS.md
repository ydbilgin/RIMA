# CURRENT_STATUS

## ⏯️ RESUME (2026-06-17 — BİTİRME MARATONU · demo ~19 Haziran)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · **cx TÜM profillerde quota-BLOCKED** (5h/week maxed) → execute=**Claude Opus subagent (builder-opus)** + ax_opus (kullanıcı tüm kanal+kota iznini verdi). HARD: TEK Unity-süren ajan (seri). **PixelLab balance=0** (yeni gen yok). Builder kuralı: `git checkout/reset KULLANMA` (başka uncommitted işi siler — bir kez oldu).

### 🎯 KARAR: `STAGING/DEMO_BITIRME_DECISION_2026-06-17.md` + ChatGPT review: `STAGING/_process/2026-06/chatgpt_review_rev2/CGPT_REVIEW_OUTPUT/`

### ✅ BU SESSION DONE (hepsi runtime-verified + pushed, son commit `9e3fd2f9`)
- **Combat P0 = GO (conditional)** — boss re-acquire · death-restart token-death · Penitent opening-çıkar+combo 85→42 (3 gerçek/2 non-repro). det 8→12 engagement.
- **Polish:** CombatJuice (_Arena) · **EnemyReadable outline** (gerçek silhouette, ambient 0.22→0.35, düşman görünmezlik çözüldü) · **boss 6 telegraph** + refinements (Wrath green-safe-ring / origin-snapshot / FlashImpact finisher-only) · **arena dressing** (6 prop) · **music bed** (JaggedStone CC0).
- **Edit-to-Play mekanizması:** deterministik ilk-oda (`combat_large_cross_01`, 3/3) — branching korundu. ⚠️ `room_current.json` _Arena için ÖLÜ YOL (kullanma).
- A1 prop import (F2 9→19) · Director IDE-skin · capture_v3 (35, capture-truth).

### 🔴 GO-CONDITION (kullanıcıda — demo gate)
**3× cold manual full-flow + elle boss-kill + F2/Director/portal seam toggle** = `STAGING/DEMO_MANUAL_TEST_CHECKLIST_2026-06-17.md`. Builder 1 otomatik cold cycle PASS yaptı; gerçek 3× kullanıcı elinde.
👉 **KULLANICI ADIM-ADIM TODO: `STAGING/USER_NEXT_SESSION_TODO_2026-06-17.md`** (test → 🚩 ver → recapture → koreografi; tek tek).

### 🔜 KALAN
1. **Kullanıcı manual test** (gate, #1) → 🚩 → fix.
2. **Truthful recapture** (ChatGPT 03_RECAPTURE_PLAN): in-game kareler=builder, **overlay (draft/runmap/char-sheet)=kullanıcı OBS**, + yedek video. capture_v3'ün ÇOĞU mislabel'di (combat=draft/death, boss=off-island artefakt, Director-Map=placeholder REMOVE).
3. **Edit-to-Play canlı koreografi** (centerpiece, combat-dışı oda — spawnProps gate) = kullanıcı tasarım.
4. Rapor doc final (`SUNUM_RAPOR_ICERIK_2026-06-17.md` yaşayan; run-of-show=`DEMO_SUNUM_PLANI_2026-06-13.md`).

### 🧠 KEY
- "yeşil-assert ≠ çalışıyor" KEZ KEZ doğrulandı (tek-arena verify bile yetmedi; multi-room+boss+token ChatGPT-review ile yakalandı). Çok-katmanlı doğrulama = rapor değeri.
- DOKUNMA: GATE/Boss-presentation/HUD/reward-bleed/Build-placement/Director-skin/spawnProps-false-gate/weaponless-anim/branching-seed.
- Minör: SeloutOutline.mat orphan · HitPauseDriver overlapping-pause latch (gerçek oynanışta tetiklenmiyor) · draft-sonrası timeScale desync (latent).

*(Detay: MEMORY.md + [[project-demo-bitirme-decision-2026-06-17]]. Önceki RESUME git'te.)*
