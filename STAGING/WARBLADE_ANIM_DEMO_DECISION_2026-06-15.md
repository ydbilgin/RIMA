# Warblade Demo Animasyon Kararı — Council (2026-06-15)

> **Soru:** Demo (~20 Haz, ~5 gün) için warblade hangi animasyonlar üretilmeli? Kaynak: kullanıcı + PixelLab "States" video stratejisi (oCJWxfEwX-o). Council = cx + ax 3.1 Pro + ax 3.5 Flash, hepsi bağımsız. Ham: `_process/2026-06/anim_council/ANIM_{cx,axpro,axflash}.md`.
> **Durum:** warblade 8dir 120x120 hazır ama `animations: none`. Budget 874 generation = BOL. **Darboğaz = generation DEĞİL, per-yön manuel cleanup ZAMANI.**

## KARAR (oybirliği: 3 council + orchestrator)

### P1 — MUST (golden-path videonun KALBİ; vaktin ~%100'ü buna, kusursuzlaştır)
1. **Idle** (breathing / fight-stance)
2. **Koşma (Run)**
3. **LMB temel vuruş** (stat→damage beat = combat centerpiece — videoda matematiği SADECE bu gösterir)

### Q/E/R/F skill'leri → REUSE (bespoke YOK) — oybirliği
Demoda `bypassStatScaling` = stat-deaf koreografi. Sıfırdan animasyon ÜRETME. **LMB/generic-cast animasyonunu REUSE et + `SkillVfx.cs` engine-juice (tint/additive/scale-fade) ile renk/efekt farklılaştır** (Dead Cells "tek sprite + engine juice"). Bespoke skill animasyonları = **POST-DEMO**.

### P2 — SADECE P1 temizse
- **Hit-react / Flinch** (stat→damage beat'i "okutur" — vurulma okunur). [birincil P2]
- *(ax_flash: Dash da P2 — golden-path'te dash GÖRÜNÜYORSA ekle; ax_pro geri çekti → opsiyonel)*

### P3 / POST-DEMO
LMB beat 2/3, RMB cleave, Death, bespoke Q/E/R/F. (5 günde tüm set = gerçekçi değil — "intihar".)

## STATE-FIRST ÜRETİM PLANI (video stratejisi)
| Anim | Önce STATE üret | Sonra |
|---|---|---|
| Idle | breathing / fight-stance-idle | Custom Animation V3 (loop) |
| Koşma | **mid-run** state | Custom Animation V3 (run loop) |
| LMB vuruş | **strike-windup** state | Custom Animation V3 + **interpolation** (windup→strike→recover) |
| (P2) Flinch | **flinch/hit** state | V3 kısa (3-5 frame) |

- **Yön:** canon = **5 üret (S, SE, E, NE, N) + 3 mirror (flipX: W←E, SW←SE, NW←NE)**. 120x120, high-top-down, 10-12 fps, PPU 64.
- **Per-yön:** kimi mükemmel bırak · kimi Pixelorama cleanup · kimi reroll (video workflow'u).

## BUDGET / GERÇEKÇİLİK
- P1 ≈ **~15 generation call** (874'ten önemsiz). Cleanup ≈ **5-7.5 saat** → 5 günde GÜVENLİ.
- P1 + 1 P2 = gerçekçi. Bespoke skill seti = gerçekçi değil.

## ÜRETİM CAVEAT (cx kod-lensi)
Animasyon üretmeden/sonra **Unity animator/sprite-swap wiring'i doğrula** (warblade şu an animasyonsuz — sprite statik olabilir). Üretilen anim'lerin oyunda gerçekten oynatıldığından emin ol. Detay → `ANIM_cx.md`.

## DURUM
- Üretim = PixelLab + kullanıcı (Claude 4.8 kotası ~%3, ~21s reset → karar şimdi, üretim sen). Geri-dönülebilir; nihai çağrı senin.
