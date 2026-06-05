ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect ekranını 3-kolon "centerpiece" layout'a yeniden kur: SOL = dikey class roster listesi (10 sınıf, avatar+isim, locked/selected durumları) · ORTA = seçili sınıfın büyük statik idle showcase'i (pedestal + cyan glow + faux-life bob) · SAĞ = sınıf kimliği + scrollable skill listesi + SEÇ/GERİ. KOD-ONLY (sahne .unity dosyasına dokunma; runtime-build). v1 = SIFIR yeni asset (hepsi reuse).

# Spec kaynağı (OKU — Section C tam detay)
STAGING/UI_REDESIGN_3SCREENS_DECISION_2026-06-04.md

# Dosya (sadece bu)
Assets/Scripts/UI/CharacterSelectScreen.cs  (procedural kalsın; prefab'a taşıma)

# Koru (DOKUNMA, reuse et)
BuildScreen(), SelectClass(), IsUnlocked(), UnlockCost(), LockedButtonText(), IdentityLockText(), LoadCanonicalSprite().
Reuse: RimaUITheme.AnchorPath/ClassAccent/ClassTagline/ClassIdentity/CreateFullScreenBackdrop, Pack sabitleri (bg_seal_keep, pedestal_seal, card_frame_9slice, button_9slice).

# Yapılacaklar (Section C reçetesi)

## Kolonlar (normalized anchor — fixed pixel genişlik KULLANMA, 4K çökme fix'i)
SOL anchor x 0.00–0.20 · ORTA 0.20–0.72 · SAĞ 0.72–1.00. CanvasScaler sahnede zaten 1920×1080 (koddaki fallback canvas da öyle olsun).

## SOL — Roster list (BuildCardGrid → BuildRosterList)
- 10 sınıf, dikey liste (1080p'de scrollsuz sığar; satır yüksekliği ~%7-8 ekran). Her satır: kare MASKELİ portre (AnchorPath idle_south, RectMask2D, preserveAspect, baş/gövde okunsun) + class NAME.
- Durumlar: Locked (6) = alpha ~0.35 + padlock glyph + Echo cost hint (UnlockCost). Unlocked idle (3) = alpha ~0.75 full color. Selected (1) = alpha 1.0 + 4px sol ACCENT-color bar + satır scale 1.05 + portre frame accent glow.
- Satıra tıkla → SelectClass(cls) (mevcut). Locked'a tıklanırsa LockedButtonText/IdentityLockText davranışı korunsun.

## ORTA — Showcase (BuildCenterPanel'i görsel hero yap)
- Backdrop: bg_seal_keep dimmed (~0.5) cover-crop (CreateFullScreenBackdrop mantığı, kolona sığdır). pedestal_seal tabana yakın + additive cyan glow.
- Büyük statik idle_south (~3.2x, pixel-crisp, preserveAspect) pedestal üstünde.
- Faux-life: unscaled-time küçük dikey bob (sine), cyan pedestal pulse, seçildiğinde accent glow + kısa cyan/beyaz flash. (GERÇEK animator OYNATMA — sadece 4 sınıfta controller var + UI'da ağır; statik+faux-life v1 kararı.)

## SAĞ — Detay + skills (BuildRightPanel → BuildSkillDetailPanel)
- Üst sabit kimlik bloğu: class adı (accent) + motto/playstyle/resource (RimaUITheme.ClassIdentity()).
- Altında SCROLLABLE skill listesi. ScrollRect yok → küçük yerel helper ekle: MakeScrollArea(parent,name) = ScrollRect + Viewport(Image+Mask2D) + Content(VerticalLayoutGroup + ContentSizeFitter). (bounded, min kod.)
- Skill satırı: 48px ikon (SkillDatabase d.icon, null ise RimaUITheme.PassiveIcon(skillName) fallback) + isim (accent) + 2-satır açıklama (~%75 beyaz).
- Data reality (cx audit): SkillDatabase.Instance.GetAll().Where(s => s.classType == sel && !s.isPassive). SADECE Warblade/Elementalist/Shadowblade/Ranger'da skill var. Veri OLMAYAN 6 sınıf → kimlik/resource bloğu + muted "Yetenekler yakında" notu (İKON ÜRETME, skill data uydurma).
- Alt sabit: SEÇ/CONFIRM (BuildStartButton mantığı, accent renkli, unlock'a göre interactable) + GERİ back butonu.

# Doğrulama (ZORUNLU)
- Unity MCP: refresh_unity compile=request → read_console types=error filter=CS → SIFIR CS hatası.
- Değiştirilen/eklenen metotları + compile sonucunu CODEX_DONE.md'ye yaz. Sahne dosyası DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED yaz.
