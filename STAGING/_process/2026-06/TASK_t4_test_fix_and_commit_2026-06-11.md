ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# TASK: T4 Test Fix + Uncommitted Yığın Commit

## GÖREV 1 — T4 Test Fix

Dosya: `Assets/Tests/ChamberRobustnessTests.cs`
Test: `T4_ChamberClasses_ContainBothUnlockedStarterClasses`

Sorun: Test "2 bekliyor" ama ChamberSelectBootstrap.cs artık 10 class tanımladı.
ChamberSelectBootstrap.cs'te `IsDemoSelectable` gate: WarbladeSentinel + ElectricLancer = 2 unlocked starter, diğer 8 kilitli.

Yapılacak:
1. Test dosyasını oku — tam assertion'ı gör
2. Assertion'ı güncelle: toplam class sayısı = 10, IsDemoSelectable = 2 (WarbladeSentinel + ElectricLancer)
3. Test pass etmeli, semantik değişiklik yok

## GÖREV 2 — Uncommitted Yığın Commit (mantıksal gruplar)

`git status` al, şu mantıksal commit gruplarını yap:

### Commit A — Lighting System
- `Assets/Data/Rooms/Lighting/` (yeni klasör, 3 profil)
- `Assets/Art/Materials/RoomEnvironment_SpriteLit.mat`
- `Assets/Scenes/_Arena.unity`
- `Assets/Prefabs/Player.prefab`
- `Assets/Data/Rooms/Generated/*.asset` (17 oda — globalIntensity→0)
- `Assets/Data/Rooms/Library/*.asset`
- `Assets/Data/Rooms/Special/Shop_01.asset`
- Script'ler: `IsoRoomBuilder.cs`, `RoomRunDirector.cs`, `RoomTemplateSO.cs`
- Commit msg: `feat(lighting): FAZ1 dual-global fix — arena 0.22 cold, 17 rooms→0, hero point light`

### Commit B — Blueprint/Props
- `Assets/Data/Blueprint/Profiles/*.asset`
- `Assets/Data/Blueprint/PropPools/*.asset`
- `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs`
- Commit msg: `feat(props): blueprint profile + prop pools (dominant/path/secondary/transition/dirt)`

### Commit C — Chamber Select
- `Assets/Scripts/UI/ChamberSelectBootstrap.cs`
- `Assets/Tests/ChamberRobustnessTests.cs` (T4 fix dahil)
- Commit msg: `feat(chamber): 10-class select UI — IsDemoSelectable gate (WB+EL unlocked)`

### Commit D — Settings
- `.claude/settings.json`
- Commit msg: `chore(config): claude settings update`

## BAŞARI KRİTERİ
- T4 test pass (Unity test runner ile doğrula — UnityMCP kullan)
- 4 commit clean, `git log --oneline -6` ile göster
- Uncommitted dosya kalmasın (CURRENT_STATUS.md hariç)
