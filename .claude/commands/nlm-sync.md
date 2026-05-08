---
description: Dosyaları NotebookLM'e kaynak olarak ekle/güncelle (eski versiyonu siler, yeni ekler). /nlm-sync → tüm değişenleri batch sync. /nlm-sync path/to/file.md → tek dosya.
allowed-tools: Bash
---

# /nlm-sync — NotebookLM Source Sync (deduplicated, parallel)

Run the command below and return its output verbatim. Do not add commentary.

```bash
NB=ed3c8952-417c-4988-84a7-425d25ba3b08
FILE="$ARGUMENTS"
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"

NODE_FILTER='
var c="",t=process.argv[1];
process.stdin.setEncoding("utf8");
process.stdin.on("data",function(d){c+=d;});
process.stdin.on("end",function(){
  try{var s=JSON.parse(c);var ids=s.filter(function(x){return x.title===t;}).map(function(x){return x.id;});if(ids.length)console.log(ids.join(" "));}catch(e){}
});'

if [ -z "$FILE" ]; then
  echo "=== NLM Batch Sync (parallel) ==="
  CHANGED=$(git -C "$REPO" status --short 2>/dev/null \
    | grep -v "^D " \
    | cut -c4- \
    | grep -E "\.(md)$" \
    | grep -iE "(TASARIM/|MEMORY/|STAGING/|CURRENT_STATUS|CLAUDE\.md|RULES\.md|AGENTS\.md)")
  if [ -z "$CHANGED" ]; then
    echo "No relevant changed files found."
  else
    PIDS=()
    while IFS= read -r rel; do
      full="$REPO/$rel"
      if [ -f "$full" ]; then
        echo "Queuing: $rel"
        (
          base=$(basename "$full")
          old=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null | node -e "$NODE_FILTER" "$base")
          if [ -n "$old" ]; then
            uvx --from notebooklm-mcp-cli nlm source delete $old --confirm 2>/dev/null
            echo "  Deleted: $base"
          fi
          uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1 | grep -E "(Added|Error|added)"
          echo "  Done: $base"
          hash=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
          [ -n "$hash" ] && echo "${rel}|${hash}" >> "$STATE"
        ) &
        PIDS+=($!)
      fi
    done <<< "$CHANGED"
    wait "${PIDS[@]}"
    echo "=== Done ==="
  fi
else
  if [[ "$FILE" = /* ]]; then
    rel="${FILE#$REPO/}"
    full="$FILE"
  else
    rel="$FILE"
    full="$REPO/$FILE"
  fi
  base=$(basename "$full")
  old=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null | node -e "$NODE_FILTER" "$base")
  if [ -n "$old" ]; then
    uvx --from notebooklm-mcp-cli nlm source delete $old --confirm 2>/dev/null
    echo "  Deleted: $base"
  fi
  uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1 | grep -E "(Added|Error|added)"
  echo "  Done: $base"
  hash=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
  [ -n "$hash" ] && echo "${rel}|${hash}" >> "$STATE"
fi
```

**Usage:**
- `/nlm-sync` → batch: git status'taki tüm TASARIM/MEMORY/STAGING/CURRENT_STATUS değişikliklerini paralel sync et
- `/nlm-sync CURRENT_STATUS.md` → tek dosya sync
- `/nlm-sync TASARIM/room_authoring.md` → tek dosya sync

**Not:** Paralel çalışır — rate limit yerse hata çıkar, sequential versiyona dön.
