#!/usr/bin/env bash
# Standalone NLM batch sync (extracted from /nlm-sync batch mode). Run verbatim.
set +e
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"

NODE_FILTER='
var c="",t=process.argv[1];
process.stdin.setEncoding("utf8");
process.stdin.on("data",function(d){c+=d;});
process.stdin.on("end",function(){
  try{var s=JSON.parse(c);var ids=s.filter(function(x){return x.title===t;}).map(function(x){return x.id;});if(ids.length)console.log(ids.join(" "));}catch(e){}
});'

list_orphans() {
  [ -f "$STATE" ] || return 0
  cut -d'|' -f1 "$STATE" | sort -u | while IFS= read -r rel; do
    [ -z "$rel" ] && continue
    full="$REPO/$rel"
    [ -f "$full" ] || echo "$rel"
  done
}

echo "=== NLM Batch Sync (hash-based, parallel) ==="
UNSYNCED=$(
  {
    find "$REPO/TASARIM" "$REPO/MEMORY" "$REPO/STAGING" -type d \( -name '_*' -o -name 'EXPERIMENTS' -o -name '.git' \) -prune -o -type f -name "*.md" -print 2>/dev/null | sed "s|$REPO/||"
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
  echo "Tum dosyalar guncel — sync gerekmez."
else
  COUNT=$(echo "$UNSYNCED" | grep -c .)
  echo "$COUNT dosya sync edilecek..."
  echo "NLM kaynak listesi aliniyor..."
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

ORPHANS=$(list_orphans)
if [ -n "$ORPHANS" ]; then
  OCOUNT=$(echo "$ORPHANS" | grep -c .)
  echo ""
  echo "WARN: $OCOUNT orphan kaynak tespit edildi (state'te var, local'de yok)."
  echo "-> /nlm-sync --cleanup-dry ile listele, /nlm-sync --cleanup ile sil"
fi
