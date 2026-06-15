# PROMPT → LaurethStudio agent (graphify query-first kuralını bu projeye benimset)

> Bunu LaurethStudio session'ına yapıştır.

---

## Görev
Bir karar aldık (RIMA'da uygulandı, global CLAUDE.md'ye de işlendi): **graphify'ın asıl değeri sorgulanabilir AI hafızası, görsel değil.** Bunu BU projede (LaurethStudio) de benimset ve uygula.

## Prensip (HARD RULE)
Çok-dosya/mimari sorularda ("X nasıl çalışıyor / Y'yi ne çağırıyor / hangi dosyalar arada / hangi dosya neye bağlı") 20 dosya okumadan ÖNCE bu projenin graphify grafını sorgula: `graphify query "..."` / `explain <node>` / `path <A> <B>`. Bulk-read'den **~71× ucuz**. Graf tazeliği (`--update` / AST rebuild) = **0 token**; sorgu = ucuz token. Görsel için native flag (`--obsidian` / `--graphml`→Gephi), elle script değil.

## Bu projede yapman gerekenler
1. **Graf var mı / kur:** Bu projenin `graphify-out/graph.json`'u var mı? Yoksa `/graphify <kod-yolu> --no-viz` (AST-only, 0 token) ile kur. **Yolunu not et.**
2. **Harness'a yaz:** Bu projenin session-yüklenen kurallar dosyasına (`PROJECT_RULES.md` veya proje `CLAUDE.md`) graphify query-first HARD RULE'unu **bu projenin graf yoluyla** ekle (RIMA'daki gibi: prensip + spesifik graf yolu + dispatch brief satırı).
3. **Sub-agent + cx + ax'e yay:** sub-agent context'ine (varsa `<proj>-context` skill) + cx hard memory (`~/.codex/memories/<proj>_graphify_query_first.md`) + ax (`~/.gemini/GEMINI.md` — prensip zaten orada, projeye özgü değil) bu kuralı işle.
4. **Kullan:** bundan sonra mimari/bug sorularında önce grafı sorgula.

## Not (graf yolu proje-bazlı)
RIMA'nın grafı RIMA'ya özgü (`STAGING/_process/.../graphify-out/graph.json`). Sen RIMA'nınkini KULLANMA — bu projenin kendi grafını kur/bul. Prensip evrensel, yol projeye özel.

## Çıktı
1. Graf durumu (vardı / kuruldu + tam yol)
2. Hangi dosyalara kuralı işledin (liste)
3. **1 örnek `graphify query` çalıştır + sonucu göster** (kuralın işe yaradığını kanıtla)
