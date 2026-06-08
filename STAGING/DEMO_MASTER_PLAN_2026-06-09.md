# RIMA DEMO — MASTER PLAN & STATUS (2026-06-09)

> Amaç: jüriye gösterilecek, baştan sona OYNANABILIR, HATASIZ/STUCK'SIZ bir dikey-slice demo. Bu doc = nerede olduğumuz + akış + oda boyutları + yol haritası + sıfır-hata stratejisi. ChatGPT'ye danışmak için hazırlandı.

---

## 1) NEREDEYİZ (Mevcut Durum — kanıtlı)

### ✅ ÇALIŞAN / KODLU
- **Boot akışı:** MainMenu → CharacterSelect/Chamber (PlayFromStartScene ile Play hep MainMenu'den başlar).
- **Chamber (sınıf-seçim odası)** — bu session ağır cilalandı (pushed):
  - Takip kamerası (sabit zoom), okunur ışık (global+fill), grounded figür/portal/kukla.
  - **Hades-tarzı tek sabit alt prompt** (ScreenSpaceOverlay → floor/cliff üstünde): `[G] WARBLADE — Bürün`, kilitli `RANGER — Kilitli`, portal `[G] RİFT — Gir`, dummy `[G] Dummy — Sınıf Seç`. Uçan isim etiketleri kaldırıldı.
  - **Pedestal mavileri proximity-only** (sadece en-yakın figür yanar), eşit aralık, doğru en-yakın highlight.
  - **Lock politikası merkezi** (`ClassUnlockPolicy`): başlangıç açık = Warblade + Elementalist; gerisi Echo ile (Ronin 150 vb.); `PlayerClassManager` kilitliyi reddeder.
  - **Saldırı profili** chamber spawn'da atanıyor (BasicAttackProfile bug çözüldü → kuklaya vurulabiliyor, SlashArc).
  - **Dummy:** "Dummy" adı, HP sayısı bar içinde, isim+HP mouse-hover'da; ölümsüz (1000 restore).
  - **Echo HUD** sol-üst.
- **Combat temeli:** odalar yükleniyor, encounter + clear + gate→sonraki oda akışı VAR; **oda-tamamlanma garantisi + test** (her template bitirilebilir).
- **Silah sistemi:** Warblade uçtan-uca canlı; 10 sınıf veri modeli; **Elementalist büyük ölçüde kodlu** (controller + 13 skill).

### 🔧 ŞU AN DÜZELTİLİYOR (in-flight)
- **Combat softlock kök-fix** (çalışıyor): "odaya geçince tekrar mobları kesince stuck" → RoomClearSequence ödül/draft beklemeleri timeout'suz + kapı-açma garantisi yok + fallback kapı ilerletmiyor. Çok-odalı doğrulamalı düzeltiliyor.

### ❌ EKSİK (demo için net-new)
- **Shop** (en büyük net-new — HİÇ YOK): fight'lar arası Echo harcama odası.
- **PauseMenu** (ESC) — yok.
- **Boss dövüşü:** intro/framework var ama gerçek dövüş (telegraph + HP + faz) gerek.
- **Zorunlu lineer sıra** (demo run = sabit oda dizisi) + run içinde **2-sınıf kilidi** uygulaması.
- **Silah mount/ikon** (post-demo per audit; demo'da minimal/opsiyonel).

---

## 2) DEMO AKIŞI (Player Journey — dikey slice)

```
MainMenu
  → Chamber (Warblade VEYA Elementalist seç; diğerleri kilitli görünür)
  → RUN BAŞLAR (zorunlu lineer sıra)
     → Combat Oda 1  (mobları kes → ödül draft → kapı aç → geç)
     → Combat Oda 2  (aynı döngü)
     → SHOP Odası     (Echo harca: 2-3 upgrade/item; sonra kapı)
     → Combat Oda 3   (aynı döngü)
     → BOSS Odası     (boss dövüşü → telegraph/HP)
  → Victory / Death → MainMenu
```

**İlkeler:**
- **Zorunlu lineer** (branching YOK — demo netliği): sabit sıra `combat → combat → shop → combat → boss`.
- **2 oynanabilir sınıf** (Warblade + Elementalist); gerisi chamber'da kilitli.
- Her oda: gir → dövüş → clear → ödül → kapı. **Asla stuck olmaz** (kök-fix garantisi).
- Pause her an ESC ile.

---

## 3) ODA BOYUTLARI (concrete — sabit kamera için)

> Mevcut sorun: Combat_Small 8×6 ve Medium 12×8 ÇOK KÜÇÜK; bank random seçiyor → "odalar çok küçük". Kamera `FitCameraToRoom` oda-başına zoom hesaplıyor → "yakınlık sabit değil".

| Oda tipi | Önerilen boyut (tile) | Neden |
|---|---|---|
| Chamber (select) | 28-32 × 20 | mevcut; 10 figür + galeri + portal + kukla |
| Combat | **22 × 15** (std) · 24×18 (teardrop varyant) | oyuncu + 3-6 mob + dodge alanı; 8×6/12×8 BANK'TAN ÇIKAR |
| Shop | 16 × 12 | sakin, dövüşsüz, 2-3 stand |
| Boss | 28 × 18 | boss + telegraph alanı |

**KAMERA = TEK SABİT ZOOM her yerde:** `FitCameraToRoom` (per-room) KALDIRILIR → sabit ortho ~**5.0** + takip kamerası (büyük odada scroll eder). Chamber da aynı hisle hizalanır (şu an 4.2 → 5.0'a yaklaştırılabilir). "Karakterin yakınlığı her yerde aynı" = bu. (Inspector-tunable.)

---

## 4) YOL HARİTASI (fazlı, sıralı)

**Faz A — COMBAT-RUN SAĞLAMLAŞTIRMA (ŞİMDİ, sıfır-stuck şartı):**
1. ✅ (in-flight) Softlock kök-fix + 3-oda üst üste doğrulama.
2. Gate hitbox küçült (1.2×1.0 → ~0.7×0.7).
3. Küçük odaları bank'tan çıkar / 22×15'e standardize et.
4. Sabit kamera zoom (FitCameraToRoom → fixed ortho ~5.0 + follow).
**ACCEPTANCE:** 5+ oda üst üste oyna, 0 stuck, 0 console error, tutarlı zoom.

**Faz B — DEMO İSKELETİ:**
- Zorunlu lineer sıra (sabit oda dizisi: combat·combat·shop·combat·boss).
- Run içinde 2-sınıf kilidi (sadece Warblade+Elementalist).
- PauseMenu (ESC → Devam / Ana Menü).

**Faz C — SHOP (en büyük net-new):**
- Shop odası: 2-3 stand, Echo ile al (upgrade/heal/item). Çıkış kapısı.
- Minimal ama tam-fonksiyonel (al → envanter/etki uygula → devam).

**Faz D — BOSS DÖVÜŞÜ:**
- Boss encounter wire (intro var) + 2-3 telegraph + HP bar + ölüm→Victory.
- Boss = mevcut mob 1.5-2× + rim + telegraph (MASTER_PLAN'da Seçenek A kilitli).

**Faz E — POLISH:**
- Silah mount/ikon (minimal), juice/SFX tutarlılığı, UI dil/HUD, tam-döngü playtest.

---

## 5) HATA/STUCK = SIFIR (mandate)

- **Softlock kök-fix** (Faz A) + her odada lifecycle state doğrulama + **regression testi** (clear→reward→door→advance döngüsü 3× deadlock'suz).
- **Otomatik kapılar:** her kod turundan sonra (a) play'e gir → console 0-error gate, (b) EditMode/PlayMode test suite, (c) çok-odalı playthrough.
- **Her oda bitirilebilir** invariant testi (zaten var) + her odada en az 1 çalışan çıkış garantisi.
- **Smart-test loop:** console-error + object-state + screenshot + kullanıcı playtest (overlay UI screenshot'a girmez → object-state + playtest).

---

## 6) ChatGPT REVIEW — DOĞRULANDI + BENİMSENEN DEĞİŞİKLİKLER (2026-06-09)
ChatGPT repo'yu okudu, review = `STAGING/_inbox/chatgpt_plan_review_2026-06-09/`. Bulguları bizim Explore'la örtüşüyor (canlı yol, FitCameraToRoom, EncounterController iyi). **Çoğu plan ONAYLANDI** (5-oda akış, 22×15/16×12/28×18, sabit ortho 5.0, 2-sınıf kilit, lean shop). BENİMSENEN FARKLAR/EKLER:

1. **🔑 FAZ SIRASI DEĞİŞTİ (ChatGPT haklı):** Placeholder boss/victory **shop'TAN ÖNCE** gelmeli — yoksa demo'nun SONU olmadan tam uçtan-uca test edemezsin. Yeni uygulama sırası:
   `1.✅ Softlock fix → 2. Forced demo sequence → 3. Fixed camera → 4. 2-sınıf kilit → 5. PauseMenu → 6. PLACEHOLDER boss + victory/death → 7. Shop (3 stand) → 8. Gerçek boss telegraph saldırıları → 9. Full playthrough testleri → 10. Görsel polish`
2. **Forced demo sequence = AYRI kod yolu** (generator'a bırakma): `[SerializeField] bool forceDemoSequence=true` + `DemoSequence = {Combat, Combat, Merchant, Combat, Boss}` → demo modunda DungeonGraph.Generate yerine DOĞRUDAN lineer graph (her node tek child). DungeonGraph şu an random/branching (`DungeonGraph.cs:85` Merchant'ı random fork'ta üretiyor) → demo'ya bırakılamaz.
3. **⚠️ CONTENT GAP:** `DemoRoomBank.merchantRooms` BOŞ (alan var `RoomBankSO.cs:16`, template yok) → **shop oda template'i üretilmeli** (16×12, dövüşsüz, 2-3 stand yeri).
4. **Mob sayısı CAP (22×15 için):** 6+ mob = collider sıkışması/görsel karmaşa. Per-oda: Combat1 = 2-3 kolay · Combat2 = 3-4 (1 ranged) · Combat3 = 4-5 (shop sonrası gücü hissettir).
5. **Kamera:** `[SerializeField] bool useFixedDemoCamera=true; float fixedOrthographicSize=5.0f` (4.7 yakın/dar · 5.0 default · 5.3 güvenli/küçük-hiss).
6. **SHOP SPEC (lean):** 3 stand — Heal 20 Echo (+%30 maxHP) · Damage 35 Echo (+%12 dmg, run-boyu) · Cooldown 35 Echo (-%10 CD). Ekonomi: combat clear = 20 Echo; shop'a girerken ~40 Echo → bir güçlü upgrade VEYA heal VEYA sakla. Inventory/relic/meta'ya BAĞLAMA.
7. **BOSS SPEC:** scale 1.5-2× tek başına yetmez. ŞART: HP bar + telegraph + victory trigger. 3 saldırı — Slam Circle (0.8s telegraph, yakın AoE) · Line Charge (çizgi telegraph, 0.9s charge, kısa stun) · Rift Burst (3 küçük AoE sırayla). Gerçek faz şart değil; HP<%50 → CD %15 hızlanır. Ölünce: input lock → Victory panel → Main Menu → timeScale restore.
8. **TEST PLAN:** EditMode (demo sequence tam 5 node · sıra Combat/Combat/Merchant/Combat/Boss · her node 1 child · boss child'sız · merchant ref var · template'ler non-null · her template ≥1 spawn+1 exit). PlayMode smoke (BeginRun demo → clear→reward→door→advance 5 oda + boss death→Victory). Manual (Warblade run · Elementalist run · boss death · her odada pause). FAIL: console error · NullRef · softlock · stuck door · timeScale-0 kaçak · floor-dışı.

## 7) ZATEN YAPILAN (bu session, pushed)
- ✅ Softlock kök-fix (reward/draft timeout + garantili exit) — 76/76 test, 3-oda doğrulama (`7489e2de`)
- ✅ Gate hitbox 1.2×1.0 → 0.8×0.7 (`HEAD`)
- ✅ Chamber tam cila (Hades prompt, proximity pedestal, lock policy, attack-profile, Dummy, HUD overlay)
