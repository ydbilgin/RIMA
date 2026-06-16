# COUNCIL — Elementalist anim seti + skill-VFX yaklaşımı + iki karakterin PixelLab promptları

ACTIVE RULES: (1) think (2) min — over-produce ETME (3) surgical — sorulan (4) BLOCKED if unclear.
NLM ACCESS: Elementalist skill kiti + RIMA VFX canon için NLM sorgula:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "Elementalist sinifinin skill kiti / yetenekleri nedir? element tipleri?"
  (+ "RIMA VFX boyut canon ve skill VFX felsefesi" gibi)
UNITY YOK: salt analiz/planlama.

## BAĞLAM
Bu, warblade animasyon kararının (oybirliği, `WARBLADE_ANIM_DEMO_DECISION_2026-06-15.md`) DEVAMI. Kullanıcı şimdi **Elementalist için de** animasyon + state + **güzel skill VFX** istiyor; PixelLab MCP ile state'leri orchestrator ÜRETECEK, **promptları kullanıcıya verecek**.
- **warblade kararı (extend et, redo etme):** P1=idle/koşma/LMB-vuruş; Q/E/R/F=REUSE+`SkillVfx.cs` engine-juice (bespoke YOK); state-first; 5 üret(S,SE,E,NE,N)+3 mirror(flipX); 120x120 high-top-down 10-12fps PPU64.
- **Karakterler (ikisi de `animations: none`):** warblade `2656075d-d113-4f18-a6c1-94b5a6b8bf65` · Elementalist `4c83c0be-e856-48f1-b8b5-9626e041a082`.
- **KAPSAM gerçeği:** Elementalist KİLİTLİ kararda **POST-DEMO** (demo tek-sınıf warblade). Bu iş = post-demo-prep; demo'yu/warblade-P1'i GECİKTİRMEMELİ. PixelLab budget 874 gen = bol; darboğaz = cleanup zamanı.
- **MCP araçları:** `create_character_state(char_id, edit_description)` → state=yeni grouped char (pose ilk-frame). `animate_character(char_id, action_description, mode=v3, frame_count, directions)` = 1 gen/yön. Template'ler: running-8-frames, fight-stance-idle-8-frames, **fireball**, cross-punch... `create_map_object`/`create_8_direction_object` = VFX projectile/impact/dekal (transparent).
- **RIMA VFX canon:** 64-128px mix (küçük 64-80, ult 96-128); PixelLab dekal HER ZAMAN transparent (alpha-doğrula); felsefe "Dead Cells tek-sprite + engine juice" (`SkillVfx.cs`: tint/additive/scale-fade).

## CEVAPLA
- **E1 — Elementalist anim seti:** Caster (warblade=melee). P1/P2/P3 sırala. Idle/koşma garanti; "attack" = temel cast/bolt mi? Hangi state'ler? (warblade-mantığı + caster farkı)
- **E2 — SKILL VFX yaklaşımı (asıl karar):** Elementalist elemental skilleri için VFX = **(A) engine-only** (SkillVfx tint/additive + Unity particle, reuse cast anim) · **(B) PixelLab-üretilmiş** (projectile/impact/AoE dekal sprite'ları + cast anim) · **(C) hybrid** (imza-element için PixelLab hero-VFX, gerisi engine). Hangisi? NEDEN? Eğer PixelLab üretilecekse: **HANGİ VFX asset'leri** (kaç adet, boyut, transparent), hangi araçla? Over-produce ETME (post-demo, sadece "güzel" hissini veren çekirdek).
- **E3 — PROMPT SETİ (cx LİDER, somut yaz):** İki karakter için PixelLab promptlarını DRAFT et:
  - **State promptları** (`create_character_state` edit_description): warblade {mid-run, strike-windup, breathing-idle, flinch} + Elementalist {mid-run, cast-charge/windup, breathing-idle, flinch}. Her prompt: karakter-kimliği koruyan, poz-odaklı, high-top-down, kısa-net.
  - **Animasyon promptları** (`animate_character` action_description, v3): her state'ten hangi anim (run loop / strike / idle breathing / cast / flinch), frame_count önerisi.
  - **VFX asset promptları** (E2'de PixelLab seçilirse): projectile/impact/AoE — açıklama + boyut + transparent + view.
- **E4 — Üretim sırası + budget:** Önce warblade-P1 (demo), sonra Elementalist (post-demo). Kaç state + kaç gen call? 5+3 mirror. Gerçekçilik.

## ÇIKTI (E1 ekonomi)
Dosyaya yaz: cx → `ANIM2_cx.md` (PROMPT SETİ burada, kopyala-yapıştır hazır) · ax_pro → `ANIM2_axpro.md` (E2 VFX + E1 anim-profili derinlik) · ax_flash → `ANIM2_axflash.md` (over-produce/scope lean check). Orchestrator'a dönüş ≤10 satır + yol.
