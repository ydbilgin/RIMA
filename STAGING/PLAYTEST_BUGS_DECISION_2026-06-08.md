# Playtest Bug Pack (ChatGPT) — COUNCIL DOĞRULAMA + FIX PLANI (2026-06-08)

**Girdi:** Kullanıcının 4 playtest screenshot'ı + ChatGPT repo-kontrol raporu (`STAGING/_inbox/playtest_bug_pack_2026-06-08/`). ChatGPT 7 bug çıkardı.
**Yöntem:** Council — cx (kod-doğrulama, file:line, linchpin) + ax-3.1-Pro (fix mimarisi) + ax-3.5-Flash (demo triyaj/over-engineering). Opus sentez.
**Ana sonuç:** **7 iddianın 3'ü STALE/yanlış-sınıflandırma** (son commit'lerle düzelmiş veya feature-gap), 2'si gerçek-kod-bug, 2'si runtime-repro gerektiriyor. ⚠️ Flash spesifik file:line verdi (RoomDebugGizmo magenta, PropColliderAutoBuilder) ama **cx bunları DOĞRULAMADI** → güvenme, canlı kontrol.

---

## 7 BUG — cx doğrulama tablosu

| # | Bug | cx VERDICT | Özet |
|---|---|---|---|
| 1 | Play→char-select bypass, arena direkt | **STALE** | `MainMenuController.cs:15,36-38` → CharacterSelect'e gidiyor; `CharacterSelectScreen.cs:168-177` ChamberSelectBootstrap ekliyor; bootstrap StartRun ÇAĞIRMIYOR. `d96e86f9` MainMenu yolunu düzeltmiş. **Kullanıcı muhtemelen editör'de `_Arena`'dan Play'e bastı** (`_Arena` buildOnStart:1 = kendi kendine başlar). Build'de gerçek bug DEĞİL. |
| 2 | 2 mob öldürünce kilitlenme | **NEEDS-RUNTIME** (clear-code iddiası REFUTED) | Clear pipeline TAM VAR: `EncounterController.cs:142-175` (enemy death→activeEnemies→OnAllEnemiesDead) → `RoomRunDirector.cs:520-529,727-817` (clear→slowmo→reward→draft→kapı→timeScale restore). Kilitlenme varsa = **2. wave kalmış** veya runtime spawn/tracking mismatch veya _Arena-direct context. Canlı repro şart. |
| 3 | ESC→Yetenek Kodeksi (PauseMenu yok) | **PARTIAL** (davranış CONFIRMED, "bug" REFUTED) | `UIManager.cs:94-103,137-150,207-222` ESC→SkillCodex toggle = `54558059`/T5 KASITLI MVP. Canlı PauseMenu sınıfı YOK. → Regression değil, **feature-gap (PauseMenu ekle).** |
| 4 | SkillCodex hover mavi bozuk tooltip | **PARTIAL (gerçek risk)** | `TooltipSystem.cs:41-49` tooltip'i `GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>()`'a parent ediyor; `SkillCodexUI.cs:45-59` kendi canvas'ı var (sortingOrder 1095). → **Yanlış-canvas/sorting mismatch riski GERÇEK.** Cleanup var (`:79-86`). Tam görsel runtime-repro. |
| 5 | Skill iconları boş/kahverengi-mor | **CONFIRMED** | `SkillBarUI.cs:276-285` icon null→sprite null + renk (0.35,0.25,0.55) **fallback YOK + log YOK** (Codex/draft'ta fallback VAR: `SkillCodexUI.cs:336`, `SkillOfferUI.cs:581-592`). + registry sadece **19/111** skill kapsıyor (100 eksik). → **GERÇEK BUG.** |
| 6 | Kılıç cliff'ten görünüyor (sorting) + asset | **CONFIRMED (sorting)** / asset subjektif | `HandAnchorAttach.cs:199-205` weapon sortingOrder sadece body'ye göre ±1; `OrientationSync.cs:52-57` SortingGroup/terrain-depth YOK; cliff'ler `IsoRoomBuilder.cs:451-459` cliffSortingLayer'da. → Weapon depth terrain-aware DEĞİL = **bilinen/gated** mount-profil eksikliği (`daeb2402` audit). |
| 7 | Mor çizgi + sağa yürüyememe + küçük oda | **NEEDS-RUNTIME / PARTIAL** | **Kalıcı mor "debug aim line" kaynağı statik bulunamadı** (adaylar kısa-ömürlü: SequentialStrike/ChainBinder line). Walkable kod kasıtlı non-walkable hücre blokluyor (`WalkabilityMap.cs:199-213,295-320`). → "Görsel floor'da yürüyememe" = seçili DemoRoomBank combat template'inde (index 0, seed 12345) **walkable-grid/görsel-floor mismatch** olası; mor çizgi canlı sahne incelemesi ister. |

**Totaller (cx):** Gerçek kod-bug: 3 (Bug 4 risk · Bug 5 · Bug 6 sorting) · Stale/feature-gap: 3 (Bug 1 · Bug 2 clear-code · Bug 3) · Runtime-repro: 2 (Bug 2 semptom · Bug 7).

---

## SENTEZ — kararlar

**🔑 En büyük içgörü:** Kullanıcının "Play→char seçmeden arena→2 fare→kilit" zinciri büyük olasılıkla **tek kök:** editör'de `_Arena` sahnesi açıkken Play'e basmak (kendi kendine başlıyor). Bu durumda char-select yok (normal), combat direkt (normal), ve kilit = o context'teki wave/tracking. **İLK ADIM = launch yöntemini netleştir** (MainMenu sahnesinden mi Play? yoksa _Arena'dan mı?). Bu, Bug 1 + Bug 2'nin büyük kısmını çözer.

**Triyaj çelişkisi (Flash: Bug 2/3/7 blocker ↔ cx: Bug 5/6/7 blocker) çözümü:** İkisi farklı şeyde haklı. cx kod-gerçeğini gösterdi (1/2/3 büyük ölçüde stale); geriye kalan GERÇEK demo-etkisi = görünür-kalite bug'ları (5/6) + runtime-mismatch (7). 

**Over-engineering (Flash + cx teyit):** Clear pipeline ZATEN var → yeni RoomStateMachine YAPMA. PauseMenu = 10-satır MVP (modal-stack DEĞİL). Tooltip = codex-canvas'a parent et (dedicated-canvas sistemi şart değil). Weapon = SortingGroup (pipeline rewrite DEĞİL).

---

## FIX PLANI (sıralı)

### Adım 0 — RUNTIME REPRO (kullanıcı veya Unity MCP, kod yazmadan)
- Launch yöntemini doğrula: MainMenu sahnesinden Play → char-select/chamber geliyor mu? (Bug 1 stale teyidi)
- _Arena'da combat: kaç mob/wave? Son mob ölünce clear/reward/kapı geliyor mu, gerçekten kilitleniyor mu? (Bug 2)
- Mor çizgi ne zaman çıkıyor (combat başında mı, skill kullanınca mı)? Sağ kenarda görünmez duvar mı var? (Bug 7)
→ Bu olmadan Bug 2 & 7 "kesin fix" yazılamaz.

### Adım 1 — Cheap + cx-confirmed (düşük risk, hemen)
- **Bug 5:** `SkillBarUI.cs:276-285`'e Codex/draft ile aynı `RimaUITheme.PassiveIcon` fallback'i + `[SKILL_ICON_MISSING]` warning ekle. (registry 19/111 genişletme = ayrı/sonra)
- **Bug 6 (sorting):** Player root'a `SortingGroup` ekle → kılıç gövdeyle birlikte cliff'e karşı sort'lansın. (Asset yeniden-üretim = gated weapon seansı, ayrı.)
- **Bug 4:** Tooltip'i ilk-bulunan-canvas yerine `SkillCodexUI` canvas'ına parent et (veya codex açıkken dedicated parent).

### Adım 2 — Feature-gap (orta)
- **Bug 3:** Minimal **PauseMenu MVP** — ESC → Time.timeScale=0 + basit panel (Resume / Skill Codex / Settings / Exit to Menu). Codex'i bu panelin butonuna taşı; ESC default = Pause. (Full art/modal-stack ERTELE.)

### Adım 3 — Runtime-bağımlı (repro sonrası)
- **Bug 2:** Repro'da kilit doğrulanırsa: wave config + activeEnemies tracking + draft/timeScale=0 takılması kontrol → cerrahi fix.
- **Bug 7:** Mor çizgi kaynağını canlı bul + kapat; DemoRoomBank index-0 template walkable-grid'ini görsel-floor'la eşitle (gerekirse oda büyüt).

### ERTELE (demo sonrası)
- Kılıç asset yeniden-üretimi (gated PixelLab seansı) · SkillIconRegistry 19→111 tam kapsama · oda boyut/design pass.

---

## ⚠️ Notlar / caveat
- **Zip'te 4 screenshot'tan sadece 1'i var** (`04_..._hover_bug` ama içerik = Screenshot 2: mor çizgi+kılıç+küçük oda). Diğer 3 görsel ChatGPT metin-tarifinden. Görsel teyit için kullanıcı 3 görseli ayrıca atmalı.
- **Flash file:line iddiaları (RoomDebugGizmo.cs:80 magenta, PropColliderAutoBuilder.EnsureDefaultLayer) cx tarafından doğrulanmadı** → uydurma olabilir; canlı kontrol şart. (PropColliderAutoBuilder/EnsureDefaultLayer RIMA backlog'unda gerçek bir not AMA cx onu Bug 7 kökü olarak göstermedi.)
- Council ham çıktıları: `STAGING/_process/2026-06/_council_*playtest-bugs.md` + `CODEX_DONE_yekta.md`. ChatGPT pack: `STAGING/_inbox/playtest_bug_pack_2026-06-08/`.
