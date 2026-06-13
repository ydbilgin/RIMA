# LINT REPORT — 2026-06-13

> RIMA Knowledge Base Health Scan (`/lint` protokolü). Kaynaklar: `MEMORY.md` (67 satır index), `CURRENT_STATUS.md`, `TASARIM/MASTER_KARAR_BELGESI.md`, `TASARIM/FAZLAR/FAZ_MASTER.md`, `TASARIM/GDD.md`, `TASARIM/_ARCHIVE_2.5D_2026-05-12/ANIMATION_REDESIGN.md`.
> NOT: Dünkü (2026-06-13) büyük triyaj baz alındı — `_archive/2026-06-13/`'e taşınanlar stale diye raporlanmadı.

---

## 🔴 Conflicts (fix immediately)

**Gerçek conflict YOK.** Tüm 5 conflict-check satırı tutarlı çıktı:

| Check | Source A | Source B | Sonuç |
|---|---|---|---|
| Class roster (10?) | `project_rima.md` (10 class implied) | `MASTER_KARAR_BELGESI` Karar #4 + `FAZ_MASTER` | ✅ TUTARLI — 10 class (Warblade/Elem/Shadow/Ranger/Ronin/Gunslinger/Ravager/Brawler/Summoner/Hexer); Crusader+Lancer KALDIRILDI; Tempest+Hemomancer post-launch DLC. Üç kaynakta da aynı. |
| Active phase | `CURRENT_STATUS` = "2026-06-13 demo günü" | `FAZ_MASTER` = Faz 1 referans | ⚠️ Çelişki değil, **stale** (aşağıda) — `project_rima.md` hâlâ "Phase 1 (S43) PixelLab fix queue" diyor. |
| Sprite system (body+weapon ayrı?) | `project_character_system.md` | `ANIMATION_REDESIGN` | ✅ TUTARLI — ikisi de Karar #144 weaponless body + Unity child WeaponSR'ye işaret ediyor. `ANIMATION_REDESIGN` zaten ARCHIVED (2026-05-13) + mob-only scope; `project_character_system.md` DEPRECATED bayraklı, doğru aktif spec'e ([[weaponless-animation-v1]]) yönlendiriyor. |
| PixelLab pipeline status | `project_pixellab_character_states_workflow.md` (project_pixellab_pipeline.md YOK) | `MASTER_KARAR_BELGESI` Karar #35/#41/#90 | ✅ ÇELİŞKİ YOK — Karar #41 CFSR/Create-from-Reference + Karar #90 batch ekonomi tutarlı. (Task'taki `project_pixellab_pipeline.md` dosyası mevcut değil — bkz. Missing.) |
| Cross-class slots (2 slot?) | `project_cross_class_skills.md` ("Max 2 cross-class slot/run") | `GDD` ("+2 aktif slot = toplam 6") | ✅ TUTARLI — iki ayrı sayı, çelişki değil: toplam 6 aktif slot; bunlardan max 2'si cross-class. GDD §7 + Karar #24 ile uyumlu. |

---

## 🟡 Stale Entries (needs update)

1. **`project_rima.md`** (44 gün eski) — `Phase: 1 (S43) — PixelLab fix queue active` + `SPRITE STATUS (S43)` + `DOC STATE (2026-04-29)`. CURRENT_STATUS demo-günü (2026-06-13) ve Director Tool / DEMO PROVA aşamasında. Bu dosya MEMORY.md index'inde **listelenmiyor** (curate dışı) ama fiziksel olarak duruyor ve sorgulandığında yanıltıcı eski faz/sprite durumu veriyor. → Güncelle veya `_archive/`'a taşı.

2. **Task adım 2 yol hatası (rapor notu)** — `/lint` task'i `TASARIM/ANIMATION_REDESIGN.md` istedi; dosya kökte YOK, `TASARIM/_ARCHIVE_2.5D_2026-05-12/ANIMATION_REDESIGN.md` altında (zaten ARCHIVED). Doküman doğru arşivlenmiş; sadece `/lint` skill'inin path referansı eski. Düşük öncelik.

3. **`project_character_system.md`** — içerik doğru (Karar #144'e yönlendiriyor) ama başlığı "DEPRECATED 2026-05-17". Aktif değeri sıfır; sadece pointer. MEMORY.md'de listelenmiyor. Konsolidasyon adayı (silinebilir, [[weaponless-animation-v1]] yeterli).

---

## 🟢 Orphans / Missing Files

**Missing (MEMORY.md'de linkli ama fiziksel dosya YOK):**
- `project_nlm_notebook_id.md` — MEMORY.md satır 61'de linkli, memory klasöründe YOK. Düşük etki: notebook ID (`30ddffa5-...`) zaten MEMORY.md içine inline yazılı. → Ya linki düz-metne çevir ya dosyayı oluştur.
- `project_pixellab_pipeline.md` ve `project_cross_class_skills.md`/`project_character_system.md` — `/lint` task'inin conflict tablosunda "MEMORY: ..." olarak gösteriliyor ama MEMORY.md bunları LİSTELEMİYOR. İkincisi (cross_class, character_system) fiziksel olarak VAR; `pixellab_pipeline` YOK (en yakını `project_pixellab_character_states_workflow.md`). Task referansı ile gerçek dosya adları drift etmiş — düşük öncelik.

**Orphan / index-dışı (fiziksel VAR ama MEMORY.md index'inde YOK):**
MEMORY.md bilinçli bir "lean pointer" index (67 satır, dünkü triyaj sonrası). Aşağıdaki ~20 `project_*` / `feedback_*` dosyası memory kökünde fiziksel duruyor ama index'te referansı yok — yani NLM/agent bunları ancak doğrudan dosya adıyla bulabilir, index'ten keşfedilemez:
- Tasarım-içerikli (potansiyel canon-değer): `project_class_balance.md`, `project_class_genders.md`, `project_combat_architecture.md`, `project_cross_class_skills.md`, `project_ghost_attack_system.md`, `project_item_matrix_decisions.md`, `project_localization.md`, `project_rift_break.md`, `project_rift_crack_architecture.md`, `project_vfx_production.md`, `project_sfx_pipeline.md`, `project_sfx_v2.md`, `project_shadow_standard.md`, `project_sim_philosophy.md`, `project_perspective_templates.md`, `project_rima_backlog.md`, `project_character_visual_identity.md`, `project_character_64px_canvas_large_for_animation.md`, `project_hud_overlay_decision.md`, `project_feel_toggles.md`, `project_rima.md`, `project_character_system.md`.
- Bu **bilinçli triyaj kalıntısı** mı yoksa index-güncellemesi unutulmuş mu — kullanıcı kararı gerekiyor. Çoğu hâlâ geçerli tasarım içeriği taşıyor (silinmemeli), ama index'ten kayıp = keşfedilemez. → Ya MEMORY.md'ye "## Tasarım canon (NLM'de)" bölümü altında toplu pointer ekle, ya bilinçli-dışlama notu yaz.

**Düzgün arşivlenmiş (orphan DEĞİL, sorun yok):** `_archive/`, `_archive_overnight_2026_05_26/` altındaki tüm dosyalar.

---

## ✅ Clean

- **Conflict checks: 5/5 temiz** — class roster, sprite system, cross-class slots, PixelLab pipeline tutarlı; phase yalnızca stale (çelişki değil).
- **MASTER_KARAR_BELGESI ↔ FAZ_MASTER senkronu** — Karar #1-#148 referansları iki dosyada da hizalı; REVOKE zincirleri (#71→#144→#146, #36→#40→#148, #49→#53, #29→#62) düzgün işaretli.
- **MEMORY.md HARD RULES + Routing bölümü** — 46/47 linkli dosya fiziksel olarak mevcut (yalnız `project_nlm_notebook_id.md` eksik).
- **Dünkü triyaj** (15 dosya `_archive/2026-06-13/` + 3 dedup) doğru uygulanmış; arşivlenenler stale olarak işaretlenmedi.
- **GDD** doğru "REFERENCE" bayraklı + Master'a öncelik notu taşıyor — desync riski açıkça yönetiliyor.

---

## Recommended Actions

1. **`project_rima.md`'yi güncelle veya arşivle** — "Phase 1 (S43)" + "DOC STATE 2026-04-29" demo-günü gerçeğiyle çelişiyor; en kolay çözüm `_archive/2026-06-13/`'e taşı (içeriği CURRENT_STATUS + Master tarafından süpersede edilmiş).
2. **MEMORY.md satır 61'i düzelt** — `project_nlm_notebook_id.md` linkini kaldır, notebook ID zaten inline; ya da boş dosyayı oluştur. (Tek gerçek "missing".)
3. **İndex-dışı ~20 tasarım memory dosyası için karar ver** — toplu olarak: (a) MEMORY.md'ye "Tasarım canon (NLM)" alt-pointer bölümü ekle, VEYA (b) bunların NLM'de kanonik olduğunu varsayıp memory klasöründen `_archive/`'a taşı + index'e tek satır "tasarım detayı = NLM" yaz. Tek tek silme — çoğu hâlâ geçerli içerik.
4. **DEPRECATED pointer dosyalarını dedup** — `project_character_system.md` (DEPRECATED, sadece [[weaponless-animation-v1]]'e yönlendiriyor) ve benzeri tek-pointer dosyaları sil; aktif spec yeterli.
5. **`/lint` skill path'lerini güncelle (düşük öncelik)** — `ANIMATION_REDESIGN.md` artık `_ARCHIVE_2.5D_2026-05-12/` altında; conflict-check'teki `project_pixellab_pipeline.md` gerçek adı `project_pixellab_character_states_workflow.md`. Skill referanslarını gerçek yollarla hizala.
