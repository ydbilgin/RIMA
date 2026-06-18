# ADVERSARIAL CRITIC — Demo-arifesi sıradaki iş planı (2026-06-18)

**Rol:** Bağımsız blind adversarial. Görev = sentez planını ÇÜRÜTMEK. Kod-kanıtı toplandı (read-only).
**Verdict: REVISE** (plan çoğunlukla sağlam ama 3 madde yanlış-spesifikasyonlu + bir FAZ-0 adımı stale).

---

## ÖZET BULGULAR (kanıtlı)
1. **A4 Merchant fix YANLIŞ-SPESİFİK + yeni demo-killer riski** — en ağır itiraz.
2. **FAZ-0 exposure adımı STALE/no-op** — ArenaPostFX.asset zaten `postExposure 0.6`.
3. **A1 failed-cast feedback eksik-spesifik** — cooldown-spam riski; "core'a dokunmaz" savı zayıf.
4. **A3 Director dup-slot "UI-only" savı YANLIŞ** — fix DraftManager skill-equip path'inde, combat-bitişik.
5. **B2 de-stack üç-yönlü overlay sorunu, plandan daha karmaşık** (ama yine de düşük risk).

---

## EKSEN 1 — Scope-drift / over-reach
FAZ-1'de 6 madde (B2'den sonra koşullu hit-flash dahil 7) demo-arifesi için **fazla**. Minimal-batch savı GEÇERLİ:
- **Kesilmeli #1: A4 Merchant** (aşağıda gerekçe — net yeni-bug riski).
- **Kesilmeli #2: B2 low-HP/Rage de-stack** — bu GÖRSEL bir glitch, sunumda ancak oyuncu <%20 HP + yüksek-Rage anında AYNI anda görünür (nadir kesişim). Risk düşük ama demo-değeri de düşük; FAZ-1'i şişiriyor. **POST-DEMO'ya ertele veya en sona koşullu.**
- **Koşullu hit-flash (madde 6):** zaten plan "SKIP + raporla" diyor — kabul, ama aşağıda hit-flash ZATEN VAR (iki ayrı driver self-wired). Yeni iş gereksiz → **NET SKIP.**
- **Tutulmalı (gerçek demo-değer):** A1 (revize), A3 (revize spec). Bu ikisi "sessiz no-op" ve "Director centerpiece bozuk slot" — canlı sunumda gerçekten görünür.

**Sonuç:** FAZ-1 = 2 madde (A1 + A3), düzgün spec ile. A4 ve B2 kesilir/ertelenir.

## EKSEN 2 — Gizli combat-path teması (sinsi sızıntı var mı?)
- **A1 (failed-cast):** Feedback ucu güvenli AMA tetik-noktası combat-bitişik. `TryActivate()` bool döner; class-controller'ların ÇOĞU dönüşü ATIYOR (`slots[idx].TryActivate();` — Ranger/Elementalist/Ronin/Shadowblade). Sadece `Warblade_SkillController` `bool used = TryActivate(); if(!used) return;` ile bakıyor. Feedback eklemek için ya her controller'ı düzenle (5 dosya, combat input path) ya da SkillBase'e feedback-hook göm. **"Core'a dokunmaz" savı kısmen yanlış** — input-execution path'ine yayılıyor.
- **A3 (Director dup-slot):** `DirectorMode.AssignSelectedSkillToSlot` → `DraftManager.TryDirectorAssignSkill(skill, slot)` → `AddComponent(skill.skillType) as SkillBase` (DraftManager.cs:744). Yani slot-assign GERÇEK skill-component instantiate ediyor. Fix UI'da değil, **DraftManager equip-mantığında** = combat-bitişik. "Loadout/slot validation seviyesi, combat numerics'e dokunma" savı teknik olarak doğru (numerics değil) ama dosya combat-equip path'i; dikkatli olunmalı.
- **A4 (Merchant):** Aşağıda — currency-source migration, en sızıntılı madde.

## EKSEN 3 — Merchant (A4) DAHİL kararı DOĞRU MU? → **HAYIR. ax(w1.5 ertele) HAKLI.**
Kanıt:
- `EchoWallet` = **PlayerPrefs disk-persist** (`rima_demo_echo_balance`, start 200). Meta-currency.
- `PlayerEconomy.Gold` = **in-memory run-local**, `startingGold = 0`.
- `PlayerEconomy.Gold` SADECE 2 kaynaktan besleniyor: `DraftManager:455` ve `ChestBehavior:160` (`AddGold`). **Merchant odasına gelene kadar Gold genelde 0** (chest/draft gold-reward seçildiyse ≠0).
- Plan "spend kaynağını `PlayerEconomy.Gold`'a sabitle" diyor. **Eğer Gold=0 ise merchant'ta HİÇBİR ŞEY alınamaz = YENİ demo-killer** (sunumda "shop bozuk").
- Ayrıca `ShopStand` her yerde `EchoWallet.TrySpend`/`Balance` + label `"{cost} Echo"` + prompt `"... Echo"` kullanıyor. Gold'a migrasyon = UI label + prompt + spend + balance-check + 3-4 string. Bu **demo-arifesi cerrahi-fix değil, currency-migration.**
- Plan'ın alternatifi ("persistent-spend yolunu demo için hard-block") = merchant'ı işlevsiz bırakır → centerpiece'i kırar.

**BINDING:** A4'ü FAZ-1'den ÇIKAR. Eğer "Echo'nun kalıcı drain'i" gerçek itirazsa, **minimal fix = run-başında EchoWallet'i demo-default'a reset** (PlayerPrefs.SetInt start'ta) — currency-source migrasyonu DEĞİL. Ama bu bile demo-arifesi gereksiz; **POST-DEMO.**

## EKSEN 4 — Eksik guardrail (her FAZ-1 maddesi)
**A1 failed-cast — KRİTİK eksik guardrail:**
- `TryActivate()` 3 sebeple false döner: (a) `!IsReady` = COOLDOWN (normal, beklenen — feedback İSTENMEZ), (b) yetersiz rage/resource, (c) `!CanExecute()` veto.
- Plan "veto + yetersiz-kaynak durumunda feedback" diyor ama **cooldown-vs-gerçek-fail ayrımını şart koşmuyor.** Eğer feedback `TryActivate==false`'a takılırsa: oyuncu cooldown'daki tuşa her bastığında SFX+flash+toast → **spam, sunumda berbat.** 
- **BINDING:** Feedback SADECE (b)+(c) için. `IsReady` true iken fail = gerçek-fail. SkillBase'e ayrı `LastFailReason` enum'u veya `TryActivate` çıkışında `IsReady && !CanExecute()` / resource-fail ayrımı; cooldown-fail SESSİZ kalır. Bu ayrım olmadan A1 net-negatif.
- İkinci guardrail: feedback geçerli cast'i tetiklememeli → başarılı dalda (`Execute()` sonrası) feedback ÇAĞRILMAZ; sadece erken-return dallarında. Tetik noktası `TryActivate` içinde return-öncesi olmalı, caller'da değil (caller'lar dönüşü atıyor).

**A3 Director dup-slot — guardrail:**
- "Shared-component davranışı": aynı `skillType` 2 slota atanırsa `DraftManager` ya aynı component'i 2 slota map'ler (gerçek shared-cd bug) ya 2. AddComponent başarısız olur. Fix = `TryDirectorAssignSkill` içinde "bu skillType zaten başka slotta mı?" check → reject/swap, `out error` ile status.
- **Guardrail:** Fix MEVCUT `out string error` mekanizmasını kullanmalı (UI zaten gösteriyor: `status.assign_failed`). Yeni UI gerekmez. Reject yerine "swap" seçilirse: eski slot temizlenmeli yoksa orphan component kalır (leak). **Reject (daha güvenli) öner, swap DEĞİL** — demo-arifesi.

**A4 (kesilirse guardrail gereksiz).**

**B2 de-stack — guardrail:**
- İKİ ayrı fullscreen overlay var: `HUDController.lowHpVignette` (alpha 0.12–0.18, `hitFlashActive` guard + `Mathf.Max` composite ZATEN VAR) ve `RageVisualFeedback` (ayrı `RageVignette` Image, alpha 0–0.5, ayrı MonoBehaviour Update). İkisi BAĞIMSIZ alpha yazıyor, aynı Canvas'a iki Image. "De-stack" = bu ikisini arbitrate et. HUDController kendi içinde zaten arbitre ediyor; sorun **iki sistemin birbirinden habersiz** olması.
- Bu plandan daha karmaşık (cross-system). Düşük demo-değer + nadir kesişim → **ERTELE.**

## EKSEN 5 — Sıra/commit + exposure
- **Polish'i ayrı commit:** DOĞRU, onaylanır. Gameplay-fix revert edilebilirliği için izolasyon mantıklı.
- **Exposure 0.6 — STALE ADIM:** `Assets/Settings/ArenaPostFX.asset` ZATEN `postExposure m_Value: 0.6, m_OverrideState: 1` (uncommitted working-tree). Plan "0.35→0.6 aç" diyor ama **0.6 zaten yazılı.** FAZ-0'ın exposure-değiştir adımı no-op. Geriye SADECE: (1) screenshot-verify, (2) commit. **BINDING:** FAZ-0'ı "0.6 ZATEN uygulanmış, screenshot doğrula + commit" olarak düzelt; "0.35→0.6" yanlış başlangıç-değeri varsayımı.
- **Canon ihlali (0.6 fazla mı?):** Memory canon = "ambient 0.22 · moody". postExposure 0.6 = +0.6 EV ≈ %50 parlaklık artışı. Karanlık-atmosfer tezini gevşetir AMA bu zaten uygulanmış+council-onaylı (UNANIMOUS Risk:0). İtiraz: screenshot ile canon-uyumu DOĞRULANMADAN commit etme; 0.6 fazla gelirse 0.45 ara-değer. **Soft uyarı, blocker değil.**

---

## BINDING FIXES (uygulanacak, madde-madde)
1. **A4 Merchant → FAZ-1'den ÇIKAR (POST-DEMO).** ax(w1.5) haklı. Currency-source migration demo-arifesi yeni-bug riski (Gold=0 → shop ölü). Gerekirse minimal = run-start EchoWallet reset, migration DEĞİL.
2. **FAZ-0 exposure → "0.6 ZATEN uygulanmış" olarak düzelt.** Adım = screenshot-verify (canon-uyumu) + commit. "0.35→0.6 değiştir" no-op/yanlış.
3. **A1 → spec'e cooldown-ayrımı ZORUNLU ekle.** Feedback yalnız resource-fail + CanExecute-veto; `IsReady`-fail (cooldown) SESSİZ. Tetik `TryActivate` içinde return-öncesi (caller'lar bool'u atıyor). Aksi halde A1 net-negatif (spam).
4. **A3 → "swap değil REJECT" + mevcut `out error`/`status.assign_failed` UI'ı kullan.** Fix yeri = `DraftManager.TryDirectorAssignSkill` (DraftManager.cs ~424/744), UI değil. Orphan-component leak'e dikkat (reject güvenli).
5. **B2 + hit-flash → ERTELE/SKIP.** Hit-flash ZATEN var (HitFlash.cs + HitFlashDriver.cs self-wired) → yeni iş gereksiz, NET SKIP. B2 üç-yönlü cross-system, nadir kesişim, düşük demo-değer → POST-DEMO.

**Revize FAZ-1 = sadece A1 (cooldown-ayrımlı) + A3 (reject).** İkisi de gerçek "canlı sunumda bozuk görünür" + spec netleştirildi. Diğerleri kesildi/ertelendi → minimal-batch + düşük-risk demo-arifesi.
