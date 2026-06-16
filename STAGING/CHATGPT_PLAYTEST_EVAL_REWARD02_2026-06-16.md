# ChatGPT Playtest Paketi Değerlendirmesi + REWARD-02 Root-Cause/Fix (2026-06-16)

Kaynak paket: `RIMA_CLAUDE_PLAYTEST_UI_COMPLETE_2026-06-16.zip` (kullanıcının Downloads'ında; ChatGPT ile yapılan playtest+UI çalışması). 6 gerçek ekran görüntüsü + bug register + root-cause docs + UI polish + asset pack + Rift-Forged Egg + roadmap.

## Paket triyajı (RIMA verified status'a göre çapraz)

- 🔴 **KRİTİK/YENİ — REWARD-02 (G basınca ödül UI açılmıyor):** Bizim "F2 GREEN, 0 fix" verdict'imizin caveat'ı GERÇEKLEŞTİ. Detay + fix aşağıda.
- ⛔ **CANON ÇATIŞMASI — REDDEDİLDİ:** Paketin `01_SOURCE_PRIORITY` + `06_AIM_FACING` dosyaları "35° ARPG, S/E/N/W **4-cardinal**, flip YOK, her yön ayrı sprite" diyor. RIMA kanonu = **8 yön LOCKED (Karar #114): 5 sprite + 3 mirror flipX**. ChatGPT halüsinasyonu/regresyon. AIM-01/02 *bug'ları gerçek* (SS-03: gövde öne, kılıç sağa) ve "tek aim kaynağı/AimSnapshot, gövde+silah aynı facing" prensibi sağlam & kanonla uyumlu — ama "4-cardinal+flip-yok" reçetesini UYGULAMA, 8-yön'e uyarla.
- 🟡 **Zaten biliniyor (teyit):** REWARD-01 (stale reward, SS-05) = backlog **F1**. UI-01 (reward kartı footer dikey çöküş, SS-04) ≈ **U1**. HUD-01/UI-03/04/05 = **U2/U3/U4** + polish.
- 🟢 **Değerli yeni katkı:** reward session-ownership root-cause modeli (`05_*`); LIFE-01 (`BuildPlacementController` scene-close leak / lazy-singleton respawn — demo dev-direct `_Arena`'da BuildMode kurulmadığı için videoyu vurmaz ama gerçek); DATA-01 ("eşleşir"→trigger+outcome); Rift-Forged Egg = yeni ekonomi DEĞİL, mevcut reward'ların mystery-presentation katmanı (post-demo, modüler-tasarım felsefesiyle uyumlu).
- ⚠️ `08_CANONICAL_REFERENCE_DOCS/` = ChatGPT'nin RIMA doc kopyaları/rekonstrüksiyonu — NLM canon ile birebir varsayma.

## REWARD-02 — DOĞRULANDI (gerçek bug, demo-kritik)

Üçlü kanıt: (1) SS-05 gerçek-oynayış screenshot "G ile açılmıyor"; (2) kod mekanizması; (3) bizim GREEN verdict'imiz sadece ForceCollect (menzil-bypass) manuel çağrısıyla yeşildi.

**Kök neden (kod, doğrulanmış):**
- `RoomRunDirector.ResolveRewardSpawnPosition()` (RoomRunDirector.cs ~1469-1487) reward'ı **oda MERKEZİNE** spawn eder, oyuncunun önüne değil.
- `RewardPickup.cs`: G yalnız `playerInRange==true` iken çalışır (satır 64-67); `playerInRange` **SADECE `OnTriggerEnter2D`** ile set edilir (satır 70-76).
- Unity 2D: bir trigger collider **zaten üst üste binmiş halde spawn olursa `OnTriggerEnter2D` ATEŞLEMEZ** (enter = geçiş eventi). Combat bittiğinde oyuncu sıklıkla tam merkezdedir → reward üstüne çıkar → enter olmaz → `playerInRange=false` → **G ölü.**
- `RewardAutoCollectTimeoutSec = 0f` (RoomRunDirector.cs ~1194) → ForceCollect timeout KAPALI → güvenlik ağı yok → soft-lock.
- Aralıklı: sadece oyuncu clear anında merkezdeyse tetiklenir (playtest "4. oda" gözlemiyle tutarlı). Golden-path videosunun "reward'a yürü→G→3 kart" beat'i risk altında.

**ÖNERİLEN FIX (cerrahi, golden-path güvenli, tek dosya `Assets/Scripts/Core/RewardPickup.cs`):**
1. `OnTriggerStay2D(Collider2D other)` ekle: Player tag → `playerInRange` zaten true değilse `ShowPrompt()` + `playerInRange=true`. (Stay, spawn-anı çakışmada sonraki frame'lerde ateşler; Enter ateşlemese de yakalar.)
   VE/VEYA
2. `Awake()` sonunda `Physics2D.OverlapCircle(transform.position, colliderRadius)` ile Player zaten içinde mi kontrol et → ise `playerInRange=true`+prompt.
- Bu, G'yi "oyuncu içeri yürüdü mü / reward üstüne mi spawn oldu" bağımsız hale getirir, kök nedeni çözer, hem RoomRunDirector hem RuntimeRoomManager yollarını kapsar (ikisi de aynı `RewardPickup`).
- Routing: crafter-sonnet (iyi-specli cerrahi) + auditor gate (writer≠reviewer). Verify: read_console 0-error + canlı repro (merkez-clear senaryosu, ForceCollect'siz G).
- ⚠️ NO-REFACTOR: TimeoutSec'i yeniden açma / iki reward sistemini konsolide etme (post-demo). Sadece RewardPickup trigger-robustness.

**Yan bulgu (post-demo):** İki paralel reward sistemi (RoomRunDirector merkeze-spawn / RuntimeRoomManager öne-spawn + `ClearActiveRewards()`). REWARD-01 (stale) muhtemelen bu çift-sistem temizlik uyumsuzluğu. Konsolidasyon = post-demo.
