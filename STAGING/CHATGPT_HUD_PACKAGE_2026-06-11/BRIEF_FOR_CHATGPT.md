# RIMA — HUD Tasarım Brief (ChatGPT için)

## Oyun Nedir
**RIMA** — 2D top-down pixel art roguelite. Karanlık fantezi + rift/antigravite teması.
- Görünüm: yüksek açılı top-down 3/4 (~70-80°, Hades / Diablo III benzeri)
- Pixel art, 32x32 tile grid, PPU 64
- URP 2D Renderer + 2D Lights (dinamik aydınlatma)

## Renk Paleti (Act 1 — Shattered Keep)
| Renk | Hex | Kullanım |
|---|---|---|
| Slate | `#3A3D42` | Zemin, duvar taş |
| Void Mor | `#3A1A4A` | Arka plan, void rengi |
| Ember / Turuncu | `#E89020` | Vurgu, rift enerjisi, border |
| Soğuk Cyan | `≤15% kompozisyon` | Lighting aksan |
| Ambient | `0.22 intensity` | Global ışık (karanlık, atmosferik) |

## Karakterler (bu pakette)
- **Warblade** — ağır kılıç, melee tank. LMB: Iron Combo (slash), RMB: Rage Outlet (AoE burst)
- **Elementalist** — büyücü, element geçişi. LMB: Rift Bolt (projectile), RMB: Lightbreak (switch)
- **Ronin** — hızlı samurai
- **Gunslinger** — tabancalı uzak mesafe

Demo'da Warblade + Elementalist açık; diğerleri kilitli.

## Mevcut HUD Durumu (ekran görüntülerinde)
- **Skill bar** (alt orta): 8 slot, slot 0-1 = LMB/RMB (ember kenarlı, daha büyük), slot 2-7 = aktif skill'ler
- **HP bar**: karakter spriteının üstünde, düşman HP'si de aynı şekilde
- **Pause / Codex menü**: void-mor arka plan, ember border, 9-slice panel
- **Class selection / Chamber**: büyük kart tabanlı seçim ekranı

## İstenen: HUD Tasarım Konsepti

Lütfen aşağıdaki HUD elemanları için **wireframe veya basit mockup** çiz:

### Zorunlu Elemanlar
1. **Skill Bar** (alt orta) — LMB/RMB sol başta (daha büyük, ember kenarlı), 6 skill slot yanında
2. **HP / Resource Bar** (sol alt veya alt sol köşe)
3. **Minimap veya Room Map** (sağ üst köşe önerisi)
4. **Boss HP Bar** (üst orta, boss odalarında)
5. **Gold / Rune sayacı** (sağ üst)

### Stil Kısıtları
- Pixel art estetik — keskin kenarlı, low-res doku tercih
- Renk: void mor (`#3A1A4A`) panel arka planı, ember (`#E89020`) vurgu/border
- Minimal: gereksiz dolgu yok, atmosferik karanlık
- Referans oyunlar: Hades, Dead Cells, Enter the Gungeon HUD sadeliği

### Opsiyonel (çizersen güzel olur)
- Status effect ikonları (zehir, buz, yanış vs.)
- Dual-class indicator (Warblade + Elementalist kombine class için)
- Rift charge / özel resource bar (sınıfa göre farklı renk)

## Ekli Dosyalar
```
char_warblade_south.png       — Warblade güney yönü sprite
char_elementalist_south.png   — Elementalist güney yönü sprite
char_ronin_south.png          — Ronin güney yönü sprite
char_gunslinger_south.png     — Gunslinger güney yönü sprite
fracture_imp.png              — Düşman örneği (Act 1)
relic_caster.png              — Düşman örneği (caster)
... (diğer mob'lar)

screen_01_arena_combat.png    — Arena combat (aydınlatmalı)
screen_02_mainmenu.png        — Ana menü estetiği
screen_03_charselect.png      — Karakter seçim ekranı
screen_04_hpbar.png           — Mevcut HP bar
screen_05_skillbar_hover.png  — Skill bar hover durumu
screen_06_chamber_dualclass.png — Dual class selection UI
screen_07_arena_room_combat.png — Oda muharebe görünümü
```
