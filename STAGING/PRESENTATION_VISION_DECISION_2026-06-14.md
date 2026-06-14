# KARAR — Bitirme Sunum Vizyonu (RIMA = environment + vertical slice) · 2026-06-14

**Yöntem:** 3-advisor council (cx + ax 3.1 Pro + ax 3.5 Flash) + full-codebase graphify AST haritası verisi. Ham: `_process/2026-06/_council_*_vision-env.md` + `.cx_dispatch/CODEX_DONE__council_cx_vision-env_*` + `graphify_fullmap/graphify-out/`.

## TEZ (kilit): "Oyun değil, environment + ilk vertical slice"
RIMA = oynanabilir 2D ARPG demosu DEĞİL sadece; **oda authoring + runtime build + run-graph + live edit + director/debug + stat/VFX routing** üzerine kurulu, tekrar-kullanılabilir bir **oyun-geliştirme environment'ı ve araç zinciri** prototipi — ve onun ilk dikey kesiti.

**Çerçeve disiplini (anlaşmazlık çözümü):** ax Pro "framework" / ax Flash "framework deme, toolset de" çatışmasında **cx'in orta yolu kazandı:**
- ✅ DE: "RIMA için geliştirdiğim, top-down oda-tabanlı ARPG/roguelite'a **özel** tekrar-kullanılabilir geliştirme environment'ı + framework-benzeri modüller, canlı artifact'larla kanıtlı."
- ❌ DEME: "Bitmiş genel-amaçlı framework / Unity'ye rakip engine."
- Sebep: domain-specific + kanıtlanabilir = inandırıcı; genel-engine = over-promise (juri "API'n hani, paket'in hani?" altında ezer).

## 🎯 EN GÜÇLÜ KANIT (graf verisi — VERİYLE)
Full-codebase god-node analizi: en çok bağlantılı 10 soyutlamadan **6'sı authoring/editor aracı** (DirectorMode-168, InPlayMapPaintOverlay-93, RoomPainterWindow-88, LargeDungeonMapPainterBase-78, MinimalTilePainter-70, BuildPlacementController-66). → Kod tabanının ağırlık merkezi oyun-mantığı değil **tooling**. "Environment" tezini ampirik kanıtlıyor. Bu slayt = savunma değil saldırı pozisyonu.

## Reusable envanter (cx, kod-kanıtlı)
- **STRONG:** data-driven oda çekirdeği (RoomTemplateSO→RoomBankSO→IsoRoomBuilder, 34 room asset) · in-game build/edit (BuildModeController/BuildPlacementController/BuildTileBrushController/LiveRoomReloader, undo/redo) · prop placement (BridsonPoisson+CompositionRole).
- **MEDIUM-STRONG:** run-graph/director (DungeonGraph/RoomRunDirector/RunMapOverlay, `M` overlay).
- **MEDIUM:** DirectorMode sandbox · stat/damage (ClassStatProfile/Runtime/DamageCalculator) · VFX event-bus (CombatEventBus/VFXRouter).
- **LIGHT:** Loc (TR/EN). [Bunlar RIMA-bespoke yerleri DÜRÜSTÇE kabul et: class/skill/reward coupling.]

## Sunum ekseni (ax Pro arc + ax Flash centerpiece + cx kanıt + graf)
**Denge:** ~%20 oyun / %60 environment-mimari / %20 graphify-audit (ax Pro). Eksen = "Mimari Vaka Analizi", oyun tanıtımı değil.

Akış:
1. **Hook:** 15 sn cilalı oynanış videosu.
2. **Paradigma:** "Bugün bu oyunu değil, bunu yapmak için icat etmek zorunda kaldığım makineyi anlatacağım."
3. **Problem:** hardcode oyun = içerik-üretim darboğazı.
4. **Çözüm (centerpiece):** **Edit-to-Play timelapse videosu** (ax Flash en-yüksek-getiri) — F2 Build Mode'da oda çiz/prop koy/undo → Play → çizdiğin odada dövüş. + cx kanıt-zinciri slaytı: `RoomTemplateSO → RoomBankSO → IsoRoomBuilder → RoomRunDirector → RunMapOverlay`.
5. **Kalite güvencesi:** graphify — "bu kadar esnek sistemi nasıl çökmeden tutuyorum?" → god-node grafiği + GRAPH_REPORT (hub/gap). **1 ekran görüntüsü + rapor bulguları**, canlı gezinme YOK, 2-3 node önceden seç.
6. **Kapanış:** "Bugün RIMA'yı teslim ediyorum; yarın bu altyapıyla yeni oyunlar yapabilirim."

## Riskler + hafifletme (cx + advisor uzlaşı)
- Over-promise → domain-specific wording (yukarıda).
- "Bitmemiş oyun" algısı → baştan "%X tooling / %Y vertical slice tech-demo" diye kapsamı çiz.
- Demo-dağılması → canlı kısım TEK sıkı sekans; F2+combat+graphify+2.sahne hepsini canlı yapma (video kullan).
- Bug bağımlılığı → environment-demosu reward-leak/draft/tooltip bug'larına BAĞLI olmasın (önce fixle ya da video).
- graphify küçük-korpus/izole-node itirafı → zayıflık değil "audit dürüstlüğü" diye sun.

## Aksiyon
- [ ] Sunum-görseli: 6925-node hairball render olmaz → **filtrelenmiş graphify viz** üret (god-node + tooling community'leri, ya da community-meta-graf). [öneri: orchestrator üretir]
- [ ] **Edit-to-Play timelapse videosu** çek (en yüksek getirili tek iş) — KULLANICI.
- [ ] Demo-polish bug'ları (F1/F2/UI) — environment-demosunu bunlara bağlamadan fixle.
- [ ] Kanıt-slaytları: cx'in 5 vurgu + artifact listesi.
