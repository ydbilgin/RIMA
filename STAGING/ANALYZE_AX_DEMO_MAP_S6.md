# AX (Gemini) ANALYSIS TASK — Demo Map: Design / Look / Feel

NLM ACCESS: RIMA design context için NLM sorgula:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Amaç
Opus otonom "güzel seamless izometrik demo map" yapacak. Sen DESIGN/LOOK analizi yap (kod değil). Opus son kararı verecek. Çıktını net, maddeli ver.

## Bağlam (locked)
- Perspektif = İZOMETRİK. Floor = seamless tile (PixelLab). Cliff = floor'un altına matematiksel oturur = derinlik (floating island look).
- Canon: void'de yüzen kırık-kale adası, charcoal/iron taş + cyan seal-energy aksanı (sparing), Hades/Bastion mood.
- Karakter/anim/mob HARİÇ (kullanıcı yapacak). Biz environment + map yapıyoruz.

## Mevcut asset (yeniden üretim YOK, bunlarla çalış)
- Floor: 16 düz + 16 iso seamless tile.
- Cliff: 8-yön (N/E/S/W + köşeler) + cyan_glow + corner_fade + edge_ao_rim.
- Decor (16): brazier_lit/unlit, banner, rift, rubble, rune, sarcophagus, seal_circle, slab_crack, bones, moss, bricks, blocks, debris.
- Kit: brazier, pillar_broken, rubble, wall_tower.

## Sorular
1. **Seamless iso map LAYOUT:** Bu asset'lerle "güzel, dolu ama dağınık değil" bir demo oda nasıl kompoze edilir? Floor şekli (kare/elmas/organik), cliff kenar kullanımı, obje yerleşim yoğunluğu (merkez-hero / çevre / parallax 3-zon). Somut bir oda taslağı ver (grid üstünde nereye ne).
2. **Cliff derinlik hissi:** Cliff'ler floor altına otururken "ada havada yüzüyor" hissi için cliff yüksekliği, AO/contact-shadow, cyan-glow nerede kullanılmalı? %limit?
3. **Demo akışı:** 2-3 odalık bir demo map için oda çeşitliliği (combat arena / dinlenme / boss-önü) nasıl farklılaşır — aynı asset setiyle?
4. **Eksik parçalar:** Kapı/gate, map-fragment (StS branch), reward pickup placeholder'ları imagegen ile üretilecek. Bunların on-brand iso görünümü ne olmalı (kısa prompt önerisi her biri için)?
5. **Referans:** Hangi oyunların environment'ı bu look için doğru referans (1-2 satır neden)?

## Çıktı
Maddeli, somut, "Opus şunu yapsın" netliğinde. Spekülasyon değil, uygulanabilir. AGY_DONE_<account>.md'ye yazılır.
