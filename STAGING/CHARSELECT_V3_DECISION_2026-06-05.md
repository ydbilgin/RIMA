# DECISION — CharacterSelect v3 + Skill-Echo-Unlock + Currency (2026-06-05)

**Council:** cx (Codex, feasibility — `CODEX_DONE_yasinderyabilgin.md`) ‖ ax Gemini 3.1 Pro (architecture — `_council_q_31pro_charselect_v3.md` çıktısı) ‖ ax Gemini 3.5 Flash (lean — `_council_q_35flash_charselect_v3.md` çıktısı) ‖ **Opus advisor (design+görsel — `_council_opus_charselect_v3.md`)** → Opus sentez (bu doküman).
**Girdiler:** kullanıcı konsept görseli (`mockups/charselect_concept_ref_2026-06-05.png`), ChatGPT önerisi (`mockups/charselect_chatgpt_brief_2026-06-05.md`, benimsenmedi — girdi), NLM canon sorgusu (`_nlm_charselect_currency.json`), kullanıcı SABİT direktifleri (brief §).

---

## K1 — Skill'ler Echo ile unlock'lanır mı? → **HAYIR (4/4 OYBİRLİĞİ). Canon korunur.**

**NLM canon:** Skill'ler meta-currency ile UNLOCK'LANMAZ. 12 common skill baştan açık; build = run-içi
3-kart draft; ölünce sıfırlanır. Görseldeki "USTALIK YETENEKLERİ — 400 ECHO" canon'a aykırı.

**Precedent sentezi (4 advisor'ın web araştırması):**
| Model | Örnek | Sonuç |
|---|---|---|
| Unlock → draft havuzuna girer | Dead Cells (Cells→Blueprint) | **BELGELENMİŞ ANTİ-PATTERN**: havuz sulanır, iyi item nadirleşir; oyuncular item'ları havuzdan ÇIKARMAK için yalvardı, dev Custom Mode yamadı. RIMA'nın 3-kart Rift-Seal draft'ına Echo'lu skill satmak = oyuncuya "havuz sulandırma" satmak. |
| Unlock → kalıcı güç | Hades (Mirror/Darkness) | Çalışıyor ÇÜNKÜ draft'tan tamamen AYRI eksen. RIMA canon "kalıcı skill tree yok" dediği için bu bile canon-amend olur — şimdi önerilmez. |
| Unlock → achievement (para YOK) | Death Must Die, Vampire Survivors | İyi karşılanıyor: "kazanılmış" hissi, "satın alınmış" değil. RIMA canon'unun karakter OR-yollarıyla birebir uyumlu. |

**Kod gerçekliği (cx):** Per-skill lock altyapısı YOK (SkillData'da unlockCost yok, pool filtresi class/depth-only,
run-sonu Echo kazanım kodu HİÇ yok, SkillDatabase'de 4/10 sınıf). Echo-skill-unlock = 4-6 dosya + 220-430
satır + ekonomi sistemi sıfırdan = negatif tasarım değeri için gerçek mühendislik maliyeti.

**KARAR:**
1. **Echo ile skill unlock YOK.** Echo'nun tek sink'i = karakter erişimi (+ ileride hub NPC upgrade'leri, canon).
2. Sağ paneldeki mastery bölümü → **achievement-gated teaser olarak re-skin (fiyat YOK)**:
   kilitli satırlar deed-koşulu gösterir ("Açılış: Act 1'i Warblade ile bitir"), Echo fiyatı ASLA.
   Demo'da = statik UI metni (0 ekonomi kodu). Gerçek mastery sistemi (açılınca draft havuzuna katılır,
   yine run-içi draft'lanır) = İLERİDE, ayrı karar.
   *(Lean alternatif — kullanıcı isterse: paneli tamamen at, yerine tek satır "Yeni yetenekler run sırasında
   Rift-Seal draft'ında belirir.")*

## K2 — Currency adı → **TAM RENAME YOK; UI'da daima tam-form "Shattered Echo" (◈ glifi)** *(öneri — kullanıcı onayı açık)*

- Council 2-2 bölündü: 3.1 Pro + 3.5 Flash → "Vestige" rename; **Opus advisor + cx → canon'u koru**.
- **Sentez kararı (Opus):** Canon adı **"Shattered Echoes"** ve lore'u (Fracturing'de saçılan class
  "yüzlerini" toplayıp bütünleşme) karakter-unlock currency'si için MÜKEMMEL — atılmaz. Çakışmanın
  gerçek kaynağı çıplak "Echo" token'ı (9+ başka kullanım: Echo Mode, Echo Imprint, Shadow Echo,
  Echo Twin, moblar...). Çözüm = **disambiguation, rename değil:**
  - Meta-currency UI'da HER ZAMAN tam-form: üst bar **"◈ 80"** (hover/unlock metninde "Shattered Echo"),
    unlock copy: "KİLİDİ AÇ — 150 ◈ Shattered Echo".
  - Çıplak "Echo" kelimesi gameplay mekaniklerine rezerve (başka ekranlarda yaşıyorlar; yan yana gelmezler).
  - Kod-ucuz (cx): display string'leri tek helper'da topla; gameplay Echo'ya global-rename YAPMA (unsafe).
- **"Vestige" = fallback** — tam-form fix'ten sonra playtester'lar hâlâ karıştırırsa; o durumda bile önce
  "Echo Mode"u (kolay zorluk) rename etmek tercih edilir.
- NLM canon'a not düşülecek (nlm-sync ile bu doküman gidecek).

## K3 — Görsel direktifler + iyileştirme planı (HTML-FIRST)

**Kullanıcı SABİT direktifleri** (tüm tasarım bunlara uyar): tek ekran scroll YOK · öğelerde dikey hareket
YOK (bob/parallax dahil) · ada altı temiz void (dekor/resim yok) · karakterler karolara gerçekçi otur
(tile-center snap) · boyutlar SABİT (seçilide büyüme yok) · kilitli = **OPAK SİYAH silüet** · Echo satın
alma/mağaza hissi YOK · karaktere tıkla-seç · alt strip/pedestal YOK.

### Konsept görsel verdict (Opus advisor — görseli gören tek advisor)
✅ İşleyen: on-brand mood (void-mor + cyan çatlaklar + ikiz brazier) · sol panel 5 stat-bar okunabilirliği ·
seçili Warblade ayak-halkası (doğru seçim dili) · ince üst bar.
❌ Düzeltilecek: (1) kilitliler RENKLİ duruyor → opak siyah olacak; (2) "400 Echo mastery" = shop-hissi →
K1 re-skin; (3) arka sıra küçük-ve-yüzüyor → TEK iso düzlem + depth + tile-snap; (4) fiyatlar 120/160/200 →
**canon 80/150/200/250** + **OR achievement yolu UI'da görünsün** ("150 ◈ *veya* Act 1 ×3"); (5) etiketler
sticker gibi → zemine işlenmiş rün-stili etiket (3.1 Pro diegetic önerisi).

### HTML mockup iterasyonu (charselect_mockup.html — kullanıcı tarayıcıda onaylayacak)
1. **Dizilim:** 5+5 iki sıra, TEK iso karo düzlemi, her karakter bir karo merkezine snap, arka sıra yarım-karo
   stagger (tablo hissi kırılır), sıralar arası 1-2 boş karo. Boyut SABİT.
2. **Kilitli:** sprite şekli #000 opak dolgu + ÇOK hafif cyan/void rim (`drop-shadow 0 0 1px rgba(0,255,204,.4)`)
   — kapkara kalır ama arka plana erimez; ayak dibinde küçük kilit glifi + "150 ◈ / Act1 ×3" düşük-kontrast etiket.
3. **Seçili:** cyan ayak-halkası + soft aura SADECE; hover = daha soluk halka (selected'dan ayırt edilir).
   Statik (pozisyon animasyonu yok; glow nefesi OK ama dikey hareket YASAK).
4. **Paneller:** ince taş/metal 9-slice çerçeve, ink-on-paper (opak slab YASAK). Sol: ad+3 tag+motto(1 satır)+
   açıklama(2 satır)+kaynak bloğu+5 stat barı. Sağ: 3 aktif skill satırı + achievement-mastery teaser (K1).
   Panel yükseklikleri sabit, overflow hidden (scroll imkânsız).
5. **Işık:** panellerin arkasına cyan (merkez rift) + turuncu (brazier) radial-gradient — bedava ambiyans
   (3.5 Flash). Brazier'lar statik; alev titremesi OK, pozisyon hareketi YOK.
6. **Alt:** TEK merkezi state-driven buton — **SEÇ / KİLİDİ AÇ (150 ◈) / YETERSİZ ◈** (kilitli+para yetmiyorsa
   pasif) + sol-alt küçük GERİ. Web-button değil taş-plaka stili (Unity'de panel_frame_9slice reuse).
7. **Üst bar:** sol "RIMA — KARAKTER SEÇ", sağ "◈ 80". Başka hiçbir şey.
8. **Ada altı:** saf karanlık void'e fade; altta İKİNCİ obje/dekor/resim YOK.
SKIP (pahalı eye-candy): particle alev sheet'leri, ada altı shader/moloz, unlock'ta ayrı modal (inline confirm yeter).

### Unity tarafı — cx bulguları: MEVCUT KODDA DİREKTİF İHLALLERİ (mockup onayından sonra fix)
- `CharacterSelectScreen.cs:996-1002` seçili scale **1.12× → KALDIR** (boyut sabit direktifi).
- `:1075-1085` seçili karakter **dikey bob → KALDIR**.
- `:1086-1120` dönen halka/mote VFX → statik glow'a indir (hareket yasağı strict yorumda).
- `:1159-1201` sağ panel **ScrollRect → sabit kompakt satırlar** (scroll yasak).
- `RosterPlacements :75-87` 4+6 → **5+5** tile-anchor yerleşimi (mockup'taki karo merkezleri normalized
  anchor olarak taşınır — cx'in "anchor mapping" riski böyle çözülür).
- `UnlockCost :938-947` 120/180/250 → **canon 80/150/200/250** + OR-path metni; default-unlocked seti
  canon'a göre daralt (canon: sadece Warblade default; demo için mevcut 4-açık kalabilir — kullanıcıya sorulacak).
- Currency label'ları tek helper: "◈ n" formu (K2).
- Stat barları: veri kaynağı YOK → `RimaUITheme.ClassIdentity`'ye 5-stat (Hasar/Dayanıklılık/Hız/Kontrol/Zorluk)
  statik tablosu eklenir (10 sınıf, tasarım değeri NLM'den).

## Ekonomi (karar + eksik kod)
- **Run-sonu Shattered Echo kazanım kodu HİÇ YOK** (cx: RunStats currency tutmuyor, death/victory ekranı
  vermiyor). Yapılacak: run-sonu award — hedef **~20-40 ◈/run** (Act1-clear ≈15, Act2 ≈25, Act3 ≈40 +
  first-time bonus), ilk alt-class (80) ≈ 3-4. run'da açılır; tam roster = uzun-kuyruk (~25-40 run, Hades/RL2
  pacing). Satın alma/double YOK. ⚠️ Rakamlar precedent-bazlı taslak — gerçek run süresine göre tuning pass gerek.
- Karakter fiyatları = canon (80/150/200/250); 400-Echo hiçbir şey YOK.

## Uygulama sırası (hepsi bu karardan sonra, kod = cx)
1. **HTML mockup v2** (K3 listesi) → kullanıcı tarayıcıda bakar → iterasyon → ONAY.
2. Onay sonrası **cx Unity task**: ihlal fix'leri (scale/bob/scroll/VFX) + 5+5 anchor + canon fiyat + OR-path +
   ◈ tam-form + stat-bar verisi + mastery-teaser satırları.
3. **Run-sonu Echo award** (küçük ayrı cx task; Death/Victory ekranına "+n ◈" satırı).
4. NLM-sync: bu doküman.

## ✅ KULLANICI ONAYLARI (2026-06-05 — KESİNLEŞTİ)
- **K2 KESİN:** "◈ Shattered Echo" tam-form ONAYLANDI. Rename yok. UI: üst bar "◈ 80"; unlock copy
  "KİLİDİ AÇ — 150 ◈ Shattered Echo". Çıplak "Echo" gameplay mekaniklerine rezerve.
- **Mastery panel KESİN (kullanıcı custom kararı):** Sağ panel sınıfın **BÜTÜN skill'lerini** gösterir
  (tam liste). Açılma-şartlı skill'ler: **karanlık gösterilir, SADECE İSMİ yazılır + açılma şartı**
  ("Açılış: Act 1'i Warblade ile bitir" formu) — açıklama/bilgi GİZLİ. Açık/draftable skill'ler normal
  görünür (3 başlangıç aktifi kısa açıklamalı, kalanlar kompakt). ⚠️ No-scroll direktifiyle birlikte =
  satırlar ÇOK kompakt olmalı (ikon+isim; detay ESC SkillCodexUI'nin işi olacak).
- **Default açık karakter KESİN:** 4 açık kalır (Warblade/Elementalist/Ranger/Shadowblade) — demo pratikliği.
  Release'de canon'a (sadece Warblade) dönülür.
