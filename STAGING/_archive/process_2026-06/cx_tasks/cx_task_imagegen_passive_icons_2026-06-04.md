ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

# Amaç
$imagegen ile RIMA draft'ındaki PASİF/relic kartlar için tutarlı, on-brand bir SKILL-IKON PACK üret. Şu an pasiflerin ikonu yok → kartta gri placeholder çıkıyor. 16 ikonluk cohesive bir sheet üret, sonra tek tek dilimlenip import edilecek.

# Stil (mevcut 19 ikona UYUMLU olmalı — referans: Assets/Sprites/UI/Icons/Icon_Warblade_GravityCleave.png vb.)
- Koyu/void zemin (#3A1A4A void-mor), merkezde beyaz/açık SİLUET motif, arkasında cyan #00FFCC rift enerji parıltısı, yer yer warm-orange #E89020 accent.
- Flat, grafik, KÜÇÜK boyutta okunaklı (HUD'da 28-34px, draft'ta ~100px). Pixel-art-adjacent, ON-BRAND (fotogerçekçi DEĞİL). Kare kompozisyon, kenarlarda hafif çerçeve boşluğu.

# Üretim
- 16 ikon, her biri **96×96 px**, TEK coherent grid sheet (4×4) olarak üret (palet/stil tutarlılığı için), sonra Unity'de dilimlenecek. Şeffaf/koyu zemin.
- Motifler (ARPG pasif temaları, çeşitli): kılıç-damgası, kalkan, yumruk, bot/hız, göz, kan-damlası, alev, çatlak-zemin, zincir, kafatası, kalp/can, şimşek, boynuz, sancak, kristal/gem, pençe.
- Çıktı: `STAGING/imagegen/passive_icons/` altına sheet PNG (+ varsa tek tek). Dosya adı `passive_icons_sheet_4x4_96.png`.

# NOT
- Bu görev SADECE üretim (PNG). Slice/import/wire orchestrator yapacak.
- Sonuç → CODEX_DONE.md: üretilen dosya yolları + sheet düzeni (hangi hücre hangi motif) + boyut.
