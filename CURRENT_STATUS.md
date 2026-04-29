# CURRENT STATUS
**2026-04-29 · S43 · Faz 1** · Pipeline → `_STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`
⚠️ Graphify son: 2026-04-22 — S43 batch sonrası güncelle.

## Anchor

| Class | Anchor | char_id | Rot | Anim |
|---|---|---|---|---|
| Warblade | ✅ | ⏳ | ⏳ | ⏳ |
| Shadowblade | ✅ | ⏳ | ⏳ | ⏳ |
| Ranger | ✅ edit⏳ | ⏳ | ⏳ | ⏳ |
| Ronin | ✅ edit⏳ | ⏳ | ⏳ | ⏳ |
| Gunslinger | ✅ edit⏳ | ⏳ | ⏳ | ⏳ |
| Brawler | ✅ 2026-04-29 | ⏳ | ⏳ | ⏳ |
| Ravager | ✅ PASS 2026-04-29 · sol üst variant · PNG save bekliyor | ⏳ | ⏳ | ⏳ |
| Elementalist | ⚠️ regen (drift) | `9fb46502` | ✅ offset | ⏳ |
| Hexer | ⏳ regen | — | — | — |
| Summoner | ⏳ regen | — | — | — |

Style refs: Warblade · Shadowblade · Ronin · Ravager · Gunslinger · Ranger · Brawler
Elementalist = style anchor DEĞİL (drift var)

## Sıra (BURADA → PixelLab üretim)
- Edit Image: Ranger · Ronin · Gunslinger
- Regen: Elementalist · Hexer · Summoner
- Create Character → char_id (tüm PASS sonrası)
- Animate: Elementalist + Warblade (MCP)
- Unity: PPU=128 refactor · death screen · skill draft UI · Faz 1 loop testi (Codex)
- Doc: STYLE_BIBLE accent sync · AGENTS revize (rima-doc)

## Açık Sorular (2026-04-29 — yeni session'da konuş)

**"Delete-the-book" edit karakteri:**
- Dosya: `pixellab-Delete-the-book-behind-the-cha-1777421830646 (1).png`
- QC sonucu: Stil PASS (Gemma + Claude). Outline/shading/proporsiyon anchors ile tutarlı. Minor: mavi tunik biraz fazla saturated, weathered doku az.
- İfade: Sinirli değil — kararlı/focused. Fractured Epic için uygun.
- **❓ Soru 1:** Bu hangi class için üretildi? (Orb tutuyor → Elementalist sanılıyor ama outfit farklı: tunik+etek vs mevcut anchor'ın crop top+pantolon)
- **❓ Soru 2:** Eğer Elementalist regen ise → mevcut Elementalist anchor'ı (contact sheet'teki) replace edecek mi? Outfit kimliği değişiyor.
- **❓ Soru 3:** Onaylanırsa `_STAGING/anchors/elementalist/elementalist_anchor.png` güncellenir ve contact sheet regen edilir.

## Beklemede
- Edit Image: Ranger (bow crack) · Ronin (scabbard crack) · Gunslinger (barrel crack)
- Ravager anchor QC: PASS; sol üst variant seçildi. Gerçek PNG workspace'e kaydedilince `_STAGING/anchors/ravager/ravager_anchor.png` güncellenecek.
- Unity Batch 01-07 QC — Claude review pending
- RageSystem inspector: ragePerHitDealt=2, ragePerKill=5, decayDelay=1.5, decayPerSecond=10 ⚠️ verify önce
- FAZ_MASTER #17-#52 sync (Faz 2 başında)
- Memory compact pass S40 (35 dosya)

## Sonraki Temizlik (S43 sprite batch bittikten sonra)
- /lint çalıştır → tutarsızlık + stale entry tara
- Genel temizlik: _STAGING düzeni, arşiv taşıması, orphan dosyalar
- Daha temiz workflow önlemleri: belirsiz prompt → önce uyar, yeni dosya açma disiplini

## Ref
Sahne: `Assets/Scenes/_IsoGame.unity` · Sistemler: SYSTEM_MAP.md · Kararlar: MASTER_KARAR_BELGESI.md
