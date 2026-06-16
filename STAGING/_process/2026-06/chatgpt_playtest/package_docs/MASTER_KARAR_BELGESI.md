# RIMA — MASTER KARAR BELGESİ
> Son güncelleme: 2026-04-30
> Bu belge Claude tarafından karar doğrulama için okunur. Detaylı tasarımlar ilgili faz dosyalarında.

---

## ONAYLANAN KARARLAR

| # | Karar | Sonuç | Tarih |
|---|-------|-------|-------|
| 1 | Crusader → **KALDIRILDI** | Lancer ile birlikte Ronin+Brawler ile değiştirildi. | 2026-04-11 |
| 2 | Lancer → **KALDIRILDI** | Fantasy oturmadı. Ronin ile değiştirildi. | 2026-04-11 |
| 3 | **Gunslinger** ekleniyor | İkili tabanca. BDO Archer awakening tarzı Rift Dash. ANA class — Faz 3 | 2026-04-09 |
| 4 | Toplam class: **10** | Warblade/Elem/Shadow/Ranger/Ronin/Gunslinger/Ravager/Brawler/Summoner/Hexer | 2026-04-11 |
| 5 | Post-launch: **Tempest + Hemomancer** | DLC/Update olarak gelecek | 2026-04-09 |
| 6 | **Rift Parry → KALDIRILDI** | Dash parry window silindi. Yerine: pasiflerle dodge/deflect seçeneği (şans bazlı veya %100, denge Faz 2'de). | 2026-04-16 |
| 7 | **Fracture Echoes** | Boss varyasyon sistemi. Her boss 5 Echo. Kategori: Arena/Adaptif/Minion/Mekanik/Yeni Faz | 2026-04-09 |
| 8 | Mob kontrolü | Antigravity (Claude) yönetir. 16 benzersiz mob. | 2026-04-09 |
| 9 | İsim teması | Seçenek C (Hybrid) + Ronin, Gunslinger, Brawler | 2026-04-11 |
| 10 | Ses pipeline | ChipTone (SFX) + AudioCraft/AudioGen (ortam, 5080 lokal) + Audacity + REAPER | 2026-04-09 |
| 11 | **Ronin** ekleniyor | BDO Musa tarzı iaido katana. Draw Tension kaynağı. ANA class — Faz 3 | 2026-04-11 |
| 12 | **Brawler** ekleniyor | Kombo yumruk/tekme dövüşçüsü. Charge kaynağı. ANA class — Faz 3 | 2026-04-11 |
| 13 | **Tüm class'lar hızlı** | Hiçbir class yavaş hissettirmez. "Ağır" hissi görselden (animasyon/partikül) gelir, input response'dan değil. Zorunlu bekleme süresi yok. | 2026-04-11 |
| 14 | **Skill anim: 3-segment workflow** | PEAK frame önce üretilir → START→PEAK interpolate → PEAK→END interpolate → birleştir. Detay: MASTER_KARAR_BELGESI #14 notu. | 2026-04-11 |
| 15 | **Shadowblade tam redesign** | WoW Rogue kopyası kaldırıldı. Yeni: Sever (pozisyonel), Rift Scar/collapse geometrisi, void aesthetic. SINIF_VE_SKILL_KARAR_BELGESI v4. | 2026-04-14 |
| 16 | **Elementalist Light elementi** | Arcane kaldırıldı. Light = synthesis state (Fire+Frost Resonance → Lightbreak). S43 canonical Light skill'leri: Prism Beam, Frost Wall, Solar Flare, Radiant Pillar. V Burst: Trinity Storm. Build axis: Radiant Break. | 2026-04-14 |
| 17 | **V Meter ayrımı — tüm classlara** | Her class için ayrı [V] Dolum koşulu eklendi. Zorunlu kural: class resource'dan farklı aksiyon tipi. Dolum isimleri: Dominance/Convergence/Predation/Kill Zone/Carnage/Flow Cut/Showtime/Crowd Hype/Grave Chorus/Dread | 2026-04-14 |
| 18 | **Item System D kilitleme** | Hybrid: Relic (2+1 garantili/run) + Skill Modifier (2+1 garantili/run). Ekipman slot yok, stat bloat yok. Kaynak: Treasure room / mini-boss / elite / shop. | 2026-04-14 |
| 19 | **Ravager V Burst redesign** | BERSERK MODE = kan siklozu (2.5 birim yarıçap, pasif AoE + 0.5s single-target darbe). Kill → +0.8s (max +3s). V dolum: kill chain (Carnage), Fury'den ayrı. | 2026-04-14 |
| 20 | **Brawler rotation fix** | Charged State (5 Charge) iki şekilde harcanabilir: (1) anında +%50 skill güçlendirme, (2) RMB ile "Overdrive Fuel" olarak bankala → Crowd Hype V'ye transfer. | 2026-04-14 |
| 21 | **Hexer rotation fix** | Hexblast 7+ stack'te kullanılabilir. 7-9: %70/stack, CD sıfırlanmaz. 10: tam ödül. Early cashout branch — 10-stack rüyası korunuyor. | 2026-04-14 |
| 22 | **Rift Break sistemi** | V Burst context-based interactive phase. Normal oda: hızlı otomatik. Boss/elite: slow-mo (0.08-0.20x), class'a özel input sekansı. Fail=base hasar, success=empowered. Hedef otomatik (context). Animasyonlar hazır olmadan detay tasarımı yapılmaz. | 2026-04-16 |
| 23 | **Rift Break — class-model eşlemesi** | Warblade/Shadow/Ronin/Ranger=timing chain; Gunslinger/Ravager=charge-release; Elementalist/Hexer=sekans; Brawler=mash+timing; Summoner=minyon yönlendirme. Boss özel: Rift Duel (faz geçişi + execution window). | 2026-04-16 |
| 24 | **Cross-class Tier Unlock** | Secondary class'tan 2 skill alınca Tier 2 cross-class açılır. Act 2 boss sonrası Tier 3 + Cross-class Ultimate açılır. Default temel sinerjiler Act 1 boss sonrası erişilebilir. Animasyonlar üretilmeden Tier 2-3 detayları yazılmaz. | 2026-04-16 |
| 25 | **Meta progression** | Şimdilik yok. Hades-Darkness benzeri kalıcı hub upgrade → Faz 4-5 scope. | 2026-04-16 |
| 26 | **Damage test → harita bağlantısı** | Combat damage testi ileride RuntimeRoomManager + oda tipi (elite/boss/corridor) bağlanacak. Şu an flat test yeterli. Faz 2 scope. | 2026-04-16 |
| 27 | **Echo Imprint sistemi** | Her 3 combat odada 1 Echo Imprint sunusu (Skill Draft'a ek). 3 kategori: Strike Form (LMB), Outlet Form (RMB), Surge Form (Dash/Resource). Max 4/run (act başına 1). Faz 2'den itibaren. | 2026-04-17 |
| 28 | **Tag Sinerji Bonusu** | 6 aktif slot'tan 2 aynı tag → otomatik pasif bonus. Max 2 sinerji aktif. Tam tablo: `TASARIM/ROOM_MECHANICS.md §9`. Draft sonrası anlık hesaplanır. | 2026-04-17 |
| 29 | **Oda sayısı revizyonu** | Act 1: 8-9 oda | Act 2: 9-11 oda | Act 3: 9-11 oda | Final: 5-6 oda. Toplam 31-37 oda + 4 boss. Beklenen run: 55-70 dk. Tam spec: `TASARIM/ROOM_MECHANICS.md`. | 2026-04-17 |
| 30 | **Proje tonu: Fractured Epic** | "Dark fantasy" ifadesi kaldırıldı. Ton: Hades benzeri — dünya kırılmış ama görsel olarak DRAMATIK ve CANLI. Void karanlığına karşı parlak kontrastlar. Grimdark değil. Renkler canlı, karakterler ifadeli. | 2026-04-17 |
| 31 | **Ghost Attack = Opsiyon C** | Her iki trigger noktasında: cross-class skill havuzu (80 skill, 2 slot) VE Z/X secondary skilleri. 12f animasyon, 2-segment, 4 yön. Sprite nötr üretilir, Unity'de MaterialPropertyBlock ile class tint. ~240g toplam 10 class. Tam spec: `GUIDES/GHOST_ATTACK_SPEC.md` | 2026-04-17 |
| 32 | **Mob Armor Variant sistemi** | Hades tarzı 3 tier: Normal (1x HP, kırmızı bar) / Armored (2x, altın→kırmızı) / Heavily Armored (3x, gümüş→altın→kırmızı). Sprite: base hazır → edit-images-v2 ile metalik zırh ekle, 1g/sprite. Act 1 varyantları: Shard Bulwark, Void Juggernaut, Iron Penitent, Chain Executioner, Relic Archon. | 2026-04-17 |
| 33 | **PixelLab Faz Master Rehberi** | Tüm fazlar için tek üretim referansı. Faz 1 tam detay, Faz 2 tam, Faz 3-4 outline. ~~`GUIDES/PIXELLAB_FAZ_MASTER.md`~~ → arşivlendi, yerini `CHARACTER_PROMPT_PIPELINE.md` aldı | 2026-04-17 |
| 34 | **Class cinsiyetleri — 5E/5K kilitlendi** | Erkek: Warblade, Brawler, Ravager, Ronin, Shadowblade. Kadın: Elementalist, Gunslinger, Hexer, Ranger, Summoner. Denge + özgünlük. Gunslinger kadın → trençkot+revolver arketipi klişeden kaçıyor. Hexer kadın → erkek dark wizard generic. | 2026-04-19 |
| 35 | **PixelLab Sprite Pipeline — Session 17 kilitlendi** | #40+#41 ile override edildi. ~~`GUIDES/CHARACTER_IDENTITY_FRAMEWORK.md`~~ → arşivlendi. Güncel spec: `CHARACTER_PROMPT_PIPELINE.md` | 2026-04-19 |
| 36 | **Kamera açısı: Hero Siege style — KİLİTLENDİ** | Tüm playable class sprite üretiminde **High Top-Down** (her iki aşamada). Prompt açı tanımı: `high overhead top-down camera, steep bird's eye view, around 75-80 degree downward angle`. Low Top-Down TERK EDİLDİ (Hades açısı — hedef değil). 8 yön üretim standart. | 2026-04-19 |
| 37 | **Ranger identity — tactical rift hunter** | Dungeon/ruins avcısı. Forest archer DEĞİL. Asimetrik utility silüeti: trap canister + tether spool. Kite-control visual language. Rift hunter arka planı. | 2026-04-19 |
| 38 | **Gunslinger identity — rift-tech dual-pistol duelist** | Western/kovboy arketipi YASAK. Rift-tech dual-pistol, kinetic run-and-gun okuma. Coat/hat silüeti altında kadın okuma korunmalı. | 2026-04-19 |
| 39 | **Helmet scope ayrımı (Gemini vs PixelLab fazı)** | Gemini reference aşaması: helmet yok (yüz okunurluğu şart). PixelLab Warblade framework aşaması: helmet intentional (QC'de kontrol edilir). Bu faz ayrımıdır, çelişki değil. | 2026-04-19 |
| 40 | **Kamera açısı REVİZYON — #36 override** | PixelLab "high top-down" = ~35 derece (ARPG açısı, Diablo 2/PoE). 75-80° PixelLab'de mevcut değil — TERK EDİLDİ. Aktif hedef: 35° ARPG. GDD "75-80°" ifadesi bu kararla override edilir. | 2026-04-20 |
| 41 | ~~**Sprite pipeline REVİZYON — #35 override**~~ ⚠️ Override: session 36 → ChatGPT Image 2 → PixelLab Create Character Pro New. Rehber: `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md` + `GUIDES/PIXELLAB_CREATE_CHARACTER_UI.md` | ~~Aktif pipeline: Gemini → concept PNG → PixelLab "Create from Reference", high top-down, female/male human preset, AI Freedom 0.~~ | 2026-04-23 |
| 42 | **Animasyon: Walk YOK, Run var** | Lokomotion animasyonu Walk değil Run. Idle = interpolate first+last frame aynı. Run = PixelLab Create Character built-in (simple loop). Attack/Dash = 3-segment interpolate workflow. | 2026-04-20 |
| 43 | **Elementalist saç rengi — honey-blonde** | Koyu siyah saç terk edildi. Warm honey-blonde, arkaya topuz, birkaç tel yüzü çerçeveler, dramatik efekt yok. Siyah saç top-down'da koyu cüppe ile kaynıyor, okunmuyordu. Saç aynı zamanda sakin/kontrollü — büyü dalgalanması efekti YOK. | 2026-04-21 |
| 44 | **Gunslinger saç rengi — deep auburn red (kızıl)** | Copper-orange terk edildi. Deep auburn red: koyu, zengin kızıl — orange/copper değil. Elementalist honey-blonde ile kontrast oluşturur, iki kadın class arasında palette ayrımı sağlanır. | 2026-04-21 |
| 45 | **Kamera açısı — PixelLab High Top-Down = ~35° ARPG** | PixelLab Create Character "High Top-Down" modu ~35° Diablo 2/PoE açısı verir. South yönünde yüz görünür, gözler görünebilir — bu tool limiti, değiştirilemez. "Gözler görünmez" kriteri terk edildi. QC kriteri güncellemesi: yönler arası ölçek tutarlı + baş-gövde oranı insan (chibi değil) = PASS. warrior_idle_128.png referans olarak Style Image slotuna yüklenir. | 2026-04-24 |
| 46 | **Run animasyonu: 6 frame, 8 yön, flip yok** | Her yön ayrı üretilir. Flip kullanmak yasak — simetri bozar, silah tarafı değişir. 8 yön × 6 frame = 48 clip per class. | 2026-04-23 |
| 47 | **Animasyon üretim yöntemi** | Run = PixelLab Animate (8 gen direkt). Attack/Skill = KF+Interpolate (3-segment workflow). Single-phase = Animate direkt. | 2026-04-23 |
| 48 | **Death/Hit reaction = 4 yön** | Lokomotion (#46) 8-yön, ama death ve hit reaction animasyonları 4-yön (ileri/geri/sol/sağ). Kısa süreli animasyonlar — köşe yönleri oyuncu okuma açısından kritik değil. Production cost yarıya iniyor. | 2026-04-24 |
| 49 | ~~**8-dir pipeline kilitli (Yol A)**~~ → **#53 ile override edildi** | ~~10 class tamamı 8-dir locomotion~~ → 4 cardinal yön kararı S43'te kilitlendi. | ~~2026-04-24~~ |
| 53 | **4 Cardinal Yön kilitlendi — S/E/N/W** (S43, #49 override) | Animasyon üretimi S/E/N/W — 8 yön DEĞİL. Sebep: RIMA kamerası 30-35° top-down ARPG, Hades gibi izometrik değil. Hades'in 4 diagonal sprite sistemi izometrik kamera için — RIMA'ya uygulanamaz. Last Epoch/D2/Cursemark referansı = cardinal yön sistemi. Runtime: 8 hareket yönü → 4 sprite yönüne 45° threshold mapping + hysteresis (son kardinal yönü koru). **Simetrik class** (Warblade vb.): W = E horizontal flip, West ayrıca üretilmez. **Asimetrik class** (Elementalist — orb tek elde, vb.): W ayrıca üretilir. Mob: 4 cardinal yön, hit/death 4 yön. Toplam üretim: ~180-200 çağrı (class) + ~96 çağrı (mob). | 2026-04-27 |
| 50 | **Game Feel Toggles — Default ON, Settings Opt-Out** | Screen shake, hit stop, low HP vignette, damage numbers, chromatic aberration, motion blur, kill slowmo vb. **default açık**. Oyuncu Settings → Accessibility menüsünden her birini ayrı ayrı kapatabilir. Kaydedilen ayar `PlayerPrefs` (Faz 1) veya `SettingsData` SO. Tasarım her feel feature eklendiğinde toggle eklenmesini zorlar — `IFeelFeature` interface + `FeelSettings` singleton pattern. | 2026-04-24 |
| 51 | **Localization — Day 1 Modular, TR+EN Öncelik** | Tüm UI/dialog/tooltip metinleri **key-based localization sisteminde** tutulur (Unity Localization Package önerilir). Hardcoded string YASAK. Öncelik: Türkçe + İngilizce. Sonradan eklenecek diller (DE/FR/ES/RU/PT/ZH/JP/KR) aynı key sistemine tablo ekleyerek girecek. Font TMP SDF multi-atlas (diakritik + CJK destek). UI Faz 1'den itibaren `LocalizedString` component kullanır — string Literal yazmak code review/QC fail. | 2026-04-24 |
| 54 | **R4 Ulti Toggle + Perfect Condition** | Per-skill Shift+key toggle. Lock ON default. "Resource MAX = ulti" TERK EDILDI → "Perfect Condition triggers empowered cast." Gunslinger = Heat ZERO, Hexer = Stack 10 (target), diger class = Resource MAX. Relock rule: ulti cast sonrasi o skill auto Lock ON. Room start: tum lock'lar ON sifirlanir. Zorunlu HUD: lock icon + armed cue + confirmation cast VFX. Detay: `TASARIM/GLOBAL_REPEAT_RULES.md` | 2026-04-30 |
| 55 | **R4 Brawler State Ownership — Shattered** | Brawler upgraded state = **Shattered** (Brawler Sundered IPTAL). Sundered = sadece Warblade uretir. Brawler Cracked (3 stack) → Shattered. Brawler Glass Strike, Sundered consume edebilir ama uretmez. State ownership lock: `TASARIM/CLASS_STATE_CONTRACT.md` | 2026-04-30 |
| 56 | **R4 Execute Gates — HP gate YASAK** | HP<30% execute tum class'larda YASAK. Her class sadece kendi state gate'ini kullanir (Broken/Sundered/Marked+Trapped/Tension/Scar/Heat ZERO/Hex10). Boss'ta execute yok — damage burst (50-70%) olarak indirgenir. | 2026-04-30 |
| 57 | **R4 Counter Arketip Ayrimi** | 3 counter arketipi keskin ayri tutulur: Warblade = absorb/break (timed block → Broken). Ronin = pre-draw timing (frame-perfect → Opened). Brawler = whiff/evade body movement (dodge into whiff → Off-Balance). Arasinda gecis ve cakisma YASAK. | 2026-04-30 |
| 58 | **R4 Movement Option C** | Space = kisa dash, no state, no damage, resource-neutral. Build'de max 1 skill movement. Skill movement = state-interaction zorunlu. State apply → CD reset YASAK. Space + skill movement i-frame overlap YASAK. | 2026-04-30 |
| 59 | **R4 Pixel Art Constraint — Skill VFX** | Skill kurban-taraf efekti: mevcut hit-react / freeze / slide / overlay / VFX / camera shake / hit-stop ONLY. Custom mob lift/throw/grapple/ragdoll YASAK. Wall-Slammed: fallback Ground-Slammed (wall yok ise Cracked refresh + dust VFX, slide yok). Boss/elite: micro-stagger only, slide yok. Detay: `TASARIM/GLOBAL_REPEAT_RULES.md §7` | 2026-04-30 |
| 52 | **Skill VFX + Projectile Mimarisi — Tüm Classlara Geçerli** | Projectile prefab yapısı: SpriteRenderer + PointLight2D (elemental renge göre) + CircleCollider2D + Rigidbody2D + `ProjectileBase.cs`. **Elemental ışık engine-side, art değil:** Fire=`#FF6A00` intensity 1.2 / Frost=`#A0D8FF` intensity 0.8 / Radiant=`#FFFFCC` intensity 1.5. Işık projectile ile hareket eder, destroy'da kaybolur. **Pixel art gerektiren:** projectile sprite + impact/hit spritesheet (fire/frost/radiant, ~48×48 4-6 frame). **Engine-side (art yok):** Freeze/slow tint = shader MaterialPropertyBlock, DoT göstergesi = Particle System, ground indicator = circle sprite tek renk. **Skill kategorileri:** Projectile / Line-cast / Slow-orb / Ground-AoE / Self-buff / Delayed-explosion — her kategorinin prefab mimarisi farklı, `ProjectileBase.cs` abstract. **Üretim sırası (Faz 2):** Impact spritesheet önce → projectile sprite → ground indicator. Area-control skillerin (Frozen Orb, Frost Wall) anim ihtiyacı yok — tek frame + Unity tween. | 2026-04-27 |

---

## FAZ DAĞILIMI

| Faz | Class'lar | Not |
|-----|----------|-----|
| Faz 1 | Warblade | Demo class |
| Faz 2 | + Elementalist, Shadowblade, Ranger | İlk 4 class |
| Faz 3 | + Ravager, **Ronin**, **Gunslinger**, **Brawler** | 8 class, cross-class pasif + secondary class |
| Faz 4 | + Summoner, Hexer | 10 class tamamlanır |
| Faz 5 | Tüm class'lar — Ultimate, Final Boss, polish | — |
| Post-Launch | + Tempest, Hemomancer | DLC/Update |

---

## ~~RIFT PARRY~~ — KALDIRILDI (Karar #6)

> Universal dash-parry sistemi kaldırıldı. Karar tarihi: 2026-04-16.
> Yerine: class'a özgü parry/deflect skill'leri (Iron Counter, Blade Veil, Predator's Fold, Counter Blow vb.)
> RiftParry.cs + ParryFeedback.cs + ClassParryBonus.cs → `ARCHIVE/`'e taşı.

---

## FRACTURE ECHOES ÖZETİ

- Her boss ilk öldürmede Echo 1, her tekrarda +1 (max 5)
- Toggle ile açılıp kapatılabilir
- Echo 3+ = guaranteed Epic drop
- Echo 5 = cosmetic ödül + lore fragment
- Boss başına: Penitent (5 echo), Echo Twin (5), Fracture Sovereign (5), The Architect (5)
- Echo 5 = yeni faz eklenir bossa

---

## SKILL ANİMASYON — 3-SEGMENT WORKFLOW (#14)

Tüm skill animasyonları (attack, dash, burst, özel skill VFX) için zorunlu üretim sırası:

```
ADIM 1 — PEAK frame üret (en dramatik an: impact, patlama, çekiş anı)
         Edit Image PRO → base sprite + kısa pose prompt → peak keyframe

ADIM 2 — START → PEAK segment
         Interpolate (new) → start=base/idle pose, end=PEAK frame
         Bu = windup / approach

ADIM 3 — PEAK → END segment
         Interpolate (new) → start=PEAK frame, end=recovery/return pose
         Bu = follow-through / recovery

ADIM 4 — Birleştir
         Aseprite'de iki segmenti tek timeline'a koy
         Overlap frame (PEAK) bir kez sayılır, silinmez
```

**Neden PEAK önce:** Peak frame yanlışsa tüm animasyon hatalı. Önce onu kilitle, sonra etrafını doldur.

**Basit animasyonlar (walk/idle/death):** Eski START→END workflow yeterli, 3-segment gereksiz.

**Karmaşık animasyonlar (skill/burst/boss attack):** 3-segment zorunlu.

---

## SES PİPELİNE ÖZETİ

```
ChipTone (SFX) → Audacity (düzenle) → REAPER (master) → Unity AudioMixer
AudioCraft/AudioGen (ortam/müzik, RTX 5080 lokal) → Audacity → REAPER → Unity
```

- SFX toplam: ~169 ses
- Müzik toplam: ~10 track
- 5080 16GB VRAM → AudioGen large model çalışır

---

## GENERATİON MALİYET ÖZETİ (PixelLab)

| Kategori | Gen |
|----------|-----|
| Karakterler (10 class) | ~1,396 |
| Moblar (16 mob) | ~1,272 |
| Mevcut mob iyileştirme | ~350 |
| Bosslar (4 boss) | ~1,320 |
| VFX/Tileset/UI | ~910 |
| Buffer (%15) | ~790 |
| **GRAND TOTAL** | **~6,038 gen** |

Aylık 5000 gen → ~2 ay tüm görsel asset'ler tamamlanır.

---

## CLASS VFX RENK DİLİ (2026-04-11)

Her class'ın görsel kimliği renk + VFX imzasıyla ayrışır. Combat okunabilirliği buradan gelir.

| Class | Ana Renk | Hex | VFX İmzası |
|-------|----------|-----|------------|
| **Warblade** | Soğuk mavi / çelik | `#66AAFF` | Rift enerji slash, demir kıvılcım, cold blue hit flash |
| **Ravager** | Kan kırmızı / turuncu | `#FF3322` | Öfke aurası, brutal darbe tozu, ısı dalgası |
| **Elementalist** | Element bazlı | Frost=`#00DDFF` Fire=`#FF6600` Light=`#FFFAAA` | Element burst, renk element türüne göre değişir; Light = sıcak beyaz radiant |
| **Shadowblade** | Void mor / rift beyazı | `#9933CC` + `#DDEEFF` | Rift Scar glow (void yara izi), boyut geçiş afterimage, collapse parlama |
| **Ranger** | Doğa yeşili / altın | `#44CC44` + `#FFCC00` | Ok izi, yaprak partikülleri, altın vuruş |
| **Gunslinger** | Sıcak sarı / pirinç | `#FFB800` | Namlu alevi, ısı shimmer, kovan fırlatma |
| **Ronin** | Saf beyaz / void siyah | `#FFFFFF` + `#111122` | Void kesim (siyah-beyaz slash), temiz parlama |
| **Brawler** | Turuncu / amber | `#FF8800` | Toz bulutu, shockwave halkası, darbe sismik |
| **Summoner** | Nekro yeşili / kemik | `#22FF88` + `#DDCCAA` | Minyon glow, kemik parçaları, çağırma dairesi |
| **Hexer** | Hasta sarı / bozulma | `#CCFF00` | Lanet işareti, vücut distorsiyon, sickly pulse |

**Karışma riski çözümleri:**
- Warblade (soğuk mavi) vs Ravager (sıcak kırmızı): tam zıt ısı rengi
- Shadowblade (mor + zehir) vs Ronin (beyaz + void): kirli vs temiz palet
- Summoner (yeşil swarm) vs Hexer (sarı tek hedef): kalabalık vs tekil VFX yoğunluğu
- Brawler: sihir yok — sadece fiziksel toz/dalgası → tüm sihir class'larından ayrışır

**LightPulse renkleri (per class hit):**
```csharp
Warblade:    LightPulse.Emit(new Color(0.4f, 0.7f, 1.0f), 1.5f, 0.10f);  // cold blue
Ravager:     LightPulse.Emit(new Color(1.0f, 0.2f, 0.1f), 1.5f, 0.10f);  // blood red
Elementalist (fire): LightPulse.Emit(new Color(1.0f, 0.4f, 0.0f), 2.0f, 0.15f); // orange
Elementalist (frost): LightPulse.Emit(new Color(0.0f, 0.9f, 1.0f), 1.8f, 0.15f); // cyan
Shadowblade: LightPulse.Emit(new Color(0.6f, 0.2f, 0.8f), 1.4f, 0.09f);  // void purple + rift afterimage
Ronin:       LightPulse.Emit(new Color(0.9f, 0.9f, 1.0f), 2.0f, 0.06f);  // sharp white
```

---

## HENÜZ TASARLANMAMIŞ (Claude: gelecek session'da tamamla)

- [ ] ~~Lancer~~ → POST-LAUNCH DLC (skip)
- [ ] Gunslinger 12 skill chain koşulları detaylandırılacak
- [ ] ~~Crusader~~ → KALDIRILDI (skip)
- [ ] Brawler 12 skill chain koşulları detaylandırılacak (v6'da temel var)
- [ ] Ronin 12 skill chain koşulları gözden geçirilecek
- [ ] 10 class cross-class pasif detayları (matris var, efektler yazılmalı)
- [ ] Cross-class Ultimate tam mekanik açıklamaları
- [ ] Legendary tier skill varyasyonları
- [ ] Fracture Echoes: loot tablosu, toggle UI
- [ ] Grudge sistemi FAZ dosyalarına işlenecek
- [ ] AudioCraft kurulumu ve test (5080'de)
- [ ] Gunslinger karakter tasarımı (PixelLab prompt) — Lancer/Crusader KALDIRILDI
