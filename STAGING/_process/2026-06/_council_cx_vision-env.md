ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA'yi hocaya bitirme sunumunda "tekrar-kullanilabilir oyun-gelistirme environment'i" olarak cerceveleme + graphify'i sunum-araci olarak kullanma stratejisini FIZIBILITE/SOMUT lensinden degerlendir.

## Baglam
RIMA = Unity 2D top-down ARPG (C#). Bitirme demosu ~20 Haz. Kullanicinin asil hedefi: sadece bu oyun degil, SONRAKI oyunlarda da kullanacagi bir environment/framework (in-game edit mode, room builder, director tools, stat sistemi vb. ZATEN var). Ayrica graphify (kod->bilgi grafigi+HTML viz) ile mimari baglantilari tarayip dogruluyor; bunu sunumda sofistikasyon sinyali olarak gostermek istiyor.

## Lens: feasibility / somut / reuse. ANALYSIS ONLY. Sonucu CODEX_DONE.md'ye yaz.
1. RIMA'da GERCEKTEN tekrar-kullanilabilir olan ne? (edit mode, room builder/IsoRoomBuilder, DirectorMode tool, stat sistemi, prop placement, Loc, VFX helper'lar...). Hangileri game-agnostic, hangileri RIMA-bespoke? Koda/memory'ye bakarak somut envanter cikar.
2. "Reusable environment" iddiasini sunumda KANITLANABILIR kilan minimum demo nedir? (or: ayni tooling'le 2. bir mini-sahne/oyun iskeleti gostermek?)
3. graphify HTML grafigi + audit report sunumda nasil somut kullanilir (mimari dogrulama, "baglantilari kontrol ediyorum" anlatisi)? Gimmick'e dusmeden gercek deger nasil gosterilir?
4. Bu cercevenin riskleri: over-promise (environment iddiasi tutmazsa), demo-dagilmasi.
5. NET: sunumda vurgulanacak 3-5 somut nokta + her biri icin kanit-artifact.

Kisa, gerekce-li, madde madde.
