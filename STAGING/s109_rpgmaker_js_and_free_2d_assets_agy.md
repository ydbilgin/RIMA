ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

İki soruya cevap ver — RIMA'ya bağlı pragmatik araştırma.

## RIMA bağlam (kısa)

- Unity 2022.3+, 2D top-down roguelite, Hades / Children of Morta / Diablo III referans
- Sprite spec (LOCKED): karakter 120×120 PPU 64, tile 32×32 top-down, VFX 64-128px mix, 8 yön (5 sprite + 3 flipX mirror), 10-12 fps anim
- View: HIGH TOP-DOWN 3/4 (~70-80° kamera), URP 2D Renderer + 2D Lights
- Aktif sistemler: cliff face (KitB), parallax bg (KitC 7 layer), portal/yarık (cyan rune), Tilemap iso/rectangular karışık

## Soru 1 — RPG Maker JS nedir?

Önceki review'inde geçti: "RPG Maker MV/MZ için JS plugin". Kısa anlatım (200 kelime max):
- RPG Maker MV vs MZ vs XP vs VX Ace farkı
- "JS plugin" ne demek (engine architecture — Pixi.js mi, NW.js mi, custom mı)
- Map auto-decoration plugin'leri nasıl çalışıyor (autotile sistemi + scatter)
- **Çalınabilir konseptler:** RPG Maker tilemap autotile mantığını Unity'ye port etmek için hangi pattern'ler işe yarar?

## Soru 2 — RIMA için ücretsiz hazır 2D asset araştır

User mevcut PixelLab production pipeline'a paralel **ücretsiz fill-in** asset arıyor. Spec'e uyan ve ticari kullanım izni veren kaynakları topla.

**İstenen asset kategorileri (öncelik sırasıyla):**
1. **Top-down floor tiles** (Hades cobblestone / cyan rune / dirt — 32×32 veya 64×64, PPU 64)
2. **Cliff faces / drop edges** (sarkıt sprite, 64×96 - 128×192 arası, top-pivot)
3. **Parallax bg layers** (gothic dungeon, cyan rift, columns, far temple — 1920×1080 veya tileable)
4. **Props** (brazier, pillar, altar, banner, torch, portal/rift)
5. **Effects** (cyan glow, fire particle, dust, blood splat — 64-128px)
6. **UI** (skill card frame, HP/MP bar, fonts)
7. **SFX** (sword hit, footstep, magic cast) — ücretsizse ekle

**Kaynak listesi araştır:**
- itch.io free pixel art packs (search query'leri öner)
- OpenGameArt (en güvenilir + lisans temiz)
- Kenney.nl (genelde top-down değil ama bazıları uygun)
- CraftPix free section
- Unity Asset Store free tier
- LimeZu / Cainos / Penzilla / 0x72 / Pixel Frog / Buch / Calciumtrice (bilinen pixel artist'lerin free pack'leri)

**Her kaynak için raporla:**
- İsim + URL + lisans (CC0 / CC-BY / commercial-OK?)
- Bulunduğu asset kategorisi
- RIMA'ya uyum (PPU 64 + top-down 3/4 perspective + Hades estetiği) — 1-5 skor
- Pratik notlar (örn "PPU farklı, rescale gerek", "perspective 90° pure top-down, 3/4'e uymaz")

## Çıktı

Markdown, max 600 kelime toplam.
- Soru 1: 1 başlık, 200 kelime max
- Soru 2: tablo formatında — Kaynak | URL | Lisans | Kategoriler | Uyum skoru | Not — 8-15 satır
- Sonda **TOP 3 IMMEDIATE RECOMMENDATION** (hangi 3 pack'i bugün indirip kullanmaya başlayabilir)
