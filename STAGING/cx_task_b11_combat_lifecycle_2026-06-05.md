ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
**B-11 COMBAT LIFECYCLE** — oynanabilir döngünün kalbi (roadmap Oturum A, karar=`STAGING/PLAYABLE_ROADMAP_DECISION_2026-06-05.md`):
oda → wave-combat → clear → slow-mo → ödül → kapılar karanlık→cyan → kapıya YÜRÜ → sonraki oda → … → ölüm/zafer.

# Mevcut altyapı (REUSE — çoğu hazır; kendi eski raporların + GECE·4 status)
`_Arena` foundation (`531bbdd0`): gerçek Warblade player + 3 düşman + kamera-fit ÇALIŞIYOR. `RoomRunDirector`
(node-bazlı, `Graph`, `CurrentChoices`, `AdvanceTo(i)`, `BuildCurrentRoom`+teleport+`BuildExitDoors`) ·
`DungeonGraph.Generate(seed, depthCount)` · `IsoRoomBuilder.BuildExitDoors` kapı GO listesi döndürüyor ·
`EncounterController`/`EncounterWaveSO`/`ThreatBudget`/`EncounterBankSO` VAR · `RoomTemplateSO.enemySpawnSockets` ·
`RewardPickup` (G-toplama) + `DraftManager.ShowDraft` · `RoomClearVictoryTrigger` telafi-ağlı mob sayacı ·
`EchoWallet.AwardRunIfNeeded` + Death ekranı "+n SHATTERED ECHO" · Chamber (`ChamberSelectBootstrap`) kapısı
şu an `_IsoGame`e gidiyor.

# KURALLAR
KOD-ONLY (.unity disk-düzenleme YASAK; _Arena'ya runtime ekleme serbest). "◈" yasak. Cerrahi: lifecycle dışı
refactor yok. Düşman görselleri placeholder kalır (üretim kullanıcıyla).

# İŞ (sıralı)
1. **Encounter wiring (_Arena):** oda kurulunca `RoomTemplateSO.enemySpawnSockets`'tan EncounterController ile
   wave spawn (EncounterBankSO+ThreatBudget; bank'te eksik tip varsa default-combat fallback). Wave-ilerleme:
   mevcut `nextWaveKillFraction` kullan.
2. **Clear tespiti:** son düşman ölünce (RoomClearVictoryTrigger pattern'i / telafi ağı dahil) → `RoomCleared` event.
3. **Slow-mo blip:** clear anında kısa timeScale dip (≈0.3 → 0.6s'de geri; `HitStop`/Time pattern'leriyle çakışma
   guard'ı — timeScale'i KENDİ restore et).
4. **Ödül:** clear → oda merkezine `RewardPickup` spawn (G ile topla → `DraftManager.ShowDraft` 3-kart akışı;
   draft kapanınca devam). Ödül alınana kadar kapılar KİLİTLİ.
5. **Kapı durumları:** `BuildExitDoors` dönen kapı GO'ları başta KARANLIK tint; ödül sonrası CYAN + collider-trigger
   aktif. Kapıya yürüyüp girince `AdvanceTo(index)` → yeni oda build + player teleport + yeni wave. (Kapı sayısı =
   graph dal sayısı — mevcut davranış korunur.)
6. **Run sonu:** Boss/çocuksuz node temizlenince → victory akışı (DemoCompleteOverlay) · player ölünce →
   DeathScreen (echo award zaten bağlı). İkisinden de MainMenu'ye dönüş çalışsın.
7. **Chamber entegrasyonu:** Chamber kapısı `_IsoGame` yerine **_Arena run akışını** başlatsın (RoomRunDirector
   BeginRun; depthCount=4-5 demo). `_IsoGame` yolu BOZULMASIN (fallback olarak dursun — sadece chamber'ın hedefi değişir).
8. **EditMode test (1 dosya, 2-3):** lifecycle state-machine geçişleri (Combat→Cleared→RewardTaken→DoorOpen→Advance)
   saf-class seviyesinde.

# Doğrulama (faz faz kanıt; play-observe)
dotnet build PASS + console 0 + canlı: wave spawn sayısı → tüm düşmanları öldür (execute_code Damage ile) →
slow-mo gözlendi → RewardPickup var → G/collect simüle → draft açıldı/kapandı → kapı tint karanlık→cyan →
kapı trigger → yeni oda kuruldu + player teleport → 2. odada tekrar → boss node'da victory → ölüm yolunda
DeathScreen+echo. CODEX_DONE.md'ye madde madde. BLOCKED olursan nedenini yaz, kalan fazlara geçme.
