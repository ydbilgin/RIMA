---
name: 8dir-mirror-production-strategy
description: "8 yön karakter sprite üretim — 5 sprite üret + 3 mirror, silahlar Unity'de attach. S86 LOCK."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# 8-Direction Mirror Strategy — S86 LOCK (2026-05-16)

**Rule:** RIMA karakter sprite'ları **8 yön LOCKED** (S, SE, E, NE, N, NW, W, SW). Production: **5 yön sprite üret** (S, N, E, SE, NE), **3 yön Unity'de mirror** (W←E, SW←SE, NW←NE).

**Why:** 8 yön Diablo/Hades benzeri immersive movement, ama 8 ayrı sprite üretmek %38 fazla maliyet. Sprite simetrisi exploit'i + Unity SpriteRenderer.flipX = aynı kalite, %38 düşük üretim. Silahlar ayrı sprite/prefab olduğundan body mirror işi bozmaz, child weapon da otomatik mirror olur.

**How to apply:**

## Production
- PixelLab Create Image Pro'da her karakter için **sadece 5 sprite üret**: S, N, E, SE, NE
- Boyut: 256×256 (single direction body)
- View: Low top-down 25-40°
- Style image: `rima_style_anchor.png` veya class anchor

## Unity Setup
- 5 sprite asset import: `{class}_S.png`, `{class}_N.png`, `{class}_E.png`, `{class}_SE.png`, `{class}_NE.png`
- 3 sprite **YOK** asset olarak — runtime'da `SpriteRenderer.flipX = true` ile mirror:
  - W movement → E sprite + flipX
  - SW movement → SE sprite + flipX
  - NW movement → NE sprite + flipX
- DirectionResolver script: movement vector → which sprite + flipX bool

## Weapon System (LOCK)
- Karakterler **silahsız** üretilir (body only)
- Silahlar ayrı sprite/prefab, child SpriteRenderer olarak attach edilir
- Pattern: Mevcut Warblade Greatsword örneği takip edilir
  - `Assets/Resources/Weapons/{Class}_{WeaponName}.png`
  - `Assets/Prefabs/Weapons/{Class}_{WeaponName}.prefab`
- Body mirror edilince child weapon transform da otomatik mirror (Unity inheritance)
- Avantaj: Aynı body sprite × farklı silahlar = unlimited weapon variants

## Asymmetric Features = YASAK (Body Sprite'ta)
Mirror düzgün çalışsın diye karakter body sprite'ında **simetrik** olmalı:
- ❌ Eyepatch (sadece tek gözde) → eyepatch ayrı prop attach
- ❌ Asymmetric armor → ayrı pauldron attach
- ❌ Tek tarafta yara izi → scar overlay sprite
- ❌ Tek tarafta belt buckle → asymmetric prop attach
- ✅ Body sprite simetrik

**Asymmetric "imza" özellikler = SİLAH veya ACCESSORY üzerinden** (ayrı child sprite olarak attach, kendi flip mantığıyla).

## Code Hint (Sprint 9 retrofit'e eklenmeli)

```csharp
// Direction enum: 8 değer
public enum Direction8 { S, SE, E, NE, N, NW, W, SW }

// Sprite resolution
public class CharacterSpriteResolver : MonoBehaviour {
    [SerializeField] Sprite spriteS, spriteN, spriteE, spriteSE, spriteNE;
    SpriteRenderer sr;

    public void SetDirection(Direction8 dir) {
        switch (dir) {
            case Direction8.S:  sr.sprite = spriteS;  sr.flipX = false; break;
            case Direction8.N:  sr.sprite = spriteN;  sr.flipX = false; break;
            case Direction8.E:  sr.sprite = spriteE;  sr.flipX = false; break;
            case Direction8.W:  sr.sprite = spriteE;  sr.flipX = true;  break;  // mirror
            case Direction8.SE: sr.sprite = spriteSE; sr.flipX = false; break;
            case Direction8.SW: sr.sprite = spriteSE; sr.flipX = true;  break;  // mirror
            case Direction8.NE: sr.sprite = spriteNE; sr.flipX = false; break;
            case Direction8.NW: sr.sprite = spriteNE; sr.flipX = true;  break;  // mirror
        }
    }
}
```

## Production Cost (S86 LOCK)
- 9 karakter regen × 5 sprite = **45 sprite üretim** (PixelLab Create Image Pro)
- Gunslinger zaten 8-yön var — KEEP
- Batch'leme: 1-2 karakter / batch, ~9-10 sprite per batch
- Sprint 10 stable olduktan sonra başla (V1 odak engellenmesin)

# See Also
- [[camera-angle-revisit-s43]] — 30-35° angle + 8-dir LOCK
- [[character-system]] — Body+Weapon separate (eski 4-yön Aseprite composite, yeni 8-yön Unity composite)
- [[pixellab-character-via-web-ui-v3]] — character üretim web UI manuel
- [[character-visual-identity]] — Symmetric design rules
- [[asset-map]] — Asset path conventions
