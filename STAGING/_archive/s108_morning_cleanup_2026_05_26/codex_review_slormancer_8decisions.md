# Codex Review Task — 8 Slormancer-Inspired Karar Adayı + Skill Genişleme Sorusu

## Görev özet
8 yeni karar adayı (3 farklı agent — Opus + Codex + Antigravity — sentezi) **Sprint planlama öncesi** PASS/FAIL/MODIFY review'i. **Kritik ek soru:** Bu kararlar (özellikle #147 Per-Skill Mastery + #148 Class Weapon Keystone) eklenince **yeni skill üretimi** gerekli mi, kaç tane, nasıl bağlanmalı?

## RIMA Mevcut Skill Ekonomisi (Karar referansları)

| Boyut | Mevcut |
|---|---|
| Sınıf sayısı | 10 (Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer) |
| Sınıf başına aktif skill | 1 LMB + 1 RMB + 1 F (Ultimate) + 4 equip slot + 1 Ghost Attack (Z/X) = **8 slot toplam** |
| Toplam skill havuzu | ~80 (sınıf başına ~8) |
| Skill taksonomi | 4 aktif (STRIKE / ZONE / REACTIVE / STATE) + 3 pasif (KEYSTONE / MODIFIER / RESONANCE) |
| Cross-class | Shadow Echo Matrix: 10×5 = 50 echo + 90 cross-class combo rating |
| Karar #144 | Silahsız body + WeaponSR child SR — **silah swap mekanik enabler** ✅ LIVE |
| Karar #145 | PixelLab Character States workflow — state-first asset üretim ✅ LIVE |
| Karar #146 candidate (BACKLOG) | Weapon Component Swap — 10 sınıf × 5 weapon × 3 component slot × 4 option = 600 combo runtime |

## 8 Karar Adayı (Detay)

### Karar #147 — Per-Skill Mastery Tree (Tier S)
- Kaynak: Codex + Opus
- Spec: Her aktif skill kill XP toplar; 3 tier unlock (T1 / T2 / T3); her tier 2-3 upgrade kartı arasından seçim
- Slormancer ref: Patch 0.9 — skills level alongside character, equipped skills get same mastery, T1/T2 upgrade picks at start
- Impact: 80 skill × 3 tier × 3 choice = **720 build varyasyonu** alt mantığı
- Sprint hedef: Sprint 16+ (combat integration sonrası)

### Karar #148 — Class Weapon Keystone (Tier S)
- Kaynak: Codex + Opus
- Spec: Her sınıf için 2-3 build-defining weapon (boss drop); her keystone skill davranışını **kökten** değiştirir
- Slormancer ref: Slorm Reapers (32 base + 51 evol + 102 Primordial); RIMA için 2-3/sınıf = 20-30 toplam
- Karar #144 / #146 ile birleşim: Boss drop weapon = 1 unique modifier + WeaponSR runtime swap
- Sprint hedef: Sprint 18+

### Karar #149 — Elite Affix Tooltip System (Tier A)
- Kaynak: Codex
- Spec: Elite mob'lara 1-2 affix etiketi (Aegis, Anti-Heal, Berserk, Frenzy, Reflective, Splitter); oda girişinde HUD tooltip
- Slormancer ref: 40+ monster affix, Steam page
- Etki: 16 mob × 6 affix = 96 varyasyon potansiyeli; oda taktik derinliği
- Sprint hedef: Sprint 15

### Karar #150 — 3-5 Kademeli Heat/Curse Scaling (Tier A)
- Kaynak: Codex
- Spec: Hades-tarzı endgame difficulty stack (Curse 1-5); her seviye handicap + ödül artışı
- Slormancer ref: Wrath 10 (genel) + Wrath 10+100 (infinite — RIMA için scope dışı)
- RIMA endgame: Rift Break Phase 4-5 mantığı ile uyumlu
- Sprint hedef: Phase 2

### Karar #151 — Loadout System (Tier S)
- Kaynak: Opus
- Spec: Run içinde 2-3 loadout preset save/swap (Tab hotkey); boss vs minion için preset switch
- Slormancer ref: Quality of life loadouts mentioned positively in reviews
- Etki: 4 equip slot × 3 loadout = 12 slot effective; build strategi
- Sprint hedef: HUD v2

### Karar #152 — Cursor-Based Active Camera (Tier S)
- Kaynak: Antigravity
- Spec: CameraFollow.cs içine player + cursor weighted center (%80 + %20); aim-mechanic sınıflar için
- Slormancer ref: Steam community + gameplay videos confirm cursor-pan camera
- Karar #100 (30-35° top-down) ile uyumlu — sadece XY offset
- Sprint hedef: Sprint 14

### Karar #153 — Dynamic UI Clutter Control (Tier A)
- Kaynak: Antigravity
- Spec: UIManager + RimaUITheme — threat points (5+ enemy + skill burst) damage number scale 0.7x + group merge
- Slormancer ref: UI filter system, loot text dynamic sizing
- Hangi durumlar: Summoner orduları, Hexer Blight burst, boss multi-phase
- Sprint hedef: Sprint 15

### Karar #154 — AoE Telegraph Decal Pass (Tier A)
- Kaynak: Antigravity
- Spec: Skill telegraph decal'leri Vivid Vulnerability **dışı** zıt neon palette
  - Hexer Blight Sigil: `#8B0000` (parlak kan kırmızısı)
  - Ranger Bone Trap: `#7BA7BC` (cold-blue)
- Slormancer ref: Genuine AoE telegraph readability (Steam reviews praise)
- RIMA palette uyumu: VISUAL_TONE_BIBLE.md + SKILL_VISUAL_CONTRACT.md cross-check gerekir
- Sprint hedef: Sprint 16+

---

## ⚠️ KRİTİK SORU — Skill Genişleme Gerekli mi?

**User'ın gözleme dayalı endişesi:** "#148 Class Weapon Keystone boss-drop silah ekleyince, eski 80 skill havuzu o yeni silahlar için **tasarlanmamış**. Yeni skill gerekli mi?"

### Olası 3 yol

**Path A — Weapon-Bound New Skills**
- Her keystone weapon **1 unique skill** açar (weapon equipped iken erişilebilir)
- 2-3 keystone × 10 sınıf = **20-30 yeni skill**
- Mevcut 80 skill korunur, ek 20-30 keystone-bound skill üretilir
- Pro: Net silah-skill bağı, build clarity
- Con: 30 yeni skill = sprite + VFX + balance = **büyük scope**

**Path B — Mastery Branch Modification**
- Yeni skill **YOK**. Keystone weapon mevcut skill'leri modify eder (Karar #147 Per-Skill Mastery üzerinden)
- Örn: Warblade "Bloodseeker Axe" → Iron Charge mastery tree'sine yeni T3 dal "HP trade" eklenir
- Pro: Skill üretim maliyeti yok; mastery sistemi zaten weapon-aware olur
- Con: Keystone weapon "kökten değiştirir" hissi azalır; daha çok stat boost gibi

**Path C — Class Skill Pool Expansion**
- Her sınıfa **2-3 yeni skill** eklenir (sınıf havuzu genişlemesi)
- 10 × 2-3 = **20-30 yeni skill**, ama weapon-bound DEĞİL — class pool'a sürekli erişilebilir
- Keystone weapon mevcut skill'leri synergistik modify eder
- Pro: Pool derinliği artar, draft variety zenginleşir
- Con: 80 → 100-110 skill, balance + üretim yükü; keystone weapon "iconic feel" azalır

### Codex'ten beklenen verdict
1. **Path A / B / C arası tercih + gerekçe** (RIMA roguelite scope + 10 sınıf × 4 equip slot dengesi)
2. Hangi keystone weapon-bound skill **gerçekten gerekli** (örn. Warblade için 2-3 keystone'dan kaçı yeni skill ister?)
3. **Sayısal hedef:** Eğer Path A/C tercih edilirse, ek skill sayısı (10-15 mi, 20-30 mu, 30+ mi?)
4. **Üretim sırası:** Skill spec → mastery tree → keystone weapon mi, yoksa weapon → bound skill → mastery mi?

---

## Codex'ten istenen review formatı

### Her karar için:
- ✅ **PASS** / ⚠️ **MODIFY** / ❌ **FAIL** verdict
- Gerekçe (1-3 cümle)
- RIMA Karar #74/#100/#143-E/#144/#145 uyum kontrolü
- Eğer MODIFY: spesifik fix önerisi
- Sprint hedef teyit / değişiklik önerisi
- Bağımlılık (hangi karar hangisinden önce yapılmalı?)

### Skill genişleme sorusuna direkt cevap:
- Path A / B / C tercih + neden
- Sayısal hedef
- 10 sınıf × keystone bazında **örnek mapping** (en az 2 sınıf detaylandır — örn. Warblade + Hexer)
- Üretim sıralaması

### Final tablo:
- 8 karar PASS/MODIFY/FAIL özeti
- Sprint sıralaması (P0/P1/P2)
- Skill genişleme verdict (kelime ve sayı)
- Toplam scope tahmini (sprint sayısı)

## Output
`STAGING/codex_review_slormancer_8decisions_DONE.md` — uzunluk 1500-2500 kelime. ASCII-only.
