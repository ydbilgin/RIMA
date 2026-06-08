# 03 — Weapon Attachment Plan

## Ana kural

Silah sprite’ı karakter body’ye gömülmeyecek. Silahsız body + ayrı weapon object kullanılacak.

```text
PlayerRoot
  SortingGroup: Characters/Entities

  Body
    SpriteRenderer

  WeaponMountRoot
    WeaponSpriteRenderer

  VFXRoot
```

## Sorting

Öneri:
```text
Ground/Floor sorting layer < Props/Cliff < Characters < VFX < UI
```

Player ve silah aynı SortingGroup altında olmalı.

Warblade sword’un cliff arkasından görünmesi kabul edilemez. O bug, weapon’ın player’dan bağımsız veya yanlış layer/order’da kaldığını gösteriyor.

## Warblade weapon

Kanonik:
- Silah: iki elli greatsword.
- Unity final: yaklaşık 96×96.
- Sprite’ın ~%45’i kadar görsel ağırlık.
- Low-guard, uç yere yakın, yatay taşıma.
- Sırtta taşınmaz.
- Koyu çelik + pirinç/kahverengi kabza.

Minimum demo çözümü:
- Tek sword sprite.
- 4 veya 8 direction offset/rotation profile.
- Attack sırasında basit rotation arc.
- Idle’da sword elde görünür.

Önerilen mount data:

```csharp
[System.Serializable]
public struct WeaponMountProfile
{
    public ClassType classType;
    public Sprite weaponSprite;
    public Vector2 northOffset;
    public Vector2 southOffset;
    public Vector2 eastOffset;
    public Vector2 westOffset;
    public float northRotation;
    public float southRotation;
    public float eastRotation;
    public float westRotation;
    public int sortingOrderOffset;
}
```

Warblade demo offset prensibi:
```text
South: body ön/sağ taraf, blade çapraz aşağı
North: body arkasına biraz daha gömülü ama tamamen kaybolmaz
East: sağ elde ileri/sağ
West: flip veya ayrı offset
```

## Elementalist weapon

Kanonik:
- Elementalist elde silah tutmaz.
- Asa/staff yasak.
- Sağ avucun yaklaşık birkaç pixel üstünde altın floating rune disc.
- Unity final yaklaşık 48×48.
- Altın/sarı accent.

Minimum demo çözümü:
```text
PlayerRoot
  Body
  HoverRuneDisc
    SpriteRenderer
    Hover bob animation
    Glow pulse on cast
```

Rune disc:
- Elin üstünde süzülmeli.
- Player flip olunca x offset mirror edilmeli.
- Cast sırasında 0.15-0.25 saniye scale/pulse.
- Projectile spawn noktası rune disc veya hand anchor olmalı.

## HandAnchor yaklaşımı

İdeal:
```text
PlayerAnimationFrameData
  direction
  handAnchorRight
  handAnchorLeft
  weaponPivotOffset
```

Demo için daha basit:
```text
Class + Direction → offset/rotation lookup
```

Şimdilik frame-frame anchor şart değil. Önce oynanabilirlik.

## Attack animation önerisi

Warblade:
- Code-only arc yeterli.
- Sword sprite 0.12-0.18 saniye rotate olur.
- Hitbox sword arc ile aynı yönde spawn olur.
- VFX sonra.

Elementalist:
- Disc pulse.
- Projectile spawn.
- 0.1 saniye hand glow.
- Projectile trail sonra.

## Test checklist

Warblade:
- Sword idle’da görünür.
- Sword floor/cliff altında kalmaz.
- Sword attack sırasında kaybolmaz.
- Direction değişince saçmalamaz.
- Sword pivot eldeymiş gibi görünür.

Elementalist:
- Staff yok.
- Disc hover eder.
- Disc player arkasında kaybolmaz.
- Projectile disc/hand civarından çıkar.
- Cast hissi Warblade’den farklıdır.

## Kaçış planı

Silah mount çok zaman yerse:

Warblade:
- Sword’u body sprite’a geçici olarak attach child yap, direction offset sadece South/East/West/North için.

Elementalist:
- Rune disc’i player child olarak sabit local offset ile bağla, sadece flipX mirror yap.

Full SpriteHandData post-demo.
