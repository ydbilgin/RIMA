# Oda Transitions Deferred Wire (Unity scene + SO)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Oda transitions impl (Codex write PASS) için 4 DEFERRED item Unity'de wire. Unity AÇIK, UnityMCP kullan.

## Bağlam
- Codex impl: `RoomLoader.cs` +220 LOC + `RoomSequenceData.cs` 47 LOC + `DemoCompleteOverlay.cs` 93 LOC + `RuntimeRoomManager.cs` +1 LOC LIVE
- 0 compile err / 0 validate warn
- Detay: `STAGING/ROOM_TRANSITIONS_DONE.md`

## İş kalemleri

### 1. RoomLoader.Start() override mikro edit (~5 LOC)
- File: `Assets/Scripts/Systems/Map/RoomLoader.cs`
- Eğer `LoadFirstRoom()` public method var ama auto-call yoksa:
  ```csharp
  void Start()
  {
      if (autoStart) LoadFirstRoom();
  }
  ```
- `[SerializeField] private bool autoStart = true;` ekle

### 2. 5 RoomSequenceData ScriptableObject .asset create + populate
- Path: `Assets/ScriptableObjects/Rooms/`
- Files:
  - `Phase1_Room1_TutorialCombat.asset`
  - `Phase1_Room2_CombatMedium.asset`
  - `Phase1_Room3_CombatHard.asset`
  - `Phase1_Room4_Vestibule.asset`
  - `Phase1_Room5_BossArena.asset`
- Her SO fields populate (spec ref `STAGING/room_layout_phase1_demo.md` Section 3-4):
  - Room1: playerStartPos=(0,-3.5,0), mob=[FractureImp×3], isRewardRoom=false, isBossRoom=false
  - Room2: playerStartPos=(0,Y=40-3.5,0), mob=[FractureImp×3 + ShardWalker×2 placeholder=FractureImp], isRewardRoom=false
  - Room3: playerStartPos=(0,Y=80-3.5,0), mob=[FractureImp×4 + ShardWalker×2 + HollowHulk×1 placeholder=FractureImp], isRewardRoom=false
  - Room4: playerStartPos=(0,Y=120-3.5,0), mob=[], isRewardRoom=true (Vestibule, no mob)
  - Room5: playerStartPos=(0,Y=160-3.5,0), mob=[PenitentSovereign×1] (Boss prefab var), isBossRoom=true
- Y offset: rooms ana arena Y=0, sub-room transition için Y+=40 (Karar #149 single-scene)

### 3. PlayableArena_Test01.unity scene wire
- RoomLoader GO (yeni veya mevcut RuntimeRoomManager GO'su) bul/oluştur
- RoomLoader.cs component attach
- `_sequence` array field 5 SO referansı set
- RuntimeRoomManager GO SetActive(false) (legacy path Faz 1'de devre dışı — Opus mitigation)
- Sahne save

### 4. 30s PlayMode smoke test
- UnityMCP execute_code ile PlayMode aç
- Console log "[Phase1] LoadFirstRoom Room 1/5" görmek
- Player position (0,-3.5,0) civar verify
- 5s sonra PlayMode kapat
- Console err/warn check (read_console)

## Dosyalar (scope)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` (mikro edit +5 LOC autoStart)
- `Assets/ScriptableObjects/Rooms/Phase1_Room1..5_*.asset` × 5 NEW
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (RoomLoader wire + RuntimeRoomManager SetActive false)

## YASAK
- Codex output dosyalarını yeniden yazma (Codex PASS, kabul)
- RoomLoader.cs LoadNext logic değişiklik (Codex impl tested OK)
- T3 subsystem (paralel ayrı dispatch)
- Cliff F2-F5 paralel dispatch ile çakışma yok (farklı dosyalar)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU

## Verify
- 0 err / 0 warn
- 5 SO Project window'da görünür, fields populate
- PlayMode → Room 1 spawn → console log
- Gate'e gir (test için manuel kill mob veya K key PlaytestRoomClearedHelper LIVE) → fade → Room 2 → loop

## Output
- `STAGING/ODA_TRANSITIONS_WIRE_DONE.md` — değişen dosyalar + smoke test sonucu + compile durum + log
