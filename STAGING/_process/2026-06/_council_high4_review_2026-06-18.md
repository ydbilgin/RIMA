# COUNCIL REVIEW — DEMO HIGH-4 FIX BATCH (2026-06-18)

**READ-ONLY. Kod/dosya/git'e DOKUNMA. Unity'yi ÇALIŞTIRMA / Play etme** (tek-Unity-ajan kuralı, başka süreç editörü kullanabilir). Sadece statik kod incele + verdict yaz.

## Bağlam
RIMA Unity ARPG, demo ~yarın. Builder-opus 4 confirmed demo-killer bug'ı fix'ledi (working tree, commit edilmedi). Compile: 0 error / 0 warning. Şimdi diff'i adversarial doğrula.

- Diff: `git diff -- '*.cs'` (RIMA kökünde). Dokunulan dosyalar: RunStats.cs, PenitentSovereign.cs, RoomRunDirector.cs, SkillBase.cs, Blink.cs, BladeRush.cs, IronCharge.cs.
- Spec: `STAGING/_process/2026-06/DEMO_HIGH4_FIX_SPEC_2026-06-18.md`
- Builder raporu: `STAGING/_process/2026-06/DEMO_HIGH4_FIX_DONE_2026-06-18.md`

## FIX'ler (özet)
1. **Movement off-map** — IronCharge/BladeRush raw velocity → `WalkabilityMap.ClampVelocityToWalkable(...)`; Blink teleport → `IsDashableWorld` + ray-snap-back.
2. **SkillBase spend-before-veto** — `protected virtual bool CanExecute() => true;`, TryActivate'te cost/cd'den ÖNCE check. ⚠️ Builder per-skill override'ları ERTELEDİ (default true → şu an HİÇBİR skill veto etmiyor).
3. **RunStats progression-desync** — `RunStats.NotifyRoomCleared()` köprüsü, RoomRunDirector.HandleEncounterCleared'dan çağrı.
4. **Boss Phase-2 burst-skip** — `phase2EnterTime` + Faz-3 trigger'ı `Time.time - phase2EnterTime >= 8f` ile gate.

## SORULAR (her birine net cevap)
1. **API doğruluğu:** `WalkabilityMap.ClampVelocityToWalkable(instance, pos, vel, dt)` ve `IsDashableWorld(point)` imzaları GERÇEK mi? PlayerController/KnockbackReceiver kullanımıyla birebir mi? (yanlış imza = derlenmiş ama yanlış davranış riski — derlendi ama mantık doğru mu?)
2. **FIX 2 KRİTİK RULING:** Infra-only `CanExecute()` (hiç override yok) ASIL semptomu (Chain Lightning menzilde düşman yokken mana+cd israfı) çözüyor MU? Çözmüyorsa: demo için HANGİ skiller `CanExecute()` override etmeli? Range-gated no-op edebilen skilleri LİSTELE (dosya + koşul). Bu demo-kritik mi yoksa ertelenebilir mi?
3. **FIX 3 double-count:** `OnRoomCleared()` private metodu SADECE `roomsCleared++` mu yapıyor, yoksa yan-etki (reward spawn, event) var mı? `_Arena` path'inde çift-tetik riski gerçekten yok mu?
4. **FIX 4:** Phase-lock mantığı doğru mu? Edge-case (boss 8s dolmadan <%33'e düşerse Faz-2'de kalıyor — doğru intent mi, kilitlenme/softlock yaratır mı)?
5. **Regresyon:** SkillBase base-class değişikliği herhangi bir skill subclass'ında davranış değiştiriyor mu? Movement clamp normal dash/charge'ı kısıtlıyor mu (false-positive void-clamp)?

## ÇIKTI
Verdict: **PASS / PASS-WITH-FIXES / FAIL** + önceliklendirilmiş bulgular (HIGH/MED/LOW), her biri dosya:satır + kanıt. Çıktıyı `STAGING/_process/2026-06/_council_high4_<advisor>_2026-06-18.md`'ye yaz, dönüşte ≤8 satır özet ver.
