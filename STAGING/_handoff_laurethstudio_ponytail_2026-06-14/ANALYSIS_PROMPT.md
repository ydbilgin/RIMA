# LaurethStudio — ponytail bagimsiz analiz istegi

> Bunu LaurethStudio'ya (Fable / kendi notebook'u) prompt olarak ver. `ponytail_opus_review.md` ile birlikte ekle. Amac: RIMA-disi, bagimsiz bir ikinci goz.

---

## Prompt (kopyala-yapistir)

Asagidaki aracin **bizim (LaurethStudio) is akisimiz** acisindan bagimsiz analizini yap. RIMA ekibinin Opus analizi ekte (`ponytail_opus_review.md`) — onu OKU ama **kendi yargini kur**, kor kabul etme.

**Arac:** ponytail — https://github.com/DietrichGebert/ponytail
- MIT, ~4.5k yildiz, aktif (v4.2.0, Haz 2026). Cross-agent "tembel senior dev" plugin'i: AI kod-ajanlarini minimal kod yazmaya iten kural+hook+skill sistemi (skip-kod -> stdlib -> native -> mevcut-dep -> one-liner). Claude Code / Codex / Cursor / Windsurf / Cline / Copilot / Aider / Kiro destegi. %97.8 JS. Iddia: %80-94 daha az kod, %47-77 maliyet dususu, 3-6x hiz.

**Cevapla:**
1. Ne ise yarar, gercek mekanigi ne (RIMA analizindeki "her-prompt hook + session enjeksiyon, tool-call kesmiyor" tespitini dogrula/curut).
2. LaurethStudio'nun coklu-projesinde (ozellikle JS/web tarafinda) bu plugin gercek deger katar mi? RIMA'nin "C# transferi zayif, benchmark'lar JS" tespiti LaurethStudio icin TERSINE avantaja mi doner (siz JS yapiyorsaniz)?
3. Mevcut kendi agent-disiplininizle cakisir mi, tamamlar mi?
4. Tam plugin mi, yoksa sadece /ponytail-review pattern'i mi? adopt-now / pilot / skip — gerekce ile.
5. RIMA verdict'i (post-demo, kismi, sadece /ponytail-review) LaurethStudio icin de gecerli mi, yoksa farkli mi? FARKI acikla.

Kisa, gerekce-li, kendi baglaminiza ozel.

---

## Notlar (RIMA tarafi)
- RIMA verdict'i: **post-demo, kismi** — tam plugin skip (mevcut Karpathy-4 disiplini ile %80 overlap, C# transferi zayif, demo penceresinde her-prompt hook gereksiz risk); tek deger = /ponytail-review lens'ini plugin-siz checklist olarak almak.
- RIMA, ayrica kendi 3-advisor council'ini (cx + ax Pro + ax Flash) ponytail icin calistiriyor; sonuclari isteyince eklerim (bu klasore).
