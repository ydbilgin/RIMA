# RIMA Oda Tasarımı — COUNCIL DECISION (2026-06-11)

> Council: cx (feasibility, laurethayday) + ax 3.1 Pro (framed-diorama) + ax 3.5 Flash (lean) → Opus sentez. Sorular: `STAGING/_process/2026-06/_council_*_room-design-tooling.md`. Üstüne oturduğu: `TILEMAP_VISUAL_QUALITY_DECISION_2026-06-11.md` + `PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md`.

## TL;DR
Odalar zaten **yeterince büyük** — sorun boyut değil, **boş+ışıksız** olmaları. Fix sırası: **(1) dekorasyon-pass'i AÇ + yoğunluğu düşür + FocalCluster boşluğunu kapat (oda başına 1 odak) → (2) per-oda ışık (en yüksek görsel kazanç) → (3) prop üretimi.** Oda BÜYÜTME, room-designer'ı BİRLEŞTİRME (post-demo), combat'ta template-props AÇMA (softlock).

## 🔑 KRİTİK REFRAME (cx kanıtı)
**Demo combat odaları ZATEN büyük + çeşitli:** DemoRoomBank = 9 combat (24×18 → 38×24), 2 elite (30×22), 1 boss (36×28), 1 shop (16×12). Sıra: Combat→Combat→Merchant→Combat→Boss→Combat (`DungeonGraph.cs:68-81`). **Senin gördüğün küçük çıplak elmas = chamber (karakter-seç) ya da eski screenshot — demo combat odası DEĞİL.** Yani "oda küçük" aslında "oda boş + ışıksız". → Fix = **DOLDUR + IŞIKLANDIR**, resize değil. (Tek istisna: Shop_01 16×12 küçük — istenirse Shop_02 ~20×16.)

## ADVISOR SENTEZİ
| Konu | 3.1 Pro | 3.5 Flash | cx (kod) | **KARAR (Opus)** |
|---|---|---|---|---|
| Oda boyutu | 24-36 ideal | büyütme, büyük+boş=ölüm | demo zaten 24-38 | **KEEP. Resize yok** (Shop hariç, opsiyonel) |
| Prop yerleşimi | framed diorama: kenar-yoğun, merkez boş, 1 landmark | auto-placer + elle 1 focal; gerisi over-eng | **density 0.65 hardcoded (yüksek!); FocalCluster generator'da HİÇ işaretlenmiyor** | **Dekorasyon AÇ + density 0.65→~0.3 kenar-ağırlıklı + FocalCluster boşluğunu kapat (oda başı 1 odak)** |
| Room designer | 3 tool özelliği ekle | SIFIR tool, elle 4 oda | **split-brain: Rooms-tab=RoomTemplateSO(live) vs Floor/Light-tab=RoomData(ölü); PropsTab unwired** | **Demo: yeni tool YAZMA. Mevcut Rooms-tab + dekorasyon-pass yeter. Split-brain birleştirme = POST-DEMO (L)** |
| Işık | per-oda key/accent/renk-kimliği | global ambient + 2-3 point, container+gizle | _Arena tek global Light2D (RoomMarkers altında, kötü org); RoomLightingProfileSO yok | **#1 KALDIRAÇ: RoomLightingProfileSO + IsoRoomBuilder Lighting-root + per-oda ambient+2-3 point + renk-kimliği (M)** |

## KARARLAR (kilitli)
1. **Oda boyutu = KEEP.** Demo zaten büyük varied odalar kullanıyor. "Küçük" hissi = boş+ışıksız. Resize yapma (Shop_01 opsiyonel büyütme). Chamber'ı combat yoluna sokma.
2. **Dekorasyon-pass AÇ + ayarla:** `enableAutoDecoration=true` (_Arena IsoRoomBuilder), `decorationRegistry` ata. **TargetDensity 0.65 → ~0.3** (Flash: kenar %15-20; cx: 0.65 fazla kalabalık). Kenar-ağırlıklı (DecoratedEdge 1.0 zaten). **[küçük kod]**
3. **🆕 FocalCluster boşluğunu kapat (en yüksek "tasarlanmış" kaldıracı):** `CompositionRoleMapGenerator` FocalCluster'ı HİÇ işaretlemiyor — placer destekliyor ama zone yok (`cx: CompositionRoleMapGenerator.cs:21-24`). Ekle: generator oda başına 1 FocalCluster bölgesi (üst/yan, high-top-down'da görünür) → dekorasyon-pass oraya 1 focal/landmark prop koyar. **[küçük-orta kod, demo-safe — pass zaten non-blocking]**
4. **Per-oda ışık (asıl görsel kazanç):** `RoomLightingProfileSO` (global renk/intensity + 2-4 point-light spec) + RoomTemplateSO opsiyonel alan + IsoRoomBuilder `Lighting` child-root oluştur/temizle + profilden Light2D instantiate. _Arena global Light2D'yi `Lighting` altına reparent (şu an RoomMarkers altında). Default profil = mevcut global ışık → regresyon yok. **[M kod]** Işıklar root altında → Scene-visibility ile gizlenir (senin sorunun: **evet**).
5. **YAPMA:** room-designer split-brain birleştirme (L, post-demo) · oda resize (genel) · combat/elite'te template-props (softlock fix `RoomRunDirector.cs:311-318` [LOCK-RİSK]) · RoomData-painter'ı live runtime sayma · JSON-lighting (canlı tüketici yok).

## PROP ÜRETİMİ (paralel, locked tier'lar)
T1 decal 32px (Batch 1 üretildi, review'da — kullanıcı onayı bekliyor) → T2 küçük 64px → T3 focal 128px (madde 3 için gerekli) → T4 landmark boss için. Üretim kullanıcı-gated/Claude-MCP.

## SIRA + SAHİP
1. Batch 1 decal onay+seç (kullanıcı görsel onay → Claude select+import)
2. Dekorasyon AÇ + density 0.3 + FocalCluster ekle — **cx (yekta)** [küçük-orta]
3. T2/T3 prop üret (focal için) — kullanıcı-gated/Claude
4. Per-oda ışık profili + Lighting-root — **cx (yekta)** [M] + Claude/Unity scene reparent
5. QC + playtest (kullanıcı) → yoğunluk/ışık his ayarı
- POST-DEMO: room-designer split-brain birleştir (RoomData↔RoomTemplateSO), Wang, landmark genişlet.

## SAKIN YAPMA (deadline tuzakları — Flash)
Room-designer arayüzü yazma. Wang kurma. Oda büyütme. Procedural prop algoritmasını mükemmelleştirme. "Oyuncu kodun güzelliğine değil, loş heykelin yaydığı ışığa bakacak."
