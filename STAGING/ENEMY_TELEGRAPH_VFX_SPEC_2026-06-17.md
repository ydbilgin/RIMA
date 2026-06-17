# ENEMY/BOSS ATTACK TELEGRAPH + VFX — IMPLEMENTATION SPEC

Date: 2026-06-17 · Scope: demo (~2 gun) · Risk profile: dusuk-bozma, tek cx Unity-agent, seri
Constraint: **PixelLab balance=0 → yeni sprite YOK.** Tum telegraph = runtime-cizilen sekil/decal + mevcut VFX.

---

## 0. TL;DR (en kritik karar)

**Telegraph motoru ZATEN VAR ve dogru tasarlanmis** — `RIMA.EnemyTelegraph` (decal-sprite + LineRenderer fallback, static factory). Yeni motor YAZILMAYACAK. Is = **mevcut motoru bos kalan saldirilara BAGLAMAK** (wire) + **2 kucuk genisletme** (delayed-explosion ring, snap-flash). Modular-vs-bespoke karari: **mevcut modulu reuse** (zaten modular ve content-light; yeni abstraction = overcomplication, S.PROJECT_RULES Karpathy-2). Orphan ikinci sistem temizlenir.

---

## 1. DURUM DEGERLENDIRMESI (ne var / ne eksik / ne olmali)

### 1a. Ne VAR (calisan, wire'li)
**Aktif sistem = `Assets/Scripts/Enemies/EnemyTelegraph.cs`** (namespace `RIMA`, static factory):
- `SpawnCircle(center, radius, dur)` · `SpawnLine(start, dir, len, width, dur)` · `SpawnCone(origin, dir, radius, angle, dur)`
- Her biri: ground decal sprite (Decals sorting layer, order 5) + alpha pulse (0→0.6 ilk %70, sonra fade) + hafif buyume (0.88→1.0) + ince LineRenderer fallback. Decal yoksa LineRenderer cizer.
- Decal sprite'lari MEVCUT: `Assets/Resources/Art/Telegraphs/telegraph_{circle_ring,line_beam,cone_fan}.png` (Resources'tan yuklenir, calisir).
- Teardown temiz: `destroyOnComplete=true`, `Destroy(gameObject)` sure dolunca. **No-leak kuralina UYGUN** (null birakmaz, GO oz-yikar).

**Wire'li tuketiciler (telegraph ZATEN cikan saldirilar):**
| Dosya | Saldiri | Telegraph |
|---|---|---|
| `EnemyAI.cs:100` | jenerik mob melee | SpawnCircle (0.35s) |
| `MobAttack_Melee.cs:55` | mob melee | SpawnCircle |
| `MobAttack_PenitentCombo.cs:69` | 3-hit combo | SpawnCircle (sadece 1. vurustan once) |
| `MobAttack_Throw.cs:64/66` | mermi/coklu | SpawnCone / SpawnLine |
| `MobAttack_ChainPull.cs:61` | zincir cek | SpawnLine |
| `MobAttack_Barrier.cs:66` | bariyer | SpawnCircle |
| `ShardWalker.cs:123/125` | shard | SpawnCone / SpawnLine |
| `PenitentSovereign.cs:341` | Chain Whip (P1) | SpawnLine ✅ |
| `PenitentSovereign.cs:395` | Penitent Surge (P1) | SpawnCircle ✅ |

### 1b. Ne EKSIK (kullanici sikayetinin kaynagi)
**Bos screenshot kaniti:** `13_combat_mid_enemies.png` + `23_boss_room_spawned.png` → dusmanlar/boss STATIK sprite, saldiri ani okunmuyor; animasyon yok + bazi saldirilar telegraph'sIZ. Spesifik bosluklar:

**A) Boss Phase-1'de 2 saldiri telegraph'sIZ (sadece sprite color-pulse, yere CIZIM YOK):**
- `Attack_ShackleThrow` (satir 415) — uzaktan mermi, **hicbir yer-uyarisi yok**, sadece `Telegraph()` color-pulse → mermi gelisi habersiz ama tehlikeli (oysa P1 "ogretici/yavas" olmali).
- `Attack_HolyLash` (satir 426) — 180° yay, **yer-uyarisi yok**; oyuncu yayin nereye baktigini goremiyor.

**B) Boss Phase-2/3'un TAMAMI yer-telegraph'sIZ** (en buyuk bosluk — en tehlikeli saldirilar habersiz):
- `Attack_FractureStrike` (451) — 3 hizli vurus, sadece 0.3s color-pulse.
- `Attack_ChainExplosion` (475/491) — **zemin isaretleri 3sn gecikmeli patlama** ama yere HIC marker cizilmiyor (kod yorumu "Place markers" diyor ama gorsel marker yok → oyuncu nereden kacacagini bilmiyor; tam ARPG-AoE konvansiyonu burada KIRIK).
- `Attack_SovereignsWrath` (516) — tum-alan + merkez guvenli bolge; **safe-zone halkasi cizilmiyor** → oyuncu nereye kacacagini bilmiyor.
- `Attack_FractureCharge` (537) — arena-boyu dash hasar cizgisi; **dash hatti onceden cizilmiyor**.

**C) "Snap-to-damage" geri-bildirimi zayif:** telegraph bittiginde hasarin "DUSTU" ani gorsel olarak vurgulanmiyor (impact flash yok). ARPG konvansiyonunda telegraph dolunca kisa beyaz/renk patlamasi olur. Boss'ta sadece ScreenShake var; mob'larda hicbir sey.

**D) Color sinyali tutarsiz:** P1 telegraph color turuncu (`telegraphColor`), P2 mor (`phase2Color`) — ama `EnemyTelegraph` decal'lari sabit kirmizimsi (`1,0.22,0.06`). Boss faz rengi ile yer-telegraph rengi ESLESMIYOR (kucuk polish boslugu, opsiyonel).

### 1c. ORPHAN sistem (temizlenecek)
**`Assets/Scripts/Enemy/Telegraph/`** klasoru ayri, component-based bir telegraph sistemi icerir (`EnemyTelegraph` [namespace `RIMA.Enemy.Telegraph`] + `TelegraphGroundMarker` + `EnemyOutlinePulse`). **Hicbir sey instantiate/attach ETMIYOR** (grep dogruladi: `StartTelegraph` cagrisi yok, sadece kendi-icinde). `CombatEventBus.PublishTelegraph` → ScreenShakeDriver dinliyor ama publisher yok. **Tamamen dead.** Ayrica `RIMA.EnemyTelegraph` ile **ayni tip-ismi** (farkli namespace) → karisiklik kaynagi.
> NOT (Karpathy-3 cerrahi): Bu dead-code SILME zorunlu degil. Onerilen aksiyon = spec'te BILDIR + cx istege gore `[System.Obsolete]` isaretler. `EnemyOutlinePulse` mantigini (dusman govdesinin telegraph aninda parlamasi) ileride reuse degerli olabilir — bkz §3-D. Demo icin sadece NOT, dokunma opsiyonel.

### 1d. Ne OLMALI (hedef)
Animasyon olmasa bile her tehlikeli saldiri **okunur** olmali:
1. **AoE → daire telegraph** (windup boyunca dolan/yanip-sonen halka → sure dolunca snap-damage). ✅ motor var, baglanacak.
2. **Hat/isin → dikdortgen/cizgi telegraph** (onceden). ✅ motor var (SpawnLine), baglanacak.
3. **Mermi → telegraph YOK** (habersiz, hizli, refleks). → bu DOGRU davranis; sadece P1 boss ShackleThrow icin kisa origin-flash (ogretici faz) opsiyonel.
4. **Snap-to-damage** flash (telegraph bitince kisa renk patlamasi). → kucuk genisletme.

---

## 2. SISTEM MIMARISI (modular vs bespoke)

**Karar: REUSE `RIMA.EnemyTelegraph` (mevcut). Yeni motor/abstraction YAZMA.**
Gerekce (PROJECT_RULES Karpathy-2 + memory `project_modular_design_philosophy`): sistem zaten 3 shape + windup + danger-color + fill-anim + teardown sagliyor; content-light (1 boss + ~5 mob); yeni `AttackTelegraph` component'i = mevcut static-factory ile cakisan ikinci abstraction = overcomplication. Boss zaten static factory'yi cagiriyor → tutarli kal.

**Iki kucuk, cerrahi genisletme (mevcut dosyaya, yeni dosya yok):**

### Genisletme-1 — `SpawnDelayedRing` (gecikmeli-patlama halkasi)
`EnemyTelegraph.cs`'e yeni static + ShowDelayedCircle overload. Davranis: halka cizilir, `delay` boyunca SABIT yuksek-alpha durur (yanip-sonmez — "buraya patlayacak" net sinyal), son ~0.25s hizli yanip-sonup snap. ChainExplosion icin. (Mevcut `ShowCircle` pulse-tabanli; delayed-explosion icin sabit-durus daha okunur.)
- Imza: `public static EnemyTelegraph SpawnDelayedRing(Vector2 center, float radius, float delay)`
- Icte: yeni alanlar `bool holdMode; float holdFraction=0.8f` → Update'te `t < holdFraction` iken alpha sabit `decalPeakAlpha`, sonra hizli flash-fade.

### Genisletme-2 — `FlashImpact` (snap-to-damage geri-bildirim)
`EnemyTelegraph.cs`'e static helper: telegraph bitince cagirilir, kisa (0.12s) parlak renk burst.
- **REUSE `SkillVfx.PlayBurst` veya `SkillVfx.ImpactBurst`** (mevcut VFX motoru) — yeni cizim yazma. Imza: `EnemyTelegraph.FlashImpact(Vector2 pos, VfxElement element=Physical)` → icte `SkillVfx.ImpactBurst(pos, element)`. (SkillVfx zaten HitSpark/DeathBurst prefab fallback'li, additive, oz-yikici.)

**Projectile path = AYRI, mevcut, dokunma:** mermiler `MobAttack_Throw`/`SpawnChainProjectile`/`Projectile`/`EnemyProjectileDamage` ile zaten calisiyor. Telegraph konvansiyonu geregi mermi HABERSIZ kalir; sadece SpawnLine/SpawnCone ile fırlatma-yonu uyarisi (zaten var). **Yeni projectile kodu YOK.**

---

## 3. 3 TELEGRAPH TIPI — SOMUT IMPLEMENTASYON

### A) Daire (AoE) — `SpawnCircle` / yeni `SpawnDelayedRing`
- **Cizim:** ground decal `telegraph_circle_ring.png` (var), Decals layer order 5; scale = radius*2; LineRenderer ring fallback.
- **Windup timing:** standart AoE `SpawnCircle(pos, r, dur)` → alpha 0→0.6 (ilk %70), fade (son %30). Gecikmeli patlama → `SpawnDelayedRing(pos, r, delay)` → sabit-durus + son flash.
- **Damage-snap:** caller windup sonunda `OverlapCircleAll` + `EnemyTelegraph.FlashImpact(pos)`.
- **Pooling:** YOK (gerek yok; demo-olcek, GO oz-yikici, GC-safe). Spekulatif pool = Karpathy-2 ihlali.
- **Teardown:** `destroyOnComplete` → GO yok olur; null birakmaz.

### B) Cizgi/dikdortgen (hat/isin) — `SpawnLine`
- **Cizim:** `telegraph_line_beam.png` midpoint'e, dir aci, scaleX=length scaleY=width*1.4. Fallback ince rect outline.
- **Timing/snap/teardown:** A ile ayni. Dash (FractureCharge) ve HolyLash icin.

### C) Mermi (projectile) — telegraph YOK (konvansiyon)
- Telegraph cizme. Mevcut firlatma yonu uyarisi (SpawnLine/Cone) zaten "geliyor" sinyali; mermi-uctusu habersiz/hizli kalir. P1 boss ShackleThrow icin OPSIYONEL: origin'de 0.2s kucuk `SkillVfx.CastFlash` (sadece ogretici faz okunurlugu; demo-kritik degil → S).

### D) (OPSIYONEL, post-demo) Dusman govde-flash
`EnemyOutlinePulse` mantigini (telegraph aninda dusman sprite'i kirmiziya pulse) ileride `EnemyTelegraph` ile birlikte tetikle → animasyonsuz dusmanin "saldiriyorum" okunurlugunu artirir. Demo icin ATLA (boss zaten `Telegraph()` color-pulse yapiyor; mob'lar yapmiyor ama decal yeterli).

---

## 4. WIRE PLANI (hangi saldiri hangi telegraph — dokunulacak dosyalar)

### Dosya 1 — `Assets/Scripts/Enemies/EnemyTelegraph.cs` (motor genisletme)
- + `SpawnDelayedRing` static + `ShowDelayedCircle` + holdMode alanlari (Genisletme-1). **S/M.**
- + `FlashImpact` static (Genisletme-2, SkillVfx.ImpactBurst sarmalar). **S.**
- Risk: DUSUK (yeni metod ekleme, mevcut imzalar degismez).

### Dosya 2 — `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` (ASIL is — bos saldirilari bagla)
Telegraph color-pulse ZATEN var; eksik = YER-telegraphlari. Her `Attack_*` icinde, mevcut `Telegraph(dur)` coroutine'i ile ESZAMANLI yer-cizimi ekle (color-pulse'u koru — ikisi birlikte en okunur):
| Saldiri | Eklenecek (windup baslangicinda) | Snap (windup sonu) |
|---|---|---|
| `Attack_HolyLash` (426) | `EnemyTelegraph.SpawnCone(pos, forwardDir, holyLashRadius, 180f, dur)` | `FlashImpact(pos)` |
| `Attack_ShackleThrow` (415) | (ops) `SkillVfx.CastFlash(gameObject, Physical)` | — |
| `Attack_FractureStrike` (451) | `SpawnCircle(pos, meleeStopRange+0.4f, 0.3f)` | `FlashImpact` her vurusta |
| `Attack_ChainExplosion` (475/491) | her marker icin `SpawnDelayedRing(offset, chainExplosionRadius, delay)` | patlama aninda `FlashImpact(worldPos)` |
| `Attack_SovereignsWrath` (516) | `SpawnCircle(pos, wrathOuterRadius, dur)` outer + safe-zone icin **2. SpawnCircle(pos, wrathSafeRadius, dur)** (ic guvenli halka, ayri renk istenirse) | `FlashImpact(pos)` (buyuk) |
| `Attack_FractureCharge` (537) | windup'ta `SpawnLine(startPos, dir, 16f, meleeStopRange*2, dur)` (dash hatti) | dash sonu `FlashImpact(endPos)` |

> KRITIK: ChainExplosion = ARPG-AoE konvansiyonunun kalbi. Su an marker GORSEL YOK → en yuksek-etki fix. `SpawnDelayedRing` + windup `chainExplosionDelay`(3s, P3'te 0.85x) ile birebir esle (cizim suresi = gercek patlama gecikmesi).

### Dosya 3 — `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs` (kucuk polish)
- Su an sadece 1. vurustan once SpawnCircle. 3-hit combo'da 3. (en sert, 40dmg) vurustan once de bir telegraph at → S. Opsiyonel.

### Dosya 4 (OPSIYONEL) — orphan isaretle
- `Assets/Scripts/Enemy/Telegraph/EnemyTelegraph.cs` → `[System.Obsolete("Dead — live telegraph = Enemies/EnemyTelegraph.cs")]` (silme; Karpathy-3). **S, risk yok.**

**Mob'lar (Melee/Throw/ChainPull/Barrier/ShardWalker) ZATEN telegraph'li → DOKUNMA.** Sadece istege bagli `FlashImpact` snap eklenebilir (dusuk oncelik, M toplu).

---

## 5. ASSET IHTIYACI

**YENI GEN YOK (balance=0).** Tum ihtiyac karsilanmis:
- Telegraph decal'lari MEVCUT: `Resources/Art/Telegraphs/telegraph_{circle_ring,line_beam,cone_fan}.png` ✅
- Snap-flash: `SkillVfx.ImpactBurst` → mevcut `Prefabs/VFX/HitSpark`/`DeathBurst` (motor zaten fallback'li) ✅
- Drain-log decal'lari (bones/blood/void-crack, `rima_decal_v1`) = ENVIRONMENT prop, telegraph-sekilli DEGIL → bu is icin GEREKSIZ.
- `SpawnDelayedRing` ayni `circle_ring.png`'i kullanir (sadece alpha-davranis farki, yeni sprite yok).

---

## 6. EFFORT / RISK + cx-EXECUTION SIRASI

| # | Is | Effort | Bozar mi | Oncelik |
|---|---|---|---|---|
| 1 | `EnemyTelegraph.SpawnDelayedRing` + `FlashImpact` (Dosya 1) | M | Hayir (ek metod) | **1 (blocker)** |
| 2 | Boss `Attack_ChainExplosion` → SpawnDelayedRing (Dosya 2) | S | Dusuk | **2 (en yuksek etki)** |
| 3 | Boss `SovereignsWrath` safe-zone + outer ring (Dosya 2) | S | Dusuk | 3 |
| 4 | Boss `HolyLash` cone + `FractureCharge` line + `FractureStrike` circle (Dosya 2) | M | Dusuk | 4 |
| 5 | Tum boss saldirilarina `FlashImpact` snap (Dosya 2) | S | Dusuk | 5 |
| 6 | PenitentCombo 3. vurus telegraph (Dosya 3) | S | Dusuk | 6 (ops) |
| 7 | Orphan `[Obsolete]` isaretle (Dosya 4) | S | Yok | 7 (ops) |
| 8 | ShackleThrow CastFlash (Dosya 2) | S | Dusuk | 8 (ops) |

**cx sirasi:** 1 → 2 → 3 → 4 → 5 tek dispatch'te (hepsi Dosya1+Dosya2, ardisik). 6-8 ayri/atlanabilir. Toplam ~M-L, 1 cx oturum yeter.

**cx brief zorunlu satirlari:**
- `ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.`
- `UNITY ERROR CHECK: is bitince read_console (Error+Warning); kendi hatani COZ, ilgisiz/onceden-var hatayi BILDIR (silme), raporda console durumunu yaz.`
- No-leak: yeni GO/coroutine'lar `destroyOnComplete`/`Destroy` ile oz-yikici olmali; null/aktif birakma yok (mevcut `EnemyTelegraph` deseni izle).
- Telegraph SURESI = gercek windup suresi ile BIREBIR esle (ozellikle ChainExplosion delay 3s ↔ ring delay 3s; P3 0.85x carpani dahil), yoksa "yalan telegraph" olur.

---

## 7. DOKUNULACAK DOSYA LISTESI (ozet)
1. `Assets/Scripts/Enemies/EnemyTelegraph.cs` — +SpawnDelayedRing, +FlashImpact (zorunlu)
2. `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs` — 6 saldiriya yer-telegraph + snap (ASIL is)
3. `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs` — 3. vurus telegraph (ops)
4. `Assets/Scripts/Enemy/Telegraph/EnemyTelegraph.cs` — `[Obsolete]` (ops, silme)
Mob attack dosyalari (Melee/Throw/ChainPull/Barrier/ShardWalker/EnemyAI) = zaten telegraph'li, DOKUNMA.
