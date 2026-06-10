# DEMO FINALIZE — Council Sentez Kararı (2026-06-10)

**Danışmanlar:** cx/Codex (yekta) · ax Gemini 3.1 Pro High · ax Gemini 3.5 Flash High · ax Opus 4.6 (timeout — kısmi).
**Orchestrator kararı (Opus 4.8):** aşağıda. Çelişkiler grep ile doğrulandı.

## A) KESİN TEŞHİS

### 1. Skill bar boş (4 danışman HEMFİKİR + cx derinleştirdi)
- **Kök:** `DraftManager.ClassKits` sadece Warblade + Elementalist. Kit'siz sınıf (Shadowblade/Ranger/Ronin) → açılış draft'ı yok → boş bar. Timeout fallback (`ForcePickFirstOpeningKitSkill`) kit'siz sınıfta sessizce `return` ediyor.
- **cx ek:** `AssignActive` non-Elementalist skill'leri `Warblade_SkillController`'a atıyor AMA `SkillBarUI` Ranger/Ronin/Shadowblade controller'ını okuyor → **atama-yolu ≠ okuma-yolu** uyumsuzluğu. Boss-secondary (`ClassSelectionUI`) unlock-check'siz Ranger verebiliyor.
- **KARAR (cx+3.5 Flash consensus, en düşük risk):** Demo'yu **Warblade + Elementalist'e KISITLA** (`CharacterSelectController.GetDefaultClasses()` + chamber). Kit+controller-routing'i tüm sınıflara eklemek sunum öncesi aşırı riskli.

### 2. Mob siyah/görünmüyor (ÇELİŞKİ çözüldü)
- **3.1 Pro TERSİNE söyledi** (a97c'ye çevir dedi — yanlış). **3.5 Flash + grep doğru:** `a97c105638bdf8b4a8650670310a4cd3` = `Sprite-Lit-Default` (URP LIT, VAR). Arena'da 2D ışık yoksa LIT materyalli prefablar **siyah** render olur.
- **Siyah suspect'ler:** HollowMite, TheWound, PenitentSovereign(boss), SeamCrawler_Elite, VoidThrall_Elite (lit). Çalışanlar (FractureImp/Penitent/HalfThrall) `Sprites-Default` (unlit).
- **cx ek (ayrı kök):** Aktif dalga (`Act1_Wave_Pilot`) sadece **3 mob** spawn ediyor (FractureImp/Penitent/HalfThrall); diğer 9 wire'lı mob HİÇ spawn olmuyor → "görünmüyor" çünkü çağrılmıyor. `Projectile.prefab` + legacy `BossAI_PenitentSovereign` null-sprite (oda-spawn değil, ayrı).
- **KARAR:** (a) 5 lit-materyal prefabı **unlit `Sprites-Default`'a çevir** (siyah biter); (b) çeşitlilik için encounter bank'i genişlet VEYA 3-mob demo setini kabul et (lean).

### 3. Build-safety (cx + 3.5 Flash HEMFİKİR — ÖNEMLİ)
- `RoomRunDirector` fallback düşman = editor-only `AssetDatabase.LoadAssetAtPath`; boss = var olmayan `Resources` yolundan `Resources.Load`. **Standalone BUILD'de null → düşman/boss spawn OLMAZ.** Editör play-mode'da çalışır.
- **KARAR:** Sunum **Editör'den** ise sorun yok. **Build** ise: fallback+boss prefabını inspector'a serialize et + Resources fallback ekle. → Kullanıcıya soruldu.

## B) boss→victory ÇELİŞKİSİ — REDDEDİLDİ
- ChatGPT "post-boss combat'ı kaldır, direkt victory" demişti. **3.5 Flash + cx + memory:** post-boss **dual-class sinerji arenası** RIMA'nın bilinçli P0 tasarımı (boss artık terminal değil). **KARAR: KORU, kaldırma.** Mevcut akış (Combat×3→Merchant→Boss→secondary-draft→post-boss→Victory) doğru.

## C) TEST OTOMASYONU
- **3.5 Flash:** sunum öncesi test yazmak over-engineering; manuel playtest + build-test öncelik.
- **KARAR:** Sunum için test YAZMA (manuel playtest). **Sunum sonrası** en yüksek-değer 3 test (regresyon döngüsünü kırar):
  1. `SpawnableEnemyPrefabs_HaveVisibleSprites_NoPlaceholder_NotLitMaterial` (EditMode, kolay) — mob-siyah + mob-eksik regresyonunu kalıcı çözer.
  2. `UnlockedClasses_HaveClassKit_AndControllerRouting` (EditMode, kolay) — skill-bar-boş regresyonunu çözer.
  3. `CombatClear_OpensDoor_NoSoftlock` (PlayMode, orta).

## D) UYGULAMA SIRASI (adım adım)
1. **[ŞİMDİ] Demo'yu Warblade+Elementalist'e kısıtla + 5 lit-mob prefabını unlit'e çevir.** (skill-bar + mob-siyah kökten biter)
2. Mob çeşitliliği: encounter bank genişlet (opsiyonel) / Projectile null-sprite kontrol.
3. Build-safety: Editör mi Build mi kararına göre.
4. Background (tüm map arkası) — daha önce başlanmıştı, tamamla.
5. Sunum sonrası: 3 regresyon testi.
