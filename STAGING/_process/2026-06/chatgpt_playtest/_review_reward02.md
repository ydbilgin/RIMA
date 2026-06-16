# REVIEW — REWARD-02 fix (RewardPickup.cs) — 2026-06-16

ACTIVE RULES: (1) think (2) min code (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>".

## Amaç
Aşağıdaki cerrahi fix'i REVIEW et (yazan: Opus orchestrator; sen REVIEWER'sın). Kod: `Assets/Scripts/Core/RewardPickup.cs`. Bağlam/karar: `STAGING/CHATGPT_PLAYTEST_EVAL_REWARD02_2026-06-16.md`.

**Bug (REWARD-02, doğrulandı):** reward `RoomRunDirector.ResolveRewardSpawnPosition()` ile oda MERKEZİNE spawn olur; oyuncu clear anında merkezdeyse reward üstüne çıkar → `OnTriggerEnter2D` (enter-geçişi) ateşlemez → `playerInRange=false` → G ölü; `RewardAutoCollectTimeoutSec=0` → soft-lock.

**Fix (3 ekleme):**
1. `OnTriggerStay2D(Collider2D)`: collected/playerInRange değilse + Player tag → playerInRange=true + ShowPrompt. (Çakışma sürerken her frame; oyuncu RB2D'li.)
2. `CheckInitialPlayerOverlap()` (Awake'te çağrılır): Physics2D.OverlapCircleAll(transform.position, colliderRadius*lossyScale) → Player varsa playerInRange=true + ShowPrompt. (Sleeping-rigidbody'de Stay durabilir → tek-seferlik kapatma.)
3. Awake sonu: `CheckInitialPlayerOverlap()` çağrısı.

## REVIEW SORULARI (PASS/FAIL + gerekçe)
1. **Correctness:** Fix gerçekten G'yi reward-üstüne-spawn senaryosunda çalıştırır mı? Mantık doğru mu?
2. **Awake timing riski:** `Physics2D.OverlapCircleAll` Awake'te yeni-instantiate obje için GEÇERLİ sonuç döner mi? (position AddComponent<RewardPickup>'tan ÖNCE set ediliyor; collider+radius da önce. Ama physics auto-sync Awake'te hazır mı? Eğer değilse Stay yine de yakalar mı?)
3. **Regression:** Çift `ShowPrompt` (Enter+Stay+initial) zararlı mı? Çift Collect riski? `playerInRange` guard yeterli mi? `ClearPlayerRange` hâlâ doğru çalışır mı?
4. **Edge:** `lossyScale` Awake'te = pickupVisualScale (1.1); RewardSpawnPop sonra scale'i 0→anim yapıyor — tek-seferlik check'i etkiler mi? OverlapCircleAll layer-mask=all → Player-dışı toplama CompareTag ile filtreleniyor mu (evet)? Alloc maliyeti önemli mi (tek sefer)?
5. **Kapsam:** Fix RewardPickup'ta → hem RoomRunDirector hem RuntimeRoomManager yolunu kapsar mı?
6. **REWARD-02 ile çakışma:** Karar dökümanı "asset üretimi prefab collider/G-input akışını değiştirmesin" diyor; bu fix sadece kod, prefab'a dokunmuyor — doğru mu?

## ÇIKTI
VERDICT (PASS / FAIL / PASS-with-nits) + bulgular. Yaz: cx → `STAGING/_process/2026-06/chatgpt_playtest/_review_reward02_cx.md` · auditor → `_review_reward02_auditor.md`. Dönüşte ≤10 satır + yol.
UNITY ERROR CHECK: compile zaten 0-error doğrulandı (read_console temiz); sen tekrar Unity ÇALIŞTIRMA (review-only, statik).
