---
description: Dosyaları NotebookLM'e kaynak olarak ekle/güncelle. Kullanım: /nlm-sync path/to/file.md
allowed-tools: Bash
---

# /nlm-sync [file_path] — NotebookLM Source Sync

Run the command below and return its output verbatim. Do not add commentary.

```bash
uvx --from notebooklm-mcp-cli nlm source add ed3c8952-417c-4988-84a7-425d25ba3b08 --file "$ARGUMENTS" --wait 2>&1
```

**Usage examples:**
- `/nlm-sync CURRENT_STATUS.md`
- `/nlm-sync TASARIM/room_authoring.md`
- `/nlm-sync MEMORY/project_session_2026_05_08.md`

**Not:** Tek seferde bir dosya. Birden fazla dosya için birkaç kez çağır.
