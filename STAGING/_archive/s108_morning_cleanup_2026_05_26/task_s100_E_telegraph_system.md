ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: 3-Kanal Düşman Telegraph Sistemi — Erişilebilirlik + Combat Feel

Düşman saldırı öncesi 3 kanalda bildirim: (1) outline pulse, (2) screen shake, (3) 3. kanal (UI/zemin işareti). Accessibility feature. Mevcut enemy prefab'lara attach edilecek.

## NLM Context Çek

Başlamadan önce:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "3-kanal telegraph sistemi detayları, outline pulse screen shake accessibility düşman saldırı uyarı, mevcut CombatEventBus VFXRouter yapısı"
```

## Mimari

### Channel 1 — Outline Pulse
- `EnemyOutlinePulse.cs` — MonoBehaviour
  - Mevcut outline/selection shader varsa kullan (MaterialPropertyBlock veya Shader.SetFloat)
  - TelegraphStart(float duration) → outline alpha 0→1→0 pulse, renk kırmızı
  - Yoksa fallback: SpriteRenderer.color tint pulse

### Channel 2 — Screen Shake (CombatEventBus üzerinden)
- Mevcut `CombatEventBus` varsa event fire et (NLM'den öğren dosya yolunu)
- Yoksa: `CameraShake.cs` — Cinemachine impulse veya transform nudge
- TelegraphStart → küçük anticipation shake (magnitude 0.05, 0.3s)

### Channel 3 — Ground Marker
- `TelegraphGroundMarker.cs` — MonoBehaviour
  - Düz renkli daire/kare sprite (built-in, beyond Sprite veya primitive)
  - Saldırı AoE alanını gösterir, alpha pulse, kırmızı
  - TelegraphStart(float duration, float radius) → marker show, TelegraphEnd → hide

### Koordinatör
- `EnemyTelegraph.cs` — MonoBehaviour, ana koordinatör
  - [SerializeField] EnemyOutlinePulse outlinePulse
  - [SerializeField] TelegraphGroundMarker groundMarker
  - bool channelScreenShake = true
  - public void StartTelegraph(float duration, float aoeRadius) — 3 kanalı tetikler
  - public void CancelTelegraph() — erken iptal

### File Paths (CREATE)
- `Assets/Scripts/Enemy/Telegraph/EnemyTelegraph.cs`
- `Assets/Scripts/Enemy/Telegraph/EnemyOutlinePulse.cs`
- `Assets/Scripts/Enemy/Telegraph/TelegraphGroundMarker.cs`
- (Yoksa) `Assets/Scripts/Combat/CameraShake.cs`

## Başarı Kriterleri

- [ ] Compile errors yok
- [ ] EnemyTelegraph Inspector'da görünür, 3 channel ref assign edilebilir
- [ ] EnemyTelegraph.StartTelegraph(1f, 1f) çağrılabilir
- [ ] Console'da hata yok
