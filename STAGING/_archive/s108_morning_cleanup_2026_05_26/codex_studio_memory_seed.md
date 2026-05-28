# Codex Task: LaurethStudio MEMORY/*.md seed dosyaları üret

**Model:** gpt-5.5, effort=high
**Profile:** laurethgame
**Süre tahmini:** 30-60 dk
**Background dispatch zorunlu**

---

## Bağlam

LaurethStudio infrastructure az önce kuruldu (`F:/LaurethStudio/`). MEMORY/INDEX.md hazır ama **gerçek memory dosyaları henüz yok**. INDEX'te referans verilmiş 8 memory dosyası create edilecek.

Mevcut Studio docs (referans için):
- `F:/LaurethStudio/CLAUDE.md`
- `F:/LaurethStudio/.claude/PROJECT_RULES.md`
- `F:/LaurethStudio/CURRENT_STATUS.md`
- `F:/LaurethStudio/AGENTS.md`
- `F:/LaurethStudio/STRUCTURE.md`
- `F:/LaurethStudio/MEMORY/INDEX.md`
- `F:/LaurethStudio/01_PIPELINE/stack_accumulation_mechanics.md`
- `F:/LaurethStudio/01_PIPELINE/player_authored_input_pattern.md`
- `F:/LaurethStudio/01_PIPELINE/layered_environment_pipeline.md`

Source docs (bilgi çekmek için):
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/3games_synthesis_s84.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/hyperlightdrifter_analysis.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/cinderia_analysis.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/hammerwatch2_analysis.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/3games_lateral_ideation.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/nemrav1_reference_analysis.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/codex_review_3games_synthesis.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/codex_review_authored_input_extension.md`

---

## Görev — 8 Memory dosyası create

Her dosya formatı:

```markdown
---
name: <kebab-case-slug>
description: <one-line summary>
type: <pattern | pipeline | anti_pattern | research | brand | workflow | _unbound>
scope: studio  # or rima | cb | caterpillar
locked: <true/false>
karar: <STUDIO_KARAR_NNN>  # if applicable
---

# <Title>

## Why this exists
<1 paragraph context — neden Studio memory'de tutulur>

## Core principle
<1-3 sentence rule>

## Cross-game application
| Game | Status | Notes |
|---|---|---|
| RIMA | LIVE / TODO / N/A | ... |
| CircuitBreaker | LIVE / TODO / N/A | ... |
| Caterpillar | LIVE / TODO / N/A | ... |

## Anti-patterns
- ❌ ...

## Implementation refs
- File / commit / pattern doc references

## Cross-link
- [[other-memory]] notation
```

### 1. `MEMORY/signature_accent_color.md`
- type: brand
- scope: studio
- karar: STUDIO_KARAR_012 (aday)
- Source: HLD pink/magenta + cyan glow accent recognition pattern (`hyperlightdrifter_analysis.md:37-41`)
- Cross-game: RIMA cyan rift LIVE / CB orange spark TODO / Caterpillar mor butterfly TODO
- Anti-pattern: Ekvivalan hue palette change (signature accent diğer renklerle karışmamalı)
- Codex review note: "recognizer accent" reframe — tek hue zorunlu değil, multi-color OK ama RECOGNIZER stable

### 2. `MEMORY/day1_accessibility.md`
- type: workflow
- scope: studio
- karar: STUDIO_KARAR_013 (aday)
- Source: HLD launch backlash photosensitivity 546 helpful Steam yorumu (`hyperlightdrifter_analysis.md:206-220`)
- Required ship-day: photosensitivity master + per-effect sliders + colorblind LUT (3 mode) + min text size + audio cue visualizer + subtitle UI
- RIMA durum: `FeelToggleSettings` 4 bool LIVE — extend gerekli
- Anti-pattern: Post-launch accessibility eklemek (itibar kaybı)

### 3. `MEMORY/sequel_reuse_ceiling.md`
- type: workflow
- scope: studio
- karar: STUDIO_KARAR_014 (aday)
- Source: HW2 case study %85 asset reuse "more depth less impact" recent %71 Steam (`hammerwatch2_analysis.md`)
- Rule: Sequel max %50 visual asset reuse, %50 fresh
- Anti-pattern: "Asset reuse to ship faster" tuzağı

### 4. `MEMORY/anti_patterns_universal.md`
- type: anti_pattern
- scope: studio
- 9 anti-pattern liste (3games_synthesis_s84.md Section 3'ten):
  - Late-game visual clutter
  - Day-1 a11y eksiği
  - Sequel "more depth less impact"
  - Multiple progression dilution
  - Photosensitive flash spam
  - Co-op scaling sikayet
  - Small +5% invisible upgrades
  - Quest/timed objective without indicator
  - Combat feedback/sting missing
- Her madde için: source game + RIMA/CB/Caterpillar uygulama notu

### 5. `MEMORY/borrow_degil_twist.md`
- type: workflow
- scope: studio
- karar: STUDIO_KARAR_017 (aday)
- Source: 3games_lateral_ideation.md çıktısı
- Rule: Referanstan direkt kopya YASAK, en az bir lateral transformation (inversion / cross-genre transplant / resource transmutation / player-side flip) zorunlu
- 4 lateral transformation method:
  1. Inversion (en sık, 8 fikir)
  2. Resource transmutation (6 fikir)
  3. Cross-genre transplant
  4. Player-side flip
- Process: research → identify core kazanım → identify shortcoming → choose transformation method → adapt

### 6. `MEMORY/studio_game_list.md`
- type: workflow
- scope: studio
- Aktif: RIMA + CircuitBreaker + Caterpillar
- Backlog (lateral 6 tohum): Insomnia Run / Atelier of Failure / Wingspan Glyph Garden / Self-Haunting / Mute Witness / Stillness Pilgrim
- Her oyun için 1 satır: tür / signature accent / status / next milestone

### 7. `MEMORY/feedback_visibility_rule.md`
- type: pattern
- scope: studio
- Codex review #2 önerisi (b70csrhr6): "feedback visibility beats raw depth"
- HLD: readable combat feedback → success
- Cinderia: snappy controls praised, clutter risk
- HW2: low-impact skill / invisible town building → criticism
- Studio rule: Her major mechanic için **görünür feedback** zorunlu (sound sting + visual telegraph + UI confirm)
- RIMA durum: Beat3 commit + 3-Layer Feedback Hierarchy LIVE (Karar #65)

### 8. `MEMORY/cross_game_pattern_application_matrix.md`
- type: workflow
- scope: studio
- Tablo: Her STUDIO_KARAR (001-016) × her game (RIMA / CB / Caterpillar) = LIVE / TODO / N/A
- Source-of-truth: bu matrix Studio'nun "hangi pattern hangi oyunda?" sorusu için referans
- Update trigger: yeni STUDIO_KARAR LOCK veya game pattern uygulama

---

## Kısıtlar

- **Sadece Studio MEMORY scope** — RIMA/CB/Caterpillar game-specific memory dokunma
- **Frontmatter zorunlu** her dosyada
- **Cross-link `[[link]]`** notasyonu kullan, gerçek dosya isimleri ver
- **`MEMORY/INDEX.md`** dosyaları zaten listeleniyor — değiştirme, sadece yeni dosyaları yaz
- Türkçe ASCII Markdown
- Her dosya 80-200 satır arası

## Output

Her dosyayı `F:/LaurethStudio/MEMORY/<filename>.md` olarak yaz.

Sonra CODEX_DONE_laurethgame.md'ye summary ekle:
- Yazılan dosya sayısı (8 olmalı)
- Her dosyanın satır sayısı
- Hangi STUDIO_KARAR'a bağlı
- Eksik bilgi varsa not (kullanıcı sorması gereken)

## CODEX_DONE Protokol

`CODEX_DONE_laurethgame.md` güncelle (cx_dispatch.py protokol).
