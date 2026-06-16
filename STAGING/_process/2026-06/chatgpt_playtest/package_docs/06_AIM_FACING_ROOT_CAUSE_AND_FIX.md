# Mouse Aim, Body Facing ve Skill Direction

## Gözlenen ayrışma

- WeaponSlot cursor'a bakıyor.
- Body movement/last move yönünde kalıyor.
- Skill hit geometry karakterin facing yönünü kullanıyor.

Bu üç ayrı truth source demektir. Combat hissini anında bozar.

## Tek aim kaynağı

```text
InputReader
  -> Mouse/RightStick
  -> AimService
     - CursorWorldPoint
     - AimDirection
     - CardinalFacing
  -> AimSnapshot (cast başlangıcında)
     - Animator
     - WeaponSlot
     - Projectile/Hitbox
     - VFX
```

## Ayrı kavramlar

- `MoveDirection`: karakterin hareket ettiği yön.
- `AimDirection`: mouse/right stick yönü.
- `FacingDirection`: gösterilecek cardinal body yönü.
- `CastContext`: attack başlangıcında alınan sabit snapshot.

Combat-ready durumda `FacingDirection`, `AimDirection` üzerinden hesaplanır. Menü/serbest dolaşım gibi özel hallerde movement-facing kullanılabilir.

## 4 cardinal mapping

```text
abs(x) > abs(y) -> East/West
aksi -> North/South
```

Hysteresis ekle: diagonal sınırında 1 frame sağ/sol titremesi oluşmasın.

## Animation lock kararı

- Hafif LMB saldırıları: facing cast başlangıcında snapshot alınır, attack bitene kadar hit geometry aynı yönü kullanır.
- Uzun channel/beam: skill tanımı izin veriyorsa aim sürekli güncellenir.
- Ground target: body yalnız görsel olarak cursor'a döner, gerçek nokta `CursorWorldPoint`tir.

## Sprite/WeaponSlot kuralı

Body ve weapon aynı cardinal facing'i kullanır. Silahın local rotation'ı, body sprite yönünü bağımsız bırakmamalı. Asimetrik sprite'larda flip kullanma; S/E/N/W ayrı clip/sprite.

## Skill tiplerine göre hedef

| Skill tipi | Kaynak |
|---|---|
| Projectile | `CastContext.AimDirection` |
| Cone/Cleave | `CastContext.AimDirection` |
| Line/Dash attack | `CastContext.AimDirection` veya explicitly movement-cast |
| Ground target | `CastContext.CursorWorldPoint` |
| Self AoE | player origin |
| Auto-target | explicit target resolver; facing fallback değil |

## Kontrolcü desteği

Mouse ve right-stick aynı AimService contract'ını kullanmalı. Son aktif cihaz değiştirilince cursor/right-stick source değişebilir; skill sistemi değişmez.
