# PROMPT → LaurethStudio (agent mimarisi: global base + proje mixin)

> Bunu LaurethStudio session'ına yapıştır. Self-contained — RIMA bağlamı varsaymaz.

---

## Görev
Bu projenin (LaurethStudio) sub-agent mimarisini "base + mixin" (kalıtım-benzeri kompozisyon) modeline geçir. Önce **araştır/doğrula**, sonra **kendine uyarla**, sonra **kalıcı kaydet** (memory + NLM). Aşağıda doğrulanmış platform gerçekleri ve RIMA'da kurulan referans pattern var; bunları başlangıç noktası al, kendi orkestrasyon modeline (Fable) göre adapte et.

## 1. Doğrulanmış Claude Code gerçekleri (platform kısıtları — önce bunları teyit et)
Resmî davranış (RIMA'da claude-code-guide ile doğrulandı; sen de teyit et, değişmiş olabilir):
- Agent system-prompt'ları için **native inheritance YOK**: `extends`/`base`/`include`/template alanı yok. Prompt body tamamen self-contained. Tek "paylaşım" = copy-paste.
- `model: inherit` **sadece model** alanına etki eder (prompt/tool'a değil).
- Native "shared preamble inject" mekanizması = **`skills:` frontmatter alanı**: bir agent, listelediği skill'in TÜM içeriğini başlangıçta context'ine preload eder. Mixin'e en yakın native lever budur.
- Çözüm önceliği: **proje `.claude/agents/` > global `~/.claude/agents/` > plugin**. Aynı isim → proje override eder.
- `.claude/agents/` **alt-klasörleri recursive taranır** → backup/eski agent'ları bu klasörün İÇİNDE bırakma, yoksa emekli agent canlı roster'a sızar.
- Desteklenen frontmatter: `name, description, model, tools, disallowedTools, permissionMode, mcpServers, hooks, skills, memory, background, isolation, effort, color, initialPrompt, maxTurns`.

## 2. Mimari: base + mixin (kalıtım yerine kompozisyon)
| Katman | Ne | Nerede | Kural |
|---|---|---|---|
| **Base (davranış)** | jenerik agent'lar: `builder-opus`, `auditor-opus`, `crafter-sonnet`, `researcher-inherit` | **global** `~/.claude/agents/` | TÜM projeler paylaşır. **Proje-özel düzenleme YAPMA** — değişirsen diğer projeleri bozarsın |
| **Mixin (proje DNA'sı)** | `<proj>-context` **skill**: proje kuralları, bilgi-tabanı erişimi (NLM/docs), anahtar path'ler, çıktı ekonomisi | **proje** `.claude/skills/<proj>-context/SKILL.md` | Bir kez yazılır; tüm proje agent'larına yayılır (DRY) |
| **Thin agent** | sadece gerçekten proje-yargısı gereken roller; `skills: [<proj>-context]` + 2 satır rol | **proje** `.claude/agents/` | Jenerik rolü dupliküleme — onun için global base + context-skill yeter |

**Kilit fikir:** TEK paylaşılan base + N proje mixin'i = sıfır çapraz-kcontaminasyon. Kalıtım olsaydı her projeye base'i kopyalardın (drift). Kompozisyon: base global kalır, "alt-sınıf" = projenin context skill'i. Tüm proje-flavor'ı mixin'de toplanır, base'e asla sızmaz.

## 3. RIMA referans implementasyonu (örnek — birebir kopyalama, adapte et)
- `.claude/skills/rima-context/SKILL.md` yazıldı: ACTIVE RULES (Karpathy 4) + NLM erişim komutu + Unity-hata kuralı + god-node path'leri + çıktı ekonomisi.
- 6 proje agent'ı `skills: [rima-context]` ile bu mixin'i preload ediyor → orchestrator artık her dispatch'te boilerplate enjekte etmiyor.
- Stale drift (eski dosya referansları) aynı geçişte temizlendi.
- Global base agent'lara hiç dokunulmadı.

## 4. Senin için görev (araştır → uyarla → kaydet)
1. **Araştır/doğrula:** Yukarıdaki CC gerçeklerini kendi ortamında teyit et. Fable-tabanlı orkestrasyonun (sizde executor farklı) bu pattern'i değiştirip değiştirmediğini değerlendir. Global base agent listenizi (`~/.claude/agents/`) ve mevcut proje agent'larınızı çıkar.
2. **Tasarla:** `studio-context` skill'inin içeriğini kararlaştır (proje agent/skill prefix'i = **`studio`** → mixin `studio-context`, thin agent'lar `studio-<rol>` ör. `studio-design`) — LaurethStudio'nun DNA'sı: proje kuralları, bilgi-tabanı erişimi (sizin NLM notebook'unuz), anahtar path'ler, çıktı disiplini. RIMA'nın rima-context'ini şablon al, RIMA'ya özel her şeyi (Unity, NLM ID, god-node) KENDİ projenle değiştir.
3. **Roster kararı:** Hangi proje-özel thin agent gerçekten gerekli (gerçek proje-yargısı), hangisi global base + context-skill ile karşılanır → dupliküleri ele.
4. **Sınır kuralı:** Global base agent'ları ASLA proje-özel düzenleme. Tüm özelleştirme mixin skill'inde.
5. **Kalıcı kaydet:**
   - Proje memory'sine: bu mimari kararı + global/proje sınır kuralı + `studio-context` tasarımı.
   - NLM notebook'unuza (varsa) sync.
   - Opsiyonel: pattern'i tekrar-kullanılabilir kılmak için global `bootstrap-project` skill'ine "her yeni proje = global base + `<proj>-context` mixin + thin agent" convention'ını öner/gömme.

## 5. Çıktı (LaurethStudio'dan beklenen)
- `studio-context` skill taslağı (içerik + path).
- Thin agent roster önerisi (tut/ele tablosu, gerekçeli).
- Kaydedilen memory/NLM kayıtlarının özeti.
- Global base'e dokunulmadığının teyidi.
