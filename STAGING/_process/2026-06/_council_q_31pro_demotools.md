# Council — Gemini 3.1 Pro (DEEP / sunum + tool tasarımı lens)

Sen kıdemli bir oyun yapım + akademik sunum danışmanısın.

## Bağlam
RIMA = 2D top-down ARPG roguelite, bir CAPSTONE projesi. Demo = **hocaya CANLI SUNUM** (Steam/oyuncu değil). Not = SİSTEMLER üzerinden veriliyor, sanat cilası ikincil.

**Öğrenci sunumda iki şeyi göstermek istiyor:**
1. **Oynanabilirlik:** dual-class'ın oynanarak kanıtı (10-dk dikey slice: chamber→class-select→combat→reward→combat→shop→boss→dual-class+post-boss→victory).
2. **ALTYAPI (öğrencinin asıl gururu):** Unity Editor'e GİRMEDEN, oyun-içi runtime tool'larıyla (Director Mode, backtick ` ile açılır) **stat değiştirme / spawn / ayar yapmayı CANLI göstermek** → "şu altyapıyı şöyle kurdum, bakın editörsüz canlı tunelliyorum" anlatısı.

## Mevcut durum
- Director Mode overlay VAR (timeScale=0). Sekmeler: Spawn (çalışıyor, palette→ghost→tıkla→sil), Stats (stat sistemi runtime), Class&Skill, Build(Tile+Cliff+Prop — kısmen stub), Map (stub), Telemetry.
- Ayrı LiveTool (Tool.exe) manuel prop palette yapıyor ama oyun-içi değil.
- Combat+VFX+UI bitti. Run animasyonları beklemede (plan: "anim = cila, kritik yol değil AMA RUN×2 en yüksek algı sıçraması"; idle fallback kabul).

## SORULAR (klişe yok, gerçek yargı)
1. **Hangi tool'lar "altyapı derinliği"ni bir akademisyene en çok kanıtlar?** Sırala: canlı stat tuning / live spawn / class-switch / manuel prop yerleştirme / map nav / telemetry/CSV. Hangisi "vay bu ciddi mühendislik" dedirtir, hangisi "gösterişli ama yüzeysel" kalır?
2. **İşlevsel olarak tool nasıl olmalı + nasıl BULUNMALI/kullanılmalı?** Bunu kör bir kullanıcı değil, SUNUCU (öğrenci) sürecek. Yani: okunabilirlik (label/renk-kod), CANLI sunumda crash-yememe, "demo-script'lenebilir" akış. Bir runtime tool'u akademik gözde "etkileyici" vs "gimmick" yapan ne?
3. **Manuel prop yerleştirme (rift crystal'ı elle koymak) gerçekten değerli mi**, yoksa canlı STAT tuning asıl altyapı flex'i mi? Sunum zamanı sınırlıysa hangisi feda edilir?
4. **Oynanabilirlik tarafı:** RUN animasyonları (×2 sınıf) görsel cila olarak YETERLİ mi, yoksa "bitmemiş" görünmemek için minimum ne lazım?
5. **Efor dağılımı:** sınırlı zamanda (a) tool derinliği, (b) oynanabilirlik/dual-class, (c) animasyon — nasıl bölünmeli? Net öncelik sırası ver.

Türkçe, numaralı, net. Akademik-sunum gerçekliğiyle konuş.
