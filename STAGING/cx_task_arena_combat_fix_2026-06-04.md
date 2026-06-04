ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
_Arena combat foundation çalışmıyor: RoomRunDirector serialized ref'leri sahnede NULL (playerPrefab/encounterBank/arenaCamera/enemyContainer) → player+enemy spawn sessizce no-op. FIX = bu ref'leri RUNTIME'da KENDİ KENDİNE çözsün (manuel sahne-wiring gerekmesin).

⚠️ **KOD-ONLY. `_Arena.unity` (veya HİÇBİR .unity) sahnesini DÜZENLEME** — Opus o sahneyi editörde açık tutuyor, scene-edit "externally modified" modal'ı tetikleyip MCP'yi kilitliyor. Sadece .cs düzenle.

## Teşhis (Opus runtime introspection)
RoomRunDirector alanları: builder=OK, roomBank=DemoRoomBank OK, fallbackTemplate=Combat_Large_01 OK, ama playerPrefab=NULL, encounterBank=NULL, arenaCamera=NULL, enemyContainer=NULL, warnedMissingPlayer=True. Yani BuildCurrentRoom oda+kapı+kamera kuruyor ama spawn için ref yok.

## Dosya
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs (ana)
- Gerekirse: Warblade player prefab / encounter bank'i Resources'tan yüklenebilir yap (aşağı bak)

## Yapılacaklar (runtime self-resolve)
1. **arenaCamera:** null ise `Camera.main` kullan.
2. **enemyContainer:** null ise runtime'da bir GameObject ("Enemies") yarat ve onu kullan.
3. **playerPrefab:** null ise runtime çöz. GERÇEK player prefab = `Assets/Prefabs/Characters/Warblade.prefab` (Resources'ta DEĞİL). Çözüm seçenekleri (sen en temizini seç):
   (a) Warblade.prefab'i (ve bağımlı Player.prefab'i) `Assets/Resources/Prefabs/` altına KOPYALA (editör-time bir kez; .cs'te `Resources.Load<GameObject>("Prefabs/Warblade")`), VEYA
   (b) PlayerClassManager/var olan player-spawn sistemini kullan (eğer Resources-tabanlı bir spawn yolu varsa).
   PlayerClassManager.SelectedClass yoksa Warblade default. Spawn'ı `IsoRoomBuilder.PlayerSpawnMarker` konumunda, tag="Player".
4. **encounterBank:** null ise `Resources.Load` ile çöz. cx-recon'da kullanılan bank = `Act1_EncounterBank_Pilot`. Resources'ta değilse `Assets/Resources/` altına uygun yere KOPYALA + `Resources.Load`. (Sadece bu asset'i + bağımlılıklarını taşı; Resources kopyaları editör-time.)
5. Spawn akışı: oda build olunca enemySpawnSockets'e EncounterBankSO.PickWave + ThreatBudget.Spawn ile düşman; eski düşmanları rebuild'de temizle (zaten activeEnemies var).
6. Null kalırsa anlamlı `Debug.LogWarning` (sessiz no-op YOK).

## Başarı kriteri (Opus play-verify edecek)
_Arena F5 → kamera odayı çerçeveler → tag="Player" Warblade spawn olur + WASD yürür/saldırır → enemySpawnSockets'te fonksiyonel düşman spawn olur + dövüş. Console 0 hata.

## Doğrulama
- dotnet/validate_script compile-clean (0 hata). Resources'a asset kopyaladıysan AssetDatabase.Refresh.
- Play-mode'a GİRME. _Arena.unity'i DÜZENLEME.
- CODEX_DONE.md'ye: hangi çözümü seçtin (3a/3b/4), değişen .cs + kopyalanan Resources asset'leri, compile.
