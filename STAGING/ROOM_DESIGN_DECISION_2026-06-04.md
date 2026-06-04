# Room Design DECISION (2026-06-04) — gameplay-feel oda seti

Council: cx (kütüphane audit + ingest/feasibility) + ax 3.1 Pro (gameplay/level design) + ax 3.5 Flash (lean) → Opus sentez. Kullanıcı: ChatGPT pack'i körü körüne import etme; analiz+birleştir+iyileştir. Brief=`ROOM_DESIGN_COUNCIL_BRIEF_2026-06-04.md`.

## En kritik bulgu (cx audit) → tartışmayı çözer
Mevcut `Assets/Data/Rooms/Library/` odaları ZATEN şekilli (walkableGrid dolu, full-rect değil) ama **KÜÇÜK**: Combat_Small 8x6, Medium 12x8, Large 16x10, Elite 10x8, Boss 14x10, Corridor 12x4/10x8, Shrine 8x8, Spawn 8x6, Treasure 6x6. ChatGPT pack ise **BÜYÜK** (24-38w). → **Boyut olarak tamamlayıcılar.** 3.1 "küçük şekil üret" ↔ 3.5 "küçük şekil üretme" çatışması ÇÖZÜLDÜ: küçük/orta = mevcut kütüphane (zaten şekilli, üretmeye gerek yok), büyük = ChatGPT'den seç.
⚠️ cx: legacy library asset'lerinin çoğu hâlâ **GÜNEY door socket** taşıyor (canon ihlali N/E/W). Temizlenecek.

## KARARLAR

### 1) Boyut katmanı (3.1'in 20/50/30 + cx audit)
- **Küçük (~%20, gergin/adrenalin):** mevcut Combat_Small (+ South-door temizliği). Üretme.
- **Orta (~%50, taktik baseline):** mevcut Combat_Medium + Elite_01 (+temizlik). Üretme.
- **Büyük (~%30, klimaks/wave):** ChatGPT'den seçilenler (aşağıda).
- Non-combat (Shrine/Spawn/Treasure): mevcut kütüphane (küçük, temiz, combat adalarından ayrı — 3 advisor hemfikir).

### 2) ChatGPT pack — hangi oda (3 advisor sentezi)
**IMPORT (8 büyük, gerçek yeni değer = büyük katman bizde YOK):**
- Büyük Combat: **donut** (merkez void=knockback-kill ring, S-tier oybirliği) · **hourglass** (waist chokepoint, oybirliği) · **bridge-lobes** (melee↔menzil, 3.1+cx) · **twin-basins** (flank/iki-lob, 3.1+cx).
- Elite: **trident** (yönlü prong→arena, cx+3.1) · **crescent** (void-edge claw, oybirliği).
- Boss: **boss-shattered-oval** (oybirliği; Boss_Intro'nun büyük versiyonu).
- Corridor: **zigzag-bridge** (yeni büyük koridor değeri; combat odası gibi AŞIRI kullanma).

**SKIP (overlap / düşük değer / redundant):** organic-blob (amorf/boş, 3.1+cx) · diamond/cross/L-shape (legacy sahne + library ile overlap, 3.5+cx) · teardrop (kuyruk ölü alan, 3.1+3.5) · reliquary-diamond + donut-vault (chest hızlı olmalı, 3.5; mevcut Treasure yeter).

### 3) UCUZ ÇEŞİTLİLİK LEVYELERİ (3.5 — yüksek değer, geometri üretmeden) — ADOPT
- **Dinamik kapı yönü:** runtime'da N/E/W kapılardan girişi değiştir → aynı oda batıdan vs kuzeyden girince taktik başlangıç değişir.
- **Enemy wave preset:** aynı layout'a 3 farklı wave kompozisyonu (swarm / teleport-caster / elite-tank). Çeşitlilik tile'dan değil COMBAT'tan gelir.
- **Template mirroring:** parse'ta yatay/dikey ayna → layout varyasyonu 2-4x.
→ Bu üçü, 8 büyük + mevcut küçük/orta ile birlikte 10-15 odalık run'ı GEOMETRİ üretmeden bolca çeşitlendirir.

### 4) Pacing (3.1) — Tension-Breath
Spawn → Small → Medium → **Large(spike)** → Elite → **Shrine/Merchant(breath)** → Medium → Large → Boss. Asla 2 Large arka arkaya (Elite hariç). Boyutlar dalgalanmalı. Run = 10-15 oda (RoomRunDirector depthCount).

### 5) İyileştirmeler ("daha iyisini yap") — v2'ye DEFER (3.5+cx: şimdilik defer)
ChatGPT odaları DÜZ floor (cover/pillar/hazard/elevation yok). v2 pass: cross/diamond'a LoS pillar · boss'a iç hazard ring · YENİ tip: Hazard/Trap odası + Choice/Split (dallanma) odası. Şimdi DEĞİL (custom collision/AI sink).

### 6) Mimari (B-12) — cx net
Veri-bazlı **`_Arena` + RoomRunDirector + RoomBankSO + IsoRoomBuilder = PRODUCTION** oda akışı; sahne-bazlı MapFlowManager emekli. IsoRoomBuilder 38w'ı kaldırır (cap yok; risk=kamera framing/sorting/gate-row, S verify). En büyük blocker asset import DEĞİL → **RoomBankSO tip kapsamı (Chest/Corridor eksik) + scene-load→data-flow geçişi** (= B-11 combat lifecycle). M/L.

## UYGULAMA SIRASI (post-decision, autonomous)
1. **Seçili 8 ChatGPT odasını import** → RoomTemplateSO (Generated). (Importer hazır; JSON'u subset'le.) — S
2. **Legacy South-door temizliği** library + (builder N/E/W enforce). — S
3. **RoomBankSO tip kapsamı** (Chest/Corridor/typed buckets) + **production RoomBank** (mevcut küçük/orta + generated büyük, pacing weights). — M
4. **Ucuz çeşitlilik:** dinamik kapı + wave preset + mirroring. — M
5. **RoomRunDirector depthCount 10-15** + pacing dizilim. — S/M
6. **_Arena combat lifecycle bağla** (B-11: encounter→clear→reward→door) — EN BÜYÜK. M/L
7. Play-verify (D3D11 restart sonrası) + commit.

**Tek cümle:** Küçük/orta = mevcut şekilli kütüphane, büyük = ChatGPT'den 8 keeper; çeşitlilik geometriden değil **kapı+wave+mirror**'dan; cover/hazard/yeni-tip v2; mimari = veri-bazlı _Arena production (B-11/B-12 ile birleşik).
