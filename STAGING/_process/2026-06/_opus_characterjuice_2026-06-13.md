# CharacterJuice — Kod-only animasyon hissi (2026-06-13)

## Amaç
Statik sprite + engine juice felsefesi (SkillVfx.cs aynı ruh). Frame animasyonu YOK; sadece
görsel child transform'larına (Body + HandAnchor) tween. Player'a "canlı" his katmak — demo için.

## Yapılan iş
Tek yeni dosya: `Assets/Scripts/VFX/CharacterJuice.cs` (RIMA.Runtime asmdef).

### Davranış
1. **Idle nefes-bob:** Body.localPosition.y'ye sin-dalga, periyot 2.2s. Pixel Perfect Camera (PPU 64)
   uyumu için bob TAM-PIXEL adımlarına yuvarlanır (1/64 katları, `SnapToPixel`). Scale KULLANILMADI
   (pixel shimmer önlendi).
2. **Yürüme:** `PlayerController.IsMoving && !IsDashing` iken Body'ye hareket yönüne 3.5° Z-tilt
   (`FacingDirection.x` işaretine göre) + daha hızlı bob (0.35s). Idle<->walk yumuşak blend.
3. **Saldırı punch:** `PlayerAttack.OnComboStep` event'inde 2px ileri-geri pozisyon-lunge (sin tepe,
   0.1s) — scale yerine pozisyon. Lunge yönü = FacingDirection.

### Kısıt uyumu
- Root pozisyonuna DOKUNULMADI — sadece Body + HandAnchor child'ları. Bob/punch offset'i HER İKİSİNE
  AYNI hesapla uygulanır (silah el hizasında kalır). Tilt yalnız Body'ye (silah dik kalsın).
- Death (`Health.IsDead`): juice durur, base konuma reset (bob'lu ceset yok).
- Pause (`Time.timeScale == 0`): early-return, unscaled time KULLANILMADI (donar).
- Inspector toggle `enableJuice` + tüm parametreler (bobAmplitudePx=1, idlePeriod, walkPeriod,
  tiltDegrees, punchPixels...) public — kapatınca base konuma döner (demo riski sıfır).

### Prefab wiring
- `Assets/Resources/Prefabs/Warblade.prefab` → CharacterJuice ×1
- `Assets/Prefabs/Characters/Warblade.prefab` → CharacterJuice ×1
- (Base `Assets/Prefabs/Player.prefab`'a yanlışlıkla eklenip variant'larda çift kopya oluştu;
  base'den kaldırıldı → her variant tam 1 kopya. Mob'lara EKLENMEDİ.)

## Doğrulama
- **Recompile:** 0 compile error (force refresh + read_console Error = boş).
- **EditMode (RIMA.Tests.EditMode):** 541 test, 11 fail — baseline 11 ile birebir aynı, YENİ fail YOK,
  CharacterJuice-kaynaklı fail YOK (failler: eksik STAGING PNG, MCP reflection, perf anti-pattern,
  prefab health, DontDestroyOnLoad — hepsi önceden mevcut).
- **Null-guard data-proof (Play Mode'a GİRİLMEDİ):** Bare GO ve Body-only GO'da Awake+LateUpdate
  reflection ile çağrıldı → NRE YOK. Body.local=(0,0,0), HandAnchor.local=(0.2,0.1,0) base korundu.
- **Prefab data-proof:** her iki Warblade prefab'ında count=1, enable=True, amp=1, ppu=64, idle=2.2,
  walk=0.35, tilt=3.5; Body+HandAnchor child'ları mevcut.

## Not / risk
- IsoSorter (Body üzerinde) world-Y*100 ile sortingOrder hesaplıyor; 1px bob (≈0.0156 birim) order'ı
  en fazla ±1-2 oynatabilir. Demo için ihmal edilebilir; sorun olursa `enableJuice` kapatılabilir.
- Git commit YAPILMADI (talimat gereği).
