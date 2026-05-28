# Karar #122 Addons (T2/T3/T4) — Final Design Synthesis

**Source:** rima-design (Opus) S69 sonu dispatch, 2026-05-14
**Status:** Tasarım sentezi tamamlandı; Codex review BEKLEMEDE (`STAGING/karar_122_addons_codex_review.md`)

## T2 — Resonance Hit Spec

**Altar Pasif Sayısı:** 10 universal Resonance pasifi. Cross-class bağımsız — hangi secondary class seçtiysen onun echo skill listesi tetiklenir.

**Scaling (15% → 25%):** Lineer +%2 per Altar pasifi.
- Base (0 Altar): %15
- 1 Altar: %17
- 2 Altar: %19
- 3 Altar: %21
- 4 Altar: %23
- 5 Altar (max): %25
- Karar #7 Altar = run-bound, ölüm sonrası reset.

**ICD Çakışma Kuralı (T1 vs T2):**
- T1 = 1.2s ICD (Beat 3 only, deterministic 100%)
- T2 = 0.8s ICD (her LMB vuruş roll, 15-25%)
- Beat 3'te ikisi de düşerse → **T1 ön** (yüksek dmg %35), T2 suppress, T2 ICD yine başlar (anti-cheese).

**Family Tag Mapping (FIXED per Altar, random değil — build identity):**

| Altar # | İsim | Family Tag |
|---|---|---|
| 1 | Whisper of Embers | Burn (Bleed family) |
| 2 | Frost Pact | Chill (Bleed slow variant) |
| 3 | Rift Hunger | Rift (T4 core tag) |
| 4 | Echo Cascade | Echo |
| 5 | Shatter Vein | Fracture (armor pen pre-state) |
| 6-10 | "Builds on" varyasyonlar | Duration/extend variants — Faz 2 detay |

**Edge Case — Secondary Class Yok:** Altar pasifleri muted (UI grey out), Yol A weapon Altar'ları normal çalışır.

## T3 — Empowered Skill Spec

**Evolution Matrix:** 10 class × 4 slot (Q=mobility/E=cc/R=ult/F=signature) × 2 path (A=aggressive/B=defensive/utility) = **80 evrim noktası**, run'da 4 seçilir.

**Warblade Sample Matrix:**

| Slot | Skill | Path A (Aggressive) | Path B (Defensive) |
|---|---|---|---|
| Q | Iron Combo Slam | dmg +30%, knockback ekle | short CD, AoE shrink |
| E | Sunder Mark | armor reduce +50% | AoE +30%, flat dmg |
| R | Earthsplitter (ult) | dmg double, longer CD | 2 cast, lower dmg/each |
| F | Gravity Cleave (signature) | pull enemies | empower next LMB |

**Echo Skill Auto-Bond (algorithmic, NOT hand-curated):**

Path A → Offensive echo, Path B → Utility/CC echo. Secondary class belirler hangi spesifik skill picked. Algorithm: her echo skill 2 family tag taşır (Melee/Ranged/Zone/Buff × Aggressive/Utility) → empowered cast'inde algorithm en uygun echo'yu picks.

**Player Tweak:** Skill Evolution dialog (Hades-style boon) → 3 echo opsiyonu önerilir, player picks.

**Sample bondings:**

| Primary Slot Path | Secondary | Echo Selected |
|---|---|---|
| Q-A (aggressive) | Elementalist | Fireball (R) — burst |
| Q-A | Shadowblade | Veil Strike (M) — close gap |
| Q-A | Ranger | Pinning Shot (R) — anti-mob |
| E-B (CC) | Hexer | Pandemic (Z) — debuff zone |
| R-A (ult burst) | Gunslinger | Deadshot (R) — single target hit |
| F-A | Summoner | Corpse Explosion (Z) — chain |

**Matematik:** 10 × 4 × 2 × 9 = 720 kombinasyon. Curated YASAK, algorithmic + player choice = scalable.

**Cooldown/VFX:**
- T3 Echo dmg = %50 primary skill dmg
- Cyan VFX trail primary weapon'da 0.5s
- Primary skill CD -%10 passive bonus

**Yol A Weapon Swap T3'te:** EVET kullanılacak. T3 trigger anında primary weapon sprite swap (Karar #99 silah görünürlüğü canon). 10 class × 2 weapon variant = 20 gen + 8h kod. **MVP'de YOK, Faz 2'de açılır.**

## T4 — Rift Proc Bond Spec

**4 Family Tag → Class Mapping:**

| Tag | Apply Mechanism | Primary Class Sources |
|---|---|---|
| **Rift** | Echo proc residual (T1/T2/T3) | Universal (all classes) |
| **Echo** | T2/T3 echo skill cast | Elementalist, Shadowblade, Summoner |
| **Fracture** | Melee crit, armor pen skill | Warblade, Ravager, Brawler |
| **Bleed** | Bleed family skill (Bloodlust, Wild Hack, Hex Bolt DoT) | Ravager, Hexer, Ronin, Shadowblade |

**Tag Süre/Refresh:**
- 2 sn (canon)
- **Refresh:** Aynı tag tekrar uygulanırsa süre full refresh, stack DEĞİL (1 tag = 1 flag)
- 3-stack için **3 farklı tag türü** lazım

**3-Tag Stack Detection UI:**
- HUD: Karakter portrait altında 4 mini icon slot (Rift / Echo / Fracture / Bleed)
- Tag aktifken icon parlak, süre dolduğunda fade
- 3 tag aktif → portrait outline flash cyan + screen pulse → T4 ready
- Player vuruş yaptığı an T4 burst → 3 tag temizlenir

**Animation/Timing (Rift Proc):**
- Primary skill cast frame'inde T4 detection
- Primary skill animation 0.3s donar (impact frame slow-motion, Hades benzeri)
- Echo skill simultaneously cast (sprite layer üst), cyan VFX birleşik
- Dmg = %100 primary + %100 echo + %50 armor pen
- T4 sonrası 4 sn tüm tag immunity (anti-cheese)

**Boss Execute Exemption:**
- Boss için execute mantığı **YOK**, dmg burst sabit %100+%100+%50 armor pen
- UI: boss üstünde "Rift Proc Resistant" yazı görünür

## Faz 2 Implementation Order

1. **T2 minimal (MVP olabilir)** — 1 Altar (Echo Cascade), ~6h
2. **T4 Family Tag system** — 4 tag class, event-driven Apply/Refresh/Detect, ~40h
3. **T2 full** — 10 Altar pasifi, scaling tablosu, ~24h
4. **T4 UI feedback** — Portrait outline + screen pulse, ~12h
5. **T3 skill evolution data** — 80 evrim noktası ScriptableObject, ~20h
6. **T3 Hades-style dialog UI** — 3-opt choice modal, ~16h
7. **T3 echo skill auto-bond algorithm** — family-match logic, ~12h
8. **T3 weapon sprite swap (Yol A bridge)** — +20-30h, 20 weapon gen

**Toplam Faz 2 scope:** ~160h kod + ~30 PixelLab gen + 6 UI mockup

## Dependencies

- **T4 önce** (T2/T3 echo proc'ları T4 tag'lerini besler)
- **T2 full T3'ten önce** (Altar canon Karar #7)
- T3 weapon swap T1/T2/T4 tamamlandıktan sonra (Yol A bridge)

## Open Questions / Risks

1. **Family Tag bloat:** 4 tag yeterli mi yeni class eklenirse? Karar gerekli.
2. **T4 boss execute scaling:** Sabit mi yoksa HP scale ile mi? Playtest sonrası.
3. **T3 boon dialog frekansı:** Her cast'te mi yoksa floor başı mı? Karar gerekli.
4. **T2 Altar #6-10 detay tasarım:** Placeholder, Faz 2 gerekir.
5. **Yol A weapon swap T3 timing:** Cast başında mı proc anında mı sprite değişir?

## Memory Commit Recommendation

**Yeni MEMORY/ files:**
- `MEMORY/project_family_tag_system.md` (T4 için 4 family canon)
- `MEMORY/feedback_t3_skill_evolution_matrix.md` (80 evrim + auto-bond algoritma)

**MASTER_KARAR_BELGESI.md güncelle:**
- Karar #122 detayı revize: T2/T3/T4 inline spec
- Karar #7 cross-reference: T2 Altar pool genişler (10 Resonance pasifi)
- Karar #5 cross-reference: T3 80 evrim noktası canon
- **YENİ Karar #123 önerisi:** "Family Tag System (Rift/Echo/Fracture/Bleed)" — T4 için ayrı locked decision

## Codex Review — Bekleyen Sorular

`STAGING/karar_122_addons_codex_review.md` dispatch edilecek. Codex'in cevaplaması gereken 6 ana soru:
1. Production cost validation (T2 ~30h, T3 ~80h + 20-30 gen, T4 ~40h doğru mu?)
2. Balance complexity (720 kombo algorithmic family-match sürdürülebilir mi?)
3. Family Tag system event-driven vs state machine
4. T3 UI yaklaşımı (her cast vs floor başı boon dialog)
5. MVP'ye T2 minimal sığar mı (1 Altar, %20, 0.8s ICD)
6. T3 weapon sprite swap Yol A integration (+20-30h scope)

## Conflicts with Locked Rules

NONE. Karar #5/#7/#99/#122 ile uyumlu, sadece detay tamamlıyor.

## Lock Status

**Tasarım locked, implementation MVP+Faz 2 ayrımı net.** T1 baseline MVP'de zaten implement edilecek; T2 minimal kıvama göre MVP'ye eklenebilir (~6h ek), T3/T4 Faz 2.
