# RIMA Görsel/Oda MASTER PLAN (2026-06-11)

> Bu = tek konsolide yol haritası. Birleştirir: `TILEMAP_VISUAL_QUALITY_DECISION` + `ROOM_DESIGN_DECISION` + `PROPS_DOORS_PLACEMENT_PLAN` (hepsi 2026-06-11). 3-tur council (cx + ax 3.1 Pro + ax 3.5 Flash → Opus) + kod ground-truth ile temellendirildi.

## 🌟 KUZEY YILDIZI
Odalar referans-oyundaki gibi **"tasarlanmış + doğal"** görünsün — top-down 3/4, room-based, PixelLab-only kalarak. Sırrı izometri değil: **ışık + mantıklı dekor yoğunluğu + derinlik illüzyonu.**

## 🔒 2 KİLİTLİ İLKE (kullanıcı vurgusu)
1. **HER ASSET ŞEFFAF.** PixelLab decal/prop'ta arka plan kesinlikle şeffaf; mark/crack/rün tipinde prompt'a "isolated, fully transparent, NO floor tile/background". Üretim sonrası alfa doğrula. [[feedback-pixellab-decals-always-transparent]]
2. **YERLEŞTİRME MANTIKLI + DOĞAL — random scatter DEĞİL.** Çerçeveli-diorama: kenar-yoğun, merkez temiz, oda başına 1 odak, grounding.

## ❌ YAPMAYACAKLARIMIZ (council kilidi)
ISO'ya geçiş · Wang/autotile painter'ı bağlama · oda resize (genel) · combat'ta template-props (softlock) · room-designer split-brain birleştirme (post-demo) · animated su · rastgele prop saçma.

---

## ASSET KATMANI — 4 tier, hepsi ŞEFFAF (64px char + PixelLab batch)
| Tier | Canvas | Item/çağrı | İçerik | Durum |
|---|---|---|---|---|
| T1 decal | 32px | 64 | çatlak/kül/moloz/rift/rün/kemik/marker | ✅ Batch 1: **40 şeffaf tutuldu** (`rima_decal_v1`); ~24 dolu-bg atıldı → yeniden-üret |
| T2 küçük prop | 64px | 16 | urn/mum/brazier-base/kase/zincir/kemik-yığını/kristal | ⏳ üretilecek |
| T3 focal | 128px | 4 | tam brazier/kırık sütun/heykel/rift-kristal kümesi | ⏳ üretilecek (FocalCluster için gerekli) |
| T4 landmark | >170px | 1 | shrine/forge/throne/obelisk/gate | ⏳ boss/özel — demo sonu/zaman kalırsa |
Üretim = Claude MCP (kullanıcı green-light'lı), import/slice/pivot/SO = Claude.

## YERLEŞTİRME MANTIĞI (doğal, random değil — council sentezi)
- **Zone'lar (mevcut CompositionRoleMap):** WallBand (dış sıra) · DoorSafety (kapı/spawn/reward, radius 3, prop YASAK) · DecoratedEdge (2-tile kenar bandı, yoğun) · CleanCenter (merkez, dövüş, ~boş) · **FocalCluster (ŞU AN GENERATOR'DA YOK — eklenecek)**.
- **Dağılım:** T1 decal → grounding (prop/duvar dipleri) + silik merkez izleri · T2 küçük → DecoratedEdge'de küme · **T3 focal → oda başına 1, FocalCluster'a (üst/yan, high-top-down'da görünür)** · T4 landmark → boss.
- **Density:** mevcut hardcoded **0.65 ÇOK YÜKSEK → ~0.3'e düşür**, kenar-ağırlıklı (DecoratedEdge 1.0 / CleanCenter 0.1 zaten). Deterministik (run seed).

## IŞIK (asıl #1 görsel kazanç — her council'de #1)
- `RoomLightingProfileSO` (global renk/intensity + 2-4 point-light spec) + RoomTemplateSO opsiyonel alan.
- IsoRoomBuilder `Lighting` child-root oluştur/temizle → profilden Light2D instantiate. _Arena global Light2D'yi (şu an RoomMarkers altında) `Lighting`'e reparent. Default = mevcut ışık → regresyon yok.
- Kompozisyon: global ambient (soğuk, kısık) + key (focal/landmark üstü) + accent (brazier/kapı point-light) + **renk-kimliği** (oda soğuk-mavi, rift/ember sıcak-turuncu).
- Işıklar root altında → Scene-visibility göz ikonuyla gizlenir (kullanıcı sorusu: evet).

## ODA BOYUTU
**KEEP** — demo zaten büyük varied (combat 24×18–38×24, boss 36×28). Küçük çıplak elmas = chamber, combat değil. "Küçük" hissi = boş+ışıksız → FILL+IŞIK çözer. (Shop_01 16×12 opsiyonel büyütme.)

---

## 📋 FAZ ROADMAP (council-review 2026-06-11 → IŞIK-ÖNCE, FINAL)
> **DÜZELTME (3 advisor oybirliği):** ışık dekordan ÖNCE — ışıksız boş alanda dekor-yoğunluğu ayarlamak kompozisyonu körleştirir; ışık zero-asset-bağımlı, dekor registry/asset-bağımlı. **+ grounding shadow (sahte gölge)** eksik kaldıraçtı, eklendi. **Routing: execute = cx laurethayday.** cx drift bulguları FAZ2'de düzeltilecek: density 0.65→0.30 (`RoomDecorationPass.cs:13`), _Arena scene-wire eksik (`enableAutoDecoration/decorationRegistry` serialize değil), spawn/reward guard radius 1 vs plan DoorSafety 3 (playtest-watch).

### FAZ 0 — Asset (paralel, kod bekletmez)
- [x] Batch 1: 40 şeffaf decal (`rima_decal_v1`).
- [ ] 40 import → PropDefinitionSO + registry — **Claude**
- [ ] T2 (64px ×16) + ~24 eksik decal'i ŞEFFAF promptla yeniden-üret — **Claude MCP**
- [ ] T3 focal (128px ×4) + **grounding-shadow decal (yarı-saydam siyah elips)** — **Claude MCP**
- [ ] (boss) T4 landmark — zaman kalırsa

### FAZ 1 — IŞIK (İLK, en yüksek görsel kazanç, zero-asset) — **cx laurethayday**
- [ ] `RoomLightingProfileSO` (global renk/intensity + 2-4 point spec) + RoomTemplateSO opsiyonel `lightingProfile` alanı.
- [ ] IsoRoomBuilder `Lighting` child-root (rebuild'de temizlenir) + profilden Light2D + _Arena global Light2D reparent. **DEFAULT-PRESERVING (profil yoksa mevcut ışık aynı kalır, regresyon yok).**
- [ ] per-oda ambient/key/accent + renk-kimliği (oda soğuk-mavi, rift/ember sıcak-turuncu).
- **🎯 İLK cx GÖREVİ = bu (default-preserving lighting scaffold). Decoration OFF kalır.**

### FAZ 2 — DEKORASYON (ışıktan SONRA, doğru tonda) — **cx laurethayday**
- [ ] `_Arena`: `decorationRegistry` ata + `enableAutoDecoration=true` (scene-wire).
- [ ] **density 0.65→0.30** (`RoomDecorationPass.cs:13` drift), kenar-ağırlıklı.
- [ ] **FocalCluster:** generator üst/yan band işaretler (⚠️ CleanCenter merkez testini BOZMA) + pass 1 focal koyar (~100-180 LOC, testle kilitle; editor caller'lar RoomQCFix/PopulateProps/PropsTab etkilenir).
- [ ] grounding shadow decal'lar prop/duvar diplerine + guard-radius plan-uyumu (spawn/reward 1 vs DoorSafety 3 — playtest-watch).

### FAZ 3 — QC + playtest + his ayarı — **kullanıcı + Claude**
- [ ] Play → odalar "tasarlanmış+doğal" mı, ışık/yoğunluk hissi → tune + context'te kötü decal ele.

### POST-DEMO
room-designer split-brain birleştir · Wang autotile · animated mesale/su (+ BuildProps sprite-only → prefab desteği gerek) · landmark genişlet · environment parallax bg.

## DELEGASYON ÖZET
Plan/karar/QC = Opus/Claude · kod = **cx yekta** (weekly) · PixelLab üretim = Claude MCP (kullanıcı green-light) · playtest/görsel-onay = kullanıcı.
