# Codex Review Task — 3 Steam Oyun Sentezi + Hexer/Stack Pattern

**Model:** gpt-5.5, effort=high
**Çıktı:** review report → `CODEX_DONE_<profil>.md`
**Süre tahmini:** 30-60 dk
**Profile:** laurethgame veya yasinderyabilgin

---

## Bağlam

3 paralel Opus sub-agent araştırma yaptı (HLD + Cinderia + HW2). Sentez yapıldı, Karar adayları çıkarıldı, Studio universal pattern (Stack/Accumulation) doc'u yazıldı. **Bu task = Codex independent review** — gözden kaçanlar, mantık hataları, eksik analiz, contradiction.

**Kullanıcı talebi:** "bütün bunları codexe de review ettirdin mi? eksiklerini göreebilir o da"

---

## Review kapsamı

Aşağıdaki 5 dosyayı tam oku ve KAPSAMLI review yaz:

1. `STAGING/3games_synthesis_s84.md` (130 satır) — sentez + Tier 1/2/3 + aksiyon listesi
2. `STAGING/hyperlightdrifter_analysis.md` (401 satır) — HLD raw analiz
3. `STAGING/cinderia_analysis.md` (326 satır) — Cinderia raw analiz
4. `STAGING/hammerwatch2_analysis.md` (423 satır) — HW2 raw analiz
5. `F:\LaurethStudio\01_PIPELINE\stack_accumulation_mechanics.md` (yeni STUDIO_KARAR_015 aday) — Hexer + Cinderia Erosion sentezi

Ayrıca aşağıdaki RIMA bağlam dosyalarına bak (referans için):
- `TASARIM/MASTER_KARAR_BELGESI.md` (özellikle Karar #21 Hexer, #34 cinsiyet, #54 Perfect Condition, #71 silahsızlık, #143-A..R 6-layer map)
- `STAGING/karar_143_six_layer_map_architecture.md` (Aşama 1+2 LIVE)

---

## Review başlıkları (her madde için PASS/FAIL/CAUTION + sebep)

### A. Karar Adayı Numarası Tutarlılığı
- #149-152 HLD / #153-160 Cinderia / #161-170 HW2 ayrımı doğru mu?
- Mevcut #144-148 BanditKnightG ile çakışma var mı?
- Karar adayı listesinde duplicate veya benzer maddeler var mı?

### B. Tier 1 LOCK Adayları (4 madde)
- #149 Studio Signature Accent: HLD source güvenilir mi? Cyan/orange/mor tartışmalı mı?
- #150 4-Layer Visual DNA: Karar #143 6-layer ile çakışma yok mu? Implementation feasible mi?
- #152 Day-1 Accessibility: Mevcut FeelToggleSettings'e nasıl entegre edilir? Eksik bir slider var mı (örn. text-to-speech)?
- #161 4-Layer Meta Progression Separation: RIMA Rift Break design'ı bunu kaldırır mı yoksa refactor mı gerek?

### C. Tier 2 DEFER (8 madde)
- Hangileri gerçekten DEFER edilmeli, hangileri Tier 1'e taşınmalı?
- Faz numaralandırması (Faz 2/3/4) gerçekçi mi?
- Eksik bir DEFER karar adayı var mı (raw analiz dosyalarında geçen ama sentezde atlanan)?

### D. Tier 3 REJECT (6 madde)
- Hepsi gerçekten REJECT mi? "Silent narrative" REJECT — RIMA Karar #79 Tone Surfaces ile gerçekten çelişiyor mu?
- Bir REJECT madde aslında "kısmen kabul" olabilir mi?

### E. Anti-Pattern Listesi
- 6 anti-pattern eksiksiz mi? HW2 review'lerinde bahsedilen başka bir anti-pattern atlandı mı?

### F. Studio Universal Pattern (Stack/Accumulation)
- Felsefe (Bölüm 1) doğru mu? Pozitif/Negatif YÖN ayrımı sağlam mı?
- Hexer case study (Bölüm 2): Karar #21 ile birebir mi? Atlanan detay var mı?
- Cinderia Erosion case study (Bölüm 3): cinderia_analysis.md ile tutarlı mı?
- Cross-Game Application (Bölüm 4): CircuitBreaker ve Caterpillar önerileri makul mu?
- Code template (Bölüm 5): C# `AccumulationMeter` abstract class doğru tasarlanmış mı? `Cashout()` virtual yeterli mi yoksa abstract mı olmalı?
- UI standartları (Bölüm 6): Signature accent uyumu var mı?
- Anti-pattern listesi (Bölüm 7): 5 madde eksiksiz mi?

### G. Cross-Reference & Source Attribution
- Steam review alıntıları gerçek mi? Helpful vote sayıları doğrulanabilir mi (random spot check)?
- Source attribution her karar adayında var mı?

### H. RIMA Spesifik Tutarlılık
- Hexer = Cursemark karışıklığı (Cinderia agent hatası) düzgün düzeltildi mi?
- Karar #71 (Hexer silahsız) ile Stack pattern uyumlu mu?
- Karar #67 (caster windup not cancellable) ile Hexer ROC uyumlu mu?
- Yeni Karar adayları (#149-170) mevcut Karar #1-148 ile bir yerde çelişiyor mu?

### I. Studio Universal Pattern Genişletilebilirliği
- STUDIO_KARAR_015 (Stack/Accumulation) gerçekten Studio çapında pattern mi yoksa RIMA-specific mi?
- 3 oyun (RIMA + CB + Caterpillar) gerçekten bu pattern'i kullanır mı? Caterpillar Wingspan bunu nasıl uygular?

### J. Eksik Analiz / Atlanmış Şeyler
- HLD/Cinderia/HW2 raw analiz dosyalarında bahsedilen ama sentezde EKSIK kalan kritik nokta var mı?
- Üç oyunun ortak bir tasarım dersi (sentez Bölüm 2'de yer almayan) var mı?
- RIMA için pratik bir "şimdi yap" aksiyon eksiği var mı?

---

## Çıktı formatı

`CODEX_DONE_<profil>.md` içinde:

```markdown
# Codex Review — 3 Games Synthesis + Stack Pattern

## Verdict Summary
- Total findings: N
- BLOCKERS (must fix): M
- WARNINGS (should fix): K
- INFO (consider): L

## Section A — Karar Numbering
- [PASS/FAIL/CAUTION]: detail

## Section B — Tier 1 LOCK
- #149: [verdict] reason
- #150: [verdict] reason
- ...

(her bölüm için aynı format)

## Critical Findings (TOP 5 prioritized)
1. ...
2. ...

## Suggested Edits (concrete file:line)
- File X line Y: change "..." to "..."
- File Y line Z: ekle "..."

## Missing Analysis (sentez eksikleri)
- ...

## Source Verification
- [3 random Steam quote spot check pass/fail]
```

---

## Kısıtlar

- **Sadece review, kod yazma YOK.** Suggested Edits sadece öneri, uygulanması Opus sorumluluğu.
- **Steam quote spot check** — `curl -s` veya `WebFetch` ile en az 3 alıntıyı doğrula
- **Karar numbering çakışması** — MASTER_KARAR_BELGESI.md baş harfi ile cross-check zorunlu
- **Hexer detayları** — Karar #21, #34, #54, #71 ile birebir karşılaştır
- Output max 500 satır, kısa ve aksiyon-odaklı

## CODEX_DONE Protokol

`CODEX_DONE_<profil>.md` güncelle (cx_dispatch.py protokol).
