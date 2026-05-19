# Codex Task: Hades / Alabaster Dawn / Sea of Stars Floor Production Research

## Problem

RIMA Spawn_01 oda görsel testlerimiz **yanlış yöne gidiyor**. PixelLab REST API ile ürettiğimiz "painted floor base" assets aslında:
- 3D-looking stone PLATFORMS (raised cliff edges) — flat painted floor DEĞİL
- Self-contained islands — composeable tileable texture değil
- Hard rectangular border veya organic raised edge — ikisi de yanlış

Bizim hedef: **Hades / Alabaster Dawn / Sea of Stars tarzı painted top-down floor**:
- FLAT painted surface (no 3D platform illusion)
- Seamless tileable extension
- Variations (path, grass, dirt) overlaid on base
- Stamp objects (rocks, debris, props) on top
- Walls as separate vertical overlays at edges

User direktifi (S92 birden çok kez tekrarladı, orchestrator her seferinde drift etti):
> "ufak ufak birleşen ama mantıklı birleşen pattern gibi belli de değil kare kare de değil sanki doğal olarak birleşiyormuş gibi olacak. insanlar bunu çözmüştür buna bakmıştın zaten tekrar bak istiyorsan"
> 
> "base floor dediğin cool granite dümdüz değil gibi ya bunu net olarak alabaster dawn hades nasıl işliyo bunu tam araştırıp insanların örneklerini bulur musun biz böyle bi yere varamayacaz gibi"

## Görev: Araştır + Belge yaz

WebFetch / WebSearch ile şu kaynakları araştır + sentezle:

### 1. Hades (Supergiant Games) — public talks / GDC / devlog
- Floor composition technique (painted vs tile?)
- Floor textures: seamless/tileable mı, tile-based mı, painted full-rooms mı?
- Decoration placement: scatter algorithm? Manual?
- Walls: separate sprite overlays mı?
- Render: SpriteRenderer mı, Tilemap mı, custom?

Ara: "Hades art pipeline", "Hades environment art", "Supergiant Games GDC", "Hades level design", "Hades painted floor"

### 2. Alabaster Dawn (Pixel Pop Studios) — devlog / Twitter / tweet refs
- Floor multi-material composition (pink soil, gray path, grass)
- Edge transitions
- Asset production workflow
- Ara: "Alabaster Dawn art", "Alabaster Dawn tile", "Spirit of the Goddess", "@SpiritOTGoddess art pipeline"

### 3. Sea of Stars (Sabotage Studios)
- Floor production technique
- Multi-biome variations
- Painted vs tile-based?
- Ara: "Sea of Stars art pipeline", "Sabotage Studios Sea of Stars devlog"

### 4. Industry best practice — painted floor in top-down ARPG
- Aseprite tutorials for painted floor textures
- "Painterly tileable floor" tutorials
- Photoshop/Procreate painted floor pipeline
- Ara: "painted top-down floor tileable tutorial", "aseprite painted floor", "non-grid tileset 2D"

### 5. Synthesis — RIMA için actionable plan

Araştırmadan sonra şu sorulara somut cevap:

A. **Floor base sprite üretimi:** Seamless tileable texture (256x256 tile repeat hides seams) MI, big painted scene (792x688) MI, multiple painted patches (composite) MI? Hangisi en yakın Hades/Alabaster Dawn?

B. **3D platform illusion:** Bizim PixelLab generates "cliff edges + raised stone platforms" — neden? Hangi prompt veya endpoint platform değil flat floor üretir? "Top-down ortho" prompt yeterli mi?

C. **Wall composition:** Walls separate sprites mı (Hades/AlabasterDawn'da nasıl)? Tile-based mı? Vertical overlays mı?

D. **Stamps/Decoration placement:** Bridson Poisson scatter doğru mu? Hades manual placement mı kullanıyor? Hybrid?

E. **Tooling:** Unity'de bunu üretmek için Tilemap mı, SpriteRenderer batch mı, custom Mesh mı? Hangisi production ready?

F. **Asset production:** PixelLab REST API ile bu tekniği nasıl üretebiliriz? Hangi endpoint? Hangi prompt formula? Veya PixelLab uygun değil mi (alternatif gerek mi)?

## Output

`STAGING/HADES_FLOOR_RESEARCH.md` yaz, sections:

```markdown
# Hades / Alabaster Dawn Floor Production — Research Synthesis

## Source Survey
[Her game için: linkler, public talks, devlog quotes, key insights]

## Technique Breakdown
### Hades approach
[Tile-based? Painted? Hybrid? Concrete description.]

### Alabaster Dawn approach
[...]

### Sea of Stars approach
[...]

## Common Patterns (cross-game)
[Industry consensus on painted top-down floor production]

## Critical findings: Where RIMA went wrong
[Why our PixelLab Pro outputs are 3D platforms not flat floors. Prompt issues. Endpoint issues.]

## Actionable Plan for RIMA
### Floor base (single image vs tileable)
[Concrete recommendation + reason]

### Walls (separate sprites)
[Concrete recommendation]

### Decorations (stamp scatter)
[Concrete recommendation]

### PixelLab REST API prompts (revised)
[Working prompt examples that produce flat painted floors, NOT 3D platforms]

### Implementation order
[1, 2, 3 steps with what to test first]

## Risks / unknowns
[What's still unclear, what to validate]
```

## Hard limits

- KOD YAZMA, sadece research + markdown sentez
- WebFetch + WebSearch kullan, gerçek kaynaklara git
- Tutorial / GDC video / devlog quotes referans göster (URL'ler dahil)
- Speculation YOK — sadece dokümante edilen public bilgi
- 30 dk effort max
- Sentez somut + actionable olsun, generic değil

## Why now

Spent: ~$3 USD generation (organic floor, decals, stones, pilot) — sonuç istenen Hades-look DEĞIL. Research olmadan daha çok harcama = boşa para. Bu research RIMA pipeline'ı düzeltir, kalan generations doğru üretir.

Subscription kalan: ~4280/5000. Animation jobs için ~4000 marj gerekecek, max ~280 generation budget bg art için.
