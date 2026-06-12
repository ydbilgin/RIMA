ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA görsel MASTER PLAN'ı PLAN-REVIEW et (yeniden türetme YOK). FEASIBILITY/SEQUENCING merceği. ANALYSIS ONLY. Sonuç CODEX_DONE.md.

# OKU
- `STAGING/VISUAL_MASTER_PLAN_2026-06-11.md` (ana hedef)
- Destek: `STAGING/ROOM_DESIGN_DECISION_2026-06-11.md`, `STAGING/TILEMAP_VISUAL_QUALITY_DECISION_2026-06-11.md`
- Kod: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (RoomDecorationPass hook ~126), `Assets/Scripts/MapDesigner/Room/Runtime/RoomDecorationPass.cs`, `Assets/Scripts/MapDesigner/Composition/CompositionRoleMapGenerator.cs` (FocalCluster işaretlenmiyor), `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs` (density 0.65)

# SORULAR (review)
1. **FAZ SIRASI doğru mu?** Plan: FAZ0 asset → FAZ1 dekorasyon(AÇ+density0.3+FocalCluster) → FAZ2 ışık → FAZ3 QC. AMA council "ışık #1 görsel kazanç" dedi. **Işık FAZ1'den ÖNCE mi olmalı?** (Argüman: dekor yoğunluğunu düz-parlak ışıkta ayarlamak yanıltıcı; ışık önce gelirse dekoru doğru tonda görürüz.) Yoksa dekor-önce mi (kod yarı-hazır)? Bağımlılık analizi yap, NET öner.
2. **FocalCluster kod değişikliği doğru scope+güvenli mi?** Generator'a focal-zone işaretleme + pass'e focal yerleştirme — hangi dosyalar, kaç LOC, mevcut testleri kırar mı, feature-flag gerekir mi?
3. **Plan'da gizli risk/blocker var mı?** Bağımlılık eksiği, yanlış sıra, atlanmış adım? Hangi faz bölünmeli/birleşmeli?
4. **İLK cx GÖREVİ ne olmalı?** (kullanıcı: execute=cx laurethayday). Net, tek, demo-safe ilk görevi TANIMLA (dosyalar + kabul kriteri + flag durumu). 
5. Over-engineering: plandan demo için KESİLECEK bir şey var mı?

ÇIKTI: file:line kanıtlı, NET sıralama önerisi + ilk-görev tanımı. [LOCK-RİSK] işaretle. CODEX_DONE.md.
