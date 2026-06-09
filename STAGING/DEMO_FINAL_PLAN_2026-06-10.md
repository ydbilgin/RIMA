# DEMO FINAL PLAN — 2026-06-10 🔒 LOCKED (Capstone Vertical Slice)

> Kaynaklar: ChatGPT repo-review + Sonnet redundancy audit + Opus kod-doğrulaması (3 bağımsız kaynak yakınsadı).
> Capstone gerçeği: not SİSTEMLER üzerinden verilir. 9 taahhüt sistemin 9'u kodda VAR.
> TEZ: Demo'nun tek gerçek boşluğu = dual-class'ın OYNANARAK kanıtlanması. Geri kalan her şey bunun etrafında.

## OPUS NET GÖRÜŞ (planın gerekçesi)
1. **Dual-class = projenin tezi ve şu an SIFIR runtime doğrulaması olan tek yeni kod.** En büyük risk boss build-gap değil (kapatıldı), bu zincirin kendisi. Her şey tek bir uçtan-uca kanıt kapısına (G1) bağlanır.
2. **Post-boss oda = KABUL.** "Seçim ekranı" → "oynanış" farkı jüri gözünde kategorik. Maliyet düşük (lineer sequence + mevcut combat node mekaniği), değer en yüksek.
3. **Tek playtest kapısı, iki değil.** Mevcut gate + P0'ı ayrı ayrı playtest etmek kullanıcı-zamanı israfı; P0 bitince zincirin TAMAMI bir kez derin test edilir.
4. **İki gizli mayın bulundu, P0 kapsamına alındı:**
   - `PlayerClassManager.SelectSecondaryClass` primary'yi REDDETMİYOR (`:64-66`) + `ClassSelectionUI.DemoChoices` statik Elementalist/Ranger → primary=Elementalist iken Elementalist kartı seçilebilir = çift sınıf. FIX: kart listesi primary'yi dışlar + manager'a `type == PrimaryClass` guard.
   - `ScreenShakeDriver` sahne-bağımlı; `_Arena`'da juice YOK (CURRENT_STATUS A6) → shake fix'i driver-bootstrap olmadan yine null'a gider. FIX driver garantisini içerir; fixed-demo-camera ile çatışırsa shake DÜŞÜRÜLÜR (kamera otoritesi kazanır).
5. **Animasyon = cila, kritik yol değil; ama RUN×2 en yüksek algı sıçraması.** Üretim async (kullanıcı PixelLab'da) → kod şeridiyle TAM PARALEL, birbirini beklemez.
6. **Feature freeze disiplini:** teslimden 24h önce KOD DONAR; sadece bugfix + build + prova. Capstone demolarını batıran şey son gece eklenen özelliktir.
7. **Ranger kartı koşullu:** kart kalır (seçim gerçek hissettirir) ama jüri senaryosu Elementalist seçer; G1'de Ranger smoke-test NRE verirse kart Elementalist-tek'e düşer. Tartışma yok, kural bu.

## ŞERİTLER (paralel, birbirini beklemez)

### LANE A — KOD (Sonnet dispatch + Opus review) — kritik yol
**A1 · P0 Dual-class oynanış kanıtı:**
- `DungeonGraph.BuildDemoSequence()` → 6 node: Combat·Combat·Merchant·Combat·Boss·**PostBoss(Combat)**
- `RoomRunDirector` boss-clear dalı (`:936-957`): draft bitince DemoComplete DEĞİL → PostBoss kapısı; PostBoss clear → DemoComplete
- "İKİNCİL SINIF AÇILDI" banner (~2.5s, büyük) + secondary kaynak barı görünürlüğü
- PostBoss wave: 3-4 düşük-HP FractureImp (zorluk değil, kanıt)
- **Edge-case fix:** ClassSelectionUI kartları primary'yi dışlar + `SelectSecondaryClass`'a primary-guard
- Kabul: sequence testleri güncel + 0 yeni fail; akış = boss→seçim→draft→kapı→mini oda→secondary skill→victory
**A2 · Boss shake restore:** `PenitentSovereign.cs` 6× ölü `ScreenShake.Instance` → `ScreenShakeDriver.Instance?.Shake()` + `_Arena`'da driver garantisi (runtime bootstrap). Kabul: boss vuruşunda gözle görülür shake, fixed-camera bozulmaz; çatışırsa shake düşür (P4'e).
**A3 · C3 çift-wiring:** `CharacterSelect.unity` aynı GO'da Controller+Screen ikisi enabled → otoriteyi belirle, diğerini disable (çift `_Arena` yükleme riski).
**A4 · P2 Enemy variety:** `Act1_Wave_Pilot`'a +1-2 wired-mob (sadece veri).
Sıra: A1 → A2 → A3 → A4. A1 bitmeden playtest İSTENMEZ.

### LANE B — ASSET ÜRETİM (KULLANICI, PixelLab — şimdi başlayabilir, async)
| Sıra | İş | Yöntem | Loop |
|---|---|---|---|
| B1 | Warblade **RUN** (VERIFY-FIRST: bunu üret→"hazır" de→Claude doğrular) | template `running-8-frames`, 8-yön | AÇIK |
| B2 | Elementalist **RUN** | template `running-8-frames`, 8-yön | AÇIK |
| B3 | Warblade **ATTACK** | V3 `"two-handed overhead greatsword swing, downward chop"`, 8f, 8-yön | KAPALI |
| B4 | Elementalist **CAST** | template `fireball` (fallback V3 `"casting a spell, both hands thrust forward"`), 8-yön | KAPALI |
| B5 (ops) | Fireball flicker | `animate_object(6ca9bb15)` `"burning flame flickering, subtle pulsing glow, seamless loop"` | AÇIK |
Karakter ID: warblade `2656075d-d113-4f18-a6c1-94b5a6b8bf65` · Elementalist `4c83c0be-e856-48f1-b8b5-9626e041a082`. Limit: 1070 gen — bol.
KES: hurt/death anim, dash/channel arketipleri, diğer 8 sınıf.

### LANE C — WIRING (Claude, asset geldikçe)
- C1: Fireball 8-dir sprite import → projectile prefab + velocity→yön seçimi; `Fireball.projectilePrefab` bağla (prosedürel daire gider)
- C2: `PlayerProjectile`'a **reusable `impactVfx` particle kancası** + ateş patlaması (particle = kendi animasyonlu; PixelLab patlama ÜRETİLMEZ)
- C3: Anim wiring — Animator controller per state, **Write Defaults=OFF**, 10-12 fps, 8-yön (5 üret + 3 mirror flipX)

## KAPILAR (gate'ler — kullanıcı-present)
- **G1 · TAM ZİNCİR PLAYTESTİ** (A1-A3 bitince, TEK derin oturum): chamber→5+1 oda→boss→seçim→draft→post-boss'ta secondary skill→victory · ölüm→restart · shop 3 stand · ESC/pause · panic F12 · Ranger smoke (NRE→kart düşür). Çıktı = fix listesi.
- **G2 · STANDALONE BUILD SMOKE** (G1 fix'leri sonrası): boss spawn build'de · death→restart · draft/shop exit · dual-class zinciri build'de.
- **G3 · FREEZE** (teslim −24h): kod donar; sadece bugfix + final build + 10-dk prova (aşağıdaki senaryo).

## JÜRİ SENARYOSU (10 dk — ChatGPT akışı kabul, post-boss eklendi)
0:00 hedef cümlesi → 0:45 chamber/class-select → 1:45 Combat-1 (temel combat) → 3:00 reward draft → 4:00 Combat-2 (encounter budget+wave) → 5:00 Shop ("economy prototype" DE) → 6:00 Boss (telegraph+faz) → 8:00 **dual-class seçim + post-boss odada Warblade+Elementalist birlikte** → 9:30 victory veya ölüm→restart.
Anlatım dili LOCK: "10 sınıflık veri modeli; demo'da 2 sınıf uçtan uca" · "shop = economy prototype" · "sanat cilası sınırlı, sistemler uçtan uca çalışıyor".

## RED / DEFER (tartışması kapalı)
| Karar | Hüküm |
|---|---|
| PenitentSovereign suppress-flag patch'i (ChatGPT) | RED — director-gate zaten var, daha temiz |
| Echo→Gold rename | DEFER — invaziv, jüri-etkisi düşük; sunumda tek cümleyle geçilir |
| A6 DungeonGraph rename + ölü script silme | DEFER demo-sonrası — jüri değeri sıfır, risk sıfır değil |
| `_IsoGame` cluster söküm | DEFER (mevcut OVERLAP_CLEANUP kararı geçerli) |
| Shop gerçek art · boss anim polish · 10-sınıf · çok-act · derin balance | CUT |
| Juice sistemini `_Arena`'ya genel taşıma | DEFER — sadece A2 boss-shake girer; kamera çatışırsa o da düşer |

## ÖZET KRİTİK YOL
**A1→A2→A3 → G1 playtest → fix → G2 build → G3 freeze → prova.** Lane B/C tamamen paralel; hiçbir anim gecikmesi kritik yolu bekletmez. Anim yetişmezse demo idle ile teslim EDİLEBİLİR (capstone sistem-notlu) — bu plan o yüzden güvenli.
