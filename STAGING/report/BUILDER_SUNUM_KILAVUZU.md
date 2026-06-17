# BUILDER-OPUS — Sunum Kılavuzu PDF (yanında-aç cep notu) — 2026-06-18

ACTIVE RULES: min code, surgical. cx KULLANMA. git commit YOK.

## AMAÇ
Kullanıcı sunarken yanında açıp bakacağı **temiz, taranabilir A4 PDF** (1-3 sayfa). reportlab ile üret (kurulu, 4.5.1). Akademik rapordan AYRI — bu "cep notu".

## ÇIKTI
`STAGING/report/make_sunum_kilavuzu_pdf.py` (reportlab) + ÇALIŞTIR → `STAGING/report/RIMA_Sunum_Kilavuzu.pdf` (var, >5KB doğrula). Türkçe, tam TR karakter (reportlab'da TTF font kaydet: Windows `C:/Windows/Fonts/segoeui.ttf` + `segoeuib.ttf` bold → registerFont; yoksa Arial/DejaVuSans). Başlıklar teal (#21576B), gövde 10-11pt, tablolar çizgili, bölümler net ayrık. Sayfaya sığsın, okunur olsun.

## İÇERİK (AYNEN kullan — kullanıcının sözü)

### Başlık
**RIMA — Rift Avcıları · Sunum Kılavuzu** (alt: "Sunarken yanında aç. Sırayla göster, notları hatırla.")

### 1. SUNUM AKIŞI (sırayla göster)
1. **AÇILIŞ — Graphify god-node görseli:** "Bu sadece oyun değil; environment + tooling. Kanıt: kod-grafındaki en bağlı 10 node'un 6'sı editor/araç sınıfı."
2. **MainMenu → BAŞLA → Warblade seç.**
3. **Combat:** hareket + LMB combo + Q/E/R/F skiller; wave temizle. Vurgu: juice (hasar sayısı, hit-stop, ekran sarsıntısı).
4. **Boss:** telegraph'lar — kırmızı tehlike dairesi + YEŞİL güvenli-halka; telegraph bitince hasar; can barı düşüşü.
5. **Reward → Draft:** kart seç (build çeşitliliği).
6. **Run-map:** branching oda ilerleme (per-run seed).
7. ⭐ **CENTERPIECE — Edit-to-Play:** F2 Build Mode → prop koy/oda düzenle → çık → AYNI odayı oyna. "Unity açmadan, oyun çalışırken seviye tasarımı."
8. **Director Mode (` tuşu):** stat slider ile canlı zorluk / spawn / telemetry.
9. **KAPANIŞ:** graphify + AI-destekli süreç (council/cx/ax) → "geliştirme sürecini de mühendislik problemi olarak ele aldım."

### 2. HOCAYA NOTLAR — "bu var, şöyle güzelleşecek" (tablo)
| Sistem | Söyle |
|---|---|
| Combat/Boss | Çekirdek combat+boss+telegraph+draft+run-map çalışıyor. İyileşecek: düşman çeşitliliği + skill VFX. |
| Build Mode (F2) | Oyun çalışırken seviye editörü. İyileşecek: Lights/Decals kategorileri + oda kaydet/yükle. |
| Director Mode | Canlı stat/spawn/telemetry. İyileşecek: kart tasarımı Hades-stili ikon+badge. |
| HUD | Modern sol-alt. İyileşecek: HP barı rengi crimson'a, can-düşük efekti. |
| Silah | 8-yön ele mount + facing ön/arka. İyileşecek: her yön ince-ayar. |
| Elementalist | İkinci sınıf var ama eksik: 8-yön sprite + skill VFX (kredi limiti). Sahnede uzun tutma. |
| ASIL GÜÇ | Mimari + oyun-içi tooling + AI-destekli süreç + graphify ile veriyle-kanıtlı tooling tezi. |

### 3. BİLMEN GEREKENLER (hızlı referans)
- **Tez:** RIMA = environment + vertical slice + reusable oyun-içi tooling. Eksen ~%20 oyun / %60 mimari / %20 graphify-audit.
- **Teknoloji:** Unity 6, URP 2D, C#, ScriptableObject veri-güdümlü, Input System.
- **Graphify:** 6925 node / 118 community; god-node'ların ~6/10'u editor/tool = tooling tezi sayısal kanıt.
- **Tooling:** Build Mode (F2, Edit-to-Play) + Director Mode (runtime UI factory) + F1 debug.
- **Sınıflar:** 5/10 derinlemesine (genişlik değil derinlik). Demo ana sınıf: Warblade.
- **Süreç:** çok-ajanlı AI orkestrasyonu (council + cx/Codex + ax/Gemini-Opus dispatch) + graphify sorgulanabilir kod-grafı.
- **Vaka analizi (güçlü):** combat-bug — "yeşil-assert ≠ çalışıyor" → data-güdümlü kök-neden (detectionRange) → cerrahi fix → full-flow runtime doğrulama.

### 4. OLASI SORULAR (sorulursa)
- **"Elementalist nerede / neden tek sınıf?"** → "Demo'da Warblade'e odaklandım; Elementalist ikinci sınıf — sistemleri hazır ama 8-yön sprite üretimi araç-kredi limitiyle beklemede."
- **"Oyun motoru mu yazdın?"** → "Hayır, Unity üzerinde; ama oyun-içi seviye editörü + runtime director aracı yazdım — bu projenin tooling/environment katmanı."
- **"AI ne kadarını yazdı?"** → "AI'ı çok-ajanlı bir mühendislik aracı olarak kullandım; kararları, doğrulamayı ve mimariyi ben kurdum — council + graphify ile denetledim."
- **"Test var mı?"** → "Evet, + çok-katmanlı doğrulama (otomatik test → bağımsız AI-review → runtime-reproduce). Combat-bug vaka analizi tam bunu gösteriyor."
- **"Tamamlanmamış kısımlar?"** → Dürüst: 5/10 sınıf (tasarım kararı), Elementalist görselleri, bazı polish (collider/VFX). "Her eksiği hızlandıran tool/süreç yazdım."

## Çıktı: bana ≤6 satır özet + PDF yolu.
