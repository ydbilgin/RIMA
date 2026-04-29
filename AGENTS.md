# AGENTS.md
> **Ne zaman yÃ¼kle:** Ajan seÃ§imi, gÃ¶rev daÄŸÄ±lÄ±mÄ± veya routing kararÄ± yaparken.

---

## Orchestrator Routing Tablosu

Claude orkestra ÅŸefidir â€” her iÅŸi kendisi yapmaz, doÄŸru agent'a yÃ¶nlendirir.
**SOLID kuralÄ±:** Her agent tek iÅŸ yapar. Claude Ã¼retim yapmaz, review yapmaz, prompt yazmaz â€” sadece orkestra eder.

| Ä°ÅŸ | Agent | Tek sorumluluÄŸu |
|----|-------|----------------|
| CURRENT_STATUS / SYSTEM_MAP / guide gÃ¼ncelleme | **rima-doc** | Doc yaz/gÃ¼ncelle |
| CODEX_TASKS.md yazma | **rima-doc** | Doc yaz/gÃ¼ncelle |
| Memory dosyasÄ± yazma/gÃ¼ncelleme | **rima-doc** | Doc yaz/gÃ¼ncelle |
| ArÅŸivleme, dosya taÅŸÄ±ma | **rima-doc** | Doc yaz/gÃ¼ncelle |
| Codex Ã§Ä±ktÄ± review, C# script kalite | **rima-qc** | Validate et, rapor dÃ¶n |
| PixelLab/Gemini gÃ¶rsel QC | **rima-qc** | Validate et, rapor dÃ¶n |
| Lint (belge tutarlÄ±lÄ±ÄŸÄ± cross-check) | **rima-qc** | Validate et, rapor dÃ¶n |
| PixelLab / ChatGPT prompt dosyasÄ± yazÄ±mÄ± | **rima-asset** | Prompt Ã¼ret (4-yÃ¶n kilit) |
| Animasyon frame/direction planlamasÄ± | **rima-asset** | Prompt Ã¼ret (4-yÃ¶n kilit) |
| **TÃ¼m bilgi prompt'a sÄ±ÄŸan Ã¼retim/dÃ¶nÃ¼ÅŸtÃ¼rme** | **general-purpose** | Dosya okumadan Ã§Ä±ktÄ± Ã¼ret |
| Class/skill/boss/oda tasarÄ±m kararÄ± | **rima-design** | TasarÄ±m karar ver |
| Cross-system trade-off kararÄ± | **rima-design** | TasarÄ±m karar ver |
| Playtest design + execution (MCP scenarios) | **Codex (GPT-5.5)** | Senaryo yaz, run_tests koş, rapor dön |
| Web araştırma / dış kaynak lookup | **Gemini CLI** | Web search only — Unity/kod kararı yok |
| Root cause debug, mimari karar | **Claude** | Devredilmez |
| Cross-file refactor, test yazma | **Claude** | Devredilmez |
| 1-2 satÄ±r direkt edit | **Claude** | Devredilmez |
| Final karar (agent Ã§Ä±ktÄ±sÄ± sonrasÄ±) | **Claude** | Devredilmez |

**general-purpose ne zaman:** Tablo/format dÃ¶nÃ¼ÅŸtÃ¼rme, CODEX_TASKS iÃ§eriÄŸi taslaÄŸÄ±, kÄ±sa metin Ã¼retimi â€” verilen spec dÄ±ÅŸÄ±nda hiÃ§bir bilgiye ihtiyaÃ§ yok.
**Spawn eÅŸiÄŸi:** 1-2 satÄ±r direkt edit â†’ Claude. Her ÅŸey else â†’ ilgili agent.

---

## Sub-Agent Tablosu (`.claude/agents/`)

| Agent | Model | Tek Sorumluluk | Write |
|-------|-------|---------------|-------|
| `rima-design` | Opus 4.7 | TasarÄ±m kararÄ± â€” class/skill/boss/oda/trade-off | âŒ |
| `rima-doc` | Sonnet 4.6 | Doc yaz/gÃ¼ncelle â€” status, guide, memory, arÅŸiv | âœ… |
| `rima-qc` | Sonnet 4.6 | Validate et, rapor dÃ¶n â€” kod, gÃ¶rsel, lint | âŒ |
| `rima-asset` | Sonnet 4.6 | Prompt Ã¼ret (4-yÃ¶n kilit) â€” PixelLab/ChatGPT, `STAGING/` altÄ±na | âœ… (`STAGING/`) |
| `general-purpose` | Sonnet 4.6 | Prompt-only Ã¼retim â€” dosya okumadan, spec'ten Ã§Ä±ktÄ± | âœ… (belirtilirse) |

**Kural:** HiÃ§bir sub-agent baÅŸka sub-agent spawn edemez.
**Token kuralÄ±:** Her agent kendi sÄ±nÄ±rÄ± dÄ±ÅŸÄ±na Ã§Ä±karsa Claude'a escalate eder â€” baÅŸka agent iÅŸine el atmaz.

---

## /clear Sinyal KurallarÄ±

Claude context dolumunu takip eder â€” kullanÄ±cÄ± hatÄ±rlatmaz.

| Durum | Sinyal |
|-------|--------|
| Review/QC dÃ¶ngÃ¼sÃ¼ bitti (PASS aldÄ±) | "â†’ /clear at, Ã¼retim temiz session'dan" |
| Yeni Ã¼retim fazÄ± baÅŸlamadan Ã¶nce | "â†’ /clear Ã¶neriyorum, context aÄŸÄ±rlaÅŸtÄ±" |
| 20+ mesaja ulaÅŸÄ±ldÄ± | Otomatik uyar |
| AÄŸÄ±r doc gÃ¼ncelleme batch'i bitti | "â†’ /clear at" |
| Codex'ten sonuÃ§ geldi, bir sonraki iÅŸ bÃ¼yÃ¼k | "â†’ /clear at, sonraki iÅŸ temiz baÅŸlasÄ±n" |

**Token-first kural:** Bir iÅŸ yapmadan Ã¶nce "bu iÅŸi daha az token'la yapabilir miyim?" diye dÃ¼ÅŸÃ¼n. Cevap evetse kullanÄ±cÄ±ya Ã¶ner.

---

## State / Memory KaybÄ± Riski

Agent'lar konuÅŸma context'ini gÃ¶rmez â€” sadece dosyalarÄ± okur. Bu yÃ¼zden:

- Agent spawn etmeden Ã¶nce, konuÅŸmadaki kritik kararlarÄ± dosyalara yaz
- CURRENT_STATUS.md ve ilgili memory dosyalarÄ± gÃ¼ncel olmalÄ±
- Aksi halde agent eski bilgiyle Ã§alÄ±ÅŸÄ±r

**Pratik kural:** Ã–nemli bir karar alÄ±ndÄ± â†’ Ã¶nce rima-doc ile kaydet â†’ sonra baÅŸka agent spawn et.

---

## Codex â€” Ä°zole Kod UzmanÄ±

**YapabileceÄŸi:** ISOLATED_CODE â€” utility script, bounded refactor, iyi tanÄ±mlÄ± implementasyon

**Kesinlikle yapamaz:** Mimari karar, Ã§oklu sistem deÄŸiÅŸikliÄŸi, QC final yargÄ±sÄ±

**Codex Ã§Ä±ktÄ±sÄ± Claude QC'sinden geÃ§meden projeye alÄ±nmaz.**

#### Codex GÃ¶rev FormatÄ± (CODEX_TASKS.md)

```
TASK: [tek cÃ¼mle gÃ¶rev]

ALLOWED FILES (sadece bunlara dokun):
- Assets/Scripts/[dosya.cs]

FORBIDDEN:
- Bu listede olmayan hiÃ§bir dosyaya, scripte, prefaba veya sahneye dokunma
- HiÃ§bir ÅŸeyi silme
- Yeni dosya oluÅŸturma â€” aksi belirtilmedikÃ§e
- TASARIM/, GUIDES/, ARCHIVE/ klasÃ¶rlerine dokunma

UNITY MCP WORKFLOW:
1. read_console â†’ mevcut hata sayÄ±sÄ±nÄ± not al
2. DeÄŸiÅŸikliÄŸi uygula
3. read_console â†’ compile hatasÄ± kontrol et
4. Temizse rapor et

SCOPE KAYARSA: dur ve rapor et.

REPORT FORMAT:
STATUS: DONE / FAILED / PARTIAL
COMPLETED: [liste]
ERRORS: [hata veya NONE]
NEXT_SIGNAL: "[Claude'a tetikleyici]"
```

---

## Gemini CLI — Web Research Only

**Can do:** Web search, external documentation lookup, Unity forum/changelog research
**Cannot do:** Unity MCP execution, playtest, code decisions, project file writes
**Handoff:** Claude asks specific question → user runs Gemini → pastes result back

**Rule:** Gemini researches, Claude decides. No MCP, no playtest, no design.

---

## Ollama Local (RTX 5080) â€” HazÄ±rlÄ±k DesteÄŸi

**YapabileceÄŸi:** Log analizi, dÃ¶kÃ¼man clustering, ucuz hazÄ±rlÄ±k, offline araÅŸtÄ±rma

**Yapamaz:** Final karar, mimari Ã¶neri, projeyi doÄŸrudan etkileyecek Ã§Ä±ktÄ±

**Kural:** Ollama hazÄ±rlar, Claude karar verir.

---

## Escalation

Devredilen gÃ¶rev ÅŸu durumda Claude'a geri dÃ¶ner:
- Scope kayarsa
- Ã‡Ä±ktÄ± kalitesi yoruma aÃ§Ä±k hale gelirse
- BaÅŸka sistemi/dosyayÄ± etkileme riski doÄŸarsa
- CURRENT_STATUS veya proje kurallarÄ±yla Ã§eliÅŸki olursa

---

## Yasak Liste (tÃ¼m ajanlar)

- `RIMA/Library/` ve `PackageCache/` iÃ§ine dokunma
- `TASARIM/GDD.md` deÄŸiÅŸtirme (sadece Claude)
- ArÅŸivlenmiÅŸ dosyalarÄ± onay olmadan aÃ§ma
- AynÄ± dosyaya iki ajan paralel yazma
