# ChatGPT REV2 + Council — ORTAK KARAR (2026-06-17, demo 19 Haz = 2 gün)

**Girdi:** ChatGPT REV2 review paketi (`STAGING/_process/2026-06/chatgpt_review_rev2/`) + 3-lens council (cx kod-fizibilite · ax Pro design/vision · ax Flash lean-skeptic). Ham yanıtlar: `STAGING/_process/2026-06/chatgpt_review_council/RESP_{cx,axpro,axflash}.md`.

**Tek-cümle verdict:** Çekirdeği yeniden yazma. Capture-truth + demo-giriş kapısını ÖNCE çöz; sonra görsel zamanı Boss/HUD/Director'a harca — **Director'ı prefab-refactor ETMEDEN.**

---

## DEMO-GİRİŞ KARARI (kullanıcı seçti: HİBRİT) — her şeyi kilitleyen kapı
cx onayladı: `DirectorMode` MainMenu/CharacterSelect'i atlıyor (`DirectorMode.cs:143-177`) + `BuildModeController` `DirectorMode.Instance`'a sıkı bağımlı (`:223-228`). Menüden başlayınca Director+F2 **ölü**.
**GATE:** bootstrap-fix'e **MAX 2 saat** (cx kök-neden buldu: scene-guard + hard-dep). 
- TUTARSA → full-flow demo (menü→oyun→F2/Director).
- TUTMAZSA → `_Arena` dev-direct runbook (F2/Director orada kesin çalışıyor) + standalone build için `DEMO_BUILD` define coverage (cx: editör _Arena başarısı ≠ build güveni).

---

## 4-BAKIŞ UZLAŞISI (kanıtlı)

| Konu | Hüküm | Kanıt / sebep |
|---|---|---|
| **Capture-QA = sahte kanıt** | P0 — güvenilirlik riski | cx SHA: `08=09` (D0564A), `20=21` (A53D37); 19/20/21 panel görünmüyor. `ScreenshotMode.cs:203-227` SHA/root-panel gate'i YOK. ax Pro: akademik sunumda kırmızı bayrak |
| **Build Mode** | polish, redesign DEĞİL (2-4h) | legacy IMGUI `InPlayMapPaintOverlay` EMEKLİ; aktif Build uGUI+world grid/cursor/ghost (`BuildPlacementController:154-170,392-405,684-727`; `BuildTileBrushController:767-831`). Hover/footprint/valid-invalid = 2-3 dosya. ChatGPT REV2 öz-düzeltmesi DOĞRU |
| **HUD resize** | P0, ucuz (1-2h, bar-only 1 dosya) | `HUDController.cs:54-56,586-649` sabitler (72×4, 48×3) procedural. Slot dahilse + `SkillBarUI` |
| **Boss** | P0 sunum, çekirdek değil (3-5h) | hook'lar var (`PenitentSovereign:132-169`; `BossHealthBar:54-55,80-128`; `RoomMonolog:123-236`). **Shop-residue kök-neden:** `RoomRunDirector:320-322,942-946` `ShopRoomController`'ı retained-ref tutmadan spawn → temizlik çalışmıyor (`ShopRoomController:107-117`) |
| **Director** | kod-skin EVET, prefab-refactor HAYIR | cx: DirectorMode runtime UI factory (`:715-745,2945-2954`), inline callback — prefab-authored değil. ChatGPT'nin shared-prefab planı = gizli refactor tuzağı (ax Pro "en aşırı öneri" dedi, ax Flash "1 günü yer"). Gerçek yol = factory kodunda skin/anchor pass (çerçeve-çizim kaldır + font + layout + renk) |

**Director çatalı çözümü:** ax Pro (en yüksek vizyon kaldıracı) + ax Flash (tuzaktan kaçın) → **ucuz kod-skin** ikisini de tatmin eder: ax Pro vizyon kazancının ~%70'i, maliyetin ~%25'i. Tez "%60 mimari / tooling showcase" → Director merkezi kalmalı.

## YENİ RİSKLER (ChatGPT atlamış)
- 🔤 ax Pro: font büyürken **pixel-perfect/SDF netliği bozulabilir** (bulanıklık) → HUD/Director resize'da TMP/PPU netliğini koru.
- 📦 cx: standalone demo build = `DEMO_BUILD` define coverage gerek.

## NE YAPILMAYACAK (4-bakış POST'a attı)
Director shared-prefab refactor · screenshot otomasyon-harness (SHA-gate; demo için 4 eksik ekranı ELLE doğru çek) · telemetry-drawer · shared-component sistemi · Build grid/geometri redesign · yeni room/boss/skill.

---

## KİLİTLİ 1.5-GÜN SIRASI
0. **GATE — bootstrap-fix ≤2h** (tutmazsa _Arena runbook + DEMO_BUILD).
1. **Capture-truth fix** — 4 eksik ekranı (09 Stats/19 CharSheet/20 Draft/21 RunMap) ELLE doğru çek; `activeInHierarchy` göz-doğrula. (otomasyon POST)
2. **Boss P0** — shop-residue cleanup (retained-ref) + sprite scale/pivot/PPU + health-bar (stone-frame/crimson, %66/%33 notch) + subtitle güvenli-alan.
3. **HUD** — HP 200-220×14-16, resource 150-170×8-10, slot 44-56; SDF-netlik koru. Low-HP full-screen wash → kenar vignette %12-18.
4. **Black-blob** — kapı/koyu-düşman rim-light + iç değer.
5. **Director kod-skin** — çerçeve-çizim kaldır + font≥12px + IDE-dock layout (`director_mode_proposed_layout.png` referans), callback/logic AYNI. assert 6/6 koru.
6. **Build polish** (zaman kalırsa) — grid Low/Normal/High + hover-cell + footprint + valid/invalid-reason + status-bar. assert 8/8 koru. **grid geometri/oda-dışı uzama DOKUNMA.**
7. **Draft synergy metni** — "X ile eşleşir" → exact trigger+payoff ("Iron Charge sonrası çekilen hedefler 1.5sn sersemler").

**Routing:** execute = cx dispatch (tek serial Unity-ajan; builder-opus socket-öldü). audit = auditor-opus/cx (writer≠reviewer). Her Unity-dispatch: read_console 0-error + assert-rerun. Council read-only kuralı korundu (bu oturum ax uslu).
