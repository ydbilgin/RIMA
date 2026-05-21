# Codex Task — Pattern Library v1 (5 Utility Classes + UV Shader)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

---

## Görev

5 Unity utility class + 1 URP 2D UV scroll shader implement et. Bu library hard-skill mekaniklerinin (chain/tether, multi-jump, projectile fan, placed effect, afterimage) generic foundation'ı.

## Read These Files (input)

1. `STAGING/_research/HARD_SKILL_PIXELLAB_FEASIBILITY_v2.md` — 6 industry pattern + per-pattern Unity setup + C# kod örnekleri (orchestrator yazdı)
2. `STAGING/_plans/progression/PROGRESSION_PLAN_v2_3_LOCK.md` Section 6.1-6.10 — implementation roadmap

## Output Files (yaz)

### 1. ChainBinder.cs
Path: `Assets/Scripts/Combat/Utilities/ChainBinder.cs`

Generic caster-target dinamik tether/chain/beam binder.

Public API:
```csharp
public class ChainBinder : MonoBehaviour {
    public void Bind(Transform caster, Transform target, ChainBinderConfig config);
    public void Unbind();
}

[Serializable]
public class ChainBinderConfig {
    public Material chainMaterial;
    public float width = 0.3f;
    public float scrollSpeed = 2f;
    public Color tint = Color.white;
    public float duration = 3f;
    public int sortingOrder = 5;
}
```

Internal:
- LineRenderer 2-point (caster + target)
- Update: pozisyonları her frame güncelle, distance'a göre texture scale ayarla
- Lifetime expire → destroy GameObject
- Anchor offset opsiyonel (caster üst noktası, target göğüs vb.)

Kullanılacak skill: Shackle Curse, Spirit Bind, Tethering Arrow, Pact Drain, Soul Drain.

### 2. SequentialStrike.cs
Path: `Assets/Scripts/Combat/Utilities/SequentialStrike.cs`

N-target sıralı VFX spawn + damage callback.

Public API:
```csharp
public class SequentialStrike : MonoBehaviour {
    public IEnumerator StrikeChain(Vector3 origin, SequentialStrikeConfig config);
}

[Serializable]
public class SequentialStrikeConfig {
    public GameObject strikePrefab;
    public int maxJumps = 3;
    public float jumpDelay = 0.08f;
    public float searchRadius = 4f;
    public LayerMask enemyMask;
    public float damage = 25f;
    public Action<Transform> onHit;
    public bool showJumpLines = true;
    public Material jumpLineMaterial;
}
```

Internal:
- Physics2D.OverlapCircleAll ile nearest unhit enemy bul
- Coroutine ile jump delay
- Brief LineRenderer flash between jumps (0.1s)
- HashSet ile hit edilen düşmanları izle

Kullanılacak: Chain Lightning, Chain Cull, Hex Bolt (multi-target variant).

### 3. ProjectileFanSpawner.cs
Path: `Assets/Scripts/Combat/Utilities/ProjectileFanSpawner.cs`

Arc/spread projectile spawn.

Public API:
```csharp
public class ProjectileFanSpawner : MonoBehaviour {
    public void SpawnFan(Transform origin, Vector3 aimDirection, ProjectileFanConfig config);
}

[Serializable]
public class ProjectileFanConfig {
    public GameObject projectilePrefab;
    public int projectileCount = 5;
    public float spreadAngleDegrees = 30f;
    public float projectileSpeed = 12f;
    public float spawnIntervalSeconds = 0f; // 0 = simultaneous
}
```

Kullanılacak: Sweep Volley, Multi Shot, Fan The Hammer, Twin Fire.

### 4. PlacedEffectSpawner.cs
Path: `Assets/Scripts/Combat/Utilities/PlacedEffectSpawner.cs`

Trap/sigil/totem yerleştirme + lifetime + trigger.

Public API:
```csharp
public class PlacedEffectSpawner : MonoBehaviour {
    public GameObject Place(Vector3 position, PlacedEffectConfig config);
    public GameObject PlaceOnEnemy(Transform target, PlacedEffectConfig config);
}

[Serializable]
public class PlacedEffectConfig {
    public GameObject effectPrefab;
    public float lifetime = 5f;
    public bool parentToTarget = false;
    public TriggerType triggerType = TriggerType.OnEnter;
    public float triggerRadius = 0.5f;
    public float damage = 0f;
    public Action<Transform> onTrigger;
}

public enum TriggerType { OnEnter, OnExit, Continuous, OnTimer }
```

Kullanılacak: Bone Trap, Explosive Trap, Wireline Trap, Living Bomb, Frozen Orb, Shadow Pin, Curse Mark, Death Mark, Body Lock.

### 5. AfterimageTrail.cs
Path: `Assets/Scripts/Combat/Utilities/AfterimageTrail.cs`

Dash/teleport afterimage spawn.

Public API:
```csharp
public class AfterimageTrail : MonoBehaviour {
    public void SpawnTrail(Transform source, AfterimageConfig config);
}

[Serializable]
public class AfterimageConfig {
    public int afterimageCount = 5;
    public float intervalSeconds = 0.05f;
    public float fadeDuration = 0.4f;
    public Color tintColor = new Color(0.5f, 0.5f, 1f, 0.5f);
    public Material afterimageMaterial; // shader supports tint + alpha fade
}
```

Internal:
- Coroutine spawn N times: sprite kopya + Material instance + fade tween
- Material shader = Sprites/Default + transparency tint

Kullanılacak: Blink, Phase Step, Shadow Step, Vanish, Disengage, Hunters Step, Blade Rush, Iron Charge.

### 6. UVScroll2D.shader
Path: `Assets/Shaders/UVScroll2D.shader`

URP 2D unlit + UV scroll + color tint shader for ChainBinder.

```hlsl
Shader "RIMA/UVScroll2D" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ScrollSpeed ("Scroll Speed", Float) = 2.0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 pos : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            sampler2D _MainTex;
            float4 _Color;
            float _ScrollSpeed;

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.pos.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                float2 uv = IN.uv;
                uv.x += _Time.y * _ScrollSpeed;
                return tex2D(_MainTex, uv) * _Color;
            }
            ENDHLSL
        }
    }
}
```

## Kısıt

- Sadece bu 6 dosyayı yaz, başka şeye dokunma
- Per-class XML doc comment (1-2 satır summary)
- Inline comment SADECE non-obvious logic için (örn. coroutine timing rationale)
- Pooling YOK Phase 1 (Instantiate + Destroy yeterli prototype için)
- SFX hook'ları yok Phase 1, sadece visual + damage
- Test scene oluştur: `Assets/Scenes/Test/PatternLibraryTest.unity` — 5 button her pattern'i test eder (dummy target Transform'a)
- `dotnet build` veya Unity compile check sonunda 0 error/warning

## Compile Check

Tüm dosyaları yazdıktan sonra Unity console kontrol et — kompilasyon hatası olmamalı. Hata varsa fix et + tekrar check.

## Effort
high
