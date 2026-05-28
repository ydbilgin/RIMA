# Codex Review Task — Player-Authored Input Extension (Beyond Spells)

**Model:** gpt-5.5, effort=high
**Çıktı:** review report → `CODEX_DONE_<profil>.md`
**Süre tahmini:** 30-45 dk
**Profile:** laurethgame veya yasinderyabilgin

---

## Bağlam

Studio doc `F:\LaurethStudio\01_PIPELINE\player_authored_input_pattern.md` STUDIO_KARAR_016 aday. Önce sadece spell drawing içeriyordu; **kullanıcı talebi:** "sadece büyü çizip yapmayacak mesela. bazı skillerde yere path de çizilebilir veya başka tür de bunu da iyice araştır yaratıcı fikri gelirse codexle reviewlatarak not al."

Doc Section 13-16 eklendi:
- Section 13 — 8 farklı drawn input tipi (ground path / movement / AoE shape / tactical command / puzzle / crafting / storytelling / rhythm)
- Section 14 — RIMA 10 class × drawn input type matrix
- Section 15 — Genişletilmiş anti-pattern listesi (6 ek madde)
- Section 16 — Studio Tier 1/2/3 risk hierarchy

---

## Review kapsamı

Aşağıdaki dosyaları tam oku:
1. `F:\LaurethStudio\01_PIPELINE\player_authored_input_pattern.md` (genişletilmiş, ~470 satır)
2. `STAGING/3games_synthesis_s84.md` (Karar #149-170 context)
3. `TASARIM/MASTER_KARAR_BELGESI.md` (özellikle Karar #21 Hexer, #54 Perfect Condition, #58 movement Option C, #67 dash-cancel, #71 silahsızlık)
4. `Assets/Scripts/Combat/` klasörü (mevcut combat sistem — InputHandler, CombatHandler, ProcLimiter)

---

## Review başlıkları (her madde için PASS/FAIL/CAUTION)

### A. 8 Drawn Input Tipi (Section 13)
- Her 8 tipin RIMA için fiziksel olarak uygulanabilir mi?
- Eksik bir type var mı? (örn: object stacking gesture, vehicle steering, social emote)
- 8 tip içinde RIMA combat tempo'sunu en az bozan hangi?
- Pattern overlap / duplicate var mı (örn: B Movement vs E Puzzle gesture)?

### B. RIMA Class Matrix (Section 14)
- Her class için "Best Fit" doğru atanmış mı?
- Karar #34 (5E/5K cinsiyet), #54 (Perfect Condition), #67 (caster cancel), #71 (Hexer silahsız) ile çelişen önerileri belirle
- Eksik: Cleric class henüz yok ama Section 13'te var — temizle veya gelecek backlog'a not düş
- Class identity ile uyumsuz öneriler (örn: Warblade'e "Charge path" — ama Warblade commit-heavy class)

### C. Anti-Pattern Listesi (Section 15)
- 6 yeni anti-pattern eksiksiz mi?
- "Drawing while moving" — ama BDO mouse movement esinlenmesi de var (Section 11). Çelişki mi?
- "Combat-critical drawing without escape valve" — fallback button design somut mu, tasarım eksik mi?

### D. Tier Hierarchy (Section 16)
- Tier 1/2/3 ayrımı net mi?
- RIMA Tier 2 max kuralı — tüm 8 input tipi Tier 2'ye sığar mı yoksa bazıları Tier 3'e mi?
- CB Tier 3 OK kuralı — implementation feasibility var mı?

### E. RIMA Combat Compatibility
- Karar #58 movement Option C (Space dash, no state) ile drawn movement (Section 13.B) çelişiyor mu?
- Karar #67 caster windup not cancellable + Hexer drawn sigil (time-slow during draw) — uyumlu mu?
- Beat3 commit windowing + drawn input — interaction problemi var mı?

### F. Cross-Game Consistency
- Section 11 cross-game application matrix — RIMA selective + CB central + Caterpillar limited + Rune Duelists core ayrımı feasible mi?
- Caterpillar "cocoon shape drawing → metamorphosis variation" — bu mekanik gerçekten Wingspan-tipi cozy oyuna oturur mu?

### G. Code Template (Section 5 mevcut)
- AuthoredInputController abstract API yeterince genişletilebilir mi?
- AuthoredArchetype enum eksiksiz mi (8 tip için 8 archetype gerek mi yoksa archetype recognition independent mi)?
- Section 13 yeni input tipleri için ek subclass'lar gerekli mi?

### H. Lateral Ideation Genişletme
- 8 tip içinde DAHA YARATICI bir 9. tip önerisi var mı?
- Cross-tipler (örn: ground path + rhythm = "yere ritimli şarkı çiz") feasible mi?
- Multiplayer (2 player aynı anda farklı path çiz, kombine sonuç) konsepti?

### I. Production Risk
- Asset üretim cost (8 tip için sprite + VFX + UI) makul mu?
- Animation bütçesi (her tip için draw → cast transition anim) Faz 2 scope mu?
- Audio cost (her tip için unique cast sound) gerekli mi?

### J. Missing / Skipped
- Studio doc'ta atlanmış ama önemli olan bir tasarım kuralı?
- RIMA'ya entegrasyon için eksik bir teknik spec?

---

## Çıktı formatı

`CODEX_DONE_<profil>.md` içinde:

```markdown
# Codex Review — Player-Authored Input Extension

## Verdict Summary
- Total findings: N
- BLOCKERS: M
- WARNINGS: K
- INFO: L

## Section A — 8 Drawn Input Types
- Type 1 (Ground Path): [verdict] reason
- Type 2 (Movement): ...
- ...

## Section B — RIMA Class Matrix
- Warblade: [verdict] reason + suggested edit
- ...

(her bölüm için aynı format)

## Critical Findings (TOP 5)
1. ...
2. ...

## Suggested Edits (file:line)
- ...

## Lateral Suggestions (Codex'in kendi yaratıcı ek fikirleri)
- 9. tip önerisi: ...
- Cross-type combo önerisi: ...

## Missing Analysis
- ...
```

---

## Kısıtlar

- **Sadece review + lateral suggestions, kod yazma YOK**
- **Section H** (Lateral Ideation Genişletme) — Codex'in kendi yaratıcı ek fikirlerini bekleriz, sadece mevcut review değil
- Karar numbering — yeni Studio Karar / RIMA Karar adayı önerirken #170+ kullan, mevcut #149-170 ile çakışma yok mu kontrol et
- Output max 600 satır

## CODEX_DONE Protokol

`CODEX_DONE_<profil>.md` güncelle (cx_dispatch.py protokol).
