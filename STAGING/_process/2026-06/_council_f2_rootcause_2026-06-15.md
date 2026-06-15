# COUNCIL: F2 "reward→kart çıkmıyor" — gerçek kök neden + golden-path etkisi

ACTIVE RULES: (1) think before answering (2) min/surgical fix, no speculation (3) evidence-grounded — cite file:line (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.

## BAĞLAM
RIMA demo bug F2: oyuncu room reward'ı topluyor → 3-kart skill draft AÇILMIYOR ("kart çıkmıyor"). Edit-to-Play sunum videosunun golden-path segmenti (1:25-1:50) buna bağlı. Golden-path = temiz MainMenu→_Arena, **İLK Combat odası**; Forge(oda 4/8), Echo, chest yollarından KAÇINIR.

## AKIŞ (cite'lı)
- `RewardPickup.Collect()` (Core/RewardPickup.cs:96) → `StartCoroutine(DraftThenOpenExit())` (L112).
- `DraftThenOpenExit()` (L171): `DraftManager.Instance != null` ise `draft.ShowDraft()` (L176), sonra `IsDraftActive` false olana kadar bekler (90sn guard), sonra kapıları açar.
- `DraftManager.ShowDraft()` (Skills/DraftManager.cs:195): EnsureDependencies → `offerGenerator==null||offerUI==null` ise warning+return (L199-204) → `IsDraftActive=true` (L206) → `room=GetLiveRoomDepth()` (L208) → **room==4 ise ForgeRoom1 return, room==8 ise ForgeRoom2 return** (L211-224, IsDraftActive=false, kartsız) → normal: offers üret → `offerUI.Show(...)` (L248).
- `GetLiveRoomDepth()` (L168-171): `return RuntimeRoomManager.Instance?.CurrentRoom ?? 1;`
- `EnsureDraftManager()` (RoomRunDirector.cs:1506-1513) reward SPAWN'dan önce (L1374) `DraftManager_Auto` yaratır.
- `EnsureDependencies()` (DraftManager.cs:554-580): offerUI/offerGenerator yoksa FindAnyObjectByType, o da yoksa AddComponent ile yaratır.
- `SkillOfferUI.Show()` (UI/SkillOfferUI.cs:65): `if(panel==null) BuildPanel()` → kendi Canvas+kartlarını PROKÜDÜREL kurar (serialized prefab gerekmez).

## ORCHESTRATOR'IN CANLI REPRO BULGUSU (Opus, 2026-06-15)
_Arena-direct play (playModeStartScene geçici null, sonra MainMenu'ye restore edildi) + execute_code:
- `DraftManager.Instance = DraftManager_Auto` (EnsureDraftManager çalıştı → **aday #1 "Instance null" ELENDİ**).
- `RuntimeRoomManager.Instance = NULL` → `GetLiveRoomDepth()` = `?? 1` → **room=1** (bu harness'ta Forge değil).
- `DraftManager.Instance.ShowDraft()` doğrudan çağrıldı → **`IsDraftActive=True`, SkillOfferPanel canvas (ScreenSpaceOverlay, enabled, sortingOrder=1050), 6 buton kuruldu.** Hiçbir branch logu (Forge/dep-null) yok.
- **SONUÇ: ShowDraft → kart-render path'i İZOLASYONDA ÇALIŞIYOR.** Aday #3 (dep-null) ve "render edemiyor" hipotezleri ELENDİ.

## ELENEN vs KALAN
ELENDİ: #1 (Instance null — EnsureDraftManager savunuyor), #3 (dep-null — self-heal), render hatası.
KALAN: (A) gerçek akışta `RuntimeRoomManager.CurrentRoom` Forge'a (4/8) çözülüyor → Forge early-return → kartsız [harness'ta RRM null olduğu için TEST EDİLEMEDİ]; (C) pickup/collect path'i (G tuşu/trigger/coroutine guard); (D) flow-spesifik fark (akış-özel bozuk obje, RRM etkileşimi).

## COUNCIL'E SORULAR
**Q1 (kök neden ranking):** ShowDraft izolasyonda çalıştığına göre, gerçek F2 kök nedeni en olası hangisi? (A) Forge room-depth misresolve / (C) pickup-collect / (D) flow-spesifik / başka? Cite ile gerekçelendir.

**Q2 (VİDEO İÇİN KRİTİK):** F2 gerçekten **golden-path'i** (normal ilk combat oda, depth 1-3, Echo değil) bozuyor mu — yoksa SADECE Forge(4/8)/Echo odalarını mı (storyboard zaten bunlardan kaçıyor)? Eğer golden-path etkilenmiyorsa, video segmenti GÜVENLİ ve F2 "bilinen limitasyon"a düşer. Bu cevabı netleştirmek en yüksek değerli iş.

**Q3 (gerçek-akış repro reçetesi — cx codebase):** `RuntimeRoomManager.CurrentRoom` nasıl artıyor? Gerçek akışta İLK reward-veren combat odasının depth'i kaç? _Arena full-flow'a hızlı ulaşmak için dev hook var mı? (G ile collect mi, otomatik mi?)

**Q4 (fix):** Düzeltme gerekiyorsa EN KÜÇÜK cerrahi fix nedir? (5 adayı körlemesine fixleme YOK.)

Çıktı: dosyaya ≤ kısa; her iddia file:line ile. Format: Q1-Q4 başlıkları, net verdict + gerekçe.
