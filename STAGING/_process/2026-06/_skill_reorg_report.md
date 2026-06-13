# Skill Yerleşim Düzeltmesi Raporu — 2026-06-13

LaurethStudio denetiminden gelen iş. Yeni politika: proje-spesifik OLMAYAN skill/command → GLOBAL; sadece o klasöre gömülü olanlar lokal kalır.
HİÇBİR DOSYA SİLİNMEDİ — global'den taşınan/değiştirilen her dosyanın eski hâli önce `~/.claude/_archive/2026-06-13/` altına kopyalandı.

---

## Arşiv (taşıma/değişiklik öncesi orijinaller)
`~/.claude/_archive/2026-06-13/commands/`: agy_image, ask_gemini, codex-task, codex_image, commit, council, nlm-sync, nlm, p, promptforge (10 dosya)
`~/.claude/_archive/2026-06-13/skills/rima-conventions/` (tüm klasör: SKILL.md + references + scripts)

---

## GÖREV A1 — GLOBAL → RIMA İNDİR

Diff sonucu (global vs RIMA lokal): nlm, codex-task, promptforge → **DIFFERENT** (council RIMA'da YOK; save-session IDENTICAL).

| Dosya | İşlem | Sonuç |
|---|---|---|
| `nlm.md` | RIMA'da DIFFERENT → ikisi de korundu | global → `RIMA/.claude/commands/nlm.from_global.md`; global kopya kaldırıldı (RIMA lokal `nlm.md` + arşiv mevcut) |
| `promptforge.md` | RIMA'da DIFFERENT → ikisi de korundu | global → `promptforge.from_global.md`; global kopya kaldırıldı |
| `codex-task.md` | RIMA'da DIFFERENT → ikisi de korundu | global → `codex-task.from_global.md`; global kopya kaldırıldı |
| `council.md` | RIMA'da YOK → **modernize edilerek** indirildi | yeni `RIMA/.claude/commands/council.md`; global kaldırıldı |
| `rima-conventions/` (skill) | proje-spesifik → RIMA lokal | `RIMA/.claude/skills/rima-conventions/` (tam klasör); global'den kaldırıldı |

**council modernizasyonu:** settings.json model-swap + try/finally restore bloğu TAMAMEN kalktı → `ax dispatch --task-file <f> --model 'Gemini 3.1 Pro (High)' --print-timeout 480` (ve Flash için `'Gemini 3.5 Flash (High)'`). "ax SERIALIZATION / model-race" hard-rule'u "model per-dispatch, paralel mümkün" olarak güncellendi (ax_pro SKILL.md güncel pattern'i örnek alındı). cx dispatch bloğu değişmedi.

**Neden lokalde kalan eşadlı dosyalar (nlm/codex-task/promptforge) silinmedi:** içerikleri global'den DIFFERENT (RIMA lokali daha güncel/özelleşmiş) → görev kuralı gereği ikisi de korundu, global'inki `.from_global.md` olarak indirildi referans için.

---

## GÖREV A2 — GLOBAL'de KAL + PARAMETRİZE

| Dosya | Eski (örnek) | Yeni (örnek) |
|---|---|---|
| `nlm-sync.md` | `NB="${NLM_NOTEBOOK_ID:-30ddffa5-...}"` + REPO RIMA fallback | `NB="${NLM_NOTEBOOK_ID}"` / `REPO="${NLM_REPO}"` — **DEFAULT YOK**; env yoksa `ERROR: ... not set` + `exit 1`. (template'in env-var'lı içeriği temel alındı, REPO da zorunlu yapıldı). Eski RIMA-default'lu içerik → `RIMA/.claude/commands/nlm-sync.from_global.md` (RIMA lokal hash-based versiyonundan DIFFERENT → ikisi de korundu) |
| `commit.md` | `REPO="${NLM_REPO:-$(git rev-parse ... \|\| echo 'F:/...RIMA')}"` (3 yer + CONFIG satırı) | `REPO="${NLM_REPO:-$(git rev-parse --show-toplevel 2>/dev/null)}"; [ -z "$REPO" ] && { echo "ERROR: not a git repo and NLM_REPO unset."; exit 1; }` — hardcoded RIMA default'u silindi |
| `agy_image.md` | `$dir = 'F:\...\RIMA\STAGING\imagegen'` | CWD'den `.claude` arayan kök-bulma (PowerShell) → `$dir = Join-Path $p 'STAGING\imagegen'` |
| `codex_image.md` | `$dir = 'F:\...\RIMA\STAGING\imagegen'` | aynı kök-bulma pattern'i |
| `p.md` | settings.json swap + try/finally + `$j.model='Gemini 3.5 Flash (High)'` | swap bloğu kalktı → `ax dispatch --task-file $tf --model 'Gemini 3.5 Flash (High)' --print-timeout 180` |
| `ask_gemini.md` | settings.json swap + try/finally + `$j.model='Gemini 3.1 Pro (High)'` | swap bloğu kalktı → `ax dispatch --task-file $tf --model 'Gemini 3.1 Pro (High)' --print-timeout 300` |

---

## GÖREV B — RIMA lokalinden GLOBAL'e çıkarma değerlendirmesi

Test: "Başka projede 1 satır config ile çalışır mı?"

| Dosya | Generic mi? | Karar | Gerekçe |
|---|---|---|---|
| `lint.md` | HAYIR | **Lokal kalsın** | RIMA-özel knowledge-base sağlık taraması: MASTER_KARAR_BELGESI, FAZ_MASTER, 10-class roster, RageSystem, PixelLab pipeline — tamamen RIMA içeriğine bağlı. Parametrize edilemez. |
| `playtest.md` | HAYIR | **Lokal kalsın** | Gömülü `execute_code` blokları RIMA tiplerine bağlı (RuntimeRoomManager, DraftManager, RageSystem, RiftGlowVFX, DungeonGraph). Generic değil. |
| `phase-close.md` | KISMEN | **Lokal kalsın (global zaten kapsıyor)** | Global'de jenerik `phase-close.md` (`<PROJECT_NAME>` placeholder'lı) ZATEN var. RIMA lokali Unity-MCP test gate + CURRENT_STATUS/SYSTEM_MAP/FAZ_MASTER özelleşmesi içeriyor → RIMA-özelleşmesi olarak lokal kalsın; global template taşıma gerektirmez. |
| `plan.md` | KISMEN | **Lokal kalsın (global zaten kapsıyor)** | Global'de jenerik `plan.md` (`<PROJECT_NAME>`) ZATEN var. RIMA lokali Codex-delegation tablosu + RIMA-spesifik risk matrisi (MCP timeout, EditMode/PlayMode, Awake/Singleton) içeriyor → lokal RIMA-özelleşmesi. |
| `save-session.md` | EVET (tamamen generic) | **Global zaten kapsıyor — RIMA lokali GEREKSİZ** | RIMA lokali ile global IDENTICAL (diff: identical). Tamamen generic (proje-agnostik session formatı). Global'de zaten var. **Öneri:** RIMA lokal `save-session.md` kaldırılabilir (global'inki birebir aynı) — ama "surgical, sadece listelenen + GÖREV B tespitleri" + "HİÇBİR DOSYA SİLİNMEZ" kuralı gereği bu sefer DOKUNULMADI, sadece tespit olarak raporlanıyor. |
| `commit.md` (RIMA lokal) | EVET (parametrize sonrası) | **Global zaten kapsıyor (A2'de parametrize edildi)** | Global commit.md A2'de NLM_REPO/git-rev-parse ile generic yapıldı. RIMA lokalinde hâlâ hardcoded `F:/...RIMA` var → lokal RIMA-default'u olarak kalabilir; global generic versiyon tüm projeleri kapsıyor. |
| `nlm-sync.md` (RIMA lokal) | HAYIR (RIMA default'lu ama zengin) | **Lokal kalsın** | RIMA lokali hash-based + orphan cleanup + recursive tarama içeren GELİŞMİŞ versiyon; NB/REPO sabit-kodlu (RIMA). Global A2'de generic template ile değiştirildi. Lokal RIMA özelleşmesi haklı (ek özellikler). |
| `nlm.md` / `codex-task.md` / `promptforge.md` (RIMA lokal) | HAYIR | **Lokal kalsın** | RIMA notebook ID / namespace / yol hardcoded; A1'de global kopyaları kaldırıldı, lokal RIMA versiyonları korundu. |

**GÖREV B net çıktı:** Globale çıkarılacak YENİ aday YOK. Tüm generic adaylar (phase-close/plan/save-session) zaten global'de mevcut; RIMA lokalleri ya RIMA-özelleşmesi (haklı) ya da save-session örneğinde birebir kopya (tespit edildi, kurala uyup dokunulmadı).

---

## DOKUNULMAYANLAR + nedenleri
- `F:\LaurethStudio` — YASAK BÖLGE, dokunulmadı.
- `ax_flash/ax_pro/ax_opus` SKILL.md'leri — 2026-06-13'te zaten `--model`'e güncellenmiş, sadece pattern örneği olarak kullanıldı.
- `bootstrap-project.md`, `accounts.md` — generic, parametrize gerektirmez, listede yok.
- `nlm-sync-template.md` (global) — A2.6'nın kaynağı; template olarak yerinde bırakıldı.
- RIMA `save-session.md` — global ile IDENTICAL, "dosya silinmez + surgical" kuralı gereği dokunulmadı (yalnızca GÖREV B'de tespit olarak raporlandı).

---

## SMOKE-TEST sonuçları
- `ax` komutu PowerShell'de çözülüyor: `C:\Users\ydbil\AppData\Roaming\npm\ax.cmd`.
- `ax dispatch --help` → `--model`, `--task-file`, `--print-timeout` flag'lerinin **HEPSİ mevcut** (modernize edilen p/ask_gemini/council çağrıları geçerli). GERÇEK dispatch ATEŞLENMEDİ (kota korundu).
- `bash -n` sözdizimi: global `nlm-sync.md` OK · RIMA `council.md` OK · `commit.md` eklenen REPO satırı izole OK (bloktaki tek "hata" LLM-şablon placeholder `<dosya1>`, gerçek değil).
- PowerShell kök-bulma pattern'i RIMA kökünden çalıştırıldığında doğru çözdü: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\imagegen`.
- Yol doğrulama: RIMA `STAGING/` mevcut; `STAGING/_process/2026-06/` oluşturuldu.

---

## Nihai durum
- **Global commands (13):** accounts, agy_image*, ask_gemini*, bootstrap-project, codex_image*, commit*, nlm-sync*(generic), nlm-sync-template, p*, phase-close, plan, playtest, save-session. (* = bu görevde değişti)
- **Global skills:** rima-conventions KALDIRILDI (artık RIMA lokal).
- **RIMA commands:** lokal 10 + 4 `.from_global.md` referans (codex-task, nlm, promptforge, nlm-sync) + yeni `council.md`.
- **RIMA skills:** rima-conventions (yeni).
