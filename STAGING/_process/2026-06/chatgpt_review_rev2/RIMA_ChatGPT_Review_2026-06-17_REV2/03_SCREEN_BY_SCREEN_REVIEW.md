# 3 — Screen-by-Screen Review

Öncelik: **P0 demo blocker**, **P1 yüksek etki**, **P2 polish**, **POST demo sonrası**.

## 01 — Main Menu

**İyi:** Backdrop RIMA dünyasını satıyor; kompozisyon menü için uygun.  
**Sorun:** Logo küçük ve jenerik; seçili menü durumu yalnızca ince cyan çizgiye bırakılmış; “Yine geldin.” metni gereksiz küçük.  
**Değişiklik:** Logo alanını yaklaşık %20 büyüt. Seçili satıra amber başlık + cyan sol işaret + hafif koyu scrim ver. Menü grubunu 40–48 px satır yüksekliğine çıkar.  
**Öncelik:** P2, 1–2 saat.

## 02 — Opening Draft

**İyi:** Üç kart modeli anlaşılır, oyun durumu doğru pause oluyor.  
**Sorun:** Kart ikonları aynı sanat dilinde değil; açıklama metni küçük; `IRON CHARGE ile eşleşir` bilgi değil, muamma. Skill'in yeni mi, upgrade mi olduğu ve hangi slota gideceği görünmüyor.  
**Değişiklik:** Her karta `NEW SKILL / UPGRADE / PASSIVE` rozeti; net synergy cümlesi; slot sonucu; hover/keyboard focus state ekle.  
**Örnek synergy:** `Iron Charge sonrasında kullanılırsa çekilen hedefler 1.5 sn sersemler.`  
**Öncelik:** P1, 2–3 saat.

## 03 / 05 / 13 — Combat + HUD

**Sorun:** HP ve resource 1080p'de okunmuyor. Alt slotlar küçük; ikon ve key label birbirine giriyor. Üst siyah bant görüntüyü kesiyor. Bazı kapı/obje ve büyük düşmanlar siyah boşluk gibi. Oda geniş, karakter/düşman yoğunluğu düşük.  
**Değişiklik:** HP 200–220×14–16 px, resource 150–170×8–10 px. LMB/RMB 52–56 px, Q/E/R/F 44–48 px. Kapılara/karanlık düşmana 1–2 px rim light. Siyah üst bandı kaldır.  
**Öncelik:** P0, 4–6 saat.

## 04 / 15 — Death Screen

**İyi:** Akış çalışıyor, Retry ve Main Menu var.  
**Sorun:** Ekran sonuç raporu gibi değil; metin ortada dağınık, arka plan yeterince ayrışmıyor, butonlar dev cyan blok. Başlık yok.  
**Değişiklik:** `RUN FRACTURED` veya Türkçe lokalizasyon ana başlığı; üç sütun/iki grup sonuç; kazanılan Echo ayrı highlight; birincil buton amber/cyan outline, ikincil buton düşük vurgu.  
**Öncelik:** P1, 2–3 saat.

## 07 / 08 / 09 — Director Mode

**Sorun:** En kritik ekran. Bütün viewport'u çerçeve içine hapsediyor. Her buton ayrı süslü kutu; sonuçta hiçbir şey önemli görünmüyor. Font küçük, sağ inspector neredeyse boş, alt komut barı okunmuyor. `09` yanlış capture.  
**Değişiklik:** `04_DIRECTOR_MODE_REDESIGN.md` ve görsel wireframe uygulanmalı.  
**Öncelik:** P0, 6–10 saatlik demo skin pass. Tam refactor POST.

## 10 / 11 / 12 — Build Mode

**Düzeltilmiş yorum:** Eğik diamond grid ve gridin mevcut oda şeklinin dışına devam etmesi bilinçli ve doğrudur. RIMA'nın 128×64 isometric tile'larını çizmek, odanın floor şeklini büyütmek ve gerçek world-space hücrelere yerleşmek için bu çalışma düzlemi gerekir. Siyah editör alanı da oyun ekranı değil authoring workspace'tir.

**İyi:** Asset seçimi, tile placement, erase, undo/redo ve working-copy isolation kanıtlanmış durumda. Mevcut sol asset alanı + orta viewport + sağ tool panel yaklaşımı işlevsel.

**Gerçek sorun:** Grid bazı bölgelerde sanat ve küçük proplardan daha yüksek kontrasta çıkıyor. Hover edilen hücre, footprint, aktif layer ve geçerli/geçersiz yerleştirme yeterince belirgin değil. Prop yerleştirildikten sonra seçili objeyi kanıtlayan geri bildirim zayıf. Üstteki mode seçimi ile asset kategori satırı kısmen tekrar ediyor.

**Değişiklik:** Layout'u ve isometric grid geometrisini koru. Grid'i room bounds içine kırpma. Normal/low/high grid görünürlük seçeneği ekle; mevcut floor ile genişletilebilir alanı ton farkıyla ayır; hover cell, footprint, valid/invalid reason ve selected-object pulse ekle. Aktif layer/tool bilgisini cursor yakınında ve status bar'da göster.

**Öncelik:** P1 UX polish, 2–4 saat. Yapısal redesign veya Director shell'e zorunlu taşıma POST ve ancak kullanım testi gerekçelendirirse.

## 14 — Low HP

**Sorun:** Full-screen kırmızı wash oynanışı ve renk bilgisini öldürüyor. Uzun sürerse yorucu.  
**Değişiklik:** Yalnız kenarlarda %12–18 kırmızı vignette; HP <%20'de 0.8–1.0 sn pulse; merkez tamamen temiz.  
**Öncelik:** P0, 30–60 dakika.

## 16 — Pause

**Sorun:** Panel gereksiz küçük; ekran merkezinde siyah debug menü gibi; seçili satır zayıf.  
**Değişiklik:** 420–480 px panel, 48 px satırlar, title/selection hierarchy, `Resume` birincil. Arka scrim mevcut kalabilir.  
**Öncelik:** P1, 1–2 saat.

## 17 — Settings

**İyi:** Bilgi grupları mantıklı ve çalışır durumda.  
**Sorun:** Panel aşırı dar ve uzun; font küçük; toggle'lar buton gibi; keybind satırları çok sıkışık.  
**Değişiklik:** 720–820 px geniş iki sütun: sol category nav, sağ content. Demo süresinde tam refactor yerine paneli 1.25× büyüt, satır yüksekliğini artır, toggle ON/OFF state'ini ikonla ayır.  
**Öncelik:** P2, 2–3 saat.

## 18 — Codex

**İyi:** Paketteki en iyi bilgi mimarisi. Class listesi ve skill satırları anlaşılır.  
**Sorun:** Büyük siyah boşluk; metin 1080p'de küçük; skill satırları tek bakışta taranamıyor; rarity yalnız sağ uçta; class tabları düğme değil etiket gibi.  
**Değişiklik:** Sol 220 px class rail, sağ skill list + detail pane. Demo quick-win: satır yüksekliği 56–64 px, ikon 40–44 px, rarity renk şeridi solda, seçili class daha net.  
**Öncelik:** P2, 2–4 saat.

## 19 — Character Sheet

**Bulgu:** Panel screenshot'ta görünmüyor.  
**Karar:** Tasarım yorumu yapılamaz; tekrar capture zorunlu.  
**Öncelik:** P0 capture fix.

## 20 — Skill Offer Draft

**Bulgu:** Panel screenshot'ta görünmüyor.  
**Karar:** `02` draft state'i referans alınabilir, fakat `20` kanıt değildir.  
**Öncelik:** P0 capture fix.

## 21 — Run Map

**Bulgu:** `20` ile aynı görüntü; map açık değil.  
**Karar:** Claude'un “M-bleed düzeldi” veya node değerlendirmesi bu paketle doğrulanamaz. Yeniden capture edilmeli.  
**Öncelik:** P0 capture fix.

## 22 — Merchant

**Sorun:** Ürünler renkli kareler, metinler zeminde yüzüyor; gerçek stand veya item kimliği yok. Portallar merchant işlevinden daha baskın.  
**Değişiklik:** Her ürün için 72–96 px item pedestal/card, ikon, isim, fiyat, kısa etki. Yaklaşınca tek bir context panel açılmalı; tüm metinler aynı anda havada durmamalı. Demo quick-win olarak kareleri gerçek skill/item ikonlarıyla değiştir ve yakınlık bazlı highlight ekle.  
**Öncelik:** P1, 2–4 saat.

## 23 / 24 — Boss

**Sorun:** “Güçlü” değil, kritik prototype problemi. Boss alt kenardan taşıyor, HUD'u kapatıyor ve zemine oturmuyor. Neon yeşil bar RIMA paletine aykırı. Merchant objeleri boss odasında kalmış. Subtitle boss'un gövdesine biniyor. Faz bilgisi yok.  
**Değişiklik:** Boss sprite scale/PPU/anchor düzelt; tamamen arena içinde görünür tut. Barı stone-dark frame + crimson/amber fill yap; %66/%33 phase notch. Intro sırasında 1 sn kontrollü camera focus; subtitle skill barın üstünde güvenli alanda. Boss state başlarken shop standlarını kaldır.  
**Öncelik:** **P0**, 3–5 saat.

## 25 + extra Scene View

**İyi:** Anchor/scale hatasını teşhis etmek için faydalı.  
**Sorun:** Runtime kalite kanıtı değildir. Kamera dışında siyah/scene grid alanı çok büyük.  
**Değişiklik:** Boss prefab root, sprite pivot, PPU ve SortingGroup incelenmeli; runtime capture ile doğrulanmalı.  
**Öncelik:** P0 boss fix kapsamında.
