# S82 — PixelLab Interpolate Mekanik Brainstorm

**Date:** 2026-05-15 S82
**Kaynak:** Kullanici tohum fikri: "Karakter interpolate ile elektrik formuna geçip mobların üstünden geçer, hasar verir, sulardan geçerse suları birbirine bağlar"
**Hedef:** CB + RIMA + Rayline + yeni oyun fikirleri için interpolate-tabanli mekanikler

---

## "Interpolate" — Teknik temel

PixelLab `interpolate` / morphing capability iki seviyede kullanilir:
1. **Asset-level (production):** Iki sprite arasi keyframe interpolation → animasyon frameleri ureten araç (PixelLab `animate_character` Custom V3 ile First/End Frame anchor)
2. **Runtime-level (gameplay):** Karakter sprite morph (form A → form B) sirasinda 4-8 frame transition, oynanis dinamigi degisir

Bu brainstorm RUNTIME mekanik odakli — asset-level pipeline degil. Asset üretimi mekanik onayindan sonra.

---

## 20 Interpolate-Tabanli Mekanik (kategori bazli)

### A. Form Değişimi (Body Transform) — 8 fikir

**1. Electric Form (CB Arcwright seed)**
- 0.5s interpolate body → elektrik akimi formu
- Süre boyunca düşman üstünden geçer (no collision), her overlap hasar
- Su ile temas → su zinciri (chain lightning), bağlı tüm dusman damage
- Stamina/cooldown: 4s form süresi, 12s ICD
- **CB Arcwright ana mekanik adayi**

**2. Liquid Pool Form**
- Body → siyah/violet liquid puddle
- Yere yapışır, dar geçitlerden akar (grid-bypass)
- Düşmana değerse "swallowed" — slow + drowning damage
- Reform sırasında 0.4s vulnerable
- **RIMA Class Signature adayi: Sump (yeni sınıf)**, ya da yan oyun "Slipway"

**3. Shadow Form**
- Karakter → 2D düz gölge silhouet
- Yerden gelmeyen saldirilara immune (sadece ground-AoE'ye vurulur)
- Duvardan/kapidan altinden geçebilir
- Light source görürse force-revert
- **Rayline mekanik adayi (stealth-tilt)**

**4. Stone/Crystal Form**
- Body → granit blok
- Tüm dmg %85 reduce, 0 hareket
- Süre dolunca yöne göre +50% counter dmg patlama
- 3s form, 15s ICD
- **RIMA Brawler ulti adayi** veya yan oyun "Stonewall Knight"

**5. Origami Fold**
- Body → kağıt kuş
- 1.5s uçar, takip edemediği dikey atlama
- Kuş halinde 1 hit alirsa eski form-da düşer
- **Rayline traversal adayi** veya yan "Foldworld"

**6. Heat Mirage Form**
- Body görselce ısı dalgası
- %60 sansla enemy attack miss
- 2.5s, 10s ICD
- **RIMA defensive utility**

**7. Beast Form (Mount Morph)**
- Body → mount-form (canine/lupine)
- Hız +80%, melee damage +40%, ranged dmg yok
- 4s, "consume" 1 stamina bar
- **RIMA Warblade ulti adayi**, veya yan oyun "Pack Avatar"

**8. Magnetic Form**
- Body → kutuplu çubuk
- Tüm metal-tier mob magnetic pull (4 tile radius)
- Pull edilenler 1s stagger
- Drops, gold, items de çekilir
- **CB Arcwright support mode**

### B. Trail/After-Image (Position Interpolate) — 5 fikir

**9. Time-Rewind Echo**
- Position interpolate son 1s'i geriye → eski pozisyona spawn
- Geride bıraktığı trail = hasar veren yol
- 1 charge/8s
- **RIMA Echo Resonance Tier 3 extension** (Karar #122'e zincir)

**10. Mirror Clone**
- Interpolate spawn → karakterin son 3s input'unu tekrarlayan klon
- Klon 3s yaşar, gerçek vuruşların %50 hasarini verir
- **Rayline support mekanik** veya yan oyun "Recurse"

**11. Phantom Stride**
- Dash sırasında body → yarisaydam afterimage chain (5 frame)
- Her afterimage düşmana değerse 10% bonus dmg
- Karar #140 dash adayi (PixelLab vs VFX vs Hibrit'e Hibrit cevabi)
- **RIMA Karar #140 ÇÖZÜM ADAYI**

**12. Echo Resonance (Karar #122 T1 extension)**
- Beat 3 commit sirasinda body interpolate → past position yarisaydam clone
- Clone aynanin gibi son 3 hit'i tekrarlar
- Karar #122 Tier 3 design boslugunu doldurur (Tier 2 ne olacakti?)
- **RIMA LOCK adayi**

**13. Slipstream**
- Hareket halinde ardarda interpolate frames → momentum trail
- Momentum dolunca next dash 2x range
- **RIMA Ranger ozelligi**

### C. Weapon/Object Morph — 4 fikir

**14. Phantom Weapon Morph**
- Tek weapon slot, talep üzerine interpolate: blade → spear → hammer
- Her form farkli reach + speed + dmg type
- **RIMA Ravager ozelligi** veya yan oyun "Shapeblade"

**15. Vine Grapple**
- Weapon → uzayabilen sarmak
- Anchor noktasına atılır, oynanan karaktere çekme veya çekilme
- Düşmana atılırsa yakına çek
- **RIMA Ranger T2 extension**

**16. Fungal Bloom Weapon**
- Weapon interpolate → mantar yumagi
- Yere atinca 2 saniyede spor bulutuna açilir
- Bulutta duranlar poison stack
- **RIMA Hexer T2 extension**

**17. Tar/Molasses Sphere**
- Weapon interpolate → siyah sıvı sphere
- Yere değdiğinde 3-tile yapışkan zemin
- Üstündeki dusman hız -70%, walking dmg over time
- **RIMA Summoner ozelligi**

### D. Environment Interpolate — 3 fikir

**18. Wall Phase**
- Karakter belirli walls'a interpolate → karşı tarafa geç
- Sadece "thin wall" tag'li tile'larda çalışır
- Karar #131 Wang tile metadata extension (wall.canPhase=true)
- **RIMA Shadowblade T2 ozelligi**

**19. Floor Liquify**
- Karakter yere değiştirici dokunur → tile interpolate liquid → düşmana zemin engeli
- Karar #135 patch overlay extension
- **Yan oyun "Tidemark" ana mekanik adayi**

**20. Tile Ripple**
- Yere yapilan ag-attack tile'lari sembolik dalga halinde geri-frame interpolate eder
- Görsel feedback only, no dmg
- Faz 1.5 polish

---

## RIMA için ÖNCELIK SıRASI (öneri)

| # | Mekanik | Class | Faz | Etki |
|---|---|---|---|---|
| 11 | Phantom Stride dash afterimage | Player baz | 1 | Karar #140 hibrit çözüm |
| 12 | Echo Resonance Tier 3 (past-clone aynası) | Cross-class | 1.5 | Karar #122 derinleştirici |
| 9 | Time-Rewind Echo | Yeni utility skill | 2 | Roguelite "save" mekanik |
| 1 | Electric Form (RIMA'da değil — CB) | — | — | CB'ye taşı |
| 4 | Stone Form | Brawler ulti | 2 | Mevcut sınıf yenileme |

**Faz 1 LOCK önerisi — Karar #141 adayı:**
- "Phantom Stride" dash afterimage hibrit (PixelLab body 6-frame + Unity SpriteSnapshot pool)
- Bu Karar #140 (dash anim seçimi) için final cevap olabilir

---

## CB Arcwright için — Elektrik Form'un genişletilmiş formu

CB ana mekaniği "Environmental Cascade Combat" (Karar #136 önerisi — RIMA pivot reddi sonrası CB'ye özel)

**Electric Form interpolate CB'de:**
- 3s elektrik form, ICD 10s
- Mob üstünden geç → her overlap %20 hp dmg
- Su tile'a girersek su tile'ları arası elektrik zinciri 4 tile'a kadar
- Metal yere değersek conductive boost (+50% chain range)
- **3-AI sentez "düşman → zemin → element tetik" mantığıyla mükemmel uyum**

CB ekstra interpolate fikirleri:
- **Capacitor Discharge:** Karakter belli dmg toplar (capacitor full), interpolate edip burst-form 2s, kapasitör tükenince eski form
- **Polarity Flip:** Interpolate ile +/- polarity arası geçiş, mob targeting yeniden hesaplanir
- **Voltage Cascade:** Birden çok mob aynı anda damage alirsa interpolate-trigger chain reaction

CB klasör notu: `F:\Antigravity Projeler\CircuitBreaker\` altinda `interpolate_mechanic_designs.md` yaz (sabah, kullanici onayindan sonra).

---

## Yeni Oyun Fikri Çekirdekleri (oyun_fikirleri/'e eklenmeye aday)

### Fikir A — "Slipway" (08_FORM_DEGISTIREN_AKSIYON klasörüne)
- Liquid Pool Form ana mekanik
- 2D top-down, küçük arena
- Karakter sürekli liquid <-> solid form arası geçiş (cooldown YOK, mana bar)
- Solid: melee dmg + collision
- Liquid: dar geçit, slow trail, vulnerable
- Pipe-puzzle + dungeon arena karışım
- Tek tür: form-management arcade

### Fikir B — "Foldworld" (Origami)
- Origami Fold form değişimi
- Karakter sürekli paper bird <-> 2D human geçiş
- Bird = traversal, human = combat
- Pixel art ama Paper Mario referansı
- 2D side-scroller + platform action
- Genre: form-shifting platformer

### Fikir C — "Recurse"
- Mirror Clone ana mekanik
- Player son N saniyenin input'unu repeat eden klonlar üretir
- Puzzle/roguelite zigzag
- Ana çelişki: klonların kontrolü kullanıcıda yok
- Genre: timing-puzzle action

### Fikir D — "Shapeblade"
- Phantom Weapon Morph mekanikten yola
- Tek karakter, tek weapon slot, ama on-demand morph
- Combat-tilt: timing weapon-swap optimal damage type pick
- Tek tür: weapon-form combat
- (Note: bu RIMA Ravager'a benzer ama daha derin — solo oyun)

### Fikir E — "Tidemark"
- Floor Liquify mekanik
- Map'in zemini interactive bir liquid katmandan oluşur
- Player saldırılarıyla zemin morphs (su pool, lava patch, ice slab)
- Enemies zemine göre davranış değiştirir
- Genre: environment-warped tactical action
- **Karar #135 Map Designer mevcut altyapısı tam olarak buna uyar — RIMA'dan portable engine**

---

## Studio (Genel Proje) için Pipeline Notu

Interpolate-based mekanikler **PixelLab'in en güçlü pazarlama noktası** olabilir:
- Form-change = 4-8 frame interpolation = PixelLab native sweet spot
- Pure pixel art studio = bütçesiz indie için ideal pipeline
- Stüdyo signature: "every game has at least one interpolate-based core mechanic"

Bu signature net olursa:
- Tüm yan oyun fikirleri form-değişim çekirdeğine sahip olabilir
- 22 oyun fikrinden hangileri uyuyor:
  - Slipway / Foldworld / Recurse / Shapeblade / Tidemark — yeni
  - Eski 22'den: "Form Değiştiren Aksiyon" (08 klasörü zaten var) — bu klasör buna aliyor
  - "Goldweave Core" — interpolate hint var ama tam değil, revize gerek

---

## Compound Growth Tezi — Oyun Yapıldıkça Pool Büyür mü?

**Cevap:** Katlanarak büyür. 5 mekanizma:

1. **Kombinatoryal patlama.** 20 atomik mekanik 400+ kombinasyon (örn: "Electric Form + Time-Rewind Echo" = geçmişe elektrik patlama bırak). Her shipping oyun 3-4 mekanik shipler → 6-12 yeni kombinasyon doğal görünür.

2. **Player feedback yaratıcılığı.** Slipway shipping → oyuncu "liquid form harika ama freeze-mid-flow olsa" der. Bu vacuum brainstorm'da çıkmaz, gerçek deneyimden gelir. Her oyun 5-10 yeni interpolate fikri pop'lar.

3. **Tooling olgunlaşması.** PixelLab Custom V3 → state graph chain → branching morph zaten var, yüzeysel kullanıyoruz. Oyun 1 = 4-frame. Oyun 2 = state graph chain. Oyun 3 = branching interpolate (player input mid-morph). Her oyun teknik tavan yükseltir.

4. **Genre cross-pollination.** Roguelite Slipway, puzzle Foldworld, action CB — her tür interpolate'i farklı kullanır. "Roguelite'ta hangi interpolate timing fun?" cevabı puzzle'a uyarlanır vb.

5. **Studio playbook tacit knowledge.** 3. oyundan sonra ekip biliyor: "300ms transition + 1.5x scale punch + audio thump = X mekanik gibi hissetiriyor". Sonraki oyun yarı sürede design.

### Matematik

```
Oyun 1: 5 mekanik test → 3 ship + 2 rezerv → 5 yeni keşif
Oyun 2: 7 yeni + 2 = 9 → 4 ship + 5 rezerv → 7 keşif
Oyun 3: 12 + 5 = 17 → 4 ship + 13 rezerv → 9 keşif
...
Oyun 5: rezerv ~30, total keşif ~35, deep-shipped 15-20.
```

5. oyundan sonra **mekanik bulma sorunu YOK, eleme sorunu var.**

### Strategic argüman

- Solo dev tek başına aynı pipeline yatırımı = compound returns
- Studio identity = "form-changer pixel art action" (Vlambeer-tier niş ama derin)
- Mekanik backlog büyür, polish + scope daralır → her oyun daha rafine

### Tek risk: brand monoculture

Her oyun interpolate-based = tipleştirme. Çözüm: 4 interpolate oyun + 1 palette-cleanser (puzzle/sim/narrative) = sağlıklı portföy. Nintendo Mario+AnimalCrossing pattern'i.

---

## Sonuç — Şimdi Karar Bekleyenler

1. **Karar #141 LOCK adayı:** Phantom Stride hibrit dash afterimage = Karar #140 cevabi. Eğer onaylanırsa Karar #140 → Phantom Stride re-label, asset prod: 1 body dash anim (PixelLab) + Unity SpriteSnapshot pool.
2. **Karar #142 LOCK adayı:** Echo Resonance Tier 3 (past-clone aynası, Karar #122 chain'inde Tier 3) — interpolate body to past position yarisaydam clone.
3. **Studio signature öneri:** "Every studio game has one interpolate-based core mechanic." Bu LOCK ederse 22 oyun fikri yeniden eleme.
4. **CB Arcwright Sezon 2 başlangıç:** Electric Form = ana mekanik. CB design doc'a interpolate sektörü eklenmeli.
5. **Yeni oyun fikri ekleme:** Slipway / Foldworld / Recurse / Shapeblade / Tidemark — `oyun_fikirleri/OYUN_FIKIRLERI/08_FORM_DEGISTIREN_AKSIYON/` altına 5 yeni pitch.

Sabah Codex twitter research tamamlandiginda bu liste ile birleştirilir → MASTER karar revize.
