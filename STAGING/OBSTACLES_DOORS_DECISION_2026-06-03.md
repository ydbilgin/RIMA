# RIMA — Engel/Prop + 3-Yön Kapı ÜRETİM KARARI (Council synthesis)
Date: 2026-06-03 · Decider: Opus, synthesizing cx (feasibility/reuse) + Gemini 3.1 Pro (deep art) + Gemini 3.5 Flash (lean) + mockup validation.
Mockup (on-brand doğrulama): `STAGING/imagegen/img_20260603_194244.png`.

## Karar özeti (advisor uzlaşı + benim çağrım)
- **Üretim aracı:** cx `$imagegen` skill'i (user seçti). Stil-kilidi: `Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png` style/init reference (izo açı sapmasını önler — her iki Gemini de vurguladı).
- **Format: TEK-TEK PNG** (cx + 3.5 uzlaşı). Büyük karışık sheet YOK. Kapı = aynı-canvas per-yön. Obstacle = tek-tek.
- **Wiring: mevcut prop sistemi REUSE** (cx) — yeni prefab YOK. Obstacle → `PropDefinitionSO` (`Assets/Data/Brush/Props/`) + `worldSprite` + footprint/blocking → `RoomTemplateSO.props`. Kapı → `GateBehavior.SpriteRenderer` (SE = SW flipX).
- **RoomType başına ayrı prop havuzu YOK** (lean) — tek ortak Shattered-Keep havuzu; oda kimliği layout+props+gate+düşmanla.
- **Import (onay sonrası):** PPU64, Point, alpha transparency, no mipmaps, pivot bottom-center (gate/obstacle) / center (reward). STAGING'de bekler → onay → import. Registry güncelle (`STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` + PLACEMENT_MANIFEST).

## ÜRETİM LİSTESİ (tiered — px = hedef import boyutu, cliff_S style-ref)

### MUST — blokerler (şimdi üret)
| İsim | Tarif | Footprint | px | Palet | Blocking |
|---|---|---|---|---|---|
| Runic Pillar | Dikey ikiye çatlamış granit obelisk, çatlaktan cyan sızar, parçalar hafif havada | 1 cell | 64×128 | %70 granit/%25 cyan | evet |
| Collapsed Barrier | Alçak, kırık köşeli taş blok barikatı, void külleri | 2 cell yatay | 128×64 | %90 granit/%10 void | evet (alçak) |
| Chained Seal Rift | Zemini patlatan parlak cyan girdap, çapraz demir zincirlerle kapalı yer-tuzağı | 2 cell | 128×64 | %50 cyan/%20 void/%30 demir | evet |

### SHOULD — dekor/ambiyans ("Room Decor" placeholder'larının yerine; "Shattered" hissi bunlardan)
| İsim | Tarif | Footprint | px | Blocking |
|---|---|---|---|---|
| Cyan-Vein Floor Decal | Zemin altından damar gibi yayılan cyan enerji çatlağı (floor overlay) | 1-2 cell decal | 128×74 | hayır |
| Floating Edge Debris | Ada kenarında void'de süzülen kırık taş parçaları | küçük | 64×64 | hayır |

### NICE — sonra
| İsim | px | Not |
|---|---|---|
| Sunder-Golem Remnant | 64×96 | dikey parlak landmark (boss/elite odası) |
| Rubble Scatter | 32×32 | collision'sız zemin süsü, çeşitlilik |
| Hanging Chains | 64×128 | collision'sız derinlik dekoru |

## 3-YÖN KAPI (final)
- **3 görünüm: Ön(S, kameraya bakar) + Sol(SW) + Sağ(SE = Sol'un flipX'i).** → **2 üretim** (Ön + Sol), Sağ Unity'de mirror.
- **Tasarım:** köşeli kırık granit kemer çerçeve + içinde dikey dönen yoğun cyan rift; rift'e değen taşlar keskin kırılıp enerjide süzülür (dikey-portal kararıyla uyumlu).
- **px:** 128×160 (2-tile geniş + kemer heybeti). pivot bottom-center.
- **Mirror disiplini (3.1 Pro):** büyük asimetrik hasarı KÖŞEYE değil MERKEZE/tepeye koy → SW↔SE flip'te "copy-paste" hissi olmaz. flat-lit olduğu için ışık sorunsuz.
- **Occlusion (3.5 Flash uyarısı):** Ön(S) kapı oyuncu görüşünü kısmen kapatabilir → mitigasyon: ince kemer (dolu duvar değil) + Y-sort (oyuncu eşikte doğru render) + cyan rift odak. Mockup bunu kabul-edilebilir gösterdi. **Fallback:** S playtest'te sorun çıkarırsa NW/NE arka kapıya geç.

## Wiring caveat'leri (cx — üretim sonrası iş)
- Obstacle: `PropRuntimeSpawner` rectangular konumlu → IsoRoomBuilder iso-prop spawn entegrasyonu `grid.CellToWorld` ile eklenecek (P2.5/P3).
- Kapı: `DoorTrigger` GateBehavior'ı AYNI GO'da arıyor ama GateBehavior child diyor — wiring'de aynı GO'ya koy ya da trigger mantığını güncelle (P5).

## Cleanup (bu kararla birlikte)
- `ObstacleInstances` (eski RoomBuilder: StoneColumn/NarrowPassage/Chasm) + "Room Decor" placeholder'ları = anlamsız → sahnelerden temizlenecek; yerlerine yeni prop sistemi + üretilen asset'ler.

---

# PART 2 — NLM CANON GROUNDING (2026-06-03, user: "lore-doğru + tam set + council onayı") — AUTHORITATIVE, yukarıyı override eder
Kaynak: NLM notebook 30ddffa5 query (full = tool-result b0xikacj6). Çakışmada bu bölüm geçerli (NLM canonical > local draft).

## LORE LOCK
- **The Fracturing:** Architect dünyayı kasten parçaladı (Rift March'ı durdurmak için). Adalar void üstünde yüzer.
- **Cyan #00FFCC** = Rift Enerjisi + çatlayan Antik Mühürler (Containment Seals); taşlar arasından sızan canlı/dengesiz güç. **Emissive** (Light2D değil, sprite emissive). Sahne genelinde ≤%15.
- **Void-mor #3A1A4A→black** = altta yutucu Abyss. **UNLIT** — objeler morla aydınlanmaz, mor = hiçlik bg.
- **Warm orange #E89020** = 2. accent, SADECE mangal/meşale/sıcak ışık (cyan'ın karşıtı).
- **Slate #3A3D42** = taş base. Palet kilidi: slate base / cyan ≤%15 / warm-orange 2nd / purple bg.
- **NÖTR ÇİZİM:** dinamik Light2D var → sprite'a gölge/cyan-glow BAKE ETME (baked+dynamic illüzyonu kırar). Taş = nötr; cyan = emissive katman.

## DOORS → "RIFT THRESHOLD" (canon)
- Klasik kapı DEĞİL: mimari eşik (taş kemer / yıkık duvar yarığı / köprü ağzı / sunak geçidi) içinde **dikey cyan rift** + **havada süzülen rün-ikonu** (oda tipini gösterir). Orb-travel ile geçilir.
- **⚠️ YÖNLER = K / D / B (GÜNEY YOK).** Canon: çıkışlar %25 K, %40 K+D, %25 K+B, %10 K+D+B; güney duvarı hep kapalı. → "düz/straight" kapı = **KUZEY (arka)**, yanlar = **Doğu(sağ) / Batı(sol)**. (Bu 3.5 Flash'ın occlusion endişesini de çözer — arka kapı oyuncuyu örtmez.)
- **3 görünüm üret: Kuzey(arka) + Batı(sol) + Doğu(=Batı'nın flipX'i)** → 2 üretim + 1 mirror.
- **Boyut:** gate ~1.5-2× karakter (Warblade ~120px) → hedef sprite ~160-200px yükseklik, ~128px geniş. pivot bottom-center.
- **Kompozisyon (modüler, canon):** (a) nötr taş eşik çerçevesi (3 yön) + (b) simetrik cyan rift dolgu (emissive) + (c) rün-ikon overlay per-oda-tipi (combat=çapraz-kılıç, elite=kafatası, treasure=kadeh, boss). İkon ayrı küçük sprite (Unity'de overlay). Rift simetrik olduğu için yön gerektirmez; sadece TAŞ ÇERÇEVE yönlü.

## OBSTACLE'LAR (canon Act 1 tablosu — collider tipi NLM keyword tablosundan)
| İsim | tile | Collider | px (hedef) | Kullanım | Palet |
|---|---|---|---|---|---|
| Taş Sütun (Pillar) | 1×1 | Capsule, block | 64×112 | taktiksel siper, çift→koridor | slate, cyan damar opsiyonel |
| Kırık Duvar (Wall Stub) | 2×1 / 3×1 | Box, block | 128×80 | L-cover, partial | slate |
| Kafes (Cage) | 1×2 | Box, block | 64×128 | ince siper + hapis dekoru | paslı demir + slate |
| Mezar Taşı (Tombstone) | 2×1 | Box, block (alçak) | 128×64 | crypt, alçak engel | slate + kemik tozu |
| Küçük Altar | 2×2 | Box, block | 128×128 | merkez tehlike, ritüel | slate + cyan rün |
| Chasm (Uçurum) | 3×2+ | (zemin yarığı, dash-gap) | 192×128 decal | geçilmez tehlike, dash-over | void purple + cyan kenar |
| Zincir Çapası (Chain Anchor) | landmark | Box (kenar, non-play) | 96×160 | Penitent teması, kenar | demir + slate |
- Kural: kapı önü 2-tile clearance; elite/boss MERKEZ BOŞ; sütun çifti ≥4 tile (dash); chasm kenarı 1-tile düz.

## DEKOR / AMBİYANS (canon — "Room Decor" + Shattered hissi)
| İsim | tip | px | block | not |
|---|---|---|---|---|
| Mangal/Brazier | ışık dekoru | 64×80 | evet (Circle) | **warm orange** ışık, canon 2nd accent |
| Meşale Direği | ışık dekoru | 32×96 | hayır (1 cell pathing) | duvar/zemin ışık |
| Urn Cluster | dekor | 64×64 | hayır | kırık küp kümesi |
| Rubble Pile | dekor | 64×48 | hayır | moloz |
| Tattered Banner | dekor | 48×96 | **hayır** (visual only) | yırtık sancak |
| Kemik/Ölüm-İşareti (bones) | dekor + mekanik | 64×48 | hayır | **canon: başarısız containment bedenleri** (rastgele değil); death-marker mekaniğine bağlanır |
| Zemin Çatlağı A (taş) | decal | 64×48 | hayır | slate+purple, unlit yapısal yorgunluk |
| Cyan Rift-Crack B | decal | 64×48 | hayır | **emissive cyan** sızıntı — "Shattered" özü |
| Kenar-Erozyon C | edge decal | 64×64 | hayır | void'e bakan dişli kopma, floating-island'ı destekler |
| Ritüel Dairesi / Seal-Socket | floor decal | 128×128 | hayır | cyan rün halkası |
| Map Fragment (Cartographer) | pickup | 48×48 | hayır (trigger) | **Kırık Taş Tablet**, cyan, oda-clear'da düşer |

## PRODUCTION REFERENCES (GERÇEK asset — concept resimleri DEĞİL, user 2026-06-03)
cx $imagegen üretiminde style/init reference olarak BUNLAR verilecek (oyunla birebir palet/pixel-density/stil eşleşmesi için):
- **Floor:** `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png` (gerçek granite iso zemin tile)
- **Cliff:** `Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png` (gerçek yönlü cliff — taş stili + cyan damar + void kenar)
- **Karakter (ölçek+palet+stil):** `Assets/Resources/Characters/Warblade/warblade_idle_SE.png` (gerçek in-game Warblade idle)
- ❌ `STAGING/imagegen/concept*_ISO.png` artık referans DEĞİL (sadece loose concept'ti; gerçek asset'ler kazanır).

## ÜRETİM SIRASI (canon, tam set — council onayından sonra cx $imagegen)
- **Batch 1 (MUST):** 3 kapı eşiği (K/B + E mirror) + rün-ikon (combat+elite) · Taş Sütun · Kırık Duvar · Kafes · Mezar Taşı.
- **Batch 2 (dekor/Shattered):** Mangal · zemin çatlağı A+B · kenar-erozyon C · kemik/death-marker · urn · rubble.
- **Batch 3 (zenginlik):** Küçük Altar · Zincir Çapası · ritüel dairesi · banner · meşale · Map Fragment.
- Hepsi tek-tek, `cliff_S.png` stil-ref, nötr çizim (gölge bake yok), cyan emissive ≤%15, warm-orange sadece mangal.

## COUNCIL-VALIDATION SONUÇLARI (2026-06-03, 2. council: cx bz2pjpkhi + 3.1 Pro bh4dn4lr2 + 3.5 Flash bjpyi60md)
- **3.1 Pro:** spec onaylandı + her asset için imagegen prompt taslağı yazdı; kapı=MODÜLER önerdi; +2 öğe (Mühür Halkası, Architect Geometrisi).
- **3.5 Flash:** Batch-1 = 8 asset (gate N+W, pillar, wall, chasm, riftcrack, brazier, bones); kategori-turları batching; ref palet/taşı kilitler ama izo-açı için METİN terimi şart; kapı=HİBRİT (frame+rift gömük, rün ayrı).
- **cx:** Batch-only collider'lar + PropDefinitionSO satırları + import tablosu verildi. **2 wiring BLOCKED_FLAG (post-production, üretimi engellemez):**
  1. `PropColliderAutoBuilder` her zaman Box yaratıyor → `colliderShape` (Capsule/Circle) onurlandırılmalı (Pillar/Brazier faithful collider için kod-fix).
  2. `GateBehavior` child rift/rün renderer'larını yönetmiyor → küçük sync component VEYA baked-composite fallback.
  3. **Chasm = sadece DECAL** (dash-gap kontratı implement değil; walkable-hole floor'u koparır → IsReachable reddeder). Gameplay-blocker YAPMA.
- **Kapı kararı (sentez):** HİBRİT — frame+rift tek sprite (N+W), rün-ikon AYRI 32×32 overlay (flipX'te rün ters dönmesin + oda-tipi dinamik). Tam-modüler (ayrı rift shader) = post-demo upgrade.
- **⚠️ ÖLÇEK DÜZELTME (user net):** Warblade görünür char = **64px = 1.0 world unit** (canvas 120px). Boyutlar 64px'e göre: gate 128×144 (~2×), pillar 64×96 (~1.5×). cx tablosundaki 120px-varsayımlı büyük boyutlar (gate 160-200, pillar 112) KÜÇÜLTÜLDÜ. [[project-character-64px-canvas-large-for-animation]]

## DÜZELTME ÖZETİ (PART 1'i override)
- Kapı: ~~Ön(S)/Sol/Sağ~~ → **Kuzey(arka)/Batı/Doğu** (güney çıkış canon'da YOK).
- Palet: +warm orange #E89020 (mangal). Cyan ≤%15, emissive, sprite'a bake YOK.
- Obstacle seti = canon tablo (pillar/wall-stub/cage/tombstone/altar/chasm/chain-anchor); torch/brazier = dekor, obstacle değil.
- Bones = canon anlatı öğesi (failed containers), Room Decor'un asıl doldurucusu.
