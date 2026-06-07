> ℹ️ Görev DURUMU için CURRENT_STATUS.md kazanır; bu dosya sıra/kapsam referansıdır. (2026-06-07)

# RIMA — NİHAİ DEĞERLENDİRME + UYGULAMA PLANI (2026-06-06)
**Kaynak zinciri:** ChatGPT V2 planı → Council round-1 (`V2_PLAN_DECISION`) → Portal Pack → Council round-2 (`PORTAL_PACK_DECISION`) → ChatGPT konsolide soru seti → **bu doküman = Opus nihai kararı.**
Kanıt tabanı: cx file:line envanterleri (`CODEX_DONE_laurethayday.md` ×2), ax-3.1 mimari, ax-3.5 lean, Opus-advisor tasarım ×2.

---

## A) KRİTİK DEĞERLENDİRME

### Doğru çıkarımlar (onaylı)
- "Var olanı bağla/skinle/tune'la, sıfırdan yazma" çerçevesi DOĞRU ve kanıtlı: juice driver'ları (`HitPauseDriver`/`ScreenShakeDriver`/`HitFlashDriver`/`BrokenStateVisual`), `SkillData.tags`+`ChainWindowTracker`, `ParallaxLayer`+`RoomMoodLightPool`, hook'lu `AudioManager`, 4 aday kemer sprite'ı, `portal_rift.png`, `RiftGlowVFX`, `FloorRiftCrack` — hepsi diskte/kodda.
- Floating-island doktrini + floor'a dokunmama + portal=geçiş objesi: canon ile uyumlu, ship edilmiş sistemle örtüşük.
- "Önce algı öldürenleri temizle" (debug, sessizlik, generic portal, görünmeyen execute) = doğru P0 felsefesi.

### Riskli / eksik çıkarımlar (düzeltildi)
1. **"5 portal türü" hâlâ listede → 4'e düşürüldü.** Heal/Lore portalı RED: run graph'ta Heal/Lore oda tipi YOK (`RoomType` = Combat/Elite/Chest/Event/Merchant/Forge/Boss; graph Chest kullanıyor). Girilemeyen portal vaat etmek = jüri güven kaybı. Demo: **Combat / Elite / Reward(Chest) / Boss**.
2. **"Pivot düzeldi" teknik kanıt, his kanıtı DEĞİL.** 82 sprite re-pivot + Entities/IsoSorter migrasyonu commit'li ve probe'lu ama kullanıcı feel-testi yapılmadı. P0'a 15-dk playtest eklendi — portal katmanına girmeden önce şart.
3. **Özet "boss readability" ve "canlı-demo vs screenshot-only" ayrımını taşımıyor.** Boss görseli yok (sprite üretilmedi); minimal çözüm karara bağlandı (aşağıda). İlk 5 madde canlı-kritik, 6-9 esnek ayrımı korunmalı.
4. **ENTRY_S "socket" dili yanıltıcı:** giriş FİZİKSEL OBJE DEĞİL — stateless varış VFX'i (3.1+Opus mutabık: giriş portalı objesi RoomRunDirector FSM'ine anlam boşluğu açar).
5. **Ölüm/zafer maddesi (T6) sırada doğru yerde ama içeriği özette zayıf:** jürinin göreceği iki uç an; Echo dökümü tek başına yetmez, son-vuruş slow-mo + boss-kill payoff şart.

### Scope-creep mayınları (yapılırsa proje yutarlar)
6-durumlu portal FSM (2 state + tween yeter) · cliff köşe/endcap kiti (sistem rewrite) · RoomVisualProfile 5 tema · build meter · danger pips · chamber restoration ekonomisi · AudioMixer bus stack · entry portal objesi · konsept sheet pixelify · 8-yön her şey · key rebinding · boss=oda redesign (post-demo'ya park).

---

## B) REVİZE ÖNCELİK LİSTESİ

| Seviye | İş | Not |
|---|---|---|
| **P0 (bugün)** | (0) KULLANICI 15-dk playtest — bugünkü spawn/kapı/pivot/sorting fix'lerinin his onayı | Portal işine girmeden şart |
| **P0** | T1 ScreenshotMode-lite + deterministik seed + debug temizliği | Her şeyin ön koşulu |
| **P1 (demo-kritik)** | T2 Juice tuning + [RMB] Execute prompt + 8-SFX (TEK paket) | Ses ayrı iş DEĞİL — sessiz hit-stop "buglı" hissedilir |
| **P1** | T3 Portal görsel katmanı (DoorPortal prefab + 4 skin) | |
| **P1** | T4 Draft readability (tag chip + sinerji satırı) | |
| **P1** | T5 Chamber atmosfer + cyan yönlendirme + varış/bürünme VFX | |
| **P1** | T6 Death/Victory mikro-pass + Echo döküm paneli | Jüri ölümü görecek |
| **P2 (rapor/sunum)** | T7 Mob 4-arketip + telegraph | Canlı demoya da değer katar ama kesilebilir |
| **P2** | T8 Void parallax + mood-light binding | |
| **P2** | T9 Rapor görsel seti + dürüstlük/tutarlılık pass'i | T1'e bağımlı |
| **P3 (ertelenebilir)** | Chamber restoration · Heal/Lore portal · cliff kit · boss=oda · tam tema sistemi · cosmetic aura · AudioMixer stack · memory fragments | Post-demo backlog |

---

## C) TASK BREAKDOWN

### T1 — ScreenshotMode-lite + seed [S/M, risk: DÜŞÜK]
**Amaç:** Debug'sız, tekrarlanabilir kareler + temiz canlı demo.
**Yapılacak:** Static `ScreenshotMode` (F12): registry ile gizle → `DemoDebugPanel`(F1), `InPlayMapPaintOverlay`(F2), dummy-HP label'ları, `IsoRoomBuilder` marker container, death/retry overlay guard, spawn/door marker'lar. 6 kamera preset (chamber-wide / pedestal-close / combat / draft / doors / room-overview). `BridsonPoissonAutoPlacer`+re-seed yoluna seed parametresi (aynı seed→aynı oda). `GameViewSetup` reuse (1920×1080).
**Sistemler:** GameViewSetup, DemoDebugPanel, IsoRoomBuilder, UIManager.
**AC:** Tek tuşla 6 temiz preset; hiçbir karede debug objesi/yanlış UI state yok; aynı seed iki koşuda aynı odayı verir.

### T2 — Juice tuning + Execute prompt + 8-SFX [M, risk: DÜŞÜK-ORTA]
**Amaç:** Vuruş hissi + imza mekaniğin (BREAK→EXECUTE) görünürlüğü + sessizliğin bitmesi. SIFIRDAN SİSTEM YOK — hepsi mevcut driver'lara değer/bağlama.
**Mevcut:** HitPauseDriver (hit .04 / crit .07 / kill .12 / finisher .18), ScreenShakeDriver, HitFlashDriver, BrokenStateVisual, FeelToggleSettings, `DeathBlow` (Broken/Sundered gate'li), AudioManager hook'ları (PlayerController/Health/Gate/DraftManager/BasicAttack'te ÇAĞRILI, klipler muted).
**Bağlanacak minimum:**
- Light hit → pause .03 + flash beyaz + küçük knockback (mevcut) + SFX `hit_impact`
- Heavy hit → pause .06 + shake S + Broken uygulaması (mevcut) + SFX `swing_heavy`→`hit_impact`
- Execute (DeathBlow) → freeze .08-.12 + büyük slash VFX + SFX `execute_payoff` + **[RMB] Execute world-prompt**: Broken/Sundered hedef ≤2 birim yakındayken hedef üstünde prompt ([G]-interact prompt altyapısı REUSE)
- Knockdown → shake M + SFX `knockdown_thud` (knockdown sistemi zaten canlı)
- Düşman hit-flash = BEYAZ + kırmızı/magenta rim. CYAN YASAK (cyan=oyuncu/Rift).
**8-SFX paketi (CC0, Kenney/Sonniss GDC):** 1 swing (light/heavy pitch-vary) · 2 hit_impact · 3 dash · 4 enemy_death · 5 execute_payoff (bas+cam kırılma) · 6 room_clear/portal_open · 7 draft hover+select · 8 chamber_ambient loop. Mapping: mevcut `Sfx` enum genişlet, `Resources/Audio/` override yolu zaten var.
**AC:** Heavy/light ayrımı hissedilir; Broken hedef yanında prompt çıkar ve execute akışı sesli/efektli; 8 event'in tamamı sesli; FeelToggleSettings'ten kapatılabilir; timeScale kaynaklı fizik bozulması yok (varsa değer geri alınır).
**+ M1 (R5):** Dash input buffer ~80ms (windup/recovery'de basılan dash kuyruklanır; coyote-time YOK — walkable-enforcement ile çatışır). AC: çift-dash yok, kenar regresyonu yok.

### T3 — Portal görsel katmanı [M, risk: ORTA (Y-sort)]
**Amaç:** Generic gate → tip-okunur dünya-içi rift portalları.
**⚠️ T3.0 ÖN-TASK (round-3 council, `GATESLOT_DECISION_2026-06-07.md`):** Authored gate slot'ları — `door_NW_01/door_N_01/door_NE_01` socketId konvansiyonu + Fix Sockets auto-placement + seçim kuralı (1→N · 2→NW+NE · 3→hepsi) + havuz filtresi + validator MUST/WARN seti + Rooms tab önizleme etiketleri. BuildExitDoors spacing/clamp matematiği silinir; portal prefab garanti slot koordinatlarına iner. (Aşağıdaki "validator 3-kapı-walkable kontrolü" maddesi bununla SUPERSEDED.)
**Yapılacak:** `DoorPortal` prefab (Frame SR + Core particle/SR + Rune SR + Label TMP + Light2D + trigger collider + 2-state dumb-view controller) + `PortalSkin[4]` serialized tablo. `BuildExitDoors` GameObject-inşadan `Instantiate(prefab)`a geçer — **returned-list sözleşmesi ve RoomRunDirector trigger wiring DEĞİŞMEZ.** Skinler mevcut sanattan: kemer (gate_arch / portal_arch_gen / arch_gate / act1_arch_exit'ten QC ile seç) + `portal_rift` core + `RiftGlowVFX` idle + `FloorRiftCrack` taban decal'i. Tip ayrımı: Combat=cyan/beyaz · Elite=magenta tint+fracture overlay (mevcut `act1_rift_fracture_overlay`) · Reward=soluk altın+`"Rare+"` etiketi · Boss=kızıl rün+yavaş nabız (2× boyut+ritüel halka T6'da). Sealed→Open tween; yaklaşınca highlight. Boyut: ~1.6× karakter (96-112px efektif); Boss 2×. Validator'a 3-kapı-yayılım-walkable kontrolü.
**AC:** 4 tip ≤1 sn'de ayırt edilir; 1/2/3 kapı akışı play-probe'da bozulmaz; Y-sort doğru (oyuncu portal önü/arkası); smoke 26/26 yeşil kalır.
**+ M3 (R5):** Portal yanı slate sütun çiftleri (mevcut pillar prop'ları; Elite=cyan rün aksanı, Boss=fracture overlay) — saf prop yerleşimi, sıfır kod, çerçeveleme/okunurluk. **+ R4 açı kararı:** PortalSkin 2 frame taşır (frontal+angled); NW=angled, NE=angled flipX, N=frontal (batch-2 asset'leri).
**GATED (kullanıcıyla PixelLab):** reward+boss rünleri 32×32 (combat/elite mevcut) — gelene kadar geçici: mevcut rünler tint'li.

### T4 — Draft readability [S/M, risk: DÜŞÜK]
**Yapılacak:** SkillOfferUI'a tag chip satırı (`SkillData.tags`, görselde max 3-5 tag) + sinerji satırı ("Broken zincirini tetikler" — `ChainWindowTracker` ilişkisinden) + sinerji yoksa nötr satır + seçim mikro-anim (kart büyür, diğerleri soluklaşır, SFX). **Build meter YOK. BuildSynergyEvaluator YOK.**
**AC:** Kartta tag'ler görünür; eldeki build ile eşleşen kart vurgulanır; tooltip sinerji bloğu okunur.
**+ M2 (R5):** Stat-diff renklendirme — hover'da pozitif modifier cyan / negatif #D8364C; statik (equipped-karşılaştırma yok). AC: layout kaymaz.

### T5 — Chamber atmosfer [M, risk: ORTA (URP Light)]
**Yapılacak:** Pedestal başına Light2D spot (cyan, düşük yoğunluk; kilitli=soluk) + mor ambient profili (Global Light2D zaten bootstrap'ta) + cyan zemin yönlendirme decal çizgisi (spawn→ilk pedestal→çıkış rift'i; 3 statik sprite) + bürünme VFX-lite (cyan patlama particle + oyuncuda outline flash + sınıf adı float-text) + ENTRY_S varış VFX'i (0.4s halka genişleme + toz + 0.3s alpha fade-in; run odalarında da aynı) + dummy-HP T1 registry'sine kayıt. Brazier'lar: Chamber'da ≤2 yanan instance, gerisi sönük.
**AC:** Chamber karesi "ritüel salon" okunur; oyuncu metinsiz olarak pedestal→kapı akışını bulur; sıcak ışık cyan/mor egemenliğini bozmaz.
**Risk mitigasyonu:** Işık pass'i KÜÇÜK başlar (sadece chamber); materyal Lit/Unlit uyumsuzluğu görülürse Lit'e toplu geçiş AYRI karar.

### T6 — Death/Victory + Echo dökümü [S/M, risk: DÜŞÜK]
**Yapılacak:** Ölümde son-vuruş slow-mo (HitPauseDriver.finisher reuse) → DeathScreen'de Echo kaynak dökümü (`RunStats`: "Oda 7×3=21 · Kill 44/5=8 · Toplam 29 ◈") + tek tuş chamber dönüşü. Victory'de boss-kill payoff: uzun slow-mo + büyütülmüş Broken-shatter VFX reuse + Echo satırı + "RUN COMPLETE" diegetik metin. Boss portal istisnaları (2×, ritüel halka decal, kızıl rün) burada. Boss minimal polish: mevcut boss 1.5-2× ölçek + koyu rim + 1-2 ground-decal telegraph. **✅ KULLANICI KARARI (2026-06-07): Seçenek A KİLİTLİ** ("boss=oda" post-demo backlog'a); boss görsel öğeleri (crown/seal overlay, telegraph decal seti) imagegen kuyruğuna eklenir, detay spec council'den.
**AC:** Ölüm ekranı "neden öldüm + ne kazandım"ı anlatır; victory ≥3 sn kutlama; her ikisi screenshot-preset'li.

### T7 — Mob 4-arketip [M, risk: DÜŞÜK-ORTA]
**Yapılacak (SIFIR yeni sprite):** Chaser=en küçük mob 0.85× + magenta rim + kısa blink telegraph · Brute=en iri mob 1.15× + koyu kırmızı rim + yavaş + KONİ telegraph · Ranged=orta mob + çizgi telegraph (LineRenderer) + mesafe tutma · Bomber=orta mob + cyan-core pulse overlay (Broken VFX reuse) + genişleyen daire telegraph. Telegraph renkleri kırmızı/mor — CYAN ASLA. EncounterController bank'ı 4 arketipe eşlenir; kalan 9 mob varyant-pool'a iner.
**AC:** 4 silüet/davranış oyunda ayırt edilir; her saldırı öncesi telegraph okunur; performans düşmez.

### T8 — Void binding [M, risk: ORTA (jitter/sorting)]
**Yapılacak:** `ParallaxLayer` (bg_L4_fog + uzak ada silüetleri) + `CliffEdgeDustEmitter` + `RoomMoodLightPool`'u _Arena oda inşa akışına bağla; portal-içine akan partikül (odak çekici). Pixel Perfect Camera ile jitter testi.
**AC:** Void düz renk değil; kamera hareketinde titreme yok; FPS hedefte.

### T9 — Rapor görsel seti + dürüstlük [S/M, risk: DÜŞÜK]
**Yapılacak:** T1 preset'leriyle yeni temiz kareler (eski 9 şekil yenilenir, Şekil 12=pedestal yakın çekim) + eksik 5: Şekil 6 Warblade render (idle 4-6× nearest-neighbor + koyu panel) · Şekil 8 diyagram (harici araç; JSON→Importer→SO→Builder→Scene→QC) · Şekil 11 sheet (5 üretilen+3 mirror etiketli manuel kompozisyon) · Şekil 13 Test Runner GERÇEK koşu (sayı metinle birebir) · Şekil 14 QC before/after (gerçek QC geçmişi `90c84995` evidence'tan, kırmızı işaretli). Metin dürüstlük pass'i: "Veri modeli 10 sınıfı destekler; demo 4 uçtan uca sınıf kitiyle pipeline'ı kanıtlar" kalıbı; "111 skill/67 impl" tarzı sayılar görsellerle çelişmeyecek şekilde yumuşatılır.
**Riskli metinler:** test sayıları (her koşuda değişir — screenshot günü sabitle) · "10 sınıf oynanabilir" iması · Map Designer "8 sekme" iddiası görselle kanıtlanmalı (sekiz sekme GERÇEK — mockup değil gerçek screenshot kullan).

---

## D) PORTAL-ONLY TEKNİK KARAR (KESİN)
- **Facing direction = 1.** Tüm portallar güneye (oyuncuya) bakar.
- **Slot = 3 çıkış + 1 giriş.** Çıkışlar arka kenarda; **mevcut merkez-anchor+spacing korunur** (EXIT_NW/N/NE ayrık slot'a kanıtsız geçilmez; validator 3-kapı-walkable kontrolü ekler, FAIL eden template'e elle offset). ENTRY_S = spawn noktası + stateless VFX; **fiziksel giriş portalı objesi YASAK.**
- **Prefab:** DoorPortal = Frame SR / Core (particle+SR) / Rune SR / Label TMP / Light2D / Trigger / 2-state dumb-view controller. Source-of-truth = RoomRunDirector.
- **Skin:** `PortalSkin { RoomType; frameSprite; runeSprite; coreTint; particleIntensity; label }` ×4 (Combat/Elite/Reward/Boss). Tek gövde, runtime varyasyon.
- **Kesinlikle yapılmayacaklar:** 8-yön portal · E/W/SW/SE kapılar · 6-durum FSM · danger pips · Heal/Lore skin'i · her portala ritüel halka · konsept sheet pixelify · giriş portalı objesi · portal için yeni kemer sanatı (4 mevcut adaydan seçilir).

## E) EN KISA UYGULANABİLİR PLAN
**İlk 24 saat:** (0) Kullanıcı 15-dk playtest (bugünkü fix'lerin his onayı) → (1) T1 ScreenshotMode+seed [cx] → (2) T2 başlangıç: 8 klip import+mapping [Flash/Sonnet] + juice değer pass'i [cx] + Execute prompt [cx] → ilk temiz combat/chamber kareleri.
**İlk 48 saat:** (3) T3 DoorPortal prefab+skin [cx, ax-Opus review] → (4) T4 draft chips [Sonnet/Flash] → (5) T5 chamber pass [Sonnet-MCP + cx] → (6) T6 death/victory [cx] → rapor figürleri ilk seti [Sonnet]. GATED PixelLab seansı (2 rün + boss halkası + Seal Monolith) kullanıcı müsait olduğunda araya girer.
**Routing:** yazar≠reviewer korunur; kod=cx, sahne/ayar=Sonnet-MCP, S-izole=ax-Flash, kritik review=ax-Opus-4.6.
