#!/usr/bin/env bash
# NLM structure fix: delete 78 confirmed-junk orphans + add 8 canonical stragglers.
set +e
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"
P="$REPO/STAGING/_process/2026-06"
RAW="$P/_nlmaudit_raw.json"
ORPH="$P/_nlm_orphans_now.txt"

ids_for() { node -e 'const fs=require("fs");const s=JSON.parse(fs.readFileSync(process.argv[1],"utf8"));console.log(s.filter(x=>x.title===process.argv[2]).map(x=>x.id).join(" "))' "$RAW" "$1"; }
ok_add() { ! echo "$1" | grep -qiE "error|fail|expired|denied|could not"; }

echo "=== A) 78 junk orphan sil ==="
DEL=0
while IFS= read -r title; do
  [ -z "$title" ] && continue
  for id in $(ids_for "$title"); do
    uvx --from notebooklm-mcp-cli nlm source delete "$id" --confirm >/dev/null 2>&1 && DEL=$((DEL+1))
  done
done < "$ORPH"
echo "silinen: $DEL"

echo ""; echo "=== B) 8 canonical straggler ekle ==="
STRAG="PIXELLAB_MASTER_REHBER.md SKILL_SYSTEM_v2.md SUBROOM_TEMPLATES_ACT1.md chibi_lore_integration_decision_2026-05-13.md Warblade_AnimationSpec.md mob_boss_skill_expansion_2026-05-13.md open_vista_decision_2026-05-13.md room_designer_topdown_enhancement_2026-05-13.md"
ADD=0
for b in $STRAG; do
  full=$(find "$REPO/TASARIM" "$REPO/MEMORY" -type d \( -name '_*' -o -name 'EXPERIMENTS' \) -prune -o -type f -name "$b" -print 2>/dev/null | head -1)
  [ -z "$full" ] && { echo "  PATH-YOK: $b"; continue; }
  r=$(uvx --from notebooklm-mcp-cli nlm source add $NB --file "$full" 2>&1)
  if ok_add "$r"; then echo "  added: $b"; ADD=$((ADD+1)); h=$(git -C "$REPO" hash-object "$full" 2>/dev/null); rel="${full#$REPO/}"; [ -n "$h" ] && echo "${rel}|${h}" >> "$STATE"; else echo "  FAIL: $b"; fi
  sleep 2
done
echo "eklenen: $ADD"

date '+%Y-%m-%d %H:%M' > "$REPO/.claude/nlm_last_sync.txt"
echo ""; echo "=== FINAL ==="
uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null | node -e 'let c="";process.stdin.on("data",d=>c+=d);process.stdin.on("end",()=>{const s=JSON.parse(c);const m={};s.forEach(x=>m[x.title]=(m[x.title]||0)+1);console.log("NLM kaynak:",s.length,"| duplicate:",Object.values(m).filter(n=>n>1).length)})'
echo "=== DONE: $DEL silindi, $ADD eklendi ==="