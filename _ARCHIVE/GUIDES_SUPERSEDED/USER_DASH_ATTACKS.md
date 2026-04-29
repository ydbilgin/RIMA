# USER TASK — DASH ATTACK ANIMATIONS (10 CLASS)
> Güncelleme: 2026-04-16 | Sen yaparsın. Bitince Claude'a "[Class] dash attack hazır" de.
> **Toplam: 8-10 frame | 2-3 segment**
> **Tüm classlarda format aynı — sadece keyframe içerikleri değişir**

---

## TEMEL KARAR

**Neden dash attack, normal attacktan farklı:**
- Karakter zaten dashta momentum kazanmış → windup kısaltıldı (3f yerine 2-3f)
- Dash enerjisi strike'a dönüşüyor — anticipation çoktan yaşandı
- Bazı classlarda strike dash SIRASINDA oluyor (Shadowblade, Ranger, Ravager, Hexer) → PEAK LEAN'den başlar
- Diğerlerinde strike dash SONRASINDA oluyor (Warblade, Elementalist, Ronin, Gunslinger, Brawler, Summoner) → LANDING'den başlar

**Frame kararlari:**
```
Momentum-through (2 segment):  4+4 = 8f  → Shadowblade, Ravager, Hexer
Dynamic-strike  (3 segment):   3+4+3 = 10f → diğer tüm classlar
```

---

## WORKFLOW — Her Class İçin (3 Aşama)

### AŞAMA 1 — Keyframe Üretimi (Edit Image PRO, zincirleme)

**ÖNEMLİ:** PEAK keyframe ÖNCE üretilir (en kritik frame), WINDUP ikinci.

```
ADIM 1: Input frame → PEAK STRIKE keyframe üret   ← önce bu
ADIM 2: Input frame → WINDUP keyframe üret         ← sonra bu (PEAK'i gözönünde bulundur)
```

**Input frame seçimi (class bazlı):**
```
LANDING'den başlayanlar:  Warblade, Elementalist, Ronin, Gunslinger, Brawler, Summoner
PEAK LEAN'den başlayanlar: Shadowblade, Ranger, Ravager, Hexer
```

**Edit Image PRO ayarları (sabit tüm classlarda):**
- Output size: 128x128
- Remove background: ✓
- Input: ilgili classin dash frame'i (LANDING veya PEAK LEAN)

**Classin dash animasyonu henüz üretilmediyse:** BASE sprite'ı kullan ve
"character still carrying forward momentum" ibaresini prompt'a ekle.

---

### AŞAMA 2 — Interpolate (2-3 Segment)

**3 segment classlarda (10f):**
```
Segment 1: Input Frame → WINDUP     | 3 frame | momentum yüklenme
Segment 2: WINDUP → PEAK STRIKE     | 4 frame | darbe
Segment 3: PEAK STRIKE → Base Idle  | 3 frame | dönüş
```

**2 segment classlarda (8f):**
```
Segment 1: Input Frame → PEAK STRIKE | 4 frame | darbe (windup'sız)
Segment 2: PEAK STRIKE → Base Idle   | 4 frame | dönüş
```

---

### AŞAMA 3 — Aseprite Birleştirme

```
1. Yeni dosya: 128×128, 8 veya 10 frame
2. Seg1 → Seg2 (→ Seg3) sırasıyla ekle
3. Frame delay: 55ms tümü
4. Export → {class_name}_dash_attack_{yön}.png
```

---

## KAYIT YERLERİ

```
Keyframeler (geçici):
  Assets\Sprites\Characters\{Char}\dash_attack_frames\windup_{yön}.png
  Assets\Sprites\Characters\{Char}\dash_attack_frames\peak_{yön}.png

Animasyon çıktıları:
  Assets\Sprites\Characters\{Char}\animations\dash-attack\{yön}\frame_000.png
```

**Yönler:** S / N / W (East = West mirror, Unity'de flipX)

---
---

# 1. WARBLADE — "Momentum Slam" (Omuz Çarpması)

**Motion:** Dash iniş momentumu omuz darbesiyle hedefe aktarılıyor. Karakter durmak yerine omuzunu hedefe gömüyor. Knockback YOK — karakter hedefin içine itmek istiyor, onu uzaklaştırmak değil.
**Input:** Iron Surge LANDING frame
**Segment:** 3 segment | 10f

---

## PEAK STRIKE keyframe — Edit Image PRO (ÖNce)
*Input: Iron Surge LANDING frame*

**SHORT**
```
Armored warrior shoulder buried in target at full impact — body nearly horizontal behind planted shoulder, legs still driving forward, head tucked down.
```

**LONG**
```
Battle-worn armored warrior, 128x128 pixel art, low top-down isometric view, {facing direction}.
Maximum shoulder impact — body nearly horizontal, chest behind planted shoulder, full weight driving forward. Head tucked in, one shoulder planted at collision point. Both legs churning ground, pushing the impact forward rather than bouncing off. This is a DRIVE tackle, not a glancing blow. Cold blue rift cracks on armor flare with impact energy. Feet planted wide, maximum forward drive.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Iron Surge LANDING frame*

**SHORT**
```
Armored warrior dropping shoulder on landing, head tucking, weight loading for impact charge — still moving forward.
```

**LONG**
```
Battle-worn armored warrior, 128x128 pixel art, low top-down isometric view, {facing direction}.
Landing from charge, immediately dropping shoulder lower — head begins tucking, one shoulder rotating forward and down. Legs still carrying momentum. Body not yet fully committed — coiling to deliver full mass into target. Cold blue rift energy pulsing at leading shoulder edge. Anticipation frame — impact 0.3s away.
```

---

## Interpolate Prompts

**Segment 1 — Input (Landing) → WINDUP (3f)**
```
Armored warrior lands from charge and immediately drops shoulder lower, tucking head, loading weight into leading shoulder for impact. Continuous forward momentum.
```

**Segment 2 — WINDUP → PEAK STRIKE (4f)**
```
Armored warrior drives planted shoulder fully into target, body going nearly horizontal behind impact, both legs pushing through. Maximum forward compression on contact.
```

**Segment 3 — PEAK STRIKE → Base Idle (3f)**
```
Armored warrior rises from shoulder impact, feet planting, body straightening to combat stance. Target has reeled back from force.
```

---

## Consistency Warnings
- Omuz tarafı tutarlı kalmalı (hangi omuz?)  → sol omuz tercih et (East/West flip için)
- Armor cracks cold blue flare sadece impactte — windup'ta henüz yok, peak'te maksimum
- Kılıç pozisyonu: impact sırasında serbest elde — omuz saldırısı, kılıç kullanılmıyor

---
---

# 2. ELEMENTALİST — "Elemental Pulse" (Unsur Patlaması)

**Motion:** Blink'ten maddeleşir ve anında aktif elemente göre 2m AoE yayıyor. Her iki el öne/aşağı itilir — enerji zeminden dışa patlar. Fire = turuncu alev halkası. Frost = buz kristali yayılımı. Light = radiant halka.
**Input:** Blink LANDING frame (henüz tam maddeleşmemiş durum)
**Segment:** 3 segment | 10f

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Blink LANDING frame*

**SHORT (Fire state)**
```
Robed mage both arms slammed outward low, orange fire energy burst expanding from hands at ground level, knees bent from landing force.
```

**SHORT (Frost state)**
```
Robed mage both arms slammed outward low, ice crystal burst expanding from hands at ground level, cold blue shards radiating outward.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down isometric view, {facing direction}.
Both arms fully extended outward and slightly downward, palms facing ground. Knees bent deep from landing impact momentum. Elemental energy erupting from both hands and ground contact point — [FIRE: orange-amber flame ring expanding outward / FROST: ice crystal formation bursting outward, cold blue shards / LIGHT: radiant white-gold ring pulsing]. Maximum energy release frame. The burst is ground-level, radial, 2m radius. Body at center of eruption.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Blink LANDING frame*

**SHORT**
```
Robed mage just re-materialized, arms raising above head, elemental energy gathering at palms, knees still absorbing landing.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down isometric view, {facing direction}.
Arms raised to chest-shoulder height, palms facing forward, elemental energy visibly condensing at fingertips. Body still absorbing blink landing — slight crouch. This is the moment between materialization and release. Energy is GATHERING at hands, not yet released. Tense coil before burst.
```

---

## Interpolate Prompts

**Segment 1 — Input (Landing) → WINDUP (3f)**
```
Robed mage re-materializes from blink, body solidifying, arms rising to gather elemental energy at palms. Landing absorption still in legs.
```

**Segment 2 — WINDUP → PEAK STRIKE (4f)**
```
Robed mage arms slam downward and outward in explosive release, elemental energy bursting from hands and ground in radial AoE. Deep knee bend from landing force channeled into the burst.
```

**Segment 3 — PEAK STRIKE → Base Idle (3f)**
```
Robed mage hands lower, energy dissipating outward, body straightening from crouch back to combat stance.
```

---

## Consistency Warnings
- Blink Landing görselinden beslendiği için maddeleşme efekti Seg1'de hâlâ biraz görünür — doğru
- Fire/Frost/Light: farklı renkler ama AYNI pose. Aynı WINDUP + PEAK üret, rengi prompt'ta değiştir.
- Elemental orb/tattoos tutarlı kalmalı

---
---

# 3. SHADOWBLADE — "Seam Rend" (Geçiş Kesiği)

**Motion:** DASH SIRASINDA hedeften geçiş anında blade uzanıyor, ardından Rift Scar bırakarak çıkış yapıyor. Karakter hedefe girmeden önce kısmen görünür, hedeften geçerken yarı-transparan, çıkışta tam maddeleşiyor. Scar karakter çıktıktan sonra pulse yapıyor.
**Input:** Shadow Step PEAK LEAN frame (mid-dash — hareket içinde)
**Segment:** 2 segment | 8f (momentum-through, windup yok)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Shadow Step PEAK LEAN frame*

**SHORT**
```
Dual-blade assassin mid-pass-through target — body semi-transparent shadow form, one blade extended fully through target position, other arm trailing behind, void trail visible.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down isometric view, {facing direction}.
Full pass-through peak: body partially translucent — shadow smoke form, not fully solid. Leading arm fully extended with blade slicing through target position (target is not shown — blade cuts through space). Trailing arm pulled back for balance. Body in mid-through rotation — torso twisted, one shoulder ahead. Void purple smoke trail from blades and body silhouette. A faint glowing Rift Scar seam left at blade contact line — thin cold void light cut.
```

---

## Windup (RECOVERY) keyframe — Edit Image PRO (Sonra)
*Input: PEAK keyframe (zincirleme — tutarlılık için)*

**SHORT**
```
Dual-blade assassin exiting pass-through — body re-solidifying from shadow smoke, blades retracting, landing on opposite side from entry.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down isometric view, {facing direction}.
Exiting the pass-through: body re-solidifying from shadow form, more opaque than peak. Both blades retracting inward, arms coming to combat ready. Body fully clear of target position, landing on opposite side. Shadow smoke trailing behind, dissipating. The Rift Scar seam glows faintly in background — planted and waiting.
```

---

## Interpolate Prompts

**Segment 1 — Input (Peak Lean) → PEAK STRIKE (4f)**
```
Dual-blade assassin enters pass-through from dash — body dissolves into shadow form, one blade extends fully through target space, body semi-transparent at peak.
```

**Segment 2 — PEAK STRIKE → Recovery (Exit) (4f)**
```
Dual-blade assassin exits pass-through on opposite side — body re-solidifies from shadow form, blades retracting, Rift Scar seam left behind glowing faintly.
```

---

## Consistency Warnings
- Hangi blade öne uzanıyor tutarlı olmalı (dominant el)
- Transparan yoğunluk: Seg1 gittikçe azalır (entering), Seg2 gittikçe artar (exiting)
- Rift Scar (bırakılan iz) PEAK'te çok belirgin olmamalı — daha sonra aktive oluyor, şimdi sadece faint glow

---
---

# 4. RANGER — "Skirmish Draw" (Saldırgan Çekim)

**Motion:** Vault DASH SIRASINDA ok elde giriyor, düşük açılı delici ok atışı yapıyor ve hareket devam ediyor. Savunma değil saldırı — öne doğru kaçarken vur. Karakter dash yönünde ilerlerken belden baskı açısında ok serbest bırakıyor.
**Input:** Vault PEAK LEAN frame (mid-dash, atletik koşu pozu)
**Segment:** 3 segment | 10f

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Vault PEAK LEAN frame*

**SHORT**
```
Archer in dynamic running crouch — bow at hip/chest level pointing forward at low angle, arrow at full draw, about to release, still carrying dash momentum forward.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down isometric view, {facing direction}.
Still in running momentum: body in athletic forward crouch. Bow pulled to hip-chest level (NOT raised overhead — this is a snap shot from running). Arrow at full draw, tip pointing forward and slightly downward — low flat angle shot. Draw arm pulled back sharply. Body weight forward from dash, not stopped. Bow arm slightly bent inward — rushed but precise. Cold blue rift shimmer on arrow tip. This reads as OFFENSIVE — a hunter closing distance and firing.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Vault PEAK LEAN frame*

**SHORT**
```
Archer in running lean, bow arm starting to swing from back to side, hand reaching for arrow, still in full dash motion.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down isometric view, {facing direction}.
Running forward, bow beginning to swing from carry position at back to shooting position. Hand reaching for arrow at quiver or nocking. Body still in full dash lean — movement not interrupted. This is the 0.2s before the shot — bow arm just starting to extend. The movement is fluid: the archer is not stopping to shoot, she is shooting while moving.
```

---

## Interpolate Prompts

**Segment 1 — Input (Peak Lean) → WINDUP (3f)**
```
Archer still running from vault dash, bow arm swinging forward from carry position, reaching to nock arrow while maintaining full forward momentum.
```

**Segment 2 — WINDUP → PEAK STRIKE (4f)**
```
Archer snaps bow to shooting position at hip level, arrow at full draw in running crouch, explosive release while still in forward motion.
```

**Segment 3 — PEAK STRIKE → Landing (3f)**
```
Arrow released, bow arm follows through, archer continues forward momentum into vault landing — shot fired, movement uninterrupted.
```

---

## Consistency Warnings
- Ok daima yay üretilmeden önce üretilmemeli — WINDUP'ta yay henüz hazır değil, PEAK'te tam gerilmiş
- Dash direction ile ok direction aynı olmalı (forward-facing shot)
- Cold blue shimmer sadece arrow tip'te — yay veya elde değil

---
---

# 5. RAVAGER — "Fury Tackle" (Omuz Çarpışması)

**Motion:** Dash momentumu düşük omuz tackle'ına dönüşüyor. KNOCKBACK YOK — Ravager hedefe giriyor ve içinde kalıyor. Hedef yerinde tökezliyor. Ravager'ın pocket'ta kalması şart: çarpıp uzaklaşmak değil, çarpıp driving yapmak. Saldırı anında kan kırmızısı Fury enerjisi flare yapıyor.
**Input:** Ravager DASH PEAK LEAN (ağır forward charge) — henüz üretilmemişse BASE sprite
**Segment:** 2 segment | 8f (momentum-through)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Ravager DASH PEAK LEAN veya BASE*

**SHORT**
```
Massive shirtless berserker shoulder fully planted against target — body near-horizontal, both legs churning forward, head down, blood-red rage energy flaring at shoulder contact. Driving through, not bouncing off.
```

**LONG**
```
Massive shirtless berserker warrior, 128x128 pixel art, low top-down isometric view, {facing direction}.
Shoulder tackle at maximum impact — body nearly horizontal, dropped low. Head tucked chin-to-chest. Both legs still churning forward, DRIVING through contact point rather than stopping. The mass is enormous — this is not a glancing blow. Blood-red rage energy (#8B1A1A) surging at impact shoulder, tribal scarifications flaring red. Axe still in trailing hand — not used for this strike. Completely committed to driving through target.
```

---

## Recovery keyframe — Edit Image PRO (Sonra)
*Input: PEAK keyframe (zincirleme)*

**SHORT**
```
Massive berserker rising from tackle, both feet planted, body straightening, rage energy settling. Target left staggered behind.
```

**LONG**
```
Massive shirtless berserker, 128x128 pixel art, low top-down isometric view, {facing direction}.
Rising from tackle impact — body coming upright from horizontal. Feet planted wide, weight stabilizing. Axe hand swinging to ready position. Blood-red rage energy cooling from tackle flare but still smoldering at shoulder. Body posture: aggressive but settling, ready for next hit. Not celebratory — immediately ready to continue.
```

---

## Interpolate Prompts

**Segment 1 — Input (Dash Lean) → PEAK STRIKE (4f)**
```
Massive berserker drops shoulder lower from dash charge, head tucking, full body weight loading into shoulder drive. Blood-red rage energy building at leading shoulder. Driving into target contact.
```

**Segment 2 — PEAK STRIKE → Recovery (4f)**
```
Berserker rising from shoulder drive, feet planting, body straightening from horizontal. Rage energy fading from tackle flare, already eyeing next target.
```

---

## Consistency Warnings
- Axe el pozisyonu: tackle sırasında trailing el, asla impact sırasında öne gelmiyor
- Blood-red sadece shoulder contact'ta — vücut boyunca değil
- Base sprite üretilmemişse: "still carrying forward momentum, body low and leaning" ibaresini ekle

---
---

# 6. RONİN — "Iaido Blur" (Anında Çekiş)

**Motion:** Dash inişinin anında hand hilt'e iniyor, patlayıcı hız kın çekişi, blade tek slash yapıp geri kına dönüyor. Tension %30 boşaltılıyor. Hareketin hızı görsel kimliği: çekişi göremeyebilirsin — bitmeden önce bitiyor. WINDUP çok kısa (2f), DRAW+RECOVERY uzun (4+4f).
**Input:** Ronin DASH LANDING frame — henüz üretilmemişse BASE sprite
**Segment:** 3 segment | 2+4+4 = 10f (kısa windup, uzun draw + sheath)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Landing/BASE frame*

**SHORT**
```
Ronin katana at full draw extension — arm straight, blade fully extended to side, body in low crouched iaido stance, foot planted. Blade barely visible as cold silver-blue streak.
```

**LONG**
```
Wanderer samurai with katana, 128x128 pixel art, low top-down isometric view, {facing direction}.
Full iaido draw at peak — body in low combat crouch, one foot planted hard. Sword arm fully extended to the side at shoulder height, blade at maximum reach. The blade itself is a cold silver-blue streak — motion so fast the pixel art should suggest blur lines rather than clear blade shape. Other hand at hip, gripping sheath. Body weight on front foot, back foot lifting. This is the 1/60th of a second the blade is visible.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Landing/BASE frame*

**SHORT**
```
Ronin landing from dash, hand dropping to hilt grip, stance lowering, drawing breath for draw — still 0.1s before blade moves.
```

**LONG**
```
Wanderer samurai with katana, 128x128 pixel art, low top-down isometric view, {facing direction}.
Just landed from dash — hand dropping to hilt with intent, fingers wrapping the grip. Body lowering into draw stance: knees bent more than normal, back slightly angled, dominant shoulder rotating forward. The blade is still fully sheathed. Everything is coiled. This frame represents pure anticipation — the moment before an iaido master's draw. Cold silver gleam from sheath slot.
```

---

## SHEATH keyframe — Edit Image PRO (Recovery)
*Input: PEAK keyframe (zincirleme)*

**SHORT**
```
Ronin blade returning to sheath in smooth arc, arm retracting, body beginning to straighten back to stance.
```

**LONG**
```
Wanderer samurai with katana, 128x128 pixel art, low top-down isometric view, {facing direction}.
Blade mid-sheath — arm retracting from extended position, sword tip arcing back toward sheath opening. Body starting to rise from low crouch back toward combat stance. The re-sheathing is as deliberate as the draw — not sloppy. Cold silver gleam on blade as it returns. Tension release visible in body posture.
```

---

## Interpolate Prompts

**Segment 1 — Input (Landing) → WINDUP (2f)**
```
Ronin lands from dash, hand drops to hilt in single motion, body lowering into draw stance. 2 frames — very fast anticipation.
```

**Segment 2 — WINDUP → PEAK STRIKE (4f)**
```
Ronin explodes from draw stance — katana drawn at maximum speed, blade sweeping to full extension in silver-blue streak. The draw itself takes 4 frames but feels instantaneous.
```

**Segment 3 — PEAK STRIKE → Base Idle (4f)**
```
Katana retracts from extended position, blade arcing back to sheath in smooth deliberate arc, body rising from low stance back to combat ready. Re-sheath is calm and intentional.
```

---

## Consistency Warnings
- Blade PEAK'te belirsiz (streak) — çok net çizilirse hız hissi yok
- SHEATH ve WINDUP'ta blade tamamen kında — ortada visible olmamalı
- Tension 80+ crit bonusu görsel değişiklik gerektirmez (gameplay mekanik, animasyon sabiti)

---
---

# 7. GUNSLİNGER — "Crossfire Entry" (V-Çapraz Ateş)

**Motion:** Rift Dash slayt momentumu devam ederken iki silah kalça-omuz seviyesine çıkıyor, V-pattern ateş ediyor. Yakın mesafe: 2m içinde düşman varsa muzzle blast. Karakterin ellerinde cold silver muzzle flash. İki ayrı mermi birbirinden uzaklaşan açıda çıkıyor.
**Input:** Rift Dash PEAK SLIDE frame — henüz üretilmemişse BASE sprite
**Segment:** 3 segment | 3+5+2 = 10f (uzun peak — muzzle flash okunabilirliği için)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Slide/BASE frame*

**SHORT**
```
Gunslinger both pistols raised and firing simultaneously in V-pattern — one gun angled slightly left, other slightly right, cold silver muzzle flash from both barrels. Body still in slide momentum crouch.
```

**LONG**
```
Dual-pistol gunslinger in long duster coat, 128x128 pixel art, low top-down isometric view, {facing direction}.
Peak firing frame: both revolvers raised to chest-shoulder height, arms spread at slightly diverging angles forming a shallow V-pattern. BOTH barrels firing simultaneously — cold silver-white muzzle flash at both gun tips (not purple, not fire — cold metallic silver). Body still crouched in slide momentum, not fully stopped. The two gun angles mean projectiles diverge outward — classic crossfire pattern. Rune etchings on gun barrels barely visible, cold silver glint.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Slide/BASE frame*

**SHORT**
```
Gunslinger in slide crouch, both arms swinging upward from low position, guns rising to firing height, still in slide momentum.
```

**LONG**
```
Dual-pistol gunslinger, 128x128 pixel art, low top-down isometric view, {facing direction}.
Still in slide/dash momentum, body low. Both arms rising from carry position — guns beginning to lift from hip/thigh level toward chest. The motion is fast, practiced: a professional draw while in motion. Not yet at firing height. Body still leaning forward from slide. This is the moment between the dash and the shot — 0.2s before both guns come up.
```

---

## Interpolate Prompts

**Segment 1 — Input (Slide) → WINDUP (3f)**
```
Gunslinger in slide momentum brings both arms up from hip level, guns rising toward firing position while maintaining slide crouch and forward lean.
```

**Segment 2 — WINDUP → PEAK STRIKE (5f)**
```
Both revolvers reach shoulder height and fire simultaneously in V-pattern, cold silver muzzle flash from both barrels, arms at diverging angles for crossfire. Peak held slightly longer for visual clarity.
```

**Segment 3 — PEAK STRIKE → Base Idle (2f)**
```
Slide completed, guns lowering from firing position, body straightening to combat stance. Fast recovery — gunslinger already moving.
```

---

## Consistency Warnings
- Muzzle flash rengi: cold silver-white, ASLA ateş rengi (turuncu/sarı) değil
- İki elin açısı: slight V divergence — aynı yöne bakmamalı
- Duster coat momentum tutarlı olmalı (slayt sırasında coat arkada sürüklenir)

---
---

# 8. BRAWLER — "Flying Knee" (Hava Diz Darbesi)

**Motion:** Dash momentumu hava diz darbesine dönüşüyor. Arka ayak tepeden itmek için kullanılıyor, bir diz hedef seviyesine hızla çıkıyor. Karakterin silüetinde diz CENTRAL olmak zorunda — ilk bakışta "diz darbesi" okunabilmeli. Perfect timing (0.2s pencere) varsa: kısa iframes + Charge+2 bonus (animasyon farkı yok, gameplay fark).
**Input:** Brawler DASH PEAK LEAN — henüz üretilmemişse BASE sprite
**Segment:** 3 segment | 3+4+3 = 10f

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Dash Lean/BASE frame*

**SHORT**
```
Bare-chested fighter one knee fully driven forward and up — knee is highest point in silhouette, body leaning into it from slight height, void purple energy crackling at knee point.
```

**LONG**
```
Bare-chested athletic fighter with void purple energy hands, 128x128 pixel art, low top-down isometric view, {facing direction}.
Flying knee at peak impact: one knee fully raised and driven forward, knee is the visual center of the frame. Body slightly airborne — back foot just off ground from push. Forward lean driving body weight BEHIND the knee. Other leg dangling below. Arms spread slightly for balance. Void purple energy crackling around the knee at impact point — this is where the power channels. The knee is absolutely the dominant shape in this frame. Body lean and knee drive read instantly as "knee strike."
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Dash Lean/BASE frame*

**SHORT**
```
Fighter back foot pushing off ground hard, body rising, knee starting to lift — launch moment, still rising into knee drive.
```

**LONG**
```
Bare-chested fighter with void purple energy, 128x128 pixel art, low top-down isometric view, {facing direction}.
Launch moment — back foot still in contact with ground, pushing hard. Body beginning to rise. Driving knee just starting to lift from ground. Arms pumping for momentum. This is the 0.1s before becoming fully airborne. Forward lean from dash still carrying. Void purple energy beginning to gather at the rising knee. Body coiling upward.
```

---

## Interpolate Prompts

**Segment 1 — Input (Dash Lean) → WINDUP (3f)**
```
Fighter pushes off back foot hard from dash momentum, body rises, driving knee lifts from ground. Void energy gathering at knee.
```

**Segment 2 — WINDUP → PEAK STRIKE (4f)**
```
Fighter fully airborne, driving knee at maximum height and extension forward, body leaning behind it. Void purple energy flaring at knee contact point. Peak aerial knee drive.
```

**Segment 3 — PEAK STRIKE → Base Idle (3f)**
```
Fighter lands from knee strike, both feet planting on ground, body absorbing impact, returning to combat crouch. Void energy dissipating.
```

---

## Consistency Warnings
- Diz silüette baskın olmak ZORUNDA — eğer peak frame'de diz okunmuyorsa yeniden üret
- Void purple sadece knee impact noktasında — vücut boyunca değil (ellerde zaten var, ama knee özel)
- Hava kısmında back foot tamamen yerden kalkık — inişte her ikisi de yere iniyor

---
---

# 9. SUMMONER — "Spirit Surge" (Ruh Yönlendirme)

**Motion:** Dash inişinden sonra bir el öne uzanıyor, ruh enerjisi en yakın minyona (veya yoksa hedefe) bağlanıyor. Minyona retarget + 2s haste boost + mark veriyor. Karakter "komutan" pozu — yönlendiriyor, kendisi vurmaya gitmiyor. Minyon yoksa: %120 staff lunge (farklı animasyon değil, farklı efekt).
**Input:** Summoner DASH LANDING frame — henüz üretilmemişse BASE sprite
**Segment:** 3 segment | 3+5+2 = 10f (uzun peak — spirit beam okunabilirliği için)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Landing/BASE frame*

**SHORT**
```
Dark-robed summoner one arm fully extended forward, cold blue spirit energy beam emanating from hand toward nearest minion target. Other hand pointed at target as marker. Commanding pose — directing, not fighting.
```

**LONG**
```
Dark-robed necromantic summoner with bone-trimmed robes, 128x128 pixel art, low top-down isometric view, {facing direction}.
Peak spirit surge: dominant arm fully extended forward at chest height, palm open. Cold blue spirit energy stream visible from palm — a direct beam connecting to minion or target point (off-frame). The beam is narrow, precise, controlled. Other hand extended at different angle, finger pointing toward a SECOND point (the target being marked). Body is planted, stable. This is a commander position: one hand sends the order, other marks the objective. Staff at side. Not aggressive — controlled authority.
```

---

## WINDUP keyframe — Edit Image PRO (Sonra)
*Input: Landing/BASE frame*

**SHORT**
```
Summoner just landed from dash, body stabilizing, dominant arm beginning to rise, staff glowing cold blue in anticipation.
```

**LONG**
```
Dark-robed summoner, 128x128 pixel art, low top-down isometric view, {facing direction}.
Just landed from dash — feet planted, body centering. Dominant arm rising from landing position, palm beginning to open. Staff in other hand starting to glow cold blue. Face turned toward target direction with focused intent. The summoner is not rushing — even in a combat dash, the gestures are deliberate. This is the 0.2s of collecting intent before the spirit channel fires.
```

---

## Interpolate Prompts

**Segment 1 — Input (Landing) → WINDUP (3f)**
```
Summoner plants from dash landing, body stabilizing, dominant arm rising, staff beginning to glow. Deliberate control, not rushed.
```

**Segment 2 — WINDUP → PEAK STRIKE (5f)**
```
Spirit energy beam fires from extended palm toward minion, other hand pointing to mark the target. Full commanding pose held while beam connects. Cold blue stream visible.
```

**Segment 3 — PEAK STRIKE → Base Idle (2f)**
```
Arms lower, spirit beam dissipates, summoner returns to neutral commanding stance. The mark has been placed.
```

---

## Consistency Warnings
- Spirit beam YALNIZCA dominant elden — stafftan değil (staff pasif, elde tutuluyor)
- Minyon yoksa (staff lunge versiyonu): aynı animasyon, peak'te beam yerine staff thrust öne — AYNI WINDUP kullanılabilir
- Cold blue beam, Warblade'in cold blue'sundan ince ve yönlü olmalı (targeted beam, AoE değil)

---
---

# 10. HEXER — "Curse Step" (Lanet Adımı)

**Motion:** DASH SIRASINDA bir ayak kasıtlı olarak yükseğe kaldırılıyor, lanet noktasına GÜÇLÜ BASIYOR. Lanet enerjisi basım noktasından radial yayılıyor. Karakterin "kasıtlı olarak basması" normal dash'ten farklı — bu ayak bir silah olarak kullanılıyor. Pressure Phase (4+) hedefte: 0.5s root ekleniyor (görsel: ayak basımında zincir/hex sembolleri zeminde).
**Input:** Hexer DASH PEAK LEAN — henüz üretilmemişse BASE sprite
**Segment:** 2 segment | 4+4 = 8f (foot raise → stamp, hızlı)

---

## PEAK STRIKE keyframe — Edit Image PRO (Önce)
*Input: Dash Lean/BASE frame*

**SHORT**
```
Curse-robed mage stamping one foot down hard onto ground, cursed hex energy radiating outward from impact point in ring pattern, other foot still in air from stamp momentum, casting hand raised.
```

**LONG**
```
Dark crimson robed hexer with iron lantern, 128x128 pixel art, low top-down isometric view, {facing direction}.
One foot FULLY planted with force — this is a deliberate stamp, not a normal step. Body weight behind the stamp, slight forward lean. Other foot raised from the stamp momentum. Cursed green-purple energy erupting FROM THE STAMP POINT outward: visible hex rings, sigil patterns in pixel art radiating from foot contact. The cursed energy is ground-born — it comes from where the foot hits. One hand raised in casting gesture channeling the curse through the step. Iron lantern swings from motion. This should read as "a deliberate cursed stamp" — the foot is a weapon.
```

---

## ANTICIPATION keyframe — Edit Image PRO (Sonra)
*Input: Dash Lean/BASE frame*

**SHORT**
```
Hexer in dash lean with one foot raised high — deliberate, heavy raise, curse energy gathering at lifted foot sole. About to stamp.
```

**LONG**
```
Dark crimson robed hexer with iron lantern, 128x128 pixel art, low top-down isometric view, {facing direction}.
Still in dash motion but one foot dramatically raised — higher than a normal step, more deliberate. The raised foot sole shows gathering cursed energy (green-purple glow). Other foot carrying full weight. Body slightly crouched, leaning into the about-to-stamp direction. Casting hand rising. This reads as INTENTION: the hexer is choosing exactly where the curse will land.
```

---

## Interpolate Prompts

**Segment 1 — Input (Dash Lean) → PEAK STRIKE (4f)**
```
Hexer raises one foot dramatically during dash, curse energy gathering at raised foot sole, then STAMPS hard onto ground — cursed energy erupting outward from impact point in radial hex rings.
```

**Segment 2 — PEAK STRIKE → Base Idle (4f)**
```
Curse energy rings expanding outward from stamp point, hexer foot still planted, body straightening from stamp impact, returning to cautious combat stance. Hex rings dissipating.
```

---

## Consistency Warnings
- Hex energy ZEMİNDEN çıkıyor (stamp point) — elden veya stafftan değil
- Green-purple renk: hem yeşil hem mor — sadece biri değil ikisi
- Ayak stampi güçlü okunmalı — eğer peak frame'de normal adım gibi görünüyorsa daha extreme yap

---
---

# QC CHECKLIST

```
DASH ATTACK PASS (her class için):

  WINDUP:
  ✓ Momentum hissediliyor — karakter hareketten geliyor, durmuş değil
  ✓ İçerik okunabilir — bu WINDUP olan class strike hazırlanıyor
  ✓ Silah/el tutarlı BASE veya DASH LANDING'le

  PEAK:
  ✓ Tek bakışta ne strike olduğu okunuyor
  ✓ Silah/vücut tutarlı WINDUP'la (zincirleme başarılı)
  ✓ Enerji efekti doğru renge sahip (class'a özel)
  ✓ Bu class'ın normal LMB attack'ından SİLÜETÇE farklı

  RECOVERY:
  ✓ Idle'a dönüş doğal
  ✓ Silah pozisyonu idle'la uyuşuyor

FAIL çözümleri:
  Peak okunmuyor      → "X should be the MOST prominent shape in frame" güçlendir
  Enerji yanlış renk  → Prompt'ta renk hex kodu yaz: "#7BA7BC" gibi
  Zincirleme bozuk    → Peak'i, WINDUP'tan değil LANDING'den yeniden üret
  3 denemede çözülmedi → Claude'a ilet
```

---

## BİTİNCE

Her class ayrı bildirilebilir: "Warblade dash attack hazır" / "Brawler dash attack hazır" vs.

Claude şunları yapar:
1. Sprite import (PPU=64, Point, No compression)
2. .anim clip build (DashAttack state, loop=false)
3. PlayerAnimator'a DashAttack state + trigger ekle
4. PlayerController dash attack window ile bağla (0.4-0.5s post-dash LMB check)
