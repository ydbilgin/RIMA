# KIRO — ANİMASYON BATCH 1
*Bu dosyayı oku, sırayla uygula, başka dosya okuma.*
*5 karakter, 35 animasyon görevi. Her animasyonu bitir, get_character ile kontrol et, kaydet, sonrakine geç.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## KARAKTER ID'LERİ

| Karakter | character_id | Canvas |
|---|---|---|
| Warblade | `f3465121-2282-4faa-a955-60b29fd2a628` | 148×148 |
| Elementalist | `480b2223-2c10-4120-94a4-2956795a38e6` | 148×148 |
| Shadowblade | `e47a5347-8800-40e2-a741-c4db69b6c64a` | 148×148 |
| Ranger | `989529d2-6d75-48d8-8fbe-27df303869bd` | 148×148 |
| ShardWalker | `15b8d12a-07bf-42e6-a9ec-89bbce32a908` | 112×112 |

---

## ANİMASYON MANTIĞI — ÖNCE OKU

### Template vs Custom

**Template** (`template_animation_id` verilir):
- 1 gen/yön → 8 yön = **8 gen** toplam
- Hızlı, ucuz, onay gerekmez

**Custom** (`action_description` verilir, template verilmez):
- ~30-40 gen/yön → 8 yön = **~280 gen** toplam
- **ZORUNLU:** `confirm_cost=false` ile önce çağır → maliyeti gör → DONE.txt'e yaz → DUR
- `confirm_cost=true` ASLA kullanıcı onayı olmadan kullanma

### İş Akışı (her animasyon için)
1. `animate_character(...)` çağır
2. 3-5 dakika bekle
3. `get_character(character_id)` ile durumu kontrol et — "completed" görene kadar bekle
4. ZIP URL'den indir, klasöre kaydet
5. Sonraki animasyona geç

---

## ANİMASYON PLANI — TÜMÜ

| # | Karakter | Animasyon | Input | Mod | Gen |
|---|---|---|---|---|---|
| 1.1 | Warblade | idle | — | Template: `fight-stance-idle-8-frames` | 8 |
| 1.2 | Warblade | walk | — | Template: `walking-8-frames` | 8 |
| 1.3 | Warblade | run | — | Template: `running-6-frames` | 8 |
| 1.4 | Warblade | heavy_attack | Sağ Tık | Template: `surprise-uppercut` | 8 |
| 1.5 | Warblade | skill_cast | Q/E/R/F | Template: `cross-punch` | 8 |
| 1.6 | Warblade | hurt | — | Template: `taking-punch` | 8 |
| 1.7 | Warblade | death | — | Template: `falling-back-death` | 8 |
| 1.8 | Warblade | dash_attack | Dash + Sol Tık | Template: `surprise-uppercut` | 8 |
| 1.9 | Warblade | dash | Spacebar | Template: `running-slide` | 8 |
| 1.10 | Warblade | basic_attack | Sol Tık | **CUSTOM greatsword horizontal slash** | ~280 |
| 2.1 | Elementalist | idle | — | Template: `breathing-idle` | 8 |
| 2.2 | Elementalist | walk | — | Template: `walking-6-frames` | 8 |
| 2.3 | Elementalist | run | — | Template: `running-6-frames` | 8 |
| 2.4 | Elementalist | cast | Sol Tık + Sağ Tık + Q/E/R/F | Template: `fireball` | 8 |
| 2.5 | Elementalist | dash | Spacebar | Blink — karakter anim yok, VFX only | 0 |
| 2.6 | Elementalist | hurt | — | Template: `taking-punch` | 8 |
| 2.7 | Elementalist | death | — | Template: `falling-back-death` | 8 |
| 3.1 | Shadowblade | idle | — | Template: `fight-stance-idle-8-frames` | 8 |
| 3.2 | Shadowblade | walk | — | Template: `crouched-walking` | 8 |
| 3.3 | Shadowblade | run | — | Template: `running-6-frames` | 8 |
| 3.4 | Shadowblade | basic_attack | Sol Tık | Template: `lead-jab` | 8 |
| 3.5 | Shadowblade | heavy_attack | Sağ Tık | Template: `surprise-uppercut` | 8 |
| 3.6 | Shadowblade | dash_attack | Dash + Sol Tık | Template: `cross-punch` | 8 |
| 3.7 | Shadowblade | dash | Spacebar | Template: `running-slide` | 8 |
| 3.8 | Shadowblade | hurt | — | Template: `taking-punch` | 8 |
| 3.9 | Shadowblade | death | — | Template: `falling-back-death` | 8 |
| 4.1 | Ranger | idle | — | Template: `breathing-idle` | 8 |
| 4.2 | Ranger | walk | — | Template: `walking-8-frames` | 8 |
| 4.3 | Ranger | run | — | Template: `running-6-frames` | 8 |
| 4.4 | Ranger | skill_cast | Q/E/R/F | Template: `throw-object` | 8 |
| 4.5 | Ranger | dash | Spacebar | Template: `jumping-2` | 8 |
| 4.6 | Ranger | hurt | — | Template: `taking-punch` | 8 |
| 4.7 | Ranger | death | — | Template: `falling-back-death` | 8 |
| 4.8 | Ranger | basic_attack | Sol Tık | **CUSTOM bow draw & release** | ~280 |
| 5.1 | ShardWalker | idle | — | Template: `fight-stance-idle-8-frames` | 8 |
| 5.2 | ShardWalker | walk | — | Template: `walking-8-frames` | 8 |
| 5.3 | ShardWalker | attack | — | Template: `throw-object` | 8 |
| 5.4 | ShardWalker | hurt | — | Template: `taking-punch` | 8 |
| 5.5 | ShardWalker | death | — | Template: `falling-back-death` | 8 |

**Template toplam: 288 gen | Custom toplam: ~560–640 gen | GENEL: ~848–928 gen**

---

## BÖLÜM 1 — WARBLADE

**character_id:** `f3465121-2282-4faa-a955-60b29fd2a628`

### 1.1 Idle
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="fight-stance-idle-8-frames",
  animation_name="idle"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/idle/`

### 1.2 Walk
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="walking-8-frames",
  animation_name="walk"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/walk/`

### 1.3 Run
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="running-6-frames",
  animation_name="run"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/run/`

### 1.4 Heavy Attack — Sağ Tık (Rage tüketir)
`surprise-uppercut` → güçlü yukarı kılıç hamlesi.
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="surprise-uppercut",
  animation_name="heavy_attack"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/heavy_attack/`

### 1.5 Skill Cast — Q/E/R/F
`cross-punch` → yatay silah aktivasyonu, tüm Warblade skill'leri için.
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="cross-punch",
  animation_name="skill_cast"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/skill_cast/`

### 1.6 Hurt
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="taking-punch",
  animation_name="hurt"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/hurt/`

### 1.7 Death
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="falling-back-death",
  animation_name="death"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/death/`

### 1.8 Dash Attack — Dash + Sol Tık Combo
`surprise-uppercut` → slide momentumunu taşıyan yukarı kılıç yayı. heavy_attack ile aynı template ama farklı bağlamda tetiklenir (Unity state machine'de ayrı state).
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="surprise-uppercut",
  animation_name="dash_attack"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/dash_attack/`

### 1.9 Dash — Spacebar (Saldırgan Omuz Çarpışması)
`running-slide` → öne doğru süzülme, omuz düşük. Yolda düşmana çarparsa hasar verir.
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  template_animation_id="running-slide",
  animation_name="dash"
)
```
Kaydet: `STAGING/Characters/Players/Warblade/animations/dash/`

### 1.10 Basic Attack — Sol Tık ⚠️ CUSTOM — MALİYET KONTROLÜ
Önce `confirm_cost=false` ile çağır. Maliyeti DONE.txt'e yaz. DUR. Onay gelmeden devam etme.
```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  action_description="powerful horizontal two-handed greatsword sweep, starting from right shoulder, blade sweeping left across body in a wide arc, full arm extension, weight shift driving the swing, blue-purple rift energy crackling along blade edge during the motion, decisive combat strike, 6-8 frames",
  animation_name="basic_attack",
  directions=["south","south-east","east","north-east","north","north-west","west","south-west"],
  confirm_cost=false
)
```
→ DONE.txt'e yaz: `[COST-CHECK] Warblade/basic_attack | X gen | ONAY BEKLENİYOR`
Onay gelince `confirm_cost=true` ile tekrarla.
Kaydet: `STAGING/Characters/Players/Warblade/animations/basic_attack/`

---

## BÖLÜM 2 — ELEMENTALİST

**character_id:** `480b2223-2c10-4120-94a4-2956795a38e6`

> Sol Tık, Sağ Tık ve Q/E/R/F aynı `cast` animasyonunu kullanır. Ateş/buz/şimşek/rüzgar farkı projectile sprite ile belirlenir. Sağ tık için Unity aynı clip'i daha yavaş oynatır.

### 2.1 Idle
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="breathing-idle",
  animation_name="idle"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/idle/`

### 2.2 Walk
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="walking-6-frames",
  animation_name="walk"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/walk/`

### 2.3 Run
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="running-6-frames",
  animation_name="run"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/run/`

### 2.4 Cast — Sol Tık + Sağ Tık + Q/E/R/F
`fireball` → staff ile kol uzatma ve enerji bırakma hareketi. Tüm element saldırıları bu animasyonla tetiklenir.
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="fireball",
  animation_name="cast"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/cast/`

### 2.5 Dash — Spacebar (Blink)
Karakter animasyonu üretilmez. Blink anlık ışınlamadır.
Unity akışı: karakter gizle → `blink_out.png` spawn → 0.1s → `blink_in.png` spawn → karakter göster.
VFX sprite'ları KIRO_PROJECTILE_BATCH1.md Görev 10'da üretiliyor.

### 2.6 Hurt
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="taking-punch",
  animation_name="hurt"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/hurt/`

### 2.7 Death
```
animate_character(
  character_id="480b2223-2c10-4120-94a4-2956795a38e6",
  template_animation_id="falling-back-death",
  animation_name="death"
)
```
Kaydet: `STAGING/Characters/Players/Elementalist/animations/death/`

---

## BÖLÜM 3 — SHADOWBLADE

**character_id:** `e47a5347-8800-40e2-a741-c4db69b6c64a`

> Q/E/R/F skill aktivasyonları için Unity'de `basic_attack` clip'i yeniden kullanılır. Ayrı skill_cast üretilmez.

### 3.1 Idle
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="fight-stance-idle-8-frames",
  animation_name="idle"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/idle/`

### 3.2 Walk — Stealth Yürüyüşü
`crouched-walking` → çömelmiş sessiz yürüyüş, suikastçı hissi.
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="crouched-walking",
  animation_name="walk"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/walk/`

### 3.3 Run
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="running-6-frames",
  animation_name="run"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/run/`

### 3.4 Basic Attack — Sol Tık
`lead-jab` → hançerli el ile hızlı ileriye hamle, dagger thrust görünümü.
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="lead-jab",
  animation_name="basic_attack"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/basic_attack/`

### 3.5 Heavy Attack — Sağ Tık (Energy/Combo Point tüketir)
`surprise-uppercut` → güçlü yukarı shadow slash. basic_attack'ten görsel olarak farklı.
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="surprise-uppercut",
  animation_name="heavy_attack"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/heavy_attack/`

### 3.6 Dash Attack — Dash + Sol Tık Combo
`cross-punch` → dash inişinde çapraz çift hançer saplama, makas hareketi. Üç saldırı tipinden görsel olarak en farklı olanı.
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="cross-punch",
  animation_name="dash_attack"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/dash_attack/`

### 3.7 Dash — Spacebar (ShadowStep)
`running-slide` → gölge izi bırakarak hızlı öne süzülme. Afterimage Unity'de material fade ile yapılır.
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="running-slide",
  animation_name="dash"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/dash/`

### 3.8 Hurt
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="taking-punch",
  animation_name="hurt"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/hurt/`

### 3.9 Death
```
animate_character(
  character_id="e47a5347-8800-40e2-a741-c4db69b6c64a",
  template_animation_id="falling-back-death",
  animation_name="death"
)
```
Kaydet: `STAGING/Characters/Players/Shadowblade/animations/death/`

---

## BÖLÜM 4 — RANGER

**character_id:** `989529d2-6d75-48d8-8fbe-27df303869bd`

> Sağ Tık (Aimed Shot) için Unity `basic_attack` animasyonunu daha yavaş oynatır — ayrı animasyon üretilmez. Dash sırasında Sol Tık basılırsa Unity `basic_attack` clip'ini mid-air state'de çalıştırır.

### 4.1 Idle
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="breathing-idle",
  animation_name="idle"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/idle/`

### 4.2 Walk
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="walking-8-frames",
  animation_name="walk"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/walk/`

### 4.3 Run
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="running-6-frames",
  animation_name="run"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/run/`

### 4.4 Skill Cast — Q/E/R/F (Tuzak + Özel Ok)
`throw-object` → öne fırlatma hareketi — tuzak bırakma ve özel ok atma için.
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="throw-object",
  animation_name="skill_cast"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/skill_cast/`

### 4.5 Dash — Spacebar (Disengage — Geriye Zıpla)
`jumping-2` → geriye doğru yükselen zıplama. Ok ateşi Unity'de animasyonun ilk frame'inde tetiklenir.
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="jumping-2",
  animation_name="dash"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/dash/`

### 4.6 Hurt
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="taking-punch",
  animation_name="hurt"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/hurt/`

### 4.7 Death
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="falling-back-death",
  animation_name="death"
)
```
Kaydet: `STAGING/Characters/Players/Ranger/animations/death/`

### 4.8 Basic Attack — Sol Tık ⚠️ CUSTOM — MALİYET KONTROLÜ
Önce `confirm_cost=false` ile çağır. Maliyeti DONE.txt'e yaz. DUR. Onay gelmeden devam etme.
```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  action_description="archer drawing bow, left arm fully extended holding bow steady pointing forward, right hand pulling bowstring back smoothly to cheek level, brief aim hold for one frame, then smooth controlled release with right arm extending forward, bow arm absorbs light recoil, clean fluid archery motion, 6-8 frames",
  animation_name="basic_attack",
  directions=["south","south-east","east","north-east","north","north-west","west","south-west"],
  confirm_cost=false
)
```
→ DONE.txt'e yaz: `[COST-CHECK] Ranger/basic_attack | X gen | ONAY BEKLENİYOR`
Onay gelince `confirm_cost=true` ile tekrarla.
Kaydet: `STAGING/Characters/Players/Ranger/animations/basic_attack/`

---

## BÖLÜM 5 — SHARDWALKER (Düşman)

**character_id:** `15b8d12a-07bf-42e6-a9ec-89bbce32a908`

### 5.1 Idle
```
animate_character(
  character_id="15b8d12a-07bf-42e6-a9ec-89bbce32a908",
  template_animation_id="fight-stance-idle-8-frames",
  animation_name="idle"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/animations/idle/`

### 5.2 Walk
```
animate_character(
  character_id="15b8d12a-07bf-42e6-a9ec-89bbce32a908",
  template_animation_id="walking-8-frames",
  animation_name="walk"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/animations/walk/`

### 5.3 Attack — Shard Fırlatma
`throw-object` → fırlatma hareketi, shard throw için birebir uyum.
```
animate_character(
  character_id="15b8d12a-07bf-42e6-a9ec-89bbce32a908",
  template_animation_id="throw-object",
  animation_name="attack"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/animations/attack/`

### 5.4 Hurt
```
animate_character(
  character_id="15b8d12a-07bf-42e6-a9ec-89bbce32a908",
  template_animation_id="taking-punch",
  animation_name="hurt"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/animations/hurt/`

### 5.5 Death
```
animate_character(
  character_id="15b8d12a-07bf-42e6-a9ec-89bbce32a908",
  template_animation_id="falling-back-death",
  animation_name="death"
)
```
Kaydet: `STAGING/Enemies/Act1/ShardWalker/animations/death/`

---

## KAYIT YAPISI

Her animasyon tamamlanınca `get_character(character_id)` ile ZIP URL al ve ilgili klasöre kaydet:

```
STAGING/
  Characters/Players/
    Warblade/animations/
      idle/  walk/  run/
      basic_attack/   (CUSTOM — onay sonrası)
      heavy_attack/   dash_attack/   dash/
      skill_cast/     hurt/          death/
    Elementalist/animations/
      idle/  walk/  run/  cast/  hurt/  death/
    Shadowblade/animations/
      idle/  walk/  run/
      basic_attack/   heavy_attack/   dash_attack/   dash/
      hurt/           death/
    Ranger/animations/
      idle/  walk/  run/
      basic_attack/   (CUSTOM — onay sonrası)
      skill_cast/   dash/   hurt/   death/
  Enemies/Act1/ShardWalker/animations/
    idle/  walk/  attack/  hurt/  death/
```

Her klasör içinde 8 yön için ayrı dosya: `south.png`, `north.png`, `east.png`, `west.png`, `south-east.png`, `south-west.png`, `north-east.png`, `north-west.png`

---

## İŞLEM SIRASI

**Aşama 1 — Template animasyonlar (onaysız, hızlı, önce bunları bitir):**
ShardWalker (5.1–5.5) → Elementalist (2.1–2.4, 2.6–2.7) → Shadowblade (3.1–3.9) → Ranger (4.1–4.7) → Warblade (1.1–1.9)
Toplam: 288 gen

**Aşama 2 — Custom basic_attack (maliyet onayı gerekli, en sona bırak):**
1. `1.10` Warblade basic_attack → `confirm_cost=false` → DONE.txt → DUR
2. Kullanıcı onayı gelince → `confirm_cost=true` → tamamla
3. `4.8` Ranger basic_attack → `confirm_cost=false` → DONE.txt → DUR
4. Kullanıcı onayı gelince → `confirm_cost=true` → tamamla
Tahmini: ~560–640 gen (API'dan kesin sayı gelince göreceksin)

---

## TAMAMLAMA

Her animasyon bitince `STAGING/DONE.txt`'e ekle:
```
[ANIM-DONE] KarakterAdı/animasyon_adı | job_id | tarih | gen_maliyeti
[COST-CHECK] KarakterAdı/animasyon_adı | X gen | ONAY BEKLENİYOR
```

---

## SONRAKI BATCH (BU DOSYADA YAPMA)

- **Projectile + Status + Boss VFX sprites:** `KIRO_PROJECTILE_BATCH1.md`
- **Diğer düşman animasyonları:** VoidThrall, SeamCrawler, ChainWarden, Penitent, RelicCaster, FractureImp — önce AI scriptleri bitmeli, sonra ayrı batch
