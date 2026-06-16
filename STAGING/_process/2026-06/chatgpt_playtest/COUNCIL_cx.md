# COUNCIL_cx - PixelLab feasibility / pipeline lens

VERDICT demo PixelLab listesi:
- P0 Warblade anim redo: demo-oncesi; player read/feel icin UI'dan daha kritik. PixelLab: Create Image Pro master sheet -> Create Character -> Custom Anim V3, 120x120 canvas, 8-dir canon, transparent.
- P1 RewardPickup skin polish: mevcut `RewardPickup.prefab` + `RewardPickup.cs` ustune sadece sprite/prompt gorseli; yeni ekonomi yok. PixelLab: Create Image Pro, 96x96 veya 128x128, transparent, shell/glow/ring ayri.
- P1 Minimal UI chrome refresh: mevcut `Assets/Sprites/UI/Chrome/UI_Chrome.spriteatlas` zaten tooltip/reward_card/slot/menu_button/minimap_frame tasiyor; sadece eksik/weak parcalar uretilsin. PixelLab: Create Image Pro sheet, 1024 atlas source, transparent.
- P1 Reward card base + combo box + selected glow: Edit-to-Play videosunda skill draft algisini guclendirir. PixelLab: Create Image Pro, card 384x576, combo 320x84, glow 256x256 alpha.
- P1 HUD slot/bar readability pass: slot_lmb_rmb/slot_normal reuse edilerek sadece vitality_frame ve overlay gerekirse. PixelLab: Create Image Pro, vitality 420x176, slots 96/76, transparent.

Post-demo listesi:
- Full 50 parcali modular UI pack: import, slice, prefab state, 1080/1440/ultrawide testi 4 gunde riskli.
- Rift-Forged Egg incubation/pet economy: paket de playtest buglari cozulmeden yapma diyor.
- Full settings/codex/minimap kit: mevcut ekranlar var, demo golden path'e gore dusuk getiri.
- Hatch/reject anim setleri: 10-12 frame hatch + fragment cleanup post-demo.

DROP listesi:
- PRO mode: maliyet riski; V3/Create Image Pro yeterli.
- 4-cardinal karakter yon sistemi: RIMA 8-dir canon ile catisiyor.
- UI icine gomulu text/icon: TMP ve existing skill icons kullanilmali.
- Boss reward icin egg: paket bile boss odulunu ozel tut diyor.

Canon flag tablosu:
- 35-degree ARPG view -> risk var -> world prop icin "high top-down 3/4 RIMA view" diye promptla; karaktere uygulama.
- 4-cardinal/no flip -> catisma var -> karakterde 8-dir locked; UI/tek-yon prop etkilenmez.
- Stil: blackened slate/dark iron/cyan/amber -> catisma yok -> purple neon overload yasak, cyan <=15%.
- UI scale 1080/1440/ultrawide -> risk var -> Canvas Scaler 1920x1080 + safe area/max-width test gerekir.
- Atlas stratejisi -> uygun -> Chrome/FX ayri kalmali; Point, mipmap off, compression none.
- Egg as economy -> risk var -> demo icin sadece mevcut reward presentation skin.

Pipeline maliyeti / reuse:
- Unity import maliyeti orta: PNG ayirma, alpha kontrol, Sprite 2D/UI, Point, mipmap off, borders, atlas pack, prefab state testi.
- 9-slice maliyeti yuksek degil ama tek tek dogrulama ister; 4 gunde yalniz P1 parcalara indir.
- Mevcut reuse: `UI_Chrome.spriteatlas` tooltip/reward_card/slots/menu_button/minimap_frame; skill icon seti; MapNodes atlas; `RewardPickup.prefab`; `RewardPickup.cs`.
- REWARD-02 ile cakisma: fix `RewardPickup.cs` trigger/input davranisinda; asset uretimi cakismasin. Prefab collider/trigger veya G input akisi degistirilmemeli.
- Egg skin mevcut `RewardPickup` sprite/prompt sunumuna baglanabilir; `RewardDefinition`/ChoiceSet yeni model demo-oncesi acilmamali.

Ekstra degerli:
- "Polish screen once, then cut parts" kuralini benimse: PNG mezarligini engeller.
- Combo box'un ayri ve sabit genislikli parca olmasi reward ekran okunurlugunu hizli iyilestirir.
- Chrome ve FX atlaslarini ayirma karari dogru; material/filter ihtiyaclari farkli.

Riskler:
- Full UI pack scope creep yaratir ve REWARD-02 fix odagini bozar.
- Transparent/alpha dogrulanmazsa Unity'de halo/edge artefact cikabilir.
- UI 9-slice parcalarinda perspektif egimi veya uneven border runtime'da kotu gerilir.
- Warblade anim redo ile UI polish ayni anda acilirsa 4 gun icin en yuksek risk context switch olur.
