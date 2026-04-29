# PIXELLAB_COMBAT_VFX_RESEARCH
Guncelleme: 2026-04-22

## 1) Executive Verdict (Per Asset Type)
PixelLab combat VFX icin genel sonuc: **viable but partial**.

- Slash arc: **Partial**
- Hit spark / hit flash: **Partial**
- Projectile (bullet/spell/ki): **Partial-to-Yes**
- Explosion / burst: **Partial**
- Death burst: **Partial**

Ana neden: hizli ilk pass uretiyor, ama combat readability ve frame consistency icin siklikla manuel cleanup gerekiyor.

## 2) Quick Answers (Q1-Q8)
### Q1) PixelLab VFX sprite uretebiliyor mu?
**Evet, parcali.** Attack/text animation ve related edit tools ile uretiliyor; direkt "hit spark/slash pack" dokumantasyonu sinirli.

### Q2) Hit effect nasil yapiliyor?
**Hibrit.** PixelLab ile ilk sequence + Aseprite/manuel cleanup.

### Q3) Tam combat showcase var mi?
**Var ama daginik.** Official attack demosu var; community tarafinda combat oyunlarinda kullanim var.

### Q4) VFX prompt stratejisi ne?
**Kisa + tek niyet + segment zinciri.** Son frame'i yeni reference yapma paterni tekrar ediyor.

### Q5) Projectile nasil uretiliyor?
**Animate with text + interpolate/edit animation** kombinasyonu.

### Q6) Death animation pratigi?
**Genelde 8-16 frame araligi + sonradan cleanup.** Dogrudan PixelLab death-VFX showcase az.

### Q7) Yaygin hatalar?
- Buyuk canvas secip frame dusurmek
- Tek promptta fazla niyet
- Son frame cleanup yapmadan zincire devam
- Direction/silhouette drift

### Q8) PixelLab vs hand-drawn consensus?
**Consensus: PixelLab hizlandirici, hand-drawn/Aseprite final kaliteyi sabitliyor.**

## 3) Evidence Table (Claude Review Ready)
| Source | Claim | Evidence | Confidence |
|---|---|---|---|
| https://www.pixellab.ai/docs/tools/animate-with-text-pro | Animate-with-text action + direction + frame trade-off var | Docs: action/view/direction optionlari ve size->frame tablosu (32/64 -> 16 frame; 65-128 -> 4 frame) | High |
| https://www.pixellab.ai/docs/tools/interpolation | Iki keyframe arasi ara frame uretiliyor | Docs: "generates intermediate frames between two keyframes" | High |
| https://www.pixellab.ai/docs/tools/edit-image-pro | Tek frame duzenleme, animasyon edit baglantisi var | Docs: single image per run; related: Edit animation (Pro) | High |
| https://www.youtube.com/watch?v=XdgK1KeN-3s | VFX icin kucuk canvas/frame avantaji anlatiliyor | Transcript: VFX/icons kucuk boyutta daha cok frame; buyukte frame dusuyor | High |
| https://www.youtube.com/watch?v=zghUW8fGqsM | Segment chaining (last frame -> new reference) kullaniliyor | Transcript: son frame reference alinarak yeni hareket devam ettiriliyor | High |
| https://www.youtube.com/watch?v=lf49lGl-2Kk | Attack animation odakli resmi tutorial mevcut | Video title/description attack animation workflow | Medium |
| https://www.youtube.com/watch?v=8GuAMFoIFBs | PixelLab attack animation showcase var | Video title/description attack showcase | Medium |
| https://growlerygames.itch.io/risk-and-riches/devlog/956226/postmortem-risk-riches | Indie oyun pipeline'inda PixelLab + Aseprite birlikte kullanilmis | Postmortem: Godot + Asesprite + PixelLab extension | High |
| https://voxelate.itch.io/grim-rejoiner/devlog/697388/a-postmortem-for-a-morbid-game | PixelLab tek basina degil, workflow araci olarak kullaniliyor | Postmortem: "not a replacement", "part of my workflow", outputs supervised | High |
| https://www.reddit.com/r/aigamedev/comments/1rtnrdx/built_a_dbz_battle_royale_game_using_claude_code/ | Combat context'te Pixellab attacks/block/ki charging icin kullanilmis, consistency zorlanmis | Post metni: 4-direction attacks/blocking/ki + consistency hardest part | Medium |

## 4) VFX Type Matrix
| Asset Type | Recommended Tooling | Typical Canvas | Typical Frames | Difficulty |
|---|---|---:|---:|---|
| Slash Arc | Animate with text + Edit animation | 64x64 or 128x128 | 4-16 | Medium-High |
| Hit Spark/Flash | Animate with text + Edit image | 32x32 to 64x64 | 4-16 | Medium |
| Projectile | Animate with text + Interpolation + Edit animation | 32x32 to 64x64 | 8-16 | Medium |
| Explosion/Burst | Animate with text + Interpolation | 64x64 to 128x128 | 8-16 | Medium-High |
| Death Burst | Animate with text + Edit animation + Aseprite cleanup | 64x64 to 128x128 | 8-16 | High |

## 5) Prompt Patterns That Recur
- Tek niyetli action: `attack`, `magic attack`, `fire blast`
- Direction explicitly set
- Zincirleme uretim: `last frame -> next reference`
- Kucuk canvas ile frame yogunlugu artirma

## 6) Community/Showcase Links
### Official
- https://www.youtube.com/watch?v=8GuAMFoIFBs
- https://www.youtube.com/watch?v=lf49lGl-2Kk
- https://www.youtube.com/watch?v=XdgK1KeN-3s
- https://www.youtube.com/watch?v=zghUW8fGqsM
- https://www.youtube.com/watch?v=LQS4J4ub8G4

### Indie/Community
- https://growlerygames.itch.io/risk-and-riches/devlog/956226/postmortem-risk-riches
- https://granatalabs.itch.io/sticks-and-stones
- https://voxelate.itch.io/grim-rejoiner/devlog/697388/a-postmortem-for-a-morbid-game
- https://www.reddit.com/r/aigamedev/comments/1rtnrdx/built_a_dbz_battle_royale_game_using_claude_code/

## 7) Known Gaps / Limits
- PixelLab Discord icindeki showcase/tips kanallari public crawl ile toplanamadi.
- Bazi YouTube videolarinda subtitle yoktu; title/description-level evidence ile sinirli kalindi.
- Hit spark/death burst icin PixelLab-specific deep tutorial kaniti az.

## 8) RIMA Pipeline Implications
1. Character attack motion: mevcut locked pipeline ile PixelLab'de kal.
2. Combat VFX: PixelLab first pass + Aseprite final cleanup standardlastir.
3. VFX boyut standardi ayir:
   - Character attacks: 128x128
   - Kucuk VFX: 32x32 / 64x64 (frame verimi icin)
4. Her VFX icin zorunlu QC:
   - impact frame readability
   - silhouette stability
   - frame-to-frame pumping/artifact kontrolu

## 9) Claude Review Checklist
- [ ] Q1-Q8 cevaplari evidence table ile tutarli mi?
- [ ] Her iddia icin en az bir kaynak var mi?
- [ ] Confidence etiketleri makul mu?
- [ ] RIMA implications uygulanabilir ve scope-uyumlu mu?
