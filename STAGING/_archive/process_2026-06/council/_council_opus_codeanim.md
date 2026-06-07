# COUNCIL — OPUS (independent) — Code-Only Animation Stratejisi (2026-06-05)

> Lens: game-design judgment + KOD ENVANTERI DOGRULAMA. Bu rapor kod-doğrulamalı — kıyas tabanı
> brief'ten DAHA İLERİ çıktı (aşağıda). Pozisyonum NET; sentezi orchestrator yapar.

---

## 0. KOD DOĞRULAMA — ne GERÇEKTEN var (brief'i düzeltir)

Brief "Knock+HitFlash KODLA çalışıyor + JuiceManager var" diyor. Gerçek durum çok daha zengin —
**yarısı zaten code-anim ve neredeyse tunable.** Doğruladığım dosya:satır:

**Knockback — İKİ paralel impl (çakışma, K3'te birleştirilmeli):**
- `Core/KnockbackReceiver.cs:24` — `ApplyKnockback(dir, force, duration=0.12f)`, coroutine lineer-decay velocity.
  `knockbackResistance` serialized (0-1). **Ana yol** — BasicAttack bunu çağırıyor.
- `Core/KnockbackComponent.cs:32` — eski impl, `recoveryTime`+`knockbackResistance` serialized,
  `ApplyKnockbackFrom(sourcePos, force)`. **Sadece boss kullanıyor** (`PenitentSovereign.cs` 7 çağrı, hard-coded force 6-12f).
- ÇAĞRI noktaları: `BasicAttackBehaviorBase.cs:101-110` (player melee), `MarkPulseBehavior.cs:143`,
  `BossAI_PenitentSovereign.cs:173`, `SkillRuntime.cs:144`.

**Per-skill knockback ZATEN tunable (ScriptableObject):**
- `BasicAttack/BasicAttackProfile.cs:40-41` — `public float[] knockbackForce = {4,5,8}` +
  `knockbackDuration = {0.10,0.12,0.18}`, combo-step başına. Validate'li (`:109-112`).
  → **K3 için altın: per-skill tunable mimari KISMEN HAZIR. Sıfırdan başlamıyoruz.**

**Juice driver katmanı — TAM bir sistem (brief "JuiceManager var" diyor, aslında 8 driver):**
`Combat/Juice/` altında: `HitFlashDriver.cs` (MaterialPropertyBlock `_FlashColor`, self-wire `Health.OnDamageTaken`),
`ScreenShakeDriver.cs`, `CameraPunchController.cs`, `HitPauseDriver.cs` (`Time.timeScale` clamp, restore),
`ImpactFrameDriver.cs`, `DamageNumberDriver.cs`, `BrokenStateVisual.cs`. Ayrıca `Core/HitStop.cs` singleton
(`HitStop.Instance.Freeze(duration)`, unscaled). İki HitFlash (`Player/HitFlash.cs` eski + `Juice/HitFlashDriver.cs` yeni).

**Merkezi toggle ZATEN var:**
- `Combat/Juice/FeelToggleSettings.cs` — static `ShakeEnabled/HitstopEnabled/VignetteEnabled/CameraPunchEnabled`,
  domain-reload reset. → **K3'te "JuiceProfile" buradan büyütülür.**

**KNOCKDOWN'IN EN KRİTİK ALTYAPISI ZATEN MEVCUT — ayrı gölge:**
- `Environment/GroundBlobShadow.cs` — procedural blob shadow, **HER mob + player'da** (`BaseMobBehavior.cs:88`,
  `PlayerController.cs:119`, `EnemyAI.cs:37`, `HollowMite/ShardWalker/TheWound`). `Ensure(transform,size,alpha)`,
  ayrı child SpriteRenderer, "Ground" layer order 120. **Bu, inandırıcı knockdown'un %1 numaralı şartı:**
  gövde havaya kavislenirken gölge yerde kalır. RIMA bunu bedavaya kazanmış durumda.

**Poise/Broken ZATEN var (stagger/knockdown trigger'ı için):**
- `Skills/SkillStateTracker.cs` — `Broken`/`Sundered` state'leri, `OnStateEntered/OnStateExpired` event'leri,
  stack'li. `Combat/Juice/BrokenStateVisual.cs` bunlara crack-overlay + pulse + shard-burst bağlıyor.
  → **Knockdown'ı "Broken'a girince" tetiklemek = yeni mekanik DEĞİL, mevcut event'e abone olmak.**

**SONUÇ:** RIMA'nın code-anim iskeletinin ~%70'i kurulu. Eksik olan tek BÜYÜK parça = "transform-space
flop/arc oynatıcısı" (gövde sprite'ını gölgeden bağımsız Y'de kaldırıp döndüren bir komponent). O da küçük.

---

## K1 — KOD-ONLY HAREKET TIER TABLOSU

Üç tier: **T1 = code-only kesin yeter (üretme)** · **T2 = code-only iyi ama 1 still-frame ÇOK yardım eder**
· **T3 = code yetmez, üretim değer.** Reçeteler RIMA mevcut komponentlerine bağlı.

| Hareket | Code-only? | Reçete (1-3 satır) | Örnek oyun |
|---|---|---|---|
| **Knockback** (baz ✓) | **T1 — bitti** | Mevcut `KnockbackReceiver` velocity-decay. Ekle: 0.9→1.0 yön-squash (genişlik squash, ezme hissi). | Nuclear Throne, Brotato |
| **Knockdown** (yere serilme) | **T1 — yapılır, ana iş** | Aşağıda DETAY reçete. Gövde sprite parabol-Y + rotasyon, gölge yerde (mevcut `GroundBlobShadow`), 1-2 sekme, get-up. | CrossCode (knockdown), Hyper Light Drifter |
| **Stagger / sendeleme** | **T1** | Hit anında gövde 2-3 px geri-zıpla + ±8° micro-rot + hitstop (mevcut `HitStop`). Knockdown'ın mini hali, poise kırılmadan. | Dead Cells, Hades |
| **Ölüm** | **T1** (mob) | Squash-Y→0.2 + alpha-fade + hafif yana-rot + gölge-küçülme; opsiyonel 3-5 "gib" `SpawnCircleVisual` (mevcut). Boss=T3. | Vampire Survivors, Nuclear Throne |
| **Spawn / doğma** | **T1** | Scale 0→1.15→1.0 ease-out-back (pop) + alpha 0→1 + gölge eşzamanlı büyür + toz-puff. | Enter the Gungeon, Brotato |
| **Dash (ghost-trail)** | **T1** | Mevcut sprite'ın N kopyası, azalan alpha, kısa ömür (afterimage pool). RIMA'da `Blink.cs` zaten benzer. | Hyper Light Drifter, Dead Cells |
| **Melee saldırı** | **T1 — kilitli** | `weapon-hand-separate` + `SlashArcVFX` (doğruladım, combo-step radius/width/dur tunable). Gövde lunge-nudge eklenebilir. | Mevcut RIMA lock |
| **Cast / büyü** | **T2** | Code: anticipation back-lean (gövde -3° + 1 frame geri) + cast-flash + VFX. **Ama belirgin "cast-pose" tek still çok okunur.** | Hades (poz var), Moonlighter |
| **Zıplama / lunge** | **T1** | Parabol-Y arc + gölge sabit + squash kalkış/iniş. Knockdown'ın "kontrollü" tersi — aynı arc-motor. | Dead Cells, CrossCode |
| **Uyanma/uyuma (elite intro)** | **T2/T3** | Code: scale-breath (sin) + slow rise + ışık-pulse yeter. **Ama elite/boss "intro" = okunabilirlik anı → 1-2 still değer.** | Hollow Knight (boss intro) |

### KNOCKDOWN — DETAYLI REÇETE (top-down 3/4 perspektif çözümü dahil)

Tek komponent: `KnockdownDriver` (mob + player'a `Ensure`-pattern, mevcut `GroundBlobShadow`/`BrokenStateVisual`
mimarisini kopyala). Coroutine, **unscaled değil** scaled time (hitstop ile uyumlu). Fazlar:

1. **Launch + arc:** Knockback velocity yatay kalır (mevcut receiver). AYRICA gövde child-sprite'ı
   `localPosition.y` parabolüyle kaldır: `y = 4*h*t*(1-t)`, h≈0.4-0.7 unit, süre ~0.35s. **Gölge yerde
   kalır** (GroundBlobShadow ayrı GO, parent'la birlikte yatay kayar ama Y'de kalkmaz) → bu tek başına
   "havadalık" hissinin %80'i.
2. **Rotasyon (perspektif sorununun ÇÖZÜMÜ — kritik):** Top-down 3/4'te bir karakteri 90° döndürmek
   "ayakta yatay" gibi okunur, KÖTÜ. **ÇÖZÜM = sprite'ı 90° YATIRMA, sadece ~25-40° eğ.** Knockdown'da
   gövde "geriye düşüyor" okunur: launch'ta hızla -30°..-40°'ye eğ (devrilme), yerde kalırken o eğikte
   tut + Y-scale'i 0.55-0.7'ye ez (ezilmiş/yassı = "yerde yatıyor" sinyali). Tam-yatay frame YOK.
   3/4 kamerada eğik+yassı gövde = "yere serilmiş" okunur; 90° = "ayakta uzanmış" okunur. Eğik tut.
3. **Sekme (bounce):** 1-2 sekme: ikinci arc h≈ilkin %35'i, süre %60'ı. Her temasta gölge bir-frame
   genişler + mini toz-puff (`SkillRuntime.SpawnCircleVisual`, mevcut). Sekme = "sert düşüş" inandırıcılığı.
4. **Yerde-kalma (downed):** Eğik+yassı pozda `downedDuration` (poise/skill'e göre tunable, 0.4-1.2s).
   Bu sırada gövde hafif sin-breath (yaşıyor sinyali). İsteğe bağlı: yerde alınan hasara `groundDamageMult`.
5. **Get-up:** Geri-rotasyon (-35°→0) + Y-scale 0.6→1.0 ease-out-back, mini-hop (h≈0.1), **get-up i-frame**
   (~0.25s, mevcut bir invuln bayrağına bağla). Toz-puff kalkışta tekrar.

**Toz/gölge ayrımı zaten elimizde** — yeni asset gerekmiyor. Tek yeni kod = arc/rot/scale tween + faz makinesi.

---

## K2 — ÜRETİM KAÇINILMAZ olan ne kalıyor (ne zaman DEĞER çizgisi)

Kural: **code-anim = generic hareket fiziği; üretim = KİMLİK ve OKUNABİLİRLİK anları.** Demo ölçeğinde
"asla üretme" değil ama bunlar tek tek değer:

1. **Boss attack-telegraph poz(lar)ı (DEĞER — yüksek).** Boss'un "şimdi vuracağım" anı oyunun kontratı.
   Code wind-up (lean+flash) zayıf okunur. Her boss için 1-2 telegraph still = en yüksek ROI üretim.
   `EnemyTelegraph.cs` zaten var → still'leri besler.
2. **Cast-pose tek-still (DEĞER — orta, elementalist/ranged sınıflar için).** Saldırı kimliği ranged'de
   melee-arc'tan zayıf. Sınıf başına 1 cast still (8-yön değil — 4 ya da hatta facing-flip'le 3) ucuz, etkili.
3. **Elite/boss "intro/uyanma" still (DEĞER — orta).** Code breath+rise yeter AMA elite'i normal mob'dan
   ayıran ilk-izlenim anı. 1 still + code motion = en iyi karışım.
4. **Mob "death sprite/gib" (DEĞER — DÜŞÜK, opsiyonel).** Code squash-fade demo için TAMAMEN yeter.
   Sadece imza moblar (boss-tier) için corpse-still değer. Sıradan moblar = T1, üretme.
5. **Player hurt/heavy-hit reaksiyonu (İSTEĞE BAĞLI).** Code stagger yeter; player okunabilirliği yüksek
   olduğundan üretim ertelenebilir.

**ÜretMEYİn (code yeter, üretim israfı):** knockback, knockdown, stagger, sıradan-mob ölüm, spawn-pop,
dash-trail, walk/idle (zaten var), zıplama/lunge. Bunların tamamı transform-space.

---

## K3 — TUNABLE MİMARİ (mevcut kod alanlarına bağlı — over-engineering YOK)

Felsefe: **mevcut iki şeyi büyüt, üçüncü-büyük-sistem KURMA.** Halihazırda
`BasicAttackProfile.knockbackForce[]` (per-skill) + `FeelToggleSettings` (global) var. Knockdown bunlara
oturur.

### (A) HitImpulse — vuruşun "ağırlığı" tek struct (per-skill, mevcut Profile'a alan ekle)
ScriptableObject ŞİŞİRME, sadece mevcut `BasicAttackProfile` + skill-data'ya küçük alanlar:
```
[System.Serializable] struct HitImpulse {
    float knockbackForce;     // VAR (taşı)
    float knockbackDuration;  // VAR (taşı)
    float poiseDamage;        // YENİ — knockdown eşiğini bu doldurur
    bool  forceKnockdown;     // YENİ — ağır skill / şarjlı: poise'a bakma, direkt ser
    float hitStopFrames;      // mevcut HitStop'a bağla
}
```
Mevcut `knockbackForce[]`/`knockbackDuration[]` combo-step dizilerini KORU (validate'li); sadece
`poiseDamage`/`forceKnockdown` ekle. Boss'un hard-coded 6-12f değerleri de bu struct'a taşınmalı
(`KnockbackComponent`→`KnockbackReceiver` birleştir, tek yol).

### (B) Poise — mob tarafı (mevcut SkillStateTracker'a binen küçük komponent)
Yeni `Poise` MonoBehaviour (mob'da): `maxPoise` (serialized, per-mob), hit aldıkça `poiseDamage` biriktir,
eşik aşılınca → `KnockdownDriver` tetikle + poise'u kademeli geri-doldur (`poiseRegenDelay`+`rate`).
**Knockdown trigger mantığı (net):**
- `forceKnockdown=true` skill (ağır/şarjlı/finisher) → her zaman ser.
- Aksi halde → `poiseDamage` birikip `maxPoise` aşınca ser (chip-away; küçük moblar düşük poise = kolay serilir,
  elite yüksek poise = nadiren).
- Boss = `maxPoise` çok yüksek / immune; knockdown yerine mevcut `Broken/Sundered` tell'i kullan.

### (C) KnockdownProfile — sadece görsel/zamanlama (global default + per-mob override)
Knockdown'ın arc/rot/bounce/get-up SAYILARI per-skill değil, çoğunlukla per-mob-arketipi:
küçük ScriptableObject `KnockdownProfile` (launchHeight, arcDuration, tiltDegrees, bounceCount,
downedDuration, getUpIFrames). 2-3 tane yeter (Light/Heavy/Boss-immune). Mob bir profile referans verir;
yoksa global default. **Yeni "JuiceProfile" mega-SO'ya GEREK YOK** — `FeelToggleSettings`'e
`KnockdownEnabled` toggle ekle, bu kadar.

### (D) Yerde hasar + i-frame (mekanik karar)
- Yerdeyken alınan hasar: `groundDamageMult` (KnockdownProfile'da, default 1.25 = "downed punish",
  juggle'a izin verir ama abartma). Player için default 1.0 (haksız juggle-death'i önle).
- Get-up i-frame: ~0.25s invuln (mevcut bir invuln bayrağı varsa ona bağla; yoksa Health'e basit flag).
  Player için ZORUNLU (juggle-lock = rage-quit). Mob için opsiyonel.

**Toplam yeni yüzey:** 1 struct genişletme + 2 küçük MonoBehaviour (`Poise`, `KnockdownDriver`) +
1 küçük SO (`KnockdownProfile`) + 1 toggle. İki knockback impl'i 1'e indir. Demo ölçeği için doğru boyut.

---

## K4 — RİSKLER + MİTİGASYON

| Risk | Mitigasyon |
|---|---|
| **64px'te okunabilirlik** — yassı+eğik gövde küçük | Hareket SİLUET değiştirir → okunur. Y-scale ezme (0.6) + ayrı gölge en güçlü sinyal; rotasyona güvenme tek başına. Get-up'ı belirgin tut. |
| **Rotasyonun pixel-grid kırması** | İki seçenek: (a) İVME değil az açı kullan — knockdown tilt ~30-40°, geçici (yerde kalırken o eğikte freeze, sürekli dönmüyor) → grid-shimmer az. (b) Kalıcı eğikte durmasından rahatsız olunursa: az-sayıda step-açı (0/15/30/40) snap. **RotSprite/outline GEREKSİZ over-engineering demo'da.** Geçiş hızlı, göz takılmaz. |
| **90° döndürüp "yatay sprite" kullanma cazibesi** | YAPMA. Top-down 3/4'te 90° = "ayakta uzanmış" okunur. Eğik(~35°)+yassı doğru çözüm (K1 reçete madde 2). |
| **Çok mob aynı anda yerde = görsel kaos** | (a) `downedDuration`'ı kısa tut (0.4-0.8s sıradan mob). (b) Aynı anda knockdown'a sınır/poise-eşik doğal frenler (hepsi aynı anda eşik aşmaz). (c) Sekme/toz'u stagger'la (rastgele faz) → senkron "dalga" kırılır. |
| **"Her şey kod" = ucuzlaşma riski (nerede dur)** | Çizgi = **KİMLİK anları üretim, FİZİK anları kod** (K2). Boss telegraph + cast-pose + elite-intro üretilirse "ucuz" hissi gitmez; bunlar oyuncunun en çok baktığı karelerdir. Generic flop/pop/dash code kalsın — kimse "knockback animasyonu yok" demez, ÇÜNKÜ hareket fiziği inandırıcıysa fark edilmez. |
| **İki knockback impl drift'i** | K3'te birleştir; yoksa boss (hard-coded 6-12f) ile tunable sistem ayrışır, tutarsız ağırlık hissi. |
| **Juggle-lock / haksız ölüm (player)** | Get-up i-frame ZORUNLU + player groundDamageMult=1.0 + forceKnockdown'ı player'a karşı çok seyrek (sadece boss imza saldırıları). |

---

## TL;DR — POZİSYONUM

1. **Knockdown code-only KESİN yapılır ve YAPILMALI.** Ayrı gölge (`GroundBlobShadow`) + poise (`SkillStateTracker`)
   + per-skill knockback (`BasicAttackProfile`) zaten elimizde → iskeletin ~%70'i kurulu. Tek büyük eksik =
   transform-space arc/flop oynatıcı (`KnockdownDriver`), o da küçük.
2. **Top-down 3/4 perspektif çözümü = 90° DÖNDÜRME, eğ (~35°) + Y-ez (0.6) + sekme + yerde-gölge.** Tam-yatay
   frame okunmaz; eğik+yassı "serilmiş" okunur. Bu tek karar perspektif sorununu çözer.
3. **Tier:** knockback/knockdown/stagger/ölüm(mob)/spawn/dash/lunge/melee = **T1 code-only, üretme.**
   cast-pose + elite-intro = **T2 (1 still çok yardım eder).** Boss-telegraph + boss-death + boss-cast = **T3, üret.**
4. **Tunable mimari = mevcut alanları büyüt, mega-SO kurma:** `HitImpulse` struct (Profile'a poiseDamage+forceKnockdown
   ekle) + küçük `Poise` MB + küçük `KnockdownProfile` SO (2-3 arketip) + `FeelToggleSettings`'e bir toggle.
   İKİ knockback impl'i BİRLEŞTİR.
5. **Üretim çizgisi = KİMLİK anları üret, FİZİK anları kodla.** Boss telegraph/cast/intro üretilirse "her şey kod"
   ucuzluğu hissedilmez. Generic hareket fiziği inandırıcıysa kimse eksik anim'i fark etmez.
6. **Risk #1 = juggle-lock:** player get-up i-frame ZORUNLU, groundDamageMult=1.0, forceKnockdown player'a seyrek.
   Risk #2 = grid-shimmer: geçici eğik (sürekli dönmeyen), RotSprite gereksiz.
