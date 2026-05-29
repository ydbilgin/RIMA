ACTIVE RULES: (1) think before answering (2) concrete, RIMA-specific (3) flag current-state from evidence (4) UNSURE if unknown.

# GÖREV — RIMA oyun-akışı UI/UX "şurada şu olmalı" listesi (teknik/feasibility lens)
RIMA = 2D top-down ARPG roguelite (Hades-like). UI canon: "UI yok, sadece bilgi" — minimal, context-sensitive, Ashen Glyph (koyu çatlak taş + cyan #00FFCC), skill-tree YOK, combat'ta HP/Rage görünür. Demo: tek Warblade, 5 oda, 1 boss, Steam-wishlist hedefi. Mevcut durum: `STAGING/INTEGRATION_BACKLOG_S6.md`.

Üret: oyunun her anı için **"bu anda şu ekranda olmalı + şu feedback tetiklenmeli"** maddeleri (mevcut-durum + teknik-feasibility ile). Fazlar:
- Boot/menü/sınıf-seçimi/arena-giriş
- Kalıcı combat HUD (HP/Rage/4-skill-cooldown/oda/progress)
- Combat anlık (hit/crit/kill/dash/cast/hasar-alma/düşük-HP)
- Oda-clear → draft → seçim
- Map fragment → reveal → gate → geçiş
- Boss (intro/telegraph/health-bar/faz/ölüm)
- Death / Victory (CTA/restart)

Her madde: an | ne olmalı | UI/feedback elementi | mevcut durum (present/partial/missing, kanıt) | öncelik (must/should/nice) | tahmini efor. Teknik feasibility + mevcut sistemlere oturma açısından ekle.

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED — yukarıdaki madde listesi + en kritik 5 eksik UX-beat.
