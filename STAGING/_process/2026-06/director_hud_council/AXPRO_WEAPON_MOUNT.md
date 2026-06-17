# ax_pro VISION — Warblade greatsword mount tuning (2026-06-18)

SALT-OKUNUR. Kod/git/Unity'ye DOKUNMA. Sadece görsel-inceleme + somut değer öner. Türkçe çıktı.

## AÇ VE İNCELE (görseller, proje köküne göre):
- `STAGING/_process/2026-06/director_hud_council/weapon_SE_closeup.png` — Warblade idle, SE facing, greatsword mount. ANA görsel.
- `STAGING/_process/2026-06/director_hud_council/weapon_SE_before.png` — geniş plan (aynı an).

## Sorun
Warblade (top-down chibi, weaponless-baked sprite) — greatsword runtime'da el-anchor'a mount ediliyor. Kullanıcı: "silah karakterin önünde/yanında saçma duruyor, sağ elinde + dinlenir gibi olmalı, idle'da thrust gibi fırlamamalı." Closeup'ta kılıç sağ-alta doğru body'den kopuk taşıyor, kabza bileğe oturmuyor.

## Mount sistemi (mevcut değerler — `OrientationSync.cs` + `WeaponDatabase.asset`)
FacingDir8 sırası: **S(0), SE(1), E(2), NE(3), N(4), NW(5), W(6), SW(7)**. Şu an SE(1) görünüyor.
- `anchorOffset` (silahın el-yüksekliği, tüm yönler ortak) = **(0.02, 0.33)**
- `handOffsets[8]` (yön başına el pos): S(0,-0.08) SE(0.08,-0.04) E(0.10,0) NE(0.07,0.05) N(0,0.08) NW(-0.07,0.05) W(-0.10,0) SW(-0.08,-0.04)
- `weaponRotations[8]` (yön başına derece): S=-90 SE=-45 E=0 NE=45 N=90 NW=135 W=180 SW=-135
- flipY: W/NW/SW'de true. Sort: N/NE/NW'de body arkasında (+behind), diğerleri önde.
- Greatsword sprite default oryantasyonu: kabza alt, namlu yukarı (0°'de dik yukarı varsay).

## Sorular (somut)
1. Idle greatsword bu top-down 3/4 chibi'de **nasıl durmalı**? (yanda aşağı dinlenir / omuzda / hafif çapraz?) SE için **somut anchorOffset + weaponRotations[SE] + handOffsets[SE]** öner.
2. Kabza karakterin **sağ eline** oturması için offset yönü ne olmalı (closeup'ta kopuk)?
3. 8 yönün her biri için **rotation + offset eğilimi** (tablo): hangi yönde silah body önünde/arkasında, hangi yönde flip. Mevcut değerleri eleştir, düzeltme öner.
4. anchorOffset.y=0.33 çok yüksek/alçak mı (kabza el yerine omuzda/belde mi)?

## Çıktı: `STAGING/_process/2026-06/director_hud_council/AXPRO_WEAPON_VERDICT.md` (≤40 satır, somut sayılarla). Dönüşte ≤8 satır özet + yol.
