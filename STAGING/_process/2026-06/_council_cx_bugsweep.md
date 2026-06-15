ACTIVE RULES: (1) think (2) min/no-speculation (3) surgical (4) BLOCKED if unclear.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json), ~71× ucuz.

# Amaç
Demo-kritik yüzeyde CODE-LEVEL bug taraması (cx feasibility lens). ANALYSIS ONLY.

OKU: STAGING/_process/2026-06/_council_bugsweep_2026-06-15.md (tam brief + scope + format ŞART) + scope'taki 6 dosya.

## CX LENS — code-level bug/risk
Tara: null-ref/NRE riski · state-desync (Director/BuildMode/draft state'leri çakışması) · lifecycle (Awake/Start/OnDestroy/DontDestroyOnLoad sıra/leak) · coroutine guard'ları (erken çıkış/sonsuz) · early-return huni'leri (F2 gibi sessiz return'ler başka yerde) · enum/case eksikleri (RewardType.CrossClassEcho missing case gibi) · event abone/çöz dengesizliği · edge-case'ler (oda 4/8 Forge, Echo cadence, ilk/son oda).

Format: brief'teki KESİN format. file:line ZORUNLU. DEMO-BLOCKING öne. CODEX_DONE.md'ye, ≤15 madde.
