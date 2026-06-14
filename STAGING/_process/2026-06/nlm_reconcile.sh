#!/usr/bin/env bash
# NLM reconcile: delete clearly-stale orphans + sync missing canonical (TASARIM/MEMORY/root) + resync CURRENT_STATUS.
set +e
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"

echo "=== fetch NLM source list ==="
uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null > /tmp/nlm_raw.json
node -e 'const s=JSON.parse(require("fs").readFileSync("/tmp/nlm_raw.json","utf8"));const t=new Set();const m={};s.forEach(x=>{t.add(x.title);(m[x.title]=m[x.title]||[]).push(x.id);});require("fs").writeFileSync("/tmp/nlm_titles.txt",[...t].sort().join("\n")+"\n");require("fs").writeFileSync("/tmp/nlm_titleid.json",JSON.stringify(m));'
echo "NLM kaynak: $(grep -c . /tmp/nlm_titles.txt)"

# ===== A) DELETE clearly-stale orphans (explicit list — guide-looking sources KEPT) =====
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
echo ""
echo "=== A) stale orphan sil ==="
DEL=0
while IFS= read -r title; do
  [ -z "$title" ] && continue
  ids=$(node -e 'const m=JSON.parse(require("fs").readFileSync("/tmp/nlm_titleid.json"));console.log((m[process.argv[1]]||[]).join(" "))' "$title")
  for id in $ids; do
    uvx --from notebooklm-mcp-cli nlm source delete "$id" --confirm >/dev/null 2>&1 && { echo "  deleted: $title"; DEL=$((DEL+1)); }
  done
done <<< "$DELLIST"
echo "silinen: $DEL"

# ===== B) SYNC missing canonical (TASARIM/MEMORY/root not already in NLM) =====
echo ""
echo "=== B) canonical missing sync ==="
{
  find "$REPO/TASARIM" "$REPO/MEMORY" -type d \( -name '_*' -o -name 'EXPERIMENTS' \) -prune -o -type f -name "*.md" -print 2>/dev/null
  for f in CURRENT_STATUS.md CLAUDE.md RULES.md AGENTS.md; do [ -f "$REPO/$f" ] && echo "$REPO/$f"; done
} | sort -u > /tmp/canon_paths.txt
ADD=0
while IFS= read -r full; do
  [ -z "$full" ] && continue
  base=$(basename "$full")
  grep -qxF "$base" /tmp/nlm_titles.txt && continue   # already in NLM, skip
  result=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1)
  if echo "$result" | grep -qiE "added"; then
    echo "  added: $base"; ADD=$((ADD+1))
    h=$(git -C "$REPO" hash-object "$full" 2>/dev/null); rel="${full#$REPO/}"
    [ -n "$h" ] && echo "${rel}|${h}" >> "$STATE"
  else
    echo "  FAIL: $base -> $(echo "$result" | head -1)"
  fi
done < /tmp/canon_paths.txt
echo "eklenen: $ADD"

# ===== C) force-resync CURRENT_STATUS (demo-critical, often stale) =====
echo ""
echo "=== C) CURRENT_STATUS force-resync ==="
ids=$(node -e 'const m=JSON.parse(require("fs").readFileSync("/tmp/nlm_titleid.json"));console.log((m["CURRENT_STATUS.md"]||[]).join(" "))')
for id in $ids; do uvx --from notebooklm-mcp-cli nlm source delete "$id" --confirm >/dev/null 2>&1 && echo "  eski CURRENT_STATUS silindi"; done
uvx --from notebooklm-mcp-cli nlm source add $NB --file "$REPO/CURRENT_STATUS.md" 2>&1 | grep -qiE "added" && echo "  CURRENT_STATUS yeniden eklendi"

date '+%Y-%m-%d %H:%M' > "$REPO/.claude/nlm_last_sync.txt"
echo ""
echo "=== DONE: $DEL silindi, $ADD canonical eklendi + CURRENT_STATUS resync ==="
echo "KORUNAN (guide-suphesi, elle teyit gerek): PIXELLAB_API_DOCS_2026-06-10.txt, PIXELLAB_PRODUCTION_GUIDE_v2.md, PRODUCTION_PLAYBOOK.md, USER_WORKFLOW_GUIDE.md, WEAPON_PRODUCTION_GUIDE.md"
