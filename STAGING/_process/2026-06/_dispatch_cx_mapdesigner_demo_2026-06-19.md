# GÖREV: RIMA Map Designer + Room Painter — canlı demo doğrulama + adım-adım akış

ACTIVE RULES: (1) think (2) min/surgical (3) BLOCKED if unclear.
**KOD DEĞİŞİKLİĞİ YOK, GİT YOK, ASSET/SCENE KAYDETME YOK.** Sadece doğrulama (edit-mode/runtime probe + execute_code OK; ama hiçbir şeyi disk'e kaydetme — _Arena/scene değişirse KAYDETME/revert et).
UNITY ERROR CHECK: iş bitince `read_console` (Error+Warning); kendi hatanı çöz, önceden-var/ilgisiz bildir; console durumunu raporla. (MCP `ExecuteCode` "objects not cleaned up" = bilinen tooling artefaktı, görmezden gel.)

## AMAÇ
Kullanıcı hocaya iki Unity editör aracını CANLI gösterecek: **RIMA/Map Designer** ve **RIMA/Room Painter** — tile koy → "Generate Cliff" bas → cliffler gelsin → oda üretilsin. Bunun GERÇEKTEN çalıştığını doğrula + hocaya net adım-adım demo akışı çıkar. **Demo-kritik: buton önceden "hiçbir şey yapmıyordu" (CliffGenerateAction.cs yorumu) — çözüldüğünü kanıtla.**

## DOSYALAR
- `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` — MenuItem `RIMA/Map Designer`; `RoomTemplateBuildUtility.BuildInArena(...)`.
- `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` — MenuItem `RIMA/Room Painter`.
- `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs` — cliff üretim mantığı (eski "button does nothing" kök neden notu).
- Runtime cliff: `Assets/Scripts/RoomPainter/RoomCliffSolver.cs`, `Assets/Scripts/Environment/CliffMeshGenerator.cs` / `CliffAutoPlacer.cs` / `DirectionalCliffTile.cs`, `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`.
(Çok-dosya akış için graphify query kullanabilirsin: graph.json `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`.)

## YAPILACAK
1. Bu dosyaları oku → **gerçek kullanım akışını** çıkar: pencere nasıl açılır (menü), tile/zemin nasıl boyanır (paint/brush), **"Generate Cliff" butonu NEREDE ve ne yapar**, oda nasıl materyalize olur (BuildInArena / IsoRoomBuilder), ekranda ne görünür (cliff-tile yüzen ada).
2. **DOĞRULA (Unity, edit-mode):**
   - `RIMA/Map Designer` ve `RIMA/Room Painter` pencereleri açılıyor mu (ShowWindow), console temiz mi?
   - **Cliff-generate GERÇEKTEN cliff üretiyor mu?** Küçük bir test odasında birkaç tile koy (veya RoomData'yı programatik kur) → cliff-generate'i tetikle (buton handler'ının çağırdığı `CliffGenerateAction`/`RoomCliffSolver` yolunu execute_code ile çalıştır) → cliff hücreleri/kenarları üretildi mi KANITLA (önce/sonra cliff sayısı).
   - Buton→handler bağlı mı (kod kanıtı: butonun handler'ı cliff-generate'i çağırıyor; eski bug gerçekten çözülmüş mü).
   - `BuildInArena` odayı sahneye basıyor mu (cliff'lerle)? (Kanıtla; ama sahneyi KAYDETME.)
3. **Canlı demo gotcha'ları:** edit-mode mi gerek, tile palette/asset yüklü olmalı mı, hangi sıra, "Generate Cliff" butonunun tam yeri/etiketi, oda Arena'da nasıl görünür, demo sırasında nelerden kaçınmalı.

## ÇIKTI
`STAGING/_process/2026-06/DEMO_MAPDESIGNER_FLOW_2026-06-19.md`: 
- HOCAYA adım-adım canlı demo akışı (numaralı, tıkla-tıkla; menü yolu → tile koy → Generate Cliff → oda).
- Her kritik adımın PASS/FAIL doğrulaması + kanıt (cliff üretildi mi, pencere açıldı mı, console).
- Gotcha/risk listesi.
Dönüşün ≤10 satır: workflow ÇALIŞIYOR mu (cliff üretiliyor + oda build), demo-flow dosya yolu, console durumu, en önemli gotcha.