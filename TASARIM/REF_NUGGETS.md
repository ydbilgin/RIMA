# RIMA — Polish & Inspiration Nuggets

> Konsolide ilham kaynaklari. Buyuk arastirma dosyalarindan damitilmis.
> Son guncelleme: 2026-05-13 (S66)
> Original kaynak dosyalar: STAGING/_archive/

## 1. Game Feel / Combat Juice (Alabaster Dawn referansi)

- **Blob shadow:** multiply blend %30-40 opacity, karakter Y offset → shadow alpha + scale lerp
- **Hit-stop:** hafif = 1 frame, agir = 2-4 frame, particle/trail unscaled time ile devam
- **Landing dust + squash/stretch:** 6-8 px fan-out 0.15s, scale Y 0.85 → 1.0 hizli donus
- **Damage numbers:** TMPro world-space, beyaz/sari/yesil/element renk kodu, 0.6-0.8s fade
- **Dash trail:** Trail Renderer time=0.08-0.1s additive VEYA 2-3 ghost sprite %30-50 opacity
- **Parallax 3-4 katman:** 0.1x / 0.3x / 0.6x / 1.0x, yatay + dikey
- **White health bar lag:** hasar → kirmizi anida, white bar ~0.5s gecikmeli (HK tarz)
- **Skill micro zoom-out:** scale 1.02x 0.1s impact

## 2. Dungeon Lighting Spec (DUNGEON_LIGHTING_GENERATION_RESEARCH)

- **Global Light 2D base:** #1E2030 to #262838, intensity 0.18-0.32
- **Torch:** #C8A96E intensity 0.75-1.10 outer 3.25-4.50 flicker ±0.08-0.16
- **Rift Crystal:** #00FFCC intensity 0.45-0.80 inner 0.50-0.90 pulse
- **Candle:** #E0C58D intensity 0.25-0.45 outer 1.20-2.10
- **Floor depth bands:** F1 torch fill %70-90 / F2 %40-65 (broken fixture ±%20-35) / F3 %10-30
- **Kural:** Her Point Light 2D'nin gorunur prop sahibi olmali. Floating light YASAK.
- **PropSpec alanlari:** emitsLight, lightSourceKind, requiresVisibleSource, depthBand weights
- **Anchor tag listesi:** WallLight_N/E/S/W, FloorLantern, RiftAccent, CandleCluster, NoLightZone, CriticalCombatClearance

## 3. PixelLab Pipeline Ek İpuçlari

- **vary_object seed-locked:** sabit seed + style_options outline+shading → mob varyant tutarliligi
- **128px boss 4-referans limiti:** anchor olarak en "saf" 64px asset sec, style_guidance_weight yuksek
- **create_map_object background_image:** prop uretiminde zemin tile referansi ver, stil tutarsizligi onler
- **Mid-stride seed recovery:** walk loop stiff gorunuyorsa neutral idle yerine mid-stride frame'den seed'le
- **Frankensprite teknigi:** AI animasyon + static kafa Aseprite paste (kafa-yon stabilizasyonu)
- **Edit Image Pro tile refinement:** AI tile her zaman plastik/flat → "muted, dark gritty palette, no gradients, heavy texture" ek prompt

## 4. Hero Siege / Hammerwatch / Cinderia (Referans Onayi)

- **64px chibi sweet spot:** Hero Siege oranlari (64px karakter / 32px tile) PixelLab icin ideal (Karar #100 kanitlandi)
- **Aesthetic match ranking:** HLD 9/10, Cinderia 9/10, Hero Siege 8/10
- **Karanlık ortam + parlak skill VFX:** glow embedding SPRITE icinde YASAK; Unity URP 2D Bloom + Particle engine-side
- **Zemin tile satürasyon:** karakterden her zaman daha dusuk satürasyon ve soguk ton — zemin goz yormamali
- **%60 padding bosluğu:** slash trail ve buyu VFX icin Unity icinde kullanilir, asla doldurmayin
