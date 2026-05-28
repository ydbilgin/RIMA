ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

RESPOND INLINE — DO NOT WRITE TO FILE. Output your review directly in this transcript.

# Amaç
Sang Hendrix'in **Realtime Parallax Map Builder** (RPG Maker MZ plugin) tool'unu **multimodal** olarak incele. Twitter video + itch.io demo video'larını **izle/dinle**. Bu tool **görsel olarak ne hissettiriyor**, **hangi mekaniklerle bu juice'u yaratıyor**, ve **RIMA (Unity URP 2D Hades-style ARPG)** projesine **design perspektifinden** adapt edilebilir mi — kapsamlı **görsel/design-odaklı** review yaz.

# Kaynaklar
1. **X (Twitter) post:** https://x.com/sanghendrix96/status/2059176117769208034
   - Video VAR — izle, frame-by-frame mekanik tarif et
2. **itch.io plugin page:** https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
   - Gif'leri, demo video'ları, screenshot'ları incele
   - Comments + community feedback varsa not al

# RIMA context (hatırlaman gereken)
RIMA = Hades-style 2D ARPG roguelite, Unity URP 2D Renderer, HIGH TOP-DOWN 3/4 (~70-80°). 
- Mevcut parallax: `Assets/Scripts/Background/ParallaxLayer.cs` — origin-based parallax, pixel snap
- 6 BG katmanı (L0-L4 + L3 islands) — verdict factors L0=0.03 → L4=0.10
- 3-Kit BG architecture: A=floor / B=cliff face / C=parallax BG
- Walless v1 Hades Elysium LOCKED — floating arena + cliff edges
- 35° iso floor + URP 2D Lights + 4 corner braziers + central portal cyan

# Review hedef soruları (her birine inline cevap)

## Q1 — Görsel feel (200 word)
Video'yu izle, **player'ın hissettiği** şeyi tarif et. "Nefes alan dünya" mı, "sinematik" mi, "atmosferik" mi? Atmosfer + immersion yaratımındaki **kilit teknik** ne?

## Q2 — Mekanik breakdown (5-8 madde)
Video'da gördüğün **somut visual mekanikler** (her biri 1-2 cümle):
- Parallax factor / layer count
- Camera pan davranışı
- Particle / weather / fog / mist / leaf / dust
- Day-night cycle / time-of-day blending
- Light pulse / ambient flicker
- Foreground occluder (player önünde geçen overlay)
- Dynamic depth (perspective shift player movement ile)
- Reflections / water shimmer / etc.

## Q3 — Sang'ın asset üretim metodu (tahminin)
Video'dan çıkarımla:
- BG asset'leri hand-painted mi yoksa AI-gen mi (Midjourney/Stable Diffusion/Krita?)
- Layer separation manuel mi yoksa Photoshop layer extract mi?
- Particle texture custom mu yoksa hazır mı?
- Sang'ın pipeline'ı bizimkine (PixelLab + Codex `$imagegen`) ne kadar yakın?

## Q4 — RIMA Hades-style ile uyum
Sang'ın RPG Maker (klasik JRPG perspektif, 2/3 top-down, blocky tilemap) vs RIMA Hades-style 3/4 angle + iso floor + walless arena + Yarık cyan theme.
- **Hangi mekanikler doğrudan adapt edilebilir** (HIGH TOP-DOWN 3/4 view'da çalışır)?
- **Hangileri perspektif uyumsuzluğu nedeniyle çalışmaz** (örn klasik tilemap-based parallax bizim iso floor ile uyumsuz olabilir)?

## Q5 — RIMA için **3 design önerisi** (priority sıralı)
Mevcut RIMA atmosfer fakir mi? Sang'ın tool'undan **5 dakika içinde** kopyalayabileceğin bir mekanik var mı? Yoksa **uzun vadeli** stratejik bir feature mi?

Her öneri için:
```
### Öneri 1: [Mekanik adı]
- Visual etkisi (player'ın hissedeceği): [1-2 cümle]
- RIMA'ya adapt: [Hades-style 3/4 view'da nasıl çalışır]
- Mevcut sistemle çakışma: [eski ParallaxLayer.cs ile sürtüşme var mı]
- Priority: P0 / P1 / P2
- Effort (gün): düşük / orta / yüksek
```

## Q7 — UI/UX inspection (user dikkat çekti — kritik)
User Sang'ın **editor arayüzü**ne özellikle hayran kaldı ("ne kadar güzel arayüzle kolay kullanımla tasarlanmış"). Video'yu izlerken editor UI'a frame-by-frame bak:
- Panel layout + visual hierarchy
- Real-time preview davranışı (drag = anlık update?)
- Layer ordering / parallax slider / depth control UX
- Color picker / asset browser / asset insertion flow
- Hover state, selection feedback, undo/redo
- Onboarding (yeni user 30 saniyede neye dokunabilir)

**RIMA `Assets/Scripts/Editor/MapDesigner/` (Unified Map Designer, MinimalTilePainter v4, RimaWorldPainter)** ile karşılaştır:
- 3 somut UX pattern çıkar (Sang'ta var, RIMA'da yok veya zayıf)
- Hangi UX iyileştirmesi en yüksek ROI sağlar (effort vs designer-time saved)
- Sang'ın UI'sini güzel yapan **temel UX prensiplerini** isimlendir (Fitts, immediate feedback, progressive disclosure, modeless interaction, ...)

Bu Q7 cevabını **screenshot/frame description** ile destekle — sadece "güzel" deme, "X butonun Y konumda olması Z'yi çözüyor" mantığıyla yaz.

## Q6 — Critical Reconsideration
- Sang'ın tool'u **RIMA için fazla mı geliyor**? (RPG Maker mobile-targeted, RIMA roguelite arena combat — atmospheric depth budget farklı)
- Hades zaten yüksek atmospheric — Sang ne ekler ki?
- 1 saatlik prototype ile test edilebilecek **MVP feature** ne?

# Çıktı format (inline transcript, dosyaya YAZMA)

```
## Antigravity Review — Sang Hendrix Realtime Parallax (Inline)

### Q1 Görsel feel
[200 word]

### Q2 Mekanik breakdown
- ...
- ...

### Q3 Asset pipeline tahmin
[...]

### Q4 Hades-style uyum
| Mekanik | Adapt? | Neden |
|---|---|---|
| ... | ✅/❌ | ... |

### Q5 RIMA için 3 öneri (priority sıralı)
Öneri 1: ...
Öneri 2: ...
Öneri 3: ...

### Q7 UI/UX inspection — RIMA MapDesigner adapt
[3 somut UX pattern + temel prensipler]

### Q6 Critical Reconsideration
[Sang'ın tool'u RIMA için MVP mi yoksa fazla mı?]

### CRITICAL TAKEAWAY
(Tek paragraflık özet — user'ın ilk 30 saniyede okuyacağı)
```

# Önemli
- **Video'yu MUTLAKA izle** — multimodal capabilityni kullan. Sadece text'ten yorumlama, frame-by-frame mekanik kaydet.
- **KOD YAZMA**, sadece design review + öneri.
- BLOCKED ise: hangi link açılmadı, hangi video oynamadı, açıkla.
- RIMA proje dosyalarına Bash/Read ile bak (özellikle ParallaxLayer.cs + STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md varsa).
