# V2 SUNUM PLANI — COUNCIL KARARI (2026-06-06)
**Konu:** ChatGPT V2 planı (`C:\Users\ydbil\Downloads\RIMA_Ayrintili_Gorsel_Oynanis_Raporu_V2.md`) kabul/red/revize.
**Council:** cx-laurethayday (feasibility, `CODEX_DONE_laurethayday.md`) ‖ ax-3.1-Pro (mimari) ‖ ax-3.5-Flash (lean) ‖ Opus-advisor (tasarım) → Opus orchestrator sentezi.

## ANA SENTEZ BULGUSU
V2'nin teşhisi 4/4 oybirliğiyle DOĞRU ("sistem var, ekranlar değerini göstermiyor") ama V2 "sıfırdan inşa" varsayıyor. **cx kanıtladı: juice driver'ları, skill tag'leri, parallax, mood-light, audio hook'ları ZATEN KODDA** — gerçek iş çoğunlukla **bağlama + tuning + içerik**, inşa değil. Bu, V2'nin 7-günlük planını ~%40 küçültür.

## KABUL / RED / REVİZE TABLOSU

| V2 maddesi | Karar | Gerekçe (advisor mutabakatı) |
|---|---|---|
| ScreenshotMode (s3) | ✅ KABUL (küçültülmüş) | 4/4 en yüksek ROI. Tam controller değil: küçük runtime toggle + debug-surface registry (DemoDebugPanel F1, F2 overlay, dummy-HP, door marker, death overlay) + GameViewSetup reuse + **deterministik seed** (3.1'in kör noktası: BridsonPoisson random — rapor görseli tekrarlanamaz). Size S/M. |
| Dünya-içi kapı portalları (s9, 20) | ✅ KABUL (görsel katman olarak) | İskelet `20d1f09c` ile ZATEN BİZDE (socket+row+trigger+validator). Eksik = `DoorPortalVisual`: tip-başına rün + açılma tween + yaklaşınca highlight. **6-durumlu FSM RED** (3.1: anti-pattern, RoomRunDirector source-of-truth; 2 state + tween). **Danger pip RED** (Opus: mobil-RPG dili; tehlike = portal görsel dili — Elite çatlak çerçeve+magenta). Ödül metni OK ("Rare+"). |
| Chamber atmosfer (s5-6) | ✅ KABUL (revize) | Pedestal Light2D + mor ambient + bürünme VFX-lite (Flash: patlama particle + float text yeter; tam 6-aşama sonra) + prompt restyle. **Ek sıcak/turuncu ışık pass'i YOK** (Opus: cyan-on-void imza; mevcut brazier PROP'ları canon, kalır — ama ışık dili cyan/mor egemen, 60-30-10). + **cyan zemin yönlendirme çizgisi** (ilk-5-dk lite, 3 sprite). |
| Combat juice (s7, 21) | ✅ KABUL (tuning olarak) | HitPauseDriver/ScreenShakeDriver/HitFlashDriver/BrokenStateVisual VAR; mevcut değerler V2'ye yakın (hit 0.04/crit 0.07/kill 0.12). İş = serialized tuning + **[RMB] Execute dünya-prompt'u** (4/4 oybirliği: en iyi tek fikir — Sundered Beat imzasını tutorial'sız öğretir; DeathBlow zaten Broken/Sundered'a kilitli). ⚠️ Düşman hit-flash'ı cyan DEĞİL (beyaz+magenta rim); cyan=oyuncu. ⚠️ timeScale hit-stop fizik bozarsa anında geri çek (3.1). |
| Draft UI (s8, 23) | ✅ KABUL (revize) | SkillOfferUI'da tier chip/glow/tooltip/synergy-pulse ZATEN VAR. Eksik = tag chip'leri (`SkillData.tags` mevcut; max 5 tag) + sinerji satırı + seçim mikro-anim. **"Build Meter" RED** (Opus: ton kırıcı spreadsheet; sinerji-parlaması aynı bilgiyi diegetik verir). Dinamik BuildSynergyEvaluator YOK — ChainWindowTracker reuse (3.1+cx). |
| RoomVisualProfile (s11) | 🔶 REVİZE/KISMEN | RoomVisualProfileSO VAR ama bağlı değil. 5 tema RED (Opus: tema başına 1 örnek = çeşitlilik paradoksu). Demo: **parallax + mood-light binding** (ParallaxLayer/RoomMoodLightPool mevcut — void derinliği) + decal havuzu; tam tema sistemi POST-DEMO. |
| Minimum ses (s25) | ✅ KABUL (küçültülmüş) | AudioManager + hook'lar VAR (muted fallback!). İş = **gerçek klip yükle** (Flash 8-SFX listesi: swing/impact/dash/death/execute/clear/draft/chamber-ambient; Kenney+Sonniss CC0) mevcut hook'lara. AudioMixer/bus stack POST-DEMO. 3.1 haklı: SFX juice'tan önce/birlikte girmeli. |
| Echo run-sonu dökümü (s24.3) | ✅ KABUL | RunStats+EchoWallet hazır; düz satırı kaynak-satırlarına çevir (oda/kill/elite bonus). S boyut, jüri "neden kazandım"ı görür. |
| Chamber Restoration (s24.2) | ⏸️ DEFER (post-demo) | Opus 1. sıraya koydu AMA cx M/L ölçtü (save-state+data+UI+görsel). Deadline'da risk. Demo'da: pedestal'da **achievement progress göstergesi** (S) yeter. Kozmetik aura + memory fragments RED/DEFER. |
| Mob 4-arketip (s22.1) | ✅ KABUL | 13 placeholder → 4 keskin arketip; SIFIR yeni sprite (ölçek+rim+davranış: Chaser 0.85×/magenta · Brute 1.15×/koyu kırmızı/koni · Ranged+asa-prop/çizgi · Bomber+cyan-core/daire). Telegraph = ground-decal+LineRenderer. Telegraph rengi KIRMIZI/MOR (cyan asla). |
| Boss (s22.2) | 🔶 MİNİMAL + KARAR-AÇIK | Mevcut boss kalır; minimum: 1.15-2.5× ölçek + rim + 1-2 telegraph. Opus'un "boss = odanın kendisi (merkezi rift + Broken add'ler)" fikri GÜÇLÜ ama deadline riski → **KULLANICI KARARI: post-demo mu, şimdi mi?** (önerimiz: post-demo). |
| İlk-5-dk akışı (s26) | 🔶 LİTE | 7-beat değil 3-beat (Opus): kamera nudge + cyan zemin çizgisi (spawn→pedestal→rift) + bürünme sonrası rift açılma VFX. Combat içi tuş yazıları SKIP. |
| Rapor dürüstlük (s27) | ✅ KABUL AYNEN | "Veri modeli 10 sınıf destekler; demo 4 uçtan uca kit ile pipeline'ı kanıtlar." Test sayıları birebir eşleşecek. 4/4 oybirliği. |
| Rapor görselleri (s4, 12-18) | ✅ KABUL (fake-it ayrımıyla) | Şekil 8 diyagram=harici araç · Şekil 11 sheet=manuel kompozisyon (dürüst) · Şekil 14 QC=staged before/after (gerçek QC geçmişimiz var, `90c84995` evidence) · Şekil 13=Test Runner gerçek koşu. ⚠️ Flash'ın "Map Designer mockup yap" önerisi YANLIŞ — 8 sekme ÇALIŞIYOR (`268848ce`), GERÇEK screenshot alınır. Şekil 12=pedestal yakın çekim (duplicate çözümü). |
| 7-günlük plan (s30) | 🔶 REVİZE | Sıra değişti (aşağıda). Flash'ın 3-günlük çekirdeği + 3.1'in bağımlılık düzeltmeleri (SFX juice'la birlikte; zemin/Y-sort kapıdan önce — ✅ bugün zaten shipped). |

## RED LİSTESİ (net)
Danger pips · Build meter · 6-durum kapı FSM · 5-tema profil · BuildSynergyEvaluator · AudioMixer stack · kozmetik aura · memory fragments · sıcak ışık pass'i · combat-içi tutorial yazıları · 8-yön kapı desteği (V2 de reddediyor) · Map Designer mockup (gerçeği var).

## ÖNCELİK SIRASI (deadline'a göre, bağımlılık-düzeltilmiş)
1. **ScreenshotMode-lite + deterministik seed** [S/M] — her şeyin ön koşulu
2. **Combat juice tuning + [RMB] Execute prompt + SFX ilk pass (birlikte)** [M] — canlı demo hissinin kalbi
3. **Kapı portal görsel katmanı** (rün seti + tween + highlight; pip yok) [S/M]
4. **Draft readability** (tag chip + sinerji satırı + seçim anim; meter yok) [S/M]
5. **Chamber atmosfer + cyan yönlendirme + bürünme VFX-lite** [M]
6. **Echo run-sonu döküm paneli + ölüm/zafer anı mikro-pass** (Opus'un V2-eksiği: son vuruş slow-mo ölümde, boss-kill büyük payoff) [S/M]
7. **Mob 4-arketip konsolidasyonu + telegraph'lar** [M]
8. **Void parallax + mood-light binding** [M]
9. **Rapor görsel seti** (yeni temiz screenshot'lar + 5 eksik şekil + dürüstlük pass'i) [S/M]

## SAATLİ BOMBA UYARILARI (3.1+Flash mutabık)
- **URP 2D Light:** chamber'da Global Light2D zaten çalışıyor ama oda sahnelerinde materyal (Lit/Unlit) karışıksa siyah-ekran/FPS riski — ışık işine küçük başla, jüri donanımında test et.
- **Parallax:** pixel-jitter + sorting riski — Pixel Perfect Camera ile birlikte test.
- **timeScale hit-stop:** fizik/coroutine bozulursa derhal kaldır (zaten FeelToggleSettings'te kapatılabilir).

## KARAR-AÇIK (kullanıcıya)
1. Boss: minimal polish (öneri) mi, "boss=oda" yeniden tasarımı mı?
2. Canlı demo / screenshot-only ayrımı: 1-5 canlı-kritik, 6-9 esnek — onay?
