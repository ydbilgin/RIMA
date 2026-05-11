# Pre-rendered 2D — RIMA Degerlendirme
Date: 2026-05-11
Status: DECISION REJECTED (B, C, D for v1) — (A) PixelLab pixel art LOCKED kalir

## Soru
Hades / Diablo 2 / PoE1 / Bastion tarzinda **pre-rendered 2D** (Blender'da 3D model + rig + ~35 derece izometrik kameradan render -> 2D sprite) pipeline'i RIMA'ya alinmali mi? Dort secenek:

- (A) PixelLab pixel art (mevcut, LOCKED)
- (B) Tam gecis: Blender pre-rendered
- (C) Hibrit: environment PixelLab, karakter Blender
- (D) Blender + pixel art shader (3D render ama cikti pixel art gorunur)

## Secenekler ve Trade-off Analizi

### (A) PixelLab pixel art -- MEVCUT
**Artilar**
- Pipeline calisiyor; anchor + metadata.json sistemi LOCKED ve test edildi
- Sifir 3D yetkinligi gerektirir; solo/kucuk takim icin uygundur
- MCP entegrasyonu hazir (`create_character`, `animate_character` Web App, vary, tile/object). RIMA'nin orchestrator modeliyle birebir oturuyor.
- "Pixel art ZORUNLU" locked kuraliyla uyumlu
- Asset/sprite basina marjinal maliyet dusuk; iterasyon hizli
- 4 sinifin anchor + visual identity work'u zaten yatirilmis (kayip = ay/ay+)

**Eksiler**
- Generation arasi varyans riski (ayni karakterin frame'leri arasinda subtle drift). Anchor + vary akisi bunu azaltiyor ama sifirlamiyor.
- Animasyon kalitesi rig-bazli interpolation'a gore dusuk (ozellikle dash/hit reaction)
- 8 yon x 7 anim x 4-8 frame x 4 sinif = ~900-1800 unique frame; tutarlilik insan denetler

### (B) Blender pre-rendered (tam gecis)
**Artilar**
- Hades benzeri gorsel zenginlik (volumetrik, isiklandirma, kiyafet salinimli)
- **Mukemmel tutarlilik** -- rig sabit, frame drift sifir
- Yeni anim eklemek = yeni keyframe set, model degismez
- Yeniden render -> cozunurluk/aci degistirme ucuz

**Eksiler -- kritik**
- **Locked kurali dogrudan ihlal eder**: "pixel art ZORUNLU, painterly YASAK". Pre-rendered ciktisi painterly/volumetric -- RIMA gorsel kimligine aykiridur.
- **Yetkinlik acigi**: model + rig + skin + animasyon + lighting + render pipeline. Solo dev icin aylar-yil. Mevcut "Blender deneyimi belirtilmedi" durumunda v1 timeline'i coker.
- **MCP otomasyonu yok**: PixelLab MCP'nin yerini Blender'da tutacak headless render orchestrator yok; her seyi elle. Orchestrator pipeline'i (Claude -> Codex -> MCP) kirilir.
- **Sunk cost**: 4 sinif anchor + metadata + project_v1_character_visuals + animation_bible LOCKED kararlari cope gider.
- Disk/repo agirligi 3D asset + render output ile patlar
- 10 sinif total scope dusunuldigunde **v1 degil, oyun bitemez** riski

**Verdict**: REJECT.

### (C) Hibrit (environment PixelLab, karakter Blender)
**Artilar**
- Tile pipeline (LOCKED chromakey + binary alpha + variety + domain warp) bozulmaz
- Sadece karakter icin 3D maliyeti

**Eksiler -- kritik**
- **Stil catismasi**: Pixel art tile + pre-rendered painterly karakter = "PS1 karakter, NES tile" hissi. Hades bunu yapmiyor -- orada **her sey** pre-rendered (zemin, mob, karakter, prop). Hibrit RIMA'da gorsel disonans yaratir.
- Locked kural ihlali (B ile ayni)
- 3D yetkinlik acigi (B ile ayni)
- Camera scale rule (project_camera_scale_zoom_rules) -- karakter ~2 tile boyu LOCKED; iki farkli stil bu orani gorsel olarak yanlis gosterir

**Verdict**: REJECT.

### (D) Blender + pixel art shader (3D render -> pixelize)
Bu, "downscale + posterize + palette quantize + dithering" geciren render boru hatidir (Eastward, Octopath benzeri ama daha agresif pixel cap).

**Artilar**
- Locked "pixel art" kuralini teknik olarak **ihlal etmez** -- cikti pixel art gorunur
- Tutarlilik avantaji (B'deki gibi)
- 8 yon cekmek tek tikla -- kamera acisi degistir, render et

**Eksiler**
- Tum (B) eksileri **3D yetkinlik tarafinda gecerli**: model + rig + skin + anim + shader tuning. Pixel shader eklemek aylar-yil skill'i kisaltmaz.
- Pixel art shader tuning **ayri bir uzmanliктir**; yanlis ayar = "lo-res 3D" hissi, pixel art degil
- PixelLab MCP'nin yerini doldurmaz -- MCP otomasyonu kirilir
- Anchor sistemi anlamini yitirir; yeniden tasarim
- Marjinal kazanc (D'nin tutarlilik) marjinal maliyetten (3D yetkinlik + pipeline yeniden insa + sunk cost) kucuk, **v1 timeline'inda**

**Verdict**: REJECT for v1. Post-v1 R&D track olarak degerlendirilebilir, **sadece** PixelLab tutarlilik scale'de bozulursa.

## Kritik Kisitlar

1. **Takim olcegi**: Solo/kucuk. 3D rig + render uzmanligi yok.
2. **Locked gorsel kimlik**: "pixel art ZORUNLU, painterly YASAK" -- (B) ve (C) dogrudan ihlal.
3. **Yatirilmis is**: 4 sinifin anchor + metadata.json + project_v1_character_visuals (MCP gorsel analiz tablosu) + animation_bible + pixellab_master_pipeline -- hepsi LOCKED, hepsi gecerli kalmali.
4. **MCP orkestrasyon mimarisi**: Claude orchestrator -> PixelLab MCP/Web App akisi calisiyor. Blender headless render bu mimariye oturmaz; ozel Codex pipeline yazilmasi gerekir -> ay+ kaybi.
5. **V1 sprint kapsamı**: ~179 sprite minimum, 7 anim x 4 sinif x 8 yon. PixelLab Web App pipeline bunu haftalarla oelcer; Blender ogrenme + rig + render aylarla.
6. **Camera/scale LOCKED**: project_camera_scale_zoom_rules -- karakter ~2 tile, 32 PPU. Pre-rendered'da bu orani korumak icin ayri shader/PPU disiplini gerekir.
7. **Animation Bible LOCKED**: 4 yon + mirror, 7 oyuncu anim, mob 4/5/8 anim. PixelLab Web App akisi bu spec'e oturuyor.

## Karar

**STATUS: (A) PixelLab pixel art -- LOCKED kalir. (B), (C), (D) v1 icin REJECTED.**

(D) tek istisna olarak **post-v1 R&D track** olarak acik tutulur -- sadece su tetiklenirse:
- PixelLab anchor + vary tutarlilik 4 sinif x 7 anim'de gozle gorulebilir drift veriyorsa, VE
- v1 shipped + production hatti stabil + zaman/butce mevcutsa, VE
- Takima 3D animator katilirsa.

Bu uc kosul birlikte saglanmadicca (D) acilmaz.

## Gerekcе (Opus analizi)

**Hades modeli RIMA'ya bire bir uymaz, cunki:**
- Supergiant'ta birden fazla profesyonel 3D animatör + concept artist + tech artist var (Jen Zee + ekip). Bastion'dan beri pre-rendered pipeline kaslarini gelistirmisler. Hibrit yapmiyorlar -- **her sey** pre-rendered, cunki insan gucu var.
- RIMA solo/kucuk takim. Pre-rendered 2D'nin **gizli maliyeti** asset uretiminde degil, **pipeline bakiminda**: rig kirilinca, lighting tutarsizlasinca, yeni anim eklenince -- hep teknik artist mudahalesi gerekir. Bu maliyet aylar surer ve urun gelistirmeyi durdurur.
- Hades **pixel art degil**. RIMA'nin gorsel kimligi LOCKED olarak pixel art. Bu iki urun ayni sepette degil. Hades'i model almak gorsel hedef secimini degistirmeyi gerektirir; bu da gorsel kimligin yeniden karari demek.

**PixelLab'in zayif noktasi ne, ve katlanilabilir mi:**
- Generation drift: gercek ve olculebilir. Anchor + vary + manual QC ile yonetiliyor. 4 sinif v1 olceginde katlanilabilir.
- Animation cinematic kalitesi: rig-based pre-render kadar yuksek degil. Ama RIMA roguelite -- read-ability > cinematic. 8 yon net silüet + okunakli attack tell yeter; bunu PixelLab veriyor.

**(D) niye v1'de bile cazip degil:**
- "Pixel art gorunur" ciktisi, "pixel art dogar" ciktisından farklidir. Yan yana koyulduğunda playtester'lar farki sezer (sub-pixel motion, anti-alias artifacts, light bleed). Locked kuralin **ruhu** "low-res, hand-pixel, palette-disciplined". (D) bunu shader'la taklit eder ama tam tutturmak icin aylarca tuning ister.
- Ve (D)'nin tum 3D maliyetleri (B) ile ayni. Ciktıyi pixel art'a benzetmek icin 3D is yukunu oduyorsun; o yuku odeyebiliyorsan zaten PixelLab'i gecmis olman gerekirdi.

**Gercek risk asimetrisi:**
- (A) yanlissa -> kalitede tavana ulasamayziz; ama oyun cikar.
- (B)/(C)/(D) yanlissa -> v1 timeline coker, sunk cost cift katlanir, urun cikmayabilir.

Solo/kucuk takimda **timeline riski > kalite tavan riski**. (A) tek rasyonel secim.

## Uygulama Kosullari
Degisiklik YOK. Mevcut pipeline aynen yurur:
- `pixellab_master_pipeline.md` LOCKED kanonik kalir
- `project_pixellab_mcp_vs_manual.md` MCP/Web App ayrimi gecerli
- `animate_character` MCP YASAK kalir (4-frame + VFX bug)
- Karakter anim Web App'tan, tile/prop/object MCP'den

## Baglantili Kararlar
- LOCKED: `feedback_concept_art_pixel_art_only` -- pixel art ZORUNLU
- LOCKED: `pixellab_master_pipeline` -- tool/boyut/varyasyon kanonik
- LOCKED: `project_animation_bible` -- 7 oyuncu anim, 4 yon + mirror
- LOCKED: `project_v1_character_visuals` -- 4 sinif anchor + metadata
- LOCKED: `project_pixellab_mcp_vs_manual` -- MCP vs Web App ayrimi
- LOCKED: `feedback_animate_character_mcp` -- animate_character YASAK
- LOCKED: `project_camera_scale_zoom_rules` -- ~2 tile karakter, 32 PPU
- LOCKED: `project_skill_system_v2_locked` -- anim talepleri bu skill listesine bagli

## Re-evaluation Trigger (post-v1 only)
(D) acilirsa su uc kosul birlikte gerceklesmeli:
1. PixelLab drift'i olculebilir hale geldi (ornek frame kareleri kanit olarak)
2. v1 shipped + production hatti stabil
3. 3D animator takima katildi veya outsource butcesi onaylandi

Aksi takdirde RIMA pre-rendered 2D pipeline'ina gecmez.
