# Camera Lock — HARD RULE (S103 2026-05-23)

**Kural:** HD-2D oyun kamerası sabit. Hiçbir scene, hiçbir codex task, hiçbir agent şu değerleri DEĞİŞTİRMESİN.

## Lock değerleri (Main Camera)

| Property | Value |
|---|---|
| Position | `(12, 8, -12)` |
| Rotation (Euler) | `(35, 315, 0)` — yaw 315 = -45° |
| Projection | Orthographic |
| Orthographic Size | `9` |
| Near Clip | `0.3` |
| Far Clip | `50` |
| Background Color | `(0, 0, 0, 1)` black |
| Clear Flags | SolidColor |

**Why:** ChatGPT_ref aesthetic + S103 user feedback (2026-05-23): "açımız aynı kalacak onu sabitleyelim". Drift olursa tüm sahnelerin görünümü tutarsızlaşır.

## How to apply

1. **CameraRig_HD2D.prefab** — `Assets/Prefabs/Camera/CameraRig_HD2D.prefab` (will be created)
2. **CameraLockController.cs** — `Assets/Scripts/Camera/CameraLockController.cs` (will be created) — LateUpdate'de değerleri zorla sıfırla, drift detect → log warning
3. Tüm yeni scene'lerde CameraRig_HD2D prefab kullanılsın
4. Mevcut SampleScene + RoomShowcase + RoomChainShowcase Main Camera'ları bu değerlere reset edilsin

## Codex / agent dispatch'inde her zaman ekle

```
CAMERA LOCK: Do NOT modify Main Camera transform, projection, or ortho size.
Position (12, 8, -12), Rotation (35, 315, 0), Orthographic size 9.
If task needs different framing, instantiate a SECONDARY camera with Tag=Untagged — never touch Main Camera.
```

## İstisnalar — sadece açık izin verildiğinde

- Cinematic moments (cutscene)
- Boss reveal close-up
- Map overview UI
- Debug/dev cameras (Tag=Untagged, disabled by default)

Tüm bu istisnalar TAGGED ayrı kamera ile yapılır, Main Camera'ya dokunulmaz.

## Future ScreenShake / FollowCam

Mevcut Lock pattern: CameraLockController LateUpdate'de transform'u zorlar. Player follow için bir CameraTarget GameObject + Main Camera parent yapısı kurulabilir — Main Camera local transform sabit kalır, parent CameraTarget hareket eder. Bu önerilen pattern.
