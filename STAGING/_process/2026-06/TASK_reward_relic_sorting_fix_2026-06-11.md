ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# TASK: reward_relic_gen.png — Floor Altında Kalıyor (Sorting Fix)

## SORUN
`Assets/Sprites/Reward/reward_relic_gen.png` kullanılan reward objesi floor tile'larının altında kalıyor — görünmüyor veya kısmen gizleniyor.

## ARAŞTIR
1. Reward prefab'ı bul: `Assets/Prefabs/Rooms/Act1/reward_01.prefab` veya `Assets/Prefabs/Rooms/Act1/Chest/reward_01.prefab`
2. SpriteRenderer'ının Sorting Layer ve Order in Layer değerlerini kontrol et
3. Floor tile'larının Sorting Layer/Order değerlerini bul (Tilemap veya floor SpriteRenderer)
4. Sorting Layer listesini bul: Project Settings → Tags & Layers

## YAPILACAK
- Reward SpriteRenderer'ının Sorting Layer'ını floor'dan üst bir layer'a al (örn. "Props" veya "Objects")
- Veya aynı layer'daysa Order in Layer'ı floor'dan yüksek yap (floor 0 ise reward 10+)
- Prefab'ı kaydet
- Eğer reward bir script ile spawn ediliyorsa (Grep: RewardSpawner, RewardPickup, reward_relic) orada da sorting ayarı olup olmadığını kontrol et

## BAŞARI KRİTERİ
- reward_relic_gen sprite floor üzerinde görünür
- Diğer prop/enemy sorting'i bozulmamış
- Compilation error yok
