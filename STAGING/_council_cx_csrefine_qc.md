ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 layout refinement'inin (framed boxes + geniş karakter yayılımı + skiller bottom-right + sol portre) UNITY'DE GERÇEKTEN uygulandığını kendi gözünle (Unity MCP play-mode) DOĞRULA. ANALİZ/OBSERVE ONLY — kod/asset/sahne DEĞİŞTİRME. PASS/FAIL + gözlemleri profil-DONE'a yaz.

# Beklenen (decision: STAGING/CHARSELECT_REFINE_DECISION_2026-06-04.md)
Yöntem: Unity MCP → manage_scene load Assets/Scenes/UI/CharacterSelect.unity → manage_editor play → execute_code ile gözlemle → manage_editor stop.

Kontrol et + raporla (her madde için gerçek değer):
1. authored "Root" activeInHierarchy=false; "RuntimeRoot_CharSelect" aktif.
2. RoomLayer'da 10 RoomCharacter_*; X anchor'ları GENİŞ (~0.08–0.92 arası, eski 0.20–0.80 DEĞİL). Birkaç karakterin (Ronin~0.08, Hexer~0.92, Elementalist~0.41) anchorMin.x değerini oku ve raporla.
3. BottomHUDStrip'te 3 zone (Identity/Skill/Action) artık panel_frame_9slice sprite'lı Image (Image.Type.Sliced) — her zone'un Image.sprite.name'ini ve type'ını raporla. Skiller zone'u en SAĞDA mı (anchorMin.x en büyük)?
4. IdentityZone'da seçili karakterin portre Image'i var mı; SelectClass(Elementalist) sonrası portre elementalist_idle_south mu?
5. SelectClass(Elementalist) → skill satırlarında FIREBALL var mı; SelectClass(Ronin) → Start button "KİLİDİ AÇ"/"KILIDI AC" + interactable=false mu?
6. Backdrop Image sprite=room_bg mi.
7. read_console types=error → 0 mı.

Verdict: PASS / PASS-WITH-NITS / FAIL + en önemli 3 gözlem (gerçek değerlerle). Sahne kaydetme.
