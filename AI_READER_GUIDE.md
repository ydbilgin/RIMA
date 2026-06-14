> Giriş noktası: `PROJECT_INDEX.md` (tek-ekran harita). Bu dosya = detaylı navigasyon.

# AI OKUYUCU REHBERİ — RIMA Reposunu Doğru Okuma Kılavuzu

> Bu dosya, repoya bağlanan yapay zekâ asistanları (ChatGPT, Claude, Gemini vb.) için yazıldı.
> **Önce bunu oku, sonra aşağıdaki sırayla ilerle.** Repo ~2 haftalık yoğun iterasyon içeriyor;
> bazı eski dosyalar SUPERSEDED kararlar taşır — öncelik kuralları bu dosyada.

## RIMA nedir? (30 saniyelik özet)
Unity 6 ile tek geliştirici tarafından yapılan **2D top-down (yüksek 3/4 kamera) chibi pixel-art roguelite**.
Evren: "The Fracturing" — kırılmış gerçeklik; oyuncu Echo'lara bürünerek (10 sınıf) yüzen ada-odalardan
oluşan run'lara girer. Referanslar: Hades (oda akışı/portal seçimi), Slay the Spire (dallanan harita),
Dead Cells (build çeşitliliği). Meta para birimi: **Shattered Echo**. Oda doktrini: duvarsız
**yüzen ada + arka kenarda Rift portalları** (fiziksel kapı yok). İkinci ana katkı: çok-ajanlı
yapay zekâ geliştirme metodolojisi (yazar≠reviewer, council kararları, üçlü kalite güvencesi).

## OKUMA SIRASI
1. **`STAGING/report/RAPOR_DRAFT_2026-06-06.md`** — EN İYİ BAŞLANGIÇ. Oyunun ve tüm sistemlerin
   güncel, dürüst, baştan sona anlatımı (bitirme raporu taslağı; ~24 sayfa).
2. **`CURRENT_STATUS.md`** — canlı proje durumu. EN ÜSTTEKİ blok en yenidir; aşağı indikçe tarih geriler.
3. **`STAGING/MASTER_PLAN_FINAL_2026-06-06.md`** — aktif yol haritası (T1-T9).
3.5. **`STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md`** — dosya:satır kanıtıyla canlı yol (_Arena → RoomRunDirector → IsoRoomBuilder). Hangi kodun aktif olduğunu anlamak için oku.
4. **`TASARIM/GDD.md`** + **`TASARIM/CLASS_SILHOUETTE_BIBLE.md`** + **`TASARIM/RIMA_GAME_FEEL_AND_MECHANICS_BIBLE.md`**
   — çekirdek tasarım. Sınıf/silah hızlı özeti için: `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md`.
5. **`STAGING/*_DECISION_*.md`** — council kararları. **Dosya adındaki tarih = geçerlilik sırası; EN YENİ kazanır.**
6. Kod: `Assets/Scripts/` (sistemler), `Assets/Data/Rooms/` (26 oda şablonu), `Assets/Tests/` (~549 test).

## ÇELİŞKİ / ÖNCELİK KURALI (drift hiyerarşisi)
Aynı konuda çelişen iki dosya görürsen geçerlilik sırası:
**En yeni tarihli `STAGING/*_DECISION_*.md` > TASARIM/ canon > MEMORY/ > eski STAGING taslakları.**
MEMORY/ dosyaları nokta-zamanlı gözlemlerdir; 2+ hafta eskiyse bayatlamış olabilir.
`_archive`/`_ARCHIVE` adlı her klasör = bilinçli emekli edilmiş içerik, güncel kabul ETME.

## BİLİNEN REVOKED/SUPERSEDED KARARLAR (eski dosyalarda görsen de GEÇERSİZ)
- ❌ 4-yön karakter sprite → ✅ **8 yön LOCKED** (5 üret + 3 flipX ayna)
- ❌ 2.5D / 3D environment / KayKit-Blender pipeline (S57-58) → ✅ saf 2D top-down
- ❌ Hexer silahı "Whip" spekülasyonu → ✅ **Grimoire / Cursed Totem / Scepter**
- ❌ Skill'lerin Echo ile unlock'lanması → ✅ RED (skill meta-unlock YOK; build = run-içi draft)
- ❌ Heal/Lore portal türleri → ✅ sadece Combat / Elite / Reward(Chest) / Boss
- ❌ Wall-heavy dungeon / fiziksel kapı → ✅ yüzen ada + Rift portalı (arka kenar NW/N/NE soketleri)
- ❌ "Tek portal cephesi" → ✅ **2 authored açı** (N=frontal, NW=açılı) + NE=runtime flipX
- ❌ Karakter canvas tartışmaları → ✅ canvas **120×120**, efektif çizim ~64px, PPU 64, Point filter
- ❌ Currency rename ("Vestige" vb.) → ✅ **"Shattered Echo"** tam-form

## DÜRÜSTLÜK NOTU (kapsam beyanı)
"10 sınıf" = 10 sınıflık VERİ MODELİ; demo'da **4'ü uçtan uca oynanabilir** (Warblade/Elementalist/
Shadowblade/Ranger). 111 skill tanımının 67'si implement, placeholder'lar draft havuzuna sızmaz
(isImplemented filtresi). Test envanteri 549 tanım; son kayıtlı tam EditMode koşusu 410 PASS / 0 FAIL / 1 inconclusive.

## LEGACY KOD (canlı yolda DEĞİL — yeni iş bağlanmaz)
`RoomLoader` / `RoomSequenceData` / `Gate.cs` / `GateBehavior` / `DoorTrigger` / `RuntimeRoomManager` = eski oda/kapı sistemi, hiçbir canlı sahnede yok.
Canlı yol kanıtı: `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md` → _Arena → RoomRunDirector → IsoRoomBuilder.BuildExitDoors → RoomRunExitDoorTrigger → DungeonGraph child-choice.

## REPO'DA OLMAYANLAR
Kullanılmayan/arşiv binary'ler (Kenney orijinalleri vb. — `STAGING/ASSET_USAGE_AUDIT_2026-06-07.md`),
Unity `Library/` (yeniden üretilir), NotebookLM bilgi tabanı (kaynakları zaten TASARIM+MEMORY+STAGING
olarak bu repoda).
