# ASSET_LOG_TEMPLATE.md
> **Ne zaman yükle:** Yeni ASSET_LOG.md oluştururken. Bu şablon — kopyala, doldur.

---

## TAM ŞABLON (Karakter / Enemy / Boss)

```markdown
# ASSET LOG — [Asset Adı]
Tip: character | enemy | boss | tile | prop | vfx | icon
Aşama: prototype_temp | candidate_base | approved_base | approved_final

---

## Kimlik

character_id: [PixelLab UUID — üretimden hemen sonra yaz, kaybedersen üretimi kaybedersin]
Üretim tarihi: YYYY-MM-DD
Üretici: Claude Code MCP | Kiro MCP | Aseprite Manuel

---

## Üretim Parametreleri

mode: standard | pro
size: [px]
view: low top-down
n_directions: 8
description: >
  [Tam kullanılan description — değiştirme, olduğu gibi yaz]

---

## Animasyonlar

| Template | Yönler | Aşama | Gen | Notlar |
|---|---|---|---|---|
| fight-stance-idle-8-frames | south | animation_test | 1 | titreme yok |
| walking-6-frames | south | animation_test | 1 | onay bekliyor |
| running-6-frames | 8 | candidate_animation | 8 | — |
| lead-jab | 8 | approved_animation | 8 | Unity'de ✓ |
| falling-back-death | 8 | approved_animation | 8 | Unity'de ✓ |
| running-slide | 8 | pending | — | henüz başlamadı |

---

## Kopya Yapısı

ORIGINAL_BASE:  approved_base/[dosya].png
WORKING_COPY:   approved_base/[dosya]_WORKING.png
EXPORT_COPY:    approved_base/[dosya]_EXPORT.png

---

## Türetilen Varyantlar

| Dosya | Kaynak | Yöntem | Aşama |
|---|---|---|---|
| warblade_elite_v1.png | approved_base_v1 | palette swap, Elite turuncu | derived_variant |

---

## Versiyon Geçmişi

| Tarih | Aşama | Açıklama |
|---|---|---|
| YYYY-MM-DD | prototype_temp | İlk standard üretim |
| YYYY-MM-DD | candidate_base | Pro üretim, south review |
| YYYY-MM-DD | approved_base | Kullanıcı onayladı |
| YYYY-MM-DD | approved_animation | Full set onaylandı |

---

## Notlar / Bilinen Sorunlar

- [Varsa: titreme, artifact, palette sorunu, timing notu]
```

---

## HIZLI ŞABLON (Tile / Prop)

```markdown
# ASSET LOG — [isim]
Tip: tile | prop
Aşama: prototype_temp | approved_final

seed: [integer — MUTLAKA YAZ, yoksa üretilemez]
description: "[tam prompt]"
tile_size: [px]
tile_type: square_topdown
outline_mode: segmentation
n_tiles: [N]

ORIGINAL_BASE: [dosya.png]
WORKING_COPY:  [dosya_WORKING.png]
EXPORT_COPY:   [dosya_EXPORT.png]

Üretim: YYYY-MM-DD — Claude Code MCP
Notlar: —
```
