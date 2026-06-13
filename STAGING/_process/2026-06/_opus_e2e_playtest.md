ACTIVE RULES: (1) think before acting (2) min değişiklik=SIFIR kod değişikliği (bu PLAYTEST/doğrulama görevi) (3) surgical (4) kanıtlayamadığın adımı UNCERTAIN/FAIL işaretle, uydurma.
NLM ACCESS: Gerekmez.
UNITY ERROR CHECK: Her Play Mode oturumu sonrası mcp__UnityMCP__read_console (Error) kontrol et; SENİN tetiklediğin hata varsa raporla.

Proje kökü: F:/Antigravity Projeler/2d roguelite/RIMA (Unity açık). BUGÜN CANLI DEMO. Görev: bu gecenin tüm fixّlerinin demo akışını uçtan uca BOZMADIĞINI kanıtla.

# GÖREV: Seviye-3 E2E PLAYTEST (UnityMCP API-driven, Play Mode)
Demo run-of-show'unun (STAGING/DEMO_SUNUM_PLANI_2026-06-13.md — oku) her beat'ini Play Mode'da programatik simüle et, state assert'le. DirectorMode'un `*ForValidation` hook'larını kullan (reference: memory `reference_directormode_validation_api.md` mantığı; execute_code ile çağır). Screenshot YERİNE data-proof (overlay UI screenshot'a çıkmaz).

## Senaryo (sırayla, her adımda compact assert string — E5):
1. **Boot:** `_Arena` Play Mode'a gir → Warblade spawn (Player tag), Health>0, PlayerController+PlayerAttack enabled. Console temiz mi?
2. **Combat:** bir düşman spawn et (DirectorMode spawn API/ForValidation) → LMB/temel saldırı hasarını programatik uygula → düşman Health düşüyor + OnDamageApplied telemetry +1.
3. **Skill-kill juice (bu gecenin fix'i):** düşmanı skill/raw yolla öldür → PublishKill TAM 1 kez (kill-juice tetiklenmeli); ölü hedefe 2. vuruş 0 publish.
4. **Stat tune (bu gecenin fix'i):** physPower 50→250 (SetStatForValidation) → LMB hasarı ölçeklenir (raw-damage yolu değil, packetized LMB); Q/E/R/F bypassStatScaling → değişmez (beklenen). debugGlobalDamageMult etkisi packetized yolda görünür.
5. **Stat preset (yeni):** TANK preset → maxHP~300/dmg~0.5; GLASS → maxHP~30/dmg~4.5; VARSAYILAN → profil default. Quick Reset ile tutarlı.
6. **Spawn cap:** 12 spawn iste → cap=10 doğrula.
7. **Ölüm → Quick Reset (bu gecenin fix'i):** oyuncuyu öldür → death screen aktif → Quick Reset → Revive (HP full), spawn'lar temiz, timeScale=1, PlayerController+PlayerAttack TEKRAR enabled (SetPlayerActive simetrisi).
8. **Director pause saldırı-kilidi (bu gecenin fix'i):** Director aç (pause) → PlayerAttack.enabled=False; kapat → True.
9. **Dual-class buton (yeni):** Director'da Dual-Class Draft butonu tetikle → Director overlay kapanır + ClassSelectionUI topmost (so=190, ts=0) → sınıf seç → controller 1→2, ManaSystem, ts=1, buton gizli. Ölüm aktifken buton → seçim AÇILMAZ.
10. **TelemetryClock pause (bu gecenin fix'i):** Director pause sırasında DPS penceresi ilerlemiyor (clock donuk).

Her Play Mode oturumundan SONRA ÇIK (isPlaying=False bırak — kullanıcının editörünü Play'de bırakma). Uzun tek-blok yerine adım adım; bir adım FAIL olursa devam et ve raporla (akışı durdurma).

## RAPOR (E1):
`STAGING/_process/2026-06/_e2e_playtest_2026-06-13.md` (tam Türkçe karakter): her adım PASS/FAIL + assert kanıtı. Sonda: GENEL DEMO-HAZIRLIK VERDİKTİ + (varsa) demo öncesi düzeltilmesi gereken kırıklar önem sırasıyla. DÖNÜŞ ≤10 satır: kaç adım PASS/FAIL + en kritik kırık (varsa) + rapor yolu.