#!/usr/bin/env bash
# NLM orphan cleanup (mirrors /nlm-sync --cleanup). Arg "dry" = list only, no delete.
# Only deletes NLM sources whose local file is gone AND that are tracked in state.
set +e
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"
MODE="$1"

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
    [ -f "$REPO/$rel" ] || echo "$rel"
  done
}

ORPHANS=$(list_orphans)
if [ -z "$ORPHANS" ]; then echo "Orphan yok — temizlik gerekmez."; exit 0; fi
OCOUNT=$(echo "$ORPHANS" | grep -c .)

if [ "$MODE" = "dry" ]; then
  echo "=== DRY RUN: $OCOUNT orphan (state'te var, local'de yok) ==="
  echo "--- ilk 30 ---"
  echo "$ORPHANS" | head -30
  echo "--- SANITY 1: aktif kok dosya yanlislikla listede mi? ---"
  echo "$ORPHANS" | grep -iE "^(CURRENT_STATUS|CLAUDE|RULES|AGENTS)\.md$" && echo "!!! UYARI: aktif kok dosya orphan listesinde !!!" || echo "  (aktif kok dosya yok - iyi)"
  echo "--- SANITY 2: orphan basename'i AKTIF bir local dosyayla cakisiyor mu? (title-match collision riski) ---"
  COLL=0
  echo "$ORPHANS" | while IFS= read -r rel; do
    base=$(basename "$rel")
    # current local files with same basename in LIVE sync scope (prune _* / EXPERIMENTS like nlm-sync does)
    hit=$(find "$REPO/TASARIM" "$REPO/MEMORY" "$REPO/STAGING" -type d \( -name '_*' -o -name 'EXPERIMENTS' \) -prune -o -type f -name "$base" -print 2>/dev/null | head -1)
    [ -n "$hit" ] && echo "  COLLISION: orphan '$rel' ile CANLI '$hit' ayni basename"
  done | head -20
  echo "--- dagilim (ust klasore gore) ---"
  echo "$ORPHANS" | sed 's#/[^/]*$##' | sort | uniq -c | sort -rn | head -15
  exit 0
fi

echo "=== NLM Cleanup: $OCOUNT orphan ==="
echo "NLM kaynak listesi aliniyor..."
ALL_SOURCES=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null)

# resolve source ids (local string match, fast) — skip collisions with active files
IDS_FILE=$(mktemp)
echo "$ORPHANS" | while IFS= read -r rel; do
  [ -z "$rel" ] && continue
  base=$(basename "$rel")
  # safety: if a LIVE (non-_*) local file shares this basename, SKIP (don't delete shared title)
  if find "$REPO/TASARIM" "$REPO/MEMORY" "$REPO/STAGING" -type d \( -name '_*' -o -name 'EXPERIMENTS' \) -prune -o -type f -name "$base" -print 2>/dev/null | grep -q .; then
    echo "SKIP-COLLISION $rel" >&2
    continue
  fi
  echo "$ALL_SOURCES" | node -e "$NODE_FILTER" "$base"
done 2> "${IDS_FILE}.skips" | tr ' ' '\n' | sort -u | grep . > "$IDS_FILE"

IDCOUNT=$(grep -c . "$IDS_FILE")
SKIPCOUNT=$(grep -c . "${IDS_FILE}.skips" 2>/dev/null || echo 0)
echo "Silinecek source id: $IDCOUNT  |  collision-skip: $SKIPCOUNT"

# parallel delete (cap 8) — only the network part
cat "$IDS_FILE" | xargs -P 8 -I {} sh -c 'uvx --from notebooklm-mcp-cli nlm source delete "$1" --confirm >/dev/null 2>&1 && echo "  deleted $1"' _ {}

# prune state of orphan lines (keep only lines whose local file still exists)
if [ -f "$STATE" ]; then
  TMP=$(mktemp)
  while IFS= read -r line; do
    [ -z "$line" ] && continue
    rel=$(echo "$line" | cut -d'|' -f1)
    [ -f "$REPO/$rel" ] && echo "$line" >> "$TMP"
  done < "$STATE"
  mv "$TMP" "$STATE"
  echo "State temizlendi."
fi
echo "=== Cleanup tamam: $IDCOUNT source silindi, $SKIPCOUNT collision-skip ==="
