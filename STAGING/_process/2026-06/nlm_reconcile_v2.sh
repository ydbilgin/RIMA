#!/usr/bin/env bash
# NLM reconcile v2 — FIX: all temp files use absolute $REPO paths (node /tmp resolved to F:\tmp => bug).
# node only READS (argv abs path) + writes to stdout; bash writes files.
set +e
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"
TMP="$REPO/STAGING/_process/2026-06/_nlmtmp"
mkdir -p "$TMP"
RAW="$TMP/raw.json"; TITLES="$TMP/titles.txt"

echo "=== fetch NLM source list ==="
uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null > "$RAW"
node -e 'const fs=require("fs");JSON.parse(fs.readFileSync(process.argv[1],"utf8")).forEach(x=>console.log(x.title))' "$RAW" | sort -u > "$TITLES"
echo "NLM kaynak: $(grep -c . "$TITLES")"

ids_for() { node -e 'const fs=require("fs");const s=JSON.parse(fs.readFileSync(process.argv[1],"utf8"));console.log(s.filter(x=>x.title===process.argv[2]).map(x=>x.id).join(" "))' "$RAW" "$1"; }
ok_add() { ! echo "$1" | grep -qiE "error|fail|expired|denied"; }

# ===== A) delete clearly-stale orphans =====
DELLIST="CODEX_CLIFF_LIVERELOAD_FIX.md
CODEX_T3_RUNTIME_TWINS.md
Cliff_System.md
GRAPH_REPORT.md
Open_Decisions.md
Reward_Portal_Flow.md
S107_Overnight_Log.md
S108_Cleanup.md
Walkability_Dash.md
agy_pixellab_youtube.md
agy_research_direction_s6.md
agy_weapon_research.md
cx_task_vertical_slice_s6.md
project_wall_production_pipeline_s99_late.md"
echo ""; echo "=== A) stale orphan sil ==="
DEL=0
while IFS= read -r title; do
  [ -z "$title" ] && continue
  for id in $(ids_for "$title"); do
    uvx --from notebooklm-mcp-cli nlm source delete "$id" --confirm >/dev/null 2>&1 && { echo "  deleted: $title"; DEL=$((DEL+1)); }
  done
done <<< "$DELLIST"
echo "silinen: $DEL"

# ===== B) sync missing canonical (TASARIM/MEMORY/root not in NLM) =====
echo ""; echo "=== B) canonical missing sync ==="
{
  find "$REPO/TASARIM" "$REPO/MEMORY" -type d \( -name '_*' -o -name 'EXPERIMENTS' \) -prune -o -type f -name "*.md" -print 2>/dev/null
  for f in CURRENT_STATUS.md CLAUDE.md RULES.md AGENTS.md; do [ -f "$REPO/$f" ] && echo "$REPO/$f"; done
} | sort -u > "$TMP/canon_paths.txt"
ADD=0; SKIP=0
while IFS= read -r full; do
  [ -z "$full" ] && continue
  base=$(basename "$full")
  if grep -qxF "$base" "$TITLES"; then SKIP=$((SKIP+1)); continue; fi
  result=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1)
  if ok_add "$result"; then
    echo "  added: $base"; ADD=$((ADD+1))
    h=$(git -C "$REPO" hash-object "$full" 2>/dev/null); rel="${full#$REPO/}"; [ -n "$h" ] && echo "${rel}|${h}" >> "$STATE"
  else
    echo "  FAIL: $base -> $(echo "$result" | tr '\n' ' ' | tail -c 120)"
  fi
done < "$TMP/canon_paths.txt"
echo "eklenen: $ADD | zaten-var-skip: $SKIP"

# ===== C) force-resync CURRENT_STATUS (delete ALL existing copies + add fresh) =====
echo ""; echo "=== C) CURRENT_STATUS force-resync ==="
for id in $(ids_for "CURRENT_STATUS.md"); do uvx --from notebooklm-mcp-cli nlm source delete "$id" --confirm >/dev/null 2>&1 && echo "  eski kopya silindi"; done
r=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$REPO/CURRENT_STATUS.md" 2>&1); ok_add "$r" && echo "  CURRENT_STATUS taze eklendi" || echo "  CURRENT_STATUS add FAIL: $r"

date '+%Y-%m-%d %H:%M' > "$REPO/.claude/nlm_last_sync.txt"
echo ""; echo "=== DONE: $DEL silindi, $ADD eklendi, $SKIP zaten-vardi + CURRENT_STATUS resync ==="
# final count
uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null | node -e 'const fs=require("fs");let c="";process.stdin.on("data",d=>c+=d);process.stdin.on("end",()=>{const s=JSON.parse(c);const m={};s.forEach(x=>m[x.title]=(m[x.title]||0)+1);console.log("FINAL NLM kaynak:",s.length,"| duplicate:",Object.values(m).filter(n=>n>1).length)})'
