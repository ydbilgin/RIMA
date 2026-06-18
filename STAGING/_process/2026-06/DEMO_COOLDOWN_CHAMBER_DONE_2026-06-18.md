# DEMO — Cooldown göstergesi + Chamber skill seçici (2026-06-18)

Status: TASK 1 DONE · TASK 2 DONE · Console 0 error / 0 warning (recompile sonrası).

## TASK 1 — Skill cooldown sayı göstergesi
Dosya: `Assets/Scripts/UI/SkillBarUI.cs` (~619-628).

Eşik düşürüldü: artık cooldown'da OLAN her skill sayı gösteriyor.
- `remaining >= 1f` → `Mathf.CeilToInt(remaining)` (örn "5","4"…"1")
- `remaining > 0.05f` (ve < 1f) → `remaining.ToString("0.0")` (örn "0.4")
- `<= 0.05f` / IsReady → boş string

Radial fill (`cdOverlay.fillAmount`) ve diğer görsel (dim/glow/rim) DOKUNULMADI — sadece `cdTimer.text` eşiği değişti. `RemainingCooldown` getter (SkillBase.cs:54) zaten vardı.

## TASK 2 — Chamber skill seçici (full roster)
Dosya: `Assets/Scripts/UI/ChamberSelectBootstrap.cs`.

UI yaklaşımı: **IMGUI `OnGUI` overlay** (chamber'da mevcut IMGUI yoktu; CharacterSelectScreen Canvas akışına dokunmamak için bağımsız, self-contained panel — yeni Canvas/prefab framework KURULMADI). Input deseni mevcut `WasPressed(Keyboard.current?.xKey)` desenini izliyor.

Kullanım: dummy yakınındayken **[K]** picker'ı aç/kapat. Panel = aktif class'ın implemented + non-passive skill listesi (`SkillDatabase.GetPool(currentClass, None)` filtreli). Skill seç → Q/E/R/F slot butonuna bas (ata) → **Uygula** (`GrantCustomLoadout`). **Varsayılan** = sabit-4 kit'e dön (`GrantPracticeLoadout`). **Kapat [K]**.

Dokunulan noktalar:
- Fields (~99-102): `skillPickerOpen`, `pickerSelectedSkill`, `pickerSlotKit[4]`, `pickerSkillScroll`.
- Update (~259-266): nearDummy + IsDemoSelectable gate'li K-toggle + auto-close (leaving dummy).
- Prompt (~366): dummy prompt'una `[K] Skill Seç` eklendi.
- `GrantPracticeLoadout` (~1867) → ortak `ApplyLoadoutKit(cls, kit, label)` helper'ına refactor edildi; yeni `GrantCustomLoadout(cls, picked)` (~1879) aynı helper'ı kullanır, default kit'ten başlayıp non-null user seçimini overlay eder (seçilmeyen slot default kalır).
- `OnGUI` picker (~2161) + `PickerSlotLabels`.

KISITLAR korundu:
- **Chamber-only**: DraftManager / run loadout'a DOKUNULMADI. Sadece chamber player'ın skill controller slotlarına yazıyor.
- **Reset garantili**: skiller in-memory component (host'a AddComponent), scene unload'da destroy → run'a girince sıfırlanır. Persist YOK.
- **Sabit-4 default korundu**: hiç seçim yapılmazsa / Varsayılan'a basılırsa default PracticeKit kalır. Picker yalnız demo-selectable class (Warblade/Elementalist) için açılır (non-demo class'ta PracticeKits yok zaten).

## Doğrulama
- refresh_unity (compile) → editor_state: is_compiling=false, domain reload tamam, ready_for_tools=true.
- read_console (error+warning): 0 entry. Jersey10 uyarısı şu an konsolda yok.
- PLAY edilmedi (kural). Sahneye dokunulmadı (sadece kod).

## Sapma / not
- Yeni input key `K` seçildi (tab/g/escape mevcut; Q/E/R/F cast tuşları). IMGUI butonları kalıcı keyboard focus almaz → WASD + Q/E/R/F cast picker açıkken çalışmaya devam eder.
