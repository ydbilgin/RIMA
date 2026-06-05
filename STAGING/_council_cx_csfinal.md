ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharSelect final-polish + demo unlock economy FEASIBILITY/ROOT-CAUSE. ANALİZ ONLY. Sonucu profil-DONE'a yaz.

# READ
STAGING/CHARSELECT_FINAL_BRIEF_2026-06-04.md + Assets/Scripts/UI/CharacterSelectScreen.cs + PlayerClassManager (grep `SelectedClass`, `PlayerClassManager`) + Echo/unlock kaynağı (grep `Echo`, `IsUnlocked`, `UnlockCost`, `Unlock`, `PlayerPrefs`).

# Answer (concrete, cite file/line)
1. **CLASS-CARRY BUG (kök neden):** Seçilen sınıf gameplay'e nasıl taşınıyor? `PlayerClassManager.SelectedClass` NEREDE set ediliyor? CharacterSelectScreen'in SelectClass / SEÇ-OnStartRun bunu set ediyor mu yoksa sadece UI mı güncelliyor? Neden HER ZAMAN Warblade geliyor (default mu, set kayıp mı, scene-load yanlış mı)? Tam dosya/satır + minimal fix.
2. **ECHO sistemi:** Echo nerede tutuluyor (RunStats? PlayerPrefs? meta-save?)? IsUnlocked/UnlockCost (CharacterSelectScreen.cs ~721/749) neyi okuyor? Unlock PERSIST ediliyor mu (bir sınıf açılınca kaydediliyor mu)? Mevcut bir "unlock" API/metot var mı?
3. **Demo Echo + functional unlock — least-code:** (a) demo başlangıç Echo bakiyesi nasıl seed edilir (en az kod), (b) KİLİDİ AÇ butonu Echo ≥ cost ise: Echo harca + unlock persist + char'ı normale/selectable çevir — nasıl? Mevcut hangi metotlar kullanılır?
4. **Locked NOT selectable:** locked char'a tıklayınca playable-seçim OLMASIN (SEÇ sadece unlocked'ta). En temiz gate (SelectClass'ta locked branch?).
5. **Locked silhouette feasibility:** idle_south sprite'ı siyah tint (Color ~black, alpha korunur) ile temiz silüet verir mi (Image.color)? Unlock'ta normale dön.
6. **Bigger panels + no-occlusion least-code:** identity/skills panel'i büyütmek + char-band'i daraltmak için hangi metot/satır (BuildScreen anchor'ları + RosterPlacements)?

Terse, cite RIMA paths/methods/lines.
