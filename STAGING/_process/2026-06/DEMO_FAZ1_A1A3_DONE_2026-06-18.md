# DEMO FAZ-1 FIX DONE — A1 + A3 (2026-06-18)

Spec: `STAGING/_process/2026-06/DEMO_FAZ1_A1A3_SPEC_2026-06-18.md`. Both items DONE, compile-clean (0 error / 0 warning). PLAY edilmedi, sahneye dokunulmadı (`_Arena` açık kaldı).

---

## A1 — FAILED-CAST FEEDBACK (cooldown-AYRIMLI) — DONE

**Dosya:** `Assets/Scripts/Skills/Base/SkillBase.cs`

**Değişiklik:**
- `TryActivate()` (satır ~82-100): üç early-return'e ayrımlı feedback eklendi.
  - `!IsReady` (COOLDOWN) → satır 84: **DEĞİŞMEDİ, sessiz** (tuş-spam SFX yok — spec binding'i). Yorum eklendi.
  - `!CanExecute()` (VETO) → satır 91: `FailedCastFeedback("Uygun hedef yok")` + return false.
  - `rageCost` yetersiz → satır 92: `FailedCastFeedback("Yetersiz kaynak")` + return false.
  - `resourceCost` yetersiz → satır 93: `FailedCastFeedback("Yetersiz kaynak")` + return false.
- Yeni private helper `FailedCastFeedback(string reason)` (satır ~108-118): sadece sunum, combat/cost/cooldown state'ine DOKUNMAZ.

**Aynalanan mevcut sistemler (yeni infra YOK):**
- **SFX:** `RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.DraftHover)` — `enum Sfx`'te ayrı "deny/fail" cue YOK; `DraftHover` en yakın nötr UI-tık cue'su (Tone 0.08s, 1100Hz, kısa), başarılı cast/hit ile karışmaz. DraftManager'ın aynı fully-qualified `RIMA.Audio.AudioManager.Play(...)` çağrı kalıbı aynalandı. (Procedural fallback default mute; gerçek klip düşünce otomatik açılır — infra zaten halleder.)
- **Flash:** `SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Void)` — her Elementalist skill'in kullandığı caster-flash deyimi birebir aynalandı; `VfxElement.Void` (mor) palette'teki en "negatif" renk → başarılı cast tinti ile karışmaz.
- **Toast:** `HUDController.Instance?.ShowToast(reason)` — mevcut `HUDController.ShowToast(string, float=2.5f)` API'si (DraftManager HandlePassivePick'te aynı şekilde kullanılıyor). Yeni toast infra KURULMADI.

**Davranış kanıtı:** cooldown yolu sessiz kaldı (early-return değişmedi); veto + iki kaynak-yetersiz yolu aynı feedback çağrısını yapıyor; Execute()/cooldownTimer/TrySpend mantığı değişmedi (TrySpend zaten false dönerse kaynak harcanmaz — mevcut davranış korundu).

## A3 — DIRECTOR DUP-SLOT REJECT — DONE

**Dosya:** `Assets/Scripts/Skills/DraftManager.cs`

**Değişiklik:**
- `TryDirectorAssignSkill(SkillData skill, int slot, out string error)` (satır ~424): `host` resolve edildikten SONRA, `RemoveOwnedSkill`/`AssignActive`'den ÖNCE dup-slot kontrolü eklendi (satır ~439-453).
  - Primary host'un slot 0-3'ü gezilir (`GetControllerSlot(host, i)` — satır 439'da zaten kullanılan helper aynalandı).
  - Hedef `slot` atlanır (aynı slota re-assign idempotent, izinli).
  - Başka bir slotta `occupant.GetType() == skill.skillType` ise → **REJECT**: `error = "<skill> zaten N. yuvada — aynı yetenek iki yuvaya atanamaz"`, `return false`. **`AssignActive`/`AddComponent` HİÇ çağrılmaz** → orphan-component leak yok, shared-cooldown bug'ı oluşmaz.
- `AssignActive`/combat numerics'e DOKUNULMADI.

**Aynalanan mevcut sistem:** mevcut `out string error` UI-bildirim mekanizması (fonksiyonun zaten kullandığı tüm diğer fail-path'ler gibi `error = "..."; return false;`). `GetControllerSlot(Component, int)` mevcut helper, satır 439'daki kullanımla birebir.

**Neden TryDirectorAssignSkill (AssignActive değil):** dup tespiti AddComponent'ten ÖNCE olmalı (spec binding). Director path her zaman primary band (slot 0-3, `ResolvePrimarySlotHost`) kullanır ve `slot` 0..3 valide edilir → 0-3 taraması tam kapsar.

---

## DOĞRULAMA
- `refresh_unity` (compile request, force, scripts) → `editor_state.compilation.is_compiling = false`, `is_domain_reload_pending = false`, `advice.ready_for_tools = true`.
- `read_console` (error+warning, 50) → **0 entry**. 0 error / 0 warning.
- PLAY edilmedi. Aktif sahne `_Arena` (is_playing=false), sahne/asset'e yazılmadı.

## DEVIATIONS / UNCERTAIN
- Yok. Spec'in iki binding'i de harfiyen uygulandı (cooldown sessiz; reject AddComponent-öncesi). Tüm feedback mevcut API'lerden aynalandı, yeni sistem/sabit eklenmedi.
- Not: `DraftHover` SFX procedural-fallback default mute olduğundan demo'da duyulabilir ses gerçek klibe bağlı (mevcut audio-policy; A1 scope dışı, infra zaten böyle çalışıyor). Flash + toast her durumda görünür.
