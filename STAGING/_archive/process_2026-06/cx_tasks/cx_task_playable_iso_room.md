ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
`_IsoGame` sahnesi F5'te (play mode) OYNANABİLİR ve İSO FLOOR'lu olsun. Şu an authored iso oda yok ediliyor, yerine çirkin DÜZ gri prosedürel oda kuruluyor. Hedef: F5 → seamless iso granit floor (floor451) görünür + oyuncu kontrol edilebilir (hareket/saldırı) + ölmez/hata yok. Bu, "oynanabilir bir demo" için ilk somut adım.

# Doğrulanmış semptom (Opus, MCP ile gözlemledi, 2026-06-01)
- EDIT mode: `_IsoGame` sahnesinde `IsoGrid` (5 child tilemap, floor451 iso granit) + tam `Player` + `Systems` var, görünüm GÜZEL (seamless iso yüzen ada).
- PLAY mode (F5): hiyerarşideki authored iso oda KULLANILMIYOR. `Systems` altındaki `RuntimeRoomManager` Start'ta prosedürel DÜZ (flat top-down) gri bir oda kuruyor → iso floor kaybolyor. Screenshot kanıt: `Assets/Screenshots/screenshot-20260601-193153.png` (gri blob oda) vs edit-mode `screenshot-20260601-193109.png` (iso ada).
- Konsol play-mode'da 0 hata (sadece "odd-numbered resolution" pixel-perfect uyarısı, önemsiz).

# Suçlu adaylar (önce INVESTIGATE et, sonra minimal fix)
- `Assets/Scripts/Core/RuntimeRoomManager.cs` (sahnede `Systems` objesinde aktif — ANA ŞÜPHELİ)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` VE `Assets/Scripts/Map/Runtime/RoomLoader.cs` (İKİ RoomLoader var — hangisi aktif?)
- `Assets/Scripts/Map/RoomBuilder.cs`
- `Assets/Scripts/Core/DungeonGraph.cs` (Systems'te, prosedürel grafiği üretiyor olabilir)
- `RoomConfig.cs` (IsoCellSize/IsoGridLayout sabitleri buradaydı — önceki floor-fix)

# Görev (minimal, surgical, RİSKSİZ)
1. F5 akışını izle: `RuntimeRoomManager` Start/Awake'te hangi yolu çağırıp authored `IsoGrid`/`Ground`'u yok edip/atlayıp prosedürel flat oda kuruyor? Tam satırı bul.
2. EN GÜVENLİ minimal fix'i uygula. TERCİH SIRASI:
   a. **Authored-room mode (tercih):** `RuntimeRoomManager`'a serialized bir bool ekle (örn. `useAuthoredSceneRoom`, default `false`). True ise: prosedürel build'i ATLA, sahnedeki mevcut `IsoGrid` odasını + Player spawn'ı kullan, combat/door/draft akışı authored odayla çalışsın. Bu flag'i SADECE `_IsoGame.unity` sahnesinde true yap (diğer sahneler default false = davranış DEĞİŞMEZ).
   b. Eğer (a) mümkün değilse: prosedürel builder'ı iso floor (floor451 + IsoGrid cellSize 0.96x0.585, squash YOK) kuracak şekilde düzelt — eski flat floor'u bırak.
3. KIRMA: `PlayableArena_Test01.unity` ve diğer sahnelerin davranışını DEĞİŞTİRME. Fix additive olsun (flag default-off). Bu yüzden (a) tercih edilir.
4. Pre-existing dead code'a dokunma; sadece kendi değişikliğinin unused import'larını temizle.

# Doğrulama (ZORUNLU — kanıtsız "done" deme)
- dotnet build VEYA Unity refresh sonrası `read_console` → 0 derleme hatası.
- Unity MCP ile: `_IsoGame` aç → F5 (manage_editor play) → `manage_camera` game_view screenshot → iso granit floor GÖRÜNÜYOR mu + Player var mı? → stop. Screenshot yolunu CODEX_DONE.md'ye yaz.
- Diğer bir sahneyi (örn. PlayableArena_Test01) açıp F5'in hâlâ eskisi gibi çalıştığını doğrula (regression yok).
- Unity zaten AÇIK; UnityMCP profil config'de tanımlı. execute_code'da `using` YASAK (tam-nitelikli namespace). AssetDatabase batch wrapper + scene save discipline.

# Çıktı (CODEX_DONE.md)
- Kök-neden (hangi dosya:satır prosedürel flat oda kuruyordu).
- Uygulanan fix (hangi yaklaşım a/b, hangi dosyalar, kaç satır).
- Doğrulama sonucu: derleme + F5 screenshot yolu + regression kontrolü.
- Değiştirilen dosya listesi.
- Belirsizlik/risk varsa BLOCKED yaz, sessizce yarım bırakma.
