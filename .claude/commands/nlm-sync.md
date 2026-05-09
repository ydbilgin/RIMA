---
description: Dosyaları NotebookLM'e kaynak olarak ekle/güncelle (eski versiyonu siler, yeni ekler). /nlm-sync → tüm unsynced dosyaları batch sync. /nlm-sync --status → sync bekleyenleri göster (sync etmez). /nlm-sync path/to/file.md → tek dosya.
allowed-tools: Bash
---

# /nlm-sync — NotebookLM Source Sync (hash-based, parallel)

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

if [ "$FILE" = "--status" ]; then
  LS=$(cat "$REPO/.claude/nlm_last_sync.txt" 2>/dev/null || echo "hiç sync edilmedi")
  UNSYNCED=$(
    {
      find "$REPO/TASARIM" "$REPO/MEMORY" "$REPO/STAGING" -maxdepth 1 -name "*.md" 2>/dev/null | sed "s|$REPO/||"
      for f in CURRENT_STATUS.md CLAUDE.md RULES.md AGENTS.md; do
        [ -f "$REPO/$f" ] && echo "$f"
      done
    } | sort -u | while IFS= read -r rel; do
      full="$REPO/$rel"
      [ -f "$full" ] || continue
      ch=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
      sh=$(grep "^${rel}|" "$STATE" 2>/dev/null | tail -1 | cut -d'|' -f2)
      if [ -z "$sh" ] || [ "$ch" != "$sh" ]; then echo "$rel"; fi
    done
  )
  echo "=== NLM Sync Status ==="
  echo "Son sync: $LS"
  if [ -z "$UNSYNCED" ]; then
    echo "Tüm dosyalar güncel."
  else
    COUNT=$(echo "$UNSYNCED" | grep -c .)
    echo "$COUNT dosya sync bekliyor:"
    echo "$UNSYNCED" | sed 's/^/  /'
  fi
  exit 0
fi

if [ -z "$FILE" ]; then
  echo "=== NLM Batch Sync (hash-based, parallel) ==="

  UNSYNCED=$(
    {
      find "$REPO/TASARIM" "$REPO/MEMORY" "$REPO/STAGING" -maxdepth 1 -name "*.md" 2>/dev/null | sed "s|$REPO/||"
      for f in CURRENT_STATUS.md CLAUDE.md RULES.md AGENTS.md; do
        [ -f "$REPO/$f" ] && echo "$f"
      done
    } | sort -u | while IFS= read -r rel; do
      full="$REPO/$rel"
      [ -f "$full" ] || continue
      ch=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
      sh=$(grep "^${rel}|" "$STATE" 2>/dev/null | tail -1 | cut -d'|' -f2)
      if [ -z "$sh" ] || [ "$ch" != "$sh" ]; then echo "$rel"; fi
    done
  )

  if [ -z "$UNSYNCED" ]; then
    echo "Tüm dosyalar güncel — sync gerekmez."
  else
    COUNT=$(echo "$UNSYNCED" | grep -c .)
    echo "$COUNT dosya sync edilecek..."
    echo "NLM kaynak listesi alınıyor..."
    ALL_SOURCES=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null)

    PIDS=()
    while IFS= read -r rel; do
      full="$REPO/$rel"
      [ -f "$full" ] || continue
      echo "Queuing: $rel"
      (
        base=$(basename "$full")
        old=$(echo "$ALL_SOURCES" | node -e "$NODE_FILTER" "$base")
        if [ -n "$old" ]; then
          uvx --from notebooklm-mcp-cli nlm source delete $old --confirm 2>/dev/null
          echo "  Deleted: $base"
        fi
        result=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1)
        echo "$result" | grep -E "(Added|Error|added)"
        echo "  Done: $base"
        h=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
        [ -n "$h" ] && echo "${rel}|${h}" >> "$STATE"
      ) &
      PIDS+=($!)
    done <<< "$UNSYNCED"
    wait "${PIDS[@]}"
    date '+%Y-%m-%d %H:%M' > "$REPO/.claude/nlm_last_sync.txt"
    echo "=== Done: $COUNT dosya ==="
  fi
else
  if [[ "$FILE" = /* ]]; then
    rel="${FILE#$REPO/}"
    full="$FILE"
  else
    rel="$FILE"
    full="$REPO/$FILE"
  fi
  echo "=== Single file sync: $rel ==="
  ALL_SOURCES=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null)
  base=$(basename "$full")
  old=$(echo "$ALL_SOURCES" | node -e "$NODE_FILTER" "$base")
  if [ -n "$old" ]; then
    uvx --from notebooklm-mcp-cli nlm source delete $old --confirm 2>/dev/null
    echo "  Deleted: $base"
  fi
  result=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1)
  echo "$result" | grep -E "(Added|Error|added)"
  echo "  Done: $base"
  h=$(git -C "$REPO" hash-object "$full" 2>/dev/null)
  [ -n "$h" ] && echo "${rel}|${h}" >> "$STATE"
  date '+%Y-%m-%d %H:%M' > "$REPO/.claude/nlm_last_sync.txt"
fi
```

**Usage:**
- `/nlm-sync --status` → sync bekleyen dosyaları göster (sync etmez, sadece durum)
- `/nlm-sync` → tüm unsynced dosyaları sync et (git status'tan bağımsız, hash karşılaştırmasıyla)
- `/nlm-sync CURRENT_STATUS.md` → tek dosya sync
- `/nlm-sync TASARIM/room_authoring.md` → tek dosya sync

**Not:** NLM kaynak listesi bir kez çekilir, paralel sync yapılır. Rate limit yerse hata çıkar.
