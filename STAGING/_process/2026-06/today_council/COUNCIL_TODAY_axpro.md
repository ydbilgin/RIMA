# COUNCIL — Bugünün Planı (ax Pro lens: STRATEJİ + ANIM/PIXELLAB DEĞER-YARGISI)

ACTIVE RULES: (1) think before answering (2) min (3) READ-ONLY analiz (4) BLOCKED if unclear.
⛔ READ-ONLY: dosya/Unity/git mutasyonu YOK. Unity'ye DOKUNMA (başka ajan Build-fix sürüyor). Tek RESP yaz.

## Bağlam
RIMA bitirme demosu **19 Haz = ~1.5 efektif gün**, hocaya canlı sunum. Tez: **tooling/architecture showcase** (%20 oyun / %60 mimari / %20 graphify) — animasyon-showcase DEĞİL, AMA görsel "öğrenci işi" damgası yememeli.
**Bugün done:** GATE full-flow Director/F2, Boss P0, reward-bleed, HUD readability; Build Mode functional fix koşuyor.
**Anim durumu:** Weaponless pivot — **4/4 silahsız char state ÜRETİLDİ** (idle/run/strike-windup/flinch, PixelLab). animate-handoff hazır (`STAGING/WARBLADE_WEAPONLESS_ANIM_HANDOFF_2026-06-16.md`, V3). Mount infra kodda. Mob'lar görsel "siyah-blob" (okunabilirlik sorunu).
**KISIT (kullanıcı):** **PixelLab kredisi BUGÜN sıfırlanmalı** (use-it-or-lose-it) → kredi demoya en faydalı şekilde harcanmalı. PixelLab ref: `~/.claude/PIXELLAB_REFERENCE.md` (V3 ucuz/kullanılır, PRO pahalı).

## SENİN GÖREVİN: bugünün optimal STRATEJİSİ + anim/PixelLab değer-yargısı
1. **Anim yatırımı cost/benefit:** Char weaponless 4/4 hazır — bunları **animate edip Unity'ye wire** etmek bugün öncelik mi? **Mob animasyonları yapmalı mıyız** — tooling-showcase tezinde mob-anim demo-değeri ne (combat sahnesi ne kadar görünür)? Net cost/benefit.
2. **PixelLab kredisi → en yüksek demo-etki:** Krediyi bugün NEYE harca? Seçenekler: (a) char weaponless anim'leri animate, (b) mob sprite/anim (black-blob okunabilirlik), (c) **eksik prop sprite'ları** (barrel vb. — Build Mode hayalet-asset'i çözer + tooling-showcase'i zenginleştirir), (d) VFX/tile/decal. Demo-etkiye göre SIRALA + tahmini kredi.
3. **"Bugün" planı (tez-hizalı):** 1.5 günde tooling-showcase için bugünün en yüksek-değer iş-sırası ne? Director-skin (en yüksek vizyon) vs Build-Mode-tamam vs anim/sprite — nasıl dengelersin?
4. **Görsel risk:** demo'da hocayı en çok utandıracak görsel açık (mob-blob? eksik-anim? Director-chrome?) — öncelik.

## ÇIKTI (E1: dönüş ≤10 satır)
RESP → `STAGING/_process/2026-06/today_council/RESP_TODAY_axpro.md`. Format: net yargılar + **"PixelLab kredisi-harcama önceliği (sıralı + gerekçe)"** + **"bugünün optimal iş-sırası"** + "mob-anim: YAP/YAPMA + neden". TAM Türkçe karakter. Dönüşte: RESP yolu + 8 satır.
