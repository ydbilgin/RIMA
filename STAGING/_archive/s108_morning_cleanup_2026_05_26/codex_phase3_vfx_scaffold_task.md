# Codex Phase 3 — VFX System Scaffold

**Date:** 2026-05-15 S82
**Mode:** Implementation + commit
**Effort:** high
**Branch:** master (commit doğrudan)
**Scope:** Tier 1 VFX scaffold (CombatEventBus + VFXRouter + ProcLimiter + Tier 1 primitives). NO integration with existing systems. Foundation only.

---

## Bağlam

Karar #137 (LOCK adayı) 4 katmanlı RIMA VFX sistemi öneriyor:
- Katman A: Rift Identity (afterimage cyan/violet, scar pulse)
- Katman B: Class Signature (sınıf-özel imza VFX)
- Katman C: Map Environment (Wang transition glitch, patch parlama)
- Katman D: Combat Juice (Tier 1/2/3 — 20 teknik)

**Bu görev sadece Katman D Tier 1 foundation.** Sistem mimarisi spec: 3-AI sentez (Codex) önerisi `CombatEventBus + StatusMatrix + VFXRouter + ProcLimiter + GroundMarkSystem` — biz bu görevde sadece `CombatEventBus + VFXRouter + ProcLimiter + Tier 1 primitives` yazıyoruz. StatusMatrix + GroundMarkSystem ileride.

Karar #122 (LIVE): Beat3Commit T1 LIVE — `Beat3CommitTrigger.cs` StateMachineBehaviour zaten var. Event pattern uyumlu olmalı.

---

## Görevler

### 1. CombatEventBus.cs

**Path:** `Assets/Scripts/Combat/CombatEventBus.cs`
**Namespace:** `RIMA.Combat`

Static event bus, herhangi bir GameObject'e bağlı değil. Publish/subscribe pattern. Generic event tipleri:

```csharp
public static class CombatEventBus
{
    // Events
    public static event Action<HitEvent> OnHit;
    public static event Action<KillEvent> OnKill;
    public static event Action<DashEvent> OnDash;
    public static event Action<StatusEvent> OnStatusApplied;
    public static event Action<CommitBeatEvent> OnCommitBeat;  // Karar #122 T1 entegrasyonu

    // Publish helpers
    public static void PublishHit(HitEvent e) { OnHit?.Invoke(e); }
    // ... vb
}
```

**Event struct'ları (aynı dosyada veya ayrı):**

```csharp
public struct HitEvent
{
    public Vector3 worldPos;
    public GameObject attacker;
    public GameObject target;
    public float damage;
    public string element;  // "physical", "rift", "fire", "elec", "water", "ice", "void"
    public bool isCrit;
    public Vector2 hitDirection;
}

public struct KillEvent
{
    public Vector3 worldPos;
    public GameObject killer;
    public GameObject victim;
    public string mobFamily;  // "spire_choirling", "warden", vb
}

public struct DashEvent
{
    public Vector3 startPos;
    public Vector3 endPos;
    public GameObject dasher;
    public float duration;
}

public struct StatusEvent
{
    public Vector3 worldPos;
    public GameObject target;
    public string statusId;  // "bleed", "burn", "rift_corrosion"
    public float duration;
}

public struct CommitBeatEvent
{
    public Vector3 worldPos;
    public GameObject attacker;
    public int beatIndex;  // 1, 2, or 3 (Karar #122)
}
```

**Önemli:** Domain reload'da event listener'lar temizlenmeli. `[RuntimeInitializeOnLoadMethod]` ile reset metodu.

### 2. VFXRouter.cs

**Path:** `Assets/Scripts/Combat/VFXRouter.cs`
**Namespace:** `RIMA.Combat`

MonoBehaviour singleton, sahnede tek instance. Bus event'lerine abone olur, tag-based routing yapar.

```csharp
public class VFXRouter : MonoBehaviour
{
    public static VFXRouter Instance { get; private set; }

    [Serializable]
    public class VFXEntry
    {
        public string tag;            // "hit_physical", "hit_rift", "kill_default", vb
        public GameObject prefab;     // Particle prefab veya VFX prefab
        public AudioClip soundEffect;
        public float lifetime = 2f;
    }

    [SerializeField] private VFXEntry[] entries;
    [SerializeField] private int poolSize = 16;

    private Dictionary<string, Queue<GameObject>> pools;
    // ...
}
```

**Functionality:**
- `Awake`: build pools, subscribe to bus events
- Routing rules:
  - `OnHit` → look up tag `hit_<element>` veya fallback `hit_default`
  - `OnKill` → look up `kill_<mobFamily>` veya fallback `kill_default`
  - `OnDash` → `dash_default`
  - `OnStatusApplied` → `status_<statusId>`
  - `OnCommitBeat` → `commit_beat_<beatIndex>` (1, 2, 3)
- Spawn prefab pooled, set position/rotation, play sound, auto-return to pool after lifetime
- `OnDestroy`: unsubscribe events

Pool implementation basit Queue<GameObject>. Object pool helper inline.

### 3. ProcLimiter.cs

**Path:** `Assets/Scripts/Combat/ProcLimiter.cs`
**Namespace:** `RIMA.Combat`

Static helper, infinite chain reaction önler. Per-tag cooldown/budget:

```csharp
public static class ProcLimiter
{
    private static Dictionary<string, float> lastTriggerTime = new();
    private static Dictionary<string, int> frameTriggerCount = new();
    private static int currentFrame = -1;

    public static bool TryProc(string tag, float minIcdSeconds = 0.05f, int maxPerFrame = 4)
    {
        // Frame reset
        if (Time.frameCount != currentFrame)
        {
            frameTriggerCount.Clear();
            currentFrame = Time.frameCount;
        }

        // Per-frame cap
        frameTriggerCount.TryGetValue(tag, out var count);
        if (count >= maxPerFrame) return false;

        // ICD check
        if (lastTriggerTime.TryGetValue(tag, out var last))
        {
            if (Time.time - last < minIcdSeconds) return false;
        }

        lastTriggerTime[tag] = Time.time;
        frameTriggerCount[tag] = count + 1;
        return true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnDomainReload()
    {
        lastTriggerTime.Clear();
        frameTriggerCount.Clear();
        currentFrame = -1;
    }
}
```

VFXRouter her event'i `ProcLimiter.TryProc(tag)` ile filtreler.

### 4. Tier 1 VFX Primitives

Aşağıdaki 4 basit komponent yaz. Her biri CombatEventBus'a abone olur, kendi davranışını yapar:

#### 4a. HitPauseDriver.cs
**Path:** `Assets/Scripts/Combat/Juice/HitPauseDriver.cs`

MonoBehaviour singleton. OnHit event'inde `Time.timeScale = 0` 50-80ms, sonra geri 1. Configurable scale + duration field.

#### 4b. ScreenShakeDriver.cs
**Path:** `Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs`

MonoBehaviour, Cinemachine yoksa Camera.main transform direkt offset. OnHit/OnCommitBeat/OnKill event'ine abone. Magnitude config:
- OnHit: 0.05 magnitude, 0.1s duration
- OnCommitBeat (beat=3): 0.15 magnitude, 0.2s duration
- OnKill: 0.1 magnitude, 0.15s duration

#### 4c. HitFlashDriver.cs
**Path:** `Assets/Scripts/Combat/Juice/HitFlashDriver.cs`

MonoBehaviour per-enemy (component on the mob). Bus event'i değil, doğrudan target üzerinden tetiklenir. SpriteRenderer.material.SetColor("_FlashColor", Color.white) 80ms. Shader uniform yoksa property block fallback: `_Color = Color.white` 80ms.

**Note:** Shader entegrasyonu yoksa MaterialPropertyBlock + sprite color override fallback. Kodda her iki yolu da yaz (try shader uniform önce, yoksa color override).

#### 4d. DamageNumberDriver.cs
**Path:** `Assets/Scripts/Combat/Juice/DamageNumberDriver.cs`

MonoBehaviour singleton. OnHit event'inde world-space TextMeshPro spawn at hit position, hasarı yazar, 1.2s fade + upward float 0.5m, pooled. Crit ise font size 1.5x + sarı renk.

TMP yoksa basit `TextMesh` fallback.

### 5. Demo / Test

**Path:** `Assets/Scripts/Combat/Demo/VFXBusDemo.cs`

EditorWindow değil, MonoBehaviour. Inspector'dan tetikleyici button'lar:
- "Publish Hit" → CombatEventBus.PublishHit(new HitEvent { ... })
- "Publish Kill"
- "Publish Dash"
- "Publish CommitBeat (3)"

Kullanım: kullanıcı sahneye atıp Play mode'da test eder.

### 6. asmdef Reference Check

`RIMA.Runtime.asmdef` zaten Unity.ugui + Unity.RenderPipelines.Core.Runtime referansı içeriyor (S80 fix). Yeni eklenen kodların:
- TMP_Pro reference gerekiyorsa asmdef'e ekle (TextMeshPro UI ya da TextMesh, hangisi gerekirse)
- Cinemachine YOK (Shake'i direkt Camera.main ile yap)

Build PASS olmazsa düzelt.

---

## Acceptance Criteria

1. `dotnet build` PASS (0 error, 0 warning)
2. Unity'de hiç compile hatası yok (`mcp__UnityMCP__read_console` Error filtreli)
3. `RIMA.Combat` namespace içinde 7 yeni script: CombatEventBus, VFXRouter, ProcLimiter, HitPauseDriver, ScreenShakeDriver, HitFlashDriver, DamageNumberDriver, VFXBusDemo (8 dosya)
4. Test sahnesi gerekmez — VFXBusDemo prefab Inspector button'ları yeterli
5. Commit message: `[S82][Phase3-VFX] CombatEventBus + VFXRouter + ProcLimiter + Tier 1 primitives`

## Out of Scope (Yapma!)

- StatusMatrix.cs (sonra)
- GroundMarkSystem.cs (sonra — Map Designer pivot ihtiyacı)
- Beat3CommitTrigger.cs entegrasyonu — sadece event bus dinler, mevcut Beat3 dokunma
- Particle prefab içerikleri (Faz 1.5'te art batch)
- Element-spesifik logic (sadece tag routing scaffold)
- Player input entegrasyonu

## Notlar

- Tüm dosyalar `using System;` `using UnityEngine;` `using System.Collections.Generic;` ile başlasın
- `MaterialPropertyBlock` shader fallback için kritik — Built-in / URP 2D Lit shader uniform isimleri farklı, her iki ihtimal de denenecek
- Pool implementation: Queue<GameObject>, instantiate prefab N times, SetActive(false), `Get()` SetActive(true) + position, `Return()` SetActive(false). Lifetime ile auto-return MonoBehaviour coroutine veya basit IEnumerator.
