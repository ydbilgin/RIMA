# BUILDER-OPUS — Sunum Kılavuzu PDF v2 (uzun + güzel tasarım) — 2026-06-18

ACTIVE RULES: min code, surgical. cx KULLANMA. git commit YOK.

## AMAÇ
Mevcut `RIMA_Sunum_Kilavuzu.pdf` çok kısaydı. v2 = **daha uzun (4-6 sayfa), daha zengin içerik, belirgin GÜZEL tasarım**. Kullanıcı sunarken yanında açıp okuyacak. Mevcut `make_sunum_kilavuzu_pdf.py`'yi GENİŞLET (reportlab 4.5.1 kurulu).

## ÇIKTI
`STAGING/report/make_sunum_kilavuzu_pdf.py` (güncelle) + ÇALIŞTIR → `STAGING/report/RIMA_Sunum_Kilavuzu.pdf` (ÜZERİNE yaz; >15KB, 4-6 sayfa doğrula, PyMuPDF ile her sayfayı görselle teyit et — TR karakter + glyph kutusu YOK). Türkçe tam karakter (Segoe UI TTF: segoeui/segoeuib; eksik glyph'leri ► gibi güvenli alternatife çevir).

## TASARIM (güzelleştir — bunlar şart)
- **Başlık banner'ı:** üstte teal (#21576B) dolgu bar, içinde beyaz "RIMA — RİFT AVCILARI · SUNUM KILAVUZU" + alt satır küçük beyaz "Sunarken yanında aç · Yasin Derya Bilgin".
- **Bölüm başlıkları:** teal, altında ince yatay çizgi (HRFlowable). Numaralı.
- **Adım kartları (demo akışı):** her adım için sol tarafta teal dolu yuvarlak/karede beyaz numara (1-9) + sağda bold başlık; altında etiketli satırlar — **GÖSTER:** (teal), **SÖYLE:** (koyu, italik tırnak içinde), **TUŞ:** (monospace gri kutu), **DİKKAT:** (amber #B5651D, varsa). Kartlar arası boşluk + ince ayraç.
- **Konuşma kutuları (Açılış/Kapanış):** açık-teal arka planlı (#EAF1F4) çerçeveli kutu (Table 1x1 + box style), içinde kelimesi-kelimesine metin.
- **Güçlü cümleler:** sol kenarda teal accent bar + bullet.
- **Tablolar:** teal başlık satırı (beyaz yazı) + alternating açık-gri/beyaz satır gölgeleme + ince gridler.
- **Checklist:** ☐ kutu glyph'i (yoksa "[ ]").
- **Gömülü görseller:** ilgili yerlere KÜÇÜK (~8cm) — `figures_2026-06-18/fig_graphify_godnodes.png` (Açılış bölümü) + `figures_2026-06-18/fig_buildmode_centerpiece.png` (Adım 7/centerpiece). Altına minik italik altyazı.
- **Footer:** her sayfada alt-orta/sağ "RIMA · Sunum Kılavuzu · S.<n>".

## İÇERİK (AYNEN — kullanıcının sözü; sırayla)

### 0. SUNUM ÖNCESİ CHECKLIST
☐ Unity açık + derlenmiş (0 error)  ☐ Play MainMenu'den başlıyor  ☐ Ses açık (müzik + SFX)  ☐ F2 (Build Mode) ve ` (Director) çalışıyor — 10 sn dene  ☐ Graphify god-node görseli/slide hazır  ☐ Yedek demo videosu hazır (canlı çökerse)  ☐ Game view tam ekran/maximize  ☐ Warblade sınıfı seçili akış prova edildi

### 1. AÇILIŞ KONUŞMASI (kutu, ~30 sn — okuyabilirsin)
"Merhaba, ben Yasin Derya Bilgin. Projem RIMA — Rift Avcıları. Bunu sadece bir oyun olarak değil; bir geliştirme environment'ı ve yeniden-kullanılabilir oyun-içi araç seti olarak tasarladım. Ekrandaki bu kod-grafında en bağlı 10 bileşenin 6'sı editör/araç sınıfı — yani bu proje içeriğiyle değil, mimarisi ve araçlarıyla değerlendirilmeli. Şimdi hem oynanabilir döngüyü hem de bu araçları göstereceğim."

### 2. DEMO AKIŞI (9 adım kartı — her birinde GÖSTER/SÖYLE/TUŞ/DİKKAT)
1. **Açılış görseli** — GÖSTER: graphify god-node görseli. SÖYLE: "Bu bir environment+tooling, veriyle." TUŞ: —. DİKKAT: konuşmayı buradan aç.
2. **Menü → Karakter** — GÖSTER: MainMenu → BAŞLA → Warblade. SÖYLE: "Ana sınıf Warblade ile gidiyorum." TUŞ: Mouse.
3. **Combat** — GÖSTER: hareket + LMB combo + Q/E/R/F, wave temizle. SÖYLE: "Stat'lar hasara yansıyor; juice ile vuruş hissi var." TUŞ: WASD, LMB, Q/E/R/F. DİKKAT: hit-stop/hasar sayısını göster.
4. **Boss** — GÖSTER: telegraph'lar + can barı düşüşü. SÖYLE: "Kırmızı tehlike, yeşil güvenli-halka; telegraph bitince hasar." TUŞ: —. DİKKAT: yeşil safe-ring'i işaret et.
5. **Ödül → Draft** — GÖSTER: kart seçimi. SÖYLE: "Her oda sonrası build'ini şekillendiriyorsun." TUŞ: G + tıkla.
6. **Run-map** — GÖSTER: branching oda ilerleme. SÖYLE: "Her koşu farklı: seed'li dallanma, Merchant/Elite." TUŞ: —.
7. **CENTERPIECE — Edit-to-Play** — GÖSTER: F2 Build Mode → prop koy/oda düzenle → çık → aynı odayı oyna. SÖYLE: "Unity'yi açıp kapatmadan, oyun çalışırken seviye tasarlıyorum — projenin kalbi bu." TUŞ: F2. DİKKAT: en güçlü an; yavaş+net göster.
8. **Director Mode** — GÖSTER: stat slider ile zorluk/spawn/telemetry. SÖYLE: "Canlı tuning aracı — dengelemeyi oyunu durdurmadan yapıyorum." TUŞ: ` (backquote).
9. **Kapanış** — GÖSTER: graphify + süreç. SÖYLE: kapanış konuşması (↓).

### 3. GÜÇLÜ CÜMLELER (accent bar + bullet — bunları sırasında düşür)
- "Tasarım iterasyonlarını Unity editörü açıp kapatmadan, oyun çalışırken yapan bir oyun-içi araç yazdım."
- "Otomatik testlerin yeşil olması yetmez — veri-güdümlü runtime doğrulama ile gerçek oynanışı kanıtladım."
- "Geliştirme sürecini de bir mühendislik problemi olarak ele aldım: çok-ajanlı AI orkestrasyonu + sorgulanabilir kod-grafı."
- "Mimari iddiamı graf-metrikleriyle destekledim: god-node'ların çoğunluğu tooling katmanı."

### 4. HOCAYA NOTLAR — "bu var, şöyle güzelleşecek" (tablo)
| Sistem | Söyle |
|---|---|
| Combat/Boss | Çalışıyor. İyileşecek: düşman çeşitliliği + skill VFX. |
| Build Mode (F2) | Oyun-içi seviye editörü. İyileşecek: Lights/Decals + oda kaydet/yükle. |
| Director Mode | Canlı tuning. İyileşecek: kart tasarımı Hades-stili ikon+badge. |
| HUD | Modern sol-alt. İyileşecek: HP rengi crimson, can-düşük efekti. |
| Silah | 8-yön mount + ön/arka. İyileşecek: yön ince-ayarı. |
| Elementalist | İkinci sınıf var; eksik: 8-yön sprite + skill VFX (kredi). Uzun tutma. |
| ASIL GÜÇ | Mimari + tooling + AI-süreç + graphify ile veriyle-kanıtlı tez. |

### 5. BİLMEN GEREKENLER (hızlı referans, bullet)
- Tez: environment + vertical slice + reusable oyun-içi tooling. Eksen ~%20 oyun / %60 mimari / %20 graphify.
- Teknoloji: Unity 6, URP 2D, C#, ScriptableObject veri-güdümlü, Input System.
- Graphify: 6925 node / 118 community; god-node ~6/10 editor = tooling tezi sayısal kanıt.
- Tooling: Build Mode (F2) + Director Mode (runtime UI factory) + F1 debug.
- Sınıflar: 5/10 derinlemesine; demo ana sınıf Warblade.
- Süreç: çok-ajanlı AI (council + cx/Codex + ax/Gemini-Opus dispatch) + graphify.
- Vaka analizi: combat-bug — "yeşil-assert ≠ çalışıyor" → data kök-neden (detectionRange) → cerrahi fix → full-flow doğrulama.

### 6. OLASI SORULAR (genişletilmiş Q&A)
- **"Elementalist nerede / neden tek sınıf?"** → "Demo'da Warblade'e odaklandım; Elementalist'in sistemleri hazır ama 8-yön sprite üretimi araç-kredi limitiyle beklemede — gösterirsem kısa tutarım."
- **"Oyun motoru mu yazdın?"** → "Hayır, Unity üzerinde; ama oyun-içi seviye editörü + runtime director aracı yazdım — projenin tooling/environment katmanı bu."
- **"AI ne kadarını yazdı?"** → "AI'ı çok-ajanlı bir mühendislik aracı olarak kullandım; mimariyi, kararları ve doğrulamayı ben kurdum; council + graphify ile denetledim."
- **"Test var mı?"** → "Evet + çok-katmanlı doğrulama: otomatik test → bağımsız AI-review → runtime-reproduce. Combat-bug vakası tam bunu gösteriyor."
- **"Neden roguelite / neden bu kapsam?"** → "Tek geliştiriciyle derinlik için: 5 sınıf derinlemesine + tekrar-oynanabilir döngü. Kapsamı araç/süreç yatırımıyla yönettim."
- **"Tamamlanmamış kısımlar?"** → Dürüst: 5/10 sınıf (tasarım kararı), Elementalist görselleri, bazı polish. "Her eksiği hızlandıran tool/süreç yazdım."
- **"Graphify nedir?"** → "Kodu sorgulanabilir bir bilgi-grafına çeviren araç; mimari iddiamı (tooling-ağırlıklı) god-node metrikleriyle kanıtlamamı sağladı."

### 7. EĞER BİR ŞEY BOZULURSA (fallback — amber kutu)
- **Combat takılır/düşman idle:** Sakin ol — "Bu canlı bir build; asıl gücü tooling" de, F2 Build Mode + Director'a geç.
- **Oyun çöker:** yedek demo videosuna geç.
- **Boss'a ulaşamazsın:** telegraph'ı başka düşmanda/Director-spawn ile göster.
- **Genel:** asla panikleme; mimari/graphify/süreç anlatısı oynanıştan bağımsız güçlü — oraya kay.

### 8. KAPANIŞ KONUŞMASI (kutu, ~30 sn)
"Özetle: RIMA çalışan bir vertical slice — combat, boss, draft ve dallanan run-map. Ama asıl katkım oyun değil: oyun-içi seviye editörü, runtime director aracı, AI-destekli çok-ajanlı geliştirme sürecim ve bu mimari iddiayı graphify ile veriyle kanıtlamam. Sıradaki adımlar: sınıf görsellerinin tamamlanması ve polish. Dinlediğiniz için teşekkürler — sorularınızı alabilirim."

## Çıktı: bana ≤6 satır özet + PDF yolu + sayfa sayısı.
