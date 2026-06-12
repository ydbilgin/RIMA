# A) Tasarım Review + Geliştirme

## Genel karar
Council’ın **sol-rail, 6 sekmeli, chrome-skinli uGUI Director Mode** tasarımı doğru yön. Üst bar kullanmamak iyi karar; RIMA’da top-down/free-cam okunurluğunu öldürmeden tool’u görünür kılıyor. Bu tip tool’da asıl risk “debug panel” kokması. Chrome kit’in kullanılması bu riski azaltıyor.

## Onaylanan ana yapı
- Master toggle: ` backtick` ile DIRECTOR / TEST geçişi.
- DIRECTOR: `timeScale = 0`, kamera free-cam, uGUI aktif, world edit açık.
- TEST: `timeScale = 1`, oyuncu input aktif, UI minimize, telemetri şeridi kalır.
- Giriş/çıkış tween ve kamera hareketi **unscaledDeltaTime** ile çalışmalı.
- Sekmeler Destroy/Instantiate değil, `CanvasGroup` ile aç/kapa olmalı.

## Sekme sırası için öneri
Council sırası genel olarak iyi, ama demo sunumu için şu akış daha güçlü:

1. **Spawn** — en hızlı “bak canlı sistem” etkisi.
2. **Class & Skill** — class swap + skill override, sandbox’ın oyunla bağlantısını gösterir.
3. **Stats** — ClassStatRuntime slider’ları, denge kararını görünür yapar.
4. **Build** — Tile/Cliff/Prop, daha teknik ama etkileyici.
5. **Map** — node jump + seed reroll.
6. **Telemetry** — export, DPS/TTK, profesyonel dengeleme kanıtı.

Neden? Kullanıcı veya hoca önce sahnede düşman, karakter, sayı ve sonuç görmeli. Map/Build güçlü ama teknik; ilk izlenimde spawn+stats daha anlaşılır.

## UX düzeltmeleri
- **Start butonu tek anlamlı olmalı:** DIRECTOR’da “BAŞLAT”, TEST’te “DIRECTOR’A DÖN”. Aynı yerde, aynı chrome.
- **World interaction state bar:** Alt telemetri şeridinde mod kısa yazmalı: `PLACE`, `ERASE`, `PAINT`, `INSPECT`.
- **Sağ tık davranışı tutarlı:** Spawn’da sil, Tile’da erase, Prop’ta remove. Her sekmede aynı kas hafızası.
- **Brush preview zorunlu:** Ghost preview yoksa oyuncu yanlış yere koyar, sonra suçu tilemap’e atar. Klasik insan savunma refleksi.
- **Selection inspector:** Bir mob/prop/node seçilince sağda küçük `tooltip_box` ile ID, HP, AI mode, source prefab gösterilmeli.
- **Quick reset Başlat anında snapshot almalı:** Snapshot yoksa sandbox değil, sadece kaotik sahne bozma makinesi olur.

## Görsel karar
- Ana panel: `minimap_frame` 9-slice.
- Rail slotları: `slot_normal` / `slot_active`.
- Büyük aksiyon: `ribbon_base`.
- Küçük buton: `menu_button`.
- Palette hücresi: `slot_normal`.
- Skill/preset kartları: `reward_card`.
- Tooltip / inspector: `tooltip_box`.

## Kritik riskler
- `SpawnEnemy(id, pos)` imzası ilk iş doğrulanmalı.
- `PaintCell` public yapılırken eski IMGUI overlay ile çakışma olmamalı.
- Cliff/Prop regenerate sahnedeki manuel işleri ezmemeli; scope ve undo kaydı şart.
- `timeScale=0` yüzünden kamera/tween/input donabilir; tüm Director loop unscaled çalışmalı.
