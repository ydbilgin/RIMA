# TASK: NLM Auth + Sync + Graphify Setup + Obsidian Path

ACTIVE RULES: (1) think before doing (2) min surgical (3) MOVE not DELETE (4) BLOCKED if user input gerekli.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: 3 görev — NLM auth fix + sync retry, Graphify focused scope build, Obsidian gerçek vault path tespit. Hepsini autonomous + temiz şekilde.

## 1. NLM Auth + Sync Retry

### Step A: Auth check
```bash
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "test query for auth check"
```
- Eğer "auth expired" hatası verirse: **BLOCKED** — user manuel `nlm login` çalıştırmalı (Chrome açılır)
- Eğer çalışırsa: devam et

### Step B: Cleanup orphan (193 dosya state'te kalmış)
```bash
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"
STATE="$REPO/.claude/nlm_sync_state.txt"
# Orphan list (state'te var ama local'de yok)
[ -f "$STATE" ] && cut -d'|' -f1 "$STATE" | sort -u | while IFS= read -r rel; do
  [ -z "$rel" ] && continue
  [ -f "$REPO/$rel" ] || echo "ORPHAN: $rel"
done
```
- Output kaydet, sayım raporla
- Cleanup: orphan'ları NLM'den sil + state'ten temizle (eğer auth varsa)

### Step C: Sync retry (sadece unsynced)
```bash
# /nlm-sync skill çalıştır (orchestrator'ın yaptığı gibi)
# Output paralel queue, ~206 dosya, hata varsa raporla
```
- Başarı sayısı / hata sayısı raporla

## 2. Graphify Focused Scope

### Step A: Karar
Tüm RIMA corpus 27M kelime — token budget riskli. Focused scope:
- **Option 1**: `STAGING/s107_obsidian_notes/` (6 note, S107 close summary) + `CURRENT_STATUS.md` + `.claude/PROJECT_RULES.md` — sadece S107 decision state, ~20 dosya, manageable
- **Option 2**: Yeni `MEMORY/` (proje root) sadece — 64 file, design context

### Step B: Tercih + execute
Option 1 yap (Sonnet tercih, küçük focused scope):
```powershell
# 1. Klasör hazırla — sadece focused scope'taki dosyaları kopyala temp dir'e
mkdir -p .graphify_scope
cp STAGING/s107_obsidian_notes/*.md .graphify_scope/ 2>/dev/null
cp CURRENT_STATUS.md .graphify_scope/ 2>/dev/null
cp .claude/PROJECT_RULES.md .graphify_scope/ 2>/dev/null

# 2. graphify detect + extract on scope
python -c "import graphify" 2>$null
if ($LASTEXITCODE -ne 0) { pip install graphifyy -q }
# graphify çalıştır — pipeline tam adım
```

### Step C: Output verify
- `graphify-out/GRAPH_REPORT.md` oluştu mu
- `graphify-out/graph.html` oluştu mu
- `graphify-out/graph.json` oluştu mu

## 3. Obsidian Path Tespit

### Step A: Default lokasyon check
```bash
# Olası Obsidian vault path'leri:
test -d "C:/Users/ydbil/Documents/Obsidian" && echo "FOUND: Documents/Obsidian"
test -d "C:/Users/ydbil/Obsidian" && echo "FOUND: ~/Obsidian"
test -d "C:/Users/ydbil/OneDrive/Documents/Obsidian" && echo "FOUND: OneDrive Obsidian"
test -d "F:/RIMA_Obsidian" && echo "FOUND: F:/RIMA_Obsidian"
test -d "F:/Obsidian" && echo "FOUND: F:/Obsidian"
# user'ın Documents klasörünü tara
ls "C:/Users/ydbil/Documents/" 2>/dev/null | grep -i obsidian
```

### Step B: Bulursa taşı
- `STAGING/s107_obsidian_notes/` içindeki 6 note → bulunan path/RIMA/ alt klasörüne taşı
- Eğer bulamazsan: BLOCKED, user'a sor + STAGING'de bırak

## Hard Constraints
- **MOVE not DELETE**
- NLM auth user input gerektirirse BLOCKED
- Graphify scope SADECE focused (büyük corpus YASAK)
- Obsidian path tespit edilemezse BLOCKED raporla
- Commit YAPMA, git YAPMA

## Inline rapor (<600 kelime)
- NLM: auth durumu, orphan sayısı, sync retry sonucu
- Graphify: scope kararı, çıktılar (path + dosya boyutları)
- Obsidian: path tespit edildi mi, taşıma yapıldı mı
- BLOCKED varsa neden + user aksiyon önerisi
