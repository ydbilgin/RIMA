# TASK: RIMA Reward + Portal Flow — Design Judgment (Opus)

ACTIVE RULES: (1) think before deciding (2) min spec, no speculation (3) cite RIMA-specific locks (4) BLOCKED if conflicting decisions.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: RIMA için Reward + Portal Flow kararı verilirken **derin design judgment** — sınıf sistemi, görsel dil, run length, meta-progression, kalıp seçimleri ve bunların 2+ sistem üzerindeki etkileri ile bütünleşik karar.

Çıktı **inline** (dosya YOK), 800-1200 kelime.

## RIMA Context (kritik)
- Wall-less V1 Hades Elysium görsel dili LOCKED (`project_walless_v1_hades_elysium_lock`)
- 8 sınıf roster v2 (`project_canonical_character_roster_v2`)
- Top-down 3/4 + 64 PPU + 10-12 fps animation lock
- Yarık (rift) 3-scale visual language — oda türüne göre density (Combat LOW / Ritual MED / Boss HIGH / Treasure ZERO / Bridge MED-HIGH / Pre-boss RISING)
- OmerDev "indie fail" dersleri (`F:/LaurethStudio/05_RESEARCH/2026_05_25_omerdev_indie_fail_lessons.md`)
- Karpathy 4 — min spec, sade ilk

## 5 Pattern Hatırlatma
| # | Akış | Referans |
|---|---|---|
| A | Clear → 1 ödül → 1 portal | Generic ARPG |
| B | Clear → 1 ödül → 3 portal (oda türü) | Hades door choice |
| C | Clear → 3 boon → 1 portal | Hades boon room |
| D | Clear → 3 portal + reward preview combo | Slay the Spire path |
| E | Clear → 2 reward + 2 portal (tied) | Custom |

## Görev — 5 bölüm

### Bölüm 1: RIMA Spesifik Trade-off Matrisi
Her pattern'i RIMA'nın LIVE kararları ile çapraz:
- **Wall-less arena** — portal görsel olarak nerede belirir, hangi pattern'de daha az görsel kirlilik?
- **8 sınıf** — class-specific boon variety hangi pattern'e ölçeklenir?
- **Yarık density (per-room)** — reward varyasyonu yarık dengesini bozar mı?
- **Top-down 3/4 + close camera** — 3 portal yan yana ekrana sığar mı (B/D)?
- **V1 Hades Elysium estetiği** — door/boon ayrımı atmosfere uygun mu?

### Bölüm 2: Sınıf Sistemi ile Etkileşim
RIMA'nın 8 sınıfı + Warblade/Elementalist starter:
- Boon pool: shared mi (tek pool, sınıf-agnostik buff) yoksa class-specific (her sınıfa farklı)?
- "Synergy" mekanizması — Hades pattern (2 boon birleşince special effect) RIMA'ya değer katar mı yoksa scope creep?
- Per-class identity — pattern seçimi her sınıfa farklı oynanış katar mı, yoksa hepsi aynı flow mu?
- Cross-class build viability — Warblade'in Elementalist boon'unu alması mantıklı mı?

### Bölüm 3: Run Length + Meta-Progression Etkileşimi
- RIMA target run length kararı bu pattern'e bağlı mı? (Hades 30-45dk = ~15 boon, RIMA için kaç ödül noktası?)
- Meta-progression (run arası kalıcı ilerleme) varsa hangi pattern daha iyi entegre olur?
- Death penalty + retry loop — boon kaybı reward flow'a nasıl yansır?
- Replayability — hangi pattern uzun vadede daha sürdürülebilir variety verir?

### Bölüm 4: OmerDev Dersleri ile Uyum
OmerDev'in #4 maddesi (rastgele seçim = oyuncu suçluyor) hangi pattern'de en güçlü?
- "Build sorumluluğu" hissi — A/B/C/D/E sırasıyla hangi en yüksek?
- "İlk 15 dakika kuralı" — onboarding'de reward flow nasıl tanıtılır, hangi pattern en az kafa karıştırır?
- "Boş yürüme yok" — portal'lar arena'da nerede ve ne kadar mesafede belirir?
- Wall-less collision auto-test ile pattern uyumu (özel risk var mı)?

### Bölüm 5: Verdict — Design Judgment
- En önerilen pattern (1 adet, kesin)
- MVP scope (ilk hangi minimal pattern, sonra hangisine geçilir)
- Karşı argüman: hangi pattern reddedilmeli ve neden
- Cross-system impact: pattern hangi sistemleri ZORUNLU getirir (örn boon → BoonRegistry + PerClassPool, portal → RoomGraph + Pathing)
- Risk: pattern downstream hangi kararı bağlar (sonradan değiştirilemez mi olur)

## Hard constraints
- Inline only — dosya YAZMA
- 2+ sistem kesen judgment — sadece "X iyidir" değil "X RIMA'nın Y sistemine değer katar çünkü Z"
- LIVE locks ile çelişki varsa açıkça flag et
- Speculative chaining yasak
- "BLOCKED" demek serbest — net karar zorunlu

## Beklenen toplam uzunluk
800-1200 kelime. Karpathy 4 — max design signal.
