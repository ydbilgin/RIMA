# KIRO — ANİMASYON BATCH 2
*Bu dosyayı oku, sırayla uygula. KIRO_ANIMATION_BATCH1.md'ye dokunma.*
*Batch 1 tamamlandıktan sonra bu dosyayı çalıştır.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## KARAKTER ID'LERİ

| Karakter | character_id | Canvas |
|---|---|---|
| Warblade | `f3465121-2282-4faa-a955-60b29fd2a628` | 148×148 |
| Ranger | `989529d2-6d75-48d8-8fbe-27df303869bd` | 148×148 |

*(Elementalist ve Shadowblade için bu batch'te yeni üretim yok — açıklaması aşağıda.)*

---

## NEDEN BATCH 2?

Yeni geliştirilen combo sistemi şunu gerektiriyor:
- Sol tık ilk kez → **basic_attack_1** animasyonu
- Sol tık ikinci kez (1.2sn içinde) → **basic_attack_2** animasyonu
- Sol tık üçüncü kez → **basic_attack_3** animasyonu (combo finisher)
- Skill kullandıktan sonraki basic attack → **chained** varyant (isteğe bağlı)

Animator parametresi: `ComboStep` int (0/1/2 normal · 3/4/5 chained)

---

## CLASS BAŞINA YENİ ÜRETİM DURUMU

| Class | Yeni Üretim | Açıklama |
|---|---|---|
| **Warblade** | ✅ Gerekli | Step 2 ve Step 3 yok |
| **Shadowblade** | ❌ Yok | Üç ayrı animasyon Batch 1'de üretildi → remap yeterli |
| **Elementalist** | ❌ Yok | Tek cast animasyonu yeterli; combo farkı projectile ile yapılır |
| **Ranger** | ✅ Gerekli | Step 2 ve Step 3 yok |

---

## SHADOWBLADE — REMAP NOTU (ÜRETİM YOK)

Batch 1'de üretilen 3 animasyon doğrudan combo state'lerine bağlanır:

| Animator State | Clip (Batch 1'den) | ComboStep |
|---|---|---|
| BasicAttack1 | `basic_attack` (lead-jab) | 0 |
| BasicAttack2 | `dash_attack` (cross-punch) | 1 |
| BasicAttack3 | `heavy_attack` (surprise-uppercut) | 2 |

Unity Animator'da bu üç state'i `Attack` trigger + `ComboStep` koşuluyla bağla.
Clip'lerin kendisi değişmiyor — sadece hangi state'e atandığı değişiyor.

---

## ELEMENTALIST — COMBO NOTU (ÜRETİM YOK)

BasicAttack1/2/3 → hepsi aynı `cast` clip'ini kullanır.
Combo farkı projectile sprite ve rengiyle sağlanır (script tarafında).
Chained versiyon için de aynı clip, sadece VFX overlay farklı.

---

## ANİMASYON PLANI — YENİ ÜRETİMLER

| # | Karakter | Animasyon | ComboStep | Mod | Gen |
|---|---|---|---|---|---|
| 1.1 | Warblade | basic_attack_2 | 1 | Custom | ~280 |
| 1.2 | Warblade | basic_attack_3 | 2 | Custom | ~280 |
| 2.1 | Ranger | basic_attack_2 | 1 | Template: `throw-object` | 8 |
| 2.2 | Ranger | basic_attack_3 | 2 | Custom (güçlü çekiş) | ~280 |

**Toplam tahmini: ~848 gen (2 template + 3 custom)**

---

## BÖLÜM 1 — WARBLADE COMBO

**character_id:** `f3465121-2282-4faa-a955-60b29fd2a628`

Combo akışı:
- Step 0 = `basic_attack` (Batch 1'de üretildi — yatay greatsword sweep)
- Step 1 = `basic_attack_2` ← **bu bölüm**
- Step 2 = `basic_attack_3` ← **bu bölüm**

Her hit görsel olarak birbirinden farklı olmalı. Üçü bir arada akıcı bir combo hissi vermeli.

---

### 1.1 Warblade — basic_attack_2 ⚠️ CUSTOM — MALİYET KONTROLÜ

Step 0 yatay sweep ile başladı → Step 1 diagonalden yukarı çıkış.

Önce `confirm_cost=false` ile çağır. Maliyeti DONE.txt'e yaz. DUR.

```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  action_description="rising diagonal greatsword slash, blade starting low-left, sweeping upward to upper-right in a powerful lifting motion, both hands gripping hilt, weight shifting from back foot to front foot during the upswing, blue-purple rift energy trailing along blade edge, follows naturally after a horizontal sweep, 6-8 frames",
  animation_name="basic_attack_2",
  directions=["south","south-east","east","north-east","north","north-west","west","south-west"],
  confirm_cost=false
)
```
→ DONE.txt'e yaz: `[COST-CHECK] Warblade/basic_attack_2 | X gen | ONAY BEKLENİYOR`
Onay gelince `confirm_cost=true` ile tekrarla.
Kaydet: `_STAGING/Characters/Players/Warblade/animations/basic_attack_2/`

---

### 1.2 Warblade — basic_attack_3 ⚠️ CUSTOM — MALİYET KONTROLÜ

Combo finisher — üç vuruşun zirvesi. En ağır, en güçlü his.

Önce `confirm_cost=false` ile çağır. Maliyeti DONE.txt'e yaz. DUR.

```
animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  action_description="powerful overhead greatsword slam, both hands raised high above head gripping hilt, blade angled skyward briefly at windup, then explosive downward crush into the ground, knees bent absorbing the impact, rift energy bursting outward from impact point, decisive finishing blow, heavier and slower than previous strikes, 7-8 frames",
  animation_name="basic_attack_3",
  directions=["south","south-east","east","north-east","north","north-west","west","south-west"],
  confirm_cost=false
)
```
→ DONE.txt'e yaz: `[COST-CHECK] Warblade/basic_attack_3 | X gen | ONAY BEKLENİYOR`
Onay gelince `confirm_cost=true` ile tekrarla.
Kaydet: `_STAGING/Characters/Players/Warblade/animations/basic_attack_3/`

---

## BÖLÜM 2 — RANGER COMBO

**character_id:** `989529d2-6d75-48d8-8fbe-27df303869bd`

Combo akışı:
- Step 0 = `basic_attack` (Batch 1'de üretildi — yay gerip bırakma)
- Step 1 = `basic_attack_2` → hızlı ikinci atış, daha az şarj ← **bu bölüm**
- Step 2 = `basic_attack_3` → power shot, uzun şarj ← **bu bölüm**

Ranger'ın combo ritmi farklı: her atış aynı mekanik (yay) ama tempo ve güç değişiyor.

---

### 2.1 Ranger — basic_attack_2 (Template)

Hızlı ikinci ok — şarj süresi kısa, çekiş sığ, bırakma anında.
`throw-object` template'i burada "snap release" hissini yeterince verir.

```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  template_animation_id="throw-object",
  animation_name="basic_attack_2"
)
```
Kaydet: `_STAGING/Characters/Players/Ranger/animations/basic_attack_2/`

---

### 2.2 Ranger — basic_attack_3 ⚠️ CUSTOM — MALİYET KONTROLÜ

Combo finisher — güç çekiş, daha uzun şarj duruşu, ağır bırakma.

Önce `confirm_cost=false` ile çağır. Maliyeti DONE.txt'e yaz. DUR.

```
animate_character(
  character_id="989529d2-6d75-48d8-8fbe-27df303869bd",
  action_description="power draw archery shot, archer leans slightly back for leverage, left arm fully extended holding bow, right arm pulling bowstring much further back past the cheek to near the shoulder, visible tension and effort in the draw, held at full draw for two frames showing the strain, then explosive powerful release with right arm fully extending, bow arm recoils more than normal, this is a heavy finishing shot, 7-8 frames",
  animation_name="basic_attack_3",
  directions=["south","south-east","east","north-east","north","north-west","west","south-west"],
  confirm_cost=false
)
```
→ DONE.txt'e yaz: `[COST-CHECK] Ranger/basic_attack_3 | X gen | ONAY BEKLENİYOR`
Onay gelince `confirm_cost=true` ile tekrarla.
Kaydet: `_STAGING/Characters/Players/Ranger/animations/basic_attack_3/`

---

## BÖLÜM 3 — CHAINED COMBO VARYANTLERİ (İSTEĞE BAĞLI)

*Skill kullandıktan sonra basic attack → farklı animasyon. ComboStep 3/4/5.*
*Eğer budget yoksa bu bölümü ATLA — Unity aynı clipi oynatır, %20 bonus hasar kodu zaten çalışır.*
*Kiro karar ver: üretmek istiyor musun? DONE.txt'e not düş.*

### Öneri: Chained versiyon üretmek gerekiyor mu?

**Gerektirmiyor** → mevcut basic_attack_1/2/3 cliplerini chained state'lerde tekrar kullan. Fark sadece VFX overlay ile yapılır (kodu hazır). **Tavsiye edilen** — üretim maliyeti sıfır.

**Gerektirir** → her class için 3 farklı "empowered" animasyon = ~840 gen daha. Sonraki batch'e bırak.

---

## KAYIT YAPISI (BU BATCH İÇİN)

```
_STAGING/
  Characters/Players/
    Warblade/animations/
      basic_attack_2/     ← yeni
      basic_attack_3/     ← yeni
    Ranger/animations/
      basic_attack_2/     ← yeni
      basic_attack_3/     ← yeni
```

Her klasörde 8 yön dosyası: `south.png`, `north.png`, `east.png`, `west.png`, `south-east.png`, `south-west.png`, `north-east.png`, `north-west.png`

---

## İŞLEM SIRASI

**Aşama 1 — Template (hızlı, onaysız):**
→ 2.1 Ranger/basic_attack_2

**Aşama 2 — Custom (maliyet kontrolü, sırayla):**
1. `1.1` Warblade/basic_attack_2 → `confirm_cost=false` → DONE.txt → DUR
2. Onay → `confirm_cost=true`
3. `1.2` Warblade/basic_attack_3 → `confirm_cost=false` → DONE.txt → DUR
4. Onay → `confirm_cost=true`
5. `2.2` Ranger/basic_attack_3 → `confirm_cost=false` → DONE.txt → DUR
6. Onay → `confirm_cost=true`

---

## ANIMATOR BAĞLANTI TABLOSU (Unity için)

### Warblade
| Animator State | Clip | ComboStep koşulu |
|---|---|---|
| BasicAttack1 | `basic_attack` (Batch 1) | ComboStep = 0 |
| BasicAttack2 | `basic_attack_2` (bu batch) | ComboStep = 1 |
| BasicAttack3 | `basic_attack_3` (bu batch) | ComboStep = 2 |
| BasicAttack1_Chained | `basic_attack` (aynı clip) | ComboStep = 3 |
| BasicAttack2_Chained | `basic_attack_2` (aynı clip) | ComboStep = 4 |
| BasicAttack3_Chained | `basic_attack_3` (aynı clip) | ComboStep = 5 |

### Shadowblade (yeni üretim yok)
| Animator State | Clip | ComboStep koşulu |
|---|---|---|
| BasicAttack1 | `basic_attack` / lead-jab (Batch 1) | ComboStep = 0 |
| BasicAttack2 | `dash_attack` / cross-punch (Batch 1) | ComboStep = 1 |
| BasicAttack3 | `heavy_attack` / surprise-uppercut (Batch 1) | ComboStep = 2 |
| BasicAttack1_Chained | `basic_attack` (aynı clip) | ComboStep = 3 |
| BasicAttack2_Chained | `dash_attack` (aynı clip) | ComboStep = 4 |
| BasicAttack3_Chained | `heavy_attack` (aynı clip) | ComboStep = 5 |

### Elementalist (yeni üretim yok)
| Animator State | Clip | ComboStep koşulu |
|---|---|---|
| BasicAttack1 | `cast` (Batch 1) | ComboStep = 0 |
| BasicAttack2 | `cast` (aynı clip) | ComboStep = 1 |
| BasicAttack3 | `cast` (aynı clip) | ComboStep = 2 |
| BasicAttack*_Chained | `cast` (aynı clip) | ComboStep = 3/4/5 |

### Ranger
| Animator State | Clip | ComboStep koşulu |
|---|---|---|
| BasicAttack1 | `basic_attack` / bow draw (Batch 1) | ComboStep = 0 |
| BasicAttack2 | `basic_attack_2` (bu batch) | ComboStep = 1 |
| BasicAttack3 | `basic_attack_3` (bu batch) | ComboStep = 2 |
| BasicAttack1_Chained | `basic_attack` (aynı clip) | ComboStep = 3 |
| BasicAttack2_Chained | `basic_attack_2` (aynı clip) | ComboStep = 4 |
| BasicAttack3_Chained | `basic_attack_3` (aynı clip) | ComboStep = 5 |

---

## TAMAMLAMA

Her animasyon bitince `_STAGING/DONE.txt`'e ekle:
```
[ANIM-DONE] KarakterAdı/animasyon_adı | job_id | tarih | gen_maliyeti
[COST-CHECK] KarakterAdı/animasyon_adı | X gen | ONAY BEKLENİYOR
```

---

## SONRAKI BATCH (BU DOSYADA YAPMA)

- **Düşman animasyonları:** VoidThrall, SeamCrawler, ChainWarden, Penitent, RelicCaster, FractureImp — ayrı batch dosyası açılacak
- **Chained combo varyantları:** Budget varsa Batch 3'e ekle
