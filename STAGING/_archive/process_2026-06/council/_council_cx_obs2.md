ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Canon-grounded obstacle/door/decor üretim spec'ini ÜRETİM-ÖNCESİ doğrula (feasibility/wiring/collider/import lens). ANALYSIS ONLY — kod değiştirme. Sonucu CODEX_DONE.md'ye yaz.

OKU: `STAGING/OBSTACLES_DOORS_DECISION_2026-06-03.md` (özellikle PART 2 — NLM CANON, AUTHORITATIVE). Önceki feasibility cevabın `CODEX_DONE.md`/`_council_cx_obstacles_doors.md` zaten prop-sistem reuse + door wiring + pack-vs-tekil'i kapsadı — TEKRARLAMA, sadece YENİ canon-spesifik soruları cevapla:

1. **Collider eşleşmesi:** PART 2 obstacle tablosundaki her obje için collider tipi (pillar→Capsule, wall-stub→Box, cage→Box, tombstone→Box, altar→Box, chain-anchor→Box, brazier→Circle) NLM keyword/physics tablosuyla (b0xikacj6 citation 29) + `PropDefinitionSO` blocking/footprint alanlarıyla tutarlı mı? Her obje için `PropDefinitionSO` doldurma satırı (footprint cell, blocking bool, collider shape) öner.
2. **Door modüler kompozisyon:** "nötr taş eşik (3 yön) + simetrik cyan rift + rün-ikon overlay" — `GateBehavior` (sprite alanları) + ayrı rün-ikon child SpriteRenderer ile runtime'da nasıl kurulur? Rift emissive katmanı ayrı SpriteRenderer mı olmalı (cyan)? flipX (Doğu=Batı) GateBehavior'da değil instance'ta — doğru mu?
3. **Chasm özel:** chasm bir PROP değil, walkable-mask deliği (dash-gap). PART 2'deki 192×128 decal + IsoRoomBuilder walkableGrid'inde hole olarak mı temsil edilmeli? Mevcut sistemde "DashTraverseGap" implemented mi yoksa design-contract mı (NLM citation 30 = not implemented)? → chasm'ı şimdilik decal-only mı yapalım?
4. **Import:** PART 2'deki px boyutları (64×112 pillar, 128×80 wall, ~160-200 door vb) PPU64'te doğru world-boyut veriyor mu? Her asset için pivot (bottom-center vs center) + PPU + import ayarı tek satır.
5. Tek-tek üretimde bu ~15 obje için pack-vs-tekil verdict'in DEĞİŞİR Mİ (sayı arttı)? Küçük tutarlı alt-pack'ler (door-set / decal-set / obstacle-set) mantıklı mı?

Kısa, path/satır-destekli. Üretim-engelleyici bir tutarsızlık varsa BLOCKED+flag.
