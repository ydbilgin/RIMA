# Canlı CLI Demo Script — "AI ile Nasıl Çalıştık?"
**Hedef süre:** 2–3 dakika | **Güvenli:** Yıkıcı komut yok, Unity'ye dokunulmaz
**Öneri:** Önce bir kez tam prova yap; adım sırasını ezberle, metni okuma.

---

## Ön Hazırlık (sunum başlamadan)

- [ ] Claude Code terminali açık ve RIMA dizininde (`F:\Antigravity Projeler\2d roguelite\RIMA`)
- [ ] Unity Editor açık, `_Arena` sahnesi yüklü, Play Mode KAPALI
- [ ] Yedek ekran görüntüleri hazır (B-Planı klasörü: `STAGING/report/figures_2026-06-18/`)
- [ ] Terminal fontu büyütülmüş (hocanın görebileceği boyut)

---

## Demo Akışı

### Adım 1 — Genel Yapı: "Stüdyo gibi, ama tek kişi" (~25 sn)

**Terminal'de göster:**
```
type ".claude\PROJECT_RULES.md"
```
*(İlk ~20 satırı göster, çok uzarsa Ctrl+C)*

**Konuşma notu:**
> "Projenin tamamı tek geliştirici tarafından yapıldı. Ama bir stüdyo gibi çalıştık.
> Bu dosya her oturumda yüklenen kurallar seti — ajan rolleri, denetim kapıları,
> hangi kararın nerede alınacağı burada yazıyor. Bir şeyi ajan yapsın demeden önce
> kabul kriterini ben belirliyorum."

---

### Adım 2 — Ajan Kadrosu: "Kim ne yapıyor?" (~30 sn)

**Terminal'de göster:**
```
type ".claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\MEMORY.md"
```
*(Routing bölümüne gel: "builder-opus / crafter-sonnet / cx / ax / auditor" geçen satırlar)*

**Konuşma notu:**
> "Orkestratör işi böler, dağıtır. builder-opus kod yazar, auditor-opus
> bağımsız denetler — yazan denetlemez. cx yani Codex statik analiz için,
> ax yani Gemini hızlı bilgi toplama için. Hepsi terminalde, tek noktadan."

---

### Adım 3 — Canlı Aksiyon: "Şimdi bir şey yapalım" (~40 sn)

**Söyle + yaz:**
> "Unity açık. Claude'a şunu söylüyorum:"

```
Claude, Unity'deki sahnenin ekran görüntüsünü al.
```

*(Claude UnityMCP üzerinden screenshot alacak, ~10 sn bekle)*

**Görüntü çıkınca:**
> "Bunu yapabilmesi için Unity Editor'e gerçek zamanlı bağlı.
> Sahneyi yükleyebiliyor, console'u okuyabiliyor, kod derleyebiliyor.
> Ama tetikleyen ben oluyorum — ajan benden izinsiz hiçbir şey başlatmıyor."

**B-Planı (internet veya Unity kapalıysa):**
Terminali kapat, `STAGING\report\figures_2026-06-18\` klasörünü dosya gezgininde aç.
Şunu söyle: _"Bu ekran görüntülerini ajanın Unity'ye bağlanarak aldığı çalışmadan aldım — şimdi ağ yok ama süreci göstermek için bunları kullanacağım."_

---

### Adım 4 — Denetim Kanıtı: "Her şey doğrulanıyor" (~25 sn)

**Terminal'de göster:**
```
dir "STAGING\_process\2026-06\_council*"
```
*(council çıktı dosyalarını listele)*

Ardından bir dosyayı aç:
```
type "STAGING\_process\2026-06\_council_cx_roomcap.md"
```
*(İlk ~15 satır yeterli — bu gerçek bir konsey çıktısı; başka `_council_*` dosyası da seçilebilir)*

**Konuşma notu:**
> "Kritik bir karar gelmeden önce üç farklı model bağımsız görüş bildiriyor —
> Codex, Gemini Pro, Gemini Flash. Ben sentezi yapıyorum.
> Ajanın 'tamamdır' demesi kanıt sayılmıyor; bağımsız denetçiden 'PASS'
> gelmedikçe bir sonraki adıma geçmiyorum."

---

### Adım 5 — Proje Hafızası: "Bilgi oturumdan oturuma taşınıyor" (~20 sn)

**Terminal'de göster:**
```
type "STAGING\report\graphify\graph_files.png"
```
*(Görüntü açılamazsa sadece sayıyı söyle)*

Ya da doğrudan söyle ve terminalde göster:
```
echo Graphify: 6925 dugum, sorgulanabilir kod hafizasi
```

**Konuşma notu:**
> "Kod tabanı 6.925 düğümlü bir bilgi grafiğine dönüştürüldü.
> Ajan 'şu sistem nasıl çalışıyor?' sorusunu sormak yerine grafı sorgular —
> onlarca dosyayı okumaktan çok daha hızlı. Tasarım kararları da NotebookLM'de
> — dün ne kararlaştırdık, hangi şeyi neden reddettik, hepsi erişilebilir."

---

### Adım 6 — Kapanış (~15 sn)

> "Özet: yön ben veriyorum, AI denetimli bir hatta uyguluyor.
> Hangi sırayla, hangi kabul kriteriyle, kimin denetlediği — hepsi
> baştan belgelenmiş. Bu yapı sayesinde tek geliştirici olarak
> çok-sistemli bir oyun motoru, editör araç zinciri ve
> sprite üretim hattını paralel yürütebildim."

---

## Riskler ve B-Planı Özeti

| Risk | B-Planı |
|------|---------|
| Unity bağlantısı yok / MCP yanıt vermiyor | `figures_2026-06-18/` klasöründen hazır ekran görüntüsü göster |
| `type` komutu çok uzun çıktı | İlk 20 satırda Ctrl+C, "yeterli" de |
| Council dosyası bulunamıyor | MEMORY.md'nin Routing bölümünü göster, söz olarak anlat |
| Terminal karakter bozukluğu (Türkçe) | Dosyayı VS Code'da aç, terminalden gösterme |
| Genel bağlantı/sistem sorunu | Slayt geçişine dön, "canlı gösteremedim ama kaydettim" de ve figures klasörünü projekte |

---

## Prova Kontrol Listesi

- [ ] Adım 3 Unity screenshot ~10 sn alıyor (önceden test et)
- [ ] `type` komutlarının çıktısı okunabilir boyutta
- [ ] B-Planı klasörü masaüstüne kısayol olarak eklendi
- [ ] Toplam süre: prova ≤ 2 dk 45 sn
