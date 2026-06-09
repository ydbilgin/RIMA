# RIMA — 24-SAATLİK "EN İYİ OYNANABİLİR DEMO" PLANI (KARAR)
> 2026-06-09 · Council (cx + ax-3.1-Pro + ax-3.5-Flash) + NLM canon-check + Explore ground-truth → Opus sentez.
> Amaç: jüriye gösterilecek, baştan-sona OYNANABİLİR, HATASIZ/STUCK'SIZ + mümkün olan en iyi cilalı dikey-slice. Süre: 24 saat (~12-16 aktif). Animasyon scope İÇİNDE (PixelLab user-present).

---

## 0) TL;DR — KARAR
Demo akışı **kod-olarak ZATEN TAMAM** (Explore + cx file:line doğruladı). 24 saatin işi = **STABİLİZE ET + AKILLI CİLA**, yeni sistem yazmak değil. İki katman:

- **KATMAN 1 — STABİLİTE (otonom, kullanıcı yok):** build-gap fix · düşman anim controller WIRE · panic-button · placeholder "bilinçli" cila · juice · gerçek test. → Bunlar kod-tamam demoyu jüri-güvenli yapar.
- **KATMAN 2 — WOW (user-present, kullanıcı dönünce):** PixelLab ile Warblade walk+attack üretimi · (opsiyonel) Echo→Gold ekonomi kararı · gerçek insan playtest.

**Altın kural (3.1 Pro):** Kötü görünen ama çalışan demo zayıf not alır; KIRILAN demo SIFIR alır. Önce kırılmazlık, sonra güzellik.

---

## 1) GROUND-TRUTH (Explore + cx, file:line — neyin GERÇEK neyin KAĞIT)

### GERÇEK (kod-backed, doğrulandı)
- Forced lineer sıra Combat→Combat→Merchant→Combat→Boss: `DungeonGraph.cs:71-110`, test `DemoSequenceTests.cs:20-86`. `RoomRunDirector.cs:100-109` forceDemoSequence=true + useFixedDemoCamera=true.
- Shop GERÇEK: `ShopStand.cs:50-91` ([G], Echo harcar, offer uygular, sold-out); efektler GERÇEK kod: Heal=%30 MaxHP `ShopOfferData.cs:55-73`, Damage=×1.12 `:81-99`, MaxHP=+20 `:107-124`; test `ShopOfferTests.cs:49-130`.
- 2-sınıf kilit GERÇEK: `ClassUnlockPolicy.cs:9-13` (Warblade+Elementalist), `PlayerClassManager.cs:40-47` kilitliyi reddeder; test `ClassUnlockPolicyTests.cs:26-35`.
- Boss telegraph→damage GERÇEK: `PenitentSovereign.cs:333-531` (line/circle/chain/wrath hepsi telegraph SONRA hasar).
- Victory→MainMenu timeScale=1 GERÇEK: `RoomRunDirector.cs:934-939` → `DemoCompleteOverlay.cs:37-40,143-159`.
- PauseMenu (ESC) GERÇEK: `UIManager.OnEsc` toggle + `ClosePause`→`ApplyTimeScale`→timeScale=1.

### KAĞIT / RİSK
- **🔴 BOSS BUILD-GAP (en kritik):** `RoomRunDirector.cs:687-698` boss prefab'ı SADECE UNITY_EDITOR'da AssetDatabase ile yükler, sonra `Resources.Load("Prefabs/Enemies/Boss/PenitentSovereign")` fallback. AMA `Assets/Resources/Prefabs`'ta sadece Warblade var. Standalone build'de bossPrefab serialize değilse → `ResolveBossPrefab` null → boss odası SESSİZCE temizlenmiş sayılır (`:621-625`) → **demo klimaksı KAYBOLUR.** → #1 jüri-görünür kırılma.
- **Test iddiası kanıtsız:** "594 test / 17 pre-existing fail" kök TestResults XML ile doğrulanamadı (kök XML 2026-05-18, 411 test). → Gerçek sayıyı yeniden çalıştırıp ölçeceğiz.
- **İnsan playtest YOK:** Kod fallback'leri var ama gerçek traversal/input/collision/kamera/build-paketleme jüri-riski sürüyor.

---

## 2) ANİMASYON — GERÇEK DURUM (en önemli scope kararı)
- **Player:** Warblade/Elementalist/Ranger/Shadowblade = SADECE 8 idle PNG + controller (idle_* state'leri; attack/walk state'i YOK). Hareket/saldırı görseli kod/input-driven, idle sprite üstünde. `PlayerAnimator.cs:31-37,109-124`; controller swap `PlayerClassManager.cs:154-168`. Elementalist silah-art bilinçli suppress (`:171-185`). → **Player anim = ÜRETİM gerek (user-present, PixelLab).**
- **🟢 Düşman (BÜYÜK OTONOM KAZANIM):** CLIP + CONTROLLER'lar DİSKTE VAR (Penitent/ChainWarden/FractureImp/RelicCaster Idle/Walk/Attack/Death blend-tree'li, ör. `Penitent.controller:358-606`). AMA prefab'larda `EnemyAnimator.m_Controller={fileID:0}` (BAĞLI DEĞİL); sadece HalfThrall bağlı. → **Wire et = düşmanlar canlanır, PixelLab GEREKMEZ, OTONOM yapılabilir.**
- **Boss:** PenitentSovereign prefab placeholder Sprite (null) + kod/telegraph/VFX; Penitent.controller VAR ama bağlı değil. → Araştır: temiz wire'lanırsa yap, sprite gerekiyorsa user-present.

**Strateji (council konsensüs):** TAM set animasyon (2 sınıf + tüm mob + boss × 8-yön × idle/walk/attack/hurt/death) = 24h için ÖLÜMCÜL scope-creep. Sıra: **(1) mevcut düşman controller'larını WIRE et** (en yüksek ROI, otonom) → **(2) Warblade'e minimal walk+attack ÜRET** (user-present) → **(3) boss görünür idle/attack** zaman kalırsa. Elementalist + diğer mob'lar kod-driven kalır.

---

## 3) NLM CANON TUTARSIZLIK KONTROLÜ (sync + sor)
NLM (kanon) demo akışını onayladı — **TEK majör çelişki:**
- **🟡 Echo ≠ run-currency:** "Shattered Echo (◈)" KALICI META-currency'dir (PlayerPrefs, run-SONRASI chamber'da sınıf/kalıcı-upgrade açar). Run-içi shop **Gold** (geçici, in-run) harcamalı. Shop'un Heal/MaxHP satması kanona uygun; ama **Echo ile satması meta/run ekonomi duvarını yıkar.** Fix = run-scoped Gold cüzdanı (clear=Gold ödül, shop Gold harcar, HUD Gold gösterir; Echo meta kalır).
- **Jüri-etkisi DÜŞÜK** (juror Echo/Gold ayrımını fark etmez; DemoStartingEcho her run reset'liyse fonksiyonel sorun maskeleniyor) ama **kanon-doğru + "meta-cüzdan drenajı" davranışını önler.** → **KULLANICIYLA KARAR** (orta efor, en invaziv değişiklik → otonom YAPMA).
- Tutarlı olanlar ✅: Warblade+Elementalist (Faz1+Faz2 ilk alt sınıf) · PenitentSovereign (kanonik Act-1 boss, demo kapanışı) · Chamber/Attunement possession (CharSelect v4 vizyonu — kanon [E], bizde [G]; iç-tutarlılık için [G] kalır) · lineer 5-oda (kanonik "Demo Skeleton", kasıtlı basitleştirme). Not: kanon oda-4 = Vestibule/Reward (shop opsiyonel); biz shop'u çekirdek oda yaptık = savunulabilir demo seçimi.

---

## 4) EN BÜYÜK KIRILMA RİSKİ + DE-RISK
1. **Boss build-gap** (yukarıda) → İLK İŞ fix + standalone build smoke.
2. **UI-state çakışması** (3.1 Pro): Shop'ta item alırken ESC, draft-timeout anı, sahne-geçişte spam. → **PANIC BUTTON (F12):** timeScale=1'e zorla + tüm overlay'leri kapat + bir sonraki odaya/exit'e force-advance. Jüri önünde softlock olursa "burada boss-faza geçiyoruz" deyip F12 ile kurtar.
3. **Editör-demo riski:** editörden sunum amatör + riskli (yanlış tık, arka-derleme, perf drop). → **Standalone build'den göster** (build-gap fix bunu açar).

---

## 5) YOL HARİTASI — KATMAN 1 (OTONOM, kullanıcı dinlenirken — Sonnet + Unity MCP)
Sıra = izolasyon/risk önceliği. Her item: küçük/güvenli değişiklik → read_console 0-error → sonraki.

| # | İş | Dosya/hedef | Kabul kriteri | Risk |
|---|---|---|---|---|
| A1 | **Boss build-gap fix** | _Arena RoomRunDirector.bossPrefab serialize VEYA prefab→Resources | build'de ResolveBossPrefab non-null | düşük |
| A2 | **Düşman controller WIRE** | demo-spawn mob prefab'ları EnemyAnimator.m_Controller | demo mob'ları idle/walk/attack oynatır | düşük |
| A3 | **Boss anim araştır** | PenitentSovereign + Penitent.controller | temizse wire, değilse rapor | düşük |
| A4 | **Panic Button F12** | UIManager / global input | F12 → timeScale=1 + overlay-kapat + force-advance | düşük |
| A5 | **Placeholder "bilinçli" cila** | shop stand + Elementalist disc: palet-rengi + bob/dönme (KOD-only, PixelLab YOK) | "sistem var, bug değil" hissi | düşük |
| A6 | **Juice quick-win** | hit-flash audit + Warblade/boss vuruşta camera-shake (varsa reuse) | vuruş hissi artar, 0 regresyon | orta (cerrahi) |
| A7 | **Gerçek test + console gate** | run_tests EditMode | gerçek pass/fail sayısı + 0 yeni fail | — |

*(A1+A2+A3 = ilk Sonnet ajanı "asset hardening", IN-FLIGHT. A4-A6 = ikinci ajan. A7 = her ajan sonu.)*

## 6) YOL HARİTASI — KATMAN 2 (USER-PRESENT, kullanıcı dönünce)
- **B1 — Animasyon jam (PixelLab):** Warblade walk + attack (en çok görülen). Method = `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md` + VERIFY-LIVE. Entegrasyon vergisini bütçele (Animator state + trigger wire). Her anim bağımsız-revert-edilebilir (bozulursa kod-driven'a dön).
- **B2 — Echo→Gold kararı** (NLM canon): yap/erteleme kullanıcıyla.
- **B3 — Gerçek 5-senaryo playtest** (build üstünde): (1) ölüm→restart (2) shop ekonomi/satın-alma HP/dmg/MaxHP mutasyonu (3) boss telegraph okunur + kaçış (4) her odada ESC pause/resume (5) tam-tur 0-stuck.
- **B4 — Final build + FREEZE + prova:** son release build, loga bakmadan baştan-sona oyna, kod dondur, F12 teyit.

## 7) CUT LİSTESİ (24h'de YAPMA — council oybirliği)
10-sınıf animasyon üretimi · her mob için tam 8-yön full-state set · legacy `_IsoGame` decommission (post-demo, `OVERLAP_CLEANUP_DECISION`) · yeni boss faz/mekanik · readable-stand ötesi shop sanatı · kilitli sınıf unlock-progression · derin balance pass (sadece bariz unfair/stuck) · her placeholder'a pixel-perfect cila.

## 8) ROUTING (2026-06-09 LOCK)
Production (Unity MCP/C#/prefab/asset/bug) = **Sonnet** (bol limit). Opus = sadece derin tasarım/strateji/review (bu plan + Echo→Gold + animasyon entegrasyon mimarisi). cx = opsiyonel advisor (kota ölü). [[feedback-sonnet-default-opus-deep-routing]]. Her Sonnet dispatch: ACTIVE RULES + NLM ACCESS + DELEGe-ETME + DO-NOT-commit (orchestrator QC+commit).
