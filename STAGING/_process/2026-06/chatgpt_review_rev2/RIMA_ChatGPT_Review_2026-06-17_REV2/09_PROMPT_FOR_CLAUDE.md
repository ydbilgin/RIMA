# Claude'a verilecek uygulama prompt'u — REV2

Aşağıdaki metni doğrudan Claude Code / RIMA oturumuna ver.

---

RIMA için `RIMA_ChatGPT_Review_2026-06-17_REV2` paketini oku. Önce sırayla:

1. `REVISION_2_CHANGELOG.md`
2. `00_READ_FIRST.md`
3. `01_EXECUTIVE_DECISION.md`
4. `02_CAPTURE_QA.md`
5. `07_TWO_DAY_DEMO_PLAN.md`
6. ardından ilgili Director, Build UX ve Game UI dosyaları

## Kritik düzeltme: Build Mode'u yanlış yeniden tasarlama

Build Mode'daki eğik ve oda dışına uzanan diamond grid, RIMA'nın 128×64 isometric tile authoring çalışma düzlemidir. Gridin amacı yalnız mevcut room floor'u göstermek değil; floor/walkable/overlay katmanlarında mevcut şeklin dışına yeni tile çizebilmektir.

Bu yüzden:

- grid açısını/geometrisini koru,
- gridi yalnız room bounds içine kırpma,
- black workspace'i sırf oyun ekranına benzemiyor diye kaldırma,
- kare screen-space gride dönüştürme,
- Build Mode'u zorunlu olarak Director'ın yeni layout'una taşıma,
- mevcut 8/8 çalışan placement çekirdeğini refactor etme.

Build için `05_BUILD_MODE_UX_POLISH.md` tek uygulama referansıdır. Eski build redesign görseli paketten kaldırılmıştır.

## Ana görev

Çalışan sistemleri yeniden yazmadan görsel/UX presentation pass yap.

### Değişmez işlevsel kontratlar

- Build Mode assert 8/8 korunacak.
- Director Mode assert 6/6 korunacak.
- `TimeScale`, spawn, stat apply, telemetry CSV, working-copy isolation, undo/redo davranışları değişmeyecek.
- Source ScriptableObject asset doğrudan mutate edilmeyecek.
- Yeni özellik eklenmeyecek; mevcut özellikler ürün gibi sunulacak.

## P0 sıra

1. Screenshot capture doğruluğu
2. Boss scale/pivot/health bar/shop residue/subtitle
3. HUD okunabilirliği + low-HP vignette
4. Siyah blob sprite/obje okunabilirliği
5. Director shell redesign

## P1 Build Mode polish

Mevcut viewport/panel düzenini ve world-space diamond grid'i koru:

- grid `Low / Normal / High` görünürlük seviyesi,
- mevcut floor ile genişletilebilir dış alan arasında ton farkı,
- hover edilen diamond cell,
- prop footprint preview,
- valid green + `VALID`,
- invalid red + kısa reason,
- yeni yerleştirilen prop pulse/outline,
- asset thumbnail/name/footprint,
- aktif tool/layer/cell,
- source-safe ve undo count status bar.

Gridin floor şeklinin dışında görünmesi beklenen davranıştır.

## Director hedefi

`visuals/director_mode_proposed_layout.png` düzenini temel al:

- 56 px top bar
- 64 px left rail
- 280–320 px contextual library
- center viewport
- 320–360 px inspector
- 28–32 px status bar
- telemetry drawer kapalı varsayılan

Mevcut callback ve tab logic'ini koru. Shared component prefabs üret:

- `DirectorPanel`
- `DirectorButton`
- `DirectorCard`
- `DirectorInput`
- `DirectorStatusBar`

Bu Director shell kararı Build Mode'un isometric viewport geometrisini override etmez.

## HUD hedefi

`visuals/combat_hud_proposed_markup.png` referansını uygula.

- HP 200–220×14–16
- resource 150–170×8–10
- LMB/RMB 52–56
- Q/E/R/F 44–48
- cooldown number + key label
- ulti lock/armed state
- full-screen red overlay yok

## Capture düzeltmesi

Şu dosyalar tekrar üretilmeli:

- `09_director_stats_physPower_177.png`
- `19_character_sheet_open_warblade.png`
- `20_skill_draft_open_3_cards.png`
- `21_runmap_open_current_node_highlight.png`

Farklı state screenshot'ları aynı SHA-256 ise capture pipeline FAIL versin.

## Boss acceptance

- sprite tam görünür
- HUD overlap yok
- shop standları yok
- health bar neon green değil
- %66/%33 phase ticks var
- subtitle güvenli alanda

## Draft metni

`X ile eşleşir` gibi belirsiz metinleri kaldır. Exact trigger ve exact payoff yaz.

Örnek:
`Iron Charge sonrasında kullanılırsa çekilen hedefler 1.5 sn sersemler.`

## Çıktı

1. Değişen dosyaların listesi
2. Her P0 için before/after screenshot
3. Build ve Director assert sonuçları
4. Capture QA sonucu
5. Build Mode için gridin dış alana uzanmasının korunduğunu gösteren screenshot
6. Kalan riskler

Önce repo'da mevcut prefab/script yapısını tespit et; ardından minimum-risk implementation planı yaz ve uygula. Büyük refactor açma. Demo stabilitesini görsel hırstan üstün tut.

---
