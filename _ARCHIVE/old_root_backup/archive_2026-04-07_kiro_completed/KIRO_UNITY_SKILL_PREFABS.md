# KIRO GÖREVI — Skill Prefabları + Player Bağlama
Tarih: 2026-04-04

Unity MCP ile sırayla uygula. Her adımdan sonra `read_console` ile hata kontrol et.
Karar gerektiren bir şeyle karşılaşırsan dur ve bildir.

---

## ADIM 1 — Klasör Oluştur

`Assets/Prefabs/Skills/` klasörünü oluştur.

---

## ADIM 2 — PlayerProjectile Prefab

**Konum:** `Assets/Prefabs/Skills/PlayerProjectile.prefab`

Bileşenler:
- `SpriteRenderer` (sprite boş, sortingLayer="Default")
- `Rigidbody2D` (gravityScale=0, bodyType=Dynamic)
- `CircleCollider2D` (isTrigger=true, radius=0.15)
- Script: `RIMA.PlayerProjectile`

---

## ADIM 3 — DamageZone Prefab

**Konum:** `Assets/Prefabs/Skills/DamageZone.prefab`

Bileşenler:
- `SpriteRenderer` (sprite boş, color=(1,1,1,0.25) yarı şeffaf)
- `CircleCollider2D` (isTrigger=true, radius=1.5)
- Script: `RIMA.DamageZone`

---

## ADIM 4 — FrozenOrb_Projectile Prefab

**Konum:** `Assets/Prefabs/Skills/FrozenOrb_Projectile.prefab`

Bileşenler:
- `SpriteRenderer` (sprite boş, color=(0.4, 0.8, 1, 1) buz mavisi)
- `Rigidbody2D` (gravityScale=0, bodyType=Dynamic)
- `CircleCollider2D` (isTrigger=true, radius=0.3)
- Script: `RIMA.FrozenOrbObject`

---

## ADIM 5 — MirrorClone Prefab

**Konum:** `Assets/Prefabs/Skills/MirrorClone.prefab`

Bileşenler:
- `SpriteRenderer` (sprite boş, color=(0.7, 0.9, 1, 0.8) yarı şeffaf mavi)
- Script: `RIMA.Health`
- Script: `RIMA.MirrorClone`

> NOT: MirrorClone prefabına Rigidbody2D veya Collider ekleme — sadece HP ve ölüm patlaması yapar.

---

## ADIM 6 — Player'ı Bul ve SkillFlowTracker Ekle

Sahnede `_Sandbox` açık olmalı.

1. Sahnede `Player` veya `Warblade` adlı GameObject'i bul.
2. `RIMA.SkillFlowTracker` script'i eklenmiş mi kontrol et.
   - Yoksa ekle.
   - Varsa atla.

---

## ADIM 7 — Skill Bileşenlerini FrozenOrb'a Bağla

Sahnede Player'ın child'larında veya üzerinde `RIMA.FrozenOrb` bileşeni varsa:
- `orbPrefab` alanına `Assets/Prefabs/Skills/FrozenOrb_Projectile.prefab` ata.

---

## ADIM 8 — Skill Bileşenlerini MirrorImage'a Bağla

Sahnede Player üzerinde `RIMA.MirrorImage` bileşeni varsa:
- `clonePrefab` alanına `Assets/Prefabs/Skills/MirrorClone.prefab` ata.

---

## ADIM 9 — Sahneyi Kaydet

`Assets/Scenes/_Sandbox.unity` kaydet.

---

## ADIM 10 — Sonuç Raporu Yaz

`_STAGING/UNITY_SKILL_PREFABS_DONE.txt` dosyası oluştur:

```
SKILL PREFAB ASSEMBLY - TAMAMLANDI
Tarih: [bugün]

Oluşturulan prefablar:
- Assets/Prefabs/Skills/PlayerProjectile.prefab
- Assets/Prefabs/Skills/DamageZone.prefab
- Assets/Prefabs/Skills/FrozenOrb_Projectile.prefab
- Assets/Prefabs/Skills/MirrorClone.prefab

SkillFlowTracker: [eklendi / zaten vardı]
FrozenOrb orbPrefab: [bağlandı / player'da yok]
MirrorImage clonePrefab: [bağlandı / player'da yok]

Console hatası: [var mı / yok]
```

---

## NOTLAR

- TrapObject ve TrapPrefab GEREKMİYOR — ExplosiveTrap.cs runtime'da `new GameObject` ile oluşturuyor.
- Her prefab oluşturduktan sonra `read_console` çalıştır.
- Compilation error varsa dur ve bildir.
