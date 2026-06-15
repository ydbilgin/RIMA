# Routing Revizyonu — Sonnet-execute + zorunlu audit (2026-06-15, kullanıcı onaylı)

## KARAR
"Opus executes" (2026-06-13 kuralı) **mutlak değil**. İki yola ayrılır:

| İş tipi | Executor | Audit (ZORUNLU gate, writer≠reviewer) |
|---|---|---|
| **İyi-specli cerrahi/mekanik** (kilitli spec + dosya:satır belli; ör. demo batch-fix) | **crafter-sonnet** (ayrı Sonnet kotası → Opus korunur) | **auditor-opus** + kritikse **council** (cx+ax Pro+ax Flash) |
| **Yeni / tasarım-hassas / specsiz** (ör. post-demo B9/B7/A2 mekanikleri) | önce **Opus/council DESIGN** (spec yazılır), sonra crafter-sonnet implement | spec'i council review + implement'i auditor-opus |

**Neden bu, "Opus-execute kuralını delmek" değil:** O kuralın amacı = limit baskısıyla kalite düşmesin. Burada Sonnet-execute'a **daha güçlü bir denetim katmanı** (Opus + council audit) eşlik ediyor → telafi edilmiş bilinçli mimari, limit-kayması değil. Memory: [[opus-executes-cx-reviews]] (REVİZYON bloğu).

## SERT KISIT — Unity-serial (HARD RULE [[one-unity-agent-at-a-time]])
Unity-süren execute = **TEK serial Unity ajanı**. Workflow fan-out / worktree izolasyonu **tek Unity Editor'a karşı ÇALIŞMAZ** (eşzamanlı MCP = embedded python köprü çökmesi, 2026-06-13). → Unity-touch eden her faz seri.

## DOĞRU SIRA VE MANTIK (kullanıcı direktifi "ikisini de yap ama doğru sıra")
1. **ÖNCE: Demo batch-fix (ŞİMDİ, time-critical, locked #1).** Şekil = ağır paralel workflow DEĞİL (6 minik edit + Unity-serial → overkill). crafter-sonnet 6 fix'i seri uygular + compile/verify (TEK Unity ajanı) → auditor-opus spec'e karşı review → orchestrator gate. *(Bu doküman yazılırken çalışıyor: crafter-sonnet dispatch.)*
2. **SONRA (demo güvenceye alınınca): Post-demo two-phase workflow.** Aşağıda.

## POST-DEMO TWO-PHASE WORKFLOW (hazır; demo PASS'ten SONRA fire et)
Adaylar (council önceliği): **B9** (rotating environmental rule, environment-tezi) > **B7** (placement→resource) ≥ **A2** (posture-crack glyph). B1/B4 = sonraki dalga.

- **FAZ 1 — DESIGN (Workflow'un parladığı yer; Unity YOK → paralel olabilir):**
  `pipeline([B9, B7, A2], stage1=Opus design-spec yaz (RIMA canon + NLM), stage2=council (cx+ax) spec review)`.
  ⚠️ A2 ön-koşulu: **postureOverflow consumer kodda kanıtlı değil** (cx council bulgusu) → A2 design-stage'i ÖNCE bu consumer tamamlığını doğrular; eksikse spec onu kapsar.
- **FAZ 2 — IMPLEMENT (Unity-serial → fan-out YOK; SIRALI dispatch):**
  Her mekanik için sırayla: crafter-sonnet implement → TEK serial Unity verify → auditor-opus review. Workflow paralelliği burada KULLANILMAZ (Unity tek Editor).

> Yani Workflow tool'u FAZ-1 design için uygun; FAZ-2 implement seri dispatch. Demo bitmeden FAZ-1 başlatma (odak + zaman demo'da).
