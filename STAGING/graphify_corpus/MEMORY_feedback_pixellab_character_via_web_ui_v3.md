---
name: pixellab-character-via-web-ui-v3
description: "Karakter üretimi web UI V3'te manuel — MCP create_character pro/standard DISPATCH ETME."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# Character production stays on PixelLab web UI V3 — DO NOT MCP-dispatch

**Rule:** Karakter sprite üretimi için MCP `create_character` (standard veya pro mode) **dispatch etme**. User karakter işlerini PixelLab web UI'da Custom Animation V3 + Custom Frames akışıyla **manuel** yapıyor; Claude'un rolü sadece **prompt formülünü sağlamak**.

**Why:** User S86 oturumunda (2026-05-16) açıkça belirtti — "character pro'yu kullanma ben karakter içinde senin promptlarınla v3 ile yaparım". Web UI'da V3'ün sunduğu kontroller (Keep First Frame, Custom Frames A→B interpolation, 220×220 resolution, Enhance Action with AI) MCP'de expose edilmemiş — kalite kontrolü için manuel akış gerekli. Ayrıca pro mode 20-40 generation/dir maliyetli; user kendi seçim akışında daha tasarruflu.

**How to apply:**
- Karakter sprite/animation üretimi gerektiğinde → **prompt yaz**, user'a ver, web UI'da çalıştırmasını bekle
- `mcp__pixellab__create_character`, `mcp__pixellab__create_character_state`, `mcp__pixellab__animate_character` → karakter context'inde KULLANMA
- Object/decal/tile context'inde (L3-L6, map dekorasyon) MCP create_object serbest — bu kural sadece karakter işleri için
- Cross-class skill ghost VFX gibi karakter-türevli animasyonlar da web UI üzerinden
- Exception: user explicit "MCP'den dispatch et" derse → onay anına özel, default değil

**S86 UPDATE 2026-05-16 (Karar #145):** Character States özelliği Web UI V3'e eklendi. Karakter üretim akışı artık:
1. Create Character (V3 base) — eski adım korunur
2. **Create State × N** — Web UI'da pose/variant state'leri (idle direction, action anchor, outfit variant)
3. Custom Animation V3 — her state'ten first-frame-locked anim (Keep First Frame **ON** Karar #145 hard rule)
4. Web UI Mirror Horizontal — SW/W/NW (Unity flipX değil)
5. Pixelorama cleanup — manuel face/mouth/artifact fix

State özelliği MCP'de henüz expose edilmedi → Web UI manuel akış zorunlu, mevcut LOCK güçlenir. Detay: [[pixellab-character-states-workflow]] (RIMA local) ve [[pixellab-character-states-animation-workflow]] (Lauret Studio global).

# See Also
- [[pixellab-tool-inventory]] — endpoint kullanım matrisi
- [[pixellab-v3-ui]] — web UI V3 spec'leri (Custom Frames, Keep First Frame, vb.)
- [[pixellab-create-character-workflow]] — Ref Standard + output size + High Top-Down LOCK
- [[pixellab-character-states-workflow]] — Karar #145 state-first workflow (RIMA local)
- [[pixellab-character-states-animation-workflow]] — Lauret Studio global canonical workflow
