ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Ödül (RewardPickup) artık üstüne gelince OTOMATİK draft açıyor; kullanıcı **G tuşuyla** açmak istiyor (menzildeyken prompt göster, G'ye basınca topla). DoorTrigger'daki press-G pattern'ini birebir reuse et.

# Dosyalar (SADECE)
- Assets/Scripts/Core/RewardPickup.cs   (OKU — şu an OnTriggerEnter2D otomatik collect)
- Assets/Scripts/Core/DoorTrigger.cs    (REFERANS — press-G pattern: Key.G, playerInRange, prompt göster/gizle, Keyboard.current[InteractKey].wasPressedThisFrame)

# Değişiklik (RewardPickup.cs)
- `using UnityEngine.InputSystem;` ekle.
- `OnTriggerEnter2D` (Player girince): artık DİREKT collect ETME. Bunun yerine `playerInRange=true` + prompt göster ("Topla: G" / "Press G"). `collected` ise hiçbir şey yapma.
- `OnTriggerExit2D` ekle: `playerInRange=false` + prompt gizle.
- `Update`: `if(playerInRange && !collected && Keyboard.current!=null && Keyboard.current[Key.G].wasPressedThisFrame) Collect();`
- Mevcut collect gövdesini (sprite/collider gizle + `collected=true` + `DraftThenOpenExit()` coroutine) bir `Collect()` metoduna taşı; davranışı AYNEN koru.
- Prompt: DoorTrigger'ın prompt yaklaşımını reuse et (HUDController.SetInteractionPrompt varsa onu çağır; yoksa DoorTrigger'ın auto-built world-space promptCanvas/promptText şeklini kopyala). Basit tut.
- `DraftThenOpenExit()` + `collected` guard + ActivateExitDoors zincirini DEĞİŞTİRME.

# Doğrulama / çıktı
- Compile-mantıklı (UnityEngine; Unity'de orchestrator doğrular). Reward→draft→door zincirini bozma.
- Sonuç → CODEX_DONE.md: değişiklik özeti + prompt nasıl gösteriliyor + riskli nokta.
