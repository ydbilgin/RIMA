---
title: Brush V1 Workflow — PNG'den Painted Room'a
status: LIVE explainer
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Üretilen PNG sprite'ları Brush V1 sistemine nasıl entegre ediyoruz — step-by-step"
---

# 🎨 Brush V1 Workflow — PNG'den Painted Room'a

**Sorunun cevabı:** PixelLab'da ürettiğin PNG'ler **direkt sahneye konmuyor**. Önce Brush V1 sistemine **register** ediyoruz, sonra Unity Editor'da painted/placed room'lara dönüşüyor.

İki ayrı path var:

```
PNG Sprite
   │
   ├── 🎨 TILE PAINTING PATH (Aseprite Tilemap style)
   │   └── Floor / Wall / Decal / Accent
   │       └── AssetPool → Brush → Paint Editor
   │
   └── 🎯 PROP PLACEMENT PATH (Aseprite Stamp style)
       └── Crate / Urn / Statue / Treasure etc.
           └── PropDefinitionSO → PropPlacer Editor
```

---

## 📊 Brush V1 Mimari Diagram

```
PNG (raw sprite)
  │
  ▼
Unity Import (PPU=64, Point filter, no compression)
  │
  ├──── TILE PATH ─────┐                  ┌──── PROP PATH ────┐
  │                    │                  │                   │
  ▼                    ▼                  ▼                   ▼
AssetPool         BrushDefinition   PropDefinitionSO    PropRegistry
(sprite group)    (paint behavior)  (placeable object)  (runtime GUID)
  │                    │                  │                   │
  └──────┬─────────────┘                  └─────────┬─────────┘
         │                                          │
         ▼                                          ▼
    Brush Palette                            Prop Palette
    in Editor                                in Editor
         │                                          │
         ▼                                          ▼
   PAINT to Room                          PLACE on Room
   (drag/click)                           (click + R rotate)
         │                                          │
         └──────────────────┬───────────────────────┘
                            ▼
                    RoomTemplate.asset
                    (saved layout)
                            │
                            ▼
                    Library/ folder
                    (10 rooms hazır)
                            │
                            ▼
                    Game runtime spawns
```

---

## 🎯 PROP PATH — Wooden Crate Örneği (Bu Gece Üretiyorsun)

PixelLab'da Wooden Crate ürettin → `wooden_crate.png` → nasıl Brush sistemine sokuyoruz?

### Adım 1: PNG İndir + Unity'e Aktar

PixelLab'dan PNG indir → projeye taşı:

```
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Props/Crates/wooden_crate.png
```

### Adım 2: Unity'de Sprite Import Ayarları

PNG'yi seç → Inspector'da:

| Setting | Değer |
|---|---|
| Texture Type | Sprite (2D and UI) |
| Sprite Mode | Single |
| Pixels Per Unit | **64** (Karar #100) |
| Filter Mode | **Point (no filter)** — pixel sharp |
| Compression | None |
| Pivot | **Bottom Center** (yere oturmalı) |

Apply → sprite ready.

### Adım 3: PropDefinitionSO Oluştur

Project window'da:
- Sağ tık → **Create → RIMA → Brush → PropDefinition**
- Dosya adı: `Prop_WoodenCrate.asset`
- Konum: `Assets/Data/Brush/Props/Crates/`

Inspector'da:

| Field | Değer |
|---|---|
| `propId` | (otomatik dolar — PropDefinitionPostprocessor S13 LIVE) |
| `displayName` | "Wooden Crate" |
| `category` | "Crate" |
| `sprite` | wooden_crate.png referansı |
| `footprintTiles` | 1×1 (basit prop) |
| `walkable` | false (engelli) |
| `sortingLayer` | "Props" |

Save → PropDefinitionSO ready.

### Adım 4: PropRegistry Otomatik Lookup

Sprint 13'te eklendiğimiz **PropRegistrySO** otomatik tarama yapıyor. Yeni PropDefinitionSO oluşturulunca:
- PropDefinitionPostprocessor propId GUID otomatik dolduruyor
- PropRegistrySO runtime'da bu GUID ile prop'u bulabiliyor

Manuel iş yok bu adımda.

### Adım 5: Brush V1 Editor'da Yerleştir

Unity menu:
- **`RIMA → MapDesigner → Brush → Open Editor`**

Editor açılır. Sekmeler:
- **Brush Tab** — tile paint için (floor, wall, decal)
- **Props Tab** — prop placement için (CRATE BURADA!)

Props Tab seç:
- Sol panelde prop palette → **Wooden Crate** görünür
- Sağ panelde room edit area
- Library yüklü room aç (örn. `Combat_Small_01.asset`)
- Crate icon'una tıkla → cursor crate olur
- Room'da istediğin tile pozisyonuna **tıkla** → crate yerleşir
- **R hotkey** → rotate (90° increments)
- **V hotkey** → variant cycle (eğer variant'ları varsa)

### Adım 6: Save Room

Editor'da **Save Room** butonu → `Library/Combat_Small_01.asset` güncellenir → yeni prop placements kalıcı.

---

## 🎨 TILE PATH — Floor / Wall / Decal Örneği

Tile-style asset (floor tile, wall Wang16, organic decal) üretirsek (mossy crypt walls gibi):

### Adım 1: PNG → Unity Import
Aynı (PPU=64, Point filter).

### Adım 2: AssetPool Oluştur

Tek tile vs çoklu variant tile'lar için:

**Tek tile:**
- Create → RIMA → Brush → **AssetPool**
- Dosya: `AssetPool_MossyCrypt_Floor.asset`
- Field: `sprites` listesi → tile PNG'leri ekle

**Wang16 wall set:**
- 16 wall sprite'ı tek pool'a ekle
- Pool'un kendisi Wang16 SliceTemplate ile linked

### Adım 3: BrushDefinition Oluştur

- Create → RIMA → Brush → **Brush**
- Dosya: `Brush_MossyCrypt_Floor.asset`
- Fields:
  - `displayName`: "Mossy Crypt Floor"
  - `assetPool`: AssetPool_MossyCrypt_Floor referansı
  - `brushType`: Floor / Wall / Decal
  - `sliceTemplate`: L3_Wang16 (walls) veya yok (floors)

### Adım 4: BiomeSkin'a Ekle

- BiomeSkin SO'ya yeni Brush'ı ekle
- Örn: `BiomeSkin_HadesNet.asset` → brushes listesine `Brush_MossyCrypt_Floor` ekle

### Adım 5: Brush V1 Editor'da Paint

- Open Brush Editor
- **Brush Tab** seç (tile painting mode)
- Sol panelde brush palette → **Mossy Crypt Floor** brush
- Brush radius slider ayarla
- Room area'da **click+drag** → floor tiles paint
- Wang16 walls için: brush walls tab'da → otomatik corner alignment

---

## 🏗️ ROOM LİBRARY HALİHAZIR HAZIR (Codex Generated)

Az önce Codex 10 sample room oluşturdu (`Assets/Data/Rooms/Library/`):

```
Spawn_01.asset            — start room (single door E)
Corridor_Linear_01.asset  — 12×4 linear corridor
Corridor_LShape_01.asset  — 10×8 L-shape
Combat_Small_01.asset     — 8×6, 3 mob, 1 barrel placeholder
Combat_Medium_01.asset    — 12×8, 4 mob, 6 barrel placeholder
Combat_Large_01.asset     — 16×10, 6 mob, 1 barrel placeholder
Elite_01.asset            — 10×8, 1 elite + 2 mob
Treasure_01.asset         — 6×6, 4 barrel placeholder (CANDLES gelecek)
Shrine_01.asset           — 8×8, 8 barrel placeholder (ALTAR + CANDLES gelecek)
Boss_Intro_01.asset       — 14×10, 1 boss spawn
```

**Bu rooms'ta zaten barrel placeholder'ları var.** Sen yeni prop'lar üretip PropDefinitionSO yapınca, Brush Editor'da bu room'ları açıp **barrel placeholder'ları yeni prop'larla değiştirebilirsin.**

Örnek: Treasure_01.asset'i aç → 4 barrel pozisyonunu sil → 4 candle yerleştir → save.

---

## 🎬 BU GECE — Concrete Workflow

### 1. PixelLab production (yaklaşık 30-45 dk)
- STEP 1-4 prompts (Wooden Crate, Stone Urn, Candle, Debris Pile)
- 4 ayrı PNG indir → masaüstüne

### 2. Unity import (yaklaşık 10 dk)
- 4 PNG'yi `Assets/Sprites/Props/` altına farklı kategori klasörlerine taşı:
  - `Crates/wooden_crate.png`
  - `Urns/stone_urn_broken.png`
  - `Lighting/candle_iron.png`
  - `Debris/debris_pile.png`
- Her birinin Sprite Settings'ini ayarla (PPU=64, Point filter, Pivot Bottom)

### 3. PropDefinitionSO oluştur (yaklaşık 5 dk)
- 4 PropDefinitionSO oluştur (`Assets/Data/Brush/Props/{Category}/Prop_X.asset`)
- Inspector'da displayName, category, sprite, footprint, walkable, sortingLayer doldur

### 4. Brush V1 Editor'da yerleştir (yaklaşık 15-20 dk)
- Editor aç → Props Tab
- `Combat_Small_01.asset` yükle
- Mevcut barrel placeholder'ı sil
- Yeni Wooden Crate yerleştir (R rotate ile yönlendir)
- Save

### 5. Sample room görselleştir (yaklaşık 10 dk)
- Unity Scene → `RoomPipelineTest.unity` aç (veya `_FazMVP_Demo.unity`)
- Combat_Small_01 prefab instantiate
- 2-3 wall torch yerleştir (`Assets/Resources/Environment/StoneDungeon/Walls/RIMA_wall_torch.png`)
- LightFlicker.cs torch'larda enable
- Ambient light: deep blue-teal #1A2438, intensity 0.35
- Camera pos + Pixel Perfect Camera config
- **Screenshot**

### Toplam süre: ~70-90 dakika → İlk güzel oda screenshot'u

---

## 🤖 Otomatik PropDefinitionSO Üretim (Codex Script Önerisi)

12 prop için manuel PropDefinitionSO oluşturma sıkıcı olabilir. **Codex Editor script** yazabilir:

**Menu:** `RIMA → MapDesigner → Brush → Generate PropDefinitions from /Sprites/Props/`

İşlem:
- `Assets/Sprites/Props/` klasörünü tarar
- Her PNG için karşılık PropDefinitionSO oluşturur
- Otomatik category (folder name'den), display name (file name'den), default footprint 1×1, walkable=false, sortingLayer="Props"

İstersen söyle, Codex'e dispatch ederim. Bu manuel adım 4'ü atlatır.

---

## 🎯 Özet

| Adım | Ne yapıyorsun | Nerede |
|---|---|---|
| 1 | PNG üret | PixelLab Create Image Pro |
| 2 | Unity import + sprite settings | Unity Inspector |
| 3 | PropDefinitionSO oluştur | Unity Project window |
| 4 | Brush Editor'da yerleştir | Unity menu → RIMA → MapDesigner → Brush → Open Editor |
| 5 | Save room | Same editor |
| 6 | Test scene'de instantiate + lighting | Unity Scene |

**Brush V1 = Aseprite Tilemap (paint mode) + Aseprite Stamp (place mode) — Unity Editor'ın içinde.**

PNG → Sprite → PropDefinitionSO → Brush palette → Click place → Save room → Done.
