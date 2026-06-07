ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 layout PASS 2 (kullanıcının render-feel-test direktifleri): tight hitbox + karakterleri ayır (ön/arka gap, 10'u tam görünür) + skiller SAĞ DİKEY panel + kimlik SOL açılır-pencere + SEÇ/GERİ altta. KOD-ONLY, sadece CharacterSelectScreen.cs. Sahne .unity'ye dokunma.

# Spec kaynağı (TAM OKU — koordinatlar + reçete orada)
STAGING/CHARSELECT_LAYOUT2_DECISION_2026-06-04.md

# Dosya
Assets/Scripts/UI/CharacterSelectScreen.cs

# Yapılacaklar
1. **Tight hitbox** (BuildRoomCharacter'daki "Hit" full-root Image+Button): kocaman full-root rect'i KALDIR. Karakter tıklama alanı = sprite'a tight. TERCİH: Button'ı sprite Image'ine koy + `image.alphaHitTestMinimumThreshold = 0.5f` (karakter-şekilli tık) — AMA bu sprite texture'ın Read/Write enabled olmasını ister; idle_south meta'larını kontrol et, değilse bu yolu KULLANMA. ELSE: tight centered rect (root'un ~%45 genişlik × %70 yükseklik, gövde üzerinde). Büyük üst üste binen rect KALMASIN. Ön-arka draw/raycast sırası korunsun (descending anchor.y).
2. **Karakter yerleşimi** (RosterPlacements): 2 sıra, NET GAP:
   - Front (unlocked, scale ~0.95): Warblade(0.20,0.34) Elementalist(0.40,0.34) Ranger(0.58,0.34) Shadowblade(0.74,0.34)
   - Back (locked, scale ~0.82): Ronin(0.12,0.62) Ravager(0.255,0.62) Gunslinger(0.39,0.62) Brawler(0.525,0.62) Summoner(0.66,0.62) Hexer(0.795,0.62)
   - x 0.12-0.80 (sağ skill paneli + bg sütunlarından uzak), hepsi TAM görünür. Gerekirse size'ı biraz küçült (taşma/kesilme olmasın).
3. **Skiller → SAĞ DİKEY panel**: x 0.84-0.99, y 0.13-0.97. Framed (panel_frame_9slice + cyan edge), "SKILLS" başlık üstte. Mevcut horizontal MakeSkillStripArea'yı DİKEY'e çevir (VerticalLayoutGroup + dikey ScrollRect taşarsa). Skill satırı icon+isim+kısa-desc. SkillDatabase query/RefreshSkillList/BuildSkillRow data AYNEN (sadece layout dikey).
4. **Kimlik → SOL açılır-pencere**: x 0.01-0.24, y ~0.30-0.86. Framed (panel_frame_9slice + cyan edge + class-accent). SelectClass'ta güncellenir: portre (LoadCanonicalSprite) + class adı (accent) + "HEAVY · MELEE · RAGE" tarzı etiketler + motto + playstyle + resource + lock text. İlk seçimden önce gizli; her seçimde CanvasGroup alpha 0→1 ~0.15s hafif fade (opsiyonel ama "açılır pencere" hissi).
5. **SEÇ + GERİ → alt ince strip** (y 0.0-0.12), ortada/sol-alt. Locked CTA ("KİLİDİ AÇ — {cost} Echo" disabled) + scene-load AYNEN.
6. Eski bottom 3-box HUD (BuildSkillDetailPanel) bu 3 parçaya (sol-popup + sağ-dikey + alt-buton) bölünür. SelectClass'taki tüm data/portre/start-button mantığı korunur.
7. KORU: RuntimeRoot+authored-disable, backdrop resilient-load, pedestal_seal seçim halkası+glow+dim, EnsureSkillDatabase, IsUnlocked/UnlockCost. Procedural kalsın. Dynamic-layout engine YOK.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode probe: 10 char yeni 2-sıra koordinatlarda (front y0.34 / back y0.62, x 0.12-0.80), hit alanı tight (full-root DEĞİL); skiller sağ dikey panel (anchorMin.x≥0.84, VerticalLayoutGroup); kimlik sol panel (x≤0.24) SelectClass(Warblade) ile portre+name+tags dolu; SEÇ/GERİ altta (y≤0.12); SelectClass(Ronin)→KİLİDİ AÇ disabled; backdrop=room_bg; authored Root inactive.
- Değişen metot/satırları + compile + probe sonucunu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
