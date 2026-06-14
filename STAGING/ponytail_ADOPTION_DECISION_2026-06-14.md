# KARAR — ponytail adoption (RIMA) · 2026-06-14

**Soru:** ponytail (cross-agent AI kod-minimalizm plugin'i, https://github.com/DietrichGebert/ponytail) RIMA'da kullanilmali mi?
**Yöntem:** Opus subagent deep-dive + 3-advisor council (cx + ax 3.1 Pro + ax 3.5 Flash). Materyal: `STAGING/_process/2026-06/ponytail_opus_review.md` + `_council_*_ponytail.md` + `.cx_dispatch/CODEX_DONE__council_cx_ponytail_*`.

## KARAR (4/4 OYBIRLIGI): Tam plugin SKIP. Post-demo, KISMI/MANUEL.

- **Şimdi:** plugin'i KURMA (demo penceresi, production freeze).
- **Post-demo:** tek değer = `/ponytail-review` silme/minimalizm lens'ini **plugin-siz checklist** olarak cx-review / rima-qc / auditor-opus pipeline'ına kopyala.

## Gerekçeler (advisor uzlaşısı)
1. **~%80 overlap** — PROJECT_RULES Karpathy-4 (think/min/surgical/BLOCKED) çekirdek disiplini zaten içeriyor; RIMA dahası **daha sıkı** (surgical/listed-files + BLOCKED ponytail'de yok).
2. **Context-ekonomisi ihlali** — her-prompt `UserPromptSubmit` hook + session enjeksiyon = E1-E8 + "hiçbir şey karışmamalı" state izolasyonuna doğrudan aykırı.
3. **Bespoke çatışması** — YAGNI-ekstremist silme-yanlısı duruş, RIMA'nın signature/boss/skill bespoke kod ihtiyacıyla (`project_modular_design_philosophy`) çatışır.
4. **C# transferi yok** — benchmark'lar JS/Python web micro-task; %80-94/%47-77 rakamları Unity C# cerrahi-edit'e transfer ETMEZ. Ölçülebilir fayda = yerel A/B benchmark gerektirir (yok).
5. **Cross-agent bedava değil** — ponytail'in Codex desteği RIMA'nın cx-dispatch wrapper'ına otomatik geçmez; checklist'i cx task brief'ine elle enjekte etmek gerekir.

## Post-demo somut reuse (opsiyonel)
- `ponytail-review` checklist: reinvented-stdlib · gereksiz-dependency · dead/speculative kod · tek-implementasyon abstraction · aynı davranış daha-az-satır.
- Opsiyonel PROJECT_RULES'a 2-satır "reuse merdiveni": mevcut RIMA component → stdlib/native Unity/C# → kurulu dependency → one-line/basit → ancak o zaman yeni abstraction.
- RIMA'nın daha sıkı kuralları OTORITE kalır.

## Aksiyon
- [ ] Demo öncesi: hiçbir şey yapma (skip).
- [ ] Post-demo: `/ponytail-review` checklist'ini rima-qc/cx-review'a ekle.
- [x] LaurethStudio bağımsız analizi için handoff bundle: `STAGING/_handoff_laurethstudio_ponytail_2026-06-14/`.
