# TASK — Combat P0 hardening (ChatGPT review NO-GO blocker'ları) + 3× cold full-flow gate

ACTIVE RULES: (1) ÖNCE review'ı oku + her bug'ı KANITLA (2) cerrahi/min — çalışan sistemleri koru, broad refactor YOK (3) combat zinciri tam geçmeden DONE deme (4) geçmezse BLOCKED + data.

NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

UNITY ERROR CHECK: her değişiklikten sonra read_console; kendi hatanı çöz, ilgisiz/önceden-var bildir.

E1 OUTPUT: DONE dosyasına yaz; dönüş ≤10 satır + yol.

## BAĞLAM
ChatGPT bağımsız kod+screenshot review'ı verdi: **"Conditional NO-GO until combat full-flow passes."** Bizim önceki verify'ımız TEK ARENA oturumuydu → multi-room / boss / token-lifecycle sorunlarını kaçırdı. ÖNCE OKU:
- `STAGING/_process/2026-06/chatgpt_review_rev2/CGPT_REVIEW_OUTPUT/04_COMBAT_TECHNICAL_REVIEW.md`
- `.../05_FULL_FLOW_TEST_PROTOCOL.md`
- `.../09_SIX_QUESTIONS_ANSWERED.md` + `.../01_EXECUTIVE_VERDICT.md`

## P0 FIX'LER (sıra önemli; her birini KANITLA sonra düzelt)
1. **Player tag/target (verify-öncelikli):** Önceki builder gerçek player'ı Warblade (RoomRunDirector.cs:795 tag="Player") diye doğruladı — RE-CONFIRM gerçek runtime root'ta. AYRICA bazı saldırılar collider'da doğrudan `CompareTag("Player")` kullanıyor (`MobAttack_*`); bu damage-path gerçek player'da çalışıyor mu doğrula. Runtime validation warning ekle (player bulunamazsa Debug.LogWarning), tag fallback'i KORU.
2. **Boss Player re-acquire:** `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` Player'ı SADECE `Start()`'ta buluyor, re-acquire YOK → spawn-order/transition yarışında boss kalıcı idle kalabilir. Mob'lardaki re-acquire deseni gibi ekle (veya shared resolver). SADECE bu, boss phase/damage logic'e dokunma.
3. **🆕 Boss off-island spawn:** ChatGPT screenshot audit'i (02): shot 32-34 boss'u **yürünebilir ada DIŞINDA** spawn ediyor, HP bar yok. Boss spawn pozisyon logic'ini bul, boss'un **playable arena İÇİNDE** (walkable) spawn olmasını sağla.
4. **AttackTokenManager lifecycle:** `Assets/Scripts/Combat/AttackTokenManager.cs` `OnDestroy()` `_shuttingDown=true` yapıyor + `DontDestroyOnLoad` yok → scene transition sonrası token isteği kalıcı null dönebilir (düşmanlar bir daha saldıramaz). Fix: `_shuttingDown=true` sadece `OnApplicationQuit`'te, VEYA `DontDestroyOnLoad` + duplicate-instance guard. **Room/scene transition SONRASI test et.**
5. **Penitent opening lethality:** `Act1_Wave_Pilot.asset` — Penitent (enemyType 3, cost 4) opening budget'ı (4) tek başına doldurabilir. + `MobAttack_PenitentCombo.cs` combo 20/25/40=85 vs 100HP. Demo-safe: **Penitent'i opening'den çıkar** (wave 2/3'e), combo'yu ~40-50'ye indir (örn 10/12/20), ilk telegraph'ı 0.6-0.75s'ye uzat, 3. vuruşa ayrı tell. **Tüm oyunu rebalance ETME.**

## GATE — 3× COLD FULL-FLOW (protokol 05; ASIL doğrulama)
Gerçek entry route (MainMenu→CharacterSelect→_Arena), **_Arena'yı tek başına açma**. Her run:
- Runtime identity: canonical player tag/layer/HP/PlayerController/Health/PlayerAttack logla; saldırılabilir collider "Player" tag'li mi.
- Combat zinciri: enemy 9-12u'da player'ı edinir → Idle→Chase→Attack → player hasar alır → ≥1 düşman öldürür → counter değişir → wave clear → reward draft (1 kez) → seç → next room.
- Seam: F2 aç/kapa, Director aç/kapa, **portal/room transition**, sonra düşman spawn → **token'lar HÂLÂ çalışıyor mu**, timeScale==1, leftover scrim yok, çift EventSystem yok.
- Boss zinciri: boss **arena İÇİNDE** spawn → Player'ı bulur → HP bar+isim → P1 → %50 geçiş → %33 low-phase → telegraph timing=damage → boss öldür → continuation.
**PASS:** 3/3 run tamamlanır, 0 console error, kalıcı timeScale pause yok, missing-player idle yok, transition sonrası ölü token-manager yok, stale reward/death UI yok, prop soft-lock yok.

## RAPOR `STAGING/_process/2026-06/demo_fix_tasks/DONE_combat_p0.md` (ChatGPT'nin istediği format)
1. değişen dosya listesi + her değişikliğin gerekçesi
2. her P0 blocker için manuel test sonucu
3. console error/warning durumu
4. full-flow sonucu RUN RUN (3 run)
5. kalan riskler
6. **açık GO / NO-GO verdict** (combat zinciri gerçek entry'den geçmeden DONE deme)

DÖNÜŞ ≤10 satır: GO/NO-GO + düzeltilen P0'lar + 3-run özeti + dosya yolu.
