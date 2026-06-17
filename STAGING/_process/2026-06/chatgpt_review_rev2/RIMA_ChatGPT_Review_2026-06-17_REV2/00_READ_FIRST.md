# RIMA Screens v2 — ChatGPT Review Package REV2

**Tarih:** 2026-06-17  
**Amaç:** Claude'un hazırladığı ekran paketi, runtime assert'leri ve RIMA görsel canon'u üzerinden uygulanabilir demo kararı vermek.

> **REV2 düzeltmesi:** Build Mode'daki eğik, geniş ve oda dışına devam eden grid; RIMA'nın isometric tile authoring çalışma düzlemidir. Bu yapı redesign edilmeyecek. Ayrıntı: `REVISION_2_CHANGELOG.md` ve `05_BUILD_MODE_UX_POLISH.md`.

## Okuma sırası

1. `REVISION_2_CHANGELOG.md` — önceki Build Mode yorumunda ne düzeltildi
2. `01_EXECUTIVE_DECISION.md` — net karar ve Claude'a katıldığım/itiraz ettiğim noktalar
3. `02_CAPTURE_QA.md` — pakette gerçekten kanıtlanmayan ekranlar
4. `03_SCREEN_BY_SCREEN_REVIEW.md` — ekran bazlı sorun / çözüm / öncelik
5. `04_DIRECTOR_MODE_REDESIGN.md` — en kritik yapısal yeniden düzenleme
6. `05_BUILD_MODE_UX_POLISH.md` — mevcut isometric authoring yapısını koruyan polish pass
7. `06_GAME_UI_REDESIGN.md` — HUD, draft, Codex, boss, merchant, menüler
8. `07_TWO_DAY_DEMO_PLAN.md` — demo öncelikleri
9. `08_POST_DEMO_BACKLOG.md` — demo sonrasına ertelenecekler
10. `09_PROMPT_FOR_CLAUDE.md` — doğrudan uygulanabilir görev prompt'u

## Görsel ekler

- `visuals/director_mode_proposed_layout.png`
- `visuals/combat_hud_proposed_markup.png`
- `visuals/capture_qa_failures.png`

Build Mode için yeni layout görseli kasıtlı olarak yoktur. Mevcut viewport ve isometric grid geometrisi korunmalıdır; karar metinsel UX kurallarıyla tanımlanmıştır.

## Tek cümlelik karar

**Sistemleri yeniden yazma. Director'ın sunum kabuğunu düzelt; Build Mode'un isometric çalışma düzlemini koruyup yalnız okunabilirlik ve placement feedback'i iyileştir; HUD, boss sunumu ve capture doğruluğunu toparla.**

Assert'ler Build Mode'un 8/8 ve Director Mode'un 6/6 çalıştığını gösteriyor. Sorun mühendislik çekirdeği değil; ürün hiyerarşisi, okunabilirlik, ölçek ve bazı placeholder'ların hâlâ finalmiş gibi sahnede durması.
