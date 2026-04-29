# Animator Controller Kurulum Rehberi
*Tüm class'lar için aynı mantık. Warblade örnek, diğerleri aynı şema.*

---

## Parametre Listesi (Unity Animator → Parameters sekmesi)

| İsim | Tip | Açıklama |
|---|---|---|
| `Speed` | Float | 0 = idle, 1 = yürüme/koşma |
| `DirX` | Float | Yatay yön (-1 / 0 / +1) |
| `DirY` | Float | Dikey yön (-1 / 0 / +1) |
| `IsDashing` | Bool | Dash animasyonu |
| `IsDead` | Trigger | Ölüm (geri dönmez) |
| `ComboStep` | Int | 0/1/2 = normal combo · 3/4/5 = skill sonrası chained combo |
| `Attack` | Trigger | Her basic attack vuruşunda tetiklenir |

---

## State Machine Şeması

```
          ┌────────────┐
          │    IDLE    │ ←───────────────────────────────┐
          └─────┬──────┘                                 │
         Speed>0│                               Exit Time (otomatik)
          ┌─────▼──────┐                                 │
          │    WALK    │                                 │
          └─────┬──────┘                                 │
         Speed>0│                                        │
          ┌─────▼──────┐                                 │
          │    RUN     │                                 │
          └────────────┘                                 │
                                                         │
          ── NORMAL COMBO ──────────────────────────────│
          AnyState ──(Attack, ComboStep=0)──▶ BasicAttack1
          AnyState ──(Attack, ComboStep=1)──▶ BasicAttack2
          AnyState ──(Attack, ComboStep=2)──▶ BasicAttack3

          ── CHAINED COMBO (skill sonrası) ─────────────│
          AnyState ──(Attack, ComboStep=3)──▶ BasicAttack1_Chained
          AnyState ──(Attack, ComboStep=4)──▶ BasicAttack2_Chained
          AnyState ──(Attack, ComboStep=5)──▶ BasicAttack3_Chained

          BasicAttack* ──[Exit Time]──▶ IDLE/WALK (otomatik)

          AnyState ──(IsDashing=true)──▶ DASH
          DASH     ──(IsDashing=false)─▶ IDLE
          AnyState ──(IsDead trigger)──▶ DEATH (no exit)
```

---

## Chained Animasyon Mantığı

Son 2.5 saniye içinde bir **skill** kullanıldıysa, bir sonraki basic attack:
- `ComboStep` = **3/4/5** olarak gelir (normal 0/1/2 yerine)  
- **%20 bonus hasar** uygular (SkillFlowTracker.chainedDamageBonus)
- Farklı animasyon → oyuncuya "bu bağlantılı vuruş" geri bildirimi

**Animasyon önerisi (class başına):**
| Class | Chained Hit 1 | Chained Hit 2 | Chained Hit 3 |
|---|---|---|---|
| Warblade | Aynı anda rage burst VFX'li slash | Daha geniş yay süpürmesi | Zemin çatlatan overhead |
| Elementalist | Farklı element projectile rengi | Çift atış (burst) | Büyük yavaş top |
| Shadowblade | Görünmez → bıçak beliren backstab | Shadow copy ile eş zamanlı | Spin finisher |
| Ranger | Çift ok (yan yana) | Geri adım atarak çekiş | Power shot (ağır şarj duruşu) |

**Not:** Eğer Kiro ayrı bir chained klip üretmek istemiyorsa, BasicAttack1_Chained → BasicAttack1 ile **aynı sprite clip** kullanılabilir. Yalnızca %20 hasar bonusu ve VFX farkı yeter — animasyon üretimi zorunlu değil.

---

## Geçiş Ayarları

### AnyState → BasicAttack1/2/3
- **Has Exit Time:** false
- **Transition Duration:** 0
- **Conditions:**
  - `Attack` (trigger)
  - `ComboStep` Equals 0  ← (1 ya da 2 diğerleri için)

### BasicAttack1/2/3 → Idle/Walk
- **Has Exit Time:** true
- **Exit Time:** 1.0 (animasyonun tamamı oynar)
- **Transition Duration:** 0.05
- **Conditions:** (yok — sadece exit time)

### AnyState → Dash
- **Has Exit Time:** false
- **Conditions:** `IsDashing` Equals true

### Dash → Idle
- **Has Exit Time:** false
- **Conditions:** `IsDashing` Equals false

---

## Per-Class Animasyon Listesi

### Warblade — BasicAttack[1/2/3]
| State | Sprite Animasyonu | Açıklama |
|---|---|---|
| BasicAttack1 | horizontal-slash (custom) | Yatay greatsword süpürmesi |
| BasicAttack2 | diagonal-upswing (custom) | Çapraz yukarı vuruş |
| BasicAttack3 | overhead-slam (cross-punch template) | Overhead ağır iniş |

### Elementalist — BasicAttack[1/2/3]
*Elementalist için 3 farklı body animasyonu YOK — tek cast body + farklı projectile.*
*Animator'da BasicAttack1/2/3 → hepsi aynı "fireball" cast animasyonuna gider.*
*Farklılaşma: ProjectilePrefab swap (fire → ice → wind), PlayerAnimator değil SkillController halleder.*

| State | Sprite Animasyonu |
|---|---|
| BasicAttack1 | fireball (cast) |
| BasicAttack2 | fireball (cast) — aynı body, farklı element VFX |
| BasicAttack3 | fireball (cast) — aynı body |

### Shadowblade — BasicAttack[1/2/3]
| State | Sprite Animasyonu | Açıklama |
|---|---|---|
| BasicAttack1 | lead-jab | Hızlı öne bıçak |
| BasicAttack2 | cross-punch | Çapraz geniş vuruş |
| BasicAttack3 | surprise-uppercut | Uppercut — combo finisher |

### Ranger — BasicAttack[1/2/3]
| State | Sprite Animasyonu | Açıklama |
|---|---|---|
| BasicAttack1 | bow draw & release (custom) | Normal hız çekiş |
| BasicAttack2 | bow draw & release (custom) | Biraz daha hızlı |
| BasicAttack3 | bow draw & release (custom) | Güçlü çekiş — kısa şarj duruşu |

*Ranger için custom animasyon confirm-cost onayı gerekiyor → KIRO_ANIMATION_BATCH1.md'ye bak.*

---

## Blend Tree (Yön) — Idle / Walk / Run

Her lokomotion state için 2D Blend Tree:
- Blend Type: 2D Simple Directional
- Parameters: DirX, DirY
- Motion list: 8 yön sprite'ları (N, NE, E, SE, S, SW, W, NW)

---

## Önemli Notlar

- **ComboStep ayarlanır → Attack trigger gelir.** Sıra önemli. Her ikisi aynı `HandleComboStep()` çağrısında set edilir, Unity bir frame içinde işler — sorun yok.
- BasicAttack state'leri **Can Transition To Self = false** olacak (aynı state tekrar başlamasın).
- Dash → attack geçişi KIRO_ANIMATION_BATCH1.md'de `dash_attack` olarak ayrı tanımlandı. Bu Animator'da `DashAttack` ayrı state olacak (ilerideki iş).
