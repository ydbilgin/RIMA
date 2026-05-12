# BIG_DESIGN_DECISIONS_2026-05-13.md
**STATUS: LOCKED 2026-05-14 — Kullanıcı onayladı (S60 Session 3 sonu)**

## Süreç Özeti
- rima-design (Sonnet) 4 soru için judgment verdi
- Codex (GPT-5.5 xhigh) bağımsız review yaptı: 3x MODIFY + 1x AGREE
- Opus orchestrator önerileri eklendi (Faz 1.0/1.5 ayrımı + vista template öncelik + sayısal trigger eşik + EN-first + PixelLab batch ekonomi düzeltmesi + accessibility 3-kanal genelleme)
- **7 karar LOCKED 2026-05-14:** #85 Open-World Backdrop Language, #86 Map Object Set (Faz 1.0/1.5), #87 Skill Effect AngleMode (5 kategori), #88 4 Yön + Sayısal Trigger, #89 EN-First Canonical, #90 PixelLab Batch Economy, #91 Accessibility 3-Kanal Standard
- Karar #72/#81/#82/#83/#84 ile çatışma YOK (Codex teyit)

## ⚡ KRİTİK GÜNCELLEME — PixelLab Batch Economy (Karar #90)
**Maliyet tahminleri ciddi şekilde düşer.** PixelLab Create from Style Reference tool tek generation'da **N cell tilesheet** veriyor:
- 32x32 sprite → 64 cell (8×8 veya 16×4) tek seferde
- 64x64 sprite → 16 cell (4×4) tek seferde
- 128x128 sprite → 4 cell tek seferde

**Esneklik:** 1 ürün × N variant veya N ürün × 1 cell veya karışım.

**Etki:**
- **Karar #86 (Map obj):** 14 obje 32px batch = 1 generation + ~2-4 saat cleanup ≈ **4-6 saat total** (eski 12-18 saat tahmini batch ekonomi ile düşer)
- **Karar #87 (Skill effect):** Directional8 (8 sprite/effect) × 4 hero = 32 sprite = TEK 32px batch
- **Projectile/fireball/ok:** Aynı batch ekonomi geçerli
- **Pipeline disiplini:** önce 1 sprite pilot test → batch'e geç. Cleanup hâlâ 5-15 dk/sprite (manuel iş).

## Faz 1.0 (Playable Demo) vs Faz 1.5 (Polish) Ayrımı

### Faz 1.0 Zorunlu (playable olmadan çıkamaz)
- 4 sınıf × 3 core effect (LMB + RMB + V Burst) = **12 skill effect**
- 3 T2 mob + Penitent Sovereign 3-phase boss
- 6 required gameplay map obj (chest/barrel/lever/shrine/spike/rift)
- 15 oda procedural
- **3 vista room template** (cliff edge / balcony / rift opening) — Karar #85 zorunlu
- F1 tile set + 3-layer parallax F1 kit + boss arena backdrop

### Faz 1.5 Polish (sonradan)
- 4 sınıf × 4 ek effect (Q/E/R/F) = **16 skill effect**
- Portal + shop counter
- 6 decor obje (sütun/moloz/bayrak/sunak/meşale/kafatası)
- Multi-tile landmark decor (non-colliding)
- 8 yön upgrade (eğer Karar #88 trigger tetiklenirse)
- Skill effect variant + barrel/rubble 2 variant

---

## Karar Adayi #85 -- Acik Dunya vs Kapali Dungeon
**SONUC:** Hades-vari arena chain + Open Vista "open-world backdrop language" (Codex modifier).

### Detay
- rima-design secim C (Hades-vari arena chain + Open Vista parallax) korunur
- Codex modifier: isimlendirme "perceived openness" degil "open-world backdrop language"
- Codex modifier: her biome/room setinde 2-3 vista room template zorunlu + transition reveal

### Faz 1 Implementation (Codex final)
- Open Vista parallax kit (3-layer) + boss backdrop
- Her biome/room setinde 2-3 vista room template + transition reveal (Codex onerisi)
- Normal odalar kapali kalabilir
- 15 node procedural -- sifir refactor
- Open Vista en iyi: odanin en az bir kenarinda kapalilik kirildiginda calisir (kirik duvar, balkon, cliff edge, rift vista, uzak yapi silueti)

### Faz 2-3
- Karar #81 pocket sub-room acilir (20x40, max 2/run)
- Act 2-3 "wider arena" room template (40x40 floor)
- **YASAK:** Free roam zone-level

### Risks Tracked
- Design risk: "acik dunya hissi" beklentisi kontrol altinda -- naming "open-world backdrop language"
- Production risk: parallax combat clarity bozmassin (gameplay tile okunurlugu oncelik)

---

## Karar Adayi #86 -- Map Object Set + Pipeline
**SONUC:** Faz 1 = 14 obje hedef, AMA **8 required gameplay + 6 nice-to-have decor** (Codex modifier).

### Required Gameplay (8, release blocker)
- chest 32x32 (3 state) -- dinamik
- breakable barrel 32x32 -- dinamik
- lever 32x32 (2 state) -- dinamik
- shrine 32x64 -- interactive
- portal 32x64 -- interactive
- shop counter 32x32 -- interactive
- spike-trap 32x32 -- hazard
- terrain rift 32x32 -- hazard

### Nice-to-Have Decor (6, paralel track, yetismezse Faz 2)
- pillar 32x64, rubble 32x32, banner 32x64, altar 32x32, torch 32x64, skull-pile 32x32

### Boyut Kurali
- 32x32 base, 32x64 (1x2 tall) allowed
- **Multi-tile gameplay collision YASAK Faz 1**
- Faz 2-3: 2x2 veya 2x3 landmark decor optional (basit rectangle veya non-colliding)

### Pipeline
- PixelLab Create Object (raw) -> Aseprite manual cleanup -> ScriptableObject -> Room Designer Object Brush
- **Sure tahmini (Codex):** 14 obje x **12-18 saat** (authoring + collision + sorting + palette + placement dahil; rima-design 10 saat iyimser kabul)
- 1 variant/obje Faz 1, Faz 2'de barrel + rubble 2 variant

### Risks Tracked
- Uretim yuku 12-18 saat
- Required 8 release blocker, decor degil

---

## Karar Adayi #87 -- Skill Effect AngleMode
**SONUC:** **5 kategori AngleMode hybrid policy** (Codex modifier).

### AngleMode Spec
| Mode | Kullanim | Asset | Yon |
|---|---|---|---|
| `ProjectileRotated` | symmetric glow/bolt | 1 sprite + Transform.rotation | continuous |
| `ProjectileDirectional8` | asymmetric arrow/knife/spear | 8 sprite | 8-snap |
| `BeamRotated` | lazer/isin | 1 tile + stretch + rotation | continuous |
| `Radial` | explosion/frost nova/healing circle | 1 sprite | yonsuz |
| `Cone` (Rotated VEYA Directional8) | flamethrower/sword sweep | skill bazli secim | hybrid |

### Faz 1 LIMIT (Codex risk control)
- **Directional8 (cone + projectile asymmetric) toplam max 2-3 hero effect**
- Diger cone'lar rotated mesh/particle ile prototiplensin
- Projectile default continuous angle; sprite artifact varsa 8-snap fallback

### Budget Tahmin
- 4 sinif x 5-7 effect = 20-28 effect total
- Cogu Rotation/Radial/Beam (cheap)
- 2-3 hero Directional8 (expensive, ~16-24 sprite total)

### Risks Tracked
- Pixel-art shimmer / jagged rotation (ozellikle keskin outline projectile)
- Asimetrik sprite engine rotation'da kotu gorunebilir
- Faz 1 scope creep (mob + class + skill effect paralel)

---

## Karar Adayi #88 -- 4 Yon vs 8 Yon
**SONUC:** **4 yon + flipX LOCKED korunur** (S59 onayi). 8 yon Faz 2-3 staged (Codex AGREE).

### Mevcut Durum
- S59 LOCKED 2026-05-12 -- 4 yon + flipX MVP
- V1 4 sinif production'a yeni baslanıyor

### Naming Duzeltme (Codex risk flag)
- "Hades match" ifadesi **"Hades-like responsiveness"** olarak guncellensin
- Birebir visual fluidity hedef degil -- timing/root motion/dust VFX/input responsiveness oncelik

### Faz 2 Trigger (Codex netlestirme)
- V1 4 sinif playable + 2-3 saat playtest sonrasi
- **Diagonal hareket sikayeti + combat readability sikayeti BIRLIKTE varsa** 8 yon protagonist upgrade acilsin (Codex onerisi)
- Tek basina diagonal sikayet yeterli degil -- readability ile birlesmelidir

### Staged Plan
- Faz 1: 4 yon LOCKED (sifir degisiklik)
- Faz 2: Protagonist 8 yon, mob T1/T2 4 yon, boss T3 8 yon
- Faz 3: V1 sinif + boss 8 yon, common mob 4 yon (visual hierarchy)

### Risks Tracked
- 8 yon upgrade = class basina 2x sprite cleanup
- "Hades match" beklenti management

---

## Cross-References
- Karar #72 silahli 1-piece
- Karar #81 Open Vista 3-layer parallax
- Karar #82 Mob 3-Tier Skill System
- Karar #83 Pocket Guardrails
- Karar #84 Mob T2/T3 Staged Budget
- S59 4 yon LOCKED 2026-05-12
- Bagli doc: CLASS_SILHOUETTE_BIBLE.md, T2_MOB_PROTOTYPE_SPEC.md, BOSS_PHASE2_RIFT_TEAR_SPEC.md, TONE_SURFACES_STANDARD.md

---

## Faz 1 Scope Realism Check (Codex risk)
| Is | Hours Estimate |
|---|---|
| 3 T2 mob prototype | 30-40h (sprite + behavior + tuning) |
| Boss Faz 2 hazard merge | 20h (Rift Tear + Rift Bloom + telegraph + accessibility) |
| 14 map obj (8 required + 6 decor) | 12-18h |
| 20-28 skill effect (2-3 Directional8 limit) | 30-40h |
| 4 sinif 4-yon anim (6-8 anim/sinif) | 80-100h |
| **TOTAL** | **172-218h ~= 4-5 hafta full-time** |

**Codex onerisi:** Static decor 6 release blocker DEGIL, Multi-tile Faz 2-3, 8 yon Faz 2-3 -> korunur. Diger kalemler Faz 1 keep.

---

## Sabah Confirm Checklist (kullaniciya)
- [ ] #85: "open-world backdrop language" naming + vista room template kurali kabul mu?
- [ ] #86: 8 required + 6 nice-to-have ayrimi tamam mi? Multi-tile Faz 2-3 optional kabul mu?
- [ ] #87: 5 AngleMode kategori + Faz 1 Directional8 limit 2-3 kabul mu?
- [ ] #88: "Hades-like responsiveness" naming + Faz 2 trigger (diagonal + readability) kabul mu?
- [ ] Confirm sonrasi: #85-88 LOCKED status -> MASTER_KARAR_BELGESI'ne eklensin

---

## Implementation Sira Onerisi (Faz 1)
1. Karar #88 LOCKED -> mevcut 4 yon production devam (zaten yapiliyor)
2. Karar #86 8 required gameplay obje -> PixelLab Create Object + Aseprite (paralel track)
3. Karar #87 5 AngleMode SO + Faz 1 limit 2-3 Directional8 -> SkillEffect ScriptableObject author
4. Karar #85 vista room template (2-3 per biome) -> Room Designer F3 ile birlesir
