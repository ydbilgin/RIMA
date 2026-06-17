# 6 — Game UI Redesign

## HUD

Görsel: `visuals/combat_hud_proposed_markup.png`

### Demo ölçüleri

| Eleman | Hedef |
|---|---:|
| HP bar | 200–220 × 14–16 px |
| Resource bar | 150–170 × 8–10 px |
| LMB/RMB slot | 52–56 px |
| Q/E/R/F slot | 44–48 px |
| Key label | 12–14 px |
| Cooldown number | 16–18 px |

`HUD_DESIGN_SPEC.md` içindeki 72×4 px HP bar, mevcut 1080p testte başarısız. Bu ölçü demo için override edilmelidir.

### Zorunlu state'ler

- HP current/max veya en az okunabilir current + bar
- class resource
- cooldown sweep + number
- key label
- disabled/empty
- Perfect Condition pulse
- ulti lock icon ve armed cue (`MASTER #54` uyumu)

### Low HP

- Full-screen red overlay kaldır.
- Kenar vignette %12–18 opacity.
- <%20: 0.8–1.0 sn pulse.
- Merkez ve düşman telegraph'ları temiz kalmalı.

## Draft

Her kartta sırayla:

1. Rarity
2. Offer type: New Skill / Upgrade / Passive
3. Icon
4. Skill name
5. One-line verb summary
6. Exact effect
7. Exact synergy condition
8. Equip/replace outcome
9. Select prompt

### Yasak metin

`Gravity Cleave ile eşleşir.`

### Doğru metin

`Iron Charge sonrasında kullanılırsa çekilen hedefler 1.5 sn sersemler.`

İlk metin bir ilişki grafiği varmış gibi davranıyor ama oyuncuya aksiyon öğretmiyor.

## Codex

Demo quick-win:

- row height 56–64
- icon 40–44
- rarity left stripe
- selected class button fill/outline
- description font büyüt
- sağda cooldown/tag group hizala

Post-demo:

- left class rail
- center skill list
- right detail pane
- search/filter

## Pause + Settings

- Pause panel 420–480 px
- 48 px row height
- Settings 1.25× büyüt
- ON/OFF toggle ikon + text
- Keybind conflict state
- Shared scrim and shared button prefab

## Merchant

### Demo quick-win

- Renkli kareleri gerçek item ikonuyla değiştir.
- Tek yakın item için context card göster.
- İsim, fiyat, etki, `E Satın Al` aynı card içinde.
- Affordable: cyan/amber; unaffordable: muted + fiyat kırmızı değil, düşük opacity.

### Post-demo

- fiziksel pedestal art
- merchant NPC
- compare tooltip
- reroll/sold state

## Boss

### P0 düzeltmeler

1. Boss sprite pivot/PPU/scale düzelt.
2. Sprite tamamen oyun alanında kalmalı.
3. HUD'u kapatmamalı.
4. Merchant standlarını boss state başlangıcında temizle.
5. Neon yeşil barı değiştir.
6. Subtitle güvenli alana taşı.

### Health bar

- 720–900 px genişlik
- 16–22 px fill
- stone/slate frame
- crimson veya desaturated amber/red fill
- phase notches: %66 / %33
- name + phase label

### Intro

- 0.6–1.0 sn camera focus
- input lock kısa ve net
- boss name reveal
- subtitle bottom-center, skill barın üstünde
- sonra camera normal gameplay framing'e döner

## Death screen

- Başlık
- run summary group
- Echo kazanımı ayrı highlight
- Retry primary
- Main Menu secondary
- sonuç metinleri sol hizalı veya iki sütun

## Main menu

Backdrop korunmalı. Yalnız:

- logo %15–20 büyüt
- selected row state güçlendir
- menü satır yüksekliği artır
- küçük flavor text'i ya kaldır ya okunabilir yap
