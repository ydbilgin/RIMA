---
status: LOCKED
faz: 1
tarih: 2026-05-09
ozet: "Map fragment + Kırık Taş Tablet sistemi (Karar #63)"
---
# Map Fragment Sistemi
**Tarih: 2026-05-09 | Durum: LOCKED v1 spec | Kaynak: S46 user requirement**

## Konsept

Dusmanlari temizledikten sonra her odada zeminden minik bir harita parcasi duser.
Topladikca onundeki 1-2 odanin **tipini** gorursun -- oda kuralinin (Combat/Elite/Shop/Curse)
ne oldugu belli, ama icindeki dusmanlar/oduller belirsiz. Bu sayede "siraladaki Elite oncesi
su skill'i alayim" diye **build planlamasi** yapabilirsin. StS2-style haritada
goruntue dayali strateji.

## Drop Mekanigi

### Tetikleyici

- Combat / Elite / Unknown (Mystery 6b) odasi temizlendiginde
- Tum dusman olu -> reward sequence:
  1. Dusman olum efekti
  2. Map fragment zemine duser (hover bobbing + cyan glow)
  3. Pickup zorunlu -- alinmadan kapi acilmaz
  4. Pickup -> Skill Draft 3-choice ekrani

### Fragment kaynagi node-by-node (Act 1)

| Node | Fragment | Not |
|---|---|---|
| 1 Entry | -- | tutorial node |
| 2 Combat | + | zorunlu |
| 3 Combat | + | zorunlu |
| 4 Rest | -- | combat-yok, fragment-yok (LOCKED) |
| 5 Elite | + | zorunlu |
| 6 Combat | + | zorunlu |
| 6b Mystery | +* | %50 event kazanirsa |
| 7 Shop | -- | kapi kilidi yok, fragment yok |
| 8 Combat | + | zorunlu |
| 9 Rest | -- | -- |
| 9b Curse Gate | +* | Burden kabul edilirse opsiyonel |
| 10 Combat | + | zorunlu |
| 11 Elite | + | zorunlu |
| 12 Combat | + | zorunlu |
| 13 Boss | -- | boss key ayri akis |

**Toplam zorunlu**: 8 fragment (combat 6 + elite 2).
**Opsiyonel bonus**: max 2 (6b + 9b).
**Boss kapisi**: 8 zorunlu fragment toplandigi zaman otomatik acilir.

## Reveal Sistemi (StS2-style)

### Gorunurluk kurali

| Durum | Gorunurluk |
|---|---|
| Mevcut node | Tam gorunur (parlak, icerik biliniyor) |
| Bitisik node (kapi acik) | Oda **tipi ikonu** gorunur (kilic=Combat, tac=Elite, cekic=Shop, vs.) |
| 2 node ileri | "?" siluet, tip belirsiz |
| 3+ node uzak | Hic gorunmez |
| Boss node | Her zaman gorunur (depth 0'dan boss silüeti map alt-merkezde) |

### Fragment toplama -> reveal genisleme

- **Pickup tetikleyici**: Bitisik (current+1) node revealed kalir +
  **bir adim daha ilerideki (current+2) node'un tipi acilir**.
- **Standart**: 1 ileri tipi acik. Pickup ile +1 daha -> toplam 2 ileri.
- **Boss reveal**: Map alt-merkezde her zaman siluet. Tum 8 fragment toplandigi zaman
  boss kapisi icon parildar.
- **Dal node visibility**: Mystery (6b) ve Curse Gate (9b) ana hat node'lardan gorunur
  ama "?" tipte olabilir (dramatic discovery).

### Build Planning UX

- TAB key -> MapPanel acilir (StS-style abstract graph, ekranin merkez %70'i)
- Edges = ince isik cizgisi, current parlak, visible nodes net, "?" siluet
- Node icon hover -> "Combat (Bruiser+2 trash composition gozukebilir)" tooltip
  -- sadece tip + threat tier (mob detayi verme; surpriz koru)
- Skill Draft sirasinda MapPanel TAB ile bakilabilir (skill secimi sirasinda planlama)

### MiniMap (sol-ust kose surekli)

- Hades-style: SADECE mevcut oda + kapi yonleri (graph DEGIL layout)
- Boyut: 128x128px
- Icerik: oda siluet + kapi oklar + N/E/S/W yon

## Cift Layer UI Karari

| UI | Konum | Icerik | Tetikleyici |
|---|---|---|---|
| **MapPanel (graph)** | Ekran merkez %70 | StS abstract graph (15 node + edges) | TAB key |
| **MiniMap (layout)** | Sol-ust 128x128 | Mevcut oda layout + kapi | Surekli acik |

Iki paradigma birden calisir: macro planlama (MapPanel) + micro navigation (MiniMap).

## Fragment Pickup Visual + Audio

- Drop animasyon: zeminden 0.4s'de yuksel + glow pulse
- Bobbing: +-0.10u amplitude, 2.2 hz
- Glow color: cyan (#00FFCC), alpha pulse 0.6-1.0 @ 3hz
- Pickup proximity: 2.5u radius
- Interact key: G
- Pickup VFX: cyan beam yukselisi 0.3s + glyph reveal SFX
- HUD counter: ekran ust-orta "X / 8 fragment" yazisi, fragment her toplanisinda +1 pulse

## Implementation Hooks

- `Assets/Scripts/Core/MapFragment.cs` -- pickup behavior (drop, bob, glow, interact)
- `RoomLoader.OnRoomCleared` event -> MapFragment instantiate
- `DungeonGraph` -- node visibility flags (`isVisible`, `revealLevel: int 0/1/2`)
- `MapPanel.cs` (yeni) -- TAB key listener, abstract graph render
- `MiniMap.cs` (mevcut, rebuild gerekebilir) -- current room layout
- `HUDController.fragmentCounter` -- top-center counter UI

## Acik Sorular

- Mystery (6b) %50 event-no-fragment durumu: oda clear etmeden cikis var mi?
  (Curse Gate gibi kilit yoksa.) v2'de netlestir.
- Curse Gate fragment kosulu: Burden kabul edilince mi yoksa odadan cikinca mi?
  Burden tetikleyici daha tutarli (oyuncu commitment bekleniyor). v2'de netlestir.

## Extracted from STALE memory (S91 2026-05-18)
### Source: project_map_system.md

**Node visibility state names (DungeonMapUI implementation reference):**

| State | Description |
|---|---|
| `visited` | Checked icon |
| `current` | Pulsing icon |
| `step1` | Revealed (icon visible, 1 node ahead) |
| `step2` | Dark "?" icon (2 nodes ahead) |
| `missed` | Unselected/skipped branches — dimmed to 40% brightness |

**Reward Cleanup rule:** Purge `activeReward` list on `StartRoom` (prevents stale loot from prior run persisting).

**Note:** Memory recorded "M key = map toggle" — LIVE spec uses TAB key (map_fragment_system.md canonical). M key historical only.

---

## v2 / v3'e Birakilan

- Spirit node ekleme (v2 onaydan sonra)
- Wild rarity etkisi loot pool genislemesi (v3)
- 5 Rift Portal turu Burden+Gift content cesitliligi (v3)
- Tile Room Memory Overlay (v2 -- DepthBandTileSet hookup sonrasi)

