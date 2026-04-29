# KIRO_UNITY_PREFAB_ASSEMBLY

> Unity MCP araçlarını kullanarak sırayla uygula.
> Her adımdan sonra `read_console` ile hata kontrol et.
> Karar gereken bir şeyle karşılaşırsan dur ve bildir.
> Script oluşturma yok — sadece mevcut script'leri prefab'lara bağla.

---

## HAZIRLIK

**1. Klasör yarat (yoksa)**
```
manage_asset → create folder: Assets/Prefabs/Enemies
```

**2. Sahneyi aç**
```
manage_scene → load: Assets/Scenes/_Sandbox.unity
```

**3. Asset refresh**
```
refresh_unity
```

**4. read_console → hata var mı kontrol et**

---

## GÖREV 1 — Global Light 2D

```
manage_gameobject → find: "Global Light 2D" (sahnede)
manage_components → Global Light 2D component → intensity: 2.5
```

---

## GÖREV 2 — VoidThrall (Normal) Prefab

```
manage_gameobject → create empty: "VoidThrall"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=9, chaseSpeed=2)
  - MobAttack_Throw  (attackRange=6, attackCooldown=1.8)

manage_prefabs → create prefab: Assets/Prefabs/Enemies/VoidThrall.prefab
manage_gameobject → delete "VoidThrall" from scene
```

**read_console**

---

## GÖREV 3 — VoidThrall Elite Prefab

```
manage_prefabs → duplicate: Assets/Prefabs/Enemies/VoidThrall.prefab
             → rename: Assets/Prefabs/Enemies/VoidThrall_Elite.prefab
manage_components → aç VoidThrall_Elite → ekle:
  - MobAttack_Summon
  - MobAffix_VoidTouched
```

**read_console**

---

## GÖREV 4 — SeamCrawler (Normal) Prefab

```
manage_gameobject → create empty: "SeamCrawler"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=7, chaseSpeed=4.5)
  - MobAttack_Melee  (attackRange=0.8, attackCooldown=0.9)
  - SeamCrawler_Trail

manage_prefabs → create prefab: Assets/Prefabs/Enemies/SeamCrawler.prefab
manage_gameobject → delete "SeamCrawler" from scene
```

**read_console**

---

## GÖREV 5 — SeamCrawler Elite Prefab

```
manage_prefabs → duplicate: Assets/Prefabs/Enemies/SeamCrawler.prefab
             → rename: Assets/Prefabs/Enemies/SeamCrawler_Elite.prefab
manage_components → aç SeamCrawler_Elite:
  - SeamCrawler_Trail KALDIR
  - SeamCrawler_Homing EKLE
```

**read_console**

---

## GÖREV 6 — ChainWarden Prefab

```
manage_gameobject → create empty: "ChainWarden"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=9, chaseSpeed=2.5)
  - MobAttack_ChainPull  (attackRange=6, attackCooldown=2.5)

manage_prefabs → create prefab: Assets/Prefabs/Enemies/ChainWarden.prefab
manage_gameobject → delete "ChainWarden" from scene
```

**read_console**

---

## GÖREV 7 — Penitent Prefab

```
manage_gameobject → create empty: "Penitent"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=8, chaseSpeed=3.5)
  - MobAttack_PenitentCombo  (attackRange=1.0, attackCooldown=1.2)

manage_prefabs → create prefab: Assets/Prefabs/Enemies/Penitent.prefab
manage_gameobject → delete "Penitent" from scene
```

**read_console**

---

## GÖREV 8 — RelicCaster Prefab

```
manage_gameobject → create empty: "RelicCaster"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=10, chaseSpeed=1.5)
  - MobAttack_Barrier  (attackRange=7, attackCooldown=3.0)

manage_prefabs → create prefab: Assets/Prefabs/Enemies/RelicCaster.prefab
manage_gameobject → delete "RelicCaster" from scene
```

**read_console**

---

## GÖREV 9 — FractureImp Prefab

```
manage_gameobject → create empty: "FractureImp"
manage_components → ekle:
  - SpriteRenderer
  - Rigidbody2D  (bodyType = Kinematic)
  - CircleCollider2D
  - BaseMobBehavior  (detectionRange=7, chaseSpeed=4.0)
  - MobAttack_Melee  (attackRange=0.9, attackCooldown=0.7)
  - FractureImp_ShardScatter

manage_prefabs → create prefab: Assets/Prefabs/Enemies/FractureImp.prefab
manage_gameobject → delete "FractureImp" from scene
```

**read_console**

---

## GÖREV 10 — Sahneyi Kaydet

```
manage_scene → save: Assets/Scenes/_Sandbox.unity
```

---

## BİTİNCE

`STAGING/UNITY_PREFAB_DONE.txt` dosyası oluştur ve içine tamamlanan prefab listesini yaz.

Hata çıktıysa hangi görevde, ne hatası aldığını yaz.
