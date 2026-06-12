# RIMA — Harita Sistemi + UI Asset Brief (ChatGPT için)

---

## BÖLÜM 1: KOŞU YOLU — Dallanma Haritası (STS2 Tarzı)

### Mevcut durum
M tuşuna basınca açılan harita şu an düz dikey liste görünüyor:
```
5: Combat
4: Boss
3: Combat
2: Merchant
1: Combat
0: Combat  ← şu anki oda (cyan)
```

### İstenen davranış
Her run'da farklı, Slay the Spire 2 (STS2) tarzı dallanan harita.

### Sistem nasıl çalışıyor (Python pseudocode)

```python
# Harita = ağaç yapısı (tree)
# depth = satır (yukarı = ilerisi), lane = sütun (yatay konum)

class Node:
    id: int
    depth: int          # 0 = başlangıç, 5 = boss
    lane: int           # -2, -1, 0, 1, 2 (yatay konum)
    room_type: RoomType # Combat / Elite / Merchant / Boss / Chest / Forge / Event
    children: List[int] # bu node'dan gidebileceğin sonraki node ID'leri
    visited: bool
    revealed: bool

class RoomType(Enum):
    Combat   = "#4A5260"  # gri-mavi
    Elite    = "#6B2A2A"  # koyu kırmızı
    Boss     = "#8B1A1A"  # parlak kırmızı
    Merchant = "#2A6B5A"  # teal
    Chest    = "#6B5A2A"  # altın
    Forge    = "#5A3A6B"  # mor
    Event    = "#3A5A6B"  # mavi

def generate_map(seed: int, depth_count: int = 6):
    random.seed(seed)  # her run farklı ama tekrar oynanabilir (seed'den aynı)
    nodes = []
    
    # Her depth katmanında 1-3 node üret
    for depth in range(depth_count):
        if depth == 0:
            node_count = 1  # başlangıç tek node
        elif depth == depth_count - 1:
            node_count = 1  # boss tek node
        else:
            node_count = random.choice([1, 2, 3])  # 1-3 paralel yol
        
        for lane_i in range(node_count):
            node = Node(
                depth=depth,
                lane=lane_i - node_count // 2,  # merkeze hizala
                room_type=pick_room_type(depth)
            )
            nodes.append(node)
    
    # Node'ları birbirine bağla (her node → sonraki depth'teki 1-2 node'a)
    connect_nodes(nodes)
    return nodes

def pick_room_type(depth: int) -> RoomType:
    if depth == 0: return RoomType.Combat       # her zaman combat başlar
    if depth == 5: return RoomType.Boss         # son node her zaman boss
    
    # Ağırlıklı random
    weights = {
        RoomType.Combat:   55,
        RoomType.Elite:    15,
        RoomType.Merchant: 15,
        RoomType.Chest:    10,
        RoomType.Event:     5,
    }
    return weighted_random(weights)
```

### Görsel düzen kuralları

```
Ekranda nasıl çizilir:

depth 5 (üst):     [Boss]
                     |
depth 4:          [Combat]
                  /        \
depth 3:     [Elite]     [Merchant]
               |               |
depth 2:     [Combat]    [Combat]
                  \        /
depth 1:          [Combat]
                     |
depth 0 (alt):    [Combat]  ← cyan border = şu anki konum

- Her node: yuvarlak köşeli dikdörtgen (~120×44 px)
- Node rengi: room type'a göre (yukarıdaki enum renkleri)
- Bağlantı çizgisi: kesik noktalı, gri (#505560)
- Mevcut oda: cyan (#00FFCC) parlak border
- Ziyaret edilen: tam dolu renk
- Görünür ama gidilmemiş: %60 opacity
- Henüz reveal olmamış: siyah/gizli
```

### Mockup isteği
Lütfen çiz:
1. **6 depth, 2-3 paralel dal** örnek harita (STS2 gibi, dikey, üstte boss)
2. Node tiplerine göre renk kodlaması
3. Mevcut oda = cyan border vurgusu
4. Node'lar arası kesik çizgi bağlantı

---

## BÖLÜM 2: Eksik UI Assetleri — Üretim İsteği

### Gerekli assetler (Opus analizi — gerçek eksikler)

#### SET A: Rarity Ribbon (3 adet)
Reward kartlarında kartın üstünde şerit olarak çıkacak.

```
Boyut: 112 × 28 px (her biri)
Format: PNG, şeffaf arkaplan
PPU: 64
Filter: Point (pixel art)

reward_rarity_ribbon_common  → #8A9098 gri,      "COMMON" yazı
reward_rarity_ribbon_rare    → #1B7BA8 mavi-cyan, "RARE" yazı
reward_rarity_ribbon_epic    → #7B3FA8 mor,       "EPİK" yazı

Stil: pixel art, düz renk + hafif highlight, kenarlar sert
Sol ve sağda küçük diamond ◆ süs
Yazı: pixel font, ortalanmış, beyaz
```

#### SET B: Minimap Markerlar (3 adet)
Sağ üst minimap'te oda ve oyuncu işaretleri.

```
Boyut: 20 × 20 px (player), 16 × 16 px (room tile), 12 × 12 px (door)
Format: PNG, şeffaf arkaplan

minimap_player_marker  → parlak cyan (#00FFCC) üçgen/ok
minimap_room_tile      → slate (#3A3D42) dolu kare
minimap_door_marker    → ember (#E89020) küçük boşluk/çizgi

Stil: çok basit pixel shape, net okunabilir küçük boyutta
```

#### SET C: Minimap Frame (1 adet)
```
Boyut: 280 × 220 px
Format: PNG, 9-slice hazır (18px border)
İç alan şeffaf (harita buraya çizilecek)

Stil: void mor (#3A1A4A) arka plan, slate (#22272D) taş border,
      ember (#E89020) köşe vurgusu, pixel art köşe ornament
```

### Üretim kuralları (HER prompt'a ekle)
```
- Pixel art style, top-down 2D game UI
- Color palette: void purple #3A1A4A, ember #E89020, slate #3A3D42, cyan #00FFCC (≤15% composition)
- Filter: Point (no anti-aliasing)
- Background: transparent
- PPU: 64
```

### Üretim sırası
1. Ribbon seti önce (renk referansı kurar)
2. Marker seti (ribbon paletinden devam)
3. Minimap frame (en büyük, en son)
