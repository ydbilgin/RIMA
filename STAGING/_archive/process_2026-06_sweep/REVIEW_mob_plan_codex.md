ACTIVE RULES: (1) think before answering (2) concrete with file:line (3) verify, don't assume (4) flag uncertainty.

# GÖREV — Mob üretim planı GROUND-TRUTH review (kod/asset)
Opus `STAGING/MOB_PRODUCTION_PLAN_S6.md` yazdı. Sen YAZMIYORSUN — mevcut mob-asset durumunu DOĞRULA + raporun "Mevcut durum" tablosunu düzelt.

DOĞRULA (grep + dosya):
1. **FractureImp** — prefab yolu? animasyon controller + clip'ler (idle/walk/attack/death) var mı? 8-dir mi? shard-scatter death script?
2. **ShardWalker** — prefab (ShardWalker_GB?) yolu? Hangi anim'ler VAR (idle/walk var deniyor — doğru mu?), attack/death var mı? sprite kaç yön?
3. **HollowHulk** — prefab? anim var mı (yok deniyor)? sprite durumu?
4. **PenitentSovereign** — prefab? Health component var mı (boss death→DemoComplete için kritik)? sprite VAR mı yok mu?
5. **EliteAffix** — recolor/aura için kullanılabilir mi? (W8) Hangi affix'ler runtime'da gerçekten OKUNUYOR (speed/shield bug'ı vardı)?
6. Genel: demo sahnesinde (PlayableArena_Test01 / RoomSequence SO'ları) hangi mob prefab'ları referanslı?

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED
- Düzeltilmiş "mevcut durum" tablosu (mob | prefab path | anim'ler | sprite | eksik)
- Raporun yanlış varsayımları
- Gen-tahmin gerçekçi mi (kod açısından)
- W8 elite recolor için teknik yol (mevcut shader/material/affix)
