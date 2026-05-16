# Silahsız Karakter Animasyon Spec'i (V1)
**Tarih:** 2026-05-16 S86 SPRINT10 (sonrası)
**Karar otoritesi:** Bu doküman — sonradan Karar #144 olarak MASTER_KARAR_BELGESI'ne eklenecek
**Override eden eski kararlar:** Karar #71 (silah hep elde) + Karar #73 (silahlı 1-piece) — bu iki karar **REVOKED**.

---

## §0 — TEMEL FELSEFE

**Body = silahsız, weapon = ayrı child SpriteRenderer.**

- Karakter sprite'ı sadece body + clothing göstertir, asla silah.
- Silah Unity'de child SR olarak parent karaktere bağlanır.
- Body animasyonları "saf kol hareketi" (silahsız), weapon child kendi animasyonunu oynar.
- Senkronizasyon: Animator events + ScriptableObject pose presetleri.

**Avantaj:** Aynı body sprite, farklı silahla farklı görünüm (Warblade kılıç/balta swap edebilir). Silah upgrade görselleri ucuz.
**Maliyet:** Unity Animator authoring artar; weapon-arm sync hassasiyet ister.

---

## §1 — DIRECTION + ANIMATION LOCK

### Sprite production (PixelLab — şu an üretiyoruz)
**8 yön sprite base** üretilir (5 produce + 3 PixelLab mirror).
- Üretilen: S, SE, E, NE, N
- Mirror: SW, W, NW

### Animation production (V1 = 4-cardinal yeterli)
**4 cardinal yön animasyon** ile başla. 8-dir animasyon V2 polish.
- Üretilen anim: S, E, N
- Unity flip: W = E flipX (per Karar #53 simetrik class; silahsız body simetrik)

8 yön sprite kalsa da animasyonu 4-dir yapmak demek: SE/NE/SW/NW yönleri **statik pose** (animasyon yok). Hareket sırasında diagonal yönlerde S→E veya S→W threshold mapping kullanılır (45° hysteresis, Karar #53).

**Upgrade path:** V2'de aynı sprite'lardan 8-dir anim üretilir (yeni call yok; PixelLab Animate same source).

---

## §2 — ANIMATION LİSTESİ + FRAME COUNT

| Anim | Frame | FPS | Süre (ms) | Loop |
|---|---|---|---|---|
| **Idle** | 4 | 4-6 | ~700-1000 | YES (loop) |
| **Run** | 6 | 10-12 | ~500-600 | YES |
| **Attack basic (3-seg)** | 3 (windup/strike/recovery) | impact=40ms, others=80-100ms | ~220 | NO (one-shot) |
| **Dash** | 3 | 12-15 | ~200-250 | NO |
| **Hit reaction** | 3 | impact=40ms | ~160 | NO |
| **Death** | 6-8 | 8-10 | ~700-800 | NO |
| **Skill 1/2/3 (per-class)** | 3-6 (3-seg) | per-segment | per-skill | NO |

**Per Karar #14:** Multi-segment skill = PEAK frame önce, START→PEAK + PEAK→END interpolate.
**Per Karar #47:** Run = PixelLab Animate (built-in loop). Attack/Skill = KF+Interpolate 3-seg workflow.

**Total per class (V1, 4-cardinal):**
- 4 idle + 6 run + 3 attack + 3 dash + 3 hit + 6-8 death = ~25-27 frames × 3 yön (S/E/N) = **~75-80 frames per class**
- W yönü Unity flipX, ek üretim yok

---

## §3 — HAND ANCHOR CONVENTION (silah-attach için)

### 64×64 sprite koordinat sistemi
Unity standard: (0,0) bottom-left. Y artar yukarı.

### Right hand (primary weapon hand) — canonical idle position

| Yön | x | y | Açıklama |
|---|---|---|---|
| **S** (önden bakış) | 42-44 | 24-26 | Karakterin sağ kalçası, sağ tarafından (kameradan baktığında sol) |
| **SE** | 44-46 | 25-27 | Sağ kalça, hafif öne |
| **E** (sağ profil) | 44-46 | 24-26 | Sağ kalça, profilden tek el görünür |
| **NE** | 42-44 | 26-28 | 3/4 arka, sağ kalça |
| **N** (arkadan) | 22-24 | 24-26 | Karakter arkası bize, sağ el ekranda sol kalçada görünür |

**Mirror yönleri:** W/SW/NW = E/SE/NE x-koordinatı 64'ten çıkar (x_mirror = 64 - x_original).

### Left hand (off-hand) — canonical idle position
Right hand mirror: x'i karakter ekseni etrafında 4-6 pixel çevir. Two-hander silahlar için sol el de bağlanır.

### Anchor metadata depolama
**Per sprite:** Anchor coordinates → `ScriptableObject` `HandAnchorMap` (sınıf başına bir asset).

```
[CreateAssetMenu(menuName = "RIMA/Character/HandAnchorMap")]
public class HandAnchorMap : ScriptableObject
{
    public string className;
    public AnchorEntry[] perDirection;  // 8 entries: S, SE, E, NE, N, NW, W, SW
}

[Serializable]
public class AnchorEntry
{
    public Direction direction;
    public Vector2Int rightHandPixel;   // (x, y) in sprite local
    public Vector2Int leftHandPixel;
    public Vector2Int weaponPivotOffset; // sprite-relative
}
```

**Üretim akışı:**
1. PixelLab sprite üret (silahsız)
2. Unity'de sprite import sonrası: `RightHand` ve `LeftHand` adlı GameObject child'larını sprite'a (görsel inceleme ile) yerleştir
3. Anchor ScriptableObject'e koordinatları yaz
4. Runtime: WeaponChild SR pozisyonu `HandAnchorMap.GetAnchor(currentDirection).rightHandPixel * PPU` ile set edilir

---

## §4 — WEAPON CHILD SR SYNC STRATEGY

### Hierarchy
```
PlayerRoot (transform, Rigidbody2D, Collider2D, PlayerController)
├── BodySR (SpriteRenderer, Animator: body animations)
└── WeaponSR (SpriteRenderer, Animator: weapon animations)
```

### Sync mechanism

**Idle/Run (passive):**
- WeaponSR pozisyonu, Direction değişince `HandAnchorMap`'ten okunur ve set edilir
- Run sırasında hand-bob istenirse: Animator'da WeaponSR Y için 6 keyframe sine wave (±1-2px)

**Attack/Skill (active):**
- BodyAnimator state "Attack_Basic" → Animation Events tetiklenir:
  - `Frame 0 (Windup)`: `WeaponAnimator.SetTrigger("AttackWindup")` — silah arkaya yatar
  - `Frame 1 (Strike, impact frame)`: `WeaponAnimator.SetTrigger("AttackStrike")` — silah ileri uzanır + hit detection açılır
  - `Frame 2 (Recovery)`: `WeaponAnimator.SetTrigger("AttackRecovery")` — silah geri döner

**WeaponAnimator state machine (per weapon type):**
- Idle (loop, weapon at rest)
- Windup (1 frame, weapon pulled back)
- Strike (1-2 frame, weapon forward — hit window)
- Recovery (1 frame, return to idle)

Bu states **per silah tipi** authored. Same body attack animation 5 farklı silaha (sword/axe/staff/dagger/spear) farklı görsel verir.

---

## §5 — PER-CLASS ATTACK PATTERN (body motion)

Body attack animasyonu sınıfa göre değişir. Silah hangisi olursa olsun, vücut hareketi sabit kalır.

| Sınıf | Body attack motion | Frame count | Hand path | Weapon impact frame |
|---|---|---|---|---|
| **Warblade** | Iki elli yan slash (sağ-üstten sol-alta) | 3 | sağ el omuzdan kalça yanı | Frame 1 (40ms impact) |
| **Ravager** | Üstten dikey slam (her iki el yukarı→aşağı) | 3 | sağ + sol el yukardan aşağı | Frame 1 |
| **Brawler** | Sağ kol ileri jab + **body step-in 4-8px** (Bandit Knight insight) | 3 | sağ el bel→ileri tam uzanış + body translate forward at strike frame | Frame 1 |
| **Ronin** | Iaido draw motion (sağ kalçadan çekiş) | 3 | sağ el kılıf→ileri kavis | Frame 1 |
| **Shadowblade** | Sağ kol ileri thrust (dagger stab) | 3 | sağ el alt→ileri düz | Frame 1 |
| **Ranger** | İki el bow draw + release | 4 | sol el ileri sabit, sağ kalbe→geri çek→bırak | Frame 2 (release) |
| **Gunslinger** | Sağ kol kaldır + recoil | 3 | sağ el kalça→omuz hizası→geri | Frame 1 (shot) |
| **Elementalist** | Sağ kol overhead cast | 3 | sağ el yan→yukarı→yan | Frame 1 (cast point) |
| **Summoner** | İki el ileri uzatma (summon gesture) | 3 | her iki el ileri→açık | Frame 1 |
| **Hexer** | Sağ kol curse gesture (parmaklar açık çekiş) | 3 | sağ el yan→ileri açık→geri | Frame 1 |

### Brawler özel (silahsız sınıf — Karar #71)
Brawler **silah taşımaz** — yumruk attığı için body attack = ana animation. WeaponSR boş (veya glow VFX). Karar #71'in Brawler için olan kısmı **HÂLÂ GEÇERLİ**.

**Bandit Knight insight (research takeaway #3):** 64×64'te yumruk küçük sprite'ta zayıf okunur. Çözüm: **body Frame 1'de 4-8px forward translation** (anti-Bandit-Knight principle — weight over speed). Hitstop 60-80ms + screen shake 40ms + dust kick VFX kombinasyonu Brawler yumruğunu Hades-tier hissettirir. Punch reach'ı silah reach kadar uzun olmaz (kasıtlı) ama impact yoğunluğu büyük.

### Ronin özel (Karar #71 sheath/draw)
Ronin'in **sheath/draw kimliği korunur** — body animasyonu draw motion içerir. WeaponSR idle state'te kınında (gizli), attack state'te elinde.

---

## §6 — ANIMATION TIMING + COMBAT FEEL HOOKS

**Per Karar #64 (ActionCommitProfile 5 alan):**
- `windupMs` — body Frame 0 süresi
- `recoveryMs` — body Frame 2 süresi
- `dashCancelStartFraction` — per-class (Karar #67)
- `hitstopMs` — impact frame'inde global hitstop (40-80ms)
- `cancelOnWhiff` — boolean (miss durumunda recovery skip)

**Combat feel sequence per attack:**
```
T=0    : Body Frame 0 (windup) — WeaponWindup tetik
T=80ms : Body Frame 1 (strike impact) — WeaponStrike tetik + HitDetection açık
         + camera shake 40ms + hitstop 60ms (if hit confirmed)
T=140ms: Body Frame 2 (recovery) — WeaponRecovery tetik
T=220ms: Idle return
```

**Cancel windows (Karar #67):**
- Ravager/Shadowblade: %15-25 (Frame 0 ortası → cancel OK)
- Ranger/Gunslinger: %30-55 (Frame 1 başı → cancel OK)
- Warblade/Brawler/Ronin: %60-75 (Frame 1 sonu → cancel OK)
- Casters (Elem/Hexer/Summoner): windup not cancellable

---

## §7 — DASH ANIMATION (per Karar #58)

3 frame motion, no state, no damage.

| Frame | Body | Weapon |
|---|---|---|
| 0 | başlangıç ileri lean | weapon at idle |
| 1 | full lean (sprint pose) | weapon trails behind body |
| 2 | recovery (geri dönüş) | weapon catch-up |

I-frame window: Frame 0-1 (~150ms).
Skill movement ile overlap YASAK (Karar #58).

---

## §8 — HIT REACTION + DEATH

### Hit reaction (3 frame, 4-dir, Karar #48)
- Frame 0: knockback impact (body bends back)
- Frame 1: peak knockback
- Frame 2: recovery to idle

Yönü: hit source direction'a göre 4-dir seçilir (S/E/N/W). WeaponSR idle pose'a döner.

### Death (6-8 frame, 4-dir)
- Frame 0-2: stagger
- Frame 3-5: collapse
- Frame 6-7: rest pose

WeaponSR death'te yere düşer (separate animation veya despawn).

---

## §9 — V1 ÜRETİM SCOPE (per FAZ_MASTER)

**Faz 1 (şu an, Sprint 10 sonrası):**
- 10 sınıf × 8 yön idle base sprite (silahsız) — şimdi üretiyoruz, 80 sprite
- Warblade tam anim (idle 4 + run 6 + attack 3 + dash 3 + hit 3 + death 8 = 27 frames × 3 cardinal = 81 frames)
- 1 silah (Warblade kılıç) child SR + WeaponAnimator
- HandAnchorMap ScriptableObject infrastructure

**Faz 2:** Elementalist + Shadowblade + Ranger tam anim
**Faz 3:** +Ravager/Ronin/Gunslinger/Brawler
**Faz 4:** +Summoner/Hexer

---

## §10 — QC CHECKLIST (her animation set için)

- [ ] Frame count locked'a uygun (4/6/3/3/3/6-8)
- [ ] Hand position frame 0'da canonical (HandAnchorMap'le sync)
- [ ] Idle: hands stable (Y varyans ≤ 2px)
- [ ] Run: contact frame (0, 3) hands at canonical Y
- [ ] Attack: peak frame impact direction net
- [ ] WeaponSR sync test (Unity'de iki SR aynı anda)
- [ ] 4-cardinal × 3 anim direction (S/E/N) + W flipX OK
- [ ] No anti-aliasing
- [ ] Animator transition Exit Time OFF (Karar memory)

---

## §11 — KARAR #144 ÖNERİSİ (MASTER_KARAR_BELGESI'ne eklenecek)

**Karar #144 — Silahsız karakter + Weapon Child SR (Karar #71 + #73 OVERRIDE):**
"Karakter base sprite **silahsız** üretilir. Silah Unity'de child SpriteRenderer olarak parent karaktere bağlanır. Body animasyonu silahsız kol hareketi gösterir; weapon child SR kendi animasyonunu oynar; senkronizasyon Animation Events + HandAnchorMap ScriptableObject. Karar #71 (silah hep elde single-state) ve Karar #73 (silahlı 1-piece) bu kararla REVOKE. Brawler hala silahsız sınıf (Karar #71 Brawler kısmı korunur). Ronin sheath/draw kimliği korunur (WeaponSR idle = sheath, attack = drawn)."
2026-05-16 S86 SPRINT10.

---

End of weapon-less animation spec.
