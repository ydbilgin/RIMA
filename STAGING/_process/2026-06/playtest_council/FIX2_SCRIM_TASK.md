# FIX-2: Overlay scrim / modal-stack (B3 — DEMO-KRİTİK)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical — sadece overlay/scrim, ilgisiz refactor YOK (4) BLOCKED yaz çözemezsen.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, önceden-var/ilgisiz hatayı BİLDİR; raporda console durumu.
GRAPHIFY: UI sınıfları çok-dosya → gerekirse `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json` query (~71x ucuz).

Tam bağlam: `STAGING/PLAYTEST_FIX_MASTER_SPEC_2026-06-16.md` → FIX-2. Council: `STAGING/_process/2026-06/playtest_council/RESP_{cx,axflash}.md` §B3.

## SORUN (canlı playtest)
M (run-map) draft AÇIKKEN basılınca **draft kartları run-map'in arkasından sızıyor** → node'lar + kart yazıları üst üste = "karışıklık". Ayrıca Settings/Pause/Director/Codex overlay'lerinde arka-plan dim/scrim yok → bleed.

## İKİ AYRI KÖK (DİKKAT):
1. **Canvas modalleri** (SkillOfferUI/draft, PauseMenuUI, DirectorMode, SkillCodexUI, SettingsMenuUI): paylaşılan **`UI_Scrim_Dimmer`** (full-screen siyah ~%75 alpha, raycast-block Image) modal açılınca ALTINA otomatik eklensin + modal `Canvas`/`sortingOrder` en üste. Modal kapanınca scrim kapansın. LIFO modal-stack mantığı `UIManager.cs`'te (zaten merkezi UI yöneticisi).
2. **RunMapOverlay** (`Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs`) = **IMGUI/OnGUI** (Canvas DEĞİL). Scrim prefab buna UYMAZ. Bunun kendi backdrop'u var (l.51 `DrawRect(... new Color(0.03f,0.02f,0.06f, 0.88f))`). Draft kartları (Canvas) IMGUI altından sızıyor çünkü 0.88 opak değil. **FIX:** (a) backdrop alpha 0.88→**0.99** yap (kartlar görünmesin), VE/VEYA (b) M açılınca draft Canvas'ı (SkillOfferUI) geçici GİZLE, M kapanınca geri göster. (a) daha basit+yeterli olabilir — ikisini de değerlendir, en temizini uygula.

## YAPILACAK
- Yeni prefab/runtime obj: `UI_Scrim_Dimmer` (Canvas üstü full-screen Image, siyah a=0.75, RaycastTarget=true). UIManager'a `PushModal/PopModal(canvas)` veya basit `ShowScrim()/HideScrim()` API'si + sortingOrder yönetimi.
- Her modal aç/kapa noktasına scrim çağrısı (Draft/Pause/Director/Codex/Settings). Mevcut open/close metodlarını KULLAN.
- RunMapOverlay bleed fix (yukarıda).
- Min kod, demo-scope. Sıfırdan UI framework yazma.

## VERIFY
- Compile **0-error** (read_console).
- Görsel verify (scrim/bleed) = orchestrator screenshot yapacak (sen compile + mantık doğrula, gerekirse execute_code ile scrim objesi sahnede oluştu mu kontrol et).
- DÖNÜŞ ≤10 satır: değişen dosyalar + ne yapıldı (Canvas-scrim yaklaşımı + run-map bleed çözümü) + compile/console durumu. Diff'i gömme.
