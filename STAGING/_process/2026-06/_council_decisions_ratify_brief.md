# COUNCIL — KARAR SETI RATIFY (red-team, LOCK oncesi)

Baglam: RIMA Unity 2D ARPG, bitirme demo ~20 Haz (6 gun). Bu session 6 karar uretildi (cogu zaten council'den gecti). LOCK oncesi SON red-team. Katildigin seyi YENIDEN TURETME — sadece HATA / kor-nokta / asiri-iddia / itiraz flag'le, + tek en-yuksek-getiri sonraki aksiyonu soyle.

## Kararlar
1. **Graphify config:** mimari harita=AST-only; bug-hunt/surpriz-coupling=deep+opus cerrahi dar-korpus (<=25 dosya); deep+sonnet ELENDI (duplicate-node topolojiyi zehirler); global LLM run=post-demo. (2x2 deney + council)
2. **Ponytail:** tam plugin SKIP; post-demo sadece /ponytail-review checklist'ini rima-qc/cx-review'a al. (4/4 council)
3. **F2 tani:** cerrahi graphify kirilma-noktalarini sıraladı (#1 RewardPickup.DraftThenOpenExit sessiz ShowDraft return huni...). HIPOTEZ kabul; fix'ten ONCE repro ile gercek tetikleyici dogrulanacak, 5'i korlemesine fixlenmeyecek.
4. **Full graphify map:** 624 dosya AST haritasi (6925 node) uretildi; eski 26-May graphify silindi; deger = god-node bulgusu (en bagli 10'un 6'si editor araci).
5. **Sunum vizyonu:** RIMA="oyun degil environment+ilk vertical slice"; domain-specific (genel framework/engine DEME); eksen %20 oyun/%60 mimari/%20 graphify-audit; centerpiece=Edit-to-Play timelapse video; graphify=mimari audit araci. (3-advisor council + graf verisi)
6. **Graphify update:** on-demand FULL rebuild (build_ast_map.py); per-commit hook/incremental DEGIL (incremental silinen node'u kaldirmaz=drift; maliyet clustering'de, AST bedava; haritaya ara sira bakiliyor).

## Orchestrator (Opus) gorusu
Anlati/strateji bitti. 6 gun kala asil risk ARTIK execution — F1/F2 bug fix + Edit-to-Play video kaydi — daha fazla analiz DEGIL. Gorkemli "environment" anlatisi ancak calisan/bug'siz demo ile hak edilir.

## Red-team sorulari
1. Hangi kararda HATA / kor-nokta / asiri-iddia var? (yoksa "hepsi saglam")
2. Update stratejisi (#6) dogru mu, yoksa per-commit/incremental daha mi iyi?
3. F2 (#3) "repro-once-then-fix" yaklasimi dogru mu?
4. Orchestrator'in "execution'a gec" gorusune katiliyor musun? Tek en-yuksek-getiri sonraki aksiyon ne?
Kisa, sadece itiraz + oneri.
