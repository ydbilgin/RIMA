# Opus Playtest Notları — Faz 4 re-test (2026-06-13)

**Model:** Opus 4.8 (orchestrator). **Yöntem:** Unity MCP `execute_code` + DirectorMode `*ForValidation` API (data-proof). Tool UI screenshot YOK (ScreenSpaceOverlay MCP'de çıkmıyor) — sayısal kanıt kullanıldı.

## Sonuçlar — HEPSİ PASS ✅
| Test | Sonuç | Beklenen | PASS? |
|---|---|---|---|
| Prop seç (rift_crystal) | `SelectFirstPropForValidation=True` | true | ✅ |
| Prop yerleştir ×2 | `count=2` | 2 | ✅ |
| Prop sil ×1 | `erase=True, count=1` | 1 | ✅ |
| Spawn cap | 12 spawn → `count=10` | ≤10 | ✅ |
| Stat hook (geçerli) | `SetStatForValidation("physPower",200)=True` | true | ✅ |
| Stat hook (geçersiz) | `badKey=False` | false | ✅ |
| Class seç | `Warblade=OK` (throw yok) | ok | ✅ |

## Gameplay screenshot
`faz4_world_props_spawns.png` — arena render OK. ⚠️ Yerleştirilen prop (3,1)'de küçük (edge_filler shard sprite), spawn'lar y=-4 viewport dışında → görselde net görünmüyor ama data-proof kesin.

## Flag'ler (yarın kullanıcı + olası fix)
1. **rift_crystal `#if UNITY_EDITOR`** AssetDatabase ile yükleniyor → editör-play'de ✓ ama **standalone build'de prop palette BOŞ olur.** Build alınacaksa Resources'a taşı veya directorPlaceableProps'a serialize et. (DirectorMode self-bootstrap olduğu için Inspector'da atama da yok → tek kaynak editör yolu.)
2. **rift_crystal sprite küçük + ışık ayarı** — demo görünürlüğü için sprite büyütme / Light2D intensity-radius ince ayarı gerekebilir (gözle, yarın).
3. **Asset drift** — git'te cx'in raporlamadığı PropPool×5 + Profile + TMP font (9KB→536KB) değişikliği var (Unity import yan etkisi). Revert edilmedi (kullanıcı kararı). Oda dekorasyonunu etkileyebilir → kontrol et.
4. **13 EditMode fail** — DirectorMode değişikliği DIŞI (STAGING asset, MCP imza, PrefabHealth, SubRoom). Targeted DirectorMode test 3/3 yeşil. Clean-baseline doğrulaması yarın opsiyonel.

## Council review (Faz 3)
ax Opus 4.6 + Opus 4.8: **PASS** (0 blocker, 0 major). Kod Spawn pattern'ini birebir aynalıyor, surgical, clamp/cap/guard doğru.
