ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
`_IsoGame` sahnesinde F5'te (play mode) oyuncu **WARBLADE karakteri olarak GÖRÜNÜR ve oynanabilir** olsun. Şu an Player kırmızı placeholder kare olarak duruyor (sprite/class atanmamış). Hedef: F5 → iso floor üstünde net Warblade sprite'ı (örn. idle SE/south), hareket/yön çalışır.

# Bağlam (Opus gözlemi, 2026-06-01)
- Önceki cx fix'i ile `_IsoGame` F5'te authored iso floor (floor451_0, IsoGrid/Ground, 560 hücre) korunuyor (useAuthoredSceneRoom flag). BU ÇALIŞIYOR — DOKUNMA/BOZMA.
- Ama `Player` objesi F5'te kırmızı kare. Player componentleri: SpriteRenderer, Animator, PlayerAnimator, PlayerController, PlayerAttack, RageSystem, Health, IsoSorter, YSortBehaviour. Sahnede `Systems` altında `PlayerClassManager` ve `PlayerStartMarker`, `[MainMenuScreen]`, CharacterSelect akışı OLABİLİR.
- Warblade asset'leri (DİSKTE MEVCUT, kullan):
  - Idle 8-yön: `Assets/Resources/Characters/Warblade/warblade_idle_{south,SE,east,NE,north,NW,west,SW}.png`
  - Animator controller: `Assets/Resources/Characters/Warblade/Warblade.controller` (veya `Assets/Animations/Characters/Warblade/Warblade.controller`)
  - Prefab: `Assets/Prefabs/Characters/Warblade.prefab` (varsa wiring referansı)

# Görev (minimal, surgical)
1. F5'te Player neden kırmızı kare? İncele: SpriteRenderer.sprite null mı, yoksa PlayerClassManager/CharacterSelect bir sınıf yüklemeden mi başlıyor? Kök-nedeni bul.
2. EN TEMİZ minimal yolla Player'ı Warblade yap. TERCİH:
   a. Eğer `PlayerClassManager` / sınıf seçimi varsa: başlangıç/varsayılan sınıfı **Warblade** yap (böylece doğru sprite + Warblade.controller + RageSystem otomatik yüklenir). Mümkünse main-menu/character-select'i F5'te bypass edip direkt Warblade ile başlat (sadece bu sahnede, diğerlerini bozmadan).
   b. Eğer sınıf sistemi yoksa/karmaşıksa: Player'ın SpriteRenderer.sprite'ını `warblade_idle_SE` yap + Animator.runtimeAnimatorController'ı `Warblade.controller` ata.
3. **KARAKTER FLOOR'A BASMALI, HAVADA SÜZÜLMEMELİ.** Pivot/feet ayakta-tile-merkezde olsun (sprite bottom pivot). NOT: oda/floor void'de yüzer = KASITLI, ona dokunma — sadece karakterin floor üstünde doğru oturmasını sağla.
4. KIRMA: diğer sahneleri (PlayableArena_Test01 vb.) ve önceki useAuthoredSceneRoom fix'ini DEĞİŞTİRME. Değişiklik additive/bu-sahne-özel olsun.

# Doğrulama (ZORUNLU)
- Unity refresh sonrası `read_console` → 0 derleme hatası.
- `_IsoGame` aç → F5 → `manage_camera` game_view screenshot → Warblade sprite'ı iso floor üstünde GÖRÜNÜYOR mu (kırmızı kare DEĞİL)? → stop. Screenshot yolunu CODEX_DONE'a yaz.
- Player'ın WASD ile hareket ettiğini/yön değiştirdiğini mümkünse doğrula.

# Çıktı (CODEX_DONE_<profil>.md)
- Kök-neden (neden kırmızı kareydi).
- Uygulanan yaklaşım (a/b), değiştirilen dosyalar + satır sayısı.
- F5 screenshot yolu + derleme + regression kontrolü.
- Belirsizlik varsa BLOCKED yaz.
