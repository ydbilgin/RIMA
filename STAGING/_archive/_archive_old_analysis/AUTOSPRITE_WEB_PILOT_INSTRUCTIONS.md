# Autosprite Web UI Pilot — Manuel Free Plan Test (3 export/gün limit)

## Hesap kullanım planı
- Free plan: 15 starter credit + 3 export/gün
- MCP generation paid-locked → web UI manuel kullan
- Hedef: 2 VFX pilot generate et, RIMA pipeline'a uygun mu A/B değerlendir

## Pilot 1 — Dash Trail (PixelLab ile A/B karşılaştırma)

**Adım 1 — autosprite.io aç + giriş yap** (token zaten LIVE)

**Adım 2 — Create Asset → Effect**
- Category: `effect`
- Style: `pixel`
- Quality: `ultra` (1 credit) veya `turbo` (1 credit, 4 draft seç)
- Description:
```
Cold blue dash trail VFX, glowing energy smear, motion blur side profile, transparent bg, painterly pixel art, icy particles, radial fade edges, no character
```
- Frame size: 64×64

**Adım 3 — Animate**
- Animation prompt:
```
energy trail swirling forward then dissipating into icy particles, looping seamless motion smear
```
- Frame count: 8
- Quality: `standard` (5 credit) veya `legendary` (daha yüksek, seamless loop için)
- Background removal: `ultra`
- Looping: `true`

**Adım 4 — Export**
- Format: PNG + Atlas
- Save'i Downloads'tan al → `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/vfx_pilot_output/autosprite_dash_trail/`
- 1/3 günlük export quota'sı kullanıldı

## Pilot 2 — Hitspark

**Yarın yap** (export quota gün başına 3, bugün dash + hitspark yaparsan 2/3, bir slot kalır).

**Veya hemen** (2/3 quota):
- Category: `effect`
- Style: `pixel`
- Description:
```
Hitspark VFX burst, white-hot core cyan edge fade, radial impact sparks, contact flash, transparent bg, painterly pixel art, abstract energy burst
```
- Frame size: 64×64
- Animation prompt:
```
sudden radial burst from center, sparks fly outward then dim, single-shot no loop
```
- Frame count: 6
- Looping: `false`

## A/B değerlendirme kriterleri (autosprite vs PixelLab)

| Kriter | Skor (1-5) |
|---|---|
| Visual quality vs RIMA painterly mood | |
| Frame consistency (loop seamless mi?) | |
| Background transparency clean mi? | |
| Color accuracy (cold blue / class-color) | |
| Unity import speed (PNG slice + Animator setup) | |
| Cost (credit + time per output) | |
| Style match with character/prop pipeline | |

Toplam: autosprite vs PixelLab puan karşılaştırması → kazanan VFX provider LOCK.

## Sonuçları paylaş
PNG'leri `STAGING/vfx_pilot_output/autosprite_*` altına koy. Ben PixelLab versiyonu ile yan yana atıp Opus karar veririm:
- Autosprite kazandı → paid sub değer mi tartış
- PixelLab kazandı → autosprite skip, hibrit pipeline PixelLab+Codex
- Eşit → cost-driven karar (PixelLab budget zaten LIVE, kazanır)

## Unity Extension setup (export sonrası, opsiyonel)
1. autosprite.io/docs/integration-unity-extension takip et
2. .unitypackage indir veya UPM scoped registry ekle
3. Asset Store gibi Unity'e drag-drop
4. Auto-slice + Animator Controller setup yapar (free plan'da çalışır)

## Notlar
- 15 starter credit + 3 export/gün ile 5 günlük test mümkün
- Generation paid, ama Unity Extension import free
- Eğer pilot başarılı → paid sub trial ay (~$10-20) deneyim aylığı al
