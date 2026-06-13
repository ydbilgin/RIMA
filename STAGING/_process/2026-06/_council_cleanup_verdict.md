# COUNCIL REVIEW: RIMA memory/yapı temizlik planının risk denetimi

**Genel Karar:** KOŞULLU ONAY

**Gerekçe:** Plan genel hatlarıyla çok tutarlı ve `_archive` klasörü ile "stale" dosyaları güvenli bir şekilde izole ediyor. Ancak "SİL" listesindeki (Bölüm 1.B1) dosyaların birçoğu yoğun teknik pipeline detayları ve tarihsel context içeriyor; bunları silmek `PROJECT_RULES`'daki "asla silinmez, arşive taşınır" (Iteration Cleanup) konservatif ilkesiyle doğrudan çelişiyor. Çakışma çözümlerinde (Bölüm 1.C) tek-kaynak (Repo) kuralı doğru uygulanmış olsa da, B1 ve B3 bölümlerinde düzeltilmesi gereken somut eksiklikler ve riskler barındırıyor.

**Somut Düzeltmeler (Madde Madde):**
1. `project_core_wall_system_v2_lock_2026_05_24.md`: **SİL → ARŞİV olmalı.** Duvar sisteminin (v2) nasıl çalıştığına dair devasa bir spec ve pipeline içeriyor. Supersede edilmiş olsa bile tarihsel/teknik kanıt değeri çok yüksek.
2. `project_brush_tool_v1.md` ve `project_brush_v1_manual_composition_system.md`: **SİL → ARŞİV olmalı.** V1 brush sisteminin mimarisini ve çok değerli "lessons learned" (örn. Scene view vs Game view yanılgısı, alpha bug teşhisleri) kısımlarını içeriyor.
3. `project_fakeiso_term_revoked_2026_05_22.md`, `project_3kit_bg_architecture_lock.md`, `project_canonical_character_roster_lock.md`, `project_high_top_down_3_4_lock_2026_05_24.md`, `project_diamond_iso_tilemap_lock_2026_05_24.md`, `project_cliff_pivot_manual_brush_2026_05_26.md`, `project_multilayer_painter_v1_lock.md`, `project_modular_pipeline_lock.md`: **SİL → ARŞİV olmalı.** Bu dosyaların "revoked" olması çöp oldukları anlamına gelmez, projenin neden o yönden vazgeçtiğinin veya o günkü pipeline'ın kanıtıdır. B1 listesindeki tüm proje dosyaları SİL kategorisinden çıkarılıp ARŞİV'e taşınmalı.
4. `feedback_layered_terrain_mandatory.md` (KULLANICIYA SOR listesinde): **SİL DEĞİL, INDEX-EKLE (veya Index'te koru) olmalı.** Raporda "revoked" olarak fişlenmiş ama dosyanın kendisi "PlayableRoom 8-layer painted top-down zorunlu" diyen, oldukça güncel ve kritik bir mimari standart belgesi. Asla silinmemeli.
5. Repo ↔ User çakışmalarındaki `project_wall_production_pipeline_s99_late.md` (Bölüm 1.C): Raporda "İKİSİ DE SİL/ARŞİV" denmiş. Doğrusu: **User-memory'dekini SİL (deduplikasyon için), Repo'dakini ARŞİVLE (tarihsel kayıt için).** İki kopyasını birden silmek kural ihlalidir.

**INDEX-EKLE'ye Eklenecek Ek Adaylar (B3 Eksikleri):**
Planın B3 listesi eksik. Aşağıdaki aktif davranış kuralları memory klasöründe bulunmasına rağmen rapordaki INDEX-EKLE listesinde unutulmuş. Bunlar INDEX'e eklenmelidir:
- `feedback_queue_decide_order_dont_ask_each.md` (Orkestratör/Opus için çok kritik bir "çoklu task'i sıraya al ve bana sormadan tek tek yap" flow kuralı)
- `feedback_fable_diagnose_agents_execute.md` (Fable'ın kullanım amacını ve sınırlarını belirleyen kural)
- `feedback_autonomous_no_block.md` (queue rule dosyasında referans verilen, yine akışı ilgilendiren aktif bir kural)

**Sonuç:** Belirtilen "SİL → ARŞİV" değişiklikleri yapıldıktan ve B3 INDEX-EKLE takviyeleri rapora yansıtıldıktan sonra, plan **uygulamaya geçilebilir.**
