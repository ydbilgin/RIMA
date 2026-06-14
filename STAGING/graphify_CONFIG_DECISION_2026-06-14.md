# KARAR — graphify config (RIMA) · 2026-06-14

**Yöntem:** 2x2 deney (normal/deep × Sonnet/Opus) → 3-advisor council (cx + ax 3.1 Pro + ax 3.5 Flash) → Opus sentez. Deney verisi: `STAGING/_process/2026-06/graphify_config_council_brief.md` + `graphify_exp/*.json`. Advisor ham çıktıları: `_process/2026-06/_council_*` + `.cx_dispatch/CODEX_DONE__council_cx_graphify-config_*`.

## Oybirliği (3/3 advisor + deney)
1. **deep+sonnet ELENDİ.** 20 "değer-kenarı" şişirilmiş: aynı dış sembolü (Health/PlayerEconomy/SkillDatabase…) her dosyada ayrı node yapıp 0.95 "similar" bağlamış → ax Pro: bu "sahte klik"ler **community detection'ı (Louvain/PageRank) matematiksel olarak zehirler**, sadece gürültü değil hatalı topoloji. cx: downstream QC'ye konsolidasyon yükü, reuse/cost zayıf.
2. **Demo'ya 6 gün kala GLOBAL/full-codebase graphify YOK.** Üçü de: dikkat dağıtan makro-optimizasyon → **post-demo'ya raf.**
3. **deep+opus = cerrahi araç.** Küçük, insan-sınırlı (≤1 chunk, 20-25 dosya), yüksek-değerli bug/tasarım analizinde 5× maliyet haklı. Tek doğru-konsolide config (shared_* node). F2 ipucunu (`BuildChestOffers ↔ MaybeInjectEchoOffer`) **yalnızca bu** buldu.

## Kilit anlaşmazlık → Opus kararı
**Soru:** Mimari-harita için **normal+sonnet** mi yoksa **AST-only** mi?
- cx + ön-karar: normal+sonnet (ucuz, reward→draft yakalandı).
- ax Pro: normal+sonnet **değer illüzyonu** — LLM katmanı AST'nin zaten verdiğini tekrar buluyor (sadece 2 trivial semantic kenar ekledi). Mimari harita istiyorsan **LLM'i kapat, AST-only** (sıfır token, deterministik). Gerçek ayrım "iki LLM config" değil, **"AST-only vs deep+opus"**.

**KARAR = ax Pro lehine.** Gerekçe + graphify'ın kendi tasarımı bunu doğruluyor: graphify'ın **code-only "fast path"i Part B'yi (LLM semantic) zaten atlar** ("AST handles code — nothing for semantic subagents to do"). Yani RIMA kodu üzerinde graphify code-only = **default'ta AST-only**. normal+sonnet'in LLM katmanı için token yakmak, AST aynı iskeleti bedava verirken marjinal.

## NİHAİ POLİTİKA
| İhtiyaç | Config | Maliyet | Ne zaman |
|---|---|---|---|
| **Mimari harita** (yapı/çağrı/bağımlılık) | **AST-only** (graphify code-only fast-path) | ~0 token | İstendiğinde; ucuz, güvenli |
| **Bug-kokusu / sürpriz coupling** (örtülü bağ) | **deep+opus**, tek-chunk, insan-seçili dar korpus | ~5× ama küçük | Hedefli, yüksek-risk alan |
| ❌ normal+sonnet (map için), normal+opus, deep+sonnet | — | — | DOMINE/ELENDİ |
| ❌ Global full-codebase LLM run | — | — | **Post-demo** |

**Şimdi:** F2 ipucu zaten elimizde. ax Flash haklı: F1/F2'nin kendisi için düz grep/debug, ek graphify'dan hızlı. → İpucunu kullan, bug'lara dön. graphify full-map = post-demo.

## Aksiyon
- [ ] graphify config politikası KİLİT (bu doc).
- [ ] (İsteğe bağlı, post-demo) full-codebase AST-only harita.
- [ ] (İsteğe bağlı, şimdi) F2/Echo cluster'ında cerrahi deep+opus — ama önce düz debug denenir.
