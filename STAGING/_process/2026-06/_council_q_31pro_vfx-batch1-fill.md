# 64px VFX Batch 1 — 13 boş slot ne ile dolmalı? DEEP / reusable-library lens (Gemini 3.1 Pro High)

Sen laurethstudio için derin asset-kütüphane mimarisi danışmanısın. RIMA = 2D top-down pixel-art roguelite, Unity, PPU 64, 10 sınıf (Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer).

Gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
READ: STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md · STAGING/PIXELLAB_VFX_BATCH_LIMITS_2026-06-12.md · STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md · STAGING/MODULAR_ABILITY_DECISION_2026-06-12.md

## Durum
PixelLab `create_1_direction_object` size=64 → 16 slot/çağrı, item_descriptions[] ile distinct item, maliyet çağrı-başına sabit (slot bedava-değer). Batch 1'de 3 dolu (slash arc, ice spike, Elementalist bolt), **13 boş**. Kullanıcı 3 yön düşünüyor: bu 2 sınıfın diğer skill'leri / diğer 8 sınıf / her-oyunda-global reusable.

## Sorular (derin, mimari)
1. **Reusable 64px VFX taksonomisi:** laurethstudio-seviyesi "her ARPG'de lazım olan" küçük VFX kütüphanesi nasıl tasarlanır? Kategoriler (impact/slash arketipleri, status auraları burn/chill/shock/poison/stun, buff/debuff glyph, heal/level-up sparkle, pickup shine, projectile bolt'lar). Hangileri 64px sprite olarak değerli, hangileri runtime-tint'lenebilir tek base'den türer (renk taksonomisiyle)?
2. **13 slot tahsisi:** demo-değeri + RIMA-gelecek + global-reuse dengesiyle 13 slotu nasıl bölersin? Önceliklendir. Modüler ders (kullanılmayacağı üretme) ile global-kütüphane hevesi arasındaki gerilimi nasıl çözersin — "şimdi üret" eşiği ne?
3. **Style coherence:** tek batch'te 13 farklı item stilistik tutarlı kalır mı, yoksa tematik bölmeli mi (örn. "physical slash/impact" batch'i ayrı, "elemental bolt/aura" batch'i ayrı)? Renk taksonomisi (Physical ember, Fire/Frost/Lightning/Void) batch bölünmesini nasıl etkiler?
4. **Cross-class kaldıraç:** 10 sınıfın paylaştığı ortak VFX nedir (jenerik melee slash, ranged impact, status aura) — bunlar sınıf-spesifik mi yoksa tek paylaşılan set + tint mi? Sınıf kimliği (Ronin tension, Hexer void) nerede özel VFX hak eder?

Somut item listesi + kategori + tint-edilebilir-base ayrımı ver. Türkçe serbest.
