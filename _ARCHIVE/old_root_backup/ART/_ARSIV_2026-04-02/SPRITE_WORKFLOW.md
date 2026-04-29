# RIMA — Sprite Üretim Yol Haritası
*Bu dosya net adım adım talimat içerir. Detay gerekirse → `URETIM_PLANI.md`*
*Araç detayı gerekirse → `GEMINI_PROMPTLARI.md`*
*Mob tasarım lore/davranış detayları → `MOB_TASARIMI.md`*

---

## GENEL PIPELINE

```
Gemini web  →  PixelLab (Aseprite)  →  Kaydet  →  ComfyUI script (animasyon)
 (referans)      (BASE sprite)         (doğru yol)   (otomatik sprite sheet)
```

**Neden bu sıra?**
- Gemini: konsept açı referansı — ücretsiz, hızlı
- PixelLab: "Edit Image Pro" Gemini görselini alıp doğrudan pixel art'a dönüştürür — tasarım fidelity yüksek, background removal built-in
- ComfyUI: animasyonları elle yapmak yerine 1 komutla halleder

---

## ADIM 0 — Tek Seferlik Kurulum

**0.1 — ComfyUI kur**
```
tools/comfyui_install.bat → sağ tık → Yönetici olarak çalıştır
~15 dakika bekle
```

**0.2 — AI modelleri indir** (~8 GB, arka planda devam eder)
```
python tools/rima_models.py
```

**0.3 — PyTorch + Diffusers kur**
```
pip install torch torchvision --index-url https://download.pytorch.org/whl/cu128
pip install diffusers transformers accelerate safetensors
```

---

## ÜRETİM TİPLERİ VE ARAÇ KURALLARI

| Tip | Boyut | PixelLab Aracı | Animasyon |
|-----|-------|----------------|-----------|
| Ana Karakter | 64×64 | Edit Image Pro | ComfyUI |
| Normal Düşman (Grunt) | 32×32 | Edit Image Pro (S-M) ⚠️ | ComfyUI |
| Elite Düşman | 64×64 | Edit Image Pro | ComfyUI |
| Boss | 128×128 | Edit Image Pro, High detail | ComfyUI |
| Statik Düşman | 32×32 | Edit Image Pro (S-M) | YOK |
| Tile | 16×16 veya 16×32 | Create tileset | YOK |
| VFX | 32×32 veya 64×64 | Edit Image Pro (S-M) | PixelLab Animate |

### ⚠️ Evrensel Kurallar

**Silah/El Kuralı:** Karakter sadece bir eliyle silah tutuyorsa, promptta ZORUNLU olarak şunu belirt:
- Description'a: `right hand holds [silah] only, left hand completely empty hanging at side`
- Negative'e: `dual wield, two weapons, weapon in both hands, [silah] in left hand`
Belirtmezsen AI her iki ele silah koyar.

**Mob Spawner Kuralı:** İçinden mob çıkan bir düşman (Hollow Cradle, Rift Maw, Echo Anchor, Shard Brood) üretirken yavru mob siluetleri Description'da belirtilmeli, Negative'de bağımsız/detaylı görünmeleri engellenmelidir. Sprite'ın kendisi ana formu temsil etmeli.

**32×32 Kuralı:** Grunt ve statik düşmanlar için Edit Image Pro kullan, canvas ≤200×200 olmalı. M-XL kullanırsan pikseller çamur olur.

**Edit Image Pro Kuralı:** Her sprite için adımlar:
1. Aseprite → File → New → [hedef boyut: 64×64 veya 32×32] | Transparent
2. Edit → PixelLab → Open plugin → Edit Image Pro
3. "Image to Edit" → Set → Gemini PNG seç (resize edilmiş, ≤180px)
4. "Remove background" → ✓ işaretle
5. Output Method → "Add new layer"

**Fallback (açı yanlışsa):** Gemini'yi yeniden üret, "strict top-down" promptu ekle.

**Top-Down Açı Doğrulama:** Her Gemini çıktısında kontrol et: omuzlar/gövde geniş görünüyor mu? Yüz görünmüyor mu? Ayaklar çok küçük veya yok mu? Değilse aşağıdaki fix komutunu kullan:
```
Regenerate from directly above, strict bird's eye top-down view. Camera is mounted on the ceiling looking straight down. Show wide shoulders, no face, tiny or hidden feet.
```

---

## ══════════════════════════════════════
## FAZ 0 — LOGO
## ══════════════════════════════════════

**Adım 1 — Gemini web'de üret**

gemini.google.com → yeni sohbet → şu promptu yapıştır:

```
Pixel art logo for a game called RIMA. The letters R-I-M-A in bold medieval fantasy style.
The letter I has a vertical crack running through it with cold blue light bleeding out.
The letter A has a void arch shape at its base. Worn gold color with dark iron grey shadows.
The font style is heavy, angular, chiseled stone. Transparent background.
```

Kontrol: "ft" → I altından sarıyor mu? Kırık noktalarda altın var mı?
Yanlışsa: `Regenerate with heavier font weight, more angular chiseling, larger cold blue crack on the I`

Kaydet: `ART/logo/rima_logo_kaynak.png`

**Adım 2 — Boyutlandır**
```
python tools/logo_resize.py "ART/logo/rima_logo_kaynak.png"
```
Çıktı: `ART/logo/rima_logo_320x80.png`, `160x40.png`, `64x64.png`

**URETIM_PLANI.md'de işaretle:** `## LOGO` → `## ✅ LOGO`

---

## ══════════════════════════════════════
## FAZ 1 — WARBLADE (Ana Karakter, 64×64)
## ══════════════════════════════════════

### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → WARBLADE bölümü**

Kaydet: `ART/karakterler/warblade/warblade_gemini_base.png`

---

### ADIM 2 — PixelLab BASE sprite üret

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/warblade/warblade_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"** tıkla
4. Ayarlar:
   - **Image to Edit → Set** → `warblade_gemini_base.png` seç
   - **Instructions:** `Heavily armored dark iron plate warrior, single greatsword in right hand only pointing downward, left hand completely empty hanging at side, cracked breastplate glowing cold blue, torn cape, heavy pauldrons, facing south, dark fantasy, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. **Generate** → beğenince → **CTRL+S**

---

### ADIM 3 — BASE sprite kaydet

`File → Export As` → `ART/karakterler/warblade/warblade_S_BASE.png`

**URETIM_PLANI.md'de işaretle:** `## WARBLADE BASE Sprite` → `## ✅ WARBLADE BASE Sprite`

---

### ADIM 4 — ComfyUI ile animasyonları üret

```
python tools/rima_generate.py --only warblade --anim-only
```

| Çıktı | Frame |
|-------|-------|
| `warblade_S_idle.png` | 4 |
| `warblade_S_walk.png` | 6 |
| `warblade_S_attack1.png` | 6 |
| `warblade_S_attack2.png` | 6 |
| `warblade_S_dash.png` | 4 |
| `warblade_S_hurt.png` | 4 |
| `warblade_S_death.png` | 8 |

~10-15 dakika sürer (RTX 5080 ile).

---

## ══════════════════════════════════════
## FAZ 2 — DİĞER KARAKTERLER (64×64)
## ══════════════════════════════════════

*Faz 2 demo karakterleri. Warblade ile aynı pipeline: Gemini → PixelLab Edit Image Pro → ComfyUI animasyon.*

---

### ELEMENTALİST (Mage | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → ELEMENTALİST bölümü**

Kaydet: `ART/karakterler/elementalist/elementalist_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 Edit Image Pro)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/elementalist/elementalist_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `elementalist_gemini_base.png` seç
   - **Instructions:** `Dark robed mage holding ornate staff in right hand only, left hand channeling arcane fire energy outward, fire embers and ice shards orbiting the body, arcane rune symbols on robes, hood covering head, facing south, dark fantasy mage character, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/elementalist/elementalist_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only elementalist --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `elementalist_S_idle.png` | 4 |
| `elementalist_S_walk.png` | 6 |
| `elementalist_S_cast1.png` | 6 |
| `elementalist_S_cast2.png` | 6 |
| `elementalist_S_blink.png` | 4 |
| `elementalist_S_hurt.png` | 4 |
| `elementalist_S_death.png` | 8 |

---

### SHADOWBLADE (Rogue | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → SHADOWBLADE bölümü**

Kaydet: `ART/karakterler/shadowblade/shadowblade_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 Edit Image Pro)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/shadowblade/shadowblade_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `shadowblade_gemini_base.png` seç
   - **Instructions:** `Dark leather armored rogue assassin, short dagger in right hand and short dagger in left hand, both hands armed as dual wield assassin, low crouching stance, dark shadow trail behind figure, smoke wisps from daggers, full black hood, facing south, dark fantasy rogue, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/shadowblade/shadowblade_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only shadowblade --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `shadowblade_S_idle.png` | 4 |
| `shadowblade_S_walk.png` | 6 |
| `shadowblade_S_attack1.png` | 5 |
| `shadowblade_S_attack2.png` | 5 |
| `shadowblade_S_stealth.png` | 4 |
| `shadowblade_S_hurt.png` | 4 |
| `shadowblade_S_death.png` | 8 |

---

### RANGER (Hunter | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → RANGER bölümü**

Kaydet: `ART/karakterler/ranger/ranger_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 Edit Image Pro)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/ranger/ranger_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `ranger_gemini_base.png` seç
   - **Instructions:** `Lean ranger hunter in dark leather armor, longbow held horizontally in left hand, right hand drawn back in release position, quiver of arrows visible on back from above, wide stance for stability, hood or ranger hat from top view, facing south, dark fantasy ranger, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/ranger/ranger_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only ranger --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `ranger_S_idle.png` | 4 |
| `ranger_S_walk.png` | 6 |
| `ranger_S_shoot.png` | 6 |
| `ranger_S_disengage.png` | 4 |
| `ranger_S_trap.png` | 5 |
| `ranger_S_hurt.png` | 4 |
| `ranger_S_death.png` | 8 |

---

### RAVAGER (Berserker | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → RAVAGER bölümü**

Kaydet: `ART/karakterler/ravager/ravager_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 M-XL)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/ravager/ravager_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `ravager_gemini_base.png` seç
   - **Instructions:** `Brutal berserker warrior in torn battle-damaged armor, massive great axe gripped with both hands in center of body, axe head wider than shoulders, mismatched broken armor pieces, visible battle scars, faint red fury aura at body edges, wide aggressive silhouette, facing south, dark fantasy berserker, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/ravager/ravager_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only ravager --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `ravager_S_idle.png` | 4 |
| `ravager_S_walk.png` | 6 |
| `ravager_S_attack1.png` | 6 |
| `ravager_S_attack2.png` | 6 |
| `ravager_S_rage.png` | 4 |
| `ravager_S_hurt.png` | 4 |
| `ravager_S_death.png` | 8 |

---

### PALADİN (Holy Warrior | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → PALADİN bölümü**

Kaydet: `ART/karakterler/paladin/paladin_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 M-XL)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/paladin/paladin_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `paladin_gemini_base.png` seç
   - **Instructions:** `Holy paladin warrior in heavy immaculate plate armor, large tower shield in left hand extending left side of silhouette, war hammer in right hand head pointing downward, golden holy light bleeding through thin cracks in armor plating, divine energy glow, facing south, dark fantasy paladin, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/paladin/paladin_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only paladin --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `paladin_S_idle.png` | 4 |
| `paladin_S_walk.png` | 6 |
| `paladin_S_attack1.png` | 5 |
| `paladin_S_shield.png` | 4 |
| `paladin_S_consecrate.png` | 6 |
| `paladin_S_hurt.png` | 4 |
| `paladin_S_death.png` | 8 |

---

### SUMMONER (Necromancer | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → SUMMONER bölümü**

Kaydet: `ART/karakterler/summoner/summoner_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 M-XL)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/summoner/summoner_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `summoner_gemini_base.png` seç
   - **Instructions:** `Dark necromancer summoner in flowing void black robes with bone skull accessories, bone staff topped with skull in right hand only, left hand outstretched palm up with dark necromantic energy rising, two tiny skeleton minion silhouettes orbiting around the figure, thin frail form, facing south, dark fantasy necromancer, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/summoner/summoner_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only summoner --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `summoner_S_idle.png` | 4 |
| `summoner_S_walk.png` | 6 |
| `summoner_S_summon.png` | 8 |
| `summoner_S_sacrifice.png` | 6 |
| `summoner_S_command.png` | 5 |
| `summoner_S_hurt.png` | 4 |
| `summoner_S_death.png` | 8 |

---

### HEXER (Warlock | 64×64)

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → HEXER bölümü**

Kaydet: `ART/karakterler/hexer/hexer_gemini_base.png`

#### ADIM 2 — PixelLab BASE sprite üret (64×64 M-XL)

1. Aseprite → `File → New` → **64×64** | Transparent → OK
2. `File → Save As` → `ART/karakterler/hexer/hexer_S_BASE.aseprite`
3. `Edit → PixelLab → Open plugin` → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `hexer_gemini_base.png` seç
   - **Instructions:** `Dark warlock hexer in deep void purple corrupted coat or robe, cursed tome held in left hand open and floating, right hand raised with fingers spread dripping void purple corruption energy downward like liquid, void purple corruption spreading across the fabric, hex symbols visible on clothing, wide brimmed hat or dark hood, facing south, dark fantasy warlock, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/karakterler/hexer/hexer_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only hexer --anim-only
```
| Çıktı | Frame |
|-------|-------|
| `hexer_S_idle.png` | 4 |
| `hexer_S_walk.png` | 6 |
| `hexer_S_curse.png` | 5 |
| `hexer_S_channel.png` | 6 |
| `hexer_S_burst.png` | 8 |
| `hexer_S_hurt.png` | 4 |
| `hexer_S_death.png` | 8 |

---

## ══════════════════════════════════════
## FAZ 1 — GRUNT TİER (32×32) — 6 MOB
## ══════════════════════════════════════

---

### GRUNT 1 — SHARD WALKER (Fractured | Act 1-2-3)

**Lore özeti:** Parçalanan bir savaşçının taş ve kemik kalıntıları yanlış sıraya girmiş. Her akt renk değişiyor (mavi → mor → altın).

**⚠️ 3 renk varyantı gerekiyor:** BASE sprite üretildikten sonra Aseprite'ta 2 recolor:
- Act 1 BASE: soğuk mavi glow → `grunt_shard_S_BASE_act1.png`
- Act 2 recolor: mavi → mor → `grunt_shard_S_BASE_act2.png`
- Act 3 recolor: mavi → altın → `grunt_shard_S_BASE_act3.png`
Palette swap: `Edit → Replace Color` ile glow rengini değiştirmek yeterli.

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → SHARD WALKER bölümü**

Kaydet: `ART/dusmanlar/grunt_shard/grunt_shard_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 — Edit Image Pro)

1. Aseprite → **File → New** → **32×32** | Transparent → OK
2. **File → Save As** → `ART/dusmanlar/grunt_shard/grunt_shard_S_BASE.aseprite`
3. **Edit → PixelLab → Open plugin** → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `grunt_shard_gemini_base.png` seç
   - **Instructions:** `Humanoid creature assembled from floating broken stone shards with gaps between pieces, cold blue light bleeding through the gaps, no solid body, hovering shards in warrior shape, facing downward, dark fantasy enemy, top-down pixel art sprite`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

Kaydet: `ART/dusmanlar/grunt_shard/grunt_shard_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only shard_walker --anim-only
```
| Animasyon | Frame | FPS |
|-----------|-------|-----|
| idle | 4 | 6 |
| walk | 6 | 10 |
| attack | 5 | 12 |
| death | 6 | 8 |

---

### GRUNT 2 — VOID THRALL (Fractured | Act 1-2)
**Split mekanik:** Öldüğünde ikiye bölünür, her yarısı bağımsız savaşır.

#### ADIM 1 — Gemini referans (1 sohbet, sadece tam form)

**→ `ART/GEMINI_PROMPTLARI.md` → VOID THRALL bölümü**

Kaydet: `ART/dusmanlar/grunt_thrall/grunt_thrall_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro — sadece tam form)

- **Edit Image Pro → Image to Edit → Set:** `grunt_thrall_gemini_base.png`
- **Instructions:** `Humanoid warrior in dark armor, deep vertical crack through chest glowing void purple, sinister medieval soldier, hidden face, facing south, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_thrall/grunt_thrall_S_BASE.png`

#### ADIM 3 — Aseprite'ta yarıları kes

Tam form BASE sprite'ı içeri al, Aseprite'ta ikiye böl:

1. `grunt_thrall_S_BASE.png` aç
2. **Sol yarı:** Canvas'ı `16×32`'ye kırp (sol tarafı tut) → sağ kenar piksellerine void mor (#9E4FE0) 2-3px glow ekle → Export: `grunt_thrall_half_left_S_BASE.png`
3. **Sağ yarı:** Orijinali tekrar aç, canvas'ı `16×32`'ye kırp (sağ tarafı tut) → sol kenar piksellerine void mor (#9E4FE0) 2-3px glow ekle → Export: `grunt_thrall_half_right_S_BASE.png`

*Neden bu yol: Gemini'den 3 ayrı sohbet istersen yarılar tam formla uyuşmaz. Aseprite'ta kesmek piksel piksel tutarlı garanti eder.*

Hepsi → `ART/dusmanlar/grunt_thrall/` klasörüne.

*Animasyon script'te tanımlı değil — sadece Split animasyonu (8 frame) Aseprite'ta elle çizilecek.*

---

### GRUNT 3 — SEAM CRAWLER (Rift-Born | Act 1-2)
**Zemin çatlaklarında yaşıyor, gövdesinin %80'i görünmüyor.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → SEAM CRAWLER bölümü**

Kaydet: `ART/dusmanlar/grunt_seam/grunt_seam_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `grunt_seam_gemini_base.png`
- **Instructions:** `Top-down view creature hiding in floor crack, only two long dark claws and spine ridge visible above surface, shadowy horror predator, flat against the fissure, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_seam/grunt_seam_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only seam_crawler --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| slide | 4 | 10 | loop — pençeler çatlak boyunca kayar |
| attack | 6 | 12 | pençeler yukarı çıkar, vurur, iner |
| death | 4 | 6 | zemine geri çekilir, çatlak kapanır |

---

### GRUNT 4 — ECHO HOUND (Fractured | Act 2-3)
**Her hareketinde afterimage bırakıyor, echo da hasar veriyor.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → ECHO HOUND bölümü**

Kaydet: `ART/dusmanlar/grunt_echo/grunt_echo_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `grunt_echo_gemini_base.png`
- **Instructions:** `Ghostly wolf-like predator creature, semi-transparent indigo body with white glowing eyes, motion afterimage trailing behind, predatory low crouch, top-down view, dark fantasy enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_echo/grunt_echo_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only echo_hound --anim-only
```
| Animasyon | Frame | FPS |
|-----------|-------|-----|
| idle | 4 | 6 |
| run | 6 | 14 |
| attack | 5 | 12 |
| death | 8 | 6 |

---

### GRUNT 5 — HOLLOW MITE (Rift-Born | Act 1 sürü, Act 2)
**Sürü halinde 4-8 birden spawn olur. Sprite alanının %30'unu kaplar — küçük olmalı.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → HOLLOW MITE bölümü**

Kaydet: `ART/dusmanlar/grunt_mite/grunt_mite_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `grunt_mite_gemini_base.png`
- **Instructions:** `Tiny hollow insect creature with transparent body showing empty interior, dark exoskeleton with six legs, tiny glowing core inside hollow shell, swarm enemy, top-down view, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_mite/grunt_mite_S_BASE.png`

*Animasyon: ComfyUI yerine Aseprite'ta 4 frame idle (titreme) yeterli — çok küçük.*

---

### GRUNT 6 — CHAIN BOUND ⭐ YENİ (Fractured | Act 2-3)
**3 sprite birden — void zincirleriyle bağlı. Biri ölünce zincir enerjisi diğerlerine akar.**

*Bu mob 3 adet 32×32 sprite'tan oluşur. Unity'de tek "enemy group" olarak işlenir.*

#### ADIM 1 — Gemini referans (1 sohbet — tek figür üret)

**→ `ART/GEMINI_PROMPTLARI.md` → CHAIN BOUND bölümü**

Kaydet: `ART/dusmanlar/grunt_chain/grunt_chain_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro — 1 kez üret)

- **Edit Image Pro → Image to Edit → Set:** `grunt_chain_gemini_base.png`
- **Instructions:** `Small damaged dark armored soldier, void purple glowing chain anchor point on chest, aggressive combat stance, facing south, dark fantasy grunt enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_chain/grunt_chain_S_BASE.png`

#### ADIM 3 — Aseprite'ta 3 varyant üret

BASE sprite'tan Aseprite'ta 3 kopya yap, her birinde küçük hasar farklılığı ekle:

| Dosya | Export yolu | Elle değiştir |
|-------|-------------|--------------|
| `grunt_chain_A_S_BASE.aseprite` | `grunt_chain_A_S_BASE.png` | Sol omuz plakasını sil/kırık göster |
| `grunt_chain_B_S_BASE.aseprite` | `grunt_chain_B_S_BASE.png` | Göğüs plakasında çatlak çiz |
| `grunt_chain_C_S_BASE.aseprite` | `grunt_chain_C_S_BASE.png` | Sağ kol kısmını hafif eksik bırak |

*Neden bu yol: 3 kez PixelLab çalıştırmak tutarsız sonuç verir. 1 BASE + 3 Aseprite varyant çok daha hızlı ve tutarlı.*

Hepsi → `ART/dusmanlar/grunt_chain/` klasörüne.

*Zincir efekti Unity'de LineRenderer ile çizilecek — sprite'ta zincir olmayacak.*

---

## ══════════════════════════════════════
## FAZ 1 — ELİTE TİER (64×64) — 6 MOB
## ══════════════════════════════════════

---

### ELİTE 1 — THE TWICE-BORN (Fractured | Act 1 nadir, Act 2)
**2 bağlı figür. Biri ölünce diğeri berserk. 64×64 alanda yan yana iki 32'lik siluet.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → THE TWICE-BORN bölümü**

Kaydet: `ART/dusmanlar/elite_twiceborn/twiceborn_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `twiceborn_gemini_base.png`
- **Instructions:** `Two tethered dark armored warriors side by side, one holding a shield in defensive stance, one with sword raised in right hand only left hand empty, golden glowing thread connecting their chests heart to heart, dual elite enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_twiceborn/twiceborn_S_BASE.png`

*Not: Unity'de tek "enemy" olarak işleniyor ama iki bağımsız HP barı var.*

---

### ELİTE 2 — FRACTURE-BORN (Emergent | Act 2-3)
**Yerden çıkan mob. 4 aşamalı spawn animasyonu — Aşama 1-2'de öldürülürse instant kill.**

*Spawn animasyonu ComfyUI tarafından BASE sprite'tan otomatik üretiliyor. Ayrı spawn sprite'a gerek yok.*

#### ADIM 1A — Gemini referans (SPAWN — atmosfer/kol referansı)

**→ `ART/GEMINI_PROMPTLARI.md` → FRACTURE-BORN bölümü → 1. SPAWN REFERANSI**

*Bu görsel PixelLab'a Init Image olarak girmeyecek — sadece kolların nasıl görüneceğini anlamak için referans.*

Kaydet: `ART/dusmanlar/elite_fracture/fracture_gemini_spawn_ref.png`

#### ADIM 1B — Gemini referans (TAM FORM — PixelLab'a girecek)

**→ `ART/GEMINI_PROMPTLARI.md` → FRACTURE-BORN bölümü → 2. TAM FORM**

Kaydet: `ART/dusmanlar/elite_fracture/fracture_gemini_fullform.png`

#### ADIM 2 — PixelLab TAM FORM sprite (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `fracture_gemini_fullform.png`
- **Instructions:** `Tall thin creature with disproportionately long arms evolved for crack climbing, near-black body with glowing joint seams, void energy at joints, crack scar on spine, predatory crouching posture, dark fantasy elite, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_fracture/fracture_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only fracture_born --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| spawn | 18 | 6 | 4 aşama — en kritik animasyon |
| idle | 6 | 4 | omurga çatlağı nefes alıyor |
| walk | 8 | 8 | uzun adımlar, kollar sürükleniyor |
| attack | 8 | 10 | kollarını uzatarak saldırı |
| death | 10 | 6 | ters spawn, zemine geri çekilme |

---

### ELİTE 3 — SPORE HOLLOW (Emergent Kolonize | Act 2)
**Ölüm patlaması 5 tile radius — uzaktan öldür, kaç.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → SPORE HOLLOW bölümü**

Kaydet: `ART/dusmanlar/elite_spore/spore_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `spore_gemini_base.png`
- **Instructions:** `Hollow human shell with blank featureless face and empty eyes, orange-brown mushroom spore growths bursting from body cracks especially shoulders and back, shambling colonized body, dark fantasy elite enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_spore/spore_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only spore_hollow --anim-only
```
| Animasyon | Frame | FPS |
|-----------|-------|-----|
| idle | 4 | 5 |
| walk | 6 | 7 |
| attack | 6 | 10 |
| death | 8 | 6 |

---

### ELİTE 4 — SHARD BROOD ⭐ YENİ (Fractured | Act 2-3)
**%50 HP'de 2 Shard Walker spawnar. Öldüğünde 1 daha. 3 kademeli bölünme.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → SHARD BROOD bölümü**

Kaydet: `ART/dusmanlar/elite_shardbrood/shardbrood_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `shardbrood_gemini_base.png`
- **Instructions:** `Large unstable humanoid assembled from many floating stone shards, three distinct smaller shard clusters attached to torso ready to break free, cold blue glow through all gaps between shards, massive fractured warrior form, dark fantasy elite enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_shardbrood/shardbrood_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only shard_brood --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| idle | 4 | 5 | kümeler hafif titriyor |
| walk | 6 | 8 | |
| attack | 6 | 10 | bir kümeyi fırlatarak saldırı |
| split | 8 | 6 | küme kopup dağılıyor — spawn anı |
| death | 8 | 6 | tüm parçalar dağılıyor |

---

### ELİTE 5 — HOLLOW CRADLE ⭐ YENİ (Emergent Kolonize | Act 2)
**İçinde Hollow Mite'lar barındırıyor. Hareket ederken mite bırakır, ölünce 4-6 mite spawnar.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → HOLLOW CRADLE bölümü**

Kaydet: `ART/dusmanlar/elite_cradle/cradle_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `cradle_gemini_base.png`
- **Instructions:** `Hollow pale grey human shell body covered in deep cracks, small insect creatures with glowing eyes visible inside through the cracks, some mites crawling out from the largest crack openings, living incubator host, dark fantasy elite enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_cradle/cradle_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only hollow_cradle --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| idle | 4 | 5 | mite gözleri parıldıyor |
| walk | 6 | 7 | ağır sendeleme |
| attack | 5 | 10 | göğüs çatlağından mite fırlatır |
| death | 10 | 6 | tamamen çatlar, mite'lar dağılır |

---

### ELİTE 6 — ECHO ANCHOR ⭐ YENİ (Fractured | Act 3)
**Sabit. Her 8s bir Echo Hound spawnar. Yok edilince tüm aktif Echo Hound'lar anında kaybolur.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → ECHO ANCHOR bölümü**

Kaydet: `ART/dusmanlar/elite_anchor/anchor_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `anchor_gemini_base.png`
- **Instructions:** `Translucent indigo ghostly humanoid figure chained to ground by void chains, faint echo hound silhouettes orbiting slowly around it, chained immobile elite spawner enemy, top-down view, dark fantasy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_anchor/anchor_S_BASE.png`

*Animasyon: sadece idle (6 frame, 4 FPS) — titreme ve orbiting echo'lar. Statik düşman.*

---

## ══════════════════════════════════════
## FAZ 1 — STATİK HAZARD TİER — 2 MOB
## ══════════════════════════════════════

---

### STATİK 1 — RIFT MAW (Rift-Born | Sadece Act 3)
**Hareket etmez. Çekim alanı var. Hollow Mite spawnar. Öncelikli hedef.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → RIFT MAW bölümü**

Kaydet: `ART/dusmanlar/static_riftmaw/riftmaw_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `riftmaw_gemini_base.png`
- **Instructions:** `Large circular rift opening in floor, jagged reality-torn edges like teeth, pure void black interior, faint golden glow deep inside, particles being pulled inward, pulsing alive edges, stationary floor hazard enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/static_riftmaw/riftmaw_S_BASE.png`

*Animasyon: idle pulse animasyonu (8 frame, 5 FPS) Aseprite'ta elle — sadece kenar titremesi.*

---

### STATİK 2 — THE WOUND (Rift-Born | Act 1-2-3 özel oda)
**Havada asılı. Etraftaki düşmanları iyileştiriyor. Önce bunu öldür.**

#### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → THE WOUND bölümü**

Kaydet: `ART/dusmanlar/static_wound/wound_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `wound_gemini_base.png`
- **Instructions:** `Floating oval wound in reality, organic ragged crimson-purple pulsing edges, void black interior, red particle tendrils extending outward to heal nearby enemies, conscious injury entity hovering above ground, dark fantasy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/static_wound/wound_S_BASE.png`

*Animasyon: idle pulse (8 frame, 5 FPS) — kenar titremesi + kırmızı partiküller döner.*

---

## ══════════════════════════════════════
## FAZ 2-3 — ÖZEL MOB'LAR (Lore-Critical)
## ══════════════════════════════════════

*Bu mob'lar Act 2-3 veya özel durum. Faz 1'den sonra üretilecek.*

---

### ÖZEL 1 — CLASS MIMIC (Fractured | Act 2-3)
**Oyuncunun primary class siluetinin bozuk kopyası. Her class için ayrı sprite üretilir.**

*Örnek: Warblade oynuyorsan Mimic de kılıç kullanıyor. Her class için ayrı Gemini promptu gerekir.*

#### ADIM 1 — Gemini referans (Warblade Mimic örneği)

**→ `ART/GEMINI_PROMPTLARI.md` → CLASS MIMIC bölümü**

Kaydet: `ART/dusmanlar/special_mimic/mimic_warblade_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `mimic_warblade_gemini_base.png`
- **Instructions:** `Distorted translucent mirror copy of dark armored warrior, desaturated void-tinted purple silhouette, no eyes only void where face should be, pixels dissolving at edges, sword in right hand only left hand empty, corrupted ghost copy enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/special_mimic/mimic_warblade_S_BASE.png`

*Her class için aynı workflow, sadece prompt sonu değişir: silah tipi, duruş.*

---

### ÖZEL 2 — REMNANT HOST (Fractured | Act 3)
**3 ruh aynı bedende. Her 15s ruh değişiyor. Renk = direnç tipi ipucu.**

#### ADIM 1 — Gemini referans (3 form — ayrı sohbetler)

**→ `ART/GEMINI_PROMPTLARI.md` → REMNANT HOST bölümü**

Kaydet: `ART/dusmanlar/special_remnant/remnant_gemini_form1.png`, `remnant_gemini_form2.png`, `remnant_gemini_form3.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

Her form için ayrı PixelLab üretimi:

- **Edit Image Pro → Image to Edit → Set:** `remnant_gemini_form1.png` (form1 için), `remnant_gemini_form2.png` (form2), `remnant_gemini_form3.png` (form3)
- **Instructions (form1):** `Humanoid figure with red tint glitching between forms, pixels dissolving at edges, three ghost overlapping forms barely visible, unstable entity with three souls fighting for control, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

| Dosya | Renk | Silah |
|-------|------|-------|
| `remnant_form1_S_BASE.png` | Kırmızı | Kılıç, sağ elde |
| `remnant_form2_S_BASE.png` | Mavi | Asa, sağ elde |
| `remnant_form3_S_BASE.png` | Yeşil | Yay, sağ elde |

*Unity'de tek enemy, 3 sprite asset arasında swap yapılıyor.*

---

## ══════════════════════════════════════
## FAZ 1 — IRON WARDEN (Act 1 Boss, 128×128)
## ══════════════════════════════════════

### ADIM 1 — Gemini referans

**→ `ART/GEMINI_PROMPTLARI.md` → IRON WARDEN bölümü**

Kaydet: `ART/dusmanlar/boss/iron_warden/boss_iron_warden_gemini_base.png`

---

### ADIM 2 — PixelLab BASE (128×128 — Edit Image Pro, HIGH DETAIL)

1. Aseprite → **File → New** → **128×128** | Transparent → OK
2. **File → Save As** → `ART/dusmanlar/boss/iron_warden/boss_iron_warden_S_BASE.aseprite`
3. **Edit → PixelLab → Open plugin** → **"Edit Image Pro"**
4. Ayarlar:
   - **Image to Edit → Set** → `boss_iron_warden_gemini_base.png` seç
   - **Instructions:** `Massive heavily armored iron golem guardian, full dark plate armor with deep cracks, cold blue energy seeping through cracked breastplate, broken swords embedded in back and shoulders, imposing enormous dark fantasy boss, top-down pixel art sprite, high detail`
   - **Remove background:** ✓ işaretle
   - **Output Method:** `Add new layer`
5. Generate → **CTRL+S**

---

### ADIM 3 — Kaydet

`File → Export As` → `ART/dusmanlar/boss/iron_warden/boss_iron_warden_S_BASE.png`

---

### ADIM 4 — Animasyon
```
python tools/rima_generate.py --only iron_warden --anim-only
```
| Animasyon | Frame | FPS |
|-----------|-------|-----|
| idle | 4 | 4 |
| walk | 6 | 6 |
| attack1 | 6 | 8 |
| charge | 8 | 10 |
| hurt | 4 | 8 |
| death | 10 | 5 |

---

## ══════════════════════════════════════
## FAZ 1 — TİLE SETİ
## ══════════════════════════════════════

*Tile'lar için PixelLab "Create tileset" kullanılır. ComfyUI animasyon gerekmez.*

| Tile | Araç | Boyut | Dosya yolu |
|------|------|-------|------------|
| Floor (3 varyasyon) | Create tileset | 16×16 | `ART/tiles/floor_v1/2/3.png` |
| Wall (2 varyasyon) | Create tileset | 16×32 | `ART/tiles/wall_v1/2.png` |
| Crack overlay (4 varyasyon) | Edit Image Pro (S-M) | 16×16 | `ART/tiles/crack_v1/2/3/4.png` |

**Tile için PixelLab ayarı notu:** Create tileset seçeneğinde "Seamless" ON olmalı — yoksa tile kenarları çakışmaz.

---

## ══════════════════════════════════════
## FAZ 1 — VFX
## ══════════════════════════════════════

*VFX sprite'lar küçük ve animasyon sayısı az. PixelLab yeterli — ComfyUI gerekmez.*

| VFX | Boyut | Frame | PixelLab Aracı |
|-----|-------|-------|----------------|
| Hit spark | 32×32 | 6 | Edit Image Pro (S-M) |
| Cold blue burst | 32×32 | 6 | Edit Image Pro (S-M) |
| Void crack | 32×32 | 4 | Edit Image Pro (S-M) |
| Death dissolve overlay | 64×64 | 8 | Edit Image Pro (S-M) |
| Spawn crack (zemin) | 32×32 | 6 | Edit Image Pro (S-M) |
| Chain link snap | 32×32 | 4 | Edit Image Pro (S-M) |
| Spore cloud | 32×32 | 6 | Edit Image Pro (S-M) |

---

## TAMAMLANMA KONTROL LİSTESİ

```
FAZ 0:
  [ ] Logo (Gemini + logo_resize.py)

FAZ 1 Ana Karakter:
  [ ] Warblade BASE (PixelLab Edit Image Pro 64×64)
  [ ] Warblade animasyonlar x7 (ComfyUI)

FAZ 1 Grunt Düşmanlar (32×32):
  [ ] Shard Walker BASE + x4 animasyon
  [ ] Void Thrall x3 BASE (tam + 2 yarı, static split)
  [ ] Seam Crawler BASE + x3 animasyon
  [ ] Echo Hound BASE + x4 animasyon
  [ ] Hollow Mite BASE + x1 idle animasyon
  [ ] Chain Bound x3 BASE (A/B/C figürleri)

FAZ 1 Elite Düşmanlar (64×64):
  [ ] The Twice-Born BASE + animasyon
  [ ] Fracture-Born SPAWN sprite + TAM FORM BASE + x5 animasyon
  [ ] Spore Hollow BASE + x4 animasyon
  [ ] Shard Brood BASE + x5 animasyon (split dahil)
  [ ] Hollow Cradle BASE + x4 animasyon
  [ ] Echo Anchor BASE + x1 idle animasyon

FAZ 1 Statik Hazard:
  [ ] Rift Maw BASE + idle pulse animasyon
  [ ] The Wound BASE + idle pulse animasyon

FAZ 1 Boss:
  [ ] Iron Warden BASE (PixelLab Edit Image Pro 128×128 High Detail)
  [ ] Iron Warden animasyonlar x6 (ComfyUI)

FAZ 1 Ortam:
  [ ] Floor tile x3 (PixelLab Create tileset)
  [ ] Wall tile x2 (PixelLab Create tileset)
  [ ] Crack overlay x4 (PixelLab S-M)

FAZ 1 VFX:
  [ ] Hit spark
  [ ] Cold blue burst
  [ ] Void crack
  [ ] Death dissolve
  [ ] Spawn crack
  [ ] Chain link snap
  [ ] Spore cloud

FAZ 2-3 Özel Mob'lar (sonra üretilecek):
  [ ] Class Mimic BASE x8 (her class için) + animasyon
  [ ] Remnant Host x3 form BASE + animasyon

FAZ 2 Karakterler (64×64):
  [ ] Elementalist BASE + x7 animasyon
  [ ] Shadowblade BASE + x7 animasyon
  [ ] Ranger BASE + x7 animasyon
  [ ] Ravager BASE + x7 animasyon
  [ ] Paladin BASE + x7 animasyon
  [ ] Summoner BASE + x7 animasyon
  [ ] Hexer BASE + x7 animasyon

Act Bazlı Yeni Mob'lar:
  [ ] Hall Sentinel BASE + x5 animasyon (Act 1 elite)
  [ ] Void Phantom BASE + x5 animasyon (Act 2 grunt)
  [ ] Convergence Wraith BASE + x5 animasyon (Act 3 elite)

Act Mekan Tile'ları:
  [ ] Act 2 Floor tile + Wall tile (PixelLab tileset)
  [ ] Act 3 Floor tile + Wall tile (PixelLab tileset)
```

---

## MOB ÇEŞİTLİLİĞİ — ÖZET TABLO

| Mob | Kategori | Akt | Boyut | Özel Mekanik |
|-----|----------|-----|-------|-------------|
| Shard Walker | Grunt | 1-2-3 | 32×32 | Renk aktla değişir |
| Void Thrall | Grunt | 1-2 | 32×32 | Ölünce ikiye bölünür |
| Seam Crawler | Grunt | 1-2 | 32×32 | Zemin çatlaklarında yaşar |
| Echo Hound | Grunt | 2-3 | 32×32 | Afterimage hasar verir |
| Hollow Mite | Grunt | 1 sürü, 2 | 32×32 | 4-8 birden spawn |
| Chain Bound | Grunt | 2-3 | 32×32 | **YENİ** — 3'lü bağlı grup |
| The Twice-Born | Elite | 1 nadir, 2 | 64×64 | İp: hasar paylaşımı |
| Fracture-Born | Elite | 2-3 | 64×64 | Yerden çıkma, spawn'da öldür |
| Spore Hollow | Elite | 2 | 64×64 | Ölüm patlaması, spor alan |
| Shard Brood | Elite | 2-3 | 64×64 | **YENİ** — 3 kademeli bölünme |
| Hollow Cradle | Elite | 2 | 64×64 | **YENİ** — içinden mite spawnar |
| Echo Anchor | Elite/Sabit | 3 | 64×64 | **YENİ** — hound spawnar, öldür = hepsi kaybolur |
| Rift Maw | Statik Hazard | 3 | 64×64 | Çekim + mite spawn |
| The Wound | Statik Hazard | 1-2-3 özel | 64×64 | Düşmanları iyileştirir |
| Class Mimic | Özel | 2-3 | 64×64 | Player build'ini kopyalar |
| Remnant Host | Özel | 3 | 64×64 | 3 ruh/form/direnç tipi |
| Iron Warden | Boss | Act 1 | 128×128 | Charge, faz |
| Hall Sentinel | Elite | 1 | 64×64 | Hareketsiz uyuma, sonra ani saldırı |
| Void Phantom | Grunt | 2 | 32×32 | Duvardan geçme, pozisyon değiştirme |
| Convergence Wraith | Elite | 3 | 64×64 | 2 farklı saldırı tipi, asimetrik |

---

## ══════════════════════════════════════
## ACT BAZLI YENİ MOB'LAR
## ══════════════════════════════════════

### ACT 1 — HALL SENTINEL (Fractured | Act 1, Elite 64×64)

**Konsept:** Artık var olmayan bir kapıyı hâlâ koruyan taş bekçi. Yaklaşılana kadar hareketsiz.

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → HALL SENTINEL bölümü**

Kaydet: `ART/dusmanlar/elite_sentinel/sentinel_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `sentinel_gemini_base.png`
- **Instructions:** `Massive stone guardian statue, tower shield in left arm shield face visible from above, stone war hammer in right arm head resting on ground, dark stone plate armor carved directly into the stone form, cold blue glowing cracks throughout the stone, perfectly still like a statue but ready to move, dark fantasy elite enemy, top-down pixel art sprite, high detail`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_sentinel/sentinel_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only hall_sentinel --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| idle | 2 | 2 | neredeyse hiç hareket yok — taş gibi |
| wake | 6 | 8 | uyanma animasyonu — gözler açılıyor |
| walk | 6 | 5 | çok ağır, yavaş adımlar |
| attack | 6 | 8 | yavaş ama ezici |
| death | 8 | 5 | parçalanarak düşüyor |

---

### ACT 2 — VOID PHANTOM (Rift-Born | Act 2, Grunt 32×32)

**Konsept:** Void'e tamamen çözülmüş enerji varlık. Duvarlardan geçebilir.

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → VOID PHANTOM bölümü**

Kaydet: `ART/dusmanlar/grunt_phantom/phantom_gemini_base.png`

#### ADIM 2 — PixelLab BASE (32×32 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `phantom_gemini_base.png`
- **Instructions:** `Humanoid figure completely dissolved into void purple energy, vague human silhouette of concentrated void energy, blurry edges fading to near-invisible, no solid features no armor no weapons, only energy in human shape, top-down view, dark fantasy grunt enemy, top-down pixel art sprite`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/grunt_phantom/phantom_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only void_phantom --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| idle | 4 | 5 | titreşim, şekil belirsizleşip netleşiyor |
| walk | 6 | 10 | kayarak hareket |
| phase | 4 | 8 | duvara geçiş — tamamen bulanıklaşıyor |
| attack | 5 | 12 | anlık yoğunlaşma ve vurma |
| death | 6 | 6 | dağılarak yok oluyor |

---

### ACT 3 — CONVERGENCE WRAITH (Emergent | Act 3, Elite 64×64)

**Konsept:** Birden fazla yok olmuş dünyanın parçalarından oluşmuş varlık. Birden fazla saldırı stili.

#### ADIM 1 — Gemini referans al

**→ `ART/GEMINI_PROMPTLARI.md` → CONVERGENCE WRAITH bölümü**

Kaydet: `ART/dusmanlar/elite_wraith/wraith_gemini_base.png`

#### ADIM 2 — PixelLab BASE (64×64 Edit Image Pro)

- **Edit Image Pro → Image to Edit → Set:** `wraith_gemini_base.png`
- **Instructions:** `Chaotic humanoid figure assembled from fragments of multiple destroyed worlds, one arm dark stone one arm corroded metal torso mixed materials with void tissue in gaps, golden glowing cracks at every seam holding pieces together kintsugi style, asymmetrical unsettling silhouette, dark fantasy Act 3 elite enemy, top-down pixel art sprite, high detail`
- **Remove background:** ✓ işaretle
- **Output Method:** `Add new layer`

Kaydet: `ART/dusmanlar/elite_wraith/wraith_S_BASE.png`

#### ADIM 3 — Animasyon
```
python tools/rima_generate.py --only convergence_wraith --anim-only
```
| Animasyon | Frame | FPS | Not |
|-----------|-------|-----|-----|
| idle | 4 | 4 | altın çatlaklar nefes alıyor |
| walk | 8 | 7 | asimetrik yürüyüş |
| attack1 | 6 | 10 | taş kol saldırısı |
| attack2 | 6 | 10 | metal kol saldırısı |
| death | 10 | 6 | parçalar ayrılarak dağılıyor |

---

## SORUN GİDERME

**ComfyUI script hata verdi:**
```
python -c "import torch; print(torch.cuda.is_available())"
```
False çıkarsa: AMD iGPU'yu Aygıt Yöneticisi'nden devre dışı bırak.

**Animasyon sonuçları beğenilmedi:**
```
rima_generate.py içinde strength=0.60 → düşür (0.50) veya artır (0.70)
Dosyayı sil → scripti tekrar çalıştır (mevcut dosyaları atlar)
```

**PixelLab top-down vermiyor:**
```
Camera View: "Low top-down" seçili mi?
Init Image Strength yükselt: 500 → 650
Gemini görselini yeniden üret (aynı chat: "Now show from directly above...")
```

**AI iki elde silah koyuyor:**
```
Negative'e ekle: dual wield, two weapons, weapon in both hands, sword in left hand
Description'a ekle: right hand holds [X] only, left hand completely empty hanging at side
```

**32×32 mob pikseller çamur çıkıyor:**
```
M-XL değil S-M kullandığından emin ol.
S-M seçiliyse Strength'i düşür: 500 → 400
```

**Mob spawner sprite'ta yavru mob çok belirgin çıkıyor:**
```
Negative'e ekle: separate individual creatures, independent enemies, standalone monsters
Description'da "faint silhouette" veya "barely visible" kullan
```
