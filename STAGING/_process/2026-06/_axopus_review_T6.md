ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerek yok — bu saf kod review'ı.

# Amaç
REVIEW-ONLY (kod değişikliği YOK): T6 + T6.1 commit'lerinin kalite denetimi. Commit'ler: 86b8cfe3 (boss intro/telegraph/payoff + echo breakdown + SeamCrawler purge) ve efc02bd6 (panel okunabilirliği + telegraph decal'leri + placeholder dedupe). `git show 86b8cfe3` ve `git show efc02bd6` ile diff'leri incele.

# Odak (file:line kanıtlı bulgu formatı: MAJOR/MINOR/NOTE)
1. BossIntroController: coroutine yaşam döngüsü (sahne unload/boss erken ölümünde leak/crash?), timeScale'e dokunuyorsa restore garantisi, çifte Begin() guard'ı.
2. PenitentSovereign: telegraph wiring'de spawn edilen decal child'larının temizliği (saldırı iptalinde decal kalıyor mu?), DeathSequence slow-mo owner-guard (mevcut HitPauseDriver/0.3 slow-mo pattern'iyle çakışma), 1.75x scale'in collider'a etkisi.
3. EnemyTelegraph: SpawnDecal'ın her telegraph'ta yeni GO üretip Destroy ettiğinden emin ol (pool yok = OK ama leak olmasın); "Decals" sorting layer'ı projede gerçekten tanımlı mı yoksa runtime'da sessizce default'a mı düşüyor?
4. DemoCompleteOverlay/DeathScreenManager: anchor/autosize değişikliklerinin farklı çözünürlüklerde kırılganlığı; string birleştirmede null RunStats guard'ı.
5. RewardPickup.Awake sprite fallback: Resources.Load her Awake'te mi (cache?); prompt canvas zaten varken davranış.
6. Prefab YAML shape değişiklikleri (10 dosya): yanlışlıkla başka alan bozulmuş mu (guid/script ref kayması).

# Çıktı
STAGING/_process/2026-06/_review_T6_axopus.md dosyasina: bulgu listesi (MAJOR=düzeltilmeli / MINOR=backlog / NOTE) + genel PASS/FAIL verdict. KISA tut — bu kota-dostu mini review.

