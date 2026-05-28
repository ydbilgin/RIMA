# PIXELLAB KNOWLEDGE BASE — RIMA CANONICAL REFERENCE

**Tarih:** 2026-05-28 (S114) | **Kaynak:** workflow `pixellab-knowledge-base` (local-docs + mcp-schemas + youtube) + agy YouTube dispatch
**Durum:** LIVE — legal kaynaklardan sentez. Discord = legal kısıt (manuel küratörlük, aşağıda).

---

## ⚠️ ÇÖZÜLMEMİŞ ÇELİŞKİLER (locklamadan önce karar gerek)

1. ~~**Mirror yöntemi (KRİTİK):** Karar #144 "flipX YASAK" vs weapon-converged "5+3 flipX". (İlk çözüm "5+3 Unity flipX" idi — KOD DOĞRULAMASI ile DÜZELTİLDİ.)~~ **ÇÖZÜLDÜ 2026-05-28 (kod-doğrulamalı): body 8 BAKED directional sprite, runtime flipX YOK.** Kanıt: `PlayerAnimator.cs:103` `sr.flipX = false` zorlar + `DirX`/`DirY` Animator blend tree (satır 110-111) → her yön için gerçek sprite/clip gerekir. Mirror = **ÜRETİM kararı, runtime değil:** 8 sprite üretirken W/SW/NW'yi **PixelLab Mirror Horizontal** ile bake et (mevcut karakterlerde 8 yön zaten var). Karar #144 (flipX kaçın) doğru — kod zaten uyuyor. **Silah bağımsız:** `OrientationSync` 8 explicit handOffset+rotation+sort → mirror yönteminden tamamen decoupled, counter-flip GEREKMEZ.
2. ~~**G10 — interpolation_v2 canvas (KRİTİK):** MEMORY "252×252 destekli" vs MCP schema "max 128px".~~ **ÇÖZÜLDÜ 2026-05-28 (MCP schema doğrulaması):** v3 interpolation canvas **max = 256×256** (`animate_object.custom_start_frame_base64` schema: "the image's dimensions become the canvas ... subject to v3's 256x256 maximum"). Memory "252" ≈ 256 doğruydu, eski 64 limiti kalkmış. "128" karışıklığı = `create_character.size` param (max 128, ayrı şey) + pro-mode frame_count tier'ları (≤128→4/9/16). İki ayrı limit karışmıştı. **LOCK: interpolation=256 max, create_character size=128 max.**

---

## 1. TOOL MATRIX

| Tool | Platform | Batch Yeteneği | Maliyet (gen) | Ne Zaman |
|---|---|---|---|---|
| **create_1_direction_object** | MCP | **EN GÜÇLÜ BATCH**: size≤42→64 aday/call; ≤85→16; ≤170→4; `item_descriptions[]` her adaya ayrı prompt; `style_images[]` (≤85→8 ref) | 20-40 gen/call (tüm adaylar dahil) | Prop + **silah batch** — birincil cost-efficient yöntem |
| **create_tiles_pro** | MCP/Web | Numbered prompt tek çağrı N tile ("1). cyan rune 2). granite...") | 20-25 gen (style ref 20-40) | ISO floor tile batch; Wang16 garantisi YOK |
| **create_topdown_tileset** | MCP/Web | 16 tile (<1.0) / 23 tile (=1.0) tek çağrı; base_tile_id zincirleme | ~20-40 gen / ~100s | **GERÇEK Wang16** terrain |
| **create_image_pro (S-XL)** | Web/API | 4 ref + 1 style image, tek görsel | 20 gen | En yüksek kalite hero sprite; max **512×512** / **688×384** (768/1024 YOK) |
| **create_from_style_pro** | Web | 8 style ref, 64px 16-frame grid (tek prompt → 16 varyasyon) | 20 gen | Stil-eşleşen asset paketi |
| **animate_with_text v3** | MCP/Web | tek çağrı async | included | Karakter anim — **v2 YASAK** (49-50% stall) |
| **animate_with_text_pro** | Web | tek yön tek çağrı | 20-40 gen | Yüksek kalite; 32/64px→16f, 128/170/256px→4f |
| **animate_character (template/v3/pro)** | MCP | template 1 gen/yön; v3 custom; pro 20-40 gen/yön | değişken | RIMA karakter = Web App'e yönlendir |
| **animate_character MCP (full)** | MCP | — | — | **KALICI YASAK** (Karar #114) |
| **animation_to_animation** | Web/MCP | motion transfer (iskelet eşleşmeli) | ~4 run tasarruf | Warblade walk → diğer class |
| **interpolation_v2 (Pro)** | Web | start+end → ara kare; async | 20-40 gen | pose-to-pose tween (⚠️ G10 size çelişkisi) |
| **create_character_state** | MCP/Web | 8 yön varyant; palette kilidi opsiyonu | ~1 gen eşdeğeri+ | **kullanıcı onayı ZORUNLU**; iterasyon için PAHALI |
| **create_character (std/v3/pro)** | MCP | 4/8 yön async | std 1 / v3 2-9 / pro 20-40 gen | kalite kademesi |
| **create_8_direction_object** | MCP | tek obje 8 açı; reference VEYA style image (mutually exclusive) | 20-40 gen | prop 8-dir |
| **generate_8_rotations_v3** | MCP | 1 ref → 8 yön | included | hızlı yön (max 256² ref) |
| **create_map_object** | MCP | tek sprite; basic (w+h) / inpaint (bg image) | ~1 gen | tek prop; 8 saat sonra silinir |
| **inpaint_v3 / edit_image_pro** | Web/MCP | maske / tam-görsel edit | included / 20 gen | seam fix / nüanslı düzenleme |
| **remove_background / pixel_art_correction** | Web/MCP | — | included | alpha / artifact temizleme |
| **GET /balance** | API | — | — | kalan gen (Tier2 5000/ay, ~1208 kalan) |

**BATCH ÖZETİ:** `create_1_direction_object` size=32 → 64 aday/call = prop+silah motoru. `create_topdown_tileset` = 16-23 Wang tile/call.

---

## 2. STATE-ANCHORED ANİMASYON WORKFLOW (Karar #145 LOCK)

**Prensip:** Önce anchor STATE üret, sonra o state'ten animasyon türet.

```
1. BASE KARAKTER (silahsız, 5 yön S/SE/E/NE/N) — silah Unity child SR + HandAnchor
2. STATE OLUŞTUR (Web UI "Create State"): idle / midwalk / attack_anticipation / hit_recoil / death_start
3. ANİMASYON TÜRET: her state → "Add first animation" → Custom V3, first_frame=ON, enhance_prompt=ON
4. MIRROR: 5 üret, 3 mirror (SW=SE, W=E, NW=NE) — ⚠️ flipX vs PixelLab-mirror çelişkisi yukarıda
5. CLEANUP (Pixelorama): yüz drift fix, artifact sil; halüsinasyon silah = HARD FAIL
```

**Bütçe:** ~70-90 gen/sınıf (~25-30 state + ~40 anim). **Pixel budget:** w×h×frames ≤ 524,288 → 252² max 8f → attack windup 4f + follow 4f split.

**Brian's Extreme Pose (run/walk, N/S):** idle→12f run anim→extreme pose seç→flip→interpolate→cleanup. E/W güvenilmez.
**Kuzey fix:** "walking away from camera" (NOT "walking north" → ters yürür).

---

## 3. PROMPT GRAMMAR

**Yapı:** `[ASSET TYPE], [SUBJECT], [CAMERA VIEW], [STYLE], [PALETTE/MATERIALS], [SILHOUETTE], transparent background, crisp pixel art, no anti-aliasing`

**RIMA Style Lock (her promptta):** `dark gritty fantasy pixel art, matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, sparse cyan-violet rift accent, high top-down ARPG ~70-80°, transparent background`

**Negative:** `no text, no labels, no numbers, no watermarks, no logo, no UI, no frame, no blurry edges, no anti-aliasing, no smooth vector gradients`

**Kritik kurallar:**
- ASLA "Cell 1: NAME" yazma → baked text tetikler
- Direction/view kontrolü ZAYIF → prompt'ta yön kelimesi + init/reference image
- Inpaint: maskeli bölgeyi değil TÜM görünür alanı tanımla
- Attack anim: karakteri canvas kenarına koy → VFX/silah yayı için yer
- create_1_direction_object: `item_descriptions[]` her adaya ayrı (max = aday sayısı)
- **Avoid buzzwords:** "photorealistic/super detailed/beautiful" YASAK → teknik param ("1px outline, dithered shading, 16-bit palette")

**Init Image Strength (0-1000):** 0-300 sadece renk / 300-400 kaba şekil / 400-600 varyasyon / 600-900 küçük edit. Düşük=özgür, yüksek=referansa kilitli.

---

## 4. KANONİK KARARLAR

| Karar | İçerik | Durum |
|---|---|---|
| **#114** | animate_character MCP KALICI YASAK; 5+3 mirror | LOCK |
| **#120** | Karakter anim klipleri Web App zorunlu | LOCK |
| **#144** | Silahsız gövde + Unity child SR silah | LOCK |
| **#145** | State-anchored workflow, first_frame=ON; create_character_state iterasyon için PAHALI | LOCK |
| **5+3 Mirror** | S/SE/E/NE/N üret, SW/W/NW mirror | LOCK |
| **V3 Gen Cost** | 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen/dir | LIVE |
| **Canvas/PPU** | anim 252² max 8f; karakter 64² native PPU=64 | LOCK |
| **Weapon Size** | PPU 64, karakter **64px** (120px canvas'a bakma). 2 tier: **küçük (dagger/pistol/gauntlet/wand) 32-40px** / **orta+büyük (sword/katana/staff/bow/greatsword/greataxe/scepter/orb/tome) 64px**. Greatsword 64px=karakter boyu (hero blade). Tool=**create_object** (kare canvas OK, köşegen sığar). **style_images = o sınıfın KARAKTERİ** (palet+sınıf rengi taşır: Warblade cyan/Hexer violet); item_descriptions=per-silah renk/kimlik. create_image S-XL sadece hero-detay yetmezse. Mevcut 64px 1dir weapon'lar uygun; off-color olanı karakterle yeniden üret; 8dir-baked'ler YANLIŞ format (sil). | LOCK 2026-05-29 |
| **Obj batch (size→item)** | ≤42px→64 item(8×8) / ≤85px→16(4×4) / ≤170px→4(2×2) / >170→1. Küçük silahlar tek 64-item call'da ucuz. | LIVE |
| **S-XL Pro Max** | 512×512 / 688×384 (768/1024 YOK) | LOCK |
| **Wang16** | create_topdown_tileset kullan, create_tiles_pro DEĞİL | LOCK |
| **Async** | MCP araçları job_id döner, poll 5-10s | STANDARD |

---

## 5. DISCORD GAP — Legal Kısıt

- Discord pinned mesajlar + bot scrape = **ToS ihlali** (kullanıcı uyarı aldı). Otomatik çekme YOK.
- **Legal yol:** (1) staff'a help-support'ta sor (resmi bot/API/export var mı), (2) manuel küratörlük (kullanıcı okur→buraya not), (3) docs/changelog/YouTube (workflow bunu topluyor).
- **En değerli izlenmesi gereken video:** "Character States: The New Way to Animate Sprites" (`oCJWxfEwX-o`, ~2 hafta önce) — YouTube transcript erişilemedi, kullanıcı izleyip not ekleyecek.

---

## 6. AÇIK SORULAR (gaps)

| # | Soru | Öncelik |
|---|---|---|
| G2 | reference_pixellab_v3_gen_cost_by_frame.md var mı (MEMORY referans, glob bulamadı) | Yüksek |
| G4 | animate_character template preset ID tam listesi | Yüksek |
| G7 | Character States video (oCJWxfEwX-o) tam workflow | Yüksek |
| G10 | interpolation_v2 canvas 252 vs 128 çelişki | **Kritik** |
| G8 | mcp__autosprite__* (ayrı provider) RIMA rolü | Orta |

---

## HIZLI REFERANS — RIMA Üretim

```
Prop/silah batch    → create_1_direction_object size=32 (64 aday) + item_descriptions[] + style_images[]
ISO floor tile      → create_tiles_pro numbered, tile_type=isometric
Wang16 terrain      → create_topdown_tileset (create_tiles_pro DEĞİL)
Karakter anim       → Web App V3, first_frame=ON, enhance=ON, state-anchored
Silah               → Unity child SR + HandAnchor (PixelLab'de silahsız)
Mirror              → ⚠️ flipX vs PixelLab-mirror — kullanıcı kararı bekliyor
Attack 8f           → windup 4f + follow 4f (252² budget)
Walking dir         → "walking away from camera"
Hero sprite         → create_image_pro (512² max)
```

---

## agy YouTube korroborasyonu (Gemini dispatch, 2026-05-28)
agy bağımsız olarak doğruladı: Character States + Mid-Walk + First-Frame Lock + Enhance Prompt + Weapon Separation; Animate v3 / Interpolate / 8-Rotation / Map Workshop / Aseprite plugin. Ekleme: **"Style Reference Bulk Production — tek style-ref'ten düzinelerce obje toplu."** (Video kareleri birebir izlenmedi; başlık+web+API'den çıkarım — agy belirtti.)
