# TASK — Portal Pack Production Hazırlığı (T3 öncesi)

**Kaynak:** ChatGPT portal-pack review'ı (kullanıcı iletti, 2026-06-07) + Opus değerlendirmesi.
**Durum:** KUYRUKTA — screenshot seansı bitince başlar. Kullanıcı pack'i ChatGPT kanalıyla değerlendirdi (= fiilî onay sinyali; T3 wiring öncesi son teyit alınacak).
**NOT:** In-game placement kanıtı (ChatGPT B6) ZATEN ÜRETİLİYOR — UnityMCP screenshot seansındaki fig05 + Image #7 (2-exit case canlı görüldü: cyan Combat + yeşil Elite kemerler odada duruyor).

## Opus değerlendirmesi (ChatGPT maddeleri)

| Madde | Karar | Not |
|---|---|---|
| A1 "2 authored facing + 1 runtime mirror" dili | ✅ DONE | Rapor §3.5.5 düzeltildi (2026-06-07); R4 kararıyla zaten uyumluydu, "tek cephe" cümlesi yanlıştı |
| A4/B5 Telegraph'lar ayrı klasör | ACCEPT | Portal ≠ telegraph; import sırasında ayrıştır |
| B2 Boss angled = optional/parked | ACCEPT | Boss = final node, center N slot; angled üretildi ama wire ETME |
| B3 Elite v1 → deprecated | ACCEPT | Manifest'te DO_NOT_USE; prefab binding sadece v2 |
| B4 Reward/Boss 32px rünler | NEEDS_REGEN olarak işaretle | PASS değil; GATED PixelLab seansı listesinde zaten 2 rün var |
| C1 PORTAL_PACK_MANIFEST.json | ACCEPT | Alanlar: path/type/facing/socket/status/canvas/PPU/pivot/anchor'lar |
| C2 PORTAL_IMPORT_SETTINGS.md | ACCEPT-basit | Proje standardı ZATEN VAR: Point + PPU 64 + uncompressed (skill-ikon fix'i 7826cb10 ile aynı) — tek sayfa |
| C3 PORTAL_SOCKET_PLACEMENT_GUIDE.md | ACCEPT-kısa | R4 + GATESLOT decision'larından derle; EXIT_*/ENTRY_S = kavramsal ad, kod=door_* köprüsü ŞART |
| C4 1/2/3-exit + boss-center test screenshot'ları | KISMEN DONE | Screenshot seansı üretiyor; eksik kalan case'ler T3 verify'ında |
| D PortalSkinSO şeması | DEFER→T3 impl kararı | Makul ama mevcut IsoRoomBuilder.BuildExitDoors + GateBehavior mimarisiyle birleştirilmeli — T3 task'inde cx'e mimari seçenek olarak sunulur, sıfırdan paralel sistem YAZILMAZ (reuse-first) |

## Klasör hedefi (import sırasında)
- `Assets/Art/Portals/` (frontal+angled kemerler, core'lar)
- `Assets/Art/Portals/Runes/` (32px rünler; needs_regen olanlar girmez)
- `Assets/Art/Telegraphs/` (line/circle/cone seti)
- `Assets/Art/Boss/` (intro ring [VFX] ve ritual circle [zemin decal] AYRI işaretli)
- Elite v1 → `STAGING/imagegen/assets/_deprecated/`

## Uygulama sırası (T3 zinciri)
1. Manifest + import-settings + placement-guide yaz (Sonnet, doc işi)
2. Asset'leri klasör yapısına import et + Point/PPU64 ayarları (cx veya Sonnet-MCP)
3. T3 portal wiring (cx): BuildExitDoors → yeni kemer sprite'ları + tip-bazlı skin (PortalSkinSO kararı burada)
4. Verify: 1/2/3-exit + boss-center in-game screenshot seti
