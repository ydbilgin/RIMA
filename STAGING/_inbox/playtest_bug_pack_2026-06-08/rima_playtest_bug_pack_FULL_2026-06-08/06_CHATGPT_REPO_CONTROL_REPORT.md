# ChatGPT Repo Kontrol Raporu — RIMA Playtest Bugları

## 1) Repo erişimi ve güncel okuma kuralı

GitHub installation artık yalnızca `ydbilgin/RIMA` reposunu gösteriyor. Bu iyi; önceki karmaşada yanlış repo okunuyordu.

RIMA'nın kendi `AI_READER_GUIDE.md` dosyasına göre güncel mimari kararlar:

- Proje Unity 6, 2D top-down yüksek 3/4 kamera, chibi pixel-art roguelite.
- Oda doktrini: duvarsız **yüzen ada + arka kenarda Rift portalları**.
- Güncel aktif kod okunurken önce `CURRENT_STATUS.md`, sonra `LIVE_FLOW_PROOF_2026-06-07.md` okunmalı.
- Legacy sistemler canlı yolda değildir: `RoomLoader`, `RoomSequenceData`, `Gate.cs`, `GateBehavior`, `DoorTrigger`, `RuntimeRoomManager`.
- Canlı yol: `_Arena -> RoomRunDirector -> IsoRoomBuilder.BuildExitDoors -> RoomRunExitDoorTrigger -> DungeonGraph child-choice`.

## 2) Canlı flow kontrolü

`STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md` şunu söylüyor:

```txt
MainMenu -> CharacterSelect/Chamber -> _Arena
_Arena içinde RoomRunDirector + IsoRoomBuilder canlı.
```

Bu yüzden kullanıcının "Play'e basınca karakter seçmeden arena/combat açılıyor" gözlemi iki ihtimale ayrılmalı:

### İhtimal A — Kullanıcı Unity Editor'da MainMenu yerine doğrudan `_Arena` sahnesinden Play'e basıyor
Bu durumda CharacterSelect beklemek doğru olmaz; `_Arena` zaten `buildOnStart: true` ile odanın kurulmasını başlatır. Claude önce aktif sahne adını loglamalı.

### İhtimal B — MainMenu akışı gerçekten CharacterSelect/Chamber'ı bypass ediyor
Bu durumda `MainMenuController`, `MainMenuScreen`, `CharacterSelectScreen`, `ChamberSelectBootstrap` birlikte incelenmeli. Özellikle iki farklı main menu sistemi hâlâ aynı anda yaşıyor olabilir.

Kontrol edilmesi gereken log:

```csharp
Debug.Log($"[BOOT_PROOF] activeScene={SceneManager.GetActiveScene().name} selectedClass={PlayerClassManager.SelectedClass}");
```

## 3) ESC bug'ı artık kesinleşti

`UIManager.cs` içinde `OnEsc()` davranışı şu an gerçekten Skill Codex açıyor:

```csharp
// Toggle skill codex
if (skillCodexOpen) CloseSkillCodex();
else                OpenSkillCodex();
```

Kullanıcı şunu istiyor:

```txt
ESC -> Pause Menu
Pause Menu -> Resume / New Run / Settings / Skill Codex / Exit to Menu / Exit Game
Skill Codex -> ESC ile kapanır, ama ESC'nin default davranışı Skill Codex değildir.
```

Bu yüzden `ESC basınca Yetenek Kodeksi geliyor` P0/P1 gerçek bug. Burada tahmin yok, kod davranışı kullanıcı gözlemini doğruluyor.

## 4) Skill Codex hover bug'ı

Görselde mouse skill satırına gelince sol tarafta mavi/dikey bozuk tooltip/text çıkıyor. Repo kontrolünde şunlar şüpheli:

- `SkillCodexUI` full-screen runtime panel olarak kuruluyor.
- `TooltipSystem` tooltip panelini `GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>()` ile ilk bulduğu canvas altına koyuyor.
- Projede `MainMenuScreen`, `CharacterSelectScreen`, `SkillCodexUI`, HUD canvas ve DontDestroyOnLoad UI objeleri birlikte yaşayabiliyor.

Bu, tooltip'in yanlış canvas'a parent edilmesi veya yanlış sorting/scale altında görünmesi riskini yaratıyor. Çözüm:

- Tooltip için dedicated `TooltipCanvas` veya aktif panel canvas'ı kullanılmalı.
- `SkillCodexUI.Close()` tooltip'i kapatıyor, iyi; ama `Open()` sırasında eski orphan tooltip/canvas temizliği de yapılmalı.
- Codex satırı hoverında tooltip istemiyorsak geçici olarak tooltip tamamen kapatılabilir.

## 5) Skill iconları bazen yüklenmiyor

`SkillCodexUI` içinde icon mantığı kabaca:

```csharp
Sprite icon = skill.icon != null ? skill.icon : RimaUITheme.PassiveIcon(skill.skillName);
```

`SkillBarUI` tarafında da `skill.icon == null` ise renkli boş/fallback görünüm kullanılıyor. Kullanıcı zaten "skill iconları her zaman yüklenmiyor" diyor.

Bu durumda eksik olan şey:

- Missing icon logları.
- Deterministic fallback.
- SkillData -> Sprite referanslarının batch doğrulaması.

Önerilen debug:

```csharp
if (skill.icon == null)
    Debug.LogWarning($"[SKILL_ICON_MISSING] {skill.classType}/{skill.skillName} id={skill.skillId}");
```

## 6) Kılıç/silah bug'ı

Görselde kılıç/fx floor üstünde değil, cliff/edge arkasından görünür gibi duruyor. Bu iki ayrı sorun:

1. **Sorting/mount sorunu:** silah player ile aynı SortingGroup altında değil veya y-sorting/cliff sorting altında kalıyor.
2. **Asset sorunu:** mevcut kılıç sprite açısı RIMA'nın top-down/chibi/pixel yönüne uymuyor, beyaz uzun slash/placeholder gibi duruyor.

Repo status notlarında weapon pipeline audit ve minimal mount-profile patch'in hâlâ ayrı/gated workstream olduğu yazıyor. Yani mevcut görüntü şaşırtıcı değil; demo için Level1 minimal mount patch gerekli.

Minimum fix:

```txt
PlayerRoot
  SortingGroup: Characters/Entities
  BodyRenderer order 0
  WeaponMountView
    WeaponRenderer order +1 veya yön profiline göre
```

## 7) Sağ tarafa yürüyememe / küçük map

Kullanıcı görselinde floor var ama sağ tarafa gidilemiyor. Bu collider değilse bile "görsel zemin = walkable zemin" sözleşmesi kırılıyor.

Canlı yol `IsoRoomBuilder + RoomTemplateSO` olduğu için şu dosya/sistemlere bakılmalı:

- `Assets/Data/Rooms/...` aktif room template.
- `RoomTemplateSO` walkable/floor hücreleri.
- `IsoRoomBuilder.Build(template)` çıktısı.
- `WalkabilityMap` veya collision tilemap.
- `_Arena` içinde camera/room bounds clamp.

Kullanıcı ayrıca map'in combat için küçük olduğunu söylüyor. Güncel hedefler açısından normal combat odası sadece güzel görünmekle kalmamalı; dash lane, kite alanı ve projectile okuması vermeli.

## 8) "2 tane fare geliyor, onları kesince kilitleniyor"

Bu çok kritik çünkü iki farklı root cause olabilir:

### A — Başlangıç sahnesi yanlış
Kullanıcı karakter seçmeden doğrudan combat odasına düşüyorsa, iki enemy spawn normal oda akışı olabilir.

### B — Room clear sequence bozuk
İki enemy öldükten sonra `RoomRunDirector` clear/reward/exit sequence takılıyor olabilir.

Kontrol noktaları:

- `aliveEnemies` veya kill counter sıfırlanıyor mu?
- Enemy death event `RoomRunDirector` tarafından dinleniyor mu?
- Reward/exit spawn bloklanıyor mu?
- Skill draft veya codex açık kaldığı için `Time.timeScale=0` mı?
- Stale chamber runtime root veya old manager objesi combat sahnesine sızıyor mu?

## 9) Kısa verdict

Benim kontrolüme göre en kesin bug:

```txt
P0/P1: ESC şu anda bilerek SkillCodex açıyor. PauseMenu yok/bağlı değil.
```

En kritik belirsiz bug:

```txt
Play flow gerçekten yanlış mı, yoksa kullanıcı editor'da _Arena sahnesinden mi Play'e basıyor?
```

En yüksek görsel/feel borcu:

```txt
Weapon mount + sword sprite + room size/walkability.
```
