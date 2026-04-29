# RIMA — Asset Location Map
*Claude'un aktif haritası. Her batch sonrası güncelle.*
*Son güncelleme: 2026-04-07*

---

## Kök Yollar

```
UNITY_ROOT  = F:/Antigravity Projeler/2d roguelite/RIMA/Assets/
SPRITES     = Assets/Sprites/
PREFABS     = Assets/Prefabs/
ANIMATORS   = Assets/Animations/
STAGING     = Assets/STAGING/
```

---

## Sprite Klasör Yapısı

### Karakterler (Player)
```
Assets/Sprites/Characters/{KarakterAdı}/animations/{anim-adı}/{yön}/frame_000.png ...
```

| Karakter | Mevcut Animasyonlar |
|---|---|
| Warblade | fight-stance-idle, walking-8-frames, running-slide, falling-back-death, quick-horizontal-slash, basic_attack_2, basic_attack_3 |
| Elementalist | breathing-idle, fight-stance-idle-8-frames, walking-6-frames, running-6-frames, running-slide, taking-punch, falling-back-death, fireball |
| Ranger | breathing-idle, fight-stance-idle-8-frames, walking-6-frames, running-6-frames, running-slide, falling-back-death, basic_attack_2, basic_attack_3, throw-object |
| Shadowblade | fight-stance-idle-8-frames, walking-6-frames, running-6-frames, crouched-walking, running-slide, lead-jab, falling-back-death |

**⚠️ Ranger bug:** `animations/Assets/Sprites/Characters/Elementalist/` — yanlış path, düzeltilecek

### Düşmanlar
```
Assets/Sprites/Enemies/{DüşmanAdı}/animations/{anim-adı}/{yön}/frame_000.png ...
```

| Düşman | Mevcut Animasyonlar | Yön Sayısı |
|---|---|---|
| Penitent | walking-6-frames, **holy-condemnation-attack** ✅, falling-back-death | 8 yön |
| ChainWarden | walking-6-frames, **chain-lash-attack** ✅, falling-back-death | 8 yön |
| RelicCaster | walking-6-frames, **relic-bolt-cast** ✅, falling-back-death | 8 yön |
| FractureImp | walking-6-frames, **shard-slash-attack** ✅, falling-back-death | 8 yön |
| SeamCrawler | walking-6-frames, cross-punch, falling-back-death | 8 yön |
| VoidThrall | walking-8-frames, **void-pulse-attack** ✅, **void-thrall-death-fall** ✅ | 8 yön (idle/walk) · 5 yön (attack/death) |
| ShardWalker | idle | — |
| HalfThrall | **half-thrall-idle** ✅, **half-thrall-walk** ✅, **cross-punch (attack)** ✅, **falling-back-death** ✅ | 5 yön |

### Yeni Animasyon Eklerken Klasör Adı Kuralı
```
kebab-case, rakam varsa sonuna yaz
✅ quick-horizontal-slash
✅ basic-attack-2
✅ fireball-cast
❌ attack2  ❌ QuickSlash  ❌ anim new
```

---

## Prefab Yolları

```
Assets/Prefabs/Player/        ← Player prefabları
Assets/Prefabs/Enemies/       ← Enemy prefabları
Assets/Prefabs/Skills/        ← Skill prefabları
Assets/Prefabs/Obstacles/     ← Obstacle prefabları
```

---

## AnimatorController Yolları

```
Assets/Animations/Characters/{KarakterAdı}/{KarakterAdı}AnimatorController.controller
Assets/Animations/Enemies/{DüşmanAdı}/{DüşmanAdı}AnimatorController.controller
```

Clip yolu:
```
Assets/Animations/Characters/{KarakterAdı}/{anim-adı}/{yön}/{KarakterAdı}_{AnimAdı}_{Yön}.anim
```

---

## Staging (Kiro ham çıktıları)

```
Assets/STAGING/DONE.txt          ← Kiro her batch sonrası buraya yazar
Assets/STAGING/{tarih}/          ← Ham zip/png'ler buraya iner
```

---

## Üretim Aracı Kararları — Net

### ✅ ASEPRİTE (kullanıcı yapar, Claude adım söyler)

| Animasyon | Neden |
|---|---|
| Player attack (Warblade/Elementalist/Ranger/Shadowblade) | 100+ kez/session, timing kritik |
| Player dash (4 class) | Çok sık, snappy hissi gerekli |
| Boss attack animasyonları | Kalite kritik |
| İlk karakter base sprite (style lock) | Referans oluşturma |
| Broken frame düzeltme | Inpaint ile |

**Aseprite adım şablonu:**
1. `Assets/Sprites/Characters/{İsim}/rotations/{İsim}_S.png` aç (south referans)
2. Ctrl+Space+P → Plugin
3. "Animate with text (new)" Ctrl+Space+A
4. Reference: Set → current layer
5. Action: "[karakter] [aksiyon] toward south, [silah], 8 frames, no loop"
6. Output: New frame → Generate → beğen/tekrar
7. Export: `File → Export Sprite Sheet` → `animations/{anim-adı}/south/frame_000.png...`
8. Rotate tool ile diğer 7 yön
9. Klasöre yaz: `Assets/Sprites/Characters/{İsim}/animations/{anim-adı}/{yön}/`

---

### ✅ MCP / KİRO (Claude batch yazar, Kiro çalıştırır)

| Animasyon | Neden |
|---|---|
| Mob walk/idle/death (bulk) | Bulk, 8 yön × birden fazla mob |
| Mob attack (Penitent/ChainWarden/RelicCaster/FractureImp) | Enemy tier, bulk |
| VoidThrall/HalfThrall create+animate | Enemy, create+animate |
| Tileset üretimi | Mekanik |
| Yeni enemy base sprite (style_image ile) | Bulk, style lock var |

**MCP kayıt yolu şablonu:**
```
Enemy attack  → Assets/Sprites/Enemies/{İsim}/animations/{anim-adı}/{yön}/frame_000.png
Enemy create  → Assets/Sprites/Enemies/{İsim}/rotations/{İsim}_{Yön}.png
```

---

### ✅ SİTE (pixellab.ai) — Kullanıcı manuel, hızlı test

Sadece: "şöyle görünür mü?" testi, MCP çalışmıyorsa yedek

---

## Aktif Batch Durumu (2026-04-07)

| Batch | Araç | Durum |
|---|---|---|
| KIRO_CHAR_ATTACKS | Aseprite (kullanıcı) | ⏳ Bekliyor |
| KIRO_DASH_ANIMS | Aseprite (kullanıcı) | ⏳ Bekliyor |
| KIRO_MOB_ATTACKS | MCP/Kiro | ⏳ Bekliyor |
| KIRO_VOID_HALFTHRALL | MCP/Kiro | ⏳ Bekliyor |

---

## Bilinen Sorunlar / Fixler

| Sorun | Dosya | Çözüm |
|---|---|---|
| Ranger yanlış klasör | `Ranger/animations/Assets/...` | Taşı → `Ranger/animations/` altına |
| HalfThrall yok | — | KIRO_VOID_HALFTHRALL ile oluşturulacak |
| cross-punch attack'lar yanlış | Penitent/ChainWarden/RelicCaster/FractureImp | KIRO_MOB_ATTACKS ile üretilecek |
