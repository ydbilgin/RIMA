# effort: xhigh

ALWAYS WRITE YOUR RESULT SUMMARY TO `CODEX_DONE.md` AS THE VERY LAST STEP.

# TASK: Reward+Portal Phase 1 (Pattern C MVP) — Wire-up

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
Tek-portal Skill Draft choice flow'unu wire-up et. Mevcut altyapı %90 hazır (Portal, PortalSpawnController, DraftManager, SkillOfferGenerator, SkillOfferUI). Eksik parça: portal trigger → draft UI bridge + reachability validation + auto-timer suppress.

## Opus Design Verdict (LOCKED)
- **Phase 1 = wiring**, yeni feature kodu değil.
- **FanLayoutSolver SİL/DEVRE DIŞI ETME** — `count=1` zaten doğru çalışıyor (`[anchor]` döner). Phase 2 reuse için kod kalır, sadece `RoomTypeData` weights `100/0/0`.
- **Yeni UI YAPMA** — SkillOfferUI canonical Hades-style draft UI (slide-in, replace mode, gold/heal fallback). Paralel Canvas+Button×3 KESINLIKLE YASAK.
- **Room-cleared auto-timer SUPPRESS** — `DraftManager.HandleRoomCleared` portal varsa early-return, draft sadece portal trigger'dan gelir.
- **Reachability flood-fill** Player start'tan WalkabilityMap üzerinde — anchor reachable set'te değilse warning + spawn reddi.

## Implementation Steps (7 step)

### Step 1 — WalkabilityMap.IsReachableFromPlayer
**File:** `Assets/Scripts/Environment/WalkabilityMap.cs`

**Add:**
- `public bool IsReachableFromPlayer(Vector3Int cell)` — BFS cache'li flood-fill from Player tag GameObject'in cell pozisyonu, sonuç set'te `cell` var mı kontrolü.
- Cache invalidation: `tilemapTileChanged` event subscribe (Tilemap.tilemapTileChanged) → reachable set null.
- ~40 satır.

**Edge cases:**
- Player yoksa: `Debug.LogWarning` + `return false`
- Player position floor dışındaysa: closest floor cell'i bul (1-cell radius)

### Step 2 — PortalSpawnAnchor reachability gizmo
**File:** `Assets/Scripts/Environment/PortalSpawnAnchor.cs`

**Add:**
- `public bool usePortalGatedDraft = true;` (default true — Phase 1 davranışı)
- `OnDrawGizmos`: Edit mode'da `WalkabilityMap.Instance` varsa anchor cell'i `IsReachableFromPlayer` ile kontrol et, false ise kırmızı sphere çiz (radius 0.5).

### Step 3 — DraftManager portal entry point + suppression
**File:** `Assets/Scripts/Skills/DraftManager.cs`

**Add:**
- `public void TriggerDraftFromPortal(Portal source)` — private `_portalSource` field set et, mevcut `ShowDraft()` logic'ini çağır.
- `HandleRoomCleared` başına early-return: scene'de `PortalSpawnAnchor` ile `usePortalGatedDraft=true` varsa skip auto-timer.
  ```csharp
  var anchor = Object.FindFirstObjectByType<PortalSpawnAnchor>();
  if (anchor != null && anchor.usePortalGatedDraft) return;
  ```

### Step 4 — PortalRewardBridge.cs (NEW)
**File:** `Assets/Scripts/Environment/PortalRewardBridge.cs` (CREATE)

**Behavior:**
- `OnEnable`: `PortalSpawnController` bul, spawned portal listesini al, her `Portal.OnEntered` event'ine subscribe et.
- Event handler: `DraftManager.Instance.TriggerDraftFromPortal(portal)` çağrısı.
- `DraftManager.OnSkillPicked` event'ine subscribe → portal "armed" state'e geçer (bool `_armed = true` Portal üstünde).
- Armed portal'a tekrar entry → `RoomLoader.LoadNext()` çağrısı.

**BLOCKED check:** `RoomLoader.LoadNext()` yoksa, **ADD** stub:
```csharp
// File: Assets/Scripts/Systems/RoomLoader.cs (NEW or extend)
public static class RoomLoader {
    public static void LoadNext() {
        Debug.Log("[Phase1] Next room transition not implemented — Phase 2 will wire real loader.");
    }
}
```

### Step 5 — RoomType_Phase1Combat.asset (NEW SO)
**File:** `Assets/Data/RoomTypes/RoomType_Phase1Combat.asset` (CREATE)

**Config:**
- ScriptableObject of type `RoomTypeData`
- `category = Combat`
- `weight1Portal = 100, weight2 = 0, weight3 = 0`
- Diğer field'lar default

### Step 6 — Scene wire-up
**File:** `Assets/Scenes/Test/PlayableArena_Test01.unity`

**GameObject'ler ekle:**
1. **PortalSpawnAnchor** (empty GO)
   - Position: designer-placed (room exit, örn: floor mass center + maxRadius north). Şimdilik world pos (10.5, 12, 0) deneyebilir veya floor north edge ortası.
   - Component: `PortalSpawnAnchor`
     - `roomType = RoomType_Phase1Combat`
     - `usePortalGatedDraft = true`
2. **PortalSpawnController** (empty GO)
   - Component: `PortalSpawnController`
     - `walkabilityMap = scene WalkabilityMap` reference
3. **PortalRewardBridge** (empty GO veya PortalSpawnController üstünde component)
   - Component: `PortalRewardBridge`
4. **DraftManager** sahnede zaten yoksa ekle (`EnsureDependencies` otomatik yapar ama explicit GO QC için temiz)

### Step 7 — Verification (Play mode test)
1. Sahneyi aç → Play
2. Mob spawn et / kill et (manuel test — mevcut mob spawn varsa)
3. Console'da bekle: `[PortalSpawn] requested=1 final=1 note=(default)` veya benzeri
4. Portal görsel olarak spawn olmalı (placeholder cyan #00FFCC kare)
5. Player portal'a yürü → console: `[Portal] Entered: Combat` veya benzeri
6. SkillOfferUI panel açılmalı (3 skill kartı)
7. Kart seç → `[Draft] Picked skill X` log → panel kapanır
8. Player portal'a tekrar yürü → `[Phase1] Next room transition not implemented` log

**Verification başarı kriteri:** 8 adımın tümü gerçekleşmeli, 0 console error.

## DO NOT (Scope Limit)

- **FanLayoutSolver disable etme** — Phase 2 için kalır, RoomTypeData weights ile constrain
- **Paralel UI YAPMA** — SkillOfferUI zorunlu (Hades-style canonical lock)
- **Portal sprite üretme** — placeholder cyan kare kalır, real sprite Phase 2 user Web UI manuel
- **Map Fragment ekleme** — Phase 2+
- **Preview thumbnail** — Phase 3
- **Camera/projection değiştirme** — HIGH TOP-DOWN 3/4 lock korunur

## Reference Files (Read önce, dokun gerektiğinde)

- `Assets/Scripts/Skills/SkillOfferGenerator.cs` — read-only, `GenerateOffers()` API kullan
- `Assets/Scripts/Skills/DraftManager.cs` — modify (Step 3)
- `Assets/Scripts/UI/SkillOfferUI.cs` — read-only, `Show(offers, callback, roomNumber)` kullan
- `Assets/Scripts/Systems/RewardOffer.cs` — read-only
- `Assets/Scripts/Environment/Portal.cs` — read-only, `OnEntered` event'i kullan
- `Assets/Scripts/Environment/PortalSpawnController.cs` — read-only, `ActivePortals` listesi kullan
- `Assets/Scripts/Environment/PortalSpawnAnchor.cs` — modify (Step 2)
- `Assets/Scripts/Environment/FanLayoutSolver.cs` — read-only
- `Assets/Scripts/Environment/WalkabilityMap.cs` — modify (Step 1)
- `Assets/Scripts/Environment/RoomTypeData.cs` — read-only (yeni SO asset üret)

## Cross-cutting

- `effort: xhigh` (üst satır)
- Çakışma çıkarsa: API mismatch → spec'e "ADD: method X" yaz, Codex eklesin
- UnityMCP ile sahne wire-up yap (manage_components, manage_scene, manage_gameobject)
- Sahne save: `manage_scene action=save`
- Console temiz olmalı: `read_console` ile error/warning kontrol et

## Output
- Kod commit etme — sadece working tree değişiklikleri
- CODEX_DONE.md'ye kısa rapor: 7 step status (✅/❌/blocked), verification log özetleri, eklenen dosya listesi
