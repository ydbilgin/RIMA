---
description: Dosyaları NotebookLM'e kaynak olarak ekle/güncelle. RECURSIVE — TASARIM/MEMORY/STAGING tüm alt klasörler taranır (`_*` ve `EXPERIMENTS` hariç). /nlm-sync → batch sync. /nlm-sync --status → bekleyenler. /nlm-sync --cleanup-dry → orphan listele. /nlm-sync --cleanup → orphan sil. /nlm-sync path/to/file.md → tek dosya.
allowed-tools: Bash
---

## CONFIG (proje-başına düzenle)
- `NLM_NOTEBOOK_ID`: [Notebook ID] (env `$env:NLM_NOTEBOOK_ID` ya da satır-içi değişken, default: RIMA notebook id = 30ddffa5-292f-4248-8e77-68074af901be)
- `NLM_REPO`: [Çalışılan proje kökü] (env `$env:NLM_REPO` ya da satır-içi değişken, default: git rev-parse ile tespit edilen kök veya RIMA dizini "F:/Antigravity Projeler/2d roguelite/RIMA")

# /nlm-sync — NotebookLM Source Sync (hash-based, parallel + orphan cleanup)

Run the command below and return its output verbatim. Do not add commentary.

```bash
NB="${NLM_NOTEBOOK_ID:-30ddffa5-292f-4248-8e77-68074af901be}"
FILE="$ARGUMENTS"
REPO="${NLM_REPO:-$(git rev-parse --show-toplevel 2>/dev/null || echo 'F:/Antigravity Projeler/2d roguelite/RIMA')}"
STATE="$REPO/.claude/nlm_sync_state.txt"

NODE_FILTER='
var c="",t=process.argv[1];
process.stdin.setEncoding("utf8");
process.stdin.on("data",function(d){c+=d;});
process.stdin.on("end",function(){
  try{var s=JSON.parse(c);var ids=s.filter(function(x){return x.title===t;}).map(function(x){return x.id;});if(ids.length)console.log(ids.join(" "));}catch(e){}
});'

# === Helper: list orphans (state'te izi olan ama local'de yok olan dosyalar) ===
list_orphans() {
  [ -f "$STATE" ] || return 0
  cut -d'|' -f1 "$STATE" | sort -u | while IFS= read -r rel; do
    [ -z "$rel" ] && continue
    full="$REPO/$rel"
    [ -f "$full" ] || echo "$rel"
  done
}

# === Mode: --status ===
if [ "$FILE" = "--status" ]; then
  LS=$(cat "$REPO/.claude/nlm_last_sync.txt" 2>/dev/null || echo "hiç sync edilmedi")
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
  ORPHANS=$(list_orphans)
  echo "=== NLM Sync Status ==="
  echo "Son sync: $LS"
  if [ -z "$UNSYNCED" ]; then
    echo "Tüm dosyalar güncel."
  else
    COUNT=$(echo "$UNSYNCED" | grep -c .)
    echo "$COUNT dosya sync bekliyor:"
    echo "$UNSYNCED" | sed 's/^/  /'
  fi
  if [ -n "$ORPHANS" ]; then
    OCOUNT=$(echo "$ORPHANS" | grep -c .)
    echo ""
    echo "⚠️  $OCOUNT orphan kaynak NLM'de duruyor (local'de yok):"
    echo "$ORPHANS" | sed 's/^/  /'
    echo "→ /nlm-sync --cleanup-dry ile incele, /nlm-sync --cleanup ile sil"
  fi
  exit 0
fi

# === Mode: --cleanup-dry ===
if [ "$FILE" = "--cleanup-dry" ]; then
  ORPHANS=$(list_orphans)
  if [ -z "$ORPHANS" ]; then
    echo "=== Orphan check ==="
    echo "Orphan yok — state ile local senkron."
    exit 0
  fi
  OCOUNT=$(echo "$ORPHANS" | grep -c .)
  echo "=== Orphan Cleanup (DRY RUN — silmez) ==="
  echo "$OCOUNT dosya state'te var ama local'de yok:"
  echo "$ORPHANS" | while IFS= read -r rel; do
    base=$(basename "$rel")
    echo "  $rel  →  NLM title: $base"
  done
  echo ""
  echo "Silmek için: /nlm-sync --cleanup"
  echo "(Sadece state'te izi olan dosyalar silinir — manuel yüklenmiş kaynaklar dokunulmaz.)"
  exit 0
fi

# === Mode: --cleanup ===
if [ "$FILE" = "--cleanup" ]; then
  ORPHANS=$(list_orphans)
  if [ -z "$ORPHANS" ]; then
    echo "=== Orphan Cleanup ==="
    echo "Orphan yok — temizlik gerekmez."
    exit 0
  fi
  OCOUNT=$(echo "$ORPHANS" | grep -c .)
  echo "=== Orphan Cleanup ==="
  echo "$OCOUNT orphan siliniyor..."
  echo "NLM kaynak listesi alınıyor..."
  ALL_SOURCES=$(uvx --from notebooklm-mcp-cli nlm source list $NB 2>/dev/null)

  DELETED=0
  NOT_FOUND=0
  echo "$ORPHANS" | while IFS= read -r rel; do
    [ -z "$rel" ] && continue
    base=$(basename "$rel")
    old=$(echo "$ALL_SOURCES" | node -e "$NODE_FILTER" "$base")
    if [ -n "$old" ]; then
      uvx --from notebooklm-mcp-cli nlm source delete $old --confirm 2>/dev/null
      echo "  Deleted NLM source: $base ($rel)"
    else
      echo "  NLM'de bulunamadı (zaten silinmiş?): $base ($rel)"
    fi
  done

  # State dosyasından orphan satırlarını çıkar
  if [ -f "$STATE" ]; then
    TMP=$(mktemp)
    while IFS= read -r line; do
      [ -z "$line" ] && continue
      rel=$(echo "$line" | cut -d'|' -f1)
      full="$REPO/$rel"
      if [ -f "$full" ]; then
        echo "$line" >> "$TMP"
      fi
    done < "$STATE"
    mv "$TMP" "$STATE"
    echo "  State dosyası temizlendi."
  fi
  echo "=== Cleanup tamamlandı ==="
  exit 0
fi

# === Mode: batch sync (no args) ===
if [ -z "$FILE" ]; then
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

  # Batch sonrası otomatik orphan uyarısı (silmez, sadece bildirir)
  ORPHANS=$(list_orphans)
  if [ -n "$ORPHANS" ]; then
    OCOUNT=$(echo "$ORPHANS" | grep -c .)
    echo ""
    echo "⚠️  $OCOUNT orphan kaynak tespit edildi (state'te var, local'de yok)."
    echo "→ /nlm-sync --cleanup-dry ile listele, /nlm-sync --cleanup ile sil"
  fi
else
  # === Mode: single file sync ===
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
- `/nlm-sync --status` → sync bekleyen + orphan dosyaları göster (sync etmez)
- `/nlm-sync` → unsynced dosyaları sync et + orphan uyarısı (silmez)
- `/nlm-sync --cleanup-dry` → orphan'ları listele (silmez)
- `/nlm-sync --cleanup` → orphan'ları NLM'den ve state'ten sil
- `/nlm-sync CURRENT_STATUS.md` → tek dosya sync
- `/nlm-sync TASARIM/room_authoring.md` → tek dosya sync

**Güvenlik:** `--cleanup` sadece **`nlm_sync_state.txt`'te izi olan ve artık local'de yok olan** dosyaları siler. NLM'e elle yüklediğin (GUIDES/, PDF, vb.) kaynaklara dokunmaz.

**Not:** NLM kaynak listesi bir kez çekilir, paralel sync yapılır. Rate limit yerse hata çıkar.
