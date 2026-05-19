# Antigravity Task — RIMA Dungeon Faz 1: Skeleton Test (35°)

Proje yolu: `F:\Antigravity Projeler\2d roguelite\RIMA\`. Filesystem + UnityMCP erişimin var.

**Bu görev SADECE Faz 1.** Roadmap (`STAGING/ROADMAP_dungeon_buildup.md`) 6 fazlı piece-by-piece plan. Sen sadece **Faz 1: Skeleton** yapacaksın. Floor paint, decoration, lighting, post-FX, animation YOK — bunlar sırayla sonraki dispatch'ler.

---

## Ne istiyoruz (sadece bu)

Mevcut PixelLab wall sprite'larıyla **5 SpriteRenderer** yerleştirip dungeon iskeletinin v2 concept'i karşılayıp karşılamadığını test et.

Hedef referans: **`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png`** — Codex image_gen ile 35° açıda yeniden çizdirildi. Karar #100 35° lock'una uygun, RIMA'nın target dungeon görselliği.

**v1 (`RIMA_Act1_Spawn01_concept_v1.png`) REFERANS DEĞİL** — 50-60° isometric'ti, REJECTED.

## Required read

1. `STAGING/ROADMAP_dungeon_buildup.md` — Faz 1 spec (tüm 6 fazı görmen lazım ki scope'u aşma)
2. `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png` — target
3. `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` — eski (rejected, sadece ne istemediğimizi anla)
4. `STAGING/WAVE_E_DONE.md` — Sonnet'in son denemesi (68% fidelity, neden başarısız)
5. `STAGING/walls_v3_spawn01.png` + `walls_v3_spawn02.png` — Wave E sonucu (improving on this)
6. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/` — mevcut wall sprite envanteri
7. `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_arch.png` — corner candidate
8. `Assets/Scenes/Demo/RoomPipelineTest.unity` — sahne (Spawn_01_NewTileSystem hierarchy)

## Yapılacak iş — SADECE BU 4 ADIM

### Adım 1: Cleanup
`Spawn_01_NewTileSystem/L3_Walls/` altındaki TÜM mevcut child GameObject'leri sil. Wave E placement'lar dahil. `Gate_Entry` + `Gate_Exit` da silinebilir (Faz 3'te tekrar yapılır).

### Adım 2: 5 SR yerleştir (zero decoration)

`Spawn_01_NewTileSystem/L3_Walls/` altında SADECE:

| # | GameObject | Sprite | Pozisyon (öneri, ayarla) | flipX/Y | SortingOrder |
|---|---|---|---|---|---|
| 1 | Top_Wall | `pixellab_wall_section_horizontal` | (9.0, 12.0) full width stretch | false | 100 |
| 2 | Bottom_Wall | aynı | (9.0, 0.0) | flipY=true (perspektif inversion) | 100 |
| 3 | Left_Side | `pixellab_wall_arch_section` | (0.0, 4.91) | false | 99 |
| 4 | Right_Side | aynı | (18.0, 4.91) | flipX=true | 99 |
| 5 | Corners (optional) | `gate_arch` × 4 | corners (0,0)(0,12)(18,0)(18,12) | flip per corner | 101 |

PPU 64, Pivot top/bottom = BottomCenter, side = BottomLeft. **Scale ve pozisyon v2 silhouette'i match'leyecek şekilde ayarla** — yukarıdaki sayılar başlangıç. v2'ye bak, manuel match et.

**Eklenecek HİÇBİR ŞEY YOK:**
- Brazier yok (mevcut varsa devre dışı bırak)
- Vine yok
- Decal yok
- Banner yok
- Chain yok
- Candle yok
- Floor paint yok (L1 boş kalır)
- Light2D ekleme yok (mevcut Spawn_01'de varsa devre dışı bırak)
- Post-FX değişikliği yok
- Character'ı kaldır veya devre dışı (sahne dışında bırak)

### Adım 3: Screenshot

Camera frame 18×12 world units (concept v2 ratio yakın). Screenshot çıkar:
- `STAGING/skeleton_v1_spawn01.png`

Yan yana karşılaştırma için aynı framing.

### Adım 4: Verdict raporu

`STAGING/ANTIGRAVITY_DONE_skeleton_faz1.md` yaz. İçerik:

#### Bölüm 1: PASS / FAIL kararı

İskelet v2 silhouette'i tutuyor mu?
- **PASS** = sadece bu 5 sprite ile concept'in temel wall yapısı okunabiliyor (chain/banner/candle henüz yok ama orada olabileceği yer net)
- **FAIL** = mevcut sprite class'ları yapısal olarak yetersiz (örnek: `pixellab_wall_section_horizontal` çok flat, v2'nin 35°'deki wall depth'i okunmuyor; veya arch sprite v2'deki gateway pozisyonunu match'lemiyor)

#### Bölüm 2: Eğer FAIL ise — regen önerisi

Hangi sprite class'ı yetmiyor? Tek-tip dispatch için:
- Sprite tanımı (ne tip, kaç tane variant)
- PixelLab prompt drafts (Pro veya MCP, hangisi)
- Toplam gen count (5'ten az olmalı, 5+ ise gerekçe gerek)
- Style ref (mevcut sprite'lardan hangisi)

#### Bölüm 3: Eğer PASS ise — Faz 2 hazırlık

`Assets/Art/Tiles/F1/Tilesets/` altında 11 mevcut Wang tileset var. v2 floor (paved stone, gray-blue tinted, moss-friendly) ile en yakın eşleşeni hangisi? Filesystem'i incele, isim listesi + ön değerlendirme yaz. Faz 2 dispatch'i bu listeyi kullanacak.

#### Bölüm 4: Out-of-scope items

Faz 1'de gözlemlediğin ama dokunmadığın şeyler (Faz 2-6'da ele alınacak). Liste:
- Floor blackness — Faz 2
- Wall accent (chain/banner/candle) — Faz 3
- Lighting — Faz 4
- Post-FX — Faz 5
- Variant rules — Faz 6

Sadece Faz 1'i yaptığını kanıtla.

## Locked karar'lar (içinde çöz, dokunma)

- Karar #98: cyan #00FFCC tek ambient accent
- Karar #100: 35° angled top-down — **v2 reference budur, koru**
- Karar #143: 6-layer Multi-Layer Painter
- Karar #147: `RoomTemplateSO.backgroundLayers` per-room visual contract
- Karar #148: Branch D + E (floor de-emphasis + 4-8° camera tilt)
- Karar #149: Combat odası = 3-5 sub-room sequence
- Transform Squash: Tilemap parent localScale.y = 0.819 (zaten uygulandı)
- User-cannot-draw: manuel pixel paint YASAK her aşamada

## Hard rules

- DO read ROADMAP_dungeon_buildup.md ÖNCE — scope creep'ten uzak dur
- DO sadece Faz 1 yap. Floor paint, lighting değiştir, decoration ekle YASAK
- DO 5 SR ile sınırlı kal (corner optional, max 9)
- DO screenshot v2 framing'e yakın çıkar
- DO `read_console` mutation sonrası her batch'te
- DO scene save adım sonu
- DO NOT regen ANY PixelLab asset bu dispatch'te (gerekirse RAPORDA öner, yapma)
- DO NOT git commit
- DO NOT modify Warblade prefab
- DO NOT call `EditorUtility.DisplayDialog`
- DO NOT modify any Karar dosyası
- DO NOT skip the verdict report — ana çıktı bu

## Yaklaşım

- Sahneye bakmadan önce v2 concept'i 2-3 dakika incele
- Sonra mevcut sprite envanterini aç
- Sonra placement et — concept'i taklit etmeye çalış, kendi yorumunu katma
- Screenshot al, yan yana koy mental olarak
- Verdict yaz, gerekçe ile

## Beklenen toplam süre

15-30 dakika. İskelet testi basit iş, dürüst PASS/FAIL kararı vermek asıl değer.

## Neden bu kadar sıkı scope?

Önceki denemelerde (Wave E vd.) "her şeyi aynı dispatch'te düzelt" yaklaşımı dungeon yapısını kuramadan dekorasyon ekledi → sprite collage. Bu sefer **iskelet → floor → decoration → lighting → post-FX → variant** sırasıyla. Bir önceki PASS olmadan sonraki yok. Faz 1 = sadece iskelet.
