# TASK: RIMA Reward + Portal Flow — Genre Research + Player Psychology

ACTIVE RULES: (1) think before reviewing (2) min response, no speculation (3) cite specific games/data points (4) BLOCKED if uncertain.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: RIMA için Reward + Portal Flow kararı verilirken **genre standartları + player psychology** açısından araştırma. Hangi pattern hangi oyunda hangi sebeple çalıştı/çökmedi, RIMA'nın spesifik koşullarına en uygun olan hangisi.

Çıktı **inline** (dosya YOK), 600-1000 kelime.

## RIMA Context
- Top-down 3/4 roguelite ARPG, Hades / Children of Morta / Diablo III tarzı
- 8 sınıf roster v2 (Warblade, Elementalist + 6 diğer)
- Wall-less Elysium görsel dili LOCKED
- Hedef run length: belirsiz (input bekleniyor — Hades 30-45dk, Dead Cells 30-60dk, StS 60-90dk)
- OmerDev "Why Most Indie Games Fail" dersinden çıkan #4: rastgele seçim → oyuncu kendini suçlar, "build yanlıştı" der → tekrar oyna
- Karpathy 4 — min spec, sade ilk

## 5 Pattern (analiz edilecek)
| # | Akış | Tipik referans |
|---|---|---|
| A | Clear → 1 ödül → 1 portal | Generic ARPG, Diablo |
| B | Clear → 1 ödül → 3 portal (oda türü seçim) | Hades door choice |
| C | Clear → 3 boon seçim → 1 portal | Hades boon room, Slay the Spire kart seçim |
| D | Clear → 3 portal + her birinde reward preview (combo) | Slay the Spire path overlay |
| E | Clear → 2 reward + 2 portal (tied — hangisini alırsan o portal) | Custom (örnek bul) |

## Görev — 4 bölüm

### Bölüm 1: Genre Standartları
- Hades / Dead Cells / Slay the Spire / Returnal / Children of Morta / Hyper Light Drifter / Risk of Rain 2 / Enter the Gungeon — bunların reward+portal flow'ları ne, hangi pattern?
- Sales + Steam playtime data ile başarı kanıtı (eğer biliyorsan): hangi oyun ne kadar median playtime aldı?
- Genre evolution — eski roguelike (NetHack, ADOM) → modern roguelite (Hades, Dead Cells) → reward flow nasıl değişti?

### Bölüm 2: Player Psychology — "Random Choice" Mechaniği
- OmerDev'in tezi (random choice = oyuncu kendini suçlar) genre genelinde geçerli mi?
- Sıkça yapılan tasarım hataları neler? (örnek: çok fazla seçenek = decision fatigue, çok az = boring)
- "Magic number" of choices — neden 3? (StS 3 kart, Hades 3 boon, Dead Cells 2-3 buff). 4 veya 5 neden değil?
- Reward "value transparency" — oyuncu seçimin etkisini ne zaman görmeli? (instant vs run sonu vs meta-progression)

### Bölüm 3: RIMA Özelinde Risk + Fırsat Analizi
- Wall-less Elysium görsel dili → portal nerede ve nasıl belirir? (yüzen platform / arena merkez / kenar)
- 8 sınıf → her sınıfın kendi boon pool'u mu yoksa shared mi? (Hades = shared, Slay the Spire = class-specific)
- Hades-style "boon synergy" mekanizması RIMA'ya değer katar mı yoksa scope creep mi?
- Decision fatigue scaling: RIMA bir runda kaç ödül noktası vermeli? (Hades ~15 boon, Dead Cells ~10 perk)
- "Meta-progression" (run arası kalıcı ilerleme) — RIMA'da olacak mı, varsa flow'u nasıl etkiler?

### Bölüm 4: Verdict — Genre/Psychology Açısından Öneri
- En önerilen pattern (1 adet, kesin)
- Sebep (player retention + design philosophy)
- MVP strategy (önce hangi pattern, sonra hangisine geçilir)
- Risk: RIMA'nın hangi kararına çelişir
- OmerDev'in "ilk 15 dakika kuralı" ile nasıl uyumlu — onboarding'de reward flow nasıl tanıtılır

## Hard constraints
- Inline only
- Speculative chaining yasak
- Spesifik oyun isimleri + spesifik data points zorunlu
- "Bilmiyorum" demek serbest — uydurma
