ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Kullanıcının playtest screenshot'larından ChatGPT'nin çıkardığı 7 bug iddiasını GERÇEK KODLA DOĞRULA. Her iddia için file:line ile CONFIRMED / REFUTED-STALE / PARTIAL / NEEDS-RUNTIME ver. ANALİZ ONLY — kod değişikliği YOK. Sonucu CODEX_DONE.md'ye yaz.

# KRİTİK BAĞLAM — son fix'ler bazı bug'ları STALE yapmış olabilir
Bu oturumda commit'lenen ilgili düzeltmeler (screenshot'lar bunlardan ÖNCE çekilmiş olabilir — DOĞRULA):
- `d96e86f9` chamber rework-2: dual-system FIX (MainMenuScreen duplicate bootstrap kaldırıldı, `_IsoGame`→`_Arena` route, CharacterSelectScreen/Controller), 5+5 silüet, [G]-interact kapı, ölümsüz dummy, AttuneRoutine crash fix `:785`.
- `0f77996` playtest-1: chamber "bleed" temizliği (stale chamber root combat-load'da DESTROY), `_Arena` kapısı walk-trigger→**[G]-interact + nabız ring**, siyah entry portal, sparse oda dolumu (Combat_Large_01/Elite_01/Boss_Intro_01).
- `54558059` (T5): **ESC SkillCodexUI MVP eklendi** — yani "ESC→Codex" muhtemelen mevcut KASITLI davranış (bug değil eksik feature: PauseMenu yok).
- `3b800815` walkable enforcement: donut deliği collider, WalkabilityMap.InitFromTemplate, mob/knockback clamp.
**Bu yüzden: her bug için "screenshot eski build'den mi?" sorusunu da yanıtla.**

# DOĞRULANAN GÖRSEL (tek mevcut screenshot)
_Arena benzeri yüzen ada (koyu arduvaz floor + cyan-kristal cliff): sağ kenarda küçük player + sarımsı kılıç cliff'ten görünüyor · floor üstünde MOR/macenta diagonal çizgi (debug aim line) · 2 koyu monolit/pedestal silüeti · merkezde kare delik. Bu = ChatGPT'nin "Screenshot 2" tarifi (mor çizgi + sağa yürüyememe + kılıç sorting + küçük oda).

# DOĞRULANACAK 7 BUG (ChatGPT iddiaları)

**BUG 1 — Play→char-select bypass + arena direkt geliyor.** ChatGPT: "MainMenu akışı CharacterSelect/Chamber bypass ediyor OLABİLİR; veya kullanıcı editor'da `_Arena`'dan Play'e basıyor." DOĞRULA: `MainMenuController`/`MainMenuScreen` Play butonu hangi sahneye gidiyor? `CharacterSelectScreen.Awake` ChamberSelectBootstrap ekliyor mu? ChamberSelectBootstrap otomatik StartRun çağırıyor mu (çağırmamalı)? `_Arena` buildOnStart:true mı? EditorBuildSettings ilk sahne ne? **rework-2 bunu zaten çözdü mü = STALE mi?**

**BUG 2 — 2 mob öldürünce kilitlenme (room clear takılıyor).** DOĞRULA: `RoomRunDirector` enemy-death event dinliyor mu? aliveEnemies/kill counter 0 oluyor mu? clear→reward→exit sequence var mı? Time.timeScale=0 kalıyor mu? `_Arena` DİREKT açılınca (MainMenu'siz) RoomRunDirector düzgün init oluyor mu yoksa wiring eksik mi? Stale chamber/manager root sızıyor mu?

**BUG 3 — ESC→Yetenek Kodeksi (PauseMenu yok).** ChatGPT "kesin" diyor: `UIManager.OnEsc()` SkillCodex toggle ediyor. DOĞRULA file:line: OnEsc gerçekten ne yapıyor? PauseMenu/PauseMenuUI sınıfı VAR MI? (T5 ile ESC-codex kasıtlı eklendiyse bu "bug" değil "PauseMenu eksik feature" — netleştir.)

**BUG 4 — SkillCodex hover'da mavi bozuk tooltip text.** DOĞRULA: `TooltipSystem` paneli hangi canvas'a parent ediyor (`GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>()` mı)? `SkillCodexUI` çoklu-canvas ortamında yanlış canvas/sorting/scale riski var mı? Codex açık/kapanışta tooltip cleanup var mı? Codex satır-hover tooltip event nerede bağlı?

**BUG 5 — Skill iconları bazen yüklenmiyor (boş/kahverengi).** DOĞRULA: `SkillCodexUI`/`SkillBarUI` icon fallback mantığı (skill.icon null → RimaUITheme.PassiveIcon). Missing-icon log VAR MI? Hangi implemented skill'lerde icon referansı null? (SkillDatabase'i tara — kabaca kaç tane eksik.)

**BUG 6 — Kılıç cliff'ten görünüyor (sorting) + kılıç asset'i kötü.** DOĞRULA: weapon renderer player ile aynı SortingGroup/sortingLayer altında mı? cliff/floor sorting'in altına düşüyor mu? (Weapon audit `daeb2402`/mount-profil gated workstream'le ilişkilendir — bu zaten biliniyor mu?)

**BUG 7 — Mor çizginin sağına yürüyememe + oda küçük.** DOĞRULA: (a) MOR diagonal çizginin KAYNAĞI ne? (bir LineRenderer/Gizmo/debug aim line — hangi script çiziyor, neden runtime'da görünüyor?) (b) Aktif RoomTemplateSO walkable hücreleri görsel floor ile eşleşiyor mu? IsoRoomBuilder collider sağ kenarda görünmez duvar üretiyor mu? player/camera clamp sağ çıkıntıyı kesiyor mu? (c) screenshot'taki oda hangi template?

# ÇIKTI FORMATI
Her bug: **VERDICT** (CONFIRMED/REFUTED-STALE/PARTIAL/NEEDS-RUNTIME) + file:line kanıt + tek-cümle root-cause + (varsa) son-commit'le STALE mi notu. Sonda: gerçek-bug sayısı vs stale sayısı + en kritik 3 demo-blocker. BLOCKED yaz belirsizse.
