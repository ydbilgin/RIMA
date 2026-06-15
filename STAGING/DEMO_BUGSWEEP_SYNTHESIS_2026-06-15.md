# Demo Bug-Sweep — 4-LENS SENTEZ (2026-06-15)

> Council: cx (Codex/yekta) + ax 3.1 Pro + ax 3.5 Flash + orchestrator yapısal graphify lens. Synthesis: Opus 4.8.
> Scope: demo-kritik yüzey (golden-path + 4 god-node tool). Full-codebase taraması = post-demo (config policy).

## YAKINSAMA (≥2 advisor = yüksek güven)

| # | Bulgu | Lensler | Gerçek demo riski |
|---|---|---|---|
| **A** | `DirectorMode.cs:150-167` GameEntryScenes → **full-flow'da Director+BuildMode HİÇ kurulmaz** (F2/backquote çalışmaz) | ax_flash + orchestrator | ⚠️ **SETUP GOTCHA (kod değil):** demo `_Arena`-direct ile koşulmalı, MainMenu full-flow ile DEĞİL. Yoksa centerpiece tuşları ölü. |
| **B** | `DraftManager.cs:113-114` OnSecondaryClassSelected anonim lambda, unsubscribe YOK | cx + ax_pro + ax_flash (**3/3**) | 🔴 **Çoklu-take/restart'ta softlock:** yok edilmiş DraftManager lambda'sı → MissingReferenceException → boss dual-class paneli açılmaz. "10× prova" = çok run → ısırabilir. Temiz fix. |
| **C** | `DirectorMode.cs:658` `hasCameraTarget` oda değişiminde reset edilmiyor | ax_pro | 🟠 **2. odada kamera void'e fırlayabilir** (storyboard 2. oda + Build Mode kullanıyor) → DOĞRULA. |
| **D** | Draft/timeScale yarışları: `ShowDraft` IsDraftActive-guard YOK (L195) · `RestoreGameplayTimeScale` koşulsuz timeScale=1 (RoomRunDirector L1754) · `HandleRoomCleared` IsDraftPending set etmiyor (L159) | ax_pro (cluster), ax_flash (RestoreTimeScale=POLISH) | 🟠 Robustluk açığı: draft çift-açılma / pause bozulma. Basit golden-path tetiklemeyebilir ama race mümkün. Defansif guard ucuz. |

## POLISH (görünür ama akış bozmaz)
- `DirectorMode.cs:1695` **maxHP slider PlayerStats'a yazıyor, Health okumuyor** → stat-demo'da HP görsel yalan. **Çözüm: koreografide maxHP slider'ına DOKUNMA** (physPower zaten kanıtlı). (cx)
- `RewardPickup.cs:189` legacy door + RoomRunDirector çift lifecycle → `_Arena` açılıyor, hibrit sahnede legacy redundancy. (cx)
- `BuildPlacementController.cs:522` eyedropper radius vs erase exact-cell. (cx)

## POST-DEMO / DISMISSED
- `DraftManager.cs:457,467` null-skill / max-passive silent return (F2-pattern) → şimdi filtreleniyor, bad-data softlock. Post-demo. (cx)
- `DamageCalculator.cs:50` ax_pro SUSPECTED "neutral default=0 → hasar sıfırlanır" → **ÇÜRÜTÜLDÜ:** orchestrator unit-test'i nötr ClassStatRuntime'la 50→50/250→250 doğru hasar verdi (mult'lar=1, 0 değil). 
- DamageCalculator posture-overflow stub (TODO), BuildMode disk-save, Echo cadence, auto-collect=0 → ax_flash POST-DEMO "dokunma" (over-engineering).
- Legacy `InPlayMapPaintOverlay` (deg=93 god-node) = dead code grafta; storyboard "RETIRED gösterme". Post-demo temizlik.

## ÖNERİLEN AKSİYON (demo öncesi)
1. **A — SETUP:** Demo `_Arena`-direct koş (kod değil, prosedür). Dökümante et.
2. **B — FIX (öneri):** OnSecondaryClassSelected'i OnDisable/OnDestroy'da unsubscribe et (named method'a çevir). Çoklu-take güvenliği. Cerrahi.
3. **C — VERIFY:** 2. oda + Build Mode'da kamera reset'ini runtime test et; gerçekse hasCameraTarget reset ekle.
4. **D — opsiyonel hardening:** `ShowDraft` başına `if (IsDraftActive) return;` defansif guard (ucuz, çift-draft önler).
5. Polish: maxHP'ye dokunma (koreografi notu).
