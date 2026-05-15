# Warblade — Single Pro UI Prompt (init image driven, minimal)

**Init image:** `Characters/anchors/warblade.png` (Pro UI Reference Image / init image olarak yükle)
**Canvas:** 64×64
**Mode:** Pro UI single character (variations = 1)
**Credit:** ~1-2

---

## Reference image description (Pro UI Reference Image field)

```
Match this character verbatim — same outfit, same identity, same pose. Only difference: remove the sword from the left hand entirely; the hand stays in the same gripping position but holds nothing.
```

---

## YAPIŞTIRMA — Main prompt

```
Dark-haired clean-shaven male warrior with short black hair and pale skin, wearing dark near-black plate armor with high collar and dark armored sleeves, a deep maroon-red cloth drape hanging from the left hip down to the thigh, dark trousers, plated boots. Calm composed low-guard stance, feet shoulder-width apart, weight even, body slightly angled forward.

Left hand held at hip-to-thigh height in a single-hand grip position as if gripping an invisible longsword pointed straight downward, fingers curled around empty air. Right hand relaxed at the right side. The sword itself is absent — no weapon visible, no blade outline, no ghostly weapon shape, no glow in the empty hand.

Negative prompt: text, words, letters, captions, labels, watermark, numbers, drawn weapon visible in hand, sword in hand, longsword in hand, katana in hand, blade visible, ghostly transparent weapon outline, weapon glow in empty hand.
```

---

## Workflow

1. Pro UI > **Single character mode** (variations = 1)
2. Reference Image: `Characters/anchors/warblade.png`
3. Reference description'a yukarıdaki metni yapıştır (silahı kaldır vurgusu)
4. Main prompt yapıştır
5. Canvas: **64×64**
6. Generate (~1 credit)
7. Sol el silahsız ama grip pozisyonunda mı kontrol et
8. Hoşa giderse → `Characters/idle_batch_v8/01_warblade.png` overwrite (eskisini `_archive/v8_initial/` altına taşı)

---

## Önemli notlar

- **Reference image identity'i tam taşıyacak** — V8'in 16-batch identity drift sorunu burada yok çünkü single character + warblade.png direct match
- Prompt **chibi/style/proportions yazmıyor** — init image zaten o detayları taşıyor
- Tek hedef: **silahı sil, kalanı koru**
- Sağ el zaten anchor'da boş — değişmiyor
- Sol el grip pozisyonu **korunuyor**, sadece silah görünmüyor (Karar #123 Yol A için kritik)

Hoşa gidene kadar 1-2 iterasyon yeterli olmalı (init image güçlü).
