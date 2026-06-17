# TASK 13 — Mob/Char sprite WIRING araştırması (READ-ONLY analiz, NO-Unity)

ACTIVE RULES: (1) think before answering (2) min (3) READ-ONLY — kod/asset/git/Unity mutasyonu YOK (4) BLOCKED if unclear.
GRAPHIFY: cross-file/asset taramada önce graphify query (`STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`).

## ⛔ READ-ONLY + NO-UNITY
Bu sadece KOD/ASSET analizi. Unity'ye DOKUNMA (başka ajan Unity'de Director-skin sürüyor). Hiçbir dosya değiştirme, git'e dokunma. Tek RESP dosyası yaz.

## Bağlam
cx-sweep bulgusu (#5/#7): **PixelLab mob sprite'ları üretilmiş ama Unity'de bağlı/erişilebilir DEĞİL** → "black-blob" düşmanlar. ChainWarden/VoidThrall/RelicCaster controller'ları `fileID 0`; demo-wave FractureImp/Penitent/HalfThrall EnemyAnimator fallback'iyle maskeleniyor; Player.prefab controller/sprite `fileID 0`. Bunları WIRE ederek (kredisiz) okunabilirlik düzelecek. Sen WIRE-PLANI çıkar.

## Üret: WIRE-PLAN (her madde dosya:satır/GUID kanıtlı)
1. **Hangi enemy prefab'ları bozuk:** `Assets/**/*.prefab` içinde Animator controller/SpriteRenderer sprite `fileID: 0` olan TÜM enemy/mob prefab'larını listele (ChainWarden/VoidThrall/RelicCaster/FractureImp/HalfThrall/Penitent + varsa diğerleri). Her biri için: prefab yolu + eksik olan (controller mı, sprite mı, ikisi mi).
2. **Mevcut sprite/anim envanteri:** Repo'da bu mob'lar için import edilmiş PixelLab sprite/sheet/AnimatorController VAR mı (Assets/Art|Sprites|Animations altında)? Hangi mob'un asset'i mevcut ama referanslanmamış, hangisi gerçekten eksik (yeni PixelLab A2-batch'i bekliyor)?
3. **Runtime loader:** mob prefab'larını yükleyen sistem (EnemyAnimator/spawn/RoomInstance) sprite'ı nereden çözüyor — prefab-ref mi, Resources/Addressable mi, by-name mi? Wire için NEREYE bağlanmalı?
4. **Player.prefab:** controller/sprite fileID 0 — weaponless state'leri (idle 89823ecc/run ccbc13ed/windup 3b8dad34/flinch ebd7f8af) wire etmek için player'da hangi alan/Animator gerekli?
5. **Minimum wire-adımları:** demo için "her mob okunur sprite gösterir" hedefine ulaşmak için EN AZ ne wire edilmeli (mevcut asset'le) + ne A2-batch'ten (yeni üretilen) gelecek. Cerrahi adım listesi.

## ÇIKTI (E1: dönüş ≤10 satır)
RESP → `STAGING/_process/2026-06/demo_fix_tasks/DONE_13_wire_plan.md`. Dönüşte: bozuk-prefab sayısı + mevcut-asset-var/yok ayrımı + loader-çözüm-yolu + minimum-wire-adımları özeti + RESP yolu.
