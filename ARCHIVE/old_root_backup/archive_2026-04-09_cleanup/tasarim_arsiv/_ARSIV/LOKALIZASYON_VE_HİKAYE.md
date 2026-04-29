# Lokalizasyon ve Hikaye Tasarımı
*Son güncelleme: 2026-03-29*

---

## HİKAYE — THE CONFLUENCE (ÇAKIŞMA NOKTASI)

### Dünya Özeti

Çok sayıda paralel dünya, "The Fracturing" (Kırılma) adı verilen gizemli bir felaketle paramparça oldu.
Bu dünyaların kırıkları, merkezi bir boyutsal girdapta — **The Confluence (Çakışma)** — bir araya geldi.

The Confluence ne bir cennet ne bir cehennem: kırık dünyaların kalıntılarının örtüştüğü, zamanın tutarsız aktığı, ölümün kalıcı olmadığı bir labirent.

Her güçlü savaşçı, büyücü veya avcı kendi dünyasının kırılmasıyla bu labirente çekilir.
Confluence güçlü ruhları "ret eder" — ölürsün ama geri dönersin, sadece anılarını taşıyarak.

**Tek çıkış yolu:** Labirentin merkezine ulaşmak, **The Nexus**'u fethetmek ve Kırılma'nın kaynağıyla yüzleşmek.

### Her Ölüm Neden Yeniden Başlatır?
Confluence, güçlü bilinçleri absorbe etmez — dışarı iter.
Her ölüm, seni labirentin eşiğine, **The Threshold**'a geri gönderir.
Orada topladığın güç kırıntıları (meta-progression) bir sonraki denemende seni biraz daha güçlü yapar.

---

## ACT ATMOSFER REHBERİ

| Act | Ortam | Renk Paleti | Atmosfer |
|-----|-------|-------------|----------|
| **Act 1 — Shattered Ruins** | Çökmüş şehir, askeri kale kalıntıları | Gri, soğuk mavi, kahverengi | Savaşın dindiği an. Sessizlik ve enkaz. |
| **Act 2 — The Bleeding Wastes** | Koyu orman, çürük bataklık, bozulmuş tapınak | Mor, kırmızı, koyu yeşil | Bozulma ve çürüme. Canlı ama tehlikeli. |
| **Act 3 — The Core Approach** | Void siyahı, altın kırıklar, boyutsal çatlaklar | Siyah, altın, soğuk beyaz | Gerçekliğin inceldiği yer. Her şey parçalanıyor. |
| **Final — The Nexus** | Mutlak karanlık + saf enerji patlamaları | Koyu mor, parlak beyaz | Başlangıç ve son aynı anda. |

---

## CLASS KÖKENLERİ (Lore)

| Class | Kırılan Dünya | Kimlik |
|-------|--------------|--------|
| **Warblade** | Militarist bir imparatorluğun son savaşı | Düşen imparatorluğun son ayakta askeri |
| **Elementalist** | Büyük Arkanik Akademi'nin çöküşü | Akademi'nin kütüphanesini kurtarmaya çalışan büyücü |
| **Shadowblade** | Koca bir suikastçılar loncasının imhası | Tek hayatta kalan, kimliği belirsiz |
| **Ranger** | Doğanın vahşileştiği bir dünya | Asıl avcının avlanır hale geldiği yer |
| **Ravager** | Gladyatör arenasının çöküşü | İçgüdüyle hayatta kalan pit fighter |
| **Paladin** | Tanrısı öldürülen bir kutsal düzen | İnancı kalmış ama tanrısı gitmiş şövalye |
| **Summoner** | Büyük nekromancer savaşı | Düşmüş askerlerine bağlı kalmış büyücü |
| **Hexer** | Yasak bilgiler için yapılan anlaşma | Boşluk varlıklarıyla pakt yapmış akademisyen |

---

## HUB KARAKTERLERİ — THE THRESHOLD

### 🗝️ The Ferryman (Eşik Bekçisi)
**Rol:** Meta-progression arayüzü, ana NPC
**Kişilik:** Kadim, alaycı, her şeyi bilen ama az söyleyen
**Görünüm:** Kapkaranlık robe, yüzü gölgede, eller iskelet değil ama çok uzun
**Diyalog tonu:** "Yine mi döndün? Hiç şaşırmıyorum."
**Fonksiyon:** Shattered Echoes harcanır, meta-upgrade satın alınır

### 🔨 Vrel (Forge Echo)
**Rol:** Craft/upgrade NPC
**Kişilik:** Takıntılı, mükemmeliyetçi, sentimentalden yoksun
**Görünüm:** Yarı-transparan duman, bir eli çekiç tutan hayalet
**Diyalog tonu:** "Bu silah berbat. Ama şimdilik idare eder."
**Fonksiyon:** Özel upgrade'ler, gear modifikasyonları (ileriki fazlar)

### 🕯️ Sister Mourne (Yas Rahibesi)
**Rol:** HP/healing ekonomisi, lore anlatıcısı
**Kişilik:** Hüzünlü ama umutlu, eski dünyasının hikayelerini anlatır
**Görünüm:** Soluk gri kefen, gözlerin etrafında yas sürme
**Diyalog tonu:** "Buraya gelenler çok şey kaybetti. Sen de öyle."
**Fonksiyon:** Heal satışı, bazı run başlangıç bonusları

### 🗺️ The Cartographer (Haritacı)
**Rol:** Harita sistemi geliştirmeleri
**Kişilik:** Yarı deli, her şeyi kaydetme obsesyonu, güvenilir bilgi kaynağı
**Görünüm:** Elleri mürekkep lekeli, üstünde her yere kağıt parçaları yapıştırılmış
**Diyalog tonu:** "Bu odayı gördüm. Evet. 1847 kere. Neden soruyorsun?"
**Fonksiyon:** Harita sistemi upgrade'leri, daha fazla oda görünürlüğü

---

## LOKALİZASYON MİMARİSİ

### Neden Baştan Ayır?

Tüm metni koddan ayırmazsan:
- Oyunun ortasında her text'i avlaman gerekir
- Machine translation'ı sonradan uygulamak karmaşıklaşır
- QA çeviri doğrulaması imkansızlaşır

**Kural: Oyunda görünen her metin bir STRING KEY ile temsil edilir.**

---

### String Key Format

```
[KATEGORI].[ALT_KATEGORİ].[ID]
```

**Örnekler:**
```
SKILL.WARBLADE.IRON_CHARGE.NAME       = "Iron Charge"
SKILL.WARBLADE.IRON_CHARGE.DESC       = "Dash 8m, stun 1.5s, Rage +20"
SKILL.WARBLADE.IRON_CHARGE.CHAIN      = "First hit on stunned enemy → +80% damage"

UI.HUD.HP_LABEL                        = "HP"
UI.HUD.RAGE_LABEL                      = "RAGE"
UI.OFFER.TIER_COMMON                   = "Common"
UI.OFFER.TIER_RARE                     = "Rare"

STORY.HUB.FERRYMAN.FIRST_MEET         = "Another soul arrives..."
STORY.HUB.FERRYMAN.AFTER_DEATH_01     = "You returned. Surprising to no one."
STORY.HUB.VREL.UPGRADE_CONFIRM        = "This will do. For now."

CLASS.WARBLADE.NAME                    = "Warblade"
CLASS.WARBLADE.FANTASY                 = "Approach. Pin. Break. Execute."
CLASS.WARBLADE.LORE                    = "Last soldier of a fallen empire..."

ENCOUNTER.FORGE_WRAITH.NAME           = "Forge Wraith"
ENCOUNTER.FORGE_WRAITH.GREETING       = "Power is forged, not found."

ACT.ACT1.NAME                          = "Shattered Ruins"
ACT.ACT1.FLAVOR                        = "The war ended. Nothing else did."

ROOM.ELITE.CLEAR_MESSAGE              = "Well fought."
ROOM.SHOP.WELCOME                     = "The merchant's wares await."

BOSS.ACT1.NAME                         = "Confluence Guardian"
BOSS.ACT1.INTRO_QUOTE                 = "You should not have come this far."
```

---

### Lokalizasyon Tablo Kategorileri

| Tablo | İçerik |
|-------|--------|
| `SkillNames` | Tüm skill isimleri |
| `SkillDescriptions` | Tüm skill açıklamaları + chain bonusları |
| `ClassStrings` | Class isimleri, fantasy cümleleri, lore metinleri |
| `UIStrings` | HUD, menü, offer ekranı tüm metinleri |
| `DialogueHub` | Hub karakter diyalogları |
| `DialogueBoss` | Boss giriş/çıkış diyalogları |
| `StoryFlavor` | Act isimleri, oda flavor metni, event metinleri |
| `SystemMessages` | Hata, konfirmasyon, tutorial ipuçları |
| `EncounterStrings` | Spirit encounter isimleri, greeting'ler, offer açıklamaları |

---

### Unity Lokalizasyon Kurulumu

**Package:** `com.unity.localization` (Unity Package Manager'dan)

**Kurulum adımları:**
1. Package Manager → Unity Registry → Localization → Install
2. Edit → Project Settings → Localization → Active Locale: Turkish (TR)
3. Supported Locales ekle: TR, EN, ES
4. Window → Asset Management → Localization Tables → Create String Table
5. Her kategori için ayrı String Table oluştur

**ScriptableObject yaklaşımı (önerim):**
```csharp
// Skill verisi için örnek
[CreateAssetMenu]
public class SkillData : ScriptableObject
{
    public LocalizedString nameKey;        // "SKILL.WARBLADE.IRON_CHARGE.NAME"
    public LocalizedString descKey;        // "SKILL.WARBLADE.IRON_CHARGE.DESC"
    public LocalizedString chainKey;       // "SKILL.WARBLADE.IRON_CHARGE.CHAIN"
    // ... diğer veriler
}
```

**Runtime kullanımı:**
```csharp
// Asla hardcode string kullanma
// YANLIŞ:
titleText.text = "Iron Charge";

// DOĞRU:
titleText.SetLocalizedString("SKILL.WARBLADE.IRON_CHARGE.NAME");
```

---

### Ollama ile Çeviri Workflow'u

**Ollama ne zaman kullanılır:**
- String table'lar İngilizce ile dolduruldu
- Tüm metinler key sistemine girdi
- Şimdi toplu ilk-taslak çeviri yapılacak

**Önerilen model:** `llama3.2` veya `mistral` (İspanyolca için ikisi de iyi)

**Batch çeviri scripti (Python):**
```python
import ollama
import csv

def translate_table(input_csv, target_lang, output_csv):
    with open(input_csv, 'r', encoding='utf-8') as f:
        reader = csv.DictReader(f)
        rows = list(reader)

    for row in rows:
        if row['EN']:  # İngilizce kaynak
            prompt = f"Translate this game UI/skill text to {target_lang}. Keep it short and impactful. Text: '{row['EN']}'"
            response = ollama.chat(model='llama3.2', messages=[
                {'role': 'user', 'content': prompt}
            ])
            row[target_lang] = response['message']['content'].strip()

    with open(output_csv, 'w', encoding='utf-8', newline='') as f:
        writer = csv.DictWriter(f, fieldnames=reader.fieldnames)
        writer.writeheader()
        writer.writerows(rows)

# Kullanım:
translate_table('SkillNames_EN.csv', 'Spanish', 'SkillNames_ES.csv')
translate_table('UIStrings_EN.csv', 'Turkish', 'UIStrings_TR.csv')
```

**Önemli notlar:**
- Ollama çevirisi ilk taslak — mutlaka gözden geçir
- Özellikle skill isimlerinde "punchiness" (kısa, güçlü) korunmalı
- "Iron Charge" → İspanyolca "Embestida de Hierro" iyi, ama "Carga de Hierro" daha punk
- Türkçe için zaten sen yazacaksın, Ollama sadece İspanyolca/diğerleri için

**Önce ne yapılmalı (sıra):**
1. Tüm İngilizce string'leri KEY sistemiyle gir
2. Unity Localization Table'a aktar
3. Türkçe sütunu manuel doldur
4. İspanyolca için Ollama batch script çalıştır
5. Gözden geçir, gerekirse düzelt

---

## DEMODAKİ DİL DESTEĞİ

**Demo Faz 1:** Sadece Türkçe + İngilizce
**Demo Faz 2:** + İspanyolca
**Release:** Topluluk tercihine göre eklenir (Almanca, Portekizce öncelikli)

---

## EPİLOG KONSEPTİ

---

### Ana Tema Cümleleri

> *"The Fracturing bir felaket değildi yalnızca. Bir tercihti.*
> *Ve sen, o tercihin hem faili hem kalıntısısın."*

> *"Loop bir ceza değildi. Bir mühürdü.*
> *Ama hiçbir mühür sonsuza kadar aynı ellerde kalmamalı."*

> *"Nexus Core seni taklit etmiyor.*
> *O, senden geriye kalan ve karar vermeyi hiç bırakmamış parça.*
> *Sen ise kararın bedelini taşıyan, ama nedenini unutmuş kısımsın."*

---

### Büyük Sır — Nasıl Açılır

**Kural:** Twist hiçbir zaman doğrudan söylenmez. Oyuncu parçaları birleştirir.

Bunlar ayrı run'larda, ayrı anlarda gelir. Hiçbiri açıklama yapmaz. Hepsi sadece gösterir:

**Run 1-3 — İlk işaretler:**
- Ferryman seni ilk tanıştığında normal bir isim kullanmaz. Seni "the Bearer" veya eski bir unvanla çağırır. Sen sormadın. O açıklamadı.
- The Cartographer'ın haritalarından birinde, keşfetmediğin bir koridorun üzerinde el yazısıyla küçük bir not: *"Bu yolu bilen biri vardı. Artık bilmiyor."*

**Run 4-6 — Kırıklar derinleşiyor:**
- Nexus Core savaşında bir şey fark ediyorsun: sana ait olmayan skill hareketleri kullanıyor. Açmadığın, seçmediğin, hatta hangi class'a ait olduğunu bilmediğin şeyler. O seni değil — senden daha fazlasını biliyor.
- Vrel bir upgrade sırasında elindeki silaha bakıp duraksıyor. Sonra sadece şunu söylüyor: *"Bu silahı daha önce tutmuş biri vardı. Ellerin aynı şekli taşıyor."* Devam etmeden önce senden onay bekliyor. Neden beklediğini söylemiyor.

**Run 7-8 — Echo parçaları:**
- Geç act'lerdeki bazı Shattered Echo parçalarında başkasının anıları var. Savaş değil — bir karar anı. Birinin elleri titriyor. Seçim yapıyor. Yüzü görünmüyor, ama duruş tanıdık. Anı tam kararın verildiği anda kesiliyor.
- Sister Mourne, bir healing sırasında sana bakarken donup kalıyor. Uzun süre. Sonra sadece: *"Buraya gelenlerin çoğu bir şeyi kaybetti. Senin kaybın... farklı."* Daha fazlasını söylemeden çekiliyor.

**Run 9 — Ferryman'ın sessizliği kırılıyor:**
Ferryman ilk kez oturuyor. Bu karakterin hiç yapmadığı bir şey. Sadece şunu söylüyor:
> *"Sana bir şey söylemem gerekiyor. Ya da daha doğrusu — söylemememi bırakmam gerekiyor."*

Ve başlıyor. Ama her şeyi bilmiyor. Gördüklerini anlatıyor. Bazı yerlerde durup *"ya da öyle hatırlıyorum"* diyor. Bir noktada yanlış bir şey söyleyip kendini düzeltiyor. Bütün resmi görmüyor — sadece o anın tanığı.

Oyuncu boşlukları dolduruyor.

---

### Büyük Sırın Kendisi

**The Fracturing** tek kişinin tasarladığı bir şey değildi. Ama tek kişinin verdiği kararla gerçekleşti.

Confluence'ı tüketen bir şey vardı. Dünyadan dünyaya geçerek büyüyen, geri çevrilemez bir yayılma. Onu durduracak tek yol vardı: dünyalar arasındaki bağlantıları tamamen koparmak. Tek bir noktadan, tek bir anda, geri dönüşsüz biçimde.

Hesap yapıldı. Bedel kabul edildi. Karar verildi.

Ama bedel, hesaplananın çok ötesinde çıktı.

Mühür kuruldu — ama kurulurken bir şey parçalandı. O karar, o mutlaklık, o "geri adım atmama" iradesi — bir yerde donduruldu. Ve geriye kalan, kararın neden verildiğini hatırlamayan, ama kararın ağırlığını taşıyan bir şey oldu.

Bu ikisi şimdi ayrı varlıklar:

**Nexus Core:** Kararı veren iradenin donmuş hali. Değişemiyor. Şüphe edemez. Geri adım atmayı unuttu çünkü hiç öğrenmedi. Dünyaları korumak için mühürü sonsuza kadar zorluyor — çünkü karar buydu ve karar değişmez. Savaşta seni kopyalamıyor: senin olabileceğin yüzleri biliyor çünkü o da aynı kaynaktan geliyor. Ama o durdu. Sen devam ettin.

**Sen:** Kararın bedelini taşıyan, nedenini unutmuş, ama hâlâ değişebilen parça. Her run'da biraz daha hatırlıyorsun. Her secondary class bir başka yüzün açılıyor — çünkü Shattered Echoes sadece hafıza kırıntısı değil. Onlar senin, karardan önce taşıdığın ama The Fracturing'de saçılan class yüzlerin. Her Echo toplayışında daha bütün oluyorsun.

**Kahraman mı? Suçlu mu? Kurban mı?**
Bu sorunun cevabı kasıtlı olarak belirsiz kalır. Oyuncu kendi kararını verir.

---

### Üç Kapanış — Gerçek Bedelleriyle

#### 1. "Mühürle" — Nöbet
*Tone: melankolik huzur, kazanılmış sessizlik*

Karar bilinçli hale geliyor. Loop hep mühürdü — ama otomatikti, kör bir zorunluluktu. Şimdi seçiyorsun. Her run artık bir kaza değil, bir eylem.

Nexus Core son savaşından sonra dağılmıyor. Sessizleşiyor. İkisi de duruyorsunuz. O sana bakıyor — sende bir şey tanıyor. Belki ilk kez.

Hub karakterleri huzur buluyor. Ferryman'ın postürü değişiyor. Vrel bir şey üretiyor ama kimseye vermiyor — sadece rafına koyuyor. Cartographer haritasını kapatıyor.

Threshold kalıyor. Sen kalıyorsun.
Loop devam ediyor — ama artık senin loopun.

---

#### 2. "Kır" — Risk
*Tone: korku ve umut aynı anda, gerçek belirsizlik, gerçek kayıp*

Tüm Echolarını topladın. Tüm class yüzlerin açık. Nexus Core'u yenmek değil — onu geri almak istiyorsun. Mührü kırmak, kararı geri almak, bitmiş olanı açmak.

Bedel gerçek:
- Threshold kapanıyor. Mühür bitince o alan da biter.
- Hub karakterlerinin bir kısmı Threshold'un varlığına bağlıydı — bazıları yok oluyor. Tüm NPC'ler değil. Ama bazıları. Hangilerinin gideceğini önceden bilmiyorsun.
- Tüketen şeyin gerçekten gitmiş mi olduğunu bilmiyorsun. Belki gitti. Belki sadece uyuyor.
- Dünyalar yeniden bağlanıyor — ama birleşik düzenleri sonsuza kadar bitti.

Sonuç ekranda belirmiyor. Sadece şu görünüyor: açık bir kapı. Ve dışarısı.

---

#### 3. "Yük" — Bilinçli Nöbet
*Tone: en saf roguelite sonu — loop seçim, ceza değil*

Gerçeği öğrendin. Ve devam etmeyi seçiyorsun.

Ama bu unutmak değil. Ferryman'a bakıyorsun ve şunu söylüyorsun:
*"Anlıyorum."*

O kadar.

Sonra döneceğin kapıya yürüyorsun.

Fark şu: artık neden koştuğunu biliyorsun. Her Shattered Echo topladığında ne topladığını biliyorsun. Nexus Core'la savaşırken ne yaptığını biliyorsun — onu yok etmiyorsun, onu dengede tutuyorsun.

Loop bitmedi. Ama loop artık bir ceza değil. Bir nöbet.

Ve nöbet tutmayı seçen biri, onu tutan biriyle aynı şey değil.

---

### Epilog Tetikleyici Sistemi

```
Run 1-3:  Ferryman unvan kullanımı (pasif, açıklama yok)
          Cartographer'ın eski not (görsel, köşede)

Run 4-6:  Nexus Core açılmamış skill kullanıyor (savaşta, fark edilirse)
          Vrel duraklama sahnesi (upgrade sırasında kısa)

Run 7-8:  Echo fragments → karar anı anısı (geç act'te özel drop)
          Sister Mourne dondurma sahnesi (healing sırasında)

Run 9:    Ferryman konuşması (tetikleyici: 9. run hub girişi)
          Kısa, tanıklık bazlı, boşlukları doldurmuyor

Run 10+:  Threshold'da üç kapı belirir
          Her kapının önünde tek bir cümle var (isim yok, sembol var)
          Seçim yapılır
```

---

### String Key'ler — Epilog

```
STORY.EPILOGUE.FERRYMAN.SIT          = "Sana bir şey söylemem gerekiyor."
STORY.EPILOGUE.FERRYMAN.WITNESS_01   = "Oradaydım. Her şeyi görmedim. Ama gördüklerimi anlatayım."
STORY.EPILOGUE.FERRYMAN.WITNESS_02   = "Ya da öyle hatırlıyorum. Bazı kısımlar silik."
STORY.EPILOGUE.DOOR_SEAL             = "Kal."
STORY.EPILOGUE.DOOR_BREAK            = "Kır."
STORY.EPILOGUE.DOOR_CARRY            = "Taşı."
STORY.EPILOGUE.ECHOMEMORY_01        = "Elleri titriyordu. Karar verilmişti."
STORY.EPILOGUE.ECHOMEMORY_02        = "Bedel kabul edildi. Hesap yanlıştı."
STORY.EPILOGUE.NEXUS_RECOGNITION     = "..."
STORY.VREL.WEAPON_PAUSE              = "Bu silahı daha önce tutmuş biri vardı."
STORY.MOURNE.DIFFERENT_LOSS          = "Senin kaybın farklı."
STORY.CARTOGRAPHER.OLD_NOTE          = "Bu yolu bilen biri vardı. Artık bilmiyor."
```

---

## GENEL YAZIM TONU

| Bağlam | Ton |
|--------|-----|
| Skill isimleri | Kısa, güçlü, eylem hissettiren |
| Skill açıklamaları | Teknik ama okunabilir, max 2 cümle |
| Hub diyalogları | Karakter'in kişiliğini yansıtır, sıkıcı değil |
| Boss intro | Dramatik, kısa (1-2 cümle max) |
| Event metinleri | Seçim sonuçları net, ama dünya hissi var |
| UI metinleri | Minimal, net, jargon yok |

---

## RIMA — DÜNYADA ANLAMI

**Karar:** Option C — RIMA protagonistin eski benliğinin bıraktığı bir iz. Dışarıdan verilmiş isim değil.

Finalde oyuncu bunun kendi el yazısıyla yazılmış olduğunu fark eder. Neyi kastettiğini hatırlamıyor.

**NPC satırları:**

| NPC | Satır |
|-----|-------|
| Ferryman | "Bu kelimeyi ilk senden duymadım. Son da olmayabilirsin." |
| Cartographer | "Eski haritalarda bu işaret var. El yazısı tanıdık." |
| Final keşif | "Bunu sen yazmışsın. Yazdığını hatırlamıyorsun." |

```
STORY.RIMA.FERRYMAN_KNOWS    = "Bu kelimeyi ilk senden duymadım."
STORY.RIMA.CARTOGRAPHER_NOTE = "Eski haritalarda bu işaret var. El yazısı tanıdık."
STORY.RIMA.DISCOVERY         = "Bunu sen yazmışsın. Yazdığını hatırlamıyorsun."
```

---

## FRACTURING — 3 TEMEL ECHO SAHNESİ

*(Run 5-7 arası kademeli açılır — üçü üst üste değil, ayrı ayrı)*

### Sahne A: Titreyen El
Ekran: Yakın plan bir el. Bir kolu sabit tutmaya çalışıyor. Parmaklar titriyor.
Metin (sırayla, 3 satır):
> "Sabit tut."
> "Tutuyorum."
> "Yeterince değil."

*Anlam: Protagonist rasyonel değildi — çaresizdi. Karar soğukkanlılıkla değil, çöküşle verildi.*

### Sahne B: Kapanan Geçit
Ekran: Dev halka kapı ışığını kaybediyor. Karşı tarafta tek siluet — yüzü görünmüyor.
Metin:
> "Bekle—"
> "Geri alamam."
> "Biliyorum."

*Anlam: Kararın kişisel bedeli. Biri geride kaldı.*

### Sahne C: Sayılar Masası
Ekran: Masada işaretlenmiş bölgeler, kayıp oranları, kırmızı çizgiler.
Metin:
> "Bu ihtimalle yaşanmaz."
> "Bu ihtimalle en azından bir şey kalır."
> "Bir şey kimin için?"

*Anlam: Kararın soğuk mantığı. Doğru olabilir. Temiz değil.*

```
STORY.ECHO.TREMBLING_HAND_01 = "Sabit tut."
STORY.ECHO.TREMBLING_HAND_02 = "Tutuyorum."
STORY.ECHO.TREMBLING_HAND_03 = "Yeterince değil."
STORY.ECHO.CLOSING_GATE_01   = "Bekle—"
STORY.ECHO.CLOSING_GATE_02   = "Geri alamam."
STORY.ECHO.CLOSING_GATE_03   = "Biliyorum."
STORY.ECHO.NUMBERS_TABLE_01  = "Bu ihtimalle yaşanmaz."
STORY.ECHO.NUMBERS_TABLE_02  = "Bu ihtimalle en azından bir şey kalır."
STORY.ECHO.NUMBERS_TABLE_03  = "Bir şey kimin için?"
```

---

## NPC DİYALOG SETLERİ — GENİŞLETİLMİŞ

*(Her NPC protagonistin geçmişini farklı bir açıdan tanır. Çelişkili tanıklıklar — hakikat tek ağızdan gelmiyor.)*

### The Ferryman — Yorgun, kısa, bazen emin değil

1. "Seni ilk taşırken daha ağırdın. Şimdi daha sessizsin."
2. "Bir isim bırakmıştın burada. Sonra geri almış gibiydin."
3. "Ben her dönüşü hatırlamıyorum. Ama seni beklemeyi hatırlıyorum."
4. "Bazen aynı insan dönmez. Yine de aynı adımla iner."
5. "Eskiden daha az konuşurdun."
6. "Bir şey sormak istiyorum ama cevabını bilmiyorum. Bu yüzden sormuyorum."
7. "Bu sefer biraz farklı yürüdün. Belki hiç önemsiz."
8. "Ağırlığı taşımaya devam etmek — bu en az cesaretli şey değil."

*Ton: Kadim ama yorgun. Bütün resmi bilmiyor — sadece döngünün tanığı.*

### Vrel — Soğuk, teknik, iz ve hareket okuyan

1. "Bu ağırlık sana yabancı değil."
2. "Bir silahı iki kişi aynı tutabilir. Aynı nedenle savurmaz."
3. "Eskiden bileğin düşünmeden karar verirdi."
4. "Hafızan gitmiş olabilir. Kasların yalan söylemiyor."
5. "Bu silahı daha önce tutmuş biri vardı. Ellerin aynı şekli taşıyor."
6. "Ne aradığını bilmiyorum. Ama ellerinin ne aradığını biliyorum."
7. "Bir şey üretiyorum bazen. Kimin için bilmiyorum. Şimdilik rafta."
8. "Güç kullanımın temiz. Bu iyi. Bu aynı zamanda tehlikeli."

*Ton: İnsani bağ kurmayan ama en keskin gözlemci.*

### Sister Mourne — Şefkatli ama acıyı romantikleştirmeyen

1. "Unutmak iyileşmek değildir."
2. "Senin kaybın isim değil. Yön kaybı."
3. "Bazıları birini kaybeder. Sen bir kararı kaybetmiş gibisin."
4. "Taşıdığın şey suçsa bile, acısı gerçek."
5. "Buraya gelenler çok şey kaybetti. Senin kaybın... farklı."
6. "Eskiden gözlerini kaçırmazdın. Şimdi kaçırıyorsun. Bu iyi olabilir."
7. "Bazı yaralar hafızadan bağımsız kalır. Seninkiler hâlâ kanıyor."
8. "İyileşmek istemek zorunda değilsin. Ama taşımaya devam edebilirsin."

*Ton: Oyunun vicdan merkezi. Suçlamaz — ama yumuşatmaz da.*

### The Cartographer — Gözlemci, şiirsel, dolaylı

1. "Bu dönüşte kuzeye daha geç baktın."
2. "Bir yer unutulabilir. Bir yön nadiren."
3. "Haritalar gerçeği değil, tekrarları sever."
4. "Bu yol senden önce de senindi."
5. "Adımların aynı. Duraksamaların değil."
6. "Bazı noktalarda hep aynı yöne bakıyorsun. Belki arıyorsun."
7. "Haritada adın yok. Ama izin var. Bunlar aynı şey değil."
8. "Eski haritada bir işaret vardı. El yazısı. Tarih yok. Seninki gibi görünüyor."

*Ton: Hikayenin şiiri. Cevap vermez — soru bırakır.*

---

## BOSS ANLATIM SATIRLARI

### Act 1 Wardens

**Iron Warden**
- Giriş: *"Mühür sürer. Sebep ölse de."*
- Ölüm: *"Düzen... hâlâ..."*
- Lore: Fracturing sonrası kurulan ilk mühürlerin kalıntısı. Korumak için tasarlandı, amacını unuttu.

**Void Warden**
- Giriş: *"Boşluk doldurulmaz. Doldurulursa başka bir şey olur."*
- Ölüm: *"Yine mi sızıyor..."*
- Lore: Void'in sızmasını engellemek için kuruldu. Void onu da içine aldı.

**Chain Warden**
- Giriş: *"Geçiş kapatıldı. Yetki kalmadı."*
- Ölüm: *"Bağ... kopar mı..."*
- Lore: Hareketi kısıtlamak için programlandı. Kısıtlama tek bildiği şey oldu.

*Tema: İlk mühürlerin yaşayan ama içi boşalmış kalıntıları. Görevini sürdürüyor — nedeni kalmasa da.*

### The Fractured King

- Giriş: *"Tahtım kırıldı. Emrim sürüyor."*
- Faz 2: *"Yarıya bölünen bir dünya tek sese itaat etmez."*
- Ölüm: *"Kırık bile olsa... hükmedilir."*
- Lore: Parçalanmış dünyanın kendine kral biçmiş kalıntısı. Otoritesi kırık, ama bunu kabul edemiyor.

### The Hollow Sovereign

- Giriş: *"Şeklin sende. İrade bende olacak."*
- Adaptasyon: *"Sen ne olursan, ben ona karşı taç giyerim."*
- Ölüm: *"Boşluk... kendi olmaktan... yoruldu."*
- Lore: Biçimini senden alan otorite boşluğu. Özsüz ama reaktif. En tehlikelisi: seni yansıtır ama senden fazlasını koyamaz.

### The Nexus Core

- Faz 1: *"Ben kararı taşıyorum. Sen sadece bedelini."*
- Faz 2: *"Ben durdum. O yüzden dünya hâlâ biçim taşıyor."*
- Faz 3 (Legendary varsa): *"Şüphe ettin. Bu yüzden parçalandın."*
- Ölüm: *"..."* *(sessiz — tek seferlik)*
- Lore: Protagonistin durmayı seçmiş, donmuş hali. Kötü değil — sabit. Çatışma iyi-kötü değil, değişim-sabitlik.

```
STORY.BOSS.IRON_WARDEN.INTRO          = "Mühür sürer. Sebep ölse de."
STORY.BOSS.VOID_WARDEN.INTRO          = "Boşluk doldurulmaz. Doldurulursa başka bir şey olur."
STORY.BOSS.CHAIN_WARDEN.INTRO         = "Geçiş kapatıldı. Yetki kalmadı."
STORY.BOSS.FRACTURED_KING.INTRO       = "Tahtım kırıldı. Emrim sürüyor."
STORY.BOSS.FRACTURED_KING.PHASE2      = "Yarıya bölünen bir dünya tek sese itaat etmez."
STORY.BOSS.HOLLOW_SOVEREIGN.INTRO     = "Şeklin sende. İrade bende olacak."
STORY.BOSS.HOLLOW_SOVEREIGN.ADAPT     = "Sen ne olursan, ben ona karşı taç giyerim."
STORY.BOSS.NEXUS_CORE.PHASE1          = "Ben kararı taşıyorum. Sen sadece bedelini."
STORY.BOSS.NEXUS_CORE.PHASE2          = "Ben durdum. O yüzden dünya hâlâ biçim taşıyor."
STORY.BOSS.NEXUS_CORE.PHASE3          = "Şüphe ettin. Bu yüzden parçalandın."
STORY.BOSS.NEXUS_CORE.DEATH           = "..."
```

---

## ÜÇ SON — KAPANIŞ METİNLERİ (Final)

### "Kal" — Nöbet
> *"Kapı kapanmadı. Sen geri çekilmedin."*
> *"İlk kez döndüğün için değil — hatırladığın için."*

Ekran: Threshold sabit. Ferryman postürü değişir — hafif eğilir, ilk kez. Sessizlik.

### "Kır" — Risk
> *"Mühür kırıldı. Sessizlik ilk kez emir gibi gelmedi."*
> *"Ne kurtulduğunu bilmiyorsun. Ne kaybettiğini de."*

Ekran: Bazı NPC'ler yok olur (hangisi bilinmez). Açık kapı. Dışarısı.

### "Taşı" — Bilinçli Nöbet
> *"Kapıya yürüdün. Bu kez ceza gibi değildi."*
> *"Yük aynı kaldı. Taşıyan değişti."*

Ekran: Ferryman'a bakıyorsun. O başını hafifçe eğiyor. Kapıya yürüyorsun. Kapı açılıyor.

```
STORY.ENDING.SEAL.LINE1   = "Kapı kapanmadı. Sen geri çekilmedin."
STORY.ENDING.SEAL.LINE2   = "İlk kez döndüğün için değil — hatırladığın için."
STORY.ENDING.BREAK.LINE1  = "Mühür kırıldı. Sessizlik ilk kez emir gibi gelmedi."
STORY.ENDING.BREAK.LINE2  = "Ne kurtulduğunu bilmiyorsun. Ne kaybettiğini de."
STORY.ENDING.CARRY.LINE1  = "Kapıya yürüdün. Bu kez ceza gibi değildi."
STORY.ENDING.CARRY.LINE2  = "Yük aynı kaldı. Taşıyan değişti."
```

---

## EVENT ODALARI — HİKAYE EVENTLERİ (5 Adet)

### Event 1: Kapanmayan Kapı
*Kurulum:* Yarı açık dev kapı. İçeriden ses yok.

| Seçim | Sonuç |
|-------|-------|
| Zorla aç | İçeride kimse yok, duvarda tırnak izleri. Küçük resource drop. |
| Kapat | Küçük HP yenilemesi. Karakter 1 beat durur. |
| Uzaklaş | Sonraki odada metin: *"Kapıyı geride bıraktın. Bu da bir karar."* |

### Event 2: İsimsiz Gölge
*Kurulum:* Bir figür seni tanıyormuş gibi yaklaşıyor. Adını söylemiyor.

| Seçim | Sonuç |
|-------|-------|
| Yaklaş | *"Her seferinde biraz daha geç tanıyorsun."* — küçük buff |
| Adını sor | *"İsim senden önce buradaydı."* — lore fragment açılır |
| Geri çekil | Figür ayrılır. *"Belki başka run."* |

### Event 3: Kırık Emir
*Kurulum:* Eski komut paneli, ekran bozuk, üç komut seçilebilir.

| Seçim | Sonuç |
|-------|-------|
| PRESERVE | Savunma buff (2 oda) |
| SEVER | Saldırı buff + küçük HP bedeli |
| DELAY | Sonraki skill offer'da +1 ekstra kart |

*Ortak metin:* *"Emrin kaydı yok. Ama yankısı var."*
*Lore notu:* Fracturing öncesi karar sisteminin kalıntısı. Üç komut = üç sona metaforik köken.

### Event 4: Boş Sedye
*Kurulum:* Küçük alan. Sedye boş ama kumaşta taze olmayan iz.

Tek seçenek: Devam et (seçim yok — hafıza anı).
Metin: *"Biri burada kurtulmuş olabilir. Ya da yalnızca bırakılmış."*
Drop: Küçük HP + Mourne sonraki ziyarette referans verir.

### Event 5: Eski Not
*Kurulum:* Cartographer tarzı oda, yerde el yazısı kağıt.

Not içeriği (run başına random 1/3):
- *"Yolu bilen biri vardı. Artık bilmiyor."*
- *"Merkez seni tanıyor. Bu teselli değil."*
- *"Aynı dönüşü ikinci kez güvenme."*

| Seçim | Sonuç |
|-------|-------|
| Al | Lore fragment + Cartographer bir sonraki konuşmada referans verir |
| Bırak | *"Bıraktın. Belki bu da bir şey söylüyor."* |

```
EVENT.CLOSING_DOOR.OPEN    = "İçeride kimse yoktu. Ama izler tazdeydi."
EVENT.CLOSING_DOOR.CLOSE   = "Kapattın. Bir an durdu."
EVENT.CLOSING_DOOR.LEAVE   = "Kapıyı geride bıraktın. Bu da bir karar."
EVENT.SHADOW.APPROACH      = "Her seferinde biraz daha geç tanıyorsun."
EVENT.SHADOW.ASK_NAME      = "İsim senden önce buradaydı."
EVENT.BROKEN_CONSOLE.RESULT = "Emrin kaydı yok. Ama yankısı var."
EVENT.EMPTY_STRETCHER       = "Biri burada kurtulmuş olabilir. Ya da yalnızca bırakılmış."
EVENT.OLD_NOTE.01           = "Yolu bilen biri vardı. Artık bilmiyor."
EVENT.OLD_NOTE.02           = "Merkez seni tanıyor. Bu teselli değil."
EVENT.OLD_NOTE.03           = "Aynı dönüşü ikinci kez güvenme."
EVENT.OLD_NOTE.LEAVE        = "Bıraktın. Belki bu da bir şey söylüyor."
```

---

## SKILL HAFIZA FISILTILARI

*(İlk alındığında bir kez tetiklenir — sonra bir daha çıkmaz)*

### Primary Class Satırları

| Skill | Fısıltı |
|-------|---------|
| Iron Charge (Warblade) | *"Bu hareketin adını çoktan biliyordun."* |
| Death Blow (Warblade) | *"Bu sonu görmüşsün. Kendi tarafından."* |
| Stealth (Shadowblade) | *"Kaybolmayı o zamandan beri biliyorsun."* |
| State geçişi (Elementalist) | *"Dengeyi bir zamanlar daha kolay tutuyordun."* |
| Uzak menzil (Ranger) | *"Mesafe hem silah hem alışkanlık olmuş."* |

### Cross-Class Kombinasyon Fısıltıları
*(Secondary class açılınca, ilk combined skill alındığında)*

| Kombinasyon | Fısıltı |
|-------------|---------|
| Warblade + Shadowblade | *"Güç ve gölge. Bir zamanlar aynı gün içinde seçmiştin."* |
| Ranger + Elementalist | *"Mesafe bazen korkunun en zarif haliydi."* |
| Hexer + Summoner | *"Pakt ve bağ. Bunlar hep birlikte gelirdi."* |
| Ravager + Warblade | *"İki farklı öfke. Ama aynı kaynak."* |

```
STORY.SKILL.MEMORY.IRON_CHARGE     = "Bu hareketin adını çoktan biliyordun."
STORY.SKILL.MEMORY.DEATH_BLOW      = "Bu sonu görmüşsün. Kendi tarafından."
STORY.SKILL.MEMORY.STEALTH         = "Kaybolmayı o zamandan beri biliyorsun."
STORY.SKILL.MEMORY.ELEMENT_STATE   = "Dengeyi bir zamanlar daha kolay tutuyordun."
STORY.SKILL.MEMORY.RANGE           = "Mesafe hem silah hem alışkanlık olmuş."
STORY.CROSSCLASS.WARBLADE_SHADOW   = "Güç ve gölge. Bir zamanlar aynı gün içinde seçmiştin."
STORY.CROSSCLASS.RANGER_ELEMENT    = "Mesafe bazen korkunun en zarif haliydi."
STORY.CROSSCLASS.HEXER_SUMMONER    = "Pakt ve bağ. Bunlar hep birlikte gelirdi."
```

---

## RELIC / ITEM HAFIZA AÇIKLAMALARI

*(Her relic'in flavor text'ine dahil edilecek — full açıklamanın altında küçük italik satır)*

- *"Bu nesne yeni değil. Sadece ismini kaybetmiş."*
- *"Kullanım izi sahibinden daha inatçı."*
- *"Birisi bunu tuttu. Sıkıca. Uzun süre."*
- *"Eski işlevi değişmiş. Ama ağırlığı aynı."*
- *"Bu kırık. Ama kırılmadan önce ne olduğu belli."*
- *"Hatırlanmayan şeyler de iz bırakır."*

```
ITEM.MEMORY.01 = "Bu nesne yeni değil. Sadece ismini kaybetmiş."
ITEM.MEMORY.02 = "Kullanım izi sahibinden daha inatçı."
ITEM.MEMORY.03 = "Birisi bunu tuttu. Sıkıca. Uzun süre."
ITEM.MEMORY.04 = "Eski işlevi değişmiş. Ama ağırlığı aynı."
ITEM.MEMORY.05 = "Bu kırık. Ama kırılmadan önce ne olduğu belli."
ITEM.MEMORY.06 = "Hatırlanmayan şeyler de iz bırakır."
```

---

## CLASS UNLOCK — FERRYMAN DİYALOGLARI

*(Her unlock anında — kısa, tek cümle, o class'a özgü)*

| Class | Ferryman Satırı |
|-------|----------------|
| Elementalist | *"Bu parça hep yakındaydı. Şimdi adını buldun."* |
| Ranger | *"Mesafe hem silah hem alışkanlık olmuş. Bunu biliyordun."* |
| Shadowblade | *"Karanlıkta saklanan yüz en son çıkar."* |
| Ravager | *"Savaşın özüne inince bu yüz açılır."* |
| Summoner | *"Bağ hafızadan önce gelir."* |
| Hexer | *"Yasak bilgi başka bilgiden büyür."* |
| Brawler | *"En başta bıraktığın. En son dönen."* |

```
STORY.UNLOCK.ELEMENTALIST = "Bu parça hep yakındaydı. Şimdi adını buldun."
STORY.UNLOCK.RANGER       = "Mesafe hem silah hem alışkanlık olmuş. Bunu biliyordun."
STORY.UNLOCK.SHADOWBLADE  = "Karanlıkta saklanan yüz en son çıkar."
STORY.UNLOCK.RAVAGER      = "Savaşın özüne inince bu yüz açılır."
STORY.UNLOCK.SUMMONER     = "Bağ hafızadan önce gelir."
STORY.UNLOCK.HEXER        = "Yasak bilgi başka bilgiden büyür."
STORY.UNLOCK.BRAWLER      = "En başta bıraktığın. En son dönen."
```
