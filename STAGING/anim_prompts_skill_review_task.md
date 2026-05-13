# Codex Task — Animation Prompts Skill Design Review

## Görev
STAGING/pixellab_animation_prompts_10char.md dosyasını oku ve her promptu hem PixelLab teknik kısıtlarına hem de aşağıdaki LOCKED skill tasarım kararlarına göre review et. Sonuçları STAGING/anim_prompts_skill_review_result.md dosyasına yaz.

## PixelLab Constraints
- Frame range: 4–16 frames (kesin limit)
- Prompts anatomik ve spesifik olmalı
- Chibi 64x64, high top-down ~30-35°

## LOCKED LMB / RMB / Skill Tasarımı (per class)

### 01 — Warblade
- LMB Iron Combo: 3 vuruş zinciri — sweep → overhead → shoulder ram
- RMB Rage Outlet: Rage 30+ iken kısa AoE patlaması, Rage-30 (görsel: ayaklar yere basar, gövde/silah braced, yakın patlama)
- Key skills: Gravity Cleave (silahı yere çarpar, çeker), Earthsplitter (3m knockup, ground crack), Iron Charge (8m dash + stun)

### 02 — Elementalist
- LMB Rift Bolt: hızlı element mermisi, her 3. bolt empowered. Saldırılar elden/jestyle — NO STAFF
- RMB Element Switch: tap ile Fire↔Frost geçiş (gesture, saldırı değil)
- Key skills: Fireball, Glacial Spike (buz hattı), Blink (ışınlanma), Meteor

### 03 — Shadowblade
- LMB Veil Strike: reverse-grip slash, mark koyar, 3 vuruş → Hold = Twin Carve + phase-step
- RMB Veil Flicker: düşmandan kısa faz geçişi, Rift Scar bırakır
- Key skills: Phase Step, Death Mark, Veil Burst, Chain Cull

### 04 — Ranger
- LMB Rift Arrow: tap=hızlı ok, hold=şarj + mark. Compound bow, sol elde
- RMB Tactical Roll: hareket yönüne geri atlama + 1 ok anında ateş
- Key skills: Pinning Shot (root), Bone Trap (cursor zone), Sweep Volley (cone), Final Strike (mark+trap hedefe %400)

### 05 — Ravager
- LMB Brutal Swing: 3 vuruş — Geniş Yay → Overhead Slam → Ground Pound (AoE)
- RMB Blood Pact: kendi HP'sini harcar → Fury kazan. GÖRSEL SALDIRI YOK — self-sacrifice trade
- Key skills: Frenzied Leap (hedefe atla, iniş AoE), Bloodlust Strike (koni saldırı, HP%'ye göre hasar), Carnage Spin (2s spin AoE), Choke Throw (yakala ve fırlat)

### 06 — Ronin
- LMB Sheath Walk: hareket halinde hafif slash, 3 ardışık = güçlü son darbe
- RMB Drawn Edge: anlık kın çekimi, önündeki 3m'ye hızlı slash. Hold = hazırlık duruşu + auto-deflect
- Key skills: Quickdraw Slash (anlık %180), Iaido Stance (stance: hareketsiz Tension dolar), Sōken-giri (3m fan 5 slash), Void Cleave (Tension boşalt, koni finisher)

### 07 — Gunslinger
- LMB Dual Fire: iki silahtan EŞZAMANLI tek mermi, basılı tut = auto-fire
- RMB Hip Shot: yana kısa kayma + aynı anda tek hedefli mermi
- Key skills: Rift Dash (öne slide + ateş), Cursor Storm (cursor alan mermi yağışı), Fan the Hammer (1s 6 hızlı ateş), Deadshot (tek hedef execute shot)

### 08 — Brawler
- LMB Jab: tek hızlı yumruk, Charge+1. Hızlı tıkla = 4'lü oto-kombo
- RMB Weave: kısa yan adım savunma dodge — SALDIRI YOK. Perfect timing = Charge+2 + iframes
- Key skills: Bully (4 hızlı yumruk), Crackjaw (jab→cross→hook→uppercut), Aerial Rave (havaya at + 3 hava vuruşu), Kidney Hook (footwork hook, Charge × çarpan)

### 09 — Summoner
- LMB Command Strike: minyon varsa → komuta gesture + kısa staff darbesi; minyon yoksa → %80 own attack
- RMB Soul Dart: kısa menzilli ruh mermisi fırlat → hedefi Ruhlanmış Hedef işaretler
- Key skills: Raise Skeleton, Summon Golem, Command Beacon (cursor'a minyon kilitle), Corpse Explosion, Mass Sacrifice

### 10 — Hexer
- LMB Hex Bolt: hızlı projectile ELDEN çıkıyor (hand gesture, staff prop ama bolt elden), her 3. bolt empowered
- RMB Curse Grasp: 4m'ye el uzatma, 2 stack. Hold = 4 stack + immobilize
- Key skills: Corruption (3 stack + DoT), Hexblast (7-10 stack patlama), Pandemic (stack kopyala), Blight Sigil (cursor curse zone)

## Review Kriterleri (Her Karakter İçin)

### 1. Skill Alignment
- `run` → karakter kimliğini yansıtıyor mu? (silah tutuşu, hareket stili)
- `attack_basic` → LMB mekanik görünümüne uygun mu? (hareket tipi, silah kullanımı)
- `attack_heavy/skill_cast` → ilgili bir skill'e veya RMB'ye mantıklı görsel karşılık veriyor mu? (RMB saldırı değilse, bir skill temsil etmeli)

### 2. Frame Count
- Her animasyon 4–16 frame aralığında mı?

### 3. Özel Kontroller
- Summoner attack_basic: Command Strike gesture mi? (projectile fırlatma değil — bu Soul Dart/RMB'dir)
- Hexer attack_basic: Hex Bolt elden mi çıkıyor? (staff thrust değil)
- Gunslinger attack_basic: İki tabanca EŞZAMANLI mı ateşleniyor? (Dual Fire = simultaneous)
- Ravager attack_heavy: Frenzied Leap mi? (double overhead smash = tasarımda yok)
- Warblade attack_basic: sweep/horizontal mı? (Iron Combo 1. vuruş)
- Warblade attack_heavy: Gravity Cleave veya skill-level smash mı?

## Çıktı Formatı

STAGING/anim_prompts_skill_review_result.md dosyasına yaz:

```
# Anim Prompts Skill Alignment Review — 2026-05-13

## Ozet
- PASS: X/30
- WARN: X
- FAIL: X

## Karakter Bazli Review

### 01 — Warblade
**run** [Nf] PASS/WARN/FAIL — kisa not
**attack_basic** [Nf] PASS/WARN/FAIL — kisa not (LMB Iron Combo uyumu)
**attack_heavy** [Nf] PASS/WARN/FAIL — kisa not (hangi skill'e karsilik)

[10 karakter icin tekrarla]

## Genel Bulgular
```

## Dosyalar
Oku: `STAGING/pixellab_animation_prompts_10char.md`
Yaz: `STAGING/anim_prompts_skill_review_result.md`

Execute every step: read the file, check all 30 prompts, write the result.
