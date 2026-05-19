# RIMA — Wall + Object Production Plan (S95 Design Lock)
> Karar tarihi: 2026-05-20 | Onay bekliyor | Üretim BAŞLAMADI

---

## 1. Duvar Mimarisi

### Soru: İsometric Tile mi, Object mi?

**Cevap: İkisi birden (Hibrit)**

| Katman | Araç | Ne üretir | Amaç |
|---|---|---|---|
| L2a — Wall Base | `create_isometric_tile` + thickness 0.15 | Alçak blok, zemin sınırı | Collider footprint + otomatik AutoConnect |
| L2b — Wall Yüzeyi | `create_object` tall sprite | 64×128 px dikey sprite | Görsel duvar yüzeyi, Hades-style |

### Soru: Corner = ayrı sprite mi, 90° rotation mı?

**Cevap: Ayrı sprite gerekiyor.**

İsometric perspektif sprite'a baked in — doğu yüzeyini gösteren duvar ile kuzey yüzeyini gösteren duvar aynı piece'in rotasyonu değil, farklı perspektif. RuleTile ile otomatik seçim yapılabilir.

**Temel piece listesi:**

| Sprite | Açıklama |
|---|---|
| `wall_face_NS` | Doğu-batı uzanan duvar, güney yüzü kameraya bakıyor |
| `wall_face_EW` | Kuzey-güney uzanan duvar, doğu yüzü kameraya bakıyor |
| `wall_corner_outer` | Dış köşe (iki yüzey buluştuğu nokta, ikisi de görünür) |
| `wall_corner_inner` | İç köşe (içbükey köşe) |
| `wall_end_cap` | Duvar ucu (körlemesine biten) |
| `wall_arch_open` | Kapı/geçit kemeri (Karar #149 mirror archway ile uyumlu) |

### Damaged / Ruined Variants

Her temel piece için 2 durum:
- `intact` — normal
- `damaged` — üst kısmı kırık, taşlar dağılmış

Ek: **Interior Ruined Wall** — duvarsın sadece alt yarısı kalmış, yıkılmış oda içi. Standalone sprite (full room interior ile birlikte). Referans görseldeki gibi içten görülen yıkık duvar hissi.

### Act 1 Material
- Granite `#3A3D42` base, cyan `#00FFCC` accent (kırıklarda, mortar aralıklarında)
- Kırık yüzeylerde exposed stone rengi `#5A5F66`

---

## 2. Obje Boyut Standardı

### Sorun
Grid transform `(1, 0.5, 1)` non-uniform scale — Tilemap child'ları Y'de %50 eziliyor. Prop'ların Tilemap parent'ından çıkarılması gerekiyor.

### Yeni Standard

| Kategori | PixelLab Boyutu | World Space | Açıklama |
|---|---|---|---|
| Floor decal / küçük prop | 32×32 px | 0.5 × 0.5 hücre | Taş, kırık parça, kan lekesi, rün |
| Orta prop | 64×64 px | 1 × 1 hücre | Sandık, kazan, kırık fıçı |
| Büyük prop (dikey) | 64×128 px | 1w × 2h hücre | Sütun, altar, büyük heykel |
| Boss prop / landmark | 128×128 px | 2 × 2 hücre | Tahta, dev kazan, asma kafes |
| Mob sprite | 64×64 px | 1 × 1 hücre | Karar #100 64px chibi |
| Item pickup | 32×32 px | 0.5 × 0.5 hücre | Küçük, yerden alınabilir |
| Item (büyük, chest) | 64×64 px | 1 × 1 hücre | — |

**Unity fix:** Prefab placement sırasında `Props_Root` (identity transform, scene root child) kullan, Tilemap parent altına KOYMA.

---

## 3. Duvara Asılan Işıklar — RIMA-Özgün Void Flame

### Konsept
Standart meşale yok — **Void Flame**: boşluktan sızan, fizik dışı bir enerji alevi. Act'e göre rengi değişir:

| Act | Renk | Unity Light Color | Efekt |
|---|---|---|---|
| Act 1 Shattered Keep | Cyan void | `#00FFCC` | Titreyen soluk mavi-yeşil |
| Act 2 Bleeding Wastes | Rust ember | `#C8502A` | Nabız atan kırmızı-turuncu |
| Act 3 Core Approach | Gold rift | `#FFD700` | Keskin altın puls |

### Üretim
- Araç: `create_object` + state (`mounted_lit`, `mounted_dim`, `floor_stand_lit`)
- Boyut: 32×64 px (duvara asılı, ince dikey)
- State'ler: `wall_sconce_lit`, `wall_sconce_unlit`, `floor_torch_lit`

### Unity Entegrasyon
Her Void Flame prefab'ına:
```
VoidFlame (SpriteRenderer)
  └── PointLight2D
        color: Act rengi
        intensity: 0.8
        outerRadius: 2.5 world units
        flickerScript: VoidFlameFlicker.cs (sin wave, 0.1-0.3 amplitude, 1.5-3 Hz)
```
`VoidFlameFlicker.cs` — 20 satır max, Codex yazar.

---

## 4. Interior Ruined Room Pieces

Referans görseldeki "yıkılmış oda içi" hissi için:

| Piece | Açıklama |
|---|---|
| `ruin_half_wall_NS` | Yarı yıkık NS duvar, üstten kırık |
| `ruin_half_wall_EW` | Yarı yıkık EW duvar |
| `ruin_rubble_pile_sm` | 32×32 küçük moloz yığını (zemin prop) |
| `ruin_rubble_pile_lg` | 64×64 büyük moloz (geçit bloker) |
| `ruin_collapsed_ceiling` | 64×96 tavan enkaz (oda içi atmosfer) |

Bu piece'ler `wall_damaged` state'inden ayrı — tam yıkılmış alanlar için.

---

## 5. Üretim Kuyruğu (Onay Sonrası)

### Sıra 1 — Duvar Base (L2b Object, önce bunlar)
Toplam: ~16-20 PixelLab gen

1. `wall_face_NS` intact
2. `wall_face_NS` damaged
3. `wall_face_EW` intact
4. `wall_face_EW` damaged
5. `wall_corner_outer` intact
6. `wall_corner_outer` damaged
7. `wall_corner_inner`
8. `wall_end_cap` NS + EW (2)
9. `wall_arch_open` (Karar #149 mirror archway)
10. Interior ruined half-walls (2)

### Sıra 2 — Void Flame
Toplam: ~6-8 gen
- 3 state × Act 1 material

### Sıra 3 — Floor Props (32×32 ve 64×64)
Toplam: ~20-30 gen
- Zemin props, küçük dekorlar, moloz yığınları
- Item pickup'lar

### Toplam Tahmini
~42-58 gen. Kalan bütçe: 2,500 gen. **Yeterli.**

---

## 6. Onay Gereken Kararlar

- [ ] **Hibrit L2a+L2b yaklaşımı** onaylı mı?
- [ ] **Corner = ayrı sprite** onaylı mı?
- [ ] **Void Flame** konsept onaylı mı, RIMA'ya has mı?
- [ ] **32×32 zemin / 64×64 mob** boyut standardı onaylı mı?
- [ ] **Yıkık iç duvar pieces** scope'a dahil mi?
- [ ] Üretim sırası (duvar önce mi, ışık önce mi, prop önce mi)?

---

> **Durum:** TASARIM LOCK BEKLIYOR — kullanıcı onayı olmadan üretim başlamaz.
> Eski dosyalar arşive taşınacak: `STAGING/OPUS_WALL_FINAL_DECISION.md` + `STAGING/OPUS_WALL_PRODUCTION_DESIGN.md` → `STAGING/_archive/walls_pre_iso/`
