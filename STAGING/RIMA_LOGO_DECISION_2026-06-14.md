# RIMA LOGO DECISION — 2026-06-14

**Karar mercii:** Opus (orchestrator) sentezi. **Advisor'lar:** bağımsız Opus sub-agent (vision) + cx/Codex (marka-strateji) + ax Gemini 3.1 Pro (vision). Bitirme sunumu ~20 Haz.

## ⭐ FINAL (2026-06-14 akşam — çapraz-rift turu)
Kullanıcı "rift RIMA yazısını **çapraz** yarıp geçsin" yönü verdi → agy/Imagen ile 5 çapraz-rift varyant üretildi (`STAGING/imagegen/rima_logos_diagonal_20260614/`). **3 bağımsız okuma aynı yere çıktı** (kullanıcı yön + Opus orchestrator + ax_pro **KÖR/yönlendirmesiz** vision review):
- **Ana wordmark = `diag_05`** — keskin tek diagonal kesik + offset harf-kaydırma; ax_pro: *"5 aday içinde tek gerçek-grafik-tasarımcı işi"*, vektöre/transparana en uygun.
- **Kare ikon = `diag_02`** — en kalın/bloklu, küçük boyutta okunur.
- **Hero / kapak = `diag_03`** — keep silüeti + "A"da amber meşale, derinlik.
- **Promote edildi** → `TASARIM/UI_CONCEPTS/BRANDING/`: `rima_wordmark_rift_2026-06-14.png` · `rima_icon_rift_2026-06-14.png` · `rima_hero_keyart_2026-06-14.png`.
- **Elenen:** diag_01 + diag_04 (ax_pro: AI-slop, bozuk harf, vektöre uygun değil).
- Oyun-içi UI canonical wordmark (`logo_rima_glyph.png`) hâlâ yerinde; diag_05 transparan dekupe edilirse oyun-içine de geçebilir = **ayrı iş**.
- Not: tüm diag çıktıları 1024² **opak** (Imagen limiti); transparan kullanım için bg-removal gerek.

---
## TL;DR (ilk tur — 2026-06-14 öğlen, ÇAPRAZ TURLA SUPERSEDED ↑)
Bu bir "tek logo seç" değil, **LOGO SİSTEMİ** kararı. Mevcut canonical wordmark KORUNUR; yeni adaylardan sadece **logo_05** (ikon) ve **logo_10** (hero) sisteme eklenir. KAYNAK amber-lav yönü elenir. (Bu tur ardından kullanıcı çapraz-rift yönü istedi → yukarıdaki FINAL geçerli.)

## Kullanım dağıtımı (KİLİTLİ)
| Kullanım | Dosya | Mutabakat |
|---|---|---|
| **Ana wordmark / title-screen / oyun-içi UI** | `TASARIM/UI_CONCEPTS/BRANDING/rima_logo_final_transparent_2026-05-05.png` (+ oyun-içi `Assets/Resources/UI/RIMA/logo_rima_glyph.png`) — DEĞİŞMEZ | OYBİRLİĞİ (Opus+cx+ax_pro) |
| **Kare app/store/slide ikon** | `STAGING/imagegen/rima_logos_20260613/logo_05.png` (R-mark, cyan-rift) | OYBİRLİĞİ — üçü de #1 |
| **Sunum kapak / hero key-art** | `STAGING/imagegen/rima_logos_20260613/logo_10.png` (RIMA + yıkık kule + rift) | 2/3 (Opus+cx). Alternatif aşağıda. |

## Anlaşmazlık + karar: sunum hero
- **Opus + cx → logo_10**: RIMA baked-in, drop-in standalone hero; thesis opener için ideal.
- **ax_pro → logo_08** (kırık rünik halka): ortadaki void = canonical wordmark'ı merkeze oturtmak için mükemmel negatif alan (kompozisyon argümanı, tek başına değil).
- **KARAR = logo_10 birincil** (drop-in, RIMA içinde, düşük efor). **logo_08 = yedek "wordmark-in-void" çerçeve** — bölüm-ayraç slide'ları için canonical wordmark'ı halkanın ortasına yerleştirme opsiyonu (compositing gerektirir, opsiyonel).
- Not: cover full-bleed olduğu için ax_pro'nun "logo_10 kalabalık" itirazı (küçük-boyut kaygısı) kapak bağlamında geçerli değil.

## Elenenler
- **KAYNAK** (`CONCEPT_ART/rima_logo_kaynak.png`, amber-lav): OYBİRLİĞİ ile ELE. Amber-dominant Act1 canon'unu bozar (canon: cyan #00FFCC odak + minik amber accent), jenerik Diablo/forge okur. Amber sadece accent kalır. Sunumda cyan-rift = daha profesyonel/modern + "rift = editor/portal/sistem enerjisi" tooling metaforu.
- **Aday red:** logo_03 (rune-slop, prop hissi), logo_04 (logo değil, environment concept-art), logo_06 (parçalanan "A" = AI-slop + küçük-boyut çamur + "vivid vulnerability tek-kıvılcım" tonu ihlali), logo_07 (mobil castle-builder slop, vizyon-dışı).
- **Belirsiz/orta:** logo_02 (güçlü ikon yedeği — ax_pro #2), logo_08 (hero-çerçeve yedeği), logo_09 (temiz ama jenerik shard), logo_01 (rift "I" rastgele kırılmış hissi → canonical'ı tahttan edemiyor).

## Regen kararı: HAYIR (şimdilik)
Üç advisor + Opus: yeni 10 KONSEPT gereksiz; mevcut set 3 ana use-case'i karşılıyor. ax_pro kapanışı: "kilitle ve üretime geç, scope-drift riski alma."
- **Opsiyonel post-golden-path boşluk** (BLOKLAYICI DEĞİL): (a) canonical wordmark'ın sunum-güvenli **yatay flat varyantı** (cx — koyu+açık slide zemininde okunur, düşük parlama); (b) Edit-to-Play videosu için **16:9 hero banner** crop (Opus). İkisi de yeni konsept değil, mevcut varlığın production türevi. **Yalnızca golden-path bittikten sonra, zaman kalırsa**, hedefli ~4-6'lık tek agy_image run ile.

## Sonraki somut adım (kullanıcı onayına bağlı)
Seçilen 3'ü kalıcı isimle `TASARIM/UI_CONCEPTS/BRANDING/`'e promote et: `rima_icon_R_2026-06-14.png` (logo_05), `rima_hero_keyart_2026-06-14.png` (logo_10). Canonical wordmark zaten yerinde. Title-screen / sunum kapağına bağlama = ayrı iş.
