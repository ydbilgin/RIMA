---
name: gate-socket-canonical-spec
description: RIMA Gate (kapı) socket canonical sprite + mekanik spec — NLM source. 8 stil variant, room type overlay sistem, lock/unlock state machine, 6-8 frame open anim. Demo Faz 1 placeholder + Track B real sprite production.
metadata:
  type: project
  source: NLM 30ddffa5 2026-05-27 (GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md)
---

## Anti-pattern
- UI floating arrows YASAK
- Fake missing doorway YASAK
- Sahte yok olmuş duvar YASAK

## Kanon stil variant'ları (8)
- Taş kemer (Stone arch)
- Yıkık duvar yarığı (Wall breach)
- Merdiven (Stair)
- Zincirli kapı (Chained doorway)
- Rift threshold
- Asansör (Lift)
- Köprü ağzı (Bridge mouth)
- Tapınak geçidi (Shrine passage)

## Boyut
- **1.5-2x karakter** gameplay zoom'da
- Warblade ~120px → gate ~180-240px

## Composition (Unity, katmanlı)
1. Base gate sprite (neutral socket, boş interior mask)
2. Room type overlay (ışık, ikon, seal — inpaint style fill)
3. Lock/fog overlay (locked state için)
4. Collider + Trigger
5. Optional animated effect layer (rift beam vs)

## Room type → Gate visual
| Room type | Gate stil | Renk/ikon |
|---|---|---|
| Combat (normal) | Taş kemer | Beyaz ikon |
| Elite | Taş kemer + kan izi + mühür | Kırmızı ikon, kırmızı seal |
| Boss | Zincirli + kırık seal + mavi rift ışığı | Altın ikon |
| Shop | Standart base | Altın renk |
| Spirit | Standart | Mor |
| Curse Gate | Standart | Koyu mor |
| Event | Standart | Yeşil |
| Unknown | Standart | Gri "?" ikon |

## Lock state machine
| State | Görsel | Collision |
|---|---|---|
| **Locked (Combat/Elite, kill öncesi)** | Sis + sönük ikon + zincir/mühür overlay + kırık çerçeve | Trigger pasif (player geçemez) |
| **Awaiting Fragment** | Hala sönük + lock overlay, kill bitti ama fragment alınmadı | Trigger pasif |
| **Unlocked** | Açık state, ikon parlak, overlay kalkar | Trigger aktif (player geçer) |
| **Unrevealed (haritada görünmemiş)** | Fog overlay + door silhouette | Trigger pasif |

**Kilit kuralı:**
- Combat/Elite: Giriş anı kilit → kill + fragment pickup → unlock
- Shop/Spirit/Event/Curse: ASLA kilit (free exit)
- Boss: 8 fragment threshold sağlanmadıkça unrevealed

## Animation
- **Open transition:** 6-8 frame first/end interpolate (locked sprite → unlocked sprite)
- **Idle (unlocked):** opsiyonel hafif glow/rift particle

## Prefab disiplini
- Standart oda prefab: **3 potansiyel kapı slot** (N/E/W)
- Kapı önünde **3 tile boş alan** zorunlu
- **DungeonGraph runtime karar** verir hangi slot aktif
- Slot N/E/W mappings: 3 directional iso (N=North, E=East, W=West — S yok çünkü player oradan girer)

## Demo Faz 1 implementation
- **Placeholder visual:** neutral cube + room type color tint (taş kemer placeholder = grey cube)
- **Component:** `Gate.cs` → state machine (Locked/AwaitingFragment/Unlocked) + `OnPlayerEnter()` trigger
- **Open anim placeholder:** scale Y 1.0 → 0.1 lerp 0.4s + alpha fade
- **Track B production:** PixelLab neutral base socket + 8 stil variant + inpaint room type overlay
- **Asset path target:** `Assets/Sprites/Environment/Gates/` (Track B)

## Cross-link
[[project-demo-phase1-milestone-lock]] [[map-fragment-canonical-spec]] [[warblade-12-common-skills-spec]]
