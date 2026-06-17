# KAPSAMLI CAPTURE EXEC SPEC — crafter-sonnet (2026-06-17)

ACTIVE RULES: (1) think (2) min code (3) surgical — SADECE capture+assert, kaynak/scene/git DEĞİŞTİRME, COMMIT YOK (4) 2 denemede ulaşamadığın state'i ATLA+LOG, akışı sürdür.
UNITY ERROR CHECK: bitince read_console (Error); kendi hatanı çöz, raporda console durumu.
> ⚠️ SEN read-only CAPTURE ajanısın. Kod yazma, git'e DOKUNMA. Sadece dev-direct play + execute_code (assert) + screenshot.

## AMAÇ
RIMA'nın TÜM yakalanabilir state'lerini **çift-kanıt** ile yakala: (1) screenshot, (2) `*ForValidation` runtime-assert (özellikle BuildMode tile/asset DOĞRULUĞU — kullanıcının asıl sorusu). Hedef: 13 değil, **40+ state.**

## KAYNAK (ÖNCE OKU)
- `STAGING/_process/2026-06/screencap_council/RESP_cx.md` — **60+ state matrisi** (kategori→state→ulaşım→alt-state) + **B bölümü = gerçek `*ForValidation` metod adlarıyla assert reçeteleri** + capture priority list (§Kisa capture oncelik listesi). BU ANA REHBER.
- `RESP_Antigravity.md` — 30+ ekran [GOLDEN]/[EDGE] (ikincil referans).

## ORTAM: dev-direct `_Arena` (Director/Build BURADA kesin çalışır)
- Başta: execute_code → `playModeStartScene=_Arena` (LoadAssetAtPath SceneAsset). Play. SONUNDA `playModeStartScene=MainMenu` restore + stop.
- State'lere ulaşım araçları: **DemoDebugPanel (F1 / runtime'da ilgili metodlar)** = jump room / next room / kill all / force clear / restart; **DirectorMode** spawn/stat; **DemoDebug** ile boss/merchant/chest odalarına atla. Draft'ı `DraftManager.Instance` ile aç/kapat. Düşman öldür = `Health.TakeDamage(99999)`.
- Build/Director: dev-direct'te `BuildModeController.Instance.Toggle()` / `DirectorMode.Instance.ToggleState()` ile gir (F2/" full-flow'da güvenilmez — dev-direct kullan).

## YAKALAMA (cx priority list sırası — GOLDEN öncelik)
1. Menü/başlangıç: MainMenu, Settings (bölümler), CharacterSelect, Chamber, opening-draft
2. **Combat full loop:** entry/room-label, wave-spawn, mid-combat, telegraph, hit-flash, low-HP, clear/"ODA TEMİZLENDİ", reward-pickup, reward-draft, kapı-açık
3. Merchant odası (3 ShopStand), Boss odası (intro+health-bar+death), dual-class, victory (DemoCompleteOverlay)
4. **BuildMode TÜM alt-state + ASSERT (kullanıcının asıl sorusu):** PROP tab (asset-seç→ghost→yerleştir), TILE tab (floor/walkable/overlay, radius), seçili-vurgu, geçerli/geçersiz ghost. **ASSERT (cx §BuildMode verify):** `SelectFirstPropForValidation()`→true · `PlaceForValidation(cell)`→`PlacedCountForValidation()`+1 + `props.Last().origin==cell` (TILE DOĞRU OTURUYOR kanıtı) · `EraseForValidation`→-1 · undo/redo. Tile: `SelectToolForValidation(1)`.
5. DirectorMode TÜM tab + ASSERT: Spawn (`SpawnSelectedEnemyAtForValidation`→count+1), Stats (`SetStatForValidation`→değer), Telemetry, Build-tab
6. Pause, Codex (her sınıf sekmesi), TAB CharacterSheet, SkillBar/passive/tooltip yakın-çekim
7. Death (`Health.TakeDamage(99999)`→DeathScreen), RoomTransitionFX, kapı/portal prompt
8. Edge (ulaşılırsa): Elite/Chest/Forge/Event

## ÇİFT-KANIT FORMATI (cx §Capture automation format)
- Screenshot: `STAGING/_process/2026-06/demo_screenshots/full/NN_kategori_state.png` (include_image=false, max_resolution=1280). Fail olursa `FAIL_NN_...png`.
- Assert: aynı klasöre `NN_kategori_state_assert.json` (veya tek toplu `_asserts.json`) — pre/action/post/PASS-FAIL. Build tile/asset assert'leri ZORUNLU.
- execute_code METOD GÖVDESİ → üst-using YOK, tam-nitelik (UnityEngine.GameObject..., RIMA....).

## DÖNÜŞ (≤20 satır)
Yakalanan state sayısı + kategoriler · BuildMode tile/asset assert sonucu (PASS/FAIL — kullanıcı bunu özellikle istiyor) · Director assert sonucu · ulaşılamayanlar+neden · console error · playModeStartScene MainMenu'ye restore (EVET/HAYIR). Ekran görüntülerini DÖNÜŞE GÖMME — orchestrator dosyaları okuyacak.
