ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç
Sang Hendrix'in **Realtime Parallax Map Builder** (RPG Maker MZ plugin) tool'unu incele. Twitter video + itch.io sayfa içeriği + varsa demo video'ları izle. Bu tool **ne yapıyor**, **hangi visual feel'ı yaratıyor**, ve **RIMA (Unity URP 2D ARPG)** projesine adapt edilebilir mi — varsa **hangi mekanikleri** Unity'de implement edebiliriz, **implementation strategy** ne olur — kapsamlı **kod-odaklı** review yaz.

# Kaynaklar
1. **X (Twitter) post:** https://x.com/sanghendrix96/status/2059176117769208034
   - Video içeriği var — izle, mekaniklerini tarif et
2. **itch.io plugin page:** https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
   - Plugin'in açıklaması, demo video'lar, feature list
   - Comments / community feedback varsa not al

# RIMA context (sen sub-agent olarak hatırlaman gereken)
RIMA = Hades-style 2D ARPG roguelite, Unity URP 2D Renderer + Pixel Perfect Camera. HIGH TOP-DOWN 3/4 view (~70-80°). Mevcut parallax sistemi:
- `Assets/Scripts/Background/ParallaxLayer.cs` — origin-based parallax, pixel snap, ExecuteAlways
- 6 BG katmanı L0-L4 + L3 islands (CURRENT_STATUS S106 line 242)
- Verdict factors: L0=0.03, L1=0.05, L2=0.08, L3=0.14, L4=0.10
- 3-Kit BG architecture LOCKED (A=floor / B=cliff face / C=parallax BG)
- Sahne ortho camera, 35° iso floor + URP 2D Lights

# Review hedef soruları (her birine net cevap)

## Q1 — Tool ne yapıyor?
Plugin'in **core feature**'ı (200 kelime özet). RPG Maker MZ runtime'da neyi enable ediyor?

## Q2 — Visual feel / juice / efekt detayı
Video'da gördüğün **kritik visual mekanikler**:
- Layer count + parallax factor pattern
- Camera-driven mi (player movement = camera pan = parallax) yoksa subject-driven mi (her layer ayrı animasyon)?
- Particle / weather / fog efekti var mı?
- Lighting / time-of-day / day-night cycle?
- Foreground occlusion (player önünde geçen leaf, mist, fog gibi)?
- Dynamic spawning (event-based layer add) var mı?

## Q3 — Teknik mimari (RPG Maker MZ JavaScript plugin)
- Layer composition nasıl yapılıyor (sprite layers, tilemap-based, mesh)?
- Realtime preview mantığı (editor inside-game vs external)
- Asset format (PNG layers, sprite atlases, particle texture)
- Performance budget (mobile mi target, ne tahmin ederim)

## Q4 — Unity URP 2D'ye adapt edilebilir mi?
Her core mekaniği için Unity karşılığı:
| Plugin feature | Unity karşılığı | RIMA için uygunluk |
|---|---|---|
| Layered BG | Sprite Renderer + sorting layers | ✅ mevcut |
| Realtime parallax | ParallaxLayer.cs | ✅ mevcut, geliştirilebilir |
| ... | ... | ... |

## Q5 — RIMA için **3 somut iyileştirme önerisi**
Mevcut `ParallaxLayer.cs` + `BG_LAYER_ARCHITECTURE_VERDICT.md`'e karşı:
1. Sang'ın hangi mekaniği RIMA'da yok ve **yüksek değer** katar?
2. Implementation cost (kaç gün, hangi dosyalar)?
3. Visual ROI (player'ın hissedeceği fark)?

Örnek format her öneri için:
```
### Öneri 1: [İsim]
- Sang'ta nasıl: [1 cümle]
- RIMA'ya adapt: [Unity yaklaşımı]
- Etkilenecek dosya: [path]
- Implementation cost: [düşük/orta/yüksek + tahmini gün]
- ROI: [player'a yansıyan fark]
```

## Q7 — UI/UX inspection (user dikkat çekti)
Sang'ın **editor/tool arayüzü** çok kolay kullanım + estetik olarak öne çıkıyor (user feedback). İncele:
- Editor panel layout (hangi controls grouped, hierarchy)
- Drag-drop / inline preview / live update davranışı
- Visual feedback (hover state, selection indicator, depth slider, vb.)
- Onboarding / discoverability (yeni user 30 saniyede neye dokunabilir)
- Shortcuts / context menu / keyboard ergonomics
- RIMA `Assets/Scripts/Editor/MapDesigner/` (Unified Map Designer, Minimal Tile Painter v4) için **adapt edilebilir UX pattern** var mı?
  - 3 somut örüntü çıkar: 1) layer ordering UX, 2) parallax factor adjust UX, 3) realtime preview UX
- Sang'ın UI **neden iyi** — temel UX prensipleri ne (Fitts's law, müdahale az, immediate feedback, undo/redo, ...)

## Q6 — Risk / pitfall
- RPG Maker'ın render pipeline'ı Pixi.js, Unity URP 2D'den farklı. **Hangi mekanikler 1:1 port edilemez**?
- Pixel Perfect Camera ile dynamic layer scaling çatışır mı?
- Sang'ın asset üretim yöntemi (Photoshop, AI?) RIMA'nın PixelLab pipeline'ı ile uyumlu mu?

# Output format
`STAGING/PARALLAX_REVIEW_CODEX.md` dosyasına yaz:

```
# Codex Verdict — Sang Hendrix Realtime Parallax Map Builder

## Q1 Core feature (200 word)
## Q2 Visual mekanikler (bullet list)
## Q3 Teknik mimari
## Q4 Unity adapt tablosu
## Q5 RIMA için 3 öneri (yapısal)
## Q6 Risk / pitfall
## Q7 UI/UX inspection + RIMA MapDesigner adapt

## Critical insight
(En değerli tek çıkarım — orchestrator'ın user'a göstermesi gereken)

## Implementation priority
| Öneri | Cost | ROI | Priority |
|---|---|---|---|
| 1 | ... | ... | P0/P1/P2 |
| 2 | ... | ... | ... |
| 3 | ... | ... | ... |
```

# Önemli
- **KOD YAZMA**, sadece review + öneri.
- Video link'lerini **mutlaka aç ve izle** — sadece itch.io text'inden yorumlama.
- RIMA mevcut parallax dosyalarını oku: `Assets/Scripts/Background/ParallaxLayer.cs`, `STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md` (varsa).
- Sang'ın yaklaşımı RPG Maker — bizim engine farklı, **1:1 port değil adapt** odakla.
- BLOCKED ise: hangi web/asset erişimi yok, açıkla.
