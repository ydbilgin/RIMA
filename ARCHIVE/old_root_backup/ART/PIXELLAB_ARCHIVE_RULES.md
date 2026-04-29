# PIXELLABARCHIVE_RULES.md
> **Ne zaman yükle:** Asset kaydederken, klasör yapısını düzenlerken.

---

## DOSYA İSİMLENDİRME

```
[isim]_[yön]_[aşama]_v[N].png

Örnekler:
  warblade_south_prototype_temp_v1.png
  warblade_south_candidate_base_v1.png
  warblade_south_approved_base_v1.png
  shardwalker_south_prototype_temp_v1.png
  floor_stone_seed42_prototype_temp_v1.png
```

ZIP dosyaları:
```
  warblade_animation_test_idle_v1.zip
  warblade_approved_animation_full_v1.zip
```

---

## KOPYA MANTIĞI — HER ASSET 3 KATMAN

Riskli edit öncesi her zaman bu yapı korunur:

```
ORIGINAL_BASE  → hiç dokunulmaz, referans
WORKING_COPY   → üzerinde çalışılan
EXPORT_COPY    → Unity/üretim için çıkan
```

Pratikte:
```
warblade_south_approved_base_v1.png          ← ORIGINAL_BASE (dokunma)
warblade_south_approved_base_v1_WORKING.png  ← WORKING_COPY (edit burada)
warblade_south_approved_base_v1_EXPORT.png   ← EXPORT_COPY (Unity'e bu gider)
```

---

## KLASÖR YAPISI

```
ART/
├── karakterler/[isim]/
│   ├── prototype_temp/
│   ├── candidate_base/
│   ├── approved_base/
│   ├── animation_test/
│   ├── candidate_animation/
│   ├── approved_animation/
│   ├── derived_variant/
│   └── ASSET_LOG.md
├── dusmanlar/[isim]/   (aynı yapı)
├── environment/
│   ├── tileset/act1_stone/seed42/
│   └── props/
├── _REVIEW/            ← south.png onay bekleme alanı
│   ├── player_classes/
│   ├── mobs/
│   └── bosses/
└── _ARSIV/             ← silinmez, referans için
```

---

## ONAY ÖNCESİ KURAL

```
candidate_base veya candidate_animation aşamasındaki dosyalar:
  → approved_base veya approved_animation klasörüne TAŞINMAZ
  → üzerinden büyük üretim yapılmaz
  → senden açık "onaylıyorum" veya "devam" gelmeden sonraki adım atılmaz
```

---

## KAYDETME KURALLARI

### Karakter üretiminde
1. `create_character` → `character_id`'yi ASSET_LOG'a yaz (kaybet = karakteri kaybet)
2. ZIP indir → `prototype_temp/` veya `candidate_base/` klasörüne çıkart
3. South.png → `_REVIEW/[tip]/` altına kopyala → senden geri bildirim bekle

### Tile üretiminde
1. `create_tiles_pro` → kullanılan `seed` + `n_tiles` + `description` ASSET_LOG'a yaz
2. Tile klasörünü `seed[N]/` ile adlandır → aynı seed = aynı sonuç

### Riskli edit öncesi
```
Edit = palette swap, merge, cleanup, resize, layer işlemi

ADIM 1: File → Save As → [dosya]_BACKUP.png
ADIM 2: Edit yap
ADIM 3: Export kopyasını çıkart
Yanlış giderse: BACKUP'tan geri dön
```

---

## NE ZAMAN SİLEBİLİRSİN

Yalnızca şu koşulların **hepsi** sağlanırsa:
1. `approved_final` aşamasına ulaşıldı
2. Unity'de çalışıyor, test edildi
3. ASSET_LOG'da kayıtlı
4. Sen açıkça "silinebilir" dedin

Şüphe varsa: **silme, _ARSIV/ altına taşı.**
