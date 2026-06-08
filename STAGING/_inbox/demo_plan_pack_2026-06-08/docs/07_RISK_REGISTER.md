# 07 — Risk Register

## Risk 1 — Scope şişmesi

Belirti:
- “Bir de Ranger ekleyelim”
- “Boss 3 fazlı olsun”
- “Shop’a 20 item koyalım”
- “Full map graph yapalım”

Çözüm:
- Demo freeze: Warblade + Elementalist.
- 3-4 combat + shop + boss.
- Yeni fikirler backlog’a.

## Risk 2 — UI softlock

Belirti:
- ESC/Codex/Shop/Draft açılınca oyun kilitleniyor.
- TimeScale 0 kalıyor.

Çözüm:
- UIManager tek timeScale sahibi.
- Scene load reset.
- Modal stack basit.
- Her overlay close sonrası Time.timeScale assert.

## Risk 3 — Weapon sorting

Belirti:
- Sword floor/cliff altında.
- Disc kayboluyor.

Çözüm:
- Player SortingGroup.
- Weapon child object.
- Direction profile.
- Rendering test scene.

## Risk 4 — Collider mismatch

Belirti:
- Görsel floor var ama yürünmüyor.
- Void’e yürünüyor.

Çözüm:
- Ground tilemap ve walkability aynı kaynaktan.
- Debug overlay.
- Oda build sonrası invariant test:
  - all floor cells walkable
  - all void cells blocked

## Risk 5 — Boss çok zor veya sıkıcı

Çözüm:
- 3 attack.
- Clear telegraph.
- HP düşük başla, sonra artır.
- İlk boss testinde oyuncu rahat kazansın; sonra sıkılaştır.

## Risk 6 — Shop vakit yer

Çözüm:
- 3 item.
- ScriptableObject ekonomisi sonra.
- Basit apply effects:
  - damage multiplier
  - max HP
  - heal
  - cooldown multiplier

## Risk 7 — Skill data havuzu karışır

Çözüm:
- Demo skill allowlist:
  - Warblade 3 skill
  - Elementalist 3 skill
- isImplemented false olanlar UI/draft’a girmez.
- Null icon fallback.

## Risk 8 — Claude/Codex yanlış eski sisteme patch atar

Çözüm:
- Promptta net yaz:
  - `RoomLoader`, `RuntimeRoomManager`, `DoorTrigger`, `GateBehavior` legacy.
  - Aktif path: `_Arena`, `RoomRunDirector`, `IsoRoomBuilder`.
- Patch hedefi canlı dosyalar.
