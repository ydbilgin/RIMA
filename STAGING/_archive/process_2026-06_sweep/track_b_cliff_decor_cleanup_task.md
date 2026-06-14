# Track B — Cliff Decor Overload Cleanup (PlayableArena_Test01)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Amaç
PlayableArena_Test01.unity sahnesinde **228 adet** `Parallax_cliff_cyan_glow_Near` GameObject var. Bu otomatik yerleşim sırasında üretilmiş duplicate / overdense decor. Performans + görsel gürültü kaynağı. Agent 2 layout pass'inde tespit edildi (`STAGING/room_layout_phase1_demo.md` 1.1 satır 26).

**Hedef:** 228 → ~30-50 strategic placement (kenar perimeter only).

## Önce KARAR (kullanıcı tarafı, dispatch öncesi)

1. **Yaklaşım A — Direct prune:** UnityMCP execute_code ile 228 GO'dan ~180'ini Destroy. Hangi 50'si kalır? Spatial cluster pick (kenar perimeter cell'lerine en yakın olanları tut).
2. **Yaklaşım B — Spawner refactor:** 228 manuel GO'yu sil, runtime spawner ekle (`CliffDecorSpawner.cs`) sahne play başlangıcında perimeter cell'lerden ~50 spawn.
3. **Yaklaşım C — Defer:** Şimdi dokunma, scene rebuild yaparken (Day 3+ runtime sub-room karar #149 implementation) baştan temizle.

**Default rec: A** (en az risk + en hızlı sonuç + sub-room runtime gelene kadar temiz sahne)

## Spec — Yaklaşım A (Direct prune via UnityMCP)

### Pre-conditions
- Unity Editor AÇIK, PlayableArena_Test01.unity yüklü
- UnityMCP bağlı (`set_active_instance` doğru)
- Yeni branch / commit: `feat(track-b)-cliff-decor-prune-pre` (rollback için)

### Adımlar (UnityMCP execute_code, 1 dispatch)
1. `find_gameobjects` ile `Parallax_cliff_cyan_glow_Near` listele → 228 GO bekle
2. Her birinin transform.position al → list<Vector3>
3. **Spatial filter:** floor tilemap perimeter cell'lerine ≤ 2 unit mesafede olanları KEEP. İçeride / yığılmada olanları DESTROY.
   - Tilemap bound: agent 2 raporu 78×77 cell (CliffTilemap bounds size)
   - Perimeter cell tanımı: WalkabilityMap.IsReachableFromPlayer false komşusu var olan walkable cell
4. Final keep ~30-50 GO bekle. Destroyed sayısını log
5. Scene save (`manage_editor save_scene` veya menu Ctrl+S)
6. `read_console` → 0 error

### Verification
- Scene root count: önce ~233, sonra ~50-70 (cliff + diğer LIVE entity)
- Grep scene .unity dosyasında `m_Name: Parallax_cliff_cyan_glow_Near` count ≤ 50
- Play mode test: cliff perimeter görsel olarak kalır, ortada dağılma yok, fps ≥ önceki + 5

### BLOCKED triggers
- Unity kapalı → BLOCKED: "Unity Editor not open, can't UnityMCP"
- WalkabilityMap.Instance null Play öncesi → BLOCKED: "need play mode to read walkability"

## Output (sub-agent rapor)

`STAGING/track_b_cliff_cleanup_DONE.md`:
- Before: 228 instance
- Destroyed: N
- Kept: M
- Scene save timestamp
- 0 compile error confirm

## ÖZ TASK ÖZETI

228 cliff decor overload tespit edildi. Yaklaşım A (direct prune, spatial filter ile ~50'ye indir) önerilir. Dispatch ÖNCESI kullanıcı onayı (autonomous risk: scene mutation), yaklaşım seç (A/B/C). S111 autonomous run'da dispatch ETME — kullanıcı yarın okur, karar verir, sonra dispatch.
