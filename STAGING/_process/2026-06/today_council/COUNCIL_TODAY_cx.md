# COUNCIL — Bugünün Planı (cx lens: KOD/ASSET GERÇEKLİĞİ + DEMO-GENELİ KÖK-NEDEN SWEEP)

ACTIVE RULES: (1) think before answering (2) min, no speculation (3) READ-ONLY analiz (4) BLOCKED if unclear.
⛔ READ-ONLY: dosya/Unity/git mutasyonu YOK. Unity runtime'a DOKUNMA (başka bir ajan şu an Unity'de Build-fix sürüyor — sadece kod/asset/doc OKU). Tek RESP dosyası yaz.
GRAPHIFY: cross-file taramada önce graphify query (`STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`).

## Bağlam
RIMA bitirme demosu **19 Haz = ~1.5 efektif gün**, hocaya canlı sunum. Tez: **tooling/architecture showcase** (%20 oyun / %60 mimari / %20 graphify) — animasyon-showcase DEĞİL.
**Bugün done+verified:** GATE (full-flow Director/F2 bootstrap), Boss P0 (residue/scale/crimson-bar/subtitle), reward-monolog-bleed, HUD readability. **Build Mode functional fix ŞU AN cx'te koşuyor** (paint-blocker/snap/phantom-asset).
**Tekrar eden DESEN:** "assert yeşil ≠ gerçekten çalışıyor" — Build '8/8 PASS' dendi ama floor/walkable-paint bozuktu (IsPointerOverUi tüm-ekranı blokluyordu); capture '25-state' dendi ama duplicate'lerdi. Bu desen demoyu tehdit ediyor.

## SENİN GÖREVİN: demo-geneli KÖK-NEDEN SWEEP (kod/asset)
1. **Latent demo-blocker tara:** `IsPointerOverUi` gibi **over-broad guard** başka nerede var (tüm-ekran raycaster, yanlış erken-return, scene-guard'lar)? Demo akışında patlayabilecek null-deref/lazy-singleton-dirilme/init-ordering riskleri? (graphify ile god-node/çağrı-yolu tara.)
2. **Phantom/kırık asset:** barrel gibi sprite'sız/eksik-referanslı kaç propdef/tile/asset katalogda listeleniyor ama placeable değil? Liste ver (Data/Brush/** + asset katalog). Demo'da görünür olanlar.
3. **Anim/sprite reality:** Repo'da hangi karakter anim state'leri WIRE'lı vs eksik (weaponless 4/4 generated ama Unity'ye import/wire edildi mi?)? Mob'ların sprite/anim durumu (black-blob neden — sprite mi yok, anim mi yok, material mi)? PixelLab'den üretilmiş ama bağlanmamış asset var mı?
4. **"Yeşil-assert ≠ çalışıyor" başka nerede:** hangi sistemlerin assert'i dar-kapsam (gerçek-akışı test etmiyor)? Demo'da elle test edilmesi gereken riskli noktalar.

## ÇIKTI (E1: dönüş ≤10 satır)
RESP → `STAGING/_process/2026-06/today_council/RESP_TODAY_cx.md`. Format: 4 başlık altında **kanıtlı (dosya:satır) bulgular** + "demo'yu en çok tehdit eden 3 kök-neden" + "bugün elle-test edilmesi şart 3 nokta". Dönüşte: RESP yolu + 8 satır en kritik.
