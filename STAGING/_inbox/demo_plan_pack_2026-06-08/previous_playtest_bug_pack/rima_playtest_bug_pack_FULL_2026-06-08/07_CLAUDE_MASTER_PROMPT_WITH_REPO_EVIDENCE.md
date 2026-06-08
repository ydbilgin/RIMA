# Claude'a Yapıştırılacak Güncel Ana Prompt — Repo Kanıtlı

RIMA playtest bug pass yap. Aşağıdaki buglar kullanıcının 4 ekran görüntüsü + ChatGPT repo kontrolünden çıktı. Lütfen eski/legacy oda sistemine patch yazma.

## Repo okuma kuralları

Önce şunları oku:

1. `AI_READER_GUIDE.md`
2. `CURRENT_STATUS.md` en üst blok
3. `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md`
4. Sonra sadece gerekli kaynak dosyalar.

Canlı oda sistemi:

```txt
MainMenu -> CharacterSelect / Chamber -> _Arena
_Arena -> RoomRunDirector -> IsoRoomBuilder -> RoomRunExitDoorTrigger
```

Patch yazma / bağlama yasağı:

```txt
RoomLoader
RoomSequenceData
Gate.cs
GateBehavior
DoorTrigger
RuntimeRoomManager
```

Bunlar legacy; canlı path değil.

---

## BUG 1 — Play'e basınca karakter seçmeden arena/combat geliyor

Kullanıcı gözlemi:

```txt
Play'e basınca karakter seçme ekranı gelmeli ama direkt yüzen oda/arena geliyor.
2 tane fare/mob geliyor, onları kesince kilitlenip kalıyor.
```

Senin ilk görevin: bunun gerçek bypass mı yoksa kullanıcının Unity Editor'da doğrudan `_Arena` sahnesinden Play'e basması mı olduğunu kanıtla.

Yapılacaklar:

- Boot sırasında aktif scene logla.
- MainMenu button hangi koddan çalışıyor kanıtla: `MainMenuController` mı, `MainMenuScreen` mi?
- `CharacterSelectScreen.Awake()` içinde `ChamberSelectBootstrap` gerçekten ekleniyor mu?
- `ChamberSelectBootstrap` otomatik `StartRun()` çağırıyor mu? Çağırmamalı; sadece kapı yakınında G ile olmalı.
- `_Arena` doğrudan açılırsa combat başlaması normal mi? Normalse kullanıcıya/editor workflow'a göre not düş.

Acceptance:

```txt
MainMenu sahnesinden Play -> CharacterSelect/Chamber görünür.
Chamber'da 10 class silüeti + dummy/selection akışı görünür.
_Arena'ya sadece kullanıcı seçip kapı/portal interaction yaptıktan sonra geçilir.
```

---

## BUG 2 — İki mob/fare öldürünce kilitlenme

Bu, ya yanlış sahneye giriş ya da room clear sequence bug'ı.

Canlı path: `RoomRunDirector`, `IsoRoomBuilder`, `RoomRunExitDoorTrigger`.

Kontrol et:

- Enemy death event RoomRunDirector'a ulaşıyor mu?
- `aliveEnemies` veya benzeri sayaç 0 oluyor mu?
- Clear sonrası reward/exit spawn ediliyor mu?
- Time.timeScale 0'da mı kalıyor?
- UI overlay/codex açık kalıp input'u kilitliyor mu?
- Stale `AttunementChamber_Runtime`, old Player, old chamber root, old menu root sahnede kalıyor mu?

Debug log önerisi:

```csharp
Debug.Log($"[ROOM_CLEAR_PROOF] enemyDied={enemy.name} alive={aliveEnemies} room={currentNodeId} state={state} timeScale={Time.timeScale}");
Debug.Log($"[ROOM_EXIT_PROOF] exits={activeExitDoors.Count} rewardPending={rewardPending}");
```

Acceptance:

```txt
Son enemy ölünce oda clear olur.
Reward/exit/portal ortaya çıkar.
Oyuncu kilitlenmez.
Input açık kalır.
Time.timeScale 1 olur.
```

---

## BUG 3 — ESC doğrudan Yetenek Kodeksi açıyor

Bu artık tahmin değil. `UIManager.OnEsc()` şu an SkillCodex toggle ediyor.

İstenen yeni davranış:

```txt
ESC -> Pause Menu
Pause Menu seçenekleri:
- Resume
- New Run
- Settings
- Skill Codex
- Exit to Main Menu
- Exit Game

Skill Codex sadece Pause Menu içinden veya ayrı bir keybind ile açılmalı.
ESC default olarak SkillCodex açmamalı.
```

Yapılacaklar:

- `UIManager.OnEsc()` davranışını değiştir.
- `PauseMenuUI` yoksa runtime-built basit ama RIMA stilinde Pause Menu oluştur.
- SkillCodex'i Pause Menu button'una bağla.
- ESC davranış stack'i:

```txt
Settings açık -> close settings
SkillCodex açık -> close codex
Pause açık -> resume
Gameplay -> open pause
```

Acceptance:

```txt
ESC basınca codex değil PauseMenu açılır.
PauseMenu'den Skill Codex açılabilir.
Codex açıkken ESC codex'i kapatır.
Resume oyunu timeScale=1'e döndürür.
```

---

## BUG 4 — Skill Codex hover'da mavi bozuk text çıkıyor

Kullanıcı görselinde skill row hover edince sol tarafta mavi/dikey bozuk tooltip/text çıkıyor.

Şüpheli sistemler:

```txt
SkillCodexUI
TooltipSystem
RimaUITheme
Canvas sorting / multiple canvas / DontDestroyOnLoad UI
```

Kontrol et:

- Tooltip hangi Canvas altına parent ediliyor?
- `FindObjectOfType<Canvas>()` yanlış canvas buluyor mu?
- Tooltip paneli `SkillCodexUI` canvas'ından daha altta/başka scale'de mi?
- Tooltip content rich-text yüzünden dikey/taşmış mı?
- Codex row hover eventleri nerede ekleniyor?

Fix önerisi:

- Tooltip için dedicated top-level `TooltipCanvas` oluştur: sortingOrder = 3000.
- Tooltip max width/height sabit ve clamp'li olsun.
- SkillCodex kapanırken `TooltipSystem.Hide()` + orphan cleanup.
- Gerekirse Codex için tooltip geçici kapat: row description zaten listede görünüyor.

Acceptance:

```txt
Hover sırasında bozuk mavi dikey text çıkmaz.
Tooltip varsa okunur, panel dışına taşmaz.
Codex kapanınca tooltip de kesin kapanır.
```

---

## BUG 5 — Skill iconları bazen yüklenmiyor

Kullanıcı gözlemi:

```txt
Skill iconları her zaman yüklenmiyor; bazıları boş/kahverengi kutu gibi.
```

Yapılacaklar:

- Tüm `SkillData` icon referanslarını tarayan validation ekle.
- Missing icon warning yaz.
- Fallback sprite belirgin ama temiz olsun.
- `RimaUITheme.PassiveIcon(skill.skillName)` null dönerse bu da loglansın.

Önerilen validation:

```csharp
foreach (var skill in skillDatabase.GetAll())
{
    if (skill == null) continue;
    if (!skill.isImplemented) continue;
    if (skill.icon == null)
        Debug.LogWarning($"[SKILL_ICON_MISSING] {skill.classType}/{skill.skillName} id={skill.skillId}");
}
```

Acceptance:

```txt
Tüm implemented skill'lerde ya gerçek icon ya temiz fallback var.
Eksik iconlar console'da tek tek listelenir.
Codex ve SkillBar farklı fallback davranışı göstermez.
```

---

## BUG 6 — Kılıç/cliff sorting ve kılıç asset'i kötü

Kullanıcı gözlemi:

```txt
Kılıç ancak köşeye gidince clifflerin arasından görünüyor.
Her zaman floor üstünde doğru açıda olmalı.
Ayrıca mevcut kılıç yeniden çizilmeli.
```

Yapılacaklar:

- Weapon renderer PlayerRoot altında aynı SortingGroup'a bağlanmalı.
- Weapon cliff/floor sorting'in altına düşmemeli.
- Kılıç sprite'ı RIMA 2D top-down/chibi perspektife uygun değilse placeholder olarak değiştirilip yeni üretim işine alınmalı.

Minimum teknik hedef:

```txt
PlayerRoot
  SortingGroup: Characters
  BodyRenderer order 0
  WeaponMountView
    WeaponRenderer order +1 veya per-direction profile
```

Acceptance:

```txt
Weapon floor üstünde görünür.
Cliff arkasından/altından çıkmaz.
Player ile birlikte y-sort eder.
Güney/doğu/batı/kuzey yönlerinde weapon açısı saçma durmaz.
```

---

## BUG 7 — Mor çizginin sağına yürünemiyor / map küçük

Kullanıcı gözlemi:

```txt
Mor debug/aim çizgisinin sağına yürüyemiyorum.
Görselde floor var ama hareket engelleniyor.
Map zaten küçük; combat için bu kadar küçük olmamalı.
```

Yapılacaklar:

- Görsel floor hücreleri ile walkability/collision hücrelerini karşılaştır.
- Aktif `RoomTemplateSO` hangi oda? Logla.
- `IsoRoomBuilder.LastFloorCells`, collision tilemap, WalkabilityMap çıktısı debug overlay ile göster.
- Camera/player clamp varsa sağ çıkıntıyı kesiyor mu kontrol et.

Debug overlay önerisi:

```txt
green = floor/walkable
red = collision/blocked
yellow = player bounds clamp
cyan = exit/portal slots
```

Acceptance:

```txt
Görsel floor olan her yerde player yürüyebilir.
Blocked alanlar görsel olarak void/cliff/obstacle'dır.
Debug aim line yürümeyi engellemez.
Normal combat odası daha geniş ve rahat okunur.
```

---

## Teslim formatı

Patch sonrası kısa rapor yaz:

```txt
Changed files:
- ...

Root cause:
- ...

Verified:
- MainMenu -> CharacterSelect/Chamber -> _Arena
- ESC PauseMenu
- Codex hover clean
- Skill icons deterministic
- Weapon visible above floor
- Walkability matches floor
- Room clear doesn't lock
```
