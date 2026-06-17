# BUILDER-OPUS — Kapsamlı sunum raporu (markdown + DOCX) — 2026-06-18

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear. cx KULLANMA. git commit YOK.

## GÖREV
İki çıktı üret:
1. `STAGING/report/SUNUM_RAPORU_2026-06-18.md` — aşağıdaki yapı + içerikle, Türkçe, TAM Türkçe karakter (ç ş ğ ü ö ı İ). Gömülü figür referansları markdown image olarak (`![başlık](figures_2026-06-18/xxx.png)`).
2. `STAGING/report/SUNUM_RAPORU_2026-06-18.docx` — `STAGING/report/create_rapor_docx.py`'yi ŞABLON al; yeni bir `make_sunum_docx.py` yaz (aynı python-docx stili: Calibri, başlık renkleri, figür gömme `doc.add_picture` ~15cm genişlik + figür altyazısı). Markdown'ı oku → stillenmiş DOCX üret. **Çalıştır ve DOCX'in oluştuğunu doğrula** (dosya var + boyut >20KB). python-docx kurulu (mevcut .docx kanıtı).

## KAYNAKLAR (oku, içeriği zenginleştir — ama AŞAĞIDAKİ yapı/notlar otorite)
- `STAGING/SUNUM_RAPOR_ICERIK_2026-06-17.md` (tez, anlatı yayı, "rapor cümleleri", açılış kancası — ANA kaynak)
- `STAGING/report/BOLUM_2_GIRIS.md` ... `BOLUM_9_SONUC.md` + `OZET_ABSTRACT.md` (akademik bölümler — gerekirse cümle/teknik detay çek, ama rapor SUNUM-odaklı kalsın, tez-uzunluğunda değil)
- `STAGING/PLAYTEST_POLISH_DECISION_2026-06-17.md` (bu oturum fix'leri)

## FİGÜRLER (hazır: `STAGING/report/figures_2026-06-18/`)
| Dosya | Nereye |
|---|---|
| `fig_graphify_godnodes.png` | Bölüm 1 (tez/açılış) + Bölüm 6 |
| `fig_graphify_full.png` | Bölüm 6 |
| `fig_gameplay_hud.png` | Bölüm 3 (gameplay+environment+yeni HUD) |
| `fig_draft_reward.png` | Bölüm 3 (draft/ödül kartları) |
| `fig_buildmode_centerpiece.png` | Bölüm 4 (CENTERPIECE — büyük koy) |
| `fig_director_mode.png` | Bölüm 4 (Director tooling) |
| `fig_weapon_mount.png` | Bölüm 7 (game-feel/polish) |

## RAPOR YAPISI (bölüm bölüm — bu otorite)

### Kapak
"RIMA — Rift Avcıları · Bitirme Projesi Sunum Raporu · 18 Haziran 2026". Alt: "Bir oyun değil; bir environment + vertical slice + oyun-içi geliştirici araç seti."

### 1. RIMA Nedir? (Tez)
- RIMA = sadece oyun DEĞİL → **environment + vertical slice + yeniden-kullanılabilir oyun-içi tooling**.
- Değerlendirme ekseni: ~%20 oyun / %60 mimari / %20 graphify-audit. Hoca sistem mimarisine + mühendislik disiplinine bakar, içerik hacmine değil.
- god-node figürünü teaser olarak koy ("6/10 god-node = editor aracı, kanıt Bölüm 6'da").

### 2. Sunumda Ne Göstermeli — Demo Akışı (Run-of-show)
Adım adım, numaralı, her adımda "ne yap + hangi tuş + ne anlat":
1. **AÇILIŞ:** Graphify god-node görseli → "bu bir environment+tooling, veriyle".
2. MainMenu → BAŞLA → karakter seç (**Warblade**).
3. **Combat:** bir oda — hareket, LMB combo, Q/E/R/F skiller, wave temizle. Juice'u vurgula (hasar sayısı, hit-stop, ekran sarsıntısı).
4. **Boss:** telegraph'ları göster — kırmızı tehlike dairesi + **yeşil güvenli-halka**; telegraph bitince hasar; can barı düşüşü.
5. **Reward → Draft:** kart seç (build çeşitliliği anlat).
6. **Run-map:** branching oda ilerleme (per-run seed, Merchant/Elite).
7. ⭐ **CENTERPIECE — Edit-to-Play:** `F2` Build Mode → prop koy/oda düzenle → çık → AYNI odayı oyna. "Unity editörü açmadan, oyun çalışırken seviye tasarımı."
8. **Director Mode (`):** stat slider ile canlı zorluk ayarı / spawn / telemetry.
9. **KAPANIŞ:** graphify + AI-destekli süreç (council/cx/ax dispatch) → "geliştirme sürecini de mühendislik problemi olarak ele aldım."

### 3. Çalışan Çekirdek Sistemler (vertical slice kanıtı)
Kısa paragraf + tablo. Her sistem 1-2 cümle: Combat loop (Warblade: hareket/LMB combo/Q-E-R-F/stat→hasar), Düşman AI (chase→attack, wave), Boss (3 faz + telegraph + can barı), Reward→Draft (kart), Branching run-map. `fig_gameplay_hud` + `fig_draft_reward` göm.

### 4. Oyun-İçi Tooling — CENTERPIECE
- **Build Mode (F2):** çalışan oyunda seviye editörü — prop/tile palette (veri-güdümlü `BuildModeAssetCatalog ← PropRegistrySO`), iso-grid, ghost-snap, undo/redo. Edit-to-Play: kur→çık→oyna. `fig_buildmode_centerpiece` BÜYÜK göm.
- **Director Mode (`):** runtime UI factory (sahnesiz boot) — Spawn/Stats/Telemetry(CSV)/Build/Map/Free-cam. Bu oturum IDE-dock skin + skill-listesi ScrollRect. `fig_director_mode` göm.
- Rapor cümlesi: "Tasarım iterasyonlarını Unity editörü açıp kapatmadan, oyun çalışırken yapan bir oyun-içi geliştirici aracı yazdım."

### 5. Mühendislik Süreci (meta-mühendislik)
- Çok-ajanlı AI orkestrasyonu: council workflow (bağımsız bakışlar + adversarial verify), cx (Codex) / ax (Gemini/Opus) dispatch zinciri, graphify sorgulanabilir kod-grafı.
- ⭐ **Combat-bug vaka analizi** (SUNUM_RAPOR_ICERIK bölüm 4'ten): "yeşil-assert ≠ çalışıyor" → data-güdümlü kök-neden (detectionRange) → cerrahi fix → full-flow runtime doğrulama. Çok-katmanlı doğrulama (otomatik test → dış AI-review → runtime-reproduce) + yanlış-pozitif eleme.
- Rapor cümlesi: "Otomatik testlerin yeşil olması yetmez; veri-güdümlü runtime doğrulama ile gerçek oynanışı kanıtladım."

### 6. Graphify Audit (veri-destekli tez)
- 6925 node / 118 community kod-grafı. **God-node analizi: en bağlı node'ların ~6/10'u editör/tool sınıfı** → "tooling environment" tezinin sayısal kanıtı. `fig_graphify_godnodes` + `fig_graphify_full` göm.
- İnteraktif graf: `STAGING/report/graphify/graph_interactive.html` (sunumda canlı açılabilir — not düş).

### 7. Görsel / Game-feel Polish
- CombatJuice (hasar sayısı/hit-stop/ekran-sarsıntı/kamera-punch), enemy outline + ambient lift, boss telegraph (yeşil safe-ring), CC0 music bed, deterministik ilk-oda.
- Bu oturum: prop Y-sıralama düzeltildi, silah mount iyileştirildi (`fig_weapon_mount`), HUD sol-alta taşındı.

### 8. Bilinen Eksikler + Yol Haritası (DÜRÜST — eksiği altyapı-gerekçesine bağla)
Tablo: Eksik | Durum | Neden/Plan.
- Elementalist 8-yön sprite | BLOCKED | PixelLab kredi limiti; pipeline hazır, kredi gelince üretilecek.
- Elementalist diğer skill VFX | yapım sırasında | SkillVfx wiring.
- Prop collider (sandık/fıçı walk-through) | sırada | PropColliderAutoBuilder ayarı.
- HUD HP barı rengi | iyileştirilecek | class-tint cyan → crimson'a çekilecek.
- Director kart tasarımı | kısmi | scroll çözüldü; Hades-stili ikon+badge sonra.
- 5/10 sınıf | tasarım kararı | derinlik > genişlik.

### 9. Hocaya Konuşma Notları — "Bu var, şöyle güzelleşecek"
Madde madde, sunumda söylenecek cümleler (ÖNEMLİ bölüm):
- "Çekirdek combat + boss + draft + run-map çalışıyor — vertical slice tamam. **İyileşecek:** düşman çeşitliliği + skill VFX zenginliği."
- "Build Mode oyun-içi seviye editörü — centerpiece. **İyileşecek:** Lights/Decals kategorileri dolacak + oda kaydet/yükle."
- "Director Mode canlı tuning aracı. **İyileşecek:** kart tasarımı Hades-stili ikon+badge'e geçecek (şu an işlevsel, metin-ağırlıklı)."
- "HUD modern sol-alt yerleşim. **İyileşecek:** HP barı rengi crimson'a, can-düşük efekti zenginleşecek."
- "Silah 8-yön ele mount + facing'e göre ön/arka katman. **İyileşecek:** her yön ince-ayar (bir yön doğrulandı, kalanı playtest sonrası)."
- "İkinci sınıf Elementalist var ama **eksik:** 8-yön sprite + skill VFX'leri (kredi/üretim sırada) — sahnede uzun tutma."
- "Asıl güç: mimari + oyun-içi tooling + AI-destekli geliştirme süreci + graphify ile veriyle-kanıtlanmış tooling tezi."

## ÇIKTI RAPORU
İş bitince `STAGING/report/SUNUM_RAPORU_DONE.md`'ye ≤15 satır: md+docx yolları, docx boyutu, gömülen figür sayısı, atlanan/riskli. Bana ≤10 satır özet döndür.
