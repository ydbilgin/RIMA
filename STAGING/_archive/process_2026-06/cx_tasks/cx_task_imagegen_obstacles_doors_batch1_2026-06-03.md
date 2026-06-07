# CX TASK — $imagegen ÜRETİM: Obstacle/Door/Decor BATCH-1 (RIMA Shattered Keep)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / STAGING / memory.

## Amaç
RIMA izo odaları için canon-grounded engel/kapı/dekor pixel-art asset'lerini **kendi `$imagegen` skill'inle ÜRET**. Çıktı = temiz, transparan, doğru-boyutlu PNG'ler → `STAGING/imagegen/assets/`. UNITY YOK (üretim+keying only). Her asset için raw + keyed + clean üret (portal/reward akışı gibi). Sonucu CODEX_DONE.md'ye + her dosyayı `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`'ye logla.

## STİL KİLİDİ (TÜM asset'lerde aynı)
- **Gerçek ref'ler** (style/init reference olarak ver, $imagegen destekliyorsa; ek olarak metinle de tarif et):
  - Zemin/palet: `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png`
  - Taş stili: `Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png`
  - Karakter ölçek/palet: `Assets/Resources/Characters/Warblade/warblade_idle_SE.png`
- **Palet:** slate stone #3A3D42 base · cyan #00FFCC EMISSIVE (≤%15 alan) · warm orange #E89020 (SADECE mangal) · void purple #3A1A4A (chasm derinliği). Concept resimleri referans DEĞİL.
- **Ortak prompt kuyruğu (her prompt'a ekle):** `pixel art, true isometric 2:1 dimetric projection, clean outline, flat neutral lighting with ZERO baked shadows or glows, transparent background, limited palette slate stone #3A3D42 with emissive cyan #00FFCC accents only`
- **KURAL:** sprite'a gölge/cyan-glow BAKE ETME (oyunda Light2D var). Taş = nötr; cyan = emissive. Background TRANSPARAN.
- **⚠️ ÖLÇEK ÇIPASI (kritik, user net söyledi):** Warblade GÖRÜNÜR karakter ≈ **64px = 1.0 world unit @ PPU64** (canvas 120px ama görünür karakter 64px — büyük canvas animasyon içindir). TÜM prop'ları **64px karaktere** göre boyutlandır, 120px'e göre DEĞİL. Kapı ≈ 2× char (≤~144px), sütun ≈ 1.5× char (~96px). Aşağıdaki "Hedef" px'ler buna göre düzeltildi. world_size = px/64.

## ÜRETİLECEK 8 ASSET (+2 rün ikon) — kategori turları halinde

### Tur 1 — KAPI EŞİKLERİ (hibrit: çerçeve+rift gömük; pivot bottom-center; ~128 geniş × 144 yüksek = ~2× char)
1. **gate_north** — `North-facing straight fractured slate granite archway threshold, hollow center filled with a vertical swirling cyan #00FFCC energy rift, jagged stones floating in the cyan glow` + kuyruk. Hedef **128×144**.
2. **gate_west** — `West-facing angled (left) fractured slate granite archway threshold, hollow center with vertical cyan rift, floating cracked stones` + kuyruk. Hedef **128×144**. (Doğu = Unity'de flipX, üretme.)

### Tur 1b — RÜN İKONLARI (ayrı, küçük, flat, 32×32, center pivot, saf emissive cyan)
3. **rune_combat** — `flat front-facing floating UI rune icon, crossed swords, pure glowing cyan #00FFCC, transparent bg, no shadow`. 32×32.
4. **rune_elite** — aynı, `stylized skull`. 32×32.

### Tur 2 — TAŞ OBSTACLE (pivot bottom-center)
5. **obstacle_pillar** — `tall slate granite obelisk cracked vertically in half, faint cyan #00FFCC energy leaking from the fracture, top half slightly levitating` + kuyruk. Hedef **64×96** (~1.5× char).
6. **obstacle_wall_stub** — `low L-shaped broken slate stone barrier wall, sharp jagged edges, void-purple ash at the base, no cyan` + kuyruk. Hedef ~128×80.

### Tur 3 — ZEMİN / DECAL (pivot center, yere yatık)
7. **chasm_gap** — `top-down-ish floor decal of a jagged chasm gap in slate stone, deep void purple #3A1A4A darkness inside, thin cyan #00FFCC seal-crack along the broken lip` + kuyruk. Hedef ~192×128. **NOT: sadece GÖRSEL decal** (cx: dash-gap kontratı implement değil → gameplay-blocker değil, walkable-hole opsiyonel).
8. **floor_riftcrack** — `floor decal of a jagged branching crack in slate stone with intense EMISSIVE flat cyan #00FFCC energy seeping out, no stone shadow` + kuyruk. Hedef ~64×48.
9. **brazier** — `stone brazier on a slate pedestal containing a bright warm-orange #E89020 flame, the only warm light, stone drawn neutral (fire casts no baked shadow)` + kuyruk. Hedef ~64×80. pivot bottom-center.
10. **bones_marker** — `scattered humanoid bones and skull from a failed containment body resting on bare slate stone, bone-white and grey, no cyan` + kuyruk. Hedef ~64×48. pivot center.

## ÇIKTI KURALLARI
- Her asset: `STAGING/imagegen/assets/<name>_raw.png` (raw) + `_keyed.png` (chroma-key transparan) + `_clean.png` (varsa pixel_cleanup). Final = `<name>.png` (temiz+transparan).
- Boyut: $imagegen 1024² verirse, hedef px'e DOWNSCALE et (nearest/point) + transparan koru. Hedef px = yukarıdaki "Hedef".
- Tüm pencere/console flash'sız (background).
- `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`'ye her asset için satır ekle (isim, hedef px, pivot, kullanım).
- CODEX_DONE.md: üretilen dosya listesi + hangi ref kullanıldı + boyut + varsayım/sorun.

BLOCKED yaz: $imagegen ref-image desteklemiyorsa (o zaman sadece metin-stil + palet ile üret, ama BELİRT). İzo açı tutmazsa not düş.
