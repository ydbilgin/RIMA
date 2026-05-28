# Demo Skeleton Plan (S114 Session 2 scoping — Sonnet read-only audit, 2026-05-28)

**Status:** SCOPED, implementation DEFERRED (kullanıcı kararı gerek — aşağıdaki 3 karar + A5 sahnesine dokunma onayı). Faz 1 demo: Warblade + 5 oda + 4 mob + Map Fragment + Gate, ~10 dk loop. Combat çekirdeği (A2-A4) + oda-transition loop (S113) HAZIR. Bu plan graybox iskeleti kapatır (art HARİÇ).

## NE VAR (mevcut)
- **Room sequence TAMAM:** 5 `RoomSequenceData` SO wired (`RoomLoader._sequence`, PlayableArena_Test01). Room1 Tutorial(3 imp) / Room2 Medium(5) / Room3 Hard(6+elite) / Room4 Vestibule(`isRewardRoom`, fragmentDropOverride) / Room5 Boss(PenitentSovereign). Transition: `RoomTransitionFX` fade + Y-offset 40u teleport + `DemoCompleteOverlay`.
- **Mob:** tüm odalar SADECE FractureImp (`34f9366f...`). Prefab havuzu: 16 enemy_XX + 8 klasik (SeamCrawler/VoidThrall/ChainWarden/Penitent/...) — AMA fonksiyonel-doğrulanmadı.
- **Gate TAMAM (kod):** `Gate.cs` state machine (Locked/AwaitingFragment/Unlocked), `RoomLoader.BuildRoomContent` her odaya instantiate, `gatePosition` SO'dan.
- **Fragment KISMİ:** `MapFragment.cs` (G topla → DungeonGraph.RevealAhead), `MapFragmentBridge.cs` (OnRoomCleared→DraftManager→Gate.Unlock→LoadNext), `MapFragmentSpawner.cs`. Room4 Vestibule manuel override + `RewardRoomAutoTrigger`.
- **Loop kapanış:** `DemoCompleteOverlay` boss ölümünde (`RoomLoader.WireBossDeathListener` coroutine).

## NE EKSİK (graybox loop için)
1. **Gate hiç açılmıyor:** `MapFragmentBridge.useFragmentGateFlow=false` (Day1 portal flow) → combat odalarında Gate `AwaitingFragment`'te kalıyor, geçilemiyor. **Loop blocker.**
2. **Mob çeşitliliği yok:** 4 farklı mob gereksinimi karşılanmıyor (hepsi FractureImp).
3. **Combat odalarda fragment drop yok:** Room1-3 cleared → fragment düşmüyor.
4. **`MapFragmentBridge` sahne wire'ı belirsiz** (PlayableArena_Test01'de GO var mı / flag set mi).
5. **Boss death→RaiseDemoComplete** wiring test edilmemiş (PenitentSovereign.Health OnDeath).

## SIRALI PLAN (art gerektirmez)
- **Adım 1 (S, scene Inspector):** `MapFragmentBridge` GO bul/ekle → `useFragmentGateFlow=true`; `MapFragmentSpawner` ekle (`gateOnBridgeFlag=true`). → PlayableArena_Test01.unity (A5 SAHNESİ — dikkat).
- **Adım 2 (S, SO edit, scene-bağımsız):** Room2/Room3 SO'larına farklı mob assign (4 çeşit). ⚠️ ÖNCE alt-mobların combat-fonksiyonel olduğunu doğrula (AI/Health), yoksa encounter bozulur.
- **Adım 3 (S, Inspector):** Combat odalara `FragmentDropAnchor` + `fragmentDropOverride` set (Adım 1 sonrası otomatik). VEYA basitleştir: clear→drop, gate fragment bekler.
- **Adım 4 (S, playtest):** PenitentSovereign.prefab Health OnDeath → RaiseDemoComplete doğrula.

## ⚠️ KARARLAR (kullanıcı — bunlar olmadan implementasyon başlamaz)
1. **Fragment-gated mi, clear-gate mi?** Demo adı "Map Fragment + Gate" → muhtemelen fragment-gated (clear→fragment→topla→gate açılır). ONAY GEREK.
2. **A5 sahnesine dokunma onayı:** Adım 1/3/4 PlayableArena_Test01'i değiştirir = senin A5 combat-feel playtest sahnen. İzole etmek istersen `PlayableArena_Demo_Skeleton.unity` duplicate'i ile ayrı çalışılabilir (A5 testi temiz kalır). Tercih?
3. **4 mob hangileri + fonksiyonel mi?** SeamCrawler/VoidThrall/ChainWarden/Penitent prefabları AI/combat-ready mi, yoksa FractureImp variant'ları mı kullanılsın?

## Loop riskleri (implementasyonda ele alınacak)
- Boss room'da hem Gate→LoadNext hem boss-death→RaiseDemoComplete tetiklenirse → sonsuz loop / 2x overlay. Guard gerek (`CurrentRoomIndex >= Length-1`).
