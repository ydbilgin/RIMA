# Warblade WEAPONLESS Anim Handoff (2026-06-16)
> **SUPERSEDES** `ANIM_PRODUCTION_HANDOFF_2026-06-15.md` §⚔️ "silahlı-bake" kararını. Test KANITLADI: silah ismi geçmeyince state'ler 8 yönde de **eli boş** kalıyor (mid-run + strike-windup 2/2 doğrulandı, S/SE/E temiz). → **Karakter TEKRAR ÜRETİLMİYOR.** Silahsız state seti + **runtime weapon mount** (`HandAnchorAttach`+`SpriteHandData`+`WeaponDatabaseSO` zaten kodda). Prompt-craft: `PIXELLAB_WEAPONLESS_ARMED_PROMPTCRAFT_2026-06-16.md`.

## WEAPONLESS STATE SETİ (base warblade `2656075d`'den türetildi, hepsi eli boş)
| Anim | State char ID | Durum |
|---|---|---|
| breathing-idle | `89823ecc-c709-41db-b635-546acf443f30` | üretiliyor |
| mid-run | `ccbc13ed-2225-465d-8b51-f04624abf150` | ✅ DONE (weaponless doğrulandı) |
| strike-windup (LMB) | `3b8dad34-6874-4d17-a7ba-2f7c592ed7a8` | ✅ DONE (weaponless doğrulandı) |
| flinch (P2) | `ebd7f8af-1c12-4705-adf2-439a66a2f216` | üretiliyor |
> Silinen obsolete (armed-inconsistent + 1 failed): e0a13068, b135a79f, b11e4a9c, 9f46d1c6, 56034be1.

## ANIMATE PROMPTLARI — Custom Animation V3 (web UI: "Add Animation" → Custom Animation V3)
**Her state için:** o state'i aç → Animation Type = **Custom Animation V3** → aşağıdaki **Action Description**'ı yapıştır → **Frame Count** ayarla → **Keep first frame (idle pose) = AÇIK** → yönleri üret (**S, SE, E, NE, N**; W/SW/NW = Unity flipX mirror) → Generate.
> ⚠️ **HEPSİ WEAPONLESS:** action metninde de silah kelimesi YOK — hareketi KOL/GÖVDE ile tarif et. Mount edilen silah bu kol-arkına oturur.

### 1) idle ← state `89823ecc` · **Frame Count: 8**
```
guarded breathing idle loop, subtle chest and shoulder rise and fall, slight weight shift between feet, hands relaxed and empty at the sides, no foot sliding
```

### 2) run ← state `ccbc13ed` · **Frame Count: 8**
```
run loop, grounded top-down ARPG movement, arms swinging naturally front to back, readable foot cycle, hands empty
```

### 3) LMB attack ← state `3b8dad34` · **Frame Count: 6**
```
fast melee strike: the lead arm sweeps from a raised wind-up down and across the body in a powerful overhead swing, torso rotates with the motion, then a quick recovery back to guard, hands empty, no weapon, no VFX
```
> ⭐ KRİTİK: "sword slash" DEME. "kol overhead savruluyor" diyerek boş-el saldırı hareketi üret → runtime'da mount edilen greatsword bu kola/ele yapışır ve doğru savrulur.

### 4) flinch ← state `ebd7f8af` · **Frame Count: 4** (P2, sadece P1 OBS-temizse)
```
short hit reaction, upper body recoils backward then recovers toward a guarded stance, hands empty, no spin, no knockdown
```

> Q/E/R/F skilleri: bespoke anim YOK → LMB/run reuse + `SkillVfx.cs` tint (önceki karar korunur).

## MALİYET / SIRA (web UI V3)
- V3: ~**2 generation / yön** (8 frame; ekrandaki "Cost: 2 generations per direction"). 3 anim × 5 yön × 2 = ~30 gen (P1: idle+run+LMB). +flinch ~8 gen. Bakiye 714 = bol.
- Sıra: idle → run → LMB (P1 demo) → flinch (P2). Her anim sonrası PixelLab UI'da **görsel teyit** (eli boş + tutarlı kaldı mı), sonra Unity import + animator wiring.

## SONRAKİ (Unity, post-anim)
1. 4 weaponless anim'i import (8-yön sprite sheet, PPU64, 10-12fps).
2. **Weapon mount:** greatsword sprite'ı `HandAnchorAttach` ile el-anchor'a bağla (per-frame pivot = `SpriteHandData`); LMB anim'de silah kola uyar.
3. Animator state machine: idle/run/LMB(+flinch) wiring + flipX mirror (W/SW/NW).
