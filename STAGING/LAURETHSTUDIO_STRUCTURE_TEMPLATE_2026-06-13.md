# LaurethStudio Yapı Şablonu — Memory / Index / Status Düzeni
**Kaynak:** RIMA'da 3+ ay evrimleşen düzen, 2026-06-13 envanteriyle denetlendi; tespit edilen mantıksızlıklar DÜZELTİLMİŞ hâliyle şablonlandı.
**Hedef:** Bu dosyayı LaurethStudio session'ındaki Fable'a ver; mevcut yapıyı (STUDIO_CONSTITUTION.md, 02_GAMES/, MEMORY/) bu şablona **map etsin, yıkmasın**. Çelişkide: STUDIO_CONSTITUTION + kullanıcıya sor.
**Karar (2026-06-13):** LaurethStudio'ya **AYRI NLM notebook** kurulur (RIMA notebook'u paylaşılmaz).

---

## 1. Katman haritası (tek bakışta)

| Katman | Dosya/Konum | Ne tutar | Kim okur/yazar |
|---|---|---|---|
| Yönlendirici stub | `CLAUDE.md` (proje kökü) | ~3 satır: "PROJECT_RULES + CURRENT_STATUS oku, başka okuma" | herkes (ucuz giriş) |
| Kural | `.claude/PROJECT_RULES.md` | TÜM çalışma kuralları (routing, dispatch formatı, yasaklar) | session başı 1 kez |
| Durum | `CURRENT_STATUS.md` | **TEK** lean RESUME bloğu (~50 satır) | session başı + kapanışta REPLACE |
| Paylaşımlı memory | `MEMORY/` + `MEMORY/INDEX.md` | ajanlar-arası proje gerçekleri | tüm ajanlar |
| Orchestrator memory | `~/.claude/projects/<proj>/memory/` + `MEMORY.md` | orchestrator çalışma kuralları (feedback/routing/referans) | sadece orchestrator |
| Canonical tasarım | `TASARIM/` (veya stüdyoda `00_RULES/`) → **NLM notebook** | kalıcı tasarım kararları | NLM-sync ile |
| Süreç | `STAGING/` üst = LIVE · `_process/<YYYY-MM>/` = geçici · `_archive/` = tarih | karar/spec vs dispatch artifact'leri | konvansiyonla |

## 2. Düzeltilen mantıksızlıklar → şablon kuralları
RIMA'da **kanıtlanan** her sorun, burada kurala çevrildi. LaurethStudio bunları gün-1'den uygular:

### K1 — Çift memory sahiplik kuralı (RIMA sorunu: 5 belgelenmiş repo↔user çakışması)
- `MEMORY/` (repo) = **proje gerçekleri** (sistem durumları, asset kanonu, pipeline kilitleri). Tüm ajanlar okur.
- user-memory = **orchestrator'ın çalışma tarzı** (routing, feedback, dispatch dersleri). Proje gerçeği BURAYA YAZILMAZ.
- Aynı bilgi iki yerde ASLA. Sınır belirsizse → repo MEMORY kazanır.

### K2 — Index zorunluluğu (RIMA sorunu: 130+ dosyanın ~100'ü orphan)
- Index satırı olmayan memory dosyası **yok hükmündedir**.
- Memory yazımı ATOMİK: dosya + index satırı aynı işlemde. `/lint` orphan bulursa: ya index'e ekle ya `_archive/`'a taşı — üçüncü seçenek yok.

### K3 — Süpersede zinciri (RIMA sorunu: "stale_pit" — süresi geçmiş point-in-time kurallar süresiz kalıyor)
- Yeni kural eskisini geçersizleştiriyorsa: eski dosyaya `superseded-by: <yeni-dosya>` satırı + index'ten düş + `_archive/`.
- Point-in-time gözlemler (tarihli "şu an böyle" notları) 30 günde bir `/lint` turunda gözden geçirilir.
- `revoked` işaretli dosya index'te KALAMAZ.

### K4 — NLM saflığı (RIMA sorunu: bayat operasyonel routing NLM'e sızdı — eski model adları, disabled profiller canonical'da duruyor)
- NLM notebook = **SADECE tasarım canonical** (TASARIM/ + kalıcı kararlar + lore/sistem).
- Operasyonel bilgi (profil sırası, model routing, dispatch mekaniği) NLM'e **SYNC EDİLMEZ** — tek kaynağı memory'dir.
- Drift hiyerarşisi İKİ AYRI eksende: **Tasarım** → NLM > memory > draft. **Operasyon** → memory tek otorite.

### K5 — `.claude/` hijyeni (RIMA sorunu: kökte ~30 bayat dispatch prompt'u + betik birikti)
- `.claude/` köküne SADECE: `PROJECT_RULES.md`, `settings*.json`, `agents/`, `commands/`, `skills/`, sync-state dosyaları.
- Dispatch task/prompt artifact'leri → `STAGING/_process/<YYYY-MM>/`. `.claude/` köküne süreç dosyası yazan dispatch = kural ihlali.

### K6 — One LIVE Version + lean STATUS (RIMA'da çalışan, korunacak)
- İterasyonlu dosyada sadece son LIVE sürüm ana dosyada; eskiler `_archive/`.
- CURRENT_STATUS: yeni RESUME bloğu yazılırken eski blok SİLİNİR (git'te zaten var). Prepend YASAK.

### K7 — Decision dosyası formatı (RIMA'da çalışan, korunacak)
- Büyük karar = tek dosya: `<KONU>_DECISION_<tarih>.md` → council görüşleri + KARAR + sonradan gelen güncellemeler AYNI dosyaya "🔄 GÜNCELLEME" bölümü olarak. Kullanıcı-override açıkça işaretlenir.

## 3. Dosya şablonları

### 3a. Memory dosyası (her iki memory için)
```markdown
---
name: <kebab-case-slug>
description: <tek satır — recall kararı bununla verilir>
metadata:
  type: user | feedback | project | reference
---
<gerçek; feedback/project ise **Why:** + **How to apply:** satırları. İlgili: [[diğer-slug]]>
```

### 3b. Index satırı (MEMORY.md / INDEX.md)
```markdown
- [Kısa Başlık](dosya.md) — tek-satır kanca (ne zaman lazım olur)
```

### 3c. CURRENT_STATUS RESUME bloğu
```markdown
# CURRENT_STATUS
## ⏯️ RESUME (<tarih> — <tek satır durum>)
**⚠️ MODEL/ROUTING:** <aktif düzen>
**✅ BİTENLER:** <kanıt yollarıyla, madde başına 1-2 satır>
**🚩 AÇIK İŞLER/RİSKLER:** <numaralı>
**⚠️ UNCOMMITTED:** <varsa>
---
*Önceki bloklar git history'de.*
```

### 3d. CLAUDE.md stub
```markdown
# <PROJE>
Session start: `.claude/PROJECT_RULES.md` + `CURRENT_STATUS.md` oku. Başka okuma.
Sub-agents: bu dosyayı yoksay, orchestrator bağlamı doğrudan verir.
```

## 4. LaurethStudio Fable'a uygulama talimatı
1. Mevcut yapıyı envanterle (kendi `00_RULES/`, `MEMORY/`, `CURRENT_STATUS.md` neler içeriyor) — **silme/taşıma yapmadan** önce bu şablonla diff çıkar.
2. Eksik katmanları kur (K1-K7 kurallarına göre): stub CLAUDE.md, PROJECT_RULES (stüdyo sürümü — RIMA'nınkinden uyarlarken Unity-özel kuralları at), lean STATUS, çift-memory sahiplik ayrımı.
3. Mevcut STUDIO_CONSTITUTION ile çakışan şablon kuralı varsa: **CONSTITUTION kazanır**, çelişkiyi kullanıcıya raporla.
4. Ayrı NLM notebook aç; K4 saflık kuralıyla sadece tasarım/karar içeriği sync'le.
5. İlk `/lint` turunu çalıştır: K2 (orphan) + K3 (stale/revoked) taraması — temiz başlangıç kanıtı üret.
6. Bitince kullanıcıya: kurulan yapı + CONSTITUTION-çelişki listesi + ilk lint raporu.

## 5. YENİ OYUN BAŞLATMA KİTİ (per-game sample — fresh klasör akışı)
Kullanıcının akışı: LaurethStudio çatı → her oyun `02_GAMES/<OyunAdı>/` (veya ayrı kök) **fresh klasör** → kit kopyalanır → oyun kendi düzenine sahip olur. LaurethStudio Fable bu kiti **bir kez** `00_RULES/_templates/new_game_kit/` altında hazırlasın; her yeni oyunda kopyala-yeniden-adlandır.

**Kit içeriği (boş iskelet):**
```
<OyunAdı>/
├── CLAUDE.md                      ← 3a'daki stub (proje adı değişir, başka değişiklik yok)
├── CURRENT_STATUS.md              ← 3c'deki boş RESUME bloğu
├── .claude/
│   ├── PROJECT_RULES.md           ← stüdyo PROJECT_RULES şablonundan kopya; oyuna-özel bölümler işaretli boş (motor/asset kuralları sonra dolar)
│   ├── agents/                    ← genel agent şablonları (rima-* DEĞİL; generic doc/qc/design — adaptation rapor Bölüm 2 UYARLA listesi)
│   └── commands/                  ← lint / phase-close / save-session / plan / commit (parametrik sürümler)
├── MEMORY/
│   └── INDEX.md                   ← boş index (K2: ilk memory dosyasıyla birlikte ilk satır)
├── STAGING/
│   ├── _process/                  ← K5: tüm dispatch artifact'leri buraya
│   └── _archive/
└── TASARIM/                       ← oyun kararları; oyunun KENDİ NLM notebook'una sync (K4 saflık kuralı)
```

**Kit kuralları:**
1. Her oyun **kendi NLM notebook'unu** açar (stüdyo notebook'u stüdyo-seviyesi kararlar için; oyun tasarımı oyununkine). Notebook ID'si oyunun PROJECT_RULES'una yazılır.
2. Global skill'ler (`/cx /ax_* /council /nlm-sync /bootstrap-project` vb.) zaten `~/.claude/skills/`'te — kit'e kopyalanmaz, otomatik her projede çalışır (dinamik kök). Kit'e sadece **proje-içi** agents/commands girer.
3. user-level memory klasörünü Claude Code otomatik açar (proje yolu bazlı) — kit'e dahil değil; K1 sahiplik kuralı gün-1'den geçerli.
4. Kit kopyalandıktan sonra ilk iş: `/bootstrap-project` çağır (varsa) veya Bölüm 4 adımlarını oyun ölçeğinde uygula + ilk `/lint`.
5. Stüdyo-evrensel varlıklar (Asset QA Gate v2, 2D Illusion Library, Wang pipeline) kit'e KOPYALANMAZ — stüdyo kökünden REFERANS verilir (One LIVE Version, K6).

---
*Referans (detay isteyen okur): `STAGING/LAURETHSTUDIO_ADAPTATION_REPORT_2026-06-13.md` (tam envanter + uyarlama listeleri) · `STAGING/LAURETHSTUDIO_PLAYBOOK_EXTRACTION_2026-06-13.md` (orkestra/kanıt disiplini playbook'u).*
