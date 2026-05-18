# Karpathy CLAUDE.md Entegrasyon Planı — RIMA
Tarih: 2026-05-18
Hazırlayan: rima-sonnet sub-agent

---

## 1. Boşluk Analizi

### Prensip 1: Think Before Coding

Durum: ◐ KISMEN

Var olan kanıt:
- PROJECT_RULES.md satır 81: "Session Start: sadece 2 dosya oku" — kör okuma önleniyor.
- Sub-agent spawn eşiği (satır 125-128): "Sub-agent açmadan önce düşün" direktifi mevcut.
- Orchestrator'ın "Claude dispatches work, does NOT do mechanical bulk itself" rolü = düşünce/uygulama ayrımı var.

Boşluk:
- Codex spec dosyalarında varsayım listeleme yok. Örnek: PHASE_A_v15c spec doğrudan "Files to MODIFY" ile başlıyor — hangi varsayımlar altında çalıştığı belirtilmemiş.
- cx_dispatch.py'nin Codex'e gönderdiği prompt (line 224) sadece "Read task_file and execute every step" diyor. "Think before you code" direktifi yok.
- Muğlak kriter durumunda Codex'in sor/flag döndürmesi için mekanizma yok; susarak implement ediyor.

### Prensip 2: Simplicity First

Durum: ◐ KISMEN

Var olan kanıt:
- `/simplify` slash command var (PROJECT_RULES.md satır 97): "Review changed code after Codex commits" — post-hoc simplicity check.
- `EditorUtility.DisplayDialog` BAN (MEMORY) = gereksiz karmaşıklık engeli.
- Token Saving bölümü (satır 162-164): Bulk iş → Codex, analiz → Gemini. Araç seçiminde sadelik prensibi var.

Boşluk:
- Codex spec'leri bazen aşırı prescriptive: v15c spec 300 satır, implementasyon detaylarına kadar iniyor. "Min code" prensibi vs "max spec" gerilimi var.
- Sub-agent'lara "don't add speculative features" direktifi verilmiyor.
- Codex'in "nothing speculative" kuralına göre hareket etmesi için explicit talimat yok.

### Prensip 3: Surgical Changes

Durum: ✗ YOK (formal direktif olarak)

Var olan kanıt:
- Iteration Cleanup One LIVE Version Rule (satır 49-70): Eski versiyonları archive'a taşı = yan etki minimizasyonu. Ruhta benzer.
- "Mutual QC: Claude reviews Codex/Gemini commits. Errors → fix and re-commit" = hata yayılımını erken kes.
- "Touch only what you must" kuralının Codex'e hiçbir şekilde iletilmediği görülüyor.

Boşluk:
- Codex spec'lerinde "bu dosyaların dışına çıkma" listesi yok. v15c spec "Files to MODIFY" listesi var ama "bu liste dışındaki dosyalara dokunma" açık emri yok.
- Pre-existing dead code davranışı (not düş ama silme) hiç tanımlanmamış.
- Codex kendi oluşturmadığı import/using statement'larını siliyor olabilir — kural yok.

### Prensip 4: Goal-Driven Execution

Durum: ✓ VAR (en güçlü alan)

Var olan kanıt:
- PASS_FOR_ORCHESTRATOR_REVIEW ve PASS_WITH_TEST_BLOCKED_FOR_ORCHESTRATOR_REVIEW pattern: doğrulanabilir çıktı.
- Acceptance Criteria bölümü v15c spec satır 248-264: 10 madde, ölçülebilir (sayısal).
- Verification commands bölümü satır 284-295: sıralı doğrulama adımları.
- Self-iteration mekanizması: "Max 3 internal iterations" + başarı koşulları tanımlı.
- DONE marker format zorunlu: test sayısı, layer coverage stats, screenshot.

Boşluk:
- Pre-conditions (başlamadan önce gerekli durum) hiç belirtilmiyor. Spec direkt implementation'a giriyor.
- Out-of-scope listesi yok — Codex neyi YAPMAMALIYDI?
- BLOCKED/PARTIAL durumu için: Codex "FAIL" yerine sessizce partial implement edebiliyor.

---

## 2. Önerilen Değişiklikler (3 Dosya)

### A) `.claude/PROJECT_RULES.md` — Başa Eklenecek Blok

Mevcut "HARD RULES (Always Active)" başlığından ÖNCE şu blok eklenecek:

```
## Universal Coding Principles (Karpathy 4 — tüm agent'lar için)

1. KOD YAZMADAN ÖNCE DÜŞÜN. Varsayımlarını listele. Belirsizlik varsa flag at, körü körüne devam etme. Birden fazla yorum varsa hepsini sun.
2. MİNİMUM KOD. Problemi çözen en az kod. Spekülatif feature ekleme. "Senior engineer bunu overcomplicate görür mü?" testi uygula.
3. CERRAHİ DEĞİŞİKLİK. Sadece görevin gerektirdiği dosyalara dokun. İlgisiz kodu refactor etme. Pre-existing dead code'u not düş, silme. Sadece kendi değişikliğinin oluşturduğu unused import'ları kaldır.
4. HEDEF ODAKLI ÇALIŞMA. Her görevi doğrulanabilir başarı kriterine çevir. Multi-step işte plan + verification step. Muğlak kriter → orchestrator'a sor, tahmin etme.
```

### B) `cx_dispatch.py` — Prompt Inject Lokasyonu

İlgili kod bloğu (satır 209-210):

```python
wrapper = f"ALWAYS WRITE YOUR RESULT SUMMARY TO {done_file} AS THE VERY LAST STEP."
task_content = f"{wrapper}\n\n{task_content}\n\n---\n{wrapper}"
```

Önerilen değişiklik — `wrapper` satırından ÖNCE `surgical_header` değişkeni ekle, ardından `task_content` formatında başa inject et:

```python
surgical_header = (
    "CODING RULES (non-negotiable):\n"
    "(1) Think before coding: list your assumptions, flag ambiguities, never blindly implement.\n"
    "(2) Surgical changes: touch ONLY files listed in this task. Do not refactor unrelated code.\n"
    "(3) Min code: no speculative features. No extra abstractions.\n"
    "(4) Goal-driven: if success criteria are unclear, write BLOCKED reason to done file immediately.\n"
)
task_content = f"{surgical_header}\n---\n\n{wrapper}\n\n{task_content}\n\n---\n{wrapper}"
```

Exact edit lokasyonu: `cx_dispatch.py` satır 209 öncesi.

### C) `STAGING/CODEX_TASK_*.md` Template — Success Criteria Bölümü

Standart bölüm şablonu:

```markdown
## Success Criteria

### Pre-conditions (başlamadan kontrol et)
- [ ] Belirtilen tüm kaynak dosyalar mevcut (listele)
- [ ] Baseline test sayısı: N PASS (Codex start'ta doğrular)
- [ ] Unity Editor açık + console 0 error (eğer UnityMCP kullanılacaksa)

### Acceptance Tests (her madde ölçülebilir)
- [ ] (sayısal kriter — örn. "386+ EditMode PASS")
- [ ] (davranış kriteri — örn. "Layer 1 = %100 cell coverage")
- [ ] (çıktı kriteri — örn. "DONE marker dosyası yazıldı, X field içeriyor")

### Out-of-Scope (dokunma)
- Bu task: [SADECE ŞU DOSYALAR]. Başka dosyalara dokunma.
- Refactor etme: listelenmemiş hiçbir class/method'a dokunma, pre-existing dead code not düş.
- Speculative: bu spec'te olmayan feature ekleme.

### DONE Marker Format
Dosya: `STAGING/CODEX_TASK_{NAME}_DONE.md`
Zorunlu alanlar: STATUS (PASS/FAIL/BLOCKED/PARTIAL) | Files modified | Test delta | Verification output

### BLOCKED Durumu
Eğer pre-condition başarısız veya kriter muğlaksa: DONE marker'a `STATUS: BLOCKED — [sebep]` yaz ve dur. Tahmin ederek devam etme.
```

---

## 3. Sub-Agent Override Pattern

**Karar: Seçenek A** — Orchestrator her rima-sonnet/rima-doc spawn'ının başına 4 satır inline ekler. cx_dispatch.py zaten Codex'e inject ediyor (Section 2B). Sub-agent'lar için orchestrator pattern'ı manuel ama güvenli.

İnline format (her sub-agent prompt'unun ilk 4 satırı):
```
ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
```

---

## 4. Risk + Karşı Argüman

### Çakışma: Karpathy "Surgical Changes" vs RIMA "Iteration Cleanup"

Karpathy: "Pre-existing dead code'u NOT düş ama silme."
RIMA Iteration Cleanup: "Sadece son LIVE versiyon kalır, eski versiyonlar archive'a taşınır."

Bu bir çakışma değil — kapsam farklı:
- Karpathy surgical = kod değişikliği sırasında ilgisiz kodu düzenleme.
- RIMA cleanup = dosya/iterasyon yönetimi (v1→vN arşivleme). Bu Codex'in görev sırasında yaptığı değil, orchestrator'ın "phase sonrası" yaptığı iş.

### Ek Risk: Codex spec overcomplicated

Karpathy "Simplicity First" ile RIMA'nın 300 satırlık prescriptive spec'leri gerilim yaratabilir. Section 2C'deki template "success criteria" odağı ile bu gerilimi çözer.

---

## 5. Implementation Order

| Adım | Dosya | Kim | Effort | Risk |
|------|-------|-----|--------|------|
| 1 | `.claude/PROJECT_RULES.md` | Orchestrator | 5 dk | Sıfır |
| 2 | `cx_dispatch.py` | Codex dispatch | 10 dk | Düşük |
| 3 | `STAGING/CODEX_TASK_TEMPLATE.md` | Orchestrator | 10 dk | Sıfır |

Toplam önerilen eklenti: ~55 satır.
