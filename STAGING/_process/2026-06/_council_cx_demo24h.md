ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA jüri demosunu 24 SAATTE mümkün olan EN İYİ oynanabilir hale getirmek için feasibility/reuse lensiyle öncelik + sıra + ne-zaten-var analizi. ANALİZ ONLY, kod değişikliği YOK. Sonucu CODEX_DONE.md'ye yaz.

# LENS = CODE / FEASIBILITY / REUSE
Senin görevin: repoyu OKU, neyin GERÇEKTE kodlu/üretilmiş olduğunu doğrula, build-vs-reuse kararı ver. Önceki audit'leri TEKRAR ÜRETME.

---

## CONTEXT — RIMA jüri demosu, 24 saatte en iyi oynanabilir hale getirme

RIMA = Unity 2D top-down ARPG roguelite (URP 2D, Pixel Perfect, 8-yön sprite, PPU 64). Hedef: jüriye gösterilecek, baştan sona OYNANABİLİR, hatasız/stuck'sız dikey-slice + MÜMKÜN OLAN EN İYİ görsel/animasyon cilası.

### Demo akışı
MainMenu → Chamber (Warblade VEYA Elementalist seç; diğer 8 sınıf kilitli görünür) → Combat1 → Combat2 → Shop → Combat3 → Boss → Victory/Death → MainMenu. Zorunlu lineer sıra (branching yok).

### MEVCUT DURUM (kod-tamam + origin/master'a push'lu, AMA insan-playtest YOK; commit 405581cb)
- Softlock kök-fix (reward/draft timeout + garantili çıkış) — 76/76 test (`7489e2de`)
- Forced lineer demo sırası (DungeonGraph.BuildDemoSequence) + sabit kamera (useFixedDemoCamera, ortho 5.0)
- 2-sınıf kilit (ClassUnlockPolicy: Warblade+Elementalist açık, PlayerClassManager kilitliyi reddeder)
- PauseMenu (ESC → Resume/Settings/Codex/MainMenu/Quit, timeScale güvenliği) — 10/10 test (`b48c763`)
- Boss demo'da spawn (SpawnBossDirectly, PenitentSovereign) + ölüm→Victory; telegraph wire'lı (Slam/Line/Rift) (`673ef5f4`)
- Shop: 3 stand (Heal 20 / Damage 35 / +MaxHP 35), Shop_01 template, 8/8 test (`be9f536f`+`6a64287b`)
- Warblade kılıç sorting fix (Default→Entities + her-frame sort) + Elementalist yanlış-kılıç suppress (`0a97c72a`)
- 594 EditMode test, 17 fail HEPSİ pre-existing, 0 yeni fail, +18 yeni yeşil

### GAP'LER (bilinçli yapılmadı)
- **ANIMASYONLAR (yeni en-büyük scope):** Üretim ~0; çoğu hareket kod-driven (knockdown=kod-only eğme/squash+i-frame). Karakter/mob/boss için gerçek sprite animasyonu (idle/walk/attack/cast/hurt/death) minimal veya YOK. PixelLab pipeline KURULU (method B-hibrit kilitli, `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md`) ama üretim user-gated. Kullanıcı ARTIK animasyonları birlikte üretmek istiyor.
- **Sanat placeholder:** Elementalist floating rune disc, shop stand'lar = renkli kareler.
- **Boss prefab build-gap:** PenitentSovereign.prefab Resources'ta değil → standalone BUILD'de editör-dışı spawn olmaz (editör demo çalışır). 1-satır fix var (RoomRunDirector.bossPrefab serialize ata VEYA prefab Resources'a taşı).
- Legacy _IsoGame decommission (post-demo, council kararı `STAGING/OVERLAP_CLEANUP_DECISION_2026-06-09.md`).

### YENİ DİREKTİF (kullanıcı, 2026-06-09)
24 SAAT var. EN İYİ haliyle oynanabilir demo. Animasyonları birlikte "tak tak" üreteceğiz (PixelLab, user present). Animasyon artık scope İÇİNDE.

---

## SORULAR (feasibility/reuse lensinden cevapla)
1. **Repoyu doğrula:** Yukarıdaki "kod-tamam" iddialarından hangisi GERÇEKTEN canlı/test'li, hangisi kağıt-üstü? Demo akışında gizli kırık nokta var mı? (Özellikle: shop satın-alma efekti gerçekten uygulanıyor mu, boss telegraph→damage zinciri tam mı, victory→MainMenu timeScale restore var mı.)
2. **Animasyon mevcut durumu:** Şu an her karakter/mob/boss için diskte HANGİ sprite/animasyon var (idle/walk/attack)? PlayerAnimator/Animator controller'lar neyi sürüyor? Kod-driven hareket nerede, sprite-anim nerede? Yani 24h'de SIFIRDAN mı üreteceğiz yoksa var-olanı mı genişleteceğiz?
3. **24h bölüşümü (feasibility):** Verilen mevcut-durumda 24h'nin en yüksek-değerli kullanımı: (a) playtest+hardening (b) animasyon üretimi (c) sanat polish (d) build-gap — hangisi ÖNCE? Animasyon üretimi gerçekçi olarak kaç saat ve kaç asset sığar (PixelLab gen süresi + import + wire)?
4. **Boss build-gap:** 1-satır fix'i şimdi yapmak mantıklı mı, yoksa editör-demo yeterli mi? Hangi yol daha az risk?
5. **En büyük kırılma riski:** Demo jüri önünde HANGI noktada kırılır (en olası) ve repo-kanıtıyla 24h'de nasıl de-risk edilir?
6. **CUT listesi:** 24h'de yetişmeyecek, KESİLMESİ gereken ne var?

Kısa, kanıtlı (file:line), feasibility-odaklı cevap ver.
