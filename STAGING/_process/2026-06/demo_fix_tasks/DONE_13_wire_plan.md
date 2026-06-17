# DONE 13 — Mob/Char sprite WIRING-PLAN (Analiz Raporu)

## 1. Bozuk Prefab Envanteri

### Animator Controller fileID: 0 (BAĞLANMAMIŞ)
- **ChainWarden** — Assets/Prefabs/Enemies/ChainWarden.prefab:174
- **VoidThrall** — Assets/Prefabs/Enemies/VoidThrall.prefab:186
- **RelicCaster** — Assets/Prefabs/Enemies/RelicCaster.prefab:173
- **HollowHulk_GB** — Assets/Prefabs/Enemies/HollowHulk_GB.prefab
- **ShardWalker_GB** — Assets/Prefabs/Enemies/ShardWalker_GB.prefab

**Toplam: 5 prefab muhtaç reparasyon (EnemyAnimator fallback ile maskeli)**

### Sprite fileID: 0 (KAYIP)
- **Player.prefab** — Assets/Prefabs/Player.prefab:117 (Body SpriteRenderer)

---

## 2. Mevcut Asset Envanteri

### Bağlı Animator Controllers (REPO'DA VAR, PREFAB'DA REF YOK)
- **ChainWarden:** Assets/Animations/Enemies/ChainWarden/ChainWarden.controller
- **RelicCaster:** Assets/Animations/Enemies/RelicCaster/RelicCaster.controller
- **VoidThrall:** Assets/Animations/Enemies/VoidThrall.controller

### Animatoren Bağlı Olan Prefab'lar (HEALTHY)
- **FractureImp:** guid 23fcf855e190c4f45a2de7cdbdd20041 (bağlı) → sprite da bağlı
- **HalfThrall:** guid 1d4e5752d5444df4791cc28988334d08 (bağlı) → sprite da bağlı
- **Penitent:** guid a1915345c95e6e745a29dbb33cf90cbc (bağlı) → sprite da bağlı

### PixelLab Generated Sprites (REPO'DA VAR)
- Assets/Sprites/Mobs/ShatteredKeep_PixelLab/ (enemy_00–enemy_15 PNG + prefab'lar)
- Hazır 16 PixelLab A2 enemy spritesheet'leri (henüz ChainWarden/VoidThrall/RelicCaster'a atanmamış)

---

## 3. Runtime Sprite Loader Mekanizması

### EnemyAnimator (Assets/Scripts/Enemies/EnemyAnimator.cs:37-71)
- **Awake:** authorized sprite cache → `_fallbackSprite`
- **Animator:** NULL sprite yaz (archived frames yok)
- **LateUpdate:** NULL veya empty-name sprite → `_fallbackSprite` restore
- **Controller:** `runtimeAnimatorController == null` check'i (satır 76)

### PlayerAnimator (Assets/Scripts/Player/PlayerAnimator.cs:138-154)
- **Awake:** body sprite cache (line 81)
- **LateUpdate:** NULL || textureless sprite → fallback restore
- **Guard:** health.IsDead iken skip (death animation don't fight)
- **Wire:** PlayerClassManager.SetFallbackSprite()

---

## 4. Player Weaponless State

### Player.prefab/Body
- **Animator.m_Controller:** {fileID: 0} ← NULL
  - Needs: Warblade.controller (Assets/Animations/Characters/Warblade/)
  - Idle states: 89823ecc / run ccbc13ed / windup 3b8dad34 / flinch ebd7f8af

- **SpriteRenderer.m_Sprite:** {fileID: 0} ← NULL
  - Filled by PlayerClassManager.SetFallbackSprite() on class apply

---

## 5. Minimum Wire Adımları (Demo)

### 5A. Mevcut Repoyla (Credits-Free)
1. ChainWarden.prefab → Animator.m_Controller = ChainWarden.controller
2. VoidThrall.prefab → Animator.m_Controller = VoidThrall.controller
3. RelicCaster.prefab → Animator.m_Controller = RelicCaster.controller
4. HollowHulk_GB / ShardWalker_GB → fallback mechanism holds (visible)
5. Player.prefab/Body → Animator.m_Controller = Warblade.controller

**Result:** 5 prefab Animator fix = demo okunabilir, visible mobs/player

### 5B. Yeni PixelLab A2 Batch (Post-Demo)
- ShatteredKeep_PixelLab/enemy_00–15 ile yeni monster prefab'lar
- ChainWarden/VoidThrall/RelicCaster için PixelLab direkt sprite/controller (8-dir A2)

---

## 6. DOSYA YOLU

F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_process\2026-06\demo_fix_tasks\DONE_13_wire_plan.md

---

## 7. Özet

| Item | Count | Status |
|------|-------|--------|
| Broken Animator (fileID:0) | 5 | Repo controller var, ref yok |
| Minimum wire adımı | 5+1 | Animator field wire |
| PixelLab sprites ready | 16 | Henüz unassigned |
| Demo requirement | SOLID | 5 Animator wire = visible demo |

