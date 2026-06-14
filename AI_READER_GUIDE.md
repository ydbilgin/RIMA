> Giris noktasi: `PROJECT_INDEX.md` (tek-ekran harita). Bu dosya = ayrintili navigasyon.

# AI OKUYUCU REHBERI — RIMA Reposunu Dogru Okuma Kilavuzu

> Bu dosya, repoya baglanan yapay zeka asistanlari (ChatGPT, Claude, Gemini vb.) icin yazildi.
> **Once bunu oku, sonra asagidaki sirayla ilerle.** Repo ~3 haftalik yogun iterasyon icerir;
> bazi eski dosyalar SUPERSEDED kararlar tasir — oncelik kurallari bu dosyada.

---

## RIMA nedir? (30 saniyelik ozet)

Unity 6 ile tek gelistirici tarafindan yapilan **2D top-down (yuksek 3/4 kamera) chibi pixel-art roguelite**.
Evren: "The Fracturing" — kirilmis gerceklik; oyuncu Echo'lara burUnerek (10 sinif veri modeli, 4'u demo'da
uctan uca oynanabilir: Warblade / Elementalist / Shadowblade / Ranger) yuzen ada-odalardan olusan run'lara
girer. Referanslar: Hades (oda akisi/portal secimi), Slay the Spire (dallanan harita), Dead Cells (build
cesitliligi). Meta para birimi: **Shattered Echo**. Oda doktrini: duvarsiz **yuzen ada + arka kenarda Rift
portallari** (fiziksel kapi yok).

**Guncellenmis bitirme tezi (2026-06-14 — 4x council + graphify AST verisiyle kilitlendi):**
RIMA = sadece oynanabilir demo degil; oda authoring + runtime build + run-graph + live edit + director/debug
+ stat/VFX routing uzerine kurulu **domain-specific oyun-gelistirme environment'i + ilk dikey kesit**.
Sunum ekseni: **%20 oyun / %60 mimari / %20 graphify-audit**; centerpiece = **Edit-to-Play timelapse videosu**
(F2 Build Mode'da oda ciz → Play → cizdiginde dov). Bitirme sunumu ~20 Haziran 2026.
Graf kaniti: en cok baglantili 10 soyutlamadan 6'si editor/authoring araci (DirectorMode-168,
InPlayMapPaintOverlay-93, RoomPainterWindow-88 vb.) → kod agirlik merkezi tooling.
Detay: `STAGING/PRESENTATION_VISION_DECISION_2026-06-14.md`.

---

## ERISIM KATLARI (kim ne gorebilir)

| Katman | Erisim |
|--------|--------|
| (A) RIMA klasoründe Claude (otonom session) | En tam: auto-memory (MEMORY.md) + NLM + repo |
| (B) Repo-clone (disaridan AI/insan) | Sadece repo: NLM/auto-memory YOK; graphify + bu dosya + TASARIM/STAGING + RIMA_CANON_BRIEF yeterli |

**NLM erisimi (icerideysen):**
Tasarim bilgi tabani = NotebookLM (notebook ID **gizli**: `.claude/nlm.local`, gitignored — repo'ya konmaz; private memory'de de var).
Sorgu: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"` (ID repo-disi).
NOT: NLM kullanicinin private Google hesabinda; dis kloncularin erisimi yok.
Dis kloncular icin repo-aynasi: `STAGING/RIMA_CANON_BRIEF_FROM_NLM.md`.

---

## HANGI KAYNAK NEYi VERIR

| Kaynak | Icerigi |
|--------|---------|
| `PROJECT_INDEX.md` | Tek-ekran giris haritasi; pointer, icerik yok |
| `CURRENT_STATUS.md` | Canli durum: RESUME blogu, demo backlog, defer listesi |
| `STAGING/PRESENTATION_VISION_DECISION_2026-06-14.md` | Bitirme sunum tezi (guncel, kilitli) |
| `STAGING/*_DECISION_*.md` | Tum yeni kararlar (2026-06 sonrasi) |
| `TASARIM/` | Cekiridek tasarim canonlari (GDD, CLASS_SILHOUETTE_BIBLE, GAME_FEEL_BIBLE) |
| `STAGING/_process/2026-06/graphify_fullmap/graphify-out/` | 6925-node AST kod grafigi (graph.json + GRAPH_REPORT.md) |
| NLM notebook (yukarda) | Tasarim ayrintisi, lore, mekanik tarihcesi |
| `C:\Users\ydbil\.claude\projects\...\memory\MEMORY.md` | Otonom memory index (hard rules, routing, proje pointer'lari) |

---

## OKUMA SIRASI

1. **`PROJECT_INDEX.md`** — giriş haritasi (pointer).
2. **`AI_READER_GUIDE.md`** — bu dosya; oncelik + navigasyon kurallari.
3. **`CURRENT_STATUS.md`** — canli durum; en ustteki blok en yenidir.
4. **`STAGING/BITIRME_DEMO_RAPORU_2026-06-13.md`** — E2E raporu (10/10 PASS, 9/9 sistem; en guncel tam rapor taslagi). Genel sistem anlayisi icin alternatif: `STAGING/report/RAPOR_DRAFT_2026-06-06.md` (~24 sayfa derin anlati, hafif bayat).
5. **`STAGING/*_DECISION_*.md`** — kararlar; **dosya adindaki tarih = gecerlilik sirasi, EN YENI kazanir**.
6. **`TASARIM/GDD.md`** + **`TASARIM/CLASS_SILHOUETTE_BIBLE.md`** + **`TASARIM/RIMA_GAME_FEEL_AND_MECHANICS_BIBLE.md`** — cekiridek tasarim.
7. Kod: `Assets/Scripts/` (sistemler) + graphify AST grafigi (yukaridaki yol).
8. Canli yol kaniti: `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md` (_Arena → RoomRunDirector → IsoRoomBuilder).

---

## GUNCEL ANA IS (2026-06-14)

**In-game Build Mode editoru (F2)** — oyun icinde oda cizimi + prop yerlesimi + undo/redo + LiveRoomReloader.
Eski room-tool'lari (RoomLoader/RuntimeRoomManager/Gate.cs/GateBehavior/DoorTrigger) SUPERSEDE edildi.
SuankI odak: F2 reward-draft akis buglari (F1 + F2 backlog) + Edit-to-Play video kaydi.
Karar dosyalari: `STAGING/*BUILD_MODE*` veya `*LEVEL_EDITOR*_DECISION*` (glob ile bul).

---

## CELISKI / ONCELIK KURALI (drift hiyerarsisi)

Ayni konuda celisen iki dosya gorursen:
**En yeni tarihli `STAGING/*_DECISION_*.md` > TASARIM/ canon > MEMORY/ > eski STAGING taslaklari.**
`_archive` / `_ARCHIVE` klasoru = bilinçli emekli edilmis icerik, guncel KABUL ETME.
MEMORY/ dosyalari nokta-zamanli gozlemdir; 2+ hafta eskiyse bayatlamis olabilir.

---

## BILINEN REVOKED/SUPERSEDED KARARLAR

- Yok: 4-yon karakter sprite → Gercek: **8 yon LOCKED** (5 uret + 3 flipX ayna)
- Yok: 2.5D / 3D environment / KayKit-Blender pipeline → Gercek: saf 2D top-down
- Yok: Hexer silahi "Whip" spekulasyonu → Gercek: **Grimoire / Cursed Totem / Scepter**
- Yok: Skill'lerin Echo ile unlock'lanmasi → Gercek: RED (skill meta-unlock YOK; build = run-ici draft)
- Yok: Heal/Lore portal turleri → Gercek: sadece Combat / Elite / Reward(Chest) / Boss
- Yok: Wall-heavy dungeon / fiziksel kapi → Gercek: yuzen ada + Rift portali (arka kenar NW/N/NE soketleri)
- Yok: "Tek portal cephesi" → Gercek: **2 authored aci** (N=frontal, NW=acili) + NE=runtime flipX
- Yok: Karakter canvas tartismalari → Gercek: canvas **120x120**, efektif cizim ~64px, PPU 64, Point filter
- Yok: Currency rename ("Vestige" vb.) → Gercek: **"Shattered Echo"** tam-form
- Yok: SYSTEM_MAP.md (cekiridek mimari dosyasi) guncel → Gercek: REVOKED/arsivde (2026-04-29 donemi); kod haritasi = graphify AST

---

## DONUSTUK NOTU (kapsam beyani)

"10 sinif" = 10 siniflik VERI MODELI; demo'da **4'u uctan uca oynanabilir** (Warblade / Elementalist /
Shadowblade / Ranger). Skill sayisi: yakl. 111 tanim, 67'si implement (isImplemented filtresi; kaynak:
onceki raporlar — guncel exact sayi icin graphify veya Assets/Data/ say). Test sayisi: yaklasik 549 tanim;
son kayitli tam EditMode kosusu 410 PASS / 0 FAIL / 1 inconclusive (kaynak: BITIRME_DEMO_RAPORU-2026-06-13).

---

## LEGACY KOD (canli yolda DEGIL — yeni is baglanmaz)

`RoomLoader` / `RoomSequenceData` / `Gate.cs` / `GateBehavior` / `DoorTrigger` / `RuntimeRoomManager` =
eski oda/kapi sistemi; hicbir canli sahnede yok.
Canli yol: `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md` → _Arena → RoomRunDirector → IsoRoomBuilder →
RoomRunExitDoorTrigger → DungeonGraph child-choice.

---

## REPO'DA OLMAYANLAR

- Kullanilmayan/arsiv binary'ler (Kenney originals vb. — `STAGING/ASSET_USAGE_AUDIT_2026-06-07.md`)
- Unity `Library/` (yeniden uretilir)
- NLM bilgi tabani (dis kloncularin erisimi yok; repo-aynasi RIMA_CANON_BRIEF_FROM_NLM.md)
- Auto-memory dosyasi (kullaniciya ozel; `PROJECT_INDEX.md`'de NLM notebook ID pointer var)
