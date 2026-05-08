---
name: RIMA Test Automation Architecture
description: Modular behavioral contract testing system — how to structure, run, and QC tests in RIMA
type: project
---

## Karar (2026-05-07)

RIMA'da **Behavioral Contract Testing** sistemi kuruldu. Tüm beklenen davranışlar önce `Contracts/` class'larında sabit olarak tanımlanır, ardından EditMode/PlayMode testleri bu kontratları doğrular.

## Klasör Yapısı

```
Assets/Tests/
  Contracts/                    ← beklenen davranış sabitleri
    TimeScaleContract.cs
    BootstrapContract.cs
    CombatContract.cs
    UIFlowContract.cs
    MovementContract.cs
    PerformanceContract.cs
  EditMode/
    Bootstrap/
    CodeQuality/         ← PerformanceAntiPatternTests.cs burada
    Contracts/           ← TimeScaleGuardTests.cs burada
  PlayMode/
    Bootstrap/
    Movement/
    UIFlow/
Assets/Scripts/Dev/
  MovementDiagnostic.cs  ← runtime debug tool, sadece Editor'da
```

## Modüller ve Kapsamları

| Modül | Tip | Kapsam |
|---|---|---|
| TimeScaleContract | EditMode | Pause yapan her UI Hide/Close'da timeScale=1 döndürür, guard'lar var |
| BootstrapContract | PlayMode | Sahne açılışından 0.5s içinde timeScale=1, PlayerController enabled, BasicAttackProfile assigned |
| MovementContract | PlayMode | WASD → RB.velocity≠0, FacingDirection değişiyor |
| CombatContract | EditMode | IBasicAttackBehavior tüm metodları override edilmiş, BasicAttackProfile.Validate() geçiyor |
| PerformanceContract | EditMode | Update/LateUpdate hot path'lerinde Find/Alloc yok |
| UIFlowContract | EditMode | timeScale=0 yapan class'ların Hide()/Close() metodu mevcut ve 1 döndürüyor |

## Duruma Göre Çalıştırma (Unity Test Runner Category Filter)

| Senaryo | Çalıştır |
|---|---|
| Kod değişikliği | `CodeQuality` + `Bootstrap` (EditMode, <2sn) |
| Yeni UI eklendi | `UIFlow` + `TimeScale` |
| Hareket bozuldu | `Movement` (PlayMode) |
| Performans fix | `Performance` |
| Büyük değişiklik / PR | Hepsi |

## Öğrenilen Kritik Bug (2026-05-07)

`MainMenuScreen.AutoInit()` → `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` her sahne yüklenişinde çalışıyordu. "OYNA" → `Destroy(gameObject)` → _IsoGame yüklenince AutoInit yeni MainMenuScreen yaratıyor → `Start()` → `timeScale=0f`. Karakter hareket etmiyordu.

Fix: `_gameStarted` static flag, `OnPlayClicked()`'ta set edildi.

**Bu yüzden BootstrapContract zorunlu:** her yeni scene'de timeScale=1 guard olmalı.

## QC Workflow

Codex mekanik implementasyon yapar. Opus (rima-qc) eleştirel review yapar:
- Test coverage boşlukları
- Yanlış assertion logic
- Contract tanımlarındaki eksikler
- Birbirini cross-validate eder

**Why:** 2026-05-07'de elle debugging ile bulunan timeScale bug'ı otomatik testle 1 dakikada bulunabilirdi. Test suite olmadan bu tür regresyonlar her seferinde elle teşhis edilecek.

**How to apply:** Yeni feature/fix eklenmeden önce ilgili contract test yazılmalı. Codex yazar, rima-qc inceler.
