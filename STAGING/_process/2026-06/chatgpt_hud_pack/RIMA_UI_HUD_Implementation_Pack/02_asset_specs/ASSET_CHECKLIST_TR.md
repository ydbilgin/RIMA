# RIMA HUD / UI Asset Checklist

Bu paket, mevcut ekranlarını **bitmiş oyun HUD’ına** çevirmek için gereken asset listesini ve referans mockup görsellerini içerir. İnsanlık yine kırmızı debug karelerini “UI” sanarak ilerlemiş, ama toparlıyoruz.

## 1. Ortak UI atlası

Tek bir ana atlas önerisi:

`UI_Atlas_ObsidianRift_1024.png` veya büyükse `2048.png`

İçinde şunlar olmalı:

| Asset | Önerilen boyut | Kullanım | Not |
|---|---:|---|---|
| `panel_9slice_small` | 96×96 | küçük HUD kutuları | 16 px border |
| `panel_9slice_large` | 160×160 | pause/codex/reward panel | 24 px border |
| `panel_inner_dark` | 64×64 | panel iç zemini | tiled veya sliced |
| `border_edge_horizontal` | 128×16 | üst/alt çerçeve | taş/metal |
| `border_edge_vertical` | 16×128 | yan çerçeve | taş/metal |
| `corner_tl/tr/bl/br` | 32×32 | panel köşeleri | cyan çatlak detayı |
| `divider_line_cyan` | 256×8 | başlık altı çizgisi | codex/pause |
| `divider_line_amber` | 256×8 | aktif vurgu çizgisi | başlık/hover |
| `ornament_gem_cyan` | 32×32 | kart/panel merkezi | rift kristali |
| `ornament_arrow_left/right` | 24×24 | seçili pause item | amber ok |
| `close_button_normal/hover` | 40×40 | codex kapatma | X butonu |
| `modal_dim_overlay` | 1×1 | pause/codex arkası | siyah, alpha ile |

## 2. Gameplay HUD assetleri

### Sol üst oyuncu paneli
Gerekenler:

- `hud_portrait_frame_96`
- `hud_level_badge_48`
- `bar_frame_hp_280x24`
- `bar_fill_hp_red`
- `bar_frame_resource_280x20`
- `bar_fill_resource_blue`
- `resource_chip_frame`
- `room_chip_frame`
- `objective_bullet_diamond`

Kullanım:
- Portrait + HP/Mana sol üstte kalmalı.
- `RIMA / WARBLADE` metni portrait altında okunmalı.
- Oda bilgisi ayrı chip: `ODA 1 / 6 — SAVAŞ`.
- Hedef metni küçük ama net: `Tüm düşmanları ortadan kaldır.`

### Sağ üst minimap
Gerekenler:

- `minimap_frame_280x220`
- `minimap_bg_black`
- `minimap_room_tile`
- `minimap_player_marker`
- `minimap_door_marker`
- `minimap_enemy_marker`
- `key_hint_small_M`

Not: Minimap gerçek veriden çizilecekse markerlar sprite olmalı, panel 9-slice olmalı.

### Alt hotbar
Gerekenler:

- `hotbar_slot_normal_72`
- `hotbar_slot_active_72`
- `hotbar_slot_hover_72`
- `hotbar_slot_disabled_72`
- `hotbar_slot_cooldown_overlay`
- `keycap_small`
- `count_bubble`
- `cooldown_radial_mask` veya shader/material
- `resource_counter_frame`
- `key_hint_TAB`
- `key_hint_ESC`

Slot düzeni:
- LMB: Temel saldırı
- RMB: Hızlı/ikincil yetenek
- Q: Yarık adımı
- E: Demir kalkan
- R: Yer yarma
- F: İksir

## 3. Reward card assetleri

Üç kart ekranı için prefab üret:

`RewardCard.prefab`

Gereken sprite parçaları:

| Asset | Boyut | Not |
|---|---:|---|
| `reward_card_frame_9slice` | 180×180 | 24 px border |
| `reward_card_inner_gradient` | 128×128 | koyu teal/black |
| `reward_rarity_ribbon_common` | 112×28 | gri |
| `reward_rarity_ribbon_rare` | 112×28 | cyan |
| `reward_rarity_ribbon_epic` | 112×28 | mor |
| `reward_class_tag_warblade` | 96×22 | amber |
| `reward_synergy_icon` | 18×18 | küçük cyan kristal |
| `reward_select_button_normal` | 180×52 | cyan |
| `reward_select_button_hover` | 180×52 | parlak cyan |
| `reward_select_button_pressed` | 180×52 | koyu cyan |
| `reward_card_top_gem` | 40×40 | merkez süs |

Ek:
- Kart ikonları mevcut skill iconlarından gelir.
- Açıklama metni 3 satırı geçmemeli. Yoksa yine PowerPoint slaytına roman sıkıştıran insanlık kazanır.

## 4. Pause menu assetleri

`PauseMenu.prefab`

Gerekenler:

- `pause_panel_9slice_520x560`
- `pause_title_divider`
- `pause_button_normal_340x58`
- `pause_button_hover_340x58`
- `pause_button_pressed_340x58`
- `pause_button_disabled_340x58`
- `pause_selected_left_arrow`
- `pause_selected_right_arrow`
- `pause_bottom_gem`
- `screen_vignette_overlay`

Butonlar:
- DEVAM ET
- AYARLAR
- YETENEK KODEKSİ
- ANA MENÜ
- ÇIKIŞ

## 5. Yetenek Kodeksi assetleri

`SkillCodex.prefab`

Gerekenler:

| Asset | Boyut | Kullanım |
|---|---:|---|
| `codex_window_9slice` | 220×220 | büyük ana panel |
| `codex_title_bar` | 1024×72 | üst başlık |
| `class_tab_normal` | 220×52 | class tab |
| `class_tab_selected` | 220×52 | seçili Warblade |
| `class_tab_hover` | 220×52 | mouse hover |
| `skill_row_normal` | 1200×62 | liste satırı |
| `skill_row_hover` | 1200×62 | hover satırı |
| `skill_row_selected` | 1200×62 | seçili satır |
| `skill_icon_frame_small` | 52×52 | ikon yuvası |
| `rarity_chip_common` | 90×28 | Common |
| `rarity_chip_rare` | 90×28 | Rare |
| `rarity_chip_epic` | 90×28 | Epic |
| `cooldown_chip` | 96×28 | CD 8S |
| `scrollbar_track` | 16×512 | sağ scroll |
| `scrollbar_handle` | 20×72 | scroll tutacağı |
| `scrollbar_arrow_up/down` | 20×20 | oklar |

Class iconları:
- Warblade sword
- Elementalist orb
- Shadowblade daggers
- Ranger bow
- Ravager axes
- Ronin mark
- Gunslinger pistol
- Brawler fist
- Summoner sigil
- Hexer hex star

## 6. Debug / placeholder temizliği

Mevcut ekranda görünen kırmızı kareler production asset değil. Bunları ya tamamen gizle ya da şunlarla değiştir:

- `enemy_spawn_marker_hidden_runtime`: editörde görünsün, oyunda görünmesin.
- `enemy_spawn_warning_rune_red_64`: düşman spawn telegraph gerekiyorsa kırmızı kare yerine rune.
- `interact_marker_cyan_diamond_24`: kapı/portal etkileşim noktası.
- `room_reward_marker_amber_32`: ödül seçilebilir işaret.

## 7. Font / text

Font dosyasını bu pakete koymadım. Lisans cehennemiyle uğraşmayalım, o da ayrı bir insan icadı.

Öneri:
- Başlık: pixel serif / gothic pixel font
- Body: okunabilir pixel sans
- Unity tarafında TextMeshPro SDF üret
- Türkçe karakter desteği şart: `ğ ü ş ı ö ç İ`

## 8. Sprite import kuralları

Unity import ayarları:

- Texture Type: Sprite (2D and UI)
- Filter Mode: Point
- Compression: None
- Generate Mip Maps: Off
- Mesh Type: Full Rect
- Pixels Per Unit: UI için 100 veya proje standardın
- Sprite Editor: 9-slice borderları kesin tanımla
- Atlas: `UI_Core`, `UI_Icons`, `UI_SkillIcons`, `UI_ClassIcons`

## 9. Öncelik sırası

1. UI Core Atlas: panel, border, button, slot, keycap.
2. Gameplay HUD prefab.
3. Reward Card prefab.
4. Pause Menu prefab.
5. Skill Codex prefab.
6. Minimap marker sistemi.
7. Cooldown overlay ve hover/selected animasyonları.
8. Kırmızı debug karelerini oyundan söküp at. Törenle.
