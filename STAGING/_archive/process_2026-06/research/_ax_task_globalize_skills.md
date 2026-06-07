# Task — RIMA proje-lokal skill'leri GLOBALE taşı (parametrize ederek)

## Amaç
`F:\Antigravity Projeler\2d roguelite\RIMA\.claude\commands\` altındaki 10 komutu ve global
`C:\Users\ydbil\.claude\commands\council.md`'yi BAŞKA PROJELERDE de kullanılabilir hale getir.
Hedef konum: `C:\Users\ydbil\.claude\commands\` (user-global).

## Kurallar
1. **RIMA'daki orijinalleri SİLME/DEĞİŞTİRME** (proje-lokal kopyalar aynen kalır; aynı isim çakışırsa
   proje-lokal kazanır — sorun değil).
2. Her dosyayı OKU, RIMA-hardcode'larını tespit et, globale şu stratejiyle kopyala:
   - Tam taşınabilirse (RIMA path/ID yoksa): aynen kopyala.
   - RIMA-hardcode varsa: **parametrize et** — dosyanın başına "## CONFIG (proje-başına düzenle)" bloğu:
     `NLM_NOTEBOOK_ID` (env `$env:NLM_NOTEBOOK_ID` ya da satır-içi değişken, default=RIMA id) ·
     repo path → "çalışılan proje kökü" ifadesi · `cx_dispatch.py` → "proje kökünde cx_dispatch.py varsa onu,
     yoksa global `cx` CLI'ı kullan" notu. Pattern örneği zaten var: `~/.claude/commands/nlm-sync-template.md`.
3. **council.md (global'de yaşıyor) İÇİNİ güncelle:** NLM ACCESS satırındaki notebook-ID'yi
   "NLM_NOTEBOOK_ID (bu proje için CLAUDE.md/PROJECT_RULES'tan al; RIMA default=30ddffa5-292f-4248-8e77-68074af901be)"
   biçimine çevir + cx adımına "cx_dispatch.py yoksa: `cx run <profil> exec ...` doğrudan" fallback satırı ekle.
   Davranışı RIMA'da DEĞİŞTİRMEMELİ (default'lar RIMA değerleri).
4. Dosya bazında karar tablosu üret (kopyalandı-aynen / parametrize-edildi / globale-taşınmadı+nedeni).
   Anlamsız olanları taşıma (örn. içerik tamamen RIMA-içi referanssa ve parametrizasyonu anlamsızsa — gerekçele).

## Dosyalar
RIMA/.claude/commands/: codex-task.md, commit.md, lint.md, nlm-sync.md, nlm.md, phase-close.md, plan.md,
playtest.md, promptforge.md, save-session.md + global council.md güncellemesi.

## Çıktı
`F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_ax_done_globalize.md` (MUTLAK path!): karar tablosu +
oluşturulan/güncellenen global dosya listesi.