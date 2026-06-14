# CURRENT_STATUS

## ⏯️ RESUME (2026-06-14 — sunum ~20 Haz; OTONOM DEVAM)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=Claude Opus sub-agent · review=auditor-opus/cx (writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash · E1-E8. **HARD: aynı anda TEK Unity-süren ajan** (eşzamanlı MCP = python köprü çökmesi); paralel=read-only/filesystem. cx profil: yasinderyabilgin→yekta(son).

**🔒 STATE DİSİPLİNİ:** Play=full-flow (`playModeStartScene=MainMenu`, dokunma/null bırakma). Commit öncesi pollution temizle. Play mode'u iş bitince STOP'la.

**✅ COMMIT BATCH TAMAM (2026-06-14, auditor-opus PASS):** Facing fix #1 (4dir→`SnapToEight` 8-yön, flipX once-in-Awake, `[RequireComponent]` kaldırıldı, `PlayerClassManager` animator-driving gate, `Player.prefab` Animator@Body) · Test01 arşivi (→`_Archive` + EditorBuildSettings girdisi silindi) · BuildMode leak fix #10. Pollution gitignore'landı (zips/_inbox/_outbox/compile-log/orphan png.meta). **PUSH BEKLİYOR** (origin master geride; kullanıcı onayı bekliyor).

**🟡 TEK AÇIK VERIFY:** Leak fix #10 edit-mode runtime verify (build mode aç→sahne kapa→"objects not cleaned up" uyarısı yok) — derlendi + auditor PASS, ama Unity'de canlı tekrar test edilmedi; conservative hideFlags guard, benign.

**📋 BACKLOG (sıra):**
1. **PUSH** (kullanıcı onayı) — sonra:
2. **F2 KARARI bekliyor:** F2 & `"` ikisi de Build Mode toggle; `DirectorMode.Instance=null`→normal play'de inert. Karar: (a)dev-gated kalsın (b)editor-build'de hep çalışsın (c)başka
3. Silah tuning #2 (polish) · boş `WeaponSprite` placeholder temizle
4. Mavi-arc #3 · Tooltip #4 · Skill akışı #5 — teşhisler `STAGING/_process/2026-06/_diag_tooltip_skillflow_2026-06-13.md` (2.skill bug=`AssignActive` skillType==null sessiz yutuyor + `ChestBehavior` GetAll() filtrelenmemiş)
5. Verified screenshot seti #6 — **scene_view capture KULLAN**
6. Tool UI/UX #7 · Hoca raporu docx #8 (EN SON)

---
*Önceki bloklar git history'de. F2 kararı = sıradaki iş.*
