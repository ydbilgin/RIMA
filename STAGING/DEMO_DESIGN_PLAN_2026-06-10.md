# DEMO TASARIM PLANI — 2026-06-10 (rima-design / Opus)

> **Statü:** LIVE karar dokümanı. Execute-agent'lar bu plandaki kararları DOĞRUDAN uygular.
> **Kapsam:** Sadece demo. "Demo-sonrası" işaretli her şey freeze'e KADAR yapılmaz.
> **Zemin (koddan doğrulandı):** F5 boş-loadout + açılış draft'ı ZATEN kodlu (`RoomRunDirector.cs:192-199`, `Warblade_SkillController.cs:68-75`). SkillBarUI 6-hex + drag-drop iskeleti var (`SkillBarUI.cs:31-55`). Ödül `Entities / sortingOrder=0` SABİT spawn ediliyor (`RoomRunDirector.cs:1295-1296`) — y-sort yok, "floor altında" bug'ının kaynağı bu. HUDController'da HP bar / Echo / oda-etiketi / etkileşim-prompt'u ZATEN yazılı (`HUDController.cs:418-744`) ama _Arena'da ekrana gelmiyor. Juice sürücüleri tam: `Assets/Scripts/Combat/Juice/` (HitPause, ScreenShake, HitFlash, CameraPunch, ImpactFrame, DamageNumber). VFX sprite envanteri: `Assets/Resources/VFX/Fireball/` (8 yön) + `Assets/Resources/VFX/Skills/` (slash_arc ×2, glacial_spike ×2, frozen_orb ×2, meteor ×2).

---

## 1) SKILL SİSTEMİ UX

### K1.1 — Slot sayısı ve tuş eşlemesi (LOCK)
**KARAR:** Skill bar = **8 görsel slot**: `LMB | RMB || Q | E | R | F || Z | X`.
- LMB = temel saldırı, RMB = sınıf-ikincil (mevcut `SlotActions` korunur).
- Q/E/R/F = 4 aktif skill slotu (run içinde doldurulur).
- Z/X = ikincil-sınıf slotları; **boss'a kadar GİZLİ**, `UnlockSecondarySlots()` (zaten kodlu, `Warblade_SkillController.cs:44-50`) tetiklenince bar'a fade-in ile eklenir.
**GEREKÇE:** Controller zaten 6-slot (4 primary + Z/X). Bar'ın mevcut 6'lık dizisi LMB/RMB/QERF — Z/X eksik. 2 slot eklemek, "kit dolu + yeni kart = replace UI" ihtiyacını TAMAMEN ortadan kaldırır (aşağıda K1.4): boss-sonrası kartlar Z/X'e akar.
**KESİM:** Replace/upgrade akışı demo'da YOK. Z/X rebind demo'da YOK (sabit).

### K1.2 — Başlangıç skill'i: sınıf başına 1 skill, AÇILIŞ DRAFT'ıyla (LOCK — F5 ile uyumlu)
**KARAR:** Run boş loadout'la başlar; ilk oda kurulur kurulmaz 3-kart **açılış draft'ı** açılır, seçim **Q slotuna** girer (F5 zaten böyle). Açılış draft içeriği SABİT (rastgele değil):
- **Warblade:** `GravityCleave` (ana vuruş) · `IronCharge` (mobilite) · `Earthsplitter` (AoE). — `SunderMark` açılışta SUNULMAZ (işaret tek başına okunmaz; detonasyon sinerjisi run ortasında anlamlı).
- **Elementalist:** `Fireball` (ana nuker) · `GlacialSpike` (kontrol) · `ChainLightning` (AoE). — `Blink` açılışta SUNULMAZ (hasarsız tek skill = ilk oda silahsız hissi).
**GEREKÇE:** "1 skill ile başla" kararının en okunur hali = oyuncuya İLK seçimi yaptırmak (Hades keepsake-anı hissi) ama 3 seçeneğin üçü de "tek başına oda temizleyebilen" skill olmalı.

### K1.3 — Ödülle slot doldurma sırası (LOCK)
**KARAR:** Oda-temizleme draft'ında seçilen skill **ilk BOŞ primary slota** girer: Q → E → R → F. Demo dizisinde (Combat·Combat·Merchant·Combat·Boss·Combat) matematik tam oturur: açılış=Q, oda1-clear=E, oda2-clear=R, oda4-clear=F → boss'a 4/4 dolu kit ile girilir.
**Draft havuzu** = sınıfın kilitli kit'inden henüz alınmamış core skill'ler + draft-extra'lar (WB: SunderMark, DeepWound, DeathBlow + alınmamış core'lar · EL: Blink, FrozenOrb, Meteor + alınmamış core'lar). Aynı skill iki kez SUNULMAZ.

### K1.4 — Slot dolunca ne olur (LOCK)
**KARAR:** Primary 4 slot doluysa tek draft kaynağı boss-sonrası **unlock draft**'ıdır ve kartları **Z**'ye, ikincisi (varsa) **X**'e girer. Replace UI YOK, kart yakma YOK.
**GEREKÇE:** Demo akışında "dolu kit + primary draft" durumu matematiksel olarak oluşmuyor (K1.3); oluşursa (timeout ile draft kaçırma vb.) draft yine ilk boş slota yazar, hiç boş yoksa draft AÇILMAZ ve kapı direkt açılır (savunma dalı, log'lu).

### K1.5 — Sürükle-bırak yeniden sıralama (LOCK)
**KARAR:** Tuş **pozisyona sabittir**, skill taşınır: bar'da basılı-tut + sürükle = iki slotun SKILL'leri yer değiştirir; yeni pozisyonun tuşu geçerli olur (mevcut `SkillBarUI.cs:482` "keybinds follow the visual order" davranışı DOĞRU, korunur).
- Kalıcılık = controller'ın `slots[]` dizisinde swap (UI-only swap YASAK; input zaten slot index'inden okuyor → dizide swap = otomatik kalıcı). Run-ötesi kalıcılık GEREKMEZ (her run boş başlar).
- Sürüklenebilir bölge: Q/E/R/F + (açıksa) Z/X arası serbest. **LMB/RMB sürüklenemez** (temel saldırı/ikincil sınıf kimliği sabit).
- Drag sırasında: kaynak slot %50 alfa, hedef slot cyan glow; geçersiz bırakma = animasyonlu geri dönüş.

### K1.6 — Slot görsel anatomisi (LOCK)
Her slot (mevcut Ashen Glyph hex korunur):
```
┌─ hex bg ───────┐
│   [skill ikon]  │  ← SkillIconRegistry'den; ikon yoksa skill adının ilk 2 harfi (mor fallback YASAK)
│  ◔ cooldown     │  ← saat-yönü radial overlay (%30 karartma) — mevcut
│           [Q]   │  ← sağ-alt tuş etiketi, binding-driven — mevcut
└─────────────────┘
```
- **Boş slot:** soluk hex + ortada "—" + tuş etiketi görünür kalır (oyuncu "burası dolacak" diye okur). Boş slota basınca slot 0.15s kırmızı-titreşim (feedback, ses yok).
- **Hazır-flash:** cooldown bitince 0.18s beyaz parlama — mevcut, korunur.
- **Yeni-skill-girdi animasyonu:** draft seçimi sonrası hedef slot 0.4s scale-pop (1.0→1.25→1.0) + cyan ring pulse. Oyuncunun gözünü "skill nereye gitti"ye çeker — demo'da ZORUNLU (1-skill-başlangıç ekonomisinin okunması buna bağlı).

---

## 2) HUD (DEMO-MİNİMAL)

### K2.1 — Demo HUD envanteri (LOCK)
HUDController'daki parçaların ÇOĞU zaten yazılı; iş = _Arena'da garanti görünür kılmak + şu kesim:

| Eleman | Demo | Not |
|---|---|---|
| HP bar | ✅ sol-üst | mevcut `BuildHpBar`; sınıf-accent dolgu |
| Resource bar | ✅ HP altı ince | mevcut |
| Echo sayacı | ✅ sağ-üst | cyan elmas + sayı — mevcut |
| Oda etiketi | ✅ üst-orta | `roomNameLabel` REUSE — bkz. Bölüm 5 |
| Skill bar | ✅ alt-orta | Bölüm 1 |
| Etkileşim prompt'u | ✅ alt-orta, skill bar üstü | mevcut `BuildInteractionPrompt` |
| Kontrol ipucu | ✅ sağ-alt | mevcut, ilk odada 8s sonra fade |
| Low-HP vignette | ✅ | mevcut |
| Minimap | ❌ demo-dışı | kapalı kalır (önceki karar) |
| Boss HP bar | ✅ sadece boss odası | mevcut `BossHealthBar` (order 90) |

### K2.2 — "HUD ekranda yok" kök-fix yönü (EXECUTE TALİMATI)
**KARAR:** HUD'un varlığı tek otoriteye bağlanır: `_Arena` boot'unda (RoomRunDirector.BeginRun ÖNCESİ) `HUDController.Instance` ensure edilir — yoksa runtime'da kur (`HUDController_Auto`), ScreenSpaceOverlay, sortingOrder=50 (gameplay üstü, ClassSelectionUI 190 / DemoComplete 200 / Pause 1090 ALTINDA). SkillBarUI aynı canvas'a parent'lanır. Teşhis sırası: (1) HUDController _Arena'da hiç kurulmuyor mu? (2) kuruluyor ama canvas disabled/alfa-0 mı? (3) kuruluyor ama başka overlay altında mı? — Çözüm hangisiyse, "ensure-on-boot + tek canvas" hedef mimarisine getir.

### K2.3 — Para göstergesi: DEMO = TEK PARA, ADI ECHO (KARAR — kanon çekincesiyle)
**KARAR:** Demo'da ekranda TEK para sayacı olur ve adı **Echo** kalır; shop da bunu harcar (mevcut davranış). Gold/Echo ayrımı (NLM: Echo=meta-currency) **demo-sonrası** işidir.
**GEREKÇE:** Jüri çift-para ekonomisini bilmez; tek sayaç + tek harcama noktası = sıfır kafa karışıklığı. Rename/ayrıştırma şu an dokunulan dosyalara (shop, HUD, wallet) yayılır = freeze öncesi risk.
**⚠️ KANON ÇAKIŞMASI:** NLM kanonu "Echo = kalıcı meta" der; bu karar demo süresince bilinçli ihlaldir. Kullanıcının "Echo→Gold kararı" backlog'da KALIR (CURRENT_STATUS'ta zaten kayıtlı), demo bunu beklemez.

### K2.4 — Yerleşim taslağı (ASCII)
```
┌──────────────────────────────────────────────────────────────┐
│ ❤ HP ▓▓▓▓▓▓▓░░  72/100        ODA 3/6 — SAVAŞ      ◆ 240    │
│ ⚡ ▓▓▓▓░░ (resource)                                 (Echo)  │
│                                                              │
│                                                              │
│                     (oyun alanı)                             │
│                                                              │
│                                  [BOSS HP — sadece boss]     │
│                  [G] Ödülü Al   (etkileşim prompt'u)         │
│      ⬡LMB ⬡RMB │ ⬡Q ⬡E ⬡R ⬡F │ ⬡Z ⬡X        WASD/SPACE…   │
│      (skill bar — Z/X boss sonrası)        (kontrol ipucu)   │
└──────────────────────────────────────────────────────────────┘
```

---

## 3) HADES-TARZI ÖDÜL ANI

### K3.1 — İki ayrı bug, iki ayrı fix (EXECUTE TALİMATI — karıştırma)
1. **Sorting bug'ı (kesin, koddan doğrulandı):** `SpawnRewardPickup` ödülü `Entities / sortingOrder=0` SABİT basıyor (`RoomRunDirector.cs:1295-1296`). Oda yüksek-Y bölgesinde y-sort'lu prop/cliff'ler ödülün üstüne çiziliyor → "floor altında" görüntüsü.
   **FIX KURALI (LOCK):** RewardPickup spawn'ına **`IsoSorter` ekle** (`Assets/Scripts/Core/IsoSorter.cs` — `order = baseOrder − y×100`), `baseOrder=+5` (aynı hücredeki dekordan önde okusun). Sabit-order yaklaşımı YASAK (32000 gibi değer her şeyin önüne geçer, oyuncunun önünden bile — yanlış). Proje kuralı: **dinamik/yerde-duran her sprite IsoSorter/YSortBehaviour taşır, sabit order taşımaz.**
2. **İlk-odada-hiç-spawn-yok (teşhis gerekli):** Spawn yalnız `RoomClearSequence:1192`'de. Execute-agent teşhis ağacı:
   - Console'da `"[RoomRunDirector] RewardPickup spawned at …"` (`:1303`) ilk odada VAR mı?
   - **VARSA** → bug #1'in (sorting) veya erişilemez-hücre fallback'inin türevi; IsoSorter fix'i + spawn-pozisyon log'unu kontrol et.
   - **YOKSA** → `RoomClearSequence` ilk odada hiç başlamıyor ya da `SpawnRewardPickup` null dönüp `:1193-1199` sessiz kapı-açma dalına düşüyor. Şüpheli: açılış draft'ı (F5) aktifken oda temizlenirse clear-sequence'in draft-bekleme/lifecycle etkileşimi. `lifecycle.State` + `IsDraftPending/Active` değerlerini clear anında logla, kökü bul. **Sessiz dal `:1193`'e uyarı log'u ekle** (her ihtimalde).

### K3.2 — Ödül anı koreografisi (LOCK)
Mevcut `ClearSlowMoBlip` korunur, üstüne şu zincir (toplam yeni iş = küçük):
1. **t=0 (son düşman ölür):** slow-mo blip (mevcut) + üst-orta oda etiketinde **"ODA TEMİZLENDİ"** 1.2s flash (`SetRoomStatus` REUSE, ekstra UI yok).
2. **t≈0.5s:** ödül spawn — **scale-pop** (0→1.15→1.0, 0.25s) + **EchoPuffBurst** cyan patlama (`Assets/Scripts/CrossClass/EchoPuffBurst.cs` REUSE) + hafif shake-S.
3. **Bekleme hali:** idle bob (`PlaceholderFloat.cs` REUSE veya 3 satır sinüs) + varsa `Light2D` cyan pulse. Ödül "canlı" okunmalı; statik sprite YASAK.
4. **Yaklaşınca:** mevcut `[G] Ödülü Al` prompt'u (RewardPickup'ta zaten var) + HUD prompt'u — korunur.
5. **[G]:** ödül gizlenir → 3-kart draft (mevcut akış) → seçilen skill slot-pop animasyonu (K1.6) → kapı açılır.
- **Adet/konum:** TEK ödül, oda merkezine en yakın 3×3 clearance'lı walkable hücre (BUG-3 fix'i zaten kodlu, korunur).
- 12s auto-collect timeout korunur AMA timeout'a düşerse console'a uyarı + ekrana kısa "Ödül otomatik alındı" toast'u ZORUNLU değil — log yeter (demo'da timeout'a düşmek istisna).

---

## 4) VFX-TABANLI SKILL "ANİMASYON" (SPRITE-FRAME YOK)

### K4.1 — Standart impact reçetesi (LOCK — her skill bunu kullanır)
| Şiddet | Hitstop (`HitPauseDriver`) | Shake (`ScreenShakeDriver`) | Flash |
|---|---|---|---|
| S | 40 ms | hafif | hedefte beyaz HitFlash 80 ms |
| M | 70 ms | orta | hedefte HitFlash + CameraPunch |
| L | 110 ms | güçlü | HitFlash + `ImpactFrameDriver` tek-kare |
Tüm sürücüler `Assets/Scripts/Combat/Juice/`'ta MEVCUT — yeni sistem yazılmaz, sadece skill'lerden ÇAĞRILIR. `_Arena`'da juice bootstrap'ının kurulduğu doğrulanır (eski not: juice sadece test sahnesindeydi).

### K4.2 — Oyuncu kod-tween sözlüğü (LOCK — 3 ilkel, hepsi sprite'sız)
- **Lunge:** gövde hedefe 0.3–0.6 birim, 0.08s ileri + 0.12s geri (ease-out).
- **Squash & stretch:** localScale (1.15, 0.85) → (1.0, 1.0) 0.15s — vuruş anı.
- **Recoil:** cast sonrası 0.15 birim geri itme + 0.1s toparlanma.
Afterimage (3 kopya sprite, alfa 0.5→0, 0.25s) yalnız dash-tipi skill'lerde.

### K4.3 — Skill-bazlı okuma tablosu (LOCK)
**Warblade** (kit: IronCharge / GravityCleave / SunderMark / Earthsplitter + draft DeepWound / DeathBlow):
| Skill | Telegraph | VFX (kaynak) | Impact | Tween |
|---|---|---|---|---|
| IronCharge | yok (anlık dash) | **afterimage ×3** (kod) | S, çarptığı her mobda | lunge-uzun + squash |
| GravityCleave | 0.1s geri-salınım (tween) | `slash_arc_main` büyük yay | **M** | lunge + squash |
| SunderMark | yok | `slash_arc_crescent` ince yay + hedef tepesinde **skill-ikonu mark** (registry sprite, 0.5 alfa pulse — YENİ ASSET YOK) | S; mark patlarsa M | lunge-kısa |
| Earthsplitter | **0.4s zemin çizgisi** (boss `SpawnLine` telegraph sistemi REUSE) | çizgi boyunca cyan-beyaz flaş (kod-renk) | **L** | squash güçlü + recoil |
| DeepWound (pasif) | — | bleed tick'te kırmızı DamageNumber (mevcut sürücü, renk parametresi) | — | — |
| DeathBlow | — | execute anında `ImpactFrameDriver` (mevcut, `ExecutePromptDriver` zaten bağlı) | **L** | — |

**Elementalist** (kit: Fireball / GlacialSpike / ChainLightning / Blink + draft FrozenOrb / Meteor):
| Skill | Telegraph | VFX (kaynak) | Impact | Tween |
|---|---|---|---|---|
| Fireball | yok | **8-yön sprite CANLI** (`VFX/Fireball/`) | M + impact'te turuncu EchoPuffBurst varyantı (renk parametresi) | recoil |
| GlacialSpike | yok | `glacial_spike_spear` projectile; impact'te `glacial_spike_cluster` | S + mobda **cyan tint 1s** (slow okuma — `sr.color`) | recoil |
| ChainLightning | yok | **LineRenderer zigzag** 2–3 segment, 0.12s flaş, kod-only (YENİ SPRITE GEREKMEZ) | S × her sıçrama | recoil-kısa |
| Blink | yok | origin + varış noktasında **EchoPuffBurst** cyan (REUSE) + 1 frame renderer-off | yok | yok |
| FrozenOrb | yok | `frozen_orb_main` kod-rotate (z-spin) ilerleyen orb; periyodik küçük `glacial_spike_cluster` saçılım | S tick | recoil |
| Meteor | **0.6s zemin dairesi** (boss `SpawnCircle` REUSE) | `meteor_comet` yukarıdan pozisyon+scale lerp düşüş → `meteor_main` impact | **L** | recoil |

**Cross-class "Burning Rage":** proc anında oyuncuda 0.2s turuncu HitFlash — başka iş YOK.

### K4.4 — "Yeterli" çıtası (LOCK — over-scope freni)
Bir skill demo-yeterli sayılır ⇔ (a) çıkış anı 1 bakışta okunur, (b) impact hissedilir (K4.1 reçetesi), (c) **sıfır yeni sprite üretimi** gerektirir. Tablodaki TÜM ihtiyaçlar mevcut sprite + kod-tween + LineRenderer + renk-parametresiyle kapanıyor. Partikül sistemi kurma, shader yazma, trail-renderer zinciri = YASAK (demo-sonrası). PixelLab'dan yeni VFX İSTENMEZ.

---

## 5) ODA KİMLİĞİ

### K5.1 — İki katman, ikisi de HUD-içi (LOCK)
1. **Kalıcı etiket** (üst-orta, mevcut `roomNameLabel` REUSE): `ODA {n}/6 — {TİP}`. Tipler: SAVAŞ · TÜCCAR · BOSS · SON ODA (post-boss). `AdvanceTo`/`BuildCurrentRoom` içinden `SetRoomStatus` çağrısı — node index + RoomType zaten RoomRunDirector'da.
2. **Oda-giriş banner'ı:** odaya girişte aynı etiketin 1.5s büyük hali (font ×2, fade-in 0.2s / bekle 0.9s / fade-out 0.4s) ekran üst-üçte-birinde. Ayrı sistem YOK — roomNameLabel'ın scale/alfa tween'i yeter.
**GEREKÇE:** En ucuz çözüm; mevcut alan REUSE; jüri "neredeyim"i hem anlık (banner) hem sürekli (etiket) okur.
**KESİM:** Kapı-üstü hedef-oda ikonu (Hades usulü) = **demo-sonrası** (demo dizisi lineer, kapı çoğunlukla tek → bilgi değeri düşük, iş orta). Chamber bu sisteme dahil değil (run-dışı).

---

## 6) ÖNCELİK + SIRALAMA

### Şu an çalışan execute-agent'ın alanı (ÇAKIŞMA YASAK — bu plan onun işini KİLİTLER, yeniden açmaz)
SkillBarUI / DraftManager / RoomRunDirector / PlayerClassManager / CameraFollow / SkillController'lar üzerindeki mevcut koşu: skill-bar görünürlüğü, HUD, ödül, drag-drop. **Bu plandaki K1.1–K1.6, K2.2, K3.1 o koşunun kabul kriteridir.** Yeni bir agent bu dosyalara, mevcut koşu commit'leyip bitmeden DOKUNMAZ.

### P0 — DEMO OLMAZSA-OLMAZ (otonom-execute)
| # | İş | Karar ref | Dosya alanı |
|---|---|---|---|
| P0.1 | HUD _Arena'da garanti görünür (HP+skill bar+Echo+prompt) | K2.1–K2.2 | HUDController, _Arena bootstrap |
| P0.2 | Ödül sorting fix (IsoSorter) + ilk-oda teşhis ağacı | K3.1 | RoomRunDirector, RewardPickup |
| P0.3 | Açılış draft içerik kilidi (sabit 3'lü, sınıf başına) + ilk-boş-slot doldurma | K1.2–K1.3 | DraftManager, SkillOfferGenerator |
| P0.4 | Drag-drop swap'ın controller `slots[]`'a yazdığının doğrulanması | K1.5 | SkillBarUI, SkillController'lar |
| P0.5 | Boss-sonrası kartlar → Z/X + bar'a Z/X slot ekleme (unlock'ta görünür) | K1.1, K1.4 | SkillBarUI, DraftManager |

### P1 — DEMO HİSSİ (otonom-execute, P0 sonrası)
| # | İş | Karar ref |
|---|---|---|
| P1.1 | Ödül koreografisi: TEMİZLENDİ flash + spawn pop + EchoPuffBurst + bob | K3.2 |
| P1.2 | Oda etiketi + giriş banner'ı | K5.1 |
| P1.3 | Standart impact reçetesinin TÜM kit skill'lerine bağlanması + _Arena juice bootstrap doğrulama | K4.1 |
| P1.4 | Yeni-skill slot-pop + boş-slot görünümü + boş-slota-basış feedback'i | K1.6 |

### P2 — SKILL-ÖZEL OKUMA (otonom-execute, P1 sonrası, freeze'e sığdığı kadar — tablo sırasıyla)
1. Earthsplitter çizgi-telegraph + Meteor daire-telegraph (boss telegraph REUSE — en yüksek "vay" / maliyet oranı)
2. ChainLightning LineRenderer + Blink çift-puff
3. IronCharge afterimage + FrozenOrb rotate/saçılım
4. SunderMark hedef-mark + GlacialSpike slow-tint
5. DeepWound kırmızı tick + Burning Rage turuncu flash

### KULLANICI-PRESENT (bu planın DIŞINDA, mevcut backlog'da kalır)
- PixelLab P1–P4 + B1 (karakter/boss sprite animasyonları) — `PIXELLAB_PRODUCTION_SHEET_2026-06-10.md`. **Bu plan hiçbir işi buna bağlamaz** (K4.4 gereği).
- G1 gerçek-el playtest, Echo→Gold nihai kararı, G3 freeze.

### DEMO-SONRASI (yapma)
Replace/upgrade draft UI · Z/X rebind · kapı-üstü oda-tipi ikonları · Gold/Echo ayrıştırma · partikül/shader VFX · minimap · 10-sınıf kit genişletme.

---

## ÇAKIŞMA BEYANI
- **K2.3 (tek para = Echo)** NLM kanonuyla (Echo = meta-currency) bilinçli, demo-süreli çelişir — yukarıda açıkça işaretli; nihai karar kullanıcı-gated backlog'da.
- Diğer tüm kararlar kilitli kurallarla (skill-kit lock `fdcd1eb6`, VFX batch `7d71a3ec`, F5 boş-loadout, CONTROL_SCHEME LMB/RMB/QERF, demo lineer dizi) UYUMLU; hiçbirini sessizce değiştirmez.
