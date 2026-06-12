ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ANALYSIS ONLY — kod/asset üretme YOK. 64px PixelLab VFX batch'inde 13 boş slot var; bunlara FEASIBILITY/REUSE açısından en değerli 64px VFX sprite'larını öner.

## Bağlam
- VFX spec: `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md` + batch limits: `STAGING/PIXELLAB_VFX_BATCH_LIMITS_2026-06-12.md`.
- Tool: `create_1_direction_object`, KARE, size=64 → **16 slot/çağrı** (item_descriptions[] distinct item). Maliyet çağrı-başına sabit (~20-40 gen) → slot doldurmak bedava-değer.
- Batch 1'de 3 item kullanılıyor: slash arc (Warblade basic), ice spike (Glacial Spike), Elementalist mini bolt. **13 slot boş.**
- Kullanıcı 3 yön istiyor: (a) bu 2 sınıfın (Warblade/Elementalist) DİĞER skill'lerinin 64px VFX'leri, (b) diğer 8 sınıfın VFX'leri, (c) her oyunda kullanılabilecek GLOBAL reusable VFX.
- ÖNEMLİ DERS: `STAGING/MODULAR_ABILITY_DECISION_2026-06-12.md` — kullanılmayacak şeyi üretme (modüler over-production tuzağı bu session'da ampirik doğrulandı).

## Sorular (somut, dosya-referanslı)
1. **Mevcut reuse:** PixelLab'da 387 obje var (bazıları `rima_decal_v1` tag'li crack/scorch decal). RIMA repo'da hangi 64px VFX sprite'ları ZATEN var (Resources/VFX, Assets/Sprites/VFX, prefab'lar)? Yani 13 slotun kaçı zaten karşılanmış, yeni üretmeye gerek yok?
2. **Demo-relevant öncelik:** Warblade/Elementalist'in kit-dışı skill'leri (Assets/Scripts/Skills/Warblade/**, Elementalist/**) tara. Hangileri 64px küçük VFX sprite'ı gerektirir (slash/stab/bolt/burst tipi) ve demo draft havuzunda görünme ihtimali var? Hangileri particle/LineRenderer (kod) olmalı, slot harcamamalı?
3. **Style coherence kısıtı:** Tek `create_1_direction_object` çağrısında item_descriptions[] ile kaç FARKLI item gerçekçi kalite verir? (Weapon batch deneyiminden — `STAGING/WEAPON_BATCH_PLAN.md` / project_weapon_production_plan: bir çağrıda kaç distinct silah konuldu, kalite düştü mü?) Yani 13'ü tek batch'e tıkmak mı, tematik 2-3 batch'e bölmek mi?
4. **Global vs proje:** "Her oyunda kullanılabilir" generic 64px VFX (slash, impact star, hit spark, dust puff, heal sparkle, status aura) PixelLab sprite mi olmalı yoksa bunlar zaten particle/code işi mi (slot israfı)?

Çıktı: 13 slot için somut item listesi önerisi (üret / reuse-var / code-olmalı ayrımıyla) + style-batch bölme önerisi. CODEX_DONE.md'ye yaz. Belirsizlik → BLOCKED.
