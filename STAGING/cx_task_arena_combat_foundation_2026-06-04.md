ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (B-11 ilk oynanabilir combat milestone)
Veri-bazlı `_Arena` sahnesinde, IsoRoomBuilder ile kurulan oda **yürünebilir + dövüşülebilir** olsun: gerçek player + düşmanlar + büyük-oda kamera-fit. (Clear→reward→door SONRAKİ milestone — bu görevde DEĞİL, ama mevcut BuildExitDoors'a dokunma.)

## ÖNCE RECON (raporla, sonra uygula)
1. `_Arena.unity` mevcut kurulum: RoomRunDirector, IsoRoomBuilder, RunMapOverlay, DemoPlayer (placeholder), kamera. RoomRunDirector oda build akışı (Pick→Build→teleport→BuildExitDoors) NASIL çalışıyor, nerede player/enemy eksik?
2. GERÇEK player: `_IsoGame.unity`'deki "Player" tag'li obje + controller'ı (movement + attack + Health) REFERANS al. Hangi prefab/component? (PlayerClassManager.SelectedClass ile sınıf geliyor.)
3. Fonksiyonel düşman prefab'ları: `Assets/Prefabs/Enemies/` (Health + BaseMobBehavior olanlar; PixelLab enemy_XX = sadece görsel, KULLANMA). Hangileri fightable?
4. EncounterController + EncounterWaveSO/EncounterBankSO/ThreatBudget var mı, API'si ne? Oda enemySpawnSockets'ine spawn için uygun mu, yoksa direkt mi spawn etmeli?

## UYGULA (recon sonrası)
A. **Kamera-fit:** IsoRoomBuilder oda kurunca kamera oda bounds'unu çerçevelesin (büyük odalar — donut 28x20, blob 36x24 — şu an taşıyor). orthographicSize'ı oda genişliğinden hesapla + merkeze al. PixelPerfect varsa refResolution/zoom ayarı. RoomRunDirector.BuildCurrentRoom sonrası çağır.
B. **Player:** DemoPlayer placeholder yerine GERÇEK player'ı `IsoRoomBuilder.PlayerSpawnMarker` konumunda spawn et (tag "Player", movement+attack+Health çalışır). PlayerClassManager.SelectedClass yoksa default Warblade.
C. **Düşman:** oda build olunca `RoomTemplateSO.enemySpawnSockets` konumlarına fonksiyonel düşman spawn et (EncounterController uygunsa onunla, değilse RoomRunDirector'da basit spawn). Düşmanlar player'a saldırsın, ölebilsin.

## Başarı kriteri (play-verify Opus yapacak)
_Arena F5 → kamera odayı çerçeveler → player WASD ile yürür + saldırır → düşmanlar var + dövüş çalışır + ölürler. Console 0 hata.

## Notlar
- Test odası: RoomRunDirector ne pick ederse (DemoRoomBank). İstersen recon'da hangi oda geldiğini söyle.
- Min code, cerrahi. Belirsizlik (player prefab net değilse) → BLOCKED yaz, tahmin etme.
- Play-mode'a GİRME (D3D11 ama yine de Opus play-verify edecek). dotnet/validate_script ile compile-clean.
- CODEX_DONE.md'ye: recon bulguları + değişen dosyalar + 3 madde (A/B/C) durumu + compile.
