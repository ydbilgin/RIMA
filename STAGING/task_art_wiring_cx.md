ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>". Direct-read: code/scenes/STAGING/memory.

Amaç: Üretilen gerçek art'ı (portal, chasm, passage) 3 map sahnesindeki placeholder objelere wire et + reward-drop sistemi kur. Sadece MOB'lar placeholder kalır. Unity açık, UnityMCP profilinden eriş.

PIVOT/IMPORT ZATEN YAPILDI (sprite'lar hazır):
- portal: Assets/Sprites/Environment/Portal/portal_rift.png (pivot 0.5,0)
- chasm:  Assets/Sprites/Environment/Props/chasm.png (pivot 0.5,0.5 flat)
- passage: Assets/Sprites/Environment/Props/narrowpassage.png (pivot 0.5,0)
- reward: Assets/Sprites/Environment/Reward/reward_relic.png (pivot 0.5,0)
- column: Assets/Sprites/Environment/Props/stonecolumn.png — HENÜZ YOK (gen sürüyor). VARSA wire et (pivot 0.5,0 bottom), YOKSA StoneColumn'u placeholder bırak + raporla.

KRİTİK WIRE RECIPE (Opus _IsoGame portal'da bunu keşfetti — placeholder objeler PlaceholderSprite ile SR'yi DISABLE eder + Default sorting layer'da bırakır):
Her hedef obje için: (1) PlaceholderSprite component'ini DestroyImmediate, (2) SpriteRenderer.enabled=true, (3) sr.sprite=<sprite>, (4) sr.color=Color.white, (5) sorting layer: STANDING objeler (DoorNorth, StoneColumn, NarrowPassage) → "Entities"; FLAT objeler (Chasm) → "Decals"; (6) sortingOrder=0 (custom-axis pivot sort).

## ADIM 1 — Wire across ALL 3 scenes
Sahneler: Assets/Scenes/_IsoGame.unity, _IsoGame_Map02.unity, _IsoGame_Map03.unity. Her sahneyi aç (manage_scene load), recipe ile şu objeleri wire et (her isimden TÜM instance'lar):
- DoorNorth → portal_rift (Entities). NOT: _IsoGame'de portal ZATEN wire edildi (atlayabilirsin ama kontrol et); Map02/Map03'te yap.
- Chasm → chasm (Decals, flat hole).
- NarrowPassage → narrowpassage (Entities).
- StoneColumn → stonecolumn EĞER dosya varsa (Entities); yoksa atla.
Her sahneyi KAYDET. Floor/IsoGrid/cliff/mob'lara DOKUNMA (mob placeholder kalır).

## ADIM 2 — Reward-drop sistemi
Oda temizlenince (RoomClearVictoryTrigger, `Assets/Scripts/Core/RoomClearVictoryTrigger.cs`) reward düşsün:
- Gate açılırken (mevcut OnAllCleared yolunda), oda merkezine (veya serialized spawnPoint, yoksa player'ın bulunduğu odanın ortası ~ floor merkezi) bir RewardPickup GameObject Instantiate et: SpriteRenderer(reward_relic sprite, "Entities" layer, pivot bottom) + küçük CircleCollider2D isTrigger.
- YENİ küçük `Assets/Scripts/Core/RewardPickup.cs` (namespace RIMA): OnTriggerEnter2D Player tag → Debug.Log("[Reward] collected") + (varsa RunStats'a basit reward sayacı; yoksa sadece log) + Destroy(gameObject). Minimal V1.
- reward_relic sprite path yukarıda. Standalone _IsoGame (MapFlowManager yok) yolunda da reward düşmesi opsiyonel — basit tut.

## ADIM 3 — VERIFY
read_console 0 error, compile temiz. CODEX_DONE.md'ye: hangi sahnelerde hangi objeler wire edildi, column durumu (wired/pending), reward-drop kuruldu mu, console/compile PASS/FAIL.

NOT: Player/mob'a dokunma. Sorting layer'lar mevcut (Default,Ground,Floor,Decals,Walls,Entities,VFX,UI,...). RewardPickup'ı animate ETME (still).
