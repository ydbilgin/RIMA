# ponytail — Adoption Review for RIMA (Opus, 2026-06-14)

Repo: https://github.com/DietrichGebert/ponytail · MIT · ~4.5k yildiz · v4.2.0 (2026-06-13) · JavaScript 97.8% · 189 fork
Karar penceresi: bitirme demo ~20 Haziran (≈6 gun), degisime kapali yuksek-risk donem.

> Tum iddialar repodan WebFetch ile dogrulandi (README, .claude-plugin/, hooks/, skills/, benchmarks/README). Birebir alintilar tirnak icinde.

---

## 1. Ne yapiyor (mekanik / entegrasyon ayak izi)

ponytail bir "lazy senior dev" davranis-yonlendirme eklentisi. Cekirdek kural metni dile-notr bir karar merdiveni:

> "1. Does this need to exist? → no: skip it (YAGNI)
> 2. Stdlib does it? → use it
> 3. Native platform feature? → use it
> 4. Installed dependency? → use it
> 5. One line? → one line
> 6. Only then: the minimum that works"

Korunan sinir (asla kisilmez):
> "trust-boundary validation, data-loss handling, security, and accessibility are never on the chopping block."

### Claude Code'a kurulunca FIILEN ne dokunuyor

Marketplace plugin (`.claude-plugin/marketplace.json` + `plugin.json` yalin metadata). Claude Code plugin convention'i ile `hooks/hooks.json` ve `skills/` otomatik yuklenir. `hooks/hooks.json` icerigi (dogrulandi):

- **SessionStart** hook — matcher `"startup|resume|clear|compact"` → `node ponytail-activate.js` (5sn timeout). Oturum basinda ponytail kural metnini context'e enjekte eder.
- **UserPromptSubmit** hook — matcher yok (HER prompt) → `node ponytail-mode-tracker.js` (5sn timeout). Her prompt'ta `/ponytail` mod komutlarini ve `"stop ponytail|normal mode"` deaktivasyon ifadelerini tarar, mod durumunu yazar.

KRITIK: `ponytail-instructions.js` (~230 satir) kurali context'e **enjekte eder**, ama **tool call'lari bloklamaz/degistirmez**. `mode-tracker.js` de "never blocks or denies tool calls or prompts; only injects/print[s] context text". Yani bu bir **system-prompt enjeksiyonu**, PreToolUse/PostToolUse interceptor DEGIL.

CLAUDE.md'ye dokunmuyor / uzerine yazmiyor (README dogruladi). Footprint = plugin marketplace girisi + 2 lifecycle hook (her node cagrisi) + 3 skill.

Modlar: **lite** (sadece tembel yolu isimlendir) · **full** (merdiven zorlanir, default) · **ultra** (YAGNI ekstremist).

Diger ajanlar farkli yontemle: Codex iki lifecycle hook'u "review and trust" ister; Cursor/Windsurf/Cline/Copilot/Aider/Kiro icin agent-ozel rules dosyalari kopyalanir (`.cursor/rules/`, `.windsurf/rules/`, `.clinerules/`, `.github/copilot-instructions.md`, `.kiro/steering/ponytail.md`). RIMA'nin cx (Codex) dispatch'i bunlardan **otomatik yararlanmaz** — cx zaten kendi non-interactive wrapper'i; ponytail Codex hook'lari ayri kurulum/guven adimi ister.

## 2. Yonlendirme nasil calisiyor

Tool call interception YOK. Mekanizma: oturum/prompt basina kural metni context'e basilir + iki yardimci skill:

- **skills/ponytail/SKILL.md** — yukaridaki merdiven; "Deletion over addition. Boring over clever"; "no interface with one implementation, no factory for one product"; bilincli sadelestirmeleri `ponytail:` yorumuyla isaretle (tavan + upgrade yolu).
- **skills/ponytail-review/SKILL.md** — talep-uzerine pass (`/ponytail-review` veya "what can we delete"). Dogruluk/guvenlik bakmaz; SADECE silinecek kompleksiteyi avlar: (1) yeniden-icat edilen stdlib, (2) gereksiz bagimliliklar, (3) olu kod/spekulatif soyutlama, (4) tek-implementasyonlu soyutlama, (5) ayni isi daha az satirla. Cikti = location + sil + yerine ne. **Bu, RIMA'nin auditor-opus/cx-review akisina temiz takilabilecek tek bagimsiz parca.**
- **skills/ponytail-help** — yardim.

## 3. Benchmark guvenilirligi — pazarlama mi?

Kismen reproducible, ama **dar ve JS/web-sekilli**. `benchmarks/README` (dogrulandi):

- Gorevler: "email validator, JS debounce, CSV sum, React countdown, FastAPI rate-limit" — 5 gunluk mikro-gorev. **Hicbiri Unity/C#/oyun degil; hepsi web/JS/Python.**
- "10 runs per cell, median reported"; 3 Claude modeli (Haiku/Sonnet/Opus); 3 kol (no-skill / caveman / ponytail).
- Olcum: "Code LOC counted from fenced code blocks; tokens, cost, latency straight from the API"; "single-shot completions, default temperature".
- Ham: ponytail 39-51 LOC vs baseline 256-693; maliyet $0.010-0.071 vs $0.032-0.141; gecikme 9.9-18.0sn vs 37.7-124.1sn.
- Yazar uyarisi: "Cost reflects single-shot calls that re-send the skill every time. In real sessions the skill is injected once and prompt-cached." Yani gercek maliyet farki tek-atimlik test gibi degil.
- Reproduce: `npx promptfoo eval -c benchmarks/promptfooconfig.yaml`.

Yargı: metodoloji acik ve tekrarlanabilir (artı), ama %80-94/%47-77 rakamlari **kisitsiz bir agent'in 5 trivial JS/Python gorevde sisirdigi koddan** turuyor. RIMA'da gercek is "var olan C# sistemine cerrahi degisiklik" — bu rakamlar transfer ETMEZ. Pazarlama degil ama dar; basligi RIMA'ya tasimak yaniltici olur.

## 4. C#/Unity transferi — gercek deger mi, JS-sekilli mi?

Cekirdek merdiven dile-notr (ornekler Python `@lru_cache`, HTML `<input type="date">`; C# YOK). Felsefe (YAGNI, soyutlama-karsiti, silme-yanlisi) C#'a tasinir.

AMA degerin buyuk kismi JS-sekilli ve RIMA baglamiyla **catisik**:
- "stdlib first / one-liner / installed dependency" idiomu JS/Python kalabalik-stdlib + npm dunyasi icin keskin. Unity C#'ta esdeger refleks "var olan RIMA sistemini/component'i yeniden kullan, yeni MonoBehaviour/abstraction yazma" — ki RIMA bunu **zaten** [[project_modular_design_philosophy]] ("modulerligi hak ediyor mu?") ve PROJECT_RULES "MINIMUM code/no speculation" ile kodluyor.
- ponytail "boring over clever, deletion over addition" der; RIMA design felsefesi ise **signature/boss/Echo bespoke kalir** der — ultra modun "YAGNI ekstremist" tavri RIMA'nin bespoke-content disiplinine ters dusebilir (asiri-sadelestirme = signature mekanigi yok etme riski).
- Benchmark'larin hicbiri oyun/C# degil; kanit tabani RIMA is-yukune sifir.

Net: felsefe %100 ortusuyor, **uygulanabilir ekstra deger ~%15** (cogu zaten var); benchmark vaadi RIMA'ya transfer etmez.

## 5. RIMA mevcut kurulumuyla ortusme

RIMA'nin PROJECT_RULES "Karpathy 4"u zaten enkode ediyor: think-before-coding · MINIMUM code/no speculation · SURGICAL (sadece listeli dosyalar) · BLOCKED-if-unclear; her dispatch "ACTIVE RULES:" satiriyla bunu enjekte ediyor. ponytail'in cekirdegi bunun **buyuk olcude duplikati**. ponytail "SURGICAL/sadece-listeli-dosya" ve "BLOCKED-if-unclear" boyutlarini ICERMIYOR (RIMA daha siki); RIMA ise "stdlib/native-feature merdiveni"ni acik dile getirmiyor (ponytail daha eksplisit). Cakisma alani buyuk, RIMA tarafi cogunlukla zaten daha kapsamli.

Cross-agent tutarlilik vaadi (Codex'i de hizalama) RIMA icin **otomatik gelmiyor**: cx kendi wrapper'iyla cagriliyor; ponytail Codex entegrasyonu ayri "trust two hooks" kurulumu ister, RIMA'nin cx dispatch'ine bedava takilmaz.

---

## Faydalar (BENIMSENIRSE)

1. **`/ponytail-review` over-engineering pass** — RIMA'da bagimsiz, dusuk-riskli deger. cx/auditor-opus review'una "ne silebiliriz" lens'i ekler; checklist'i (reinvented-stdlib, tek-impl abstraction, olu kod) somut. Tek basina skill olarak (plugin kurmadan) bile kopyalanabilir.
2. **Merdivenin eksplisit dili** — "stdlib → native → existing dep → one line" PROJECT_RULES'a iki cumle olarak eklenebilir; RIMA'nin "MINIMUM code"unu daha operasyonel yapar (ama bu plugin kurmayi gerektirmez, sadece metin).

## Riskler / catismalar

1. **Demo penceresi + her-prompt hook** — 6 gun kala UserPromptSubmit'te HER prompt'ta node cagrisi (5sn timeout) + SessionStart enjeksiyonu = davranis/context degisikligi, marjinal latency ve "bir sey karismasin" [[feedback_no_debug_state_leak]] kuralina ters yeni degisken. Yuksek-risk donemde plugin-seviye kurulum gereksiz yuzey.
2. **Duplikasyon + ultra-mod catismasi** — Karpathy-4 ile %80 ayni metni ikinci kez enjekte etmek context israfi ([[feedback_context_economy_rules]]); ultra'nin "YAGNI ekstremist" tavri RIMA'nin bespoke signature/boss disiplinine ([[project_modular_design_philosophy]]) ters cekebilir. Cross-agent tutarlilik (cx) vaadi de RIMA setup'inda otomatik gerceklesmez.

## VERDICT: **adopt-post-demo (kismi: sadece `/ponytail-review` deseni)** 

Cekirdek felsefe RIMA'nin hand-rolled Karpathy-4 + modular-design disiplininin duplikati; benchmark vaadi JS/web-sekilli ve C#/Unity'ye transfer etmiyor; demo penceresinde plugin-seviye + her-prompt hook kurmak gereksiz risk. Tek gercek katma-deger `/ponytail-review` (silme-odakli pass) — onu da plugin kurmadan, demodan sonra bir skill/checklist olarak ic review akisina ekle. Tam plugin benimsemesi = SKIP; review-deseni odunc al = post-demo.
