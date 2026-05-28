ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

LaurethStudio için **studio-level 2D illüzyon know-how dökümanı** üret. RIMA'ya bağlı kalma — LaurethStudio'nun gelecekteki tüm 2D oyunları (roguelite, cozy farm, 2D RPG, platformer, side-scroller, top-down adventure, retro arcade, casual incremental) için reusable knowledge library.

## LaurethStudio bağlam

- Master plan: çok-tür stüdyo (RIMA bir oyun)
- Hibrit yapı: 2D pixel + 3D low-poly potansiyel
- Coding angle: active incremental + procgen + tool-driven authoring
- Shared libraries: LaurethProc (procgen), PainterSuite (UPM package v0.4.0 LIVE)
- Engine: Unity 2022.3+ standart, URP / Built-in karışık (proje bazında)
- Asset pipeline: PixelLab (S-XL Pro / Map Object / Tiles Pro / Edit Image)

## Görev

Önceki dispatch (s109_2d_illusion_tricks_pixellab_agy.md) RIMA odaklıydı. Bunu **studio katalog formuna** genişlet. 30+ teknik hedefle.

### Kategoriler (en az şu 8, eksik bulduklarını ekle)

1. **Depth & perspective illusions** — parallax, forced-perspective, foreshortening, atmospheric perspective, vanishing point fake
2. **Lighting & shading illusions** — baked vs runtime, dithered shadow, palette light ramps, normal-map fake, godrays, bloom-of-the-poor
3. **Motion illusions** — sprite scaling pseudo-zoom, Mode 7, persistence-of-vision flicker, sub-pixel scroll, parallax mismatch, fake camera shake
4. **Volume & 3D illusions** — billboards, HD-2D, fake reflections, water plane, layered sprites, isometric flatten, dimetric squash
5. **Particle & atmosphere** — dust mote, ember, fog, mist, snow/rain, lens flare, god rays, sparkle pulse
6. **Time & rhythm illusions** — palette cycling, low-frame loops, animation easing, sprite ghost trail, ASMR-tier feedback (tile crackle)
7. **Composition & framing** — vignette, foreground DoF blur, edge fade, fog-of-war, viewport reveal, letterbox dynamic
8. **Engine/render trick illusions** — pixel-perfect snap, shader graph UV-offset, 9-slice fake stretch, sprite atlas swap animation, fake-3D layered scrolling

### Çıktı formatı

Markdown, **~1500-2000 kelime** (büyük katalog, derin değil — referans tablo).

Her teknik için **tek satır**:
```
| Teknik | Kategori | Klasik örnek (yıl) | Modern indie örnek | Tür uyumu | Efor | Tek-satır mekanizma |
```

**Tür uyumu sütunu:** hangi LaurethStudio oyun türlerine uygun (roguelite / cozy / RPG / platformer / arcade / incremental). YES/PARTIAL/NO not.

**Efor:** XS (1 sprite, 5 dk) / S (sprite+setup 30dk) / M (multi-sprite+shader 2-4 saat) / L (full system 1 gün+)

### Ek bölümler (tablonun altında)

1. **"Studio cookbook" — TOP 10 cross-game patterns** (LaurethStudio'da yapılacak HER oyunda işe yarayan 10 illüzyon, kısa neden)
2. **Pixel art-specific tricks** (PixelLab pipeline'ında prompt formülleri ile birlikte) — TOP 10
3. **Common mistakes** — yeni başlayanların yaptığı 5-7 illüzyon hatası (uniform parallax velocity, baked light + dynamic light overlap, vb.)
4. **Tooling önerileri** — PainterSuite v1.1+ seed olarak hangi illüzyon teknikleri otomatize edilebilir? (örn "Auto-DoF Foreground Layer Generator", "Parallax Profile Editor")

### Önemli — yapılma şekli

- **Tarihsel + modern karışık.** NES Zelda yanında Hyper Light Drifter, SNES Mode 7 yanında Octopath Traveler.
- **Western + Japon karışık.** Stardew Valley + Eastward, Hades + Sakuna of Rice and Ruin.
- **Indie + AAA karışım.** AAA referansları sadece teknik kanıt için (Diablo, Hades).
- **Türlere göre etiketle.** roguelite/cozy/RPG/platformer/arcade/incremental tag'leri kullan.
- **Genel know-how, RIMA spec'ine bağlanma.** PPU 64 / 120×120 karakter / Hades Elysium gibi RIMA-özel detayları geçme — LaurethStudio level.

## Çıktı sonu

**3 actionable seed for LaurethStudio v1.0 platformuna eklenecek tool/lib:**
- Hangi illüzyon teknik kategorileri PainterSuite v1.1+ veya LaurethProc içine kütüphane olarak eklenebilir?
- Her seed için: kategori + effort estimate + market diferansiyator (hangi rakip stüdyo bunu yapmıyor?)

Web search izinli — tilde game design articles, gamedeveloper.com (eski gamasutra), 80.lv, indiedb postmortem'ler, GDC talks dokümantasyonları.
