# CLAUDE MASTER PROMPT — RIMA DEMO SCOPE LOCK

RIMA için demo scope’u netleştiriyoruz. Lütfen büyük refactor, yeni sınıf, full polish, 10-class completion gibi scope şişiren işleri önermeden önce aşağıdaki dar demo hedefini uygula.

## DEMO HEDEFİ

2 sınıflı kısa playable demo:

```text
MainMenu
→ CharacterSelect / Chamber
→ Warblade veya Elementalist seç
→ _Arena run
→ 3-4 combat room
→ 1 shop room
→ 1 boss room
→ Demo Clear
```

## ÖNEMLİ REPO GERÇEĞİ

Canlı oda akışı legacy sistem değil.

Patch hedefi:
```text
_Arena
RoomRunDirector
IsoRoomBuilder
RoomTemplateSO
ChamberSelectBootstrap
CharacterSelectScreen
UIManager
SkillCodexUI
SkillBarUI
Player weapon/mount scripts
```

Patch yazılmayacak / legacy:
```text
RoomLoader
RoomSequenceData
RuntimeRoomManager
DoorTrigger
GateBehavior
Gate.cs
```

## P0 FIXLER

1. Play/New Run yanlışlıkla Skill Codex açmamalı.
2. ESC Skill Codex açmamalı. ESC PauseMenu açmalı.
3. Codex/overlay kapanınca Time.timeScale ve input kilitlenmemeli.
4. Tek cursor kalmalı.
5. Character seçilmeden run başlamamalı.
6. CharacterSelect/Chamber → _Arena akışı stabil olmalı.
7. Warblade sword floor/cliff altında render olmamalı.
8. Elementalist staff taşımamalı; floating rune disc kullanmalı.
9. Görsel floor = yürünebilir alan olmalı.
10. Demo forced room sequence çalışmalı.

## DEMO CLASS SCOPE

Sadece:
```text
Warblade
Elementalist
```

Diğer classlar:
- data/model olarak kalabilir
- görsel display-only olabilir
- demo run’a girmesin
- bug yaratmasın

## WARBlade

Weapon:
- Greatsword
- PlayerRoot child
- SortingGroup altında
- Characters/Entities layer
- Direction offset/rotation profile
- Floor/cliff altında asla kalmaz

Skills:
- LMB Basic Slash
- RMB Cleave
- Q Iron Charge
- Space Dash

## ELEMENTALIST

Weapon:
- Staff yok
- Elde obje yok
- Sağ avuç üstünde floating golden rune disc
- Disc child object, hover bob, cast pulse
- Projectile spawn disc/hand civarından

Skills:
- LMB Rune Bolt projectile
- RMB Arcane Burst small AoE
- Q Burn/Frost Field
- Space Dash

## ROOM SEQUENCE

Demo için random graph gerekmez. Forced sequence kabul:

```text
Combat_Small_Intro
Combat_Medium
Shop_01
Combat_PreBoss
Boss_PenitentSovereign_Demo
DemoClear
```

## ROOM SIZE

Combat:
- min 24x18
- ideal 28x20

Boss:
- min 32x24
- ideal 36x26 / 40x30

Her combat odasında:
- 2 temiz dash lane
- enemy spawn player’dan uzakta
- visible floor walkable
- no invisible wall

## SHOP MINIMUM

3 item:
1. Heal
2. Damage +15%
3. Max HP veya Cooldown -10%

Echo fiyatı, satın alma, apply effect, sold out/disabled state.

## BOSS MINIMUM

1 boss, 3 attack:
1. Slam
2. Telegraph line/AoE
3. Projectile burst veya charge

HP %50’de küçük hızlanma opsiyonel.
Boss ölünce Demo Clear.

## TESTLER

Aşağıdaki testleri ekle veya manuel checklist ile doğrula:

1. BootFlowTest
   - MainMenu → CharacterSelect/Chamber
   - SkillCodex auto-open yok
   - ESC PauseMenu

2. CharacterSelectTest
   - Warblade seçilir
   - Elementalist seçilir
   - SelectedClass _Arena’ya taşınır

3. WeaponSortingTest
   - Warblade sword Characters/Entities altında
   - Elementalist rune disc visible
   - Ground/Cliff altında kalmaz

4. WalkabilityTest
   - visual floor cells walkable
   - void cells blocked
   - no right-side invisible wall

5. DemoSequenceSmokeTest
   - Combat → Combat → Shop → Combat → Boss → Clear

## DELIVERABLE

Lütfen çıktı olarak:

1. Değişen dosya listesi
2. Root cause açıklaması
3. Uygulanan fixler
4. Test sonuçları
5. Kalan riskler
6. Re-playtest checklist

ver.

Kapsam dışı:
- 10 sınıfı bitirme
- full skill matrix
- full item economy
- yeni biome
- büyük art polish
- full animation redesign

Önce demo yürüsün. Görsel makyaj sonra.
