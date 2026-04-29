# Skill Revizyon Planı — 10 Class
**Tarih:** 2026-04-25 · S41
**Sebep:** Sheet inceleme + Codex review + RIMA tonu + Ranger/Shadowblade tam redesign

---

## Genel Prensipler

- **Aim mechanic** range/utility class'lar için zorunlu — cursor-aim line + hold-release
- **Ground-target zone** her class'ta en az 1 (cursor area)
- **Skill silüet ayrımı** — aynı silüet 2 skill'de olmaz
- **VFX palette** class kimliği ile tutarlı (örn. Warblade rage=kırmızı, iron=gri/silver, mavi YOK)
- **V Burst** "büyük tek varyant" değil — class identity'nin doruğu olmalı
- **Filler MMO skill yasak** (Flare, Battle Cry tek-buff, Critical Shot tek-buff vb.)

---

## 01 WARBLADE — Küçük Revizyon
**Identity korunuyor.** VFX palette + 2 skill ayrımı.

| # | Eski | Yeni | Sebep |
|---|---|---|---|
| 1 | Iron Charge | ✅ kalır | core gap-close |
| 2 | Crippling Blow | ✅ kalır | slow/setup |
| 3 | Iron Crush | ✅ kalır (silüet revize) | War Stomp ile çakışmasın |
| 4 | Gravity Cleave | ✅ kalır | utility |
| 5 | Sunder Mark | ✅ kalır | armor break |
| 6 | War Stomp | **→ Earthsplitter** (line crack, knockup line) | silüet ayrımı + line shape |
| 7 | Ironclad Momentum | ✅ kalır | buff |
| 8 | Iron Counter | ✅ kalır (visual revize: tell glow) | counter okunmuyor |
| 9 | Blade Rush | ✅ kalır | mobility-attack |
| 10 | Battle Surge | ✅ kalır | regen |
| 11 | Deep Wound | ✅ kalır | bleed core |
| 12 | Death Blow | ✅ kalır | execute |
| V | Bladestorm | ✅ kalır | iconic |

**VFX:** mavi tonlar → **steel silver + ember orange** (rage = sıcak). Battle Surge mavi → kırmızı/altın healing.

**Cross-class etkisi:** İsim değişiklikleri yok → **cross-class matrix etkilenmez**.

---

## 02 ELEMENTALİST — Büyük Revizyon
**Codex haklı: baştan ele alınmalı.** 3 element identity netleşmeli.

**Pose ayrımı (zorunlu):**
- Fire skill: öne basılı saldırgan cast pose
- Frost skill: geriye yaslanma + savunmacı staff guard
- Radiance skill: göğüs açık + yukarı bakan, kollar yana açık

| # | Eski | Yeni | Sebep |
|---|---|---|---|
| 1 | Fireball | ✅ kalır | core fire |
| 2 | Glacial Spike | ✅ kalır | core frost |
| 3 | Living Bomb | ✅ kalır | DoT |
| 4 | Blink | ✅ kalır | mobility |
| 5 | Frozen Orb | ✅ kalır | iconic kite |
| 6 | Prism Lance | **→ Prism Beam** (LINE aim — cursor channel beam) | aim mechanic eksik |
| 7 | Meteor | ✅ kalır (cursor target) | core |
| 8 | Halo Fracture | **→ Frost Wall** (LINE wall, cursor placement) | jenerik patlama→geometri |
| 9 | Sunshard Torrent | **→ Solar Flare** (cursor cone, radiance burst) | Meteor ile çakışıyor |
| 10 | Luminary Surge | **→ Radiant Pillar** (kendine yakın aura, allies heal allusion) | jenerik yer çatlağı |
| 11 | Combustion | **→ Element Charge** (passive: 3 element switch hızı + crit) | jenerik yanma |
| 12 | Blizzard | ✅ kalır | iconic frost AoE |
| V | Inferno (sadece fire) | **→ Trinity Storm** (3 element birden, dönen central rune) | tek element ihanet |

**Cross-class etkisi:** 
- WB+Elem secondary pool: Fireball, Glacial Spike, Blink, Arcane Blast (?), Chain Lightning (?), Mirror Image (?) → **bu 3'ünün kaynağı sheet'te yok, kontrol gerek**
- Etkilenen değişiklikler: Halo Fracture, Sunshard, Luminary, Combustion **secondary pool'da değil** → cross-class etkilenmez
- Prism Lance → Prism Beam: kullanıcı pool'a girmiyorsa etki yok

---

## 03 SHADOWBLADE — TAM REDESIGN
Önceki mesajda detay. 8 exportable yeni:

| Eski (cross-class kullanımda) | Yeni | Notu |
|---|---|---|
| Backstab | **Backstab Mark** | benzer fonksiyon |
| Shadow Step | **Phase Step** | benzer dash + 0.3sn invis |
| Hemorrhage (bleed) | **Death Mark** | DoT → 4sn sonra patlama |
| Kidney Shot (5sn stun) | **Shadow Pin** | 1.5sn root, dagger fırlat |
| Evasion (dodge buff) | **Smoke Veil** | AoE stealth (single player: kendi) |
| Fan of Knives (AoE bleed) | **Veil Burst** | etrafa 4 phase teleport-strike |
| Vanish (stealth) | **Smoke Veil** | yukarıyla aynı (Vanish replaced) |
| Ambush (%300 stealth) | **Sever** | low-HP execute line |

**12 skill listesi (full):**
1. Veil Strike (LMB) — quick reverse-grip slash, mark koyar
2. Phase Step
3. Backstab Mark (marked'e backstab = guarantee crit)
4. Shadow Clone (decoy phantom)
5. Death Mark (4sn delay patlama)
6. Veil Burst
7. Sever
8. Smoke Veil
9. Chain Cull (marked'den marked'e zıplar, 3 hop)
10. Shadow Pin
11. Twin Carve (önde 2 slash, ikincide phase-step arkaya)
12. Wraith Form (V Burst — 5sn ghost)

**Cross-class etkisi (büyük):**

| Combo | Eski havuz değişikliği | Yeni havuz |
|---|---|---|
| WB+Shadow | Hemorrhage→Death Mark, Kidney Shot→Shadow Pin, Evasion→Smoke Veil, Fan of Knives→Veil Burst | Backstab Mark, Phase Step, Death Mark, Shadow Pin, Smoke Veil, Veil Burst |
| Elem+Shadow | Vanish→Smoke Veil, Evasion→Sever, Hemorrhage→Death Mark, Fan of Knives→Veil Burst | Smoke Veil, Phase Step, Backstab Mark, Sever, Veil Burst, Death Mark |
| Ranger+Shadow | Vanish→Smoke Veil, Hemorrhage→Death Mark, Evasion→Sever, Ambush→Backstab Mark, Fan of Knives→Veil Burst | Smoke Veil, Phase Step, Death Mark, Sever, Backstab Mark, Veil Burst |

**Sinerji skill revize:**
- Iron Phantom (WB+Shadow): "Iron Charge sonra Phase Step" — bleed→Death Mark (Mark aktifken Iron Charge)
- Crimson Surge (Shadow+WB): "Smoke Veil veya Sever aktifken Iron Charge"
- Shadow Flame (Shadow+Elem): "Backstab Mark + yanma combo → Mark patlamasında yanma stack damage"
- Phantom Inferno (Elem+Shadow): "Smoke Veil sonra ilk spell, Death Mark + yanma birleşim"
- Hunter's Mark (Shadow+Ranger): "Smoke Veil içinde Bone Trap → Mark aktif hedefe Rift Arrow"
- Ghost Arrow (Ranger+Shadow): "Smoke Veil içinde Rift Arrow charge"

---

## 04 RANGER — TAM REDESIGN
Önceki mesajda detay. 8 exportable yeni:

| Eski | Yeni | Notu |
|---|---|---|
| Aimed Shot | **Rift Arrow** | charge mechanic + mark |
| Concussive Arrow | **Pinning Shot** | 1.5sn root |
| Disengage | **Hunter's Step** | dash + sonraki crit |
| Barbed Net Shot | **Bone Trap** | ground-target root+mark |
| Explosive Trap | ❌ çıkar | Bone Trap birleşti |
| Volley | **Sweep Volley** | cone (sağ-sol) |
| Tethering Arrow | ❌ çıkar | filler |
| Flare | ❌ çıkar | filler MMO |
| — | **Marked Detonate** | yeni: mark patlatır |
| — | **Predator's Mark** | yeni: AoE mark zone |
| — | **Rift Step** | yeni: void short-dash |

**12 skill listesi (full):**
1. Rift Arrow (LMB hold = charge + mark)
2. Pinning Shot
3. Marked Detonate
4. Hunter's Step
5. Bone Trap (cursor zone)
6. Sweep Volley (cone)
7. Skirmish Shot (Codex önerisi: hareket halinde tek hedef vur-kaç)
8. Predator's Mark (AoE mark zone)
9. Multi-Mark (5 düşmana mark)
10. Final Strike (mark'lı + low-HP execute)
11. Rift Step
12. Spirit Bow (V Burst — büyük rift-bow, 6sn infinite ammo + mark all)

**Cross-class etkisi:**

| Combo | Eski havuz değişikliği | Yeni havuz |
|---|---|---|
| WB+Ranger | Aimed Shot→Rift Arrow, Concussive→Pinning Shot, Disengage→Hunter's Step, Barbed→Bone Trap, Explosive Trap→**Marked Detonate**, Volley→Sweep Volley | Rift Arrow, Pinning Shot, Hunter's Step, Bone Trap, Marked Detonate, Sweep Volley |
| Elem+Ranger | Aimed→Rift Arrow, Disengage→Hunter's Step, Concussive→Pinning Shot, Flare→**Predator's Mark**, Barbed→Bone Trap, Tethering→**Rift Step** | Rift Arrow, Hunter's Step, Pinning Shot, Predator's Mark, Bone Trap, Rift Step |
| Shadow+Ranger | Aimed→Rift Arrow, Disengage→Hunter's Step, Barbed→Bone Trap, Explosive→Marked Detonate, Flare→**Predator's Mark**, Concussive→Pinning Shot | Rift Arrow, Hunter's Step, Bone Trap, Marked Detonate, Predator's Mark, Pinning Shot |

**Sinerji skill revize:**
- Predator's Advance (WB+Ranger): "Root altına Iron Charge" → root kaynağı Pinning Shot veya Bone Trap. "Aimed Shot CD yok" → Rift Arrow CD yok.
- Storm Shot (Elem+Ranger): "Root altına spell" → Pinning/Bone Trap kaynağı. "Aimed Shot hold mermi element" → Rift Arrow hold + element.
- Hunter's Mark (Shadow+Ranger): "Vanish içinde Explosive Trap" → Smoke Veil içinde Bone Trap. "Bleed aktif hedefe Aimed Shot" → Mark aktif hedefe Rift Arrow + crit guarantee.
- War Hunter (Ranger+WB): Tethering Arrow ile bağlanan hedef → **Predator's Mark alanındaki hedefe Iron Charge → mark patlar + bonus**.
- Elemental Arrow (Ranger+Elem): Aimed Shot hold + slow → Rift Arrow charge + slow (Glacial Spike). Blink + ok DoT → Rift Step + ok yanma.
- Ghost Arrow (Ranger+Shadow): Vanish + Aimed Shot hold → Smoke Veil + Rift Arrow charge.

---

## 05 RAVAGER — Orta Revizyon

| # | Eski | Yeni |
|---|---|---|
| 1 | Bloodlust Strike | ✅ kalır |
| 2 | Whirlwind | **→ Carnage Spin** (sheet rename, silüet farklılaştır: Bladestorm zarif/dans, Carnage Spin brute/parça uçar) |
| 3 | Frenzied Leap | ✅ kalır |
| 4 | Reckless Swing | ✅ kalır |
| 5 | Bloodthirst | ✅ kalır (lifesteal) |
| 6 | Intimidating Shout | **→ Bloodied Roar** (% HP düştükçe buff güçlenir, dramatic) |
| 7 | Barbaric Charge | ✅ kalır |
| 8 | Undying Tenacity | ✅ kalır |
| 9 | Iron Grab | ✅ kalır (single-target grab + slam) |
| 10 | Blood-Drunk Leap | ✅ kalır |
| 11 | Shatter Armor | ✅ kalır |
| 12 | Death Wish | ✅ kalır |
| Battle Cry (RMB action) | **→ Blood Pact** (RMB: kendi HP harca, Fury kazan) | shout filler değil, resource trade |
| V | Berserk Mode | ✅ kalır (sheet'te küçük → görsel büyütülecek) |

**VFX:** kırmızı slash tekrarı azalt — Battle Cry/Shout mor halo'lar **kaldırılacak**, sıcak kırmızı + et/kemik partikül.

**Cross-class etkisi:** Whirlwind→Carnage Spin (rename only), Battle Cry→Blood Pact (mekanik değişti, tema benzer). Faz 3 cross-class taslak etkilenir; **detay Faz 3'te netleşir**, şimdi sadece isim güncelle.

---

## 06 RONIN — Küçük Revizyon

| # | Eski | Yeni |
|---|---|---|
| 1 | Quickdraw Slash | ✅ kalır |
| 2 | Haste Dash | ✅ kalır |
| 3 | Mille Feuille Cut | **→ Sōken-giri** (multi-cut, Japon adı) |
| 4 | Iaido Stance | ✅ kalır |
| 5 | Wind Step | ✅ kalır (silüet farklılaştır: Wind = düz line, Phantom = arc) |
| 6 | Counter Draw | ✅ kalır |
| 7 | Phantom Step | ✅ kalır (yukarıdaki silüet ayrımı ile) |
| 8 | Blade Veil | **→ Sakura Veil** (savunma posture, petal swirl, tek sakit cycle) |
| 9 | Crescent Arc | ✅ kalır |
| 10 | Flash Draw | ✅ kalır (iaido execute) |
| 11 | Iai Pressure | ✅ kalır (debuff) |
| 12 | Void Cleave | ✅ kalır |
| V | Mugen No Kiri | ✅ kalır |

**Sheet:** content underweight, daha fazla detay (her skill için 3-4 frame mini). VFX: mavi blur ↓, beyaz/silver edge + yumuşak motion line.

**Cross-class etkisi:** Mille Feuille→Sōken-giri (rename), Blade Veil→Sakura Veil (rename + visual). Faz 3 taslak etkilenir; **isim güncelle**.

---

## 07 GUNSLINGER — Orta Revizyon

| # | Eski | Yeni |
|---|---|---|
| 1 | Rift Dash | ✅ kalır |
| 2 | Quickdraw | ✅ kalır |
| 3 | Bullet Rain | **→ Cursor Storm** (cursor area, mermi yağışı sadece o noktaya) |
| 4 | Critical Shot (buff) | **→ Deadshot** (Codex: cursor-line tek hedef execute shot) |
| 5 | Smoke Grenade | ✅ kalır |
| 6 | Fan the Hammer | ✅ kalır (cone) |
| 7 | Suppression Fire | ✅ kalır (channel) |
| 8 | Dead Eye (buff) | **→ Rift Grenade** (Codex: cursor zone, gecikmeli patlama) |
| 9 | Ricochet | ✅ kalır |
| 10 | Reload Dance | ✅ kalır |
| 11 | Burning Ammo | ✅ kalır |
| 12 | Point Blank Execute | ✅ kalır |
| V | Full Metal Storm | ✅ kalır (Bullet Rain ayrıştı, çakışma yok artık) |

**Karakter visual:** "western lady" cliché → daha kirli/ritualistic. Hat korunabilir ama **bone/feather aksesuar** + worn leather coat (puffy sleeves yok), boyun çevresinde **rift-marked bandana**. Renk: deep auburn red kararı korunur (#44).

**Cross-class etkisi:** Critical Shot→Deadshot, Dead Eye→Rift Grenade — Faz 3 taslak etkilenir; **isim+mekanik güncelle**.

---

## 08 BRAWLER — Orta Revizyon

| # | Eski | Yeni |
|---|---|---|
| 1 | Mach Punch | ✅ kalır (jab pose net) |
| 2 | Shockwave Slam | ✅ kalır |
| 3 | Tornado Kick | ✅ kalır |
| 4 | Rush Combo | **→ Combo Chain** (jab→cross→hook→uppercut, 4-frame net pose) |
| 5 | Guard Break | ✅ kalır |
| 6 | Repulse | ✅ kalır |
| 7 | Counter Blow | ✅ kalır (counter tell glow ekle) |
| 8 | Aerial Rave | ✅ kalır (rising attack) |
| 9 | Cyclone Drive | ✅ kalır |
| 10 | Seismic Stomp | ✅ kalır |
| 11 | Momentum Strike | **→ Pivot Hook** (footwork-based hook, side step + power punch) |
| 12 | Unstoppable Force | ✅ kalır |
| V | Overdrive | ✅ kalır |

**Karakter:** statik dik → dinamic guard pose (sol omuz öne, sol el yüksek, sağ el alt çene koruma). **Gauntlet** silüet net görünmeli — kemik/metal protector.

**VFX:** mor patlama tekrarı ↓ — yumruk impact'ları beden hareketinden okunmalı (motion blur ucu, omuz rotasyon ipucu). Mor sadece "arcane fury proc" anlarında.

**Cross-class etkisi:** Rush Combo→Combo Chain (mekanik aynı, görsel net), Momentum Strike→Pivot Hook (mekanik biraz değişti). Faz 3 taslak; **isim güncelle**.

---

## 09 SUMMONER — Küçük + Codex Eklemeleri

| # | Eski | Yeni |
|---|---|---|
| 1 | Raise Skeleton | ✅ kalır |
| 2 | Summon Golem | ✅ kalır |
| 3 | Rally Cry | **→ Command Beacon** (Codex: cursor noktasına minyon kitleme) |
| 4 | Corpse Explosion | ✅ kalır |
| 5 | Death Nova | ✅ kalır |
| 6 | Commanding Strike | ✅ kalır (cursor-target enemy mark, minyonlar saldırır) |
| 7 | Blood for Power | ✅ kalır |
| 8 | Bone Shield | ✅ kalır |
| 9 | Soul Siphon Totem | ✅ kalır (cursor placement) |
| 10 | Mass Sacrifice | ✅ kalır |
| 11 | Dark Pact | ✅ kalır |
| 12 | Lich Form | ✅ kalır |
| V | Army of the Dead | ✅ kalır |

**Karakter:** Hexer ile cloak silüet çakışması → Summoner **lantern + skeleton helm** öne çıksın (hood ↓). 

**VFX:** soul mavi → cyan + bone white kombosu, **command line** (master→minion) net görünür ipucu.

**Cross-class etkisi:** Rally Cry→Command Beacon. Faz 4 taslak; **isim güncelle**.

---

## 10 HEXER — Küçük + Codex Eklemesi

Sheet zaten güçlü. Tek ekleme:

| # | Eski | Yeni |
|---|---|---|
| 11 | Cursed Mirror | **→ Blight Sigil** (Codex: cursor curse zone, basana stack) |

veya **Cursed Mirror'ı kaldırma**, yerine Blight Sigil ekleme — kullanıcı kararı. Önerim: Cursed Mirror enteresan, **Soul Bargain (#12) yerine Blight Sigil eklensin**, Soul Bargain çok meta.

**Karakter:** Summoner ile cloak çakışması → Hexer **floating sigil sembolleri** etrafında + **soul lantern yeşil** (Summoner mavi, Hexer yeşil ayrımı net).

**Cross-class etkisi:** Faz 4 taslak; isim güncelle.

---

## ÖZET — Cross-Class Değişen 8-Slot Listesi

| Class | Eski → Yeni 8-Exportable |
|---|---|
| Warblade | (değişiklik yok) |
| Elementalist | (değişiklik yok — pool skills aynı) |
| Shadowblade | Backstab Mark, Phase Step, Death Mark, Shadow Pin, Smoke Veil, Veil Burst, Sever, Backstab Mark (8) |
| Ranger | Rift Arrow, Pinning Shot, Hunter's Step, Bone Trap, Marked Detonate, Predator's Mark, Sweep Volley, Rift Step (8) |
| Ravager | Bloodlust Strike, Frenzied Leap, Reckless Swing, **Bloodied Roar** (was Battle Cry/Intimidating Shout), Undying Tenacity, Death Wish, ... |
| Ronin | (rename: Mille Feuille→Sōken-giri, Blade Veil→Sakura Veil) |
| Gunslinger | Rift Dash, Quickdraw, **Cursor Storm** (was Bullet Rain), **Deadshot** (was Critical Shot), Smoke Grenade, **Rift Grenade** (was Dead Eye), Ricochet, ... |
| Brawler | (rename: Rush Combo→Combo Chain, Momentum Strike→Pivot Hook) |
| Summoner | (rename: Rally Cry→Command Beacon) |
| Hexer | **Blight Sigil** (was Soul Bargain veya Cursed Mirror — kullanıcı karar) |

**Update edilecek dosyalar:**
- `TASARIM/CROSS_CLASS_SKILL_MATRIX.md` — pool tabloları + sinerji skill koşulları
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` — class skill listeleri
- `Assets/Scripts/CrossClass/CrossClassSkillData.cs` — SO'lar (skill ID rename)
- `Assets/Data/CrossClass/` 10 SO asset — yeni isim/efekt

**Codex'e devredilecek (mekanik):**
- SkillDatabase.cs SO ID rename
- CrossClassSkillData asset rename
- SINIF_VE_SKILL_KARAR_BELGESI tablo update
