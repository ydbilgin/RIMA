ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect'i "diegetic roster room"a çevir: tek backdrop + odada duran 10 karakter sprite'ı + karaktere-tıkla-seç + seçilinin altında pedestal_seal halkası + diğerleri dim + alt HUD strip (kimlik | skills | SEÇ/GERİ). Sol-liste + center-showcase KALDIRILIYOR. TÜM data wiring KORUNUYOR. KOD-ONLY (sahne .unity'ye dokunma).

# Spec kaynağı (TAM OKU — section section uygula)
STAGING/CHARSELECT_ROSTERROOM_DECISION_2026-06-04.md

# Dosya (sadece bu)
Assets/Scripts/UI/CharacterSelectScreen.cs

# Build planı (cx feasibility'nin kendi least-code planı — uygula)
- KORU: BuildScreen canvas/scaler/raycaster/RuntimeRoot/authored-disable (satır 100-157) · SelectClass(557-604) · RefreshIdentityPanel(606-624) · RefreshSkillList(626-650) · EnsureSkillDatabase(840-859) · OnStartRun/OnBackClicked(700-718) · IsUnlocked/UnlockCost/LockedButtonText/IdentityLockText/CardActionText(721-758) · LoadCanonicalSprite(803-807) · RimaUITheme.ClassAccent/AnchorPath/ClassTagline/ClassIdentity.
- DEĞİŞTİR: BuildRosterList/BuildClassCard(198-324) + BuildCenterPanel(326-431) → `BuildRosterRoom(parent)` + `BuildRoomCharacter(...)`. `cards` dict → `roomEntries` dict.
- `RoomEntry` struct: root RectTransform, sprite Image, hit Image, Button, seal Image, glow Image, lock glyph/Image, costChip label, CanvasGroup, ClassType.
- Backdrop: `private const string RoomBackdrop = "UI/RIMA/CharacterSelect/room_bg";` → `RimaUITheme.CreateFullScreenBackdrop(roomLayer, RoomBackdrop, RimaUITheme.BackgroundDark)` (asset henüz yoksa BackgroundDark fallback — sorun değil, sonra import edilecek).
- Placement table (normalized anchor, decision doc'taki koordinatlar): pivot (0.5,0), anchorMin==anchorMax==pos, sizeDelta ref-units. Front (unlocked): Warblade(.35,.45) Elementalist(.45,.40) Ranger(.55,.40) Shadowblade(.65,.45). Back (locked): Ronin(.20,.60) Ravager(.32,.55) Gunslinger(.44,.52) Brawler(.56,.52) Summoner(.68,.55) Hexer(.80,.60). Y'ye göre azalan sırada sibling ata (yüksek-Y/arka önce, düşük-Y/ön sonra → ön üstte çizilir). Seçili scale ~1.12, arka base ~0.85.
- Per-character: transparent Button hit (MainMenuController.AddNakedButton pattern), listener `ClassType cls` capture → SelectClass(cls). Selected → pedestal_seal halka + cyan glow pulse + scale 1.12 + flash; others dim CanvasGroup.alpha (unlocked-unselected ~0.75, locked-unselected ~0.40). Hover glow.
- Locked: dimmed sprite (~0.40 + hafif void-purple tint) + cyan padlock glyph üstte + Echo-cost chip altta. Tıklanır → SelectClass kimlik/skills günceller; SEÇ butonu "KİLİDİ AÇ — {cost} Echo"ya döner (LockedButtonText), affordable ise cyan; startButton.interactable=IsUnlocked. Hexer özel path korunur.
- Bottom HUD strip (3 zone): dark glass #110817@0.90 + 2px cyan top border, bottom 0.0-0.25. Sol(~25%)=kimlik (name+tagline+resource, ClassIdentity). Center(~50%)=skill chips (48px icon+name+keyword, SkillDatabase query, empty-state korunur). Right(~25%)=büyük SEÇ + küçük GERİ. BuildSkillDetailPanel/BuildStartButton/BuildBackButton bu stripe taşınır (davranış aynı).
- AnimateShowcase → AnimateRoomSelection: sadece seçili entry'nin seal/glow pulse + bob. Merkez-only field'ları (portraitImage/showcaseRoot/pedestalRoot) yeni room entry'ler eşdeğer görseli sahiplendikten SONRA kaldır.
- SelectClass görsel loop: ApplyCardLockVisual → ApplyRoomEntryVisual(entry, selected). Data/start-button/scene-load mantığı AYNEN.
- Procedural kalsın (prefab YOK).

# Doğrulama (ZORUNLU)
- Unity MCP: refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode hierarchy: authored Root activeInHierarchy=false; RuntimeRoot_CharSelect altında 10 room character (her birinde sprite+hit Button) + bottom HUD strip + SEÇ/GERİ. SelectClass(Elementalist) → bottom bar Elementalist skills (Fireball/...). Locked sınıf seçilince SEÇ "KİLİDİ AÇ ... Echo".
- Değiştirilen/eklenen metotları + compile + hierarchy sonucunu profil-DONE dosyasına yaz. Sahne .unity DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
