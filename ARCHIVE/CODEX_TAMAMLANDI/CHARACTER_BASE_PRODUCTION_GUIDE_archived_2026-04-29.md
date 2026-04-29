# Character Base Production Guide
> Tüm 10 class için geçerli. Son güncelleme: 2026-04-23

## Neden Bu Pipeline?

Önceki sorun: idle (Create Character) + run (V3) → farklı engine, farklı canvas fill → karakter boyutu değişiyor.

Çözüm: Her iki animasyon da **aynı base sprite'ı first frame** olarak kullanır → scale tutarlı.

```
ChatGPT Image 2 concept → PixelLab Create Character (128×128) → [base sprite / scale anchor]
                                                                          ↓
                                                      V3 idle (Keep First Frame ON)
                                                      V3 run  (Keep First Frame ON)
```

> **BASE LOCK KURALI:** Animasyona geçmeden önce S yönü base sprite'ı kilitle (`[class]_base_S.png`).
> Bu dosya "golden source"dur — overwrite edilmez. Kalan 7 yön da overlay QC'den geçmeden animasyona girmez.

---

## Adım 1 — ChatGPT Image 2 Concept

ChatGPT Image 2 ile class başına bir concept üretiyorsun. Bu görsel PixelLab Create Character'a referans olacak.

### Base Prompt Template

```
pixel art character sprite, top-down ARPG gameplay camera, slightly tilted overhead around 60-degree downward view, full body visible head to feet, character fills about 60-65% of image height centered, neutral combat-ready stance, clean dark neutral background, no motion blur, sharp pixel art silhouette, [CLASS ADDITION]
```

### Class Additions (tabloya ekle)

| Class | Gender | Class Addition |
|-------|--------|----------------|
| Warblade | M | heavily armored male warrior, massive two-handed greatsword held in diagonal front guard with blade tip toward lower ground, heavy plate armor with dark metal finish, short dark brown hair |
| Shadowblade | M | lean male rogue in dark leather armor, dual daggers in ambidextrous low-ready (one blade forward low, one at mid-guard), crouched alert stance, hood up |
| Brawler | M | broad bare-chested male fighter, fists raised in boxer guard with faint void purple energy on knuckles, wide combat stance, aggressive forward lean, heavy scarred arms, tribal tattoos on shoulders and chest, minimal lower armor |
| Ravager | M | massive male berserker, large greataxe resting on one shoulder, wide brutal stance, fur-trimmed heavy armor, aggressive silhouette |
| Ronin | M | composed male wanderer-samurai, one katana drawn at low guard with cold silver-blue edge shimmer, second katana sheathed on back, torn dark grey-green battle robe with partial iron armor, no purple no fire — cold silver-blue accent only |
| Elementalist | F | female mage, tall staff planted at side with orb at tip, other hand slightly raised with faint magical energy trace, upright scholarly stance, flowing robes |
| Ranger | F | female archer, recurve bow held diagonally at side, arrow in draw hand ready but not drawn, slight forward lean, leather ranger armor, hood optional |
| Gunslinger | F | female gunslinger, revolver held in one raised hand at chest-height, other hand at hip, confident ready stance, long coat, wide-brim hat |
| Hexer | F | female curse mage, iron lantern held in one hand with cursed green-purple flame inside, dark wooden staff in other hand, floor-length dark crimson tattered robes, cursed green and void purple both present in design, decay tendrils at feet, pale gaunt face |
| Summoner | F | female necromancer, iron staff with cold blue fracture crystal at tip held forward, layered dark grey-black robes with bone-white trim, cold blue summoning circle at feet, commanding calm stance, no purple no green — cold blue only |

**Önemli:** Prompt'a oyun adı, "dark fantasy", "3/4 view", "80 degree", "isometric" YAZMA.

> **Prompt authority:** Bu Guide üretim için tek referanstır. STYLE_BIBLE'daki prompt metin şablonları (code block'lar) kamera kilidinden önce yazıldı — "isometric, 45 degree" içeriyorlar ve **deprecated**dır.
> - **Bu Guide'ı kullan:** kamera açısı, prompt template, canvas fill kuralı için
> - **STYLE_BIBLE'ı kullan:** silhouette, renk, silah, kimlik (ne üretileceği) için — prompt metnini değil, kimlik tanımını

---

## Adım 2 — PixelLab Create Character

ChatGPT Image 2 çıktısını referans olarak kullanıp 128×128 pixel art sprite üretiyorsun.

### Reference Image Selection (Claude reviewed 2026-04-23)

| Class | Use File | Location |
|-------|----------|----------|
| Warblade | `01_warblade.png` (OLD) | `C:/Users/ydbil/Downloads/chatgpt chars/` |
| Shadowblade | `shadowblade.png` (NEW) | `C:/Users/ydbil/Downloads/chatgpt chars/yeni/` |
| Brawler | `03_brawler.png` (OLD) | `C:/Users/ydbil/Downloads/chatgpt chars/` |
| Ravager | `04_ravager.png` (OLD) | `C:/Users/ydbil/Downloads/chatgpt chars/` |
| Ronin | `05_ronin.png` (OLD) | `C:/Users/ydbil/Downloads/chatgpt chars/` |
| Elementalist | `elementalist.png` (NEW) | `C:/Users/ydbil/Downloads/chatgpt chars/yeni/` |
| Ranger | `ranger.png` (NEW) | `C:/Users/ydbil/Downloads/chatgpt chars/yeni/` |
| Gunslinger | `gunslinger.png` (NEW) | `C:/Users/ydbil/Downloads/chatgpt chars/yeni/` |
| Hexer | `hexer.png` (NEW) | `C:/Users/ydbil/Downloads/chatgpt chars/yeni/` |
| Summoner | `10_summoner.png` (OLD) | `C:/Users/ydbil/Downloads/chatgpt chars/` |

**Ayarlar:**
- Canvas: `128 × 128`
- Reference image: ChatGPT Image 2'den gelen concept görsel
- Style reference: İlk class'tan sonra o class'ın çıktısını diğer classlar için stil anchor olarak ekle (cross-class tutarlılık)
- Prompt (Create Character'da): per-class prompt below — copy the exact single-line prompt for the class being produced.

| Class | PixelLab Create Character Prompt |
|-------|----------------------------------|
| Warblade | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, male mercenary warrior greatsword, dark grey cloth tunic clearly distinct from black leather armor pieces, cold blue uniform glow along full blade length no bright tip flare, short dark brown hair, scarred jaw` |
| Shadowblade | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, male rogue dual daggers hood, midnight blue leather vest clearly blue NOT brown NOT black, dark navy trousers, bone-white cross-harness straps, plain steel daggers NO purple NO glow on blades` |
| Brawler | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, male bare-chest fighter fists raised, burnt orange rust-brown leather trousers clearly warm orange NOT dark brown, void purple energy wrapping both fists and lower forearms, bare skin clearly distinct from trousers, tribal tattoos on shoulders and chest` |
| Ravager | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, male berserker dual axes fur mantle, warm dark brown fur mantle clearly distinct from sandy-tan leather trousers, bone necklace ivory white, bare chest, crude war paint on skin, NO purple NO blue accents anywhere` |
| Ronin | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, male wanderer samurai drawn katana, sage green kimono clearly saturated green NOT khaki NOT grey-green, torn weathered cloth edges, cold silver-blue edge shimmer on drawn blade only, grey iron chest plate clearly separate tone from green robe` |
| Elementalist | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, female mage staff amber orb, deep indigo blue-purple robes clearly colored NOT black NOT dark grey, warm amber golden glow on staff orb and raised hand arcane energy, warm honey-blonde hair in low bun` |
| Ranger | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, female archer recurve bow hooded, forest green leather vest clearly saturated green NOT brown-green, dark olive trousers distinctly separate darker tone from green vest, dark chestnut hair, leather bracers and knee guards warm brown` |
| Gunslinger | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, female gunslinger dual revolvers long coat, deep burgundy-crimson long coat clearly red-toned weathered NOT dark grey NOT black, deep auburn red hair prominent and loose, plain steel revolvers no energy glow, wide leather gun belt` |
| Hexer | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, female curse mage lantern staff crimson robes, dark crimson tattered robes clearly red NOT brown NOT black, sickly green-purple cursed flame inside iron lantern, pale sickly green decay tendrils at feet, pale gaunt skin` |
| Summoner | `full body centered, same scale as reference, no zoom-in, top-down ARPG view, female necromancer crystal staff bone robes, dark grey-black robes with prominent bone-white ribcage and spine pattern sewn into fabric, cold blue glow on crystal staff tip and small skeleton wisp spirit, NO purple NO green cold blue only` |

**Canvas fill QC:** Üretilen sprite'da karakter canvas yüksekliğinin %60-65'ini doldurmalı. Ayaklar görünür, başın üstünde biraz boşluk var. Daha küçük veya büyükse yeniden üret.

**Dosya adı:** `[class]_base_S.png` (büyük harf yön suffix zorunlu: `_S`, `_SE`, `_E`, `_NE`, `_N`, `_NW`, `_W`, `_SW`)

**Çıktı klasörü:** `Assets/Sprites/Characters/[Class]/base/[class]_base_S.png` — bu S direction için. Diğer 7 yön V3 ile üretilir.

**Direction Convention (SE/SW karıştırma!):**
| Yön | Anlamı | Blend Position |
|-----|--------|---------------|
| _S | kameraya bakıyor | (0, -1) |
| _SE | sağ+aşağı | (0.71, -0.71) |
| _E | sağa bakıyor | (1, 0) |
| _NE | sağ+yukarı | (0.71, 0.71) |
| _N | kameradan uzak | (0, 1) |
| _NW | sol+yukarı | (-0.71, 0.71) |
| _W | sola bakıyor | (-1, 0) |
| _SW | sol+aşağı | (-0.71, -0.71) |

---

## Adım 3 — Custom Animation V3: Idle

**Ayarlar:**
- Mode: Custom Animation V3
- Keep First Frame: **ON**
- Frame count: 4-6
- Canvas: 128×128
- Reference: base sprite (_S veya ilgili yön)

**Idle Prompt Template (tek satır):**

```
character standing in combat-ready guard, subtle breathing weight shift, torso slightly sways, feet planted, no stepping motion, weapon stable, pixel art sprite
```

**Class notları:**
- Ağır zırhlı (Warblade, Ravager): daha yavaş, minimal hareket — `slow heavy breathing, armor settles`
- Hafif/çevik (Shadowblade, Ranger, Gunslinger): biraz daha enerjik — `alert ready, slight weight shift`
- Büyücü (Elementalist, Hexer, Summoner): floating/energy trace ekle — `subtle magical energy trace, robes drift slightly`

**8 yön:** S direction'dan başla, çıktıyı baseline olarak kilitle. Kalan 7 yön için aynı ayarları kullan, sadece character reference'ı o yöne döndürülmüş versiyonuyla değiştir.

---

## Adım 4 — Custom Animation V3: Run

**Ayarlar:**
- Mode: Custom Animation V3
- Keep First Frame: **ON** (aynı base sprite → scale tutarlı)
- Frame count: 6-8
- Canvas: 128×128
- Reference: aynı base sprite (idle ile aynı — scale sabitleniyor)

**Run Prompt Template (tek satır):**

```
character running forward at full speed, body leaning into stride, legs in full run cycle, arms pumping, weapon carried stable, pixel art sprite
```

**Class weapon handling:**
| Class | Weapon during run |
|-------|------------------|
| Warblade | greatsword carried at right shoulder, both hands on hilt (two-handed grip preserved during run) |
| Shadowblade | daggers sheathed or held low, arms free |
| Brawler | fists loose at sides, full arm swing |
| Ravager | greataxe dragged or carried at side |
| Ronin | one katana drawn at low guard, second katana sheathed on back throughout run |
| Elementalist | staff held upright in one hand |
| Ranger | bow slung or held at side |
| Gunslinger | revolver holstered, full arm swing |
| Hexer | iron lantern carried in one hand, dark staff in other hand, both held during run |
| Summoner | crystal staff carried upright in one hand, cold blue crystal stable |

**8 yön:** S direction baseline → kalan 7 yön.

**Run Start / Run Stop (ağır silahlılar için):**
Idle → Run geçişte snap görünürse 2-3 frame'lik `run_start` klip üret (idle son frame → run ilk frame).
Run → Idle için `run_stop` (run son frame → idle ilk frame). Test et, snap yoksa bu klipler gerekmez.

---

## QC Checklist (Unity'e geçmeden önce)

1. **Aseprite overlay:** Aynı yön idle frame + run frame'i üst üste koy. Ayak noktası ve silhouette top hizalanmalı. Belirgin fark varsa → yeniden üret.
2. **Canvas fill:** Karakter %60-65 canvas height. Daha az veya fazla → yeniden üret.
3. **Yön doğruluğu:** _S = kameraya bakıyor, _E = sağa bakıyor, _N = kameradan uzak bakıyor.
4. **Şeffaflık:** Sprite kenarlarında blur/halo yok.

---

## Import (Unity)

Tüm sprite'lar için sabit:
- Sprite Mode: **Multiple**
- PPU: **64**
- Frame: **128×128** per cell
- Pivot: **Center (0.5, 0.5)**
- Filter: **Point (no filter)**
- Compression: **Uncompressed**
