# RIMA tarafi — ponytail council sentezi (LaurethStudio'ya referans)

> RIMA'nin ponytail icin calistirdigi 3-advisor council + Opus subagent ozeti. LaurethStudio kendi yargisini kurarken karsilastirma icin. NOT: bu RIMA-baglamli (Unity C#, demo'ya 6 gun, mevcut Karpathy-4 disiplini); LaurethStudio'nun JS/web projelerinde sonuc FARKLI olabilir.

## Verdict: 4/4 oybirligi — tam plugin SKIP, post-demo kismi (sadece /ponytail-review checklist)

| Advisor | Lens | Karar |
|---|---|---|
| Opus subagent | teknik + fit | adopt-post-demo, partial; full plugin skip |
| cx (Codex) | feasibility/reuse | adopt-now: no; post-demo partial/manual; cross-agent Codex consistency NOT free |
| ax 3.1 Pro | mimari/stratejik | RED (plugin) / ASIMILE ET (sadece /ponytail-review); iki rakip kural sistemi + context ihlali |
| ax 3.5 Flash | yalin/ship-fast | simdi sifir-deger, skip; post-demo borrow-pattern |

## Ortak gerekceler
1. ~%80 overlap RIMA Karpathy-4 ile (RIMA dahasi daha siki).
2. Her-prompt UserPromptSubmit hook = context-ekonomisi + state izolasyonu ihlali.
3. YAGNI-ekstremist silme egilimi bespoke boss/skill koduyla catisir.
4. C# transferi yok (benchmark'lar JS/Python; olculmedi).
5. cross-agent Codex tutarliligi bedava degil (cx ayri wrapper).

## RIMA icin reuse plani (post-demo)
- /ponytail-review silme/minimalizm checklist'i -> rima-qc / cx-review pipeline'ina (plugin-siz).
- Opsiyonel 2-satir reuse-merdiveni PROJECT_RULES'a.

## LaurethStudio icin acik soru
RIMA'nin "C# transferi zayif" tespiti, LaurethStudio JS/web yapiyorsa TERSINE avantaj olabilir — ponytail'in asil benchmark'lari tam o alanda. Kendi baglaminizda degerlendirin.
