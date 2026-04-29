# Sınıf ve Skill Karar Belgesi
*2026-04-11 güncellendi | Crusader+Lancer kaldırıldı → Ronin+Brawler+Gunslinger eklendi*
*2026-04-11 v2 — Balance pass: 29 nokta düzeltme (abuse kapatma, loop kırma, mekanik netleştirme)*
*2026-04-14 v3 — ChatGPT full skill audit (RIMA_full_skill_audit_claude_ready.md) işlendi: 10 class cerrahi fix + 5 ek mikro-buff*
*2026-04-14 v4 — ChatGPT Parts 1-9 işlendi: Shadowblade tam redesign (Sever/Rift Scar), Elementalist Light synthesis state, V meter tüm classlara ayrı dolum koşulu, Ravager V burst kan siklozu, Hexer Hexblast 7+ stacks, Brawler Charge banking, Ranger/Gunslinger dash attack kimliği, Item System D kilitleme*
*2026-04-16 v5 — ChatGPT + Gemini çapraz audit: dash attack 6 sınıf cerrahi (Warblade CC↓/Rage↓, Ravager knockback→stagger/Fury↓, Ronin hold kaldırıldı, Brawler Charge↓, Summoner Commanding Strike redundancy kapatıldı, Hexer sadeleştirildi)*
*2026-04-17 v6 — RMB mekaniği eklendi (Ravager Battle Cry, Ronin Drawn Edge, Hexer Curse Grasp). Elementalist RMB/Dash ayrıştırıldı. Warblade [V] Dolum: Rift Parry → Iron Counter (Karar #6 uyumu).*
*2026-04-25 v7 (S41) — Skill revizyon planı işlendi: Shadowblade+Ranger tam redesign, 8 class kısmi revizyon, VFX palette düzeltmeleri. Kaynak: `STAGING/SKILL_REVIZYON_PLANI.md`.*
*2026-04-29 v8 (S43) — Eski skill isim drift'i temizlendi; Elementalist Light isimleri MASTER #16 ile senkronlandı; cross-class matrix arşiv notu eklendi.*

---

## CLASS VERB TABLOSU (S41 KİLİT)

> Her class'ın 3 verb'i combat kimliğini tanımlar. Cross-class matrix ve sinerji skill tasarımı bu verb'lere dayalı.

| Class | Verbs |
|---|---|
| Warblade | engage, break, execute |
| Elementalist | switch, shape, detonate |
| Shadowblade | phase, scar, collapse |
| Ranger | mark, trap, detonate |
| Ravager | suffer, trade, frenzy |
| Ronin | wait, draw, punish |
| Gunslinger | slide, shoot, reload |
| Brawler | weave, combo, launch |
| Summoner | command, sacrifice, raise |
| Hexer | stack, spread, blast |

## Cross-Class Interactions

> S43 notu: Eski `CROSS_CLASS_SKILL_MATRIX.md` arşivlendi; yeni cross-class rebuild bu belgedeki canonical class/skill isimleriyle yapılmalı.

- Cross-class kombinasyonları yönlüdür: A primary + B secondary, B primary + A secondary ile aynı kimlik olmak zorunda değildir.
- Auto pasifler salt stat bonusu olmamalı; primary aksiyonu tetikler, secondary buna davranışsal cevap verir.
- Sinerji skill'leri Epic tier olarak ele alınır ve her biri iki class verb'i arasında okunabilir bir köprü kurar.

---

## S41 SKILL REVİZYON ÖZETİ

> Ayrıntı: `STAGING/SKILL_REVIZYON_PLANI.md`. Aşağıda class başlıklarında revize edilen skill'ler `[S41]` etiketi ile işaretlendi.

**Genel prensipler:**
- Aim mechanic range/utility class'lar için zorunlu (cursor-aim line + hold-release)
- Ground-target zone her class'ta en az 1
- Skill silüet ayrımı — aynı silüet 2 skill'de olmaz
- VFX palette class kimliği ile tutarlı
- Filler MMO skill yasak (Flare, Battle Cry tek-buff, Critical Shot tek-buff vb.)

**Değişen isimler (cross-class matrix için):**
| Class | Eski | Yeni |
|---|---|---|
| Warblade | War Stomp | **Earthsplitter** (line crack, knockup line) |
| Elementalist | Prism Lance | **Prism Beam** (LINE aim, cursor channel beam) |
| Elementalist | Halo Fracture | **Frost Wall** (LINE wall, cursor placement) |
| Elementalist | Sunshard Torrent | **Solar Flare** (cursor cone, radiance burst) |
| Elementalist | Luminary Surge | **Radiant Pillar** (kendine yakın aura) |
| Elementalist | Combustion | **Element Charge** (passive: 3 element switch hızı + crit) |
| Elementalist | Inferno [V] | **Trinity Storm** (3 element birden, dönen central rune) |
| Shadowblade | (tam redesign) | bkz. Shadowblade bölümü |
| Ranger | (tam redesign) | bkz. Ranger bölümü |
| Ravager | Whirlwind | **Carnage Spin** (brute/parça uçar) |
| Ravager | Intimidating Shout | **Bloodied Roar** (% HP düştükçe buff güçlenir) |
| Ravager | Battle Cry (RMB) | **Blood Pact** (HP harca, Fury kazan) |
| Ronin | Mille Feuille Cut | **Sōken-giri** (multi-cut, Japon adı) |
| Ronin | Blade Veil | **Sakura Veil** (petal swirl, tek sakit cycle) |
| Gunslinger | Bullet Rain | **Cursor Storm** (cursor area) |
| Gunslinger | Critical Shot | **Deadshot** (cursor-line tek hedef execute shot) |
| Gunslinger | Dead Eye | **Rift Grenade** (cursor zone, gecikmeli patlama) |
| Brawler | Rush Combo | **Combo Chain** (jab→cross→hook→uppercut) |
| Brawler | Momentum Strike | **Pivot Hook** (footwork-based hook) |
| Summoner | Rally Cry | **Command Beacon** (cursor noktasına minyon kitleme) |
| Hexer | Soul Bargain (#12) | **Blight Sigil** (cursor curse zone, basana stack) |

---

## İSİM TEMATİĞİ — 3 SEÇENEK

Şu an isimler karışık: Warblade (silah ismi), Elementalist (rol ismi), Rogue (D&D generic), Brawler (çok kasual).

### Seçenek A — "Dark Action" (Koyu/Epik)
Warblade → **Juggernaut** | Elementalist → **Arcanist** | Rogue → **Shadowblade** | Ranger → **Strider** | Brawler → **Ravager** | Paladin → **Templar** | Summoner → **Necromancer** | Hexer → **Hexer** | Hız → **Tempest** | Kan → **Hemomancer**

### Seçenek B — "MMORPG Classic" (Tanıdık)
Warblade → **Warrior** | Elementalist → **Mage** | Rogue → **Rogue** | Ranger → **Hunter** | Brawler → **Berserker** | Paladin → **Paladin** | Summoner → **Summoner** | Hexer → **Warlock** | Hız → **Drifter** | Kan → **Bloodmage**

### Seçenek C — "Hybrid" (Şu anki tona yakın)
Warblade → **Warblade** | Elementalist → **Elementalist** | Rogue → **Shadowblade** | Ranger → **Ranger** | Brawler → **Ravager** | Paladin → **Paladin** | Summoner → **Summoner** | Hexer → **Hexer** | Hız → **Drifter** | Kan → **Hemomancer**

> Aşağıda sınıflar Seçenek C isimleriyle yazıldı — karar sonrası toplu değiştirilebilir.

---

## GÖSTERIM FORMATI

`[YENİ]` = Araştırma önerisiyle değiştirildi
`[MEVCUT]` = Orijinal, korunuyor
`★` = İmza skill (ilk oda garantili teklif)

---

## ⚔️ 1. WARBLADE ✅ FİNAL

**Core Fantasy:** "Yaklaş. Sabitle. Zırhı kır. İnfaz et."
**Kaynak:** Rage (0-100) — hasar VEREREK +10/vuruş, CC'li düşmana +20, boşta -5/sn *(savaş dışında — oda temizse — drain yok)*
> Note: Skill Rage values (Rage+N) are additive burst gains on top of passive code accumulation. Canonical passive: 1/hit-dealt, 5/hit-taken, 3/kill, decay 10/s. Code is authoritative for base rate.
**[V] Dolum (Dominance):** CC uygulamaları, başarılı Iron Counter (0.8s pencere), CC sonrası punish aksiyonları. Hasar verme döngüsünden ayrı ritim.
**[V] Burst:** BLADESTORM — V Meter dolunca: 5s spin, CC immune, her 0.5s AoE %120 hasar
**Demo sınıfı:** ✅ Faz 1'de mevcut

**🖱️ Temel:**
- **LMB — Iron Combo:** 3 vuruşluk melee zincir (sweep → overhead → shoulder ram). Her vuruş Rage+8, 3. vuruş küçük knockback + Rage+15. 0.8s duraksama = combo sıfırlanır.
- **RMB — Rage Outlet:** Rage 30+ iken aktif. Kısa AoE patlaması, Rage −30, çevredeki düşmanlar sendeliyor. Rage boşaltırken hasar verir, CD 1.5s.
- **Dash Attack — Momentum Slam:** Dash sonrası 0.5s içinde LMB → omuz darbesi, 0.6s dizzy + Rage+12. Iron Charge'dan farkı: skill slot tutmaz, her zaman hazır, kısa mesafe agresif giriş.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Iron Charge** ★ | ▶⬡ | Core | 8m dash + 1.5s stun, Rage+20 | Stun'daki hedefe ilk vuruş → +%80 hasar |
| 2 | **Crippling Blow** | ⚡💥 | Core | Büyük hasar + iyileşme -%50 (6s) | Iron Charge sonrası → iyileşme -%100 |
| 3 | **Iron Crush** | ✦ | Core | 6s: tüm hasar +%30 | Sunder Mark aktif hedefte → katlanır |
| 4 | **Gravity Cleave** | ⬡⚡ | Core | Silahı yere çarpar, 4m çapında çeker + %140 hasar, 0.8s slow | Iron Charge sonrası → çekilenler 1.5s stun, Rage+15 |
| 5 | **Sunder Mark** | ✦↓ | Core | Hedefe işaret: 8s zırh -%40, tüm hasar bonusu görünür | Death Blow aktifken → zırh -%60 |
| 6 | **Earthsplitter** | ⬡↑ | Core | 3m knockup 2s, Rage+25 | Bladestorm sırasında → +1s uzar |
| 7 | **Ironclad Momentum** | ↑✦ | Core | 6s: alınan hasar %30 yok sayılır + her 10 hasar = +10 Rage | Earthsplitter sonrasi → savunma %50'ye çıkar |
| 8 | **Iron Counter** | ↑⚡ | Core | 0.8s pencere: vurulursa %180 karşı saldırı + Rage+25 + 0.5s stun | Rage 80+ → knockback şok dalgası + 0.5s stagger |
| 9 | **Blade Rush** | ▶↑ | Advanced | 6m dash + çizgideki herkese %120, Rage+15/hedef | 3+ hedef → Rage+50 |
| 10 | **Battle Surge** | ✦↑ | Advanced | 8s: her Rage harcaması = HP +%5 *(harcama sonrası 2s internal CD — bu sürede ikinci harcama heal üretmez)* | Rage 80+'ta aktive → süre 12s |
| 11 | **Deep Wound** | ⚡↑ | Advanced | Bleed DoT 8s + Rage+35 | Iron Crush window → bleed tick 2× |
| 12 | **Death Blow** | 💥⬡ | Master | SADECE HP<%30: %400 hasar, Rage boşaltır | Crippling Blow aktifken → %600 |

**Build Eksenleri:**
- **"Execution"** → Iron Charge + Crippling Blow + Iron Crush + Death Blow
- **"Control Breaker"** → Gravity Cleave + War Stomp + Sunder Mark + Death Blow
- **"Last Stand"** → Ironclad Momentum + Iron Counter + Battle Surge + Death Blow

**Kaynak notu:** Rage VEREREK dolar (Ravager'dan farkı: Ravager alarak dolar)

---

## 🔥 2. ELEMENTALİST → ELEMENTALİST / ARCANİST

**Core Fantasy:** "Her şeyi yakıyorum. Ama önce ritmi buluyorum."
**Kaynak:** Mana (0-100, +8/sn) + Elemental State (Fire veya Frost, max 5 stack)
**[V] Dolum (Convergence):** Element reaksiyonları, Lightbreak tetiklemeleri, Fire→Frost / Frost→Fire combo switch pencereleri. Mana döngüsünden ayrı ritim — ritim ustalığını ödüllendirir.
**[V] Burst:** TRINITY STORM — V Meter dolunca: 7s boyunca Fire/Frost/Light üçlemesini aynı anda döndüren central rune fırtınası

**🖱️ Temel:**
- **LMB — Rift Bolt:** Hızlı rift enerji mermisi, Mana+3/isabet. Her 3. bolt empowered (daha büyük, +1 Elemental State). Hareket halinde atılabilir.
- **RMB — Element Switch:** Fire ↔ Frost geçiş (tap). Her Fire/Frost cast ayrı Resonance biriktirir. Her ikisi 3'e ulaşınca 0.2s basılı tut = **Lightbreak** tetiklenir (3+3 stack tüketir, 6s Light State açar, 3 Light stack üretir). Light State bitince önceki elemente döner. CD yok (tap); Lightbreak CD 8s.
- **Dash Attack — Elemental Pulse:** Dash sonrası 0.4s içinde LMB → aktif elemente göre 2m AoE mikro-patlama. Fire = anlık yanma DoT. Frost = 0.5s chill + slow. Lightbreak aktifken → radiant patlama, Light State+1. Mobil caster kimliğinin ofansif çıkışı.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Fireball** ★ | ↑▶ | Core | Orta hasar + ateş DoT 4s, Fire State+1 | 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | **[YENİ] Glacial Spike** | ⬡↑ | Core | 6m buz hattı: hattaki düşmanlar %40 slow + %180 hasar, Frost State+2. Fire State 1 stack tüketir | Fireball DoT aktif hedefe → Freeze 2s + DoT hasarı tek seferde patlar (%150) |
| 3 | **Living Bomb** | ⚡↓ | Core | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Glacial Spike slow altında → patlama yarıçapı 2× |
| 4 | **Blink** | ▶⚓ | Core | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Düşmanın içinden → 0.5s stun |
| 5 | **Frozen Orb** | ⬡⚓ | Core | Yavaş hareket eden küre, yolundakileri 5s chill | Orb üzerinden Blink → Orb patlar, Frozen 2s |
| 6 | **Prism Beam** | ↑⬡ | Core | Cursor line channel beam: düz hat boyunca tüm düşmanları deler; Light State stack'leri harcarsa hasar artar | Light State 3 stack → patlama + 2m AoE radiant burst |
| 7 | **Meteor** | 💥⬡ | Core | 0.5s wind-up → büyük AoE knockdown (hareket devam eder, kanallamaz) | Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | **Frost Wall** | ⚓↑ | Core | Cursor placement LINE wall: 4s ışık-buz bariyeri, temas eden düşmanları yavaşlatır ve radiant çatlama hasarı verir | Frozen düşman yakında → Freeze'i 1s uzatır |
| 9 | **Solar Flare** | ⚓⚡ | Advanced | Cursor cone radiant burst; hat boyunca kesişen düşmanlara piercing ışık hasarı uygular | Light State aktifken → cone içinde ek radiant pulse |
| 10 | **Radiant Pillar** | ✦▶ | Advanced | 6s: kendine yakın aura; Fire/Frost skill'ler radiant echo oluşturur, Light skill recovery -%30 | Lightbreak sonrası cast → süre 10s'ye uzar |
| 11 | **Element Charge** | ✦▶ | Advanced | 8s: tüm Fire spell instant cast, mana maliyet ×2 | Fire State 5 stack → mana maliyet artışı yok |
| 12 | **Blizzard** | ⬡↑ | Master | 1s cast → bölge bağımsız 8s slow+tick (kanal gerekmez, hareket devam eder) | Meteor'dan önce → Meteor knockdown 4s |

**Build Eksenleri:** "Fire Burst" (Element Charge+Fireball+Living Bomb+Meteor) / "Frost Lock" (Glacial Spike+Blizzard+Frozen Orb+Meteor) / "Radiant Break" (Prism Beam+Radiant Pillar+Frost Wall+Solar Flare)

---

## 🌀 3. SHADOWBLADE ✦ TAM REDESİGN — Faz 2

> **[S41] TAM REDESIGN** — Eski isimler (Vanish, Fan of Knives, Kidney Shot, Backstab, Hemorrhage, Evasion, Ambush) geçersiz.

**Core Fantasy:** "Yankını vuruyorum. Yara mekânda açık kalıyor."
**Kaynak:** Sever (0-100) — düşmandan geçerek, hedefe karşı tarafı değiştirerek veya saldırıdan faz geçerek dolar. Stack-on-hit değil: pozisyonel, geometrik hareket.
**[V] Dolum (Predation):** Rift Scar collapse zincirleri ve collapse sonrası hızlı öldürme pencereleri. Sever'den farklı aksiyon tipi: sona erdirme vs. geçiş.
**[V] Burst:** WRAITH FORM — 5sn ghost, Scar'larla tüm engeller geçilir.

**🖱️ Temel:**
- **LMB — Veil Strike:** Hızlı reverse-grip slash, mark koyar. Sever+8/vuruş.
- **RMB — Veil Flicker:** Düşmandan/vektörden kısa faz geçişi; geçilen hedef Rift Scar alır. Sever+15. CD 1.2s.
- **Dash Attack — Seam Rend:** Evrensel dash + LMB hedeften geçerek keser, gecikmeli Rift Scar bırakır. Scar yalnızca karşı taraftan vurulursa collapse olur.

**S41 skill listesi (12 + V):**
1. Veil Strike (LMB) — quick reverse-grip slash, mark koyar
2. Phase Step — benzer dash + 0.3sn invis
3. Backstab Mark — marked'e backstab = guarantee crit
4. Shadow Clone — decoy phantom
5. Death Mark — 4sn delay patlama
6. Veil Burst — etrafa 4 phase teleport-strike
7. Severance — low-HP execute line
8. Smoke Veil — AoE stealth (kendi single player)
9. Chain Cull — marked'den marked'e zıplar, 3 hop
10. Shadow Pin — 1.5sn root, dagger fırlat
11. Twin Carve — önde 2 slash, ikincide phase-step arkaya
12. Night Aperture — 6s: her dash/Veil Flicker giriş ve çıkış noktasında mirrored Scar

**V Burst:** Wraith Form (5sn ghost)

**Cross-class exportable pool (8):**
Backstab Mark, Phase Step, Death Mark, Shadow Pin, Smoke Veil, Veil Burst, Sever, Chain Cull

**Sinerji skill güncellemesi:**
- Iron Phantom (WB+Shadow): "Iron Charge sonra Phase Step" — Mark aktifken Iron Charge + Death Mark combo
- Crimson Surge (Shadow+WB): "Smoke Veil veya Sever aktifken Iron Charge"
- Shadow Flame (Shadow+Elem): "Backstab Mark + yanma combo → Mark patlamasında yanma stack damage"
- Phantom Inferno (Elem+Shadow): "Smoke Veil sonra ilk spell, Death Mark + yanma birleşim"
- Hunter's Mark (Shadow+Ranger): "Smoke Veil içinde Bone Trap → Mark aktif hedefe Rift Arrow"
- Ghost Arrow (Ranger+Shadow): "Smoke Veil içinde Rift Arrow charge"

**Build Eksenleri:**
- **"Phase Predator"** → Phase Step + Veil Burst + Chain Cull + Wraith Form
- **"Mark Collapse"** → Backstab Mark + Death Mark + Shadow Pin + Sever
- **"Smoke Duelist"** → Smoke Veil + Twin Carve + Shadow Clone + Night Aperture

---

## 🏹 4. RANGER → RANGER / STRİDER

> **[S41] TAM REDESIGN** — Eski isimler (Aimed Shot, Concussive Arrow, Disengage, Barbed Net Shot, Explosive Trap, Volley, Tethering Arrow, Flare) geçersiz ya da revize edildi.

**Core Fantasy:** "Sana ulaşamazsın. Her saniye kayıp veriyorsun."
**Kaynak:** Focus (4m+: +10/sn | 2m-: -20/sn) — Focus 75+: +%25 hasar | Focus 100: next skill free cast
**[V] Dolum (Kill Zone):** Tuzak tetiklemeleri, mark detonasyonları, menzilden onaylı öldürmeler. Distance-based Focus'tan farklı ritim: alan kontrolü olayları.
**[V] Burst:** Spirit Bow — 6sn infinite ammo + mark all

**🖱️ Temel:**
- **LMB — Rift Arrow** (LMB hold = charge + mark): Anında tek ok, Focus+4. Hold 1s = charge + guaranteed mark.
- **RMB — Tactical Roll:** Hareket yönüne kısa geri atlama + atlama sırasında 1 ok anında serbest ateş. Focus+10, CD 1.2s.

**S41 skill listesi (12 + V):**
1. Rift Arrow (LMB hold = charge + mark)
2. Pinning Shot (1.5sn root)
3. Marked Detonate (mark patlatır)
4. Hunter's Step (dash + sonraki crit)
5. Bone Trap (cursor zone — ground-target root+mark)
6. Sweep Volley (cone, sağ-sol)
7. Skirmish Shot (hareket halinde tek hedef vur-kaç)
8. Predator's Mark (AoE mark zone)
9. Multi-Mark (5 düşmana mark)
10. Final Strike (mark'lı + low-HP execute)
11. Rift Step (void short-dash)
12. Spirit Bow (V Burst — büyük rift-bow, 6sn infinite ammo + mark all)

**V Burst:** Spirit Bow (6sn)

**Cross-class exportable pool (8):**
Rift Arrow, Pinning Shot, Hunter's Step, Bone Trap, Marked Detonate, Predator's Mark, Sweep Volley, Rift Step

**Sinerji skill güncellemesi:**
- Predator's Advance (WB+Ranger): "Root altına Iron Charge" → Pinning Shot/Bone Trap kaynağı. "Rift Arrow CD yok" koşulu.
- Storm Shot (Elem+Ranger): "Root altına spell" → Pinning/Bone Trap kaynağı. "Rift Arrow hold + element."
- Hunter's Mark (Shadow+Ranger): "Smoke Veil içinde Bone Trap → Mark aktif hedefe Rift Arrow + crit guarantee."
- War Hunter (Ranger+WB): "Predator's Mark alanındaki hedefe Iron Charge → mark patlar + bonus."
- Elemental Arrow (Ranger+Elem): "Rift Arrow charge + slow (Glacial Spike). Rift Step + ok yanma."
- Ghost Arrow (Ranger+Shadow): "Smoke Veil + Rift Arrow charge."

**Build Eksenleri:** "Sniper Mark" (Rift Arrow+Predator's Mark+Multi-Mark+Final Strike) / "Trap Master" (Bone Trap+Marked Detonate+Pinning Shot+Hunter's Step) / "Kite Burst" (Sweep Volley+Rift Step+Skirmish Shot+Spirit Bow)

---

## 👊 5. RAVAGER

> **[S41] ORTA REVİZYON** — Whirlwind→Carnage Spin, Intimidating Shout→Bloodied Roar, Battle Cry (RMB)→Blood Pact. VFX: mor halo kaldırıldı, sıcak kırmızı + et/kemik partikül.

**Core Fantasy:** "Az canken daha tehlikeliyim. Bu hata değil, strateji."
**Kaynak:** Fury (0-100) — SADECE hasar alarak +15/vuruş, HP düştükçe daha hızlı
**[V] Dolum (Carnage):** Kill zinciri uzunluğu — her öldürme öncekinden 4s içindeyse V dolar, zincir uzadıkça her kill daha fazla V üretir. Zincir koparsa V duraklar (sıfırlanmaz). Fury'den (hasar alarak) farklı ritim: öldürmek vs. öldürülmek.
**[V] Burst:** BERSERK MODE — V Meter dolunca: 6s kan siklozu. Ravager etrafında (2.5 birim yarıçap) dönen kan halkası alandaki tüm düşmanlara sn başına hasar verir. Her 0.5s ana hedefe konsantre kan darbesi. Her kill +0.8s uzatır (max +3s). Süre boyunca: Fury kazanımı 2×, hareket hızı +%20. Süre bitiminde kan halkası patlar (AoE hasar).

**🖱️ Temel:**
- **LMB — Brutal Swing:** 3 vuruşluk ağır balta zinciri (Geniş Yay → Overhead Slam → Ground Pound). Yay ve Slam 1-3 düşmana, Ground Pound yere çarparak AoE. Her vuruş Fury+12/isabet, son 1s hasar aldıysan Fury+20. 3. vuruş Fury+30 bonus. 1.0s duraksama = combo sıfırlanır.
- **RMB — Blood Pact** [S41]: Kendi HP'sini harcar, Fury kazan. Fury+20. Savaş dışında kullanılamaz. CD 8s. *HP trade → Fury döngüsü için tetikleyici. Shout filler değil, resource trade.*
- **Dash Attack — Fury Tackle:** Dash sonrası 0.5s içinde LMB → lowered shoulder tackle, 0.5s stagger (knockback yok — berserker pocket'ta kalır) + Fury+20. Son 1s içinde hasar aldıysan Fury+30. 1.5m çevredeki düşmanları sana yönlendirir + 0.4s kısa zırh. CD 2s. Fury doldurma döngüsünü hızlandırır.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Bloodlust Strike** ★ | ⚡💥 | Core | Koni saldırı, HP'ye göre hasar artar (%30HP=+%120) | Fury %80+ → Slaughter anında açılır |
| 2 | **Carnage Spin** [S41] | ⚓↓ | Core | 2s spin AoE, her vuruş savunma -%5 (max -%30). Silüet ayrımı: Bladestorm zarif/dans, Carnage Spin brute/parça uçar | Savunma -%30 → Fury +20/spin |
| 3 | **Frenzied Leap** | ▶↑ | Core | Hedefe atla, iniş AoE, hit=CD sıfır | 3 farklı hedefe → 5s Frenzy +%50 hasar |
| 4 | **Reckless Swing** | 💥↓ | Core | Devasa tek hasar, 2s tam savunmasız | Savunmasızken hasar → Fury+40 + 0.8s invuln |
| 5 | **Bloodthirst** | ⚓↑ | Core | Hızlı 5 vuruş, her vuruş küçük iyileşme | HP<%20 + Fury%100 → 8 vuruşa yükselir |
| 6 | **Bloodied Roar** [S41] | ⬡ | Core | 3m çevresinde 3s stagger. % HP düştükçe buff güçlenir | Stagger'daki hedefe Bloodlust Strike → +%100 hasar |
| 7 | **Barbaric Charge** | ▶⬡ | Core | Düz çizgide her şeyi iter, stun/root immune | Duvara çarparsa → stun 2s |
| 8 | **[YENİ] Undying Tenacity** | ⚓↑ | Core | Fatal hasar HP'yi 1'e indirir; 4s hasar bagisikligi kazanir, bu pencerede iyilesme devre disi. Cooldown: 45s. | Death Wish aktifken → pencere 6s'ye uzar, bitis aninda Fury +30 |
| 9 | **Iron Grab** | ⬡▶ | Advanced | Yakala (≤3m): 1.5s hold, fırlat, Fury+30 | Fırlatılan 3.'e çarparsa → her ikisi stun |
| 10 | **[YENİ] Blood-Drunk Leap** | ▶↓ | Advanced | Hedefe sıçrar, %120 hasar. Fury'nin %30'unu tüketir, her 10 Fury +%20 hasar | HP<%40 iken → vuruşun %15'i lifesteal + 2s CC bağışıklığı |
| 11 | **Shatter Armor** | ✦▶ | Advanced | Hedefin savunması -%40, 10s | Warblade Sunder Mark aktifken → -%60 |
| 12 | **Death Wish** | ⚓↑ | Master | 5s: HP 1 altına düşemez, Fury ×3 hızlı dolar | Fury %100'e ulaşırsa → [V] Burst anında tetiklenebilir |

**Build Eksenleri:** "Glass Cannon" (Reckless Swing+Bloodlust Strike+Blood-Drunk Leap+Death Wish) / "Fury Engine" (Undying Tenacity+Bloodthirst+Carnage Spin+Shatter Armor) / "Crowd Crusher" (Iron Grab+Barbaric Charge+Bloodied Roar+Frenzied Leap)

---

## ⚔️ 6. RONİN ✦ YENİ — Faz 3

> **[S41] KÜÇÜK REVİZYON** — Mille Feuille Cut→Sōken-giri, Blade Veil→Sakura Veil. VFX: mavi blur ↓, beyaz/silver edge + yumuşak motion line.

**Core Fantasy:** "Çek. Kes. Bitir." — BDO Musa esinli iaido katana
**Kaynak:** Draw Tension (0-100) — hareket halinde +20/sn, Quickdraw'da +30, 3s hareketsiz = -30/sn *(savaş dışında — oda temizse — drain yok)*. Tension 100: sonraki Quickdraw ×2 hasar
**[V] Dolum (Flow Cut):** Mükemmel deflectler (Sakura Veil timing), temiz stance çıkışları (Iaido Stance sonrası anında Quickdraw), hatasız hareket sekansları. Tension birikiminden farklı ritim: hassasiyet anları.
**[V] Burst:** MUGEN NO KIRI — V Meter dolunca: 5s her input instant draw-cut, CD yok, cut anlarında iframes

**🖱️ Temel:**
- **LMB — Sheath Walk:** Hareket halinde hafif slash, Tension+5. 3 ardışık = öne kısa atılım + güçlü son darbe. Quickdraw Slash'tan farkı: iframes yok, skill slot tutmuyor, ama her zaman mevcut.
- **RMB — Drawn Edge:** Anlık kın çekimi, Tension'ın %20'sini harcar → önündeki 3m'ye hızlı slash. Tension 60+: 2. slash otomatik eklenir. 0.5s basılı tutulursa → "hazırlık duruşu": bu sürede gelen saldırı otomatik Sakura Veil'a dönüşür (Tension+20 bonus). CD 1.8s. *Tension harcaması + anlık deflect fırsatı — Ronin'in tanımlayıcı ikinci hareketi.*
- **Dash Attack — Iaido Blur:** Dash sonrası 0.4s içinde LMB → hızlı kın çekimi, Tension'ın %30'unu boşaltır = tek yüksek hasar vuruş. Tension 80+: guarantee crit. Skill CD'lerine dokunmaz.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Quickdraw Slash** ★ | ▶⚡ | Core | Anlık katana çekimi, %180 ST hasar | Tension 80+ → +%100 hasar, 0.3s hitstop |
| 2 | **Haste Dash** | ▶↑ | Core | İleri slide, geçilen düşmanlara %120, afterimage bırakır. *Tension bonusları yalnızca Iai Pressure aktifken uygulanır* | Afterimage 1s içinde vurulursa → kopyası da vurur (+%80) |
| 3 | **Sōken-giri** [S41] | ⬡⚡ | Core | 3m önde 5 slash fan, her biri %80 AoE | Tension 60+ → 7 slash'a çıkar |
| 4 | **Iaido Stance** | ✦⚓ | Core | 0.8s stance: hareketsiz durulsa bile Tension dolar, sonraki Quickdraw guaranteed crit | Quickdraw ile çıkılırsa → Iaido CD sıfır |
| 5 | **Wind Step** | ▶⬡ | Core | 3 hızlı yön değişimi + her adımda kısa slash, Tension +10/adım | 3 yönde tamamlanırsa → Tension +40 bonus |
| 6 | **Counter Draw** | ↑⚡ | Core | 0.5s pencere: gelen saldırıyı karşıla → %200 hasar + knockback, Tension +30 | Warblade Iron Counter ile aynı anda → knockback 2×, her ikisi ayrı tetiklenir |
| 7 | **Phantom Step** | ⬡⚓ | Core | 3 konuma afterimage bırak 3s; düşmanlar afterimage'a saldırırken Ronin konumunu gizler *(tactical deception — MMO aggro değil)* | Summoner dual: afterimage minyon sayılır, feda edilebilir |
| 8 | **Sakura Veil** [S41] | ⚓↑ | Core | 1s deflect penceresi, petal swirl + tek sakit cycle. Tension +20/deflect | Deflect sırasında Quickdraw → +%150 hasar |
| 9 | **Crescent Arc** | ⬡💥 | Advanced | Daire çizerek geçme, çizgideki her düşmana %140, son hedefe %280 | 4+ hedef → Tension anında 100'e çıkar |
| 10 | **Flash Draw** | ⚡▶ | Advanced | Görüş alanındaki en yakın 3 düşmana ışınla-kes, her biri %160 | Son hedef HP<%30 → %200 execute cut |
| 11 | **Iai Pressure** | ✦↑ | Advanced | 6s: her Haste Dash Quickdraw bonusunu taşır | Tension 100'de iken → tüm dashler extra blade wave bırakır |
| 12 | **Void Cleave** | 💥✦ | Master | Tension'ın tamamını boşalt → önündeki 10m koni içindeki tüm düşmanlara %15/Tension hasar, animasyon boyunca dokunulmaz *(yönlü finisher, ekran-wide değil)* | Iaido Stance öncesindeyse → hasar ×2, 1s hitstop |

**Build Eksenleri:** "Iaido Burst" (Quickdraw Slash+Iaido Stance+Tension 100+Void Cleave) / "Phantom Dance" (Haste Dash+Wind Step+Phantom Step+Flash Draw) / "Wave Clear" (Sōken-giri+Crescent Arc+Iai Pressure+Wind Step)

---

## 🔫 7. GUNSLİNGER ✦ YENİ — Faz 3

> **[S41] ORTA REVİZYON** — Bullet Rain→Cursor Storm, Critical Shot→Deadshot, Dead Eye→Rift Grenade. VFX: mor patlama tekrarı ↓, yumruk impact motion blur. Karakter: bone/feather aksesuar + worn leather coat + rift-marked bandana.

**Core Fantasy:** "Dur, nişan al değil — koş, ateş et, bitir."
**Kaynak:** Heat (0-100) — her ateşte +8. 100 = Overheat: 3s hasar +%50 + muzzle flash AoE, ardından 2s forced cooldown (Dual Fire + Fan the Hammer + Cursor Storm kilitli; Rift Dash, Deadshot, Quickdraw kullanılabilir). Heat yönetimi gerçek trade-off
**[V] Dolum (Showtime):** Kontrollü Overheat girişleri, slide kills, iyi zamanlı Heat temizleme. Ateş hacminden farklı ritim: stil + döngü yönetimi.
**[V] Burst:** FULL METAL STORM — V Meter dolunca: 5s position-lock yok, dual-fire, her ateş AoE muzzle flash
**Dash Saldırısı — Crossfire Entry:** Dash sonrası 0.35s içinde LMB: iki ayrılan mermi; 2m içinde düşman varsa yakın namlu patlaması. Hip Shot'tan farkı: lateral snap değil agresif oda girişi. 2 hedefe isabet → Showtime bonus V.

**🖱️ Temel:**
- **LMB — Dual Fire:** İki silahtan eşzamanlı tek mermi, Heat+6 her ateşte. Tıkla ya da basılı tut = otomatik ateş. Hareket kesmeden ateş edilir.
- **RMB — Hip Shot:** Yana kısa kayma + aynı anda tek hedefli mermi, Heat+10. CD 0.8s. Hem konumlanma hem hasar — Gunslinger'ın tanımlayıcı hareketi.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Rift Dash** ★ | ▶⬡ | Core | Öne slide/takla, geçerken iki silahla ateş, 3m AoE %120 | Heat 50+ → mesafe 2×, +2 mermi |
| 2 | **Quickdraw** | ⚡↑ | Core | İki silahla anında tek hedef %180 burst | Rift Dash'tan hemen sonra → guaranteed crit + Heat +20 |
| 3 | **Cursor Storm** [S41] | ⬡↑ | Core | Cursor area, 3s mermi yağışı sadece o noktaya. Heat +30 | Overheat sırasında → %150 hasar, alan 2× |
| 4 | **Deadshot** [S41] | 💥⚡ | Core | Cursor-line tek hedef execute shot. %300 ST | Hedef ≤4m → %450 + knockback |
| 5 | **Smoke Grenade** | ⬡⚓ | Core | 5m duman, içindeki düşman kör+yavaş 3s | Shadowblade dual: duman = stealth trigger |
| 6 | **Fan the Hammer** | ⚓↓ | Core | 1s içinde 6 hızlı ateş, Heat +40 | Overheat'e girilirse → 7. mermi ücretsiz %200 |
| 7 | **Suppression Fire** | ⬡✦ | Core | 4m hat boyunca iter + %80 hasar | Cursor Storm aktifken → hasar %130 |
| 8 | **Rift Grenade** [S41] | ✦⚡ | Core | Cursor zone, gecikmeli patlama. Heat +20 | Hedef stun/root altında → patlama hasarı ×1.5 |
| 9 | **Ricochet** | ⚓⬡ | Advanced | Mermi 3 düşmana sekiyor, her sekişte +%20 hasar | 3 farklı hedefe sekerse → Heat -30, CD sıfırlanır (tam reset yok) |
| 10 | **Reload Dance** | ▶✦ | Advanced | Geri çekilme + reload: tüm skill CD -%20, Heat -30 | Rift Dash sonrası → CD -%40 |
| 11 | **Burning Ammo** | ✦↓ | Advanced | 8s: tüm mermiler ateş DoT | Overheat sırasında → DoT anında patlama, AoE |
| 12 | **Point Blank Execute** | 💥↓ | Master | ≤2m: %400 hasar, Heat anında 100 → Overheat tetikler | Rift Dash'tan hemen sonra VE en az 2 düşmana isabetliyse → %600 hasar |

**Build Eksenleri:** "Heat Engine" (Fan the Hammer+Burning Ammo+Cursor Storm+Full Metal Storm) / "Mobile Assassin" (Rift Dash+Quickdraw+Deadshot+Point Blank Execute) / "Crowd Suppressor" (Suppression Fire+Smoke Grenade+Ricochet+Cursor Storm)

---

## 👊 8. BRAWLER ✦ YENİ — Faz 3

> **[S41] ORTA REVİZYON** — Rush Combo→Combo Chain, Momentum Strike→Pivot Hook. Karakter: dinamic guard pose (sol omuz öne, sol el yüksek, sağ el alt çene). Gauntlet silüeti net. VFX: mor patlama ↓, yumruk impact motion blur + omuz rotasyon ipucu.

**Core Fantasy:** "Durma. Vur. Ritim bul. Tekrar."
**Kaynak:** Charge (0-5) — her vuruşta +1, 3s hareketsiz = sıfırlanır. Her Charge: hasar +%10, hız +%3. 5 Charge = Charged State: **Seçim:** sonraki skill anında +%50 güçlenir (Charge sıfırlanır) VEYA RMB ile Charged State "Overdrive Fuel" olarak bankala → Crowd Hype V meter'a transfer edilir.
**[V] Dolum (Crowd Hype):** Mükemmel Weave timing (0.2s pencere), hava juggleleri, multi-target slam anları, banked Overdrive Fuel. Hit sayısından farklı ritim: timing olayları ve bilinçli erteleme.
**[V] Burst:** OVERDRIVE — V Meter dolunca: 5s: her vuruş Charged State gibi davranır, Charge azalmaz

**🖱️ Temel:**
- **LMB — Jab:** Tek hızlı yumruk, Charge+1. Hızlıca tıklanırsa 4'lü oto-kombo (her hit +1 Charge). Brawler'ın ritim kaynağı — her şey buradan başlar.
- **RMB — Weave:** Kısa yan adım savunma dodge'u. Gelen saldırı bu adım sırasında gelirse Charge+2 bonus. **Perfect timing (0.2s pencere): Charge+2 + 0.3s iframes.** 5 Charge / Charged State varken RMB, Charged State'i "Overdrive Fuel" olarak bankalar ve Crowd Hype V meter'a transfer eder.
- **Dash Attack — Flying Knee:** Dash sonrası 0.5s içinde LMB → hava diz darbesi, küçük stagger + Charge+2. Weave'den farkı: Weave savunma dodge, Flying Knee agresif giriş knocker.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Mach Punch** ★ | ⚡↑ | Core | 4 hızlı yumruk, +4 Charge | 5. Charge = son yumruk +%150 hasar |
| 2 | **Shockwave Slam** | ⬡💥 | Core | Yumruk yere, 4m şok dalgası AoE | Charged State → 6m + iter |
| 3 | **Tornado Kick** | ⬡▶ | Core | 360° döner tekme, +2 Charge | 3+ hedefe → her hedef için +1 Charge bonus |
| 4 | **Combo Chain** [S41] | ▶⚡ | Core | jab→cross→hook→uppercut, 4-frame net pose. 5m atılır, +3 Charge | 5 Charge ile → son vuruş Charged State bonus |
| 5 | **Guard Break** | ↑💥 | Core | Hedef savunma -%40, 6s, +3 Charge | Charged State → -%60 + 1s stun |
| 6 | **Repulse** | ⬡↑ | Core | Çevredeki tüm düşmanları iter, +1 Charge/düşman | 4+ düşman → Charge anında 5 |
| 7 | **Counter Blow** | ↑⚡ | Core | 0.4s pencere: gelen vuruşa %200 karşı punch + Charge +3 | Charged State'deyken → %350 + kısa stun |
| 8 | **Aerial Rave** | ▶⬡ | Core | Düşmanı havaya atar, 3 hava vuruşu, Charge korunur | Warblade War Stomp sonrası → 5 hava vuruşu |
| 9 | **Cyclone Drive** | ⬡⚓ | Advanced | 2s döner hareket, temas edene %100/tur, Charge dolmaya devam | Charged State ile → %150/tur |
| 10 | **Seismic Stomp** | 💥⬡ | Advanced | 6m hat boyunca tüm düşmanlar 1.5s havaya kalkar | Aerial Rave combo → havadakilere +%100 hasar |
| 11 | **Pivot Hook** [S41] | ✦⚡ | Advanced | Footwork-based hook, side step + power punch. Charge sayısı × çarpan: 5 Charge = %500 tek vuruş | Overdrive sırasında → hasar ×1.5 |
| 12 | **Unstoppable Force** | ↑▶ | Master | 4s: Charge azalmaz, hız +%50, her dash = otomatik Combo Chain | Cyclone Drive ile → Charge sıfırlanmaz, Cyclone Drive süre +2s (loop yok) |

**Build Eksenleri:** "Combo Machine" (Mach Punch+Combo Chain+Aerial Rave+Pivot Hook) / "Ground Breaker" (Shockwave Slam+Seismic Stomp+Guard Break+Overdrive) / "Counter Fighter" (Counter Blow+Repulse+Cyclone Drive+Unstoppable Force)

---

## 💀 9. SUMMONER → SUMMONER / NECROMANCER

> **[S41] KÜÇÜK REVİZYON + CODEX EKLEMELERİ** — Rally Cry→Command Beacon. Karakter: lantern + skeleton helm öne çıksın (hood ↓). VFX: soul mavi → cyan + bone white; command line (master→minion) net görünür.

**Core Fantasy:** "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır."
**Kaynak:** Charges (0-4, auto +1/8s; minyon ölünce +1 anında)
**[V] Dolum (Grave Chorus):** Sacrifice detonasyonları (Blood for Power, Mass Sacrifice), komuta edilmiş odaklanmış öldürmeler, aktif cesedin dönüşümleri (Corpse Explosion). Pasif Charge üretiminden farklı ritim: aktif komuta aksiyonları.
**[V] Burst:** ARMY OF THE DEAD — V Meter dolunca: 6s tüm minyonlar +%150, ölümsüz

**🖱️ Temel:**
- **LMB — Command Strike:** Minyon varsa: imlece en yakın düşmana hepsini yönlendirir + Summoner kısa staff darbesi. Minyon yoksa: Summoner'ın kendi saldırısı (%80 hasar). Dual fonksiyonlu — asla boşta kalmaz.
- **RMB — Soul Dart:** Hedefe kısa menzilli ruh mermisi fırlat. İsabet: o düşmanı 6s boyunca **Ruhlanmış Hedef** işaretler — tüm minyonlar bu hedefe öncelikle saldırır, bu hedefe verilen minyon hasarı +%25. Charge+0.5/isabet. CD 3s. Minyon yoksa: %90 hasar + 0.8s slow. *LMB toplu yönlendirme, Soul Dart tekil komuta — farklı taktik ritimler.*
- **Dash Attack — Spirit Surge:** Dash sonrası 0.4s içinde LMB → en yakın 1 minyona retarget + 2s kısa haste boost + mark (4s: minyonlar öncelik verir). Summoner kısa ruh darbesi. Minyon yoksa: %120 staff lunge. +0.5 Charge üretir. Konumlama aracı — Commanding Strike'ın ucuzlaştırılmış kopyası değil.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Raise Skeleton** ★ | ↑▶ | Core | 1 Charge → melee iskelet (max 3) | 3 iskelet → sonraki iskelet +%20 hasar |
| 2 | **Summon Golem** | ⚓⬡ | Core | 2 Charge → 1 tank Golem. HP<%20=patlama AoE | Golem'e Commanding Strike → duvara çarpar, stun |
| 3 | **Command Beacon** [S41] | ✦ | Core | Cursor noktasına minyon kitleme + tüm minyonlar +%20 hasar+hız 10s | Karışık minyon tipleri → bonus +%40 |
| 4 | **Corpse Explosion** | 💥⚡ | Core | Düşman veya minyon cesedini patlatır, AoE | 3+ cesetle → zincir patlama |
| 5 | **Death Nova** | ⚡⬡ | Core | 1 minyonu feda: 8s zehir bulutu | Hexer dual → zehir bulutu Hexer debufflarını yayar |
| 6 | **Commanding Strike** | ✦⬡ | Core | Seçili minyon ×4 hasar+invuln; minyon yoksa Summoner ×2 | Golem'e emir → hedefi duvara çarpar |
| 7 | **Blood for Power** | ↑↓ | Core | Minyon feda → Charge+1 + tüm CD -%30 | 3 minyon feda → Charge+3 + [V] meter +30 |
| 8 | **Bone Shield** | ⚓↑ | Core | 5s: minyon kalkan olur, hasar absorbe → Charge+1 | Golem kalkan → absorbe ×2 |
| 9 | **[YENİ] Soul Siphon Totem** | ↑⚓ | Advanced | 8s totem: 5m çevresindeki düşmanlardan ruh emer, 0.5 Charge/sn üretir | Totem yakınında minyon ölürse → totem patlar, 2 Charge anında |
| 10 | **[YENİ] Mass Sacrifice** | 💥⬡ | Advanced | Tüm aktif minyonları anında feda eder, her biri konumunda %150 AoE patlama | 3+ minyon → düşmanlar 3s grounded, her 2 minyon başına +1 Charge (max +3) |
| 11 | **Dark Pact** | ↑ | Advanced | HP -%12 → Charge olmadan minyon çağır. 15s internal CD (sonsuz sacrifice döngüsü engeli) | Ravager dual → HP kaybı Fury doldurur |
| 12 | **Lich Form** | ✦⚓ | Master | 10s: Summoner ghostal (fiziksel hasar immune, sihirsel/elementel hasar tam alır), minyonlar +%60 | Lich Form sırasında feda → hasar ×3 |

**Build Eksenleri:** "Sacrifice Engine" (Blood for Power+Death Nova+Mass Sacrifice+Command Beacon) / "Army Commander" (Raise Skeleton+Summon Golem+Commanding Strike+Soul Siphon Totem) / "Lich Burst" (Lich Form+Dark Pact+Corpse Explosion+Bone Shield)

---

## 🔮 10. HEXER → HEXER / WARLOCK

> **[S41] KÜÇÜK REVİZYON + CODEX EKLEMESİ** — Soul Bargain (#12)→Blight Sigil (cursor curse zone, basana stack). Karakter: floating sigil sembolleri + soul lantern yeşil (Summoner mavi, Hexer yeşil ayrımı).

**Core Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks (0-10/düşman, 5s decay)
**Faz sistemi:** 0-3 Debuff / 4-6 Pressure (+%20 güç) / 7-9 Overload (+%30 hasar alır) / 10 HEXBLAST (tam ödül)
**[V] Dolum (Dread):** Curse Grasp aktif tutma aksiyonları (hold süresi V üretir). Stack sayısından farklı ritim: fiziksel baskı eylemi, uygulama sayısı değil.
**[V] Burst:** HEX CASCADE — V Meter dolunca: hedefte 10 stack varsa tüm düşmanlara 3 stack kopyalanır; yoksa en yüksek stack'lı hedefe Dread patlaması (stack × hasar)

**🖱️ Temel:**
- **LMB — Hex Bolt:** Hızlı projectile, isabette 1 Hex Stack. Her 3. bolt empowered: 3 stack uygular + 0.3s slow. Overload (7+) hedefte empowered bolt 4 stack uygular. Hızlı ve sürekli — stack'leri biriktirir.
- **RMB — Curse Grasp:** 4m'ye el uzatma. Tek basış: hedefe anında 2 Hex Stack. 0.6s basılı tut: 4 stack + 0.5s immobilize. Basılı tutarken Hexer yerinde sabit ama yönlenebilir. Overload Phase (7+) hedefte: hold süre -%50 (daha hızlı sonuç). CD 2.5s. *Baskı uygulama eylemi — Dread V fill condition bunu ödüllendirir (hold süresi V üretir).*
- **Dash Attack — Curse Step:** Dash sonrası 0.4s içinde LMB → 2.5m çevreye anında 2 Hex Stack. Pressure Phase (4+) hedefte: 3 stack + 0.5s root. Hexer'ın tek yakın mesafe baskı anı.

| # | İsim | Tag | Tier | Efekt | Chain Koşulu → Bonus |
|---|------|-----|------|-------|----------------------|
| 1 | **Corruption** ★ | ▶↑ | Core | Anında 3 stack + 4s orta DoT | Corruption→Agony ard arda → Agony ×2 hızlı |
| 2 | **Agony** | ↑ | Core | Süregelen DoT, 2 stack/tick, durdurulamaz | Pressure Phase → tick 3'e çıkar |
| 3 | **Pandemic** | ✦⚡ | Core | Bir hedefteki TÜM stack'ları yakın düşmanlara kopyalar | Overload Phase hedef → kopyalanan stack +3 |
| 4 | **Hexblast** | 💥↓ | Core | **7-9 stack:** azaltılmış hasar (%70/stack), CD sıfırlanmaz. **10 stack:** %100/stack, kill=CD sıfır, yakına 2 stack yayılır | 3+ hedef Pressure Phase → zincirlenerek tüm odaya |
| 5 | **Empathy** | ⚡⬡ | Core | Lanet: saldırıların %30'u kendine döner. Her refleksiyon → +1 Hex Stack (max 2 stack/sn) | Overload Phase → dönen hasar %60 |
| 6 | **Haunt** | ↑⬡ | Core | Hayalet bağla: takip+tick+3 stack, 10=otomatik Hexblast | Veil Burst (Shadowblade dual) → Haunt tüm düşmanlara |
| 7 | **Unstable Affliction** | 💥⚡ | Core | Dispel/heal edilirse → patlama+stun | CC altında hedef → guaranteed full stack |
| 8 | **Enervate** | ⬡✦ | Core | Hız -%50, saldırı hızı -%40, 10s | 5+ stack → süre ×2 |
| 9 | **Mass Hex** | ▶⬡ | Advanced | Görüntüdeki TÜM düşmanlara 2 stack, HP -%8 | Pressure Phase düşmanlar → 3 stack |
| 10 | **[YENİ] Hex Overload** | ⚡💥 | Advanced | 6s: hedef Pressure Phase (4+) veya üstündeyken aldığı her hasar +1 stack kazandırır (max 2 stack/sn, per-target cap), bu sürede Hexblast ×2 hasar | Corruption sonrası → süre 10s'ye çıkar |
| 11 | **Cursed Mirror** | ⚡↑ | Advanced | 8s: sana uygulanan her debuff → düşmana %100 güçle yansır | Enervate sana uygulanırsa → düşman kendi slow'unu alır |
| 12 | **Blight Sigil** [S41] | 💥↓ | Master | Cursor curse zone — basana stack biriktirir. HP -%8 → hedefe anında 3 stack | Hedef zaten Pressure Phase (4-6) → Overload Phase'e zorlar (7'ye iter), Hexblast tetiklenmez |

**Build Eksenleri:** "Patient DoT" (Corruption+Agony+Pandemic+Hexblast) / "Hex Overload" (Hex Overload+Empathy+Mass Hex+Hexblast) / "Soul Burst" (Blight Sigil+Haunt+Unstable Affliction+Hex Overload)

---

## 🗡️ POST-LAUNCH: LANCER (Mızrak) — KONSEPT

**Neden post-launch:** Faz 3 zaten 4 class taşıyor. Ama niche tamamen boş: kimse optimal mesafe oynamıyor.

**Core Fantasy:** "Yaklaşma. Uzakta kal. İkisi de ölüm."
**Kaynak:** Flow (0-100) — 3-5m arası düşman = +15/sn; <2m = −10/sn; >6m = yalnız throw'dan giriyor. Flow 100: sonraki saldırı ×2 hasar + 30 Flow iade

**[V] Burst:** SPEAR STORM — Flow 100: 5s odadaki herkesten hızla geçiş, enerji izi bırakır

**🖱️ Temel:**
- **LMB — Thrust:** Hızlı mızrak saplaması. 2-4m = tam hasar. <1m = küt pommel (yarı hasar). 3 ardışık → spinning sweep (AoE).
- **RMB — Throw & Recall:** Mızrağı fırlat (2-6m), isabet = kısa impale. 0.5s sonra geri çağırılır. Throw sırasında LMB = mızrağa dash.

**Placement:** Tempest + Hemomancer + Lancer = 3 post-launch DLC paketi

---

## ~~9. HIZ/MOMENTUM~~ → KALDIRILDI (Ronin ile değiştirildi)

---

## ~~10. HEMOMANCER~~ → POST-LAUNCH DLC

---

## ÖZET — FİNAL KARARLAR (2026-04-14 v3 audit update)

### v3 Değişiklik Özeti
| Class | Cerrahi Fix | Ek Mikro-Buff |
|---|---|---|
| Warblade | Deep Wound Rage+20 → +35 | RMB Rage Outlet eşiği 40+ → 30+ |
| Elementalist | Mirror Image 2 → 3 kopya | — |
| Shadowblade | Kidney Shot 5CP → 3CP min gate | — |
| Ranger | Flare Focus+20 → +40 | RMB Tactical Roll Focus+8 → +10 |
| Ravager | Intimidating Shout fear → stagger | Iron Grab menzili ≤2m → ≤3m |
| Ronin | Phantom Step 2 → 3 afterimage | Iaido Stance 1s → 0.8s |
| Gunslinger | Smoke Grenade 3m → 5m | — |
| Brawler | Guard Break +2 → +3 Charge | — |
| Summoner | Bone Shield 3s → 5s | — |
| Hexer | Cursed Mirror %50 → %100 | Soul Bargain HP-%25 → -%20 |

**İzleme flagları (Faz 2 playtest):**
- Brawler: Rotation-Lock HIGH — Empower vs Overdrive fork ayrımı yeterince net mi?
- Hexer: Rotation-Lock HIGH — Patient DoT lineer baskısı kırılabiliyor mu?

---

## ÖZET — FİNAL KARARLAR (2026-04-11)

| # | Karar | Sonuç |
|---|-------|-------|
| 1 | İsim teması | **Seçenek C (Hybrid)** — Warblade, Elementalist, Shadowblade, Ranger, Ronin, Gunslinger, Ravager, Brawler, Summoner, Hexer |
| 2 | Warblade ismi | **Warblade** — korunuyor |
| 3 | Musa-tipi hız class | **Ronin** — Draw Tension kaynağı, BDO Musa esinli iaido |
| 4 | Hız class ismi | Çözüldü: **Ronin** |
| 5 | Sınıf sayısı | **10** — Lancer+Crusader kaldırıldı, Ronin+Gunslinger+Brawler eklendi. Post-launch: Tempest+Hemomancer |
| 6 | Tüm class hız kuralı | **Hiçbir class yavaş hissettirmez.** Ağır hissi görselden gelir, input'tan değil |
| 7 | Skill anim workflow | **3-segment:** PEAK önce → START→PEAK → PEAK→END → birleştir |

---

*Bu belge kapalıdır. Yeni class kararları MASTER_KARAR_BELGESI.md'ye gider.*

## ~~ESKI BÖLÜMLER (ARŞİV)~~

*(Momentum ve Hemomancer detayları BACKUP'ta saklanıyor — post-launch DLC için)*
