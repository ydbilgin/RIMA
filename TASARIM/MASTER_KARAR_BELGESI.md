---
status: LOCKED
faz: 1
tarih: 2026-05-14
ozet: "Tüm RIMA tasarım kararlarının canonical listesi"
---
# RIMA — MASTER KARAR BELGESİ
> Son güncelleme: 2026-05-13
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
| 29 | ~~**Oda sayisi revizyonu**~~ -> **#62 ile override** | ~~Act 1: 8-9 oda~~ -> Act 1 = 15 node (Karar #62). Act 2/3/Final oda sayilari henuz revize edilmedi. | 2026-04-17 |
| 30 | **Proje tonu: Fractured Epic** | "Dark fantasy" ifadesi kaldırıldı. Ton: Hades benzeri — dünya kırılmış ama görsel olarak DRAMATIK ve CANLI. Void karanlığına karşı parlak kontrastlar. Grimdark değil. Renkler canlı, karakterler ifadeli. | 2026-04-17 |
| 31 | **Ghost Attack → Shadow Echo + F-Slot Migration** (Karar #60 + #24 ile uyumlandırıldı) | ~~Z/X secondary~~ REVOKED (Karar #60, 2026-05-06). **Ghost Attack MECHANIC korunur:** cross-class skill kullanıldığında secondary class'ın phantom'unun animasyonu Shadow Echo aura+phantom+UI flash katmanlarına entegre edilir. **Cross-class havuzu (80 skill, 2 slot)** mevcut keybind sisteminde F slot içinde (LMB/RMB/QERF+V+Space, Karar #60). **Tier unlock** Karar #24'e uygun: Tier 2 Act 2 boss sonrası, Tier 3 Cross-class Ultimate Faz 4 (varsa). 12f animasyon Shadow Echo phantom layer'ında render. Sprite nötr üretilir, MaterialPropertyBlock class tint. ~240g toplam 10 class. Tam spec: `GUIDES/GHOST_ATTACK_SPEC.md` (Shadow Echo entegrasyon revize edilecek). | 2026-04-17 → 2026-05-13 REVISED |
| 32 | **Mob Armor Variant sistemi** | Hades tarzı 3 tier: Normal (1x HP, kırmızı bar) / Armored (2x, altın→kırmızı) / Heavily Armored (3x, gümüş→altın→kırmızı). Sprite: base hazır → edit-images-v2 ile metalik zırh ekle, 1g/sprite. Act 1 varyantları: Shard Bulwark, Void Juggernaut, Iron Penitent, Chain Executioner, Relic Archon. | 2026-04-17 |
| 33 | **PixelLab Faz Master Rehberi** | Tüm fazlar için tek üretim referansı. Faz 1 tam detay, Faz 2 tam, Faz 3-4 outline. ~~`GUIDES/PIXELLAB_FAZ_MASTER.md`~~ → arşivlendi, yerini `CHARACTER_PROMPT_PIPELINE.md` aldı | 2026-04-17 |
| 34 | **Class cinsiyetleri — 5E/5K kilitlendi** | Erkek: Warblade, Brawler, Ravager, Ronin, Shadowblade. Kadın: Elementalist, Gunslinger, Hexer, Ranger, Summoner. Denge + özgünlük. Gunslinger kadın → trençkot+revolver arketipi klişeden kaçıyor. Hexer kadın → erkek dark wizard generic. | 2026-04-19 |
| 35 | **PixelLab Sprite Pipeline — Session 17 kilitlendi** | #40+#41 ile override edildi. ~~`GUIDES/CHARACTER_IDENTITY_FRAMEWORK.md`~~ → arşivlendi. Güncel spec: `CHARACTER_PROMPT_PIPELINE.md` | 2026-04-19 |
| 36 | **Kamera açısı: Hero Siege style — KİLİTLENDİ** | Tüm playable class sprite üretiminde **High Top-Down** (her iki aşamada). Prompt açı tanımı: `high overhead top-down camera, steep bird's eye view, around 75-80 degree downward angle`. Low Top-Down TERK EDİLDİ (Hades açısı — hedef değil). 8 yön üretim standart. | 2026-04-19 |
| 37 | **Ranger identity — tactical rift hunter** | Dungeon/ruins avcısı. Forest archer DEĞİL. Asimetrik utility silüeti: trap canister + tether spool. Kite-control visual language. Rift hunter arka planı. | 2026-04-19 |
| 38 | **Gunslinger identity — rift-tech dual-pistol duelist** | Western/kovboy arketipi YASAK. Rift-tech dual-pistol, kinetic run-and-gun okuma. Coat/hat silüeti altında kadın okuma korunmalı. | 2026-04-19 |
| 39 | **Helmet scope ayrımı (Gemini vs PixelLab fazı)** | Gemini reference aşaması: helmet yok (yüz okunurluğu şart). PixelLab Warblade framework aşaması: helmet intentional (QC'de kontrol edilir). Bu faz ayrımıdır, çelişki değil. | 2026-04-19 |
| 40 | **Kamera açısı REVİZYON — #36 override** | PixelLab "low top-down" = ~35 derece (ARPG açısı, Diablo 2/PoE). 75-80° PixelLab'de mevcut değil — TERK EDİLDİ. Aktif hedef: 35° ARPG. GDD "75-80°" ifadesi bu kararla override edilir. | 2026-04-20 |
| 41 | ~~**Sprite pipeline REVİZYON — #35 override**~~ ⚠️ Override: session 36 → ChatGPT Image 2 → PixelLab Create Character Pro New. ~~Rehber: `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md` + `GUIDES/PIXELLAB_CREATE_CHARACTER_UI.md`~~ (dosyalar silindi — aktif spec: `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`) | ~~Aktif pipeline: Gemini → concept PNG → PixelLab "Create from Reference", high top-down, female/male human preset, AI Freedom 0.~~ | 2026-04-23 |
| 42 | **Animasyon: Walk YOK, Run var** | Lokomotion animasyonu Walk değil Run. Idle = interpolate first+last frame aynı. Run = PixelLab Create Character built-in (simple loop). Attack/Dash = 3-segment interpolate workflow. | 2026-04-20 |
| 43 | **Elementalist saç rengi — honey-blonde** | Koyu siyah saç terk edildi. Warm honey-blonde, arkaya topuz, birkaç tel yüzü çerçeveler, dramatik efekt yok. Siyah saç top-down'da koyu cüppe ile kaynıyor, okunmuyordu. Saç aynı zamanda sakin/kontrollü — büyü dalgalanması efekti YOK. | 2026-04-21 |
| 44 | **Gunslinger saç rengi — deep auburn red (kızıl)** | Copper-orange terk edildi. Deep auburn red: koyu, zengin kızıl — orange/copper değil. Elementalist honey-blonde ile kontrast oluşturur, iki kadın class arasında palette ayrımı sağlanır. | 2026-04-21 |
| 45 | **Kamera açısı — PixelLab Low Top-Down = ~35° ARPG** | PixelLab Create Character "Low Top-Down" modu ~35° Diablo 2/PoE açısı verir. South yönünde yüz görünür, gözler görünebilir — bu tool limiti, değiştirilemez. "Gözler görünmez" kriteri terk edildi. QC kriteri güncellemesi: yönler arası ölçek tutarlı + baş-gövde oranı insan (chibi değil) = PASS. warrior_idle_128.png referans olarak Style Image slotuna yüklenir. | 2026-04-24 |
| 46 | ~~**Run animasyonu: 6 frame, 8 yön, flip yok**~~ → Karar #53 ile override | ~~Her yön ayrı üretilir. Flip kullanmak yasak — simetri bozar, silah tarafı değişir.~~ **8 yön REVOKED 2026-04-27 Karar #53'e taşındı** — 4 cardinal yön (S/E/N/W, W=flipX simetrik class için, asimetrik class W ayrı). 4 × 6 frame = 24 clip per simetrik class. v1 MVP standardı. | 2026-04-23 |
| 47 | **Animasyon üretim yöntemi** | Run = PixelLab Animate (8 gen direkt). Attack/Skill = KF+Interpolate (3-segment workflow). Single-phase = Animate direkt. | 2026-04-23 |
| 48 | **Death/Hit reaction = 4 yön** | Lokomotion (#46) 8-yön, ama death ve hit reaction animasyonları 4-yön (ileri/geri/sol/sağ). Kısa süreli animasyonlar — köşe yönleri oyuncu okuma açısından kritik değil. Production cost yarıya iniyor. | 2026-04-24 |
| 49 | ~~**8-dir pipeline kilitli (Yol A)**~~ → **#53 ile override edildi** | ~~10 class tamamı 8-dir locomotion~~ → 4 cardinal yön kararı S43'te kilitlendi. | ~~2026-04-24~~ |
> **REVOKED 2026-05-13:** Karar #114 ile 8 yön LOCKED; bu karar pasifize.
| 53 | **4 Cardinal Yön kilitlendi — S/E/N/W** (S43, #49 override) | Animasyon üretimi S/E/N/W — 8 yön DEĞİL. Sebep: RIMA kamerası 30-35° top-down ARPG, Hades gibi izometrik değil. Hades'in 4 diagonal sprite sistemi izometrik kamera için — RIMA'ya uygulanamaz. Last Epoch/D2/Cursemark referansı = cardinal yön sistemi. Runtime: 8 hareket yönü → 4 sprite yönüne 45° threshold mapping + hysteresis (son kardinal yönü koru). **Simetrik class** (Warblade vb.): W = E horizontal flip, West ayrıca üretilmez. **Asimetrik class** (Elementalist — orb tek elde, vb.): W ayrıca üretilir. Mob: 4 cardinal yön, hit/death 4 yön. Toplam üretim: ~180-200 çağrı (class) + ~96 çağrı (mob). | 2026-04-27 |
| 50 | **Game Feel Toggles — Default ON, Settings Opt-Out** | Screen shake, hit stop, low HP vignette, damage numbers, chromatic aberration, motion blur, kill slowmo vb. **default açık**. Oyuncu Settings → Accessibility menüsünden her birini ayrı ayrı kapatabilir. Kaydedilen ayar `PlayerPrefs` (Faz 1) veya `SettingsData` SO. Tasarım her feel feature eklendiğinde toggle eklenmesini zorlar — `IFeelFeature` interface + `FeelSettings` singleton pattern. | 2026-04-24 |
| 51 | **Localization — Day 1 Modular, TR+EN Öncelik** | Tüm UI/dialog/tooltip metinleri **key-based localization sisteminde** tutulur (Unity Localization Package önerilir). Hardcoded string YASAK. Öncelik: Türkçe + İngilizce. Sonradan eklenecek diller (DE/FR/ES/RU/PT/ZH/JP/KR) aynı key sistemine tablo ekleyerek girecek. Font TMP SDF multi-atlas (diakritik + CJK destek). UI Faz 1'den itibaren `LocalizedString` component kullanır — string Literal yazmak code review/QC fail. | 2026-04-24 |
| 54 | **R4 Ulti Toggle + Perfect Condition** | Per-skill Shift+key toggle. Lock ON default. "Resource MAX = ulti" TERK EDILDI → "Perfect Condition triggers empowered cast." Gunslinger = Heat ZERO, Hexer = Stack 10 (target), diger class = Resource MAX. Relock rule: ulti cast sonrasi o skill auto Lock ON. Room start: tum lock'lar ON sifirlanir. Zorunlu HUD: lock icon + armed cue + confirmation cast VFX. Detay: `TASARIM/GLOBAL_REPEAT_RULES.md` | 2026-04-30 |
| 55 | **R4 Brawler State Ownership — Shattered** | Brawler upgraded state = **Shattered** (Brawler Sundered IPTAL). Sundered = sadece Warblade uretir. Brawler Cracked (3 stack) → Shattered. Brawler Glass Strike, Sundered consume edebilir ama uretmez. State ownership lock: `TASARIM/CLASS_STATE_CONTRACT.md` | 2026-04-30 |
| 56 | **R4 Execute Gates — HP gate YASAK** | HP<30% execute tum class'larda YASAK. Her class sadece kendi state gate'ini kullanir (Broken/Sundered/Marked+Trapped/Tension/Scar/Heat ZERO/Hex10). Boss'ta execute yok — damage burst (50-70%) olarak indirgenir. | 2026-04-30 |
| 57 | **R4 Counter Arketip Ayrimi** | 3 counter arketipi keskin ayri tutulur: Warblade = absorb/break (timed block → Broken). Ronin = pre-draw timing (frame-perfect → Opened). Brawler = whiff/evade body movement (dodge into whiff → Off-Balance). Arasinda gecis ve cakisma YASAK. | 2026-04-30 |
| 58 | **R4 Movement Option C** | Space = kisa dash, no state, no damage, resource-neutral. Build'de max 1 skill movement. Skill movement = state-interaction zorunlu. State apply → CD reset YASAK. Space + skill movement i-frame overlap YASAK. | 2026-04-30 |
| 59 | **R4 Pixel Art Constraint — Skill VFX** | Skill kurban-taraf efekti: mevcut hit-react / freeze / slide / overlay / VFX / camera shake / hit-stop ONLY. Custom mob lift/throw/grapple/ragdoll YASAK. Wall-Slammed: fallback Ground-Slammed (wall yok ise Cracked refresh + dust VFX, slide yok). Boss/elite: micro-stagger only, slide yok. Detay: `TASARIM/GLOBAL_REPEAT_RULES.md §7` | 2026-04-30 |
| 60 | **Skill System Taxonomy LOCKED** | 4 aktif tip: STRIKE / ZONE / REACTIVE / STATE. 3 pasif tip: KEYSTONE (zorunlu 1) / MODIFIER (2) / RESONANCE (1). Upgrade: skill basina max 3, draft-only, REPLACE prompt. Identity Passive: her class 1 sabit, upgrade edilemez. Cross-Family Carrier: sadece Legendary upgrade, tag pip only, sigil yasak. Summoner/Hexer: alt-tag (summon/accumulation), yeni tip yok. Ghost Attack: sadece STRIKE, summon tag excluded. Detay: `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md` | 2026-05-06 |
| 61 | **Dungeon Architecture KEEP — Hades-style** | Acik dunya / Diablo paradigm REJECTED. Hades-style discrete room flow + StS macro graph hibrit modeli LOCKED. Combat v1 commit-windowing + Cross-Class 3-tag Resonance v2 ayrik arena + kapi kilidi gerektiriyor. Detay: `TASARIM/dungeon_act1_map.md`. | 2026-05-09 |
| 62 | **Act 1 Map LOCKED v1 (15 node) — #29 OVERRIDE** | ~~Karar #29~~ (8-9 oda) override edildi. Act 1 = 15 node: 1 Entry + 6 Combat + 2 Elite + 2 Rest (F1->F2, F2->F3 gecis) + 1 Shop + 1 Curse Gate (dal) + 1 Mystery (dal) + 1 Boss. Topoloji sabit, icerik random (RoomRegistry pool, mob composition, reward draft). Detay: `TASARIM/dungeon_act1_map.md`. | 2026-05-09 |
| 63 | **Map Fragment + Kirik Tas Tablet Sistemi LOCKED v1** | StS2-style harita parcasi reveal sistemi. Combat/Elite/Mystery oda temizlenince fragment duser (cyan glow + bobbing), G ile pickup zorunlu. Reveal: standart 1 node ileri %65 / 2 node %30 / 3 node %5. Acik node bonus: +1 hop ileri acar. Onun puslu = garanti drop, aciksa olasilikli drop. Boss kapisi 8 fragment (combat 6 + elite 2). Map UI metaforu: "Kirik Tas Tablet" — soyut grid + pasli altin cerceve + cyan rift catlaklari. Tablet 4 Act'te gorsel evrim gecirir (Act1: kale oymalari -> Act2: damarli et -> Act3: yuzen parcalar -> Act4: ayna). Hibrit UI: TAB MapPanel (StS-style) + sol-ust MiniMap (Hades-style 128x128). Detay: `TASARIM/map_fragment_system.md`. Q1-Q5 kapatildi: Rest fragment YOK / Mystery AUTHORED events / Spirit v1'de YOK / Map UI HIBRIT / Boss 8 fragment. | 2026-05-09 |
| 64 | **ActionCommitProfile 5 alan LOCKED v1** | Combat fluidity v1 sprint. BasicAttackProfile genisletilir 5 alanla: windupMs, recoveryMs, dashCancelStartFraction (per-class), hitstopMs, cancelOnWhiff. Alabaster Dawn animation commitment prensibi RIMA'ya tasindi. Detay: AD eval memory/project_alabaster_dawn_eval.md. | 2026-05-09 |
| 65 | **3-Layer Feedback Hierarchy LOCKED v1** | Hit feedback 3 katman: Normal / Commit / Break. Posture break window'da (3s, +50% dmg) gorsel + ses katmani yukselir. Named outcome glyph v1'DE YOK (v2 aday). | 2026-05-09 |
| 66 | **Boolean hasInterruptArmor v1** | Mob/boss posture v1: boolean hasInterruptArmor flag. Sayisal poise meter v2'ye ertelendi. Boss posture kalibrasyon: 450 light boss / 850 heavy boss. showPostureMeter Settings toggle (default ON, AD v2 eklemesi). | 2026-05-09 |
| 67 | **Dash-Cancel Windows Per-Class** | BasicAttackProfile.cancelWindowFraction class-spesifik: Ravager/Shadowblade: 15-25% (early cancel). Ranger/Gunslinger: 30-55% (genisletildi v2 AD eklemesiyle). Warblade/Brawler: 60-75% (commit agirlikli). Casters (Elementalist/Hexer/Summoner): windup not cancellable. | 2026-05-09 |
| 68 | **OnDash Passive Proc (4. pasif tipi)** | Cross-Class Proc system'e OnDash eklendi. CrossClassEffectType.OnDash enum + CrossClassSkillManager.OnDash() method. Shadowblade/Ronin primary kullanicilar. | 2026-05-09 |
| 69 | **Cross-Class Proc Text Feedback** | 3-tag Rift / 2-tag Resonance proc tetiklenince sigil glyph ustune 1 satir 12px text ("Tremor!" / "Severance!" / "Bloodveil!" vb.) + SFX. Death recap + next-run hint UX layer (boss yenilgisi sonrasi, opsiyonel). | 2026-05-09 |
| 70 | **Resonance 2-tag Named Outcomes v2 ADAY** | [!] v2 aday, henuz LOCKED degil. 10 pair: Tremor/Bloodveil/Severance/Crushblood/Resonant Hold/Lockedge/Splinter/Phantom Pulse/Hammerwound/Hemorrhage. Rift 3-tag kurali KORUNUR. v2 onayi sonrasi LOCKED isaretlenecek. | 2026-05-09 |
| 71 | **Silah Gorunurluk: Single-State LOCKED** | Tum siniflarda silah hep elde / hazir pozisyonda — combat-out ayri state YOK, "puf" mekanigi YOK. Istisna sadece **Ronin** (sheath/draw kimligi) ve kismen Gunslinger (kilif'tan cekme). Pixel art constraint + combat-readability + animasyon scope (10 sinif x 4 yon x 6 frame zaten 1800-2000 uretim) bu karari zorluyor. Alabaster Dawn'in 3-state "puf" modeli RIMA'da rejected cunku pixel-art frame teleport bug'lari + solo dev scope kabul edilemez. Hub atmosferi ve cinematic moments icin ayri **Makeup Backlog** + **Cinematic Layer** dosyalari (TASARIM/MAKEUP_BACKLOG.md + CINEMATIC_LAYER_v1.md). 10 sinif konumlandirma: Warblade=sag omuzda, Shadowblade=ters tutus vucuda yakin (phase step'te bile kaybolmaz), Ranger=elde at-rest, Ravager=omuzda, Ronin=KINDA (kimlik), Gunslinger=kilif'ta/yaninda, Brawler=ciplak el (silah yok), Summoner=kristal asa elde, Elementalist=SILAHSIZ (buyu el jestleriyle), Hexer=SILAHSIZ (run eller + bel'de ruh feneri). | 2026-05-09 |
| 52 | **Skill VFX + Projectile Mimarisi — Tüm Classlara Geçerli** | Projectile prefab yapısı: SpriteRenderer + PointLight2D (elemental renge göre) + CircleCollider2D + Rigidbody2D + `ProjectileBase.cs`. **Elemental ışık engine-side, art değil:** Fire=`#FF6A00` intensity 1.2 / Frost=`#A0D8FF` intensity 0.8 / Radiant=`#FFFFCC` intensity 1.5. Işık projectile ile hareket eder, destroy'da kaybolur. **Pixel art gerektiren:** projectile sprite + impact/hit spritesheet (fire/frost/radiant, ~48×48 4-6 frame). **Engine-side (art yok):** Freeze/slow tint = shader MaterialPropertyBlock, DoT göstergesi = Particle System, ground indicator = circle sprite tek renk. **Skill kategorileri:** Projectile / Line-cast / Slow-orb / Ground-AoE / Self-buff / Delayed-explosion — her kategorinin prefab mimarisi farklı, `ProjectileBase.cs` abstract. **Üretim sırası (Faz 2):** Impact spritesheet önce → projectile sprite → ground indicator. Area-control skillerin (Frozen Orb, Frost Wall) anim ihtiyacı yok — tek frame + Unity tween. | 2026-04-27 |
| 72 | **S59 Pivot — Pure 2D Top-Down LOCKED** | 2.5D mimari (3D env + 2D billboard) REVOKED. Pure 2D top-down (Hammerwatch/HLD ref) LOCKED. Karakter 64x64 chibi (eski 128px native + chibi YASAK kararlari REVOKED), tile 32x32 top-down (eski 64x64 floor + 64x128 wall iso REVOKED), VFX 64-128px mix. URP 2D Renderer + Pixel Perfect Camera + 2D Lights. Anim view: high top-down ~30-35° (Hades match KEEP). Mevcut RIMA projesi RESTORE (RIMA_2.5D nested arsivlenecek). 4 yon + flip KEEP. PPU=64. Detay: `MEMORY/project_pure_2d_topdown_pivot_2026-05-12.md` (auto-memory). | 2026-05-12 |
| 73 | **Karakter Silah Entegrasyonu — Silahlı 1-piece LOCKED** | Karakter 64px chibi sınıf-spesifik silahla TEK SPRITE üretilir. Body-only + WeaponAnchorMap + AnimationCurve senkron sistemi REVOKED. Sebep: 64px'te body-only AI variance yuksek, pixel-precise anchor imkansiz; PixelLab Create Character native silahli 1-piece uretiyor; sınıf↔silah sabit (Warblade=kılıç, Ranger=yay, Shadowblade=hançer, Elementalist=asa); referans oyunlar (Hammerwatch/HLD/Tunic) hepsi 1-piece. Karar #71 (silah hep elde single-state) ile uyumlu. Detay: `MEMORY/project_64px_armed_character_locked.md`. | 2026-05-12 |
| 74 | **Boss/Mob Boyut Hiyerarşisi 2^n + PPU=64 Standardize** | Tum sprite PPU=64 (boyut farki sprite canvas ile gelir, PPU manipulasyonuyla DEGIL). 2^n hierarchy: Karakter 64x64, kucuk/orta mob 64x64, elite mob 128x128, miniboss 128x128, Act Boss 256x256, Final Boss 256x256 (sahnede ~2.5x oyuncu = Hades benchmark). Eski Final Boss PPU=32 + Faz 4 = 96px insan formu (boyut degisimi) REVOKED. Boss kimligi mekanik + faz degisimleri + VFX ile gelir, boyut DEGIL. 4 faz mekanik kurali NOT_LOCKED (design sprint bekliyor). Detay: `MEMORY/project_boss_size_hierarchy_2026-05-12.md`. | 2026-05-12 |
| 75 | **PixelLab Map Tools KULLANILMAYACAK** | NLM 2026-05-11 LOCKED kararı: `create_map_object` + `create_topdown_tileset` connected playable room/autotileset output icin KULLANILMAYACAK (Discord lesson: PixelLab tileset output separate tiles, not production-ready connected). RIMA application: small style reference pack uretilir (single tile sprite OK), Unity'de masks + RuleTiles + sockets ile assemble. PixelLab tile generation tek tile bazinda OK, multi-tile connected output YASAK. | 2026-05-12 |
| 76 | **Asset Prompt Format — TYPE/HEAD/BODY/LIMBS** | Karakter + mob + boss asset prompt formati: TYPE/STYLE/HEAD/BODY/LIMBS/EXTRA/CLOTHING/HANDS/SILHOUETTE/COLOR/POSE blok formati. Image #2 (ranger_walk_v1.txt referans) inspired. PixelLab Create Character / Create Image Pro / vary_object icin standart template. 8-direction sheet layout: 3x3 grid, center empty (Image #1 referans). LAYOUT + RULES bloklari sonda (size lock + footprint lock + anchor + anti-aliasing yasak). Detay: `STAGING/asset_production_plan_2026-05-12.md`. | 2026-05-12 |
| 77 | **Vivid Vulnerability Tonal Model** | Chibi 64x64 + Fractured Epic uzlasi = "Vivid Vulnerability": kucuk canli kahraman / monumental hostile sistem kontrasti. Hades theatrical mythic + Salt and Sanctuary chibi-but-serious primary referans. HLD dread / Hammerwatch ironic-cute / Don't Starve macabre-whimsical CAUTION (primary degil). Drama scale-contrast + color-vividness'tan gelir, restraint/silence'tan DEGIL. Cute roguelite (joke names, cozy tavern) YASAK. Grimdark despair YASAK. Detay: `TASARIM/chibi_lore_integration_decision_2026-05-13.md`. | 2026-05-13 |
| 78 | **Premium Cinematic Portrait Tier — v1 Scope CUT** | Portrait illustration tier (~512x768, PixelLab Pro base + manual paint-over) v1'de SADECE: 1 Architect reveal portrait + 1 ending illustration (max 2). NPC hub portrait modali CUT (chibi+bubble+namecard yeterli). Penitent/Echo Twin/Fracture Sovereign bosslar icin sigil+title-card sistem (no portrait). 8-12 portrait scope post-v1 expansion. Detay: `TASARIM/chibi_lore_integration_decision_2026-05-13.md`. | 2026-05-13 |
| 79 | **Tone Surfaces Standard** | Tum non-combat metin yuzeyleri "Vivid Vulnerability" tonuna uyar: death screen, run summary, class select, codex unlocks, boss titles, achievement names, loading tips, ending choice UI. Joke names YASAK, cozy tavern dili YASAK, generic grimdark despair YASAK, ironic chibi distortion YASAK. Standardize: `TASARIM/TONE_SURFACES_STANDARD.md` (yeni dosya, Faz 1 oncesi draft). | 2026-05-13 |
| 80 | **Class Silhouette Bible** | Her 10 sinif icin 64x64 chibi proportion'da locked identity profile: (a) weapon silhouette signature, (b) head/shoulder shape, (c) idle posture, (d) accent color, (e) VFX motif, (f) animation energy. PixelLab batch oncesi her sinif icin bu 6 alan dokumante. `TASARIM/CLASS_SILHOUETTE_BIBLE.md` (yeni dosya). Karar #71 (silah single-state) + Karar #34 (5E/5K cinsiyet) + V1 Karakter Visual Detayları memory ile uyumlu. | 2026-05-13 |
| 81 | **Open Vista Composition + Rift Side-Pockets** | Her oda 3-katman parallax: (1) sky/vista per-floor tema (F1 ruined kingdom, F2 corrupted forest, F3 void sky), (2) world-bleed rift overlay, (3) gameplay 32x32 foreground. Boss arena = massive rift backdrop showing world-merging. Mystery (node 6b) + Curse Gate (9b) odalarinin %50'sinde optional rift side-pocket entry → 20x40 open vista pocket world (Karar #83 guardraili icinde). Karar #61 (Hades-style room flow) KEEP. Production: Faz 1 = F1 parallax kit + boss backdrop ONLY (Codex R8: 80/30 alternatif), 3 pocket template Faz 2-3. Detay: `TASARIM/open_vista_decision_2026-05-13.md`. | 2026-05-13 |
| 82 | **Mob 3-Tier Skill System (STAGED)** | Her mob 3-tier skill: T1 Core (mevcut signature KEEP), T2 Environmental (lore-tied terrain/rift/seal etkilesim), T3 Synergy (pair-conditional combo). Boss her phase 1 environmental hazard. **STAGED ROLLOUT (Codex R7 + Karar #84):** Faz 1 = 3 T2 prototype mob + 1 boss hazard merge (mevcut Rift Tear escalation, yeni Rift Bloom DEGIL); Faz 2 = kalan 5 T2 mob; Faz 3 = T3 synergy combo (combat readability pass'i gectikten sonra). v1 scope: 8 mob x 3 tier degil, 3 mob T2 + 4 boss x 3 phase hazard merge. Detay: `TASARIM/mob_boss_skill_expansion_2026-05-13.md`. | 2026-05-13 |
| 83 | **Open Vista Side-Pocket Guardrails** | Karar #81 pocket'larinin LOCKED kurallari: (a) authored sub-rooms only, macro-map node DEGIL; (b) Shop/Rest/Entry/Boss odalarinda YASAK (sadece Mystery + Curse Gate'de); (c) max 2 per run cap (eski draft 2-3 idi, Codex R6 ile 2 LOCKED); (d) tek entrance + tek exit + tek encounter beat; (e) reward non-progression-critical (Karar #63 8-fragment boss gate sistemini bozmaz); (f) branching routes + free-roam YASAK; (g) 20x40 max size, persistent spatial continuity YOK. Karar #61 (open world rejected) sinirini guardrail. | 2026-05-13 |
| 84 | **Mob T2/T3 Staged Budget** | Karar #82 staged rollout kurallari: Faz 1 normal room = max 2 simultaneous T2-capable enemy; Faz 1 Elite room = max 3 T2-capable + 1 T3 combo family; Boss add phases T3 DISABLED; Boss pressure window'da max 1 T2 activation. T2/T3 cooldown gates ZORUNLU (Tear Open + Seal Repair Channel + Imp Tide + Subterranean Web). T3 pair synergies Faz 1'de FORBIDDEN — combat readability pass'i gectikten sonra Faz 3'te aktif. Tell max 1.5s + window min 0.4s floor KEEP. Detay: `TASARIM/mob_boss_skill_expansion_2026-05-13.md` (Codex review R3 + R4). | 2026-05-13 |
| 85 | **Open-World Backdrop Language (Açık Dünya Hissi)** | Karar #81 Open Vista ile uyumlu. RIMA mevcut arena chain DNA (15 node procedural) KORUNUR — yapısal kapalı arena. Bazı odalarda duvar parçalı + 3-layer parallax ile uzak dünya görünür. "Perceived openness" naming Codex feedback ile REVOKED → **"Open-World Backdrop Language"** (daha dürüst, daha düşük beklenti). Faz 1 zorunlu liste: F1 parallax kit (3-layer) + boss arena backdrop + **3 vista room template (cliff edge / balcony / rift opening) — Room Designer F3'ün ilk işi öne çekilir**. Free roam zone-level KESINLIKLE REJECTED. Pocket sub-rooms Faz 2-3 (Karar #83). Detay: `TASARIM/_ARCHIVE_2.5D_2026-05-12/BIG_DESIGN_DECISIONS_2026-05-13.md`. | 2026-05-14 |
| 86 | **Map Object Set + Pipeline (Faz 1.0 / Faz 1.5 ayrımı)** | 14 obje hedef. **Faz 1.0 zorunlu (6):** chest, breakable barrel, lever, shrine, spike-trap, terrain rift. **Faz 1.5'e ertelenen (2):** portal + shop counter (Act 1'de henüz açık değil). **Decor (6) nice-to-have paralel track:** sütun/moloz/bayrak/sunak/meşale/kafatası — yetişmezse Faz 1.5+. Boyut: 32x32 base + 32x64 (1×2 tall) allowed. **Multi-tile (4×4+) Faz 1'de YASAK** (collision + maliyet); Faz 2-3 optional non-colliding landmark decor açık. Pipeline: PixelLab CFSR (Karar #90 batch ekonomi) → Aseprite manual cleanup → ScriptableObject author → Room Designer Object Brush. Variant: 1 Faz 1.0, 2 Faz 1.5+. Toplam tahmini **4-6 saat** (eski 12-18 saat tahmini batch ekonomi ile düşer). | 2026-05-14 |
| 87 | **Skill Effect AngleMode (5 Kategori) + Faz 1.0 MVP / Faz 1.5 Polish** | SkillEffect ScriptableObject 5 angleMode enum: (1) **ProjectileRotated** (simetrik bolt/glow, 1 sprite + Transform.rotation, continuous 16-32 angle), (2) **ProjectileDirectional8** (asimetrik ok/hançer/mızrak, 8 sprite, 8-snap), (3) **BeamRotated** (lazer/ışın, 1 tile + stretch + rotation), (4) **Radial** (yönsüz patlama/AOE, 1 sprite), (5) **Cone** (Rotated mesh/particle VEYA Directional8 sprite, skill bazlı seçim). **Faz 1.0 MVP:** sınıf başına 3 core effect = LMB + RMB + V Burst → 4 sınıf × 3 = **12 effect**. **Faz 1.5 Polish:** Q/E/R/F slot doldurma → 16 ek effect. **Faz 1.0 Directional8 LIMIT: max 4 hero effect (sınıf başına 1, genellikle V Burst).** PixelLab batch ekonomi (Karar #90) Directional8 maliyetini ciddi düşürür: 32px batch tek generation = 64 cell = ~8 hero effect × 8 yön. Continuous angle default; pixel-art artifact varsa 8-snap fallback. Detay: `TASARIM/_ARCHIVE_2.5D_2026-05-12/BIG_DESIGN_DECISIONS_2026-05-13.md`. | 2026-05-14 |
> **REVOKED 2026-05-13:** Karar #114 ile sayısal trigger atlatıldı, 8 yön doğrudan LOCKED.
| 88 | **4 Yön + flipX LOCKED + Faz 2 Trigger Sayısal Eşik** | S59 LOCKED 2026-05-12 KEEP — 4 yön + flipX (N/S/E sprite, W=flipX). 8 yön Faz 2-3 staged. **Codex naming düzeltmesi:** "Hades match" → **"Hades-like responsiveness"** (input cevap hissi hedef, birebir visual fluidity değil). **Faz 2 8 yön upgrade trigger sayısal eşik:** V1 4 sınıf playable + 3 saat playtest sonrası, oyuncuların **%30+'ı "diagonal step hissi" feedback verirse** 8 yön protagonist upgrade tetiklenir. Solo dev playtest yapamıyorsa: Faz 2 sonu + 1 hafta self-test. Trigger ÖLÇÜLMELİ — yoksa 8 yön Faz 5'e kayar. Staged: protagonist önce, mob T1/T2 4 yön kalır, boss T3 8 yön. Faz 3: tüm V1 sınıf + boss 8 yön, common mob 4 yön (visual hierarchy). | 2026-05-14 |
| 89 | **Game Language EN-First Canonical** | Oyun canonical dili **İngilizce** — lore, diyalog, tone, kelime hazinesi (hollow/rift/echo/shackle/penitent gibi katmanlı EN terimleri) **EN-first kompozisyon** olur. Türkçe **localization/translation** — sonradan çeviri, canonical değil. Architect monolog + boss cinematic dialog ("...this is not enough anymore" stilinde) + lore portrait monolog + ending text → EN canonical doğsun. `TONE_SURFACES_STANDARD.md` format: EN canonical YUKARI + TR localization AŞAĞI. Steam page + marketing EN. Pazarlama hedef: International audience first, TR localization secondary. | 2026-05-14 |
| 90 | **PixelLab Batch Economy (Create from Style Reference Esnekliği)** | PixelLab CFSR tool **tek generation = N cell tilesheet** veriyor. Tile boyutuna göre: **32x32 sprite → 64 cell** tilesheet (8x8 veya 16x4), **64x64 sprite → 16 cell** (4x4), 128x128 sprite → 4 cell. **Esneklik bize kalmış:** (a) 1 ürün × N variant (örn: 64 farklı barrel variant), (b) N ürün × 1 cell (örn: 64 farklı obje tipi), (c) karışım (örn: 5 ürün × ~13 variant). **Production etkisi:** Map object 14 obje = tek 32px batch (~5-10 dk generation + 2-4 saat cleanup) ≈ **4-6 saat total** (eski Codex tahmini 12-18 saat = batch ekonomiyle ~%70 düşer). Skill effect Directional8 (8 sprite/effect) × 2-3 hero = 16-24 sprite = tek 32px batch (64 cell kapasitesi içinde). Aynı esneklik **fırlatılabilir effect (projectile/fireball/ok)** için geçerli. **Pipeline disiplini:** önce 1 sprite pilot test (Warblade) → batch'e geç. Aseprite manual cleanup hala 5-15 dk/sprite. Generation maliyeti minimum, esas iş cleanup. | 2026-05-14 |
| 91 | **Accessibility Telegraph 3-Kanal Standard (Proje Geneli)** | RIMA hazard telegraph **3 paralel kanal** ZORUNLU her hazard için: (1) **Outline thickness pulse** 0→4→0px @ 0.6s (renk-bağımsız, primary cue), (2) **Subtle ground shake** max 2px (audio-off backup, motion sickness'a duyarlı), (3) **Color glow** (cyan/red/yellow palette match — DECORATIVE, ana güvenilirlik kaynağı DEĞİL). Deuteranopia/protanopia uyumlu. Tüm hazard'lar (mob T2 telegraph + boss skill telegraph + skill counter window + terrain hazard rift/spike/lava) bu standardı izler. `BOSS_PHASE2_RIFT_TEAR_SPEC.md` Decision 4'ten proje geneline genelleme. | 2026-05-14 |
| 92 | **Ranger Gorsel Kimlik Kilidi** | Off-white bleached-ivory sac, alcak gevşek orgu, savasta yipranmis taktik yabanillik (asimetrik zirh, yara izi), kabile/feral YASAK. Detay: `CLASS_SILHOUETTE_BIBLE.md` Ranger bolumu. — UPDATED: Sac rengi korunur (off-white/bleached-ivory), sac stili guncellendi: half-shaved undercut + braid. | 2026-05-12 |
| 93 | **Elementalist Kostum Kilidi** | Asimetrik iki-el jesti (sag omuz-sag avuc ileri, sol bel-sol avuc yukari), dis krem/fildisi robe + #2A1F35 trim; Karar #43 (honey-blonde topuz) HOLDS. Detay: `CLASS_SILHOUETTE_BIBLE.md` Elementalist bolumu. — REVOKED by Karar #95 (2026-05-13) | 2026-05-12 |
| 94 | **Mimari** | Karakter Sprite Style Reset (S62): Mature 5-6 head proportions, 64x64 canvas, ~60 derece ARPG kamera (Last Epoch/Hero Siege/Diablo 2 ref). S59 chibi+35 derece CHARACTER SPRITES ICIN REVOKED. Tile/VFX/UI degismez. Stil anchor: style_anchor_v2.png (Image #11). -- REVOKED 2026-05-13 by Karar #100. Reason: 64px mature gives ~11px head with no facial detail; PixelLab + animation pipeline optimized for chibi. S59 chibi+35 derece KEEP for characters. | 2026-05-13 LOCKED |
| 95 | **CLASS_SILHOUETTE_BIBLE** | Elementalist Kostum Reset: Karar #93 REVOKED. Yeni: cropped sleeveless top + flowing yuksek belli etek + bare midriff + krem sash. NO STAFF, NO ROBE. Sag avucta floating golden rune disc (hovering). Sol el casting gesture. Honey-golden sac korunur. -- CONFIRMED 2026-05-13: cropped top + bare midriff CONFIRMED chibi-compatible (4-6px midriff strip at chibi proportions works). Outfit: cropped sleeveless top + small bare midriff strip + flowing skirt + dark fitted tights + high boots + cream sash. Floating rune disc + asymmetric gesture + honey-blonde bun KEEP. | 2026-05-13 LOCKED |
| 96 | **CLASS_SILHOUETTE_BIBLE** | Summoner Palet Reset: Yesil (#00FF88) REVOKED. Yeni: dark indigo + cyan glow + violet accent. Renk ekonomisi: yesil = SADECE Hexer (lanet/cadi); cyan+violet = Summoner + rift mob'lar. | 2026-05-13 LOCKED |
| 97 | **CLASS_SILHOUETTE_BIBLE** | Hexer Curse Staff: Grimoire birincil kimlik tasiyicisi DEGIL — bele zincirli kucuk kitap aksesuar olarak kalir. Birincil: sap ucunda yesil alev olan lanet asasi (sag elde). Yesil alev = yuz aydinlatma kaynagi (cin/goz). | 2026-05-13 LOCKED |
| 98 | **Asset** | Rift Renk Dili Kilidi: Tum rift yaratiklarl ve rift efektleri = cyan + violet palet. Fracture Imp, Relic Caster, Rift Hound, Plate Widow, Hollow Arbiter hepsi bu paleti kullanir. Rift Crack VFX ile gorsel tutarlilik. | 2026-05-13 LOCKED |
| 99 | **Asset** | Silah Kamera Uyumluluk Kurali: ~35° top-down (Hades reference, Karar #100)'da sirta monte silahlar siluetten kaybolur. Kural: tum hero birincil silahlari elde veya belde net dikey siluet olusturur. Warblade greatsword elde, Ranger bow elde, Ronin katana belde. | 2026-05-13 LOCKED |
| 100 | **Mimari** | S62 Style Reset REVOKE: Karar #94 (mature 60°) iptal. S59 chibi 64x64 + ~35° kamera (Hades match) CHARACTER SPRITES ICIN GERI YUKLENDI. Dark gritty tone Salt and Sanctuary chibi inspirasyonu KORUNUR (Hades bright theatrical degil). Image #12 (chibi warrior 64x64) yeni style anchor -- 4x4 batch sheet uretiminde reference image olarak kullanilir. | 2026-05-13 LOCKED |
| #110 | Combat FAZ 1.0 Mimari | AttackToken player-scope DISI (mob-only); Cancel window progress-based (BasicAttackProfile.cancelWindowFraction); MercifulDodge 0.18s grace flag | 2026-05-13 |
| #111 | Awakening + Trace Sistemi | Faz 1.0'da 4 base class için 60sn Awakening (ilk seçimde zorunlu, held-to-skip 0.5sn). Run içi Trace = rift crack overlay (NPC değil), post-combat/vista transition trigger, run başına max 1, 4-cap pool, pure narrative. Faz 2'de 6 class + 8 Trace + mekanik reward. | 2026-05-13 |
| #112 | Lore Glossary | Capital-S **Shard** = Fracturing reality fragment. lowercase **shard** = currency/material. **Trace** = run-içi cryptic identity sinyali. **Awakening** = class intro micro-segment. **Echoes** (currency) + **Echo Twin** (boss) korunur, "echo" kelimesi başka bağlamda kullanılmaz. | 2026-05-13 |
| #113 | Camera + Perspective Convergence | Karakter + tile + VFX tek konverjans ~35° high top-down (Hades reference). 45° tile REJECT. Geniş savaş alanı = Orthographic Size kalibrasyonu (combat framing profili 320x180 ref). PixelPerfectCamera: GridSnapping=UpscaleRenderTexture, FilterMode=Point, CropFrame platform-test. PPU=64 sabit. CameraFollow.combatOrthographicSize PixelPerfect ownership netleşmeli. Drop shadow = child SpriteRenderer opsiyonel polish (perspektif maskesi DEĞİL). | 2026-05-13 |
| #114 | 8 Direction Animation Locked | Tüm playable + T2 mob + boss animasyonları 8 yön (N/NE/E/SE/S/SW/W/NW). Üretim: 5 yön gen (S, SE, E, NE, N) + 3 mirror (W=E flip, SW=SE flip, NW=NE flip). Karar #53 + #88 REVOKED. VFX AngleMode (#102) 8 yön projection için Faz 1.0 sonu rekalibrasyon. PixelLab Custom Animation V3 pipeline değişmez, direction count artar. | 2026-05-13 |
| #115 | AI-Assisted Map Builder | RIMA Map Builder = Unity Editor Window tabanli AI-assisted authoring araci. Faz 1.0: deterministic C# RoomBaselineGenerator (System.Random, GenerationInput{seed,biome,archetypeId,w,h,generatorVersion} kontrati), RoomBaselineTemplate ScriptableObject parametre kaynagi, toolbar Generate butonu, floor/wall tilemap ciktisi, FloorVariantPainter+WallAutoConnect bake entegrasyonu, byte[] grid + LUT variant metadata, RoomBlueprint+Prefab+RoomPrefabLink+RoomConfig save koprusu. Faz 1.5: Unity-internal inpaint region re-seed (kilitsiz hucreler), lock-aware overrideVariantIndex+floorOverrideVariantIndex rebake, tile-mask anchor zone painter (zone type+weight), force re-seed komutu, RenderTexture cache+debounce, preview kamera ~35 derece konverjans kalibrasyonu. REJECTED: fullscreen oyun-gibi in-game editor framing, LLM runtime/editor cagrisi, PixelLab Inpaint API cagrisi, PNG export, runtime procedural 15-node placement override, RoomLoader secim mantigi bypass, rect/polygon anchor schema, UnityEngine.Random global state kullanimi. Path: Assets/Scripts/Systems/Map/ + Assets/Scripts/Runtime/Rooms/. Exit criteria Faz 1.0: ayni seed+biome+archetype bit-identical room, 5 oda generate runtime hatasiz, designer duzeltme %20 alti, RoomLoader RoomConfig-missing hata yok. | 2026-05-13 |
| #116 | Tile Transition Quality Standard | F1+ tum tilesets icin zorunlu kalite standardi (Alabaster Dawn polish bar): (a) Floor-wall transition Raggedness >=40% — Wang autotile kullanilir, grid-block hissi yasak; (b) Ayni terrain icinde 3+ varyant zorunlu — Perlin-driven asimetrik dagilim, copy-paste hissi yasak; (c) Multi-terrain seam = Wang transition tiles + opsiyonel decal layer (organik damar, dust trail, rift crack icin); (d) Lichen/moss/dust/rune patches + foreground prop'lar center playable area'da dusuk yogunluk, edge zone'larda yuksek (combat clarity onceligi); (e) Baked light/glow tile icinde YASAK — Unity URP 2D Light runtime'da uygulanir (Karar #103 spec); (f) Cliff/elevation = sprite-baked yukseklik illuzyonu (gercek 3D geometri DEGIL); Karar #115 vista odalari icin tile prefab icinde gomulu Y-offset + drop shadow kullanilir; (g) Drop shadow (child SpriteRenderer multiply oval) tum karakter/mob/decor icin zorunlu polish (Faz 1.5). QC: 4x zoom'da pixel grain dogal, hairline crack continuity bozulmaz, tile seam okunmaz. Referans: Alabaster Dawn (transition smoothness + cliff illusion + low playable density), Hades (palette discipline). Bu kalite Karar #75 REVISION'la onaylanan create_topdown_tileset Pro mode + create_tiles_pro style mode kombinasyonu ile saglanir. | 2026-05-13 |
| #117 | Room Designer Portable Core | Karar #115 Map Builder implementation'inda Core/Game Layer ayrimi ZORUNLU. **Core** (`Assets/RoomDesigner.Core/`): BrushController + Brushes (Stamp/Eraser/Picker/Bucket), FloorVariantPainter Perlin algoritmasi, WallAutoConnect Wang NSEW mask logic, RoomBaselineGenerator abstract base, SeedPipeline, byte[] grid + LUT pattern, Editor UXML/USS — RIMA referansi YASAK (palette hex'leri, biome enum, archetype isimleri Core'da olmaz). **Game Layer** (`Assets/Scripts/Systems/Map/Rima*`): RimaBiomeType enum, RimaRoomBaselineTemplate concrete SO (Core abstract'tan extends), RimaRoomConfig schema, RimaArchetypeGenerators (combat/loot/elite/boss/vista) — RIMA-specific. Core katmani standalone Unity Package olarak baska top-down 2D projelere tasinabilir (~1-2 hafta integration vs sifirdan ~2-3 ay editor). Faz 1.0 exit criteria'ya **"Core layer RIMA-free derlenebilirligi"** test eklenir. Strategic yatirim: editor bir kez yazilir, sonraki oyunlarda parametrize edilir. | 2026-05-13 |
| #118 | Hybrid Tile Composition System (Multi-Layer + create_tiles_pro Primary) | RIMA tile pipeline 3-katman tilemap + PixelLab tool ayrimi: **Tool Stratejisi:** (i) Wang `create_topdown_tileset` Pro = SADECE floor-wall + floor-path base relationships (terrain transition + okunabilir edge icin); chain SADECE F2/F3 biome gecislerinde (`lower_base_tile_id` renk tutarliligi); (ii) `create_tiles_pro` (square_topdown 32px 64-cell batch, Karar #90 batch economy direkt uyumlu) = floor/wall variant zenginligi, style reference ile palet anchor; (iii) `create_object` transparent BG = decals (moss, rift crack, dust, grime, small debris — Decal layer'a) + props (pillar, rubble, brazier — Prop layer'a). **Disiplin:** PixelLab Map editor oda tasarimi veya mockup icin KULLANILMAZ — tum map composition Unity Room Designer (Karar #115) icinde yapilir. Wang sheet'ler yalnizca terrain transition icin; moss/crack/grime gibi yuzey detaylari Wang'a sokulmaz, Decal layer'da overlay. **Unity Tilemap Stack:** (1) Base layer = RuleTile + RandomTile (floor + path + variants, autoconnect + weighted variant); (2) Decal layer = transparent overlay (moss/rift/dust, collider OFF); (3) Wall layer = Wang autotile (gameplay collider); (4) Prop layer = discrete sprites. **Agent Pipeline:** Codex `TileImportWizard` PixelLab Export Parser (Faz 1.0 zorunlu): folder secimi → `asset_000.json` parse → sheet auto-slice → standard Wang mapping → RuleTile auto-create → multi-layer tilemap iskeleti. Room Designer brush mode'lari: Paint Base / Paint Decal / Paint Wall / Paint Prop. Karar #116 (c) pratik uygulamasi, Karar #117 Portable Core uyumlu (Game Layer'da RIMA-specific RuleTile rules, Core'da generic multi-layer infrastructure). | 2026-05-13 |
| #119 | AI-Assisted ASCII Matrix Parser | Gemini 3.1 Pro önerisi. Offline LLM → 3-katman ASCII (Elevation/Ground/Props) → AITilemapImporter parser. Karar #115 deterministic mantığı korunur (System.Random, GenerationInput kontratı). Brush/Save bypass yok, RoomBaselineTemplate parametre kaynağı ile uyumlu. Faz 1.6 Codex dispatch ~6-8h. S68 P4 priority. Detay: MEMORY user-level `project_ai_ascii_matrix_parser.md`. | 2026-05-13 |
| #120 | Split-Animation Technique — LOCKED 2026-05-13 | Karmashik animasyonlari (>=12f + net apex) Create State apex referansinda ikiye bolerek uret. 3-Stage: Apex State (Create State 20-40 gen) → Part 1 Custom V3 (End Frame=Apex) → Part 2 Custom V3 (First Frame=Apex). Aseprite'ta apex bir kez, 5-madde export checklist (hash verification). Karar #71 weapon lock pozitif-only enforcement. Loop/smear/multi-peak animasyonlarda YASAK. Cross-ref: Karar #71 (strengthen), #108 (inherit), #109 (unaffected), #114 (inherit). Detay: TASARIM/ANIMATION_SPLIT_TECHNIQUE.md | 2026-05-13 |
| #121 | Scatter Brush System — LOCKED 2026-05-13 | Grid-bagimsiz SpriteRenderer scatter katmani. ScatterBrushWindow (Tools>RIMA>Scatter Brush): radius/density slider, 4 kategori (Stones/Moss/Rubble/Dirt), Perlin noise Generate. Sorting: Floor=0, Scatter=5, Wall=10, Prop=15. | 2026-05-13 |
| #122 | Echo Resonance Multi-Tier | Cross-class Shadow Echo extension, 4-tier architecture **extending Karar #5/#7** (NO conflict). **T1 Commit-Beat** (100% proc, 1.2s ICD, 35% dmg) = MVP baseline. **T2 Resonance Hit** (15-25% proc, 0.8s ICD, 25%) = Faz 2. **T3 Empowered Skill** (100% on cast, 50%) = Faz 2. **T4 Rift Proc Bond** (3 Family Tags → 100% + armor pen) = Faz 2. Universal 3-Beat Combo + Facing-Relative Spawn + Primary Skill Enhancement (per-tier +20%). Family Tags (4 categories). Detay: `STAGING/karar_122_echo_resonance_tiers.md` + `STAGING/karar_122_addons_final.md`. | 2026-05-14 |
| #123 | Weapon Decouple Architecture (Yol A) — **EN SON KARAR** | Body silahsız sprite + ayrı weapon sprite + Unity Transform attach (Hades/ETG/Brotato pattern). PixelLab weapon-drift problemini yapısal olarak çözer. **Player primary:** decouple zorunlu. **Phantom Echo:** weapon-baked OK (0.4s brief, drift mümkün değil). **Level 1 MVP:** orbit attach (single anchor, weapon idle pose). **Level 2 polish:** per-frame anchor (animation hand position tracking). **Karar #71 ile uyumlu:** Silah hâlâ "hep elde" gözükür — sprite 2 parçaya bölündü, render aynı. **İstisnalar:** Brawler bare fists (silah yok), Elementalist disc Unity VFX (body unarmed), Ronin sheath body sprite'da kalır (kimlik). Class matrix: 8/10 class decouple needed (Brawler + Elementalist body-only). Detay: user-memory `project_yol_a_weapon_decouple.md`. | 2026-05-14 |

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
| **Summoner** | Nekro yeşili / kemik | dark indigo + cyan + violet (Karar #96) | Minyon glow, kemik parçaları, çağırma dairesi |
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
