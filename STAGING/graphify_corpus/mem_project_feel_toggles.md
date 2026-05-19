---
name: Game Feel Toggles — Default ON, Settings Opt-Out
description: Screen shake, hit stop, low HP vignette vb. feel feature'ları default açık, oyuncu Settings'ten kapatabilir. Her yeni feel eklendiğinde toggle zorunlu.
type: project
originSessionId: d6e053e9-da4b-433e-85e4-415ede98e315
---
Karar #50 (2026-04-24). Bütün feel/juice feature'ları default ON, Settings → Accessibility menüsünden ayrı ayrı kapatılabilir.

**Kapsam (Faz 1):** screen shake, hit stop, hit flash, low HP vignette, damage numbers, kill slowmo, chromatic aberration, motion blur, camera lead.

**Faz 2+:** ragdoll, heavy VFX, controller vibration, photo sensitivity mode.

**Pattern:**
- `IFeelFeature` interface: `bool Enabled { get; }`
- `FeelSettings` singleton: PlayerPrefs (Faz 1) → `SettingsData` SO (Faz 2+)
- Her feature execute etmeden önce `FeelSettings.I.IsEnabled("shake")` kontrol eder
- Settings menüsü auto-populate (enum + inspector list)

**Why:** Accessibility (photo sensitivity, motion sickness) + oyuncu tercihi. Default ON çünkü juice pazarlama vitrinidir — off pazarlamada ters. Kapatma seçeneği etik zorunluluk.

**How to apply:** Yeni feel feature yazarken **aynı commit'te** toggle entry ekle. Code review'de toggle yoksa FAIL. Settings menüsü Faz 1 exit scope'una dahil değil ama pattern bugünden kurulur — sonradan retrofit acı verici.
