# F2 "reward→kart çıkmıyor" — ROOT-CAUSE DECISION (2026-06-15)

> Council: cx (Codex/yekta) + ax 3.5 Flash. ax 3.1 Pro TIMED OUT (480s, yanıt yok — yeniden dispatch edilmedi: cx+Flash+canlı repro yeterince yakınsadı). Synthesis: Opus 4.8 (orchestrator).
> Yöntem: 2 bağımsız canlı repro (_Arena-direct play + execute_code) + kod-temelli council. Repro sonrası Unity temiz (playModeStartScene=MainMenu restore, debug leak yok).

---

## KARAR: Golden-path GREEN — **0 kod fix.** F2 = Forge/Echo için "bilinen limitasyon".

Edit-to-Play video golden-path segmenti (storyboard 1:25-1:50) **F2 fix'ine BAĞLI DEĞİL** — reward→kart zinciri ilk combat odasında (depth=1) zaten uçtan uca çalışıyor.

---

## KANIT

### Canlı repro 1 — ShowDraft izolasyon (room=1, auto-managers)
`DraftManager.Instance.ShowDraft()` doğrudan → `SkillOfferPanel` canvas (ScreenSpaceOverlay, sort=1050, enabled) + 6 buton, `IsDraftActive=True`, hiçbir early-return logu. → **kart-render çalışıyor.**

### Canlı repro 2 — gerçek reward + full collect zinciri (DECISIVE)
`RoomRunDirector.SpawnRewardPickup()` (reflection, gerçek prefab+placement) → `RewardPickup@(1.9,6.7)` → `reward.ForceCollect()` →
- `WasCollected=True` (Collect() fire etti)
- `IsDraftActive=True` (ShowDraft çalıştı)
- `SkillOfferUI: canvas=[SkillOfferPanel] enabled=True ScreenSpaceOverlay **buttons=3**` (3 kart render)

→ **`Collect → DraftThenOpenExit → ShowDraft → SkillOfferUI.Show → 3 kart` depth=1'de UÇTAN UCA çalışıyor.**

### Kod kanıtı (cx, file:line)
- Demo `graph.startId=0` (`DungeonGraph.cs:88-92`); draft depth = `CurrentNodeId+1` (`DraftManager.cs:162-171`) → **ilk reward odası depth=1**, Forge 4/8 altı (`DraftManager.cs:52-53,211-224`).
- Echo (oda 3) sadece offer değiştirir, UI bastırmaz (`DraftManager.cs:344-357`).
- `EnsureDraftManager` reward-spawn'dan önce `DraftManager_Auto` kurar (`RoomRunDirector.cs:1374,1506`) → aday #1 (Instance null) savunulmuş.
- `_Arena` RoomRunDirector varken `RuntimeRoomManager.CurrentRoom` KULLANMIYOR (`RuntimeRoomManager.cs:24-26`) → harness'taki RRM=null/room=1 gerçek ilk odayı temsil etti.
- Auto-collect KAPALI (`RewardAutoCollectTimeoutSec=0`, `RoomRunDirector.cs:1187-1193`) → reward menzilde-G ile toplanır (`RewardPickup.cs:15,62-68`).

## ELENEN ADAYLAR
- ❌ #1 Instance null — EnsureDraftManager savunuyor.
- ❌ #3 dep-null — EnsureDependencies + SkillOfferUI.Show self-bootstrap.
- ❌ render hatası — panel/kart kuruluyor.
- ❌ #2 Forge early-return — golden-path depth=1, Forge'a girmez.
- ❌ #5 chest Echo case — golden-path chest'ten kaçar.

## TEK AÇIK CAVEAT (honest)
ForceCollect literal **G-tuşu + player-in-range** gate'ini bypass eder (`RewardPickup.Update/OnTriggerEnter2D`, RewardPickup.cs:62-76). Bu gate standart (collider.isTrigger=true RewardPickup.cs:30-31, Player-tag check L72) ve kullanıcı kayıtta elle yapacak. **Video OBS provası (10×) bunun pratikte çalıştığının nihai teyidi olacak.**

## SONUÇ / AKSİYON
1. ✅ F2 golden-path için kapandı — **kod değişikliği YOK** (over-engineering'den kaçınıldı; sunum ~20 Haz zamanı F2'ye harcanmadı).
2. F2 = Forge(4/8)/Echo odaları için bilinen limitasyon (post-demo, bespoke).
3. Storyboard golden-path segmenti **tam çekilebilir** (fallback slayt gereksiz, F2-dependency kalktı).
4. Video provasında G-collect pratik teyidi yapılacak.
