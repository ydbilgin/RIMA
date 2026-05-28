# Oda Transitions — Day 3+ Runtime Sub-Room Implementation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## Amaç (kullanıcı ilk istek, 2026-05-27 sabah)
"Odaları birbirine bağla sanal bi kapı yap içinden geçe geçe odaları gezmek istiyorum karakterimle"

5-oda Faz 1 demo loop. Kullanıcı kararı 2026-05-27 gece: **Fade + offset hybrid** + **5 oda (Faz 1 demo spec)**.

## Bağlam (locked decisions)
- 5 oda spec LIVE: `STAGING/room_layout_phase1_demo.md` (Agent 2 Sonnet design 2026-05-27)
- Room1 Tutorial Combat / Room2 Combat Medium / Room3 Combat Hard L-shape / Room4 Vestibule Reward / Room5 Boss Arena
- **Karar #149 LOCK:** Runtime sub-room transitions (EncounterTemplateSO sequence + RoomTransitionFX fade) — aynı sahnede oda swap, multi-scene değil
- Mevcut altyapı:
  - `Assets/Scripts/Core/RuntimeRoomManager.cs` (1434 LOC LIVE)
  - `Assets/Scripts/Core/RoomTransitionFX.cs` LIVE
  - `Assets/Scripts/Systems/Map/RoomLoader.cs:47-50` **STUB** — `LoadNext()` sadece Debug.Log atıyor
  - `Assets/Scripts/Environment/Gate.cs` LIVE (D2 Gate flow)
  - `Assets/Scripts/Environment/MapFragmentBridge.cs` Day 2 flow LIVE
  - `Assets/Scripts/Environment/MapFragmentSpawner.cs` Day 2.5 LIVE
- Day 2 flow zaten kuruldu: room cleared → fragment spawn → G pickup → skill draft → gate unlock → enter → `RoomLoader.LoadNext()` çağrı (ama stub)

## Triple-AI dispatch yapısı (HARD rule `feedback_code_writer_rotation`)
Bu task **mimari + impl** karışımı. İdeal rotation:
- **Opus (rima-design)** — design judgment: sub-room state machine, room sequence indexing, fade timing, player position teleport pattern
- **Codex xhigh write** — RoomLoader.LoadNext() core impl + RoomTransitionFX integration (algoritma yoğun)
- **Sonnet review** — scene wire, prefab integration, smoke test
- Orchestrator: triple-AI subagent içinde (Opus dispatch Codex+Sonnet), sentez orchestrator'a döner

## İş kalemleri

### 1. RoomLoader.LoadNext() implementation
- File: `Assets/Scripts/Systems/Map/RoomLoader.cs` (line 47-50 STUB doldur)
- State: `static int currentRoomIndex` (0-4 for 5 oda)
- LoadNext akışı:
  1. currentRoomIndex++
  2. if currentRoomIndex >= 5 → "Demo complete" overlay (Room 5 sonu)
  3. RoomTransitionFX.Instance.FadeOut(0.3f)
  4. Mevcut oda content teardown (mob despawn, fragment cleanup, gate destroy)
  5. Yeni oda content spawn (next RoomConfig'ten)
  6. Player position teleport (next oda's PlayerStartMarker)
  7. Camera follow target update
  8. RoomTransitionFX.Instance.FadeIn(0.3f)
  9. "Room N/5" overlay (HUD)
- ~80 LOC

### 2. 5-oda sequence configuration
- ScriptableObject veya prefab inventory: 5 oda data
- Her oda için:
  - PlayerStartMarker position (relative to room origin or global)
  - Mob spawn data (Room1: 3 Imp, Room2: 3 Imp + 2 Walker, Room3: 4 Imp + 2 Walker + 1 Hulk Elite, Room4: 0 mob (vestibule), Room5: 1 Boss + 4 chain anchor)
  - Cliff perimeter pattern
  - Gate position(s)
  - Focal element (Room1 cyan rune, Room2 brazier, Room3 broken column, Room4 plinth, Room5 chain anchors)
- spec ref: `STAGING/room_layout_phase1_demo.md` Section 3-4

### 3. Sub-room layout strategy (Y offset teleport vs scene reuse)
- **Y offset teleport** (önerilen — basit, hızlı):
  - Tek sahne, oda content offset Y=0, Y=40, Y=80, Y=120, Y=160
  - LoadNext = Player+Camera teleport (Y += 40)
  - Mevcut oda content destroy edilir, yeni oda content instantiate
- **Sahnede pre-built** (alternatif):
  - 5 oda content sahnede önceden var, sadece GameObject.SetActive(true/false)
  - Player position teleport
- Karar: Opus subagent verir (sub-agent dispatch'te)

### 4. Mob spawn integration
- Her oda yüklendiğinde RoomConfig'ten mob data oku
- FractureImp_Playtest prefab spawn (Room 1-3)
- ShardWalker, HollowHulk prefab gerek (yoksa placeholder)
- PenitentSovereign Room 5 (zaten LIVE)
- Player'a göre safe spawn position (WalkabilityMap LIVE)

### 5. Gate Y-offset adjust
- Her oda kendi gate'i. LoadNext sonrası önceki gate destroy, yeni gate spawn (next oda)
- Gate position oda merkezinden N kenar (Karar #149 spec)
- MapFragmentBridge zaten LIVE, Gate.OnPlayerEntered → RoomLoader.LoadNext

### 6. Room 5 demo end
- Boss kill (PenitentSovereign %50 HP placeholder Faz 1)
- "Demo Complete" overlay + restart button (Room 1 reload)
- `NotifyBossDefeated()` zaten LIVE RuntimeRoomManager:543

### 7. HUD oda counter
- "Room 1/5", "Room 2/5"... fade ile göster
- Mevcut HUDController ile entegrasyon (LIVE)

## Dosyalar (scope)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` (EXTEND ~80 LOC)
- `Assets/Scripts/Core/RuntimeRoomManager.cs` (extend ~50 LOC — sub-room sequence integration)
- Yeni `Assets/ScriptableObjects/Rooms/Phase1_*.asset` × 5 oda data (ScriptableObject)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (5 oda offset Y position wire)
- ~200 LOC + 5 SO + scene mutation

## YASAK
- Multi-scene (Karar #149 single-scene sub-room LOCK)
- Yeni RoomTransitionFX yazma (mevcut LIVE)
- Player kontrolü değişiklik (D2 fix kapsamı)
- T3 standalone tool ile çakışma (RoomLoader runtime, T3 editor — bağımsız)
- Yeni .cs → `refresh_unity scope=all mode=force` ZORUNLU

## Verify
- PlayMode aç → Room 1 spawn → 3 Imp kill → Fragment drop → G pickup → SkillDraft → pick → Gate unlock → Gate'e gir
- Fade out 0.3s → Player teleport Y+=40 → Fade in 0.3s → "Room 2/5" overlay
- Loop devam Room 2-3-4-5
- Room 5 boss spawn, %50 HP kill → "Demo Complete" overlay + restart button
- Restart → Room 1'e dön

## Output
- `STAGING/ROOM_TRANSITIONS_DONE.md` — değişen dosyalar + verify checklist + compile durum + demo loop test recording

## Süre
~2-3 saat triple-AI flow. Opus design (1-2 saat) + Codex write (45-60 dk) + Sonnet review (30 dk) paralel ortak ~3 saat wallclock.

BLOCKED durumu: (a) RuntimeRoomManager.cs 1434 LOC complex — refactor scope çıkarsa orchestrator'a flag. (b) Sub-room layout strategy Opus karar veremedi → user'a sor. (c) Mob prefab (ShardWalker, HollowHulk) eksik → placeholder FractureImp fallback.
