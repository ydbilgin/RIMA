ACTIVE RULES: (1) think before answering (2) concrete file:line (3) flag real bugs/regressions (4) UNSURE if can't verify.

# GÖREV — W1 kod batch review (code correctness)
Opus 4 değişiklik yaptı (uncommitted). Sen YAZMIYORSUN — review + regression flag.

İncele:
1. **`Assets/Scripts/Skills/DraftManager.cs`** EnsureDependencies — SkillDatabase auto-create eklendi (Instance==null && FindAnyObjectByType==null → new GameObject+AddComponent). Doğru sıra mı (ShowDraft generator'dan ÖNCE)? Singleton guard güvenli mi? Play-test: SkillDB=63 skill, draft açıldı (DOĞRULANDI).
2. **`Assets/Scripts/Player/PlayerAttack.cs`** Update — `behavior` null-self-heal (domain-reload sonrası profilden yeniden yarat). NRE-spam fix. Mantık doğru mu, side-effect var mı?
3. **`Assets/Scripts/Enemies/EliteAffix.cs`** Shielded — `ScaleMaxHP(int-div→x1)` bug'ı → `SetMaxHP(RoundToInt(MaxHP*1.3f))`. Doğru fix mi? SetMaxHP currentHP'yi de fulle set ediyor (spawn'da sorun değil) — onay?
4. **`Assets/Scripts/UI/Map/MapProgressController.cs`** (YENİ) — orphan MapPanelUI'yi RoomLoader.OnRoomChanged'e bağlar, self-bootstrap Canvas+panel, M-toggle (Keyboard.current), flash-on-transition. Kontrol: OnRoomChanged signature (int) doğru mu? Input System kullanımı asmdef'te referanslı mı (PlayerAttack zaten kullanıyor)? FindFirstObjectByType(FindObjectsInactive.Include) Unity 6 doğru mu? MapPanelUI.Awake placeholder-show ile çakışma? Leak/coroutine guard?

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED
- Her değişiklik: AGREE / ISSUE (+ file:line + fix önerisi)
- Regression riski / asmdef-Input riski
- compile-blocker var mı
