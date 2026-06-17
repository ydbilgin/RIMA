# 4 — Director Mode Redesign

## Karar

Director Mode “fantasy HUD ile çevrilmiş debug panel” olmamalı. **RIMA renklerine sahip profesyonel bir runtime editor** olmalı.

Mevcut sistemde korunacak işlevler:

- Spawn
- Class & Build
- Stats
- Build Mode
- Map
- Telemetry
- Play/paused test state
- Selection inspector
- Quick reset

Değişecek olan bunların yerleşimi ve görsel ağırlığıdır.

## 1920×1080 hedef layout

| Bölge | Ölçü | İşlev |
|---|---:|---|
| Top app bar | 56 px | Room/breadcrumb, mode state, Play/Step/Reset, Save/Export |
| Sol icon rail | 64 px | Playtest, Spawn, Class, Stats, Build, Map, Telemetry |
| Sol contextual library | 280–320 px | Arama, filtre, kartlar, presets |
| Orta viewport | kalan alan | Dünya ve placement feedback |
| Sağ inspector | 320–360 px | Seçim, statlar, transform, validation |
| Bottom status | 28–32 px | Hotkey, selected tool, cell, undo, error |
| Telemetry drawer | 200–240 px, kapalı varsayılan | Grafik/log gerektiğinde açılır |

Görsel: `visuals/director_mode_proposed_layout.png`

## Tasarım kuralları

### 1. Tek çerçeve, çok panel değil

- Tam viewport etrafındaki kalın turuncu/mor frame kaldırılmalı.
- Küçük butonların her birine 9-slice süslü frame uygulanmamalı.
- Ana container'larda 1 px slate border yeterli.
- RIMA chrome yalnız başlık/logo veya birincil action'da kullanılmalı.

### 2. Renk hiyerarşisi

- Base surface: `#11131A`
- Panel: `#1B1F28`
- Raised control: `#252A35`
- Border: `#343B48`
- Selection / valid state: cyan `#55D6E3`
- Primary destructive/reset or commit: ember `#C8742A`
- Perfect/locked/system state: void purple `#9B5DE5`
- Text primary: bone `#F2EFE8`
- Text secondary: `#9AA3B2`

Cyan ekranın %15'ini geçmemeli. Turuncu her panelin iskeleti değil, karar rengidir.

### 3. Font ve spacing

- Başlık: 20–24 px
- Panel heading: 16–18 px
- Body/input: 14–16 px
- Status/hotkey: 12–13 px minimum
- Control height: 36–40 px
- Card: yaklaşık 116×128 px veya liste satırı 48–56 px
- 8 px temel spacing sistemi: 8/16/24/32

Mevcut minik pixel font, tool kullanımında estetik değil engeldir.

## Spawn workspace

### Sol panel

- Search
- All / Mob / Elite / Boss filters
- 2-column enemy cards
- Card: thumbnail, display name, role, threat point
- Selected card: 2–3 px cyan outline + check/active mark

### Viewport

- Cursor altında ghost marker
- `Valid / Invalid` state
- Cell coordinate
- Collision/room bounds reason
- Spawned entity seçildiğinde outline

### Sağ inspector

- Entity name + role
- HP, damage, threat, count
- Facing
- spawn rules/caps
- Place Selected / Delete

## Stats workspace

- `09` screenshot yeniden çekilmeli.
- Sol tarafta stat group listesi: Core, Offense, Defense, Resource, Movement
- Ortada veya sağ inspector'da numeric fields
- Değişen değer amber flash ile 0.5 sn vurgulanmalı
- `physPower 177` açıkça görünmeli
- Reset to class default action ayrı ve geri alınabilir olmalı

## Telemetry

Telemetry kalıcı kutu olmamalı. Bottom drawer olarak açılmalı:

- DPS
- hit count
- damage type distribution
- last 20 events
- CSV Export
- Clear

Demo quick-win: tablo + summary. Grafik POST.

## Uygulama yaklaşımı

**Demo öncesi tam kod refactor yapma.** Mevcut tab/panel logic'i koru; yeni parent shell altında yeniden anchor et.

1. Existing buttons aynı callbacks'i kullanır.
2. Decorative frame objects disable edilir.
3. Shared `DirectorPanel`, `DirectorButton`, `DirectorCard`, `DirectorInput` prefabs oluşturulur.
4. Spawn ve Stats panelleri bu shared prefablarla yeniden skinlenir. Build Mode kendi isometric authoring viewport ve panel geometrisini korur; yalnız ortak top/status chrome paylaşabilir.
5. Assert'ler aynen korunur.

## Acceptance criteria

- 1920×1080'de hiçbir body text 12 px altına düşmez.
- Viewport ekranın en az %55'ini kaplar.
- Selection, hover, pressed, disabled state görsel olarak ayrıdır.
- Spawn işlemi sonrası status bar count artışını gösterir.
- Stats sekmesinde `physPower=177` görünür.
- Telemetry kapalıyken viewport'u işgal etmez.
- Play/Paused state tek bakışta anlaşılır.
