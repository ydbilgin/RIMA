# WEAPON BATCH PLAN — RIMA (S114 S4 LOCK, 2026-05-29)

**Karar:** Silah başına 1 sprite, 8 yön KOD ile (OrientationSync rotation+flipY+sort). 8dir-bake YOK. PPU 64. Karakter = 64px (120px canvas'a bakma).

**⚠️ ÜRETİM = CLAUDE, MCP `create_1_direction_object` ile (KULLANICI YETKİ VERDİ S114 S4 — `feedback_pixellab_mcp_halt_strict` ban'ı BU GÖREV için kalktı). Sadece 3 batch.**

**🔧 ŞEMA KISITI (kritik):** `size` ve `style_images` BİRLİKTE verilemez. style_images verilince **en büyük style image çıktı boyutunu belirler** (her ref ≤256px, base64 PNG). Item sayısı boyuta bağlı: ≤42px→64 item (≤? refs), ≤85px→16 item (≤8 ref), ≤170→4, else 1.
**→ Style-ref'leri HEDEF boyutta hazırla:** Batch 2/3 (64px çıktı) için 64px ref; Batch 1 (32-40px) için ~40px ref.
**Style-ref stratejisi (hibrit):** (a) aynı boyutta MEVCUT WEAPON (weapon-shape stili, on-brand — örn. cyan greatsword 64px Batch 3 için) + (b) downscale edilmiş SINIF KARAKTERİ (sınıf rengi: Warblade cyan/Hexer violet). İkisini birden style_images[]'a koy (≤8). Karakter sprite'ı get_character ile çek → ≤256px base64.
**Finalize akışı:** create_1_direction_object → 'review' status → get_object (candidate'leri gör) → select_object_frames(indices) ile tut / dismiss_review ile at.

**Toplam 3 batch.** Bakiye 1208 gen, her batch ~20-40 gen → toplam ~60-120 gen.

## Sınıf → silah roster (10 sınıf)
| Sınıf | Silah | Tier | Mevcut asset (reuse) |
|---|---|---|---|
| Warblade | Greatsword (cyan rift) | Büyük | ✅ `31ee0f73` cyan greatsword |
| Ravager | Greataxe | Büyük | ❌ üret |
| Ronin | Katana | Orta | ✅ `a032d9b5` |
| Ranger | Compound bow | Orta | ✅ `ebc33ebf` |
| Hexer | Curse staff / Grimoire / Scepter | Orta | ✅ staff `4bde2642` |
| Elementalist | Staff / Orb | Orta | ❌ üret |
| Summoner | Tome / Orb | Orta | ❌ üret |
| Shadowblade | Reverse-grip dagger | Küçük | ✅ `9312ea86` |
| Gunslinger | Flintlock pistol | Küçük | ✅ `894bba4a` |
| Brawler | Gauntlet / fist | Küçük | ❌ üret |

## BATCH 1 — Küçük silahlar (32-40px canvas → 64-item tier)
- **Sınıflar:** Shadowblade (dagger), Gunslinger (pistol), Brawler (gauntlet).
- **İçerik (varyantlarla):** dagger ×8, pistol ×8, gauntlet ×8, küçük wand/throwing ×varyant — 64 slot'a kadar.
- **style_images:** Shadowblade + Gunslinger + Brawler karakterleri.
- ~20-40 gen.

## BATCH 2 — Orta 1H silahlar (64px canvas → 16-item tier)
- **Sınıflar:** Ronin (katana), Hexer (curse staff/scepter), Elementalist (staff/orb), Summoner (tome/orb), Ranger (bow), generic sword.
- **İçerik:** katana ×2-3, staff ×3, scepter ×2, orb ×2, tome ×2, bow ×2, sword ×2 ≈ 16.
- **style_images:** Ronin + Hexer + Elementalist + Ranger karakterleri.
- ~20-40 gen.

## BATCH 3 — Büyük 2H silahlar (64px canvas → 16-item tier)
- **Sınıflar:** Warblade (greatsword), Ravager (greataxe) + ağır varyantlar.
- **İçerik:** greatsword ×6 (Warblade rift varyantları), greataxe ×6 (Ravager), 2H hammer ×2, polearm ×2 ≈ 16.
- **style_images:** Warblade + Ravager karakterleri (+ cyan greatsword `31ee0f73` kalite anchor).
- ~20-40 gen.

## Üretim notları
- Mevcut 64px weapon'lar uygun → SADECE eksikler (Ravager greataxe, Elementalist staff, Summoner tome, Brawler gauntlet) + swap varyantları üretilir.
- Sapan/off-color tek silah → o sınıfın karakteriyle TEK BAŞINA yeniden üret (tightest match).
- Demo (Warblade) = cyan greatsword `31ee0f73` zaten yeterli, batch beklemez.
- **Claude üretir** (MCP, yetki verildi). Her batch: style-ref base64 hazırla → create_1_direction_object(description, item_descriptions[], style_images[]) → review → get_object → select_object_frames. Sonra Unity'ye import (PPU 64) + WeaponDatabase entry. 3 batch ile SINIRLI.
