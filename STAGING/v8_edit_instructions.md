# V8 Batch Edit Instructions — 3 sprite düzeltme

**V8 batch:** `Characters/idle_batch_v8/01_warblade.png` ... `16_rift_acolyte.png` (16 sprite, 64×64)
**Genel sonuç:** İLK TAM BAŞARILI BATCH (text yok, identity diversity, chibi proportions ✓)
**Düzeltme gerekenler:** 3 sprite

---

## EDIT #1 — Ravager (`05_ravager.png`)

**Sorun:** İki yumruk başında subtle kahverengi balta-kabza formları kalmış. Karar #123 Yol A weapon decouple = body silahsız olmalı; Unity'de WeaponDatabase'den dual hatchet attach edilecek.

**PixelLab Pro UI Edit Image yöntemi:**
1. PixelLab Pro UI > **Edit Image** mode
2. Yükle: `Characters/idle_batch_v8/05_ravager.png`
3. **Mask** tool: iki yumruğun başındaki kahverengi balta kabza-formlarını işaretle (her yumruğun üst kısmı)
4. Edit prompt:
   ```
   Replace selected area with plain clenched bare fist (clenched knuckles only, no weapon, no axe handle, no grip). Match the existing skin tone and pose. Aggressive predator hip-level clench. Karar #123 weapon-decouple: weapon will attach later in Unity.
   ```
5. Generate → kaydet `05_ravager_edited.png`
6. Karşılaştır: balta gone, pure fist görünür mü?

**Alternatif (eğer edit drift yaratırsa):** Tek sprite regenerate. V8 prompt'un 5. satırını biraz değiştir:
- Eski: "as if gripping invisible dual hatchets"
- Yeni: "knuckles bare and clenched in aggressive empty-handed grip, no weapon shape, plain visible fists"
- Pro UI tek-sprite regenerate ~1 credit

---

## EDIT #2 — Ronin (`06_ronin.png`)

**Sorun:** Sol hip'te empty sheath silüet **subtle / belirsiz**. Karar #71 + canon: Ronin'in sheath/draw kimliği KORUNMALI (Ronin'in tek istisna).

**PixelLab Pro UI Edit Image yöntemi:**
1. Edit Image > yükle `Characters/idle_batch_v8/06_ronin.png`
2. **Mask** tool: sol hip alanı (karakterin sol tarafı, hip yüksekliği, ~8×16 px alan)
3. Edit prompt:
   ```
   Add a plain black empty katana sheath silhouette at the left hip, vertical orientation, ~14 pixels tall × 4 pixels wide, pure dark black #0A0A12 outlined in 1px black. The sheath is EMPTY — no katana handle visible, no decorative grip. The character's hand position does not change. Top-down 35° view. Karar #71 sheath/draw identity preserved.
   ```
4. Generate → kaydet `06_ronin_edited.png`

**Alternatif:** Tek sprite regenerate. V8 prompt'un 6. satırına ekle:
- Eski: "a plain black empty sheath silhouette clearly visible at left hip"
- Yeni: "a plain black solid empty sheath silhouette CLEARLY visible at left hip, ~14 pixels tall vertical bar in pure dark black, must be readable at thumbnail size"
- Pro UI tek-sprite regenerate ~1 credit

---

## EDIT #3 — Shadowblade (`03_shadowblade.png`) — ❌ İPTAL

QC yeniden değerlendirme (2026-05-15): Shadowblade aslında **kel değil** — kafa üst kısmında koyu siyah saç mevcut, sadece koyu armor ile near-black palette'te kaynaştığı için ilk bakışta silüet kel gibi görünüyor. Canon LOCK ("short dark messy hair to eyebrows, clean-shaven") V8 batch'te tutturulmuş. Edit/regen GEREKMEZ.

---

## Üretim sonrası

1. Düzeltilmiş sprite'ları `Characters/idle_batch_v8/` altına override et (aynı isim) veya `Characters/idle_batch_v8/edited/` alt klasörüne kaydet
2. Final 16 sprite seti onaylandığında: her sprite'ı PixelLab MCP `create_character` çağrısına reference olarak ver
3. 8-direction sprite sheet üretimi → Unity'ye import → Karar #123 WeaponDatabase + HandAnchorAttach.cs zaten implemented (commit 3662ec6)

---

## Status

| Sprite | Mevcut | Edit/Regen önerisi | Credit tahmin |
|---|---|---|---|
| 05 Ravager | balta-form subtle | Edit Image mask + erase OR regen | 1-2 |
| 06 Ronin | sheath belirsiz | Edit Image mask + add sheath OR regen | 1-2 |
| ~~03 Shadowblade~~ | ~~kel~~ → koyu saç doğru | İPTAL (yanlış QC) | 0 |
| **Toplam** | | | **~2-4 credit** |

Diğer 14 sprite onaylı, weapon-attach hazır.
