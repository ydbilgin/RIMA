# TASK 0 — GATE: full-flow Director/F2 bootstrap fix (≤2h time-box)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: RIMA design context gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / kod / STAGING / memory.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`), bulk-read'den ~71x ucuz.

## Bağlam (oku)
Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md`. Senin önceki kök-neden analizin: `STAGING/_process/2026-06/chatgpt_review_council/RESP_cx.md` (Q6).

## Sorun
Demo menüden başlayınca (MainMenu→CharacterSelect→oyun) **DirectorMode bootstrap OLMUYOR** → backquote (`) ölü + F2 (Build Mode, `DirectorMode.Instance`'a sıkı bağımlı) ölü. Senin bulduğun kök-neden:
- `Assets/Scripts/UI/DirectorMode.cs:143-177` — MainMenu/CharacterSelect'i atlayan scene-guard.
- `Assets/Scripts/UI/BuildModeController.cs:223-228` — `DirectorMode.Instance` hard-dependency.

⚠️ **ax bir kez denedi (sceneLoaded-hook ekledi) ve VERIFY'da ÇALIŞMADI** — DirectorMode full-flow'da yine kurulmadı, hook tetiklenmedi. Yani yüzeysel "hook ekle" YETMİYOR. **ÖNCE neden tetiklenmediğini kök-nedenle** (lifecycle/ordering/singleton-revival/guard-mantığı), sonra minimal fix.

## Hedef (başarı kriteri — RUNTIME, compile DEĞİL)
MainMenu'den başlayıp normal oyun-akışıyla bir oda'ya girildiğinde:
1. `DirectorMode.Instance != null` (bootstrap oldu),
2. backquote (`) Director'ı açıyor,
3. F2 Build Mode'u açıyor (DirectorMode.Instance'a erişebiliyor).

## ⚠️ VERIFY — ZORUNLU RUNTIME KANITI (ax'in hatası tekrar etmesin)
Compile-0-error YETMEZ. Şunu KANITLA:
- `manage_editor` ile `playModeStartScene=MainMenu` ayarla → play → full-flow ile bir room'a gir (mümkünse otomatik; değilse en yakın deterministik yol).
- `execute_code` ile çalışma-anında assert: `DirectorMode.Instance` null mu, BuildModeController F2'ye yanıt veriyor mu → compact string döndür.
- Mümkünse 1 screenshot (scene_view) + assert çıktısını evidence dosyasına yaz.
- Çalışıyorsa: kanıtla. **2h içinde runtime'da çalışmıyorsa → `BLOCKED` yaz + "_Arena runbook'a düş" öner** (sessizce partial bırakma).

## Kısıt
- **Cerrahi:** sadece DirectorMode.cs + (gerekirse) BuildModeController.cs + bootstrap-ilgili 1-2 dosya. İlgisiz refactor YOK.
- Mevcut spawn/stat/telemetry/assert mantığına DOKUNMA — sadece bootstrap/erişim.
- `DEMO_BUILD`/`UNITY_EDITOR` define farkını NOT düş (cx kendi uyarın: standalone'da define gerekebilir) ama bu task'te editör-runtime yeterli; over-scope etme.
- git'e DOKUNMA (commit/add YOK).

## ÇIKTI (E1: dönüş ≤10 satır)
Evidence + detay → `STAGING/_process/2026-06/demo_fix_tasks/DONE_0_GATE.md`. Dönüşte SADECE: değişen dosyalar + runtime-verify sonucu (PASS/BLOCKED + kanıt) + console durumu + kalan risk. Rapor içeriğini dönüşe gömme.
