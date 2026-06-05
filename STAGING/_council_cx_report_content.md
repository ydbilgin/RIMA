ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Senior design (bitirme projesi) raporu içerik planlaması — kod tabanı ENVANTER lensi. ANALYSIS ONLY, no code changes.

# Bağlam
RIMA = Unity 6 URP 2D izometrik roguelite. Ara rapor mevcut (ARA_RAPOR_RIMA.docx, repo kökü — DOCX, okuyamazsan skip; plan dosyası yeterli). Detaylı plan: STAGING/SENIOR_DESIGN_REPORT_PLAN.md (READ THIS). Hedef: ~30-40 sayfa Türkçe akademik rapor. Oyun artık OYNANABILIR tam döngüye sahip: MainMenu→Attunement Chamber (yürünebilir karakter seçimi, E-bürünme)→_Arena run (RoomRunDirector state machine: combat→clear→reward→branching doors→boss→victory/death→Shattered Echo award).

# Görev — repo'dan SOMUT envanter çıkar (rapor bunlara atıf yapacak)
Aşağıdaki 5 alanda "raporda ne iddia edebiliriz, kanıtı kod tabanında nerede" envanteri (file:line + sayılar):
1. Oynanabilir döngü: RoomRunDirector/EncounterController/RewardPickup/DraftManager/DungeonGraph zinciri — state machine adımları, kaç sahne, build settings.
2. Data-driven oda sistemi: RoomTemplateSO + IsoRoomBuilder (cliff auto-placement kuralları) + kaç oda template (Assets/Data/Rooms say) + RoomJsonImporter (JSON şeması) + Room Browser + Unified Map Designer (kaç sekme/özellik) + BridsonPoissonAutoPlacer/CompositionRoleMap (prop auto-placement algoritması — akademik değeri var: Poisson-disk sampling).
3. Combat/feel: knockdown paketi (HitImpulse/KnockdownProfile/KnockdownDriver), skill draft sistemi (SkillOfferGenerator tier weights), TooltipSystem, SkillCodexUI, ChainWindowTracker (synergy), kaç sınıf/skill (SkillDatabase).
4. Test/QC altyapısı: Assets/Tests altında kaç EditMode/PlayMode test (yaklaşık say), test kategorileri, son görsel oda-QC süreci (STAGING/ROOM_QC_REPORT_2026-06-05.md varsa oku).
5. Asset pipeline: PixelLab karakter üretimi (10 sınıf × 8 yön, 120px canvas), imagegen env asset'leri, pixel-art kuralları (PPU 64, Point filter).

# Çıktı (CODEX_DONE.md'ye)
Her alan için: rapor-iddiası (1 cümle) + kanıt (file:line / sayı) + rapora girecek somut metrik tablosu önerisi. Sonunda: mevcut SENIOR_DESIGN_REPORT_PLAN.md'de OLMAYAN ama bugünkü durumda eklenebilir 5-8 yeni bölüm/alt-bölüm önerisi (öncelik sıralı). Prior audit'leri REPRODUCE ETME.
