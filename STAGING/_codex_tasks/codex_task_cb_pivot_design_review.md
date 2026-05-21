# Codex Task — CB Pivot Decision + Design Expansion Review

**Tarih:** 2026-05-18
**Profile:** laurethayday (xhigh)
**Effort:** high
**Output:** `STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md`

---

## ACTIVE RULES
(1) think before answering (2) min commentary, no fluff (3) surgical — sadece sorulan sorulara cevap ver (4) BLOCKED yaz eğer kararsızsan

## NLM ACCESS
If you need RIMA design context, query NLM first:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

Direct-read sadece: bu dosya, `F:/LaurethStudio/02_GAMES/CircuitBreaker/00_FULL_DESIGN_DOC.md`, `01_MVP_PLAN.md`, `03_MARKET_REFERENCE.md`

---

## 1. KONTEKST — Neden bu task var

Kullanıcı (solo dev, okul ödevi RIMA olarak verildi, 128→64 scope refinement zaten yapıldı) **RIMA'dan CB'ye pivot kararı** veriyor. Sebep: RIMA'da floor/tile pipeline drift'i 5+ seans devam etti (top-down vs Hades 35° vs Karar #143 6-layer composition). Solo dev için scope mismatch — pure top-down CB scope'u daha temiz.

Bu seansta CB design **5 yeni katmanla genişletildi** (aşağıda). Codex'ten istenen: **scope realism + design coherence + map design pivot + 16 hafta MVP feasibility** değerlendirmesi.

---

## 2. CB DESIGN GENİŞLEMESİ — Bu seansta eklenen 5 katman

CB `00_FULL_DESIGN_DOC.md` mevcut. Aşağıdaki **5 ek katman** design doc'a entegre edilmeli:

### 2.1 Tetik silahı 2-modlu yapı (Section 3.1 revize)

Mevcut: 5 element tetik silahı (Elektrik/Ateş/Su/Buz/Vakum) — sadece **tetik mode**.

Yeni: Her silah **iki modlu**:

| Silah | Tetik mode (sol-tık, anlık) | Drop mode (sağ-tık veya basılı, zemin) |
|---|---|---|
| ⚡ Elektrik | Bolt, tile reaction tetikler | Statik field 2x2 (kendine dokunma riski, 5 sn) |
| 🔥 Ateş | Bolt, yangın tetikler | Yağ damacaalı patch 3x3 (kayar + yanar) |
| 💧 Su | Atış, söndürür/seyreltir | Su gölü 3x3 (8 sn) |
| ❄️ Buz | Atış, freeze | Buz yüzey 4x4 (kayar, freeze yürüyene) |
| 🌪️ Vakum | Anlık çekiş, mermi yönlendir | Toz bulutu 4x4 (görüş -, slow) |

**Etki:** Self-combo loop mümkün — boss arenası boş bile olsa oyuncu drop+swap+tetik üçlüsüyle cascade kurar. Düşman bağımlılığı azalır.

**Combat akışı örnek:**
1. Sağ-tık (Su mode) → önüne su gölü bırak
2. Q ile elektrik'e swap (0.3 sn)
3. Sol-tık (Elektrik mode) → su gölüne tetik = AOE şok
4. E ile ateş'e swap
5. Sağ-tık (Ateş mode) → boss'un üzerine yağ patch
6. Q ile elektrik'e swap
7. Sol-tık = yağ + statik = patlama
8. Combo charge dolu → next tetik 2x AOE

= 5-8 saniyede 3 farklı kombo.

### 2.2 Element Form Ultimate (Section 4 her sınıfa 4. skill)

Mevcut: Her sınıf 3-4 skill (zemin + tetik booster + mobility).

Yeni: Her sınıfa **"Element Form" Ultimate** (R tuş, 30 sn cooldown):

- **Aquamancer → Liquid Form (3 sn):** Mevcut su tile'larında anlık ışınlama (held tuş + tile click). Form'dan çıkış = basıldığı tile'a buz patlaması (3x3 freeze).
- **Pyrotechnist → Inferno Dash (3 sn):** Hareket = arkasında yangın izi (her tile 0.2 sn ateş). Düşmana çarpış = pierce + ignite. Form bitince = fire nova (5x5 patlama).
- **Stormcaller → Tornado Body (3 sn):** Hava süpürme (düşman ve item çek). Toz/gaz tile'lardan geçince yutar. Form bitince = vakum patlaması (yutulmuş her tile = ek damage).
- **Alchemist (Faz 2):** Acid Pool Form — zehir tile'larda ışınla, çıkış = acid geyser.
- **Magnetist (Faz 2):** Magnet Field — mermi cancel + metal çek, çıkış = mermi salvo geri at.

**Etki:** Hız hissi (3-4x movespeed), VFX şölen, route planlaması = setup'ın ikinci katmanı. Hades signature spell muadili.

### 2.3 Hibrit zemin sistemi (Section 5b YENİ)

Mevcut: 15 düşman → 15 tek-tip zemin.

Yeni: İki farklı zemin bitişikte = **hibrit tile**:

| Zemin A | + Zemin B | = Hibrit | Reaksiyon |
|---|---|---|---|
| Su | Buz | **Slush** | Elektrik = 2x AOE (geniş) |
| Su | Yağ | **Emülsiyon** | Ateş = yavaş yanma (10 sn) — daha fazla cascade fırsatı |
| Yağ | Statik | **Volatile** | Herhangi tetik = patlama (oyuncu hata yapabilir!) |
| Ateş | Toz | **Sıcak duman** | Görüş - + AOE damage |
| Asit | Su | **Seyreltik asit** | Damage az, alan 2x |
| Manyetik | Statik | **EMP field** | Düşman mermisi paralize |
| Buz | Yağ | **Donmuş kaygan** | Slip + freeze |

MVP'de **7 hibrit**. Sistem oturunca 15-20.

### 2.4 Status hybridler (düşman üstünde)

Düşman üstüne element status bin'erse:

| Status A | + Status B | = Hibrit | Etki |
|---|---|---|---|
| Wet | Shocked | **Conductive** | Yakın düşmanlara zincir |
| Burning | Wet | **Steaming** | Self-damage + boss aim'i bozar |
| Oiled | Burning | **Combusting** | 3 sn sonra patlar (zincir yağlanmış düşmana) |
| Frozen | Physical hit | **Shatter** | Anında ölüm + parça mermisi |
| Magnetized | Anything | Mermi çek | Boss düşmanına manyetik → kendi mermisi ona döner |

= Düşman canlı walking conductor. Yağ tile'a basan düşmanı ateşle vur → patlar → zincir.

### 2.5 Modifier sistemi (Section 6b YENİ, Hades boon muadili)

Run boyu 3-5 modifier slot → skill'leri evrimleştir.

**Örnek (Aquamancer):**

| Modifier | Etki |
|---|---|
| **Deep Pool** | Tide Pool 3x3 → 5x5 ama cooldown +%50 |
| **Flash Freeze** | Tide Pool koyunca %30 freeze status |
| **Conductive Pool** | Su tile elektriğe vuruldu = 2 sn AOE zone (anlık değil) |
| **Liquid Loop** | Liquid Form sırasında her teleport mini-damla bırakır |
| **Tidal Echo** | Boss vuruşları su tile spawn'lar |

MVP'de 30-50 modifier (3 sınıf × 10-15 modifier). Run sonu 4-5 modifier sentezi = build identity.

### 2.6 Run loop derinleştirme (Section 7 revize, 6 yeni katman)

Mevcut: 5 oda + boss = düz.

Yeni katmanlar:

**A. Choice forks (oda 2-3 arası):**
| Kapı | İçerik | Ödül |
|---|---|---|
| ⚔️ Bloodlust | Daha çok düşman + elite | + relic |
| 💰 Gold | Az düşman + chest | + spark |
| 🔮 Mystery | Bilinmeyen mini-event | + rare item |
| 💀 Curse | Self-debuff + büyük ödül | + 2 relic + 1 echo |

**B. Mid-run events (NPC):**
- Wandering Alchemist (tetik upgrade)
- Element Shrine (1 element kalıcı boost)
- Cursed Mirror (full heal + sonraki oda 2x düşman)
- Sigil Vendor (relic re-roll)
- Boss Echo (eski boss + unique boon)

**C. Boss phases × arena state (3 phase):**
- Phase 1: Normal pattern, arena boş
- Phase 2 (HP 66%): Boss kendi zemin spawn'lar → oyuncu kendi zeminiyle birleştir → mega damage
- Phase 3 (HP 33%): Boss arenayı boş bırakır → tetik silahı charge atış (defansif)

**D. Hidden rooms + secret boss:**
- 5 odadan 3'ünde %30 ihtimal secret door (gizli element kombosuyla açılır)
- Lore parçaları topla → secret 6. oda (gerçek final boss)

**E. Class story hooks:**
- Aquamancer: "Donmuş anneyi bul" → 10 run sonra farklı final arena
- Pyrotechnist: "Şehri yakanın izi" → 5 run sonra mid-run NPC değişir
- Stormcaller: "Rüzgar Ruhları onayı" → secret boss farklı

**F. 3-tier currency:**
| Currency | Drop | Harcama | Persistent |
|---|---|---|---|
| **Spark** | Düşman, dalga sonu | Run içi: heal, reroll, upgrade | Run sonu silinir |
| **Cinder** | Boss, secret room | Meta: skill/sınıf/silah aç | Permanent |
| **Echo** | Secret boss, %100 clear | Pact of Element (zorluk modifier) | Permanent + leaderboard |

---

## 3. MAP DESIGN — RIMA Map Designer port stratejisi

CB pure top-down → RIMA'nın Karar #143 6-layer composition GEREKMİYOR. Port stratejisi:

**RIMA'da hazır olanlar** (port edilecek):
- Tile Palette + RuleTile sistemi (Wang16 ready, `Assets/Art/Tiles/F1/Tilesets/`)
- Asset Pack Browser (32 tile + 3 RandomTile pool)
- Brush Executor (tile painting tools)
- Room Template SO (oda data model)
- Blueprint Zone System (zone-based generation)
- Adjacency Rules (tile transition logic)

**CB'ye port (1-2 hafta Codex iş):**
1. RIMA Map Designer'ı ayrı asmdef paketle
2. CB Unity projesine package import
3. Karar #143 6-layer SİL (PURE top-down)
4. Tile Palette + RuleTile + Brush yeterli
5. **YENİ:** Tile state system (CB özel — su tile süreli, ateş tile yayılır, hibrit tile birleşim)

**MVP map scope:**
- 1 biome (Shattered Keep / Cursed Lab)
- 32x24 tile grid
- 16 tile variant: floor + 4 wall + 4 decoration + 3 hazard
- PixelLab `create_tiles_pro` 1 dispatch (~25-40 generation)
- 5 oda preset (open arena, choke point, pillar maze, hazard floor, boss arena)

---

## 4. CODEX'TEN İSTENEN — Bu sorulara cevap ver

### 4.1 Scope realism

**Soru 1:** 16 hafta MVP'ye sığar mı?
- 3 sınıf × 4 skill (12 skill) + Element Form Ultimate (3 form)
- 5 element × 2-mod silah (10 silah modu)
- 7 hibrit zemin + 5 status hybrid
- 30-50 modifier (3 sınıf × 10-15)
- 5 oda + boss + secret boss
- Choice fork + 5 NPC + class story hooks (3)
- 3-tier currency
- Map system port

**Beklenen format:** Hafta hafta breakdown (1-16 hafta), her hafta ne, hangi sistem **POST-MVP'ye atılmalı**.

### 4.2 Hangi 5 sistem LOCK vs DEFER

**Soru 2:** Yukarıdaki 5 katman + 6 run loop katmanından **MVP için kritik 5**, **post-MVP'ye defer 5** hangileri? Gerekçeli sırala.

### 4.3 Teknik risk

**Soru 3:** 2-mod tetik silahı UX riski (oyuncu sol-tık vs sağ-tık kafa karışıklığı)?
**Soru 4:** Hibrit tile state machine complexity — `TileStateMachine` 7 tek-tile + 7 hibrit state = 14 state, transitions ~30. Performans riski 32x24 grid'de?
**Soru 5:** Modifier sistemi 30-50 modifier × event bus integration — `ModifierDef` SO yapısı yeterli mi yoksa scripting node graph mı gerekir?

### 4.4 Run length validation

**Soru 6:** Önerilen 20-25 dk run gerçekçi mi? Hesaplama:
- 5 oda × 90-150 sn = 7.5-12.5 dk
- Boss 3-5 dk
- Choice fork + NPC event = 1-2 dk
- TOPLAM = 12-19 dk **TARGET 20-25 dk**
Aradaki 5-7 dk açığı (a) extra dalga (b) ek oda (c) longer boss → hangi?

### 4.5 Map design pivot

**Soru 7:** RIMA Map Designer port — neyi sakla, neyi sil?
- Karar #143 6-layer painted composition → SİL (onaylar mısın?)
- Wang16 tilesets `Assets/Art/Tiles/F1/Tilesets/` → CB'ye sakla mı yoksa CB için yeni Act 1 tile gen mi?
- BlueprintZoneSystem → CB'de gerek var mı yoksa basit RoomTemplate yeter mi?

**Soru 8:** Tile state system (CB özel) — `RoomDataSO` mevcut RIMA spec'ine eklenebilir mi, yoksa `CBTileStateSO` ayrı yeni asset mı?

### 4.6 Pivot timing

**Soru 9:** Net pivot timing önerisi:
- (A) Full pivot now — RIMA dondur, CB Unity setup başlat (1-2 hafta setup + 14 hafta dev)
- (B) RIMA-lite önce — Karar #143 sil, PURE top-down + Wang16 ile RIMA Faz 1 ship (4-6 hafta), sonra CB
- (C) Hybrid — CB design doc tamamla (1 hafta), RIMA'da küçük POC (2-mod tetik test, 2 hafta), sonra full pivot

Okul ödevi context'i (RIMA olarak verildi, 128→64 zaten yapıldı, "scope iteration" olarak satılabilir) ışığında **gerekçeli öneri**.

### 4.7 Anti-klon kontrolü

**Soru 10:** Bu genişletmelerle CB hangi oyunlara **daha çok benzer** oldu?
- Magicka (element kombo) — risk var mı?
- Noita (element reaction) — risk var mı?
- Hades (boon + form + run loop) — risk var mı?
- Magicraft (spatial spell) — yakın emsal sorun mu?

Her birine 1 cümle anti-klon mitigasyon.

---

## 5. OUTPUT FORMATI

Output: `STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md`

Yapı:

```markdown
# CB Pivot Design Review — Codex Verdict

## 1. Scope realism (16 hafta breakdown)
[hafta hafta tablo]

## 2. MVP LOCK 5 vs DEFER 5
LOCK: ...
DEFER: ...

## 3. Teknik riskler
- 2-mod UX: ...
- Hibrit tile state: ...
- Modifier system: ...

## 4. Run length validation
[hesap + açık dolduran öneri]

## 5. Map design port
[sakla/sil tablosu]

## 6. Pivot timing — NET ÖNERİ (A/B/C)
[karar + gerekçe + 16 hafta timeline]

## 7. Anti-klon kontrolü
[4 oyun × mitigasyon cümlesi]

## 8. EXECUTIVE SUMMARY (5 madde, kullanıcı için)
1. ...
2. ...
3. ...
4. ...
5. ...
```

**Tahmini cevap uzunluğu:** 800-1500 satır markdown.

**Bitişte:** `CODEX VERDICT COMPLETE` satırı.
