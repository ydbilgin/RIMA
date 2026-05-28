# Karar #122 Addons (T2/T3/T4) — Codex Production & Balance Review

## Bağlam
RIMA roguelite, Karar #122 Echo Resonance Multi-Tier 4 tier:
- T1 Commit-Beat (free baseline, MVP, 100%/1.2s/35% dmg)
- T2 Resonance Hit (Altar pasifi, 15-25%/0.8s/25%)
- T3 Empowered Skill (Skill Evolution draft, 100%/skill CD/50%)
- T4 Rift Proc Bond (3 family tags → 100%+100%+50% armor pen)

## Tasarım Özeti (rima-design synthesis)

**T2 Spec:**
- 10 universal Altar pasifi (Echo Cascade, Whisper of Embers, vs.)
- Lineer scaling +%2 per Altar pasifi (15% → 25%)
- T1/T2 çakışma: T1 öncelik, T2 ICD yine başlar
- Family tag fixed per Altar (Burn/Chill/Rift/Echo/Fracture)
- 1 Altar pasifi minimal MVP candidate (~6h kod, 0 gen)

**T3 Spec:**
- 10 primary × 4 slot × 2 path = 80 evrim noktası
- Run'da 4 path seçilir (her slot 1)
- Echo bond ALGORITHMIC: her echo skill 2 family tag, primary path + secondary class → algorithm best echo picks
- Player choice: Hades-style boon dialog 3 opsiyon
- T3 trigger → cyan VFX trail + skill CD -10%
- Yol A weapon sprite swap T3'te entegre olabilir (10 class × 2 variant = 20 gen + 8h)

**T4 Spec:**
- 4 Family Tag: Rift (universal residual), Echo (proc-heavy classes), Fracture (Warblade/Ravager/Brawler), Bleed (Ravager/Hexer/Ronin/Shadowblade)
- Tag süre 2sn, refresh değil stack
- 3 farklı tag → portrait outline flash + screen pulse → T4 ready
- Rift Proc: primary 0.3s freeze + echo simultaneously + 100%+100%+50% armor pen
- Boss execute YASAK (canon), burst dmg sabit

## Review Soruları (Codex Cevapla)

### 1. Production Cost Validation

- **T2 ~30h kod:** 10 Altar pasifi data + chance/ICD roll + family tag apply. Doğru mu, revize öner.
- **T3 ~80h kod + 20-30 PixelLab gen:** Evolution dialog UI (Hades-style) + auto-bond algorithm + weapon swap. Doğru mu?
- **T4 ~40h kod:** 4 Family Tag class + event-driven Apply/Refresh + 3-stack detection + UI HUD + Rift Proc burst animation. Doğru mu?

### 2. Balance Complexity (720 kombinasyon)

- 10 primary × 2 path/slot × 4 slot × 9 secondary class = 720 unique echo bondings
- Hand-curated imkansız → algorithmic family-match yaklaşımı sürdürülebilir mi?
- Algorithm: her echo skill 2 family tag (örn. "Melee/Aggressive"), primary path family tag karşılaştır, best match picks. Sağlam mı?
- T1 (%35) → T4 (%200 + armor pen) burst gap mantıklı mı? Oyuncu T4'e ulaşamazsa T1 yeterli mi (T2/T3 progression)?

### 3. Family Tag System Implementation

- Event-driven (`OnDamageDealt → tag.Apply(2s, type)`) recommended. Performance impact 50+ mob arenas?
- Alternative state machine (per enemy state bloat) maliyet?
- Tag refresh logic: aynı tag tekrar → süre reset (not stack) — kod karmaşıklığı?

### 4. T3 UI Yaklaşımı

- Hades-style boon dialog her empowered cast'te mi (flow break riski) yoksa floor başı yenileme mi?
- Player'a 3 echo opsiyonu (algorithmic match'ten) → UI mock-up önerin
- Dialog acılma trigger noktası: empowered skill button basıldığı an mı, skill cast başladığında mı?

### 5. MVP'ye T2 Minimal

- 1 Altar pasifi ("Echo Cascade", %20 fixed chance, 0.8s ICD) → demonstrable MVP'de mi?
- Faz 2'de T2 full 10 Altar açılma sırası önerin
- T1 + T2 minimal kombo gameplay'i yeterince zenginleştirir mi school demo için?

### 6. ECHO + Yol A Weapon Decouple Visual Integration

- T3'te weapon sprite swap (Karar #99 silah görünürlüğü canon) +20-30h scope ekler mi yoksa daha fazla mı?
- 10 class × 2 weapon variant = 20 PixelLab gen maliyet doğru mu (her variant ~1 gen)?
- Hangi sıra: MVP'de SKIP, Faz 2 başlangıçta entegre. Onayla veya alternatif öner.

## Output Format

Cevaplarını şu yapıda ver:

```
# T2/T3/T4 Production Review

## 1. Cost Validation
T2: [revize/onayla, gerekçe]
T3: [revize/onayla, gerekçe]  
T4: [revize/onayla, gerekçe]

## 2. Balance
- Algorithmic family-match verdict
- T1→T4 burst gap verdict
- Implementation risk

## 3. Family Tag Implementation
- Event-driven vs state machine recommendation
- Performance estimate (50+ mob)
- Refresh logic complexity

## 4. T3 UI
- Boon dialog frequency recommendation
- 3-opt UI mock-up suggestion (text descripts)
- Dialog trigger point

## 5. MVP T2 Minimal
- 1-Altar MVP demonstrable: YES/NO + gerekçe
- T2 full open order
- T1+T2 minimal demo richness verdict

## 6. T3 Weapon Sprite Swap (Yol A bridge)
- Scope estimate validation
- Gen cost validation (20 gen?)
- Recommended order (MVP/Faz 2/post-Faz 2)

## Implementation Risk Top 3
1. [risk]
2. [risk]
3. [risk]

## Final Recommendation
- MVP scope addition: [tier minimal var mı?]
- Faz 2 start order: [tier sırası]
- Tek cümle: bu sistem RIMA'da iş görür mü?
```

## Effort: high
