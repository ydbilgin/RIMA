# DEMO FAZ-1 FIX SPEC — A1 + A3 (council + adversarial-critic FINAL, 2026-06-18)

Demo ~yarın editörde. Council (cx+ax_pro+ax_flash) + adversarial Opus-4.8 kritik kararı: SADECE 2 madde, kritik binding-fix'leriyle. Diğer her şey post-demo (over-reach demo-eve riskli).

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear. **Core combat damage/execution path'ine DOKUNMA** (sadece feedback/validation ucu).

## UNITY ERROR CHECK
İş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR. Instance `RIMA@ed023e0b`. **Sen TEK Unity-ajansın.** PLAY ETME — compile-clean yeter. Kullanıcı `_Arena` açık tutuyor — SAHNEYE DOKUNMA/kaydetme, sadece kod.

---

## A1 — FAILED-CAST FEEDBACK (cooldown-AYRIMLI) 🔴 en yüksek demo-değeri
**Sorun:** Skill basıldığında veto/yetersiz-kaynak durumunda ses/flash/mesaj YOK → oyun bozuk sanılır.
**KRİTİK BINDING (cooldown-spam riski):** `SkillBase.TryActivate()` false döner: (a) `!IsReady` = COOLDOWN, (b) `!CanExecute()` = veto, (c) rage/resource yetersiz. **Feedback SADECE (b) veto + (c) yetersiz-kaynak için. Cooldown (a) SESSİZ kalmalı** (tuş-spam SFX olmasın).

**Fix:**
1. ÖNCE `SkillBase.cs` `TryActivate()`'i oku (mevcut early-return sırası: IsReady → CanExecute → rage → resource).
2. ÖNCE mevcut feedback sistemlerini bul + AYNALA (yeni infra KURMA):
   - SFX: `AudioManager.Play(Sfx.XXX)` — uygun bir "deny/error/fail" sfx var mı? (Grep `enum Sfx`). Yoksa en yakın uygun olanı kullan, yenisini ekleme.
   - Flash: `SkillVfx` (CastFlash benzeri) — caster flash için.
   - Toast/mesaj: HUD'da mevcut toast API'si var mı? (Grep `Toast`/`ShowToast`/HUDController). VARSA kullan, YOKSA SFX+flash yeter — **toast infra'sı KURMA**.
3. `IsReady` false → değişiklik YOK (sessiz). `CanExecute()` false → feedback("hedef yok/menzil dışı") + return false. rage/resource yetersiz → feedback("yetersiz kaynak") + return false.
4. Feedback yan-etkisiz, sadece sunum; combat state DEĞİŞMEZ.

## A3 — DIRECTOR DUP-SLOT REJECT 🔴 centerpiece
**Sorun:** Zaten-equipped bir skill 2. slota atanınca aynı component 2. kez ekleniyor → shared cooldown / bozuk davranış (Director Mode demo-centerpiece).
**KRİTİK BINDING:** Slot-assign yolu `DraftManager.TryDirectorAssignSkill` → `AddComponent(skillType)` = **combat-equip path** ("UI-only" değil). **SWAP DEĞİL, REJECT.** `AddComponent` çağrılmadan ÖNCE reddet (orphan-component leak'i önle).

**Fix:**
1. ÖNCE `DraftManager.cs` `TryDirectorAssignSkill` + `AssignActive` (~740-750) oku; mevcut `out error` / `status.assign_failed` (veya benzeri) UI-bildirim mekanizmasını bul.
2. Assign öncesi kontrol: atanmak istenen `skillType` BAŞKA bir aktif slotta zaten equipped mi? → evetse **REJECT**: `AddComponent` ÇAĞIRMA, mevcut error/status mekanizmasıyla "already equipped" bildir, false/early-return.
3. Sadece DraftManager. Combat numerics'e dokunma.

---

## DOĞRULAMA
Recompile → `editor_state.isCompiling` false → `read_console` (Error+Warning) → 0 error. PLAY ETME.

## ÇIKTI (E1)
Detay → `STAGING/_process/2026-06/DEMO_FAZ1_A1A3_DONE_2026-06-18.md` (her madde: dokunulan dosya:satır, NE/neden, hangi mevcut sistemi aynaladın, varsa BLOCKED/uncertain). Dönüş ≤8 satır: A1/A3 DONE/BLOCKED, console durumu, rapor yolu.
