ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Sodaman (Steam bullet-heaven roguelite) inceleme: RIMA için NE ALINABİLİR — özellikle SKILL-SEÇİM HOVER/feedback UX + 2 bekleyen özellik (run-içi sol skill paneli toggle, ESC codex skill ekranı). FEASIBILITY / REUSE lens: RIMA'da ZATEN ne var, neyi reuse ederiz, en az kodla nasıl bağlanır.

ANALYSIS ONLY — no code changes. Sonucu CODEX_DONE.md'ye yaz. Önceki herhangi bir audit'i TEKRARLAMA.

---

## SODAMAN — FACT SHEET (Steam + incelemelerden, ground-truth)
- Tür: cyberpunk **bullet-heaven roguelite**, top-down, pixel 2D. Dev: Tape Corps (4 kişi), Pub: GameDev.ist. ~%78 olumlu / 630 inceleme. ~4-8 saat içerik.
- Core loop: süper-asker alien sürülerine karşı, prosedürel gezegenler, "soda-powered" build'ler.
- **İMZA DRAFT MEKANİĞİ:** level-up'ta power-up'ı **3 SODA KUTUSUNDAN** seçersin (tematik kap) — tür-standardı "3 kart"ın yerine. **7 soda RENGİ**, her rengin **10+ skill'i** var. Renkleri karıştırmak "soda kokteyli" = sinerji efektleri yaratır ("mix, shake, add a dash of fury").
- Meta/deck: haritaya saçılı **40+ enhancement kartı** topla → deck kur. **6 vücut parçası + 30+ sibernetik augment** (blueprint'ler boss/sandıktan). **8 silah**, her biri farklı oyun-tarzı, sinerji keşfi.
- Run-arası **uzay-gemisi HUB** (dinlen + customize/meta).
- 91 achievement, score attack.

## RIMA — MEVCUT DURUM (skill-seçim ile ilgili)
2D iso roguelite ARPG, 10 sınıf, Hades/CoM 3/4 top-down, URP 2D + Pixel Perfect 640×360, PPU64. Canon = "Vivid Vulnerability / UI yoktur sadece bilgi vardır" (opak kutu YASAK, ink-on-paper, renk=anlam). Currency = "Echo" (isim emin değil).

## SUB-QUESTIONS (feasibility/reuse açısından cevapla)
1. **MEVCUT SKILL-SEÇİM KODU ENVANTERİ:** RIMA'da skill seçimi/hover ile ilgili NE VAR? Grep/oku ve raporla (varsa dosya+satır):
   - `DraftManager` + `SkillOfferUI` (3-kart reward draft; HoverScale child VisualRoot + sabit raycast hitbox + select-flash + rarity_glow olduğu söyleniyor — DOĞRULA).
   - `RimaUITheme` (merkezi tema; `card_frame_9slice`, `rarity_glow_*`, `PassiveIcon(name)`, ClassIdentity, accent renkleri).
   - `SkillBar` / SkillBar controller (in-run Q/E/R/F/Z/X, class-accent ready-glow).
   - Skill DATA modeli: skill tanımları nerede (SO? enum? `*_SkillController`?), her skill'in name/description/icon/cooldown/cost alanları VAR MI? Hover-tooltip için gereken metin/veri mevcut mu?
   - CharacterSelect "roster room" (RuntimeRoot_CharSelect) — skill listesi gösteriyor mu?
2. **HOVER/FEEDBACK — reuse:** Mevcut `SkillOfferUI` hover/select juice'u (scale/glow/flash) NE KADAR var, hangi parçalar tooltip/synergy-highlight için yeniden kullanılabilir? Yeni hover efektleri (tooltip reveal, equipped-synergy highlight, audio) için en az-kod ekleme noktaları neresi?
3. **RUN-İÇİ SOL SKILL PANELİ (toggle tuş):** Bu panel run'da equipped/selected skilleri gösterecek (in-game HUD overlay, bir tuşla aç/kapa slide). RIMA'da hangi mevcut sistemden veri çeker (equipped skill listesi nerede tutuluyor — SkillBar slot'ları? SkillController array?)? Reuse: RimaUITheme + mevcut icon pipeline + 9-slice frame. En lean implementasyon iskeleti (yeni MonoBehaviour mı, mevcut HUDController'a mı eklenir)?
4. **ESC CODEX SKILL EKRANI:** Oyunu durdurup (Time.timeScale=0) TÜM sınıfa-özel skilleri tarama (build/theorycraft). Veri kaynağı: tüm class skill tanımları tek yerden enumere edilebiliyor mu? Mevcut pause/menu altyapısı var mı (SettingsMenuUI pattern reuse)? Locked/unlocked durumu nereden gelir?
5. **CURRENCY adı "Echo":** kodda nerede tanımlı/string olarak geçiyor (RunStats? bir const?), isim değişirse kaç yer dokunulur (refactor maliyeti)?

ÇIKTI FORMATI: Her sub-question için kısa "VAR / YOK / KISMEN" + dosya:satır kanıtı + "reuse şu / yeni yaz şu" önerisi. Spekülasyon yok, gerçekten gördüğünü yaz; göremediğini BLOCKED işaretle.
