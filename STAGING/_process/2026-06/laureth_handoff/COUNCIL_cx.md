# COUNCIL_cx - LaurethStudio mekanik/polish handoff karari (2026-06-15)

## S1 - Grup A
A1 Flowstep - POST: yeni STATE + dash-cancel/micro-step davranisi golden-path GREEN akisini destabilize eder; demo tezine degil combat hissine yatirim.
A2 Posture-Crack + SHATTERED glyph - POST: en guclu aday, ama mevcut kodda postureOverflow consumer TODO gorunuyor; BrokenStateVisual/crack altyapisina yaslanarak demo sonrasi alinmali.
A3 Aggression-Gated Heal - POST: iyi risk-odul ani uretir ama heal ekonomisi ve UI dengesi yeni sistemdir; 5 gunluk scope-lock icin fazla riskli.
A4 Sweet-Spot Damage - POST: tap-to-aim imzasina uyar fakat A2 ile carpim/stack riski yaratir; demo icin gerekli degil.
A5 Rolling/Deferred Health bar - POST: polish degeri var ama A3 ile ayni semantik alana girer; demo oncesi HUD davranisi degistirme.
A6 Accessibility Contract - POST: juri icin yararli olabilir ama UI/trigger/balance yeni yuzey acar; golden-path video odakli demoda sart degil.

## S2 - Grup B
B1 Fragment Contract Door - POST: canon-guvenli run-ici sistem, fakat yeni oda/kontrat karar yuzeyi demoya girmemeli.
B2 Hub Rift Script room-mutator - DROP(DEMO), CANON CONFLICT: Karar#25 kalici hub meta Faz 4-5; demo penceresinde acilmamali.
B3 Curse Gate - POST: opt-in risk/odul iyi, ancak tutorialize edilmis yeni oda tipi ve light/perf riski var.
B4 Resonance Forecast - POST: mevcut draft okunurlugunu artirabilir; post-demo Warblade-only en dusuk riskli derinlik adaylarindan biri.
B5 Echo Memory Bias + Heir Bequest - DROP(DEMO), CANON CONFLICT: kalici run-arasi bias Karar#25 kapsaminda; sadece run-ici varyant post-demo dusunulebilir.
B6 Luck stat - DROP: gorunmez stat ve RNG egri ayari demo/environment tezine deger katmaz.
B7 Placement-Driven Resource - POST: environment-combat koprusu guclu, ama geometry query ve denge yeni risk tasir.
B8 Status Layering/Element Manifold - DROP(DEMO): cok-sinif/Elementalist-Hexer derinligi; Warblade vertical slice icin yan konu.
B9 Fixed-Verb + Rotating Environmental Rule - POST: environment tezine en uygun Grup B adayi, ama demo oncesi oda kurali degistirmek golden-path riskidir.
B10 Drop&Bank at-risk currency - DROP(DEMO), CANON CONFLICT: hub-bank kalici meta Karar#25; run-ici retrieval bile demo tezine ikincil.
Post-demo en degerli 1-3: B9, B4, B1; gerekce: environment/draft/run-ici derinlik katarlar ve kalici hub-meta yasaagini delmezler.

## S3 - Polish
ZATEN VAR: HitStop, ScreenShakeDriver/ScreenShake/CameraShake, CameraPunchController, DamageNumberDriver/DamagePopup, EnemyTelegraph, PixelPerfectCamera/sub-pixel snap izleri, vignette varyantlari, squash/death residue, RiftGlow/RiftPulse, MapFragment, SkillOffer card anim, BossIntroController.
Tier-1 IN yok: baseline juice'in buyuk kismi zaten mevcut veya wiring/scene dogrulamasi ister; simdi eklemek batch-fix ve runtime verify oncesi yan yuzey acmak olur.
Tier-2 IN yok: #8 A2'ye bagli ve posture consumer hazir gorunmuyor; #9/#10 A3/A1'e bagli; #11-14 zaten kismen mevcut olsa bile yeni polish pass degil video koreografisi konusu.
Tier-3 POST: audio juice post-demo backlog'a yazilabilir, ama 20 Haz penceresinde yeni ses/VFX entegrasyonu golden-path'i profesyonellestirmekten cok test borcu yaratir.
Eger polish alinacaksa siralama: once 6 fix + console/runtime verify PASS, sonra sadece mevcut toggle/parametrelerin sahnede acik oldugunu kontrol et; yeni sistem kodlama yok.

## S4 - Meta
Paketin demo penceresindeki net degeri dusuk: oneriler kaliteli backlog malzemesi, ama kilit sunum tezi combat derinligi degil environment + edit-to-play + graphify-audit.
Orchestrator egilimi DOGRU: demo scope-lock HOLD edilmeli, paket post-demo backlog'a alinmali; tek ek notum A2'nin "mevcut Posture'a yaslanir" varsayimi kodda tam kanitli degil, once postureOverflow consumer tamamligi dogrulanmali.
Eksik kalan risk: LaurethStudio paketi "oyun hissi" icin degerli, fakat demo video akisi zaten GREEN ve en kritik is 6 cerrahi no-refactor fix; yeni mekanik girmek firsat maliyeti yaratir.

TEK CUMLE TAVSIYE: 20 Haz demosuna yeni mekanik veya polish kodu sokma; 6 fix + verification'i bitir, mevcut juice'u sahnede dogru kullan, A2/B9/B4/B1'i post-demo backlog'un tepesine koy.
