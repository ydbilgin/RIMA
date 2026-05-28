ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

User talimatı (verbatim):
> "eski oyunlarda 2D ile bazı ilüzyonlar var rima için neler kullanılabilir. pixellabda nasıl kullanırsam basit iş ama ilüzyonla güzel görüntüler elde edebilirim bazı şeyler için bunu araştırsın agy"

İki birleşik sonuç istiyor:
1. **Eski oyunlardan 2D illüzyon tekniği katalogu** — RIMA estetiğine uyanlar (Hades / Children of Morta / Diablo III / Hyper Light Drifter / Eastward / Dead Cells / Death's Door tarzı 3/4 top-down)
2. **PixelLab production cookbook** — her teknik için "basit prompt + minimum efor → güzel illüzyon" formülü

## RIMA bağlam

- 2D top-down 3/4 (70-80°), URP 2D Renderer + 2D Lights + Pixel Perfect disabled
- Karakter 120×120 PPU 64, tile 32×32, parallax 7 layer (KitC) + cliff face (KitB)
- PixelLab erişim: Create Image Pro (S-XL Pro, max 1:1=512×512), Create Map Object (non-square WxH), Tiles Pro (4-type top-down), Edit Image (inpaint + init image AI Freedom 0.0-1.0)
- Aktif sahne: PlayableArena_Test01 (Hades Elysium estetiği — yüzen ada + cliff edges + cyan rune + portal/yarık)

## Araştırılacak illüzyon teknikleri (en az şu kategoriler — eksik bulduklarını ekle)

1. **Fake depth & parallax tricks** (Donkey Kong Country bg, FFVI airship, Sonic forced-perspective, Hyper Light Drifter scroll mismatch)
2. **Fake 3D objects / billboards** (Octopath Traveler HD-2D, Eastward water reflection, Stardew chimney smoke)
3. **Sprite scaling / pseudo-zoom** (Mode 7, Genesis distance scaling, RPG Maker auto-shadow)
4. **Lighting illusion** (sprite-baked highlight/shadow vs runtime 2D Lights, Diablo II torch dance, Hades fake bloom)
5. **Animated decals & loops** (water ripple, dust particles, rune pulse, banner sway — 2-4 frame trick'leri)
6. **Color cycling / palette tricks** (NES retro şelale, Loom, Another World, ZX Spectrum gradient)
7. **Perspective fake** (Zelda Link's Awakening 3/4 wall, Pokemon staircase, Diablo II isometric flatten)
8. **Reflection / glass illusion** (water mirror, mirror dungeon, glass orb refraction)
9. **Particle illusion** (god rays, dust mote, ember from torch, mist from rift)
10. **Composition illusion** (vignette, fog of war, edge fade, depth-of-field via sprite blur)

## Görev

Her teknik için şu formatta raporla:

```
### <Teknik adı>
- **Referans oyun:** (1-2 ünlü örnek)
- **Mekanizma:** (1-2 cümle nasıl çalışıyor görsel olarak)
- **RIMA uyumluluğu:** YES / PARTIAL / NO + sebep
- **PixelLab prompt formülü:** (kopya-yapıştır prompt, max 50 kelime, hangi tool — S-XL Pro / Map Object / Edit Image)
- **Efor:** XS (1 sprite, 5 dk) / S (sprite + Unity setup 30 dk) / M (multi-sprite + shader 2-4 saat) / L (full system 1 gün+)
```

## Çıktı

Markdown, max 1200 kelime. **TOP 5 IMMEDIATE WINS** sonda — bugün/yarın PixelLab'a girip 30 dakikada güzel illüzyon çıkarabilecek 5 teknik. Her biri için prompt + 1-2 cümle Unity setup notu.

Web search izinli. Vintage game tech sitelerini tarayabilirsin (NESdev, mode7.cc, retroachievements, polygon.com retro articles). RIMA'ya uymayanları SKIP olarak işaretle, listede tut ama yıldız koyma.
