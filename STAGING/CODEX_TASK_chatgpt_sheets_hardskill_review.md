# Codex Task — ChatGPT v6 Sheet Quality + Hard-Skill Analysis Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```

---

## Görev

Üçüncü göz review — iki belge + 10 sheet üzerinde quality + completeness validation. v2.3 LOCK live, bu task non-blocker review.

## Read These Files

### v2.3 LOCK (latest progression spec)
- `STAGING/_plans/progression/PROGRESSION_PLAN_v2_3_LOCK.md`

### Sheet review target
- `STAGING/CHATGPTSPRITESHEETS/` — 10 PNG (ChatGPT BATCH v6 output)
  - `ChatGPT Image 21 May 2026 13_41_31 (1).png` → Warblade 14 skill
  - `(2).png` → Ronin 4 skill
  - `(3).png` → Shadowblade 22 skill
  - `(4).png` → Ranger 20 skill
  - `(5).png` → Summoner 8 skill
  - `(6).png` → Gunslinger 8 skill
  - `(7).png` → Ravager 8 skill (user noted: single axe — should be dual)
  - `(8).png` → Hexer 8 skill
  - `(9).png` → Brawler 8 skill
  - `(10).png` → Elementalist 15 skill

### Hard-skill analysis (orchestrator wrote)
- `STAGING/_research/HARD_SKILL_PIXELLAB_FEASIBILITY_v2.md`

### Canonical references
- `Assets/Art/Characters/{Class}/Rotations/{class}_south.png` — 10 canonical sprite
- `STAGING/_chatgpt/sprites_south/` — same files renamed for upload
- `STAGING/concepts/skill_sheets_v6/canonical_sprite_bible.md`

---

## Review Section 1 — Sheet Quality Audit

Her sheet için (10 toplam) verdict yaz:

| Sheet | Class | Skill Count Expected | Skill Count Visible | Sprite Faithful (Y/N) | Painterly Quality (1-5) | Issues |
|---|---|---|---|---|---|---|

Spesifik kontroller:
- **Skill count match:** Beklenen sayı vs sheet'te görünen panel sayısı
- **Sprite faithful:** Canonical south.png ile karşılaştır (anatomi, kıyafet, saç, ten, silah)
- **Class signature weapon:** Doğru weapon mu? (Ravager dual axe değil tek balta — already flagged)
- **Mob variety:** Aynı mob 2+ kez kullanılmış mı (panel başına farklı mob ideal)
- **VFX painterly:** Geometric primitive / programmatic look var mı (v5 problem)
- **Caption legibility:** Skill name + description okunabilir mi
- **30-35° iso perspective:** Tutarlı mı

## Review Section 2 — Hard-Skill Analysis Validation

`HARD_SKILL_PIXELLAB_FEASIBILITY_v2.md` doc'ta yazılan 4 kategori (EASY/MEDIUM/MEDIUM-HARD/HARD) + 6 industry pattern verdict'i:

| Section | Verdict | Notes |
|---|---|---|
| Category A EASY (~75 skill) | AGREE/DISAGREE | Eksik veya yanlış kategorize edilmiş skill var mı? |
| Category B MEDIUM (~25 skill) | AGREE/DISAGREE | Same |
| Category C MEDIUM-HARD (5 skill) | AGREE/DISAGREE | Chain/tether skills doğru LineRenderer pattern'e atanmış mı? |
| Category D HARD (3 skill) | AGREE/DISAGREE | Sequential spawn vs persistent line ayrımı doğru mu? |
| 6 Industry Patterns (#1-#6) | AGREE/DISAGREE | Hades/PoE/Diablo/Hollow Knight/RoR2 references accurate mı? Ek pattern var mı? |
| Production budget (6 gens + 12-18 saat Unity) | AGREE/DISAGREE | Realistic mı? |

## Review Section 3 — v2.3 LOCK Consistency Check

`PROGRESSION_PLAN_v2_3_LOCK.md`'de açık bıraktığı 3 open item:
- **O1:** B01/B02 mutual exclusivity (14+2=16 vs 14+1=15 nodes)
- **O2:** Architect ending meta-unlock content (Phase 2+)
- **O3:** Hub Phase 2+ catalog

Bunlar Phase 1 blocking mi? Veya Phase 1'de kararlanması zorunlu olan başka hidden conflict var mı?

Ayrıca:
- Topology revision (16 nodes vs Karar #62 LOCK 15 nodes) tutarlı mı?
- Bug fix integration tam mı (Hybrid Rest 1 Rest/Act, Boss auto-unlock Depth 12, MapFragment MacroRoomController guard)?
- 7 Codex Conflict + 3 Antigravity Bug baked-in tam mı?

## Output

`STAGING/_research/CODEX_REVIEW_chatgpt_sheets_v2.md`:

```markdown
# Codex Review — ChatGPT v6 Sheets + Hard-Skill Analysis

## Section 1 — Sheet Quality Audit
[10 sheet table + per-sheet notes]

## Section 2 — Hard-Skill Analysis Validation
[Per-category + per-pattern verdict + missing items]

## Section 3 — v2.3 LOCK Consistency Check
[O1/O2/O3 review + hidden conflict scan + integration completeness]

## Section 4 — Recommendations
[Per-class regenerate priority (Ravager dual-axe vs others)]
[Industry pattern code library priority]
[v2.3 LOCK open item resolution suggestions]

## Section 5 — Open Questions (orchestrator/user)
```

## Kısıt

- v2.3 LOCK'u re-write etme — sadece review, patch list önerisi
- Industry pattern doğrulaması: NLM veya web search ile cross-check
- ChatGPT sheet'lerine yeniden gen önerme — sadece quality verdict + rev önerisi
- Production budget gerçekçi mi: PixelLab + Unity-side dengeli mi check

## Effort
high
