---
status: OPUS_REVIEW
faz: 1
tarih: 2026-05-18
ozet: "Tum_Karakterler_Isimlendirilmis 32 dosya Opus inceleme — kullanım önerileri + illogical bulgu"
folder: STAGING/antigravity/Tum_Karakterler_Isimlendirilmis/
---

# Antigravity Karakter Klasörü — OPUS Review

## TL;DR
**32 dosya:** 10 Ana (= ANCHORS/characters/), 1 Gunslinger_Ana2 (yedek), 5 Alternatif (skin candidates), 1 Kullanilmayan (REJECT), 5 NPC (Hub candidates), 10 Yedek (image16_split duplicate'leri).

**Önemli bulgular:**
1. ⚠️ **Alternatif_Shadowblade_Mor_Kapsonlu** yanlış sınıflandırılmış — Shadowblade canonical "void purple" değil, bu teal/blue saçlı = **Frost Elementalist skin** olmalı (re-classify)
2. ✅ **Alternatif_Ranger_Yasli_Avci** = potansiyel **AGED WARBLADE candidate** (yaşlı sakal+leather)! S-XL aging workflow yerine bunu init image olarak kullanabiliriz
3. ❌ **Kullanilmayan_Modern_Gri_Kapsonlu** = modern hoodie/casual, RIMA tone ihlali, REJECT
4. ⚠️ **NPC_Sari_SivriSacli_Genc** = anime/young vibe, RIMA grimdark tonuyla riskli — test gerek
5. ✅ **5 NPC slot'unun 3'ü solid** (Demirci/GuildMaster/YetenekHocasi), 2'si optional

---

## 10 Ana — Status (= ANCHORS/characters/)

| # | Dosya | Sınıf | Status |
|---|---|---|---|
| 01 | `01_Warblade_Ana.png` | Warblade | ⏳ Pending lock (user aging tweak isteyebilir) |
| 02 | `02_Brawler_Ana.png` | Brawler | ✅ Canonical match |
| 03 | `03_Ravager_Ana.png` | Ravager | ✅ Canonical match |
| 04 | `04_Ranger_Ana.png` | Ranger | ✅ Canonical match |
| 05 | `05_Shadowblade_Ana.png` | Shadowblade | ⏳ Pending (purple accent gerek doğrulama) |
| 06 | `06_Gunslinger_Ana.png` | Gunslinger | ⏳ Pending (kızıl saç eski canonical, user fix dark hair LOCK) |
| 06 | `06_Gunslinger_Ana2.png` | Gunslinger | ✅ **Dark-skin sleek = USER FIX canonical** — bunu lock |
| 07 | `07_Ronin_Ana.png` | Ronin | ⏳ Pending (topknot doğrulama) |
| 08 | `08_Elementalist_Ana.png` | Elementalist | ⏳ Pending (honey-blonde drift kontrol) |
| 09 | `09_Hexer_Ana.png` | Hexer | ⏳ Pending (curse rune accent gerek) |
| 10 | `10_Summoner_Ana.png` | Summoner | ⏳ Pending (summoning gesture gerek) |

**Kritik karar:** `06_Gunslinger_Ana2.png` (dark-skin sleek) **canonical** olarak lock — `06_Gunslinger_Ana.png` (red ponytail) **eski canonical** olarak archive.

---

## 5 Alternatif — Skin Variant Candidates

| Dosya | Antigravity etiketi | Opus verdict | Önerilen kullanım |
|---|---|---|---|
| `Alternatif_Brawler_Kesis_Kel.png` | Brawler Kesiş Kel | ✅ Doğru sınıf | **Brawler Monk Class Skin** (Karar #155 Phase 2 skin pilot batch 2) |
| `Alternatif_Elementalist_Doga_Yalinayak.png` | Elementalist Doğa | ✅ Doğru sınıf (yaratıcı) | **Druid/Nature Elementalist Class Skin** — dark-skin diversity ekler! Karar #155 batch 2 |
| `Alternatif_Gunslinger_Kirmizi_Sackuyrugu.png` | Gunslinger Kırmızı | ◐ Eski canonical | **Hub NPC "Gunslinger Mentor"** OR Phase 2 skin (red-hair alt) |
| `Alternatif_Ranger_Yasli_Avci.png` | Ranger Yaşlı Avcı | ⚠️ **MISCLASSIFIED** — bu **AGED WARBLADE** candidate | **Warblade Aged Skin** veya S-XL aging init image referansı! |
| `Alternatif_Shadowblade_Mor_Kapsonlu.png` | Shadowblade Mor | ❌ **MISCLASSIFIED** — teal/blue ≠ Shadowblade canonical (void purple) | **Frost Elementalist Class Skin** — re-classify! Karar #155 Phase 2 |

### 🔑 Anahtar bulgu: Alternatif_Ranger_Yasli_Avci = Aged Warblade

Bu karakter (yaşlı bearded male + dark leather) **kullanıcının istediği aged Warblade görünümüne çok yakın**. S-XL aging prompt yazmak yerine **bu sprite'ı direkt Warblade aged skin olarak kullanabiliriz** — Karar #145 Use #4 (Class Skin) altında "Warblade Veteran" variant.

Alternatif olarak: S-XL aging init image olarak `Alternatif_Ranger_Yasli_Avci.png` kullanılır → mevcut Warblade canonical'a uygulanır.

---

## 1 Kullanılmayan

| Dosya | Verdict |
|---|---|
| `Kullanilmayan_Modern_Gri_Kapsonlu.png` | ❌ **REJECT** — modern hoodie/casual, RIMA "Fractured Epic + Ritual Catastrophe" tonu ihlali, Karar #79 Tone Surfaces Standard'a uymaz |

→ Archive'a taşı veya sil.

---

## 5 NPC — Hub Candidate Review (max 3 v1 per Karar #156)

| Dosya | Önerilen rol | Verdict |
|---|---|---|
| `NPC_GuildMaster_Gumus_Sacli.png` | **Mentor (primary)** — Hub guild leader | ✅ **Pilot v1** |
| `NPC_Demirci_Kahverengi_Yelek.png` | **Vendor / Crafter (Blacksmith)** | ✅ **Pilot v1** |
| `NPC_YetenekHocasi_Yasli_Kemerli.png` | **Skill Trainer** | ✅ **Pilot v1** |
| `NPC_GorevVeren_KisaSac_Paltolu.png` | **Quest-giver** | ⚠️ Optional (GuildMaster'a merge edilebilir) |
| `NPC_Sari_SivriSacli_Genc.png` | Genç stagiaire NPC | ❌ **Tone risk** — anime/young vibe, RIMA grimdark uyumsuz, REJECT veya regen |

### v1 Hub NPC LOCK (Sprint 18 Karar #156 hedef)
- **Vendor:** `NPC_Demirci_Kahverengi_Yelek.png`
- **Mentor:** `NPC_GuildMaster_Gumus_Sacli.png`
- **Lore-Keeper / Skill Trainer:** `NPC_YetenekHocasi_Yasli_Kemerli.png`

---

## 10 Yedek — Backup Slots

`Yedek_Grid1_*` ve `Yedek_Grid2_*` 10 dosya = `image16_split/` duplicate'leri. **Aksiyon:** archive klasöre taşı veya sil — Image #16 split zaten var.

---

## "ELİMİZDEKİ DİĞER TÜRLERİ" — Kullanım Önerileri

| Karakter | Mevcut sınıflandırma | Önerilen "diğer kullanım" |
|---|---|---|
| Alt_Brawler_Kesis_Kel | Brawler Monk skin | Sprint 17 Skin Pilot batch 1 (yedek) — kullanılmazsa Phase 2 |
| Alt_Elementalist_Doga | Druid Elementalist skin | **YENİ ÖNERİ:** "Druid Elementalist" alt skin (dark-skin diversity + nature theme) |
| Alt_Gunslinger_Kirmizi | NPC Mentor | "Gunslinger Master" Hub NPC (Sprint 18 candidate) — eğer kullanmazsak archive |
| Alt_Ranger_Yasli_Avci | **AGED WARBLADE** | Karar #145 Use #4 "Warblade Veteran" class skin — kullanıcının aging isteğini direkt karşılıyor |
| Alt_Shadowblade_Mor | **Frost Elementalist** (re-classify) | Karar #155 Skin Pilot — (4,2) yerine **bu** anchor olarak Frost Elementalist üret |
| Kullanilmayan_Modern | ❌ REJECT | Archive |
| NPC_GorevVeren | Quest-giver | Phase 2 Hub'da, eğer Cinematic Layer V1 quest UI gelirse |
| NPC_Sari | ⚠️ Tone risk | REJECT veya "Apprentice" Phase 3 storytelling karakter |

### Yeni Karar Adayı (Image #14/#16 + Antigravity'den çıkan)
- **Karar #155 v2:** Skin Pilot batch 1 değişiyor — eskiden (4,2 Frost) + (4,4 Brawler Female), şimdi:
  - **Frost Elementalist** = `Alt_Shadowblade_Mor_Kapsonlu.png` (re-classify Shadowblade → Frost Elem)
  - **Druid Elementalist** = `Alt_Elementalist_Doga.png` (dark-skin diversity)
  - **Warblade Veteran** = `Alt_Ranger_Yasli_Avci.png` (aged version — user aging request direkt karşılanır)
  - Brawler Female skin → Karar #145 Use #6 "make him female" state workflow ile sonra üretilir (yeni anchor değil)

---

## State Workflow ile Düzeltilebilecek Şeyler

User'ın "state ekranında düzeltebiliriz bazı şeyleri" notuna istinaden — **mevcut Ana karakterlere state prompt ile uygulanabilecek fix'ler:**

| Karakter | Drift / Eksik | State prompt (Karar #145 Use #6) |
|---|---|---|
| 05_Shadowblade_Ana | Purple accent zayıf | "add bright void purple glow to shoulder pads and chest seams" |
| 07_Ronin_Ana | Topknot belirsiz | "tie the black hair into a clear samurai topknot at the back" |
| 08_Elementalist_Ana | Saç drift kontrol | "ensure honey-blonde hair tied in a low bun (NOT red NOT auburn)" |
| 09_Hexer_Ana | Curse rune zayıf | "add dark red hex-rune accent on collar and cuffs" |
| 10_Summoner_Ana | Summoning gesture yok | "raise one hand in a downward palm conducting gesture, faint cyan glow at fingertips" |
| 01_Warblade_Ana | Yaşlı görünüm istemi | "make the face older and weathered, late 40s veteran, light wrinkles + grizzled beard with grey streaks" |

**5000 gen budget rahat** — bu 6 state fix toplam ~6-10 gen tutar.

---

## ⚠️ MANTIKSIZ / İLLOGICAL BULGULAR

1. **`Alternatif_Shadowblade_Mor_Kapsonlu.png` → Shadowblade ETİKETİ YANLIŞ.** Teal/blue saç + lavender hood = Frost Mage/Sorceress vibes, Shadowblade'in canonical "narrow dark assassin + void purple" kimliğine **uymuyor**.
2. **`Alternatif_Ranger_Yasli_Avci.png` → Ranger ETİKETİ YANLIŞ.** Bearded older male + dark leather = Warblade Veteran/Aged kimliğine **çok yakın**, Ranger canonical (female + bleached ivory + forest green) ile **alakasız**.
3. **`NPC_Sari_SivriSacli_Genc.png` tone uyumsuz.** Anime/young/spiky-blonde RIMA grimdark tone (Karar #79) ile çakışır.
4. **`Kullanilmayan_Modern_Gri_Kapsonlu.png` tone uyumsuz.** Modern casual hoodie ≠ Fractured Epic / Ritual Catastrophe.
5. **`Yedek_*` 10 dosya = image16_split duplicate.** Cleanup gerek.

---

## ÖNERİLEN AKSIYON LİSTESİ

| # | Aksiyon | Owner | Önclelik |
|---|---|---|---|
| 1 | `06_Gunslinger_Ana.png` (red ponytail) → archive, `06_Gunslinger_Ana2.png` (dark-skin) → **canonical lock** | USER + Opus rename | P0 |
| 2 | `Alternatif_Shadowblade_Mor_Kapsonlu.png` → **rename + re-classify** olarak `Frost_Elementalist_Skin_Candidate.png` | Opus rename | P0 |
| 3 | `Alternatif_Ranger_Yasli_Avci.png` → **rename + re-classify** olarak `Warblade_Veteran_Skin_Candidate.png` (user aging isteği karşılanır!) | Opus rename | P0 |
| 4 | `Kullanilmayan_Modern_Gri_Kapsonlu.png` → archive (`_archive_rejected/`) | Opus mv | P1 |
| 5 | `NPC_Sari_SivriSacli_Genc.png` → archive veya regen | USER | P1 |
| 6 | `Yedek_Grid*` 10 dosya → archive (image16_split zaten var) | Opus mv | P2 |
| 7 | State workflow ile 5 Ana karakter drift fix (Sprint 14 hazırlık) | USER PixelLab | P1 |
| 8 | Hub NPC v1 pilot lock: Demirci + GuildMaster + YetenekHocasi | Sprint 18 | P2 |
| 9 | Skin Pilot Sprint 17 batch 1 güncel: Frost Elem + Druid Elem + Warblade Veteran (3 skin, hepsi Alt klasöründen) | Karar #155 update | P1 |

---

## CODEX REVIEW BEKLENİYOR

Bu Opus inceleme **Codex review** dispatch edilecek (parallel) → Codex 2nd opinion verecek → Opus final verdict.
