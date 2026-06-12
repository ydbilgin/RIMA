# RIMA Tilemap Görsel Kalite — COUNCIL DECISION (2026-06-11)

> Council: cx (feasibility/reuse, laurethayday — analiz arka planda) + ax Gemini 3.1 Pro High (deep) + ax Gemini 3.5 Flash High (lean) → Opus sentez. Kaynak sorular: `STAGING/_process/2026-06/_council_*_tilemap-quality.md`.
> Tetik: kullanıcı referans projedeki zengin izometrik fantasy tilemap'leri (Asset Store paketi + Animated Tile su + Animator meşale + autotile) RIMA'ya uyarlamak istedi.

## TL;DR KARAR
**O güzelliğin sırrı izometri DEĞİL — ışık + yoğunluk + derinlik illüzyonu.** RIMA top-down-3/4 içinde kalarak oraya ulaşır. Sıra: **(1) URP 2D ışık + post-process atmosfer → (2) props/dekorasyonu live'a bağla (mevcut sistem, sıfır yeni art) → (3) animated meşale + ışık flicker + shadow-blob.** **Wang/autotile painter'a DOKUNMA** (demo öncesi bataklık). **ISO'ya GEÇME** ([LOCK-RİSK], demo'yu kırar).

## İKİ ADVISOR'IN OYBİRLİĞİ (3.1 Pro + 3.5 Flash)
| Konu | Verdict | Gerekçe |
|---|---|---|
| **ISO'ya geçiş** | ❌ HAYIR [LOCK-RİSK] | collision/pathfinding/sorting/hitbox + tüm PixelLab asset'leri kırılır; S59 ihlali; action-ARPG için top-down-3/4 zaten DOĞRU (Hades neden 3/4 → dövüş okunabilirliği) |
| **Dormant Wang/layered painter'ı bağla** | ❌ HAYIR (demo öncesi) | "edge-case cehennemi/bataklık" (3.5); "düşük-etki/yüksek-risk, en az fark edilen unsur prop'larla gizlenince" (3.1); kapı/sorting/delik bug riski → demo SONRASI |
| **#1 kaldıraç = URP 2D ışık + post-process** | ✅ EVET | global ışığı karart + point light + vignette/color-grading/bloom = "10 dk'da premium indie atmosferi"; düz pikseli kurtarır, gözü zemin eksikliğinden aksiyona çeker |
| **Props/dekorasyonu live'a bağla** | ✅ EVET | ~%80 hazır (BridsonPoissonAutoPlacer); grid hissini yok eder; en organik+ucuz "zemin zenginliği" (autotile'dan iyi) |
| **Animated meşale + flicker + shadow-blob** | ✅ EVET (yalın) | Animator 3-4 kare + `Mathf.PingPong`/noise ile 2D Light intensity titreşimi; sprite-tabanlı sahte elips gölge = ucuz+hatasız derinlik |
| **Animated su** | ⚠️ ERTELE | shoreline = autotile gerektirir → zaman tuzağı; gerekirse sadece erişilemez alanda basit scroll |

**Fark yaratan unsur sırası (3.1 Pro):** 1) 2D ışık/gölge 2) dekor-yoğunluğu 3) katman/yükseklik (Y-sort duvar, cliff) 4) palet-uyumu 5) animated detay 6) autotile (en az fark edilen — prop'larla gizlenir).

**Pixel Perfect uyarısı (3.1 Pro):** meşale direği statik + sadece alev oynamalı (titreme yok); 2D Light flicker'da Light Render Texture'ı PPU'ya göre ayarla. Meşale pivot = dibinde (Y-sort doğru). Perf: animated-tile batch'lenir (ucuz), Animator oda başına onlarca olursa CPU yükü.

## cx FEASIBILITY (laurethayday — TAMAMLANDI, Gemini'leri kanıtla doğruladı + 2 kritik ek)
- **Wang painter readiness:** resolver algoritmaları çoğu production-ready util + test'li (WangResolver 16-maske test, FeatureEdgeSmoothing test) AMA **live entegrasyon HAZIR DEĞİL** — IsoRoomBuilder hiçbirini çağırmıyor; `FloorWangResolver` TileBase değil asset-İSMİ döndürüyor (runtime lookup eksik). Bağlama = Option A (S/M, FloorWangResolver→BuildFloor, 40-90 LOC, ama "iso diamond naming" + sadece same-layer floor) / Option B (M/L, RoomTemplateAdapter→MapLayerOrchestrator bridge, 120-250 LOC, cliff double-paint riski). **Demo öncesi = medium-high risk → ERTELE.** (IsoRoomBuilder.cs:109-116,283-314; FloorWangResolver.cs:6-17,44; RoomTemplateAdapter.cs:18.)
- **🆕 KRİTİK CAVEAT-1:** RoomRunDirector **Combat/Elite'te template props'u KAPATIYOR** (spawn-softlock önlemi, RoomRunDirector.cs:315-318). → Demo-safe dekorasyon = template-props'u açmak DEĞİL; **ayrı, collider'sız, role/door/spawn-guard'lı, feature-flag'li dekorasyon-pass'i** olmalı. (Bu wiring'in kalbi.)
- **🆕 KRİTİK CAVEAT-2:** `IsoRoomBuilder.BuildProps` sadece worldSprite'tan SpriteRenderer kuruyor, **prefab instantiate ETMİYOR** (IsoRoomBuilder.cs:694-718) → animated meşale için prefab-desteği/özel animated-prop yolu gerek.
- **🆕 İYİ HABER:** Unity **2D Tilemap Extras 6.0.2 KURULU** (Animated Tile hazır) + **BrazierBreath.anim + .controller ZATEN VAR** (meşale animasyonu substratı mevcut, reuse edilebilir). RuleTile asset'leri de mevcut.
- cx önceliği = dekorasyon-pass #1 ROI (sistem built+tested), sonra atlas, floor-enrich, animated brazier, water overlay, env-parallax, Wang-bridge (demo sonrası). Gemini'lerle örtüşür; tek fark cx kod-reuse açısından dekorasyonu, Gemini görsel-bang açısından ışığı #1 koyuyor → **sentez: ışık en ucuz görsel kazanç (config), dekorasyon en yüksek reuse (sistem hazır); ikisi de demo-safe.**

## DEMO-SAFE ÖNCELİK SIRASI (her ikisinin sentezi)
1. **Işık + post-process atmosfer pass** (`_Arena`) — global dark + point light + Vignette/ColorGrading/Bloom. Yüksek etki / düşük risk / SIFIR yeni art. → **kod, en güvenli ilk adım.**
2. **Decoration late-pass wiring** — mevcut BridsonPoissonAutoPlacer + validator + spawner'ı IsoRoomBuilder'a bağla, **mevcut 17 Act1 prop** ile. Feature-flag default-OFF (demo'yu bozmaz). → kod.
3. **Animated meşale + ışık flicker + shadow-blob** — PixelLab `animate_object` ile meşale karesi (kullanıcı-gated) + TorchFlicker.cs + sprite shadow-blob. → kod + PixelLab.
4. **Cliff/kenar derinlik cilası** (uçurum diplerini karart) — orta etki/orta risk.
5. ~~Wang/autotile~~ + ~~animated su~~ + ~~ISO~~ → **DEMO SONRASI / YAPMA.**

## GÖREV DAĞILIMI
- Plan/karar = Opus (bu doc). Kod = **cx (yekta — weekly reset penceresi, sonra normal quota-aware)**. PixelLab meşale = kullanıcı-gated. Wiring/QC = Claude.
- Bu karar props/env-bg planının (`STAGING/PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md`) ÜSTÜNE oturur: o plan props+parallax'ı, bu karar ışık+animasyon+ "Wang/iso YAPMA" netliğini ekler.

## SAKIN YAPMA (deadline tuzakları)
- Dormant Wang painter'ı canlandırma. ISO'ya geçme. Asset-store paketi alma (PixelLab-only + iso paketler top-down'a uymaz). Animated su shoreline'ı için autotile yazma.
