# COUNCIL + OPUS BRIEF — ChatGPT paketi asset/Egg/UI + PixelLab üretim önceliklendirme (2026-06-16)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: RIMA design context gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read: bu brief + paket docs + memory + STAGING.
E1: Çıktını DOSYAYA yaz, dönüşte ≤12 satır özet + dosya yolu ver.

## Bağlam
RIMA bitirme demosu 19 Haziran (= 3 gün, 2026-06-16 itibarıyla). Strateji KİLİTLİ: "oyun değil environment + ilk vertical slice" (%20 oyun/%60 mimari/%20 graphify), centerpiece=Edit-to-Play video, **golden-path-first**, **scope-lock HOLD** (LaurethStudio handoff council oybirliği = polish/mekanik öneriler HEPSİ post-demo). Az önce **REWARD-02 (G ödül UI açmıyor) GERÇEK bug olarak doğrulandı** = demo-kritik fix bekliyor (RewardPickup.cs OnTriggerStay2D+OverlapCircle, NO-REFACTOR).

Kullanıcı **PixelLab'dan asset üretmek istiyor** ("pixellabdan bi şeyler çıkaracaz"). ChatGPT playtest paketi (`package_docs/` aynı klasörde) 3 büyük asset teslimatı içeriyor:
- **(A) Modüler UI asset pack** (`10_MODULAR_UI_ASSET_PACK.md` + `11_UI_ASSET_MANIFEST.csv`): ~50 parça, 9-slice/overlay; polish-first altın kural; atlas stratejisi; PixelLab/ChatGPT-Image üretim akışı + prompt şablonu.
- **(B) Rift-Forged Egg** (`12_RIFT_FORGED_EGG_SYSTEM.md` + `13_..._ART_PROMPTS.md`): world-reward mystery container (YENİ ekonomi DEĞİL = mevcut reward'ın diegetic skin'i); 7 visual state; 96/128px anim; 3 art prompt. Paket KENDİSİ "playtest bugları çözülmeden yapılmamalı, incubation post-demo" diyor.
- **(C) UI/UX polish spec** (`08_UI_UX_POLISH_SPEC.md`, `09_SCREEN_BY_SCREEN_VISUAL_GUIDE.md`): HUD/Pause/Settings/Codex/Reward hedef görünümleri.
Görsel polish referansları (ax_pro vision için): `C:/Users/ydbil/AppData/Local/Temp/rima_playtest/RIMA_CLAUDE_PLAYTEST_UI_COMPLETE_2026-06-16/03_UI_UX_POLISH/GENERATED_POLISH_SCREENS/VP-01..06.png` + `OVERVIEW_IMAGES/OV-01/02.png`.

Ayrıca beklemede (bu paketten ÖNCE): **warblade anim redo** (armed-anchor greatsword'dan, tutarlılık için) — `STAGING/ANIM_PRODUCTION_HANDOFF_2026-06-15.md`.

## RIMA CANON (asset üretimi için bağlayıcı — çatışma flag'le)
- **Yön:** Karakter sprite **8 yön LOCKED (Karar #114)** = 5 sprite (S/SE/E/NE/N) + 3 mirror flipX. ⚠️ ChatGPT paketi "S/E/N/W 4-cardinal, flip YOK, 35° ARPG" diyor → KARAKTER için canon-ÇATIŞMASI. (UI flat = yön yok; tek-yön prop/Egg = yön kuralı uygulanmaz ama view-açısı tutarlı olmalı.)
- **View:** HIGH TOP-DOWN 3/4 (~70-80° from horizon, Hades/Children of Morta ref). Paketin "35-degree top-down"u bunu mu kastediyor, çelişiyor mu — netleştir.
- **Stil canon (Act1):** slate #3A3D42 · void MOR #3A1A4A (sınırlı) · cyan ≤%15 · ember/amber #E89020 · "no dark-fantasy, no purple neon overload". Paket stil tarifi ("blackened slate, dark iron, restrained cyan, amber, no purple overload") = canon ile UYUMLU görünüyor — teyit et.
- **Boyut/PPU:** karakter canvas 120x120, tile 32x32, PPU 64 (sprite), VFX 64-128. UI = ayrı pipeline (9-slice, Point filter, mipmap off, PPU irrelevant).
- **PixelLab maliyet:** **V3 mode ucuz/kullanılır; PRO pahalı — KULLANMA.** decals daima transparent + alfa-doğrula. Karakter: Create Image Pro (master sheet)→Create Character→Custom Anim V3. UI/prop: Create Image Pro / init image.
- **Felsefe:** modüler-tasarım disiplini ("modülerliği hak ediyor mu?"); polish-first (önce hedef ekran onayı, sonra parça çıkarımı) = PNG-mezarlığı önleme.

## SORULAR (her advisor net cevaplasın)
1. **Demo öncesi (≤4 gün) PixelLab'dan NE üretmeli, NE post-demo?** Golden-path filtresi: sadece Edit-to-Play videosundaki akışı/algıyı güçlendiren asset. (A/B/C + warblade-anim'i demo/post-demo ayır; gerekçe.)
2. **ChatGPT paketinde benimsenmeye değer EKSTRA ne var** (asset/Egg/UI dışında, REWARD/canon-dışında gözden kaçan değerli bir şey)?
3. **Canon-çatışması/risk var mı?** (35° vs 70-80°; 4-cardinal vs 8-dir; stil; UI scale 1080p/1440p; atlas stratejisi RIMA'ya uygun mu).
4. **Üretilecek HER asset için kısa spec doğrulaması:** PixelLab modu (V3/Create Image Pro), boyut, RIMA-stil prompt doğru mu, transparent mı.

## ÇIKTI FORMAT (dosyaya yaz)
- VERDICT: demo PixelLab üretim listesi (öncelikli) | post-demo listesi | DROP listesi.
- Canon-flag tablosu (madde → çatışma var/yok → öneri).
- "Ekstra değerli" 1-3 madde.
- Riskler.
Maks ~60 satır. Dönüşte ≤12 satır özet + yazdığın dosya yolu.
