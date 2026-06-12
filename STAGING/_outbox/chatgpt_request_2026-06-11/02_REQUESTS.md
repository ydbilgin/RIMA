# RIMA — ÜRETİM İSTEKLERİ (öncelikli)

## 🔴 A — Animated Background v2 (KANON-DÜZELTİLMİŞ) [yüksek öncelik]
Önceki paketin (RIMA_AnimatedBackground) iyi bir baseline ama **void'i MAVİ yaptın → kanon MOR (#3A1A4A)** ve cyan fazla soluktu + jenerikti. v2'de düzelt:
- **3 parallax katman, 2048×1152** (önceki 1280×720 küçüktü), 16:9.
- **Void:** Deep Purple **#3A1A4A** → siyah (mavi DEĞİL).
- **Kimlik = RIFT + SHATTERED KEEP** (jenerik gece göğü değil): uzakta **parçalanmış kale silüetleri** (kırık kuleler, void'de süzülen hapishane molozu) = kahraman öğe. **Neon cyan #00FFCC rift** dramatik bir **boyutsal yırtık/fraktür** olarak (soluk dalga değil), ekranın ≤%15'i. Uzak warm-ember **#E89020** kıvılcımları (çok az).
- **FAR:** statik (void + kale silüetleri + rift yırtığı). OPAK.
- **MID:** 6-8 frame seamless loop — sürüklenen sis + **nabız atan cyan rift glow**. ŞEFFAF PNG.
- **FRONT:** 6-8 frame seamless loop — süzülen ember/toz. ŞEFFAF, düşük alpha.
- Teslim: katman başına ayrı PNG + sprite-strip + GIF önizleme. Low-contrast, ön-planı bozmaz.
- (Opsiyonel) Boss-oda varyantı: daha yoğun/agresif rift.

## 🔴 B — Sunum Mimari Diyagramı [yüksek — bitirme sunumu için]
RIMA'nın **çok-agent AI orkestrasyon pipeline'ını** gösteren temiz, sunum-kalite diyagram:
- **Human (System Architect)** → **Claude Sonnet (Orchestrator)** (karar/dispatch/QC)
- Orchestrator → **Codex/cx (Executor)** · **Gemini 3.1 Pro + 3.5 Flash + Opus (Council)** · **Reviewer (author≠reviewer)**
- Paylaşılan MCP: **NotebookLM (Design Knowledge Base)** · **Unity MCP** · **PixelLab MCP**
- Döngü oku: **Task file → Execute → Council review → Verify (tests) → Commit** (kalıcı bağlam = NotebookLM)
- Çıktı: **RIMA (oyun)**
- Stil: koyu tema (RIMA paleti — slate/cyan/ember), net etiketli, okunur. PNG + (mümkünse) düzenlenebilir SVG.

## 🟡 C — "Hedef Oda" Konsept Mockup'ı [orta — north-star]
Bitmiş bir Act1 odasının TEK konsept görseli (high top-down 3/4), bizim görsel hedefimiz olsun:
- Slate **#3A3D42** zemin (temiz merkez) · zemini saran **Visual Shell** (duvar kalıntısı + uçurum kenarı + devasa zincir) · **Rift yırtığından yayılan cyan #00FFCC çatlak** (anchored) · 1-2 warm-ember **#E89020** mangal (ışık havuzu) · kemikler (failed containers) · 1 odak landmark · void'e düşen karanlık uçurum.
- Kanon kurallarına uy (anchored detail, merkez temiz, ≤%15 decor). Sadece KONSEPT (import için değil, hedef referans).

## 🟢 D — (Opsiyonel) Bağımsız referans kod
Sunum için **self-contained** bir overlay: ekranda mevcut oda-adı + kontrol ipucu (F1-F7 oda geçişi, fade) gösteren basit HUD scripti. *(RIMA-entegre kod değil — sadece referans; entegrasyonu biz yaparız.)*

---
**Öncelik:** A + B önce (BG v2 + diyagram), sonra C, D opsiyonel. Tek ZIP'te dön.
