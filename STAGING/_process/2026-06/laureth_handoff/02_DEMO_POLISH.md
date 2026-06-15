# RIMA — Demo Polish / "Juice" Opsiyonları (2026-06-15)

> **Mekanik DEĞİL** — demoyu güzelleştiren his/görsel dokunuşlar. **Emir değil opsiyon; ne kullanacağın senin.** Hepsi düşük-efor + FPS-güvenli olacak şekilde seçildi.
> Studio canon: 2D-illusion library Top-10 cross-game pattern (sub-pixel snap + dithered shadow + screen shake + parallax = vertical-slice min-4).

## ⚠️ FPS UYARISI (juice = "1 FPS" felaket bölgesi)
Spawn-ağır juice (damage#, glyph, ghost, particle, sparkle) = **HEPSİ pool + hard-cap + zero-alloc.** UI = Canvas-rebuild fırtınası yok. Işık = shader/quad, per-frame dinamik 2D-light yok. Build'de profille. *Juice 1 FPS'e mal olursa jüri-etkisi NEGATİF olur.*

## TIER-1 — baseline cila (birkaç saat, "profesyonel" his)
| # | Dokunuş | Ne yapar | Perf |
|---|---|---|---|
| 1 | Sub-pixel snap + pixel-perfect | mixel/titreme yok, "temiz pixel" | nötr |
| 2 | Dithered drop shadow (her aktör) | "yere basıyor" + derinlik | tek decal/aktör, pool |
| 3 | Screen shake = yön + mesafe-decay | vuruş yönünde, uzak düşman az sallar | kamera-offset, alloc yok |
| 4 | Hit-stop kademesi | normal 0.05s / crit 0.10s / **Posture-break 0.15s+1kare freeze** | time-scale |
| 5 | Damage# pop + coalesce | world-space mesh-TMP, ardışık vuruş tek büyüyen sayı | **pool+cap ŞART** |
| 6 | Low-opacity vignette (%10-15) | odak merkeze, screenshot punch | tek quad shader |
| 7 | Squash & stretch (dash/land/hit-react) | chibi'ye elastik his | scale-anim |

## TIER-2 — demo "vay" anları (yarım gün civarı her biri)
| # | Dokunuş | Ne yapar |
|---|---|---|
| 8 | Posture-crack telegraph + **"SHATTERED!" glyph** | kırılma anı sunum-altını (A2 mekaniğinin görsel katmanı) |
| 9 | Gri-can rally animasyonu | "ölüyordum geri döndüm" görünür (A3'ün görsel katmanı) |
| 10 | Flowstep cyan ghost-trail | "dans gibi" akış hissi (A1'in görsel katmanı) |
| 11 | Rift Portal geçiş juice | pseudo-zoom + cyan parçacık-çekim + beyaz fade |
| 12 | Map Fragment pickup | sparkle-pulse + manyetik-çekim + ekran-flash + counter punch |
| 13 | Skill Draft kart-reveal | stagger flip + rarity-glow + seçimde hit-stop |
| 14 | Boss intro framing | kısa letterbox + isim-kartı + 1 kamera-pan |

## TIER-3 — atmosfer (vakit kalırsa)
Foreground DoF (kenar bulanık katman) · ortam parçacık (toz/cyan ember) · palette shimmer (fracture cyan döngü) · **audio juice** (hit-thud, cam-kırılma SFX/SHATTERED, akış-drone, pickup-chime, draft-whoosh — game-feel'in ~%50'si ses).

## 💭 GÖRÜŞÜM (bağlayıcı değil)
**#8/#9/#10 ayrı iş DEĞİL** — A2/A5/A1 mekaniklerini kodlarken onların görsel katmanı olarak birlikte yaparsan efor paylaşılır. TIER-1'in tamamı birkaç saatte demoyu belirgin "cilalı" yapar. Gerisi senin vaktine + kod durumuna kalmış.
