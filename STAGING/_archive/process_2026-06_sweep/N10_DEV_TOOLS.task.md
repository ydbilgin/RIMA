ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read: kod / STAGING / CURRENT_STATUS.md.

# Amaç
Demo/dev iş akışını hızlandıran 3 küçük tool yaz. Writer=sen (Codex), reviewer=Opus. Animasyon-bağımsız, düşük-risk, compile-clean bırak. Spec: STAGING/N9_UX_TOOLS_FINAL.md.

# ÖNCE OKU
- Assets/Scripts/Systems/Map/RoomLoader.cs  (LoadFirstRoom / SwapRoomWhileBlack / BuildRoomContent pattern — LoadRoomByIndex bunları reuse edecek)
- Assets/Scripts/Core/Health.cs  (TakeDamage(int), CurrentHP/MaxHP — Debug panel kullanır)

# GÖREV (3 dosya)
## 1. RoomLoader.cs += LoadRoomByIndex
- `public void LoadRoomByIndex(int index)` — `SwapRoomWhileBlack(index, _sequence[index])` pattern'i (Teardown + TeleportPlayer(playerStartPos) + BuildRoomContent + CurrentRoomIndex/CurrentRoomData set + OnRoomChanged + SetHudRoomStatus). Range guard (0..Length-1). Transition FX opsiyonel (direkt swap yeter). Static helper de ekle: `public static void JumpToRoom(int i)` → FindFirstObjectByType<RoomLoader>()?.LoadRoomByIndex(i).
- Mevcut metotları BOZMA, sadece ekle.

## 2. YENİ `Assets/Scripts/Debug/DemoDebugPanel.cs`
- `#if DEVELOPMENT_BUILD || UNITY_EDITOR` ile tüm sınıfı sarmala (production'da sıfır kod).
- MonoBehaviour, OnGUI. F1 ile toggle (Keyboard.current.f1Key.wasPressedThisFrame — Input System aktif, legacy Input.Get* KULLANMA).
- Butonlar: **Kill All Mobs** (FindObjectsByType<Health> + tag=Enemy → TakeDamage(99999)), **God Mode** (player Health'i her frame MaxHP'ye set eden toggle flag), **Speed** (Time.timeScale 1↔2↔0.25 cycle), **Force Room Clear** (RoomLoader.OnRoomCleared'ı tetikle — RoomLoader'a `public static void DebugForceRoomCleared()` helper ekleyebilirsin VEYA reflection; helper tercih), **Restart Room** (RoomLoader.JumpToRoom(CurrentRoomIndex)), **Next Room** (RoomLoader.LoadNext), **Jump Room 1-5** (JumpToRoom(i)).
- Self-bootstrap: `[RuntimeInitializeOnLoadMethod]` ile sahnede yoksa bir GameObject'e AddComponent (DontDestroyOnLoad). Sahneyi MODIFY ETME.

## 3. YENİ `Assets/Editor/RimaDevShortcuts.cs`
- Editor-only. `[MenuItem("RIMA/Play Arena _F5")]` (veya ShortcutAttribute) → açık sahneyi kaydet (EditorSceneManager.SaveOpenScenes) + OpenScene("Assets/Scenes/Test/PlayableArena_Test01.unity") + EditorApplication.isPlaying=true.
- Ek `[MenuItem("RIMA/Stop Play _F6")]` opsiyonel.

# KISITLAR
- Min kod, surgical. SADECE bu 3 dosya (+ RoomLoader'a helper). Sahne/prefab MODIFY ETME.
- Input System (`UnityEngine.InputSystem`, Keyboard.current) — legacy Input YASAK.
- DemoDebugPanel #if guard'lı (production temiz).
- Compile-clean. dotnet build veya en azından syntax-temiz bırak. Belirsizlik → BLOCKED.

# ÇIKTI
Değişen/yeni dosyalar + özet + compile durumu. Reflection kullandıysan belirt.
