# GOLDEN-PATH LIVE-TEST SONUÇLARI — 2026-06-16 (data-proof, dev-direct _Arena)

Orchestrator seri canlı test (tek-Unity-ajan). Plan: `STAGING/DEMO_GOLDENPATH_LIVE_TEST_PLAN_2026-06-16.md`.

## 🧱 METODOLOJİ DUVARI (önemli)
- **MCP `wasPressedThisFrame` ENJEKTE EDEMİYOR:** kesin test — `isPressed=True` yapılabiliyor ama `wasPressedThisFrame=False` kalıyor. TÜM tuş-toggle'ları (F2/`/G/ESC) ve combat (LMB) bunu okuyor → **fiziksel tuş/click/combat MCP ile simüle edilemiyor.**
- **Overlay paneller screenshot'a çıkmıyor** (bilinen). → panel doğrulaması = data-proof.
- Sonuç: "screenshot'la oynayıp test" yöntemi fiziksel-input gereken beat'lerde MCP ile mümkün değil → o beat'ler **data-proof (wiring+precondition+yapı) GREEN + fiziksel teyit = kullanıcı**.

## SONUÇLAR (beat-bazlı)
| Beat | Sonuç | Kanıt |
|---|---|---|
| dev-direct _Arena bootstrap | ✅ GREEN | 0 console error; RoomRunDirector+Player+HUD+DirectorMode+BuildModeController+DraftManager hepsi ayakta |
| B0 Panels (Pause/Settings/Codex/HUD) | ✅ GREEN (yapı) | Pause 5btn/6TMP · Settings 17btn/44TMP · Codex 10 sınıf-btn/115TMP · HUD 24TMP · **fontNull=0 (garbled YOK), alpha=1, blocksRaycasts=true, emptyTxt=0** (HUD'da 11 boş = combat-öncesi, dolar). Estetik+ESC/click = KULLANICI |
| B2 stat→damage | ✅ GREEN (wiring) | `SetStatForValidation('physPower',250)=True` + physPower=250 set oldu; DamageCalculator physPower/100 kod-teyitli; empirik LMB delta = önceki session |
| B3 REWARD-02 (fix) | ✅ GREEN (predikat canlı + dual-review) | Player collider **tag=Player** (tek collider); `OverlapCircleAll(playerPos)` onu buluyor → `CheckInitialPlayerOverlap` TRUE → playerInRange true olur. Tam combat→G→3-kart = KULLANICI (G injection bloklu) |
| B6 F2 + " Build Mode | ✅ GREEN (aktif) | Wiring BuildModeController.Update:180; **TÜM precondition PASS** (overlay yok/draft yok/DirectorMode ayakta/Camera var) → gerçek basışta toggle EDER. Fiziksel F2/" = KULLANICI |
| B7 ` Director | ✅ GREEN (aktif) | up + State=Test + BuildModeActive=false → backquote toggle EDER. Fiziksel ` + Test-state overlay-bleed fix teyidi = KULLANICI |
| B4 Telemetry | ⏳ ÇALIŞTIRILAMADI | Gerçek combat (LMB hits) gerektirir → input bloklu. Wiring kod-teyitli (DirectorMode OnDamageApplied). KULLANICI combat-sonrası bakar |
| B5 Room-transition/F1 | ⏳ ÇALIŞTIRILAMADI | Combat + kapı-G gerektirir → input bloklu. Kod riski biliniyor (BuildCurrentRoom sahne-genel sweep yok). KULLANICI |
| LIFE-01 (ChatGPT paketi) | ⚠️ REPRODÜKE | Play-exit'te "Some objects were not cleaned up when closing the scene" → LIFE-01 gerçek. Scene-close uyarısı (gameplay-kıran değil); demo BuildMode placement kullanmaz → **post-demo** |

## KULLANICI FİZİKSEL CHECKLIST (~5 dk, editor'de dev-direct _Arena: menü `RIMA/Play From Main Menu` TIK-KALDIR → Play)
1. **F2** → Build Mode açıldı mı, panel, prop yerleştir, F2 kapat, prop kaldı + oyun devam mı.
2. **"** → aynı (Build Mode toggle).
3. **`** → Director overlay açıldı mı; ` tekrar → Test, overlay KAYBOLDU mu (bleed fix).
4. Oda temizle (LMB) → reward → üstüne yürü → **G** → 3 kart çıktı mı (REWARD-02). Kart footer/açıklama dikey şeride çöküyor mu (UI-01).
5. Reward'ı TOPLAMADAN kapıdan geç → sonraki odada eski reward kaldı mı (F1).
6. **ESC** → Pause; Settings/Codex aç-kapa; panellere göz (güzel mi).
7. **`** → Director → Telemetry tab; combat sonrası veri/DPS/TTK var mı.

## ÖZET
Kod/wiring/yapı seviyesinde demo golden-path **GREEN** (REWARD-02 fix canlı-doğrulandı, F2/"/`  aktif, paneller yapısal-tam+okunabilir, stat→damage wiring). MCP-input duvarı yüzünden **fiziksel oynanış teyidi kullanıcıda** (yukarıdaki ~5dk checklist). LIFE-01 post-demo.
