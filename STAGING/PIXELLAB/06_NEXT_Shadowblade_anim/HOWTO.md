# Class Animation HOWTO — Master Pipeline
*Source: `GUIDES/RIMA_MASTER_ART_PIPELINE.md` (LOCKED v1) + `TASARIM/ANIMATION_BIBLE.md`*

Bu HOWTO 4 class klasörü için **ortaktır**. Sadece `PROMPT.md` class-spesifik.

---

## Pipeline (5 Adım, Web App ZORUNLU)

> **MCP `animate_character` KULLANMA** — 4-frame limit + VFX bug. Tüm karakter anim'i Web App üzerinden.

### Adım 1 — Base 4-yön (Create Character Pro New)
- Web App → Create Character Pro New
- **Body-only** prompt (silahsız!)
- Canvas: 252×252
- Hero Anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png` yükle
- Yönler:
  - **Simetrik class (Warblade):** S, E, N üret. **W = Unity flipX** (üretme!)
  - **Asimetrik class (Ranger / Shadowblade / Elementalist):** S, E, N, W **dördünü ayrı** üret
- Çıktı → `outputs/01_base_4dir/<class>_base_<dir>.png`

### Adım 2 — Idle / Hurt / Death (Animate with Text NEW)
- Adım 1'in idle sprite'ı = referans
- **Idle:** 6-8 frame, "subtle breathing motion"
- **Hurt:** 3 frame, "flinch backwards" (Scar Memory 1.2s zorunlu)
- **Death:** 6 frame, "collapse to ground"
- Her yön ayrı (S/E/N + W asimetrikse)
- Çıktı → `outputs/02_idle_hit_death/<class>_<anim>_<dir>.png`

### Adım 3 — Walk/Run Cycle (Brian's Extreme Pose Method)
- Animate with Text NEW: "running forward, leg extended" → 12 frame al
- En uç poz seç (diz en yukarıda) = **Extreme Pose A**
- Pose A'yı yatay flip → **Extreme Pose B**
- Interpolate NEW: A → B arası 4-6 frame
- Toplam: **6 frame walk cycle / yön**
- Çıktı → `outputs/03_run_cycle/<class>_walk_<dir>.png`

### Adım 4 — Action (3-Segment KF+Interpolate)
**Attack_LMB, Attack_RMB, Dash için aynı pattern:**

```
4.1 — PEAK frame üret (en dramatik an: impact / burst peak)
      Tool: Animate with Text NEW (tek frame mod) veya Edit Image Pro
      Output: 1 keyframe

4.2 — START → PEAK segment
      Tool: Interpolate NEW
      Input: idle sprite + PEAK frame
      Output: 4 frame windup/anticipation

4.3 — PEAK → END segment
      Tool: Interpolate NEW
      Input: PEAK frame + recovery pose
      Output: 4 frame follow-through/recovery
```

**Toplam:** 1 + 4 + 4 = 9 frame her aksiyon, her yön.

- Çıktı → `outputs/04_attack_LMB/<class>_lmb_<dir>.png` (vs 05_attack_RMB, 06_dash)

### Adım 5 — Weapon Pass (Edit Image Pro)
**Body-only animation seti hazır olunca, silahı son adım olarak ekle.**

- Idle S frame'i seç
- Edit Image Pro: "add greatsword on right shoulder" prompt
- Tek frame'de silah eklenir → diğer frame'lere propagate
- **Asimetrik:** her yön (S/E/N/W) için 1 base inpaint, sonra propagate

**İstisnalar:**
- **Elementalist:** Silahsız (büyü el jestleri) → Adım 5 ATLA
- **Ronin:** Silah hep kında (idle/run'da kın görünür, sadece skill'de çekilir) — Ronin v2'de

- Çıktı → `outputs/07_weapon_pass/<class>_weapon_<dir>.png`

---

## Unity Standartları (LOCKED, sapma yasak)

| Setting | Value |
|---|---|
| Texture Type | Sprite (2D and UI) |
| PPU | **64** |
| Sprite Mode | **Multiple** |
| Frame size (Unity-side) | **128×128 per cell** |
| Pivot | Center (0.5, 0.5) |
| Filter Mode | **Point** |
| Compression | **Uncompressed** |

**Canvas Boyut Akışı:**
- PixelLab v3 üretimi: **252×252** (otomatik, silah headroom)
- Unity import: **128×128** (Aseprite'ta merkez-kırp)

---

## 4 Yön Sistemi

```
S = south (kameraya, default)
E = east (sağa)
N = north (sırt kameraya)
W = west (sola)
```

Diagonal hareket → en yakın cardinal'e snap (45° threshold + hysteresis).

---

## 7 Anim Listesi (Animation Bible LOCKED)

| # | Anim | Frame | Adım |
|---|---|---|---|
| 1 | Idle | 6-8 | Adım 2 |
| 2 | Walk | 6 / yön | Adım 3 |
| 3 | Attack_LMB | 9 (1+4+4) | Adım 4 |
| 4 | Attack_RMB | 9 (1+4+4) | Adım 4 |
| 5 | Dash | 4 | Adım 4 |
| 6 | Hurt | 3 | Adım 2 |
| 7 | Death | 6 | Adım 2 |

---

## QC Checklist (her sprite/anim sonrası)

- [ ] **Silüet:** Squint test — 1 sn'de class kim?
- [ ] **4 yön tutarlılığı:** Boyut + orantı + palette
- [ ] **Embedded VFX YASAK:** Karakter sprite'ında glow/particle YOK (VFX engine-side)
- [ ] **Body-only (Adım 1-4):** Silah YOK
- [ ] **Palette:** Class accent rengi (Class Energy tablosu)
- [ ] **252px tam mı:** Aseprite extend ile 251/253'ü düzelt
- [ ] **Frame count:** Idle=8, Walk=6, LMB/RMB=9, Dash=4, Hurt=3, Death=6
- [ ] **Cardinal flip:** Simetrik W = E flipX mi? Asimetrik W ayrı üretildi mi?
- [ ] **Hero anchor uyum:** Eyes facing forward, no extreme overhead

---

## Output Folder Layout

```
outputs/
├── 01_base_4dir/         # Adım 1
├── 02_idle_hit_death/    # Adım 2 (idle + hurt + death)
├── 03_run_cycle/         # Adım 3 (walk)
├── 04_attack_LMB/        # Adım 4
├── 05_attack_RMB/        # Adım 4
├── 06_dash/              # Adım 4
└── 07_weapon_pass/       # Adım 5 (Elementalist için boş)
```

## Tahmini Maliyet (Tier 2 Aktif)

| İçerik | Call | Kredit |
|---|---|---|
| Base 4-yön | 4 | ~30-50 |
| Idle/Hurt/Death (4 yön) | ~24 | ~150-200 |
| Walk cycle (4 yön) | ~12 | ~80-120 |
| 3 action × 3-segment × 4 yön | ~36-48 | ~300-450 |
| Weapon pass (4 yön + propagate) | ~6-8 | ~80-120 |
| **Toplam / class** | **~80-100** | **~640-940** |

> **Simetrik class (Warblade) ~%25 daha az** (3 yön + W flip).
