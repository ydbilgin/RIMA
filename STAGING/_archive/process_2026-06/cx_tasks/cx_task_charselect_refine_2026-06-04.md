ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 layout refinement: (1) bottom HUD'da 3 ÇERÇEVELİ kutu (9-slice), (2) karakterleri GENİŞ yay (koordinat tablosu), (3) skiller bottom-bar SAĞ zone (vertical panel DEĞİL), (4) sol kutu = seçili karakter portresi + kimlik. KOD-ONLY, sadece CharacterSelectScreen.cs. Sahne .unity'ye dokunma.

# Spec kaynağı (TAM OKU)
STAGING/CHARSELECT_REFINE_DECISION_2026-06-04.md (Implementation bölümü + koordinatlar).

# Dosya (sadece bu)
Assets/Scripts/UI/CharacterSelectScreen.cs

# Yapılacaklar (cx'in kendi least-code planı)
1. Const ekle (satır ~30-33): `private const string PackPanelFrame = "UI/RIMA/Pack/panel_frame_9slice";`
2. **Framed boxes** — BuildSkillDetailPanel (413-507): IdentityZone/SkillZone/ActionZone'u MakeRect'ten **framed MakePanel**'e çevir (aynı anchor/offset). Her zone Image: `sprite = Resources.Load<Sprite>(PackPanelFrame) ?? RimaUITheme.SmallPanelFrame; type = Image.Type.Sliced; color = koyu translucent tint (örn #110817 @ ~0.88); raycastTarget=false;` + ince cyan (#00FFCC) kenar child (sliced, +2px). İç içerik (text/skill/button) AYNEN kalsın. Zone sırası: SKİLLER en SAĞDA olsun.
3. **Sol kutu portresi** — IdentityZone'a seçili karakterin portre Image'ini ekle (LoadCanonicalSprite(cls), preserveAspect, RectMask2D ile kare) + mevcut name/tagline/resource. SelectClass'ta bu portre güncellensin (LoadCanonicalSprite(cls)).
4. **Geniş yay** — RosterPlacements (65-77): koordinatları DEĞİŞTİR:
   - Front (unlocked, size 300x410, scale ~0.98): Warblade(0.22,0.44) Elementalist(0.41,0.39) Ranger(0.59,0.39) Shadowblade(0.78,0.44)
   - Back (locked, size 260x360, scale ~0.84): Ronin(0.08,0.61) Ravager(0.25,0.57) Gunslinger(0.42,0.54) Brawler(0.58,0.54) Summoner(0.75,0.57) Hexer(0.92,0.61)
5. DOKUNMA: RefreshSkillList, BuildSkillRow, MakeSkillStripArea, BuildRoomCharacter click/lock/seal, SelectClass data wiring (sadece portre refresh ekle), backdrop resilient-load, EnsureSkillDatabase.
6. Procedural kalsın. Dynamic-layout engine YOK (hardcoded coords + CanvasScaler).

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode probe: 10 char yeni geniş koordinatlarda (x .08-.92); IdentityZone/SkillZone/ActionZone'da panel_frame_9slice sprite'lı Image (Sliced); skiller en sağ zone'da; sol kutuda seçili portre; SelectClass(Elementalist)→Fireball skills; SelectClass(Ronin)→KİLİDİ AÇ disabled; authored Root inactive; Backdrop sprite=room_bg.
- Değişen metot/satırları + compile + probe sonucunu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
