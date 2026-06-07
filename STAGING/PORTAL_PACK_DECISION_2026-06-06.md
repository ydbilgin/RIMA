# PORTAL PACK — ROUND-2 COUNCIL KARARI (2026-06-06)
**Konu:** "RIMA Portal Only Updated Pack" (`STAGING/_incoming/portal_pack_2026-06-06/`) değerlendirmesi.
**İlişki:** Round-1 kararını (`V2_PLAN_DECISION_2026-06-06.md`) TAMAMLAR — özellikle öncelik #3 (kapı portal görsel katmanı) bu kararla detaylanır. Yeni üst-seviye iş AÇILMAZ.
**Council:** cx-laurethayday (asset/kod envanteri, `CODEX_DONE_laurethayday.md`) ‖ ax-3.1-Pro (mimari) ‖ ax-3.5-Flash (lean) ‖ Opus-advisor (tasarım, konsept sheet'leri görerek) → Opus orchestrator sentezi.

## ANA SENTEZ BULGUSU
Paketin doktrin katmanı (floating-island, duvarsız, portal=kapı, 1 yön + 3 arka slot + güney giriş) **bugün ship edilenle birebir örtüşüyor** — onay niteliğinde, iş üretmiyor. Asset listesi (~30 parça) ise cx envanteriyle çürütüldü: **kemer, rift çekirdeği, crack decal, toz/sis, prop sanatının çoğu DİSKTE VAR.** Gerçek boşluk = `IsoRoomBuilder`'da tip→skin eşlemesi (kod, S/M) + 2 rün + 1-2 landmark prop + kayıt (registry) işçiliği.

## ÇATIŞMA ÇÖZÜMLERI (advisor uyuşmazlıkları → kararım)

| Çatışma | Taraflar | KARAR |
|---|---|---|
| **Slot modeli** | 3.1: "3 ayrık slot'a geç (organik şekilde void'e taşma)" vs bugün ship edilen merkez-anchor+clamp | **Merkez-anchor KALIR.** Smoke 26/26 + _Arena probe kanıtlı; clamp var. EK: validator'a "3-kapı yayılımı walkable mı" kontrolü eklenir — FAIL eden template olursa SADECE onlara elle offset yazılır. Kanıtsız migrasyon yok. |
| **Prefab vs kod-inşa** | 3.1: prefab'a derhal geç · cx: serialized skin tablosu yeter | **HİBRİT:** tek `DoorPortal` prefab'ı (frame+core+rune+label+VFX katmanları içinde, Y-sort prefab'da statik) + `PortalSkin[]` serialized tablo (`RoomType→frame/rune/tint/label/vfx yoğunluğu`). `BuildExitDoors` Instantiate eder, returned-list sözleşmesi DEĞİŞMEZ. |
| **Cliff kiti** | 3.1: straight/damaged drop-in · Flash: hiç dokunma · cx: endcap/köşe first-class değil, düşük ROI | **SKIP (Flash+cx).** Köşe/endcap = sistem rewrite, RED. Ön kenar screenshot'ta bozuk OKUNURSA tek iş: damaged-front varyantını mevcut tile kuralına S-boyut enjekte. Kusur maskeleme = mevcut rubble prop + sis. |
| **Ritüel halka decal** | Flash: ucuz kazanç · Opus: gürültü, sadece Boss | **OPUS + cx köprüsü:** büyük ritüel halka = SADECE Boss portalı (tören hissi). Normal portalların dibine mevcut `FloorRiftCrack` decal'i (64x48, kayıtlı, küçük) — zemine oturma hissi, halka töreni değil. |
| **Arka kenar** | 3.1: tamamen açık void (sorting riski) · Opus: alçak parapet (yoksa bitmemiş durur) | **KANITLA KARAR:** Önce açık void (bedava) → ScreenshotMode ile kare al → "asılı/bitmemiş" okunursa `wall_low_parapet.png` (MEVCUT, RuinedKeepKit) 3-4 segment ELLE prop olarak (prosedürel değil) portal sırasının arkasına. İki advisor da kısmen haklı; ucuz yoldan başla. |
| **Portal tip sayısı** | Paket: 5 · Opus: 3+1 (Heal/Lore = girilemeyen oda tuzağı) | **4 SKIN:** Combat / Elite / Reward / Boss. cx kanıtı: `RoomType.Chest` VAR (chest template'leri bank'ta) → Reward ships. Heal/Lore = post-demo (graph'ta oda tipi yok). |
| **Portal boyutu** | Paket: 96x128-128x160 · Opus: karakteri ezmesin | **~1.6× karakter (≈96-112px efektif), Boss istisna 2×.** Mevcut gate_north 128x144 → runtime scale ile ayar; yeni kemer sanatı ÜRETİLMEZ (4 mevcut aday: gate_arch / portal_arch_gen / arch_gate / act1_arch_exit_cyan_rift). |
| **Konsept sheet kullanımı** | — | **Sadece kompozisyon/silüet referansı.** Direkt pixelify YOK (PPU uyumsuz silüet üretir). Cyan disiplini sheet'te zaten doğru. Sheet'in "High risk" caption'ları in-game'e GİRMEZ (round-1 pip reddi geçerli; ödül metni "Rare+" istisnası kalır). |

## RED LİSTESİ (round-2 ek)
Heal/Lore portal tipi · cliff köşe/endcap sistemi · prosedürel parapet · giriş PORTALI objesi (FSM kirletir — 3.1+Opus mutabık; yerine stateless varış VFX'i) · her portala ritüel halka · konsept sheet pixelify · 11 prop'un 7'si (kemik yığını landmark olarak, zincir/bayrak, kırık heykel parçası, urn/mum, rubble landmark — kayıtlı küçük filler'lar zaten var).

## YAPILACAK İŞ (round-1 kuyruğuna gömülür, yeni slot YOK)

### → Round-1 #3 "Kapı portal görsel katmanı" şu hale gelir [M toplam]:
1. `DoorPortal` prefab + `PortalSkin[4]` tablosu (`IsoRoomBuilder.BuildExitDoors` Instantiate'e geçer, sözleşme korunur) — **kod, cx'e**
2. Skin içerikleri MEVCUT sanattan: kemer (4 adaydan seçim) + `portal_rift.png` çekirdek + `RiftGlowVFX` idle + tip tint'i (Combat=cyan/beyaz · Elite=magenta+çatlak overlay · Reward=soluk altın · Boss=kızıl+yavaş nabız) + `FloorRiftCrack` taban decal'i
3. Reward portalına küçük "Rare+" TMP etiketi (UI değil world-space, okunur)
4. Açılma tween'i (Sealed→Open, 2 state; FSM yok) + yaklaşınca highlight ([G]-interact altyapısı reuse)
5. Validator: 3-kapı yayılım walkable kontrolü
### → Round-1 #3 ile #4 arasına mikro-slot: **ENTRY_S varış VFX'i [S]** — `EnsurePlayerAtSpawn` sonrası cyan halka genişleme 0.4s + toz + alpha fade-in 0.3s + 1 SFX. Stateless.
### → Round-1 #6 (ölüm/zafer): Boss portal istisnaları (2× boyut + ritüel halka decal + kızıl rün) burada.
### → Round-1 #8 (void/parallax): `CliffEdgeDustEmitter` + `bg_L4_fog` + portal-içine-akan partikül (Flash'ın "portal gravity" önerisi — ucuz odak çekici); Seal Monolith + Rift Crystal landmark yerleşimi.
### → Prop registry işçiliği [S, cx]: mevcut başıboş sanatı `PropRegistry`'ye kaydet: rift_crystal, rubble_pile, pillar_broken, statue_toppled, altar. (Sanat üretimi DEĞİL, wiring.)

## GATED PixelLab LİSTESİ (kullanıcıyla, indirgenemez sanat — toplam 4-5 parça)
1. **Reward rünü** 32×32 (rune_combat/rune_elite mevcut → sadece 2 eksik)
2. **Boss rünü** 32×32
3. **Boss ritüel halka decal'i** ~256×256 alpha
4. **Seal Monolith** landmark (~128×160) — paketin en yüksek kimlik-değerli eksik prop'u
5. (opsiyonel) Pedestal premium yenileme — Chamber'da mevcut `echo_pedestal` yeterliyse SKIP

## BRAZIER UZLAŞISI (round-1 sıcak-ışık uyarısı ile)
Brazier prop'u kalır (kayıtlı, canon) ama default SÖNÜK; YANAN instance sadece Chamber + Boss girişi (≤2). Cyan-on-void imzası bozulmaz.

## AÇIK KARARLAR (kullanıcıya)
1. Round-1'den taşınan: **Boss** minimal polish mi, "boss=oda" redesign mı? (önerimiz: minimal, post-demo redesign)
2. Bu karar + round-1 öncelik sırasıyla **otonom uygulamaya başla** onayı (1: ScreenshotMode → 2: juice+SFX → 3: portal katmanı [bu doküman] → ...).
