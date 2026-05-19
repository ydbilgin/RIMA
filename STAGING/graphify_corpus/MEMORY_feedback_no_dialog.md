---
name: Feedback: Do not use DisplayDialog
description: BANNED in Unity Editor scripts. Blocks main thread.
type: feedback
---
* **Rule:** `EditorUtility.DisplayDialog(...)` is **BANNED**.
* **Reason:** Blocks Editor, causes MCP timeouts.
* **Alternative:** Use `Debug.Log(...)` only.
