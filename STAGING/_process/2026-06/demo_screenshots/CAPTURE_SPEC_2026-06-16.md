# SCREENSHOT CAPTURE AUTOMATION SPEC — crafter-sonnet (2026-06-16)

ACTIVE RULES: (1) think before acting (2) min code (3) surgical — only capture, NO source/scene edits (4) BLOCKED if a step fails 2x, log it and continue to next screen.
UNITY ERROR CHECK: iş bitince read_console (Error); kendi hatanı çöz, önceden-var/ilgisiz hatayı raporda BİLDİR.
NLM ACCESS gerekmez (mekanik capture).

## AMAÇ
RIMA'nın TÜM oyuncu-bakan ekranlarını + dev/Director modlarını tek tek screenshot'la → ChatGPT'ye güzelleştirme incelemesi için görsel envanter. **Sen SADECE yakala; kaynak/scene DEĞİŞTİRME.** Screenshot'lar `STAGING/_process/2026-06/demo_screenshots/` klasörüne, aşağıdaki adlarla.

## ARAÇLAR
- `manage_editor` (play/stop), `manage_camera` (screenshot), `execute_code`, `read_console`, `manage_scene`.
- **Screenshot komutu (her ekran için):**
  `manage_camera(action="screenshot", output_folder="STAGING/_process/2026-06/demo_screenshots", screenshot_file_name="<AD>.png", include_image=false, max_resolution=1280)`
  → `include_image=false` (token tasarrufu; orchestrator dosyaları sonra okur). Varsayılan ScreenCapture = overlay UI'yi YAKALAR.
- **execute_code KRİTİK kuralı:** kod bir METOD GÖVDESİ olarak derlenir → **üst-seviye `using` YOK**. Tip adlarını TAM-NİTELİKLİ yaz: `UnityEngine.GameObject.FindObjectOfType<...>()`, `UnityEngine.UI.Button`, `TMPro.TextMeshProUGUI`. `return "<özet>";` ile sonuç döndür.
- Her play-mode sahne geçişi/overlay açılışından sonra screenshot'tan ÖNCE 1 no-op tool call (read_console) ile bir frame ilerlet.

## SESSION 1 — FULL-FLOW (playModeStartScene = MainMenu, mevcut ayar; DEĞİŞTİRME)
1. `manage_editor play` → MainMenu yüklenir. read_console (0 error doğrula). Screenshot **`01_mainmenu`**.
2. AYARLAR menüsünü aç: execute_code ile label'ı "AYAR" içeren Button'u bul + `onClick.Invoke()`. Screenshot **`02_settings`**. (geri dönebiliyorsan dön; dönemiyorsan stop+yeniden play.)
3. BAŞLA: label'ı "BA" ile başlayan Button'u bul + Invoke → CharacterSelect sahnesi yüklenir (read_console ile frame ilerlet). Screenshot **`03_characterselect`**.
4. Mümkünse bir sınıf seç + ilerle (ChamberSelect yüklenirse Screenshot **`04_chamberselect`**; _Arena gameplay yüklenirse **`05a_fullflow_gameplay`**). Takılırsan LOG'la, Session 2'ye geç.
5. `manage_editor stop`.

## SESSION 2 — DEV-DIRECT (_Arena: Director/BuildMode/RunMap burada bootstrap olur)
**playModeStartScene'i _Arena yap, SONUNDA MainMenu'ye geri al (debug-leak yok):**
- Başta: execute_code →
  `var sa = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>("Assets/Scenes/_Arena.unity"); UnityEditor.SceneManagement.EditorSceneManager.playModeStartScene = sa; return "set _Arena";`
- `manage_editor play`. read_console (0 error). buildOnStart ilk odayı kurar + açılış kit-draft'ı açılır.
6. Screenshot **`06_opening_draft`** (açılış reward/kit draft kartları + HUD).
7. Draft'ı kapat: execute_code ile DraftManager/SkillOfferUI bul, ilk seçeneği seç (public bir "Select/Choose/Pick(0)" metodu veya görünür ilk kart Button'unu Invoke). Screenshot **`07_gameplay_hud`** (temiz arena + oyuncu + HUD barları).
8. Run-map: execute_code → `var o=UnityEngine.GameObject.FindObjectOfType<RIMA.MapDesigner.Room.Runtime.RunMapOverlay>(); o.Toggle(); return o!=null?"toggled":"null";`. Screenshot **`08_runmap`**. (sonra tekrar Toggle ile kapat.)
9. Director: execute_code ile DirectorMode bul + aç (public toggle metodu ara: `Show/Toggle/SetActive`; bulamazsan backquote tuş enjekte et veya `DirectorMode.Instance` üzerinden). Screenshot **`09_director`**. (kapat.)
10. Build Mode: execute_code ile BuildModeController bul + EnterBuildMode/Toggle çağır (draft KAPALI olmalı). Screenshot **`10_buildmode`**. (çık.)
11. Skill Codex: execute_code ile SkillCodexUI/UIManager bul + codex toggle (ESC davranışı). Screenshot **`11_codex`**.
12. (opsiyonel, kolaysa) PassiveStatusUI/SkillBar yakın-çekim, CharacterSheetUI. Ulaşılamıyorsa LOG.
- `manage_editor stop`.
- **GERİ AL:** execute_code →
  `var sa = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>("Assets/Scenes/MainMenu.unity"); UnityEditor.SceneManagement.EditorSceneManager.playModeStartScene = sa; return "restored MainMenu";`
  (MainMenu.unity yolu farklıysa Build Settings'ten ilk sahneyi bul.)

## ULAŞILMASI ZOR (deneme; takılırsan ATLA + LOG): ChestUI (chest oda), ForgeUI (forge oda), BossHealthBar (boss), DeathScreenManager (ölüm), DemoCompleteOverlay (run bitişi). Bunlar oda-tipi/duruma bağlı; golden-path'te kolay değilse atla.

## RAPOR FORMATI (dönüşün ≤15 satır + dosya listesi)
- Yakalanan dosyalar: `<AD>.png | <ekran adı> | nasıl ulaşıldı | sorun(varsa)` — her satır.
- Ulaşılamayanlar + neden.
- read_console final durumu (error sayısı + kendi-hatası mı).
- playModeStartScene MainMenu'ye geri alındı mı (EVET/HAYIR).
**Ekran görüntülerini DÖNÜŞE GÖMME** (orchestrator dosyaları okuyacak). Sadece özet + liste.
