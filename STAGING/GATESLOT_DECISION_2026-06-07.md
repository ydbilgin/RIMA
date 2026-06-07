# GATE-SLOT SİSTEMİ — COUNCIL ROUND-3 KARARI (2026-06-07)
**Konu:** Kullanıcı isteği — Map Designer'da template başına authored gate slot'ları: ENTRY belli + 3 EXIT slotu, runtime graph'a göre 1/2/3 kapıyı O slot'larda render eder. ChatGPT konsolide önerisi de değerlendirildi.
**Council:** cx-laurethayday (kod kanıtı) ‖ ax-3.1-Pro (mimari) ‖ ax-3.5-Flash (lean) ‖ Opus-advisor (tasarım) → Opus orchestrator sentezi.
**Statü:** Round-2'deki "merkez-anchor kalır" kararını KULLANICI TERCİHİ ile SUPERSEDE eder.

## OYBİRLİĞİ (4/4)
- **Seçim kuralı:** `1 kapı → N` · `2 kapı → NW+NE (simetrik)` · `3 kapı → NW+N+NE`. Atama deterministik: graph children soldan sağa index sırası (`child[0]→en sol`). Kapı listesi sırası = choice index (RoomRunDirector trigger sözleşmesi AYNEN).
- **Sıra:** Slot sistemi T3'ten ÖNCE bağımsız ön-task (data doğruluğu commit edilir → T3 portal prefab'ı garanti koordinatlara iner; BuildExitDoors'taki spacing/clamp matematiği SİLİNİR).
- **Migrasyon:** elle düzenleme YOK — Fix Sockets genişletilir, 25 template'e yeniden koşulur (eski door_W/E yan socket'leri silinir).
- **Güvenlik ağı:** slot geçersiz/yetersizse → eski anchor-row fallback + uyarı. ASLA void üstünde portal.
- **ENTRY = mevcut playerSpawn.** Fiziksel giriş objesi YOK (round-2 kararı geçerli); designer önizlemede "ENTRY" etiketi + oyunda stateless varış VFX'i (T5).

## ÇÖZÜLEN ÇATIŞMALAR
| Konu | Görüşler | KARAR |
|---|---|---|
| **Şema** | Opus: 3 yeni Vector2Int alan · 3.1: ExitRole enum · cx: socketId konvansiyonu | **cx kazandı: SIFIR şema değişikliği.** Mevcut `doorSockets` listesi + `door_NW_01 / door_N_01 / door_NE_01` ID konvansiyonu (hepsi direction=North, isExit=true) + küçük slot-resolver. Gerekçe: validator/QC listeyi jenerik okuyor (RoomTemplateValidator.cs:84-118), tüm ID'ler zaten string-konvansiyon, 25 asset serialization'ına dokunulmaz. Enum = post-demo refactor notu. |
| **Designer UX** | 3.1: SceneView tutamaç · cx+Opus+Flash: otomatik+etiket+Inspector | **Otomatik yerleşim + önizleme etiketleri + Inspector nudge (S).** UnifiedMapDesigner.cs:376-392 zaten socket+spawn çiziyor — sadece NW/N/NE renk/etiket ayrımı + ENTRY etiketi eklenir. Tıkla-sürükle/SceneView tutamaç = Phase-2 (Flash: 6-8h vakit bombası; ancak T9 cilasında gerçek sürtünme çıkarsa). |
| **Kapasite ↔ Graph** | Opus: runtime'da kapı düşür · 3.1: havuz filtresi | **İKİSİ DE (katmanlı):** birincil = 3.1'in tek-yönlü havuz filtresi (`ValidSlotCount >= node.childCount` olmayan template o derinlikte SEÇİLMEZ — graph kör kalır, otoritesi bozulmaz); ikincil = runtime fallback (drift'e karşı kemer-pantolon askısı). |
| **Semantic zone'lar (ChatGPT önerisi)** | — | **ELLE AUTHORING RED — TÜRETİLMİŞ overlay.** Combat Core/Edge Filler/Forbidden mantığı kodda zaten var (CompositionRoleMap + CleanCenter + PropFootprintValidator). Zone'lar validator/önizleme görselleştirmesi olarak GÖSTERİLİR, 26 template'e elle çizilmez. |

## VALIDATOR KURALLARI (birleşik liste)
**MUST (kaydı/audit'i bloklar):**
1. Slot hücresi walkable + kuzey komşusu non-walkable (mevcut IsDoorEdge kuralı — "arka kenar" garantisi)
2. Slot'un güney koridoru (2 hücre) walkable (oyuncu portala ulaşabilir; cliff-dikiş çakışmasını da önler)
3. Slot'lar birbirinden ≥3 hücre ayrık (portal sprite ~1.6× karakter; <4'te uyarı)
4. 3 slot DISTINCT + South exit yasak (mevcut)
5. spawn'dan her slota ≥4 hücre mesafe (mevcut kural korunur)
**WARN (uyarır, bloklamaz):**
6. |Y_NW − Y_NE| ≤ 2 (yatay hiza — auto-fixer mümkünse eşitler; hilal/ark için gevşek)
7. Slot, authored prop bölgesine <2 hücre
8. spawn→slot flood-fill erişilebilirlik (donut/kopuk segment tespiti)

## AUTO-PLACEMENT KURALI (Fix Sockets genişletmesi)
Kuzey-kenar walkable+IsDoorEdge hücrelerini topla → x'e göre sırala → bitişik x-segmentlerine kümele → 3 ayrık slot verebilen EN BÜYÜK segmenti tercih et → sol-üçte-birden NW, merkezden N, sağ-üçte-birden NE (min-ayrıklık zorla) → segment yetmezse: mevcut segmentlerden sıralı seç + uyarı listesine yaz (sessiz fail YOK) → hiç olmuyorsa slot boş kalır + template uyarı listesi (designer Inspector'dan nudge'lar).

## RUNTIME ALGORİTMA (BuildExitDoors)
```
slots = template.ResolveExitSlots()        // [NW?, N?, NE?] konvansiyon-resolver, sıralı
n = doorTypes.Count                         // 1..3 (graph)
if (geçerli slot < n) → ESKİ anchor-row fallback + tek uyarı (template adıyla)
eşleme: n==1 → [N] (N yoksa en merkeze yakın geçerli slot)
        n==2 → [NW, NE] (biri yoksa N + diğer kanat)
        n==3 → [NW, N, NE]
her (child_i, slot_i) → kapı objesi slot konumunda (T3'te DoorPortal prefab + PortalSkin[child.roomType])
return doors  // sıra = choice index, sözleşme değişmez
```
+ RoomRunDirector template seçiminde havuz filtresi: `t.ValidExitSlotCount >= node.childCount`.

## ACCEPTANCE CRITERIA
- Fix Sockets sonrası: Audit 25/25 temiz (Chamber hariç) + Smoke 26/26 + 0 exception
- _Arena play-probe: 1, 2 ve 3 kapı senaryoları en az dikdörtgen + donut + hilal template'lerinde doğru slot'larda (2 kapıda merkez BOŞ)
- Rooms tab önizleme: ENTRY (yeşil, etiketli) + NW/N/NE (ayrı renkler, etiketli) görünür
- Trigger/choice-index sözleşmesi bozulmadı (kapıdan geçiş doğru node'a ilerler)
- EditMode testler güncel + yeşil (RoomTemplateSocketTests 3-slot kuralına göre)

## YAPILMAYACAKLAR
Tıkla-sürükle slot editörü (Phase-2) · ExitSlot enum şeması (post-demo refactor) · elle semantic zone authoring · entry portal objesi · graph'ı odaya göre yeniden dengeleme · shape-aware otomatik snap sihirbazlığı (validator + nudge yeter) · Heal/Lore slot'u.

## BOYUT + ROUTING
Toplam **[M]**: Fix Sockets genişletme + resolver + BuildExitDoors + validator + önizleme etiketleri + test güncelleme + 25-template re-author + doğrulama. Tek cx task'i; ax-Opus-4.6 review. T3 (portal prefab) bunun ÜSTÜNE oturur.
