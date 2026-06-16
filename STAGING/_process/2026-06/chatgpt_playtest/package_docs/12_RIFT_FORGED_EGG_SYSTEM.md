# Rift-Forged Egg ve Ortadaki World Reward Objeleri

## Tasarım kararı

`Rift-Forged Egg`, yeni ve ayrı bir ekonomi sistemi olmak zorunda değil. Demo için en sağlıklı kullanım: **mevcut reward tiplerinden birini diegetic olarak sunan mystery container / world reward skin**.

Böylece oyuncuya yeni bir “egg currency, incubation inventory, pet economy” öğretmek zorunda kalmazsın. RIMA zaten skill, passive, item, component, relic, Echo Imprint ve map sistemleri taşıyor. Bir kategori daha eklemek, solo geliştiricinin kendine karşı açtığı savaş olur.

## Önerilen işlev

- `Rift-Forged Egg`: içeriği tamamen veya kısmen gizli reward container.
- Kaynağa göre mevcut bir reward açar:
  - Combat: Skill/passive/upgrade offer
  - Elite: Rare+ offer veya component
  - Unknown/Event: hidden reward
  - Boss: kullanılmamalı; boss ödülü daha özel sunulmalı
- Egg envantere gitmez; dünya üzerinde etkileşir ve aynı odada açılır.

## 3'lü dünya seçimi

Ekrandaki `Reckoning Shard / Rift-Forged Egg / Vitality Crystal` gibi objeler bir `WorldRewardChoiceSet` içinde olmalı.

```text
ChoiceSet
  Choice A: known reward
  Choice B: mystery egg
  Choice C: recovery reward
```

Birini seçince:
1. Seçilen obje pulse olur.
2. Confirm kısa UI açılır.
3. Reward uygulanır veya egg hatch ile reveal edilir.
4. Diğer iki obje cyan crack/shatter ile kaybolur.
5. Session tamamlanır.
6. Kapılar açılır.

## Visual states

1. `Dormant`: koyu basalt shell, çok hafif cyan seam.
2. `Proximity`: ground ring ve isim plate görünür.
3. `Focused`: crack pulse + küçük hover rise.
4. `Inspecting`: shell durur; UI title/category/risk gösterir.
5. `Hatching`: crack genişler, shell parçaları dışa açılır.
6. `Claimed`: reward icon/beam çıkar, sonra temizlenir.
7. `SiblingRejected`: diğer seçenekler sönüp fracture dust olur.

## Modüler art parçaları

- `egg_shell_base_96.png`
- `egg_crack_stage_01..04.png`
- `egg_inner_glow_mask.png`
- `egg_ground_shadow.png`
- `egg_ground_ring.png`
- `egg_focus_outline.png`
- `egg_shell_fragments_01..08.png`
- `egg_hatch_flash.png`
- `egg_rarity_socket.png`
- `world_reward_title_plate_9slice.png`
- `interaction_keycap_G.png` veya runtime TMP/key icon

Shell ve glow ayrı olmalı. Böylece rarity/class/biome rengi shader/material ile değiştirilebilir.

## Animasyon

Native 96×96 veya 128×128:
- Idle: 6 frame, 0.8–1.2s loop; yalnız 1–2px breathing/pulse.
- Focus: 4 frame crack intensity.
- Hatch: 10–12 frame; anticipation 3, break 2, fragments 4, reveal 2.
- Reject: 6–8 frame dissolve/shatter.

Egg hareketli canlı gibi zıplamamalı. Ağır, ritüel obje hissi vermeli.

## Interaction UX

Yakında:
```text
RIFT-FORGED EGG
G — İNCELE
```

Inspect:
```text
BİLİNMEYEN RIFT ÖDÜLÜ
Nadir veya daha iyi bir ödül içerir.
G — AÇ    ESC — GERİ
```

Risk varsa gizleme:
```text
Bir ödül ve olası bir Rift Bedeli içerir.
```

## Teknik model

`RewardDefinition` ScriptableObject:
- ID
- reward type
- rarity
- display icon/title
- known/hidden flags
- apply strategy
- combo metadata
- world presentation prefab

`WorldRewardChoice` yalnız presentation + session handle taşır. Reward'ın gerçek datasını prefab içine gömme.

## İleri faz alternatifi: incubation

Gerçek incubation/pet companion sistemi ancak core demo stabil olduktan sonra düşünülebilir. O durumda egg 2–3 oda boyunca taşınır, alınan damage/skill usage ile şekillenir ve companion/relic hatch eder. Bu ayrı tasarım ve balance sistemi olur; mevcut playtest bugları çözülmeden yapılmamalı.
