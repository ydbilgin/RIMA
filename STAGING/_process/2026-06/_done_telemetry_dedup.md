## Telemetry dedup — 2026-06-18

| File | Removed line (content) |
|------|------------------------|
| `Assets/Scripts/Skills/Warblade/WarStomp.cs` ~L43 | `Debug.Log($"[Damage] {damage} -> {hp.name} (warstomp)");` |
| `Assets/Scripts/Skills/Elementalist/Blink.cs` ~L52 | `Debug.Log($"[Damage] {damage} -> {hp.name} (blink)");` |
| `Assets/Scripts/Skills/Elementalist/GlacialSpike.cs` ~L67 | `Debug.Log($"[Damage] {dmg} -> {hp.name} (glacialspike)");` |
| `Assets/Scripts/Skills/Warblade/DeepWound.cs` ~L53 | `Debug.Log($"[Damage] {bleedDamagePerTick} -> {hp.name} (bleed)");` |

Console after recompile: **0 errors, 0 warnings**.
BLOCKED: none. DO-NOT-TOUCH lines (DeepWound ~L38 2-arg, Earthsplitter ~L41) untouched.
