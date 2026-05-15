# 3 Steam Oyun Research Sentezi — S84

**Date:** 2026-05-16
**Source agents:** Hyper Light Drifter (a90d2dab2d48eca78, 401 satır), Cinderia (a2634e6552f11c931, 326 satır), Hammerwatch II (ade0d25d44966ce27, 423 satır)
**Total raw:** 1150 satır, 3 Opus sub-agent paralel ~7-8 dk

---

## 1. Karar adayı numaralandırma (LOCK adayı = kullanıcı onayı bekliyor)

**Mevcut LOCK:** Karar #143-R'ya kadar. BanditKnightG #144-#148 öneri. **Yeni numaralar #149'dan başlar.**

### Tier 1 — Hemen LOCK adayı (4 karar, az risk + yüksek değer)

| # | Karar | Source | RIMA etki | Studio etki |
|---|---|---|---|---|
| **#149** | **Studio Signature Accent Color LOCK** — RIMA cyan rift / CB orange spark / Caterpillar mor butterfly. Her oyun 1 accent renk LOCK, palette değişse bile bu renk korunur (HLD pink/magenta pattern) | HLD | Cyan rift accent zaten LIVE — formal LOCK | Brand kuralı 3 oyun |
| **#150** | **HLD 4-Layer Visual DNA** — flat fill + gradient overlay + vignette + soft glow Karar #143 L4-L6 stack için template | HLD | L4 transition brush + L5 detail + L6 accent shader/material guide | Pixel art genre default |
| **#152** | **Day-1 Accessibility Slider Lock** — photosensitivity (shake/bloom/flash) + colorblind + audio cue toggles. Ship öncesi zorunlu, post-launch eklemek itibar kaybı (HLD case study) | HLD | RIMA `FeelToggleSettings` zaten Shake/Hitstop/Vignette/CameraPunch var → photosensitivity master toggle eklenmeli | Universal launch checklist |
| **#161** | **4-Layer Meta Progression Separation** — per-run only / per-run convert / persistent / account-level explicit ayrı, "+5% bonus" pattern REJECT. HW2 sikayeti "multiple systems dilute impact" | HW2 | RIMA Rift Break tasarımı için kritik — şu an karışık | Universal roguelite progression |

### Tier 2 — Detaylı tasarım gerekli, DEFER (8 karar)

| # | Karar | Source | Neden defer | Faz |
|---|---|---|---|---|
| #151 | Dash-into-Attack +20% damage — combat v4 enrich | HLD | Mevcut Dash v2 + Beat3 trigger ile birleştirilmeli, tasarım gerek | Combat v5 |
| #153 | Erosion/Curse meter — güçlü skill spam → permanent run curse | Cinderia | Cursemark karakter karşılığı — class system overhaul | Faz 2 |
| #154 | Visual Clarity Override flag — late-game L5/L6 density cap | Cinderia | Karar #143 atlas-level toggle, mevcut centerPathDensityReduction yeterli mi test | Faz 1 polish |
| #155 | Build-as-Identity combinatorics — class+weapon+blessing kombinasyonu run kimliği | Cinderia | Mevcut 80 cross-class skill + 10 class ile zaten var, sadece UI surfacing | Faz 2 |
| #156 | OST as Standalone DLC Revenue — Stable Audio Open pipeline Studio çapı | Cinderia | Faz 1 close sonrası audio LOCK olunca | Faz 3+ |
| #162 | Town Hub Tier-Gate Discovery — Guild Hall blueprint unlock | HW2 | Sanctum tasarımı henüz handcrafted, scope arttırır | Faz 4 (Sanctum-2) |
| #163 | Floor-Specific Material Drop — biome'a özel resource type | HW2 | Karar #75 prop role tags ile bağlı, refactor gerek | Faz 2 |
| #164 | Co-op 4-player drop-in design | HW2 | Solo-first lock korunmalı (HW2 multiplayer scaling sikayet hacmi) | Faz 4+ post-launch |

### Tier 3 — REJECT (RIMA LOCK'larıyla çatışıyor, kaydet ama uygulama yok)

| Karar | Source | Neden REJECT |
|---|---|---|
| Full silent narrative | HLD | RIMA 80 cross-class skill kart text gerektirir. Hibrit: pictographic dialog + text skill kart |
| 2.5D isometric perspective | Cinderia | Karar #100/#101 LOCK 128px native 30-35° 3/4 ARPG view |
| Non-pixel art style | Cinderia | RIMA chibi pixel + Studio LOCK |
| Default run pacing (Cinderia "slow") | Cinderia | Beat3 + dash v2 hızlı combat tempo |
| Inverse difficulty curve (HLD) | HLD | RIMA progression curve standart upward |
| Sequel %85 asset reuse | HW2 | RIMA Faz 1 asset pipeline yeni başlıyor, irrelevant |

---

## 2. LaurethStudio Universal Pattern Eklemeleri (3 madde)

**STUDIO_KARAR_012 (yeni aday):** Signature Accent Color LOCK kuralı
- Her oyun 1 accent renk LOCK'lar (RIMA #7BA7BC cyan / CB orange / Caterpillar mor)
- Palette diğer renkleri değişebilir, accent değişmez
- Logo, UI highlight, key VFX color tüm bu accent'i kullanır
- Source: HLD case study (Steam reviews accent recognition oran %78)

**STUDIO_KARAR_013 (yeni aday):** Day-1 Accessibility Slider Standard
- Photosensitivity (master toggle + per-effect: shake / bloom / flash / particles)
- Colorblind mode (Protanopia / Deuteranopia / Tritanopia LUT)
- Audio cue alternatives (visual hint for important sound events)
- Subtitle / dialog box options
- Min text size adjustable
- Source: HLD launch backlash 546 upvote'lu Steam yorumu → day-1 ship zorunluluğu

**STUDIO_KARAR_014 (yeni aday):** Sequel Reuse Ceiling — %50 visual asset reuse max
- HW2 case study: %85 asset reuse "more depth less impact" sikayeti üretti
- Sequel = 50% reused (engine, systems, tooling) + 50% fresh (art, biomes, mechanics)
- "Asset reuse to ship faster" tuzaktan koru
- Source: HW2 Steam reviews recent %71 (genel %84'ten düşüş)

---

## 3. Anti-Pattern Listesi (yapma listesi, Studio universal)

| Anti-pattern | Source | Aksiyon |
|---|---|---|
| Late-game visual clutter | Cinderia | L5/L6 density cap + readability override flag (Karar #154 adayı) |
| Day-1 accessibility eksiği | HLD | STUDIO_KARAR_013 zorunlu |
| Sequel "more depth less impact" | HW2 | STUDIO_KARAR_014 reuse ceiling |
| Multiple progression systems dilute impact | HW2 | Karar #161 4-layer separation |
| Photosensitive flash spam | HLD | Karar #152 toggle |
| Co-op scaling sikayetleri (solo→multi) | HW2 | Solo-first lock — Faz 1-3 solo benchmark |

---

## 4. RIMA spesifik aksiyon listesi (kod/asset değişim gerekenler)

### Hemen yapılabilir (S84 / S85 içinde)
1. **`FeelToggleSettings.cs`'a photosensitivity master toggle ekle** (Karar #152 #149 hazırlık) — `PhotosensitivityReducedEnabled` bool, ON ise Shake/Hitstop/Vignette intensity'leri yarıya iner (5 satır kod)
2. **`PatchAtlasSO`'a `readabilityOverride` bool ekle** (Karar #154 hazırlık) — true ise centerPathDensityReduction × 0.5 ek azaltma, late-game readability korur
3. **MEMORY/INDEX.md'ye 3 yeni entry ekle:** [hld-design-patterns](memory_hld_design_patterns.md), [cinderia-anti-patterns](memory_cinderia_anti_patterns.md), [hw2-progression-lessons](memory_hw2_progression_lessons.md)

### Faz 1 close öncesi
4. **Studio signature accent LOCK** dosyasını `F:\LaurethStudio\00_RULES\STUDIO_KARAR_012_signature_accent.md` olarak yaz (Karar #149)
5. **HLD 4-layer visual DNA** prototip — bir test L4 sprite'a flat fill + gradient mask + vignette + soft glow shader denemesi (Karar #150)

### Faz 2 başlangıcı
6. **Cursemark karakter sistemi tasarım** — Erosion/Curse meter mekaniği (Karar #153 detay)
7. **Rift Break 4-layer separation refactor** — meta progression açıklamasını netleştir (Karar #161)

---

## 5. Karar numbering reconciliation

Agent'lar #150 numarasını 3 ayrı maddeye atadı (conflict). Resolved:
- **HLD #149-152:** signature accent / 4-layer DNA / dash-attack / accessibility
- **Cinderia #153-160:** Erosion/Curse + 7 BORROW
- **HW2 #161-170:** Town hub + Rift Break separation

Toplam yeni karar aday sayısı: 4 Tier1 LOCK + 8 Tier2 DEFER + 6 Tier3 REJECT = 18 madde.

---

## 6. Kaynak dosyalar

- `STAGING/hyperlightdrifter_analysis.md` (401 satır)
- `STAGING/cinderia_analysis.md` (326 satır)
- `STAGING/hammerwatch2_analysis.md` (423 satır)
- Toplam research: 1150 satır
- Agent IDs: a90d2dab2d48eca78 / a2634e6552f11c931 / ade0d25d44966ce27

---

## 7. Sonraki adım

**Kullanıcı kararı bekleyen sorular:**

1. Tier 1 LOCK adayları (#149, #150, #152, #161) — hepsi LOCK olsun mu, yoksa tek tek tartışalım mı?
2. STUDIO_KARAR_012/013/014 LaurethStudio'ya direkt yazılsın mı?
3. Cursemark karakter (Cinderia Erosion/Curse) Faz 2 design dispatch hazır mı, defer mi?
4. Photosensitivity master toggle hemen yazılsın mı (5 satır, low risk)?
