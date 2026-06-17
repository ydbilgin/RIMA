# CURRENT_STATUS

## ⏯️ RESUME (2026-06-17 — BİTİRME AŞAMASI · demo ~19 Haziran)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=**cx dispatch (bugra sağlam; yekta/yasinderyabilgin maxed)** + her iş sonrası **elle full-flow playtest** ("yeşil-assert ≠ çalışıyor"). HARD: TEK Unity-süren ajan (seri). cx hard-cap kaldırıldı. **PixelLab balance=0 (5125/5000) → yeni gen YOK** (reset'e kadar; Warblade/Elementalist animate bekler). Otonom mandat AKTİF: oynanabilir+güzel yap, playtest'le ayarla, mevcut asset'leri kullan.

### 🎯 KARAR (canonical): `STAGING/DEMO_BITIRME_DECISION_2026-06-17.md`
Council `wf_78b723a8` (23 agent) + telegraph spec + A1 prop import sentezi. **Tek cümle:** son ~1 günü CANLI Edit-to-Play hikâyesine harca; 6 biten sistemi koru; demo hocanın canlı provada gördüğü+duyduğu ile kazanılır.

### 🔄 ŞU AN KOŞAN
- 📸 **cx capture V3** (Unity) → bütün ekranlar taze+capture-truth; bitince `capture_v3/DONE_capture_v3.md` oku.

### 🔒 KİLİTLİ SIRA (capture bitince Unity serbest → başla)
1. **6 sistemi TEK kesintisiz dry-run'da RE-VERIFY** [manual] — SEAM'ler (F2/Director/portal); takılırsa dur-düzelt. **Non-negotiable.**
2. **CombatJuice.prefab → `_Arena.unity`** [cx] — #1 etki/efor; stack kodlu ama _Arena'da YOK. TEST: play melee/kill juice + F2/Director sonrası.
3. **`Enemy_SeloutOutline.mat` → FractureImp/HalfThrall/Penitent** [cx, step-2 ile batch] — koyu mob görünmezliği #1 kusur.
4. **Arena döşeme 5-6 prop (KULLANICI elle) + room_current.json pre-bake/commit** [user+cx].
5. **🎯 Telegraph: boss P2/P3 + `SpawnDelayedRing`/`FlashImpact`** [cx] — sistem ZATEN var (`Enemies/EnemyTelegraph.cs`); spec `ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`. windup↔hasar BİREBİR.
6. **Merchant teardown boss-öncesi** [cx, canlı-bug ise] · 7. **capture-truth draft+runmap (OBS, EN SON)** · 8. opsiyonel UI-skin/scrim/music · 9. sunum run-sheet (graphify god-node=açılış).

### ⚠️ SÜRPRİZLER (önceki planı düzeltti)
- **🔴 5-enemy black-blob wire = İPTAL** — canlı dalga (`Act1_Wave_Pilot`) sadece FractureImp/Penitent/HalfThrall spawn ediyor (zaten wired); 5 isimli prefab demo'da görünmez + anim frame'leri projede yok. RESUME'daki "ucuz wire" YANLIŞMIŞ.
- **🔊 Audio zaten CANLI** (18/19 cue, 42 call-site) — sadece prova + 1 `music_demo.wav` (CC0) bed eksik.
- **✅ A1 prop import DONE:** F2 AllProps 9→19, console 0, select_object_frames çağrılmadı (16 aday korundu).
- **✅ Director IDE-skin DONE** (council-test PASS-WITH-NITS).

### 📦 SONRAKİ PROCESS
Karar sonrası: **commit+push → ChatGPT paketi** (GitHub erişimi var → repo+taze screenshot+DECISION+telegraph spec review). Sonra otonom execute+playtest loop.

### 🧠 KEY
- DOKUNMA: GATE/Boss/HUD/reward-bleed/Build-placement/Director-skin/3-enemy-controller/spawnProps-false-gate/weaponless-anim.
- Latent: PortalSpawn fail-open · standalone DEMO_BUILD define · [Obsolete] MainMenuScreen çift-EventSystem (entry dry-run'da doğrula).

*(Detay: MEMORY.md + [[project-demo-bitirme-decision-2026-06-17]]. Önceki RESUME git'te.)*
