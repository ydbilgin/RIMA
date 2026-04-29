# PIXELLAB_PROTOTYPE_VS_FINAL.md
> **Ne zaman yükle:** Asset hangi aşamada üretileceğine karar verirken.

---

## AŞAMA SİSTEMİ

| Aşama | Anlam | Kim koyar | Bir sonraki |
|---|---|---|---|
| `prototype_temp` | İlk hızlı üretim, test için | Claude Code (MCP) | Senden onay → `candidate_base` |
| `candidate_base` | Onay bekliyor, yönlendirme yapılabilir | Claude Code | Senden onay → `approved_base` |
| `approved_base` | Sen onayladın → animasyon/varyant zinciri başlar | Sen | → `derived_variant` / `animation_test` |
| `derived_variant` | `approved_base`'den türetilmiş varyant | Claude Code | Senden onay → `approved_final` |
| `animation_test` | Test amaçlı animasyon, henüz onaylı değil | Claude Code (MCP) | Senden onay → `candidate_animation` |
| `candidate_animation` | Animasyon onay bekliyor | Claude Code | Senden onay → `approved_animation` |
| `approved_animation` | Onaylı animasyon, Unity'e alınabilir | Sen | Unity export |
| `approved_final` | Tüm aşamalar onaylı, shipmaya hazır | Sen | — |

---

## KURALLAR

1. `approved_base` olmadan büyük animasyon zinciri kurma
2. `animation_test` üretilebilir ama bunları final gibi işleme
3. Her aşama geçişi için senden açık onay beklenir ("onaylıyorum", "devam", vb.)
4. `candidate_base` → `approved_base` geçişi: south.png review zorunlu
5. Zayıf base üzerinde animasyon üretme — önce base kalitesi kararı

---

## PROTOTYPE vs FINAL KARŞILAŞTIRMA

| | Prototype | Final |
|---|---|---|
| `create_character` mode | standard | pro |
| Yön | 8 (artık standart) | 8 |
| Animasyon | 2 template test | 6 template full set |
| Gen maliyeti (karakter) | 8 gen | ~30 gen + 48 anim gen |
| Aseprite cleanup | Gerekirse | Gerekirse |
| Unity'de kullanım | Test | Ship |
| Aşama etiketi | `prototype_temp` | `approved_final` |

---

## UPGRADE AKIŞI

```
1. create_character standard → prototype_temp
2. Sen south.png görürsün → [onaylıyorsun veya prompt değiştirir]
3. Onay → create_character pro → candidate_base → south.png review
4. Sen onaylarsın → approved_base
5. animate_character template → animation_test (2 template, kısa kontrol)
6. Sen onaylarsın → candidate_animation → full set
7. Sen full set onaylarsın → approved_animation
8. Unity export → approved_final
```

---

## KLASÖR YAPISI

```
ART/karakterler/warblade/
├── prototype_temp/
│   └── warblade_south_std_v1.png
├── candidate_base/
│   └── warblade_south_pro_v1.png     ← review bekleniyor
├── approved_base/
│   └── warblade_all_pro_v1.zip       ← sen onayladın
├── animation_test/
│   └── warblade_idle_test.zip
├── candidate_animation/
│   └── warblade_animations_v1.zip
├── approved_animation/
│   └── warblade_animations_final.zip ← Unity'e alınacak
└── ASSET_LOG.md
```
