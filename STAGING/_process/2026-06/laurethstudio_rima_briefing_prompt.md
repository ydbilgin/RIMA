# PROMPT → LaurethStudio agent (RIMA'yı tanı + mekanik bankandan öner)

> Bunu LaurethStudio session'ına yapıştır. `<NLM_ID>` yerine RIMA notebook ID'sini koy (= `.claude/nlm.local`; ham ID repo'ya yazılmaz).

---

## Görev
Başka bir projeyi (RIMA) öğren, sonra **kendi mekanik bankandan** RIMA'ya uygun öneriler getir. Önce ARAŞTIR, sonra ÖNER. Genel değil — RIMA'ya özgü, araştırmaya dayalı, gerekçeli.

## RIMA nedir (kısa)
2D top-down roguelite ARPG, Unity (URP 2D + Pixel Perfect + 2D Lights). Chibi pixel-art, 8-yön. Bitirme/sunum tezi (~20 Haziran): **"RIMA bir oyun değil, environment + ilk vertical slice"** — domain-specific reusable tooling. Yani oyun-içi seviye editörü (Build Mode), Director Mode (stat/spawn/telemetry), oda sistemi (IsoRoomBuilder) gibi araçlar ön planda; oyunun kendisi bir dikey kesit.

## Erişimin (araştırmak için)
1. **Tasarım canon = NLM notebook** (RIMA'nın TÜM tasarım kararları burada):
   ```
   uvx --from notebooklm-mcp-cli nlm notebook query <NLM_ID> "<soru>"
   ```
   Örnek sorular: "RIMA'da hangi sınıflar var, skill setleri ne?" · "combat + kaynak sistemleri nasıl?" · "roguelite loop / oda akışı?" · "Echo / dual-class mekaniği?" · "art direction / Act1 görsel canon?" · "demo'daki 9 sistem ne?"
2. **Anlık durum / kod** (erişebiliyorsan): repo `F:\Antigravity Projeler\2d roguelite\RIMA` → `CURRENT_STATUS.md` (anlık iş), `PROJECT_INDEX.md` (harita), `CODE_MAP.md` (god-node + kod yolları). NLM tazeliği son sync'e bağlı; en güncel durum CURRENT_STATUS'ta.

## Ne araştır
- Sınıflar/skiller + cross-class slot sistemi
- Combat + kaynak (stat, rage/sever vb.)
- Roguelite loop: oda gir → düşman → reward → kart (DraftManager) → sonraki oda
- Mevcut vs planlanan (demo scope; post-demo bespoke: Echo / Void-boss / inversion)
- **Tasarım kilitleri** (önerilerin on-brand olması için): chibi pixel-art · no-dark-fantasy · 8-yön · **modüler-tasarım felsefesi** (signature/boss/Echo bespoke kalır, prototipte modülerleştirme yok) · "environment" tezi

## Ne öner (kendi mekanik bankandan)
RIMA'nın pillarlarına ve MEVCUT BOŞLUKLARINA uygun mekanik/skill/sistem önerileri. Her öneri için:
- Hangi RIMA boşluğunu/pillar'ını besliyor
- Mevcut sisteme nasıl oturur (oda / skill / combat / editor)
- Neden ON-BRAND (tasarım kilitlerine uyuyor)
- Tercihen "environment / reusable tooling" tezini güçlendiren ya da vertical slice'ı zenginleştiren

Önce 3-5 net soruyla NLM'i sorgula, SONRA öner.

## Çıktı
1. RIMA durumu özeti (araştırmandan, ~10 satır)
2. Mekanik bankandan 5-8 öneri (yukarıdaki formatla)
3. RIMA ekibine 2-3 net soru (belirsiz kalan noktalar)
