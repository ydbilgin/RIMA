---
name: map-fragment-canonical-spec
description: RIMA Map Fragment (Kırık Taş Tablet) canonical visual + mekanik spec — NLM source. Drop kuralı (Combat/Elite/Unknown only), pickup feedback, idle anim values, 8-fragment Act 1 boss gate threshold. Demo Faz 1 implementation referans.
metadata:
  type: project
  source: NLM 30ddffa5 2026-05-27 (map_fragment_system.md)
---

## Form + Renk
- **Form:** Kırık Taş Tablet (Broken Stone Tablet)
- **Renk:** Cyan `#00FFCC` (kanonik rift rengi)
- **UI icon (map panel):** 48×48 px (drop sprite boyutu doc'ta spesifik değil — placeholder Faz 1)

## Drop animasyon (0.4s)
- Yerden yükselir + glow pulse yayar
- Tetik: room cleared (last mob death)

## Idle animasyon (hover state)
- **Bobbing:** ±0.10 unit amplitude, 2.2 Hz
- **Alpha pulse:** 0.6 → 1.0 → 0.6, 3 Hz
- **NO rotate** — sadece hover + pulse

## Drop kuralı
| Room type | Fragment drop? |
|---|---|
| Combat | ✅ 1 fragment |
| Elite | ✅ 1 fragment |
| Unknown (combat çıkarsa) | ✅ 1 fragment |
| Rest | ❌ |
| Shop | ❌ |
| Boss | ❌ (boss kapı zaten 8 fragment ile açıldı) |

## Pickup
- **Tuş:** G
- **Proximity:** 2.5 unit radius
- **3-katman feedback:**
  - **VFX:** Cyan beam yukarı 0.3s
  - **SFX:** "Glyph reveal" (rün açığa çıkma)
  - **UI:** HUD "X / 8 fragment" sayacı +1 pulse
- **Tetik:** Hades 3-kart Skill Draft UI açılır + MapPanel +1 hop ileri reveal

## Act 1 Boss Gate threshold
- **Toplam 8 mandatory fragment** (6 combat + 2 elite) → Boss kapı parlar + erişime açılır
- Faz 1 Demo (5 oda) için: 3 combat = 3 fragment. Faz 1 demo'da boss gate threshold = 3 (tweak), Faz 4'te canonical 8.

## Narrative
- "The Fracturing" (Büyük Kırılma) — parçalanmış dünyanın görsel anlatısı
- Mekanik: bilgi = güç (1 hop ileri reveal). Stratejik rota planlaması.
- **Son act'te dönüşüm:** Doc'ta belirtilmemiş (Echoes deneyimine odaklı, fiziksel transformation yok)

## Demo Faz 1 implementation
- Sprite placeholder: cyan parlayan tablet quad veya sphere (Track B'de PixelLab gen)
- Component: `MapFragment.cs` → `Pickup(Player)` → broadcast `OnFragmentPickedUp(int currentCount, int targetCount)`
- Bridge: [[map-fragment-bridge]] (Day 1 naming refactor sonrası MapFragmentBridge)
- Gate unlock: fragment count >= room threshold → gate state machine "unlocked" transition

## Cross-link
[[project-demo-phase1-milestone-lock]] [[gate-socket-canonical-spec]] [[warblade-12-common-skills-spec]]
